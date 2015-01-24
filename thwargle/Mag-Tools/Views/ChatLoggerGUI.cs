using System;

using MagTools.Loggers;
using MagTools.Loggers.Chat;

using Mag.Shared;

using VirindiViewService.Controls;

namespace MagTools.Views
{
	class ChatLoggerGUI : ILoggerTarget<LoggedChat>, IDisposable
	{
		readonly ILogger<LoggedChat> logger;
		readonly Settings.SettingsManager.ChatLogger.Group settings;
		readonly HudList hudList;

		public ChatLoggerGUI(ILogger<LoggedChat> logger, Settings.SettingsManager.ChatLogger.Group settings, HudList hudList)
		{
			try
			{
				this.logger = logger;
				this.settings = settings;
				this.hudList = hudList;

				hudList.ClearColumnsAndRows();

				hudList.AddColumn(typeof(HudStaticText), 75, null);
				hudList.AddColumn(typeof(HudStaticText), 500, null);

				HudList.HudListRowAccessor newRow = hudList.AddRow();
				((HudStaticText)newRow[0]).Text = "Time";
				((HudStaticText)newRow[1]).Text = "Message";

				logger.LogItem += new Action<LoggedChat>(chatLogger_LogItem);
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
					logger.LogItem -= new Action<LoggedChat>(chatLogger_LogItem);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void chatLogger_LogItem(LoggedChat item)
		{
			AddItem(item);
		}

		public void AddItem(LoggedChat item)
		{
			Util.ChatChannels chatChannels = Util.ChatChannels.None;

			if (settings.Area.Value) chatChannels |= Util.ChatChannels.Area;
			if (settings.Tells.Value) chatChannels |= Util.ChatChannels.Tells;
			if (settings.Fellowship.Value) chatChannels |= Util.ChatChannels.Fellowship;
			if (settings.General.Value) chatChannels |= Util.ChatChannels.General;
			if (settings.Trade.Value) chatChannels |= Util.ChatChannels.Trade;
			if (settings.Allegiance.Value) chatChannels |= Util.ChatChannels.Allegiance;

			if ((chatChannels & item.ChatType) != 0)
			{
				// Limit the list to no more than 10,000 rows
				if (hudList.RowCount >= 10000)
				{
					for (int i = hudList.RowCount - 1; i > 9000; i--)
						hudList.RemoveRow(i);
				}

				HudList.HudListRowAccessor newRow = hudList.InsertRow(1);

				((HudStaticText)newRow[0]).Text = item.TimeStamp.ToString("yy/MM/dd HH:mm");
				((HudStaticText)newRow[1]).Text = Util.CleanMessage(item.Message);
			}
		}

		public void Clear()
		{
			hudList.ClearRows();

			HudList.HudListRowAccessor newRow = hudList.AddRow();
			((HudStaticText)newRow[0]).Text = "Time";
			((HudStaticText)newRow[1]).Text = "Message";
		}
	}
}
