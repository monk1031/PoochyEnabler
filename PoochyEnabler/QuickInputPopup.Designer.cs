
namespace PoochyEnabler
{
    partial class QuickInputPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QuickInputPopup));
            this.lblTargetOffset = new System.Windows.Forms.Label();
            this.txtTargetOffset = new System.Windows.Forms.TextBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.cmbDataType = new System.Windows.Forms.ComboBox();
            this.lblEntryCount = new System.Windows.Forms.Label();
            this.nudEntryCount = new System.Windows.Forms.NumericUpDown();
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.txtSelectFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.grpInput = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryCount)).BeginInit();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTargetOffset
            // 
            this.lblTargetOffset.AutoSize = true;
            this.lblTargetOffset.Location = new System.Drawing.Point(20, 28);
            this.lblTargetOffset.Name = "lblTargetOffset";
            this.lblTargetOffset.Size = new System.Drawing.Size(80, 12);
            this.lblTargetOffset.TabIndex = 0;
            this.lblTargetOffset.Text = "Target Offset :";
            // 
            // txtTargetOffset
            // 
            this.txtTargetOffset.Location = new System.Drawing.Point(112, 24);
            this.txtTargetOffset.Name = "txtTargetOffset";
            this.txtTargetOffset.Size = new System.Drawing.Size(80, 19);
            this.txtTargetOffset.TabIndex = 1;
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(20, 52);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(64, 12);
            this.lblDataType.TabIndex = 0;
            this.lblDataType.Text = "Data Type :";
            // 
            // cmbDataType
            // 
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Location = new System.Drawing.Point(112, 48);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(144, 20);
            this.cmbDataType.TabIndex = 2;
            // 
            // lblEntryCount
            // 
            this.lblEntryCount.AutoSize = true;
            this.lblEntryCount.Location = new System.Drawing.Point(20, 76);
            this.lblEntryCount.Name = "lblEntryCount";
            this.lblEntryCount.Size = new System.Drawing.Size(72, 12);
            this.lblEntryCount.TabIndex = 0;
            this.lblEntryCount.Text = "Entry Count :";
            // 
            // nudEntryCount
            // 
            this.nudEntryCount.Location = new System.Drawing.Point(112, 72);
            this.nudEntryCount.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudEntryCount.Name = "nudEntryCount";
            this.nudEntryCount.Size = new System.Drawing.Size(56, 19);
            this.nudEntryCount.TabIndex = 3;
            // 
            // lblSelectFile
            // 
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Location = new System.Drawing.Point(20, 100);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(66, 12);
            this.lblSelectFile.TabIndex = 4;
            this.lblSelectFile.Text = "Select File :";
            // 
            // txtSelectFile
            // 
            this.txtSelectFile.Location = new System.Drawing.Point(112, 96);
            this.txtSelectFile.Name = "txtSelectFile";
            this.txtSelectFile.ReadOnly = true;
            this.txtSelectFile.Size = new System.Drawing.Size(144, 19);
            this.txtSelectFile.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(262, 94);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 23);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(20, 162);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(364, 31);
            this.btnApply.TabIndex = 7;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lblTargetOffset);
            this.grpInput.Controls.Add(this.lblDataType);
            this.grpInput.Controls.Add(this.btnBrowse);
            this.grpInput.Controls.Add(this.lblEntryCount);
            this.grpInput.Controls.Add(this.txtSelectFile);
            this.grpInput.Controls.Add(this.txtTargetOffset);
            this.grpInput.Controls.Add(this.lblSelectFile);
            this.grpInput.Controls.Add(this.cmbDataType);
            this.grpInput.Controls.Add(this.nudEntryCount);
            this.grpInput.Location = new System.Drawing.Point(20, 16);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(364, 136);
            this.grpInput.TabIndex = 8;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "Input";
            // 
            // QuickInputPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 211);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QuickInputPopup";
            this.Text = "QuickInputPopup";
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryCount)).EndInit();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTargetOffset;
        private System.Windows.Forms.TextBox txtTargetOffset;
        private System.Windows.Forms.Label lblDataType;
        private System.Windows.Forms.ComboBox cmbDataType;
        private System.Windows.Forms.Label lblEntryCount;
        private System.Windows.Forms.NumericUpDown nudEntryCount;
        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.TextBox txtSelectFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox grpInput;
    }
}