using System.Collections.Generic;

using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Equipment
{
	class EquipmentPiece
	{
		private EquipmentPiece()
		{
			ArmorSet = ArmorSet.NoArmorSet;

			Spells = new List<Spell>();
		}

		// Copper Chainmail Leggings, AL 607, Tinks 10, Epic Invulnerability, Wield Lvl 150, Melee Defense 390 to Activate, Diff 262
		// Gold Top, Tinks 2, Augmented Health III, Augmented Damage II, Major Storm Ward, Wield Lvl 150, Diff 410, Craft 9
		// Iron Amuli Coat, Defender's Set, AL 618, Tinks 10, Epic Strength, Wield Lvl 180, Melee Defense 300 to Activate, Diff 160
		public EquipmentPiece(string itemInfo) : this()
		{
			string[] sections = itemInfo.Split(',');

			// Trim off all white spaces from each section
			for (int i = 0 ; i < sections.Length ; i++)
				sections[i] = sections[i].Trim();

			// We expect the name to be the first item
			if (sections.Length >= 1)
				Name = sections[0];

			// Pick out the Armor Set
			foreach (string section in sections)
			{
				if (section.Contains(" Set"))
					ArmorSet = ArmorSet.GetArmorSet(section);
			}

			// Find the AL
			foreach (string section in sections)
			{
				if (section.StartsWith("AL "))
				{
					int al;
					int.TryParse(section.Remove(0, 3), out al);
					ArmorLevel = al;
				}
			}

			// Find the number of tinks
			foreach (string section in sections)
			{
				if (section.StartsWith("Tinks "))
				{
					int tinks;
					int.TryParse(section.Remove(0, 6), out tinks);
					Tinks = tinks;
				}
			}

			// Find out if the piece has been imbued
			foreach (string section in sections)
			{
				if (section == "CS" || section == "CB" || section == "AR" || section.EndsWith("Rend") || section.EndsWith("Imbue") || section == "Hematited" || section.EndsWith("Absorb"))
					Imbued = true;
			}

			// Add the spells
			foreach (string section in sections)
			{
				if (Spell.IsAKnownSpell(section))
					Spells.Add(Spell.GetSpell(section));
			}

			// Determine our base armor level
			if (ArmorLevel == 0 || (EquipableSlots & Constants.EquippableSlotFlags.AllBodyArmor) == 0)
				BaseArmorLevel = 0;
			else
			{
				int armorFromTinks = 0;

				if (Tinks > 0)
					armorFromTinks = Imbued ? (Tinks - 1) * 20 : Tinks * 20;

				BaseArmorLevel = ArmorLevel - armorFromTinks;

				// Lets try to determine if this item has been buffed. If so, lets just assume its been buffed with Impen 8 and subtract 240 from the piece.
				// According to this page http://ac.wikkii.net/wiki/Loot#Armor, the highest piece of enchantable armor is 314
				if (BaseArmorLevel > 314)
					BaseArmorLevel -= 240;
			}

			// Add Impen to the base armor level
			foreach (Spell spell in Spells)
			{
				if (spell.Name.Contains("Impenetrability"))
				{
					if (spell.CantripLevel == Spell.CantripLevels.Minor) BaseArmorLevel += 20;
					if (spell.CantripLevel == Spell.CantripLevels.Major) BaseArmorLevel += 40;
					if (spell.CantripLevel == Spell.CantripLevels.Epic) BaseArmorLevel += 60;

					break;
				}
			}
		}

		public bool Locked { get; set; }

		private string _name;
		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;

				EquipableSlots = Constants.GetEquippableSlots(Name);
			}
		}

		private Constants.EquippableSlotFlags _equipableSlots;
		public Constants.EquippableSlotFlags EquipableSlots
		{
			get { return _equipableSlots; }
			private set
			{
				_equipableSlots = value;

				// Determine how many protectable body slots this piece covers
				BodyPartsCovered = 0;
				int slotsCovered = (int)EquipableSlots;

				if (Constants.IsUnderwear(Name))
					slotsCovered = (int)Constants.GetUnderwearCoverage(Name);

				while (slotsCovered != 0)
				{
					if ((slotsCovered & 1) == 1)
						BodyPartsCovered++;
					slotsCovered >>= 1;
				}

				// If this is jewelry, it doesn't cover any body parts.
				if (EquipableSlots >= Constants.EquippableSlotFlags.Necklace && EquipableSlots <= Constants.EquippableSlotFlags.Trinket)
					BodyPartsCovered = 0;
			}
		}

		/// <summary>
		/// This represents the number of protectable (by armor) body slots
		/// </summary>
		public int BodyPartsCovered { get; private set; }

		public ArmorSet ArmorSet { get; set; }

		/// <summary>
		/// This is the actual armor level of the piece as it was described
		/// </summary>
		public int ArmorLevel { get; private set; }

		public int Tinks { get; private set; }

		public bool Imbued { get; private set; }

		public List<Spell> Spells { get; private set; }

		/// <summary>
		/// This is the calculated base armor level of the piece, before tinks but including minor/major/epic Impen
		/// </summary>
		public int BaseArmorLevel { get; set; }

		public string Spell1 { get { return Spells.Count > 0 && Spells[0] != null ? Spells[0].ToString() : null; } set { SetSpellAtIndex(0, value); } }
		public string Spell2 { get { return Spells.Count > 1 && Spells[1] != null ? Spells[1].ToString() : null; } set { SetSpellAtIndex(1, value); } }
		public string Spell3 { get { return Spells.Count > 2 && Spells[2] != null ? Spells[2].ToString() : null; } set { SetSpellAtIndex(2, value); } }
		public string Spell4 { get { return Spells.Count > 3 && Spells[3] != null ? Spells[3].ToString() : null; } set { SetSpellAtIndex(3, value); } }
		public string Spell5 { get { return Spells.Count > 4 && Spells[4] != null ? Spells[4].ToString() : null; } set { SetSpellAtIndex(4, value); } }
		public string Spell6 { get { return Spells.Count > 5 && Spells[5] != null ? Spells[5].ToString() : null; } set { SetSpellAtIndex(5, value); } }
		public string Spell7 { get { return Spells.Count > 6 && Spells[6] != null ? Spells[6].ToString() : null; } set { SetSpellAtIndex(6, value); } }
		public string Spell8 { get { return Spells.Count > 7 && Spells[7] != null ? Spells[7].ToString() : null; } set { SetSpellAtIndex(7, value); } }
		public string Spell9 { get { return Spells.Count > 8 && Spells[8] != null ? Spells[8].ToString() : null; } set { SetSpellAtIndex(8, value); } }

		private void SetSpellAtIndex(int index, string text)
		{
			if (!Spell.IsAKnownSpell(text))
				return;

			if (Spells.Count > index)
				Spells[index] = Spell.GetSpell(text);
			else
				Spells.Add(Spell.GetSpell(text));
		}

		public override string ToString()
		{
			string output = Name;

			if (ArmorLevel > 0)
				output += ", AL " + ArmorLevel;

			foreach (Spell spell in Spells)
				output += ", " + spell;

			return output;
		}
	}
}
