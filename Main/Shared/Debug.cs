using System;
using System.Globalization;
using System.IO;

using Decal.Adapter;

namespace Mag.Shared
{
	static class Debug
	{
		static string _errorLogPath;

		static string _pluginName;

		public static void Init(string errorLogPath, string pluginName)
		{
			_errorLogPath = errorLogPath;

			_pluginName = pluginName;
		}

		/// <summary>
		/// This will only write the exception to the errors.txt file if DebugEnabled is true.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="note"></param>
		public static void LogException(Exception ex, string note = null)
		{
			try
			{
				//if (!Settings.SettingsManager.Misc.DebuggingEnabled.Value)
				//	return;

				if (note != null)
					CoreManager.Current.Actions.AddChatText("<{" + _pluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + "Note: " + note, 5);
				else
					CoreManager.Current.Actions.AddChatText("<{" + _pluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace, 5);

				if (String.IsNullOrEmpty(_errorLogPath))
					return;

				FileInfo fileInfo = new FileInfo(_errorLogPath);

				// Limit the file to 1MB
				bool append = !(fileInfo.Exists && fileInfo.Length > 1048576);

				using (StreamWriter writer = new StreamWriter(fileInfo.FullName, append))
				{
					writer.WriteLine("============================================================================");

					writer.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture));
					writer.WriteLine(ex);

					if (note != null)
						writer.WriteLine("Note: " + note);

					writer.WriteLine("============================================================================");
					writer.WriteLine("");
					writer.Close();
				}
			}
			catch
			{
				// Eat the exception, yumm.
			}
		}

		public static void LogText(string text)
		{
			try
			{
				//if (!Settings.SettingsManager.Misc.DebuggingEnabled.Value)
				//	return;

				CoreManager.Current.Actions.AddChatText("<{" + _pluginName + "}>: " + "Log Text: " + text, 5);

				if (String.IsNullOrEmpty(_errorLogPath))
					return;

				FileInfo fileInfo = new FileInfo(_errorLogPath);

				// Limit the file to 1MB
				bool append = !(fileInfo.Exists && fileInfo.Length > 1048576);

				using (StreamWriter writer = new StreamWriter(fileInfo.FullName, append))
				{
					writer.WriteLine(DateTime.Now + ": " + text);
					writer.Close();
				}
			}
			catch (Exception ex) { LogException(ex); }
		}

		/// <summary>
		/// This will only write the message to the chat if DebugEnabled is true.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="color"></param>
		/// <param name="target"></param>
		public static void WriteToChat(string message, int color = 5, int target = 1)
		{
			try
			{
				//if (!Settings.SettingsManager.Misc.DebuggingEnabled.Value)
				//	return;

				CoreManager.Current.Actions.AddChatText("<{" + _pluginName + "}>: " + message, color, target);
			}
			catch (Exception ex) { LogException(ex); }
		}
	}
}
