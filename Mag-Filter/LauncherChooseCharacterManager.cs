using System;

using Mag.Shared;

using Decal.Adapter;

namespace MagFilter
{
    class LauncherChooseCharacterManager
    {
        readonly LoginCharacterTools loginCharacterTools;

        readonly System.Windows.Forms.Timer launcherChooseCharTimer = new System.Windows.Forms.Timer();

        int state;

        string zonename;
        string server;

        public LauncherChooseCharacterManager(LoginCharacterTools loginCharacterTools)
        {
            this.loginCharacterTools = loginCharacterTools;

            launcherChooseCharTimer.Tick += new EventHandler(launcherChooseCharTimer_Tick);
            launcherChooseCharTimer.Interval = 1000;
        }

        public void FilterCore_ServerDispatch(object sender, NetworkMessageEventArgs e)
        {
            // When we login for the first time we get the following for messages in the following order

            if (e.Message.Type == 0xF658) // Character List (we get this when we log out a character as well)
            {
                zonename = Convert.ToString(e.Message["zonename"]);
            }

            if (e.Message.Type == 0xF7E1) // Server Name (we get this when we log out a character as well)
            {
                //getting the Server from the message, but then ignore it and set to the one we know works from the files
                server = Convert.ToString(e.Message["server"]);
                var launchInfo = LaunchControl.GetLaunchInfo();
                server = launchInfo.ServerName;
                log.WriteInfo("Server as retrieved from launchInfo: " + server);
            }

            // F7E5 - Unknown? (we only get this the first time we connect), E5 F7 00 00 01 00 00 00 01 00 00 00 01 00 00 00 02 00 00 00 00 00 00 00 01 00 00 00 

            if (e.Message.Type == 0xF7EA) // Unknown? (we only get this the first time we connect), EA F7 00 0
            {
                launcherChooseCharTimer.Start();
            }
        }

        public void FilterCore_CommandLineText(object sender, ChatParserInterceptEventArgs e)
        {
            string lower = e.Text.ToLower();

            if (lower.StartsWith("/mf dlc set"))
            {
                DefaultFirstCharacterLoader.SetDefaultFirstCharacter(new DefaultFirstCharacter(server, zonename, CoreManager.Current.CharacterFilter.Name));
                Debug.WriteToChat("Default Login Character set to: " + CoreManager.Current.CharacterFilter.Name);

                e.Eat = true;
            }
            else if (lower == "/mf dlc clear")
            {
                DefaultFirstCharacterLoader.DeleteDefaultFirstCharacter(server, zonename);
                Debug.WriteToChat("Default Login Character cleared");

                e.Eat = true;
            }
        }

        void launcherChooseCharTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Override - instead of using the plugin xml, use the launch file
                var launchInfo = LaunchControl.GetLaunchInfo();
                if (launchInfo.IsValid)
                {
                    TimeSpan FiveMinutes = new TimeSpan(0, 0, 5, 0);
                    if (DateTime.UtcNow - launchInfo.LaunchTime < FiveMinutes)
                    {
                        var ourCharacter = new DefaultFirstCharacter(launchInfo.ServerName, zonename, launchInfo.CharacterName);

                        if (ourCharacter.ZoneId == zonename && ourCharacter.Server == server)
                        {
                            // Bypass movies/logos
                            if (state == 1 || state == 2)
                                PostMessageTools.SendMouseClick(350, 100);

                            if (state == 3)
                            {
                                bool ok = loginCharacterTools.LoginCharacter(ourCharacter.CharacterName);
                                if (ok)
                                {
                                    Heartbeat.RecordCharacterName(ourCharacter.CharacterName);
                                }
                            }
                        }
                    }
                    else
                    {
                        log.WriteInfo("launcherChooseCharTimer_Tick: LaunchInfo too old: " + launchInfo.LaunchTime.ToString());
                    }
                }
                else
                {
                    log.WriteInfo("launcherChooseCharTimer_Tick: LaunchInfo not valid");
                }

                if (state >= 3)
                    launcherChooseCharTimer.Stop();

                state++;
            }
            catch (Exception ex) { Debug.LogException(ex); }
        }
    }
}
