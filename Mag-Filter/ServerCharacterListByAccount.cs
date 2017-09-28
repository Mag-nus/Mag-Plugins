using System;
using System.Collections.Generic;
using System.Text;

namespace MagFilter
{
    public class ServerCharacterListByAccount
    {
        public string ZoneId { get; set; }
        private List<Character> _dictionary = new List<Character>();
        public List<Character> CharacterList { get { return _dictionary; } set { _dictionary = value; }}
    }
}
