using System;
using System.IO;

namespace MagTools
{
	public static class Debug
	{
		public static bool DebugEnabled = false;

		/// <summary>
		/// This will only write the exception to the errors.txt file if Debugging is enabled.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogException(Exception ex)
		{
			try
			{
				if (!DebugEnabled)
					return;

				DirectoryInfo dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + PluginCore.PluginName);

				if (!dir.Exists)
					dir.Create();

				using (StreamWriter writer = new StreamWriter(dir.FullName + @"\Exceptions.txt", true))
				{
					writer.WriteLine("============================================================================");
					writer.WriteLine(DateTime.Now.ToString());
					writer.WriteLine("Error: " + ex.Message);
					writer.WriteLine("Source: " + ex.Source);
					writer.WriteLine("Stack: " + ex.StackTrace);
					if (ex.InnerException != null)
					{
						writer.WriteLine("Inner: " + ex.InnerException.Message);
						writer.WriteLine("Inner Stack: " + ex.InnerException.StackTrace);
					}
					writer.WriteLine("============================================================================");
					writer.WriteLine("");
					writer.Close();
				}
			}
			catch
			{
			}
		}

		/// <summary>
		/// This will only write the message to the chat if Debugging is enabled.
		/// </summary>
		/// <param name="message"></param>
		public static void WriteToChat(string message)
		{
			try
			{
				if (!DebugEnabled)
					return;

				PluginCore.host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + message, 5);
			}
			catch (Exception ex) { LogException(ex); }
		}
	}
}
