using Newtonsoft.Json.Linq;
using SocketIO4Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Web;
using SocketIOClient;
using Windows.UI.Core;
using Newtonsoft.Json;
using Windows.System;
using Windows.Storage;
using Windows.Storage.Pickers;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace DigimonCard
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class MediaPlayPage : DigimonCard.Common.LayoutAwarePage
    {
        public int readyPersonNum = 0;
        public bool hostIsReady = false;
        public bool challengerIsReady = false;
        private Client socketIO;
        public bool isChooseCardsbegin = false;
        private bool _isFullscreenToggle = false;
        private bool _sliderpressed = false;
        private Size _previousVideoContainerSize = new Size();
        private CoreDispatcher SampleDispatcher;
        public int win_width = (int)Window.Current.Bounds.Width;
        public int win_height = (int)Window.Current.Bounds.Height;

        public MediaPlayPage()
        {
            this.InitializeComponent();

            maskPanel.Width = win_width;
            maskPanel.Height = win_height;
            //videoContainer.Width = win_width - 500;
            //videoContainer.Height = 2 * (win_width - 500) / 3;
            //mediaElement.Width = win_width - 500;
            //mediaElement.Height = 2 * (win_width - 500) / 3;

            Canvas.SetLeft(mediaPlayPanel, win_width / 2 - 500);
            Canvas.SetLeft(mylogo, win_width - 110);
            Canvas.SetLeft(panelPopBtn, win_width - 92);
            Canvas.SetTop(chatPanel, win_height - 438);
            Canvas.SetLeft(esc_fullscreen, win_width - 75);
            roomNum.Text = Self.roomNum.ToString();
            vedioNum.Text = Self.roomNum.ToString();
            volumeSlider.Value = 100;

            SampleDispatcher = Window.Current.CoreWindow.Dispatcher; //此实例是负责处理窗口消息，事件调度给客户端。

            createConnect();
            chooseVedioUrl();

            storyboard_disapp.Completed += storyboard_disapp_Completed;

        }

        public void chooseVedioUrl()
        {
            switch (Self.roomNum)
            {
                case 1:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/001.mp4", UriKind.Absolute);
                    break;
                case 2:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/002.mp4", UriKind.Absolute);
                    break;
                case 3:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/003.mp4", UriKind.Absolute);
                    break;
                case 4:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/004.mp4", UriKind.Absolute);
                    break;
                case 5:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/005.mp4", UriKind.Absolute);
                    break;
                case 6:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/006.mp4", UriKind.Absolute);
                    break;
                case 7:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/007.mp4", UriKind.Absolute);
                    break;
                case 8:
                    mediaElement.Source = new Uri("http://service.twtstudio.com/phone/games/008.mp4", UriKind.Absolute);
                    break;
            }
        }

        public void createConnect()
        {
            //socketIO = new Client("http://168.63.151.29:3000");

            socketIO = new Client("http://test.twtstudio.com:3000/");
            socketIO.Message += socketIO_Message;
            socketIO.SocketConnectionClosed += socketIO_SocketConnectionClosed;
            socketIO.Error += socketIO_Error;

            socketIO.On("connect", (message) =>
            {
                Debug.WriteLine("on connect called!!!");
                JObject jo = new JObject();
                jo["username"] = Self.self.GetName();
                socketIO.Emit("join", jo);
            });


            socketIO.On("chat", async (message) =>
            {
                Debug.WriteLine("chat string");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    JObject o = (JObject)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                    JArray jb = (JArray)JsonConvert.DeserializeObject(o["args"].ToString());
                    JObject ob = (JObject)jb[0];
                    if (ob["roomNum"].ToString().Equals(Self.roomNum.ToString()))
                        ChangedEventHandler(ob["username"] + ":" + ob["chat"]);
                });
            });

            socketIO.On("num", async (message) =>
            {
                Debug.WriteLine("num changed");
                Debug.WriteLine(message.Json.ToJsonString());
                await SampleDispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {

                    Debug.WriteLine(message.Json.ToJsonString());
                    JObject o = (JObject)JsonConvert.DeserializeObject(message.Json.ToJsonString());
                    JArray jb = (JArray)JsonConvert.DeserializeObject(o["args"].ToString());
                    JObject ob = (JObject)jb[0];
                    if (ob["roomNum"].ToString().Equals(Self.roomNum.ToString()))
                        ChangedNumHandler(ob["counts"].ToString());
                });
            });

            socketIO.ConnectAsync();

            string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString()  + "\"}";
            socketIO.Emit("enter_room", JObject.Parse(s));
        }

        private void ChangedNumHandler(string p)
        {
            this.onlinePersonNum.Text = p;
        }
        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
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

        void socketIO_Error(object sender, ErrorEventArgs e)
        {
            WebErrorStatus status = WebSocketError.GetStatus(e.Exception.HResult);
            Debug.WriteLine("-->SOCKET_IO_ERROR::" + status);
        }

        void socketIO_SocketConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine(">>>socketIO_SocketConnectionClosed::Closed!");
        }

        private void sendBtn_click(object sender, RoutedEventArgs e)
        {
            if (socketIO != null && socketIO.IsConnected && !this.sendTbx.Text.Equals(""))
            {
                Debug.WriteLine("on send connect called!!!");
                //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\",\"chat\":\"" + this.sendTbx.Text + "\"}";
                socketIO.Emit("client_chat", JObject.Parse(s));

            }
        }

        private void ChangedEventHandler(string s)
        {
            this.listenTbx.Text += s + "\n";
            this.sendTbx.Text = "";
        }

        private void pop_fold_Btn_Click(object sender, RoutedEventArgs e)
        {
            if ((string)pop_fold_Btn.Content == "弹出聊天框")
            {
                storyboard_chatappe.Begin();
                pop_fold_Btn.Content = "收起聊天框";
            }
            else
            {
                storyboard_chatdisa.Begin();
                pop_fold_Btn.Content = "弹出聊天框";
            }
        }

        private void keyboard_Click(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {

                if (socketIO != null && socketIO.IsConnected && !this.sendTbx.Text.Equals(""))
                {
                    Debug.WriteLine("on send connect called!!!");
                    //socketIO.Emit("hConnect", JObject.Parse(sendTbx.Text));
                    string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\",\"chat\":\"" + this.sendTbx.Text + "\"}";
                    socketIO.Emit("client_chat", JObject.Parse(s));

                }
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            string s = "{ \"username\":\"" + Self.self.GetName() + "\",\"roomNum\":\"" + Self.roomNum.ToString() + "\"}";
            socketIO.Emit("exit_room", JObject.Parse(s));
            if (socketIO != null)
            {
                JObject jo = new JObject();
                jo["username"] = "username";
                socketIO.Emit("client_close", jo);

                socketIO.Close();
            }
            Frame.Navigate(typeof(GameLobbyPage));
        }

        private void cardsInHand_click(object sender, RoutedEventArgs e)
        {
            if (isChooseCardsbegin == false)
            {
                maskPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                storyboard_appear.Begin();
                isChooseCardsbegin = true;
                panelPopBtn.Content = "完成";
                storyboard_pan.Begin();
            }
            else
            {
                storyboard_disapp.Begin();
                isChooseCardsbegin = false;
                mediaElement.Stop();
                storyboard_disapp.Completed += storyboard_disapp_Completed;
            }
        }

        private void storyboard_disapp_Completed(object sender, object e)
        {
            maskPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            panelPopBtn.Content = "播放";

            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.UriSource = new Uri("ms-appx:///Images/cardBack.png");

        }

        public bool IsFullscreen
        {
            get { return _isFullscreenToggle; }
            set { _isFullscreenToggle = value; }
        }

        private void FullscreenToggle()
        {
            this.IsFullscreen = !this.IsFullscreen;

            if (this.IsFullscreen)
            {    
                TransportControlsPanel.Visibility = Visibility.Collapsed;

                _previousVideoContainerSize.Width = videoContainer.ActualWidth;
                _previousVideoContainerSize.Height = videoContainer.ActualHeight;

                videoContainer.Width = win_width ;
                videoContainer.Height = win_height;
                mediaElement.Width = win_width;
                mediaElement.Height = win_height;

                esc_fullscreen.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                TransportControlsPanel.Visibility = Visibility.Visible;

                videoContainer.Width = _previousVideoContainerSize.Width;
                videoContainer.Height = _previousVideoContainerSize.Height;
                mediaElement.Width = _previousVideoContainerSize.Width;
                mediaElement.Height = _previousVideoContainerSize.Height;
            }
        }

        private void btnFullScreenToggle_Click(object sender, RoutedEventArgs e)
        {
            FullscreenToggle();
        }

        private void esc_fullscreen_click(object sender, RoutedEventArgs e)
        {
            FullscreenToggle();
            esc_fullscreen.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (mediaElement.DefaultPlaybackRate != 1)
            {
                mediaElement.DefaultPlaybackRate = 1.0;
            }

            mediaElement.Play();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        private void btnMute_Click(object sender, PointerRoutedEventArgs e)
        {
            mediaElement.IsMuted = !mediaElement.IsMuted;

            if (mediaElement.IsMuted)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri("ms-appx:///Images/mute.png");
                btnMute.Source = bitmapImage;
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri("ms-appx:///Images/sound_large.png");
                btnMute.Source = bitmapImage;
            }
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            timelineSlider.ValueChanged += timelineSlider_ValueChanged;

            PointerEventHandler pointerpressedhandler = new PointerEventHandler(slider_PointerEntered);
            timelineSlider.AddHandler(Control.PointerPressedEvent, pointerpressedhandler, true);

            PointerEventHandler pointerreleasedhandler = new PointerEventHandler(slider_PointerCaptureLost);
            timelineSlider.AddHandler(Control.PointerCaptureLostEvent, pointerreleasedhandler, true);
        }

        void videoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            double absvalue = (int)Math.Round(
                mediaElement.NaturalDuration.TimeSpan.TotalSeconds,
                MidpointRounding.AwayFromZero);

            timelineSlider.Maximum = absvalue;

            timelineSlider.StepFrequency =
                SliderFrequency(mediaElement.NaturalDuration.TimeSpan);

            SetupTimer();
        }

        void slider_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _sliderpressed = true;
        }

        void slider_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(timelineSlider.Value);
            _sliderpressed = false;
        }

        void timelineSlider_ValueChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (!_sliderpressed)
            {
                mediaElement.Position = TimeSpan.FromSeconds(e.NewValue);
            }
        }

        void videoMediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                if (_sliderpressed)
                {
                    _timer.Stop();
                }
                else
                {
                    _timer.Start();
                }
            }

            if (mediaElement.CurrentState == MediaElementState.Paused)
            {
                _timer.Stop();
            }

            if (mediaElement.CurrentState == MediaElementState.Stopped)
            {
                _timer.Stop();
                timelineSlider.Value = 0;
            }
        }

        void videoMediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            StopTimer();
            timelineSlider.Value = 0.0;
        }

        private void videoMediaElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            // get HRESULT from event args 
            string hr = GetHresultFromErrorMessage(e);

            // Handle media failed event appropriately 
        }

        private string GetHresultFromErrorMessage(ExceptionRoutedEventArgs e)
        {
            String hr = String.Empty;
            String token = "HRESULT - ";
            const int hrLength = 10;     // eg "0xFFFFFFFF"

            int tokenPos = e.ErrorMessage.IndexOf(token, StringComparison.Ordinal);
            if (tokenPos != -1)
            {
                hr = e.ErrorMessage.Substring(tokenPos + token.Length, hrLength);
            }

            return hr;
        }

        private DispatcherTimer _timer;

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(timelineSlider.StepFrequency);
            StartTimer();
        }

        private void _timer_Tick(object sender, object e)
        {
            if (!_sliderpressed)
            {
                timelineSlider.Value = mediaElement.Position.TotalSeconds;
            }
        }

        private void StartTimer()
        {
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void StopTimer()
        {
            _timer.Stop();
            _timer.Tick -= _timer_Tick;
        }

        private double SliderFrequency(TimeSpan timevalue)
        {
            double stepfrequency = -1;

            double absvalue = (int)Math.Round(
                timevalue.TotalSeconds, MidpointRounding.AwayFromZero);

            stepfrequency = (int)(Math.Round(absvalue / 100));

            if (timevalue.TotalMinutes >= 10 && timevalue.TotalMinutes < 30)
            {
                stepfrequency = 10;
            }
            else if (timevalue.TotalMinutes >= 30 && timevalue.TotalMinutes < 60)
            {
                stepfrequency = 30;
            }
            else if (timevalue.TotalHours >= 1)
            {
                stepfrequency = 60;
            }

            if (stepfrequency == 0) stepfrequency += 1;

            if (stepfrequency == 1)
            {
                stepfrequency = absvalue / 100;
            }

            return stepfrequency;
        }

        private async void findDocuments(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;

            picker.FileTypeFilter.Add(".wmv");
            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mp3");
            picker.FileTypeFilter.Add(".rmvb");
            picker.FileTypeFilter.Add(".avi");
            picker.FileTypeFilter.Add(".wma");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                // 指定需要让 MediaElement 播放的媒体流
                mediaElement.SetSource(stream, file.ContentType);
            }
        }

        private void volumeChanged_draged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double d = (double)(((double)volumeSlider.Value) / 100);
            mediaElement.Volume = d;
            volumeText.Text = (double.Parse(d.ToString("F2")) * 100) + "";
        }

    }
}

