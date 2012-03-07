using System;
using System.Collections.Generic;
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

				//if (!piece.IsArmor)
				//	continue;

				equipmentPieces.Add(piece);
			}

			progressBar1.Value = 0;

			Collection<EquipmentGroup> equipmentGroups = new Collection<EquipmentGroup>();

			if (equipmentPieces.Count > 0)
			{
				//for (int i = 0 ; i < equipmentPieces.Count ; i++)
				//{
					EquipmentGroup equipmentGroup = new EquipmentGroup();
					equipmentGroups.Add(equipmentGroup);

					ProcessEquipmentPieces(equipmentGroups, equipmentPieces, 0, equipmentGroup);

					//progressBar1.Value = (int)(((i + 1) / (float)equipmentPieces.Count) * 100.0);
					//progressBar1.Refresh();
				//}
			}

			// Find the highest AL group
			int highest = 0;
			foreach (EquipmentGroup equipmentGroup in equipmentGroups)
			{
				if (equipmentGroup.TotalPotentialTinkedArmorLevel > highest)
					highest = equipmentGroup.TotalPotentialTinkedArmorLevel;
			}

			// Add the top 1,000 groups
			Collection<EquipmentGroup> topEquipmentGroups = new Collection<EquipmentGroup>();

			while (topEquipmentGroups.Count < 10000)
			{
				if (topEquipmentGroups.Count >= equipmentGroups.Count)
					break;

				if (highest == 0)
					break;

				for (int i = 0 ; i < equipmentGroups.Count ; i++)
				{
					if (equipmentGroups[i].TotalPotentialTinkedArmorLevel == highest)
					{
						//if (equipmentGroups[i].ArmorSetPieces.ContainsKey("Tinker's Set") && equipmentGroups[i].ArmorSetPieces["Tinker's Set"] == 5)
						topEquipmentGroups.Add(equipmentGroups[i]);

						if (topEquipmentGroups.Count == 10000)
							break;
					}
				}

				highest--;
			}

			listBox1.Items.Clear();

			foreach (EquipmentGroup equipmentGroup in topEquipmentGroups)
			{
				listBox1.Items.Add(equipmentGroup);
			}
			//MessageBox.Show(equipmentGroups.Count.ToString() + " " + topEquipmentGroups.Count);
			//equipmentGroups.Sort(delegate(EquipmentGroup a, EquipmentGroup b) { return a.TotalPotentialTinkedArmorLevel.CompareTo(b.TotalPotentialTinkedArmorLevel); });

			//for (int i = 0 ; i < equipmentGroups.Count ; i++)
			//	MessageBox.Show(equipmentGroups[i].ToString(), i + " of " + equipmentGroups.Count);
		}

		private void ProcessEquipmentPieces(Collection<EquipmentGroup> equipmentGroups, Collection<EquipmentPiece> equipmentPieces, int startIndex, EquipmentGroup workingGroup)
		{
			for (int i = startIndex ; i < equipmentPieces.Count ; i++)
			{
				if (!workingGroup.CanAdd(equipmentPieces[i]))
					continue;

				if (!workingGroup.CanOfferBeneficialSpell(equipmentPieces[i]))
					continue;

				// If we're adding a multi-slot piece, we need to create a group branch for each possible slot of that piece
				int setBits = 0;
				int value = (int)equipmentPieces[i].EquipableSlots;
				while (value != 0)
				{
					if ((value & 1) == 1)
						setBits++;
					value >>= 1;
				}

				if (setBits > 1)
				{
					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.Chest) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.Chest);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.Abdomen) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.Abdomen);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.UpperArms) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.UpperArms);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LowerArms) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LowerArms);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.UpperLegs) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.UpperLegs);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LowerLegs) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LowerLegs);
						if (!newGroup.IsFull) 
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LeftBracelet) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LeftBracelet);
						if (!newGroup.IsFull)
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.RightBracelet) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.RightBracelet);
						if (!newGroup.IsFull)
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LeftRing) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LeftRing);
						if (!newGroup.IsFull)
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.RightRing) != 0)
					{
						EquipmentGroup newGroup = workingGroup.Clone();
						equipmentGroups.Add(newGroup);

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.RightRing);
						if (!newGroup.IsFull)
							ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
					}
				}
				else
				{
					EquipmentGroup newGroup = workingGroup.Clone();
					equipmentGroups.Add(newGroup);

					newGroup.Add(equipmentPieces[i]);

					if (!newGroup.IsFull)
						ProcessEquipmentPieces(equipmentGroups, equipmentPieces, i + 1, newGroup);
				}
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			EquipmentGroup equipmentGroup = ((ListBox)sender).SelectedItem as EquipmentGroup;

			PopuldateFromEquipmentGroup(equipmentGroup);
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
