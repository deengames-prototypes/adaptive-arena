using DeenGames.BioBot.Scenes;
using Puffin.Infrastructure.MonoGame;

namespace DeenGames.BioBot
{
    public class BioBotGame : PuffinGame
    {
        public BioBotGame() :
            base(Constants.DISPLAY_TILES_WIDE * Constants.TILE_WIDTH * Constants.GAME_ZOOM,
                (Constants.DISPLAY_TILES_HIGH * Constants.TILE_HEIGHT * Constants.GAME_ZOOM) + Constants.STATUS_BAR_HEIGHT)
        {
        }

        override protected void Ready()
        {
            this.ShowScene(new CoreGameScene());
        }
    }
}