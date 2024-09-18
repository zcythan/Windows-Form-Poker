using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_3
{
    internal class Poker
    {
        public Player[] players { get; private set; }
        public PokerHand cards { get; private set; }
        public int numCard {get; private set; }
        public int winner { get; private set; }
        public int payout { get; private set; }
        public int round { get; private set; }
        public bool noMoney { get; private set; }
        private bool calls;
        private int curCall;
        private bool checks;
      
        public bool turn { get; private set; } //True for player 1, false for player 2.
        public bool[] allowedMoves; //check, bet, call, fold

        public Poker()
        {
            /*//debugging, force a hand
            Card[] cards1 = { new Card(), new Card(), new Card(), new Card(), new Card() };
            int i = 14;
            foreach (Card c in cards1)
            {
                c.value = i;
                c.suit = 4;
                i--;
            } //use copy constructor below */
            cards = new PokerHand();
            numCard = 0;
            players = new Player[] { new Player(), new Player() };
            turn = true;
            allowedMoves = new bool[] { false, true, false, true };
            winner = 0;
            round = 1;
            curCall = 0;
            calls = false;
            checks = false;
            payout = 0;
            noMoney = false;
        }

        private void endHand()
        {
            if(winner == 1)
            {
                if(players[0].Funds == 0)
                {
                    noMoney = false;
                }
                players[0].Funds += payout;
            }
            else if (winner == 2){
                if(players[1].Funds == 0)
                {
                    noMoney = false;
                }
                players[1].Funds += payout;
            }
            else if (winner == 3)
            {
                if (players[0].Funds == 0 || players[1].Funds == 0)
                {
                    noMoney = false;
                }
                players[0].Funds += payout/2;
                players[1].Funds += payout/2;
            }

            for(int i = 0; i <allowedMoves.Length; i++)
            {
                allowedMoves[i] = false;
            }
        }

        private void updateVals()
        {
            
            if(round == 4 && (calls || checks))
            {
                foreach (Player p in players)
                {
                    p.hand.genHands(cards);
                }
                if (players[0].hand.CompareTo(players[1].hand) == 1)
                {
                    winner = 1;
                }
                else if (players[0].hand.CompareTo(players[1].hand) == -1)
                {
                    winner = 2;
                }
                else
                {
                    winner = 3;
                }
                endHand();
            }
            else if (calls || checks)
            {
                round++;
                foreach (Player p in players)
                {
                    if (p.Funds <= 0)
                    {
                        noMoney = true;
                    }
                }
                if (noMoney)
                {
                    allowedMoves = new bool[] { true, false, false, true };
                }
                if (checks)
                {
                    turn = !turn;
                }

                if(numCard == 0)
                {
                    numCard = 3;
                }
                else if (numCard < 5)
                {
                    numCard++;
                }
            } 
            else
            {
                turn = !turn;
            }
            checks = false;
            calls = false;
        }

        public void fold()
        {
            if (turn)
            {
                winner = 2;
            }
            else
            {
                winner = 1;
            }
            endHand();
        }

        public void bet()
        {
            checks = false;
            if (turn) 
            {
                 players[0].Funds -= 1;
            }
            else
            {
                players[1].Funds -= 1;
            }
            payout++;
            curCall++;
            allowedMoves = new bool[] { false, false, true ,true}; //call and fold 
            updateVals();
        }

        public void call()
        {
            checks = false;
            if (turn) 
            {
                players[0].Funds -= 1;
            }
            else
            {
                players[1].Funds -= 1;
            }
            payout++;
            curCall++;
            allowedMoves = new bool[] { true, true, false, true }; //check, bet and fold 
            calls = true;
            updateVals();
        }

        public void check()
        {
            checks = true;
            allowedMoves = new bool[] { true, true, false, true }; //check, bet and fold 
            updateVals();
        }

        public void resetFunds()
        {
            foreach (Player p in players)
            {
                p.Funds = 100;
            }
            payout = 0;
            noMoney = false;
        }

        public void reset()
        {
            cards.reDraw();
            foreach(Player player in players)
            {
                player.hand.reDraw();
            }
            numCard = 0;
            turn = true;
            allowedMoves = new bool[] { false, true, false, true };
            winner = 0;
            round = 1;
            curCall = 0;
            calls = false;
            checks = false;
            payout = 0;
            noMoney = false;
        }
    }

}
