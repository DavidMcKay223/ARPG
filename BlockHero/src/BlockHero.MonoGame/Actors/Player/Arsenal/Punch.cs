using BlockHero.MonoGame.Actors.Player.Bio;
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
    public class Punch : AbstractWeapon
    {
        public override int ManaCost => 3;
        public override float CooldownTime => 0.4f;
        public override int Damage => 25;

        private WeaponEffectManager _effectManager = new();

        public Punch(Stats stats) : base(stats) { }

        public override void LoadContent(ContentManager content)
        {
            _projectileTexture = content.Load<Texture2D>("Aesthetics/Sprites/boxing_glove_sprite");
        }

        protected override void Attack(Vector2 ownerPosition)
        {
            if (_stats.SpendMana(ManaCost))
            {
                var effect = new PunchEffect(ownerPosition, this, _projectileTexture);
                _effectManager.AddEffect(effect);
            }
        }

        public override void Update(GameTime gameTime, Vector2 ownerPosition, bool isAttacking)
        {
            base.Update(gameTime, ownerPosition, isAttacking);
            _effectManager.Update(gameTime, ownerPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _effectManager.Draw(spriteBatch);
        }
    }
}
