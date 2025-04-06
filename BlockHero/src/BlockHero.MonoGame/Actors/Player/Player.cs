using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlockHero.MonoGame.Actors.Player
{
    public class Player
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _speed;

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Player()
        {
            // Initial position (you can adjust this)
            _position = new Vector2(100, 100);
            _speed = 200f; // Pixels per second
        }

        public void LoadContent(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _texture = content.Load<Texture2D>("Aesthetics/Sprites/player_sprite");
        }

        public void Update(GameTime gameTime)
        {
            // Get the current keyboard state
            KeyboardState keyboardState = Keyboard.GetState();

            // Calculate movement based on input and speed
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W)) // Up
            {
                movement.Y -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.S)) // Down
            {
                movement.Y += 1;
            }
            if (keyboardState.IsKeyDown(Keys.A)) // Left
            {
                movement.X -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.D)) // Right
            {
                movement.X += 1;
            }

            // Normalize the movement vector to prevent faster diagonal movement
            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }

            // Apply movement to the position
            _position += movement * _speed * deltaTime;

            // You can add boundary checks here to keep the player within the screen
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_texture != null)
            {
                // Draw the texture at the player's position with a scale of 0.5
                spriteBatch.Draw(
                    _texture,
                    _position,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    0.125f, // Adjust this value to change the size
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
