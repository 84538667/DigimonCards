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
    public partial class AttractCards : Cards
    {
        private String name;
        private int cardId; //卡牌编号
        private int ap; //攻击力
        private int dp; //防御力
        private int exp; //进化经验值

        public AttractCards()
        {
            this.InitializeComponent();
            backImage.UriSource = new Uri("ms-appx:///Images/cardBack.png");
            frontImage.UriSource = new Uri("ms-appx:///Images/card.png");
        }

        public AttractCards(String name, int cardId, int ap, int dp, int exp)
        {
            this.InitializeComponent();

            this.name = name;
            this.cardId = cardId;
            this.ap = ap;
            this.dp = dp;
            this.exp = exp;
        }

        public String GetName()
        {
            return name;
        }

        public int GetCardId()
        {
            return cardId;
        }

        public int GetAp()
        {
            return ap;
        }

        public int GetDp()
        {
            return dp;
        }

        public int GetExp()
        {
            return exp;
        }

    }
}
