///////////////////////////////////////////////////////////////////////////////
//File: VCS_Connector.cs
//
//Description: Connector class for Virindi Chat System 5.
//
//References required:
//  VCS5
//
//This file is Copyright (c) 2013 VirindiPlugins
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Decal.Adapter;
using Decal.Adapter.Wrappers;
using System.Reflection;

namespace MyClasses
{
	static class VCS_Connector
	{
		public enum eVVSConsoleColorClass
		{
			SystemMessage = 0,
			Magic = 1,
			MyMeleeAttack = 2,
			OtherMeleeAttack = 3,
			MyTell = 4,
			OtherTell = 5,
			GlobalChat = 6,
			AllegianceChat = 7,
			FellowChat = 8,
			OpenChat = 9,
			OpenEmote = 10,
			StatusError = 11,
			StatRaised = 12,
			RareFound = 13,

			PluginMessage = 96,
			PluginError = 97,
			Link = 98,
			Unknown = 99,
		}

		public static string ThisPluginName = "???";
		public static PluginHost Host = null;

        /// <summary>
        /// A shortcut method to initialize plugin name and the PluginHost object.
        /// </summary>
        /// <param name="myhost">PluginCore.Host</param>
        /// <param name="mypluginname">The friendly name of this plugin. Used in the presets list.</param>
		public static void Initialize(PluginHost myhost, string mypluginname)
		{
			Host = myhost;
			ThisPluginName = mypluginname;
		}

        #region SendChatText

        /// <summary>
        /// Sends text as regular chat. Deprecated.
        /// </summary>
        /// <param name="host">PluginCore.Host</param>
        /// <param name="text">The chat message.</param>
        /// <param name="color">The default AC console color ID.</param>
        /// <param name="window">The default target window, 0=auto, 1=main, 2=float1</param>
        /// <param name="vvsfilteras">The VVS console control filter type.</param>
        [Obsolete]
        public static void SendChatText(PluginHost host, string text, int color, int window, eVVSConsoleColorClass vvsfilteras)
		{
			if (IsVCSPresent(host))
			{
				//Send using VCS
				Curtain_SendChatTextVCS(text, color, window);
			}
			else
			{
				//Send the old way
				host.Actions.AddChatTextRaw(text, color, window);
			}


			if (IsVirindiViewsPresent(host))
				Curtain_SendChatTextVViews(text, color, (int)vvsfilteras);
		}

		static void Curtain_SendChatTextVCS(string text, int color, int window)
		{
			VCS5.PluginCore.Instance.FilterOutputText(text, window, color);
		}

		static void Curtain_SendChatTextVViews(string text, int color, int vvsfilteras)
		{
			VirindiViewService.Controls.HudChatbox.SendChatText(text, (VirindiViewService.Controls.HudConsole.eACTextColor)color, (VirindiViewService.eConsoleColorClass)vvsfilteras);
		}

        #endregion SendChatText

        #region Sending Categorized Text

        /// <summary>
        /// Send a filtered chat message by VCS preset. Call Initialize() first, then call InitializeCategory() to
        /// create the preset, then finally call this to output text.
        /// </summary>
        /// <param name="categoryname">The preset name. Should already be initialized by InitializeCategory().</param>
        /// <param name="text">The output chat text.</param>
        /// <param name="color">The default AC console color ID.</param>
        /// <param name="windows">The default target windows, 0=auto, 1=main, 2=float1</param>
        public static void SendChatTextCategorized(string categoryname, string text, int color, params int[] windows)
		{
			if ((windows == null) || (windows.Length == 0)) windows = new int[] { 1 };

			if (IsVCSPresent(Host))
			{
				Curtain_SendChatTextCategorized(categoryname, text, color, windows);
			}
			else
			{
				foreach (int x in windows)
				{
					Host.Actions.AddChatText(text, color, x);
				}
			}

			if (IsVirindiViewsPresent(Host))
				Curtain_SendChatTextVViews(text, color, (int)eVVSConsoleColorClass.PluginMessage);
		}

		static void Curtain_SendChatTextCategorized(string categoryname, string text, int color, params int[] windows)
		{
			VCS5.Presets.FilterOutputPreset(ThisPluginName, categoryname, text, color, windows);
		}

        /// <summary>
        /// Creates a VCS preset type which can later be used for chat. Will appear in the VCS presets list. Call Initialize() first.
        /// </summary>
        /// <param name="categoryname">The preset name.</param>
        /// <param name="description">The preset description.</param>
		public static void InitializeCategory(string categoryname, string description)
		{
			if (IsVCSPresent(Host))
				Curtain_InitializeCategory(categoryname, description);
		}

		static void Curtain_InitializeCategory(string categoryname, string description)
		{
			VCS5.Presets.RegisterPreset(ThisPluginName, categoryname, description);
		}

        #endregion Sending Categorized Text

        #region VCS and VVS online checks

        static bool seenvcsassembly = false;

        public static bool IsVCSPresent(PluginHost pHost)
        {
            try
            {
                //See if VCS assembly is loaded
                if (!seenvcsassembly)
                {
                    System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (System.Reflection.Assembly a in asms)
                    {
                        AssemblyName nmm = a.GetName();
                        if ((nmm.Name == "VCS5") && (nmm.Version >= new System.Version("5.0.0.5")))
                        {
                            seenvcsassembly = true;
                            break;
                        }
                    }
                }

                if (seenvcsassembly)
                    if (Curtain_VCSInstanceEnabled())
                        return true;
            }
            catch
            {

            }

            return false;
        }

        static bool Curtain_VCSInstanceEnabled()
        {
            return VCS5.PluginCore.Running;
        }

        static bool has_cachedvvsresult = false;
        static bool cachedvvsresult = false;

		//Doh
		//Need to know about VVS to post to VVS "console" controls.
        //Since VVS is a service and can't be turned on and off at runtime, we only need to do this once.
		public static bool IsVirindiViewsPresent(PluginHost pHost)
		{
			try
			{
                if (has_cachedvvsresult) return cachedvvsresult;

				//See if VCS assembly is loaded
				System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();
				bool l = false;
				foreach (System.Reflection.Assembly a in asms)
				{
					AssemblyName nmm = a.GetName();
					if ((nmm.Name == "VirindiViewService") && (nmm.Version >= new System.Version("1.0.0.14")))
					{
						l = true;
						break;
					}
				}

				if (l)
                    if (Curtain_VirindiViewsInstanceEnabled())
                    {
                        has_cachedvvsresult = true;
                        cachedvvsresult = true;
                        return true;
                    }
			}
			catch
			{

			}

            has_cachedvvsresult = true;
            cachedvvsresult = false;
			return false;
		}

		static bool Curtain_VirindiViewsInstanceEnabled()
		{
			return VirindiViewService.Service.Running;
        }

        #endregion VCS and VVS online checks

    }
}