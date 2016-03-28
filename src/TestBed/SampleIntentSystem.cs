using PhoenixSystem.Engine.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoenixSystem.Engine;
using PhoenixSystem.Engine.Game;
using Microsoft.Xna.Framework.Input;
using PhoenixSystem.Engine.Extensions;
using PhoenixSystem.Engine.Channel;

namespace PhoenixSystem.Monogame.Sample
{
    public class SampleIntentSystem : BaseSystem
    {
        private IEnumerable<SampleIntentAspect> _aspectList;
        private KeyboardState _previousState;
        private KeyboardState _currentState;

        public SampleIntentSystem(IChannelManager cm, int priority, params string[] channels): base(cm, priority, channels)
        {

        }
        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<SampleIntentAspect>();
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {

        }

        public override void Update(ITickEvent tickEvent)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();

            var intentAspect = _aspectList.FirstOrDefault();
            if (intentAspect != null)
            {
                var cameraInputMap = intentAspect.GetComponent<CameraIntentMappingComponent>();
                var SampleIntent = intentAspect.GetComponent<SampleIntentComponent>();
                SampleIntent.Reset();
                if (_currentState.IsKeyDown(cameraInputMap.MoveDownKeyboard))
                    SampleIntent.CameraMoveDown = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveUpKeyboard))
                    SampleIntent.CameraMoveUp = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveRightKeyboard))
                    SampleIntent.CameraMoveRight = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveLeftKeyboard))
                    SampleIntent.CameraMoveLeft = true;
                if (_currentState.IsKeyDown(cameraInputMap.ZoomInKeyboard))
                    SampleIntent.CameraZoomIn = true;
                if (_currentState.IsKeyDown(cameraInputMap.ZoomOutKeyboard))
                    SampleIntent.CameraZoomOut = true;
                        
            }

        }
    }
}
