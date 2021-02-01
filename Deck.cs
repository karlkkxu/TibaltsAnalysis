using System;
using System.Collections.Generic;

namespace TibaltsAnalysis
{
    internal class Deck
    {
        private List<Card> cards = new List<Card>();
        private Random rand;

        public Deck(int[] deckList, Random rand)
        {
            this.rand = rand;

            int notPlayset = deckList[2] % 4;
            int numberOfPlaysets = (deckList[2] - notPlayset) / 4;
            List<int> cardIDMachine = new List<int>();
            for (int i = 0; i < numberOfPlaysets; i++)
            {
                cardIDMachine.Add(i + 1);
                cardIDMachine.Add(i + 1);
                cardIDMachine.Add(i + 1);
                cardIDMachine.Add(i + 1);
            }
            int lastID = cardIDMachine[cardIDMachine.Count - 1];
            for (int i = 0; i < notPlayset; i++)
            {
                cardIDMachine.Add(lastID + 1);
            }

            for (int i = 0; i < deckList.Length; i++)
            {
                for (int j = 0; j < deckList[i]; j++)
                {
                    if (i == 2)
                    {
                        this.cards.Add(new Card(i, cardIDMachine[0]));
                        cardIDMachine.RemoveAt(0);
                    }
                    else
                        this.cards.Add(new Card(i));
                }
            }
        }

        internal List<Card> draw(int amount)
        {
            List<Card> drawn = new List<Card>();
            for (int i = 0; i < amount; i++)
            {
                drawn.Add(this.cards[0]);
                this.cards.RemoveAt(0);
            }
            return drawn;
        }

        public void PrintCards()
        {
            int[] sums = new int[5];
            foreach (Card card in this.cards)
            {
                sums[(int) card.GetRole()] ++;
            }

            int i = 0;
            int sum = 0;
            foreach (Card.CardType e in Enum.GetValues(typeof(Card.CardType))) {
                Console.WriteLine(e + ": (" + sums[i] + ")");
                sum += sums[i];
                i++;
            }
            Console.WriteLine("Total sum: " + sum);
        }

        public void PrintOrder()
        {
            foreach (Card c in this.cards)
            {
                Console.Write(c.GetRole() + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        internal void Return(Card card)
        {
            this.cards.Add(card);
        }

        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        internal void Scry()
        {
            Card c = this.cards[0];
            if (c.GetRole() != Card.CardType.Trigger)
            {
                this.cards.RemoveAt(0);
                this.cards.Add(c);
            }
        }

        internal Hand.TibState CastTrickery(Card trigger)
        {
            for (int i = 0; i < rand.Next(1, 4); i++)
            {
                this.cards.RemoveAt(0);
            }

            foreach (Card c in this.cards)
            {
                if (c.GetRole() != Card.CardType.Land && c.GetRole() != Card.CardType.ScryLand)
                {
                    if (c.GetID() == trigger.GetID() || c.GetRole() == Card.CardType.Tibalt) return Hand.TibState.Failed;
                    if (c.GetRole() == Card.CardType.Bomb) return Hand.TibState.Success;
                }
            }
            return Hand.TibState.Incomplete;
        }
    }
}