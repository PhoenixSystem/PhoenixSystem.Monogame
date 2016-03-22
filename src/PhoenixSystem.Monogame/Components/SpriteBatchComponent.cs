using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class SpriteBatchComponent : BaseComponent
    {
        public SpriteSortMode SortMode { get; set; } = SpriteSortMode.Deferred;

        public BlendState BlendState { get; set; } = BlendState.AlphaBlend;

        public SamplerState SamplerState { get; set; } = SamplerState.LinearClamp;

        public DepthStencilState DepthStencilState { get; set; } = DepthStencilState.None;

        public RasterizerState RasterizerState { get; set; } = RasterizerState.CullCounterClockwise;

        public Matrix TransformMatrix { get; set; } = Matrix.Identity;

        public Effect Effect { get; set; } = null; 

        public override IComponent Clone()
        {
            return new SpriteBatchComponent()
            {
                BlendState = this.BlendState,
                SamplerState = this.SamplerState,
                SortMode = this.SortMode,
                DepthStencilState = this.DepthStencilState,
                RasterizerState = this.RasterizerState,
                TransformMatrix = this.TransformMatrix,
                Effect = this.Effect
            };
        }

        public override void Reset()
        {
            this.SortMode = SpriteSortMode.Deferred;
            this.BlendState = BlendState.AlphaBlend;
            this.SamplerState = SamplerState.LinearClamp;
            this.DepthStencilState = DepthStencilState.None;
            this.RasterizerState = RasterizerState.CullCounterClockwise;
            this.TransformMatrix = Matrix.Identity;
            this.Effect = null;
        }
    }
}
