using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.StoreManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.StoreManagement;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.StoreManagement.FromWise_Helper_Methods;
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
namespace Architecture.WinApp.StoreManagement
{

public class frmDeliveryChallanAgainstPurchasePreBill : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtDetailGrid = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtLastSavedRecord = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtCondition = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmDeliveryChallanAgainstPurchasePreBill";

	private bool UpdateMode;

	private bool FormInitialized;

	private bool Approved;

	public bool OnlyPending;

	public static int RecId;

	private int DocumentTypeId = 148;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	public DateTime FromDate;

	public DateTime ToDate;

	private frmPendingPurchasePreBillLoader LoadPO;

	private List<DeliveryChallanDetail> lstRemoveRecord = new List<DeliveryChallanDetail>();

	private IContainer components;

	private Panel panel1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private DateTimePicker txtDocDate;

	private Label label17;

	private TextBox txtDocNo;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal GridEX grdhistoryDetail;

	private ToolStrip toolStrip2;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private Panel panel10;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel8;

	private Panel panel9;

	private Panel panel12;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel13;

	private Label label49;

	private Label label55;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel14;

	private Label label5;

	private TextBox txtRemarksMain;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Panel panel6;

	private Label label7;

	private TextBox txtBiltyNo;

	private Label label12;

	private TextBox txtVehicleNo;

	internal UltraCombo CmbCityName;

	private Label label28;

	private Label label19;

	private Panel panel11;

	private Label label81;

	private Panel panel15;

	private Panel panel16;

	internal UltraCombo CmbDeliveryTerm;

	private Label label27;

	private ToolStripButton btnPurchaseOrderLoader;

	private GroupBox groupBox3;

	private Label label6;

	private Label label9;

	private Label label11;

	private Label label13;

	private Label label14;

	private Label label35;

	private Label label2;

	internal GridEX grdInvExp;

	private Label label1;

	public TabControl tabControl1;

	public Button btnShow;

	public TextBox ToDocNoHistory;

	public TextBox FromDocNoHistory;

	public DateTimePicker ToDateHistory;

	public DateTimePicker FromDateHistory;

	public UltraCombo CmbItemHistory;

	public UltraCombo cmbDateTypeHistory;

	public UltraCombo CmbBillToPartyHistory;

	public frmDeliveryChallanAgainstPurchasePreBill(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmDeliveryChallanAgainstPurchasePreBill_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1200, 650);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtSupplier.Columns.Add("CurrencyId", typeof(int));
			dtSupplier.Columns.Add("CurrencyCode", typeof(string));
			frmDeliveryChallanAgainstPurchasePreBill_Helper.InitializeDetailtable(dtDetailGrid);
			dtExpGrid.Columns.Add("Id", typeof(int));
			dtExpGrid.Columns.Add("PreBillId", typeof(int));
			dtExpGrid.Columns.Add("PreBillExpenseId", typeof(int));
			dtExpGrid.Columns.Add("PreBillNo", typeof(int));
			dtExpGrid.Columns.Add("ItemId", typeof(int));
			dtExpGrid.Columns.Add("Qty", typeof(double));
			dtExpGrid.Columns.Add("Rate", typeof(double));
			dtExpGrid.Columns.Add("Amount", typeof(double));
			dtExpGrid.Columns.Add("Remarks");
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
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			string documentNoData = DocumentNoDbCall();
			DataTable termData = DeliveryTerm.FormHistory();
			DataTable historyComboData = HistoryComboDbCall();
			OtherItemdtDbCall();
			return new
			{
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				TermData = termData
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnsave.Enabled = formright.DoHaveSaveRight;
					btnupdate.Enabled = formright.DoHaveUpdateRights;
					btnDelete.Enabled = formright.DoHaveCanDelete;
					btnprint.Enabled = formright.DoHavePrintRights;
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
					ctrlGrdBar2.mGridSaveLayouts.Visible = false;
					ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
					GetConfigurationsFromGlobal();
					if (RecId == 0)
					{
						txtDocNo.Text = result.DocumentNoData;
					}
					DeliveryTerm(result.TermData);
					CityDtFillFromGlobalAndBind();
					BindGrids();
					HistoryComboBind(result.HistoryComboData);
					Datetypefill();
					if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
					{
						FromDateHistory.Checked = false;
						ToDateHistory.Value = ToDate;
						btnShow_Click(null, null);
					}
					else
					{
						FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
						ToDateHistory.Value = DateTime.Now;
					}
					FormInitialized = true;
					txtDocDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int cityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("City Area"));
			if (cityId > 0)
			{
				CmbCityName.Value = cityId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)DeliveryChallanHeader.GenerateCode(new DeliveryChallanHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private void DeliveryTerm(DataTable dtTerm)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dtTerm, "Id", "Description", "Delivery Term", AllColumns: true, Conversion.ToInt(CmbDeliveryTerm.Value), insertDefaultRow: true, null, null, ActivateRow: true, 2);
			if (dtTerm.Rows.Count > 0)
			{
				((UltraGridBase)CmbDeliveryTerm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalAllCities, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			InfragisticsHelper.BindAndRetainSelection(CmbCityName, dt, "Id", "Description", "City Name", AllColumns: true, Conversion.ToInt(CmbCityName.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDtFill()
	{
		try
		{
			List<GlobalItemCondition> lst = clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList();
			dtCondition = DatatableHelper.PopulateDataTableAndReturn(lst, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemdtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtDetailGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtDetailGrid;
				grd.RetrieveStructure();
				grdDetailSetting();
			}
			DataTable dataTable2 = dtExpGrid;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				if (dtExpGrid.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.DataSource = dtExpGrid;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				if (e.Column.Key == "ThisQty")
				{
					RecalculateRow(row);
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RecalculateRow(GridEXRow row)
	{
		decimal allowedQty = Conversion.ToDecimal(row.Cells["BalanceQty"].Value);
		decimal thisQty = Conversion.ToDecimal(row.Cells["ThisQty"].Value);
		if (thisQty > allowedQty)
		{
			thisQty = allowedQty;
			row.Cells["ThisQty"].Value = thisQty;
			MessageBox.Show("You can't add Qty more than Balance Qty");
		}
	}

	private void GridComboBind()
	{
		try
		{
			GridDtFill();
			GridEX_Helper.ComboBind(grd.RootTable.Columns["ItemConditionId"], dtCondition, "Id", "Description", "Item Condition", 90);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSetting()
	{
		try
		{
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ThisQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			frmDeliveryChallanAgainstPurchasePreBill_Helper.DetailGridCommonSetting(grd, editableColumns);
			GridComboBind();
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					DeliveryChallanDetail d = new DeliveryChallanDetail();
					d.DeliveryChallanDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.ActionTypeId = 3;
					lstRemoveRecord.Add(d);
					dtDetailGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			else
			{
				dtDetailGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
	}

	private void DeleteRowInExpenseGrid(GridEXRow item)
	{
	}

	private void ExpenseGridCombBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Other Item Name");
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "PreBillId", "PreBillExpenseId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdInvExp, 1, 3, hiddenColumns, editableColumns);
			grdInvExp.RootTable.Columns["PreBillNo"].Width = Constants.InventoryConstants.DateConstant + 20;
			ExpenseGridCombBind();
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteRowInExpenseGrid(item);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInvExpGrid();
				}
			}
			grdInvExp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInvExp.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				default:
					return;
				case "Qty":
				case "Rate":
					UpdateAmount(row);
					break;
				case "Amount":
					UpdateRate(row);
					break;
				}
				grdInvExp.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAmount(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		row.Cells["Amount"].Value = qty * rate;
	}

	private void UpdateRate(GridEXRow row)
	{
		double amount = Conversion.ToDouble(row.Cells["Amount"].Value);
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		row.Cells["Rate"].Value = ((qty == 0.0) ? 0.0 : (amount / qty));
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					DeliveryChallanHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Expected O, but got Unknown
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Detail Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				(txtDocNo, "Doc No", FormHelper.ValidType.INT),
				(txtVehicleNo, "Vehicle No", FormHelper.ValidType.String)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			DeliveryChallanHeader Or = new DeliveryChallanHeader();
			Or.DeliveryChallanHeaderId = RecId;
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = DocumentTypeId;
			Or.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			Or.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			Or.DeliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			Or.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
			Or.CityId = Conversion.ToInt(CmbCityName.Value);
			Or.CityName = Conversion.ToString((object)((Control)(object)CmbCityName).Text);
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksMain.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					Or.DeliveryChallanDetailList.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				DeliveryChallanDetail vd = new DeliveryChallanDetail();
				vd.DeliveryChallanDetailId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.DeliveryChallanDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r2);
				FormHelper.ValidateField(vd.PurchaseDemandHeaderId, "Demand No", r2.RowIndex);
				FormHelper.ValidateField(vd.PurchaseDemandDetailId, "Demand No", r2.RowIndex);
				FormHelper.ValidateField(vd.ItemId, "Item", r2.RowIndex);
				FormHelper.ValidateField(vd.UomId, "Uom", r2.RowIndex);
				FormHelper.ValidateField(vd.Qty, "This Qty", r2.RowIndex);
				if (vd.Qty > vd.PurchasePreBillQty - vd.AlreadyUsedPreBillQtyInDeliveryChallan)
				{
					throw new Exception($"You can't add Qty more than Balance Qty in Detail Grid at row No: {r2.RowIndex + 1}");
				}
				Or.DeliveryChallanDetailList.Add(vd);
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
				{
					DeliveryChallanExpenseDetail PE = new DeliveryChallanExpenseDetail();
					if (Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0)
					{
						throw new Exception($"Amount Field Required in Expense Grid Row no : {r.RowIndex + 1}");
					}
					PE.DeliveryChallanExpenseDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					PE.PurchasePreBillHeaderId = Conversion.ToInt(r.Cells["PreBillId"].Value);
					PE.PurchasePreBillExpenseDetailId = Conversion.ToInt(r.Cells["PreBillExpenseId"].Value);
					PE.OtherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					PE.OtherItemName = Conversion.ToString((object)r.Cells["ItemId"].Text);
					PE.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					Or.DeliveryChallanExpenseDetailList.Add(PE);
				}
			}
			Or.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			Or.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<DeliveryChallanHeader>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = DeliveryChallanHeader.Save(Or);
			Or.DeliveryChallanHeaderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
			}
			FromReset();
			if (ChkBox.Checked && formright.DoHavePrintRights)
			{
				ShowPrint(Or.DeliveryChallanHeaderId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			FromReset();
			RecId = Id;
			DeliveryChallanHeader Or = DeliveryChallanHeader.ReadById(RecId);
			if (Or == null || Or.DeliveryChallanDetailList == null)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = true;
			txtDocDate.Value = Or.DocDate;
			txtDocNo.Text = Conversion.ToString((object)Or.DocNo);
			List<DeliveryChallanDetail> deliveryChallanDetailList = Or.DeliveryChallanDetailList;
			if (deliveryChallanDetailList != null && deliveryChallanDetailList.Count > 0)
			{
				DeliveryChallanDetail dd = Or.DeliveryChallanDetailList[0];
				txtVehicleNo.Text = Conversion.ToString((object)dd.VehicleNo);
				txtBiltyNo.Text = Conversion.ToString((object)dd.BiltyNo);
			}
			CmbDeliveryTerm.Value = Conversion.ToInt((object)Or.DeliveryTermId);
			CmbCityName.Value = Conversion.ToInt((object)Or.CityId);
			txtRemarksMain.Text = Conversion.ToString((object)Or.RemarksHeader);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			dtDetailGrid.Rows.Clear();
			frmDeliveryChallanAgainstPurchasePreBill_Helper.FillDetailFromListCommonForReadById(Or, dtDetailGrid);
			dtExpGrid.Rows.Clear();
			foreach (DeliveryChallanExpenseDetail d in Or.DeliveryChallanExpenseDetailList)
			{
				DataRow dr = dtExpGrid.NewRow();
				dr["Id"] = d.DeliveryChallanExpenseDetailId;
				dr["PreBillId"] = d.PurchasePreBillHeaderId;
				dr["PreBillExpenseId"] = d.PurchasePreBillExpenseDetailId;
				dr["PreBillNo"] = d.PreBillNo;
				dr["ItemId"] = d.OtherItemId;
				dr["Qty"] = d.Qty;
				dr["Rate"] = d.Rate;
				dr["Amount"] = d.Amount;
				dr["Remarks"] = d.Remarks;
				dtExpGrid.Rows.Add(dr);
			}
			BindGrids();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			txtDocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(DeliveryChallanDetail vd, GridEXRow r)
	{
		vd.PurchaseDemandHeaderId = Conversion.ToInt(r.Cells["PurchaseDemandHeaderId"].Value);
		vd.PurchaseDemandDetailId = Conversion.ToInt(r.Cells["PurchaseDemandDetailId"].Value);
		vd.PurchaseDemandDocNo = Conversion.ToInt(r.Cells["PurchaseDemandNo"].Value);
		vd.PurchasePreBillHeaderId = Conversion.ToInt(r.Cells["PurchasePreBillHeaderId"].Value);
		vd.PurchasePreBillDetailId = Conversion.ToInt(r.Cells["PurchasePreBillDetailId"].Value);
		vd.PurchasePreBillDocNo = Conversion.ToInt(r.Cells["PurchasePreBillNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
		vd.Uom = Conversion.ToString(r.Cells["Uom"].Value);
		vd.UomEquivalent = Conversion.ToDouble(r.Cells["UomEquivalent"].Value);
		vd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		vd.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		vd.PurchaseDemandQty = Conversion.ToDouble(r.Cells["TotalDemandQty"].Value);
		vd.PurchasePreBillQty = Conversion.ToDouble(r.Cells["PreBillQty"].Value);
		vd.AlreadyUsedPreBillQtyInDeliveryChallan = Conversion.ToDouble(r.Cells["UsedQty"].Value);
		vd.Qty = Conversion.ToDouble(r.Cells["ThisQty"].Value);
		vd.PartyBillNo = Conversion.ToString(r.Cells["PartyBillNo"].Value);
		vd.PartyBillDate = Conversion.ToDateTime(r.Cells["PartyBillDate"].Value);
		vd.VendorSupplierId = Conversion.ToInt(r.Cells["VendorSupplierId"].Value);
		vd.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
		vd.VendorSupplierName = Conversion.ToString(r.Cells["VendorSupplier"].Value);
		vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
		vd.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
		vd.BiltyNo = Conversion.ToString((object)txtBiltyNo.Text);
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			Approved = false;
			RecId = 0;
			dtLastSavedRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtVehicleNo.Text = string.Empty;
			txtBiltyNo.Text = string.Empty;
			txtRemarksMain.Text = string.Empty;
			dtDetailGrid.Clear();
			dtExpGrid.Clear();
			BindGrids();
			txtDocNo.Text = DocumentNoDbCall();
			GetConfigurationsFromGlobal();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			txtDocDate.Focus();
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
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "AllCities", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			DeliveryTerm(DeliveryTerm.FormHistory());
			CityDtFillFromGlobalAndBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			OtherItemdtDbCall();
			ExpenseGridCombBind();
			GridComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDeliveryChallanAgainstPurchasePreBill_FormClosing(object sender, FormClosingEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtDocDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return DeliveryChallanHeader.GetDataForDropDownFromDeliveryChallanHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtST = new DataTable();
			dtST.Columns.Add("Id", typeof(int));
			dtST.Columns.Add("Name");
			DataTable dtItem = dtST.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "CityName")
				{
					dtST.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBillToPartyHistory, dtST, "Id", "Name", "City Name", AllColumns: true, Conversion.ToInt(CmbBillToPartyHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemHistory, dtItem, "Id", "Name", "Item Name", AllColumns: true, Conversion.ToInt(CmbItemHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbBillToPartyHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			grdhistoryDetail.DataSource = null;
			drdocdate.Checked = true;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		Datetypefill();
		HistoryComboBind(HistoryComboDbCall());
	}

	public void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights,
				EntryUser = UserAccount.ID
			};
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.CityId = Conversion.ToInt(CmbBillToPartyHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			obj.PendingForView = (OnlyPending ? 1 : 0);
			DataTable dtHistory = DeliveryChallanHeader.FormHistory(obj);
			if (dtHistory.Rows.Count == 0)
			{
				grdhistory.DataSource = null;
				grdhistoryDetail.DataSource = null;
				return;
			}
			if (dtHistoryGrid.Columns.Count == 0)
			{
				dtHistoryGrid.Columns.Add("Id", typeof(int));
				dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtHistoryGrid.Columns.Add("DocNo", typeof(int));
				dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
				dtHistoryGrid.Columns.Add("ReferenceNo", typeof(string));
				dtHistoryGrid.Columns.Add("DeliveryTermId", typeof(int));
				dtHistoryGrid.Columns.Add("DeliveryTerm", typeof(string));
				dtHistoryGrid.Columns.Add("CityId", typeof(int));
				dtHistoryGrid.Columns.Add("CityName", typeof(string));
				dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
				dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtHistoryGrid.Columns.Add("EntryUserName", typeof(string));
				dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
				dtHistoryGrid.Columns.Add("ModifyUserName", typeof(string));
				dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
				dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
				dtHistoryGrid.Columns.Add("ApprovalUserName", typeof(string));
				dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
			}
			dtHistoryGrid.Rows.Clear();
			foreach (DataRow row in dtHistory.Rows)
			{
				DataRow dr = dtHistoryGrid.NewRow();
				dr["Id"] = row["DeliveryChallanHeaderId"];
				dr["DocumentTypeId"] = row["DocumentTypeId"];
				dr["DocNo"] = row["DocNo"];
				dr["DocDate"] = row["DocDate"];
				dr["ReferenceNo"] = row["ReferenceNo"];
				dr["DeliveryTermId"] = row["DeliveryTermId"];
				dr["DeliveryTerm"] = row["DeliveryTerm"];
				dr["CityId"] = row["CityId"];
				dr["CityName"] = row["CityName"];
				dr["RemarksHeader"] = row["RemarksHeader"];
				dr["EntryDate"] = row["EntryDate"];
				dr["EntryUserName"] = row["EntryUserName"];
				dr["ModifyDate"] = row["ModifyDate"];
				dr["ModifyUserName"] = row["ModifyUserName"];
				dr["IsApproved"] = row["IsApproved"];
				dr["ApprovedDate"] = row["ApprovedDate"];
				dr["ApprovalUserName"] = row["ApprovalUserName"];
				dr["NoOfAttachments"] = row["NoOfAttachments"];
				dtHistoryGrid.Rows.Add(dr);
			}
			grdhistory.DataSource = dtHistoryGrid;
			grdhistory.RetrieveStructure();
			HistoryGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "BillToPartyId", "DeliveryTermId", "CityId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3, 3, hiddenColumns);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["CityName"].Width = 120;
			grdhistory.RootTable.Columns["DeliveryTerm"].Width = 120;
			grdhistory.RootTable.Columns["ReferenceNo"].Width = 120;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 50, grdhistory.RootTable.Columns["DocNo"].Position - 1);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 50, grdhistory.RootTable.Columns["DocNo"].Position - 1);
			grdhistory.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				if (!formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				ShowPrint(Conversion.ToInt(r.Cells["Id"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			DeliveryChallanHeader Obj = DeliveryChallanHeader.ReadById(Id);
			if (Obj != null && Obj.DeliveryChallanDetailList.Count == 0)
			{
				grdhistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtDetailGrid.Clone();
			dtDetailHistory.Columns.Add("ItemCondition");
			frmDeliveryChallanAgainstPurchasePreBill_Helper.FillDetailFromListCommonForReadById(Obj, dtDetailHistory, ExtraColumns: true);
			grdhistoryDetail.DataSource = dtDetailHistory;
			grdhistoryDetail.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			frmDeliveryChallanAgainstPurchasePreBill_Helper.DetailGridCommonSetting(grdhistoryDetail);
			grdhistoryDetail.RootTable.Columns["UsedQty"].Visible = false;
			grdhistoryDetail.RootTable.Columns["BalanceQty"].Visible = false;
			grdhistoryDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			grdhistoryDetail.RootTable.Columns["ItemCondition"].Position = grdhistoryDetail.RootTable.Columns["ItemConditionId"].Position + 1;
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtDetailGrid);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdhistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			ShowPrint(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowPrint(int PrintId)
	{
		try
		{
			CrystalReportPrint_Helper.StoreDeliveryChallanSlip148(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefPartyForm_Click(object sender, EventArgs e)
	{
		try
		{
			DefineReferenceParties defineReferenceParties = new DefineReferenceParties(UserAccount);
			defineReferenceParties.CmbReferencePartyType.Value = 4;
			((Control)(object)defineReferenceParties.CmbReferencePartyType).Enabled = false;
			defineReferenceParties.CmbReferencePartyType_Leave(null, null);
			defineReferenceParties.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDispatch_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					BtnRefPartyForm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					if (formright.DoHavePrintRights)
					{
						btnprint_Click(null, null);
					}
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273");
			dt.Rows.Add("Alt+1", "For Print 273");
			dt.Rows.Add("Ctrl+F1", "For Open Reference Party Form");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXColumn currentColumn = grd.CurrentColumn;
				if (((currentColumn != null) ? currentColumn.Key : null) == "Delete")
				{
					DeleteDetailRow(r);
					return;
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			else if (e.KeyCode == Keys.F1)
			{
				GridEXColumn currentColumn2 = grd.CurrentColumn;
				if (currentColumn2 != null)
				{
					_ = currentColumn2.Key;
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
					grdInvExp.UpdateData();
				}
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Delete")
				{
					DeleteRowInExpenseGrid(r);
					grdInvExp.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteRowInExpenseGrid(r);
				grdInvExp.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "Print")
				{
					ShowPrint(CuurID);
				}
				else if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocDateMain_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnPurchaseOrderLoader_Click(object sender, EventArgs e)
	{
		try
		{
			LoadPO = new frmPendingPurchasePreBillLoader(UserAccount);
			LoadPO.ShowDialog();
			LoadPO.BringToFront();
			LoadInGridDetail(LoadPO.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dtLoader)
	{
		try
		{
			if (dtLoader == null || dtLoader.Rows.Count == 0)
			{
				return;
			}
			string poIds = string.Join(",", (from r in dtLoader.AsEnumerable()
				where r["PurchasePreBillHeaderId"] != DBNull.Value
				select Conversion.ToInt(r["PurchasePreBillHeaderId"])).Distinct());
			txtVehicleNo.Text = Conversion.ToString(dtLoader.Rows[0]["VehicleNo"]);
			txtBiltyNo.Text = Conversion.ToString(dtLoader.Rows[0]["BiltyNo"]);
			CmbDeliveryTerm.Value = Conversion.ToInt(dtLoader.Rows[0]["DeliveryTermId"]);
			CmbCityName.Value = Conversion.ToInt(dtLoader.Rows[0]["CityId"]);
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["PurchasePreBillDetailId"].Value));
			foreach (DataRow row in dtLoader.Rows)
			{
				int detailId = Conversion.ToInt(row["DetailId"]);
				if (existingDetailIds.Add(detailId))
				{
					decimal totalDemandQty = Conversion.ToDecimal(row["PurchaseDemandQty"]);
					decimal preBillQty = Conversion.ToDecimal(row["Qty"]);
					decimal usedQty = Conversion.ToDecimal(row["QtyUsedInChallan"]);
					decimal balanceQty = Conversion.ToDecimal(row["BalanceQty"]);
					decimal thisQty = balanceQty;
					DataRow dr = dtDetailGrid.NewRow();
					dr["Id"] = 0;
					dr["PurchaseDemandHeaderId"] = row["PurchaseDemandHeaderId"];
					dr["PurchaseDemandDetailId"] = row["PurchaseDemandDetailId"];
					dr["PurchaseDemandNo"] = row["PurchaseDemandDocNo"];
					dr["PurchasePreBillHeaderId"] = row["PurchasePreBillHeaderId"];
					dr["PurchasePreBillDetailId"] = row["DetailId"];
					dr["PurchasePreBillNo"] = row["DocNo"];
					dr["ItemId"] = row["ItemId"];
					dr["ItemCode"] = row["ItemCode"];
					dr["ItemName"] = row["ItemName"];
					dr["UomId"] = row["UomId"];
					dr["Uom"] = row["Uom"];
					dr["UomEquivalent"] = row["UomEquivalent"];
					dr["ItemConditionId"] = row["ItemConditionId"];
					dr["TotalDemandQty"] = totalDemandQty;
					dr["PreBillQty"] = preBillQty;
					dr["UsedQty"] = usedQty;
					dr["BalanceQty"] = balanceQty;
					dr["ThisQty"] = thisQty;
					dr["PartyBillNo"] = row["PartyBillNo"];
					dr["PartyBillDate"] = row["PartyBillDate"];
					dr["VendorSupplierId"] = row["VendorSupplierId"];
					dr["ReferencePartyId"] = row["ReferencePartyId"];
					dr["VendorSupplier"] = ((Conversion.ToInt(row["VendorSupplierId"]) > 0) ? row["VendorSupplierName"] : row["ReferencePartyName"]);
					dr["Remarks"] = row["RemarksDetail"];
					dtDetailGrid.Rows.Add(dr);
				}
			}
			LoadExpData(poIds);
			BindGrids();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string poIds)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(poIds))
			{
				return;
			}
			DataTable expItems = PurchasePreBillHeader.PurchasePreBillExpenseDetail_GetByHeaderIds(poIds);
			if (expItems == null || expItems.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingExpIds = new HashSet<int>(from r in grdInvExp.GetRows()
				where (int)r.RowType == 0
				select Conversion.ToInt(r.Cells["PreBillExpenseId"].Value));
			foreach (DataRow row in expItems.Rows)
			{
				int expId = Conversion.ToInt(row["PurchasePreBillExpenseDetailId"]);
				if (existingExpIds.Add(expId))
				{
					DataRow newRow = dtExpGrid.NewRow();
					newRow["Id"] = 0;
					newRow["PreBillId"] = row["PurchasePreBillHeaderId"];
					newRow["PreBillExpenseId"] = expId;
					newRow["PreBillNo"] = row["PreBillNo"];
					newRow["ItemId"] = row["OtherItemId"];
					newRow["Qty"] = Conversion.ToDouble(row["Qty"]);
					newRow["Rate"] = Conversion.ToDouble(row["Rate"]);
					newRow["Amount"] = Conversion.ToDouble(row["Amount"]);
					newRow["Remarks"] = Conversion.ToString(row["Remarks"]);
					dtExpGrid.Rows.Add(newRow);
				}
			}
			foreach (DataRow r2 in (from r in dtExpGrid.AsEnumerable()
				where Conversion.ToInt(r["PreBillId"]) == 0 && Conversion.ToInt(r["PreBillExpenseId"]) == 0 && Conversion.ToInt(r["ItemId"]) == 0 && Conversion.ToDouble(r["Qty"]) == 0.0 && Conversion.ToDouble(r["Rate"]) == 0.0 && Conversion.ToDouble(r["Amount"]) == 0.0
				select r).ToList())
			{
				dtExpGrid.Rows.Remove(r2);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
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
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Expected O, but got Unknown
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Expected O, but got Unknown
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Expected O, but got Unknown
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Expected O, but got Unknown
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eab: Expected O, but got Unknown
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Expected O, but got Unknown
		//IL_2b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b71: Expected O, but got Unknown
		//IL_2b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance125 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance157 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmDeliveryChallanAgainstPurchasePreBill));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbDeliveryTerm = new UltraCombo();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbCityName = new UltraCombo();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksMain = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseOrderLoader = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.grdhistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.CmbItemHistory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.cmbDateTypeHistory = new UltraCombo();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.CmbBillToPartyHistory = new UltraCombo();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 753);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1034, 753);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel15);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1026, 724);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(2, 125);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1022, 426);
		this.tabControl2.TabIndex = 4;
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Controls.Add(this.panel10);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1014, 397);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1008, 366);
		this.panel6.TabIndex = 3;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1008, 366);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1008, 25);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(967, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel15.BackColor = System.Drawing.Color.White;
		this.panel15.Controls.Add(this.panel16);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel15.Location = new System.Drawing.Point(2, 551);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1022, 171);
		this.panel15.TabIndex = 0;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel16.Controls.Add(this.panel11);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(713, 171);
		this.panel16.TabIndex = 1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(713, 151);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 240;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.label81);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.ForeColor = System.Drawing.Color.Teal;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(713, 20);
		this.panel11.TabIndex = 36;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label81.ForeColor = System.Drawing.Color.Teal;
		this.label81.Location = new System.Drawing.Point(3, 3);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(468, 14);
		this.label81.TabIndex = 25;
		this.label81.Text = "Other Expenses Credit to Party && Charge to Selected Other Item's Account";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 51);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1022, 74);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityName);
		this.groupBox1.Controls.Add(this.txtBiltyNo);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Controls.Add(this.txtRemarksMain);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1023, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(538, 13);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 10;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNo.Location = new System.Drawing.Point(323, 15);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(130, 23);
		this.txtVehicleNo.TabIndex = 8;
		this.CmbCityName.AllowNull = (DefaultableBoolean)1;
		this.CmbCityName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbCityName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance130;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityName).Location = new System.Drawing.Point(538, 41);
		((System.Windows.Forms.Control)(object)this.CmbCityName).Name = "CmbCityName";
		((System.Windows.Forms.Control)(object)this.CmbCityName).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityName).TabIndex = 11;
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyNo.Location = new System.Drawing.Point(323, 43);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(130, 23);
		this.txtBiltyNo.TabIndex = 9;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(257, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(49, 15);
		this.label7.TabIndex = 3;
		this.label7.Text = "Bilty No";
		this.txtDocNo.BackColor = System.Drawing.SystemColors.Control;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNo.Location = new System.Drawing.Point(124, 15);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(130, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.txtDocNo.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(457, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(80, 15);
		this.label27.TabIndex = 34;
		this.label27.Text = "Delivery Term";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(112, 18);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 218;
		this.label19.Text = "*";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(457, 47);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 15);
		this.label28.TabIndex = 34;
		this.label28.Text = "City Name";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(124, 43);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(130, 22);
		this.txtDocDate.TabIndex = 1;
		this.txtDocDate.TabStop = false;
		this.txtDocDate.ValueChanged += new System.EventHandler(txtDocDateMain_ValueChanged);
		this.txtRemarksMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksMain.Location = new System.Drawing.Point(715, 13);
		this.txtRemarksMain.Multiline = true;
		this.txtRemarksMain.Name = "txtRemarksMain";
		this.txtRemarksMain.Size = new System.Drawing.Size(301, 54);
		this.txtRemarksMain.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(120, 15);
		this.label1.TabIndex = 3;
		this.label1.Text = "Delivery Challan Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(111, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Delivery Challan No";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(661, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(257, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 3;
		this.label12.Text = "Vehicle No";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1022, 24);
		this.panel3.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(357, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Challan Against Purchase Pre_Bill";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(898, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1022, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnDelete, this.btnattachment, this.btnprint, this.btnPurchaseOrderLoader, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1022, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
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
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "148-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnPurchaseOrderLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPurchaseOrderLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseOrderLoader.Name = "btnPurchaseOrderLoader";
		this.btnPurchaseOrderLoader.Size = new System.Drawing.Size(113, 24);
		this.btnPurchaseOrderLoader.Text = "Load Pre_Bill";
		this.btnPurchaseOrderLoader.Click += new System.EventHandler(btnPurchaseOrderLoader_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1026, 724);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryDetail);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1022, 720);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 144);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1022, 399);
		this.panel12.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1022, 399);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label49);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(0, 543);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1022, 19);
		this.panel13.TabIndex = 49;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		this.grdhistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.grdhistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistoryDetail).BackColor = System.Drawing.Color.White;
		grdhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryDetail.DesignTimeLayout = grdhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistoryDetail.DynamicFiltering = true;
		this.grdhistoryDetail.FilterMode = (FilterMode)2;
		this.grdhistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Location = new System.Drawing.Point(0, 562);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Name = "grdhistoryDetail";
		((ScrollableBase)this.grdhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Size = new System.Drawing.Size(1022, 158);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).TabIndex = 0;
		this.grdhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label55);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 122);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1022, 22);
		this.panel9.TabIndex = 47;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(1, 1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 0;
		this.label55.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(973, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1022, 95);
		this.panel8.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 27);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1144, 66);
		this.groupBox3.TabIndex = 51;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(729, 34);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(726, 17);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 182;
		this.label6.Text = "Item Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1044, 41);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance133;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance138).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(3, 34);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1044, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(963, 20);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(268, 36);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 6;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(136, 36);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(471, 36);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.ToDocNoHistory.TabIndex = 10;
		this.ToDocNoHistory.Tag = "int";
		this.CmbBillToPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBillToPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillToPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillToPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance151).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillToPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillToPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Location = new System.Drawing.Point(543, 34);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Name = "CmbBillToPartyHistory";
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillToPartyHistory).TabIndex = 5;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(400, 36);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(70, 23);
		this.FromDocNoHistory.TabIndex = 8;
		this.FromDocNoHistory.Tag = "int";
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(963, 40);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(543, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(69, 15);
		this.label9.TabIndex = 180;
		this.label9.Text = "Bill To Party";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 17);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 174;
		this.label11.Text = "Date Type";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(909, 35);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(54, 25);
		this.btnShow.TabIndex = 13;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(272, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 171;
		this.label2.Text = "To Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(136, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(63, 15);
		this.label13.TabIndex = 171;
		this.label13.Text = "From Date";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(471, 17);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 15);
		this.label14.TabIndex = 173;
		this.label14.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(400, 17);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label5);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1022, 25);
		this.panel14.TabIndex = 50;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(464, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "Consumeable Store / Demand Purchase Pre_Bill History";
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1022, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1034, 753);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmDeliveryChallanAgainstPurchasePreBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmDeliveryChallanAgainstPurchasePreBill_FormClosing);
		base.Load += new System.EventHandler(frmDeliveryChallanAgainstPurchasePreBill_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SupplierDispatch_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillToPartyHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
