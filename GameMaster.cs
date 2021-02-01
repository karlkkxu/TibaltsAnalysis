using System;
using System.Collections.Generic;
using System.Text;

namespace TibaltsAnalysis
{
    class GameMaster
    {
        private List<Game> listOfGames = new List<Game>();
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

            GameMaster gm = new GameMaster();

            for (int i = 0; i < 500; i++)
            {
                Game game = new Game(decklist, rand);
                game.Play();
                gm.listOfGames.Add(game);
            }

            Console.WriteLine("Played " + gm.listOfGames.Count + " games");

            Console.WriteLine("The combo was pulled off on turn");
            int[] turnCount = new int[7];
            foreach (Game g in gm.listOfGames)
            {
                if (g.turns < 7) turnCount[g.turns]++;
                else turnCount[6]++;
            }
            for (int i = 1; i <= turnCount.Length - 1; i++)
            {
                if (i == turnCount.Length - 1) Console.WriteLine(i + "+: " + turnCount[i]);
                else Console.WriteLine(i + ":  " + turnCount[i]);
            }

            int[] firstSplit = new int[2];
            foreach (Game g in gm.listOfGames)
            {
                if (g.goFirst) firstSplit[0]++;
                else firstSplit[1]++;
            }
            Console.WriteLine("The player went first in " + firstSplit[0] + " games");


        }
    }
}
