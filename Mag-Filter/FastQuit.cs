using System;

using Decal.Adapter;

namespace MagFilter
{
	class FastQuit
	{
		public void FilterCore_WindowMessage(object sender, WindowMessageEventArgs e)
		{
			if (String.IsNullOrEmpty(CoreManager.Current.CharacterFilter.Name) || CoreManager.Current.CharacterFilter.Name == "LoginNotComplete")
			{
				/*
				message 0100 WM_KEYDOWN
				wParam 0000001B
				lParam 00010001
				*/
				if (e.Msg == 0x0100 && e.WParam == 0x0000001B && e.LParam == 0x00010001) // Esc Key
				{
					// Click the Yes button
					Mag.Shared.PostMessageTools.ClickYes();
				}
			}
		}
	}
}
