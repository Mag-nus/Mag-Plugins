using System;

namespace MagFilter
{
	public class Character
	{
        public int Id { get; set; }

	    public string Name { get; set; }

        public TimeSpan DeleteTimeout { get; set; }

		public Character(int id, string name, int timeout)
		{
			Id = id;

			Name = name;

			DeleteTimeout = TimeSpan.FromSeconds(timeout);
		}
	}
}
