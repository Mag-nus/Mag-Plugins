using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
	class LoginNextCharacterManager
	{
		readonly LoginCharacterTools loginCharacterTools;

		readonly System.Windows.Forms.Timer loginNextCharTimer = new System.Windows.Forms.Timer();

		string nextCharacter;
		int nextCharByInt;

		public LoginNextCharacterManager(LoginCharacterTools loginCharacterTools)
		{
			this.loginCharacterTools = loginCharacterTools;

			nextCharByInt = -1;

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
				nextCharByInt = -1;
				nextCharacter = lower.Substring(12, lower.Length - 12);
				Debug.WriteToChat("Login Next Character set to: " + nextCharacter);

				e.Eat = true;
			}
			else if (lower == "/mf lnc clear")
			{
				nextCharacter = null;
				Debug.WriteToChat("Login Next Character cleared");

				e.Eat = true;
			}
			else if (lower.StartsWith("/mf lncbi set "))
			{
				nextCharByInt = int.Parse(lower.Substring(14, lower.Length - 14));

				if (nextCharByInt > 10)
				{
					nextCharByInt = -1;
					Debug.WriteToChat("Login Next Character failed with input too large: " + nextCharByInt);
				}
				else if (nextCharByInt < 0)
				{
					nextCharByInt = -1;
					Debug.WriteToChat("Login Next Character failed with input too small: " + nextCharByInt);
				}
				else
				{
					Debug.WriteToChat("Login Next Character set to index: " + nextCharByInt);
					nextCharacter = null;
				}

				e.Eat = true;
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
				else if (nextCharByInt >= 0 && nextCharByInt <= 10)
				{
					loginCharacterTools.LoginByIndex(nextCharByInt);
					nextCharByInt = -1;
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
