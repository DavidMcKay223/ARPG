using BlockHero.MonoGame.Actors.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems.Modifiers
{
    public class RangeModifier : IGearModifier
    {
        private float rangeBoost = 50f;
        public void Apply(Player player) => player.Stats.Range += rangeBoost;
        public void Remove(Player player) => player.Stats.Range -= rangeBoost;
        public string GetTooltip() => $"+{rangeBoost} Attack Range";
    }
}
