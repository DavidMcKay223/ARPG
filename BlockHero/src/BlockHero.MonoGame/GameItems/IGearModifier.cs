using BlockHero.MonoGame.Actors.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockHero.MonoGame.GameItems
{
    public interface IGearModifier
    {
        void Apply(Player player);
        void Remove(Player player);
        string GetTooltip();
    }
}
