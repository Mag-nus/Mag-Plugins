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
				/*
				// Duplicate multi-slot pieces into single slot copies
				if (piece.EquipableSlots == (Constants.EquippableSlotFlags.LeftBracelet | Constants.EquippableSlotFlags.RightBracelet))
				{
					piece.EquipableSlots = Constants.EquippableSlotFlags.LeftBracelet;

					piece = new EquipmentPiece(line);
					piece.EquipableSlots = Constants.EquippableSlotFlags.RightBracelet;
					equipmentPieces.Add(piece);
				}

				if (piece.EquipableSlots == (Constants.EquippableSlotFlags.LeftRing | Constants.EquippableSlotFlags.RightRing))
				{
					piece.EquipableSlots = Constants.EquippableSlotFlags.LeftRing;

					piece = new EquipmentPiece(line);
					piece.EquipableSlots = Constants.EquippableSlotFlags.RightRing;
					equipmentPieces.Add(piece);
				}
				*/
			}

			progressBar1.Value = 0;

			Collection<EquipmentGroup> equipmentGroups = new Collection<EquipmentGroup>();

			for (int i = 0 ; i < equipmentPieces.Count ; i++)
			{
				EquipmentGroup equipmentGroup = new EquipmentGroup();

				equipmentGroups.Add(equipmentGroup);

				ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i, equipmentGroup);

				progressBar1.Value = (int)(((i + 1) / (float)equipmentPieces.Count) * 100.0);
				progressBar1.Refresh();
			}

			MessageBox.Show(equipmentGroups.Count.ToString());

			/*
			EquipmentGroup equipmentGroup = new EquipmentGroup();

			foreach (EquipmentPiece equipmentPiece in equipmentPieces)
			{
				equipmentGroup.Add(equipmentPiece);
			}

			PopuldateFromEquipmentGroup(equipmentGroup);
			*/
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

				if ((equipmentGroups.Count % 100000) == 0)
				{
					lblPossibleCombinations.Text = "Possible Combinations: " + equipmentGroups.Count;
					lblPossibleCombinations.Refresh();
				}

				workingGroup.Add(equipmentPieces[i]);

				if (!workingGroup.IsFull)
					ProcessEquipmentPieces(equipmentGroups, equipmentPieces, startIndex + 1, workingGroup);

				if (originalGroup.Count > 0)
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
