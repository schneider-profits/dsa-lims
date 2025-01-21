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
    public partial class FormSelectSamplingMethod : Form
    {
        private Guid mSamplingMethodId = Guid.Empty;
        private string mSamplingMethodName = String.Empty;

        public Guid SelectedSamplingMethodId { get { return mSamplingMethodId; } }
        public string SelectedSamplingMethodName { get { return mSamplingMethodName; } }

        public FormSelectSamplingMethod()
        {
            InitializeComponent();
        }

        private void FormSelectSamplingMethod_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                UI.PopulateSamplingMethods(conn, InstanceStatus.Active, gridSamplingMethods);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
                DialogResult = DialogResult.Abort;
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
            if (gridSamplingMethods.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a sampler first");
                return;
            }

            mSamplingMethodId = Utils.MakeGuid(gridSamplingMethods.SelectedRows[0].Cells["id"].Value);
            mSamplingMethodName = gridSamplingMethods.SelectedRows[0].Cells["name"].Value.ToString();

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
