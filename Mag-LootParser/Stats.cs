using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared;
using Mag.Shared.Constants;

using Mag_LootParser.ItemGroups;

namespace Mag_LootParser
{
    class Stats
    {
        public readonly string ContainerName;
        public readonly int Tier;

        public int TotalContainers;
        public int TotalItems;

        public readonly Dictionary<int, int> WorkmanshipHits = new Dictionary<int, int>();

        public readonly Dictionary<MaterialType, int> MaterialHits = new Dictionary<MaterialType, int>();

        public readonly ItemGroupStats MeleeWeapons = new ItemGroupStats();
        public readonly ItemGroupStats MissileWeapons = new ItemGroupStats();
        public readonly ItemGroupStats WandStaffOrbs = new ItemGroupStats();

        public readonly ItemGroupStats Shields = new ItemGroupStats();

        public readonly ItemGroupStats Armor = new ItemGroupStats();

        public readonly ItemGroupStats Underwear = new ItemGroupStats();

        public readonly ItemGroupStats Jewelry = new ItemGroupStats();

        public readonly ItemGroupStats ManaStones = new ItemGroupStats();
        public readonly ItemGroupStats HealingKits = new ItemGroupStats();
        public readonly ItemGroupStats Food = new ItemGroupStats();
        public readonly ItemGroupStats Lockpicks = new ItemGroupStats();
        public readonly GemStats Gems = new GemStats();

        // todo
        // Money
        // Misc
        // SpellComponent
        // Key
        // Scroll
        public readonly Dictionary<ObjectClass, ItemGroupStats> AllOthers = new Dictionary<ObjectClass, ItemGroupStats>();

        public Stats(string containerName, int tier)
        {
            ContainerName = containerName;
            Tier = tier;
        }

        public void ProcessItem(IdentResponse item)
        {
            TotalItems++;

            if (item.LongValues.ContainsKey(IntValueKey.Workmanship))
            {
                if (!WorkmanshipHits.ContainsKey(item.LongValues[IntValueKey.Workmanship]))
                    WorkmanshipHits[item.LongValues[IntValueKey.Workmanship]] = 0;

                WorkmanshipHits[item.LongValues[IntValueKey.Workmanship]]++;
            }

            if (item.LongValues.ContainsKey(IntValueKey.Material))
            {
                if (!MaterialHits.ContainsKey((MaterialType)item.LongValues[IntValueKey.Material]))
                    MaterialHits[(MaterialType)item.LongValues[IntValueKey.Material]] = 0;

                MaterialHits[(MaterialType)item.LongValues[IntValueKey.Material]]++;
            }

            if (item.ObjectClass == ObjectClass.MeleeWeapon)
                MeleeWeapons.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.MissileWeapon)
                MissileWeapons.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.WandStaffOrb)
                WandStaffOrbs.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.Armor)
                Armor.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.Clothing)
            {
                if (item.LongValues.ContainsKey(IntValueKey.Coverage))
                {
                    if (((CoverageFlags)item.LongValues[IntValueKey.Coverage]).IsUnderwear())
                        Underwear.ProcessItem(item);
                    else
                        Armor.ProcessItem(item);

                    // todo: this could still be a cloak
                }
            }
            else if (item.ObjectClass == ObjectClass.Jewelry)
                Jewelry.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.ManaStone)
                ManaStones.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.HealingKit)
                HealingKits.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.Food)
                Food.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.Lockpick)
                Lockpicks.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.Gem)
                Gems.ProcessItem(item);
            else
            {
                // These slipped through
                if (!AllOthers.ContainsKey(item.ObjectClass))
                    AllOthers[item.ObjectClass] = new ItemGroupStats();

                AllOthers[item.ObjectClass].ProcessItem(item);
            }
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Tier: " + Tier);
            sb.AppendLine("Total Containers: " + TotalContainers.ToString("N0"));
            sb.AppendLine("Total Items: " + TotalItems.ToString("N0"));
            sb.AppendLine();

            sb.AppendLine("Workmanship Hits: ");
            var totalWorkmanshipHits = WorkmanshipHits.Values.Sum();
            foreach (var kvp in WorkmanshipHits.OrderBy(i => i.Key))
            {
                sb.AppendLine(kvp.Key.ToString().PadLeft(2) + " [" + kvp.Value.ToString("N0").PadLeft(6) + " " + (kvp.Value / (float)totalWorkmanshipHits * 100).ToString("N1").PadLeft(4) + "%]");
            }
            sb.AppendLine();

            // This is really dependant on the type of object being dropped
            /*
            sb.AppendLine("Material Hits: ");
            var totalMaterialHits = MaterialHits.Values.Sum();
            foreach (var kvp in MaterialHits.OrderBy(i => i.Key))
            {
                sb.AppendLine(kvp.Key.ToString().PadRight(15) + " [" + kvp.Value.ToString("N0").PadLeft(6) + " " + (kvp.Value / (float)totalMaterialHits * 100).ToString("N1").PadLeft(4) + "%]");
            }
            sb.AppendLine();*/

            sb.AppendLine("Melee Weapons: ");
            sb.Append(MeleeWeapons);

            sb.AppendLine();
            sb.AppendLine("Missile Weapons: ");
            sb.Append(MissileWeapons);

            sb.AppendLine();
            sb.AppendLine("Wand Staff Orbs: ");
            sb.Append(WandStaffOrbs);

            sb.AppendLine();
            sb.AppendLine("Armor: ");
            sb.Append(Armor);

            sb.AppendLine();
            sb.AppendLine("Clothing (Underwear): ");
            sb.Append(Underwear);

            sb.AppendLine();
            sb.AppendLine("Jewelry: ");
            sb.Append(Jewelry);

            sb.AppendLine();
            sb.AppendLine("Gems: ");
            sb.Append(Gems);

            return sb.ToString();
        }
    }
}
