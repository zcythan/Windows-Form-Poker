namespace Project_3
{
    public partial class Form1 : Form
    {
        private Poker game;
        private Label[] gameCards;
        private Label[] playCards;
        private Button[] playButtons;
        private bool fold;
        public Form1()
        {
            InitializeComponent();
            game = new Poker();
            gameCards = new Label[] { gameCard1, gameCard2, gameCard3, gameCard4, gameCard5 };
            playButtons = new Button[] { checkButton, betButton, callButton, foldButton };
            playCards = new Label[] { p1card1, p1card2, p2card1, p2card2 };
            fold = false;
        }

        private string getSuit(int suit, Label card)
        {
            if(suit == 1) //diamond
            {
                card.ForeColor = Color.Red;
                return "\u2666";
            }
            else if (suit == 2) //heart
            {
                card.ForeColor = Color.Red;
                return "\u2665";
            }
            else if (suit == 3) //spade
            {
                card.ForeColor = Color.Black;
                return "\u2660";
            }
            else if (suit == 4) //club
            {
                card.ForeColor = Color.Black;
                return "\u2663";
            }
            return "Unknown input";
        }

        private string getValue(int value)
        {
            string[] highCard = new string[] { "J", "Q", "K", "A" };
            if(value < 11)
            {
                return value.ToString();
            }
            
            for(int i = 0; i < highCard.Length; i++)
            {
                if(value == i + 11)
                {
                    return highCard[i];
                }
            }
            return "Error";
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            drawButton.Visible = false;
            drawButton.Enabled = false;
            fold = false;
            p1result.Text = string.Empty;
            p2result.Text = string.Empty;
            winner.Text = string.Empty;
            payout.Text = $"Payout: ${game.payout}";
            p1Funds.Text = $"Funds: ${game.players[0].Funds}";
            p2Funds.Text = $"Funds: ${game.players[1].Funds}";
            //updating player's card UI
            int z = 0;
            foreach (Player p in game.players)
            {
                foreach (Card c in p.hand.cards)
                {
                    playCards[z].Text = getValue(c.value) + '\n' + getSuit(c.suit, playCards[z]);
                    z++;
                }

            }
        
            p1turn.Visible = game.turn;
            p2turn.Visible = !game.turn;
            foldButton.Enabled = true;
            for (int i = 0; i < gameCards.Length; i++)
            {
                gameCards[i].Visible = false;
                gameCards[i].Text = getValue(game.cards.hand[i].value) + '\n' + getSuit(game.cards.hand[i].suit, gameCards[i]);
            }
            for(int i = 0; i < playButtons.Length; i++)
            {
                playButtons[i].Enabled = game.allowedMoves[i];
            }
        }

        private string getOutcomeText(int handStat)
        {
            string[] outcomes = { "High Card", "Pair", "Two Pair", "Three of a Kind", "Straight", 
                "Flush", "Full House", "Four of a Kind", "Straight Flush", "Royal Flush!" };
            for(int i = 0; i < outcomes.Length; i++)
            {
                if(handStat == i + 1)
                {
                    return outcomes[i];
                }
            }
            return string.Empty;
        }


        private void roundEnd()
        {
            if (!fold)
            {
                p1result.Text = getOutcomeText(game.players[0].hand.handStatus);
                p2result.Text = getOutcomeText(game.players[1].hand.handStatus);
            }
                payout.Text =$"Payout: ${game.payout.ToString()}";
            drawButton.Visible = true; 
            if (game.noMoney)
            {
                resetMoney.Enabled = true;
                resetMoney.Visible = true;
            }
            else
            {
                drawButton.Enabled = true;
            }
            if (game.winner == 1)
            {
                winner.Text = "Player 1 wins!";
                p1Funds.Text += $" + ${game.payout}";
            }
            else if(game.winner == 2)
            {
                winner.Text = "Player 2 wins!";
                p2Funds.Text += $" + ${game.payout}";
            }
            else if (game.winner == 3)
            {
                winner.Text = "Tie!";
                p1Funds.Text += $" + ${game.payout/2}";
                p2Funds.Text += $" + ${game.payout/2}";
            }
            else
            {
                winner.Text = "Error";
            }
            game.reset();
            for (int i = 0; i < playButtons.Length; i++)
            {
                playButtons[i].Enabled = false;
            }
        }

        private void checkUpdates()
        {
            if(game.round > 1)
            {
                for(int i = 0; i < game.numCard; i++)
                {
                    gameCards[i].Visible = true;
                }
            }
            if(game.winner != 0)
            {
                roundEnd();
            }
            else
            {
                p1turn.Visible = game.turn;
                p2turn.Visible = !game.turn;
                p1Funds.Text = $"Funds: ${game.players[0].Funds}";
                p2Funds.Text = $"Funds: ${game.players[1].Funds}";
                payout.Text = $"Payout: ${game.payout}";
                for (int i = 0; i < playButtons.Length; i++)
                {
                    playButtons[i].Enabled = game.allowedMoves[i];
                }
            }
        }


        private void foldButton_Click(object sender, EventArgs e)
        {
            game.fold();
            checkUpdates();
        }

        private void betButton_Click(object sender, EventArgs e)
        {
            game.bet();
            checkUpdates();
        }

        private void callButton_Click(object sender, EventArgs e)
        {
            game.call();
            checkUpdates();
        }

        private void checkButton_Click(object sender, EventArgs e)
        {
            game.check();
            checkUpdates();
        }

        private void resetMoney_Click(object sender, EventArgs e)
        {
            game.resetFunds();
            p1Funds.Text = $"Funds: ${game.players[0].Funds}";
            p2Funds.Text = $"Funds: ${game.players[1].Funds}";
            payout.Text = $"Payout: ${game.payout}";
            drawButton.Enabled = true;
            resetMoney.Visible = false;
            resetMoney.Enabled = false;
        }
    }
}