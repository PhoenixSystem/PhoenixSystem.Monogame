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

namespace PhoenixSystemMonogame.Systems
{
    public class AlphaTweenSystem : BaseSystem
    {
        private IEnumerable<AlphaTweenAspect> _aspectList;

        public AlphaTweenSystem(IChannelManager cm, int priority, params string[] channels) : base(cm, priority, channels)
        {

        }

        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<AlphaTweenAspect>();
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
                var alphatween = aspect.GetComponent<AlphaTweenComponent>();
                var color = aspect.GetComponent<ColorComponent>();
                var curColor = color.Color;
                var directionModifier = alphatween.StartAlpha > alphatween.EndAlpha ? -1 : 1;
                if (!alphatween.Started)
                {
                    curColor.A = alphatween.StartAlpha;
                    alphatween.Started = true;
                }
                else if ((directionModifier > 0 && curColor.A >= alphatween.EndAlpha) ||
                           (directionModifier < 0 && curColor.A <= alphatween.EndAlpha))
                {
                    curColor.A = alphatween.EndAlpha;
                    if (alphatween.Loop)
                    {
                        var startAlpha = alphatween.StartAlpha;
                        alphatween.StartAlpha = alphatween.EndAlpha;
                        alphatween.EndAlpha = startAlpha;
                    }
                    else
                        alphatween.Complete = true;
                }
                else {
                    // do the tween
                    var durInMs = alphatween.TweenDuration;
                    var timeSinceLastTick = tickEvent.ElapsedGameTime.Milliseconds;
                    var percentStep = timeSinceLastTick / durInMs;
                    var distance = CalcDistance(alphatween.StartAlpha, alphatween.EndAlpha);
                    var step = percentStep * distance;
                    step *= directionModifier;
                    curColor.A = (byte)(curColor.A + step);
                }
                color.Color = curColor;
            }
        }
    }
}
