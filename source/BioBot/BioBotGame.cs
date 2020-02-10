using DeenGames.BioBot.Scenes;
using Puffin.Infrastructure.MonoGame;

namespace DeenGames.BioBot
{
    public class BioBotGame : PuffinGame
    {
        public BioBotGame() : base(960, 540)
        {
        }

        override protected void Ready()
        {
            this.ShowScene(new CoreGameScene());
        }
    }
}