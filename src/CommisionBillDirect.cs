using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.CmTr.TransactionForms
{

public class CommisionBillDirect : Form
{
	private UserAccount UserAccount = new UserAccount();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtsummaryBySupplier = new DataTable();

	private DataTable dtPackingMaterial = new DataTable();

	private DataTable dtOtherRevenue = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtitem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private bool HasMultiCurrencyFeature;

	private int RecId;

	private bool UpdateMode;

	private frmLoadFromLoadingDelivery LoadFromLoadingDelivery;

	public string LoadDeliveryDetailIds = "";

	public string LoadDeliveryMainIds = "";

	private IContainer components;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private ToolStripButton BtnPrint;

	private ToolStripButton btnDelete;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox1;

	private Label label7;

	internal UltraCombo cmbCustomername;

	private Label label17;

	private TextBox txtbillno;

	private Label label2;

	internal UltraCombo cmbpayterm;

	private Label label3;

	private TextBox txtduedays;

	private DateTimePicker txtbilldate;

	private Label label6;

	internal UltraCombo cmbdeliveryterm;

	private Label label5;

	private TextBox txtmanualref;

	private Label label9;

	private TextBox txtbiltyno;

	private Label label8;

	private TextBox txtvehicleno;

	private Label label10;

	private RichTextBox txtRemarks;

	private Label label11;

	private GroupBox groupBox2;

	private Label label12;

	internal UltraCombo cmbCommAgent;

	private Label label19;

	private TextBox txtbrokeryamount;

	private Label label18;

	private TextBox txtbrokeryrate;

	private Label label15;

	internal UltraCombo cmbcommtype;

	private Label label14;

	private TextBox txtcommamount;

	private Label label13;

	private TextBox txtcommrate;

	private Label label20;

	private TextBox txtcustbillamnt;

	private Label label21;

	internal UltraCombo cmbcurrency;

	private Label label23;

	private TextBox txtfcyamount;

	private Label label22;

	private TextBox txtexchangerate;

	private Label label25;

	internal UltraCombo cmbsubbrokeryacc;

	private Label label27;

	private TextBox txtsubbrokeryamnt;

	private Label label26;

	private TextBox txtsubbrokeryrate;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnShortcutKeys;

	private Panel panel10;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label29;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label28;

	private Label label4;

	private TabControl tabControl1;

	private TabPage Form;

	private Panel panel5;

	internal GridEX grdrevenue;

	private Panel panel7;

	internal GridEX grdPackingMaterial;

	internal GridEX grdDetail;

	private Panel panel12;

	private Label label30;

	private Panel panel1;

	private Panel panel3;

	internal GridEX grdsummary;

	private Panel panel4;

	private Label label24;

	private Panel panel17;

	private Label label1;

	private TabPage History;

	internal GridEX grdhistoryDetail;

	private Panel panel8;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdHistory;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnLoadAllHistory;

	private Label label16;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton BtnLoadFromLoadingDelivery;

	private TextBox txtbiltydates;

	private ToolStripButton btnPrintVoucher;

	public CommisionBillDirect(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void CommisionBill_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnUpdate.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar7.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar7.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar7.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar7.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar7.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar7.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtsummaryBySupplier.Columns.Add("SupplierNameId", typeof(int));
			dtsummaryBySupplier.Columns.Add("SupplierName", typeof(string));
			dtsummaryBySupplier.Columns.Add("ItemAmount", typeof(double));
			dtsummaryBySupplier.Columns.Add("PackingAmount", typeof(double));
			dtsummaryBySupplier.Columns.Add("Expenses", typeof(double));
			dtsummaryBySupplier.Columns.Add("BillAmount", typeof(double));
			AddRowSummaryBySupplier();
			grdsummary.RetrieveStructure();
			grdsummarysetting();
			dtPackingMaterial.Columns.Add("Id", typeof(int));
			dtPackingMaterial.Columns.Add("PackingTypeId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemUomId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemSize", typeof(double));
			dtPackingMaterial.Columns.Add("Rate", typeof(double));
			dtPackingMaterial.Columns.Add("Weight", typeof(double));
			dtPackingMaterial.Columns.Add("Qty", typeof(double));
			dtPackingMaterial.Columns.Add("Amount", typeof(double));
			dtPackingMaterial.Columns.Add("CreditAccountId", typeof(int));
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdPackingMaterial.RetrieveStructure();
			AddRowPackingmaterial();
			grdpackingsetting();
			dtOtherRevenue.Columns.Add("OtherRevenueId", typeof(int));
			dtOtherRevenue.Columns.Add("Qty", typeof(double));
			dtOtherRevenue.Columns.Add("Rate", typeof(double));
			dtOtherRevenue.Columns.Add("AmountDr", typeof(double));
			dtOtherRevenue.Columns.Add("AmountCr", typeof(double));
			dtOtherRevenue.Columns.Add("SupplierAccountId", typeof(int));
			grdrevenue.DataSource = dtOtherRevenue;
			grdrevenue.RetrieveStructure();
			AddRowOtherRevenue();
			grdrevenuesetting();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("OrderId", typeof(int));
			dtDetail.Columns.Add("OrderDetailId", typeof(int));
			dtDetail.Columns.Add("OrderNo", typeof(string));
			dtDetail.Columns.Add("LoadingDeliveryId", typeof(int));
			dtDetail.Columns.Add("LoadingDeliveryDetailId", typeof(int));
			dtDetail.Columns.Add("LoadingDeliveryNo", typeof(string));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("CropYearId", typeof(int));
			dtDetail.Columns.Add("PackingTypeId", typeof(int));
			dtDetail.Columns.Add("PackUom", typeof(double));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("CustomerAccountId", typeof(int));
			dtDetail.Columns.Add("CusGrossWeight", typeof(double));
			dtDetail.Columns.Add("CusPMWeight", typeof(double));
			dtDetail.Columns.Add("CusAdlsWeight", typeof(double));
			dtDetail.Columns.Add("CusNetWeight", typeof(double));
			dtDetail.Columns.Add("CusRate", typeof(double));
			dtDetail.Columns.Add("CusRateUom", typeof(double));
			dtDetail.Columns.Add("CusAmount", typeof(double));
			dtDetail.Columns.Add("CusFcyAmount", typeof(double));
			dtDetail.Columns.Add("SupGrossWeight", typeof(double));
			dtDetail.Columns.Add("SupPMWeight", typeof(double));
			dtDetail.Columns.Add("SupAdlsWeight", typeof(double));
			dtDetail.Columns.Add("SupNetWeight", typeof(double));
			dtDetail.Columns.Add("SupRate", typeof(double));
			dtDetail.Columns.Add("SupRateUom", typeof(double));
			dtDetail.Columns.Add("SupAmount", typeof(double));
			dtDetail.Columns.Add("SupFcyAmount", typeof(double));
			dtDetail.Columns.Add("SupplierAccountId", typeof(int));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			AddRowIndetail();
			grdDetailsetting();
			PaymentTerms();
			suppliercustomer();
			DeliveryTerm();
			CurrencyFill();
			CommissionTypeFill();
			multiCurrencyFeature();
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int BaseCurrency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				cmbcurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = Conversion.ToInt((object)BaseRatelist[0].ConfigKey.ToString());
				txtexchangerate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void multiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbcurrency).Visible = true;
				txtexchangerate.Visible = true;
				txtfcyamount.Visible = true;
				label21.Visible = true;
				label22.Visible = true;
				label23.Visible = true;
				txtRemarks.Size = new Size(229, 79);
			}
			else
			{
				((Control)(object)cmbcurrency).Visible = false;
				txtexchangerate.Visible = false;
				txtfcyamount.Visible = false;
				label21.Visible = false;
				label22.Visible = false;
				label23.Visible = false;
				txtRemarks.Size = new Size(449, 79);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexchangerate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["CusFcyAmount"].Value) / Conversion.ToDouble((object)txtexchangerate.Text);
					obj.Cells["CusFcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["CusFcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
	}

	private void suppliercustomer()
	{
		try
		{
			int suppId = Conversion.ToInt(cmbCustomername.Value);
			int agentId = Conversion.ToInt(cmbCommAgent.Value);
			int SubId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbCustomername, "Id", "CompanyName", "Customer Name", true);
				DDL.BindDDLNew(dtSupplier, cmbCommAgent, "Id", "CompanyName", "Broker Ac", true);
				DDL.BindDDLNew(dtSupplier, cmbsubbrokeryacc, "Id", "CompanyName", "Broker Ac", true);
				((UltraGridBase)cmbCustomername).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbCommAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbsubbrokeryacc).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbCustomername.Value = suppId;
				}
				else
				{
					((Control)(object)cmbCustomername).Text = string.Empty;
				}
			}
			if (agentId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == agentId))
				{
					cmbCommAgent.Value = agentId;
				}
				else
				{
					((Control)(object)cmbCommAgent).Text = string.Empty;
				}
			}
			if (agentId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubId))
				{
					cmbsubbrokeryacc.Value = SubId;
				}
				else
				{
					((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int PayId = Conversion.ToInt(cmbpayterm.Value);
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, cmbpayterm, "Id", "TermsDescription", "Supplier Name", true);
				((UltraGridBase)cmbpayterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (PayId > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PayId))
				{
					cmbpayterm.Value = PayId;
				}
				else
				{
					((Control)(object)cmbpayterm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			int DeliveryId = Conversion.ToInt(cmbdeliveryterm.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, cmbdeliveryterm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: true);
			((UltraGridBase)cmbdeliveryterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (DeliveryId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DeliveryId))
				{
					cmbdeliveryterm.Value = DeliveryId;
				}
				else
				{
					((Control)(object)cmbdeliveryterm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbcurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbcurrency, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)cmbcurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcurrency.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbcommtype.Value);
			DataTable dt = new DataTable();
			DataTable dtlook = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(4, 0);
			if (dtlook.Rows.Count > 0)
			{
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CommissionType", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtlook.Rows.Count; i++)
				{
					dt.Rows.Add(dtlook.Rows[i]["Id"], dtlook.Rows[i]["LookupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: true);
					((UltraGridBase)cmbcommtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((UltraGridBase)cmbcommtype).DataSource = null;
					((Control)(object)cmbcommtype).Text = string.Empty;
				}
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcommtype.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcommtype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule uom = new UOMSchedule();
			uom.ItemId = Conversion.ToInt((object)ID);
			uom.OrganizationId = UserAccount.OrganizationId;
			uom.CompanyId = UserAccount.CompanyId;
			dtUOM = UOMSchedule.SearchByObject(uom);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			grdsummary.RootTable.Columns["SupplierNameId"].ColumnType = (ColumnType)0;
			grdsummary.RootTable.Columns["SupplierNameId"].HasValueList = true;
			grdsummary.RootTable.Columns["SupplierNameId"].LimitToList = true;
			grdsummary.RootTable.Columns["SupplierNameId"].EditType = (EditType)4;
			grdsummary.RootTable.Columns["SupplierNameId"].Caption = "SupplierName";
			grdsummary.RootTable.Columns["SupplierNameId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdsummary.RootTable.Columns["SupplierNameId"].Width = 250;
			grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "ItemName";
			grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdPackingMaterial.RootTable.Columns["ItemId"].Width = 250;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Caption = "PackingType";
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Width = 250;
			grdDetail.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["PackingTypeId"].Caption = "PackingType";
			grdDetail.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grdDetail.RootTable.Columns["PackingTypeId"].Width = 250;
			grdDetail.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["SupplierAccountId"].Caption = "SupplierAccount";
			grdDetail.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdDetail.RootTable.Columns["SupplierAccountId"].Width = 250;
			grdDetail.RootTable.Columns["PackUomId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["PackUomId"].HasValueList = true;
			grdDetail.RootTable.Columns["PackUomId"].LimitToList = true;
			grdDetail.RootTable.Columns["PackUomId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["PackUomId"].Caption = "UOM";
			grdDetail.RootTable.Columns["PackUomId"].ValueList.PopulateValueList((IEnumerable)dtUOM.DefaultView, "Id", "Equivalent");
			grdDetail.RootTable.Columns["PackUomId"].Width = 250;
			grdDetail.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["ItemId"].HasValueList = true;
			grdDetail.RootTable.Columns["ItemId"].LimitToList = true;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["ItemId"].Caption = "ItemName";
			grdDetail.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdDetail.RootTable.Columns["ItemId"].Width = 250;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridDetailComboFill()
	{
		try
		{
			dtPackingType = InvPackingType.Getall();
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			dtitem = CommonServices.ItemGetForComboServiceBind();
			grdDetail.RootTable.Columns["CropYearId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["CropYearId"].HasValueList = true;
			grdDetail.RootTable.Columns["CropYearId"].LimitToList = true;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["CropYearId"].Caption = "Crop Year";
			grdDetail.RootTable.Columns["CropYearId"].ValueList.PopulateValueList((IEnumerable)dtCrop.DefaultView, "Id", "CropYear");
			grdDetail.RootTable.Columns["CropYearId"].Width = 250;
			grdDetail.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
			grdDetail.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grdDetail.RootTable.Columns["PackingTypeId"].Width = 250;
			grdDetail.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["SupplierAccountId"].Caption = "Supplier Account";
			grdDetail.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdDetail.RootTable.Columns["SupplierAccountId"].Width = 250;
			grdDetail.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["ItemId"].HasValueList = true;
			grdDetail.RootTable.Columns["ItemId"].LimitToList = true;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["ItemId"].Caption = "ItemName";
			grdDetail.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "Item Name");
			grdDetail.RootTable.Columns["ItemId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailsetting()
	{
		try
		{
			GridDetailComboFill();
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderNo"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].Visible = false;
			grdDetail.RootTable.Columns["CustomerAccountId"].Visible = false;
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["SupAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SupAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CusAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CusAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["CusRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["SupRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupPMWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupPMWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupPMWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusPMWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusPMWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusPMWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "Delete";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.RootTable.Columns.Add("Add");
			grdDetail.RootTable.Columns["Add"].Key = "Add";
			grdDetail.RootTable.Columns["Add"].ButtonText = "Add";
			grdDetail.RootTable.Columns["Add"].Caption = "Add";
			grdDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Add"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowIndetail()
	{
		try
		{
			dtDetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdDetail.DataSource = dtsummaryBySupplier;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdDetail.CurrentRow;
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtDetail.Rows.Count == 0)
				{
					AddRowIndetail();
				}
			}
			if (e.Column.Key == "Add" && grdDetail.GetRows().Count() == 0)
			{
				AddRowIndetail();
			}
			GetIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdpackingsetting()
	{
		try
		{
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Caption = "SupplierName";
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "CompanyName");
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Width = 250;
			grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "ItemName";
			grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
			grdPackingMaterial.RootTable.Columns["ItemId"].Width = 250;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].HasValueList = true;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].LimitToList = true;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].Caption = "CreditAccount";
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdPackingMaterial.RootTable.Columns["CreditAccountId"].Width = 250;
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "Add New";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "Add";
			grdPackingMaterial.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterial.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdPackingMaterial.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdPackingMaterial.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdPackingMaterial.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdPackingMaterial.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdPackingMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdPackingMaterial.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdsummary);
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowPackingmaterial()
	{
		try
		{
			dtPackingMaterial.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdPackingMaterial.DataSource = dtsummaryBySupplier;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPackingMaterial.CurrentRow;
				dtPackingMaterial.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPackingMaterial.Rows.Count == 0)
				{
					AddRowPackingmaterial();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowPackingmaterial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowOtherRevenue()
	{
		try
		{
			dtOtherRevenue.Rows.Add(0, 0, 0, 0, 0, 0);
			grdrevenue.DataSource = dtsummaryBySupplier;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdrevenue.CurrentRow;
				dtOtherRevenue.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtOtherRevenue.Rows.Count == 0)
				{
					AddRowOtherRevenue();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowOtherRevenue();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenuesetting()
	{
		try
		{
			grdrevenue.RootTable.Columns["OtherRevenueId"].Visible = false;
			grdrevenue.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
			grdrevenue.RootTable.Columns["SupplierAccountId"].HasValueList = true;
			grdrevenue.RootTable.Columns["SupplierAccountId"].LimitToList = true;
			grdrevenue.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
			grdrevenue.RootTable.Columns["SupplierAccountId"].Caption = "SupplierAccount";
			grdrevenue.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdrevenue.RootTable.Columns["SupplierAccountId"].Width = 250;
			grdrevenue.RootTable.Columns.Add("Delete");
			grdrevenue.RootTable.Columns["Delete"].Key = "Delete";
			grdrevenue.RootTable.Columns["Delete"].Caption = "Delete";
			grdrevenue.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdrevenue.RootTable.Columns.Add("Add");
			grdrevenue.RootTable.Columns["Add"].Key = "Add";
			grdrevenue.RootTable.Columns["Add"].Caption = "Add New";
			grdrevenue.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Add"].ButtonText = "Add";
			grdrevenue.RootTable.Columns["AmountCr"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["AmountCr"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["AmountCr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdrevenue.RootTable.Columns["AmountCr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdrevenue.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdrevenue.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdrevenue.RootTable.Columns["AmountDr"].AggregateFunction = (AggregateFunction)2;
			grdrevenue.RootTable.Columns["AmountDr"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["AmountDr"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdrevenue.RootTable.Columns["AmountDr"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdrevenue.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
			grdrevenue.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdrevenue.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdrevenue.RootTable.Columns["Rate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdsummary);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsummarysetting()
	{
		try
		{
			grdsummary.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["Expenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["Expenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["PackingAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["PackingAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["PackingAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["PackingAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdsummary);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowSummaryBySupplier()
	{
		try
		{
			dtsummaryBySupplier.Rows.Add(0, "", 0, 0, 0);
			grdsummary.DataSource = dtsummaryBySupplier;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsummary_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdsummary.CurrentRow;
				dtsummaryBySupplier.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtsummaryBySupplier.Rows.Count == 0)
				{
					AddRowSummaryBySupplier();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowSummaryBySupplier();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtbiltydates.Text.Trim() == string.Empty || txtbillno.Text.Trim() == "0" || txtbillno.Text.Trim() == null)
		{
			MessageBox.Show("Bill No Field is Required");
			txtbillno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCustomername).ActiveRow == null || Conversion.ToInt(cmbCustomername.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			cmbCustomername.Focus();
			return false;
		}
		if (((UltraGridBase)cmbpayterm).ActiveRow == null || Conversion.ToInt(cmbpayterm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			cmbpayterm.Focus();
			return false;
		}
		if (((Control)(object)cmbpayterm).Text == "Credit" && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required when Payment Term is Credit");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeliveryterm).ActiveRow == null || Conversion.ToInt(cmbdeliveryterm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			cmbdeliveryterm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtexchangerate.Focus();
				return false;
			}
			if (txtfcyamount.Text.Trim() == string.Empty || txtfcyamount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtfcyamount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtexchangerate.Focus();
				return false;
			}
		}
		return true;
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			PaymentTerms();
			suppliercustomer();
			CurrencyFill();
			CommissionTypeFill();
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
			RecId = 0;
			((Control)(object)cmbCustomername).Text = string.Empty;
			txtcustbillamnt.Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)cmbpayterm).Text = string.Empty;
			((Control)(object)cmbdeliveryterm).Text = string.Empty;
			txtbilldate.Value = DateTime.Now;
			txtduedays.Text = string.Empty;
			txtmanualref.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtbiltyno.Text = string.Empty;
			txtbiltydates.Text = string.Empty;
			txtcustbillamnt.Text = string.Empty;
			txtexchangerate.Text = string.Empty;
			txtfcyamount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)cmbCommAgent).Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtbrokeryamount.Text = string.Empty;
			txtbrokeryrate.Text = string.Empty;
			((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
			txtsubbrokeryamnt.Text = string.Empty;
			txtsubbrokeryrate.Text = string.Empty;
			cmbCustomername.Focus();
			btnSave.Enabled = true;
			btnUpdate.Enabled = false;
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

	private void CommisionBill_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdrevenue).Name);
			if (fs != null)
			{
				grdrevenue.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistoryDetail).Name);
			if (fs != null)
			{
				grdhistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPackingMaterial).Name);
			if (fs != null)
			{
				grdPackingMaterial.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void CommisionBill_KeyDown(object sender, KeyEventArgs e)
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
					cmbCustomername.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
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
				if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbCustomername.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdDetail).Focused)
					{
						((Control)(object)grdPackingMaterial).Focus();
					}
					else if (((Control)(object)grdPackingMaterial).Focused)
					{
						((Control)(object)grdrevenue).Focus();
					}
					else
					{
						((Control)(object)grdDetail).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbCustomername.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode != Keys.P)
				{
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdhistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdhistoryDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdhistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
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

	private void BtnLoadFromLoadingDelivery_Click(object sender, EventArgs e)
	{
		try
		{
			LoadFromLoadingDelivery = new frmLoadFromLoadingDelivery(UserAccount);
			if (LoadDeliveryDetailIds != string.Empty)
			{
				LoadFromLoadingDelivery.orderMainIds = LoadDeliveryMainIds;
			}
			LoadFromLoadingDelivery.ShowDialog();
			LoadFromLoadingDelivery.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		try
		{
			string GdnIds = "";
			if (Conversion.ToInt((object)LoadFromLoadingDelivery.dtGDNS.Rows[0][0].ToString()) == 0)
			{
				return;
			}
			for (int j = 0; j < LoadFromLoadingDelivery.dtGDNS.Rows.Count; j++)
			{
				GdnIds = GdnIds + "," + int.Parse(LoadFromLoadingDelivery.dtGDNS.Rows[j][0].ToString());
			}
			DataTable dtLoaderData = CommTradeLoadingDelivery.PendingCommTradeLoadingDeliveryForBill(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1701,
				GdnIds = GdnIds
			});
			if (dtLoaderData.Rows.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < dtLoaderData.Rows.Count; i++)
				{
					if (grdDetail.GetRows().Length != 0)
					{
						GridEXRow[] rows = grdDetail.GetRows();
						for (int k = 0; k < rows.Length; k++)
						{
							if (Conversion.ToInt(rows[k].Cells["LoadingDeliveryDetailId"].Value) == Conversion.ToInt(dtLoaderData.Rows[i]["DetailId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtDetail.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["OrderId"]), Conversion.ToInt(dtLoaderData.Rows[i]["OrderDetailId"]), dtLoaderData.Rows[i]["OrderNo"], Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["DetailId"]), Conversion.ToInt(dtLoaderData.Rows[i]["ItemId"]), dtLoaderData.Rows[i]["ItemCode"], Conversion.ToInt(dtLoaderData.Rows[i]["CropYearId"]), Conversion.ToInt(dtLoaderData.Rows[i]["PackingTypeId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["PackUomEquivalent"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToInt(dtLoaderData.Rows[i]["CustomerId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusNetWeight"]), 0, 0, 0, 0, Conversion.ToDecimal(dtLoaderData.Rows[i]["SupGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupNetWeight"]), 0, 0, 0, 0, Conversion.ToInt(dtLoaderData.Rows[i]["SupplierId"]));
						flag = false;
						grdDetail.DataSource = dtDetail;
						grdDetail.RetrieveStructure();
					}
				}
				grdDetailsetting();
				GetIds();
			}
			else
			{
				Reset();
			}
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetIds()
	{
		try
		{
			LoadDeliveryDetailIds = "";
			LoadDeliveryMainIds = "";
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				LoadDeliveryDetailIds = LoadDeliveryDetailIds + "," + Conversion.ToInt(item.Cells["LoadingDeliveryDetailId"].Value);
				LoadDeliveryMainIds = LoadDeliveryMainIds + "," + Conversion.ToInt(item.Cells["LoadingDeliveryId"].Value);
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
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Expected O, but got Unknown
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bc: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Expected O, but got Unknown
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_4d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d79: Expected O, but got Unknown
		//IL_4eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee4: Expected O, but got Unknown
		//IL_4fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fd2: Expected O, but got Unknown
		//IL_52e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ee: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.TransactionForms.CommisionBillDirect));
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
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
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance18 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		GridEXLayout grdrevenue_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdsummary_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrintVoucher = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadFromLoadingDelivery = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtfcyamount = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtsubbrokeryamnt = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtsubbrokeryrate = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbsubbrokeryacc = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtbrokeryamount = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtbrokeryrate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbcommtype = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCommAgent = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtbiltydates = new System.Windows.Forms.TextBox();
		this.txtexchangerate = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.txtmanualref = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbpayterm = new UltraCombo();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtbilldate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.cmbcurrency = new UltraCombo();
		this.cmbCustomername = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbdeliveryterm = new UltraCombo();
		this.txtcustbillamnt = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label29 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label28 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdrevenue = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdPackingMaterial = new GridEX();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdsummary = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.History = new System.Windows.Forms.TabPage();
		this.grdhistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label31 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAllHistory = new System.Windows.Forms.ToolStripButton();
		this.label16 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsummary).BeginInit();
		this.panel4.SuspendLayout();
		this.panel17.SuspendLayout();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 28);
		this.panel2.TabIndex = 1;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.btnUpdate, this.btnSave, this.btnAttachment, this.btnPrintVoucher, this.BtnPrint, this.btnDelete, this.btnRefresh, this.BtnLoadFromLoadingDelivery, this.btnShortcutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrintVoucher.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintVoucher.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintVoucher.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintVoucher.Name = "btnPrintVoucher";
		this.btnPrintVoucher.Size = new System.Drawing.Size(61, 24);
		this.btnPrintVoucher.Text = "Print";
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(61, 24);
		this.BtnPrint.Text = "Print";
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnLoadFromLoadingDelivery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnLoadFromLoadingDelivery.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadFromLoadingDelivery.Image = (System.Drawing.Image)resources.GetObject("BtnLoadFromLoadingDelivery.Image");
		this.BtnLoadFromLoadingDelivery.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadFromLoadingDelivery.Name = "BtnLoadFromLoadingDelivery";
		this.BtnLoadFromLoadingDelivery.Size = new System.Drawing.Size(191, 24);
		this.BtnLoadFromLoadingDelivery.Text = "Load From Loading Delivery";
		this.BtnLoadFromLoadingDelivery.Click += new System.EventHandler(BtnLoadFromLoadingDelivery_Click);
		this.btnShortcutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortcutKeys.Image");
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.groupBox1.Controls.Add(this.txtfcyamount);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.groupBox2);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtbiltydates);
		this.groupBox1.Controls.Add(this.txtexchangerate);
		this.groupBox1.Controls.Add(this.txtbiltyno);
		this.groupBox1.Controls.Add(this.txtmanualref);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpayterm);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtbilldate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtbillno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomername);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliveryterm);
		this.groupBox1.Controls.Add(this.txtcustbillamnt);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(885, 240);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtfcyamount.BackColor = System.Drawing.Color.White;
		this.txtfcyamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfcyamount.Location = new System.Drawing.Point(394, 214);
		this.txtfcyamount.Name = "txtfcyamount";
		this.txtfcyamount.ReadOnly = true;
		this.txtfcyamount.Size = new System.Drawing.Size(131, 23);
		this.txtfcyamount.TabIndex = 14;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(76, 158);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(229, 79);
		this.txtRemarks.TabIndex = 6;
		this.txtRemarks.Text = "";
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.txtsubbrokeryamnt);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtsubbrokeryrate);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.txtbrokeryamount);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtbrokeryrate);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtcommamount);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtcommrate);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommAgent);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(530, 10);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(352, 208);
		this.groupBox2.TabIndex = 15;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Commission & Brokery";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(168, 179);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(73, 15);
		this.label27.TabIndex = 33;
		this.label27.Text = "Brokery Amt";
		this.txtsubbrokeryamnt.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryamnt.Enabled = false;
		this.txtsubbrokeryamnt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryamnt.Location = new System.Drawing.Point(241, 175);
		this.txtsubbrokeryamnt.Name = "txtsubbrokeryamnt";
		this.txtsubbrokeryamnt.ReadOnly = true;
		this.txtsubbrokeryamnt.Size = new System.Drawing.Size(108, 23);
		this.txtsubbrokeryamnt.TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1, 179);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(73, 15);
		this.label26.TabIndex = 31;
		this.label26.Text = "Brokery Rate";
		this.txtsubbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryrate.Location = new System.Drawing.Point(88, 175);
		this.txtsubbrokeryrate.Name = "txtsubbrokeryrate";
		this.txtsubbrokeryrate.ReadOnly = true;
		this.txtsubbrokeryrate.Size = new System.Drawing.Size(78, 23);
		this.txtsubbrokeryrate.TabIndex = 7;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1, 150);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 15);
		this.label25.TabIndex = 29;
		this.label25.Text = "Sub Broker A/c";
		this.cmbsubbrokeryacc.AllowNull = (DefaultableBoolean)1;
		this.cmbsubbrokeryacc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsubbrokeryacc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsubbrokeryacc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsubbrokeryacc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Location = new System.Drawing.Point(88, 144);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Name = "cmbsubbrokeryacc";
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1, 110);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 15);
		this.label19.TabIndex = 27;
		this.label19.Text = "Brokery Amt";
		this.txtbrokeryamount.BackColor = System.Drawing.Color.White;
		this.txtbrokeryamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryamount.Enabled = false;
		this.txtbrokeryamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryamount.Location = new System.Drawing.Point(88, 106);
		this.txtbrokeryamount.Name = "txtbrokeryamount";
		this.txtbrokeryamount.ReadOnly = true;
		this.txtbrokeryamount.Size = new System.Drawing.Size(108, 23);
		this.txtbrokeryamount.TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(197, 84);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(73, 15);
		this.label18.TabIndex = 25;
		this.label18.Text = "Brokery Rate";
		this.txtbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryrate.Location = new System.Drawing.Point(270, 80);
		this.txtbrokeryrate.Name = "txtbrokeryrate";
		this.txtbrokeryrate.ReadOnly = true;
		this.txtbrokeryrate.Size = new System.Drawing.Size(78, 23);
		this.txtbrokeryrate.TabIndex = 4;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1, 55);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(74, 15);
		this.label15.TabIndex = 21;
		this.label15.Text = "Comm. Type";
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(88, 49);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1, 84);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 19;
		this.label14.Text = "Comm Amt";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(88, 80);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(108, 23);
		this.txtcommamount.TabIndex = 3;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(197, 55);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 17;
		this.label13.Text = "Comm Rate";
		this.txtcommrate.BackColor = System.Drawing.Color.White;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.Location = new System.Drawing.Point(270, 51);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.ReadOnly = true;
		this.txtcommrate.Size = new System.Drawing.Size(78, 23);
		this.txtcommrate.TabIndex = 2;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1, 27);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "Comm Agent";
		this.cmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Location = new System.Drawing.Point(88, 21);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Name = "cmbCommAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Size = new System.Drawing.Size(260, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(4, 168);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 26;
		this.label11.Text = "Remarks";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(310, 218);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(72, 15);
		this.label23.TabIndex = 35;
		this.label23.Text = "Fcy Amount";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 136);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(63, 15);
		this.label10.TabIndex = 24;
		this.label10.Text = "Bilty Dates";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(4, 109);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 21;
		this.label9.Text = "Bilty Nos.";
		this.txtbiltydates.BackColor = System.Drawing.Color.White;
		this.txtbiltydates.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltydates.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltydates.Location = new System.Drawing.Point(76, 132);
		this.txtbiltydates.Name = "txtbiltydates";
		this.txtbiltydates.ReadOnly = true;
		this.txtbiltydates.Size = new System.Drawing.Size(229, 23);
		this.txtbiltydates.TabIndex = 5;
		this.txtexchangerate.BackColor = System.Drawing.Color.White;
		this.txtexchangerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangerate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtexchangerate.Location = new System.Drawing.Point(394, 187);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.ReadOnly = true;
		this.txtexchangerate.Size = new System.Drawing.Size(131, 23);
		this.txtexchangerate.TabIndex = 13;
		this.txtexchangerate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtbiltyno.BackColor = System.Drawing.Color.White;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyno.Location = new System.Drawing.Point(76, 105);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.ReadOnly = true;
		this.txtbiltyno.Size = new System.Drawing.Size(229, 23);
		this.txtbiltyno.TabIndex = 4;
		this.txtmanualref.BackColor = System.Drawing.Color.White;
		this.txtmanualref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualref.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualref.Location = new System.Drawing.Point(394, 19);
		this.txtmanualref.Name = "txtmanualref";
		this.txtmanualref.ReadOnly = true;
		this.txtmanualref.Size = new System.Drawing.Size(131, 23);
		this.txtmanualref.TabIndex = 7;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(310, 191);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 33;
		this.label22.Text = "Exchange Rate";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 80);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(72, 15);
		this.label8.TabIndex = 19;
		this.label8.Text = "Vehicle Nos.";
		this.cmbpayterm.AllowNull = (DefaultableBoolean)1;
		this.cmbpayterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpayterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpayterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpayterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Location = new System.Drawing.Point(394, 45);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Name = "cmbpayterm";
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).TabIndex = 8;
		this.txtvehicleno.BackColor = System.Drawing.Color.White;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(76, 76);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.ReadOnly = true;
		this.txtvehicleno.Size = new System.Drawing.Size(229, 23);
		this.txtvehicleno.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(310, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 9;
		this.label2.Text = "Payment Term";
		this.txtbilldate.CustomFormat = "dd-MMM-yy";
		this.txtbilldate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbilldate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtbilldate.Location = new System.Drawing.Point(193, 19);
		this.txtbilldate.Name = "txtbilldate";
		this.txtbilldate.Size = new System.Drawing.Size(112, 23);
		this.txtbilldate.TabIndex = 1;
		this.txtbilldate.TabStop = false;
		this.txtduedays.BackColor = System.Drawing.Color.White;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(394, 76);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.ReadOnly = true;
		this.txtduedays.Size = new System.Drawing.Size(131, 23);
		this.txtduedays.TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(142, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 7;
		this.label4.Text = "Bill Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(310, 164);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(55, 15);
		this.label21.TabIndex = 31;
		this.label21.Text = "Fcy Code";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(42, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Bill No";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(310, 80);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 15);
		this.label3.TabIndex = 11;
		this.label3.Text = "Due Days";
		this.txtbillno.BackColor = System.Drawing.Color.White;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(76, 19);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.ReadOnly = true;
		this.txtbillno.Size = new System.Drawing.Size(65, 23);
		this.txtbillno.TabIndex = 0;
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(394, 158);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 12;
		this.cmbCustomername.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Location = new System.Drawing.Point(76, 45);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Name = "cmbCustomername";
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(310, 136);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(75, 15);
		this.label20.TabIndex = 29;
		this.label20.Text = "Cust Bill Amt";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 51);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 5;
		this.label7.Text = "Customer";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(310, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 15;
		this.label5.Text = "Manual Ref";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(310, 109);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 17;
		this.label6.Text = "Delivery Term";
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(394, 103);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(131, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 10;
		this.txtcustbillamnt.BackColor = System.Drawing.Color.White;
		this.txtcustbillamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustbillamnt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcustbillamnt.Location = new System.Drawing.Point(394, 132);
		this.txtcustbillamnt.Name = "txtcustbillamnt";
		this.txtcustbillamnt.ReadOnly = true;
		this.txtcustbillamnt.Size = new System.Drawing.Size(131, 23);
		this.txtcustbillamnt.TabIndex = 11;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label29);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(584, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(586, 24);
		this.panel10.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(539, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar1.TabIndex = 43;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(3, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(99, 14);
		this.label29.TabIndex = 40;
		this.label29.Text = "Other Revenue";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar7);
		this.panel9.Controls.Add(this.label28);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(584, 24);
		this.panel9.TabIndex = 4;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(541, -1);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar7.MyGrid = null;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar7.TabIndex = 43;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 3);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(100, 14);
		this.label28.TabIndex = 40;
		this.label28.Text = "Packing Matrial";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 43;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 752);
		this.tabControl1.TabIndex = 6;
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.Form.Controls.Add(this.panel12);
		this.Form.Controls.Add(this.panel1);
		this.Form.Controls.Add(this.panel17);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(3);
		this.Form.Size = new System.Drawing.Size(1176, 723);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdrevenue);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 561);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 159);
		this.panel5.TabIndex = 46;
		((ScrollableBase)this.grdrevenue).BackColor = System.Drawing.Color.White;
		grdrevenue_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdrevenue.DesignTimeLayout = grdrevenue_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdrevenue.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Location = new System.Drawing.Point(584, 24);
		((System.Windows.Forms.Control)(object)this.grdrevenue).Name = "grdrevenue";
		((ScrollableBase)this.grdrevenue).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Size = new System.Drawing.Size(586, 135);
		((System.Windows.Forms.Control)(object)this.grdrevenue).TabIndex = 0;
		this.grdrevenue.TotalRow = (InheritableBoolean)1;
		this.grdrevenue.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdrevenue).VisualStyle = (VisualStyle)4;
		this.grdrevenue.ColumnButtonClick += new ColumnActionEventHandler(grdrevenue_ColumnButtonClick);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(584, 159);
		this.panel7.TabIndex = 1;
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(584, 135);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 0;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 319);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1170, 242);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label30);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 296);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 23);
		this.panel12.TabIndex = 44;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(42, 14);
		this.label30.TabIndex = 40;
		this.label30.Text = "Detail";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 56);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 240);
		this.panel1.TabIndex = 10;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdsummary);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(885, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(285, 240);
		this.panel3.TabIndex = 10;
		this.grdsummary.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdsummary).BackColor = System.Drawing.Color.White;
		grdsummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsummary.DesignTimeLayout = grdsummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsummary).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdsummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsummary).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdsummary).Name = "grdsummary";
		((ScrollableBase)this.grdsummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsummary).Size = new System.Drawing.Size(285, 218);
		((System.Windows.Forms.Control)(object)this.grdsummary).TabIndex = 0;
		this.grdsummary.TotalRow = (InheritableBoolean)1;
		this.grdsummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsummary).VisualStyle = (VisualStyle)4;
		this.grdsummary.ColumnButtonClick += new ColumnActionEventHandler(grdsummary_ColumnButtonClick);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label24);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(285, 22);
		this.panel4.TabIndex = 3;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(3, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(143, 14);
		this.label24.TabIndex = 40;
		this.label24.Text = "Summary By Suppliers";
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label1);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 31);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1170, 25);
		this.panel17.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(137, 19);
		this.label1.TabIndex = 40;
		this.label1.Text = "Commission Bill";
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryDetail);
		this.History.Controls.Add(this.panel8);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.History.Controls.Add(this.panel6);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(3);
		this.History.Size = new System.Drawing.Size(1176, 723);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdhistoryDetail).BackColor = System.Drawing.Color.White;
		grdhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryDetail.DesignTimeLayout = grdhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Location = new System.Drawing.Point(3, 570);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Name = "grdhistoryDetail";
		((ScrollableBase)this.grdhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Size = new System.Drawing.Size(1170, 150);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).TabIndex = 47;
		this.grdhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label31);
		this.panel8.Controls.Add(this.ctrlGrdBar4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 545);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 25);
		this.panel8.TabIndex = 48;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(3, 6);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(215, 14);
		this.label31.TabIndex = 40;
		this.label31.Text = "Detail of the Above Selected Row";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = null;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar4.TabIndex = 43;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 61);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1170, 484);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 45;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 58);
		this.panel6.TabIndex = 46;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnLoadAllHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip2.TabIndex = 44;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnLoadAllHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAllHistory.Name = "BtnLoadAllHistory";
		this.BtnLoadAllHistory.Size = new System.Drawing.Size(82, 24);
		this.BtnLoadAllHistory.Text = "&Load All";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 32);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(200, 19);
		this.label16.TabIndex = 40;
		this.label16.Text = "Commission Bill History";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1132, 34);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 43;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1184, 752);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommisionBillDirect";
		this.Text = "CommisionBill";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(CommisionBill_FormClosed);
		base.Load += new System.EventHandler(CommisionBill_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommisionBill_KeyDown);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).EndInit();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsummary).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
