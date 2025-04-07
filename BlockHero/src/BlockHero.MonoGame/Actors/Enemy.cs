using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace BlockHero.MonoGame.Actors
{
    public class Enemy
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }
        public int Health { get; set; }
        public float ChaseRange { get; set; } // How close the player needs to be to trigger chasing
        public float AttackRange { get; set; } // How close the enemy needs to be to attack (implement later)
        public bool IsActive { get; set; } // Use this to remove dead enemies

        // Basic state machine
        private enum EnemyState { Idle, Chasing }
        private EnemyState _currentState = EnemyState.Idle;

        // For Idle movement (optional)
        private Vector2 _idleMoveTarget;
        private float _idleTimer;
        private static Random _random = new Random(); // Static random for efficiency

        // Bounding box for collision
        public Rectangle BoundingBox => Texture == null ? Rectangle.Empty : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);


        public Enemy(Vector2 position)
        {
            Position = position;
            Speed = 100f; // Slower than player?
            Health = 50;
            ChaseRange = 350f; // Pixels
            AttackRange = 50f; // Pixels
            IsActive = true;
            SetNewIdleTarget();
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Aesthetics/Sprites/enemy_sprite");
        }

        public void Update(GameTime gameTime, Player.Player player) // Pass the player object
        {
            if (!IsActive || player == null) return; // Don't update if inactive or player is null

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float distanceToPlayer = Vector2.Distance(Position, player.Position);

            // --- State Transitions ---
            if (distanceToPlayer <= ChaseRange)
            {
                _currentState = EnemyState.Chasing;
            }
            else
            {
                // Optional: Add a buffer zone so it doesn't flip-flop constantly
                // if (distanceToPlayer > ChaseRange * 1.1f)
                _currentState = EnemyState.Idle;
            }

            // --- Execute State Logic ---
            switch (_currentState)
            {
                case EnemyState.Idle:
                    UpdateIdle(deltaTime);
                    break;
                case EnemyState.Chasing:
                    UpdateChasing(deltaTime, player.Position);
                    break;
            }

            // Basic boundary checks (optional)
            // Position = Vector2.Clamp(Position, Vector2.Zero, new Vector2(1200 - BoundingBox.Width, 900 - BoundingBox.Height));

        }

        private void UpdateIdle(float deltaTime)
        {
            // Simple idle: move towards a random point for a while, then pick a new point
            _idleTimer -= deltaTime;
            if (_idleTimer <= 0 || Vector2.DistanceSquared(Position, _idleMoveTarget) < 10f) // Close enough to target
            {
                SetNewIdleTarget();
            }

            // Move towards the idle target
            Vector2 direction = _idleMoveTarget - Position;
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }
            Position += direction * (Speed * 0.5f) * deltaTime; // Move slower when idle
        }

        private void SetNewIdleTarget()
        {
            // Pick a random point within a certain radius of the current position
            float wanderRadius = 150f;
            _idleMoveTarget = Position + new Vector2(
                (float)(_random.NextDouble() * 2 - 1) * wanderRadius, // Random X offset
                (float)(_random.NextDouble() * 2 - 1) * wanderRadius  // Random Y offset
            );
            // Clamp target to screen bounds if needed
            // _idleMoveTarget = Vector2.Clamp(_idleMoveTarget, Vector2.Zero, new Vector2(1200, 900));

            _idleTimer = (float)(_random.NextDouble() * 3 + 2); // Wait 2-5 seconds before picking new target
        }


        private void UpdateChasing(float deltaTime, Vector2 playerPosition)
        {
            // Move directly towards the player
            Vector2 direction = playerPosition - Position;
            if (direction != Vector2.Zero) // Avoid NaN if distance is zero
            {
                // Optional: Stop moving if within attack range (implement attack later)
                // if (direction.LengthSquared() > AttackRange * AttackRange)
                // {
                direction.Normalize();
                Position += direction * Speed * deltaTime;
                // }
            }
        }

        // Method to handle taking damage
        public void TakeDamage(int amount)
        {
            Health -= amount;
            if (Health <= 0)
            {
                IsActive = false; // Enemy is dead
                                  // Add effects here: play sound, particle effect, drop loot etc.
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive && Texture != null)
            {
                spriteBatch.Draw(
                    Texture,
                    Position,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero, // Origin top-left
                    0.05f,
                    SpriteEffects.None,
                    0f
                );
            }
        }
    }
}
