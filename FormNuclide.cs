﻿/*	
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
using Newtonsoft.Json;

namespace DSA_lims
{
    public partial class FormNuclide : Form
    {
        private Dictionary<string, object> p = new Dictionary<string, object>();

        public Guid NuclideId
        {
            get { return p.ContainsKey("id") ? (Guid)p["id"] : Guid.Empty; }
        }

        public string NuclideName
        {
            get { return p.ContainsKey("name") ? p["name"].ToString() : String.Empty; }
        }        

        public FormNuclide()
        {
            InitializeComponent();

            // Create new nuclide                                    
            Text = "New nuclide";            
            using (SqlConnection conn = DB.OpenConnection())
            {
                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status");
            }
            cboxInstanceStatus.SelectedValue = InstanceStatus.Active;
        }

        public FormNuclide(Guid nid)
        {
            InitializeComponent();

            // Edit existing nuclide
            p["id"] = nid;
            Text = "Edit nuclide";

            using (SqlConnection conn = DB.OpenConnection())
            {
                cboxInstanceStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status");

                SqlCommand cmd = new SqlCommand("csp_select_nuclide", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", nid);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if(!reader.HasRows)                    
                        throw new Exception("Nuclide with ID " + p["id"] + " was not found");                    

                    reader.Read();                    
                    tbName.Text = reader["name"].ToString();
                    tbProtons.Text = reader["protons"].ToString();
                    tbNeutrons.Text = reader["neutrons"].ToString();
                    cbMetaStable.Checked = Convert.ToBoolean(reader["meta_stable"]);
                    tbHalflife.Text = reader["half_life_year"].ToString();
                    cboxInstanceStatus.SelectedValue = reader["instance_status_id"];
                    tbComment.Text = reader["comment"].ToString();
                    p["create_date"] = reader["create_date"];
                    p["created_by"] = reader["created_by"];
                    p["update_date"] = reader["update_date"];
                    p["updated_by"] = reader["updated_by"];
                }
            }            
        }

        private void FormNuclide_Load(object sender, EventArgs e)
        {
            tbProtons.KeyPress += CustomEvents.Integer_KeyPress;
            tbNeutrons.KeyPress += CustomEvents.Integer_KeyPress;
            tbHalflife.KeyPress += CustomEvents.Numeric_KeyPress;
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
                MessageBox.Show("Nuclide name is mandatory");
                return;
            }

            if (String.IsNullOrEmpty(tbHalflife.Text))
            {
                MessageBox.Show("Halflife is mandatory");
                return;
            }

            int protons = Convert.ToInt32(tbProtons.Text);
            int neutrons = Convert.ToInt32(tbNeutrons.Text);
            int meta = cbMetaStable.Checked ? 1 : 0;
            int zas = (protons * 10000000) + ((neutrons + protons) * 10000) + meta;

            p["zas"] = zas;
            p["name"] = tbName.Text.Trim().ToUpper();
            p["protons"] = protons;
            p["neutrons"] = neutrons;
            p["meta_stable"] = meta;
            p["halflife"] = Convert.ToDouble(tbHalflife.Text);
            p["instance_status_id"] = cboxInstanceStatus.SelectedValue;
            p["comment"] = tbComment.Text.Trim();

            SqlConnection connection = null;
            SqlTransaction transaction = null;
            bool success = true;

            try
            {
                connection = DB.OpenConnection();
                transaction = connection.BeginTransaction();

                if (DB.NameExists(connection, transaction, "nuclide", p["name"].ToString(), NuclideId))
                {
                    MessageBox.Show("The nuclide '" + p["name"] + "' already exists");
                    return;
                }

                if (!p.ContainsKey("id"))
                    InsertNuclide(connection, transaction);
                else
                    UpdateNuclide(connection, transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                success = false;
                transaction?.Rollback();
                Common.Log.Error(ex);                
            }
            finally
            {
                connection?.Close();
            }

            DialogResult = success ? DialogResult.OK : DialogResult.Abort;
            Close();            
        }

        private void InsertNuclide(SqlConnection conn, SqlTransaction trans)
        {            
            p["create_date"] = DateTime.Now;
            p["created_by"] = Common.Username;
            p["update_date"] = DateTime.Now;
            p["updated_by"] = Common.Username;        

            SqlCommand cmd = new SqlCommand("csp_insert_nuclide", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            p["id"] = Guid.NewGuid();
            cmd.Parameters.AddWithValue("@id", p["id"]);
            cmd.Parameters.AddWithValue("@zas", p["zas"]);
            cmd.Parameters.AddWithValue("@name", p["name"]);
            cmd.Parameters.AddWithValue("@protons", p["protons"]);
            cmd.Parameters.AddWithValue("@neutrons", p["neutrons"]);
            cmd.Parameters.AddWithValue("@meta_stable", p["meta_stable"]);
            cmd.Parameters.AddWithValue("@half_life_year", p["halflife"]);                
            cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
            cmd.Parameters.AddWithValue("@comment", p["comment"], String.Empty);
            cmd.Parameters.AddWithValue("@create_date", p["create_date"]);
            cmd.Parameters.AddWithValue("@created_by", p["created_by"]);
            cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
            cmd.Parameters.AddWithValue("@updated_by", p["updated_by"]);
            cmd.ExecuteNonQuery();                        
        }

        private void UpdateNuclide(SqlConnection conn, SqlTransaction trans)
        {            
            p["update_date"] = DateTime.Now;
            p["updated_by"] = Common.Username;        

            SqlCommand cmd = new SqlCommand("csp_update_nuclide", conn, trans);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", p["id"]);
            cmd.Parameters.AddWithValue("@zas", p["zas"]);
            cmd.Parameters.AddWithValue("@name", p["name"]);
            cmd.Parameters.AddWithValue("@protons", p["protons"]);
            cmd.Parameters.AddWithValue("@neutrons", p["neutrons"]);
            cmd.Parameters.AddWithValue("@meta_stable", p["meta_stable"]);
            cmd.Parameters.AddWithValue("@half_life_year", p["halflife"]);
            cmd.Parameters.AddWithValue("@instance_status_id", p["instance_status_id"]);
            cmd.Parameters.AddWithValue("@comment", p["comment"], String.Empty);
            cmd.Parameters.AddWithValue("@update_date", p["update_date"]);
            cmd.Parameters.AddWithValue("@updated_by", p["updated_by"]);
            cmd.ExecuteNonQuery();                        
        }        
    }
}
