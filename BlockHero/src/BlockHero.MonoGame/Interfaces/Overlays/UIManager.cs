using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Interfaces.Overlays
{
    public class UIManager
    {
        public InventoryUI InventoryUI { get; } = new InventoryUI();
        public ShopUI ShopUI { get; } = new ShopUI();
        public TalentUI TalentUI { get; } = new TalentUI();

        private KeyboardState _previousKeyboardState;

        public void HandleInput()
        {
            var current = Keyboard.GetState();

            if (IsKeyPressed(current, Keys.I))
                InventoryUI.Toggle();

            if (IsKeyPressed(current, Keys.B))
                ShopUI.Toggle();

            if (IsKeyPressed(current, Keys.T))
                TalentUI.Toggle();

            _previousKeyboardState = current;
        }

        private bool IsKeyPressed(KeyboardState current, Keys key)
        {
            return current.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            InventoryUI.LoadContent(graphicsDevice, content);
        }

        public void Update(GameTime gameTime)
        {
            if (InventoryUI.Visible) InventoryUI.Update(gameTime);
            if (ShopUI.Visible) ShopUI.Update(gameTime);
            if (TalentUI.Visible) TalentUI.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (InventoryUI.Visible) InventoryUI.Draw(spriteBatch);
            if (ShopUI.Visible) ShopUI.Draw(spriteBatch);
            if (TalentUI.Visible) TalentUI.Draw(spriteBatch);
        }
    }
}
