using Microsoft.Xna.Framework;
using PhoenixSystem.Engine.Component;
using PhoenixSystem.Monogame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class KeyframeTransformComponent : BaseComponent
    {

        public List<KeyframeTransform> KeyFrames { get; set; } = new List<KeyframeTransform>();

        public int CurrentFrameIndex { get; set; } = 0;

        public bool IsComplete
        {
            get { return CurrentFrameIndex == KeyFrames.Count - 1 && CurrentFrame.IsComplete;  }
        }

        public KeyframeTransform CurrentFrame
        {
            get
            {
                return KeyFrames[CurrentFrameIndex];
            }
        }
        public override IComponent Clone()
        {
            var transforms = new List<KeyframeTransform>(KeyFrames.Count);
            foreach(var kf in KeyFrames)
            {
                transforms.Add(new KeyframeTransform()
                {
                    DurationInSeconds = kf.DurationInSeconds,
                    LastAmount = kf.LastAmount,
                    Position = new Vector4(kf.Position.X,kf.Position.Y,kf.Position.Z,kf.Position.W),
                    Scale = new Vector2(kf.Scale.X,kf.Scale.Y),
                    Rotation = new Vector2(kf.Rotation.X, kf.Rotation.Y)
                });
            }
            return new KeyframeTransformComponent()
            {
                CurrentFrameIndex = this.CurrentFrameIndex,
                KeyFrames = transforms
            };
        }

        public override void Reset()
        {
            KeyFrames.Clear();
            CurrentFrameIndex = 0;
        }
    }
}
