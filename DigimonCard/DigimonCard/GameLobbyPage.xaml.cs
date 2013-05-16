using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace DigimonCard
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class GameLobbyPage : DigimonCard.Common.LayoutAwarePage
    {
        public RoomCard[] roomCard = new RoomCard[100];
        public int roomCard_buffer;
        public bool isFirst_Click = false;
        public static Uri baseUri = new Uri("ms-appx:///");
        public int currentPageNum = 1;

        public GameLobbyPage()
        {
            this.InitializeComponent();

            for (int i = 0; i < 100; i++)
            {
                roomCard[i] = new RoomCard(i + 1);
                roomCard[i].PointerPressed += roomCard_pressed;
            }
            for (int i = 0; i < 25; i++)
                for (int j = 0; j < 4; j++)
                {
                    Canvas.SetLeft(roomCard[i * 4 + j], j * 270 + 50);
                    Canvas.SetTop(roomCard[i * 4 + j], 50);
                    roomArea.Children.Add(roomCard[i * 4 + j]);
                }
            for (int i = 4; i < 100; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            this.pageBox.SelectedIndex = currentPageNum - 1;
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

        private void roomCard_pressed(object sender, PointerRoutedEventArgs e)
        {
             for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                 if (roomCard[i] == (RoomCard)sender)
                 {
                     if (isFirst_Click != false)
                         roomCard[roomCard_buffer].ClickBox.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                     else
                         isFirst_Click = true;
                     roomCard_buffer = i;
                 }
        }

        private void NewRoomBtn_pressed(object sender, PointerRoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Images/newRoomBtn_click.png");
            newRoomBtn.Source = bitmapImage;
        }

        private void NewRoomBtn_released(object sender, PointerRoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Images/newRoomBtn.png");
            newRoomBtn.Source = bitmapImage;

            //新建房间测试内容，现在还没有用
            //Canvas.SetLeft(roomCard[0], 50);
            //Canvas.SetTop(roomCard[0], 50);
            //roomArea.Children.Add(roomCard[0]);
        }

        private void QuickJoinBtn_pressed(object sender, PointerRoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Images/QuickJoinBtn_click.png");
            QuickJoinBtn.Source = bitmapImage;
        }

        private void QuickJoinBtn_released(object sender, PointerRoutedEventArgs e)
        {

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("ms-appx:///Images/QuickJoinBtn.png");
            QuickJoinBtn.Source = bitmapImage;

            this.Frame.Navigate(typeof(GamePage));
        }

        private void lastPageBtn_click(object sender, RoutedEventArgs e)
        {
            if (currentPageNum != 1)
            {
                for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                currentPageNum--;

                for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            this.pageBox.SelectedIndex = currentPageNum - 1;
        }

        private void nextPageBtn_click(object sender, RoutedEventArgs e)
        {
            if (currentPageNum != 25)
            {
                for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                currentPageNum++;

                for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;

                this.pageBox.SelectedIndex = currentPageNum - 1;
            }
        }

        private void PageChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            currentPageNum = pageBox.SelectedIndex + 1;

            for (int i = (currentPageNum - 1) * 4; i < (currentPageNum - 1) * 4 + 4; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;

            this.pageBox.SelectedIndex = currentPageNum - 1;
        }


    }
}
