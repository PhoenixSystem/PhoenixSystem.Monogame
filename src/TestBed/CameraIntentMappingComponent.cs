using Microsoft.Xna.Framework.Input;
using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    public class CameraIntentMappingComponent : BaseComponent
    {
        public Keys MoveUpKeyboard { get; set; } = Keys.W;
        public Keys MoveDownKeyboard { get; set; } = Keys.S;
        public Keys MoveRightKeyboard { get; set; } = Keys.D;
        public Keys MoveLeftKeyboard { get; set; } = Keys.A;
        public override IComponent Clone()
        {
            return new CameraIntentMappingComponent()
            {
                MoveUpKeyboard = this.MoveUpKeyboard,
                MoveDownKeyboard = this.MoveDownKeyboard,
                MoveLeftKeyboard = this.MoveLeftKeyboard,
                MoveRightKeyboard = this.MoveRightKeyboard
            };
        }

        public override void Reset()
        {
            this.MoveUpKeyboard = Keys.W;
            this.MoveDownKeyboard = Keys.S;
            this.MoveRightKeyboard = Keys.D;
            this.MoveLeftKeyboard = Keys.A;
        }
    }
}
