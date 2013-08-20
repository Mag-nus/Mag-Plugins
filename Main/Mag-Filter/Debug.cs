using System;
using System.Globalization;
using System.IO;

using Decal.Adapter;

namespace MagFilter
{
	static class Debug
	{
		public static void LogException(Exception ex, string note = null)
		{
			try
			{
				if (note != null)
					CoreManager.Current.Actions.AddChatText("<{" + FilterCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + "Note: " + note, 5);
				else
					CoreManager.Current.Actions.AddChatText("<{" + FilterCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace, 5);

				using (StreamWriter writer = new StreamWriter(FilterCore.PluginPersonalFolder.FullName + @"\Exceptions.txt", true))
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
				CoreManager.Current.Actions.AddChatText("<{" + FilterCore.PluginName + "}>: " + "Log Text: " + text, 5);

				using (StreamWriter writer = new StreamWriter(FilterCore.PluginPersonalFolder.FullName + @"\Exceptions.txt", true))
				{
					writer.WriteLine(DateTime.Now + ": " + text);
					writer.Close();
				}
			}
			catch
			{
				// Eat the exception, yumm.
			}
		}
	}
}
