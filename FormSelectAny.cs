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
    public partial class FormSelectAny : Form
    {
        private string mDBTable = String.Empty;

        public Lemma<Guid, string> SelectedItem { get; set; }

        public FormSelectAny(string title, string dbTable)
        {
            InitializeComponent();

            lblInfo.Text = title;
            mDBTable = dbTable;
        }

        private void FormSelectAny_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                grid.DataSource = DB.GetDataTable(conn, null, "select id, name from " + mDBTable + " where instance_status_id = 1 order by name", CommandType.Text);

                grid.Columns["id"].Visible = false;
                grid.Columns["name"].HeaderText = "Name";
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(Utils.makeErrorMessage(ex.Message));
            }
            finally
            {
                conn?.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedItem = null;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(grid.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select an item");
                return;
            }

            SelectedItem = new Lemma<Guid, string>(Utils.MakeGuid(grid.SelectedRows[0].Cells["id"].Value), grid.SelectedRows[0].Cells["name"].ToString());

            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
