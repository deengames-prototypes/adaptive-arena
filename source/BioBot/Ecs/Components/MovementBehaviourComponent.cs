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
        RandomWalk, // Walk around randomly, all the time.
        NaiveStalk, // Walk toward the player, but not intelligently
        Pathfind, // Relentlessly stalk the player, with pathfinding
    }

    public enum SeenPlayerBehaviour
    {
        NaiveStalk, // Randomly walk X/Y toward player
        Pathfind, // Use path-finding
    }
}