using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigimonCard.cards
{
    class AC
    {
        public static AttractCards[] attractCards;
        public static void Init()
        {
            attractCards[1] = new AttractCards("亚古兽",1,100,80,30);
        }
        
    }
}
