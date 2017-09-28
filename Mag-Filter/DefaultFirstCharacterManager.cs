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
            {
                zonename = Convert.ToString(e.Message["zonename"]);
                log.WriteInfo("FilterCore_ServerDispatch: 0xF658");
            }

			if (e.Message.Type == 0xF7E1) // Server Name (we get this when we log out a character as well)
            {
                //getting the Server from the message, but then ignore it and set to the one we know works from the files
                server = Convert.ToString(e.Message["server"]);
                var launchInfo = LaunchControl.GetLaunchInfo();
                server = launchInfo.ServerName;
                log.WriteInfo("Server as retrieved from launchInfo: " + server);
            }
				

			// F7E5 - Unknown? (we only get this the first time we connect), E5 F7 00 00 01 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 

			if (e.Message.Type == 0xF7EA) // Unknown? (we only get this the first time we connect), EA F7 00 0
			{
			    defaultFirstCharTimer.Start();
			}
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			string lower = e.Text.ToLower();

			if (lower.StartsWith("/mf dlc set"))
			{
				DefaultFirstCharacterLoader.SetDefaultFirstCharacter(new DefaultFirstCharacter(server, zonename, CoreManager.Current.CharacterFilter.Name));
				Debug.WriteToChat("Default Login Character set to: " + CoreManager.Current.CharacterFilter.Name);

				e.Eat = true;
			}
			else if (lower == "/mf dlc clear")
			{
				DefaultFirstCharacterLoader.DeleteDefaultFirstCharacter(server, zonename);
				Debug.WriteToChat("Default Login Character cleared");

				e.Eat = true;
			}
		}

		void defaultFirstCharTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				var defaultFirstCharacters = DefaultFirstCharacterLoader.DefaultFirstCharacters;

				foreach (var character in defaultFirstCharacters)
				{
					if (character.ZoneId == zonename && character.Server == server)
					{
						// Bypass movies/logos
						if (state == 1 || state == 2)
							PostMessageTools.SendMouseClick(350, 100);

						if (state == 3)
						{
                            loginCharacterTools.LoginCharacter(character.CharacterName);
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
