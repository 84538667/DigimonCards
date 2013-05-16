using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigimonCard
{
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

        public String GetName()
        {
            return name;
        }

        public int GetTotalTimes()
        {
            return totalTimes;
        }

        public int GetWinTimes()
        {
            return winTimes;
        }

        public int GetLoseTimes()
        {
            return loseTimes;
        }

        public int[] GetCardsOwned()
        {
            return cardsOwned;
        }

        public int[] GetCardsUsed()
        {
            return cardsUsed;
        }


    }
}
