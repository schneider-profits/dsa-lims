﻿using System;
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
    public partial class FormSampTypeXPrepMeth : Form
    {
        private Lemma<Guid, string> SampleType = null;        

        public FormSampTypeXPrepMeth(Lemma<Guid, string> sampType, string existingMethods)
        {
            InitializeComponent();

            SampleType = sampType;            

            tbSampleType.Text = StrUtils.SampleTypeNameToLabel(SampleType.Name);            

            using (SqlConnection conn = DB.OpenConnection())
            {
                string query = "";
                if (String.IsNullOrEmpty(existingMethods))
                    query = "select id, name from preparation_method order by name";
                else query = "select id, name from preparation_method where id not in(" + existingMethods + ") order by name";

                using (SqlDataReader reader = DB.GetDataReader(conn, query, CommandType.Text))
                {
                    lbPrepMeth.Items.Clear();

                    while (reader.Read())
                    {
                        var pm = new Lemma<Guid, string>(new Guid(reader["id"].ToString()), reader["name"].ToString());
                        lbPrepMeth.Items.Add(pm);
                    }
                }
            }                
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (lbPrepMeth.SelectedItems.Count > 0)
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand("insert into sample_type_x_preparation_method values(@sample_type_id, @preparation_method_id)", conn);

                    foreach (object item in lbPrepMeth.SelectedItems)
                    {
                        var selItem = item as Lemma<Guid, string>;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@sample_type_id", SampleType.Id);
                        cmd.Parameters.AddWithValue("@preparation_method_id", selItem.Id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
