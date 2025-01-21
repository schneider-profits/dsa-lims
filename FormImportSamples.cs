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
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using OfficeOpenXml;
using Newtonsoft.Json;

namespace DSA_lims
{
    public partial class FormImportSamples : Form
    {
        private CultureInfo ciNO = new CultureInfo("nb-NO");

        private TreeView mTreeSampleTypes = null;
        private Guid mFileId = Guid.Empty;
        private string mProject = String.Empty;        

        public int FirstInsertedSampleNumber { get; private set; }
        public int LastInsertedSampleNumber { get; private set; }

        private List<SampleImportEntry> mSamples = new List<SampleImportEntry>();
        public List<SampleImportEntry> ImportedSamples { get { return mSamples; } private set { } }

        private Guid mSelectedLaboratoryId { get; set; }
        public Guid SelectedLaboratoryId { get { return mSelectedLaboratoryId; } private set { } }

        private Guid mSelectedSubProjectId { get; set; }
        public Guid SelectedSubProjectId { get { return mSelectedSubProjectId; } private set { } }

        public FormImportSamples(TreeView treeSampleTypes)
        {
            InitializeComponent();

            mTreeSampleTypes = treeSampleTypes;
            lblStatus.Text = "";
        }

        private void FormImportSamplesSampReg_Load(object sender, EventArgs e)
        {            
            SqlConnection conn = null;
            try
            {
                conn = DB.OpenConnection();

                UI.PopulateComboBoxes(conn, "csp_select_projects_main_short", new[] {
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxProjectMain);

                UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxLaboratory);
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

        private void miClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Validate required fields

            if(!Utils.IsValidGuid(cboxLaboratory.SelectedValue))
            {
                MessageBox.Show("You must select a laboratory first");
                return;
            }

            if (!Utils.IsValidGuid(cboxProjectSub.SelectedValue))
            {
                MessageBox.Show("You must select a project first");
                return;
            }
            
            if(mSamples.Count(x => x.Importable == true) < 1)
            {
                MessageBox.Show("No samples to import");
                return;
            }

            SqlConnection conn = null;
            SqlTransaction trans = null;

            try
            {
                panelImport.Enabled = false;
                progressImport.Maximum = mSamples.Count * 2;
                progressImport.Value = 0;
                progressImport.Visible = true;

                foreach (SampleImportEntry se in mSamples)
                {
                    progressImport.Value++;

                    if (!se.Importable)
                        continue;

                    if (!Utils.IsValidGuid(se.LIMSSampleTypeId))
                    {
                        MessageBox.Show("Sample " + se.Number + " must have a LIMS sample type");
                        return;
                    }

                    if (se.ReferenceDate == null || se.ReferenceDate == DateTime.MinValue)
                    {
                        MessageBox.Show("Sample " + se.Number + " must have a reference date");
                        return;
                    }                                        
                }            

                mSelectedLaboratoryId = Utils.MakeGuid(cboxLaboratory.SelectedValue);
                mSelectedSubProjectId = Utils.MakeGuid(cboxProjectSub.SelectedValue);

                // Define required parameter names

                Dictionary<string, string> paramNames = new Dictionary<string, string>()
                {
                    { "Uke", "Integer" },
                    { "Gjennomstrømningsvolum (m3)", "Decimal" },
                    { "Volum (l)", "Decimal" },
                    { "Dyp (m)", "Decimal" },
                    { "Lufthastighet (m3/t)", "Decimal" },
                    { "Salinitet (%)", "Decimal" },
                    { "Sjikt fra (cm)", "Decimal" },
                    { "Sjikt til (cm)", "Decimal" },
                    { "Areal (cm2)", "Decimal" },
                    { "Temp. C.", "Decimal" },
                    { "Tracer nukl.", "Text" },
                    { "Tracer vekt (g)", "Decimal" },
                    { "Tracer volum (ml)", "Decimal" },
                    { "Tiltenkt analyse", "Text" },
                    { "pH", "Decimal" },
                    { "Skal filtreres? (J/N)", "Text" }
                };                
            
                conn = DB.OpenConnection();
                trans = conn.BeginTransaction();

                panelImport.Enabled = false;
                progressImport.Maximum = mSamples.Count;
                progressImport.Value = 0;
                progressImport.Visible = true;

                // Create required parameter names

                foreach (KeyValuePair<string, string> p in paramNames)
                {
                    DB.CreateSampleParameterName(conn, trans, p.Key, p.Value);
                }

                // Insert samples

                DateTime currDate = DateTime.Now;
                bool firstSample = true;
                int nextSampleNumber = 0;

                foreach (SampleImportEntry se in mSamples)
                {
                    progressImport.Value++;

                    if (!se.Importable)
                        continue;

                    Sample sample = new Sample();
                    nextSampleNumber = DB.GetNextSampleCount(conn, trans);
                    sample.Number = nextSampleNumber;
                    sample.ExternalId = se.ExternalId;
                    sample.Latitude = se.Latitude;
                    sample.Longitude = se.Longitude;
                    sample.Altitude = se.Altitude;
                    sample.SamplingDateFrom = se.SamplingDateFrom == DateTime.MinValue ? null : se.SamplingDateFrom;
                    sample.SamplingDateTo = se.SamplingDateTo == DateTime.MinValue ? null : se.SamplingDateTo;
                    sample.ReferenceDate = se.ReferenceDate == DateTime.MinValue ? null : se.ReferenceDate;
                    if (!String.IsNullOrEmpty(se.Location))
                    {
                        sample.LocationType = se.LIMSLocationType;
                        sample.Location = se.Location;
                    }
                    sample.LaboratoryId = mSelectedLaboratoryId;
                    sample.SampleTypeId = se.LIMSSampleTypeId;
                    sample.DryWeight_g = se.DryWeight_g;
                    sample.WetWeight_g = se.WetWeight_g;
                    sample.SampleTypeId = se.LIMSSampleTypeId;
                    sample.SampleComponentId = se.LIMSSampleComponentId;
                    sample.SamplerId = se.LIMSSamplerId;
                    sample.SamplingMethodId = se.LIMSSamplingMethodId;
                    sample.ProjectSubId = mSelectedSubProjectId;                    
                    sample.InstanceStatusId = InstanceStatus.Active;
                    sample.Comment = se.Comment;
                    sample.ImportedFrom = se.ImportedFrom;
                    sample.ImportedFromId = se.ImportedFromId;
                    sample.CreateDate = currDate;
                    sample.CreateId = Common.UserId;
                    sample.UpdateDate = currDate;
                    sample.UpdateId = Common.UserId;
                    sample.StoreToDB(conn, trans);

                    // Insert parameters
                    if(se.WeekNumber != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Uke", se.WeekNumber);

                    if (se.FlowVolume_m3 != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Gjennomstrømningsvolum (m3)", se.FlowVolume_m3);

                    if (se.Volume_l != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Volum (l)", se.Volume_l);

                    if (se.Depth_m != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Dyp (m)", se.Depth_m);

                    if (se.AirSpeed_m3_t != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Lufthastighet (m3/t)", se.AirSpeed_m3_t);

                    if (se.Salinity != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Salinitet (%)", se.Salinity);

                    if (se.LayerFrom_cm != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Sjikt fra (cm)", se.LayerFrom_cm);

                    if (se.LayerTo_cm != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Sjikt til (cm)", se.LayerTo_cm);

                    if (se.Area_cm2 != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Areal (cm2)", se.Area_cm2);

                    if (se.Temperature_C != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Temp. C.", se.Temperature_C);

                    if (!String.IsNullOrEmpty(se.TracerNuclide))
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Tracer nukl.", se.TracerNuclide);

                    if (se.TracerWeight_g != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Tracer vekt (g)", se.TracerWeight_g);

                    if (se.TracerVolume_ml != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Tracer volum (ml)", se.TracerVolume_ml);

                    if (!String.IsNullOrEmpty(se.IntendedAnalysis))
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Tiltenkt analyse", se.IntendedAnalysis);

                    if (se.pH != null)
                        DB.InsertSampleParameter(conn, trans, sample.Id, "pH", se.pH);

                    if (!String.IsNullOrEmpty(se.RequireFiltering))
                        DB.InsertSampleParameter(conn, trans, sample.Id, "Skal filtreres? (J/N)", se.RequireFiltering);

                    string json = JsonConvert.SerializeObject(sample);
                    DB.AddAuditMessage(conn, trans, "sample", sample.Id, AuditOperationType.Insert, json, "");

                    if(firstSample)
                    {
                        FirstInsertedSampleNumber = nextSampleNumber;
                        firstSample = false;
                    }                    
                }

                // Insert file id
                if (Utils.IsValidGuid(mFileId))
                {
                    DB.InsertImportFileIdentifier(conn, trans, mFileId);
                }

                LastInsertedSampleNumber = nextSampleNumber;

                trans.Commit();
            }
            catch (Exception ex)
            {
                trans?.Rollback();
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn?.Close();
                panelImport.Enabled = true;
                progressImport.Visible = false;
            }

            DialogResult = DialogResult.OK;
            Close();
        }        

        private void Populate()
        {            
            SqlConnection conn = null;

            try
            {
                panelImport.Enabled = false;
                progressImport.Maximum = gridSamples.SelectedRows.Count + mSamples.Count + mSamples.Count;
                progressImport.Value = 0;
                progressImport.Visible = true;

                tbFileID.Text = mFileId.ToString();
                tbProject.Text = mProject;

                List<int> selectedIndices = new List<int>();
                foreach (DataGridViewRow row in gridSamples.SelectedRows)
                {
                    progressImport.Value++;

                    selectedIndices.Add(row.Index);
                }

                gridSamples.Rows.Clear();

                conn = DB.OpenConnection();                

                foreach (SampleImportEntry se in mSamples)
                {
                    progressImport.Value++;

                    if (se.LIMSSampleTypeId == Guid.Empty && !String.IsNullOrEmpty(se.SampleType))
                    {                        
                        using (SqlDataReader reader = DB.GetDataReader(conn, null, "select id, name from sample_type where name like @name", CommandType.Text, new SqlParameter("@name", se.SampleType)))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                se.LIMSSampleType = reader["name"].ToString();
                                se.LIMSSampleTypeId = Utils.MakeGuid(reader["id"]);
                            }
                        }
                    }

                    if (se.LIMSSamplerId == Guid.Empty && !String.IsNullOrEmpty(se.Sampler))
                    {
                        using (SqlDataReader reader = DB.GetDataReader(conn, null, "select id, person_name from cv_sampler where person_name like @name", CommandType.Text, new SqlParameter("@name", se.Sampler)))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                se.LIMSSampler = reader["person_name"].ToString();
                                se.LIMSSamplerId = Utils.MakeGuid(reader["id"]);
                            }
                        }
                    }

                    if (se.LIMSSamplingMethodId == Guid.Empty && !String.IsNullOrEmpty(se.SamplingMethod))
                    {
                        using (SqlDataReader reader = DB.GetDataReader(conn, null, "select id, name from sampling_method where name like @name", CommandType.Text, new SqlParameter("@name", se.SamplingMethod)))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                se.LIMSSamplingMethod = reader["name"].ToString();
                                se.LIMSSamplingMethodId = Utils.MakeGuid(reader["id"]);
                            }
                        }
                    }

                    if (se.LIMSTracerNuclideId == Guid.Empty && !String.IsNullOrEmpty(se.TracerNuclide))
                    {
                        using (SqlDataReader reader = DB.GetDataReader(conn, null, "select id, name from nuclide where name like @name", CommandType.Text, new SqlParameter("@name", se.TracerNuclide)))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                se.LIMSTracerNuclide = reader["name"].ToString();
                                se.LIMSTracerNuclideId = Utils.MakeGuid(reader["id"]);
                            }
                        }
                    }

                    gridSamples.Rows.Add(
                        se.Importable,
                        se.Number,
                        se.SampleType,
                        se.LIMSSampleType,
                        se.SampleComponent,
                        se.LIMSSampleComponent,
                        se.ExternalId,
                        se.SamplingDateFrom == DateTime.MinValue ? null : se.SamplingDateFrom,
                        se.SamplingDateTo == DateTime.MinValue ? null : se.SamplingDateTo,
                        se.ReferenceDate == DateTime.MinValue ? null : se.ReferenceDate,
                        se.Location,
                        se.LIMSLocationType,
                        se.Latitude,
                        se.Longitude,
                        se.Altitude,
                        se.Sampler,
                        se.LIMSSampler,
                        se.SamplingMethod,
                        se.LIMSSamplingMethod,
                        se.Comment,
                        se.WeekNumber,
                        se.FlowVolume_m3,
                        se.Volume_l,
                        se.Depth_m,
                        se.DryWeight_g,
                        se.WetWeight_g,
                        se.AirSpeed_m3_t,
                        se.Salinity,
                        se.LayerFrom_cm,
                        se.LayerTo_cm,
                        se.Area_cm2,
                        se.Temperature_C,
                        se.TracerNuclide,
                        se.LIMSTracerNuclide,
                        se.TracerWeight_g,
                        se.TracerVolume_ml,
                        se.IntendedAnalysis,
                        se.pH,
                        se.RequireFiltering
                    );                    
                }

                gridSamples.Columns["colSamplingDateFrom"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;
                gridSamples.Columns["colSamplingDateTo"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;
                gridSamples.Columns["colReferenceDate"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;

                foreach (DataGridViewRow row in gridSamples.Rows)
                {
                    progressImport.Value++;

                    if (selectedIndices.Contains(row.Index))
                        row.Selected = true;
                    else row.Selected = false;
                }
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn?.Close();

                panelImport.Enabled = true;
                progressImport.Visible = false;
            }            
        }

        private void cboxProjectMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxProjectMain.SelectedValue))
            {
                cboxProjectSub.DataSource = null;
                return;
            }

            SqlConnection conn = null;
            try
            {
                Guid projectId = Utils.MakeGuid(cboxProjectMain.SelectedValue);

                conn = DB.OpenConnection();

                UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                    new SqlParameter("@project_main_id", projectId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxProjectSub);
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

        private void ParseLine(string line)
        {
            string[] items = line.Split(new char[] { '|' });

            if (items.Length != 14)
                throw new Exception("Wrong number of items in line. Expected 14, got " + items.Length);

            SampleImportEntry s = new SampleImportEntry();
            s.Number = Convert.ToInt32(items[2]);
            s.ExternalId = items[0] + " - " + items[2];
            mProject = items[1].Trim();
            DateTimeOffset dto = DateTimeOffset.Parse(items[3], CultureInfo.InvariantCulture);
            s.SamplingDateFrom = dto.DateTime + dto.Offset;
            s.Latitude = Convert.ToDouble(items[4].Trim());
            s.Longitude = Convert.ToDouble(items[5].Trim());
            s.Altitude = Convert.ToDouble(items[6].Trim());
            s.Location = items[7].Trim();
            s.SampleType = items[8].Trim();
            if (items.Length >= 14)
                s.Comment = items[13].Trim();

            mSamples.Add(s);
        }                

        private void miImportLIMSSamples_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.Text = "";

            // import from LIMS sample import sheet
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(dialog.FileName)))
                {
                    var sheet = xlPackage.Workbook.Worksheets.First(x => x.Name == "Prøver");
                    if (sheet == null)
                    {
                        MessageBox.Show("Invalid Excel sheet: Unable to locate sheet 'Prøver'");
                        return;
                    }

                    var totalRows = sheet.Dimension.End.Row;
                    var totalColumns = sheet.Dimension.End.Column;

                    if (totalColumns < 31)
                    {
                        MessageBox.Show("Invalid Excel sheet: Too few columns");
                        return;
                    }

                    if (totalRows < 6)
                    {
                        MessageBox.Show("Invalid Excel sheet: Too few rows");
                        return;
                    }

                    int version = Convert.ToInt32(sheet.GetValue(2, 12));
                    if (version <= 0)
                    {
                        MessageBox.Show("Invalid Excel sheet: Unrecognized version");
                        return;
                    }

                    //MessageBox.Show("Version: " + version);

                    mFileId = Guid.Empty;
                    mSamples.Clear();                    

                    for (int row = 6; row <= totalRows; row++)
                    {
                        SampleImportEntry sie = new SampleImportEntry();

                        sie.Number = row - 5;
                        sie.SampleType = GetExcelString(sheet, row, 1);
                        sie.SampleComponent = GetExcelString(sheet, row, 2);
                        sie.ExternalId = GetExcelString(sheet, row, 3);                        
                        try
                        {
                            sie.SamplingDateFrom = GetExcelOADate(sheet, row, 4);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Sampling date from");
                        }
                        try
                        {
                            sie.SamplingDateTo = GetExcelOADate(sheet, row, 5);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Sampling date to");
                        }
                        try
                        {
                            sie.ReferenceDate = GetExcelOADate(sheet, row, 6);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Reference date");
                        }                        
                        sie.Location = GetExcelString(sheet, row, 7);
                        try
                        {
                            sie.Latitude = GetExcelDouble(sheet, row, 8);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Latitude");
                        }
                        try
                        {
                            sie.Longitude = GetExcelDouble(sheet, row, 9);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Longitude");
                        }
                        try
                        {
                            sie.Altitude = GetExcelDouble(sheet, row, 10);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Altitude");
                        }                        
                        sie.Comment = GetExcelString(sheet, row, 11);
                        sie.Sampler = GetExcelString(sheet, row, 12);
                        sie.SamplingMethod = GetExcelString(sheet, row, 13);
                        try
                        {
                            sie.WeekNumber = GetExcelInt(sheet, row, 14);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Week number");
                        }
                        try
                        {
                            sie.FlowVolume_m3 = GetExcelDouble(sheet, row, 15);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Flow volume");
                        }
                        try
                        {
                            sie.Volume_l = GetExcelDouble(sheet, row, 16);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Volume");
                        }
                        try
                        {
                            sie.Depth_m = GetExcelDouble(sheet, row, 17);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Depth");
                        }
                        try
                        {
                            sie.DryWeight_g = GetExcelDouble(sheet, row, 18);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Dry weight");
                        }
                        try
                        {
                            sie.WetWeight_g = GetExcelDouble(sheet, row, 19);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Wet weight");
                        }
                        try
                        {
                            sie.AirSpeed_m3_t = GetExcelDouble(sheet, row, 20);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Air speed");
                        }
                        try
                        {
                            sie.Salinity = GetExcelDouble(sheet, row, 21);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Salinity");
                        }
                        try
                        {
                            sie.LayerFrom_cm = GetExcelDouble(sheet, row, 22);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Layer from");
                        }
                        try
                        {
                            sie.LayerTo_cm = GetExcelDouble(sheet, row, 23);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Layer to");
                        }
                        try
                        {
                            sie.Area_cm2 = GetExcelDouble(sheet, row, 24);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Area");
                        }
                        try
                        {
                            sie.Temperature_C = GetExcelDouble(sheet, row, 25);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Temperature");
                        }                        
                        sie.TracerNuclide = GetExcelString(sheet, row, 26);
                        try
                        {
                            sie.TracerWeight_g = GetExcelDouble(sheet, row, 27);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Tracer weight");
                        }
                        try
                        {
                            sie.TracerVolume_ml = GetExcelDouble(sheet, row, 28);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Tracer volume");
                        }                        
                        sie.IntendedAnalysis = GetExcelString(sheet, row, 29);
                        try
                        {
                            sie.pH = GetExcelDouble(sheet, row, 30);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": pH");
                        }                        
                        sie.RequireFiltering = GetExcelString(sheet, row, 31);

                        mSamples.Add(sie);
                    }
                }

                Populate();
            }
            catch(Exception ex)
            {                
                Common.Log.Error(ex);
                mSamples.Clear();
                MessageBox.Show(ex.Message);
            }
        }

        private string GetExcelString(ExcelWorksheet sheet, int row, int column)
        {
            object o = sheet.GetValue(row, column);
            if (o != null)            
                return o.ToString();
            return String.Empty;
        }

        private int? GetExcelInt(ExcelWorksheet sheet, int row, int column)
        {
            object o = sheet.GetValue(row, column);
            if (o != null)            
                return Convert.ToInt32(o, ciNO);
            return null;
        }

        private double? GetExcelDouble(ExcelWorksheet sheet, int row, int column)
        {
            object o = sheet.GetValue(row, column);
            if (o != null)
                return Convert.ToDouble(o, ciNO);
            return null;
        }

        private DateTime? GetExcelOADate(ExcelWorksheet sheet, int row, int column)
        {
            object o = sheet.GetValue(row, column);
            if (o != null)
                return DateTime.FromOADate(Convert.ToDouble(o));
            return null;
        }

        private DateTime? GetExcelDateTime(ExcelWorksheet sheet, int row, int column)
        {
            object o = sheet.GetValue(row, column);
            if (o != null)
            {
                DateTime dt;
                if (!DateTime.TryParseExact(o.ToString(), "dd.MM.yyyy", null, DateTimeStyles.None, out dt))
                    return null;
                return dt;
            }
            return null;
        }

        private void miImportSampleRegistration_Click(object sender, EventArgs e)
        {
            // import from sample registration

            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.Text = "";

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            mFileId = Guid.Empty;
            mSamples.Clear();
            mProject = String.Empty;            

            String line = String.Empty;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader(dialog.FileName);

                line = reader.ReadLine();
                if (line == null)
                {
                    MessageBox.Show("File " + dialog.FileName + " does not appear to be valid");
                    return;
                }

                try
                {
                    mFileId = Guid.Parse(line.Trim());
                }
                catch
                {
                    MessageBox.Show("File " + dialog.FileName + " does not appear to be valid");
                    return;
                }

                using (SqlConnection conn = DB.OpenConnection())
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from imported_file_identifiers where id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", mFileId);
                    int cnt = (int)cmd.ExecuteScalar();
                    if(cnt > 0)
                    {
                        lblStatus.ForeColor = Color.OrangeRed;
                        lblStatus.Text = "Warning: Samples has been imported from this file before";
                    }
                }                    

                while ((line = reader.ReadLine()) != null)
                {
                    ParseLine(line);
                }

                Populate();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                reader?.Close();
            }
        }

        private void miSetLIMSSampleType_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectSampleType form = new FormSelectSampleType();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSSampleTypeId = form.SelectedSampleTypeId;
                se.LIMSSampleType = form.SelectedSampleTypeName;                

                se.LIMSSampleComponentId = Guid.Empty;
                se.LIMSSampleComponent = String.Empty;
            }

            Populate();
        }

        private void miSetLIMSSampleComponent_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            Guid currentSampleTypeId = Guid.Empty;
            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                if (currentSampleTypeId == Guid.Empty)
                {
                    currentSampleTypeId = se.LIMSSampleTypeId;
                }
                else if(currentSampleTypeId != se.LIMSSampleTypeId)
                {
                    MessageBox.Show("All selected samples must have the same type");
                    return;
                }
            }

            if(currentSampleTypeId == Guid.Empty)
            {
                MessageBox.Show("No sample type found for the selected samples");
                return;
            }

            TreeNode[] nodes = mTreeSampleTypes.Nodes.Find(currentSampleTypeId.ToString(), true);
            if(nodes.Length < 1)
            {
                MessageBox.Show("No tree node found for the selected sample type");
                return;
            }

            FormSelectSampleComponent form = new FormSelectSampleComponent(currentSampleTypeId, nodes[0]);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSSampleComponentId = form.SelectedSampleComponentId;
                se.LIMSSampleComponent = form.SelectedSampleComponent;                
            }

            Populate();
        }

        private void miSetExternalId_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Text");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);                
                if (se == null)
                    continue;

                se.ExternalId = form.SelectedValue;
            }

            Populate();
        }

        private void miSetSamplingDateFrom_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectDateTime form = new FormSelectDateTime("Select sampling date from");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.SamplingDateFrom = form.SelectedDateTime;
            }

            Populate();
        }

        private void miSetSamplingDateTo_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectDateTime form = new FormSelectDateTime("Select sampling date to");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.SamplingDateTo = form.SelectedDateTime;
            }

            Populate();
        }

        private void miSetReferenceDate_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectDateTime form = new FormSelectDateTime("Select reference date");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.ReferenceDate = form.SelectedDateTime;
            }

            Populate();
        }

        private void miSetLocationType_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormSelectLocationType form = new FormSelectLocationType();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSLocationTypeId = form.SelectedLocationTypeId;
                se.LIMSLocationType = form.SelectedLocationType;
            }

            Populate();
        }

        private void miSetLocation_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Text");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Location = form.SelectedValue;
            }

            Populate();
        }

        private void miSetLatitude_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Latitude = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetLongitude_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Longitude = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetAltitude_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Altitude = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetSampler_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectSampler form = new FormSelectSampler();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSSamplerId = form.SelectedSamplerId;
                se.LIMSSampler = form.SelectedSamplerName;
            }

            Populate();
        }

        private void miSetSamplingMethod_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectSamplingMethod form = new FormSelectSamplingMethod();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSSamplingMethodId = form.SelectedSamplingMethodId;
                se.LIMSSamplingMethod = form.SelectedSamplingMethodName;
            }

            Populate();
        }

        private void miSetComment_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Text");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Comment = form.SelectedValue;
            }

            Populate();
        }

        private void miSetWeekNumber_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Integer");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.WeekNumber = Convert.ToInt32(form.SelectedValue);
            }

            Populate();
        }

        private void miSetFlowVolume_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.FlowVolume_m3 = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetVolume_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Volume_l = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetDepth_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Depth_m = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetDryWeight_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.DryWeight_g = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetWetWeight_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.WetWeight_g = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetAirSpeed_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.AirSpeed_m3_t = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetSalinity_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Salinity = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetLayerFrom_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LayerFrom_cm = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetLayerTo_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LayerTo_cm = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetArea_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Area_cm2 = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetTemperature_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Temperature_C = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetTracerNuclide_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            FormSelectNuclide form = new FormSelectNuclide(false);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.LIMSTracerNuclideId = form.SelectedNuclides[0].Id;
                se.LIMSTracerNuclide = form.SelectedNuclides[0].Name;
            }

            Populate();
        }

        private void miSetTracerWeight_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.TracerWeight_g = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetTracerVolume_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.TracerVolume_ml = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetIntendedAnalysis_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Text");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.IntendedAnalysis = form.SelectedValue;
            }

            Populate();
        }

        private void miSetPH_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Decimal");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.pH = Convert.ToDouble(form.SelectedValue);
            }

            Populate();
        }

        private void miSetRequireFiltering_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            FormGetTypedValue form = new FormGetTypedValue("Text");
            if (form.ShowDialog() != DialogResult.OK)
                return;

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.RequireFiltering = form.SelectedValue;
            }

            Populate();
        }

        private void miSetImportable_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Importable = true;
            }

            Populate();

            SetStatusLabel();
        }

        private void miSetNonImportable_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more rows first");
                return;
            }

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                int num = Convert.ToInt32(row.Cells["colNumber"].Value);
                SampleImportEntry se = mSamples.Find(x => x.Number == num);
                if (se == null)
                    continue;

                se.Importable = false;
            }

            Populate();

            SetStatusLabel();
        }

        private void miMarkAllImportable_Click(object sender, EventArgs e)
        {
            foreach (SampleImportEntry se in mSamples)
                se.Importable = true;            

            Populate();

            SetStatusLabel();
        }

        private void SetStatusLabel()
        {
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.Text = "";
            int importable = 0;

            foreach (SampleImportEntry se in mSamples)
            {
                if(se.Importable && String.IsNullOrEmpty(se.ExternalId))
                {
                    lblStatus.ForeColor = Color.OrangeRed;
                    lblStatus.Text = "Warning: One or more selected samples are missing an external ID";
                    return;
                }

                if(se.Importable)
                    importable++;
            }
            
            if(importable > 0)            
                lblStatus.Text = importable + " samples selected";
        }

        private void miMarkAllNonImportable_Click(object sender, EventArgs e)
        {
            foreach (SampleImportEntry se in mSamples)
                se.Importable = false;

            Populate();

            SetStatusLabel();
        }

        private void miImportMINASamples_Click(object sender, EventArgs e)
        {
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.Text = "";

            // import from MINA sample import sheet
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "XLSX files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(dialog.FileName)))
                {
                    ExcelWorksheet sheet = null;
                    try
                    {
                        sheet = xlPackage.Workbook.Worksheets.First(x => x.Name.ToLower() == "samples");
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Excel sheet: Unable to locate sheet 'Samples'");
                        return;
                    }

                    if(sheet == null)
                    {
                        MessageBox.Show("Invalid Excel sheet: Unable to locate sheet 'Samples'");
                        return;
                    }

                    var totalRows = sheet.Dimension.End.Row;
                    var totalColumns = sheet.Dimension.End.Column;

                    if (totalRows < 2)
                    {
                        MessageBox.Show("Invalid Excel sheet: Too few rows");
                        return;
                    }

                    int? minaProject = 0;
                    try
                    {
                        minaProject = GetExcelInt(sheet, 2, 1);
                    }
                    catch
                    {                        
                        MessageBox.Show("Invalid Excel sheet: Unable to extract project ID from field A:2");
                        return;
                    }

                    if (minaProject == null || minaProject.Value <= 0)
                    {
                        MessageBox.Show("Invalid Excel sheet: Unrecognized project ID in field A:2");
                        return;
                    }

                    int? sampleIdColumn = null;
                    int? sampleTypeColumn = null;
                    int? sampleSubTypeColumn = null;
                    int? samplePartColumn = null;
                    int? refDateColumn = null;
                    int? latitudeColumn = null;
                    int? longitudeColumn = null;
                    int? commentColumn = null;
                    int? waterDepthColumn = null;
                    int? salinityColumn = null;
                    int? temperatureColumn = null;
                    int? localityColumn = null;
                    int? stationColumn = null;

                    for (int i = 0; i < totalColumns; i++)
                    {
                        string s = GetExcelString(sheet, 4, i);
                        switch(s)
                        {
                            case "SAMPLEID":
                                sampleIdColumn = i;
                                break;
                            case "SAMPLETYPE":
                                sampleTypeColumn = i;
                                break;
                            case "SAMPLESUBTYPE":
                                sampleSubTypeColumn = i;
                                break;
                            case "SAMPLEPART":
                                samplePartColumn = i;
                                break;
                            case "REF_DATE":
                                refDateColumn = i;
                                break;
                            case "LATITUDE":
                                latitudeColumn = i;
                                break;
                            case "LONGITUDE":
                                longitudeColumn = i;
                                break;
                            case "COMMENT":
                                commentColumn = i;
                                break;
                            case "WATERDEPTH":
                                waterDepthColumn = i;
                                break;
                            case "SALINITY":
                                salinityColumn = i;
                                break;
                            case "TEMPERATURE":
                                temperatureColumn = i;
                                break;
                            case "LOCALITY":
                                localityColumn = i;
                                break;
                            case "STATION":
                                stationColumn = i;
                                break;
                        }
                    }

                    if (sampleIdColumn == null)
                        throw new Exception("Invalid Excel sheet: No column found for SAMPLEID");
                    if (sampleTypeColumn == null)
                        throw new Exception("Invalid Excel sheet: No column found for SAMPLETYPE");
                    if (refDateColumn == null)
                        throw new Exception("Invalid Excel sheet: No column found for REF_DATE");

                    mFileId = Guid.Empty;
                    mSamples.Clear();

                    panelImport.Enabled = false;
                    progressImport.Maximum = totalRows;
                    progressImport.Value = 0;
                    progressImport.Visible = true;

                    for (int row = 6; row <= totalRows; row++)
                    {
                        SampleImportEntry sie = new SampleImportEntry();                        

                        sie.Number = row - 5;
                        
                        sie.ExternalId = GetExcelString(sheet, row, sampleIdColumn.Value);

                        if (sampleSubTypeColumn != null)
                        {
                            sie.SampleType = GetExcelString(sheet, row, sampleSubTypeColumn.Value);
                            if(String.IsNullOrEmpty(sie.SampleType))
                            {
                                sie.SampleType = GetExcelString(sheet, row, sampleTypeColumn.Value);
                            }
                        }

                        if (samplePartColumn != null)
                        {
                            sie.SampleComponent = GetExcelString(sheet, row, samplePartColumn.Value);
                        }

                        try
                        {
                            sie.ReferenceDate = GetExcelDateTime(sheet, row, refDateColumn.Value);
                        }
                        catch
                        {
                            throw new Exception("Invalid format on row " + row + ": Reference date (Format must be 'dd.MM.yyyy')");
                        }

                        if (latitudeColumn != null)
                        {
                            try
                            {
                                sie.Latitude = GetExcelDouble(sheet, row, latitudeColumn.Value);
                            }
                            catch
                            {
                                throw new Exception("Invalid format on row " + row + ": Latitude");
                            }
                        }

                        if (longitudeColumn != null)
                        {
                            try
                            {
                                sie.Longitude = GetExcelDouble(sheet, row, longitudeColumn.Value);
                            }
                            catch
                            {
                                throw new Exception("Invalid format on row " + row + ": Longitude");
                            }
                        }

                        if (commentColumn != null)                        
                            sie.Comment = GetExcelString(sheet, row, commentColumn.Value);

                        if (waterDepthColumn != null)
                        {
                            try
                            {
                                sie.Depth_m = GetExcelDouble(sheet, row, waterDepthColumn.Value);
                            }
                            catch
                            {
                                throw new Exception("Invalid format on row " + row + ": Water depth");
                            }
                        }

                        if (salinityColumn != null)
                        {
                            try
                            {
                                sie.Salinity = GetExcelDouble(sheet, row, salinityColumn.Value);
                            }
                            catch
                            {
                                throw new Exception("Invalid format on row " + row + ": Salinity");
                            }
                        }

                        if (temperatureColumn != null)
                        {
                            try
                            {
                                sie.Temperature_C = GetExcelDouble(sheet, row, temperatureColumn.Value);
                            }
                            catch
                            {
                                throw new Exception("Invalid format on row " + row + ": Temperature");
                            }
                        }

                        if (localityColumn != null)
                        {
                            sie.Location = GetExcelString(sheet, row, localityColumn.Value);                            
                        }

                        if (stationColumn != null)
                        {
                            string st = GetExcelString(sheet, row, stationColumn.Value);
                            if (!String.IsNullOrEmpty(st))
                            {
                                if (!String.IsNullOrEmpty(sie.Location))
                                    sie.Location += ", Station: " + st;
                                else sie.Location = "Station: " + st;
                            }
                        }

                        if (!String.IsNullOrEmpty(sie.Location))
                        {
                            sie.LIMSLocationType = "Other";
                            sie.LIMSLocationTypeId = 5;
                        }

                        sie.ImportedFrom = "MINA";
                        sie.ImportedFromId = sie.ExternalId;

                        mSamples.Add(sie);

                        progressImport.Value++;
                    }
                }

                Populate();
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                mSamples.Clear();
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panelImport.Enabled = true;
                progressImport.Visible = false;
            }
        }
    }    
}
