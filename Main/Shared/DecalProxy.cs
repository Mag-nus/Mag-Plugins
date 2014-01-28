///////////////////////////////////////////////////////////////////////////////
//File: DecalProxy.cs
//
//Description: Contains reflection-based calls of newer Decal methods, so that
//  they can be easily invoked only if the currently running version contains
//  the desired feature.
//
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

//Virindi: yes, or you could just directly resize the window
//Virindi: note...
//Virindi: right now you have to call move after you call resize
//Virindi: so right now what you have to do if you use that resize is grab the location, resize, then move back to the original location

using System;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;

using Decal.Adapter;

namespace Mag.Shared
{
    internal static class DecalProxy
    {
        public enum UIElementType : int
        {
            Smartbox = 0x1000049A,
            Chat = 0x10000601,
            FloatChat1 = 0x10000505,
            FloatChat2 = 0x1000050E,
            FloatChat3 = 0x1000050F,
            FloatChat4 = 0x10000510,
            Examination = 0x100005F7,
            Vitals = 0x100005FA,
            EnvPack = 0x100005FD,
            Panels = 0x100005FF,
            TBar = 0x10000603,
            Indicators = 0x10000611,
            ProgressBar = 0x10000613,
            Combat = 0x100006B5,
            Radar = 0x100006D2,
        }


        static Version iCachedDecalVersion = null;
        static Assembly iCachedDecalAssembly = null;
        public static bool TestDecalVersion(Version v)
        {
            if (iCachedDecalVersion != null) return iCachedDecalVersion >= v;

            System.Reflection.Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach (System.Reflection.Assembly a in asms)
            {
                AssemblyName nmm = a.GetName();
                if (nmm.Name == "Decal.Adapter")
                {
                    iCachedDecalVersion = nmm.Version;
                    iCachedDecalAssembly = a;
                    return (nmm.Version >= v); 
                }
            }

            return false;
        }

        public static void Hooks_UIElementMove(Decal.Adapter.Wrappers.HooksWrapper hooks, UIElementType e, int x, int y)
        {
            //This will load the cached assembly. We don't actually care what the version is
            //since we just check below if the required members are present.
            TestDecalVersion(new Version());

            int ee = (int)e;

            Type DecalType_UIElementType = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.UIElementType");
            if (DecalType_UIElementType == null) return;
            object e_decal = Enum.ToObject(DecalType_UIElementType, ee);

            Type DecalType_HooksWrapper = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.HooksWrapper");
            if (DecalType_HooksWrapper == null) return;
            MethodInfo call = DecalType_HooksWrapper.GetMethod("UIElementMove", new Type[] { DecalType_UIElementType, typeof(int), typeof(int) });
            if (call == null) return;

            call.Invoke(hooks, new object[] { e_decal, x, y });
        }

        public static void Hooks_UIElementResize(Decal.Adapter.Wrappers.HooksWrapper hooks, UIElementType e, int width, int height)
        {
            //This will load the cached assembly. We don't actually care what the version is
            //since we just check below if the required members are present.
            TestDecalVersion(new Version());

            int ee = (int)e;

            Type DecalType_UIElementType = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.UIElementType");
            if (DecalType_UIElementType == null) return;
            object e_decal = Enum.ToObject(DecalType_UIElementType, ee);

            Type DecalType_HooksWrapper = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.HooksWrapper");
            if (DecalType_HooksWrapper == null) return;
            MethodInfo call = DecalType_HooksWrapper.GetMethod("UIElementResize", new Type[] { DecalType_UIElementType, typeof(int), typeof(int) });
            if (call == null) return;

            call.Invoke(hooks, new object[] { e_decal, width, height });
        }

        public static Rectangle Hooks_UIElementRegion(Decal.Adapter.Wrappers.HooksWrapper hooks, UIElementType e)
        {
            //This will load the cached assembly. We don't actually care what the version is
            //since we just check below if the required members are present.
            TestDecalVersion(new Version());

            int ee = (int)e;

            Type DecalType_UIElementType = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.UIElementType");
            if (DecalType_UIElementType == null) return Rectangle.Empty;
            object e_decal = Enum.ToObject(DecalType_UIElementType, ee);

            Type DecalType_HooksWrapper = iCachedDecalAssembly.GetType("Decal.Adapter.Wrappers.HooksWrapper");
            if (DecalType_HooksWrapper == null) return Rectangle.Empty;
            MethodInfo call = DecalType_HooksWrapper.GetMethod("UIElementRegion", new Type[] { DecalType_UIElementType });
            if (call == null) return Rectangle.Empty;

            return (Rectangle)call.Invoke(hooks, new object[] { e_decal });
        }


		[DllImport("Decal.dll")]
		static extern int DispatchOnChatCommand(ref IntPtr str, [MarshalAs(UnmanagedType.U4)] int target);

		static bool Decal_DispatchOnChatCommand(string cmd)
		{
			IntPtr bstr = Marshal.StringToBSTR(cmd);

			try
			{
				bool eaten = (DispatchOnChatCommand(ref bstr, 1) & 0x1) > 0;

				return eaten;
			}
			finally
			{
				Marshal.FreeBSTR(bstr);
			}
		}

		/// <summary>
		/// This will first attempt to send the messages to all plugins. If no plugins set e.Eat to true on the message, it will then simply call InvokeChatParser.
		/// </summary>
		/// <param name="cmd"></param>
		public static void DispatchChatToBoxWithPluginIntercept(string cmd)
		{
			if (!Decal_DispatchOnChatCommand(cmd))
				CoreManager.Current.Actions.InvokeChatParser(cmd);
		}
    }
}