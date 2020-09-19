using System;
using System.Collections.Generic;

namespace Mag_LootParser.ItemGroups.Helpers
{
   public static class LightWeaponNames
    {
        public static readonly HashSet<string> Dolabras = new HashSet<string>()
        {
            // light - axe
            "Dolabra",
            "Acid Dolabra",
            "Lightning Dolabra",
            "Flaming Dolabra",
            "Frost Dolabra",
        };

        public static readonly HashSet<string> HandAxes = new HashSet<string>()
        {
            // light - axe
            "Hand Axe",
            "Acid Hand Axe",
            "Lightning Hand Axe",
            "Flaming Hand Axe",
            "Frost Hand Axe",
        };

        public static readonly HashSet<string> Onos = new HashSet<string>()
        {
            // light - axe
            "Ono",
            "Acid Ono",
            "Lightning Ono",
            "Flaming Ono",
            "Frost Ono",
        };

        public static readonly HashSet<string> WarHammers = new HashSet<string>()
        {
            // light - axe
            "War Hammer",
            "Acid War Hammer",
            "Lightning War Hammer",
            "Flaming War Hammer",
            "Frost War Hammer",
        };

        public static readonly HashSet<string> Daggers = new HashSet<string>()
        {
            // light - dagger
            "Dagger",
            "Acid Dagger",
            "Lightning Dagger",
            "Flaming Dagger",
            "Frost Dagger",
        };

        public static readonly HashSet<string> Khanjars = new HashSet<string>()
        {
            // light - dagger
            "Khanjar",
            "Acid Khanjar",
            "Lightning Khanjar",
            "Flaming Khanjar",
            "Frost Khanjar",
        };

        public static readonly HashSet<string> Clubs = new HashSet<string>()
        {
            // light - mace
            "Club",
            "Acid Club",
            "Lightning Club",
            "Flaming Club",
            "Frost Club",
        };

        public static readonly HashSet<string> Kasrullahs = new HashSet<string>()
        {
            // light - mace
            "Kasrullah",
            "Acid Kasrullah",
            "Lightning Kasrullah",
            "Flaming Kasrullah",
            "Frost Kasrullah",
        };

        public static readonly HashSet<string> SpikedClubs = new HashSet<string>()
        {
            // light - mace
            "Spiked Club",
            "Frost Spiked Club",
            "Fire Spiked Club",
            "Acid Spiked Club",
            "Electric Spiked Club",
        };

        public static readonly HashSet<string> Spears = new HashSet<string>()
        {
            // light - spear
            "Spear",
            "Acid Spear",
            "Lightning Spear",
            "Flaming Spear",
            "Frost Spear",
        };

        public static readonly HashSet<string> Yaris = new HashSet<string>()
        {
            // light - spear
            "Yari",
            "Acid Yari",
            "Lightning Yari",
            "Flaming Yari",
            "Frost Yari",
        };

        public static readonly HashSet<string> QuarterStaffs = new HashSet<string>()
        {
            // light - staff
            "Quarter Staff",
            "Acid Quarter Staff",
            "Lightning Quarter Staff",
            "Flaming Quarter Staff",
            "Frost Quarter Staff",
        };

        public static readonly HashSet<string> BroadSwords = new HashSet<string>()
        {
            // light - sword
            "Broad Sword",
            "Acid Broad Sword",
            "Lightning Broad Sword",
            "Flaming Broad Sword",
            "Frost Broad Sword",
        };

        public static readonly HashSet<string> DericostBlades = new HashSet<string>()
        {
            // light - sword
            "Dericost Blade",
            "Frost Dericost Blade",
            "Acid Dericost Blade",
            "Lightning Dericost Blade",
            "Flaming Dericost Blade",
        };

        public static readonly HashSet<string> Epees = new HashSet<string>()
        {
            // light - sword
            "Epee",
            "Acid Epee",
            "Lightning Epee",
            "Flaming Epee",
            "Frost Epee",
        };

        public static readonly HashSet<string> Kaskaras = new HashSet<string>()
        {
            // light - sword
            "Kaskara",
            "Acid Kaskara",
            "Lightning Kaskara",
            "Flaming Kaskara",
            "Frost Kaskara",
        };

        public static readonly HashSet<string> Shamshirs = new HashSet<string>()
        {
            // light - sword
            "Shamshir",
            "Acid Shamshir",
            "Lightning Shamshir",
            "Flaming Shamshir",
            "Frost Shamshir",
        };

        public static readonly HashSet<string> Spadas = new HashSet<string>()
        {
            // light - sword
            "Spada",
            "Lightning Spada",
            "Frost Spada",
            "Flaming Spada",
            "Acid Spada",
        };

        public static readonly HashSet<string> Katars = new HashSet<string>()
        {
            // light - unarmed
            "Katar",
            "Acid Katar",
            "Lightning Katar",
            "Flaming Katar",
            "Frost Katar",
        };

        public static readonly HashSet<string> Knuckles = new HashSet<string>()
        {
            // light - unarmed
            "Knuckles",
            "Lightning Knuckles",
            "Flaming Knuckles",
            "Frost Knuckles",
            "Acid Knuckles",
        };

        public static List<string> Names = new List<string>()
        {
            "Dolabras",
            "HandAxes",
            "Onos",
            "WarHammers",
            "Daggers",
            "Khanjars",
            "Clubs",
            "Kasrullahs",
            "SpikedClubs",
            "Spears",
            "Yaris",
            "QuarterStaffs",
            "BroadSwords",
            "DericostBlades",
            "Epees",
            "Kaskaras",
            "Shamshirs",
            "Spadas",
            "Katars",
            "Knuckles",
        };


        public static List<HashSet<string>> AllLightWeapons = new List<HashSet<string>>()
        {
            Dolabras,       // axe
            HandAxes,
            Onos,
            WarHammers,
            Daggers,        // dagger
            Khanjars,
            Clubs,          // mace
            Kasrullahs,
            SpikedClubs,
            Spears,         // spear
            Yaris,
            QuarterStaffs,  // staff
            BroadSwords,    // sword
            DericostBlades,
            Epees,
            Kaskaras,
            Shamshirs,
            Spadas,
            Katars,         // unarmed
            Knuckles,
        };

        public static List<HashSet<string>> LightAxes = new List<HashSet<string>>()
        {
            Dolabras,       // axe
            HandAxes,
            Onos,
            WarHammers,
        };

        public static List<HashSet<string>> LightDaggers = new List<HashSet<string>>()
        {
            Daggers,        // dagger
            Khanjars,
        };

        public static List<HashSet<string>> LightDaggers_SingleStrike = new List<HashSet<string>>()
        {
            Khanjars,       // dagger - single strike
        };

        public static List<HashSet<string>> LightDaggers_MultiStrike = new List<HashSet<string>>()
        {
            Daggers,        // dagger - multi strike
        };

        public static List<HashSet<string>> LightMaces = new List<HashSet<string>>()
        {
            Clubs,          // mace
            Kasrullahs,
            SpikedClubs,
        };

        public static List<HashSet<string>> LightSpears = new List<HashSet<string>>()
        {
            Spears,         // spear
            Yaris,
        };

        public static List<HashSet<string>> LightStaffs = new List<HashSet<string>>()
        {
            QuarterStaffs,  // staff
        };

        public static List<HashSet<string>> LightSwords = new List<HashSet<string>>()
        {
            BroadSwords,    // sword
            DericostBlades,
            Epees,
            Kaskaras,
            Shamshirs,
            Spadas,
        };

        public static List<HashSet<string>> LightSwords_SingleStrike = new List<HashSet<string>>()
        {
            BroadSwords,    // sword - single strike
            DericostBlades,
            Kaskaras,
            Shamshirs,
            Spadas,
        };

        public static List<HashSet<string>> LightSwords_MultiStrike = new List<HashSet<string>>()
        {
            Epees,          // sword - multi strike
        };

        public static List<HashSet<string>> LightUnarmed = new List<HashSet<string>>()
        {
            Katars,         // unarmed
            Knuckles,
        };

        public static HashSet<string> Combined = new HashSet<string>();

        static LightWeaponNames()
        {
            foreach (var list in AllLightWeapons)
            {
                foreach (var item in list)
                    Combined.Add(item);
            }
        }
    }
}
