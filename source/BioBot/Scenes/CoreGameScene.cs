using System.IO;
using System.Linq;
using Puffin.Core;
using Puffin.Core.Ecs;
using Puffin.Core.Tiles;

namespace DeenGames.BioBot.Scenes
{
    public class CoreGameScene : Scene
    {
        public CoreGameScene()
        {
            var groundMap = new TileMap(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH, Path.Combine("Content", "Images", "Tileset.png"), Constants.TILE_WIDTH, Constants.TILE_HEIGHT);

            groundMap.Define("Wall", 0, 0, true);

            foreach (var y in Enumerable.Range(0, Constants.MAP_TILES_HIGH))
            {
                groundMap.Set(0, y, "Wall");
                groundMap.Set(Constants.MAP_TILES_WIDE - 1, y, "Wall");
            }
            
            foreach (var x in Enumerable.Range(0, Constants.MAP_TILES_WIDE))
            {
                groundMap.Set(x, 0, "Wall");
                groundMap.Set(x, Constants.MAP_TILES_HIGH - 1, "Wall");
            }

            this.Add(groundMap);

            var entitiesMap = new TileMap(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH, Path.Combine("Content", "Images", "Characters.png"), Constants.TILE_WIDTH, Constants.TILE_HEIGHT);
            entitiesMap.Define("Player", 0, 0);
            entitiesMap.Set(Constants.DISPLAY_TILES_WIDE / 2, Constants.DISPLAY_TILES_HIGH / 2, "Player");
            this.Add(entitiesMap);
        }
    }
}