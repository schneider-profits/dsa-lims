using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTextSharp.text.TabStop;

namespace DSA_lims
{
    public partial class FormOrderCopy : Form
    {        
        private Assignment mAssignment = null;
        private Customer mCustomer = null;

        public FormOrderCopy(Assignment a)
        {
            InitializeComponent();
            mAssignment = a;            
        }

        private void FormOrderCopy_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Copy order " + mAssignment.Name;
            tbDescription.Text = mAssignment.Description;
            tbDeadline.Tag = mAssignment.Deadline.Value;
            tbDeadline.Text = mAssignment.Deadline.Value.ToString(Utils.DateFormatNorwegian);
            tbCustomer.Text = mAssignment.CustomerContactName;
            tbContentComment.Text = mAssignment.ContentComment;

            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                UI.PopulateComboBoxes(conn, "csp_select_accounts_for_laboratory_short", new[] {
                    new SqlParameter("@laboratory_id", mAssignment.LaboratoryId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxResponsible);

                cboxResponsible.SelectedValue = mAssignment.AccountId;

                tbLaboratory.Text = mAssignment.LaboratoryName(conn, null);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn?.Close();
            }            
        }

        private void pbDeadline_Click(object sender, EventArgs e)
        {
            FormSelectDate form = new FormSelectDate();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            DateTime selectedDate = form.SelectedDate;

            if (selectedDate.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Deadline can not be in the past");
                return;
            }

            tbDeadline.Tag = selectedDate;
            tbDeadline.Text = selectedDate.ToString(Utils.DateFormatNorwegian);
        }

        private void pbCustomer_Click(object sender, EventArgs e)
        {
            FormSelectCustomer form = new FormSelectCustomer(InstanceStatus.Active);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            mCustomer = form.SelectedCustomer;
            tbCustomer.Text = mCustomer.ContactName;            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxResponsible.SelectedValue))
            {
                MessageBox.Show("Responsible is mandatory");
                return;
            }

            if (tbDeadline.Tag == null)
            {
                MessageBox.Show("Deadline is mandatory");
                return;
            }

            DateTime dl = (DateTime)tbDeadline.Tag;
            if (dl.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Deadline can not be in the past");
                return;
            }

            mAssignment.Deadline = (DateTime)tbDeadline.Tag;
            mAssignment.AccountId = Utils.MakeGuid(cboxResponsible.SelectedValue);

            if (mCustomer != null)
            {
                mAssignment.CustomerCompanyAddress = mCustomer.CompanyAddress;
                mAssignment.CustomerCompanyEmail = mCustomer.CompanyEmail;
                mAssignment.CustomerCompanyName = mCustomer.CompanyName;
                mAssignment.CustomerCompanyPhone = mCustomer.CompanyPhone;
                mAssignment.CustomerContactAddress = mCustomer.ContactAddress;
                mAssignment.CustomerContactEmail = mCustomer.ContactEmail;
                mAssignment.CustomerContactName = mCustomer.ContactName;
                mAssignment.CustomerContactPhone = mCustomer.ContactPhone;
            }

            mAssignment.Description = tbDescription.Text.Trim();
            mAssignment.ContentComment = tbContentComment.Text.Trim();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
