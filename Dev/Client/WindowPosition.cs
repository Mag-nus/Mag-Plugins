
namespace MagTools.Client
{
	struct WindowPosition
	{
		public string Server;
		public string AccountName;

		public int X;
		public int Y;

		public WindowPosition(string server, string accountName, int x, int y)
		{
			Server = server;
			AccountName = accountName;

			X = x;
			Y = y;
		}
	}
}
