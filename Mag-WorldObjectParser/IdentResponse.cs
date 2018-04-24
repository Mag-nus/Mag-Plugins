using System.Collections.Generic;

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
    }
}
