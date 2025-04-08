using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public interface IOverlayUI
    {
        bool Visible { get; }
        void Toggle();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
