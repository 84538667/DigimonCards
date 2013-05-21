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
        public RoomCard[] roomCard = new RoomCard[120];
        public Cards[] cards = new Cards[51];
        public int roomCard_buffer;
        public bool isFirst_Click = true;
        public bool isChooseCardsbegin = false;
        public static Uri baseUri = new Uri("ms-appx:///");
        public int currentPageNum = 1;

        private int currentPageTotalRoomNum = 8;
        private int totalPage = 15;

        public GameLobbyPage()
        {
            this.InitializeComponent();
            Gule.Begin();

            for (int i = 0 ; i < 17 ; i++)
                for (int j = 0 ; j < 3 ; j++)
                {
                    cards[i*3+j] = new Cards();
                    Canvas.SetLeft(cards[i * 3 + j] , i * 100 + 20);
                    Canvas.SetTop(cards[i * 3 + j], j * 150 + 50);
                    Canvas_cards.Children.Add(cards[i*3+j]);
                }
            
            
            for (int i = 0; i < currentPageTotalRoomNum * totalPage; i++)
            {
                roomCard[i] = new RoomCard(i + 1);
                roomCard[i].PointerPressed += roomCard_pressed;
            }
            for (int i = 0; i < totalPage; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Canvas.SetLeft(roomCard[i * 8 + j], j * 180 + 80);
                    Canvas.SetTop(roomCard[i * 8 + j], 0);
                    roomArea.Children.Add(roomCard[i * 8 + j]);
                }
                for (int j = 4; j < currentPageTotalRoomNum; j++)
                {
                    Canvas.SetLeft(roomCard[i * 8 + j], (j - 4) * 180 + 350);
                    Canvas.SetTop(roomCard[i * 8 + j], 180);
                    roomArea.Children.Add(roomCard[i * 8 + j]);
                }
            }
            for (int i = 8; i < 120; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            
            storyboard_appear.Completed += storyboard_artWordBegin;
            storyboard_artWord.Completed += storyboard_artWord_completed;
            storyboard_visible.Completed += storyboard_visible_Completed;
            Gule.Completed += storyboard_guleshou_Completed;

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

        private void storyboard_artWordBegin(object sender, object e)
        {
            storyboard_artWord.Begin();
        }

        private void storyboard_artWord_completed(object sender, object e)
        {
            storyboard_visible.Begin();
        }

        private void storyboard_visible_Completed(object sender, object e)
        {
            for (int i = 0; i < 51; i++)
            {
                cards[i].turn2front();
            }
        }

        private void storyboard_guleshou_Completed(object sender, object e)
        {

            gule1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gule2.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gule3.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gule4.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gule5.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            Gule.Begin();
        }

        private void roomCard_pressed(object sender, PointerRoutedEventArgs e)
        {
            for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                if (roomCard[i] == (RoomCard)sender)
                {
                    if (isFirst_Click == true)
                        isFirst_Click = false;
                    else if (i == roomCard_buffer)
                        roomCard[i].roomPng.Opacity = 0.7;
                    else
                    {
                        roomCard[i].roomPng.Opacity = 0.7;
                        roomCard[roomCard_buffer].roomPng.Opacity = 1.0;
                    }
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

        private void previousPageBtn_click(object sender, RoutedEventArgs e)
        {
            if (currentPageNum != 1)
            {
                for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                currentPageNum--;

                for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            this.pageBox.SelectedIndex = currentPageNum - 1;
            
        }

        private void nextPageBtn_click(object sender, RoutedEventArgs e)
        {
            if (currentPageNum != totalPage)
            {
                for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                currentPageNum++;

                for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                    roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;

                this.pageBox.SelectedIndex = currentPageNum - 1;
            }
        }

        private void PageChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            currentPageNum = pageBox.SelectedIndex + 1;

            for (int i = (currentPageNum - 1) * currentPageTotalRoomNum; i < (currentPageNum - 1) * currentPageTotalRoomNum + currentPageTotalRoomNum; i++)
                roomCard[i].Visibility = Windows.UI.Xaml.Visibility.Visible;

            this.pageBox.SelectedIndex = currentPageNum - 1;
           
        }

        private void cardsInHand_click(object sender, RoutedEventArgs e)
        {
            if (isChooseCardsbegin == false)
            {
                maskPanel.Visibility = Windows.UI.Xaml.Visibility.Visible;
                storyboard_appear.Begin();
                isChooseCardsbegin = true;
                chooseCardBtn.Content = "完成";
            }
            else
            {
                storyboard_disapp.Begin();
                isChooseCardsbegin = false;
                storyboard_disapp.Completed += storyboard_disapp_Completed;
            }
        }

        private void storyboard_disapp_Completed(object sender, object e)
        {
            maskPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            chooseCardBtn.Content = "手牌";
            Canvas_cards.Opacity = 0;

            BitmapImage bitmapimage = new BitmapImage();
            bitmapimage.UriSource = new Uri("ms-appx:///Images/cardBack.png");

            for (int i = 0; i < 51; i++)
                cards[i].image.Source = bitmapimage;

        }


    }
}
