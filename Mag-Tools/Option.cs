
namespace MagTools
{
	class Option
	{
		public readonly OptionGroup OptionGroup;

		private readonly string xpath;

		public string Xpath
		{
			get
			{
				return OptionGroup.Xpath + "/" + xpath;
			}
		}

		public readonly string Name;

		private Option(OptionGroup optionGroup, string xpath, string name)
		{
			this.OptionGroup = optionGroup;

			this.xpath = xpath;

			this.Name = name;
		}

		private static readonly Option autoRechargeMana = new Option(OptionGroup.ManaMagement, "AutoRecharge", "Auto Recharge Mana");
		public static Option AutoRechargeMana { get { return autoRechargeMana; } }

		private static readonly Option filterAttackEvades = new Option(OptionGroup.Filters, "AttackEvades", "Filter Attack Evades");
		public static Option FilterAttackEvades { get { return filterAttackEvades; } }

		private static readonly Option filterDefenseEvades = new Option(OptionGroup.Filters, "DefenseEvades", "Filter Defense Evades");
		public static Option FilterDefenseEvades { get { return filterDefenseEvades; } }

		private static readonly Option filterAttackResists = new Option(OptionGroup.Filters, "AttackResists", "Filter Attack Resists");
		public static Option FilterAttackResists { get { return filterAttackResists; } }

		private static readonly Option filterDefenseResists = new Option(OptionGroup.Filters, "DefenseResists", "Filter Defense Resists");
		public static Option FilterDefenseResists { get { return filterDefenseResists; } }

		private static readonly Option filterSpellCasting = new Option(OptionGroup.Filters, "SpellCasting", "Filter Spell Casting");
		public static Option FilterSpellCasting { get { return filterSpellCasting; } }

		private static readonly Option filterSpellCastFizzles = new Option(OptionGroup.Filters, "SpellCastFizzles", "Filter Spell Cast Fizzles");
		public static Option FilterSpellCastFizzles { get { return filterSpellCastFizzles; } }

		private static readonly Option filterCompUsage = new Option(OptionGroup.Filters, "CompUsage", "Filter Comp Usage");
		public static Option FilterCompUsage { get { return filterCompUsage; } }

		private static readonly Option filterSpellExpires = new Option(OptionGroup.Filters, "SpellExpires", "Filter Spell Expires");
		public static Option FilterSpellExpires { get { return filterSpellExpires; } }

		private static readonly Option filterNPKFails = new Option(OptionGroup.Filters, "NPKFails", "Filter NPK Fails");
		public static Option FilterNPKFails { get { return filterNPKFails; } }

		private static readonly Option filterVendorTells = new Option(OptionGroup.Filters, "VendorTells", "Filter Vendor Tells");
		public static Option FilterVendorTells { get { return filterVendorTells; } }

		private static readonly Option filterHealingKitSuccess = new Option(OptionGroup.Filters, "HealingKitSuccess", "Filter Healing Kit Success");
		public static Option FilterHealingKitSuccess { get { return filterHealingKitSuccess; } }

		private static readonly Option filterHealingKitFail = new Option(OptionGroup.Filters, "HealingKitFail", "Filter Healing Kit Fail");
		public static Option FilterHealingKitFail { get { return filterHealingKitFail; } }

		private static readonly Option filterMonsterDeaths = new Option(OptionGroup.Filters, "MonsterDeaths", "Filter Monster Deaths");
		public static Option FilterMonsterDeaths { get { return filterMonsterDeaths; } }

		private static readonly Option filterSalvaging = new Option(OptionGroup.Filters, "Salvaging", "Filter Salvaging");
		public static Option FilterSalvaging { get { return filterSalvaging; } }

		private static readonly Option filterSalvagingFails = new Option(OptionGroup.Filters, "SalvagingFails", "Filter Salvaging Fails");
		public static Option FilterSalvagingFails { get { return filterSalvagingFails; } }

		private static readonly Option tradeBuffBotSpam = new Option(OptionGroup.Filters, "TradeBuffBotSpam", "Filter Trade/Buff Bot Spam");
		public static Option TradeBuffBotSpam { get { return tradeBuffBotSpam; } }


		private static readonly Option itemInfoOnIdent = new Option(OptionGroup.ItemInfoOnIdent, "Enabled", "Show Item Info On Ident");
		public static Option ItemInfoOnIdent { get { return itemInfoOnIdent; } }


		private static readonly Option autoBuySellEnabled = new Option(OptionGroup.AutoBuySell, "Enabled", "Auto Buy/Sell Enabled");
		public static Option AutoBuySellEnabled { get { return autoBuySellEnabled; } }


		private static readonly Option autoTradeAdd = new Option(OptionGroup.AutoTradeAdd, "Enabled", "Auto Add To Trade Enabled");
		public static Option AutoTradeAdd { get { return autoTradeAdd; } }


		private static readonly Option autoTradeAcceptEnabled = new Option(OptionGroup.AutoTradeAccept, "Enabled", "Auto Trade Accept Enabled");
		public static Option AutoTradeAcceptEnabled { get { return autoTradeAcceptEnabled; } }


		private static readonly Option chestLooterEnabled = new Option(OptionGroup.Looting, "Enabled", "Auto Loot Chests");
		public static Option ChestLooterEnabled { get { return chestLooterEnabled; } }


		private static readonly Option openMainPackOnLogin = new Option(OptionGroup.Misc, "OpenMainPackOnLogin", "Open Main Pack On Login");
		public static Option OpenMainPackOnLogin { get { return openMainPackOnLogin; } }

		private static readonly Option removeWindowFrame = new Option(OptionGroup.Misc, "RemoveWindowFrame", "Remove Window Frame On Login");
		public static Option RemoveWindowFrame { get { return removeWindowFrame; } }

		private static readonly Option debuggingEnabled = new Option(OptionGroup.Misc, "DebuggingEnabled", "Debugging Enabled");
		public static Option DebuggingEnabled { get { return debuggingEnabled; } }
	}
}
