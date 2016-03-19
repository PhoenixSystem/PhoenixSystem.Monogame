using PhoenixSystem.Engine.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoenixSystem.Engine;
using PhoenixSystem.Engine.Game;
using PhoenixSystem.Monogame.Aspects;
using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Extensions;
using PhoenixSystem.Monogame.Components;
using Microsoft.Xna.Framework;

namespace PhoenixSystem.Monogame.Systems
{
    public class LerpColorSystem : BaseSystem
    {
        private IEnumerable<LerpColorAspect> _aspectList;

        public LerpColorSystem(IChannelManager cm, int priority, params string[] channels) : base(cm, priority, channels)
        {

        }

        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<LerpColorAspect>();
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {

        }

        private float CalcDistance(float num1, float num2)
        {
            if (num1 > num2)
                return num1 - num2;
            else
                return num2 - num1;
        }

        public override void Update(ITickEvent tickEvent)
        {
            foreach (var aspect in _aspectList)
            {
                var lerp = aspect.GetComponent<LerpColorComponent>();
                var color = aspect.GetComponent<ColorComponent>();

                if (lerp.LastAmount == 0.0f)
                {
                    color.Color = lerp.From;

                }
                else if (lerp.LastAmount == 1.0f && lerp.Loop)
                {

                    var from = lerp.From;
                    lerp.From = lerp.To;
                    lerp.To = from;
                    lerp.LastAmount = 0.0f;
                }

                if (lerp.LastAmount <= 1.0f)
                {
                    var distance = 1.0f / (lerp.DurationInSeconds / tickEvent.ElapsedGameTime.TotalSeconds);
                    lerp.LastAmount = MathHelper.Clamp(lerp.LastAmount + (float)distance, 0.0f, 1.0f);
                    color.Color = Color.Lerp(lerp.From, lerp.To, lerp.LastAmount);
                }
            }
        }
    }
}
