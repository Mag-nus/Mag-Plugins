using System;
using System.Windows.Forms;

using Mag.Shared;

using Decal.Adapter;
using Decal.Adapter.Wrappers;

namespace MagTools.Macros
{
	class IdleActionManager : IDisposable
	{
		readonly Timer timer = new Timer();

		public IdleActionManager()
		{
			try
			{
				timer.Tick += new EventHandler(timer_Tick);
				timer.Interval = 4000;

				CoreManager.Current.WorldFilter.CreateObject += new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
				CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
				CoreManager.Current.CharacterFilter.Logoff += new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
				CoreManager.Current.EchoFilter.ServerDispatch += new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		private bool disposed;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!disposed)
			{
				if (disposing)
				{
					CoreManager.Current.WorldFilter.CreateObject -= new EventHandler<CreateObjectEventArgs>(WorldFilter_CreateObject);
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.CharacterFilter.Logoff -= new EventHandler<LogoffEventArgs>(CharacterFilter_Logoff);
					CoreManager.Current.EchoFilter.ServerDispatch -= new EventHandler<NetworkMessageEventArgs>(EchoFilter_ServerDispatch);
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void WorldFilter_CreateObject(object sender, CreateObjectEventArgs e)
		{
			try
			{
				if (Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value && e.New.ObjectClass == ObjectClass.Gem && e.New.Name == "Coalesced Aetheria")
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.HeartCarver.Value && e.New.ObjectClass == ObjectClass.Misc && e.New.Name.EndsWith(" Heart"))
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.ShatteredKeyFixer.Value && e.New.ObjectClass == ObjectClass.Misc && e.New.Name.StartsWith("Shattered ") && e.New.Name.EndsWith(" Key"))
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.KeyRinger.Value)
				{
					if (e.New.ObjectClass == ObjectClass.Misc && e.New.Name == "Burning Sands Keyring")
						CoreManager.Current.Actions.RequestId(e.New.Id);

					if (e.New.ObjectClass == ObjectClass.Key && e.New.Name == "Aged Legendary Key")
						timer.Start();
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value && e.Changed.ObjectClass == ObjectClass.Gem && e.Changed.Name == "Coalesced Aetheria")
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.HeartCarver.Value && e.Changed.ObjectClass == ObjectClass.Misc && e.Changed.Name.EndsWith(" Heart"))
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.ShatteredKeyFixer.Value && e.Changed.ObjectClass == ObjectClass.Misc && e.Changed.Name.StartsWith("Shattered ") && e.Changed.Name.EndsWith(" Key"))
					timer.Start();

				if (Settings.SettingsManager.InventoryManagement.KeyRinger.Value)
				{
					if (e.Changed.ObjectClass == ObjectClass.Misc && e.Changed.Name == "Burning Sands Keyring")
					{
						if (e.Change == WorldChangeType.IdentReceived)
							timer.Start();
						else
							CoreManager.Current.Actions.RequestId(e.Changed.Id);
					}

					if (e.Changed.ObjectClass == ObjectClass.Key && e.Changed.Name == "Aged Legendary Key")
						timer.Start();
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void CharacterFilter_Logoff(object sender, LogoffEventArgs e)
		{
			try
			{
				timer.Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		DateTime lastActionThatCouldRequireConfirmation;

		void timer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (CoreManager.Current.Actions.CombatMode != CombatState.Peace)
					return;

				try
				{
					if (CoreManager.Current.Actions.OpenedContainer != 0)
						return;
				} 
				/*System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt.
					at Decal.Interop.Core.ACHooksClass.get_OpenedContainer()
					at Decal.Adapter.Wrappers.HooksWrapper.get_OpenedContainer()*/
				catch (AccessViolationException)
				{
					timer.Stop();
					return;
				}

				if (CoreManager.Current.Actions.BusyState != 0)
					return;

				int toolId = 0;
				int targetId = 0;
				bool couldRequireConfirmation = false;

				if ((toolId == 0 || targetId == 0) && Settings.SettingsManager.InventoryManagement.AetheriaRevealer.Value)
				{
					toolId = 0; targetId = 0; couldRequireConfirmation = false;
					foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (wo.ObjectClass == ObjectClass.Gem && wo.Name == "Aetheria Mana Stone") toolId = wo.Id;
						if (wo.ObjectClass == ObjectClass.Gem && wo.Name == "Coalesced Aetheria") targetId = wo.Id;
					}
				}

				if ((toolId == 0 || targetId == 0) && Settings.SettingsManager.InventoryManagement.HeartCarver.Value && CoreManager.Current.CharacterFilter.Skills[CharFilterSkillType.Lockpick].Training >= TrainingType.Trained)
				{
					toolId = 0; targetId = 0; couldRequireConfirmation = true;
					foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (wo.ObjectClass == ObjectClass.Misc && wo.Name == "Intricate Carving Tool") toolId = wo.Id;
						if (wo.ObjectClass == ObjectClass.Misc && wo.Name.EndsWith(" Heart")) targetId = wo.Id;
					}
				}

				if ((toolId == 0 || targetId == 0) && Settings.SettingsManager.InventoryManagement.ShatteredKeyFixer.Value && CoreManager.Current.CharacterFilter.Skills[CharFilterSkillType.Lockpick].Training >= TrainingType.Trained)
				{
					toolId = 0; targetId = 0; couldRequireConfirmation = true;
					foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
					{
						if (wo.ObjectClass == ObjectClass.Misc && wo.Name == "Intricate Carving Tool") toolId = wo.Id;
						if (wo.ObjectClass == ObjectClass.Misc && wo.Name.StartsWith("Shattered ") && wo.Name.EndsWith(" Key")) targetId = wo.Id;
					}
				}

				if ((toolId == 0 || targetId == 0) && Settings.SettingsManager.InventoryManagement.KeyRinger.Value && CoreManager.Current.Actions.OpenedContainer == 0)
				{
					// Make sure we're not by a chest
					var closestChest = Util.GetClosestObject(" Chest", true);

					if (closestChest == null || Util.GetDistanceFromPlayer(closestChest) > 10)
					{
						toolId = 0; targetId = 0; couldRequireConfirmation = false;
						WorldObject bestKeyRing = null;
						foreach (var wo in CoreManager.Current.WorldFilter.GetInventory())
						{
							if (wo.HasIdData && wo.ObjectClass == ObjectClass.Misc && wo.Name == "Burning Sands Keyring" && wo.Values(LongValueKey.UsesRemaining) > 0 && wo.Values(LongValueKey.KeysHeld) < 24)
							{
								if (bestKeyRing == null || (bestKeyRing.Values(LongValueKey.KeysHeld) < wo.Values(LongValueKey.KeysHeld)))
									bestKeyRing = wo;
								else if (bestKeyRing.Values(LongValueKey.KeysHeld) == 0 && bestKeyRing.Values(LongValueKey.UsesRemaining) > wo.Values(LongValueKey.UsesRemaining))
									bestKeyRing = wo;
							}
							if (wo.ObjectClass == ObjectClass.Key && wo.Name == "Aged Legendary Key") targetId = wo.Id;
						}
						if (bestKeyRing != null)
							toolId = bestKeyRing.Id;
					}
				}

				if (toolId != 0 && targetId != 0)
				{
					CoreManager.Current.Actions.SelectItem(targetId);
					CoreManager.Current.Actions.UseItem(toolId, 1, targetId);
					if (couldRequireConfirmation)
						lastActionThatCouldRequireConfirmation = DateTime.Now;
					return;
				}

				timer.Stop();
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void EchoFilter_ServerDispatch(object sender, NetworkMessageEventArgs e)
		{
			try
			{ 
				if (lastActionThatCouldRequireConfirmation != DateTime.MinValue && DateTime.Now - lastActionThatCouldRequireConfirmation < TimeSpan.FromSeconds(5))
				{
					if (e.Message.Type == 0xF7B0 && (int)e.Message["event"] == 0x0274 && e.Message.Value<int>("type") == 5) // 0x0274 = Confirmation Panel
					{
						lastActionThatCouldRequireConfirmation = DateTime.MinValue;
						PostMessageTools.ClickYes();
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}
