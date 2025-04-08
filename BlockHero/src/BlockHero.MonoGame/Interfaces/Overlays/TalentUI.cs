using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public class TalentUI : IOverlayUI
    {
        public bool Visible { get; private set; } = false;

        public void Toggle()
        {
            Visible = !Visible;
        }

        public void Update(GameTime gameTime)
        {
            if (!Visible) return;

            // Talent upgrade logic here
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;

            spriteBatch.Draw(Game1.Instance.WhitePixel, new Rectangle(750, 50, 300, 400), Color.MediumPurple * 0.8f);
        }
    }
}
