using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Project_3
{
    internal class PokerHand : IComparable<PokerHand>
    {
        public Card[] hand { get; private set; }
        public int handStatus { get; private set; }
        public int highVal { get; private set; }
        public int rank { get; private set; }
        private int[] tempRanks; //pairs, straights, flushes
        public void reDraw()
        {
            foreach (Card card in hand)
            {
                card.genCard();
            }

        }
        private int greatToLeast(Card a, Card b)
        {
            return b.CompareTo(a);
        }

        private int genOfAKind()
        {
            int[] numCard = Enumerable.Repeat(0, 13).ToArray(); //2-9, j,q,k,a


            for (int i = 0; i < numCard.Length; i++) //counts how many of each card there are. 
            {
                for (int j = 0; j < hand.Length; j++)
                {
                    if (hand[j].value == i + 2)
                    {
                        numCard[i]++;
                    }
                }
            }
            for (int i = 0; i < numCard.Length; i++)
            {
                if (numCard[i] >= 2)
                {
                    tempRanks[0] += numCard[i] * (i + 2);
                }
            }
            bool[] ofAKind = new bool[] { false, false, false, false }; //two, three, four of a kind.
            for (int i = 0; i < numCard.Length; i++)//for every card total calculation
            {
                if (numCard[i] == 2)
                {
                    if (ofAKind[0])
                    {
                        ofAKind[3] = true; //we have 2 pair.
                    }
                    else
                    {
                        ofAKind[0] = true;
                    }
                }
                else
                {
                    for (int j = 3; j < 5; j++)
                    {
                        if (numCard[i] >= j)
                        {
                            ofAKind[j - 2] = true;
                        }
                    }
                }
            }

            if (ofAKind[2]) // four of a kind
            {
                return 5;
            }
            else if (ofAKind[0] && ofAKind[1]) //full house
            {
                return 4;
            }
            else if (ofAKind[1]) //three of a kind
            {
                return 3;
            }
            else if (ofAKind[3]) //two pairs.
            {
                return 2;
            }
            else if (ofAKind[0]) //one pair.
            {
                return 1;
            }
            else //no pairs.
            {
                return 0;
            }
        }

        private bool genStraight()
        {
            for (int i = 0; i < hand.Length; i++)
            {
                if (i == hand.Length - 1)
                {
                    break;
                }
                if (hand[i].value == 14 && hand[i+1].value == 5)
                {
                    continue;
                } 
                if (hand[i].value - 1 == hand[i + 1].value)
                {
                    continue;
                }
                return false;
            }
            foreach (Card c in hand)
            {
                tempRanks[1] += c.value;
            }
            return true;
        }

        private bool genFlush()
        {
            for (int i = 0; i < hand.Length; i++)
            {
                if (i == hand.Length- 1)
                {
                    break;
                }
                if (hand[i].suit == hand[i + 1].suit)
                {
                    continue;
                }
                return false;
            }
            return true;
        }


        private int calcValue() 
        {
            Array.Sort(hand, greatToLeast);
            highVal = hand[0].value;
            int kinds = genOfAKind();
            bool straight = genStraight();
            bool flush = genFlush();
            if (straight && flush && tempRanks[1] == 60) //royal flush
            {
                rank = 50;
                return 10;
            }
            else if (straight && flush)
            {
                rank = tempRanks[1];
                return 9;
            }
            else if (kinds == 5) 
            {
                rank = tempRanks[0];
                return 8;
            }
            else if (kinds == 4)
            {
                rank = tempRanks[0];
                return 7;
            }
            else if (flush)
            {
                rank = highVal;
                return 6;
            }
            else if (straight)
            {
                rank = tempRanks[1];
                return 5;
            }
            else if (kinds == 3)
            {
                rank = tempRanks[0];
                return 4;
            }
            else if (kinds == 2)
            {
                rank = tempRanks[0];
                return 3;
            }
            else if (kinds == 1)
            {
                rank = tempRanks[0];
                return 2;
            }
            else
            {
                rank = highVal;
                return 1;
            }

            //1 is high card, 9 is straight flush.

        }

        //Overloaded copy constructor 
        public PokerHand(Card[] cards)
        {
            hand = new Card[cards.Length];
            Array.Copy(cards, hand, cards.Length);
            rank = 0;
            tempRanks = new int[] { 0, 0};
            handStatus = calcValue();
        }

        public PokerHand()
        {
            hand = new Card[] { new Card(), new Card(), new Card(), new Card(), new Card() };
            rank = 0;
            tempRanks = new int[] { 0, 0, 0};
            handStatus = calcValue();
        }

        public int CompareTo(PokerHand? other)
        {
            if (other != null)
            {
                if (handStatus > other.handStatus) return 1;
                else if (handStatus < other.handStatus) return -1;
                else
                {
                    if (rank > other.rank) return 1; 
                    else if (rank < other.rank) return -1; 
                    else return 0;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
