
namespace MagFilter
{
	internal struct DefaultFirstCharacter
	{
		public string Server;
		public string AccountName;

		public string CharacterName;

		public DefaultFirstCharacter(string server, string accountName, string characterName)
		{
			Server = server;
			AccountName = accountName;
			CharacterName = characterName;
		}
	}
}
