using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigimonCard
{
    class AttractCards
    {
        
        //卡牌名称
        private String name;
        //卡牌编号
        private int cardId;
        //攻击力
        private int ap;
        //防御力
        private int dp;
        //进化经验值
        private int exp;

        public AttractCards( String name, int cardId, int ap, int dp, int exp)
        {
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
