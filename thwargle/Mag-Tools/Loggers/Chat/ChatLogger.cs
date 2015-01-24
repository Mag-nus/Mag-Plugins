using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.Loggers.Chat
{
	class ChatLogger : ILogger<LoggedChat>, IDisposable
	{
		/// <summary>
		/// This is raised when an item has been pushed by the logger.
		/// </summary>
		public event Action<LoggedChat> LogItem;

		public ChatLogger()
		{
			try
			{
				CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

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
			if (!disposed)
			{
				if (disposing)
				{
					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				// Do we even have a receiver?
				if (LogItem == null)
					return;

				if (Util.IsChat(e.Text, Util.ChatFlags.NpcSays | Util.ChatFlags.NpcTellsYou))
					return;

				if (Util.IsSpellCastingMessage(e.Text))
					return;

				LoggedChat item;

				if (Util.IsChat(e.Text, Util.ChatFlags.PlayerTellsYou | Util.ChatFlags.YouTell))
					item = new LoggedChat(DateTime.Now, Util.ChatChannels.Tells, e.Text);
				else if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysLocal | Util.ChatFlags.YouSay))
					item = new LoggedChat(DateTime.Now, Util.ChatChannels.Area, e.Text);
				else if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysChannel))
				{
					Util.ChatChannels channel = Util.GetChatChannel(e.Text);

					if (channel == Util.ChatChannels.None)
						return;

					item = new LoggedChat(DateTime.Now, channel, e.Text);
				}
				else
					//item = new LoggedChat(DateTime.Now, Util.ChatChannels.None, e.Text);
					return;

				if (LogItem != null)
					LogItem(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
