using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigimonCard
{

    //需要创建一个静态class
    //搞一个静态player表示自己，方便在各个class传递这个乱七八糟的参数。
    class Player
    {
        private int hp;
        private int cardsNum;
        private int totalTimes;
        private int winTimes;
        private int loseTimes;
        private String name;
        private int[] cardsOwned;
        private int[] cardsUsed;

        public Player(String name, int hp, int totalTimes, int winTimes, int loseTimes, int cardsNum,int[] cardsOwned, int[]cardsUsed)
        {
            this.name = name;
            this.hp = hp;
            this.totalTimes = totalTimes;
            this.winTimes = winTimes;
            this.loseTimes = loseTimes;
            this.cardsNum = cardsNum;
            this.cardsOwned = cardsOwned;
            this.cardsUsed = cardsUsed;
        }

        public int GetHp()
        {
            return hp;
        }

        public void SetHp( int hp )
        {
            this.hp = hp;
        }

        public String GetName()
        {
            return name;
        }

        public void SetName(String name)
        {
            this.name = name;
        }

        public int GetTotalTimes()
        {
            return totalTimes;
        }

        public void SetTotalTimes(int tt)
        {
            this.totalTimes = tt;
        }

        public int GetWinTimes()
        {
            return winTimes;
        }

        public void SetWinTimes(int wt)
        {
            this.winTimes = wt;
        }

        public int GetLoseTimes()
        {
            return loseTimes;
        }

        public void SetLoseTimes(int lt)
        {
            this.loseTimes = lt;
        }

        public int[] GetCardsOwned()
        {
            return cardsOwned;
        }

        public void SetCardsOwned(int[] co)
        {
            this.cardsOwned = co;
        }

        public int[] GetCardsUsed()
        {
            return cardsUsed;
        }

        public void SetCardsUsed(int[] cu)
        {
            this.cardsUsed = cu;
        }


    }
}
