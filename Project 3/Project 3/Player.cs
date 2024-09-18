using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3
{
    internal class Player
    {
        private int funds;
        public HoldemHand hand { get; private set; }

        public int Funds
        {
            get { return funds; }
            set
            {
                if (value < 0)
                {
                    funds = 0;
                }
                else
                {
                    funds = value;
                }
            }
        }

        public Player()
        {
            hand = new HoldemHand();
            funds = 100;
        }
    }
}
