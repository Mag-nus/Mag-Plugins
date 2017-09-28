using System;
using System.Collections.Generic;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
    class AfterLoginCompleteMessageQueueManager
    {
        bool freshLogin;

        LoginCommands _loginCmds = new LoginCommands();
        bool sendingLastEnter;

        DateTime loginCompleteTime = DateTime.MaxValue;

        public void FilterCore_ClientDispatch(object sender, NetworkMessageEventArgs e)
        {
            if (e.Message.Type == 0xF7C8) // Enter Game
                freshLogin = true;

            if (freshLogin && e.Message.Type == 0xF7B1 && Convert.ToInt32(e.Message["action"]) == 0xA1) // Character Materialize (Any time is done portalling in, login or portal)
            {
                freshLogin = false;

                string characterName = GameRepo.Game.Character;
                if (string.IsNullOrEmpty(characterName))
                {
                    // Do not know why GameRepo.Game.Character is not yet populated, but it isn't
                    var launchInfo = LaunchControl.GetLaunchInfo();
                    if (launchInfo.IsValid)
                    {
                        characterName = launchInfo.CharacterName;
                    }
                }

                var persister = new LoginCommandPersister(GameRepo.Game.Account, GameRepo.Game.Server, characterName);

                log.WriteDebug("FilterCore_ClientDispatch: Character: '{0}'", GameRepo.Game.Character);

                _loginCmds = persister.ReadAndCombineQueues();

                if (_loginCmds.Commands.Count > 0)
                {
                    loginCompleteTime = DateTime.Now;

                    sendingLastEnter = false;
                    CoreManager.Current.RenderFrame += new EventHandler<EventArgs>(Current_RenderFrame);
                }
            }
        }

        void Current_RenderFrame(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Subtract(TimeSpan.FromMilliseconds(_loginCmds.WaitMillisencds)) < loginCompleteTime)
                    return;

                if (_loginCmds.Commands.Count == 0 && sendingLastEnter == false)
                {
                    CoreManager.Current.RenderFrame -= new EventHandler<EventArgs>(Current_RenderFrame);
                    return;
                }

                bool useMagToolsStyle = true;

                if (useMagToolsStyle)
                {
                    string cmd = _loginCmds.Commands.Dequeue();
                    DecalProxy.DispatchChatToBoxWithPluginIntercept(cmd);
                }
                else
                {
                    if (sendingLastEnter)
                    {
                        PostMessageTools.SendEnter();
                        sendingLastEnter = false;
                    }
                    else
                    {
                        PostMessageTools.SendEnter();
                        string cmd = _loginCmds.Commands.Dequeue();
                        // The game is losing the first character of our commands
                        // So deliberately send a space at the start
                        if (!cmd.StartsWith(" "))
                        {
                            cmd = " " + cmd;
                        }
                        PostMessageTools.SendCharString(cmd);
                        sendingLastEnter = true;
                    }
                }
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }

        private string TextRemainder(string text, string prefix)
        {
            if (text.Length <= prefix.Length) { return string.Empty; }
            return text.Substring(prefix.Length);
        }
        public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
        {
            bool writeChanges = true;
            bool global = false;
            string cmdtext = e.Text;
            if (cmdtext.Contains("/mfglobal"))
            {
                cmdtext = cmdtext.Replace(" /mfglobal", " /mf");
                cmdtext = cmdtext.Replace("/mfglobal ", "/mf ");
                cmdtext = cmdtext.Replace("/mfglobal", "/mf");
                global = true;
            }
            if (cmdtext.StartsWith("/mf log "))
            {
                string logmsg = TextRemainder(cmdtext, "/mf log ");
                log.WriteInfo(logmsg);

                e.Eat = true;
            }
            else if (cmdtext.StartsWith("/mf alcmq add ") || cmdtext.StartsWith("/mf olcmq add "))
            {
                string cmd = TextRemainder(cmdtext, "/mf alcmq add ");
                _loginCmds.Commands.Enqueue(cmd);
                Debug.WriteToChat("After Login Complete Message Queue added: " + cmd);

                e.Eat = true;
            }
            else if (cmdtext == "/mf alcmq clear" || cmdtext == "/mf olcmq clear")
            {
                _loginCmds.Commands.Clear();
                Debug.WriteToChat("After Login Complete Message Queue cleared");

                e.Eat = true;
            }
            else if (cmdtext.StartsWith("/mf alcmq wait set "))
            {
                string valstr = TextRemainder(cmdtext, "/mf alcmq wait set ");
                _loginCmds.WaitMillisencds = int.Parse(valstr);
                Debug.WriteToChat("After Login Complete Message Queue Wait time set: " + valstr + "ms");

                e.Eat = true;
            }
            else if (cmdtext.StartsWith("/mf olcwait set ")) // Backwards Compatability
            {
                string valstr = TextRemainder(cmdtext, "/mf olcwait set ");
                _loginCmds.WaitMillisencds = int.Parse(valstr);
                Debug.WriteToChat("After Login Complete Message Queue Wait time set: " + valstr + "ms");

                e.Eat = true;
            }
            else if (cmdtext == "/mf alcmq wait clear" || cmdtext == "/mf olcwait clear")
            {
                _loginCmds.ClearWait();
                Debug.WriteToChat(string.Format("After Login Complete Wait time reset to default {0} ms", LoginCommands.DefaultMillisecondsToWaitAfterLoginComplete));

                e.Eat = true;
            }
            else if (cmdtext == "/mf alcmq show" || cmdtext == "/mf olcmq show" || cmdtext == "/mf alcmq list" || cmdtext == "/mf olcmq list")
            {
                var rdr = new LoginCommandPersister(GameRepo.Game.Account, GameRepo.Game.Server, GameRepo.Game.Character);
                var queue = rdr.ReadQueue(global);
                Debug.WriteToChat(string.Format("LoginCmds: {0}", queue.Commands.Count));
                foreach (string cmd in queue.Commands)
                {
                    Debug.WriteToChat(string.Format("cmd: {0}", cmd));
                }
                Debug.WriteToChat(string.Format("Wait: {0}", queue.WaitMillisencds));
                e.Eat = true;
                writeChanges = false;
            }
            if (e.Eat && writeChanges)
            {
                var persister = new LoginCommandPersister(GameRepo.Game.Account, GameRepo.Game.Server, GameRepo.Game.Character);
                persister.WriteQueue(_loginCmds, global);
            }
        }
    }
}