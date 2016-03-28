using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class FPSComponent : BaseComponent
    {
        public int FrameCounter { get; set; } = 0;
        public int FrameRate { get; set; } = 0;
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public override IComponent Clone()
        {
            return new FPSComponent() { ElapsedTime = this.ElapsedTime, FrameCounter = this.FrameCounter, FrameRate = this.FrameRate };
        }

        public override void Reset()
        {
            FrameRate = FrameCounter = 0;
            ElapsedTime = TimeSpan.Zero;
        }
    }
}
