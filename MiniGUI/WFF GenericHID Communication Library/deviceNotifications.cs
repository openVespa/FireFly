//-----------------------------------------------------------------------------
//
//  deviceNotifications.cs
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

using System.Windows.Forms;
using System.Runtime.InteropServices;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace WFF_GenericHID_Communication_Library
    {
    /// <summary>
    /// This partial class contains the methods required for detecting when
    /// the USB device is attached or detached.
    /// </summary>
    public partial class WFF_GenericHID_Communication_Library : Control
        {

        /// <summary>
        /// Create a delegate for the USB event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void usbEventsHandler(object sender, EventArgs e);

        /// <summary>
        /// Define the event
        /// </summary>
        public event usbEventsHandler usbEvent;

        /// <summary>
        /// The usb event thrower
        /// </summary>
        /// <param name="e"></param>
        protected virtual void onUsbEvent(EventArgs e)
            {
            if (usbEvent != null)
                {
                Debug.WriteLine("usbGenericHidCommunications:onUsbEvent() -> Throwing a USB event to a listener");
                usbEvent(this, e);
                }
            else Debug.WriteLine("usbGenericHidCommunications:onUsbEvent() -> Attempted to throw a USB event, but no one was listening");
            }

        /// <summary>
        /// isNotificationForTargetDevice - Compares the target devices pathname against the
        /// pathname of the device which caused the event message
        /// </summary>
        private Boolean isNotificationForTargetDevice(Message m)
            {
            Int32 stringSize;

            try
                {
                DEV_BROADCAST_DEVICEINTERFACE_1 devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE_1();
                DEV_BROADCAST_HDR devBroadcastHeader = new DEV_BROADCAST_HDR();

                Marshal.PtrToStructure(m.LParam, devBroadcastHeader);

                // Is the notification event concerning a device interface?
                if ((devBroadcastHeader.dbch_devicetype == DBT_DEVTYP_DEVICEINTERFACE))
                    {
                    // Get the device path name of the affected device
                    stringSize = System.Convert.ToInt32((devBroadcastHeader.dbch_size - 32) / 2);    
                    devBroadcastDeviceInterface.dbcc_name = new Char[stringSize + 1];
                    Marshal.PtrToStructure(m.LParam, devBroadcastDeviceInterface);
                    String deviceNameString = new String(devBroadcastDeviceInterface.dbcc_name, 0, stringSize);

                    // Compare the device name with our target device's pathname (strings are moved to lower case
                    // using en-US to ensure case insensitivity accross different regions)
                    if ((String.Compare(deviceNameString.ToLower(new System.Globalization.CultureInfo("en-US")),
                        deviceInformation.devicePathName.ToLower(new System.Globalization.CultureInfo("en-US")), true) == 0)) return true;
                    else return false;
                    }
                }
            catch (Exception)
                {
                Debug.WriteLine("usbGenericHidCommunication:isNotificationForTargetDevice() -> EXCEPTION: An unknown exception has occured!");
                return false;
                }
            return false;
            }

        /// <summary>
        /// registerForDeviceNotification - registers the window (identified by the windowHandle) for 
        /// device notification messages from Windows
        /// </summary>
        public Boolean registerForDeviceNotifications(IntPtr windowHandle)
            {
            Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Method called");

            // A DEV_BROADCAST_DEVICEINTERFACE header holds information about the request.
            DEV_BROADCAST_DEVICEINTERFACE devBroadcastDeviceInterface = new DEV_BROADCAST_DEVICEINTERFACE();
            IntPtr devBroadcastDeviceInterfaceBuffer = IntPtr.Zero;
            Int32 size = 0;

            // Get the required GUID
            System.Guid systemHidGuid = new Guid();
            HidD_GetHidGuid(ref systemHidGuid);

            try
                {
                // Set the parameters in the DEV_BROADCAST_DEVICEINTERFACE structure.
                size = Marshal.SizeOf(devBroadcastDeviceInterface);
                devBroadcastDeviceInterface.dbcc_size = size;
                devBroadcastDeviceInterface.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
                devBroadcastDeviceInterface.dbcc_reserved = 0;
                devBroadcastDeviceInterface.dbcc_classguid = systemHidGuid;

                devBroadcastDeviceInterfaceBuffer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(devBroadcastDeviceInterface, devBroadcastDeviceInterfaceBuffer, true);

                // Register for notifications and store the returned handle
                deviceInformation.deviceNotificationHandle = RegisterDeviceNotification(windowHandle, devBroadcastDeviceInterfaceBuffer, DEVICE_NOTIFY_WINDOW_HANDLE);
                Marshal.PtrToStructure(devBroadcastDeviceInterfaceBuffer, devBroadcastDeviceInterface);

                if ((deviceInformation.deviceNotificationHandle.ToInt32() == IntPtr.Zero.ToInt32()))
                    {
                    Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Notification registration failed");
                    return false;
                    }
                else
                    {
                    Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> Notification registration succeded");
                    return true;
                    }
                }
            catch (Exception)
                {
                Debug.WriteLine("usbGenericHidCommunication:registerForDeviceNotifications() -> EXCEPTION: An unknown exception has occured!");
                }
            finally
                {
                // Free the memory allocated previously by AllocHGlobal.
                if (devBroadcastDeviceInterfaceBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(devBroadcastDeviceInterfaceBuffer);
                }
            return false;
            }

        /// <summary>
        /// handleDeviceNotificationMessages - this method examines any windows devices messages that are
        /// received to check if they are relevant to our target USB device.  If so the method takes the 
        /// correct action dependent on the message type.
        /// </summary>
        /// <param name="m"></param>
        public void handleDeviceNotificationMessages(Message m)
            {
            //Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> Method called");
            
            // Make sure this is a device notification
            if (m.Msg != WM_DEVICECHANGE) return;

            Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> Device notification received");

            try
                {
                switch (m.WParam.ToInt32())
                    {
                    // Device attached
                    case DBT_DEVICEARRIVAL:
                        Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> New device attached");
                        // If our target device is not currently attached, this could be our device, so we attempt to find it.
                        if (!isDeviceAttached)
                            {
                            findTargetDevice();
                            onUsbEvent(EventArgs.Empty); // Generate an event
                            }
                        break;

                    // Device removed
                    case DBT_DEVICEREMOVECOMPLETE:
                        Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> A device has been removed");

                        // Was this our target device?  
                        if (isNotificationForTargetDevice(m))
                            {
                            // If so detach the USB device.
                            Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> The target USB device has been removed - detaching...");
                            detachUsbDevice();
                            onUsbEvent(EventArgs.Empty); // Generate an event
                            }
                        break;

                    // Other message
                    default:
                        Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> Unknown notification message");
                        break;
                    }
                }
            catch (Exception)
                {
                Debug.WriteLine("usbGenericHidCommunication:handleDeviceNotificationMessages() -> EXCEPTION: An unknown exception has occured!");
                } 
            }

        /// <summary>
        /// WndProc - This method overrides the WinProc in order to pass notification messages
        /// to the base WndProc
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
            {
            handleDeviceNotificationMessages(m);

            // Pass the notification message to the base WndProc
            base.WndProc(ref m);
            }
        }
    }
