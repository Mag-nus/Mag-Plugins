using System.Collections.Generic;

using Decal.Adapter.Wrappers;

namespace Mag.Shared
{
	public static class MyWorldObjectCreator
	{
		public static MyWorldObject Create(WorldObject wo)
		{
			MyWorldObject mwo = new MyWorldObject();

			Dictionary<int, bool> boolValues = new Dictionary<int,bool>();
			Dictionary<int, double> doubleValues = new Dictionary<int,double>();
			Dictionary<int, int> intValues = new Dictionary<int, int>();
			Dictionary<int, string> stringValues = new Dictionary<int,string>();
			List<int> activeSpells = new List<int>();
			List<int> spells = new List<int>();

			foreach (var key in wo.BoolKeys)
				boolValues.Add(key, wo.Values((BoolValueKey)key));

			foreach (var key in wo.DoubleKeys)
				doubleValues.Add(key, wo.Values((DoubleValueKey)key));

			foreach (var key in wo.LongKeys)
				intValues.Add(key, wo.Values((LongValueKey)key));

			foreach (var key in wo.StringKeys)
				stringValues.Add(key, wo.Values((StringValueKey)key));

			for (int i = 0 ; i < wo.ActiveSpellCount ; i++)
				activeSpells.Add(wo.ActiveSpell(i));

			for (int i = 0; i < wo.SpellCount; i++)
				spells.Add(wo.Spell(i));

			mwo.Init(wo.HasIdData, wo.Id, wo.LastIdTime, (int)wo.ObjectClass, boolValues, doubleValues, intValues, stringValues, activeSpells, spells);

			return mwo;
		}

		public static MyWorldObject Combine(MyWorldObject older, WorldObject newer)
		{
			if (!older.HasIdData || newer.HasIdData)
				return Create(newer);

			MyWorldObject mwo = Create(newer);

			older.AddTo(mwo.BoolValues, mwo.DoubleValues, mwo.IntValues, mwo.StringValues);

			return older;
		}
	}
}
