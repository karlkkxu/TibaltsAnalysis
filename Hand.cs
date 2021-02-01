using System;
using System.Collections.Generic;
using System.Linq;

namespace TibaltsAnalysis
{
    internal class Hand
    {
        private List<Card> cards = new List<Card>();
        private Deck deck;
        private int[] sums;
        internal int mulligans = 7;
        
        internal enum TibState
        {
            Incomplete, //The combo has not been cast
            Success,    //The combo was cast and hit a bomb card
            Failed      //The combo was cast but hit a wrong card (trigger or Tibalt's)
        }

        internal TibState state = 0;

        public Hand(Deck deck)
        {
            this.deck = deck;
        }

        internal void DrawAndMulligan()
        {
            for (int timesMulliganed = 0; timesMulliganed < 7; timesMulliganed++)
            {
                deck.Shuffle();
                Draw(7);
                if (HandIsGood() == true)
                {
                    ChooseCards(timesMulliganed);
                    this.mulligans = timesMulliganed;
                    break;
                }
                else
                {
                    while (this.cards.Any()) PutToBottom(this.cards.First());
                }
            }
        }

        private void Draw(int amount)
        {
            this.cards.AddRange(this.deck.draw(amount));
        }

        internal int PlayFirstTurn(bool goFirst)
        {
            if (goFirst == false)
                Draw(1);
            CheckSums();
            if (this.sums[1] > 0)
            {
                //There is a scryland, scry and return as full land since it'll be untapped anyway turn 2
                deck.Scry();
                Discard(Card.CardType.ScryLand);
                return 1;
            }
            if (this.sums[0] > 0)
            {
                //There is a normal land but no scrylands
                Discard(Card.CardType.Land);
                return 1;
            }

            //No lands. Sad.
            return 0;
        }

        internal int PlayTurn(int turns, int lands)
        {
            Draw(1);
            CheckSums();
            if (this.sums[0] > 0)
            {
                int newLands = lands + 1;
                Discard(Card.CardType.Land);
                CheckComboState(newLands);
                return 1;
            }
            if (this.sums[1] > 0)
            {
                deck.Scry();
                Discard(Card.CardType.ScryLand);
                CheckComboState(lands);
                return 1;
            }
            CheckComboState(lands);
            return 0;
        }

        private void CheckComboState(int lands)
        {
            CheckSums();
            if (lands >= 2)
            {
                foreach (Card trigger in cards)
                {
                    if (trigger.GetRole() == Card.CardType.Trigger)
                    {
                        foreach (Card Tibalt in cards)
                        {
                            if (Tibalt.GetRole() == Card.CardType.Tibalt)
                            {
                                this.state = this.deck.CastTrickery(trigger);
                            }
                        }
                    }
                }
            }
        }

        private void Discard(Card.CardType role)
        {
            foreach (Card c in this.cards)
            {
                if (c.GetRole() == role)
                {
                    this.cards.Remove(c);
                    return;
                }
            }
        }

        private bool HandIsGood()
        {
            foreach (Card c in this.cards)
            {
                if (c.GetRole() == Card.CardType.Tibalt) return true;
            }
            return false;
        }

        private void CheckSums()
        {
            int[] sums = new int[5];
            foreach (Card card in this.cards)
            {
                sums[(int)card.GetRole()]++;
            }
            this.sums = sums;
        }

        private void ChooseCards(int amount)
        {
            for (int discarded = 0; discarded < amount;)
            {
                CheckSums();
                //Discard all bombs first if needed
                while (sums[3] > 0 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.Bomb) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //Discard all regular lands as long as there are at least two lands total in hand
                while ((sums[0] + sums[1]) > 2 && sums[0] > 0 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.Land) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //Discard all scrylands as long as there are at least two lands total in hand
                while ((sums[0] + sums[1]) > 2 && sums[1] > 0 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.ScryLand) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //Discard duplicates of triggers
                while (sums[2] > 1 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.Trigger) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //At this point, no bombs, no regular lands, only two scrylands, and only one trigger card
                //Discard duplicates of Tibalt's
                while (sums[4] > 1 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.Tibalt) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //You'd never hope to get to this situation
                //Discard all lands
                while (sums[1] > 0 && discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() == Card.CardType.ScryLand) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }

                //Discard everything but the last Tibalt's
                while (discarded < amount)
                {
                    foreach (Card c in this.cards)
                    {
                        if (c.GetRole() != Card.CardType.Tibalt) { PutToBottom(c); discarded++; break; }
                    }
                    CheckSums();
                }
            }
            
        }

        private void PutToBottom(Card card)
        {
            this.deck.Return(card);
            this.cards.Remove(card);
        }

        internal void Print()
        {
            foreach (Card c in this.cards)
            {
                Console.Write(c.GetRole() + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }


    }
}