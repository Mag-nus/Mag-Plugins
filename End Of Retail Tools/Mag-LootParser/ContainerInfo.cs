using System;
using System.Collections.Generic;
using System.Linq;

namespace Mag_LootParser
{
    class ContainerInfo
    {
        public uint Id;

        public string Name;

        public int Landcell;

        public string Location;

        public readonly List<IdentResponse> Items = new List<IdentResponse>();

        private int? tier;

        /// <summary>
        /// This is the tier calculated based on Items.
        /// It may not be the actual tier for the parent creature.
        ///  -1 = Player Corpse
        ///   0 = Not Identified
        /// 1-8 = Retail Tier
        /// </summary>
        public int Tier
        {
            get
            {
                if (!tier.HasValue)
                    tier = TierCalculator.Calculate(Items);

                return tier.Value;
            }
        }

        public static bool IsChest(string name)
        {
	        if (name.StartsWith("Corpse of")) return false;
	        if (name.StartsWith("Treasure of")) return false;

	        if (name.EndsWith("Chest")) return true;
	        if (name.EndsWith("Vault")) return true;
	        if (name.EndsWith("Reliquary")) return true;

			// This is ACE specific
			if (name.StartsWith("DID ")) return false;

	        throw new Exception($"Unable to determine if container is chest: {name}");
		}
    }

    static class ContainerInfoExtensions
    {
		/// <summary>
		/// Make sure you have called UpdateVariablesAfterAllItmesHaveBeenAdded() on all the containerInfos first
		/// Will return -1 for player, 0 if a tier was unable to be calculated, or the tier #.
		/// </summary>
		public static int GetTier(this ICollection<ContainerInfo> containerInfos)
        {
            int tier = 0;

            foreach (var containerInfo in containerInfos)
            {
                if (containerInfo.Tier == -1)
                    return -1;

                if (containerInfo.Tier > tier)
                    tier = containerInfo.Tier;
            }

            return tier;
        }

        public static Dictionary<int, List<ContainerInfo>> GroupContainerInfosByTier(this ICollection<ContainerInfo> containerInfos)
        {
            var results = new Dictionary<int, List<ContainerInfo>>();

            var name = containerInfos.First().Name;

			// Test Code
			/*var sb = new System.Text.StringBuilder();

				var sorted = containerInfos.OrderBy(r => r.Tier).ToList();
				foreach (var sort in sorted)
					sb.AppendLine(sort.Tier + " " + sort.Landcell.ToString("X8") + " " + sort.Location);

				sb.AppendLine();

				var landcells = containerInfos.OrderBy(r => r.Landcell).Select(r => (r.Landcell & 0xFFFF0000)).Distinct();
				foreach (var landcell in landcells)
				{
					var hits = new Dictionary<int, int>();

					foreach (var containerInfo in containerInfos)
					{
						if ((containerInfo.Landcell & 0xFFFF0000) == landcell)
						{
							if (!hits.ContainsKey(containerInfo.Tier))
								hits[containerInfo.Tier] = 0;

							hits[containerInfo.Tier]++;
						}
					}

					foreach (var hit in hits)
						sb.AppendLine(landcell.ToString("X8") + ", tier: " + hit.Key + ", hits: " + hit.Value);

					sb.AppendLine();
				}

				System.Windows.Forms.Clipboard.SetText(sb.ToString());*/

			if (name == "Corpse of Zombie")
            {
	            results[1] = new List<ContainerInfo>(); // Zombie (Level 15)
	            results[2] = new List<ContainerInfo>(); // ??
	            results[3] = new List<ContainerInfo>(); // Zombie (Level 50)

	            foreach (var containerInfo in containerInfos)
	            {
		            if (containerInfo.Tier >= 3)
			            results[3].Add(containerInfo);
		            else if (containerInfo.Tier >= 2)
			            results[2].Add(containerInfo);
		            else
			            results[1].Add(containerInfo);
	            }
            }
            else if (name == "Corpse of Tumerok Major")
            {
	            results[2] = new List<ContainerInfo>(); // Tumerok Major (50)
	            results[3] = new List<ContainerInfo>(); // Tumerok Major (80)

	            foreach (var containerInfo in containerInfos)
	            {
		            if (containerInfo.Tier >= 3)
			            results[3].Add(containerInfo);
		            else
			            results[2].Add(containerInfo);
	            }
            }
            else if (name == "Corpse of Olthoi Larvae")
            {
	            // todo these come in different tiers, loot tiers seen: 0,2,3,4,5,6
            }
			else if (name == "Corpse of Falatacot Consort")
            {
	            // todo these come in different tiers, loot tiers seen: 2,3,4,5,6,7
            }
            else if (name == "Corpse of Mercenary")
            {
	            results[4] = new List<ContainerInfo>(); // Mercenary (Level 80)
	            results[5] = new List<ContainerInfo>(); // Mercenary (Level 115)

	            foreach (var containerInfo in containerInfos)
	            {
		            if (containerInfo.Landcell >> 16 == 0x004A)
			            results[5].Add(containerInfo);
		            else
			            results[4].Add(containerInfo);
	            }
            }
			else if (name == "Corpse of Panumbris Shadow")
            {
                results[5] = new List<ContainerInfo>(); // Panumbris Shadow (80)
                results[8] = new List<ContainerInfo>(); // Panumbris Shadow (240)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 6)
                        results[8].Add(containerInfo);
                    else
                        results[5].Add(containerInfo);
                }
            }
            else if (name == "Corpse of Raven Hunter")
            {
                results[5] = new List<ContainerInfo>(); // Raven Hunter (Level 135)
                results[6] = new List<ContainerInfo>(); // Raven Hunter (Level 160)

                foreach (var containerInfo in containerInfos)
                {
                    if (containerInfo.Tier >= 6)
                        results[6].Add(containerInfo);
                    else
                        results[5].Add(containerInfo);
                }
            }
			else if (name == "Corpse of Bandit Mana Hunter")
            {
	            // todo has tier 5, 7
            }
            else if (name == "Corpse of Ashris Niffis")
            {
	            // todo has tier 5, 7
            }
            else if (name == "Corpse of Blighted Verdant Moarsman")
            {
	            // todo has tier 5, 7
            }
            else if (name == "Corpse of Falatacot Blood Prophetess")
            {
	            // todo has tier 5, 7
            }
			else if (name == "Corpse of Banderling Striker")
			{
				// Colo creature with weird wields I can't line up, tiers 3, 5
			}
			else if (name == "Corpse of Olthoi Slasher" || name == "Corpse of Olthoi Ripper" ||
			         name == "Corpse of Degenerate Shadow" || name == "Corpse of Depraved Shadow" ||
					 name == "Corpse of Parfal Sleech" || name == "Corpse of Listris Sleech" ||
			         name == "Corpse of Ravager")
            {
	            results[6] = new List<ContainerInfo>(); // Colo
	            results[7] = new List<ContainerInfo>();

	            foreach (var containerInfo in containerInfos)
	            {
		            if ((containerInfo.Landcell & 0xFFF00000) == 0x00B00000) // Colo
			            results[6].Add(containerInfo);
		            else
			            results[7].Add(containerInfo);
	            }
			}
			else if (name == "Corpse of Reedshark Hunter" || name == "Corpse of Reedshark Seeker" ||
			         name == "Corpse of War Reaper" || name == "Corpse of Tamed Reaper" ||
			         name == "Corpse of Tamed Armoredillo" || name == "Corpse of Guardian Armoredillo" || name == "Corpse of War Armoredillo")
            {
				// Neftet creatures of that are multi-tier
			}
			else
            {
                var tier = containerInfos.GetTier();

                results[tier] = new List<ContainerInfo>(containerInfos);
            }

			// remove any empty results
			var keys = results.Keys.ToList();
			foreach (var key in keys)
				if (results[key].Count == 0)
					results.Remove(key);

			return results;
        }
    }
}
