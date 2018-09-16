using System;

namespace MagFilter
{
	struct Character
	{
		public readonly int Id;

		public readonly string Name;

		public readonly TimeSpan DeleteTimeout;

		public Character(int id, string name, int timeout)
		{
			Id = id;

			Name = name;

			DeleteTimeout = TimeSpan.FromSeconds(timeout);
		}
	}
}
