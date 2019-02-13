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
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.Reflection;
using System.Data.SqlClient;
using log4net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DSA_lims
{
    public partial class FormMain : Form
    {
        private ResourceManager r = null;
        
        int statusMessageTimeout = 20000;
        System.Timers.Timer statusMessageTimer = null;

        private Sample sample = new Sample();
        private Assignment assignment = new Assignment();
        private Analysis analysis = new Analysis();
        private Preparation preparation = new Preparation();

        public FormMain()
        {
            InitializeComponent();

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en");
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            tabs.Appearance = TabAppearance.FlatButtons;
            tabs.ItemSize = new Size(0, 1);
            tabs.SizeMode = TabSizeMode.Fixed;
            tabs.SelectedTab = tabMenu;

            tabsPrepAnal.Appearance = TabAppearance.FlatButtons;
            tabsPrepAnal.ItemSize = new Size(0, 1);
            tabsPrepAnal.SizeMode = TabSizeMode.Fixed;
            tabsPrepAnal.SelectedTab = tabMenu;

            lblCurrentTab.Text = tabs.SelectedTab.Text;
            lblStatus.Text = "";
            lblCurrentUser.Text = "";
            lblSampleToolId.Text = "";
            lblSampleToolExId.Text = "";
            lblSampleToolProject.Text = "";
            lblSampleToolSubProject.Text = "";
            lblSampleToolLaboratory.Text = "";
            tbMenuLookup.Text = "";

            tbMenuLookup.KeyPress += CustomEvents.Integer_KeyPress;
            tbSamplesLookup.KeyPress += CustomEvents.Integer_KeyPress;            
            tbSampleInfoAltitude.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalWetWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalDryWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalVolume.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalLODStartWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalLODEndWeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalLODWater.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalPrepFillHeight.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalPrepAmount.KeyPress += CustomEvents.Numeric_KeyPress;
            tbPrepAnalPrepQuantity.KeyPress += CustomEvents.Numeric_KeyPress;            
        }        

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Directory.Exists(DSAEnvironment.SettingsPath))
                    Directory.CreateDirectory(DSAEnvironment.SettingsPath);

                Common.Log = DSALogger.CreateLogger(DSAEnvironment.SettingsPath + Path.DirectorySeparatorChar + "dsa-lims.log");

                Common.Log.Info("Loading settings file " + DSAEnvironment.SettingsFilename);
                LoadSettings(DSAEnvironment.SettingsFilename);

                DB.ConnectionString = Common.Settings.ConnectionString;

                r = new ResourceManager("DSA_lims.lang_" + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Assembly.GetExecutingAssembly());
                Common.Log.Info("Setting language " + CultureInfo.CurrentUICulture.TwoLetterISOLanguageName);
                SetLanguageLabels(r);

                statusMessageTimer = new System.Timers.Timer(statusMessageTimeout);
                statusMessageTimer.SynchronizingObject = this;
                statusMessageTimer.Elapsed += StatusMessageTimer_Elapsed;
                statusMessageTimer.AutoReset = false;
                statusMessageTimer.Enabled = false;
            }
            catch (Exception ex)
            {
                if(Common.Log != null)
                    Common.Log.Fatal(ex);
                MessageBox.Show(ex.Message);
                Environment.Exit(1);
            }
        }

        private void StatusMessageTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lblStatus.Text = "";
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            bool initialized = false;
            while (!initialized)
            {
                HideMenuItems();
                ShowLogin();
                initialized = InitializeUI();
                Application.Idle += Application_Idle;
            }
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            if (tabs.SelectedTab == tabPrepAnal)
            {
                if (tabsPrepAnal.SelectedTab == tabPrepAnalAnalysis)
                {
                    if (analysis.IsDirty())
                    {
                        btnPrepAnalAnalUpdate.ForeColor = Color.Green;
                        btnPrepAnalAnalDiscard.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnPrepAnalAnalUpdate.ForeColor = SystemColors.ControlText;
                        btnPrepAnalAnalDiscard.ForeColor = SystemColors.ControlText;
                    }
                }
                else if (tabsPrepAnal.SelectedTab == tabPrepAnalPreps)
                {
                    if (preparation.IsDirty())
                    {
                        btnPrepAnalPrepUpdate.ForeColor = Color.Green;
                        btnPrepAnalPrepDiscard.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnPrepAnalPrepUpdate.ForeColor = SystemColors.ControlText;
                        btnPrepAnalPrepDiscard.ForeColor = SystemColors.ControlText;
                    }
                }
                else if (tabsPrepAnal.SelectedTab == tabPrepAnalSample)
                {
                    if(sample.IsDirty())
                    {
                        btnPrepAnalSampleUpdate.ForeColor = Color.Green;
                        btnPrepAnalSampleDiscard.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnPrepAnalSampleUpdate.ForeColor = SystemColors.ControlText;
                        btnPrepAnalSampleDiscard.ForeColor = SystemColors.ControlText;
                    }
                }
            }
            else if (tabs.SelectedTab == tabOrder)
            {
                if (assignment.IsDirty())
                {
                    btnOrderSave.ForeColor = Color.Green;
                    btnOrderDiscard.ForeColor = Color.Red;
                }
                else
                {
                    btnOrderSave.ForeColor = SystemColors.ControlText;
                    btnOrderDiscard.ForeColor = SystemColors.ControlText;
                }
            }
            else if (tabs.SelectedTab == tabSample)
            {
                if (sample.IsDirty())
                {
                    btnSampleUpdate.ForeColor = Color.Green;
                    btnSampleDiscard.ForeColor = Color.Red;
                }
                else
                {
                    btnSampleUpdate.ForeColor = SystemColors.ControlText;
                    btnSampleDiscard.ForeColor = SystemColors.ControlText;
                }
            }
        }

        private bool InitializeUI()
        {
            try
            {
                cbMachineSettingsUseAD.Checked = Common.Settings.UseActiveDirectoryCredentials;

                using (SqlConnection conn = DB.OpenConnection())
                {
                    DB.LoadSampleTypes(conn, null);

                    cboxSamplesStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status");

                    cboxSampleInstanceStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_instance_status");

                    cboxPrepAnalPrepAmountUnit.DataSource = DB.GetIntLemmata(conn, null, "csp_select_preparation_units", true);

                    cboxPrepAnalPrepQuantityUnit.DataSource = DB.GetIntLemmata(conn, null, "csp_select_quantity_units", true);

                    cboxPrepAnalAnalWorkflowStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_workflow_status");

                    cboxPrepAnalPrepWorkflowStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_workflow_status");

                    cboxOrderStatus.DataSource = DB.GetIntLemmata(conn, null, "csp_select_workflow_status");

                    cboxSampleInfoLocationTypes.DataSource = DB.GetIntLemmata(conn, null, "csp_select_location_types", true);

                    cboxOrderRequestedSigma.DataSource = DB.GetSigmaValues(conn);
                    cboxOrderRequestedSigmaMDA.DataSource = DB.GetSigmaMDAValues(conn);

                    UI.PopulatePersons(conn, gridSysPers);

                    UI.PopulateCompanies(conn, gridMetaCompanies);

                    UI.PopulatePreparationUnits(conn, gridMetaUnitPrepUnits);

                    UI.PopulateActivityUnits(conn, gridMetaUnitActivityUnits);

                    UI.PopulateQuantityUnits(conn, gridMetaUnitQuantUnits);

                    UI.PopulateActivityUnitTypes(conn, gridMetaUnitActivityUnitTypes);

                    UI.PopulateComboBoxes(conn, "csp_select_activity_units_short", new SqlParameter[] { }, cboxPrepAnalAnalUnit);

                    UI.PopulateComboBoxes(conn, "csp_select_activity_unit_types", new SqlParameter[] { }, cboxPrepAnalAnalUnitType);

                    UI.PopulateProjectsMain(conn, gridProjectMain);

                    UI.PopulateComboBoxes(conn, "csp_select_projects_main_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSamplesProjects, cboxSampleProject);

                    UI.PopulateLaboratories(conn, InstanceStatus.Deleted, gridMetaLab);

                    UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleLaboratory, cboxOrderLaboratory);

                    UI.PopulateUsers(conn, InstanceStatus.Deleted, gridMetaUsers);

                    UI.PopulateNuclides(conn, gridSysNuclides);

                    UI.PopulateGeometries(conn, gridSysGeom);

                    UI.PopulateComboBoxes(conn, "csp_select_preparation_geometries_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxPrepAnalPrepGeom);

                    UI.PopulateCounties(conn, gridSysCounty);

                    UI.PopulateComboBoxes(conn, "csp_select_counties_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleCounties);

                    UI.PopulateStations(conn, gridMetaStation);

                    UI.PopulateComboBoxes(conn, "csp_select_stations_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleInfoStations);

                    UI.PopulateSampleStorage(conn, gridMetaSampleStorage);

                    UI.PopulateComboBoxes(conn, "csp_select_sample_storages_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleSampleStorage);

                    UI.PopulateSamplers(conn, gridMetaSamplers);

                    UI.PopulateComboBoxes(conn, "csp_select_samplers_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleInfoSampler);

                    UI.PopulateSamplingMethods(conn, gridMetaSamplingMeth);

                    UI.PopulateComboBoxes(conn, "csp_select_sampling_methods_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSampleInfoSamplingMeth);

                    UI.PopulatePreparationMethods(conn, gridTypeRelPrepMeth);

                    UI.PopulateAnalysisMethods(conn, gridTypeRelAnalMeth);

                    UI.PopulateSampleTypes(conn, treeSampleTypes);

                    UI.PopulateSampleTypes(treeSampleTypes, cboxSampleSampleType);

                    UI.PopulateCustomers(conn, InstanceStatus.Deleted, gridCustomers);

                    UI.PopulateOrderYears(conn, cboxOrdersYear);

                    UI.PopulateOrderWorkflowStatus(conn, cboxOrdersWorkflowStatus);

                    UI.PopulateComboBoxes(conn, "csp_select_assignments_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSamplesOrders);

                    UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                        new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                    }, cboxSamplesLaboratory, cboxOrdersLaboratory);
                }

                cboxOrdersTop.DataSource = DB.GetTopValues();
                cboxOrdersTop.SelectedValue = 50;

                cboxSamplesTop.DataSource = DB.GetTopValues();
                cboxSamplesTop.SelectedValue = 50;

                if (Common.LabId != Guid.Empty)
                {
                    cboxOrdersLaboratory.SelectedValue = Common.LabId;
                    cboxSamplesLaboratory.SelectedValue = Common.LabId;
                }                    
                
                cboxSamplesStatus.SelectedValue = InstanceStatus.Active;
                cboxOrdersWorkflowStatus.SelectedValue = WorkflowStatus.Construction;
                
                PopulateOrders();
                btnOrdersSearch.ForeColor = SystemColors.ControlText;

                PopulateSamples();
                btnSamplesSearch.ForeColor = SystemColors.ControlText;

                panelSampleLatLonAlt_Resize(null, null);

                HideMenuItems();
                SetMenuItemVisibilities();

                ActiveControl = tbMenuLookup;
                
                Common.Log.Info("Application initialized successfully");

                return true;
            }
            catch (Exception ex)
            {
                Common.Log.Fatal(ex);
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        private bool DiscardUnsavedChanges()
        {
            if (tabs.SelectedTab == tabPrepAnal)
            {
                if (analysis.IsDirty())
                {
                    DialogResult r = MessageBox.Show("Changes to the current analysis will be discarded. Do you want to continue?", "Warning", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                        return false;
                }

                if (preparation.IsDirty())
                {
                    DialogResult r = MessageBox.Show("Changes to the current preparation will be discarded. Do you want to continue?", "Warning", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                        return false;
                }

                gridPrepAnalResults.DataSource = null;
                preparation.ClearDirty();
                analysis.ClearDirty();
            }
            else if (tabs.SelectedTab == tabOrder)
            {
                if (assignment.IsDirty())
                {
                    DialogResult r = MessageBox.Show("Changes to the current assignment will be discarded. Do you want to continue?", "Warning", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                        return false;
                }

                assignment.ClearDirty();
            }

            return true;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {            
            try
            {
                if(!DiscardUnsavedChanges())
                {
                    e.Cancel = true;
                    return;
                }

                Common.Log.Info("Application closing down");

                if (!String.IsNullOrEmpty(DB.ConnectionString))
                {
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        DB.UnlockSamples(conn);
                        DB.UnlockOrders(conn);
                    }
                }

                SaveSettings(DSAEnvironment.SettingsFilename);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Common.Log.Error(ex);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Common.Log.Info("Application closed successfully");
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            //
        }

        private void SetStatusMessage(string msg, StatusMessageType msgLevel = StatusMessageType.Success)
        {
            switch(msgLevel)
            {
                case StatusMessageType.Success:
                    lblStatus.Text = Utils.makeStatusMessage(msg);
                    lblStatus.ForeColor = SystemColors.ControlText;
                    break;
                case StatusMessageType.Warning:
                    lblStatus.Text = Utils.makeStatusMessage(msg);
                    lblStatus.ForeColor = Color.OrangeRed;
                    break;
                case StatusMessageType.Error:
                    lblStatus.Text = Utils.makeErrorMessage(msg);
                    lblStatus.ForeColor = Color.Red;
                    break;
            }

            statusMessageTimer.Stop();
            statusMessageTimer.Start();
        }

        private void HideMenuItems()
        {
            miSample.Visible = miOrder.Visible = miSearch.Visible = miMeta.Visible = miOrders.Visible 
                = miSamples.Visible = miProjects.Visible = miCustomers.Visible = miTypeRelations.Visible 
                = miAuditLog.Visible = miSys.Visible = false;
        }

        private void HideMetaMenuItems()
        {
            miStations.Visible = false;
            miSampleStorage.Visible = false;
            miUnits.Visible = false;
            miSamplers.Visible = false;
            miSamplingMethods.Visible = false;            
        }

        private void HideSysMenuItems()
        {
            miLaboratories.Visible = false;
            miUsers.Visible = false;
            miNuclides.Visible = false;
            miMunicipalities.Visible = false;
            miAccreditationRules.Visible = false;
            miGeometries.Visible = false;
        }        

        private void miLogout_Click(object sender, EventArgs e)
        {
            if (!DiscardUnsavedChanges())            
                return;

            bool initialized = false;
            while (!initialized)
            {
                HideMenuItems();
                ShowLogin();
                initialized = InitializeUI();
            }
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ShowLogin()
        {
            if (tabs.SelectedTab != tabMenu)
                tabs.SelectedTab = tabMenu;

            Common.UserId = Guid.Empty;
            Common.Username = String.Empty;
            Common.LabId = Guid.Empty;
            Common.LabLogo = null;
            Common.LabAccredLogo = null;
            Roles.UserRoles.Clear();

            lblCurrentUser.Text = "";

            FormLogin formLogin = new FormLogin(Common.Settings);
            if (formLogin.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
            }

            DB.ConnectionString = Common.Settings.ConnectionString;

            Common.UserId = formLogin.UserId;
            Common.Username = formLogin.UserName;
            Common.LabId = formLogin.LabId;

            lblCurrentUser.Text = Common.Username;

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.LoadUserRoles(conn, null, Common.UserId, ref Roles.UserRoles);

                if(Common.LabId != Guid.Empty)
                {
                    using (SqlDataReader reader = DB.GetDataReader(conn, null, "select laboratory_logo, accredited_logo from laboratory where id = @id", 
                        CommandType.Text, new SqlParameter("@id", Common.LabId)))
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();

                            if(!reader.IsDBNull(0))
                                Common.LabLogo = Image.FromStream(new MemoryStream((byte[])reader["laboratory_logo"]));
                            if (!reader.IsDBNull(1))
                                Common.LabAccredLogo = Image.FromStream(new MemoryStream((byte[])reader["accredited_logo"]));
                        }
                    }
                }
            }
        }

        public void SaveSettings(string settingsFilename)
        {
            try
            {
                // Serialize settings to file
                using (StreamWriter sw = new StreamWriter(settingsFilename))
                {
                    XmlSerializer x = new XmlSerializer(Common.Settings.GetType());
                    x.Serialize(sw, Common.Settings);
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message, ex);
            }
        }

        public void LoadSettings(string settingsFilename)
        {
            try
            {
                if (!File.Exists(settingsFilename))
                    SaveSettings(settingsFilename);

                // Deserialize settings from file
                using (StreamReader sr = new StreamReader(settingsFilename))
                {
                    XmlSerializer x = new XmlSerializer(Common.Settings.GetType());
                    Common.Settings = x.Deserialize(sr) as DSASettings;
                }
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex.Message, ex);
            }
        }        

        private void SetLanguageLabels(ResourceManager r)
        {
            lblMenuSamples.Text = r.GetString(lblMenuSamples.Name);
            lblMenuOrders.Text = r.GetString(lblMenuOrders.Name);
            btnMenuNewSample.Text = r.GetString(btnMenuNewSample.Name);
            miFile.Text = r.GetString(miFile.Name);
        }        

        private void SetMenuItemVisibilities()
        {
            bool isAdmin = Roles.IsAdmin();
            miSearchView.Enabled = miProjectsView.Enabled = miCustomersView.Enabled = miTypeRelationsView.Enabled = miMetadataView.Enabled = miSystemDataView.Enabled = miAuditLogView.Enabled = isAdmin;
            btnMenuNewSample.Enabled = btnMenuSamples.Enabled = btnMenuNewOrder.Enabled = btnOrders.Enabled = btnMenuCustomer.Enabled = btnMenuProjects.Enabled = btnMenuMetadata.Enabled = btnMenuSearch.Enabled = isAdmin;

            miTypeRelSampleTypesNewRoot.Enabled = btnTypeRelSampleTypesNewRoot.Enabled = miTypeRelSampleTypesNew.Enabled = btnTypeRelSampleTypesNew.Enabled = miTypeRelSampleTypesEdit.Enabled = btnTypeRelSampleTypesEdit.Enabled = miTypeRelSampleTypesDelete.Enabled = btnTypeRelSampleTypesDelete.Enabled = isAdmin;
            miNewLaboratory.Enabled = miEditLaboratory.Enabled = miDeleteLaboratory.Enabled = btnMetaLabNew.Enabled = btnMetaLabEdit.Enabled = btnMetaLabDelete.Enabled = isAdmin;
            miNewUser.Enabled = miEditUser.Enabled = miDeleteUser.Enabled = btnMetaUsersNew.Enabled = btnMetaUsersEdit.Enabled = btnMetaUsersDelete.Enabled = isAdmin;
            miNewCounty.Enabled = miEditCounty.Enabled = miDeleteCounty.Enabled = miNewMunicipality.Enabled = miEditMunicipality.Enabled = miDeleteMunicipality.Enabled = isAdmin;
            btnNewCounty.Enabled = btnEditCounty.Enabled = btnDeleteCounty.Enabled = btnNewMunicipality.Enabled = btnEditMunicipality.Enabled = btnDeleteMunicipality.Enabled = isAdmin;
            miTypeRelSampleTypesCompNew.Enabled = miTypeRelSampleTypesCompEdit.Enabled = btnTypeRelSampTypeCompAdd.Enabled = btnTypeRelSampTypeCompEdit.Enabled = btnTypeRelSampTypeCompDelete.Enabled = isAdmin;
            miPreparationMethodsNew.Enabled = miPreparationMethodEdit.Enabled = miPreparationMethodDelete.Enabled = btnTypeRelSampTypePrepMethAdd.Enabled = btnPreparationMethodDelete.Enabled = isAdmin;
            miSamplesSetOrder.Enabled = btnSamplesSetOrder.Enabled = btnSampleAddSampleToOrder.Enabled = isAdmin;
            miSamplesPrepAnal.Enabled = btnSamplesPrepAnal.Enabled = btnSampleGoToPrepAnal.Enabled = isAdmin;
            miSamplesUnlock.Enabled = btnSamplesUnlock.Visible = isAdmin;
            miOrdersUnlock.Enabled = btnOrdersUnlock.Visible = isAdmin;

            cboxOrderStatus.Enabled = cbOrderApprovedLaboratory.Enabled = Roles.HasAccess(Role.LaboratoryAdministrator);

            // FIXME: Accreditation rules

            if (Roles.HasAccess(Role.LaboratoryAdministrator))
            {                
                miSearchView.Enabled = miProjectsView.Enabled = miCustomersView.Enabled = miTypeRelationsView.Enabled = miMetadataView.Enabled = miSystemDataView.Enabled = miAuditLogView.Enabled = true;
                btnMenuNewSample.Enabled = btnMenuSamples.Enabled = btnMenuNewOrder.Enabled = btnOrders.Enabled = btnMenuCustomer.Enabled = btnMenuProjects.Enabled = btnMenuMetadata.Enabled = btnMenuSearch.Enabled = true;
                miPreparationMethodsNew.Enabled = miPreparationMethodEdit.Enabled = miPreparationMethodDelete.Enabled = btnTypeRelSampTypePrepMethAdd.Enabled = btnPreparationMethodDelete.Enabled = true;
                miSamplesSetOrder.Enabled = btnSamplesSetOrder.Enabled = btnSampleAddSampleToOrder.Enabled = true;
                miSamplesPrepAnal.Enabled = btnSamplesPrepAnal.Enabled = btnSampleGoToPrepAnal.Enabled = true;
            }

            if (Roles.HasAccess(Role.LaboratoryOperator))
            {
                miMetadataView.Visible = true;
                btnMenuSamples.Enabled = btnMenuNewSample.Enabled = true;
                miSearchView.Enabled = btnMenuSearch.Enabled = true;
                miSamplesSetOrder.Enabled = btnSamplesSetOrder.Enabled = btnSampleAddSampleToOrder.Enabled = true;
                miSamplesPrepAnal.Enabled = btnSamplesPrepAnal.Enabled = btnSampleGoToPrepAnal.Enabled = true;
            }

            if (Roles.HasAccess(Role.SampleRegistration))
            {
                btnMenuSamples.Enabled = btnMenuNewSample.Enabled = true;
            }

            if (Roles.HasAccess(Role.OrderAdministrator))
            {
                btnMenuNewOrder.Enabled = btnOrders.Enabled = true;
            }

            if (Roles.HasAccess(Role.OrderOperator))
            {
                btnMenuNewOrder.Enabled = btnOrders.Enabled = true;
            }
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblCurrentTab.Text = tabs.SelectedTab.Text;            

            HideMenuItems();

            if (tabs.SelectedTab == tabMenu)
            {
                tbMenuLookup.Text = "";
                ActiveControl = tbMenuLookup;                
            }
            else if (tabs.SelectedTab == tabSample)
            {
                miSample.Visible = true;
            }
            else if (tabs.SelectedTab == tabOrder)
            {
                miOrder.Visible = true;                
            }
            else if (tabs.SelectedTab == tabSearch)
            {
                miSearch.Visible = true;
            }            
            else if(tabs.SelectedTab == tabMetadata)
            {                
                miMeta.Visible = true;
                tabsMeta_SelectedIndexChanged(sender, e);
            }
            else if (tabs.SelectedTab == tabOrders)
            {
                miOrders.Visible = true;
            }
            else if (tabs.SelectedTab == tabSamples)
            {
                miSamples.Visible = true;
                tbSamplesLookup.Text = "";
                ActiveControl = tbSamplesLookup;
            }            
            else if (tabs.SelectedTab == tabProjects)
            {
                miProjects.Visible = true;
            }
            else if (tabs.SelectedTab == tabCustomers)
            {
                miCustomers.Visible = true;
            }
            else if (tabs.SelectedTab == tabTypeRel)
            {
                miTypeRelations.Visible = true;
            }
            else if (tabs.SelectedTab == tabAuditLog)
            {
                miAuditLog.Visible = true;
            }            
            else if (tabs.SelectedTab == tabSysdata)
            {
                miSys.Visible = true;
                tabsSys_SelectedIndexChanged(sender, e);
            }
        }

        private void btnSamples_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabSamples;
        }                

        private void btnOrders_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabOrders;
        }                                                        

        private void tabsSample_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }

        private void treeSampleTypes_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            TreeNode tnode = e.Node;

            lblTypeRelSampCompSel.Text = lblTypeRelSampParSel.Text = lblTypeRelSampPrepSel.Text = tnode.Text;

            try
            {
                lbSampleTypesComponents.Items.Clear();
                lbSampleTypesInheritedComponents.Items.Clear();

                Guid sampleTypeId = Guid.Parse(tnode.Name);

                using (SqlConnection conn = DB.OpenConnection())
                {
                    // add sample components
                    AddSampleTypeComponents(conn, sampleTypeId, false, tnode);

                    // add preparation methods
                    UI.PopulateSampleTypePrepMeth(conn, tnode, lbTypeRelSampTypePrepMeth, lbTypeRelSampTypeInheritedPrepMeth);
                }
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
            }            
        }        

        private void AddSampleTypeComponents(SqlConnection conn, Guid sampleTypeId, bool inherited, TreeNode tnode)
        {
            ListBox lb = inherited ? lbSampleTypesInheritedComponents : lbSampleTypesComponents;

            using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_sample_components_for_sample_type", CommandType.StoredProcedure,
                    new SqlParameter("@sample_type_id", sampleTypeId)))
            {
                while (reader.Read())
                    lb.Items.Add(new Lemma<Guid, string>(Guid.Parse(reader["id"].ToString()), reader["name"].ToString()));
            }

            if (tnode.Parent != null)
            {
                Guid parentId = Guid.Parse(tnode.Parent.Name);
                AddSampleTypeComponents(conn, parentId, true, tnode.Parent);
            }
        }

        private void miUserSettings_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabUserSettings;
        }

        private void miMachineSettings_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabMachineSettings;
        }

        private void btnMachineSettingsSave_Click(object sender, EventArgs e)
        {
            Common.Settings.UseActiveDirectoryCredentials = cbMachineSettingsUseAD.Checked;

            SaveSettings(DSAEnvironment.SettingsFilename);
            SetStatusMessage("Settings saved");
        }

        private void btnUserSettingsSave_Click(object sender, EventArgs e)
        {
            //
        }                        

        private void miNewLaboratory_Click(object sender, EventArgs e)
        {            
            FormLaboratory form = new FormLaboratory();
            switch(form.ShowDialog())
            {                
                case DialogResult.OK:
                    SetStatusMessage("Laboratory " + form.LaboratoryName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateLaboratories(conn, InstanceStatus.Deleted, gridMetaLab);

                        UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Active)
                        }, cboxSampleLaboratory, cboxOrderLaboratory);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create laboratory failed", StatusMessageType.Error);
                    break;
            }                            
        }

        private void miDeleteLaboratory_Click(object sender, EventArgs e)
        {
            // delete laboratory
        }

        private void tabsMeta_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideMetaMenuItems();

            if(tabsMeta.SelectedTab == tabSysLaboratories)
            {
                miLaboratories.Visible = true;
            }
            else if (tabsMeta.SelectedTab == tabSysUsers)
            {
                miUsers.Visible = true;
            }
        }

        private void tabsSys_SelectedIndexChanged(object sender, EventArgs e)
        {            
            HideSysMenuItems();

            if (tabsSys.SelectedTab == tabSysLaboratories)
            {
                miLaboratories.Visible = true;
            }
            else if (tabsSys.SelectedTab == tabSysUsers)
            {
                miUsers.Visible = true;
            }
        }

        private void miNewUser_Click(object sender, EventArgs e)
        {
            // New user

            if(!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create laboratory users");
                return;
            }

            FormUser form = new FormUser();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateUsers(conn, InstanceStatus.Deleted, gridMetaUsers);
            }
        }

        private void miEditUser_Click(object sender, EventArgs e)
        {
            // edit user

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit laboratory users");
                return;
            }

            if (gridMetaUsers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a user first");
                return;
            }

            Guid uid = Guid.Parse(gridMetaUsers.SelectedRows[0].Cells["id"].Value.ToString());

            FormUser form = new FormUser(uid);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateUsers(conn, InstanceStatus.Deleted, gridMetaUsers);
            }
        }        

        private void miDeleteUser_Click(object sender, EventArgs e)
        {
            // Delete user
        }

        private void miResetPass_Click(object sender, EventArgs e)
        {
            if(gridMetaUsers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a user first");
                return;
            }

            Guid userId = Guid.Parse(gridMetaUsers.SelectedRows[0].Cells["id"].Value.ToString());
            string username = gridMetaUsers.SelectedRows[0].Cells["username"].Value.ToString();
            FormResetPassword form = new FormResetPassword(userId, username);
            if(form.ShowDialog() == DialogResult.OK)
            {
                SetStatusMessage("Password updated for user " + username, StatusMessageType.Success);
            }
        }

        private void miEditLaboratory_Click(object sender, EventArgs e)
        {
            // edit lab
            if (gridMetaLab.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridMetaLab.SelectedRows[0];
            Guid lid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormLaboratory form = new FormLaboratory(lid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Laboratory " + form.LaboratoryName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {                        
                        UI.PopulateLaboratories(conn, InstanceStatus.Deleted, gridMetaLab);
                        UI.PopulateComboBoxes(conn, "csp_select_laboratories_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Active)
                        }, cboxSampleLaboratory, cboxOrderLaboratory);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update laboratory failed", StatusMessageType.Error);                    
                    break;
            }                        
        }        

        private void miProjectsNew_Click(object sender, EventArgs e)
        {
            // new main project

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create projects");
                return;
            }

            FormProject form = new FormProject();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Main project " + form.ProjectName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateProjectsMain(conn, gridProjectMain);

                        UI.PopulateComboBoxes(conn, "csp_select_projects_main_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSamplesProjects, cboxSampleProject);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create main project failed", StatusMessageType.Error);
                    break;
            }                        
        }        

        private void miProjectsEdit_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit projects");
                return;
            }

            if (gridProjectMain.SelectedRows.Count < 1)
                return;

            Guid pmid = Guid.Parse(gridProjectMain.SelectedRows[0].Cells["id"].Value.ToString());

            FormProject form = new FormProject(pmid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Project " + form.ProjectName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateProjectsMain(conn, gridProjectMain);

                        UI.PopulateComboBoxes(conn, "csp_select_projects_main_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSamplesProjects, cboxSampleProject);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Edit main project failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miProjectsDelete_Click(object sender, EventArgs e)
        {
            // delete project

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to delete projects");
                return;
            }
        }

        private void miProjectsSubNew_Click(object sender, EventArgs e)
        {
            // new sub project

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create projects");
                return;
            }

            if (gridProjectMain.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a main project first");
                return;
            }

            Guid pmid = Guid.Parse(gridProjectMain.SelectedRows[0].Cells["id"].Value.ToString());
            string pmname = gridProjectMain.SelectedRows[0].Cells["name"].Value.ToString();
            
            FormProjectSub form = new FormProjectSub(pmname, pmid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sub project " + form.ProjectSubName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateProjectsSub(conn, pmid, gridProjectSub);

                        UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                            new SqlParameter("@project_main_id", pmid),
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleSubProject);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create main project failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miProjectsSubEdit_Click(object sender, EventArgs e)
        {
            // edit sub project

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit projects");
                return;
            }

            if (gridProjectMain.SelectedRows.Count < 1)
                return;

            if (gridProjectSub.SelectedRows.Count < 1)
                return;

            Guid pmid = Guid.Parse(gridProjectMain.SelectedRows[0].Cells["id"].Value.ToString());
            string pmname = gridProjectMain.SelectedRows[0].Cells["name"].Value.ToString();
            Guid psid = Guid.Parse(gridProjectSub.SelectedRows[0].Cells["id"].Value.ToString());

            FormProjectSub form = new FormProjectSub(pmname, pmid, psid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Project " + form.ProjectSubName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateProjectsSub(conn, pmid, gridProjectSub);

                        UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                            new SqlParameter("@project_main_id", pmid),
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleSubProject);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Edit sub project failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miProjectsSubDelete_Click(object sender, EventArgs e)
        {
            // delete sub project

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to delete projects");
                return;
            }
        }

        private void miNuclidesNew_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create nuclides");
                return;
            }

            FormNuclide form = new FormNuclide();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Nuclide " + form.NuclideName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                        UI.PopulateNuclides(conn, gridSysNuclides);
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create nuclide failed", StatusMessageType.Error);
                    break;
            }            
        }        

        private void miSampleTypesNew_Click(object sender, EventArgs e)
        {
            // New sample type

            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }

            FormSampleType form = new FormSampleType(treeSampleTypes.SelectedNode, false);

            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample type " + form.SampleTypeName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        DB.LoadSampleTypes(conn, null);
                        UI.PopulateSampleTypes(conn, treeSampleTypes);
                        UI.PopulateSampleTypes(treeSampleTypes, cboxSampleSampleType);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create sample type failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSampleTypesEdit_Click(object sender, EventArgs e)
        {
            // Edit sample type

            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }

            if (treeSampleTypes.SelectedNode == null)
            {
                MessageBox.Show("You must select a parent sample type");
                return;
            }

            FormSampleType form = new FormSampleType(treeSampleTypes.SelectedNode, true);

            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample type " + form.SampleTypeName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        DB.LoadSampleTypes(conn, null);
                        UI.PopulateSampleTypes(conn, treeSampleTypes);
                        UI.PopulateSampleTypes(treeSampleTypes, cboxSampleSampleType);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update sample type failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSampleTypesDelete_Click(object sender, EventArgs e)
        {
            // Delete sample type

            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }
        }

        private void miSystemDataView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabSysdata;
        }

        private void miLogView_Click(object sender, EventArgs e)
        {
            string tempFileName = Path.GetTempFileName() + ".txt";
            File.Copy(DSALogger.LogFile, tempFileName, true);
            Process.Start(tempFileName);
        }

        private void miAuditLogView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabAuditLog;
        }

        private void miSearchView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabSearch;
        }

        private void miTypeRelationsView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabTypeRel;
        }

        private void miMetadataView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabMetadata;
        }

        private void miCustomersView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabMetadata;
            tabsMeta.SelectedTab = tabCustomers;
        }

        private void miProjectsView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabProjects;
        }

        private void miMainMenuView_Click(object sender, EventArgs e)
        {
            tabs.SelectedTab = tabMenu;
        }

        private void miNuclidesEdit_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit nuclides");
                return;
            }

            if (gridSysNuclides.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridSysNuclides.SelectedRows[0];            
            Guid nid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormNuclide form = new FormNuclide(nid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Nuclide " + form.NuclideName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                        UI.PopulateNuclides(conn, gridSysNuclides);
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update nuclide failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miNewGeometry_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create preparation geometries");
                return;
            }

            FormGeometry form = new FormGeometry();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Geometry " + form.GeometryName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateGeometries(conn, gridSysGeom);                        
                        UI.PopulateComboBoxes(conn, "csp_select_preparation_geometries_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxPrepAnalPrepGeom);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create geometry failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miEditGeometry_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit preparation geometries");
                return;
            }

            if (gridSysGeom.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridSysGeom.SelectedRows[0];
            Guid gid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormGeometry form = new FormGeometry(gid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Geometry " + form.GeometryName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                        UI.PopulateGeometries(conn, gridSysGeom);
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update geometry failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miDeleteGeometry_Click(object sender, EventArgs e)
        {
            // delete geom

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to delete preparation geometries");
                return;
            }
        }

        private void miNewCounty_Click(object sender, EventArgs e)
        {            
            FormCounty form = new FormCounty();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("County " + form.CountyName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCounties(conn, gridSysCounty);

                        UI.PopulateComboBoxes(conn, "csp_select_counties_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleCounties);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create county failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miEditCounty_Click(object sender, EventArgs e)
        {        
            if (gridSysCounty.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridSysCounty.SelectedRows[0];
            Guid cid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormCounty form = new FormCounty(cid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("County " + form.CountyName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCounties(conn, gridSysCounty);

                        UI.PopulateComboBoxes(conn, "csp_select_counties_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Active)
                        }, cboxSampleCounties);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create county failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miDeleteCounty_Click(object sender, EventArgs e)
        {
            // delete county
        }

        private void miNewMunicipality_Click(object sender, EventArgs e)
        {        
            if (gridSysCounty.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridSysCounty.SelectedRows[0];
            Guid cid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormMunicipality form = new FormMunicipality(cid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Municipality " + form.MunicipalityName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                        UI.PopulateMunicipalities(conn, cid, gridSysMunicipality);
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create municipality failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miEditMunicipality_Click(object sender, EventArgs e)
        {        
            if (gridSysCounty.SelectedRows.Count < 1)
                return;

            if (gridSysMunicipality.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridSysCounty.SelectedRows[0];
            Guid cid = Guid.Parse(row.Cells["id"].Value.ToString());            

            row = gridSysMunicipality.SelectedRows[0];
            Guid mid = Guid.Parse(row.Cells["id"].Value.ToString());            

            FormMunicipality form = new FormMunicipality(cid, mid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Municipality " + form.MunicipalityName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                        UI.PopulateMunicipalities(conn, cid, gridSysMunicipality);
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update municipality failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miDeleteMunicipality_Click(object sender, EventArgs e)
        {
            // delete municipality
        }        

        private void miNewStation_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to create stations");
                return;
            }

            FormStation form = new FormStation();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Station " + form.StationName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateStations(conn, gridMetaStation);                        
                        UI.PopulateComboBoxes(conn, "csp_select_stations_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoStations);
                    }                        
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create station failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miEditStation_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to edit stations");
                return;
            }

            if (gridMetaStation.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridMetaStation.SelectedRows[0];
            Guid sid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormStation form = new FormStation(sid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Station " + form.StationName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateStations(conn, gridMetaStation);                        
                        UI.PopulateComboBoxes(conn, "csp_select_stations_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoStations);
                    }                        
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update station failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miDeleteStation_Click(object sender, EventArgs e)
        {
            // delete station

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to delete stations");
                return;
            }
        }

        private void miNewSampleStorage_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create sample storage");
                return;
            }

            FormSampleStorage form = new FormSampleStorage();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample storage " + form.SampleStorageName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSampleStorage(conn, gridMetaSampleStorage);

                        UI.PopulateComboBoxes(conn, "csp_select_sample_storages_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Active)
                        }, cboxSampleSampleStorage);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create sample storage failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miEditSampleStorage_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit sample storage");
                return;
            }

            if (gridMetaSampleStorage.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridMetaSampleStorage.SelectedRows[0];
            Guid ssid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormSampleStorage form = new FormSampleStorage(ssid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample storage " + form.SampleStorageName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSampleStorage(conn, gridMetaSampleStorage);

                        UI.PopulateComboBoxes(conn, "csp_select_sample_storages_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Active)
                        }, cboxSampleSampleStorage);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update sample storage failed", StatusMessageType.Error);
                    break;
            }                        
        }

        private void miDeleteSampleStorage_Click(object sender, EventArgs e)
        {
            // delete sample storage

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create delete storage");
                return;
            }
        }

        private void miSamplerNew_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to create samplers");
                return;
            }

            FormSampler form = new FormSampler();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sampler " + form.SamplerName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSamplers(conn, gridMetaSamplers);

                        UI.PopulateComboBoxes(conn, "csp_select_samplers_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoSampler);
                    }                        
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create sampler failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSamplerEdit_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to edit samplers");
                return;
            }

            if (gridMetaSamplers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a sampler first");
                return;
            }

            DataGridViewRow row = gridMetaSamplers.SelectedRows[0];
            Guid sid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormSampler form = new FormSampler(sid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sampler " + form.SamplerName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSamplers(conn, gridMetaSamplers);                        

                        UI.PopulateComboBoxes(conn, "csp_select_samplers_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoSampler);
                    }                        
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update sampler failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSamplerDelete_Click(object sender, EventArgs e)
        {
            // delete sampler
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to delete samplers");
                return;
            }
        }

        private void cboxSampleSampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleSampleType.SelectedValue))
            {
                cboxSampleSampleComponent.DataSource = null;
                return;
            }

            Guid sampleTypeId = Guid.Parse(cboxSampleSampleType.SelectedValue.ToString());
            TreeNode[] tnodes = treeSampleTypes.Nodes.Find(sampleTypeId.ToString(), true);
            if (tnodes.Length < 1)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateSampleComponentsAscending(conn, sampleTypeId, tnodes[0], cboxSampleSampleComponent);
            }
        }        

        private void cboxSampleSampleType_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cboxSampleSampleType.Text.Trim()))
            {                
                cboxSampleSampleType.SelectedItem = Guid.Empty;
                cboxSampleSampleComponent.DataSource = null;
                return;
            }

            if(!Utils.IsValidGuid(cboxSampleSampleType.SelectedValue))
            {
                cboxSampleSampleType.SelectedValue = Guid.Empty;
                cboxSampleSampleComponent.DataSource = null;
                SetStatusMessage("You must select an existing sample type", StatusMessageType.Warning);
            }
        }

        private void btnSampleSelectSampleType_Click(object sender, EventArgs e)
        {
            FormSelectSampleType form = new FormSelectSampleType();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            cboxSampleSampleType.SelectedValue = form.SelectedSampleTypeId;
        }

        private void cboxSampleProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleProject.SelectedValue))
            {
                lblSampleToolProject.Text = "";
                lblSampleToolSubProject.Text = "";
                cboxSampleSubProject.DataSource = null;
                return;
            }

            Guid projectId = Guid.Parse(cboxSampleProject.SelectedValue.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                    new SqlParameter("@project_main_id", projectId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                }, cboxSampleSubProject);
            }

            lblSampleToolProject.Text = "[Project] " + cboxSampleProject.Text;
            lblSampleToolSubProject.Text = "";
        }

        private void cboxSampleSubProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleSubProject.SelectedValue))
            {
                lblSampleToolSubProject.Text = "";
                return;
            }

            lblSampleToolSubProject.Text = cboxSampleSubProject.Text;
        }

        private void cboxSampleInfoStations_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleInfoStations.SelectedValue))
            {
                tbSampleInfoLatitude.Text = "";
                tbSampleInfoLongitude.Text = "";
                tbSampleInfoAltitude.Text = "";
                return;
            }

            Guid stationId = Guid.Parse(cboxSampleInfoStations.SelectedValue.ToString());

            using (SqlConnection conn = DB.OpenConnection())
            {
                using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_station", CommandType.StoredProcedure, 
                    new SqlParameter("@id", stationId)))
                {
                    reader.Read();
                    tbSampleInfoLatitude.Text = reader["latitude"].ToString();
                    tbSampleInfoLongitude.Text = reader["longitude"].ToString();
                    tbSampleInfoAltitude.Text = reader["altitude"].ToString();
                }
            }                
        }

        private void tbSampleExId_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (String.IsNullOrEmpty(tbSampleExId.Text.Trim()))
            {
                lblSampleToolExId.Text = "";
                return;
            }

            lblSampleToolExId.Text = "[Ex.Id] " + tbSampleExId.Text.Trim();
        }

        private void cboxSampleCounties_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleCounties.SelectedValue))
            {
                cboxSampleMunicipalities.DataSource = null;
                return;
            }

            Guid countyId = Guid.Parse(cboxSampleCounties.SelectedValue.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateComboBoxes(conn, "csp_select_municipalities_for_county_short", new[] {
                    new SqlParameter("@county_id", countyId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Active)
                }, cboxSampleMunicipalities);
            }
        }        

        private void miSamplesNew_Click(object sender, EventArgs e)
        {
            // new sample                        
            FormSampleNew form = new FormSampleNew(treeSampleTypes);
            if (form.ShowDialog() != DialogResult.OK)
                return;            

            using (SqlConnection conn = DB.OpenConnection())
            {
                sample.LoadFromDB(conn, null, form.SampleId);

                PopulateSample(conn, null, sample, true);
            }

            tabs.SelectedTab = tabSample;

            btnSamplesSearch.ForeColor = Color.Red;

            SetStatusMessage("Sample " + form.SampleNumber + " created successfully");
        }

        private void miSamplesImportExcel_Click(object sender, EventArgs e)
        {
            // Import sample from excel
        }

        private void miSamplesEdit_Click(object sender, EventArgs e)
        {
            // edit sample

            if(gridSamples.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select a single sample first");
                return;
            }

            Guid sid = Guid.Parse(gridSamples.SelectedRows[0].Cells["id"].Value.ToString());

            using (SqlConnection conn = DB.OpenConnection())
            {
                sample.LoadFromDB(conn, null, sid);

                if (Common.LabId == Guid.Empty)
                {                    
                    if(Common.Username.ToUpper() != sample.CreatedBy.ToUpper())
                    {
                        MessageBox.Show("Can not edit this sample. Sample does not belong to your user");
                        return;
                    }
                }
                else
                {
                    bool allow = false;
                    
                    if (Common.LabId == sample.GetAssignmentLaboratory(conn, null))
                    {
                        allow = true;
                    }
                    else
                    {                        
                        if (Common.Username.ToUpper() == sample.CreatedBy.ToUpper())
                            allow = true;
                    }

                    if (!allow)
                    {
                        MessageBox.Show("Can not edit this sample. Sample does not belong to you or your laboratory");
                        return;
                    }
                }
                
                PopulateSample(conn, null, sample, true);
            }

            tabsSample.SelectedTab = tabSamplesInfo;
            tabs.SelectedTab = tabSample;
        }

        private void PopulateSample(SqlConnection conn, SqlTransaction trans, Sample s, bool clearDirty)
        {
            cboxSampleSampleType.SelectedValue = s.SampleTypeId;
            cboxSampleSampleComponent.SelectedValue = s.SampleComponentId;
            cboxSampleInfoSampler.SelectedValue = s.SamplerId;
            cboxSampleInfoSamplingMeth.SelectedValue = s.SamplingMethodId;

            if (Utils.IsValidGuid(s.ProjectSubId))
            {
                object mpid = DB.GetScalar(conn, null, "select project_main_id from project_sub where id = @id", CommandType.Text, 
                    new SqlParameter("@id", s.ProjectSubId));
                cboxSampleProject.SelectedValue = mpid;
                cboxSampleSubProject.SelectedValue = s.ProjectSubId;
            }

            if (!Utils.IsValidGuid(s.StationId))
            {
                tbSampleInfoLatitude.Text = s.Latitude.ToString();
                tbSampleInfoLongitude.Text = s.Longitude.ToString();
                tbSampleInfoAltitude.Text = s.Altitude.ToString();
            }
            else cboxSampleInfoStations.SelectedValue = s.StationId;

            if (Utils.IsValidGuid(s.MunicipalityId))
            {
                object cid = DB.GetScalar(conn, null, "select county_id from municipality where id = @id", CommandType.Text, 
                    new SqlParameter("@id", s.MunicipalityId));
                cboxSampleCounties.SelectedValue = cid;
                cboxSampleMunicipalities.SelectedValue = s.MunicipalityId;
            }

            cboxSampleInfoLocationTypes.Text = s.LocationType;
            tbSampleLocation.Text = s.Location;

            cboxSampleLaboratory.SelectedValue = s.LaboratoryId;

            tbSampleSamplingDateFrom.TextChanged -= tbSampleSamplingDateFrom_TextChanged;
            tbSampleSamplingDateTo.TextChanged -= tbSampleSamplingDateTo_TextChanged;
            tbSampleReferenceDate.TextChanged -= tbSampleReferenceDate_TextChanged;

            if (s.SamplingDateFrom == DateTime.MinValue)
            {
                tbSampleSamplingDateFrom.Tag = null;
                tbSampleSamplingDateFrom.Text = "";                
            }   
            else
            {
                tbSampleSamplingDateFrom.Tag = s.SamplingDateFrom;
                tbSampleSamplingDateFrom.Text = s.SamplingDateFrom.ToString(Utils.DateTimeFormatNorwegian);                
            }            

            if (s.SamplingDateTo == DateTime.MinValue)
            {
                tbSampleSamplingDateTo.Tag = null;
                tbSampleSamplingDateTo.Text = "";                
            }
            else
            {
                tbSampleSamplingDateTo.Tag = s.SamplingDateTo;
                tbSampleSamplingDateTo.Text = s.SamplingDateTo.ToString(Utils.DateTimeFormatNorwegian);                
            }

            if (s.ReferenceDate == DateTime.MinValue)
            {
                tbSampleReferenceDate.Tag = null;
                tbSampleReferenceDate.Text = "";                
            }
            else
            {
                tbSampleReferenceDate.Tag = s.ReferenceDate;
                tbSampleReferenceDate.Text = s.ReferenceDate.ToString(Utils.DateTimeFormatNorwegian);                
            }

            tbSampleSamplingDateFrom.TextChanged += tbSampleSamplingDateFrom_TextChanged;
            tbSampleSamplingDateTo.TextChanged += tbSampleSamplingDateTo_TextChanged;
            tbSampleReferenceDate.TextChanged += tbSampleReferenceDate_TextChanged;

            tbSampleExId.Text = s.ExternalId;
            cbSampleConfidential.Checked = s.Confidential;

            cboxSampleSampleStorage.SelectedValue = s.SampleStorageId;

            cboxSampleInstanceStatus.SelectedValue = s.InstanceStatusId;

            tbSampleComment.Text = s.Comment;
            lblSampleToolId.Text = "[Sample] " + s.Number.ToString();
            lblSampleToolLaboratory.Text = String.IsNullOrEmpty(cboxSampleLaboratory.Text) ? "" : "[Laboratory] " + cboxSampleLaboratory.Text;

            // Show attachments
            UI.PopulateAttachments(conn, trans, "sample", s.Id, gridSampleAttachments);

            if(clearDirty)
            {
                sample.ClearDirty();
            }
        }

        private void miSamplesDelete_Click(object sender, EventArgs e)
        {
            // delete sample
        }

        private void miSamplesSplit_Click(object sender, EventArgs e)
        {
            // split sample
            if(gridSamples.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select a single sample to split");
                return;
            }

            Guid sid = Guid.Parse(gridSamples.SelectedRows[0].Cells["id"].Value.ToString());
            string comp = gridSamples.SelectedRows[0].Cells["sample_component_name"].Value.ToString();
            if(!String.IsNullOrEmpty(comp))
            {
                MessageBox.Show("Cannot split a sample which is already a component");
                return;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                int mergeTest = Convert.ToInt32(DB.GetScalar(conn, null, "select count(transform_to_id) from sample where id = '" + sid.ToString() + "'", CommandType.Text));
                if (mergeTest > 0)
                {
                    MessageBox.Show("Cannot split, sample has already been merged");
                    return;
                }
            }   
                                     
            FormSampleSplit form = new FormSampleSplit(sid, treeSampleTypes);
            if (form.ShowDialog() != DialogResult.OK)
                return;
            
            PopulateSamples();

            SetStatusMessage("Splitting sample successful");
        }

        private void miSamplesMerge_Click(object sender, EventArgs e)
        {
            // merge sample
            if (gridSamples.SelectedRows.Count < 2)
            {
                MessageBox.Show("You must select two or more samples to merge");
                return;
            }

            List<Guid> sampleIds = new List<Guid>();
            foreach(DataGridViewRow row in gridSamples.SelectedRows)            
                sampleIds.Add(Guid.Parse(row.Cells["id"].Value.ToString()));

            var sampleIdsArr = from item in sampleIds select "'" + item + "'";
            string sampleIdsCsv = string.Join(",", sampleIdsArr);

            using (SqlConnection conn = DB.OpenConnection())
            {
                int mergeTest = Convert.ToInt32(DB.GetScalar(conn, null, "select count(transform_to_id) from sample where id in (" + sampleIdsCsv + ")", CommandType.Text));
                if(mergeTest > 0)
                {
                    MessageBox.Show("Cannot merge, one or more of these samples has already been merged");
                    return;
                }

                Func<string, int> nCheck = field => Convert.ToInt32(DB.GetScalar(conn, null, "select count(distinct(" + field + ")) from sample where id in(" + sampleIdsCsv + ")", CommandType.Text));

                // FIXME: Must select new sample type
                if ((nCheck("laboratory_id") & nCheck("project_sub_id")) != 1)
                {
                    MessageBox.Show("All samples to be merged must have the same laboratory and project");
                    return;
                }
            }                

            FormSampleMerge form = new FormSampleMerge(sampleIdsCsv);
            if (form.ShowDialog() != DialogResult.OK)
                return;
            
            PopulateSamples();

            SetStatusMessage("Merging samples successful");
        }

        private void miSamplesSetOrder_Click(object sender, EventArgs e)
        {
            // add sample to order

            if (gridSamples.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select a single sample first");
                return;
            }                

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to add samples to orders");
                return;
            }            

            Guid sampleId = Guid.Parse(gridSamples.SelectedRows[0].Cells["id"].Value.ToString());
            string sampleName = gridSamples.SelectedRows[0].Cells["number"].Value.ToString();

            bool hasLock = false;
            using (SqlConnection conn = DB.OpenConnection())
            {
                hasLock = DB.LockSample(conn, sampleId);
            }

            if (!hasLock)
            {
                MessageBox.Show("Sample " + sampleName + " is already locked by someone else");
                return;
            }

            FormSelectOrder form = new FormSelectOrder(treeSampleTypes, sampleId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                SetStatusMessage("Successfully added sample " + sampleName + " to order " + form.SelectedOrderName);
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.UnlockSamples(conn);
            }
        }

        private void miSamplesSetProject_Click(object sender, EventArgs e)
        {
            // sample, set project
        }

        private void miSamplesSetCustomer_Click(object sender, EventArgs e)
        {
            // sample, set customer
        }

        private void miSamplesSetSampler_Click(object sender, EventArgs e)
        {
            // sample, set sampler
        }

        private void miSamplesSetSamplingMethod_Click(object sender, EventArgs e)
        {
            // sample, set sampling method
        }

        private void miSamplesPrepAnal_Click(object sender, EventArgs e)
        {
            // go to sample prep/anal

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to preparations and analyses");
                return;
            }
            
            if(gridSamples.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select a single sample first");
                return;
            }

            Guid sid = Guid.Parse(gridSamples.SelectedRows[0].Cells["id"].Value.ToString());            

            using (SqlConnection conn = DB.OpenConnection())
            {
                sample.LoadFromDB(conn, null, sid);

                if (!PopulatePrepAnal(conn, sample))
                    return;
            }

            tabs.SelectedTab = tabPrepAnal;
        }

        private bool PopulatePrepAnal(SqlConnection conn, Sample s)
        {
            if(!s.HasRequiredFields())
            {
                MessageBox.Show("This sample is not complete yet");
                return false;
            }

            treePrepAnal.Nodes.Clear();

            TreeNode sampleNode = null;
            Font fontSample = new Font(treePrepAnal.Font, FontStyle.Bold);            

            using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_sample_header", CommandType.StoredProcedure, 
                new SqlParameter("@id", s.Id)))
            {
                reader.Read();
                string txt = reader["sample_number"].ToString() + " - " + reader["sample_type_name"].ToString() + ", " + reader["laboratory_name"].ToString();
                sampleNode = treePrepAnal.Nodes.Add(sample.Id.ToString(), txt);
                sampleNode.NodeFont = fontSample;
            }
            
            using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_preparation_headers_for_sample", CommandType.StoredProcedure,
                new SqlParameter("@sample_id", s.Id)))
            {
                while (reader.Read())
                {
                    int status = Convert.ToInt32(reader["workflow_status_id"]);
                    string txt = reader["preparation_number"].ToString() + " - " + reader["preparation_method_name"].ToString();
                    if(!String.IsNullOrEmpty(reader["assignment_name"].ToString()))
                        txt += ", " + reader["assignment_name"].ToString();
                    TreeNode prepNode = sampleNode.Nodes.Add(reader["preparation_id"].ToString(), txt);
                    prepNode.ToolTipText = reader["preparation_method_name_full"].ToString();
                    prepNode.ForeColor = WorkflowStatus.GetStatusColor(status);
                }
            }
                
            foreach (TreeNode prepNode in sampleNode.Nodes)
            {
                Guid prepId = Guid.Parse(prepNode.Name);
                using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_analysis_headers_for_preparation", CommandType.StoredProcedure,
                    new SqlParameter("@preparation_id", prepId)))
                {
                    while (reader.Read())
                    {
                        int status = Convert.ToInt32(reader["workflow_status_id"]);
                        string txt = reader["analysis_number"].ToString() + " - " + reader["analysis_method_name"].ToString();
                        if(!String.IsNullOrEmpty(reader["assignment_name"].ToString()))
                            txt += ", " + reader["assignment_name"].ToString();
                        TreeNode analNode = prepNode.Nodes.Add(reader["analysis_id"].ToString(), txt);
                        analNode.ToolTipText = reader["analysis_method_name_full"].ToString();
                        analNode.ForeColor = WorkflowStatus.GetStatusColor(status);
                    }
                }
            }

            treePrepAnal.ExpandAll();
            
            tbPrepAnalInfoComment.Text = s.Comment;
            tbPrepAnalWetWeight.Text = s.WetWeight_g.ToString();
            tbPrepAnalDryWeight.Text = s.DryWeight_g.ToString();
            tbPrepAnalVolume.Text = s.Volume_l.ToString();
            tbPrepAnalLODStartWeight.Text = s.LodWeightStart.ToString();
            tbPrepAnalLODEndWeight.Text = s.LodWeightEnd.ToString();
            tbPrepAnalLODTemp.Text = s.LodTemperature.ToString();            

            preparation.ClearDirty();
            analysis.ClearDirty();

            tabsPrepAnal.SelectedTab = tabPrepAnalSample;
            return true;
        }

        private void miSamplesSetExcempt_Click(object sender, EventArgs e)
        {
            // set sample excempt from public
        }

        private void miSamplingMethodNew_Click(object sender, EventArgs e)
        {
            // new sampling method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to create sampling methods");
                return;
            }

            FormSamplingMeth form = new FormSamplingMeth();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sampling method " + form.SamplingMethodName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSamplingMethods(conn, gridMetaSamplingMeth);                        

                        UI.PopulateComboBoxes(conn, "csp_select_sampling_methods_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoSamplingMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create sampler failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSamplingMethodEdit_Click(object sender, EventArgs e)
        {
            // edit sampling method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to edit sampling methods");
                return;
            }

            if (gridMetaSamplingMeth.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridMetaSamplingMeth.SelectedRows[0];
            Guid smid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormSamplingMeth form = new FormSamplingMeth(smid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sampling method " + form.SamplingMethodName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSamplingMethods(conn, gridMetaSamplingMeth);
                                              
                        UI.PopulateComboBoxes(conn, "csp_select_sampling_methods_short", new[] {
                            new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                        }, cboxSampleInfoSamplingMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update sampling method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSamplingMethodDelete_Click(object sender, EventArgs e)
        {
            // delete sampling method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to delete sampling methods");
                return;
            }
        }

        private void cboxSamplesProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxSamplesProjects.SelectedValue))
            {
                cboxSamplesProjectsSub.DataSource = null;
                return;
            }

            Guid projectId = Guid.Parse(cboxSamplesProjects.SelectedValue.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {                
                UI.PopulateComboBoxes(conn, "csp_select_projects_sub_short", new[] {
                    new SqlParameter("@project_main_id", projectId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                }, cboxSamplesProjectsSub);
            }

            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void miPreparationMethodsNew_Click(object sender, EventArgs e)
        {
            // new preparation method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods");
                return;
            }

            FormPreparationMethod form = new FormPreparationMethod();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Preparation method " + form.PreparationMethodName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulatePreparationMethods(conn, gridTypeRelPrepMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create preparation method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miPreparationMethodEdit_Click(object sender, EventArgs e)
        {
            // edit preparation method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods");
                return;
            }

            if (gridTypeRelPrepMeth.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridTypeRelPrepMeth.SelectedRows[0];
            Guid pmid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormPreparationMethod form = new FormPreparationMethod(pmid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Preparation method " + form.PreparationMethodName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulatePreparationMethods(conn, gridTypeRelPrepMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update preparation method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miPreparationMethodDelete_Click(object sender, EventArgs e)
        {
            // delete preparation method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods");
                return;
            }
        }

        private void miAnalysisMethodsNew_Click(object sender, EventArgs e)
        {
            // new analysis method
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage analysis methods");
                return;
            }

            FormAnalysisMethods form = new FormAnalysisMethods();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Analysis method " + form.AnalysisMethodName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateAnalysisMethods(conn, gridTypeRelAnalMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create preparation method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miAnalysisMethodsEdit_Click(object sender, EventArgs e)
        {
            // edit analysis method
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage analysis methods");
                return;
            }

            if (gridTypeRelAnalMeth.SelectedRows.Count < 1)
                return;

            DataGridViewRow row = gridTypeRelAnalMeth.SelectedRows[0];
            Guid amid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormAnalysisMethods form = new FormAnalysisMethods(amid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Analysis method " + form.AnalysisMethodName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateAnalysisMethods(conn, gridTypeRelAnalMeth);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update analysis method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miAnalysisMethodsDelete_Click(object sender, EventArgs e)
        {
            // delete analysis method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage analysis methods");
                return;
            }
        }

        private void miAddPrepMethToSampType_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods for sample types");
                return;
            }

            if (treeSampleTypes.SelectedNode == null)
            {
                MessageBox.Show("You must select a sample type first");
                return;
            }
            
            List<Guid> methodsAbove = GetPreparationMethodsForSampleType(treeSampleTypes.SelectedNode, true);

            List<Guid> methodsBelow = new List<Guid>();
            GetPreparationMethodsBelowSampleType(treeSampleTypes.SelectedNode, methodsBelow);

            FormSampTypeXPrepMeth form = new FormSampTypeXPrepMeth(
                treeSampleTypes.SelectedNode.Text, 
                Guid.Parse(treeSampleTypes.SelectedNode.Name), 
                methodsAbove, 
                methodsBelow);

            if (form.ShowDialog() == DialogResult.Cancel)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateSampleTypePrepMeth(conn, treeSampleTypes.SelectedNode, lbTypeRelSampTypePrepMeth, lbTypeRelSampTypeInheritedPrepMeth);
            }
        }        

        private List<Guid> GetPreparationMethodsForSampleType(TreeNode tnode, bool ascend)
        {
            List<Guid> existingMethods = new List<Guid>();
            Guid sampleTypeId = Guid.Parse(tnode.Name);
            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("csp_select_preparation_methods_for_sample_type_short", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sample_type_id", DB.MakeParam(typeof(Guid), sampleTypeId));
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())                    
                        existingMethods.Add(Guid.Parse(reader["id"].ToString()));
                }

                if (ascend)
                {
                    while (tnode.Parent != null)
                    {
                        tnode = tnode.Parent;
                        sampleTypeId = Guid.Parse(tnode.Name);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@sample_type_id", DB.MakeParam(typeof(Guid), sampleTypeId));
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                                existingMethods.Add(Guid.Parse(reader["id"].ToString()));
                        }
                    }
                }
            }            

            return existingMethods;
        }

        private void GetPreparationMethodsBelowSampleType(TreeNode tnode, List<Guid> methods)
        {
            foreach (TreeNode tn in tnode.Nodes)
            {
                methods.AddRange(GetPreparationMethodsForSampleType(tn, false));
                GetPreparationMethodsBelowSampleType(tn, methods);
            }
        }

        private void miTypeRelSampleTypesCompNew_Click(object sender, EventArgs e)
        {
            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }

            if (treeSampleTypes.SelectedNode == null)
            {
                MessageBox.Show("You must select a sample type first");
                return;
            }

            // new sample component
            Guid sampleTypeId = Guid.Parse(treeSampleTypes.SelectedNode.Name);

            FormSampleComponent form = new FormSampleComponent(sampleTypeId, treeSampleTypes.SelectedNode.Text);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample component " + form.SampleComponentName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {                        
                        UI.PopulateSampleComponents(conn, sampleTypeId, lbSampleTypesComponents);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create preparation method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miTypeRelSampleTypesCompEdit_Click(object sender, EventArgs e)
        {
            // edit sample component

            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }

            if (treeSampleTypes.SelectedNode == null)
            {
                MessageBox.Show("You must select a sample type first");
                return;
            }

            if (lbSampleTypesComponents.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select a sample component first");
                return;
            }

            Guid sampleTypeId = Guid.Parse(treeSampleTypes.SelectedNode.Name);

            Lemma<Guid, string> sampleComponent = lbSampleTypesComponents.SelectedItems[0] as Lemma<Guid, string>;

            FormSampleComponent form = new FormSampleComponent(sampleTypeId, treeSampleTypes.SelectedNode.Text, sampleComponent.Id);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample component " + form.SampleComponentName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateSampleComponents(conn, sampleTypeId, lbSampleTypesComponents);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update preparation method failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miAnalysisMethodsAddNuclide_Click(object sender, EventArgs e)
        {
            // add nuclide to analysis method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage analysis methods");
                return;
            }

            if (gridTypeRelAnalMeth.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select an analysis type first");
                return;
            }

            Guid amid = Guid.Parse(gridTypeRelAnalMeth.SelectedRows[0].Cells["id"].Value.ToString());
            string amname = gridTypeRelAnalMeth.SelectedRows[0].Cells["name"].Value.ToString();

            List<Guid> existingNuclides = GetNuclidesForAnalysisType(amid);

            FormAnalMethXNuclide form = new FormAnalMethXNuclide(amname, amid, existingNuclides);

            if (form.ShowDialog() == DialogResult.Cancel)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateAnalMethNuclides(conn, amid, lbTypRelAnalMethNuclides);
            }
        }

        public List<Guid> GetNuclidesForAnalysisType(Guid amid)
        {
            List<Guid> existingNuclides = new List<Guid>();
            using (SqlConnection conn = DB.OpenConnection())
            {
                using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_nuclides_for_analysis_method", CommandType.StoredProcedure,
                    new SqlParameter("@analysis_method_id", amid)))
                {
                    while (reader.Read())
                        existingNuclides.Add(Guid.Parse(reader["id"].ToString()));
                }
            }

            return existingNuclides;
        }

        private void miAnalysisMethodsRemNuclide_Click(object sender, EventArgs e)
        {
            // remove nuclides from analysis method

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage analysis methods");
                return;
            }
        }        

        private void miTypeRelPrepMethAddAnalMeth_Click(object sender, EventArgs e)
        {
            // add analysis methods to preparation method
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods");
                return;
            }

            if (gridTypeRelPrepMeth.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a preparation method first");
                return;
            }

            Guid pmid = Guid.Parse(gridTypeRelPrepMeth.SelectedRows[0].Cells["id"].Value.ToString());
            string pmname = gridTypeRelPrepMeth.SelectedRows[0].Cells["name"].Value.ToString();

            List<Guid> existingAnalysisMethods = GetAnalysisMethodsForPreparationMethod(pmid);

            FormPrepMethXAnalMeth form = new FormPrepMethXAnalMeth(pmname, pmid, existingAnalysisMethods);

            if (form.ShowDialog() == DialogResult.Cancel)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulatePrepMethAnalMeths(conn, pmid, lbTypRelPrepMethAnalMeth);
            }
        }

        public List<Guid> GetAnalysisMethodsForPreparationMethod(Guid pmid)
        {
            List<Guid> existingAnalysisMethods = new List<Guid>();            

            using (SqlConnection conn = DB.OpenConnection())
            {
                using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_analysis_methods_for_preparation_method", CommandType.StoredProcedure,
                    new SqlParameter("@preparation_method_id", pmid)))
                {
                    while (reader.Read())
                        existingAnalysisMethods.Add(Guid.Parse(reader["id"].ToString()));
                }
            }

            return existingAnalysisMethods;
        }

        private void miTypeRelPrepMethRemAnalMeth_Click(object sender, EventArgs e)
        {
            // remove analysis methods to preparation method
            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to manage preparation methods");
                return;
            }
        }                        

        private void miOrdersNew_Click(object sender, EventArgs e)
        {
            // create new order            
            FormOrderNew form = new FormOrderNew();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            assignment = new Assignment();            

            using (SqlConnection conn = DB.OpenConnection())
            {
                assignment.LoadFromDB(conn, null, form.OrderId);
                tbOrderName.Text = form.OrderName;
                PopulateOrder(conn, null, assignment, true);

                UI.PopulateComboBoxes(conn, "csp_select_assignments_short", new[] {
                    new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                }, cboxSamplesOrders);

                UI.PopulateOrderYears(conn, cboxOrdersYear);
            }

            tabs.SelectedTab = tabOrder;
        }

        private void miOrdersEdit_Click(object sender, EventArgs e)
        {
            // edit existing order
            if(gridOrders.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select an order first");
                return;
            }

            Guid orderId = Guid.Parse(gridOrders.SelectedRows[0].Cells["id"].Value.ToString());            

            using (SqlConnection conn = DB.OpenConnection())
            {
                assignment.LoadFromDB(conn, null, orderId);
                
                if(assignment.LaboratoryId != Common.LabId)
                {
                    MessageBox.Show("You can not edit this order. Order does not belong to your laboratory");
                    return;
                }

                PopulateOrder(conn, null, assignment, true);
            }            

            tabsOrder.SelectedTab = tabOrderInfo;
            tabs.SelectedTab = tabOrder;
        }

        private void miOrdersDelete_Click(object sender, EventArgs e)
        {
            // delete order
        }        

        private void PopulateOrder(SqlConnection conn, SqlTransaction trans, Assignment a, bool clearDirty)
        {
            tbOrderName.Text = a.Name;
            cboxOrderLaboratory.SelectedValue = a.LaboratoryId;
            cboxOrderResponsible.SelectedValue = a.AccountId;
            tbOrderDeadline.Tag = a.Deadline;
            tbOrderDeadline.Text = a.Deadline.ToString(Utils.DateFormatNorwegian);            
            cboxOrderRequestedSigma.SelectedValue = a.RequestedSigmaAct;
            cboxOrderRequestedSigmaMDA.SelectedValue = a.RequestedSigmaMDA;
            tbOrderCustomer.Text = a.CustomerContactName;
            tbOrderCustomerInfo.Text =
                a.CustomerContactName + Environment.NewLine +
                a.CustomerCompanyName + Environment.NewLine + Environment.NewLine +
                a.CustomerContactEmail + Environment.NewLine +
                a.CustomerContactPhone + Environment.NewLine + Environment.NewLine +
                a.CustomerContactAddress;
            tbOrderContentComment.Text = a.ContentComment;
            tbOrderReportComment.Text = a.ReportComment;
            cbOrderApprovedCustomer.Checked = a.ApprovedCustomer;
            tbOrderApprovedCustomerBy.Text = a.ApprovedCustomerBy;
            cbOrderApprovedLaboratory.Checked = a.ApprovedLaboratory;
            tbOrderApprovedLaboratoryBy.Text = a.ApprovedLaboratoryBy;
            cboxOrderStatus.SelectedValue = a.WorkflowStatusId;
            tbOrderLastWorkflowStatusBy.Text = a.LastWorkflowStatusBy;

            PopulateOrderContent(conn, trans, a);

            // Show attachments
            UI.PopulateAttachments(conn, null, "assignment", a.Id, gridOrderAttachments);

            // Populate assigned tree

            Font fontSample = new Font(tvOrderContent.Font, FontStyle.Bold);
                
            tvOrderContent.Nodes.Clear();

            TreeNode root = tvOrderContent.Nodes.Add(a.Name);
            root.NodeFont = fontSample;
            
            using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_sample_headers_for_assignment", CommandType.StoredProcedure, 
                new SqlParameter("@assignment_id", a.Id)))
            {
                while (reader.Read())
                {
                    string label = "Sample " + reader["sample_number"].ToString() + ", " + reader["sample_type_name"].ToString() + " " + reader["sample_component_name"].ToString();
                    TreeNode sNode = root.Nodes.Add(reader["sample_id"].ToString(), label);
                    sNode.NodeFont = fontSample;
                    if (DB.IsValidField(reader["sample_comment"]))
                        sNode.ToolTipText = reader["sample_comment"].ToString();
                }
            }

            foreach(TreeNode tnode in root.Nodes)
            {
                Guid sid = Guid.Parse(tnode.Name);
                using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_preparation_headers_for_sample2", CommandType.StoredProcedure,
                    new SqlParameter("@sample_id", sid)))
                {
                    while (reader.Read())
                    {
                        int status = Convert.ToInt32(reader["workflow_status_id"]);
                        string label = "Preparation " + reader["preparation_number"].ToString() + ", " + reader["preparation_method_name"].ToString() + ", " + reader["workflow_status_name"].ToString();
                        TreeNode pNode = tnode.Nodes.Add(reader["preparation_id"].ToString(), label);
                        pNode.ForeColor = WorkflowStatus.GetStatusColor(status);
                        if (DB.IsValidField(reader["preparation_comment"]))
                            pNode.ToolTipText = reader["preparation_comment"].ToString();
                    }
                }
            }

            foreach (TreeNode tnode in root.Nodes)
            {
                foreach (TreeNode tn in tnode.Nodes)
                {
                    Guid pid = Guid.Parse(tn.Name);
                    using (SqlDataReader reader = DB.GetDataReader(conn, null, "csp_select_analysis_headers_for_preparation_assignment", CommandType.StoredProcedure, new[] {
                        new SqlParameter("@preparation_id", pid),
                        new SqlParameter("@assignment_id", a.Id)
                    }))
                    {
                        while (reader.Read())
                        {
                            int status = Convert.ToInt32(reader["workflow_status_id"]);
                            string label = "Analysis " + reader["analysis_number"].ToString() + ", " + reader["analysis_method_name"].ToString() + ", " + reader["workflow_status_name"].ToString();
                            TreeNode aNode = tn.Nodes.Add(reader["analysis_id"].ToString(), label);
                            aNode.ForeColor = WorkflowStatus.GetStatusColor(status);
                            if (DB.IsValidField(reader["analysis_comment"]))
                                aNode.ToolTipText = reader["analysis_comment"].ToString();
                        }
                    }
                }
            }

            tvOrderContent.ExpandAll();

            if(clearDirty)
            {
                assignment.Dirty = false;
                foreach (AssignmentSampleType ast in a.SampleTypes)
                {
                    ast.Dirty = false;
                    foreach (AssignmentPreparationMethod apm in ast.PreparationMethods)
                    {
                        apm.Dirty = false;
                        foreach (AssignmentAnalysisMethod aam in apm.AnalysisMethods)                 
                            aam.Dirty = false;
                    }
                }
            }
        }

        private void PopulateOrderContent(SqlConnection conn, SqlTransaction trans, Assignment a)
        {
            treeOrderContent.Nodes.Clear();

            foreach (AssignmentSampleType ast in a.SampleTypes)
            {
                string txt = ast.SampleCount.ToString() + ", " + ast.SampleTypeName(conn, trans);
                if (Utils.IsValidGuid(ast.SampleComponentId))
                    txt += ", " + ast.SampleComponentName(conn, trans);
                TreeNode[] nodes = treeSampleTypes.Nodes.Find(ast.SampleTypeId.ToString(), true);
                if (nodes.Length > 0)
                    txt += " -> " + nodes[0].FullPath;
                if (ast.ReturnToSender)
                    txt += ", Return to customer";

                TreeNode tnode = treeOrderContent.Nodes.Add(ast.Id.ToString(), txt);
                tnode.Tag = ast;
                tnode.ToolTipText = ast.Comment;
                tnode.NodeFont = new Font(treeOrderContent.Font.FontFamily, treeOrderContent.Font.Size, FontStyle.Bold);

                foreach (AssignmentPreparationMethod apm in ast.PreparationMethods)
                {
                    txt = apm.PreparationMethodCount.ToString() + ", " + apm.PreparationMethodName(conn, trans);
                    if (Utils.IsValidGuid(apm.PreparationLaboratoryId))
                        txt += " (" + apm.PreparationLaboratoryName(conn, trans) + ")";

                    TreeNode tn = tnode.Nodes.Add(apm.Id.ToString(), txt);
                    tn.Tag = apm;
                    tn.ToolTipText = apm.PreparationMethodNameFull(conn, trans) + Environment.NewLine + Environment.NewLine + apm.Comment;

                    foreach (AssignmentAnalysisMethod aam in apm.AnalysisMethods)
                    {
                        txt = aam.AnalysisMethodCount.ToString() + ", " + aam.AnalysisMethodName(conn, trans);
                        TreeNode tn2 = tn.Nodes.Add(aam.Id.ToString(), txt);
                        tn2.Tag = aam;
                        tn2.ToolTipText = aam.AnalysisMethodNameFull(conn, trans) + Environment.NewLine + Environment.NewLine + aam.Comment;
                    }
                }
            }

            treeOrderContent.ExpandAll();
        }

        private void miOrderAddSampleType_Click(object sender, EventArgs e)
        {
            // add sample type to order
            if (assignment.WorkflowStatusId == WorkflowStatus.Complete)
            {
                MessageBox.Show("This order has been closed and can not be updated");
                return;
            }
                            
            if (assignment.ApprovedLaboratory || assignment.ApprovedCustomer)
            {
                MessageBox.Show("This order has been approved and can not be updated");
                return;
            }

            FormOrderAddSampleType form = new FormOrderAddSampleType(assignment, treeSampleTypes);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateOrderContent(conn, null, assignment);
            }
        }

        private void miOrderRemSampleType_Click(object sender, EventArgs e)
        {
            // remove sample type from order
        }

        private void miOrderAddPrepMeth_Click(object sender, EventArgs e)
        {
            // add preparation method to order
            if (treeOrderContent.SelectedNode == null)
            {
                MessageBox.Show("You must select a sample type first");
                return;
            }

            TreeNode tnode = treeOrderContent.SelectedNode;
            if(tnode.Level != 0)
            {
                MessageBox.Show("You must select a sample type first");
                return;
            }

            AssignmentSampleType ast = tnode.Tag as AssignmentSampleType;
            
            if (assignment.WorkflowStatusId == WorkflowStatus.Complete)
            {
                MessageBox.Show("This order has been closed and can not be updated");
                return;
            }

            if (assignment.ApprovedLaboratory || assignment.ApprovedCustomer)
            {
                MessageBox.Show("This order has been approved and can not be updated");
                return;
            }
            
            FormOrderAddPrepMeth form = new FormOrderAddPrepMeth(ast);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateOrderContent(conn, null, assignment);
            }
        }

        private void miOrderRemPrepMeth_Click(object sender, EventArgs e)
        {
            // remove preparation method from order
        }

        private void miOrderAddAnalMeth_Click(object sender, EventArgs e)
        {
            // add analysis method to order
            if (treeOrderContent.SelectedNode == null)
            {
                MessageBox.Show("You must select a preparation method first");
                return;
            }

            TreeNode tnode = treeOrderContent.SelectedNode;

            if (tnode.Level != 1)
            {
                MessageBox.Show("You must select a preparation method first");
                return;
            }

            AssignmentPreparationMethod apm = tnode.Tag as AssignmentPreparationMethod;

            if (assignment.WorkflowStatusId == WorkflowStatus.Complete)
            {
                MessageBox.Show("This order has been closed and can not be updated");
                return;
            }

            if (assignment.ApprovedLaboratory || assignment.ApprovedCustomer)
            {
                MessageBox.Show("This order has been approved and can not be updated");
                return;
            }
            
            FormOrderAddAnalMeth form = new FormOrderAddAnalMeth(apm);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateOrderContent(conn, null, assignment);
            }
        }

        private void miOrderRemAnalMeth_Click(object sender, EventArgs e)
        {
            // remove analysis method from order
        }

        private void miOrderSave_Click(object sender, EventArgs e)
        {
            // save order            
            if(!assignment.IsDirty())
            {
                SetStatusMessage("Nothing to save for order " + assignment.Name);
                return;
            }

            if (!Utils.IsValidGuid(cboxOrderResponsible.SelectedValue))
            {
                MessageBox.Show("Responsible is mandatory");
                return;
            }

            if (tbOrderDeadline.Tag == null)
            {
                MessageBox.Show("Deadline is mandatory");
                return;
            }

            DateTime deadline = (DateTime)tbOrderDeadline.Tag;

            if (deadline < DateTime.Now)
            {
                MessageBox.Show("Deadline can not be in the past");
                return;
            }

            if (String.IsNullOrEmpty(tbOrderCustomer.Text))
            {
                MessageBox.Show("Customer is mandatory");
                return;
            }

            if (assignment.ApprovedLaboratory != cbOrderApprovedLaboratory.Checked)
            {
                if (!Roles.HasAccess(Role.LaboratoryAdministrator))
                {
                    MessageBox.Show("You are not allowed to approve orders");
                    return;
                }
            }

            if (cbOrderApprovedCustomer.Checked && !cbOrderApprovedLaboratory.Checked)
            {
                MessageBox.Show("Laboratory must approve this order before customer");
                return;
            }

            if (!Utils.IsValidGuid(cboxOrderLaboratory.SelectedValue))
            {
                MessageBox.Show("Laboratory is mandatory");
                return;
            }

            SqlConnection conn = null;
            SqlTransaction trans = null;

            try
            {
                conn = DB.OpenConnection();
                trans = conn.BeginTransaction();

                int wfStatus = Convert.ToInt32(cboxOrderStatus.SelectedValue);

                if (wfStatus == WorkflowStatus.Complete)
                {
                    int nCurrSamples, nCurrPreparations, nCurrAnalyses;
                    DB.GetOrderCurrentInventory(conn, trans, assignment.Id, out nCurrSamples, out nCurrPreparations, out nCurrAnalyses);

                    int nReqSamples, nReqPreparations, nReqAnalyses;
                    DB.GetOrderRequiredInventory(conn, trans, assignment.Id, out nReqSamples, out nReqPreparations, out nReqAnalyses);

                    if(nCurrSamples < nReqSamples)
                    {
                        MessageBox.Show("Can not set this order to complete. Connected samples " + nCurrSamples + " of " + nReqSamples);
                        return;
                    }

                    if (nCurrPreparations < nReqPreparations)
                    {
                        MessageBox.Show("Can not set this order to complete. Connected preparations " + nCurrPreparations + " of " + nReqPreparations);
                        return;
                    }

                    if (nCurrAnalyses < nReqAnalyses)
                    {
                        MessageBox.Show("Can not set this order to complete. Connected analyses " + nCurrAnalyses + " of " + nReqAnalyses);
                        return;
                    }
                }

                assignment.LaboratoryId = Guid.Parse(cboxOrderLaboratory.SelectedValue.ToString());
                assignment.AccountId = Guid.Parse(cboxOrderResponsible.SelectedValue.ToString());
                assignment.Deadline = deadline;
                assignment.RequestedSigmaAct = Convert.ToDouble(cboxOrderRequestedSigma.SelectedValue);
                assignment.RequestedSigmaMDA = Convert.ToDouble(cboxOrderRequestedSigmaMDA.SelectedValue);
                assignment.ContentComment = tbOrderContentComment.Text.Trim();
                assignment.ApprovedLaboratory = cbOrderApprovedLaboratory.Checked;
                assignment.ApprovedCustomer = cbOrderApprovedCustomer.Checked;
                assignment.ReportComment = tbOrderReportComment.Text.Trim();
                assignment.WorkflowStatusId = wfStatus;                           

                assignment.StoreToDB(conn, trans);

                trans.Commit();

                btnOrdersSearch_Click(sender, e);

                SetStatusMessage("Order " + assignment.Name + " updated");
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
            }
        }

        private void btnOrderSelectDeadline_Click(object sender, EventArgs e)
        {
            FormSelectDate form = new FormSelectDate();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            DateTime selectedDate = form.SelectedDate;
            tbOrderDeadline.Tag = selectedDate;
            tbOrderDeadline.Text = selectedDate.ToString(Utils.DateFormatNorwegian);

            assignment.Dirty = true;
        }

        private void miCustomersNew_Click(object sender, EventArgs e)
        {
            // create new customer            

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to create customers");
                return;
            }

            FormCustomer form = new FormCustomer();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Customer created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCustomers(conn, InstanceStatus.Deleted, gridCustomers);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create customer failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miCustomersEdit_Click(object sender, EventArgs e)
        {
            // edit customer

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to edit customers");
                return;
            }

            if (gridCustomers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a customer first");
                return;
            }

            DataGridViewRow row = gridCustomers.SelectedRows[0];
            Guid cid = Guid.Parse(row.Cells["id"].Value.ToString());

            FormCustomer form = new FormCustomer(cid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Customer updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCustomers(conn, InstanceStatus.Deleted, gridCustomers);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update customer failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miCustomersDelete_Click(object sender, EventArgs e)
        {
            // delete customer

            if (!Roles.HasAccess(Role.LaboratoryAdministrator))
            {
                MessageBox.Show("You don't have access to delete customers");
                return;
            }
        }        

        private void gridSysCounty_SelectionChanged(object sender, EventArgs e)
        {
            if (gridSysCounty.SelectedRows.Count < 1)
                return;

            Guid cid = Guid.Parse(gridSysCounty.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
                UI.PopulateMunicipalities(conn, cid, gridSysMunicipality);
        }

        private void gridProjectMain_SelectionChanged(object sender, EventArgs e)
        {
            if (gridProjectMain.SelectedRows.Count < 1)
                return;

            Guid pmid = Guid.Parse(gridProjectMain.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateProjectsSub(conn, pmid, gridProjectSub);
            }
        }

        private void gridTypeRelAnalMeth_SelectionChanged(object sender, EventArgs e)
        {
            if (gridTypeRelAnalMeth.SelectedRows.Count < 1)
                return;

            Guid amid = Guid.Parse(gridTypeRelAnalMeth.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateAnalMethNuclides(conn, amid, lbTypRelAnalMethNuclides);
            }
        }

        private void gridTypeRelPrepMeth_SelectionChanged(object sender, EventArgs e)
        {
            if (gridTypeRelPrepMeth.SelectedRows.Count < 1)
                return;

            Guid pmid = Guid.Parse(gridTypeRelPrepMeth.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulatePrepMethAnalMeths(conn, pmid, lbTypRelPrepMethAnalMeth);
            }
        }

        private void cboxOrderLaboratory_SelectedIndexChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;

            if (!Utils.IsValidGuid(cboxOrderLaboratory.SelectedValue))
            {            
                cboxOrderResponsible.SelectedValue = Guid.Empty;
                return;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateComboBoxes(conn, "csp_select_accounts_for_laboratory_short", new[] {
                    new SqlParameter("@laboratory_id", assignment.LaboratoryId),
                    new SqlParameter("@instance_status_level", InstanceStatus.Deleted)
                }, cboxOrderResponsible);
            }
        }

        private void treePrepAnal_AfterSelect(object sender, TreeViewEventArgs e)
        {
            btnPrepAnalAddAnal.Enabled = false;
            gridPrepAnalResults.DataSource = null;
            preparation.ClearDirty();
            analysis.ClearDirty();

            using (SqlConnection conn = DB.OpenConnection())
            {
                Guid sid, pid;
                switch (e.Node.Level)
                {
                    case 0:                        
                        sid = Guid.Parse(e.Node.Name);
                        sample.LoadFromDB(conn, null, sid);
                        PopulateSampleInfo(conn, null, sample, e.Node, true);
                        tabsPrepAnal.SelectedTab = tabPrepAnalSample;
                        break;
                    case 1:
                        sid = Guid.Parse(e.Node.Parent.Name);
                        sample.LoadFromDB(conn, null, sid);
                        pid = Guid.Parse(e.Node.Name);
                        preparation.LoadFromDB(conn, null, pid);
                        PopulatePreparation(conn, null, preparation, true);
                        btnPrepAnalAddAnal.Enabled = true;
                        tabsPrepAnal.SelectedTab = tabPrepAnalPreps;
                        break;
                    case 2:
                        sid = Guid.Parse(e.Node.Parent.Parent.Name);
                        sample.LoadFromDB(conn, null, sid);
                        pid = Guid.Parse(e.Node.Parent.Name);
                        preparation.LoadFromDB(conn, null, pid);
                        Guid aid = Guid.Parse(e.Node.Name);
                        analysis.LoadFromDB(conn, null, aid);
                        PopulateAnalysis(conn, null, analysis, true);
                        tabsPrepAnal.SelectedTab = tabPrepAnalAnalysis;
                        break;
                }
            }
        }

        private void TreeDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e.Node == null)
                return;
            
            var selected = (e.State & TreeNodeStates.Selected) == TreeNodeStates.Selected;
            var unfocused = !e.Node.TreeView.Focused;
            
            if (selected && unfocused)
            {
                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, e.Bounds, SystemColors.HighlightText, TextFormatFlags.GlyphOverhangPadding);
            }
            else e.DrawDefault = true;            
        }

        private void treePrepAnal_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            TreeDrawNode(e);
        }

        private void treeSampleTypes_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            TreeDrawNode(e);
        }

        private void tabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tabSample || e.TabPage == tabPrepAnal)
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    if (!DB.LockSample(conn, sample.Id))
                    {
                        MessageBox.Show("Unable to lock sample");
                        e.Cancel = true;
                    }
                }
            }
            else if (e.TabPage == tabOrder)
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    if (!DB.LockOrder(conn, assignment.Id))
                    {
                        MessageBox.Show("Unable to lock order");
                        e.Cancel = true;
                    }
                }
            }            
        }

        private void tabs_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (!DiscardUnsavedChanges())
            {
                e.Cancel = true;
                return;
            }
            
            if (e.TabPage == tabSample || e.TabPage == tabPrepAnal)
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    DB.UnlockSamples(conn);
                }                    
            }
            else if (e.TabPage == tabOrder)
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    DB.UnlockOrders(conn);
                }
            }
        }

        private void miBack_Click(object sender, EventArgs e)
        {
            if (tabs.SelectedTab == tabSample || tabs.SelectedTab == tabPrepAnal)
                tabs.SelectedTab = tabSamples;
            else if (tabs.SelectedTab == tabOrder)
                tabs.SelectedTab = tabOrders;
            else tabs.SelectedTab = tabMenu;
        }

        private void btnSampleUpdate_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(cboxSampleLaboratory.SelectedValue))
            {
                MessageBox.Show("Laboratory is mandatory");
                return;
            }

            if (!Utils.IsValidGuid(cboxSampleSampleType.SelectedValue))
            {
                MessageBox.Show("Sample type is mandatory");
                return;
            }

            if (!Utils.IsValidGuid(cboxSampleProject.SelectedValue))
            {
                MessageBox.Show("Main project is mandatory");
                return;
            }

            if (!Utils.IsValidGuid(cboxSampleSubProject.SelectedValue))
            {
                MessageBox.Show("Sub project is mandatory");
                return;
            }            

            double lat = -1d, lon = -1d, alt = -1d;

            try
            {
                if (!String.IsNullOrEmpty(tbSampleInfoLatitude.Text.Trim()))
                    lat = Utils.GetLatitude(tbSampleInfoLatitude.Text.Trim());

                if (!String.IsNullOrEmpty(tbSampleInfoLongitude.Text.Trim()))
                    lon = Utils.GetLongitude(tbSampleInfoLongitude.Text.Trim());                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            if (!String.IsNullOrEmpty(tbSampleInfoAltitude.Text))
                alt = Convert.ToDouble(tbSampleInfoAltitude.Text);

            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                Guid newSampleTypeId = Utils.MakeGuid(cboxSampleSampleType.SelectedValue);
                if (sample.SampleTypeId != newSampleTypeId && sample.HasOrders(conn, null))
                {
                    MessageBox.Show("Can not change sample type. This sample belongs to one or more orders");
                    return;
                }

                sample.LaboratoryId = Utils.MakeGuid(cboxSampleLaboratory.SelectedValue);
                sample.SampleTypeId = Utils.MakeGuid(cboxSampleSampleType.SelectedValue);
                sample.SampleStorageId = Utils.MakeGuid(cboxSampleSampleStorage.SelectedValue);
                sample.SampleComponentId = Utils.MakeGuid(cboxSampleSampleComponent.SelectedValue);
                sample.ProjectSubId = Utils.MakeGuid(cboxSampleSubProject.SelectedValue);
                sample.StationId = Utils.MakeGuid(cboxSampleInfoStations.SelectedValue);
                sample.SamplerId = Utils.MakeGuid(cboxSampleInfoSampler.SelectedValue);
                sample.SamplingMethodId = Utils.MakeGuid(cboxSampleInfoSamplingMeth.SelectedValue);
                sample.MunicipalityId = Utils.MakeGuid(cboxSampleMunicipalities.SelectedValue);
                sample.LocationType = cboxSampleInfoLocationTypes.Text;
                sample.Location = tbSampleLocation.Text;
                sample.Latitude = lat;
                sample.Longitude = lon;
                sample.Altitude = alt;
                if(tbSampleSamplingDateFrom.Tag != null)
                    sample.SamplingDateFrom = (DateTime)tbSampleSamplingDateFrom.Tag;
                if(tbSampleSamplingDateTo.Tag != null)
                    sample.SamplingDateTo = (DateTime)tbSampleSamplingDateTo.Tag;
                if(tbSampleReferenceDate.Tag != null)
                    sample.ReferenceDate = (DateTime)tbSampleReferenceDate.Tag;
                sample.ExternalId = tbSampleExId.Text;
                sample.Confidential = cbSampleConfidential.Checked;
                sample.InstanceStatusId = Convert.ToInt32(cboxSampleInstanceStatus.SelectedValue);
                sample.Comment = tbSampleComment.Text.Trim();

                sample.StoreToDB(conn, null);

                SetStatusMessage("Sample " + sample.Number + " updated");
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

        private void panelSampleLatLonAlt_Resize(object sender, EventArgs e)
        {
            int w = panelSampleLatLonAlt.Width;
            tbSampleInfoLatitude.Width = w / 3;
            tbSampleInfoAltitude.Width = w / 3;
        }

        private void btnPrepAnalPrepUpdate_Click(object sender, EventArgs e)
        {
            if (!preparation.IsDirty())
            {
                SetStatusMessage("Nothing to save for preparation " + preparation.Number);
                return;
            }

            SqlConnection conn = null;
            SqlTransaction trans = null;

            try
            {
                conn = DB.OpenConnection();
                trans = conn.BeginTransaction();

                if(preparation.IsClosed(conn, trans))
                {
                    MessageBox.Show("This preparation belongs to a closed order and can not be updated");
                    return;
                }

                Guid pgid = Guid.Parse(cboxPrepAnalPrepGeom.SelectedValue.ToString());
                if (!String.IsNullOrEmpty(tbPrepAnalPrepFillHeight.Text) && Utils.IsValidGuid(pgid))
                {
                    double fh = Convert.ToDouble(tbPrepAnalPrepFillHeight.Text);
                    PreparationGeometry pg = new PreparationGeometry(conn, trans, pgid);
                    if(fh < pg.MinFillHeightMM || fh > pg.MaxFillHeightMM)
                    {
                        MessageBox.Show("Fill height " + fh + " is outside valid range. Valid range is [" + pg.MinFillHeightMM + ", " + pg.MaxFillHeightMM + "]");
                        return;
                    }
                }

                preparation.PreparationGeometryId = pgid;
                preparation.FillHeightMM = String.IsNullOrEmpty(tbPrepAnalPrepFillHeight.Text) ? 0d : Convert.ToDouble(tbPrepAnalPrepFillHeight.Text);
                preparation.Amount = String.IsNullOrEmpty(tbPrepAnalPrepAmount.Text) ? 0d : Convert.ToDouble(tbPrepAnalPrepAmount.Text);
                preparation.PrepUnitId = Convert.ToInt32(cboxPrepAnalPrepAmountUnit.SelectedValue);
                preparation.Quantity = String.IsNullOrEmpty(tbPrepAnalPrepQuantity.Text) ? 0d : Convert.ToDouble(tbPrepAnalPrepQuantity.Text);
                preparation.QuantityUnitId = Convert.ToInt32(cboxPrepAnalPrepQuantityUnit.SelectedValue);
                preparation.Comment = tbPrepAnalPrepComment.Text.Trim();
                preparation.WorkflowStatusId = Convert.ToInt32(cboxPrepAnalPrepWorkflowStatus.SelectedValue);

                preparation.StoreToDB(conn, trans);

                trans.Commit();

                treePrepAnal.SelectedNode.ForeColor = WorkflowStatus.GetStatusColor(preparation.WorkflowStatusId);
                SetStatusMessage("Preparation updated successfully");
            }
            catch(Exception ex)
            {
                trans?.Rollback();
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn?.Close();
            }
        }

        private void PopulatePreparation(SqlConnection conn, SqlTransaction trans, Preparation p, bool clearDirty)
        {
            lblPrepAnalPrepRange.Text = "";
            cboxPrepAnalPrepGeom.SelectedValue = p.PreparationGeometryId;
            tbPrepAnalPrepFillHeight.Text = p.FillHeightMM == 0d ? "" : p.FillHeightMM.ToString();
            tbPrepAnalPrepAmount.Text = p.Amount == 0d ? "" : p.Amount.ToString();
            cboxPrepAnalPrepAmountUnit.SelectedValue = p.PrepUnitId;
            tbPrepAnalPrepQuantity.Text = p.Quantity == 0d ? "" : p.Quantity.ToString();
            cboxPrepAnalPrepQuantityUnit.SelectedValue = p.QuantityUnitId;
            tbPrepAnalPrepComment.Text = p.Comment;
            cboxPrepAnalPrepWorkflowStatus.SelectedValue = p.WorkflowStatusId;
            tbPrepAnalPrepReqUnit.Text = p.GetRequestedActivityUnitName(conn, trans);

            if (Utils.IsValidGuid(p.PreparationGeometryId))
            {
                PreparationGeometry pg = new PreparationGeometry(conn, trans, p.PreparationGeometryId);
                lblPrepAnalPrepRange.Text = "[" + pg.MinFillHeightMM + ", " + pg.MaxFillHeightMM + "]";
            }

            UI.PopulateAttachments(conn, trans, "preparation", p.Id, gridPrepAnalPrepAttachments);

            btnPrepAnalPrepUpdate.Enabled = true;
            btnPrepAnalPrepDiscard.Enabled = true;

            if (Utils.IsValidGuid(p.AssignmentId))
            {
                Guid labId = DB.GetLaboratoryIdFromOrderId(conn, trans, p.AssignmentId);
                if(Utils.IsValidGuid(labId))
                {
                    if(labId != Common.LabId)
                    {
                        btnPrepAnalPrepUpdate.Enabled = false;
                        btnPrepAnalPrepDiscard.Enabled = false;
                    }
                }
            }

            if (clearDirty)
                p.ClearDirty();
        }

        private void btnPrepAnalAnalUpdate_Click(object sender, EventArgs e)
        {
            if(!analysis.IsDirty())
            {                
                SetStatusMessage("Nothing to save for analysis " + preparation.Number + "/" + analysis.Number);
                return;
            }
            
            SqlConnection conn = null;
            SqlTransaction trans = null;

            try
            {
                conn = DB.OpenConnection();
                trans = conn.BeginTransaction();

                if (analysis.IsClosed(conn, trans))
                {
                    MessageBox.Show("This analysis belongs to a closed order and can not be updated");
                    return;
                }                

                analysis.ActivityUnitId = Guid.Parse(cboxPrepAnalAnalUnit.SelectedValue.ToString());
                analysis.ActivityUnitTypeId = Guid.Parse(cboxPrepAnalAnalUnitType.SelectedValue.ToString());
                analysis.SpecterReference = tbPrepAnalAnalSpecRef.Text;
                analysis.Comment = tbPrepAnalAnalComment.Text;
                analysis.WorkflowStatusId = Convert.ToInt32(cboxPrepAnalAnalWorkflowStatus.SelectedValue);                

                analysis.StoreToDB(conn, trans);

                if (!String.IsNullOrEmpty(analysis.ImportFile) && File.Exists(analysis.ImportFile))
                {
                    try
                    {
                        DB.AddAttachment(conn, trans, "analysis", analysis.Id, Path.GetFileNameWithoutExtension(analysis.ImportFile), ".lis", File.ReadAllBytes(analysis.ImportFile));
                        analysis.ImportFile = String.Empty;
                        UI.PopulateAttachments(conn, trans, "analysis", analysis.Id, gridPrepAnalAnalAttachments);
                    }
                    catch { }
                }

                trans.Commit();

                treePrepAnal.SelectedNode.ForeColor = WorkflowStatus.GetStatusColor(analysis.WorkflowStatusId);
                SetStatusMessage("Analysis updated successfully");
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
            }            
        }        

        private void cboxPrepAnalPrepGeom_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblPrepAnalPrepRange.Text = "";            

            if (Utils.IsValidGuid(cboxPrepAnalPrepGeom.SelectedValue))
            {
                using (SqlConnection conn = DB.OpenConnection())
                {
                    PreparationGeometry pg = new PreparationGeometry(conn, null, Guid.Parse(cboxPrepAnalPrepGeom.SelectedValue.ToString()));
                    lblPrepAnalPrepRange.Text = "[" + pg.MinFillHeightMM + ", " + pg.MaxFillHeightMM + "]";
                }
            }

            preparation.Dirty = true;
        }

        private void btnPrepAnalSampleUpdate_Click(object sender, EventArgs e)
        {
            double lodStart = -1, lodEnd = -1;

            if (!String.IsNullOrEmpty(tbPrepAnalLODStartWeight.Text) && !String.IsNullOrEmpty(tbPrepAnalLODEndWeight.Text))
            {
                lodStart = Convert.ToDouble(tbPrepAnalLODStartWeight.Text);
                lodEnd = Convert.ToDouble(tbPrepAnalLODEndWeight.Text);
                if (lodStart < lodEnd)
                {
                    MessageBox.Show("LOD start weight cannot be smaller than end weight");
                    return;
                }
            }

            SqlConnection conn = null;

            try
            {
                conn = DB.OpenConnection();

                if (sample.IsClosed(conn, null))
                {
                    MessageBox.Show("This sample belongs to a closed order and can not be updated");
                    return;
                }

                if (!String.IsNullOrEmpty(tbPrepAnalWetWeight.Text))
                    sample.WetWeight_g = Convert.ToDouble(tbPrepAnalWetWeight.Text);

                if (!String.IsNullOrEmpty(tbPrepAnalDryWeight.Text))
                    sample.DryWeight_g = Convert.ToDouble(tbPrepAnalDryWeight.Text);

                if (!String.IsNullOrEmpty(tbPrepAnalVolume.Text))
                    sample.Volume_l = Convert.ToDouble(tbPrepAnalVolume.Text);

                if (!String.IsNullOrEmpty(tbPrepAnalLODTemp.Text))
                    sample.LodTemperature = Convert.ToDouble(tbPrepAnalLODTemp.Text);

                sample.LodWeightStart = lodStart;
                sample.LodWeightEnd = lodEnd;                

                sample.StoreLabInfoToDB(conn, null);

                SetStatusMessage("Lab data updated for sample " + sample.Number);
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

        private void PopulateSampleInfo(SqlConnection conn, SqlTransaction trans, Sample s, TreeNode tnode, bool clearDirty)
        {                        
            tnode.ToolTipText = "Component: " + sample.GetSampleComponentName(conn, trans) + Environment.NewLine
                + "External Id: " + sample.ExternalId + Environment.NewLine
                + "Project: " + sample.GetProjectName(conn, trans) + Environment.NewLine
                + "Reference date: " + sample.ReferenceDate.ToString(Utils.DateTimeFormatNorwegian);
            
            tbPrepAnalInfoComment.Text = s.Comment;
            tbPrepAnalWetWeight.Text = s.WetWeight_g.ToString();
            tbPrepAnalDryWeight.Text = s.DryWeight_g.ToString();
            tbPrepAnalVolume.Text = s.Volume_l.ToString();
            tbPrepAnalLODStartWeight.Text = s.LodWeightStart.ToString();
            tbPrepAnalLODEndWeight.Text = s.LodWeightEnd.ToString();
            tbPrepAnalLODTemp.Text = s.LodTemperature.ToString();

            btnPrepAnalSampleUpdate.Enabled = true;
            Guid labId = sample.GetAssignmentLaboratory(conn, trans);
            if (Utils.IsValidGuid(labId) && labId != Common.LabId)
                btnPrepAnalSampleUpdate.Enabled = false;

            if (clearDirty)
                s.ClearDirty();
        }

        private void miImportLISFile_Click(object sender, EventArgs e)
        {    
            using (SqlConnection conn = DB.OpenConnection())
            {
                if (analysis.IsClosed(conn, null))
                {
                    MessageBox.Show("This analysis belongs to a closed order and can not be updated");
                    return;
                }
            }                    
                
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "LIS files (*.lis)|*.lis";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            Analysis a = analysis.Clone();
            a.ImportFile = dialog.FileName;

            FormImportAnalysisLIS form = new FormImportAnalysisLIS(preparation, a);
            if (form.ShowDialog() != DialogResult.OK)            
                return;

            analysis = a.Clone();

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateAnalysis(conn, null, analysis, false);
            }

            SetStatusMessage("Imported LIS file " + analysis.ImportFile + " for analysis");
        }

        private void PopulateAnalysis(SqlConnection conn, SqlTransaction trans, Analysis a, bool clearDirty)
        {
            cboxPrepAnalAnalUnit.SelectedValue = a.ActivityUnitId;
            cboxPrepAnalAnalUnitType.SelectedValue = a.ActivityUnitTypeId;
            cboxPrepAnalAnalWorkflowStatus.SelectedValue = a.WorkflowStatusId;
            tbPrepAnalAnalSpecRef.Text = a.SpecterReference;
            tbPrepAnalAnalNuclLib.Text = a.NuclideLibrary;
            tbPrepAnalAnalMDALib.Text = a.MDALibrary;
            tbPrepAnalAnalComment.Text = a.Comment;            

            PopulateAnalysisResults(a, clearDirty);

            UI.PopulateAttachments(conn, trans, "analysis", a.Id, gridPrepAnalAnalAttachments);

            btnPrepAnalAnalUpdate.Enabled = true;
            btnPrepAnalAnalDiscard.Enabled = true;

            if (Utils.IsValidGuid(a.AssignmentId))
            {
                Guid labId = DB.GetLaboratoryIdFromOrderId(conn, trans, a.AssignmentId);
                if (Utils.IsValidGuid(labId))
                {
                    if (labId != Common.LabId)
                    {
                        btnPrepAnalAnalUpdate.Enabled = false;
                        btnPrepAnalAnalDiscard.Enabled = false;
                    }
                }
            }

            if (clearDirty)
                a.ClearDirty();
        }

        private void PopulateAnalysisResults(Analysis a, bool clearDirty)
        {
            a.Results.Sort((r1, r2) => r1.NuclideName.CompareTo(r2.NuclideName));

            gridPrepAnalResults.DataSource = null;
            gridPrepAnalResults.DataSource = a.Results;

            gridPrepAnalResults.Columns["Id"].Visible = false;
            gridPrepAnalResults.Columns["AnalysisId"].Visible = false;
            gridPrepAnalResults.Columns["NuclideId"].Visible = false;
            gridPrepAnalResults.Columns["UniformActivity"].Visible = false;
            gridPrepAnalResults.Columns["UniformActivityUnitId"].Visible = false;
            gridPrepAnalResults.Columns["InstanceStatusId"].Visible = false;
            gridPrepAnalResults.Columns["CreateDate"].Visible = false;
            gridPrepAnalResults.Columns["CreatedBy"].Visible = false;
            gridPrepAnalResults.Columns["UpdateDate"].Visible = false;
            gridPrepAnalResults.Columns["UpdatedBy"].Visible = false;            

            gridPrepAnalResults.Columns["NuclideName"].HeaderText = "Nuclide";
            gridPrepAnalResults.Columns["ActivityUncertaintyABS"].HeaderText = "Act.Unc.";
            gridPrepAnalResults.Columns["ActivityApproved"].HeaderText = "Act.Appr.";
            gridPrepAnalResults.Columns["DetectionLimit"].HeaderText = "Det.Lim.";
            gridPrepAnalResults.Columns["DetectionLimitApproved"].HeaderText = "Det.Lim.Appr";

            gridPrepAnalResults.Columns["Activity"].DefaultCellStyle.Format = Utils.ScientificFormat;
            gridPrepAnalResults.Columns["ActivityUncertaintyABS"].DefaultCellStyle.Format = Utils.ScientificFormat;
            gridPrepAnalResults.Columns["DetectionLimit"].DefaultCellStyle.Format = Utils.ScientificFormat;

            if(clearDirty)
                foreach (AnalysisResult ar in a.Results)
                    ar.Dirty = false;
        }

        public void CalculateLODPercent()
        {
            double lws, lwe;
            try
            {
                lws = Convert.ToDouble(tbPrepAnalLODStartWeight.Text);
                lwe = Convert.ToDouble(tbPrepAnalLODEndWeight.Text);
            }
            catch
            {
                tbPrepAnalLODWater.Text = "";
                return;
            }

            if (lws < lwe)
            {
                tbPrepAnalLODWater.Text = "";
                return;
            }

            double delta = lws - lwe;
            double percent = (delta / lws) * 100.0;
            tbPrepAnalLODWater.Text = percent.ToString("0.0#");
        }

        private void tbPrepAnalLODStartWeight_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
            CalculateLODPercent();
        }

        private void btnPrepAnalAddPrep_Click(object sender, EventArgs e)
        {
            FormPrepAnalAddPrep form = new FormPrepAnalAddPrep(sample.Id);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulatePrepAnal(conn, sample);
            }
        }

        private void btnPrepAnalAddAnal_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(treePrepAnal.SelectedNode.Name))
                return;

            Guid prepId = Guid.Parse(treePrepAnal.SelectedNode.Name);

            FormPrepAnalAddAnal form = new FormPrepAnalAddAnal(prepId);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulatePrepAnal(conn, sample);
            }
        }

        private void btnSampleAddSampleToOrder_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to add samples to orders");
                return;
            }

            FormSelectOrder form = new FormSelectOrder(treeSampleTypes, sample.Id);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            SetStatusMessage("Successfully added sample " + form.SelectedSampleNumber.ToString() + " to order " + form.SelectedOrderName);
        }

        private void btnSampleSamplingDateFrom_Click(object sender, EventArgs e)
        {
            FormSelectDateTime form = new FormSelectDateTime();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            tbSampleSamplingDateFrom.Tag = form.SelectedDateTime;
            tbSampleSamplingDateFrom.Text = form.SelectedDateTime.ToString(Utils.DateTimeFormatNorwegian);

            btnSampleSamplingDateTo.Enabled = true;
            btnSampleSamplingDateToClear.Enabled = true;
            tbSampleSamplingDateTo.Enabled = true;
        }

        private void btnSampleSamplingDateFromClear_Click(object sender, EventArgs e)
        {
            tbSampleSamplingDateTo.Tag = null;
            tbSampleSamplingDateTo.Text = "";
            tbSampleSamplingDateTo.Enabled = false;

            btnSampleSamplingDateTo.Enabled = false;
            btnSampleSamplingDateToClear.Enabled = false;

            tbSampleSamplingDateFrom.Tag = null;
            tbSampleSamplingDateFrom.Text = "";            
        }

        private void btnSampleSamplingDateToClear_Click(object sender, EventArgs e)
        {            
            tbSampleSamplingDateTo.Tag = null;
            tbSampleSamplingDateTo.Text = "";
        }

        private void btnSampleSamplingDateTo_Click(object sender, EventArgs e)
        {
            FormSelectDateTime form = new FormSelectDateTime();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            tbSampleSamplingDateTo.Tag = form.SelectedDateTime;
            tbSampleSamplingDateTo.Text = form.SelectedDateTime.ToString(Utils.DateTimeFormatNorwegian);            
        }

        private void btnSampleReferenceDateClear_Click(object sender, EventArgs e)
        {            
            tbSampleReferenceDate.Tag = null;
            tbSampleReferenceDate.Text = "";
        }

        private void btnSampleReferenceDate_Click(object sender, EventArgs e)
        {
            FormSelectDateTime form = new FormSelectDateTime();
            if (form.ShowDialog() != DialogResult.OK)
                return;

            tbSampleReferenceDate.Tag = form.SelectedDateTime;
            tbSampleReferenceDate.Text = form.SelectedDateTime.ToString(Utils.DateTimeFormatNorwegian);            
        }

        private void cboxSampleLaboratory_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (!Utils.IsValidGuid(cboxSampleLaboratory.SelectedValue))            
                lblSampleToolLaboratory.Text = "";
            else            
                lblSampleToolLaboratory.Text = "[Laboratory] " + cboxSampleLaboratory.Text;
        }

        private void btnOrderClearDeadline_Click(object sender, EventArgs e)
        {
            tbOrderDeadline.Text = "";
            tbOrderDeadline.Tag = null;

            assignment.Dirty = true;
        }

        private void btnOrderSelectCustomer_Click(object sender, EventArgs e)
        {
            FormSelectCustomer form = new FormSelectCustomer(InstanceStatus.Deleted);
            if (form.ShowDialog() != DialogResult.OK)
                return;
            
            assignment.CustomerContactName = form.SelectedCustomer.ContactName;
            assignment.CustomerContactEmail = form.SelectedCustomer.ContactEmail;
            assignment.CustomerContactPhone = form.SelectedCustomer.ContactPhone;
            assignment.CustomerContactAddress = form.SelectedCustomer.ContactAddress;
            assignment.CustomerCompanyName = form.SelectedCustomer.CompanyName;
            assignment.CustomerCompanyEmail = form.SelectedCustomer.CompanyEmail;
            assignment.CustomerCompanyPhone = form.SelectedCustomer.CompanyPhone;
            assignment.CustomerCompanyAddress = form.SelectedCustomer.CompanyAddress;

            tbOrderCustomer.Text = assignment.CustomerContactName;
            tbOrderCustomerInfo.Text =
                assignment.CustomerContactName + Environment.NewLine +
                assignment.CustomerCompanyName + Environment.NewLine + Environment.NewLine +
                assignment.CustomerContactEmail + Environment.NewLine +
                assignment.CustomerContactPhone + Environment.NewLine + Environment.NewLine +
                assignment.CustomerContactAddress;

            assignment.Dirty = true;
        }

        private void miPersonNew_Click(object sender, EventArgs e)
        {
            // new person
            FormPerson form = new FormPerson();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Person " + form.PersonName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulatePersons(conn, gridSysPers);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create person failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miPersonEdit_Click(object sender, EventArgs e)
        {
            // edit person
            if(gridSysPers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a person first");
                return;
            }

            Guid pid = Guid.Parse(gridSysPers.SelectedRows[0].Cells["id"].Value.ToString());
            FormPerson form = new FormPerson(pid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Person " + form.PersonName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulatePersons(conn, gridSysPers);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update person failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miPersonDelete_Click(object sender, EventArgs e)
        {
            // delete person
        }

        private void miCompanyNew_Click(object sender, EventArgs e)
        {
            // new company
            FormCompany form = new FormCompany();
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Company " + form.CompName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCompanies(conn, gridMetaCompanies);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create company failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miCompanyEdit_Click(object sender, EventArgs e)
        {
            // edit company
            if (gridMetaCompanies.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a company first");
                return;
            }

            Guid cid = Guid.Parse(gridMetaCompanies.SelectedRows[0].Cells["id"].Value.ToString());
            FormCompany form = new FormCompany(cid);
            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Company " + form.CompName + " updated");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        UI.PopulateCompanies(conn, gridMetaCompanies);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Update company failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miCompanyDelete_Click(object sender, EventArgs e)
        {
            // delete company
        }

        private void treeOrderContent_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            Guid orderSampleTypeId = Guid.Empty;
            
            miOrderAddPrepMeth.Enabled = false;
            btnOrderAddPrepMeth.Enabled = false;
            miOrderAddAnalMeth.Enabled = false;
            btnOrderAddAnalMeth.Enabled = false;

            miOrderEditPrepMeth.Enabled = false;
            btnOrderEditPrepMeth.Enabled = false;
            miOrderEditAnalMeth.Enabled = false;
            btnOrderEditAnalMeth.Enabled = false;

            miOrderRemPrepMeth.Enabled = false;
            btnOrderDelPrepMeth.Enabled = false;
            miOrderRemAnalMeth.Enabled = false;
            btnOrderDelAnalMeth.Enabled = false;

            switch (e.Node.Level)
            {
                case 0:
                    miOrderAddPrepMeth.Enabled = true;
                    btnOrderAddPrepMeth.Enabled = true;
                    miOrderEditPrepMeth.Enabled = true;
                    btnOrderEditPrepMeth.Enabled = true;
                    miOrderRemPrepMeth.Enabled = true;
                    btnOrderDelPrepMeth.Enabled = true;

                    orderSampleTypeId = Guid.Parse(e.Node.Name);
                    break;

                case 1:                    
                    miOrderAddAnalMeth.Enabled = true;
                    btnOrderAddAnalMeth.Enabled = true;
                    miOrderEditAnalMeth.Enabled = true;
                    btnOrderEditAnalMeth.Enabled = true;
                    miOrderRemAnalMeth.Enabled = true;
                    btnOrderDelAnalMeth.Enabled = true;

                    orderSampleTypeId = Guid.Parse(e.Node.Parent.Name);
                    break;

                case 2:
                    orderSampleTypeId = Guid.Parse(e.Node.Parent.Parent.Name);
                    break;
            }
        }        

        private void miTypeRelSampleTypesExportSampTypeList_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();            
            dialog.Filter = "TXT files (*.txt)|*.txt";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            List<string> stList = new List<string>();
            AddSampleTypesToList(treeSampleTypes.Nodes, stList);

            stList.Sort(delegate (string s1, string s2) { return s1.CompareTo(s2); });

            using (TextWriter writer = File.CreateText(dialog.FileName))
            {
                foreach (string s in stList)
                    writer.WriteLine(s);
            }
        }

        private void AddSampleTypesToList(TreeNodeCollection nodes, List<string> list)
        {
            foreach (TreeNode tn in nodes)
            {
                list.Add(tn.Text + " -> " + tn.Tag.ToString());
                AddSampleTypesToList(tn.Nodes, list);
            }
        }

        private void btnPrepAnalEditResult_Click(object sender, EventArgs e)
        {
            if (!Utils.IsValidGuid(analysis.Id))            
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                if (analysis.IsClosed(conn, null))
                {
                    MessageBox.Show("This analysis belongs to a closed order and can not be updated");
                    return;
                }
            }

            if (!Utils.IsValidGuid(analysis.ActivityUnitId))
            {
                MessageBox.Show("You must save a unit first");
                return;
            }

            if (gridPrepAnalResults.SelectedRows.Count != 1)
            {
                MessageBox.Show("You must select a single result first");
                return;
            }

            DataGridViewCell cell = null;
            try
            {
                cell = gridPrepAnalResults.SelectedRows[0].Cells["Id"];
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
                return;
            }

            if(cell.Value == null)
            {
                Common.Log.Error("Analysis result cell is null");
                return;
            }

            Guid resultId = Guid.Parse(cell.Value.ToString());
            FormPrepAnalResult form = new FormPrepAnalResult(analysis, resultId);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            PopulateAnalysisResults(analysis, false);
        }

        private void btnPrepAnalAddResult_Click(object sender, EventArgs e)
        {
            if(!Utils.IsValidGuid(analysis.ActivityUnitId))
            {
                MessageBox.Show("You must save a unit first");
                return;
            }
            
            Dictionary<string, Guid> nuclides = null;
            using (SqlConnection conn = DB.OpenConnection())
            {
                if (analysis.IsClosed(conn, null))
                {
                    MessageBox.Show("This analysis belongs to a closed order and can not be updated");
                    return;
                }

                nuclides = DB.GetNuclideNamesForAnalysisMethod(conn, null, analysis.AnalysisMethodId);
            }
            
            foreach (AnalysisResult ar in analysis.Results)            
                nuclides.Remove(ar.NuclideName.ToUpper());                            
            
            FormPrepAnalResult form = new FormPrepAnalResult(analysis, nuclides);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateAnalysis(conn, null, analysis, false);
            }
        }

        private void cboxSampleInfoLocationTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (cboxSampleInfoLocationTypes.SelectedValue == null)
            {
                tbSampleLocation.Text = "";
                tbSampleLocation.Enabled = false;
            }
            else
            {
                tbSampleLocation.Enabled = true;
            }
        }

        private void btnSampleGoToPrepAnal_Click(object sender, EventArgs e)
        {
            if (!Roles.HasAccess(Role.LaboratoryAdministrator, Role.LaboratoryOperator))
            {
                MessageBox.Show("You don't have access to preparations and analyses");
                return;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                if (!PopulatePrepAnal(conn, sample))
                    return;
            }            

            tabs.SelectedTab = tabPrepAnal;
        }

        private void tbSamplesLookup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (String.IsNullOrEmpty(tbSamplesLookup.Text))
                return;

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {                
                int snum = Convert.ToInt32(tbSamplesLookup.Text);

                btnSamplesClearFilters_Click(sender, e);

                if (gridSamples.SelectedRows.Count == 1)
                {
                    int selNum = Convert.ToInt32(gridSamples.SelectedRows[0].Cells["number"].Value);
                    if (snum == selNum)
                        miSamplesPrepAnal_Click(sender, e);
                    else
                        PopulateSamplesSingle(snum);
                }
                
                e.Handled = true;
                btnSamplesSearch.ForeColor = Color.Red;
                return;
            }
        }

        private void PopulateSamples()
        {
            string query = "select distinct ";
            if (cboxSamplesTop.SelectedValue != null)
                query += "top " + cboxSamplesTop.SelectedValue + " ";

            query += @"
		s.id,	
		s.number,
		s.external_id,
		l.name as 'laboratory_name',
		st.name as 'sample_type_name',	
		sc.name as 'sample_component_name',        
		pm.name + ' - ' + ps.name as 'project_name',
		ss.name as 'sample_storage_name',
		s.reference_date,
		insta.name as 'instance_status_name',
		s.locked_by,
		(select number from sample where id = s.transform_from_id) as 'split_from',
		(select number from sample where id = s.transform_to_id) as 'merge_to',
		(select convert(varchar(80), number) + ', ' as 'data()' from sample where transform_to_id = s.id for XML PATH('')) as 'merge_from'
	from sample s 
		left outer join laboratory l on s.laboratory_id = l.id
		left outer join sample_type st on s.sample_type_id = st.id
		left outer join sample_storage ss on s.sample_storage_id = ss.id
		left outer join sample_component sc on s.sample_component_id = sc.id
		inner join project_sub ps on s.project_sub_id = ps.id
		inner join project_main pm on pm.id = ps.project_main_id
		inner join instance_status insta on s.instance_status_id = insta.id
        left outer join sample_x_assignment_sample_type sxast on sxast.sample_id = s.id
        left outer join assignment_sample_type ast on ast.id = sxast.assignment_sample_type_id
        left outer join assignment a on a.id = ast.assignment_id
	where 
        s.instance_status_id = @instance_status_level
";
            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter("", conn);

                if (Utils.IsValidGuid(cboxSamplesProjects.SelectedValue))
                {
                    query += " and pm.id = @project_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@project_id", DB.MakeParam(typeof(Guid), cboxSamplesProjects.SelectedValue));
                }

                if (Utils.IsValidGuid(cboxSamplesProjectsSub.SelectedValue))
                {
                    query += " and ps.id = @project_sub_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@project_sub_id", DB.MakeParam(typeof(Guid), cboxSamplesProjectsSub.SelectedValue));
                }

                if (Utils.IsValidGuid(cboxSamplesOrders.SelectedValue))
                {
                    query += " and a.id = @assignment_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@assignment_id", DB.MakeParam(typeof(Guid), cboxSamplesOrders.SelectedValue));
                }

                if (Utils.IsValidGuid(cboxSamplesLaboratory.SelectedValue))
                {
                    query += " and l.id = @lab_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@lab_id", DB.MakeParam(typeof(Guid), cboxSamplesLaboratory.SelectedValue));
                }

                query += " order by s.number desc";
                
                adapter.SelectCommand.CommandText = query;
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.AddWithValue("@instance_status_level", DB.MakeParam(typeof(int), cboxSamplesStatus.SelectedValue));
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gridSamples.DataSource = dt;

                gridSamples.Columns["id"].Visible = false;
                gridSamples.Columns["merge_to"].Visible = false;

                gridSamples.Columns["number"].HeaderText = "Sample number";
                gridSamples.Columns["external_id"].HeaderText = "Ex.Id";
                gridSamples.Columns["laboratory_name"].HeaderText = "Laboratory";
                gridSamples.Columns["sample_type_name"].HeaderText = "Type";
                gridSamples.Columns["sample_component_name"].HeaderText = "Component";
                gridSamples.Columns["project_name"].HeaderText = "Project";
                gridSamples.Columns["sample_storage_name"].HeaderText = "Storage";
                gridSamples.Columns["reference_date"].HeaderText = "Ref.date";
                gridSamples.Columns["instance_status_name"].HeaderText = "Status";
                gridSamples.Columns["locked_by"].HeaderText = "Locked by";
                gridSamples.Columns["split_from"].HeaderText = "Split from";
                gridSamples.Columns["merge_from"].HeaderText = "Merge from";

                gridSamples.Columns["reference_date"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;
            }

            if (Utils.IsValidGuid(sample.Id))
            {
                gridSamples.ClearSelection();
                foreach (DataGridViewRow row in gridSamples.Rows)
                {
                    Guid sid = Guid.Parse(row.Cells["id"].Value.ToString());
                    if (sample.Id == sid)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }

            ActiveControl = tbSamplesLookup;
        }

        private void PopulateSamplesSingle(int sampleNumber)
        {
            cboxSamplesProjects.SelectedValue = Guid.Empty;
            cboxSamplesProjectsSub.SelectedValue = Guid.Empty;
            cboxSamplesOrders.SelectedValue = Guid.Empty;
            cboxSamplesLaboratory.SelectedValue = Guid.Empty;            

            string query = @"
select distinct
		s.id,	
		s.number,
		s.external_id,
		l.name as 'laboratory_name',
		st.name as 'sample_type_name',	
		sc.name as 'sample_component_name',        
		pm.name + ' - ' + ps.name as 'project_name',
		ss.name as 'sample_storage_name',
		s.reference_date,
		insta.name as 'instance_status_name',
		s.locked_by,
		(select number from sample where id = s.transform_from_id) as 'split_from',
		(select number from sample where id = s.transform_to_id) as 'merge_to',
		(select convert(varchar(80), number) + ', ' as 'data()' from sample where transform_to_id = s.id for XML PATH('')) as 'merge_from'
	from sample s 
		left outer join laboratory l on s.laboratory_id = l.id
		left outer join sample_type st on s.sample_type_id = st.id
		left outer join sample_storage ss on s.sample_storage_id = ss.id
		left outer join sample_component sc on s.sample_component_id = sc.id
		inner join project_sub ps on s.project_sub_id = ps.id
		inner join project_main pm on pm.id = ps.project_main_id
		inner join instance_status insta on s.instance_status_id = insta.id
        left outer join sample_x_assignment_sample_type sxast on sxast.sample_id = s.id
        left outer join assignment_sample_type ast on ast.id = sxast.assignment_sample_type_id
        left outer join assignment a on a.id = ast.assignment_id	
where s.number = @sample_number
";
            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);                

                adapter.SelectCommand.CommandText = query;
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.AddWithValue("@sample_number", sampleNumber);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gridSamples.DataSource = dt;

                gridSamples.Columns["id"].Visible = false;
                gridSamples.Columns["merge_to"].Visible = false;

                gridSamples.Columns["number"].HeaderText = "Sample number";
                gridSamples.Columns["external_id"].HeaderText = "Ex.Id";
                gridSamples.Columns["laboratory_name"].HeaderText = "Laboratory";
                gridSamples.Columns["sample_type_name"].HeaderText = "Type";
                gridSamples.Columns["sample_component_name"].HeaderText = "Component";
                gridSamples.Columns["project_name"].HeaderText = "Project";
                gridSamples.Columns["sample_storage_name"].HeaderText = "Storage";
                gridSamples.Columns["reference_date"].HeaderText = "Ref.date";
                gridSamples.Columns["instance_status_name"].HeaderText = "Status";
                gridSamples.Columns["locked_by"].HeaderText = "Locked by";
                gridSamples.Columns["split_from"].HeaderText = "Split from";
                gridSamples.Columns["merge_from"].HeaderText = "Merge from";

                gridSamples.Columns["reference_date"].DefaultCellStyle.Format = Utils.DateTimeFormatNorwegian;
            }

            ActiveControl = tbSamplesLookup;
        }

        private void btnSamplesClearFilters_Click(object sender, EventArgs e)
        {            
            tbSamplesLookup.Text = "";
            cboxSamplesProjects.SelectedValue = Guid.Empty;
            cboxSamplesProjectsSub.SelectedValue = Guid.Empty;
            cboxSamplesOrders.SelectedValue = Guid.Empty;
            cboxSamplesStatus.SelectedValue = InstanceStatus.Active;
            cboxSamplesLaboratory.SelectedValue = Common.LabId;
            cboxSamplesTop.SelectedValue = 50;
        }

        private void PopulateOrders()
        {
            string query = "select ";
            if(cboxOrdersTop.SelectedValue != null)            
                query += "top " + cboxOrdersTop.SelectedValue + " ";

            query += @"
		a.id,
		a.name,		
		l.name as 'laboratory_name',
		va.name as 'account_name',
		a.deadline,
		a.requested_sigma_act,
		a.requested_sigma_mda,        
		a.customer_contact_name,
        a.customer_company_name,
		a.approved_customer,
		a.approved_laboratory,			
        wf.name as 'workflow_status',
		a.locked_by		
	from assignment a 		
		left outer join laboratory l on a.laboratory_id = l.id
		left outer join cv_account va on a.account_id = va.id
        inner join workflow_status wf on a.workflow_status_id = wf.id
	where 1 = 1
";
            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter("", conn);

                if (Utils.IsValidGuid(cboxOrdersLaboratory.SelectedValue))
                {
                    query += " and l.id = @laboratory_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@laboratory_id", DB.MakeParam(typeof(Guid), cboxOrdersLaboratory.SelectedValue));
                }

                if (!String.IsNullOrEmpty(cboxOrdersYear.Text))
                {
                    query += " and year(a.create_date) = @year";
                    adapter.SelectCommand.Parameters.AddWithValue("@year", Convert.ToInt32(cboxOrdersYear.Text));
                }

                int wstat = Convert.ToInt32(cboxOrdersWorkflowStatus.SelectedValue);
                if (wstat != 0)
                {
                    query += " and a.workflow_status_id = @workflow_status_id";
                    adapter.SelectCommand.Parameters.AddWithValue("@workflow_status_id", wstat);
                }

                query += " order by a.create_date desc";

                adapter.SelectCommand.CommandText = query;
                adapter.SelectCommand.CommandType = CommandType.Text;
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                gridOrders.DataSource = dt;

                gridOrders.Columns["id"].Visible = false;

                gridOrders.Columns["name"].HeaderText = "Name";
                gridOrders.Columns["laboratory_name"].HeaderText = "Laboratory";
                gridOrders.Columns["account_name"].HeaderText = "Responsible";
                gridOrders.Columns["requested_sigma_act"].HeaderText = "Req.Sig.Act.";
                gridOrders.Columns["requested_sigma_mda"].HeaderText = "Req.Sig.MDA";
                gridOrders.Columns["deadline"].HeaderText = "Deadline";                
                gridOrders.Columns["customer_contact_name"].HeaderText = "Contact";
                gridOrders.Columns["customer_company_name"].HeaderText = "Company";
                gridOrders.Columns["approved_customer"].HeaderText = "Appr.Cust";
                gridOrders.Columns["approved_laboratory"].HeaderText = "Appr.Lab";
                gridOrders.Columns["workflow_status"].HeaderText = "Status";                
                gridOrders.Columns["locked_by"].HeaderText = "Locked by";

                gridOrders.Columns["deadline"].DefaultCellStyle.Format = Utils.DateFormatNorwegian;
            }

            if (Utils.IsValidGuid(assignment.Id))
            {
                gridOrders.ClearSelection();
                foreach (DataGridViewRow row in gridOrders.Rows)
                {
                    Guid oid = Guid.Parse(row.Cells["id"].Value.ToString());
                    if (assignment.Id == oid)
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private void miOrdersClearAllFilters_Click(object sender, EventArgs e)
        {            
            cboxOrdersLaboratory.SelectedValue = Guid.Empty;
            cboxOrdersYear.Text = "";
            cboxOrdersWorkflowStatus.SelectedValue = 0;
            cboxOrdersTop.SelectedValue = 50;
        }

        private void btnOrderCreateReport_Click(object sender, EventArgs e)
        {
            FormCreateOrderReport form = new FormCreateOrderReport(assignment.Id);
            form.ShowDialog();
        }

        private void layoutPrepAnalAnal_Resize(object sender, EventArgs e)
        {
            cboxPrepAnalAnalUnitType.Width = panelPrepAnalAnalUnit.Width / 2;
        }                                

        private void btnSysUsersAddRoles_Click(object sender, EventArgs e)
        {
            if(!Roles.IsAdmin())
            {
                MessageBox.Show("You must log in as LIMSAdministrator to manage roles");
                return;
            }

            if(gridMetaUsers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a user first");
                return;
            }

            Guid userId = Guid.Parse(gridMetaUsers.SelectedRows[0].Cells["id"].Value.ToString());
            List<Guid> existingRoles = new List<Guid>();
            foreach (Lemma<Guid, string> l in lbSysUsersRoles.Items)
                existingRoles.Add(l.Id);

            FormAccountXRoles form = new FormAccountXRoles(userId, existingRoles);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateRoles(conn, userId, lbSysUsersRoles);
            }
        }

        private void gridMetaUsers_SelectionChanged(object sender, EventArgs e)
        {
            if (gridMetaUsers.SelectedRows.Count < 1)
                return;

            Guid uid = Guid.Parse(gridMetaUsers.SelectedRows[0].Cells["id"].Value.ToString());

            if(Roles.IsAdmin() || uid == Common.UserId)
            {
                miResetPass.Enabled = true;
                btnMetaUsersResetPass.Enabled = true;
            }
            else
            {
                miResetPass.Enabled = false;
                btnMetaUsersResetPass.Enabled = false;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateRoles(conn, uid, lbSysUsersRoles);
            }
        }

        private void btnSysUsersRemRoles_Click(object sender, EventArgs e)
        {
            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You must log in as LIMSAdministrator to manage roles");
                return;
            }

            if (gridMetaUsers.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a user first");
                return;
            }

            Guid userId = Guid.Parse(gridMetaUsers.SelectedRows[0].Cells["id"].Value.ToString());

            if (lbSysUsersRoles.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select one or more roles first");
                return;
            }

            string query = "delete from account_x_role where account_id = @account_id and role_id = @role_id";

            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                foreach(Lemma<Guid, string> l in lbSysUsersRoles.SelectedItems)
                {
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@account_id", userId);
                    cmd.Parameters.AddWithValue("@role_id", l.Id);
                    cmd.ExecuteNonQuery();
                }

                UI.PopulateRoles(conn, userId, lbSysUsersRoles);
            }
        }        

        private void tbSampleSamplingDateFrom_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (String.IsNullOrEmpty(tbSampleSamplingDateFrom.Text))
            {
                if (tbSampleSamplingDateTo.Tag != null)
                {
                    DateTime sdt = (DateTime)tbSampleSamplingDateTo.Tag;
                    tbSampleReferenceDate.Tag = sdt;
                    tbSampleReferenceDate.Text = sdt.ToString(Utils.DateTimeFormatNorwegian);
                }
                return;
            }

            DateTime sdf = (DateTime)tbSampleSamplingDateFrom.Tag;
            if(sdf > Common.CurrentDate(true))
            {
                MessageBox.Show("Sampling time from must be earlier than current time");
                tbSampleSamplingDateFrom.Tag = null;
                tbSampleSamplingDateFrom.Text = "";
                return;
            }

            if (tbSampleSamplingDateTo.Tag == null)
            {            
                tbSampleReferenceDate.Tag = sdf;
                tbSampleReferenceDate.Text = sdf.ToString(Utils.DateTimeFormatNorwegian);
            }
            else
            {                
                DateTime sdt = (DateTime)tbSampleSamplingDateTo.Tag;
                if (sdf > sdt)
                {
                    MessageBox.Show("Sampling time to must be later than sampling date from");
                    tbSampleSamplingDateFrom.Tag = null;
                    tbSampleSamplingDateFrom.Text = "";
                    return;
                }
                long addTicks = (sdt.Ticks - sdf.Ticks) / 2;
                DateTime rd = new DateTime(sdf.Ticks + addTicks);
                tbSampleReferenceDate.Tag = rd;
                tbSampleReferenceDate.Text = rd.ToString(Utils.DateTimeFormatNorwegian);
            }
        }

        private void tbSampleSamplingDateTo_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;

            if (String.IsNullOrEmpty(tbSampleSamplingDateTo.Text))
            {
                if (tbSampleSamplingDateFrom.Tag != null)
                {
                    DateTime sdf = (DateTime)tbSampleSamplingDateFrom.Tag;
                    tbSampleReferenceDate.Tag = sdf;
                    tbSampleReferenceDate.Text = sdf.ToString(Utils.DateTimeFormatNorwegian);
                }
                return;
            }

            DateTime sdt = (DateTime)tbSampleSamplingDateTo.Tag;
            if (sdt > Common.CurrentDate(true))
            {
                MessageBox.Show("Sampling time to must be earlier than current time");
                tbSampleSamplingDateTo.Tag = null;
                tbSampleSamplingDateTo.Text = "";
                return;
            }

            if (tbSampleSamplingDateFrom.Tag == null)
            {                
                tbSampleReferenceDate.Tag = sdt;
                tbSampleReferenceDate.Text = sdt.ToString(Utils.DateTimeFormatNorwegian);
            }
            else
            {
                DateTime sdf = (DateTime)tbSampleSamplingDateFrom.Tag;
                if(sdf > sdt)
                {
                    MessageBox.Show("Sampling time to must be later than sampling date from");
                    tbSampleSamplingDateTo.Tag = null;
                    tbSampleSamplingDateTo.Text = "";
                    return;
                }
                long addTicks = (sdt.Ticks - sdf.Ticks) / 2;
                DateTime rd = new DateTime(sdf.Ticks + addTicks);
                tbSampleReferenceDate.Tag = rd;
                tbSampleReferenceDate.Text = rd.ToString(Utils.DateTimeFormatNorwegian);
            }
        }

        private void tbSampleReferenceDate_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void btnSamplePrintSampleLabel_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DB.OpenConnection())
            {
                if (!sample.HasRequiredFields())
                {
                    MessageBox.Show("Can not print label. Required fields for this sample must be saved first");
                    return;
                }
            }

            List<Guid> sampleIds = new List<Guid>();
            sampleIds.Add(sample.Id);
            FormPrintSampleLabel form = new FormPrintSampleLabel(Common.Settings, sampleIds);
            form.ShowDialog();
        }

        private void tbMenuLookup_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (String.IsNullOrEmpty(tbMenuLookup.Text))            
                return;            

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                int snum = Convert.ToInt32(tbMenuLookup.Text);                
                tabs.SelectedTab = tabSamples;
                PopulateSamplesSingle(snum);                
                tbMenuLookup.Text = "";
                e.Handled = true;
                return;
            }
        }

        private void gridSamples_SelectionChanged(object sender, EventArgs e)
        {
            ActiveControl = tbSamplesLookup;
        }

        private void gridAttachments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.ShowAttachment(conn, e.RowIndex, grid);
            }
        }

        private void btnSampleScanAttachment_Click(object sender, EventArgs e)
        {
            FormScan form = new FormScan(Common.Settings);
            if (form.ShowDialog() != DialogResult.OK)
                return;            

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "sample", sample.Id, form.DocumentName, ".pdf", form.PdfData);

                UI.PopulateAttachments(conn, null, "sample", sample.Id, gridSampleAttachments);
            }
        }        

        private void btnOrderScanAttachment_Click(object sender, EventArgs e)
        {
            FormScan form = new FormScan(Common.Settings);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "assignment", assignment.Id, form.DocumentName, ".pdf", form.PdfData);

                UI.PopulateAttachments(conn, null, "assignment", assignment.Id, gridOrderAttachments);
            }
        }

        private void btnPrepAnalPrepScanAttachment_Click(object sender, EventArgs e)
        {
            Guid prepId = Guid.Parse(treePrepAnal.SelectedNode.Name);

            FormScan form = new FormScan(Common.Settings);
            if (form.ShowDialog() != DialogResult.OK)
                return;            

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "preparation", prepId, form.DocumentName, ".pdf", form.PdfData);

                UI.PopulateAttachments(conn, null, "preparation", prepId, gridPrepAnalPrepAttachments);
            }
        }

        private void btnPrepAnalAnalScanAttachment_Click(object sender, EventArgs e)
        {
            Guid analId = Guid.Parse(treePrepAnal.SelectedNode.Name);

            FormScan form = new FormScan(Common.Settings);
            if (form.ShowDialog() != DialogResult.OK)
                return;            

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "analysis", analId, form.DocumentName, ".pdf", form.PdfData);

                UI.PopulateAttachments(conn, null, "analysis", analId, gridPrepAnalAnalAttachments);
            }
        }

        private void btnProjectScanAttachment_Click(object sender, EventArgs e)
        {
            if(gridProjectSub.SelectedRows.Count < 1)
            {
                MessageBox.Show("No sub-project selected");
                return;
            }

            Guid psid = Guid.Parse(gridProjectSub.SelectedRows[0].Cells["id"].Value.ToString());

            FormScan form = new FormScan(Common.Settings);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "project_sub", psid, form.DocumentName, ".pdf", form.PdfData);

                UI.PopulateAttachments(conn, null, "project_sub", psid, gridProjectAttachments);
            }
        }

        private void gridProjectSub_SelectionChanged(object sender, EventArgs e)
        {
            if (gridProjectSub.SelectedRows.Count < 1)
                return;

            Guid psid = Guid.Parse(gridProjectSub.SelectedRows[0].Cells["id"].Value.ToString());

            using (SqlConnection conn = DB.OpenConnection())
            {
                UI.PopulateAttachments(conn, null, "project_sub", psid, gridProjectAttachments);
            }
        }

        private void miTypeRelSampleTypesNewRoot_Click(object sender, EventArgs e)
        {
            // New root sample type

            if (!Roles.IsAdmin())
            {
                MessageBox.Show("You don't have access to manage sample types");
                return;
            }

            FormSampleType form = new FormSampleType(null, false);

            switch (form.ShowDialog())
            {
                case DialogResult.OK:
                    SetStatusMessage("Sample type " + form.SampleTypeName + " created");
                    using (SqlConnection conn = DB.OpenConnection())
                    {
                        DB.LoadSampleTypes(conn, null);
                        UI.PopulateSampleTypes(conn, treeSampleTypes);
                        UI.PopulateSampleTypes(treeSampleTypes, cboxSampleSampleType);
                    }
                    break;
                case DialogResult.Abort:
                    SetStatusMessage("Create sample type failed", StatusMessageType.Error);
                    break;
            }
        }

        private void miSamplesPrintSampleLabels_Click(object sender, EventArgs e)
        {
            if(gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("No samples selected");
                return;
            }

            List<Guid> sampleIds = new List<Guid>();

            foreach (DataGridViewRow row in gridSamples.SelectedRows)
            {
                Guid sid = Guid.Parse(row.Cells["id"].Value.ToString());
                sampleIds.Add(sid);
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                foreach (Guid sid in sampleIds)
                {
                    if (!Sample.HasRequiredFields(conn, null, sid))
                    {
                        MessageBox.Show("Can not print labels. Required fields missing for one or more samples");
                        return;
                    }
                }
            }

            FormPrintSampleLabel form = new FormPrintSampleLabel(Common.Settings, sampleIds);
            form.ShowDialog();
        }

        private void btnSampleBrowseAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files(*)|*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            string fileExt = Path.GetExtension(dialog.FileName);
            byte[] content = File.ReadAllBytes(dialog.FileName);

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "sample", sample.Id, fileName, fileExt, content);

                UI.PopulateAttachments(conn, null, "sample", sample.Id, gridSampleAttachments);
            }
        }

        private void btnOrderBrowseAttachment_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files(*)|*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            string fileExt = Path.GetExtension(dialog.FileName);
            byte[] content = File.ReadAllBytes(dialog.FileName);

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "assignment", assignment.Id, fileName, fileExt, content);

                UI.PopulateAttachments(conn, null, "assignment", assignment.Id, gridOrderAttachments);
            }
        }

        private void btnProjectBrowseAttachment_Click(object sender, EventArgs e)
        {
            if(gridProjectSub.SelectedRows.Count < 1)
            {
                MessageBox.Show("No project selected");
                return;
            }

            Guid pid = Guid.Parse(gridProjectSub.SelectedRows[0].Cells["id"].Value.ToString());

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files(*)|*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            string fileExt = Path.GetExtension(dialog.FileName);
            byte[] content = File.ReadAllBytes(dialog.FileName);

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "project_sub", pid, fileName, fileExt, content);

                UI.PopulateAttachments(conn, null, "project_sub", pid, gridProjectAttachments);
            }
        }

        private void btnPrepAnalPrepBrowseAttachment_Click(object sender, EventArgs e)
        {
            if (treePrepAnal.SelectedNode == null)
            {
                MessageBox.Show("No sample selected");
                return;
            }

            if(treePrepAnal.SelectedNode.Level != 1)
            {
                MessageBox.Show("No preparation selected");
                return;
            }

            Guid pid = Guid.Parse(treePrepAnal.SelectedNode.Name);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files(*)|*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            string fileExt = Path.GetExtension(dialog.FileName);
            byte[] content = File.ReadAllBytes(dialog.FileName);

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "preparation", pid, fileName, fileExt, content);

                UI.PopulateAttachments(conn, null, "preparation", pid, gridPrepAnalPrepAttachments);
            }
        }

        private void btnPrepAnalAnalBrowseAttachment_Click(object sender, EventArgs e)
        {
            if (treePrepAnal.SelectedNode == null)
            {
                MessageBox.Show("No sample selected");
                return;
            }

            if (treePrepAnal.SelectedNode.Level != 2)
            {
                MessageBox.Show("No analysis selected");
                return;
            }

            Guid aid = Guid.Parse(treePrepAnal.SelectedNode.Name);

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All files(*)|*";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            string fileName = Path.GetFileNameWithoutExtension(dialog.FileName);
            string fileExt = Path.GetExtension(dialog.FileName);
            byte[] content = File.ReadAllBytes(dialog.FileName);

            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.AddAttachment(conn, null, "analysis", aid, fileName, fileExt, content);

                UI.PopulateAttachments(conn, null, "analysis", aid, gridPrepAnalAnalAttachments);
            }
        }

        private void btnPrepAnalAnalDeleteAttachment_Click(object sender, EventArgs e)
        {
            if(gridPrepAnalAnalAttachments.SelectedRows.Count < 1)
            {
                MessageBox.Show("No attachment selected");
                return;
            }

            if (treePrepAnal.SelectedNode == null || treePrepAnal.SelectedNode.Level != 2)
            {
                MessageBox.Show("No analysis selected");
                return;
            }

            string attName = gridPrepAnalAnalAttachments.SelectedRows[0].Cells["name"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you sure you want to delete attachment '" + attName + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                return;

            Guid analId = Guid.Parse(treePrepAnal.SelectedNode.Name);
            Guid attId = Guid.Parse(gridPrepAnalAnalAttachments.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.DeleteAttachment(conn, null, "analysis", attId);

                UI.PopulateAttachments(conn, null, "analysis", analId, gridPrepAnalAnalAttachments);
            }
        }

        private void btnPrepAnalPrepDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (gridPrepAnalPrepAttachments.SelectedRows.Count < 1)
            {
                MessageBox.Show("No attachment selected");
                return;
            }

            if (treePrepAnal.SelectedNode == null || treePrepAnal.SelectedNode.Level != 1)
            {
                MessageBox.Show("No preparation selected");
                return;
            }

            string attName = gridPrepAnalPrepAttachments.SelectedRows[0].Cells["name"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you sure you want to delete attachment '" + attName + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                return;

            Guid prepId = Guid.Parse(treePrepAnal.SelectedNode.Name);
            Guid attId = Guid.Parse(gridPrepAnalPrepAttachments.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.DeleteAttachment(conn, null, "preparation", attId);

                UI.PopulateAttachments(conn, null, "preparation", prepId, gridPrepAnalPrepAttachments);
            }
        }

        private void btnProjectDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (gridProjectSub.SelectedRows.Count < 1)
            {
                MessageBox.Show("No project selected");
                return;
            }

            if (gridProjectAttachments.SelectedRows.Count < 1)
            {
                MessageBox.Show("No attachment selected");
                return;
            }

            string attName = gridProjectAttachments.SelectedRows[0].Cells["name"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you sure you want to delete attachment '" + attName + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                return;

            Guid projId = Guid.Parse(gridProjectSub.SelectedRows[0].Cells["id"].Value.ToString());
            Guid attId = Guid.Parse(gridProjectAttachments.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.DeleteAttachment(conn, null, "project_sub", attId);

                UI.PopulateAttachments(conn, null, "project_sub", projId, gridProjectAttachments);
            }
        }

        private void btnOrderDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (gridOrderAttachments.SelectedRows.Count < 1)
            {
                MessageBox.Show("No attachment selected");
                return;
            }

            string attName = gridOrderAttachments.SelectedRows[0].Cells["name"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you sure you want to delete attachment '" + attName + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                return;

            Guid attId = Guid.Parse(gridOrderAttachments.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.DeleteAttachment(conn, null, "assignment", attId);

                UI.PopulateAttachments(conn, null, "assignment", assignment.Id, gridOrderAttachments);
            }
        }

        private void btnSampleDeleteAttachment_Click(object sender, EventArgs e)
        {
            if (gridSampleAttachments.SelectedRows.Count < 1)
            {
                MessageBox.Show("No attachment selected");
                return;
            }

            string attName = gridSampleAttachments.SelectedRows[0].Cells["name"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you sure you want to delete attachment '" + attName + "' ?", "Warning", MessageBoxButtons.YesNo);
            if (dr != DialogResult.Yes)
                return;

            Guid attId = Guid.Parse(gridSampleAttachments.SelectedRows[0].Cells["id"].Value.ToString());
            using (SqlConnection conn = DB.OpenConnection())
            {
                DB.DeleteAttachment(conn, null, "sample", attId);

                UI.PopulateAttachments(conn, null, "sample", sample.Id, gridSampleAttachments);
            }
        }

        private void btnPrepAnalPrepPrintLabel_Click(object sender, EventArgs e)
        {
            if (treePrepAnal.SelectedNode == null || treePrepAnal.SelectedNode.Level != 1)
            {
                MessageBox.Show("You must select a preparation first");
                return;
            }

            if(preparation.WorkflowStatusId != WorkflowStatus.Complete)
            {
                MessageBox.Show("Preparation must be saved with complete status before printing a label");
                return;
            }

            List<Guid> prepIds = new List<Guid>();
            prepIds.Add(preparation.Id);

            FormPrintPrepLabel form = new FormPrintPrepLabel(Common.Settings, prepIds);
            form.ShowDialog();
        }

        private void AddSampleTypeToXML(SqlConnection conn, SampleTypeModel stm, XmlDocument doc, XmlNode xnode)
        {
            XmlNode newXNode = doc.CreateElement("sampletype");
            XmlAttribute attrId = doc.CreateAttribute("id");
            attrId.InnerText = stm.Id.ToString();
            newXNode.Attributes.Append(attrId);
            XmlAttribute attrName = doc.CreateAttribute("name");
            attrName.InnerText = stm.Name;
            newXNode.Attributes.Append(attrName);
            XmlAttribute attrCommonName = doc.CreateAttribute("name_common");
            attrCommonName.InnerText = stm.NameCommon;
            newXNode.Attributes.Append(attrCommonName);
            XmlAttribute attrLatinName = doc.CreateAttribute("name_latin");
            attrLatinName.InnerText = stm.NameLatin;
            newXNode.Attributes.Append(attrLatinName);            

            using(SqlDataReader reader = DB.GetDataReader(conn, null, "select id, name from sample_component where sample_type_id = @stid", CommandType.Text, 
                new SqlParameter("@stid", stm.Id)))
            {
                while(reader.Read())
                {
                    XmlNode newCNode = doc.CreateElement("component");
                    XmlAttribute attrCompId = doc.CreateAttribute("id");
                    attrCompId.InnerText = reader["id"].ToString();
                    newCNode.Attributes.Append(attrCompId);
                    XmlAttribute attrCompName = doc.CreateAttribute("name");
                    attrCompName.InnerText = reader["name"].ToString();
                    newCNode.Attributes.Append(attrCompName);
                    newXNode.AppendChild(newCNode);
                }
            }

            xnode.AppendChild(newXNode);

            List<SampleTypeModel> stmNodes = Common.SampleTypeList.FindAll(x => x.ParentId == stm.Id);

            foreach (SampleTypeModel m in stmNodes)
            {
                AddSampleTypeToXML(conn, m, doc, newXNode);
            }
        }

        private void miTypeRelSampleTypesExportSampTypeXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "STX files (*.stx)|*.stx";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            SqlConnection connection = null;            

            try
            {
                connection = DB.OpenConnection();

                XmlDocument doc = new XmlDocument();

                XmlNode xroot = doc.CreateElement("sampletypes");
                doc.AppendChild(xroot);

                List<SampleTypeModel> roots = Common.SampleTypeList.FindAll(x => x.ParentId == Guid.Empty);

                foreach (SampleTypeModel m in roots)
                {
                    AddSampleTypeToXML(connection, m, doc, xroot);
                }

                doc.Save(dialog.FileName);
            }
            catch(Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void miNuclidesExportNuclidesXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "NUX files (*.nux)|*.nux";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            SqlConnection connection = null;

            try
            {
                connection = DB.OpenConnection();

                XmlDocument doc = new XmlDocument();

                XmlNode xroot = doc.CreateElement("nuclides");
                doc.AppendChild(xroot);

                using (SqlDataReader reader = DB.GetDataReader(connection, null, "select * from nuclide", CommandType.Text))
                {
                    while (reader.Read())
                    {
                        XmlNode newXNode = doc.CreateElement("nuclide");

                        XmlAttribute attrId = doc.CreateAttribute("id");
                        attrId.InnerText = reader["id"].ToString();
                        newXNode.Attributes.Append(attrId);

                        XmlAttribute attrZAS = doc.CreateAttribute("zas");
                        attrZAS.InnerText = reader["zas"].ToString();
                        newXNode.Attributes.Append(attrZAS);

                        XmlAttribute attrName = doc.CreateAttribute("name");
                        attrName.InnerText = reader["name"].ToString();
                        newXNode.Attributes.Append(attrName);

                        XmlAttribute attrProtons = doc.CreateAttribute("protons");
                        attrProtons.InnerText = Convert.ToInt32(reader["protons"]).ToString();
                        newXNode.Attributes.Append(attrProtons);

                        XmlAttribute attrNeutrons = doc.CreateAttribute("neutrons");
                        attrNeutrons.InnerText = Convert.ToInt32(reader["neutrons"]).ToString();
                        newXNode.Attributes.Append(attrNeutrons);

                        XmlAttribute attrMetaStable = doc.CreateAttribute("meta_stable");
                        attrMetaStable.InnerText = Convert.ToInt32(reader["meta_stable"]).ToString();
                        newXNode.Attributes.Append(attrMetaStable);

                        XmlAttribute attrHalfLife = doc.CreateAttribute("half_life_year");
                        attrHalfLife.InnerText = Convert.ToDouble(reader["half_life_year"]).ToString();
                        newXNode.Attributes.Append(attrHalfLife);

                        xroot.AppendChild(newXNode);
                    }
                }

                doc.Save(dialog.FileName);
            }
            catch (Exception ex)
            {
                Common.Log.Error(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection?.Close();
            }
        }

        private void cboxPrepAnalAnalUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;            
        }

        private void cboxPrepAnalAnalUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;            
        }

        private void tbPrepAnalAnalSpecRef_TextChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;            
        }

        private void tbPrepAnalAnalNuclLib_TextChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;            
        }

        private void tbPrepAnalAnalMDALib_TextChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;            
        }

        private void tbPrepAnalAnalComment_TextChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;
        }

        private void cboxPrepAnalAnalWorkflowStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            analysis.Dirty = true;
        }

        private void tbPrepAnalPrepFillHeight_TextChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;            
        }

        private void tbPrepAnalPrepAmount_TextChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void cboxPrepAnalPrepAmountUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void tbPrepAnalPrepQuantity_TextChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void cboxPrepAnalPrepQuantityUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void tbPrepAnalPrepComment_TextChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void cboxPrepAnalPrepWorkflowStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            preparation.Dirty = true;
        }

        private void treePrepAnal_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (!DiscardUnsavedChanges())
            {
                e.Cancel = true;
                return;
            }
        }

        private void btnPrepAnalRemoveResult_Click(object sender, EventArgs e)
        {
            if (gridPrepAnalResults.SelectedRows.Count > 0)
            {
                DialogResult r = MessageBox.Show("Are you sure you want to delete " + gridPrepAnalResults.SelectedRows.Count + " results from this analysis?", "Warning", MessageBoxButtons.YesNo);
                if (r == DialogResult.No)                
                    return;

                foreach (DataGridViewRow row in gridPrepAnalResults.SelectedRows)
                {
                    if (row.Cells["Id"].Value != null)
                    {
                        Guid id = Guid.Parse(row.Cells["Id"].Value.ToString());
                        analysis.Results.RemoveAll(x => x.Id == id);
                    }
                }

                using (SqlConnection conn = DB.OpenConnection())
                {
                    PopulateAnalysis(conn, null, analysis, false);
                }

                analysis.Dirty = true;
            }
        }

        private void btnPrepAnalAnalDiscard_Click(object sender, EventArgs e)
        {
            if (!analysis.IsDirty())
            {
                SetStatusMessage("Nothing to discard for analysis " + preparation.Number + "/" + analysis.Number);
                return;
            }

            DialogResult res = MessageBox.Show("Are you sure you want to discard current changes?", "Confirmation", MessageBoxButtons.YesNo);
            if (res != DialogResult.Yes)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                analysis.LoadFromDB(conn, null, analysis.Id);
                PopulateAnalysis(conn, null, analysis, true);
            }

            SetStatusMessage("Changes discarded for analysis " + preparation.Number + "/" + analysis.Number);
        }

        private void btnPrepAnalPrepDiscard_Click(object sender, EventArgs e)
        {
            if (!preparation.IsDirty())
            {
                SetStatusMessage("Nothing to discard for preparation " + preparation.Number);
                return;
            }

            DialogResult res = MessageBox.Show("Are you sure you want to discard current changes?", "Confirmation", MessageBoxButtons.YesNo);
            if (res != DialogResult.Yes)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                preparation.LoadFromDB(conn, null, preparation.Id);
                PopulatePreparation(conn, null, preparation, true);
            }

            SetStatusMessage("Changes discarded for preparation " + preparation.Number);
        }

        private void miSamplesUnlock_Click(object sender, EventArgs e)
        {
            if (gridSamples.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select a sample first");
                return;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("update sample set locked_by = @locked_by where id = @id", conn);

                foreach (DataGridViewRow row in gridSamples.SelectedRows)
                {
                    Guid sampleId = Guid.Parse(row.Cells["id"].Value.ToString());
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@locked_by", DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", sampleId);
                    cmd.ExecuteNonQuery();
                }

                PopulateSamples();
            }
        }

        private void miOrdersUnlock_Click(object sender, EventArgs e)
        {
            if (gridOrders.SelectedRows.Count < 1)
            {
                MessageBox.Show("You must select one or more samples first");
                return;
            }

            using (SqlConnection conn = DB.OpenConnection())
            {
                SqlCommand cmd = new SqlCommand("update assignment set locked_by = @locked_by where id = @id", conn);

                foreach (DataGridViewRow row in gridOrders.SelectedRows)
                {
                    Guid orderId = Guid.Parse(row.Cells["id"].Value.ToString());
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@locked_by", DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", orderId);
                    cmd.ExecuteNonQuery();
                }

                PopulateOrders();
            }
        }

        private void btnSamplesSearch_Click(object sender, EventArgs e)
        {                                    
            PopulateSamples();
            btnSamplesSearch.ForeColor = SystemColors.ControlText;
        }

        private void btnOrdersSearch_Click(object sender, EventArgs e)
        {
            PopulateOrders();
            btnOrdersSearch.ForeColor = SystemColors.ControlText;
        }

        private void cboxOrdersLaboratory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOrdersSearch.ForeColor = Color.Red;
        }

        private void cboxOrdersYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOrdersSearch.ForeColor = Color.Red;
        }

        private void cboxOrdersWorkflowStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOrdersSearch.ForeColor = Color.Red;
        }

        private void cboxOrdersTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOrdersSearch.ForeColor = Color.Red;
        }

        private void cboxSamplesProjectsSub_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void cboxSamplesTop_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void cboxSamplesOrders_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void cboxSamplesStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void cboxSamplesLaboratory_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSamplesSearch.ForeColor = Color.Red;
        }

        private void btnSampleSelectCoords_Click(object sender, EventArgs e)
        {
            double? lat = null, lon = null;

            if (!String.IsNullOrEmpty(tbSampleInfoLatitude.Text) && !String.IsNullOrEmpty(tbSampleInfoLongitude.Text))
            {
                try
                {
                    lat = Convert.ToDouble(tbSampleInfoLatitude.Text);
                    lon = Convert.ToDouble(tbSampleInfoLongitude.Text);
                }
                catch { }
            }

            FormGetCoords form = new FormGetCoords(lat, lon);
            if (form.ShowDialog() != DialogResult.OK)
                return;

            tbSampleInfoLatitude.Text = form.SelectedLatitude.ToString();
            tbSampleInfoLongitude.Text = form.SelectedLongitude.ToString();
        }

        private void btnPrepAnalShowAudit_Click(object sender, EventArgs e)
        {
            if (treePrepAnal.SelectedNode == null)
                return;

            string title;

            switch (treePrepAnal.SelectedNode.Level)
            {
                case 0:
                    break;
                case 1:
                    Guid pid = Guid.Parse(treePrepAnal.SelectedNode.Name);
                    title = treePrepAnal.SelectedNode.Parent.Text + "     " + treePrepAnal.SelectedNode.Text;
                    ShowAuditLog("preparation", pid, title);
                    break;
                case 2:
                    Guid aid = Guid.Parse(treePrepAnal.SelectedNode.Name);
                    title = treePrepAnal.SelectedNode.Parent.Parent.Text + "     " + treePrepAnal.SelectedNode.Parent.Text + "     " + treePrepAnal.SelectedNode.Text;
                    ShowAuditLog("analysis", aid, title);
                    break;
            }
        }

        private void ShowAuditLog(string table, Guid id, string title)
        {
            lbAuditLog.Items.Clear();
            lblAuditLogTitle.Text = title;

            using (SqlConnection conn = DB.OpenConnection())
            {
                string query = "select value from audit_log where source_table = @table and source_id = @id order by create_date desc";
                using (SqlDataReader reader = DB.GetDataReader(conn, null, query, CommandType.Text, new[] {
                    new SqlParameter("@table", table),
                    new SqlParameter("@id", id),
                }))
                {
                    while (reader.Read())
                    {
                        lbAuditLog.Items.Add(reader["value"].ToString());                        
                    }
                }                
            }

            tabs.SelectedTab = tabAuditLog;
        }

        private void miOrderDiscard_Click(object sender, EventArgs e)
        {
            if (!assignment.IsDirty())
            {
                SetStatusMessage("Nothing to discard for order " + assignment.Name);
                return;
            }

            DialogResult res = MessageBox.Show("Are you sure you want to discard current changes?", "Confirmation", MessageBoxButtons.YesNo);
            if (res != DialogResult.Yes)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                assignment.LoadFromDB(conn, null, assignment.Id);
                PopulateOrder(conn, null, assignment, true);
            }

            SetStatusMessage("Changes discarded for order " + assignment.Name);
        }

        private void cboxOrderResponsible_SelectedIndexChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cboxOrderRequestedSigma_SelectedIndexChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cboxOrderRequestedSigmaMDA_SelectedIndexChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void tbOrderContentComment_TextChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void tbOrderReportComment_TextChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cbOrderApprovedLaboratory_CheckedChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cbOrderApprovedCustomer_CheckedChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cboxOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            assignment.Dirty = true;
        }

        private void cboxSampleSampleComponent_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cbSampleConfidential_CheckedChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cboxSampleInfoSampler_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cboxSampleInfoSamplingMeth_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbSampleInfoLatitude_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbSampleInfoLongitude_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbSampleInfoAltitude_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cboxSampleMunicipalities_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbSampleLocation_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cboxSampleSampleStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void cboxSampleInstanceStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbSampleComment_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void btnSampleDiscard_Click(object sender, EventArgs e)
        {
            if (!sample.IsDirty())
            {
                SetStatusMessage("Nothing to discard for sample " + sample.Number);
                return;
            }

            DialogResult res = MessageBox.Show("Are you sure you want to discard current changes?", "Confirmation", MessageBoxButtons.YesNo);
            if (res != DialogResult.Yes)
                return;

            using (SqlConnection conn = DB.OpenConnection())
            {
                sample.LoadFromDB(conn, null, sample.Id);
                PopulateSample(conn, null, sample, true);
            }

            SetStatusMessage("Changes discarded for sample " + sample.Number);
        }

        private void miAbout_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
        }

        private void tbPrepAnalWetWeight_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbPrepAnalDryWeight_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbPrepAnalVolume_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void tbPrepAnalLODTemp_TextChanged(object sender, EventArgs e)
        {
            sample.Dirty = true;
        }

        private void btnPrepAnalSampleDiscard_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DB.OpenConnection())
            {
                PopulateSampleInfo(conn, null, sample, treePrepAnal.Nodes[0], true);
            }
        }
    }    
}
