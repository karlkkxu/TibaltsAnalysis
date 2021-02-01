using System;
using System.Collections.Generic;

namespace TibaltsAnalysis
{
    class Game
    {
        internal int turns = 1;
        internal Deck deck;
        internal Hand hand;
        internal int lands = 0;
        internal bool goFirst;
        internal int mulligans;

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
            this.mulligans = this.hand.mulligans;
            lands += hand.PlayFirstTurn(goFirst);
            while (hand.state == Hand.TibState.Incomplete)
            {
                turns++;
                lands += hand.PlayTurn(turns, lands);
            }
            //PrintState();
            this.deck = null;
            this.hand = null;
        }

        internal void PrintState()
        {
            Hand.TibState state = this.hand.state;
            Console.WriteLine("Game over at turn " + this.turns);
            if (goFirst) Console.WriteLine("Player went first");
            else Console.WriteLine("Player went second");
            Console.WriteLine("Amount of mulligans was " + this.mulligans);
            Console.WriteLine("Tibalt cast state: " + state);
            Console.WriteLine();

        }
    }
}
