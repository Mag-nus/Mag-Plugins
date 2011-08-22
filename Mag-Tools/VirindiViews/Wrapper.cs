///////////////////////////////////////////////////////////////////////////////
//File: Wrapper.cs
//
//Description: Contains the interface definitions for the MetaViewWrappers classes.
//
//References required:
//  System.Drawing
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

#if METAVIEW_PUBLIC_NS
namespace MetaViewWrappers
#else
namespace MyClasses.MetaViewWrappers
#endif
{
#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    delegate void dClickedList(object sender, int row, int col);

    
    #region EventArgs Classes

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVControlEventArgs : EventArgs
    {
        private int id;

        internal MVControlEventArgs(int ID)
        {
            this.id = ID;
        }

        public int Id
        {
            get { return this.id; }
        }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVIndexChangeEventArgs : MVControlEventArgs
    {
        private int index;

        internal MVIndexChangeEventArgs(int ID, int Index)
            : base(ID)
        {
            this.index = Index;
        }

        public int Index
        {
            get { return this.index; }
        }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVListSelectEventArgs : MVControlEventArgs
    {
        private int row;
        private int col;

        internal MVListSelectEventArgs(int ID, int Row, int Column)
            : base(ID)
        {
            this.row = Row;
            this.col = Column;
        }

        public int Row
        {
            get { return this.row; }
        }

        public int Column
        {
            get { return this.col; }
        }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVCheckBoxChangeEventArgs : MVControlEventArgs
    {
        private bool check;

        internal MVCheckBoxChangeEventArgs(int ID, bool Check)
            : base(ID)
        {
            this.check = Check;
        }

        public bool Checked
        {
            get { return this.check; }
        }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVTextBoxChangeEventArgs : MVControlEventArgs
    {
        private string text;

        internal MVTextBoxChangeEventArgs(int ID, string text)
            : base(ID)
        {
            this.text = text;
        }

        public string Text
        {
            get { return this.text; }
        }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    class MVTextBoxEndEventArgs : MVControlEventArgs
    {
        private bool success;

        internal MVTextBoxEndEventArgs(int ID, bool success)
            : base(ID)
        {
            this.success = success;
        }

        public bool Success
        {
            get { return this.success; }
        }
    }

    #endregion EventArgs Classes


    #region View

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IView: IDisposable
    {
        void Initialize(Decal.Adapter.Wrappers.PluginHost p, string pXML);
        void InitializeRawXML(Decal.Adapter.Wrappers.PluginHost p, string pXML);
        void Initialize(Decal.Adapter.Wrappers.PluginHost p, string pXML, string pWindowKey);
        void InitializeRawXML(Decal.Adapter.Wrappers.PluginHost p, string pXML, string pWindowKey);

        void SetIcon(int icon, int iconlibrary);
        void SetIcon(int portalicon);

        string Title { get; set; }
        bool Visible { get; set; }
#if !VVS_WRAPPERS_PUBLIC
        ViewSystemSelector.eViewSystem ViewType { get; }
#endif

        System.Drawing.Point Location { get; set; }
        System.Drawing.Rectangle Position { get; set; }
        System.Drawing.Size Size { get; }

        IControl this[string id] { get; }

        void Activate();
        void Deactivate();
        bool Activated { get; set; }
    }

    #endregion View

    #region Controls

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IControl : IDisposable
    {
        string Name { get; }
        bool Visible { get; set; }
        string TooltipText { get; set;}
        int Id { get; }
        System.Drawing.Rectangle LayoutPosition { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IButton : IControl
    {
        string Text { get; set; }
        event EventHandler Hit;
        event EventHandler<MVControlEventArgs> Click;
        System.Drawing.Color TextColor { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ICheckBox : IControl
    {
        string Text { get; set; }
        bool Checked { get; set; }
        event EventHandler<MVCheckBoxChangeEventArgs> Change;
        event EventHandler Change_Old;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ITextBox : IControl
    {
        string Text { get; set; }
        event EventHandler<MVTextBoxChangeEventArgs> Change;
        event EventHandler Change_Old;
        event EventHandler<MVTextBoxEndEventArgs> End;
        int Caret { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ICombo : IControl
    {
        IComboIndexer Text { get; }
        IComboDataIndexer Data { get; }
        int Count { get; }
        int Selected { get; set; }
        event EventHandler<MVIndexChangeEventArgs> Change;
        event EventHandler Change_Old;
        void Add(string text);
        void Add(string text, object obj);
        void Insert(int index, string text);
        void RemoveAt(int index);
        void Remove(int index);
        void Clear();
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IComboIndexer
    {
        string this[int index] { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
 interface IComboDataIndexer
    {
        object this[int index] { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface ISlider : IControl
    {
        int Position { get; set; }
        event EventHandler<MVIndexChangeEventArgs> Change;
        event EventHandler Change_Old;
        int Maximum { get; set; }
        int Minimum { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IList : IControl
    {
        event EventHandler<MVListSelectEventArgs> Selected;
        event dClickedList Click;
        void Clear();
        IListRow this[int row] { get; }
        IListRow AddRow();
        IListRow Add();
        IListRow InsertRow(int pos);
        IListRow Insert(int pos);
        int RowCount { get; }
        void RemoveRow(int index);
        void Delete(int index);
        int ColCount { get; }
        int ScrollPosition { get; set;}
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IListRow
    {
        IListCell this[int col] { get; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IListCell
    {
        System.Drawing.Color Color { get; set; }
        int Width { get; set; }
        object this[int subval] { get; set; }
        void ResetColor();
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IStaticText : IControl
    {
        string Text { get; set; }
        event EventHandler<MVControlEventArgs> Click;
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface INotebook : IControl
    {
        event EventHandler<MVIndexChangeEventArgs> Change;
        int ActiveTab { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IProgressBar : IControl
    {
        int Position { get; set; }
        int Value { get; set; }
        string PreText { get; set; }
        int MaxValue { get; set; }
    }

#if VVS_WRAPPERS_PUBLIC
    public
#else
    internal
#endif
    interface IImageButton : IControl
    {
        event EventHandler<MVControlEventArgs> Click;
        void SetImages(int unpressed, int pressed);
        void SetImages(int hmodule, int unpressed, int pressed);
        int Background { set; }
        System.Drawing.Color Matte { set; }
    }

    #endregion Controls
}
