using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Models
{
    public class KeyframeTransformMaker
    {
        Vector2 _startPosition = Vector2.Zero;
        float _startScale = float.MinValue;
        float _startRotation = float.MinValue;

        private LinkedList<KeyframeTransform> _transforms = new LinkedList<KeyframeTransform>();
        private KeyframeTransformMaker(Vector2 startPos, float startScale, float startRotation)
        {
            _startPosition = startPos;
            _startScale = startScale;
            _startRotation = startRotation;
        }

        public KeyframeTransformMaker Then(float durationInSeconds, Vector2? position = null, float? scale = null, float? rotation = null)
        {
            Vector2 lastPos;
            float lastScale;
            float lastRotation;
            if (_transforms.Count == 0)
            {
                lastPos = _startPosition;
                lastScale = _startScale;
                lastRotation = _startRotation;
            }
            else
            {
                var last = _transforms.Last.Value;
                lastPos = new Vector2(last.Position.Y, last.Position.W);
                lastScale = last.Scale.Y;
                lastRotation = last.Rotation.Y;
            }
            
            _transforms.AddLast(
                new KeyframeTransform()
                {
                    DurationInSeconds = durationInSeconds,
                    Position = new Vector4(lastPos.X, position.HasValue ? position.Value.X : lastPos.X, lastPos.Y, position.HasValue ? position.Value.Y : lastPos.Y),
                    LastAmount = 0.0f,
                    Rotation = new Vector2(lastRotation, rotation ?? lastRotation),
                    Scale = new Vector2(lastScale, scale ?? lastScale)                
                }
                );
            return this;
        }

        public List<KeyframeTransform> List
        {
            get
            {
                return _transforms.ToList();
            }
        }

        public static KeyframeTransformMaker Start(Vector2 position, float scale, float rotation)
        {
            return new KeyframeTransformMaker(position, scale, rotation);
        }
    }
}
