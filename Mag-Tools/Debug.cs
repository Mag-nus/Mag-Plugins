using System;
using System.IO;

namespace MagTools
{
	public static class Debug
	{
		/// <summary>
		/// This option is defaultd on.
		/// </summary>
		public static bool Enabled { private get; set; }

		static Debug()
		{
			Enabled = true;
		}

		/// <summary>
		/// This will only write the exception to the errors.txt file if DebugEnabled is true.
		/// </summary>
		/// <param name="ex"></param>
		public static void LogException(Exception ex, string note = null)
		{
			try
			{
				if (!Enabled)
					return;

				if (note != null)
					PluginCore.host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + "Note: " + note, 5);
				else
					PluginCore.host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + "Exception caught: " + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace, 5);

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
		public static void WriteToChat(string message)
		{
			try
			{
				if (!Enabled)
					return;

				PluginCore.host.Actions.AddChatText("<{" + PluginCore.PluginName + "}>: " + message, 5);
			}
			catch (Exception ex) { LogException(ex); }
		}
	}
}
