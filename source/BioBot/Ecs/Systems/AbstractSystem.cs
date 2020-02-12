using System.Collections.Generic;
using DeenGames.BioBot.Ecs.Entities;

namespace DeenGames.BioBot.Ecs.Systems
{
    public class AbstractSystem
    {
        protected IList<Entity> entities = new List<Entity>();

        public void Add(Entity e)
        {
            this.entities.Add(e);
        }

        public void Remove(Entity e)
        {
            this.entities.Remove(e);
        }

        public virtual void OnUpdate()
        {

        }
    }
}