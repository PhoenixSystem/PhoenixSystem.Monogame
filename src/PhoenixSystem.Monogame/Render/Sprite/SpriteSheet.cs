using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace PhoenixSystem.Monogame.Render.Sprite
{
    public class SpriteSheet
    {
        public Texture2D Texture
        {
            get;
        }

        public SpriteSheet(Texture2D texture)
        {
            Texture = texture;
        }

        public IDictionary<string, SpriteFrame> SpriteList { get; private set; } = new Dictionary<string, SpriteFrame>();

        public void Add(SpriteSheet otherSheet)
        {
            foreach (var frame in otherSheet.SpriteList)
            {
                SpriteList.Add(frame);
            }
        }

    }
}