using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public class ShopUI : IOverlayUI
    {
        public bool Visible { get; private set; } = false;

        public void Toggle()
        {
            Visible = !Visible;
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible) return;

            // Shop interaction logic
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(Game1.Instance.WhitePixel, new Rectangle(400, 50, 300, 400), Color.SaddleBrown * 0.85f);
        }
    }
}
