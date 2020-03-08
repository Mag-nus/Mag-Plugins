using System;
using System.IO;

using Decal.Adapter;

namespace Mag_PacketLogger
{
	[FriendlyName("Mag-PacketLogger")]
	public class FilterCore : FilterBase
    {
		internal static string PluginName = "Mag-PacketLogger";

		internal static DirectoryInfo PluginPersonalFolder
		{
			get
			{
				DirectoryInfo pluginPersonalFolder = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\Decal Plugins\" + PluginName);

				try
				{
					if (!pluginPersonalFolder.Exists)
						pluginPersonalFolder.Create();
				}
				catch { }

				return pluginPersonalFolder;
			}
		}

		protected override void Startup()
		{


			ClientDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch += new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
		}

		protected override void Shutdown()
		{
			ClientDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ClientDispatch);
			ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(FilterCore_ServerDispatch);
		}

		void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				LogMessage(MessageDirection.Outbound, e.Message);
			}
			catch { }
		}

		void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{
				LogMessage(MessageDirection.Inbound, e.Message);
			}
			catch { }
		}

		private void LogMessage(MessageDirection direction, Message message)
		{
			string logFileName = PluginPersonalFolder.FullName + @"\" + CoreManager.Current.CharacterFilter.AccountName + ".csv";

			FileInfo logFile = new FileInfo(logFileName);

			if (!logFile.Exists)
			{
				using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
				{
					writer.WriteLine("Direction,Type,Count,Next == null,Parent == null,RawData");

					writer.Close();
				}
			}

			using (StreamWriter writer = new StreamWriter(logFile.FullName, true))
			{
				writer.WriteLine(direction + "," + message.Type + "," + message.Count + "," + (message.Next == null) + "," + (message.Parent == null) + " " + ByteArrayToHex(message.RawData));

				writer.Close();
			}
		}

		internal static string ByteArrayToHex(byte[] array)
		{
			char[] c = new char[array.Length * 2 + (Math.Max(0, array.Length - 1))];

			for (int i = 0; i < array.Length; ++i)
			{
				byte b = ((byte)(array[i] >> 4));

				c[i * 2 + i] = (char)(b > 9 ? b + 0x37 : b + 0x30);

				b = ((byte)(array[i] & 0xF));

				c[i * 2 + 1 + i] = (char)(b > 9 ? b + 0x37 : b + 0x30);

				if (i < array.Length - 1)
					c[i * 2 + 2 + i] = (char)0x20;
			}

			return new string(c);
		}
	}
}
