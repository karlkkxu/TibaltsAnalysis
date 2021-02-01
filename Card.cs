using System;

namespace TibaltsAnalysis
{
    internal class Card
    {
        internal enum CardType
        {
            Land,
            ScryLand,
            Trigger,
            Bomb,
            Tibalt
        }

        private CardType role;

        public Card(int type)
        {
            this.role = (CardType)type;
        }

        internal CardType GetRole()
        {
            //Console.WriteLine(this.role);
            return this.role;
        }
    }
}