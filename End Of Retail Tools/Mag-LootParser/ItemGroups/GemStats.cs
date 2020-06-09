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

            sb.AppendLine($"Blue Aetheria Stats By Level: {BlueAetheriaCountsByLevel[1]} {BlueAetheriaCountsByLevel[2]} {BlueAetheriaCountsByLevel[3]} {BlueAetheriaCountsByLevel[4]} {BlueAetheriaCountsByLevel[5]}");
            sb.AppendLine($"Yellow Aetheria Stats By Level: {YellowAetheriaCountsByLevel[1]} {YellowAetheriaCountsByLevel[2]} {YellowAetheriaCountsByLevel[3]} {YellowAetheriaCountsByLevel[4]} {YellowAetheriaCountsByLevel[5]}");
            sb.AppendLine($"Red Aetheria Stats By Level: {RedAetheriaCountsByLevel[1]} {RedAetheriaCountsByLevel[2]} {RedAetheriaCountsByLevel[3]} {RedAetheriaCountsByLevel[4]} {RedAetheriaCountsByLevel[5]}");

            return sb.ToString();
        }
    }
}
