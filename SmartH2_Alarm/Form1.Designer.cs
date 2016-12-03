﻿namespace SmartH2_Alarm
{
    partial class Form_Alarms
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Alarms));
            this.label_app_name = new System.Windows.Forms.Label();
            this.checked_list_box_alarms = new System.Windows.Forms.CheckedListBox();
            this.button_options = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.listViewValues = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label_app_name
            // 
            this.label_app_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_app_name.Location = new System.Drawing.Point(61, 9);
            this.label_app_name.Name = "label_app_name";
            this.label_app_name.Size = new System.Drawing.Size(426, 70);
            this.label_app_name.TabIndex = 0;
            this.label_app_name.Text = "Smart2O Alarms";
            this.label_app_name.Click += new System.EventHandler(this.label_app_name_Click);
            // 
            // checked_list_box_alarms
            // 
            this.checked_list_box_alarms.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checked_list_box_alarms.FormattingEnabled = true;
            this.checked_list_box_alarms.Location = new System.Drawing.Point(27, 165);
            this.checked_list_box_alarms.Name = "checked_list_box_alarms";
            this.checked_list_box_alarms.Size = new System.Drawing.Size(507, 130);
            this.checked_list_box_alarms.TabIndex = 1;
            // 
            // button_options
            // 
            this.button_options.AutoSize = true;
            this.button_options.Image = global::SmartH2_Alarm.Properties.Resources.settings;
            this.button_options.Location = new System.Drawing.Point(493, 66);
            this.button_options.Name = "button_options";
            this.button_options.Size = new System.Drawing.Size(38, 38);
            this.button_options.TabIndex = 2;
            this.button_options.UseVisualStyleBackColor = true;
            this.button_options.Click += new System.EventHandler(this.button_options_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Image = global::SmartH2_Alarm.Properties.Resources.recyclebin_empty__1_;
            this.buttonRemove.Location = new System.Drawing.Point(113, 122);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(39, 37);
            this.buttonRemove.TabIndex = 5;
            this.buttonRemove.UseVisualStyleBackColor = true;
            // 
            // buttonEdit
            // 
            this.buttonEdit.Image = global::SmartH2_Alarm.Properties.Resources.pencil_small;
            this.buttonEdit.Location = new System.Drawing.Point(70, 122);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(37, 37);
            this.buttonEdit.TabIndex = 4;
            this.buttonEdit.UseVisualStyleBackColor = true;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonAdd.Image")));
            this.buttonAdd.Location = new System.Drawing.Point(26, 122);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(38, 37);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.UseVisualStyleBackColor = true;
            // 
            // listViewValues
            // 
            this.listViewValues.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewValues.Location = new System.Drawing.Point(27, 316);
            this.listViewValues.Name = "listViewValues";
            this.listViewValues.Size = new System.Drawing.Size(507, 130);
            this.listViewValues.TabIndex = 6;
            this.listViewValues.UseCompatibleStateImageBehavior = false;
            // 
            // Form_Alarms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(552, 463);
            this.Controls.Add(this.listViewValues);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.button_options);
            this.Controls.Add(this.checked_list_box_alarms);
            this.Controls.Add(this.label_app_name);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form_Alarms";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alarms";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_app_name;
        private System.Windows.Forms.CheckedListBox checked_list_box_alarms;
        private System.Windows.Forms.Button button_options;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonRemove;

        private void label_app_name_Click(object sender, System.EventArgs e)
        {

        }

        private System.Windows.Forms.ListView listViewValues;
    }
}
