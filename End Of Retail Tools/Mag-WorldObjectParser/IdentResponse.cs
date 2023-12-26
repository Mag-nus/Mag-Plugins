using System;
using System.Collections.Generic;
using System.Text.Json;

using Mag.Shared;
using Mag.Shared.Constants;

namespace Mag_WorldObjectParser
{
    class IdentResponse : LogItem
    {
        public int Id;

        public ObjectClass ObjectClass;

        public Dictionary<int, bool> BoolValues = new Dictionary<int, bool>();
        public Dictionary<int, double> DoubleValues = new Dictionary<int, double>();
        public Dictionary<IntValueKey, int> LongValues = new Dictionary<IntValueKey, int>();
        public Dictionary<int, string> StringValues = new Dictionary<int, string>();

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
						Id = int.Parse(kvp.Value.ToString(), System.Globalization.NumberStyles.AllowLeadingSign);
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
                                var key = int.Parse(kvp2.Key);

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
    }
}
