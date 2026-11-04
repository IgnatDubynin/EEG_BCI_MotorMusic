namespace PolyHubNet
{
    partial class fmMain
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
            this.components = new System.ComponentModel.Container();
            this.tabSsvepAnalysisViews = new System.Windows.Forms.TabControl();
            this.tabLiveAnalysis = new System.Windows.Forms.TabPage();
            this.tabPerformance = new System.Windows.Forms.TabPage();
            this.pnlPerformanceCommands = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAcceptanceRadius = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAcceptanceMargin = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAcceptanceNote = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlAcceptanceSave = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPerformanceView = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPerformanceAnalyze = new System.Windows.Forms.Button();
            this.lblAcceptanceRadius = new System.Windows.Forms.Label();
            this.numAcceptanceRadius = new System.Windows.Forms.NumericUpDown();
            this.lblAcceptanceMargin = new System.Windows.Forms.Label();
            this.numAcceptanceMargin = new System.Windows.Forms.NumericUpDown();
            this.lblAcceptanceNote = new System.Windows.Forms.Label();
            this.cmbAcceptanceNote = new System.Windows.Forms.ComboBox();
            this.lblAcceptanceThreshold = new System.Windows.Forms.Label();
            this.numAcceptanceThreshold = new System.Windows.Forms.NumericUpDown();
            this.btnAcceptanceSave = new System.Windows.Forms.Button();
            this.btnAcceptanceReset = new System.Windows.Forms.Button();
            this.lblAcceptanceDirty = new System.Windows.Forms.Label();
            this.chkPerformanceLog = new System.Windows.Forms.CheckBox();
            this.lblPerformanceView = new System.Windows.Forms.Label();
            this.cmbPerformanceView = new System.Windows.Forms.ComboBox();
            this.cmbPerformanceRocMode = new System.Windows.Forms.ComboBox();
            this.lblPerformanceSummary = new System.Windows.Forms.Label();
            this.pnlPerformanceContent = new System.Windows.Forms.Panel();
            this.gridPerformanceSummary = new System.Windows.Forms.DataGridView();
            this.gridPerformanceConfusion = new System.Windows.Forms.DataGridView();
            this.performancePlot = new System.Windows.Forms.Panel();
            this.chkPerformanceSmooth = new System.Windows.Forms.CheckBox();
            this.chkPerformanceShowAll = new System.Windows.Forms.CheckBox();
            this.perfSummaryNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryAttempts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryFirstCorrect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryFirstWrong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryNoSelection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryDetected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryLatency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryAucNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryAucBackground = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfSummaryScored = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote41 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote43 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote45 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote47 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote48 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote50 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote52 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.perfMatrixNote53 = new System.Windows.Forms.DataGridViewTextBoxColumn();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(fmMain));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.grpBxServices = new System.Windows.Forms.GroupBox();
            this.btnDeactivateSavedRoutes = new System.Windows.Forms.Button();
            this.lblDebug = new System.Windows.Forms.Label();
            this.btnApplyAllRoutes = new System.Windows.Forms.Button();
            this.treeViewPipeline = new System.Windows.Forms.TreeView();
            this.btnStartAllServices = new System.Windows.Forms.Button();
            this.dataGridViewEpochs = new System.Windows.Forms.DataGridView();
            this.listBoxEpochClasses = new System.Windows.Forms.ListBox();
            this.treeViewServices = new System.Windows.Forms.TreeView();
            this.grpBxParameters = new System.Windows.Forms.GroupBox();
            this.panelHost = new System.Windows.Forms.Panel();
            this.tabCntrlViewer = new System.Windows.Forms.TabControl();
            this.tabPageSignal = new System.Windows.Forms.TabPage();
            this.grpBxSignalViewer = new System.Windows.Forms.GroupBox();
            this.glCtrlViewer = new OpenTK.GLControl();
            this.numHeogEpochLabel = new System.Windows.Forms.NumericUpDown();
            this.menuHeogEpoch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuHeogStartHere = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHeogAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHeogDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRecordingSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.menuRecordingInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbBxScaleAmpl = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numUpDwnTimeBaseInSec = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblViewerInputPin = new System.Windows.Forms.Label();
            this.tabPageTimeFreq = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.glCtrlTimeFreqViewer = new OpenTK.GLControl();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBxScaleTFAmpl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numUpDwnTimeFBaseInSec = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPageAnalysis = new System.Windows.Forms.TabPage();
            this.splitSsvepAnalysis = new System.Windows.Forms.SplitContainer();
            this.splitSsvepModelDetails = new System.Windows.Forms.SplitContainer();
            this.tblSsvepModel = new System.Windows.Forms.TableLayoutPanel();
            this.tblSsvepModelControls = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSsvepModelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.tblSsvepStatus = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSsvepCalibration = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeogMarkup = new System.Windows.Forms.TableLayoutPanel();
            this.btnHeogMarkup = new System.Windows.Forms.Button();
            this.lblHeogMarkupRange = new System.Windows.Forms.Label();
            this.numHeogMarkupRange = new System.Windows.Forms.NumericUpDown();
            this.lblHeogMarkupLegend = new System.Windows.Forms.Label();
            this.prgHeogMarkup = new System.Windows.Forms.ToolStripProgressBar();
            this.statusHeogMarkup = new System.Windows.Forms.ToolStripStatusLabel();
            this.grpBxAnalysis = new System.Windows.Forms.GroupBox();
            this.glCtrlAnalysis = new OpenTK.GLControl();
            this.grpBxSsvepModel = new System.Windows.Forms.GroupBox();
            this.btnSsvepCalbr1 = new System.Windows.Forms.Button();
            this.btnSsvepCalbr2 = new System.Windows.Forms.Button();
            this.rtbSsvepStatus = new System.Windows.Forms.RichTextBox();
            this.prgSsvepTraining = new System.Windows.Forms.ProgressBar();
            this.lblSsvepModelInfo = new System.Windows.Forms.Label();
            this.lblSsvepActiveModel = new System.Windows.Forms.Label();
            this.btnSsvepDelete = new System.Windows.Forms.Button();
            this.btnSsvepUse = new System.Windows.Forms.Button();
            this.btnSsvepTrain = new System.Windows.Forms.Button();
            this.lstViewSsvepModels = new System.Windows.Forms.ListView();
            this.colSsvepModelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colSsvepModelModified = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.grpBxEogRejection = new System.Windows.Forms.GroupBox();
            this.tblEogRejection = new System.Windows.Forms.TableLayoutPanel();
            this.lblEogRejectionProfile = new System.Windows.Forms.Label();
            this.cmbBxEogRejectionProfile = new System.Windows.Forms.ComboBox();
            this.chkBxVeogRejection = new System.Windows.Forms.CheckBox();
            this.chkBxHeogRejection = new System.Windows.Forms.CheckBox();
            this.pnlVeogThreshold = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeogThreshold = new System.Windows.Forms.FlowLayoutPanel();
            this.numUpDwnVeogThreshold = new System.Windows.Forms.NumericUpDown();
            this.numUpDwnHeogThreshold = new System.Windows.Forms.NumericUpDown();
            this.lblVeogThreshold = new System.Windows.Forms.Label();
            this.lblHeogThreshold = new System.Windows.Forms.Label();
            this.toolTipEogRejection = new System.Windows.Forms.ToolTip(this.components);
            this.tabPageAnalysis2 = new System.Windows.Forms.TabPage();
            this.grpBxAnalysis2 = new System.Windows.Forms.GroupBox();
            this.glCtrlAnalysis2 = new OpenTK.GLControl();
            this.grpBxAnalysis2Controls = new System.Windows.Forms.GroupBox();
            this.lblAnalysis2SensAmpl = new System.Windows.Forms.Label();
            this.cmbBxAnalysis2SensAmpl = new System.Windows.Forms.ComboBox();
            this.cmbBxAnalysis2Mode = new System.Windows.Forms.ComboBox();
            this.lblAnalysis2Mode = new System.Windows.Forms.Label();
            this.cmbBxAnalysis2Component = new System.Windows.Forms.ComboBox();
            this.lblAnalysis2Component = new System.Windows.Forms.Label();
            this.cmbBxAnalysis2View = new System.Windows.Forms.ComboBox();
            this.lblAnalysis2View = new System.Windows.Forms.Label();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusFrequency = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.trackBarPlayback = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlStrpStartRecording = new System.Windows.Forms.ToolStripButton();
            this.tlStrpRecordingIndicator = new System.Windows.Forms.ToolStripButton();
            this.tlStrpStopRecording = new System.Windows.Forms.ToolStripButton();
            this.tlStrpOpenRecording = new System.Windows.Forms.ToolStripButton();
            this.tlStrpPlayRecording = new System.Windows.Forms.ToolStripButton();
            this.tlStrpStopPlayback = new System.Windows.Forms.ToolStripButton();
            this.tlStrpSaveAnalysis = new System.Windows.Forms.ToolStripButton();
            this.lblAnalysisDirty = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPlaybackTotalTime = new System.Windows.Forms.ToolStripLabel();
            this.lblPlaybackCurrentTime = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.grpBxServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEpochs)).BeginInit();
            this.grpBxParameters.SuspendLayout();
            this.tabCntrlViewer.SuspendLayout();
            this.tabPageSignal.SuspendLayout();
            this.grpBxSignalViewer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnTimeBaseInSec)).BeginInit();
            this.tabPageTimeFreq.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnTimeFBaseInSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitSsvepAnalysis)).BeginInit();
            this.splitSsvepAnalysis.Panel1.SuspendLayout();
            this.splitSsvepAnalysis.Panel2.SuspendLayout();
            this.splitSsvepAnalysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitSsvepModelDetails)).BeginInit();
            this.splitSsvepModelDetails.Panel1.SuspendLayout();
            this.splitSsvepModelDetails.Panel2.SuspendLayout();
            this.splitSsvepModelDetails.SuspendLayout();
            this.tblSsvepModel.SuspendLayout();
            this.tblSsvepModelControls.SuspendLayout();
            this.pnlSsvepModelButtons.SuspendLayout();
            this.tblSsvepStatus.SuspendLayout();
            this.pnlSsvepCalibration.SuspendLayout();
            this.pnlHeogMarkup.SuspendLayout();
            this.menuHeogEpoch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeogEpochLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeogMarkupRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceMargin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceRadius)).BeginInit();
            this.tabPageAnalysis.SuspendLayout();
            this.grpBxAnalysis.SuspendLayout();
            this.grpBxSsvepModel.SuspendLayout();
            this.grpBxEogRejection.SuspendLayout();
            this.tblEogRejection.SuspendLayout();
            this.pnlVeogThreshold.SuspendLayout();
            this.pnlHeogThreshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnVeogThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnHeogThreshold)).BeginInit();
            this.tabPageAnalysis2.SuspendLayout();
            this.grpBxAnalysis2.SuspendLayout();
            this.grpBxAnalysis2Controls.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPlayback)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 23);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabCntrlViewer);
            this.splitContainer1.Size = new System.Drawing.Size(1212, 694);
            this.splitContainer1.SplitterDistance = 483;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grpBxServices);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grpBxParameters);
            this.splitContainer2.Size = new System.Drawing.Size(483, 694);
            this.splitContainer2.SplitterDistance = 330;
            this.splitContainer2.TabIndex = 0;
            // 
            // grpBxServices
            // 
            this.grpBxServices.Controls.Add(this.btnDeactivateSavedRoutes);
            this.grpBxServices.Controls.Add(this.lblDebug);
            this.grpBxServices.Controls.Add(this.btnApplyAllRoutes);
            this.grpBxServices.Controls.Add(this.treeViewPipeline);
            this.grpBxServices.Controls.Add(this.btnStartAllServices);
            this.grpBxServices.Controls.Add(this.dataGridViewEpochs);
            this.grpBxServices.Controls.Add(this.listBoxEpochClasses);
            this.grpBxServices.Controls.Add(this.treeViewServices);
            this.grpBxServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxServices.Location = new System.Drawing.Point(0, 0);
            this.grpBxServices.Name = "grpBxServices";
            this.grpBxServices.Size = new System.Drawing.Size(483, 330);
            this.grpBxServices.TabIndex = 2;
            this.grpBxServices.TabStop = false;
            this.grpBxServices.Text = "Services";
            // 
            // btnDeactivateSavedRoutes
            // 
            this.btnDeactivateSavedRoutes.Location = new System.Drawing.Point(224, 184);
            this.btnDeactivateSavedRoutes.Name = "btnDeactivateSavedRoutes";
            this.btnDeactivateSavedRoutes.Size = new System.Drawing.Size(100, 23);
            this.btnDeactivateSavedRoutes.TabIndex = 10;
            this.btnDeactivateSavedRoutes.Text = "DeactAllRoutes";
            this.btnDeactivateSavedRoutes.UseVisualStyleBackColor = true;
            this.btnDeactivateSavedRoutes.Click += new System.EventHandler(this.btnDeactivateSavedRoutes_Click);
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(414, 189);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(49, 13);
            this.lblDebug.TabIndex = 9;
            this.lblDebug.Text = "lblDebug";
            // 
            // btnApplyAllRoutes
            // 
            this.btnApplyAllRoutes.Location = new System.Drawing.Point(118, 184);
            this.btnApplyAllRoutes.Name = "btnApplyAllRoutes";
            this.btnApplyAllRoutes.Size = new System.Drawing.Size(100, 23);
            this.btnApplyAllRoutes.TabIndex = 8;
            this.btnApplyAllRoutes.Text = "ApplyAllRoutes";
            this.btnApplyAllRoutes.UseVisualStyleBackColor = true;
            this.btnApplyAllRoutes.Click += new System.EventHandler(this.btnApplyAllRoutes_Click);
            // 
            // treeViewPipeline
            // 
            this.treeViewPipeline.Location = new System.Drawing.Point(244, 16);
            this.treeViewPipeline.Name = "treeViewPipeline";
            this.treeViewPipeline.Size = new System.Drawing.Size(233, 160);
            this.treeViewPipeline.TabIndex = 7;
            // 
            // btnStartAllServices
            // 
            this.btnStartAllServices.Location = new System.Drawing.Point(12, 184);
            this.btnStartAllServices.Name = "btnStartAllServices";
            this.btnStartAllServices.Size = new System.Drawing.Size(100, 23);
            this.btnStartAllServices.TabIndex = 6;
            this.btnStartAllServices.Text = "StartAllServices";
            this.btnStartAllServices.UseVisualStyleBackColor = true;
            this.btnStartAllServices.Click += new System.EventHandler(this.btnStartAllServices_Click);
            // 
            // dataGridViewEpochs
            // 
            this.dataGridViewEpochs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEpochs.Location = new System.Drawing.Point(6, 213);
            this.dataGridViewEpochs.Name = "dataGridViewEpochs";
            this.dataGridViewEpochs.Size = new System.Drawing.Size(232, 110);
            this.dataGridViewEpochs.TabIndex = 5;
            this.dataGridViewEpochs.SelectionChanged += new System.EventHandler(this.dataGridViewEpochs_SelectionChanged);
            // 
            // listBoxEpochClasses
            // 
            this.listBoxEpochClasses.FormattingEnabled = true;
            this.listBoxEpochClasses.Location = new System.Drawing.Point(244, 213);
            this.listBoxEpochClasses.Name = "listBoxEpochClasses";
            this.listBoxEpochClasses.Size = new System.Drawing.Size(233, 108);
            this.listBoxEpochClasses.TabIndex = 4;
            this.listBoxEpochClasses.SelectedIndexChanged += new System.EventHandler(this.listBoxEpochClasses_SelectedIndexChanged);
            // 
            // treeViewServices
            // 
            this.treeViewServices.Location = new System.Drawing.Point(6, 16);
            this.treeViewServices.Name = "treeViewServices";
            this.treeViewServices.Size = new System.Drawing.Size(232, 160);
            this.treeViewServices.TabIndex = 2;
            this.treeViewServices.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewServices_BeforeCollapse);
            this.treeViewServices.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewServices_BeforeExpand);
            this.treeViewServices.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewServices_AfterSelect);
            this.treeViewServices.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewServices_NodeMouseDoubleClick);
            this.treeViewServices.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewServices_MouseDown);
            this.treeViewServices.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeViewServices_MouseUp);
            // 
            // grpBxParameters
            // 
            this.grpBxParameters.Controls.Add(this.panelHost);
            this.grpBxParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxParameters.Location = new System.Drawing.Point(0, 0);
            this.grpBxParameters.Name = "grpBxParameters";
            this.grpBxParameters.Size = new System.Drawing.Size(483, 360);
            this.grpBxParameters.TabIndex = 3;
            this.grpBxParameters.TabStop = false;
            this.grpBxParameters.Text = "Parameters";
            // 
            // panelHost
            // 
            this.panelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHost.Location = new System.Drawing.Point(3, 16);
            this.panelHost.Name = "panelHost";
            this.panelHost.Size = new System.Drawing.Size(477, 341);
            this.panelHost.TabIndex = 0;
            this.panelHost.Resize += new System.EventHandler(this.panelHost_Resize);
            // 
            // tabCntrlViewer
            // 
            this.tabCntrlViewer.Controls.Add(this.tabPageSignal);
            this.tabCntrlViewer.Controls.Add(this.tabPageTimeFreq);
            this.tabCntrlViewer.Controls.Add(this.tabPageAnalysis);
            this.tabCntrlViewer.Controls.Add(this.tabPageAnalysis2);
            this.tabCntrlViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCntrlViewer.Location = new System.Drawing.Point(0, 0);
            this.tabCntrlViewer.Name = "tabCntrlViewer";
            this.tabCntrlViewer.SelectedIndex = 0;
            this.tabCntrlViewer.Size = new System.Drawing.Size(725, 694);
            this.tabCntrlViewer.TabIndex = 5;
            // 
            // tabPageSignal
            // 
            this.tabPageSignal.Controls.Add(this.grpBxSignalViewer);
            this.tabPageSignal.Location = new System.Drawing.Point(4, 22);
            this.tabPageSignal.Name = "tabPageSignal";
            this.tabPageSignal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSignal.Size = new System.Drawing.Size(717, 668);
            this.tabPageSignal.TabIndex = 0;
            this.tabPageSignal.Text = "TimeSeries";
            this.tabPageSignal.UseVisualStyleBackColor = true;
            // 
            // grpBxSignalViewer
            // 
            this.grpBxSignalViewer.Controls.Add(this.glCtrlViewer);
            this.grpBxSignalViewer.Controls.Add(this.label6);
            this.grpBxSignalViewer.Controls.Add(this.cmbBxScaleAmpl);
            this.grpBxSignalViewer.Controls.Add(this.label7);
            this.grpBxSignalViewer.Controls.Add(this.numUpDwnTimeBaseInSec);
            this.grpBxSignalViewer.Controls.Add(this.label4);
            this.grpBxSignalViewer.Controls.Add(this.label5);
            this.grpBxSignalViewer.Controls.Add(this.lblViewerInputPin);
            this.grpBxSignalViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxSignalViewer.Location = new System.Drawing.Point(3, 3);
            this.grpBxSignalViewer.Name = "grpBxSignalViewer";
            this.grpBxSignalViewer.Size = new System.Drawing.Size(711, 662);
            this.grpBxSignalViewer.TabIndex = 5;
            this.grpBxSignalViewer.TabStop = false;
            this.grpBxSignalViewer.Text = "SignalViewer";
            // 
            // glCtrlViewer
            // 
            this.glCtrlViewer.Controls.Add(this.numHeogEpochLabel);
            this.glCtrlViewer.BackColor = System.Drawing.Color.Black;
            this.glCtrlViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glCtrlViewer.Location = new System.Drawing.Point(3, 16);
            this.glCtrlViewer.Name = "glCtrlViewer";
            this.glCtrlViewer.Size = new System.Drawing.Size(705, 643);
            this.glCtrlViewer.TabIndex = 1;
            this.glCtrlViewer.VSync = false;
            this.glCtrlViewer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.glCtrlViewer_HeogLabelClick);
            this.glCtrlViewer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glCtrlViewer_HeogLabelMouseMove);
            this.glCtrlViewer.Resize += new System.EventHandler(this.glCtrlViewer_HeogLabelResize);
            //
            // numHeogEpochLabel
            //
            this.numHeogEpochLabel.Name = "numHeogEpochLabel";
            this.numHeogEpochLabel.AccessibleName = "Fixation target label";
            this.numHeogEpochLabel.Location = new System.Drawing.Point(80, 32);
            this.numHeogEpochLabel.Size = new System.Drawing.Size(56, 22);
            this.numHeogEpochLabel.Minimum = 0M;
            this.numHeogEpochLabel.Maximum = 53M;
            this.numHeogEpochLabel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numHeogEpochLabel.Visible = false;
            this.numHeogEpochLabel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numHeogEpochLabel_KeyDown);
            this.numHeogEpochLabel.Leave += new System.EventHandler(this.numHeogEpochLabel_Leave);
            //
            // menuHeogEpoch
            //
            this.menuHeogEpoch.Name = "menuHeogEpoch";
            this.menuHeogEpoch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuHeogAdd, this.menuHeogDelete, this.menuHeogStartHere, this.menuRecordingSeparator, this.menuRecordingInfo });
            //
            // menuRecordingInfo
            this.menuRecordingSeparator.Name = "menuRecordingSeparator";
            this.menuRecordingInfo.Name = "menuRecordingInfo";
            this.menuRecordingInfo.Text = "Recording info…";
            this.menuRecordingInfo.Click += new System.EventHandler(this.menuRecordingInfo_Click);
            // menuHeogAdd
            //
            this.menuHeogAdd.Name = "menuHeogAdd";
            this.menuHeogAdd.Text = "Add";
            this.menuHeogAdd.Click += new System.EventHandler(this.menuHeogAdd_Click);
            //
            // menuHeogDelete
            //
            this.menuHeogDelete.Name = "menuHeogDelete";
            this.menuHeogDelete.Text = "Delete";
            this.menuHeogDelete.Click += new System.EventHandler(this.menuHeogDelete_Click);
            //
            // menuHeogStartHere
            //
            this.menuHeogStartHere.Name = "menuHeogStartHere";
            this.menuHeogStartHere.Text = "Start sequence here";
            this.menuHeogStartHere.Click += new System.EventHandler(this.menuHeogStartHere_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(662, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 13);
            this.label6.TabIndex = 39;
            this.label6.Text = "/ Div";
            // 
            // cmbBxScaleAmpl
            // 
            this.cmbBxScaleAmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxScaleAmpl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbBxScaleAmpl.FormattingEnabled = true;
            this.cmbBxScaleAmpl.Items.AddRange(new object[] {
            "0.5 µV",
            "1 µV",
            "2 µV",
            "5 µV",
            "10 µV",
            "20 µV",
            "50 µV",
            "100 µV",
            "200 µV",
            "500 µV",
            "1 mV",
            "2 mV",
            "5 mV",
            "10 mV",
            "20 mV",
            "50 mV",
            "100 mV",
            "200 mV",
            "500 mV",
            "1 V",
            "2 V",
            "5 V"});
            this.cmbBxScaleAmpl.Location = new System.Drawing.Point(581, -3);
            this.cmbBxScaleAmpl.Name = "cmbBxScaleAmpl";
            this.cmbBxScaleAmpl.Size = new System.Drawing.Size(78, 21);
            this.cmbBxScaleAmpl.TabIndex = 40;
            this.cmbBxScaleAmpl.SelectedIndexChanged += new System.EventHandler(this.cmbBxScaleAmpl_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(518, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 38;
            this.label7.Text = "Scale Ampl:";
            // 
            // numUpDwnTimeBaseInSec
            // 
            this.numUpDwnTimeBaseInSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numUpDwnTimeBaseInSec.Location = new System.Drawing.Point(414, -2);
            this.numUpDwnTimeBaseInSec.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numUpDwnTimeBaseInSec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDwnTimeBaseInSec.Name = "numUpDwnTimeBaseInSec";
            this.numUpDwnTimeBaseInSec.Size = new System.Drawing.Size(42, 19);
            this.numUpDwnTimeBaseInSec.TabIndex = 34;
            this.numUpDwnTimeBaseInSec.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDwnTimeBaseInSec.ValueChanged += new System.EventHandler(this.numUpDwnTimeBaseInSec_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(336, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Timebase, sec:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(459, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "/ Page";
            // 
            // lblViewerInputPin
            // 
            this.lblViewerInputPin.Location = new System.Drawing.Point(75, 0);
            this.lblViewerInputPin.Name = "lblViewerInputPin";
            this.lblViewerInputPin.Size = new System.Drawing.Size(261, 15);
            this.lblViewerInputPin.TabIndex = 2;
            this.lblViewerInputPin.Text = "Viewer.In <= <empty>";
            // 
            // tabPageTimeFreq
            // 
            this.tabPageTimeFreq.Controls.Add(this.groupBox1);
            this.tabPageTimeFreq.Location = new System.Drawing.Point(4, 22);
            this.tabPageTimeFreq.Name = "tabPageTimeFreq";
            this.tabPageTimeFreq.Size = new System.Drawing.Size(717, 668);
            this.tabPageTimeFreq.TabIndex = 3;
            this.tabPageTimeFreq.Text = "TimeFreq";
            this.tabPageTimeFreq.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.glCtrlTimeFreqViewer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbBxScaleTFAmpl);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numUpDwnTimeFBaseInSec);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(717, 668);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "FreqViewer";
            // 
            // glCtrlTimeFreqViewer
            // 
            this.glCtrlTimeFreqViewer.BackColor = System.Drawing.Color.Black;
            this.glCtrlTimeFreqViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glCtrlTimeFreqViewer.Location = new System.Drawing.Point(3, 16);
            this.glCtrlTimeFreqViewer.Name = "glCtrlTimeFreqViewer";
            this.glCtrlTimeFreqViewer.Size = new System.Drawing.Size(711, 649);
            this.glCtrlTimeFreqViewer.TabIndex = 1;
            this.glCtrlTimeFreqViewer.VSync = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(662, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "X";
            // 
            // cmbBxScaleTFAmpl
            // 
            this.cmbBxScaleTFAmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxScaleTFAmpl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cmbBxScaleTFAmpl.FormattingEnabled = true;
            this.cmbBxScaleTFAmpl.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cmbBxScaleTFAmpl.Location = new System.Drawing.Point(581, -3);
            this.cmbBxScaleTFAmpl.Name = "cmbBxScaleTFAmpl";
            this.cmbBxScaleTFAmpl.Size = new System.Drawing.Size(78, 21);
            this.cmbBxScaleTFAmpl.TabIndex = 40;
            this.cmbBxScaleTFAmpl.SelectedIndexChanged += new System.EventHandler(this.cmbBxScaleTFAmpl_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(518, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "Sens Ampl:";
            // 
            // numUpDwnTimeFBaseInSec
            // 
            this.numUpDwnTimeFBaseInSec.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.numUpDwnTimeFBaseInSec.Location = new System.Drawing.Point(414, -2);
            this.numUpDwnTimeFBaseInSec.Maximum = new decimal(new int[] {
            160,
            0,
            0,
            0});
            this.numUpDwnTimeFBaseInSec.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDwnTimeFBaseInSec.Name = "numUpDwnTimeFBaseInSec";
            this.numUpDwnTimeFBaseInSec.Size = new System.Drawing.Size(42, 19);
            this.numUpDwnTimeFBaseInSec.TabIndex = 34;
            this.numUpDwnTimeFBaseInSec.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numUpDwnTimeFBaseInSec.ValueChanged += new System.EventHandler(this.numUpDwnTimeFBaseInSec_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(336, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Timebase, sec:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(459, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "/ Page";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(75, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(261, 15);
            this.label9.TabIndex = 2;
            this.label9.Text = "Viewer.In <= <empty>";
            // 
            // tabPageAnalysis
            // 
            this.tabPageAnalysis.Controls.Add(this.splitSsvepAnalysis);
            this.tabPageAnalysis.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalysis.Name = "tabPageAnalysis";
            this.tabPageAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAnalysis.Size = new System.Drawing.Size(717, 668);
            this.tabPageAnalysis.TabIndex = 1;
            this.tabPageAnalysis.Text = "SSVEP Model";
            this.tabPageAnalysis.UseVisualStyleBackColor = true;
            // 
            // grpBxAnalysis
            // 
            this.grpBxAnalysis.Controls.Add(this.glCtrlAnalysis);
            this.grpBxAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxAnalysis.Location = new System.Drawing.Point(3, 277);
            this.grpBxAnalysis.Name = "grpBxAnalysis";
            this.grpBxAnalysis.Size = new System.Drawing.Size(711, 388);
            this.grpBxAnalysis.TabIndex = 2;
            this.grpBxAnalysis.TabStop = false;
            this.grpBxAnalysis.Text = "AnalysisViewer";
            // 
            // glCtrlAnalysis
            // 
            this.glCtrlAnalysis.BackColor = System.Drawing.Color.Black;
            this.glCtrlAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glCtrlAnalysis.Location = new System.Drawing.Point(3, 16);
            this.glCtrlAnalysis.Name = "glCtrlAnalysis";
            this.glCtrlAnalysis.Size = new System.Drawing.Size(705, 369);
            this.glCtrlAnalysis.TabIndex = 1;
            this.glCtrlAnalysis.VSync = false;
            // 
            // grpBxSsvepModel
            // 
            this.grpBxSsvepModel.Controls.Add(this.splitSsvepModelDetails);
            this.grpBxSsvepModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxSsvepModel.Location = new System.Drawing.Point(3, 3);
            this.grpBxSsvepModel.Name = "grpBxSsvepModel";
            this.grpBxSsvepModel.Size = new System.Drawing.Size(711, 194);
            this.grpBxSsvepModel.TabIndex = 0;
            this.grpBxSsvepModel.TabStop = false;
            this.grpBxSsvepModel.Text = "SSVEP Model";
            // 
            // btnSsvepCalbr1
            // 
            this.btnSsvepCalbr1.Enabled = false;
            this.btnSsvepCalbr1.Location = new System.Drawing.Point(382, 19);
            this.btnSsvepCalbr1.Name = "btnSsvepCalbr1";
            this.btnSsvepCalbr1.Size = new System.Drawing.Size(82, 27);
            this.btnSsvepCalbr1.TabIndex = 8;
            this.btnSsvepCalbr1.Text = "Calbr1";
            this.btnSsvepCalbr1.UseVisualStyleBackColor = true;
            this.btnSsvepCalbr1.Click += new System.EventHandler(this.btnSsvepCalbr1_Click);
            // 
            // btnSsvepCalbr2
            // 
            this.btnSsvepCalbr2.Enabled = false;
            this.btnSsvepCalbr2.Location = new System.Drawing.Point(470, 19);
            this.btnSsvepCalbr2.Name = "btnSsvepCalbr2";
            this.btnSsvepCalbr2.Size = new System.Drawing.Size(82, 27);
            this.btnSsvepCalbr2.TabIndex = 9;
            this.btnSsvepCalbr2.Text = "Calbr2";
            this.btnSsvepCalbr2.UseVisualStyleBackColor = true;
            this.btnSsvepCalbr2.Click += new System.EventHandler(this.btnSsvepCalbr2_Click);
            // 
            // rtbSsvepStatus
            // 
            this.rtbSsvepStatus.BackColor = System.Drawing.SystemColors.Window;
            this.rtbSsvepStatus.DetectUrls = false;
            this.rtbSsvepStatus.Font = new System.Drawing.Font("Consolas", 8F);
            this.rtbSsvepStatus.Location = new System.Drawing.Point(382, 52);
            this.rtbSsvepStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbSsvepStatus.Name = "rtbSsvepStatus";
            this.rtbSsvepStatus.ReadOnly = true;
            this.rtbSsvepStatus.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbSsvepStatus.Size = new System.Drawing.Size(314, 130);
            this.rtbSsvepStatus.TabIndex = 7;
            this.rtbSsvepStatus.Text = "";
            this.rtbSsvepStatus.WordWrap = false;
            // 
            // prgSsvepTraining
            // 
            this.prgSsvepTraining.Location = new System.Drawing.Point(8, 164);
            this.prgSsvepTraining.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prgSsvepTraining.Name = "prgSsvepTraining";
            this.prgSsvepTraining.Size = new System.Drawing.Size(364, 18);
            this.prgSsvepTraining.TabIndex = 6;
            // 
            // lblSsvepModelInfo
            // 
            this.lblSsvepModelInfo.AutoEllipsis = true;
            this.lblSsvepModelInfo.Location = new System.Drawing.Point(8, 143);
            this.lblSsvepModelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSsvepModelInfo.Name = "lblSsvepModelInfo";
            this.lblSsvepModelInfo.Size = new System.Drawing.Size(364, 17);
            this.lblSsvepModelInfo.TabIndex = 5;
            this.lblSsvepModelInfo.Text = "No model selected";
            // 
            // lblSsvepActiveModel
            // 
            this.lblSsvepActiveModel.AutoEllipsis = true;
            this.lblSsvepActiveModel.Location = new System.Drawing.Point(290, 119);
            this.lblSsvepActiveModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSsvepActiveModel.Name = "lblSsvepActiveModel";
            this.lblSsvepActiveModel.Size = new System.Drawing.Size(406, 18);
            this.lblSsvepActiveModel.TabIndex = 4;
            this.lblSsvepActiveModel.Text = "Active: none";
            // 
            // btnSsvepDelete
            // 
            this.btnSsvepDelete.Location = new System.Drawing.Point(290, 85);
            this.btnSsvepDelete.Name = "btnSsvepDelete";
            this.btnSsvepDelete.Size = new System.Drawing.Size(82, 27);
            this.btnSsvepDelete.TabIndex = 3;
            this.btnSsvepDelete.Text = "Delete";
            this.btnSsvepDelete.UseVisualStyleBackColor = true;
            this.btnSsvepDelete.Click += new System.EventHandler(this.btnSsvepDelete_Click);
            // 
            // btnSsvepUse
            // 
            this.btnSsvepUse.Location = new System.Drawing.Point(290, 52);
            this.btnSsvepUse.Name = "btnSsvepUse";
            this.btnSsvepUse.Size = new System.Drawing.Size(82, 27);
            this.btnSsvepUse.TabIndex = 2;
            this.btnSsvepUse.Text = "Use";
            this.btnSsvepUse.UseVisualStyleBackColor = true;
            this.btnSsvepUse.Click += new System.EventHandler(this.btnSsvepUse_Click);
            // 
            // btnSsvepTrain
            // 
            this.btnSsvepTrain.Location = new System.Drawing.Point(290, 19);
            this.btnSsvepTrain.Name = "btnSsvepTrain";
            this.btnSsvepTrain.Size = new System.Drawing.Size(82, 27);
            this.btnSsvepTrain.TabIndex = 1;
            this.btnSsvepTrain.Text = "Train";
            this.btnSsvepTrain.UseVisualStyleBackColor = true;
            this.btnSsvepTrain.Click += new System.EventHandler(this.btnSsvepTrain_Click);
            // 
            // lstViewSsvepModels
            // 
            this.lstViewSsvepModels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSsvepModelName,
            this.colSsvepModelModified});
            this.lstViewSsvepModels.FullRowSelect = true;
            this.lstViewSsvepModels.HideSelection = false;
            this.lstViewSsvepModels.Location = new System.Drawing.Point(8, 19);
            this.lstViewSsvepModels.MultiSelect = false;
            this.lstViewSsvepModels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstViewSsvepModels.Name = "lstViewSsvepModels";
            this.lstViewSsvepModels.Size = new System.Drawing.Size(275, 119);
            this.lstViewSsvepModels.TabIndex = 0;
            this.lstViewSsvepModels.UseCompatibleStateImageBehavior = false;
            this.lstViewSsvepModels.View = System.Windows.Forms.View.Details;
            this.lstViewSsvepModels.SelectedIndexChanged += new System.EventHandler(this.lstViewSsvepModels_SelectedIndexChanged);
            this.lstViewSsvepModels.DoubleClick += new System.EventHandler(this.lstViewSsvepModels_DoubleClick);
            // 
            // colSsvepModelName
            // 
            this.colSsvepModelName.Text = "Model";
            this.colSsvepModelName.Width = 165;
            // 
            // colSsvepModelModified
            // 
            this.colSsvepModelModified.Text = "Modified";
            this.colSsvepModelModified.Width = 88;
            //
            // grpBxEogRejection
            //
            this.grpBxEogRejection.Controls.Add(this.tblEogRejection);
            this.grpBxEogRejection.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBxEogRejection.Location = new System.Drawing.Point(3, 197);
            this.grpBxEogRejection.Name = "grpBxEogRejection";
            this.grpBxEogRejection.Size = new System.Drawing.Size(711, 80);
            this.grpBxEogRejection.TabIndex = 1;
            this.grpBxEogRejection.TabStop = false;
            this.grpBxEogRejection.Text = "EOG Rejection";
            //
            // tblEogRejection
            //
            this.tblEogRejection.ColumnCount = 3;
            this.tblEogRejection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tblEogRejection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblEogRejection.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblEogRejection.Controls.Add(this.lblEogRejectionProfile, 0, 0);
            this.tblEogRejection.Controls.Add(this.cmbBxEogRejectionProfile, 0, 1);
            this.tblEogRejection.Controls.Add(this.chkBxVeogRejection, 1, 0);
            this.tblEogRejection.Controls.Add(this.pnlVeogThreshold, 1, 1);
            this.tblEogRejection.Controls.Add(this.chkBxHeogRejection, 2, 0);
            this.tblEogRejection.Controls.Add(this.pnlHeogThreshold, 2, 1);
            this.tblEogRejection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblEogRejection.Location = new System.Drawing.Point(3, 16);
            this.tblEogRejection.Name = "tblEogRejection";
            this.tblEogRejection.Padding = new System.Windows.Forms.Padding(8, 0, 8, 4);
            this.tblEogRejection.RowCount = 2;
            this.tblEogRejection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tblEogRejection.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblEogRejection.Size = new System.Drawing.Size(705, 61);
            this.tblEogRejection.TabIndex = 0;
            //
            // lblEogRejectionProfile
            //
            this.lblEogRejectionProfile.AutoSize = true;
            this.lblEogRejectionProfile.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEogRejectionProfile.Name = "lblEogRejectionProfile";
            this.lblEogRejectionProfile.TabIndex = 0;
            this.lblEogRejectionProfile.Text = "Profile";
            //
            // cmbBxEogRejectionProfile
            //
            this.cmbBxEogRejectionProfile.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbBxEogRejectionProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxEogRejectionProfile.FormattingEnabled = true;
            this.cmbBxEogRejectionProfile.Items.AddRange(new object[] {
                "Gaze selection",
                "Covert attention"});
            this.cmbBxEogRejectionProfile.Margin = new System.Windows.Forms.Padding(3, 0, 14, 3);
            this.cmbBxEogRejectionProfile.Name = "cmbBxEogRejectionProfile";
            this.cmbBxEogRejectionProfile.TabIndex = 1;
            this.toolTipEogRejection.SetToolTip(this.cmbBxEogRejectionProfile, "Select the profile to edit. Each analysis layer uses its own profile.");
            this.cmbBxEogRejectionProfile.SelectedIndexChanged += new System.EventHandler(this.cmbBxEogRejectionProfile_SelectedIndexChanged);
            //
            // chkBxVeogRejection
            //
            this.chkBxVeogRejection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBxVeogRejection.AutoSize = true;
            this.chkBxVeogRejection.Name = "chkBxVeogRejection";
            this.chkBxVeogRejection.TabIndex = 2;
            this.chkBxVeogRejection.Text = "VEOG";
            this.chkBxVeogRejection.UseVisualStyleBackColor = true;
            this.toolTipEogRejection.SetToolTip(this.chkBxVeogRejection, "Enable VEOG rejection for this profile.");
            this.chkBxVeogRejection.CheckedChanged += new System.EventHandler(this.EogRejectionSettings_Changed);
            //
            // chkBxHeogRejection
            //
            this.chkBxHeogRejection.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkBxHeogRejection.AutoSize = true;
            this.chkBxHeogRejection.Name = "chkBxHeogRejection";
            this.chkBxHeogRejection.TabIndex = 4;
            this.chkBxHeogRejection.Text = "HEOG";
            this.chkBxHeogRejection.UseVisualStyleBackColor = true;
            this.toolTipEogRejection.SetToolTip(this.chkBxHeogRejection, "Enable HEOG rejection for this profile.");
            this.chkBxHeogRejection.CheckedChanged += new System.EventHandler(this.EogRejectionSettings_Changed);
            //
            // pnlVeogThreshold
            //
            this.pnlVeogThreshold.Controls.Add(this.numUpDwnVeogThreshold);
            this.pnlVeogThreshold.Controls.Add(this.lblVeogThreshold);
            this.pnlVeogThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlVeogThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.pnlVeogThreshold.Name = "pnlVeogThreshold";
            this.pnlVeogThreshold.TabIndex = 3;
            this.pnlVeogThreshold.WrapContents = false;
            //
            // pnlHeogThreshold
            //
            this.pnlHeogThreshold.Controls.Add(this.numUpDwnHeogThreshold);
            this.pnlHeogThreshold.Controls.Add(this.lblHeogThreshold);
            this.pnlHeogThreshold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeogThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.pnlHeogThreshold.Name = "pnlHeogThreshold";
            this.pnlHeogThreshold.TabIndex = 5;
            this.pnlHeogThreshold.WrapContents = false;
            //
            // numUpDwnVeogThreshold
            //
            this.numUpDwnVeogThreshold.AccessibleName = "VEOG threshold (µV)";
            this.numUpDwnVeogThreshold.DecimalPlaces = 1;
            this.numUpDwnVeogThreshold.Increment = new decimal(new int[] {10, 0, 0, 0});
            this.numUpDwnVeogThreshold.Maximum = new decimal(new int[] {10000, 0, 0, 0});
            this.numUpDwnVeogThreshold.Minimum = new decimal(new int[] {1, 0, 0, 65536});
            this.numUpDwnVeogThreshold.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.numUpDwnVeogThreshold.Name = "numUpDwnVeogThreshold";
            this.numUpDwnVeogThreshold.Size = new System.Drawing.Size(80, 20);
            this.numUpDwnVeogThreshold.TabIndex = 0;
            this.numUpDwnVeogThreshold.ThousandsSeparator = true;
            this.numUpDwnVeogThreshold.Value = new decimal(new int[] {150, 0, 0, 0});
            this.toolTipEogRejection.SetToolTip(this.numUpDwnVeogThreshold, "Maximum VEOG peak-to-peak amplitude within an epoch or analysis window, in microvolts.");
            this.numUpDwnVeogThreshold.ValueChanged += new System.EventHandler(this.EogRejectionSettings_Changed);
            //
            // numUpDwnHeogThreshold
            //
            this.numUpDwnHeogThreshold.AccessibleName = "HEOG threshold (µV)";
            this.numUpDwnHeogThreshold.DecimalPlaces = 1;
            this.numUpDwnHeogThreshold.Increment = new decimal(new int[] {10, 0, 0, 0});
            this.numUpDwnHeogThreshold.Maximum = new decimal(new int[] {10000, 0, 0, 0});
            this.numUpDwnHeogThreshold.Minimum = new decimal(new int[] {1, 0, 0, 65536});
            this.numUpDwnHeogThreshold.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.numUpDwnHeogThreshold.Name = "numUpDwnHeogThreshold";
            this.numUpDwnHeogThreshold.Size = new System.Drawing.Size(80, 20);
            this.numUpDwnHeogThreshold.TabIndex = 0;
            this.numUpDwnHeogThreshold.ThousandsSeparator = true;
            this.numUpDwnHeogThreshold.Value = new decimal(new int[] {100, 0, 0, 0});
            this.toolTipEogRejection.SetToolTip(this.numUpDwnHeogThreshold, "Maximum absolute HEOG amplitude in microvolts (differential output).");
            this.numUpDwnHeogThreshold.ValueChanged += new System.EventHandler(this.EogRejectionSettings_Changed);
            //
            // lblVeogThreshold
            //
            this.lblVeogThreshold.AutoSize = true;
            this.lblVeogThreshold.Margin = new System.Windows.Forms.Padding(0, 4, 3, 0);
            this.lblVeogThreshold.Name = "lblVeogThreshold";
            this.lblVeogThreshold.TabIndex = 1;
            this.lblVeogThreshold.Text = "Threshold (µV)";
            //
            // lblHeogThreshold
            //
            this.lblHeogThreshold.AutoSize = true;
            this.lblHeogThreshold.Margin = new System.Windows.Forms.Padding(0, 4, 3, 0);
            this.lblHeogThreshold.Name = "lblHeogThreshold";
            this.lblHeogThreshold.TabIndex = 1;
            this.lblHeogThreshold.Text = "Threshold (µV)";
            //
            // splitSsvepAnalysis
            //
            this.splitSsvepAnalysis.Name = "splitSsvepAnalysis";
            this.splitSsvepAnalysis.Size = new System.Drawing.Size(711, 662);
            this.splitSsvepAnalysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSsvepAnalysis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.splitSsvepAnalysis.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSsvepAnalysis.SplitterWidth = 6;
            this.splitSsvepAnalysis.SplitterDistance = 194;
            this.splitSsvepAnalysis.Panel1MinSize = 184;
            this.splitSsvepAnalysis.Panel2MinSize = 240;
            this.splitSsvepAnalysis.Panel1.Controls.Add(this.grpBxSsvepModel);
            this.splitSsvepAnalysis.Panel2.Controls.Add(this.tabSsvepAnalysisViews);
            this.splitSsvepAnalysis.Panel2.Controls.Add(this.pnlHeogMarkup);
            this.splitSsvepAnalysis.Panel2.Controls.Add(this.grpBxEogRejection);
            //
            // splitSsvepModelDetails
            //
            this.splitSsvepModelDetails.Name = "splitSsvepModelDetails";
            this.splitSsvepModelDetails.Size = new System.Drawing.Size(705, 175);
            this.splitSsvepModelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitSsvepModelDetails.SplitterWidth = 6;
            this.splitSsvepModelDetails.SplitterDistance = 364;
            this.splitSsvepModelDetails.Panel1MinSize = 260;
            this.splitSsvepModelDetails.Panel2MinSize = 200;
            this.splitSsvepModelDetails.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitSsvepModelDetails.Panel1.Controls.Add(this.tblSsvepModel);
            this.splitSsvepModelDetails.Panel2.Controls.Add(this.tblSsvepStatus);
            //
            // tblSsvepModel
            //
            this.tblSsvepModel.Name = "tblSsvepModel";
            this.tblSsvepModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSsvepModel.ColumnCount = 1;
            this.tblSsvepModel.RowCount = 4;
            this.tblSsvepModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblSsvepModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblSsvepModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblSsvepModel.Controls.Add(this.tblSsvepModelControls, 0, 0);
            this.tblSsvepModel.Controls.Add(this.lblSsvepActiveModel, 0, 1);
            this.tblSsvepModel.Controls.Add(this.lblSsvepModelInfo, 0, 2);
            this.tblSsvepModel.Controls.Add(this.prgSsvepTraining, 0, 3);
            //
            // tblSsvepModelControls
            //
            this.tblSsvepModelControls.Name = "tblSsvepModelControls";
            this.tblSsvepModelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSsvepModelControls.ColumnCount = 2;
            this.tblSsvepModelControls.RowCount = 1;
            this.tblSsvepModelControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepModelControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tblSsvepModelControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepModelControls.Controls.Add(this.lstViewSsvepModels, 0, 0);
            this.tblSsvepModelControls.Controls.Add(this.pnlSsvepModelButtons, 1, 0);
            //
            // pnlSsvepModelButtons
            //
            this.pnlSsvepModelButtons.Name = "pnlSsvepModelButtons";
            this.pnlSsvepModelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSsvepModelButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlSsvepModelButtons.WrapContents = false;
            this.pnlSsvepModelButtons.Margin = new System.Windows.Forms.Padding(0);
            this.btnSsvepTrain.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.pnlSsvepModelButtons.Controls.Add(this.btnSsvepTrain);
            this.btnSsvepUse.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.pnlSsvepModelButtons.Controls.Add(this.btnSsvepUse);
            this.btnSsvepDelete.Margin = new System.Windows.Forms.Padding(3, 0, 3, 4);
            this.pnlSsvepModelButtons.Controls.Add(this.btnSsvepDelete);
            this.lblSsvepActiveModel.Margin = new System.Windows.Forms.Padding(0);
            this.lblSsvepModelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.prgSsvepTraining.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lstViewSsvepModels.Margin = new System.Windows.Forms.Padding(0, 0, 3, 3);
            //
            // tblSsvepStatus
            //
            this.tblSsvepStatus.Name = "tblSsvepStatus";
            this.tblSsvepStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblSsvepStatus.ColumnCount = 1;
            this.tblSsvepStatus.RowCount = 2;
            this.tblSsvepStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tblSsvepStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblSsvepStatus.Controls.Add(this.pnlSsvepCalibration, 0, 0);
            this.tblSsvepStatus.Controls.Add(this.rtbSsvepStatus, 0, 1);
            this.rtbSsvepStatus.Margin = new System.Windows.Forms.Padding(0);
            //
            // pnlSsvepCalibration
            //
            this.pnlSsvepCalibration.Name = "pnlSsvepCalibration";
            this.pnlSsvepCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSsvepCalibration.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSsvepCalibration.Controls.Add(this.btnSsvepCalbr1);
            this.pnlSsvepCalibration.Controls.Add(this.btnSsvepCalbr2);
            //
            // pnlHeogMarkup
            //
            this.pnlHeogMarkup.Name = "pnlHeogMarkup";
            this.pnlHeogMarkup.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeogMarkup.Height = 36;
            this.pnlHeogMarkup.Padding = new System.Windows.Forms.Padding(5, 3, 0, 0);
            this.pnlHeogMarkup.ColumnCount = 4;
            this.pnlHeogMarkup.RowCount = 1;
            this.pnlHeogMarkup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.pnlHeogMarkup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.pnlHeogMarkup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.pnlHeogMarkup.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlHeogMarkup.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlHeogMarkup.Controls.Add(this.btnHeogMarkup, 0, 0);
            this.pnlHeogMarkup.Controls.Add(this.lblHeogMarkupRange, 1, 0);
            this.pnlHeogMarkup.Controls.Add(this.numHeogMarkupRange, 2, 0);
            this.pnlHeogMarkup.Controls.Add(this.lblHeogMarkupLegend, 3, 0);
            //
            // btnHeogMarkup
            //
            this.btnHeogMarkup.Name = "btnHeogMarkup";
            this.btnHeogMarkup.Text = "Mark HEOG";
            this.btnHeogMarkup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHeogMarkup.UseVisualStyleBackColor = true;
            this.btnHeogMarkup.TabIndex = 0;
            this.btnHeogMarkup.Click += new System.EventHandler(this.btnHeogMarkup_Click);
            //
            // lblHeogMarkupRange
            //
            this.lblHeogMarkupRange.Name = "lblHeogMarkupRange";
            this.lblHeogMarkupRange.Text = "Range (µV)";
            this.lblHeogMarkupRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeogMarkupRange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHeogMarkupRange.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            //
            // numHeogMarkupRange
            //
            this.numHeogMarkupRange.Name = "numHeogMarkupRange";
            this.numHeogMarkupRange.Minimum = 0.1M;
            this.numHeogMarkupRange.Maximum = 10000M;
            this.numHeogMarkupRange.DecimalPlaces = 1;
            this.numHeogMarkupRange.Value = 20M;
            this.numHeogMarkupRange.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numHeogMarkupRange.Width = 78;
            this.numHeogMarkupRange.TabIndex = 1;
            this.numHeogMarkupRange.ValueChanged += new System.EventHandler(this.HeogMarkupThresholdChanged);
            //
            // lblHeogMarkupLegend
            //
            this.lblHeogMarkupLegend.Name = "lblHeogMarkupLegend";
            this.lblHeogMarkupLegend.Text = "Green: fixation; click its number to edit";
            this.lblHeogMarkupLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeogMarkupLegend.AutoEllipsis = true;
            this.lblHeogMarkupLegend.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHeogMarkupLegend.Margin = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.toolTipEogRejection.SetToolTip(this.lblHeogMarkupLegend, "Red: gaze shift; pale green: fixation, including short ones. Click its number to edit. Notes: 41, 43, 45, 47, 48, 50, 52, 53; squares: 9-14; 0: unassigned. Right-click a fixation to add/delete its label or choose the start.");
            this.toolTipEogRejection.SetToolTip(this.btnHeogMarkup, "Mark HEOG fixations in the opened PHS. Start at the fixation containing the first note-on marker; label 41 to 53 and back. Verify manually. Re-marking resets labels.");
            this.toolTipEogRejection.SetToolTip(this.numHeogMarkupRange, "Minimum rapid HEOG change after local trend removal, in microvolts. Independent of EOG rejection thresholds.");
            //
            // prgHeogMarkup
            //
            this.prgHeogMarkup.Name = "prgHeogMarkup";
            this.prgHeogMarkup.Visible = false;
            this.prgHeogMarkup.Width = 130;
            //
            // statusHeogMarkup
            //
            this.statusHeogMarkup.Name = "statusHeogMarkup";
            this.statusHeogMarkup.Visible = false;
            this.statusHeogMarkup.Text = "HEOG markup";
            // 
            // tabPageAnalysis2
            // 
            this.tabPageAnalysis2.Controls.Add(this.grpBxAnalysis2);
            this.tabPageAnalysis2.Controls.Add(this.grpBxAnalysis2Controls);
            this.tabPageAnalysis2.Location = new System.Drawing.Point(4, 22);
            this.tabPageAnalysis2.Name = "tabPageAnalysis2";
            this.tabPageAnalysis2.Size = new System.Drawing.Size(717, 668);
            this.tabPageAnalysis2.TabIndex = 2;
            this.tabPageAnalysis2.Text = "Spatial Analysis";
            this.tabPageAnalysis2.UseVisualStyleBackColor = true;
            // 
            // grpBxAnalysis2
            // 
            this.grpBxAnalysis2.Controls.Add(this.glCtrlAnalysis2);
            this.grpBxAnalysis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBxAnalysis2.Location = new System.Drawing.Point(0, 52);
            this.grpBxAnalysis2.Name = "grpBxAnalysis2";
            this.grpBxAnalysis2.Size = new System.Drawing.Size(717, 616);
            this.grpBxAnalysis2.TabIndex = 1;
            this.grpBxAnalysis2.TabStop = false;
            this.grpBxAnalysis2.Text = "Spatial analysis";
            // 
            // glCtrlAnalysis2
            // 
            this.glCtrlAnalysis2.BackColor = System.Drawing.Color.Black;
            this.glCtrlAnalysis2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.glCtrlAnalysis2.Location = new System.Drawing.Point(3, 16);
            this.glCtrlAnalysis2.Name = "glCtrlAnalysis2";
            this.glCtrlAnalysis2.Size = new System.Drawing.Size(711, 597);
            this.glCtrlAnalysis2.TabIndex = 1;
            this.glCtrlAnalysis2.VSync = false;
            // 
            // grpBxAnalysis2Controls
            // 
            this.grpBxAnalysis2Controls.Controls.Add(this.lblAnalysis2SensAmpl);
            this.grpBxAnalysis2Controls.Controls.Add(this.cmbBxAnalysis2SensAmpl);
            this.grpBxAnalysis2Controls.Controls.Add(this.cmbBxAnalysis2Mode);
            this.grpBxAnalysis2Controls.Controls.Add(this.lblAnalysis2Mode);
            this.grpBxAnalysis2Controls.Controls.Add(this.cmbBxAnalysis2Component);
            this.grpBxAnalysis2Controls.Controls.Add(this.lblAnalysis2Component);
            this.grpBxAnalysis2Controls.Controls.Add(this.cmbBxAnalysis2View);
            this.grpBxAnalysis2Controls.Controls.Add(this.lblAnalysis2View);
            this.grpBxAnalysis2Controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBxAnalysis2Controls.Location = new System.Drawing.Point(0, 0);
            this.grpBxAnalysis2Controls.Name = "grpBxAnalysis2Controls";
            this.grpBxAnalysis2Controls.Size = new System.Drawing.Size(717, 52);
            this.grpBxAnalysis2Controls.TabIndex = 0;
            this.grpBxAnalysis2Controls.TabStop = false;
            this.grpBxAnalysis2Controls.Text = "Display";
            // 
            // lblAnalysis2SensAmpl
            // 
            this.lblAnalysis2SensAmpl.AutoSize = true;
            this.lblAnalysis2SensAmpl.Location = new System.Drawing.Point(598, 23);
            this.lblAnalysis2SensAmpl.Name = "lblAnalysis2SensAmpl";
            this.lblAnalysis2SensAmpl.Size = new System.Drawing.Size(57, 13);
            this.lblAnalysis2SensAmpl.TabIndex = 0;
            this.lblAnalysis2SensAmpl.Text = "Sens Ampl";
            // 
            // cmbBxAnalysis2SensAmpl
            // 
            this.cmbBxAnalysis2SensAmpl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxAnalysis2SensAmpl.FormattingEnabled = true;
            this.cmbBxAnalysis2SensAmpl.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cmbBxAnalysis2SensAmpl.Location = new System.Drawing.Point(660, 19);
            this.cmbBxAnalysis2SensAmpl.Name = "cmbBxAnalysis2SensAmpl";
            this.cmbBxAnalysis2SensAmpl.Size = new System.Drawing.Size(48, 21);
            this.cmbBxAnalysis2SensAmpl.TabIndex = 6;
            this.cmbBxAnalysis2SensAmpl.SelectedIndexChanged += new System.EventHandler(this.cmbBxAnalysis2SensAmpl_SelectedIndexChanged);
            // 
            // cmbBxAnalysis2Mode
            // 
            this.cmbBxAnalysis2Mode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxAnalysis2Mode.Items.AddRange(new object[] {
            "RESS spatial weights",
            "Channel spectra"});
            this.cmbBxAnalysis2Mode.Location = new System.Drawing.Point(51, 19);
            this.cmbBxAnalysis2Mode.Name = "cmbBxAnalysis2Mode";
            this.cmbBxAnalysis2Mode.Size = new System.Drawing.Size(160, 21);
            this.cmbBxAnalysis2Mode.TabIndex = 5;
            this.cmbBxAnalysis2Mode.SelectedIndexChanged += new System.EventHandler(this.cmbBxAnalysis2Mode_SelectedIndexChanged);
            // 
            // lblAnalysis2Mode
            // 
            this.lblAnalysis2Mode.AutoSize = true;
            this.lblAnalysis2Mode.Location = new System.Drawing.Point(8, 23);
            this.lblAnalysis2Mode.Name = "lblAnalysis2Mode";
            this.lblAnalysis2Mode.Size = new System.Drawing.Size(37, 13);
            this.lblAnalysis2Mode.TabIndex = 7;
            this.lblAnalysis2Mode.Text = "Mode:";
            // 
            // cmbBxAnalysis2Component
            // 
            this.cmbBxAnalysis2Component.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxAnalysis2Component.FormattingEnabled = true;
            this.cmbBxAnalysis2Component.Items.AddRange(new object[] {
            "Base frequency",
            "2nd harmonic"});
            this.cmbBxAnalysis2Component.Location = new System.Drawing.Point(294, 19);
            this.cmbBxAnalysis2Component.Name = "cmbBxAnalysis2Component";
            this.cmbBxAnalysis2Component.Size = new System.Drawing.Size(128, 21);
            this.cmbBxAnalysis2Component.TabIndex = 3;
            this.cmbBxAnalysis2Component.SelectedIndexChanged += new System.EventHandler(this.cmbBxAnalysis2Component_SelectedIndexChanged);
            // 
            // lblAnalysis2Component
            // 
            this.lblAnalysis2Component.AutoSize = true;
            this.lblAnalysis2Component.Location = new System.Drawing.Point(224, 23);
            this.lblAnalysis2Component.Name = "lblAnalysis2Component";
            this.lblAnalysis2Component.Size = new System.Drawing.Size(64, 13);
            this.lblAnalysis2Component.TabIndex = 2;
            this.lblAnalysis2Component.Text = "Component:";
            // 
            // cmbBxAnalysis2View
            // 
            this.cmbBxAnalysis2View.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBxAnalysis2View.FormattingEnabled = true;
            this.cmbBxAnalysis2View.Items.AddRange(new object[] {
            "Grid",
            "Topography"});
            this.cmbBxAnalysis2View.Location = new System.Drawing.Point(470, 19);
            this.cmbBxAnalysis2View.Name = "cmbBxAnalysis2View";
            this.cmbBxAnalysis2View.Size = new System.Drawing.Size(112, 21);
            this.cmbBxAnalysis2View.TabIndex = 1;
            this.cmbBxAnalysis2View.SelectedIndexChanged += new System.EventHandler(this.cmbBxAnalysis2View_SelectedIndexChanged);
            // 
            // lblAnalysis2View
            // 
            this.lblAnalysis2View.AutoSize = true;
            this.lblAnalysis2View.Location = new System.Drawing.Point(431, 23);
            this.lblAnalysis2View.Name = "lblAnalysis2View";
            this.lblAnalysis2View.Size = new System.Drawing.Size(33, 13);
            this.lblAnalysis2View.TabIndex = 0;
            this.lblAnalysis2View.Text = "View:";
            // 
            // statusStripMain
            // 
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusFrequency,
            this.statusHeogMarkup,
            this.prgHeogMarkup});
            this.statusStripMain.Location = new System.Drawing.Point(0, 717);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1212, 22);
            this.statusStripMain.TabIndex = 6;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1107, 17);
            this.toolStripStatusLabel1.Spring = true;
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusFrequency
            // 
            this.toolStripStatusFrequency.AutoSize = false;
            this.toolStripStatusFrequency.Name = "toolStripStatusFrequency";
            this.toolStripStatusFrequency.Size = new System.Drawing.Size(90, 17);
            this.toolStripStatusFrequency.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.trackBarPlayback);
            this.splitContainer3.Panel2.Controls.Add(this.toolStrip1);
            this.splitContainer3.Size = new System.Drawing.Size(1212, 25);
            this.splitContainer3.SplitterDistance = 60;
            this.splitContainer3.Panel1Collapsed = true;
            this.splitContainer3.TabIndex = 10;
            // 
            // trackBarPlayback
            // 
            this.trackBarPlayback.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarPlayback.AutoSize = false;
            this.trackBarPlayback.Location = new System.Drawing.Point(480, 0);
            this.trackBarPlayback.Name = "trackBarPlayback";
            this.trackBarPlayback.Size = new System.Drawing.Size(732, 29);
            this.trackBarPlayback.TabIndex = 11;
            this.trackBarPlayback.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlStrpStartRecording,
            this.tlStrpRecordingIndicator,
            this.tlStrpStopRecording,
            this.tlStrpOpenRecording,
            this.tlStrpSaveAnalysis,
            this.lblAnalysisDirty,
            this.tlStrpPlayRecording,
            this.tlStrpStopPlayback,
            this.toolStripSeparator1,
            this.lblPlaybackTotalTime,
            this.lblPlaybackCurrentTime});
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1212, 25);
            this.toolStrip1.TabIndex = 10;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlStrpStartRecording
            // 
            this.tlStrpStartRecording.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpStartRecording.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpStartRecording.Image")));
            this.tlStrpStartRecording.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlStrpStartRecording.Name = "tlStrpStartRecording";
            this.tlStrpStartRecording.Size = new System.Drawing.Size(54, 22);
            this.tlStrpStartRecording.Text = "StartRec";
            this.tlStrpStartRecording.Click += new System.EventHandler(this.tlStrpStartRecording_Click);
            // 
            // tlStrpRecordingIndicator
            // 
            this.tlStrpRecordingIndicator.AutoSize = false;
            this.tlStrpRecordingIndicator.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tlStrpRecordingIndicator.Enabled = false;
            this.tlStrpRecordingIndicator.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpRecordingIndicator.Image")));
            this.tlStrpRecordingIndicator.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tlStrpRecordingIndicator.Margin = new System.Windows.Forms.Padding(0);
            this.tlStrpRecordingIndicator.Name = "tlStrpRecordingIndicator";
            this.tlStrpRecordingIndicator.Size = new System.Drawing.Size(24, 24);
            this.tlStrpRecordingIndicator.Text = "Recording";
            this.tlStrpRecordingIndicator.ToolTipText = "Not recording";
            // 
            // tlStrpStopRecording
            // 
            this.tlStrpStopRecording.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpStopRecording.Enabled = false;
            this.tlStrpStopRecording.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpStopRecording.Image")));
            this.tlStrpStopRecording.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlStrpStopRecording.Name = "tlStrpStopRecording";
            this.tlStrpStopRecording.Size = new System.Drawing.Size(54, 22);
            this.tlStrpStopRecording.Text = "StopRec";
            this.tlStrpStopRecording.Click += new System.EventHandler(this.tlStrpStopRecording_Click);
            // 
            // tlStrpOpenRecording
            // 
            this.tlStrpOpenRecording.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpOpenRecording.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpOpenRecording.Image")));
            this.tlStrpOpenRecording.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlStrpOpenRecording.Name = "tlStrpOpenRecording";
            this.tlStrpOpenRecording.Size = new System.Drawing.Size(59, 22);
            this.tlStrpOpenRecording.Text = "OpenRec";
            this.tlStrpOpenRecording.Click += new System.EventHandler(this.tlStrpOpenRecording_Click);
            // 
            // tlStrpPlayRecording
            // 
            this.tlStrpPlayRecording.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpPlayRecording.Enabled = false;
            this.tlStrpPlayRecording.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpPlayRecording.Image")));
            this.tlStrpPlayRecording.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlStrpPlayRecording.Name = "tlStrpPlayRecording";
            this.tlStrpPlayRecording.Size = new System.Drawing.Size(33, 22);
            this.tlStrpPlayRecording.Text = "Play";
            this.tlStrpPlayRecording.Click += new System.EventHandler(this.tlStrpPlayRecording_Click);
            // 
            // tlStrpStopPlayback
            // 
            this.tlStrpStopPlayback.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpStopPlayback.Enabled = false;
            this.tlStrpStopPlayback.Image = ((System.Drawing.Image)(resources.GetObject("tlStrpStopPlayback.Image")));
            this.tlStrpStopPlayback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlStrpStopPlayback.Name = "tlStrpStopPlayback";
            this.tlStrpStopPlayback.Size = new System.Drawing.Size(57, 22);
            this.tlStrpStopPlayback.Text = "StopPlay";
            this.tlStrpStopPlayback.Click += new System.EventHandler(this.tlStrpStopPlayback_Click);
            // 
            // tlStrpSaveAnalysis
            //
            this.tlStrpSaveAnalysis.Name = "tlStrpSaveAnalysis";
            this.tlStrpSaveAnalysis.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tlStrpSaveAnalysis.Text = "Save";
            this.tlStrpSaveAnalysis.Enabled = false;
            this.tlStrpSaveAnalysis.Size = new System.Drawing.Size(36, 22);
            this.tlStrpSaveAnalysis.ToolTipText = "Save annotations and HEOG settings";
            this.tlStrpSaveAnalysis.Click += new System.EventHandler(this.tlStrpSaveAnalysis_Click);
            //
            // lblAnalysisDirty
            //
            this.lblAnalysisDirty.Name = "lblAnalysisDirty";
            this.lblAnalysisDirty.AutoSize = false;
            this.lblAnalysisDirty.Size = new System.Drawing.Size(12, 22);
            this.lblAnalysisDirty.Text = "●";
            this.lblAnalysisDirty.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblAnalysisDirty.Visible = false;
            this.lblAnalysisDirty.ToolTipText = "Unsaved annotation changes";
            this.lblAnalysisDirty.AccessibleName = "Unsaved annotation changes";
            //
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPlaybackTotalTime
            // 
            this.lblPlaybackTotalTime.Name = "lblPlaybackTotalTime";
            this.lblPlaybackTotalTime.Size = new System.Drawing.Size(55, 22);
            this.lblPlaybackTotalTime.Text = "00:00.000";
            // 
            // lblPlaybackCurrentTime
            // 
            this.lblPlaybackCurrentTime.Name = "lblPlaybackCurrentTime";
            this.lblPlaybackCurrentTime.Size = new System.Drawing.Size(55, 22);
            this.lblPlaybackCurrentTime.Text = "00:00.000";
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 739);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.statusStripMain);
            this.Name = "fmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PolyHubNET";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fmMain_FormClosing);
            this.Load += new System.EventHandler(this.fmMain_Load);
            // Analysis views: existing viewer and offline performance share the same lower area.
            this.tabSsvepAnalysisViews.Name = "tabSsvepAnalysisViews";
            this.tabSsvepAnalysisViews.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSsvepAnalysisViews.Controls.Add(this.tabLiveAnalysis);
            this.tabSsvepAnalysisViews.Controls.Add(this.tabPerformance);
            this.tabLiveAnalysis.Name = "tabLiveAnalysis";
            this.tabLiveAnalysis.Text = "Analysis Viewer";
            this.tabLiveAnalysis.Padding = new System.Windows.Forms.Padding(3);
            this.tabLiveAnalysis.UseVisualStyleBackColor = true;
            this.tabLiveAnalysis.Controls.Add(this.grpBxAnalysis);
            this.tabPerformance.Name = "tabPerformance";
            this.tabPerformance.Text = "Performance";
            this.tabPerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPerformance.UseVisualStyleBackColor = true;
            this.tabPerformance.Controls.Add(this.pnlPerformanceContent);
            this.tabPerformance.Controls.Add(this.lblPerformanceSummary);
            this.tabPerformance.Controls.Add(this.pnlPerformanceCommands);
            this.pnlPerformanceCommands.Name = "pnlPerformanceCommands";
            this.pnlPerformanceCommands.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPerformanceCommands.Height = 34;
            this.pnlPerformanceCommands.WrapContents = true;
            this.pnlPerformanceCommands.AutoSize = true;
            this.pnlPerformanceCommands.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPerformanceCommands.MinimumSize = new System.Drawing.Size(0, 34);
            this.btnPerformanceAnalyze.Name = "btnPerformanceAnalyze";
            this.btnPerformanceAnalyze.Text = "Analyze";
            this.btnPerformanceAnalyze.Size = new System.Drawing.Size(82, 27);
            this.btnPerformanceAnalyze.UseVisualStyleBackColor = true;
            this.btnPerformanceAnalyze.Click += new System.EventHandler(this.btnPerformanceAnalyze_Click);
            this.lblAcceptanceRadius.Name = "lblAcceptanceRadius";
            this.lblAcceptanceRadius.Text = "Radius ×";
            this.lblAcceptanceRadius.AutoSize = true;
            this.lblAcceptanceRadius.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.lblAcceptanceMargin.Name = "lblAcceptanceMargin";
            this.lblAcceptanceMargin.Text = "BG margin";
            this.lblAcceptanceMargin.AutoSize = true;
            this.lblAcceptanceMargin.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.lblAcceptanceNote.Name = "lblAcceptanceNote";
            this.lblAcceptanceNote.Text = "Note";
            this.lblAcceptanceNote.AutoSize = true;
            this.lblAcceptanceNote.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.lblAcceptanceThreshold.Name = "lblAcceptanceThreshold";
            this.lblAcceptanceThreshold.Text = "BG threshold";
            this.lblAcceptanceThreshold.AutoSize = true;
            this.lblAcceptanceThreshold.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.lblAcceptanceDirty.Name = "lblAcceptanceDirty";
            this.lblAcceptanceDirty.Text = "*";
            this.lblAcceptanceDirty.AutoSize = true;
            this.lblAcceptanceDirty.Margin = new System.Windows.Forms.Padding(8, 8, 3, 0);
            this.numAcceptanceRadius.Name = "numAcceptanceRadius";
            this.numAcceptanceRadius.DecimalPlaces = 3;
            this.numAcceptanceRadius.Increment = 0.05m;
            this.numAcceptanceRadius.Minimum = 0.001m;
            this.numAcceptanceRadius.Maximum = 100m;
            this.numAcceptanceRadius.Value = 1m;
            this.numAcceptanceRadius.Width = 70;
            this.numAcceptanceRadius.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numAcceptanceRadius.Enabled = false;
            this.numAcceptanceRadius.ValueChanged += new System.EventHandler(this.AcceptanceValueChanged);
            this.numAcceptanceMargin.Name = "numAcceptanceMargin";
            this.numAcceptanceMargin.DecimalPlaces = 3;
            this.numAcceptanceMargin.Increment = 0.05m;
            this.numAcceptanceMargin.Minimum = -1000m;
            this.numAcceptanceMargin.Maximum = 1000m;
            this.numAcceptanceMargin.Value = 0.5m;
            this.numAcceptanceMargin.Width = 75;
            this.numAcceptanceMargin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numAcceptanceMargin.Enabled = false;
            this.numAcceptanceMargin.ValueChanged += new System.EventHandler(this.AcceptanceValueChanged);
            this.numAcceptanceThreshold.Name = "numAcceptanceThreshold";
            this.numAcceptanceThreshold.DecimalPlaces = 6;
            this.numAcceptanceThreshold.Increment = 0.05m;
            this.numAcceptanceThreshold.Minimum = -1000m;
            this.numAcceptanceThreshold.Maximum = 1000m;
            this.numAcceptanceThreshold.Value = 0m;
            this.numAcceptanceThreshold.Width = 100;
            this.numAcceptanceThreshold.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.numAcceptanceThreshold.Enabled = false;
            this.numAcceptanceThreshold.ValueChanged += new System.EventHandler(this.AcceptanceValueChanged);
            this.cmbAcceptanceNote.Name = "cmbAcceptanceNote";
            this.cmbAcceptanceNote.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAcceptanceNote.Width = 52;
            this.cmbAcceptanceNote.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmbAcceptanceNote.Enabled = false;
            this.cmbAcceptanceNote.SelectedIndexChanged += new System.EventHandler(this.AcceptanceNoteChanged);
            this.btnAcceptanceSave.Name = "btnAcceptanceSave";
            this.btnAcceptanceSave.Text = "Save";
            this.btnAcceptanceSave.Size = new System.Drawing.Size(55, 27);
            this.btnAcceptanceSave.UseVisualStyleBackColor = true;
            this.btnAcceptanceSave.Enabled = false;
            this.btnAcceptanceSave.Click += new System.EventHandler(this.btnAcceptanceSave_Click);
            this.btnAcceptanceReset.Name = "btnAcceptanceReset";
            this.btnAcceptanceReset.Text = "Reset";
            this.btnAcceptanceReset.Size = new System.Drawing.Size(55, 27);
            this.btnAcceptanceReset.UseVisualStyleBackColor = true;
            this.btnAcceptanceReset.Enabled = false;
            this.btnAcceptanceReset.Click += new System.EventHandler(this.btnAcceptanceReset_Click);
            this.lblAcceptanceDirty.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblAcceptanceDirty.Visible = false;
            this.toolTipEogRejection.SetToolTip(this.numAcceptanceRadius, "Radius multiplier r: larger values allow greater deviation from calibration examples. Applies to the active decoder and the next Analyze run.");
            this.toolTipEogRejection.SetToolTip(this.numAcceptanceMargin, "Global background margin M. Effective threshold for note j is Tj + M. Larger values reject more windows for all notes.");
            this.toolTipEogRejection.SetToolTip(this.numAcceptanceThreshold, "Individual background threshold Tj for the selected note. Effective threshold is Tj + M. Larger values reject more windows for this note only.");
            this.toolTipEogRejection.SetToolTip(this.cmbAcceptanceNote, "Select the note whose individual background threshold is shown. Selecting a note does not change settings.");
            this.toolTipEogRejection.SetToolTip(this.btnAcceptanceSave, "Save current acceptance settings to the active model JSON.");
            this.toolTipEogRejection.SetToolTip(this.btnAcceptanceReset, "Restore all acceptance settings loaded when this model was activated. The model file changes only when you press Save.");
            this.toolTipEogRejection.SetToolTip(this.lblAcceptanceDirty, "Unsaved model settings.");
            this.chkPerformanceLog.Name = "chkPerformanceLog";
            this.chkPerformanceLog.Text = "Log";
            this.chkPerformanceLog.AutoSize = true;
            this.chkPerformanceLog.Margin = new System.Windows.Forms.Padding(8, 7, 8, 3);
            this.lblPerformanceView.Name = "lblPerformanceView";
            this.lblPerformanceView.Text = "View";
            this.lblPerformanceView.AutoSize = true;
            this.lblPerformanceView.Margin = new System.Windows.Forms.Padding(3, 8, 3, 0);
            this.cmbPerformanceView.Name = "cmbPerformanceView";
            this.cmbPerformanceView.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPerformanceView.Width = 115;
            this.cmbPerformanceView.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmbPerformanceView.Items.AddRange(new object[] { "Summary", "Confusion", "ROC", "Timeline" });
            this.cmbPerformanceView.SelectedIndexChanged += new System.EventHandler(this.PerformanceViewChanged);
            this.cmbPerformanceRocMode.Name = "cmbPerformanceRocMode";
            this.cmbPerformanceRocMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPerformanceRocMode.Width = 130;
            this.cmbPerformanceRocMode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.cmbPerformanceRocMode.Items.AddRange(new object[] { "Other notes", "Background" });
            this.cmbPerformanceRocMode.Visible = false;
            this.cmbPerformanceRocMode.SelectedIndexChanged += new System.EventHandler(this.PerformanceViewChanged);
            this.pnlAcceptanceRadius.Name = "pnlAcceptanceRadius";
            this.pnlAcceptanceRadius.AutoSize = true;
            this.pnlAcceptanceRadius.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAcceptanceRadius.WrapContents = false;
            this.pnlAcceptanceRadius.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAcceptanceRadius.Controls.Add(this.lblAcceptanceRadius);
            this.pnlAcceptanceRadius.Controls.Add(this.numAcceptanceRadius);
            this.pnlAcceptanceMargin.Name = "pnlAcceptanceMargin";
            this.pnlAcceptanceMargin.AutoSize = true;
            this.pnlAcceptanceMargin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAcceptanceMargin.WrapContents = false;
            this.pnlAcceptanceMargin.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAcceptanceMargin.Controls.Add(this.lblAcceptanceMargin);
            this.pnlAcceptanceMargin.Controls.Add(this.numAcceptanceMargin);
            this.pnlAcceptanceNote.Name = "pnlAcceptanceNote";
            this.pnlAcceptanceNote.AutoSize = true;
            this.pnlAcceptanceNote.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAcceptanceNote.WrapContents = false;
            this.pnlAcceptanceNote.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAcceptanceNote.Controls.Add(this.lblAcceptanceNote);
            this.pnlAcceptanceNote.Controls.Add(this.cmbAcceptanceNote);
            this.pnlAcceptanceNote.Controls.Add(this.lblAcceptanceThreshold);
            this.pnlAcceptanceNote.Controls.Add(this.numAcceptanceThreshold);
            this.pnlAcceptanceSave.Name = "pnlAcceptanceSave";
            this.pnlAcceptanceSave.AutoSize = true;
            this.pnlAcceptanceSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlAcceptanceSave.WrapContents = false;
            this.pnlAcceptanceSave.Margin = new System.Windows.Forms.Padding(0);
            this.pnlAcceptanceSave.Controls.Add(this.btnAcceptanceSave);
            this.pnlAcceptanceSave.Controls.Add(this.lblAcceptanceDirty);
            this.pnlAcceptanceSave.Controls.Add(this.btnAcceptanceReset);
            this.pnlPerformanceView.Name = "pnlPerformanceView";
            this.pnlPerformanceView.AutoSize = true;
            this.pnlPerformanceView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlPerformanceView.WrapContents = false;
            this.pnlPerformanceView.Margin = new System.Windows.Forms.Padding(0);
            this.pnlPerformanceView.Controls.Add(this.lblPerformanceView);
            this.pnlPerformanceView.Controls.Add(this.cmbPerformanceView);
            this.pnlPerformanceView.Controls.Add(this.cmbPerformanceRocMode);
            this.pnlPerformanceCommands.Controls.Add(this.btnPerformanceAnalyze);
            this.pnlPerformanceCommands.Controls.Add(this.pnlAcceptanceRadius);
            this.pnlPerformanceCommands.Controls.Add(this.pnlAcceptanceMargin);
            this.pnlPerformanceCommands.Controls.Add(this.pnlAcceptanceNote);
            this.pnlPerformanceCommands.Controls.Add(this.pnlAcceptanceSave);
            this.pnlPerformanceCommands.Controls.Add(this.chkPerformanceLog);
            this.pnlPerformanceCommands.Controls.Add(this.pnlPerformanceView);
            this.lblPerformanceSummary.Name = "lblPerformanceSummary";
            this.lblPerformanceSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPerformanceSummary.Height = 62;
            this.lblPerformanceSummary.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lblPerformanceSummary.Text = "Activate a model with Use, verify HEOG labels, then Analyze.";
            this.lblPerformanceSummary.AutoEllipsis = true;
            this.pnlPerformanceContent.Name = "pnlPerformanceContent";
            this.pnlPerformanceContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPerformanceContent.Controls.Add(this.gridPerformanceSummary);
            this.pnlPerformanceContent.Controls.Add(this.gridPerformanceConfusion);
            this.pnlPerformanceContent.Controls.Add(this.performancePlot);
            this.performancePlot.Name = "performancePlot";
            this.performancePlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.performancePlot.Visible = false;
            this.performancePlot.BackColor = System.Drawing.SystemColors.Window;
            this.performancePlot.Size = new System.Drawing.Size(200, 100);
            this.performancePlot.Controls.Add(this.chkPerformanceSmooth);
            this.performancePlot.Controls.Add(this.chkPerformanceShowAll);
            this.chkPerformanceSmooth.Name = "chkPerformanceSmooth";
            this.chkPerformanceSmooth.Text = "Smooth";
            this.chkPerformanceSmooth.AutoSize = true;
            this.chkPerformanceSmooth.Size = new System.Drawing.Size(65, 17);
            this.chkPerformanceSmooth.Location = new System.Drawing.Point(129, 77);
            this.chkPerformanceSmooth.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.chkPerformanceSmooth.BackColor = System.Drawing.SystemColors.Window;
            this.chkPerformanceSmooth.UseVisualStyleBackColor = false;
            this.chkPerformanceSmooth.Visible = false;
            this.chkPerformanceSmooth.CheckedChanged += new System.EventHandler(this.PerformanceViewChanged);
            this.toolTipEogRejection.SetToolTip(this.chkPerformanceSmooth, "Visual smoothing only. ROC values, AUC and current threshold points remain unchanged.");
            this.chkPerformanceShowAll.Name = "chkPerformanceShowAll";
            this.chkPerformanceShowAll.Text = "Show all data";
            this.chkPerformanceShowAll.AutoSize = true;
            this.chkPerformanceShowAll.Size = new System.Drawing.Size(90, 17);
            this.chkPerformanceShowAll.Location = new System.Drawing.Point(104, 77);
            this.chkPerformanceShowAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.chkPerformanceShowAll.BackColor = System.Drawing.SystemColors.Window;
            this.chkPerformanceShowAll.UseVisualStyleBackColor = false;
            this.chkPerformanceShowAll.Visible = false;
            this.chkPerformanceShowAll.CheckedChanged += new System.EventHandler(this.PerformanceViewChanged);
            this.toolTipEogRejection.SetToolTip(this.chkPerformanceShowAll, "Display the complete recording. Gray data are outside labeled green fixations and excluded from metrics and CSV.");
            this.performancePlot.Paint += new System.Windows.Forms.PaintEventHandler(this.performancePlot_Paint);
            this.performancePlot.Resize += new System.EventHandler(this.performancePlot_Resize);
            this.performancePlot.MouseMove += new System.Windows.Forms.MouseEventHandler(this.performancePlot_MouseMove);
            this.performancePlot.MouseLeave += new System.EventHandler(this.performancePlot_MouseLeave);
            this.gridPerformanceSummary.Name = "gridPerformanceSummary";
            this.gridPerformanceSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPerformanceSummary.ReadOnly = true;
            this.gridPerformanceSummary.AllowUserToAddRows = false;
            this.gridPerformanceSummary.AllowUserToDeleteRows = false;
            this.gridPerformanceSummary.AllowUserToResizeRows = false;
            this.gridPerformanceSummary.RowHeadersVisible = false;
            this.gridPerformanceSummary.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridPerformanceSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridPerformanceSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPerformanceSummary.AutoGenerateColumns = false;
            this.gridPerformanceSummary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridPerformanceSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPerformanceSummary.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(246, 248, 250);
            this.gridPerformanceSummary.Visible = true;
            this.gridPerformanceSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.perfSummaryNote, this.perfSummaryAttempts, this.perfSummaryFirstCorrect, this.perfSummaryFirstWrong, this.perfSummaryNoSelection, this.perfSummaryDetected, this.perfSummaryLatency, this.perfSummaryAucNotes, this.perfSummaryAucBackground, this.perfSummaryScored });
            this.perfSummaryNote.Name = "perfSummaryNote";
            this.perfSummaryNote.HeaderText = "Note";
            this.perfSummaryNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryAttempts.Name = "perfSummaryAttempts";
            this.perfSummaryAttempts.HeaderText = "Attempts";
            this.perfSummaryAttempts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryFirstCorrect.Name = "perfSummaryFirstCorrect";
            this.perfSummaryFirstCorrect.HeaderText = "First OK";
            this.perfSummaryFirstCorrect.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryFirstWrong.Name = "perfSummaryFirstWrong";
            this.perfSummaryFirstWrong.HeaderText = "First wrong";
            this.perfSummaryFirstWrong.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryNoSelection.Name = "perfSummaryNoSelection";
            this.perfSummaryNoSelection.HeaderText = "No selection";
            this.perfSummaryNoSelection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryDetected.Name = "perfSummaryDetected";
            this.perfSummaryDetected.HeaderText = "Correct seen";
            this.perfSummaryDetected.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryLatency.Name = "perfSummaryLatency";
            this.perfSummaryLatency.HeaderText = "Mean correct (s)";
            this.perfSummaryLatency.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryAucNotes.Name = "perfSummaryAucNotes";
            this.perfSummaryAucNotes.HeaderText = "AUC notes";
            this.perfSummaryAucNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryAucBackground.Name = "perfSummaryAucBackground";
            this.perfSummaryAucBackground.HeaderText = "AUC BG";
            this.perfSummaryAucBackground.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryScored.Name = "perfSummaryScored";
            this.perfSummaryScored.HeaderText = "Scored windows";
            this.perfSummaryScored.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.gridPerformanceConfusion.Name = "gridPerformanceConfusion";
            this.gridPerformanceConfusion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridPerformanceConfusion.ReadOnly = true;
            this.gridPerformanceConfusion.AllowUserToAddRows = false;
            this.gridPerformanceConfusion.AllowUserToDeleteRows = false;
            this.gridPerformanceConfusion.AllowUserToResizeRows = false;
            this.gridPerformanceConfusion.RowHeadersVisible = false;
            this.gridPerformanceConfusion.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridPerformanceConfusion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gridPerformanceConfusion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPerformanceConfusion.AutoGenerateColumns = false;
            this.gridPerformanceConfusion.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.gridPerformanceConfusion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPerformanceConfusion.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(246, 248, 250);
            this.gridPerformanceConfusion.Visible = false;
            this.gridPerformanceConfusion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { this.perfMatrixTarget, this.perfMatrixNone, this.perfMatrixNote41, this.perfMatrixNote43, this.perfMatrixNote45, this.perfMatrixNote47, this.perfMatrixNote48, this.perfMatrixNote50, this.perfMatrixNote52, this.perfMatrixNote53 });
            this.perfMatrixTarget.Name = "perfMatrixTarget";
            this.perfMatrixTarget.HeaderText = "Target";
            this.perfMatrixTarget.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNone.Name = "perfMatrixNone";
            this.perfMatrixNone.HeaderText = "No command";
            this.perfMatrixNone.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote41.Name = "perfMatrixNote41";
            this.perfMatrixNote41.HeaderText = "41";
            this.perfMatrixNote41.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote43.Name = "perfMatrixNote43";
            this.perfMatrixNote43.HeaderText = "43";
            this.perfMatrixNote43.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote45.Name = "perfMatrixNote45";
            this.perfMatrixNote45.HeaderText = "45";
            this.perfMatrixNote45.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote47.Name = "perfMatrixNote47";
            this.perfMatrixNote47.HeaderText = "47";
            this.perfMatrixNote47.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote48.Name = "perfMatrixNote48";
            this.perfMatrixNote48.HeaderText = "48";
            this.perfMatrixNote48.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote50.Name = "perfMatrixNote50";
            this.perfMatrixNote50.HeaderText = "50";
            this.perfMatrixNote50.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote52.Name = "perfMatrixNote52";
            this.perfMatrixNote52.HeaderText = "52";
            this.perfMatrixNote52.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfMatrixNote53.Name = "perfMatrixNote53";
            this.perfMatrixNote53.HeaderText = "53";
            this.perfMatrixNote53.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.perfSummaryFirstCorrect.ToolTipText = "Attempts whose first NEW note selection was correct; shown as count / attempts.";
            this.perfSummaryDetected.ToolTipText = "Attempts with a correct NEW selection at any time; later correctness does not erase the first wrong selection.";
            this.perfSummaryLatency.ToolTipText = "Mean time from target fixation start to first correct NEW selection, only among detected attempts.";
            this.perfSummaryScored.ToolTipText = "Target decisions with finite margins available for ROC; decisions without scores still count in the confusion matrix.";
            this.toolTipEogRejection.SetToolTip(this.chkPerformanceLog, "Save all decisions, selection events, model, markup, ROC points and summaries to an analysis CSV beside the PHS file.");
            this.toolTipEogRejection.SetToolTip(this.lblPerformanceSummary, "Estimated Wolpaw ITR: eight note choices; first NEW selection per attempt; omissions count as errors. Timing starts at fixation and excludes gaze shifts and GUI/IPC delay. Correlated windows and repeated targets require cautious interpretation.");
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.grpBxServices.ResumeLayout(false);
            this.grpBxServices.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEpochs)).EndInit();
            this.grpBxParameters.ResumeLayout(false);
            this.tabCntrlViewer.ResumeLayout(false);
            this.tabPageSignal.ResumeLayout(false);
            this.grpBxSignalViewer.ResumeLayout(false);
            this.grpBxSignalViewer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnTimeBaseInSec)).EndInit();
            this.tabPageTimeFreq.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnTimeFBaseInSec)).EndInit();
            this.splitSsvepAnalysis.Panel1.ResumeLayout(false);
            this.splitSsvepAnalysis.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSsvepAnalysis)).EndInit();
            this.splitSsvepAnalysis.ResumeLayout(false);
            this.splitSsvepAnalysis.PerformLayout();
            this.splitSsvepModelDetails.Panel1.ResumeLayout(false);
            this.splitSsvepModelDetails.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitSsvepModelDetails)).EndInit();
            this.splitSsvepModelDetails.ResumeLayout(false);
            this.splitSsvepModelDetails.PerformLayout();
            this.tblSsvepModel.ResumeLayout(false);
            this.tblSsvepModel.PerformLayout();
            this.tblSsvepModelControls.ResumeLayout(false);
            this.tblSsvepModelControls.PerformLayout();
            this.pnlSsvepModelButtons.ResumeLayout(false);
            this.pnlSsvepModelButtons.PerformLayout();
            this.tblSsvepStatus.ResumeLayout(false);
            this.tblSsvepStatus.PerformLayout();
            this.pnlSsvepCalibration.ResumeLayout(false);
            this.pnlSsvepCalibration.PerformLayout();
            this.toolTipEogRejection.SetToolTip(this.numHeogEpochLabel, "Notes: 41, 43, 45, 47, 48, 50, 52, 53; squares: 9-14; 0: unassigned. Enter: save; Esc: cancel. Clicking outside saves.");
            ((System.ComponentModel.ISupportInitialize)(this.numHeogEpochLabel)).EndInit();
            this.menuHeogEpoch.ResumeLayout(false);
            this.pnlHeogMarkup.ResumeLayout(false);
            this.pnlHeogMarkup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHeogMarkupRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceMargin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptanceRadius)).EndInit();
            this.tabPageAnalysis.ResumeLayout(false);
            this.grpBxAnalysis.ResumeLayout(false);
            this.grpBxSsvepModel.ResumeLayout(false);
            this.grpBxEogRejection.ResumeLayout(false);
            this.tblEogRejection.ResumeLayout(false);
            this.tblEogRejection.PerformLayout();
            this.pnlVeogThreshold.ResumeLayout(false);
            this.pnlVeogThreshold.PerformLayout();
            this.pnlHeogThreshold.ResumeLayout(false);
            this.pnlHeogThreshold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnVeogThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDwnHeogThreshold)).EndInit();
            this.tabPageAnalysis2.ResumeLayout(false);
            this.grpBxAnalysis2.ResumeLayout(false);
            this.grpBxAnalysis2Controls.ResumeLayout(false);
            this.grpBxAnalysis2Controls.PerformLayout();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPlayback)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox grpBxServices;
        private System.Windows.Forms.GroupBox grpBxParameters;
        private System.Windows.Forms.Panel panelHost;
        private System.Windows.Forms.TabControl tabSsvepAnalysisViews;
        private System.Windows.Forms.TabPage tabLiveAnalysis;
        private System.Windows.Forms.TabPage tabPerformance;
        private System.Windows.Forms.FlowLayoutPanel pnlPerformanceCommands;
        private System.Windows.Forms.FlowLayoutPanel pnlAcceptanceRadius;
        private System.Windows.Forms.FlowLayoutPanel pnlAcceptanceMargin;
        private System.Windows.Forms.FlowLayoutPanel pnlAcceptanceNote;
        private System.Windows.Forms.FlowLayoutPanel pnlAcceptanceSave;
        private System.Windows.Forms.FlowLayoutPanel pnlPerformanceView;
        private System.Windows.Forms.Button btnPerformanceAnalyze;
        private System.Windows.Forms.Label lblAcceptanceRadius;
        private System.Windows.Forms.NumericUpDown numAcceptanceRadius;
        private System.Windows.Forms.Label lblAcceptanceMargin;
        private System.Windows.Forms.NumericUpDown numAcceptanceMargin;
        private System.Windows.Forms.Label lblAcceptanceNote;
        private System.Windows.Forms.ComboBox cmbAcceptanceNote;
        private System.Windows.Forms.Label lblAcceptanceThreshold;
        private System.Windows.Forms.NumericUpDown numAcceptanceThreshold;
        private System.Windows.Forms.Button btnAcceptanceSave;
        private System.Windows.Forms.Button btnAcceptanceReset;
        private System.Windows.Forms.Label lblAcceptanceDirty;
        private System.Windows.Forms.CheckBox chkPerformanceLog;
        private System.Windows.Forms.Label lblPerformanceView;
        private System.Windows.Forms.ComboBox cmbPerformanceView;
        private System.Windows.Forms.ComboBox cmbPerformanceRocMode;
        private System.Windows.Forms.Label lblPerformanceSummary;
        private System.Windows.Forms.Panel pnlPerformanceContent;
        private System.Windows.Forms.DataGridView gridPerformanceSummary;
        private System.Windows.Forms.DataGridView gridPerformanceConfusion;
        private System.Windows.Forms.Panel performancePlot;
        private System.Windows.Forms.CheckBox chkPerformanceSmooth;
        private System.Windows.Forms.CheckBox chkPerformanceShowAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryAttempts;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryFirstCorrect;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryFirstWrong;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryNoSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryDetected;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryLatency;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryAucNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryAucBackground;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfSummaryScored;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixTarget;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNone;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote41;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote43;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote45;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote47;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote48;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote50;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote52;
        private System.Windows.Forms.DataGridViewTextBoxColumn perfMatrixNote53;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusFrequency;
        private System.Windows.Forms.TreeView treeViewServices;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tlStrpStartRecording;
        private System.Windows.Forms.ToolStripButton tlStrpRecordingIndicator;
        private System.Windows.Forms.ToolStripButton tlStrpStopRecording;
        private System.Windows.Forms.ToolStripButton tlStrpOpenRecording;
        private System.Windows.Forms.ToolStripButton tlStrpPlayRecording;
        private System.Windows.Forms.ToolStripButton tlStrpStopPlayback;
        private System.Windows.Forms.ToolStripButton tlStrpSaveAnalysis;
        private System.Windows.Forms.ToolStripLabel lblAnalysisDirty;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TrackBar trackBarPlayback;
        private System.Windows.Forms.ListBox listBoxEpochClasses;
        private System.Windows.Forms.DataGridView dataGridViewEpochs;
        private System.Windows.Forms.Button btnStartAllServices;
        private System.Windows.Forms.TreeView treeViewPipeline;
        private System.Windows.Forms.Button btnApplyAllRoutes;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Button btnDeactivateSavedRoutes;
        private System.Windows.Forms.ToolStripLabel lblPlaybackTotalTime;
        private System.Windows.Forms.ToolStripLabel lblPlaybackCurrentTime;
        private System.Windows.Forms.TabControl tabCntrlViewer;
        private System.Windows.Forms.TabPage tabPageSignal;
        private System.Windows.Forms.GroupBox grpBxSignalViewer;
        private OpenTK.GLControl glCtrlViewer;
        private System.Windows.Forms.NumericUpDown numHeogEpochLabel;
        private System.Windows.Forms.ContextMenuStrip menuHeogEpoch;
        private System.Windows.Forms.ToolStripMenuItem menuHeogStartHere;
        private System.Windows.Forms.ToolStripMenuItem menuHeogAdd;
        private System.Windows.Forms.ToolStripMenuItem menuHeogDelete;
        private System.Windows.Forms.ToolStripSeparator menuRecordingSeparator;
        private System.Windows.Forms.ToolStripMenuItem menuRecordingInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbBxScaleAmpl;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.NumericUpDown numUpDwnTimeBaseInSec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblViewerInputPin;
        private System.Windows.Forms.TabPage tabPageAnalysis;
        private System.Windows.Forms.GroupBox grpBxSsvepModel;
        private System.Windows.Forms.Button btnSsvepCalbr1;
        private System.Windows.Forms.Button btnSsvepCalbr2;
        private System.Windows.Forms.GroupBox grpBxEogRejection;
        private System.Windows.Forms.TableLayoutPanel tblEogRejection;
        private System.Windows.Forms.Label lblEogRejectionProfile;
        private System.Windows.Forms.ComboBox cmbBxEogRejectionProfile;
        private System.Windows.Forms.CheckBox chkBxVeogRejection;
        private System.Windows.Forms.CheckBox chkBxHeogRejection;
        private System.Windows.Forms.FlowLayoutPanel pnlVeogThreshold;
        private System.Windows.Forms.FlowLayoutPanel pnlHeogThreshold;
        private System.Windows.Forms.NumericUpDown numUpDwnVeogThreshold;
        private System.Windows.Forms.NumericUpDown numUpDwnHeogThreshold;
        private System.Windows.Forms.Label lblVeogThreshold;
        private System.Windows.Forms.Label lblHeogThreshold;
        private System.Windows.Forms.ToolTip toolTipEogRejection;
        private System.Windows.Forms.ListView lstViewSsvepModels;
        private System.Windows.Forms.ColumnHeader colSsvepModelName;
        private System.Windows.Forms.ColumnHeader colSsvepModelModified;
        private System.Windows.Forms.Button btnSsvepTrain;
        private System.Windows.Forms.Button btnSsvepUse;
        private System.Windows.Forms.Button btnSsvepDelete;
        private System.Windows.Forms.Label lblSsvepActiveModel;
        private System.Windows.Forms.Label lblSsvepModelInfo;
        private System.Windows.Forms.ProgressBar prgSsvepTraining;
        private System.Windows.Forms.RichTextBox rtbSsvepStatus;
        private System.Windows.Forms.GroupBox grpBxAnalysis;
        private OpenTK.GLControl glCtrlAnalysis;
        private System.Windows.Forms.TabPage tabPageAnalysis2;
        private System.Windows.Forms.GroupBox grpBxAnalysis2Controls;
        private System.Windows.Forms.Label lblAnalysis2View;
        private System.Windows.Forms.ComboBox cmbBxAnalysis2SensAmpl;
        private System.Windows.Forms.Label lblAnalysis2SensAmpl;
        private System.Windows.Forms.ComboBox cmbBxAnalysis2Mode;
        private System.Windows.Forms.Label lblAnalysis2Mode;
        private System.Windows.Forms.ComboBox cmbBxAnalysis2View;
        private System.Windows.Forms.Label lblAnalysis2Component;
        private System.Windows.Forms.ComboBox cmbBxAnalysis2Component;
        private System.Windows.Forms.GroupBox grpBxAnalysis2;
        private OpenTK.GLControl glCtrlAnalysis2;
        private System.Windows.Forms.TabPage tabPageTimeFreq;
        private System.Windows.Forms.GroupBox groupBox1;
        private OpenTK.GLControl glCtrlTimeFreqViewer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBxScaleTFAmpl;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numUpDwnTimeFBaseInSec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.SplitContainer splitSsvepAnalysis;
        private System.Windows.Forms.SplitContainer splitSsvepModelDetails;
        private System.Windows.Forms.TableLayoutPanel tblSsvepModel;
        private System.Windows.Forms.TableLayoutPanel tblSsvepModelControls;
        private System.Windows.Forms.FlowLayoutPanel pnlSsvepModelButtons;
        private System.Windows.Forms.TableLayoutPanel tblSsvepStatus;
        private System.Windows.Forms.FlowLayoutPanel pnlSsvepCalibration;
        private System.Windows.Forms.TableLayoutPanel pnlHeogMarkup;
        private System.Windows.Forms.Button btnHeogMarkup;
        private System.Windows.Forms.Label lblHeogMarkupRange;
        private System.Windows.Forms.NumericUpDown numHeogMarkupRange;
        private System.Windows.Forms.Label lblHeogMarkupLegend;
        private System.Windows.Forms.ToolStripProgressBar prgHeogMarkup;
        private System.Windows.Forms.ToolStripStatusLabel statusHeogMarkup;
    }
}

