using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			Text = "Mag-SuitBuilder " + Application.ProductVersion;
		}

		private void calculatePossibilities_Click(object sender, System.EventArgs e)
		{
			if (txtEquipmentEntries.TextLength == 0)
			{
				MessageBox.Show("Enter some equipment in the equipment tab first.");

				return;
			}

			Collection<EquipmentPiece> equipmentPieces = new Collection<EquipmentPiece>();

			// Parse the equipment pieces from the text input
			foreach (string line in txtEquipmentEntries.Lines)
			{
				EquipmentPiece piece = new EquipmentPiece(line);

				if (piece.EquipableSlots == Constants.EquippableSlotFlags.None)
					continue;

				if (!piece.IsArmor)
					continue;

				equipmentPieces.Add(piece);
			}

			progressBar1.Value = 0;

			Collection<EquipmentGroup> equipmentGroups = new Collection<EquipmentGroup>();

			if (equipmentPieces.Count > 0)
			{
				for (int i = 0 ; i < equipmentPieces.Count - 1 ; i++)
				{
					EquipmentGroup equipmentGroup = new EquipmentGroup();
					equipmentGroup.Add(equipmentPieces[i]);
					equipmentGroups.Add(equipmentGroup);

					ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, equipmentGroup);

					progressBar1.Value = (int)(((i + 2) / (float)equipmentPieces.Count) * 100.0);
					progressBar1.Refresh();
				}
			}

			for (int i = 0 ; i < equipmentGroups.Count ; i++)
				MessageBox.Show(equipmentGroups[i].ToString(), i + " of " + equipmentGroups.Count);
		}

		private void ProcessEquipmentPieces(Collection<EquipmentGroup> equipmentGroups, Collection<EquipmentPiece> equipmentPieces, int startIndex, EquipmentGroup workingGroup)
		{
			for (int i = startIndex ; i < equipmentPieces.Count ; i++)
			{
				if (!workingGroup.CanAdd(equipmentPieces[i]))
					continue;

				if (!workingGroup.CanOfferBeneficialSpell(equipmentPieces[i]))
					continue;

				EquipmentGroup originalGroup = workingGroup.Clone();
				equipmentGroups.Add(originalGroup);

				workingGroup.Add(equipmentPieces[i]);

				if (!workingGroup.IsFull)
					ProcessEquipmentPieces(equipmentGroups, equipmentPieces, startIndex + 1, workingGroup);

				ProcessEquipmentPieces(equipmentGroups, equipmentPieces, startIndex + 1, originalGroup);
			}
		}

		private void PopuldateFromEquipmentGroup(EquipmentGroup equipmentGroup)
		{
			foreach (Control cntrl in tabPage1.Controls)
			{
				if (cntrl is EquipmentPieceControl)
				{
					EquipmentPieceControl coveragePiece = (cntrl as EquipmentPieceControl);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Necklace) coveragePiece.SetEquipmentPiece(equipmentGroup.Necklace);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Trinket) coveragePiece.SetEquipmentPiece(equipmentGroup.Trinket);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.LeftBracelet) coveragePiece.SetEquipmentPiece(equipmentGroup.LeftBracelet);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.RightBracelet) coveragePiece.SetEquipmentPiece(equipmentGroup.RightBracelet);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.LeftRing) coveragePiece.SetEquipmentPiece(equipmentGroup.LeftRing);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.RightRing) coveragePiece.SetEquipmentPiece(equipmentGroup.RightRing);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Head) coveragePiece.SetEquipmentPiece(equipmentGroup.Head);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Chest) coveragePiece.SetEquipmentPiece(equipmentGroup.Chest);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.UpperArms) coveragePiece.SetEquipmentPiece(equipmentGroup.UpperArms);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.LowerArms) coveragePiece.SetEquipmentPiece(equipmentGroup.LowerArms);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Hands) coveragePiece.SetEquipmentPiece(equipmentGroup.Hands);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Abdomen) coveragePiece.SetEquipmentPiece(equipmentGroup.Abdomen);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.UpperLegs) coveragePiece.SetEquipmentPiece(equipmentGroup.UpperLegs);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.LowerLegs) coveragePiece.SetEquipmentPiece(equipmentGroup.LowerLegs);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Feet) coveragePiece.SetEquipmentPiece(equipmentGroup.Feet);

					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Shirt) coveragePiece.SetEquipmentPiece(equipmentGroup.Shirt);
					if (coveragePiece.EquipableSlot == Constants.EquippableSlotFlags.Pants) coveragePiece.SetEquipmentPiece(equipmentGroup.Pants);
				}
			}
		}
	}
}
