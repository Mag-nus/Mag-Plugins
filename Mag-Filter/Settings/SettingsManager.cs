using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

using Mag.Shared.Settings;

namespace MagFilter.Settings
{
	static class SettingsManager
	{
		public static class CharacterSelectionScreen
		{
			public static readonly Setting<uint> MaxFPS = new Setting<uint>("CharacterSelectionScreen/MaxFPS");

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
								character.AccountName = childNode.Attributes["AccountName"].Value;

								character.CharacterName = childNode.Attributes["CharacterName"].Value;
								if (childNode.Attributes["CharacterIndex"] != null)
									int.TryParse(childNode.Attributes["CharacterIndex"].Value, out character.CharacterIndex);
								else
									character.CharacterIndex = -1;

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
					if (characters[i].Server == newDefaultFirstCharacter.Server && characters[i].AccountName == newDefaultFirstCharacter.AccountName)
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
					attributes.Add("AccountName", character.AccountName);

					attributes.Add("CharacterName", character.CharacterName);
					attributes.Add("CharacterIndex", character.CharacterIndex.ToString());

					collection.Add(attributes);
				}

				Dictionary<string, string> newAttributes = new Dictionary<string, string>();

				newAttributes.Add("Server", newDefaultFirstCharacter.Server);
				newAttributes.Add("AccountName", newDefaultFirstCharacter.AccountName);

				newAttributes.Add("CharacterName", newDefaultFirstCharacter.CharacterName);
				newAttributes.Add("CharacterIndex", newDefaultFirstCharacter.CharacterIndex.ToString());

				collection.Add(newAttributes);

				SettingsFile.SetNodeChilderen("CharacterSelectionScreen/DefaultLoginChars", "DefaultLoginChar", collection);
			}

			public static void DeleteDefaultFirstCharacter(string server, string accountName)
			{
				SettingsFile.ReloadXmlDocument();

				Collection<DefaultFirstCharacter> characters = DefaultFirstCharacters;

				for (int i = 0; i < characters.Count; i++)
				{
					if (characters[i].Server == server && characters[i].AccountName == accountName)
					{
						characters.RemoveAt(i);

						Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

						foreach (DefaultFirstCharacter character in characters)
						{
							Dictionary<string, string> attributes = new Dictionary<string, string>();

							attributes.Add("Server", character.Server);
							attributes.Add("AccountName", character.AccountName);

							attributes.Add("CharacterName", character.CharacterName);
							attributes.Add("CharacterIndex", character.CharacterIndex.ToString());

							collection.Add(attributes);
						}

						SettingsFile.SetNodeChilderen("CharacterSelectionScreen/DefaultLoginChars", "DefaultLoginChar", collection);

						break;
					}
				}
			}

			public static void DeleteDefaultFirstCharacters(string server)
			{
				SettingsFile.ReloadXmlDocument();

				Collection<DefaultFirstCharacter> characters = DefaultFirstCharacters;

				for (int i = 0; i < characters.Count; i++)
				{
					if (characters[i].Server == server)
						characters.RemoveAt(i);
				}

				Collection<Dictionary<string, string>> collection = new Collection<Dictionary<string, string>>();

				foreach (DefaultFirstCharacter character in characters)
				{
					Dictionary<string, string> attributes = new Dictionary<string, string>();

					attributes.Add("Server", character.Server);
					attributes.Add("AccountName", character.AccountName);

					attributes.Add("CharacterName", character.CharacterName);
					attributes.Add("CharacterIndex", character.CharacterIndex.ToString());

					collection.Add(attributes);
				}

				SettingsFile.SetNodeChilderen("CharacterSelectionScreen/DefaultLoginChars", "DefaultLoginChar", collection);
			}
		}
	}
}
