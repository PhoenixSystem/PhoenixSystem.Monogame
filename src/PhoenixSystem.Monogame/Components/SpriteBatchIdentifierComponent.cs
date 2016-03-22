using PhoenixSystem.Engine.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixSystem.Monogame.Components
{
    public class SpriteBatchIdentifierComponent : BaseComponent
    {
        public string Identifier { get; set; } = string.Empty;
        public override IComponent Clone()
        {
            return new SpriteBatchIdentifierComponent() { Identifier = this.Identifier };
        }

        public override void Reset()
        {
            Identifier = string.Empty;
        }
    }
}
