using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Troschuetz.Random;
using Troschuetz.Random.Generators;

namespace DeenGames.BioBot.Model
{
    // Your typical core-game map. It's a "floor" number of a specific biome, probably.
    public class AreaMap
    {
        private readonly ArrayMap<bool> isWalkable;
        private readonly int areaNumber = 1; // "floor" number
        private readonly IGenerator globalRandom;
        private readonly int tilesWide = 0;
        private readonly int tilesHigh = 0;
        internal int PlayerX { get; private set; } = 0;
        internal int PlayerY { get; private set; } = 0;

        public AreaMap(IGenerator globalRandom, int tilesWide, int tilesHigh)
        {
            this.globalRandom = globalRandom;
            this.tilesWide = tilesWide;
            this.tilesHigh = tilesHigh;
            this.isWalkable = new ArrayMap<bool>(Constants.MAP_TILES_WIDE, Constants.MAP_TILES_HIGH);

            // Each method gets its own RNG, so hopefully things are more segregated (less cascading changes)
            this.GenerateMap(new StandardGenerator(globalRandom.Next()));
            // TODO: more sophisticated
            PlayerX = this.tilesWide / 4;
            PlayerY =  this.tilesHigh / 4;
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

            if (destinationX >= 0 && destinationX < this.tilesWide && destinationY >= 0 && destinationY < this.tilesHigh &&
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
                var startX = exitSize + random.Next(this.tilesWide - (2 * exitSize));
                var y = side == "up" ? 0 : this.tilesHigh - 1;
                for (var x = startX; x < startX + exitSize; x++)
                {
                    this.isWalkable[x, y] = true;
                }
            }
            else
            {
                var startY = exitSize + random.Next(this.tilesHigh - (2 * exitSize));
                var x = side == "left" ? 0 : this.tilesWide - 1;
                for (var y = startY; y < startY + exitSize; y++)
                {
                    this.isWalkable[x, y] = true;
                }
            }
        }
    }
}