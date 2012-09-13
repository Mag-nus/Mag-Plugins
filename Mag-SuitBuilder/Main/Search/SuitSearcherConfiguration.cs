using System.Collections.Generic;
using System.Collections.ObjectModel;

using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	class SuitSearcherConfiguration
	{
		public SuitSearcherConfiguration()
		{
			CantripsToLookFor = new Collection<Spell>();
		}

		/// <summary>
		/// This should be the minimum untinked, unbuffed (but including minor/major/epic impen) armor level.
		/// </summary>
		public int MinimumArmorLevelPerPiece { get; set; }

		public ICollection<Spell> CantripsToLookFor { get; set; }

		public string PrimaryArmorSet { get; set; }

		public string SecondaryArmorSet { get; set; }

		public bool OnlyAddPiecesWithArmor { get; set; }
	}
}
