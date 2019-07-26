using System;
using System.Collections.Generic;

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


        public void ParseFromDictionary(Dictionary<string, object> result)
        {
            foreach (var kvp in result)
            {
                switch (kvp.Key)
                {
                    case "Id":
                        Id = (uint)int.Parse((string)kvp.Value);
                        break;

                    case "ObjectClass":
                        ObjectClass = (ObjectClass)Enum.Parse(typeof(ObjectClass), (string)kvp.Value);
                        break;

                    case "BoolValues":
                        {
                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = bool.Parse(kvp2.Value.ToString());

                                BoolValues[key] = value;
                            }

                            break;
                        }

                    case "DoubleValues":
                        {
                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = double.Parse(kvp2.Value.ToString());

                                DoubleValues[key] = value;
                            }

                            break;
                        }

                    case "LongValues":
                        {
                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                var key = (IntValueKey)int.Parse(kvp2.Key);
                                var value = int.Parse(kvp2.Value.ToString());

                                LongValues[key] = value;
                            }

                            break;
                        }

                    case "StringValues":
                        {
                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                var key = (StringValueKey)int.Parse(kvp2.Key);

                                StringValues[key] = kvp2.Value.ToString();
                            }

                            break;
                        }

                    case "ActiveSpells":
                        if (!string.IsNullOrEmpty((string)kvp.Value))
                        {
                            var spellsSplit = ((string)kvp.Value).Split(',');

                            foreach (var spell in spellsSplit)
                                ActiveSpells.Add(int.Parse(spell));
                        }

                        break;

                    case "Spells":
                        if (!string.IsNullOrEmpty((string)kvp.Value))
                        {
                            var spellsSplit = ((string)kvp.Value).Split(',');

                            foreach (var spell in spellsSplit)
                                Spells.Add(int.Parse(spell));
                        }

                        break;

                    case "Attributes":
                        {
                            ExtendIDAttributeInfo = new ExtendIDAttributeInfo();

                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                switch (kvp2.Key)
                                {
                                    case "healthMax":
                                        ExtendIDAttributeInfo.healthMax = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "manaMax":
                                        ExtendIDAttributeInfo.manaMax = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "staminaMax":
                                        ExtendIDAttributeInfo.staminaMax = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "strength":
                                        ExtendIDAttributeInfo.strength = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "endurance":
                                        ExtendIDAttributeInfo.endurance = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "quickness":
                                        ExtendIDAttributeInfo.quickness = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "coordination":
                                        ExtendIDAttributeInfo.coordination = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "focus":
                                        ExtendIDAttributeInfo.focus = uint.Parse((string)kvp2.Value);
                                        break;

                                    case "self":
                                        ExtendIDAttributeInfo.self = uint.Parse((string)kvp2.Value);
                                        break;

                                    default:
                                        throw new NotImplementedException();
                                }
                            }

                            break;
                        }

                    case "Resources":
                        {
                            var values = (Dictionary<string, object>)kvp.Value;

                            foreach (var kvp2 in values)
                            {
                                var key = int.Parse(kvp2.Key);
                                var value = int.Parse(kvp2.Value.ToString());

                                Resources[key] = value;
                            }

                            break;
                        }

                    default:
                        throw new NotImplementedException();
                }
            }
        }


        public bool IsTrophy()
        {
            if (ObjectClass == ObjectClass.MeleeWeapon ||
                ObjectClass == ObjectClass.MissileWeapon ||
                ObjectClass == ObjectClass.WandStaffOrb ||
                ObjectClass == ObjectClass.Armor ||
                ObjectClass == ObjectClass.Clothing ||
                ObjectClass == ObjectClass.Jewelry)
            {
                if (!LongValues.ContainsKey(IntValueKey.Workmanship))
                    return true;
            }

            return false;
        }
    }
}
