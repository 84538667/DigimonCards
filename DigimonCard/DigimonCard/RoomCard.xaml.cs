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
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace DigimonCard
{
    public sealed partial class RoomCard : UserControl
    {
        //public int isClick = 0;
        public bool roomCard_OnClick = false;

        public RoomCard()
        {
            this.InitializeComponent();
        }

        public RoomCard(int rmNum)
        {
            this.InitializeComponent();

            roomNum.Text = rmNum.ToString();
        }

        private void JoinIn_byPhoto(object sender, PointerRoutedEventArgs e)
        {

        }

        private void BeSelect(object sender, PointerRoutedEventArgs e)
        {
                ClickBox.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }
    }
}
