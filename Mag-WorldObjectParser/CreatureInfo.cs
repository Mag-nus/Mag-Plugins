using System.Collections.Generic;

namespace Mag_WorldObjectParser
{
    class CreatureInfo : ExtendIDAttributeInfo
    {
        public int Count;

        /// <summary>
        /// This is a list of Landcells this creatureInfo was found at.
        /// I've found that creatures with the same name can have different attributes in different areas
        /// </summary>
        public List<int> Landcell = new List<int>();

        public string Name;

        public int Level;

        // Add ratings

        public bool Equals(CreatureInfo other)
        {
            if (Name != other.Name) return false;

            if (Level != other.Level) return false;

            if (healthMax != other.healthMax) return false;
            if (staminaMax != other.staminaMax) return false;
            if (manaMax != other.manaMax) return false;
            if (strength != other.strength) return false;
            if (endurance != other.endurance) return false;
            if (quickness != other.quickness) return false;
            if (coordination != other.coordination) return false;
            if (focus != other.focus) return false;
            if (self != other.self) return false;

            return true;
        }
    }
}
