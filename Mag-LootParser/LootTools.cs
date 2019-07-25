using System;
using System.Collections.Generic;

namespace Mag_LootParser
{
    static class LootTools
    {
        public static readonly HashSet<string> Trophies = new HashSet<string>
        {
            "Drudge Championship Belt",
            "Sword of Lost Light",
            "Gibbering Claw",
            "Shadow Captain's Heaume",
            "Hammer of Lightning",
            "Hammer of Lightning ", // A record exists with the trailing space
            "Drudge Championship Belt",
            "Baron's Amulet of Life Giving",
            "Quarter Staff of Fire",
        };

        public static bool IsTrophy(IdentResponse item)
        {
            var name = item.StringValues[Mag.Shared.Constants.StringValueKey.Name];

            return Trophies.Contains(name);
        }
    }
}
