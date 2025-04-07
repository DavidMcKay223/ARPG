using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlockHero.MonoGame.Workroom;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public class ChainLightningEffect : ActiveWeaponEffect
    {
        private readonly Vector2 _startPosition;
        private readonly ChainLightning _weapon;
        private int _bounceCount;
        private readonly int _maxBounces;
        private bool _hasBounced = false;

        public ChainLightningEffect(Vector2 start, Enemy firstTarget, ChainLightning weapon, Texture2D texture)
            : base(texture, weapon.LightningDuration)
        {
            _startPosition = start;
            _weapon = weapon;
            _maxBounces = weapon.MaxBounces;
            Targets.Add(firstTarget);
            ApplyDamage(firstTarget, _weapon.BaseDamage, 0);
        }

        protected override Vector2 GetStartPosition() => _startPosition;

        public override void Update(GameTime gameTime)
        {
            if (!_hasBounced)
            {
                var lastTarget = Targets.Last();

                while (_bounceCount < _maxBounces)
                {
                    var next = EnemyQuery.FindClosestEnemy(
                        lastTarget.CenterPosition,
                        _weapon.BounceRange,
                        Targets,
                        Game1.Instance.Enemies
                    );

                    if (next == null || !next.IsActive)
                        break;

                    Targets.Add(next);
                    _bounceCount++;
                    ApplyDamage(next, _weapon.BaseDamage, _bounceCount);
                    lastTarget = next;
                }

                _hasBounced = true;
                Timer = _weapon.LightningDuration;
            }
            else
            {
                Timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Timer <= 0f)
                    IsFinished = true;
            }
        }
    }
}
