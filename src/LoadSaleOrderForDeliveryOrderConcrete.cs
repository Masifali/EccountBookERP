using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.pcc.Sale
{

public class LoadSaleOrderForDeliveryOrderConcrete : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public DataTable dtLoader = new DataTable();

	public DataTable dtGrid = new DataTable();

	public DataTable dtBindCombo = new DataTable();

	private string HeaderorderIds = "";

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grdMain;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Button btnReset;

	private Label label4;

	private TextBox txtOrderNoFrom;

	private TextBox txtOrderNoTo;

	private GroupBox groupBox1;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	internal UltraCombo cmbItem;

	internal UltraCombo cmbItemtype;

	private Label label5;

	internal UltraCombo cmbCategory;

	private Label label8;

	internal UltraCombo cmbParentCategory;

	private Label label6;

	private Label label3;

	internal GridEX grdDetail;

	private Panel panel4;

	private GroupBox groupBox3;

	private Label label7;

	private Label label12;

	private Label label11;

	private Label label10;

	private Label label24;

	private Label label25;

	private TextBox txtBalanceAmountHeader;

	private TextBox txtBalanceWeightHeadrer;

	private TextBox txtBalanceQtyHeader;

	private TextBox txtDispatchAmountHeader;

	private TextBox txtDispatchWeightHeader;

	private TextBox txtDispatchQtyHeader;

	private TextBox txtOrderAmountHeader;

	private TextBox txtQrderWeightHeader;

	private TextBox txtOrderQtyHeader;

	public LoadSaleOrderForDeliveryOrderConcrete(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			dtLoader.Columns.Add("Id", typeof(int));
			dtLoader.Columns.Add("DetailId", typeof(int));
			dtLoader.Columns.Add("DocumentTypeId", typeof(int));
			dtLoader.Columns.Add("DocNo", typeof(string));
			dtLoader.Columns.Add("DocDate", typeof(DateTime));
			dtLoader.Columns.Add("SupplierCustomerId", typeof(int));
			dtLoader.Columns.Add("RefrenenceNo", typeof(string));
			dtLoader.Columns.Add("Distance", typeof(string));
			dtLoader.Columns.Add("VisitedById", typeof(int));
			dtLoader.Columns.Add("PaymentTermsId", typeof(int));
			dtLoader.Columns.Add("OrderDueDays", typeof(int));
			dtLoader.Columns.Add("OrderDueDate", typeof(DateTime));
			dtLoader.Columns.Add("OrderExpiryDate", typeof(DateTime));
			dtLoader.Columns.Add("DeliveryTerm", typeof(string));
			dtLoader.Columns.Add("DeliveryStartDate", typeof(DateTime));
			dtLoader.Columns.Add("DeliveryDays", typeof(int));
			dtLoader.Columns.Add("ReferencePartyId", typeof(int));
			dtLoader.Columns.Add("ReferencPartyName", typeof(string));
			dtLoader.Columns.Add("ReferencPartyAddress", typeof(string));
			dtLoader.Columns.Add("ReferencPartyCellNo", typeof(string));
			dtLoader.Columns.Add("BuildingStoreyId", typeof(int));
			dtLoader.Columns.Add("BuildingStorey", typeof(string));
			dtLoader.Columns.Add("BuildingHeightId", typeof(int));
			dtLoader.Columns.Add("BuildingHeight", typeof(string));
			dtLoader.Columns.Add("BuildingArea", typeof(string));
			dtLoader.Columns.Add("RemarksHeader", typeof(string));
			dtLoader.Columns.Add("CommissionAgentId", typeof(int));
			dtLoader.Columns.Add("RefSalesManId", typeof(int));
			dtLoader.Columns.Add("OrderQty", typeof(decimal));
			dtLoader.Columns.Add("OrderWeight", typeof(decimal));
			dtLoader.Columns.Add("OrderItemId", typeof(int));
			dtLoader.Columns.Add("SaleGLAC", typeof(int));
			dtLoader.Columns.Add("ItemCode", typeof(string));
			dtLoader.Columns.Add("ItemName", typeof(string));
			dtLoader.Columns.Add("ItemAttributeVarientId", typeof(int));
			dtLoader.Columns.Add("ItemArrtibuteVarient", typeof(string));
			dtLoader.Columns.Add("VarientEquivalent", typeof(decimal));
			dtLoader.Columns.Add("JobLotId", typeof(int));
			dtLoader.Columns.Add("JobLotDescription", typeof(string));
			dtLoader.Columns.Add("ItemQty", typeof(decimal));
			dtLoader.Columns.Add("DispatchQty", typeof(decimal));
			dtLoader.Columns.Add("BalanceQty", typeof(decimal));
			dtLoader.Columns.Add("ItemWeight", typeof(decimal));
			dtLoader.Columns.Add("NetWeight", typeof(decimal));
			dtLoader.Columns.Add("DispatchWeight", typeof(decimal));
			dtLoader.Columns.Add("BalanceWeight", typeof(decimal));
			dtLoader.Columns.Add("CityId", typeof(int));
			dtLoader.Columns.Add("CityName", typeof(string));
			dtLoader.Columns.Add("RemarksDetail", typeof(string));
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			StockComboFill();
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockComboFill()
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		try
		{
			DataTable dtItemType = new DataTable();
			dtItemType.Columns.Add("Id");
			dtItemType.Columns.Add("name");
			DataTable dtItemCategories = new DataTable();
			dtItemCategories.Columns.Add("Id");
			dtItemCategories.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtSupplieerCustomer = new DataTable();
			dtSupplieerCustomer.Columns.Add("Id");
			dtSupplieerCustomer.Columns.Add("name");
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id");
			dtParentCategory.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = StocksReport.StockReportComboByOrganizationAndCompanyId(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemTypes")
				{
					dtItemType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ItemCategories")
				{
					dtItemCategories.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Items")
				{
					dtItem.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "Supplier_Customer")
				{
					dtSupplieerCustomer.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
				{
					dtParentCategory.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
			}
			if (dtItemType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemType, cmbItemtype, "Id", "name", "Item Type", false);
				((UltraGridBase)cmbItemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItem, "Id", "name", "Item", false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCategories.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategories, cmbCategory, "Id", "name", "Item Categories", false);
				((UltraGridBase)cmbCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtSupplieerCustomer.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplieerCustomer, CmbSupplier, "Id", "name", "PartyName", false);
				((UltraGridBase)CmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtParentCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtParentCategory, cmbParentCategory, "Id", "name", "Parent Category", false);
				((UltraGridBase)cmbParentCategory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
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
			((Control)(object)CmbSupplier).Text = string.Empty;
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtLoader = null;
		Hide();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			StockComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingPurchaseOrderRegularForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PendingPurchaseOrderRegularForLoad()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			obj.DocumentTypeId = 1852;
			obj.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemtype.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbParentCategory.Value);
			dtGrid = SaleOrder.LoadSaleOrderOnInvoiceForConcrete(obj);
			if (dtGrid.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DocNo", typeof(string));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("CustomerName", typeof(string));
			dt.Columns.Add("RefrenenceNo", typeof(string));
			dt.Columns.Add("Distance", typeof(string));
			dt.Columns.Add("VisitedByName", typeof(string));
			dt.Columns.Add("ReferencePartyId", typeof(int));
			dt.Columns.Add("ReferenceParty", typeof(string));
			dt.Columns.Add("RefPartyCellNo", typeof(string));
			dt.Columns.Add("BuildingStoreyId", typeof(int));
			dt.Columns.Add("BuildingStorey", typeof(string));
			dt.Columns.Add("BuildingHeightId", typeof(int));
			dt.Columns.Add("BuildingHeight", typeof(string));
			dt.Columns.Add("BuildingArea", typeof(string));
			dt.Columns.Add("CommissionAgent", typeof(string));
			dt.Columns.Add("RefSalesMan", typeof(string));
			dt.Columns.Add("CommAmount", typeof(decimal));
			dt.Columns.Add("PaymentTerm", typeof(string));
			dt.Columns.Add("OrderDueDays", typeof(int));
			dt.Columns.Add("OrderDueDate", typeof(DateTime));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
			dt.Columns.Add("TransporterName", typeof(string));
			dt.Columns.Add("FrieghtAmountHeader", typeof(decimal));
			dt.Columns.Add("WagesAccount", typeof(string));
			dt.Columns.Add("WagesAmountHeader", typeof(decimal));
			dt.Columns.Add("Status", typeof(string));
			dt.Columns.Add("IsApproved", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("EntryUserName", typeof(string));
			dt.Columns.Add("ModifyDate", typeof(DateTime));
			dt.Columns.Add("ModifyUserName", typeof(string));
			dt.Columns.Add("ApprovedDate", typeof(DateTime));
			dt.Columns.Add("ApprovedUserName", typeof(string));
			dt.Columns.Add("RemarksHeader", typeof(string));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			dt.Columns.Add("OrderQtyHeader", typeof(decimal));
			dt.Columns.Add("OrderWeightHeader", typeof(decimal));
			dt.Columns.Add("OrderAmountHeader", typeof(decimal));
			dt.Columns.Add("DispatchQtyHeader", typeof(decimal));
			dt.Columns.Add("DispatchWeightHeader", typeof(decimal));
			dt.Columns.Add("DispatchAmountHeader", typeof(decimal));
			dt.Columns.Add("BalanceQtyHeader", typeof(decimal));
			dt.Columns.Add("BalanceWeightHeader", typeof(decimal));
			dt.Columns.Add("BalanceAmountHeader", typeof(decimal));
			foreach (DataRow row in dtGrid.Rows)
			{
				if (!dt.AsEnumerable().Any((DataRow r) => r.Field<int>("Id") == Conversion.ToInt(row["Id"])))
				{
					dt.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["CustomerName"], row["RefrenenceNo"], row["Distance"], row["VisitedByName"], row["ReferencePartyId"], row["ReferencPartyName"], row["ReferencPartyCellNo"], row["BuildingStoreyId"], row["BuildingStorey"], row["BuildingHeightId"], row["BuildingHeight"], row["BuildingArea"], row["CommissionAgent"], row["RefSalesMan"], row["CommissionAmount"], row["TermsDescription"], row["OrderDueDays"], row["OrderDueDate"], row["DeliveryTerm"], row["DeliveryStartDate"], row["TransporterName"], row["FrieghtAmountHeader"], row["OtherWagesAccount"], row["OtherWagesHeader"], row["OrderStatus"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["RemarksHeader"], row["NoOfAttachments"], row["OrderQtyHeader"], row["OrderWeightHeader"], row["OrderAmountHeader"], row["DispatchQtyHeader"], row["DispatchWeightHeader"], row["DispatchAmountHeader"], row["BalanceQtyHeader"], row["BalanceWeightHeader"], row["BalanceAmountHeader"]);
				}
			}
			grdMain.DataSource = dt;
			grdMain.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotal()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				decimal OrderQty = default(decimal);
				decimal OrderWeight = default(decimal);
				decimal OrderAmount = default(decimal);
				decimal DispatchQty = default(decimal);
				decimal DispatchWeight = default(decimal);
				decimal DispatchAmount = default(decimal);
				decimal BalQty = default(decimal);
				decimal BalWeight = default(decimal);
				decimal BalAmount = default(decimal);
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id != 0)
				{
					DataRow matchingRow = dtGrid.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["Id"]) == Id);
					if (matchingRow != null)
					{
						OrderQty = Conversion.ToDecimal(matchingRow["OrderQtyHeader"]);
						OrderWeight = Conversion.ToDecimal(matchingRow["OrderWeightHeader"]);
						OrderAmount = Conversion.ToDecimal(matchingRow["OrderAmountHeader"]);
						DispatchQty = Conversion.ToDecimal(matchingRow["DispatchQtyHeader"]);
						DispatchWeight = Conversion.ToDecimal(matchingRow["DispatchWeightHeader"]);
						DispatchAmount = Conversion.ToDecimal(matchingRow["DispatchAmountHeader"]);
						BalQty = Conversion.ToDecimal(matchingRow["BalanceQtyHeader"]);
						BalWeight = Conversion.ToDecimal(matchingRow["BalanceWeightHeader"]);
						BalAmount = Conversion.ToDecimal(matchingRow["BalanceAmountHeader"]);
					}
					txtOrderQtyHeader.Text = OrderQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtQrderWeightHeader.Text = OrderWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmountHeader.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchQtyHeader.Text = DispatchQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchWeightHeader.Text = DispatchWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtDispatchAmountHeader.Text = DispatchAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceQtyHeader.Text = BalQty.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceWeightHeadrer.Text = BalWeight.ToString(clsGlobalVariables.stringFormatsingle);
					txtBalanceAmountHeader.Text = BalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtOrderQtyHeader.Text = 0.ToString();
				txtQrderWeightHeader.Text = 0.ToString();
				txtOrderAmountHeader.Text = 0.ToString();
				txtDispatchQtyHeader.Text = 0.ToString();
				txtDispatchWeightHeader.Text = 0.ToString();
				txtDispatchAmountHeader.Text = 0.ToString();
				txtBalanceQtyHeader.Text = 0.ToString();
				txtBalanceWeightHeadrer.Text = 0.ToString();
				txtBalanceAmountHeader.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdMain.RootTable.Columns["Id"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdMain.RootTable.Columns["OrderQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["OrderWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["OrderAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["DispatchAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["BalanceQtyHeader"].Visible = false;
			grdMain.RootTable.Columns["BalanceWeightHeader"].Visible = false;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["WagesAmountHeader"].Visible = false;
			grdMain.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdMain.RootTable.Columns["BuildingStoreyId"].Visible = false;
			grdMain.RootTable.Columns["BuildingHeightId"].Visible = false;
			grdMain.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdMain.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdMain.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["FrieghtAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["WagesAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["WagesAmountHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["WagesAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["WagesAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["OrderQtyHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["OrderQtyHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["OrderQtyHeader"].FormatString = "#,##0.##";
			grdMain.RootTable.Columns["OrderQtyHeader"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["OrderWeightHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["OrderWeightHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["OrderWeightHeader"].FormatString = "##,#.##";
			grdMain.RootTable.Columns["OrderWeightHeader"].TotalFormatString = "##,#.##";
			grdMain.RootTable.Columns["OrderAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["OrderAmountHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["OrderAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["OrderAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["DispatchQtyHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["DispatchQtyHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["DispatchQtyHeader"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchQtyHeader"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchWeightHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["DispatchWeightHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["DispatchWeightHeader"].FormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchWeightHeader"].TotalFormatString = "#,##0.###";
			grdMain.RootTable.Columns["DispatchAmountHeader"].AggregateFunction = (AggregateFunction)2;
			grdMain.RootTable.Columns["DispatchAmountHeader"].TextAlignment = (TextAlignment)3;
			grdMain.RootTable.Columns["DispatchAmountHeader"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdMain.RootTable.Columns["DispatchAmountHeader"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdMain);
			grdMain.RootTable.Columns.Add("Select");
			grdMain.RootTable.Columns["Select"].Key = "Select";
			grdMain.RootTable.Columns["Select"].ActAsSelector = true;
			grdMain.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdMain.RootTable.Columns["Select"].Position = 0;
			grdMain.RootTable.Columns["NoOfAttachments"].Position = 1;
			grdMain.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(string HeaderIds)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (HeaderIds != null && HeaderIds != string.Empty)
			{
				GridEXRow item = grdMain.CurrentRow;
				if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
				{
					DataTable dtOrder = new DataTable();
					dtOrder.Columns.Add("Id", typeof(int));
					dtOrder.Columns.Add("DetailId", typeof(int));
					dtOrder.Columns.Add("DocumentTypeId", typeof(int));
					dtOrder.Columns.Add("DocNo", typeof(string));
					dtOrder.Columns.Add("DocDate", typeof(DateTime));
					dtOrder.Columns.Add("SupplierCustomerId", typeof(int));
					dtOrder.Columns.Add("RefrenenceNo", typeof(string));
					dtOrder.Columns.Add("Distance", typeof(string));
					dtOrder.Columns.Add("VisitedById", typeof(int));
					dtOrder.Columns.Add("PaymentTermsId", typeof(int));
					dtOrder.Columns.Add("OrderDueDays", typeof(int));
					dtOrder.Columns.Add("OrderDueDate", typeof(DateTime));
					dtOrder.Columns.Add("OrderExpiryDate", typeof(DateTime));
					dtOrder.Columns.Add("DeliveryTerm", typeof(string));
					dtOrder.Columns.Add("DeliveryStartDate", typeof(DateTime));
					dtOrder.Columns.Add("DeliveryDays", typeof(int));
					dtOrder.Columns.Add("ReferencePartyId", typeof(int));
					dtOrder.Columns.Add("ReferencPartyName", typeof(string));
					dtOrder.Columns.Add("ReferencPartyAddress", typeof(string));
					dtOrder.Columns.Add("ReferencPartyCellNo", typeof(string));
					dtOrder.Columns.Add("BuildingStoreyId", typeof(int));
					dtOrder.Columns.Add("BuildingStorey", typeof(string));
					dtOrder.Columns.Add("BuildingHeightId", typeof(int));
					dtOrder.Columns.Add("BuildingHeight", typeof(string));
					dtOrder.Columns.Add("BuildingArea", typeof(string));
					dtOrder.Columns.Add("RemarksHeader", typeof(string));
					dtOrder.Columns.Add("CommissionAgentId", typeof(int));
					dtOrder.Columns.Add("RefSalesManId", typeof(int));
					dtOrder.Columns.Add("OrderQtyHeader", typeof(decimal));
					dtOrder.Columns.Add("OrderWeightHeader", typeof(decimal));
					dtOrder.Columns.Add("OrderItemId", typeof(int));
					dtOrder.Columns.Add("SaleGLAC", typeof(int));
					dtOrder.Columns.Add("ItemCode", typeof(string));
					dtOrder.Columns.Add("ItemName", typeof(string));
					dtOrder.Columns.Add("ItemAttributeVarientId", typeof(int));
					dtOrder.Columns.Add("ItemArrtibuteVarient", typeof(string));
					dtOrder.Columns.Add("VarientEquivalent", typeof(decimal));
					dtOrder.Columns.Add("JobLotId", typeof(int));
					dtOrder.Columns.Add("JobLotDescription", typeof(string));
					dtOrder.Columns.Add("ItemQty", typeof(decimal));
					dtOrder.Columns.Add("DispatchQty", typeof(decimal));
					dtOrder.Columns.Add("BalanceQty", typeof(decimal));
					dtOrder.Columns.Add("ItemWeight", typeof(decimal));
					dtOrder.Columns.Add("NetWeight", typeof(decimal));
					dtOrder.Columns.Add("DispatchWeight", typeof(decimal));
					dtOrder.Columns.Add("BalanceWeight", typeof(decimal));
					dtOrder.Columns.Add("CityId", typeof(int));
					dtOrder.Columns.Add("CityName", typeof(string));
					dtOrder.Columns.Add("RemarksDetail", typeof(string));
					dtOrder.Rows.Clear();
					List<int> headerIdList = ((IEnumerable<string>)HeaderIds.Split(',')).Select((Func<string, int>)Conversion.ToInt).ToList();
					DataTable resultRows = (from row in dtGrid.AsEnumerable()
						where headerIdList.Contains(row.Field<int>("Id"))
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtOrder.Rows.Add(resultRows.Rows[i]["Id"], resultRows.Rows[i]["DetailId"], resultRows.Rows[i]["DocumentTypeId"], resultRows.Rows[i]["DocNo"], resultRows.Rows[i]["DocDate"], resultRows.Rows[i]["SupplierCustomerId"], resultRows.Rows[i]["RefrenenceNo"], resultRows.Rows[i]["Distance"], resultRows.Rows[i]["VisitedById"], resultRows.Rows[i]["PaymentTermsId"], resultRows.Rows[i]["OrderDueDays"], resultRows.Rows[i]["OrderDueDate"], resultRows.Rows[i]["OrderExpiryDate"], resultRows.Rows[i]["DeliveryTerm"], resultRows.Rows[i]["DeliveryStartDate"], resultRows.Rows[i]["DeliveryDays"], resultRows.Rows[i]["ReferencePartyId"], resultRows.Rows[i]["ReferencPartyName"], resultRows.Rows[i]["ReferencPartyAddress"], resultRows.Rows[i]["ReferencPartyCellNo"], resultRows.Rows[i]["BuildingStoreyId"], resultRows.Rows[i]["BuildingStorey"], resultRows.Rows[i]["BuildingHeightId"], resultRows.Rows[i]["BuildingHeight"], resultRows.Rows[i]["BuildingArea"], resultRows.Rows[i]["RemarksHeader"], resultRows.Rows[i]["CommissionAgentId"], resultRows.Rows[i]["RefSalesManId"], resultRows.Rows[i]["OrderQtyHeader"], resultRows.Rows[i]["OrderWeightHeader"], resultRows.Rows[i]["OrderItemId"], resultRows.Rows[i]["SaleGLAC"], resultRows.Rows[i]["ItemCode"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemAttributeVarientId"], resultRows.Rows[i]["ItemArrtibuteVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["JobLotId"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["DispatchQty"], resultRows.Rows[i]["BalanceQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["DispatchWeight"], resultRows.Rows[i]["BalanceWeight"], resultRows.Rows[i]["CityId"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					grdDetail.DataSource = dtOrder;
					grdDetail.RetrieveStructure();
					GridDetailSetting();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
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
			grdDetail.RootTable.Columns["BuildingStoreyId"].Visible = false;
			grdDetail.RootTable.Columns["BuildingStorey"].Visible = false;
			grdDetail.RootTable.Columns["BuildingHeightId"].Visible = false;
			grdDetail.RootTable.Columns["BuildingHeight"].Visible = false;
			grdDetail.RootTable.Columns["BuildingArea"].Visible = false;
			grdDetail.RootTable.Columns["ReferencePartyId"].Visible = false;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DetailId"].Visible = false;
			grdDetail.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["DocNo"].Visible = false;
			grdDetail.RootTable.Columns["DocDate"].Visible = false;
			grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdDetail.RootTable.Columns["RefrenenceNo"].Visible = false;
			grdDetail.RootTable.Columns["Distance"].Visible = false;
			grdDetail.RootTable.Columns["VisitedById"].Visible = false;
			grdDetail.RootTable.Columns["PaymentTermsId"].Visible = false;
			grdDetail.RootTable.Columns["OrderDueDays"].Visible = false;
			grdDetail.RootTable.Columns["OrderDueDate"].Visible = false;
			grdDetail.RootTable.Columns["OrderExpiryDate"].Visible = false;
			grdDetail.RootTable.Columns["DeliveryTerm"].Visible = false;
			grdDetail.RootTable.Columns["DeliveryStartDate"].Visible = false;
			grdDetail.RootTable.Columns["DeliveryDays"].Visible = false;
			grdDetail.RootTable.Columns["ReferencPartyName"].Visible = false;
			grdDetail.RootTable.Columns["ReferencPartyAddress"].Visible = false;
			grdDetail.RootTable.Columns["ReferencPartyCellNo"].Visible = false;
			grdDetail.RootTable.Columns["RemarksHeader"].Visible = false;
			grdDetail.RootTable.Columns["CommissionAgentId"].Visible = false;
			grdDetail.RootTable.Columns["RefSalesManId"].Visible = false;
			grdDetail.RootTable.Columns["OrderQtyHeader"].Visible = false;
			grdDetail.RootTable.Columns["OrderWeightHeader"].Visible = false;
			grdDetail.RootTable.Columns["SaleGLAC"].Visible = false;
			grdDetail.RootTable.Columns["ItemAttributeVarientId"].Visible = false;
			grdDetail.RootTable.Columns["OrderItemId"].Visible = false;
			grdDetail.RootTable.Columns["CityId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotDescription"].Caption = "JobLot";
			grdDetail.RootTable.Columns["ItemArrtibuteVarient"].Caption = "Varient";
			grdDetail.RootTable.Columns["VarientEquivalent"].Caption = "VarientUnit";
			grdDetail.RootTable.Columns["OrderQtyHeader"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OrderQtyHeader"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OrderQtyHeader"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["OrderQtyHeader"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["OrderWeightHeader"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["OrderWeightHeader"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["OrderWeightHeader"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["OrderWeightHeader"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["BalanceQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalanceQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalanceQty"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["BalanceQty"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grdDetail.RootTable.Columns["DispatchWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DispatchWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DispatchWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DispatchWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Select");
			grdDetail.RootTable.Columns["Select"].ActAsSelector = true;
			grdDetail.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdDetail.RootTable.Columns["Select"].Width = 40;
			grdDetail.RootTable.Columns["Select"].Position = 0;
			grdDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Select")
			{
				GetCheckedRowsId();
				CalculateTotal();
				DetailGridBind(HeaderorderIds);
				CheckedAllDetailRows();
			}
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
			if (grdMain.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.IsChecked = Convert.ToBoolean(value: true);
					obj.EndEdit();
				}
				grdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCheckedRowsId()
	{
		try
		{
			HeaderorderIds = "";
			if (grdMain.GetCheckedRows().Count() > 0)
			{
				GridEXRow[] checkedRows = grdMain.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int orderId = Conversion.ToInt(checkedRows[i].Cells["Id"].Value);
					HeaderorderIds = HeaderorderIds + "," + orderId;
				}
			}
			else
			{
				HeaderorderIds = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMain_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdMain.CurrentColumn != null && grdMain.CurrentColumn.Key == "Select")
			{
				grdMain.UpdateData();
				GetCheckedRowsId();
				CalculateTotal();
				DetailGridBind(HeaderorderIds);
				CheckedAllDetailRows();
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
			if (Conversion.ToInt((object)grdDetail.GetCheckedRows().Count().ToString()) > 0)
			{
				dtLoader.Rows.Clear();
				GridEXRow[] checkedRows = grdDetail.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					Conversion.ToInt(r.Cells["Id"].Value);
					dtLoader.Rows.Add(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DetailId"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value), Conversion.ToInt(r.Cells["DocNo"].Value), Conversion.ToDateTime(r.Cells["DocDate"].Value), Conversion.ToInt(r.Cells["SupplierCustomerId"].Value), Conversion.ToString(r.Cells["RefrenenceNo"].Value), Conversion.ToString(r.Cells["Distance"].Value), Conversion.ToInt(r.Cells["VisitedById"].Value), Conversion.ToInt(r.Cells["PaymentTermsId"].Value), Conversion.ToInt(r.Cells["OrderDueDays"].Value), Conversion.ToDateTime(r.Cells["OrderDueDate"].Value), Conversion.ToDateTime(r.Cells["OrderExpiryDate"].Value), Conversion.ToString(r.Cells["DeliveryTerm"].Value), Conversion.ToDateTime(r.Cells["DeliveryStartDate"].Value), Conversion.ToInt(r.Cells["DeliveryDays"].Value), Conversion.ToInt(r.Cells["ReferencePartyId"].Value), Conversion.ToString(r.Cells["ReferencPartyName"].Value), Conversion.ToString(r.Cells["ReferencPartyAddress"].Value), Conversion.ToString(r.Cells["ReferencPartyCellNo"].Value), Conversion.ToInt(r.Cells["BuildingStoreyId"].Value), Conversion.ToString(r.Cells["BuildingStorey"].Value), Conversion.ToInt(r.Cells["BuildingHeightId"].Value), Conversion.ToString(r.Cells["BuildingHeight"].Value), Conversion.ToString(r.Cells["BuildingArea"].Value), Conversion.ToString(r.Cells["RemarksHeader"].Value), Conversion.ToInt(r.Cells["CommissionAgentId"].Value), Conversion.ToInt(r.Cells["RefSalesManId"].Value), Conversion.ToDecimal(r.Cells["OrderQtyHeader"].Value), Conversion.ToDecimal(r.Cells["OrderWeightHeader"].Value), Conversion.ToInt(r.Cells["OrderItemId"].Value), Conversion.ToInt(r.Cells["SaleGLAC"].Value), Conversion.ToString(r.Cells["ItemCode"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToInt(r.Cells["ItemAttributeVarientId"].Value), Conversion.ToString(r.Cells["ItemArrtibuteVarient"].Value), Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value), Conversion.ToInt(r.Cells["JobLotId"].Value), Conversion.ToString(r.Cells["JobLotDescription"].Value), Conversion.ToDecimal(r.Cells["ItemQty"].Value), Conversion.ToDecimal(r.Cells["DispatchQty"].Value), Conversion.ToDecimal(r.Cells["BalanceQty"].Value), Conversion.ToDecimal(r.Cells["ItemWeight"].Value), Conversion.ToDecimal(r.Cells["NetWeight"].Value), Conversion.ToDecimal(r.Cells["DispatchWeight"].Value), Conversion.ToDecimal(r.Cells["BalanceWeight"].Value), Conversion.ToInt(r.Cells["CityId"].Value), Conversion.ToString(r.Cells["CityName"].Value), Conversion.ToString(r.Cells["RemarksDetail"].Value));
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdMain).Name);
			if (fs != null)
			{
				grdMain.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
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

	private void LoadPurchaseOrder_KeyDown(object sender, KeyEventArgs e)
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
				btngrnlod_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadOnInvoice_Click_1(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnReset_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdMain).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1852);
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
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Expected O, but got Unknown
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Expected O, but got Unknown
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Expected O, but got Unknown
		//IL_0983: Unknown result type (might be due to invalid IL or missing references)
		//IL_098d: Expected O, but got Unknown
		GridEXLayout grdMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance3 = new Appearance();
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
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label23 = new System.Windows.Forms.Label();
		this.grdMain = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.txtBalanceAmountHeader = new System.Windows.Forms.TextBox();
		this.txtBalanceWeightHeadrer = new System.Windows.Forms.TextBox();
		this.txtBalanceQtyHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchAmountHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchWeightHeader = new System.Windows.Forms.TextBox();
		this.txtDispatchQtyHeader = new System.Windows.Forms.TextBox();
		this.txtOrderAmountHeader = new System.Windows.Forms.TextBox();
		this.txtQrderWeightHeader = new System.Windows.Forms.TextBox();
		this.txtOrderQtyHeader = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbItemtype = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbCategory = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbParentCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.btnReset = new System.Windows.Forms.Button();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 50);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 25);
		this.toolStrip1.TabIndex = 27;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.Teal;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 27);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(166, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Sale Order";
		this.grdMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdMain).BackColor = System.Drawing.Color.White;
		grdMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMain.DesignTimeLayout = grdMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMain).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdMain.FilterMode = (FilterMode)2;
		this.grdMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMain.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdMain).Location = new System.Drawing.Point(0, 212);
		((System.Windows.Forms.Control)(object)this.grdMain).Name = "grdMain";
		((ScrollableBase)this.grdMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMain).Size = new System.Drawing.Size(1034, 312);
		((System.Windows.Forms.Control)(object)this.grdMain).TabIndex = 0;
		this.grdMain.TotalRow = (InheritableBoolean)1;
		this.grdMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMain).VisualStyle = (VisualStyle)4;
		this.grdMain.ColumnButtonClick += new ColumnActionEventHandler(grdMain_ColumnButtonClick);
		this.grdMain.SelectionChanged += new System.EventHandler(grdMain_SelectionChanged);
		this.grdMain.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdMain).Click += new System.EventHandler(grdMain_Click);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox3);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 136);
		this.panel2.TabIndex = 0;
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label12);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add(this.txtBalanceAmountHeader);
		this.groupBox3.Controls.Add(this.txtBalanceWeightHeadrer);
		this.groupBox3.Controls.Add(this.txtBalanceQtyHeader);
		this.groupBox3.Controls.Add(this.txtDispatchAmountHeader);
		this.groupBox3.Controls.Add(this.txtDispatchWeightHeader);
		this.groupBox3.Controls.Add(this.txtDispatchQtyHeader);
		this.groupBox3.Controls.Add(this.txtOrderAmountHeader);
		this.groupBox3.Controls.Add(this.txtQrderWeightHeader);
		this.groupBox3.Controls.Add(this.txtOrderQtyHeader);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(605, -1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(421, 133);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Total Of Selected Row";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Dispatch";
		this.label12.BackColor = System.Drawing.Color.Silver;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(299, 22);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(116, 20);
		this.label12.TabIndex = 0;
		this.label12.Text = "Amount";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label12.Visible = false;
		this.label11.BackColor = System.Drawing.Color.Silver;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(180, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(116, 20);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.BackColor = System.Drawing.Color.Silver;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(61, 22);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(116, 20);
		this.label10.TabIndex = 0;
		this.label10.Text = "Qty";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(5, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(50, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Ordered";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(5, 108);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 0;
		this.label25.Text = "Balance";
		this.txtBalanceAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceAmountHeader.Location = new System.Drawing.Point(299, 104);
		this.txtBalanceAmountHeader.Name = "txtBalanceAmountHeader";
		this.txtBalanceAmountHeader.ShortcutsEnabled = false;
		this.txtBalanceAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceAmountHeader.TabIndex = 8;
		this.txtBalanceAmountHeader.Visible = false;
		this.txtBalanceWeightHeadrer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeightHeadrer.Location = new System.Drawing.Point(180, 104);
		this.txtBalanceWeightHeadrer.Name = "txtBalanceWeightHeadrer";
		this.txtBalanceWeightHeadrer.ShortcutsEnabled = false;
		this.txtBalanceWeightHeadrer.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceWeightHeadrer.TabIndex = 5;
		this.txtBalanceQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceQtyHeader.Location = new System.Drawing.Point(61, 104);
		this.txtBalanceQtyHeader.Name = "txtBalanceQtyHeader";
		this.txtBalanceQtyHeader.ShortcutsEnabled = false;
		this.txtBalanceQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtBalanceQtyHeader.TabIndex = 2;
		this.txtDispatchAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchAmountHeader.Location = new System.Drawing.Point(299, 75);
		this.txtDispatchAmountHeader.Name = "txtDispatchAmountHeader";
		this.txtDispatchAmountHeader.ShortcutsEnabled = false;
		this.txtDispatchAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchAmountHeader.TabIndex = 7;
		this.txtDispatchAmountHeader.Visible = false;
		this.txtDispatchWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchWeightHeader.Location = new System.Drawing.Point(180, 75);
		this.txtDispatchWeightHeader.Name = "txtDispatchWeightHeader";
		this.txtDispatchWeightHeader.ShortcutsEnabled = false;
		this.txtDispatchWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchWeightHeader.TabIndex = 4;
		this.txtDispatchQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDispatchQtyHeader.Location = new System.Drawing.Point(61, 75);
		this.txtDispatchQtyHeader.Name = "txtDispatchQtyHeader";
		this.txtDispatchQtyHeader.ShortcutsEnabled = false;
		this.txtDispatchQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtDispatchQtyHeader.TabIndex = 1;
		this.txtOrderAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderAmountHeader.Location = new System.Drawing.Point(299, 46);
		this.txtOrderAmountHeader.Name = "txtOrderAmountHeader";
		this.txtOrderAmountHeader.ShortcutsEnabled = false;
		this.txtOrderAmountHeader.Size = new System.Drawing.Size(116, 23);
		this.txtOrderAmountHeader.TabIndex = 6;
		this.txtOrderAmountHeader.Visible = false;
		this.txtQrderWeightHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQrderWeightHeader.Location = new System.Drawing.Point(180, 46);
		this.txtQrderWeightHeader.Name = "txtQrderWeightHeader";
		this.txtQrderWeightHeader.ShortcutsEnabled = false;
		this.txtQrderWeightHeader.Size = new System.Drawing.Size(116, 23);
		this.txtQrderWeightHeader.TabIndex = 3;
		this.txtOrderQtyHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderQtyHeader.Location = new System.Drawing.Point(61, 46);
		this.txtOrderQtyHeader.Name = "txtOrderQtyHeader";
		this.txtOrderQtyHeader.ShortcutsEnabled = false;
		this.txtOrderQtyHeader.Size = new System.Drawing.Size(116, 23);
		this.txtOrderQtyHeader.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemtype);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentCategory);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.txtOrderNoTo);
		this.groupBox1.Controls.Add(this.btnLoadOnInvoice);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.txtOrderNoFrom);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(603, 133);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.cmbItemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbItemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Location = new System.Drawing.Point(320, 44);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Name = "cmbItemtype";
		((System.Windows.Forms.Control)(object)this.cmbItemtype).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemtype).TabIndex = 6;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(230, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(60, 15);
		this.label5.TabIndex = 124;
		this.label5.Text = "Item Type";
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(320, 15);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(230, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 123;
		this.label8.Text = "Item Category";
		this.cmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Location = new System.Drawing.Point(91, 100);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Name = "cmbParentCategory";
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentCategory).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(1, 106);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 122;
		this.label6.Text = "Parent Category";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(228, 79);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 116;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(320, 73);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(48, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(522, 44);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(61, 26);
		this.btnReset.TabIndex = 11;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(157, 75);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(70, 23);
		this.txtOrderNoTo.TabIndex = 3;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(522, 73);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(61, 26);
		this.btnLoadOnInvoice.TabIndex = 10;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(320, 102);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 8;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(522, 102);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(61, 26);
		this.btngrnlod.TabIndex = 9;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(228, 108);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Customer Name";
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(91, 17);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(136, 23);
		this.FromDate.TabIndex = 0;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(91, 75);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(64, 23);
		this.txtOrderNoFrom.TabIndex = 2;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(91, 46);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(136, 23);
		this.Todate.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(5, 79);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "No From && To";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 186);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(985, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 547);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1034, 114);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 32;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 524);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 23);
		this.panel4.TabIndex = 33;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 661);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdMain);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LoadSaleOrderForDeliveryOrderConcrete";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LoadPurchaseOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMain).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
