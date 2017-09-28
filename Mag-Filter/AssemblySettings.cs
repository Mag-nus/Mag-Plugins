// Mike Woodring
// http://www.bearcanyon.com
//
using System;
using System.Reflection;
using System.Collections;
using System.Xml;
using System.Configuration;
using System.Runtime.CompilerServices;

// AssemblySettings usage:
//
// If you know the keys you're after, the following is probably
// the most convenient:
//
//      AssemblySettings settings = new AssemblySettings();
//      string someSetting1 = settings["someKey1"];
//      string someSetting2 = settings["someKey2"];
//
// If you want to enumerate over the settings (or just as an
// alternative approach), you can do this too:
//
//      IDictionary settings = AssemblySettings.GetConfig();
//
//      foreach( DictionaryEntry entry in settings )
//      {
//          // Use entry.Key or entry.Value as desired...
//      }
//
// In either of the above two scenarios, the calling assembly
// (the one that called the constructor or GetConfig) is used
// to determine what file to parse and what the name of the
// settings collection element is.  For example, if the calling
// assembly is c:\foo\bar\TestLib.dll, then the configuration file
// that's parsed is c:\foo\bar\TestLib.dll.config, and the
// configuration section that's parsed must be named <assemblySettings>.
//
// To retrieve the configuration information for an arbitrary assembly,
// use the overloaded constructor or GetConfig method that takes an
// Assembly reference as input.
//
// If your assembly is being automatically downloaded from a web
// site by an "href-exe" (an application that's run directly from a link
// on a web page), then the enclosed web.config shows the mechanism
// for allowing the AssemblySettings library to download the
// configuration files you're using for your assemblies (while not
// allowing web.config itself to be downloaded).
//
// If the assembly you are trying to use this with is installed in, and loaded
// from, the GAC then you'll need to place the config file in the GAC directory where
// the assembly is installed.  On the first release of the CLR, this directory is
// <windir>\assembly\gac\libName\verNum__pubKeyToken]]>.  For example,
// the assembly "SomeLib, Version=1.2.3.4, Culture=neutral, PublicKeyToken=abcd1234"
// would be installed to the c:\winnt\assembly\gac\SomeLib\1.2.3.4__abcd1234 diretory
// (assuming the OS is installed in c:\winnt).  For future versions of the CLR, this
// directory scheme may change, so you'll need to check the <code>CodeBase</code> property
// of a GAC-loaded assembly in the debugger to determine the correct directory location.
//

public class AssemblySettings
{
    private IDictionary settings;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public AssemblySettings()
        : this(Assembly.GetCallingAssembly())
    {
    }

    public AssemblySettings( Assembly asm )
    {
        settings = GetConfig(asm);
    }

    public string this[ string key ]
    {
        get
        {
            string settingValue = null;

            if( settings != null )
            {
                settingValue = settings[key] as string;
            }

            return(settingValue == null ? "" : settingValue);
        }
    }
    public bool ContainsKey(string key)
    {
        if (key == null || settings == null) { return false; }
        return settings.Contains(key);
    }
    public string GetValue(string key, string defval)
    {
        if (!ContainsKey(key)) { return defval; }
        return this[key];
    }

    public static IDictionary GetConfig()
    {
        return GetConfig(Assembly.GetCallingAssembly());
    }

    public static IDictionary GetConfig( Assembly asm )
    {
        // Open and parse configuration file for specified
        // assembly, returning collection to caller for future
        // use outside of this class.
        //
        try
        {
            string cfgUncPath = asm.CodeBase + ".config";
            string cfgFile = new Uri(cfgUncPath).LocalPath; // convert file:// with more forward slashes into a proper path

            const string nodeName = "assemblySettings";

            XmlDocument doc = new XmlDocument();
            if (System.IO.File.Exists(cfgFile))
            {
                doc.Load(new XmlTextReader(cfgFile));

                XmlNodeList nodes = doc.GetElementsByTagName(nodeName);

                foreach (XmlNode node in nodes)
                {
                    if (node.LocalName == nodeName)
                    {
                        DictionarySectionHandler handler = new DictionarySectionHandler();
                        return (IDictionary)handler.Create(null, null, node);
                    }
                }
            }
        }
        catch
        {
        }

        return(null);
    }
}
