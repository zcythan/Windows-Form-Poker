using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project_3
{
    internal class Card : IComparable<Card>
    {
        //1 diamond, 2 hearts, 3 spades, 4 clubs
        public int value { get; private set; } 
        public int suit { get; private set; }
       
        public void genCard()   
        {
            Random rand = new Random();
            value = rand.Next(2, 15);
            suit = rand.Next(1, 5);
        }

        public int CompareTo(Card? other)
        {
            if (other != null)
            {
                if (value > other.value) return 1;
                else if (value < other.value) return -1;
                else return 0;
            }
            else return -1;
        }

        public Card() 
        {
            genCard();
        }
    }
}
