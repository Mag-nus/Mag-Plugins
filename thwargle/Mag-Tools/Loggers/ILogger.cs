using System;

namespace MagTools.Loggers
{
	public interface ILogger<T>
	{
		/// <summary>
		/// This is raised when an item has been pushed by the logger.
		/// </summary>
		event Action<T> LogItem;
	}
}
