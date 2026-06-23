
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
            this.btnSave = new System.Windows.Forms.Button();
            this.lblClassName = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.lblPrizeMulti = new System.Windows.Forms.Label();
            this.nudPrizeMulti = new System.Windows.Forms.NumericUpDown();
            this.grpExtraData = new System.Windows.Forms.GroupBox();
            this.lblEncounterMusic = new System.Windows.Forms.Label();
            this.nudEncounterMusic = new System.Windows.Forms.NumericUpDown();
            this.lblBattleMusic = new System.Windows.Forms.Label();
            this.nudBattleMusic = new System.Windows.Forms.NumericUpDown();
            this.lblPokeBallIdx = new System.Windows.Forms.Label();
            this.nudPokeBallIdx = new System.Windows.Forms.NumericUpDown();
            this.lblBaseIV = new System.Windows.Forms.Label();
            this.nudBaseIV = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudClassIdx)).BeginInit();
            this.grpClassData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPrizeMulti)).BeginInit();
            this.grpExtraData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncounterMusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBallIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIV)).BeginInit();
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
            this.grpClassData.Controls.Add(this.nudPrizeMulti);
            this.grpClassData.Controls.Add(this.txtClassName);
            this.grpClassData.Controls.Add(this.lblPrizeMulti);
            this.grpClassData.Controls.Add(this.lblClassName);
            this.grpClassData.Location = new System.Drawing.Point(20, 82);
            this.grpClassData.Name = "grpClassData";
            this.grpClassData.Size = new System.Drawing.Size(272, 86);
            this.grpClassData.TabIndex = 4;
            this.grpClassData.TabStop = false;
            this.grpClassData.Text = "Class Data";
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
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(20, 28);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(73, 12);
            this.lblClassName.TabIndex = 0;
            this.lblClassName.Text = "Class Name :";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(104, 24);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(144, 19);
            this.txtClassName.TabIndex = 1;
            // 
            // lblPrizeMulti
            // 
            this.lblPrizeMulti.AutoSize = true;
            this.lblPrizeMulti.Location = new System.Drawing.Point(20, 54);
            this.lblPrizeMulti.Name = "lblPrizeMulti";
            this.lblPrizeMulti.Size = new System.Drawing.Size(65, 12);
            this.lblPrizeMulti.TabIndex = 0;
            this.lblPrizeMulti.Text = "Prize Multi :";
            // 
            // nudPrizeMulti
            // 
            this.nudPrizeMulti.Location = new System.Drawing.Point(104, 50);
            this.nudPrizeMulti.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudPrizeMulti.Name = "nudPrizeMulti";
            this.nudPrizeMulti.Size = new System.Drawing.Size(56, 19);
            this.nudPrizeMulti.TabIndex = 2;
            // 
            // grpExtraData
            // 
            this.grpExtraData.Controls.Add(this.nudBaseIV);
            this.grpExtraData.Controls.Add(this.nudPokeBallIdx);
            this.grpExtraData.Controls.Add(this.nudBattleMusic);
            this.grpExtraData.Controls.Add(this.nudEncounterMusic);
            this.grpExtraData.Controls.Add(this.lblBaseIV);
            this.grpExtraData.Controls.Add(this.lblPokeBallIdx);
            this.grpExtraData.Controls.Add(this.lblBattleMusic);
            this.grpExtraData.Controls.Add(this.lblEncounterMusic);
            this.grpExtraData.Location = new System.Drawing.Point(20, 178);
            this.grpExtraData.Name = "grpExtraData";
            this.grpExtraData.Size = new System.Drawing.Size(220, 140);
            this.grpExtraData.TabIndex = 5;
            this.grpExtraData.TabStop = false;
            this.grpExtraData.Text = "Extra Data";
            // 
            // lblEncounterMusic
            // 
            this.lblEncounterMusic.AutoSize = true;
            this.lblEncounterMusic.Location = new System.Drawing.Point(20, 28);
            this.lblEncounterMusic.Name = "lblEncounterMusic";
            this.lblEncounterMusic.Size = new System.Drawing.Size(96, 12);
            this.lblEncounterMusic.TabIndex = 0;
            this.lblEncounterMusic.Text = "Encounter Music :";
            // 
            // nudEncounterMusic
            // 
            this.nudEncounterMusic.Location = new System.Drawing.Point(124, 24);
            this.nudEncounterMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudEncounterMusic.Name = "nudEncounterMusic";
            this.nudEncounterMusic.Size = new System.Drawing.Size(72, 19);
            this.nudEncounterMusic.TabIndex = 2;
            // 
            // lblBattleMusic
            // 
            this.lblBattleMusic.AutoSize = true;
            this.lblBattleMusic.Location = new System.Drawing.Point(20, 54);
            this.lblBattleMusic.Name = "lblBattleMusic";
            this.lblBattleMusic.Size = new System.Drawing.Size(76, 12);
            this.lblBattleMusic.TabIndex = 0;
            this.lblBattleMusic.Text = "Battle Music :";
            // 
            // nudBattleMusic
            // 
            this.nudBattleMusic.Location = new System.Drawing.Point(124, 50);
            this.nudBattleMusic.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudBattleMusic.Name = "nudBattleMusic";
            this.nudBattleMusic.Size = new System.Drawing.Size(72, 19);
            this.nudBattleMusic.TabIndex = 2;
            // 
            // lblPokeBallIdx
            // 
            this.lblPokeBallIdx.AutoSize = true;
            this.lblPokeBallIdx.Location = new System.Drawing.Point(20, 80);
            this.lblPokeBallIdx.Name = "lblPokeBallIdx";
            this.lblPokeBallIdx.Size = new System.Drawing.Size(71, 12);
            this.lblPokeBallIdx.TabIndex = 0;
            this.lblPokeBallIdx.Text = "PokeBall ID :";
            // 
            // nudPokeBallIdx
            // 
            this.nudPokeBallIdx.Location = new System.Drawing.Point(124, 76);
            this.nudPokeBallIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudPokeBallIdx.Name = "nudPokeBallIdx";
            this.nudPokeBallIdx.Size = new System.Drawing.Size(56, 19);
            this.nudPokeBallIdx.TabIndex = 2;
            // 
            // lblBaseIV
            // 
            this.lblBaseIV.AutoSize = true;
            this.lblBaseIV.Location = new System.Drawing.Point(20, 106);
            this.lblBaseIV.Name = "lblBaseIV";
            this.lblBaseIV.Size = new System.Drawing.Size(52, 12);
            this.lblBaseIV.TabIndex = 0;
            this.lblBaseIV.Text = "Base IV :";
            // 
            // nudBaseIV
            // 
            this.nudBaseIV.Location = new System.Drawing.Point(124, 102);
            this.nudBaseIV.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudBaseIV.Name = "nudBaseIV";
            this.nudBaseIV.Size = new System.Drawing.Size(56, 19);
            this.nudBaseIV.TabIndex = 2;
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
            ((System.ComponentModel.ISupportInitialize)(this.nudPrizeMulti)).EndInit();
            this.grpExtraData.ResumeLayout(false);
            this.grpExtraData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudEncounterMusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBattleMusic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPokeBallIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBaseIV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblClassIdx;
        private System.Windows.Forms.NumericUpDown nudClassIdx;
        private System.Windows.Forms.ComboBox cmbClassIdx;
        private System.Windows.Forms.GroupBox grpClassData;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.NumericUpDown nudPrizeMulti;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label lblPrizeMulti;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.GroupBox grpExtraData;
        private System.Windows.Forms.NumericUpDown nudBattleMusic;
        private System.Windows.Forms.NumericUpDown nudEncounterMusic;
        private System.Windows.Forms.Label lblBattleMusic;
        private System.Windows.Forms.Label lblEncounterMusic;
        private System.Windows.Forms.NumericUpDown nudBaseIV;
        private System.Windows.Forms.NumericUpDown nudPokeBallIdx;
        private System.Windows.Forms.Label lblBaseIV;
        private System.Windows.Forms.Label lblPokeBallIdx;
    }
}