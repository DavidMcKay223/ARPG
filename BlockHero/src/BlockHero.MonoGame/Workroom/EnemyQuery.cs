using BlockHero.MonoGame.Actors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.Workroom
{
    public static class EnemyQuery
    {
        public static Enemy GetClosestEnemy(Vector2 origin, IEnumerable<Enemy> candidates, float maxRange)
        {
            return candidates
                .Where(e => e.IsActive && Vector2.Distance(origin, e.Position) <= maxRange)
                .OrderBy(e => Vector2.Distance(origin, e.Position))
                .FirstOrDefault();
        }
    }
}
