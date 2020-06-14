using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mag.Shared.Constants;

namespace Mag_LootParser.ItemGroups
{
	class ClothingStats : WieldableStats
	{
		public readonly Dictionary<int, int> CloakCountsByLevel = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

		public ClothingStats(bool limitStatsToOnlyItemsWithWorkmanship = false) : base(limitStatsToOnlyItemsWithWorkmanship)
		{
		}

		public override void ProcessItem(IdentResponse item)
		{
			base.ProcessItem(item);

			var equipableSlots = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.EquipableSlots_Decal);
			var iconOverlay = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.IconOverlay_Decal_DID);

			if ((EquipMask)equipableSlots.Value == EquipMask.Cloak)
				CloakCountsByLevel[iconOverlay.Value - 27700 + 1]++;
		}


		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append(base.ToString());

			sb.AppendLine();

			{
				var totalHits = CloakCountsByLevel.Values.Sum();
				for (int i = 1; i <= 5; i++)
					sb.AppendLine($"Cloak Hits By Level {i} [" + CloakCountsByLevel[i].ToString().PadRight(4) + " " + (CloakCountsByLevel[i] / (float) totalHits * 100).ToString("N1").PadLeft(4) + " %]");
			}

			return sb.ToString();
		}
	}
}
