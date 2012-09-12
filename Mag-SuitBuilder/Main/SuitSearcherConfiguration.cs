using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
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
