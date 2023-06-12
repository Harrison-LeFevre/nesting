namespace NestingOpenSource
{
    partial class XFrameNestingControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.PartName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NestCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NestPartsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.MaterialWidthBox = new System.Windows.Forms.ComboBox();
            this.MaterialLengthBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.IterationProgressBar = new System.Windows.Forms.ProgressBar();
            this.IterationLabel = new System.Windows.Forms.Label();
            this.UseMultipleSheetsCheckBox = new System.Windows.Forms.CheckBox();
            this.UseBoundingBoxesCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Parts To Nest";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NestingOpenSource.Properties.Resources.XF_Full_Logo___Black_with_TM_Small;
            this.pictureBox1.Location = new System.Drawing.Point(99, 14);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 60);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PartName,
            this.NestCount});
            this.dataGridView1.Location = new System.Drawing.Point(33, 186);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(236, 338);
            this.dataGridView1.TabIndex = 2;
            // 
            // PartName
            // 
            this.PartName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.PartName.HeaderText = "Part Name";
            this.PartName.Name = "PartName";
            this.PartName.Width = 76;
            // 
            // NestCount
            // 
            this.NestCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.NestCount.HeaderText = "Number to Nest";
            this.NestCount.Name = "NestCount";
            this.NestCount.Width = 78;
            // 
            // NestPartsButton
            // 
            this.NestPartsButton.Location = new System.Drawing.Point(32, 601);
            this.NestPartsButton.Name = "NestPartsButton";
            this.NestPartsButton.Size = new System.Drawing.Size(236, 50);
            this.NestPartsButton.TabIndex = 3;
            this.NestPartsButton.Text = "Nest";
            this.NestPartsButton.UseVisualStyleBackColor = true;
            this.NestPartsButton.Click += new System.EventHandler(this.NestPartsButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Sheet Dimensions";
            // 
            // MaterialWidthBox
            // 
            this.MaterialWidthBox.FormattingEnabled = true;
            this.MaterialWidthBox.Items.AddRange(new object[] {
            "1200",
            "1219",
            "1800"});
            this.MaterialWidthBox.Location = new System.Drawing.Point(49, 102);
            this.MaterialWidthBox.Name = "MaterialWidthBox";
            this.MaterialWidthBox.Size = new System.Drawing.Size(89, 21);
            this.MaterialWidthBox.TabIndex = 5;
            // 
            // MaterialLengthBox
            // 
            this.MaterialLengthBox.FormattingEnabled = true;
            this.MaterialLengthBox.Items.AddRange(new object[] {
            "2400",
            "2438",
            "2700",
            "3000",
            "3600"});
            this.MaterialLengthBox.Location = new System.Drawing.Point(159, 102);
            this.MaterialLengthBox.Name = "MaterialLengthBox";
            this.MaterialLengthBox.Size = new System.Drawing.Size(89, 21);
            this.MaterialLengthBox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "W";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "L";
            // 
            // IterationProgressBar
            // 
            this.IterationProgressBar.Location = new System.Drawing.Point(32, 561);
            this.IterationProgressBar.Name = "IterationProgressBar";
            this.IterationProgressBar.Size = new System.Drawing.Size(237, 23);
            this.IterationProgressBar.TabIndex = 9;
            // 
            // IterationLabel
            // 
            this.IterationLabel.AutoSize = true;
            this.IterationLabel.Location = new System.Drawing.Point(31, 536);
            this.IterationLabel.Name = "IterationLabel";
            this.IterationLabel.Size = new System.Drawing.Size(51, 13);
            this.IterationLabel.TabIndex = 10;
            this.IterationLabel.Text = "Iteration: ";
            // 
            // UseMultipleSheetsCheckBox
            // 
            this.UseMultipleSheetsCheckBox.AutoSize = true;
            this.UseMultipleSheetsCheckBox.Location = new System.Drawing.Point(32, 134);
            this.UseMultipleSheetsCheckBox.Name = "UseMultipleSheetsCheckBox";
            this.UseMultipleSheetsCheckBox.Size = new System.Drawing.Size(120, 17);
            this.UseMultipleSheetsCheckBox.TabIndex = 11;
            this.UseMultipleSheetsCheckBox.Text = "Use Multiple Sheets";
            this.UseMultipleSheetsCheckBox.UseVisualStyleBackColor = true;
            // 
            // UseBoundingBoxesCheckBox
            // 
            this.UseBoundingBoxesCheckBox.AutoSize = true;
            this.UseBoundingBoxesCheckBox.Location = new System.Drawing.Point(159, 134);
            this.UseBoundingBoxesCheckBox.Name = "UseBoundingBoxesCheckBox";
            this.UseBoundingBoxesCheckBox.Size = new System.Drawing.Size(125, 17);
            this.UseBoundingBoxesCheckBox.TabIndex = 12;
            this.UseBoundingBoxesCheckBox.Text = "Use Bounding Boxes";
            this.UseBoundingBoxesCheckBox.UseVisualStyleBackColor = true;
            // 
            // XFrameNestingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.UseBoundingBoxesCheckBox);
            this.Controls.Add(this.UseMultipleSheetsCheckBox);
            this.Controls.Add(this.IterationLabel);
            this.Controls.Add(this.IterationProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MaterialLengthBox);
            this.Controls.Add(this.MaterialWidthBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NestPartsButton);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Name = "XFrameNestingControl";
            this.Size = new System.Drawing.Size(303, 678);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NestCount;
        private System.Windows.Forms.Button NestPartsButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MaterialWidthBox;
        private System.Windows.Forms.ComboBox MaterialLengthBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar IterationProgressBar;
        private System.Windows.Forms.Label IterationLabel;
        private System.Windows.Forms.CheckBox UseMultipleSheetsCheckBox;
        private System.Windows.Forms.CheckBox UseBoundingBoxesCheckBox;
    }
}
