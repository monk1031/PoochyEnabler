
namespace PoochyEnabler.Forms
{
    partial class OverworldEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OverworldEditor));
            this.btnSave = new System.Windows.Forms.Button();
            this.lblTableIdx = new System.Windows.Forms.Label();
            this.nudTableIdx = new System.Windows.Forms.NumericUpDown();
            this.lblEntryIdx = new System.Windows.Forms.Label();
            this.nudEntryIdx = new System.Windows.Forms.NumericUpDown();
            this.lstEntry = new System.Windows.Forms.ListBox();
            this.grpEntryData = new System.Windows.Forms.GroupBox();
            this.btnCreateSpriteTable = new System.Windows.Forms.Button();
            this.txtUnkPtr2 = new System.Windows.Forms.TextBox();
            this.txtShiftRedrawPtr = new System.Windows.Forms.TextBox();
            this.txtSpriteTablePtr = new System.Windows.Forms.TextBox();
            this.txtSizeDrawPtr = new System.Windows.Forms.TextBox();
            this.txtUnkPtr1 = new System.Windows.Forms.TextBox();
            this.chkUnkFlag3 = new System.Windows.Forms.CheckBox();
            this.chkUnkFlag2 = new System.Windows.Forms.CheckBox();
            this.chkUnkFlag1 = new System.Windows.Forms.CheckBox();
            this.nudPaletteSlot = new System.Windows.Forms.NumericUpDown();
            this.nudUnkValue = new System.Windows.Forms.NumericUpDown();
            this.cmbPaletteIdx2 = new System.Windows.Forms.ComboBox();
            this.cmbFrameSize = new System.Windows.Forms.ComboBox();
            this.cmbTextColor = new System.Windows.Forms.ComboBox();
            this.cmbFootPrint = new System.Windows.Forms.ComboBox();
            this.cmbPaletteIdx1 = new System.Windows.Forms.ComboBox();
            this.lblUnkFlags = new System.Windows.Forms.Label();
            this.lblPaletteSlot = new System.Windows.Forms.Label();
            this.lblUnkPtr2 = new System.Windows.Forms.Label();
            this.lblShiftRedrawPtr = new System.Windows.Forms.Label();
            this.lblSpriteTablePtr = new System.Windows.Forms.Label();
            this.lblSizeDrawPtr = new System.Windows.Forms.Label();
            this.lblUnkPtr1 = new System.Windows.Forms.Label();
            this.lblUnkValue = new System.Windows.Forms.Label();
            this.lblFrameSize = new System.Windows.Forms.Label();
            this.lblTextColor = new System.Windows.Forms.Label();
            this.lblFootprint = new System.Windows.Forms.Label();
            this.lblPaletteIdx1 = new System.Windows.Forms.Label();
            this.btnExportData = new System.Windows.Forms.Button();
            this.btnImportData = new System.Windows.Forms.Button();
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.btnExportFrameSprites = new System.Windows.Forms.Button();
            this.btnImportFrameSprites = new System.Windows.Forms.Button();
            this.txtFrameOffset = new System.Windows.Forms.TextBox();
            this.nudPreviewIdx = new System.Windows.Forms.NumericUpDown();
            this.btnPreviewNext = new System.Windows.Forms.Button();
            this.btnPreviewPrev = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.grpPaletteTable = new System.Windows.Forms.GroupBox();
            this.btnCreatePalette = new System.Windows.Forms.Button();
            this.picPalettePreview = new System.Windows.Forms.PictureBox();
            this.txtPaletteOffset = new System.Windows.Forms.TextBox();
            this.cmbPaletteIdx3 = new System.Windows.Forms.ComboBox();
            this.lblPalettePreview = new System.Windows.Forms.Label();
            this.lblPaletteOffset = new System.Windows.Forms.Label();
            this.lblPaletteIdx2 = new System.Windows.Forms.Label();
            this.txtEntryOffset = new System.Windows.Forms.TextBox();
            this.lblEntryOffset = new System.Windows.Forms.Label();
            this.btnCreateEntry = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudTableIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryIdx)).BeginInit();
            this.grpEntryData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPaletteSlot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUnkValue)).BeginInit();
            this.grpPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreviewIdx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.grpPaletteTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPalettePreview)).BeginInit();
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
            // lblTableIdx
            // 
            this.lblTableIdx.AutoSize = true;
            this.lblTableIdx.Location = new System.Drawing.Point(20, 50);
            this.lblTableIdx.Name = "lblTableIdx";
            this.lblTableIdx.Size = new System.Drawing.Size(70, 12);
            this.lblTableIdx.TabIndex = 1;
            this.lblTableIdx.Text = "Table Index :";
            // 
            // nudTableIdx
            // 
            this.nudTableIdx.Location = new System.Drawing.Point(156, 46);
            this.nudTableIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudTableIdx.Name = "nudTableIdx";
            this.nudTableIdx.Size = new System.Drawing.Size(56, 19);
            this.nudTableIdx.TabIndex = 2;
            // 
            // lblEntryIdx
            // 
            this.lblEntryIdx.AutoSize = true;
            this.lblEntryIdx.Location = new System.Drawing.Point(20, 74);
            this.lblEntryIdx.Name = "lblEntryIdx";
            this.lblEntryIdx.Size = new System.Drawing.Size(116, 12);
            this.lblEntryIdx.TabIndex = 1;
            this.lblEntryIdx.Text = "Entries -1 (unused?) :";
            // 
            // nudEntryIdx
            // 
            this.nudEntryIdx.Location = new System.Drawing.Point(156, 70);
            this.nudEntryIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudEntryIdx.Name = "nudEntryIdx";
            this.nudEntryIdx.Size = new System.Drawing.Size(56, 19);
            this.nudEntryIdx.TabIndex = 2;
            // 
            // lstEntry
            // 
            this.lstEntry.FormattingEnabled = true;
            this.lstEntry.ItemHeight = 12;
            this.lstEntry.Location = new System.Drawing.Point(20, 100);
            this.lstEntry.Name = "lstEntry";
            this.lstEntry.ScrollAlwaysVisible = true;
            this.lstEntry.Size = new System.Drawing.Size(192, 292);
            this.lstEntry.TabIndex = 3;
            // 
            // grpEntryData
            // 
            this.grpEntryData.Controls.Add(this.btnCreateSpriteTable);
            this.grpEntryData.Controls.Add(this.txtUnkPtr2);
            this.grpEntryData.Controls.Add(this.txtShiftRedrawPtr);
            this.grpEntryData.Controls.Add(this.txtSpriteTablePtr);
            this.grpEntryData.Controls.Add(this.txtSizeDrawPtr);
            this.grpEntryData.Controls.Add(this.txtUnkPtr1);
            this.grpEntryData.Controls.Add(this.chkUnkFlag3);
            this.grpEntryData.Controls.Add(this.chkUnkFlag2);
            this.grpEntryData.Controls.Add(this.chkUnkFlag1);
            this.grpEntryData.Controls.Add(this.nudPaletteSlot);
            this.grpEntryData.Controls.Add(this.nudUnkValue);
            this.grpEntryData.Controls.Add(this.cmbPaletteIdx2);
            this.grpEntryData.Controls.Add(this.cmbFrameSize);
            this.grpEntryData.Controls.Add(this.cmbTextColor);
            this.grpEntryData.Controls.Add(this.cmbFootPrint);
            this.grpEntryData.Controls.Add(this.cmbPaletteIdx1);
            this.grpEntryData.Controls.Add(this.lblUnkFlags);
            this.grpEntryData.Controls.Add(this.lblPaletteSlot);
            this.grpEntryData.Controls.Add(this.lblUnkPtr2);
            this.grpEntryData.Controls.Add(this.lblShiftRedrawPtr);
            this.grpEntryData.Controls.Add(this.lblSpriteTablePtr);
            this.grpEntryData.Controls.Add(this.lblSizeDrawPtr);
            this.grpEntryData.Controls.Add(this.lblUnkPtr1);
            this.grpEntryData.Controls.Add(this.lblUnkValue);
            this.grpEntryData.Controls.Add(this.lblFrameSize);
            this.grpEntryData.Controls.Add(this.lblTextColor);
            this.grpEntryData.Controls.Add(this.lblFootprint);
            this.grpEntryData.Controls.Add(this.lblPaletteIdx1);
            this.grpEntryData.Controls.Add(this.btnExportData);
            this.grpEntryData.Controls.Add(this.btnImportData);
            this.grpEntryData.Location = new System.Drawing.Point(232, 40);
            this.grpEntryData.Name = "grpEntryData";
            this.grpEntryData.Size = new System.Drawing.Size(316, 398);
            this.grpEntryData.TabIndex = 4;
            this.grpEntryData.TabStop = false;
            this.grpEntryData.Text = "Entry Data";
            // 
            // btnCreateSpriteTable
            // 
            this.btnCreateSpriteTable.Location = new System.Drawing.Point(236, 331);
            this.btnCreateSpriteTable.Name = "btnCreateSpriteTable";
            this.btnCreateSpriteTable.Size = new System.Drawing.Size(56, 23);
            this.btnCreateSpriteTable.TabIndex = 6;
            this.btnCreateSpriteTable.Text = "New";
            this.btnCreateSpriteTable.UseVisualStyleBackColor = true;
            // 
            // txtUnkPtr2
            // 
            this.txtUnkPtr2.Location = new System.Drawing.Point(140, 358);
            this.txtUnkPtr2.Name = "txtUnkPtr2";
            this.txtUnkPtr2.Size = new System.Drawing.Size(88, 19);
            this.txtUnkPtr2.TabIndex = 5;
            // 
            // txtShiftRedrawPtr
            // 
            this.txtShiftRedrawPtr.Location = new System.Drawing.Point(140, 306);
            this.txtShiftRedrawPtr.Name = "txtShiftRedrawPtr";
            this.txtShiftRedrawPtr.Size = new System.Drawing.Size(88, 19);
            this.txtShiftRedrawPtr.TabIndex = 5;
            // 
            // txtSpriteTablePtr
            // 
            this.txtSpriteTablePtr.Location = new System.Drawing.Point(140, 332);
            this.txtSpriteTablePtr.Name = "txtSpriteTablePtr";
            this.txtSpriteTablePtr.ReadOnly = true;
            this.txtSpriteTablePtr.Size = new System.Drawing.Size(88, 19);
            this.txtSpriteTablePtr.TabIndex = 5;
            // 
            // txtSizeDrawPtr
            // 
            this.txtSizeDrawPtr.Location = new System.Drawing.Point(140, 280);
            this.txtSizeDrawPtr.Name = "txtSizeDrawPtr";
            this.txtSizeDrawPtr.Size = new System.Drawing.Size(88, 19);
            this.txtSizeDrawPtr.TabIndex = 5;
            // 
            // txtUnkPtr1
            // 
            this.txtUnkPtr1.Location = new System.Drawing.Point(140, 254);
            this.txtUnkPtr1.Name = "txtUnkPtr1";
            this.txtUnkPtr1.Size = new System.Drawing.Size(88, 19);
            this.txtUnkPtr1.TabIndex = 5;
            // 
            // chkUnkFlag3
            // 
            this.chkUnkFlag3.AutoSize = true;
            this.chkUnkFlag3.Location = new System.Drawing.Point(216, 178);
            this.chkUnkFlag3.Name = "chkUnkFlag3";
            this.chkUnkFlag3.Size = new System.Drawing.Size(30, 16);
            this.chkUnkFlag3.TabIndex = 4;
            this.chkUnkFlag3.Text = "3";
            this.chkUnkFlag3.UseVisualStyleBackColor = true;
            // 
            // chkUnkFlag2
            // 
            this.chkUnkFlag2.AutoSize = true;
            this.chkUnkFlag2.Location = new System.Drawing.Point(178, 178);
            this.chkUnkFlag2.Name = "chkUnkFlag2";
            this.chkUnkFlag2.Size = new System.Drawing.Size(30, 16);
            this.chkUnkFlag2.TabIndex = 4;
            this.chkUnkFlag2.Text = "2";
            this.chkUnkFlag2.UseVisualStyleBackColor = true;
            // 
            // chkUnkFlag1
            // 
            this.chkUnkFlag1.AutoSize = true;
            this.chkUnkFlag1.Location = new System.Drawing.Point(140, 178);
            this.chkUnkFlag1.Name = "chkUnkFlag1";
            this.chkUnkFlag1.Size = new System.Drawing.Size(30, 16);
            this.chkUnkFlag1.TabIndex = 4;
            this.chkUnkFlag1.Text = "1";
            this.chkUnkFlag1.UseVisualStyleBackColor = true;
            // 
            // nudPaletteSlot
            // 
            this.nudPaletteSlot.Location = new System.Drawing.Point(140, 150);
            this.nudPaletteSlot.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudPaletteSlot.Name = "nudPaletteSlot";
            this.nudPaletteSlot.Size = new System.Drawing.Size(72, 19);
            this.nudPaletteSlot.TabIndex = 3;
            // 
            // nudUnkValue
            // 
            this.nudUnkValue.Location = new System.Drawing.Point(140, 98);
            this.nudUnkValue.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudUnkValue.Name = "nudUnkValue";
            this.nudUnkValue.Size = new System.Drawing.Size(72, 19);
            this.nudUnkValue.TabIndex = 3;
            // 
            // cmbPaletteIdx2
            // 
            this.cmbPaletteIdx2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaletteIdx2.FormattingEnabled = true;
            this.cmbPaletteIdx2.Location = new System.Drawing.Point(220, 72);
            this.cmbPaletteIdx2.Name = "cmbPaletteIdx2";
            this.cmbPaletteIdx2.Size = new System.Drawing.Size(72, 20);
            this.cmbPaletteIdx2.TabIndex = 2;
            // 
            // cmbFrameSize
            // 
            this.cmbFrameSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFrameSize.FormattingEnabled = true;
            this.cmbFrameSize.Location = new System.Drawing.Point(140, 124);
            this.cmbFrameSize.Name = "cmbFrameSize";
            this.cmbFrameSize.Size = new System.Drawing.Size(152, 20);
            this.cmbFrameSize.TabIndex = 2;
            // 
            // cmbTextColor
            // 
            this.cmbTextColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTextColor.FormattingEnabled = true;
            this.cmbTextColor.Location = new System.Drawing.Point(140, 228);
            this.cmbTextColor.Name = "cmbTextColor";
            this.cmbTextColor.Size = new System.Drawing.Size(88, 20);
            this.cmbTextColor.TabIndex = 2;
            // 
            // cmbFootPrint
            // 
            this.cmbFootPrint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFootPrint.FormattingEnabled = true;
            this.cmbFootPrint.Location = new System.Drawing.Point(140, 202);
            this.cmbFootPrint.Name = "cmbFootPrint";
            this.cmbFootPrint.Size = new System.Drawing.Size(88, 20);
            this.cmbFootPrint.TabIndex = 2;
            // 
            // cmbPaletteIdx1
            // 
            this.cmbPaletteIdx1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaletteIdx1.FormattingEnabled = true;
            this.cmbPaletteIdx1.Location = new System.Drawing.Point(140, 72);
            this.cmbPaletteIdx1.Name = "cmbPaletteIdx1";
            this.cmbPaletteIdx1.Size = new System.Drawing.Size(72, 20);
            this.cmbPaletteIdx1.TabIndex = 2;
            // 
            // lblUnkFlags
            // 
            this.lblUnkFlags.AutoSize = true;
            this.lblUnkFlags.Location = new System.Drawing.Point(20, 180);
            this.lblUnkFlags.Name = "lblUnkFlags";
            this.lblUnkFlags.Size = new System.Drawing.Size(89, 12);
            this.lblUnkFlags.TabIndex = 1;
            this.lblUnkFlags.Text = "Unknown Flags :";
            // 
            // lblPaletteSlot
            // 
            this.lblPaletteSlot.AutoSize = true;
            this.lblPaletteSlot.Location = new System.Drawing.Point(20, 154);
            this.lblPaletteSlot.Name = "lblPaletteSlot";
            this.lblPaletteSlot.Size = new System.Drawing.Size(71, 12);
            this.lblPaletteSlot.TabIndex = 1;
            this.lblPaletteSlot.Text = "Palette Slot :";
            // 
            // lblUnkPtr2
            // 
            this.lblUnkPtr2.AutoSize = true;
            this.lblUnkPtr2.Location = new System.Drawing.Point(20, 362);
            this.lblUnkPtr2.Name = "lblUnkPtr2";
            this.lblUnkPtr2.Size = new System.Drawing.Size(107, 12);
            this.lblUnkPtr2.TabIndex = 1;
            this.lblUnkPtr2.Text = "Unknown Pointer 2 :";
            // 
            // lblShiftRedrawPtr
            // 
            this.lblShiftRedrawPtr.AutoSize = true;
            this.lblShiftRedrawPtr.Location = new System.Drawing.Point(20, 310);
            this.lblShiftRedrawPtr.Name = "lblShiftRedrawPtr";
            this.lblShiftRedrawPtr.Size = new System.Drawing.Size(115, 12);
            this.lblShiftRedrawPtr.TabIndex = 1;
            this.lblShiftRedrawPtr.Text = "Shift-redraw Pointer :";
            // 
            // lblSpriteTablePtr
            // 
            this.lblSpriteTablePtr.AutoSize = true;
            this.lblSpriteTablePtr.Location = new System.Drawing.Point(20, 336);
            this.lblSpriteTablePtr.Name = "lblSpriteTablePtr";
            this.lblSpriteTablePtr.Size = new System.Drawing.Size(113, 12);
            this.lblSpriteTablePtr.TabIndex = 1;
            this.lblSpriteTablePtr.Text = "Sprite Table Pointer :";
            // 
            // lblSizeDrawPtr
            // 
            this.lblSizeDrawPtr.AutoSize = true;
            this.lblSizeDrawPtr.Location = new System.Drawing.Point(20, 284);
            this.lblSizeDrawPtr.Name = "lblSizeDrawPtr";
            this.lblSizeDrawPtr.Size = new System.Drawing.Size(102, 12);
            this.lblSizeDrawPtr.TabIndex = 1;
            this.lblSizeDrawPtr.Text = "Size-draw Pointer :";
            // 
            // lblUnkPtr1
            // 
            this.lblUnkPtr1.AutoSize = true;
            this.lblUnkPtr1.Location = new System.Drawing.Point(20, 258);
            this.lblUnkPtr1.Name = "lblUnkPtr1";
            this.lblUnkPtr1.Size = new System.Drawing.Size(107, 12);
            this.lblUnkPtr1.TabIndex = 1;
            this.lblUnkPtr1.Text = "Unknown Pointer 1 :";
            // 
            // lblUnkValue
            // 
            this.lblUnkValue.AutoSize = true;
            this.lblUnkValue.Location = new System.Drawing.Point(20, 102);
            this.lblUnkValue.Name = "lblUnkValue";
            this.lblUnkValue.Size = new System.Drawing.Size(90, 12);
            this.lblUnkValue.TabIndex = 1;
            this.lblUnkValue.Text = "Unknown Value :";
            // 
            // lblFrameSize
            // 
            this.lblFrameSize.AutoSize = true;
            this.lblFrameSize.Location = new System.Drawing.Point(20, 128);
            this.lblFrameSize.Name = "lblFrameSize";
            this.lblFrameSize.Size = new System.Drawing.Size(68, 12);
            this.lblFrameSize.TabIndex = 1;
            this.lblFrameSize.Text = "Frame Size :";
            // 
            // lblTextColor
            // 
            this.lblTextColor.AutoSize = true;
            this.lblTextColor.Location = new System.Drawing.Point(20, 232);
            this.lblTextColor.Name = "lblTextColor";
            this.lblTextColor.Size = new System.Drawing.Size(65, 12);
            this.lblTextColor.TabIndex = 1;
            this.lblTextColor.Text = "Text Color :";
            // 
            // lblFootprint
            // 
            this.lblFootprint.AutoSize = true;
            this.lblFootprint.Location = new System.Drawing.Point(20, 206);
            this.lblFootprint.Name = "lblFootprint";
            this.lblFootprint.Size = new System.Drawing.Size(57, 12);
            this.lblFootprint.TabIndex = 1;
            this.lblFootprint.Text = "Footprint :";
            // 
            // lblPaletteIdx1
            // 
            this.lblPaletteIdx1.AutoSize = true;
            this.lblPaletteIdx1.Location = new System.Drawing.Point(20, 76);
            this.lblPaletteIdx1.Name = "lblPaletteIdx1";
            this.lblPaletteIdx1.Size = new System.Drawing.Size(62, 12);
            this.lblPaletteIdx1.TabIndex = 1;
            this.lblPaletteIdx1.Text = "Palette ID :";
            // 
            // btnExportData
            // 
            this.btnExportData.Location = new System.Drawing.Point(124, 30);
            this.btnExportData.Name = "btnExportData";
            this.btnExportData.Size = new System.Drawing.Size(96, 23);
            this.btnExportData.TabIndex = 0;
            this.btnExportData.Text = "Export Data";
            this.btnExportData.UseVisualStyleBackColor = true;
            // 
            // btnImportData
            // 
            this.btnImportData.Location = new System.Drawing.Point(20, 30);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(96, 23);
            this.btnImportData.TabIndex = 0;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            // 
            // grpPreview
            // 
            this.grpPreview.Controls.Add(this.btnExportFrameSprites);
            this.grpPreview.Controls.Add(this.btnImportFrameSprites);
            this.grpPreview.Controls.Add(this.txtFrameOffset);
            this.grpPreview.Controls.Add(this.nudPreviewIdx);
            this.grpPreview.Controls.Add(this.btnPreviewNext);
            this.grpPreview.Controls.Add(this.btnPreviewPrev);
            this.grpPreview.Controls.Add(this.picPreview);
            this.grpPreview.Location = new System.Drawing.Point(566, 40);
            this.grpPreview.Name = "grpPreview";
            this.grpPreview.Size = new System.Drawing.Size(296, 266);
            this.grpPreview.TabIndex = 5;
            this.grpPreview.TabStop = false;
            this.grpPreview.Text = "Preview";
            // 
            // btnExportFrameSprites
            // 
            this.btnExportFrameSprites.Location = new System.Drawing.Point(20, 222);
            this.btnExportFrameSprites.Name = "btnExportFrameSprites";
            this.btnExportFrameSprites.Size = new System.Drawing.Size(256, 23);
            this.btnExportFrameSprites.TabIndex = 4;
            this.btnExportFrameSprites.Text = "Export Frame Sprites";
            this.btnExportFrameSprites.UseVisualStyleBackColor = true;
            // 
            // btnImportFrameSprites
            // 
            this.btnImportFrameSprites.Location = new System.Drawing.Point(20, 194);
            this.btnImportFrameSprites.Name = "btnImportFrameSprites";
            this.btnImportFrameSprites.Size = new System.Drawing.Size(256, 23);
            this.btnImportFrameSprites.TabIndex = 4;
            this.btnImportFrameSprites.Text = "Import Frame Sprites";
            this.btnImportFrameSprites.UseVisualStyleBackColor = true;
            // 
            // txtFrameOffset
            // 
            this.txtFrameOffset.Location = new System.Drawing.Point(196, 166);
            this.txtFrameOffset.Name = "txtFrameOffset";
            this.txtFrameOffset.ReadOnly = true;
            this.txtFrameOffset.Size = new System.Drawing.Size(80, 19);
            this.txtFrameOffset.TabIndex = 3;
            // 
            // nudPreviewIdx
            // 
            this.nudPreviewIdx.Location = new System.Drawing.Point(56, 166);
            this.nudPreviewIdx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudPreviewIdx.Name = "nudPreviewIdx";
            this.nudPreviewIdx.Size = new System.Drawing.Size(56, 19);
            this.nudPreviewIdx.TabIndex = 2;
            // 
            // btnPreviewNext
            // 
            this.btnPreviewNext.Location = new System.Drawing.Point(118, 164);
            this.btnPreviewNext.Name = "btnPreviewNext";
            this.btnPreviewNext.Size = new System.Drawing.Size(30, 23);
            this.btnPreviewNext.TabIndex = 1;
            this.btnPreviewNext.Text = ">";
            this.btnPreviewNext.UseVisualStyleBackColor = true;
            // 
            // btnPreviewPrev
            // 
            this.btnPreviewPrev.Location = new System.Drawing.Point(20, 164);
            this.btnPreviewPrev.Name = "btnPreviewPrev";
            this.btnPreviewPrev.Size = new System.Drawing.Size(30, 23);
            this.btnPreviewPrev.TabIndex = 1;
            this.btnPreviewPrev.Text = "<";
            this.btnPreviewPrev.UseVisualStyleBackColor = true;
            // 
            // picPreview
            // 
            this.picPreview.Location = new System.Drawing.Point(20, 28);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(256, 128);
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            // 
            // grpPaletteTable
            // 
            this.grpPaletteTable.Controls.Add(this.btnCreatePalette);
            this.grpPaletteTable.Controls.Add(this.picPalettePreview);
            this.grpPaletteTable.Controls.Add(this.txtPaletteOffset);
            this.grpPaletteTable.Controls.Add(this.cmbPaletteIdx3);
            this.grpPaletteTable.Controls.Add(this.lblPalettePreview);
            this.grpPaletteTable.Controls.Add(this.lblPaletteOffset);
            this.grpPaletteTable.Controls.Add(this.lblPaletteIdx2);
            this.grpPaletteTable.Location = new System.Drawing.Point(566, 318);
            this.grpPaletteTable.Name = "grpPaletteTable";
            this.grpPaletteTable.Size = new System.Drawing.Size(258, 120);
            this.grpPaletteTable.TabIndex = 6;
            this.grpPaletteTable.TabStop = false;
            this.grpPaletteTable.Text = "Palette Table";
            // 
            // btnCreatePalette
            // 
            this.btnCreatePalette.Location = new System.Drawing.Point(180, 26);
            this.btnCreatePalette.Name = "btnCreatePalette";
            this.btnCreatePalette.Size = new System.Drawing.Size(56, 23);
            this.btnCreatePalette.TabIndex = 8;
            this.btnCreatePalette.Text = "New";
            this.btnCreatePalette.UseVisualStyleBackColor = true;
            // 
            // picPalettePreview
            // 
            this.picPalettePreview.Location = new System.Drawing.Point(92, 80);
            this.picPalettePreview.Name = "picPalettePreview";
            this.picPalettePreview.Size = new System.Drawing.Size(80, 20);
            this.picPalettePreview.TabIndex = 7;
            this.picPalettePreview.TabStop = false;
            // 
            // txtPaletteOffset
            // 
            this.txtPaletteOffset.Location = new System.Drawing.Point(92, 54);
            this.txtPaletteOffset.Name = "txtPaletteOffset";
            this.txtPaletteOffset.ReadOnly = true;
            this.txtPaletteOffset.Size = new System.Drawing.Size(80, 19);
            this.txtPaletteOffset.TabIndex = 6;
            // 
            // cmbPaletteIdx3
            // 
            this.cmbPaletteIdx3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaletteIdx3.FormattingEnabled = true;
            this.cmbPaletteIdx3.Location = new System.Drawing.Point(92, 28);
            this.cmbPaletteIdx3.Name = "cmbPaletteIdx3";
            this.cmbPaletteIdx3.Size = new System.Drawing.Size(80, 20);
            this.cmbPaletteIdx3.TabIndex = 4;
            // 
            // lblPalettePreview
            // 
            this.lblPalettePreview.AutoSize = true;
            this.lblPalettePreview.Location = new System.Drawing.Point(20, 84);
            this.lblPalettePreview.Name = "lblPalettePreview";
            this.lblPalettePreview.Size = new System.Drawing.Size(51, 12);
            this.lblPalettePreview.TabIndex = 3;
            this.lblPalettePreview.Text = "Preview :";
            // 
            // lblPaletteOffset
            // 
            this.lblPaletteOffset.AutoSize = true;
            this.lblPaletteOffset.Location = new System.Drawing.Point(20, 58);
            this.lblPaletteOffset.Name = "lblPaletteOffset";
            this.lblPaletteOffset.Size = new System.Drawing.Size(43, 12);
            this.lblPaletteOffset.TabIndex = 3;
            this.lblPaletteOffset.Text = "Offset :";
            // 
            // lblPaletteIdx2
            // 
            this.lblPaletteIdx2.AutoSize = true;
            this.lblPaletteIdx2.Location = new System.Drawing.Point(20, 32);
            this.lblPaletteIdx2.Name = "lblPaletteIdx2";
            this.lblPaletteIdx2.Size = new System.Drawing.Size(62, 12);
            this.lblPaletteIdx2.TabIndex = 3;
            this.lblPaletteIdx2.Text = "Palette ID :";
            // 
            // txtEntryOffset
            // 
            this.txtEntryOffset.Location = new System.Drawing.Point(68, 400);
            this.txtEntryOffset.Name = "txtEntryOffset";
            this.txtEntryOffset.ReadOnly = true;
            this.txtEntryOffset.Size = new System.Drawing.Size(80, 19);
            this.txtEntryOffset.TabIndex = 8;
            // 
            // lblEntryOffset
            // 
            this.lblEntryOffset.AutoSize = true;
            this.lblEntryOffset.Location = new System.Drawing.Point(20, 404);
            this.lblEntryOffset.Name = "lblEntryOffset";
            this.lblEntryOffset.Size = new System.Drawing.Size(43, 12);
            this.lblEntryOffset.TabIndex = 7;
            this.lblEntryOffset.Text = "Offset :";
            // 
            // btnCreateEntry
            // 
            this.btnCreateEntry.Location = new System.Drawing.Point(156, 398);
            this.btnCreateEntry.Name = "btnCreateEntry";
            this.btnCreateEntry.Size = new System.Drawing.Size(56, 23);
            this.btnCreateEntry.TabIndex = 9;
            this.btnCreateEntry.Text = "New";
            this.btnCreateEntry.UseVisualStyleBackColor = true;
            // 
            // OverworldEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 459);
            this.Controls.Add(this.btnCreateEntry);
            this.Controls.Add(this.txtEntryOffset);
            this.Controls.Add(this.lblEntryOffset);
            this.Controls.Add(this.grpPaletteTable);
            this.Controls.Add(this.grpPreview);
            this.Controls.Add(this.grpEntryData);
            this.Controls.Add(this.lstEntry);
            this.Controls.Add(this.nudEntryIdx);
            this.Controls.Add(this.nudTableIdx);
            this.Controls.Add(this.lblEntryIdx);
            this.Controls.Add(this.lblTableIdx);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "OverworldEditor";
            this.Text = "OverworldEditor";
            ((System.ComponentModel.ISupportInitialize)(this.nudTableIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryIdx)).EndInit();
            this.grpEntryData.ResumeLayout(false);
            this.grpEntryData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPaletteSlot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudUnkValue)).EndInit();
            this.grpPreview.ResumeLayout(false);
            this.grpPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPreviewIdx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.grpPaletteTable.ResumeLayout(false);
            this.grpPaletteTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPalettePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTableIdx;
        private System.Windows.Forms.NumericUpDown nudTableIdx;
        private System.Windows.Forms.Label lblEntryIdx;
        private System.Windows.Forms.NumericUpDown nudEntryIdx;
        private System.Windows.Forms.ListBox lstEntry;
        private System.Windows.Forms.GroupBox grpEntryData;
        private System.Windows.Forms.NumericUpDown nudUnkValue;
        private System.Windows.Forms.ComboBox cmbPaletteIdx2;
        private System.Windows.Forms.ComboBox cmbPaletteIdx1;
        private System.Windows.Forms.Label lblUnkValue;
        private System.Windows.Forms.Label lblPaletteIdx1;
        private System.Windows.Forms.Button btnExportData;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.ComboBox cmbFrameSize;
        private System.Windows.Forms.Label lblFrameSize;
        private System.Windows.Forms.NumericUpDown nudPaletteSlot;
        private System.Windows.Forms.Label lblPaletteSlot;
        private System.Windows.Forms.CheckBox chkUnkFlag3;
        private System.Windows.Forms.CheckBox chkUnkFlag2;
        private System.Windows.Forms.CheckBox chkUnkFlag1;
        private System.Windows.Forms.ComboBox cmbFootPrint;
        private System.Windows.Forms.Label lblUnkFlags;
        private System.Windows.Forms.Label lblFootprint;
        private System.Windows.Forms.TextBox txtUnkPtr1;
        private System.Windows.Forms.Label lblUnkPtr1;
        private System.Windows.Forms.TextBox txtUnkPtr2;
        private System.Windows.Forms.TextBox txtShiftRedrawPtr;
        private System.Windows.Forms.TextBox txtSpriteTablePtr;
        private System.Windows.Forms.TextBox txtSizeDrawPtr;
        private System.Windows.Forms.Label lblUnkPtr2;
        private System.Windows.Forms.Label lblShiftRedrawPtr;
        private System.Windows.Forms.Label lblSpriteTablePtr;
        private System.Windows.Forms.Label lblSizeDrawPtr;
        private System.Windows.Forms.ComboBox cmbTextColor;
        private System.Windows.Forms.Label lblTextColor;
        private System.Windows.Forms.GroupBox grpPreview;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnExportFrameSprites;
        private System.Windows.Forms.Button btnImportFrameSprites;
        private System.Windows.Forms.TextBox txtFrameOffset;
        private System.Windows.Forms.NumericUpDown nudPreviewIdx;
        private System.Windows.Forms.Button btnPreviewNext;
        private System.Windows.Forms.Button btnPreviewPrev;
        private System.Windows.Forms.Button btnCreateSpriteTable;
        private System.Windows.Forms.GroupBox grpPaletteTable;
        private System.Windows.Forms.ComboBox cmbPaletteIdx3;
        private System.Windows.Forms.Label lblPaletteIdx2;
        private System.Windows.Forms.Button btnCreatePalette;
        private System.Windows.Forms.PictureBox picPalettePreview;
        private System.Windows.Forms.TextBox txtPaletteOffset;
        private System.Windows.Forms.Label lblPalettePreview;
        private System.Windows.Forms.Label lblPaletteOffset;
        private System.Windows.Forms.TextBox txtEntryOffset;
        private System.Windows.Forms.Label lblEntryOffset;
        private System.Windows.Forms.Button btnCreateEntry;
    }
}