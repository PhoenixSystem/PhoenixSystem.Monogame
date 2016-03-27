using PhoenixSystem.Engine.Aspect;
using PhoenixSystem.Engine.Attributes;
using PhoenixSystem.Monogame.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Sample
{
    [AssociatedComponents(typeof(PositionComponent), typeof(VelocityComponent), typeof(Camera2dComponent))]
    public class CameraMovementAspect : BaseAspect
    {
        
    }
}
