using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Sample
{
    public class SampleIntentComponent : BaseComponent
    {
        public bool CameraMoveRight { get; set; } = false;
        public bool CameraMoveLeft { get; set; } = false;
        public bool CameraMoveUp { get; set; } = false;
        public bool CameraMoveDown { get; set; } = false;
        public bool CameraZoomIn { get; set; } = false;
        public bool CameraZoomOut { get; set; } = false;

        public override IComponent Clone()
        {
            return new SampleIntentComponent()
            {
                CameraMoveDown = this.CameraMoveDown,
                CameraMoveLeft = this.CameraMoveLeft,
                CameraMoveRight = this.CameraMoveRight,
                CameraMoveUp = this.CameraMoveUp,
                CameraZoomIn = this.CameraZoomIn,
                CameraZoomOut = this.CameraZoomOut
            };
        }

        public override void Reset()
        {
            CameraZoomIn = CameraZoomOut = CameraMoveUp = CameraMoveRight = CameraMoveLeft = CameraMoveDown = false;
        }
    }
}
