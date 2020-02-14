using System;
using DeenGames.BioBot.Ecs.Components;
using DeenGames.BioBot.Events;
using Puffin.Core.Events;

namespace DeenGames.BioBot.Ecs.Systems
{
    class DamageSystem : AbstractSystem
    {
        public DamageSystem()
        {
        }

        override public void OnUpdate()
        {
            foreach (var entity in this.entities)
            {
                var damageComponent = entity.Get<DamageComponent>();
                var healthComponent = entity.Get<HealthComponent>();

                if (healthComponent != null && damageComponent != null)
                {
                    int damage = Math.Max(damageComponent.Damage, 0);
                    healthComponent.CurrentHealth -= damage;
                    if (healthComponent.CurrentHealth <= 0)
                    {
                        EventBus.LatestInstance.Broadcast(Signal.EntityDied, entity);
                    }
                    else
                    {
                        EventBus.LatestInstance.Broadcast(Signal.EntityHurt, entity);
                    }
                }

                entity.Remove(damageComponent);
            }
        }
    }
}