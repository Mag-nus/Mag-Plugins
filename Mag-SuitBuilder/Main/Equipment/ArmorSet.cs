using System.Collections.Generic;

namespace Mag_SuitBuilder.Equipment
{
	class ArmorSet
	{
		readonly string name;

		private ArmorSet(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}

		private static readonly Dictionary<string, ArmorSet> Armorsets = new Dictionary<string, ArmorSet>();

		static ArmorSet()
		{
			Armorsets.Add(NoArmorSet.name, NoArmorSet);
			Armorsets.Add(AnyArmorSet.name, AnyArmorSet);
		}

		public static ArmorSet GetArmorSet(string name)
		{
			if (!Armorsets.ContainsKey(name))
				Armorsets.Add(name, new ArmorSet(name));

			return Armorsets[name];
		}

		public static IEnumerable<ArmorSet> GetAllArmorSets()
		{
			return Armorsets.Values;
		}

		public static ArmorSet NoArmorSet = new ArmorSet("No Armor Set");
		public static ArmorSet AnyArmorSet = new ArmorSet("Any Armor Set");
	}
}
