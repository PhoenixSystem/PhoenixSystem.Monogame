using PhoenixSystem.Engine.Aspect;
using PhoenixSystem.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestBed
{
    [AssociatedComponents(typeof(TestBedIntentComponent),typeof(CameraIntentMappingComponent))]
    public class TestBedIntentAspect : BaseAspect
    {
        
    }
}
