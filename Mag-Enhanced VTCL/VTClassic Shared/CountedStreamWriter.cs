///////////////////////////////////////////////////////////////////////////////
//File: CountedStreamWriter.cs
//
//Description: A class derived from System.IO.StreamWriter, overriding all
//  write methods so that a written character count can be maintained.
//  This file is shared between the VTClassic Plugin and the VTClassic Editor.
//
//This file is Copyright (c) 2010 VirindiPlugins
//
//The original copy of this code can be obtained from http://www.virindi.net/repos/virindi_public
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
using System.IO;

namespace VTClassic
{
    internal class CountedStreamWriter : StreamWriter
    {
        public CountedStreamWriter(Stream u)
            : base(u)
        {

        }

        public CountedStreamWriter(string u)
            : base(u)
        {

        }

        int iCount = 0;
        public int Count { get { return iCount; } }
        public void ResetCount() { iCount = 0; }

        public override void Write(string value)
        {
            iCount += value.Length;
            base.Write(value);
        }
        public override void Write(char[] buffer)
        {
            iCount += buffer.Length;
            base.Write(buffer);
        }
        public override void Write(char[] buffer, int index, int count)
        {
            iCount += count;
            base.Write(buffer, index, count);
        }
        public override void WriteLine()
        {
            Write(NewLine);
        }
        public override void WriteLine(string value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(char[] buffer)
        {
            Write(buffer);
            WriteLine();
        }
        public override void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }

        public override void Write(bool value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(char value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(decimal value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(double value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(float value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(int value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(long value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(object value)
        {
            Write(value.ToString());
        }
        public override void Write(uint value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(ulong value)
        {
            Write(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        public override void Write(string format, params object[] arg)
        {
            Write(String.Format(format, arg));
        }
        public override void Write(string format, object arg0)
        {
            Write(String.Format(format, arg0));
        }
        public override void Write(string format, object arg0, object arg1)
        {
            Write(String.Format(format, arg0, arg1));
        }
        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            Write(String.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(bool value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(char value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(decimal value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(double value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(float value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(int value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(long value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(object value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(uint value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(ulong value)
        {
            Write(value);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0)
        {
            Write(format, arg0);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0, object arg1)
        {
            Write(format, arg0, arg1);
            WriteLine();
        }
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(format, arg0, arg1, arg2);
            WriteLine();
        }
        public override void WriteLine(string format, params object[] arg)
        {
            Write(format, arg);
            WriteLine();
        }

    }
}