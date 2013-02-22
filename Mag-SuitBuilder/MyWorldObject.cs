using System;
using System.Collections.Generic;

namespace Mag_SuitBuilder
{
	[Serializable]
	public class MyWorldObject
	{
		public int ActiveSpellCount;
		public int Behavior;
		//public List<int> BoolKeys;
		public int Category;
		public int Container;
		//public List<int> DoubleKeys;
		public int GameDataFlags1;
		public bool HasIdData;
		public int Icon;
		public int Id;
		public int LastIdTime;
		//public List<int> LongKeys;
		public string Name;
		public string ObjectClass;
		public int PhysicsDataFlags;
		//public int SpellCount;
		//public List<int> StringKeys;
		public int Type;

		public SerializableDictionary<int, bool> BoolValues = new SerializableDictionary<int, bool>();
		public SerializableDictionary<int, double> DoubleValues = new SerializableDictionary<int, double>();
		public SerializableDictionary<int, long> LongValues = new SerializableDictionary<int, long>();
		public SerializableDictionary<int, string> StringValues = new SerializableDictionary<int, string>();

		public List<int> Spells = new List<int>();

		// QuadValues

		public MyWorldObject()
		{
			
		}
	}
}
