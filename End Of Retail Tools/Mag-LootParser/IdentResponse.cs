using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using Mag.Shared;
using Mag.Shared.Constants;

namespace Mag_LootParser
{
    class IdentResponse : LogItem
    {
        public uint Id;

        public ObjectClass ObjectClass;

        public Dictionary<int, bool> BoolValues = new Dictionary<int, bool>();
        public Dictionary<int, double> DoubleValues = new Dictionary<int, double>();
        public Dictionary<IntValueKey, int> LongValues = new Dictionary<IntValueKey, int>();
        public Dictionary<StringValueKey, string> StringValues = new Dictionary<StringValueKey, string>();

        public List<int> ActiveSpells = new List<int>();
        public List<int> Spells = new List<int>();

        /// <summary>
        /// Null if not present
        /// </summary>
        public ExtendIDAttributeInfo ExtendIDAttributeInfo;

        public Dictionary<int, int> Resources = new Dictionary<int, int>();


        public void ParseFromDictionary(IDictionary<string, object?> result)
        {
            foreach (var kvp in result)
            {
                switch (kvp.Key)
                {
                    case "Id":
						Id = (uint)int.Parse(kvp.Value.ToString(), System.Globalization.NumberStyles.AllowLeadingSign);
                        break;

                    case "ObjectClass":
                        ObjectClass = (ObjectClass)Enum.Parse(typeof(ObjectClass), kvp.Value.ToString());
                        break;

                    case "BoolValues":
                        {
							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
									continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

							foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = bool.Parse(kvp2.Value.ToString());

                                BoolValues[key] = value;
                            }
                        }

						break;

					case "DoubleValues":
                        {
							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
								continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

                            foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = double.Parse(kvp2.Value.ToString());

                                DoubleValues[key] = value;
                            }
                        }

						break;

					case "LongValues":
                        {
							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
								continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

							foreach (var kvp2 in values)
                            {
                                var key = (IntValueKey)int.Parse(kvp2.Key);
                                var value = int.Parse(kvp2.Value.ToString());

                                LongValues[key] = value;
                            }
                        }

						break;

					case "StringValues":
                        {
							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
								continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

							foreach (var kvp2 in values)
                            {
                                var key = (StringValueKey)int.Parse(kvp2.Key);

                                StringValues[key] = kvp2.Value.ToString();
                            }
                        }

						break;

					case "ActiveSpells":
						{
							var valueAsString = kvp.Value.ToString();

							if (!string.IsNullOrEmpty(valueAsString))
							{
								var spellsSplit = valueAsString.Split(',');

								foreach (var spell in spellsSplit)
									ActiveSpells.Add(int.Parse(spell));
							}
						}

                        break;

                    case "Spells":
						{
							var valueAsString = kvp.Value.ToString();

							if (!string.IsNullOrEmpty(valueAsString))
							{
								var spellsSplit = valueAsString.Split(',');

								foreach (var spell in spellsSplit)
									Spells.Add(int.Parse(spell));
							}
						}

						break;

					case "Attributes":
                        {
                            ExtendIDAttributeInfo = new ExtendIDAttributeInfo();

							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
								continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

                            foreach (var kvp2 in values)
                            {
                                switch (kvp2.Key)
                                {
                                    case "healthMax":
                                        ExtendIDAttributeInfo.healthMax = uint.Parse(kvp2.Value);
                                        break;

                                    case "manaMax":
                                        ExtendIDAttributeInfo.manaMax = uint.Parse(kvp2.Value);
                                        break;

                                    case "staminaMax":
                                        ExtendIDAttributeInfo.staminaMax = uint.Parse(kvp2.Value);
                                        break;

                                    case "strength":
                                        ExtendIDAttributeInfo.strength = uint.Parse(kvp2.Value);
                                        break;

                                    case "endurance":
                                        ExtendIDAttributeInfo.endurance = uint.Parse(kvp2.Value);
                                        break;

                                    case "quickness":
                                        ExtendIDAttributeInfo.quickness = uint.Parse(kvp2.Value);
                                        break;

                                    case "coordination":
                                        ExtendIDAttributeInfo.coordination = uint.Parse(kvp2.Value);
                                        break;

                                    case "focus":
                                        ExtendIDAttributeInfo.focus = uint.Parse(kvp2.Value);
                                        break;

                                    case "self":
                                        ExtendIDAttributeInfo.self = uint.Parse(kvp2.Value);
                                        break;

                                    default:
                                        throw new NotImplementedException();
                                }
                            }
						}

						break;

					case "Resources":
                        {
							var valueAsString = kvp.Value.ToString();

							if (valueAsString == "{}")
								continue;

							var values = JsonSerializer.Deserialize<Dictionary<string, string>>(valueAsString);

							foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = int.Parse(kvp2.Value.ToString());

                                Resources[key] = value;
                            }
                        }

						break;

					default:
                        throw new NotImplementedException();
                }
            }
        }


        public void ParseFromBiota(ACE.Entity.Models.Biota biota)
        {
	        Id = biota.Id;

	        LongValues[IntValueKey.WeenieClassId_Decal] = (int)biota.WeenieClassId;

	        ObjectClass = ObjectClassTools.FromWeenieType((ItemType)biota.PropertiesInt.FirstOrDefault(r => r.Key == ACE.Entity.Enum.Properties.PropertyInt.ItemType).Value, (WeenieType)biota.WeenieType);

			if (biota.PropertiesBool != null)
			{
				foreach (var property in biota.PropertiesBool)
					BoolValues[(int) property.Key] = property.Value;
			}

			if (biota.PropertiesFloat != null)
			{
				foreach (var property in biota.PropertiesFloat)
					DoubleValues[(int) property.Key] = property.Value;
			}

			if (biota.PropertiesInt != null)
			{
				foreach (var property in biota.PropertiesInt)
					LongValues[(IntValueKey) property.Key] = property.Value;
			}

			// Int64

			// IID

			// DID
			var iconOverlay = biota.PropertiesDID.FirstOrDefault(r => r.Key == ACE.Entity.Enum.Properties.PropertyDataId.IconOverlay);
			if (iconOverlay.Key != 0)
				LongValues[IntValueKey.IconOverlay_Decal_DID] = (int)(iconOverlay.Value & 0xFFFF);

			if (biota.PropertiesString != null)
			{
				foreach (var property in biota.PropertiesString)
					StringValues[(StringValueKey) property.Key] = property.Value;
			}

			if (biota.PropertiesSpellBook != null)
			{
				foreach (var spell in biota.PropertiesSpellBook)
					Spells.Add(spell.Key);
			}
        }
	}
}
