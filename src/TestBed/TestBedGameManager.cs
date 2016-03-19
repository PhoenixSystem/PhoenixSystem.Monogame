using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Entity;
using PhoenixSystem.Engine.Game;

namespace TestBed
{
    public class TestBedGameManager : BaseGameManager
    {
        public TestBedGameManager(IEntityAspectManager eam, IEntityManager em, IChannelManager cm) : base(eam, em, cm)
        {

        }
    }
}