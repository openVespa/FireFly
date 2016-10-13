//-----------------------------------------------------------------------------
//
//  user32Dll.cs
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
    /// user32Dll - Internal class containing the API definitions required for interoperability
    /// with the user32.dll.  Primarily this provides methods for erceiving event notifications
    /// which are used to alert the application when devices are added and removed from the 
    /// system.
    /// </summary>
    public partial class WFF_GenericHID_Communication_Library
        {
        // from dbt.h
        internal const Int32 DBT_DEVICEARRIVAL = 0x8000;
        internal const Int32 DBT_DEVICEREMOVECOMPLETE = 0x8004;
        internal const Int32 DBT_DEVTYP_DEVICEINTERFACE = 5;
        internal const Int32 DBT_DEVTYP_HANDLE = 6;
        internal const Int32 DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;
        internal const Int32 DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        internal const Int32 DEVICE_NOTIFY_WINDOW_HANDLE = 0;
        internal const Int32 WM_DEVICECHANGE = 0x219;

        [StructLayout(LayoutKind.Sequential)]
        internal class DEV_BROADCAST_DEVICEINTERFACE
            {
            internal Int32 dbcc_size;
            internal Int32 dbcc_devicetype;
            internal Int32 dbcc_reserved;
            internal Guid dbcc_classguid;
            internal Int16 dbcc_name;
            }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal class DEV_BROADCAST_DEVICEINTERFACE_1
            {
            internal Int32 dbcc_size;
            internal Int32 dbcc_devicetype;
            internal Int32 dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            internal Byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            internal Char[] dbcc_name;
            }

        [StructLayout(LayoutKind.Sequential)]
        internal class DEV_BROADCAST_HDR
            {
            internal Int32 dbch_size;
            internal Int32 dbch_devicetype;
            internal Int32 dbch_reserved;
            }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr RegisterDeviceNotification(
            IntPtr hRecipient,
            IntPtr NotificationFilter, 
            Int32 Flags
            );

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern Boolean UnregisterDeviceNotification(
            IntPtr Handle
            );
        }
    }
