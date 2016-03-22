using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    public class TestBedIntentComponent : BaseComponent
    {
        public bool CameraMoveRight { get; set; } = false;
        public bool CameraMoveLeft { get; set; } = false;
        public bool CameraMoveUp { get; set; } = false;
        public bool CameraMoveDown { get; set; } = false;

        public override IComponent Clone()
        {
            return new TestBedIntentComponent()
            {
                CameraMoveDown = this.CameraMoveDown,
                CameraMoveLeft = this.CameraMoveLeft,
                CameraMoveRight = this.CameraMoveRight,
                CameraMoveUp = this.CameraMoveUp
            };
        }

        public override void Reset()
        {
            CameraMoveUp = CameraMoveRight = CameraMoveLeft = CameraMoveDown = false;
        }
    }
}
