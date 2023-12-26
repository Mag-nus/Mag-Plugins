using System;

using ACE.Database.Models.Shard;

namespace Mag_SuitBuilder.ACE_Helpers
{
	static class ACEBiotaCreator
	{
        public static ACE.Entity.Enum.WeenieType DetermineWeenieType(Biota biota)
        {
            var objectDescriptionFlagProperty = biota.GetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8003);

            if (objectDescriptionFlagProperty == null)
                return ACE.Entity.Enum.WeenieType.Undef;

            var objectDescriptionFlag = (ACE.Entity.Enum.ObjectDescriptionFlag)objectDescriptionFlagProperty;

            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.LifeStone))
                return ACE.Entity.Enum.WeenieType.LifeStone;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.BindStone))
                return ACE.Entity.Enum.WeenieType.AllegianceBindstone;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.PkSwitch))
                return ACE.Entity.Enum.WeenieType.PKModifier;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.NpkSwitch))
                return ACE.Entity.Enum.WeenieType.PKModifier;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Lockpick))
                return ACE.Entity.Enum.WeenieType.Lockpick;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Food))
                return ACE.Entity.Enum.WeenieType.Food;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Healer))
                return ACE.Entity.Enum.WeenieType.Healer;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Book))
                return ACE.Entity.Enum.WeenieType.Book;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Portal))
                return ACE.Entity.Enum.WeenieType.Portal;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Door))
                return ACE.Entity.Enum.WeenieType.Door;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Vendor))
                return ACE.Entity.Enum.WeenieType.Vendor;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Admin))
                return ACE.Entity.Enum.WeenieType.Admin;
            if (objectDescriptionFlag.HasFlag(ACE.Entity.Enum.ObjectDescriptionFlag.Corpse))
                return ACE.Entity.Enum.WeenieType.Corpse;

            if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ValidLocations) == (int)ACE.Entity.Enum.EquipMask.MissileAmmo)
                return ACE.Entity.Enum.WeenieType.Ammunition;

            var itemTypeProperty = biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.ItemType);

            if (itemTypeProperty == null)
                return ACE.Entity.Enum.WeenieType.Undef;

            var itemType = (ACE.Entity.Enum.ItemType)itemTypeProperty;

            switch (itemType)
            {
                case ACE.Entity.Enum.ItemType.Misc:
                    if (
                           biota.WeenieClassId == 9548 || // W_HOUSE_CLASS
                           biota.WeenieClassId >= 9693 && biota.WeenieClassId <= 10492 || // W_HOUSECOTTAGE1_CLASS to W_HOUSECOTTAGE800_CLASS
                           biota.WeenieClassId >= 10493 && biota.WeenieClassId <= 10662 || // W_HOUSEVILLA801_CLASS to W_HOUSEVILLA970_CLASS
                           biota.WeenieClassId >= 10663 && biota.WeenieClassId <= 10692 || // W_HOUSEMANSION971_CLASS to W_HOUSEMANSION1000_CLASS
                           biota.WeenieClassId >= 10746 && biota.WeenieClassId <= 10750 || // W_HOUSETEST1_CLASS to W_HOUSETEST5_CLASS
                           biota.WeenieClassId >= 10829 && biota.WeenieClassId <= 10839 || // W_HOUSETEST6_CLASS to W_HOUSETEST16_CLASS
                           biota.WeenieClassId >= 11677 && biota.WeenieClassId <= 11682 || // W_HOUSETEST17_CLASS to W_HOUSETEST22_CLASS
                           biota.WeenieClassId >= 12311 && biota.WeenieClassId <= 12460 || // W_HOUSECOTTAGE1001_CLASS to W_HOUSECOTTAGE1150_CLASS
                           biota.WeenieClassId >= 12775 && biota.WeenieClassId <= 13024 || // W_HOUSECOTTAGE1151_CLASS to W_HOUSECOTTAGE1400_CLASS
                           biota.WeenieClassId >= 13025 && biota.WeenieClassId <= 13064 || // W_HOUSEVILLA1401_CLASS to W_HOUSEVILLA1440_CLASS
                           biota.WeenieClassId >= 13065 && biota.WeenieClassId <= 13074 || // W_HOUSEMANSION1441_CLASS to W_HOUSEMANSION1450_CLASS
                           biota.WeenieClassId == 13234 || // W_HOUSECOTTAGETEST10000_CLASS
                           biota.WeenieClassId == 13235 || // W_HOUSEVILLATEST10001_CLASS
                           biota.WeenieClassId >= 13243 && biota.WeenieClassId <= 14042 || // W_HOUSECOTTAGE1451_CLASS to W_HOUSECOTTAGE2350_CLASS
                           biota.WeenieClassId >= 14043 && biota.WeenieClassId <= 14222 || // W_HOUSEVILLA1851_CLASS to W_HOUSEVILLA2440_CLASS
                           biota.WeenieClassId >= 14223 && biota.WeenieClassId <= 14242 || // W_HOUSEMANSION1941_CLASS to W_HOUSEMANSION2450_CLASS
                           biota.WeenieClassId >= 14938 && biota.WeenieClassId <= 15087 || // W_HOUSECOTTAGE2451_CLASS to W_HOUSECOTTAGE2600_CLASS
                           biota.WeenieClassId >= 15088 && biota.WeenieClassId <= 15127 || // W_HOUSEVILLA2601_CLASS to W_HOUSEVILLA2640_CLASS
                           biota.WeenieClassId >= 15128 && biota.WeenieClassId <= 15137 || // W_HOUSEMANSION2641_CLASS to W_HOUSEMANSION2650_CLASS
                           biota.WeenieClassId >= 15452 && biota.WeenieClassId <= 15457 || // W_HOUSEAPARTMENT2851_CLASS to W_HOUSEAPARTMENT2856_CLASS
                           biota.WeenieClassId >= 15458 && biota.WeenieClassId <= 15607 || // W_HOUSECOTTAGE2651_CLASS to W_HOUSECOTTAGE2800_CLASS
                           biota.WeenieClassId >= 15612 && biota.WeenieClassId <= 15661 || // W_HOUSEVILLA2801_CLASS to W_HOUSEVILLA2850_CLASS
                           biota.WeenieClassId >= 15897 && biota.WeenieClassId <= 16890 || // W_HOUSEAPARTMENT2857_CLASS to W_HOUSEAPARTMENT3850_CLASS
                           biota.WeenieClassId >= 16923 && biota.WeenieClassId <= 18923 || // W_HOUSEAPARTMENT4051_CLASS to W_HOUSEAPARTMENT6050_CLASS
                           biota.WeenieClassId >= 18924 && biota.WeenieClassId <= 19073 || // W_HOUSECOTTAGE3851_CLASS to W_HOUSECOTTAGE4000_CLASS
                           biota.WeenieClassId >= 19077 && biota.WeenieClassId <= 19126 || // W_HOUSEVILLA4001_CLASS to W_HOUSEVILLA4050_CLASS
                           biota.WeenieClassId >= 20650 && biota.WeenieClassId <= 20799 || // W_HOUSECOTTAGE6051_CLASS to W_HOUSECOTTAGE6200_CLASS
                           biota.WeenieClassId >= 20800 && biota.WeenieClassId <= 20839 || // W_HOUSEVILLA6201_CLASS to W_HOUSEVILLA6240_CLASS
                           biota.WeenieClassId >= 20840 && biota.WeenieClassId <= 20849    // W_HOUSEMANSION6241_CLASS to W_HOUSEMANSION6250_CLASS
                           )
                        return ACE.Entity.Enum.WeenieType.House;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Deed"))
                        return ACE.Entity.Enum.WeenieType.Deed;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Button") ||
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Lever") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Broken")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Candle") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Floating") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bronze")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Torch") && biota.WeenieClassId != 293
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Plant") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fertilized")
                        )
                        return ACE.Entity.Enum.WeenieType.Switch;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Essence") && biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStructure) == 50)
                        return ACE.Entity.Enum.WeenieType.PetDevice;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Mag-Ma!")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name) == "Acid"
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Vent")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Steam")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Electric Floor")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Refreshing")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name) == "Sewer"
                        //|| parsed.wdesc._name.m_buffer.Contains("Ice") && !parsed.wdesc._name.m_buffer.Contains("Box")
                        //|| parsed.wdesc._name.m_buffer.Contains("Firespurt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Flames")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Plume")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("The Black Breath")
                        //|| parsed.wdesc._name.m_buffer.Contains("Bonfire")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Geyser")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Magma")
                        || biota.WeenieClassId == 14805
                        //|| parsed.wdesc._name.m_buffer.Contains("Pool") && !parsed.wdesc._name.m_buffer.Contains("of")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Firespurt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bonfire")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pool") && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("of")
                        )
                        return ACE.Entity.Enum.WeenieType.HotSpot;
                    else
                        goto default;

                case ACE.Entity.Enum.ItemType.Caster:
                    return ACE.Entity.Enum.WeenieType.Caster;
                case ACE.Entity.Enum.ItemType.Jewelry:
                    return ACE.Entity.Enum.WeenieType.Generic;
                case ACE.Entity.Enum.ItemType.Armor:
                case ACE.Entity.Enum.ItemType.Clothing:
                    return ACE.Entity.Enum.WeenieType.Clothing;

                case ACE.Entity.Enum.ItemType.Container:
                    if (
                        biota.WeenieClassId == 9686 || // W_HOOK_CLASS
                        biota.WeenieClassId == 11697 || // W_HOOK_FLOOR_CLASS
                        biota.WeenieClassId == 11698 || // W_HOOK_CEILING_CLASS
                        biota.WeenieClassId == 12678 || // W_HOOK_ROOF_CLASS
                        biota.WeenieClassId == 12679    // W_HOOK_YARD_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.Hook;
                    else if (
                        biota.WeenieClassId == 9687     // W_STORAGE_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.Storage;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Backpack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Sack")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pouch")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Basket")
                        )
                        return ACE.Entity.Enum.WeenieType.Container;
                    else
                        return ACE.Entity.Enum.WeenieType.Chest;

                case ACE.Entity.Enum.ItemType.None:
                    if (
                        biota.WeenieClassId == 9621 || // W_SLUMLORD_CLASS
                        biota.WeenieClassId == 10752 || // W_SLUMLORDTESTCHEAP_CLASS
                        biota.WeenieClassId == 10753 || // W_SLUMLORDTESTEXPENSIVE_CLASS
                        biota.WeenieClassId == 10754 || // W_SLUMLORDTESTMODERATE_CLASS
                        biota.WeenieClassId == 11711 || // W_SLUMLORDCOTTAGECHEAP_CLASS
                        biota.WeenieClassId == 11712 || // W_SLUMLORDCOTTAGEEXPENSIVE_CLASS
                        biota.WeenieClassId == 11713 || // W_SLUMLORDCOTTAGEMODERATE_CLASS
                        biota.WeenieClassId == 11714 || // W_SLUMLORDMANSIONCHEAP_CLASS
                        biota.WeenieClassId == 11715 || // W_SLUMLORDMANSIONEXPENSIVE_CLASS
                        biota.WeenieClassId == 11716 || // W_SLUMLORDMANSIONMODERATE_CLASS
                        biota.WeenieClassId == 11717 || // W_SLUMLORDVILLACHEAP_CLASS
                        biota.WeenieClassId == 11718 || // W_SLUMLORDVILLAEXPENSIVE_CLASS
                        biota.WeenieClassId == 11719 || // W_SLUMLORDVILLAMODERATE_CLASS
                        biota.WeenieClassId == 11977 || // W_SLUMLORDCOTTAGES349_579_CLASS
                        biota.WeenieClassId == 11978 || // W_SLUMLORDVILLA851_925_CLASS
                        biota.WeenieClassId == 11979 || // W_SLUMLORDCOTTAGE580_800_CLASS
                        biota.WeenieClassId == 11980 || // W_SLUMLORDVILLA926_970_CLASS
                        biota.WeenieClassId == 11980 || // W_SLUMLORDVILLA926_970_CLASS
                        biota.WeenieClassId == 12461 || // W_SLUMLORDCOTTAGE1001_1075_CLASS
                        biota.WeenieClassId == 12462 || // W_SLUMLORDCOTTAGE1076_1150_CLASS
                        biota.WeenieClassId == 13078 || // W_SLUMLORDCOTTAGE1151_1275_CLASS
                        biota.WeenieClassId == 13079 || // W_SLUMLORDCOTTAGE1276_1400_CLASS
                        biota.WeenieClassId == 13080 || // W_SLUMLORDVILLA1401_1440_CLASS
                        biota.WeenieClassId == 13081 || // W_SLUMLORDMANSION1441_1450_CLASS
                        biota.WeenieClassId == 14243 || // W_SLUMLORDCOTTAGE1451_1650_CLASS
                        biota.WeenieClassId == 14244 || // W_SLUMLORDCOTTAGE1651_1850_CLASS
                        biota.WeenieClassId == 14245 || // W_SLUMLORDVILLA1851_1940_CLASS
                        biota.WeenieClassId == 14246 || // W_SLUMLORDMANSION1941_1950_CLASS
                        biota.WeenieClassId == 14247 || // W_SLUMLORDCOTTAGE1951_2150_CLASS
                        biota.WeenieClassId == 14248 || // W_SLUMLORDCOTTAGE2151_2350_CLASS
                        biota.WeenieClassId == 14249 || // W_SLUMLORDVILLA2351_2440_CLASS
                        biota.WeenieClassId == 14250 || // W_SLUMLORDMANSION2441_2450_CLASS
                        biota.WeenieClassId == 14934 || // W_SLUMLORDCOTTAGE2451_2525_CLASS
                        biota.WeenieClassId == 14935 || // W_SLUMLORDCOTTAGE2526_2600_CLASS
                        biota.WeenieClassId == 14936 || // W_SLUMLORDVILLA2601_2640_CLASS
                        biota.WeenieClassId == 14937 || // W_SLUMLORDMANSION2641_2650_CLASS
                                                        // wo.WeenieClassId == 15273 || // W_SLUMLORDFAKENUHMUDIRA_CLASS
                        biota.WeenieClassId == 15608 || // W_SLUMLORDAPARTMENT_CLASS
                        biota.WeenieClassId == 15609 || // W_SLUMLORDCOTTAGE2651_2725_CLASS
                        biota.WeenieClassId == 15610 || // W_SLUMLORDCOTTAGE2726_2800_CLASS
                        biota.WeenieClassId == 15611 || // W_SLUMLORDVILLA2801_2850_CLASS
                        biota.WeenieClassId == 19074 || // W_SLUMLORDCOTTAGE3851_3925_CLASS
                        biota.WeenieClassId == 19075 || // W_SLUMLORDCOTTAGE3926_4000_CLASS
                        biota.WeenieClassId == 19076 || // W_SLUMLORDVILLA4001_4050_CLASS
                        biota.WeenieClassId == 20850 || // W_SLUMLORDCOTTAGE6051_6125_CLASS
                        biota.WeenieClassId == 20851 || // W_SLUMLORDCOTTAGE6126_6200_CLASS
                        biota.WeenieClassId == 20852 || // W_SLUMLORDVILLA6201_6240_CLASS
                        biota.WeenieClassId == 20853    // W_SLUMLORDMANSION6241_6250_CLASS
                                                        // wo.WeenieClassId == 22118 || // W_SLUMLORDHAUNTEDMANSION_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.SlumLord;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bolt")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("wave")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wave")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Blast")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Ring")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Stream")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fist")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Missile")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Egg")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Death")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Fury")
                         || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wind")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Flaming Skull")
                         || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Edge")
                        // || wo.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowball")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Bomb")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Blade")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Stalactite")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Boulder")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Whirlwind")
                        )
                        return ACE.Entity.Enum.WeenieType.ProjectileSpell;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Missile")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Egg")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowball")
                        )
                        return ACE.Entity.Enum.WeenieType.Missile;
                    else
                        goto default;

                case ACE.Entity.Enum.ItemType.Creature:
                    var weenieHeaderFlag2 = (ACE.Entity.Enum.WeenieHeaderFlag2)(biota.GetProperty((ACE.Entity.Enum.Properties.PropertyDataId)8002) ?? (uint)ACE.Entity.Enum.WeenieHeaderFlag2.None);
                    if (weenieHeaderFlag2.HasFlag(ACE.Entity.Enum.WeenieHeaderFlag2.PetOwner))
                        if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor).HasValue && biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor) == (int)ACE.Entity.Enum.RadarColor.Yellow)
                            return ACE.Entity.Enum.WeenieType.Pet;
                        else
                            return ACE.Entity.Enum.WeenieType.CombatPet;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Pet")
                        || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Wind-up")
                        || biota.WeenieClassId == 48881
                        || biota.WeenieClassId == 34902
                        || biota.WeenieClassId == 48891
                        || biota.WeenieClassId == 48879
                        || biota.WeenieClassId == 34906
                        || biota.WeenieClassId == 48887
                        || biota.WeenieClassId == 48889
                        || biota.WeenieClassId == 48883
                        || biota.WeenieClassId == 34900
                        || biota.WeenieClassId == 34901
                        || biota.WeenieClassId == 34908
                        || biota.WeenieClassId == 34898
                        )
                        return ACE.Entity.Enum.WeenieType.Pet;
                    else if (
                        biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Cow")
                        && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Auroch")
                        && !biota.GetProperty(ACE.Entity.Enum.Properties.PropertyString.Name).Contains("Snowman")
                        )
                        return ACE.Entity.Enum.WeenieType.Cow;
                    else if (
                        biota.WeenieClassId >= 14342 && biota.WeenieClassId <= 14347
                        || biota.WeenieClassId >= 14404 && biota.WeenieClassId <= 14409
                        )
                        return ACE.Entity.Enum.WeenieType.GamePiece;
                    else
                        return ACE.Entity.Enum.WeenieType.Creature;

                case ACE.Entity.Enum.ItemType.Gameboard:
                    return ACE.Entity.Enum.WeenieType.Game;

                case ACE.Entity.Enum.ItemType.Portal:
                    if (
                        biota.WeenieClassId == 9620 || // W_PORTALHOUSE_CLASS
                        biota.WeenieClassId == 10751 || // W_PORTALHOUSETEST_CLASS
                        biota.WeenieClassId == 11730    // W_HOUSEPORTAL_CLASS
                        )
                        return ACE.Entity.Enum.WeenieType.HousePortal;
                    else
                        return ACE.Entity.Enum.WeenieType.Portal;

                case ACE.Entity.Enum.ItemType.MeleeWeapon:
                    return ACE.Entity.Enum.WeenieType.MeleeWeapon;

                case ACE.Entity.Enum.ItemType.MissileWeapon:
                    if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.AmmoType).HasValue)
                        return ACE.Entity.Enum.WeenieType.MissileLauncher;
                    else
                        return ACE.Entity.Enum.WeenieType.Missile;

                case ACE.Entity.Enum.ItemType.Money:
                    return ACE.Entity.Enum.WeenieType.Coin;
                case ACE.Entity.Enum.ItemType.Gem:
                    return ACE.Entity.Enum.WeenieType.Gem;
                case ACE.Entity.Enum.ItemType.SpellComponents:
                    return ACE.Entity.Enum.WeenieType.SpellComponent;
                case ACE.Entity.Enum.ItemType.ManaStone:
                    return ACE.Entity.Enum.WeenieType.ManaStone;
                case ACE.Entity.Enum.ItemType.TinkeringTool:
                    return ACE.Entity.Enum.WeenieType.CraftTool;
                case ACE.Entity.Enum.ItemType.Key:
                    return ACE.Entity.Enum.WeenieType.Key;
                case ACE.Entity.Enum.ItemType.PromissoryNote:
                    return ACE.Entity.Enum.WeenieType.Stackable;
                case ACE.Entity.Enum.ItemType.Writable:
                    return ACE.Entity.Enum.WeenieType.Scroll;

                default:
                    if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStructure).HasValue || biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.TargetType).HasValue)
                        return ACE.Entity.Enum.WeenieType.CraftTool;
                    else if (biota.GetProperty(ACE.Entity.Enum.Properties.PropertyInt.MaxStackSize).HasValue)
                        return ACE.Entity.Enum.WeenieType.Stackable;
                    else if (biota.BiotaPropertiesSpellBook.Count > 0)
                        return ACE.Entity.Enum.WeenieType.Switch;
                    else
                        return ACE.Entity.Enum.WeenieType.Generic;
            }
        }
	}
}
