using System;
using PhoenixSystem.Engine.Component;

namespace PhoenixSystem.Monogame.Components
{
    public class ScaleComponent : BaseComponent
    {
        public float Factor { get; set; } = 1.0f;

        public override IComponent Clone()
        {
            return new ScaleComponent() { Factor = this.Factor };
        }

        public override void Reset()
        {
            this.Factor = 1.0f;
        }
    }
}