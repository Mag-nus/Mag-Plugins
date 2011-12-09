using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Xml;

namespace MagTools.Settings
{
	static class SettingsManager
	{
		public static class ManaManagement
		{
			public static readonly Setting<bool> AutoRecharge = new Setting<bool>("ManaManagement/AutoRecharge", "Auto Recharge Mana", true);
		}

		public static class AutoBuySell
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoBuySell/Enabled", "Auto Buy/Sell Enabled", true);
		}

		public static class AutoTradeAdd
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoTradeAdd/Enabled", "Auto Add To Trade Enabled");
		}

		public static class AutoTradeAccept
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("AutoTradeAccept/Enabled", "Auto Trade Accept Enabled");

			public static IEnumerable<Regex> Whitelist
			{
				get
				{
					Collection<Regex> whitelist = new Collection<Regex>();

					IEnumerable<string> collection = SettingsFile.GetCollection("AutoTradeAccept/Whitelist");

					foreach (string s in collection)
					{
						whitelist.Add(new Regex("^" + s + "$"));
					}

					return whitelist;
				}
			}
		}

		public static class Looting
		{
			static Looting()
			{
				// In 1.0.8.7 we added corpse looting
				if (Enabled.Value)
				{
					AutoLootChests.Value = true;
					Enabled.Value = false;
				}
			}

			public static readonly Setting<bool> Enabled = new Setting<bool>("Looting/Enabled", "Auto Loot Chests");

			public static readonly Setting<bool> AutoLootChests = new Setting<bool>("Looting/AutoLootChests", "Auto Loot Chests");

			public static readonly Setting<bool> AutoLootCorpses = new Setting<bool>("Looting/AutoLootCorpses", "Auto Loot Corpses");
		}

		public static class ItemInfoOnIdent
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("ItemInfoOnIdent/Enabled", "Show Item Info On Ident", true);

			public static readonly Setting<bool> ShowBuffedValues = new Setting<bool>("ItemInfoOnIdent/ShowBuffedValues", "Show Item Info Buffed* Values", true);

			public static readonly Setting<bool> LeftClickIdent = new Setting<bool>("ItemInfoOnIdent/LeftClickIdent", "Ident Items on Left Click", true);

			public static readonly Setting<bool> AutoClipboard = new Setting<bool>("ItemInfoOnIdent/AutoClipboard", "Clipboard Item Info On Ident");
		}

		public static class CombatTracker
		{
			public static readonly Setting<bool> Persistent = new Setting<bool>("CombatTracker/Persistent", "Keep Stats Persistent");

			public static readonly Setting<bool> ExportOnLogOff = new Setting<bool>("CombatTracker/ExportOnLogOff", "Export Stats on LogOff");
		}

		public static class Misc
		{
			public static readonly Setting<bool> OpenMainPackOnLogin = new Setting<bool>("Misc/OpenMainPackOnLogin", "Open Main Pack On Login");

			public static readonly Setting<bool> RemoveWindowFrame = new Setting<bool>("Misc/RemoveWindowFrame", "Remove Window Frame");

			public static readonly Setting<bool> DebuggingEnabled = new Setting<bool>("Misc/DebuggingEnabled", "Debugging Enabled", true);

			public static Collection<Client.WindowPosition> WindowPositions
			{
				get
				{
					Collection<Client.WindowPosition> windowPositions = new Collection<Client.WindowPosition>();

					XmlNode node = SettingsFile.GetNode("Misc/WindowPositions");

					if (node != null)
					{
						foreach (XmlNode childNode in node.ChildNodes)
						{
							if (childNode.Attributes != null)
							{
								Client.WindowPosition windowPosition;

								windowPosition.Server = childNode.Attributes["Server"].Value;
								windowPosition.AccountName = childNode.Attributes["AccountName"].Value;

								int.TryParse(childNode.Attributes["X"].Value, out windowPosition.X);
								int.TryParse(childNode.Attributes["Y"].Value, out windowPosition.Y);

								windowPositions.Add(windowPosition);
							}
						}
					}

					return windowPositions;
				}
			}

			public static void SetWindowPosition(Client.WindowPosition newWindowPosition)
			{
				SettingsFile.ReloadXmlDocument();

				Collection<Client.WindowPosition> windowPositions = WindowPositions;

				for (int i = 0 ; i < windowPositions.Count ; i++)
				{
					if (windowPositions[i].Server == newWindowPosition.Server && windowPositions[i].AccountName == newWindowPosition.AccountName)
					{
						windowPositions.RemoveAt(i);

						break;
					}
				}

				Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

				foreach (Client.WindowPosition windowPosition in windowPositions)
				{
					Dictionary<string, string> attributes = new Dictionary<string, string>();

					attributes.Add("Server", windowPosition.Server);
					attributes.Add("AccountName", windowPosition.AccountName);

					attributes.Add("X", windowPosition.X.ToString());
					attributes.Add("Y", windowPosition.Y.ToString());

					collection.Add(attributes);
				}

				Dictionary<string, string> newAttributes = new Dictionary<string, string>();

				newAttributes.Add("Server", newWindowPosition.Server);
				newAttributes.Add("AccountName", newWindowPosition.AccountName);

				newAttributes.Add("X", newWindowPosition.X.ToString());
				newAttributes.Add("Y", newWindowPosition.Y.ToString());

				collection.Add(newAttributes);

				SettingsFile.SetNodeChilderen("Misc/WindowPositions", "WindowPosition", collection);
			}

			public static void DeleteWindowPosition(string server, string accountName)
			{
				SettingsFile.ReloadXmlDocument();

				Collection<Client.WindowPosition> windowPositions = WindowPositions;

				for (int i = 0 ; i < windowPositions.Count ; i++)
				{
					if (windowPositions[i].Server == server && windowPositions[i].AccountName == accountName)
					{
						windowPositions.RemoveAt(i);

						Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

						foreach (Client.WindowPosition windowPosition in windowPositions)
						{
							Dictionary<string, string> attributes = new Dictionary<string, string>();

							attributes.Add("Server", windowPosition.Server);
							attributes.Add("AccountName", windowPosition.AccountName);

							attributes.Add("X", windowPosition.X.ToString());
							attributes.Add("Y", windowPosition.Y.ToString());

							collection.Add(attributes);
						}

						SettingsFile.SetNodeChilderen("Misc/WindowPositions", "WindowPosition", collection);

						break;
					}
				}
			}
		}

		public static class Filters
		{
			static Filters()
			{
				// In 1.0.8.5 we split SpellCasting into SpellCastingMine and SpellCastingOthers
				if (SpellCasting.Value)
				{
					SpellCastingMine.Value = true;
					SpellCastingOthers.Value = true;
					SpellCasting.Value = false;
				}
			}

			public static readonly Setting<bool> AttackEvades = new Setting<bool>("Filters/AttackEvades", "Attack Evades");

			public static readonly Setting<bool> DefenseEvades = new Setting<bool>("Filters/DefenseEvades", "Defense Evades");

			public static readonly Setting<bool> AttackResists = new Setting<bool>("Filters/AttackResists", "Attack Resists");

			public static readonly Setting<bool> DefenseResists = new Setting<bool>("Filters/DefenseResists", "Defense Resists");

			public static readonly Setting<bool> NPKFails = new Setting<bool>("Filters/NPKFails", "NPK Fails");

			public static readonly Setting<bool> MonsterDeaths = new Setting<bool>("Filters/MonsterDeaths", "Monster Deaths");


			public static readonly Setting<bool> SpellCasting = new Setting<bool>("Filters/SpellCasting", "Spell Casting");

			public static readonly Setting<bool> SpellCastingMine = new Setting<bool>("Filters/SpellCastingMine", "Spell Casting - Mine");

			public static readonly Setting<bool> SpellCastingOthers = new Setting<bool>("Filters/SpellCastingOthers", "Spell Casting - Others");

			public static readonly Setting<bool> SpellCastFizzles = new Setting<bool>("Filters/SpellCastFizzles", "Spell Cast Fizzles");

			public static readonly Setting<bool> CompUsage = new Setting<bool>("Filters/CompUsage", "Comp Usage");

			public static readonly Setting<bool> SpellExpires = new Setting<bool>("Filters/SpellExpires", "Spell Expires");


			public static readonly Setting<bool> HealingKitSuccess = new Setting<bool>("Filters/HealingKitSuccess", "Healing Kit Success");

			public static readonly Setting<bool> HealingKitFail = new Setting<bool>("Filters/HealingKitFail", "Healing Kit Fail");

			public static readonly Setting<bool> Salvaging = new Setting<bool>("Filters/Salvaging", "Salvaging");

			public static readonly Setting<bool> SalvagingFails = new Setting<bool>("Filters/SalvagingFails", "Salvaging Fails");

			public static readonly Setting<bool> ManaStoneUsage = new Setting<bool>("Filters/ManaStoneUsage", "Mana Stone Usage");


			public static readonly Setting<bool> TradeBuffBotSpam = new Setting<bool>("Filters/TradeBuffBotSpam", "Trade/Buff Bot Spam");

			public static readonly Setting<bool> FailedAssess = new Setting<bool>("Filters/FailedAssess", "Someone failed to assess you");


			public static readonly Setting<bool> KillTaskComplete = new Setting<bool>("Filters/KillTaskComplete", "Kill Task Complete");

			public static readonly Setting<bool> VendorTells = new Setting<bool>("Filters/VendorTells", "Vendor Tells");

			public static readonly Setting<bool> MonsterTell = new Setting<bool>("Filters/MonsterTell", "Monster Tells");

			public static readonly Setting<bool> NpcChatter = new Setting<bool>("Filters/NPCChatter", "NPC Chatter");

			public static readonly Setting<bool> MasterArbitratorSpam = new Setting<bool>("Filters/MasterArbitratorSpam", "Master Arbitrator Spam");

			public static readonly Setting<bool> AllMasterArbitratorChat = new Setting<bool>("Filters/AllMasterArbitratorChat", "All Master Arbitrator Chat");
		}
	}
}
