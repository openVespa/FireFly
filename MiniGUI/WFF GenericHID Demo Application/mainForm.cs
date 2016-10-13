//-----------------------------------------------------------------------------
//
//  mainForm.cs
//
//  WFF UDB GenericHID Demonstration Application (Version 4_0)
//  A demonstration application for the WFF GenericHID Communication Library
//
//-----------------------------------------------------------------------------

// BAL  ADDED MSG PARSING
// BAL  Tested and the HB LED blinked
// BAL  Added logging
//
// BAL  Added EGT gauge and code for that. Ray visit.
// BAL  20 NOV 2015, this is what I am using. Seems to mostly work.
// BAL  15 APRIL 2016, still using this.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace WFF_GenericHID_Demo_Application
    {
 
    public partial class mainForm : Form
        {
        Label[] Testlabel = new Label[101];
        public mainForm()
            {
            
            InitializeComponent();
            FillMapPanel();
            // FFmini test code uses Atmels test VID/PID VID=0x03EB and PID=0x2150
            theUsbDemoDevice = new usbDemoDevice(0x03EB, 0x204F);
   
            // Add a listener for usb events
            theUsbDemoDevice.usbEvent += new usbDemoDevice.usbEventsHandler(usbEvent_receiver);

            // Perform an initial search for the target USB device (in case
            // it is already connected as we will not get an event for it)
            theUsbDemoDevice.findTargetDevice();
            
            }

        // Create an instance of the USB reference device object
        private usbDemoDevice theUsbDemoDevice;
        private StreamWriter writer = new StreamWriter("c:\\BROOKS_TEMP\\LOGGER16APRIL2016.CSV");

        // Create a listener for USB events
        private void usbEvent_receiver(object o, EventArgs e)
            {
            // Check the status of the USB device and update the form accordingly
            if (theUsbDemoDevice.isDeviceAttached)
                {
                // USB Device is currently attached
                // Update the form's status label
                this.usbDeviceStatusLabel.Text = "USB Device is attached";
                }

            else
                {
                // USB Device is currently unattached

                // Update the form's status label
                this.usbDeviceStatusLabel.Text = "USB Device is detached";

                // Update the form
                this.potStateLabel.Text = "0";
                //this.potentiometerAnalogMeter.Value = 0;
                this.TACH_GAUGE.Value = 0;
                theUsbDemoDevice.TachState = 0;
                theUsbDemoDevice.MANum = 0;
                theUsbDemoDevice.MALNum = 0;
                theUsbDemoDevice.MAHNum = 0;
                theUsbDemoDevice.LOGState = false;
                theUsbDemoDevice.Led2State = false;
                theUsbDemoDevice.HBState = false;
                theUsbDemoDevice.VRState = false;

                this.LOGPictureBox.Image = Properties.Resources.red_off_16;
                this.led2PictureBox.Image = Properties.Resources.red_off_16;
                this.HBPictureBox.Image = Properties.Resources.red_off_16;
                this.VRPictureBox.Image = Properties.Resources.red_off_16;

                // Clear the debug window
                this.debugTextBox.Clear();
                }
            }

          // When the device status poll timer ticks we query the USB device for status
        private void deviceStatusPollTimer_Tick(object sender, EventArgs e)
        {    
            // Get the device's status
            if (theUsbDemoDevice.isDeviceAttached)
            {
                if (theUsbDemoDevice.getDeviceStatus())
                {
                    // Update the form
                    if (theUsbDemoDevice.LOGState == true) this.LOGPictureBox.Image = Properties.Resources.red_on_16;
                    else this.LOGPictureBox.Image = Properties.Resources.red_off_16;
                    if (theUsbDemoDevice.Led2State == true) this.led2PictureBox.Image = Properties.Resources.red_on_16;
                    else this.led2PictureBox.Image = Properties.Resources.red_off_16;
                    if (theUsbDemoDevice.HBState == true) this.HBPictureBox.Image = Properties.Resources.red_on_16;
                    else this.HBPictureBox.Image = Properties.Resources.red_off_16;
                    if (theUsbDemoDevice.VRState == true) this.VRPictureBox.Image = Properties.Resources.red_on_16;
                    else this.VRPictureBox.Image = Properties.Resources.red_off_16;

                    this.potStateLabel.Text = Convert.ToString(theUsbDemoDevice.TachState * 10);
                    this.TACH_GAUGE.Value = (float)(theUsbDemoDevice.TachState/100.0);

// MOVE THIS SOMEWHERE ELSE!!!!!!!!!!!

                    UInt16 temp_CHT;
                    UInt16 temp_EGT;
                    float tE;
                    float tC;
          
// **********************************   CHT   ****************************************************
                    if ((theUsbDemoDevice.CHT_RAW & 0x0004) > 0)// 0xFFFF)
                    {
                        this.CHT_GAUGE.Value = 0;
                        CHT_txt.Text = "OPEN SENSOR";
                    }
                    else
                    {
                        temp_CHT = theUsbDemoDevice.CHT_RAW;
                        temp_CHT &= 0x7FF8;
                        temp_CHT >>= 0x03;
                        tC = (float)(((temp_CHT * 0.25) * 1.8) + 32.0);
                        this.CHT_GAUGE.Value = tC;
                        CHT_txt.Text = Convert.ToString(tC);
                    }
// **********************************   EGT   ****************************************************
                    if ((theUsbDemoDevice.EGT_RAW & 0x0004) > 0) //== 0xFFFF)
                    {
                        this.EGT_GAUGE.Value = 0;
                        EGT_txt.Text = "OPEN SENSOR";
                    }
                    else
                    {
                        temp_EGT = theUsbDemoDevice.EGT_RAW;
                        temp_EGT &= 0x7FF8;
                        temp_EGT >>= 0x03;
                        //tE = (float)(temp_EGT / 4.0);
                        //tE = (float)((temp_EGT * 1.8) + 32.0);
                        tE = (float)(((temp_EGT * 0.25) * 1.8) + 32.0);
                        this.EGT_GAUGE.Value = tE;
                        EGT_txt.Text = Convert.ToString(tE);
                    }
// **********************************   RPM   ****************************************************
                    RPM_txt.Text = Convert.ToString(theUsbDemoDevice.TachState*10);

                    this.MAPLabel.Text = Convert.ToString(theUsbDemoDevice.MapNum);
                    this.MODELabel.Text = Convert.ToString(theUsbDemoDevice.ModeNum);

                    switch (theUsbDemoDevice.MsgType)
                    {
                        case 1:

                            this.debugTextBox.AppendText("Data\r\n");

                            if (theUsbDemoDevice.LOGState == true)
                            {
                                // Currently logging RESET REG, MAP NUM, TACH STATE, MAL, MA, MAH
                                this.writer.WriteLine(Convert.ToString(theUsbDemoDevice.RESETReg) + "," + Convert.ToString(theUsbDemoDevice.CYCLECount) + "," + Convert.ToString(theUsbDemoDevice.MapNum) + "," + Convert.ToString(theUsbDemoDevice.TachState * 10) + "," + CHT_txt.Text + "," + EGT_txt.Text + "," + Convert.ToString(theUsbDemoDevice.MALNum) + "," + Convert.ToString(theUsbDemoDevice.MANum) + "," + Convert.ToString(theUsbDemoDevice.MAHNum) + ";");
                                //this.writer.WriteLine(Convert.ToString(theUsbDemoDevice.RESETReg) + "," + Convert.ToString(theUsbDemoDevice.CYCLECount) + "," + Convert.ToString(theUsbDemoDevice.MapNum) + "," + Convert.ToString(theUsbDemoDevice.TachState * 10) + "," + Convert.ToString(theUsbDemoDevice.MALNum) + "," + Convert.ToString(theUsbDemoDevice.MANum) + "," + Convert.ToString(theUsbDemoDevice.MAHNum) + "," + Convert.ToString(theUsbDemoDevice.YYCOUNT) + "," + Convert.ToString(Y1LOG) + "," + Convert.ToString(Y2LOG) + "," + Convert.ToString(Y3LOG) + "," + Convert.ToString(Y4LOG) + "," + Convert.ToString(Y5LOG) + "," + Convert.ToString(Y6LOG) + "," + Convert.ToString(Y7LOG) + "," + Convert.ToString(theUsbDemoDevice.PURPLECOUNT) + "," + Convert.ToString(P1LOG) + "," + Convert.ToString(P2LOG) + "," + Convert.ToString(P3LOG) + "," + Convert.ToString(P4LOG) + "," + Convert.ToString(P5LOG) + "," + Convert.ToString(P6LOG) + "," + Convert.ToString(theUsbDemoDevice.CHT_RAW) + ";");
                            }
                            break;

                        case 2:
                            if (theUsbDemoDevice.ModeNum == 3)//138)
                            {
                                this.debugTextBox.AppendText("Mode: LOAD LUT\r\n");
                            }
                            else if (theUsbDemoDevice.ModeNum == 4)//139)
                            {
                                this.debugTextBox.AppendText("Mode: READ LUT\r\n");
                            }
                            else if (theUsbDemoDevice.ModeNum == 5)//140)
                            {
                                this.debugTextBox.AppendText("Mode: LOAD FW\r\n");
                            }
                            else
                            {
                                this.debugTextBox.AppendText("Mode: UNKNOWN\r\n");
                            }
                            break;

                        case 3:

                            this.debugTextBox.AppendText("HeartBeat\r\n");
                            break;

                        case 4:

                            this.debugTextBox.AppendText("SERIAL\r\n");
                            break;

                        case 0x8A:
                            this.debugTextBox.AppendText("Write Map ACK\r\n");
                            
                            if (theUsbDemoDevice.ModeNum < 11)
                            {
                                // Update progress bar
                                progressBar1.Value = (int)(theUsbDemoDevice.ModeNum * 10);
                            }
                            else
                            {
                                progressBar1.Value = 100;
                                LUTLOADbutton.Enabled = true;
                                LUTREADbutton.Enabled = true;
                                FWbutton.Enabled = true;
                                RUNbutton.Enabled = true;
                                progressBar1.Value = 0;
                            }
                            break;
                        case 0x8B:
                            this.debugTextBox.AppendText("Read Map ACK\r\n");
                            STATUpDown.Value = theUsbDemoDevice.STATMAPNum;
                            if (theUsbDemoDevice.READMapState < 11)
                            {
                                // Update progress bar
                                progressBar1.Value = (int)(theUsbDemoDevice.READMapState * 10);
                            }
                            else
                            {
                                progressBar1.Value = 100;
                                LUTLOADbutton.Enabled = true;
                                LUTREADbutton.Enabled = true;
                                FWbutton.Enabled = true;
                                RUNbutton.Enabled = true;
                                progressBar1.Value = 0;
                            }
                            break;

                        default:
                            this.debugTextBox.AppendText("!! BAD PACKET !!\r\n");
                            break;

                    }
                }
            }
        }

        private void LOGPictureBox_Click(object sender, EventArgs e)
        {
                // LOG LED clicked

                if (theUsbDemoDevice.LOGState == false)
                 {
                    this.LOGPictureBox.Image = Properties.Resources.red_on_16;
                    theUsbDemoDevice.LOGState = true;
                    this.writer.WriteLine("RESETReg, CYCLECount, MapNum, RPM, CHT, IAT, TPS, EGT, MALNum, MANum, MAHNum, FILTERCount, FILTERMA1, FILTERRPM1, FILTERMA2, FILTERRPM2, FILTERMA3, FILTERRPM3, FILTERMA4, FILTERRPM4, FILTERMA5, FILTERRPM5, PURPLECOUNT, P1LOG, P2LOG, P3LOG, P4LOG, P5LOG, P6LOG ;");
                 }
                else
                 {
                     this.writer.WriteLine("STOP LOG");
                     this.writer.Close();
                     this.LOGPictureBox.Image = Properties.Resources.red_off_16;
                     theUsbDemoDevice.LOGState = false;
                 }
        }

        private void LUTLOADbutton_Click(object sender, EventArgs e)
        {
            DialogResult LUTLOADyn;
            if (theUsbDemoDevice.isDeviceAttached)
            {
                LUTLOADyn = MessageBox.Show("Are you sure you want to load this map to FF?\r\nIt will over write existing map.", "Confirm Map Load", MessageBoxButtons.YesNo);
                if (LUTLOADyn == DialogResult.Yes)
                {
                    LUTLOADbutton.Enabled = false;
                    LUTREADbutton.Enabled = false;
                    RUNbutton.Enabled = false;
                    FWbutton.Enabled = false;
                    // This starts the SEND MAP to FF sequence
                    theUsbDemoDevice.sendMAP(0x00);
                }
            }

            else
            {
                LUTLOADyn = MessageBox.Show("Connect Firelfy and try again.", "Firefly not Attached", MessageBoxButtons.OK);
            }
        }

        private void LUTREADbutton_Click(object sender, EventArgs e)
        {
            DialogResult LUTREADyn;
            if (theUsbDemoDevice.isDeviceAttached)
            {
                LUTLOADbutton.Enabled = false;
                LUTREADbutton.Enabled = false;
                FWbutton.Enabled = false;
                RUNbutton.Enabled = false;
                LUTREADyn = MessageBox.Show("Are you sure you want to read the map from FF?\r\nAll unsaved IGN map data will be lost.", "Confirm Read Map", MessageBoxButtons.YesNo);
                if (LUTREADyn == DialogResult.Yes)
                {
                    
                    theUsbDemoDevice.readMAP(0x00);
                }
                else
                {
                    LUTLOADbutton.Enabled = true;
                    LUTREADbutton.Enabled = true;
                    FWbutton.Enabled = true;
                    RUNbutton.Enabled = true;
                }
            }
            else
            {
                LUTREADyn = MessageBox.Show("Connect Firelfy and try again.","Firefly not Attached", MessageBoxButtons.OK);
            }

        }

        private void FWbutton_Click(object sender, EventArgs e)
        {
            LUTLOADbutton.Enabled = true;
            LUTREADbutton.Enabled = true;
            FWbutton.Enabled = false;
            theUsbDemoDevice.JUMPtoFW(0x8C);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //int size = -1;
            StreamReader readerOPEN = null;
            string stringReader;
            string tempLOC;
            string tempVALUE;
            string tempCRC;
            bool ReadMapError = false;
            int i;

            int indexC;
            int indexV;
            int indexer;

            openFileDialog1.Filter = "FireFly Ign |*.fly";
            openFileDialog1.Title = "Open a FireFly Map";
            openFileDialog1.FileName = "";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    readerOPEN = new StreamReader(openFileDialog1.FileName);
                    if (readerOPEN != null)
                    {
                        using (readerOPEN)
                        {
                            // Insert code to read the stream here.
                            //Until reader.Peek = -1
                            while ((stringReader = readerOPEN.ReadLine()) != null)
                            {
                                //stringReader = readerOPEN.ReadLine();
                                if (stringReader.IndexOf("[#NOTE:", 0) != -1)
                                {
                                    indexer = stringReader.IndexOf("END]", 0);
                                    // the_notes = stringReader.Substring( 7, stringReader.Length - 13);
                                    //MessageBox.Show(stringReader.Substring(7, stringReader.Length - 13));
                                }
                                else if (stringReader.IndexOf("[#STAT:", 0) != -1)
                                {
                                    indexer = stringReader.IndexOf("END]", 0);
                                    indexC = stringReader.IndexOf("C", 0);
                                    STATUpDown.Value = Convert.ToUInt16(stringReader.Substring(7, indexC - 7));
                                    //MessageBox.Show(stringReader.Substring(7, indexC - 7));
                                    //TO DO: CHECK CRC VALUES
                                }
                                else if (stringReader.IndexOf("[#MAP:", 0) != -1)
                                {
                                    indexer = stringReader.IndexOf("END]", 0);
                                    indexC = stringReader.IndexOf("C", 0);
                                    indexV = stringReader.IndexOf("V", 0);
                                    tempLOC = stringReader.Substring(6, indexV - 6);
                                    tempVALUE = stringReader.Substring(indexV + 1, indexC - indexV - 1);
                                    tempCRC = stringReader.Substring(indexC + 1, indexer - indexC - 1);
                                    ChartGlobalData.TestNum[Convert.ToUInt16(tempLOC)].Value = Convert.ToUInt16(tempVALUE);
                                    //TO DO: CHECK CRC VALUES
                                    //MsgBox(tempLOC & "  " & tempVALUE & "  " & tempCRC)
                                }

                                else
                                {
                                    //   MsgBox("ooops FILE READ")
                                    MessageBox.Show("ooops FILE READ");
                                    ReadMapError = true;
                                }
                            }
                            // Did this to assign value in cell# 100 
                            //FIX ME!!!
                            ChartGlobalData.TestNum[100].Value = ChartGlobalData.TestNum[99].Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ReadMapError = true;
                    readerOPEN.Close();
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
                if (ReadMapError == false)
                {
                    for (i = 0; i < 101; i++)
                    {
                        Testlabel[i].Visible = true;
                        ChartGlobalData.TestNum[i].Visible = true;
                        ChartGlobalData.TestNum[i].ReadOnly = false;
                    }
                    chart1.Visible = true;
                    MAPpanel.Visible = true;
                    STATUpDown.Visible = true;
                    saveToolStripMenuItem.Enabled = true;
                    RUNbutton.Visible = true;
                    LUTLOADbutton.Visible = true;
                    LUTREADbutton.Visible = true;
                    TACH_GAUGE.Visible = false;
                    CHT_GAUGE.Visible = false;
                    
                    EGT_GAUGE.Visible = false;
                    
                    RPM_txt.Visible = false;
                    CHT_txt.Visible = false;
                    
                    EGT_txt.Visible = false;
                    
                    HBPictureBox.Visible = false;
                    VRPictureBox.Visible = false;
                    LOGPictureBox.Visible = false;
                    led2PictureBox.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    label4.Visible = false;
                    label5.Visible = false;
                    label6.Visible = false;
                    label7.Visible = false;
                    label8.Visible = false;
                    label10.Visible = false;
                    potStateLabel.Visible = false;
                    MAPLabel.Visible = false;
                    MODELabel.Visible = false;
                    groupBox1.Text = "IGN MAP Edit";
                }
            }
            //          Console.WriteLine(size); // <-- Shows file size in debugging mode.
            //Console.WriteLine(result); // <-- For debugging use only.

           
        }

        //**************************************************************************
        private void FillMapPanel()
        {
            int i = 0;
            Series seriesX;

            for (i = 0; i < 101; i++)
            {
                Testlabel[i] = new System.Windows.Forms.Label();
                this.MAPpanel.Controls.Add(Testlabel[i]);
                Testlabel[i].AutoSize = true;
                Testlabel[i].Location = new System.Drawing.Point(2, (26 * i) + 5);
                Testlabel[i].Name = "TestLabel" + i.ToString();
                Testlabel[i].Size = new System.Drawing.Size(13, 13);
                Testlabel[i].TabIndex = 50 + i;
                Testlabel[i].Text = i.ToString();
                Testlabel[i].Visible = false;

                //************************************************************************************
                ChartGlobalData.TestNum[i] = new System.Windows.Forms.NumericUpDown();
                this.MAPpanel.Controls.Add(ChartGlobalData.TestNum[i]);
                ChartGlobalData.TestNum[i].Location = new System.Drawing.Point(21, (26 * i) + 3);
                ChartGlobalData.TestNum[i].Name = "ChartGlobalData.TestNum" + i.ToString();
                ChartGlobalData.TestNum[i].Size = new System.Drawing.Size(40, 40);
                ChartGlobalData.TestNum[i].TabIndex = 80 + i;
                ChartGlobalData.TestNum[i].ReadOnly = true;
                ChartGlobalData.TestNum[i].ValueChanged += new System.EventHandler(TestNum_ValueChanged);
                ChartGlobalData.TestNum[i].Visible = false;

            }
            //************************************************************************************
            seriesX = new Series();
            seriesX.Name = "Tester";
            chart1.Series.Add(seriesX);
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 10000;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1000;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
        
        }
        public void TestNum_ValueChanged(object sender, EventArgs e)
        {
            int i = 0;
            chart1.Series["Tester"].Points.Clear();
            chart1.ChartAreas[0].AxisX.MajorGrid.Interval = 1000;
            chart1.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
            for (i = 0; i < 101; i++)
            {
                //chart1.Series["Tester"].Points.AddY(i);
                chart1.Series["Tester"].Points.AddXY(i * 100, ChartGlobalData.TestNum[i].Value);
            }
            // Set series chart type
            chart1.Series["Tester"].ChartType = SeriesChartType.Line;
            chart1.Series["Tester"].Color = System.Drawing.Color.Red;
            chart1.Series["Tester"].BorderWidth = 3;
            // MessageBox.Show(((NumericUpDown)sender).Name.ToString());
        }

        private void RUNbutton_Click(object sender, EventArgs e)
        {
            int i;
            DialogResult RUNyn;
            RUNyn = MessageBox.Show("Are you sure you want to return to RUN mode? All unsaved IGN map data will be lost", "Return to RUN MODE?", MessageBoxButtons.YesNo);
            if (RUNyn == DialogResult.Yes)
            {
                for (i = 0; i < 101; i++)
                {
                    Testlabel[i].Visible = false;
                    ChartGlobalData.TestNum[i].Visible = false;
                    ChartGlobalData.TestNum[i].ReadOnly = true;
                }
                chart1.Visible = false;
                MAPpanel.Visible = false;
                STATUpDown.Visible = false;
                saveToolStripMenuItem.Enabled = false;
                RUNbutton.Visible = false;
                LUTLOADbutton.Visible = false;
                LUTREADbutton.Visible = false;
                TACH_GAUGE.Visible = true;
                CHT_GAUGE.Visible = true;
                
                EGT_GAUGE.Visible = true;
                
                RPM_txt.Visible = true;
                CHT_txt.Visible = true;
                
                EGT_txt.Visible = true;
                
                HBPictureBox.Visible = true;
                VRPictureBox.Visible = true;
                LOGPictureBox.Visible = true;
                led2PictureBox.Visible = true;
                label1.Visible = true;
                label2.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label10.Visible = true;
                potStateLabel.Visible = true;
                MAPLabel.Visible = true;
                MODELabel.Visible = true;
                groupBox1.Text = "FF Run Mode";
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i;
            STATUpDown.Visible = true;
            STATUpDown.Value = 18;
            for (i = 0; i < 101; i++)
            {
                Testlabel[i].Visible = true;
                ChartGlobalData.TestNum[i].Visible = true;
                ChartGlobalData.TestNum[i].ReadOnly = false;
                ChartGlobalData.TestNum[i].Value = STATUpDown.Value - 1;
            }
            chart1.Visible = true;
            MAPpanel.Visible = true;
 
            saveToolStripMenuItem.Enabled = true;
            RUNbutton.Visible = true;
            LUTLOADbutton.Visible = true;
            LUTREADbutton.Visible = true;
            TACH_GAUGE.Visible = false;
            CHT_GAUGE.Visible = false;
            
            EGT_GAUGE.Visible = false;
            
            RPM_txt.Visible = false;
            CHT_txt.Visible = false;
            
            EGT_txt.Visible = false;
            
            HBPictureBox.Visible = false;
            VRPictureBox.Visible = false;
            LOGPictureBox.Visible = false;
            led2PictureBox.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label10.Visible = false;
            potStateLabel.Visible = false;
            MAPLabel.Visible = false;
            MODELabel.Visible = false;
            editToolStripMenuItem.Enabled = true;
            groupBox1.Text = "IGN MAP Edit";
            
        }

 
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
            int i;
            saveFileDialog1.Filter = "FireFly Ign |*.fly";
            saveFileDialog1.Title = "Save a FireFly Map";
            saveFileDialog1.FileName = "";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
            // If the file name is not an empty string open it for saving.
            if( saveFileDialog1.FileName != null ) 
              {
                StreamWriter writerSAVE= new StreamWriter(saveFileDialog1.FileName);
                writerSAVE.Write("[#NOTE:" + "ADD NOTES HERE" + "C8END]");
                writerSAVE.WriteLine("");
                writerSAVE.Write("[#STAT:" + Convert.ToString(STATUpDown.Value) + "C8END]");
                writerSAVE.WriteLine("");
                for (i=0;i<101;i++)
                {
                    writerSAVE.Write("[#MAP:" + Convert.ToString(i ) + "V" + Convert.ToString(ChartGlobalData.TestNum[i].Value) + "C8END]");
                    writerSAVE.WriteLine("");
                    progressBar1.Value = i;
                }

                try
                {
                    writerSAVE.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not close file.", "File save error", MessageBoxButtons.OK);
                }
              }
 
            }
        }

        private void STATUpDown_ValueChanged(object sender, EventArgs e)
        {
            uint i;
            uint temp_min;
            //DAMN!! Gotta update all the min/max values for the table.
            for (i = 0; i < 101; i++)
            {
                ChartGlobalData.TestNum[i].Maximum = STATUpDown.Value  - 1;
                if (i > 0)
                {
                    temp_min = 65536 / (26667 / i);
                    if (temp_min > 0)
                    {
                        if (STATUpDown.Value > temp_min)
                        {
                            ChartGlobalData.TestNum[i].Minimum = STATUpDown.Value - temp_min;
                        }
                        else
                        {
                            ChartGlobalData.TestNum[i].Minimum = 0;
                        }

                    }
                }
                else
                {
                    ChartGlobalData.TestNum[i].Minimum = STATUpDown.Value - 1;
                }
            }
            ChartGlobalData.TestNum[0].Minimum = STATUpDown.Value - 1;
            theUsbDemoDevice.STATMAPNum = (Byte)STATUpDown.Value;
        }

        private void blockFillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TO DO!!!", "ADD BLOCK FILL", MessageBoxButtons.OK);
        }

        private void mapNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TO DO!!!", "ADD MAP NOTES", MessageBoxButtons.OK);
        }

   
            //**********************************************************************************
    
        }
    public class ChartGlobalData
    {
        public static NumericUpDown[] TestNum = new NumericUpDown[101];
    }
    }
