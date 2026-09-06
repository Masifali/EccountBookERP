using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmPendingPurchaseInvoiceForReturn : Form
{
	public bool BranchImplemented;

	private bool FormInitialized;

	private DropDownBind DDL = new DropDownBind();

	public DataTable dtLoader = new DataTable();

	public DataTable dtGrid = new DataTable();

	public DataTable dtBranch = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button BtnShow;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label2;

	private Label label3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label4;

	internal UltraCombo CmbBranch;

	private Panel panel4;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label1;

	internal GridEX grdDetail;

	public frmPendingPurchaseInvoiceForReturn(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmLoadGRN_Load(object sender, EventArgs e)
	{
		FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
		dtBranch.Columns.Add("BranchId", typeof(int));
		dtBranch.Columns.Add("BranchName");
		BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("PurchaseInvoiceReturnBranchWise"));
		FormInitialized = true;
		BranchesFill();
		PendingGdnLoad();
	}

	private void BranchesFill()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)CmbBranch).Text);
			dtBranch.Clear();
			((UltraGridBase)CmbBranch).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				dtBranch = InvPurchaseInvoice.GetBranchsAllocatedToUserFromPurchaseInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			}
			if (dtBranch.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtBranch, CmbBranch, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
			UltraGridColumn obj = ((UltraGridBase)CmbBranch).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			CmbBranch.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			((UltraGridBase)CmbBranch).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			CmbBranch.LimitToList.ToString();
			((Control)(object)CmbBranch).Text = UserAccount.BranchName.ToString();
			if (string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)CmbBranch).Text)))
			{
				if (!string.IsNullOrEmpty(BranchName))
				{
					((Control)(object)CmbBranch).Text = BranchName;
				}
				else
				{
					((Control)(object)CmbBranch).Text = Conversion.ToString(dtBranch.Rows[0]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtLoader.Rows.Add(0);
		Hide();
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingGdnLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingGdnLoad()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtGrid.Clear();
			ReportsParameters gdn = new ReportsParameters();
			gdn.OrganizationId = UserAccount.OrganizationId;
			gdn.CompanyId = UserAccount.CompanyId;
			gdn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gdn.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			gdn.ToDate = Conversion.ToDateTime((object)Todate.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranch).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranch).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				gdn.BranchesIds = BranchIds;
				dtGrid = InvPurchaseInvoice.PendingPurchaseInvoiceStoreForReturnInvoice(gdn);
				if (dtGrid.Rows.Count > 0)
				{
					DataTable dtcol = new DataTable();
					dtcol.Columns.Add("BranchId", typeof(int));
					dtcol.Columns.Add("BranchName", typeof(string));
					dtcol.Columns.Add("DocumentTypeId", typeof(int));
					dtcol.Columns.Add("DocumentType", typeof(string));
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("DocNo", typeof(int));
					dtcol.Columns.Add("DocDate", typeof(DateTime));
					dtcol.Columns.Add("SupplierCustomerId", typeof(int));
					dtcol.Columns.Add("SupplierName", typeof(string));
					dtcol.Columns.Add("PaymentTerm", typeof(string));
					dtcol.Columns.Add("DueDays", typeof(int));
					dtcol.Columns.Add("DueDate", typeof(DateTime));
					dtcol.Columns.Add("BillAmount", typeof(decimal));
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("EntryUser", typeof(string));
					dtcol.Columns.Add("ModifyDate", typeof(DateTime));
					dtcol.Columns.Add("ModifyUser", typeof(string));
					dtcol.Columns.Add("ApprovalStatus", typeof(string));
					dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
					dtcol.Columns.Add("ApprovedUser", typeof(string));
					dtcol.Columns.Add("NoOfAttachments", typeof(int));
					dtcol.Rows.Clear();
					HashSet<int> addedIds = new HashSet<int>();
					foreach (DataRow row in dtGrid.Rows)
					{
						int id = Conversion.ToInt(row["Id"]);
						if (addedIds.Add(id))
						{
							dtcol.Rows.Add(row["BranchId"], row["BranchName"], row["DocumentTypeId"], row["DocumentType"], row["Id"], row["DocNo"], row["DocDate"], row["SupplierCustomerId"], row["SupplierName"], row["TermsDescription"], row["DueDays"], row["DueDate"], row["BillAmount"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovedUserName"], row["NoOfAttachments"]);
						}
					}
					grd.DataSource = dtcol;
					grd.RetrieveStructure();
					grdSettingsForGrnPending();
				}
				else
				{
					grd.DataSource = null;
				}
				return;
			}
			CmbBranch.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdSettingsForGrnPending()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["BranchId"].Visible = false;
			grd.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grd, 3);
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grd.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grd.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["ApprovalStatus"].Width = Constants.InventoryConstants.IsApproved;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 30;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(currentRow.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grd.DataSource;
				if (dt != null)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dt);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				DataTable dt = (DataTable)grdDetail.DataSource;
				if (dt != null)
				{
					GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dt);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnLoad(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)grd.GetCheckedRows().Count().ToString()) > 0)
			{
				int Id = 0;
				string Ids = "";
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					int DocumentId = Conversion.ToInt(r2.Cells["Id"].Value);
					if (DocumentId != 0)
					{
						if (Id == 0)
						{
							Id = DocumentId;
						}
						if (Id != DocumentId)
						{
							Ids = "";
							dtLoader.Rows.Clear();
							MessageBox.Show("Sorry!. you can only select one invoice at a time");
							return;
						}
					}
					Ids = Ids + "," + r2.Cells["Id"].Value.ToString();
				}
				if (Ids != string.Empty)
				{
					string[] idsArray = Ids.Split(',');
					EnumerableRowCollection<DataRow> rows = from r in dtGrid.AsEnumerable()
						where idsArray.Contains(r.Field<int>("Id").ToString())
						select r;
					dtLoader = (rows.Any() ? rows.CopyToDataTable() : dtGrid.Clone());
				}
				Hide();
				return;
			}
			throw new Exception("Chek the row first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+L", "To Press Load Button");
			dt.Rows.Add("Ctrl+S", "For Search");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void frmLoadPurchaseInvoiceForReturn_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				dtLoader = null;
				Hide();
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				BtnShow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				BtnLoad(null, null);
			}
			else if ((!e.Control || e.KeyCode != Keys.N) && (!e.Control || e.KeyCode != Keys.R))
			{
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDate.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				DetailGridBind(new HashSet<int> { id });
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(HashSet<int> selectedIds)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DetailId", typeof(int));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemCode", typeof(string));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("WareHouseName", typeof(string));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("ItemCondition", typeof(string));
			dt.Columns.Add("JobLot", typeof(string));
			dt.Columns.Add("Uom", typeof(string));
			dt.Columns.Add("Qty", typeof(decimal));
			dt.Columns.Add("Rate", typeof(decimal));
			dt.Columns.Add("ItemAmount", typeof(decimal));
			dt.Columns.Add("UsedQty", typeof(decimal));
			dt.Columns.Add("BalQty", typeof(decimal));
			dt.Columns.Add("UsedAmount", typeof(decimal));
			dt.Columns.Add("BalAmount", typeof(decimal));
			dt.Columns.Add("GpNo", typeof(int));
			dt.Columns.Add("VehicleNo", typeof(string));
			foreach (DataRow r2 in from r in dtGrid.AsEnumerable()
				where selectedIds.Contains(Conversion.ToInt(r["Id"]))
				select r)
			{
				DataRow newRow = dt.NewRow();
				newRow["Id"] = r2["Id"];
				newRow["DetailId"] = r2["DetailId"];
				newRow["ItemId"] = r2["ItemId"];
				newRow["ItemCode"] = r2["ItemCode"];
				newRow["ItemName"] = r2["ItemName"];
				newRow["WareHouseName"] = r2["WareHouseName"];
				newRow["RackName"] = r2["RackName"];
				newRow["ItemCondition"] = r2["ItemCondition"];
				newRow["JobLot"] = r2["JobLotCode"];
				newRow["Uom"] = r2["PackUomCode"];
				newRow["Qty"] = r2["ItemQty"];
				newRow["Rate"] = r2["ItemRate"];
				newRow["ItemAmount"] = r2["ItemAmount"];
				newRow["UsedQty"] = r2["UsedQty"];
				newRow["BalQty"] = r2["BalQty"];
				newRow["UsedAmount"] = r2["UsedAmount"];
				newRow["BalAmount"] = r2["BalAmount"];
				newRow["GpNo"] = r2["GpNo"];
				newRow["VehicleNo"] = r2["VehicleNo"];
				dt.Rows.Add(newRow);
			}
			grdDetail.DataSource = dt;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DetailId", "ItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCode", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(grdDetail, "WareHouseName", Constants.InventoryConstants.WareHouseName);
			GridEX_Helper.SetColumnWidth(grdDetail, "RackName", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemCondition", Constants.InventoryConstants.StringConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "JobLot", Constants.InventoryConstants.JobLotConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "Uom", Constants.InventoryConstants.UomConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "Qty", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "UsedQty", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "BalQty", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "Rate", 80);
			GridEX_Helper.SetColumnWidth(grdDetail, "ItemAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "UsedAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "BalAmount", Constants.InventoryConstants.AmountConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "GpNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(grdDetail, "VehicleNo", Constants.InventoryConstants.VehicleNoConstant);
			ctrlGrdBar2_Load(null, null);
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_073c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmPendingPurchaseInvoiceForReturn));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.BtnShow = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1037, 51);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1037, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Visible = false;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Visible = false;
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKeys.Image");
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(1, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(239, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Invoices For Return";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 116);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 206);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.btnLoadOnInvoice);
		this.panel2.Controls.Add(this.BtnShow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 51);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1037, 40);
		this.panel2.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(12, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(55, 15);
		this.label4.TabIndex = 18;
		this.label4.Text = "Branches";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(77, 5);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(232, 29);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 17;
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(575, 8);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(135, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(385, 8);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(135, 23);
		this.FromDate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(521, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 15;
		this.label2.Text = "To Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(317, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 16;
		this.label3.Text = "From Date";
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(786, 7);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(58, 25);
		this.btnLoadOnInvoice.TabIndex = 3;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(BtnLoad);
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(716, 7);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(64, 25);
		this.BtnShow.TabIndex = 2;
		this.BtnShow.Text = "Search";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 91);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 25);
		this.panel3.TabIndex = 31;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(4, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(116, 19);
		this.label1.TabIndex = 29;
		this.label1.Text = "Filtered Data";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel4.Location = new System.Drawing.Point(0, 322);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1037, 25);
		this.panel4.TabIndex = 32;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(197, 19);
		this.label5.TabIndex = 29;
		this.label5.Text = "Selected Invoice Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(995, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 10;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 347);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1037, 234);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 33;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 581);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "frmPendingPurchaseInvoiceForReturn";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmLoadGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLoadPurchaseInvoiceForReturn_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}
