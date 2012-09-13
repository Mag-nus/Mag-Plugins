using System;
using System.Collections.Generic;

using Mag_SuitBuilder.Equipment;
using Mag_SuitBuilder.Spells;

namespace Mag_SuitBuilder.Search
{
	class CompletedSuit : Dictionary<Constants.EquippableSlotFlags, EquipmentPiece>
	{
		public override string ToString()
		{
			int totalBaseArmorLevel = 0;

			foreach (EquipmentPiece piece in Values)
				totalBaseArmorLevel += (piece.BaseArmorLevel * piece.BodyPartsCovered);

			int totalEpics = 0;
			int totalMajors = 0;

			List<Spell> spells = new List<Spell>();

			foreach (EquipmentPiece piece in Values)
			{
				foreach (Spell spell in piece.Spells)
				{
					for (int i = 0; i <= spells.Count; i++)
					{
						if (i == spells.Count)
						{
							spells.Add(spell);
							break;
						}

						if (spell.IsSameOrSurpasses(spells[i]))
						{
							spells[i] = spell;
							break;
						}
					}
				}
			}

			foreach (Spell spell in spells)
			{
				if (spell.CantripLevel >= Spell.CantripLevels.Epic)
					totalEpics++;
				else if (spell.CantripLevel >= Spell.CantripLevels.Major)
					totalMajors++;
			}

			Dictionary<string, int> setPieces = new Dictionary<string, int>();

			foreach (EquipmentPiece piece in Values)
			{
				if (!String.IsNullOrEmpty(piece.ArmorSet))
				{
					if (setPieces.ContainsKey(piece.ArmorSet))
						setPieces[piece.ArmorSet]++;
					else
						setPieces.Add(piece.ArmorSet, 1);
				}
			}

			string sets = null;

			foreach (KeyValuePair<string, int> kvp in setPieces)
			{
				if (sets != null)
					sets += ", ";

				sets += kvp.Key + " " + kvp.Value;

			}

			return Count + ", AL: " + totalBaseArmorLevel + ", Epics: " + totalEpics + ", Majors: " + totalMajors + ", " + sets;
		}
	}
}
