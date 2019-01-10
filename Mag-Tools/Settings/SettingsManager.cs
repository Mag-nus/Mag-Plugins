using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

using Mag.Shared.Settings;

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

			public static readonly Setting<bool> TestMode = new Setting<bool>("AutoBuySell/TestMode", "- Auto Buy/Sell Test Mode");

			static AutoBuySell()
			{
				TestMode.Changed += new System.Action<Setting<bool>>(ChildOption_Changed);
			}

			static void ChildOption_Changed(Setting<bool> obj)
			{
				if (obj.Value)
					Enabled.Value = true;
			}
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

					IEnumerable<string> collection = SettingsFile.GetChilderenInnerTexts("AutoTradeAccept/Whitelist");

					foreach (string s in collection)
						whitelist.Add(new Regex("^" + s + "$"));

					return whitelist;
				}
			}
		}

		public static class Looting
		{
			public static readonly Setting<bool> AutoLootChests = new Setting<bool>("Looting/AutoLootChests", "Auto Loot Chests", true);

			public static readonly Setting<bool> AutoLootCorpses = new Setting<bool>("Looting/AutoLootCorpses", "Auto Loot Corpses", true);

			public static readonly Setting<bool> AutoLootMyCorpses = new Setting<bool>("Looting/AutoLootMyCorpse", "Auto Loot My Corpses", true);

			public static readonly Setting<bool> LootSalvage = new Setting<bool>("Looting/LootSalvage", "Auto Loot Salvage");
		}

		public static class Tinkering
		{
			public static readonly Setting<bool> AutoClickYes = new Setting<bool>("Tinkering/AutoClickYes", "Auto Click Yes on 100%");
		}

		public static class InventoryManagement
		{
			public static readonly Setting<bool> InventoryLogger = new Setting<bool>("InventoryManagement/InventoryLogger", "Inventory Logger Enabled");

			public static readonly Setting<bool> AetheriaRevealer = new Setting<bool>("InventoryManagement/AetheriaRevealer", "Auto Reveal Aetheria", true);

			public static readonly Setting<bool> HeartCarver = new Setting<bool>("InventoryManagement/HeartCarver", "Auto Carve Hearts");

			public static readonly Setting<bool> ShatteredKeyFixer = new Setting<bool>("InventoryManagement/ShatteredKeyFixer", "Auto Fix Shattered Keys");

			public static readonly Setting<bool> KeyRinger = new Setting<bool>("InventoryManagement/KeyRinger", "Auto Ring Keys");

			public static readonly Setting<bool> KeyDeringer = new Setting<bool>("InventoryManagement/KeyDeringer", "Auto Dering Keys");
		}

		public static class ItemInfoOnIdent
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("ItemInfoOnIdent/Enabled", "Show Item Info On Ident", true);

			public static readonly Setting<bool> ShowBuffedValues = new Setting<bool>("ItemInfoOnIdent/ShowBuffedValues", "- Show Item Info Buffed* Values", true);

			public static readonly Setting<bool> ShowValueAndBurden = new Setting<bool>("ItemInfoOnIdent/ShowValueAndBurden", "- Show Value and Burden");

			public static readonly Setting<bool> LeftClickIdent = new Setting<bool>("ItemInfoOnIdent/LeftClickIdent", "- Ident Items on Left Click");

			public static readonly Setting<bool> AutoClipboard = new Setting<bool>("ItemInfoOnIdent/AutoClipboard", "- Clipboard Item Info On Ident");

			static ItemInfoOnIdent()
			{
				ShowBuffedValues.Changed += new System.Action<Setting<bool>>(ChildOption_Changed);
				ShowValueAndBurden.Changed += new System.Action<Setting<bool>>(ChildOption_Changed);
				LeftClickIdent.Changed += new System.Action<Setting<bool>>(ChildOption_Changed);
				AutoClipboard.Changed += new System.Action<Setting<bool>>(ChildOption_Changed);
			}

			static void ChildOption_Changed(Setting<bool> obj)
			{
				if (obj.Value)
					Enabled.Value = true;
			}
		}

		public static class CombatTracker
		{
			public static readonly Setting<bool> Persistent = new Setting<bool>("CombatTracker/Persistent", "Keep Stats Persistent", true);

			public static readonly Setting<bool> ExportOnLogOff = new Setting<bool>("CombatTracker/ExportOnLogOff", "Export Stats on LogOff");

			public static readonly Setting<bool> SortAlphabetically = new Setting<bool>("CombatTracker/SortAlphabetically", "Sort Alphabetically");
		}

		public static class CorpseTracker
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("CorpseTracker/Enabled", "Corpse Tracker Enabled", true);

			public static readonly Setting<bool> Persistent = new Setting<bool>("CorpseTracker/Persistent", "Keep Stats Persistent", true);

			public static readonly Setting<bool> TrackAllCorpses = new Setting<bool>("CorpseTracker/TrackAllCorpses", "Track All Corpses");

			public static readonly Setting<bool> TrackFellowCorpses = new Setting<bool>("CorpseTracker/TrackFellowCorpses", "Track Fellow Corpses");

			public static readonly Setting<bool> TrackPermittedCorpses = new Setting<bool>("CorpseTracker/TrackPermittedCorpses", "Track Permitted Corpses", true);
		}

		public static class PlayerTracker
		{
			public static readonly Setting<bool> Enabled = new Setting<bool>("PlayerTracker/Enabled", "Player Tracker Enabled");

			public static readonly Setting<bool> Persistent = new Setting<bool>("PlayerTracker/Persistent", "Keep Stats Persistent");
		}

		public static class ChatLogger
		{
			public static readonly Setting<bool> Persistent = new Setting<bool>("ChatLogger/Persistent", "Keep Logs Persistent", true);

			public class Group
			{
				public Group(int groupNumber)
				{
					Area = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Area", "Area");
					if (groupNumber == 1)
						Tells = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Tells", "Tells", true);
					else
						Tells = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Tells", "Tells");
					Fellowship = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Fellowship", "Fellowship");
					General = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/General", "General");
					Trade = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Trade", "Trade");
					Allegiance = new Setting<bool>("ChatLogger/Group" + groupNumber.ToString(CultureInfo.InvariantCulture) + "/Allegiance", "Allegiance");
				}

				public readonly Setting<bool> Area;
				public readonly Setting<bool> Tells;
				public readonly Setting<bool> Fellowship;
				public readonly Setting<bool> General;
				public readonly Setting<bool> Trade;
				public readonly Setting<bool> Allegiance;
			}

			public static readonly Group[] Groups = new Group[2];

			static ChatLogger()
			{
				Groups[0] = new Group(1);
				Groups[1] = new Group(2);
			}
		}

		public struct PeriodicCommand
		{
			public readonly string Command;
			public readonly TimeSpan Interval;
			public readonly TimeSpan OffsetFromMidnight;

			public PeriodicCommand(string command, TimeSpan interval, TimeSpan offsetFromMidnight)
			{
				Command = command;
				Interval = interval;
				OffsetFromMidnight = offsetFromMidnight;
			}
		}

		public static class AccountServerCharacter
		{
			public static IList<string> GetOnLoginCommands(string account, string server, string character)
			{
				return SettingsFile.GetChilderenInnerTexts("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/OnLoginCommands");
			}

			public static void SetOnLoginCommands(string account, string server, string character, IList<string> commands)
			{
				SettingsFile.SetNodeChilderen("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/OnLoginCommands", "Command", commands);
			}

			public static IList<string> GetOnLoginCompleteCommands(string account, string server, string character)
			{
				return SettingsFile.GetChilderenInnerTexts("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/OnLoginCompleteCommands");
			}

			public static void SetOnLoginCompleteCommands(string account, string server, string character, IList<string> commands)
			{
				SettingsFile.SetNodeChilderen("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/OnLoginCompleteCommands", "Command", commands);
			}

			public static IList<PeriodicCommand> GetPeriodicCommands(string account, string server, string character)
			{
				var commands = new List<PeriodicCommand>();

				var xmlNode = SettingsFile.GetNode("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/PeriodicCommands");

				if (xmlNode != null && xmlNode.HasChildNodes)
				{
					foreach (XmlNode childNode in xmlNode.ChildNodes)
					{
						int interval = 0;
						int offset = 0;

						if (childNode.Attributes != null)
						{
							int.TryParse(childNode.Attributes["interval"].Value, out interval);
							int.TryParse(childNode.Attributes["offset"].Value, out offset);
						}


						commands.Add(new PeriodicCommand(childNode.InnerText, TimeSpan.FromMinutes(interval), TimeSpan.FromMinutes(offset)));
					}
				}

				return commands;
			}

			public static void SetPeriodicCommands(string account, string server, string character, IList<PeriodicCommand> commands)
			{
				SettingsFile.ReloadXmlDocument();

				var xmlNode = SettingsFile.GetNode("_" + account + "_" + XmlConvert.EncodeName(server) + "/" + XmlConvert.EncodeName(character) + "/PeriodicCommands", true);

				xmlNode.RemoveAll();

				foreach (var command in commands)
				{
					XmlNode childNode = xmlNode.AppendChild(SettingsFile.XmlDocument.CreateElement("PeriodicCommand"));

					childNode.InnerText = command.Command;

					XmlAttribute attribute = SettingsFile.XmlDocument.CreateAttribute("offset");
					attribute.Value = command.OffsetFromMidnight.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					if (childNode.Attributes != null) childNode.Attributes.Append(attribute);

					attribute = SettingsFile.XmlDocument.CreateAttribute("interval");
					attribute.Value = command.Interval.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					if (childNode.Attributes != null) childNode.Attributes.Append(attribute);
				}

				SettingsFile.SaveXmlDocument();
			}
		}

		public static class Server
		{
			public static IList<string> GetOnLoginCommands(string server)
			{
				return SettingsFile.GetChilderenInnerTexts("_" + XmlConvert.EncodeName(server) + "/OnLoginCommands");
			}

			public static void SetOnLoginCommands(string server, IList<string> commands)
			{
				SettingsFile.SetNodeChilderen("_" + XmlConvert.EncodeName(server) + "/OnLoginCommands", "Command", commands);
			}

			public static IList<string> GetOnLoginCompleteCommands(string server)
			{
				return SettingsFile.GetChilderenInnerTexts("_" + XmlConvert.EncodeName(server) + "/OnLoginCompleteCommands");
			}

			public static void SetOnLoginCompleteCommands(string server, IList<string> commands)
			{
				SettingsFile.SetNodeChilderen("_" + XmlConvert.EncodeName(server) + "/OnLoginCompleteCommands", "Command", commands);
			}

			public static IList<PeriodicCommand> GetPeriodicCommands(string server)
			{
				var commands = new List<PeriodicCommand>();

				var xmlNode = SettingsFile.GetNode("_" + XmlConvert.EncodeName(server) + "/PeriodicCommands");

				if (xmlNode != null && xmlNode.HasChildNodes)
				{
					foreach (XmlNode childNode in xmlNode.ChildNodes)
					{
						int interval = 0;
						int offset = 0;

						if (childNode.Attributes != null)
						{
							int.TryParse(childNode.Attributes["interval"].Value, out interval);
							int.TryParse(childNode.Attributes["offset"].Value, out offset);
						}


						commands.Add(new PeriodicCommand(childNode.InnerText, TimeSpan.FromMinutes(interval), TimeSpan.FromMinutes(offset)));
					}
				}

				return commands;
			}

			public static void SetPeriodicCommands(string server, IList<PeriodicCommand> commands)
			{
				SettingsFile.ReloadXmlDocument();

				var xmlNode = SettingsFile.GetNode("_" + XmlConvert.EncodeName(server) + "/PeriodicCommands", true);

				xmlNode.RemoveAll();

				foreach (var command in commands)
				{
					XmlNode childNode = xmlNode.AppendChild(SettingsFile.XmlDocument.CreateElement("PeriodicCommand"));

					childNode.InnerText = command.Command;

					XmlAttribute attribute = SettingsFile.XmlDocument.CreateAttribute("offset");
					attribute.Value = command.OffsetFromMidnight.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					if (childNode.Attributes != null) childNode.Attributes.Append(attribute);

					attribute = SettingsFile.XmlDocument.CreateAttribute("interval");
					attribute.Value = command.Interval.TotalMinutes.ToString(CultureInfo.InvariantCulture);
					if (childNode.Attributes != null) childNode.Attributes.Append(attribute);
				}

				SettingsFile.SaveXmlDocument();
			}
		}

		public static class Misc
		{
			public static readonly Setting<bool> OpenMainPackOnLogin = new Setting<bool>("Misc/OpenMainPackOnLogin", "Open Main Pack On Login", true);

			public static readonly Setting<bool> MaximizeChatOnLogin = new Setting<bool>("Misc/MaximizeChatOnLogin", "Maximize Chat On Login");

			public static readonly Setting<bool> RemoveWindowFrame = new Setting<bool>("Misc/RemoveWindowFrame", "Remove Window Frame");

			public static readonly Setting<bool> LogOutOnDeath = new Setting<bool>("Misc/LogOutOnDeath", "Log Out on Death");

			public static readonly Setting<bool> DebuggingEnabled = new Setting<bool>("Misc/DebuggingEnabled", "Debugging Enabled", true);

			public static readonly Setting<bool> VerboseDebuggingEnabled = new Setting<bool>("Misc/VerboseDebuggingEnabled", "Verbose Debugging Enabled");

			public static readonly Setting<int> OutputTargetWindow = new Setting<int>("Misc/OutputTargetWindow", "Output Window", 1);

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

					attributes.Add("X", windowPosition.X.ToString(CultureInfo.InvariantCulture));
					attributes.Add("Y", windowPosition.Y.ToString(CultureInfo.InvariantCulture));

					collection.Add(attributes);
				}

				Dictionary<string, string> newAttributes = new Dictionary<string, string>();

				newAttributes.Add("Server", newWindowPosition.Server);
				newAttributes.Add("AccountName", newWindowPosition.AccountName);

				newAttributes.Add("X", newWindowPosition.X.ToString(CultureInfo.InvariantCulture));
				newAttributes.Add("Y", newWindowPosition.Y.ToString(CultureInfo.InvariantCulture));

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

							attributes.Add("X", windowPosition.X.ToString(CultureInfo.InvariantCulture));
							attributes.Add("Y", windowPosition.Y.ToString(CultureInfo.InvariantCulture));

							collection.Add(attributes);
						}

						SettingsFile.SetNodeChilderen("Misc/WindowPositions", "WindowPosition", collection);

						break;
					}
				}
			}

			public static readonly Setting<int> NoFocusFPS = new Setting<int>("Misc/NoFocusFPS", "No Focus FPS", 10);

			public static readonly Setting<int> MaxFPS = new Setting<int>("Misc/MaxFPS", "Max FPS", 0);
		}

		public static class Filters
		{
			public static readonly Setting<bool> AttackEvades = new Setting<bool>("Filters/AttackEvades", "Attack Evades");

			public static readonly Setting<bool> DefenseEvades = new Setting<bool>("Filters/DefenseEvades", "Defense Evades");

			public static readonly Setting<bool> AttackResists = new Setting<bool>("Filters/AttackResists", "Attack Resists");

			public static readonly Setting<bool> DefenseResists = new Setting<bool>("Filters/DefenseResists", "Defense Resists");

			public static readonly Setting<bool> NPKFails = new Setting<bool>("Filters/NPKFails", "NPK Fails");

			public static readonly Setting<bool> DirtyFighting = new Setting<bool>("Filters/DirtyFighting", "Dirty Fighting");

			public static readonly Setting<bool> MonsterDeaths = new Setting<bool>("Filters/MonsterDeaths", "Monster Deaths");


			public static readonly Setting<bool> SpellCastingMine = new Setting<bool>("Filters/SpellCastingMine", "Spell Casting - Mine");

			public static readonly Setting<bool> SpellCastingOthers = new Setting<bool>("Filters/SpellCastingOthers", "Spell Casting - Others");

			public static readonly Setting<bool> SpellCastFizzles = new Setting<bool>("Filters/SpellCastFizzles", "Spell Cast Fizzles");

			public static readonly Setting<bool> CompUsage = new Setting<bool>("Filters/CompUsage", "Comp Usage");

			public static readonly Setting<bool> SpellExpires = new Setting<bool>("Filters/SpellExpires", "Spell Expires");


			public static readonly Setting<bool> HealingKitSuccess = new Setting<bool>("Filters/HealingKitSuccess", "Healing Kit Success");

			public static readonly Setting<bool> HealingKitFail = new Setting<bool>("Filters/HealingKitFail", "Healing Kit Fail");

			public static readonly Setting<bool> Salvaging = new Setting<bool>("Filters/Salvaging", "Salvaging");

			public static readonly Setting<bool> SalvagingFails = new Setting<bool>("Filters/SalvagingFails", "Salvaging Fails");

			public static readonly Setting<bool> AuraOfCraftman = new Setting<bool>("Filters/AuraOfCraftman", "Aura Of Craftman Spam");

			public static readonly Setting<bool> ManaStoneUsage = new Setting<bool>("Filters/ManaStoneUsage", "Mana Stone Usage");


			public static readonly Setting<bool> TradeBuffBotSpam = new Setting<bool>("Filters/TradeBuffBotSpam", "Trade/Buff Bot Spam");

			public static readonly Setting<bool> FailedAssess = new Setting<bool>("Filters/FailedAssess", "Someone failed to assess you");


			public static readonly Setting<bool> KillTaskComplete = new Setting<bool>("Filters/KillTaskComplete", "Kill Task Complete");

			public static readonly Setting<bool> VendorTells = new Setting<bool>("Filters/VendorTells", "Vendor Tells");

			public static readonly Setting<bool> MonsterTell = new Setting<bool>("Filters/MonsterTell", "Monster Tells");

			public static readonly Setting<bool> NpcChatter = new Setting<bool>("Filters/NPCChatter", "NPC Chatter");

			public static readonly Setting<bool> MasterArbitratorSpam = new Setting<bool>("Filters/MasterArbitratorSpam", "Master Arbitrator Spam");

			public static readonly Setting<bool> AllMasterArbitratorChat = new Setting<bool>("Filters/AllMasterArbitratorChat", "All Master Arbitrator Chat");


			public static readonly Setting<bool> StatusTextYoureTooBusy = new Setting<bool>("Filters/StatusTextYoureTooBusy", "Status Text: You're too busy!");

			public static readonly Setting<bool> StatusTextCasting = new Setting<bool>("Filters/StatusTextCasting", "Status Text: Casting ...");

			public static readonly Setting<bool> StatusTextAll = new Setting<bool>("Filters/StatusTextAll", "Status Text: All");
		}
	}
}
