using System;
using System.Collections.Generic;

namespace Mag_LootParser.ItemGroups.Helpers
{
    public static class TwoHandedWeaponNames
    {
        public static readonly HashSet<string> GreatAxes = new HashSet<string>()
        {
            // two-handed - axe
            "Greataxe",
            "Acid Greataxe",
            "Lightning Greataxe",
            "Flaming Greataxe",
            "Frost Greataxe",
        };

        public static readonly HashSet<string> GreatStarMaces = new HashSet<string>()
        {
            // two-handed - mace
            "Great Star Mace",
            "Acid Great Star Mace",
            "Lightning Great Star Mace",
            "Flaming Great Star Mace",
            "Frost Great Star Mace",
        };

        public static readonly HashSet<string> KhandaHandledMaces = new HashSet<string>()
        {
            // two-handed - mace
            "Khanda-handled Mace",
            "Acid Khanda-handled Mace",
            "Lightning Khanda-handled Mace",
            "Flaming Khanda-handled Mace",
            "Frost Khanda-handled Mace",
        };

        public static readonly HashSet<string> Quadrelles = new HashSet<string>()
        {
            // two-handed - mace
            "Quadrelle",
            "Acid Quadrelle",
            "Lightning Quadrelle",
            "Flaming Quadrelle",
            "Frost Quadrelle",
        };

        public static readonly HashSet<string> Tetsubos = new HashSet<string>()
        {
            // two-handed - mace
            "Tetsubo",
            "Acid Tetsubo",
            "Lightning Tetsubo",
            "Flaming Tetsubo",
            "Frost Tetsubo",
        };

        public static readonly HashSet<string> Assagais = new HashSet<string>()
        {
            // two-handed - spear
            "Assagai",
            "Acid Assagai",
            "Lightning Assagai",
            "Flaming Assagai",
            "Frost Assagai",
        };

        public static readonly HashSet<string> Corsecas = new HashSet<string>()
        {
            // two-handed - spear
            "Corsesca",
            "Acid Corsesca",
            "Lightning Corsesca",
            "Flaming Corsesca",
            "Frost Corsesca",
        };

        public static readonly HashSet<string> MagariYaris = new HashSet<string>()
        {
            // two-handed - spear
            "Magari Yari",
            "Acid Magari Yari",
            "Lightning Magari Yari",
            "Flaming Magari Yari",
            "Frost Magari Yari",
        };

        public static readonly HashSet<string> Pikes = new HashSet<string>()
        {
            // two-handed - spear
            "Pike",
            "Acid Pike",
            "Lightning Pike",
            "Flaming Pike",
            "Frost Pike",
        };

        public static readonly HashSet<string> Nodachis = new HashSet<string>()
        {
            // two-handed - sword
            "Nodachi",
            "Acid Nodachi",
            "Lightning Nodachi",
            "Flaming Nodachi",
            "Frost Nodachi",
        };

        public static readonly HashSet<string> Shashqas = new HashSet<string>()
        {
            // two-handed - sword
            "Shashqa",
            "Acid Shashqa",
            "Lightning Shashqa",
            "Flaming Shashqa",
            "Frost Shashqa",
        };

        public static readonly HashSet<string> Spadones = new HashSet<string>()
        {
            // two-handed - sword
            "Spadone",
            "Acid Spadone",
            "Lightning Spadone",
            "Flaming Spadone",
            "Frost Spadone",
        };

        public static List<string> Names = new List<string>()
        {
            "GreatAxes",
            "GreatStarMaces",
            "KhandaHandledMaces",
            "Quadrelles",
            "Tetsubos",
            "Assagais",
            "Corsecas",
            "MagariYaris",
            "Pikes",
            "Nodachis",
            "Shashqas",
            "Spadones",
        };


        public static List<HashSet<string>> AllTwoHandedWeapons = new List<HashSet<string>>()
        {
            GreatAxes,          // axe
            GreatStarMaces,     // mace
            KhandaHandledMaces,
            Quadrelles,
            Tetsubos,
            Assagais,           // spear
            Corsecas,
            MagariYaris,
            Pikes,
            Nodachis,           // sword
            Shashqas,
            Spadones,
        };

        public static List<HashSet<string>> TwoHandedAxes = new List<HashSet<string>>()
        {
            GreatAxes,          // axe
        };

        public static List<HashSet<string>> TwoHandedMaces = new List<HashSet<string>>()
        {
            GreatStarMaces,     // mace
            KhandaHandledMaces,
            Quadrelles,
            Tetsubos,
        };

        public static List<HashSet<string>> TwoHandedSpears = new List<HashSet<string>>()
        {
            Assagais,           // spear
            Corsecas,
            MagariYaris,
            Pikes,
        };

        public static List<HashSet<string>> TwoHandedSwords = new List<HashSet<string>>()
        {
            Nodachis,           // sword
            Shashqas,
            Spadones,
        };

        public static HashSet<string> Combined = new HashSet<string>();

        static TwoHandedWeaponNames()
        {
            foreach (var list in AllTwoHandedWeapons)
            {
                foreach (var item in list)
                    Combined.Add(item);
            }
        }
    }
}
