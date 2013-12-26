using System;
using System.Collections.Generic;
using System.Globalization;

using Mag.Shared.Constants;

namespace Mag.Shared
{
	/// <summary>
	/// This is a user defiend object to represent a Decal.Adapter.WorldObject.
	/// All properties will return -1 if the property isn't supported.
	/// </summary>
	[Serializable]
	public class MyWorldObject
	{
		public bool HasIdData;
		public int Id;
		public int LastIdTime;
		public int ObjectClass;

		public SerializableDictionary<int, bool> BoolValues = new SerializableDictionary<int, bool>();
		public SerializableDictionary<int, double> DoubleValues = new SerializableDictionary<int, double>();
		public SerializableDictionary<int, int> IntValues = new SerializableDictionary<int, int>();
		public SerializableDictionary<int, string> StringValues = new SerializableDictionary<int, string>();

		public List<int> ActiveSpells = new List<int>();
		public List<int> Spells = new List<int>();

		public void Init(bool hasIdData, int id, int lastIdTime, int objectClass, IDictionary<int, bool> boolValues, IDictionary<int, double> doubleValues, IDictionary<int, int> intValues, IDictionary<int, string> stringValues, IList<int> activeSpells, IList<int> spells)
		{
			HasIdData = hasIdData;
			Id = id;
			LastIdTime = lastIdTime;
			ObjectClass = objectClass;

			AddTo(boolValues, doubleValues, intValues, stringValues);

			ActiveSpells.Clear();
			foreach (var i in activeSpells)
				ActiveSpells.Add(i);

			Spells.Clear();
			foreach (var i in spells)
				Spells.Add(i);
		}

		public void AddTo(IDictionary<int, bool> boolValues, IDictionary<int, double> doubleValues, IDictionary<int, int> intValues, IDictionary<int, string> stringValues)
		{
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

			foreach (var kvp in intValues)
			{
				if (intValues.ContainsKey(kvp.Key))
					IntValues[kvp.Key] = kvp.Value;
				else
					IntValues.Add(kvp.Key, kvp.Value);
			}

			foreach (var kvp in stringValues)
			{
				if (stringValues.ContainsKey(kvp.Key))
					StringValues[kvp.Key] = kvp.Value;
				else
					StringValues.Add(kvp.Key, kvp.Value);
			}
		}


		public string Material { get { if (IntValues.ContainsKey(131)) return Dictionaries.MaterialInfo.ContainsKey(IntValues[131]) ? Dictionaries.MaterialInfo[IntValues[131]] : IntValues[131].ToString(CultureInfo.InvariantCulture); return null; } }
		
		public string Name { get { return StringValues.ContainsKey(1) ? StringValues[1] : null; } }


		public string EquipSkill { get { if (IntValues.ContainsKey(218103840)) return Dictionaries.SkillInfo.ContainsKey(IntValues[218103840]) ? Dictionaries.SkillInfo[IntValues[218103840]] : IntValues[218103840].ToString(CultureInfo.InvariantCulture); return null; } }

		public string Mastery { get { if (IntValues.ContainsKey(353)) return Dictionaries.MasteryInfo.ContainsKey(IntValues[353]) ? Dictionaries.MasteryInfo[IntValues[353]] : IntValues[353].ToString(CultureInfo.InvariantCulture); return null; } }

		public string ItemSet { get { if (IntValues.ContainsKey(265)) return Dictionaries.AttributeSetInfo.ContainsKey(IntValues[265]) ? Dictionaries.AttributeSetInfo[IntValues[265]] : IntValues[265].ToString(CultureInfo.InvariantCulture); return null; } }


		public int ArmorLevel { get { return IntValues.ContainsKey(28) ? IntValues[28] : -1; } }

		public string Imbue
		{
			get
			{
				if (!IntValues.ContainsKey(179) || IntValues[179] == 0) return null;

				string retVal = String.Empty;
				if ((IntValues[179] & 1) == 1) retVal += " CS";
				if ((IntValues[179] & 2) == 2) retVal += " CB";
				if ((IntValues[179] & 4) == 4) retVal += " AR";
				if ((IntValues[179] & 8) == 8) retVal += " SlashRend";
				if ((IntValues[179] & 16) == 16) retVal += " PierceRend";
				if ((IntValues[179] & 32) == 32) retVal += " BludgeRend";
				if ((IntValues[179] & 64) == 64) retVal += " AcidRend";
				if ((IntValues[179] & 128) == 128) retVal += " FrostRend";
				if ((IntValues[179] & 256) == 256) retVal += " LightRend";
				if ((IntValues[179] & 512) == 512) retVal += " FireRend";
				if ((IntValues[179] & 1024) == 1024) retVal += " MeleeImbue";
				if ((IntValues[179] & 4096) == 4096) retVal += " MagicImbue";
				if ((IntValues[179] & 8192) == 8192) retVal += " Hematited";
				if ((IntValues[179] & 536870912) == 536870912) retVal += " MagicAbsorb";
				retVal = retVal.Trim();
				return retVal;
			}
		}

		public int Tinks { get { return IntValues.ContainsKey(171) ? IntValues[171] : -1; } }


		public int MaxDamage { get { return IntValues.ContainsKey(218103842) ? IntValues[218103842] : -1; } }

		public int ElementalDmgBonus { get { return IntValues.ContainsKey(204) ? IntValues[204] : -1; } }

		public Double Variance { get { return DoubleValues.ContainsKey(167772171) ? DoubleValues[167772171] : -1; } }

		public Double DamageBonus { get { return DoubleValues.ContainsKey(167772174) ? DoubleValues[167772174] : -1; } }

		public Double ElementalDamageVersusMonsters { get { return DoubleValues.ContainsKey(152) ? DoubleValues[152] : -1; } }

		public Double AttackBonus { get { return DoubleValues.ContainsKey(167772172) ? DoubleValues[167772172] : -1; } }

		public Double MeleeDefenseBonus { get { return DoubleValues.ContainsKey(29) ? DoubleValues[29] : -1; } }

		public Double MagicDBonus { get { return DoubleValues.ContainsKey(150) ? DoubleValues[150] : -1; } }

		public Double MissileDBonus { get { return DoubleValues.ContainsKey(149) ? DoubleValues[149] : -1; } }

		public Double ManaCBonus { get { return DoubleValues.ContainsKey(144) ? DoubleValues[144] : -1; } }


		public int WieldLevel { get { if (IntValues.ContainsKey(160) && IntValues[160] > 0 && IntValues.ContainsKey(158) && IntValues[158] == 7 && IntValues.ContainsKey(159) && IntValues[159] == 1) return IntValues[160]; return -1; }  }

		public int SkillLevel { get { if (IntValues.ContainsKey(160) && IntValues[160] > 0 && (!IntValues.ContainsKey(158) || IntValues[158] != 7) && IntValues.ContainsKey(159)) return IntValues[160]; return -1; } }


		public int LoreRequirement { get { return IntValues.ContainsKey(109) ? IntValues[109] : -1; } }

		public Double SalvageWorkmanship { get { return DoubleValues.ContainsKey(167772169) ? DoubleValues[167772169] : -1; } }

		public int Workmanship { get { return IntValues.ContainsKey(105) ? IntValues[105] : -1; } }

		public int Value { get { return IntValues.ContainsKey(19) ? IntValues[19] : -1; } }

		public int Burden { get { return IntValues.ContainsKey(5) ? IntValues[5] : -1; } }


		public int DamRating { get { return IntValues.ContainsKey(370) ? IntValues[370] : -1; } }

		public int DamResistRating { get { return IntValues.ContainsKey(371) ? IntValues[371] : -1; } }

		public int CritRating { get { return IntValues.ContainsKey(372) ? IntValues[372] : -1; } }

		public int CritResistRating { get { return IntValues.ContainsKey(373) ? IntValues[373] : -1; } }

		public int CritDamRating { get { return IntValues.ContainsKey(374) ? IntValues[374] : -1; } }

		public int CritDamResistRating { get { return IntValues.ContainsKey(375) ? IntValues[375] : -1; } }

		public int HealBoostRating { get { return IntValues.ContainsKey(376) ? IntValues[376] : -1; } }

		public int VitalityRating { get { return IntValues.ContainsKey(379) ? IntValues[379] : -1; } }

		/// <summary>
		/// Returns the sum of all the ratings found on this item, or -1 if no ratings exist.
		/// </summary>
		public int TotalRating
		{ 
			get
			{
				if (DamRating == -1 && DamResistRating == -1 && CritRating == -1 && CritResistRating == -1 && CritDamRating == -1 && CritDamResistRating == -1 && HealBoostRating == -1 && VitalityRating == -1)
					return -1;

				return Math.Max(DamRating, 0) + Math.Max(DamResistRating, 0) + Math.Max(CritRating, 0) + Math.Max(CritResistRating, 0) + Math.Max(CritDamRating, 0) + Math.Max(CritDamResistRating, 0) + Math.Max(HealBoostRating, 0) + Math.Max(VitalityRating, 0);
			}
		}


		/// <summary>
		/// This will take the current AmorLevel of the item, subtract any buffs, subtract tinks as 20 AL each (not including imbue), and add any impen cantrips.
		/// </summary>
		public int CalcedStartingArmorLevel
		{
			get
			{
				int armorFromTinks = 0;
				int armorFromBuffs = 0;

				if (Tinks > 0 && ArmorLevel > 0)
					armorFromTinks = (Imbue != null) ? (Tinks - 1) * 20 : Tinks * 20; // This assumes each tink adds an amor level of 20

				if ((!IntValues.ContainsKey(131) || IntValues[131] == 0) && ArmorLevel > 0) // If this item has no material, its not a loot gen, assume its a quest item and subtract 200 al
					armorFromTinks = 200;

				foreach (int spell in ActiveSpells)
				{
					foreach (var effect in Dictionaries.LongValueKeySpellEffects)
					{
						if (spell == effect.Key && effect.Value.Key == 28)
							armorFromBuffs += effect.Value.Change;
					}
				}

				foreach (int spell in Spells)
				{
					foreach (var effect in Dictionaries.LongValueKeySpellEffects)
					{
						if (spell == effect.Key && effect.Value.Key == 28)
							armorFromBuffs -= effect.Value.Bonus;
					}
				}

				return ArmorLevel - armorFromTinks - armorFromBuffs;
			}
		}

		/// <summary>
		/// This will take into account Variance, MaxDamage and Tinks of a melee weapon and determine what its optimal 10 tinked DamageOverTime is.
		/// </summary>
		public double CalcedBuffedTinkedDoT
		{
			get
			{
				if (!DoubleValues.ContainsKey(167772171) || !IntValues.ContainsKey(218103842))
					return -1;

				double variance = DoubleValues.ContainsKey(167772171) ? DoubleValues[167772171] : 0;
				int maxDamage = GetBuffedIntValueKey(218103842);

				int numberOfTinksLeft = Math.Max(10 - Math.Max(Tinks, 0), 0);

				if (!IntValues.ContainsKey(179) || IntValues[179] == 0)
					numberOfTinksLeft--; // Factor in an imbue tink

				// If this is not a loot generated item, it can't be tinked
				if (!IntValues.ContainsKey(131) || IntValues[131] == 0)
					numberOfTinksLeft = 0;

				for (int i = 1; i <= numberOfTinksLeft; i++)
				{
					double ironTinkDoT = CalculateDamageOverTime(maxDamage + 24 + 1, variance);
					double graniteTinkDoT = CalculateDamageOverTime(maxDamage + 24, variance * .8);

					if (ironTinkDoT >= graniteTinkDoT)
						maxDamage++;
					else
						variance *= .8;
				}

				return CalculateDamageOverTime(maxDamage + 24, variance);
			}
		}

		/// <summary>
		///  GetBuffedIntValueKey(LongValueKey.MaxDamage) + (((GetBuffedDoubleValueKey(DoubleValueKey.DamageBonus) - 1) * 100) / 3) + GetBuffedIntValueKey(LongValueKey.ElementalDmgBonus);
		/// </summary>
		public double CalcedBuffedMissileDamage { get { if (!IntValues.ContainsKey(218103842) || !DoubleValues.ContainsKey(167772174) || !IntValues.ContainsKey(204)) return -1; return GetBuffedIntValueKey(218103842) + (((GetBuffedDoubleValueKey(167772174) - 1) * 100) / 3) + GetBuffedIntValueKey(204); } }

		public double BuffedElementalDamageVersusMonsters { get { return GetBuffedDoubleValueKey(152, -1); } }

		public double BuffedAttackBonus { get { return GetBuffedDoubleValueKey(167772172, -1); } }

		public double BuffedMeleeDefenseBonus { get { return GetBuffedDoubleValueKey(29, -1); } }

		public double BuffedManaCBonus { get { return GetBuffedDoubleValueKey(144, -1); } }

		public int GetBuffedIntValueKey(int key, int defaultValue = 0)
		{
			if (!IntValues.ContainsKey(key))
				return defaultValue;

			int value = IntValues[key];

			foreach (int spell in ActiveSpells)
			{
				if (Dictionaries.LongValueKeySpellEffects.ContainsKey(spell) && Dictionaries.LongValueKeySpellEffects[spell].Key == key)
					value -= Dictionaries.LongValueKeySpellEffects[spell].Change;
			}

			foreach (int spell in Spells)
			{
				if (Dictionaries.LongValueKeySpellEffects.ContainsKey(spell) && Dictionaries.LongValueKeySpellEffects[spell].Key == key)
					value += Dictionaries.LongValueKeySpellEffects[spell].Bonus;
			}

			return value;
		}

		public double GetBuffedDoubleValueKey(int key, double defaultValue = 0)
		{
			if (!DoubleValues.ContainsKey(key))
				return defaultValue;

			double value = DoubleValues[key];

			foreach (int spell in ActiveSpells)
			{
				if (Dictionaries.DoubleValueKeySpellEffects.ContainsKey(spell) && Dictionaries.DoubleValueKeySpellEffects[spell].Key == key)
				{
					if (Math.Abs(Dictionaries.DoubleValueKeySpellEffects[spell].Change - 1) < Double.Epsilon)
						value /= Dictionaries.DoubleValueKeySpellEffects[spell].Change;
					else
						value -= Dictionaries.DoubleValueKeySpellEffects[spell].Change;
				}
			}

			foreach (int spell in Spells)
			{
				if (Dictionaries.DoubleValueKeySpellEffects.ContainsKey(spell) && Dictionaries.DoubleValueKeySpellEffects[spell].Key == key && Math.Abs(Dictionaries.DoubleValueKeySpellEffects[spell].Bonus - 0) > Double.Epsilon)
				{
					if (Math.Abs(Dictionaries.DoubleValueKeySpellEffects[spell].Change - 1) < Double.Epsilon)
						value *= Dictionaries.DoubleValueKeySpellEffects[spell].Bonus;
					else
						value += Dictionaries.DoubleValueKeySpellEffects[spell].Bonus;
				}
			}

			return value;
		}

		/// <summary>
		/// maxDamage * ((1 - critChance) * (2 - variance) / 2 + (critChance * critMultiplier));
		/// </summary>
		/// <param name="maxDamage"></param>
		/// <param name="variance"></param>
		/// <param name="critChance"></param>
		/// <param name="critMultiplier"></param>
		/// <returns></returns>
		public static double CalculateDamageOverTime(int maxDamage, double variance, double critChance = .1, double critMultiplier = 2)
		{
			return maxDamage * ((1 - critChance) * (2 - variance) / 2 + (critChance * critMultiplier));
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
