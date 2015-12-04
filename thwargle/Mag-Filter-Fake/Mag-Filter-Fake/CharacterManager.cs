using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace MagFilter
{
    public class CharacterManager
    {
        private Dictionary<string, ServerCharacterListByAccount> _data = null;

        private CharacterManager(Dictionary<string, ServerCharacterListByAccount> dictionary)
        {
            _data = dictionary;
        }
        private CharacterManager()
        {
            _data = new Dictionary<string, ServerCharacterListByAccount>();
        }
        public IEnumerable<string> GetKeys()
        {
            return _data.Keys;
        }
        public ServerCharacterListByAccount GetCharacters(string serverName, string accountName)
        {
            string key = GetKey(server: serverName, accountName: accountName);
            if (this._data.ContainsKey(key))
            {
                return this._data[key];
            }
            else
            {
                return null;
            }
        }
        internal ServerCharacterListByAccount GetCharacters(string key)
        {
            return this._data[key];
        }
        private static string GetKey(string server, string accountName)
        {
            return string.Format("{0}-{1}", server, accountName);
        }

        public void WriteCharacters(string server, string zonename, List<Character> characters)
        {
            var launchInfo = (new LaunchControl()).GetLaunchInfo();
            log.writeLogs(string.Format("WriteCharacters({2}) server={0}, zone={1}", server, zonename, characters.Count));
            log.writeLogs(string.Format("LaunchServer='{0}', LaunchAccount='{1}'", launchInfo.ServerName, launchInfo.AccountName));
            string key = GetKey(server: server, accountName: launchInfo.AccountName);
            log.writeLogs("Key: " + key);
            var clist = new ServerCharacterListByAccount()
                {
                    ZoneId = zonename,
                    CharacterList = characters
                };
            this._data[key] = clist;
            log.writeLogs("P22");
            string contents = JsonConvert.SerializeObject(_data, Formatting.Indented);
            log.writeLogs("P24--contents: " + contents);
            string path = FileLocations.GetCharacterFilePath();
            using (var file = new StreamWriter(path, append: false))
            {
                file.Write(contents);
            }
            log.writeLogs("P28-finished WriteCharacters");

            /*

            file.WriteLine("Server: " + server + " Zonename: " + accountName);

            foreach (Character dude in characters)
                file.WriteLine(dude.Id.ToString() + "," + dude.Name);

            file.Close();
             * */
        }

        public static CharacterManager ReadCharacters()
        {
            try
            {
                return ReadCharactersImpl();
            }
            catch (Exception exc)
            {
                log.writeLogs("ReadCharacterImpl Exception: " + exc.ToString());
                return null;
            }
        }

        private static CharacterManager ReadCharactersImpl()
        {
            string path = FileLocations.GetCharacterFilePath();

            log.writeLogs("Q22-start ReadCharactersImpl");

            if (!File.Exists(path)) { return new CharacterManager(); }
            log.writeLogs("Q24"); // TODO - delete Q log calls
            using (var file = new StreamReader(path))
            {
                string contents = file.ReadToEnd();
                var data = JsonConvert.DeserializeObject<Dictionary<string, ServerCharacterListByAccount>>(contents);
                CharacterManager charMgr = new CharacterManager(data);
                log.writeLogs("Q28 - succeeded ReadCharactersImpl");
                return charMgr;
            }
            /*
            string line = file.ReadLine();
            if (!line.Contains("Server")) { throw new Exception("bad config file");}
            while (true)
            {
                line = file.ReadLine();
                if (line == null)
                {
                    break;
                }
                string[] parts = line.Split(',');
                var dude = new Character(id: int.Parse(parts[0]), name: parts[1], timeout: 999);
                characters.Add(dude);
            }
            return characters;
             * */
        }
    }
}
