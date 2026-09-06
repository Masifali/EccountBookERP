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
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.CmTr.TransactionForms;

public class CommisionBillDirectAndAgainstOrder : Form
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

	private DataTable dtCommission = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtgrid = new DataTable();

	private DataTable dtitem = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private bool HasMultiCurrencyFeature;

	private int RecId;

	private bool UpdateMode;

	private frmLoadFromLoadingDelivery LoadFromLoadingDelivery;

	public string LoadDeliveryDetailIds = "";

	public string LoadDeliveryMainIds = "";

	private List<CommTradeTransactionDetail> lstRemoveRecord = new List<CommTradeTransactionDetail>();

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

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label4;

	private TabControl tabControl1;

	private TabPage Form;

	internal GridEX grdrevenue;

	internal GridEX grdPackingMaterial;

	private Panel panel1;

	private Panel panel3;

	internal GridEX grdsummary;

	private Panel panel4;

	private Label label24;

	private Panel panel17;

	private Label label1;

	private TabPage History;

	internal GridEX grdHistoryDetail;

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

	private Panel panel5;

	private Panel panel10;

	private Label label29;

	private Panel panel7;

	private Panel panel9;

	private Label label28;

	internal GridEX grdDetail;

	private Panel panel12;

	private Label label30;

	private GroupBox PanelDetail;

	private Button btnAddDetail;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Label label35;

	private TextBox txtcustnetwt;

	private Label label32;

	private TextBox txtcustadlswt;

	private Label label37;

	private TextBox txtcustpckmtrlwt;

	private Label label38;

	private TextBox txtcustgrosswt;

	private GroupBox groupBox5;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	internal UltraCombo cmbitem;

	private Label label33;

	private TextBox txtItemQty;

	internal UltraCombo cmbuom;

	internal UltraCombo cmbcropYear;

	internal UltraCombo CmbPackingType;

	private Label label36;

	private Label labItemName;

	private Label label40;

	private Label label41;

	private Label label42;

	private TextBox txtsupremarks;

	private Label label43;

	private TextBox txtSupNetWeight;

	private Label label44;

	private TextBox txtSupAdLsWt;

	private Label label45;

	private TextBox txtSupPckMtrlWeight;

	private Label label46;

	private TextBox txtsupgrosswt;

	internal UltraCombo cmbsupplier;

	private Label label47;

	private Label label39;

	private TextBox txtsupamount;

	private Label label48;

	private TextBox txtsuprate;

	private Label label34;

	private Label label49;

	private TextBox txtcusamount;

	private TextBox txtcusrate;

	public CommisionBillDirectAndAgainstOrder(UserAccount user)
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
			BtnPrint.Enabled = formright.DoHavePrintRights;
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
			dtCommission.Columns.Add("Id", typeof(int));
			dtCommission.Columns.Add("CommissionAgentId", typeof(int));
			dtCommission.Columns.Add("ChargesIncentiveId", typeof(int));
			dtCommission.Columns.Add("RevenueExpenseId", typeof(int));
			dtCommission.Columns.Add("CalcParameterId", typeof(int));
			dtCommission.Columns.Add("Rate", typeof(decimal));
			dtCommission.Columns.Add("Amount", typeof(decimal));
			dtCommission.Columns.Add("SupplierCustomerId", typeof(int));
			dtsummaryBySupplier.Columns.Add("SupplierId", typeof(int));
			dtsummaryBySupplier.Columns.Add("ItemAmount", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("PackingAmount", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("Expenses", typeof(decimal));
			dtsummaryBySupplier.Columns.Add("BillAmount", typeof(decimal));
			grdsummary.DataSource = dtsummaryBySupplier;
			grdsummary.RetrieveStructure();
			grdsummarysetting();
			dtPackingMaterial.Columns.Add("Id", typeof(int));
			dtPackingMaterial.Columns.Add("PackingTypeId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemId", typeof(int));
			dtPackingMaterial.Columns.Add("ItemSize", typeof(string));
			dtPackingMaterial.Columns.Add("Weight", typeof(double));
			dtPackingMaterial.Columns.Add("Qty", typeof(double));
			dtPackingMaterial.Columns.Add("Rate", typeof(double));
			dtPackingMaterial.Columns.Add("Amount", typeof(double));
			dtPackingMaterial.Columns.Add("CreditAccountId", typeof(int));
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdPackingMaterial.RetrieveStructure();
			AddRowPackingmaterial();
			grdpackingsetting();
			dtOtherRevenue.Columns.Add("Id", typeof(int));
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
			dtDetail.Columns.Add("UomId", typeof(int));
			dtDetail.Columns.Add("Uom", typeof(string));
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("CustomerAccountId", typeof(int));
			dtDetail.Columns.Add("CusGrossWeight", typeof(double));
			dtDetail.Columns.Add("CusPMWeight", typeof(double));
			dtDetail.Columns.Add("CusAdlsWeight", typeof(double));
			dtDetail.Columns.Add("CusNetWeight", typeof(double));
			dtDetail.Columns.Add("CusRate", typeof(double));
			dtDetail.Columns.Add("CusAmount", typeof(double));
			dtDetail.Columns.Add("CusFcyAmount", typeof(double));
			dtDetail.Columns.Add("SupplierAccountId", typeof(int));
			dtDetail.Columns.Add("SupGrossWeight", typeof(double));
			dtDetail.Columns.Add("SupPMWeight", typeof(double));
			dtDetail.Columns.Add("SupAdlsWeight", typeof(double));
			dtDetail.Columns.Add("SupNetWeight", typeof(double));
			dtDetail.Columns.Add("SupRate", typeof(double));
			dtDetail.Columns.Add("SupAmount", typeof(double));
			dtDetail.Columns.Add("SupFcyAmount", typeof(double));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdDetailsetting();
			DocumentNoFill();
			PaymentTerms();
			suppliercustomer();
			DeliveryTerm();
			CurrencyFill();
			CommissionTypeFill();
			multiCurrencyFeature();
			ConfigurationDefault();
			txtbilldate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
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
			int CurrId = Conversion.ToInt(cmbcurrency.Value);
			decimal CurrRate = Conversion.ToDecimal((object)txtexchangerate.Text);
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
			if (HasMultiCurrencyFeature)
			{
				if (CurrId > 0)
				{
					cmbcurrency.Value = CurrId;
				}
				if (CurrRate > 0m)
				{
					txtexchangerate.Text = CurrRate.ToString();
				}
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
			CalculateUpdateFcyInDetailGrid();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = CommTradeTransactionHeader.GenerateCode(new CommTradeTransactionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1704,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtbillno.Text = code.ToString();
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
			DataTable dtTerm = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(5, 0);
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, cmbpayterm, "Id", "LookUpName", "Payment Term", true);
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
			grdDetail.RootTable.Columns["CustomerAccountId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["CustomerAccountId"].HasValueList = true;
			grdDetail.RootTable.Columns["CustomerAccountId"].LimitToList = true;
			grdDetail.RootTable.Columns["CustomerAccountId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["CustomerAccountId"].Caption = "Customer Account";
			grdDetail.RootTable.Columns["CustomerAccountId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdDetail.RootTable.Columns["CustomerAccountId"].Width = 250;
			grdDetail.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["ItemId"].HasValueList = true;
			grdDetail.RootTable.Columns["ItemId"].LimitToList = true;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdDetail.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
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
			GridDetailSupplierForRevenueGrid();
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["OrderNo"].Visible = false;
			grdDetail.RootTable.Columns["OrderDetailId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["CusFcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["SupFcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["LoadingDeliveryNo"].Caption = "LoadingNo";
			grdDetail.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grdDetail.RootTable.Columns["CusAdlsWeight"].Caption = "Adls Weight";
			grdDetail.RootTable.Columns["CusPMWeight"].Caption = "PM Weight";
			grdDetail.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grdDetail.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grdDetail.RootTable.Columns["SupAdlsWeight"].Caption = "Adls Weight";
			grdDetail.RootTable.Columns["SupPMWeight"].Caption = "PM Weight";
			grdDetail.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LoadingDeliveryNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CustomerAccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusGrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusAdlsWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusPMWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CusFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupFcyAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupGrossWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupAdlsWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupPMWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupNetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["SupplierAccountId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Uom"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UomId"].EditType = (EditType)0;
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
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].Width = 30;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grdDetail.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					CommTradeTransactionDetail vd = new CommTradeTransactionDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.CommTradeOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.CommTradeOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r.Cells["LoadingDeliveryId"].Value);
					vd.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r.Cells["LoadingDeliveryDetailId"].Value);
					vd.dDocDate = Conversion.ToDateTime((object)txtbilldate.Value);
					vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
					vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
					vd.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
					vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
					vd.CustFcyAmount = Conversion.ToDecimal(r.Cells["CusFcyAmount"].Value);
					vd.SupFcyAmount = Conversion.ToDecimal(r.Cells["SupFcyAmount"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
					vd.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierAccountId"].Value);
					vd.SupGrossWeight = Conversion.ToDecimal(r.Cells["SupGrossWeight"].Value);
					vd.SupPckMtrlWeight = Conversion.ToDecimal(r.Cells["SupPMWeight"].Value);
					vd.SupAdLsWt = Conversion.ToDecimal(r.Cells["SupAdLsWeight"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
					vd.CusGrossWeight = Conversion.ToDecimal(r.Cells["CusGrossWeight"].Value);
					vd.CusPckMtrlWeight = Conversion.ToDecimal(r.Cells["CusPMWeight"].Value);
					vd.CusAdLsWt = Conversion.ToDecimal(r.Cells["CusAdLsWeight"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			GetIds();
			txtexchangerate_TextChanged(null, null);
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "CusRate" && item != null && (int)item.RowType == 0)
			{
				int CusRateUom = 40;
				string CusRate = item.Cells["CusRate"].Value.ToString();
				double CusAmount = Conversion.ToDouble((object)item.Cells["CusNetWeight"].Value.ToString()) / Conversion.ToDouble((object)CusRateUom) * Conversion.ToDouble((object)CusRate);
				item.Cells["CusAmount"].Value = Math.Round(CusAmount, 0, MidpointRounding.AwayFromZero);
				item.Cells["SupRate"].Value = CusRate;
				double SupAmount2 = Conversion.ToDouble((object)item.Cells["SupNetWeight"].Value.ToString()) / Conversion.ToDouble((object)CusRateUom) * Conversion.ToDouble((object)CusRate);
				item.Cells["SupAmount"].Value = Math.Round(SupAmount2, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "SupRate" && item != null && (int)item.RowType == 0)
			{
				int SupRateUom = 40;
				string SupRate = item.Cells["SupRate"].Value.ToString();
				double SupAmount = Conversion.ToDouble((object)item.Cells["SupNetWeight"].Value.ToString()) / Conversion.ToDouble((object)SupRateUom) * Conversion.ToDouble((object)SupRate);
				item.Cells["SupAmount"].Value = Math.Round(SupAmount, 0, MidpointRounding.AwayFromZero);
			}
			grdDetail.UpdateData();
			txtexchangerate_TextChanged(null, null);
			CustomerBillAmount();
			TotalCommissionAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPackingMaterialCombobind()
	{
		try
		{
			DataTable dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15");
			dtPackingType = InvPackingType.Getall();
			dtitem = CommonServices.GetItemByItemTypeId("14");
			if (dtPackingType.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].Caption = "Packing Type";
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
				grdPackingMaterial.RootTable.Columns["PackingTypeId"].Width = 250;
			}
			if (dtitem.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["ItemId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["ItemId"].EditType = (EditType)4;
				grdPackingMaterial.RootTable.Columns["ItemId"].Caption = "Item Name";
				grdPackingMaterial.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtitem.DefaultView, "Id", "ItemName");
				grdPackingMaterial.RootTable.Columns["ItemId"].Width = 250;
			}
			if (dtAccounts.Rows.Count > 0)
			{
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ColumnType = (ColumnType)0;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].HasValueList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].LimitToList = true;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].EditType = (EditType)4;
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].Caption = "CreditAccount";
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdPackingMaterial.RootTable.Columns["CreditAccountId"].Width = 250;
			}
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
			GridPackingMaterialCombobind();
			grdPackingMaterial.RootTable.Columns["Id"].EditType = (EditType)0;
			grdPackingMaterial.RootTable.Columns["Id"].Visible = false;
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
			CommonServices.GridAutoAdjustmentNew(grdPackingMaterial);
			grdPackingMaterial.RootTable.Columns.Add("Delete");
			grdPackingMaterial.RootTable.Columns["Delete"].Key = "Delete";
			grdPackingMaterial.RootTable.Columns["Delete"].Caption = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonText = "X";
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Delete"].Width = 30;
			grdPackingMaterial.RootTable.Columns["Delete"].Position = 0;
			grdPackingMaterial.RootTable.Columns.Add("Add");
			grdPackingMaterial.RootTable.Columns["Add"].Key = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].Caption = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonText = "Add";
			grdPackingMaterial.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPackingMaterial.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPackingMaterial.RootTable.Columns["Add"].Width = 40;
			grdPackingMaterial.RootTable.Columns["Add"].Position = 1;
			grdPackingMaterial.FrozenColumns = 2;
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
			dtPackingMaterial.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0);
			grdPackingMaterial.DataSource = dtPackingMaterial;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPackingMaterial.UpdateData();
			GridEXRow item = grdPackingMaterial.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["Amount"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			grdPackingMaterial.UpdateData();
			CustomerBillAmount();
			FillSummaryGrid();
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
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSupplierForRevenueGrid()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("SupplierId", typeof(int));
			dt.Columns.Add("SupplierName", typeof(string));
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("SupplierId") == Conversion.ToInt(item.Cells["SupplierAccountId"].Value)))
				{
					dt.Rows.Add(Conversion.ToInt(item.Cells["SupplierAccountId"].Value), Conversion.ToString((object)item.Cells["SupplierAccountId"].Text));
				}
			}
			if (dt.Rows.Count > 0)
			{
				grdrevenue.RootTable.Columns["SupplierAccountId"].ColumnType = (ColumnType)0;
				grdrevenue.RootTable.Columns["SupplierAccountId"].HasValueList = true;
				grdrevenue.RootTable.Columns["SupplierAccountId"].LimitToList = true;
				grdrevenue.RootTable.Columns["SupplierAccountId"].EditType = (EditType)4;
				grdrevenue.RootTable.Columns["SupplierAccountId"].Caption = "Supplier Name";
				grdrevenue.RootTable.Columns["SupplierAccountId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "SupplierId", "SupplierName");
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
			dtOtherRevenue.Rows.Add(0, 0, 0, 0, 0, 0, 0);
			grdrevenue.DataSource = dtOtherRevenue;
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
			FillSummaryGrid();
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
			GridDetailSupplierForRevenueGrid();
			grdrevenue.RootTable.Columns["Id"].EditType = (EditType)0;
			grdrevenue.RootTable.Columns["Id"].Visible = false;
			DataTable ReveneuExpenseAccount = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(6, 0);
			grdrevenue.RootTable.Columns["OtherRevenueId"].ColumnType = (ColumnType)0;
			grdrevenue.RootTable.Columns["OtherRevenueId"].HasValueList = true;
			grdrevenue.RootTable.Columns["OtherRevenueId"].LimitToList = true;
			grdrevenue.RootTable.Columns["OtherRevenueId"].EditType = (EditType)4;
			grdrevenue.RootTable.Columns["OtherRevenueId"].Caption = "Other Revenue";
			grdrevenue.RootTable.Columns["OtherRevenueId"].ValueList.PopulateValueList((IEnumerable)ReveneuExpenseAccount.DefaultView, "Id", "LookUpName");
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
			CommonServices.GridAutoAdjustmentNew(grdrevenue);
			grdrevenue.RootTable.Columns.Add("Delete");
			grdrevenue.RootTable.Columns["Delete"].Key = "Delete";
			grdrevenue.RootTable.Columns["Delete"].ButtonText = "X";
			grdrevenue.RootTable.Columns["Delete"].Caption = "X";
			grdrevenue.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Delete"].Width = 40;
			grdrevenue.RootTable.Columns["Delete"].Position = 0;
			grdrevenue.RootTable.Columns.Add("Add");
			grdrevenue.RootTable.Columns["Add"].Key = "Add";
			grdrevenue.RootTable.Columns["Add"].ButtonText = "Add";
			grdrevenue.RootTable.Columns["Add"].Caption = "Add";
			grdrevenue.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdrevenue.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdrevenue.RootTable.Columns["Add"].Width = 40;
			grdrevenue.RootTable.Columns["Add"].Position = 1;
			grdrevenue.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdrevenue_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdrevenue.UpdateData();
			GridEXRow item = grdrevenue.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
				decimal Amount = Conversion.ToDecimal((object)Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString())) * Conversion.ToDecimal((object)Rate);
				item.Cells["AmountCr"].Value = Math.Round(Amount, 0, MidpointRounding.AwayFromZero);
			}
			if (e.Column.Key == "AmountCr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountCr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountCr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountDr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			if (e.Column.Key == "AmountDr" && item != null && (int)item.RowType == 0)
			{
				item.Cells["AmountDr"].Value = Math.Round(Conversion.ToDouble(item.Cells["AmountDr"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["AmountCr"].Value = 0;
				item.Cells["Rate"].Value = 0;
				item.Cells["Qty"].Value = 0;
			}
			_ = e.Column.Key == "AccountId";
			grdrevenue.UpdateData();
			CustomerBillAmount();
			FillSummaryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillSummaryGrid()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			GridEXRow PM = grdPackingMaterial.CurrentRow;
			GridEXRow RS = grdrevenue.CurrentRow;
			dtsummaryBySupplier.Clear();
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			string supplierIds = "";
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				supplierIds = supplierIds + "," + Conversion.ToString(row.Cells["SupplierAccountId"].Value);
			}
			if (string.IsNullOrEmpty(supplierIds))
			{
				return;
			}
			foreach (int Id in from id in supplierIds.Split(',').Distinct()
				select Conversion.ToInt((object)id))
			{
				if (Id == 0)
				{
					continue;
				}
				decimal ItemAmount = default(decimal);
				decimal ItemFcyAmount = default(decimal);
				decimal PackingAmount = default(decimal);
				decimal DrExpenses = default(decimal);
				decimal CrExpenses = default(decimal);
				rows = grdDetail.GetRows();
				foreach (GridEXRow rdetail in rows)
				{
					if (Conversion.ToInt(rdetail.Cells["SupplierAccountId"].Value) == Id)
					{
						ItemAmount += Conversion.ToDecimal(rdetail.Cells["SupAmount"].Value);
						ItemFcyAmount += Conversion.ToDecimal(rdetail.Cells["SupFcyAmount"].Value);
					}
				}
				if (PM != null && (int)PM.RowType == 0 && PM.Cells.Count > 0)
				{
					PackingAmount = default(decimal);
					rows = grdPackingMaterial.GetRows();
					foreach (GridEXRow rPM in rows)
					{
						if (SupplierCustomer.GetSupplierCustomerIdbyGlAccountId(Conversion.ToInt(rPM.Cells["CreditAccountId"].Value)) == Id)
						{
							PackingAmount += Conversion.ToDecimal(rPM.Cells["Amount"].Value);
						}
					}
				}
				if (RS != null && (int)RS.RowType == 0 && RS.Cells.Count > 0)
				{
					DrExpenses = default(decimal);
					CrExpenses = default(decimal);
					rows = grdrevenue.GetRows();
					foreach (GridEXRow rRS in rows)
					{
						if (Conversion.ToInt(rRS.Cells["SupplierAccountId"].Value) == Id)
						{
							DrExpenses += Conversion.ToDecimal(rRS.Cells["AmountDr"].Value);
							CrExpenses += Conversion.ToDecimal(rRS.Cells["AmountCr"].Value);
						}
					}
				}
				dtsummaryBySupplier.Rows.Add(Id, ItemAmount, PackingAmount, DrExpenses + CrExpenses, ItemAmount + PackingAmount + (DrExpenses + CrExpenses));
			}
			if (dtsummaryBySupplier.Rows.Count > 0)
			{
				grdsummary.DataSource = dtsummaryBySupplier;
				grdsummary.RetrieveStructure();
				grdsummarysetting();
			}
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
			grdsummary.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdsummary.RootTable.Columns["SupplierId"].HasValueList = true;
			grdsummary.RootTable.Columns["SupplierId"].LimitToList = true;
			grdsummary.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdsummary.RootTable.Columns["SupplierId"].Caption = "Supplier Name";
			grdsummary.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdsummary.RootTable.Columns["SupplierId"].Width = 250;
			grdsummary.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["Expenses"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["Expenses"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["PackingAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["PackingAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["PackingAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["PackingAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdsummary);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtbillno.Text.Trim() == string.Empty || txtbillno.Text.Trim() == "0" || txtbillno.Text.Trim() == null)
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

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_1072: Unknown result type (might be due to invalid IL or missing references)
		//IL_1079: Expected O, but got Unknown
		//IL_1207: Unknown result type (might be due to invalid IL or missing references)
		//IL_120e: Expected O, but got Unknown
		//IL_13e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ee: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			CommTradeTransactionHeader ct = new CommTradeTransactionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ct.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
			GridEXRow[] rows = grdrevenue.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if ((Conversion.ToDouble(r3.Cells["AmountCr"].Value) > 0.0 || Conversion.ToInt(r3.Cells["AmountDr"].Value) > 0) && Conversion.ToInt(r3.Cells["OtherRevenueId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Other Item In Revenue Grid In Row No" + (r3.RowIndex + 1));
					return;
				}
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToDouble(r4.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r4.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Packing Material Expense In Row No" + (r4.RowIndex + 1));
					return;
				}
			}
			dtCommission.Clear();
			if (Conversion.ToInt(cmbCommAgent.Value) != 0 && Conversion.ToDecimal((object)txtcommrate.Text) != 0m)
			{
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbCommAgent.Value), 5, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtcommrate.Text), Conversion.ToDecimal((object)txtcommamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbCommAgent.Value), 6, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtbrokeryrate.Text), Conversion.ToDecimal((object)txtbrokeryamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbsubbrokeryacc.Value), 6, 2, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtsubbrokeryrate.Text), Conversion.ToDecimal((object)txtsubbrokeryamnt.Text), Conversion.ToInt64(cmbCommAgent.Value));
			}
			ct.OrganizationId = UserAccount.OrganizationId;
			ct.CompanyId = UserAccount.CompanyId;
			ct.BranchesId = UserAccount.BranchesId;
			ct.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			ct.DocumentTypeId = 1704;
			ct.ModifyUserId = UserAccount.ID;
			ct.EntryUserId = UserAccount.ID;
			ct.ApprovedUserId = UserAccount.ID;
			ct.EntryDate = DateTime.Now;
			ct.ModifyDate = DateTime.Now;
			ct.ApprovedDate = DateTime.Now;
			ct.DocDate = Conversion.ToDateTime((object)txtbilldate.Value);
			ct.DocNo = Conversion.ToInt((object)txtbillno.Text);
			ct.ManualBillNo = txtmanualref.Text;
			ct.IsApproved = false;
			ct.VehicleNos = Conversion.ToString((object)txtvehicleno.Text);
			ct.BiltyNos = Conversion.ToString((object)txtbiltyno.Text);
			ct.BiltyDates = Conversion.ToString((object)txtbiltydates.Text);
			ct.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			ct.CommissionAgentId = Conversion.ToInt(cmbCommAgent.Value);
			ct.CalculationParameterId = Conversion.ToInt(cmbcommtype.Value);
			ct.CommRate = Conversion.ToDecimal((object)txtcommrate.Text);
			ct.CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			ct.BrokeryRate = Conversion.ToDecimal((object)txtbrokeryrate.Text);
			ct.BrokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
			ct.SubBrokerId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			ct.SubBrokeryRate = Conversion.ToDecimal((object)txtsubbrokeryrate.Text);
			ct.SubBrokeryAmount = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text);
			ct.BillAmount = Conversion.ToDecimal((object)txtcustbillamnt.Text);
			ct.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
			ct.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
			ct.FcyAmount = Conversion.ToDecimal((object)txtfcyamount.Text);
			ct.CommTradeTransactionDetailList = new List<CommTradeTransactionDetail>();
			ct.CommTradeTransactionPmOtherList = new List<CommTradeTransactionPmOther>();
			ct.CommTradeTransactionRevExpList = new List<CommTradeTransactionRevExp>();
			ct.CommTradeTransactionCommBrokeryList = new List<CommTradeTransactionCommBrokery>();
			rows = grdDetail.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				CommTradeTransactionDetail vd = new CommTradeTransactionDetail();
				vd.Id = Conversion.ToInt(r5.Cells["Id"].Value);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.CommTradeOrderId = Conversion.ToInt(r5.Cells["OrderId"].Value);
				vd.CommTradeOrderDetailId = Conversion.ToInt(r5.Cells["OrderDetailId"].Value);
				vd.CommTradeLoadingDeliveryId = Conversion.ToInt(r5.Cells["LoadingDeliveryId"].Value);
				vd.CommTradeLoadingDeliveryDetailId = Conversion.ToInt(r5.Cells["LoadingDeliveryDetailId"].Value);
				vd.dDocDate = Conversion.ToDateTime((object)txtbilldate.Value);
				vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
				vd.CusDeliveryTerm = Conversion.ToString((object)((Control)(object)cmbdeliveryterm).Text);
				vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.SupDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
				vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
				if (Conversion.ToInt(r5.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Item Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
				if (Conversion.ToInt(r5.Cells["CropYearId"].Value) == 0)
				{
					MessageBox.Show("CropYear Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.CropYearId = Conversion.ToInt(r5.Cells["CropYearId"].Value);
				if (Conversion.ToInt(r5.Cells["PackingTypeId"].Value) == 0)
				{
					MessageBox.Show("PackingType Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.PackingTypeId = Conversion.ToInt(r5.Cells["PackingTypeId"].Value);
				if (Conversion.ToInt(r5.Cells["UomId"].Value) == 0)
				{
					MessageBox.Show("Uom Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.UomId = Conversion.ToInt(r5.Cells["UomId"].Value);
				if (Conversion.ToDecimal(r5.Cells["ItemQty"].Value) == 0m)
				{
					MessageBox.Show("ItemQty Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.Qty = Conversion.ToDecimal(r5.Cells["ItemQty"].Value);
				if (Conversion.ToInt(r5.Cells["SupplierAccountId"].Value) == 0)
				{
					MessageBox.Show("SupplierAccount Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.SupplierId = Conversion.ToInt(r5.Cells["SupplierAccountId"].Value);
				if (Conversion.ToDecimal(r5.Cells["SupGrossWeight"].Value) == 0m)
				{
					MessageBox.Show("SupGrossWeight Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.SupGrossWeight = Conversion.ToDecimal(r5.Cells["SupGrossWeight"].Value);
				vd.SupPckMtrlWeight = Conversion.ToDecimal(r5.Cells["SupPMWeight"].Value);
				vd.SupAdLsWt = Conversion.ToDecimal(r5.Cells["SupAdLsWeight"].Value);
				if (Conversion.ToDecimal(r5.Cells["SupNetWeight"].Value) == 0m)
				{
					MessageBox.Show("SupNetWeight Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.SupNetWeight = Conversion.ToDecimal(r5.Cells["SupNetWeight"].Value);
				if (Conversion.ToDecimal(r5.Cells["SupRate"].Value) == 0m)
				{
					MessageBox.Show("SupRate Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.SupRate = Conversion.ToDecimal(r5.Cells["SupRate"].Value);
				if (Conversion.ToDecimal(r5.Cells["SupAmount"].Value) == 0m)
				{
					MessageBox.Show("SupAmount Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.SupAmount = Conversion.ToDecimal(r5.Cells["SupAmount"].Value);
				vd.SupFcyAmount = Conversion.ToDecimal(r5.Cells["SupFcyAmount"].Value);
				vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
				if (Conversion.ToDecimal(r5.Cells["CusGrossWeight"].Value) == 0m)
				{
					MessageBox.Show("CusGrossWeight Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.CusGrossWeight = Conversion.ToDecimal(r5.Cells["CusGrossWeight"].Value);
				vd.CusPckMtrlWeight = Conversion.ToDecimal(r5.Cells["CusPMWeight"].Value);
				vd.CusAdLsWt = Conversion.ToDecimal(r5.Cells["CusAdLsWeight"].Value);
				if (Conversion.ToDecimal(r5.Cells["CusNetWeight"].Value) == 0m)
				{
					MessageBox.Show("CusNetWeight Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.CusNetWeight = Conversion.ToDecimal(r5.Cells["CusNetWeight"].Value);
				if (Conversion.ToDecimal(r5.Cells["CusRate"].Value) == 0m)
				{
					MessageBox.Show("CusRate Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.CusRate = Conversion.ToDecimal(r5.Cells["CusRate"].Value);
				if (Conversion.ToDecimal(r5.Cells["CusAmount"].Value) == 0m)
				{
					MessageBox.Show("CusAmount Is Required In Detail Grid In row No " + (r5.RowIndex + 1));
					return;
				}
				vd.CusAmount = Conversion.ToDecimal(r5.Cells["CusAmount"].Value);
				vd.CustFcyAmount = Conversion.ToDecimal(r5.Cells["CusFcyAmount"].Value);
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r5.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Equivalent='" + 40 + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("RateUom of the Item " + Conversion.ToString((object)r5.Cells["ItemId"].Text) + " in row No " + (r5.RowIndex + 1) + " is not define please check ");
						return;
					}
					vd.CusRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
					vd.SupRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				ct.CommTradeTransactionDetailList.Add(vd);
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					ct.CommTradeTransactionDetailList.Add(lstRemoveRecord[j]);
				}
			}
			rows = grdPackingMaterial.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0)
				{
					CommTradeTransactionPmOther PE = new CommTradeTransactionPmOther();
					PE.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					PE.PackingMaterialId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
					PE.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					PE.ItemSize = Conversion.ToString(r2.Cells["ItemSize"].Value);
					PE.ItemWeight = Conversion.ToDecimal(r2.Cells["Weight"].Value);
					PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					PE.CreditAccountId = Conversion.ToInt(r2.Cells["CreditAccountId"].Value);
					ct.CommTradeTransactionPmOtherList.Add(PE);
				}
			}
			rows = grdrevenue.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["OtherRevenueId"].Value.ToString()) != 0)
				{
					CommTradeTransactionRevExp PJ2 = new CommTradeTransactionRevExp();
					PJ2.Id = Conversion.ToInt(r.Cells["Id"].Value);
					PJ2.AccountId = Conversion.ToInt(r.Cells["OtherRevenueId"].Value);
					PJ2.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierAccountId"].Value);
					PJ2.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					PJ2.Rate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					PJ2.DebitAmt = Conversion.ToDecimal(r.Cells["AmountDr"].Value);
					PJ2.CreditAmt = Conversion.ToDecimal(r.Cells["AmountCr"].Value);
					PJ2.ReRemarks = "";
					ct.CommTradeTransactionRevExpList.Add(PJ2);
				}
				else if (Conversion.ToInt(r.Cells["SupplierAccountId"].Value) != 0 && (Conversion.ToDecimal((object)r.Cells["AmountCr"].Text) != 0m || Conversion.ToDecimal((object)r.Cells["AmountDr"].Text) != 0m))
				{
					MessageBox.Show("OtherRevenue Is Required In Revenue Grid In row No " + (r.RowIndex + 1));
					return;
				}
			}
			for (int i = 0; i < dtCommission.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtCommission.Rows[i]["CommissionAgentId"]) != 0)
				{
					CommTradeTransactionCommBrokery PJ = new CommTradeTransactionCommBrokery();
					PJ.Id = Conversion.ToInt(dtCommission.Rows[i]["Id"]);
					PJ.CommissionAgentId = Conversion.ToInt(dtCommission.Rows[i]["CommissionAgentId"]);
					if (PJ.CommissionAgentId == 0)
					{
						MessageBox.Show("CommissionAgent  Is Required");
						return;
					}
					PJ.ChargesIncentiveId = Conversion.ToInt(dtCommission.Rows[i]["ChargesIncentiveId"]);
					PJ.RevenueExpenseId = Conversion.ToInt(dtCommission.Rows[i]["RevenueExpenseId"]);
					PJ.CalcParameterId = Conversion.ToInt(dtCommission.Rows[i]["CalcParameterId"]);
					PJ.Rate = Conversion.ToDecimal(dtCommission.Rows[i]["Rate"]);
					PJ.Amount = Conversion.ToDecimal(dtCommission.Rows[i]["Amount"]);
					PJ.SupplierCustomerId = Conversion.ToInt(dtCommission.Rows[i]["SupplierCustomerId"]);
					ct.CommTradeTransactionCommBrokeryList.Add(PJ);
				}
			}
			ct.AttachmentsList = new List<DMSAttachments>();
			ct.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<CommTradeTransactionHeader>(ct, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = CommTradeTransactionHeader.Save(ct);
			ct.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + ct.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					DMSAttachments.RemoveByIdAndNames(RecId, base.Name, ct.DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + ct.DocNo);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
					CommTradeTransactionHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					Reset();
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

	private void ReadById(int ID)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)ID);
			CommTradeTransactionHeader po = new CommTradeTransactionHeader();
			po = CommTradeTransactionHeader.ReadById(RecId);
			tabControl1.SelectedIndex = 0;
			txtbilldate.Value = po.DocDate;
			cmbcurrency.Value = po.CurrencyId;
			txtexchangerate.Text = Conversion.ToString((object)po.ExchangeRate);
			txtbillno.Text = Conversion.ToString((object)po.DocNo);
			txtmanualref.Text = Conversion.ToString((object)po.ManualBillNo);
			txtRemarks.Text = Conversion.ToString((object)po.RemarksHeader);
			txtbiltyno.Text = Conversion.ToString((object)po.BiltyNos);
			txtbiltydates.Text = Conversion.ToString((object)po.BiltyDates);
			txtvehicleno.Text = Conversion.ToString((object)po.VehicleNos);
			if (po.CommTradeTransactionCommBrokeryList.Count > 0)
			{
				for (int l = 0; l < po.CommTradeTransactionCommBrokeryList.Count; l++)
				{
					if (po.CommTradeTransactionCommBrokeryList[l].RevenueExpenseId == 1 && po.CommTradeTransactionCommBrokeryList[l].ChargesIncentiveId == 5)
					{
						cmbCommAgent.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[l].CommissionAgentId);
						cmbcommtype.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[l].CalcParameterId);
						txtcommrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Rate);
						txtcommamount.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Amount);
					}
					if (po.CommTradeTransactionCommBrokeryList[l].RevenueExpenseId == 1 && po.CommTradeTransactionCommBrokeryList[l].ChargesIncentiveId == 6)
					{
						txtbrokeryrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Rate);
						txtbrokeryamount.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Amount);
					}
					if (po.CommTradeTransactionCommBrokeryList[l].RevenueExpenseId == 2 && po.CommTradeTransactionCommBrokeryList[l].ChargesIncentiveId == 6)
					{
						cmbsubbrokeryacc.Value = Conversion.ToInt((object)po.CommTradeTransactionCommBrokeryList[l].CommissionAgentId);
						txtsubbrokeryrate.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Rate);
						txtsubbrokeryamnt.Text = Conversion.ToString((object)po.CommTradeTransactionCommBrokeryList[l].Amount);
					}
				}
			}
			if (po.CommTradeTransactionDetailList.Count > 0)
			{
				cmbCustomername.Value = po.CommTradeTransactionDetailList[0].CustomerId;
				((Control)(object)cmbCustomername).Enabled = false;
				cmbpayterm.Value = po.CommTradeTransactionDetailList[0].CusPaymentTermsId;
				((Control)(object)cmbdeliveryterm).Text = po.CommTradeTransactionDetailList[0].CusDeliveryTerm;
				txtduedays.Text = Conversion.ToString((object)po.CommTradeTransactionDetailList[0].CusDueDays);
				dtDetail.Clear();
				for (int k = 0; k < po.CommTradeTransactionDetailList.Count; k++)
				{
					dtDetail.Rows.Add(po.CommTradeTransactionDetailList[k].Id, po.CommTradeTransactionDetailList[k].CommTradeOrderId, po.CommTradeTransactionDetailList[k].CommTradeOrderDetailId, po.CommTradeTransactionDetailList[k].OrderNo, po.CommTradeTransactionDetailList[k].CommTradeLoadingDeliveryId, po.CommTradeTransactionDetailList[k].CommTradeLoadingDeliveryDetailId, po.CommTradeTransactionDetailList[k].LoadingDeliveryDocNo, po.CommTradeTransactionDetailList[k].ItemId, po.CommTradeTransactionDetailList[k].ItemCode, po.CommTradeTransactionDetailList[k].CropYearId, po.CommTradeTransactionDetailList[k].PackingTypeId, po.CommTradeTransactionDetailList[k].UomId, po.CommTradeTransactionDetailList[k].PackUom, po.CommTradeTransactionDetailList[k].Qty, po.CommTradeTransactionDetailList[k].CustomerId, po.CommTradeTransactionDetailList[k].CusGrossWeight, po.CommTradeTransactionDetailList[k].CusPckMtrlWeight, po.CommTradeTransactionDetailList[k].CusAdLsWt, po.CommTradeTransactionDetailList[k].CusNetWeight, po.CommTradeTransactionDetailList[k].CusRate, po.CommTradeTransactionDetailList[k].CusAmount, po.CommTradeTransactionDetailList[k].CustFcyAmount, po.CommTradeTransactionDetailList[k].SupplierId, po.CommTradeTransactionDetailList[k].SupGrossWeight, po.CommTradeTransactionDetailList[k].SupPckMtrlWeight, po.CommTradeTransactionDetailList[k].SupAdLsWt, po.CommTradeTransactionDetailList[k].SupNetWeight, po.CommTradeTransactionDetailList[k].SupRate, po.CommTradeTransactionDetailList[k].SupAmount, po.CommTradeTransactionDetailList[k].SupFcyAmount);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdDetailsetting();
			}
			dtPackingMaterial.Rows.Clear();
			if (po.CommTradeTransactionPmOtherList.Count > 0)
			{
				for (int j = 0; j < po.CommTradeTransactionPmOtherList.Count; j++)
				{
					dtPackingMaterial.Rows.Add(po.CommTradeTransactionPmOtherList[j].Id, po.CommTradeTransactionPmOtherList[j].PackingMaterialId, po.CommTradeTransactionPmOtherList[j].ItemId, po.CommTradeTransactionPmOtherList[j].ItemSize, po.CommTradeTransactionPmOtherList[j].ItemWeight, po.CommTradeTransactionPmOtherList[j].Qty, po.CommTradeTransactionPmOtherList[j].Rate, po.CommTradeTransactionPmOtherList[j].Amount, po.CommTradeTransactionPmOtherList[j].CreditAccountId);
				}
			}
			dtOtherRevenue.Rows.Clear();
			if (po.CommTradeTransactionRevExpList.Count > 0)
			{
				for (int i = 0; i < po.CommTradeTransactionRevExpList.Count; i++)
				{
					dtOtherRevenue.Rows.Add(po.CommTradeTransactionRevExpList[i].Id, po.CommTradeTransactionRevExpList[i].AccountId, po.CommTradeTransactionRevExpList[i].Qty, po.CommTradeTransactionRevExpList[i].Rate, po.CommTradeTransactionRevExpList[i].DebitAmt, po.CommTradeTransactionRevExpList[i].CreditAmt, po.CommTradeTransactionRevExpList[i].SupplierCustomerId);
				}
			}
			if (dtPackingMaterial.Rows.Count == 0)
			{
				AddRowPackingmaterial();
			}
			grdPackingMaterial.DataSource = dtPackingMaterial;
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			if (dtOtherRevenue.Rows.Count == 0)
			{
				AddRowOtherRevenue();
			}
			grdrevenue.DataSource = dtOtherRevenue;
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
			TotalCommissionAmount();
			CustomerBillAmount();
			FillSummaryGrid();
			tabControl1.SelectedIndex = 0;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
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
			PaymentTerms();
			suppliercustomer();
			CurrencyFill();
			CommissionTypeFill();
			ConfigurationDefault();
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			((Control)(object)cmbCustomername).Text = string.Empty;
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
			LoadDeliveryDetailIds = string.Empty;
			LoadDeliveryMainIds = string.Empty;
			cmbCustomername.Focus();
			((Control)(object)cmbCustomername).Enabled = true;
			btnSave.Enabled = true;
			btnUpdate.Visible = false;
			grdDetail.DataSource = null;
			dtDetail.Clear();
			btnSave.Visible = true;
			dtDetail.Rows.Clear();
			dtPackingMaterial.Rows.Clear();
			dtOtherRevenue.Rows.Clear();
			dtsummaryBySupplier.Rows.Clear();
			grdDetail.DataSource = dtDetail;
			grdsummary.DataSource = dtsummaryBySupplier;
			AddRowOtherRevenue();
			grdrevenue.RetrieveStructure();
			grdrevenuesetting();
			AddRowPackingmaterial();
			grdPackingMaterial.RetrieveStructure();
			grdpackingsetting();
			DocumentNoFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryDetail).Name);
			if (fs != null)
			{
				grdHistoryDetail.LoadLayoutFile((Stream)fs);
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
				((Control)(object)grdHistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistoryDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistoryDetail).Focus();
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
				LoadFromLoadingDelivery.orderDetailIds = LoadDeliveryDetailIds;
				LoadFromLoadingDelivery.orderMainIds = LoadDeliveryMainIds;
			}
			LoadFromLoadingDelivery.ShowDialog();
			LoadFromLoadingDelivery.BringToFront();
			LoadInGridDetail();
			FillSummaryGrid();
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
				DocumentTypeId = 1702,
				GdnIds = GdnIds
			});
			if (dtLoaderData.Rows.Count > 0)
			{
				cmbCustomername.Value = Conversion.ToInt(dtLoaderData.Rows[0]["CustomerId"]);
				((Control)(object)cmbCustomername).Enabled = false;
				cmbCommAgent.Value = Conversion.ToInt(dtLoaderData.Rows[0]["BrokerId"]);
				IEnumerable<int> distinctIds = (from row in dtLoaderData.AsEnumerable()
					select Conversion.ToInt(row["Id"])).Distinct();
				List<DataRow> distinctData = (from row in dtLoaderData.AsEnumerable()
					where distinctIds.Contains(Conversion.ToInt(row["Id"]))
					select row).ToList();
				string vehicleNos = string.Join(",", distinctData.Select((DataRow row) => Conversion.ToString(row["VehicleNo"])));
				string biltyNos = string.Join(",", distinctData.Select((DataRow row) => Conversion.ToString(row["BiltyNo"])));
				string biltyDates = string.Join(",", distinctData.Select((DataRow row) => Conversion.ToDateTime(row["BiltyDate"]).ToShortDateString()));
				txtvehicleno.Text = vehicleNos;
				txtbiltyno.Text = biltyNos;
				txtbiltydates.Text = biltyDates;
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
						dtDetail.Rows.Add(0, Conversion.ToInt(dtLoaderData.Rows[i]["OrderId"]), Conversion.ToInt(dtLoaderData.Rows[i]["OrderDetailId"]), dtLoaderData.Rows[i]["OrderNo"], Conversion.ToInt(dtLoaderData.Rows[i]["Id"]), Conversion.ToInt(dtLoaderData.Rows[i]["DetailId"]), Conversion.ToInt(dtLoaderData.Rows[i]["DocNo"]), Conversion.ToInt(dtLoaderData.Rows[i]["ItemId"]), dtLoaderData.Rows[i]["ItemCode"], Conversion.ToInt(dtLoaderData.Rows[i]["CropYearId"]), Conversion.ToInt(dtLoaderData.Rows[i]["PackingTypeId"]), Conversion.ToInt(dtLoaderData.Rows[i]["UomId"]), Conversion.ToString(dtLoaderData.Rows[i]["PackUom"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["Qty"]), Conversion.ToInt(dtLoaderData.Rows[i]["CustomerId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["CusNetWeight"]), 0, 0, 0, Conversion.ToInt(dtLoaderData.Rows[i]["SupplierId"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupGrossWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupPckMtrlWeight"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupAdLsWt"]), Conversion.ToDecimal(dtLoaderData.Rows[i]["SupNetWeight"]), 0, 0, 0);
						flag = false;
					}
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
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

	private void CalculateUpdateFcyInDetailGrid()
	{
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdDetail.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Cusamount = Conversion.ToDouble(obj.Cells["CusAmount"].Value) / Conversion.ToDouble((object)txtexchangerate.Text);
					obj.Cells["CusFcyAmount"].Value = Math.Round(Cusamount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					double Supamount = Conversion.ToDouble(obj.Cells["SupAmount"].Value) / Conversion.ToDouble((object)txtexchangerate.Text);
					obj.Cells["SupFcyAmount"].Value = Math.Round(Supamount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else if (grdDetail.CurrentRow != null && (int)grdDetail.CurrentRow.RowType == 0 && grdDetail.CurrentRow.Cells.Count > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["CusFcyAmount"].Value = 0;
					obj2.Cells["SupFcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalCommissionAmount()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCustomername.Value) != 0 && Conversion.ToInt(cmbcommtype.Value) != 0)
			{
				if (Conversion.ToString((object)((Control)(object)cmbcommtype).Text) == "Flat")
				{
					if (CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					}).Rows.Count > 0)
					{
						double CommRate2 = Conversion.ToDouble((object)((Control)(object)cmbcommtype).Text.Trim());
						txtcommrate.Text = CommRate2.ToString();
						txtcommamount.Text = CommRate2.ToString();
					}
					DataTable dtBrokery3 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery3.Rows.Count > 0)
					{
						decimal BrokeryRate3 = Conversion.ToDecimal(dtBrokery3.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate3.ToString();
						txtbrokeryamount.Text = BrokeryRate3.ToString();
					}
					DataTable dtSubBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (dtSubBrokery.Rows.Count > 0)
					{
						decimal SubBrokeryRate3 = Conversion.ToDecimal(dtSubBrokery.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate3.ToString();
						txtsubbrokeryamnt.Text = SubBrokeryRate3.ToString();
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Percent")
				{
					DataTable dtcomm2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					});
					if (dtcomm2.Rows.Count > 0)
					{
						object total3 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal commRate = Conversion.ToDecimal(dtcomm2.Rows[0]["Rate"]);
						txtcommrate.Text = commRate.ToString();
						if (total3 != null)
						{
							decimal CommAmt4 = Conversion.ToDecimal((object)total3.ToString()) * commRate / 100m;
							txtcommamount.Text = Math.Round(CommAmt4).ToString();
						}
					}
					DataTable dtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery2.Rows.Count > 0)
					{
						object total2 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal BrokeryRate2 = Conversion.ToDecimal(dtBrokery2.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate2.ToString();
						if (total2 != null)
						{
							decimal CommAmt3 = Conversion.ToDecimal((object)total2.ToString()) * BrokeryRate2 / 100m;
							txtbrokeryamount.Text = Math.Round(CommAmt3).ToString();
						}
					}
					DataTable SubdtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (SubdtBrokery2.Rows.Count > 0)
					{
						object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal SubBrokeryRate2 = Conversion.ToDecimal(SubdtBrokery2.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate2.ToString();
						if (total != null)
						{
							decimal CommAmt2 = Conversion.ToDecimal((object)total.ToString()) * SubBrokeryRate2 / 100m;
							txtsubbrokeryamnt.Text = Math.Round(CommAmt2).ToString();
						}
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Weight")
				{
					DataTable dtcomm = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					});
					if (dtcomm.Rows.Count > 0)
					{
						object weight3 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal CommRate = Conversion.ToDecimal(dtcomm.Rows[0]["Rate"]);
						decimal UOM3 = Conversion.ToDecimal(dtcomm.Rows[0]["RateUom"]);
						txtcommrate.Text = CommRate.ToString();
						if (weight3 != null)
						{
							decimal CommAmt = Conversion.ToDecimal((object)weight3.ToString()) / UOM3 * CommRate;
							txtcommamount.Text = Math.Round(CommAmt).ToString();
						}
					}
					DataTable dtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery.Rows.Count > 0)
					{
						object weight2 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal BrokeryRate = Conversion.ToDecimal(dtBrokery.Rows[0]["Rate"]);
						decimal UOM2 = Conversion.ToDecimal(dtBrokery.Rows[0]["RateUom"]);
						txtbrokeryrate.Text = BrokeryRate.ToString();
						if (weight2 != null)
						{
							decimal BrokeryAmt2 = Conversion.ToDecimal((object)weight2.ToString()) / UOM2 * BrokeryRate;
							txtbrokeryamount.Text = Math.Round(BrokeryAmt2).ToString();
						}
					}
					DataTable SubdtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtbilldate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbsubbrokeryacc.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (SubdtBrokery.Rows.Count > 0)
					{
						object weight = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal SubBrokeryRate = Conversion.ToDecimal(SubdtBrokery.Rows[0]["Rate"]);
						decimal UOM = Conversion.ToDecimal(SubdtBrokery.Rows[0]["RateUom"]);
						txtsubbrokeryrate.Text = SubBrokeryRate.ToString();
						if (weight != null)
						{
							decimal BrokeryAmt = Conversion.ToDecimal((object)weight.ToString()) / UOM * SubBrokeryRate;
							txtsubbrokeryamnt.Text = Math.Round(BrokeryAmt).ToString();
						}
					}
				}
			}
			_ = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text) > 0m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerBillAmount()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				decimal JournalDebit = default(decimal);
				decimal JournalCredit = default(decimal);
				decimal TotalCommAmount = default(decimal);
				decimal TotalBokeryAmount = default(decimal);
				decimal TotalAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CusFcyAmount"], (AggregateFunction)2));
				if (grdrevenue.GetRows().Count() > 0)
				{
					GridEXRow[] rows = grdrevenue.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt((object)r.Cells["OtherRevenueId"].Value.ToString()) > 0)
						{
							JournalDebit += Conversion.ToDecimal((object)r.Cells["AmountDr"].Value.ToString());
							JournalCredit += Conversion.ToDecimal((object)r.Cells["AmountCr"].Value.ToString());
						}
					}
					if (JournalDebit > 0m)
					{
						TotalAmount += JournalDebit;
					}
					else if (JournalCredit > 0m)
					{
						TotalAmount -= JournalCredit;
					}
				}
				if (((UltraGridBase)cmbCommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m && Conversion.ToInt(cmbCommAgent.Value) != Conversion.ToInt(cmbCustomername.Value))
				{
					TotalCommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
					TotalBokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
					TotalAmount -= TotalCommAmount + TotalBokeryAmount;
					if (Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
					{
						TotalFcyAmount -= (TotalCommAmount + TotalBokeryAmount) / Conversion.ToDecimal((object)txtexchangerate.Text);
					}
					else
					{
						TotalFcyAmount -= TotalCommAmount + TotalBokeryAmount;
					}
				}
				if (TotalAmount > 0m)
				{
					txtcustbillamnt.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtcustbillamnt.Text = "0";
				}
				if (TotalFcyAmount > 0m)
				{
					txtfcyamount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
				else
				{
					txtfcyamount.Text = "0";
				}
			}
			else
			{
				txtcustbillamnt.Text = "0";
				txtfcyamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCustomerLeave_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyInDetailGrid();
			TotalCommissionAmount();
			CustomerBillAmount();
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
				GridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1704;
			obj.EntryUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtgrid = CommTradeTransactionHeader.FormHistory(obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("ManualBillNo", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("CommRate", typeof(decimal));
				dtcol.Columns.Add("CommAmount", typeof(decimal));
				dtcol.Columns.Add("BrokeryRate", typeof(decimal));
				dtcol.Columns.Add("BrokeryAmount", typeof(decimal));
				dtcol.Columns.Add("SubBrokeryRate", typeof(decimal));
				dtcol.Columns.Add("SubBrokeryAmount", typeof(decimal));
				dtcol.Columns.Add("BiltyNos", typeof(string));
				dtcol.Columns.Add("BiltyDates", typeof(string));
				dtcol.Columns.Add("VehicleNos", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("TotalQty", typeof(int));
				dtcol.Columns.Add("SupTotalNetWeight", typeof(decimal));
				dtcol.Columns.Add("CusTotalNetWeight", typeof(decimal));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtgrid.Rows[i]["RecordNo"], dtgrid.Rows[i]["DocumentTypeId"], dtgrid.Rows[i]["Id"], dtgrid.Rows[i]["DocDate"], dtgrid.Rows[i]["DocNo"], dtgrid.Rows[i]["ManualBillNo"], dtgrid.Rows[i]["RemarksHeader"], dtgrid.Rows[i]["CommRate"], dtgrid.Rows[i]["CommAmount"], dtgrid.Rows[i]["BrokeryRate"], dtgrid.Rows[i]["BrokeryAmount"], dtgrid.Rows[i]["SubBrokeryRate"], dtgrid.Rows[i]["SubBrokeryAmount"], dtgrid.Rows[i]["BiltyNos"], dtgrid.Rows[i]["BiltyDates"], dtgrid.Rows[i]["VehicleNos"], dtgrid.Rows[i]["EntryDate"], dtgrid.Rows[i]["ModifyDate"], dtgrid.Rows[i]["IsApproved"], dtgrid.Rows[i]["TotalQty"], dtgrid.Rows[i]["SupTotalNetWeight"], dtgrid.Rows[i]["CusTotalNetWeight"], dtgrid.Rows[i]["EntryUserName"], dtgrid.Rows[i]["ModifyUserName"], dtgrid.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtcol;
				grdHistory.RetrieveStructure();
				GridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistorySetting()
	{
		try
		{
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CommRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryRate"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SubBrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SupTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalNetWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CusTotalNetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Key = "Edit";
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Key = "Print";
				grdHistory.RootTable.Columns["Print"].Caption = "Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
				grdHistory.RootTable.Columns["Print"].Width = 50;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.RootTable.Columns["Edit"].Position = 1;
				grdHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtbilldate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtbilldate.Focus();
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				int Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			CommTradeTransactionHeader po = new CommTradeTransactionHeader();
			po = CommTradeTransactionHeader.ReadById(Id);
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("LoadingNo", typeof(int));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("Uom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("CustomerAccount", typeof(string));
				dt.Columns.Add("CusGrossWeight", typeof(double));
				dt.Columns.Add("CusPMWeight", typeof(double));
				dt.Columns.Add("CusAdlsWeight", typeof(double));
				dt.Columns.Add("CusNetWeight", typeof(double));
				dt.Columns.Add("CusRate", typeof(double));
				dt.Columns.Add("CusAmount", typeof(double));
				dt.Columns.Add("CusFcyAmount", typeof(double));
				dt.Columns.Add("SupplierAccount", typeof(string));
				dt.Columns.Add("SupGrossWeight", typeof(double));
				dt.Columns.Add("SupPMWeight", typeof(double));
				dt.Columns.Add("SupAdlsWeight", typeof(double));
				dt.Columns.Add("SupNetWeight", typeof(double));
				dt.Columns.Add("SupRate", typeof(double));
				dt.Columns.Add("SupAmount", typeof(double));
				dt.Columns.Add("SupFcyAmount", typeof(double));
				dt.Clear();
				for (int i = 0; i < po.CommTradeTransactionDetailList.Count; i++)
				{
					dt.Rows.Add(po.CommTradeTransactionDetailList[i].Id, po.CommTradeTransactionDetailList[i].OrderNo, po.CommTradeTransactionDetailList[i].LoadingDeliveryDocNo, po.CommTradeTransactionDetailList[i].ItemId, po.CommTradeTransactionDetailList[i].ItemName, po.CommTradeTransactionDetailList[i].ItemCode, po.CommTradeTransactionDetailList[i].CropYear, po.CommTradeTransactionDetailList[i].PackingType, po.CommTradeTransactionDetailList[i].PackUom, po.CommTradeTransactionDetailList[i].Qty, po.CommTradeTransactionDetailList[i].CustomerName, po.CommTradeTransactionDetailList[i].CusGrossWeight, po.CommTradeTransactionDetailList[i].CusPckMtrlWeight, po.CommTradeTransactionDetailList[i].CusAdLsWt, po.CommTradeTransactionDetailList[i].CusNetWeight, po.CommTradeTransactionDetailList[i].CusRate, po.CommTradeTransactionDetailList[i].CusAmount, po.CommTradeTransactionDetailList[i].CustFcyAmount, po.CommTradeTransactionDetailList[i].SupplierName, po.CommTradeTransactionDetailList[i].SupGrossWeight, po.CommTradeTransactionDetailList[i].SupPckMtrlWeight, po.CommTradeTransactionDetailList[i].SupAdLsWt, po.CommTradeTransactionDetailList[i].SupNetWeight, po.CommTradeTransactionDetailList[i].SupRate, po.CommTradeTransactionDetailList[i].SupAmount, po.CommTradeTransactionDetailList[i].SupFcyAmount);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				GridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryDetailSetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["OrderNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].Visible = false;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].Visible = false;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["CusAdlsWeight"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].Caption = "Gross Weight";
			grdHistoryDetail.RootTable.Columns["SupAdlsWeight"].Caption = "Adls Weight";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].Caption = "PM Weight";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].Caption = "Net Weight";
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["SupAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["SupAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["CusAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["CusAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["CusRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["SupRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["CusFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["SupFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["SupPMWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["SupGrossWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusPMWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["CusGrossWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString().ToString()), 1704);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grddetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaddAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = CommTradeTransactionHeader.CommissionTransactionSlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Id = PrintId,
				DocumentTypeId = 1704
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1704-CommTradeTransactionHeader_Slip.rpt");
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
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Expected O, but got Unknown
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_0592: Expected O, but got Unknown
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Expected O, but got Unknown
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0895: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_0903: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_097c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_4b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba0: Expected O, but got Unknown
		//IL_4bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb7: Expected O, but got Unknown
		//IL_4d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d35: Expected O, but got Unknown
		//IL_4d42: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d4c: Expected O, but got Unknown
		//IL_5369: Unknown result type (might be due to invalid IL or missing references)
		//IL_5373: Expected O, but got Unknown
		//IL_5380: Unknown result type (might be due to invalid IL or missing references)
		//IL_538a: Expected O, but got Unknown
		//IL_8af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_8afe: Expected O, but got Unknown
		//IL_8b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_8b2c: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.TransactionForms.CommisionBillDirectAndAgainstOrder));
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		GridEXLayout grdrevenue_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		GridEXLayout grdsummary_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
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
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdrevenue = new GridEX();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdPackingMaterial = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.PanelDetail = new System.Windows.Forms.GroupBox();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtsupremarks = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtcusamount = new System.Windows.Forms.TextBox();
		this.txtcusrate = new System.Windows.Forms.TextBox();
		this.txtcustnetwt = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.label32 = new System.Windows.Forms.Label();
		this.txtsupamount = new System.Windows.Forms.TextBox();
		this.txtcustadlswt = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.txtcustpckmtrlwt = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.txtcustgrosswt = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtsuprate = new System.Windows.Forms.TextBox();
		this.cmbitem = new UltraCombo();
		this.txtSupNetWeight = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.txtSupAdLsWt = new System.Windows.Forms.TextBox();
		this.cmbuom = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.cmbcropYear = new UltraCombo();
		this.txtSupPckMtrlWeight = new System.Windows.Forms.TextBox();
		this.CmbPackingType = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtsupgrosswt = new System.Windows.Forms.TextBox();
		this.labItemName = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.cmbsupplier = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdsummary = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.History = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
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
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.PanelDetail.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsummary).BeginInit();
		this.panel4.SuspendLayout();
		this.panel17.SuspendLayout();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnUpdate, this.btnSave, this.btnAttachment, this.BtnPrint, this.btnDelete, this.btnRefresh, this.BtnLoadFromLoadingDelivery, this.btnShortcutKeys });
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
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(100, 24);
		this.BtnPrint.Text = "1704-&Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
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
		this.txtfcyamount.Enabled = false;
		this.txtfcyamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfcyamount.Location = new System.Drawing.Point(396, 214);
		this.txtfcyamount.Name = "txtfcyamount";
		this.txtfcyamount.ReadOnly = true;
		this.txtfcyamount.Size = new System.Drawing.Size(130, 23);
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
		this.txtsubbrokeryrate.Enabled = false;
		this.txtsubbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryrate.Location = new System.Drawing.Point(88, 175);
		this.txtsubbrokeryrate.Name = "txtsubbrokeryrate";
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
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
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
		this.txtbrokeryrate.Enabled = false;
		this.txtbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryrate.Location = new System.Drawing.Point(270, 80);
		this.txtbrokeryrate.Name = "txtbrokeryrate";
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
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(88, 49);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbCustomerLeave_Leave);
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
		this.txtcommrate.Enabled = false;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.Location = new System.Drawing.Point(270, 51);
		this.txtcommrate.Name = "txtcommrate";
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
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
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
		this.label23.Location = new System.Drawing.Point(307, 218);
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
		this.txtbiltydates.Size = new System.Drawing.Size(229, 23);
		this.txtbiltydates.TabIndex = 5;
		this.txtexchangerate.BackColor = System.Drawing.Color.White;
		this.txtexchangerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangerate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtexchangerate.Location = new System.Drawing.Point(396, 187);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.Size = new System.Drawing.Size(130, 23);
		this.txtexchangerate.TabIndex = 13;
		this.txtexchangerate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtbiltyno.BackColor = System.Drawing.Color.White;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyno.Location = new System.Drawing.Point(76, 105);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(229, 23);
		this.txtbiltyno.TabIndex = 4;
		this.txtmanualref.BackColor = System.Drawing.Color.White;
		this.txtmanualref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualref.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualref.Location = new System.Drawing.Point(396, 19);
		this.txtmanualref.Name = "txtmanualref";
		this.txtmanualref.Size = new System.Drawing.Size(130, 23);
		this.txtmanualref.TabIndex = 7;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(307, 191);
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
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpayterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Location = new System.Drawing.Point(396, 45);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Name = "cmbpayterm";
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).TabIndex = 8;
		this.txtvehicleno.BackColor = System.Drawing.Color.White;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(76, 76);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(229, 23);
		this.txtvehicleno.TabIndex = 3;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(307, 51);
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
		this.txtbilldate.Leave += new System.EventHandler(cmbCustomerLeave_Leave);
		this.txtduedays.BackColor = System.Drawing.Color.White;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(396, 76);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(130, 23);
		this.txtduedays.TabIndex = 9;
		this.txtduedays.TextChanged += new System.EventHandler(cmbCustomerLeave_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(142, 23);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(51, 15);
		this.label4.TabIndex = 7;
		this.label4.Text = "Bill Date";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(307, 164);
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
		this.label3.Location = new System.Drawing.Point(307, 80);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 15);
		this.label3.TabIndex = 11;
		this.label3.Text = "Due Days";
		this.txtbillno.BackColor = System.Drawing.Color.White;
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Enabled = false;
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
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(396, 158);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(130, 26);
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
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Location = new System.Drawing.Point(76, 45);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Name = "cmbCustomername";
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).TabIndex = 2;
		this.cmbCustomername.ValueChanged += new System.EventHandler(cmbCustomerLeave_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(307, 134);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(89, 20);
		this.label20.TabIndex = 29;
		this.label20.Text = "Bill Amount";
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
		this.label5.Location = new System.Drawing.Point(307, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 15;
		this.label5.Text = "Manual Ref";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(307, 109);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 17;
		this.label6.Text = "Delivery Term";
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(396, 103);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 10;
		this.txtcustbillamnt.BackColor = System.Drawing.Color.White;
		this.txtcustbillamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustbillamnt.Enabled = false;
		this.txtcustbillamnt.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtcustbillamnt.Location = new System.Drawing.Point(396, 132);
		this.txtcustbillamnt.Name = "txtcustbillamnt";
		this.txtcustbillamnt.ReadOnly = true;
		this.txtcustbillamnt.Size = new System.Drawing.Size(130, 25);
		this.txtcustbillamnt.TabIndex = 11;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(538, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdrevenue;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(44, 22);
		this.ctrlGrdBar1.TabIndex = 43;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		((ScrollableBase)this.grdrevenue).BackColor = System.Drawing.Color.White;
		grdrevenue_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdrevenue.DesignTimeLayout = grdrevenue_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdrevenue.GroupByBoxVisible = false;
		this.grdrevenue.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Location = new System.Drawing.Point(584, 0);
		((System.Windows.Forms.Control)(object)this.grdrevenue).Name = "grdrevenue";
		((ScrollableBase)this.grdrevenue).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdrevenue).Size = new System.Drawing.Size(586, 130);
		((System.Windows.Forms.Control)(object)this.grdrevenue).TabIndex = 0;
		this.grdrevenue.TotalRow = (InheritableBoolean)1;
		this.grdrevenue.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdrevenue).VisualStyle = (VisualStyle)4;
		this.grdrevenue.CellUpdated += new ColumnActionEventHandler(grdrevenue_CellUpdated);
		this.grdrevenue.ColumnButtonClick += new ColumnActionEventHandler(grdrevenue_ColumnButtonClick);
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(539, 2);
		this.ctrlGrdBar7.MyGrid = this.grdPackingMaterial;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 43;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		((ScrollableBase)this.grdPackingMaterial).BackColor = System.Drawing.Color.White;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		this.grdPackingMaterial.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(584, 130);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 0;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		this.grdPackingMaterial.CellUpdated += new ColumnActionEventHandler(grdPackingMaterial_CellUpdated);
		this.grdPackingMaterial.ColumnButtonClick += new ColumnActionEventHandler(grdPackingMaterial_ColumnButtonClick);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 761);
		this.tabControl1.TabIndex = 6;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.Form.Controls.Add(this.panel12);
		this.Form.Controls.Add(this.PanelDetail);
		this.Form.Controls.Add(this.panel1);
		this.Form.Controls.Add(this.panel17);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(3);
		this.Form.Size = new System.Drawing.Size(1176, 732);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdrevenue);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 599);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 130);
		this.panel5.TabIndex = 50;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label29);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(584, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(586, 24);
		this.panel10.TabIndex = 4;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(3, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(99, 14);
		this.label29.TabIndex = 40;
		this.label29.Text = "Other Revenue";
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(584, 130);
		this.panel7.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label28);
		this.panel9.Controls.Add(this.ctrlGrdBar7);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(584, 24);
		this.panel9.TabIndex = 4;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(3, 3);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(100, 14);
		this.label28.TabIndex = 40;
		this.label28.Text = "Packing Matrial";
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 460);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1170, 139);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 49;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label30);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 437);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 23);
		this.panel12.TabIndex = 48;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(3, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(42, 14);
		this.label30.TabIndex = 40;
		this.label30.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 43;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.PanelDetail.Controls.Add(this.groupBox5);
		this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.PanelDetail.Location = new System.Drawing.Point(3, 296);
		this.PanelDetail.Name = "PanelDetail";
		this.PanelDetail.Size = new System.Drawing.Size(1170, 141);
		this.PanelDetail.TabIndex = 47;
		this.PanelDetail.TabStop = false;
		this.groupBox5.Controls.Add(this.btnAddDetail);
		this.groupBox5.Controls.Add(this.btnCancelDetail);
		this.groupBox5.Controls.Add(this.label34);
		this.groupBox5.Controls.Add(this.btnUpdateDetail);
		this.groupBox5.Controls.Add(this.txtsupremarks);
		this.groupBox5.Controls.Add(this.label49);
		this.groupBox5.Controls.Add(this.label42);
		this.groupBox5.Controls.Add(this.txtcusamount);
		this.groupBox5.Controls.Add(this.txtcusrate);
		this.groupBox5.Controls.Add(this.txtcustnetwt);
		this.groupBox5.Controls.Add(this.label35);
		this.groupBox5.Controls.Add(this.label39);
		this.groupBox5.Controls.Add(this.rdSearchByCode);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Controls.Add(this.txtsupamount);
		this.groupBox5.Controls.Add(this.txtcustadlswt);
		this.groupBox5.Controls.Add(this.label48);
		this.groupBox5.Controls.Add(this.label37);
		this.groupBox5.Controls.Add(this.rdSearchByName);
		this.groupBox5.Controls.Add(this.txtcustpckmtrlwt);
		this.groupBox5.Controls.Add(this.label43);
		this.groupBox5.Controls.Add(this.txtcustgrosswt);
		this.groupBox5.Controls.Add(this.label38);
		this.groupBox5.Controls.Add(this.txtsuprate);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox5.Controls.Add(this.txtSupNetWeight);
		this.groupBox5.Controls.Add(this.label33);
		this.groupBox5.Controls.Add(this.label44);
		this.groupBox5.Controls.Add(this.txtItemQty);
		this.groupBox5.Controls.Add(this.txtSupAdLsWt);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.groupBox5.Controls.Add(this.label45);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropYear);
		this.groupBox5.Controls.Add(this.txtSupPckMtrlWeight);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox5.Controls.Add(this.label46);
		this.groupBox5.Controls.Add(this.label36);
		this.groupBox5.Controls.Add(this.txtsupgrosswt);
		this.groupBox5.Controls.Add(this.labItemName);
		this.groupBox5.Controls.Add(this.label47);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox5.Controls.Add(this.label40);
		this.groupBox5.Controls.Add(this.label41);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(5, 21);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1160, 117);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Detail Infomation";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(1010, 83);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 27);
		this.btnAddDetail.TabIndex = 3;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1081, 83);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 27);
		this.btnCancelDetail.TabIndex = 5;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(891, 66);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(73, 15);
		this.label34.TabIndex = 128;
		this.label34.Text = "Cus Amount";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1010, 83);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 27);
		this.btnUpdateDetail.TabIndex = 4;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.txtsupremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupremarks.Location = new System.Drawing.Point(210, 85);
		this.txtsupremarks.Name = "txtsupremarks";
		this.txtsupremarks.Size = new System.Drawing.Size(223, 23);
		this.txtsupremarks.TabIndex = 5;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(800, 66);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(52, 15);
		this.label49.TabIndex = 129;
		this.label49.Text = "Cus Rate";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(210, 66);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(52, 15);
		this.label42.TabIndex = 121;
		this.label42.Text = "Remarks";
		this.txtcusamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcusamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcusamount.Location = new System.Drawing.Point(893, 85);
		this.txtcusamount.Name = "txtcusamount";
		this.txtcusamount.ReadOnly = true;
		this.txtcusamount.ShortcutsEnabled = false;
		this.txtcusamount.Size = new System.Drawing.Size(115, 23);
		this.txtcusamount.TabIndex = 127;
		this.txtcusamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcusrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcusrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcusrate.Location = new System.Drawing.Point(800, 85);
		this.txtcusrate.Name = "txtcusrate";
		this.txtcusrate.ShortcutsEnabled = false;
		this.txtcusrate.Size = new System.Drawing.Size(91, 23);
		this.txtcusrate.TabIndex = 126;
		this.txtcusrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustnetwt.Enabled = false;
		this.txtcustnetwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustnetwt.Location = new System.Drawing.Point(696, 85);
		this.txtcustnetwt.Name = "txtcustnetwt";
		this.txtcustnetwt.ShortcutsEnabled = false;
		this.txtcustnetwt.Size = new System.Drawing.Size(102, 23);
		this.txtcustnetwt.TabIndex = 4;
		this.txtcustnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(693, 66);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(45, 15);
		this.label35.TabIndex = 119;
		this.label35.Text = "Net Wt";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(93, 66);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(75, 15);
		this.label39.TabIndex = 122;
		this.label39.Text = "Sup Amount";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(126, 18);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 1;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(618, 66);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(52, 15);
		this.label32.TabIndex = 117;
		this.label32.Text = "AdLs Wt";
		this.txtsupamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupamount.Location = new System.Drawing.Point(91, 85);
		this.txtsupamount.Name = "txtsupamount";
		this.txtsupamount.ReadOnly = true;
		this.txtsupamount.ShortcutsEnabled = false;
		this.txtsupamount.Size = new System.Drawing.Size(115, 23);
		this.txtsupamount.TabIndex = 121;
		this.txtsupamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustadlswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustadlswt.Location = new System.Drawing.Point(618, 85);
		this.txtcustadlswt.Name = "txtcustadlswt";
		this.txtcustadlswt.ShortcutsEnabled = false;
		this.txtcustadlswt.Size = new System.Drawing.Size(76, 23);
		this.txtcustadlswt.TabIndex = 3;
		this.txtcustadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 66);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(54, 15);
		this.label48.TabIndex = 123;
		this.label48.Text = "Sup Rate";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(540, 66);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(45, 15);
		this.label37.TabIndex = 115;
		this.label37.Text = "P.M Wt";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(66, 18);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 0;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.txtcustpckmtrlwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustpckmtrlwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustpckmtrlwt.Location = new System.Drawing.Point(540, 85);
		this.txtcustpckmtrlwt.Name = "txtcustpckmtrlwt";
		this.txtcustpckmtrlwt.ShortcutsEnabled = false;
		this.txtcustpckmtrlwt.Size = new System.Drawing.Size(76, 23);
		this.txtcustpckmtrlwt.TabIndex = 2;
		this.txtcustpckmtrlwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(1058, 20);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(45, 15);
		this.label43.TabIndex = 119;
		this.label43.Text = "Net Wt";
		this.txtcustgrosswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustgrosswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustgrosswt.Location = new System.Drawing.Point(436, 85);
		this.txtcustgrosswt.Name = "txtcustgrosswt";
		this.txtcustgrosswt.ShortcutsEnabled = false;
		this.txtcustgrosswt.Size = new System.Drawing.Size(102, 23);
		this.txtcustgrosswt.TabIndex = 1;
		this.txtcustgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(436, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(55, 15);
		this.label38.TabIndex = 113;
		this.label38.Text = "Gross Wt";
		this.txtsuprate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuprate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuprate.Location = new System.Drawing.Point(5, 85);
		this.txtsuprate.Name = "txtsuprate";
		this.txtsuprate.ShortcutsEnabled = false;
		this.txtsuprate.Size = new System.Drawing.Size(83, 23);
		this.txtsuprate.TabIndex = 120;
		this.txtsuprate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(5, 37);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 0;
		this.txtSupNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupNetWeight.Enabled = false;
		this.txtSupNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupNetWeight.Location = new System.Drawing.Point(1058, 39);
		this.txtSupNetWeight.Name = "txtSupNetWeight";
		this.txtSupNetWeight.ShortcutsEnabled = false;
		this.txtSupNetWeight.Size = new System.Drawing.Size(102, 23);
		this.txtSupNetWeight.TabIndex = 4;
		this.txtSupNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(512, 20);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(51, 15);
		this.label33.TabIndex = 109;
		this.label33.Text = "ItemQty";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(977, 20);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(52, 15);
		this.label44.TabIndex = 117;
		this.label44.Text = "AdLs Wt";
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(512, 39);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ShortcutsEnabled = false;
		this.txtItemQty.Size = new System.Drawing.Size(83, 23);
		this.txtItemQty.TabIndex = 4;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupAdLsWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupAdLsWt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupAdLsWt.Location = new System.Drawing.Point(980, 39);
		this.txtSupAdLsWt.Name = "txtSupAdLsWt";
		this.txtSupAdLsWt.ShortcutsEnabled = false;
		this.txtSupAdLsWt.Size = new System.Drawing.Size(76, 23);
		this.txtSupAdLsWt.TabIndex = 3;
		this.txtSupAdLsWt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(436, 37);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(74, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 3;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(905, 20);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(45, 15);
		this.label45.TabIndex = 115;
		this.label45.Text = "P.M Wt";
		this.cmbcropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbcropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Location = new System.Drawing.Point(210, 37);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Name = "cmbcropYear";
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).TabIndex = 1;
		this.txtSupPckMtrlWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupPckMtrlWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupPckMtrlWeight.Location = new System.Drawing.Point(902, 39);
		this.txtSupPckMtrlWeight.Name = "txtSupPckMtrlWeight";
		this.txtSupPckMtrlWeight.ShortcutsEnabled = false;
		this.txtSupPckMtrlWeight.Size = new System.Drawing.Size(76, 23);
		this.txtSupPckMtrlWeight.TabIndex = 2;
		this.txtSupPckMtrlWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(315, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 2;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(797, 20);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(55, 15);
		this.label46.TabIndex = 113;
		this.label46.Text = "Gross Wt";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(436, 20);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(35, 15);
		this.label36.TabIndex = 107;
		this.label36.Text = "UOM";
		this.txtsupgrosswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupgrosswt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupgrosswt.Location = new System.Drawing.Point(798, 39);
		this.txtsupgrosswt.Name = "txtsupgrosswt";
		this.txtsupgrosswt.ShortcutsEnabled = false;
		this.txtsupgrosswt.Size = new System.Drawing.Size(102, 23);
		this.txtsupgrosswt.TabIndex = 1;
		this.txtsupgrosswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.labItemName.AutoSize = true;
		this.labItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labItemName.Location = new System.Drawing.Point(5, 20);
		this.labItemName.Name = "labItemName";
		this.labItemName.Size = new System.Drawing.Size(32, 15);
		this.labItemName.TabIndex = 105;
		this.labItemName.Text = "Item";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(598, 20);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(51, 15);
		this.label47.TabIndex = 111;
		this.label47.Text = "Supplier";
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(598, 37);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 0;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(210, 20);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(57, 15);
		this.label40.TabIndex = 107;
		this.label40.Text = "Crop Year";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(315, 20);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(77, 15);
		this.label41.TabIndex = 107;
		this.label41.Text = "Packing Type";
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
		this.grdsummary.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdsummary).BackColor = System.Drawing.Color.White;
		grdsummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsummary.DesignTimeLayout = grdsummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsummary).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdsummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsummary.GroupByBoxVisible = false;
		this.grdsummary.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdsummary).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdsummary).Name = "grdsummary";
		((ScrollableBase)this.grdsummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsummary).Size = new System.Drawing.Size(285, 218);
		((System.Windows.Forms.Control)(object)this.grdsummary).TabIndex = 0;
		this.grdsummary.TotalRow = (InheritableBoolean)1;
		this.grdsummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsummary).VisualStyle = (VisualStyle)4;
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
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.History.Controls.Add(this.panel8);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.History.Controls.Add(this.panel6);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(3);
		this.History.Size = new System.Drawing.Size(1176, 732);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 570);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1170, 159);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 47;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
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
		this.ctrlGrdBar4.MyGrid = this.grdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar4.TabIndex = 43;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
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
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
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
		this.BtnLoadAllHistory.Click += new System.EventHandler(btnLoaddAllHistory_Click);
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
		this.ctrlGrdBar3.MyGrid = this.grdHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 43;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1184, 761);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommisionBillDirectAndAgainstOrder";
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
		((System.ComponentModel.ISupportInitialize)this.grdrevenue).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.PanelDetail.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsummary).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
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
