using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigimonCard
{
    class AttractCard
    {

        //卡牌名称
        private String name;
        //
        private int cardId;
        //攻击力
        private int ap;
        //防御力
        private int dp;
        //进化经验值
        private int exp;
        //种族 兽 人 植物 变异
        private String race;

        public String getName()
        {
            return name;
        }

        public int getCardId()
        {
            return cardId;
        }

        public int getAp()
        {
            return ap;
        }

        public int getDp()
        {
            return dp;
        }

        public int getExp()
        {
            return exp;
        }

        public String getRace()
        {
            return race;
        }
    }
}
