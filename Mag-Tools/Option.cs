
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

		private static readonly Option filterCompUsage = new Option(OptionGroup.Filters, "CompUsage", "Filter Comp Usage");
		public static Option FilterCompUsage { get { return filterCompUsage; } }

		private static readonly Option filterSpellExpires = new Option(OptionGroup.Filters, "SpellExpires", "Filter Spell Expires");
		public static Option FilterSpellExpires { get { return filterSpellExpires; } }

		private static readonly Option filterNPKFails = new Option(OptionGroup.Filters, "NPKFails", "Filter NPK Fails");
		public static Option FilterNPKFails { get { return filterNPKFails; } }

		private static readonly Option filterVendorTells = new Option(OptionGroup.Filters, "VendorTells", "Filter Vendor Tells");
		public static Option FilterVendorTells { get { return filterVendorTells; } }


		private static readonly Option itemInfoOnIdent = new Option(OptionGroup.ItemInfoOnIdent, "Enabled", "Show Item Info On Ident");
		public static Option ItemInfoOnIdent { get { return itemInfoOnIdent; } }


		private static readonly Option autoBuySellEnabled = new Option(OptionGroup.AutoBuySell, "Enabled", "Auto Buy/Sell Enabled");
		public static Option AutoBuySellEnabled { get { return autoBuySellEnabled; } }


		private static readonly Option autoTradeAcceptEnabled = new Option(OptionGroup.AutoTradeAccept, "Enabled", "Auto Trade Accept Enabled");
		public static Option AutoTradeAcceptEnabled { get { return autoTradeAcceptEnabled; } }


		private static readonly Option openMainPackOnLogin = new Option(OptionGroup.Misc, "OpenMainPackOnLogin", "Open Main Pack On Login");
		public static Option OpenMainPackOnLogin { get { return openMainPackOnLogin; } }

		private static readonly Option debuggingEnabled = new Option(OptionGroup.Misc, "DebuggingEnabled", "Debugging Enabled");
		public static Option DebuggingEnabled { get { return debuggingEnabled; } }
	}
}
