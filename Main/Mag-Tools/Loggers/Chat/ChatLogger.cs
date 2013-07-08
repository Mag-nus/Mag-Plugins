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

				LoggedChat item;

				if (Util.IsChat(e.Text, Util.ChatFlags.PlayerTellsYou | Util.ChatFlags.YouTell))
					item = new LoggedChat(DateTime.Now, Util.ChatChannels.Tells, e.Text);
				else if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysLocal | Util.ChatFlags.YouSay))
					item = new LoggedChat(DateTime.Now, Util.ChatChannels.Area, e.Text);
				else if (Util.IsChat(e.Text, Util.ChatFlags.PlayerSaysChannel))
				{
					Util.ChatChannels channel = Util.GetChatChannel(e.Text);

					item = new LoggedChat(DateTime.Now, channel, e.Text);
				}
				else
					item = new LoggedChat(DateTime.Now, Util.ChatChannels.None, e.Text);

				if (LogItem != null)
					LogItem(item);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public void ImportLogs(string xmlFileName)
		{
			/*CorpseTrackerImporter importer = new CorpseTrackerImporter(xmlFileName);

			List<TrackedCorpse> importedList = new List<TrackedCorpse>();

			importer.Import(importedList);

			foreach (var item in importedList)
			{
				if (trackedItems.ContainsKey(item.Id))
					continue;

				trackedItems.Add(item.Id, item);

				if (ItemAdded != null)
					ItemAdded(item);
			}*/
		}

		public void ExportLogs(string xmlFileName, bool showMessage = false)
		{
			/*if (trackedItems.Count == 0)
				return;

			List<TrackedCorpse> exportedList = new List<TrackedCorpse>();

			foreach (var kvp in trackedItems)
				exportedList.Add(kvp.Value);

			CorpseTrackerExporter exporter = new CorpseTrackerExporter(exportedList);

			exporter.Export(xmlFileName);

			if (showMessage)
				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Stats exported to: " + xmlFileName, 5, Settings.SettingsManager.Misc.OutputTargetWindow.Value);*/
		}
	}
}
