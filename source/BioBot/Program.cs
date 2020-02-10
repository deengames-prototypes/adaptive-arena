using System;

namespace DeenGames.BioBot
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new BioBotGame())
            {
                game.Run();
            }
        }
    }
}
