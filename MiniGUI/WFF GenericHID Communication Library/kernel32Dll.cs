//-----------------------------------------------------------------------------
//
//  kernel32Dll.cs
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

namespace WFF_GenericHID_Communication_Library
    {
    /// <summary>
    /// kernel32Dll - Internal class containing the API definitions required for interoperability
    /// with the kernel32.dll.  Primarily this provides file operation methods required for
    /// sending and receiving data from Windows' USB generic HID driver.
    /// </summary>
    public partial class WFF_GenericHID_Communication_Library
        {
        internal const Int32 FILE_FLAG_OVERLAPPED = 0x40000000;
        internal const Int32 FILE_SHARE_READ = 1;
        internal const Int32 FILE_SHARE_WRITE = 2;
        internal const UInt32 GENERIC_READ = 0x80000000;
        internal const UInt32 GENERIC_WRITE = 0x40000000;
        internal const Int32 INVALID_HANDLE_VALUE = -1;
        internal const Int32 OPEN_EXISTING = 3;
        internal const Int32 WAIT_TIMEOUT = 0x102;
        internal const Int32 WAIT_OBJECT_0 = 0;

        [StructLayout(LayoutKind.Sequential)]
        internal class SECURITY_ATTRIBUTES
            {
            internal Int32 nLength;
            internal Int32 lpSecurityDescriptor;
            internal Int32 bInheritHandle;
            }

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Int32 CancelIo(
            SafeFileHandle hFile
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CreateEvent(
            IntPtr SecurityAttributes,
            Boolean bManualReset,
            Boolean bInitialState,
            String lpName
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFile(
            String lpFileName,
            UInt32 dwDesiredAccess,
            Int32 dwShareMode,
            IntPtr lpSecurityAttributes,
            Int32 dwCreationDisposition,
            Int32 dwFlagsAndAttributes,
            Int32 hTemplateFile
            );

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern Boolean GetOverlappedResult(
            SafeFileHandle hFile,
            IntPtr lpOverlapped,
            ref Int32 lpNumberOfBytesTransferred,
            Boolean bWait
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Boolean ReadFile(
            SafeFileHandle hFile,
            IntPtr lpBuffer,
            Int32 nNumberOfBytesToRead,
            ref Int32 lpNumberOfBytesRead,
            IntPtr lpOverlapped
            );

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Int32 WaitForSingleObject(
            IntPtr hHandle,
            Int32 dwMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern Boolean WriteFile(
            SafeFileHandle hFile,
            Byte[] lpBuffer,
            Int32 nNumberOfBytesToWrite,
            ref Int32 lpNumberOfBytesWritten,
            IntPtr lpOverlapped
            );

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool CloseHandle(
            IntPtr h
            );
        }
    }
