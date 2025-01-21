/*	
	DSA Lims - Laboratory Information Management System
    Copyright (C) 2018  Norwegian Radiation Protection Authority

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
// Authors: Dag Robole,

using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormReportAnalysisReport : Form
    {        
        byte[] mContent = null;
        bool mHasNewVersion;
        Assignment mAssignment = null;
        string mISOName;

        public byte[] ReportData { get { return mContent; } }

        public bool HasNewVersion { get { return mHasNewVersion; } }

        public FormReportAnalysisReport(Assignment assignment, string ISOName)
        {
            InitializeComponent();

            mAssignment = assignment;        
            mISOName = ISOName;
            mHasNewVersion = false;
        }

        private void FormReportViewer_Load(object sender, EventArgs e)
        {
            string strActSig, strMdaSig;
            if(mAssignment.RequestedSigmaAct == 1)
                strActSig = "ca. 68.3 % (tilsvarer k = 1)";
            else if (mAssignment.RequestedSigmaAct == 1.96)
                strActSig = "ca. 95 % (tilsvarer k = 1.96)";
            else if(mAssignment.RequestedSigmaAct == 2 || mAssignment.RequestedSigmaAct == 0)
                strActSig = "ca. 95.5 % (tilsvarer k = 2)";
            else strActSig = "ca. 99.9 % (tilsvarer k = 3)";

            if (mAssignment.RequestedSigmaMDA == 1)
                strMdaSig = "ca. 84.1 % (tilsvarer k = 1)";
            else if (mAssignment.RequestedSigmaMDA == 1.645 || mAssignment.RequestedSigmaMDA == 0)
                strMdaSig = "ca. 95 % (tilsvarer k = 1.645)";
            else if (mAssignment.RequestedSigmaMDA == 2)
                strMdaSig = "ca. 97.2 % (tilsvarer k = 2)";
            else strMdaSig = "ca. 99.95 % (tilsvarer k = 3)";

            ReportParameter param1 = new ReportParameter("actsig", strActSig);
            ReportParameter param2 = new ReportParameter("mdasig", strMdaSig);
            reportViewer.LocalReport.SetParameters(param1);
            reportViewer.LocalReport.SetParameters(param2);

            ReportParameter param3 = new ReportParameter("isoname", mISOName);
            reportViewer.LocalReport.SetParameters(param3);

            DataTable1TableAdapter.Fill(DSOrderReport.DataTable1, (float)mAssignment.RequestedSigmaAct, (float)mAssignment.RequestedSigmaMDA, mAssignment.Name);
            DataTable2TableAdapter.Fill(DSOrderReport.DataTable2, mAssignment.Name);

            reportViewer.LocalReport.DataSources.Clear();
            ReportDataSource rd1 = new ReportDataSource("DataSet1", DSOrderReport.Tables[0]);
            ReportDataSource rd2 = new ReportDataSource("DataSet2", DSOrderReport.Tables[1]);
            reportViewer.LocalReport.DataSources.Add(rd1);
            reportViewer.LocalReport.DataSources.Add(rd2);
            reportViewer.LocalReport.Refresh();
            reportViewer.RefreshReport();
        }        

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCreateVersion_Click(object sender, EventArgs e)
        {
            if (mAssignment.WorkflowStatusId != WorkflowStatus.Complete)
            {
                MessageBox.Show("Order must be saved as complete first");
                return;
            }
            
            int newVersion = mAssignment.AnalysisReportVersion + 1;

            if (mAssignment.AnalysisReportVersion > 0)
            {
                FormReportAuditComment form = new FormReportAuditComment();
                if (form.ShowDialog() != DialogResult.OK)
                    return;

                mAssignment.AuditComment += "v." + newVersion + ": " + form.SelectedComment + Environment.NewLine;
            }

            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();                
                mAssignment.AnalysisReportVersion = newVersion;
                mAssignment.Dirty = true;
                mAssignment.StoreToDB(conn, null);

                DataTable1TableAdapter.Fill(DSOrderReport.DataTable1, (float)mAssignment.RequestedSigmaAct, (float)mAssignment.RequestedSigmaMDA, mAssignment.Name);
                DataTable2TableAdapter.Fill(DSOrderReport.DataTable2, mAssignment.Name);

                reportViewer.LocalReport.DataSources.Clear();
                ReportDataSource rd1 = new ReportDataSource("DataSet1", DSOrderReport.Tables[0]);
                ReportDataSource rd2 = new ReportDataSource("DataSet2", DSOrderReport.Tables[1]);
                reportViewer.LocalReport.DataSources.Add(rd1);
                reportViewer.LocalReport.DataSources.Add(rd2);
                reportViewer.LocalReport.Refresh();
                reportViewer.RefreshReport();

                var deviceInfo = @"<DeviceInfo>
                    <EmbedFonts>None</EmbedFonts>
                   </DeviceInfo>";

                mContent = reportViewer.LocalReport.Render("PDF", deviceInfo);

                mHasNewVersion = true;
                btnCreateVersion.Enabled = false;
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
    }
}
