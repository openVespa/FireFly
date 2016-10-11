//-----------------------------------------------------------------------------
//
//  deviceCommunication.cs
//
//  WFF GenericHID Communication Library (Version 4_0)
//  A class for communicating with Generic HID USB devices
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Threading;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace WFF_GenericHID_Communication_Library
    {
    public partial class WFF_GenericHID_Communication_Library
        {
        #region outputToDeviceRegion

        /// <summary>
        /// writeRawReportToDevice - Writes a report to the device using interrupt transfer.
        /// Note: this method performs no checking on the buffer.  The first byte must 
        /// always be zero (or the write will fail!) and the second byte should be the
        /// command number for the USB device firmware.
        /// </summary>
        private bool writeRawReportToDevice(Byte[] outputReportBuffer)
            {
            bool success = false;
            int numberOfBytesWritten = 0;

            // Since the Windows API requires us to have a leading 0 in all file output buffers
            // we must create a 65 byte array, set the first byte to 0 and then copy in the 64
            // bytes of real data in order for the read/write operation to function correctly
            Byte[] adjustedOutputBuffer = new Byte[outputReportBuffer.Length + 1];
            adjustedOutputBuffer[0] = 0;
            outputReportBuffer.CopyTo(adjustedOutputBuffer, 1);

            // Make sure a device is attached
            if (!isDeviceAttached)
                {
                Debug.WriteLine("usbGenericHidCommunication:writeRawReportToDevice(): -> No device attached!");
                return success;
                }

            try
                {
                // Set an output report via interrupt to the device
                success = WriteFile(
                    deviceInformation.writeHandle,
                    adjustedOutputBuffer,
                    adjustedOutputBuffer.Length,
                    ref numberOfBytesWritten,
                    IntPtr.Zero);

                if (success) Debug.WriteLine("usbGenericHidCommunication:writeRawReportToDevice(): -> Write report succeeded");
                else
                    {
                    Debug.WriteLine("usbGenericHidCommunication:writeRawReportToDevice(): -> Write report failed!");
                    }

                return success;
                }
            catch (Exception)
                {
                // An error - send out some debug and return failure
                Debug.WriteLine("usbGenericHidCommunication:writeRawReportToDevice(): -> EXCEPTION: When attempting to send an output report");
                return false;
                }    
            }

        /// <summary>
        /// writeSingleReportToDevice - Writes a single report packet to the USB device.
        /// The size of the passed outputReportBuffer must be correct for the device, so
        /// this method checks the passed buffer's size against the output report size
        /// discovered by the device enumeration.
        /// </summary>
        /// <param name="outputReportBuffer"></param>
        /// <returns></returns>
        protected bool writeSingleReportToDevice(Byte[] outputReportBuffer)
            {
            bool success;

            // The size of our outputReportBuffer must be at least the same size as the output report (which is length+1).
            if (outputReportBuffer.Length != (int)deviceInformation.capabilities.outputReportByteLength - 1)
                {
                // outputReportBuffer is not the right length!
                Debug.WriteLine(
                    "usbGenericHidCommunication:writeSingleReportToDevice(): -> ERROR: The referenced outputReportBuffer size is incorrect for the output report size!");
                return false;
                }

            // The writeRawReportToDevice method will write the passed buffer or return false
            success = writeRawReportToDevice(outputReportBuffer);

            return success;
            }

        /// <summary>
        /// writeMultipleReportsToDevice - Attempts to write multiple reports to the device in 
        /// a single write.  This action can be block the form execution if you write too much data.
        /// If you need more data to the device and want to avoid any blocking you will have to make
        /// multiple commands to the device and deal with the multiple requests and responses in your
        /// application.
        /// </summary>
        /// <param name="outputReportBuffer"></param>
        /// <param name="numberOfReports"></param>
        /// <returns></returns>
        protected bool writeMultipleReportsToDevice(Byte[] outputReportBuffer, int numberOfReports)
            {
            bool success = false;

            // Range check the number of reports
            if (numberOfReports == 0)
                {
                Debug.WriteLine(
                    "usbGenericHidCommunication:writeMultipleReportsToDevice(): -> ERROR: You cannot write 0 reports!");
                return false;
                }

            if (numberOfReports > 128)
                {
                Debug.WriteLine(
                    "usbGenericHidCommunication:writeMultipleReportsToDevice(): -> ERROR: Reference application testing does not verify the code for more than 128 reports");
                return false;
                }

            // The size of our outputReportBuffer must be at least the same size as the output report multiplied by the number of reports to be written.
            if (outputReportBuffer.Length != (((int)deviceInformation.capabilities.outputReportByteLength - 1) * numberOfReports))
                {
                // outputReportBuffer is not the right length!
                Debug.WriteLine(
                    "usbGenericHidCommunication:writeMultipleReportsToDevice(): -> ERROR: The referenced outputReportBuffer size is incorrect for the number of output reports requested!");
                return false;
                }

            // The windows API returns a write failure if we try to send more than deviceInformation.capabilities.outputReportByteLength bytes
            // in a single write to the device (would be nice if the HID API handled this, but it doesn't).  Therefore we have to split a multi-
            // report send into 64 byte chunks and send one at a time...

            Int64 reportNumber;
            Byte[] tempOutputBuffer = new Byte[deviceInformation.capabilities.outputReportByteLength - 1];

            for (reportNumber = 0; reportNumber < numberOfReports; reportNumber++)
                {
                // Copy the next chunk of 64 bytes into the temporary output buffer
                Int64 startByte = 0;
                Int64 pointer = 0;

                for (startByte = reportNumber * 64; startByte < (reportNumber * 64) + 64; startByte++)
                    {
                    tempOutputBuffer[pointer] = outputReportBuffer[startByte];
                    pointer++;
                    }

                // The writeRawReportToDevice method will write the passed buffer or return false
                success = writeRawReportToDevice(tempOutputBuffer);

                if (success == false)
                    {
                    Debug.WriteLine(
                    "usbGenericHidCommunication:writeMultipleReportsToDevice(): -> ERROR: Sending failed for report {0} of {0}!",
                    reportNumber, numberOfReports);

                    // Give up
                    return false;
                    }
                }

            return success;
            }
        #endregion

        #region inputFromDeviceRegion
        /// <summary>
        /// readRawReportFromDevice - Reads a raw report from the device with timeout handling
        /// Note: This method performs no checking on the buffer.  The first byte returned is
        /// usually zero, the second byte is the command that the USB firmware is replying to.
        /// The other 63 bytes are (possibly) data.
        /// 
        /// The maximum report size will be determind by the length of the inputReportBuffer.
        /// </summary>
        private bool readRawReportFromDevice(ref Byte[] inputReportBuffer, ref int numberOfBytesRead)
            {
            IntPtr eventObject = IntPtr.Zero;
            NativeOverlapped hidOverlapped = new NativeOverlapped();
            IntPtr nonManagedBuffer = IntPtr.Zero;
            IntPtr nonManagedOverlapped = IntPtr.Zero;
            Int32 result = 0;
            bool success;

            // Since the Windows API requires us to have a leading 0 in all file input buffers
            // we must create a 65 byte array, set the first byte to 0 and then copy in the 64
            // bytes of real data in order for the read/write operation to function correctly
            Byte[] adjustedInputBuffer = new Byte[inputReportBuffer.Length + 1];
            adjustedInputBuffer[0] = 0;

            // Make sure a device is attached
            if (!isDeviceAttached)
                {
                Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> No device attached!");
                return false;
                }

            try
                {
                // Prepare an event object for the overlapped ReadFile
                eventObject = CreateEvent(IntPtr.Zero, false, false, "");

                hidOverlapped.OffsetLow = 0;
                hidOverlapped.OffsetHigh = 0;
                hidOverlapped.EventHandle = eventObject;

                // Allocate memory for the unmanaged input buffer and overlap structure.
                nonManagedBuffer = Marshal.AllocHGlobal(adjustedInputBuffer.Length);
                nonManagedOverlapped = Marshal.AllocHGlobal(Marshal.SizeOf(hidOverlapped));
                Marshal.StructureToPtr(hidOverlapped, nonManagedOverlapped, false);

                // Read the input report buffer
                Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> Attempting to ReadFile");
                success = ReadFile(
                    deviceInformation.readHandle,
                    nonManagedBuffer,
                    adjustedInputBuffer.Length,
                    ref numberOfBytesRead,
                    nonManagedOverlapped);

                if (!success)
                    {
                    // We are now waiting for the FileRead to complete
                    Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> ReadFile started, waiting for completion...");

                    // Wait a maximum of 3 seconds for the FileRead to complete
                    result = WaitForSingleObject(eventObject, 3000);

                    switch (result)
                        {
                        // Has the ReadFile completed successfully?
                        case (System.Int32)WAIT_OBJECT_0:
                            success = true;

                            // Get the number of bytes transferred
                            GetOverlappedResult(deviceInformation.readHandle, nonManagedOverlapped, ref numberOfBytesRead, false);

                            Debug.WriteLine(string.Format("usbGenericHidCommunication:readRawReportFromDevice(): -> ReadFile successful (overlapped) {0} bytes read", numberOfBytesRead));
                            break;

                        // Did the FileRead operation timeout?
                        case WAIT_TIMEOUT:
                            success = false;

                            Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> ReadFile timedout! USB device detached");

                            // Cancel the ReadFile operation
                            CancelIo(deviceInformation.readHandle);
                            if (!deviceInformation.hidHandle.IsInvalid) deviceInformation.hidHandle.Close();
                            if (!deviceInformation.readHandle.IsInvalid) deviceInformation.readHandle.Close();
                            if (!deviceInformation.writeHandle.IsInvalid) deviceInformation.writeHandle.Close();

                            // Detach the USB device to try to get us back in a known state
                            detachUsbDevice();

                            break;

                        // Some other unspecified error has occurred?
                        default:
                            success = false;

                            // Cancel the ReadFile operation

                            // Detach the USB device to try to get us back in a known state
                            detachUsbDevice();

                            Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> ReadFile unspecified error! USB device detached");
                            break;
                        }
                    }
                if (success)
                    {
                    // Report receieved correctly, copy the unmanaged input buffer over to the managed array buffer
                    Marshal.Copy(nonManagedBuffer, adjustedInputBuffer, 0, numberOfBytesRead);
        
                    // Make sure we didn't get a successful read with 0 bytes back
                    if (numberOfBytesRead > 0)
                        {
                        // Now we need to loose the leading 0 byte and transfer the buffer over to the real input buffer
                        // (I couldn't find a nicer way of doing this with a managed array of bytes, but if you know of 
                        // one, let me know ;)
                        Int64 byteCounter;
                        for (byteCounter = 1; byteCounter < adjustedInputBuffer.Length; byteCounter++)
                            inputReportBuffer[byteCounter - 1] = adjustedInputBuffer[byteCounter];

                        // Adjust the number of bytes read (since it's returned by reference)
                        numberOfBytesRead -= 1;
                        }

                    Debug.WriteLine(string.Format("usbGenericHidCommunication:readRawReportFromDevice(): -> ReadFile successful returning {0} bytes", numberOfBytesRead));
                    }
                }
            catch (Exception)
                {
                // An error - send out some debug and return failure
                Debug.WriteLine("usbGenericHidCommunication:readRawReportFromDevice(): -> EXCEPTION: When attempting to receive an input report");
                return false;
                }

            // Release non-managed objects before returning
            Marshal.FreeHGlobal(nonManagedBuffer);
            Marshal.FreeHGlobal(nonManagedOverlapped);

            // Close the file handle to release the object
            CloseHandle(eventObject);

            return success;
            }

        /// <summary>
        /// readSingleReportFromDevice - Reads a single report packet from the USB device.
        /// The size of the passed inputReportBuffer must be correct for the device, so
        /// this method checks the passed buffer's size against the input report size
        /// discovered by the device enumeration.
        /// </summary>
        /// <param name="inputReportBuffer"></param>
        /// <returns></returns>
        protected bool readSingleReportFromDevice(ref Byte[] inputReportBuffer)
            {
            bool success;
            int numberOfBytesRead = 0;

            // The size of our inputReportBuffer must be at least the same size as the input report.
            if (inputReportBuffer.Length != ((int)deviceInformation.capabilities.inputReportByteLength - 1))
                {
                // inputReportBuffer is not the right length!
                Debug.WriteLine(
                    "usbGenericHidCommunication:readSingleReportFromDevice(): -> ERROR: The referenced inputReportBuffer size is incorrect for the input report size!");
                return false;
                }

            // The readRawReportFromDevice method will fill the passed readBuffer or return false
            success = readRawReportFromDevice(ref inputReportBuffer, ref numberOfBytesRead);

            return success;
            }

        /// <summary>
        /// readMultipleReportsFromDevice - Attempts to retrieve multiple reports from the device in 
        /// a single read.  This action can be block the form execution if you request too much data.
        /// If you need more data from the device and want to avoid any blocking you will have to make
        /// multiple commands to the device and deal with the multiple requests and responses in your
        /// application.
        /// </summary>
        /// <param name="inputReportBuffer"></param>
        /// <param name="numberOfReports"></param>
        /// <returns></returns>
        protected bool readMultipleReportsFromDevice(ref Byte[] inputReportBuffer, int numberOfReports)
            {
            bool success = false;
            int numberOfBytesRead = 0;
            long pointerToBuffer = 0;

            // Note: The Windows HID API always returns with 65 bytes (i.e. a leading 0 and 64 real bytes) no
            // matter how much data we request from the ReadFile, so this method is effectively the same as
            // calling readRawReportFromDevice many times with a 64 byte buffer.  However it provides a nice
            // abstraction so it is kept.

            // Define a temporary buffer for assembling partial data reads into the completed inputReportBuffer
            Byte[] temporaryBuffer = new Byte[deviceInformation.capabilities.inputReportByteLength - 1];

            // Range check the number of reports
            if (numberOfReports == 0)
                {
                Debug.WriteLine(
                    "usbGenericHidCommunication:readMultipleReportsFromDevice(): -> ERROR: You cannot request 0 reports!");
                return false;
                }

            if (numberOfReports > 128)
                {
                Debug.WriteLine(
                    "usbGenericHidCommunication:readMultipleReportsFromDevice(): -> ERROR: Reference application testing does not verify the code for more than 128 reports");
                return false;
                }

            // The size of our inputReportBuffer must be at least the same size as the input report multiplied by the number of reports requested.
            if (inputReportBuffer.Length != (((int)deviceInformation.capabilities.inputReportByteLength - 1) * numberOfReports))
                {
                // inputReportBuffer is not the right length!
                Debug.WriteLine(
                    "usbGenericHidCommunication:readMultipleReportsFromDevice(): -> ERROR: The referenced inputReportBuffer size is incorrect for the number of input reports requested!");
                return false;
                }

            // The readRawReportFromDevice method will fill the passed read buffer or return false
            while (pointerToBuffer != (((int)deviceInformation.capabilities.inputReportByteLength-1) * numberOfReports))
                {
                Debug.WriteLine(
                    "usbGenericHidCommunication:readMultipleReportsFromDevice(): -> Reading from device...");
                success = readRawReportFromDevice(ref temporaryBuffer, ref numberOfBytesRead);

                // Was the read successful?
                if (!success)
                    {
                    return false;
                    }

                // Copy the received data into the referenced input buffer
                Array.Copy(temporaryBuffer, 0, inputReportBuffer, pointerToBuffer, (long)numberOfBytesRead);
                pointerToBuffer += (long)numberOfBytesRead;
                }


            Debug.WriteLine(
                "usbGenericHidCommunication:readMultipleReportsFromDevice(): -> Got {0} bytes from device", pointerToBuffer);
            return success;
            }

        #endregion
        }
    }
