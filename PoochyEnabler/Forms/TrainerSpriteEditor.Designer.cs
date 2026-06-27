
namespace PoochyEnabler.Forms
{
    partial class TrainerSpriteEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrainerSpriteEditor));
            this.btnSave = new System.Windows.Forms.Button();
            this.picSprite = new System.Windows.Forms.PictureBox();
            this.nudSpriteIdx = new System.Windows.Forms.NumericUpDown();
            this.btnSpriteIdxPrev = new System.Windows.Forms.Button();
            this.btnSpriteIdxNext = new System.Windows.Forms.Button();
            this.lblImageOffset = new System.Windows.Forms.Label();
            this.txtImageOffset = new System.Windows.Forms.TextBox();
            this.lblPaletteOffset = new System.Windows.Forms.Label();
            this.txtPaletteOffset = new System.Windows.Forms.TextBox();
            this.lblYPosition = new System.Windows.Forms.Label();
            this.nudYPosition = new System.Windows.Forms.NumericUpDown();
            this.grpAnim = new System.Windows.Forms.GroupBox();
            this.txtAnimDataOffset = new System.Windows.Forms.TextBox();
            this.txtAnimPointer = new System.Windows.Forms.TextBox();
            this.lblAnimDataOffset = new System.Windows.Forms.Label();
            this.lblAnimPointer = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImportImage = new System.Windows.Forms.Button();
            this.btnImportPalette = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picSprite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPosition)).BeginInit();
            this.grpAnim.SuspendLayout();
            this.SuspendLayout();
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
            // picSprite
            // 
            this.picSprite.Location = new System.Drawing.Point(20, 52);
            this.picSprite.Name = "picSprite";
            this.picSprite.Size = new System.Drawing.Size(64, 64);
            this.picSprite.TabIndex = 1;
            this.picSprite.TabStop = false;
            // 
            // nudSpriteIdx
            // 
            this.nudSpriteIdx.Location = new System.Drawing.Point(20, 124);
            this.nudSpriteIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSpriteIdx.Name = "nudSpriteIdx";
            this.nudSpriteIdx.Size = new System.Drawing.Size(64, 19);
            this.nudSpriteIdx.TabIndex = 2;
            // 
            // btnSpriteIdxPrev
            // 
            this.btnSpriteIdxPrev.Location = new System.Drawing.Point(20, 150);
            this.btnSpriteIdxPrev.Name = "btnSpriteIdxPrev";
            this.btnSpriteIdxPrev.Size = new System.Drawing.Size(30, 23);
            this.btnSpriteIdxPrev.TabIndex = 3;
            this.btnSpriteIdxPrev.Text = "<";
            this.btnSpriteIdxPrev.UseVisualStyleBackColor = true;
            // 
            // btnSpriteIdxNext
            // 
            this.btnSpriteIdxNext.Location = new System.Drawing.Point(54, 150);
            this.btnSpriteIdxNext.Name = "btnSpriteIdxNext";
            this.btnSpriteIdxNext.Size = new System.Drawing.Size(30, 23);
            this.btnSpriteIdxNext.TabIndex = 3;
            this.btnSpriteIdxNext.Text = ">";
            this.btnSpriteIdxNext.UseVisualStyleBackColor = true;
            // 
            // lblImageOffset
            // 
            this.lblImageOffset.AutoSize = true;
            this.lblImageOffset.Location = new System.Drawing.Point(104, 52);
            this.lblImageOffset.Name = "lblImageOffset";
            this.lblImageOffset.Size = new System.Drawing.Size(77, 12);
            this.lblImageOffset.TabIndex = 4;
            this.lblImageOffset.Text = "Image Offset :";
            // 
            // txtImageOffset
            // 
            this.txtImageOffset.Location = new System.Drawing.Point(196, 48);
            this.txtImageOffset.Name = "txtImageOffset";
            this.txtImageOffset.Size = new System.Drawing.Size(80, 19);
            this.txtImageOffset.TabIndex = 5;
            // 
            // lblPaletteOffset
            // 
            this.lblPaletteOffset.AutoSize = true;
            this.lblPaletteOffset.Location = new System.Drawing.Point(104, 78);
            this.lblPaletteOffset.Name = "lblPaletteOffset";
            this.lblPaletteOffset.Size = new System.Drawing.Size(83, 12);
            this.lblPaletteOffset.TabIndex = 4;
            this.lblPaletteOffset.Text = "Palette Offset :";
            // 
            // txtPaletteOffset
            // 
            this.txtPaletteOffset.Location = new System.Drawing.Point(196, 74);
            this.txtPaletteOffset.Name = "txtPaletteOffset";
            this.txtPaletteOffset.Size = new System.Drawing.Size(80, 19);
            this.txtPaletteOffset.TabIndex = 5;
            // 
            // lblYPosition
            // 
            this.lblYPosition.AutoSize = true;
            this.lblYPosition.Location = new System.Drawing.Point(104, 104);
            this.lblYPosition.Name = "lblYPosition";
            this.lblYPosition.Size = new System.Drawing.Size(63, 12);
            this.lblYPosition.TabIndex = 4;
            this.lblYPosition.Text = "Y Position :";
            // 
            // nudYPosition
            // 
            this.nudYPosition.Location = new System.Drawing.Point(196, 100);
            this.nudYPosition.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudYPosition.Name = "nudYPosition";
            this.nudYPosition.Size = new System.Drawing.Size(80, 19);
            this.nudYPosition.TabIndex = 6;
            // 
            // grpAnim
            // 
            this.grpAnim.Controls.Add(this.txtAnimDataOffset);
            this.grpAnim.Controls.Add(this.txtAnimPointer);
            this.grpAnim.Controls.Add(this.lblAnimDataOffset);
            this.grpAnim.Controls.Add(this.lblAnimPointer);
            this.grpAnim.Location = new System.Drawing.Point(104, 130);
            this.grpAnim.Name = "grpAnim";
            this.grpAnim.Size = new System.Drawing.Size(208, 86);
            this.grpAnim.TabIndex = 7;
            this.grpAnim.TabStop = false;
            this.grpAnim.Text = "Animation?";
            // 
            // txtAnimDataOffset
            // 
            this.txtAnimDataOffset.Location = new System.Drawing.Point(104, 48);
            this.txtAnimDataOffset.Name = "txtAnimDataOffset";
            this.txtAnimDataOffset.ReadOnly = true;
            this.txtAnimDataOffset.Size = new System.Drawing.Size(80, 19);
            this.txtAnimDataOffset.TabIndex = 8;
            // 
            // txtAnimPointer
            // 
            this.txtAnimPointer.Location = new System.Drawing.Point(104, 24);
            this.txtAnimPointer.Name = "txtAnimPointer";
            this.txtAnimPointer.ReadOnly = true;
            this.txtAnimPointer.Size = new System.Drawing.Size(80, 19);
            this.txtAnimPointer.TabIndex = 9;
            // 
            // lblAnimDataOffset
            // 
            this.lblAnimDataOffset.AutoSize = true;
            this.lblAnimDataOffset.Location = new System.Drawing.Point(20, 52);
            this.lblAnimDataOffset.Name = "lblAnimDataOffset";
            this.lblAnimDataOffset.Size = new System.Drawing.Size(71, 12);
            this.lblAnimDataOffset.TabIndex = 6;
            this.lblAnimDataOffset.Text = "Data Offset :";
            // 
            // lblAnimPointer
            // 
            this.lblAnimPointer.AutoSize = true;
            this.lblAnimPointer.Location = new System.Drawing.Point(20, 28);
            this.lblAnimPointer.Name = "lblAnimPointer";
            this.lblAnimPointer.Size = new System.Drawing.Size(47, 12);
            this.lblAnimPointer.TabIndex = 7;
            this.lblAnimPointer.Text = "Pointer :";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(20, 180);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(64, 23);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnImportImage
            // 
            this.btnImportImage.Location = new System.Drawing.Point(284, 46);
            this.btnImportImage.Name = "btnImportImage";
            this.btnImportImage.Size = new System.Drawing.Size(96, 23);
            this.btnImportImage.TabIndex = 9;
            this.btnImportImage.Text = "Import Image";
            this.btnImportImage.UseVisualStyleBackColor = true;
            // 
            // btnImportPalette
            // 
            this.btnImportPalette.Location = new System.Drawing.Point(284, 72);
            this.btnImportPalette.Name = "btnImportPalette";
            this.btnImportPalette.Size = new System.Drawing.Size(96, 23);
            this.btnImportPalette.TabIndex = 9;
            this.btnImportPalette.Text = "Import Palette";
            this.btnImportPalette.UseVisualStyleBackColor = true;
            // 
            // TrainerSpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 233);
            this.Controls.Add(this.btnImportPalette);
            this.Controls.Add(this.btnImportImage);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.grpAnim);
            this.Controls.Add(this.nudYPosition);
            this.Controls.Add(this.txtPaletteOffset);
            this.Controls.Add(this.txtImageOffset);
            this.Controls.Add(this.lblYPosition);
            this.Controls.Add(this.lblPaletteOffset);
            this.Controls.Add(this.lblImageOffset);
            this.Controls.Add(this.btnSpriteIdxNext);
            this.Controls.Add(this.btnSpriteIdxPrev);
            this.Controls.Add(this.nudSpriteIdx);
            this.Controls.Add(this.picSprite);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TrainerSpriteEditor";
            this.Text = "TrainerSpriteEditor";
            ((System.ComponentModel.ISupportInitialize)(this.picSprite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpriteIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYPosition)).EndInit();
            this.grpAnim.ResumeLayout(false);
            this.grpAnim.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.PictureBox picSprite;
        private System.Windows.Forms.NumericUpDown nudSpriteIdx;
        private System.Windows.Forms.Button btnSpriteIdxPrev;
        private System.Windows.Forms.Button btnSpriteIdxNext;
        private System.Windows.Forms.Label lblImageOffset;
        private System.Windows.Forms.TextBox txtImageOffset;
        private System.Windows.Forms.Label lblPaletteOffset;
        private System.Windows.Forms.TextBox txtPaletteOffset;
        private System.Windows.Forms.Label lblYPosition;
        private System.Windows.Forms.NumericUpDown nudYPosition;
        private System.Windows.Forms.GroupBox grpAnim;
        private System.Windows.Forms.TextBox txtAnimDataOffset;
        private System.Windows.Forms.TextBox txtAnimPointer;
        private System.Windows.Forms.Label lblAnimDataOffset;
        private System.Windows.Forms.Label lblAnimPointer;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImportImage;
        private System.Windows.Forms.Button btnImportPalette;
    }
}