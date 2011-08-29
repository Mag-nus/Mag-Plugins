using System;
using System.IO;

namespace MagTools
{
	public static class Debug
	{
		/// <summary>
		/// This option is defaultd on.
		/// </summary>
		public static bool DebugEnabled { private get; set; }

		static Debug()
		{
			DebugEnabled = true;
		}

		/// <summary>
		/// This will only write the exception to the errors.txt file if DebugEnabled is true.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogException(Exception ex)
		{
			try
			{
				if (!DebugEnabled)
					return;

				PluginCore.host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace, 5);

				using (StreamWriter writer = new StreamWriter(PluginCore.PluginPersonalFolder.FullName + @"\Exceptions.txt", true))
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
				// Eat the exception, yumm.
			}
		}

		/// <summary>
		/// This will only write the message to the chat if DebugEnabled is true.
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
