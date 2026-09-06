// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Cmagt;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.CommissionAgent
{

public class CommissionAgentTrade_Register : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private bool forminitiliazied;

	private DataTable dtDbCall = new DataTable();

	private DataTable gf = new DataTable();

	private DataTable dtDocumentWiseProfitAndLoss = new DataTable();

	private DataTable dtVehicleWiseProfitAndLoss = new DataTable();

	private DataTable dtTradingRegister = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	private IContainer components;

	private Panel panel2;

	internal GridEX grdfrm;

	internal UltraCombo cmbGlAccount;

	private Panel panel5;

	private Label Item;

	private Button btnshow;

	internal UltraCombo cmbDelivery;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	internal UltraCombo cmbpayment;

	private Label label2;

	private Label label1;

	private Label label3;

	private Label label6;

	private Label label8;

	internal UltraCombo cmbcustomer;

	private Label label7;

	internal UltraCombo cmbsupplier;

	private Panel panel3;

	private Label label23;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcutkeys;

	private Label label9;

	internal UltraCombo cmbDateType;

	private GroupBox groupBox1;

	private Panel panel4;

	private Label label10;

	private Label label15;

	internal UltraCombo cmbvehicleno;

	private Label label14;

	internal UltraCombo cmbbroker;

	private Label label13;

	internal UltraCombo cmbcommagent;

	private Label label12;

	internal UltraCombo cmbitem;

	private Label label11;

	internal UltraCombo cmbitemtype;

	private Label label5;

	internal UltraCombo cmbitemcat;

	private Label label4;

	internal UltraCombo cmbparentcat;

	private RadioButton rdpurchase;

	private RadioButton raforsale;

	private Label cmbActivity;

	internal UltraCombo cmbReportType;

	private RadioButton rdAll;

	private ToolStripDropDownButton tsDrowdownCmgtBill;

	public UltraCombo CmbIsApproved;

	private Label label16;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label46;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private RadioButton drdocdate;

	private Panel panel6;

	public CommissionAgentTrade_Register(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void InventoryStockEvaluation_GenrateStocks_Load(object sender, EventArgs e)
	{
		try
		{
			Datetypefill();
			cmbDateType.Focus();
			ComboFill();
			ReportTypeFill();
			StatusFill();
			ReportsLoad();
			forminitiliazied = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("Detail", "Detail");
			dtreporttype.Rows.Add("DocumentWiseProfitAndLoss", "DocumentWiseProfitAndLoss");
			dtreporttype.Rows.Add("PurchaseSaleRegister", "PurchaseSaleRegister");
			DDL.BindDDLNew(dtreporttype, cmbReportType, "Id", "name", "Report Type", false);
			((UltraGridBase)cmbReportType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
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
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			InfragisticsHelper.BindComboDateType(cmbDateType);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
			((UltraGridBase)cmbDateType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id");
			dtSupplier.Columns.Add("name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("name");
			DataTable dtglaccount = new DataTable();
			dtglaccount.Columns.Add("Id");
			dtglaccount.Columns.Add("name");
			DataTable dtpayment = new DataTable();
			dtpayment.Columns.Add("Id");
			dtpayment.Columns.Add("name");
			DataTable dtDelivery = new DataTable();
			dtDelivery.Columns.Add("Id");
			dtDelivery.Columns.Add("name");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("name");
			DataTable dtparent = new DataTable();
			dtparent.Columns.Add("Id");
			dtparent.Columns.Add("name");
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id");
			dtItemCategory.Columns.Add("name");
			DataTable dtitemtype = new DataTable();
			dtitemtype.Columns.Add("Id");
			dtitemtype.Columns.Add("name");
			DataTable dtcommagent = new DataTable();
			dtcommagent.Columns.Add("Id");
			dtcommagent.Columns.Add("name");
			DataTable dtBroker = new DataTable();
			dtBroker.Columns.Add("Id");
			dtBroker.Columns.Add("name");
			DataTable dtVehicle = new DataTable();
			dtVehicle.Columns.Add("Id");
			dtVehicle.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = InvCommAgentTradeBill.GetDataForDropDownFromCommOrder(obj);
			if (dtBindCombo.Rows.Count > 0)
			{
				Dictionary<string, DataTable> activityMap = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase)
				{
					{ "Supplier", dtSupplier },
					{ "Customer", dtCustomer },
					{ "TradingAccount", dtglaccount },
					{ "PaymentTerm", dtpayment },
					{ "DeliveryTerm", dtDelivery },
					{ "Item", dtitem },
					{ "ParentCategory", dtparent },
					{ "ItemCategory", dtItemCategory },
					{ "ItemType", dtitemtype },
					{ "CommissionAgent", dtcommagent },
					{ "Broker", dtBroker },
					{ "VehicleNo", dtVehicle }
				};
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					string activity = Conversion.ToString(dtBindCombo.Rows[i]["Activity"]);
					if (activityMap.TryGetValue(activity, out var dt))
					{
						dt.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbsupplier, "Id", "name", "Supplier", ZeroIndex: false);
				((UltraGridBase)cmbsupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbcustomer, "Id", "name", "Customer", ZeroIndex: false);
				((UltraGridBase)cmbcustomer).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtglaccount.Rows.Count > 0)
			{
				DDL.BindDDL(dtglaccount, cmbGlAccount, "Id", "name", "Gl Account", ZeroIndex: false);
				((UltraGridBase)cmbGlAccount).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtpayment.Rows.Count > 0)
			{
				DDL.BindDDL(dtpayment, cmbpayment, "Id", "name", "Payment Term", ZeroIndex: false);
				((UltraGridBase)cmbpayment).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtDelivery.Rows.Count > 0)
			{
				DDL.BindDDL(dtDelivery, cmbDelivery, "Id", "name", "Delivery Term", ZeroIndex: false);
				((UltraGridBase)cmbDelivery).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, cmbitem, "Id", "name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbitem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtparent.Rows.Count > 0)
			{
				DDL.BindDDL(dtparent, cmbparentcat, "Id", "name", "Parent category", ZeroIndex: false);
				((UltraGridBase)cmbparentcat).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCategory, cmbitemcat, "Id", "name", "Item Category", ZeroIndex: false);
				((UltraGridBase)cmbitemcat).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtitemtype.Rows.Count > 0)
			{
				DDL.BindDDL(dtitemtype, cmbitemtype, "Id", "name", "Item type", ZeroIndex: false);
				((UltraGridBase)cmbitemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtcommagent.Rows.Count > 0)
			{
				DDL.BindDDL(dtcommagent, cmbcommagent, "Id", "name", "Comm. Agent", ZeroIndex: false);
			}
			if (dtBroker.Rows.Count > 0)
			{
				DDL.BindDDL(dtBroker, cmbbroker, "Id", "name", "Broker", ZeroIndex: false);
			}
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDL(dtVehicle, cmbvehicleno, "Id", "name", "Vehicle No.", ZeroIndex: false);
				((UltraGridBase)cmbvehicleno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				txtDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFrom.Value = first;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFrom.Value = first2;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				GridDbCall();
				GridBind(Conversion.ToString((object)((Control)(object)cmbReportType).Text));
				PrintButtonManage();
			}
			else
			{
				MessageBox.Show("Please Select Activity First...");
				cmbReportType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReportsParameters obj = new ReportsParameters();
		obj.OrganizationId = UserAccount.OrganizationId;
		obj.CompanyId = UserAccount.CompanyId;
		obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
		obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
		obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
		obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
		obj.ParentCategoryId = Conversion.ToInt(cmbparentcat.Value);
		obj.ItemCategoryId = Conversion.ToInt(cmbitemcat.Value);
		obj.ItemTypeId = Conversion.ToInt(cmbitemtype.Value);
		obj.ItemId = Conversion.ToInt(cmbitem.Value);
		obj.SupplierId = Conversion.ToInt(cmbsupplier.Value);
		obj.CustomerId = Conversion.ToInt(cmbcustomer.Value);
		obj.GlAccountId = Conversion.ToInt(cmbGlAccount.Value);
		obj.SubBrokerId = Conversion.ToInt(cmbbroker.Value);
		obj.PaymenetTermId = Conversion.ToInt(cmbparentcat.Value);
		obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)cmbDelivery).Text);
		obj.VehicleNo = Conversion.ToString((object)((Control)(object)cmbvehicleno).Text);
		if (string.IsNullOrEmpty(((Control)(object)CmbIsApproved).Text) || ((Control)(object)CmbIsApproved).Text == "All")
		{
			obj.ApprovedFilter = "All";
		}
		else
		{
			obj.IsApproved = ((Control)(object)CmbIsApproved).Text == "Approved" || (!(((Control)(object)CmbIsApproved).Text == "UnApproved") && obj.IsApproved);
		}
		if (drdocdate.Checked)
		{
			obj.FromDate = txtDateFrom.Value;
			obj.ToDate = txtDateTo.Value;
		}
		else if (rdentrydate.Checked)
		{
			obj.EntryDateFrom = txtDateFrom.Value;
			obj.EntryDateTo = txtDateTo.Value;
		}
		else if (rdmodifydate.Checked)
		{
			obj.ModifyDateFrom = txtDateFrom.Value;
			obj.ModifyDateTo = txtDateTo.Value;
		}
		obj.ReportType = ((Control)(object)cmbReportType).Text;
		dtDbCall = InvCommAgentTradeBill.CommissionTradeRegister(obj);
	}

	public void GridBind(string ReportType)
	{
		DataTable dataTable = dtDbCall;
		if ((dataTable != null && dataTable.Rows.Count == 0) || string.IsNullOrEmpty(ReportType))
		{
			grdfrm.DataSource = null;
		}
		switch (ReportType)
		{
		case "Detail":
			BindDetailActivity();
			break;
		case "DocumentWiseProfitAndLoss":
			BindDocumentWiseProfitAndLossActivity();
			break;
		case "VehicleWiseProfitAndLoss":
			BindVehicleWiseProfitAndLossActivity();
			break;
		case "PurchaseSaleRegister":
			BindTradingRegisterActivity();
			break;
		}
	}

	private void BindDetailActivity()
	{
		if (gf.Columns.Count == 0)
		{
			gf.Columns.Add("Id", typeof(int));
			gf.Columns.Add("DocumentTypeId", typeof(int));
			gf.Columns.Add("DocNo", typeof(int));
			gf.Columns.Add("DocDate", typeof(DateTime));
			gf.Columns.Add("SupplierName", typeof(string));
			gf.Columns.Add("CustomerName", typeof(string));
			gf.Columns.Add("TradingAccount", typeof(string));
			gf.Columns.Add("PaymentTerm", typeof(string));
			gf.Columns.Add("DeliveryTerm", typeof(string));
			gf.Columns.Add("DueDays", typeof(int));
			gf.Columns.Add("BrokeryAmount", typeof(double));
			gf.Columns.Add("ItemName", typeof(string));
			gf.Columns.Add("Crop", typeof(string));
			gf.Columns.Add("PackingType", typeof(string));
			gf.Columns.Add("PackUom", typeof(string));
			gf.Columns.Add("Qty", typeof(double));
			gf.Columns.Add("GrossWeight", typeof(double));
			gf.Columns.Add("EbCut", typeof(double));
			gf.Columns.Add("EbCutTotal", typeof(double));
			gf.Columns.Add("BillWeight", typeof(double));
			gf.Columns.Add("RatePurchase", typeof(double));
			gf.Columns.Add("RateUom", typeof(string));
			gf.Columns.Add("PurchaseAmount", typeof(double));
			gf.Columns.Add("PurchaseFreight", typeof(double));
			gf.Columns.Add("PurchaseExpenses", typeof(double));
			gf.Columns.Add("SaleEbCut", typeof(double));
			gf.Columns.Add("SaleEbCutTotal", typeof(double));
			gf.Columns.Add("AddLessSale", typeof(double));
			gf.Columns.Add("SaleBillWeight", typeof(double));
			gf.Columns.Add("RateSale", typeof(double));
			gf.Columns.Add("SaleAmount", typeof(double));
			gf.Columns.Add("SaleExpenses", typeof(double));
			gf.Columns.Add("SaleNetAmount", typeof(double));
			gf.Columns.Add("RefDoc", typeof(string));
			gf.Columns.Add("VehicleNo", typeof(string));
			gf.Columns.Add("Remarks", typeof(string));
			gf.Columns.Add("ProfitAndLoss", typeof(double));
			gf.Columns.Add("EntryDate", typeof(DateTime));
			gf.Columns.Add("EntryUser", typeof(string));
			gf.Columns.Add("ModifyDate", typeof(DateTime));
			gf.Columns.Add("ModifyUser", typeof(string));
			gf.Columns.Add("ApprovedDate", typeof(DateTime));
			gf.Columns.Add("ApprovalUser", typeof(string));
		}
		rdpurchase.Visible = true;
		raforsale.Visible = true;
		rdAll.Visible = true;
		((Control)(object)grdfrm).Name = "Detail";
		gf.Rows.Clear();
		for (int i = 0; i < dtDbCall.Rows.Count; i++)
		{
			gf.Rows.Add(Conversion.ToInt(dtDbCall.Rows[i]["Id"]), Conversion.ToInt(dtDbCall.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtDbCall.Rows[i]["DocNo"]), Conversion.ToDateTime(dtDbCall.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dtDbCall.Rows[i]["SupplierName"]), Conversion.ToString(dtDbCall.Rows[i]["CustomerName"]), Conversion.ToString(dtDbCall.Rows[i]["TradingAccount"]), Conversion.ToString(dtDbCall.Rows[i]["TermsDescription"]), Conversion.ToString(dtDbCall.Rows[i]["DeliveryTerm"]), Conversion.ToInt(dtDbCall.Rows[i]["DueDays"]), Conversion.ToDouble(dtDbCall.Rows[i]["BrokeryAmount"]), Conversion.ToString(dtDbCall.Rows[i]["ItemName"]), Conversion.ToString(dtDbCall.Rows[i]["Crop"]), Conversion.ToString(dtDbCall.Rows[i]["PackTypeDesc"]), Conversion.ToString(dtDbCall.Rows[i]["PackUom"]), Conversion.ToDouble(dtDbCall.Rows[i]["Qty"]), Conversion.ToDouble(dtDbCall.Rows[i]["GrossWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["EbCut"]), Conversion.ToDouble(dtDbCall.Rows[i]["EbCutTotal"]), Conversion.ToDouble(dtDbCall.Rows[i]["BillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["RatePurchase"]), Conversion.ToString(dtDbCall.Rows[i]["RateUom"]), Conversion.ToDouble(dtDbCall.Rows[i]["PurchaseNetAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["PurchaseFreight"]), Conversion.ToDouble(dtDbCall.Rows[i]["PurchaseExpenses"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleEbCut"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleEbCutTotal"]), Conversion.ToDouble(dtDbCall.Rows[i]["AddLessSale"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleBillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["RateSale"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleExpenses"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleNetAmount"]), Conversion.ToString(dtDbCall.Rows[i]["RefDocNo"]), Conversion.ToString(dtDbCall.Rows[i]["VehicleNo"]), Conversion.ToString(dtDbCall.Rows[i]["RemarksDetail"]), Conversion.ToDouble(dtDbCall.Rows[i]["ProfitAndLoss"]), Conversion.ToDateTime(dtDbCall.Rows[i]["entryDate"]), Conversion.ToString(dtDbCall.Rows[i]["EntryUserName"]), Conversion.ToDateTime(dtDbCall.Rows[i]["modifyDate"]), Conversion.ToString(dtDbCall.Rows[i]["ModifyUserName"]), Conversion.ToDateTime(dtDbCall.Rows[i]["approvedDate"]), Conversion.ToString(dtDbCall.Rows[i]["ApprovalUserName"]));
		}
		grdfrm.DataSource = gf;
		grdfrm.RetrieveStructure();
		Gridsetting();
	}

	private void BindDocumentWiseProfitAndLossActivity()
	{
		if (dtDocumentWiseProfitAndLoss.Columns.Count == 0)
		{
			dtDocumentWiseProfitAndLoss.Columns.Add("Id", typeof(int));
			dtDocumentWiseProfitAndLoss.Columns.Add("DocumentTypeId", typeof(int));
			dtDocumentWiseProfitAndLoss.Columns.Add("DocNo", typeof(int));
			dtDocumentWiseProfitAndLoss.Columns.Add("DocDate", typeof(DateTime));
			dtDocumentWiseProfitAndLoss.Columns.Add("SupplierName", typeof(string));
			dtDocumentWiseProfitAndLoss.Columns.Add("CustomerName", typeof(string));
			dtDocumentWiseProfitAndLoss.Columns.Add("RefDocNo", typeof(string));
			dtDocumentWiseProfitAndLoss.Columns.Add("VehicleNo", typeof(string));
			dtDocumentWiseProfitAndLoss.Columns.Add("BillWeight", typeof(double));
			dtDocumentWiseProfitAndLoss.Columns.Add("SaleBillWeight", typeof(double));
			dtDocumentWiseProfitAndLoss.Columns.Add("PurchaseNetAmount", typeof(double));
			dtDocumentWiseProfitAndLoss.Columns.Add("SaleNetAmount", typeof(double));
			dtDocumentWiseProfitAndLoss.Columns.Add("ProfitAndLoss", typeof(double));
		}
		rdpurchase.Visible = false;
		raforsale.Visible = false;
		rdAll.Visible = false;
		((Control)(object)grdfrm).Name = "DocumentWiseProfitAndLoss";
		dtDocumentWiseProfitAndLoss.Rows.Clear();
		for (int i = 0; i < dtDbCall.Rows.Count; i++)
		{
			dtDocumentWiseProfitAndLoss.Rows.Add(Conversion.ToInt(dtDbCall.Rows[i]["Id"]), Conversion.ToInt(dtDbCall.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtDbCall.Rows[i]["DocNo"]), Conversion.ToDateTime(dtDbCall.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dtDbCall.Rows[i]["SupplierName"]), Conversion.ToString(dtDbCall.Rows[i]["CustomerName"]), Conversion.ToString(dtDbCall.Rows[i]["RefDocNo"]), Conversion.ToString(dtDbCall.Rows[i]["VehicleNo"]), Conversion.ToDouble(dtDbCall.Rows[i]["BillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleBillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["PurchaseNetAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleNetAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["ProfitAndLoss"]));
		}
		grdfrm.DataSource = dtDocumentWiseProfitAndLoss;
		grdfrm.RetrieveStructure();
		Gridsetting();
	}

	private void BindVehicleWiseProfitAndLossActivity()
	{
		if (dtVehicleWiseProfitAndLoss.Columns.Count == 0)
		{
			dtVehicleWiseProfitAndLoss.Columns.Add("SupplierName", typeof(string));
			dtVehicleWiseProfitAndLoss.Columns.Add("CustomerName", typeof(string));
			dtVehicleWiseProfitAndLoss.Columns.Add("RefDocNo", typeof(string));
			dtVehicleWiseProfitAndLoss.Columns.Add("VehicleNo", typeof(string));
			dtVehicleWiseProfitAndLoss.Columns.Add("BillWeight", typeof(double));
			dtVehicleWiseProfitAndLoss.Columns.Add("SaleBillWeight", typeof(double));
			dtVehicleWiseProfitAndLoss.Columns.Add("PurchaseNetAmount", typeof(double));
			dtVehicleWiseProfitAndLoss.Columns.Add("SaleNetAmount", typeof(double));
			dtVehicleWiseProfitAndLoss.Columns.Add("ProfitAndLoss", typeof(double));
		}
		rdpurchase.Visible = false;
		raforsale.Visible = false;
		rdAll.Visible = false;
		((Control)(object)grdfrm).Name = "VehicleWiseProfitAndLoss";
		dtVehicleWiseProfitAndLoss.Rows.Clear();
		for (int i = 0; i < dtDbCall.Rows.Count; i++)
		{
			dtVehicleWiseProfitAndLoss.Rows.Add(Conversion.ToString(dtDbCall.Rows[i]["SupplierName"]), Conversion.ToString(dtDbCall.Rows[i]["CustomerName"]), Conversion.ToString(dtDbCall.Rows[i]["RefDocNo"]), Conversion.ToString(dtDbCall.Rows[i]["VehicleNo"]), Conversion.ToDouble(dtDbCall.Rows[i]["BillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleBillWeight"]), Conversion.ToDouble(dtDbCall.Rows[i]["PurchaseNetAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["SaleNetAmount"]), Conversion.ToDouble(dtDbCall.Rows[i]["ProfitAndLoss"]));
		}
		grdfrm.DataSource = dtVehicleWiseProfitAndLoss;
		grdfrm.RetrieveStructure();
		Gridsetting();
	}

	private void BindTradingRegisterActivity()
	{
		if (dtTradingRegister.Columns.Count == 0)
		{
			dtTradingRegister.Columns.Add("Id", typeof(int));
			dtTradingRegister.Columns.Add("DocumentTypeId", typeof(int));
			dtTradingRegister.Columns.Add("EntrySide", typeof(int));
			dtTradingRegister.Columns.Add("EntryType", typeof(string));
			dtTradingRegister.Columns.Add("DocDate", typeof(DateTime));
			dtTradingRegister.Columns.Add("DocNo", typeof(int));
			dtTradingRegister.Columns.Add("RefDocNo", typeof(string));
			dtTradingRegister.Columns.Add("VehicleNo", typeof(string));
			dtTradingRegister.Columns.Add("PartyName", typeof(string));
			dtTradingRegister.Columns.Add("DeliveryToPartyName", typeof(string));
			dtTradingRegister.Columns.Add("TradingAccount", typeof(string));
			dtTradingRegister.Columns.Add("PaymentTerm", typeof(string));
			dtTradingRegister.Columns.Add("DeliveryTerm", typeof(string));
			dtTradingRegister.Columns.Add("CommissionAgent", typeof(string));
			dtTradingRegister.Columns.Add("BrokerName", typeof(string));
			dtTradingRegister.Columns.Add("ItemName", typeof(string));
			dtTradingRegister.Columns.Add("Crop", typeof(string));
			dtTradingRegister.Columns.Add("PackingType", typeof(string));
			dtTradingRegister.Columns.Add("PackUom", typeof(string));
			dtTradingRegister.Columns.Add("Qty", typeof(double));
			dtTradingRegister.Columns.Add("GrossWeight", typeof(double));
			dtTradingRegister.Columns.Add("EbCut", typeof(double));
			dtTradingRegister.Columns.Add("EbCutTotal", typeof(double));
			dtTradingRegister.Columns.Add("AddLessWeight", typeof(double));
			dtTradingRegister.Columns.Add("BillWeight", typeof(double));
			dtTradingRegister.Columns.Add("Rate", typeof(double));
			dtTradingRegister.Columns.Add("RateUom", typeof(string));
			dtTradingRegister.Columns.Add("ItemAmount", typeof(double));
			dtTradingRegister.Columns.Add("FreightAmount", typeof(double));
			dtTradingRegister.Columns.Add("ExpenseAmount", typeof(double));
			dtTradingRegister.Columns.Add("ItemNetAmount", typeof(double));
			dtTradingRegister.Columns.Add("BillAmount", typeof(double));
			dtTradingRegister.Columns.Add("AddExpense", typeof(double));
			dtTradingRegister.Columns.Add("AddFreight", typeof(double));
			dtTradingRegister.Columns.Add("LessFreight", typeof(double));
			dtTradingRegister.Columns.Add("TotalValue", typeof(double));
			dtTradingRegister.Columns.Add("FreightAmountHeader", typeof(double));
			dtTradingRegister.Columns.Add("FreightAddLessHeader", typeof(double));
			dtTradingRegister.Columns.Add("FreightNetHeader", typeof(double));
			dtTradingRegister.Columns.Add("TaxAmountHeader", typeof(double));
			dtTradingRegister.Columns.Add("OtherExpenseAmount", typeof(double));
			dtTradingRegister.Columns.Add("RemarksDetail", typeof(string));
			dtTradingRegister.Columns.Add("GdnShipToParty", typeof(string));
			dtTradingRegister.Columns.Add("GdnQty", typeof(double));
			dtTradingRegister.Columns.Add("GrnQty", typeof(double));
			dtTradingRegister.Columns.Add("ProfitAndLoss", typeof(double));
			dtTradingRegister.Columns.Add("EntryUser", typeof(string));
			dtTradingRegister.Columns.Add("EntryDate", typeof(DateTime));
			dtTradingRegister.Columns.Add("ModifyUser", typeof(string));
			dtTradingRegister.Columns.Add("ModifyDate", typeof(DateTime));
		}
		rdpurchase.Visible = false;
		raforsale.Visible = false;
		rdAll.Visible = false;
		((Control)(object)grdfrm).Name = "TradingRegister";
		dtTradingRegister.Rows.Clear();
		var expenseColumnPairs = (from DataColumn c in dtDbCall.Columns
			where c.ColumnName.StartsWith("ExpenseName")
			select new
			{
				NameCol = c.ColumnName,
				AmountCol = "ExpenseAmount" + c.ColumnName.Substring("ExpenseName".Length)
			} into p
			where dtDbCall.Columns.Contains(p.AmountCol)
			select p).ToList();
		List<string> expenseNames = new List<string>();
		foreach (DataRow row in dtDbCall.Rows)
		{
			foreach (var pair2 in expenseColumnPairs)
			{
				string name3 = Conversion.ToString(row[pair2.NameCol]);
				if (!string.IsNullOrWhiteSpace(name3) && !expenseNames.Contains(name3))
				{
					expenseNames.Add(name3);
				}
			}
		}
		foreach (string name2 in expenseNames)
		{
			if (!dtTradingRegister.Columns.Contains(name2))
			{
				dtTradingRegister.Columns.Add(name2, typeof(double));
			}
		}
		for (int i = 0; i < dtDbCall.Rows.Count; i++)
		{
			DataRow dr = dtTradingRegister.NewRow();
			dr["Id"] = Conversion.ToInt(dtDbCall.Rows[i]["Id"]);
			dr["DocumentTypeId"] = Conversion.ToInt(dtDbCall.Rows[i]["DocumentTypeId"]);
			dr["EntrySide"] = Conversion.ToInt(dtDbCall.Rows[i]["EntrySide"]);
			dr["EntryType"] = Conversion.ToString(dtDbCall.Rows[i]["EntryType"]);
			dr["DocDate"] = Conversion.ToDateTime(dtDbCall.Rows[i]["DocDate"]);
			dr["DocNo"] = Conversion.ToInt(dtDbCall.Rows[i]["DocNo"]);
			dr["RefDocNo"] = Conversion.ToString(dtDbCall.Rows[i]["RefDocNo"]);
			dr["VehicleNo"] = Conversion.ToString(dtDbCall.Rows[i]["VehicleNo"]);
			dr["PartyName"] = Conversion.ToString(dtDbCall.Rows[i]["PartyName"]);
			dr["DeliveryToPartyName"] = Conversion.ToString(dtDbCall.Rows[i]["DeliveryToPartyName"]);
			dr["TradingAccount"] = Conversion.ToString(dtDbCall.Rows[i]["TradingAccount"]);
			dr["PaymentTerm"] = Conversion.ToString(dtDbCall.Rows[i]["TermsDescription"]);
			dr["DeliveryTerm"] = Conversion.ToString(dtDbCall.Rows[i]["DeliveryTerm"]);
			dr["CommissionAgent"] = Conversion.ToString(dtDbCall.Rows[i]["CommissionAgent"]);
			dr["BrokerName"] = Conversion.ToString(dtDbCall.Rows[i]["BrokerName"]);
			dr["ItemName"] = Conversion.ToString(dtDbCall.Rows[i]["ItemName"]);
			dr["Crop"] = Conversion.ToString(dtDbCall.Rows[i]["Crop"]);
			dr["PackingType"] = Conversion.ToString(dtDbCall.Rows[i]["PackTypeDesc"]);
			dr["PackUom"] = Conversion.ToString(dtDbCall.Rows[i]["PackUom"]);
			dr["Qty"] = Conversion.ToDouble(dtDbCall.Rows[i]["Qty"]);
			dr["GrossWeight"] = Conversion.ToDouble(dtDbCall.Rows[i]["GrossWeight"]);
			dr["EbCut"] = Conversion.ToDouble(dtDbCall.Rows[i]["EbCut"]);
			dr["EbCutTotal"] = Conversion.ToDouble(dtDbCall.Rows[i]["EbCutTotal"]);
			dr["AddLessWeight"] = Conversion.ToDouble(dtDbCall.Rows[i]["AddLessWeight"]);
			dr["BillWeight"] = Conversion.ToDouble(dtDbCall.Rows[i]["BillWeight"]);
			dr["Rate"] = Conversion.ToDouble(dtDbCall.Rows[i]["Rate"]);
			dr["RateUom"] = Conversion.ToString(dtDbCall.Rows[i]["RateUom"]);
			dr["ItemAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["ItemAmount"]);
			dr["FreightAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["FreightAmount"]);
			dr["ExpenseAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["ExpenseAmount"]);
			dr["ItemNetAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["ItemNetAmount"]);
			dr["BillAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["BillAmount"]);
			dr["AddExpense"] = Conversion.ToDouble(dtDbCall.Rows[i]["AddExpense"]);
			dr["AddFreight"] = Conversion.ToDouble(dtDbCall.Rows[i]["AddFreight"]);
			dr["LessFreight"] = Conversion.ToDouble(dtDbCall.Rows[i]["LessFreight"]);
			dr["TotalValue"] = Conversion.ToDouble(dtDbCall.Rows[i]["TotalValue"]);
			dr["FreightAmountHeader"] = Conversion.ToDouble(dtDbCall.Rows[i]["FreightAmountHeader"]);
			dr["FreightAddLessHeader"] = Conversion.ToDouble(dtDbCall.Rows[i]["FreightAddLessHeader"]);
			dr["FreightNetHeader"] = Conversion.ToDouble(dtDbCall.Rows[i]["FreightNetHeader"]);
			dr["TaxAmountHeader"] = Conversion.ToDouble(dtDbCall.Rows[i]["TaxAmountHeader"]);
			dr["OtherExpenseAmount"] = Conversion.ToDouble(dtDbCall.Rows[i]["OtherExpenseAmount"]);
			dr["RemarksDetail"] = Conversion.ToString(dtDbCall.Rows[i]["RemarksDetail"]);
			dr["GdnShipToParty"] = Conversion.ToString(dtDbCall.Rows[i]["GdnShipToParty"]);
			dr["GdnQty"] = Conversion.ToDouble(dtDbCall.Rows[i]["GdnQty"]);
			dr["GrnQty"] = Conversion.ToDouble(dtDbCall.Rows[i]["GrnQty"]);
			dr["ProfitAndLoss"] = Conversion.ToDouble(dtDbCall.Rows[i]["ProfitAndLoss"]);
			dr["EntryUser"] = Conversion.ToString(dtDbCall.Rows[i]["EntryUserName"]);
			dr["EntryDate"] = Conversion.ToDateTime(dtDbCall.Rows[i]["entryDate"]);
			dr["ModifyUser"] = Conversion.ToString(dtDbCall.Rows[i]["ModifyUserName"]);
			dr["ModifyDate"] = Conversion.ToDateTime(dtDbCall.Rows[i]["modifyDate"]);
			foreach (string name in expenseNames)
			{
				dr[name] = 0;
			}
			foreach (var pair in expenseColumnPairs)
			{
				string expenseName = Conversion.ToString(dtDbCall.Rows[i][pair.NameCol]);
				if (!string.IsNullOrWhiteSpace(expenseName))
				{
					dr[expenseName] = Conversion.ToDouble(dtDbCall.Rows[i][pair.AmountCol]);
				}
			}
			dtTradingRegister.Rows.Add(dr);
		}
		grdfrm.DataSource = dtTradingRegister;
		grdfrm.RetrieveStructure();
		Gridsetting();
	}

	private void Gridsetting()
	{
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		try
		{
			string Activity = Conversion.ToString((object)cmbActivity.Text);
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId" };
			if (((Control)(object)cmbReportType).Text == "Detail")
			{
				if (raforsale.Checked && rdmodifydate.Checked)
				{
					hiddenColumns.AddRange(new string[8] { "EbCut", "EbCutTotal", "BillWeight", "RatePurchase", "RateUom", "PurchaseAmount", "PurchaseFreight", "PurchaseExpenses" });
				}
				else if (rdpurchase.Checked && rdmodifydate.Checked)
				{
					hiddenColumns.AddRange(new string[8] { "SaleEbCut", "SaleEbCutTotal", "AddLessSale", "SaleBillWeight", "RateSale", "SaleAmount", "SaleExpenses", "SaleNetAmount" });
				}
				else if (raforsale.Checked && rdentrydate.Checked)
				{
					hiddenColumns.AddRange(new string[8] { "EbCut", "EbCutTotal", "BillWeight", "RatePurchase", "RateUom", "PurchaseAmount", "PurchaseFreight", "PurchaseExpenses" });
				}
				else if (rdpurchase.Checked && rdentrydate.Checked)
				{
					hiddenColumns.AddRange(new string[8] { "SaleEbCut", "SaleEbCutTotal", "AddLessSale", "SaleBillWeight", "RateSale", "SaleAmount", "SaleExpenses", "SaleNetAmount" });
				}
			}
			if (((Control)(object)cmbReportType).Text == "Detail" || ((Control)(object)cmbReportType).Text == "DocumentWiseProfitAndLoss" || ((Control)(object)cmbReportType).Text == "PurchaseSaleRegister")
			{
				grdfrm.RootTable.Columns["DocNo"].ColumnType = (ColumnType)5;
			}
			if (((Control)(object)cmbReportType).Text == "PurchaseSaleRegister")
			{
				hiddenColumns.AddRange(new string[18]
				{
					"EntrySide", "EntryType", "BrokerName", "CommissionAgent", "EbCut", "ItemAmount", "FreightAmount", "ExpenseAmount", "AddExpense", "AddFreight",
					"LessFreight", "TotalValue", "FreightAmountHeader", "FreightAddLessHeader", "RemarksDetail", "GdnShipToParty", "GdnQty", "GrnQty"
				});
				grdfrm.RootTable.Columns["FreightNetHeader"].Caption = "Freight Net Amount";
				grdfrm.RootTable.Columns["TaxAmountHeader"].Caption = "Tax Amount";
				grdfrm.RootTable.Columns["OtherExpenseAmount"].Caption = "Other Expense Amount";
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 3, 3, hiddenColumns);
			string slipname = ((Activity == "PurchaseSaleRegister") ? "1056 Slip" : "1056 Slip");
			GridEX_Helper.AddButton(grdfrm, "Print", slipname, 70, 1);
			grdfrm.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdfrm.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			if (((Control)(object)cmbReportType).Text != "Detail")
			{
				GridEXFormatCondition fc = new GridEXFormatCondition(grdfrm.RootTable.Columns["ProfitAndLoss"], (ConditionOperator)2, (object)0);
				fc.TargetColumn = grdfrm.RootTable.Columns["ProfitAndLoss"];
				fc.FormatStyle.ForeColor = Color.Red;
				fc.FormatStyle.FontBold = (TriState)2;
				grdfrm.RootTable.FormatConditions.Add(fc);
				GridEXFormatCondition fc2 = new GridEXFormatCondition(grdfrm.RootTable.Columns["ProfitAndLoss"], (ConditionOperator)3, (object)0);
				fc2.TargetColumn = grdfrm.RootTable.Columns["ProfitAndLoss"];
				fc2.FormatStyle.ForeColor = Color.Green;
				fc2.FormatStyle.FontBold = (TriState)2;
				grdfrm.RootTable.FormatConditions.Add(fc2);
			}
			grdfrm.FrozenColumns = 4;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void raforsale_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			SetColumnsOnBaseOfRadioButton();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdpurchase_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			SetColumnsOnBaseOfRadioButton();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SetColumnsOnBaseOfRadioButton()
	{
		try
		{
			if (dtDbCall.Rows.Count > 0)
			{
				if (((Control)(object)cmbReportType).Text == "Detail")
				{
					if (rdAll.Checked)
					{
						grdfrm.RootTable.Columns["SaleEbCut"].Visible = true;
						grdfrm.RootTable.Columns["SaleEbCutTotal"].Visible = true;
						grdfrm.RootTable.Columns["AddLessSale"].Visible = true;
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["RateSale"].Visible = true;
						grdfrm.RootTable.Columns["SaleAmount"].Visible = true;
						grdfrm.RootTable.Columns["SaleExpenses"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
						grdfrm.RootTable.Columns["EbCut"].Visible = true;
						grdfrm.RootTable.Columns["EbCutTotal"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["RatePurchase"].Visible = true;
						grdfrm.RootTable.Columns["RateUom"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseAmount"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseFreight"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseExpenses"].Visible = true;
					}
					else if (raforsale.Checked)
					{
						grdfrm.RootTable.Columns["RatePurchase"].Visible = false;
						grdfrm.RootTable.Columns["PurchaseAmount"].Visible = false;
						grdfrm.RootTable.Columns["PurchaseFreight"].Visible = false;
						grdfrm.RootTable.Columns["PurchaseExpenses"].Visible = false;
						grdfrm.RootTable.Columns["BillWeight"].Visible = false;
						grdfrm.RootTable.Columns["EbCutTotal"].Visible = false;
						grdfrm.RootTable.Columns["EbCut"].Visible = false;
						grdfrm.RootTable.Columns["SaleEbCut"].Visible = true;
						grdfrm.RootTable.Columns["SaleEbCutTotal"].Visible = true;
						grdfrm.RootTable.Columns["AddLessSale"].Visible = true;
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["RateSale"].Visible = true;
						grdfrm.RootTable.Columns["SaleAmount"].Visible = true;
						grdfrm.RootTable.Columns["SaleExpenses"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
					}
					else if (rdpurchase.Checked)
					{
						grdfrm.RootTable.Columns["SaleEbCut"].Visible = false;
						grdfrm.RootTable.Columns["SaleEbCutTotal"].Visible = false;
						grdfrm.RootTable.Columns["AddLessSale"].Visible = false;
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = false;
						grdfrm.RootTable.Columns["RateSale"].Visible = false;
						grdfrm.RootTable.Columns["SaleAmount"].Visible = false;
						grdfrm.RootTable.Columns["SaleExpenses"].Visible = false;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = false;
						grdfrm.RootTable.Columns["EbCut"].Visible = true;
						grdfrm.RootTable.Columns["EbCutTotal"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["RatePurchase"].Visible = true;
						grdfrm.RootTable.Columns["RateUom"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseAmount"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseFreight"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseExpenses"].Visible = true;
					}
				}
				else if (((Control)(object)cmbReportType).Text == "DocumentWiseProfitAndLoss")
				{
					if (rdAll.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
					else if (raforsale.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = false;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = false;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
					else if (rdpurchase.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = false;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = false;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
				}
				else if (((Control)(object)cmbReportType).Text == "VehicleWiseProfitAndLoss")
				{
					if (rdAll.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
					else if (raforsale.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = true;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = true;
						grdfrm.RootTable.Columns["BillWeight"].Visible = false;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = false;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
					else if (rdpurchase.Checked)
					{
						grdfrm.RootTable.Columns["SaleBillWeight"].Visible = false;
						grdfrm.RootTable.Columns["SaleNetAmount"].Visible = false;
						grdfrm.RootTable.Columns["BillWeight"].Visible = true;
						grdfrm.RootTable.Columns["ProfitAndLoss"].Visible = true;
						grdfrm.RootTable.Columns["PurchaseNetAmount"].Visible = true;
					}
				}
			}
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (!(e.Column.Key == "DocNo"))
			{
				return;
			}
			if (DocumentTypeId == 1056)
			{
				FormHelper.CanOpenForm("frmCommissionAgentTradeBillAgainstGdn", delegate
				{
					frmCommissionAgentTradeBillAgainstGdn frmCommissionAgentTradeBillAgainstGdn = new frmCommissionAgentTradeBillAgainstGdn(UserAccount);
					frmCommissionAgentTradeBillAgainstGdn.Show();
					frmCommissionAgentTradeBillAgainstGdn.ReadById(Id);
				}, UserAccount);
			}
			else
			{
				FormHelper.CanOpenForm("CommissionAgentTradeBill_162", delegate
				{
					CommissionAgentTradeBill_162 commissionAgentTradeBill_ = new CommissionAgentTradeBill_162(UserAccount);
					commissionAgentTradeBill_.Show();
					commissionAgentTradeBill_.ReadById(Id);
				}, UserAccount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Print")
				{
					OpenSlip(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			((Control)(object)cmbDelivery).Text = string.Empty;
			((Control)(object)cmbsupplier).Text = string.Empty;
			((Control)(object)cmbcustomer).Text = string.Empty;
			((Control)(object)cmbpayment).Text = string.Empty;
			((Control)(object)cmbGlAccount).Text = string.Empty;
			((Control)(object)cmbparentcat).Text = string.Empty;
			((Control)(object)cmbitemcat).Text = string.Empty;
			((Control)(object)cmbitemtype).Text = string.Empty;
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbGlAccount).Text = string.Empty;
			((Control)(object)cmbbroker).Text = string.Empty;
			((Control)(object)cmbvehicleno).Text = string.Empty;
			cmbDateType.Focus();
			Datetypefill();
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
			ComboFill();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RptGenrateStocks_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdfrm")
			{
				cmbDateType.Focus();
			}
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
			if (forminitiliazied)
			{
				string Type = Conversion.ToString((object)((Control)(object)cmbReportType).Text);
				DataTable dt = new DataTable();
				switch (Type)
				{
				case "Detail":
					dt = gf;
					break;
				case "DocumentWiseProfitAndLoss":
					dt = dtDocumentWiseProfitAndLoss;
					break;
				case "PurchaseSaleRegister":
					dt = dtTradingRegister;
					break;
				}
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdfrm, dt);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintButtonManage()
	{
		try
		{
			tsDrowdownCmgtBill.Visible = false;
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrint.Visible = true;
				if (((Control)(object)cmbReportType).Text == "Detail" && rdAll.Checked)
				{
					tsDrowdownCmgtBill.Visible = true;
					btnPrint.Text = "1560-CommissionTradeAllRpt";
				}
				else if (((Control)(object)cmbReportType).Text == "Detail" && rdpurchase.Checked)
				{
					btnPrint.Text = "1558-CommissionTradePurchaseRpt";
				}
				else if (((Control)(object)cmbReportType).Text == "Detail" && raforsale.Checked)
				{
					btnPrint.Text = "1559-CommissionTradeSaleRpt";
				}
				else if (((Control)(object)cmbReportType).Text == "DocumentWiseProfitAndLoss")
				{
					btnPrint.Text = "1561-DocumentWiseProfitAndLossAll";
				}
				else if (((Control)(object)cmbReportType).Text == "VehicleWiseProfitAndLoss" && rdAll.Checked)
				{
					btnPrint.Text = "1566-VehicleWiseProfitAndLossAll";
				}
				else if (((Control)(object)cmbReportType).Text == "VehicleWiseProfitAndLoss" && rdpurchase.Checked)
				{
					btnPrint.Text = "1564-VehicleWiseProfitAndLossPurchase";
				}
				else if (((Control)(object)cmbReportType).Text == "VehicleWiseProfitAndLoss" && raforsale.Checked)
				{
					btnPrint.Text = "1565-VehicleWiseProfitAndLossSale";
				}
				else if (((Control)(object)cmbReportType).Text == "PurchaseSaleRegister")
				{
					btnPrint.Text = "1567_CommissionAgentPurchaseSaleRegister";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDbCall == null || dtDbCall.Rows.Count == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			string reportName = btnPrint.Text + ".rpt";
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress);
			val.RptPerameter("@CompanyName", UserAccount.CompName);
			val.ShowReportWithDataTable(dtDbCall, reportName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tsDrowdownCmgtBill_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtDbCall.Rows.Count == 0 || dtDbCall == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtDbCall, e.ClickedItem.Text + ".rpt");
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
			string[] files = CommonServices.DynamicReportsLoad("CommissionAgentBill");
			if (files != null && files.Length != 0)
			{
				for (int i = 0; i < files.Length; i++)
				{
					tsDrowdownCmgtBill.DropDownItems.Add(Path.GetFileNameWithoutExtension(files[i]));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OpenSlip(int PrintId, int ReportNo = 1)
	{
		try
		{
			if (ReportNo == 1)
			{
				CrystalReportPrint_Helper.CommissionAgentBillSlip_1056(PrintId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void RptGenrateStocks_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbDateType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl + P", "For print");
			dt.Rows.Add("Ctrl+F5", "For Focus On DateType ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Expected O, but got Unknown
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Expected O, but got Unknown
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Expected O, but got Unknown
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_068a: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Expected O, but got Unknown
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a54: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Expected O, but got Unknown
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbGlAccount = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.CmbIsApproved = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.raforsale = new System.Windows.Forms.RadioButton();
		this.cmbitemtype = new UltraCombo();
		this.rdpurchase = new System.Windows.Forms.RadioButton();
		this.cmbReportType = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbsupplier = new UltraCombo();
		this.cmbpayment = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbActivity = new System.Windows.Forms.Label();
		this.cmbvehicleno = new UltraCombo();
		this.cmbparentcat = new UltraCombo();
		this.Item = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDelivery = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbcustomer = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbitemcat = new UltraCombo();
		this.cmbcommagent = new UltraCombo();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.cmbitem = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbbroker = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.cmbDateType = new UltraCombo();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tsDrowdownCmgtBill = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAccount).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpayment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicleno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDelivery).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbroker).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 225);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1304, 436);
		this.panel2.TabIndex = 9;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1304, 409);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1304, 27);
		this.panel4.TabIndex = 80;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1263, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(3, 2);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 19);
		this.label10.TabIndex = 1;
		this.label10.Text = "History";
		this.cmbGlAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbGlAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGlAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGlAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGlAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGlAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Location = new System.Drawing.Point(650, 69);
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Name = "cmbGlAccount";
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbGlAccount).TabIndex = 13;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add(this.panel3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 28);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1304, 197);
		this.panel5.TabIndex = 0;
		this.groupBox1.Controls.Add(this.panel6);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApproved);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.raforsale);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemtype);
		this.groupBox1.Controls.Add(this.rdpurchase);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpayment);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.cmbActivity);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGlAccount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicleno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentcat);
		this.groupBox1.Controls.Add(this.Item);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDelivery);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcustomer);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcat);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbroker);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtDateTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 37);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1295, 157);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.panel6.Controls.Add(this.drdocdate);
		this.panel6.Controls.Add(this.rdmodifydate);
		this.panel6.Controls.Add(this.rdentrydate);
		this.panel6.Location = new System.Drawing.Point(1182, 13);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(102, 64);
		this.panel6.TabIndex = 613;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(5, 5);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 610;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(5, 39);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 612;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(5, 22);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 611;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(172, 100);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(75, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.Tag = "int";
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.CmbIsApproved.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApproved.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApproved.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApproved.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance105).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApproved).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIsApproved.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Location = new System.Drawing.Point(950, 41);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Name = "CmbIsApproved";
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApproved).TabIndex = 17;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1124, 98);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 26);
		this.btnshow.TabIndex = 22;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(566, 75);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(66, 15);
		this.label3.TabIndex = 71;
		this.label3.Text = "Gl Account";
		this.rdAll.AutoSize = true;
		this.rdAll.Checked = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(1040, 102);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(70, 19);
		this.rdAll.TabIndex = 21;
		this.rdAll.TabStop = true;
		this.rdAll.Text = "For Both";
		this.rdAll.UseVisualStyleBackColor = true;
		this.raforsale.AutoSize = true;
		this.raforsale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.raforsale.Location = new System.Drawing.Point(965, 102);
		this.raforsale.Name = "raforsale";
		this.raforsale.Size = new System.Drawing.Size(67, 19);
		this.raforsale.TabIndex = 20;
		this.raforsale.Text = "For Sale";
		this.raforsale.UseVisualStyleBackColor = true;
		this.raforsale.CheckedChanged += new System.EventHandler(raforsale_CheckedChanged);
		this.cmbitemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbitemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Location = new System.Drawing.Point(331, 41);
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Name = "cmbitemtype";
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemtype).TabIndex = 7;
		this.rdpurchase.AutoSize = true;
		this.rdpurchase.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpurchase.Location = new System.Drawing.Point(875, 102);
		this.rdpurchase.Name = "rdpurchase";
		this.rdpurchase.Size = new System.Drawing.Size(93, 19);
		this.rdpurchase.TabIndex = 19;
		this.rdpurchase.Text = "For Purchase";
		this.rdpurchase.UseVisualStyleBackColor = true;
		this.rdpurchase.CheckedChanged += new System.EventHandler(rdpurchase_CheckedChanged);
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(950, 69);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 18;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(875, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(64, 15);
		this.label15.TabIndex = 136;
		this.label15.Text = "Vehicle No";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label16.Location = new System.Drawing.Point(875, 39);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(73, 30);
		this.label16.TabIndex = 521;
		this.label16.Text = "Approval      \r\nStatus        ";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(650, 13);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 11;
		this.cmbpayment.AllowNull = (DefaultableBoolean)1;
		this.cmbpayment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpayment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpayment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbpayment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpayment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbpayment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpayment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpayment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbpayment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpayment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpayment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpayment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbpayment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpayment).Location = new System.Drawing.Point(650, 98);
		((System.Windows.Forms.Control)(object)this.cmbpayment).Name = "cmbpayment";
		((System.Windows.Forms.Control)(object)this.cmbpayment).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbpayment).TabIndex = 14;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(566, 104);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(84, 15);
		this.label6.TabIndex = 74;
		this.label6.Text = "Payment Term";
		this.cmbActivity.AutoSize = true;
		this.cmbActivity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbActivity.Location = new System.Drawing.Point(875, 75);
		this.cmbActivity.Name = "cmbActivity";
		this.cmbActivity.Size = new System.Drawing.Size(70, 15);
		this.cmbActivity.TabIndex = 138;
		this.cmbActivity.Text = "Report Type";
		this.cmbvehicleno.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicleno.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicleno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicleno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicleno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicleno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicleno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicleno).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicleno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicleno).Location = new System.Drawing.Point(950, 13);
		((System.Windows.Forms.Control)(object)this.cmbvehicleno).Name = "cmbvehicleno";
		((System.Windows.Forms.Control)(object)this.cmbvehicleno).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicleno).TabIndex = 16;
		this.cmbparentcat.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcat.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentcat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Location = new System.Drawing.Point(95, 125);
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Name = "cmbparentcat";
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbparentcat).TabIndex = 5;
		this.Item.AutoSize = true;
		this.Item.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Item.Location = new System.Drawing.Point(566, 131);
		this.Item.Name = "Item";
		this.Item.Size = new System.Drawing.Size(80, 15);
		this.Item.TabIndex = 67;
		this.Item.Text = "Delivery Term";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(566, 19);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 15);
		this.label7.TabIndex = 76;
		this.label7.Text = "Supplier";
		this.cmbDelivery.AllowNull = (DefaultableBoolean)1;
		this.cmbDelivery.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDelivery.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDelivery.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDelivery).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbDelivery).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDelivery).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDelivery).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDelivery.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDelivery).Location = new System.Drawing.Point(650, 125);
		((System.Windows.Forms.Control)(object)this.cmbDelivery).Name = "cmbDelivery";
		((System.Windows.Forms.Control)(object)this.cmbDelivery).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbDelivery).TabIndex = 15;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 131);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 124;
		this.label4.Text = "Parent Category";
		this.cmbcustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbcustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Location = new System.Drawing.Point(650, 41);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Name = "cmbcustomer";
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).TabIndex = 12;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(566, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 78;
		this.label8.Text = "Customer";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(250, 75);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 130;
		this.label12.Text = "Item Name";
		this.cmbitemcat.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcat.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Location = new System.Drawing.Point(331, 13);
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Name = "cmbitemcat";
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcat).TabIndex = 6;
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(331, 98);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 9;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(95, 100);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(75, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.Tag = "int";
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(331, 69);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(250, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(82, 15);
		this.label5.TabIndex = 126;
		this.label5.Text = "Item Category";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(4, 104);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(95, 15);
		this.label46.TabIndex = 600;
		this.label46.Text = "Doc No From,To";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(250, 47);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 128;
		this.label11.Text = "Item Type";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(250, 131);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(41, 15);
		this.label14.TabIndex = 134;
		this.label14.Text = "Broker";
		this.cmbbroker.AllowNull = (DefaultableBoolean)1;
		this.cmbbroker.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbroker.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbroker.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbbroker).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbroker).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbroker).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbroker).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbbroker).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbroker).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbbroker).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbroker).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbroker).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbbroker).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbroker).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbroker).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbroker).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbroker.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbroker).Location = new System.Drawing.Point(331, 125);
		((System.Windows.Forms.Control)(object)this.cmbbroker).Name = "cmbbroker";
		((System.Windows.Forms.Control)(object)this.cmbbroker).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbbroker).TabIndex = 10;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(250, 104);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(81, 15);
		this.label13.TabIndex = 132;
		this.label13.Text = "Comm. Agent";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(60, 15);
		this.label9.TabIndex = 122;
		this.label9.Text = "Date Type";
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(95, 71);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(152, 23);
		this.txtDateTo.TabIndex = 2;
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(95, 13);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(95, 43);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(152, 23);
		this.txtDateFrom.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 69;
		this.label1.Text = "Date From";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 70;
		this.label2.Text = "Date To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1304, 32);
		this.panel3.TabIndex = 79;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(287, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Commission Agent Trade Register";
		this.panel1.BackColor = System.Drawing.SystemColors.Control;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1304, 28);
		this.panel1.TabIndex = 6;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnNew, this.btnRefresh, this.btnPrint, this.tsDrowdownCmgtBill, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1304, 28);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 25);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 25);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(84, 25);
		this.btnPrint.Text = "Register";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tsDrowdownCmgtBill.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDrowdownCmgtBill.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDrowdownCmgtBill.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDrowdownCmgtBill.Name = "tsDrowdownCmgtBill";
		this.tsDrowdownCmgtBill.Size = new System.Drawing.Size(70, 25);
		this.tsDrowdownCmgtBill.Text = "Print";
		this.tsDrowdownCmgtBill.Visible = false;
		this.tsDrowdownCmgtBill.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDrowdownCmgtBill_DropDownItemClicked);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 25);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1304, 661);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "CommissionAgentTrade_Register";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(RptGenrateStocks_FormClosed);
		base.Load += new System.EventHandler(InventoryStockEvaluation_GenrateStocks_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(RptGenrateStocks_KeyDown);
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbGlAccount).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApproved).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpayment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicleno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDelivery).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbroker).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
