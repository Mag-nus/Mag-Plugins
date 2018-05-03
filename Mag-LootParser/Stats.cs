using System.Text;

using Mag.Shared;

namespace Mag_LootParser
{
    class Stats
    {
        public readonly ItemGroupStats MeleeWeapons = new ItemGroupStats();
        public readonly ItemGroupStats MissileWeapons = new ItemGroupStats();
        public readonly ItemGroupStats WandStaffOrbs = new ItemGroupStats();

        public readonly ItemGroupStats Armor = new ItemGroupStats();
        public readonly ItemGroupStats Clothing = new ItemGroupStats();

        public readonly ItemGroupStats Jewelry = new ItemGroupStats();


        // todo
        // Food
        // Money
        // Misc
        // Gem
        // SpellComponent
        // Key
        // ManaStone
        // Healing Kit
        // Lockpick
        // Scroll


        public void ProcessItem(IdentResponse item)
        {
            if (item.ObjectClass == ObjectClass.MeleeWeapon)
                MeleeWeapons.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.MissileWeapon)
                MissileWeapons.ProcessItem(item);
            else if (item.ObjectClass == ObjectClass.WandStaffOrb)
                WandStaffOrbs.ProcessItem(item);
            else
                ; // catch here to see if we're not processing an object class
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

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
            sb.AppendLine("Clothing: ");
            sb.Append(Clothing);

            sb.AppendLine();
            sb.AppendLine("Jewelry: ");
            sb.Append(Jewelry);

            return sb.ToString();
        }
    }
}
