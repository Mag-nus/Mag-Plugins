
namespace MagTools.Settings
{
	static class SettingsManager
	{
		public static class ManaManagement
		{
			public static readonly Setting<bool> AutoRecharge = new Setting<bool>("ManaManagement/AutoRecharge", "Auto Recharge Mana");
		}

		public static class AutoBuySell
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoBuySell/Enabled", "Auto Buy/Sell Enabled");
		}

		public static class AutoTradeAdd
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoTradeAdd/Enabled", "Auto Add To Trade Enabled");
		}

		public static class AutoTradeAccept
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoTradeAccept/Enabled", "Auto Trade Accept Enabled");
		}

		public static class Looting
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("Looting/Enabled", "Auto Loot Chests");
		}

		public static class ItemInfoOnIdent
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("ItemInfoOnIdent/Enabled", "Show Item Info On Ident");

			public static readonly Setting<bool> AutoClipboard = new Setting<bool>("ItemInfoOnIdent/AutoClipboard", "Clipboard Item Info On Ident");
		}

		public static class Misc
		{
			public static readonly Setting<bool> OpenMainPackOnLogin = new Setting<bool>("Misc/OpenMainPackOnLogin", "Open Main Pack On Login");

			public static readonly Setting<bool> RemoveWindowFrame = new Setting<bool>("Misc/RemoveWindowFrame", "Remove Window Frame");

			public static readonly Setting<bool> DebuggingEnabled = new Setting<bool>("Misc/DebuggingEnabled", "Debugging Enabled");
		}

		public static class Filters
		{
			public static readonly Setting<bool> AttackEvades = new Setting<bool>("Filters/AttackEvades", "Attack Evades");

			public static readonly Setting<bool> DefenseEvades = new Setting<bool>("Filters/DefenseEvades", "Defense Evades");

			public static readonly Setting<bool> AttackResists = new Setting<bool>("Filters/AttackResists", "Attack Resists");

			public static readonly Setting<bool> DefenseResists = new Setting<bool>("Filters/DefenseResists", "Defense Resists");

			public static readonly Setting<bool> SpellCasting = new Setting<bool>("Filters/SpellCasting", "Spell Casting");

			public static readonly Setting<bool> SpellCastFizzles = new Setting<bool>("Filters/SpellCastFizzles", "Spell Cast Fizzles");

			public static readonly Setting<bool> CompUsage = new Setting<bool>("Filters/CompUsage", "Comp Usage");

			public static readonly Setting<bool> SpellExpires = new Setting<bool>("Filters/SpellExpires", "Spell Expires");

			public static readonly Setting<bool> NPKFails = new Setting<bool>("Filters/NPKFails", "NPK Fails");

			public static readonly Setting<bool> VendorTells = new Setting<bool>("Filters/VendorTells", "Vendor Tells");

			public static readonly Setting<bool> HealingKitSuccess = new Setting<bool>("Filters/HealingKitSuccess", "Healing Kit Success");

			public static readonly Setting<bool> HealingKitFail = new Setting<bool>("Filters/HealingKitFail", "Healing Kit Fail");

			public static readonly Setting<bool> MonsterDeaths = new Setting<bool>("Filters/MonsterDeaths", "Monster Deaths");

			public static readonly Setting<bool> Salvaging = new Setting<bool>("Filters/Salvaging", "Salvaging");

			public static readonly Setting<bool> SalvagingFails = new Setting<bool>("Filters/SalvagingFails", "Salvaging Fails");

			public static readonly Setting<bool> TradeBuffBotSpam = new Setting<bool>("Filters/TradeBuffBotSpam", "Trade/Buff Bot Spam");

			public static readonly Setting<bool> KillTaskComplete = new Setting<bool>("Filters/KillTaskComplete", "Kill Task Complete");

			public static readonly Setting<bool> FailedAssess = new Setting<bool>("Filters/FailedAssess", "Someone failed to assess you");

			public static readonly Setting<bool> NpcChatter = new Setting<bool>("Filters/NPCChatter", "NPC Chatter");
		}
	}
}
