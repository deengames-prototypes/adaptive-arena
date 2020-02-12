using System;
using System.Collections.Generic;
using System.IO;
using DeenGames.BioBot.Ecs.Entities;
using DeenGames.BioBot.Ecs.Systems;
using DeenGames.BioBot.Events;
using DeenGames.BioBot.Model;
using DeenGames.BioBot.UI;
using Puffin.Core;
using Puffin.Core.IO;
using Puffin.Core.Tiles;
using Troschuetz.Random.Generators;

namespace DeenGames.BioBot.Scenes
{
    public class CoreGameScene : Scene
    {
        private readonly AreaMap map;
        private readonly TileMap entitiesMap;
        private List<AbstractSystem> systems = new List<AbstractSystem>();

        public CoreGameScene()
        {
            this.BackgroundColour = Palette.DarkestBrown;

            // Probably shouldn't go here ...
            var gameSeed = new Random().Next();
            Console.WriteLine($"Global seed is {gameSeed}");

            this.systems = new List<AbstractSystem>()
            {
                new DamageSystem(),
            };

            // Testing, testing, 1 2 3 ...
            this.map = new AreaMap(new StandardGenerator(gameSeed), systems);

            // Create ground tilemap
            var groundMap = new TileMap(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH, Path.Combine("Content", "Images", "Tileset.png"), Constants.TILE_WIDTH, Constants.TILE_HEIGHT);

            groundMap.Define("Wall", 0, 0, true);

            for (var y = 0; y < Constants.MAP_TILES_HIGH; y++)
            {
                for (var x = 0; x < Constants.MAP_TILES_WIDE; x++)
                {
                    if (map[x, y] == false)
                    {
                        groundMap.Set(x, y, "Wall");
                    }
                }
            }
            this.Add(groundMap);

            // Create entities tilemap
            this.entitiesMap = new TileMap(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH, Path.Combine("Content", "Images", "Characters.png"), Constants.TILE_WIDTH, Constants.TILE_HEIGHT);
            this.entitiesMap.Define("Player", 0, 0);
            this.entitiesMap.Define("Slime", 0, 1);
            this.Add(this.entitiesMap);

            this.OnActionPressed = (data) =>
            {
                var action = (PuffinAction)data;

                if (action == PuffinAction.Up)
                {
                    map.TryToMovePlayer(0, -1);
                }
                else if (action == PuffinAction.Down)
                {
                    map.TryToMovePlayer(0, 1);
                }
                else if (action == PuffinAction.Left)
                {
                    map.TryToMovePlayer(-1, 0);
                }
                else if (action == PuffinAction.Right)
                {
                    map.TryToMovePlayer(1, 0);
                }
            };
        }

        override public void Update(int elapsedMilliseconds)
        {
            // Apply all systems. This is probably overkill. Things happen on-move only.
            foreach (var system in this.systems)
            {
                system.OnUpdate();
            }

            // clear/redraw all entities, instead of tracking their old locations.
            this.entitiesMap.Clear();
            this.entitiesMap.Set(map.Player.X, map.Player.Y, "Player");
            foreach (var monster in this.map.Monsters)
            {
                this.entitiesMap.Set(monster.X, monster.Y, monster.Name);
            }
        }
    }
}