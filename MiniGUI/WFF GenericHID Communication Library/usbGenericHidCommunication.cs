//-----------------------------------------------------------------------------
//
//  usbGenericHidCommunications.cs
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
using Microsoft.Win32.SafeHandles;

using System.Collections;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace WFF_GenericHID_Communication_Library
    {
    // Since the Class Library is using System.windows.forms we need to set the following
    // attribute to prevent VS2010 getting confused and attempting to open class library
    // .cs files in the form designer.
    [System.ComponentModel.DesignerCategory("")]

    /// <summary>
    /// WFF GenericHID Communication Library
    /// </summary>
    /// <remarks>This Library provides communication and device handling events
    /// for USB devices which implement the generic HID protocol. It is
    /// designed to be inherited into a class which deals with a specific
    /// device firmware and is therefore defined as an abstract class.
    /// 
    /// This class definition contains the highest level parts of the
    /// class which is defined over a number of files</remarks>
    public partial class WFF_GenericHID_Communication_Library
        {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>This constructor method creates an object for HID communication and attempts to find
        /// (and bind to) the USB device indicated by the passed VID (Vendor ID) and PID (Product ID) which
        /// should both be passed as unsigned integers.</remarks>
        public WFF_GenericHID_Communication_Library(int vid, int pid)
            {
            Debug.WriteLine("WFF_GenericHID_Communication_Library:WFF_GenericHID_Communication_Library() -> Class constructor called");

            // Set the deviceAttached flag to false
            deviceInformation.deviceAttached = false;

            // Store the target device's VID and PID in the device attributes
            deviceInformation.targetVid = (UInt16)vid;
            deviceInformation.targetPid = (UInt16)pid;

            // Register for device notifications
            registerForDeviceNotifications(this.Handle);
            }

        /// <summary>
        /// Destructor
        /// </summary>
        /// <remarks>This method closes any open connections to the USB device and clears up any resources
        /// that have been consumed over the lifetime of the object.</remarks>
        ~WFF_GenericHID_Communication_Library()
            {
            Debug.WriteLine("WFF_GenericHID_Communication_Library:WFF_GenericHID_Communication_Library() -> Class destructor called");

            // Detach the USB device (performs required clean up operations)
            detachUsbDevice();
            }

        /// <summary>
        /// deviceAttributesStructure is used to represent the details of the target USB device as the methods
        /// discover them so they can be reused by other methods when communicating with the device
        /// </summary>
        private struct deviceInformationStructure
            {
            public UInt16 targetVid;                // Our target device's VID
            public UInt16 targetPid;                // Our target device's PID
            public bool deviceAttached;             // Device attachment state flag
            public HIDD_ATTRIBUTES attributes;      // HID Attributes
            public HIDP_CAPS capabilities;          // HID Capabilities
            public SafeFileHandle readHandle;       // Read handle from the device
            public SafeFileHandle writeHandle;      // Write handle to the device
            public SafeFileHandle hidHandle;        // Handle used for communicating via hid.dll
            public String devicePathName;           // The device's path name
            public IntPtr deviceNotificationHandle; // The device's notification handle
            }

        /// <summary>
        /// deviceAttributes contains the discovered attributes of the target USB device
        /// </summary>
        private deviceInformationStructure deviceInformation;

        /// <summary>
        /// Detach the USB device
        /// </summary>
        /// <remarks>This method detaches the USB device and frees the read and write handles
        /// to the device.</remarks>
        private void detachUsbDevice()
            {
            Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> Method called");

            // Is a device currently attached?
            if (isDeviceAttached)
                {
                Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> Detaching device and closing file handles");
                // Set the device status to detached;
                isDeviceAttached = false;

                // Close the readHandle, writeHandle and hidHandle
                if (!deviceInformation.hidHandle.IsInvalid) deviceInformation.hidHandle.Close();
                if (!deviceInformation.readHandle.IsInvalid) deviceInformation.readHandle.Close();
                if (!deviceInformation.writeHandle.IsInvalid) deviceInformation.writeHandle.Close();

                // Throw an event
                onUsbEvent(EventArgs.Empty);
                }
            else Debug.WriteLine("WFF_GenericHID_Communication_Library:detachUsbDevice() -> No device attached");
            }

        /// <summary>
        /// Is device attached?
        /// </summary>
        /// <remarks>This method is used to set (private) or test (public) the device attachment status</remarks>
        public bool isDeviceAttached
            {
            get
                {
                return deviceInformation.deviceAttached;
                }
            private set
                {
                deviceInformation.deviceAttached = value;
                }
            }
        }
    }
