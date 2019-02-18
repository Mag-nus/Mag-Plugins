using System;
using System.Runtime.InteropServices;

namespace Mag.Shared
{
	public static class User32
	{
		public const int WM_ACTIVATE		= 0x0006;
		public const int WM_SETFOCUS		= 0x0007;
		public const int WM_KILLFOCUS		= 0x0008;
		public const int WM_ACTIVATEAPP		= 0x001C;
		public const int WM_DESTROY	        = 0x0002;
		public const int WM_KEYDOWN		= 0x0100;
		public const int WM_KEYUP		= 0x0101;
		public const int WM_CHAR		= 0x0102;

		public const int WM_MOUSEMOVE		= 0x0200;
		public const int WM_LBUTTONDOWN		= 0x0201;
		public const int WM_LBUTTONUP		= 0x0202;

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern bool PostMessage(IntPtr hhwnd, uint msg, IntPtr wparam, UIntPtr lparam);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;

			public int Width { get { return Right - Left; } }
			public int Height { get { return Bottom - Top; } }
		}

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		//Gets window attributes
		[DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		//Sets window attributes
		/// <summary>
		/// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
		/// </summary>
		/// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs..</param>
		/// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer. To set any other value, specify one of the following values: GWL_EXSTYLE, GWL_HINSTANCE, GWL_ID, GWL_STYLE, GWL_USERDATA, GWL_WNDPROC </param>
		/// <param name="dwNewLong">The replacement value.</param>
		/// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.
		/// If the function fails, the return value is zero. To get extended error information, call GetLastError. </returns>
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		/// <summary>
		/// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="x">Specifies the new position of the left side of the window.</param>
		/// <param name="y">Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.</param>
		/// <param name="nHeight">Specifies the new height of the window.</param>
		/// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
	}
}
