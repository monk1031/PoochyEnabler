
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
            this.bynApply = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryCount)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTargetOffset
            // 
            this.lblTargetOffset.AutoSize = true;
            this.lblTargetOffset.Location = new System.Drawing.Point(20, 20);
            this.lblTargetOffset.Name = "lblTargetOffset";
            this.lblTargetOffset.Size = new System.Drawing.Size(80, 12);
            this.lblTargetOffset.TabIndex = 0;
            this.lblTargetOffset.Text = "Target Offset :";
            // 
            // txtTargetOffset
            // 
            this.txtTargetOffset.Location = new System.Drawing.Point(112, 16);
            this.txtTargetOffset.Name = "txtTargetOffset";
            this.txtTargetOffset.Size = new System.Drawing.Size(80, 19);
            this.txtTargetOffset.TabIndex = 1;
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(20, 44);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(64, 12);
            this.lblDataType.TabIndex = 0;
            this.lblDataType.Text = "Data Type :";
            // 
            // cmbDataType
            // 
            this.cmbDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDataType.FormattingEnabled = true;
            this.cmbDataType.Location = new System.Drawing.Point(112, 40);
            this.cmbDataType.Name = "cmbDataType";
            this.cmbDataType.Size = new System.Drawing.Size(144, 20);
            this.cmbDataType.TabIndex = 2;
            // 
            // lblEntryCount
            // 
            this.lblEntryCount.AutoSize = true;
            this.lblEntryCount.Location = new System.Drawing.Point(20, 68);
            this.lblEntryCount.Name = "lblEntryCount";
            this.lblEntryCount.Size = new System.Drawing.Size(72, 12);
            this.lblEntryCount.TabIndex = 0;
            this.lblEntryCount.Text = "Entry Count :";
            // 
            // nudEntryCount
            // 
            this.nudEntryCount.Location = new System.Drawing.Point(112, 64);
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
            this.lblSelectFile.Location = new System.Drawing.Point(20, 92);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(66, 12);
            this.lblSelectFile.TabIndex = 4;
            this.lblSelectFile.Text = "Select File :";
            // 
            // txtSelectFile
            // 
            this.txtSelectFile.Location = new System.Drawing.Point(112, 88);
            this.txtSelectFile.Name = "txtSelectFile";
            this.txtSelectFile.ReadOnly = true;
            this.txtSelectFile.Size = new System.Drawing.Size(144, 19);
            this.txtSelectFile.TabIndex = 5;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(262, 86);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 23);
            this.btnBrowse.TabIndex = 6;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // bynApply
            // 
            this.bynApply.Location = new System.Drawing.Point(20, 112);
            this.bynApply.Name = "bynApply";
            this.bynApply.Size = new System.Drawing.Size(322, 31);
            this.bynApply.TabIndex = 7;
            this.bynApply.Text = "Apply";
            this.bynApply.UseVisualStyleBackColor = true;
            // 
            // QuickInputPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 159);
            this.Controls.Add(this.bynApply);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSelectFile);
            this.Controls.Add(this.lblSelectFile);
            this.Controls.Add(this.nudEntryCount);
            this.Controls.Add(this.cmbDataType);
            this.Controls.Add(this.txtTargetOffset);
            this.Controls.Add(this.lblEntryCount);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.lblTargetOffset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QuickInputPopup";
            this.Text = "QuickInputPopup";
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Button bynApply;
    }
}