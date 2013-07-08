
namespace MagTools.Loggers
{
	public interface ILoggerTarget<T>
	{
		void AddItem(T item);

		void Clear();
	}
}
