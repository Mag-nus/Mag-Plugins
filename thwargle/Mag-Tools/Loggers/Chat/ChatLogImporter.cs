using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Mag.Shared;

namespace MagTools.Loggers.Chat
{
	static class ChatLogImporter
	{
		public static bool Import(string fileName, ILoggerTarget<LoggedChat> target, int limitEndOfStreamImportToNumberOfBytes = 1048576)
		{
			List<ILoggerTarget<LoggedChat>> list = new List<ILoggerTarget<LoggedChat>>();

			list.Add(target);

			return Import(fileName, list, limitEndOfStreamImportToNumberOfBytes);
		}

		public static bool Import(string fileName, IList<ILoggerTarget<LoggedChat>> targets, int limitEndOfStreamImportToNumberOfBytes = 1048576)
		{
			if (targets == null || targets.Count == 0)
				return false;

			FileInfo fileInfo = new FileInfo(fileName);

			if (!fileInfo.Exists)
				return false;


			using (StreamReader sr = new StreamReader(fileName))
			{
				if (limitEndOfStreamImportToNumberOfBytes >= 0 && sr.BaseStream.Length > limitEndOfStreamImportToNumberOfBytes)
					sr.BaseStream.Position = sr.BaseStream.Length - limitEndOfStreamImportToNumberOfBytes;

				string line;

				while ((line = sr.ReadLine()) != null)
				{
					LoggedChat convertedLine;

					if (ConvertLine(line, out convertedLine))
					{
						foreach (var target in targets)
						{
							if (target == null)
								continue;

							target.AddItem(convertedLine);
						}
					}
				}
			}

			return true;
		}

		static bool ConvertLine(string line, out LoggedChat convertedLine)
		{
			convertedLine = null;

			string[] split = line.Split(',');

			if (split.Length < 3)
				return false;

			DateTime timeStamp;
			Util.ChatChannels chatType;

			if (split[0].Contains("/"))
			{
				if (!DateTime.TryParseExact(split[0], "yy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStamp))
					return false;

				chatType = (Util.ChatChannels)Enum.Parse(typeof(Util.ChatChannels), split[1]);
			}
			else
			{
				if (!DateTime.TryParseExact(split[0], "yyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeStamp))
					return false;

				int chatTypeInt;

				if (!int.TryParse(split[1], out chatTypeInt))
					return false;

				chatType = (Util.ChatChannels)chatTypeInt;
			}

			string message = line.Substring(split[0].Length + 1 + split[1].Length + 1, line.Length - (split[0].Length + 1 + split[1].Length + 1));

			convertedLine = new LoggedChat(timeStamp, chatType, message);

			return true;
		}
	}
}
