using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mag_LootParser.ItemGroups
{
    class GemStats : ItemGroupStats
    {
        public readonly Dictionary<int, int> BlueAetheriaCountsByLevel   = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };
        public readonly Dictionary<int, int> YellowAetheriaCountsByLevel = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };
        public readonly Dictionary<int, int> RedAetheriaCountsByLevel    = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

        public override void ProcessItem(IdentResponse item)
        {
            base.ProcessItem(item);

            var type = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.WeenieClassId_Decal);
            var iconOverlay = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.IconOverlay_Decal_DID);

            if (type.Value == 42635) // Blue
                BlueAetheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
            else if (type.Value == 42637) // Yellow
                YellowAetheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
            else if (type.Value == 42636) // Red
                RedAetheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(base.ToString());

            sb.AppendLine();

            {
	            var totalHits = BlueAetheriaCountsByLevel.Values.Sum();
	            for (int i = 1; i <= 5; i++)
					sb.AppendLine($"Blue   Aetheria Hits By Level {i} [" + BlueAetheriaCountsByLevel[i].ToString().PadRight(4) + " " + (BlueAetheriaCountsByLevel[i] / (float)totalHits * 100).ToString("N1").PadLeft(4) + " %]");
			}

            {
	            var totalHits = YellowAetheriaCountsByLevel.Values.Sum();
				for (int i = 1; i <= 5; i++)
					sb.AppendLine($"Yellow Aetheria Hits By Level {i} [" + YellowAetheriaCountsByLevel[i].ToString().PadRight(4) + " " + (YellowAetheriaCountsByLevel[i] / (float)totalHits * 100).ToString("N1").PadLeft(4) + " %]");
			}

            {
	            var totalHits = RedAetheriaCountsByLevel.Values.Sum();
	            for (int i = 1; i <= 5; i++)
		            sb.AppendLine($"Red    Aetheria Hits By Level {i} [" + RedAetheriaCountsByLevel[i].ToString().PadRight(4) + " " + (RedAetheriaCountsByLevel[i] / (float)totalHits * 100).ToString("N1").PadLeft(4) + " %]");
			}

            return sb.ToString();
        }
    }
}
