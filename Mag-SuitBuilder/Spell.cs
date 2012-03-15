using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	class Spell
	{
		public readonly string Name;

		enum SpellLevel
		{
			Unknown = 0,
			Minor = 1,
			Major = 2,
			Epic = 3,
		}

		readonly SpellLevel level;

		// This is a little hack for fast comparison of spell name without doing a string compare
		private static Collection<string> NameWithoutLevels = new Collection<string>();
		private int nameWithoutLevelIndex = -1;

		public Spell(string name)
		{
			Name = name;

			if (Name.Contains("Minor "))
				level = SpellLevel.Minor;
			else if (Name.Contains("Major "))
				level = SpellLevel.Major;
			else if (Name.Contains("Epic "))
				level = SpellLevel.Epic;
			else
				level = SpellLevel.Unknown;

			string nameWithoutLevel ;

			if (level == SpellLevel.Unknown)
				nameWithoutLevel = Name;
			else
				nameWithoutLevel = Name.Substring(Name.IndexOf(' ') + 1, Name.Length - Name.IndexOf(' ') - 1);

			if (!NameWithoutLevels.Contains(nameWithoutLevel))
				NameWithoutLevels.Add(nameWithoutLevel);

			nameWithoutLevelIndex = NameWithoutLevels.IndexOf(nameWithoutLevel);
		}

		public bool IsSame(Spell compareSpell)
		{
			return level == compareSpell.level && nameWithoutLevelIndex == compareSpell.nameWithoutLevelIndex;
		}

		public bool Surpasses(Spell compareSpell)
		{
			if (level == SpellLevel.Unknown || compareSpell.level == SpellLevel.Unknown)
				return false;

			return level > compareSpell.level && nameWithoutLevelIndex == compareSpell.nameWithoutLevelIndex;
		}

		public bool IsSameOrSurpasses(Spell compareSpell)
		{
			if (level == SpellLevel.Unknown || compareSpell.level == SpellLevel.Unknown)
				return false;

			return level >= compareSpell.level && nameWithoutLevelIndex == compareSpell.nameWithoutLevelIndex;
		}

		public bool IsMinor { get { return level == SpellLevel.Minor; } }
		public bool IsMajor { get { return level == SpellLevel.Major; } }
		public bool IsEpic { get { return level == SpellLevel.Epic; } }

		public override string ToString()
		{
			return Name;
		}
	}
}
