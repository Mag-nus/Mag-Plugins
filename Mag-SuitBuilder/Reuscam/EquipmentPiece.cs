using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	class EquipmentPiece : IEquipmentPiece
	{
		// Copper Chainmail Leggings, AL 607, Tinks 10, Epic Invulnerability, Wield Lvl 150, Melee Defense 390 to Activate, Diff 262
		// Gold Top, Tinks 2, Augmented Health III, Augmented Damage II, Major Storm Ward, Wield Lvl 150, Diff 410, Craft 9
		// Iron Amuli Coat, Defender's Set, AL 618, Tinks 10, Epic Strength, Wield Lvl 180, Melee Defense 300 to Activate, Diff 160
        
		public EquipmentPiece(string itemInfo, System.Collections.Generic.Dictionary<string, bool> enabled) //todo:: turn this into a map, second param
		{

			string[] sections = itemInfo.Split(',');

			for (int i = 0 ; i < sections.Length ; i++)
				sections[i] = sections[i].Trim();

			if (sections.Length >= 1)
				Name = sections[0].Trim();

			EquipableSlots = Constants.GetEquippableSlots(Name);

			int value = (int)EquipableSlots;
			while (value != 0)
			{
				if ((value & 1) == 1)
					NumberOfSlotsCovered++;
				value >>= 1;
			}

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

			Collection<Spell> spells = new Collection<Spell>();

			// This could use a better way to add spells
			foreach (string section in sections)
			{
                if (section.Contains("Minor ") || section.Contains("Major ") || section.Contains("Epic "))
                {
                    //Want to filter out the item if its strikeout on the grid.  probably a better way to do this too besides referencing the control, 
                    //optimize lookup later, make it go now!
                    //todo:: optimize 
                    //stolen from spell.cs.  need to parse out the spell name, prior to creating a spell, to ignore it if needed.
                    string nameWithoutLevel = section.Substring(section.IndexOf(' ') + 1, section.Length - section.IndexOf(' ') - 1);
                    try
                    {                                           
                        if (enabled[nameWithoutLevel] != false)
                        {
                            spells.Add(new Spell(section));
                        } 
                    }
                    catch (System.Exception ex){ 
                        /*(carry on mr parser! */
                        System.Diagnostics.Debug.WriteLine("Not found:" + nameWithoutLevel);
                        
                    }
                }
			}

			Spells = new ReadOnlyCollection<Spell>(spells);

			// Add Impen to the armor level
			foreach (Spell spell in spells)
			{
				if (spell.Name.Contains("Impenetrability"))
				{
					if (spell.IsMinor) ArmorLevel += 20;
					if (spell.IsMajor) ArmorLevel += 40;
					if (spell.IsEpic) ArmorLevel += 60;

					break;
				}
			}
		}

		public EquipmentPiece(Constants.EquippableSlotFlags equipableSlots, string armorSet, params string[] spellNames)
		{
			EquipableSlots = equipableSlots;

			int value = (int)EquipableSlots;
			while (value != 0)
			{
				if ((value & 1) == 1)
					NumberOfSlotsCovered++;
				value >>= 1;
			}

			ArmorSet = armorSet;

			Collection<Spell> spells = new Collection<Spell>();

			foreach (string spellName in spellNames)
			{
				spells.Add(new Spell(spellName));
			}

			Spells = new ReadOnlyCollection<Spell>(spells);
		}


		public string Name { get; private set; }

		public Constants.EquippableSlotFlags EquipableSlots { get; private set; }

		public int NumberOfSlotsCovered { get; private set; }

		public string ArmorSet { get; private set; }

		public int ArmorLevel { get; private set; }

		public int Tinks { get; private set; }

		public ReadOnlyCollection<Spell> Spells { get; private set; }

		/*
		private int PotentialTinkedArmorLevel
		{
			get
			{
				if (IsUnderwear || ArmorLevel == 0)
					return ArmorLevel;

				return ArmorLevel + (Math.Max(10 - Tinks, 0) * 20);
			}
		}

		private bool IsArmor
		{
			get
			{
				return (EquipableSlots & Constants.EquippableSlotFlags.Head) != 0 ||
					(EquipableSlots & Constants.EquippableSlotFlags.Chest) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.UpperArms) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.LowerArms) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.Hands) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.Abdomen) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.UpperLegs) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.LowerLegs) != 0 || 
					(EquipableSlots & Constants.EquippableSlotFlags.Feet) != 0;
			}
		}

		private bool IsUnderwear
		{
			get
			{
				return (EquipableSlots & Constants.EquippableSlotFlags.Shirt) != 0 || (EquipableSlots & Constants.EquippableSlotFlags.Pants) != 0;
			}
		}

		private int UnderwearCoverageSlots
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
		*/
		public override string ToString()
		{
			string output = Name;

			if (ArmorLevel > 0)
				output += " AL " + ArmorLevel;

			foreach (Spell spell in Spells)
				output += " " + spell;

			return output;
		}
	}
}
