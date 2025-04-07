using BlockHero.MonoGame.Actors.Player.Arsenal;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Evolutions
{
    public class ArcaneExplosion : Ability
    {
        public override Keys ActivationKey => Keys.D2;
        public override float CooldownTime => 3f;

        protected override void Activate(Player player)
        {
            Texture2D missileTexture = Game1.Instance.Content.Load<Texture2D>("Aesthetics/Sprites/lightning_ball_sprite");
            
            for (int i = 0; i < 8; i++)
            {
                float angle = MathHelper.TwoPi * i / 8f;
                Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                var missile = new ExplosionMissile(
                    missileTexture,
                    player.CenterPosition,
                    dir,
                    300f,
                    50,     // damage
                    1.5f    // lifespan
                );

                Game1.Instance.SpawnProjectile(missile);
            }
        }
    }
}
