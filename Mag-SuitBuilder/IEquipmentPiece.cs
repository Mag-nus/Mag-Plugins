using System.Collections.ObjectModel;

namespace Mag_SuitBuilder
{
	interface IEquipmentPiece
	{
		string Name { get; }

		Constants.EquippableSlotFlags EquipableSlots { get; }

		int NumberOfSlotsCovered { get; }

		string ArmorSet { get; }

		int ArmorLevel { get; }

		ReadOnlyCollection<Spell> Spells { get; }
	}
}
