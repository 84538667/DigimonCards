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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace DigimonCard
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class GamePage : Page
    {
        public GamePage()
        {
            this.InitializeComponent();
            this.Init();
        }

        private void Init()
        {
            Player player1 = new Player("string", 1, 1, 1, 1, 1, null, null);
            Player player2 = new Player("string", 1, 1, 1, 1, 1, null, null);

            String p1Name = player1.GetName();
            String p2Name = player2.GetName();

            int p1Hp = player1.GetHp();
            int p2Hp = player2.GetHp();

            Deck p1Deck = new Deck(player1.GetCardsUsed());
            Deck p2Deck = new Deck(player2.GetCardsUsed());

            p1Deck.Init();
            p2Deck.Init();

            AttractCards[] p1HandCards = new AttractCards[5];
            AttractCards[] p2HandCards = new AttractCards[5];

            for (int i = 0; i < 5; i++)
            {
                p1HandCards[i] = p1Deck.GetFirstCard();
                p2HandCards[i] = p2Deck.GetFirstCard();
            }

        }
        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /*onclick(){
         *   应该是点击之后处理操作并，发送信息给服务器
         *   服务器收到信息之后广播
         *   然后设置一个接受的receiver 等待服务器信息
         *   服务器返回信息之后则改变状态
         *   
        }*/


    }
}
