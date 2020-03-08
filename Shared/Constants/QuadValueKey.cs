
namespace Mag.Shared.Constants
{
	// https://github.com/ACEmulator/ACE/blob/master/Source/ACE.Entity/Enum/Properties/PropertyInt64.cs
	public enum QuadValueKey
	{
        Undef               = 0,
        [SendOnLogin]
        TotalExperience     = 1,
        [SendOnLogin]
        AvailableExperience = 2,
        AugmentationCost    = 3,
        ItemTotalXp         = 4,
        ItemBaseXp          = 5,
        [SendOnLogin]
        AvailableLuminance  = 6,
        [SendOnLogin]
        MaximumLuminance    = 7,
        InteractionReqs     = 8,


		// ACE Specific
        /* custom */
        [ServerOnly]
        AllegianceXPCached    = 9000,
        [ServerOnly]
        AllegianceXPGenerated = 9001,
        [ServerOnly]
        AllegianceXPReceived  = 9002,
        [ServerOnly]
        VerifyXp              = 9003
	}
}
