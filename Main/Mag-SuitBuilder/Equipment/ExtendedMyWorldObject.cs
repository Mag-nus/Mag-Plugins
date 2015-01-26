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
	public class ExtendedMyWorldObject : MyWorldObject
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

		[XmlIgnore]
		[Browsable(false)]
		public int ItemSetId;

		private readonly List<Spell> cachedSpells = new List<Spell>();

		[XmlIgnore]
		[Browsable(false)]
		public IList<Spell> CachedSpells { get { return cachedSpells; } }

		/// <summary>
		/// This will init ItemSetID and CachedSpells.
		/// </summary>
		public void BuiltItemSearchCache()
		{
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
		}
	}
}
