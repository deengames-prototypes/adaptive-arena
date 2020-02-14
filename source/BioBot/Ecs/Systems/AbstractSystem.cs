using System.Collections.Generic;
using DeenGames.BioBot.Ecs.Entities;

namespace DeenGames.BioBot.Ecs.Systems
{
    public class AbstractSystem
    {
        protected IList<BioBotEntity> entities = new List<BioBotEntity>();

        public virtual void Add(BioBotEntity e)
        {
            this.entities.Add(e);
        }

        public void Remove(BioBotEntity e)
        {
            this.entities.Remove(e);
        }

        public virtual void OnUpdate()
        {

        }
    }
}