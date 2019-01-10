using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class DefaultFirstCharacterManager
	{
		readonly LoginCharacterTools loginCharacterTools;

		readonly System.Windows.Forms.Timer defaultFirstCharTimer = new System.Windows.Forms.Timer();

		int state;

		string zonename;
		string server;

		public DefaultFirstCharacterManager(LoginCharacterTools loginCharacterTools)
		{
			this.loginCharacterTools = loginCharacterTools;

			defaultFirstCharTimer.Tick += new EventHandler(defaultFirstCharTimer_Tick);
			defaultFirstCharTimer.Interval = 1000;
		}

		public void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			// When we login for the first time we get the following for messages in the following order

			if (e.Message.Type == 0xF658) // Character List (we get this when we log out a character as well)
				zonename = Convert.ToString(e.Message["zonename"]);

			if (e.Message.Type == 0xF7E1) // Server Name (we get this when we log out a character as well)
				server = Convert.ToString(e.Message["server"]);

			// F7E5 - Unknown? (we only get this the first time we connect), E5 F7 00 00 01 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 

			if (e.Message.Type == 0xF7EA) // Unknown? (we only get this the first time we connect), EA F7 00 0
				defaultFirstCharTimer.Start();
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			string lower = e.Text.ToLower();

			if (lower.StartsWith("/mf dlc set"))
			{
				Settings.SettingsManager.CharacterSelectionScreen.SetDefaultFirstCharacter(new DefaultFirstCharacter(server, zonename, CoreManager.Current.CharacterFilter.Name));
				Debug.WriteToChat("Default Login Character set to: " + CoreManager.Current.CharacterFilter.Name);

				e.Eat = true;
			}
			else if (lower.StartsWith("/mf dlcbi set "))
			{
				var index = int.Parse(lower.Substring(14, lower.Length - 14));

				if (index > 10)
				{
					index = -1;
					Debug.WriteToChat("Default Login Character failed with input too large: " + index);
				}
				else if (index < 0)
				{
					index = -1;
					Debug.WriteToChat("Default Login Character failed with input too small: " + index);
				}
				else
				{

					Settings.SettingsManager.CharacterSelectionScreen.SetDefaultFirstCharacter(new DefaultFirstCharacter(server, zonename, null, index));
					Debug.WriteToChat("Default Login Character set to index: " + index);
				}

				e.Eat = true;
			}
			else if (lower == "/mf dlc clear" || lower == "/mf dlcbi clear")
			{
				Settings.SettingsManager.CharacterSelectionScreen.DeleteDefaultFirstCharacter(server, zonename);
				Debug.WriteToChat("Default Login Character cleared");

				e.Eat = true;
			}
			else if (lower.StartsWith("/mf sdlcbi set "))
			{
				var index = int.Parse(lower.Substring(15, lower.Length - 15));

				if (index > 10)
				{
					index = -1;
					Debug.WriteToChat("Default Login Character failed with input too large: " + index);
				}
				else if (index < 0)
				{
					index = -1;
					Debug.WriteToChat("Default Login Character failed with input too small: " + index);
				}
				else
				{

					Settings.SettingsManager.CharacterSelectionScreen.SetDefaultFirstCharacter(new DefaultFirstCharacter(server, null, null, index));
					Debug.WriteToChat("Server Default Login Character set to index: " + index);
				}

				e.Eat = true;
			}
			else if (lower == "/mf sdlcbi clear")
			{
				Settings.SettingsManager.CharacterSelectionScreen.DeleteDefaultFirstCharacters(server);
				Debug.WriteToChat("Server Default Login Characters cleared");

				e.Eat = true;
			}
		}

		void defaultFirstCharTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				var defaultFirstCharacters = Settings.SettingsManager.CharacterSelectionScreen.DefaultFirstCharacters;

				foreach (var character in defaultFirstCharacters)
				{
					if ((String.IsNullOrEmpty(character.AccountName) || character.AccountName == zonename) && character.Server == server)
					{
						// Bypass movies/logos
						if (state == 1 || state == 2)
							PostMessageTools.SendMouseClick(350, 100);

						if (state == 3)
						{
							if (!String.IsNullOrEmpty(character.CharacterName))
								loginCharacterTools.LoginCharacter(character.CharacterName);
							else if (character.CharacterIndex != -1)
								loginCharacterTools.LoginByIndex(character.CharacterIndex);
						}

						break;
					}
				}

				if (state >= 3)
					defaultFirstCharTimer.Stop();

				state++;
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
