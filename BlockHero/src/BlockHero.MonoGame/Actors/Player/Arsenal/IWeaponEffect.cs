using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Actors.Player.Arsenal
{
    public interface IWeaponEffect
    {
        void Apply(Projectile source, Enemy target);
    }
}
