using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Sale;

public class LoadSaleInvoiceForReturn : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtRecordsFromDb = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtBranch = new DataTable();

	public DataTable dtLoader = new DataTable();

	private bool BranchImplemented;

	private bool FormInitialized;

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

	public LoadSaleInvoiceForReturn(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 500);
			dtBranch.Columns.Add("BranchId", typeof(int));
			dtBranch.Columns.Add("BranchName");
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			Todate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SaleInvoiceReturnBranchWise"));
			BranchesDbCall();
			PendingDataDbCall(useBranchDt: true);
		}).ContinueWith(delegate(Task t)
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				ctrlGrdBar1.mGridChooseFielder.Visible = false;
				ctrlGrdBar1.mGridSaveLayouts.Visible = false;
				ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
				BranchesBind();
				PendingDataGridBind();
				FromDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void BranchesDbCall()
	{
		try
		{
			dtBranch.Clear();
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				dtBranch = InvSaleInvoice.GetBranchsAllocatedToUserFromSaleInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesBind()
	{
		try
		{
			string BranchName = Conversion.ToString((object)((Control)(object)CmbBranch).Text);
			((UltraGridBase)CmbBranch).DataSource = null;
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			Todate.Value = DateTime.Now;
			grd.DataSource = null;
			FromDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesDbCall();
			BranchesBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			PendingDataDbCall();
			PendingDataGridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingDataDbCall(bool useBranchDt = false)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			dtRecordsFromDb.Clear();
			ReportsParameters gdn = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				FromDate = Conversion.ToDateTime((object)FromDate.Value),
				ToDate = Conversion.ToDateTime((object)Todate.Value)
			};
			List<string> branchIdList = new List<string>();
			if (useBranchDt)
			{
				foreach (DataRow dr in dtBranch.Rows)
				{
					branchIdList.Add(Conversion.ToString(dr["BranchId"]));
				}
			}
			else
			{
				string obj = Conversion.ToString((object)((Control)(object)CmbBranch).Text);
				if (string.IsNullOrWhiteSpace(obj))
				{
					CmbBranch.Focus();
					throw new Exception("Select branch first");
				}
				string[] array = obj.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string branchName in array)
				{
					DataRow[] matchedRows = dtBranch.Select("BranchName='" + branchName.Trim() + "'");
					if (matchedRows.Length != 0)
					{
						branchIdList.Add(Conversion.ToString(matchedRows[0].ItemArray[0]));
					}
				}
			}
			gdn.BranchesIds = string.Join(",", branchIdList);
			dtRecordsFromDb = InvSaleInvoice.PendingSaleInvoiceForReturnInvoice(gdn);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void PendingDataGridBind()
	{
		try
		{
			if (dtRecordsFromDb.Rows.Count > 0)
			{
				if (dtGrid.Columns.Count == 0)
				{
					dtGrid.Columns.Add("BranchId", typeof(int));
					dtGrid.Columns.Add("BranchName", typeof(string));
					dtGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtGrid.Columns.Add("DocumentType", typeof(string));
					dtGrid.Columns.Add("Id", typeof(int));
					dtGrid.Columns.Add("DocNo", typeof(int));
					dtGrid.Columns.Add("DocDate", typeof(DateTime));
					dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dtGrid.Columns.Add("CustomerName", typeof(string));
					dtGrid.Columns.Add("CommissionAgent", typeof(string));
					dtGrid.Columns.Add("CommAmount", typeof(decimal));
					dtGrid.Columns.Add("PaymentTerm", typeof(string));
					dtGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtGrid.Columns.Add("EntryUser", typeof(string));
					dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtGrid.Columns.Add("ModifyUser", typeof(string));
					dtGrid.Columns.Add("ApprovalStatus", typeof(string));
					dtGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtGrid.Columns.Add("ApprovedUser", typeof(string));
					dtGrid.Columns.Add("BillAmount", typeof(decimal));
					dtGrid.Columns.Add("WareHouseName", typeof(string));
					dtGrid.Columns.Add("ItemId", typeof(int));
					dtGrid.Columns.Add("ItemCode", typeof(string));
					dtGrid.Columns.Add("ItemName", typeof(string));
					dtGrid.Columns.Add("CropYear", typeof(string));
					dtGrid.Columns.Add("JobLot", typeof(string));
					dtGrid.Columns.Add("PackUom", typeof(string));
					dtGrid.Columns.Add("ItemQty", typeof(decimal));
					dtGrid.Columns.Add("NetBillWeight", typeof(decimal));
					dtGrid.Columns.Add("ItemRate", typeof(decimal));
					dtGrid.Columns.Add("RateUom", typeof(string));
					dtGrid.Columns.Add("ItemAmount", typeof(decimal));
					dtGrid.Columns.Add("UsedQty", typeof(decimal));
					dtGrid.Columns.Add("UsedWeight", typeof(decimal));
					dtGrid.Columns.Add("UsedAmount", typeof(decimal));
					dtGrid.Columns.Add("BalQty", typeof(decimal));
					dtGrid.Columns.Add("BalWeight", typeof(decimal));
					dtGrid.Columns.Add("BalAmount", typeof(decimal));
					dtGrid.Columns.Add("GpNo", typeof(int));
					dtGrid.Columns.Add("VehicleNo", typeof(string));
					dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtGrid.Rows.Clear();
				foreach (DataRow row in dtRecordsFromDb.Rows)
				{
					dtGrid.Rows.Add(row["BranchId"], row["BranchName"], row["DocumentTypeId"], row["DocumentType"], row["Id"], row["DocNo"], row["DocDate"], row["SupplierCustomerId"], row["CustomerName"], row["CommissionAgent"], row["CommAmount"], row["TermsDescription"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["IsApproved"], row["ApprovedDate"], row["ApprovedUserName"], row["BillAmount"], row["WareHouseName"], row["ItemId"], row["ItemCode"], row["ItemName"], row["CropYear"], row["JobLotCode"], row["PackUomCode"], row["ItemQty"], row["NetBillWeight"], row["ItemRate"], row["RateUomCode"], row["ItemAmount"], row["UsedQty"], row["UsedWeight"], row["UsedAmount"], row["BalQty"], row["BalWeight"], row["BalAmount"], row["GpNo"], row["VehicleNo"], row["NoOfAttachments"]);
				}
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettingsForGrnPending();
			}
			else
			{
				grd.DataSource = null;
			}
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
			List<string> hiddenCols = new List<string> { "Id", "BranchId", "DocumentTypeId", "SupplierCustomerId", "BranchName" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenCols);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grd.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grd.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["ApprovalStatus"].Width = Constants.InventoryConstants.IsApproved;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grd.RootTable.Columns["BillAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["NetBillWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns["UsedQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["UsedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["UsedAmount"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grd.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			GridEX_Helper.AddSelectButton(grd, "Select", 30, 0);
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Column.Key == "NoOfAttachments")
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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
				int BranchId = 0;
				int sID = 0;
				int DocID = 0;
				string Ids = "";
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r2 in checkedRows)
				{
					int supplierCustomer = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
					if (supplierCustomer != 0)
					{
						if (sID == 0)
						{
							sID = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
						}
						if (sID != supplierCustomer)
						{
							Ids = "";
							dtLoader.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Invoices are not of same Customer");
							return;
						}
					}
					int DocumentTypeId = Conversion.ToInt(r2.Cells["DocumentTypeId"].Value);
					if (DocumentTypeId != 0)
					{
						if (DocID == 0)
						{
							DocID = Conversion.ToInt(r2.Cells["DocumentTypeId"].Value);
						}
						if (DocID != DocumentTypeId)
						{
							Ids = "";
							dtLoader.Rows.Clear();
							MessageBox.Show("Sorry!. The Selected Invoices are not of same DocumentType");
							return;
						}
					}
					if (BranchImplemented)
					{
						int CurrBranchId = Conversion.ToInt(r2.Cells["BranchId"].Value);
						if (CurrBranchId != 0)
						{
							if (BranchId == 0)
							{
								BranchId = Conversion.ToInt(r2.Cells["BranchId"].Value);
							}
							if (BranchId != CurrBranchId)
							{
								Ids = "";
								dtLoader.Rows.Clear();
								MessageBox.Show("Sorry!. The Selected Invoices are not of same Branch");
								return;
							}
						}
					}
					Ids = Ids + "," + r2.Cells["Id"].Value.ToString();
					sID = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
					DocID = Conversion.ToInt(r2.Cells["DocumentTypeId"].Value);
					BranchId = Conversion.ToInt(r2.Cells["BranchId"].Value);
				}
				if (Ids != string.Empty)
				{
					string[] idsArray = Ids.Split(',');
					EnumerableRowCollection<DataRow> rows = from r in dtRecordsFromDb.AsEnumerable()
						where idsArray.Contains(r.Field<int>("Id").ToString())
						select r;
					dtLoader = (rows.Any() ? rows.CopyToDataTable() : dtRecordsFromDb.Clone());
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

	private void LoadSaleInvoiceForReturn_KeyDown(object sender, KeyEventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "NoOfAttachments")
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
			}
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
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_06dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.LoadSaleInvoiceForReturn));
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
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.panel3.SuspendLayout();
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
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1037, 560);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
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
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 91);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1037, 25);
		this.panel3.TabIndex = 31;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1037, 676);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Name = "LoadSaleInvoiceForReturn";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadSaleInvoiceForReturn_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.panel3.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
