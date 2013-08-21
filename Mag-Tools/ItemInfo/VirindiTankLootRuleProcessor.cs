using System;
using System.Collections.Generic;

using Mag.Shared;

/*
 * me: FLootPluginClassifyCallback and immediate, difference being anything other than the obv?
	V: immediate should only be called after queryneedsid and then IDing if needed
	V: and with all those you need to wait 1 frame after the packet
 * 
 * (12:25:27 AM) V: you can create something like a vtankinterface class
	(12:25:38 AM) V: and then only create an instance of it when vtank is present
	(12:25:57 AM) me: ok, i'll do that when I'm ready to distribute, for now I'd like to work on getting my ideas working
	(12:26:03 AM) V: so everytime you want to call something you check to see if that instance is null and if not you call the interface class which calls vtank
 * 
 * */

namespace MagTools.ItemInfo
{
	class VirindiTankLootRuleProcessor : ILootRuleProcessor
	{
		struct ItemWaitingForCallback
		{
			public readonly ItemInfoIdentArgs ItemInfoIdentArgs;
			public readonly ItemInfoCallback ItemInfoCallBack;

			public ItemWaitingForCallback(ItemInfoIdentArgs itemInfoIdentArgs, ItemInfoCallback itemInfoCallBack)
			{
				ItemInfoIdentArgs = itemInfoIdentArgs;
				ItemInfoCallBack = itemInfoCallBack;
			}
		}

		readonly List<ItemWaitingForCallback> itemsWaitingForCallback = new List<ItemWaitingForCallback>();

		public bool GetLootRuleInfoFromItemInfo(ItemInfoIdentArgs itemInfoIdentArgs, ItemInfoCallback itemInfoCallback)
		{
			try
			{
				if (uTank2.PluginCore.PC.FLootPluginQueryNeedsID(itemInfoIdentArgs.IdentifiedItem.Id))
				{
					// public delegate void delFLootPluginClassifyCallback(int obj, LootAction result, bool getsuccess);
					//uTank2.PluginCore.delFLootPluginClassifyCallback callback = new uTank2.PluginCore.delFLootPluginClassifyCallback(uTankCallBack);
					//uTank2.PluginCore.PC.FLootPluginClassifyCallback(itemInfoIdentArgs.IdentifiedItem.Id, callback);

					ItemWaitingForCallback itemWaitingForCallback = new ItemWaitingForCallback(itemInfoIdentArgs, itemInfoCallback);
					itemsWaitingForCallback.Add(itemWaitingForCallback);

					uTank2.PluginCore.PC.FLootPluginClassifyCallback(itemInfoIdentArgs.IdentifiedItem.Id, uTankCallBack);
				}
				else
				{
					uTank2.LootPlugins.LootAction result = uTank2.PluginCore.PC.FLootPluginClassifyImmediate(itemInfoIdentArgs.IdentifiedItem.Id);

					itemInfoCallback(itemInfoIdentArgs, !result.IsNoLoot, result.IsSalvage, result.RuleName);
				}
			}
			catch (NullReferenceException) // Virindi tank probably not loaded.
			{
				return false;
			}

			return true;
		}

		void uTankCallBack(int obj, uTank2.LootPlugins.LootAction result, bool getsuccess)
		{
			try
			{
				if (!getsuccess)
					return;

				for (int i = 0 ; i < itemsWaitingForCallback.Count ; i++)
				{
					ItemWaitingForCallback itemWaitingForCallback = itemsWaitingForCallback[i];

					if (itemWaitingForCallback.ItemInfoIdentArgs.IdentifiedItem.Id == obj)
					{
						itemsWaitingForCallback.RemoveAt(i);

						itemWaitingForCallback.ItemInfoCallBack(itemWaitingForCallback.ItemInfoIdentArgs, !result.IsNoLoot, result.IsSalvage, result.RuleName);

						break;
					}
				}
			}
			catch (Exception ex) { Debug.LogException(ex); }
		}
	}
}