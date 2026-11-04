namespace PolyHubNet
{
    partial class RecordingInfoDialog
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.summary = new System.Windows.Forms.RichTextBox();
            this.inputs = new System.Windows.Forms.DataGridView();
            this.inputColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.roleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.impedanceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.copy = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.inputs)).BeginInit();
            this.SuspendLayout();
            this.summary.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.summary.BackColor = System.Drawing.SystemColors.Control;
            this.summary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.summary.DetectUrls = false;
            this.summary.Location = new System.Drawing.Point(16, 16);
            this.summary.Name = "summary";
            this.summary.ReadOnly = true;
            this.summary.Size = new System.Drawing.Size(748, 120);
            this.summary.TabIndex = 0;
            this.summary.Text = "";
            this.inputs.AllowUserToAddRows = false;
            this.inputs.AllowUserToDeleteRows = false;
            this.inputs.AllowUserToResizeRows = false;
            this.inputs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.inputs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.inputs.BackgroundColor = System.Drawing.SystemColors.Window;
            this.inputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.inputs.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.inputs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.inputColumn, this.nameColumn, this.roleColumn, this.impedanceColumn });
            this.inputs.Location = new System.Drawing.Point(16, 146);
            this.inputs.Name = "inputs";
            this.inputs.ReadOnly = true;
            this.inputs.RowHeadersVisible = false;
            this.inputs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.inputs.Size = new System.Drawing.Size(748, 336);
            this.inputs.TabIndex = 1;
            this.inputColumn.HeaderText = "Input";
            this.inputColumn.Name = "inputColumn";
            this.inputColumn.FillWeight = 70;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.FillWeight = 100;
            this.roleColumn.HeaderText = "Role";
            this.roleColumn.Name = "roleColumn";
            this.roleColumn.FillWeight = 110;
            this.impedanceColumn.HeaderText = "Impedance (kΩ)";
            this.impedanceColumn.Name = "impedanceColumn";
            this.impedanceColumn.FillWeight = 90;
            this.copy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.copy.Location = new System.Drawing.Point(594, 496);
            this.copy.Name = "copy";
            this.copy.Size = new System.Drawing.Size(80, 28);
            this.copy.TabIndex = 2;
            this.copy.Text = "Copy";
            this.copy.UseVisualStyleBackColor = true;
            this.copy.Click += new System.EventHandler(this.copy_Click);
            this.close.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(684, 496);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(80, 28);
            this.close.TabIndex = 3;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            this.AcceptButton = this.close;
            this.CancelButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 540);
            this.Controls.Add(this.summary);
            this.Controls.Add(this.inputs);
            this.Controls.Add(this.copy);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(660, 400);
            this.Name = "RecordingInfoDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Recording info";
            ((System.ComponentModel.ISupportInitialize)(this.inputs)).EndInit();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.RichTextBox summary;
        private System.Windows.Forms.DataGridView inputs;
        private System.Windows.Forms.DataGridViewTextBoxColumn inputColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn roleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn impedanceColumn;
        private System.Windows.Forms.Button copy;
        private System.Windows.Forms.Button close;
    }
}
