using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

using Mag_SuitBuilder.Spells;

using Mag.Shared;

namespace Mag_SuitBuilder.Equipment
{
	public class SuitBuildableMyWorldObject : MyWorldObject
	{
		[XmlIgnore]
		public string Owner { get; set; }

		[XmlIgnore]
		public bool Locked { get; set; }

		[XmlIgnore]
		public bool Exclude { get; set; }

		[XmlIgnore]
		public ObjectClass ObjClass
		{
			get
			{
				return (ObjectClass)ObjectClass;
			}
		}

		[XmlIgnore]
		public EquippableSlotFlags EquippableSlots
		{
			get
			{
				return IntValues.ContainsKey(218103822) ? (EquippableSlotFlags)IntValues[218103822] : EquippableSlotFlags.None;
			}
		}

		[XmlIgnore]
		public CoverageFlags Coverage
		{
			get
			{
				return IntValues.ContainsKey(218103821) ? (CoverageFlags)IntValues[218103821] : CoverageFlags.None;
			}
		}

		[XmlIgnore]
		public EquippableSlotFlags EquippedSlot
		{
			get
			{
				return IntValues.ContainsKey(10) ? (EquippableSlotFlags)IntValues[10] : EquippableSlotFlags.None;
			}
		}

		private string itemSpells;

		[XmlIgnore]
		public string ItemSpells
		{
			get
			{
				if (itemSpells != null)
					return itemSpells;

				List<Spell> spellList = new List<Spell>();

				foreach (var spell in Spells)
				{
					if (Spell.IsAKnownSpell(spell))
						spellList.Add(Spell.GetSpell(spell));

				}
				List<Spell> spellsInOrder = new List<Spell>();

				for (Spell.CantripLevels level = Spell.CantripLevels.Legendary; level > Spell.CantripLevels.None; level--)
				{
					foreach (Spell spell in spellList)
					{
						if (spellsInOrder.Contains(spell))
							continue;

						if (spell.CantripLevel >= level)
							spellsInOrder.Add(spell);
					}
				}

				for (Spell.BuffLevels level = Spell.BuffLevels.VIII; level >= Spell.BuffLevels.None; level--)
				{
					foreach (Spell spell in spellList)
					{
						if (spellsInOrder.Contains(spell))
							continue;

						if (spell.BuffLevel >= level)
							spellsInOrder.Add(spell);
					}
				}

				foreach (var spell in spellsInOrder)
				{
					if (itemSpells != null) itemSpells += ", ";
					itemSpells += spell;
				}

				return itemSpells;
			}
		}


		private bool itemSearchCacheBuilt;
		public void BuiltItemSearchCache()
		{
			if (itemSearchCacheBuilt)
				return;

			ItemSetId = IntValues.ContainsKey(265) ? IntValues[265] : 0;

			foreach (var spellId in Spells)
			{
				Spell spell = Spell.GetSpell(spellId);
				cachedSpells.Add(spell);
			}

			itemSearchCacheBuilt = true;
		}

		[XmlIgnore]
		[Browsable(false)]
		public int ItemSetId;

		private readonly List<Spell> cachedSpells = new List<Spell>();

		[XmlIgnore]
		[Browsable(false)]
		public IList<Spell> CachedSpells { get { return cachedSpells; } }

		/// <summary>
		/// Is this item surpassed by something else in the group?
		/// </summary>
		[XmlIgnore]
		[Browsable(false)]
		public bool IsSurpassed { get; set; }



		[XmlIgnore]
		[Browsable(false)]
		public List<Spell> SpellsToUseInSearch = new List<Spell>();



		public bool IsSurpassedBy(SuitBuildableMyWorldObject compareItem)
		{
			if (compareItem.Exclude)
				return false;

			// Items must be of the same armor set
			if (compareItem.ItemSetId != ItemSetId)
				return false;

			// This checks to see that the compare item covers at least all the slots that the passed item does
			if (compareItem.Coverage.IsBodyArmor() && Coverage.IsBodyArmor())
			{
				if ((compareItem.Coverage & Coverage) != Coverage)
					return false;
			}
			else if ((compareItem.EquippableSlots & EquippableSlots) != EquippableSlots)
				return false;

			// Find the highest level spell on this item
			Spell.CantripLevels highestCantrip = Spell.CantripLevels.None;

			foreach (Spell itemSpell in cachedSpells)
			{
				if (itemSpell.CantripLevel > highestCantrip)
					highestCantrip = itemSpell.CantripLevel;
			}

			// Does this item have spells that equal or surpass this items at the highest cantrip level found?
			foreach (Spell itemSpell in cachedSpells)
			{
				if (itemSpell.CantripLevel < highestCantrip)
					continue;

				foreach (Spell compareSpell in compareItem.cachedSpells)
				{
					if (compareSpell.CantripLevel < highestCantrip)
						continue;

					if (compareSpell.IsSameOrSurpasses(itemSpell))
						goto next;
				}

				return false;

				next:;
			}

			/*// Does this item have a spell that the compare item does not?
			{
				bool itemHasSpellSurpasingPreviousItem = false;

				foreach (Spell itemSpell in CachedSpells)
				{
					if (itemSpell.CantripLevel < Spell.CantripLevels.Epic)
						return false;

					bool itemSpellSurpasesPreviousItemSpells = false;
					bool spellOfSameFamilyAndGroupFound = false;

					foreach (Spell previousSpell in compareItem.CachedSpells)
					{
						if (itemSpell.Surpasses(previousSpell))
						{
							itemSpellSurpasesPreviousItemSpells = true;
							spellOfSameFamilyAndGroupFound = true;
						}
						else if (itemSpell.IsOfSameFamilyAndGroup(previousSpell))
							spellOfSameFamilyAndGroupFound = true;
					}

					if (itemSpellSurpasesPreviousItemSpells || !spellOfSameFamilyAndGroupFound)
					{
						itemHasSpellSurpasingPreviousItem = true;
						break;
					}
				}

				if (itemHasSpellSurpasingPreviousItem)
					return false;
			}*/

			// If this item has higher AL, it's not surpassed
			if (compareItem.CalcedStartingArmorLevel < CalcedStartingArmorLevel)
				return false;

			if (compareItem.TotalRating < TotalRating)
				return false;
			if (Owner == "Mag-one" && Name == "Heavy Bracelet" && compareItem.Owner == "Mag-z bling" && compareItem.Name == "Heavy Bracelet" && compareItem.Id == -2104201881) System.Console.WriteLine("9: " + Id + " " + this + ", " + compareItem.Id + " " + compareItem);
			if (compareItem.DamRating <= DamRating && compareItem.DamResistRating <= DamResistRating &&
				compareItem.CritRating <= CritRating && compareItem.CritResistRating <= CritResistRating &&
				compareItem.CritDamRating <= CritDamRating && compareItem.CritDamResistRating <= CritDamResistRating &&
				compareItem.HealBoostRating <= HealBoostRating && compareItem.VitalityRating <= VitalityRating)
				return false;

			return true;
		}
	}
}
