using PhoenixSystem.Engine.Aspect;
using PhoenixSystem.Engine.Attributes;
using PhoenixSystem.Monogame.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Aspects
{
    [AssociatedComponents(typeof(AlphaTweenComponent),typeof(ColorComponent))]
    public class AlphaTweenAspect :BaseAspect
    {
        
    }
}
