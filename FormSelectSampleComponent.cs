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
    public partial class FormSelectSampleComponent : Form
    {
        private Guid mSampleTypeId = Guid.Empty;
        private TreeNode mTreeNode = null;

        public Guid SelectedSampleComponentId { get; set; }
        public string SelectedSampleComponent { get; set; }

        public FormSelectSampleComponent(Guid sampleTypeId, TreeNode treeNode)
        {
            InitializeComponent();

            mSampleTypeId = sampleTypeId;
            mTreeNode = treeNode;
            SelectedSampleComponentId = Guid.Empty;
            SelectedSampleComponent = String.Empty;
            lblInfo.Text = "Select a sample component for sample type " + mTreeNode.Text;
        }

        private void FormSelectSampleComponent_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                UI.PopulateSampleComponentsAscending(conn, mSampleTypeId, mTreeNode, cboxSampleComponent);
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn?.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedSampleComponentId = Guid.Empty;
            SelectedSampleComponent = String.Empty;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Lemma<Guid, string> l = cboxSampleComponent.SelectedItem as Lemma<Guid, string>;
            if (Utils.IsValidGuid(l.Id))
            {
                SelectedSampleComponentId = l.Id;
                SelectedSampleComponent = l.Name;
            }
            else
            {
                SelectedSampleComponentId = Guid.Empty;
                SelectedSampleComponent = String.Empty;
            }

            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
