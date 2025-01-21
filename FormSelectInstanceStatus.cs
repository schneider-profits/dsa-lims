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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormSelectInstanceStatus : Form
    {        
        public int SelectedInstanceStatus { get; private set; }

        public FormSelectInstanceStatus()
        {
            InitializeComponent();    
        }

        private void FormSelectInstanceStatus_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "Select an instance status";

            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status", true);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if((int)cboxInstanceStatus.SelectedValue == 0)
            {
                MessageBox.Show("You must select a valid instance status");
                return;
            }

            SelectedInstanceStatus = (int)cboxInstanceStatus.SelectedValue;

            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
