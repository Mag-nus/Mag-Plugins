/*using System;
using System.Collections.Generic;

namespace Mag_SuitBuilder.Equipment
{
	class ArmorSet : IComparable
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

		/// <summary>
		/// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <returns>
		/// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>. 
		/// </returns>
		/// <param name="obj">An object to compare with this instance. </param><exception cref="T:System.ArgumentException"><paramref name="obj"/> is not the same type as this instance. </exception><filterpriority>2</filterpriority>
		public int CompareTo(object obj)
		{
			if (obj == null) return 1;

			ArmorSet otherObj = obj as ArmorSet;

			if (otherObj != null)
			{
				// NoArmorSet should come first
				if (this == NoArmorSet && obj != NoArmorSet) return -1;
				if (this != NoArmorSet && obj == NoArmorSet) return 1;
				if (this == NoArmorSet && obj == NoArmorSet) return 0;

				return name.CompareTo(otherObj.name);
			}

			throw new ArgumentException("Object is not an ArmorSet");
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
*/