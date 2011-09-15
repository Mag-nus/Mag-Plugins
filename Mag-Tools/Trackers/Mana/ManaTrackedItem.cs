using System;
using System.Collections.Generic;

using Decal.Adapter;
using Decal.Adapter.Wrappers;
using Decal.Filters;

namespace MagTools.Trackers.Mana
{
	public class ManaTrackedItem : IDisposable
	{
		/// <summary>
		/// This is raised when an item the tracker is watching has been changed.
		/// </summary>
		public event Action<ManaTrackedItem> Changed;

		private readonly PluginHost Host;

		public readonly int Id;

		private DateTime timeOfLastManaIdent = DateTime.MinValue;

		private System.Windows.Forms.Timer burnTimer = new System.Windows.Forms.Timer();

		public ManaTrackedItem(PluginHost host, int id)
		{
			this.Host = host;

			this.Id = id;

			WorldObject wo = CoreManager.Current.WorldFilter[Id];

			if (wo == null)
				return;

			Host.Actions.RequestId(Id);

			CoreManager.Current.WorldFilter.ChangeObject += new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
			CoreManager.Current.ChatBoxMessage += new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);

			burnTimer.Tick += new EventHandler(burnTimer_Tick);
		}

		private bool _disposed = false;

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
			if (!_disposed)
			{
				if (disposing)
				{
					CoreManager.Current.WorldFilter.ChangeObject -= new EventHandler<ChangeObjectEventArgs>(WorldFilter_ChangeObject);
					CoreManager.Current.ChatBoxMessage -= new EventHandler<ChatTextInterceptEventArgs>(Current_ChatBoxMessage);

					burnTimer.Tick -= new EventHandler(burnTimer_Tick);
					burnTimer.Dispose();
				}

				// Indicate that the instance has been disposed.
				_disposed = true;
			}
		}

		void burnTimer_Tick(object sender, EventArgs e)
		{
			try
			{
				if (Changed != null)
					Changed(this);
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void WorldFilter_ChangeObject(object sender, ChangeObjectEventArgs e)
		{
			try
			{
				if (e.Changed.Id != Id)
					return;

				if (e.Change == WorldChangeType.IdentReceived && ItemState == ManaTrackedItemState.Active)
				{
					burnTimer.Interval = (int)(SecondsPerBurn * 1000);
					burnTimer.Start();
				}

				// Change object between IdentReceived and ManaChange will take some explaning.
				// It clarifies why we store timeOfLastManaIdent instead of just using wo.LastIdTime.
				// When an IdentReceived is received on an item, it updates the LastIdTime as well as the LongValueKey.CurrentMana value.
				// However, when ManaChange is received, LongValueKey.CurrentMana is updated but LastIdTime is not.
				// Because of this, if we received a ManaChange and used LastIdTime to recalculate against LongValueKey.CurrentMana, it would obviously be off.
				// So, we simply cache the time we received an ident OR a mana change, that way we have the time our LongValueKey.CurrentMana was last updated,
				// and for our calculations, thats all we need.
				if (e.Change == WorldChangeType.IdentReceived || e.Change == WorldChangeType.ManaChange)
				{
					timeOfLastManaIdent = DateTime.UtcNow;

					if (Changed != null)
						Changed(this);
				}

			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		void Current_ChatBoxMessage(object sender, ChatTextInterceptEventArgs e)
		{
			try
			{
				if (e.Text.StartsWith("You say, ") || e.Text.Contains("says, \""))
					return;

				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null)
					return;

				// This is triggered when an item has 2 minutes left of mana.
				// Your Bronze Haebrean Breastplate is low on Mana.
				if (e.Text.Contains("Your ") && e.Text.Contains(" is low on Mana."))
				{
					// Fire refill event here
				}

				// The Mana Stone is destroyed.
				// The Mana Stone gives 11,376 points of mana to the following items: Satin Flared Shirt, Steel Chainmail Bracers, Velvet Trousers, Leather Loafers, Copper Chainmail Greaves, Enhanced White Empyrean Ring, Enhanced Red Empyrean Ring, Iron Diforsa Pauldrons, Bronze Chainmail Tassets, Copper Heavy Bracelet, Silver Olthoi Amuli Gauntlets, Ivory Heavy Bracelet, Steel Coronet, Emerald Amulet, Silver Puzzle Box, Sunstone Fire Sceptre
				// Your items are fully charged.
				if (e.Text.Contains("The Mana Stone gives "))
				{
					if (e.Text.Contains(wo.Name))
						Host.Actions.RequestId(Id);
				}

				// Your Bronze Haebrean Breastplate is out of Mana.
				if (e.Text.Contains("Your ") && e.Text.Contains(" is out of Mana."))
				{
					if (e.Text.Contains(wo.Name))
						Host.Actions.RequestId(Id);
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}

		public int MaximumMana
		{
			get
			{
				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null)
					return 0;

				return wo.Values(LongValueKey.MaximumMana, 0);
			}
		}

		private DateTime lastIdTime = DateTime.MinValue;
		private ManaTrackedItemState itemState = ManaTrackedItemState.Unknown;

		public ManaTrackedItemState ItemState
		{
			get
			{
				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null)
					return ManaTrackedItemState.Unknown;

				if (lastIdTime != timeOfLastManaIdent || lastIdTime == DateTime.MinValue)
				{
					lastIdTime = timeOfLastManaIdent;
					itemState = RecaclulteState(wo);
				}

				return itemState;
			}
		}

		private ManaTrackedItemState RecaclulteState(WorldObject wo)
		{
			// We need basic IdData to determine if an item is active
			if (!wo.HasIdData)
				return ManaTrackedItemState.Unknown;

			// If this item has no spells, its not activateable
			if (wo.Values(LongValueKey.SpellCount) == 0 || wo.Values(LongValueKey.MaximumMana) == 0)
				return ManaTrackedItemState.NotActivatable;

			// If this item has no mana in it, it's not active
			if (wo.Values(LongValueKey.CurrentMana, 0) == 0)
				return ManaTrackedItemState.NotActive;


			// Go through and find all of our current active spells (enchantments)
			List<int> activeSpellsOnChar = new List<int>();

			foreach (EnchantmentWrapper wrapper in CoreManager.Current.CharacterFilter.Enchantments)
			{
				// Only add ones that are cast by items (have no duration)
				if (wrapper.TimeRemaining <= 0)
					activeSpellsOnChar.Add(wrapper.SpellId);
			}

			FileService service = CoreManager.Current.Filter<FileService>();


			// Lets check if the item is not active We check to see if this item has any spells that are not activated.
			bool inactiveSpellFound = false;

			// Go through all of this items spells to determine if all are active.
			for (int i = 0 ; i < wo.Values(LongValueKey.SpellCount) ; i++)
			{
				int spellOnItemId = wo.Spell(i);

				if (wo.Exists(LongValueKey.AssociatedSpell) && (wo.Values(LongValueKey.AssociatedSpell) == spellOnItemId))
					continue;

				Spell spellOnItem = service.SpellTable.GetById(spellOnItemId);

				// If it is offensive, it is probably a cast on strike spell
				if (spellOnItem.IsDebuff || spellOnItem.IsOffensive)
					continue;


				// Check if this particular spell is active
				bool thisSpellIsActive = false;


				// Check to see if this item cast any spells on itself.
				for (int j = 0 ; j < wo.Values(LongValueKey.ActiveSpellCount) ; j++)
				{
					int activeSpellOnItemId = wo.ActiveSpell(j);

					if ((service.SpellTable.GetById(activeSpellOnItemId).Family == spellOnItem.Family) && (service.SpellTable.GetById(activeSpellOnItemId).Difficulty >= spellOnItem.Difficulty))
					{
						thisSpellIsActive = true;
						break;
					}
				}

				if (thisSpellIsActive)
					continue;


				// Check to see if this item cast any spells on the player.
				foreach (int j in activeSpellsOnChar)
				{
					if (service.SpellTable.GetById(j) != null && (service.SpellTable.GetById(j).Family == spellOnItem.Family) && (service.SpellTable.GetById(j).Difficulty >= spellOnItem.Difficulty))
					{
						thisSpellIsActive = true;
						break;
					}
				}

				if (thisSpellIsActive)
					continue;

				// This item has not cast this particular spell.
				inactiveSpellFound = true;
				break;
			}


			if (inactiveSpellFound)
				return ManaTrackedItemState.NotActive;

			return ManaTrackedItemState.Active;
		}

		private double SecondsPerBurn
		{
			get
			{
				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null || !wo.HasIdData)
					return 0;

				// This doesn't take into account any luminance augs. It should, so that fix would go here.

				// Items can burn mana in 5 sec increments. If it says 1 every 18 sec, its really 1 every 20.
				return ((int)Math.Ceiling(-0.2 / wo.Values(DoubleValueKey.ManaRateOfChange))) * 5;
			}
		}

		/// <summary>
		/// This will factor in item.Values(DoubleValueKey.ManaRateOfChange) with item.Values(LongValueKey.CurrentMana) and item.LastIdTime to come up with how much
		/// mana this item should have right now.
		/// </summary>
		public int CalculatedCurrentMana
		{
			get
			{
				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null || ItemState == ManaTrackedItemState.Unknown || ItemState == ManaTrackedItemState.NotActivatable)
					return 0;

				if (ItemState == ManaTrackedItemState.NotActive)
					return wo.Values(LongValueKey.CurrentMana);

				int burnedMana = 0;

				//DateTime utcLastIdTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(wo.LastIdTime);
				TimeSpan timeSinceLastIdent = DateTime.UtcNow - timeOfLastManaIdent;

				// Calculate how much mana has been burned off since we last identified the object.
				burnedMana = (int)(timeSinceLastIdent.TotalSeconds / SecondsPerBurn);

				if (burnedMana > wo.Values(LongValueKey.CurrentMana))
					burnedMana = wo.Values(LongValueKey.CurrentMana);

				return Math.Max(wo.Values(LongValueKey.CurrentMana) - burnedMana, 0);
			}
		}

		public int ManaNeededToRefill
		{
			get
			{
				return Math.Max(MaximumMana - CalculatedCurrentMana, 0);
			}
		}

		/// <summary>
		/// This will return the amount of time that the CurrentMana will keep this item activated.
		/// If the item is not active, it will return a timespan of 0.
		/// </summary>
		public TimeSpan ManaTimeRemaining
		{
			get
			{
				WorldObject wo = CoreManager.Current.WorldFilter[Id];

				if (wo == null || ItemState != ManaTrackedItemState.Active)
					return new TimeSpan();

				return TimeSpan.FromSeconds(CalculatedCurrentMana * SecondsPerBurn);
			}
		}
	}
}
