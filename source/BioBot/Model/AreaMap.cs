using System.Collections.Generic;
using System.Linq;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace DeenGames.BioBot.Model
{
    // Your typical core-game map. It's a "floor" number of a specific biome, probably.
    public class AreaMap
    {
        // TODO: refactor into player
        internal int PlayerX { get; private set; } = 0;
        internal int PlayerY { get; private set; } = 0;
        internal List<Monster> Monsters = new List<Monster>();
        
        private const int currentDifficutly = 1000;
        private readonly ArrayMap<bool> isWalkable;
        private readonly int areaNumber = 1; // "floor" number
        private readonly IGenerator globalRandom;
        
        // In TILES
        private readonly int width = 0;
        private readonly int height = 0;
        

        public AreaMap(IGenerator globalRandom)
        {
            this.globalRandom = globalRandom;
            this.width = Constants.MAP_TILES_WIDE;
            this.height = Constants.MAP_TILES_HIGH;
            this.isWalkable = new ArrayMap<bool>(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH);

            // Each method gets its own RNG, so hopefully things are more segregated (less cascading changes)
            this.GenerateMap(new StandardGenerator(globalRandom.Next()));
            // TODO: more sophisticated.
            PlayerX = this.width / 4;
            PlayerY =  this.height / 4;
            this.GenerateMonsters(new StandardGenerator(globalRandom.Next()));
        }

        public bool this[int x, int y]
        {
            get {
                return this.isWalkable[x, y];
            }
        }

        public void TryToMovePlayer(int deltaX, int deltaY)
        {
            var destinationX = PlayerX + deltaX;
            var destinationY = PlayerY + deltaY;

            if (destinationX >= 0 && destinationX < this.width && destinationY >= 0 && destinationY < this.height &&
                this.isWalkable[destinationX, destinationY] == true)
            {
                this.PlayerX = destinationX;
                this.PlayerY = destinationY;
            }
        }

        private void GenerateMap(IGenerator random)
        {
            QuickGenerators.GenerateRectangleMap(isWalkable);
            
            // Generate the exit. It's one side of the map where you can walk off.
            
            // TODO: don't collide with the entrance; we probably don't want to be on the same side,
            // and we definitely don't want to be too close (in terms of walking distance) from the
            // entrance to the exit. It should be a suitably long walk.

            // Pick a random side
            var sides = new string[] { "up", "down", "left", "right" };
            var side = sides[random.Next(sides.Length)];
            var exitSize = 3;

            if (side == "up" || side == "down")
            {
                var startX = exitSize + random.Next(this.width - (2 * exitSize));
                var y = side == "up" ? 0 : this.height - 1;
                for (var x = startX; x < startX + exitSize; x++)
                {
                    this.isWalkable[x, y] = true;
                }
            }
            else
            {
                var startY = exitSize + random.Next(this.height - (2 * exitSize));
                var x = side == "left" ? 0 : this.width - 1;
                for (var y = startY; y < startY + exitSize; y++)
                {
                    this.isWalkable[x, y] = true;
                }
            }
        }

        public void GenerateMonsters(IGenerator random)
        {
            // TODO: more sophisticated.
            var numMonsters = random.Next(6, 10);
            while (numMonsters-- > 0)
            {
                (var x, var y) = (random.Next(this.width), random.Next(this.height));
                
                while (!isWalkable[x, y] || Monsters.Any(m => m.X == x && m.Y == y) || (x == this.PlayerX && y == this.PlayerY))
                {
                    (x, y) = (random.Next(this.width), random.Next(this.height));
                }

                var monster = new Monster("Slime", x, y);
                this.Monsters.Add(monster);
            }
        }
    }
}