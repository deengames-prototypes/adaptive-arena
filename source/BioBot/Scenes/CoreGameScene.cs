using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DeenGames.BioBot.Ecs.Components;
using DeenGames.BioBot.Ecs.Systems;
using DeenGames.BioBot.Events;
using DeenGames.BioBot.Model;
using DeenGames.BioBot.UI;
using Puffin.Core;
using Puffin.Core.Ecs;
using Puffin.Core.Ecs.Components;
using Puffin.Core.Events;
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
        private Puffin.Core.Ecs.Entity statusBar;
        private bool gameOver = false;

        public CoreGameScene()
        {
            this.BackgroundColour = Palette.DarkestBrown;

            // Probably shouldn't go here ...
            var gameSeed = new Random().Next();
            Console.WriteLine($"Global seed is {gameSeed}");

            this.map = new AreaMap(new StandardGenerator(gameSeed));

            this.systems = new List<AbstractSystem>()
            {
                new DamageSystem(),
                new MovementBehaviourSystem(this.map),
            };

            // Add everything to every system
            foreach (var system in systems)
            {
                system.Add(map.Player);
                foreach (var monster in map.Monsters)
                {
                    system.Add(monster);
                }
            }

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

            var playerHealth = map.Player.Get<HealthComponent>();

            this.statusBar = new Puffin.Core.Ecs.Entity().Move(0, Constants.DISPLAY_TILES_HIGH * Constants.TILE_HEIGHT)
                .Colour(Palette.Black, Constants.DISPLAY_TILES_WIDE * Constants.TILE_WIDTH, Constants.STATUS_BAR_HEIGHT)
                .Label("");

            this.Add(this.statusBar);

            this.UpdateStatusBar();

            // Event handlers for things
            this.OnActionPressed = this.ProcessPlayerInput;
            // Update status bar when something gets hurt
            EventBus.LatestInstance.Subscribe(Signal.EntityDied, (data) => this.UpdateStatusBar());
            EventBus.LatestInstance.Subscribe(Signal.EntityHurt, (data) => this.UpdateStatusBar());
            EventBus.LatestInstance.Subscribe(Signal.EntityDied, (obj) =>
            {
                if (obj == this.map.Player)
                {
                    this.statusBar.Get<TextLabelComponent>().Text = "Your biobot crumbles ...";
                    this.gameOver = true;
                }
            });
        }

        override public void Ready()
        {
            statusBar.Get<TextLabelComponent>().FontSize = 24;
        }

        private void ProcessPlayerInput(object data)
        {
            if (this.gameOver)
            {
                return;
            }

            var action = (PuffinAction)data;
            var moved = false;

            if (action == PuffinAction.Up)
            {
                map.TryToMove(map.Player, 0, -1);
                moved = true;
            }
            else if (action == PuffinAction.Down)
            {
                map.TryToMove(map.Player, 0, 1);
                moved = true;
            }
            else if (action == PuffinAction.Left)
            {
                map.TryToMove(map.Player, -1, 0);
                moved = true;
            }
            else if (action == PuffinAction.Right)
            {
                map.TryToMove(map.Player, 1, 0);
                moved = true;
            }

            if (moved)
            {
                EventBus.LatestInstance.Broadcast(Signal.PlayerMoved);
            }
            
            this.UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            var label = this.statusBar.Get<TextLabelComponent>();
            var playerHealth = map.Player.Get<HealthComponent>();
            label.Text = $"HP: {playerHealth.CurrentHealth}/{playerHealth.TotalHealth}\n";

            var adjacents = map.Monsters.Where(m => GoRogue.Distance.EUCLIDEAN.Calculate(map.Player.X, map.Player.Y, m.X, m.Y) <= 1);
            foreach (var m in adjacents)
            {
                var health = m.Get<HealthComponent>();
                label.Text += $"{m.Name.ToLower()[0]}: {health.CurrentHealth}/{health.TotalHealth} ";
            }
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