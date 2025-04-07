using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ChainLightning : IWeapon
    {
        private Texture2D _lightningTexture;
        private float _cooldownTimer = 0f;
        private List<ActiveChainLightning> _activeLightnings = new List<ActiveChainLightning>();

        public float CooldownTime => 1.5f;
        public int BaseDamage => 55;
        public float Range => 450f;
        public float BounceRange => 200f;
        public int MaxBounces => 6;
        public float LightningDuration => 0.25f;

        public bool CanAttack => _cooldownTimer <= 0f;

        public void LoadContent(ContentManager content)
        {
            _lightningTexture = content.Load<Texture2D>("Aesthetics/Sprites/white_pixel");
        }

        public void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= deltaTime;
            }

            if (isAttacking && CanAttack)
            {
                _cooldownTimer = CooldownTime;
                PerformChainLightning(ownerPosition);
            }

            for (int i = _activeLightnings.Count - 1; i >= 0; i--)
            {
                _activeLightnings[i].Update(gameTime, this);
                if (_activeLightnings[i].IsFinished)
                {
                    _activeLightnings.RemoveAt(i);
                }
            }
        }

        private void PerformChainLightning(Vector2 startPosition)
        {
            if (Game1.Instance == null || Game1.Instance.Enemies == null || !Game1.Instance.Enemies.Any())
                return;

            Enemy firstTarget = FindClosestEnemy(startPosition, Range, null, Game1.Instance.Enemies);

            if (firstTarget != null && firstTarget.IsActive)
            {
                var newLightning = new ActiveChainLightning(startPosition, firstTarget);
                newLightning.ApplyDamage(firstTarget, 0, this);
                _activeLightnings.Add(newLightning);
            }
        }

        // Changed access modifier to public
        public Enemy FindClosestEnemy(Vector2 origin, float range, List<Enemy> ignoredTargets, List<Enemy> allEnemies)
        {
            Enemy closestEnemy = null;
            float minDistanceSquared = float.MaxValue;
            Vector2 originCenter = origin;

            foreach (var enemy in allEnemies)
            {
                if (!enemy.IsActive || (ignoredTargets != null && ignoredTargets.Contains(enemy)))
                    continue;

                float distanceSquared = Vector2.DistanceSquared(originCenter, enemy.CenterPosition);

                if (distanceSquared < minDistanceSquared && distanceSquared <= range * range)
                {
                    minDistanceSquared = distanceSquared;
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        }

        public List<Projectile> GetNewProjectiles()
        {
            return new List<Projectile>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var lightning in _activeLightnings)
            {
                lightning.Draw(spriteBatch, _lightningTexture);
            }
        }
    }

    public class ActiveChainLightning
    {
        public List<Enemy> CurrentTargets { get; private set; } = new List<Enemy>();
        private int _bounceCount = 0;
        private float _timer;
        private int _maxTotalBounces;
        private ChainLightning _weapon;
        private Vector2 _startPosition;
        public bool IsFinished { get; private set; } = false;

        public ActiveChainLightning(Vector2 startPosition, Enemy firstTarget)
        {
            _startPosition = startPosition;
            CurrentTargets.Add(firstTarget);
            _timer = 0f; // Initialize timer
            if (Game1.Instance != null)
            {
                _timer = _weapon?.LightningDuration ?? 0.25f;
            }
        }

        public void Update(GameTime gameTime, ChainLightning weapon)
        {
            _weapon = weapon;
            if (Game1.Instance != null && Game1.Instance.Player != null)
            {
                _maxTotalBounces = _weapon.MaxBounces;// + (int)Math.Floor((double)Game1.Instance.Player.Intelligence / 100);
            }
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _timer -= deltaTime;

            if (_timer <= 0)
            {
                if (_bounceCount < _maxTotalBounces)
                {
                    Enemy lastTarget = CurrentTargets.Last();
                    Enemy nextTarget = _weapon.FindClosestEnemy(lastTarget.CenterPosition, _weapon.BounceRange, CurrentTargets, Game1.Instance.Enemies);

                    if (nextTarget != null && nextTarget.IsActive)
                    {
                        CurrentTargets.Add(nextTarget);
                        ApplyDamage(nextTarget, _bounceCount + 1, _weapon);
                        _bounceCount++;
                        _timer = _weapon.LightningDuration;
                    }
                    else
                    {
                        IsFinished = true;
                    }
                }
                else
                {
                    IsFinished = true;
                }
            }
        }

        public void ApplyDamage(Enemy enemy, int bounceNumber, ChainLightning weapon)
        {
            if (enemy != null && enemy.IsActive && Game1.Instance != null && Game1.Instance.Player != null)
            {
                //TODO: implement stats for player but later
                int intBonusDamage = 1;//Game1.Instance.Player.Intelligence;
                double damageMultiplier = Math.Pow(2, bounceNumber);
                float totalDamage = (weapon.BaseDamage + intBonusDamage) * (float)damageMultiplier;
                float damageRollMultiplier = 1f;

                //if (Game1.Instance.Player.Intelligence >= 1000)
                //    damageRollMultiplier = Game1.Instance.GameRandom.Next(1, 11);
                //else if (Game1.Instance.Player.Intelligence >= 500)
                //    damageRollMultiplier = Game1.Instance.GameRandom.Next(1, 8);
                //else if (Game1.Instance.Player.Intelligence >= 100)
                //    damageRollMultiplier = Game1.Instance.GameRandom.Next(1, 4);

                totalDamage *= damageRollMultiplier;

                enemy.TakeDamage((int)totalDamage);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D lightningTexture)
        {
            if (CurrentTargets.Any())
            {
                Vector2 start = _startPosition;
                foreach (var target in CurrentTargets)
                {
                    DrawLine(spriteBatch, lightningTexture, start, target.CenterPosition, Color.Yellow, 2);
                    start = target.CenterPosition;
                }
            }
        }

        private void DrawLine(SpriteBatch spriteBatch, Texture2D pixel, Vector2 startPoint, Vector2 endPoint, Color color, int thickness)
        {
            Vector2 edge = endPoint - startPoint;
            float angle = (float)Math.Atan2(edge.Y, edge.X);
            spriteBatch.Draw(pixel,
                new Rectangle(
                    (int)startPoint.X,
                    (int)startPoint.Y,
                    (int)edge.Length(),
                    thickness),
                null,
                color,
                angle,
                Vector2.Zero,
                SpriteEffects.None,
                0);
        }
    }
}
