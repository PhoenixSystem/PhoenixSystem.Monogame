using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Entity;
using PhoenixSystem.Engine.Game;
using PhoenixSystem.Engine.System;

namespace PhoenixSystem.Monogame.Sample
{
    public class SampleGameManager : BaseGameManager
    {
        public SampleGameManager(IEntityAspectManager eam, IEntityManager em, ISystemManager sm) : base(eam, em, sm, new ManagerManager())
        {

        }
    }
}