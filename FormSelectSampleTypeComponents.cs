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
    public partial class FormSelectSampleTypeComponents : Form
    {
        private Guid mSampleTypeId = Guid.Empty;

        public List<Lemma<Guid, string>> SelectedComponents { get; set; } = new List<Lemma<Guid, string>>();

        public FormSelectSampleTypeComponents(Guid sampleTypeId)
        {
            InitializeComponent();

            mSampleTypeId = sampleTypeId;
        }

        private void FormSelectSampleTypeComponents_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                Guid sid = mSampleTypeId;                

                while (true)
                {
                    string query = "select id, name from sample_component where sample_type_id = @sample_type_id order by name";
                    using (SqlDataReader reader = DB.GetDataReader(conn, null, query, CommandType.Text, new SqlParameter("@sample_type_id", sid)))
                    {
                        while (reader.Read())
                        {
                            lbSampleComponents.Items.Add(new Lemma<Guid, string>(reader.GetGuid("id"), reader.GetString("name")));
                        }
                    }

                    sid = (Guid)DB.GetScalar(conn, null, "select parent_id from sample_type where id = @id", CommandType.Text, new SqlParameter("@id", sid));
                    if (sid == Guid.Empty)
                        break;
                }
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
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(lbSampleComponents.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select one or more sample components");
                return;
            }

            SelectedComponents.Clear();

            foreach (Lemma<Guid, string> l in lbSampleComponents.SelectedItems)            
                SelectedComponents.Add(l);

            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
