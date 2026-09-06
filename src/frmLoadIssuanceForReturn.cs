using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.StoreManagement
{

public class frmLoadIssuanceForReturn : Form
{
	private bool FormInitialized;

	private DataTable dtData = new DataTable();

	private DataTable dtMain = new DataTable();

	private DataTable dtDetail = new DataTable();

	public DataTable dtForLoad = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panelheading;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Button btnReset;

	private Label label2;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar2;

	public frmLoadIssuanceForReturn(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadGRN_Load_1(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtMain.Columns.Add("Id", typeof(int));
			dtMain.Columns.Add("DetailId", typeof(int));
			dtMain.Columns.Add("DocDate", typeof(DateTime));
			dtMain.Columns.Add("DocNo", typeof(int));
			dtMain.Columns.Add("DocumentType", typeof(string));
			dtMain.Columns.Add("ManualNo", typeof(string));
			dtMain.Columns.Add("TotalIssueQty", typeof(double));
			dtMain.Columns.Add("TotalReplacementQty", typeof(double));
			dtMain.Columns.Add("TotalBalanceQty", typeof(double));
			dtMain.Columns.Add("EntryDate", typeof(DateTime));
			dtMain.Columns.Add("EntryUser", typeof(string));
			dtMain.Columns.Add("ModifyDate", typeof(DateTime));
			dtMain.Columns.Add("ModifyUser", typeof(string));
			dtMain.Columns.Add("Remarks");
			grd.DataSource = dtMain;
			grd.RetrieveStructure();
			grdSettingsPending();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("DetailId", typeof(int));
			dtDetail.Columns.Add("Warehouse");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("PackUom");
			dtDetail.Columns.Add("ConditionStatus");
			dtDetail.Columns.Add("AssetName");
			dtDetail.Columns.Add("DepartmentName");
			dtDetail.Columns.Add("IssueQty", typeof(double));
			dtDetail.Columns.Add("ReplacementQty", typeof(double));
			dtDetail.Columns.Add("BalanceQty", typeof(double));
			dtDetail.Columns.Add("ItemRate", typeof(double));
			dtDetail.Columns.Add("ItemAmount", typeof(double));
			FromDate.Focus();
			PendingOrderForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingOrderForLoad()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		try
		{
			dtDetail.Rows.Clear();
			GridDetail.ClearStructure();
			dtData.Rows.Clear();
			ReportsParameters Order = new ReportsParameters();
			Order.OrganizationId = UserAccount.OrganizationId;
			Order.CompanyId = UserAccount.CompanyId;
			Order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Order.FromDate = FromDate.Value;
			Order.ToDate = Todate.Value;
			dtData = InvGsStoreIssuanceHeader.PendingLoaderDataForStoreReturn(Order);
			if (dtData.Rows.Count > 0)
			{
				foreach (var item in (from r in dtData.AsEnumerable()
					group r by r.Field<int>("Id")).Select(delegate(IGrouping<int, DataRow> g)
				{
					DataRow dataRow = g.First();
					double totalIssueQty = g.Sum((DataRow r) => r.Field<double>("IssueQty"));
					double totalReplacementQty = g.Sum((DataRow r) => r.Field<double>("ReplacementQty"));
					double totalBalanceQty = g.Sum((DataRow r) => r.Field<double>("BalanceQty"));
					return new
					{
						Id = dataRow["Id"],
						DetailId = dataRow["DetailId"],
						DocDate = dataRow["DocDate"],
						DocNo = dataRow["DocNo"],
						DocumentType = dataRow["DocumentType"],
						ManualNo = dataRow["ManualNo"],
						TotalIssueQty = totalIssueQty,
						TotalReplacementQty = totalReplacementQty,
						TotalBalanceQty = totalBalanceQty,
						EntryDate = dataRow["EntryDate"],
						EntryUser = dataRow["EntryUserName"],
						ModifyDate = dataRow["ModifyDate"],
						ModifyUser = dataRow["ModifyUserName"],
						Remarks = dataRow["Remarks"]
					};
				}))
				{
					DataRow dr = dtMain.NewRow();
					dr["Id"] = item.Id;
					dr["DetailId"] = item.DetailId;
					dr["DocDate"] = item.DocDate;
					dr["DocNo"] = item.DocNo;
					dr["DocumentType"] = item.DocumentType;
					dr["ManualNo"] = item.ManualNo ?? DBNull.Value;
					dr["TotalIssueQty"] = item.TotalIssueQty;
					dr["TotalReplacementQty"] = item.TotalReplacementQty;
					dr["TotalBalanceQty"] = item.TotalBalanceQty;
					dr["EntryDate"] = item.EntryDate;
					dr["EntryUser"] = item.EntryUser ?? DBNull.Value;
					dr["ModifyDate"] = item.ModifyDate;
					dr["ModifyUser"] = item.ModifyUser ?? DBNull.Value;
					dr["Remarks"] = item.Remarks ?? DBNull.Value;
					dtMain.Rows.Add(dr);
				}
				grd.DataSource = dtMain;
				grd.RetrieveStructure();
				grdSettingsPending();
			}
			else
			{
				grd.ClearStructure();
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPending()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			grd.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentTypeCode;
			grd.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["TotalIssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["TotalReplacementQty"].Width = Constants.InventoryConstants.QtyConstant + 15;
			grd.RootTable.Columns["TotalBalanceQty"].Width = Constants.InventoryConstants.QtyConstant + 10;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddSelectButton(grd);
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtMain);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, GridDetail, dtDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			dtForLoad = dtData.Clone();
			if (GridDetail.GetCheckedRows().Count() > 0)
			{
				int DetailId = 0;
				dtForLoad.Rows.Clear();
				GridEXRow[] checkedRows = GridDetail.GetCheckedRows();
				for (int j = 0; j < checkedRows.Length; j++)
				{
					DetailId = Conversion.ToInt(checkedRows[j].Cells["DetailId"].Value);
					for (int i = 0; i < dtData.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtData.Rows[i]["DetailId"]) == DetailId)
						{
							DataRow dr = dtData.Rows[i];
							dtForLoad.Rows.Add(dr.ItemArray);
						}
					}
				}
				Hide();
			}
			else
			{
				MessageBox.Show("Check the Row first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingOrderForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtForLoad = null;
		Hide();
	}

	private void frmLoadGRN_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btngrnlod_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindDetailToDetailGridOfRowId(int Id)
	{
		try
		{
			int InitialCount = dtDetail.Rows.Count;
			EnumerableRowCollection<DataRow> Rows = from row in dtData.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable detail = (Rows.Any() ? Rows.CopyToDataTable() : dtData.Clone());
			if (detail.Rows.Count <= 0)
			{
				return;
			}
			for (int j = 0; j < detail.Rows.Count; j++)
			{
				dtDetail.Rows.Add(detail.Rows[j]["Id"], detail.Rows[j]["DetailId"], detail.Rows[j]["WareHouseName"], detail.Rows[j]["ItemName"], detail.Rows[j]["PackUom"], detail.Rows[j]["ConditionStatus"], detail.Rows[j]["AssetName"], detail.Rows[j]["DepartmentName"], detail.Rows[j]["IssueQty"], detail.Rows[j]["ReplacementQty"], detail.Rows[j]["BalanceQty"], detail.Rows[j]["ItemRate"], detail.Rows[j]["ItemAmount"]);
			}
			GridDetail.DataSource = dtDetail;
			if (InitialCount == 0)
			{
				GridDetail.RetrieveStructure();
				GridDetailSetting();
			}
			int FinalCount = dtDetail.Rows.Count;
			if (InitialCount == 0)
			{
				CheckedAllDetailRows();
				return;
			}
			for (int i = InitialCount; i < FinalCount; i++)
			{
				GridEXRow row2 = GridDetail.GetRow(InitialCount++);
				row2.BeginEdit();
				row2.IsChecked = true;
				row2.EndEdit();
			}
			GridDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CheckedAllDetailRows()
	{
		try
		{
			if (grd.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] rows = GridDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.IsChecked = Convert.ToBoolean(value: true);
					obj.EndEdit();
				}
				GridDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId" };
			GridEX_Helper.GridWrappingAndColumnSettings(GridDetail, 2, 3, hiddenColumns);
			GridDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			GridDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			GridDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			GridDetail.RootTable.Columns["ConditionStatus"].Width = Constants.InventoryConstants.StringConstant;
			GridDetail.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.AccountTitle;
			GridDetail.RootTable.Columns["DepartmentName"].Width = Constants.InventoryConstants.StringConstant;
			GridDetail.RootTable.Columns["IssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			GridDetail.RootTable.Columns["ReplacementQty"].Width = Constants.InventoryConstants.QtyConstant + 10;
			GridDetail.RootTable.Columns["BalanceQty"].Width = Constants.InventoryConstants.QtyConstant + 10;
			GridDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			GridDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			GridEX_Helper.AddSelectButton(GridDetail);
			GridDetail.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailFromDetailGridOfRowId(int Id)
	{
		try
		{
			EnumerableRowCollection<DataRow> Rows = from row in dtDetail.AsEnumerable()
				where row.Field<int>("Id") == Id
				select row;
			DataTable detail = (Rows.Any() ? Rows.CopyToDataTable() : dtData.Clone());
			if (detail.Rows.Count <= 0)
			{
				return;
			}
			int rowIndex = -1;
			for (int j = 0; j < dtDetail.Rows.Count; j++)
			{
				if (Conversion.ToInt(dtDetail.Rows[j]["Id"]) == Id)
				{
					rowIndex = j;
					break;
				}
			}
			if (rowIndex != -1)
			{
				for (int i = 0; i < detail.Rows.Count; i++)
				{
					dtDetail.Rows[rowIndex].Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
	{
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row.IsChecked)
			{
				BindDetailToDetailGridOfRowId(Conversion.ToInt(row.Cells["Id"].Value));
			}
			else
			{
				DeleteDetailFromDetailGridOfRowId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panelheading = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.GridDetail = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panelheading.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panelheading);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1149, 33);
		this.panel1.TabIndex = 0;
		this.panelheading.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panelheading.Controls.Add(this.ctrlGrdBar1);
		this.panelheading.Controls.Add(this.label23);
		this.panelheading.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelheading.Location = new System.Drawing.Point(0, 0);
		this.panelheading.Name = "panelheading";
		this.panelheading.Size = new System.Drawing.Size(1149, 33);
		this.panelheading.TabIndex = 29;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1107, 5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1149, 333);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.RowCheckStateChanged += new RowCheckStateChangeEventHandler(grd_RowCheckStateChanged);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(265, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Issuance Load For Store Return";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnReset);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label13);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel2.Location = new System.Drawing.Point(0, 33);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1149, 50);
		this.panel2.TabIndex = 30;
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(393, 21);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 27);
		this.btnReset.TabIndex = 7;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(332, 21);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 27);
		this.btnLoadOnInvoice.TabIndex = 6;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(267, 21);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(65, 27);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yyyy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(137, 23);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(128, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yyyy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(8, 23);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(128, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(137, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 83);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1149, 565);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1109, 333);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 355);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1149, 210);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 4;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Dock = System.Windows.Forms.DockStyle.Top;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(0, 333);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(1149, 22);
		this.label2.TabIndex = 3;
		this.label2.Text = "Header Record's Details To Load";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1149, 648);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmLoadIssuanceForReturn";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load_1);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panelheading.ResumeLayout(false);
		this.panelheading.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
