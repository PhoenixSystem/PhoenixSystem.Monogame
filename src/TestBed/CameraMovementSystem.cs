using PhoenixSystem.Engine.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoenixSystem.Engine;
using PhoenixSystem.Engine.Game;
using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Extensions;
using PhoenixSystem.Monogame.Components;
using Microsoft.Xna.Framework;

namespace TestBed
{
    public class CameraMovementSystem : BaseSystem
    {
        private IEnumerable<CameraMovementAspect> _cameraAspects;
        private IEnumerable<TestBedIntentAspect> _intentAspects;

        public CameraMovementSystem(IChannelManager cm, int priority, params string[] channels) : base(cm, priority, channels)
        {

        }
        public override void AddToGameManager(IGameManager gameManager)
        {
            _intentAspects = gameManager.GetAspectList<TestBedIntentAspect>();
            _cameraAspects = gameManager.GetAspectList<CameraMovementAspect>();
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {
            
        }

        public override void Update(ITickEvent tickEvent)
        {
            var intentAspect = _intentAspects.FirstOrDefault();
            var cameraAspect = _cameraAspects.FirstOrDefault();

            if(intentAspect!=null && cameraAspect != null)
            {
                var velocity = cameraAspect.GetComponent<VelocityComponent>();
                var intent = intentAspect.GetComponent<TestBedIntentComponent>();
                var camera = cameraAspect.GetComponent<Camera2dComponent>();
                Vector2 direction = new Vector2();

                if (intent.CameraMoveDown)
                    direction.Y = 1;
                if (intent.CameraMoveUp)
                    direction.Y = -1;

                if (intent.CameraMoveLeft)
                    direction.X = -1;
                if (intent.CameraMoveRight)
                    direction.X = 1;

                if (intent.CameraZoomIn)
                    camera.Zoom += .05f;
                if (intent.CameraZoomOut)
                    camera.Zoom -= .05f;
                velocity.Direction = direction;
                   
            }
        }
    }
}
