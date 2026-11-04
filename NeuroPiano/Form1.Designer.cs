namespace NeuroPiano
{
    partial class fmMain
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlKeyboardHost;
        private System.Windows.Forms.ToolTip toolTipNotes;
        private System.Windows.Forms.ContextMenuStrip menuKeyboard;
        private System.Windows.Forms.ToolStripMenuItem menuShowTooltips;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlKeyboardHost = new System.Windows.Forms.Panel();
            this.toolTipNotes = new System.Windows.Forms.ToolTip(this.components);
            this.menuKeyboard = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuShowTooltips = new System.Windows.Forms.ToolStripMenuItem();
            this.menuKeyboard.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlKeyboardHost
            // 
            this.pnlKeyboardHost.BackColor = System.Drawing.Color.Black;
            this.pnlKeyboardHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlKeyboardHost.Location = new System.Drawing.Point(0, 0);
            this.pnlKeyboardHost.Name = "pnlKeyboardHost";
            this.pnlKeyboardHost.Size = new System.Drawing.Size(1280, 720);
            this.pnlKeyboardHost.TabIndex = 0;
            // 
            // toolTipNotes
            // 
            this.toolTipNotes.AutoPopDelay = 3000;
            // 
            // menuKeyboard
            // 
            this.menuKeyboard.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShowTooltips});
            this.menuKeyboard.Name = "menuKeyboard";
            this.menuKeyboard.Opening += new System.ComponentModel.CancelEventHandler(this.PianoKeyboard_HideNoteToolTip);
            // 
            // menuShowTooltips
            // 
            this.menuShowTooltips.Checked = true;
            this.menuShowTooltips.CheckOnClick = true;
            this.menuShowTooltips.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuShowTooltips.Name = "menuShowTooltips";
            this.menuShowTooltips.Text = "Show tooltips";
            this.menuShowTooltips.CheckedChanged += new System.EventHandler(this.menuShowTooltips_CheckedChanged);
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(13, 13, 14);
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.pnlKeyboardHost);
            this.MinimumSize = new System.Drawing.Size(800, 450);
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NeuroPiano";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmMain_FormClosing);
            this.menuKeyboard.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
