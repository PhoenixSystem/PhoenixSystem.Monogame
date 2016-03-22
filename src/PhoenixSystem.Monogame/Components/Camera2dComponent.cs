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
    public class Camera2dComponent : BaseComponent
    {
        Viewport _viewport;
        public Camera2dComponent(Viewport viewport)
        {
            _viewport = viewport;
            _origin = new Vector2(viewport.Width / 2.0f, _viewport.Height / 2.0f);
        }

        public Vector2 Parallax { get; set; } = Vector2.One;  
        public float MaxZoom { get; set; }
        public float MinZoom { get; set; }

        public Viewport ViewPort { get { return _viewport; } }
        private Rectangle? _limits = null;
        public Rectangle? Limits
        {
            get { return _limits; }
            set { _limits = value; }
        }
        private float _zoom = 1.0f;
        private Vector2 _origin;

        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = MathHelper.Clamp(value, MinZoom, MaxZoom);
            }
        }

        private void ValidateZoom()
        {
            if (_limits.HasValue)
            {
                float minZoomX = (float)(_viewport.Width / _limits.Value.Width);
                float minZoomY = (float)(_viewport.Height / _limits.Value.Height);
                Zoom = MathHelper.Max(_zoom, MathHelper.Max(minZoomX, minZoomY));
            }
        }
        
        public Vector2 Origin
        {
            get
            {
                return _origin;
            }
        }

        public Matrix? CameraMatrix { get; set; }

        public override IComponent Clone()
        {
            return new Camera2dComponent(_viewport)
            {
                MinZoom = this.MinZoom,
                MaxZoom = this.MaxZoom,
                Zoom = _zoom,
                Parallax = this.Parallax,
                CameraMatrix = this.CameraMatrix
            };
        }

        public override void Reset()
        {
            Parallax = Vector2.One;
            MaxZoom = 1.5f;
            MinZoom = .5f;
            this.CameraMatrix = Matrix.Identity;
            
        }
    }
}
