using System;
using DeenGames.BioBot.Ecs.Components;
using DeenGames.BioBot.Ecs.Entities;
using DeenGames.BioBot.Events;
using DeenGames.BioBot.Model;
using Puffin.Core.Events;

namespace DeenGames.BioBot.Ecs.Systems
{
    public class MovementBehaviourSystem : AbstractSystem
    {
        private Random random = new Random();
        private AreaMap areaMap;

        public MovementBehaviourSystem(AreaMap areaMap)
        {
            this.areaMap = areaMap;
            EventBus.LatestInstance.Subscribe(Signal.PlayerMoved, this.OnPlayerMoved);
        }

        override public void Add(BioBotEntity e)
        {
            if (e.Get<MovementBehaviourComponent>() != null)
            {
                base.Add(e);
            }
        }

        private void OnPlayerMoved(object obj)
        {
            foreach (var entity in this.entities)
            {
                var behaviour = entity.Get<MovementBehaviourComponent>();

                var distanceToPlayer = GoRogue.Distance.EUCLIDEAN.Calculate(entity.X, entity.Y, areaMap.Player.X, areaMap.Player.Y);
                if (distanceToPlayer <= behaviour.SightRange)
                {
                    // Apply seen behaviour
                    switch (behaviour.SeenPlayerBehaviour)
                    {
                        case SeenPlayerBehaviour.NaiveStalk:
                            this.NaivelyMoveTowardsPlayer(entity);
                            break;
                        case SeenPlayerBehaviour.Pathfind:
                            this.PathFindTowardsPlayer(entity);
                            break;
                        default:
                            throw new NotImplementedException($"{behaviour.SeenPlayerBehaviour} isn't implemented yet.");
                    }
                }
                else
                {
                    // Apply idle behaviour
                    switch (behaviour.IdleBehaviour)
                    {
                        case IdleBehaviour.DoNothing:
                            break;
                        case IdleBehaviour.RandomWalk:
                            this.RandomWalk(entity);
                            break;
                        case IdleBehaviour.NaiveStalk:
                            this.NaivelyMoveTowardsPlayer(entity);
                            break;
                        case IdleBehaviour.Pathfind:
                            this.PathFindTowardsPlayer(entity);
                            break;
                        default:
                            throw new NotImplementedException($"{behaviour.IdleBehaviour} isn't implemented yet.");
                    }
                }
            }
        }

        private void NaivelyMoveTowardsPlayer(BioBotEntity entity)
        {
            // Look at whether we should move on the X-axis, Y-axis, or both, and then move randomly from whichever ones we need to move.
            (var dx, var dy) = (areaMap.Player.X - entity.X, areaMap.Player.Y - entity.Y);
            if (dx == 0)
            {
                areaMap.TryToMove(entity, 0, Math.Sign(dy));
            }
            else if (dy  == 0)
            {
                areaMap.TryToMove(entity, Math.Sign(dx), 0);
            }
            else
            {
                // Randomly move
                if (random.NextDouble() < 0.5)
                {
                    areaMap.TryToMove(entity, Math.Sign(dx), 0);
                }
                else
                {
                    areaMap.TryToMove(entity, 0, Math.Sign(dy));
                }
            }
        }

        private void RandomWalk(BioBotEntity entity)
        {
            // Actually random, doesn't matter if that tile is walkable or not. Meaning, sometimes do nothing. :shrug:
            var dx = random.NextDouble() < 0.5 ? -1 : 1;
            var dy = random.NextDouble() < 0.5 ? -1 : 1;
            if (random.NextDouble() < 0.5)
            {
                areaMap.TryToMove(entity, Math.Sign(dx), 0);
            }
            else
            {
                areaMap.TryToMove(entity, 0, Math.Sign(dy));
            }
        }

        private void PathFindTowardsPlayer(BioBotEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}