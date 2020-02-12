namespace DeenGames.BioBot.Ecs.Components
{
    public class FightComponent
    {
        public int Strength { get; set; }
        public int Toughness { get; set; }
        
        public FightComponent(int strength, int toughness)
        {
            this.Strength = strength;
            this.Toughness = toughness;
        }
    }
}