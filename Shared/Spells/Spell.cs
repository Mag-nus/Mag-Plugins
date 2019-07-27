
namespace Mag.Shared.Spells
{
	/// <summary>
	/// Use GetSpell() to intialize a Spell object
	/// </summary>
	public class Spell
	{
		public readonly int Id;
		public readonly string Name;
		public readonly int Difficulty;
		public readonly int Duration;
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

		public Spell(int id, string name, int difficulty, int duration, int family)
		{
			Id = id;
			Name = name;
			Difficulty = difficulty;
			Duration = duration;
			Family = family;

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
			else if (name.StartsWith("Incantation ") || name.StartsWith("Aura of Incantation "))
				BuffLevel = BuffLevels.VIII;

			if (name.StartsWith("Feeble "))
				CantripLevel = CantripLevels.Feeble;
			else if (name.StartsWith("Minor "))
				CantripLevel = CantripLevels.Minor;
			else if (name.StartsWith("Lesser "))
				CantripLevel = CantripLevels.Lesser;
			else if (name.StartsWith("Moderate "))
				CantripLevel = CantripLevels.Moderate;
			else if (name.StartsWith("Inner ") && name != "Inner Calm")
				CantripLevel = CantripLevels.Inner;
			else if (name.StartsWith("Major "))
				CantripLevel = CantripLevels.Major;
			else if (name.StartsWith("Epic "))
				CantripLevel = CantripLevels.Epic;
			else if (name.StartsWith("Legendary "))
				CantripLevel = CantripLevels.Legendary;

			// Try to determine if this is a lvl x
			if (BuffLevel == BuffLevels.None && CantripLevel == CantripLevels.None)
			{
                // These spells don't have levels
                if (name.StartsWith("Prodigal ") || // Rares
                    name.StartsWith("Cloaked in ") || name.StartsWith("Shroud of ")) // Cloaks
                    return;

                if (difficulty == 1 && duration == 1800)
                    BuffLevel = BuffLevels.I;
                if (difficulty == 50 && duration == -1 && name.StartsWith("Evaporate "))
                    BuffLevel = BuffLevels.II;

                if (difficulty == 50 && duration == 1800)
                    BuffLevel = BuffLevels.II;
                if (difficulty == 100 && duration == -1 && name.StartsWith("Extinguish "))
                    BuffLevel = BuffLevels.II;

                if (difficulty == 100 && duration == 1800)
                    BuffLevel = BuffLevels.III;
                if (difficulty == 150 && duration == -1 && name.StartsWith("Cleanse "))
                    BuffLevel = BuffLevels.III;

                if (difficulty == 150 && duration == 1800)
                    BuffLevel = BuffLevels.IV;
                if (difficulty == 200 && duration == -1 && name.StartsWith("Devour "))
                    BuffLevel = BuffLevels.IV;

                if (difficulty == 200 && duration == 1800)
                    BuffLevel = BuffLevels.V;
                if (difficulty == 250 && duration == -1 && name.StartsWith("Purge "))
                    BuffLevel = BuffLevels.V;

                if (difficulty == 200 && duration == -1 && !name.StartsWith("Devour ")) // Ring spells
                    BuffLevel = BuffLevels.VI;
                if (difficulty == 250 && duration == 2700)
                    BuffLevel = BuffLevels.VI;
                if (difficulty == 300 && duration == -1 && name.StartsWith("Nullify "))
                    BuffLevel = BuffLevels.VI;

                if (difficulty == 300 && duration == -1 && !name.StartsWith("Nullify "))
                    BuffLevel = BuffLevels.VII;
                if (difficulty == 300 && duration == 240)
                    BuffLevel = BuffLevels.VII;
                if (difficulty == 300 && duration == 3600)
                    BuffLevel = BuffLevels.VII;
                if (difficulty == 325 && duration == -1)
                    BuffLevel = BuffLevels.VII;
                if (difficulty == 325 && duration == 240)
                    BuffLevel = BuffLevels.VII;
                if (difficulty == 350 && duration == -1)
                    BuffLevel = BuffLevels.VII;

                if (difficulty == 400 && duration == 5400)
                    BuffLevel = BuffLevels.VIII;
            }
		}


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
	}
}
