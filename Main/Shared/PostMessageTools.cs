using System;
using System.Runtime.InteropServices;
using System.Collections;

namespace Mag_Tools
{
	public static class PostMessageTools
	{
		internal class ac : ArrayList
		{
			public ac()
			{
				GCHandle handle = GCHandle.Alloc(this);
				ac.a a = new ac.a(ac.af);
				EnumWindows(a, (IntPtr)handle);
				handle.Free();
			}

			private static bool af(int A_0, IntPtr A_1)
			{
				GCHandle handle = (GCHandle)A_1;
				((ac)handle.Target).Add(A_0);
				return true;
			}

			[DllImport("user32")]
			private static extern int EnumWindows(ac.a A_0, IntPtr A_1);

			private delegate bool a(int A_0, IntPtr A_1);
		}

		[DllImport("kernel32.dll")]
		public static extern int GetCurrentProcessId();
		[DllImport("user32.dll", SetLastError = true)]
		static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);

		private static int _AChWnd;
		private static int AChWnd
		{
			get
			{
				if (_AChWnd != 0)
					return _AChWnd;

				int currentProcessId = GetCurrentProcessId();
				ac ac = new ac();
				foreach (int ac_hWnd in ac)
				{
					IntPtr ac_hWnd_ptr = (IntPtr)ac_hWnd;

					uint ac_ProcessId = 0;
					GetWindowThreadProcessId(ac_hWnd_ptr, out ac_ProcessId);
					if (ac_ProcessId == currentProcessId)
					{
						System.Text.StringBuilder builder = new System.Text.StringBuilder(0xff);
						GetWindowText(ac_hWnd_ptr, builder, builder.Capacity);
						if (builder.ToString() == "Asheron's Call")
						{
							_AChWnd = ac_hWnd;
							return _AChWnd;
						}
					}
				}

				return 0;
			}
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool PostMessage(int hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);

		// http://msdn.microsoft.com/en-us/library/dd375731%28v=vs.85%29.aspx

		// Captured using spy++
		// <01856> 001603E6 P WM_KEYDOWN nVirtKey:VK_CONTROL cRepeat:1 ScanCode:1D fExtended:0 fAltDown:0 fRepeat:0 fUp:0	0x00000011	0x001D0001
		// <01857> 001603E6 P WM_KEYDOWN nVirtKey:'Q' cRepeat:1 ScanCode:10 fExtended:0 fAltDown:0 fRepeat:0 fUp:0			0x00000051	0x00100001
		// <01858> 001603E6 P WM_CHAR chCharCode:'17' (17) cRepeat:1 ScanCode:10 fExtended:0 fAltDown:0 fRepeat:0 fUp:0		0x00000011	0x00100001
		// <01859> 001603E6 P WM_KEYUP nVirtKey:'Q' cRepeat:1 ScanCode:10 fExtended:0 fAltDown:0 fRepeat:1 fUp:1			0x00000051	0xC0100001
		// <01860> 001603E6 P WM_KEYUP nVirtKey:VK_CONTROL cRepeat:1 ScanCode:1D fExtended:0 fAltDown:0 fRepeat:1 fUp:1		0x00000011	0xC01D0001

		private const int WM_KEYDOWN	= 0x0100;
		private const int WM_KEYUP		= 0x0101;
		private const int WM_CHAR		= 0x0102;

		private const byte VK_RETURN	= 0x0D;
		private const byte VK_CONTROL	= 0x11;
		private const byte VK_PAUSE		= 0x13;

		private static byte ScanCode(char Char)
		{
			switch (char.ToLower(Char))
			{
				case 'a': return 0x1E;
				case 'b': return 0x30;
				case 'c': return 0x2e;
				case 'd': return 0x20;
				case 'e': return 0x12;
				case 'f': return 0x21;
				case 'g': return 0x22;
				case 'h': return 0x23;
				case 'i': return 0x17;
				case 'j': return 0x24;
				case 'k': return 0x25;
				case 'l': return 0x26;
				case 'm': return 0x32;
				case 'n': return 0x31;
				case 'o': return 0x18;
				case 'p': return 0x19;
				case 'q': return 0x10;
				case 'r': return 0x13;
				case 's': return 0x1f;
				case 't': return 0x14;
				case 'u': return 0x16;
				case 'v': return 0x2f;
				case 'w': return 0x11;
				case 'x': return 0x2d;
				case 'y': return 0x15;
				case 'z': return 0x2c;
				case '/': return 0x35;
				case ' ': return 0x39;
			}
			return 0;
		}

		private static byte CharCode(char Char)
		{
			switch (char.ToLower(Char))
			{
				case 'a': return 0x41;
				case 'b': return 0x42;
				case 'c': return 0x43;
				case 'd': return 0x44;
				case 'e': return 0x45;
				case 'f': return 0x46;
				case 'g': return 0x47;
				case 'h': return 0x48;
				case 'i': return 0x49;
				case 'j': return 0x4A;
				case 'k': return 0x4B;
				case 'l': return 0x4C;
				case 'm': return 0x4D;
				case 'n': return 0x4E;
				case 'o': return 0x4F;
				case 'p': return 0x50;
				case 'q': return 0x51;
				case 'r': return 0x52;
				case 's': return 0x53;
				case 't': return 0x54;
				case 'u': return 0x55;
				case 'v': return 0x56;
				case 'w': return 0x57;
				case 'x': return 0x58;
				case 'y': return 0x59;
				case 'z': return 0x5A;
				case '/': return 0xBF;
				case ' ': return 0x20;
			}
			return 0x20;
		}

		public static void SendEnter()
		{
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)VK_RETURN, (UIntPtr)0x001C0001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)VK_RETURN, (UIntPtr)0xC01C0001);
		}

		public static void SendPause()
		{
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)VK_PAUSE, (UIntPtr)0x00450001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)VK_PAUSE, (UIntPtr)0xC0450001);
		}

		public static void SendCntrl(char ch)
		{
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)VK_CONTROL, (UIntPtr)0x001D0001);
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)CharCode(ch), (UIntPtr)0x00100001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)CharCode(ch), (UIntPtr)0xC0100001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)VK_CONTROL, (UIntPtr)0xC01D0001);
		}

		/// <summary>
		/// Opens/Closes fellowship view
		/// </summary>
		public static void SendF4()
		{
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)0x00000073, (UIntPtr)0x003E0001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)0x00000073, (UIntPtr)0xC03E0001);
		}

		/// <summary>
		/// Opens/Closes main pack view
		/// </summary>
		public static void SendF12()
		{
			PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)0x0000007B, (UIntPtr)0x00580001);
			PostMessage(AChWnd, WM_KEYUP, (IntPtr)0x0000007B, (UIntPtr)0xC0580001);
		}

		public static void SendMsg(string msg)
		{
			foreach (char ch in msg)
			{
				byte code = CharCode(ch);
				uint lparam = (uint)((ScanCode(ch) << 0x10) | 1);
				PostMessage(AChWnd, WM_KEYDOWN, (IntPtr)code, (UIntPtr)(lparam));
				PostMessage(AChWnd, WM_KEYUP, (IntPtr)code, (UIntPtr)(0xC0000000 | lparam));
			}
		}
	}
}
