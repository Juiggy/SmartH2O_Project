namespace SmartH2O_SeeApp
{
    partial class Form1
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
            this.label_app_name = new System.Windows.Forms.Label();
            this.lbAlarme = new System.Windows.Forms.ListBox();
            this.btSensor = new System.Windows.Forms.Button();
            this.btAlarm = new System.Windows.Forms.Button();
            this.dtAlarm1 = new System.Windows.Forms.DateTimePicker();
            this.dtAlarm2 = new System.Windows.Forms.DateTimePicker();
            this.rbAlarm1 = new System.Windows.Forms.RadioButton();
            this.rbAlarm2 = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.clbParameters = new System.Windows.Forms.CheckedListBox();
            this.dtSensor1 = new System.Windows.Forms.DateTimePicker();
            this.dtSensor2 = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.rbSensor2 = new System.Windows.Forms.RadioButton();
            this.rbSensor1 = new System.Windows.Forms.RadioButton();
            this.rbSensor3 = new System.Windows.Forms.RadioButton();
            this.lbWeek = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.lbWeek)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_app_name
            // 
            this.label_app_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_app_name.Location = new System.Drawing.Point(256, 13);
            this.label_app_name.Name = "label_app_name";
            this.label_app_name.Size = new System.Drawing.Size(280, 56);
            this.label_app_name.TabIndex = 0;
            this.label_app_name.Text = "Smart H2O";
            // 
            // lbAlarme
            // 
            this.lbAlarme.FormattingEnabled = true;
            this.lbAlarme.Location = new System.Drawing.Point(15, 243);
            this.lbAlarme.Name = "lbAlarme";
            this.lbAlarme.Size = new System.Drawing.Size(736, 199);
            this.lbAlarme.TabIndex = 2;
            this.lbAlarme.Visible = false;
            // 
            // btSensor
            // 
            this.btSensor.Location = new System.Drawing.Point(228, 197);
            this.btSensor.Name = "btSensor";
            this.btSensor.Size = new System.Drawing.Size(46, 21);
            this.btSensor.TabIndex = 3;
            this.btSensor.Text = "Show";
            this.btSensor.UseVisualStyleBackColor = true;
            this.btSensor.Click += new System.EventHandler(this.btSensor_Click);
            // 
            // btAlarm
            // 
            this.btAlarm.Location = new System.Drawing.Point(448, 197);
            this.btAlarm.Name = "btAlarm";
            this.btAlarm.Size = new System.Drawing.Size(49, 21);
            this.btAlarm.TabIndex = 4;
            this.btAlarm.Text = "Show";
            this.btAlarm.UseVisualStyleBackColor = true;
            this.btAlarm.Click += new System.EventHandler(this.btAlarm_Click);
            // 
            // dtAlarm1
            // 
            this.dtAlarm1.Location = new System.Drawing.Point(528, 178);
            this.dtAlarm1.Name = "dtAlarm1";
            this.dtAlarm1.Size = new System.Drawing.Size(157, 20);
            this.dtAlarm1.TabIndex = 5;
            this.dtAlarm1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dtAlarm2
            // 
            this.dtAlarm2.Location = new System.Drawing.Point(528, 211);
            this.dtAlarm2.Name = "dtAlarm2";
            this.dtAlarm2.Size = new System.Drawing.Size(157, 20);
            this.dtAlarm2.TabIndex = 6;
            this.dtAlarm2.Visible = false;
            // 
            // rbAlarm1
            // 
            this.rbAlarm1.AutoSize = true;
            this.rbAlarm1.Checked = true;
            this.rbAlarm1.Location = new System.Drawing.Point(19, 27);
            this.rbAlarm1.Margin = new System.Windows.Forms.Padding(2);
            this.rbAlarm1.Name = "rbAlarm1";
            this.rbAlarm1.Size = new System.Drawing.Size(48, 17);
            this.rbAlarm1.TabIndex = 7;
            this.rbAlarm1.TabStop = true;
            this.rbAlarm1.Text = "Daily";
            this.rbAlarm1.UseVisualStyleBackColor = true;
            // 
            // rbAlarm2
            // 
            this.rbAlarm2.AutoSize = true;
            this.rbAlarm2.Location = new System.Drawing.Point(19, 53);
            this.rbAlarm2.Margin = new System.Windows.Forms.Padding(2);
            this.rbAlarm2.Name = "rbAlarm2";
            this.rbAlarm2.Size = new System.Drawing.Size(98, 17);
            this.rbAlarm2.TabIndex = 8;
            this.rbAlarm2.Text = "Between Dates\r\n";
            this.rbAlarm2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(569, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 20;
            this.label2.Text = "ALARMS";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 44);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(222, 20);
            this.label3.TabIndex = 21;
            this.label3.Text = "SENSOR INFORMATIONS";
            // 
            // clbParameters
            // 
            this.clbParameters.CheckOnClick = true;
            this.clbParameters.Cursor = System.Windows.Forms.Cursors.Default;
            this.clbParameters.FormattingEnabled = true;
            this.clbParameters.Items.AddRange(new object[] {
            "PH",
            "NH3",
            "CI2"});
            this.clbParameters.Location = new System.Drawing.Point(357, 101);
            this.clbParameters.Margin = new System.Windows.Forms.Padding(2);
            this.clbParameters.Name = "clbParameters";
            this.clbParameters.Size = new System.Drawing.Size(58, 49);
            this.clbParameters.TabIndex = 22;
            // 
            // dtSensor1
            // 
            this.dtSensor1.Location = new System.Drawing.Point(23, 178);
            this.dtSensor1.Margin = new System.Windows.Forms.Padding(2);
            this.dtSensor1.Name = "dtSensor1";
            this.dtSensor1.Size = new System.Drawing.Size(158, 20);
            this.dtSensor1.TabIndex = 27;
            // 
            // dtSensor2
            // 
            this.dtSensor2.Location = new System.Drawing.Point(23, 211);
            this.dtSensor2.Margin = new System.Windows.Forms.Padding(2);
            this.dtSensor2.Name = "dtSensor2";
            this.dtSensor2.Size = new System.Drawing.Size(158, 20);
            this.dtSensor2.TabIndex = 28;
            this.dtSensor2.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(316, 72);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Select parameters to show";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // rbSensor2
            // 
            this.rbSensor2.AutoSize = true;
            this.rbSensor2.Location = new System.Drawing.Point(11, 44);
            this.rbSensor2.Margin = new System.Windows.Forms.Padding(2);
            this.rbSensor2.Name = "rbSensor2";
            this.rbSensor2.Size = new System.Drawing.Size(94, 17);
            this.rbSensor2.TabIndex = 31;
            this.rbSensor2.Text = "Week Number";
            this.rbSensor2.UseVisualStyleBackColor = true;
            // 
            // rbSensor1
            // 
            this.rbSensor1.AutoSize = true;
            this.rbSensor1.Checked = true;
            this.rbSensor1.Location = new System.Drawing.Point(11, 18);
            this.rbSensor1.Margin = new System.Windows.Forms.Padding(2);
            this.rbSensor1.Name = "rbSensor1";
            this.rbSensor1.Size = new System.Drawing.Size(83, 17);
            this.rbSensor1.TabIndex = 32;
            this.rbSensor1.TabStop = true;
            this.rbSensor1.Text = "Specific day";
            this.rbSensor1.UseVisualStyleBackColor = true;
            // 
            // rbSensor3
            // 
            this.rbSensor3.AutoSize = true;
            this.rbSensor3.Location = new System.Drawing.Point(11, 71);
            this.rbSensor3.Margin = new System.Windows.Forms.Padding(2);
            this.rbSensor3.Name = "rbSensor3";
            this.rbSensor3.Size = new System.Drawing.Size(101, 17);
            this.rbSensor3.TabIndex = 33;
            this.rbSensor3.Text = "Between  Dates";
            this.rbSensor3.UseVisualStyleBackColor = true;
            // 
            // lbWeek
            // 
            this.lbWeek.Location = new System.Drawing.Point(119, 44);
            this.lbWeek.Margin = new System.Windows.Forms.Padding(2);
            this.lbWeek.Maximum = new decimal(new int[] {
            52,
            0,
            0,
            0});
            this.lbWeek.Name = "lbWeek";
            this.lbWeek.Size = new System.Drawing.Size(60, 20);
            this.lbWeek.TabIndex = 34;
            this.lbWeek.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.lbWeek.Visible = false;
            this.lbWeek.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSensor2);
            this.groupBox1.Controls.Add(this.lbWeek);
            this.groupBox1.Controls.Add(this.rbSensor1);
            this.groupBox1.Controls.Add(this.rbSensor3);
            this.groupBox1.Location = new System.Drawing.Point(12, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 94);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select information type";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbAlarm2);
            this.groupBox2.Controls.Add(this.rbAlarm1);
            this.groupBox2.Location = new System.Drawing.Point(528, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 94);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Alarm information type";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 475);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dtSensor2);
            this.Controls.Add(this.dtSensor1);
            this.Controls.Add(this.clbParameters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtAlarm2);
            this.Controls.Add(this.dtAlarm1);
            this.Controls.Add(this.btAlarm);
            this.Controls.Add(this.btSensor);
            this.Controls.Add(this.lbAlarme);
            this.Controls.Add(this.label_app_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbWeek)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_app_name;
        private System.Windows.Forms.ListBox lbAlarme;
        private System.Windows.Forms.Button btSensor;
        private System.Windows.Forms.Button btAlarm;
        private System.Windows.Forms.DateTimePicker dtAlarm1;
        private System.Windows.Forms.DateTimePicker dtAlarm2;
        private System.Windows.Forms.RadioButton rbAlarm1;
        private System.Windows.Forms.RadioButton rbAlarm2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox clbParameters;
        private System.Windows.Forms.DateTimePicker dtSensor1;
        private System.Windows.Forms.DateTimePicker dtSensor2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rbSensor2;
        private System.Windows.Forms.RadioButton rbSensor1;
        private System.Windows.Forms.RadioButton rbSensor3;
        private System.Windows.Forms.NumericUpDown lbWeek;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

