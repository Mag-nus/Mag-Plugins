using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Mag_SuitBuilder.Spells
{
	/// <summary>
	/// Use GetSpell() to intialize a Spell object
	/// </summary>
	public class Spell
	{
		static readonly List<string> SpellTableHeader = new List<string>();
		static readonly Collection<string[]> SpellTable = new Collection<string[]>();

		static Spell()
		{
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Mag_SuitBuilder.Spells.Spells.csv"))
			using (StreamReader reader = new StreamReader(stream))
			{
				SpellTableHeader = new List<string>(reader.ReadLine().Split(','));

				while (!reader.EndOfStream)
					SpellTable.Add(reader.ReadLine().Split(','));
			}
		}

		private Spell(string name)
		{
			Name = name;

			if (name.EndsWith(" I"))
				BuffLevel = BuffLevels.I;
			else if (name.EndsWith(" II"))
				BuffLevel = BuffLevels.II;
			else if (name.EndsWith(" III"))
				BuffLevel = BuffLevels.III;
			else if (name.EndsWith(" IV"))
				BuffLevel = BuffLevels.IV;
			else if (name.EndsWith(" V"))
				BuffLevel = BuffLevels.V;
			else if (name.EndsWith(" VI"))
				BuffLevel = BuffLevels.VI;
			else if (name.EndsWith(" VII")) // This doesn't pick up every lvl 7
				BuffLevel = BuffLevels.VII;
			else if (name.StartsWith("Incantation "))
				BuffLevel = BuffLevels.VIII;

			if (name.StartsWith("Feeble "))
				CantripLevel = CantripLevels.Feeble;
			else if (name.StartsWith("Minor "))
				CantripLevel = CantripLevels.Minor;
			else if (name.StartsWith("Lesser "))
				CantripLevel = CantripLevels.Lesser;
			else if (name.StartsWith("Moderate "))
				CantripLevel = CantripLevels.Moderate;
			else if (name.StartsWith("Inner "))
				CantripLevel = CantripLevels.Inner;
			else if (name.StartsWith("Major "))
				CantripLevel = CantripLevels.Major;
			else if (name.StartsWith("Epic "))
				CantripLevel = CantripLevels.Epic;
			else if (name.StartsWith("Legendary "))
				CantripLevel = CantripLevels.Legendary;

			// Get the Family
			int nameIndex = SpellTableHeader.IndexOf("Name");
			int familyIndex = SpellTableHeader.IndexOf("Family");

			foreach (string[] line in SpellTable)
			{
				if (line[nameIndex] == Name)
				{
					Family = int.Parse(line[familyIndex]);
					break;
				}
			}

			// Try to determine if this is a lvl 7
			if (BuffLevel == BuffLevels.None && CantripLevel == CantripLevels.None)
			{
				int difficultyIndex = SpellTableHeader.IndexOf("Difficulty");
				int durationIndex = SpellTableHeader.IndexOf("Duration");

				foreach (string[] line in SpellTable)
				{
					if (line[nameIndex] == Name)
					{
						int difficulty = int.Parse(line[difficultyIndex]);
						int duration = int.Parse(line[durationIndex]);

						if (difficulty == 300 && duration == 3600)
							BuffLevel = BuffLevels.VII;

						break;
					}
				}
			}
		}

		public readonly string Name;

		public readonly int Family;

		public enum BuffLevels
		{
			None,

			I,
			II,
			III,
			IV,
			V,
			VI,
			VII,
			VIII,
		}

		public readonly BuffLevels BuffLevel;

		public enum CantripLevels
		{
			None,

			Feeble,
			Minor,
			Lesser,
			Moderate,
			Inner,
			Major,
			Epic,
			Legendary,
		}

		public readonly CantripLevels CantripLevel;

		public bool IsOfSameFamilyAndGroup(Spell compareSpell)
		{
			if (Family != compareSpell.Family)
				return false;

			if (BuffLevel != 0 && compareSpell.BuffLevel != 0)
				return true;

			if (CantripLevel != 0 && compareSpell.CantripLevel != 0)
				return true;

			// Are both spells are of an unkown group?
			if (BuffLevel == 0 && compareSpell.BuffLevel == 0 && CantripLevel == 0 && compareSpell.CantripLevel == 0)
				return true;

			return false;
		}

		public bool IsSameOrSurpasses(Spell compareSpell)
		{
			return (this == compareSpell || Surpasses(compareSpell));
		}

		public bool Surpasses(Spell compareSpell)
		{
			if (Family == 0 || Family != compareSpell.Family)
				return false;

			if (BuffLevel > 0 && compareSpell.BuffLevel > 0 && BuffLevel > compareSpell.BuffLevel)
				return true;

			if (CantripLevel > 0 && compareSpell.CantripLevel > 0 && CantripLevel > compareSpell.CantripLevel)
				return true;

			return false;
		}

		public override string ToString()
		{
			return Name;
		}

		private static readonly Dictionary<string, Spell> Spells = new Dictionary<string, Spell>();

		public static Spell GetSpell(string text)
		{
			if (!Spells.ContainsKey(text))
				Spells.Add(text, new Spell(text));

			return Spells[text];
		}

		public static Spell GetSpell(int id)
		{
			int idIndex = SpellTableHeader.IndexOf("Id");
			int nameIndex = SpellTableHeader.IndexOf("Name");

			foreach (string[] line in SpellTable)
			{
				if (line[idIndex] == id.ToString())
					return GetSpell(line[nameIndex]);
			}

			throw new ArgumentException("Spell of id: " + id + " not found in Spells.csv");
		}

		public static bool IsAKnownSpell(int id)
		{
			int idIndex = SpellTableHeader.IndexOf("Id");

			foreach (string[] line in SpellTable)
			{
				if (line[idIndex] == id.ToString())
					return true;
			}

			return false;
		}

		public static bool IsAKnownSpell(string text)
		{
			int nameIndex = SpellTableHeader.IndexOf("Name");

			foreach (string[] line in SpellTable)
			{
				if (line[nameIndex] == text)
					return true;
			}

			return false;
		}
	}
}
