namespace DSA_lims
{
    partial class FormSampleView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSampleView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.tblInfo = new System.Windows.Forms.DataGridView();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgSampleViewAttachments = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblInfo)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSampleViewAttachments)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 721);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(922, 28);
            this.panel1.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(789, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(133, 28);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tblInfo
            // 
            this.tblInfo.AllowUserToAddRows = false;
            this.tblInfo.AllowUserToDeleteRows = false;
            this.tblInfo.AllowUserToResizeColumns = false;
            this.tblInfo.AllowUserToResizeRows = false;
            this.tblInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tblInfo.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.tblInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tblInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblInfo.ColumnHeadersVisible = false;
            this.tblInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTitle,
            this.colValue});
            this.tblInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblInfo.Location = new System.Drawing.Point(0, 0);
            this.tblInfo.MultiSelect = false;
            this.tblInfo.Name = "tblInfo";
            this.tblInfo.ReadOnly = true;
            this.tblInfo.RowHeadersVisible = false;
            this.tblInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblInfo.Size = new System.Drawing.Size(922, 420);
            this.tblInfo.TabIndex = 12;
            // 
            // colTitle
            // 
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.colTitle.FillWeight = 194.9239F;
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Width = 5;
            // 
            // colValue
            // 
            this.colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colValue.FillWeight = 5.076141F;
            this.colValue.HeaderText = "Value";
            this.colValue.Name = "colValue";
            this.colValue.ReadOnly = true;
            // 
            // tbComment
            // 
            this.tbComment.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbComment.Dock = System.Windows.Forms.DockStyle.Left;
            this.tbComment.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbComment.Location = new System.Drawing.Point(0, 0);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ReadOnly = true;
            this.tbComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComment.Size = new System.Drawing.Size(468, 301);
            this.tbComment.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgSampleViewAttachments);
            this.panel2.Controls.Add(this.tbComment);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 420);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(922, 301);
            this.panel2.TabIndex = 14;
            // 
            // dgSampleViewAttachments
            // 
            this.dgSampleViewAttachments.AllowUserToAddRows = false;
            this.dgSampleViewAttachments.AllowUserToDeleteRows = false;
            this.dgSampleViewAttachments.AllowUserToResizeRows = false;
            this.dgSampleViewAttachments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgSampleViewAttachments.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dgSampleViewAttachments.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgSampleViewAttachments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSampleViewAttachments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSampleViewAttachments.Location = new System.Drawing.Point(468, 0);
            this.dgSampleViewAttachments.MultiSelect = false;
            this.dgSampleViewAttachments.Name = "dgSampleViewAttachments";
            this.dgSampleViewAttachments.ReadOnly = true;
            this.dgSampleViewAttachments.RowHeadersVisible = false;
            this.dgSampleViewAttachments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSampleViewAttachments.Size = new System.Drawing.Size(454, 301);
            this.dgSampleViewAttachments.TabIndex = 14;
            this.dgSampleViewAttachments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgSampleViewAttachments_CellDoubleClick);
            // 
            // FormSampleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 749);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tblInfo);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSampleView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DSA Lims - View Sample";
            this.Load += new System.EventHandler(this.FormSampleView_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tblInfo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSampleViewAttachments)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView tblInfo;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dgSampleViewAttachments;
    }
}