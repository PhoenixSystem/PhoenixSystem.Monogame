using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PhoenixSystem.Engine;
using PhoenixSystem.Engine.Channel;
using PhoenixSystem.Engine.Extensions;
using PhoenixSystem.Engine.Game;
using PhoenixSystem.Engine.System;
using PhoenixSystem.Monogame.Aspects;
using PhoenixSystem.Monogame.Components;

namespace PhoenixSystem.Monogame.Systems
{
    public class SpriteBatchRenderSystem : BaseSystem, IDrawableSystem
    {
        private const float ClockwiseNinetyDegreeRotation = (float)(Math.PI / 2.0f);
        private IEnumerable<Camera2dAspect> _cameraAspects;
        private IEnumerable<SpriteBatchAspect> _spriteBatchAspects;
        private IEnumerable<TextRenderAspect> _textAspects;
        private IEnumerable<TextureRenderAspect> _textureAspects;
        private IDictionary<string, SpriteBatch> _spriteBatches = new Dictionary<string, SpriteBatch>();
        private IDictionary<string, Camera2dComponent> _cameras = new Dictionary<string, Camera2dComponent>();
        private GraphicsDevice _graphicsDevice;

        public SpriteBatchRenderSystem(GraphicsDevice graphicsDevice, IChannelManager channelManager, int priority,
            params string[] channels)
            : base(channelManager, priority, channels)
        {
            _graphicsDevice = graphicsDevice;
        }

        public bool IsDrawing { get; set; }

        public override void AddToGameManager(IGameManager gameManager)
        {
            _textureAspects = gameManager.GetAspectList<TextureRenderAspect>();
            _cameraAspects = gameManager.GetAspectList<Camera2dAspect>();
            _textAspects = gameManager.GetAspectList<TextRenderAspect>();
            _spriteBatchAspects = gameManager.GetAspectList<SpriteBatchAspect>();
        }

        public void Draw(ITickEvent tickEvent)
        {
            foreach (var cam in _cameraAspects)
            {
                var component = cam.GetComponent<Camera2dComponent>();
                var identifier = cam.GetComponent<SpriteBatchIdentifierComponent>().Identifier;

                if (!_cameras.ContainsKey(identifier))
                {
                    _cameras.Add(identifier, component);
                }
            }
            foreach (var sbAspect in _spriteBatchAspects)
            {
                var id = sbAspect.GetComponent<SpriteBatchIdentifierComponent>().Identifier;
                var sb = sbAspect.GetComponent<SpriteBatchComponent>();

                if (!_spriteBatches.ContainsKey(sbAspect.GetComponent<SpriteBatchIdentifierComponent>().Identifier))
                {
                    _spriteBatches.Add(id, new SpriteBatch(_graphicsDevice));
                }

                _spriteBatches[id].Begin(sb.SortMode, sb.BlendState, sb.SamplerState, sb.DepthStencilState, sb.RasterizerState, sb.Effect, _cameras[id].CameraMatrix);
            }
            IsDrawing = true;
            RenderTextures();
            RenderText();

            foreach (var sb in _spriteBatches.Values)
            {
                sb.End();
            }
            IsDrawing = false;
        }

        private void RenderText()
        {
            foreach (var aspect in _textAspects)
            {
                var font = aspect.GetComponent<SpriteFontComponent>().Font;
                var text = aspect.GetComponent<StringComponent>().Text;
                var pos = aspect.GetComponent<PositionComponent>().CurrentPosition;
                var color = aspect.GetComponent<ColorComponent>().Color;
                var scale = aspect.GetComponent<ScaleComponent>().Factor;
                var layerDepth = aspect.GetComponent<RenderLayerComponent>().Depth;
                var id = aspect.GetComponent<SpriteBatchIdentifierComponent>().Identifier;
                _spriteBatches[id].DrawString(font, text, pos, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        private void RenderTextures()
        {
            foreach (var aspect in _textureAspects)
            {
                var position = aspect.GetComponent<PositionComponent>();
                var texture = aspect.GetComponent<TextureRenderComponent>();
                var color = aspect.GetComponent<ColorComponent>();
                var scale = aspect.GetComponent<ScaleComponent>();
                var rotation = aspect.GetComponent<RotationComponent>().Factor;
                var spriteEffects = texture.SpriteEffects;
                var origin = texture.Origin;
                var identifier = aspect.GetComponent<SpriteBatchIdentifierComponent>().Identifier;
                if (texture.IsRotated)
                {
                    rotation -= ClockwiseNinetyDegreeRotation;

                    switch (spriteEffects)
                    {
                        case SpriteEffects.FlipHorizontally:
                            spriteEffects = SpriteEffects.FlipVertically;
                            break;

                        case SpriteEffects.FlipVertically:
                            spriteEffects = SpriteEffects.FlipHorizontally;
                            break;

                        case SpriteEffects.None:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (spriteEffects)
                {
                    case SpriteEffects.FlipHorizontally:
                        origin.X = texture.SourceRect.Width - origin.X;
                        break;

                    case SpriteEffects.FlipVertically:
                        origin.Y = texture.SourceRect.Height - origin.Y;
                        break;

                    case SpriteEffects.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _spriteBatches[identifier].Draw(texture.Texture, position.CurrentPosition, sourceRectangle: texture.SourceRect, color: color.Color, rotation: rotation, origin: origin, scale: new Vector2(scale.Factor, scale.Factor), effects: spriteEffects);
            }
        }

        public override void RemoveFromGameManager(IGameManager gameManager)
        {
        }

        public override void Update(ITickEvent tickEvent)
        {
        }
    }
}