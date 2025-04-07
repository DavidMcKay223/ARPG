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
    public abstract class AbstractWeapon : IWeapon
    {
        protected float _cooldownTimer = 0f;
        protected List<Projectile> _newProjectiles = new();
        protected Texture2D _projectileTexture;

        public abstract float CooldownTime { get; }
        public virtual bool CanAttack => _cooldownTimer <= 0f;

        public virtual void LoadContent(ContentManager content) 
        {
            _projectileTexture = content.Load<Texture2D>("Aesthetics/Sprites/white_pixel");
        }

        public virtual void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            _newProjectiles.Clear();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_cooldownTimer > 0f)
                _cooldownTimer -= deltaTime;

            if (isAttacking && CanAttack)
            {
                _cooldownTimer = CooldownTime;
                Attack(ownerPosition);
            }
        }

        protected abstract void Attack(Vector2 ownerPosition);

        public virtual List<Projectile> GetNewProjectiles() => _newProjectiles;

        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}
