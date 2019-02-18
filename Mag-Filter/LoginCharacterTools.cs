using System;
using System.Collections.Generic;

using Decal.Adapter;

namespace MagFilter
{
	class LoginCharacterTools
	{
		int characterSlots;

		readonly List<Character> characters = new List<Character>();

		internal void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			if (e.Message.Type == 0xF658) // Character List
			{
				characterSlots = Convert.ToInt32(e.Message["slotCount"]);

				characters.Clear();

				MessageStruct charactersStruct = e.Message.Struct("characters");

				for (int i = 0; i < charactersStruct.Count; i++)
				{
					int character = Convert.ToInt32(charactersStruct.Struct(i)["character"]);
					string name = Convert.ToString(charactersStruct.Struct(i)["name"]);
					int deleteTimeout = Convert.ToInt32(charactersStruct.Struct(i)["deleteTimeout"]);

					characters.Add(new Character(character, name, deleteTimeout));
				}

				characters.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));
			}
		}

		public bool LoginCharacter(int id)
		{
			for (int i = 0 ; i< characters.Count ;i++)
			{
				if (characters[i].Id == id)
					return LoginByIndex(i);
			}

			return false;
		}

		public bool LoginCharacter(string name)
		{
			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].Name.StartsWith("+") && !name.StartsWith("+"))
				{
					if (String.Compare(characters[i].Name.TrimStart('+'), name, StringComparison.OrdinalIgnoreCase) == 0)
						return LoginByIndex(i);
				}
				else
				{
					if (String.Compare(characters[i].Name, name, StringComparison.OrdinalIgnoreCase) == 0)
						return LoginByIndex(i);
				}
			}

			return false;
		}

		private const int XPixelOffset = 121;
		private const int YTopOfBox = 209;
		private const int YBottomOfBox = 532;

		public bool LoginByIndex(int index)
		{
			if (index >= characters.Count)
				return false;

			float characterNameSize = (YBottomOfBox - YTopOfBox) / (float)characterSlots;

			int yOffset = (int)(YTopOfBox + (characterNameSize / 2) + (characterNameSize * index));

			// Select the character
			Mag.Shared.PostMessageTools.SendMouseClick(XPixelOffset, yOffset);

			// Click the Enter button
			Mag.Shared.PostMessageTools.SendMouseClick(0x015C, 0x0185);

			return true;
		}
	}
}
