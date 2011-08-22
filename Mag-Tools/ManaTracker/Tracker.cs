using System;
using System.Collections.Generic;

using Decal.Adapter.Wrappers;
using Decal.Filters;
using MyClasses.MetaViewWrappers;

namespace MagTools.ManaTracker
{
	class Tracker : IDisposable
	{
		private IList ManaList;
		private IStaticText ManaTotal;
		private ICheckBox RefillMana;

		private System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();

		public Tracker()
		{
			try
			{
				this.ManaList = PluginCore.mainView.ManaList;
				this.ManaTotal = PluginCore.mainView.ManaTotal;
				this.RefillMana = PluginCore.mainView.RefillMana;

				PluginCore.core.CharacterFilter.LoginComplete += new EventHandler(CharacterFilter_LoginComplete);
				PluginCore.core.CharacterFilter.Logoff += new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);

				Timer.Interval = 500;
				Timer.Tick += new EventHandler(Timer_Tick);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		public void Dispose()
		{
			try
			{
				PluginCore.core.CharacterFilter.LoginComplete -= new EventHandler(CharacterFilter_LoginComplete);
				PluginCore.core.CharacterFilter.Logoff -= new EventHandler<Decal.Adapter.Wrappers.LogoffEventArgs>(CharacterFilter_Logoff);
			}
			catch (Exception ex) { Util.LogError(ex); }
		}


		void CharacterFilter_LoginComplete(object sender, EventArgs e)
		{
			try
			{
				Timer.Start();

				RefreshInventoryList();
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		void CharacterFilter_Logoff(object sender, Decal.Adapter.Wrappers.LogoffEventArgs e)
		{
			try
			{
				Timer.Stop();
			}
			catch (Exception ex) { Util.LogError(ex); }
		}


		private uint tick;

		void Timer_Tick(object sender, EventArgs e)
		{
			try
			{
				tick++;

				// Only refresh our inventory list once every 20 think itterations. Each itteration is .5 sec
				if (tick % 20 == 0)
					RefreshInventoryList();

				// Add auto recharge stuff here
			}
			catch (Exception ex) { Util.LogError(ex); }
		}

		private void RefreshInventoryList()
		{
			const int IconActive = 0x60011f9;
			const int IconInactive = 0x60011f8;
			const int IconNone = 0x600287A;

			// Go through and find all of our current active spells (enchantments)
			List<int> activeSpellsOnChar = new List<int>();

			foreach (EnchantmentWrapper wrapper in PluginCore.core.CharacterFilter.Enchantments)
			{
				// Only add ones that are cast by items (have no duration)
				if (wrapper.TimeRemaining == -1)
					activeSpellsOnChar.Add(wrapper.SpellId);
			}


			// Cycle through our inventory and check equipped items
			FileService service = PluginCore.core.Filter<FileService>();

			List<int> equippedItems = new List<int>();

			foreach (WorldObject item in PluginCore.core.WorldFilter.GetByContainer(PluginCore.core.CharacterFilter.Id))
			{
				// If this is not an equipped item, we don't need to check it
				if (item.Values(LongValueKey.EquippedSlots) <= 0)
					continue;

				// We also don't load aetheria
				if (item.Name != null && item.Name.Contains("Aetheria"))
					continue;

				// Don't show arrows
				if (item.Values(LongValueKey.EquippedSlots) == 8388608)
					continue;


				equippedItems.Add(item.Id);

				// If we don't have Id data for this item yet, lets request it. We won't have the extended info this time around, but should for the next.
				if (!item.HasIdData)
					PluginCore.host.Actions.RequestId(item.Id);


				// Lets check if the item is not active. We start off by assuming it is active
				bool itemIsActive = true;

				// Go through all of this items spells to determine if all are active.
				for (int i = 0 ; i < item.Values(LongValueKey.SpellCount) ; i++)
				{
					int spellOnItemId = item.Spell(i);

					if (item.Exists(LongValueKey.AssociatedSpell) && (item.Values(LongValueKey.AssociatedSpell) == spellOnItemId))
						continue;

					Spell spellOnItem = service.SpellTable.GetById(spellOnItemId);

					// If it is offensive, it is probably a cast on strike spell
					if (spellOnItem.IsDebuff || spellOnItem.IsOffensive)
						continue;

					// Check if this particular spell is active
					bool hasActiveSpell = false;

					// Check to see if this item cast any spells on itself.
					for (int j = 0 ; j < item.Values(LongValueKey.ActiveSpellCount) ; j++)
					{
						int activeSpellOnItemId = item.ActiveSpell(j);

						if ((service.SpellTable.GetById(activeSpellOnItemId).Family == spellOnItem.Family) && (service.SpellTable.GetById(activeSpellOnItemId).Difficulty >= spellOnItem.Difficulty))
						{
							hasActiveSpell = true;
							break;
						}
					}

					if (hasActiveSpell)
						continue;


					// Check to see if this item cast any spells on the player.
					foreach (int j in activeSpellsOnChar)
					{
						if (service.SpellTable.GetById(j) != null && (service.SpellTable.GetById(j).Family == spellOnItem.Family) && (service.SpellTable.GetById(j).Difficulty >= spellOnItem.Difficulty))
						{
							hasActiveSpell = true;
							break;
						}
					}

					if (hasActiveSpell)
						continue;

					// This item has not cast this particular spell.
					itemIsActive = false;
					break;
				}


				// If this item has no mana, it can't be active
				if (itemIsActive && item.HasIdData && item.Values(LongValueKey.CurrentMana, 0) == 0)
					itemIsActive = false;


				// Make sure this item is in our list
				for (int row = 0 ; row <= ManaList.RowCount ; row++)
				{
					if (row != 0 && int.Parse(ManaList[row - 1][5][0].ToString()) == item.Id)
					{
						if (item.Values(LongValueKey.SpellCount) != 0)
						{
							if (itemIsActive)			ManaList[row - 1][2][1] = IconActive;
							else if (item.HasIdData)	ManaList[row - 1][2][1] = IconInactive;
							else						ManaList[row - 1][2][1] = IconNone;
						}

						break;
					}

					// We've reached the end of this meaning we haven't found it, so we'll add it
					if (row == ManaList.RowCount)
					{
						IListRow newRow = ManaList.Add();
						newRow[0][1] = item.Icon + 0x6000000;
						newRow[1][0] = item.Name;
						newRow[2][1] = IconNone;
						if (item.Values(LongValueKey.SpellCount) != 0)
						{
							if (itemIsActive)			newRow[2][1] = IconActive;
							else if (item.HasIdData)	newRow[2][1] = IconInactive;
							else						newRow[2][1] = IconNone;
						}
						newRow[5][0] = item.Id.ToString();
						newRow[6][0] = int.MaxValue.ToString();

						break;
					}
				}
			}

			// Go through our list and remove any items that are not in our equippedItems list
			for (int row = 1 ; row <= ManaList.RowCount ; row++)
			{
				if (!equippedItems.Contains(int.Parse(ManaList[row - 1][5][0].ToString())))
				{
					ManaList.RemoveRow(row - 1);

					row--;
				}
			}


			// Go through our list and update the current mana and time left
			int totalManaNeededToRefill = 0;

			for (int row = 0 ; row < ManaList.RowCount ; row++)
			{
				int itemId = int.Parse(ManaList[row][5][0].ToString());

				WorldObject wo = PluginCore.core.WorldFilter[itemId];

				if (wo == null || !wo.HasIdData || !wo.Exists(DoubleValueKey.ManaRateOfChange) || int.Parse(ManaList[row][2][1].ToString()) == IconNone)
				{
					ManaList[row][3][0] = "-";
					ManaList[row][4][0] = "-";
					ManaList[row][6][0] = int.MaxValue.ToString();
				}
				else
				{
					int burnedMana = 0;
					TimeSpan timeRemaining = new TimeSpan();

					// Calculate how much mana we've burned
					if ((int)ManaList[row][2][1] == IconActive)
					{
						DateTime utcLastIdTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(wo.LastIdTime);
						TimeSpan timeSinceLastIdent = DateTime.UtcNow - utcLastIdTime;

						// Items can burn mana in 5 sec increments. If it says 1 every 18 sec, its really 1 every 20.
						double secondsPerBurn = ((int)Math.Ceiling(-0.2 / wo.Values(DoubleValueKey.ManaRateOfChange))) * 5;

						// Calculate how much mana has been burned off since we last identified the object.
						burnedMana = (int)(timeSinceLastIdent.TotalSeconds / secondsPerBurn);

						if (burnedMana > wo.Values(LongValueKey.CurrentMana))
							burnedMana = wo.Values(LongValueKey.CurrentMana);

						// Calculate how much time is left before this item runs out of mana
						timeRemaining = TimeSpan.FromSeconds((wo.Values(LongValueKey.CurrentMana) - burnedMana) * secondsPerBurn);
					}

					// Update the display
					ManaList[row][3][0] = (wo.Values(LongValueKey.CurrentMana) - burnedMana) + " / " + wo.Values(LongValueKey.MaximumMana);

					totalManaNeededToRefill = wo.Values(LongValueKey.MaximumMana) - ((wo.Values(LongValueKey.CurrentMana) - burnedMana));

					ManaList[row][4][0] = string.Format("{0:d}h{1:d2}m", (int)timeRemaining.TotalHours, (int)timeRemaining.Minutes);

					ManaList[row][6][0] = timeRemaining.TotalSeconds.ToString();
				}
			}

			ManaTotal.Text = "Mana needed: " + totalManaNeededToRefill;


			// Sorth the list based on active mana duration in items
			if (ManaList.RowCount > 1)
			{
				for (int row = 0 ; row < ManaList.RowCount - 1 ; row++)
				{
					for (int compareRow = row + 1 ; compareRow < ManaList.RowCount ; compareRow++)
					{
						if (double.Parse(ManaList[row][6][0].ToString()) > double.Parse(ManaList[compareRow][6][0].ToString()))
						{
							object obj;
							
							obj = ManaList[row][0][1];
							ManaList[row][0][1] = ManaList[compareRow][0][1];
							ManaList[compareRow][0][1] = obj;

							obj = ManaList[row][1][0];
							ManaList[row][1][0] = ManaList[compareRow][1][0];
							ManaList[compareRow][1][0] = obj;

							obj = ManaList[row][2][1];
							ManaList[row][2][1] = ManaList[compareRow][2][1];
							ManaList[compareRow][2][1] = obj;

							obj = ManaList[row][3][0];
							ManaList[row][3][0] = ManaList[compareRow][3][0];
							ManaList[compareRow][3][0] = obj;

							obj = ManaList[row][4][0];
							ManaList[row][4][0] = ManaList[compareRow][4][0];
							ManaList[compareRow][4][0] = obj;

							obj = ManaList[row][5][0];
							ManaList[row][5][0] = ManaList[compareRow][5][0];
							ManaList[compareRow][5][0] = obj;

							obj = ManaList[row][6][0];
							ManaList[row][6][0] = ManaList[compareRow][6][0];
							ManaList[compareRow][6][0] = obj;
						}
					}
				}
			}
		}
	}
}
