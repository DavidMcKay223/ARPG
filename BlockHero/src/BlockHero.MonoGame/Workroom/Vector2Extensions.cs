using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Workroom
{
    public static class Vector2Extensions
    {
        public static float AngleBetween(Vector2 a, Vector2 b)
        {
            if (a.LengthSquared() == 0 || b.LengthSquared() == 0)
                return 0;

            a.Normalize();
            b.Normalize();

            float dot = Vector2.Dot(a, b);
            dot = MathHelper.Clamp(dot, -1f, 1f);
            return (float)Math.Acos(dot);
        }
    }
}
