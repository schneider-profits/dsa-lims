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
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormOrderAddPrepMeth : Form
    {
        private Assignment mAssignment = null;
        private AssignmentSampleType mAst = null;        

        public FormOrderAddPrepMeth(Assignment ass, AssignmentSampleType ast)
        {
            InitializeComponent();

            tbCount.KeyPress += CustomEvents.Integer_KeyPress;

            mAssignment = ass;
            mAst = ast;
            cbPrepsAlreadyExists.Checked = false;
            cboxPrepMethLaboratory.Enabled = false;            
        }

        private void FormOrderAddPrepMeth_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxPrepMethLaboratory);

                cboxPrepMethLaboratory.SelectedValue = mAssignment.LaboratoryId;
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
            if (!Utils.IsValidGuid(cboxPreparationMethod.SelectedValue))
            {
                MessageBox.Show("Preparation method is mandatory");
                return;
            }

            if(cbPrepsAlreadyExists.Checked && !Utils.IsValidGuid(cboxPrepMethLaboratory.SelectedValue))
            {
                MessageBox.Show("You must select a laboratory for external preparations");
                return;
            }

            if (String.IsNullOrEmpty(tbCount.Text.Trim()))
            {
                MessageBox.Show("Preparation method count is mandatory");
                return;
            }

            if(!Utils.IsValidInteger(tbCount.Text))
            {
                MessageBox.Show("Preparation method count must be a number");
                return;
            }

            int cnt = Convert.ToInt32(tbCount.Text);
            if(cnt < 1 || cnt > 10000)
            {
                MessageBox.Show("Preparation method count must be between 1 and 10000");
                return;
            }

            AssignmentPreparationMethod apm = new AssignmentPreparationMethod();
            apm.AssignmentSampleTypeId = mAst.Id;
            apm.PreparationMethodId = Utils.MakeGuid(cboxPreparationMethod.SelectedValue);
            apm.PreparationMethodCount = cnt;
            apm.PreparationLaboratoryId = Utils.MakeGuid(cboxPrepMethLaboratory.SelectedValue);
            apm.Comment = tbComment.Text.Trim();
            apm.CreateDate = DateTime.Now;
            apm.CreateId = Common.UserId;
            apm.UpdateDate = DateTime.Now;
            apm.UpdateId = Common.UserId;
            apm.UseExistingPreparation = cbPrepsAlreadyExists.Checked;
            apm.Dirty = true;
            mAst.PreparationMethods.Add(apm);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cbPrepsAlreadyExists_CheckedChanged(object sender, EventArgs e)
        {
            cboxPrepMethLaboratory.Enabled = cbPrepsAlreadyExists.Checked;
            if (!cbPrepsAlreadyExists.Checked)
                cboxPrepMethLaboratory.SelectedValue = mAssignment.LaboratoryId;
        }

        private void cboxPrepMethLaboratory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxPrepMethLaboratory.SelectedValue))
            {
                cboxPreparationMethod.DataSource = null;
                return;
            }

            Guid labId = Utils.MakeGuid(cboxPrepMethLaboratory.SelectedValue);

            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                UI.PopulateSampleTypePrepMeth(conn, mAst.SampleTypeId, labId, cboxPreparationMethod);
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
