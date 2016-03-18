using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class AlphaTweenComponent : BaseComponent
    {
        public byte StartAlpha { get; set; } = 255;
        public byte EndAlpha { get; set; } = 0;
        public float TweenDuration { get; set; } = 1.0f;
        public bool Complete { get; set; } = false;
        public bool Started { get; set; } = false;
        public bool Loop { get; set; } = false;

        public override IComponent Clone()
        {
            return new AlphaTweenComponent()
            {
                Complete = this.Complete,
                Started = this.Started,
                Loop = this.Loop,
                TweenDuration = this.TweenDuration,
                StartAlpha = this.StartAlpha,
                EndAlpha = this.EndAlpha
            };
        }

        public override void Reset()
        {
            this.StartAlpha = 255;
            this.EndAlpha = 0;
            this.TweenDuration = 1.0f;
            this.Complete = false;
            this.Started = false;
            this.Loop = false;
        }
    }
}
