using System;
using System.Text;

namespace MagFilter
{
    /// <summary>
    /// Implement singleton storage of game info
    /// Currently has no thread locks
    /// </summary>
    class GameRepo
    {
        private static GameRepo Instance = new GameRepo();
        public static GameRepo Game { get { return Instance; } }
        private string _server;
        private string _account;
        private string _character;
        public string Server { get { return _server; } }
        public string Account { get { return _account; } }
        public string Character { get { return _character; } }
        public void SetServerAccount(string server, string account)
        {
            _server = server;
            _account = account;
        }
        public void SetCharacter(string character)
        {
            _character = character;
        }
    }
}
