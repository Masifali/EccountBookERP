using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc.Approvals;

public class PurchaseInvoiceForApproval_Concrete : Form
{
	private DataTable grdlst = new DataTable();

	private DataTable dtPending = new DataTable();

	private bool AllowEditOnVoucherNoinReports;

	public int DocumentTypeId;

	private bool HasMultiCurrencyFeature;

	private bool HasSubsidiaryFeature;

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private Label label23;

	internal UltraCombo CmbSupplier;

	private Label label1;

	internal UltraCombo CmbItem;

	private Label label2;

	private DateTimePicker fromdate;

	private Label label33;

	private DateTimePicker ToDate;

	private Label label3;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo CmbIsApproved;

	private Button btnApproved;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel6;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label9;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label8;

	private TabPage tabPage4;

	private Panel panel7;

	private Panel panel5;

	private Panel panel8;

	private Label label5;

	private Panel panel9;

	private Label label10;

	private Button btnPendingForView;

	private Panel panel11;

	private Label label12;

	private Button btnApprovePendingRecord;

	private Panel panel15;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdPendingViewVoucher;

	private Panel panel17;

	private Label label13;

	internal GridEX GridDetailForPendingView;

	private ToolStripButton btnNew;

	private GroupBox groupBox1;

	private ToolStripButton btnShortCutKeys;

	private ToolStripButton btnRefresh;

	public PurchaseInvoiceForApproval_Concrete(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			HasSubsidiaryFeature = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			GetDataFromPurchaseInvoiceForComboBind();
			ApprovedStatusFill();
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				fromdate.Value = Convert.ToDateTime(RequestedFromDate);
				ToDate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataFromPurchaseInvoiceForComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtCustomer = new DataTable
			{
				Columns = { "Id", "CustomerName" }
			};
			DataTable dtItem = new DataTable
			{
				Columns = { "Id", "ItemName" }
			};
			DataTable dt = InvPurchaseInvoice.DropDownFillFromPurchaseInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Items")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbSupplier, "Id", "CustomerName", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApprovedStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApproved");
			dt.Rows.Add(2, "Approved");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApproved, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			ToDate.Value = DateTime.Now;
			((Control)(object)CmbSupplier).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((UltraGridBase)CmbIsApproved).Rows[0].Activate();
			fromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void SaleOrderForApproval_Concrete_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		try
		{
			gridHisory();
			GetPendingForViewRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void gridHisory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.BranchesId = UserAccount.BranchesId;
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			order.DocumentTypeId = DocumentTypeId;
			order.FromDate = fromdate.Value;
			order.ToDate = ToDate.Value;
			order.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			order.ItemId = Conversion.ToInt(CmbItem.Value);
			if (((Control)(object)CmbIsApproved).Text == "Approved")
			{
				order.IsApproved = true;
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApproved")
			{
				order.IsApproved = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "All")
			{
				order.ApprovedFilter = "All";
			}
			order.PendingForView = 1;
			grdlst = InvPurchaseInvoice.PurchaseInvoice_ApprovalHistory(order);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dtInvoice = new DataTable();
				dtInvoice.Columns.Add("Id", typeof(int));
				dtInvoice.Columns.Add("DocumentTypeId", typeof(int));
				dtInvoice.Columns.Add("DocNo", typeof(string));
				dtInvoice.Columns.Add("DocDate", typeof(DateTime));
				dtInvoice.Columns.Add("GlAccountId", typeof(int));
				dtInvoice.Columns.Add("SupplierId", typeof(int));
				dtInvoice.Columns.Add("SupplierName", typeof(string));
				dtInvoice.Columns.Add("TaxAccount", typeof(string));
				dtInvoice.Columns.Add("ManualBillNo", typeof(string));
				dtInvoice.Columns.Add("PaymentTerm", typeof(string));
				dtInvoice.Columns.Add("DueDays", typeof(int));
				dtInvoice.Columns.Add("DueDate", typeof(DateTime));
				dtInvoice.Columns.Add("DeliveryTerm", typeof(string));
				dtInvoice.Columns.Add("InvoiceQty", typeof(int));
				dtInvoice.Columns.Add("InvoiceWeight", typeof(decimal));
				dtInvoice.Columns.Add("CommissionAgentId", typeof(int));
				dtInvoice.Columns.Add("CommissionAgent", typeof(string));
				dtInvoice.Columns.Add("CommRate", typeof(decimal));
				dtInvoice.Columns.Add("CommAmount", typeof(decimal));
				dtInvoice.Columns.Add("TransporterId", typeof(string));
				dtInvoice.Columns.Add("Transporter", typeof(string));
				dtInvoice.Columns.Add("Frieght", typeof(decimal));
				dtInvoice.Columns.Add("BillAmount", typeof(decimal));
				dtInvoice.Columns.Add("FcyCode", typeof(string));
				dtInvoice.Columns.Add("ExchangeRate", typeof(decimal));
				dtInvoice.Columns.Add("FcyAmount", typeof(decimal));
				dtInvoice.Columns.Add("EntryDate", typeof(DateTime));
				dtInvoice.Columns.Add("EntryUser", typeof(string));
				dtInvoice.Columns.Add("ModifyDate", typeof(DateTime));
				dtInvoice.Columns.Add("ModifyUser", typeof(string));
				dtInvoice.Columns.Add("ApprovedDate", typeof(DateTime));
				dtInvoice.Columns.Add("ApprovedUser", typeof(string));
				dtInvoice.Columns.Add("NoOfAttachments", typeof(int));
				dtInvoice.Clear();
				foreach (DataRow row in grdlst.Rows)
				{
					if (!dtInvoice.AsEnumerable().Any((DataRow rr) => rr.Field<int>("Id") == Conversion.ToInt(row["Id"])))
					{
						dtInvoice.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["GlAccountId"], row["SupplierCustomerId"], row["CustomerName"], row["TaxAccountTitle"], row["ManualBillNo"], row["PaymentTerm"], row["DueDays"], Conversion.ToDateTime(row["DueDate"]).ToShortDateString(), row["DeliveryTerm"], row["InvoiceQty"], row["InvoiceWeight"], row["CommissionAgentId"], row["CommissionAgent"], row["CommissionRate"], row["CommissionAmount"], row["TransporterId"], row["TransporterName"], row["FreightAmount"], row["BillAmount"], row["CurrencyNameHeader"], row["ExchangeRateHeader"], row["FcyAmountHeader"], Conversion.ToDateTime(row["EntryDate"]).ToShortDateString(), row["EntryUserName"], Conversion.ToDateTime(row["ModifyDate"]).ToShortDateString(), row["ModifyUserName"], Conversion.ToDateTime(row["ApprovedDate"]).ToShortDateString(), row["ApprovedUserName"], row["NoOfAttachments"]);
					}
				}
				grdsaleorder.DataSource = dtInvoice;
				grdsaleorder.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grdsaleorder.ClearStructure();
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["Id"].Visible = false;
			grdsaleorder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdsaleorder.RootTable.Columns["SupplierId"].Visible = false;
			grdsaleorder.RootTable.Columns["GlAccountId"].Visible = false;
			grdsaleorder.RootTable.Columns["CommissionAgentId"].Visible = false;
			grdsaleorder.RootTable.Columns["TransporterId"].Visible = false;
			grdsaleorder.RootTable.Columns["FcyCode"].Visible = HasMultiCurrencyFeature;
			grdsaleorder.RootTable.Columns["ExchangeRate"].Visible = HasMultiCurrencyFeature;
			grdsaleorder.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdsaleorder.RootTable.Columns["InvoiceQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["InvoiceQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["InvoiceQty"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["InvoiceQty"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["InvoiceWeight"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["InvoiceWeight"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["InvoiceWeight"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["InvoiceWeight"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["CommRate"].FormatString = "#,##0.####";
			grdsaleorder.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.#####";
			grdsaleorder.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Frieght"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["Frieght"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["Frieght"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["Frieght"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsaleorder.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			CommonServices.GridAutoAdjustmentNew(grdsaleorder);
			grdsaleorder.RootTable.Columns.Add("Print");
			grdsaleorder.RootTable.Columns["Print"].Caption = "Slip";
			grdsaleorder.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdsaleorder.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdsaleorder.RootTable.Columns["Print"].ButtonText = "Slip";
			grdsaleorder.RootTable.Columns["Print"].Width = 60;
			grdsaleorder.RootTable.Columns["Print"].Position = 0;
			grdsaleorder.FrozenColumns = 1;
			if (((Control)(object)CmbIsApproved).Text == "UnApproved")
			{
				grdsaleorder.RootTable.Columns.Add("IsApproved");
				grdsaleorder.RootTable.Columns["IsApproved"].Caption = "Approve";
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "Approve";
				grdsaleorder.RootTable.Columns["IsApproved"].Width = 60;
				grdsaleorder.RootTable.Columns.Add("Select");
				grdsaleorder.RootTable.Columns["Select"].ActAsSelector = true;
				grdsaleorder.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdsaleorder.RootTable.Columns["Select"].Width = 40;
				grdsaleorder.RootTable.Columns["Select"].Position = 0;
				grdsaleorder.RootTable.Columns["IsApproved"].Position = 1;
				grdsaleorder.RootTable.Columns["Print"].Position = 2;
				grdsaleorder.FrozenColumns = 3;
			}
			CommonServices.GridAutoAdjustmentNew(grdsaleorder);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				CommonServices.PurchaseInvoiceAndRetunFinishGoodsPrint1859A(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value));
			}
			else if (e.Column.Key == "IsApproved")
			{
				GridColumnApproveButtonClick();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (e.Column.Key == "SupplierName" && item != null && (int)item.RowType == 0)
			{
				int SupplierCustomerId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["SupplierId"].Value);
				int GlId = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["GlAccountId"].Value);
				if (HasSubsidiaryFeature)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, fromdate.Value, ToDate.Value, SupplierCustomerId);
				}
				else
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, fromdate.Value, ToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("DetailId", typeof(int));
					dt.Columns.Add("GrnId", typeof(int));
					dt.Columns.Add("GrnDetailId", typeof(int));
					dt.Columns.Add("GrnNo", typeof(string));
					dt.Columns.Add("OrderId", typeof(int));
					dt.Columns.Add("OrderDetailId", typeof(int));
					dt.Columns.Add("OrderNo", typeof(string));
					dt.Columns.Add("Warehouse", typeof(string));
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("PurchaseGLAC", typeof(string));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("ItemCode", typeof(string));
					dt.Columns.Add("ItemArrtibuteVarrient", typeof(string));
					dt.Columns.Add("VarientEquivalent", typeof(string));
					dt.Columns.Add("ItemQty", typeof(int));
					dt.Columns.Add("ItemWeight", typeof(decimal));
					dt.Columns.Add("ItemNetWeight", typeof(decimal));
					dt.Columns.Add("ItemRateWithOutRateCut", typeof(decimal));
					dt.Columns.Add("RateCut", typeof(decimal));
					dt.Columns.Add("NetRate", typeof(decimal));
					dt.Columns.Add("ItemAmount", typeof(decimal));
					dt.Columns.Add("TaxType", typeof(string));
					dt.Columns.Add("Tax%", typeof(decimal));
					dt.Columns.Add("TaxAmount", typeof(decimal));
					dt.Columns.Add("Expense", typeof(decimal));
					dt.Columns.Add("Commission", typeof(decimal));
					dt.Columns.Add("Freight", typeof(decimal));
					dt.Columns.Add("ItemNetAmount", typeof(decimal));
					dt.Columns.Add("FcyAmount", typeof(decimal));
					dt.Columns.Add("JobLotDescription", typeof(string));
					dt.Columns.Add("GpDate", typeof(DateTime));
					dt.Columns.Add("GpNo", typeof(string));
					dt.Columns.Add("VehicleNo", typeof(string));
					dt.Columns.Add("CityName", typeof(string));
					dt.Columns.Add("RemarksDetail", typeof(string));
					DataTable resultRows = (from row in grdlst.AsEnumerable()
						where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dt.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["InvGrnId"], resultRows.Rows[i]["InvGrnDetailId"], resultRows.Rows[i]["GrnNo"], resultRows.Rows[i]["PurchaseOrderId"], resultRows.Rows[i]["PurchaseOrderDetailId"], resultRows.Rows[i]["OrderNo"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemAttributeVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["ItemNetWeight"], resultRows.Rows[i]["ItemRateWithoutRateCut"], resultRows.Rows[i]["RateCut"], resultRows.Rows[i]["ItemRate"], resultRows.Rows[i]["ItemAmount"], resultRows.Rows[i]["TaxName"], resultRows.Rows[i]["TaxPercent"], resultRows.Rows[i]["TaxAmount"], resultRows.Rows[i]["ExpenseAmount"], resultRows.Rows[i]["CommissionAmountDetail"], resultRows.Rows[i]["FreightAmountDetail"], resultRows.Rows[i]["ItemNetAmount"], resultRows.Rows[i]["FcyAmount"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["GpDate"], resultRows.Rows[i]["GpNo"], resultRows.Rows[i]["VehicleNo"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					GridDetail.DataSource = dt;
					GridDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridDetail.RootTable.Columns["Id"].Visible = false;
			GridDetail.RootTable.Columns["DetailId"].Visible = false;
			GridDetail.RootTable.Columns["GrnId"].Visible = false;
			GridDetail.RootTable.Columns["GrnDetailId"].Visible = false;
			GridDetail.RootTable.Columns["OrderId"].Visible = false;
			GridDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			GridDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridEXRow row = GridDetail.GetRow(0);
			if (row != null && (int)row.RowType == 0)
			{
				if (Conversion.ToInt(row.Cells["GrnId"].Value) > 0 && Conversion.ToInt(row.Cells["OrderId"].Value) > 0)
				{
					GridDetail.RootTable.Columns["OrderNo"].Visible = true;
					GridDetail.RootTable.Columns["GrnNo"].Visible = true;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) > 0 && Conversion.ToInt(row.Cells["OrderId"].Value) == 0)
				{
					GridDetail.RootTable.Columns["OrderNo"].Visible = false;
					GridDetail.RootTable.Columns["GrnNo"].Visible = true;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) == 0 && Conversion.ToInt(row.Cells["OrderId"].Value) > 0)
				{
					GridDetail.RootTable.Columns["OrderNo"].Visible = true;
					GridDetail.RootTable.Columns["GrnNo"].Visible = false;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) == 0 && Conversion.ToInt(row.Cells["OrderId"].Value) == 0)
				{
					GridDetail.RootTable.Columns["OrderNo"].Visible = false;
					GridDetail.RootTable.Columns["GrnNo"].Visible = false;
				}
			}
			GridDetail.RootTable.Columns["GrnNo"].ColumnType = (ColumnType)5;
			GridDetail.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			GridDetail.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["VarientEquivalent"].FormatString = "#,##0.###";
			GridDetail.RootTable.Columns["VarientEquivalent"].TotalFormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemWeight"].FormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemNetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemNetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemNetWeight"].FormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemNetWeight"].TotalFormatString = "#,##0.###";
			GridDetail.RootTable.Columns["ItemRateWithOutRateCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemRateWithOutRateCut"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["RateCut"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["NetRate"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Tax%"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["TaxAmount"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.####";
			GridDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Expense"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["Expense"].TotalFormatString = "#,##.####";
			GridDetail.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Commission"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["Commission"].TotalFormatString = "#,##.####";
			GridDetail.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["Freight"].FormatString = "#,##0.####";
			GridDetail.RootTable.Columns["Freight"].TotalFormatString = "#,##.####";
			GridDetail.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = GridDetail.CurrentRow;
			int GrnId = Conversion.ToInt(currentRow.Cells["GrnId"].Value);
			int OrderId = Conversion.ToInt(currentRow.Cells["OrderId"].Value);
			if (e.Column.Key == "GrnNo")
			{
				CommonServices.GrnConcreteSlip(GrnId);
			}
			if (e.Column.Key == "OrderNo")
			{
				CommonServices.PurchaseOrderSlipConcrete(OrderId);
			}
			_ = e.Column.Key == "ItemName";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (tabControl2.SelectedIndex == 0)
			{
				if (grdlst.Rows.Count == 0 || grdlst == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(grdlst, "1859-PurchaseInvoice_ApprovalRegister.rpt");
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				if (dtPending.Rows.Count == 0 || dtPending == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtPending, "1859-PurchaseInvoice_ApprovalRegister.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApproved" && MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.BranchesId = UserAccount.BranchesId;
					item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.ReqType = "AP";
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvPurchaseInvoice.PurchaseInvoiceandVoucherApprove(obj);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridColumnApproveButtonClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Conversion.ToInt(grdsaleorder.CurrentRow.Cells["Id"].Value);
				item.ReqType = "AP";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvPurchaseInvoice.PurchaseInvoiceandVoucherApprove(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingForViewRecords()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtPending.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FromDate = fromdate.Value;
			obj.ToDate = ToDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemId = Conversion.ToInt(CmbItem.Value);
			if (((Control)(object)CmbIsApproved).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)CmbIsApproved).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)CmbIsApproved).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			obj.PendingForView = 2;
			dtPending = InvPurchaseInvoice.PurchaseInvoice_ApprovalHistory(obj);
			if (dtPending.Rows.Count > 0)
			{
				DataTable dtInvoice = new DataTable();
				dtInvoice.Columns.Add("Id", typeof(int));
				dtInvoice.Columns.Add("DocumentTypeId", typeof(int));
				dtInvoice.Columns.Add("DocNo", typeof(string));
				dtInvoice.Columns.Add("DocDate", typeof(DateTime));
				dtInvoice.Columns.Add("GlAccountId", typeof(int));
				dtInvoice.Columns.Add("SupplierId", typeof(int));
				dtInvoice.Columns.Add("SupplierName", typeof(string));
				dtInvoice.Columns.Add("TaxAccount", typeof(string));
				dtInvoice.Columns.Add("ManualBillNo", typeof(string));
				dtInvoice.Columns.Add("PaymentTerm", typeof(string));
				dtInvoice.Columns.Add("DueDays", typeof(int));
				dtInvoice.Columns.Add("DueDate", typeof(DateTime));
				dtInvoice.Columns.Add("DeliveryTerm", typeof(string));
				dtInvoice.Columns.Add("InvoiceQty", typeof(int));
				dtInvoice.Columns.Add("InvoiceWeight", typeof(decimal));
				dtInvoice.Columns.Add("CommissionAgentId", typeof(int));
				dtInvoice.Columns.Add("CommissionAgent", typeof(string));
				dtInvoice.Columns.Add("CommRate", typeof(decimal));
				dtInvoice.Columns.Add("CommAmount", typeof(decimal));
				dtInvoice.Columns.Add("TransporterId", typeof(string));
				dtInvoice.Columns.Add("Transporter", typeof(string));
				dtInvoice.Columns.Add("Frieght", typeof(decimal));
				dtInvoice.Columns.Add("BillAmount", typeof(decimal));
				dtInvoice.Columns.Add("FcyCode", typeof(string));
				dtInvoice.Columns.Add("ExchangeRate", typeof(decimal));
				dtInvoice.Columns.Add("FcyAmount", typeof(decimal));
				dtInvoice.Columns.Add("EntryDate", typeof(DateTime));
				dtInvoice.Columns.Add("EntryUser", typeof(string));
				dtInvoice.Columns.Add("ModifyDate", typeof(DateTime));
				dtInvoice.Columns.Add("ModifyUser", typeof(string));
				dtInvoice.Columns.Add("ApprovedDate", typeof(DateTime));
				dtInvoice.Columns.Add("ApprovedUser", typeof(string));
				dtInvoice.Columns.Add("NoOfAttachments", typeof(int));
				dtInvoice.Clear();
				foreach (DataRow row in dtPending.Rows)
				{
					if (!dtInvoice.AsEnumerable().Any((DataRow rr) => rr.Field<int>("Id") == Conversion.ToInt(row["Id"])))
					{
						dtInvoice.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["GlAccountId"], row["SupplierCustomerId"], row["CustomerName"], row["TaxAccountTitle"], row["ManualBillNo"], row["PaymentTerm"], row["DueDays"], Conversion.ToDateTime(row["DueDate"]).ToShortDateString(), row["DeliveryTerm"], row["InvoiceQty"], row["InvoiceWeight"], row["CommissionAgentId"], row["CommissionAgent"], row["CommissionRate"], row["CommissionAmount"], row["TransporterId"], row["TransporterName"], row["FreightAmount"], row["BillAmount"], row["CurrencyNameHeader"], row["ExchangeRateHeader"], row["FcyAmountHeader"], Conversion.ToDateTime(row["EntryDate"]).ToShortDateString(), row["EntryUserName"], Conversion.ToDateTime(row["ModifyDate"]).ToShortDateString(), row["ModifyUserName"], Conversion.ToDateTime(row["ApprovedDate"]).ToShortDateString(), row["ApprovedUserName"], row["NoOfAttachments"]);
					}
				}
				grdPendingViewVoucher.DataSource = dtInvoice;
				grdPendingViewVoucher.RetrieveStructure();
				GridSettingsForPending();
			}
			else
			{
				grdPendingViewVoucher.ClearStructure();
				GridDetailForPendingView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettingsForPending()
	{
		try
		{
			grdPendingViewVoucher.RootTable.Columns["Id"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["GlAccountId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["SupplierId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["CommissionAgentId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["TransporterId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["FcyCode"].Visible = HasMultiCurrencyFeature;
			grdPendingViewVoucher.RootTable.Columns["ExchangeRate"].Visible = HasMultiCurrencyFeature;
			grdPendingViewVoucher.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdPendingViewVoucher.RootTable.Columns["InvoiceQty"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["InvoiceQty"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["InvoiceQty"].FormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["InvoiceQty"].TotalFormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["InvoiceWeight"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["InvoiceWeight"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["InvoiceWeight"].FormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["InvoiceWeight"].TotalFormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["CommRate"].FormatString = "#,##0.###";
			grdPendingViewVoucher.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["CommAmount"].FormatString = "#,##0.####";
			grdPendingViewVoucher.RootTable.Columns["CommAmount"].TotalFormatString = "#,##0.####";
			grdPendingViewVoucher.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.#####";
			grdPendingViewVoucher.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["Frieght"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["Frieght"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["Frieght"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["Frieght"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPendingViewVoucher.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdPendingViewVoucher.RootTable.Columns["SupplierName"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdPendingViewVoucher);
			grdPendingViewVoucher.RootTable.Columns.Add("Slip");
			grdPendingViewVoucher.RootTable.Columns["Slip"].Caption = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].Width = 60;
			grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 0;
			grdPendingViewVoucher.FrozenColumns = 1;
			if (((Control)(object)CmbIsApproved).Text == "UnApproved")
			{
				grdPendingViewVoucher.RootTable.Columns.Add("Select");
				grdPendingViewVoucher.RootTable.Columns["Select"].ActAsSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].Width = 40;
				grdPendingViewVoucher.RootTable.Columns.Add("Approve");
				grdPendingViewVoucher.RootTable.Columns["Approve"].Caption = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonText = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].Width = 60;
				grdPendingViewVoucher.RootTable.Columns["Select"].Position = 0;
				grdPendingViewVoucher.RootTable.Columns["Approve"].Position = 1;
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 2;
				grdPendingViewVoucher.FrozenColumns = 3;
			}
			CommonServices.GridAutoAdjustmentNew(grdPendingViewVoucher);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grdPendingViewVoucher.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				CommonServices.PurchaseInvoiceAndRetunFinishGoodsPrint1859A(Id);
			}
			if (e.Column.Key == "Approve")
			{
				ApproveOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Column.Key == "NoOfAttachments")
			{
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
			else if (e.Column.Key == "SupplierName")
			{
				int SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierId"].Value);
				int GlId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
				if (HasSubsidiaryFeature)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, fromdate.Value, ToDate.Value, SupplierCustomerId);
				}
				else
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, fromdate.Value, ToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DetailId", typeof(int));
				dt.Columns.Add("GrnId", typeof(int));
				dt.Columns.Add("GrnDetailId", typeof(int));
				dt.Columns.Add("GrnNo", typeof(string));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderDetailId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(string));
				dt.Columns.Add("Warehouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("PurchaseGLAC", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemArrtibuteVarrient", typeof(string));
				dt.Columns.Add("VarientEquivalent", typeof(string));
				dt.Columns.Add("ItemQty", typeof(int));
				dt.Columns.Add("ItemWeight", typeof(decimal));
				dt.Columns.Add("ItemNetWeight", typeof(decimal));
				dt.Columns.Add("ItemRateWithOutRateCut", typeof(decimal));
				dt.Columns.Add("RateCut", typeof(decimal));
				dt.Columns.Add("NetRate", typeof(decimal));
				dt.Columns.Add("ItemAmount", typeof(decimal));
				dt.Columns.Add("TaxType", typeof(string));
				dt.Columns.Add("Tax%", typeof(decimal));
				dt.Columns.Add("TaxAmount", typeof(decimal));
				dt.Columns.Add("Expense", typeof(decimal));
				dt.Columns.Add("Commission", typeof(decimal));
				dt.Columns.Add("Freight", typeof(decimal));
				dt.Columns.Add("ItemNetAmount", typeof(decimal));
				dt.Columns.Add("FcyAmount", typeof(decimal));
				dt.Columns.Add("JobLotDescription", typeof(string));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpNo", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("RemarksDetail", typeof(string));
				DataTable resultRows = (from row in dtPending.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["InvGrnId"], resultRows.Rows[i]["InvGrnDetailId"], resultRows.Rows[i]["GrnNo"], resultRows.Rows[i]["PurchaseOrderId"], resultRows.Rows[i]["PurchaseOrderDetailId"], resultRows.Rows[i]["OrderNo"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["PurchaseGLAC"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemAttributeVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["ItemNetWeight"], resultRows.Rows[i]["ItemRateWithoutRateCut"], resultRows.Rows[i]["RateCut"], resultRows.Rows[i]["ItemRate"], resultRows.Rows[i]["ItemAmount"], resultRows.Rows[i]["TaxName"], resultRows.Rows[i]["TaxPercent"], resultRows.Rows[i]["TaxAmount"], resultRows.Rows[i]["ExpenseAmount"], resultRows.Rows[i]["CommissionAmountDetail"], resultRows.Rows[i]["FreightAmountDetail"], resultRows.Rows[i]["ItemNetAmount"], resultRows.Rows[i]["FcyAmount"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["GpDate"], resultRows.Rows[i]["GpNo"], resultRows.Rows[i]["VehicleNo"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
				}
				GridDetailForPendingView.DataSource = dt;
				GridDetailForPendingView.RetrieveStructure();
				GridDetailSettingForPendingRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettingForPendingRecord()
	{
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridDetailForPendingView.RootTable.Columns["Id"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["DetailId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["GrnId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["GrnDetailId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["OrderId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["OrderDetailId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			GridDetailForPendingView.RootTable.Columns["ItemId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["PurchaseGLAC"].Visible = false;
			GridEXRow row = GridDetailForPendingView.GetRow(0);
			if (row != null && (int)row.RowType == 0)
			{
				if (Conversion.ToInt(row.Cells["GrnId"].Value) > 0 && Conversion.ToInt(row.Cells["OrderId"].Value) > 0)
				{
					GridDetailForPendingView.RootTable.Columns["OrderNo"].Visible = true;
					GridDetailForPendingView.RootTable.Columns["GrnNo"].Visible = true;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) > 0 && Conversion.ToInt(row.Cells["OrderId"].Value) == 0)
				{
					GridDetailForPendingView.RootTable.Columns["OrderNo"].Visible = false;
					GridDetailForPendingView.RootTable.Columns["GrnNo"].Visible = true;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) == 0 && Conversion.ToInt(row.Cells["OrderId"].Value) > 0)
				{
					GridDetailForPendingView.RootTable.Columns["OrderNo"].Visible = true;
					GridDetailForPendingView.RootTable.Columns["GrnNo"].Visible = false;
				}
				else if (Conversion.ToInt(row.Cells["GrnId"].Value) == 0 && Conversion.ToInt(row.Cells["OrderId"].Value) == 0)
				{
					GridDetailForPendingView.RootTable.Columns["OrderNo"].Visible = false;
					GridDetailForPendingView.RootTable.Columns["GrnNo"].Visible = false;
				}
			}
			GridDetailForPendingView.RootTable.Columns["GrnNo"].ColumnType = (ColumnType)5;
			GridDetailForPendingView.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			GridDetailForPendingView.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["VarientEquivalent"].FormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["VarientEquivalent"].TotalFormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].FormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemNetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemNetWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemNetWeight"].FormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemNetWeight"].TotalFormatString = "#,##0.###";
			GridDetailForPendingView.RootTable.Columns["ItemRateWithOutRateCut"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemRateWithOutRateCut"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["RateCut"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["NetRate"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["Tax%"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["TaxAmount"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["Expense"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["Expense"].TotalFormatString = "#,##.####";
			GridDetailForPendingView.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["Commission"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["Commission"].TotalFormatString = "#,##.####";
			GridDetailForPendingView.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["Freight"].FormatString = "#,##0.####";
			GridDetailForPendingView.RootTable.Columns["Freight"].TotalFormatString = "#,##.####";
			GridDetailForPendingView.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GridDetailForPendingView.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(GridDetailForPendingView);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetailForPendingView.CurrentRow;
			int GrnId = Conversion.ToInt(item.Cells["GrnId"].Value);
			int OrderId = Conversion.ToInt(item.Cells["OrderId"].Value);
			if (e.Column.Key == "OrderNo")
			{
				CommonServices.PurchaseOrderSlipConcrete(OrderId);
			}
			if (e.Column.Key == "GrnNo")
			{
				CommonServices.GrnConcreteSlip(GrnId);
			}
			if (e.Column.Key == "ItemName")
			{
				Conversion.ToInt(item.Cells["PurchaseGLAC"].Value);
				CommonServices.GoToItemEvaluationLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["ItemId"].Value), fromdate.Value, ToDate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ApproveOrder(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Id;
				item.ReqType = "AP";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvPurchaseInvoice.PurchaseInvoiceandVoucherApprove(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
				GetPendingForViewRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApproved")
			{
				if (MessageBox.Show("Are you sure to Move Invoice in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "PendingForView";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					InvPurchaseInvoice.PurchaseInvoiceandVoucherApprove(obj);
					MessageBox.Show("Record Updated Successfully");
					gridHisory();
					GetPendingForViewRecords();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovePendingRecord_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdPendingViewVoucher.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)CmbIsApproved).Text == "UnApproved")
			{
				if (MessageBox.Show("Are you sure to Approve Invoice?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdPendingViewVoucher.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["Id"].Value);
						item.ReqType = "AP";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					InvPurchaseInvoice.PurchaseInvoiceandVoucherApprove(obj);
					MessageBox.Show("Record Approve Successfully");
					gridHisory();
					GetPendingForViewRecords();
				}
				return;
			}
			throw new Exception("Please Select UnApproved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailForPendingView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GridDetailForPendingView.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				_ = GridDetailForPendingView.CurrentColumn.Key == "ItemName";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (grdsaleorder.CurrentColumn == null)
			{
				return;
			}
			if (grdsaleorder.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdsaleorder.CurrentColumn.Key == "Print")
			{
				CommonServices.PurchaseInvoiceAndRetunFinishGoodsPrint1859A(Id);
			}
			else if (grdsaleorder.CurrentColumn.Key == "SupplierName")
			{
				int SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierId"].Value);
				int GlId = Conversion.ToInt(item.Cells["GlAccountId"].Value);
				if (HasSubsidiaryFeature)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, fromdate.Value, ToDate.Value, SupplierCustomerId);
				}
				else
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, fromdate.Value, ToDate.Value);
				}
			}
			else if (grdsaleorder.CurrentColumn.Key == "IsApproved")
			{
				GridColumnApproveButtonClick();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GridDetail).Focus();
					}
					else
					{
						((Control)(object)grdsaleorder).Focus();
					}
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGridPendingView")
				{
					((Control)(object)GridDetailForPendingView).Focus();
				}
				else
				{
					((Control)(object)grdPendingViewVoucher).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				fromdate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdPendingViewVoucher).Focus();
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grdsaleorder).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null && GridDetail.CurrentColumn.Key == "ItemName")
				{
					Conversion.ToInt(r.Cells["PurchaseGLAC"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingViewVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingViewVoucher.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingViewVoucher.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (grdPendingViewVoucher.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Slip")
			{
				CommonServices.SaleOrderSlipConcrete(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "Approve")
			{
				ApproveOrder(Id);
			}
			else if (grdPendingViewVoucher.CurrentColumn.Key == "SupplierName")
			{
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierId"].Value);
				int GlId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
				if (HasSubsidiaryFeature)
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(GlId, fromdate.Value, ToDate.Value, SupplierCustomerId);
				}
				else
				{
					CommonServices.GoToCustomerLedgerFromLinkedEvent(SupplierCustomerId, fromdate.Value, ToDate.Value);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on from Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grids ");
			dt.Rows.Add("Ctrl+ArrowRight", "For Toggle Between Tabs");
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingViewVoucher).Name);
			if (fs != null)
			{
				grdPendingViewVoucher.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			GetDataFromPurchaseInvoiceForComboBind();
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
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Expected O, but got Unknown
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Expected O, but got Unknown
		//IL_0c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9f: Expected O, but got Unknown
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Expected O, but got Unknown
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d7: Expected O, but got Unknown
		//IL_1616: Unknown result type (might be due to invalid IL or missing references)
		//IL_1620: Expected O, but got Unknown
		//IL_1644: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Expected O, but got Unknown
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.btnApproved = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.GridDetailForPendingView = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.grdPendingViewVoucher = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnApprovePendingRecord = new System.Windows.Forms.Button();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbSupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbIsApproved = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label33 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel11.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(931, 719);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 139);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(931, 580);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Controls.Add(this.ctrlGrdBar3);
		this.tabPage3.Controls.Add(this.label8);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(923, 551);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 303);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(917, 245);
		this.panel6.TabIndex = 29;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 22);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(917, 223);
		this.panel7.TabIndex = 28;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(917, 223);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 0;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		this.GridDetail.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(875, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 22);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(917, 22);
		this.label9.TabIndex = 27;
		this.label9.Text = "Detail Of the Above Selected row";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.panel9);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(917, 278);
		this.panel5.TabIndex = 28;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 35);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(917, 243);
		this.panel9.TabIndex = 2;
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxVisible = false;
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(917, 243);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Tag = "MasterGrid";
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		this.grdsaleorder.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.SelectionChanged += new System.EventHandler(grdsaleorder_SelectionChanged);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.btnPendingForView);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.btnApproved);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(917, 35);
		this.panel8.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.DarkBlue;
		this.label10.Location = new System.Drawing.Point(437, 9);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(315, 17);
		this.label10.TabIndex = 143;
		this.label10.Text = "Click To Transfer Selected rows In Pending for View";
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(757, 4);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(134, 26);
		this.btnPendingForView.TabIndex = 1;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.DarkBlue;
		this.label5.Location = new System.Drawing.Point(15, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(222, 17);
		this.label5.TabIndex = 134;
		this.label5.Text = "Click To Approve The Selected rows";
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(242, 4);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(68, 26);
		this.btnApproved.TabIndex = 0;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(877, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(917, 22);
		this.label8.TabIndex = 26;
		this.label8.Text = "Main Information";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.tabPage4.Controls.Add(this.panel17);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.tabPage4.Controls.Add(this.panel11);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(923, 551);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Pending For View";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.GridDetailForPendingView.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailForPendingView).BackColor = System.Drawing.Color.White;
		this.GridDetailForPendingView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetailForPendingView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailForPendingView.DesignTimeLayout = GridDetailForPendingView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailForPendingView.FilterMode = (FilterMode)2;
		this.GridDetailForPendingView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailForPendingView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Location = new System.Drawing.Point(3, 353);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Name = "GridDetailForPendingView";
		((ScrollableBase)this.GridDetailForPendingView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(917, 195);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 10;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label13);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 330);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(917, 23);
		this.panel17.TabIndex = 9;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(234, 17);
		this.label13.TabIndex = 22;
		this.label13.Text = "Detail Of the Above Selected row";
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingViewVoucher).BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(3, 62);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(917, 268);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Tag = "MasterGridPendingView";
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel11.Controls.Add(this.label12);
		this.panel11.Controls.Add(this.btnApprovePendingRecord);
		this.panel11.Controls.Add(this.panel15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(917, 59);
		this.panel11.TabIndex = 7;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.DarkBlue;
		this.label12.Location = new System.Drawing.Point(3, 35);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(222, 17);
		this.label12.TabIndex = 138;
		this.label12.Text = "Click To Approve The Selected rows";
		this.btnApprovePendingRecord.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovePendingRecord.FlatAppearance.BorderSize = 0;
		this.btnApprovePendingRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovePendingRecord.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovePendingRecord.ForeColor = System.Drawing.Color.White;
		this.btnApprovePendingRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovePendingRecord.Location = new System.Drawing.Point(233, 30);
		this.btnApprovePendingRecord.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovePendingRecord.Name = "btnApprovePendingRecord";
		this.btnApprovePendingRecord.Size = new System.Drawing.Size(83, 26);
		this.btnApprovePendingRecord.TabIndex = 0;
		this.btnApprovePendingRecord.Text = "Approve";
		this.btnApprovePendingRecord.UseVisualStyleBackColor = false;
		this.btnApprovePendingRecord.Click += new System.EventHandler(btnApprovePendingRecord_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label11);
		this.panel15.Controls.Add(this.ctrlGrdBar4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(917, 28);
		this.panel15.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(128, 17);
		this.label11.TabIndex = 2;
		this.label11.Text = "Main Information";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(875, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdPendingViewVoucher;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(931, 86);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.ToDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(850, 73);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(247, 39);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(247, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 123;
		this.label1.Text = "Supplier Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(459, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Item Name";
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(655, 38);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 4;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(459, 39);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 3;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(773, 38);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 26);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(6, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 127;
		this.label33.Text = "From Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(655, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(40, 15);
		this.label6.TabIndex = 135;
		this.label6.Text = "Status";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(6, 41);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(119, 23);
		this.fromdate.TabIndex = 0;
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(126, 41);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(119, 23);
		this.ToDate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(126, 21);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 129;
		this.label3.Text = "To Date";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(931, 53);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 30);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(225, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Purchase Invoice Approval";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.print, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(931, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(127, 24);
		this.print.Text = "1859-Approval";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.ToolTipText = "ShortCutKeys";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(931, 719);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceForApproval_Concrete";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SaleOrderForApproval_Concrete_FormClosed);
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
