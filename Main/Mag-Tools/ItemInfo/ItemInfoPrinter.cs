using System;
using System.IO;

using Mag.Shared;

using Decal.Adapter;

namespace MagTools.ItemInfo
{
	class ItemInfoPrinter : IDisposable
	{
		readonly DetectItemsIdentifiedByUser detectItemsIdentifiedByUser = new DetectItemsIdentifiedByUser();
		readonly DetectItemsInContainers detectItemsInContainers = new DetectItemsInContainers();

		ILootRuleProcessor virindiTankLootRuleProcessor;

		public ItemInfoPrinter()
		{
			detectItemsIdentifiedByUser.ItemIdentified += new EventHandler<ItemInfoIdentArgs>(detectItemsIdentifiedByUser_ItemIdentified);
			detectItemsInContainers.ItemIdentified += new EventHandler<ItemInfoIdentArgs>(detectItemsInContainers_ItemIdentified);

			// Virindi Tank Extensions, depends on utank2-i.dll
			try
			{
				virindiTankLootRuleProcessor = new VirindiTankLootRuleProcessor();
			}
			catch (FileNotFoundException) { virindiTankLootRuleProcessor = null; /* Eat this error (Virindi Tank isn't loaded) */ }
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
					detectItemsIdentifiedByUser.ItemIdentified -= new EventHandler<ItemInfoIdentArgs>(detectItemsIdentifiedByUser_ItemIdentified);
					detectItemsInContainers.ItemIdentified -= new EventHandler<ItemInfoIdentArgs>(detectItemsInContainers_ItemIdentified);

					detectItemsIdentifiedByUser.Dispose();
					detectItemsInContainers.Dispose();
				}

				// Indicate that the instance has been disposed.
				disposed = true;
			}
		}

		void detectItemsIdentifiedByUser_ItemIdentified(object sender, ItemInfoIdentArgs e)
		{
			processDetectedItem(e, true);
		}

		void detectItemsInContainers_ItemIdentified(object sender, ItemInfoIdentArgs e)
		{
			processDetectedItem(e);
		}

		void processDetectedItem(ItemInfoIdentArgs e, bool allowAutoClipboard = false)
		{
			if (virindiTankLootRuleProcessor != null)
			{
				try
				{
					if (allowAutoClipboard && Settings.SettingsManager.ItemInfoOnIdent.AutoClipboard.Value)
					{
						if (virindiTankLootRuleProcessor.GetLootRuleInfoFromItemInfo(e, processItemInfoCallbackWithAutoClipboard))
							return;
					}
					else
					{
						if (virindiTankLootRuleProcessor.GetLootRuleInfoFromItemInfo(e, processItemInfoCallback))
							return;
					}
				}
				catch (FileNotFoundException) { virindiTankLootRuleProcessor = null; /* Eat this error (Virindi Tank isn't loaded) */ }
				catch (Exception ex) { Debug.LogException(ex); }
			}

			if (!e.DontShowIfItemHasNoRule)
			{
				ItemInfo itemInfo = new ItemInfo(e.IdentifiedItem);

				CoreManager.Current.Actions.AddChatText(itemInfo.ToString(), 14, Settings.SettingsManager.Misc.OutputTargetWindow.Value);

				if (allowAutoClipboard && Settings.SettingsManager.ItemInfoOnIdent.AutoClipboard.Value)
				{
					try
					{
						System.Windows.Forms.Clipboard.SetDataObject(itemInfo.ToString());
					}
					catch
					{
					}
				}
			}
		}

		void processItemInfoCallbackWithAutoClipboard(ItemInfoIdentArgs e, bool itemPassesLootRule, bool isSalvage, string lootRuleName)
		{
			doProcessItemInfoCallback(e, itemPassesLootRule, isSalvage, lootRuleName, true);
		}

		void processItemInfoCallback(ItemInfoIdentArgs e, bool itemPassesLootRule, bool isSalvage, string lootRuleName)
		{
			doProcessItemInfoCallback(e, itemPassesLootRule, isSalvage, lootRuleName);
		}

		void doProcessItemInfoCallback(ItemInfoIdentArgs e, bool itemPassesLootRule, bool isSalvage, string lootRuleName, bool clipboardInfo = false)
		{
			if (e.DontShowIfIsSalvageRule && isSalvage)
				return;

			if (e.DontShowIfItemHasNoRule && string.IsNullOrEmpty(lootRuleName))
				return;

			//<Tell:IIDString:221112:-2024140046>(Epics)<\\Tell>

			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(@"<Tell:IIDString:221112:" + e.IdentifiedItem.Id + @">");

			sb.Append(itemPassesLootRule ? "+" : "-");

			if (!String.IsNullOrEmpty(lootRuleName))
				sb.Append("(" + lootRuleName + ")");

			sb.Append(@"<\\Tell>");

			sb.Append(" ");

			ItemInfo itemInfo = new ItemInfo(e.IdentifiedItem);

			sb.Append(itemInfo);

			CoreManager.Current.Actions.AddChatText(sb.ToString(), 14, Settings.SettingsManager.Misc.OutputTargetWindow.Value);

			if (clipboardInfo && Settings.SettingsManager.ItemInfoOnIdent.AutoClipboard.Value)
			{
				try
				{
					System.Text.StringBuilder clipboardBuilder = new System.Text.StringBuilder();

					clipboardBuilder.Append(itemPassesLootRule ? "+" : "-");

					if (!String.IsNullOrEmpty(lootRuleName))
						clipboardBuilder.Append("(" + lootRuleName + ")");

					clipboardBuilder.Append(" ");

					clipboardBuilder.Append(itemInfo.ToString());

					System.Windows.Forms.Clipboard.SetDataObject(clipboardBuilder.ToString());
				}
				catch
				{
				}
			}
		}
	}
}
