using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SocketIOClient.Eventing;
using SocketIOClient.Messages;
using System.Net.Http;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Web;
using SocketIO4Net.Helpers;
using Windows.System.Threading;

namespace SocketIOClient
{
    /// <summary>
    /// Class to emulate socket.io javascript client capabilities for .net classes
    /// </summary>
    /// <exception cref = "ArgumentException">Connection for wss or https urls</exception>  
    public class Client : IDisposable, SocketIOClient.IClient
    {
        private ThreadPoolTimer socketHeartBeatTimer; // HeartBeat timer 
        private Task dequeuOutBoundMsgTask;
        private BlockingCollection<string> outboundQueue;
        private readonly static object padLock = new object(); // allow one connection attempt at a time

        /// <summary>
        /// Uri of Websocket server
        /// </summary>
        protected Uri uri;
        /// <summary>
        /// Underlying WebSocket implementation
        /// </summary>
        protected MessageWebSocket mws;


        private WebSocketState mwsState = WebSocketState.None;
        /// <summary>
        /// RegistrationManager for dynamic events
        /// </summary>
        protected RegistrationManager registrationManager;  // allow registration of dynamic events (event names) for client actions

        // Events
        /// <summary>
        /// Opened event comes from the underlying websocket client connection being opened.  This is not the same as socket.io returning the 'connect' event
        /// </summary>
        public event EventHandler Opened;
        public event EventHandler<MessageEventArgs> Message;
        public event EventHandler HeartBeatTimerEvent;
        /// <summary>
        /// <para>The underlying websocket connection has closed (unexpectedly)</para>
        /// <para>The Socket.IO service may have closed the connection due to a heartbeat timeout, or the connection was just broken</para>
        /// <para>Call the client.Connect() method to re-establish the connection</para>
        /// </summary>
        public event EventHandler SocketConnectionClosed;
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// ResetEvent for Outbound MessageQueue Empty Event - all pending messages have been sent
        /// </summary>
        public ManualResetEvent MessageQueueEmptyEvent = new ManualResetEvent(true);

        /// <summary>
        /// Connection Open Event
        /// </summary>
        public ManualResetEvent ConnectionOpenEvent = new ManualResetEvent(false);



        
        private string LastWebErrorMessage;

        /// <summary>
        /// Represents the initial handshake parameters received from the socket.io service (SID, HeartbeatTimeout etc)
        /// </summary>
        public SocketIOHandshake HandShake { get; private set; }

        /// <summary>
        /// Returns boolean of ReadyState == WebSocketState.Open
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return this.ReadyState == WebSocketState.Connected;
            }
        }

        /// <summary>
        /// Connection state of websocket client: None, Connecting, Open, Closing, Closed
        /// </summary>
        public WebSocketState ReadyState
        {
            get
            {
                return this.mwsState;
            }
        }


        public Client(string url)
        {
            this.uri = new Uri(url);
            this.registrationManager = new RegistrationManager();
            this.outboundQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
            this.dequeuOutBoundMsgTask = Task.Factory.StartNew(() => dequeuOutboundMessages(), TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// Initiate the connection with Socket.IO service
        /// </summary>
        public async void ConnectAsync()
        {
            if (!(this.ReadyState == WebSocketState.Connecting || this.ReadyState == WebSocketState.Connected))
            {
                try
                {
                    this.ConnectionOpenEvent.Reset();
                    this.HandShake = await this.requestHandshake(uri);// perform an initial HTTP request as a new, non-handshaken connection

                    if (this.HandShake == null || string.IsNullOrWhiteSpace(this.HandShake.SID) || this.HandShake.HadError)
                    {
                        Debug.WriteLine("Error initializing handshake with {0}", uri.ToString());
                    }
                    else
                    {
                        string wsScheme = (uri.Scheme == "https" ? "wss" : "ws");

                        this.mws = new MessageWebSocket();
                        mws.Control.MessageType = SocketMessageType.Utf8;
                        this.mws.MessageReceived += mws_MessageReceived;
                        this.mws.Closed += mws_Closed;
                        this.mwsState = WebSocketState.Connecting;
                        await mws.ConnectAsync(new Uri(string.Format("{0}://{1}:{2}/socket.io/1/websocket/{3}", wsScheme, uri.Host, uri.Port, this.HandShake.SID)));
                        this.mwsState = WebSocketState.Connected;
                        mws_OpenEvent();
                    }
                }
                catch (Exception ex)
                {
                    this.mwsState = WebSocketState.Closed;
                    Debug.WriteLine(string.Format("Connect threw an exception...{0}", ex.GetBaseException().HResult));
                    WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                    this.OnErrorEvent(this, new ErrorEventArgs(status.ToString(), ex));

                }
            }

        }

        void mws_Closed(IWebSocket sender, WebSocketClosedEventArgs args)
        {
            MessageWebSocket websocket = Interlocked.Exchange(ref mws, null);
            if (websocket != null)
            {
                websocket.Dispose();
            }
            //if(this.mwsState == WebSocketState.Closing)
                this.OnSocketConnectionClosedEvent(this, EventArgs.Empty);
            this.mwsState = WebSocketState.Closed;

        }

        /// <summary>
        ///  Raw websocket messages from server - convert to message types and call subscribers of events and/or callbacks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void mws_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            string read = null;
            try
            {
                using (DataReader reader = args.GetDataReader())
                {
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    read = reader.ReadString(reader.UnconsumedBufferLength);
                    Debug.WriteLine("[SOCKET_IO]: < " + read);
                }
                IMessage iMsg = SocketIOClient.Messages.Message.Factory(read);

                if (iMsg.Event == "responseMsg")
                    Debug.WriteLine(string.Format("InvokeOnEvent: {0}", iMsg.RawMessage));

                switch (iMsg.MessageType)
                {
                    case SocketIOMessageTypes.Disconnect:
                        this.OnMessageEvent(iMsg);
                        if (string.IsNullOrWhiteSpace(iMsg.Endpoint)) // Disconnect the whole socket
                            this.Close();
                        break;
                    case SocketIOMessageTypes.Heartbeat:
                        this.OnHeartBeatTimerCallback(null);
                        break;
                    case SocketIOMessageTypes.Connect:
                    case SocketIOMessageTypes.Message:
                    case SocketIOMessageTypes.JSONMessage:
                    case SocketIOMessageTypes.Event:
                    case SocketIOMessageTypes.Error:
                        this.OnMessageEvent(iMsg);
                        break;
                    case SocketIOMessageTypes.ACK:
                        this.registrationManager.InvokeCallBack(iMsg.AckId, iMsg.Json);
                        break;
                    default:
                        Debug.WriteLine("unknown mws message Received...");
                        break;
                }
            }
            catch (Exception ex) // For debugging
            {
                this.mwsState = WebSocketState.Closed;
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine("mws_MessageReceived::exception : " + status);
                this.OnErrorEvent(this, new ErrorEventArgs(status.ToString(), ex));
                //this.Close();
            }
        }




        /// <summary>
        /// <para>Asynchronously calls the action delegate on event message notification</para>
        /// <para>Mimicks the Socket.IO client 'socket.on('name',function(data){});' pattern</para>
        /// <para>Reserved socket.io event names available: connect, disconnect, open, close, error, retry, reconnect  </para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        /// <example>
        /// client.On("testme", (data) =>
        ///    {
        ///        Debug.WriteLine(data.ToJson());
        ///    });
        /// </example>
        public virtual void On(
            string eventName,
            Action<IMessage> action)
        {
            this.registrationManager.AddOnEvent(eventName, action);
        }
        public virtual void On(
            string eventName,
            string endPoint,
            Action<IMessage> action)
        {
            this.registrationManager.AddOnEvent(eventName, endPoint, action);
        }
        /// <summary>
        /// <para>Asynchronously sends payload using eventName</para>
        /// <para>payload must a string or Json Serializable</para>
        /// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
        /// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="payload">must be a string or a Json Serializable object</param>
        /// <remarks>ArgumentOutOfRangeException will be thrown on reserved event names</remarks>
        public void Emit(string eventName, dynamic payload, string endPoint = "", Action<dynamic> callback = null)
        {
            string lceventName = eventName.ToLower();
            IMessage msg = null;
            switch (lceventName)
            {
                case "message":
                    if (payload is string)
                        msg = new TextMessage() { MessageText = payload };
                    else
                        msg = new JSONMessage(payload);
                    this.Send(msg);
                    break;
                case "connect":
                case "disconnect":
                case "open":
                case "close":
                case "error":
                case "retry":
                case "reconnect":
                    throw new System.ArgumentOutOfRangeException(eventName, "Event name is reserved by socket.io, and cannot be used by clients or servers with this message type");
                default:


                    if (!string.IsNullOrWhiteSpace(endPoint) && !endPoint.StartsWith("/"))
                        endPoint = "/" + endPoint;
                    msg = new EventMessage(eventName, payload, endPoint, callback);
                    if (callback != null)
                        this.registrationManager.AddCallBack(msg);

                    this.Send(msg);
                    break;
            }
        }

        /// <summary>
        /// <para>Asynchronously sends payload using eventName</para>
        /// <para>payload must a string or Json Serializable</para>
        /// <para>Mimicks Socket.IO client 'socket.emit('name',payload);' pattern</para>
        /// <para>Do not use the reserved socket.io event names: connect, disconnect, open, close, error, retry, reconnect</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="payload">must be a string or a Json Serializable object</param>
        public void Emit(string eventName, dynamic payload)
        {
            this.Emit(eventName, payload, string.Empty, null);
        }

        /// <summary>
        /// Queue outbound message
        /// </summary>
        /// <param name="msg"></param>
        public void Send(IMessage msg)
        {
            this.MessageQueueEmptyEvent.Reset();
            if (this.outboundQueue != null)
                this.outboundQueue.Add(msg.Encoded);
        }

        private void Send(string rawEncodedMessageText)
        {
            this.MessageQueueEmptyEvent.Reset();
            if (this.outboundQueue != null)
                this.outboundQueue.Add(rawEncodedMessageText);
        }

        /// <summary>
        /// if a registerd event name is found, don't raise the more generic Message event
        /// </summary>
        /// <param name="msg"></param>
        protected void OnMessageEvent(IMessage msg)
        {

            bool skip = false;
            if (!string.IsNullOrEmpty(msg.Event))
                skip = this.registrationManager.InvokeOnEvent(msg); // 

            var handler = this.Message;
            if (handler != null && !skip)
            {
                //Debug.WriteLine(string.Format("webSocket_OnMessage: {0}", msg.RawMessage));
                handler(this, new MessageEventArgs(msg));
            }
        }

        /// <summary>
        /// Close SocketIO4Net.Client and clear all event registrations 
        /// </summary>
        public void Close()
        {
            this.mwsState = WebSocketState.Closing;
            // stop the heartbeat time
            this.closeHeartBeatTimer();

            // stop outbound messages
            this.closeOutboundQueue();

            this.closeWebSocketClient();

            if (this.registrationManager != null)
            {
                this.registrationManager.Dispose();
                this.registrationManager = null;
            }

        }

        protected void closeHeartBeatTimer()
        {
            // stop the heartbeat timer
            if (this.socketHeartBeatTimer != null)
            {
                this.socketHeartBeatTimer.Cancel();
                this.socketHeartBeatTimer = null;
            }
        }
        protected void closeOutboundQueue()
        {
            // stop outbound messages
            if (this.outboundQueue != null)
            {
                this.outboundQueue.CompleteAdding(); // stop adding any more items;
                this.dequeuOutBoundMsgTask.Wait(700); // wait for dequeue thread to stop
                this.outboundQueue.Dispose();
                this.outboundQueue = null;
            }
        }
        protected void closeWebSocketClient()
        {
            if (this.mws != null)
            {
                // unwire events
                //this.mws.Closed -= this.mws_Closed;
                //this.mws.MessageReceived -= this.mws_MessageReceived;

                if (this.mwsState != WebSocketState.Closed)
                {
                    try { this.mws.Close(1000, ""); }
                    catch { Debug.WriteLine("exception raised trying to close websocket: can safely ignore, socket is being closed"); }
                }
            }
        }

        // websocket client events - open, messages, errors, closing
        private void mws_OpenEvent()
        {
            this.socketHeartBeatTimer = ThreadPoolTimer.CreatePeriodicTimer(OnHeartBeatTimerCallback, HandShake.HeartbeatInterval);
            this.ConnectionOpenEvent.Set();

            this.OnMessageEvent(new EventMessage() { Event = "open" });
            if (this.Opened != null)
            {
                try { this.Opened(this, EventArgs.Empty); }
                catch (Exception ex) { Debug.WriteLine(ex); }
            }

        }

        protected void OnErrorEvent(object sender, ErrorEventArgs e)
        {
            if (this.Error != null)
            {
                try { this.Error.Invoke(this, e); }
                catch { }
            }
            Debug.WriteLine(string.Format("Error Event: {0}\r\n\t{1}", e.Message, e.Exception));
        }
        protected void OnSocketConnectionClosedEvent(object sender, EventArgs e)
        {
            if (this.SocketConnectionClosed != null)
            {
                try { this.SocketConnectionClosed(sender, e); }
                catch { }
            }
            Debug.WriteLine("SocketConnectionClosedEvent");
        }

        // Housekeeping
        protected void OnHeartBeatTimerCallback(object state)
        {
            if (this.ReadyState == WebSocketState.Connected)
            {
                IMessage msg = new Heartbeat();
                try
                {
                    if (this.outboundQueue != null && !this.outboundQueue.IsAddingCompleted)
                    {
                        this.outboundQueue.Add(msg.Encoded);
                        if (this.HeartBeatTimerEvent != null)
                        {
                            this.HeartBeatTimerEvent.BeginInvoke(this, EventArgs.Empty, null, null);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 
                    Debug.WriteLine(string.Format("OnHeartBeatTimerCallback Error Event: {0}\r\n\t{1}", ex.Message, ex.InnerException));
                }
            }
        }
      
        /// <summary>
        /// While connection is open, dequeue and send messages to the socket server
        /// </summary>
        protected void dequeuOutboundMessages()
        {
            while (this.outboundQueue != null && !this.outboundQueue.IsAddingCompleted)
            {
                if (this.ReadyState == WebSocketState.Connected)
                {
                    string msgString;
                    try
                    {
                        if (this.outboundQueue.TryTake(out msgString, 500))
                        {
                            //Debug.WriteLine(string.Format("webSocket_Send: {0}", msgString));

                            Write2mwsOutputStream(msgString);
                        }
                        else
                            this.MessageQueueEmptyEvent.Set();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("The outboundQueue is no longer open..." + ex.Message);
                    }
                }
                else
                {
                    this.ConnectionOpenEvent.WaitOne(2000); // wait for connection event
                }
            }
        }

        private async void Write2mwsOutputStream(string msg)
        {
            if (this.mws != null)
            {
                using (DataWriter dataWriter = new DataWriter(this.mws.OutputStream))
                {
                    dataWriter.WriteString(msg);
                    await dataWriter.StoreAsync();
                    dataWriter.DetachStream();
                    Debug.WriteLine("[SOCKET_IO]: > " + msg);
                }
            }
        }

        /// <summary>
        /// <para>Client performs an initial HTTP POST to obtain a SessionId (sid) assigned to a client, followed
        ///  by the heartbeat timeout, connection closing timeout, and the list of supported transports.</para>
        /// <para>The tansport and sid are required as part of the ws: transport connection</para>
        /// </summary>
        /// <param name="uri">http://localhost:3000</param>
        /// <returns>Handshake object with sid value</returns>
        /// <example>DownloadString: 13052140081337757257:15:25:websocket,htmlfile,xhr-polling,jsonp-polling</example>
        protected async Task<SocketIOHandshake> requestHandshake(Uri uri)
        {
            string value = string.Empty;
            string errorText = string.Empty;
            SocketIOHandshake handshake = null;

            using (HttpClient http = new HttpClient())
            {
                try
                {
                    value = await http.GetStringAsync(string.Format("{0}://{1}:{2}/socket.io/1/{3}", uri.Scheme, uri.Host, uri.Port, uri.Query)); // #5 tkiley: The uri.Query is available in socket.io's handshakeData object during authorization
                    // 13052140081337757257:15:25:websocket,htmlfile,xhr-polling,jsonp-polling
                    if (string.IsNullOrEmpty(value))
                        errorText = "Did not receive handshake string from server";
                }
                catch (Exception ex)
                {
                    errorText = string.Format("Error getting handsake from Socket.IO host instance: {0}", ex.Message);
                    this.OnErrorEvent(this, new ErrorEventArgs(ex.Message, ex));
                }
            }
            if (string.IsNullOrEmpty(errorText))
                handshake = SocketIOHandshake.LoadFromString(value);
            else
            {
                handshake = new SocketIOHandshake();
                handshake.ErrorMessage = errorText;
            }

            return handshake;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code 
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                this.Close();
                this.MessageQueueEmptyEvent.Dispose();
                this.ConnectionOpenEvent.Dispose();
            }

        }
    }


}
