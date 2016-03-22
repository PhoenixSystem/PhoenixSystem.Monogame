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
using PhoenixSystem.Monogame.Aspects;

namespace PhoenixSystem.Monogame.Systems
{
    public class Camera2dSystem : BaseSystem
    {
        private IEnumerable<Camera2dAspect> _aspectList;

        public Camera2dSystem(IChannelManager icm, int priority, params string[] channels) : base(icm, priority, channels)
        {

        }

        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<Camera2dAspect>();
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {

        }

        private void ValidatePosition(Camera2dAspect aspect)
        {
            
            var camera = aspect.GetComponent<Camera2dComponent>();
            var position = aspect.GetComponent<PositionComponent>();
            if (!camera.CameraMatrix.HasValue) return;
            Vector2 cameraWorldMin = Vector2.Transform(Vector2.Zero, Matrix.Invert(camera.CameraMatrix.Value));
            Vector2 cameraSize = new Vector2(camera.ViewPort.Width, camera.ViewPort.Height) / camera.Zoom;
            Vector2 limitWorldMin = new Vector2(camera.Limits.Value.Left, camera.Limits.Value.Top);
            Vector2 limitWorldMax = new Vector2(camera.Limits.Value.Right, camera.Limits.Value.Bottom);
            Vector2 positionOffset = position.CurrentPosition - cameraWorldMin;
            position.CurrentPosition = Vector2.Clamp(cameraWorldMin, limitWorldMin, limitWorldMax - cameraSize) + positionOffset;
        }

        public override void Update(ITickEvent tickEvent)
        {
            foreach (var aspect in _aspectList)
            {
                var camera = aspect.GetComponent<Camera2dComponent>();
                var rotation = aspect.GetComponent<RotationComponent>();
                var position = aspect.GetComponent<PositionComponent>();

                ValidatePosition(aspect);
                camera.CameraMatrix =
                           Matrix.CreateTranslation(new Vector3(-position.CurrentPosition * camera.Parallax, 0.0f)) *
                           Matrix.CreateTranslation(new Vector3(-camera.Origin, 0.0f)) *
                           Matrix.CreateRotationX(rotation.Factor) * 
                           Matrix.CreateScale(camera.Zoom,camera.Zoom,1) * 
                           Matrix.CreateTranslation(new Vector3(camera.Origin, 0.0f));

            }
        }
    }
}
