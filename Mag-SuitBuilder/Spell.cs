using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	public enum SpellLevel
	{
		Unknown		= 0,
		Minor		= 1,
		Major		= 2,
		Epic		= 3,
	}

	class Spell
	{
		public readonly string Name;

		public SpellLevel Level;

		// This is a little hack for fast comparison of spell name without doing a string compare
		private static Collection<string> NameWithoutLevels = new Collection<string>();
		private int nameWithoutLevelIndex = -1;

		public Spell(string name)
		{
			Name = name;

			if (Name.Contains("Minor "))
				Level = SpellLevel.Minor;
			else if (Name.Contains("Major "))
				Level = SpellLevel.Major;
			else if (Name.Contains("Epic "))
				Level = SpellLevel.Epic;
			else
				Level = SpellLevel.Unknown;

			string nameWithoutLevel ;

			if (Level == SpellLevel.Unknown)
				nameWithoutLevel = Name;
			else
				nameWithoutLevel = Name.Substring(Name.IndexOf(' ') + 1, Name.Length - Name.IndexOf(' ') - 1);

			if (!NameWithoutLevels.Contains(nameWithoutLevel))
				NameWithoutLevels.Add(nameWithoutLevel);

			nameWithoutLevelIndex = NameWithoutLevels.IndexOf(nameWithoutLevel);
		}

		public bool IsOfSameFamily(Spell compareSpell)
		{
			return nameWithoutLevelIndex == compareSpell.nameWithoutLevelIndex;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
