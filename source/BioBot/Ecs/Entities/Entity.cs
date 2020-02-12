using System.Collections.Generic;
using System.Linq;

namespace DeenGames.BioBot.Ecs.Entities
{
    public class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public readonly string Name;

        private readonly List<object> components = new List<object>();

        public Entity(string name, int x, int y)
        {
            this.Name = name;
            this.X = x;
            this.Y = y;
        }

        public Entity Add(object component)
        {
            this.components.Add(component);
            return this;
        }

        public T Get<T>()
        {
            var type = typeof(T);
            return (T)this.components.FirstOrDefault(c => c.GetType() == type);
        }

        public void Remove(object component)
        {
            this.components.Remove(component);
        }
    }
}