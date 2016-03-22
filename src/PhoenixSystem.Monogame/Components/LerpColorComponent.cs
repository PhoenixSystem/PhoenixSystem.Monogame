using Microsoft.Xna.Framework;
using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class LerpColorComponent : BaseComponent
    {
        public Color From { get; set; } = Color.White;
        public Color To { get; set; } = Color.Transparent;
        public float DurationInSeconds { get; set; } = 1.0f;
        public float LastAmount { get; set; } = 0.0f;
        public bool Loop { get; set; } = false;
        public override IComponent Clone()
        {
            return new LerpColorComponent()
            {
                Loop = this.Loop,
                DurationInSeconds = this.DurationInSeconds,
                From = this.From,
                To = this.To,
             };
        }

        public override void Reset()
        {
            this.From = Color.White;
            this.To = Color.Transparent;
            this.DurationInSeconds = 1.0f;
            this.Loop = false;
            }
    }
}
