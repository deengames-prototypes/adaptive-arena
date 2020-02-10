using DeenGames.BioBot.Scenes;
using Puffin.Infrastructure.MonoGame;

namespace DeenGames.BioBot
{
    public class BioBotGame : PuffinGame
    {
        public BioBotGame() : base(Constants.DISPLAY_TILES_WIDE * Constants.TILE_WIDTH, Constants.DISPLAY_TILES_HIGH * Constants.TILE_HEIGHT)
        {
        }

        override protected void Ready()
        {
            this.ShowScene(new CoreGameScene());
        }
    }
}