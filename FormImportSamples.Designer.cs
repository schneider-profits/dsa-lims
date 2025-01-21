namespace DSA_lims
{
    partial class FormImportSamples
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportSamples));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cboxProjectSub = new System.Windows.Forms.ComboBox();
            this.cboxProjectMain = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxLaboratory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.gridSamples = new System.Windows.Forms.DataGridView();
            this.colImportable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSampleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSSampleType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSampleComponent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSSampComp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExternalId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSamplingDateFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSamplingDateTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReferenceDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSLocationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLatitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAltitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSampler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSSampler = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSamplingMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSSamplingMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWeekNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFlowVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDepth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDryWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWetWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAirSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalinity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLayerFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLayerTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTracerNuclide = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLIMSTracerNuclide = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTracerWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTracerVolume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIntendedAnalysis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequireFiltering = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmenuSamples = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiSetAllImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetAllNonImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetNonImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSetLIMSSampleType = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetLIMSSampleComponent = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetExternalId = new System.Windows.Forms.ToolStripMenuItem();
            this.setSamplingDateFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSamplingDateToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetReferenceDate = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetLocationType = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSetLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.setLatitudeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLongitudeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAltitudeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLIMSSamplerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLIMSSamplingMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWeekNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFlowVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDepthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setDryWeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWetWeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAirSpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSalinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLayerFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setLayerToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTemperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTracerNuclideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTracerWeightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTracerVolumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setIntendedAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setPHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setRequireFilteringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFileID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbProject = new System.Windows.Forms.TextBox();
            this.panelImport = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressImport = new System.Windows.Forms.ToolStripProgressBar();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnMarkAllImportable = new System.Windows.Forms.ToolStripButton();
            this.btnUnmarkAllImportable = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSetImportable = new System.Windows.Forms.ToolStripButton();
            this.btnSetNonImportable = new System.Windows.Forms.ToolStripButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tools = new System.Windows.Forms.ToolStrip();
            this.btnSelectFileLimsSamples = new System.Windows.Forms.ToolStripButton();
            this.btnSelectFileSampleReg = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.miImport = new System.Windows.Forms.ToolStripMenuItem();
            this.miImportLIMSSamples = new System.Windows.Forms.ToolStripMenuItem();
            this.miImportSampleRegistration = new System.Windows.Forms.ToolStripMenuItem();
            this.miImportMINASamples = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miSet = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkAllImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.miMarkAllNonImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetNonImportable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.miSetLIMSSampleType = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLIMSSampleComponent = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetExternalId = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetSamplingDateFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetSamplingDateTo = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetReferenceDate = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLocationType = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLatitude = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLongitude = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetAltitude = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetSampler = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetSamplingMethod = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetComment = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetWeekNumber = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetFlowVolume = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetVolume = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetDepth = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetDryWeight = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetWetWeight = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetAirSpeed = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetSalinity = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLayerFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetLayerTo = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetArea = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetTemperature = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetTracerNuclide = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetTracerWeight = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetTracerVolume = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetIntendedAnalysis = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetPH = new System.Windows.Forms.ToolStripMenuItem();
            this.miSetRequireFiltering = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples)).BeginInit();
            this.cmenuSamples.SuspendLayout();
            this.panelImport.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tools.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cboxProjectSub);
            this.panel1.Controls.Add(this.cboxProjectMain);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cboxLaboratory);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 595);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 8, 0, 6);
            this.panel1.Size = new System.Drawing.Size(984, 44);
            this.panel1.TabIndex = 8;
            // 
            // cboxProjectSub
            // 
            this.cboxProjectSub.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboxProjectSub.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxProjectSub.FormattingEnabled = true;
            this.cboxProjectSub.Location = new System.Drawing.Point(444, 8);
            this.cboxProjectSub.Name = "cboxProjectSub";
            this.cboxProjectSub.Size = new System.Drawing.Size(150, 23);
            this.cboxProjectSub.TabIndex = 12;
            // 
            // cboxProjectMain
            // 
            this.cboxProjectMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboxProjectMain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxProjectMain.FormattingEnabled = true;
            this.cboxProjectMain.Location = new System.Drawing.Point(294, 8);
            this.cboxProjectMain.Name = "cboxProjectMain";
            this.cboxProjectMain.Size = new System.Drawing.Size(150, 23);
            this.cboxProjectMain.TabIndex = 11;
            this.cboxProjectMain.SelectedIndexChanged += new System.EventHandler(this.cboxProjectMain_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(230, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 30);
            this.label3.TabIndex = 10;
            this.label3.Text = "Project";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxLaboratory
            // 
            this.cboxLaboratory.Dock = System.Windows.Forms.DockStyle.Left;
            this.cboxLaboratory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxLaboratory.FormattingEnabled = true;
            this.cboxLaboratory.Location = new System.Drawing.Point(80, 8);
            this.cboxLaboratory.Name = "cboxLaboratory";
            this.cboxLaboratory.Size = new System.Drawing.Size(150, 23);
            this.cboxLaboratory.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 30);
            this.label2.TabIndex = 8;
            this.label2.Text = "Laboratory";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(784, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.miClose_Click);
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(884, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 30);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // gridSamples
            // 
            this.gridSamples.AllowUserToAddRows = false;
            this.gridSamples.AllowUserToDeleteRows = false;
            this.gridSamples.AllowUserToResizeRows = false;
            this.gridSamples.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridSamples.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridSamples.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colImportable,
            this.colNumber,
            this.colSampleType,
            this.colLIMSSampleType,
            this.colSampleComponent,
            this.colLIMSSampComp,
            this.colExternalId,
            this.colSamplingDateFrom,
            this.colSamplingDateTo,
            this.colReferenceDate,
            this.colLocation,
            this.colLIMSLocationType,
            this.colLatitude,
            this.colLongitude,
            this.colAltitude,
            this.colSampler,
            this.colLIMSSampler,
            this.colSamplingMethod,
            this.colLIMSSamplingMethod,
            this.colComment,
            this.colWeekNumber,
            this.colFlowVolume,
            this.colVolume,
            this.colDepth,
            this.colDryWeight,
            this.colWetWeight,
            this.colAirSpeed,
            this.colSalinity,
            this.colLayerFrom,
            this.colLayerTo,
            this.colArea,
            this.colTemperature,
            this.colTracerNuclide,
            this.colLIMSTracerNuclide,
            this.colTracerWeight,
            this.colTracerVolume,
            this.colIntendedAnalysis,
            this.colPH,
            this.colRequireFiltering});
            this.gridSamples.ContextMenuStrip = this.cmenuSamples;
            this.gridSamples.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSamples.Location = new System.Drawing.Point(0, 111);
            this.gridSamples.Name = "gridSamples";
            this.gridSamples.ReadOnly = true;
            this.gridSamples.RowHeadersVisible = false;
            this.gridSamples.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSamples.Size = new System.Drawing.Size(984, 484);
            this.gridSamples.TabIndex = 10;
            // 
            // colImportable
            // 
            this.colImportable.HeaderText = "Import";
            this.colImportable.Name = "colImportable";
            this.colImportable.ReadOnly = true;
            this.colImportable.Width = 48;
            // 
            // colNumber
            // 
            this.colNumber.HeaderText = "Number";
            this.colNumber.Name = "colNumber";
            this.colNumber.ReadOnly = true;
            this.colNumber.Width = 77;
            // 
            // colSampleType
            // 
            this.colSampleType.HeaderText = "Samp. Type";
            this.colSampleType.Name = "colSampleType";
            this.colSampleType.ReadOnly = true;
            this.colSampleType.Width = 89;
            // 
            // colLIMSSampleType
            // 
            this.colLIMSSampleType.HeaderText = "LIMS Samp. Type";
            this.colLIMSSampleType.Name = "colLIMSSampleType";
            this.colLIMSSampleType.ReadOnly = true;
            this.colLIMSSampleType.Width = 118;
            // 
            // colSampleComponent
            // 
            this.colSampleComponent.HeaderText = "Samp. Comp.";
            this.colSampleComponent.Name = "colSampleComponent";
            this.colSampleComponent.ReadOnly = true;
            this.colSampleComponent.Width = 98;
            // 
            // colLIMSSampComp
            // 
            this.colLIMSSampComp.HeaderText = "LIMS Samp. Comp.";
            this.colLIMSSampComp.Name = "colLIMSSampComp";
            this.colLIMSSampComp.ReadOnly = true;
            this.colLIMSSampComp.Width = 127;
            // 
            // colExternalId
            // 
            this.colExternalId.HeaderText = "External Id";
            this.colExternalId.Name = "colExternalId";
            this.colExternalId.ReadOnly = true;
            this.colExternalId.Width = 83;
            // 
            // colSamplingDateFrom
            // 
            this.colSamplingDateFrom.HeaderText = "Sampl. Date From";
            this.colSamplingDateFrom.Name = "colSamplingDateFrom";
            this.colSamplingDateFrom.ReadOnly = true;
            this.colSamplingDateFrom.Width = 95;
            // 
            // colSamplingDateTo
            // 
            this.colSamplingDateTo.HeaderText = "Sampl. Date To";
            this.colSamplingDateTo.Name = "colSamplingDateTo";
            this.colSamplingDateTo.ReadOnly = true;
            this.colSamplingDateTo.Width = 95;
            // 
            // colReferenceDate
            // 
            this.colReferenceDate.HeaderText = "Ref. Date";
            this.colReferenceDate.Name = "colReferenceDate";
            this.colReferenceDate.ReadOnly = true;
            this.colReferenceDate.Width = 77;
            // 
            // colLocation
            // 
            this.colLocation.HeaderText = "Location";
            this.colLocation.Name = "colLocation";
            this.colLocation.ReadOnly = true;
            this.colLocation.Width = 79;
            // 
            // colLIMSLocationType
            // 
            this.colLIMSLocationType.HeaderText = "LIMS Location Type";
            this.colLIMSLocationType.Name = "colLIMSLocationType";
            this.colLIMSLocationType.ReadOnly = true;
            this.colLIMSLocationType.Width = 128;
            // 
            // colLatitude
            // 
            this.colLatitude.HeaderText = "Latitude";
            this.colLatitude.Name = "colLatitude";
            this.colLatitude.ReadOnly = true;
            this.colLatitude.Width = 76;
            // 
            // colLongitude
            // 
            this.colLongitude.HeaderText = "Longitude";
            this.colLongitude.Name = "colLongitude";
            this.colLongitude.ReadOnly = true;
            this.colLongitude.Width = 87;
            // 
            // colAltitude
            // 
            this.colAltitude.HeaderText = "Altitude";
            this.colAltitude.Name = "colAltitude";
            this.colAltitude.ReadOnly = true;
            this.colAltitude.Width = 72;
            // 
            // colSampler
            // 
            this.colSampler.HeaderText = "Sampler";
            this.colSampler.Name = "colSampler";
            this.colSampler.ReadOnly = true;
            this.colSampler.Width = 79;
            // 
            // colLIMSSampler
            // 
            this.colLIMSSampler.HeaderText = "LIMS Sampler";
            this.colLIMSSampler.Name = "colLIMSSampler";
            this.colLIMSSampler.ReadOnly = true;
            this.colLIMSSampler.Width = 102;
            // 
            // colSamplingMethod
            // 
            this.colSamplingMethod.HeaderText = "Sampling Method";
            this.colSamplingMethod.Name = "colSamplingMethod";
            this.colSamplingMethod.ReadOnly = true;
            this.colSamplingMethod.Width = 119;
            // 
            // colLIMSSamplingMethod
            // 
            this.colLIMSSamplingMethod.HeaderText = "LIMS Sampling Method";
            this.colLIMSSamplingMethod.Name = "colLIMSSamplingMethod";
            this.colLIMSSamplingMethod.ReadOnly = true;
            this.colLIMSSamplingMethod.Width = 148;
            // 
            // colComment
            // 
            this.colComment.HeaderText = "Comment";
            this.colComment.Name = "colComment";
            this.colComment.ReadOnly = true;
            this.colComment.Width = 86;
            // 
            // colWeekNumber
            // 
            this.colWeekNumber.HeaderText = "Week number";
            this.colWeekNumber.Name = "colWeekNumber";
            this.colWeekNumber.ReadOnly = true;
            // 
            // colFlowVolume
            // 
            this.colFlowVolume.HeaderText = "Flow Volume (m3)";
            this.colFlowVolume.Name = "colFlowVolume";
            this.colFlowVolume.ReadOnly = true;
            this.colFlowVolume.Width = 121;
            // 
            // colVolume
            // 
            this.colVolume.HeaderText = "Volume (l)";
            this.colVolume.Name = "colVolume";
            this.colVolume.ReadOnly = true;
            this.colVolume.Width = 81;
            // 
            // colDepth
            // 
            this.colDepth.HeaderText = "Depth (m)";
            this.colDepth.Name = "colDepth";
            this.colDepth.ReadOnly = true;
            this.colDepth.Width = 80;
            // 
            // colDryWeight
            // 
            this.colDryWeight.HeaderText = "Dry weight (g)";
            this.colDryWeight.Name = "colDryWeight";
            this.colDryWeight.ReadOnly = true;
            this.colDryWeight.Width = 98;
            // 
            // colWetWeight
            // 
            this.colWetWeight.HeaderText = "Wet weight (g)";
            this.colWetWeight.Name = "colWetWeight";
            this.colWetWeight.ReadOnly = true;
            this.colWetWeight.Width = 87;
            // 
            // colAirSpeed
            // 
            this.colAirSpeed.HeaderText = "AirSpeed (m3/t)";
            this.colAirSpeed.Name = "colAirSpeed";
            this.colAirSpeed.ReadOnly = true;
            this.colAirSpeed.Width = 107;
            // 
            // colSalinity
            // 
            this.colSalinity.HeaderText = "Salinity (%)";
            this.colSalinity.Name = "colSalinity";
            this.colSalinity.ReadOnly = true;
            this.colSalinity.Width = 86;
            // 
            // colLayerFrom
            // 
            this.colLayerFrom.HeaderText = "Layer from (cm)";
            this.colLayerFrom.Name = "colLayerFrom";
            this.colLayerFrom.ReadOnly = true;
            this.colLayerFrom.Width = 86;
            // 
            // colLayerTo
            // 
            this.colLayerTo.HeaderText = "Layer to (cm)";
            this.colLayerTo.Name = "colLayerTo";
            this.colLayerTo.ReadOnly = true;
            this.colLayerTo.Width = 72;
            // 
            // colArea
            // 
            this.colArea.HeaderText = "Area (cm2)";
            this.colArea.Name = "colArea";
            this.colArea.ReadOnly = true;
            this.colArea.Width = 85;
            // 
            // colTemperature
            // 
            this.colTemperature.HeaderText = "Temperature (C)";
            this.colTemperature.Name = "colTemperature";
            this.colTemperature.ReadOnly = true;
            this.colTemperature.Width = 112;
            // 
            // colTracerNuclide
            // 
            this.colTracerNuclide.HeaderText = "Tracer Nuclide";
            this.colTracerNuclide.Name = "colTracerNuclide";
            this.colTracerNuclide.ReadOnly = true;
            this.colTracerNuclide.Width = 103;
            // 
            // colLIMSTracerNuclide
            // 
            this.colLIMSTracerNuclide.HeaderText = "LIMS Tracer Nuclide";
            this.colLIMSTracerNuclide.Name = "colLIMSTracerNuclide";
            this.colLIMSTracerNuclide.ReadOnly = true;
            this.colLIMSTracerNuclide.Width = 132;
            // 
            // colTracerWeight
            // 
            this.colTracerWeight.HeaderText = "Tracer weight (g)";
            this.colTracerWeight.Name = "colTracerWeight";
            this.colTracerWeight.ReadOnly = true;
            // 
            // colTracerVolume
            // 
            this.colTracerVolume.HeaderText = "Tracer volume (ml)";
            this.colTracerVolume.Name = "colTracerVolume";
            this.colTracerVolume.ReadOnly = true;
            this.colTracerVolume.Width = 104;
            // 
            // colIntendedAnalysis
            // 
            this.colIntendedAnalysis.HeaderText = "Intended analysis";
            this.colIntendedAnalysis.Name = "colIntendedAnalysis";
            this.colIntendedAnalysis.ReadOnly = true;
            this.colIntendedAnalysis.Width = 116;
            // 
            // colPH
            // 
            this.colPH.HeaderText = "pH";
            this.colPH.Name = "colPH";
            this.colPH.ReadOnly = true;
            this.colPH.Width = 48;
            // 
            // colRequireFiltering
            // 
            this.colRequireFiltering.HeaderText = "Require filtering";
            this.colRequireFiltering.Name = "colRequireFiltering";
            this.colRequireFiltering.ReadOnly = true;
            this.colRequireFiltering.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colRequireFiltering.Width = 109;
            // 
            // cmenuSamples
            // 
            this.cmenuSamples.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiSetAllImportable,
            this.cmiSetAllNonImportable,
            this.cmiSetImportable,
            this.cmiSetNonImportable,
            this.toolStripSeparator2,
            this.cmiSetLIMSSampleType,
            this.cmiSetLIMSSampleComponent,
            this.cmiSetExternalId,
            this.setSamplingDateFromToolStripMenuItem,
            this.setSamplingDateToToolStripMenuItem,
            this.cmiSetReferenceDate,
            this.cmiSetLocationType,
            this.cmiSetLocation,
            this.setLatitudeToolStripMenuItem,
            this.setLongitudeToolStripMenuItem,
            this.setAltitudeToolStripMenuItem,
            this.setLIMSSamplerToolStripMenuItem,
            this.setLIMSSamplingMethodToolStripMenuItem,
            this.setCommentToolStripMenuItem,
            this.setWeekNumberToolStripMenuItem,
            this.setFlowVolumeToolStripMenuItem,
            this.setVolumeToolStripMenuItem,
            this.setDepthToolStripMenuItem,
            this.setDryWeightToolStripMenuItem,
            this.setWetWeightToolStripMenuItem,
            this.setAirSpeedToolStripMenuItem,
            this.setSalinityToolStripMenuItem,
            this.setLayerFromToolStripMenuItem,
            this.setLayerToToolStripMenuItem,
            this.setAreaToolStripMenuItem,
            this.setTemperatureToolStripMenuItem,
            this.setTracerNuclideToolStripMenuItem,
            this.setTracerWeightToolStripMenuItem,
            this.setTracerVolumeToolStripMenuItem,
            this.setIntendedAnalysisToolStripMenuItem,
            this.setPHToolStripMenuItem,
            this.setRequireFilteringToolStripMenuItem});
            this.cmenuSamples.Name = "cmenuSamples";
            this.cmenuSamples.Size = new System.Drawing.Size(227, 802);
            // 
            // cmiSetAllImportable
            // 
            this.cmiSetAllImportable.Name = "cmiSetAllImportable";
            this.cmiSetAllImportable.Size = new System.Drawing.Size(226, 22);
            this.cmiSetAllImportable.Text = "Mark all for import";
            this.cmiSetAllImportable.Click += new System.EventHandler(this.miMarkAllImportable_Click);
            // 
            // cmiSetAllNonImportable
            // 
            this.cmiSetAllNonImportable.Name = "cmiSetAllNonImportable";
            this.cmiSetAllNonImportable.Size = new System.Drawing.Size(226, 22);
            this.cmiSetAllNonImportable.Text = "Unmark all for import";
            this.cmiSetAllNonImportable.Click += new System.EventHandler(this.miMarkAllNonImportable_Click);
            // 
            // cmiSetImportable
            // 
            this.cmiSetImportable.Name = "cmiSetImportable";
            this.cmiSetImportable.Size = new System.Drawing.Size(226, 22);
            this.cmiSetImportable.Text = "Mark for import";
            this.cmiSetImportable.Click += new System.EventHandler(this.miSetImportable_Click);
            // 
            // cmiSetNonImportable
            // 
            this.cmiSetNonImportable.Name = "cmiSetNonImportable";
            this.cmiSetNonImportable.Size = new System.Drawing.Size(226, 22);
            this.cmiSetNonImportable.Text = "Unmark for import";
            this.cmiSetNonImportable.Click += new System.EventHandler(this.miSetNonImportable_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(223, 6);
            // 
            // cmiSetLIMSSampleType
            // 
            this.cmiSetLIMSSampleType.Name = "cmiSetLIMSSampleType";
            this.cmiSetLIMSSampleType.Size = new System.Drawing.Size(226, 22);
            this.cmiSetLIMSSampleType.Text = "Set LIMS &Sample type";
            this.cmiSetLIMSSampleType.Click += new System.EventHandler(this.miSetLIMSSampleType_Click);
            // 
            // cmiSetLIMSSampleComponent
            // 
            this.cmiSetLIMSSampleComponent.Name = "cmiSetLIMSSampleComponent";
            this.cmiSetLIMSSampleComponent.Size = new System.Drawing.Size(226, 22);
            this.cmiSetLIMSSampleComponent.Text = "Set LIMS Sample component";
            this.cmiSetLIMSSampleComponent.Click += new System.EventHandler(this.miSetLIMSSampleComponent_Click);
            // 
            // cmiSetExternalId
            // 
            this.cmiSetExternalId.Name = "cmiSetExternalId";
            this.cmiSetExternalId.Size = new System.Drawing.Size(226, 22);
            this.cmiSetExternalId.Text = "Set &External id";
            this.cmiSetExternalId.Click += new System.EventHandler(this.miSetExternalId_Click);
            // 
            // setSamplingDateFromToolStripMenuItem
            // 
            this.setSamplingDateFromToolStripMenuItem.Name = "setSamplingDateFromToolStripMenuItem";
            this.setSamplingDateFromToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setSamplingDateFromToolStripMenuItem.Text = "Set Sampling date from";
            this.setSamplingDateFromToolStripMenuItem.Click += new System.EventHandler(this.miSetSamplingDateFrom_Click);
            // 
            // setSamplingDateToToolStripMenuItem
            // 
            this.setSamplingDateToToolStripMenuItem.Name = "setSamplingDateToToolStripMenuItem";
            this.setSamplingDateToToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setSamplingDateToToolStripMenuItem.Text = "Set Sampling date to";
            this.setSamplingDateToToolStripMenuItem.Click += new System.EventHandler(this.miSetSamplingDateTo_Click);
            // 
            // cmiSetReferenceDate
            // 
            this.cmiSetReferenceDate.Name = "cmiSetReferenceDate";
            this.cmiSetReferenceDate.Size = new System.Drawing.Size(226, 22);
            this.cmiSetReferenceDate.Text = "Set Reference date";
            this.cmiSetReferenceDate.Click += new System.EventHandler(this.miSetReferenceDate_Click);
            // 
            // cmiSetLocationType
            // 
            this.cmiSetLocationType.Name = "cmiSetLocationType";
            this.cmiSetLocationType.Size = new System.Drawing.Size(226, 22);
            this.cmiSetLocationType.Text = "Set Location type";
            this.cmiSetLocationType.Click += new System.EventHandler(this.miSetLocationType_Click);
            // 
            // cmiSetLocation
            // 
            this.cmiSetLocation.Name = "cmiSetLocation";
            this.cmiSetLocation.Size = new System.Drawing.Size(226, 22);
            this.cmiSetLocation.Text = "Set Location";
            this.cmiSetLocation.Click += new System.EventHandler(this.miSetLocation_Click);
            // 
            // setLatitudeToolStripMenuItem
            // 
            this.setLatitudeToolStripMenuItem.Name = "setLatitudeToolStripMenuItem";
            this.setLatitudeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLatitudeToolStripMenuItem.Text = "Set Latitude";
            this.setLatitudeToolStripMenuItem.Click += new System.EventHandler(this.miSetLatitude_Click);
            // 
            // setLongitudeToolStripMenuItem
            // 
            this.setLongitudeToolStripMenuItem.Name = "setLongitudeToolStripMenuItem";
            this.setLongitudeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLongitudeToolStripMenuItem.Text = "Set Longitude";
            this.setLongitudeToolStripMenuItem.Click += new System.EventHandler(this.miSetLongitude_Click);
            // 
            // setAltitudeToolStripMenuItem
            // 
            this.setAltitudeToolStripMenuItem.Name = "setAltitudeToolStripMenuItem";
            this.setAltitudeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setAltitudeToolStripMenuItem.Text = "Set Altitude";
            this.setAltitudeToolStripMenuItem.Click += new System.EventHandler(this.miSetAltitude_Click);
            // 
            // setLIMSSamplerToolStripMenuItem
            // 
            this.setLIMSSamplerToolStripMenuItem.Name = "setLIMSSamplerToolStripMenuItem";
            this.setLIMSSamplerToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLIMSSamplerToolStripMenuItem.Text = "Set LIMS Sampler";
            this.setLIMSSamplerToolStripMenuItem.Click += new System.EventHandler(this.miSetSampler_Click);
            // 
            // setLIMSSamplingMethodToolStripMenuItem
            // 
            this.setLIMSSamplingMethodToolStripMenuItem.Name = "setLIMSSamplingMethodToolStripMenuItem";
            this.setLIMSSamplingMethodToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLIMSSamplingMethodToolStripMenuItem.Text = "Set LIMS Sampling Method";
            this.setLIMSSamplingMethodToolStripMenuItem.Click += new System.EventHandler(this.miSetSamplingMethod_Click);
            // 
            // setCommentToolStripMenuItem
            // 
            this.setCommentToolStripMenuItem.Name = "setCommentToolStripMenuItem";
            this.setCommentToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setCommentToolStripMenuItem.Text = "Set Comment";
            this.setCommentToolStripMenuItem.Click += new System.EventHandler(this.miSetComment_Click);
            // 
            // setWeekNumberToolStripMenuItem
            // 
            this.setWeekNumberToolStripMenuItem.Name = "setWeekNumberToolStripMenuItem";
            this.setWeekNumberToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setWeekNumberToolStripMenuItem.Text = "Set Week number";
            this.setWeekNumberToolStripMenuItem.Click += new System.EventHandler(this.miSetWeekNumber_Click);
            // 
            // setFlowVolumeToolStripMenuItem
            // 
            this.setFlowVolumeToolStripMenuItem.Name = "setFlowVolumeToolStripMenuItem";
            this.setFlowVolumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setFlowVolumeToolStripMenuItem.Text = "Set Flow volume";
            this.setFlowVolumeToolStripMenuItem.Click += new System.EventHandler(this.miSetFlowVolume_Click);
            // 
            // setVolumeToolStripMenuItem
            // 
            this.setVolumeToolStripMenuItem.Name = "setVolumeToolStripMenuItem";
            this.setVolumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setVolumeToolStripMenuItem.Text = "Set Volume";
            this.setVolumeToolStripMenuItem.Click += new System.EventHandler(this.miSetVolume_Click);
            // 
            // setDepthToolStripMenuItem
            // 
            this.setDepthToolStripMenuItem.Name = "setDepthToolStripMenuItem";
            this.setDepthToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setDepthToolStripMenuItem.Text = "Set Depth";
            this.setDepthToolStripMenuItem.Click += new System.EventHandler(this.miSetDepth_Click);
            // 
            // setDryWeightToolStripMenuItem
            // 
            this.setDryWeightToolStripMenuItem.Name = "setDryWeightToolStripMenuItem";
            this.setDryWeightToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setDryWeightToolStripMenuItem.Text = "Set Dry weight";
            this.setDryWeightToolStripMenuItem.Click += new System.EventHandler(this.miSetDryWeight_Click);
            // 
            // setWetWeightToolStripMenuItem
            // 
            this.setWetWeightToolStripMenuItem.Name = "setWetWeightToolStripMenuItem";
            this.setWetWeightToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setWetWeightToolStripMenuItem.Text = "Set Wet weight";
            this.setWetWeightToolStripMenuItem.Click += new System.EventHandler(this.miSetWetWeight_Click);
            // 
            // setAirSpeedToolStripMenuItem
            // 
            this.setAirSpeedToolStripMenuItem.Name = "setAirSpeedToolStripMenuItem";
            this.setAirSpeedToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setAirSpeedToolStripMenuItem.Text = "Set Air speed";
            this.setAirSpeedToolStripMenuItem.Click += new System.EventHandler(this.miSetAirSpeed_Click);
            // 
            // setSalinityToolStripMenuItem
            // 
            this.setSalinityToolStripMenuItem.Name = "setSalinityToolStripMenuItem";
            this.setSalinityToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setSalinityToolStripMenuItem.Text = "Set Salinity";
            this.setSalinityToolStripMenuItem.Click += new System.EventHandler(this.miSetSalinity_Click);
            // 
            // setLayerFromToolStripMenuItem
            // 
            this.setLayerFromToolStripMenuItem.Name = "setLayerFromToolStripMenuItem";
            this.setLayerFromToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLayerFromToolStripMenuItem.Text = "Set Layer from";
            this.setLayerFromToolStripMenuItem.Click += new System.EventHandler(this.miSetLayerFrom_Click);
            // 
            // setLayerToToolStripMenuItem
            // 
            this.setLayerToToolStripMenuItem.Name = "setLayerToToolStripMenuItem";
            this.setLayerToToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setLayerToToolStripMenuItem.Text = "Set Layer to";
            this.setLayerToToolStripMenuItem.Click += new System.EventHandler(this.miSetLayerTo_Click);
            // 
            // setAreaToolStripMenuItem
            // 
            this.setAreaToolStripMenuItem.Name = "setAreaToolStripMenuItem";
            this.setAreaToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setAreaToolStripMenuItem.Text = "Set Area";
            this.setAreaToolStripMenuItem.Click += new System.EventHandler(this.miSetArea_Click);
            // 
            // setTemperatureToolStripMenuItem
            // 
            this.setTemperatureToolStripMenuItem.Name = "setTemperatureToolStripMenuItem";
            this.setTemperatureToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setTemperatureToolStripMenuItem.Text = "Set Temperature";
            this.setTemperatureToolStripMenuItem.Click += new System.EventHandler(this.miSetTemperature_Click);
            // 
            // setTracerNuclideToolStripMenuItem
            // 
            this.setTracerNuclideToolStripMenuItem.Name = "setTracerNuclideToolStripMenuItem";
            this.setTracerNuclideToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setTracerNuclideToolStripMenuItem.Text = "Set Tracer nuclide";
            this.setTracerNuclideToolStripMenuItem.Click += new System.EventHandler(this.miSetTracerNuclide_Click);
            // 
            // setTracerWeightToolStripMenuItem
            // 
            this.setTracerWeightToolStripMenuItem.Name = "setTracerWeightToolStripMenuItem";
            this.setTracerWeightToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setTracerWeightToolStripMenuItem.Text = "Set Tracer weight";
            this.setTracerWeightToolStripMenuItem.Click += new System.EventHandler(this.miSetTracerWeight_Click);
            // 
            // setTracerVolumeToolStripMenuItem
            // 
            this.setTracerVolumeToolStripMenuItem.Name = "setTracerVolumeToolStripMenuItem";
            this.setTracerVolumeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setTracerVolumeToolStripMenuItem.Text = "Set Tracer volume";
            this.setTracerVolumeToolStripMenuItem.Click += new System.EventHandler(this.miSetTracerVolume_Click);
            // 
            // setIntendedAnalysisToolStripMenuItem
            // 
            this.setIntendedAnalysisToolStripMenuItem.Name = "setIntendedAnalysisToolStripMenuItem";
            this.setIntendedAnalysisToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setIntendedAnalysisToolStripMenuItem.Text = "Set Intended analysis";
            this.setIntendedAnalysisToolStripMenuItem.Click += new System.EventHandler(this.miSetIntendedAnalysis_Click);
            // 
            // setPHToolStripMenuItem
            // 
            this.setPHToolStripMenuItem.Name = "setPHToolStripMenuItem";
            this.setPHToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setPHToolStripMenuItem.Text = "Set PH";
            this.setPHToolStripMenuItem.Click += new System.EventHandler(this.miSetPH_Click);
            // 
            // setRequireFilteringToolStripMenuItem
            // 
            this.setRequireFilteringToolStripMenuItem.Name = "setRequireFilteringToolStripMenuItem";
            this.setRequireFilteringToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.setRequireFilteringToolStripMenuItem.Text = "Set Require filtering";
            this.setRequireFilteringToolStripMenuItem.Click += new System.EventHandler(this.miSetRequireFiltering_Click);
            // 
            // label4
            // 
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "File Id";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbFileID
            // 
            this.tbFileID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbFileID.Location = new System.Drawing.Point(45, 0);
            this.tbFileID.Name = "tbFileID";
            this.tbFileID.ReadOnly = true;
            this.tbFileID.Size = new System.Drawing.Size(308, 21);
            this.tbFileID.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "File project";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbProject
            // 
            this.tbProject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbProject.Location = new System.Drawing.Point(85, 0);
            this.tbProject.Name = "tbProject";
            this.tbProject.ReadOnly = true;
            this.tbProject.Size = new System.Drawing.Size(295, 21);
            this.tbProject.TabIndex = 4;
            // 
            // panelImport
            // 
            this.panelImport.Controls.Add(this.gridSamples);
            this.panelImport.Controls.Add(this.panel1);
            this.panelImport.Controls.Add(this.statusStrip1);
            this.panelImport.Controls.Add(this.toolStrip1);
            this.panelImport.Controls.Add(this.flowLayoutPanel1);
            this.panelImport.Controls.Add(this.tools);
            this.panelImport.Controls.Add(this.menuMain);
            this.panelImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImport.Location = new System.Drawing.Point(0, 0);
            this.panelImport.Name = "panelImport";
            this.panelImport.Size = new System.Drawing.Size(984, 661);
            this.panelImport.TabIndex = 11;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressImport,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 639);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 16;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressImport
            // 
            this.progressImport.Name = "progressImport";
            this.progressImport.Size = new System.Drawing.Size(100, 16);
            this.progressImport.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(68, 17);
            this.lblStatus.Text = "<lblStatus>";
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMarkAllImportable,
            this.btnUnmarkAllImportable,
            this.toolStripSeparator4,
            this.btnSetImportable,
            this.btnSetNonImportable});
            this.toolStrip1.Location = new System.Drawing.Point(0, 86);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnMarkAllImportable
            // 
            this.btnMarkAllImportable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMarkAllImportable.Image = global::DSA_lims.Properties.Resources.check1;
            this.btnMarkAllImportable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMarkAllImportable.Name = "btnMarkAllImportable";
            this.btnMarkAllImportable.Size = new System.Drawing.Size(23, 22);
            this.btnMarkAllImportable.Text = "Mark all for import";
            this.btnMarkAllImportable.ToolTipText = "Mark all samples for import";
            this.btnMarkAllImportable.Click += new System.EventHandler(this.miMarkAllImportable_Click);
            // 
            // btnUnmarkAllImportable
            // 
            this.btnUnmarkAllImportable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnUnmarkAllImportable.Image = global::DSA_lims.Properties.Resources.uncheck1;
            this.btnUnmarkAllImportable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnmarkAllImportable.Name = "btnUnmarkAllImportable";
            this.btnUnmarkAllImportable.Size = new System.Drawing.Size(23, 22);
            this.btnUnmarkAllImportable.Text = "Unmark all for import";
            this.btnUnmarkAllImportable.ToolTipText = "Unmark all samples for import";
            this.btnUnmarkAllImportable.Click += new System.EventHandler(this.miMarkAllNonImportable_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSetImportable
            // 
            this.btnSetImportable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSetImportable.Image = global::DSA_lims.Properties.Resources.check2;
            this.btnSetImportable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetImportable.Name = "btnSetImportable";
            this.btnSetImportable.Size = new System.Drawing.Size(23, 22);
            this.btnSetImportable.Text = "Mark selected for import";
            this.btnSetImportable.ToolTipText = "Mark selected samples for import";
            this.btnSetImportable.Click += new System.EventHandler(this.miSetImportable_Click);
            // 
            // btnSetNonImportable
            // 
            this.btnSetNonImportable.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSetNonImportable.Image = global::DSA_lims.Properties.Resources.uncheck2;
            this.btnSetNonImportable.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetNonImportable.Name = "btnSetNonImportable";
            this.btnSetNonImportable.Size = new System.Drawing.Size(23, 22);
            this.btnSetNonImportable.Text = "Unmark selected for import";
            this.btnSetNonImportable.ToolTipText = "Unmark selected samples for import";
            this.btnSetNonImportable.Click += new System.EventHandler(this.miSetNonImportable_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 49);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(984, 37);
            this.flowLayoutPanel1.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbFileID);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(8, 8);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(353, 21);
            this.panel2.TabIndex = 7;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tbProject);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Location = new System.Drawing.Point(367, 8);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(380, 21);
            this.panel4.TabIndex = 8;
            // 
            // tools
            // 
            this.tools.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectFileLimsSamples,
            this.btnSelectFileSampleReg,
            this.toolStripButton1});
            this.tools.Location = new System.Drawing.Point(0, 24);
            this.tools.Name = "tools";
            this.tools.Size = new System.Drawing.Size(984, 25);
            this.tools.TabIndex = 13;
            this.tools.Text = "toolStrip1";
            // 
            // btnSelectFileLimsSamples
            // 
            this.btnSelectFileLimsSamples.Image = global::DSA_lims.Properties.Resources.file_16;
            this.btnSelectFileLimsSamples.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectFileLimsSamples.Name = "btnSelectFileLimsSamples";
            this.btnSelectFileLimsSamples.Size = new System.Drawing.Size(123, 22);
            this.btnSelectFileLimsSamples.Text = "DSA Lims samples";
            this.btnSelectFileLimsSamples.ToolTipText = "Import samples from DSA Lims sample sheet";
            this.btnSelectFileLimsSamples.Click += new System.EventHandler(this.miImportLIMSSamples_Click);
            // 
            // btnSelectFileSampleReg
            // 
            this.btnSelectFileSampleReg.Image = global::DSA_lims.Properties.Resources.file_16;
            this.btnSelectFileSampleReg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectFileSampleReg.Name = "btnSelectFileSampleReg";
            this.btnSelectFileSampleReg.Size = new System.Drawing.Size(157, 22);
            this.btnSelectFileSampleReg.Text = "Sample Registration App";
            this.btnSelectFileSampleReg.ToolTipText = "Import samples from Sample Registration Mobile App";
            this.btnSelectFileSampleReg.Click += new System.EventHandler(this.miImportSampleRegistration_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = global::DSA_lims.Properties.Resources.file_16;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(105, 22);
            this.toolStripButton1.Text = "MINA Samples";
            this.toolStripButton1.ToolTipText = "Import samples from MINA Sample sheet";
            this.toolStripButton1.Click += new System.EventHandler(this.miImportMINASamples_Click);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miSet});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(984, 24);
            this.menuMain.TabIndex = 12;
            this.menuMain.Text = "menuStrip1";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miImport,
            this.toolStripSeparator1,
            this.miClose});
            this.miFile.Name = "miFile";
            this.miFile.Size = new System.Drawing.Size(37, 20);
            this.miFile.Text = "&File";
            // 
            // miImport
            // 
            this.miImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miImportLIMSSamples,
            this.miImportSampleRegistration,
            this.miImportMINASamples});
            this.miImport.Name = "miImport";
            this.miImport.Size = new System.Drawing.Size(110, 22);
            this.miImport.Text = "&Import";
            // 
            // miImportLIMSSamples
            // 
            this.miImportLIMSSamples.Name = "miImportLIMSSamples";
            this.miImportLIMSSamples.Size = new System.Drawing.Size(209, 22);
            this.miImportLIMSSamples.Text = "DSA Lims samples (Excel)";
            this.miImportLIMSSamples.Click += new System.EventHandler(this.miImportLIMSSamples_Click);
            // 
            // miImportSampleRegistration
            // 
            this.miImportSampleRegistration.Name = "miImportSampleRegistration";
            this.miImportSampleRegistration.Size = new System.Drawing.Size(209, 22);
            this.miImportSampleRegistration.Text = "Sample registration (App)";
            this.miImportSampleRegistration.Click += new System.EventHandler(this.miImportSampleRegistration_Click);
            // 
            // miImportMINASamples
            // 
            this.miImportMINASamples.Name = "miImportMINASamples";
            this.miImportMINASamples.Size = new System.Drawing.Size(209, 22);
            this.miImportMINASamples.Text = "MINA Samples (Excel)";
            this.miImportMINASamples.Click += new System.EventHandler(this.miImportMINASamples_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(107, 6);
            // 
            // miClose
            // 
            this.miClose.Name = "miClose";
            this.miClose.Size = new System.Drawing.Size(110, 22);
            this.miClose.Text = "&Close";
            this.miClose.Click += new System.EventHandler(this.miClose_Click);
            // 
            // miSet
            // 
            this.miSet.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miMarkAllImportable,
            this.miMarkAllNonImportable,
            this.miSetImportable,
            this.miSetNonImportable,
            this.toolStripSeparator3,
            this.miSetLIMSSampleType,
            this.miSetLIMSSampleComponent,
            this.miSetExternalId,
            this.miSetSamplingDateFrom,
            this.miSetSamplingDateTo,
            this.miSetReferenceDate,
            this.miSetLocationType,
            this.miSetLocation,
            this.miSetLatitude,
            this.miSetLongitude,
            this.miSetAltitude,
            this.miSetSampler,
            this.miSetSamplingMethod,
            this.miSetComment,
            this.miSetWeekNumber,
            this.miSetFlowVolume,
            this.miSetVolume,
            this.miSetDepth,
            this.miSetDryWeight,
            this.miSetWetWeight,
            this.miSetAirSpeed,
            this.miSetSalinity,
            this.miSetLayerFrom,
            this.miSetLayerTo,
            this.miSetArea,
            this.miSetTemperature,
            this.miSetTracerNuclide,
            this.miSetTracerWeight,
            this.miSetTracerVolume,
            this.miSetIntendedAnalysis,
            this.miSetPH,
            this.miSetRequireFiltering});
            this.miSet.Name = "miSet";
            this.miSet.Size = new System.Drawing.Size(44, 20);
            this.miSet.Text = "Set...";
            // 
            // miMarkAllImportable
            // 
            this.miMarkAllImportable.Name = "miMarkAllImportable";
            this.miMarkAllImportable.Size = new System.Drawing.Size(207, 22);
            this.miMarkAllImportable.Text = "Mark all for import";
            this.miMarkAllImportable.Click += new System.EventHandler(this.miMarkAllImportable_Click);
            // 
            // miMarkAllNonImportable
            // 
            this.miMarkAllNonImportable.Name = "miMarkAllNonImportable";
            this.miMarkAllNonImportable.Size = new System.Drawing.Size(207, 22);
            this.miMarkAllNonImportable.Text = "Unmark all for import";
            this.miMarkAllNonImportable.Click += new System.EventHandler(this.miMarkAllNonImportable_Click);
            // 
            // miSetImportable
            // 
            this.miSetImportable.Name = "miSetImportable";
            this.miSetImportable.Size = new System.Drawing.Size(207, 22);
            this.miSetImportable.Text = "Mark for import";
            this.miSetImportable.Click += new System.EventHandler(this.miSetImportable_Click);
            // 
            // miSetNonImportable
            // 
            this.miSetNonImportable.Name = "miSetNonImportable";
            this.miSetNonImportable.Size = new System.Drawing.Size(207, 22);
            this.miSetNonImportable.Text = "Unmark for import";
            this.miSetNonImportable.Click += new System.EventHandler(this.miSetNonImportable_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(204, 6);
            // 
            // miSetLIMSSampleType
            // 
            this.miSetLIMSSampleType.Name = "miSetLIMSSampleType";
            this.miSetLIMSSampleType.Size = new System.Drawing.Size(207, 22);
            this.miSetLIMSSampleType.Text = "LIMS Sample type";
            this.miSetLIMSSampleType.Click += new System.EventHandler(this.miSetLIMSSampleType_Click);
            // 
            // miSetLIMSSampleComponent
            // 
            this.miSetLIMSSampleComponent.Name = "miSetLIMSSampleComponent";
            this.miSetLIMSSampleComponent.Size = new System.Drawing.Size(207, 22);
            this.miSetLIMSSampleComponent.Text = "LIMS Sample component";
            this.miSetLIMSSampleComponent.Click += new System.EventHandler(this.miSetLIMSSampleComponent_Click);
            // 
            // miSetExternalId
            // 
            this.miSetExternalId.Name = "miSetExternalId";
            this.miSetExternalId.Size = new System.Drawing.Size(207, 22);
            this.miSetExternalId.Text = "External Id";
            this.miSetExternalId.Click += new System.EventHandler(this.miSetExternalId_Click);
            // 
            // miSetSamplingDateFrom
            // 
            this.miSetSamplingDateFrom.Name = "miSetSamplingDateFrom";
            this.miSetSamplingDateFrom.Size = new System.Drawing.Size(207, 22);
            this.miSetSamplingDateFrom.Text = "Sampling date from";
            this.miSetSamplingDateFrom.Click += new System.EventHandler(this.miSetSamplingDateFrom_Click);
            // 
            // miSetSamplingDateTo
            // 
            this.miSetSamplingDateTo.Name = "miSetSamplingDateTo";
            this.miSetSamplingDateTo.Size = new System.Drawing.Size(207, 22);
            this.miSetSamplingDateTo.Text = "Sampling date to";
            this.miSetSamplingDateTo.Click += new System.EventHandler(this.miSetSamplingDateTo_Click);
            // 
            // miSetReferenceDate
            // 
            this.miSetReferenceDate.Name = "miSetReferenceDate";
            this.miSetReferenceDate.Size = new System.Drawing.Size(207, 22);
            this.miSetReferenceDate.Text = "Reference date";
            this.miSetReferenceDate.Click += new System.EventHandler(this.miSetReferenceDate_Click);
            // 
            // miSetLocationType
            // 
            this.miSetLocationType.Name = "miSetLocationType";
            this.miSetLocationType.Size = new System.Drawing.Size(207, 22);
            this.miSetLocationType.Text = "LIMS Location type";
            this.miSetLocationType.Click += new System.EventHandler(this.miSetLocationType_Click);
            // 
            // miSetLocation
            // 
            this.miSetLocation.Name = "miSetLocation";
            this.miSetLocation.Size = new System.Drawing.Size(207, 22);
            this.miSetLocation.Text = "Location";
            this.miSetLocation.Click += new System.EventHandler(this.miSetLocation_Click);
            // 
            // miSetLatitude
            // 
            this.miSetLatitude.Name = "miSetLatitude";
            this.miSetLatitude.Size = new System.Drawing.Size(207, 22);
            this.miSetLatitude.Text = "Latitude";
            this.miSetLatitude.Click += new System.EventHandler(this.miSetLatitude_Click);
            // 
            // miSetLongitude
            // 
            this.miSetLongitude.Name = "miSetLongitude";
            this.miSetLongitude.Size = new System.Drawing.Size(207, 22);
            this.miSetLongitude.Text = "Longitude";
            this.miSetLongitude.Click += new System.EventHandler(this.miSetLongitude_Click);
            // 
            // miSetAltitude
            // 
            this.miSetAltitude.Name = "miSetAltitude";
            this.miSetAltitude.Size = new System.Drawing.Size(207, 22);
            this.miSetAltitude.Text = "Altitude";
            this.miSetAltitude.Click += new System.EventHandler(this.miSetAltitude_Click);
            // 
            // miSetSampler
            // 
            this.miSetSampler.Name = "miSetSampler";
            this.miSetSampler.Size = new System.Drawing.Size(207, 22);
            this.miSetSampler.Text = "LIMS Sampler";
            this.miSetSampler.Click += new System.EventHandler(this.miSetSampler_Click);
            // 
            // miSetSamplingMethod
            // 
            this.miSetSamplingMethod.Name = "miSetSamplingMethod";
            this.miSetSamplingMethod.Size = new System.Drawing.Size(207, 22);
            this.miSetSamplingMethod.Text = "LIMS Sampling method";
            this.miSetSamplingMethod.Click += new System.EventHandler(this.miSetSamplingMethod_Click);
            // 
            // miSetComment
            // 
            this.miSetComment.Name = "miSetComment";
            this.miSetComment.Size = new System.Drawing.Size(207, 22);
            this.miSetComment.Text = "Comment";
            this.miSetComment.Click += new System.EventHandler(this.miSetComment_Click);
            // 
            // miSetWeekNumber
            // 
            this.miSetWeekNumber.Name = "miSetWeekNumber";
            this.miSetWeekNumber.Size = new System.Drawing.Size(207, 22);
            this.miSetWeekNumber.Text = "Week number";
            this.miSetWeekNumber.Click += new System.EventHandler(this.miSetWeekNumber_Click);
            // 
            // miSetFlowVolume
            // 
            this.miSetFlowVolume.Name = "miSetFlowVolume";
            this.miSetFlowVolume.Size = new System.Drawing.Size(207, 22);
            this.miSetFlowVolume.Text = "Flow volume";
            this.miSetFlowVolume.Click += new System.EventHandler(this.miSetFlowVolume_Click);
            // 
            // miSetVolume
            // 
            this.miSetVolume.Name = "miSetVolume";
            this.miSetVolume.Size = new System.Drawing.Size(207, 22);
            this.miSetVolume.Text = "Volume";
            this.miSetVolume.Click += new System.EventHandler(this.miSetVolume_Click);
            // 
            // miSetDepth
            // 
            this.miSetDepth.Name = "miSetDepth";
            this.miSetDepth.Size = new System.Drawing.Size(207, 22);
            this.miSetDepth.Text = "Depth";
            this.miSetDepth.Click += new System.EventHandler(this.miSetDepth_Click);
            // 
            // miSetDryWeight
            // 
            this.miSetDryWeight.Name = "miSetDryWeight";
            this.miSetDryWeight.Size = new System.Drawing.Size(207, 22);
            this.miSetDryWeight.Text = "Dry weight";
            this.miSetDryWeight.Click += new System.EventHandler(this.miSetDryWeight_Click);
            // 
            // miSetWetWeight
            // 
            this.miSetWetWeight.Name = "miSetWetWeight";
            this.miSetWetWeight.Size = new System.Drawing.Size(207, 22);
            this.miSetWetWeight.Text = "Wet weight";
            this.miSetWetWeight.Click += new System.EventHandler(this.miSetWetWeight_Click);
            // 
            // miSetAirSpeed
            // 
            this.miSetAirSpeed.Name = "miSetAirSpeed";
            this.miSetAirSpeed.Size = new System.Drawing.Size(207, 22);
            this.miSetAirSpeed.Text = "Air speed";
            this.miSetAirSpeed.Click += new System.EventHandler(this.miSetAirSpeed_Click);
            // 
            // miSetSalinity
            // 
            this.miSetSalinity.Name = "miSetSalinity";
            this.miSetSalinity.Size = new System.Drawing.Size(207, 22);
            this.miSetSalinity.Text = "Salinity";
            this.miSetSalinity.Click += new System.EventHandler(this.miSetSalinity_Click);
            // 
            // miSetLayerFrom
            // 
            this.miSetLayerFrom.Name = "miSetLayerFrom";
            this.miSetLayerFrom.Size = new System.Drawing.Size(207, 22);
            this.miSetLayerFrom.Text = "Layer from";
            this.miSetLayerFrom.Click += new System.EventHandler(this.miSetLayerFrom_Click);
            // 
            // miSetLayerTo
            // 
            this.miSetLayerTo.Name = "miSetLayerTo";
            this.miSetLayerTo.Size = new System.Drawing.Size(207, 22);
            this.miSetLayerTo.Text = "Layer to";
            this.miSetLayerTo.Click += new System.EventHandler(this.miSetLayerTo_Click);
            // 
            // miSetArea
            // 
            this.miSetArea.Name = "miSetArea";
            this.miSetArea.Size = new System.Drawing.Size(207, 22);
            this.miSetArea.Text = "Area";
            this.miSetArea.Click += new System.EventHandler(this.miSetArea_Click);
            // 
            // miSetTemperature
            // 
            this.miSetTemperature.Name = "miSetTemperature";
            this.miSetTemperature.Size = new System.Drawing.Size(207, 22);
            this.miSetTemperature.Text = "Temperature";
            this.miSetTemperature.Click += new System.EventHandler(this.miSetTemperature_Click);
            // 
            // miSetTracerNuclide
            // 
            this.miSetTracerNuclide.Name = "miSetTracerNuclide";
            this.miSetTracerNuclide.Size = new System.Drawing.Size(207, 22);
            this.miSetTracerNuclide.Text = "Tracer nuclide";
            this.miSetTracerNuclide.Click += new System.EventHandler(this.miSetTracerNuclide_Click);
            // 
            // miSetTracerWeight
            // 
            this.miSetTracerWeight.Name = "miSetTracerWeight";
            this.miSetTracerWeight.Size = new System.Drawing.Size(207, 22);
            this.miSetTracerWeight.Text = "Tracer weight";
            this.miSetTracerWeight.Click += new System.EventHandler(this.miSetTracerWeight_Click);
            // 
            // miSetTracerVolume
            // 
            this.miSetTracerVolume.Name = "miSetTracerVolume";
            this.miSetTracerVolume.Size = new System.Drawing.Size(207, 22);
            this.miSetTracerVolume.Text = "Tracer volume";
            this.miSetTracerVolume.Click += new System.EventHandler(this.miSetTracerVolume_Click);
            // 
            // miSetIntendedAnalysis
            // 
            this.miSetIntendedAnalysis.Name = "miSetIntendedAnalysis";
            this.miSetIntendedAnalysis.Size = new System.Drawing.Size(207, 22);
            this.miSetIntendedAnalysis.Text = "Intended analysis";
            this.miSetIntendedAnalysis.Click += new System.EventHandler(this.miSetIntendedAnalysis_Click);
            // 
            // miSetPH
            // 
            this.miSetPH.Name = "miSetPH";
            this.miSetPH.Size = new System.Drawing.Size(207, 22);
            this.miSetPH.Text = "pH";
            this.miSetPH.Click += new System.EventHandler(this.miSetPH_Click);
            // 
            // miSetRequireFiltering
            // 
            this.miSetRequireFiltering.Name = "miSetRequireFiltering";
            this.miSetRequireFiltering.Size = new System.Drawing.Size(207, 22);
            this.miSetRequireFiltering.Text = "Require filtering";
            this.miSetRequireFiltering.Click += new System.EventHandler(this.miSetRequireFiltering_Click);
            // 
            // FormImportSamples
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 661);
            this.Controls.Add(this.panelImport);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuMain;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FormImportSamples";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DSA-Lims - Import samples";
            this.Load += new System.EventHandler(this.FormImportSamplesSampReg_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSamples)).EndInit();
            this.cmenuSamples.ResumeLayout(false);
            this.panelImport.ResumeLayout(false);
            this.panelImport.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tools.ResumeLayout(false);
            this.tools.PerformLayout();
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.DataGridView gridSamples;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbProject;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFileID;
        private System.Windows.Forms.Panel panelImport;
        private System.Windows.Forms.ToolStrip tools;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripButton btnSelectFileLimsSamples;
        private System.Windows.Forms.ToolStripButton btnSelectFileSampleReg;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ContextMenuStrip cmenuSamples;
        private System.Windows.Forms.ToolStripMenuItem cmiSetExternalId;
        private System.Windows.Forms.ToolStripMenuItem miFile;
        private System.Windows.Forms.ToolStripMenuItem miImport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem miClose;
        private System.Windows.Forms.ToolStripMenuItem miImportLIMSSamples;
        private System.Windows.Forms.ToolStripMenuItem miImportSampleRegistration;
        private System.Windows.Forms.ToolStripMenuItem miSet;
        private System.Windows.Forms.ToolStripMenuItem miSetLIMSSampleType;
        private System.Windows.Forms.ToolStripMenuItem miSetLIMSSampleComponent;
        private System.Windows.Forms.ToolStripMenuItem miSetExternalId;
        private System.Windows.Forms.ToolStripMenuItem cmiSetLIMSSampleType;
        private System.Windows.Forms.ToolStripMenuItem cmiSetLIMSSampleComponent;
        private System.Windows.Forms.ToolStripMenuItem miSetSamplingDateFrom;
        private System.Windows.Forms.ToolStripMenuItem miSetSamplingDateTo;
        private System.Windows.Forms.ToolStripMenuItem setSamplingDateFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSamplingDateToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem miSetReferenceDate;
        private System.Windows.Forms.ToolStripMenuItem miSetLocationType;
        private System.Windows.Forms.ToolStripMenuItem miSetLocation;
        private System.Windows.Forms.ToolStripMenuItem cmiSetReferenceDate;
        private System.Windows.Forms.ToolStripMenuItem cmiSetLocationType;
        private System.Windows.Forms.ToolStripMenuItem cmiSetLocation;
        private System.Windows.Forms.ToolStripMenuItem miSetLatitude;
        private System.Windows.Forms.ToolStripMenuItem miSetLongitude;
        private System.Windows.Forms.ToolStripMenuItem miSetAltitude;
        private System.Windows.Forms.ToolStripMenuItem setLatitudeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLongitudeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAltitudeToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox cboxProjectSub;
        private System.Windows.Forms.ComboBox cboxProjectMain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxLaboratory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem miSetSampler;
        private System.Windows.Forms.ToolStripMenuItem miSetSamplingMethod;
        private System.Windows.Forms.ToolStripMenuItem miSetComment;
        private System.Windows.Forms.ToolStripMenuItem miSetWeekNumber;
        private System.Windows.Forms.ToolStripMenuItem miSetFlowVolume;
        private System.Windows.Forms.ToolStripMenuItem miSetVolume;
        private System.Windows.Forms.ToolStripMenuItem miSetDepth;
        private System.Windows.Forms.ToolStripMenuItem miSetDryWeight;
        private System.Windows.Forms.ToolStripMenuItem miSetWetWeight;
        private System.Windows.Forms.ToolStripMenuItem miSetAirSpeed;
        private System.Windows.Forms.ToolStripMenuItem miSetSalinity;
        private System.Windows.Forms.ToolStripMenuItem miSetLayerFrom;
        private System.Windows.Forms.ToolStripMenuItem miSetLayerTo;
        private System.Windows.Forms.ToolStripMenuItem miSetArea;
        private System.Windows.Forms.ToolStripMenuItem miSetTemperature;
        private System.Windows.Forms.ToolStripMenuItem miSetTracerNuclide;
        private System.Windows.Forms.ToolStripMenuItem miSetTracerWeight;
        private System.Windows.Forms.ToolStripMenuItem miSetTracerVolume;
        private System.Windows.Forms.ToolStripMenuItem miSetIntendedAnalysis;
        private System.Windows.Forms.ToolStripMenuItem miSetPH;
        private System.Windows.Forms.ToolStripMenuItem miSetRequireFiltering;
        private System.Windows.Forms.ToolStripMenuItem setLIMSSamplerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLIMSSamplingMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setCommentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWeekNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFlowVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDepthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setDryWeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWetWeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAirSpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSalinityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLayerFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setLayerToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAreaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTemperatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTracerNuclideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTracerWeightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTracerVolumeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setIntendedAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setPHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setRequireFilteringToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colImportable;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSSampleType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleComponent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSSampComp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExternalId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSamplingDateFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSamplingDateTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReferenceDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSLocationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLatitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLongitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAltitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSSampler;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSamplingMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSSamplingMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWeekNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFlowVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDryWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWetWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAirSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalinity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLayerFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLayerTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTracerNuclide;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLIMSTracerNuclide;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTracerWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTracerVolume;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIntendedAnalysis;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPH;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequireFiltering;
        private System.Windows.Forms.ToolStripButton btnSetImportable;
        private System.Windows.Forms.ToolStripMenuItem miSetImportable;
        private System.Windows.Forms.ToolStripMenuItem miSetNonImportable;
        private System.Windows.Forms.ToolStripButton btnSetNonImportable;
        private System.Windows.Forms.ToolStripMenuItem cmiSetImportable;
        private System.Windows.Forms.ToolStripMenuItem cmiSetNonImportable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnMarkAllImportable;
        private System.Windows.Forms.ToolStripButton btnUnmarkAllImportable;
        private System.Windows.Forms.ToolStripMenuItem miMarkAllImportable;
        private System.Windows.Forms.ToolStripMenuItem miMarkAllNonImportable;
        private System.Windows.Forms.ToolStripMenuItem cmiSetAllImportable;
        private System.Windows.Forms.ToolStripMenuItem cmiSetAllNonImportable;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ToolStripMenuItem miImportMINASamples;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripProgressBar progressImport;
    }
}