namespace SmartHome
{
    partial class frmMain
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
            this.btnGetDevices = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.txtControllerAddress = new System.Windows.Forms.TextBox();
            this.lblControllerAddress = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.tvwDevices = new System.Windows.Forms.TreeView();
            this.btnPrintDeviceInfo = new System.Windows.Forms.Button();
            this.btnSwitchOn = new System.Windows.Forms.Button();
            this.btnSwitchOff = new System.Windows.Forms.Button();
            this.btnSetLevel = new System.Windows.Forms.Button();
            this.txtLevel = new System.Windows.Forms.MaskedTextBox();
            this.btnDeleteAlerts = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetDevices
            // 
            this.btnGetDevices.Location = new System.Drawing.Point(93, 38);
            this.btnGetDevices.Name = "btnGetDevices";
            this.btnGetDevices.Size = new System.Drawing.Size(75, 23);
            this.btnGetDevices.TabIndex = 0;
            this.btnGetDevices.Text = "Get &Devices";
            this.btnGetDevices.UseVisualStyleBackColor = true;
            this.btnGetDevices.Click += new System.EventHandler(this.btnGetDevices_ClickAsync);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutput.Location = new System.Drawing.Point(324, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(613, 492);
            this.txtOutput.TabIndex = 1;
            this.txtOutput.WordWrap = false;
            // 
            // txtControllerAddress
            // 
            this.txtControllerAddress.Location = new System.Drawing.Point(110, 12);
            this.txtControllerAddress.Name = "txtControllerAddress";
            this.txtControllerAddress.Size = new System.Drawing.Size(208, 20);
            this.txtControllerAddress.TabIndex = 2;
            this.txtControllerAddress.Text = "192.168.1.27";
            this.txtControllerAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblControllerAddress
            // 
            this.lblControllerAddress.AutoSize = true;
            this.lblControllerAddress.Location = new System.Drawing.Point(12, 15);
            this.lblControllerAddress.Name = "lblControllerAddress";
            this.lblControllerAddress.Size = new System.Drawing.Size(92, 13);
            this.lblControllerAddress.TabIndex = 3;
            this.lblControllerAddress.Text = "ControllerAddress:";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(12, 38);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "&Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tvwDevices
            // 
            this.tvwDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.tvwDevices.Location = new System.Drawing.Point(12, 224);
            this.tvwDevices.Name = "tvwDevices";
            this.tvwDevices.ShowNodeToolTips = true;
            this.tvwDevices.Size = new System.Drawing.Size(306, 280);
            this.tvwDevices.TabIndex = 5;
            // 
            // btnPrintDeviceInfo
            // 
            this.btnPrintDeviceInfo.Location = new System.Drawing.Point(174, 38);
            this.btnPrintDeviceInfo.Name = "btnPrintDeviceInfo";
            this.btnPrintDeviceInfo.Size = new System.Drawing.Size(103, 23);
            this.btnPrintDeviceInfo.TabIndex = 6;
            this.btnPrintDeviceInfo.Text = "Print Device &Info";
            this.btnPrintDeviceInfo.UseVisualStyleBackColor = true;
            this.btnPrintDeviceInfo.Click += new System.EventHandler(this.btnPrintDeviceInfo_ClickAsync);
            // 
            // btnSwitchOn
            // 
            this.btnSwitchOn.Location = new System.Drawing.Point(12, 67);
            this.btnSwitchOn.Name = "btnSwitchOn";
            this.btnSwitchOn.Size = new System.Drawing.Size(75, 23);
            this.btnSwitchOn.TabIndex = 7;
            this.btnSwitchOn.Text = "Switch O&n";
            this.btnSwitchOn.UseVisualStyleBackColor = true;
            this.btnSwitchOn.Click += new System.EventHandler(this.btnSwitchOn_ClickAsync);
            // 
            // btnSwitchOff
            // 
            this.btnSwitchOff.Location = new System.Drawing.Point(93, 67);
            this.btnSwitchOff.Name = "btnSwitchOff";
            this.btnSwitchOff.Size = new System.Drawing.Size(75, 23);
            this.btnSwitchOff.TabIndex = 8;
            this.btnSwitchOff.Text = "Switch O&ff";
            this.btnSwitchOff.UseVisualStyleBackColor = true;
            this.btnSwitchOff.Click += new System.EventHandler(this.btnSwitchOff_ClickAsync);
            // 
            // btnSetLevel
            // 
            this.btnSetLevel.Location = new System.Drawing.Point(174, 67);
            this.btnSetLevel.Name = "btnSetLevel";
            this.btnSetLevel.Size = new System.Drawing.Size(75, 23);
            this.btnSetLevel.TabIndex = 9;
            this.btnSetLevel.Text = "Set &Level";
            this.btnSetLevel.UseVisualStyleBackColor = true;
            this.btnSetLevel.Click += new System.EventHandler(this.btnSetLevel_Click);
            // 
            // txtLevel
            // 
            this.txtLevel.Location = new System.Drawing.Point(255, 69);
            this.txtLevel.Mask = "00#";
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(63, 20);
            this.txtLevel.TabIndex = 10;
            this.txtLevel.ValidatingType = typeof(int);
            // 
            // btnDeleteAlerts
            // 
            this.btnDeleteAlerts.Location = new System.Drawing.Point(12, 96);
            this.btnDeleteAlerts.Name = "btnDeleteAlerts";
            this.btnDeleteAlerts.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteAlerts.TabIndex = 11;
            this.btnDeleteAlerts.Text = "Clear &Alerts";
            this.btnDeleteAlerts.UseVisualStyleBackColor = true;
            this.btnDeleteAlerts.Click += new System.EventHandler(this.btnDeleteAlerts_ClickAsync);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 516);
            this.Controls.Add(this.btnDeleteAlerts);
            this.Controls.Add(this.txtLevel);
            this.Controls.Add(this.btnSetLevel);
            this.Controls.Add(this.btnSwitchOff);
            this.Controls.Add(this.btnSwitchOn);
            this.Controls.Add(this.btnPrintDeviceInfo);
            this.Controls.Add(this.tvwDevices);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lblControllerAddress);
            this.Controls.Add(this.txtControllerAddress);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnGetDevices);
            this.Name = "frmMain";
            this.Text = "Smart Home";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetDevices;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.TextBox txtControllerAddress;
        private System.Windows.Forms.Label lblControllerAddress;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TreeView tvwDevices;
        private System.Windows.Forms.Button btnPrintDeviceInfo;
        private System.Windows.Forms.Button btnSwitchOn;
        private System.Windows.Forms.Button btnSwitchOff;
        private System.Windows.Forms.Button btnSetLevel;
        private System.Windows.Forms.MaskedTextBox txtLevel;
        private System.Windows.Forms.Button btnDeleteAlerts;
    }
}

