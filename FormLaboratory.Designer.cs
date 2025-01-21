namespace DSA_lims
{
    partial class FormLaboratory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLaboratory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPrefix = new System.Windows.Forms.TextBox();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboxInstanceStatus = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.picLaboratoryLogo = new System.Windows.Forms.PictureBox();
            this.picAccreditedLogo = new System.Windows.Forms.PictureBox();
            this.panelLaboratoryLogo = new System.Windows.Forms.FlowLayoutPanel();
            this.panelAccreditedLogo = new System.Windows.Forms.FlowLayoutPanel();
            this.linkLaboratoryLogo = new System.Windows.Forms.LinkLabel();
            this.linkAccreditedLogo = new System.Windows.Forms.LinkLabel();
            this.lblLaboratoryLogoSize = new System.Windows.Forms.Label();
            this.lblAccreditedLogoSize = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbISODesc = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLaboratoryLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccreditedLogo)).BeginInit();
            this.panelLaboratoryLogo.SuspendLayout();
            this.panelAccreditedLogo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 455);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(924, 34);
            this.panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(658, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(133, 34);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnOk.Location = new System.Drawing.Point(791, 0);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(133, 34);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(152, 32);
            this.tbName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbName.MaxLength = 256;
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(345, 22);
            this.tbName.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Prefix";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 100);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 132);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Email";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 164);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Phone number";
            // 
            // tbPrefix
            // 
            this.tbPrefix.Location = new System.Drawing.Point(152, 64);
            this.tbPrefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPrefix.MaxLength = 4;
            this.tbPrefix.Name = "tbPrefix";
            this.tbPrefix.Size = new System.Drawing.Size(345, 22);
            this.tbPrefix.TabIndex = 1;
            // 
            // tbAddress
            // 
            this.tbAddress.Location = new System.Drawing.Point(152, 96);
            this.tbAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbAddress.MaxLength = 256;
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(345, 22);
            this.tbAddress.TabIndex = 2;
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(152, 128);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbEmail.MaxLength = 80;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(345, 22);
            this.tbEmail.TabIndex = 3;
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(152, 160);
            this.tbPhone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPhone.MaxLength = 80;
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(345, 22);
            this.tbPhone.TabIndex = 4;
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(152, 226);
            this.tbComment.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbComment.MaxLength = 1000;
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbComment.Size = new System.Drawing.Size(345, 180);
            this.tbComment.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(33, 230);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Comment";
            // 
            // cboxInstanceStatus
            // 
            this.cboxInstanceStatus.DisplayMember = "Name";
            this.cboxInstanceStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxInstanceStatus.FormattingEnabled = true;
            this.cboxInstanceStatus.Location = new System.Drawing.Point(152, 193);
            this.cboxInstanceStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cboxInstanceStatus.Name = "cboxInstanceStatus";
            this.cboxInstanceStatus.Size = new System.Drawing.Size(345, 24);
            this.cboxInstanceStatus.TabIndex = 5;
            this.cboxInstanceStatus.ValueMember = "Id";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 197);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 16);
            this.label7.TabIndex = 15;
            this.label7.Text = "Status";
            // 
            // picLaboratoryLogo
            // 
            this.picLaboratoryLogo.BackColor = System.Drawing.SystemColors.Info;
            this.picLaboratoryLogo.Location = new System.Drawing.Point(4, 4);
            this.picLaboratoryLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picLaboratoryLogo.Name = "picLaboratoryLogo";
            this.picLaboratoryLogo.Size = new System.Drawing.Size(200, 98);
            this.picLaboratoryLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picLaboratoryLogo.TabIndex = 16;
            this.picLaboratoryLogo.TabStop = false;
            this.picLaboratoryLogo.DoubleClick += new System.EventHandler(this.picLaboratoryLogo_DoubleClick);
            // 
            // picAccreditedLogo
            // 
            this.picAccreditedLogo.BackColor = System.Drawing.SystemColors.Info;
            this.picAccreditedLogo.Location = new System.Drawing.Point(4, 4);
            this.picAccreditedLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picAccreditedLogo.Name = "picAccreditedLogo";
            this.picAccreditedLogo.Size = new System.Drawing.Size(200, 99);
            this.picAccreditedLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picAccreditedLogo.TabIndex = 17;
            this.picAccreditedLogo.TabStop = false;
            this.picAccreditedLogo.DoubleClick += new System.EventHandler(this.picAccreditedLogo_DoubleClick);
            // 
            // panelLaboratoryLogo
            // 
            this.panelLaboratoryLogo.AutoScroll = true;
            this.panelLaboratoryLogo.Controls.Add(this.picLaboratoryLogo);
            this.panelLaboratoryLogo.Location = new System.Drawing.Point(540, 48);
            this.panelLaboratoryLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelLaboratoryLogo.Name = "panelLaboratoryLogo";
            this.panelLaboratoryLogo.Size = new System.Drawing.Size(341, 158);
            this.panelLaboratoryLogo.TabIndex = 22;
            // 
            // panelAccreditedLogo
            // 
            this.panelAccreditedLogo.AutoScroll = true;
            this.panelAccreditedLogo.Controls.Add(this.picAccreditedLogo);
            this.panelAccreditedLogo.Location = new System.Drawing.Point(540, 247);
            this.panelAccreditedLogo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelAccreditedLogo.Name = "panelAccreditedLogo";
            this.panelAccreditedLogo.Size = new System.Drawing.Size(341, 158);
            this.panelAccreditedLogo.TabIndex = 23;
            // 
            // linkLaboratoryLogo
            // 
            this.linkLaboratoryLogo.AutoSize = true;
            this.linkLaboratoryLogo.Location = new System.Drawing.Point(540, 28);
            this.linkLaboratoryLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLaboratoryLogo.Name = "linkLaboratoryLogo";
            this.linkLaboratoryLogo.Size = new System.Drawing.Size(102, 16);
            this.linkLaboratoryLogo.TabIndex = 7;
            this.linkLaboratoryLogo.TabStop = true;
            this.linkLaboratoryLogo.Text = "Laboratory logo";
            this.linkLaboratoryLogo.Click += new System.EventHandler(this.picLaboratoryLogo_DoubleClick);
            // 
            // linkAccreditedLogo
            // 
            this.linkAccreditedLogo.AutoSize = true;
            this.linkAccreditedLogo.Location = new System.Drawing.Point(540, 226);
            this.linkAccreditedLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkAccreditedLogo.Name = "linkAccreditedLogo";
            this.linkAccreditedLogo.Size = new System.Drawing.Size(102, 16);
            this.linkAccreditedLogo.TabIndex = 8;
            this.linkAccreditedLogo.TabStop = true;
            this.linkAccreditedLogo.Text = "Accredited logo";
            this.linkAccreditedLogo.Click += new System.EventHandler(this.picAccreditedLogo_DoubleClick);
            // 
            // lblLaboratoryLogoSize
            // 
            this.lblLaboratoryLogoSize.AutoSize = true;
            this.lblLaboratoryLogoSize.Location = new System.Drawing.Point(704, 28);
            this.lblLaboratoryLogoSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLaboratoryLogoSize.Name = "lblLaboratoryLogoSize";
            this.lblLaboratoryLogoSize.Size = new System.Drawing.Size(157, 16);
            this.lblLaboratoryLogoSize.TabIndex = 26;
            this.lblLaboratoryLogoSize.Text = "<lblLaboratoryLogoSize>";
            // 
            // lblAccreditedLogoSize
            // 
            this.lblAccreditedLogoSize.AutoSize = true;
            this.lblAccreditedLogoSize.Location = new System.Drawing.Point(704, 226);
            this.lblAccreditedLogoSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccreditedLogoSize.Name = "lblAccreditedLogoSize";
            this.lblAccreditedLogoSize.Size = new System.Drawing.Size(157, 16);
            this.lblAccreditedLogoSize.TabIndex = 27;
            this.lblAccreditedLogoSize.Text = "<lblAccreditedLogoSize>";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 417);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(111, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "Report ISO Desc.";
            // 
            // tbISODesc
            // 
            this.tbISODesc.Location = new System.Drawing.Point(152, 414);
            this.tbISODesc.MaxLength = 1024;
            this.tbISODesc.Name = "tbISODesc";
            this.tbISODesc.Size = new System.Drawing.Size(729, 22);
            this.tbISODesc.TabIndex = 29;
            // 
            // FormLaboratory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 489);
            this.Controls.Add(this.tbISODesc);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblAccreditedLogoSize);
            this.Controls.Add(this.lblLaboratoryLogoSize);
            this.Controls.Add(this.linkAccreditedLogo);
            this.Controls.Add(this.linkLaboratoryLogo);
            this.Controls.Add(this.panelAccreditedLogo);
            this.Controls.Add(this.panelLaboratoryLogo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboxInstanceStatus);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.tbPrefix);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormLaboratory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DSA-Lims - Laboratory";
            this.Load += new System.EventHandler(this.FormLaboratory_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLaboratoryLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAccreditedLogo)).EndInit();
            this.panelLaboratoryLogo.ResumeLayout(false);
            this.panelLaboratoryLogo.PerformLayout();
            this.panelAccreditedLogo.ResumeLayout(false);
            this.panelAccreditedLogo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPrefix;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboxInstanceStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox picLaboratoryLogo;
        private System.Windows.Forms.PictureBox picAccreditedLogo;
        private System.Windows.Forms.FlowLayoutPanel panelLaboratoryLogo;
        private System.Windows.Forms.FlowLayoutPanel panelAccreditedLogo;
        private System.Windows.Forms.LinkLabel linkLaboratoryLogo;
        private System.Windows.Forms.LinkLabel linkAccreditedLogo;
        private System.Windows.Forms.Label lblLaboratoryLogoSize;
        private System.Windows.Forms.Label lblAccreditedLogoSize;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbISODesc;
    }
}