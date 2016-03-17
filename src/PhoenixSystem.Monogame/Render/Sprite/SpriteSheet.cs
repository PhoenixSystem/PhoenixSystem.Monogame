using System.Collections.Generic;

namespace PhoenixSystem.Monogame.Render.Sprite
{
    public class SpriteSheet
    {
        

        public SpriteSheet()
        {
            
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