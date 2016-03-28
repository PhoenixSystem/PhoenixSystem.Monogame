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
using Microsoft.Xna.Framework;

namespace PhoenixSystem.Monogame.Systems
{
    public class KeyframeTransformSystem : BaseSystem
    {
        private IEnumerable<KeyframeTransformAspect> _aspectList;

        public KeyframeTransformSystem(IChannelManager cm, int priority, params string[] channels) : base(cm, priority, channels)
        {

        }
        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<KeyframeTransformAspect>();
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {

        }

        public override void Update(ITickEvent tickEvent)
        {
            foreach (var aspect in _aspectList)
            {
                var position = aspect.GetComponent<PositionComponent>();
                var scale = aspect.GetComponent<ScaleComponent>();
                var rotation = aspect.GetComponent<RotationComponent>();
                var keyframe = aspect.GetComponent<KeyframeTransformComponent>();

                if (keyframe.IsComplete)
                    continue;

                if (keyframe.CurrentFrame.IsComplete)
                {
                    var lastFrame = keyframe.CurrentFrame;
                    keyframe.CurrentFrameIndex++;
                    keyframe.CurrentFrame.Position = new Vector4(lastFrame.Position.Y, keyframe.CurrentFrame.Position.Y,
                                                                    lastFrame.Position.W, keyframe.CurrentFrame.Position.W);
                    keyframe.CurrentFrame.Rotation = new Vector2(lastFrame.Rotation.Y, keyframe.CurrentFrame.Rotation.Y);
                    keyframe.CurrentFrame.Scale = new Vector2(lastFrame.Scale.Y, keyframe.CurrentFrame.Scale.Y);
                }                
                var distance = 1.0f / (keyframe.CurrentFrame.DurationInSeconds / tickEvent.ElapsedGameTime.TotalSeconds);  
                keyframe.CurrentFrame.LastAmount = MathHelper.Clamp(keyframe.CurrentFrame.LastAmount + (float)distance, 0.0f, 1.0f);
                position.CurrentPosition = Vector2.Lerp(new Vector2(keyframe.CurrentFrame.Position.X, keyframe.CurrentFrame.Position.Z),
                                                        new Vector2(keyframe.CurrentFrame.Position.Y, keyframe.CurrentFrame.Position.W),
                                                        keyframe.CurrentFrame.LastAmount);
                rotation.Factor = MathHelper.Lerp(keyframe.CurrentFrame.Rotation.X, keyframe.CurrentFrame.Rotation.Y, keyframe.CurrentFrame.LastAmount);
                scale.Factor = MathHelper.Lerp(keyframe.CurrentFrame.Scale.X, keyframe.CurrentFrame.Scale.Y, keyframe.CurrentFrame.LastAmount);            



            }
        }
    }
}
