
namespace MagFilter
{
	internal struct DefaultFirstCharacter
	{
		public string Server;
		public string AccountName;

		public string CharacterName;
		public int CharacterIndex;

		public DefaultFirstCharacter(string server, string accountName, string characterName, int characterIndex = -1)
		{
			Server = server;
			AccountName = accountName;
			CharacterName = characterName;
			CharacterIndex = characterIndex;
		}
	}
}
