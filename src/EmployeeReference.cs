using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.PolicyManagment;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.EmployeeManagement;
using Architecture.Model.HRM.PolicyManagment;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.HRM.EmployeeManagement
{

public class EmployeeReference : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private bool UpdateMode;

	private int Success;

	private IContainer components;

	private ToolStripButton btnPrint;

	private ToolStripButton btnupdate;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripLabel toolStripLabel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnsave;

	private Panel panel4;

	internal GridEX grdEmployeeRefrence;

	private Panel panel2;

	private Panel panel1;

	private Panel panel3;

	private TextBox txtDescription;

	private Label label7;

	private TextBox txtEmail;

	private Label label4;

	private TextBox txtMobile;

	private Label label5;

	internal UltraCombo cmbDesignation;

	private TextBox txtOrganizationName;

	private Label label2;

	internal UltraCombo comproject;

	private Label label41;

	internal UltraCombo combranches;

	private Label label42;

	private Label label6;

	private TextBox txtRefrenceName;

	internal UltraCombo cmbEmployeeName;

	private Label label1;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	public EmployeeReference(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void EmployeeReference_Load(object sender, EventArgs e)
	{
		try
		{
			Project();
			braches();
			EmployeeName();
			DesignationNameComboFill();
			cmbEmployeeName.Focus();
			UpdateMode = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool Validation()
	{
		if (((UltraGridBase)combranches).ActiveRow == null)
		{
			MessageBox.Show("Branche Required!");
			combranches.Focus();
			return false;
		}
		if (((UltraGridBase)comproject).ActiveRow == null)
		{
			MessageBox.Show("Project Required!");
			comproject.Focus();
			return false;
		}
		if (((UltraGridBase)cmbEmployeeName).ActiveRow == null)
		{
			MessageBox.Show("Employee Name Required!");
			cmbEmployeeName.Focus();
			return false;
		}
		if (txtRefrenceName.Text == string.Empty)
		{
			MessageBox.Show("Refrence Name Required!");
			txtRefrenceName.Focus();
			return false;
		}
		if (txtOrganizationName.Text == string.Empty)
		{
			MessageBox.Show("Organization Name Required!");
			txtOrganizationName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDesignation).ActiveRow == null)
		{
			MessageBox.Show("Designation Required!");
			cmbDesignation.Focus();
			return false;
		}
		if (txtMobile.Text == string.Empty)
		{
			MessageBox.Show("Mobile No Required!");
			txtMobile.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			RecId = 0;
			((Control)(object)cmbEmployeeName).Text = "";
			((Control)(object)cmbDesignation).Text = "";
			txtRefrenceName.Text = "";
			txtOrganizationName.Clear();
			txtDescription.Clear();
			txtMobile.Clear();
			txtEmail.Clear();
			cmbEmployeeName.Focus();
			UpdateMode = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = hrmEmployeeReference.Getall(new hrmEmployeeReference
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("EmployeeRefrenceId");
			dt.Columns.Add("Employee");
			dt.Columns.Add("RefrenceName");
			dt.Columns.Add("OrganizationName");
			dt.Columns.Add("Designation");
			dt.Columns.Add("Mobile");
			dt.Columns.Add("Email");
			dt.Columns.Add("Description");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["EmployeeReferenceId"], lst.Rows[i]["EmployeeName"], lst.Rows[i]["Name"], lst.Rows[i]["OrginizationName"], lst.Rows[i]["DesignationName"], lst.Rows[i]["Mobile"], lst.Rows[i]["Email"], lst.Rows[i]["Description"]);
			}
			grdEmployeeRefrence.DataSource = dt;
			grdEmployeeRefrence.RetrieveStructure();
			grdEmployeeRefrenceSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdEmployeeRefrenceSetting()
	{
		try
		{
			grdEmployeeRefrence.RootTable.Columns["Employee"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["RefrenceName"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["OrganizationName"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["Designation"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["Mobile"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["Email"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["Description"].EditType = (EditType)0;
			grdEmployeeRefrence.RootTable.Columns["Employee"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["RefrenceName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["OrganizationName"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["Designation"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["Mobile"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["Email"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["Description"].FilterEditType = (FilterEditType)1;
			grdEmployeeRefrence.RootTable.Columns["EmployeeRefrenceId"].Visible = false;
			grdEmployeeRefrence.RootTable.Columns["Employee"].Width = 250;
			grdEmployeeRefrence.RootTable.Columns["RefrenceName"].Width = 150;
			grdEmployeeRefrence.RootTable.Columns["OrganizationName"].Width = 150;
			grdEmployeeRefrence.RootTable.Columns["Designation"].Width = 200;
			grdEmployeeRefrence.RootTable.Columns["Mobile"].Width = 150;
			grdEmployeeRefrence.RootTable.Columns["Email"].Width = 200;
			grdEmployeeRefrence.RootTable.Columns["Description"].Width = 200;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			hrmEmployeeReference refr = new hrmEmployeeReference();
			RecId = Id;
			refr = hrmEmployeeReference.GetByID(RecId);
			if (refr != null)
			{
				combranches.Value = Conversion.ToInt((object)refr.BranchId);
				comproject.Value = Conversion.ToInt((object)refr.ProjectId);
				cmbEmployeeName.Value = Conversion.ToInt((object)refr.EmployeeId);
				cmbDesignation.Value = Conversion.ToInt((object)refr.DesignationId);
				txtRefrenceName.Text = Conversion.ToString((object)refr.Name);
				txtDescription.Text = Conversion.ToString((object)refr.Description);
				txtMobile.Text = Conversion.ToString((object)refr.Mobile);
				txtEmail.Text = Conversion.ToString((object)refr.Email);
				txtOrganizationName.Text = Conversion.ToString((object)refr.OrginizationName);
				btnupdate.Visible = true;
				btnsave.Visible = false;
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtProject = CommonServices.ProjectServiceBind();
			if (dtProject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).DisplayLayout.Bands[0].Columns[1].Width = 168;
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable dtBranch = CommonServices.BrancheServiceBind();
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBranch, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).DisplayLayout.Bands[0].Columns[1].Width = 180;
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = genEmployee.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EmployeeName");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["EmployeeId"], lst.Rows[i]["FistName"]?.ToString() + " " + lst.Rows[i]["MiddleName"]?.ToString() + " " + lst.Rows[i]["LastName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbEmployeeName, "Id", "EmployeeName", "Employee Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DesignationNameComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			DataTable dtDes = genProfile.GetByProfileTypeId(new genProfile
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ProfileTypeId = Conversion.ToInt((object)32)
			});
			if (dtDes.Rows.Count > 0)
			{
				DDL.BindDDL(dtDes, cmbDesignation, "ProfileId", "ProfileName", "Designation Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmployeeReference_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnupdate_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				hrmEmployeeReference refr = new hrmEmployeeReference();
				refr.EmployeeReferenceId = Conversion.ToInt((object)RecId);
				refr.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				refr.Name = Conversion.ToString((object)txtRefrenceName.Text);
				refr.OrginizationName = Conversion.ToString((object)txtOrganizationName.Text);
				refr.DesignationId = Conversion.ToInt(cmbDesignation.Value);
				refr.Description = Conversion.ToString((object)txtDescription.Text);
				refr.Mobile = Conversion.ToString((object)txtMobile.Text);
				refr.Email = Conversion.ToString((object)txtEmail.Text);
				refr.ProjectId = Conversion.ToInt(comproject.Value);
				refr.CreatedById = UserAccount.ID;
				refr.AlteredById = UserAccount.ID;
				refr.CompanyId = UserAccount.CompanyId;
				refr.OrganizationId = UserAccount.OrganizationId;
				refr.BranchId = Conversion.ToInt(combranches.Value);
				refr.ProjectId = Conversion.ToInt(comproject.Value);
				Success = hrmEmployeeReference.Save(refr);
				if (Success > 0)
				{
					MessageBox.Show("Update Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (Validation())
			{
				hrmEmployeeReference refr = new hrmEmployeeReference();
				refr.EmployeeId = Conversion.ToInt(cmbEmployeeName.Value);
				refr.Name = Conversion.ToString((object)txtRefrenceName.Text);
				refr.OrginizationName = Conversion.ToString((object)txtOrganizationName.Text);
				refr.DesignationId = Conversion.ToInt(cmbDesignation.Value);
				refr.Description = Conversion.ToString((object)txtDescription.Text);
				refr.Mobile = Conversion.ToString((object)txtMobile.Text);
				refr.Email = Conversion.ToString((object)txtEmail.Text);
				refr.ProjectId = Conversion.ToInt(comproject.Value);
				refr.CreatedById = UserAccount.ID;
				refr.AlteredById = UserAccount.ID;
				refr.OrganizationId = UserAccount.OrganizationId;
				refr.CompanyId = UserAccount.CompanyId;
				refr.BranchId = Conversion.ToInt(combranches.Value);
				refr.ProjectId = Conversion.ToInt(comproject.Value);
				Success = hrmEmployeeReference.Save(refr);
				if (Success > 0)
				{
					MessageBox.Show("Save Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmployeeRefrence_DoubleClick(object sender, EventArgs e)
	{
		RetrivedData(Conversion.ToInt((object)grdEmployeeRefrence.CurrentRow.Cells[0].Value.ToString()));
		UpdateMode = true;
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdEmployeeRefrence).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdEmployeeRefrence).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdEmployeeRefrence.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.EmployeeManagement.EmployeeReference));
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdEmployeeRefrence = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtEmail = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtMobile = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbDesignation = new UltraCombo();
		this.txtOrganizationName = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtRefrenceName = new System.Windows.Forms.TextBox();
		this.cmbEmployeeName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRefrence).BeginInit();
		this.panel2.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).BeginInit();
		base.SuspendLayout();
		this.btnPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(58, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.btnupdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(77, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnnew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripLabel1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(113, 24);
		this.toolStripLabel1.Text = "Employee Refrence";
		this.toolStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripLabel1, this.toolStripSeparator1, this.btnnew, this.btnupdate, this.btnsave, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1028, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 153);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1028, 406);
		this.panel4.TabIndex = 2;
		this.grdEmployeeRefrence.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmployeeRefrence.FilterMode = (FilterMode)2;
		this.grdEmployeeRefrence.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmployeeRefrence.GroupByBoxVisible = false;
		this.grdEmployeeRefrence.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Name = "grdEmployeeRefrence";
		((ScrollableBase)this.grdEmployeeRefrence).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdEmployeeRefrence).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdEmployeeRefrence).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).Size = new System.Drawing.Size(1028, 406);
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).TabIndex = 0;
		this.grdEmployeeRefrence.TotalRow = (InheritableBoolean)1;
		this.grdEmployeeRefrence.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmployeeRefrence).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEmployeeRefrence).DoubleClick += new System.EventHandler(grdEmployeeRefrence_DoubleClick);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1028, 28);
		this.panel2.TabIndex = 0;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1028, 559);
		this.panel1.TabIndex = 3;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.txtDescription);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.txtEmail);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtMobile);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDesignation);
		this.panel3.Controls.Add(this.txtOrganizationName);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add(this.label41);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label42);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.txtRefrenceName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployeeName);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1028, 125);
		this.panel3.TabIndex = 1;
		this.txtDescription.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDescription.Location = new System.Drawing.Point(104, 90);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(919, 23);
		this.txtDescription.TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 94);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(70, 15);
		this.label7.TabIndex = 109;
		this.label7.Text = "Description";
		this.txtEmail.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmail.Location = new System.Drawing.Point(459, 63);
		this.txtEmail.Name = "txtEmail";
		this.txtEmail.Size = new System.Drawing.Size(228, 23);
		this.txtEmail.TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(339, 67);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(38, 15);
		this.label4.TabIndex = 107;
		this.label4.Text = "Email";
		this.txtMobile.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMobile.Location = new System.Drawing.Point(104, 63);
		this.txtMobile.Name = "txtMobile";
		this.txtMobile.Size = new System.Drawing.Size(228, 23);
		this.txtMobile.TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(12, 67);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 105;
		this.label5.Text = "Mobile";
		this.cmbDesignation.AllowNull = (DefaultableBoolean)1;
		this.cmbDesignation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDesignation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDesignation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDesignation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDesignation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Location = new System.Drawing.Point(795, 36);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Name = "cmbDesignation";
		((System.Windows.Forms.Control)(object)this.cmbDesignation).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbDesignation).TabIndex = 5;
		this.txtOrganizationName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrganizationName.Location = new System.Drawing.Point(459, 35);
		this.txtOrganizationName.Name = "txtOrganizationName";
		this.txtOrganizationName.Size = new System.Drawing.Size(228, 23);
		this.txtOrganizationName.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(339, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(112, 15);
		this.label2.TabIndex = 102;
		this.label2.Text = "Organization Name";
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(459, 3);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(339, 12);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(46, 15);
		this.label41.TabIndex = 99;
		this.label41.Text = "Project";
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(104, 6);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(8, 12);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(80, 15);
		this.label42.TabIndex = 98;
		this.label42.Text = "Branch Name";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(693, 39);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 15);
		this.label6.TabIndex = 29;
		this.label6.Text = "Designation";
		this.txtRefrenceName.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefrenceName.Location = new System.Drawing.Point(104, 37);
		this.txtRefrenceName.Name = "txtRefrenceName";
		this.txtRefrenceName.Size = new System.Drawing.Size(228, 23);
		this.txtRefrenceName.TabIndex = 3;
		this.cmbEmployeeName.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployeeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployeeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployeeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployeeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployeeName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Location = new System.Drawing.Point(795, 6);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Name = "cmbEmployeeName";
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployeeName).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(693, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 19;
		this.label1.Text = "Employee Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 39);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 17;
		this.label3.Text = "Refrence Name";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(978, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdEmployeeRefrence;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1028, 559);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EmployeeReference";
		this.Text = "Employee Reference";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(EmployeeReference_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EmployeeReference_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmployeeRefrence).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDesignation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployeeName).EndInit();
		base.ResumeLayout(false);
	}
}

}
