
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

		public Spell(string name)
		{
			Name = name;
		}

		public SpellLevel Level
		{
			get
			{
				if (Name.Contains("Minor ")) return SpellLevel.Minor;
				if (Name.Contains("Major ")) return SpellLevel.Major;
				if (Name.Contains("Epic ")) return SpellLevel.Epic;

				return SpellLevel.Unknown;
			}
		}

		public string NameWithoutLevel
		{
			get
			{
				if (Level == SpellLevel.Unknown)
					return Name;

				return Name.Substring(Name.IndexOf(' ') + 1, Name.Length - Name.IndexOf(' ') - 1);
			}
		}

		public override string ToString()
		{
			return Name;
		}
	}
}
