using System;
using System.Collections.Generic;

namespace TibaltsAnalysis
{
    class Game
    {
        private int turns = 1;
        private Deck deck;
        private Hand hand;
        private int lands = 0;
        private bool goFirst;

        public Game(int[] decklist, Random rand)
        {
            this.deck = new Deck(decklist, rand);
            this.hand = new Hand(this.deck);

            if (rand.Next(1, 3) == 1) goFirst = true;
            else goFirst = false;
        }

        public void Play()
        {
            hand.DrawAndMulligan();
            lands += hand.PlayFirstTurn(goFirst);
            while (hand.state == Hand.TibState.Incomplete)
            {
                turns++;
                lands += hand.PlayTurn(turns, lands);
            }
            ReportState();
        }

        internal void ReportState()
        {
            Hand.TibState state = this.hand.state;
            Console.WriteLine("Game over at turn " + this.turns);
            if (goFirst) Console.WriteLine("Player went first");
            else Console.WriteLine("Player went second");
            Console.WriteLine("Tibalt cast state: " + state);
            Console.WriteLine();

        }
    }
}
