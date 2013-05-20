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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace DigimonCard
{
    public partial class Cards : UserControl
    {
        public BitmapImage frontImage = new BitmapImage();
        public BitmapImage backImage = new BitmapImage();
        public BitmapImage imageBuffer = new BitmapImage();

        public Cards()
        {
            this.InitializeComponent();

            backImage.UriSource = new Uri("ms-appx:///Images/cardBack.png");
            frontImage.UriSource = new Uri("ms-appx:///Images/card.png");

            Storyboard_turnTo90.Completed += Storyboard_turnTo90_Completed;
        }

        private void Storyboard_turnTo90_Completed(object sender, object e)
        {
            image.Source = imageBuffer;
            Storyboard_turnTo180.Begin();
        }

        public void turn2back()
        {
            Storyboard_turnTo90.Begin();
            imageBuffer = backImage; 
        }

        public void turn2front()
        {
            Storyboard_turnTo90.Begin();
            imageBuffer = frontImage;
        }
    }
}
