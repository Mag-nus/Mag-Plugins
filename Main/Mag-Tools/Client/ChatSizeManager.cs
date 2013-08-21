
using Mag.Shared;

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

		public static bool Minimize(bool force = false)
		{
			if (CurrentState == ChatState.Minimized && !force)
				return false;

			System.Drawing.Rectangle rect = CoreManager.Current.Actions.UIElementRegion(Decal.Adapter.Wrappers.UIElementType.Chat);

			PostMessageTools.SendMouseClick(rect.X + 459, rect.Y + 11);

			CurrentState = ChatState.Minimized;

			return true;
		}

		public static bool Maximize(bool force = false)
		{
			if (CurrentState == ChatState.Maximized && !force)
				return false;

			System.Drawing.Rectangle rect = CoreManager.Current.Actions.UIElementRegion(Decal.Adapter.Wrappers.UIElementType.Chat);

			PostMessageTools.SendMouseClick(rect.X + 459, rect.Y + 11);

			CurrentState = ChatState.Maximized;

			return true;
		}
	}
}
