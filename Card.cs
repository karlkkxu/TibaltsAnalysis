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
        private int ID = 0; //Used to tell trigger cards from one another. Only triggers have ID != 0

        public Card(int type)
        {
            this.role = (CardType)type;
        }

        public Card(int type, int id)
        {
            this.role = (CardType)type;
            this.ID = id;
        }

        internal CardType GetRole()
        {
            //Console.WriteLine(this.role);
            return this.role;
        }

        internal int GetID()
        {
            return this.ID;
        }
    }
}