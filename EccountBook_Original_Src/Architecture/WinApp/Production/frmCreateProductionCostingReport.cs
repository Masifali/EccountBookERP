using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmCreateProductionCostingReport : Form
{
	public class CreateProductionCostingReportIdEventArgs : EventArgs
	{
		public int ProductionJobOrderId { get; }

		public CreateProductionCostingReportIdEventArgs(int id)
		{
			ProductionJobOrderId = id;
		}
	}

	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtInputDetail = new DataTable();

	private DataTable dtBpDetail = new DataTable();

	private DataTable dtdetail = new DataTable();

	private bool FormInitialized;

	private bool DataReset;

	private string ScreenName = "frmCreateProductionCostingReport";

	private int DocumentTypeId = 921;

	private int RecId;

	private int updateDetailIndex = -1;

	private string CustomAttachmentsValues = "";

	private string AttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private frmProductionCostingReport CostingReportForm;

	private List<ProductionManualCostingByProductDetail> lstBpRemoveRecord = new List<ProductionManualCostingByProductDetail>();

	private List<ProductionManualCostingInputDetail> lstInputRemoveRecord = new List<ProductionManualCostingInputDetail>();

	private List<ProductionManualCostingInvoiceDetail> lstDetailRemoveRecord = new List<ProductionManualCostingInvoiceDetail>();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator2;

	internal UltraCombo CmbJobOrderNo;

	private Label label68;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripDropDownButton BtnPrintDropDown;

	private Panel panel20;

	private Button btnShow;

	internal GridEX grdByProductRatio;

	private Panel panel23;

	internal GridEX grdInputWorking;

	private Panel panel21;

	private Label label36;

	private Panel panel3;

	private Panel panel2;

	private TabControl tabControl1;

	private TabPage tabInputWorking;

	private TabPage tabByProductRatio;

	private Panel panel4;

	internal GridEX grd;

	private Panel panel7;

	private Panel panel6;

	internal UltraCombo CmbCustomerDetail;

	internal UltraCombo CmbInvoiceNoDetail;

	private Label label64;

	private TextBox txtPackingChargesDetail;

	private Button btnAddDetail;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Label label1;

	private Label label2;

	internal UltraCombo CmbFcyCodeDetail;

	private Label label7;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label12;

	private Label label11;

	private TextBox txtFinancialChargesDetail;

	private TextBox txtFreightDetail;

	private TextBox txtTaxDetail;

	private TextBox txtFobChargesDetail;

	private Label label10;

	private TextBox txtCommissionAmountDetail;

	private TextBox txtFcyAmountDetail;

	private TextBox txtTotalCostDetail;

	private Label label8;

	private TextBox txtExchangeRateDetail;

	private Label label9;

	private TextBox txtamountAfterCommissionDetail;

	private Label label13;

	private TextBox txtCostAfterProcessingDetail;

	private Label label3;

	private TextBox txtContributionPerMTonDetail;

	private Label label15;

	private TextBox txtItemSaleAmountDetail;

	private Label label14;

	private TextBox txtRatePerMTonHeader;

	private TextBox txtTotalCostAfterProcessingPerMtonHeader;

	private TextBox txtProcessingChargesHeader;

	private TextBox txtTotalCostHeader;

	private TextBox txtByProductCostHeader;

	private Label label23;

	private Label label22;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtInputToBeProcessedHeader;

	private Label label18;

	private TextBox txtFinishOutPutPrcntObtainedHeader;

	private Label label17;

	private TextBox txtInputAvgRateHeader;

	private Label label16;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnDelete;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnShortCutKey;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label26;

	private Label label25;

	private Label label29;

	private Label label28;

	private Label label27;

	private Label label24;

	private Label label30;

	private TextBox txtRemarksDetail;

	private Label label39;

	private Label label38;

	private Label label37;

	private Label label35;

	private Label label34;

	private Label label33;

	private Label label32;

	private Label label40;

	private Label label31;

	private Label label42;

	private Label label41;

	private Label label43;

	private Button BtnByProductSettlementPopUp;

	private TabControl tabControlMain;

	private TabPage tabForm;

	private TabPage tabHistory;

	private Panel PanelOtherForms;

	public frmCreateProductionCostingReport(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("InvoiceId", typeof(int));
			dtdetail.Columns.Add("InvoiceNo");
			dtdetail.Columns.Add("SupplierCustomerId", typeof(int));
			dtdetail.Columns.Add("Customer");
			dtdetail.Columns.Add("CostAfterProcessing", typeof(double));
			dtdetail.Columns.Add("PackingCharges", typeof(double));
			dtdetail.Columns.Add("FOBCharges", typeof(double));
			dtdetail.Columns.Add("Tax", typeof(double));
			dtdetail.Columns.Add("Freight", typeof(double));
			dtdetail.Columns.Add("FinancialCharges", typeof(double));
			dtdetail.Columns.Add("TotalCost", typeof(double));
			dtdetail.Columns.Add("ExchangeRate", typeof(double));
			dtdetail.Columns.Add("FcyCodeId", typeof(int));
			dtdetail.Columns.Add("FcyCode");
			dtdetail.Columns.Add("FcyAmount", typeof(double));
			dtdetail.Columns.Add("CommissionAmount", typeof(double));
			dtdetail.Columns.Add("AmountAfterCommission", typeof(double));
			dtdetail.Columns.Add("ItemSaleAmount", typeof(double));
			dtdetail.Columns.Add("ContributionPerMTon", typeof(double));
			dtdetail.Columns.Add("Remarks");
			dtInputDetail.Columns.Add("Id", typeof(int));
			dtInputDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtInputDetail.Columns.Add("RefDocumentType", typeof(string));
			dtInputDetail.Columns.Add("RefDocId", typeof(int));
			dtInputDetail.Columns.Add("RefDocNo", typeof(string));
			dtInputDetail.Columns.Add("ItemId", typeof(int));
			dtInputDetail.Columns.Add("ItemCode", typeof(string));
			dtInputDetail.Columns.Add("ItemName", typeof(string));
			dtInputDetail.Columns.Add("Qty", typeof(double));
			dtInputDetail.Columns.Add("Weight", typeof(double));
			dtInputDetail.Columns.Add("InputAmount", typeof(double));
			dtInputDetail.Columns.Add("InputRate", typeof(double));
			dtInputDetail.Columns.Add("Rate", typeof(double));
			dtInputDetail.Columns.Add("FinalAmount", typeof(double));
			dtBpDetail.Columns.Add("Id", typeof(int));
			dtBpDetail.Columns.Add("ItemId", typeof(int));
			dtBpDetail.Columns.Add("ItemCode", typeof(string));
			dtBpDetail.Columns.Add("ItemName", typeof(string));
			dtBpDetail.Columns.Add("PercentOfTotalInput", typeof(double));
			dtBpDetail.Columns.Add("Ratio", typeof(double));
			dtBpDetail.Columns.Add("Rate", typeof(double));
			dtBpDetail.Columns.Add("ByProductAmount", typeof(double));
			dtBpDetail.Columns.Add("Amount", typeof(double));
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
			ScreenName = Conversion.ToString(base.Tag);
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable jobData = InvFoodProduction.FoodProduction_JobOrderWithFinishOutPutPrcntObtained(UserAccount.OrganizationId, UserAccount.CompanyId);
			DataTable invoiceData = ExImInvoice.ExImInvoice_GetInvoicesData(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id);
			return new
			{
				JobData = jobData,
				InvoiceData = invoiceData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				GetConfigurationsFromGlobal();
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				BtnDelete.Enabled = formrights.DoHaveCanDelete;
				BtnPrintDropDown.Enabled = formrights.DoHavePrintRights;
				ReportsLoad();
				_ = RecId;
				JobOrderNoBind(result.JobData);
				ExportInvoiceNoBind(result.InvoiceData);
				BindGrids();
				FormInitialized = true;
				CmbJobOrderNo.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
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

	private void JobOrderNoBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbJobOrderNo, dt, "JobOrderId", "JobOrderNo", "JobOrderNo", AllColumns: true, Conversion.ToInt(CmbJobOrderNo.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportInvoiceNoBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNoDetail, dt, "Id", "InvoiceNo", "Invoice No", AllColumns: true, Conversion.ToInt(CmbInvoiceNoDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			CmbInvoiceNoDetail_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNoDetail_Leave(object sender, EventArgs e)
	{
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Expected O, but got Unknown
		try
		{
			UltraGridRow selectedRow = ((UltraGridBase)CmbInvoiceNoDetail).ActiveRow;
			int invoiceId = Conversion.ToInt(CmbInvoiceNoDetail.Value);
			if (selectedRow == null || invoiceId <= 0)
			{
				((UltraGridBase)CmbCustomerDetail).DataSource = null;
				((UltraGridBase)CmbFcyCodeDetail).DataSource = null;
				txtExchangeRateDetail.Text = 0.ToString();
				return;
			}
			int customerId = Conversion.ToInt(selectedRow.Cells["SupplierCustomerId"].Value);
			string customerName = Conversion.ToString(selectedRow.Cells["CustomerName"].Value);
			int currencyId = Conversion.ToInt(selectedRow.Cells["FcurrencyId"].Value);
			string currencyCode = Conversion.ToString(selectedRow.Cells["FcyCode"].Value);
			double exchangeRate = Conversion.ToDouble(selectedRow.Cells["ExchangeRate"].Value);
			DataTable dtCus = new DataTable();
			dtCus.Columns.Add("Id", typeof(int));
			dtCus.Columns.Add("Name", typeof(string));
			dtCus.Rows.Add(customerId, customerName);
			DataTable dtFcy = dtCus.Clone();
			dtFcy.Rows.Add(currencyId, currencyCode);
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerDetail, dtCus, "Id", "Name", "Customer Name", AllColumns: true, Conversion.ToInt(CmbCustomerDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbFcyCodeDetail, dtFcy, "Id", "Name", "Fcy Code", AllColumns: true, Conversion.ToInt(CmbFcyCodeDetail.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			CmbCustomerDetail.Value = customerId;
			CmbFcyCodeDetail.Value = currencyId;
			txtExchangeRateDetail.Text = exchangeRate.ToString();
			if (!dtdetail.AsEnumerable().Any((DataRow rr) => rr.Field<int>("InvoiceId") == invoiceId && rr.Field<int>("Id") > 0) || DataReset)
			{
				DataTable dt = ProductionManualCosting.ProductionManualCosting_GetDataByJobAndInvoiceId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = DocumentTypeId,
					InvoiceId = invoiceId,
					JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value)
				});
				if (dt != null && dt.Rows.Count > 0)
				{
					txtPackingChargesDetail.Text = dt.Rows[0]["PackingCharges"]?.ToString();
					txtFobChargesDetail.Text = dt.Rows[0]["FOBCharges"]?.ToString();
					txtTaxDetail.Text = dt.Rows[0]["Tax"]?.ToString();
					txtFreightDetail.Text = dt.Rows[0]["Freight"]?.ToString();
					txtFinancialChargesDetail.Text = dt.Rows[0]["FinancialCharges"]?.ToString();
					txtCommissionAmountDetail.Text = dt.Rows[0]["CommissionAmount"]?.ToString();
					txtItemSaleAmountDetail.Text = dt.Rows[0]["ItemSaleAmount"]?.ToString();
				}
				else
				{
					DataReset = true;
					txtPackingChargesDetail.Text = 0.ToString();
					txtFobChargesDetail.Text = 0.ToString();
					txtTaxDetail.Text = 0.ToString();
					txtFreightDetail.Text = 0.ToString();
					txtFinancialChargesDetail.Text = 0.ToString();
					txtCommissionAmountDetail.Text = 0.ToString();
					txtItemSaleAmountDetail.Text = 0.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool DetailFormValidation()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)CmbInvoiceNoDetail, "Invoice No", FormHelper.ValidType.INT),
			((Control)(object)CmbCustomerDetail, "Customer", FormHelper.ValidType.INT),
			(txtCostAfterProcessingDetail, "Cost After Processing", FormHelper.ValidType.Double),
			(txtTotalCostDetail, "Total Cost", FormHelper.ValidType.Double),
			(txtExchangeRateDetail, "Exchange Rate", FormHelper.ValidType.Double),
			((Control)(object)CmbFcyCodeDetail, "FCY Code", FormHelper.ValidType.INT),
			(txtFcyAmountDetail, "FCY Amount", FormHelper.ValidType.Double),
			(txtItemSaleAmountDetail, "Item Sale Amount", FormHelper.ValidType.Double),
			(txtContributionPerMTonDetail, "Contribution Per M.Ton", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			int JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			if (((UltraGridBase)CmbJobOrderNo).ActiveRow == null || JobOrderId == 0)
			{
				MessageBox.Show("Please Select JobOrder First");
				CmbJobOrderNo.Focus();
				return;
			}
			double FinishOutPutPrcntObtainedHeader = Conversion.ToDouble(((UltraDropDownBase)CmbJobOrderNo).SelectedRow.Cells["FinishOutPutPrcntObtained"].Value);
			if (!ReadById(JobOrderId))
			{
				CmbJobOrderNo.Value = JobOrderId;
				txtFinishOutPutPrcntObtainedHeader.Text = FinishOutPutPrcntObtainedHeader.ToString("#,##0.###");
				InputBindByJobOrder(JobOrderId);
				ByProductBindByJobOrder(JobOrderId);
			}
			else
			{
				dtBpDetail.Rows.Clear();
				ByProductBindByJobOrder(JobOrderId);
			}
			CalculateHeaderValues();
			((Control)(object)CmbJobOrderNo).Enabled = false;
			btnShow.Enabled = false;
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
			if (dtInputDetail != null && dtInputDetail.Columns.Count > 0)
			{
				grdInputWorking.DataSource = dtInputDetail;
				grdInputWorking.RetrieveStructure();
				grdInputSettings();
			}
			if (dtBpDetail != null && dtBpDetail.Columns.Count > 0)
			{
				grdByProductRatio.DataSource = dtBpDetail;
				grdByProductRatio.RetrieveStructure();
				grdByProductSettings();
			}
			if (dtdetail != null && dtdetail.Columns.Count > 0)
			{
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InputBindByJobOrder(int JobOrderId)
	{
		try
		{
			DataTable dt = InvFoodProduction.FoodProduction_InputSummaryByJobOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id, JobOrderId);
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					double qty = Conversion.ToDouble(dr["Qty"]);
					double Weight = Conversion.ToDouble(dr["Weight"]);
					double amount = Conversion.ToDouble(dr["Amount"]);
					double rate = ((Weight != 0.0) ? (amount / Weight) : 0.0);
					double finalAmount = Conversion.ToDouble(dr["Amount"]);
					dtInputDetail.Rows.Add(0, Conversion.ToInt(dr["RefDocumentTypeId"]), Conversion.ToString(dr["RefDocumentType"]), Conversion.ToInt(dr["RefDocNoId"]), Conversion.ToString(dr["RefDocNo"]), Conversion.ToInt(dr["ItemId"]), Conversion.ToString(dr["ItemCode"]), Conversion.ToString(dr["ItemName"]), qty, Weight, amount, rate, rate, finalAmount);
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "RefDocumentTypeId", "RefDocId", "ItemId", "Qty", "InputAmount" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Rate",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdInputWorking, 2, 3, hiddenColumns, editableColumns);
			grdInputWorking.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grdInputWorking.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdInputWorking.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInputWorking.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode - 10;
			grdInputWorking.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdInputWorking.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInputWorking.RootTable.Columns["InputAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInputWorking.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInputWorking.RootTable.Columns["InputRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInputWorking.RootTable.Columns["FinalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInputWorking.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputWorking_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInputWorking.UpdateData();
			GridEXRow row = grdInputWorking.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "Rate")
			{
				double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
				if (rate < 0.0)
				{
					MessageBox.Show("Rate cannot be negative.");
					row.Cells["Rate"].Value = 0;
					return;
				}
				double InputRate = Conversion.ToDouble(row.Cells["InputRate"].Value);
				double Weight = Conversion.ToDouble(row.Cells["Weight"].Value);
				double finalAmount = ((rate > 0.0) ? (rate * Weight) : (InputRate * Weight));
				row.Cells["FinalAmount"].Value = finalAmount;
			}
			grdInputWorking.UpdateData();
			CalculateHeaderValues();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ByProductBindByJobOrder(int JobOrderId)
	{
		try
		{
			DataTable dt = InvFoodProduction.FoodProduction_ByProductSummaryByJobOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id, JobOrderId);
			if (dt != null && dt.Rows.Count > 0)
			{
				double totalInputWeight = GridEX_Helper.GetColumnSum(grdInputWorking, "Weight");
				double FinishPercent = Conversion.ToDouble((object)txtFinishOutPutPrcntObtainedHeader.Text);
				double totalBpRatio = ((FinishPercent > 0.0) ? (1000.0 / FinishPercent) : 0.0);
				foreach (DataRow dr in dt.Rows)
				{
					double rate = Conversion.ToDouble(dr["RatePerKg"]);
					double TotalAmount = Conversion.ToDouble(dr["TotalAmount"]);
					double TotalWeight = Conversion.ToDouble(dr["Weight"]);
					double percentOfTotalInput = ((totalInputWeight > 0.0) ? (TotalWeight / totalInputWeight * 100.0) : 0.0);
					double Ratio = totalBpRatio * percentOfTotalInput;
					double FinalAmount = rate * Ratio;
					dtBpDetail.Rows.Add(0, Conversion.ToInt(dr["ItemId"]), Conversion.ToString(dr["ItemCode"]), Conversion.ToString(dr["ItemName"]), percentOfTotalInput, Ratio, rate, TotalAmount, FinalAmount);
				}
			}
			grdByProductRatio.DataSource = dtBpDetail;
			grdByProductRatio.RetrieveStructure();
			grdByProductSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProductSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "ByProductAmount" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdByProductRatio, 2, 3, hiddenColumns);
			grdByProductRatio.FrozenColumns = 1;
			grdByProductRatio.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdByProductRatio.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdByProductRatio.RootTable.Columns["PercentOfTotalInput"].Width = Constants.InventoryConstants.PercentOfTotal * 2;
			grdByProductRatio.RootTable.Columns["Ratio"].Width = Constants.InventoryConstants.ExRateConstant;
			grdByProductRatio.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdByProductRatio.RootTable.Columns["ByProductAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdByProductRatio.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, Conversion.ToInt(CmbInvoiceNoDetail.Value), ((Control)(object)CmbInvoiceNoDetail).Text, Conversion.ToInt(CmbCustomerDetail.Value), ((Control)(object)CmbCustomerDetail).Text, Conversion.ToDouble((object)txtCostAfterProcessingDetail.Text), Conversion.ToDouble((object)txtPackingChargesDetail.Text), Conversion.ToDouble((object)txtFobChargesDetail.Text), Conversion.ToDouble((object)txtTaxDetail.Text), Conversion.ToDouble((object)txtFreightDetail.Text), Conversion.ToDouble((object)txtFinancialChargesDetail.Text), Conversion.ToDouble((object)txtTotalCostDetail.Text), Conversion.ToDouble((object)txtExchangeRateDetail.Text), Conversion.ToInt(CmbFcyCodeDetail.Value), ((Control)(object)CmbFcyCodeDetail).Text, Conversion.ToDouble((object)txtFcyAmountDetail.Text), Conversion.ToDouble((object)txtCommissionAmountDetail.Text), Conversion.ToDouble((object)txtamountAfterCommissionDetail.Text), Conversion.ToDouble((object)txtItemSaleAmountDetail.Text), Conversion.ToDouble((object)txtContributionPerMTonDetail.Text), Conversion.ToString((object)txtRemarksDetail.Text));
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				CmbInvoiceNoDetail.Value = item.Cells["InvoiceId"].Value;
				CmbInvoiceNoDetail_Leave(null, null);
				CmbCustomerDetail.Value = item.Cells["SupplierCustomerId"].Value;
				txtCostAfterProcessingDetail.Text = item.Cells["CostAfterProcessing"].Value?.ToString();
				txtPackingChargesDetail.Text = item.Cells["PackingCharges"].Value?.ToString();
				txtFobChargesDetail.Text = item.Cells["FOBCharges"].Value?.ToString();
				txtTaxDetail.Text = item.Cells["Tax"].Value?.ToString();
				txtFreightDetail.Text = item.Cells["Freight"].Value?.ToString();
				txtFinancialChargesDetail.Text = item.Cells["FinancialCharges"].Value?.ToString();
				txtTotalCostDetail.Text = item.Cells["TotalCost"].Value?.ToString();
				txtExchangeRateDetail.Text = item.Cells["ExchangeRate"].Value?.ToString();
				CmbFcyCodeDetail.Value = item.Cells["FcyCodeId"].Value;
				txtFcyAmountDetail.Text = item.Cells["FcyAmount"].Value?.ToString();
				txtCommissionAmountDetail.Text = item.Cells["CommissionAmount"].Value?.ToString();
				txtamountAfterCommissionDetail.Text = item.Cells["AmountAfterCommission"].Value?.ToString();
				txtItemSaleAmountDetail.Text = item.Cells["ItemSaleAmount"].Value?.ToString();
				txtContributionPerMTonDetail.Text = item.Cells["ContributionPerMTon"].Value?.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value?.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				CmbInvoiceNoDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["InvoiceId"] = Conversion.ToInt(CmbInvoiceNoDetail.Value);
				dtdetail.Rows[updateDetailIndex]["InvoiceNo"] = ((Control)(object)CmbInvoiceNoDetail).Text;
				dtdetail.Rows[updateDetailIndex]["SupplierCustomerId"] = Conversion.ToInt(CmbCustomerDetail.Value);
				dtdetail.Rows[updateDetailIndex]["Customer"] = ((Control)(object)CmbCustomerDetail).Text;
				dtdetail.Rows[updateDetailIndex]["CostAfterProcessing"] = Conversion.ToDouble((object)txtCostAfterProcessingDetail.Text);
				dtdetail.Rows[updateDetailIndex]["PackingCharges"] = Conversion.ToDouble((object)txtPackingChargesDetail.Text);
				dtdetail.Rows[updateDetailIndex]["FOBCharges"] = Conversion.ToDouble((object)txtFobChargesDetail.Text);
				dtdetail.Rows[updateDetailIndex]["Tax"] = Conversion.ToDouble((object)txtTaxDetail.Text);
				dtdetail.Rows[updateDetailIndex]["Freight"] = Conversion.ToDouble((object)txtFreightDetail.Text);
				dtdetail.Rows[updateDetailIndex]["FinancialCharges"] = Conversion.ToDouble((object)txtFinancialChargesDetail.Text);
				dtdetail.Rows[updateDetailIndex]["TotalCost"] = Conversion.ToDouble((object)txtTotalCostDetail.Text);
				dtdetail.Rows[updateDetailIndex]["ExchangeRate"] = Conversion.ToDouble((object)txtExchangeRateDetail.Text);
				dtdetail.Rows[updateDetailIndex]["FcyCodeId"] = Conversion.ToInt(CmbFcyCodeDetail.Value);
				dtdetail.Rows[updateDetailIndex]["FcyCode"] = ((Control)(object)CmbFcyCodeDetail).Text;
				dtdetail.Rows[updateDetailIndex]["FcyAmount"] = Conversion.ToDouble((object)txtFcyAmountDetail.Text);
				dtdetail.Rows[updateDetailIndex]["CommissionAmount"] = Conversion.ToDouble((object)txtCommissionAmountDetail.Text);
				dtdetail.Rows[updateDetailIndex]["AmountAfterCommission"] = Conversion.ToDouble((object)txtamountAfterCommissionDetail.Text);
				dtdetail.Rows[updateDetailIndex]["ItemSaleAmount"] = Conversion.ToDouble((object)txtItemSaleAmountDetail.Text);
				dtdetail.Rows[updateDetailIndex]["ContributionPerMTon"] = Conversion.ToDouble((object)txtContributionPerMTonDetail.Text);
				dtdetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void grdSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvoiceId", "SupplierCustomerId", "FcyCodeId" };
			List<string> ExcludeC = new List<string> { "CostAfterProcessing" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, ExcludeC);
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["CostAfterProcessing"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["PackingCharges"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FOBCharges"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Tax"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FinancialCharges"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["TotalCost"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ExchangeRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["FcyCode"].Width = Constants.InventoryConstants.FcyConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["CommissionAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["AmountAfterCommission"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemSaleAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ContributionPerMTon"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddButton(grd, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 1);
			grd.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
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
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ProductionManualCostingInvoiceDetail vd = new ProductionManualCostingInvoiceDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillInvoiceDetailListCommonForInsertAndDelete(vd, r);
					vd.ActionTypeId = 3;
					lstDetailRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grd.UpdateData();
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Expected O, but got Unknown
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		try
		{
			if (grdInputWorking.GetRows().Count() == 0)
			{
				throw new Exception("Grid Input Record Not Found");
			}
			if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)CmbJobOrderNo, "JobOrder No", FormHelper.ValidType.String),
				(txtInputAvgRateHeader, "InputAvg Rate", FormHelper.ValidType.Double),
				(txtFinishOutPutPrcntObtainedHeader, "Finish OutPut % Obtained", FormHelper.ValidType.Double),
				(txtInputToBeProcessedHeader, "Input To Be Processed Obtained", FormHelper.ValidType.Double),
				(txtTotalCostHeader, "Total Cost", FormHelper.ValidType.Double),
				(txtRatePerMTonHeader, "Rate In MPR", FormHelper.ValidType.Double)
			}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			ProductionManualCosting obj = new ProductionManualCosting();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.Id = RecId;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.CostingDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ProductionJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			obj.InputAvgRate = Conversion.ToDecimal((object)txtInputAvgRateHeader.Text.Trim());
			obj.FinishedOutputPercent = Conversion.ToDecimal((object)txtFinishOutPutPrcntObtainedHeader.Text.Trim());
			obj.InputToBeProcessed = Conversion.ToDecimal((object)txtInputToBeProcessedHeader.Text.Trim());
			obj.ByProductCost = Conversion.ToDecimal((object)txtByProductCostHeader.Text.Trim());
			obj.TotalCost = Conversion.ToDecimal((object)txtTotalCostHeader.Text.Trim());
			obj.ProcessingCharges = Conversion.ToDecimal((object)txtProcessingChargesHeader.Text.Trim());
			obj.RateInPMT = Conversion.ToDecimal((object)txtRatePerMTonHeader.Text.Trim());
			obj.ProductionManualCostingByProductDetailList = new List<ProductionManualCostingByProductDetail>();
			obj.ProductionManualCostingInputDetailList = new List<ProductionManualCostingInputDetail>();
			obj.ProductionManualCostingInvoiceDetailList = new List<ProductionManualCostingInvoiceDetail>();
			if (lstBpRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int k = 0; k < lstBpRemoveRecord.Count; k++)
				{
					obj.ProductionManualCostingByProductDetailList.Add(lstBpRemoveRecord[k]);
				}
			}
			GridEXRow[] rows = grdInputWorking.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				ProductionManualCostingInputDetail vd3 = new ProductionManualCostingInputDetail();
				vd3.Id = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				vd3.ActionTypeId = ((vd3.Id <= 0) ? 1 : 2);
				FillInputDetailListCommonForInsertAndDelete(vd3, r3);
				FormHelper.ValidateField(vd3.ItemId, "Item", r3.RowIndex);
				FormHelper.ValidateField(vd3.Weight, "Weight", r3.RowIndex);
				FormHelper.ValidateField(vd3.Rate, "Rate", r3.RowIndex);
				FormHelper.ValidateField(vd3.FinalAmount, "Final Amount", r3.RowIndex);
				obj.ProductionManualCostingInputDetailList.Add(vd3);
			}
			if (lstInputRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstInputRemoveRecord.Count; j++)
				{
					obj.ProductionManualCostingInputDetailList.Add(lstInputRemoveRecord[j]);
				}
			}
			rows = grdByProductRatio.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ProductionManualCostingByProductDetail vd2 = new ProductionManualCostingByProductDetail();
				vd2.Id = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
				FillByProductListCommonForInsertAndDelete(vd2, r2);
				FormHelper.ValidateField(vd2.ItemId, "Item", r2.RowIndex);
				FormHelper.ValidateField(vd2.PercentOfTotalInput, "Percent Of Total Input", r2.RowIndex);
				FormHelper.ValidateField(vd2.Ratio, "Ratio", r2.RowIndex);
				FormHelper.ValidateField(vd2.Rate, "Rate", r2.RowIndex);
				FormHelper.ValidateField(vd2.Amount, "Amount", r2.RowIndex);
				obj.ProductionManualCostingByProductDetailList.Add(vd2);
			}
			if (lstDetailRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstDetailRemoveRecord.Count; i++)
				{
					obj.ProductionManualCostingInvoiceDetailList.Add(lstDetailRemoveRecord[i]);
				}
			}
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				ProductionManualCostingInvoiceDetail vd = new ProductionManualCostingInvoiceDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				FillInvoiceDetailListCommonForInsertAndDelete(vd, r);
				FormHelper.ValidateField(vd.ExImInvoiceId, "Invoice", r.RowIndex);
				FormHelper.ValidateField(vd.SupplierCustomerId, "Customer", r.RowIndex);
				FormHelper.ValidateField(vd.CostAfterProcessing, "Cost After Processing", r.RowIndex);
				FormHelper.ValidateField(vd.TotalCost, "Total Cost", r.RowIndex);
				FormHelper.ValidateField(vd.ExchangeRate, "Exchange Rate", r.RowIndex);
				FormHelper.ValidateField(vd.FcyCodeId, "FCY Code", r.RowIndex);
				FormHelper.ValidateField(vd.FcyAmount, "FCY Amount", r.RowIndex);
				FormHelper.ValidateField(vd.AmountAfterComm, "Amount After Commission", r.RowIndex);
				FormHelper.ValidateField(vd.ItemSaleAmount, "Item Sale Amount", r.RowIndex);
				FormHelper.ValidateField(vd.ContributionPerMTon, "Contribution Per M.Ton", r.RowIndex);
				obj.ProductionManualCostingInvoiceDetailList.Add(vd);
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ProductionManualCosting>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			ProductionManualCosting.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillInputDetailListCommonForInsertAndDelete(ProductionManualCostingInputDetail vd, GridEXRow r)
	{
		vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		vd.RefDocumentType = Conversion.ToString(r.Cells["RefDocumentType"].Value);
		vd.RefDocId = Conversion.ToInt(r.Cells["RefDocId"].Value);
		vd.RefDocNo = Conversion.ToString(r.Cells["RefDocNo"].Value);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
		vd.Weight = Conversion.ToDecimal(r.Cells["Weight"].Value);
		vd.InputAmount = Conversion.ToDecimal(r.Cells["InputAmount"].Value);
		vd.InputRate = Conversion.ToDecimal(r.Cells["InputRate"].Value);
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.FinalAmount = Conversion.ToDecimal(r.Cells["FinalAmount"].Value);
		vd.RemarksDetail = "";
	}

	private void FillByProductListCommonForInsertAndDelete(ProductionManualCostingByProductDetail vd, GridEXRow r)
	{
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.PercentOfTotalInput = Conversion.ToDecimal(r.Cells["PercentOfTotalInput"].Value);
		vd.Ratio = Conversion.ToDecimal(r.Cells["Ratio"].Value);
		vd.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.ByProductAmount = Conversion.ToDecimal(r.Cells["ByProductAmount"].Value);
		vd.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.RemarksDetail = "";
	}

	private void FillInvoiceDetailListCommonForInsertAndDelete(ProductionManualCostingInvoiceDetail vd, GridEXRow r)
	{
		vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
		vd.InvoiceNo = Conversion.ToString(r.Cells["InvoiceNo"].Value);
		vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
		vd.CustomerName = Conversion.ToString(r.Cells["Customer"].Value);
		vd.CostAfterProcessing = Conversion.ToDecimal(r.Cells["CostAfterProcessing"].Value);
		vd.PackingCharges = Conversion.ToDecimal(r.Cells["PackingCharges"].Value);
		vd.FOBCharges = Conversion.ToDecimal(r.Cells["FOBCharges"].Value);
		vd.Tax = Conversion.ToDecimal(r.Cells["Tax"].Value);
		vd.Freight = Conversion.ToDecimal(r.Cells["Freight"].Value);
		vd.FinancialCharges = Conversion.ToDecimal(r.Cells["FinancialCharges"].Value);
		vd.TotalCost = Conversion.ToDecimal(r.Cells["TotalCost"].Value);
		vd.ExchangeRate = Conversion.ToDecimal(r.Cells["ExchangeRate"].Value);
		vd.FcyCodeId = Conversion.ToInt(r.Cells["FcyCodeId"].Value);
		vd.FcyCode = Conversion.ToString(r.Cells["FcyCode"].Value);
		vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
		vd.CommissionAmount = Conversion.ToDecimal(r.Cells["CommissionAmount"].Value);
		vd.AmountAfterComm = Conversion.ToDecimal(r.Cells["AmountAfterCommission"].Value);
		vd.ItemSaleAmount = Conversion.ToDecimal(r.Cells["ItemSaleAmount"].Value);
		vd.ContributionPerMTon = Conversion.ToDecimal(r.Cells["ContributionPerMTon"].Value);
		vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	public bool ReadById(int JobOrderId)
	{
		try
		{
			bool Value = false;
			Reset();
			ProductionManualCosting Obj = ProductionManualCosting.ReadByJobOrderId(JobOrderId);
			if (Obj != null)
			{
				Value = true;
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				BtnDelete.Visible = true;
				RecId = Obj.Id;
				CmbJobOrderNo.Value = JobOrderId;
				txtInputAvgRateHeader.Text = Obj.InputAvgRate.ToString("#,##0.###");
				txtFinishOutPutPrcntObtainedHeader.Text = Obj.FinishedOutputPercent.ToString("#,##0.###");
				txtInputToBeProcessedHeader.Text = Obj.InputToBeProcessed.ToString("#,##0.###");
				txtByProductCostHeader.Text = Obj.ByProductCost.ToString("#,##0.###");
				txtTotalCostHeader.Text = Obj.TotalCost.ToString("#,##0.###");
				txtProcessingChargesHeader.Text = Obj.ProcessingCharges.ToString("#,##0.###");
				txtRatePerMTonHeader.Text = Obj.RateInPMT.ToString("#,##0.###");
				AttachmentsValues = Obj.AttachmentsValues;
				CustomAttachmentsValues = Obj.CustomAttachmentsValues;
				dtdetail.Rows.Clear();
				FillInvoiceDetailFromListCommonForReadById(Obj, dtdetail);
				dtInputDetail.Rows.Clear();
				FillInputDetailFromListCommonForReadById(Obj, dtInputDetail);
				dtBpDetail.Rows.Clear();
				FillByProductDetailFromListCommonForReadById(Obj, dtBpDetail);
				BindGrids();
				FormHelper.LoadAttachmentsForObject(AT, RecId, ScreenName);
			}
			return Value;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FillInputDetailFromListCommonForReadById(ProductionManualCosting Obj, DataTable dt)
	{
		foreach (ProductionManualCostingInputDetail item in Obj.ProductionManualCostingInputDetailList)
		{
			dt.Rows.Add(item.Id, item.RefDocumentTypeId, item.RefDocumentType, item.RefDocId, item.RefDocNo, item.ItemId, item.ItemCode, item.ItemName, item.Qty, item.Weight, item.InputAmount, item.InputRate, item.Rate, item.FinalAmount);
		}
	}

	private void FillByProductDetailFromListCommonForReadById(ProductionManualCosting Obj, DataTable dt)
	{
		decimal totalInputFinalAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdInputWorking, "FinalAmount"));
		decimal FinishPercent = Conversion.ToDecimal((object)txtFinishOutPutPrcntObtainedHeader.Text);
		decimal totalBpRatio = ((FinishPercent > 0m) ? (1000m / FinishPercent) : 0m);
		foreach (ProductionManualCostingByProductDetail item in Obj.ProductionManualCostingByProductDetailList)
		{
			decimal percentOfTotalInput = ((totalInputFinalAmount > 0m) ? (item.ByProductAmount / totalInputFinalAmount * 100m) : 0m);
			decimal Ratio = totalBpRatio * percentOfTotalInput;
			decimal FinalAmount = item.Rate * Ratio;
			dt.Rows.Add(item.Id, item.ItemId, item.ItemCode, item.ItemName, item.PercentOfTotalInput, Ratio, item.Rate, item.ByProductAmount, FinalAmount);
		}
	}

	private void FillInvoiceDetailFromListCommonForReadById(ProductionManualCosting Obj, DataTable dt)
	{
		foreach (ProductionManualCostingInvoiceDetail item in Obj.ProductionManualCostingInvoiceDetailList)
		{
			dt.Rows.Add(item.Id, item.ExImInvoiceId, item.InvoiceNo, item.SupplierCustomerId, item.CustomerName, item.CostAfterProcessing, item.PackingCharges, item.FOBCharges, item.Tax, item.Freight, item.FinancialCharges, item.TotalCost, item.ExchangeRate, item.FcyCodeId, item.FcyCode, item.FcyAmount, item.CommissionAmount, item.AmountAfterComm, item.ItemSaleAmount, item.ContributionPerMTon, item.RemarksDetail);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmCreateProductionCostingReport_FormClosed(object sender, FormClosedEventArgs e)
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
			GetConfigurationsFromGlobal();
			JobOrderNoBind(InvFoodProduction.FoodProduction_JobOrderWithFinishOutPutPrcntObtained(UserAccount.OrganizationId, UserAccount.CompanyId));
			ExportInvoiceNoBind(ExImInvoice.ExImInvoice_GetInvoicesData(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id));
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
			lstDetailRemoveRecord.Clear();
			lstInputRemoveRecord.Clear();
			lstBpRemoveRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			((Control)(object)CmbJobOrderNo).Enabled = true;
			btnShow.Enabled = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			BtnDelete.Visible = false;
			DataReset = false;
			RecId = 0;
			((Control)(object)CmbJobOrderNo).Text = string.Empty;
			ResetDetail();
			txtRemarksDetail.Text = "";
			dtdetail.Rows.Clear();
			dtInputDetail.Rows.Clear();
			dtBpDetail.Rows.Clear();
			BindGrids();
			txtInputAvgRateHeader.Text = string.Empty;
			txtFinishOutPutPrcntObtainedHeader.Text = string.Empty;
			txtInputToBeProcessedHeader.Text = string.Empty;
			txtByProductCostHeader.Text = string.Empty;
			txtTotalCostHeader.Text = string.Empty;
			txtProcessingChargesHeader.Text = string.Empty;
			txtTotalCostAfterProcessingPerMtonHeader.Text = string.Empty;
			txtCostAfterProcessingDetail.Text = string.Empty;
			txtRatePerMTonHeader.Text = string.Empty;
			tabControl1.SelectedIndex = 0;
			CmbJobOrderNo.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
			((UltraGridBase)CmbCustomerDetail).DataSource = null;
			((Control)(object)CmbCustomerDetail).Text = string.Empty;
			txtPackingChargesDetail.Text = string.Empty;
			txtFobChargesDetail.Text = string.Empty;
			txtTaxDetail.Text = string.Empty;
			txtFreightDetail.Text = string.Empty;
			txtFinancialChargesDetail.Text = string.Empty;
			txtTotalCostDetail.Text = string.Empty;
			txtExchangeRateDetail.Text = string.Empty;
			((UltraGridBase)CmbFcyCodeDetail).DataSource = null;
			((Control)(object)CmbFcyCodeDetail).Text = string.Empty;
			txtFcyAmountDetail.Text = string.Empty;
			txtCommissionAmountDetail.Text = string.Empty;
			txtamountAfterCommissionDetail.Text = string.Empty;
			txtItemSaleAmountDetail.Text = string.Empty;
			txtContributionPerMTonDetail.Text = string.Empty;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			CmbInvoiceNoDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateHeaderValues()
	{
		try
		{
			double TotalInputFinalAmount = GridEX_Helper.GetColumnSum(grdInputWorking, "FinalAmount");
			double TotalInputWeight = GridEX_Helper.GetColumnSum(grdInputWorking, "Weight");
			double inputAvgRateHeader = ((TotalInputFinalAmount > 0.0 && TotalInputWeight > 0.0) ? (TotalInputFinalAmount / TotalInputWeight * 1000.0) : 0.0);
			txtInputAvgRateHeader.Text = inputAvgRateHeader.ToString(clsGlobalVariables.DecimalRateFormate);
			double outputPercent = Conversion.ToDouble((object)txtFinishOutPutPrcntObtainedHeader.Text.Trim());
			double inputToBeProcessed = ((outputPercent > 0.0) ? (inputAvgRateHeader / outputPercent * 100.0) : 0.0);
			txtInputToBeProcessedHeader.Text = inputToBeProcessed.ToString(clsGlobalVariables.stringFormatsingle);
			double byProductCost = GridEX_Helper.GetColumnSum(grdByProductRatio, "Amount");
			txtByProductCostHeader.Text = byProductCost.ToString(clsGlobalVariables.stringFormatsingle);
			double totalCost = inputToBeProcessed - byProductCost;
			txtTotalCostHeader.Text = totalCost.ToString(clsGlobalVariables.stringFormatsingle);
			double processingCharges = Conversion.ToDouble((object)txtProcessingChargesHeader.Text);
			double totalAfterProcessing = totalCost + processingCharges;
			txtTotalCostAfterProcessingPerMtonHeader.Text = totalAfterProcessing.ToString(clsGlobalVariables.stringFormatsingle);
			txtCostAfterProcessingDetail.Text = totalAfterProcessing.ToString(clsGlobalVariables.stringFormatsingle);
			double ratePerMTon = totalAfterProcessing / 1000.0;
			txtRatePerMTonHeader.Text = ratePerMTon.ToString(clsGlobalVariables.stringFormatsingle);
			UpdateInvoiceGridValues();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProcessingChargesHeader_TextChanged(object sender, EventArgs e)
	{
		CalculateHeaderValues();
	}

	private void UpdateInvoiceGridValues()
	{
		try
		{
			double totalAfterProcessing = Conversion.ToDouble((object)txtTotalCostAfterProcessingPerMtonHeader.Text);
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				obj.BeginEdit();
				obj.Cells["CostAfterProcessing"].Value = totalAfterProcessing;
				double packingCharges = Conversion.ToDouble(obj.Cells["PackingCharges"].Value);
				double fobCharges = Conversion.ToDouble(obj.Cells["FOBCharges"].Value);
				double tax = Conversion.ToDouble(obj.Cells["Tax"].Value);
				double freight = Conversion.ToDouble(obj.Cells["Freight"].Value);
				double financialCharges = Conversion.ToDouble(obj.Cells["FinancialCharges"].Value);
				double totalCost = totalAfterProcessing + packingCharges + fobCharges + tax + freight + financialCharges;
				obj.Cells["TotalCost"].Value = totalCost;
				double ExchangeRate = Conversion.ToDouble(obj.Cells["ExchangeRate"].Value);
				double FcyAmount = ((ExchangeRate > 0.0) ? (totalCost / ExchangeRate) : 0.0);
				obj.Cells["FcyAmount"].Value = FcyAmount;
				double commissionAmount = Conversion.ToDouble(obj.Cells["CommissionAmount"].Value);
				double amountAfterCommission = FcyAmount - commissionAmount;
				obj.Cells["AmountAfterCommission"].Value = amountAfterCommission;
				double ContributionPerMTon = Conversion.ToDouble(obj.Cells["ItemSaleAmount"].Value) - amountAfterCommission;
				obj.Cells["ContributionPerMTon"].Value = ContributionPerMTon;
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalCostDetail()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtCostAfterProcessingDetail.Text.Trim());
			double packingCharges = Conversion.ToDouble((object)txtPackingChargesDetail.Text.Trim());
			double fobCharges = Conversion.ToDouble((object)txtFobChargesDetail.Text.Trim());
			double tax = Conversion.ToDouble((object)txtTaxDetail.Text.Trim());
			double freight = Conversion.ToDouble((object)txtFreightDetail.Text.Trim());
			double financialCharges = Conversion.ToDouble((object)txtFinancialChargesDetail.Text.Trim());
			double totalCost = num + packingCharges + fobCharges + tax + freight + financialCharges;
			txtTotalCostDetail.Text = totalCost.ToString(clsGlobalVariables.stringFormatsingle);
			double ExchangeRate = Conversion.ToDouble((object)txtExchangeRateDetail.Text);
			double FcyAmount = ((ExchangeRate > 0.0) ? (totalCost / ExchangeRate) : 0.0);
			txtFcyAmountDetail.Text = FcyAmount.ToString(clsGlobalVariables.stringFormatsingle);
			double commissionAmount = Conversion.ToDouble((object)txtCommissionAmountDetail.Text.Trim());
			double amountAfterCommission = FcyAmount - commissionAmount;
			txtamountAfterCommissionDetail.Text = amountAfterCommission.ToString(clsGlobalVariables.stringFormatsingle);
			double ContributionPerMTon = Conversion.ToDouble((object)txtItemSaleAmountDetail.Text.Trim()) - amountAfterCommission;
			txtContributionPerMTonDetail.Text = ContributionPerMTon.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCostAfterProcessingDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtPackingChargesDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtFobChargesDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtTaxDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtFreightDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtFinancialChargesDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtExchangeRateDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtCommissionAmountDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void txtItemSaleAmountDetail_TextChanged(object sender, EventArgs e)
	{
		CalculateTotalCostDetail();
	}

	private void tsDropDowngrnWiseInput_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		try
		{
			int JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			if (((UltraGridBase)CmbJobOrderNo).ActiveRow == null || JobOrderId == 0)
			{
				MessageBox.Show("Please Select JobOrder First");
				CmbJobOrderNo.Focus();
			}
			else
			{
				OpenPrint(JobOrderId, e.ClickedItem.Text);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenPrint(int JobOrderId, string ReportName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ProductionManualCosting.ProductionManualCosting_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value),
				DocumentTypeId = DocumentTypeId
			});
			if (dt.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found For DisPlay");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject SubInput = new SubReportObject(ProductionManualCosting.ProductionManualCosting_InputDetailSubReport(UserAccount.OrganizationId, UserAccount.CompanyId, JobOrderId), "ProductionManualCosting_InputDetailSubRepirt.rpt");
			lstSubReports.Add(SubInput);
			SubReportObject SubByProduct = new SubReportObject(ProductionManualCosting.ProductionManualCosting_ByProductDetailSubReport(UserAccount.OrganizationId, UserAccount.CompanyId, JobOrderId), "ProductionManualCosting_ByProductDetailSubReport.rpt");
			lstSubReports.Add(SubByProduct);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, ReportName + ".rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsLoad()
	{
		try
		{
			CrystalReportPrint_Helper.LoadReportsToDropdown("ProductionManualCosting", BtnPrintDropDown);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdInputWorking, dtInputDetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdByProductRatio, dtBpDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtdetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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

	private void frmCreateProductionCostingReport_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.S && btnsave.Visible)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && RecId > 0)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				tsDropDowngrnWiseInput_DropDownItemClicked(null, null);
			}
			if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
			{
				tsDropDowngrnWiseInput_DropDownItemClicked(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbJobOrderNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else
				{
					CmbInvoiceNoDetail.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdByProductRatio).Focus();
				}
				else
				{
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdByProductRatio).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					CmbInvoiceNoDetail.Focus();
				}
				else if (tabControl1.SelectedIndex == 1 && ((Control)(object)CmbInvoiceNoDetail).Focused)
				{
					tabControl1.SelectedIndex = 0;
					((Control)(object)grdInputWorking).Focus();
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on JobOrder");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Grids");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Column in Detail for Entry");
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

	private void BtnByProductSettlementPopUp_Click(object sender, EventArgs e)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Expected O, but got Unknown
		try
		{
			int JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			DataTable productionByProductDataByJobOrderId = InvFoodProduction.getProductionByProductDataByJobOrderId(UserAccount.CompanyId, JobOrderId);
			List<string> columnsToHide = new List<string> { "ItemId", "JobOrderId" };
			Dictionary<string, EditType> editableCols = new Dictionary<string, EditType> { 
			{
				"Rate/Kg",
				(EditType)1
			} };
			GrdPopUpToReturndt popup = new GrdPopUpToReturndt(productionByProductDataByJobOrderId, selection: false, returnFullGrdDt: true, columnsToHide, editableCols)
			{
				StartPosition = FormStartPosition.CenterScreen,
				WindowState = FormWindowState.Normal,
				Text = "Please double-click any row to update values."
			};
			popup.ShowDialog();
			popup.BringToFront();
			DataTable dtReturn = popup.dtReturn;
			if (dtReturn == null || dtReturn.Rows.Count <= 0)
			{
				return;
			}
			ReportsParameters obj = new ReportsParameters();
			obj.ReportsParameterslist = new List<ReportsParameters>();
			foreach (DataRow item in popup.dtReturn.Rows)
			{
				ReportsParameters rpt = new ReportsParameters();
				rpt.CompanyId = UserAccount.CompanyId;
				rpt.JobOrderId = JobOrderId;
				rpt.ItemId = Conversion.ToInt(item["ItemId"]);
				rpt.ItemRate = Conversion.ToDouble(item["Rate/Kg"]);
				obj.ReportsParameterslist.Add(rpt);
			}
			InvFoodProduction.ProductionByProductRateUpdateByJobOrderId(obj);
			btnShow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlMain.SelectedTab.Name == "tabHistory")
			{
				PanelOtherForms.Visible = true;
				tabControlMain.Dock = DockStyle.Top;
				tabControlMain.Height = 27;
				string screenName = "frmProductionCostingReport";
				frmProductionCostingReport form = new frmProductionCostingReport(UserAccount);
				form.OpenInTab = true;
				form.OpenInTabId = 1;
				form.CreateProductionCostingReadById += FrmProductionCostingReport_EditRequested;
				FormHelper.OpenFormInPanel(screenName, form, PanelOtherForms);
				CostingReportForm = FormHelper.OpenOrGetFormInPanel(screenName, () => new frmProductionCostingReport(UserAccount), PanelOtherForms);
			}
			else
			{
				PanelOtherForms.Visible = false;
				tabControlMain.Dock = DockStyle.Fill;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrmProductionCostingReport_EditRequested(object sender, CreateProductionCostingReportIdEventArgs e)
	{
		try
		{
			int id = e.ProductionJobOrderId;
			tabControlMain.SelectedTab = tabForm;
			PanelOtherForms.Visible = false;
			CmbJobOrderNo.Value = id;
			btnShow_Click(null, null);
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
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab8: Expected O, but got Unknown
		//IL_519b: Unknown result type (might be due to invalid IL or missing references)
		//IL_51a5: Expected O, but got Unknown
		Appearance appearance5 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmCreateProductionCostingReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.txtRatePerMTonHeader = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtTotalCostAfterProcessingPerMtonHeader = new System.Windows.Forms.TextBox();
		this.txtProcessingChargesHeader = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtTotalCostHeader = new System.Windows.Forms.TextBox();
		this.txtByProductCostHeader = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtInputToBeProcessedHeader = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtFinishOutPutPrcntObtainedHeader = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtInputAvgRateHeader = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabInputWorking = new System.Windows.Forms.TabPage();
		this.grdInputWorking = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabByProductRatio = new System.Windows.Forms.TabPage();
		this.grdByProductRatio = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.BtnByProductSettlementPopUp = new System.Windows.Forms.Button();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel20 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbJobOrderNo = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.CmbCustomerDetail = new UltraCombo();
		this.CmbFcyCodeDetail = new UltraCombo();
		this.CmbInvoiceNoDetail = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.txtFinancialChargesDetail = new System.Windows.Forms.TextBox();
		this.txtFreightDetail = new System.Windows.Forms.TextBox();
		this.txtTaxDetail = new System.Windows.Forms.TextBox();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtFobChargesDetail = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtCommissionAmountDetail = new System.Windows.Forms.TextBox();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.txtPackingChargesDetail = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtTotalCostDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtExchangeRateDetail = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtContributionPerMTonDetail = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.txtItemSaleAmountDetail = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtamountAfterCommissionDetail = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtCostAfterProcessingDetail = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintDropDown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.PanelOtherForms = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabInputWorking.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputWorking).BeginInit();
		this.panel21.SuspendLayout();
		this.tabByProductRatio.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProductRatio).BeginInit();
		this.panel23.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCodeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabControlMain.SuspendLayout();
		this.tabForm.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1070, 528);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add(this.txtRatePerMTonHeader);
		this.panel3.Controls.Add(this.label39);
		this.panel3.Controls.Add(this.label38);
		this.panel3.Controls.Add(this.label37);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add(this.label34);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.txtTotalCostAfterProcessingPerMtonHeader);
		this.panel3.Controls.Add(this.txtProcessingChargesHeader);
		this.panel3.Controls.Add(this.label32);
		this.panel3.Controls.Add(this.txtTotalCostHeader);
		this.panel3.Controls.Add(this.txtByProductCostHeader);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.txtInputToBeProcessedHeader);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.txtFinishOutPutPrcntObtainedHeader);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.txtInputAvgRateHeader);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(747, 50);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(323, 168);
		this.panel3.TabIndex = 0;
		this.txtRatePerMTonHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRatePerMTonHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRatePerMTonHeader.Location = new System.Drawing.Point(182, 217);
		this.txtRatePerMTonHeader.Name = "txtRatePerMTonHeader";
		this.txtRatePerMTonHeader.ReadOnly = true;
		this.txtRatePerMTonHeader.Size = new System.Drawing.Size(150, 23);
		this.txtRatePerMTonHeader.TabIndex = 7;
		this.txtRatePerMTonHeader.TabStop = false;
		this.txtRatePerMTonHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.ForeColor = System.Drawing.Color.Red;
		this.label39.Location = new System.Drawing.Point(167, 220);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(14, 17);
		this.label39.TabIndex = 592;
		this.label39.Text = "*";
		this.label39.Visible = false;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Red;
		this.label38.Location = new System.Drawing.Point(167, 195);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(14, 17);
		this.label38.TabIndex = 592;
		this.label38.Text = "*";
		this.label38.Visible = false;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(167, 170);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 17);
		this.label37.TabIndex = 592;
		this.label37.Text = "*";
		this.label37.Visible = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(167, 127);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 592;
		this.label35.Text = "*";
		this.label35.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(167, 102);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 592;
		this.label34.Text = "*";
		this.label34.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(167, 36);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 592;
		this.label43.Text = "*";
		this.label43.Visible = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(167, 77);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 592;
		this.label33.Text = "*";
		this.label33.Visible = false;
		this.txtTotalCostAfterProcessingPerMtonHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalCostAfterProcessingPerMtonHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalCostAfterProcessingPerMtonHeader.Location = new System.Drawing.Point(182, 192);
		this.txtTotalCostAfterProcessingPerMtonHeader.Name = "txtTotalCostAfterProcessingPerMtonHeader";
		this.txtTotalCostAfterProcessingPerMtonHeader.ReadOnly = true;
		this.txtTotalCostAfterProcessingPerMtonHeader.Size = new System.Drawing.Size(150, 23);
		this.txtTotalCostAfterProcessingPerMtonHeader.TabIndex = 6;
		this.txtTotalCostAfterProcessingPerMtonHeader.TabStop = false;
		this.txtTotalCostAfterProcessingPerMtonHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtProcessingChargesHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProcessingChargesHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProcessingChargesHeader.Location = new System.Drawing.Point(182, 167);
		this.txtProcessingChargesHeader.Name = "txtProcessingChargesHeader";
		this.txtProcessingChargesHeader.Size = new System.Drawing.Size(150, 23);
		this.txtProcessingChargesHeader.TabIndex = 5;
		this.txtProcessingChargesHeader.Tag = "double";
		this.txtProcessingChargesHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtProcessingChargesHeader.TextChanged += new System.EventHandler(txtProcessingChargesHeader_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(167, 10);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 592;
		this.label32.Text = "*";
		this.label32.Visible = false;
		this.txtTotalCostHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalCostHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalCostHeader.Location = new System.Drawing.Point(182, 124);
		this.txtTotalCostHeader.Name = "txtTotalCostHeader";
		this.txtTotalCostHeader.ReadOnly = true;
		this.txtTotalCostHeader.Size = new System.Drawing.Size(150, 23);
		this.txtTotalCostHeader.TabIndex = 4;
		this.txtTotalCostHeader.TabStop = false;
		this.txtTotalCostHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtByProductCostHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtByProductCostHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtByProductCostHeader.Location = new System.Drawing.Point(182, 99);
		this.txtByProductCostHeader.Name = "txtByProductCostHeader";
		this.txtByProductCostHeader.ReadOnly = true;
		this.txtByProductCostHeader.Size = new System.Drawing.Size(150, 23);
		this.txtByProductCostHeader.TabIndex = 3;
		this.txtByProductCostHeader.TabStop = false;
		this.txtByProductCostHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(6, 223);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(71, 15);
		this.label23.TabIndex = 197;
		this.label23.Text = "Rate in PMT\r\n";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(6, 196);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(148, 15);
		this.label22.TabIndex = 197;
		this.label22.Text = "Total Cost After Processing";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 171);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(109, 15);
		this.label21.TabIndex = 197;
		this.label21.Text = "Processing Charges";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(6, 128);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(59, 15);
		this.label20.TabIndex = 197;
		this.label20.Text = "Total Cost";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(6, 103);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(93, 15);
		this.label19.TabIndex = 197;
		this.label19.Text = "By-Product Cost";
		this.txtInputToBeProcessedHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputToBeProcessedHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputToBeProcessedHeader.Location = new System.Drawing.Point(182, 74);
		this.txtInputToBeProcessedHeader.Name = "txtInputToBeProcessedHeader";
		this.txtInputToBeProcessedHeader.ReadOnly = true;
		this.txtInputToBeProcessedHeader.Size = new System.Drawing.Size(150, 23);
		this.txtInputToBeProcessedHeader.TabIndex = 2;
		this.txtInputToBeProcessedHeader.TabStop = false;
		this.txtInputToBeProcessedHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(6, 78);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(122, 15);
		this.label18.TabIndex = 197;
		this.label18.Text = "Input to Be Processed";
		this.txtFinishOutPutPrcntObtainedHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinishOutPutPrcntObtainedHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinishOutPutPrcntObtainedHeader.Location = new System.Drawing.Point(182, 33);
		this.txtFinishOutPutPrcntObtainedHeader.Name = "txtFinishOutPutPrcntObtainedHeader";
		this.txtFinishOutPutPrcntObtainedHeader.ReadOnly = true;
		this.txtFinishOutPutPrcntObtainedHeader.Size = new System.Drawing.Size(150, 23);
		this.txtFinishOutPutPrcntObtainedHeader.TabIndex = 1;
		this.txtFinishOutPutPrcntObtainedHeader.TabStop = false;
		this.txtFinishOutPutPrcntObtainedHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(6, 37);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(157, 15);
		this.label17.TabIndex = 197;
		this.label17.Text = "Finished Output % Obtained";
		this.txtInputAvgRateHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInputAvgRateHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInputAvgRateHeader.Location = new System.Drawing.Point(182, 7);
		this.txtInputAvgRateHeader.Name = "txtInputAvgRateHeader";
		this.txtInputAvgRateHeader.ReadOnly = true;
		this.txtInputAvgRateHeader.Size = new System.Drawing.Size(150, 23);
		this.txtInputAvgRateHeader.TabIndex = 0;
		this.txtInputAvgRateHeader.TabStop = false;
		this.txtInputAvgRateHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(6, 3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(140, 31);
		this.label16.TabIndex = 197;
		this.label16.Text = "Input Avg Rate (Total Cost / Total Qty)";
		this.panel4.Controls.Add(this.tabControl1);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 50);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(747, 168);
		this.panel4.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabInputWorking);
		this.tabControl1.Controls.Add(this.tabByProductRatio);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 37);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(747, 131);
		this.tabControl1.TabIndex = 123;
		this.tabInputWorking.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputWorking);
		this.tabInputWorking.Controls.Add(this.panel21);
		this.tabInputWorking.Location = new System.Drawing.Point(4, 25);
		this.tabInputWorking.Name = "tabInputWorking";
		this.tabInputWorking.Padding = new System.Windows.Forms.Padding(3);
		this.tabInputWorking.Size = new System.Drawing.Size(739, 102);
		this.tabInputWorking.TabIndex = 0;
		this.tabInputWorking.Text = "Input Working";
		this.tabInputWorking.UseVisualStyleBackColor = true;
		this.grdInputWorking.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdInputWorking).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInputWorking.DynamicFiltering = true;
		this.grdInputWorking.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdInputWorking).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputWorking.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputWorking).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.grdInputWorking).Name = "grdInputWorking";
		((ScrollableBase)this.grdInputWorking).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputWorking).Size = new System.Drawing.Size(733, 76);
		((System.Windows.Forms.Control)(object)this.grdInputWorking).TabIndex = 1;
		this.grdInputWorking.TotalRow = (InheritableBoolean)1;
		this.grdInputWorking.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputWorking).VisualStyle = (VisualStyle)4;
		this.grdInputWorking.CellUpdated += new ColumnActionEventHandler(grdInputWorking_CellUpdated);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.ctrlGrdBar1);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(3, 3);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(733, 20);
		this.panel21.TabIndex = 122;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(691, 0);
		this.ctrlGrdBar1.MyGrid = this.grdInputWorking;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 20);
		this.ctrlGrdBar1.TabIndex = 60;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabByProductRatio.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProductRatio);
		this.tabByProductRatio.Controls.Add(this.panel23);
		this.tabByProductRatio.Location = new System.Drawing.Point(4, 25);
		this.tabByProductRatio.Name = "tabByProductRatio";
		this.tabByProductRatio.Padding = new System.Windows.Forms.Padding(3);
		this.tabByProductRatio.Size = new System.Drawing.Size(739, 185);
		this.tabByProductRatio.TabIndex = 1;
		this.tabByProductRatio.Text = "By Product Ratio";
		this.tabByProductRatio.UseVisualStyleBackColor = true;
		this.grdByProductRatio.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdByProductRatio.DynamicFiltering = true;
		this.grdByProductRatio.FilterMode = (FilterMode)2;
		this.grdByProductRatio.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProductRatio.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).Location = new System.Drawing.Point(3, 23);
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).Name = "grdByProductRatio";
		((ScrollableBase)this.grdByProductRatio).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).Size = new System.Drawing.Size(733, 159);
		((System.Windows.Forms.Control)(object)this.grdByProductRatio).TabIndex = 2;
		this.grdByProductRatio.TotalRow = (InheritableBoolean)1;
		this.grdByProductRatio.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProductRatio).VisualStyle = (VisualStyle)4;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.BtnByProductSettlementPopUp);
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(3, 3);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(733, 20);
		this.panel23.TabIndex = 124;
		this.BtnByProductSettlementPopUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnByProductSettlementPopUp.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnByProductSettlementPopUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnByProductSettlementPopUp.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnByProductSettlementPopUp.ForeColor = System.Drawing.Color.White;
		this.BtnByProductSettlementPopUp.Location = new System.Drawing.Point(519, -2);
		this.BtnByProductSettlementPopUp.Margin = new System.Windows.Forms.Padding(2);
		this.BtnByProductSettlementPopUp.Name = "BtnByProductSettlementPopUp";
		this.BtnByProductSettlementPopUp.Size = new System.Drawing.Size(165, 22);
		this.BtnByProductSettlementPopUp.TabIndex = 60;
		this.BtnByProductSettlementPopUp.Text = "BY-Product Item Wise";
		this.BtnByProductSettlementPopUp.UseVisualStyleBackColor = false;
		this.BtnByProductSettlementPopUp.Click += new System.EventHandler(BtnByProductSettlementPopUp_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(691, 0);
		this.ctrlGrdBar2.MyGrid = this.grdByProductRatio;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 19);
		this.ctrlGrdBar2.TabIndex = 59;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel20.Controls.Add(this.btnShow);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.panel20.Controls.Add(this.label68);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(747, 37);
		this.panel20.TabIndex = 0;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(337, 6);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(60, 25);
		this.btnShow.TabIndex = 1;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(86, 5);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 0;
		this.label68.AutoSize = true;
		this.label68.BackColor = System.Drawing.Color.White;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 8.5f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Black;
		this.label68.Location = new System.Drawing.Point(5, 11);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(81, 15);
		this.label68.TabIndex = 117;
		this.label68.Text = "Job Order No";
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.label40);
		this.panel7.Controls.Add(this.label26);
		this.panel7.Controls.Add(this.label25);
		this.panel7.Controls.Add(this.label31);
		this.panel7.Controls.Add(this.label42);
		this.panel7.Controls.Add(this.label41);
		this.panel7.Controls.Add(this.label29);
		this.panel7.Controls.Add(this.label28);
		this.panel7.Controls.Add(this.label27);
		this.panel7.Controls.Add(this.label24);
		this.panel7.Controls.Add(this.btnAddDetail);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerDetail);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.label6);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label30);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add(this.label64);
		this.panel7.Controls.Add(this.txtFinancialChargesDetail);
		this.panel7.Controls.Add(this.txtFreightDetail);
		this.panel7.Controls.Add(this.txtTaxDetail);
		this.panel7.Controls.Add(this.txtRemarksDetail);
		this.panel7.Controls.Add(this.txtFobChargesDetail);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.txtCommissionAmountDetail);
		this.panel7.Controls.Add(this.txtFcyAmountDetail);
		this.panel7.Controls.Add(this.txtPackingChargesDetail);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.txtTotalCostDetail);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.btnCancelDetail);
		this.panel7.Controls.Add(this.txtExchangeRateDetail);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.btnUpdateDetail);
		this.panel7.Controls.Add(this.txtContributionPerMTonDetail);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.txtItemSaleAmountDetail);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.txtamountAfterCommissionDetail);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.txtCostAfterProcessingDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(0, 218);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1070, 100);
		this.panel7.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(306, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(119, 15);
		this.label3.TabIndex = 197;
		this.label3.Text = "Cost After Processing";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(1050, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 17);
		this.label40.TabIndex = 592;
		this.label40.Text = "*";
		this.label40.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(949, 5);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 592;
		this.label26.Text = "*";
		this.label26.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(162, 5);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(14, 17);
		this.label25.TabIndex = 592;
		this.label25.Text = "*";
		this.label25.Visible = false;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(422, 5);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 17);
		this.label31.TabIndex = 592;
		this.label31.Text = "*";
		this.label31.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(696, 54);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 592;
		this.label42.Text = "*";
		this.label42.Visible = false;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(446, 54);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 592;
		this.label41.Text = "*";
		this.label41.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(563, 54);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 592;
		this.label29.Text = "*";
		this.label29.Visible = false;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(176, 54);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 592;
		this.label28.Text = "*";
		this.label28.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(55, 54);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 592;
		this.label27.Text = "*";
		this.label27.Visible = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.Red;
		this.label24.Location = new System.Drawing.Point(85, 5);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(14, 17);
		this.label24.TabIndex = 592;
		this.label24.Text = "*";
		this.label24.Visible = false;
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(996, 73);
		this.btnAddDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(41, 23);
		this.btnAddDetail.TabIndex = 17;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.CmbCustomerDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDetail).Location = new System.Drawing.Point(105, 24);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDetail).Name = "CmbCustomerDetail";
		((System.Windows.Forms.Control)(object)this.CmbCustomerDetail).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDetail).TabIndex = 1;
		this.CmbFcyCodeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCodeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCodeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCodeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCodeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCodeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Location = new System.Drawing.Point(3, 71);
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Name = "CmbFcyCodeDetail";
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbFcyCodeDetail).TabIndex = 10;
		this.CmbInvoiceNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Name = "CmbInvoiceNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Leave += new System.EventHandler(CmbInvoiceNoDetail_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(792, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(99, 15);
		this.label7.TabIndex = 206;
		this.label7.Text = "Financial Charges";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(711, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(44, 15);
		this.label6.TabIndex = 206;
		this.label6.Text = "Frieght";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(630, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(25, 15);
		this.label5.TabIndex = 206;
		this.label5.Text = "Tax";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(711, 55);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(52, 15);
		this.label30.TabIndex = 206;
		this.label30.Text = "Remarks";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(539, 6);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(74, 15);
		this.label4.TabIndex = 206;
		this.label4.Text = "FOB Charges";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(214, 55);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(120, 15);
		this.label12.TabIndex = 206;
		this.label12.Text = "Commission Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(105, 55);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(72, 15);
		this.label11.TabIndex = 206;
		this.label11.Text = "Fcy Amount";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(437, 6);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(94, 15);
		this.label64.TabIndex = 206;
		this.label64.Text = "Packing Charges";
		this.txtFinancialChargesDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFinancialChargesDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFinancialChargesDetail.Location = new System.Drawing.Point(792, 26);
		this.txtFinancialChargesDetail.Name = "txtFinancialChargesDetail";
		this.txtFinancialChargesDetail.Size = new System.Drawing.Size(100, 23);
		this.txtFinancialChargesDetail.TabIndex = 7;
		this.txtFinancialChargesDetail.Tag = "double";
		this.txtFinancialChargesDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFinancialChargesDetail.TextChanged += new System.EventHandler(txtFinancialChargesDetail_TextChanged);
		this.txtFreightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightDetail.Location = new System.Drawing.Point(711, 26);
		this.txtFreightDetail.Name = "txtFreightDetail";
		this.txtFreightDetail.Size = new System.Drawing.Size(80, 23);
		this.txtFreightDetail.TabIndex = 6;
		this.txtFreightDetail.Tag = "double";
		this.txtFreightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightDetail.TextChanged += new System.EventHandler(txtFreightDetail_TextChanged);
		this.txtTaxDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxDetail.Location = new System.Drawing.Point(630, 26);
		this.txtTaxDetail.Name = "txtTaxDetail";
		this.txtTaxDetail.Size = new System.Drawing.Size(80, 23);
		this.txtTaxDetail.TabIndex = 5;
		this.txtTaxDetail.Tag = "double";
		this.txtTaxDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxDetail.TextChanged += new System.EventHandler(txtTaxDetail_TextChanged);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(711, 73);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(282, 23);
		this.txtRemarksDetail.TabIndex = 16;
		this.txtRemarksDetail.Tag = "double";
		this.txtRemarksDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarksDetail.TextChanged += new System.EventHandler(txtFobChargesDetail_TextChanged);
		this.txtFobChargesDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFobChargesDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobChargesDetail.Location = new System.Drawing.Point(539, 26);
		this.txtFobChargesDetail.Name = "txtFobChargesDetail";
		this.txtFobChargesDetail.Size = new System.Drawing.Size(90, 23);
		this.txtFobChargesDetail.TabIndex = 4;
		this.txtFobChargesDetail.Tag = "double";
		this.txtFobChargesDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobChargesDetail.TextChanged += new System.EventHandler(txtFobChargesDetail_TextChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(3, 55);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(55, 15);
		this.label10.TabIndex = 193;
		this.label10.Text = "Fcy Code";
		this.txtCommissionAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionAmountDetail.Location = new System.Drawing.Point(214, 73);
		this.txtCommissionAmountDetail.Name = "txtCommissionAmountDetail";
		this.txtCommissionAmountDetail.Size = new System.Drawing.Size(120, 23);
		this.txtCommissionAmountDetail.TabIndex = 12;
		this.txtCommissionAmountDetail.Tag = "double";
		this.txtCommissionAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommissionAmountDetail.TextChanged += new System.EventHandler(txtCommissionAmountDetail_TextChanged);
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(105, 73);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ReadOnly = true;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(108, 23);
		this.txtFcyAmountDetail.TabIndex = 11;
		this.txtFcyAmountDetail.Tag = "double";
		this.txtFcyAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingChargesDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingChargesDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingChargesDetail.Location = new System.Drawing.Point(437, 26);
		this.txtPackingChargesDetail.Name = "txtPackingChargesDetail";
		this.txtPackingChargesDetail.Size = new System.Drawing.Size(101, 23);
		this.txtPackingChargesDetail.TabIndex = 3;
		this.txtPackingChargesDetail.Tag = "double";
		this.txtPackingChargesDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingChargesDetail.TextChanged += new System.EventHandler(txtPackingChargesDetail_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 193;
		this.label1.Text = "Export Invoice";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(105, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 193;
		this.label2.Text = "Customer";
		this.txtTotalCostDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalCostDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalCostDetail.Location = new System.Drawing.Point(893, 26);
		this.txtTotalCostDetail.Name = "txtTotalCostDetail";
		this.txtTotalCostDetail.ReadOnly = true;
		this.txtTotalCostDetail.Size = new System.Drawing.Size(100, 23);
		this.txtTotalCostDetail.TabIndex = 8;
		this.txtTotalCostDetail.TabStop = false;
		this.txtTotalCostDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(893, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(59, 15);
		this.label8.TabIndex = 197;
		this.label8.Text = "Total Cost";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(996, 49);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(70, 23);
		this.btnCancelDetail.TabIndex = 19;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.txtExchangeRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRateDetail.Location = new System.Drawing.Point(994, 26);
		this.txtExchangeRateDetail.Name = "txtExchangeRateDetail";
		this.txtExchangeRateDetail.Size = new System.Drawing.Size(85, 23);
		this.txtExchangeRateDetail.TabIndex = 9;
		this.txtExchangeRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRateDetail.TextChanged += new System.EventHandler(txtExchangeRateDetail_TextChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(994, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(58, 15);
		this.label9.TabIndex = 197;
		this.label9.Text = "Exch Rate";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(996, 73);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 23);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtContributionPerMTonDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContributionPerMTonDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContributionPerMTonDetail.Location = new System.Drawing.Point(582, 73);
		this.txtContributionPerMTonDetail.Name = "txtContributionPerMTonDetail";
		this.txtContributionPerMTonDetail.ReadOnly = true;
		this.txtContributionPerMTonDetail.Size = new System.Drawing.Size(128, 23);
		this.txtContributionPerMTonDetail.TabIndex = 15;
		this.txtContributionPerMTonDetail.Tag = "double";
		this.txtContributionPerMTonDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(582, 55);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(116, 15);
		this.label15.TabIndex = 197;
		this.label15.Text = "Contribution / MTon";
		this.txtItemSaleAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemSaleAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemSaleAmountDetail.Location = new System.Drawing.Point(461, 73);
		this.txtItemSaleAmountDetail.Name = "txtItemSaleAmountDetail";
		this.txtItemSaleAmountDetail.Size = new System.Drawing.Size(120, 23);
		this.txtItemSaleAmountDetail.TabIndex = 14;
		this.txtItemSaleAmountDetail.Tag = "double";
		this.txtItemSaleAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemSaleAmountDetail.TextChanged += new System.EventHandler(txtItemSaleAmountDetail_TextChanged);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(461, 55);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(104, 15);
		this.label14.TabIndex = 197;
		this.label14.Text = "Item Sale Amount";
		this.txtamountAfterCommissionDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamountAfterCommissionDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamountAfterCommissionDetail.Location = new System.Drawing.Point(335, 73);
		this.txtamountAfterCommissionDetail.Name = "txtamountAfterCommissionDetail";
		this.txtamountAfterCommissionDetail.ReadOnly = true;
		this.txtamountAfterCommissionDetail.Size = new System.Drawing.Size(125, 23);
		this.txtamountAfterCommissionDetail.TabIndex = 13;
		this.txtamountAfterCommissionDetail.Tag = "double";
		this.txtamountAfterCommissionDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(335, 55);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(108, 15);
		this.label13.TabIndex = 197;
		this.label13.Text = "Amount After Com";
		this.txtCostAfterProcessingDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCostAfterProcessingDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostAfterProcessingDetail.Location = new System.Drawing.Point(306, 26);
		this.txtCostAfterProcessingDetail.Name = "txtCostAfterProcessingDetail";
		this.txtCostAfterProcessingDetail.ReadOnly = true;
		this.txtCostAfterProcessingDetail.Size = new System.Drawing.Size(130, 23);
		this.txtCostAfterProcessingDetail.TabIndex = 2;
		this.txtCostAfterProcessingDetail.TabStop = false;
		this.txtCostAfterProcessingDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostAfterProcessingDetail.TextChanged += new System.EventHandler(txtCostAfterProcessingDetail_TextChanged);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(0, 318);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1070, 21);
		this.panel6.TabIndex = 126;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1026, 1);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(41, 19);
		this.ctrlGrdBar3.TabIndex = 59;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 339);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1070, 189);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label36);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1070, 50);
		this.panel2.TabIndex = 2;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12.5f, System.Drawing.FontStyle.Bold);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(6, 28);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(237, 21);
		this.label36.TabIndex = 178;
		this.label36.Text = "Production Costing Report";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripButton1, this.toolStripSeparator2, this.BtnRefresh, this.btnsave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.BtnPrintDropDown, this.btnShortCutKey });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1070, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton1.Click += new System.EventHandler(btnnew_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
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
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "Delete";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnPrintDropDown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintDropDown.Name = "BtnPrintDropDown";
		this.BtnPrintDropDown.Size = new System.Drawing.Size(122, 24);
		this.BtnPrintDropDown.Text = "921_Reports";
		this.BtnPrintDropDown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDropDowngrnWiseInput_DropDownItemClicked);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabControlMain.Controls.Add(this.tabForm);
		this.tabControlMain.Controls.Add(this.tabHistory);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1084, 563);
		this.tabControlMain.TabIndex = 1;
		this.tabControlMain.SelectedIndexChanged += new System.EventHandler(tabControlMain_SelectedIndexChanged);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Location = new System.Drawing.Point(4, 25);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1076, 534);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.tabForm.UseVisualStyleBackColor = true;
		this.tabHistory.Location = new System.Drawing.Point(4, 22);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(192, 74);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.PanelOtherForms.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.PanelOtherForms.Location = new System.Drawing.Point(0, 563);
		this.PanelOtherForms.Name = "PanelOtherForms";
		this.PanelOtherForms.Size = new System.Drawing.Size(1084, 48);
		this.PanelOtherForms.TabIndex = 174;
		this.PanelOtherForms.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1084, 611);
		base.Controls.Add(this.tabControlMain);
		base.Controls.Add(this.PanelOtherForms);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmCreateProductionCostingReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmCreateProductionCostingReport_FormClosed);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmCreateProductionCostingReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabInputWorking.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputWorking).EndInit();
		this.panel21.ResumeLayout(false);
		this.tabByProductRatio.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProductRatio).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCodeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabControlMain.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
