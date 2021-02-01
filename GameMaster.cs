using System;
using System.Collections.Generic;
using System.Text;

namespace TibaltsAnalysis
{
    class GameMaster
    {
        static void Main()
        {
            int[] decklist = new int[]
                    {
                    14,
                    12,
                    8,
                    22,
                    4
                    };
            Random rand = new Random();
            
            for (int i = 0; i < 20; i++)
            {
                Game game = new Game(decklist, rand);
                game.Play();
            }

        }
    }
}
