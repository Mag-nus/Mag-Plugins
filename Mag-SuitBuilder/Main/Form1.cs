﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Mag_SuitBuilder
{
	public partial class Form1 : Form
	{
		[DllImport("user32.dll")]
		static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

		public Form1()
		{
			InitializeComponent();

			Text = "Mag-SuitBuilder " + Application.ProductVersion;

			dataGridView1.Rows.Add(7);

			dataGridView1[0, 0].Value = "Strength";
			dataGridView1[0, 1].Value = "Endurance";
			dataGridView1[0, 2].Value = "Coordination";
			dataGridView1[0, 3].Value = "Quickness";
			dataGridView1[0, 4].Value = "Focus";
			dataGridView1[0, 5].Value = "Willpower";
			dataGridView1[0, 6].Selected = true;

			dataGridView1[1, 0].Value = "Slashing Ward";
			dataGridView1[1, 1].Value = "Piercing Ward";
			dataGridView1[1, 2].Value = "Bludgeoning Ward";
			dataGridView1[1, 3].Value = "Flame Ward";
			dataGridView1[1, 4].Value = "Frost Ward";
			dataGridView1[1, 5].Value = "Acid Ward";
			dataGridView1[1, 6].Value = "Storm Ward";

			dataGridView1[2, 0].Value = "Life Magic";
			dataGridView1[2, 1].Value = "Creature Ench";
			dataGridView1[2, 2].Value = "Item Ench";
			dataGridView1[2, 3].Value = "War Magic";
			dataGridView1[2, 4].Value = "Void Magic";
			dataGridView1[2, 5].Value = "Mana C";
			dataGridView1[2, 6].Value = "Arcane";

			dataGridView1[3, 0].Value = "Missile";
			dataGridView1[3, 1].Value = "Heavy";
			dataGridView1[3, 2].Value = "Light";
			dataGridView1[3, 3].Value = "Finesse";
			dataGridView1[3, 4].Value = "Healing";
			dataGridView1[3, 5].Value = "Shield";

			dataGridView1[4, 0].Value = "Two Hand";
			dataGridView1[4, 1].Value = "Dual Wield";
			dataGridView1[4, 2].Value = "Dirty Fighting";
			dataGridView1[4, 3].Value = "Recklessness";
			dataGridView1[4, 4].Value = "Sneak Attack";
			//
			//

			dataGridView1[5, 0].Value = "Invulnerability";
			dataGridView1[5, 1].Value = "Magic Resistance";
			dataGridView1[5, 2].Value = "Impregnability";
			dataGridView1[5, 3].Value = "Armor";
			dataGridView1[5, 4].Value = "Deception";
			dataGridView1[5, 5].Value = "Person";
			dataGridView1[5, 6].Value = "Monster";

			dataGridView1[6, 0].Value = "Item Tinker";
			dataGridView1[6, 1].Value = "Armor Tinker";
			dataGridView1[6, 2].Value = "Weapon Tinker";
			dataGridView1[6, 3].Value = "Magic Item";
			dataGridView1[6, 4].Value = "Cooking";
			dataGridView1[6, 5].Value = "Alchemy";
			dataGridView1[6, 6].Value = "Fletching";
		}

		private List<IEquipmentPiece> equipmentPieces;

		private List<EquipmentGroup> equipmentGroups;

		private void calculatePossibilities_Click(object sender, System.EventArgs e)
		{
			if (txtEquipmentEntries.TextLength == 0)
			{
				MessageBox.Show("Enter some equipment in the equipment tab first.");

				return;
			}

			tabControl1.Enabled = false;
			progressBar1.Value = 1;

			equipmentPieces = new List<IEquipmentPiece>();

			// Parse the equipment pieces from the text input
			foreach (string line in txtEquipmentEntries.Lines)
			{
				EquipmentPiece piece = new EquipmentPiece(line);

				if (piece.EquipableSlots == Constants.EquippableSlotFlags.None)
					continue;

				equipmentPieces.Add(piece);
			}

			// We should go through our equipment pieces here and find pieces with the same coverage/set/spells and only keep the highest AL piece.
			for (int i = 0 ; i < equipmentPieces.Count ; i++)
			{
				for (int j = i + 1 ; j < equipmentPieces.Count ; j++)
				{
					if (equipmentPieces[i].EquipableSlots == equipmentPieces[j].EquipableSlots && equipmentPieces[i].ArmorSet == equipmentPieces[j].ArmorSet && equipmentPieces[i].Spells.Count == equipmentPieces[j].Spells.Count)
					{
						// This is a little hacky but it works
						EquipmentGroup iGroup = new EquipmentGroup();
						iGroup.Add(equipmentPieces[i]);

						EquipmentGroup jGroup = new EquipmentGroup();
						jGroup.Add(equipmentPieces[j]);

						if (!iGroup.CanOfferBeneficialSpell(equipmentPieces[j], chkIgnoreMajors.Checked, chkIgnoreMinors.Checked) && !jGroup.CanOfferBeneficialSpell(equipmentPieces[i], chkIgnoreMajors.Checked, chkIgnoreMinors.Checked))
						{
							// Drop the one with the lowest armor level
							if (equipmentPieces[i].ArmorLevel < equipmentPieces[j].ArmorLevel)
								equipmentPieces.RemoveAt(i);
							else
								equipmentPieces.RemoveAt(j);
							i--;
							break;
						}
					}
				}
			}

			equipmentGroups = new List<EquipmentGroup>();

			DateTime startTime = DateTime.Now;

			if (equipmentPieces.Count > 0)
			{
				EquipmentGroup equipmentGroup = new EquipmentGroup();

				// Add any locked pieces of equipment from the form into our base equipment group.
				foreach (Control control in tabPage1.Controls)
				{
					if (control is EquipmentPieceControl)
					{
						EquipmentPieceControl equipmentPieceControl = control as EquipmentPieceControl;

						if (equipmentPieceControl.IsLocked)
							equipmentGroup.Add(control as IEquipmentPiece);
					}
				}

				// Now that we've loaded our locked pieces, remove pieces we can't add from our equipment pieces list
				for (int i = 0 ; i < equipmentPieces.Count ; i++)
				{
					if (!equipmentGroup.CanAdd(equipmentPieces[i]) || !equipmentGroup.CanOfferBeneficialSpell(equipmentPieces[i], chkIgnoreMajors.Checked, chkIgnoreMinors.Checked))
						equipmentPieces.RemoveAt(i);
				}

				int endIndex = equipmentPieces.Count - 1;

				// Sort the list putting all armor first
				equipmentPieces.Sort((a, b) =>
				{
					if (a.ArmorLevel > 0 && b.ArmorLevel == 0)
						return -1;

					if (a.ArmorLevel == 0 && b.ArmorLevel > 0)
						return 1;

					return 0;
				});

				// Set our end index to the last piece of armor we have.
				// We build our suits based around armor combos first, and then compliment them with jewelry/clothing
				// We only do this if we have less than the max amount of wearable pieces
				if (equipmentPieces.Count > EquipmentGroup.MaximumPieces)
				{
					for (int i = 0 ; i < equipmentPieces.Count ; i++)
					{
						if (equipmentPieces[i].ArmorLevel == 0)
						{
							endIndex = i - 1;
							break;
						}
					}
				}

				// Are we are building a suit based around a primary set?
				if (!String.IsNullOrEmpty(txtPrimaryArmorSet.Text))
				{
					// If we are building a suit based around a primary set, put all the set pieces first
					equipmentPieces.Sort((a, b) =>
					{
						if (a.ArmorSet == txtPrimaryArmorSet.Text && b.ArmorSet != txtPrimaryArmorSet.Text)
							return -1;

						if (a.ArmorSet != txtPrimaryArmorSet.Text && b.ArmorSet == txtPrimaryArmorSet.Text)
							return 1;

						return 0;
					});

					for (int i = 0 ; i < equipmentPieces.Count ; i++)
					{
						if (equipmentPieces[i].ArmorSet != txtPrimaryArmorSet.Text)
						{
							endIndex = i - 1;
							break;
						}
					}

					// Need at least 5 pieces to build a suit based around a primary set
					endIndex -= 4; // (Inclusive)
				}

				if (Environment.ProcessorCount <= 1)
					ProcessEquipmentPieces(0, endIndex, equipmentGroup);
				else
				{
					ProcessEquipmentPieces(0, endIndex, equipmentGroup);

					/* this is shit
					ProcessEquipmentPieces(0, 0, equipmentGroup.Clone());

					Thread[] threads = new Thread[endIndex];

					for (int i = 1 ; i <= endIndex ; i++)
					{
						int i1 = i;
						threads[i - 1] = new Thread(delegate() { ProcessEquipmentPieces(i1, i1, equipmentGroup.Clone()); });
						threads[i - 1].Start();
					}

					for (int i = 1 ; i <= endIndex ; i++)
					{
						//while (threads[i - 1].IsAlive)
						//	Thread.Sleep(1);
					}
					*/
				}
			}

			//MessageBox.Show(equipmentGroups.Count + " " + (DateTime.Now - startTime));

			// Sort the equipment groups based on armor level
			equipmentGroups.Sort((a, b) => b.TotalPotentialTinkedArmorLevel.CompareTo(a.TotalPotentialTinkedArmorLevel));

			listBox1.BeginUpdate();

			listBox1.Items.Clear();

			foreach (EquipmentGroup equipmentGroup in equipmentGroups)
			{
				listBox1.Items.Add(equipmentGroup);

				// Don't add too many items to our listbox
				if (listBox1.Items.Count >= 1000)
					break;
			}

			listBox1.EndUpdate();

			#region ' old code '
			/*
			List<EquipmentGroup> newGroups = new List<EquipmentGroup>(equipmentGroups.Count);

			// Go through and remove groups that have the same equipment
			for (int i = 0 ; i < equipmentGroups.Count ; i++)
			{
				for (int j = i + 1 ; j <= equipmentGroups.Count ; j++)
				{
					if (j == equipmentGroups.Count)
					{
						newGroups.Add(equipmentGroups[i]);
						break;
					}

					if (equipmentGroups[i].EquipmentPieceHash == equipmentGroups[j].EquipmentPieceHash)
					{
						//equipmentGroups.RemoveAt(j);
						break;
					}
				}
			}
			equipmentGroups = newGroups;
			MessageBox.Show(equipmentGroups.Count + " " + (DateTime.Now - startTime) + " " + Environment.ProcessorCount);

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

			MessageBox.Show(equipmentGroups.Count.ToString() + " " + topEquipmentGroups.Count);
			equipmentGroups.Sort(delegate(EquipmentGroup a, EquipmentGroup b) { return a.TotalPotentialTinkedArmorLevel.CompareTo(b.TotalPotentialTinkedArmorLevel); });

			for (int i = 0 ; i < equipmentGroups.Count ; i++)
				MessageBox.Show(equipmentGroups[i].ToString(), i + " of " + equipmentGroups.Count);
			*/
			#endregion

			progressBar1.Value = 100;
			tabControl1.Enabled = true;

			FlashWindow(this.Handle, true);
		}

		private void ProcessEquipmentPieces(int startIndex, int endIndex, EquipmentGroup workingGroup)
		{
			for (int i = startIndex ; i <= endIndex + 1 ; i++)
			{
				if (i == endIndex + 1 || workingGroup.EquipmentPieceCount >= EquipmentGroup.MaximumPieces || i == equipmentPieces.Count)
				{
					workingGroup.CalculateEquipmentPieceHash(equipmentPieces);

					// Only add this group if its equipment isn't a subset of an already loaded equipment group.
					for (int j = 0 ; j < equipmentGroups.Count ; j++)
					{
						if (equipmentGroups[j].EquipmentPieceCount < workingGroup.EquipmentPieceCount)
						{
							/*
							// This is only effective if we're multithreading the process loop
							if (equipmentGroups[j].IsEquipmentSubsetOfGroup(workingGroup))
							{
								equipmentGroups[j] = workingGroup;
								return;
							}
							*/
							continue;
						}

						// Do both groups have the same items?
						if (equipmentGroups[j].EquipmentPieceCount == workingGroup.EquipmentPieceCount)
						{
							if (equipmentGroups[j].EquipmentPieceHash == workingGroup.EquipmentPieceHash)
								return;

							continue;
						}

						if (workingGroup.IsEquipmentSubsetOfGroup(equipmentGroups[j]))
							return;
					}

					// If we want full armor suits, make sure we have all the body armor slots covered.
					if (chkOnlyBuildFullSuits.Checked)
					{
						if ((workingGroup.equippedSlots & Constants.EquippableSlotFlags.AllBodyArmor) != Constants.EquippableSlotFlags.AllBodyArmor &&
							(workingGroup.equippedSlots & Constants.EquippableSlotFlags.AllBodyArmor) != 0)
							return;
					}

					equipmentGroups.Add(workingGroup);

					return;
				}

				// If we're building a suit around an armor set, and we don't have 5 pieces of that armor set..
				// and we've already gotten to other pieces in our equipment list, don't bother continuing..
				if (!workingGroup.HasFullPrimaryAmrorSet && !String.IsNullOrEmpty(txtPrimaryArmorSet.Text))
				{
					if (txtPrimaryArmorSet.Text != equipmentPieces[i].ArmorSet)
						return;
				}

				if (!workingGroup.CanOfferBeneficialSpell(equipmentPieces[i], chkIgnoreMajors.Checked, chkIgnoreMinors.Checked))
					continue;

				// If we're adding a multi-slot piece, we need to create a group branch for each possible slot of that piece
				if (equipmentPieces[i].NumberOfSlotsCovered > 1)
				{
					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.Chest) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.Chest))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.Chest);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.Abdomen) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.Abdomen))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.Abdomen);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.UpperArms) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.UpperArms))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.UpperArms);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LowerArms) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.LowerArms))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LowerArms);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);

						continue;
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.UpperLegs) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.UpperLegs))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.UpperLegs);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LowerLegs) != 0 && workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.LowerLegs))
					{
						EquipmentGroup newGroup = workingGroup.Clone();

						newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LowerLegs);
						ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);

						continue;
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LeftBracelet) != 0 || (equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.RightBracelet) != 0)
					{
						if (workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.LeftBracelet))
						{
							EquipmentGroup newGroup = workingGroup.Clone();

							newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LeftBracelet);
							ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
						}
						else if (workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.RightBracelet))
						{
							EquipmentGroup newGroup = workingGroup.Clone();

							newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.RightBracelet);
							ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
						}

						continue;
					}

					if ((equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.LeftRing) != 0 || (equipmentPieces[i].EquipableSlots & Constants.EquippableSlotFlags.RightRing) != 0)
					{
						if (workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.LeftRing))
						{
							EquipmentGroup newGroup = workingGroup.Clone();

							newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.LeftRing);
							ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
						}
						else if (workingGroup.CanAdd(equipmentPieces[i], Constants.EquippableSlotFlags.RightRing))
						{
							EquipmentGroup newGroup = workingGroup.Clone();

							newGroup.Add(equipmentPieces[i], Constants.EquippableSlotFlags.RightRing);
							ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
						}

						continue;
					}
				}
				else if (workingGroup.CanAdd(equipmentPieces[i]))
				{
					EquipmentGroup newGroup = workingGroup.Clone();

					newGroup.Add(equipmentPieces[i]);
					ProcessEquipmentPieces(i + 1, equipmentPieces.Count - 1, newGroup);
				}
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			EquipmentGroup equipmentGroup = ((ListBox)sender).SelectedItem as EquipmentGroup;

			PopulateFromEquipmentGroup(equipmentGroup);
		}

		private void PopulateFromEquipmentGroup(EquipmentGroup equipmentGroup)
		{
			if (equipmentGroup == null)
				return;

			foreach (Control cntrl in tabPage1.Controls)
			{
				if (cntrl is EquipmentPieceControl)
				{
					EquipmentPieceControl coveragePiece = (cntrl as EquipmentPieceControl);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Necklace) coveragePiece.SetEquipmentPiece(equipmentGroup.Necklace);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Trinket) coveragePiece.SetEquipmentPiece(equipmentGroup.Trinket);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.LeftBracelet) coveragePiece.SetEquipmentPiece(equipmentGroup.LeftBracelet);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.RightBracelet) coveragePiece.SetEquipmentPiece(equipmentGroup.RightBracelet);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.LeftRing) coveragePiece.SetEquipmentPiece(equipmentGroup.LeftRing);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.RightRing) coveragePiece.SetEquipmentPiece(equipmentGroup.RightRing);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Head) coveragePiece.SetEquipmentPiece(equipmentGroup.Head);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Chest) coveragePiece.SetEquipmentPiece(equipmentGroup.Chest);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.UpperArms) coveragePiece.SetEquipmentPiece(equipmentGroup.UpperArms);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.LowerArms) coveragePiece.SetEquipmentPiece(equipmentGroup.LowerArms);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Hands) coveragePiece.SetEquipmentPiece(equipmentGroup.Hands);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Abdomen) coveragePiece.SetEquipmentPiece(equipmentGroup.Abdomen);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.UpperLegs) coveragePiece.SetEquipmentPiece(equipmentGroup.UpperLegs);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.LowerLegs) coveragePiece.SetEquipmentPiece(equipmentGroup.LowerLegs);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Feet) coveragePiece.SetEquipmentPiece(equipmentGroup.Feet);

					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Shirt) coveragePiece.SetEquipmentPiece(equipmentGroup.Shirt);
					if (coveragePiece.EquipableSlots == Constants.EquippableSlotFlags.Pants) coveragePiece.SetEquipmentPiece(equipmentGroup.Pants);

					cntrl.Refresh();
				}
			}

			for (int col = 0 ; col < dataGridView1.Columns.Count ; col++)
			{
				for (int row = 0 ; row < dataGridView1.Rows.Count ; row++)
				{
					if (dataGridView1[col, row].Value == null)
						continue;

					dataGridView1[col, row].Style.BackColor = Color.White;

					if (dataGridView1[col, row].Style.BackColor == Color.White)
					{
						for (int spell = 0 ; spell < equipmentGroup.SpellCount ; spell++)
						{
							if (dataGridView1[col, row].Value.ToString() == "Armor" && !equipmentGroup.Spells[spell].Name.EndsWith("Armor"))
								continue;

							if (equipmentGroup.Spells[spell].Name.Contains(dataGridView1[col, row].Value.ToString()))
							{
								if (equipmentGroup.Spells[spell].IsEpic)
								{
									dataGridView1[col, row].Style.BackColor = Color.LightGreen;
									break;
								}
							}
						}
					}

					if (dataGridView1[col, row].Style.BackColor == Color.White)
					{
						for (int spell = 0 ; spell < equipmentGroup.SpellCount ; spell++)
						{
							if (dataGridView1[col, row].Value.ToString() == "Armor" && !equipmentGroup.Spells[spell].Name.EndsWith("Armor"))
								continue;

							if (equipmentGroup.Spells[spell].Name.Contains(dataGridView1[col, row].Value.ToString()))
							{
								if (equipmentGroup.Spells[spell].IsMajor)
								{
									dataGridView1[col, row].Style.BackColor = Color.Pink;
									break;
								}
							}
						}
					}

					if (dataGridView1[col, row].Style.BackColor == Color.White)
					{
						for (int spell = 0 ; spell < equipmentGroup.SpellCount ; spell++)
						{
							if (dataGridView1[col, row].Value.ToString() == "Armor" && !equipmentGroup.Spells[spell].Name.EndsWith("Armor"))
								continue;

							if (equipmentGroup.Spells[spell].Name.Contains(dataGridView1[col, row].Value.ToString()))
							{
								if (equipmentGroup.Spells[spell].IsMinor)
								{
									dataGridView1[col, row].Style.BackColor = Color.LightBlue;
									break;
								}
							}
						}
					}
				}
			}

			dataGridView1.Refresh();
		}
	}
}