using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Configurations
{

public class frmDefineAppModules : Form
{
	private int RecId;

	private DataTable dtParentModules = new DataTable();

	private DataTable dtModuleType = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private Panel panel5;

	private Label label23;

	internal GridEX grd;

	private Label label2;

	private TextBox txtModuleDescription;

	private Label label1;

	internal UltraCombo CmbModuleType;

	private Label label3;

	internal UltraCombo CmbAppId;

	private Label label5;

	private TextBox txtIconUrl;

	private Label label4;

	private TextBox txtSortNo;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnUpdateByHistory;

	public frmDefineAppModules(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void DefineCountry_Load(object sender, EventArgs e)
	{
		try
		{
			ModuleTypeBind();
			AppIdBind();
			FormHistory();
			txtModuleDescription.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			AppModules obj = new AppModules();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.ModuleDescription = Conversion.ToString((object)txtModuleDescription.Text.Trim());
			obj.ModelMenuControllName = obj.ModuleDescription;
			obj.ModuleTypeId = Conversion.ToInt(CmbModuleType.Value);
			obj.AppId = Conversion.ToInt(CmbAppId.Value);
			obj.IconUrl = Conversion.ToString((object)txtIconUrl.Text.Trim());
			obj.SortNo = Conversion.ToInt((object)txtSortNo.Text);
			AppModules.save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			txtModuleDescription.Focus();
			txtModuleDescription.Text = string.Empty;
			((Control)(object)CmbAppId).Text = string.Empty;
			txtIconUrl.Text = string.Empty;
			txtSortNo.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			FormHistory();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not found for update");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdcountrydefine_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			AppModules obj = AppModules.GetById(RecId);
			if (obj != null)
			{
				txtModuleDescription.Text = obj.ModuleDescription;
				CmbModuleType.Value = obj.ModuleTypeId;
				CmbAppId.Value = obj.AppId;
				txtIconUrl.Text = obj.IconUrl;
				txtSortNo.Text = obj.SortNo.ToString();
				CmbModuleType.Focus();
				btnsave.Visible = false;
				btnUpdate.Visible = true;
			}
			else
			{
				RecId = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormHistory()
	{
		try
		{
			dtHistory.Rows.Clear();
			dtHistory = AppModules.ReadAll();
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ModuleDescription");
				dt.Columns.Add("AppId", typeof(int));
				dt.Columns.Add("ModuleTypeId", typeof(int));
				dt.Columns.Add("IconUrl");
				dt.Columns.Add("SortNo", typeof(int));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ModuleDescription"], dtHistory.Rows[i]["AppId"], dtHistory.Rows[i]["ModuleTypeId"], dtHistory.Rows[i]["IconUrl"], dtHistory.Rows[i]["SortNo"]);
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSetting();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			GrdCombosBind();
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ModuleDescription"].Width = 250;
			grd.RootTable.Columns["AppId"].Width = 100;
			grd.RootTable.Columns["ModuleTypeId"].Width = 70;
			grd.RootTable.Columns["IconUrl"].Width = 160;
			grd.RootTable.Columns["SortNo"].Width = 70;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdCombosBind()
	{
		try
		{
			grd.RootTable.Columns["AppId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AppId"].EditType = (EditType)5;
			grd.RootTable.Columns["AppId"].HasValueList = true;
			grd.RootTable.Columns["AppId"].LimitToList = true;
			grd.RootTable.Columns["AppId"].Caption = "Parent Module";
			grd.RootTable.Columns["AppId"].ValueList.PopulateValueList((IEnumerable)dtParentModules.DefaultView, "Id", "App");
			grd.RootTable.Columns["ModuleTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ModuleTypeId"].EditType = (EditType)5;
			grd.RootTable.Columns["ModuleTypeId"].HasValueList = true;
			grd.RootTable.Columns["ModuleTypeId"].LimitToList = true;
			grd.RootTable.Columns["ModuleTypeId"].Caption = "Module Type";
			grd.RootTable.Columns["ModuleTypeId"].ValueList.PopulateValueList((IEnumerable)dtModuleType.DefaultView, "Id", "type");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtModuleDescription.Text.Trim() == string.Empty)
		{
			MessageBox.Show("District Name Field is Required");
			txtModuleDescription.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAppId).ActiveRow == null || Conversion.ToInt(CmbAppId.Value) == 0)
		{
			MessageBox.Show("Parent Module Field is Required");
			CmbAppId.Focus();
			return false;
		}
		if (((UltraGridBase)CmbModuleType).ActiveRow == null || Conversion.ToInt(CmbModuleType.Value) == 0)
		{
			MessageBox.Show("Module Type Field is Required");
			CmbModuleType.Focus();
			return false;
		}
		return true;
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void DefineCountry_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.N)
		{
			btnnew_Click(null, null);
		}
		if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
		{
			RecId = 0;
			Insert();
		}
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			Insert();
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void ModuleTypeBind()
	{
		try
		{
			dtModuleType.Rows.Clear();
			dtModuleType = CommonServices.StaticColumnsService("ModuleType");
			if (dtModuleType.Rows.Count > 0)
			{
				DDL.BindDDL(dtModuleType, CmbModuleType, "Id", "type", "Module Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AppIdBind()
	{
		try
		{
			dtParentModules.Rows.Clear();
			dtParentModules = AppModules.ReadAllApps();
			if (dtParentModules.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtParentModules, CmbAppId, "Id", "App", "App", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDefineAppModules_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ModuleTypeBind();
			AppIdBind();
			GrdCombosBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateByHistory_Click(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			grd.UpdateData();
			List<DataRow> SaveList = ((DataTable)grd.DataSource).AsEnumerable().Where(delegate(DataRow originalRow)
			{
				DataRow dataRow = dtHistory.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
				return dataRow != null && (originalRow.Field<string>("ModuleDescription") != dataRow.Field<string>("ModuleDescription") || originalRow.Field<int>("AppId") != dataRow.Field<int>("AppId") || originalRow.Field<int>("ModuleTypeId") != dataRow.Field<int>("ModuleTypeId") || originalRow.Field<string>("IconUrl") != dataRow.Field<string>("IconUrl") || originalRow.Field<int>("SortNo") != dataRow.Field<int>("SortNo"));
			}).ToList();
			AppModules obj = new AppModules();
			obj.AppModuleList = new List<AppModules>();
			if (SaveList.Count > 0)
			{
				for (int i = 0; i < SaveList.Count; i++)
				{
					AppModules d = new AppModules();
					d.Id = Conversion.ToInt(SaveList[i]["Id"]);
					if (d.Id == 0)
					{
						throw new Exception("Record Id not found");
					}
					d.ModuleDescription = Conversion.ToString(SaveList[i]["ModuleDescription"]).Trim();
					if (d.ModuleDescription == null || d.ModuleDescription == string.Empty)
					{
						throw new Exception("Module Description not found");
					}
					d.ModelMenuControllName = Conversion.ToString(SaveList[i]["ModuleDescription"]);
					d.AppId = Conversion.ToInt(SaveList[i]["AppId"]);
					if (d.AppId == 0)
					{
						throw new Exception("Parent Module not found of Module Description:" + d.ModuleDescription);
					}
					d.ModuleTypeId = Conversion.ToInt(SaveList[i]["ModuleTypeId"]);
					if (d.ModuleTypeId == 0)
					{
						throw new Exception("Module Type not found of Module Description:" + d.ModuleDescription);
					}
					d.IconUrl = Conversion.ToString(SaveList[i]["IconUrl"]);
					d.SortNo = Conversion.ToInt(SaveList[i]["SortNo"]);
					obj.AppModuleList.Add(d);
				}
				AppModules.MultiRecordUpdate(obj);
				MessageBox.Show("Records have been updated Successfully!");
				FormHistory();
				return;
			}
			throw new Exception("No change found for update!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSortNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtIconUrl = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtSortNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbAppId = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbModuleType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtModuleDescription = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateByHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAppId).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(771, 490);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 144);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(771, 346);
		this.panel4.TabIndex = 3;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(771, 346);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grdcountrydefine_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtIconUrl);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.txtSortNo);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAppId);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbModuleType);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.txtModuleDescription);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(771, 89);
		this.panel5.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(10, 64);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(49, 15);
		this.label5.TabIndex = 11;
		this.label5.Text = "Icon Url";
		this.txtIconUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIconUrl.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtIconUrl.Location = new System.Drawing.Point(141, 60);
		this.txtIconUrl.Name = "txtIconUrl";
		this.txtIconUrl.Size = new System.Drawing.Size(170, 23);
		this.txtIconUrl.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(315, 65);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 9;
		this.label4.Text = "Sort No";
		this.txtSortNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSortNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSortNo.Location = new System.Drawing.Point(394, 61);
		this.txtSortNo.Name = "txtSortNo";
		this.txtSortNo.Size = new System.Drawing.Size(123, 23);
		this.txtSortNo.TabIndex = 4;
		this.txtSortNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSortNo_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(10, 38);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(85, 15);
		this.label3.TabIndex = 8;
		this.label3.Text = "Parent Module";
		this.CmbAppId.AllowNull = (DefaultableBoolean)1;
		this.CmbAppId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAppId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAppId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAppId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAppId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAppId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAppId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbAppId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAppId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbAppId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAppId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAppId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAppId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAppId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAppId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAppId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAppId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAppId).Location = new System.Drawing.Point(141, 32);
		((System.Windows.Forms.Control)(object)this.CmbAppId).Name = "CmbAppId";
		((System.Windows.Forms.Control)(object)this.CmbAppId).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbAppId).TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(315, 38);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Module Type";
		this.CmbModuleType.AllowNull = (DefaultableBoolean)1;
		this.CmbModuleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbModuleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbModuleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbModuleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbModuleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbModuleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbModuleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbModuleType).Location = new System.Drawing.Point(394, 33);
		((System.Windows.Forms.Control)(object)this.CmbModuleType).Name = "CmbModuleType";
		((System.Windows.Forms.Control)(object)this.CmbModuleType).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbModuleType).TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(10, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(112, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Module Description";
		this.txtModuleDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtModuleDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtModuleDescription.Location = new System.Drawing.Point(141, 7);
		this.txtModuleDescription.Name = "txtModuleDescription";
		this.txtModuleDescription.Size = new System.Drawing.Size(376, 23);
		this.txtModuleDescription.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(771, 28);
		this.panel3.TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(200, 23);
		this.label23.TabIndex = 25;
		this.label23.Text = "Define App Modules";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(771, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnUpdateByHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(771, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnUpdateByHistory.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnUpdateByHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateByHistory.Name = "btnUpdateByHistory";
		this.btnUpdateByHistory.Size = new System.Drawing.Size(150, 24);
		this.btnUpdateByHistory.Text = "Update by History";
		this.btnUpdateByHistory.Click += new System.EventHandler(btnUpdateByHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(771, 490);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDefineAppModules";
		this.Text = "Define District";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmDefineAppModules_FormClosed);
		base.Load += new System.EventHandler(DefineCountry_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DefineCountry_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAppId).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbModuleType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
