namespace DSA_lims
{
    partial class FormOrderCopy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOrderCopy));
            this.lblTitle = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboxResponsible = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbDeadline = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pbDeadline = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbCustomer = new System.Windows.Forms.TextBox();
            this.pbCustomer = new System.Windows.Forms.PictureBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbContentComment = new System.Windows.Forms.TextBox();
            this.tbLaboratory = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeadline)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCustomer)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(30, 22);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(88, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Copy order...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(144, 58);
            this.tbDescription.MaxLength = 80;
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(287, 21);
            this.tbDescription.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Responsible";
            // 
            // cboxResponsible
            // 
            this.cboxResponsible.DisplayMember = "Name";
            this.cboxResponsible.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxResponsible.FormattingEnabled = true;
            this.cboxResponsible.Location = new System.Drawing.Point(144, 117);
            this.cboxResponsible.Name = "cboxResponsible";
            this.cboxResponsible.Size = new System.Drawing.Size(287, 23);
            this.cboxResponsible.TabIndex = 4;
            this.cboxResponsible.ValueMember = "Id";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Deadline";
            // 
            // tbDeadline
            // 
            this.tbDeadline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDeadline.Location = new System.Drawing.Point(0, 0);
            this.tbDeadline.Name = "tbDeadline";
            this.tbDeadline.ReadOnly = true;
            this.tbDeadline.Size = new System.Drawing.Size(261, 21);
            this.tbDeadline.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbDeadline);
            this.panel1.Controls.Add(this.pbDeadline);
            this.panel1.Location = new System.Drawing.Point(144, 149);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(287, 24);
            this.panel1.TabIndex = 7;
            // 
            // pbDeadline
            // 
            this.pbDeadline.BackgroundImage = global::DSA_lims.Properties.Resources.datetime_16;
            this.pbDeadline.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbDeadline.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbDeadline.Location = new System.Drawing.Point(261, 0);
            this.pbDeadline.Name = "pbDeadline";
            this.pbDeadline.Size = new System.Drawing.Size(26, 24);
            this.pbDeadline.TabIndex = 7;
            this.pbDeadline.TabStop = false;
            this.pbDeadline.Click += new System.EventHandler(this.pbDeadline_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "Customer";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbCustomer);
            this.panel2.Controls.Add(this.pbCustomer);
            this.panel2.Location = new System.Drawing.Point(144, 179);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(287, 24);
            this.panel2.TabIndex = 9;
            // 
            // tbCustomer
            // 
            this.tbCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbCustomer.Location = new System.Drawing.Point(0, 0);
            this.tbCustomer.Name = "tbCustomer";
            this.tbCustomer.ReadOnly = true;
            this.tbCustomer.Size = new System.Drawing.Size(261, 21);
            this.tbCustomer.TabIndex = 1;
            // 
            // pbCustomer
            // 
            this.pbCustomer.BackgroundImage = global::DSA_lims.Properties.Resources.user_16;
            this.pbCustomer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbCustomer.Dock = System.Windows.Forms.DockStyle.Right;
            this.pbCustomer.Location = new System.Drawing.Point(261, 0);
            this.pbCustomer.Name = "pbCustomer";
            this.pbCustomer.Size = new System.Drawing.Size(26, 24);
            this.pbCustomer.TabIndex = 0;
            this.pbCustomer.TabStop = false;
            this.pbCustomer.Click += new System.EventHandler(this.pbCustomer_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCancel);
            this.panel3.Controls.Add(this.btnCreate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 360);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(469, 28);
            this.panel3.TabIndex = 10;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCancel.Location = new System.Drawing.Point(269, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCreate.Location = new System.Drawing.Point(369, 0);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(100, 28);
            this.btnCreate.TabIndex = 6;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(30, 212);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "Content comment";
            // 
            // tbContentComment
            // 
            this.tbContentComment.Location = new System.Drawing.Point(144, 209);
            this.tbContentComment.MaxLength = 4000;
            this.tbContentComment.Multiline = true;
            this.tbContentComment.Name = "tbContentComment";
            this.tbContentComment.Size = new System.Drawing.Size(287, 112);
            this.tbContentComment.TabIndex = 12;
            // 
            // tbLaboratory
            // 
            this.tbLaboratory.Location = new System.Drawing.Point(144, 86);
            this.tbLaboratory.Name = "tbLaboratory";
            this.tbLaboratory.ReadOnly = true;
            this.tbLaboratory.Size = new System.Drawing.Size(287, 21);
            this.tbLaboratory.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 89);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 15);
            this.label7.TabIndex = 14;
            this.label7.Text = "Laboratory";
            // 
            // FormOrderCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 388);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbLaboratory);
            this.Controls.Add(this.tbContentComment);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboxResponsible);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOrderCopy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DSA Lims - Copy order";
            this.Load += new System.EventHandler(this.FormOrderCopy_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbDeadline)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCustomer)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboxResponsible;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbDeadline;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pbDeadline;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pbCustomer;
        private System.Windows.Forms.TextBox tbCustomer;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbContentComment;
        private System.Windows.Forms.TextBox tbLaboratory;
        private System.Windows.Forms.Label label7;
    }
}