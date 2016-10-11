namespace WFF_GenericHID_Demo_Application
    {
    partial class mainForm
        {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
            {
            if (disposing && (components != null))
                {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
            {
                this.components = new System.ComponentModel.Container();
                System.Windows.Forms.AGaugeLabel aGaugeLabel1 = new System.Windows.Forms.AGaugeLabel();
                System.Windows.Forms.AGaugeRange aGaugeRange1 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange2 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange3 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeLabel aGaugeLabel2 = new System.Windows.Forms.AGaugeLabel();
                System.Windows.Forms.AGaugeLabel aGaugeLabel3 = new System.Windows.Forms.AGaugeLabel();
                System.Windows.Forms.AGaugeRange aGaugeRange4 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange5 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange6 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
                System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
                System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
                System.Windows.Forms.AGaugeLabel aGaugeLabel4 = new System.Windows.Forms.AGaugeLabel();
                System.Windows.Forms.AGaugeRange aGaugeRange7 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange8 = new System.Windows.Forms.AGaugeRange();
                System.Windows.Forms.AGaugeRange aGaugeRange9 = new System.Windows.Forms.AGaugeRange();
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
                this.statusStrip1 = new System.Windows.Forms.StatusStrip();
                this.usbDeviceStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
                this.groupBox1 = new System.Windows.Forms.GroupBox();
                this.RPM_txt = new System.Windows.Forms.TextBox();
                this.CHT_txt = new System.Windows.Forms.TextBox();
                this.CHT_GAUGE = new System.Windows.Forms.AGauge();
                this.TACH_GAUGE = new System.Windows.Forms.AGauge();
                this.MODELabel = new System.Windows.Forms.Label();
                this.label10 = new System.Windows.Forms.Label();
                this.label8 = new System.Windows.Forms.Label();
                this.label2 = new System.Windows.Forms.Label();
                this.label7 = new System.Windows.Forms.Label();
                this.label5 = new System.Windows.Forms.Label();
                this.MAPLabel = new System.Windows.Forms.Label();
                this.label6 = new System.Windows.Forms.Label();
                this.potStateLabel = new System.Windows.Forms.Label();
                this.label4 = new System.Windows.Forms.Label();
                this.label1 = new System.Windows.Forms.Label();
                this.VRPictureBox = new System.Windows.Forms.PictureBox();
                this.HBPictureBox = new System.Windows.Forms.PictureBox();
                this.led2PictureBox = new System.Windows.Forms.PictureBox();
                this.LOGPictureBox = new System.Windows.Forms.PictureBox();
                this.MAPpanel = new System.Windows.Forms.Panel();
                this.STATUpDown = new System.Windows.Forms.NumericUpDown();
                this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
                this.EGT_txt = new System.Windows.Forms.TextBox();
                this.EGT_GAUGE = new System.Windows.Forms.AGauge();
                this.groupBox2 = new System.Windows.Forms.GroupBox();
                this.debugTextBox = new System.Windows.Forms.TextBox();
                this.deviceStatusPollTimer = new System.Windows.Forms.Timer(this.components);
                this.RUNbutton = new System.Windows.Forms.Button();
                this.LUTLOADbutton = new System.Windows.Forms.Button();
                this.FWbutton = new System.Windows.Forms.Button();
                this.LUTREADbutton = new System.Windows.Forms.Button();
                this.menuStrip1 = new System.Windows.Forms.MenuStrip();
                this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.blockFillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.mapNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
                this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                this.progressBar1 = new System.Windows.Forms.ProgressBar();
                this.statusStrip1.SuspendLayout();
                this.groupBox1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.VRPictureBox)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.HBPictureBox)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.led2PictureBox)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.LOGPictureBox)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.STATUpDown)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
                this.groupBox2.SuspendLayout();
                this.menuStrip1.SuspendLayout();
                this.SuspendLayout();
                // 
                // statusStrip1
                // 
                this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usbDeviceStatusLabel});
                this.statusStrip1.Location = new System.Drawing.Point(0, 493);
                this.statusStrip1.Name = "statusStrip1";
                this.statusStrip1.Size = new System.Drawing.Size(933, 22);
                this.statusStrip1.SizingGrip = false;
                this.statusStrip1.TabIndex = 0;
                this.statusStrip1.Text = "statusStrip1";
                // 
                // usbDeviceStatusLabel
                // 
                this.usbDeviceStatusLabel.Name = "usbDeviceStatusLabel";
                this.usbDeviceStatusLabel.Size = new System.Drawing.Size(108, 17);
                this.usbDeviceStatusLabel.Text = "USB device detached";
                // 
                // groupBox1
                // 
                this.groupBox1.Controls.Add(this.RPM_txt);
                this.groupBox1.Controls.Add(this.CHT_txt);
                this.groupBox1.Controls.Add(this.CHT_GAUGE);
                this.groupBox1.Controls.Add(this.TACH_GAUGE);
                this.groupBox1.Controls.Add(this.MODELabel);
                this.groupBox1.Controls.Add(this.label10);
                this.groupBox1.Controls.Add(this.label8);
                this.groupBox1.Controls.Add(this.label2);
                this.groupBox1.Controls.Add(this.label7);
                this.groupBox1.Controls.Add(this.label5);
                this.groupBox1.Controls.Add(this.MAPLabel);
                this.groupBox1.Controls.Add(this.label6);
                this.groupBox1.Controls.Add(this.potStateLabel);
                this.groupBox1.Controls.Add(this.label4);
                this.groupBox1.Controls.Add(this.label1);
                this.groupBox1.Controls.Add(this.VRPictureBox);
                this.groupBox1.Controls.Add(this.HBPictureBox);
                this.groupBox1.Controls.Add(this.led2PictureBox);
                this.groupBox1.Controls.Add(this.LOGPictureBox);
                this.groupBox1.Controls.Add(this.MAPpanel);
                this.groupBox1.Controls.Add(this.STATUpDown);
                this.groupBox1.Controls.Add(this.chart1);
                this.groupBox1.Location = new System.Drawing.Point(13, 28);
                this.groupBox1.Name = "groupBox1";
                this.groupBox1.Size = new System.Drawing.Size(792, 416);
                this.groupBox1.TabIndex = 2;
                this.groupBox1.TabStop = false;
                this.groupBox1.Text = "FF Run  Mode";
                // 
                // RPM_txt
                // 
                this.RPM_txt.Location = new System.Drawing.Point(57, 263);
                this.RPM_txt.Name = "RPM_txt";
                this.RPM_txt.Size = new System.Drawing.Size(100, 20);
                this.RPM_txt.TabIndex = 27;
                // 
                // CHT_txt
                // 
                this.CHT_txt.Location = new System.Drawing.Point(307, 264);
                this.CHT_txt.Name = "CHT_txt";
                this.CHT_txt.Size = new System.Drawing.Size(100, 20);
                this.CHT_txt.TabIndex = 27;
                // 
                // CHT_GAUGE
                // 
                this.CHT_GAUGE.BaseArcColor = System.Drawing.Color.Gray;
                this.CHT_GAUGE.BaseArcRadius = 80;
                this.CHT_GAUGE.BaseArcStart = 135;
                this.CHT_GAUGE.BaseArcSweep = 270;
                this.CHT_GAUGE.BaseArcWidth = 2;
                this.CHT_GAUGE.Center = new System.Drawing.Point(100, 100);
                aGaugeLabel1.Color = System.Drawing.SystemColors.WindowText;
                aGaugeLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aGaugeLabel1.Name = "GaugeLabel1";
                aGaugeLabel1.Position = new System.Drawing.Point(85, 50);
                aGaugeLabel1.Text = "CHT";
                this.CHT_GAUGE.GaugeLabels.Add(aGaugeLabel1);
                aGaugeRange1.Color = System.Drawing.Color.Lime;
                aGaugeRange1.EndValue = 350F;
                aGaugeRange1.InnerRadius = 75;
                aGaugeRange1.InRange = false;
                aGaugeRange1.Name = "GaugeRange1";
                aGaugeRange1.OuterRadius = 80;
                aGaugeRange1.StartValue = 0F;
                aGaugeRange2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                aGaugeRange2.EndValue = 350F;
                aGaugeRange2.InnerRadius = 70;
                aGaugeRange2.InRange = false;
                aGaugeRange2.Name = "GaugeRange2";
                aGaugeRange2.OuterRadius = 75;
                aGaugeRange2.StartValue = 0F;
                aGaugeRange3.Color = System.Drawing.Color.Red;
                aGaugeRange3.EndValue = 400F;
                aGaugeRange3.InnerRadius = 70;
                aGaugeRange3.InRange = false;
                aGaugeRange3.Name = "GaugeRange3";
                aGaugeRange3.OuterRadius = 80;
                aGaugeRange3.StartValue = 350F;
                this.CHT_GAUGE.GaugeRanges.Add(aGaugeRange1);
                this.CHT_GAUGE.GaugeRanges.Add(aGaugeRange2);
                this.CHT_GAUGE.GaugeRanges.Add(aGaugeRange3);
                this.CHT_GAUGE.Location = new System.Drawing.Point(258, 104);
                this.CHT_GAUGE.MaxValue = 400F;
                this.CHT_GAUGE.MinValue = 0F;
                this.CHT_GAUGE.Name = "CHT_GAUGE";
                this.CHT_GAUGE.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Yellow;
                this.CHT_GAUGE.NeedleColor2 = System.Drawing.Color.Olive;
                this.CHT_GAUGE.NeedleRadius = 80;
                this.CHT_GAUGE.NeedleType = System.Windows.Forms.NeedleType.Advance;
                this.CHT_GAUGE.NeedleWidth = 2;
                this.CHT_GAUGE.ScaleLinesInterColor = System.Drawing.Color.Black;
                this.CHT_GAUGE.ScaleLinesInterInnerRadius = 73;
                this.CHT_GAUGE.ScaleLinesInterOuterRadius = 80;
                this.CHT_GAUGE.ScaleLinesInterWidth = 1;
                this.CHT_GAUGE.ScaleLinesMajorColor = System.Drawing.Color.Black;
                this.CHT_GAUGE.ScaleLinesMajorInnerRadius = 70;
                this.CHT_GAUGE.ScaleLinesMajorOuterRadius = 80;
                this.CHT_GAUGE.ScaleLinesMajorStepValue = 50F;
                this.CHT_GAUGE.ScaleLinesMajorWidth = 2;
                this.CHT_GAUGE.ScaleLinesMinorColor = System.Drawing.Color.Gray;
                this.CHT_GAUGE.ScaleLinesMinorInnerRadius = 75;
                this.CHT_GAUGE.ScaleLinesMinorOuterRadius = 80;
                this.CHT_GAUGE.ScaleLinesMinorTicks = 9;
                this.CHT_GAUGE.ScaleLinesMinorWidth = 1;
                this.CHT_GAUGE.ScaleNumbersColor = System.Drawing.Color.Black;
                this.CHT_GAUGE.ScaleNumbersFormat = null;
                this.CHT_GAUGE.ScaleNumbersRadius = 95;
                this.CHT_GAUGE.ScaleNumbersRotation = 0;
                this.CHT_GAUGE.ScaleNumbersStartScaleLine = 0;
                this.CHT_GAUGE.ScaleNumbersStepScaleLines = 1;
                this.CHT_GAUGE.Size = new System.Drawing.Size(205, 180);
                this.CHT_GAUGE.TabIndex = 22;
                this.CHT_GAUGE.Text = "aGauge1";
                this.CHT_GAUGE.Value = 0F;
                // 
                // TACH_GAUGE
                // 
                this.TACH_GAUGE.BaseArcColor = System.Drawing.Color.Gray;
                this.TACH_GAUGE.BaseArcRadius = 80;
                this.TACH_GAUGE.BaseArcStart = 135;
                this.TACH_GAUGE.BaseArcSweep = 270;
                this.TACH_GAUGE.BaseArcWidth = 2;
                this.TACH_GAUGE.Center = new System.Drawing.Point(100, 100);
                aGaugeLabel2.Color = System.Drawing.SystemColors.WindowText;
                aGaugeLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aGaugeLabel2.Name = "GaugeLabel1";
                aGaugeLabel2.Position = new System.Drawing.Point(85, 130);
                aGaugeLabel2.Text = "x1000";
                aGaugeLabel3.Color = System.Drawing.Color.Black;
                aGaugeLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aGaugeLabel3.Name = "GaugeLabel2";
                aGaugeLabel3.Position = new System.Drawing.Point(85, 50);
                aGaugeLabel3.Text = "RPM";
                this.TACH_GAUGE.GaugeLabels.Add(aGaugeLabel2);
                this.TACH_GAUGE.GaugeLabels.Add(aGaugeLabel3);
                aGaugeRange4.Color = System.Drawing.Color.Red;
                aGaugeRange4.EndValue = 10F;
                aGaugeRange4.InnerRadius = 70;
                aGaugeRange4.InRange = false;
                aGaugeRange4.Name = "AlertRange";
                aGaugeRange4.OuterRadius = 80;
                aGaugeRange4.StartValue = 8F;
                aGaugeRange5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                aGaugeRange5.EndValue = 8F;
                aGaugeRange5.InnerRadius = 70;
                aGaugeRange5.InRange = true;
                aGaugeRange5.Name = "GaugeRange3";
                aGaugeRange5.OuterRadius = 75;
                aGaugeRange5.StartValue = 0F;
                aGaugeRange6.Color = System.Drawing.Color.Lime;
                aGaugeRange6.EndValue = 8F;
                aGaugeRange6.InnerRadius = 75;
                aGaugeRange6.InRange = true;
                aGaugeRange6.Name = "GaugeRange2";
                aGaugeRange6.OuterRadius = 80;
                aGaugeRange6.StartValue = 0F;
                this.TACH_GAUGE.GaugeRanges.Add(aGaugeRange4);
                this.TACH_GAUGE.GaugeRanges.Add(aGaugeRange5);
                this.TACH_GAUGE.GaugeRanges.Add(aGaugeRange6);
                this.TACH_GAUGE.Location = new System.Drawing.Point(6, 103);
                this.TACH_GAUGE.MaxValue = 10F;
                this.TACH_GAUGE.MinValue = 0F;
                this.TACH_GAUGE.Name = "TACH_GAUGE";
                this.TACH_GAUGE.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Yellow;
                this.TACH_GAUGE.NeedleColor2 = System.Drawing.Color.Olive;
                this.TACH_GAUGE.NeedleRadius = 80;
                this.TACH_GAUGE.NeedleType = System.Windows.Forms.NeedleType.Advance;
                this.TACH_GAUGE.NeedleWidth = 2;
                this.TACH_GAUGE.ScaleLinesInterColor = System.Drawing.Color.Black;
                this.TACH_GAUGE.ScaleLinesInterInnerRadius = 73;
                this.TACH_GAUGE.ScaleLinesInterOuterRadius = 80;
                this.TACH_GAUGE.ScaleLinesInterWidth = 1;
                this.TACH_GAUGE.ScaleLinesMajorColor = System.Drawing.Color.Black;
                this.TACH_GAUGE.ScaleLinesMajorInnerRadius = 70;
                this.TACH_GAUGE.ScaleLinesMajorOuterRadius = 80;
                this.TACH_GAUGE.ScaleLinesMajorStepValue = 1F;
                this.TACH_GAUGE.ScaleLinesMajorWidth = 2;
                this.TACH_GAUGE.ScaleLinesMinorColor = System.Drawing.Color.Gray;
                this.TACH_GAUGE.ScaleLinesMinorInnerRadius = 75;
                this.TACH_GAUGE.ScaleLinesMinorOuterRadius = 80;
                this.TACH_GAUGE.ScaleLinesMinorTicks = 9;
                this.TACH_GAUGE.ScaleLinesMinorWidth = 1;
                this.TACH_GAUGE.ScaleNumbersColor = System.Drawing.Color.Black;
                this.TACH_GAUGE.ScaleNumbersFormat = null;
                this.TACH_GAUGE.ScaleNumbersRadius = 95;
                this.TACH_GAUGE.ScaleNumbersRotation = 0;
                this.TACH_GAUGE.ScaleNumbersStartScaleLine = 0;
                this.TACH_GAUGE.ScaleNumbersStepScaleLines = 1;
                this.TACH_GAUGE.Size = new System.Drawing.Size(205, 180);
                this.TACH_GAUGE.TabIndex = 8;
                this.TACH_GAUGE.Text = "aGauge1";
                this.TACH_GAUGE.Value = 0F;
                // 
                // MODELabel
                // 
                this.MODELabel.AutoSize = true;
                this.MODELabel.Location = new System.Drawing.Point(253, 63);
                this.MODELabel.Name = "MODELabel";
                this.MODELabel.Size = new System.Drawing.Size(13, 13);
                this.MODELabel.TabIndex = 21;
                this.MODELabel.Text = "0";
                // 
                // label10
                // 
                this.label10.AutoSize = true;
                this.label10.Location = new System.Drawing.Point(208, 63);
                this.label10.Name = "label10";
                this.label10.Size = new System.Drawing.Size(42, 13);
                this.label10.TabIndex = 20;
                this.label10.Text = "MODE:";
                // 
                // label8
                // 
                this.label8.AutoSize = true;
                this.label8.Location = new System.Drawing.Point(206, 18);
                this.label8.Name = "label8";
                this.label8.Size = new System.Drawing.Size(22, 13);
                this.label8.TabIndex = 19;
                this.label8.Text = "OT";
                // 
                // label2
                // 
                this.label2.AutoSize = true;
                this.label2.Location = new System.Drawing.Point(162, 18);
                this.label2.Name = "label2";
                this.label2.Size = new System.Drawing.Size(29, 13);
                this.label2.TabIndex = 18;
                this.label2.Text = "LOG";
                // 
                // label7
                // 
                this.label7.AutoSize = true;
                this.label7.Location = new System.Drawing.Point(124, 18);
                this.label7.Name = "label7";
                this.label7.Size = new System.Drawing.Size(22, 13);
                this.label7.TabIndex = 17;
                this.label7.Text = "VR";
                // 
                // label5
                // 
                this.label5.AutoSize = true;
                this.label5.Location = new System.Drawing.Point(86, 18);
                this.label5.Name = "label5";
                this.label5.Size = new System.Drawing.Size(22, 13);
                this.label5.TabIndex = 16;
                this.label5.Text = "HB";
                // 
                // MAPLabel
                // 
                this.MAPLabel.AutoSize = true;
                this.MAPLabel.Location = new System.Drawing.Point(170, 63);
                this.MAPLabel.Name = "MAPLabel";
                this.MAPLabel.Size = new System.Drawing.Size(13, 13);
                this.MAPLabel.TabIndex = 15;
                this.MAPLabel.Text = "0";
                // 
                // label6
                // 
                this.label6.AutoSize = true;
                this.label6.Location = new System.Drawing.Point(128, 63);
                this.label6.Name = "label6";
                this.label6.Size = new System.Drawing.Size(43, 13);
                this.label6.TabIndex = 14;
                this.label6.Text = "MAP #:";
                // 
                // potStateLabel
                // 
                this.potStateLabel.AutoSize = true;
                this.potStateLabel.Location = new System.Drawing.Point(88, 63);
                this.potStateLabel.Name = "potStateLabel";
                this.potStateLabel.Size = new System.Drawing.Size(13, 13);
                this.potStateLabel.TabIndex = 10;
                this.potStateLabel.Text = "0";
                // 
                // label4
                // 
                this.label4.AutoSize = true;
                this.label4.Location = new System.Drawing.Point(42, 63);
                this.label4.Name = "label4";
                this.label4.Size = new System.Drawing.Size(40, 13);
                this.label4.TabIndex = 9;
                this.label4.Text = "TEMP:";
                // 
                // label1
                // 
                this.label1.AutoSize = true;
                this.label1.Location = new System.Drawing.Point(29, 33);
                this.label1.Name = "label1";
                this.label1.Size = new System.Drawing.Size(53, 13);
                this.label1.TabIndex = 4;
                this.label1.Text = "STATUS:";
                // 
                // VRPictureBox
                // 
                this.VRPictureBox.Image = global::WFF_GenericHID_Demo_Application.Properties.Resources.red_off_16;
                this.VRPictureBox.Location = new System.Drawing.Point(128, 32);
                this.VRPictureBox.Name = "VRPictureBox";
                this.VRPictureBox.Size = new System.Drawing.Size(16, 16);
                this.VRPictureBox.TabIndex = 3;
                this.VRPictureBox.TabStop = false;
                // 
                // HBPictureBox
                // 
                this.HBPictureBox.Image = global::WFF_GenericHID_Demo_Application.Properties.Resources.red_off_16;
                this.HBPictureBox.Location = new System.Drawing.Point(88, 32);
                this.HBPictureBox.Name = "HBPictureBox";
                this.HBPictureBox.Size = new System.Drawing.Size(16, 16);
                this.HBPictureBox.TabIndex = 2;
                this.HBPictureBox.TabStop = false;
                // 
                // led2PictureBox
                // 
                this.led2PictureBox.Image = global::WFF_GenericHID_Demo_Application.Properties.Resources.red_off_16;
                this.led2PictureBox.Location = new System.Drawing.Point(208, 32);
                this.led2PictureBox.Name = "led2PictureBox";
                this.led2PictureBox.Size = new System.Drawing.Size(16, 16);
                this.led2PictureBox.TabIndex = 1;
                this.led2PictureBox.TabStop = false;
                // 
                // LOGPictureBox
                // 
                this.LOGPictureBox.Image = global::WFF_GenericHID_Demo_Application.Properties.Resources.red_off_16;
                this.LOGPictureBox.InitialImage = null;
                this.LOGPictureBox.Location = new System.Drawing.Point(167, 32);
                this.LOGPictureBox.Name = "LOGPictureBox";
                this.LOGPictureBox.Size = new System.Drawing.Size(16, 16);
                this.LOGPictureBox.TabIndex = 0;
                this.LOGPictureBox.TabStop = false;
                this.LOGPictureBox.Click += new System.EventHandler(this.LOGPictureBox_Click);
                // 
                // MAPpanel
                // 
                this.MAPpanel.AutoScroll = true;
                this.MAPpanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                this.MAPpanel.Location = new System.Drawing.Point(2, 42);
                this.MAPpanel.Name = "MAPpanel";
                this.MAPpanel.Size = new System.Drawing.Size(83, 364);
                this.MAPpanel.TabIndex = 24;
                this.MAPpanel.Visible = false;
                // 
                // STATUpDown
                // 
                this.STATUpDown.Location = new System.Drawing.Point(36, 16);
                this.STATUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
                this.STATUpDown.Name = "STATUpDown";
                this.STATUpDown.Size = new System.Drawing.Size(46, 20);
                this.STATUpDown.TabIndex = 25;
                this.STATUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
                this.STATUpDown.Visible = false;
                this.STATUpDown.ValueChanged += new System.EventHandler(this.STATUpDown_ValueChanged);
                // 
                // chart1
                // 
                chartArea1.Name = "ChartArea1";
                this.chart1.ChartAreas.Add(chartArea1);
                legend1.Enabled = false;
                legend1.Name = "Legend1";
                this.chart1.Legends.Add(legend1);
                this.chart1.Location = new System.Drawing.Point(91, 18);
                this.chart1.Name = "chart1";
                series1.ChartArea = "ChartArea1";
                series1.Legend = "Legend1";
                series1.Name = "Series1";
                this.chart1.Series.Add(series1);
                this.chart1.Size = new System.Drawing.Size(705, 388);
                this.chart1.TabIndex = 23;
                this.chart1.Text = "chart1";
                this.chart1.Visible = false;
                // 
                // EGT_txt
                // 
                this.EGT_txt.Location = new System.Drawing.Point(564, 290);
                this.EGT_txt.Name = "EGT_txt";
                this.EGT_txt.Size = new System.Drawing.Size(100, 20);
                this.EGT_txt.TabIndex = 26;
                // 
                // EGT_GAUGE
                // 
                this.EGT_GAUGE.BaseArcColor = System.Drawing.Color.Gray;
                this.EGT_GAUGE.BaseArcRadius = 80;
                this.EGT_GAUGE.BaseArcStart = 135;
                this.EGT_GAUGE.BaseArcSweep = 270;
                this.EGT_GAUGE.BaseArcWidth = 2;
                this.EGT_GAUGE.Center = new System.Drawing.Point(100, 100);
                aGaugeLabel4.Color = System.Drawing.SystemColors.WindowText;
                aGaugeLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                aGaugeLabel4.Name = "GaugeLabel1";
                aGaugeLabel4.Position = new System.Drawing.Point(85, 50);
                aGaugeLabel4.Text = "EGT";
                this.EGT_GAUGE.GaugeLabels.Add(aGaugeLabel4);
                aGaugeRange7.Color = System.Drawing.Color.Lime;
                aGaugeRange7.EndValue = 1500F;
                aGaugeRange7.InnerRadius = 75;
                aGaugeRange7.InRange = false;
                aGaugeRange7.Name = "GaugeRange1";
                aGaugeRange7.OuterRadius = 80;
                aGaugeRange7.StartValue = 0F;
                aGaugeRange8.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
                aGaugeRange8.EndValue = 1500F;
                aGaugeRange8.InnerRadius = 70;
                aGaugeRange8.InRange = false;
                aGaugeRange8.Name = "GaugeRange2";
                aGaugeRange8.OuterRadius = 75;
                aGaugeRange8.StartValue = 0F;
                aGaugeRange9.Color = System.Drawing.Color.Red;
                aGaugeRange9.EndValue = 1800F;
                aGaugeRange9.InnerRadius = 70;
                aGaugeRange9.InRange = false;
                aGaugeRange9.Name = "GaugeRange3";
                aGaugeRange9.OuterRadius = 80;
                aGaugeRange9.StartValue = 1500F;
                this.EGT_GAUGE.GaugeRanges.Add(aGaugeRange7);
                this.EGT_GAUGE.GaugeRanges.Add(aGaugeRange8);
                this.EGT_GAUGE.GaugeRanges.Add(aGaugeRange9);
                this.EGT_GAUGE.Location = new System.Drawing.Point(513, 130);
                this.EGT_GAUGE.MaxValue = 1800F;
                this.EGT_GAUGE.MinValue = 0F;
                this.EGT_GAUGE.Name = "EGT_GAUGE";
                this.EGT_GAUGE.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Yellow;
                this.EGT_GAUGE.NeedleColor2 = System.Drawing.Color.Olive;
                this.EGT_GAUGE.NeedleRadius = 80;
                this.EGT_GAUGE.NeedleType = System.Windows.Forms.NeedleType.Advance;
                this.EGT_GAUGE.NeedleWidth = 2;
                this.EGT_GAUGE.ScaleLinesInterColor = System.Drawing.Color.Black;
                this.EGT_GAUGE.ScaleLinesInterInnerRadius = 73;
                this.EGT_GAUGE.ScaleLinesInterOuterRadius = 80;
                this.EGT_GAUGE.ScaleLinesInterWidth = 1;
                this.EGT_GAUGE.ScaleLinesMajorColor = System.Drawing.Color.Black;
                this.EGT_GAUGE.ScaleLinesMajorInnerRadius = 70;
                this.EGT_GAUGE.ScaleLinesMajorOuterRadius = 80;
                this.EGT_GAUGE.ScaleLinesMajorStepValue = 100F;
                this.EGT_GAUGE.ScaleLinesMajorWidth = 2;
                this.EGT_GAUGE.ScaleLinesMinorColor = System.Drawing.Color.Gray;
                this.EGT_GAUGE.ScaleLinesMinorInnerRadius = 75;
                this.EGT_GAUGE.ScaleLinesMinorOuterRadius = 80;
                this.EGT_GAUGE.ScaleLinesMinorTicks = 10;
                this.EGT_GAUGE.ScaleLinesMinorWidth = 1;
                this.EGT_GAUGE.ScaleNumbersColor = System.Drawing.Color.Black;
                this.EGT_GAUGE.ScaleNumbersFormat = null;
                this.EGT_GAUGE.ScaleNumbersRadius = 95;
                this.EGT_GAUGE.ScaleNumbersRotation = 0;
                this.EGT_GAUGE.ScaleNumbersStartScaleLine = 0;
                this.EGT_GAUGE.ScaleNumbersStepScaleLines = 1;
                this.EGT_GAUGE.Size = new System.Drawing.Size(217, 182);
                this.EGT_GAUGE.TabIndex = 23;
                this.EGT_GAUGE.Text = "EGT";
                this.EGT_GAUGE.Value = 0F;
                // 
                // groupBox2
                // 
                this.groupBox2.Controls.Add(this.debugTextBox);
                this.groupBox2.Location = new System.Drawing.Point(815, 28);
                this.groupBox2.Name = "groupBox2";
                this.groupBox2.Size = new System.Drawing.Size(105, 416);
                this.groupBox2.TabIndex = 3;
                this.groupBox2.TabStop = false;
                this.groupBox2.Text = "Debug";
                // 
                // debugTextBox
                // 
                this.debugTextBox.Location = new System.Drawing.Point(9, 16);
                this.debugTextBox.Multiline = true;
                this.debugTextBox.Name = "debugTextBox";
                this.debugTextBox.ReadOnly = true;
                this.debugTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
                this.debugTextBox.Size = new System.Drawing.Size(90, 390);
                this.debugTextBox.TabIndex = 0;
                // 
                // deviceStatusPollTimer
                // 
                this.deviceStatusPollTimer.Enabled = true;
                this.deviceStatusPollTimer.Interval = 1;
                this.deviceStatusPollTimer.Tick += new System.EventHandler(this.deviceStatusPollTimer_Tick);
                // 
                // RUNbutton
                // 
                this.RUNbutton.Location = new System.Drawing.Point(13, 462);
                this.RUNbutton.Name = "RUNbutton";
                this.RUNbutton.Size = new System.Drawing.Size(89, 25);
                this.RUNbutton.TabIndex = 4;
                this.RUNbutton.Text = "RUN";
                this.RUNbutton.UseVisualStyleBackColor = true;
                this.RUNbutton.Visible = false;
                this.RUNbutton.Click += new System.EventHandler(this.RUNbutton_Click);
                // 
                // LUTLOADbutton
                // 
                this.LUTLOADbutton.Location = new System.Drawing.Point(110, 462);
                this.LUTLOADbutton.Name = "LUTLOADbutton";
                this.LUTLOADbutton.Size = new System.Drawing.Size(89, 25);
                this.LUTLOADbutton.TabIndex = 5;
                this.LUTLOADbutton.Text = "SEND MAP";
                this.LUTLOADbutton.UseVisualStyleBackColor = true;
                this.LUTLOADbutton.Visible = false;
                this.LUTLOADbutton.Click += new System.EventHandler(this.LUTLOADbutton_Click);
                // 
                // FWbutton
                // 
                this.FWbutton.Location = new System.Drawing.Point(327, 462);
                this.FWbutton.Name = "FWbutton";
                this.FWbutton.Size = new System.Drawing.Size(89, 25);
                this.FWbutton.TabIndex = 6;
                this.FWbutton.Text = "FW UPDATE";
                this.FWbutton.UseVisualStyleBackColor = true;
                this.FWbutton.Click += new System.EventHandler(this.FWbutton_Click);
                // 
                // LUTREADbutton
                // 
                this.LUTREADbutton.Location = new System.Drawing.Point(220, 462);
                this.LUTREADbutton.Name = "LUTREADbutton";
                this.LUTREADbutton.Size = new System.Drawing.Size(89, 25);
                this.LUTREADbutton.TabIndex = 7;
                this.LUTREADbutton.Text = "READ MAP";
                this.LUTREADbutton.UseVisualStyleBackColor = true;
                this.LUTREADbutton.Visible = false;
                this.LUTREADbutton.Click += new System.EventHandler(this.LUTREADbutton_Click);
                // 
                // menuStrip1
                // 
                this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem});
                this.menuStrip1.Location = new System.Drawing.Point(0, 0);
                this.menuStrip1.Name = "menuStrip1";
                this.menuStrip1.Size = new System.Drawing.Size(933, 24);
                this.menuStrip1.TabIndex = 8;
                this.menuStrip1.Text = "menuStrip1";
                // 
                // fileToolStripMenuItem
                // 
                this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
                this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
                this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
                this.fileToolStripMenuItem.Text = "File";
                // 
                // newToolStripMenuItem
                // 
                this.newToolStripMenuItem.Name = "newToolStripMenuItem";
                this.newToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
                this.newToolStripMenuItem.Text = "New";
                this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
                // 
                // openToolStripMenuItem
                // 
                this.openToolStripMenuItem.Name = "openToolStripMenuItem";
                this.openToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
                this.openToolStripMenuItem.Text = "Open";
                this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
                // 
                // saveToolStripMenuItem
                // 
                this.saveToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
                this.saveToolStripMenuItem.Size = new System.Drawing.Size(111, 22);
                this.saveToolStripMenuItem.Text = "Save";
                this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
                // 
                // editToolStripMenuItem
                // 
                this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blockFillToolStripMenuItem,
            this.mapNotesToolStripMenuItem});
                this.editToolStripMenuItem.Enabled = false;
                this.editToolStripMenuItem.Name = "editToolStripMenuItem";
                this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
                this.editToolStripMenuItem.Text = "Edit";
                // 
                // blockFillToolStripMenuItem
                // 
                this.blockFillToolStripMenuItem.Name = "blockFillToolStripMenuItem";
                this.blockFillToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
                this.blockFillToolStripMenuItem.Text = "Block Fill";
                this.blockFillToolStripMenuItem.Click += new System.EventHandler(this.blockFillToolStripMenuItem_Click);
                // 
                // mapNotesToolStripMenuItem
                // 
                this.mapNotesToolStripMenuItem.Name = "mapNotesToolStripMenuItem";
                this.mapNotesToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
                this.mapNotesToolStripMenuItem.Text = "Map Notes";
                this.mapNotesToolStripMenuItem.Click += new System.EventHandler(this.mapNotesToolStripMenuItem_Click);
                // 
                // openFileDialog1
                // 
                this.openFileDialog1.FileName = "openFileDialog1";
                // 
                // progressBar1
                // 
                this.progressBar1.Location = new System.Drawing.Point(815, 450);
                this.progressBar1.Name = "progressBar1";
                this.progressBar1.Size = new System.Drawing.Size(100, 23);
                this.progressBar1.TabIndex = 9;
                // 
                // mainForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(933, 515);
                this.Controls.Add(this.EGT_txt);
                this.Controls.Add(this.progressBar1);
                this.Controls.Add(this.EGT_GAUGE);
                this.Controls.Add(this.LUTREADbutton);
                this.Controls.Add(this.FWbutton);
                this.Controls.Add(this.LUTLOADbutton);
                this.Controls.Add(this.RUNbutton);
                this.Controls.Add(this.groupBox2);
                this.Controls.Add(this.statusStrip1);
                this.Controls.Add(this.menuStrip1);
                this.Controls.Add(this.groupBox1);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                this.MainMenuStrip = this.menuStrip1;
                this.MaximizeBox = false;
                this.Name = "mainForm";
                this.ShowIcon = false;
                this.Text = "FireFly Mini";
                this.statusStrip1.ResumeLayout(false);
                this.statusStrip1.PerformLayout();
                this.groupBox1.ResumeLayout(false);
                this.groupBox1.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.VRPictureBox)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.HBPictureBox)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.led2PictureBox)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.LOGPictureBox)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.STATUpDown)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
                this.groupBox2.ResumeLayout(false);
                this.groupBox2.PerformLayout();
                this.menuStrip1.ResumeLayout(false);
                this.menuStrip1.PerformLayout();
                this.ResumeLayout(false);
                this.PerformLayout();

            }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel usbDeviceStatusLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox VRPictureBox;
        private System.Windows.Forms.PictureBox led2PictureBox;
        private System.Windows.Forms.PictureBox LOGPictureBox;
        private System.Windows.Forms.Label potStateLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox debugTextBox;
        private System.Windows.Forms.Timer deviceStatusPollTimer;
        private System.Windows.Forms.PictureBox HBPictureBox;
        private System.Windows.Forms.Label MAPLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label MODELabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button RUNbutton;
        private System.Windows.Forms.Button LUTLOADbutton;
        private System.Windows.Forms.Button FWbutton;
        private System.Windows.Forms.Button LUTREADbutton;
        private System.Windows.Forms.AGauge TACH_GAUGE;
        private System.Windows.Forms.AGauge CHT_GAUGE;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel MAPpanel;
        private System.Windows.Forms.NumericUpDown STATUpDown;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blockFillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapNotesToolStripMenuItem;
        private System.Windows.Forms.AGauge EGT_GAUGE;
        private System.Windows.Forms.TextBox RPM_txt;
        private System.Windows.Forms.TextBox CHT_txt;
        private System.Windows.Forms.TextBox EGT_txt;
        }
    }

