using BlockHero.MonoGame.Actors.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems.Modifiers
{
    public class DoubleAttackModifier : IGearModifier
    {
        public void Apply(Player player) => player.Stats.AttackCount += 1;
        public void Remove(Player player) => player.Stats.AttackCount -= 1;
        public string GetTooltip() => "Attacks twice per click";
    }
}
