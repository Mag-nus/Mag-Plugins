using System;
using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	class EquipmentPiece
	{
		// Copper Chainmail Leggings, AL 607, Tinks 10, Epic Invulnerability, Wield Lvl 150, Melee Defense 390 to Activate, Diff 262
		// Gold Top, Tinks 2, Augmented Health III, Augmented Damage II, Major Storm Ward, Wield Lvl 150, Diff 410, Craft 9
		// Iron Amuli Coat, Defender's Set, AL 618, Tinks 10, Epic Strength, Wield Lvl 180, Melee Defense 300 to Activate, Diff 160

		public EquipmentPiece(string itemInfo)
		{
			string[] sections = itemInfo.Split(',');

			for (int i = 0 ; i < sections.Length ; i++)
				sections[i] = sections[i].Trim();

			if (sections.Length >= 1)
				Name = sections[0].Trim();

			EquipableSlots = Constants.GetEquippableSlots(Name);

			if (sections.Length >= 2 && sections[1].Contains(" Set"))
				ArmorSet = sections[1];

			foreach (string section in sections)
			{
				if (section.StartsWith("AL "))
				{
					int al;

					int.TryParse(section.Remove(0, 3), out al);

					ArmorLevel = al;

					break;
				}
			}

			foreach (string section in sections)
			{
				if (section.StartsWith("Tinks "))
				{
					int tinks;

					int.TryParse(section.Remove(0, 6), out tinks);

					Tinks = tinks;

					break;
				}
			}

			foreach (string section in sections)
			{
				if (section.Contains("Minor ") || section.Contains("Major ") || section.Contains("Epic "))
					Spells.Add(new Spell(section));
			}

			// Add Impen to the armor level
			foreach (Spell spell in Spells)
			{
				if (spell.Name.Contains("Impenetrability"))
				{
					if (spell.Level == SpellLevel.Minor) ArmorLevel += 20;
					if (spell.Level == SpellLevel.Major) ArmorLevel += 40;
					if (spell.Level == SpellLevel.Epic) ArmorLevel += 60;

					break;
				}
			}
		}

		public EquipmentPiece(Constants.EquippableSlotFlags equipableSlots, string armorSet, params string[] spellNames)
		{
			EquipableSlots = equipableSlots;

			ArmorSet = armorSet;

			foreach (string spellName in spellNames)
			{
				if (spellName.Contains("Minor ") || spellName.Contains("Major ") || spellName.Contains("Epic "))
					Spells.Add(new Spell(spellName));
			}
		}

		public readonly string Name;

		public readonly Constants.EquippableSlotFlags EquipableSlots;

		public readonly string ArmorSet;

		public readonly int ArmorLevel;

		public int PotentialTinkedArmorLevel
		{
			get
			{
				if (IsUnderwear || ArmorLevel == 0)
					return ArmorLevel;

				return ArmorLevel + (Math.Max(10 - Tinks, 0) * 20);
			}
		}

		private readonly int Tinks;

		public readonly Collection<Spell> Spells = new Collection<Spell>();

		public bool IsArmor
		{
			get
			{
				return EquipableSlots == Constants.EquippableSlotFlags.Head ||
					EquipableSlots == Constants.EquippableSlotFlags.Chest || 
					EquipableSlots == Constants.EquippableSlotFlags.UpperArms || 
					EquipableSlots == Constants.EquippableSlotFlags.LowerArms || 
					EquipableSlots == Constants.EquippableSlotFlags.Hands || 
					EquipableSlots == Constants.EquippableSlotFlags.Abdomen || 
					EquipableSlots == Constants.EquippableSlotFlags.UpperLegs || 
					EquipableSlots == Constants.EquippableSlotFlags.LowerLegs || 
					EquipableSlots == Constants.EquippableSlotFlags.Feet;
			}
		}

		public bool IsUnderwear
		{
			get
			{
				return EquipableSlots == Constants.EquippableSlotFlags.Shirt || EquipableSlots == Constants.EquippableSlotFlags.Pants;
			}
		}

		public int UnderwearCoverageSlots
		{
			get
			{
				if (!IsUnderwear)
					return 0;

				int underwearCoverage = (int)Constants.GetUnderwearCoverage(Name);

				int setBits = 0;

				while (underwearCoverage != 0)
				{
					if ((underwearCoverage & 1) == 1)
						setBits++;

					underwearCoverage >>= 1;
				}

				return setBits;
			}
		}

		public override string ToString()
		{
			string output = Name;

			if (ArmorLevel > 0)
				output += ", AL " + ArmorLevel;

			foreach (Spell spell in Spells)
				output += " " + spell;

			return output;
		}
	}
}
