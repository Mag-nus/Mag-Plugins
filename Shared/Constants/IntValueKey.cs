
namespace Mag.Shared.Constants
{
	// https://github.com/ACEmulator/ACE/blob/master/Source/ACE.Entity/Enum/Properties/PropertyInt.cs
	public enum IntValueKey
	{
        // properties marked as ServerOnly are properties we never saw in PCAPs, from here:
        // http://ac.yotesfan.com/ace_object/not_used_enums.php
        // source: @OptimShi
        // description attributes are used by the weenie editor for a cleaner display name

        Undef                                    = 0,
        [ServerOnly]
        ItemType                                 = 1,
        CreatureType                             = 2,
        [ServerOnly]
        PaletteTemplate                          = 3,
        ClothingPriority                         = 4,
        [SendOnLogin]
        EncumbranceVal                           = 5, // ENCUMB_VAL_INT,
        [SendOnLogin]
        ItemsCapacity                            = 6,
        [SendOnLogin]
        ContainersCapacity                       = 7,
        [ServerOnly]
        Mass                                     = 8,
        [ServerOnly]
        ValidLocations                           = 9, // LOCATIONS_INT
        [ServerOnly]
        CurrentWieldedLocation                   = 10,
        [ServerOnly]
        MaxStackSize                             = 11,
        [ServerOnly]
        StackSize                                = 12,
        [ServerOnly]
        StackUnitEncumbrance                     = 13,
        [ServerOnly]
        StackUnitMass                            = 14,
        [ServerOnly]
        StackUnitValue                           = 15,
        [ServerOnly]
        ItemUseable                              = 16,
        RareId                                   = 17,
        [ServerOnly]
        UiEffects                                = 18,
        Value                                    = 19,
        [Ephemeral][SendOnLogin]
        CoinValue                                = 20,
        TotalExperience                          = 21,
        AvailableCharacter                       = 22,
        TotalSkillCredits                        = 23,
        [SendOnLogin]
        AvailableSkillCredits                    = 24,
        [SendOnLogin]
        Level                                    = 25,
        AccountRequirements                      = 26,
        ArmorType                                = 27,
        ArmorLevel                               = 28,
        AllegianceCpPool                         = 29,
        [SendOnLogin]
        AllegianceRank                           = 30,
        ChannelsAllowed                          = 31,
        ChannelsActive                           = 32,
        Bonded                                   = 33,
        MonarchsRank                             = 34,
        AllegianceFollowers                      = 35,
        ResistMagic                              = 36,
        ResistItemAppraisal                      = 37,
        ResistLockpick                           = 38,
        DeprecatedResistRepair                   = 39,
        [SendOnLogin]
        CombatMode                               = 40,
        CurrentAttackHeight                      = 41,
        CombatCollisions                         = 42,
        [SendOnLogin]
        NumDeaths                                = 43,
        Damage                                   = 44,
        DamageType                               = 45,
        [ServerOnly]
        DefaultCombatStyle                       = 46,
        [SendOnLogin]
        AttackType                               = 47,
        WeaponSkill                              = 48,
        WeaponTime                               = 49,
        AmmoType                                 = 50,
        CombatUse                                = 51,
        [ServerOnly]
        ParentLocation                           = 52,
        /// <summary>
        /// TODO: Migrate inventory order away from this and instead use the new InventoryOrder property
        /// TODO: PlacementPosition is used (very sparingly) in cache.bin, so it has (or had) a meaning at one point before we hijacked it
        /// TODO: and used it for our own inventory order
        /// </summary>
        [ServerOnly]
        PlacementPosition                        = 53,
        WeaponEncumbrance                        = 54,
        WeaponMass                               = 55,
        ShieldValue                              = 56,
        ShieldEncumbrance                        = 57,
        MissileInventoryLocation                 = 58,
        FullDamageType                           = 59,
        WeaponRange                              = 60,
        AttackersSkill                           = 61,
        DefendersSkill                           = 62,
        AttackersSkillValue                      = 63,
        AttackersClass                           = 64,
        [ServerOnly]
        Placement                                = 65,
        CheckpointStatus                         = 66,
        Tolerance                                = 67,
        TargetingTactic                          = 68,
        CombatTactic                             = 69,
        HomesickTargetingTactic                  = 70,
        NumFollowFailures                        = 71,
        FriendType                               = 72,
        FoeType                                  = 73,
        MerchandiseItemTypes                     = 74,
        MerchandiseMinValue                      = 75,
        MerchandiseMaxValue                      = 76,
        NumItemsSold                             = 77,
        NumItemsBought                           = 78,
        MoneyIncome                              = 79,
        MoneyOutflow                             = 80,
        [Ephemeral]
        MaxGeneratedObjects                      = 81,
        [Ephemeral]
        InitGeneratedObjects                     = 82,
        ActivationResponse                       = 83,
        OriginalValue                            = 84,
        NumMoveFailures                          = 85,
        MinLevel                                 = 86,
        MaxLevel                                 = 87,
        LockpickMod                              = 88,
        BoosterEnum                              = 89,
        BoostValue                               = 90,
        MaxStructure                             = 91,
        Structure                                = 92,
        [ServerOnly]
        PhysicsState                             = 93,
        [ServerOnly]
        TargetType                               = 94,
        RadarBlipColor                           = 95,
        EncumbranceCapacity                      = 96,
        LoginTimestamp                           = 97,
        [SendOnLogin]
        CreationTimestamp                        = 98,
        PkLevelModifier                          = 99,
        GeneratorType                            = 100,
        AiAllowedCombatStyle                     = 101,
        LogoffTimestamp                          = 102,
        GeneratorDestructionType                 = 103,
        ActivationCreateClass                    = 104,
        ItemWorkmanship                          = 105,
        ItemSpellcraft                           = 106,
        ItemCurMana                              = 107,
        ItemMaxMana                              = 108,
        ItemDifficulty                           = 109,
        ItemAllegianceRankLimit                  = 110,
        PortalBitmask                            = 111,
        AdvocateLevel                            = 112,
        [SendOnLogin]
        Gender                                   = 113,
        Attuned                                  = 114,
        ItemSkillLevelLimit                      = 115,
        GateLogic                                = 116,
        ItemManaCost                             = 117,
        Logoff                                   = 118,
        Active                                   = 119,
        AttackHeight                             = 120,
        NumAttackFailures                        = 121,
        AiCpThreshold                            = 122,
        AiAdvancementStrategy                    = 123,
        Version                                  = 124,
        [SendOnLogin]
        Age                                      = 125,
        VendorHappyMean                          = 126,
        VendorHappyVariance                      = 127,
        CloakStatus                              = 128,
        [SendOnLogin]
        VitaeCpPool                              = 129,
        NumServicesSold                          = 130,
        MaterialType                             = 131,
        [SendOnLogin]
        NumAllegianceBreaks                      = 132,
        [Ephemeral]
        ShowableOnRadar                          = 133,
        [SendOnLogin]
        PlayerKillerStatus                       = 134,
        VendorHappyMaxItems                      = 135,
        ScorePageNum                             = 136,
        ScoreConfigNum                           = 137,
        ScoreNumScores                           = 138,
        [SendOnLogin]
        DeathLevel                               = 139,
        AiOptions                                = 140,
        OpenToEveryone                           = 141,
        GeneratorTimeType                        = 142,
        GeneratorStartTime                       = 143,
        GeneratorEndTime                         = 144,
        GeneratorEndDestructionType              = 145,
        XpOverride                               = 146,
        NumCrashAndTurns                         = 147,
        ComponentWarningThreshold                = 148,
        HouseStatus                              = 149,
        [ServerOnly]
        HookPlacement                            = 150,
        [ServerOnly]
        HookType                                 = 151,
        [ServerOnly]
        HookItemType                             = 152,
        AiPpThreshold                            = 153,
        GeneratorVersion                         = 154,
        HouseType                                = 155,
        PickupEmoteOffset                        = 156,
        WeenieIteration                          = 157,
        WieldRequirements                        = 158,
        WieldSkillType                           = 159,
        WieldDifficulty                          = 160,
        HouseMaxHooksUsable                      = 161,
        HouseCurrentHooksUsable                  = 162,
        AllegianceMinLevel                       = 163,
        AllegianceMaxLevel                       = 164,
        HouseRelinkHookCount                     = 165,
        SlayerCreatureType                       = 166,
        ConfirmationInProgress                   = 167,
        ConfirmationTypeInProgress               = 168,
        TsysMutationData                         = 169,
        NumItemsInMaterial                       = 170,
        NumTimesTinkered                         = 171,
        AppraisalLongDescDecoration              = 172,
        AppraisalLockpickSuccessPercent          = 173,
        [Ephemeral]
        AppraisalPages                           = 174,
        [Ephemeral]
        AppraisalMaxPages                        = 175,
        AppraisalItemSkill                       = 176,
        GemCount                                 = 177,
        GemType                                  = 178,
        ImbuedEffect                             = 179,
        AttackersRawSkillValue                   = 180,
        [SendOnLogin]
        ChessRank                                = 181,
        ChessTotalGames                          = 182,
        ChessGamesWon                            = 183,
        ChessGamesLost                           = 184,
        TypeOfAlteration                         = 185,
        SkillToBeAltered                         = 186,
        SkillAlterationCount                     = 187,
        [SendOnLogin]
        HeritageGroup                            = 188,
        TransferFromAttribute                    = 189,
        TransferToAttribute                      = 190,
        AttributeTransferCount                   = 191,
        [SendOnLogin]
        FakeFishingSkill                         = 192,
        NumKeys                                  = 193,
        DeathTimestamp                           = 194,
        PkTimestamp                              = 195,
        VictimTimestamp                          = 196,
        HookGroup                                = 197,
        AllegianceSwearTimestamp                 = 198,
        [SendOnLogin]
        HousePurchaseTimestamp                   = 199,
        RedirectableEquippedArmorCount           = 200,
        MeleeDefenseImbuedEffectTypeCache        = 201,
        MissileDefenseImbuedEffectTypeCache      = 202,
        MagicDefenseImbuedEffectTypeCache        = 203,
        ElementalDamageBonus                     = 204,
        ImbueAttempts                            = 205,
        ImbueSuccesses                           = 206,
        CreatureKills                            = 207,
        PlayerKillsPk                            = 208,
        PlayerKillsPkl                           = 209,
        RaresTierOne                             = 210,
        RaresTierTwo                             = 211,
        RaresTierThree                           = 212,
        RaresTierFour                            = 213,
        RaresTierFive                            = 214,
        [SendOnLogin]
        AugmentationStat                         = 215,
        [SendOnLogin]
        AugmentationFamilyStat                   = 216,
        [SendOnLogin]
        AugmentationInnateFamily                 = 217,
        [SendOnLogin]
        AugmentationInnateStrength               = 218,
        [SendOnLogin]
        AugmentationInnateEndurance              = 219,
        [SendOnLogin]
        AugmentationInnateCoordination           = 220,
        [SendOnLogin]
        AugmentationInnateQuickness              = 221,
        [SendOnLogin]
        AugmentationInnateFocus                  = 222,
        [SendOnLogin]
        AugmentationInnateSelf                   = 223,
        [SendOnLogin]
        AugmentationSpecializeSalvaging          = 224,
        [SendOnLogin]
        AugmentationSpecializeItemTinkering      = 225,
        [SendOnLogin]
        AugmentationSpecializeArmorTinkering     = 226,
        [SendOnLogin]
        AugmentationSpecializeMagicItemTinkering = 227,
        [SendOnLogin]
        AugmentationSpecializeWeaponTinkering    = 228,
        [SendOnLogin]
        AugmentationExtraPackSlot                = 229,
        [SendOnLogin]
        AugmentationIncreasedCarryingCapacity    = 230,
        [SendOnLogin]
        AugmentationLessDeathItemLoss            = 231,
        [SendOnLogin]
        AugmentationSpellsRemainPastDeath        = 232,
        [SendOnLogin]
        AugmentationCriticalDefense              = 233,
        [SendOnLogin]
        AugmentationBonusXp                      = 234,
        [SendOnLogin]
        AugmentationBonusSalvage                 = 235,
        [SendOnLogin]
        AugmentationBonusImbueChance             = 236,
        [SendOnLogin]
        AugmentationFasterRegen                  = 237,
        [SendOnLogin]
        AugmentationIncreasedSpellDuration       = 238,
        [SendOnLogin]
        AugmentationResistanceFamily             = 239,
        [SendOnLogin]
        AugmentationResistanceSlash              = 240,
        [SendOnLogin]
        AugmentationResistancePierce             = 241,
        [SendOnLogin]
        AugmentationResistanceBlunt              = 242,
        [SendOnLogin]
        AugmentationResistanceAcid               = 243,
        [SendOnLogin]
        AugmentationResistanceFire               = 244,
        [SendOnLogin]
        AugmentationResistanceFrost              = 245,
        [SendOnLogin]
        AugmentationResistanceLightning          = 246,
        RaresTierOneLogin                        = 247,
        RaresTierTwoLogin                        = 248,
        RaresTierThreeLogin                      = 249,
        RaresTierFourLogin                       = 250,
        RaresTierFiveLogin                       = 251,
        RaresLoginTimestamp                      = 252,
        RaresTierSix                             = 253,
        RaresTierSeven                           = 254,
        RaresTierSixLogin                        = 255,
        RaresTierSevenLogin                      = 256,
        ItemAttributeLimit                       = 257,
        ItemAttributeLevelLimit                  = 258,
        ItemAttribute2ndLimit                    = 259,
        ItemAttribute2ndLevelLimit               = 260,
        CharacterTitleId                         = 261,
        NumCharacterTitles                       = 262,
        ResistanceModifierType                   = 263,
        FreeTinkersBitfield                      = 264,
        EquipmentSetId                           = 265,
        PetClass                                 = 266,
        Lifespan                                 = 267,
        [Ephemeral]
        RemainingLifespan                        = 268,
        UseCreateQuantity                        = 269,
        WieldRequirements2                       = 270,
        WieldSkillType2                          = 271,
        WieldDifficulty2                         = 272,
        WieldRequirements3                       = 273,
        WieldSkillType3                          = 274,
        WieldDifficulty3                         = 275,
        WieldRequirements4                       = 276,
        WieldSkillType4                          = 277,
        WieldDifficulty4                         = 278,
        Unique                                   = 279,
        SharedCooldown                           = 280,
        Faction1Bits                             = 281,
        Faction2Bits                             = 282,
        Faction3Bits                             = 283,
        Hatred1Bits                              = 284,
        Hatred2Bits                              = 285,
        Hatred3Bits                              = 286,
        SocietyRankCelhan                        = 287,
        SocietyRankEldweb                        = 288,
        SocietyRankRadblo                        = 289,
        HearLocalSignals                         = 290,
        HearLocalSignalsRadius                   = 291,
        Cleaving                                 = 292,
        [SendOnLogin]
        AugmentationSpecializeGearcraft          = 293,
        [SendOnLogin]
        AugmentationInfusedCreatureMagic         = 294,
        [SendOnLogin]
        AugmentationInfusedItemMagic             = 295,
        [SendOnLogin]
        AugmentationInfusedLifeMagic             = 296,
        [SendOnLogin]
        AugmentationInfusedWarMagic              = 297,
        [SendOnLogin]
        AugmentationCriticalExpertise            = 298,
        [SendOnLogin]
        AugmentationCriticalPower                = 299,
        [SendOnLogin]
        AugmentationSkilledMelee                 = 300,
        [SendOnLogin]
        AugmentationSkilledMissile               = 301,
        [SendOnLogin]
        AugmentationSkilledMagic                 = 302,
        ImbuedEffect2                            = 303,
        ImbuedEffect3                            = 304,
        ImbuedEffect4                            = 305,
        ImbuedEffect5                            = 306,
        [SendOnLogin]
        DamageRating                             = 307,
        [SendOnLogin]
        DamageResistRating                       = 308,
        [SendOnLogin]
        AugmentationDamageBonus                  = 309,
        [SendOnLogin]
        AugmentationDamageReduction              = 310,
        ImbueStackingBits                        = 311,
        [SendOnLogin]
        HealOverTime                             = 312,
        [SendOnLogin]
        CritRating                               = 313,
        [SendOnLogin]
        CritDamageRating                         = 314,
        [SendOnLogin]
        CritResistRating                         = 315,
        [SendOnLogin]
        CritDamageResistRating                   = 316,
        [SendOnLogin]
        HealingResistRating                      = 317,
        [SendOnLogin]
        DamageOverTime                           = 318,
        ItemMaxLevel                             = 319,
        ItemXpStyle                              = 320,
        EquipmentSetExtra                        = 321,
        [SendOnLogin]
        AetheriaBitfield                         = 322,
        [SendOnLogin]
        HealingBoostRating                       = 323,
        HeritageSpecificArmor                    = 324,
        AlternateRacialSkills                    = 325,
        [SendOnLogin]
        AugmentationJackOfAllTrades              = 326,
        [SendOnLogin]
        AugmentationResistanceNether             = 327,
        [SendOnLogin]
        AugmentationInfusedVoidMagic             = 328,
        [SendOnLogin]
        WeaknessRating                           = 329,
        [SendOnLogin]
        NetherOverTime                           = 330,
        [SendOnLogin]
        NetherResistRating                       = 331,
        LuminanceAward                           = 332,
        [SendOnLogin]
        LumAugDamageRating                       = 333,
        [SendOnLogin]
        LumAugDamageReductionRating              = 334,
        [SendOnLogin]
        LumAugCritDamageRating                   = 335,
        [SendOnLogin]
        LumAugCritReductionRating                = 336,
        [SendOnLogin]
        LumAugSurgeEffectRating                  = 337,
        [SendOnLogin]
        LumAugSurgeChanceRating                  = 338,
        [SendOnLogin]
        LumAugItemManaUsage                      = 339,
        [SendOnLogin]
        LumAugItemManaGain                       = 340,
        [SendOnLogin]
        LumAugVitality                           = 341,
        [SendOnLogin]
        LumAugHealingRating                      = 342,
        [SendOnLogin]
        LumAugSkilledCraft                       = 343,
        [SendOnLogin]
        LumAugSkilledSpec                        = 344,
        [SendOnLogin]
        LumAugNoDestroyCraft                     = 345,
        RestrictInteraction                      = 346,
        OlthoiLootTimestamp                      = 347,
        OlthoiLootStep                           = 348,
        UseCreatesContractId                     = 349,
        [SendOnLogin]
        DotResistRating                          = 350,
        [SendOnLogin]
        LifeResistRating                         = 351,
        CloakWeaveProc                           = 352,
        WeaponType                               = 353,
        [SendOnLogin]
        MeleeMastery                             = 354,
        [SendOnLogin]
        RangedMastery                            = 355,
        SneakAttackRating                        = 356,
        RecklessnessRating                       = 357,
        DeceptionRating                          = 358,
        CombatPetRange                           = 359,
        [SendOnLogin]
        WeaponAuraDamage                         = 360,
        [SendOnLogin]
        WeaponAuraSpeed                          = 361,
        [SendOnLogin]
        SummoningMastery                         = 362,
        HeartbeatLifespan                        = 363,
        UseLevelRequirement                      = 364,
        [SendOnLogin]
        LumAugAllSkills                          = 365,
        UseRequiresSkill                         = 366,
        UseRequiresSkillLevel                    = 367,
        UseRequiresSkillSpec                     = 368,
        UseRequiresLevel                         = 369,
        [SendOnLogin]
        GearDamage                               = 370,
        [SendOnLogin]
        GearDamageResist                         = 371,
        [SendOnLogin]
        GearCrit                                 = 372,
        [SendOnLogin]
        GearCritResist                           = 373,
        [SendOnLogin]
        GearCritDamage                           = 374,
        [SendOnLogin]
        GearCritDamageResist                     = 375,
        [SendOnLogin]
        GearHealingBoost                         = 376,
        [SendOnLogin]
        GearNetherResist                         = 377,
        [SendOnLogin]
        GearLifeResist                           = 378,
        [SendOnLogin]
        GearMaxHealth                            = 379,
        Unknown380                               = 380,
        [SendOnLogin]
        PKDamageRating                           = 381,
        [SendOnLogin]
        PKDamageResistRating                     = 382,
        [SendOnLogin]
        GearPKDamageRating                       = 383,
        [SendOnLogin]
        GearPKDamageResistRating                 = 384,
        Unknown385                               = 385,
        /// <summary>
        /// Overpower chance % for endgame creatures.
        /// </summary>
        [SendOnLogin]
        Overpower                                = 386,
        [SendOnLogin]
        OverpowerResist                          = 387,
        // Client does not display accurately
        [SendOnLogin]
        GearOverpower                            = 388,
        // Client does not display accurately
        [SendOnLogin]
        GearOverpowerResist                      = 389,
        // Number of times a character has enlightened
        [SendOnLogin]
        Enlightenment                            = 390,


		// ACE Specific
        [ServerOnly]
        PCAPRecordedAutonomousMovement           = 8007,
        [ServerOnly]
        PCAPRecordedMaxVelocityEstimated         = 8030,
        [ServerOnly]
        PCAPRecordedPlacement                    = 8041,
        [ServerOnly]
        PCAPRecordedAppraisalPages               = 8042,
        [ServerOnly]
        PCAPRecordedAppraisalMaxPages            = 8043,

        //[ServerOnly]
        //TotalLogins                              = 9001,
        //[ServerOnly]
        //DeletionTimestamp                        = 9002,
        //[ServerOnly]
        //CharacterOptions1                        = 9003,
        //[ServerOnly]
        //CharacterOptions2                        = 9004,
        //[ServerOnly]
        //LootTier                                 = 9005,
        //[ServerOnly]
        //GeneratorProbability                     = 9006,
        //[ServerOnly]
        //WeenieType                               = 9007 // I don't think this property type is needed anymore. We don't store the weenie type in the property bags, we store it as a separate field in the base objects.
        [ServerOnly]
        CurrentLoyaltyAtLastLogoff              = 9008,
        [ServerOnly]
        CurrentLeadershipAtLastLogoff           = 9009,
        [ServerOnly]
        AllegianceOfficerRank                   = 9010,
        [ServerOnly]
        HouseRentTimestamp                      = 9011,
        /// <summary>
        ///  Stores the player's selected hairstyle at creation or after a barber use. This is used only for Gear Knights and Olthoi characters who have more than a single part/texture for a "hairstyle" (BodyStyle)
        /// </summary>
        [ServerOnly]
        Hairstyle                               = 9012,
        /// <summary>
        /// Used to store the calculated Clothing Priority for use with armor reduced items and items like Over-Robes.
        /// </summary>
        [Ephemeral][ServerOnly]
        VisualClothingPriority                  = 9013,
        [ServerOnly]
        SquelchGlobal                           = 9014,

        /// <summary>
        /// TODO: This is a place holder for future use. See PlacementPosition
        /// This is the sort order for items in a container
        /// </summary>
        [ServerOnly]
        InventoryOrder                          = 9015,

		// Decal Specific
		WeenieClassId_Decal                     = 218103808,
		Icon_Decal_DID                          = 218103809,
		Container_Decal_IID                     = 218103810,
		Landblock_Decal                         = 218103811,
		ItemSlots_Decal                         = 218103812,
		PackSlots_Decal                         = 218103813,
		StackCount_Decal                        = 218103814,
		StackMax_Decal                          = 218103815,
		Spell_Decal_DID                         = 218103816,
		SlotLegacy_Decal                        = 218103817,
		Wielder_Decal_IID                       = 218103818,
		WieldingSlot_Decal                      = 218103819,
		Monarch_Decal_IID                       = 218103820,
		Coverage_Decal                          = 218103821,
		EquipableSlots_Decal                    = 218103822,
		EquipType_Decal                         = 218103823,
		IconOutline_Decal                       = 218103824,
		MissileType_Decal                       = 218103825,
		UsageMask_Decal                         = 218103826,
		HouseOwner_Decal_IID                    = 218103827,
		HookMask_Decal                          = 218103828,
		HookType_Decal                          = 218103829,
		Setup_Decal_DID                         = 218103830,
		ObjectDescriptionFlags_Decal            = 218103831,
		CreateFlags1_Decal                      = 218103832,
		CreateFlags2_Decal                      = 218103833,
		Category_Decal                          = 218103834,
		Behavior_Decal                          = 218103835,
		MagicDef_Decal                          = 218103836,
		SpecialProps_Decal                      = 218103837,
		SpellCount_Decal                        = 218103838,
		WeapSpeed_Decal                         = 218103839,
		EquipSkill_Decal                        = 218103840,
		DamageType_Decal                        = 218103841,
		MaxDamage_Decal                         = 218103842,
		Unknown10_Decal                         = 218103843, // CurrentWieldLocation?
		Unknown100000_Decal                     = 218103844, // RadarBlipColor ???
		Unknown800000_Decal                     = 218103845,
		Unknown8000000_Decal                    = 218103846,
		PhysicsDataFlags_Decal                  = 218103847,
		ActiveSpellCount_Decal                  = 218103848,
		IconOverlay_Decal_DID                   = 218103849,
		IconUnderlay_Decal_DID                  = 218103850,
		Slot_Decal                              = 231735296,
	}

	public static class IntValueKeyTools
	{
		/// <summary>
		/// Converts a decal specific IntValueKey to the actual IntValueKey.
		/// If this is not an IntValueKey, 0 will be returned.
		/// </summary>
		public static uint ConvertToInt(IntValueKey input)
		{
			if (input == IntValueKey.Category_Decal)		return (int)IntValueKey.ItemType;
			if (input == IntValueKey.Coverage_Decal)		return (int)IntValueKey.ClothingPriority;
			if (input == IntValueKey.ItemSlots_Decal)		return (int)IntValueKey.ItemsCapacity;
			if (input == IntValueKey.PackSlots_Decal)		return (int)IntValueKey.ContainersCapacity;
			if (input == IntValueKey.EquipableSlots_Decal)	return (int)IntValueKey.ValidLocations;
			//if (input == IntValueKey.WieldingSlot_Decal)	return (int)IntValueKey.CurrentWieldedLocation;
			if (input == IntValueKey.StackMax_Decal)		return (int)IntValueKey.MaxStackSize;
			if (input == IntValueKey.StackCount_Decal)		return (int)IntValueKey.StackSize;
			if (input == IntValueKey.IconOutline_Decal)		return (int)IntValueKey.UiEffects;
			if (input == IntValueKey.MaxDamage_Decal)		return (int)IntValueKey.Damage;
			if (input == IntValueKey.DamageType_Decal)		return (int)IntValueKey.DamageType;
			if (input == IntValueKey.EquipSkill_Decal)		return (int)IntValueKey.WeaponSkill;
			if (input == IntValueKey.WeapSpeed_Decal)		return (int)IntValueKey.WeaponTime;
			if (input == IntValueKey.MissileType_Decal)		return (int)IntValueKey.AmmoType;
			if (input == IntValueKey.EquipType_Decal)		return (int)IntValueKey.CombatUse;
			if (input == IntValueKey.UsageMask_Decal)		return (int)IntValueKey.TargetType;
			if (input == IntValueKey.HookMask_Decal)		return (int)IntValueKey.HookType;

			return 0;
		}

		/// <summary>
		/// If input is not a IID, 0 will be returned
		/// </summary>
		public static uint ConvertToIID(IntValueKey input)
		{
			if (input == IntValueKey.Container_Decal_IID)	return 2;  // CONTAINER_IID
			if (input == IntValueKey.Wielder_Decal_IID)		return 3;  // WIELDER_IID
			if (input == IntValueKey.Monarch_Decal_IID)		return 26; // MONARCH_IID
			if (input == IntValueKey.HouseOwner_Decal_IID)	return 32; // HOUSE_OWNER_IID

			return 0;
		}

		/// <summary>
		/// If input is not a DID, 0 will be returned
		/// </summary>
		public static uint ConvertToDID(IntValueKey input)
		{
			if (input == IntValueKey.Setup_Decal_DID)			return 1;  // SETUP_DID
			if (input == IntValueKey.Icon_Decal_DID)			return 8;  // ICON_DID
			if (input == IntValueKey.Spell_Decal_DID)			return 28; // SPELL_DID
			if (input == IntValueKey.IconOverlay_Decal_DID)		return 50; // ICON_OVERLAY_DID
			if (input == IntValueKey.IconUnderlay_Decal_DID)	return 52; // ICON_UNDERLAY_DID

			return 0;
		}
	}
}
