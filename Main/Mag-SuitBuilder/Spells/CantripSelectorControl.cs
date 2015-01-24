using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

using Mag.Shared.Spells;

namespace Mag_SuitBuilder.Spells
{
	public partial class CantripSelectorControl : UserControl, ICollection<Spell>, INotifyCollectionChanged
	{
		public CantripSelectorControl()
		{
			InitializeComponent();

			dataGridView1.Rows.Add(7);

			dataGridView1[0, 0].Value = "Strength";			dataGridView1[0, 0].Tag = new [] { SpellTools.GetSpell(6107), SpellTools.GetSpell(3965), SpellTools.GetSpell(2576), SpellTools.GetSpell(2583) };
			dataGridView1[0, 1].Value = "Endurance";		dataGridView1[0, 1].Tag = new [] { SpellTools.GetSpell(6104), SpellTools.GetSpell(4226), SpellTools.GetSpell(2573), SpellTools.GetSpell(2580) };
			dataGridView1[0, 2].Value = "Coordination";		dataGridView1[0, 2].Tag = new [] { SpellTools.GetSpell(6103), SpellTools.GetSpell(3963), SpellTools.GetSpell(2572), SpellTools.GetSpell(2579) };
			dataGridView1[0, 3].Value = "Quickness";		dataGridView1[0, 3].Tag = new [] { SpellTools.GetSpell(6106), SpellTools.GetSpell(4019), SpellTools.GetSpell(2575), SpellTools.GetSpell(2582) };
			dataGridView1[0, 4].Value = "Focus";			dataGridView1[0, 4].Tag = new [] { SpellTools.GetSpell(6105), SpellTools.GetSpell(3964), SpellTools.GetSpell(2574), SpellTools.GetSpell(2581) };
			dataGridView1[0, 5].Value = "Willpower";		dataGridView1[0, 5].Tag = new [] { SpellTools.GetSpell(6101), SpellTools.GetSpell(4227), SpellTools.GetSpell(2577), SpellTools.GetSpell(2584) };
			//

			dataGridView1[1, 0].Value = "Slashing Ward";	dataGridView1[1, 0].Tag = new [] { SpellTools.GetSpell(6085), SpellTools.GetSpell(3957), SpellTools.GetSpell(2614), SpellTools.GetSpell(2621) }; // 4678,Epic Slashing Ward
			dataGridView1[1, 1].Value = "Piercing Ward";	dataGridView1[1, 1].Tag = new [] { SpellTools.GetSpell(6084), SpellTools.GetSpell(3956), SpellTools.GetSpell(2613), SpellTools.GetSpell(2620) }; // 4677,Epic Piercing Ward
			dataGridView1[1, 2].Value = "Bludgeoning Ward"; dataGridView1[1, 2].Tag = new [] { SpellTools.GetSpell(6081), SpellTools.GetSpell(4674), SpellTools.GetSpell(2610), SpellTools.GetSpell(2617) };
			dataGridView1[1, 3].Value = "Flame Ward";		dataGridView1[1, 3].Tag = new [] { SpellTools.GetSpell(6082), SpellTools.GetSpell(4675), SpellTools.GetSpell(2611), SpellTools.GetSpell(2618) };
			dataGridView1[1, 4].Value = "Frost Ward";		dataGridView1[1, 4].Tag = new [] { SpellTools.GetSpell(6083), SpellTools.GetSpell(4676), SpellTools.GetSpell(2612), SpellTools.GetSpell(2619) };
			dataGridView1[1, 5].Value = "Acid Ward";		dataGridView1[1, 5].Tag = new [] { SpellTools.GetSpell(6080), SpellTools.GetSpell(4673), SpellTools.GetSpell(2609), SpellTools.GetSpell(2616) };
			dataGridView1[1, 6].Value = "Storm Ward";		dataGridView1[1, 6].Tag = new [] { SpellTools.GetSpell(6079), SpellTools.GetSpell(4679), SpellTools.GetSpell(2615), SpellTools.GetSpell(2622) };

			dataGridView1[2, 0].Value = "Life Magic";		dataGridView1[2, 0].Tag = new [] { SpellTools.GetSpell(6060), SpellTools.GetSpell(4700), SpellTools.GetSpell(2520), SpellTools.GetSpell(2555) };
			dataGridView1[2, 1].Value = "Creature Ench";	dataGridView1[2, 1].Tag = new [] { SpellTools.GetSpell(6046), SpellTools.GetSpell(4689), SpellTools.GetSpell(2507), SpellTools.GetSpell(2542) };
			dataGridView1[2, 2].Value = "Item Ench";		dataGridView1[2, 2].Tag = new [] { SpellTools.GetSpell(6056), SpellTools.GetSpell(4697), SpellTools.GetSpell(2516), SpellTools.GetSpell(2551) };
			dataGridView1[2, 3].Value = "War Magic";		dataGridView1[2, 3].Tag = new [] { SpellTools.GetSpell(6075), SpellTools.GetSpell(4715), SpellTools.GetSpell(2534), SpellTools.GetSpell(2569) };
			dataGridView1[2, 4].Value = "Void Magic";		dataGridView1[2, 4].Tag = new [] { SpellTools.GetSpell(6074), SpellTools.GetSpell(5429), SpellTools.GetSpell(5428), SpellTools.GetSpell(5427) };
			dataGridView1[2, 5].Value = "Mana C";			dataGridView1[2, 5].Tag = new [] { SpellTools.GetSpell(6064), SpellTools.GetSpell(4705), SpellTools.GetSpell(2525), SpellTools.GetSpell(2560) };
			dataGridView1[2, 6].Value = "Arcane";			dataGridView1[2, 6].Tag = new [] { SpellTools.GetSpell(6041), SpellTools.GetSpell(4684), SpellTools.GetSpell(2502), SpellTools.GetSpell(2537) };

			dataGridView1[3, 0].Value = "Missile";			dataGridView1[3, 0].Tag = new [] { SpellTools.GetSpell(6044), SpellTools.GetSpell(4687), SpellTools.GetSpell(2505), SpellTools.GetSpell(2540) }; // 4690 4713,Epic Missile Weapon Aptitude	2508 2532,Major Missile Weapon Aptitude	2543 2567,Minor Missile Weapon Aptitude
			dataGridView1[3, 1].Value = "Heavy";			dataGridView1[3, 1].Tag = new [] { SpellTools.GetSpell(6072), SpellTools.GetSpell(4712), SpellTools.GetSpell(2531), SpellTools.GetSpell(2566) };
			dataGridView1[3, 2].Value = "Light";			dataGridView1[3, 2].Tag = new [] { SpellTools.GetSpell(6043), SpellTools.GetSpell(4686), SpellTools.GetSpell(2504), SpellTools.GetSpell(2539) }; // 4702 4709 4711 4714,Epic Light Weapon Aptitude	2522 2528 2530 2533,Major Light Weapon Aptitude	2557 2563 2565 2568,Minor Light Weapon Aptitude
			dataGridView1[3, 3].Value = "Finesse";			dataGridView1[3, 3].Tag = new [] { SpellTools.GetSpell(6047), SpellTools.GetSpell(4691), SpellTools.GetSpell(2509), SpellTools.GetSpell(2544) };
			dataGridView1[3, 4].Value = "Healing";			dataGridView1[3, 4].Tag = new [] { SpellTools.GetSpell(6053), SpellTools.GetSpell(4694), SpellTools.GetSpell(2513), SpellTools.GetSpell(2548) };
			dataGridView1[3, 5].Value = "Shield";			dataGridView1[3, 5].Tag = new [] { SpellTools.GetSpell(6069), SpellTools.GetSpell(5896), SpellTools.GetSpell(5891), SpellTools.GetSpell(5886) };

			dataGridView1[4, 0].Value = "Two Hand";			dataGridView1[4, 0].Tag = new [] { SpellTools.GetSpell(6073), SpellTools.GetSpell(5034), SpellTools.GetSpell(5070), SpellTools.GetSpell(5072) };	
			dataGridView1[4, 1].Value = "Dual Wield";		dataGridView1[4, 1].Tag = new [] { SpellTools.GetSpell(6050), SpellTools.GetSpell(5894), SpellTools.GetSpell(5889), SpellTools.GetSpell(5884) };
			dataGridView1[4, 2].Value = "Dirty Fighting";	dataGridView1[4, 2].Tag = new [] { SpellTools.GetSpell(6049), SpellTools.GetSpell(5893), SpellTools.GetSpell(5888), SpellTools.GetSpell(5883) };
			dataGridView1[4, 3].Value = "Recklessness";		dataGridView1[4, 3].Tag = new [] { SpellTools.GetSpell(6067), SpellTools.GetSpell(5895), SpellTools.GetSpell(5890), SpellTools.GetSpell(5885) };
			dataGridView1[4, 4].Value = "Sneak Attack";		dataGridView1[4, 4].Tag = new [] { SpellTools.GetSpell(6070), SpellTools.GetSpell(5897), SpellTools.GetSpell(5892), SpellTools.GetSpell(5887) };
			dataGridView1[4, 5].Value = "Summoning";		dataGridView1[4, 5].Tag = new [] { SpellTools.GetSpell(6125), SpellTools.GetSpell(6124), SpellTools.GetSpell(6126), SpellTools.GetSpell(6127) };
			//
			//

			dataGridView1[5, 0].Value = "Invulnerability";	dataGridView1[5, 0].Tag = new [] { SpellTools.GetSpell(6055), SpellTools.GetSpell(4696), SpellTools.GetSpell(2515), SpellTools.GetSpell(2550) };	
			dataGridView1[5, 1].Value = "Magic Resistance";	dataGridView1[5, 1].Tag = new [] { SpellTools.GetSpell(6063), SpellTools.GetSpell(4704), SpellTools.GetSpell(2524), SpellTools.GetSpell(2559) };
			dataGridView1[5, 2].Value = "Impregnability";	dataGridView1[5, 2].Tag = new [] { SpellTools.GetSpell(6054), SpellTools.GetSpell(4695), SpellTools.GetSpell(2514), SpellTools.GetSpell(2549) };
			dataGridView1[5, 3].Value = "Armor";			dataGridView1[5, 3].Tag = new [] { SpellTools.GetSpell(6102), SpellTools.GetSpell(4911), SpellTools.GetSpell(2571), SpellTools.GetSpell(2578) };
			dataGridView1[5, 4].Value = "Deception";		dataGridView1[5, 4].Tag = new [] { SpellTools.GetSpell(6048), SpellTools.GetSpell(4020), SpellTools.GetSpell(2510), SpellTools.GetSpell(2545) };
			dataGridView1[5, 5].Value = "Person";			dataGridView1[5, 5].Tag = new [] { SpellTools.GetSpell(6066), SpellTools.GetSpell(4707), SpellTools.GetSpell(2527), SpellTools.GetSpell(2562) };
			dataGridView1[5, 6].Value = "Monster";			dataGridView1[5, 6].Tag = new [] { SpellTools.GetSpell(6065), SpellTools.GetSpell(4706), SpellTools.GetSpell(2526), SpellTools.GetSpell(2561) };

			dataGridView1[6, 0].Value = "Item Tinker";		dataGridView1[6, 0].Tag = new [] { SpellTools.GetSpell(6057), SpellTools.GetSpell(4698), SpellTools.GetSpell(2517), SpellTools.GetSpell(2552) }; // 5033,Epic Item Tinkering Expertise	5069,Major Item Tinkering Expertise	5071,Minor Item Tinkering Expertise
			dataGridView1[6, 1].Value = "Armor Tinker";		dataGridView1[6, 1].Tag = new [] { SpellTools.GetSpell(6042), SpellTools.GetSpell(4685), SpellTools.GetSpell(2503), SpellTools.GetSpell(2538) };
			dataGridView1[6, 2].Value = "Weapon Tinker";	dataGridView1[6, 2].Tag = new [] { SpellTools.GetSpell(6039), SpellTools.GetSpell(4912), SpellTools.GetSpell(2535), SpellTools.GetSpell(2570) };
			dataGridView1[6, 3].Value = "Magic Item";		dataGridView1[6, 3].Tag = new [] { SpellTools.GetSpell(6062), SpellTools.GetSpell(4703), SpellTools.GetSpell(2523), SpellTools.GetSpell(2558) };
			dataGridView1[6, 4].Value = "Cooking";			dataGridView1[6, 4].Tag = new [] { SpellTools.GetSpell(6045), SpellTools.GetSpell(4688), SpellTools.GetSpell(2506), SpellTools.GetSpell(2541) };
			dataGridView1[6, 5].Value = "Alchemy";			dataGridView1[6, 5].Tag = new [] { SpellTools.GetSpell(6040), SpellTools.GetSpell(4683), SpellTools.GetSpell(2501), SpellTools.GetSpell(2536) };
			dataGridView1[6, 6].Value = "Fletching";		dataGridView1[6, 6].Tag = new [] { SpellTools.GetSpell(6052), SpellTools.GetSpell(4693), SpellTools.GetSpell(2512), SpellTools.GetSpell(2547) };

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

		readonly Collection<Spell> items = new Collection<Spell>();

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		private bool suspendChangedEvent;

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

			if (!suspendChangedEvent && CollectionChanged != null)
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

			if (!suspendChangedEvent && CollectionChanged != null)
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
				if (!suspendChangedEvent && CollectionChanged != null)
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
			suspendChangedEvent = true;

			Clear();

			// Attributes
			switch (skill)
			{
				case "War":
				case "Void":
					Add(SpellTools.GetSpell(6104));
					Add(SpellTools.GetSpell(6103));
					Add(SpellTools.GetSpell(6106));
					Add(SpellTools.GetSpell(6105));
					Add(SpellTools.GetSpell(6101));
					break;

				case "Missile":
				case "Heavy":
				case "Light":
				case "Finesse":
				case "Two Hand":
				case "Dual Wield":
					Add(SpellTools.GetSpell(6107));
					Add(SpellTools.GetSpell(6104));
					Add(SpellTools.GetSpell(6103));
					Add(SpellTools.GetSpell(6106));
					Add(SpellTools.GetSpell(6105));
					Add(SpellTools.GetSpell(6101));
					break;

				case "Tinker":
					Add(SpellTools.GetSpell(6107));
					Add(SpellTools.GetSpell(6104));
					Add(SpellTools.GetSpell(6103));
					Add(SpellTools.GetSpell(6105));
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
					Add(SpellTools.GetSpell(6085));
					Add(SpellTools.GetSpell(6084));
					Add(SpellTools.GetSpell(6081));
					Add(SpellTools.GetSpell(6082));
					Add(SpellTools.GetSpell(6083));
					Add(SpellTools.GetSpell(6080));
					Add(SpellTools.GetSpell(6079));
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
					Add(SpellTools.GetSpell(6055));
					Add(SpellTools.GetSpell(6063));
					Add(SpellTools.GetSpell(6102));
					break;

				case "Tinker":
					break;
			}

			// Skills
			switch (skill)
			{
				case "War":
					Add(SpellTools.GetSpell(6075));
					break;
				case "Void":
					Add(SpellTools.GetSpell(6074));
					break;
				case "Missile":
					Add(SpellTools.GetSpell(6044));
					Add(SpellTools.GetSpell(6053)); // Healing
					Add(SpellTools.GetSpell(6052)); // Fletching
					break;
				case "Heavy":
					Add(SpellTools.GetSpell(6072));
					Add(SpellTools.GetSpell(6053)); // Healing
					break;
				case "Light":
					Add(SpellTools.GetSpell(6043));
					Add(SpellTools.GetSpell(6053)); // Healing
					break;
				case "Finesse":
					Add(SpellTools.GetSpell(6047));
					Add(SpellTools.GetSpell(6053)); // Healing
					break;
				case "Two Hand":
					Add(SpellTools.GetSpell(6073));
					Add(SpellTools.GetSpell(6053)); // Healing
					break;
				case "Dual Wield":
					Add(SpellTools.GetSpell(6050));
					Add(SpellTools.GetSpell(6053)); // Healing
					break;

				case "Tinker":
					Add(SpellTools.GetSpell(6057));
					Add(SpellTools.GetSpell(6042));
					Add(SpellTools.GetSpell(6039));
					Add(SpellTools.GetSpell(6062));
					Add(SpellTools.GetSpell(6045));
					Add(SpellTools.GetSpell(6040));
					break;
			}

			if (CollectionChanged != null)
				CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

			suspendChangedEvent = false;
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
