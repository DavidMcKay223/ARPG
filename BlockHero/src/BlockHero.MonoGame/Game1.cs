using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BlockHero.MonoGame.Actors.Player;
using BlockHero.MonoGame.Actors;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BlockHero.MonoGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private List<Enemy> _enemies = new List<Enemy>();

        // Timer for spawning enemies (optional)
        private float _enemySpawnTimer = 0f;
        private float _enemySpawnCooldown = 3.0f; // Spawn enemy every 3 seconds
        private Random _random = new Random();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();

            _player = new Player();

            SpawnEnemy(new Vector2(500, 300));
            SpawnEnemy(new Vector2(700, 500));
            SpawnEnemy(new Vector2(200, 600));

            base.Initialize();
        }

        private void SpawnEnemy(Vector2 position)
        {
            Enemy newEnemy = new Enemy(position);
            newEnemy.LoadContent(Content);
            _enemies.Add(newEnemy);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _player.LoadContent(GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update Player
            _player.Update(gameTime);

            // --- Enemy Spawning Logic (Optional) ---
            _enemySpawnTimer += deltaTime;
            if (_enemySpawnTimer >= _enemySpawnCooldown)
            {
                _enemySpawnTimer = 0f;
                // Spawn enemy at a random edge of the screen
                int edge = _random.Next(4);
                Vector2 spawnPos = Vector2.Zero;
                switch (edge)
                {
                    case 0: spawnPos = new Vector2(-50, _random.Next(_graphics.PreferredBackBufferHeight)); break; // Left
                    case 1: spawnPos = new Vector2(_graphics.PreferredBackBufferWidth + 50, _random.Next(_graphics.PreferredBackBufferHeight)); break; // Right
                    case 2: spawnPos = new Vector2(_random.Next(_graphics.PreferredBackBufferWidth), -50); break; // Top
                    case 3: spawnPos = new Vector2(_random.Next(_graphics.PreferredBackBufferWidth), _graphics.PreferredBackBufferHeight + 50); break; // Bottom
                }
                SpawnEnemy(spawnPos);
            }

            // Update Enemies
            foreach (var enemy in _enemies)
            {
                enemy.Update(gameTime, _player); // Pass player info to enemy
            }

            // --- Collision Detection ---
            HandleCollisions();

            // --- Remove Inactive Entities ---
            _enemies.RemoveAll(enemy => !enemy.IsActive);

            base.Update(gameTime);
        }

        private void HandleCollisions()
        {
            // Get a copy of the player's projectiles to avoid modification issues during iteration
            var projectiles = _player.ActiveProjectiles.ToList();

            foreach (var projectile in projectiles)
            {
                if (!projectile.IsActive) continue; // Skip already inactive projectiles

                foreach (var enemy in _enemies)
                {
                    if (!enemy.IsActive) continue; // Skip inactive enemies

                    // Simple Rectangle intersection check
                    if (projectile.BoundingBox.Intersects(enemy.BoundingBox))
                    {
                        enemy.TakeDamage(projectile.Damage); // Enemy takes damage
                        projectile.IsActive = false;        // Deactivate projectile on hit
                        break; // Projectile hit an enemy, no need to check others for this projectile
                    }
                }
            }

            // Optional: Add Player <-> Enemy collision later
            // foreach (var enemy in _enemies) { ... check if enemy.BoundingBox intersects _player.BoundingBox ... }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            _spriteBatch.Begin();

            // Draw Enemies first (behind player)
            foreach (var enemy in _enemies)
            {
                enemy.Draw(_spriteBatch);
            }

            // Draw Player (and their projectiles)
            _player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
