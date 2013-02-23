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
		public Constants.EquippableSlotFlags EquippableSlot
		{
			get
			{
				return LongValues.ContainsKey(218103822) ? (Constants.EquippableSlotFlags)LongValues[218103822] : Constants.EquippableSlotFlags.None;
			}
		}

		[XmlIgnore]
		public Constants.CoverageFlags Coverage
		{
			get
			{
				return LongValues.ContainsKey(218103821) ? (Constants.CoverageFlags)LongValues[218103821] : Constants.CoverageFlags.None;
			}
		}

		[XmlIgnore]
		public Constants.EquippableSlotFlags EquippedSlot
		{
			get
			{
				return LongValues.ContainsKey(10) ? (Constants.EquippableSlotFlags)LongValues[10] : Constants.EquippableSlotFlags.None;
			}
		}

		[Browsable(false)]
		public long ItemSetId { get { return LongValues.ContainsKey(265) ? LongValues[265] : -1; } }

		private readonly List<Spell> cachedSpells = new List<Spell>();
		private bool buildSpellCache;
		public void BuildSpellCache()
		{
			if (buildSpellCache)
				return;
			foreach (var spellId in Spells)
			{
				Spell spell = Spell.GetSpell(spellId);
				cachedSpells.Add(spell);
			}
			buildSpellCache = true;
		}
		[Browsable(false)]
		public IEnumerable<Spell> CachedSpells { get { return cachedSpells; } }
	}
}
