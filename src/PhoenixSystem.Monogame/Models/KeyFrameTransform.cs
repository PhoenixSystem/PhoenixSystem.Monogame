using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Models
{
    public class KeyframeTransform
    {
        public Vector2 Scale { get; set; }
        public Vector2 Rotation { get; set; }
        public Vector4 Position { get; set; }

        public float DurationInSeconds { get; set; }
        public float LastAmount { get; set; } = 0.0f;

        public bool IsComplete
        {
            get
            {
                return LastAmount >= 1.0f;
            }
        }
    }
}
