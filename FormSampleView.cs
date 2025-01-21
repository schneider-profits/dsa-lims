using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormSampleView : Form
    {
        private Guid mSampleId;        

        public FormSampleView(Guid sampleId)
        {
            InitializeComponent();
            mSampleId = sampleId;
        }

        private void FormSampleView_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                Sample s = new Sample();
                s.LoadFromDB(conn, null, mSampleId);
                string sampleType = s.GetSampleTypeName(conn, null);
                string sampleComponent = s.GetSampleComponentName(conn, null);
                string projectName = s.GetProjectName(conn, null);
                string sampler = s.GetSamplerName(conn, null);
                string samplingMethod = s.GetSamplingMethodName(conn, null);
                string station = s.GetStationName(conn, null);
                string countyMunicipality = s.GetCountyMunicipalityName(conn, null);
                string labName = s.GetLaboratoryName(conn, null);

                DataGridViewRow row1 = new DataGridViewRow();
                row1.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sample number" });
                row1.Cells.Add(new DataGridViewTextBoxCell() { Value = s.Number.ToString() });
                tblInfo.Rows.Add(row1);

                DataGridViewRow row2 = new DataGridViewRow();
                row2.Cells.Add(new DataGridViewTextBoxCell() { Value = "Laboratory" });
                row2.Cells.Add(new DataGridViewTextBoxCell() { Value = labName });
                tblInfo.Rows.Add(row2);

                DataGridViewRow row3 = new DataGridViewRow();
                row3.Cells.Add(new DataGridViewTextBoxCell() { Value = "Type" });
                row3.Cells.Add(new DataGridViewTextBoxCell() { Value = sampleType });
                tblInfo.Rows.Add(row3);

                DataGridViewRow row4 = new DataGridViewRow();
                row4.Cells.Add(new DataGridViewTextBoxCell() { Value = "Component" });
                row4.Cells.Add(new DataGridViewTextBoxCell() { Value = sampleComponent });
                tblInfo.Rows.Add(row4);

                DataGridViewRow row5 = new DataGridViewRow();
                row5.Cells.Add(new DataGridViewTextBoxCell() { Value = "External Id" });
                row5.Cells.Add(new DataGridViewTextBoxCell() { Value = s.ExternalId });
                tblInfo.Rows.Add(row5);

                DataGridViewRow row6 = new DataGridViewRow();
                row6.Cells.Add(new DataGridViewTextBoxCell() { Value = "Project" });
                row6.Cells.Add(new DataGridViewTextBoxCell() { Value = projectName });
                tblInfo.Rows.Add(row6);

                DataGridViewRow row7 = new DataGridViewRow();
                row7.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sampler" });
                row7.Cells.Add(new DataGridViewTextBoxCell() { Value = sampler });
                tblInfo.Rows.Add(row7);

                DataGridViewRow row8 = new DataGridViewRow();
                row8.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sampling method" });
                row8.Cells.Add(new DataGridViewTextBoxCell() { Value = samplingMethod });
                tblInfo.Rows.Add(row8);

                DataGridViewRow row9 = new DataGridViewRow();
                row9.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sampling time from" });
                row9.Cells.Add(new DataGridViewTextBoxCell() { Value = (s.SamplingDateFrom == null ? "" : s.SamplingDateFrom.Value.ToString(Utils.DateTimeFormatNorwegian)) });
                tblInfo.Rows.Add(row9);

                DataGridViewRow row10 = new DataGridViewRow();
                row10.Cells.Add(new DataGridViewTextBoxCell() { Value = "Sampling time to" });
                row10.Cells.Add(new DataGridViewTextBoxCell() { Value = (s.SamplingDateTo == null ? "" : s.SamplingDateTo.Value.ToString(Utils.DateTimeFormatNorwegian)) });
                tblInfo.Rows.Add(row10);

                DataGridViewRow row11 = new DataGridViewRow();
                row11.Cells.Add(new DataGridViewTextBoxCell() { Value = "Reference time" });
                row11.Cells.Add(new DataGridViewTextBoxCell() { Value = (s.ReferenceDate == null ? "" : s.ReferenceDate.Value.ToString(Utils.DateTimeFormatNorwegian)) });
                tblInfo.Rows.Add(row11);

                DataGridViewRow row12 = new DataGridViewRow();
                row12.Cells.Add(new DataGridViewTextBoxCell() { Value = "Station" });
                row12.Cells.Add(new DataGridViewTextBoxCell() { Value = station });
                tblInfo.Rows.Add(row12);

                DataGridViewRow row13 = new DataGridViewRow();
                row13.Cells.Add(new DataGridViewTextBoxCell() { Value = "County - Municipality" });
                row13.Cells.Add(new DataGridViewTextBoxCell() { Value = countyMunicipality });
                tblInfo.Rows.Add(row13);

                DataGridViewRow row14 = new DataGridViewRow();
                row14.Cells.Add(new DataGridViewTextBoxCell() { Value = "Location" });
                string locinfo;
                if (String.IsNullOrEmpty(s.LocationType))
                    locinfo = s.Location;
                else locinfo = "[" + s.LocationType + "] " + s.Location;
                row14.Cells.Add(new DataGridViewTextBoxCell() { Value = locinfo });
                tblInfo.Rows.Add(row14);

                DataGridViewRow row15 = new DataGridViewRow();
                row15.Cells.Add(new DataGridViewTextBoxCell() { Value = "Latitude" });
                row15.Cells.Add(new DataGridViewTextBoxCell() { Value = s.Latitude.ToString() });
                tblInfo.Rows.Add(row15);

                DataGridViewRow row16 = new DataGridViewRow();
                row16.Cells.Add(new DataGridViewTextBoxCell() { Value = "Longitude" });
                row16.Cells.Add(new DataGridViewTextBoxCell() { Value = s.Longitude.ToString() });
                tblInfo.Rows.Add(row16);

                DataGridViewRow row17 = new DataGridViewRow();
                row17.Cells.Add(new DataGridViewTextBoxCell() { Value = "Altitude" });
                row17.Cells.Add(new DataGridViewTextBoxCell() { Value = s.Altitude.ToString() });
                tblInfo.Rows.Add(row17);

                DataGridViewRow row18 = new DataGridViewRow();
                row18.Cells.Add(new DataGridViewTextBoxCell() { Value = "Exempt from public" });
                row18.Cells.Add(new DataGridViewTextBoxCell() { Value = (s.Confidential ? "Yes" : "No") });
                tblInfo.Rows.Add(row18);            

                tblInfo.Columns[0].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                tblInfo.Columns[1].DefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Regular);

                tbComment.Text = s.Comment;

                UI.PopulateAttachments(conn, null, "sample", mSampleId, dgSampleViewAttachments);
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dgSampleViewAttachments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.ShowAttachment(conn, e.RowIndex, grid);
            }
        }
    }
}
