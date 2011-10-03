using System;
using System.Runtime.InteropServices;

using Decal.Adapter;

namespace MagTools
{
	class WindowMover : IDisposable
	{
		public WindowMover()
		{
			try
			{
				PluginCore.mainView.SetWindowPos.Hit += new EventHandler(SetWindowPos_Hit);
				PluginCore.mainView.DelWindowPos.Hit += new EventHandler(DelWindowPos_Hit);

				CoreManager.Current.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!_disposed)
			{
				if (disposing)
				{
					PluginCore.mainView.SetWindowPos.Hit -= new EventHandler(SetWindowPos_Hit);
					PluginCore.mainView.DelWindowPos.Hit -= new EventHandler(DelWindowPos_Hit);

					CoreManager.Current.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		internal struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;

		}
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

		/// <summary>
		/// The MoveWindow function changes the position and dimensions of the specified window. For a top-level window, the position and dimensions are relative to the upper-left corner of the screen. For a child window, they are relative to the upper-left corner of the parent window's client area.
		/// </summary>
		/// <param name="hWnd">Handle to the window.</param>
		/// <param name="X">Specifies the new position of the left side of the window.</param>
		/// <param name="Y">Specifies the new position of the top of the window.</param>
		/// <param name="nWidth">Specifies the new width of the window.</param>
		/// <param name="nHeight">Specifies the new height of the window.</param>
		/// <param name="bRepaint">Specifies whether the window is to be repainted. If this parameter is TRUE, the window receives a message. If the parameter is FALSE, no repainting of any kind occurs. This applies to the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent window uncovered as a result of moving a child window.</param>
		/// <returns>If the function succeeds, the return value is nonzero.
		/// <para>If the function fails, the return value is zero. To get extended error information, call GetLastError.</para></returns>
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				System.Xml.XmlNode childNode = GetChildNode;

				if (childNode != null)
				{
					int X;
					int.TryParse(childNode.Attributes["X"].Value, out X);

					int Y;
					int.TryParse(childNode.Attributes["Y"].Value, out Y);

					RECT rect = new RECT();

					GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

					MoveWindow(CoreManager.Current.Decal.Hwnd, X, Y, rect.right - rect.left, rect.bottom - rect.top, true);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void DelWindowPos_Hit(object sender, EventArgs e)
		{
			try
			{
				System.Xml.XmlNode node = GetChildNode;

				if (node != null)
					node.ParentNode.RemoveChild(node);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void SetWindowPos_Hit(object sender, EventArgs e)
		{
			try
			{
				System.Xml.XmlNode childNode = GetChildNode;

				if (childNode == null)
				{
					System.Xml.XmlNode parentNode = PluginCore.pluginConfigFile.GetNode(OptionGroup.Misc.Xpath + "/WindowPositions");

					if (parentNode != null)
						childNode = PluginCore.pluginConfigFile.CreateNode(parentNode, "/WindowPosition", true);
					else
						childNode = PluginCore.pluginConfigFile.CreateNode(OptionGroup.Misc.Xpath + "/WindowPositions/WindowPosition");

					if (childNode == null)
						return;

					PluginCore.pluginConfigFile.AddAttribute(childNode, "Server");
					PluginCore.pluginConfigFile.AddAttribute(childNode, "AccountName");
					PluginCore.pluginConfigFile.AddAttribute(childNode, "X");
					PluginCore.pluginConfigFile.AddAttribute(childNode, "Y");
				}

				childNode.Attributes["Server"].Value = CoreManager.Current.CharacterFilter.Server;
				childNode.Attributes["AccountName"].Value = CoreManager.Current.CharacterFilter.AccountName;

				RECT rect = new RECT();

				GetWindowRect(CoreManager.Current.Decal.Hwnd, ref rect);

				childNode.Attributes["X"].Value = rect.left.ToString();
				childNode.Attributes["Y"].Value = rect.top.ToString();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		System.Xml.XmlNode GetChildNode
		{
			get
			{
				System.Xml.XmlNode parentNode = PluginCore.pluginConfigFile.GetNode(OptionGroup.Misc.Xpath + "/WindowPositions");

				if (parentNode == null || !parentNode.HasChildNodes)
					return null;

				foreach(System.Xml.XmlNode childNode in parentNode.ChildNodes)
				{
					if (childNode.Attributes["Server"] == null || childNode.Attributes["AccountName"].Value == null || childNode.Attributes["X"].Value == null || childNode.Attributes["Y"].Value == null)
						continue;

					if (childNode.Attributes["Server"].Value == CoreManager.Current.CharacterFilter.Server && childNode.Attributes["AccountName"].Value == CoreManager.Current.CharacterFilter.AccountName)
						return childNode;
				}

				return null;
			}
		}
	}
}