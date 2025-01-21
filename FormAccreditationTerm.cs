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
    public partial class FormAccreditationTerm : Form
    {
        private Dictionary<string, object> p = new Dictionary<string, object>();

        public string ModifiedAccreditationTerm
        {
            get { return p["name"].ToString(); }
            private set { }
        }

        public FormAccreditationTerm()
        {
            InitializeComponent();
            Text = "DSA-Lims - New accreditation term";
        }

        public FormAccreditationTerm(Guid accTermId)
        {
            InitializeComponent();
            Text = "DSA-Lims - Edit accreditation term";
            p["id"] = accTermId;
        }

        private void FormAccreditationTerm_Load(object sender, EventArgs e)
        {
            tbMinFillHeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMaxFillHeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMinWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMaxWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMinVolume.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMaxVolume.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMinDensity.KeyPress += CustomEvents.Numeric_KeyPress;
            tbMaxDensity.KeyPress += CustomEvents.Numeric_KeyPress;

            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();

                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status", false);

                if (p.ContainsKey("id"))
                {
                    using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_accreditation_term", CommandType.StoredProcedure,
                        new SqlParameter("@id", p["id"])))
                    {
                        reader.Read();
                        tbName.Text = reader["name"].ToString();
                        tbMinFillHeight.Text = reader["fill_height_min"].ToString();
                        tbMaxFillHeight.Text = reader["fill_height_max"].ToString();
                        tbMinWeight.Text = reader["weight_min"].ToString();
                        tbMaxWeight.Text = reader["weight_max"].ToString();
                        tbMinVolume.Text = reader["volume_min"].ToString();
                        tbMaxVolume.Text = reader["volume_max"].ToString();
                        tbMinDensity.Text = reader["density_min"].ToString();
                        tbMaxDensity.Text = reader["density_max"].ToString();
                        cboxInstanceStatus.SelectedValue = reader["instance_status_id"];
                    }
                }
                else
                {                 
                    cboxInstanceStatus.SelectedValue = InstanceStatus.Active;
                }
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
            if (String.IsNullOrEmpty(tbName.Text.Trim()))
            {
                MessageBox.Show("Name is mandatory");
                return;
            }

            p["name"] = tbName.Text.Trim();
            double? min, max;
            min = Utils.ToDouble(tbMinFillHeight.Text);
            max = Utils.ToDouble(tbMaxFillHeight.Text);
            if(min != null && max != null && max < min)
            {
                MessageBox.Show("Fill height max. can not be smaller than fill height min.");
                return;
            }
            p["fill_height_min"] = min;
            p["fill_height_max"] = max;

            min = Utils.ToDouble(tbMinWeight.Text);
            max = Utils.ToDouble(tbMaxWeight.Text);
            if (min != null && max != null && max < min)
            {
                MessageBox.Show("Weight max. can not be smaller than weight min.");
                return;
            }
            p["weight_min"] = min;
            p["weight_max"] = max;

            min = Utils.ToDouble(tbMinVolume.Text);
            max = Utils.ToDouble(tbMaxVolume.Text);
            if (min != null && max != null && max < min)
            {
                MessageBox.Show("Volume max. can not be smaller than volume min.");
                return;
            }
            p["volume_min"] = Utils.ToDouble(tbMinVolume.Text);
            p["volume_max"] = Utils.ToDouble(tbMaxVolume.Text);

            min = Utils.ToDouble(tbMinDensity.Text);
            max = Utils.ToDouble(tbMaxDensity.Text);
            if (min != null && max != null && max < min)
            {
                MessageBox.Show("Density max. can not be smaller than density min.");
                return;
            }
            p["density_min"] = Utils.ToDouble(tbMinDensity.Text);
            p["density_max"] = Utils.ToDouble(tbMaxDensity.Text);

            p["instance_status_id"] = cboxInstanceStatus.SelectedValue;

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            bool success = true;

            try
            {
                connection = DB.OpenConnection();
                transaction = connection.BeginTransaction();

                if (!p.ContainsKey("id"))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from accreditation_term where name = @name", connection, transaction);
                    cmd.Parameters.AddWithValue("@name", p["name"]);

                    int cnt = (int)cmd.ExecuteScalar();
                    if (cnt > 0)
                    {
                        MessageBox.Show("Accreditation term " + p["name"] + " already exists");
                        return;
                    }

                    InsertAccretitationTerm(connection, transaction);
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from accreditation_term where name = @name and id <> @id", connection, transaction);                    
                    cmd.Parameters.AddWithValue("@name", p["name"]);
                    cmd.Parameters.AddWithValue("@id", p["id"]);

                    int cnt = (int)cmd.ExecuteScalar();
                    if (cnt > 0)
                    {
                        MessageBox.Show("Accreditation term " + p["name"] + " already exists");
                        return;
                    }

                    UpdateAccretitationTerm(connection, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                success = false;
                transaction?.Rollback();
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }

            DialogResult = success ? DialogResult.OK : DialogResult.Abort;
            Close();
        }

        private void InsertAccretitationTerm(SqlConnection conn, SqlTransaction trans)
        {
            p["create_date"] = DateTime.Now;
            p["create_id"] = Common.UserId;
            p["update_date"] = DateTime.Now;
            p["update_id"] = Common.UserId;

            SqlCommand cmd = new SqlCommand("csp_insert_accreditation_term", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            p["id"] = Guid.NewGuid();
            cmd.Parameters.AddWithValue("@id", p["id"]);
            cmd.Parameters.AddWithValue("@name", p["name"], String.Empty);
            cmd.Parameters.AddWithValue("@fill_height_min", p["fill_height_min"], null);
            cmd.Parameters.AddWithValue("@fill_height_max", p["fill_height_max"], null);
            cmd.Parameters.AddWithValue("@weight_min", p["weight_min"], null);
            cmd.Parameters.AddWithValue("@weight_max", p["weight_max"], null);
            cmd.Parameters.AddWithValue("@volume_min", p["volume_min"], null);
            cmd.Parameters.AddWithValue("@volume_max", p["volume_max"], null);
            cmd.Parameters.AddWithValue("@density_min", p["density_min"], null);
            cmd.Parameters.AddWithValue("@density_max", p["density_max"], null);
            cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
            cmd.Parameters.AddWithValue("@create_date", p["create_date"]);
            cmd.Parameters.AddWithValue("@create_id", p["create_id"]);
            cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
            cmd.Parameters.AddWithValue("@update_id", p["update_id"]);
            cmd.ExecuteNonQuery();
        }

        private void UpdateAccretitationTerm(SqlConnection conn, SqlTransaction trans)
        {
            p["update_date"] = DateTime.Now;
            p["update_id"] = Common.UserId;

            SqlCommand cmd = new SqlCommand("csp_update_accreditation_term", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", p["id"]);
            cmd.Parameters.AddWithValue("@name", p["name"], String.Empty);
            cmd.Parameters.AddWithValue("@fill_height_min", p["fill_height_min"], null);
            cmd.Parameters.AddWithValue("@fill_height_max", p["fill_height_max"], null);
            cmd.Parameters.AddWithValue("@weight_min", p["weight_min"], null);
            cmd.Parameters.AddWithValue("@weight_max", p["weight_max"], null);
            cmd.Parameters.AddWithValue("@volume_min", p["volume_min"], null);
            cmd.Parameters.AddWithValue("@volume_max", p["volume_max"], null);
            cmd.Parameters.AddWithValue("@density_min", p["density_min"], null);
            cmd.Parameters.AddWithValue("@density_max", p["density_max"], null);
            cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
            cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
            cmd.Parameters.AddWithValue("@update_id", p["update_id"]);
            cmd.ExecuteNonQuery();
        }
    }
}
