using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mag_LootParser.ItemGroups
{
    class GemStats : ItemGroupStats
    {
        public readonly Dictionary<int, int> BlueAtheriaCountsByLevel   = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };
        public readonly Dictionary<int, int> YellowAtheriaCountsByLevel = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };
        public readonly Dictionary<int, int> RedAtheriaCountsByLevel    = new Dictionary<int, int> { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 } };

        public override void ProcessItem(IdentResponse item)
        {
            base.ProcessItem(item);

            var type = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.Type);
            var iconOverlay = item.LongValues.FirstOrDefault(r => r.Key == Mag.Shared.Constants.IntValueKey.IconOverlay);

            if (type.Value == 42635) // Blue
                BlueAtheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
            else if (type.Value == 42637) // Yellow
                YellowAtheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
            else if (type.Value == 42636) // Red
                RedAtheriaCountsByLevel[iconOverlay.Value - 27700 + 1]++;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append(base.ToString());

            sb.AppendLine();

            sb.AppendLine($"Blue Aetheria Stats By Level: {BlueAtheriaCountsByLevel[1]} {BlueAtheriaCountsByLevel[2]} {BlueAtheriaCountsByLevel[3]} {BlueAtheriaCountsByLevel[4]} {BlueAtheriaCountsByLevel[5]}");
            sb.AppendLine($"Yellow Aetheria Stats By Level: {YellowAtheriaCountsByLevel[1]} {YellowAtheriaCountsByLevel[2]} {YellowAtheriaCountsByLevel[3]} {YellowAtheriaCountsByLevel[4]} {YellowAtheriaCountsByLevel[5]}");
            sb.AppendLine($"Red Aetheria Stats By Level: {RedAtheriaCountsByLevel[1]} {RedAtheriaCountsByLevel[2]} {RedAtheriaCountsByLevel[3]} {RedAtheriaCountsByLevel[4]} {RedAtheriaCountsByLevel[5]}");

            return sb.ToString();
        }
    }
}
