
namespace PoochyEnabler.Forms
{
    partial class TrainerClassEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainerClassEditor));
            this.lblClassIdx = new System.Windows.Forms.Label();
            this.nudClassIdx = new System.Windows.Forms.NumericUpDown();
            this.cmbClassIdx = new System.Windows.Forms.ComboBox();
            this.grpClassData = new System.Windows.Forms.GroupBox();
            this.nudClassPrizeMulti = new System.Windows.Forms.NumericUpDown();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.lblClassPrizeMulti = new System.Windows.Forms.Label();
            this.lblClassName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpExtraData = new System.Windows.Forms.GroupBox();
            this.nudClassBaseIV = new System.Windows.Forms.NumericUpDown();
            this.nudClassPokeBall = new System.Windows.Forms.NumericUpDown();
            this.nudClassBattleMusic = new System.Windows.Forms.NumericUpDown();
            this.nudClassEncounterMusic = new System.Windows.Forms.NumericUpDown();
            this.lblClassBaseIV = new System.Windows.Forms.Label();
            this.lblClassPokeBall = new System.Windows.Forms.Label();
            this.lblClassBattleMusic = new System.Windows.Forms.Label();
            this.lblClassEncounterMusic = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassIdx)).BeginInit();
            this.grpClassData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMulti)).BeginInit();
            this.grpExtraData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassBaseIV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPokeBall)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassBattleMusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassEncounterMusic)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClassIdx
            // 
            this.lblClassIdx.AutoSize = true;
            this.lblClassIdx.Location = new System.Drawing.Point(20, 56);
            this.lblClassIdx.Name = "lblClassIdx";
            this.lblClassIdx.Size = new System.Drawing.Size(25, 12);
            this.lblClassIdx.TabIndex = 1;
            this.lblClassIdx.Text = "No. ";
            // 
            // nudClassIdx
            // 
            this.nudClassIdx.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudClassIdx.Location = new System.Drawing.Point(52, 52);
            this.nudClassIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassIdx.Name = "nudClassIdx";
            this.nudClassIdx.ReadOnly = true;
            this.nudClassIdx.Size = new System.Drawing.Size(56, 19);
            this.nudClassIdx.TabIndex = 2;
            // 
            // cmbClassIdx
            // 
            this.cmbClassIdx.FormattingEnabled = true;
            this.cmbClassIdx.Location = new System.Drawing.Point(124, 52);
            this.cmbClassIdx.Name = "cmbClassIdx";
            this.cmbClassIdx.Size = new System.Drawing.Size(144, 20);
            this.cmbClassIdx.TabIndex = 3;
            // 
            // grpClassData
            // 
            this.grpClassData.Controls.Add(this.nudClassPrizeMulti);
            this.grpClassData.Controls.Add(this.txtClassName);
            this.grpClassData.Controls.Add(this.lblClassPrizeMulti);
            this.grpClassData.Controls.Add(this.lblClassName);
            this.grpClassData.Location = new System.Drawing.Point(20, 82);
            this.grpClassData.Name = "grpClassData";
            this.grpClassData.Size = new System.Drawing.Size(272, 86);
            this.grpClassData.TabIndex = 4;
            this.grpClassData.TabStop = false;
            this.grpClassData.Text = "Class Data";
            // 
            // nudClassPrizeMulti
            // 
            this.nudClassPrizeMulti.Location = new System.Drawing.Point(104, 50);
            this.nudClassPrizeMulti.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassPrizeMulti.Name = "nudClassPrizeMulti";
            this.nudClassPrizeMulti.Size = new System.Drawing.Size(56, 19);
            this.nudClassPrizeMulti.TabIndex = 2;
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(104, 24);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(144, 19);
            this.txtClassName.TabIndex = 1;
            // 
            // lblClassPrizeMulti
            // 
            this.lblClassPrizeMulti.AutoSize = true;
            this.lblClassPrizeMulti.Location = new System.Drawing.Point(20, 54);
            this.lblClassPrizeMulti.Name = "lblClassPrizeMulti";
            this.lblClassPrizeMulti.Size = new System.Drawing.Size(65, 12);
            this.lblClassPrizeMulti.TabIndex = 0;
            this.lblClassPrizeMulti.Text = "Prize Multi :";
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(20, 28);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(73, 12);
            this.lblClassName.TabIndex = 0;
            this.lblClassName.Text = "Class Name :";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(20, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(96, 23);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // grpExtraData
            // 
            this.grpExtraData.Controls.Add(this.nudClassBaseIV);
            this.grpExtraData.Controls.Add(this.nudClassPokeBall);
            this.grpExtraData.Controls.Add(this.nudClassBattleMusic);
            this.grpExtraData.Controls.Add(this.nudClassEncounterMusic);
            this.grpExtraData.Controls.Add(this.lblClassBaseIV);
            this.grpExtraData.Controls.Add(this.lblClassPokeBall);
            this.grpExtraData.Controls.Add(this.lblClassBattleMusic);
            this.grpExtraData.Controls.Add(this.lblClassEncounterMusic);
            this.grpExtraData.Location = new System.Drawing.Point(20, 178);
            this.grpExtraData.Name = "grpExtraData";
            this.grpExtraData.Size = new System.Drawing.Size(220, 140);
            this.grpExtraData.TabIndex = 5;
            this.grpExtraData.TabStop = false;
            this.grpExtraData.Text = "Extra Data";
            // 
            // nudClassBaseIV
            // 
            this.nudClassBaseIV.Location = new System.Drawing.Point(124, 102);
            this.nudClassBaseIV.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassBaseIV.Name = "nudClassBaseIV";
            this.nudClassBaseIV.Size = new System.Drawing.Size(56, 19);
            this.nudClassBaseIV.TabIndex = 2;
            // 
            // nudClassPokeBall
            // 
            this.nudClassPokeBall.Location = new System.Drawing.Point(124, 76);
            this.nudClassPokeBall.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudClassPokeBall.Name = "nudClassPokeBall";
            this.nudClassPokeBall.Size = new System.Drawing.Size(56, 19);
            this.nudClassPokeBall.TabIndex = 2;
            // 
            // nudClassBattleMusic
            // 
            this.nudClassBattleMusic.Location = new System.Drawing.Point(124, 50);
            this.nudClassBattleMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudClassBattleMusic.Name = "nudClassBattleMusic";
            this.nudClassBattleMusic.Size = new System.Drawing.Size(72, 19);
            this.nudClassBattleMusic.TabIndex = 2;
            // 
            // nudClassEncounterMusic
            // 
            this.nudClassEncounterMusic.Location = new System.Drawing.Point(124, 24);
            this.nudClassEncounterMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudClassEncounterMusic.Name = "nudClassEncounterMusic";
            this.nudClassEncounterMusic.Size = new System.Drawing.Size(72, 19);
            this.nudClassEncounterMusic.TabIndex = 2;
            // 
            // lblClassBaseIV
            // 
            this.lblClassBaseIV.AutoSize = true;
            this.lblClassBaseIV.Location = new System.Drawing.Point(20, 106);
            this.lblClassBaseIV.Name = "lblClassBaseIV";
            this.lblClassBaseIV.Size = new System.Drawing.Size(52, 12);
            this.lblClassBaseIV.TabIndex = 0;
            this.lblClassBaseIV.Text = "Base IV :";
            // 
            // lblClassPokeBall
            // 
            this.lblClassPokeBall.AutoSize = true;
            this.lblClassPokeBall.Location = new System.Drawing.Point(20, 80);
            this.lblClassPokeBall.Name = "lblClassPokeBall";
            this.lblClassPokeBall.Size = new System.Drawing.Size(71, 12);
            this.lblClassPokeBall.TabIndex = 0;
            this.lblClassPokeBall.Text = "PokeBall ID :";
            // 
            // lblClassBattleMusic
            // 
            this.lblClassBattleMusic.AutoSize = true;
            this.lblClassBattleMusic.Location = new System.Drawing.Point(20, 54);
            this.lblClassBattleMusic.Name = "lblClassBattleMusic";
            this.lblClassBattleMusic.Size = new System.Drawing.Size(76, 12);
            this.lblClassBattleMusic.TabIndex = 0;
            this.lblClassBattleMusic.Text = "Battle Music :";
            // 
            // lblClassEncounterMusic
            // 
            this.lblClassEncounterMusic.AutoSize = true;
            this.lblClassEncounterMusic.Location = new System.Drawing.Point(20, 28);
            this.lblClassEncounterMusic.Name = "lblClassEncounterMusic";
            this.lblClassEncounterMusic.Size = new System.Drawing.Size(96, 12);
            this.lblClassEncounterMusic.TabIndex = 0;
            this.lblClassEncounterMusic.Text = "Encounter Music :";
            // 
            // TrainerClassEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 335);
            this.Controls.Add(this.grpExtraData);
            this.Controls.Add(this.grpClassData);
            this.Controls.Add(this.cmbClassIdx);
            this.Controls.Add(this.nudClassIdx);
            this.Controls.Add(this.lblClassIdx);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrainerClassEditor";
            this.Text = "TrainerClassEditor";
            ((System.ComponentModel.ISupportInitialize)(this.nudClassIdx)).EndInit();
            this.grpClassData.ResumeLayout(false);
            this.grpClassData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPrizeMulti)).EndInit();
            this.grpExtraData.ResumeLayout(false);
            this.grpExtraData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassBaseIV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassPokeBall)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassBattleMusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassEncounterMusic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClassIdx;
        private System.Windows.Forms.NumericUpDown nudClassIdx;
        private System.Windows.Forms.ComboBox cmbClassIdx;
        private System.Windows.Forms.GroupBox grpClassData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown nudClassPrizeMulti;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label lblClassPrizeMulti;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.GroupBox grpExtraData;
        private System.Windows.Forms.NumericUpDown nudClassBattleMusic;
        private System.Windows.Forms.NumericUpDown nudClassEncounterMusic;
        private System.Windows.Forms.Label lblClassBattleMusic;
        private System.Windows.Forms.Label lblClassEncounterMusic;
        private System.Windows.Forms.NumericUpDown nudClassBaseIV;
        private System.Windows.Forms.NumericUpDown nudClassPokeBall;
        private System.Windows.Forms.Label lblClassBaseIV;
        private System.Windows.Forms.Label lblClassPokeBall;
    }
}