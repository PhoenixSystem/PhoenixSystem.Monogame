using PhoenixSystem.Engine.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoenixSystem.Engine;
using PhoenixSystem.Engine.Game;
using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Monogame.Aspects;
using PhoenixSystem.Engine.Extensions;
using PhoenixSystem.Monogame.Components;

namespace PhoenixSystem.Monogame.Systems
{
    public class FPSSystem : BaseSystem, IDrawableSystem
    {
        private IEnumerable<FPSAspect> _aspectList;

        public FPSSystem(IChannelManager cm, int priority, params string[] channels) : base(cm, priority, channels)
        {

        }

        public bool IsDrawing
        {
            get;
            private set;
        }

        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<FPSAspect>();
        }

        public void Draw(ITickEvent tickEvent)
        {
            IsDrawing = true;
            foreach(var aspect in _aspectList)
            {
                var str = aspect.GetComponent<StringComponent>();
                var fps = aspect.GetComponent<FPSComponent>();

                fps.FrameCounter++;

                str.Text = string.Format("fps: {0}", fps.FrameRate);
            }
            IsDrawing = false;
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {
            
        }

        public override void Update(ITickEvent tickEvent)
        {
            foreach(var aspect in _aspectList)
            {
                var fps = aspect.GetComponent<FPSComponent>();

                fps.ElapsedTime += tickEvent.ElapsedGameTime;

                if (fps.ElapsedTime > TimeSpan.FromSeconds(1))
                {
                    fps.ElapsedTime -= TimeSpan.FromSeconds(1);
                    fps.FrameRate = fps.FrameCounter;
                    fps.FrameCounter = 0;
                }
            }
        }
    }
}
