
namespace MagFilter
{
	internal struct DefaultFirstCharacter
	{
		public string Server;
		public string ZoneId;

		public string CharacterName;

		public DefaultFirstCharacter(string server, string zoneId, string characterName)
		{
			Server = server;
			ZoneId = zoneId;
			CharacterName = characterName;
		}
	}
}
