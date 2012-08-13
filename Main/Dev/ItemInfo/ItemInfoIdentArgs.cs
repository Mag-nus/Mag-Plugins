using System;

using Decal.Adapter.Wrappers;

namespace MagTools.ItemInfo
{
	class ItemInfoIdentArgs : EventArgs
	{
		public readonly WorldObject IdentifiedItem;

		public readonly bool DontShowIfItemHasNoRule;

		public readonly bool DontShowIfIsSalvageRule;

		public ItemInfoIdentArgs(WorldObject identifiedItem, bool dontShowIfItemHasNoRule = false, bool dontShowIfIsSalvageRule = false)
		{
			IdentifiedItem = identifiedItem;

			DontShowIfItemHasNoRule = dontShowIfItemHasNoRule;

			DontShowIfIsSalvageRule = dontShowIfIsSalvageRule;
		}
	}
}
