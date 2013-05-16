using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigimonCard.cards;

namespace DigimonCard
{
    class Deck
    {
        int[] cards = new int[30];
        AttractCards[] aCards = new AttractCards[30];
        int numLeft;

        public Deck( int[] cards )
        {
            this.cards = cards;
            numLeft = 30;
        }

        public void SetDeck(int[] cards)
        {
            this.cards = cards;
        }

        public void Init()
        {
            int[] c = new int[50];
            for (int i = 0; i < 50; i++)
            {
                c[i] = 0;
            }

            Random myRandom = new Random();
            for (int i = 0; i < 30; i++)
            {
                int x = myRandom.Next(30);
                if (c[x] == 0)
                {
                    c[x] = this.cards[i];
                }
                else
                {
                    i--;
                }
            }

            AC.Init();

            for (int i = 0; i < 30; i++)
            {
                aCards[i] = AC.attractCards[c[i]];
            }

        }

        public AttractCards[] GetDeckCards()
        {
            return this.aCards;
        }

        public AttractCards GetFirstCard()
        {
            AttractCards a = this.aCards[30-numLeft];
            numLeft = numLeft - 1;
            return a;
        }
    }
}
