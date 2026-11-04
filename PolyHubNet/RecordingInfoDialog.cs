using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using PolyHubIpc;

namespace PolyHubNet
{
    public partial class RecordingInfoDialog : Form
    {
        public RecordingInfoDialog() { InitializeComponent(); }
        public RecordingInfoDialog(string filePath, SessionReader reader) : this()
        {
            Text = "Recording info — " + Path.GetFileName(filePath);
            var h = reader.Header;
            var info = reader.AcquisitionInfo;
            double seconds = Math.Max(0, (reader.GetLastSourceQpc() - reader.GetFirstSourceQpc()) / (double)h.QpcFrequency);
            summary.Text = "File: " + filePath + Environment.NewLine +
                "PHS " + h.Version + " | Duration: " + TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss\.fff") +
                " | Channels: " + h.ChannelCount + Environment.NewLine +
                SignalAcquisitionInfo.Status(h.SampleRate, info) + Environment.NewLine +
                "Impedance measured: " + (info == null || info.ImpedanceMeasuredUtcTicks == 0 ? "not recorded" :
                    new DateTime(info.ImpedanceMeasuredUtcTicks, DateTimeKind.Utc).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss zzz"));
            if (info == null || info.PhysicalInputs.Length == 0)
                summary.AppendText(Environment.NewLine + "Physical input metadata: not recorded");
            else foreach (var input in info.PhysicalInputs)
            {
                string port = input.Kind == PhysicalInputKind.Ground ? "GND" :
                    (input.Kind == PhysicalInputKind.Eeg ? "EEG " : "AUX ") + input.Number;
                string role = RoleText(input.Role, input.Kind);
                string impedance = info.ImpedanceMeasuredUtcTicks > 0 && input.Kind != PhysicalInputKind.Aux &&
                    input.ImpedanceOhm != uint.MaxValue ? (input.ImpedanceOhm / 1000.0).ToString("0.###", CultureInfo.InvariantCulture) : "—";
                inputs.Rows.Add(port, input.Name, role, impedance);
            }
        }
        private static string RoleText(PhysicalInputRole role, PhysicalInputKind kind)
        {
            var names = new System.Collections.Generic.List<string>();
            if ((role & PhysicalInputRole.Signal) != 0) names.Add(kind == PhysicalInputKind.Aux ? "AUX" : "EEG");
            if ((role & PhysicalInputRole.Reference) != 0) names.Add("REF");
            if ((role & PhysicalInputRole.HeogLeft) != 0) names.Add("HEOG-L");
            if ((role & PhysicalInputRole.HeogRight) != 0) names.Add("HEOG-R");
            if ((role & PhysicalInputRole.VeogUp) != 0) names.Add("VEOG-U");
            if ((role & PhysicalInputRole.VeogDown) != 0) names.Add("VEOG-D");
            if (kind == PhysicalInputKind.Ground) names.Add("GND");
            return string.Join(", ", names);
        }
        private void copy_Click(object sender, EventArgs e)
        {
            var text = new StringBuilder(summary.Text);
            text.AppendLine().AppendLine().AppendLine("Input\tName\tRole\tImpedance (kΩ)");
            foreach (DataGridViewRow row in inputs.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++) { if (i > 0) text.Append('\t'); text.Append(row.Cells[i].Value); }
                text.AppendLine();
            }
            try { Clipboard.SetText(text.ToString()); }
            catch (System.Runtime.InteropServices.ExternalException)
            { MessageBox.Show(this, "Clipboard is unavailable.", "Recording info", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }
    }
}
