using System;
using System.Runtime.InteropServices;

using Decal.Adapter;

namespace MagTools.Client
{
	static class ChatSizeManager
	{
		public enum ChatState
		{
			Unknown,
			Minimized,
			Maximized,
		}

		public static ChatState CurrentState = ChatState.Unknown;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		static extern bool PostMessage(IntPtr hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);

		public static bool Minimize(bool force = false)
		{
			if (CurrentState == ChatState.Minimized && !force)
				return false;

			// 1680, 1050 AC client size
			// 1686, 1078 Actual window size
			// 1362, 698 click point

			// 0200 WM_MOUSEMOVE
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0200, (IntPtr)0x00000000, (UIntPtr)(((ACClientHeight - 352) * 0x10000) + (ACClientWidth - 318)));
			// 0201 WM_LBUTTONDOWN
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0201, (IntPtr)0x00000001, (UIntPtr)(((ACClientHeight - 352) * 0x10000) + (ACClientWidth - 318)));
			// 0202 WM_LBUTTONUP
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0202, (IntPtr)0x00000000, (UIntPtr)(((ACClientHeight - 352) * 0x10000) + (ACClientWidth - 318)));

			CurrentState = ChatState.Minimized;

			return true;
		}

		public static bool Maximize(bool force = false)
		{
			if (CurrentState == ChatState.Maximized && !force)
				return false;

			// 1680, 1050 AC client size
			// 1686, 1078 Actual window size
			// 1362, 958 click point

			// 800, 600 AC client size
			// 806, 628 Actual window size
			// 482, 508 click point

			// The click point is 92 pixels from the bottom and 318 pixels from the right

			// 0200 WM_MOUSEMOVE
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0200, (IntPtr)0x00000000, (UIntPtr)(((ACClientHeight - 92) * 0x10000) + (ACClientWidth - 318)));
			// 0201 WM_LBUTTONDOWN
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0201, (IntPtr)0x00000001, (UIntPtr)(((ACClientHeight - 92) * 0x10000) + (ACClientWidth - 318)));
			// 0202 WM_LBUTTONUP
			PostMessage(CoreManager.Current.Decal.Hwnd, 0x0202, (IntPtr)0x00000000, (UIntPtr)(((ACClientHeight - 92) * 0x10000) + (ACClientWidth - 318)));

			CurrentState = ChatState.Maximized;

			return true;
		}

		internal struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public int Width { get { return Right - Left; } }
			public int Height { get { return Bottom - Top; } }
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		static int ACClientWidth
		{
			get
			{
				// This is a hack because I don't know how to get the windows current theme border information
				// So, I just compare the current form size to the known AC client window sizes

				RECT rect = new RECT();

				GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				// Widths: 800 1024 1152 1280 1360 1400 1440 1600 1680 1792 1800 1856 1920 2048 2560
				if (rect.Width >= 2560) return 2560;
				if (rect.Width >= 2048) return 2048;
				if (rect.Width >= 1920) return 1920;
				if (rect.Width >= 1856) return 1856;
				if (rect.Width >= 1800) return 1800;
				if (rect.Width >= 1792) return 1792; // This would be broken for styles that have a border with >= 8, as it would be 1800+ then.
				if (rect.Width >= 1680) return 1680;
				if (rect.Width >= 1600) return 1600;
				if (rect.Width >= 1440) return 1440;
				if (rect.Width >= 1400) return 1400;
				if (rect.Width >= 1360) return 1360;
				if (rect.Width >= 1280) return 1280;
				if (rect.Width >= 1152) return 1152;
				if (rect.Width >= 1024) return 1024;
				if (rect.Width >= 800) return 800;

				return 0;
			}
		}

		static int ACClientHeight
		{
			get
			{
				// This is a hack because I don't know how to get the windows current theme border information
				// So, I just compare the current form size to the known AC client window sizes

				RECT rect = new RECT();

				GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				// Workarounds go here:
				if (rect.Height == (1024 + 28)) return 1024;

				// Heights: 600 720 768 800 864 900 960 1024 1050 1080 1200 1344 1392 1440 1536 1600
				if (rect.Height >= 1600) return 1600;
				if (rect.Height >= 1536) return 1536;
				if (rect.Height >= 1440) return 1440;
				if (rect.Height >= 1392) return 1392;
				if (rect.Height >= 1344) return 1344;
				if (rect.Height >= 1200) return 1200;
				if (rect.Height >= 1080) return 1080;
				if (rect.Height >= 1050) return 1050;
				if (rect.Height >= 1024) return 1024;
				if (rect.Height >= 960) return 960;
				if (rect.Height >= 900) return 900;
				if (rect.Height >= 864) return 864;
				if (rect.Height >= 800) return 800;
				if (rect.Height >= 768) return 768;
				if (rect.Height >= 720) return 720;
				if (rect.Height >= 600) return 600;

				return 0;
			}
		}
	}
}
