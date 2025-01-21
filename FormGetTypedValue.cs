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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DSA_lims
{
    public partial class FormGetTypedValue : Form
    {
        public string SelectedValue { get; set; }

        public FormGetTypedValue(string useType)
        {
            InitializeComponent();

            if(cboxType.Items.Contains(useType))
            {
                cboxType.Text = useType;
                cboxType.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedValue = tbValue.Text.Trim();            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cboxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbValue.KeyPress -= CustomEvents.Numeric_KeyPress;
            tbValue.KeyPress -= CustomEvents.Integer_KeyPress;            

            switch (cboxType.Text)
            {
                case "Decimal":
                    tbValue.KeyPress += CustomEvents.Numeric_KeyPress;
                    break;
                case "Integer":
                    tbValue.KeyPress += CustomEvents.Integer_KeyPress;
                    break;
            }
        }        
    }
}
