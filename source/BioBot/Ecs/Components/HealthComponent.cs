namespace DeenGames.BioBot.Ecs.Components
{
    public class HealthComponent
    {
        public int CurrentHealth { get; set; }
        public int TotalHealth { get; set; }

        public HealthComponent(int totalHealth)
        {
            this.TotalHealth = totalHealth;
            this.CurrentHealth = totalHealth;
        }
    }
}