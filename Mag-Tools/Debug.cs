using System;
using System.IO;

using Decal.Adapter;

namespace MagTools
{
	static class Debug
	{
		/// <summary>
		/// This will only write the exception to the errors.txt file if DebugEnabled is true.
		/// </summary>
		/// <param name="ex"></param>
		/// <param name="note"></param>
		public static void LogException(Exception ex, string note = null)
		{
			try
			{
				if (!Settings.SettingsManager.Misc.DebuggingEnabled.Value)
					return;

				if (note != null)
					CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + "Note: " + note, 5);
				else
					CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace, 5);

				using (StreamWriter writer = new StreamWriter(PluginCore.PluginPersonalFolder.FullName + @"\Exceptions.txt", true))
				{
					writer.WriteLine("============================================================================");

					writer.WriteLine(DateTime.Now.ToString());
					writer.WriteLine(ex);

					/*
					writer.WriteLine(DateTime.Now.ToString());
					writer.WriteLine("Error: " + ex.Message);
					writer.WriteLine("Source: " + ex.Source);
					writer.WriteLine("Stack: " + ex.StackTrace);

					Exception innerException = ex.InnerException;

					while (innerException != null)
					{
						writer.WriteLine("Inner: " + ex.InnerException.Message);
						writer.WriteLine("Inner Stack: " + ex.InnerException.StackTrace);

						innerException = innerException.InnerException;
					}
					*/

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
				if (!Settings.SettingsManager.Misc.DebuggingEnabled.Value)
					return;

				CoreManager.Current.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + message, color, target);
			}
			catch (Exception ex) { LogException(ex); }
		}
	}
}
