
namespace MagTools.Trackers.Combat
{
	public enum AttackDirection
	{
		Unknown,

		/// <summary>
		/// Player received the attack.
		/// </summary>
		PlayerReceived,
		
		/// <summary>
		/// Player attacked something else.
		/// </summary>
		PlayerInitiated,
	}
}
