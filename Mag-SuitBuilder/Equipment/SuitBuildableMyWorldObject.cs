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
	}
}
