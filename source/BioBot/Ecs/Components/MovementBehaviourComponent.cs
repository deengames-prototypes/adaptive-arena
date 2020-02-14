namespace DeenGames.BioBot.Ecs.Components
{
    public class MovementBehaviourComponent
    {
        public IdleBehaviour IdleBehaviour { get; private set; }
        public SeenPlayerBehaviour SeenPlayerBehaviour { get; private set; }
        public int SightRange { get; private set; }

        public MovementBehaviourComponent(int sightRange, IdleBehaviour idle, SeenPlayerBehaviour seen)
        {
            this.SightRange = sightRange;
            this.IdleBehaviour = idle;
            this.SeenPlayerBehaviour = seen;
        }
    }

    public enum IdleBehaviour
    {
        DoNothing,
        // Walk around randomly, all the time.
        RandomWalk,
        // Relentlessly stalk the player
        Stalk,
    }

    public enum SeenPlayerBehaviour
    {
        // Randomly walk X/Y toward player
        NaiveWalk,
        // Use path-finding
        Pathfind,
    }
}