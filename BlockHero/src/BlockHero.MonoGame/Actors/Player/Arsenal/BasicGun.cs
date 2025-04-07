using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class BasicGun : IWeapon
    {
        private Texture2D _projectileTexture;
        private float _cooldownTimer = 0f;
        private List<Projectile> _newProjectiles = new List<Projectile>();

        public float CooldownTime => 0.5f; // 2 attacks per second
        public bool CanAttack => _cooldownTimer <= 0f;

        public void LoadContent(ContentManager content)
        {
            _projectileTexture = content.Load<Texture2D>("Aesthetics/Sprites/projectile_sprite"); // Adjust path as needed
        }

        public void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            _newProjectiles.Clear(); // Clear projectiles from the previous frame
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update cooldown
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= deltaTime;
            }

            // Check for attack input and cooldown
            if (isAttacking && CanAttack && _projectileTexture != null)
            {
                _cooldownTimer = CooldownTime; // Reset cooldown

                // --- Calculate Attack Direction (towards mouse) ---
                MouseState mouseState = Mouse.GetState();
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                Vector2 direction = mousePosition - ownerPosition;
                if (direction == Vector2.Zero)
                {
                    direction = Vector2.UnitX; // Default direction if mouse is exactly on player
                }
                // Do not normalize here if you want speed to be constant; normalization happens in Projectile constructor

                // --- Create Projectile ---
                // Adjust spawn position (e.g., center of player, front of player)
                Vector2 projectileStartPosition = ownerPosition + new Vector2(20, 20); // Example offset

                Projectile newProjectile = new Projectile(
                    _projectileTexture,
                    projectileStartPosition,
                    direction,
                    speed: 400f,      // Projectile speed
                    damage: 10,       // Projectile damage
                    lifespan: 2.0f    // Projectile lasts 2 seconds
                );
                _newProjectiles.Add(newProjectile);
            }
        }

        // Returns the list of projectiles created *this frame*
        public List<Projectile> GetNewProjectiles()
        {
            return _newProjectiles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // This weapon doesn't have its own visual representation on the player
            // But you could draw one here if desired.
        }
    }
}
