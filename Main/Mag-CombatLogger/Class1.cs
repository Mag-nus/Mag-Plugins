using System;
using System.Collections.Generic;
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
		private DirectoryInfo pluginPersonalFolder;

		protected override void Startup()
		{
			pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\Mag-CombatLogger");

			if (!pluginPersonalFolder.Exists)
				pluginPersonalFolder.Create();

			Core.EchoFilter.ServerDispatch += EchoFilter_ServerDispatch;
			Core.EchoFilter.ClientDispatch += EchoFilter_ClientDispatch;
			Core.CharacterFilter.SpellCast += CharacterFilter_SpellCast;

			Core.ChatBoxMessage += Core_ChatBoxMessage;
		}

		protected override void Shutdown()
		{
			Core.ChatBoxMessage -= Core_ChatBoxMessage;

			Core.EchoFilter.ServerDispatch -= EchoFilter_ServerDispatch;
			Core.EchoFilter.ClientDispatch -= EchoFilter_ClientDispatch;
			Core.CharacterFilter.SpellCast -= CharacterFilter_SpellCast;
		}


		private void EchoFilter_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
			}
			catch { }
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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Attack Completed\",");
							output.Append("\"number\":\"" + e.Message.Value<uint>("number") + "\"}\"");

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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Inflict Melee Damage\",");
							output.Append("\"target\":\"" + e.Message.Value<string>("target") + "\",");
							output.Append("\"type\":\"" + e.Message.Value<uint>("type") + "\",");
							output.Append("\"severity\":\"" + e.Message.Value<double>("severity") + "\",");
							output.Append("\"damage\":\"" + e.Message.Value<uint>("damage") + "\",");
							output.Append("\"critical\":\"" + e.Message.Value<bool>("critical") + "\"}\"");

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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Receive Melee Damage\",");
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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Other Melee Evade\",");
							output.Append("\"target\":\"" + e.Message.Value<string>("target") + "\"}\"");

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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Self Melee Evade\",");
							output.Append("\"attacker\":\"" + e.Message.Value<string>("attacker") + "\"}\"");

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

							// "Timestamp,Landcell,JSON"
							output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
							output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

							output.Append("\"{\"ServerDispatch\":\"Start Melee Attack\",");
							output.Append("\"attacker\":\"" + e.Message.Value<string>("attacker") + "\"}\"");

							writer.WriteLine(output);

							writer.Close();
						}
					}
				}
			}
			catch { }
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

						// "Timestamp,Landcell,JSON"
						output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
						output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

						output.Append("\"{\"EventType\":\"" + e.EventType + "\",");
						output.Append("\"SpellId\":\"" + e.SpellId + "\",");
						output.Append("\"TargetId\":\"" + e.TargetId + "\"}\"");

						writer.WriteLine(output);

						writer.Close();
					}
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

					// "Timestamp,Landcell,JSON"
					output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
					output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

					output.Append("\"{\"GameText\":\"" + e.Text + "\"}\"");

					writer.WriteLine(output);

					writer.Close();
				}
			}
			catch (Exception ex) { Core.Actions.AddChatText("<{Mag-CombatLogger}>: Exception " + ex + " parsing: " + e.Text, 5); }
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
					writer.WriteLine("\"Timestamp\",\"LandCell\",\"JSON\"");

					writer.Close();
				}
			}

			return logFileName;
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
			if (!variableHistory.ContainsKey("Aura.AuraValor") || variableHistory["Aura.AuraValor"] != value) { variableHistory["Aura.AuraValor"] = value; changedVariables["Aura.AuraValor"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraProtection).ToString();
			if (!variableHistory.ContainsKey("Aura.AuraProtection") || variableHistory["Aura.AuraProtection"] != value) { variableHistory["Aura.AuraProtection"] = value; changedVariables["Aura.AuraProtection"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraGlory).ToString();
			if (!variableHistory.ContainsKey("Aura.AuraGlory") || variableHistory["Aura.AuraGlory"] != value) { variableHistory["Aura.AuraGlory"] = value; changedVariables["Aura.AuraGlory"] = value; }

			value = Core.CharacterFilter.GetCharProperty((int)Augmentations.AuraTemperance).ToString();
			if (!variableHistory.ContainsKey("Aura.AuraTemperance") || variableHistory["Aura.AuraTemperance"] != value) { variableHistory["Aura.AuraTemperance"] = value; changedVariables["Aura.AuraTemperance"] = value; }


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
				if (!variableHistory.ContainsKey("CreatureDamRating") || variableHistory["CreatureDamRating"] != value) { variableHistory["CreatureDamRating"] = value; changedVariables["CreatureDamRating"] = value; }
			}

			if (charwo.LongKeys.Contains(314))
			{
				value = charwo.Values((LongValueKey)314).ToString();
				if (!variableHistory.ContainsKey("CreatureCritDamRating") || variableHistory["CreatureCritDamRating"] != value) { variableHistory["CreatureCritDamRating"] = value; changedVariables["CreatureCritDamRating"] = value; }
			}

			if (charwo.LongKeys.Contains(308))
			{
				value = charwo.Values((LongValueKey)308).ToString();
				if (!variableHistory.ContainsKey("CreatureDamResist") || variableHistory["CreatureDamResist"] != value) { variableHistory["CreatureDamResist"] = value; changedVariables["CreatureDamResist"] = value; }
			}

			if (charwo.LongKeys.Contains(316))
			{
				value = charwo.Values((LongValueKey)316).ToString();
				if (!variableHistory.ContainsKey("CreatureCritDamResist") || variableHistory["CreatureCritDamResist"] != value) { variableHistory["CreatureCritDamResist"] = value; changedVariables["CreatureCritDamResist"] = value; }
			}


			// Character State
			value = Core.Actions.CombatMode.ToString();
			if (!variableHistory.ContainsKey("CombatMode") || variableHistory["CombatMode"] != value) { variableHistory["CombatMode"] = value; changedVariables["CombatMode"] = value; }

			value = Core.CharacterFilter.Stamina.ToString();
			if (!variableHistory.ContainsKey("Stamina") || variableHistory["Stamina"] != value) { variableHistory["Stamina"] = value; changedVariables["Stamina"] = value; }

			value = Core.CharacterFilter.Burden.ToString();
			if (!variableHistory.ContainsKey("Burden") || variableHistory["Burden"] != value) { variableHistory["Burden"] = value; changedVariables["Burden"] = value; }

			// Healing? (action state)


			// Equipment
			// Weapon mods & properties
			// Shield
			// Armor levels


			// Combat Properties
			// attack strength.. we need to get this from the actual attack... vtank holds down the button for this period of time


			// Target
			// Is target vuln'ed or imp'd?


			if (changedVariables.Count > 0)
			{
				using (StreamWriter writer = new StreamWriter(logFileName, true))
				{
					StringBuilder output = new StringBuilder();

					// "Timestamp,Landcell,JSON"
					output.Append('"' + String.Format("{0:u}", DateTime.UtcNow) + ",");
					output.Append('"' + CoreManager.Current.Actions.Landcell.ToString("X8") + '"' + ",");

					output.Append("\"{");

					foreach (var variable in changedVariables)
						output.Append("\"" + variable.Key + "\":\"" + variable.Value + "\",");

					// Remove the last comma
					output.Remove(output.Length - 1, 1);

					output.Append("}\"");

					writer.WriteLine(output);

					writer.Close();
				}
			}
		}
	}
}
