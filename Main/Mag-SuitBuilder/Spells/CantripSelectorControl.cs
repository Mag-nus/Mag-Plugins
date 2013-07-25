using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace Mag_SuitBuilder.Spells
{
	public partial class CantripSelectorControl : UserControl, ICollection<Spell>, INotifyCollectionChanged
	{
		public CantripSelectorControl()
		{
			InitializeComponent();

			dataGridView1.Rows.Add(7);

			dataGridView1[0, 0].Value = "Strength";			dataGridView1[0, 0].Tag = new [] { Spell.GetSpell(6107), Spell.GetSpell(3965), Spell.GetSpell(2576), Spell.GetSpell(2583) };
			dataGridView1[0, 1].Value = "Endurance";		dataGridView1[0, 1].Tag = new [] { Spell.GetSpell(6104), Spell.GetSpell(4226), Spell.GetSpell(2573), Spell.GetSpell(2580) };
			dataGridView1[0, 2].Value = "Coordination";		dataGridView1[0, 2].Tag = new [] { Spell.GetSpell(6103), Spell.GetSpell(3963), Spell.GetSpell(2572), Spell.GetSpell(2579) };
			dataGridView1[0, 3].Value = "Quickness";		dataGridView1[0, 3].Tag = new [] { Spell.GetSpell(6106), Spell.GetSpell(4019), Spell.GetSpell(2575), Spell.GetSpell(2582) };
			dataGridView1[0, 4].Value = "Focus";			dataGridView1[0, 4].Tag = new [] { Spell.GetSpell(6105), Spell.GetSpell(3964), Spell.GetSpell(2574), Spell.GetSpell(2581) };
			dataGridView1[0, 5].Value = "Willpower";		dataGridView1[0, 5].Tag = new [] { Spell.GetSpell(6101), Spell.GetSpell(4227), Spell.GetSpell(2577), Spell.GetSpell(2584) };
			//

			dataGridView1[1, 0].Value = "Slashing Ward";	dataGridView1[1, 0].Tag = new [] { Spell.GetSpell(6085), Spell.GetSpell(3957), Spell.GetSpell(2614), Spell.GetSpell(2621) }; // 4678,Epic Slashing Ward
			dataGridView1[1, 1].Value = "Piercing Ward";	dataGridView1[1, 1].Tag = new [] { Spell.GetSpell(6084), Spell.GetSpell(3956), Spell.GetSpell(2613), Spell.GetSpell(2620) }; // 4677,Epic Piercing Ward
			dataGridView1[1, 2].Value = "Bludgeoning Ward"; dataGridView1[1, 2].Tag = new [] { Spell.GetSpell(6081), Spell.GetSpell(4674), Spell.GetSpell(2610), Spell.GetSpell(2617) };
			dataGridView1[1, 3].Value = "Flame Ward";		dataGridView1[1, 3].Tag = new [] { Spell.GetSpell(6082), Spell.GetSpell(4675), Spell.GetSpell(2611), Spell.GetSpell(2618) };
			dataGridView1[1, 4].Value = "Frost Ward";		dataGridView1[1, 4].Tag = new [] { Spell.GetSpell(6083), Spell.GetSpell(4676), Spell.GetSpell(2612), Spell.GetSpell(2619) };
			dataGridView1[1, 5].Value = "Acid Ward";		dataGridView1[1, 5].Tag = new [] { Spell.GetSpell(6080), Spell.GetSpell(4673), Spell.GetSpell(2609), Spell.GetSpell(2616) };
			dataGridView1[1, 6].Value = "Storm Ward";		dataGridView1[1, 6].Tag = new [] { Spell.GetSpell(6079), Spell.GetSpell(4679), Spell.GetSpell(2615), Spell.GetSpell(2622) };

			dataGridView1[2, 0].Value = "Life Magic";		dataGridView1[2, 0].Tag = new [] { Spell.GetSpell(6060), Spell.GetSpell(4700), Spell.GetSpell(2520), Spell.GetSpell(2555) };
			dataGridView1[2, 1].Value = "Creature Ench";	dataGridView1[2, 1].Tag = new [] { Spell.GetSpell(6046), Spell.GetSpell(4689), Spell.GetSpell(2507), Spell.GetSpell(2542) };
			dataGridView1[2, 2].Value = "Item Ench";		dataGridView1[2, 2].Tag = new [] { Spell.GetSpell(6056), Spell.GetSpell(4697), Spell.GetSpell(2516), Spell.GetSpell(2551) };
			dataGridView1[2, 3].Value = "War Magic";		dataGridView1[2, 3].Tag = new [] { Spell.GetSpell(6075), Spell.GetSpell(4715), Spell.GetSpell(2534), Spell.GetSpell(2569) };
			dataGridView1[2, 4].Value = "Void Magic";		dataGridView1[2, 4].Tag = new [] { Spell.GetSpell(6074), Spell.GetSpell(5429), Spell.GetSpell(5428), Spell.GetSpell(5427) };
			dataGridView1[2, 5].Value = "Mana C";			dataGridView1[2, 5].Tag = new [] { Spell.GetSpell(6064), Spell.GetSpell(4705), Spell.GetSpell(2525), Spell.GetSpell(2560) };
			dataGridView1[2, 6].Value = "Arcane";			dataGridView1[2, 6].Tag = new [] { Spell.GetSpell(6041), Spell.GetSpell(4684), Spell.GetSpell(2502), Spell.GetSpell(2537) };

			dataGridView1[3, 0].Value = "Missile";			dataGridView1[3, 0].Tag = new [] { Spell.GetSpell(6044), Spell.GetSpell(4687), Spell.GetSpell(2505), Spell.GetSpell(2540) }; // 4690 4713,Epic Missile Weapon Aptitude	2508 2532,Major Missile Weapon Aptitude	2543 2567,Minor Missile Weapon Aptitude
			dataGridView1[3, 1].Value = "Heavy";			dataGridView1[3, 1].Tag = new [] { Spell.GetSpell(6072), Spell.GetSpell(4712), Spell.GetSpell(2531), Spell.GetSpell(2566) };
			dataGridView1[3, 2].Value = "Light";			dataGridView1[3, 2].Tag = new [] { Spell.GetSpell(6043), Spell.GetSpell(4686), Spell.GetSpell(2504), Spell.GetSpell(2539) }; // 4702 4709 4711 4714,Epic Light Weapon Aptitude	2522 2528 2530 2533,Major Light Weapon Aptitude	2557 2563 2565 2568,Minor Light Weapon Aptitude
			dataGridView1[3, 3].Value = "Finesse";			dataGridView1[3, 3].Tag = new [] { Spell.GetSpell(6047), Spell.GetSpell(4691), Spell.GetSpell(2509), Spell.GetSpell(2544) };
			dataGridView1[3, 4].Value = "Healing";			dataGridView1[3, 4].Tag = new [] { Spell.GetSpell(6053), Spell.GetSpell(4694), Spell.GetSpell(2513), Spell.GetSpell(2548) };
			dataGridView1[3, 5].Value = "Shield";			dataGridView1[3, 5].Tag = new [] { Spell.GetSpell(6069), Spell.GetSpell(5896), Spell.GetSpell(5891), Spell.GetSpell(5886) };

			dataGridView1[4, 0].Value = "Two Hand";			dataGridView1[4, 0].Tag = new [] { Spell.GetSpell(6073), Spell.GetSpell(5034), Spell.GetSpell(5070), Spell.GetSpell(5072) };	
			dataGridView1[4, 1].Value = "Dual Wield";		dataGridView1[4, 1].Tag = new [] { Spell.GetSpell(6050), Spell.GetSpell(5894), Spell.GetSpell(5889), Spell.GetSpell(5884) };
			dataGridView1[4, 2].Value = "Dirty Fighting";	dataGridView1[4, 2].Tag = new [] { Spell.GetSpell(6049), Spell.GetSpell(5893), Spell.GetSpell(5888), Spell.GetSpell(5883) };
			dataGridView1[4, 3].Value = "Recklessness";		dataGridView1[4, 3].Tag = new [] { Spell.GetSpell(6067), Spell.GetSpell(5895), Spell.GetSpell(5890), Spell.GetSpell(5885) };
			dataGridView1[4, 4].Value = "Sneak Attack";		dataGridView1[4, 4].Tag = new [] { Spell.GetSpell(6070), Spell.GetSpell(5897), Spell.GetSpell(5892), Spell.GetSpell(5887) };
			dataGridView1[4, 5].Value = "Summoning";		dataGridView1[4, 5].Tag = new [] { Spell.GetSpell(6125), Spell.GetSpell(6124), Spell.GetSpell(6126), Spell.GetSpell(6127) };
			//
			//

			dataGridView1[5, 0].Value = "Invulnerability";	dataGridView1[5, 0].Tag = new [] { Spell.GetSpell(6055), Spell.GetSpell(4696), Spell.GetSpell(2515), Spell.GetSpell(2550) };	
			dataGridView1[5, 1].Value = "Magic Resistance";	dataGridView1[5, 1].Tag = new [] { Spell.GetSpell(6063), Spell.GetSpell(4704), Spell.GetSpell(2524), Spell.GetSpell(2559) };
			dataGridView1[5, 2].Value = "Impregnability";	dataGridView1[5, 2].Tag = new [] { Spell.GetSpell(6054), Spell.GetSpell(4695), Spell.GetSpell(2514), Spell.GetSpell(2549) };
			dataGridView1[5, 3].Value = "Armor";			dataGridView1[5, 3].Tag = new [] { Spell.GetSpell(6102), Spell.GetSpell(4911), Spell.GetSpell(2571), Spell.GetSpell(2578) };
			dataGridView1[5, 4].Value = "Deception";		dataGridView1[5, 4].Tag = new [] { Spell.GetSpell(6048), Spell.GetSpell(4020), Spell.GetSpell(2510), Spell.GetSpell(2545) };
			dataGridView1[5, 5].Value = "Person";			dataGridView1[5, 5].Tag = new [] { Spell.GetSpell(6066), Spell.GetSpell(4707), Spell.GetSpell(2527), Spell.GetSpell(2562) };
			dataGridView1[5, 6].Value = "Monster";			dataGridView1[5, 6].Tag = new [] { Spell.GetSpell(6065), Spell.GetSpell(4706), Spell.GetSpell(2526), Spell.GetSpell(2561) };

			dataGridView1[6, 0].Value = "Item Tinker";		dataGridView1[6, 0].Tag = new [] { Spell.GetSpell(6057), Spell.GetSpell(4698), Spell.GetSpell(2517), Spell.GetSpell(2552) }; // 5033,Epic Item Tinkering Expertise	5069,Major Item Tinkering Expertise	5071,Minor Item Tinkering Expertise
			dataGridView1[6, 1].Value = "Armor Tinker";		dataGridView1[6, 1].Tag = new [] { Spell.GetSpell(6042), Spell.GetSpell(4685), Spell.GetSpell(2503), Spell.GetSpell(2538) };
			dataGridView1[6, 2].Value = "Weapon Tinker";	dataGridView1[6, 2].Tag = new [] { Spell.GetSpell(6039), Spell.GetSpell(4912), Spell.GetSpell(2535), Spell.GetSpell(2570) };
			dataGridView1[6, 3].Value = "Magic Item";		dataGridView1[6, 3].Tag = new [] { Spell.GetSpell(6062), Spell.GetSpell(4703), Spell.GetSpell(2523), Spell.GetSpell(2558) };
			dataGridView1[6, 4].Value = "Cooking";			dataGridView1[6, 4].Tag = new [] { Spell.GetSpell(6045), Spell.GetSpell(4688), Spell.GetSpell(2506), Spell.GetSpell(2541) };
			dataGridView1[6, 5].Value = "Alchemy";			dataGridView1[6, 5].Tag = new [] { Spell.GetSpell(6040), Spell.GetSpell(4683), Spell.GetSpell(2501), Spell.GetSpell(2536) };
			dataGridView1[6, 6].Value = "Fletching";		dataGridView1[6, 6].Tag = new [] { Spell.GetSpell(6052), Spell.GetSpell(4693), Spell.GetSpell(2512), Spell.GetSpell(2547) };

			// Run through and check for errors
			/*
			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Tag == null)
						continue;

					Spell[] cellSpells = cell.Tag as Spell[];
					
					if (cellSpells != null)
					{
						for (int i = 1 ; i < cellSpells.Length ; i++)
						{
							if (!cellSpells[0].IsOfSameFamilyAndGroup(cellSpells[i]))
								throw new System.Exception("Spell group mismatch detected for cell: " + cell.Value + " " + cellSpells[0] + " - " + cellSpells[i]);
						}
					}
				}
			}
			*/
		}

		Collection<Spell> items = new Collection<Spell>();

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public IEnumerator<Spell> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Add(Spell item)
		{
			for (int i = items.Count - 1 ; i >= 0 ; i--)
			{
				if (items[i].IsSameOrSurpasses(item))
					return;

				if (item.Surpasses(items[i]))
					Remove(items[i]);
			}
		
			items.Add(item);

			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Tag == null)
						continue;

					Spell[] spells = cell.Tag as Spell[];

					if (spells != null && spells[0].IsOfSameFamilyAndGroup(item))
					{
						Color newColor = Color.Empty;

						if (spells.Length > 0 && item.IsSameOrSurpasses(spells[0]))
							newColor = lblLegendary.BackColor;
						else if (spells.Length > 1 && item.IsSameOrSurpasses(spells[1]))
							newColor = lblEpic.BackColor;
						else if (spells.Length > 2 && item.IsSameOrSurpasses(spells[2]))
							newColor = lblMajor.BackColor;
						else if (spells.Length > 3 && item.IsSameOrSurpasses(spells[3]))
							newColor = lblMinor.BackColor;

						cell.Style.BackColor = newColor;
						cell.Style.SelectionBackColor = newColor;

						return;
					}
				}
			}
		}

		public void Clear()
		{
			items.Clear();

			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					cell.Style.BackColor = Color.Empty;
					cell.Style.SelectionBackColor = Color.Empty;
				}
			}
		}

		public bool Contains(Spell item)
		{
			return items.Contains(item);
		}

		public void CopyTo(Spell[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public bool Remove(Spell item)
		{
			bool result = items.Remove(item);

			if (result)
			{
				if (CollectionChanged != null)
					CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
			}

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					if (cell.Tag == null)
						continue;

					Spell[] spells = cell.Tag as Spell[];

					if (spells != null && spells[0].IsOfSameFamilyAndGroup(item))
					{
						cell.Style.BackColor = Color.Empty;
						cell.Style.SelectionBackColor = Color.Empty;
						return result;
					}
				}
			}

			return result;
		}

		public int Count { get { return items.Count; } }
		public bool IsReadOnly { get { return false; } }

		public void LoadDefaults(string skill)
		{
			Clear();

			// Attributes
			switch (skill)
			{
				case "War":
				case "Void":
					Add(Spell.GetSpell(6104));
					Add(Spell.GetSpell(6103));
					Add(Spell.GetSpell(6106));
					Add(Spell.GetSpell(6105));
					Add(Spell.GetSpell(6101));
					break;

				case "Missile":
				case "Heavy":
				case "Light":
				case "Finesse":
				case "Two Hand":
				case "Dual Wield":
					Add(Spell.GetSpell(6107));
					Add(Spell.GetSpell(6104));
					Add(Spell.GetSpell(6103));
					Add(Spell.GetSpell(6106));
					Add(Spell.GetSpell(6105));
					Add(Spell.GetSpell(6101));
					break;

				case "Tinker":
					Add(Spell.GetSpell(6107));
					Add(Spell.GetSpell(6104));
					Add(Spell.GetSpell(6103));
					Add(Spell.GetSpell(6105));
					break;
			}

			// Wards
			switch (skill)
			{
				case "War":
				case "Void":
				case "Missile":
				case "Heavy":
				case "Light":
				case "Finesse":
				case "Two Hand":
				case "Dual Wield":
					Add(Spell.GetSpell(6085));
					Add(Spell.GetSpell(6084));
					Add(Spell.GetSpell(6081));
					Add(Spell.GetSpell(6082));
					Add(Spell.GetSpell(6083));
					Add(Spell.GetSpell(6080));
					Add(Spell.GetSpell(6079));
					break;

				case "Tinker":
					break;
			}

			// Magic/Melee/Armor
			switch (skill)
			{
				case "War":
				case "Void":
				case "Missile":
				case "Heavy":
				case "Light":
				case "Finesse":
				case "Two Hand":
				case "Dual Wield":
					Add(Spell.GetSpell(6055));
					Add(Spell.GetSpell(6063));
					Add(Spell.GetSpell(6102));
					break;

				case "Tinker":
					break;
			}

			// Skills
			switch (skill)
			{
				case "War":
					Add(Spell.GetSpell(6075));
					break;
				case "Void":
					Add(Spell.GetSpell(6074));
					break;
				case "Missile":
					Add(Spell.GetSpell(6044));
					Add(Spell.GetSpell(6053)); // Healing
					Add(Spell.GetSpell(6052)); // Fletching
					break;
				case "Heavy":
					Add(Spell.GetSpell(6072));
					Add(Spell.GetSpell(6053)); // Healing
					break;
				case "Light":
					Add(Spell.GetSpell(6043));
					Add(Spell.GetSpell(6053)); // Healing
					break;
				case "Finesse":
					Add(Spell.GetSpell(6047));
					Add(Spell.GetSpell(6053)); // Healing
					break;
				case "Two Hand":
					Add(Spell.GetSpell(6073));
					Add(Spell.GetSpell(6053)); // Healing
					break;
				case "Dual Wield":
					Add(Spell.GetSpell(6050));
					Add(Spell.GetSpell(6053)); // Healing
					break;

				case "Tinker":
					Add(Spell.GetSpell(6057));
					Add(Spell.GetSpell(6042));
					Add(Spell.GetSpell(6039));
					Add(Spell.GetSpell(6062));
					Add(Spell.GetSpell(6045));
					Add(Spell.GetSpell(6040));
					break;
			}
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewCell cell = dataGridView1[e.ColumnIndex, e.RowIndex];

			if (cell.Value == null || cell.Tag == null)
				return;

			int index = -1;

			if (cell.Style.BackColor == lblLegendary.BackColor)
				index = 0;
			else if (cell.Style.BackColor == lblEpic.BackColor)
				index = 1;
			else if (cell.Style.BackColor == lblMajor.BackColor)
				index = 2;
			else if (cell.Style.BackColor == lblMinor.BackColor)
				index = 3;

			Spell[] spells = cell.Tag as Spell[];

			if (spells == null)
				return;

			if (index != -1)
				Remove(spells[index]);

			if (index >= spells.Length - 1)
				return;

			index++;

			Add(spells[index]);
		}

		private void cmdLoadDefaults_Click(object sender, System.EventArgs e)
		{
			LoadDefaults(defaultsComboBox.Text);
		}

		private void cmdClear_Click(object sender, System.EventArgs e)
		{
			Clear();
		}
	}
}
