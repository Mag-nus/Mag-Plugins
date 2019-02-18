using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class LoginNextCharacterManager
	{
		readonly LoginCharacterTools loginCharacterTools;

		readonly System.Windows.Forms.Timer loginNextCharTimer = new System.Windows.Forms.Timer();

		string nextCharacterName;
		int nextCharacterIndex;

		public LoginNextCharacterManager(LoginCharacterTools loginCharacterTools)
		{
			this.loginCharacterTools = loginCharacterTools;

			nextCharacterIndex = -1;

			loginNextCharTimer.Tick += new EventHandler(defaultFirstCharTimer_Tick);
			loginNextCharTimer.Interval = 1000;
		}

		public void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			// When we logout we get the following messages in order
			// =========================================================================================
			// 0xF658 Character List (we get this when we log out a character as well)

			if (e.Message.Type == 0xF7E1) // Server Name (we get this when we log out a character as well)
				loginNextCharTimer.Start();
		}

		public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
		{
			string lower = e.Text.ToLower();

			if (lower.StartsWith("/mf lnc set "))
			{
				nextCharacterName = lower.Substring(12, lower.Length - 12);
				nextCharacterIndex = -1;

				Debug.WriteToChat("Login Next Character set to: " + nextCharacterName);

				e.Eat = true;
			}
			else if (lower.StartsWith("/mf lncbi set "))
			{
				nextCharacterName = null;
				nextCharacterIndex = int.Parse(lower.Substring(14, lower.Length - 14));

				if (nextCharacterIndex > 10)
				{
					nextCharacterIndex = -1;
					Debug.WriteToChat("Login Next Character failed with input too large: " + nextCharacterIndex);
				}
				else if (nextCharacterIndex < 0)
				{
					nextCharacterIndex = -1;
					Debug.WriteToChat("Login Next Character failed with input too small: " + nextCharacterIndex);
				}
				else
					Debug.WriteToChat("Login Next Character set to index: " + nextCharacterIndex);

				e.Eat = true;
			}
			else if (lower == "/mf lnc clear" || lower == "/mf lncbi clear")
			{
				nextCharacterName = null;
				nextCharacterIndex = -1;

				Debug.WriteToChat("Login Next Character cleared");

				e.Eat = true;
			}
		}

		void defaultFirstCharTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				loginNextCharTimer.Stop();

				if (!String.IsNullOrEmpty(nextCharacterName))
				{
					loginCharacterTools.LoginCharacter(nextCharacterName);
					nextCharacterName = null;
				}
				else if (nextCharacterIndex >= 0 && nextCharacterIndex <= 10)
				{
					loginCharacterTools.LoginByIndex(nextCharacterIndex);
					nextCharacterIndex = -1;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
