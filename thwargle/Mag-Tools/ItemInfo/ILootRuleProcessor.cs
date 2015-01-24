
namespace MagTools.ItemInfo
{
	interface ILootRuleProcessor
	{
		bool GetLootRuleInfoFromItemInfo(ItemInfoIdentArgs itemInfoIdentArgs, ItemInfoCallback itemInfoCallBack);
	}
}
