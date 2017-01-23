using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

using Mag.Shared;

namespace Mag_CombatLogger
{
	// http://asheroncallwiki.altervista.org/wiki/combat
	//http://asheron.wikia.com/wiki/Combat

	[FriendlyName("Mag-CombatLogger")]
	public class Class1 : PluginBase
	{
		private readonly int FILEVERSION = 2;

		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-CombatLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;
			Core.EchoFilter.ClientDispatch += EchoFilter_ClientDispatch;

			Core.ItemSelected += Core_ItemSelected;
			Core.CharacterFilter.SpellCast += CharacterFilter_SpellCast;

			Core.ChatBoxMessage += Core_ChatBoxMessage;
		}

		protected override void Shutdown()
		{
			Core.ChatBoxMessage -= Core_ChatBoxMessage;

			Core.EchoFilter.ServerDispatch -= EchoFilter_ServerDispatch;
			Core.EchoFilter.ClientDispatch -= EchoFilter_ClientDispatch;

			Core.ItemSelected += Core_ItemSelected;
			Core.CharacterFilter.SpellCast -= CharacterFilter_SpellCast;
		}


		private bool CheckForAetheriaAndCloak()
		{
			// If the user has Aetheria or a Cloak equipped, throw a warning and quit
			foreach (var wo in Core.WorldFilter.GetInventory())
			{
				if (wo.Values(LongValueKey.EquippedSlots) > 0 && (wo.Name.Contains("Aetheria") || wo.Name.Contains("Cloak")))
				{
					Core.Actions.AddChatText("<{Mag-CombatLogger}>: Combat Logging disabled when an Aetheria or Cloak is equipped.", 5);
					return false;
				}
			}

			return true;
		}


		private string CreateFileIfItDoesntExist()
		{
			string logFileName = pluginPersonalFolder.FullName + @"\" + Core.CharacterFilter.Server + " " + Core.CharacterFilter.Name + ".csv";

			//foreach (var c in System.IO.Path.GetInvalidFileNameChars())
			//	logFileName = logFileName.Replace(c, '_');

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("Timestamp,LandCell,JSON");

					writer.Close();
				}
			}

			using (StreamWriter writer = new StreamWriter(logFileName, true))
			{
				StringBuilder output = new StringBuilder();

				// Timestamp,Landcell,JSON
				output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
				output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

				output.Append("{\"FileVersion\":\"" + FILEVERSION + "\"}");

				writer.WriteLine(output);

				writer.Close();
			}

			return logFileName;
		}


		private void EchoFilter_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (e.Message.Type == 0xF7B1) // Game Event
				{
					var actionId = e.Message.Value<uint>("action");

					if (actionId == 7) // Evt_Combat__UntargetedMeleeAttack_ID
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							using (MemoryStream memoryStream = new MemoryStream(e.Message.RawData))
							using (BinaryReader binaryReader = new BinaryReader(memoryStream))
							{
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();

								output.Append("{\"ClientDispatch\":\"Untargeted Melee Attack\",");
								output.Append("\"targetId\":\"" + binaryReader.ReadInt32() + "\",");
								output.Append("\"attackHeight\":\"" + binaryReader.ReadUInt32() + "\",");
								output.Append("\"powerLevel\":\"" + binaryReader.ReadSingle() + "\"}");
							}

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (actionId == 8) // Evt_Combat__TargetedMeleeAttack_ID
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							using (MemoryStream memoryStream = new MemoryStream(e.Message.RawData))
							using (BinaryReader binaryReader = new BinaryReader(memoryStream))
							{
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();

								var targetId = binaryReader.ReadInt32();

								output.Append("{\"ClientDispatch\":\"Targeted Melee Attack\",");
								output.Append("\"targetId\":\"" + targetId + "\",");

								var wo = Core.WorldFilter[targetId];
								if (wo != null)
									output.Append("\"targetName\":\"" + wo .Name + "\",");

								output.Append("\"attackHeight\":\"" + binaryReader.ReadUInt32() + "\",");
								output.Append("\"powerLevel\":\"" + binaryReader.ReadSingle() + "\"}");
							}

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (actionId == 10) // Evt_Combat__TargetedMissileAttack_ID
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							using (MemoryStream memoryStream = new MemoryStream(e.Message.RawData))
							using (BinaryReader binaryReader = new BinaryReader(memoryStream))
							{
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();

								output.Append("{\"ClientDispatch\":\"Untargeted Missile Attack\",");
								output.Append("\"targetId\":\"" + binaryReader.ReadInt32() + "\",");
								output.Append("\"attackHeight\":\"" + binaryReader.ReadUInt32() + "\",");
								output.Append("\"accuracy\":\"" + binaryReader.ReadSingle() + "\"}");
							}

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (actionId == 11) // Evt_Combat__UntargetedMissileAttack_ID
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							using (MemoryStream memoryStream = new MemoryStream(e.Message.RawData))
							using (BinaryReader binaryReader = new BinaryReader(memoryStream))
							{
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();
								binaryReader.ReadUInt32();

								var targetId = binaryReader.ReadInt32();

								output.Append("{\"ClientDispatch\":\"Targeted Missile Attack\",");
								output.Append("\"targetId\":\"" + targetId + "\",");

								var wo = Core.WorldFilter[targetId];
								if (wo != null)
									output.Append("\"targetName\":\"" + wo.Name + "\",");

								output.Append("\"attackHeight\":\"" + binaryReader.ReadUInt32() + "\",");
								output.Append("\"accuracy\":\"" + binaryReader.ReadSingle() + "\"}");
							}

							writer.WriteLine(output);

							writer.Close();
						}
					}
				}
			}
			catch { }
		}

		private void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				if (e.Message.Type == 0xF7B0) // Game Event
				{
					var eventId = e.Message.Value<uint>("event");

					if (eventId == 0x01A7) // Attack Completed
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Attack Completed\",");
							output.Append("\"number\":\"" + e.Message.Value<uint>("number") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (eventId == 0x01B1) // Inflict Melee Damage
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Inflict Melee Damage\",");
							output.Append("\"target\":\"" + e.Message.Value<string>("target") + "\",");
							output.Append("\"type\":\"" + e.Message.Value<uint>("type") + "\",");
							output.Append("\"severity\":\"" + e.Message.Value<double>("severity") + "\",");
							output.Append("\"damage\":\"" + e.Message.Value<uint>("damage") + "\",");
							output.Append("\"critical\":\"" + e.Message.Value<bool>("critical") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (eventId == 0x1B2) // Receive Melee Damage
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Receive Melee Damage\",");
							output.Append("\"attacker\":\"" + e.Message.Value<string>("attacker") + "\",");
							output.Append("\"type\":\"" + e.Message.Value<uint>("type") + "\",");
							output.Append("\"severity\":\"" + e.Message.Value<double>("severity") + "\",");
							output.Append("\"damage\":\"" + e.Message.Value<uint>("damage") + "\",");
							output.Append("\"location\":\"" + e.Message.Value<uint>("location") + "\",");
							output.Append("\"critical\":\"" + e.Message.Value<bool>("critical") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (eventId == 0x1B3) // Other Melee Evade
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Other Melee Evade\",");
							output.Append("\"target\":\"" + e.Message.Value<string>("target") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (eventId == 0x1B4) // Self Melee Evade
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Self Melee Evade\",");
							output.Append("\"attacker\":\"" + e.Message.Value<string>("attacker") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
					else if (eventId == 0x1B8) // Start Melee Attack
					{
						if (!CheckForAetheriaAndCloak())
							return;

						var logFileName = CreateFileIfItDoesntExist();

						AddCurrentVariablesToLog(logFileName);

						using (StreamWriter writer = new StreamWriter(logFileName, true))
						{
							StringBuilder output = new StringBuilder();

							// Timestamp,Landcell,JSON
							output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

							output.Append("{\"ServerDispatch\":\"Start Melee Attack\",");
							output.Append("\"attacker\":\"" + e.Message.Value<string>("attacker") + "\"}");

							writer.WriteLine(output);

							writer.Close();
						}
					}
				}

				if (e.Message.Type == 0xF74C) // Animation
				{
					var objectID = e.Message.Value<int>("object");

					// I don't even know if these variables are working correctly in decal
					if (objectID == Core.CharacterFilter.Id && e.Message.Value<byte>("animation_type") == 0 && (e.Message.Value<int>("flags") & 0x80) != 0)
					{
						/*
						14:34:47 4CF70000687111501402 1100070000000001 40 00C1 00000040000000C03F5F000100000000400000F6B3ACDC	attack
						14:34:47 4CF70000687111501402 1200070001000001 40 0087 00000040000500878CEEBF5F00010000000040F6B3ACDC
						14:34:48 4CF70000687111501402 1300070001000001 40 0081 00000040005F000100000000400000F6B3ACDC
								 4CF70000687111501402 1A000B0000000001 3E 00C1 0000003E000000C0BF5F000200000000400000E8B3ACDC
								 4CF70000687111501402 22000F0000000001 3C 00C1 0000003C000000C0BF66000300000000400000F8B3ACDC
						14:35:46 4CF70000687111501402 23000F0001000001 3C 0087 0000003C000500878CEEBF6600030000000040F8B3ACDC
						6:39	 4CF70000687111501402 2B00160000000000 49 0081 000000490076000600000000400000						Cast Spell/Teleport
						14:36:41 4CF70000687111501402 2C00170000000000 49 0081 000000490074000700000000400000
 	    						 4CF70000687111501402 2500100000000000 3C 0081 0000003C000E0104000000803F0000						Heal in combat
						14:36:20 4CF70000687111501402 2900140000000000 3D 0080 0000000    E0105000000803F							Heal in peace
						4:37:59  4CF70000687111501402 32001D0000000000 49 0081 00000049000E0109000000803F0000						Heal
						*/

						// This is a major hack
						if ((e.Message.Value<int>("flags") == 0x80 && e.Message.RawData[24] == 0x0E) || (e.Message.Value<int>("flags") == 0x81 && e.Message.RawData[26] == 0x0E))
						{
							if (!CheckForAetheriaAndCloak())
								return;

							var logFileName = CreateFileIfItDoesntExist();

							AddCurrentVariablesToLog(logFileName);

							using (StreamWriter writer = new StreamWriter(logFileName, true))
							{
								StringBuilder output = new StringBuilder();

								// Timestamp,Landcell,JSON
								output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
								output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

								output.Append("{\"Event\":\"Healing\"}");

								writer.WriteLine(output);

								writer.Close();
							}
						}
					}
				}
			}
			//catch { }
			catch (Exception ex) { Core.Actions.AddChatText("<{Mag-CombatLogger}>: Exception " + ex, 5); }
		}


		private void CharacterFilter_SpellCast(object sender, SpellCastEventArgs e)
		{
			try
			{
				if (e.TargetId != Core.CharacterFilter.Id)
				{
					if (!CheckForAetheriaAndCloak())
						return;

					var logFileName = CreateFileIfItDoesntExist();

					AddCurrentVariablesToLog(logFileName);

					using (StreamWriter writer = new StreamWriter(logFileName, true))
					{
						StringBuilder output = new StringBuilder();

						// Timestamp,Landcell,JSON
						output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
						output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

						output.Append("{\"Event\":\"SpellCast\",");
						output.Append("\"EventType\":\"" + e.EventType + "\",");
						output.Append("\"SpellId\":\"" + e.SpellId + "\",");
						output.Append("\"TargetId\":\"" + e.TargetId + "\"}");

						writer.WriteLine(output);

						writer.Close();
					}
				}
			}
			catch { }
		}

		private int lastItemIDSelected = -1;

		private void Core_ItemSelected(object sender, ItemSelectedEventArgs e)
		{
			try
			{
				var wo = Core.WorldFilter[e.ItemGuid];

				if (wo == null)
					return;

				if (wo.ObjectClass != ObjectClass.Monster)
					return;

				if (e.ItemGuid == lastItemIDSelected)
					return;

				lastItemIDSelected = e.ItemGuid;

				if (!CheckForAetheriaAndCloak())
					return;

				var logFileName = CreateFileIfItDoesntExist();

				AddCurrentVariablesToLog(logFileName);

				using (StreamWriter writer = new StreamWriter(logFileName, true))
				{
					StringBuilder output = new StringBuilder();

					// Timestamp,Landcell,JSON
					output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
					output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

					output.Append("{\"Event\":\"ItemSelected\",");
					output.Append("\"ItemGuid\":\"" + e.ItemGuid + "\",");
					output.Append("\"Name\":\"" + wo.Name + "\"}");

					writer.WriteLine(output);

					writer.Close();
				}
			}
			catch { }
		}


		private void Core_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (String.IsNullOrEmpty(e.Text))
					return;

				if (Util.IsChat(e.Text))
					return;

				bool isCombatMessage = false;

				// You evaded Remoran Corsair!
				// Ruschk Sadist evaded your attack.
				// You resist the spell cast by Remoran Corsair
				// Sentient Crystal Shard resists your spell
				if (CombatMessages.IsFailedAttack(e.Text))
				{
					isCombatMessage = true;
				}
				// You flatten Noble Remains's body with the force of your assault!
				// Your killing blow nearly turns Shivering Crystalline Wisp inside-out!
				// The thunder of crushing Pyre Minion is followed by the deafening silence of death!
				// Old Bones is shattered by your assault!
				else if (CombatMessages.IsKilledByMeMessage(e.Text))
				{
					isCombatMessage = true;
				}
				else
				{
					foreach (Regex regex in CombatMessages.MeleeMissileReceivedAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							isCombatMessage = true;
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MeleeMissileGivenAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							isCombatMessage = true;
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicReceivedAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							isCombatMessage = true;
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicGivenAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							isCombatMessage = true;
							goto Found;
						}
					}

					foreach (Regex regex in CombatMessages.MagicCastAttacks)
					{
						Match match = regex.Match(e.Text);

						if (match.Success)
						{
							isCombatMessage = true;
							goto Found;
						}
					}

					Found:;
				}

				if (!isCombatMessage)
					return;


				// If the user has Aetheria or a Cloak equipped, throw a warning and quit
				if (!CheckForAetheriaAndCloak())
					return;


				var logFileName = CreateFileIfItDoesntExist();

				AddCurrentVariablesToLog(logFileName);

				using (StreamWriter writer = new StreamWriter(logFileName, true))
				{
					StringBuilder output = new StringBuilder();

					// Timestamp,Landcell,JSON
					output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
					output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

					output.Append("{\"GameText\":\"" + e.Text + "\"}");

					writer.WriteLine(output);

					writer.Close();
				}
			}
			catch (Exception ex) { Core.Actions.AddChatText("<{Mag-CombatLogger}>: Exception " + ex + " parsing: " + e.Text, 5); }
		}


		private DateTime lastPlayerIdent = DateTime.MinValue;

		private readonly Dictionary<string, string> variableHistory = new Dictionary<string, string>();

		private void AddCurrentVariablesToLog(string logFileName)
		{
			// We need character id info to get some of these stats like ratings
			// When you equip any weapon/staff/whatever, your dmg rating is increased by 5, unless its weeping, then it's -5
			if (DateTime.UtcNow - lastPlayerIdent > TimeSpan.FromSeconds(10))
			{
				Core.Actions.RequestId(Core.CharacterFilter.Id);
				lastPlayerIdent = DateTime.UtcNow;
			}


			var charwo = Core.WorldFilter[Core.CharacterFilter.Id];

			var changedVariables = new Dictionary<string, string>();


			// Augmentations
			var value = Core.CharacterFilter.GetCharProperty((int)Augmentations.BladeTurner).ToString();
			if (!variableHistory.ContainsKey("Aug.BladeTurner") || variableHistory["Aug.BladeTurner"] != value) { variableHistory["Aug.BladeTurner"] = value; changedVariables["Aug.BladeTurner"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.ArrowTurner).ToString();
			if (!variableHistory.ContainsKey("Aug.ArrowTurner") || variableHistory["Aug.ArrowTurner"] != value) { variableHistory["Aug.ArrowTurner"] = value; changedVariables["Aug.ArrowTurner"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.MaceTurner).ToString();
			if (!variableHistory.ContainsKey("Aug.MaceTurner") || variableHistory["Aug.MaceTurner"] != value) { variableHistory["Aug.MaceTurner"] = value; changedVariables["Aug.MaceTurner"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.CausticEnhancement).ToString();
			if (!variableHistory.ContainsKey("Aug.CausticEnhancement") || variableHistory["Aug.CausticEnhancement"] != value) { variableHistory["Aug.CausticEnhancement"] = value; changedVariables["Aug.CausticEnhancement"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.FieryEnchancment).ToString();
			if (!variableHistory.ContainsKey("Aug.FieryEnchancment") || variableHistory["Aug.FieryEnchancment"] != value) { variableHistory["Aug.FieryEnchancment"] = value; changedVariables["Aug.FieryEnchancment"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.IcyEnchancement).ToString();
			if (!variableHistory.ContainsKey("Aug.IcyEnchancement") || variableHistory["Aug.IcyEnchancement"] != value) { variableHistory["Aug.IcyEnchancement"] = value; changedVariables["Aug.IcyEnchancement"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.LightningEnhancement).ToString();
			if (!variableHistory.ContainsKey("Aug.LightningEnhancement") || variableHistory["Aug.LightningEnhancement"] != value) { variableHistory["Aug.LightningEnhancement"] = value; changedVariables["Aug.LightningEnhancement"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.CriticalProtection).ToString();
			if (!variableHistory.ContainsKey("Aug.CriticalProtection") || variableHistory["Aug.CriticalProtection"] != value) { variableHistory["Aug.CriticalProtection"] = value; changedVariables["Aug.CriticalProtection"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.FrenzySlayer).ToString();
			if (!variableHistory.ContainsKey("Aug.FrenzySlayer") || variableHistory["Aug.FrenzySlayer"] != value) { variableHistory["Aug.FrenzySlayer"] = value; changedVariables["Aug.FrenzySlayer"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.IronSkinInvincible).ToString();
			if (!variableHistory.ContainsKey("Aug.IronSkinInvincible") || variableHistory["Aug.IronSkinInvincible"] != value) { variableHistory["Aug.IronSkinInvincible"] = value; changedVariables["Aug.IronSkinInvincible"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.EyeRemorseless).ToString();
			if (!variableHistory.ContainsKey("Aug.EyeRemorseless") || variableHistory["Aug.EyeRemorseless"] != value) { variableHistory["Aug.EyeRemorseless"] = value; changedVariables["Aug.EyeRemorseless"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.HandRemorseless).ToString();
			if (!variableHistory.ContainsKey("Aug.HandRemorseless") || variableHistory["Aug.HandRemorseless"] != value) { variableHistory["Aug.HandRemorseless"] = value; changedVariables["Aug.HandRemorseless"] = value; }


			// Luminance Auras
			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraValor).ToString();
			if (!variableHistory.ContainsKey("AuraValor") || variableHistory["AuraValor"] != value) { variableHistory["AuraValor"] = value; changedVariables["AuraValor"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraProtection).ToString();
			if (!variableHistory.ContainsKey("AuraProtection") || variableHistory["AuraProtection"] != value) { variableHistory["AuraProtection"] = value; changedVariables["AuraProtection"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraGlory).ToString();
			if (!variableHistory.ContainsKey("AuraGlory") || variableHistory["AuraGlory"] != value) { variableHistory["AuraGlory"] = value; changedVariables["AuraGlory"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraTemperance).ToString();
			if (!variableHistory.ContainsKey("AuraTemperance") || variableHistory["AuraTemperance"] != value) { variableHistory["AuraTemperance"] = value; changedVariables["AuraTemperance"] = value; }


			// Skills
			value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.MeleeDefense].ToString();
			if (!variableHistory.ContainsKey("MeleeDefense") || variableHistory["MeleeDefense"] != value) { variableHistory["MeleeDefense"] = value; changedVariables["MeleeDefense"] = value; }

			value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.MissileDefense].ToString();
			if (!variableHistory.ContainsKey("MissileDefense") || variableHistory["MissileDefense"] != value) { variableHistory["MissileDefense"] = value; changedVariables["MissileDefense"] = value; }

			value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.MagicDefense].ToString();
			if (!variableHistory.ContainsKey("MagicDefense") || variableHistory["MagicDefense"] != value) { variableHistory["MagicDefense"] = value; changedVariables["MagicDefense"] = value; }

			if (Core.CharacterFilter.Skills[CharFilterSkillType.HeavyWeapons].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.HeavyWeapons].ToString();
				if (!variableHistory.ContainsKey("HeavyWeapons") || variableHistory["HeavyWeapons"] != value) { variableHistory["HeavyWeapons"] = value; changedVariables["HeavyWeapons"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.LightWeapons].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.LightWeapons].ToString();
				if (!variableHistory.ContainsKey("LightWeapons") || variableHistory["LightWeapons"] != value) { variableHistory["LightWeapons"] = value; changedVariables["LightWeapons"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.FinesseWeapons].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.FinesseWeapons].ToString();
				if (!variableHistory.ContainsKey("FinesseWeapons") || variableHistory["FinesseWeapons"] != value) { variableHistory["FinesseWeapons"] = value; changedVariables["FinesseWeapons"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.TwoHandedCombat].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.TwoHandedCombat].ToString();
				if (!variableHistory.ContainsKey("TwoHandedCombat") || variableHistory["TwoHandedCombat"] != value) { variableHistory["TwoHandedCombat"] = value; changedVariables["TwoHandedCombat"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.WarMagic].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.WarMagic].ToString();
				if (!variableHistory.ContainsKey("WarMagic") || variableHistory["WarMagic"] != value) { variableHistory["WarMagic"] = value; changedVariables["WarMagic"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.VoidMagic].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.VoidMagic].ToString();
				if (!variableHistory.ContainsKey("VoidMagic") || variableHistory["VoidMagic"] != value) { variableHistory["VoidMagic"] = value; changedVariables["VoidMagic"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.Shield].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.Shield].ToString();
				if (!variableHistory.ContainsKey("Shield") || variableHistory["Shield"] != value) { variableHistory["Shield"] = value; changedVariables["Shield"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.DualWield].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.DualWield].ToString();
				if (!variableHistory.ContainsKey("DualWield") || variableHistory["DualWield"] != value) { variableHistory["DualWield"] = value; changedVariables["DualWield"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.SneakAttack].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.SneakAttack].ToString();
				if (!variableHistory.ContainsKey("SneakAttack") || variableHistory["SneakAttack"] != value) { variableHistory["SneakAttack"] = value; changedVariables["SneakAttack"] = value; }
			}

			if (Core.CharacterFilter.Skills[CharFilterSkillType.Recklessness].Training >= TrainingType.Trained)
			{
				value = Core.CharacterFilter.EffectiveSkill[CharFilterSkillType.Recklessness].ToString();
				if (!variableHistory.ContainsKey("Recklessness") || variableHistory["Recklessness"] != value) { variableHistory["Recklessness"] = value; changedVariables["Recklessness"] = value; }
			}


			// Character Ratings
			if (charwo.LongKeys.Contains(307))
			{
				value = charwo.Values((LongValueKey)307).ToString();
				if (!variableHistory.ContainsKey("Self.CreatureDamRating") || variableHistory["Self.CreatureDamRating"] != value) { variableHistory["Self.CreatureDamRating"] = value; changedVariables["Self.CreatureDamRating"] = value; }
			}

			if (charwo.LongKeys.Contains(314))
			{
				value = charwo.Values((LongValueKey)314).ToString();
				if (!variableHistory.ContainsKey("Self.CreatureCritDamRating") || variableHistory["Self.CreatureCritDamRating"] != value) { variableHistory["Self.CreatureCritDamRating"] = value; changedVariables["Self.CreatureCritDamRating"] = value; }
			}

			if (charwo.LongKeys.Contains(308))
			{
				value = charwo.Values((LongValueKey)308).ToString();
				if (!variableHistory.ContainsKey("Self.CreatureDamResist") || variableHistory["Self.CreatureDamResist"] != value) { variableHistory["Self.CreatureDamResist"] = value; changedVariables["Self.CreatureDamResist"] = value; }
			}

			if (charwo.LongKeys.Contains(316))
			{
				value = charwo.Values((LongValueKey)316).ToString();
				if (!variableHistory.ContainsKey("Self.CreatureCritDamResist") || variableHistory["Self.CreatureCritDamResist"] != value) { variableHistory["Self.CreatureCritDamResist"] = value; changedVariables["Self.CreatureCritDamResist"] = value; }
			}


			// Character State
			value = Core.Actions.CombatMode.ToString();
			if (!variableHistory.ContainsKey("CombatMode") || variableHistory["CombatMode"] != value) { variableHistory["CombatMode"] = value; changedVariables["CombatMode"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Strength].ToString();
			if (!variableHistory.ContainsKey("Strength") || variableHistory["Strength"] != value) { variableHistory["Strength"] = value; changedVariables["Strength"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Endurance].ToString();
			if (!variableHistory.ContainsKey("Endurance") || variableHistory["Endurance"] != value) { variableHistory["Endurance"] = value; changedVariables["Endurance"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Quickness].ToString();
			if (!variableHistory.ContainsKey("Quickness") || variableHistory["Quickness"] != value) { variableHistory["Quickness"] = value; changedVariables["Quickness"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Coordination].ToString();
			if (!variableHistory.ContainsKey("Coordination") || variableHistory["Coordination"] != value) { variableHistory["Coordination"] = value; changedVariables["Coordination"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Focus].ToString();
			if (!variableHistory.ContainsKey("Focus") || variableHistory["Focus"] != value) { variableHistory["Focus"] = value; changedVariables["Focus"] = value; }

			value = Core.CharacterFilter.Attributes[CharFilterAttributeType.Self].ToString();
			if (!variableHistory.ContainsKey("Self") || variableHistory["Self"] != value) { variableHistory["Self"] = value; changedVariables["Self"] = value; }

			value = Core.CharacterFilter.Stamina.ToString();
			if (!variableHistory.ContainsKey("Stamina") || variableHistory["Stamina"] != value) { variableHistory["Stamina"] = value; changedVariables["Stamina"] = value; }

			value = Core.CharacterFilter.Burden.ToString();
			if (!variableHistory.ContainsKey("Burden") || variableHistory["Burden"] != value) { variableHistory["Burden"] = value; changedVariables["Burden"] = value; }


			// Weapon
			Dictionary<string, string> weaponVariables = new Dictionary<string, string>();

			foreach (var item in Core.WorldFilter.GetInventory())
			{
				if (item.Values(LongValueKey.EquippedSlots) < 0x00100000 || item.Values(LongValueKey.EquippedSlots) > 0x01000000)
					continue;

				if (item.Values(LongValueKey.EquippedSlots) == 0x00100000) // 00100000 -1	Melee Weapon
				{
					weaponVariables["meleeId"] = item.Id.ToString();
					weaponVariables["meleeName"] = item.Name;
				}

				if (item.Values(LongValueKey.EquippedSlots) == 0x00200000) // 00200000 -1	Shield/Off Hand
				{
					if (item.ObjectClass == ObjectClass.MeleeWeapon)
					{
						weaponVariables["offHandId"] = item.Id.ToString();
						weaponVariables["offHandName"] = item.Name;
					}
					else
					{
						weaponVariables["shieldId"] = item.Id.ToString();
						weaponVariables["shieldName"] = item.Name;

						// The final value of each element is the prot * ArmorLevel.
						// If ArmorLevel is 600, and prot is 2, then the final prot level will be 1200.
						// If ArmorLevel is 600, and prot is 3, then the final prot level will still be 1200 as prots are capped at 2x AmorLevel.
						// These are the buffed values
						// We should also log base values for accurate numbers vs hollow creatures
						weaponVariables["shieldProts"] = 
							item.Values(DoubleValueKey.SlashProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.PierceProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.BludgeonProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.ColdProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.FireProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.AcidProt).ToString("N2") + "/" +
							item.Values(DoubleValueKey.LightningProt).ToString("N2");

						if (!item.HasIdData)
							Core.Actions.RequestId(item.Id);
					}
				}

				if (item.Values(LongValueKey.EquippedSlots) == 0x00400000) // 00400000 -1	Missile Weapon
				{
					weaponVariables["missileId"] = item.Id.ToString();
					weaponVariables["missileName"] = item.Name;
				}

				if (item.Values(LongValueKey.EquippedSlots) == 0x00800000) // 00800000 -1	Missile Ammo
				{
					weaponVariables["ammoId"] = item.Id.ToString();
					weaponVariables["ammoName"] = item.Name;
				}

				if (item.Values(LongValueKey.EquippedSlots) == 0x01000000) // 01000000 -1	Wand/Staff/Orb
				{
					weaponVariables["wandId"] = item.Id.ToString();
					weaponVariables["wandName"] = item.Name;
				}

				if (item.Values(LongValueKey.ArmorLevel) > 0)
					weaponVariables["ArmorLevel"] = item.Values(LongValueKey.ArmorLevel).ToString();

				if (item.Values(LongValueKey.Imbued) > 0)
				{
					if ((item.Values(LongValueKey.Imbued) & 1) == 1) weaponVariables["Imbued"] = "CS";
					if ((item.Values(LongValueKey.Imbued) & 2) == 2) weaponVariables["Imbued"] = "CB";
					if ((item.Values(LongValueKey.Imbued) & 4) == 4) weaponVariables["Imbued"] = "AR";
					if ((item.Values(LongValueKey.Imbued) & 8) == 8) weaponVariables["Imbued"] = "SlashRend";
					if ((item.Values(LongValueKey.Imbued) & 16) == 16) weaponVariables["Imbued"] = "PierceRend";
					if ((item.Values(LongValueKey.Imbued) & 32) == 32) weaponVariables["Imbued"] = "BludgeRend";
					if ((item.Values(LongValueKey.Imbued) & 64) == 64) weaponVariables["Imbued"] = "AcidRend";
					if ((item.Values(LongValueKey.Imbued) & 128) == 128) weaponVariables["Imbued"] = "FrostRend";
					if ((item.Values(LongValueKey.Imbued) & 256) == 256) weaponVariables["Imbued"] = "LightRend";
					if ((item.Values(LongValueKey.Imbued) & 512) == 512) weaponVariables["Imbued"] = "FireRend";
					if ((item.Values(LongValueKey.Imbued) & 1024) == 1024) weaponVariables["Imbued"] = "MeleeImbue";
					if ((item.Values(LongValueKey.Imbued) & 4096) == 4096) weaponVariables["Imbued"] = "MagicImbue";
					if ((item.Values(LongValueKey.Imbued) & 8192) == 8192) weaponVariables["Imbued"] = "Hematited";
					if ((item.Values(LongValueKey.Imbued) & 536870912) == 536870912) weaponVariables["Imbued"] = "MagicAbsorb";
				}

				if (item.Values(LongValueKey.MaxDamage) != 0)
					weaponVariables["MaxDamage"] = item.Values(LongValueKey.MaxDamage).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.Variance) != 0)
					weaponVariables["Variance"] = item.Values(DoubleValueKey.Variance).ToString(CultureInfo.InvariantCulture);

				if (item.Values(LongValueKey.ElementalDmgBonus, 0) != 0)
					weaponVariables["ElementalDmgBonus"] = item.Values(LongValueKey.ElementalDmgBonus).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.DamageBonus, 1) != 1)
					weaponVariables["DamageBonus"] = item.Values(DoubleValueKey.DamageBonus).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.ElementalDamageVersusMonsters, 1) != 1)
					weaponVariables["ElementalDamageVersusMonsters"] = item.Values(DoubleValueKey.ElementalDamageVersusMonsters).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.AttackBonus, 1) != 1)
					weaponVariables["AttackBonus"] = item.Values(DoubleValueKey.AttackBonus).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.MeleeDefenseBonus, 1) != 1)
					weaponVariables["MeleeDefenseBonus"] = item.Values(DoubleValueKey.MeleeDefenseBonus).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.MagicDBonus, 1) != 1)
					weaponVariables["MagicDBonus"] = item.Values(DoubleValueKey.MagicDBonus).ToString(CultureInfo.InvariantCulture);

				if (item.Values(DoubleValueKey.MissileDBonus, 1) != 1)
					weaponVariables["MissileDBonus"] = item.Values(DoubleValueKey.MissileDBonus).ToString(CultureInfo.InvariantCulture);
			}

			StringBuilder weaponOutput = new StringBuilder();
			weaponOutput.Append("{");
			foreach (var variable in weaponVariables)
				weaponOutput.Append("\"" + variable.Key + "\":\"" + variable.Value + "\",");
			weaponOutput.Remove(weaponOutput.Length - 1, 1); // Remove the last comma
			weaponOutput.Append("}");
			if (!variableHistory.ContainsKey("Weapon") || variableHistory["Weapon"] != weaponOutput.ToString()) { variableHistory["Weapon"] = weaponOutput.ToString(); changedVariables["Weapon"] = weaponOutput.ToString(); }


			// Armor
			Dictionary<string, string> armorPieces = new Dictionary<string, string>();

			foreach (var item in Core.WorldFilter.GetInventory())
			{
				if (item.Values(LongValueKey.EquippedSlots) <= 0)
					continue;

				if (item.Values(LongValueKey.ArmorLevel) <= 0)
					continue;

				if (item.Values(LongValueKey.EquippedSlots) == 0x00200000) // 00200000 -1	Shield/Off Hand
					continue;

				Dictionary<string, string> armorVariables = new Dictionary<string, string>();

				armorVariables["Id"] = item.Id.ToString();
				armorVariables["Name"] = item.Name;

				armorVariables["ArmorLevel"] = item.Values(LongValueKey.ArmorLevel).ToString();

				// The final value of each element is the prot * ArmorLevel.
				// If ArmorLevel is 600, and prot is 2, then the final prot level will be 1200.
				// If ArmorLevel is 600, and prot is 3, then the final prot level will still be 1200 as prots are capped at 2x AmorLevel.
				// These are the buffed values
				// We should also log base values for accurate numbers vs hollow creatures
				armorVariables["Prots"] =
					item.Values(DoubleValueKey.SlashProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.PierceProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.BludgeonProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.ColdProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.FireProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.AcidProt).ToString("N2") + "/" +
					item.Values(DoubleValueKey.LightningProt).ToString("N2");

				StringBuilder armorPieceOutput = new StringBuilder();

				armorPieceOutput.Append("{");
				foreach (var variable in armorVariables)
					armorPieceOutput.Append("\"" + variable.Key + "\":\"" + variable.Value + "\",");
				armorPieceOutput.Remove(armorPieceOutput.Length - 1, 1); // Remove the last comma
				armorPieceOutput.Append("}");

				armorPieces[item.Values(LongValueKey.EquippedSlots).ToString()] = armorPieceOutput.ToString();

				if (!item.HasIdData)
					Core.Actions.RequestId(item.Id);
			}

			StringBuilder armorOutput = new StringBuilder();
			armorOutput.Append("{");
			foreach (var variable in armorPieces)
				armorOutput.Append("\"" + variable.Key + "\":" + variable.Value + ",");
			armorOutput.Remove(armorOutput.Length - 1, 1); // Remove the last comma
			armorOutput.Append("}");
			if (!variableHistory.ContainsKey("Armor") || variableHistory["Armor"] != armorOutput.ToString()) { variableHistory["Armor"] = armorOutput.ToString(); changedVariables["Armor"] = armorOutput.ToString(); }


			// Combat Properties
			var monstersWithin3 = 0;
			foreach (var monster in Core.WorldFilter.GetByObjectClass(ObjectClass.Monster))
			{
				if (Util.GetDistanceFromPlayer(monster) <= 3)
					monstersWithin3++;
			}
			if (!variableHistory.ContainsKey("MonstersWithin3") || variableHistory["MonstersWithin3"] != monstersWithin3.ToString()) { variableHistory["MonstersWithin3"] = monstersWithin3.ToString(); changedVariables["MonstersWithin3"] = monstersWithin3.ToString(); }


			// Target
			// Is target vuln'ed or imp'd?


			if (changedVariables.Count > 0)
			{
				using (StreamWriter writer = new StreamWriter(logFileName, true))
				{
					StringBuilder output = new StringBuilder();

					// Timestamp,Landcell,JSON
					output.Append(String.Format("{0:u}", DateTime.UtcNow) + ",");
					output.Append(CoreManager.Current.Actions.Landcell.ToString("X8") + ",");

					output.Append("{");

					output.Append("\"Event\":\"VariableUpdate\",");

					foreach (var variable in changedVariables)
					{
						if (variable.Value.StartsWith("{"))
							output.Append("\"" + variable.Key + "\":" + variable.Value + ",");
						else
							output.Append("\"" + variable.Key + "\":\"" + variable.Value + "\",");
					}

					// Remove the last comma
					output.Remove(output.Length - 1, 1);

					output.Append("}");

					writer.WriteLine(output);

					writer.Close();
				}
			}
		}
	}
}
