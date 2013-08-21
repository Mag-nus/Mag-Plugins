using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class LoginNextCharacterManager
	{
		readonly LoginCharacterTools loginCharacterTools;

		readonly System.Windows.Forms.Timer loginNextCharTimer = new System.Windows.Forms.Timer();

		string zonename;
		string server;

		string nextCharacter;

		public LoginNextCharacterManager(LoginCharacterTools loginCharacterTools)
		{
			this.loginCharacterTools = loginCharacterTools;

			loginNextCharTimer.Tick += new EventHandler(defaultFirstCharTimer_Tick);
			loginNextCharTimer.Interval = 1000;
		}

		public void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			// When we logout we get the following messages in order
			// =========================================================================================
			if (e.Message.Type == 0xF658) // Character List (we get this when we log out a character as well)
				zonename = Convert.ToString(e.Message["zonename"]);

			if (e.Message.Type == 0xF7E1) // Server Name (we get this when we log out a character as well)
			{
				server = Convert.ToString(e.Message["server"]);
				loginNextCharTimer.Start();
			}
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			string lower = e.Text.ToLower();

			if (lower.StartsWith("/mf lnc set "))
			{
				nextCharacter = lower.Substring(12, lower.Length - 12);
				Debug.WriteToChat("Login Next Character set to: " + nextCharacter);
			}
			else if (lower == "/mf lnc clear")
			{
				nextCharacter = null;
				Debug.WriteToChat("Login Next Character cleared");
			}
		}

		void defaultFirstCharTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				loginNextCharTimer.Stop();

				if (!String.IsNullOrEmpty(nextCharacter))
				{
					loginCharacterTools.LoginCharacter(nextCharacter);
					nextCharacter = null;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
