///////////////////////////////////////////////////////////////////////////////
//File: FileVersioning.cs
//
//Description: Contains information about which features to load from each version
//  UTL files.
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
using System.Collections.Generic;
using System.Text;

#if VTC_PLUGIN
using uTank2.LootPlugins;
#endif

namespace VTClassic
{
    internal enum eUTLFileFeature
    {
        RuleExpression = 1,
        RequirementLengthCode = 2,
    }

    internal static class UTLVersionInfo
    {
        public const int MAX_PROFILE_VERSION = 1;

        public static bool VersionHasFeature(eUTLFileFeature feature, int version)
        {
            switch (version)
            {
                case 0:
                    return false;
                case 1:
                    if (feature == eUTLFileFeature.RequirementLengthCode) return true;
                    if (feature == eUTLFileFeature.RuleExpression) return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}