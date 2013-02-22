using System;
using System.Collections.Generic;
using System.Globalization;

namespace MagTools.Inventory
{
	[Serializable]
	public class MyWorldObject
	{
		public bool HasIdData;
		public int Id;
		public int LastIdTime;
		public int ObjectClass;

		public SerializableDictionary<int, bool> BoolValues = new SerializableDictionary<int, bool>();
		public SerializableDictionary<int, double> DoubleValues = new SerializableDictionary<int, double>();
		public SerializableDictionary<int, long> LongValues = new SerializableDictionary<int, long>();
		public SerializableDictionary<int, string> StringValues = new SerializableDictionary<int, string>();

		public List<int> ActiveSpells = new List<int>();
		public List<int> Spells = new List<int>();

		public void Init(bool hasIdData, int id, int lastIdTime, int objectClass, IDictionary<int, bool> boolValues, IDictionary<int, double> doubleValues, IDictionary<int, long> longValues, IDictionary<int, string> stringValues, IList<int> activeSpells, IList<int> spells)
		{
			HasIdData = hasIdData;
			Id = id;
			LastIdTime = lastIdTime;
			ObjectClass = objectClass;

			foreach (var kvp in boolValues)
			{
				if (boolValues.ContainsKey(kvp.Key))
					BoolValues[kvp.Key] = kvp.Value;
				else
					BoolValues.Add(kvp.Key, kvp.Value);
			}

			foreach (var kvp in doubleValues)
			{
				if (doubleValues.ContainsKey(kvp.Key))
					DoubleValues[kvp.Key] = kvp.Value;
				else
					DoubleValues.Add(kvp.Key, kvp.Value);
			}

			foreach (var kvp in longValues)
			{
				if (longValues.ContainsKey(kvp.Key))
					LongValues[kvp.Key] = kvp.Value;
				else
					LongValues.Add(kvp.Key, kvp.Value);
			}

			foreach (var kvp in stringValues)
			{
				if (stringValues.ContainsKey(kvp.Key))
					StringValues[kvp.Key] = kvp.Value;
				else
					StringValues.Add(kvp.Key, kvp.Value);
			}

			Spells.Clear();
			foreach (var i in spells)
				Spells.Add(i);

			ActiveSpells.Clear();
			foreach (var i in activeSpells)
				ActiveSpells.Add(i);
		}


		public string Material { get { if (LongValues.ContainsKey(131)) return Constants.GetMaterialInfo().ContainsKey((int)LongValues[131]) ? Constants.GetMaterialInfo()[(int)LongValues[131]] : LongValues[131].ToString(CultureInfo.InvariantCulture); return null; } }
		
		public string Name { get { return StringValues.ContainsKey(1) ? StringValues[1] : null; } }


		public string Mastery { get { if (LongValues.ContainsKey(353)) return Constants.GetMasteryInfo().ContainsKey((int)LongValues[353]) ? Constants.GetMasteryInfo()[(int)LongValues[353]] : LongValues[353].ToString(CultureInfo.InvariantCulture); return null; } }

		public string ItemSet { get { if (LongValues.ContainsKey(265)) return Constants.GetAttributeSetInfo().ContainsKey((int)LongValues[265]) ? Constants.GetAttributeSetInfo()[(int)LongValues[265]] : LongValues[131].ToString(CultureInfo.InvariantCulture); return null; } }


		public int ArmorLevel { get { return LongValues.ContainsKey(28) ? (int)LongValues[28] : -1; } }

		public string Imbue
		{
			get
			{
				if (!LongValues.ContainsKey(179) || LongValues[179] == 0) return null;

				string retVal = String.Empty;
				if ((LongValues[179] & 1) == 1) retVal += " CS";
				if ((LongValues[179] & 2) == 2) retVal += " CB";
				if ((LongValues[179] & 4) == 4) retVal += " AR";
				if ((LongValues[179] & 8) == 8) retVal += " SlashRend";
				if ((LongValues[179] & 16) == 16) retVal += " PierceRend";
				if ((LongValues[179] & 32) == 32) retVal += " BludgeRend";
				if ((LongValues[179] & 64) == 64) retVal += " AcidRend";
				if ((LongValues[179] & 128) == 128) retVal += " FrostRend";
				if ((LongValues[179] & 256) == 256) retVal += " LightRend";
				if ((LongValues[179] & 512) == 512) retVal += " FireRend";
				if ((LongValues[179] & 1024) == 1024) retVal += " MeleeImbue";
				if ((LongValues[179] & 4096) == 4096) retVal += " MagicImbue";
				if ((LongValues[179] & 8192) == 8192) retVal += " Hematited";
				if ((LongValues[179] & 536870912) == 536870912) retVal += " MagicAbsorb";
				retVal = retVal.Trim();
				return retVal;
			}
		}

		public int Tinks { get { return LongValues.ContainsKey(171) ? (int)LongValues[171] : -1; } }


		public int MaxDamage { get { return LongValues.ContainsKey(218103842) ? (int)LongValues[218103842] : -1; } }

		public int ElementalDmgBonus { get { return LongValues.ContainsKey(204) ? (int)LongValues[204] : -1; } }

		public Double Variance { get { return DoubleValues.ContainsKey(167772171) ? DoubleValues[167772171] : -1; } }

		public Double DamageBonus { get { return DoubleValues.ContainsKey(167772174) ? DoubleValues[167772174] : -1; } }

		public Double ElementalDamageVersusMonsters { get { return DoubleValues.ContainsKey(152) ? DoubleValues[152] : -1; } }

		public Double AttackBonus { get { return DoubleValues.ContainsKey(167772172) ? DoubleValues[167772172] : -1; } }

		public Double MeleeDefenseBonus { get { return DoubleValues.ContainsKey(29) ? DoubleValues[29] : -1; } }

		public Double MagicDBonus { get { return DoubleValues.ContainsKey(150) ? DoubleValues[150] : -1; } }

		public Double MissileDBonus { get { return DoubleValues.ContainsKey(149) ? DoubleValues[149] : -1; } }

		public Double ManaCBonus { get { return DoubleValues.ContainsKey(144) ? DoubleValues[144] : -1; } }


		// Wield Level
		// Skill Level

		public int LoreRequirement { get { return LongValues.ContainsKey(109) ? (int)LongValues[109] : -1; } }

		public Double SalvageWorkmanship { get { return DoubleValues.ContainsKey(167772169) ? DoubleValues[167772169] : -1; } }

		public int Workmanship { get { return LongValues.ContainsKey(105) ? (int)LongValues[105] : -1; } }

		public int Value { get { return LongValues.ContainsKey(19) ? (int)LongValues[19] : -1; } }

		public int Burden { get { return LongValues.ContainsKey(5) ? (int)LongValues[5] : -1; } }


		public int CalcedStartingArmorLevel
		{
			get
			{
				int armorFromTinks = 0;
				int armorFromBuffs = 0;

				if (Tinks > 0)
					armorFromTinks = (Imbue != null) ? (Tinks - 1) * 20 : Tinks * 20; // This assumes each tink adds an amor level of 20

				foreach (int spell in ActiveSpells)
				{
					foreach (var effect in Constants.LongValueKeySpellEffects)
					{
						if (spell == effect.Key && (int)effect.Value.Key == 28)
							armorFromBuffs -= (int)effect.Value.Change;
					}
				}

				foreach (int spell in Spells)
				{
					foreach (var effect in Constants.LongValueKeySpellEffects)
					{
						if (spell == effect.Key && (int)effect.Value.Key == 28)
							armorFromBuffs += (int)effect.Value.Bonus;
					}
				}

				return ArmorLevel - armorFromTinks - armorFromBuffs;
			}
		}

		// CalcedBuffedTinkedDamage

		// BuffedMissileDamage

		// ElementalDamageVersusMonsters

		// AttackBonus

		// MeleeDefenseBonus

		// ManaCBonus

		public override string ToString()
		{
			return Name;
		}
	}
}
