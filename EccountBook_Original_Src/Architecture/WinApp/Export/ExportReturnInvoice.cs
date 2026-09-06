using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Main;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class ExportReturnInvoice : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private DataTable dtDetail = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtPendingGrns = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtWare = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private int RecId;

	private int DocumentTypeId = 242;

	private string ScreenName = "ExportReturnInvoice";

	private int DefaultDaysToLessFromHistoryFromDate;

	public bool UpdateMode;

	private bool FormInitialized;

	private bool FinancialisActiveonCommercialInvoice;

	private bool EnableExportReturnReverseFlow;

	private frmLoadCommercialInvoiceForReturn LoadPO;

	private List<ExportReturnInvoiceDetail> lstRemoveRecord = new List<ExportReturnInvoiceDetail>();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage Form;

	private Panel panel4;

	private Panel panel3;

	private TabPage History;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton BtnAttachment;

	private Label ExpenseVoucherLable;

	private Panel panel5;

	private Label label10;

	private Label label5;

	private Label label15;

	private DateTimePicker datVoucherDate;

	private CtrlGrdBar ctrlGrdBar1;

	private TextBox txtVoucherCode;

	private BackgroundWorker backgroundWorker1;

	private Timer timer1;

	internal GridEX DataGridHistoryDetail;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox2;

	private Label label4;

	private TextBox txtRsAmount;

	private Label label3;

	private TextBox txtExchangeRate;

	private Label label18;

	private CheckBox ChkBox;

	private TextBox txtFcyAmount;

	private Label label1;

	internal UltraCombo CmbRefAccountId;

	private Label label2;

	private Label label7;

	private TextBox txtInvoiceNo;

	internal UltraCombo CmbCurrencyCode;

	private Label label8;

	private TextBox txtCustomerGlAcId;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private TextBox txtExImInvoiceId;

	internal UltraCombo CmbCreditAccount;

	private Label label9;

	private DateTimePicker datInvoiceDate;

	private Label label12;

	private TextBox txtTotalWeight;

	internal GridEX grdPending;

	private Panel panelLabelPending;

	private Label label6;

	internal GridEX grd;

	private Label label11;

	private TextBox txtRemarks;

	private Panel panel10;

	private TextBox txtToDocNoHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private Button btnshowHistory;

	private DateTimePicker ToDateHistory;

	private Label label26;

	private DateTimePicker fromdateHistory;

	private Label label36;

	internal UltraCombo CmbInvoiceNoHistory;

	private Label label38;

	internal UltraCombo CmbCustomerHistory;

	private Label label43;

	private Panel panel8;

	private Label label13;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Panel panel16;

	internal UltraCombo CmbDebitAccountHistory;

	private Label label25;

	internal GridEX DataGridHistory;

	private Label label14;

	private ToolStripButton btnDelete;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnPrint102;

	private Label label20;

	private TextBox txtAddlessAmount;

	private Label label21;

	private TextBox txtFcyamountWithoutAddLess;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX grdpaymentdetail;

	private Panel panel20;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnInvoiceLoader;

	private TabControl tabControl3;

	private TabPage tabDetailH;

	private Panel panel13;

	private Label label59;

	private TabPage tabPaymentDetailH;

	internal GridEX grdPaymentHistory;

	private Panel panel31;

	private Label label74;

	public ExportReturnInvoice(UserAccount puserAccount)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			ScreenName = Conversion.ToString((object)ScreenName);
			dtViewAttachments.Columns.Add("AttchmentPath");
			if (dtSupplier.Columns.Count == 0)
			{
				dtSupplier.Columns.Add("Id", typeof(int));
				dtSupplier.Columns.Add("CompanyName", typeof(string));
				dtSupplier.Columns.Add("PartyCode", typeof(string));
				dtSupplier.Columns.Add("CityName", typeof(string));
				dtSupplier.Columns.Add("MobileNo", typeof(string));
			}
			if (dtWare.Columns.Count == 0)
			{
				dtWare.Columns.Add("Id", typeof(int));
				dtWare.Columns.Add("WareHouseName");
			}
			if (dtJobLot.Columns.Count == 0)
			{
				dtJobLot.Columns.Add("Id", typeof(int));
				dtJobLot.Columns.Add("JobLotDescription");
			}
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ExImInvoiceId", typeof(int));
			dtDetail.Columns.Add("ExImInvoiceDetailId", typeof(int));
			dtDetail.Columns.Add("GrnId", typeof(int));
			dtDetail.Columns.Add("GrnDetailId", typeof(int));
			dtDetail.Columns.Add("GrnNo", typeof(int));
			dtDetail.Columns.Add("WarehouseId", typeof(int));
			dtDetail.Columns.Add("Warehouse", typeof(string));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("CropYear", typeof(string));
			dtDetail.Columns.Add("JobLotId", typeof(int));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackingType", typeof(string));
			dtDetail.Columns.Add("PackUomId", typeof(int));
			dtDetail.Columns.Add("PackUomCode", typeof(string));
			dtDetail.Columns.Add("PackUomEquivalent", typeof(decimal));
			dtDetail.Columns.Add("Qty", typeof(decimal));
			dtDetail.Columns.Add("NetWeight", typeof(decimal));
			dtDetail.Columns.Add("StockWeight", typeof(decimal));
			dtDetail.Columns.Add("Mton", typeof(decimal));
			dtDetail.Columns.Add("RatePrice", typeof(decimal));
			dtDetail.Columns.Add("RateUomId", typeof(int));
			dtDetail.Columns.Add("RateUomCode", typeof(string));
			dtDetail.Columns.Add("RateUomEquivalent", typeof(decimal));
			dtDetail.Columns.Add("FcyAmountWithOutAddLess", typeof(decimal));
			dtDetail.Columns.Add("AddLessAmount", typeof(decimal));
			dtDetail.Columns.Add("FcyAmount", typeof(decimal));
			dtDetail.Columns.Add("LcyAmount", typeof(decimal));
			dtDetail.Columns.Add("RemarksDetail", typeof(string));
			dtPaymentTerm.Columns.Add("Id", typeof(int));
			dtPaymentTerm.Columns.Add("InvoiceId", typeof(int));
			dtPaymentTerm.Columns.Add("InvoiceDetailId", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("PaymentTerm");
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(double));
			dtPaymentTerm.Columns.Add("FcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("LcyAmount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays");
			dtPaymentTerm.Columns.Add("AdjustmentAmount", typeof(double));
			dtPaymentTerm.Columns.Add("Remarks");
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
			SetConfigurationsFromGlobal();
			DataTable dataForDropDownFromExportReturnInvoice = ExportReturnInvoice.GetDataForDropDownFromExportReturnInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null, (string)null);
			if (!EnableExportReturnReverseFlow)
			{
				PendingGrnDbCall();
			}
			return new
			{
				documentNoData = GetDocumentCode(),
				HistoryComboData = dataForDropDownFromExportReturnInvoice
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				BtnSave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				BtnPrint.Enabled = formright.DoHavePrintRights;
				ChkBox.Enabled = formright.DoHavePrintRights;
				ChkBox.Checked = formright.DoHavePrintRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
				ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
				ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
				ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
				ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BindingForConfigurations();
				SupplierDtFillFromGlobal();
				BindSupplierName();
				CurrencyBindFromGlobal();
				CreditGlAccountBindFromGlobal();
				if (!EnableExportReturnReverseFlow)
				{
					PendingGrnGridBind();
				}
				DetailComboDtFillFromGlobal();
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				fromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				BtnSave.Visible = true;
				btnUpdate.Visible = false;
				txtVoucherCode.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void HistoryComboBind(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("Name");
			DataTable dtDebitAccount = dtCustomer.Clone();
			DataTable dtInvoice = dtCustomer.Clone();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
				{
					dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DebitAccount")
				{
					dtDebitAccount.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
				{
					dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtCustomer, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbDebitAccountHistory, dtDebitAccount, "Id", "Name", "DebitAccount", AllColumns: true, Conversion.ToInt(CmbDebitAccountHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoHistory, dtInvoice, "Id", "Name", "InvoiceNo", AllColumns: true, Conversion.ToInt(CmbInvoiceNoHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SetConfigurationsFromGlobal()
	{
		try
		{
			FinancialisActiveonCommercialInvoice = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("FinancialisActiveonCommercialInvoice"));
			EnableExportReturnReverseFlow = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EnableExportReturnReverseFlow"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindingForConfigurations()
	{
		try
		{
			btnInvoiceLoader.Visible = EnableExportReturnReverseFlow;
			panelLabelPending.Visible = !EnableExportReturnReverseFlow;
			((Control)(object)grdPending).Visible = !EnableExportReturnReverseFlow;
			tabControl2.Dock = ((!EnableExportReturnReverseFlow) ? DockStyle.Top : DockStyle.Fill);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtVoucherCode.Text = documentData;
	}

	private string GetDocumentCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)ExportReturnInvoice.GenerateCode(new ExportReturnInvoice
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
			throw new Exception(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			foreach (getGlobalAllSupplierCustomer item in clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => r.CustomerGroupId == 7 && !r.IsSubSupCust))
			{
				dtSupplier.Rows.Add(Conversion.ToInt((object)item.Id), Conversion.ToString((object)item.CompanyName), Conversion.ToString((object)item.PartyCode), Conversion.ToInt((object)item.GlAccountId));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbRefAccountId, dtSupplier, "Id", "CompanyName", "Buyer Name", AllColumns: true, Conversion.ToInt(CmbRefAccountId.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyBindFromGlobal()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CurrencyCode", typeof(string));
			foreach (MultiCurrency dd in clsGlobalVariables.globalMultiCurrency)
			{
				dt.Rows.Add(dd.Id, dd.CurrencyCode);
			}
			InfragisticsHelper.BindAndRetainSelection(CmbCurrencyCode, dt, "Id", "CurrencyCode", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbCurrencyCode.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CreditGlAccountBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[4] { 3, 6, 8, 10 });
			InfragisticsHelper.BindAndRetainSelection(CmbCreditAccount, dt, "Id", "AccountTitle", "AccountTitle", AllColumns: true, Conversion.ToInt(CmbCreditAccount.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailComboDtFillFromGlobal()
	{
		try
		{
			List<GlobalWarehousesWithBranch> lst1 = clsGlobalVariables.globalWarehousesWithBranches;
			DatatableHelper.PopulateDataTable(dtWare, lst1, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			List<GetJobLotGlIdsandName> lst2 = clsGlobalVariables.globalJobLot;
			DatatableHelper.PopulateDataTable(dtJobLot, lst2, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtInvoiceNo == null || txtInvoiceNo.Text == string.Empty || txtInvoiceNo.Text == null)
		{
			MessageBox.Show("Invoice No Field is Required");
			txtInvoiceNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCurrencyCode).ActiveRow == null || Conversion.ToInt(CmbCurrencyCode.Value) == 0)
		{
			MessageBox.Show("Currency Code Field is Required");
			CmbCurrencyCode.Focus();
			return false;
		}
		if (txtExchangeRate.Text == string.Empty || txtExchangeRate.Text == "0" || Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m)
		{
			MessageBox.Show("ExchangeRate Field is Required");
			txtExchangeRate.Focus();
			return false;
		}
		if (txtFcyAmount.Text == string.Empty || txtFcyAmount.Text == "0" || Conversion.ToDecimal((object)txtFcyAmount.Text) == 0m)
		{
			MessageBox.Show("FcyAmount Field is Required");
			txtFcyAmount.Focus();
			return false;
		}
		if (txtRsAmount.Text == string.Empty || txtRsAmount.Text == "0" || Conversion.ToDecimal((object)txtRsAmount.Text) == 0m)
		{
			MessageBox.Show("RsAmount Field is Required");
			txtRsAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRefAccountId).ActiveRow == null || Conversion.ToInt(CmbRefAccountId.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			CmbRefAccountId.Focus();
			return false;
		}
		return true;
	}

	private void BindGrids()
	{
		try
		{
			if (dtDetail != null && dtDetail.Columns.Count > 0)
			{
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings(grd);
			}
			if (dtPaymentTerm != null && dtPaymentTerm.Columns.Count > 0)
			{
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm(grdpaymentdetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ExImInvoiceId", "ExImInvoiceDetailId", "GrnId", "GrnDetailId", "ItemId", "RateUomId", "PackUomId", "CropYearId", "JobLotId",
				"PackingTypeId", "WarehouseId", "PackUomEquivalent", "RateUomEquivalent"
			};
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)grid).Name == "grd")
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"RatePrice",
						(EditType)1
					},
					{
						"Remarks",
						(EditType)1
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 3, 2, hiddenColumns, editableColumns);
			if (EnableExportReturnReverseFlow)
			{
				grid.RootTable.Columns["GrnNo"].Visible = false;
				if (((Control)(object)grid).Name == "grd")
				{
					grid.RootTable.Columns["Qty"].EditType = (EditType)1;
					GridEX_Helper.AddButton(grid, "Delete", "X", 30, 0);
				}
			}
			grid.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			grid.RootTable.Columns["PackUomCode"].Caption = "Pack Uom";
			if (((Control)(object)grid).Name == "grd")
			{
				ctrlGrdBar1_Load(null, null);
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Qty")
				{
					decimal num = Conversion.ToDecimal(item.Cells["Qty"].Value);
					decimal OuterUom = Conversion.ToDecimal(item.Cells["PackUomEquivalent"].Value);
					decimal NetWeight2 = num * OuterUom;
					item.Cells["NetWeight"].Value = Conversion.ToString((object)Math.Round(NetWeight2));
					item.Cells["StockWeight"].Value = Conversion.ToString((object)Math.Round(NetWeight2));
					item.Cells["Mton"].Value = Conversion.ToString((object)Math.Round(NetWeight2 / 1000m));
					decimal RatePrice2 = Conversion.ToDecimal(item.Cells["RatePrice"].Value);
					decimal PackUom2 = Conversion.ToDecimal(item.Cells["RateUomEquivalent"].Value);
					decimal FcyAmt2 = default(decimal);
					if (NetWeight2 > 0m && RatePrice2 > 0m && PackUom2 > 0m)
					{
						decimal FcyAmtWOAdls2 = NetWeight2 / PackUom2 * RatePrice2;
						item.Cells["FcyAmountWithOutAddLess"].Value = FcyAmtWOAdls2;
						decimal AddLessAmount2 = Conversion.ToDecimal(item.Cells["AddLessAmount"].Value);
						FcyAmt2 = FcyAmtWOAdls2 + AddLessAmount2;
						item.Cells["FcyAmount"].Value = FcyAmt2;
					}
					else
					{
						FcyAmt2 = default(decimal);
						item.Cells["FcyAmountWithOutAddLess"].Value = FcyAmt2;
						item.Cells["FcyAmount"].Value = FcyAmt2;
					}
					decimal ExchangeRate2 = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
					decimal LcyAmount2 = FcyAmt2 * ExchangeRate2;
					item.Cells["LcyAmount"].Value = LcyAmount2;
				}
				if (e.Column.Key == "RatePrice")
				{
					decimal NetWeight = Conversion.ToDecimal(item.Cells["NetWeight"].Value);
					decimal RatePrice = Conversion.ToDecimal(item.Cells["RatePrice"].Value);
					decimal PackUom = Conversion.ToDecimal(item.Cells["RateUomEquivalent"].Value);
					decimal FcyAmt = default(decimal);
					if (NetWeight > 0m && RatePrice > 0m && PackUom > 0m)
					{
						decimal FcyAmtWOAdls = NetWeight / PackUom * RatePrice;
						item.Cells["FcyAmountWithOutAddLess"].Value = FcyAmtWOAdls;
						decimal AddLessAmount = Conversion.ToDecimal(item.Cells["AddLessAmount"].Value);
						FcyAmt = FcyAmtWOAdls + AddLessAmount;
						item.Cells["FcyAmount"].Value = FcyAmt;
					}
					else
					{
						FcyAmt = default(decimal);
						item.Cells["FcyAmountWithOutAddLess"].Value = FcyAmt;
						item.Cells["FcyAmount"].Value = FcyAmt;
					}
					decimal ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
					decimal LcyAmount = FcyAmt * ExchangeRate;
					item.Cells["LcyAmount"].Value = LcyAmount;
				}
				grd.UpdateData();
			}
			AmountCalculation();
			UpdatePaymentTerms(grdpaymentdetail, Conversion.ToDouble((object)txtFcyAmount.Text));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExportReturnInvoiceDetail vd = new ExportReturnInvoiceDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				FillDetailListCommonForInsertAndDelete(vd, r);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				dtDetail.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			grd.UpdateData();
			AmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm(GridEX grid)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId", "InvoiceDetailId", "PaymentTermId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"AdjustmentAmount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grid, 3, 2, hiddenColumns, editableColumns);
			grid.RootTable.Columns["PaymentTerm"].Width = 350;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpaymentdetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdpaymentdetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "AdjustmentAmount")
			{
				decimal AdjustmentAmount = Conversion.ToDecimal(r.Cells["AdjustmentAmount"].Value);
				decimal FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
				if (Conversion.ToInt(r.Cells["PaymentTermId"].Value) == 1)
				{
					AdjustmentAmount = FcyAmount;
					r.Cells["AdjustmentAmount"].Value = AdjustmentAmount;
					MessageBox.Show("you Cannot Change AdjustmentAmount For PaymentType Advance");
				}
				if (AdjustmentAmount > FcyAmount)
				{
					r.Cells["AdjustmentAmount"].Value = FcyAmount;
					MessageBox.Show("AdjustmentAmount Cannot greater Than Invoice's FcyAmount");
				}
			}
			grdpaymentdetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdatePaymentTerms(GridEX grid, double netTotalAmount)
	{
	}

	private void PendingGrnDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtPendingGrns.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			dtPendingGrns = ExportReturnGrn.PendingGrnDataForExportReturnInvoice(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingGrnGridBind()
	{
		try
		{
			if (dtPendingGrns.Rows.Count > 0)
			{
				DataTable grnDataTable = new DataTable();
				grnDataTable.Columns.Add("GrnId", typeof(int));
				grnDataTable.Columns.Add("GrnDetailId", typeof(int));
				grnDataTable.Columns.Add("GrnNo", typeof(string));
				grnDataTable.Columns.Add("GrnDate", typeof(DateTime));
				grnDataTable.Columns.Add("ExImInvoiceId", typeof(int));
				grnDataTable.Columns.Add("InvoiceDetailId", typeof(int));
				grnDataTable.Columns.Add("InvoiceNo", typeof(string));
				grnDataTable.Columns.Add("InvoiceDate", typeof(DateTime));
				grnDataTable.Columns.Add("LCOrderId", typeof(int));
				grnDataTable.Columns.Add("LcOrderNo", typeof(string));
				grnDataTable.Columns.Add("LcOrderDate", typeof(DateTime));
				grnDataTable.Columns.Add("SupplierCustomerId", typeof(int));
				grnDataTable.Columns.Add("CustomerName", typeof(string));
				grnDataTable.Columns.Add("LoadingPortId", typeof(int));
				grnDataTable.Columns.Add("LoadingPort", typeof(string));
				grnDataTable.Columns.Add("DestinationPortId", typeof(int));
				grnDataTable.Columns.Add("DestinationPort", typeof(string));
				grnDataTable.Columns.Add("WarehouseId", typeof(int));
				grnDataTable.Columns.Add("WareHouseName", typeof(string));
				grnDataTable.Columns.Add("ItemId", typeof(int));
				grnDataTable.Columns.Add("ItemName", typeof(string));
				grnDataTable.Columns.Add("ItemCode", typeof(string));
				grnDataTable.Columns.Add("CropYearId", typeof(int));
				grnDataTable.Columns.Add("CropYear", typeof(string));
				grnDataTable.Columns.Add("JobLotId", typeof(int));
				grnDataTable.Columns.Add("JobLotDescription", typeof(string));
				grnDataTable.Columns.Add("PackingTypeId", typeof(int));
				grnDataTable.Columns.Add("PackingType", typeof(string));
				grnDataTable.Columns.Add("PackUomId", typeof(int));
				grnDataTable.Columns.Add("PackUomCode", typeof(string));
				grnDataTable.Columns.Add("PackUomEquivalent", typeof(decimal));
				grnDataTable.Columns.Add("ItemQty", typeof(decimal));
				grnDataTable.Columns.Add("NetWeight", typeof(decimal));
				grnDataTable.Columns.Add("MTon", typeof(decimal));
				grnDataTable.Columns.Add("StockWeight", typeof(decimal));
				grnDataTable.Columns.Add("RatePrice", typeof(decimal));
				grnDataTable.Columns.Add("RateUomId", typeof(int));
				grnDataTable.Columns.Add("RateUomCode", typeof(string));
				grnDataTable.Columns.Add("RateUomEquivalent", typeof(decimal));
				grnDataTable.Columns.Add("FcyAmount", typeof(decimal));
				grnDataTable.Columns.Add("RemarksDetail", typeof(string));
				grnDataTable.Columns.Add("EntryUser", typeof(string));
				grnDataTable.Columns.Add("ModifyUser", typeof(string));
				grnDataTable.Columns.Add("ApprovedUser", typeof(string));
				foreach (DataRow row in dtPendingGrns.Rows)
				{
					grnDataTable.Rows.Add(row["GrnId"], row["GrnDetailId"], row["GrnNo"], row["GrnDate"], row["ExImInvoiceId"], row["InvoiceDetailId"], row["InvoiceNo"], row["InvoiceDate"], row["LCOrderId"], row["LcOrderNo"], row["LcOrderDate"], row["SupplierCustomerId"], row["CustomerName"], row["LoadingPortId"], row["LoadingPort"], row["DestinationPortId"], row["DestinationPort"], row["WarehouseId"], row["WareHouseName"], row["ItemId"], row["ItemName"], row["ItemCode"], row["CropYearId"], row["CropYear"], row["JobLotId"], row["JobLotDescription"], row["PackingTypeId"], row["PackingType"], row["PackUomId"], row["PackUomCode"], row["PackUomEquivalent"], row["ItemQty"], row["NetWeight"], row["MTon"], row["StockWeight"], row["RatePrice"], row["RateUomId"], row["RateUomCode"], row["RateUomEquivalent"], row["FcyAmount"], row["RemarksDetail"], row["EntryUser"], row["ModifyUser"], row["ApprovedUser"]);
				}
				grdPending.DataSource = grnDataTable;
				grdPending.RetrieveStructure();
				grdPendingSetting();
			}
			else
			{
				grdPending.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"GrnId", "GrnDetailId", "ExImInvoiceId", "InvoiceDetailId", "InvoiceDate", "LCOrderId", "LcOrderDate", "SupplierCustomerId", "LoadingPortId", "DestinationPortId",
				"WarehouseId", "ItemId", "CropYearId", "JobLotId", "PackingTypeId", "PackUomId", "PackUomEquivalent", "RateUomId", "RateUomEquivalent"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdPending, 2, 3, hiddenColumns);
			grdPending.RootTable.Columns["PackUomCode"].Caption = "Pack Uom";
			grdPending.RootTable.Columns["RateUomCode"].Caption = "Rate Uom";
			GridEX_Helper.AddButton(grdPending, "Load", "Load", 40, 0);
			GridEX_Helper.AddSelectButton(grdPending, "Select", 40, 1);
			grdPending.FrozenColumns = 2;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPending_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPending.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "Load")
			{
				LoadDataFronGridPending();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataFronGridPending()
	{
		try
		{
			if (grdPending.GetCheckedRows().Count() > 0)
			{
				string GrnIds = "";
				int InvoiceId = 0;
				GridEXRow[] checkedRows = grdPending.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					int CurrInvoiceId = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
					if (InvoiceId == 0)
					{
						InvoiceId = CurrInvoiceId;
					}
					if (InvoiceId != CurrInvoiceId)
					{
						GrnIds = "";
						MessageBox.Show("You Can Only Select Rows Of Same Invoice");
						return;
					}
					GrnIds = GrnIds + "," + Conversion.ToInt(r.Cells["GrnId"].Value);
				}
				GrnIds = GrnIds.TrimStart(',');
				if (!string.IsNullOrEmpty(GrnIds))
				{
					PendingDataLoadInDetail(GrnIds);
				}
				if (InvoiceId != 0)
				{
					BindPaymentDetailByInvoiceId(InvoiceId);
				}
				NetFcyAmountCalculation();
				AddLessProportionInGrid();
				AmountCalculation();
				return;
			}
			throw new Exception("Please Check the Row First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingDataLoadInDetail(string GrnIds)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				grd.ClearStructure();
				dtDetail.Rows.Clear();
			}
			else
			{
				if (MessageBox.Show("Grid Detail Already have Record,Do You Want To Reset and Load Again???", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
					{
						ExportReturnInvoiceDetail vd = new ExportReturnInvoiceDetail();
						vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						FillDetailListCommonForInsertAndDelete(vd, r2);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtDetail.Rows[r2.RowIndex].Delete();
						r2.Delete();
					}
					else
					{
						r2.Delete();
					}
					grd.UpdateData();
				}
			}
			DataRow[] matchingRows = dtPendingGrns.Select("GrnId IN (" + GrnIds + ")");
			if (matchingRows.Length != 0)
			{
				dtDetail.Rows.Clear();
				txtExImInvoiceId.Text = Conversion.ToString(matchingRows[0]["ExImInvoiceId"]);
				txtInvoiceNo.Text = Conversion.ToString(matchingRows[0]["InvoiceNo"]);
				datInvoiceDate.Value = Conversion.ToDateTime(matchingRows[0]["InvoiceDate"]);
				CmbCurrencyCode.Value = Conversion.ToInt(matchingRows[0]["FcurrencyId"]);
				txtExchangeRate.Text = Conversion.ToString(matchingRows[0]["ConversionRate"]);
				CmbRefAccountId.Value = Conversion.ToInt(matchingRows[0]["SupplierCustomerId"]);
				txtCustomerGlAcId.Text = Conversion.ToString(matchingRows[0]["CustomerGlAcId"]);
				CmbCreditAccount.Value = Conversion.ToInt(matchingRows[0]["CreditAccountId"]);
				DataRow[] array = matchingRows;
				foreach (DataRow r in array)
				{
					dtDetail.Rows.Add(0, r["ExImInvoiceId"], r["InvoiceDetailId"], r["GrnId"], r["GrnDetailId"], r["GrnNo"], r["WarehouseId"], r["WareHouseName"], r["ItemId"], r["ItemName"], r["ItemCode"], r["CropYearId"], r["CropYear"], r["JobLotId"], r["JobLotDescription"], r["PackingTypeId"], r["PackingType"], r["PackUomId"], r["PackUomCode"], r["PackUomEquivalent"], r["ItemQty"], r["NetWeight"], r["StockWeight"], r["MTon"], r["RatePrice"], r["RateUomId"], r["RateUomCode"], r["RateUomEquivalent"], r["FcyAmount"], 0, r["FcyAmount"], 0, r["RemarksDetail"]);
				}
				if (dtDetail.Rows.Count > 0)
				{
					grd.DataSource = dtDetail;
					grd.RetrieveStructure();
					grdSettings(grd);
					AmountCalculation();
				}
				else
				{
					grd.ClearStructure();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPaymentDetailByInvoiceId(int ExImInvoiceId)
	{
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dtPaymentTerm.Rows.Clear();
			DataTable dt = ExImInvoice.InvoicePaymentTermsDetailByInvoiceId(ExImInvoiceId);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow r2 in dt.Rows)
			{
				if (Conversion.ToInt(r2["PaymentTermId"]) == 1)
				{
					dtPaymentTerm.Rows.Add(0, r2["ExImInvoiceId"], r2["Id"], r2["PaymentTermId"], r2["PaymentTerm"], r2["PrcntOfTotal"], r2["FcyAmount"], Conversion.ToDouble(r2["FcyAmount"]) * Conversion.ToDouble((object)txtExchangeRate.Text), r2["DueDays"], r2["FcyAmount"], r2["PaymentRemarks"]);
				}
				else
				{
					dtPaymentTerm.Rows.Add(0, r2["ExImInvoiceId"], r2["Id"], r2["PaymentTermId"], r2["PaymentTerm"], r2["PrcntOfTotal"], r2["FcyAmount"], Conversion.ToDouble(r2["FcyAmount"]) * Conversion.ToDouble((object)txtExchangeRate.Text), r2["DueDays"], 0, r2["PaymentRemarks"]);
				}
			}
			if (dtPaymentTerm.Rows.Count > 0)
			{
				grdpaymentdetail.DataSource = dtPaymentTerm;
				grdpaymentdetail.RetrieveStructure();
				grdSettingsPaymentTerm(grdpaymentdetail);
				if (grdpaymentdetail.RowCount != 1)
				{
					return;
				}
				GridEXRow r = grdpaymentdetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					decimal totalFcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text.Trim());
					decimal FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					if (totalFcyAmount <= FcyAmount)
					{
						r.Cells["AdjustmentAmount"].Value = totalFcyAmount;
					}
				}
				grdpaymentdetail.UpdateData();
			}
			else
			{
				grdpaymentdetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			RecId = 0;
			UpdateMode = false;
			BtnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtExImInvoiceId.Text = "0";
			txtInvoiceNo.Text = string.Empty;
			datInvoiceDate.Value = DateTime.Now;
			txtFcyamountWithoutAddLess.Text = string.Empty;
			txtAddlessAmount.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			((Control)(object)CmbCurrencyCode).Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtRsAmount.Text = string.Empty;
			txtTotalWeight.Text = string.Empty;
			txtCustomerGlAcId.Text = "0";
			((Control)(object)CmbCreditAccount).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			dtDetail.Clear();
			dtPaymentTerm.Clear();
			BindGrids();
			UpdateDocumentNoUI(GetDocumentCode());
			if (!EnableExportReturnReverseFlow)
			{
				PendingGrnDbCall();
				PendingGrnGridBind();
			}
			datVoucherDate.Focus();
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
			Reset();
			((Control)(object)CmbRefAccountId).Text = string.Empty;
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
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems", "MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			SetConfigurationsFromGlobal();
			BindingForConfigurations();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CurrencyBindFromGlobal();
			CreditGlAccountBindFromGlobal();
			if (!EnableExportReturnReverseFlow)
			{
				PendingGrnGridBind();
			}
			DetailComboDtFillFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportReturnInvoice_FormClosed(object sender, FormClosedEventArgs e)
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

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				throw new Exception("Please Check Detail Grid");
			}
			if (!FormValidation())
			{
				return;
			}
			ExportReturnInvoice forwarding = new ExportReturnInvoice();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				forwarding.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = UserAccount.BranchesId;
			forwarding.ProjectsId = UserAccount.BranchesId;
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.DocumentTypeId = DocumentTypeId;
			forwarding.DocNo = Conversion.ToInt((object)txtVoucherCode.Text.Trim());
			forwarding.DocDate = Conversion.ToDateTime((object)datVoucherDate.Value);
			forwarding.ExImInvoiceId = Conversion.ToInt((object)txtExImInvoiceId.Text);
			forwarding.CurrencyId = Conversion.ToInt(CmbCurrencyCode.Value);
			forwarding.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text.Trim());
			forwarding.FcyAmountWithOutAddLess = Conversion.ToDecimal((object)txtFcyamountWithoutAddLess.Text);
			forwarding.AddLessAmount = Conversion.ToDecimal((object)txtAddlessAmount.Text);
			forwarding.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			forwarding.LcyAmount = Conversion.ToDecimal((object)txtRsAmount.Text);
			forwarding.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			forwarding.NetWeight = Conversion.ToDecimal((object)txtTotalWeight.Text.Trim());
			forwarding.SupplierCustomerId = Conversion.ToInt(CmbRefAccountId.Value);
			forwarding.DebitAccounId = Conversion.ToInt(CmbCreditAccount.Value);
			forwarding.EntryUserId = UserAccount.ID;
			forwarding.ModifyUserId = UserAccount.ID;
			forwarding.ApprovedUserId = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.ApprovedDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.InvoiceNo = Conversion.ToString((object)txtInvoiceNo.Text);
			forwarding.CurrencyName = ((Control)(object)CmbCurrencyCode).Text.Trim();
			forwarding.ExportReturnInvoiceDetailList = new List<ExportReturnInvoiceDetail>();
			forwarding.ExportReturnInvoicePaymentDetailList = new List<ExportReturnInvoicePaymentDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExportReturnInvoiceDetail vd = new ExportReturnInvoiceDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r2);
				FormHelper.ValidateField(vd.ExImInvoiceDetailId, "Invoice Detail ID", r2.RowIndex);
				FormHelper.ValidateField(vd.ExImInvoiceId, "Invoice ID", r2.RowIndex);
				if (!EnableExportReturnReverseFlow)
				{
					FormHelper.ValidateField(vd.GrnId, "GRN ID", r2.RowIndex);
					FormHelper.ValidateField(vd.GrnDetailId, "GRN Detail ID", r2.RowIndex);
				}
				FormHelper.ValidateField(vd.WarehouseId, "Warehouse", r2.RowIndex);
				FormHelper.ValidateField(vd.ItemId, "Item Name", r2.RowIndex);
				FormHelper.ValidateField(vd.CropYearId, "Crop Year", r2.RowIndex);
				FormHelper.ValidateField(vd.JobLotId, "Job Lot", r2.RowIndex);
				FormHelper.ValidateField(vd.PackingTypeId, "Packing Type", r2.RowIndex);
				FormHelper.ValidateField(vd.PackUomId, "Pack UOM", r2.RowIndex);
				FormHelper.ValidateField(vd.Qty, "Qty", r2.RowIndex);
				FormHelper.ValidateField(vd.NetWeight, "Net Weight", r2.RowIndex);
				FormHelper.ValidateField(vd.Mton, "M.Ton", r2.RowIndex);
				FormHelper.ValidateField(vd.StockWeight, "Stock Weight", r2.RowIndex);
				FormHelper.ValidateField(vd.RatePrice, "Rate Price", r2.RowIndex);
				FormHelper.ValidateField(vd.RateUomId, "Rate UOM", r2.RowIndex);
				FormHelper.ValidateField(vd.FcyAmount, "FCY Amount", r2.RowIndex);
				forwarding.ExportReturnInvoiceDetailList.Add(vd);
			}
			if (forwarding.ExportReturnInvoiceDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			if (RecId > 0 && lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					forwarding.ExportReturnInvoiceDetailList.Add(lstRemoveRecord[i]);
				}
			}
			if (grdpaymentdetail.RowCount > 0)
			{
				double TotalAdjustmentAmountInPaymentGrid = Math.Round(GridEX_Helper.GetColumnSum(grdpaymentdetail, "AdjustmentAmount"), 0);
				double InvoiceAmount = Math.Round(Conversion.ToDouble((object)forwarding.FcyAmount), 0);
				if (TotalAdjustmentAmountInPaymentGrid != InvoiceAmount)
				{
					throw new Exception($"Total InvoiceAmount {InvoiceAmount} and Total Payment UtilizeAmount {TotalAdjustmentAmountInPaymentGrid} Not equal Please Check");
				}
				rows = grdpaymentdetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExportReturnInvoicePaymentDetail vdp = new ExportReturnInvoicePaymentDetail();
					vdp.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vdp.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vdp.ExImInvoicePaymentDetailId = Conversion.ToInt(r.Cells["InvoiceDetailId"].Value);
					vdp.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vdp.PrcntOfTotal = Conversion.ToDouble(r.Cells["%OfTotal"].Value);
					vdp.FcyAmount = Conversion.ToDouble(r.Cells["FcyAmount"].Value);
					vdp.LcyAmount = Conversion.ToDouble(r.Cells["LcyAmount"].Value);
					vdp.AdjustmentAmount = Conversion.ToDouble(r.Cells["AdjustmentAmount"].Value);
					vdp.DueDays = Conversion.ToInt(r.Cells["DueDays"].Value);
					vdp.PaymentRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vdp.SortNo = r.RowIndex + 1;
					vdp.EntryUserId = UserAccount.ID;
					vdp.ModifyUserId = UserAccount.ID;
					FormHelper.ValidateField(vdp.PaymentTermId, "PaymentTerm", r.RowIndex, "PaymentTerm Detail");
					FormHelper.ValidateField(vdp.FcyAmount, "FcyAmount", r.RowIndex, "PaymentTerm Detail");
					forwarding.ExportReturnInvoicePaymentDetailList.Add(vdp);
				}
			}
			forwarding.AttachmentsList = new List<DMSAttachments>();
			forwarding.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExportReturnInvoice>(forwarding, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExportReturnInvoice.Save(forwarding);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update SuccessFully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, forwarding.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save SuccessFully");
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.ExportReturnInvoiceSlip242(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Save_Click(object sender, EventArgs e)
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

	private void Update_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(ExportReturnInvoiceDetail vd, GridEXRow r)
	{
		vd.ExImInvoiceDetailId = Conversion.ToInt(r.Cells["ExImInvoiceDetailId"].Value);
		vd.ExImInvoiceId = Conversion.ToInt(r.Cells["ExImInvoiceId"].Value);
		vd.GrnId = Conversion.ToInt(r.Cells["GrnId"].Value);
		vd.GrnDetailId = Conversion.ToInt(r.Cells["GrnDetailId"].Value);
		vd.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
		vd.WareHouseName = Conversion.ToString(r.Cells["Warehouse"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLot = Conversion.ToString(r.Cells["JobLot"].Value);
		vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.PackingType = Conversion.ToString(r.Cells["PackingType"].Value);
		vd.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
		vd.PackUomCode = Conversion.ToString(r.Cells["PackUomCode"].Value);
		vd.PackUomEquivalent = Conversion.ToDouble(r.Cells["PackUomEquivalent"].Value);
		vd.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text.Trim());
		vd.NetWeight = Conversion.ToDecimal((object)r.Cells["NetWeight"].Text.Trim());
		vd.Mton = Conversion.ToDecimal((object)r.Cells["Mton"].Text.Trim());
		vd.StockWeight = Conversion.ToDecimal((object)r.Cells["StockWeight"].Text.Trim());
		vd.RatePrice = Conversion.ToDecimal((object)r.Cells["RatePrice"].Text.Trim());
		vd.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text.Trim());
		vd.RateUomCode = Conversion.ToString(r.Cells["RateUomCode"].Value);
		vd.RateUomEquivalent = Conversion.ToDouble(r.Cells["RateUomEquivalent"].Value);
		vd.FcyAmountWithOutAddLess = Conversion.ToDecimal((object)r.Cells["FcyAmountWithOutAddLess"].Text.Trim());
		vd.AddLessAmount = Conversion.ToDecimal((object)r.Cells["AddLessAmount"].Text.Trim());
		vd.FcyAmount = Conversion.ToDecimal((object)r.Cells["FcyAmount"].Text.Trim());
		vd.CurrencyId = Conversion.ToInt(CmbCurrencyCode.Value);
		vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
		vd.LcyAmount = Conversion.ToDecimal((object)r.Cells["LcyAmount"].Text.Trim());
		vd.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
	}

	public void ReadById(int ID)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				Reset();
				RecId = ID;
				ExportReturnInvoice exImLc = ExportReturnInvoice.ReadById(RecId);
				if (exImLc != null)
				{
					tabControl1.SelectedIndex = 0;
					BtnSave.Visible = false;
					btnUpdate.Visible = true;
					btnDelete.Visible = true;
					UpdateMode = true;
					txtVoucherCode.Text = Conversion.ToString((object)exImLc.DocNo);
					datVoucherDate.Value = Conversion.ToDateTime((object)exImLc.DocDate);
					txtExImInvoiceId.Text = Conversion.ToString((object)exImLc.ExImInvoiceId);
					txtInvoiceNo.Text = Conversion.ToString((object)exImLc.InvoiceNo);
					datInvoiceDate.Value = Conversion.ToDateTime((object)exImLc.InvoiceDate);
					txtFcyamountWithoutAddLess.Text = Conversion.ToDecimal((object)exImLc.FcyAmountWithOutAddLess).ToString(clsGlobalVariables.stringFormatsingleForFcy);
					txtAddlessAmount.Text = Conversion.ToDecimal((object)exImLc.AddLessAmount).ToString(clsGlobalVariables.stringFormatsingleForFcy);
					txtFcyAmount.Text = Conversion.ToDecimal((object)exImLc.FcyAmount).ToString(clsGlobalVariables.stringFormatsingleForFcy);
					CmbCurrencyCode.Value = Conversion.ToInt((object)exImLc.CurrencyId);
					txtExchangeRate.Text = Conversion.ToDecimal((object)exImLc.ExchangeRate).ToString("#,#.####");
					txtRsAmount.Text = Conversion.ToDecimal((object)exImLc.LcyAmount).ToString(clsGlobalVariables.stringFormatsingle);
					txtRemarks.Text = Conversion.ToString((object)exImLc.RemarksHeader);
					txtTotalWeight.Text = Conversion.ToDecimal((object)exImLc.NetWeight).ToString("##,#.####");
					CmbRefAccountId.Value = Conversion.ToInt((object)exImLc.SupplierCustomerId);
					CmbCreditAccount.Value = Conversion.ToInt((object)exImLc.DebitAccounId);
					dtDetail.Rows.Clear();
					FillDetailFromListCommonForReadById(exImLc, dtDetail);
					dtPaymentTerm.Rows.Clear();
					FillPaymentDetailFromListCommonForReadById(exImLc, dtPaymentTerm);
					BindGrids();
					FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
					txtExchangeRate_TextChanged(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvoucherno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Delete because RecId No Found");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ExportReturnInvoice.DeleteByID(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId, UserAccount.ID, RecId);
				MessageBox.Show("Deleted Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(ExportReturnInvoice invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExportReturnInvoiceDetailList : null) == null || invGrn.ExportReturnInvoiceDetailList.Count == 0)
		{
			return;
		}
		foreach (ExportReturnInvoiceDetail detail in invGrn.ExportReturnInvoiceDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.ExImInvoiceId, detail.ExImInvoiceDetailId, detail.GrnId, detail.GrnDetailId, detail.GrnNo, detail.WarehouseId, detail.WareHouseName, detail.ItemId, detail.ItemName,
				detail.ItemCode, detail.CropYearId, detail.CropYear, detail.JobLotId, detail.JobLot, detail.PackingTypeId, detail.PackingType, detail.PackUomId, detail.PackUomCode, detail.PackUomEquivalent,
				detail.Qty, detail.NetWeight, detail.StockWeight, detail.Mton, detail.RatePrice, detail.RateUomId, detail.RateUomCode, detail.RateUomEquivalent, detail.FcyAmountWithOutAddLess, detail.AddLessAmount,
				detail.FcyAmount, detail.LcyAmount, detail.RemarksDetail
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void FillPaymentDetailFromListCommonForReadById(ExportReturnInvoice invGrn, DataTable dt)
	{
		if (((invGrn != null) ? invGrn.ExportReturnInvoicePaymentDetailList : null) == null || invGrn.ExportReturnInvoicePaymentDetailList.Count == 0)
		{
			return;
		}
		foreach (ExportReturnInvoicePaymentDetail detail in invGrn.ExportReturnInvoicePaymentDetailList)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.ExImInvoiceId, detail.ExImInvoicePaymentDetailId, detail.PaymentTermId, detail.PaymentTerm, detail.PrcntOfTotal, detail.FcyAmount, detail.LcyAmount, detail.DueDays, detail.AdjustmentAmount,
				detail.PaymentRemarks
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			fromdateHistory.Focus();
		}
		else
		{
			datVoucherDate.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ResetHistory()
	{
		try
		{
			fromdateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			txtFromNoHistory.Clear();
			txtToDocNoHistory.Clear();
			((Control)(object)CmbInvoiceNoHistory).Text = string.Empty;
			((Control)(object)CmbDebitAccountHistory).Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			DataGridHistoryDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(ExportReturnInvoice.GetDataForDropDownFromExportReturnInvoice(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null, (string)null));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.BranchesId = UserAccount.BranchesId;
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			order.DocumentTypeId = DocumentTypeId;
			order.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (fromdateHistory.Checked)
			{
				order.FromDate = Conversion.ToDateTime((object)fromdateHistory.Value);
			}
			if (ToDateHistory.Checked)
			{
				order.ToDate = Conversion.ToDateTime((object)ToDateHistory.Value);
			}
			order.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			order.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			order.DebitAccountId = Conversion.ToInt(CmbDebitAccountHistory.Value);
			order.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			order.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtGrid = ExportReturnInvoice.FormHistory(order);
			if (dtGrid.Rows.Count > 0)
			{
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocNo", typeof(string));
					dtHistoryGrid.Columns.Add("DocDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("SupplierCustomerId", typeof(int));
					dtHistoryGrid.Columns.Add("CustomerName", typeof(string));
					dtHistoryGrid.Columns.Add("DebitAccountId", typeof(int));
					dtHistoryGrid.Columns.Add("DebitAccountTitle", typeof(string));
					dtHistoryGrid.Columns.Add("CurrencyId", typeof(int));
					dtHistoryGrid.Columns.Add("CurrencyName", typeof(string));
					dtHistoryGrid.Columns.Add("FcyAmountWithOutAddLess", typeof(decimal));
					dtHistoryGrid.Columns.Add("AddLessAmount", typeof(decimal));
					dtHistoryGrid.Columns.Add("FcyAmount", typeof(decimal));
					dtHistoryGrid.Columns.Add("ExchangeRate", typeof(decimal));
					dtHistoryGrid.Columns.Add("LcyAmount", typeof(decimal));
					dtHistoryGrid.Columns.Add("NetWeight", typeof(decimal));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUser", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyUser", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ApprovedUser", typeof(string));
					dtHistoryGrid.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("IsApproved", typeof(bool));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistoryGrid.Clear();
				foreach (DataRow row in dtGrid.Rows)
				{
					dtHistoryGrid.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["DocumentTypeId"], row["SupplierCustomerId"], row["CustomerName"], row["DebitAccountId"], row["DebitAccountTitle"], row["CurrencyId"], row["CurrencyName"], row["FcyAmountWithOutAddLess"], row["AddLessAmount"], row["FcyAmount"], row["ExchangeRate"], row["LcyAmount"], row["NetWeight"], row["RemarksHeader"], row["EntryDate"], row["EntryUser"], row["ModifyUser"], row["ModifyDate"], row["ApprovedUser"], row["ApprovedDate"], row["IsApproved"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistoryDetail.ClearStructure();
				grdPaymentHistory.ClearStructure();
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "SupplierCustomerId", "DebitAccountId", "CurrencyId" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 4, hiddenColumns);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 1);
			GridEX_Helper.AddButton(DataGridHistory, "Voucher", "Voucher", 50, 2);
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Position = 3;
			DataGridHistory.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.ExportReturnInvoiceSlip242(CurrId);
				}
				if (e.Column.Key == "Voucher")
				{
					ExportReturnVoucherSlip(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
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
			ExportReturnInvoice Obj = ExportReturnInvoice.ReadById(Id);
			if (Obj == null || Obj.ExportReturnInvoiceDetailList.Count == 0)
			{
				DataGridHistoryDetail.ClearStructure();
				grdPaymentHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtDetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			DataGridHistoryDetail.DataSource = dtDetailHistory;
			DataGridHistoryDetail.RetrieveStructure();
			grdSettings(DataGridHistoryDetail);
			DataTable dtPaymentH = dtPaymentTerm.Clone();
			FillPaymentDetailFromListCommonForReadById(Obj, dtPaymentH);
			grdPaymentHistory.DataSource = dtPaymentH;
			grdPaymentHistory.RetrieveStructure();
			grdSettingsPaymentTerm(grdPaymentHistory);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdPending, dtPendingGrns);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnAttachment_Click(object sender, EventArgs e)
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

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtExchangeRate.Text) > 0.0 && Conversion.ToDouble((object)txtFcyAmount.Text) > 0.0)
			{
				double fcyamount = Conversion.ToDouble((object)txtFcyAmount.Text);
				double ExRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				if (fcyamount > 0.0 && ExRate > 0.0)
				{
					txtRsAmount.Text = (fcyamount * ExRate).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtRsAmount.Text = "0";
				}
			}
			else
			{
				txtRsAmount.Text = "0";
			}
			AddLessProportionInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetFcyAmountCalculation()
	{
		try
		{
			double fcyamount = Conversion.ToDouble((object)txtFcyamountWithoutAddLess.Text);
			if (fcyamount > 0.0)
			{
				double addlss = Conversion.ToDouble((object)txtAddlessAmount.Text);
				txtFcyAmount.Text = (fcyamount + addlss).ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtFcyAmount.Text = fcyamount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCalculation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalFcyAmountWithOutAddLess = default(decimal);
				decimal TotalFcyAmount = default(decimal);
				decimal TotalLcyAmount = default(decimal);
				decimal TotalWeight = default(decimal);
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					decimal amount = Conversion.ToDecimal(item.Cells["FcyAmount"].Value) * Conversion.ToDecimal((object)txtExchangeRate.Text);
					item.Cells["LcyAmount"].Value = amount;
					TotalFcyAmountWithOutAddLess += Conversion.ToDecimal(item.Cells["FcyAmountWithOutAddLess"].Value);
					TotalFcyAmount += Conversion.ToDecimal(item.Cells["FcyAmount"].Value);
					TotalWeight += Conversion.ToDecimal(item.Cells["NetWeight"].Value);
					TotalLcyAmount += amount;
					item.EndEdit();
				}
				txtFcyamountWithoutAddLess.Text = TotalFcyAmountWithOutAddLess.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtRsAmount.Text = TotalLcyAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalWeight.Text = TotalWeight.ToString("#,#");
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["LcyAmount"].Value = 0;
					obj.EndEdit();
				}
				txtFcyamountWithoutAddLess.Text = "";
				txtFcyAmount.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFcyAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtAddlessAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetFcyAmountCalculation();
			AddLessProportionInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddLessProportionInGrid()
	{
		try
		{
			decimal AddLessAmountHeader = Conversion.ToDecimal((object)txtAddlessAmount.Text);
			decimal NetWeight = ((grd.RowCount > 0) ? Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString()) : 0m);
			decimal Fcyamount = default(decimal);
			if (AddLessAmountHeader > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal rowWt = Conversion.ToDecimal((object)obj.Cells["NetWeight"].Value.ToString());
					decimal CurrAddLessAmount = AddLessAmountHeader / NetWeight * rowWt;
					obj.Cells["AddLessAmount"].Value = CurrAddLessAmount;
					Fcyamount = Conversion.ToDecimal((object)obj.Cells["FcyAmountWithOutAddLess"].Value.ToString()) + CurrAddLessAmount;
					obj.Cells["FcyAmount"].Value = Fcyamount;
					decimal amount = Fcyamount * Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.Cells["LcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					decimal CurrAddLessAmount2 = default(decimal);
					obj2.Cells["AddLessAmount"].Value = CurrAddLessAmount2;
					Fcyamount = Conversion.ToDecimal((object)obj2.Cells["FcyAmountWithOutAddLess"].Value.ToString()) + CurrAddLessAmount2;
					obj2.Cells["FcyAmount"].Value = Fcyamount;
					decimal amount2 = Fcyamount * Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj2.Cells["LcyAmount"].Value = amount2;
					obj2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ExportReturnInvoiceSlip242(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint102_Click(object sender, EventArgs e)
	{
		try
		{
			ExportReturnVoucherSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportReturnVoucherSlip(int PrintId)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (PrintId == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			DataTable dt = VoucherReports.ExportReturnInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = CommonServices.VoucherHeadIdGet(PrintId, DocumentTypeId)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(dt, "102-ExportReturnInvoiceVoucherSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDebitAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtAddlessAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportReturnInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					datVoucherDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					fromdateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && BtnSave.Visible && BtnSave.Enabled)
				{
					Save_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					Update_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					datVoucherDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdPending).Focus();
					}
					else
					{
						((Control)(object)grdPending).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtExchangeRate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					BtnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					Print_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshowHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistoryDetail).Focused)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNewHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				BtnRefreshHistory_Click(null, null);
			}
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
			dt.Rows.Add("Ctrl+S", "For Save and for Show on history");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 242 Slip");
			dt.Rows.Add("Alt+1", "For Print 242 Slip");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Exchange Rate");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.KeyCode == Keys.F1 && EnableExportReturnReverseFlow)
			{
				int Id = 0;
				string Name = "";
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse"))
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtWare, "Id", "WareHouseName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
					r.Cells["Warehouse"].Value = Name;
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.ExportReturnInvoiceSlip242(CurrId);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.ExportReturnInvoiceSlip242(CurrId);
			}
			((Control)(object)DataGridHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPending_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPending.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPending.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdPending.CurrentColumn.Key == "Load")
			{
				LoadDataFronGridPending();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnInvoiceLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (EnableExportReturnReverseFlow)
			{
				LoadPO = new frmLoadCommercialInvoiceForReturn(UserAccount);
				LoadPO.ShowDialog();
				LoadPO.BringToFront();
				LoadInGridDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			DataTable dtInvoice = LoadPO.dtLoader;
			if (dtInvoice.Rows.Count <= 0)
			{
				return;
			}
			DataRow dr = dtInvoice.Rows[0];
			int InvoiceId = Conversion.ToInt(dr["ExImInvoiceId"]);
			if (Conversion.ToInt((object)txtExImInvoiceId.Text) > 0 && Conversion.ToInt((object)txtExImInvoiceId.Text) != InvoiceId)
			{
				throw new Exception("Data Against Another Invoice Already Loaded");
			}
			txtExImInvoiceId.Text = Conversion.ToString((object)InvoiceId);
			txtInvoiceNo.Text = Conversion.ToString(dr["InvoiceNo"]);
			datInvoiceDate.Value = Conversion.ToDateTime(dr["InvoiceDate"]);
			txtAddlessAmount.Text = Conversion.ToDouble(dr["AddLessAmount"]).ToString("#,#0");
			CmbCurrencyCode.Value = Conversion.ToInt(dr["FCurrencyId"]);
			txtExchangeRate.Text = Conversion.ToDouble(dr["ExchRate"]).ToString("#,#0.##");
			CmbRefAccountId.Value = Conversion.ToInt(dr["SupplierCustomerId"]);
			bool flag = false;
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["ExImInvoiceDetailId"].Value) == Conversion.ToInt(dtInvoice.Rows[i]["InvoiceDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtDetail.Rows.Add(0, dtInvoice.Rows[i]["ExImInvoiceId"], dtInvoice.Rows[i]["InvoiceDetailId"], 0, 0, 0, 0, "", dtInvoice.Rows[i]["ItemId"], dtInvoice.Rows[i]["ItemName"], dtInvoice.Rows[i]["ItemCode"], dtInvoice.Rows[i]["CropYearId"], dtInvoice.Rows[i]["CropYear"], dtInvoice.Rows[i]["JobLotId"], dtInvoice.Rows[i]["JobLotDescription"], dtInvoice.Rows[i]["PackingMaterialTypeId"], dtInvoice.Rows[i]["PackTypeDesc"], dtInvoice.Rows[i]["PackUomId"], dtInvoice.Rows[i]["PackUom"], dtInvoice.Rows[i]["PackEquivalent"], dtInvoice.Rows[i]["BalQty"], dtInvoice.Rows[i]["BalWeight"], dtInvoice.Rows[i]["BalWeight"], Conversion.ToDouble(dtInvoice.Rows[i]["BalWeight"]) / 1000.0, dtInvoice.Rows[i]["RatePrice"], dtInvoice.Rows[i]["RateUomId"], dtInvoice.Rows[i]["RateUomCode"], dtInvoice.Rows[i]["RateUomEquivalent"], dtInvoice.Rows[i]["BalAmount"], 0, dtInvoice.Rows[i]["BalAmount"], Conversion.ToDouble(dtInvoice.Rows[i]["BalAmount"]) * Conversion.ToDouble((object)txtExchangeRate.Text), "");
				}
				flag = false;
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings(grd);
			BindPaymentDetailByInvoiceId(InvoiceId);
			NetFcyAmountCalculation();
			AddLessProportionInGrid();
			AmountCalculation();
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Expected O, but got Unknown
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Expected O, but got Unknown
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Expected O, but got Unknown
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_100a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Expected O, but got Unknown
		//IL_1021: Unknown result type (might be due to invalid IL or missing references)
		//IL_102b: Expected O, but got Unknown
		//IL_1313: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_3d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d65: Expected O, but got Unknown
		//IL_3d89: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d93: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdPending_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdpaymentdetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance100 = new Appearance();
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
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.grdPending = new GridEX();
		this.panelLabelPending = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdpaymentdetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtVoucherCode = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.datInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.datVoucherDate = new System.Windows.Forms.DateTimePicker();
		this.CmbCreditAccount = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbCurrencyCode = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtInvoiceNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtRsAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtTotalWeight = new System.Windows.Forms.TextBox();
		this.txtAddlessAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtFcyamountWithoutAddLess = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbRefAccountId = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtExImInvoiceId = new System.Windows.Forms.TextBox();
		this.txtCustomerGlAcId = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ExpenseVoucherLable = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint102 = new System.Windows.Forms.ToolStripButton();
		this.btnInvoiceLoader = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.History = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabDetailH = new System.Windows.Forms.TabPage();
		this.DataGridHistoryDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPaymentDetailH = new System.Windows.Forms.TabPage();
		this.grdPaymentHistory = new GridEX();
		this.panel31 = new System.Windows.Forms.Panel();
		this.label74 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel10 = new System.Windows.Forms.Panel();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.CmbDebitAccountHistory = new UltraCombo();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.fromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPending).BeginInit();
		this.panelLabelPending.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).BeginInit();
		this.panel20.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel13.SuspendLayout();
		this.tabPaymentDetailH.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).BeginInit();
		this.panel31.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(718, 1);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1830, 27);
		this.miniToolStrip.TabIndex = 46;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1038, 586);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1038, 586);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grdPending);
		this.Form.Controls.Add(this.panelLabelPending);
		this.Form.Controls.Add(this.tabControl2);
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add(this.panel3);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1030, 557);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.grdPending.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPending).BackColor = System.Drawing.Color.White;
		grdPending_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPending.DesignTimeLayout = grdPending_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPending).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPending.FilterMode = (FilterMode)2;
		this.grdPending.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPending).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPending.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPending).Location = new System.Drawing.Point(2, 459);
		((System.Windows.Forms.Control)(object)this.grdPending).Name = "grdPending";
		((ScrollableBase)this.grdPending).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPending).Size = new System.Drawing.Size(1026, 96);
		((System.Windows.Forms.Control)(object)this.grdPending).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdPending).TabStop = false;
		this.grdPending.TotalRow = (InheritableBoolean)1;
		this.grdPending.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPending).VisualStyle = (VisualStyle)4;
		this.grdPending.ColumnButtonClick += new ColumnActionEventHandler(grdPending_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPending).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPending_KeyDown);
		this.panelLabelPending.BackColor = System.Drawing.Color.Teal;
		this.panelLabelPending.Controls.Add(this.label6);
		this.panelLabelPending.Controls.Add(this.ctrlGrdBar4);
		this.panelLabelPending.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelLabelPending.Location = new System.Drawing.Point(2, 435);
		this.panelLabelPending.Name = "panelLabelPending";
		this.panelLabelPending.Size = new System.Drawing.Size(1026, 24);
		this.panelLabelPending.TabIndex = 6;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(250, 18);
		this.label6.TabIndex = 0;
		this.label6.Text = "Pending GRN For Return Invoice";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(978, 1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdPending;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(46, 22);
		this.ctrlGrdBar4.TabIndex = 20;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Location = new System.Drawing.Point(2, 188);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1026, 247);
		this.tabControl2.TabIndex = 7;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1018, 218);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1012, 188);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1012, 24);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(964, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(46, 22);
		this.ctrlGrdBar1.TabIndex = 19;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdpaymentdetail);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1018, 218);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Payment Detail";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdpaymentdetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdpaymentdetail).BackColor = System.Drawing.Color.White;
		grdpaymentdetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdpaymentdetail.DesignTimeLayout = grdpaymentdetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdpaymentdetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Location = new System.Drawing.Point(3, 26);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Name = "grdpaymentdetail";
		((ScrollableBase)this.grdpaymentdetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdpaymentdetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdpaymentdetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).Size = new System.Drawing.Size(1012, 189);
		((System.Windows.Forms.Control)(object)this.grdpaymentdetail).TabIndex = 2;
		this.grdpaymentdetail.TotalRow = (InheritableBoolean)1;
		this.grdpaymentdetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdpaymentdetail).VisualStyle = (VisualStyle)4;
		this.grdpaymentdetail.CellUpdated += new ColumnActionEventHandler(grdpaymentdetail_CellUpdated);
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.ctrlGrdBar2);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(3, 3);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1012, 23);
		this.panel20.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(972, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdpaymentdetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 22);
		this.ctrlGrdBar2.TabIndex = 27;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.txtExImInvoiceId);
		this.panel5.Controls.Add(this.txtCustomerGlAcId);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 53);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1026, 135);
		this.panel5.TabIndex = 0;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.txtVoucherCode);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.datInvoiceDate);
		this.groupBox2.Controls.Add(this.datVoucherDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCreditAccount);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencyCode);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtInvoiceNo);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtRemarks);
		this.groupBox2.Controls.Add(this.txtRsAmount);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtTotalWeight);
		this.groupBox2.Controls.Add(this.txtAddlessAmount);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtFcyamountWithoutAddLess);
		this.groupBox2.Controls.Add(this.txtFcyAmount);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefAccountId);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1020, 131);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtVoucherCode.BackColor = System.Drawing.Color.White;
		this.txtVoucherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVoucherCode.Enabled = false;
		this.txtVoucherCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVoucherCode.Location = new System.Drawing.Point(79, 17);
		this.txtVoucherCode.Name = "txtVoucherCode";
		this.txtVoucherCode.ReadOnly = true;
		this.txtVoucherCode.Size = new System.Drawing.Size(61, 23);
		this.txtVoucherCode.TabIndex = 0;
		this.txtVoucherCode.Leave += new System.EventHandler(txtvoucherno_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(5, 21);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(76, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Doc No,Date";
		this.datInvoiceDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.datInvoiceDate.Enabled = false;
		this.datInvoiceDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInvoiceDate.Location = new System.Drawing.Point(79, 74);
		this.datInvoiceDate.Name = "datInvoiceDate";
		this.datInvoiceDate.Size = new System.Drawing.Size(173, 23);
		this.datInvoiceDate.TabIndex = 3;
		this.datInvoiceDate.TabStop = false;
		this.datVoucherDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVoucherDate.CustomFormat = "dd-MMM-yy";
		this.datVoucherDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datVoucherDate.Location = new System.Drawing.Point(142, 17);
		this.datVoucherDate.Name = "datVoucherDate";
		this.datVoucherDate.Size = new System.Drawing.Size(110, 23);
		this.datVoucherDate.TabIndex = 1;
		this.CmbCreditAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbCreditAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCreditAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCreditAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCreditAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCreditAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Location = new System.Drawing.Point(777, 72);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Name = "CmbCreditAccount";
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbCreditAccount).TabIndex = 13;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(692, 78);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(84, 15);
		this.label9.TabIndex = 6;
		this.label9.Text = "Debit Account";
		this.CmbCurrencyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCurrencyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Location = new System.Drawing.Point(557, 15);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Name = "CmbCurrencyCode";
		this.CmbCurrencyCode.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyCode).TabStop = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(470, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(84, 15);
		this.label8.TabIndex = 42;
		this.label8.Text = "Currency Code";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 78);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(74, 15);
		this.label11.TabIndex = 41;
		this.label11.Text = "Invoice Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 49);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(65, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "Invoice No";
		this.txtInvoiceNo.BackColor = System.Drawing.Color.White;
		this.txtInvoiceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNo.Location = new System.Drawing.Point(79, 45);
		this.txtInvoiceNo.Name = "txtInvoiceNo";
		this.txtInvoiceNo.ReadOnly = true;
		this.txtInvoiceNo.Size = new System.Drawing.Size(173, 23);
		this.txtInvoiceNo.TabIndex = 2;
		this.txtInvoiceNo.TabStop = false;
		this.txtInvoiceNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(470, 78);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 39;
		this.label4.Text = "Rs. Amount";
		this.txtRemarks.BackColor = System.Drawing.Color.White;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(79, 101);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.ReadOnly = true;
		this.txtRemarks.Size = new System.Drawing.Size(935, 23);
		this.txtRemarks.TabIndex = 4;
		this.txtRsAmount.BackColor = System.Drawing.Color.White;
		this.txtRsAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRsAmount.Enabled = false;
		this.txtRsAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRsAmount.Location = new System.Drawing.Point(557, 74);
		this.txtRsAmount.Name = "txtRsAmount";
		this.txtRsAmount.ReadOnly = true;
		this.txtRsAmount.Size = new System.Drawing.Size(132, 23);
		this.txtRsAmount.TabIndex = 10;
		this.txtRsAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(692, 21);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(75, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "Total Weight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(256, 49);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(77, 15);
		this.label20.TabIndex = 37;
		this.label20.Text = "AddLess Amt";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(470, 49);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 37;
		this.label3.Text = "Exchange Rate";
		this.txtTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeight.Enabled = false;
		this.txtTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeight.Location = new System.Drawing.Point(777, 17);
		this.txtTotalWeight.Name = "txtTotalWeight";
		this.txtTotalWeight.ReadOnly = true;
		this.txtTotalWeight.Size = new System.Drawing.Size(237, 23);
		this.txtTotalWeight.TabIndex = 11;
		this.txtTotalWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTotalWeight.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtAddlessAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddlessAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddlessAmount.Location = new System.Drawing.Point(334, 45);
		this.txtAddlessAmount.Name = "txtAddlessAmount";
		this.txtAddlessAmount.Size = new System.Drawing.Size(132, 23);
		this.txtAddlessAmount.TabIndex = 6;
		this.txtAddlessAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddlessAmount.TextChanged += new System.EventHandler(txtAddlessAmount_TextChanged);
		this.txtAddlessAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddlessAmount_KeyPress);
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(557, 45);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(132, 23);
		this.txtExchangeRate.TabIndex = 9;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(256, 21);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(72, 15);
		this.label21.TabIndex = 35;
		this.label21.Text = "Fcy Amount";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(256, 78);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(73, 15);
		this.label18.TabIndex = 35;
		this.label18.Text = "Net Fcy Amt";
		this.txtFcyamountWithoutAddLess.BackColor = System.Drawing.Color.White;
		this.txtFcyamountWithoutAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyamountWithoutAddLess.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyamountWithoutAddLess.Location = new System.Drawing.Point(334, 17);
		this.txtFcyamountWithoutAddLess.Name = "txtFcyamountWithoutAddLess";
		this.txtFcyamountWithoutAddLess.ReadOnly = true;
		this.txtFcyamountWithoutAddLess.Size = new System.Drawing.Size(132, 23);
		this.txtFcyamountWithoutAddLess.TabIndex = 5;
		this.txtFcyamountWithoutAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyamountWithoutAddLess.TextChanged += new System.EventHandler(txtFcyAmount_TextChanged);
		this.txtFcyAmount.BackColor = System.Drawing.Color.White;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(334, 74);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ReadOnly = true;
		this.txtFcyAmount.Size = new System.Drawing.Size(132, 23);
		this.txtFcyAmount.TabIndex = 7;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtFcyAmount_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(5, 106);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 6;
		this.label1.Text = "Remarks";
		this.CmbRefAccountId.AllowNull = (DefaultableBoolean)1;
		this.CmbRefAccountId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefAccountId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefAccountId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefAccountId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRefAccountId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Location = new System.Drawing.Point(777, 43);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Name = "CmbRefAccountId";
		this.CmbRefAccountId.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefAccountId).TabIndex = 12;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(692, 49);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(72, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "Buyer Name";
		this.txtExImInvoiceId.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExImInvoiceId.Location = new System.Drawing.Point(1027, 12);
		this.txtExImInvoiceId.Name = "txtExImInvoiceId";
		this.txtExImInvoiceId.Size = new System.Drawing.Size(55, 23);
		this.txtExImInvoiceId.TabIndex = 45;
		this.txtExImInvoiceId.TabStop = false;
		this.txtExImInvoiceId.Text = "0";
		this.txtExImInvoiceId.Visible = false;
		this.txtCustomerGlAcId.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCustomerGlAcId.Location = new System.Drawing.Point(1027, 40);
		this.txtCustomerGlAcId.Name = "txtCustomerGlAcId";
		this.txtCustomerGlAcId.Size = new System.Drawing.Size(55, 23);
		this.txtCustomerGlAcId.TabIndex = 43;
		this.txtCustomerGlAcId.TabStop = false;
		this.txtCustomerGlAcId.Text = "0";
		this.txtCustomerGlAcId.Visible = false;
		this.label10.AutoSize = true;
		this.label10.Enabled = false;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(955, 66);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(46, 15);
		this.label10.TabIndex = 2;
		this.label10.Text = "Branch";
		this.label10.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Enabled = false;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(955, 92);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(45, 15);
		this.label5.TabIndex = 4;
		this.label5.Text = "Project";
		this.label5.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ExpenseVoucherLable);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 2);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1026, 51);
		this.panel3.TabIndex = 0;
		this.ExpenseVoucherLable.AutoSize = true;
		this.ExpenseVoucherLable.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ExpenseVoucherLable.ForeColor = System.Drawing.Color.White;
		this.ExpenseVoucherLable.Location = new System.Drawing.Point(0, 30);
		this.ExpenseVoucherLable.Name = "ExpenseVoucherLable";
		this.ExpenseVoucherLable.Size = new System.Drawing.Size(225, 19);
		this.ExpenseVoucherLable.TabIndex = 0;
		this.ExpenseVoucherLable.Text = "EXPORT RETURN INVOICE";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.BtnNew, this.btnRefresh, this.BtnSave, this.btnUpdate, this.btnDelete, this.BtnAttachment, this.BtnPrint, this.btnPrint102, this.btnInvoiceLoader, this.btnShortCutKey });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1026, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(64, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(Save_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(Update_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.BtnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttachment.Name = "BtnAttachment";
		this.BtnAttachment.Size = new System.Drawing.Size(109, 24);
		this.BtnAttachment.Text = "&Attachment";
		this.BtnAttachment.Click += new System.EventHandler(BtnAttachment_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "216-Print";
		this.BtnPrint.Click += new System.EventHandler(Print_Click);
		this.btnPrint102.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint102.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint102.Name = "btnPrint102";
		this.btnPrint102.Size = new System.Drawing.Size(92, 24);
		this.btnPrint102.Text = "102-Print";
		this.btnPrint102.Click += new System.EventHandler(btnPrint102_Click);
		this.btnInvoiceLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnInvoiceLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnInvoiceLoader.Name = "btnInvoiceLoader";
		this.btnInvoiceLoader.Size = new System.Drawing.Size(122, 24);
		this.btnInvoiceLoader.Text = "Load Invoices";
		this.btnInvoiceLoader.Click += new System.EventHandler(btnInvoiceLoader_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.ImageAlign = System.Drawing.ContentAlignment.TopRight;
		this.ChkBox.Location = new System.Drawing.Point(902, 29);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.TabStop = false;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.History.Controls.Add(this.tabControl3);
		this.History.Controls.Add(this.panel16);
		this.History.Controls.Add(this.panel10);
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1030, 557);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 141);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1026, 187);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabDetailH);
		this.tabControl3.Controls.Add(this.tabPaymentDetailH);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(2, 328);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1026, 227);
		this.tabControl3.TabIndex = 350;
		this.tabDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabDetailH.Controls.Add(this.panel13);
		this.tabDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabDetailH.Name = "tabDetailH";
		this.tabDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetailH.Size = new System.Drawing.Size(1018, 198);
		this.tabDetailH.TabIndex = 0;
		this.tabDetailH.Text = "Detail";
		this.tabDetailH.UseVisualStyleBackColor = true;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1012, 167);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 2;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1012, 25);
		this.panel13.TabIndex = 346;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(5, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(228, 17);
		this.label59.TabIndex = 562;
		this.label59.Text = "Detail of the selected above row";
		this.tabPaymentDetailH.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentHistory);
		this.tabPaymentDetailH.Controls.Add(this.panel31);
		this.tabPaymentDetailH.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetailH.Name = "tabPaymentDetailH";
		this.tabPaymentDetailH.Padding = new System.Windows.Forms.Padding(3);
		this.tabPaymentDetailH.Size = new System.Drawing.Size(1014, 198);
		this.tabPaymentDetailH.TabIndex = 1;
		this.tabPaymentDetailH.Text = "Payment Detail";
		this.tabPaymentDetailH.UseVisualStyleBackColor = true;
		this.grdPaymentHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentHistory).BackColor = System.Drawing.Color.White;
		this.grdPaymentHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentHistory.DesignTimeLayout = grdPaymentHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentHistory.FilterMode = (FilterMode)2;
		this.grdPaymentHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentHistory.GroupByBoxVisible = false;
		this.grdPaymentHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Name = "grdPaymentHistory";
		((ScrollableBase)this.grdPaymentHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).Size = new System.Drawing.Size(1008, 167);
		((System.Windows.Forms.Control)(object)this.grdPaymentHistory).TabIndex = 347;
		this.grdPaymentHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentHistory).VisualStyle = (VisualStyle)4;
		this.panel31.BackColor = System.Drawing.Color.Teal;
		this.panel31.Controls.Add(this.label74);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(3, 3);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1008, 25);
		this.panel31.TabIndex = 348;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label74.ForeColor = System.Drawing.Color.White;
		this.label74.Location = new System.Drawing.Point(5, 5);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(301, 17);
		this.label74.TabIndex = 562;
		this.label74.Text = "Payement Detail of the selected above row";
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label14);
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(2, 118);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1026, 23);
		this.panel16.TabIndex = 49;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(142, 19);
		this.label14.TabIndex = 99;
		this.label14.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(979, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel10.Controls.Add(this.txtToDocNoHistory);
		this.panel10.Controls.Add(this.label77);
		this.panel10.Controls.Add(this.txtFromNoHistory);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory);
		this.panel10.Controls.Add(this.btnshowHistory);
		this.panel10.Controls.Add(this.label25);
		this.panel10.Controls.Add(this.ToDateHistory);
		this.panel10.Controls.Add(this.label26);
		this.panel10.Controls.Add(this.fromdateHistory);
		this.panel10.Controls.Add(this.label36);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.panel10.Controls.Add(this.label38);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.panel10.Controls.Add(this.label43);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 55);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1026, 63);
		this.panel10.TabIndex = 0;
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(420, 5);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(231, 9);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(88, 15);
		this.label77.TabIndex = 177;
		this.label77.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(320, 5);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.CmbDebitAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDebitAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDebitAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDebitAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance147;
		((SpecialBoxBase)((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance152).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDebitAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDebitAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Location = new System.Drawing.Point(619, 31);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Name = "CmbDebitAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbDebitAccountHistory).TabIndex = 6;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(863, 31);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 7;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(522, 37);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(84, 15);
		this.label25.TabIndex = 163;
		this.label25.Text = "Debit Account";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(74, 33);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(154, 23);
		this.ToDateHistory.TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 37);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(48, 15);
		this.label26.TabIndex = 162;
		this.label26.Text = "To Date";
		this.fromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdateHistory.CustomFormat = "dd-MMM-yy";
		this.fromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdateHistory.Location = new System.Drawing.Point(74, 5);
		this.fromdateHistory.Name = "fromdateHistory";
		this.fromdateHistory.ShowCheckBox = true;
		this.fromdateHistory.Size = new System.Drawing.Size(154, 23);
		this.fromdateHistory.TabIndex = 0;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(5, 9);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 161;
		this.label36.Text = "From Date";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(320, 31);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(197, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 4;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(231, 37);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(65, 15);
		this.label38.TabIndex = 160;
		this.label38.Text = "Invoice No";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance123;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(619, 3);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(522, 9);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(93, 15);
		this.label43.TabIndex = 159;
		this.label43.Text = "Customer Name";
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.toolStrip3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1026, 53);
		this.panel8.TabIndex = 47;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 30);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(251, 19);
		this.label13.TabIndex = 98;
		this.label13.Text = "Export Return Invoice History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.BtnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1026, 27);
		this.toolStrip3.TabIndex = 53;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1038, 586);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ExportReturnInvoice";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExportReturnInvoice_FormClosed);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExportReturnInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPending).EndInit();
		this.panelLabelPending.ResumeLayout(false);
		this.panelLabelPending.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdpaymentdetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCreditAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefAccountId).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabPaymentDetailH.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentHistory).EndInit();
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDebitAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
