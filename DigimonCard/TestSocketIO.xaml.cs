using Newtonsoft.Json.Linq;
using SocketIO4Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using SocketIOClient;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DigimonCard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestSocketIO : Page
    {
        private Client socketIO;

        public TestSocketIO()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        void socketIO_Message(object sender, MessageEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Message.Event))
            {
                Debug.WriteLine("--> SOCKET_IO_MESSAGE:: {0}", e.Message.MessageText);
            }
            else
            {
                Debug.WriteLine("--> SOCKET_IO_MESSAGE:: {0} : {1}", e.Message.Event, e.Message.Json.ToJsonString());
                //screenTbk.Text += e.Message.Json.ToJsonString();
            }

        }

        private void connectBt_Click(object sender, RoutedEventArgs e)
        {
            //socketIO = new Client(serverUrl.Text);
            socketIO = new Client("http://168.63.151.29:3000");
            socketIO.Message += socketIO_Message;
            socketIO.SocketConnectionClosed += socketIO_SocketConnectionClosed;
            socketIO.Error += socketIO_Error;

            socketIO.On("connect", (message) =>
            {
                Debug.WriteLine("on connect called!!!");
                JObject jo = new JObject();
                jo["publisher"] = "username";
                jo["password"] = "password";
                socketIO.Emit("hConnect", jo);
            });

            socketIO.On("news", (message) =>
            {
                Debug.WriteLine("start listening");
                this.listenTbx.Text+=("\n"+message.Json.ToJsonString());
            });

            socketIO.ConnectAsync();

        }

        

        void socketIO_Error(object sender, ErrorEventArgs e)
        {
            WebErrorStatus status = WebSocketError.GetStatus(e.Exception.HResult);
            Debug.WriteLine("-->SOCKET_IO_ERROR::" + status);
        }

        void socketIO_SocketConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine(">>>socketIO_SocketConnectionClosed::Closed!");
        }

        private void disConnectBt_Click(object sender, RoutedEventArgs e)
        {
            if (socketIO != null)
            {
                socketIO.Close();
            }
        }

        private void sendBt_Click(object sender, RoutedEventArgs e)
        {
            if (socketIO != null && socketIO.IsConnected)
            {

                Debug.WriteLine("on send connect called!!!");
                //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                socketIO.Emit("hConnect", JObject.Parse("{\"a\":"+this.sendTbx.Text+"}"));
            
            }

        }

    }
}
