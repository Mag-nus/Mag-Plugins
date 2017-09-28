using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

using Mag.Shared.Settings;

namespace MagFilter
{
	static class DefaultFirstCharacterLoader
	{
		public static Collection<DefaultFirstCharacter> DefaultFirstCharacters
		{
			get
			{
				Collection<DefaultFirstCharacter> characters = new Collection<DefaultFirstCharacter>();

				XmlNode node = SettingsFile.GetNode("CharacterSelectionScreen/DefaultLoginChars");

				if (node != null)
				{
					foreach (XmlNode childNode in node.ChildNodes)
					{
						if (childNode.Attributes != null)
						{
							DefaultFirstCharacter character;

							character.Server = childNode.Attributes["Server"].Value;
							character.ZoneId = childNode.Attributes["ZoneId"].Value;

							character.CharacterName = childNode.Attributes["CharacterName"].Value;

							characters.Add(character);
						}
					}
				}

				return characters;
			}
		}

		public static void SetDefaultFirstCharacter(DefaultFirstCharacter newDefaultFirstCharacter)
		{
			SettingsFile.ReloadXmlDocument();

			Collection<DefaultFirstCharacter> characters = DefaultFirstCharacters;

			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].Server == newDefaultFirstCharacter.Server && characters[i].ZoneId == newDefaultFirstCharacter.ZoneId)
				{
					characters.RemoveAt(i);

					break;
				}
			}

			Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

			foreach (DefaultFirstCharacter character in characters)
			{
				Dictionary<string, string> attributes = new Dictionary<string, string>();

				attributes.Add("Server", character.Server);
				attributes.Add("ZoneId", character.ZoneId);

				attributes.Add("CharacterName", character.CharacterName);

				collection.Add(attributes);
			}

			Dictionary<string, string> newAttributes = new Dictionary<string, string>();

			newAttributes.Add("Server", newDefaultFirstCharacter.Server);
			newAttributes.Add("ZoneId", newDefaultFirstCharacter.ZoneId);

			newAttributes.Add("CharacterName", newDefaultFirstCharacter.CharacterName);

			collection.Add(newAttributes);

			SettingsFile.SetNodeChildrenAndSaveToDisk("CharacterSelectionScreen/DefaultLoginChars", "DefaultLoginChar", collection);
		}

		public static void DeleteDefaultFirstCharacter(string server, string zoneId)
		{
			SettingsFile.ReloadXmlDocument();

			Collection<DefaultFirstCharacter> characters = DefaultFirstCharacters;

            int index = FindCharacterByServerAndZone(characters, server, zoneId);
            if (index == -1) { return; }

			characters.RemoveAt(index);

			Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

			foreach (DefaultFirstCharacter character in characters)
			{
				Dictionary<string, string> attributes = new Dictionary<string, string>();

				attributes.Add("Server", character.Server);
				attributes.Add("ZoneId", character.ZoneId);

				attributes.Add("CharacterName", character.CharacterName);

				collection.Add(attributes);
			}

			SettingsFile.SetNodeChildrenAndSaveToDisk("CharacterSelectionScreen/DefaultLoginChars", "DefaultLoginChar", collection);
		}
        /// <summary>
        /// Find the character specified and return its offset in collection, or -1 if not found
        /// </summary>
        private static int FindCharacterByServerAndZone(Collection<DefaultFirstCharacter> characters, string server, string zoneId)
        {
			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].Server == server && characters[i].ZoneId == zoneId)
				{
                    return i;
                }
            }
            return -1;
        }
	}
}
