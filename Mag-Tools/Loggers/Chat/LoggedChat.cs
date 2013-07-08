using System;

using Mag.Shared;

namespace MagTools.Loggers.Chat
{
	public class LoggedChat
	{
		public readonly DateTime TimeStamp;

		public readonly Util.ChatChannels ChatType;

		public readonly string Message;

		public LoggedChat(DateTime timeStamp, Util.ChatChannels chatType, string message)
		{
			TimeStamp = timeStamp;

			ChatType = chatType;

			Message = message;
		}
	}
}
