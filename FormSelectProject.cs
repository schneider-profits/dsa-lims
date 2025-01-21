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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormSelectProject : Form
    {
        public Guid SelectedSubProjectId { get; set; }

        public FormSelectProject()
        {
            InitializeComponent();
        }

        private void FormSelectProject_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                UI.PopulateComboBoxes(conn, "csp_select_projects_main_short", new[] {
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxMainProject);
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
                Close();
            }
            finally
            {
                conn?.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedSubProjectId = Guid.Empty;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(!Utils.IsValidGuid(cboxSubProject.SelectedValue))
            {
                MessageBox.Show("You must select a valid sub-project");
                return;
            }

            SelectedSubProjectId = Utils.MakeGuid(cboxSubProject.SelectedValue);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cboxMainProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!Utils.IsValidGuid(cboxMainProject.SelectedValue))
            {
                cboxSubProject.DataSource = null;
                return;
            }

            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                Guid pmid = Utils.MakeGuid(cboxMainProject.SelectedValue);

                UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                    new SqlParameter("@project_main_id", pmid),
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxSubProject);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
                Close();
            }
            finally
            {
                conn?.Close();
            }
        }
    }
}
