namespace DeenGames.BioBot.Model
{
    public class Monster
    {
        public int X { get; set; }
        public int Y { get; set; }
        public readonly string Name;

        public Monster(string name, int x, int y)
        {
            this.Name = name;
            X = x;
            Y = y;
        }
    }
}