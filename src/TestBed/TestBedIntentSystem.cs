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

namespace TestBed
{
    public class TestBedIntentSystem : BaseSystem
    {
        private IEnumerable<TestBedIntentAspect> _aspectList;
        private KeyboardState _previousState;
        private KeyboardState _currentState;

        public TestBedIntentSystem(IChannelManager cm, int priority, params string[] channels): base(cm, priority, channels)
        {

        }
        public override void AddToGameManager(IGameManager gameManager)
        {
            _aspectList = gameManager.GetAspectList<TestBedIntentAspect>();
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
                var testBedIntent = intentAspect.GetComponent<TestBedIntentComponent>();
                testBedIntent.Reset();
                if (_currentState.IsKeyDown(cameraInputMap.MoveDownKeyboard))
                    testBedIntent.CameraMoveDown = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveUpKeyboard))
                    testBedIntent.CameraMoveUp = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveRightKeyboard))
                    testBedIntent.CameraMoveRight = true;
                if (_currentState.IsKeyDown(cameraInputMap.MoveLeftKeyboard))
                    testBedIntent.CameraMoveLeft = true;
            }

        }
    }
}
