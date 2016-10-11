//-----------------------------------------------------------------------------
//
//  setupapiDll.cs
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

using System.Runtime.InteropServices;

namespace WFF_GenericHID_Communication_Library
    {
    /// <summary>
    /// setupapiDll - Internal class containing the API definitions required for interoperability
    /// with the setupapi.dll.  Primarily this provides methods for device discovery and 
    /// identification.
    /// </summary>
    public partial class WFF_GenericHID_Communication_Library
        {
        // from setupapi.h
        internal const Int32 DIGCF_PRESENT = 2;
        internal const Int32 DIGCF_DEVICEINTERFACE = 0X10;

        internal struct SP_DEVICE_INTERFACE_DATA
            {
            internal Int32 cbSize;
            internal System.Guid InterfaceClassGuid;
            internal Int32 Flags;
            internal IntPtr Reserved;
            }

        //internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        //    {
        //    internal Int32 cbSize;
        //    internal String DevicePath;
        //    }

        //internal struct SP_DEVINFO_DATA
        //    {
        //    internal Int32 cbSize;
        //    internal System.Guid ClassGuid;
        //    internal Int32 DevInst;
        //    internal Int32 Reserved;
        //    }

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Int32 SetupDiCreateDeviceInfoList(
            ref System.Guid ClassGuid,
            Int32 hwndParent
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Int32 SetupDiDestroyDeviceInfoList(
            IntPtr DeviceInfoSet
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern Boolean SetupDiEnumDeviceInterfaces(
            IntPtr DeviceInfoSet,
            IntPtr DeviceInfoData,
            ref System.Guid InterfaceClassGuid,
            Int32 MemberIndex,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData
            );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SetupDiGetClassDevs(
            ref System.Guid ClassGuid,
            IntPtr Enumerator,
            IntPtr hwndParent,
            Int32 Flags
            );

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern Boolean SetupDiGetDeviceInterfaceDetail(
            IntPtr DeviceInfoSet,
            ref SP_DEVICE_INTERFACE_DATA DeviceInterfaceData,
            IntPtr DeviceInterfaceDetailData,
            Int32 DeviceInterfaceDetailDataSize,
            ref Int32 RequiredSize,
            IntPtr DeviceInfoData);
        }
    }
