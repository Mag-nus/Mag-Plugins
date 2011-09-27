using System;

namespace MagTools
{
	class OptionGroup
	{
		public readonly string Xpath;

		public readonly string Name;

		private OptionGroup(string xpath, string name)
		{
			this.Xpath = xpath;

			this.Name = name;
		}

		private static readonly OptionGroup manaMagement = new OptionGroup("ManaManagement", "Mana Management");
		public static OptionGroup ManaMagement { get { return manaMagement; } }

		private static readonly OptionGroup filters = new OptionGroup("Filters", "Filters");
		public static OptionGroup Filters { get { return filters; } }

		private static readonly OptionGroup itemInfoOnIdent = new OptionGroup("ItemInfoOnIdent", "Item Info On Ident");
		public static OptionGroup ItemInfoOnIdent { get { return itemInfoOnIdent; } }

		private static readonly OptionGroup autoBuySell = new OptionGroup("AutoBuySell", "Auto Buy/Sell");
		public static OptionGroup AutoBuySell { get { return autoBuySell; } }

		private static readonly OptionGroup autoTradeAdd = new OptionGroup("AutoTradeAdd", "Auto Add To Trade");
		public static OptionGroup AutoTradeAdd { get { return autoTradeAdd; } }

		private static readonly OptionGroup autoTradeAccept = new OptionGroup("AutoTradeAccept", "Auto Trade Accept");
		public static OptionGroup AutoTradeAccept { get { return autoTradeAccept; } }

		private static readonly OptionGroup looting = new OptionGroup("Looting", "Looting");
		public static OptionGroup Looting { get { return looting; } }

		private static readonly OptionGroup misc = new OptionGroup("Misc", "Misc");
		public static OptionGroup Misc { get { return misc; } }
	}
}
