using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockHero.MonoGame.Actors.Player.Arsenal;
using BlockHero.MonoGame.Actors.Player.Bio;
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
        private Stats _stats;

        private IWeapon _currentWeapon;
        private List<Projectile> _activeProjectiles = new List<Projectile>();

        public IReadOnlyList<Projectile> ActiveProjectiles => _activeProjectiles;

        public Rectangle BoundingBox => _texture == null ? Rectangle.Empty : new Rectangle((int)Position.X, (int)Position.Y, (int)(_texture.Width * 0.125f), (int)(_texture.Height * 0.125f));

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 CenterPosition => _texture == null ? _position : _position + new Vector2(BoundingBox.Width / 2f, BoundingBox.Height / 2f);

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public Stats Stats => _stats;

        public Player()
        {
            _position = new Vector2(100, 100);
            _speed = 200f; // Pixels per second
            _currentWeapon = new Hammer();
            _stats = new Stats();
        }

        public void LoadContent(GraphicsDevice graphicsDevice, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            _texture = content.Load<Texture2D>("Aesthetics/Sprites/player_sprite");
            _currentWeapon?.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
            UpdateProjectiles(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState(); // Get mouse state for attacking

            // --- Movement ---
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 movement = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
            if (keyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
            if (keyboardState.IsKeyDown(Keys.D)) movement.X += 1;

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
            }
            _position += movement * _speed * deltaTime;

            // --- Attacking ---
            bool isAttacking = mouseState.LeftButton == ButtonState.Pressed; // Attack on left click

            // Update the weapon (passing player position and attack state)
            _currentWeapon?.Update(gameTime, CenterPosition, isAttacking);

            // Add any new projectiles created by the weapon this frame
            if (_currentWeapon != null)
            {
                _activeProjectiles.AddRange(_currentWeapon.GetNewProjectiles());
            }

            // Optional: Boundary checks
            // _position.X = MathHelper.Clamp(_position.X, 0, 1200 - BoundingBox.Width);
            // _position.Y = MathHelper.Clamp(_position.Y, 0, 900 - BoundingBox.Height);
        }

        private void UpdateProjectiles(GameTime gameTime)
        {
            // Update all active projectiles
            for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
            {
                _activeProjectiles[i].Update(gameTime);
                if (!_activeProjectiles[i].IsActive)
                {
                    _activeProjectiles.RemoveAt(i); // Remove inactive projectiles
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw Player
            if (_texture != null)
            {
                spriteBatch.Draw(
                    _texture,
                    _position,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero, // Origin top-left
                    1.0f,
                    SpriteEffects.None,
                    0f
                );
            }

            // Draw Weapon (if it has visuals)
            _currentWeapon?.Draw(spriteBatch);

            // Draw Projectiles
            foreach (var projectile in _activeProjectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
