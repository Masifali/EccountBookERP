using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Tax.Definitions;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Tax.Definitions;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Taxation;
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
namespace Architecture.WinApp.Tax_Definition
{

public class frmTaxTypeAndBusinessTypeMapping : Form
{
	public UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtHistoryDbCall = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private IContainer components;

	private ToolStrip tsItemType;

	private ToolStripButton btnRefresh;

	private ToolStripButton BtnfrmTaxTypeAndBusinessTypeMapping;

	private Panel panel2;

	private Panel panel1;

	internal GridEX GridAllocated;

	private Button btnDeleteTemplateII;

	private Label labelAllocateGridTemplateII;

	private Panel panel5;

	internal GridEX GridUnAllocated;

	private Button BtnSaveTemplateII;

	private Label labelUnAllocateGridTemplateII;

	private ToolStripButton BtnDefineTaxType;

	private Panel panel10;

	internal GridEX grdBusinessType;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	private Panel panel8;

	private Panel panel6;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX grdFormHistory;

	private Label label1;

	public frmTaxTypeAndBusinessTypeMapping(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmItemsCustomizedGroups_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(980, 500);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			BtnSaveTemplateII.Enabled = formright.DoHaveSaveRight;
			btnDeleteTemplateII.Enabled = formright.DoHaveUpdateRights || formright.DoHaveCanDelete;
			BusinessTypeGridFill();
			HistoryDbCall();
			HistoryGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BusinessTypeGridFill()
	{
		try
		{
			DataTable dtSource = BusinessType.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0, true);
			if (dtSource.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("BusinessTypeName");
				dt.Columns.Add("Description");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtSource.Rows.Count; i++)
				{
					dt.Rows.Add(dtSource.Rows[i]["BusinessTypeID"], dtSource.Rows[i]["BusinessTypeName"], dtSource.Rows[i]["Description"], dtSource.Rows[i]["IsActive"]);
				}
				grdBusinessType.DataSource = dt;
				grdBusinessType.RetrieveStructure();
				GridBusinessTypeSetting();
			}
			else
			{
				grdBusinessType.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridBusinessTypeSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "IsActive" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdBusinessType, 2, 3, hiddenColumns);
			grdBusinessType.HideSelection = (HideSelection)2;
			grdBusinessType.RootTable.Columns["BusinessTypeName"].Caption = "Business Type";
			grdBusinessType.RootTable.Columns["Description"].Caption = "Description";
			grdBusinessType.RootTable.Columns["BusinessTypeName"].Width = 130;
			grdBusinessType.RootTable.Columns["Description"].Width = 300;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdItemName_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdBusinessType.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int businessTypeId = Conversion.ToInt(r.Cells["Id"].Value);
				string businessType = Conversion.ToString(r.Cells["BusinessTypeName"].Value);
				UnAllocatedTaxTypeGridFill(businessTypeId);
				AllocatedTaxTypeGridFill(businessTypeId);
				labelUnAllocateGridTemplateII.Text = "UnAllocated Tax Types Of Business Type : '" + businessType + "'";
				labelAllocateGridTemplateII.Text = "Allocated Tax Types Of Business Type : '" + businessType + "'";
				r.CheckState = (RowCheckState)0;
			}
			else
			{
				GridUnAllocated.DataSource = null;
				GridAllocated.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UnAllocatedTaxTypeGridFill(int businessTypeId)
	{
		try
		{
			DataTable dtSource = TaxTypeAndBusinessTypeMapping.TaxTypeAndBusinessTypeMapping_UnAllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, businessTypeId);
			if (dtSource.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("BusinessTypeId");
				dt.Columns.Add("BusinessTypeName");
				dt.Columns.Add("TaxTypeId");
				dt.Columns.Add("TaxTypeName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtSource.Rows.Count; i++)
				{
					dt.Rows.Add(dtSource.Rows[i]["Id"], dtSource.Rows[i]["BusinessTypeId"], dtSource.Rows[i]["BusinessTypeName"], dtSource.Rows[i]["TaxTypeId"], dtSource.Rows[i]["TaxTypeName"], dtSource.Rows[i]["IsActive"]);
				}
				GridUnAllocated.DataSource = dt;
				GridUnAllocated.RetrieveStructure();
				GridSetting(GridUnAllocated);
			}
			else
			{
				GridUnAllocated.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllocatedTaxTypeGridFill(int businessTypeId)
	{
		try
		{
			DataTable dtSource = TaxTypeAndBusinessTypeMapping.TaxTypeAndBusinessTypeMapping_AllocatedData(UserAccount.OrganizationId, UserAccount.CompanyId, businessTypeId);
			if (dtSource.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("BusinessTypeId");
				dt.Columns.Add("BusinessTypeName");
				dt.Columns.Add("TaxTypeId");
				dt.Columns.Add("TaxTypeName");
				dt.Columns.Add("IsActive");
				for (int i = 0; i < dtSource.Rows.Count; i++)
				{
					dt.Rows.Add(dtSource.Rows[i]["Id"], dtSource.Rows[i]["BusinessTypeId"], dtSource.Rows[i]["BusinessTypeName"], dtSource.Rows[i]["TaxTypeId"], dtSource.Rows[i]["TaxTypeName"], dtSource.Rows[i]["IsActive"]);
				}
				GridAllocated.DataSource = dt;
				GridAllocated.RetrieveStructure();
				GridSetting(GridAllocated);
			}
			else
			{
				GridAllocated.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "BusinessTypeId", "BusinessTypeName", "TaxTypeId", "IsActive" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 2, 3, hiddenColumns);
			gridEX.RootTable.Columns["TaxTypeName"].Caption = "Tax Type";
			gridEX.RootTable.Columns["TaxTypeName"].Width = 150;
			GridEX_Helper.AddSelectButton(gridEX, "Select", 30, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnSaveTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			InsertTemplateII(GridUnAllocated, "Allocate");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertTemplateII(GridEX grd, string message)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		try
		{
			if (grd.GetCheckedRows().Length == 0)
			{
				throw new Exception("Check rows to " + message);
			}
			TaxTypeAndBusinessTypeMapping obj = new TaxTypeAndBusinessTypeMapping();
			obj.TaxTypeAndBusinessTypeMappingList = new List<TaxTypeAndBusinessTypeMapping>();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				TaxTypeAndBusinessTypeMapping item = new TaxTypeAndBusinessTypeMapping();
				item.Id = Conversion.ToInt(r.Cells["Id"].Value);
				item.BusinessTypeId = Conversion.ToInt(r.Cells["BusinessTypeId"].Value);
				item.TaxTypeId = Conversion.ToInt(r.Cells["TaxTypeId"].Value);
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.IsActive = true;
				item.EntryUserId = UserAccount.ID;
				item.ModifyUserId = UserAccount.ID;
				item.EntryDate = DateTime.Now;
				item.ModifyDate = DateTime.Now;
				obj.TaxTypeAndBusinessTypeMappingList.Add(item);
			}
			TaxTypeAndBusinessTypeMapping.Save(obj);
			MessageBox.Show(message + " Successfully");
			grdItemName_SelectionChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			InsertTemplateII(GridAllocated, "Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDeleteTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			if (GridAllocated.GetCheckedRows().Length == 0)
			{
				throw new Exception("Select rows first");
			}
			int businessTypeId = Conversion.ToInt((grdBusinessType.CurrentRow ?? throw new Exception("Select Business Type First")).Cells["Id"].Value);
			string ids = "";
			GridEXRow[] checkedRows = GridAllocated.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				ids = ids + Conversion.ToString(item.Cells["TaxTypeId"].Value) + ",";
			}
			TaxTypeAndBusinessTypeMapping.UpdateStatus(UserAccount.OrganizationId, UserAccount.CompanyId, ids, businessTypeId, UserAccount.ID);
			MessageBox.Show("UnAllocated Successfully");
			grdItemName_SelectionChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistoryDbCall.Clear();
			TaxTypeAndBusinessTypeMapping obj = new TaxTypeAndBusinessTypeMapping();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.IsActive = true;
			dtHistoryDbCall = TaxTypeAndBusinessTypeMapping.FormHistory(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridBind()
	{
		try
		{
			if (dtHistoryDbCall == null || dtHistoryDbCall.Rows.Count <= 0)
			{
				return;
			}
			if (dtHistoryGrid.Columns.Count == 0)
			{
				dtHistoryGrid.Columns.Add("Id", typeof(int));
				dtHistoryGrid.Columns.Add("BusinessTypeId", typeof(int));
				dtHistoryGrid.Columns.Add("BusinessTypeName", typeof(string));
				dtHistoryGrid.Columns.Add("TaxTypeId", typeof(int));
				dtHistoryGrid.Columns.Add("TaxName", typeof(string));
			}
			dtHistoryGrid.Clear();
			foreach (DataRow row in dtHistoryDbCall.Rows)
			{
				dtHistoryGrid.Rows.Add(Conversion.ToInt(row["Id"]), Conversion.ToInt(row["BusinessTypeId"]), Conversion.ToString(row["BusinessTypeName"]), Conversion.ToInt(row["TaxTypeId"]), Conversion.ToString(row["TaxName"]));
			}
			grdFormHistory.DataSource = dtHistoryGrid;
			grdFormHistory.RetrieveStructure();
			grdSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSetting()
	{
		try
		{
			grdFormHistory.RootTable.Columns["Id"].Visible = false;
			grdFormHistory.RootTable.Columns["TaxTypeId"].Visible = false;
			grdFormHistory.RootTable.Columns["BusinessTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdFormHistory, 2);
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
			BusinessTypeGridFill();
			HistoryDbCall();
			HistoryGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmTaxTypeAndBusinessTypeMapping_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnItemDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new InvfrmAddTaxSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnfrmTaxTypeAndBusinessTypeMapping_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineBusinessType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmItemsCustomizedGroups_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.R)
		{
			btnRefresh_Click(null, null);
		}
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (e.Control && e.Alt)
		{
			MakeShortCutKeys();
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			((Control)(object)grdBusinessType).Focus();
		}
		if (e.Control && e.KeyCode == Keys.U)
		{
			BtnUpdateTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.A)
		{
			BtnSaveTemplateII_Click(null, null);
		}
		if (e.Control && (e.KeyCode == Keys.D || e.KeyCode == Keys.Delete))
		{
			btnDeleteTemplateII_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			((Control)(object)grdBusinessType).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (Conversion.ToString(base.ActiveControl.Tag) == "UnAllocatedGrid")
			{
				((Control)(object)GridAllocated).Focus();
			}
			else
			{
				((Control)(object)GridUnAllocated).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Left)
		{
			((Control)(object)GridUnAllocated).Focus();
		}
		if (e.Control && e.KeyCode == Keys.Right)
		{
			((Control)(object)GridAllocated).Focus();
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+A/Ctrl+S", "For Press Allocate Button");
			dt.Rows.Add("Ctrl+U", "For Press Update Button");
			dt.Rows.Add("Ctrl+D/Ctrl+Delete", "For Press UnAllocate Button");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Item Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
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
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Tax_Definition.frmTaxTypeAndBusinessTypeMapping));
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnDefineTaxType = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnfrmTaxTypeAndBusinessTypeMapping = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.GridAllocated = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnDeleteTemplateII = new System.Windows.Forms.Button();
		this.labelAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.GridUnAllocated = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnSaveTemplateII = new System.Windows.Forms.Button();
		this.labelUnAllocateGridTemplateII = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdBusinessType = new GridEX();
		this.label7 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdFormHistory = new GridEX();
		this.label1 = new System.Windows.Forms.Label();
		this.tsItemType.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).BeginInit();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBusinessType).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnRefresh, this.toolStripSeparator3, this.BtnDefineTaxType, this.toolStripSeparator2, this.BtnfrmTaxTypeAndBusinessTypeMapping, this.btnshortcutkeys, this.toolStripSeparator1 });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1034, 27);
		this.tsItemType.TabIndex = 2;
		this.tsItemType.Text = "toolStrip4";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnDefineTaxType.Image = (System.Drawing.Image)resources.GetObject("BtnDefineTaxType.Image");
		this.BtnDefineTaxType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineTaxType.Name = "BtnDefineTaxType";
		this.BtnDefineTaxType.Size = new System.Drawing.Size(164, 24);
		this.BtnDefineTaxType.Text = "Define Tax Schedule";
		this.BtnDefineTaxType.Click += new System.EventHandler(BtnItemDefine_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnfrmTaxTypeAndBusinessTypeMapping.Image = (System.Drawing.Image)resources.GetObject("BtnfrmTaxTypeAndBusinessTypeMapping.Image");
		this.BtnfrmTaxTypeAndBusinessTypeMapping.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnfrmTaxTypeAndBusinessTypeMapping.Name = "BtnfrmTaxTypeAndBusinessTypeMapping";
		this.BtnfrmTaxTypeAndBusinessTypeMapping.Size = new System.Drawing.Size(168, 24);
		this.BtnfrmTaxTypeAndBusinessTypeMapping.Text = "Define Business type";
		this.BtnfrmTaxTypeAndBusinessTypeMapping.Click += new System.EventHandler(BtnfrmTaxTypeAndBusinessTypeMapping_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdFormHistory);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel1.Location = new System.Drawing.Point(700, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(331, 431);
		this.panel1.TabIndex = 80;
		this.GridAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridAllocated.FilterMode = (FilterMode)2;
		this.GridAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridAllocated.GroupByBoxVisible = false;
		this.GridAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Location = new System.Drawing.Point(0, 306);
		((System.Windows.Forms.Control)(object)this.GridAllocated).Name = "GridAllocated";
		((ScrollableBase)this.GridAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridAllocated).Size = new System.Drawing.Size(250, 125);
		((System.Windows.Forms.Control)(object)this.GridAllocated).TabIndex = 64;
		this.GridAllocated.TotalRow = (InheritableBoolean)1;
		this.GridAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridAllocated).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnDeleteTemplateII);
		this.panel8.Controls.Add(this.labelAllocateGridTemplateII);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 267);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(250, 39);
		this.panel8.TabIndex = 69;
		this.btnDeleteTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDeleteTemplateII.BackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnDeleteTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.btnDeleteTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDeleteTemplateII.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDeleteTemplateII.Location = new System.Drawing.Point(213, 5);
		this.btnDeleteTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.btnDeleteTemplateII.Name = "btnDeleteTemplateII";
		this.btnDeleteTemplateII.Size = new System.Drawing.Size(33, 30);
		this.btnDeleteTemplateII.TabIndex = 5;
		this.btnDeleteTemplateII.UseVisualStyleBackColor = false;
		this.btnDeleteTemplateII.Click += new System.EventHandler(btnDeleteTemplateII_Click);
		this.labelAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelAllocateGridTemplateII.Location = new System.Drawing.Point(2, 0);
		this.labelAllocateGridTemplateII.Name = "labelAllocateGridTemplateII";
		this.labelAllocateGridTemplateII.Size = new System.Drawing.Size(205, 35);
		this.labelAllocateGridTemplateII.TabIndex = 4;
		this.labelAllocateGridTemplateII.Text = "Allocated TaxTypes Of BusinessType : Company\r\n";
		this.labelAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridAllocated);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocated);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(450, 30);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(250, 431);
		this.panel5.TabIndex = 79;
		this.GridUnAllocated.AllowEdit = (InheritableBoolean)2;
		this.GridUnAllocated.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Dock = System.Windows.Forms.DockStyle.Top;
		this.GridUnAllocated.FilterMode = (FilterMode)2;
		this.GridUnAllocated.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocated.GroupByBoxVisible = false;
		this.GridUnAllocated.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Name = "GridUnAllocated";
		((ScrollableBase)this.GridUnAllocated).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.GridUnAllocated).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.GridUnAllocated).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Size = new System.Drawing.Size(250, 228);
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.GridUnAllocated).Tag = "UnAllocatedGrid";
		this.GridUnAllocated.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocated.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocated).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.BtnSaveTemplateII);
		this.panel6.Controls.Add(this.labelUnAllocateGridTemplateII);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(250, 39);
		this.panel6.TabIndex = 68;
		this.BtnSaveTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnSaveTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnSaveTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnSaveTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnSaveTemplateII.Image = Architecture.WinApp.Properties.Resources.Forward;
		this.BtnSaveTemplateII.Location = new System.Drawing.Point(213, 5);
		this.BtnSaveTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnSaveTemplateII.Name = "BtnSaveTemplateII";
		this.BtnSaveTemplateII.Size = new System.Drawing.Size(33, 30);
		this.BtnSaveTemplateII.TabIndex = 66;
		this.BtnSaveTemplateII.UseVisualStyleBackColor = false;
		this.BtnSaveTemplateII.Click += new System.EventHandler(BtnSaveTemplateII_Click);
		this.labelUnAllocateGridTemplateII.BackColor = System.Drawing.Color.Teal;
		this.labelUnAllocateGridTemplateII.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.labelUnAllocateGridTemplateII.ForeColor = System.Drawing.Color.White;
		this.labelUnAllocateGridTemplateII.Location = new System.Drawing.Point(6, 0);
		this.labelUnAllocateGridTemplateII.Name = "labelUnAllocateGridTemplateII";
		this.labelUnAllocateGridTemplateII.Size = new System.Drawing.Size(205, 35);
		this.labelUnAllocateGridTemplateII.TabIndex = 3;
		this.labelUnAllocateGridTemplateII.Text = "UnAllocated TaxTypes Of BusinessType : Company\r\n";
		this.labelUnAllocateGridTemplateII.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdBusinessType);
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(450, 431);
		this.panel10.TabIndex = 81;
		this.grdBusinessType.AllowEdit = (InheritableBoolean)2;
		this.grdBusinessType.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdBusinessType.FilterMode = (FilterMode)2;
		this.grdBusinessType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBusinessType.GroupByBoxVisible = false;
		this.grdBusinessType.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Name = "grdBusinessType";
		((ScrollableBase)this.grdBusinessType).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdBusinessType).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdBusinessType).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Size = new System.Drawing.Size(450, 392);
		((System.Windows.Forms.Control)(object)this.grdBusinessType).TabIndex = 67;
		((System.Windows.Forms.Control)(object)this.grdBusinessType).Tag = "UnAllocatedGrid";
		this.grdBusinessType.TotalRow = (InheritableBoolean)1;
		this.grdBusinessType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBusinessType).VisualStyle = (VisualStyle)4;
		this.grdBusinessType.SelectionChanged += new System.EventHandler(grdItemName_SelectionChanged);
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Top;
		this.label7.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(0, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(450, 39);
		this.label7.TabIndex = 3;
		this.label7.Text = "Business Type";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 30);
		this.panel2.TabIndex = 3;
		this.grdFormHistory.AllowEdit = (InheritableBoolean)2;
		this.grdFormHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFormHistory.FilterMode = (FilterMode)2;
		this.grdFormHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFormHistory.GroupByBoxVisible = false;
		this.grdFormHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Location = new System.Drawing.Point(0, 39);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Name = "grdFormHistory";
		((ScrollableBase)this.grdFormHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdFormHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdFormHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Size = new System.Drawing.Size(331, 392);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).TabIndex = 69;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Tag = "UnAllocatedGrid";
		this.grdFormHistory.TotalRow = (InheritableBoolean)1;
		this.grdFormHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFormHistory).VisualStyle = (VisualStyle)4;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(331, 39);
		this.label1.TabIndex = 68;
		this.label1.Text = "History";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 461);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel10);
		base.Controls.Add(this.panel2);
		base.KeyPreview = true;
		base.Name = "frmTaxTypeAndBusinessTypeMapping";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmTaxTypeAndBusinessTypeMapping_FormClosing);
		base.Load += new System.EventHandler(frmItemsCustomizedGroups_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmItemsCustomizedGroups_KeyDown);
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridAllocated).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocated).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBusinessType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).EndInit();
		base.ResumeLayout(false);
	}
}

}
