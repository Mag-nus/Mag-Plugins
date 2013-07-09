using System;
using System.Collections.Generic;
using System.IO;

namespace MagTools.Loggers.Chat
{
	class BufferedChatLogFileWriter : IDisposable
	{
		public string FileName;
		readonly ILogger<LoggedChat> logger;

		readonly List<LoggedChat> items = new List<LoggedChat>();

		readonly System.Windows.Forms.Timer saveTimer = new System.Windows.Forms.Timer();

		public BufferedChatLogFileWriter(string fileName, ILogger<LoggedChat> logger, TimeSpan saveInterval)
		{
			FileName = fileName;
			this.logger = logger;

			logger.LogItem += new Action<LoggedChat>(logger_LogItem);

			saveTimer.Tick += new EventHandler(saveTimer_Tick);
			saveTimer.Interval = (int)saveInterval.TotalMilliseconds;
			saveTimer.Start();
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
					logger.LogItem -= new Action<LoggedChat>(logger_LogItem);

					saveTimer.Stop();
					saveTimer.Tick -= new EventHandler(saveTimer_Tick);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void logger_LogItem(LoggedChat item)
		{
			if (!Settings.SettingsManager.ChatLogger.Persistent.Value)
				return;

			items.Add(item);
		}

		void saveTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				Flush();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
		
		public void Flush()
		{
			if (!Settings.SettingsManager.ChatLogger.Persistent.Value)
				return;

			if (String.IsNullOrEmpty(FileName))
				return;

			using (StreamWriter sw = File.AppendText(FileName))
			{
				foreach (var item in items)
					sw.WriteLine(item.TimeStamp.ToString("yy/MM/dd HH:mm:ss") + "," + item.ChatType.ToString() + "," + item.Message);
			}

			items.Clear();
		}
	}
}
