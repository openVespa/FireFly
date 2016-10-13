//-----------------------------------------------------------------------------
//
//  usbDemoDevice.cs
//
//  WFF UDB GenericHID Demonstration Application (Version 4_0)
//  A demonstration application for the WFF GenericHID Communication Library
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// The following namespace allows debugging output (when compiled in debug mode)
using System.Diagnostics;

namespace WFF_GenericHID_Demo_Application
    {
    using WFF_GenericHID_Communication_Library;

    class usbDemoDevice : WFF_GenericHID_Communication_Library
        {
 
        // Class constructor - place any initialisation here

        public usbDemoDevice(int vid, int pid) : base(vid, pid)
            {
            // Initialise the local copy of the device's state
            deviceState.LOG_LED_State = false;
            deviceState.led2State = false;
            deviceState.HB_LED_State = false;
            deviceState.VR_LED_State = false;
            deviceState.tachState = 0;
            deviceState.msgType = 0x00;
            deviceState.readMapState = 0x00;
            deviceState.statMAPNum = 0x00;
            deviceState.mapNum = 0x00;
            deviceState.modeNum = 0x00;
            deviceState.maNum = 0x00;
            deviceState.maLNum = 0x00;
            deviceState.maHNum = 0x00;
            deviceState.resetReg = 0x00;
            deviceState.cycleCount = 0x00;
            deviceState.CHT_raw = 0;
            deviceState.EGT_raw = 0;
            }

        // Methods to store and access a local copy of the device's state

        // Initialise the local copy of the device's state
        private struct deviceStateStruct
            {
            public Boolean LOG_LED_State;
            public Boolean led2State;
            public Boolean HB_LED_State;
            public Boolean VR_LED_State;
            public UInt32 tachState;
            public Byte msgType;
            public Byte readMapState;
            public Byte statMAPNum;
            public Byte mapNum;
            public Byte modeNum;
            public Byte maNum;
            public Byte maLNum;
            public Byte maHNum;
            public Byte resetReg;
            public Byte cycleCount;
            public UInt16 CHT_raw;
            public UInt16 EGT_raw;
  

            }

        // Create a game state object
        private deviceStateStruct deviceState;

        // Accessor classes for the device's state values
        public Boolean LOGState
            {
            get { return deviceState.LOG_LED_State; }
            set { deviceState.LOG_LED_State = value; }
            }
        public Boolean Led2State
            {
            get { return deviceState.led2State; }
            set { deviceState.led2State = value; }
            }
        public Boolean HBState
            {
            get { return deviceState.HB_LED_State; }
            set { deviceState.HB_LED_State = value; }
            }
        public Boolean VRState
            {
            get { return deviceState.VR_LED_State; }
            set { deviceState.VR_LED_State = value; }
            }
 
        public UInt32 TachState
            {
            get { return deviceState.tachState; }
            set { deviceState.tachState = value; }
            }
        public UInt32 MsgType
        {
            get { return deviceState.msgType; }
            set { deviceState.msgType = (byte)value; }
        }
        public UInt32 READMapState
        {
            get { return deviceState.readMapState; }
            set { deviceState.readMapState = (byte)value; }
        }
        public UInt32 STATMAPNum
        {
            get { return deviceState.statMAPNum; }
            set { deviceState.statMAPNum = (byte)value; }
        }
        public UInt32 MapNum
        {
            get { return deviceState.mapNum; }
            set { deviceState.mapNum = (byte)value; }
        }
        public UInt32 ModeNum
        {
            get { return deviceState.modeNum; }
            set { deviceState.modeNum = (byte)value; }
        }
        public UInt32 MANum
        {
            get { return deviceState.maNum; }
            set { deviceState.maNum = (byte)value; }
        }
        public UInt32 MALNum
        {
            get { return deviceState.maLNum; }
            set { deviceState.maLNum = (byte)value; }
        }
        public UInt32 MAHNum
        {
            get { return deviceState.maHNum; }
            set { deviceState.maHNum = (byte)value; }
        }

        public UInt32 RESETReg
        {
            get { return deviceState.resetReg; }
            set { deviceState.resetReg = (byte)value; }
        }
        public UInt32 CYCLECount
        {
            get { return deviceState.cycleCount; }
            set { deviceState.cycleCount = (byte)value; }
        }
        public UInt16 CHT_RAW
        {
            get { return deviceState.CHT_raw; }
            set { deviceState.CHT_raw = value; }
        }
 
        public UInt16 EGT_RAW
        {
            get { return deviceState.EGT_raw; }
            set { deviceState.EGT_raw = value; }
        }
//*****************************************************************************
        
//*****************************************************************************
        // Code to send commands to the device ----------

        public bool getDeviceStatus()
            {
   
            // Declare our input buffer
            //14MAY2014 64 to 32    Byte[] inputBuffer = new Byte[64];
            Byte[] inputBuffer = new Byte[32];

            bool success;
            uint TotalTime = 0;
            uint CompATime = 0;
            uint CompBTime = 0;
            uint MA = 0;
            uint MAL = 0;
            uint MAH = 0;
            uint RPM_Calc = 0;
            uint RPM_Ret = 0;
            UInt16 tempMAP_RAW;
            UInt16 tempMAP_LOC;
            UInt16 tempMAP_CHART;
            byte temp_MAP_STAT;
            double temp_CONVERT;
            uint i;
            uint temp_min;
           // Perform the read
                success = readSingleReportFromDevice(ref inputBuffer);

                // Was the read successful?
                if (!success) return false;
                
                // Time to parse the MSG
                // Check header and footer
                //if ((inputBuffer[0] == 0xAB) && (inputBuffer[1] == 0xCD) && (inputBuffer[61] == 0xDE) && (inputBuffer[62] == 0xAD))
                if ((inputBuffer[0] == 0xAB) && (inputBuffer[1] == 0xCD) )
                {
                    // Get the MSG type
                    switch (inputBuffer[2])
                    {
                        case 0x01: // Data Packet
                            TotalTime = (uint)((inputBuffer[5] << 16) + (inputBuffer[3] << 8) + inputBuffer[4]);
                            CompATime = (uint)((inputBuffer[6] << 8) + inputBuffer[7]);
                            CompBTime = (uint)((inputBuffer[8] << 8) + inputBuffer[9]);

                            if (TotalTime > 0)
                            {
                                RPM_Calc = (60 * (16000000 / TotalTime));
                                RPM_Ret = (uint)(inputBuffer[10] * 100);
                                // Toggle VR LED
                                if (deviceState.VR_LED_State == true) { deviceState.VR_LED_State = false; }
                                else { deviceState.VR_LED_State = true; }
                            }

                            else { deviceState.VR_LED_State = false; }
                          
                            deviceState.tachState = RPM_Calc / 10;
                            deviceState.msgType = 0x01;
                            

                            deviceState.EGT_raw = (UInt16)((inputBuffer[16] << 8) + inputBuffer[17]);
                            deviceState.CHT_raw = (UInt16)((inputBuffer[18] << 8) + inputBuffer[19]);
                            

                            deviceState.maNum  = inputBuffer[12];
                            deviceState.maLNum = inputBuffer[13];
                            deviceState.maHNum = inputBuffer[14];
                            deviceState.cycleCount = inputBuffer[15];
                            deviceState.mapNum = inputBuffer[20];
                            //Debug.WriteLine("RPM CALC: " + Convert.ToString(RPM_Calc) + " RPM RET: " + Convert.ToString(RPM_Ret) + " TT: " + Convert.ToString(TotalTime) + " MA: " + Convert.ToString(MA) + " MAL: " + Convert.ToString(MAL) + " MAH: " + Convert.ToString(MAH));
                            Debug.WriteLine("RPM CALC: " + Convert.ToString(RPM_Calc) + " RPM RET: " + Convert.ToString(RPM_Ret) + " TT: " + Convert.ToString(TotalTime) + " COMPA: " + Convert.ToString(CompATime) + " COMPB: " + Convert.ToString(CompBTime));

                            break;

                        case 0x02: // MODE packet

                            deviceState.modeNum = inputBuffer[11];
                            deviceState.msgType = 0x02;
                            Debug.WriteLine("MODE: " + Convert.ToString(inputBuffer[11]));
                            break;

                        case 0x03: // Heart Beat
                            // Nothing important in HB MSG, just toggle HB LED
                            if (deviceState.HB_LED_State == true)
                            {
                                deviceState.HB_LED_State = false;
                            }
                            else
                            {
                                deviceState.HB_LED_State = true;
                            }
                            deviceState.msgType = 0x03;
                            deviceState.mapNum = inputBuffer[20];
                            deviceState.resetReg = inputBuffer[21];
                            deviceState.EGT_raw = (UInt16)((inputBuffer[16] << 8) + inputBuffer[17]);
                            deviceState.CHT_raw = (UInt16)((inputBuffer[18] << 8) + inputBuffer[19]);
                            
                            Debug.WriteLine("HeartBeat");
                            break;

                        case 0x04: // Serial Stream
                            
                            deviceState.msgType = 0x04;
                            
                            Debug.WriteLine("SER:");
                            for (int tempi = 3; tempi < inputBuffer[30]; tempi++)
                            {
                                Debug.Write(Convert.ToChar( inputBuffer[tempi]));
                                
                            }
                            Debug.Write("\r\n");
                            break;
//******************************************************************************************************
                        case 0x8A:  //sendMAP
                            deviceState.modeNum = (byte)((inputBuffer[3] & 0x7F));
                            deviceState.msgType = 0x8A;
                            Debug.WriteLine("SEQ NUM:" + Convert.ToString(deviceState.modeNum));
                            ++deviceState.modeNum;
                            //deviceState.writeMapState = deviceState.modeNum;
                            //  0 to start
                            //  1 for 0-24
                            //  2 for 25-49
                            //  3 for 50-74
                            //  4 for 75-99
                            //14MAY2014 64 to 32    if (deviceState.modeNum < 5)  
                            if (deviceState.modeNum < 11)  
                            {
                                sendMAP(deviceState.modeNum);
                            }
                            else
                            {
                                // Reset FF
                                Debug.WriteLine("WRITE DONE. NEED TO RESET FF!!");
                                resetFF();
                            }
                            break;
//******************************************************************************************************
                        case 0x8B:  //readMAP
                            deviceState.modeNum = (byte)((inputBuffer[3]& 0x7F));
                            deviceState.msgType = 0x8B;

                            Debug.WriteLine("SEQ NUM:" + Convert.ToString(deviceState.modeNum));
                            if (deviceState.modeNum != 0)
                            {
                                temp_MAP_STAT = inputBuffer[29];
                                deviceState.statMAPNum = temp_MAP_STAT;

                                //DAMN!! Gotta update all the min/max values for the table. Fuck
                                for (i = 0; i < 101; i++)
                                {
                                    ChartGlobalData.TestNum[i].Maximum = temp_MAP_STAT - 1;
                                    if (i > 0)
                                    {
                                        temp_min = 65536 / (26667 / i);
                                        if (temp_min > 0)
                                        {
                                            if (temp_MAP_STAT > temp_min)
                                            {
                                                ChartGlobalData.TestNum[i].Minimum = temp_MAP_STAT - temp_min;
                                            }
                                            else
                                            {
                                                ChartGlobalData.TestNum[i].Minimum = 0;
                                            }

                                        }
                                    }
                                    else
                                    {
                                        ChartGlobalData.TestNum[i].Minimum= temp_MAP_STAT - 1;
                                    }
                                }
                                ChartGlobalData.TestNum[0].Minimum = temp_MAP_STAT - 1;

                                //14MAY2014 64 to 32    for (i = 4; i < 54; i++)
                                for (i = 4; i < 24; i++)
                                {
                                    // Covert 2 bytes to 16 bit integer
                                    tempMAP_RAW = (UInt16)((inputBuffer[i] << 8) + inputBuffer[i + 1]);
                                    //14MAY2014 64 to 32    tempMAP_LOC = (UInt16)(((i / 2) - 2)+ ((deviceState.modeNum - 1)*25)) ;
                                    tempMAP_LOC = (UInt16)(((i / 2) - 2) + ((deviceState.modeNum - 1) * 10));
                                    if (tempMAP_LOC == 0)
                                    {
                                        temp_CONVERT = (((600.0 * (1) * tempMAP_RAW) / 16000000.0) + 0.5);
                                    }
                                    else
                                    {
                                        temp_CONVERT = (((600.0 * (tempMAP_LOC) * tempMAP_RAW) / 16000000.0) + 0.5);
                                    }
                                    //temp_CONVERT = (((600.0 * (tempMAP_LOC) * tempMAP_RAW) / 16000000.0) + 0.5);
                                    tempMAP_CHART = (UInt16)(temp_CONVERT);// (((600 * (tempMAP_LOC) * tempMAP_RAW) / 16000000) + 0.5);
                                    ChartGlobalData.TestNum[tempMAP_LOC].Value = temp_MAP_STAT - tempMAP_CHART;
                                    i++;
                                    Debug.WriteLine(Convert.ToString(tempMAP_LOC) + ": " + Convert.ToString(tempMAP_RAW) + " : " + Convert.ToString(temp_MAP_STAT) + " : " + Convert.ToString(tempMAP_CHART) + " : " + Convert.ToString(ChartGlobalData.TestNum[tempMAP_LOC].Value));
                                }
                            }
                            else
                            {
                                Debug.WriteLine("ReadMAP start MSG ACK received");
                            }
                            ++deviceState.modeNum;
                            deviceState.readMapState = deviceState.modeNum;
                            if (deviceState.modeNum < 11)
                            {
                                readMAP(deviceState.modeNum);
                            }
                            else
                            {
                                // Reset FF
                                Debug.WriteLine("READ DONE. NEED TO RESET FF!!");
                                resetFF();
                            }
                            break;

                        default:
                            deviceState.msgType = 0xFF;
                            Debug.WriteLine("UNKNOWN PACKET");
                            break;
                    }
                    
                }
                else
                {
                    Debug.WriteLine("BAD PACKET");
                }

                success = true;
 
            // The data was sent and received ok!
            return success;
            }




        // *********************************************************************************************
        // *********************************************************************************************
        public bool sendMAP(byte SEQ)
        {
            UInt16 tempMAP_RAW;
            UInt16 tempMAP_LOC;
            UInt16 tempMAP_CHART;
            byte temp_MAP_STAT = deviceState.statMAPNum;// 31;
            double temp_CONVERT;
            uint i;
            Debug.WriteLine("Demo Application -> Sending MAP to FF" );

            // Declare our output buffer
            //14MAY2014 64 to 32    Byte[] outputBuffer = new Byte[64];
            Byte[] outputBuffer = new Byte[32];
            outputBuffer[0] = 0xAB;
            outputBuffer[1] = 0xCD;
            outputBuffer[2] = 0x8A;  
            outputBuffer[3] = SEQ; // First packet of sequence
            /*14MAY2014 64 to 32
            outputBuffer[60] = deviceState.statMAPNum;
            outputBuffer[62] = 0xDE;
            outputBuffer[63] = 0xAD;*/
            outputBuffer[29] = deviceState.statMAPNum;
            outputBuffer[30] = 0xDE;
            outputBuffer[31] = 0xAD;
            // Get the values from the chart
            if (SEQ > 0)
            {
                //14MAY2014 64 to 32    for (i = 4; i < 54; i++)
                for (i = 4; i < 24; i++)
                {
                    //14MAY2014 64 to 32    tempMAP_LOC = (UInt16)(((i / 2) - 2) + ((SEQ - 1) * 25));
                    tempMAP_LOC = (UInt16)(((i / 2) - 2) + ((SEQ - 1) * 10));
                    tempMAP_CHART = (UInt16) (temp_MAP_STAT - ChartGlobalData.TestNum[tempMAP_LOC].Value);
                    if (tempMAP_LOC == 0)
                    {
                        //temp_CONVERT = 30000;
                        temp_CONVERT = (16000000.0 * tempMAP_CHART) / (600.0 * 1);
                    }
                    else
                    {
                        temp_CONVERT = (16000000.0 * tempMAP_CHART) / (600.0 * tempMAP_LOC);
                    }
                    tempMAP_RAW = Convert.ToUInt16(temp_CONVERT);
                    outputBuffer[i] = (byte)((tempMAP_RAW & 0xFF00) >> 8); // MSB
                    outputBuffer[i+1] = (byte)(tempMAP_RAW & 0x00FF); // LSB
            
                    Debug.WriteLine("MAP SEND  " + Convert.ToString(tempMAP_LOC) + ": " + Convert.ToString(tempMAP_RAW) + " : " + Convert.ToString(outputBuffer[i]) + " : " + Convert.ToString(outputBuffer[i+1]));
                    ++i;
                }
            }
            // Perform the write command
            bool success;
            success = writeSingleReportToDevice(outputBuffer);

            return success;
        }
        public bool readMAP(byte SEQ)
        {
            Debug.WriteLine("Demo Application -> Reading MAP from FF ");

            // Declare our output buffer
            //14MAY2014 64 to 32    Byte[] outputBuffer = new Byte[64];
            Byte[] outputBuffer = new Byte[32];
            outputBuffer[0] = 0xAB;
            outputBuffer[1] = 0xCD;
            outputBuffer[2] = 0x8B;   // Send Mode #
            outputBuffer[3] = SEQ;
            //outputBuffer[30] = 0xDE;    //outputBuffer[62] = 0xDE;
            //outputBuffer[31] = 0xAD;    //outputBuffer[63] = 0xAD;
            // Perform the write command
            bool success;
            success = writeSingleReportToDevice(outputBuffer);

            return success;
        }
        public bool JUMPtoFW(Byte modenumber)
        {
            Debug.WriteLine("Demo Application -> Sending set device MODE number: " + Convert.ToString(modenumber));

            // Declare our output buffer
            //14MAY2014 64 to 32    Byte[] outputBuffer = new Byte[64];
            Byte[] outputBuffer = new Byte[32];
            outputBuffer[0] = 0xAB;
            outputBuffer[1] = 0xCD;
            outputBuffer[2] = 0x8C;// modenumber;   // Send Mode #
            outputBuffer[3] = 0x00; // First packet of sequence
            outputBuffer[30] = 0xDE;    //outputBuffer[62] = 0xDE;
            outputBuffer[31] = 0xAD;    //outputBuffer[63] = 0xAD;
            // Perform the write command
            bool success;
            success = writeSingleReportToDevice(outputBuffer);

            return success;
        }

        public bool resetFF()
        {
            Debug.WriteLine("Demo Application -> Resetting the FF ");
            // Declare our output buffer
            //14MAY2014 64 to 32    Byte[] outputBuffer = new Byte[64];
            Byte[] outputBuffer = new Byte[32];
            outputBuffer[0] = 0xAB;
            outputBuffer[1] = 0xCD;
            outputBuffer[2] = 0x8F;   // Reset FF
            //outputBuffer[3] = SEQ;
            //outputBuffer[30] = 0xDE;    //outputBuffer[62] = 0xDE;
            //outputBuffer[31] = 0xAD;    //outputBuffer[63] = 0xAD;
            // Perform the write command
            bool success;
            success = writeSingleReportToDevice(outputBuffer);

            return success;
        }
        // *********************************************************************************************
        // *********************************************************************************************
 
        }
    }
