using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Account_Definition;

public class CustomersAllocateToSalesMan : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Label label1;

	private Panel panel3;

	internal UltraCombo CmbSalesMan;

	private Label label2;

	internal UltraCombo CmbCity;

	private Label label3;

	private Panel panel4;

	private Label label4;

	private Button btnDeAllocate;

	private Panel panel5;

	private Label label5;

	private Button BtnAllocateCustomers;

	private CheckBox CbCityWiseUnAllocatedCustomers;

	private CheckBox CbCityWiseAllocatedCustomers;

	private DateTimePicker txtEffectiveDate;

	private Label EFfectiveDate;

	private Label label6;

	internal UltraCombo CmbCommissionType;

	private TextBox txtCommRate;

	private Label label7;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private CheckBox CbIsActive;

	private Panel panel6;

	internal GridEX GrdUnAllocatedAccounts;

	private Panel panel7;

	internal GridEX GrdAllocatedAccounts;

	private Button btnShow;

	public CustomersAllocateToSalesMan(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void CustomersAllocateToSalesMan_Load(object sender, EventArgs e)
	{
		try
		{
			panel1.AutoScroll = true;
			panel1.HorizontalScroll.Visible = true;
			panel1.HorizontalScroll.Enabled = true;
			SalesManComboFill();
			CommissionTypeFill();
			CityComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalesManComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt = CommonServices.GetSupplierustomerByCustomerGroupId("4");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbSalesMan, "Id", "CompanyName", "Sales Man", false);
				((UltraGridBase)CmbSalesMan).DisplayLayout.Bands[0].Columns[1].Width = 185;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, CmbCommissionType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)CmbCommissionType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityComboFill()
	{
		try
		{
			DataTable dt = CommonServices.CityGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCity, "Id", "CityName", "City Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSalesMan_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbSalesMan.Value) > 0)
			{
				ReadAllocatedCustomers();
				ReadUnAllocatedCustomers();
			}
			else
			{
				GrdAllocatedAccounts.ClearStructure();
				GrdUnAllocatedAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadAllocatedCustomers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (CbCityWiseAllocatedCustomers.Checked)
			{
				if (Conversion.ToInt(CmbCity.Value) <= 0)
				{
					MessageBox.Show("Please Select City First For Allocated Customers");
					CmbCity.Focus();
					return;
				}
				obj.CityId = Conversion.ToInt(CmbCity.Value);
			}
			obj.SupplierCustomerId = Conversion.ToInt(CmbSalesMan.Value);
			DataTable dt = CustomerAllocateToSalesMan.ReadAllocatedCustomerBySalesMan(obj);
			if (dt.Rows.Count > 0)
			{
				GrdAllocatedAccounts.ClearStructure();
				GrdAllocatedAccounts.DataSource = dt;
				GrdAllocatedAccounts.RetrieveStructure();
				GrdAllocatedAccounts.RootTable.Columns["CustomerId"].Visible = false;
				GrdAllocatedAccounts.ColumnAutoResize = true;
				GrdAllocatedAccounts.RootTable.Columns.Add("Select");
				GrdAllocatedAccounts.RootTable.Columns["Select"].ActAsSelector = true;
				GrdAllocatedAccounts.RootTable.Columns["Select"].UseHeaderSelector = true;
				GrdAllocatedAccounts.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				GrdAllocatedAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadUnAllocatedCustomers()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (CbCityWiseUnAllocatedCustomers.Checked)
			{
				if (Conversion.ToInt(CmbCity.Value) <= 0)
				{
					MessageBox.Show("Please Select City First For UnAllocated Customers");
					CmbCity.Focus();
					return;
				}
				obj.CityId = Conversion.ToInt(CmbCity.Value);
			}
			DataTable dt = CustomerAllocateToSalesMan.ReadUnAllocatedCustomer(obj);
			if (dt.Rows.Count > 0)
			{
				GrdUnAllocatedAccounts.ClearStructure();
				GrdUnAllocatedAccounts.DataSource = dt;
				GrdUnAllocatedAccounts.RetrieveStructure();
				GrdUnAllocatedAccounts.ColumnAutoResize = true;
				GrdUnAllocatedAccounts.RootTable.Columns["Id"].Visible = false;
				GrdUnAllocatedAccounts.RootTable.Columns.Add("Select");
				GrdUnAllocatedAccounts.RootTable.Columns["Select"].ActAsSelector = true;
				GrdUnAllocatedAccounts.RootTable.Columns["Select"].UseHeaderSelector = true;
				GrdUnAllocatedAccounts.RootTable.Columns["Select"].Width = 50;
			}
			else
			{
				GrdUnAllocatedAccounts.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbSalesMan).ActiveRow == null)
		{
			MessageBox.Show("Sales Man Field is Required");
			CmbSalesMan.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCommissionType).ActiveRow == null)
		{
			MessageBox.Show("Commission Type Field is Required");
			CmbCommissionType.Focus();
			return false;
		}
		if (txtCommRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtCommRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Commission Rate Field is Required");
			txtCommRate.Focus();
			return false;
		}
		return true;
	}

	private void BtnAllocateCustomers_Click(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (GrdUnAllocatedAccounts.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked row first");
			}
			if (MessageBox.Show("Are you sure to Allocate Customer's?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No || !FormValidation())
			{
				return;
			}
			CustomerAllocateToSalesMan obj = new CustomerAllocateToSalesMan();
			obj.customerAllocateToSalesMenList = new List<CustomerAllocateToSalesMan>();
			GridEXRow[] checkedRows = GrdUnAllocatedAccounts.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				CustomerAllocateToSalesMan Allocate = new CustomerAllocateToSalesMan();
				Allocate.OrganizationId = UserAccount.OrganizationId;
				Allocate.CompanyId = UserAccount.CompanyId;
				if (CbIsActive.Checked)
				{
					Allocate.IsActive = true;
				}
				else
				{
					Allocate.IsActive = false;
				}
				Allocate.EffectiveDate = txtEffectiveDate.Value;
				Allocate.SalesManId = Conversion.ToInt(CmbSalesMan.Value);
				Allocate.CommType = ((Control)(object)CmbCommissionType).Text;
				Allocate.CommRate = Conversion.ToDouble((object)txtCommRate.Text);
				Allocate.CustomerId = Conversion.ToInt(item.Cells["Id"].Value);
				obj.customerAllocateToSalesMenList.Add(Allocate);
			}
			CustomerAllocateToSalesMan.Save(obj);
			MessageBox.Show("Record's Allocated Successfully");
			ReadAllocatedCustomers();
			ReadUnAllocatedCustomers();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDeAllocate_Click(object sender, EventArgs e)
	{
		try
		{
			if (GrdAllocatedAccounts.GetCheckedRows().Length == 0)
			{
				throw new Exception("Checked row first");
			}
			if (MessageBox.Show("Are you sure to UnAllocate Customer?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				string ids = null;
				GridEXRow[] checkedRows = GrdAllocatedAccounts.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					ids = ids + Conversion.ToString(item.Cells["CustomerId"].Value) + ",";
				}
				CustomerAllocateToSalesMan.DeleteById(ids);
				MessageBox.Show("Record's UnAllocated Successfully");
				ReadAllocatedCustomers();
				ReadUnAllocatedCustomers();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			ReadAllocatedCustomers();
			ReadUnAllocatedCustomers();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomersAllocateToSalesMan_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		GridEXLayout GrdUnAllocatedAccounts_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdAllocatedAccounts_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GrdUnAllocatedAccounts = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnAllocateCustomers = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GrdAllocatedAccounts = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnDeAllocate = new System.Windows.Forms.Button();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbCity = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CbCityWiseUnAllocatedCustomers = new System.Windows.Forms.CheckBox();
		this.CbCityWiseAllocatedCustomers = new System.Windows.Forms.CheckBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbSalesMan = new UltraCombo();
		this.CbIsActive = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.EFfectiveDate = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtEffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCommissionType = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdUnAllocatedAccounts).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedAccounts).BeginInit();
		this.panel7.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesMan).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionType).BeginInit();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(939, 638);
		this.panel1.TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(466, 121);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(471, 517);
		this.panel5.TabIndex = 3;
		this.GrdUnAllocatedAccounts.AllowEdit = (InheritableBoolean)2;
		this.GrdUnAllocatedAccounts.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdUnAllocatedAccounts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdUnAllocatedAccounts.DesignTimeLayout = GrdUnAllocatedAccounts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdUnAllocatedAccounts.DynamicFiltering = true;
		this.GrdUnAllocatedAccounts.FilterMode = (FilterMode)2;
		this.GrdUnAllocatedAccounts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdUnAllocatedAccounts.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).Location = new System.Drawing.Point(0, 65);
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).Name = "GrdUnAllocatedAccounts";
		((ScrollableBase)this.GrdUnAllocatedAccounts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).Size = new System.Drawing.Size(471, 452);
		((System.Windows.Forms.Control)(object)this.GrdUnAllocatedAccounts).TabIndex = 3;
		this.GrdUnAllocatedAccounts.TotalRow = (InheritableBoolean)1;
		this.GrdUnAllocatedAccounts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdUnAllocatedAccounts).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add(this.BtnAllocateCustomers);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 31);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(471, 34);
		this.panel6.TabIndex = 2;
		this.BtnAllocateCustomers.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnAllocateCustomers.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnAllocateCustomers.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnAllocateCustomers.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnAllocateCustomers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAllocateCustomers.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.BtnAllocateCustomers.Location = new System.Drawing.Point(5, 4);
		this.BtnAllocateCustomers.Margin = new System.Windows.Forms.Padding(0);
		this.BtnAllocateCustomers.Name = "BtnAllocateCustomers";
		this.BtnAllocateCustomers.Size = new System.Drawing.Size(25, 25);
		this.BtnAllocateCustomers.TabIndex = 0;
		this.BtnAllocateCustomers.UseVisualStyleBackColor = true;
		this.BtnAllocateCustomers.Click += new System.EventHandler(BtnAllocateCustomers_Click);
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(471, 31);
		this.label5.TabIndex = 1;
		this.label5.Text = "UnAllocated Customers";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts);
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 121);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(466, 517);
		this.panel4.TabIndex = 2;
		this.GrdAllocatedAccounts.AllowEdit = (InheritableBoolean)2;
		this.GrdAllocatedAccounts.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdAllocatedAccounts_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdAllocatedAccounts.DesignTimeLayout = GrdAllocatedAccounts_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdAllocatedAccounts.DynamicFiltering = true;
		this.GrdAllocatedAccounts.FilterMode = (FilterMode)2;
		this.GrdAllocatedAccounts.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdAllocatedAccounts.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).Location = new System.Drawing.Point(0, 65);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).Name = "GrdAllocatedAccounts";
		((ScrollableBase)this.GrdAllocatedAccounts).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).Size = new System.Drawing.Size(466, 452);
		((System.Windows.Forms.Control)(object)this.GrdAllocatedAccounts).TabIndex = 3;
		this.GrdAllocatedAccounts.TotalRow = (InheritableBoolean)1;
		this.GrdAllocatedAccounts.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdAllocatedAccounts).VisualStyle = (VisualStyle)4;
		this.panel7.Controls.Add(this.btnDeAllocate);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 31);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(466, 34);
		this.panel7.TabIndex = 2;
		this.btnDeAllocate.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeAllocate.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeAllocate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeAllocate.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeAllocate.Location = new System.Drawing.Point(6, 4);
		this.btnDeAllocate.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeAllocate.Name = "btnDeAllocate";
		this.btnDeAllocate.Size = new System.Drawing.Size(25, 25);
		this.btnDeAllocate.TabIndex = 0;
		this.btnDeAllocate.UseVisualStyleBackColor = true;
		this.btnDeAllocate.Click += new System.EventHandler(btnDeAllocate_Click);
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(466, 31);
		this.label4.TabIndex = 0;
		this.label4.Text = "Allocated Customers";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 38);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(939, 83);
		this.panel3.TabIndex = 1;
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.CbCityWiseUnAllocatedCustomers);
		this.groupBox2.Controls.Add(this.CbCityWiseAllocatedCustomers);
		this.groupBox2.Location = new System.Drawing.Point(615, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(321, 78);
		this.groupBox2.TabIndex = 112;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filter By City";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(262, 50);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(50, 23);
		this.btnShow.TabIndex = 13;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(78, 10);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(204, 24);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 15);
		this.label2.TabIndex = 12;
		this.label2.Text = "City Name";
		this.CbCityWiseUnAllocatedCustomers.AutoSize = true;
		this.CbCityWiseUnAllocatedCustomers.Location = new System.Drawing.Point(79, 56);
		this.CbCityWiseUnAllocatedCustomers.Name = "CbCityWiseUnAllocatedCustomers";
		this.CbCityWiseUnAllocatedCustomers.Size = new System.Drawing.Size(180, 17);
		this.CbCityWiseUnAllocatedCustomers.TabIndex = 2;
		this.CbCityWiseUnAllocatedCustomers.Text = "CityWise UnAllocated Customers";
		this.CbCityWiseUnAllocatedCustomers.UseVisualStyleBackColor = true;
		this.CbCityWiseAllocatedCustomers.AutoSize = true;
		this.CbCityWiseAllocatedCustomers.Location = new System.Drawing.Point(79, 38);
		this.CbCityWiseAllocatedCustomers.Name = "CbCityWiseAllocatedCustomers";
		this.CbCityWiseAllocatedCustomers.Size = new System.Drawing.Size(166, 17);
		this.CbCityWiseAllocatedCustomers.TabIndex = 1;
		this.CbCityWiseAllocatedCustomers.Text = "CityWise Allocated Customers";
		this.CbCityWiseAllocatedCustomers.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalesMan);
		this.groupBox1.Controls.Add(this.CbIsActive);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtCommRate);
		this.groupBox1.Controls.Add(this.EFfectiveDate);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtEffectiveDate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionType);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(606, 77);
		this.groupBox1.TabIndex = 111;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sales Man Fields";
		this.CmbSalesMan.AllowNull = (DefaultableBoolean)1;
		this.CmbSalesMan.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalesMan.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalesMan.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesMan).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalesMan).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalesMan).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalesMan.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).Location = new System.Drawing.Point(93, 47);
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).Name = "CmbSalesMan";
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).Size = new System.Drawing.Size(187, 24);
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSalesMan).Leave += new System.EventHandler(CmbSalesMan_Leave);
		this.CbIsActive.AutoSize = true;
		this.CbIsActive.Location = new System.Drawing.Point(536, 54);
		this.CbIsActive.Name = "CbIsActive";
		this.CbIsActive.Size = new System.Drawing.Size(67, 17);
		this.CbIsActive.TabIndex = 4;
		this.CbIsActive.Text = "Is Active";
		this.CbIsActive.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 52);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 10;
		this.label3.Text = "Sales Man";
		this.txtCommRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommRate.Location = new System.Drawing.Point(414, 48);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(116, 23);
		this.txtCommRate.TabIndex = 3;
		this.txtCommRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommRate_KeyPress);
		this.EFfectiveDate.AutoSize = true;
		this.EFfectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.EFfectiveDate.Location = new System.Drawing.Point(6, 25);
		this.EFfectiveDate.Name = "EFfectiveDate";
		this.EFfectiveDate.Size = new System.Drawing.Size(81, 15);
		this.EFfectiveDate.TabIndex = 105;
		this.EFfectiveDate.Text = "Effective Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(296, 52);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(102, 15);
		this.label7.TabIndex = 109;
		this.label7.Text = "Commission Rate";
		this.txtEffectiveDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectiveDate.CustomFormat = "dd-MMM-yy";
		this.txtEffectiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectiveDate.Location = new System.Drawing.Point(93, 22);
		this.txtEffectiveDate.Name = "txtEffectiveDate";
		this.txtEffectiveDate.Size = new System.Drawing.Size(187, 21);
		this.txtEffectiveDate.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(296, 25);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(102, 15);
		this.label6.TabIndex = 2;
		this.label6.Text = "Commission Type";
		this.CmbCommissionType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Location = new System.Drawing.Point(414, 20);
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Name = "CmbCommissionType";
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).Size = new System.Drawing.Size(116, 24);
		((System.Windows.Forms.Control)(object)this.CmbCommissionType).TabIndex = 2;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(939, 38);
		this.panel2.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(313, 25);
		this.label1.TabIndex = 0;
		this.label1.Text = "Customer Allocation To Sales Man";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(939, 638);
		base.Controls.Add(this.panel1);
		base.Name = "CustomersAllocateToSalesMan";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(CustomersAllocateToSalesMan_FormClosing);
		base.Load += new System.EventHandler(CustomersAllocateToSalesMan_Load);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdUnAllocatedAccounts).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdAllocatedAccounts).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalesMan).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		base.ResumeLayout(false);
	}
}
