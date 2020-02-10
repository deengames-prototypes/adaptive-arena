using Puffin.Core;
using Puffin.Core.Ecs;

namespace DeenGames.BioBot.Scenes
{
    public class CoreGameScene : Scene
    {
        public CoreGameScene()
        {
            this.Add(new Entity().Label("Hello, world!"));
        }
    }
}