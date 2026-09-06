// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
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
namespace Architecture.WinApp.ExportReports
{

public class PerformaInvoiceRegister : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	public int DocumentTypeId;

	public DataTable dtSupply = new DataTable();

	private DataTable dtloadExIO = new DataTable();

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private Panel panel3;

	internal GridEX grd;

	private Label label2;

	internal UltraCombo CmbPartyName;

	private Button btnReset;

	private Label label5;

	internal UltraCombo cmbCurrency;

	private Label label3;

	internal UltraCombo cmbItemName;

	private GroupBox groupBox1;

	private Panel panel4;

	internal GridEX grddetail;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel6;

	private Panel panel7;

	private Label label6;

	private Panel panel5;

	internal GridEX GridMainDetail;

	private Panel panel8;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar3;

	private CheckBox ChboxSkipZero;

	private Button btnPrint;

	public PerformaInvoiceRegister(UserAccount user)
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
			base.AutoScrollMinSize = new Size(1100, 700);
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			dtSupply.Columns.Add("InvoiceId");
			ExportInvoicesLoad();
			PartyNameFill();
			CurrencyFill();
			ItemFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItemName, "Id", "ItemName", "Item Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyName", "Currency Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyNameFill()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("7");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPartyName, "Id", "CompanyName", "Party Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportInvoicesLoad()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dtloadExIO.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = "151";
			obj.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.FcyId = Conversion.ToInt(cmbCurrency.Value);
			if (ChboxSkipZero.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			dtloadExIO = ExImInvoice.GetProformaDataForInvoices(obj);
			if (dtloadExIO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("Pri_Ref#", typeof(string));
				dt.Columns.Add("Pri_RefDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("PartyName", typeof(string));
				dt.Columns.Add("SalesPersonId", typeof(int));
				dt.Columns.Add("SalesMan", typeof(string));
				dt.Columns.Add("CommissionAgentId", typeof(int));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("Comm%", typeof(double));
				dt.Columns.Add("CommAmount", typeof(double));
				dt.Columns.Add("CurrencyCode", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("LoadingPort", typeof(string));
				dt.Columns.Add("DestinationPort", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("PackingWeight", typeof(double));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("NoOfContainers", typeof(double));
				dt.Columns.Add("NoOfBags/Cntnr", typeof(double));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("MTon", typeof(double));
				dt.Columns.Add("ShipMTon", typeof(double));
				dt.Columns.Add("BalMTon", typeof(double));
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("ShipBags", typeof(double));
				dt.Columns.Add("BalBags", typeof(double));
				dt.Columns.Add("PackingTotalWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ShipWeight", typeof(double));
				dt.Columns.Add("BalWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("ShipAmount", typeof(double));
				dt.Columns.Add("BalAmount", typeof(double));
				dt.Columns.Add("PackingExpiryDate", typeof(string));
				dt.Columns.Add("ContainerSize", typeof(string));
				dt.Columns.Add("OtherDescription", typeof(string));
				dt.Columns.Add("OuterPackUomDescription", typeof(string));
				dt.Columns.Add("InsuranceRemarks", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int j = 0; j < dtloadExIO.Rows.Count; j++)
				{
					dt.Rows.Add(dtloadExIO.Rows[j]["Id"], dtloadExIO.Rows[j]["DocumentTypeId"], dtloadExIO.Rows[j]["DocNo"], Conversion.ToDateTime(dtloadExIO.Rows[j]["ProformaDate"]), dtloadExIO.Rows[j]["ProformaNo"], Conversion.ToDateTime(dtloadExIO.Rows[j]["SalesContratDate"]), dtloadExIO.Rows[j]["SupCustId"], dtloadExIO.Rows[j]["CustomerName"], dtloadExIO.Rows[j]["SalesPersonId"], dtloadExIO.Rows[j]["SalesMan"], dtloadExIO.Rows[j]["CommissionAgentId"], dtloadExIO.Rows[j]["CommissionAgent"], dtloadExIO.Rows[j]["CommRate"], dtloadExIO.Rows[j]["CommAmount"], dtloadExIO.Rows[j]["CurrencyCode"], dtloadExIO.Rows[j]["DeliveryTerm"], dtloadExIO.Rows[j]["PaymentTerm"], dtloadExIO.Rows[j]["LoadingPort"], dtloadExIO.Rows[j]["DestinationPort"], dtloadExIO.Rows[j]["ExImItemId"], dtloadExIO.Rows[j]["ItemName"], dtloadExIO.Rows[j]["PackingType"], dtloadExIO.Rows[j]["PackingWeight"], dtloadExIO.Rows[j]["CropYear"], dtloadExIO.Rows[j]["NoofContainerDetail"], dtloadExIO.Rows[j]["NoofBagsPerContainer"], dtloadExIO.Rows[j]["PackUom"], dtloadExIO.Rows[j]["M_Ton"], dtloadExIO.Rows[j]["ShipMTon"], dtloadExIO.Rows[j]["BalMTon"], dtloadExIO.Rows[j]["NoOfBags"], dtloadExIO.Rows[j]["ShipBags"], dtloadExIO.Rows[j]["BalNoofBags"], dtloadExIO.Rows[j]["PackingTotalWeight"], dtloadExIO.Rows[j]["NetWeight"], dtloadExIO.Rows[j]["ShipWeight"], dtloadExIO.Rows[j]["BalWeight"], dtloadExIO.Rows[j]["RatePrice"], dtloadExIO.Rows[j]["Amount"], dtloadExIO.Rows[j]["ShipAmount"], dtloadExIO.Rows[j]["BalShipAmount"], dtloadExIO.Rows[j]["PackingExpiryDate"], dtloadExIO.Rows[j]["ContainerSize"], dtloadExIO.Rows[j]["Other_Description"], dtloadExIO.Rows[j]["OuterPackDescription"], dtloadExIO.Rows[j]["InsuranceRemarks"], dtloadExIO.Rows[j]["RemarksHeader"], dtloadExIO.Rows[j]["EntryUser"], Conversion.ToDateTime(dtloadExIO.Rows[j]["EntryDate"]), dtloadExIO.Rows[j]["ModifyUser"], Conversion.ToDateTime(dtloadExIO.Rows[j]["ModifyDate"]), dtloadExIO.Rows[j]["NoOfAttachments"]);
				}
				GridMainDetail.DataSource = dt;
				GridMainDetail.RetrieveStructure();
				GridSettingForMainDetail();
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("DocumentTypeId", typeof(int));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("Pri_Ref#", typeof(string));
				dtMain.Columns.Add("Pri_RefDate", typeof(DateTime));
				dtMain.Columns.Add("SupplierCustomerId", typeof(int));
				dtMain.Columns.Add("PartyName", typeof(string));
				dtMain.Columns.Add("SalesPersonId", typeof(int));
				dtMain.Columns.Add("SalesMan", typeof(string));
				dtMain.Columns.Add("CommissionAgentId", typeof(int));
				dtMain.Columns.Add("CommissionAgent", typeof(string));
				dtMain.Columns.Add("Comm%", typeof(double));
				dtMain.Columns.Add("CommAmount", typeof(double));
				dtMain.Columns.Add("FCurrencyId", typeof(string));
				dtMain.Columns.Add("CurrencyCode", typeof(string));
				dtMain.Columns.Add("DeliveryTermId", typeof(int));
				dtMain.Columns.Add("DeliveryTerm", typeof(string));
				dtMain.Columns.Add("PaymentTermId", typeof(int));
				dtMain.Columns.Add("PaymentTerm", typeof(string));
				dtMain.Columns.Add("LoadingPortId", typeof(int));
				dtMain.Columns.Add("LoadingPort", typeof(string));
				dtMain.Columns.Add("DestinationPortId", typeof(int));
				dtMain.Columns.Add("DestinationPort", typeof(string));
				dtMain.Columns.Add("InsuranceRemarks", typeof(string));
				dtMain.Columns.Add("RemarksHeader", typeof(string));
				dtMain.Columns.Add("EntryUser", typeof(string));
				dtMain.Columns.Add("EntryDate", typeof(DateTime));
				dtMain.Columns.Add("ModifyUser", typeof(string));
				dtMain.Columns.Add("ModifyDate", typeof(DateTime));
				dtMain.Columns.Add("NoOfAttachments", typeof(int));
				int i;
				for (i = 0; i < dtloadExIO.Rows.Count; i++)
				{
					if (!dtMain.AsEnumerable().Any((DataRow dr) => (int)dr["Id"] == Convert.ToInt32(dtloadExIO.Rows[i]["Id"])))
					{
						dtMain.Rows.Add(dtloadExIO.Rows[i]["Id"], dtloadExIO.Rows[i]["DocumentTypeId"], dtloadExIO.Rows[i]["DocNo"], Conversion.ToDateTime(dtloadExIO.Rows[i]["ProformaDate"]), dtloadExIO.Rows[i]["ProformaNo"], Conversion.ToDateTime(dtloadExIO.Rows[i]["SalesContratDate"]), dtloadExIO.Rows[i]["SupCustId"], dtloadExIO.Rows[i]["CustomerName"], dtloadExIO.Rows[i]["SalesPersonId"], dtloadExIO.Rows[i]["SalesMan"], dtloadExIO.Rows[i]["CommissionAgentId"], dtloadExIO.Rows[i]["CommissionAgent"], dtloadExIO.Rows[i]["CommRate"], dtloadExIO.Rows[i]["CommAmount"], dtloadExIO.Rows[i]["FCurrencyId"], dtloadExIO.Rows[i]["CurrencyCode"], dtloadExIO.Rows[i]["DeliveryTermId"], dtloadExIO.Rows[i]["DeliveryTerm"], dtloadExIO.Rows[i]["PaymentTermId"], dtloadExIO.Rows[i]["PaymentTerm"], dtloadExIO.Rows[i]["LoadingPortId"], dtloadExIO.Rows[i]["LoadingPort"], dtloadExIO.Rows[i]["DestinationPortId"], dtloadExIO.Rows[i]["DestinationPort"], dtloadExIO.Rows[i]["InsuranceRemarks"], dtloadExIO.Rows[i]["RemarksHeader"], dtloadExIO.Rows[i]["EntryUser"], Conversion.ToDateTime(dtloadExIO.Rows[i]["EntryDate"]), dtloadExIO.Rows[i]["ModifyUser"], Conversion.ToDateTime(dtloadExIO.Rows[i]["ModifyDate"]), dtloadExIO.Rows[i]["NoOfAttachments"]);
					}
				}
				grd.DataSource = dtMain;
				grd.RetrieveStructure();
				grdSettings();
			}
			ctrlGrdBar1_Load(null, null);
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["DestinationPortId"].Visible = false;
			grd.RootTable.Columns["CommissionAgentId"].Visible = false;
			grd.RootTable.Columns["LoadingPortId"].Visible = false;
			grd.RootTable.Columns["DeliveryTermId"].Visible = false;
			grd.RootTable.Columns["PaymentTermId"].Visible = false;
			grd.RootTable.Columns["FCurrencyId"].Visible = false;
			grd.RootTable.Columns["SalesPersonId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustment(AryColWidth, grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForMainDetail()
	{
		try
		{
			GridMainDetail.RootTable.Columns["Id"].Visible = false;
			GridMainDetail.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridMainDetail.RootTable.Columns["SalesPersonId"].Visible = false;
			GridMainDetail.RootTable.Columns["CommissionAgentId"].Visible = false;
			GridMainDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridMainDetail.RootTable.Columns["ItemId"].Visible = false;
			GridMainDetail.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridMainDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["Amount"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipAmount"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["ShipAmount"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["ShipAmount"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipAmount"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["BalAmount"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipBags"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["ShipBags"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["ShipBags"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipBags"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalBags"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["BalBags"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["BalBags"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalBags"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipWeight"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["ShipWeight"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["ShipWeight"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["ShipWeight"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["BalWeight"].FormatString = "#,##0";
			GridMainDetail.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0";
			GridMainDetail.RootTable.Columns["NoOfBags/Cntnr"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["NoOfBags/Cntnr"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["NoOfBags/Cntnr"].FormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["NoOfBags/Cntnr"].TotalFormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["PackingWeight"].FormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["PackingTotalWeight"].AggregateFunction = (AggregateFunction)2;
			GridMainDetail.RootTable.Columns["PackingTotalWeight"].TextAlignment = (TextAlignment)3;
			GridMainDetail.RootTable.Columns["PackingTotalWeight"].FormatString = "#,##0.##";
			GridMainDetail.RootTable.Columns["PackingTotalWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, GridMainDetail);
			ctrlGrdBar3_Load(null, null);
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
			ExportInvoicesLoad();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grddetail).Name);
			if (fs != null)
			{
				grddetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridMainDetail).Name);
			if (fs != null)
			{
				GridMainDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		((Control)(object)CmbPartyName).Text = string.Empty;
		PartyNameFill();
		ItemFill();
		CurrencyFill();
		ExportInvoicesLoad();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		dtSupply = null;
		Hide();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtloadExIO == null || dtloadExIO.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtloadExIO, "546-GetProformaDataForInvoices.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("PerformaDetailId", typeof(int));
				dtDetail.Columns.Add("ItemId", typeof(int));
				dtDetail.Columns.Add("ItemName", typeof(string));
				dtDetail.Columns.Add("PackingType", typeof(string));
				dtDetail.Columns.Add("PackingWeight", typeof(double));
				dtDetail.Columns.Add("CropYear", typeof(string));
				dtDetail.Columns.Add("NoOfContainers", typeof(double));
				dtDetail.Columns.Add("NoOfBags/Cntnr", typeof(double));
				dtDetail.Columns.Add("PackUom", typeof(string));
				dtDetail.Columns.Add("MTon", typeof(double));
				dtDetail.Columns.Add("ShipMTon", typeof(double));
				dtDetail.Columns.Add("BalMTon", typeof(double));
				dtDetail.Columns.Add("NoOfBags", typeof(double));
				dtDetail.Columns.Add("ShipBags", typeof(double));
				dtDetail.Columns.Add("BalBags", typeof(double));
				dtDetail.Columns.Add("PackingTotalWeight", typeof(double));
				dtDetail.Columns.Add("NetWeight", typeof(double));
				dtDetail.Columns.Add("ShipWeight", typeof(double));
				dtDetail.Columns.Add("BalWeight", typeof(double));
				dtDetail.Columns.Add("ItemRate", typeof(double));
				dtDetail.Columns.Add("Amount", typeof(double));
				dtDetail.Columns.Add("ShipAmount", typeof(double));
				dtDetail.Columns.Add("BalAmount", typeof(double));
				dtDetail.Columns.Add("PackingExpiryDate", typeof(string));
				dtDetail.Columns.Add("ContainerSize", typeof(string));
				dtDetail.Columns.Add("OtherDescription", typeof(string));
				dtDetail.Columns.Add("OuterPackUomDescription", typeof(string));
				DataTable resultRows = (from row in dtloadExIO.AsEnumerable()
					where row.Field<int>("Id") == Conversion.ToInt(item.Cells["Id"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["ContractDetailId"], resultRows.Rows[i]["ExImItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackingType"], resultRows.Rows[i]["PackingWeight"], resultRows.Rows[i]["CropYear"], resultRows.Rows[i]["NoofContainerDetail"], resultRows.Rows[i]["NoofBagsPerContainer"], resultRows.Rows[i]["PackUom"], resultRows.Rows[i]["M_Ton"], resultRows.Rows[i]["ShipMTon"], resultRows.Rows[i]["BalMTon"], resultRows.Rows[i]["NoOfBags"], resultRows.Rows[i]["ShipBags"], resultRows.Rows[i]["BalNoofBags"], resultRows.Rows[i]["PackingTotalWeight"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["ShipWeight"], resultRows.Rows[i]["BalWeight"], resultRows.Rows[i]["RatePrice"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["ShipAmount"], resultRows.Rows[i]["BalShipAmount"], resultRows.Rows[i]["PackingExpiryDate"], resultRows.Rows[i]["ContainerSize"], resultRows.Rows[i]["Other_Description"], resultRows.Rows[i]["OuterPackDescription"]);
				}
				grddetail.DataSource = dtDetail;
				grddetail.RetrieveStructure();
				GridDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettings()
	{
		try
		{
			grddetail.RootTable.Columns["ItemId"].Visible = false;
			grddetail.RootTable.Columns["PerformaDetailId"].Visible = false;
			grddetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["Amount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipAmount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipAmount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipAmount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipAmount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalAmount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NetWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipWeight"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["NoOfBags/Cntnr"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NoOfBags/Cntnr"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NoOfBags/Cntnr"].FormatString = "#,##0.##";
			grddetail.RootTable.Columns["NoOfBags/Cntnr"].TotalFormatString = "#,##0.##";
			grddetail.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grddetail.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grddetail.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["PackingWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["PackingWeight"].FormatString = "#,##0.##";
			grddetail.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			grddetail.RootTable.Columns["PackingTotalWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["PackingTotalWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["PackingTotalWeight"].FormatString = "#,##0.##";
			grddetail.RootTable.Columns["PackingTotalWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grddetail);
			ctrlGrdBar2_Load(null, null);
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
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value), 151);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(GridMainDetail.CurrentRow.Cells["Id"].Value), 151);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PerformaInvoiceRegister_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Expected O, but got Unknown
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_2162: Unknown result type (might be due to invalid IL or missing references)
		//IL_216c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		GridEXLayout grddetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridMainDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnPrint = new System.Windows.Forms.Button();
		this.ChboxSkipZero = new System.Windows.Forms.CheckBox();
		this.CmbPartyName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.cmbItemName = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grddetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.GridMainDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridMainDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(913, 32);
		this.panel1.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(223, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Performa Invoice Register";
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(899, 322);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 32);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(913, 113);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnPrint);
		this.groupBox1.Controls.Add(this.ChboxSkipZero);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(677, 107);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPrint.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.ForeColor = System.Drawing.Color.Black;
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPrint.Location = new System.Drawing.Point(576, 77);
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(88, 23);
		this.btnPrint.TabIndex = 8;
		this.btnPrint.Text = "Print-546";
		this.btnPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btnPrint.UseVisualStyleBackColor = true;
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.ChboxSkipZero.AutoSize = true;
		this.ChboxSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChboxSkipZero.Location = new System.Drawing.Point(350, 80);
		this.ChboxSkipZero.Name = "ChboxSkipZero";
		this.ChboxSkipZero.Size = new System.Drawing.Size(76, 19);
		this.ChboxSkipZero.TabIndex = 5;
		this.ChboxSkipZero.Text = "Skip Zero";
		this.ChboxSkipZero.UseVisualStyleBackColor = true;
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(349, 19);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(315, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 25);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 82);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 15);
		this.label5.TabIndex = 30;
		this.label5.Text = "Currency";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 54);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(48, 15);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(78, 76);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 2;
		this.FromDate.CustomFormat = "dd-MM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(77, 21);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(185, 23);
		this.FromDate.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(267, 54);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 26;
		this.label3.Text = "Item Name";
		this.Todate.CustomFormat = "dd-MM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(78, 50);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(185, 23);
		this.Todate.TabIndex = 1;
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(349, 48);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(315, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 4;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(449, 77);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(65, 24);
		this.btngrnlod.TabIndex = 6;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(517, 77);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 24);
		this.btnReset.TabIndex = 7;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(266, 25);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Party Name";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(899, 26);
		this.panel3.TabIndex = 31;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 19);
		this.label4.TabIndex = 66;
		this.label4.Text = "Main";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(851, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 65;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(899, 26);
		this.panel4.TabIndex = 32;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(4, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(58, 19);
		this.label6.TabIndex = 67;
		this.label6.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(850, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grddetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 64;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grddetail.AllowDelete = (InheritableBoolean)1;
		this.grddetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetail).BackColor = System.Drawing.Color.White;
		this.grddetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetail.DesignTimeLayout = grddetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetail.FilterMode = (FilterMode)2;
		this.grddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetail).Name = "grddetail";
		((ScrollableBase)this.grddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Size = new System.Drawing.Size(899, 193);
		((System.Windows.Forms.Control)(object)this.grddetail).TabIndex = 33;
		this.grddetail.TotalRow = (InheritableBoolean)1;
		this.grddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetail).VisualStyle = (VisualStyle)4;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 145);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(913, 604);
		this.tabControl1.TabIndex = 34;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.GridMainDetail);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage1.Location = new System.Drawing.Point(4, 27);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(905, 573);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detailed";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.GridMainDetail.AllowDelete = (InheritableBoolean)1;
		this.GridMainDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridMainDetail).BackColor = System.Drawing.Color.White;
		this.GridMainDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridMainDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridMainDetail.DesignTimeLayout = GridMainDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridMainDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridMainDetail.FilterMode = (FilterMode)2;
		this.GridMainDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridMainDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridMainDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridMainDetail).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.GridMainDetail).Name = "GridMainDetail";
		((ScrollableBase)this.GridMainDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridMainDetail).Size = new System.Drawing.Size(899, 541);
		((System.Windows.Forms.Control)(object)this.GridMainDetail).TabIndex = 33;
		this.GridMainDetail.TotalRow = (InheritableBoolean)1;
		this.GridMainDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridMainDetail).VisualStyle = (VisualStyle)4;
		this.GridMainDetail.LinkClicked += new ColumnActionEventHandler(GridMainDetail_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label7);
		this.panel8.Controls.Add(this.ctrlGrdBar3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(899, 26);
		this.panel8.TabIndex = 32;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(0, 19);
		this.label7.TabIndex = 66;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(857, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridMainDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 65;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Location = new System.Drawing.Point(4, 27);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(905, 573);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Main Detail";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 351);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(899, 219);
		this.panel6.TabIndex = 35;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grddetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 26);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(899, 193);
		this.panel7.TabIndex = 34;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(899, 322);
		this.panel5.TabIndex = 34;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(913, 749);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "PerformaInvoiceRegister";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PerformaInvoiceRegister_FormClosing);
		base.Load += new System.EventHandler(LoadInvoices_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridMainDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
