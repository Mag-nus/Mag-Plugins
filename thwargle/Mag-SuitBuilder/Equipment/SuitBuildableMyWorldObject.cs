using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

using Mag.Shared;
using Mag.Shared.Constants;
using Mag.Shared.Spells;

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
					if (SpellTools.IsAKnownSpell(spell))
						spellList.Add(SpellTools.GetSpell(spell));

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
				try
				{
					Spell spell = SpellTools.GetSpell(spellId);
					cachedSpells.Add(spell);
				}
				catch (ArgumentException)
				{
					MessageBox.Show("Unable to cache spell id: " + spellId + " on item: " + Name + ". Spell ID not found in the master table.");
				}
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

		[XmlIgnore]
		[Browsable(false)]
		public List<Spell> SpellsToUseInSearch = new List<Spell>();


		readonly Dictionary<SuitBuildableMyWorldObject, bool> surpassedCache = new Dictionary<SuitBuildableMyWorldObject, bool>();

		public bool IsSurpassedBy(SuitBuildableMyWorldObject compareItem)
		{
			if (compareItem.Exclude)
				return false;

			if (surpassedCache.ContainsKey(compareItem))
				return surpassedCache[compareItem];

			BuiltItemSearchCache();
			compareItem.BuiltItemSearchCache();

			// Items must be of the same armor set
			if (compareItem.ItemSetId != ItemSetId)
			{
				surpassedCache.Add(compareItem, false);
				return false;
			}

			// This checks to see that the compare item covers at least all the slots that the passed item does
			if (compareItem.Coverage.IsBodyArmor() && Coverage.IsBodyArmor())
			{
				if ((compareItem.Coverage & Coverage) != Coverage)
				{
					surpassedCache.Add(compareItem, false);
					return false;
				}
			}
			else if ((compareItem.EquippableSlots & EquippableSlots) != EquippableSlots)
			{
				surpassedCache.Add(compareItem, false);
				return false;
			}

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
					if (compareSpell.Surpasses(itemSpell))
					{
						surpassedCache.Add(compareItem, true);
						return true;
					}

					if (compareSpell.IsSameOrSurpasses(itemSpell))
						goto next;
				}

				surpassedCache.Add(compareItem, false);
				return false;

				next:;
			}

			if (compareItem.CalcedStartingArmorLevel > CalcedStartingArmorLevel)
			{
				surpassedCache.Add(compareItem, true);
				return true;
			}

			if (compareItem.DamRating > DamRating && DamRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.DamResistRating > DamResistRating && DamResistRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.CritRating > CritRating && CritRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.CritResistRating > CritResistRating && CritResistRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.CritDamRating > CritDamRating && CritDamRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.CritDamResistRating > CritDamResistRating && CritDamResistRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.HealBoostRating > HealBoostRating && HealBoostRating > 0) { surpassedCache.Add(compareItem, true); return true; }
			if (compareItem.VitalityRating > VitalityRating && VitalityRating > 0) { surpassedCache.Add(compareItem, true); return true; }

			surpassedCache.Add(compareItem, false);
			return false;
		}
	}
}
