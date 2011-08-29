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

		private static readonly OptionGroup filters = new OptionGroup("Filters", "Filters");
		public static OptionGroup Filters { get { return filters; } }

		private static readonly OptionGroup itemInfoOnIdent = new OptionGroup("ItemInfoOnIdent", "Item Info On Ident");
		public static OptionGroup ItemInfoOnIdent { get { return itemInfoOnIdent; } }

		private static readonly OptionGroup misc = new OptionGroup("Misc", "Misc");
		public static OptionGroup Misc { get { return misc; } }
	}
}
