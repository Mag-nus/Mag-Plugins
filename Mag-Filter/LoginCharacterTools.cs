using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Decal.Adapter;

namespace MagFilter
{
	class LoginCharacterTools
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);

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
				if (characters[i].Name == name)
					return LoginByIndex(i);
			}

			return false;
		}

		static int _xPixelOffset = 121;
		static int _yTopOfBox = 209;
		static int _yBottomOfBox = 532;

		public bool LoginByIndex(int index)
		{
			if (index >= characters.Count)
				return false;

			float characterNameSize = (_yBottomOfBox - _yTopOfBox) / (float)characterSlots;

			int yOffset = (int)(_yTopOfBox + (characterNameSize / 2) + (characterNameSize * index));

			int loc = (yOffset * 0x10000) + _xPixelOffset;

			// Select the character
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0200, (IntPtr)0x00000000, (UIntPtr)loc);
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0201, (IntPtr)0x00000001, (UIntPtr)loc);
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0202, (IntPtr)0x00000000, (UIntPtr)loc);

			// Click the Enter button
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0200, (IntPtr)0x00000000, (UIntPtr)0x0185015C);
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0201, (IntPtr)0x00000001, (UIntPtr)0x0185015C);
			PostMessage(CoreManager.Current.Decal.Hwnd.ToInt32(), 0x0202, (IntPtr)0x00000000, (UIntPtr)0x0185015C);

			return true;
		}
	}
}
