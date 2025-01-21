using System;
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
    public partial class FormSelectLocationType : Form
    {
        public int SelectedLocationTypeId { get; set; }
        public string SelectedLocationType { get; set; }

        public FormSelectLocationType()
        {
            InitializeComponent();

            SelectedLocationTypeId = 0;
            SelectedLocationType = String.Empty;
        }

        private void FormSelectLocationType_Load(object sender, EventArgs e)
        {
            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();
                cboxLocationType.DataSource = DB.GetIntLemmata(conn, null, "csp_select_location_types", true);
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
            SelectedLocationTypeId = 0;
            SelectedLocationType = String.Empty;

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Lemma<int, string> l = cboxLocationType.SelectedItem as Lemma<int, string>;

            SelectedLocationTypeId = l.Id;
            SelectedLocationType = l.Name;

            DialogResult = DialogResult.OK;
            Close();
        }        
    }
}
