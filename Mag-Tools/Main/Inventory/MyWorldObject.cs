using System;
using System.Collections.Generic;

using Decal.Adapter.Wrappers;

namespace MagTools.Inventory
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
		public string ObjectClass; // I should have made this an int. :(
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

		public MyWorldObject(WorldObject wo)
		{
			InitFromObject(wo);
		}

		void InitFromObject(WorldObject wo, bool refreshExisting = false)
		{
			ActiveSpellCount = wo.ActiveSpellCount;
			Behavior = wo.Behavior;
			//BoolKeys
			Category = wo.Category;
			Container = wo.Container;
			//DoubleKeys
			GameDataFlags1 = wo.GameDataFlags1;
			HasIdData = wo.HasIdData;
			Icon = wo.Icon;
			Id = wo.Id;
			LastIdTime = wo.LastIdTime;
			//LongKeys
			Name = wo.Name;
			ObjectClass = wo.ObjectClass.ToString();
			PhysicsDataFlags = wo.PhysicsDataFlags;
			//SpellCount
			//StringKeys
			Type = wo.Type;

			if (!refreshExisting)
				BoolValues.Clear();
			foreach (var key in wo.BoolKeys)
			{
				if (refreshExisting && BoolValues.ContainsKey(key))
				{
					BoolValues[key] = wo.Values((BoolValueKey)key);
					continue;
				}

				BoolValues.Add(key, wo.Values((BoolValueKey) key));
			}


			if (!refreshExisting)
				DoubleValues.Clear();
			foreach (var key in wo.DoubleKeys)
			{
				if (refreshExisting && DoubleValues.ContainsKey(key))
				{
					DoubleValues[key] = wo.Values((DoubleValueKey)key);
					continue;
				}

				DoubleValues.Add(key, wo.Values((DoubleValueKey) key));
			}

			if (!refreshExisting)
				LongValues.Clear();
			foreach (var key in wo.LongKeys)
			{
				if (refreshExisting && LongValues.ContainsKey(key))
				{
					LongValues[key] = wo.Values((LongValueKey)key);
					continue;
				}

				LongValues.Add(key, wo.Values((LongValueKey) key));
			}

			if (!refreshExisting)
				StringValues.Clear();
			foreach (var key in wo.StringKeys)
			{
				if (refreshExisting && StringValues.ContainsKey(key))
				{
					StringValues[key] = wo.Values((StringValueKey)key);
					continue;
				}

				StringValues.Add(key, wo.Values((StringValueKey) key));
			}

			if (!refreshExisting)
			{
				Spells.Clear();
				for (int i = 0; i < wo.SpellCount; i++)
					Spells.Add(wo.Spell(i));
			}
		}

		public void UpdateFromObject(WorldObject wo)
		{
			if (wo.HasIdData || !HasIdData)
				InitFromObject(wo);
			else
				InitFromObject(wo, true);
		}
	}
}
