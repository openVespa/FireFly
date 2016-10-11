//-----------------------------------------------------------------------------
//
//  hidDll.cs
//
//  WFF GenericHID Communication Library (Version 4_0)
//  A class for communicating with Generic HID USB devices
//
//  Copyright (c) 2011 Simon Inns
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
//
//  Web:    http://www.waitingforfriday.com
//  Email:  simon.inns@gmail.com
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace WFF_GenericHID_Communication_Library
    {
    /// <summary>
    /// hidDll - Partial class containing the API definitions required for interoperability
    /// with the hdd.dll API from the Windows Driver Kit (WDK).  Primarily this provides
    /// methods for communicating with Windows' USB generic HID driver.
    /// </summary>
    public partial class WFF_GenericHID_Communication_Library
        {
        // API declarations for hid.dll, taken from hidpi.h (part of the 
        // Windows Driver Kit (WDK

        internal const Int16 HidP_Input = 0;
        internal const Int16 HidP_Output = 1;
        internal const Int16 HidP_Feature = 2;

        [StructLayout(LayoutKind.Sequential)]
        internal struct HIDD_ATTRIBUTES
            {
            internal Int32 size;
            internal UInt16 vendorId;
            internal UInt16 productId;
            internal UInt16 versionNumber;
            }

        internal struct HIDP_CAPS
            {
            internal Int16 usage;
            internal Int16 usagePage;
            internal Int16 inputReportByteLength;
            internal Int16 outputReportByteLength;
            internal Int16 featureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            internal Int16[] reserved;
            internal Int16 numberLinkCollectionNodes;
            internal Int16 numberInputButtonCaps;
            internal Int16 numberInputValueCaps;
            internal Int16 numberInputDataIndices;
            internal Int16 numberOutputButtonCaps;
            internal Int16 numberOutputValueCaps;
            internal Int16 numberOutputDataIndices;
            internal Int16 numberFeatureButtonCaps;
            internal Int16 numberFeatureValueCaps;
            internal Int16 numberFeatureDataIndices;
            }

        //internal struct HidP_Value_Caps
        //    {
        //    internal Int16 usagePage;
        //    internal Byte reportID;
        //    internal Int32 isAlias;
        //    internal Int16 bitField;
        //    internal Int16 linkCollection;
        //    internal Int16 linkUsage;
        //    internal Int16 linkUsagePage;
        //    internal Int32 isRange;
        //    internal Int32 isStringRange;
        //    internal Int32 isDesignatorRange;
        //    internal Int32 isAbsolute;
        //    internal Int32 hasNull;
        //    internal Byte reserved;
        //    internal Int16 bitSize;
        //    internal Int16 reportCount;
        //    internal Int16 reserved2;
        //    internal Int16 reserved3;
        //    internal Int16 reserved4;
        //    internal Int16 reserved5;
        //    internal Int16 reserved6;
        //    internal Int32 logicalMin;
        //    internal Int32 logicalMax;
        //    internal Int32 physicalMin;
        //    internal Int32 physicalMax;
        //    internal Int16 usageMin;
        //    internal Int16 usageMax;
        //    internal Int16 stringMin;
        //    internal Int16 stringMax;
        //    internal Int16 designatorMin;
        //    internal Int16 designatorMax;
        //    internal Int16 dataIndexMin;
        //    internal Int16 dataIndexMax;
        //    }

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_FlushQueue(
            SafeFileHandle HidDeviceObject
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_FreePreparsedData(
            IntPtr PreparsedData
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetAttributes(
            SafeFileHandle HidDeviceObject,
            ref HIDD_ATTRIBUTES Attributes
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetFeature(
            SafeFileHandle HidDeviceObject,
            Byte[] lpReportBuffer,
            Int32 ReportBufferLength
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetInputReport(
            SafeFileHandle HidDeviceObject,
            Byte[] lpReportBuffer,
            Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern void HidD_GetHidGuid(
            ref System.Guid HidGuid
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetNumInputBuffers(
            SafeFileHandle HidDeviceObject,
            ref Int32 NumberBuffers
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_GetPreparsedData(
            SafeFileHandle HidDeviceObject,
            ref IntPtr PreparsedData
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_SetFeature(
            SafeFileHandle HidDeviceObject,
            Byte[] lpReportBuffer,
            Int32 ReportBufferLength);

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_SetNumInputBuffers(
            SafeFileHandle HidDeviceObject,
            Int32 NumberBuffers
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Boolean HidD_SetOutputReport(
            SafeFileHandle HidDeviceObject,
            Byte[] lpReportBuffer,
            Int32 ReportBufferLength
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Int32 HidP_GetCaps(
            IntPtr PreparsedData,
            ref HIDP_CAPS Capabilities
            );

        [DllImport("hid.dll", SetLastError = true)]
        internal static extern Int32 HidP_GetValueCaps(
            Int32 ReportType,
            Byte[] ValueCaps,
            ref Int32 ValueCapsLength,
            IntPtr PreparsedData
            );
        }
    }
