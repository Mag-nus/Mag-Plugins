///////////////////////////////////////////////////////////////////////////////
//File: Wrapper_WireupHelper.cs
//
//Description: A helper utility that emulates Decal.Adapter's automagic view
//  creation and control/event wireup with the MetaViewWrappers. A separate set
//  of attributes is used.
//
//References required:
//  Wrapper.cs
//
//This file is Copyright (c) 2010 VirindiPlugins
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
using System.Reflection;

#if METAVIEW_PUBLIC_NS
namespace MetaViewWrappers
#else
namespace MyClasses.MetaViewWrappers
#endif
{
    #region Attribute Definitions

    [AttributeUsage(AttributeTargets.Class)]
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    sealed class MVWireUpControlEventsAttribute : Attribute
    {
        public MVWireUpControlEventsAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Field)]
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    sealed class MVControlReferenceAttribute : Attribute
    {
        string ctrl;

        // Summary:
        //     Construct a new ControlReference
        //
        // Parameters:
        //   control:
        //     Control to reference
        public MVControlReferenceAttribute(string control)
        {
            ctrl = control;
        }

        // Summary:
        //     The Control Name
        public string Control
        {
            get
            {
                return ctrl;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    sealed class MVControlReferenceArrayAttribute : Attribute
    {
        private System.Collections.ObjectModel.Collection<string> myControls;

        /// <summary>
        /// Constructs a new ControlReference array
        /// </summary>
        /// <param name="controls">Names of the controls to put in the array</param>
        public MVControlReferenceArrayAttribute(params string[] controls)
            : base()
        {
            this.myControls = new System.Collections.ObjectModel.Collection<string>(controls);
        }

        /// <summary>
        /// Control collection
        /// </summary>
        public System.Collections.ObjectModel.Collection<string> Controls
        {
            get
            {
                return this.myControls;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    sealed class MVViewAttribute : Attribute
    {
        string res;

        // Summary:
        //     Constructs a new view from the specified resource
        //
        // Parameters:
        //   Resource:
        //     Embedded resource path
        public MVViewAttribute(string resource)
        {
            res = resource;
        }

        // Summary:
        //     The resource to load
        public string Resource
        {
            get
            {
                return res;
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    sealed class MVControlEventAttribute : Attribute
    {
        string c;
        string e;
        // Summary:
        //     Constructs the ControlEvent
        //
        // Parameters:
        //   control:
        //     Control Name
        //
        //   controlEvent:
        //     Event to Wire
        public MVControlEventAttribute(string control, string eventName)
        {
            c = control;
            e = eventName;
        }

        // Summary:
        //     Control Name
        public string Control
        {
            get
            {
                return c;
            }
        }

        //
        // Summary:
        //     Event to Wire
        public string EventName
        {
            get
            {
                return e;
            }
        }
    }

    #endregion Attribute Definitions

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    static class MVWireupHelper
    {
        private class ViewObjectInfo
        {
            public List<MyClasses.MetaViewWrappers.IView> Views = new List<IView>();
        }
        static Dictionary<object, ViewObjectInfo> VInfo = new Dictionary<object, ViewObjectInfo>();

        public static MyClasses.MetaViewWrappers.IView GetDefaultView(object ViewObj)
        {
            if (!VInfo.ContainsKey(ViewObj))
                return null;
            if (VInfo[ViewObj].Views.Count == 0)
                return null;
            return VInfo[ViewObj].Views[0];
        }

        public static void WireupStart(object ViewObj, Decal.Adapter.Wrappers.PluginHost Host)
        {
            if (VInfo.ContainsKey(ViewObj))
                WireupEnd(ViewObj);
            ViewObjectInfo info = new ViewObjectInfo();
            VInfo[ViewObj] = info;

            Type ObjType = ViewObj.GetType();

            //Start views
            object[] viewattrs = ObjType.GetCustomAttributes(typeof(MVViewAttribute), true);
            foreach (MVViewAttribute a in viewattrs)
            {
                info.Views.Add(MyClasses.MetaViewWrappers.ViewSystemSelector.CreateViewResource(Host, a.Resource));
            }

            //Wire up control references
            foreach (FieldInfo fi in ObjType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (Attribute.IsDefined(fi, typeof(MVControlReferenceAttribute)))
                {
                    MVControlReferenceAttribute attr = (MVControlReferenceAttribute)Attribute.GetCustomAttribute(fi, typeof(MVControlReferenceAttribute));
                    MetaViewWrappers.IControl mycontrol = null;

                    //Try each view
                    foreach (MyClasses.MetaViewWrappers.IView v in info.Views)
                    {
                        try
                        {
                            mycontrol = v[attr.Control];
                        }
                        catch { }
                        if (mycontrol != null)
                            break;
                    }

                    if (mycontrol == null)
                        throw new Exception("Invalid control reference \"" + attr.Control + "\"");

                    if (!fi.FieldType.IsAssignableFrom(mycontrol.GetType()))
                        throw new Exception("Control reference \"" + attr.Control + "\" is of wrong type");

                    fi.SetValue(ViewObj, mycontrol);
                }
                else if (Attribute.IsDefined(fi, typeof(MVControlReferenceArrayAttribute)))
                {
                    MVControlReferenceArrayAttribute attr = (MVControlReferenceArrayAttribute)Attribute.GetCustomAttribute(fi, typeof(MVControlReferenceArrayAttribute));

                    //Only do the first view
                    if (info.Views.Count == 0)
                        throw new Exception("No views to which a control reference can attach");

                    Array controls = Array.CreateInstance(fi.FieldType.GetElementType(), attr.Controls.Count);

                    IView view = info.Views[0];
                    for (int i = 0; i < attr.Controls.Count; ++i)
                    {
                        controls.SetValue(view[attr.Controls[i]], i);
                    }

                    fi.SetValue(ViewObj, controls);
                }
            }

            //Wire up events
            foreach (MethodInfo mi in ObjType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (!Attribute.IsDefined(mi, typeof(MVControlEventAttribute)))
                    continue;
                Attribute[] attrs = Attribute.GetCustomAttributes(mi, typeof(MVControlEventAttribute));

                foreach (MVControlEventAttribute attr in attrs)
                {
                    MetaViewWrappers.IControl mycontrol = null;
                    //Try each view
                    foreach (MyClasses.MetaViewWrappers.IView v in info.Views)
                    {
                        try
                        {
                            mycontrol = v[attr.Control];
                        }
                        catch { }
                        if (mycontrol != null)
                            break;
                    }

                    if (mycontrol == null)
                        throw new Exception("Invalid control reference \"" + attr.Control + "\"");

                    EventInfo ei = mycontrol.GetType().GetEvent(attr.EventName);
                    ei.AddEventHandler(mycontrol, Delegate.CreateDelegate(ei.EventHandlerType, ViewObj, mi.Name));
                }
            }
        }

        public static void WireupEnd(object ViewObj)
        {
            if (!VInfo.ContainsKey(ViewObj))
                return;

            foreach (MyClasses.MetaViewWrappers.IView v in VInfo[ViewObj].Views)
                v.Dispose();

            VInfo.Remove(ViewObj);
        }
    }
}