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
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PurchaseTrading;

public class PurchaseInvoiceTradePro : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private int BaseCurrency;

	private double BaseRate;

	private Attachment AT = new Attachment();

	public DataTable dtGPGRNS = new DataTable();

	private int VoucherHeadId;

	public int RecId;

	public static int SupplierId;

	private int updateDetailIndex = -1;

	private bool IsDetailHasPurchaseOrderId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtTaxType = new DataTable();

	private DataTable dtSchedule = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtFreight = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlstForGrids = new DataTable();

	private DataTable dtAccountlstForFreight = new DataTable();

	public bool IsPO;

	public bool IsTaxable;

	public bool TaxPercntEditableOrNot;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool FreightDebitToExpenses;

	private bool MultiCurrencyFeatureVisibilty;

	private UserAccount UserAccount = new UserAccount();

	private LoadPurchaseOrderWithQty LoadPO;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel2;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnLoadPo;

	private Panel panel5;

	private Label label20;

	private Label label26;

	private Label label11;

	private Label label25;

	private Label label21;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	private Label label2;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Timer timer1;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grdSchedule;

	internal UltraCombo cmbstockprty;

	internal UltraCombo CmbTaxAccount;

	internal UltraCombo cmbcommagent;

	private TextBox txtSupplierGLId;

	private TextBox txtCommAgentId;

	private Label label9;

	private ToolStripButton btnSlip;

	private Panel panel14;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnFrmRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnDelete;

	internal UltraCombo combdeliverytrm;

	private Label label30;

	private RichTextBox txtcommremarks;

	internal UltraCombo cmbcommtype;

	internal UltraCombo cmbcommuom;

	private DateTimePicker GpDate;

	private TextBox txtVehicleNo;

	private Label label12;

	internal UltraCombo combrateuom;

	private Label label43;

	private Label label48;

	internal UltraCombo CmbWareHouse;

	private TextBox txtTaxAmount;

	private TextBox txtTotalAmount;

	private TextBox txtTaxPercent;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo combjob;

	internal UltraCombo CmbPackUOM;

	private Label label49;

	private Label label34;

	internal UltraCombo CmbItem;

	private Button btnplus;

	private TextBox txtGpNo;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtItemAmount;

	private TextBox txtqty;

	private Label label40;

	private Label label44;

	private Label label14;

	private Label label41;

	private Label label38;

	private Label label36;

	private Label label37;

	private Label label16;

	private Panel panel7;

	private Panel panel8;

	private Panel panel3;

	private Panel panel4;

	private Panel panel13;

	private Label label23;

	private GroupBox groupBox1;

	private Label label13;

	private TextBox txtdocno;

	private DateTimePicker DueDate;

	private DateTimePicker DocDate;

	internal UltraCombo combpttrm;

	private Label label18;

	private Label label1;

	private Label label5;

	private TextBox txtDueDays;

	private TextBox txtbillno;

	private Label label22;

	private Label label3;

	private Label label4;

	private Label label6;

	private TextBox txtBillAmount;

	private Label label10;

	internal UltraCombo cmbbranch;

	internal UltraCombo cmbsuppliername;

	internal UltraCombo cmbproject;

	private CheckBox ChkBok;

	private GroupBox groupBox2;

	private Label label27;

	private TextBox txtNetBillWeight;

	private Label label24;

	private TextBox txtAdLsWeight;

	private Label label17;

	private TextBox txtGrossWeight;

	internal GridEX grd;

	private Panel panel18;

	private Label label31;

	internal GridEX grdHistoryDetail;

	private Panel panel15;

	private Label label32;

	internal UltraCombo cmbtaxtype;

	private DateTimePicker datDeliveryStartDate;

	private TextBox txtDeliveryDays;

	private Label label33;

	private Label label35;

	private TextBox txtRateCutTotal;

	private Label label42;

	private TextBox txtRateCut;

	private Label label39;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnPartySlip;

	private CheckBox ChkVoucherPreview;

	private TabControl tabControl2;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Label label45;

	private Label label15;

	private TabPage tabPage3;

	private Label label46;

	private Panel panel11;

	private Panel panel10;

	private Panel panel9;

	internal GridEX grdFreight;

	private TextBox txtExchangeRate;

	private TextBox txtRemarks;

	private Label lblExchangeRate;

	private Label lblFcyAmount;

	private TextBox txtFcyAmount;

	internal UltraCombo cmbCurrency;

	private Label lblFcyCode;

	private Label label7;

	private Panel panel12;

	private GroupBox groupBox3;

	private Button btnshow;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnNewHistory;

	private Panel panel17;

	private Panel panel19;

	private Panel panel16;

	private RadioButton rdapproved;

	private RadioButton rdmodify;

	private RadioButton rddoc;

	private RadioButton rdentry;

	private Button button1;

	public PurchaseInvoiceTradePro(UserAccount user)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchaseInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 680);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
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
			dtAccountlstForGrids.Columns.Add("Id");
			dtAccountlstForGrids.Columns.Add("SupplierCustomerId");
			dtAccountlstForGrids.Columns.Add("AccountTitle");
			dtAccountlstForFreight.Columns.Add("Id");
			dtAccountlstForFreight.Columns.Add("SupplierCustomerId");
			dtAccountlstForFreight.Columns.Add("AccountTitle");
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			MultiCurrencyFeature();
			string FREIGHTDEBIT = CommonServices.GetConfigurationByOrgCompandConfigDescription("DebitAmountChargetoExpenseAcFreightGridPurchase");
			if (FREIGHTDEBIT != string.Empty)
			{
				FreightDebitToExpenses = Conversion.ToBool((object)FREIGHTDEBIT);
			}
			GetallTaxType();
			AccountsFill();
			CurrencyFill();
			TaxPercntEditableOrNot = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("TaxPercentEditable"));
			if (TaxPercntEditableOrNot)
			{
				txtTaxPercent.Enabled = true;
				txtTaxPercent.ReadOnly = false;
			}
			else
			{
				txtTaxPercent.Enabled = false;
				txtTaxPercent.ReadOnly = true;
			}
			AccountsFillforGrid();
			dtGPGRNS.Columns.Add("Id");
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderId", typeof(int));
			dtGrid.Columns.Add("PurchaseOrderDetailId", typeof(int));
			dtGrid.Columns.Add("OrderNo", typeof(string));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName", typeof(string));
			dtGrid.Columns.Add("ItemUomId", typeof(int));
			dtGrid.Columns.Add("PackUom", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("ValidateQty", typeof(double));
			dtGrid.Columns.Add("GrossWeight", typeof(double));
			dtGrid.Columns.Add("AdLsWeight", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("ItemRate", typeof(double));
			dtGrid.Columns.Add("OrderItemRateUOMId", typeof(int));
			dtGrid.Columns.Add("RateUOM", typeof(string));
			dtGrid.Columns.Add("EquivalentPoRate", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("RateCut", typeof(double));
			dtGrid.Columns.Add("RateCutAmount", typeof(double));
			dtGrid.Columns.Add("TaxId", typeof(int));
			dtGrid.Columns.Add("Tax%", typeof(double));
			dtGrid.Columns.Add("TaxAmount", typeof(double));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("FcyAmount", typeof(double));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("VehicleNo", typeof(string));
			dtGrid.Columns.Add("Freight", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Commission", typeof(double));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtGrdGL.Columns.Add("AccountId");
			dtGrdGL.Columns.Add("GlAccountId", typeof(int));
			dtGrdGL.Columns.Add("AccountTitle");
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtSchedule.Columns.Add("DueDays");
			dtSchedule.Columns.Add("DueDate");
			dtSchedule.Columns.Add("Due%", typeof(double));
			dtSchedule.Columns.Add("Amount", typeof(double));
			AddRowInScheduleGrid();
			grdSchedule.RetrieveStructure();
			ScheduleGridSetting();
			dtFreight.Columns.Add("Transporter");
			dtFreight.Columns.Add("GlAccountId");
			dtFreight.Columns.Add("Freight", typeof(double));
			dtFreight.Columns.Add("Debit", typeof(double));
			dtFreight.Columns.Add("Remarks");
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			BranchFill();
			ProjectFill();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			SupplierNameFilll();
			DocumentNo();
			ItemDetailFill();
			combojoblotfill();
			WareHouseFill();
			PreviousPurchaseTaxAc();
			btnSave.Visible = true;
			btnplus.Visible = true;
			btnUpdate.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			btnUpdateDetail.Visible = false;
			DocDate.Focus();
			ChkBok.Checked = true;
			DefaultConfigurations();
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1604"
			};
			DataTable dtSupplier = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					{
						"Supplier",
						typeof(string)
					}
				}
			};
			DataTable dt = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DefaultConfigurations()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				combjob.Value = JobId;
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				cmbCurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			MultiCurrencyFeatureVisibilty = CommonServices.GetERPFeatureById(6);
			if (MultiCurrencyFeatureVisibilty)
			{
				lblFcyCode.Visible = true;
				((Control)(object)cmbCurrency).Visible = true;
				lblExchangeRate.Visible = true;
				txtExchangeRate.Visible = true;
				lblFcyAmount.Visible = true;
				txtFcyAmount.Visible = true;
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				lblFcyCode.Visible = false;
				lblExchangeRate.Visible = false;
				lblFcyAmount.Visible = false;
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
			int id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurrency.Value = id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddRowInFreightGrid()
	{
		dtFreight.Rows.Add(0, 0, 0, 0, "");
		grdFreight.DataSource = dtFreight;
	}

	private void grdFreightSettings()
	{
		try
		{
			if (!FreightDebitToExpenses)
			{
				grdFreight.RootTable.Columns["Debit"].Visible = false;
			}
			else
			{
				grdFreight.RootTable.Columns["Debit"].Visible = true;
			}
			grdFreight.RootTable.Columns["GlAccountId"].Visible = false;
			grdFreight.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdFreight.RootTable.Columns["Freight"].Caption = "Credit";
			grdFreight.RootTable.Columns["Transporter"].ColumnType = (ColumnType)0;
			grdFreight.RootTable.Columns["Transporter"].HasValueList = true;
			grdFreight.RootTable.Columns["Transporter"].LimitToList = true;
			grdFreight.RootTable.Columns["Transporter"].EditType = (EditType)4;
			grdFreight.RootTable.Columns["Transporter"].Caption = "Transporter";
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdFreight.RootTable.Columns["Transporter"].ValueList.PopulateValueList((IEnumerable)dtAccountlstForFreight.DefaultView, "Id", "AccountTitle");
			}
			grdFreight.RootTable.Columns["Transporter"].Width = 250;
			grdFreight.RootTable.Columns["Freight"].Width = 50;
			grdFreight.RootTable.Columns.Add("Delete");
			grdFreight.RootTable.Columns["Delete"].Key = "Delete";
			grdFreight.RootTable.Columns["Delete"].Caption = "X";
			grdFreight.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Delete"].ButtonText = "X";
			grdFreight.RootTable.Columns["Delete"].Width = 20;
			grdFreight.RootTable.Columns.Add("Add");
			grdFreight.RootTable.Columns["Add"].Key = "Add";
			grdFreight.RootTable.Columns["Add"].Caption = "+";
			grdFreight.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFreight.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdFreight.RootTable.Columns["Add"].ButtonText = "+";
			grdFreight.RootTable.Columns["Add"].Width = 20;
			grdFreight.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdFreight.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdFreight.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdFreight.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item2 = grdFreight.CurrentRow;
				dtFreight.Rows[item2.RowIndex].Delete();
				if (item2 != null && (int)item2.RowType == 0)
				{
					item2.Delete();
				}
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				GridEXRow item = grdFreight.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					AddRowInFreightGrid();
					grdFreight.DataSource = dtFreight;
				}
			}
			grdFreight.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow item = grdFreight.CurrentRow;
			if (e.Column.Key == "Transporter")
			{
				SupCustIdUpdateforFreightGrid();
			}
			if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
			{
				double Freight = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Freight"].Value = Freight;
			}
			if (FreightDebitToExpenses)
			{
				if (e.Column.Key == "Freight" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Freight"].Value = Math.Round(Conversion.ToDouble(item.Cells["Freight"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
					{
						item.Cells["Freight"].Value = 0;
						MessageBox.Show("Debit Side is aleady added");
					}
				}
				if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
				{
					item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					if (Conversion.ToDouble((object)item.Cells["Freight"].Value.ToString()) > 0.0)
					{
						item.Cells["Debit"].Value = 0;
						MessageBox.Show("Credit Side is aleady added");
					}
				}
			}
			grdFreight.UpdateData();
			BillAmount();
			FreightProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupCustIdUpdateforFreightGrid()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFreight.UpdateData();
			GridEXRow r = grdFreight.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				string selectedAcId = Conversion.ToString((object)r.Cells["Transporter"].Text);
				if (selectedAcId != "0" && selectedAcId != string.Empty)
				{
					DataRow[] matchingRows = dtAccountlstForFreight.Select("AccountTitle='" + selectedAcId + "'");
					if (matchingRows.Length != 0)
					{
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
						r.EndEdit();
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(((UltraDropDownBase)cmbsuppliername).SelectedRow.Cells[2].Value) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Selected Account Can not be Same As Supplier Account");
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = 0;
						r.Cells["Transporter"].Value = 0;
						r.EndEdit();
						return;
					}
				}
				else if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(r.Cells["Transporter"].Value))
				{
					MessageBox.Show("Selected Account Can not be Same As Supplier Account");
					r.BeginEdit();
					r.Cells["Transporter"].Value = 0;
					r.Cells["GlAccountId"].Value = 0;
					r.EndEdit();
					return;
				}
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFreight_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFreight.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdFreight.CurrentColumn.Key == "Delete")
				{
					dtFreight.Rows[r.RowIndex].Delete();
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					r.Delete();
					if (dtFreight.Rows.Count == 0)
					{
						AddRowInFreightGrid();
					}
				}
				else if (grdFreight.CurrentColumn.Key == "Add")
				{
					AddRowInFreightGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtFreight.Rows[r.RowIndex].Delete();
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				BillAmount();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInFreightGrid();
				BillAmount();
			}
			if (e.KeyCode == Keys.F1 && grdFreight.CurrentColumn != null && grdFreight.CurrentColumn.Key == "Transporter")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccountlstForFreight, "SupplierCustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["Transporter"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlstForFreight, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["Transporter"].Value = obj2.ReturnId;
				}
				SupCustIdUpdateforFreightGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	public void grdInvExpSettings()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		try
		{
			List<InventoryItemsOther> lst = new List<InventoryItemsOther>();
			DataTable dt = InventoryItemsOther.GetAll(new InventoryItemsOther
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					InventoryItemsOther items = new InventoryItemsOther();
					items.Id = Conversion.ToInt(dt.Rows[i]["Id"]);
					items.OtherItemName = dt.Rows[i]["OtherItemName"].ToString();
					lst.Add(items);
				}
			}
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)lst, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			grdInvExp.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				double Amount = num * Rate;
				item.Cells["Amount"].Value = Amount;
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, 0, "", "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	public void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["AccountId"].Visible = false;
			grdGLedger.RootTable.Columns["GlAccountId"].Visible = false;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = "#,##0.###";
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = "#,##0.###";
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "X";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "X";
			grdGLedger.RootTable.Columns["Delete"].Width = 20;
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "+";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "+";
			grdGLedger.RootTable.Columns["Add"].Width = 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Debit" || e.Column.Key == "Credit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdGLedger_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdGLedger.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdGLedger.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdGLedger.CurrentColumn.Key == "Delete")
				{
					if (item != null && (int)item.RowType == 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						dtGrdGL.Rows[item.RowIndex].Delete();
						item.Delete();
					}
					if (dtGrdGL.Rows.Count == 0)
					{
						AddRowInGLGrid();
					}
				}
				if (grdGLedger.CurrentColumn.Key == "Add")
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				BillAmount();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGLGrid();
				BillAmount();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdGLedger.CurrentColumn != null && (grdGLedger.CurrentColumn.Key == "AccountId" || grdGLedger.CurrentColumn.Key == "GlAccountId" || grdGLedger.CurrentColumn.Key == "AccountTitle"))
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					DataTable dt = CommonServices.GetVendorsAndCustomers();
					GrdPopUp obj = new GrdPopUp(dt, "Id", "CompanyName");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					Name = Conversion.ToString((object)obj.ReturnName);
					if (Id > 0)
					{
						item.Cells["AccountId"].Value = Id;
						item.Cells["AccountTitle"].Value = Name;
						DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Id);
						int GlAccountId = ((foundRow != null) ? Conversion.ToInt(foundRow["GlAccountId"]) : 0);
						item.Cells["GlAccountId"].Value = GlAccountId;
						if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(item.Cells["AccountId"].Value))
						{
							MessageBox.Show("Selected Account Can not be Same As Supplier Account");
							item.Cells["AccountId"].Value = 0;
							item.Cells["GlAccountId"].Value = 0;
							item.Cells["AccountTitle"].Value = "";
							return;
						}
					}
					else
					{
						item.Cells["AccountId"].Value = 0;
						item.Cells["GlAccountId"].Value = 0;
						item.Cells["AccountTitle"].Value = "";
					}
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccountlstForGrids, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					Name = Conversion.ToString((object)obj2.ReturnName);
					if (Id > 0)
					{
						item.Cells["GlAccountId"].Value = Id;
						item.Cells["AccountId"].Value = 0;
						item.Cells["AccountTitle"].Value = Name;
						if (Conversion.ToInt(((UltraDropDownBase)cmbsuppliername).SelectedRow.Cells[2].Value) == Conversion.ToInt(item.Cells["GlAccountId"].Value))
						{
							MessageBox.Show("Selected Account Can not be Same As Supplier Account");
							item.Cells["GlAccountId"].Value = 0;
							item.Cells["AccountId"].Value = 0;
							item.Cells["AccountTitle"].Value = "";
							return;
						}
					}
					else
					{
						item.Cells["GlAccountId"].Value = 0;
						item.Cells["AccountId"].Value = 0;
						item.Cells["AccountTitle"].Value = "";
					}
				}
			}
			grdGLedger.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdGLedger);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Qty * Rate;
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
			{
				item.Cells["Credit"].Value = 0;
				MessageBox.Show("Debit Side is aleady added");
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0 && Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
			{
				item.Cells["Debit"].Value = 0;
				MessageBox.Show("Credit Side is aleady added");
			}
			grdGLedger.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AddRowInScheduleGrid()
	{
		try
		{
			dtSchedule.Rows.Add("", "", 0, 0);
			grdSchedule.DataSource = dtSchedule;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ScheduleGridSetting()
	{
		try
		{
			grdSchedule.RootTable.Columns["DueDays"].Width = 50;
			grdSchedule.RootTable.Columns["DueDate"].Width = 100;
			grdSchedule.RootTable.Columns["Due%"].Width = 50;
			grdSchedule.RootTable.Columns["Amount"].Width = 80;
			grdSchedule.RootTable.Columns["DueDate"].EditType = (EditType)3;
			grdSchedule.RootTable.Columns.Add("Delete");
			grdSchedule.RootTable.Columns["Delete"].Key = "Delete";
			grdSchedule.RootTable.Columns["Delete"].Caption = "X";
			grdSchedule.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdSchedule.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdSchedule.RootTable.Columns["Delete"].ButtonText = "X";
			grdSchedule.RootTable.Columns["Delete"].Width = 20;
			grdSchedule.RootTable.Columns.Add("Add");
			grdSchedule.RootTable.Columns["Add"].Key = "Add";
			grdSchedule.RootTable.Columns["Add"].Caption = "+";
			grdSchedule.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdSchedule.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdSchedule.RootTable.Columns["Add"].ButtonText = "+";
			grdSchedule.RootTable.Columns["Add"].Width = 20;
			grdSchedule.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSchedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdSchedule.UpdateData();
			GridEXRow item = grdSchedule.CurrentRow;
			if (e.Column.Key == "DueDays")
			{
				item.Cells["DueDate"].Value = DocDate.Value.AddDays(Conversion.ToInt((object)item.Cells["DueDays"].Text.ToString()));
			}
			if (e.Column.Key == "Due%")
			{
				if (Conversion.ToInt(item.Cells["Due%"].Value) > 100)
				{
					MessageBox.Show("Percent should be less or equal then 100");
					return;
				}
				double percent = double.Parse(grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString()) * (double.Parse(item.Cells["Due%"].Value.ToString()) / 100.0);
				item.Cells["Amount"].Value = Math.Round(percent, 2);
			}
			grdSchedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetallTaxType()
	{
		try
		{
			dtTaxType.Rows.Clear();
			dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
			if (dtTaxType.Rows.Count > 0)
			{
				DDL.BindDDL(dtTaxType, cmbtaxtype, "TaxNameId", "TaxName", "Tax Type", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["TaxId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["TaxId"].HasValueList = true;
			grd.RootTable.Columns["TaxId"].LimitToList = true;
			grd.RootTable.Columns["TaxId"].EditType = (EditType)(TaxPercntEditableOrNot ? 4 : 0);
			grd.RootTable.Columns["TaxId"].Caption = "TaxName";
			grd.RootTable.Columns["TaxId"].ValueList.PopulateValueList((IEnumerable)dtTaxType.DefaultView, "TaxNameId", "TaxName");
			grd.RootTable.Columns["TaxId"].Width = 60;
			DataTable dt = CommonServices.WareHouseGetAllService();
			grd.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouseId"].HasValueList = true;
			grd.RootTable.Columns["WareHouseId"].LimitToList = true;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouseId"].Caption = "Warehouse";
			grd.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WareHouseId"].Width = 250;
			DataTable dtJob = CommonServices.JobLotGetAllService();
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "Job Lot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["JobLotId"].Width = 250;
			grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
			grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["PurchaseOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["PurchaseOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["EquivalentPoRate"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderItemRateUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Freight"].EditType = (EditType)0;
			grd.RootTable.Columns["Journal"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCutAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ValidateQty"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["Commission"].EditType = (EditType)0;
			grd.RootTable.Columns["RateCutAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Tax%"].EditType = (EditType)(TaxPercntEditableOrNot ? 1 : 0);
			grd.RootTable.Columns["TaxAmount"].EditType = (EditType)0;
			if (IsDetailHasPurchaseOrderId)
			{
				grd.RootTable.Columns["OrderNo"].Visible = true;
				grd.RootTable.Columns["GpDate"].EditType = (EditType)2;
				grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
				grd.RootTable.Columns.Add("Add");
				grd.RootTable.Columns["Add"].Key = "Add";
				grd.RootTable.Columns["Add"].Caption = "+";
				grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Add"].ButtonText = "+";
				grd.RootTable.Columns["Add"].Width = 20;
			}
			else
			{
				grd.RootTable.Columns["OrderNo"].Visible = false;
				grd.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
				grd.FrozenColumns = 1;
			}
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			if (IsDetailHasPurchaseOrderId)
			{
				grd.RootTable.Columns["Add"].Position = 0;
				grd.RootTable.Columns["Delete"].Position = 1;
				grd.FrozenColumns = 2;
			}
			else
			{
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.FrozenColumns = 1;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderId"].Visible = false;
			grd.RootTable.Columns["ValidateQty"].Visible = false;
			grd.RootTable.Columns["PurchaseOrderDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemUomId"].Visible = false;
			grd.RootTable.Columns["EquivalentPoRate"].Visible = false;
			grd.RootTable.Columns["OrderItemRateUOMId"].Visible = false;
			grd.RootTable.Columns["Commission"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["RateCutAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateCutAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateCutAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["RateCutAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = "#,##0.####";
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Tax%"].FormatString = "#,##0.####";
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "ItemRate" || e.Column.Key == "RateCut" || e.Column.Key == "EquivalentPoRate" || e.Column.Key == "AdLsWeight")
			{
				if (item != null && (int)item.RowType == 0)
				{
					string RateCut = item.Cells["RateCut"].Value.ToString();
					string GrossWeight = item.Cells["GrossWeight"].Value.ToString();
					double AdLsWeight = Conversion.ToDouble(item.Cells["AdLsWeight"].Value);
					double NetBillWeight = 0.0;
					NetBillWeight = ((AdLsWeight == 0.0) ? (Conversion.ToDouble((object)GrossWeight) + Math.Abs(AdLsWeight)) : (Conversion.ToDouble((object)GrossWeight) + AdLsWeight));
					item.Cells["NetBillWeight"].Value = NetBillWeight.ToString();
					string EquivalentPoRate = item.Cells["EquivalentPoRate"].Text.ToString();
					double RateCutAmount = Conversion.ToDouble((object)NetBillWeight) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)RateCut);
					item.Cells["RateCutAmount"].Value = RateCutAmount;
					string ItemRate = item.Cells["ItemRate"].Value.ToString();
					double Amount = Conversion.ToDouble((object)item.Cells["NetBillWeight"].Value.ToString()) / Conversion.ToDouble((object)EquivalentPoRate) * Conversion.ToDouble((object)ItemRate);
					item.Cells["ItemAmount"].Value = Amount - RateCutAmount;
				}
			}
			else if (e.Column.Key == "TaxId")
			{
				int selectedTaxId = Conversion.ToInt(item.Cells["TaxId"].Value);
				if (selectedTaxId == 0)
				{
					item.Cells["Tax%"].Value = 0;
				}
				else
				{
					DataRow[] matchingRows = dtTaxType.Select("TaxNameId = " + selectedTaxId);
					if (matchingRows.Length != 0)
					{
						item.BeginEdit();
						item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows[0]["TaxPercent"]);
						item.EndEdit();
					}
				}
			}
			else if (e.Column.Key == "Tax%" && Conversion.ToDouble(item.Cells["Tax%"].Value) == 0.0)
			{
				int selectedTaxId2 = Conversion.ToInt(item.Cells["TaxId"].Value);
				DataRow[] matchingRows2 = dtTaxType.Select("TaxNameId = " + selectedTaxId2);
				if (matchingRows2.Length != 0)
				{
					item.BeginEdit();
					item.Cells["Tax%"].Value = Conversion.ToDouble(matchingRows2[0]["TaxPercent"]);
					item.EndEdit();
				}
			}
			int TaxType = Conversion.ToInt(item.Cells["TaxId"].Value);
			double TaxPercent = Conversion.ToDouble(item.Cells["Tax%"].Value);
			double ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
			double TaxAmount = 0.0;
			if (TaxPercent > 100.0)
			{
				MessageBox.Show("Tax % Can't be greater than 100...");
				TaxPercent = 100.0;
				item.Cells["Tax%"].Value = TaxPercent;
			}
			if (ItemAmount > 0.0 && TaxPercent > 0.0 && TaxType > 0)
			{
				TaxAmount = ItemAmount * TaxPercent / 100.0;
				item.BeginEdit();
				item.Cells["TaxAmount"].Value = Math.Round(TaxAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.EndEdit();
			}
			else
			{
				item.Cells["Tax%"].Value = 0;
				item.Cells["TaxAmount"].Value = 0;
			}
			grd.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtGrid.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(combjob.Value);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtGrid.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)CmbItem).Text);
				dtGrid.Rows[updateDetailIndex]["ItemUomId"] = Conversion.ToInt(CmbPackUOM.Value);
				dtGrid.Rows[updateDetailIndex]["PackUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUOM).Text);
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtqty.Text);
				dtGrid.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtGrid.Rows[updateDetailIndex]["AdLsWeight"] = Conversion.ToDouble((object)txtAdLsWeight.Text);
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = Conversion.ToDouble((object)txtNetBillWeight.Text);
				dtGrid.Rows[updateDetailIndex]["OrderItemRateUOMId"] = Conversion.ToInt(combrateuom.Value);
				dtGrid.Rows[updateDetailIndex]["EquivalentPoRate"] = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["RateUOM"] = Conversion.ToString((object)((Control)(object)combrateuom).Text);
				dtGrid.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				dtGrid.Rows[updateDetailIndex]["RateCut"] = Conversion.ToDouble((object)txtRateCut.Text);
				dtGrid.Rows[updateDetailIndex]["RateCutAmount"] = Conversion.ToDouble((object)txtRateCutTotal.Text);
				dtGrid.Rows[updateDetailIndex]["TaxId"] = Conversion.ToInt(cmbtaxtype.Value);
				dtGrid.Rows[updateDetailIndex]["Tax%"] = Conversion.ToDouble((object)txtTaxPercent.Text);
				dtGrid.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDouble((object)txtTaxAmount.Text);
				dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtGpNo.Text);
				dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)GpDate.Value).ToShortDateString();
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtVehicleNo.Text);
				grd.DataSource = dtGrid;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
				BillAmount();
				CommonServices.GridAutoAdjustmentNew(grd);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			tabControl1.SelectedIndex = 0;
			GridEXRow item = grd.CurrentRow;
			if (Conversion.ToInt(item.Cells["PurchaseOrderId"].Value) > 0)
			{
				((Control)(object)CmbItem).Enabled = false;
				txtRate.Enabled = false;
			}
			else
			{
				((Control)(object)CmbItem).Enabled = true;
				txtRate.Enabled = true;
			}
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["WareHouseId"].Value) > 0)
				{
					CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				}
				combjob.Value = item.Cells["JobLotId"].Value.ToString();
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbItem).Text = item.Cells["ItemName"].Text.ToString();
				CmbItem_Leave(null, null);
				CmbPackUOM.Value = item.Cells["ItemUomId"].Value.ToString();
				((Control)(object)CmbPackUOM).Text = item.Cells["PackUOM"].Text.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtAdLsWeight.Text = item.Cells["AdLsWeight"].Value.ToString();
				txtNetBillWeight.Text = item.Cells["NetBillWeight"].Value.ToString();
				txtRate.Text = item.Cells["ItemRate"].Value.ToString();
				combrateuom.Value = item.Cells["OrderItemRateUOMId"].Value.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRateCut.Text = item.Cells["RateCut"].Value.ToString();
				txtRateCutTotal.Text = item.Cells["RateCutAmount"].Value.ToString();
				if (Conversion.ToInt(item.Cells["TaxId"].Value) > 0)
				{
					cmbtaxtype.Value = item.Cells["TaxId"].Value.ToString();
				}
				txtTaxPercent.Text = item.Cells["Tax%"].Value.ToString();
				txtTaxAmount.Text = item.Cells["TaxAmount"].Value.ToString();
				txtGpNo.Text = item.Cells["GpNo"].Text.ToString();
				GpDate.Value = Conversion.ToDateTime(item.Cells["GpDate"].Value);
				txtVehicleNo.Text = item.Cells["VehicleNo"].Text.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouse.Focus();
				TotalAmount();
				CalculateTaxAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && IsDetailHasPurchaseOrderId)
			{
				ResetDetail();
				throw new Exception("You Can't Add A Direct Entry Because Record Against Purchase Order Already Present");
			}
			if (FormValidationDetail())
			{
				dtGrid.Rows.Add(Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToString((object)0), Conversion.ToInt(CmbWareHouse.Value), Conversion.ToInt(combjob.Value), Conversion.ToInt(CmbItem.Value), Conversion.ToString((object)((Control)(object)CmbItem).Text), Conversion.ToInt(CmbPackUOM.Value), Conversion.ToString((object)((Control)(object)CmbPackUOM).Text), Conversion.ToDouble((object)txtqty.Text), Conversion.ToDouble((object)txtqty.Text), Conversion.ToDouble((object)txtGrossWeight.Text), Conversion.ToDouble((object)txtAdLsWeight.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToInt(combrateuom.Value), Conversion.ToString((object)((Control)(object)combrateuom).Text), Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtItemAmount.Text), Conversion.ToDouble((object)txtRateCut.Text), Conversion.ToDouble((object)txtRateCutTotal.Text), Conversion.ToInt(cmbtaxtype.Value), Conversion.ToDouble((object)txtTaxPercent.Text), Conversion.ToDouble((object)txtTaxAmount.Text), Conversion.ToDouble((object)txtTotalAmount.Text), Conversion.ToDouble((object)0), Conversion.ToInt((object)txtGpNo.Text), Conversion.ToDateTime((object)GpDate.Value), Conversion.ToString((object)txtVehicleNo.Text), Conversion.ToDouble((object)0), Conversion.ToDouble((object)0));
				IsDetailHasPurchaseOrderId = false;
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				txtExchangeRate_TextChanged(null, null);
				FreightProportion();
				ExpProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		grd.UpdateData();
		TotalCommissionAmount();
		FreightProportion();
		BillAmount();
		grd.UpdateData();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Add")
				{
					DataRow dr = dtGrid.Rows[r.RowIndex];
					dtGrid.Rows.Add(dr.ItemArray);
					grd.DataSource = dtGrid;
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						dtGrid.Rows[dtGrid.Rows.Count - 1]["Id"] = 0;
					}
				}
				else if (e.Column.Key == "Delete")
				{
					DeleteDetailrow(r);
				}
			}
			if (grd.GetRows().Count() == 0 && IsDetailHasPurchaseOrderId)
			{
				Reset();
			}
			CommissionProportion();
			FreightProportion();
			ExpProportion();
			txtExchangeRate_TextChanged(null, null);
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailrow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				InvPurchaseInvoice.StockInReferenceValidationReferredOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, 1604, RecId, Id);
			}
			r.Delete();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ExpProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["ItemQty"].Value.ToString());
				obj.Cells["Expense"].Value = (TotalAmount / NetWeight * Weight).ToString();
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble(r.Cells["ItemAmount"].Value) + Conversion.ToDouble(r.Cells["TaxAmount"].Value) + Conversion.ToDouble(r.Cells["Expense"].Value) + Conversion.ToDouble(r.Cells["Commission"].Value);
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CommissionProportion()
	{
	}

	public void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2));
			double TaxAmount = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2));
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			double TransporterCredit = 0.0;
			double TransporterDebit = 0.0;
			GridEXRow[] rows = grdFreight.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["Transporter"].Value) <= 0 && Conversion.ToInt(r2.Cells["GlAccountId"].Value) <= 0)
				{
					continue;
				}
				if (SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(r2.Cells["Transporter"].Value))
					{
						TransporterCredit += Conversion.ToDouble(r2.Cells["Freight"].Value);
						TransporterDebit += Conversion.ToDouble(r2.Cells["Debit"].Value);
					}
				}
				else if (Conversion.ToInt(cmbsuppliername.Value) > 0 && Conversion.ToInt(((UltraDropDownBase)cmbsuppliername).SelectedRow.Cells[2].Value) == Conversion.ToInt(r2.Cells["GlAccountId"].Value))
				{
					TransporterCredit += Conversion.ToDouble(r2.Cells["Freight"].Value);
					TransporterDebit += Conversion.ToDouble(r2.Cells["Debit"].Value);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 || Conversion.ToInt(r.Cells["GlAccountId"].Value) > 0)
				{
					JournalDebit += Conversion.ToDouble(r.Cells["Debit"].Value);
					JournalCredit += Conversion.ToDouble(r.Cells["Credit"].Value);
				}
			}
			BillAmount = ItemAmount + TaxAmount + ExpAmount;
			BillAmount = BillAmount + JournalDebit - JournalCredit;
			BillAmount = BillAmount + TransporterCredit - TransporterDebit;
			if (Conversion.ToInt(cmbsuppliername.Value) == Conversion.ToInt(cmbcommagent.Value))
			{
				BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			txtBillAmount.Text = BillAmount.ToString("#,##0.####");
			BillProportion();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty && ((Control)(object)cmbcommtype).Text == "Flat")
			{
				double CommRate3 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				txtcommamount.Text = CommRate3.ToString();
			}
			if (txtcommrate.Text != string.Empty && ((Control)(object)cmbcommtype).Text == "Percent")
			{
				double CommRate2 = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
					txtcommamount.Text = CommAmt2.ToString("#,##0.####");
				}
			}
			if (txtcommrate.Text != string.Empty && ((Control)(object)cmbcommtype).Text == "CommWeight")
			{
				double CommRate = Conversion.ToDouble((object)txtcommrate.Text.Trim());
				object Weight = grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2);
				if (Weight != null)
				{
					double num = Conversion.ToDouble((object)Weight.ToString());
					double UOM = Conversion.ToDouble((object)((Control)(object)cmbcommuom).Text.Trim());
					double CommAmt = num / UOM * CommRate;
					txtcommamount.Text = CommAmt.ToString("#,##0.####");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		CommissionProportion();
	}

	public void CalculateTaxAmount()
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					txtTaxPercent.Text = TaxPercent.ToString();
				}
				double num = Conversion.ToDouble((object)txtItemAmount.Text);
				double TaxAmt = num * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString("#,##0.####");
				double TotalAmount = num + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString("#,##0.####");
			}
			else
			{
				txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString("#,##.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			double RateUOM = 0.0;
			double Rate = 0.0;
			double RateCut = 0.0;
			double NetWeight = 0.0;
			double ItemAmount = 0.0;
			double RateCutTotal = 0.0;
			if (((UltraGridBase)combrateuom).ActiveRow != null && ((Control)(object)combrateuom).Text != string.Empty)
			{
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value);
			}
			if (txtRate.Text.Trim() != string.Empty)
			{
				Rate = double.Parse(txtRate.Text);
			}
			if (txtNetBillWeight.Text.Trim() != string.Empty)
			{
				NetWeight = double.Parse(txtNetBillWeight.Text);
			}
			RateCut = Conversion.ToDouble((object)txtRateCut.Text);
			RateCutTotal = Conversion.ToDouble((object)txtRateCutTotal.Text);
			if (NetWeight > 0.0 && RateUOM > 0.0)
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "RateCutTotal")
				{
					RateCut = ((RateCutTotal > 0.0) ? (RateCutTotal / (NetWeight / RateUOM)) : 0.0);
					txtRateCut.Text = RateCut.ToString("#,##0.####");
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "RateCut")
				{
					RateCutTotal = ((NetWeight > 0.0) ? (NetWeight / RateUOM * RateCut) : 0.0);
					txtRateCutTotal.Text = RateCutTotal.ToString("#,##0.####");
				}
				else
				{
					RateCut = ((RateCutTotal > 0.0) ? (RateCutTotal / (NetWeight / RateUOM)) : 0.0);
					txtRateCut.Text = RateCut.ToString("#,##0.####");
				}
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				ItemAmount -= RateCutTotal;
				txtItemAmount.Text = ItemAmount.ToString("#,##0.####");
				txtTotalAmount.Text = ItemAmount.ToString("#,##0.####");
			}
			else
			{
				txtItemAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			double GrossWeight = 0.0;
			double AddLess = 0.0;
			if (((Control)(object)CmbPackUOM).Text != string.Empty)
			{
				Conversion.ToDouble(((UltraDropDownBase)CmbPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				double.Parse(txtqty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = double.Parse(txtGrossWeight.Text.Trim().ToString());
			}
			if (txtNetBillWeight.Text != string.Empty && txtNetBillWeight.Text != null)
			{
				double.Parse(txtNetBillWeight.Text.Trim().ToString());
			}
			AddLess = ((!(txtAdLsWeight.Text != string.Empty) || txtAdLsWeight.Text == null) ? 0.0 : Conversion.ToDouble((object)txtAdLsWeight.Text.Trim()));
			double Weight = 0.0;
			txtNetBillWeight.Text = (GrossWeight + AddLess).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				double PackUOM = 0.0;
				double Qty = 0.0;
				double GrossWeight = 0.0;
				if (((Control)(object)CmbPackUOM).Text != string.Empty)
				{
					PackUOM = Conversion.ToDouble(((UltraDropDownBase)CmbPackUOM).SelectedRow.Cells[2].Value);
				}
				if (txtqty.Text != string.Empty)
				{
					Qty = double.Parse(txtqty.Text.ToString());
				}
				if (txtGrossWeight.Text != string.Empty)
				{
					GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
				}
				txtGrossWeight.Text = (PackUOM * Qty).ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		TotalAmount();
		CalculateTaxAmount();
		TotalCommissionAmount();
	}

	private void combrateuom_Leave(object sender, EventArgs e)
	{
		TotalAmount();
		CalculateTaxAmount();
		TotalCommissionAmount();
	}

	private void CmbPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalWeight();
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdLsWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateCut_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdFreight.GetTotal(grdFreight.RootTable.Columns["Freight"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				double TotalDiffernce = CreditForFreight;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble(obj.Cells["NetBillWeight"].Value);
					obj.Cells["Freight"].Value = TotalDiffernce / NetWeight * Weight;
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Freight"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbbranch).Text.Trim() == string.Empty || ((Control)(object)cmbbranch).Text.Trim() == "0")
		{
			MessageBox.Show("Branch Name is Required");
			cmbbranch.Focus();
			return false;
		}
		if (((Control)(object)cmbproject).Text.Trim() == string.Empty || ((Control)(object)cmbproject).Text.Trim() == "0")
		{
			MessageBox.Show("Project Name is Required");
			cmbproject.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No is Required");
			txtdocno.Focus();
			return false;
		}
		if (((Control)(object)cmbsuppliername).Text.Trim() == string.Empty || ((Control)(object)cmbsuppliername).Text.Trim() == "0")
		{
			MessageBox.Show("Supplier Name is Required");
			cmbsuppliername.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term is Required");
			combpttrm.Focus();
			return false;
		}
		if (Conversion.ToInt(combpttrm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term is Required");
			combdeliverytrm.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((Control)(object)CmbWareHouse).Text.Trim() == string.Empty || ((Control)(object)CmbWareHouse).Text.Trim() == "0")
		{
			MessageBox.Show("WareHouse Name is Required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((Control)(object)CmbItem).Text.Trim() == string.Empty || ((Control)(object)CmbItem).Text.Trim() == "0")
		{
			MessageBox.Show("Item Name is Required");
			CmbItem.Focus();
			return false;
		}
		if (((Control)(object)CmbPackUOM).Text.Trim() == string.Empty || ((Control)(object)CmbPackUOM).Text.Trim() == "0")
		{
			MessageBox.Show("Pack UOM is Required");
			CmbPackUOM.Focus();
			return false;
		}
		if (((Control)(object)combjob).Text.Trim() == string.Empty || ((Control)(object)combjob).Text.Trim() == "0")
		{
			MessageBox.Show("Job/Lot Name is Required");
			combjob.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || txtqty.Text.Trim() == "0" || txtqty.Text.Trim() == null)
		{
			MessageBox.Show("Qty is Required");
			txtqty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || txtRate.Text.Trim() == "0" || txtRate.Text.Trim() == null)
		{
			MessageBox.Show("Rate is Required");
			txtRate.Focus();
			return false;
		}
		if (((Control)(object)combrateuom).Text.Trim() == string.Empty || ((Control)(object)combrateuom).Text.Trim() == "0" || ((Control)(object)combrateuom).Text.Trim() == null)
		{
			MessageBox.Show("RateUOM is Required");
			combrateuom.Focus();
			return false;
		}
		if (txtItemAmount.Text.Trim() == string.Empty || txtItemAmount.Text.Trim() == "0" || txtItemAmount.Text.Trim() == null)
		{
			MessageBox.Show("Item Amount is Required");
			txtItemAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0)
		{
			if (txtTaxPercent.Text.Trim() == string.Empty || txtTaxPercent.Text.Trim() == "0" || txtTaxPercent.Text.Trim() == null)
			{
				MessageBox.Show("Tax Percent is Required");
				txtTaxPercent.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0 && ((UltraGridBase)cmbtaxtype).ActiveRow == null)
			{
				MessageBox.Show("Tax Name is Required");
				cmbtaxtype.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void PreviousPurchaseTaxAc()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int PurchaseTaxAcId = InvPurchaseInvoice.GetLastPurchaseTaxAcId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (PurchaseTaxAcId > 0)
			{
				CmbTaxAccount.Value = PurchaseTaxAcId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, cmbbranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)cmbbranch).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbproject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)cmbproject).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbsuppliername, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtSupplier, cmbcommagent, "Id", "CompanyName", "Commission Agent Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", false);
				((UltraGridBase)combpttrm).Rows[1].Activate();
				((UltraGridBase)combpttrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "CommWeight");
			DDL.BindDDL(dt, cmbcommtype, "Id", "CommType", "Comm Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "5");
			dt.Rows.Add(2, "25");
			dt.Rows.Add(3, "40");
			dt.Rows.Add(4, "50");
			dt.Rows.Add(5, "60");
			dt.Rows.Add(6, "80");
			dt.Rows.Add(7, "100");
			DDL.BindDDL(dt, cmbcommuom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)cmbcommuom).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			DataTable data = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("3,6,8,16,17,18,19");
			if (data.Rows.Count > 0)
			{
				DDL.BindDDL(data, CmbTaxAccount, "Id", "AccountTitle", "AccountTitle", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFillforGrid()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomers(1);
				if (dt.Rows.Count > 0)
				{
					dtAccountlstForGrids.Clear();
					for (int j = 0; j < dt.Rows.Count; j++)
					{
						dtAccountlstForGrids.Rows.Add(Conversion.ToInt(dt.Rows[j]["GlAccountId"]), Conversion.ToInt(dt.Rows[j]["Id"]), Conversion.ToString(dt.Rows[j]["CompanyName"]));
					}
				}
				DataTable dt3 = SupplierCustomer.GetVendorsAndCustomersForTransporter(UserAccount.OrganizationId, UserAccount.CompanyId);
				if (dt3.Rows.Count > 0)
				{
					dtAccountlstForFreight.Clear();
					for (int i = 0; i < dt3.Rows.Count; i++)
					{
						dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dt3.Rows[i]["GlAccountId"]), Conversion.ToInt(dt3.Rows[i]["Id"]), Conversion.ToString(dt3.Rows[i]["CompanyName"]));
					}
				}
				return;
			}
			dtAccountlstForGrids.Rows.Clear();
			dtAccountlstForFreight.Rows.Clear();
			DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
			if (dt2.Rows.Count > 0)
			{
				dtAccountlstForGrids.Clear();
				for (int l = 0; l < dt2.Rows.Count; l++)
				{
					dtAccountlstForGrids.Rows.Add(Conversion.ToInt(dt2.Rows[l]["Id"]), 0, Conversion.ToString(dt2.Rows[l]["AccountTitle"]));
				}
			}
			if (!FreightDebitToExpenses)
			{
				dtAccountlstForFreight.Clear();
				dtAccountlstForFreight = dtAccountlstForGrids;
				return;
			}
			DataTable dtfreight = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,15,22");
			if (dtfreight.Rows.Count > 0)
			{
				dtAccountlstForFreight.Clear();
				for (int k = 0; k < dtfreight.Rows.Count; k++)
				{
					dtAccountlstForFreight.Rows.Add(Conversion.ToInt(dtfreight.Rows[k]["Id"]), 0, Conversion.ToString(dtfreight.Rows[k]["AccountTitle"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		try
		{
			int Code = CommonServices.PurchaseInvoiceGenerateCode(1604);
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
				return;
			}
			throw new Exception("Max Number Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable lst = CommonServices.getActiveWareHouse();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbWareHouse, "Id", "WareHouseName", "WareHouse Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable dtitem = CommonServices.ItemGetForComboServiceBindForEngr();
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TaxScheduleGet()
	{
		try
		{
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(CmbItem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTax = new DataTable();
				dtTax.Columns.Add("Id");
				dtTax.Columns.Add("TaxType");
				dtTax.Columns.Add("TaxPrcnt");
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, cmbtaxtype, "Id", "TaxType", "TaxType", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else if (dtTaxType.Rows.Count > 0)
			{
				DataTable dtTax2 = new DataTable();
				dtTax2.Columns.Add("Id");
				dtTax2.Columns.Add("TaxType");
				dtTax2.Columns.Add("TaxPrcnt");
				for (int i = 0; i < dtTaxType.Rows.Count; i++)
				{
					dtTax2.Rows.Add(dtTaxType.Rows[i]["TaxNameId"], dtTaxType.Rows[i]["TaxName"], dtTaxType.Rows[i]["TaxPercent"]);
				}
				DDL.BindDDL(dtTax2, cmbtaxtype, "Id", "TaxType", "TaxType", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)cmbtaxtype).Text = string.Empty;
				((UltraGridBase)cmbtaxtype).DataSource = null;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			if (updateDetailIndex > -1 && Conversion.ToInt(grd.CurrentRow.Cells["TaxId"].Value) > 0)
			{
				cmbtaxtype.Value = Conversion.ToInt(grd.CurrentRow.Cells["TaxId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		bindRateUomAndItemPackUom();
		TotalCommissionAmount();
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUOM).Text;
			string Rateuom = ((Control)(object)combrateuom).Text;
			((Control)(object)CmbPackUOM).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUOM, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUOM).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUOM).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, combrateuom, "Id", "UOMCode", "RateUOM", ZeroIndex: false);
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUOM).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUOM).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Rateuom))
				{
					((Control)(object)combrateuom).Text = Rateuom;
				}
				else
				{
					((Control)(object)combrateuom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUOM).Text = string.Empty;
				((Control)(object)combrateuom).Text = string.Empty;
				((UltraGridBase)CmbPackUOM).DataSource = null;
				((UltraGridBase)combrateuom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combojoblotfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<JobLot> lst = jobLot.GetList(new JobLot
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count > 0)
			{
				DataTable dtJob = new DataTable();
				dtJob.Columns.Add("Id");
				dtJob.Columns.Add("JobLotDescription");
				dtJob.Rows.Clear();
				for (int i = 0; i < lst.Count; i++)
				{
					dtJob.Rows.Add(lst[i].Id, lst[i].JobLotDescription);
				}
				if (dtJob.Rows.Count > 0)
				{
					DDL.BindDDL(dtJob, combjob, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void cmbcommuom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void GenerateReportVoucher103()
	{
		try
		{
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1604);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReportVoucher103();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet(int VoucherId)
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(VoucherId, 1604);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton3_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				LoadPO = new LoadPurchaseOrderWithQty(UserAccount);
				SupplierId = Convert.ToInt32(cmbsuppliername.Value);
				LoadPO.ShowDialog();
				LoadPO.BringToFront();
				LoadDataDetailGridAgainstGP();
				return;
			}
			if (Conversion.ToInt(grd.GetRow(0).Cells["PurchaseOrderId"].Value) > 0)
			{
				LoadPO = new LoadPurchaseOrderWithQty(UserAccount);
				LoadPO.CmbSupplier.Value = cmbsuppliername.Value;
				((Control)(object)LoadPO.CmbSupplier).Enabled = false;
				LoadPO.ShowDialog();
				LoadPO.BringToFront();
				LoadDataDetailGridAgainstGP();
				return;
			}
			throw new Exception("You Can't Load Purchase Order Because Direct Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadDataDetailGridAgainstGP()
	{
		try
		{
			DataTable invGrn = LoadPO.dtPurchaseOrder;
			if (invGrn.Rows.Count <= 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0 && Conversion.ToInt(grd.GetRow(0).Cells["PurchaseOrderId"].Value) != Conversion.ToInt(invGrn.Rows[0]["Id"]))
			{
				throw new Exception("Already Loaded Row's Have Different Order. So you Can't Load Rows Of Different Order!");
			}
			((Control)(object)cmbsuppliername).Enabled = false;
			txtRemarks.Text = invGrn.Rows[0]["RemarksHeader"].ToString();
			combpttrm.Value = invGrn.Rows[0]["PaymentTermsId"];
			txtDueDays.Text = invGrn.Rows[0]["OrderDueDays"].ToString();
			txtDeliveryDays.Text = invGrn.Rows[0]["DeliveryDays"].ToString();
			((Control)(object)combdeliverytrm).Text = invGrn.Rows[0]["DeliveryTerm"].ToString();
			cmbsuppliername.Value = invGrn.Rows[0]["OrderSupplierId"];
			for (int j = 0; j < invGrn.Rows.Count; j++)
			{
				bool flag = false;
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int k = 0; k < rows.Length; k++)
					{
						if (Conversion.ToInt(rows[k].Cells["PurchaseOrderDetailId"].Value) == Conversion.ToInt(invGrn.Rows[j]["PoDId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtGrid.Rows.Add(0, invGrn.Rows[j]["Id"], invGrn.Rows[j]["PoDId"], invGrn.Rows[j]["DocNo"], 0, invGrn.Rows[j]["JobLotId"], invGrn.Rows[j]["OrderItemId"], invGrn.Rows[j]["ItemName"], invGrn.Rows[j]["OrderItemUOMId"], invGrn.Rows[j]["UOMCodeItm"], invGrn.Rows[j]["BalQty"], invGrn.Rows[j]["BalQty"], invGrn.Rows[j]["BalWeight"], 0, invGrn.Rows[j]["BalWeight"], invGrn.Rows[j]["OrderItemRate"], invGrn.Rows[j]["OrderItemRateUOMId"], invGrn.Rows[j]["UOMCodeRate"], Conversion.ToDouble(invGrn.Rows[j]["EquivalentRate"]), invGrn.Rows[j]["BalAmount"], 0, 0, 0, invGrn.Rows[j]["TaxPercent"], invGrn.Rows[j]["TaxAmount"], invGrn.Rows[j]["TotalAmount"], 0, 0, Conversion.ToDateTime((object)"").ToShortDateString(), "", 0, 0, 0, 0);
				}
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			IsDetailHasPurchaseOrderId = true;
			dtInvExp.Clear();
			string Invexp = "";
			for (int i = 0; i < LoadPO.dtPurchaseOrder.Rows.Count; i++)
			{
				Invexp = Invexp + "," + int.Parse(LoadPO.dtPurchaseOrder.Rows[i][0].ToString());
			}
			LoadExpData(Invexp);
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			TotalCommissionAmount();
			ExpProportion();
			CommissionProportion();
			BillProportion();
			BillAmount();
			grdSettings();
			Invexp = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadExpData(string ID)
	{
		DataTable item = InvPurchaseInvoice.GetItemQtyAndItemRateFromPO(ID);
		if (item.Rows.Count > 0)
		{
			dtInvExp.Rows.Add(0, item.Rows[0]["ItemQty"], item.Rows[0]["BagPrice"], Conversion.ToDouble(item.Rows[0]["ItemQty"]) * Conversion.ToDouble(item.Rows[0]["BagPrice"]), 0);
		}
	}

	private void GetAll(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 1604;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (rddoc.Checked)
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
			else if (rdentry.Checked)
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
			else if (rdmodify.Checked)
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
			else if (rdapproved.Checked)
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			DataTable dthistory = InvPurchaseInvoice.FormHistory(obj);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierName");
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocumentTypeId"], dthistory.Rows[i]["RecordNo"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["SupplierName"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["BillAmount"], dthistory.Rows[i]["CurrencyCode"], dthistory.Rows[i]["ExchangeRate"], dthistory.Rows[i]["FcyAmount"], Conversion.ToString(dthistory.Rows[i]["ApprovedStatus"]), dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUser"], dthistory.Rows[i]["PostDate"], dthistory.Rows[i]["RemarksHeader"], dthistory.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["FcyCode"].Visible = MultiCurrencyFeatureVisibilty;
			grdHistory.RootTable.Columns["ExchangeRate"].Visible = MultiCurrencyFeatureVisibilty;
			grdHistory.RootTable.Columns["FcyAmount"].Visible = MultiCurrencyFeatureVisibilty;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("PartySlip");
			grdHistory.RootTable.Columns["PartySlip"].Caption = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PartySlip"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PartySlip"].ButtonText = "PartySlip";
			grdHistory.RootTable.Columns["PartySlip"].Width = 70;
			grdHistory.RootTable.Columns["PartySlip"].Position = 0;
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "ItemSlip";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "ItemSlip";
			grdHistory.RootTable.Columns["View"].Width = 60;
			grdHistory.RootTable.Columns["View"].Position = 1;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 65;
			grdHistory.RootTable.Columns["Voucher"].Position = 2;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns["Edit"].Position = 3;
			grdHistory.FrozenColumns = 4;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.####";
			grdHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		try
		{
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(grdHistory.CurrentRow.Cells["IsApproved"].Value) == "Approved")
			{
				throw new Exception("you Cannot Update Approved Record");
			}
			ReadById(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
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
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1604);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				if (Conversion.ToString(r.Cells["IsApproved"].Value) == "Approved")
				{
					throw new Exception("you Cannot Update Approved Record");
				}
				ReadById(Id);
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Convert.ToInt32(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 1604);
			}
			if (e.Column.Key == "View")
			{
				GenerateReportItemSlip(Id);
			}
			if (e.Column.Key == "PartySlip")
			{
				int SupCustId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				DateTime Date = Conversion.ToDateTime(r.Cells["DocDate"].Value);
				CommonServices.PurchaseInvoiceDirectSlipEngr_1604A(Id, SupCustId, 0, Date, Date);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderNo", typeof(string));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("ItemCode", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("AdLsWeight", typeof(double));
				dt.Columns.Add("NetBillWeight", typeof(double));
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("RateUOM", typeof(string));
				dt.Columns.Add("RateCut", typeof(double));
				dt.Columns.Add("RateCutAmount", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("Tax%", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("Expense", typeof(double));
				dt.Columns.Add("Commission", typeof(double));
				dt.Columns.Add("Freights", typeof(double));
				dt.Columns.Add("Journal", typeof(double));
				dt.Clear();
				for (int i = 0; i < invPI.invPurchaseInvoiceDetailList.Count; i++)
				{
					dt.Rows.Add(invPI.invPurchaseInvoiceDetailList[i].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[i].WareHouseName, invPI.invPurchaseInvoiceDetailList[i].JobLotDescription, invPI.invPurchaseInvoiceDetailList[i].ItemCode, invPI.invPurchaseInvoiceDetailList[i].ItemName, invPI.invPurchaseInvoiceDetailList[i].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[i].ItemQty, invPI.invPurchaseInvoiceDetailList[i].GrossWeight, invPI.invPurchaseInvoiceDetailList[i].AdLsWeight, invPI.invPurchaseInvoiceDetailList[i].NetBillWeight, invPI.invPurchaseInvoiceDetailList[i].ItemRate, invPI.invPurchaseInvoiceDetailList[i].RateUom, invPI.invPurchaseInvoiceDetailList[i].RateCut, invPI.invPurchaseInvoiceDetailList[i].RateCutAmount, invPI.invPurchaseInvoiceDetailList[i].ItemAmount, invPI.invPurchaseInvoiceDetailList[i].TaxPercent, invPI.invPurchaseInvoiceDetailList[i].TaxAmount, invPI.invPurchaseInvoiceDetailList[i].BillAmount, invPI.invPurchaseInvoiceDetailList[i].GpNo, invPI.invPurchaseInvoiceDetailList[i].GpDate, invPI.invPurchaseInvoiceDetailList[i].VehicleNo, invPI.invPurchaseInvoiceDetailList[i].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[i].CommissionAmount, invPI.invPurchaseInvoiceDetailList[i].FreightAmount, invPI.invPurchaseInvoiceDetailList[i].JournalAmount);
					grdHistoryDetail.DataSource = dt;
					grdHistoryDetail.RetrieveStructure();
					grdHistoryDetail.RootTable.Columns["WareHouseName"].Caption = "WareHouse";
					grdHistoryDetail.RootTable.Columns["Rate"].Caption = "ItemRate";
					grdHistoryDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["TaxAmount"].FormatString = "#,##0.###";
					grdHistoryDetail.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.###";
					grdHistoryDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
					grdHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
					grdHistoryDetail.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["AdLsWeight"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["AdLsWeight"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["AdLsWeight"].FormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["AdLsWeight"].TotalFormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.####";
					grdHistoryDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
					grdHistoryDetail.RootTable.Columns["RateCut"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["RateCut"].FormatString = clsGlobalVariables.DecimalRateFormate;
					grdHistoryDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Freights"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["Freights"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Freights"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Freights"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Commission"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Commission"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["Commission"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Commission"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
					grdHistoryDetail.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
					grdHistoryDetail.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
					grdHistoryDetail.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
					CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
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
			if (TaxPercntEditableOrNot)
			{
				txtTaxPercent.Enabled = true;
				txtTaxPercent.ReadOnly = false;
			}
			else
			{
				txtTaxPercent.Enabled = false;
				txtTaxPercent.ReadOnly = true;
			}
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			((Control)(object)cmbsuppliername).Enabled = true;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			IsDetailHasPurchaseOrderId = true;
			dtGrid.Rows.Clear();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtSchedule.Rows.Clear();
			dtFreight.Rows.Clear();
			DocDate.Focus();
			((Control)(object)cmbtaxtype).Text = string.Empty;
			((Control)(object)CmbTaxAccount).Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			((Control)(object)cmbcommagent).Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)cmbcommuom).Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtDeliveryDays.Text = string.Empty;
			txtbillno.Text = string.Empty;
			((Control)(object)combdeliverytrm).Text = string.Empty;
			txtRemarks.Text = string.Empty;
			DocDate.Value = DateTime.Now;
			grd.DataSource = dtGrid;
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			AddRowInScheduleGrid();
			grdSchedule.RetrieveStructure();
			ScheduleGridSetting();
			AddRowInFreightGrid();
			grdFreight.RetrieveStructure();
			grdFreightSettings();
			BranchFill();
			ProjectFill();
			SupplierNameFilll();
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			cmbsuppliername.Focus();
			dtGPGRNS.Rows.Clear();
			((Control)(object)cmbsuppliername).Enabled = true;
			ResetDetail();
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
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)cmbtaxtype).Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtAdLsWeight.Text = string.Empty;
			((Control)(object)CmbPackUOM).Text = string.Empty;
			((Control)(object)combjob).Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			updateDetailIndex = -1;
			txtTotalAmount.Clear();
			txtqty.Clear();
			txtItemAmount.Clear();
			txtRate.Clear();
			txtItemAmount.Clear();
			txtTaxPercent.Clear();
			txtTaxAmount.Clear();
			txtGpNo.Clear();
			txtVehicleNo.Clear();
			GpDate.Value = DateTime.Now;
			CmbWareHouse.Focus();
			((Control)(object)CmbItem).Enabled = true;
			txtRate.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFrmRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GetallTaxType();
			BranchFill();
			ProjectFill();
			PaymentTerms();
			SupplierNameFilll();
			ItemDetailFill();
			combojoblotfill();
			WareHouseFill();
			PreviousPurchaseTaxAc();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void closeHistory_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Expected O, but got Unknown
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Expected O, but got Unknown
		//IL_0f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f84: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseInvoice purchaseInvoice = new InvPurchaseInvoice();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				purchaseInvoice.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if ((Conversion.ToDouble(r2.Cells["Credit"].Value) > 0.0 || Conversion.ToInt(r2.Cells["Debit"].Value) > 0) && Conversion.ToInt(r2.Cells["AccountId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Account Against JL First");
					return;
				}
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r6 in rows)
			{
				if (Conversion.ToDouble(r6.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r6.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToDouble(rows[i].Cells["TaxAmount"].Value) > 0.0 && (((Control)(object)CmbTaxAccount).Text.Trim() == string.Empty || ((Control)(object)CmbTaxAccount).Text.Trim() == "0"))
				{
					MessageBox.Show("Tax Account is Required");
					CmbTaxAccount.Focus();
					return;
				}
			}
			purchaseInvoice.BranchesId = Conversion.ToInt(cmbbranch.Value);
			purchaseInvoice.DocDate = DocDate.Value;
			purchaseInvoice.DocNo = Conversion.ToInt((object)txtdocno.Text);
			purchaseInvoice.DocumentTypeId = 1604;
			purchaseInvoice.ManualBillNo = txtbillno.Text.Trim();
			purchaseInvoice.ProjectsId = Conversion.ToInt(cmbproject.Value);
			purchaseInvoice.TaxAccountId = Conversion.ToInt(CmbTaxAccount.Value);
			purchaseInvoice.RemarksHeader = txtRemarks.Text.Trim();
			purchaseInvoice.SupplierCustomerId = Conversion.ToInt(cmbsuppliername.Value);
			purchaseInvoice.UomScheduleIdCmRate = ((Control)(object)cmbcommuom).Text.Trim();
			purchaseInvoice.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			purchaseInvoice.SupplierInvoiceDate = DateTime.Now;
			purchaseInvoice.DeliveryTerm = ((Control)(object)combdeliverytrm).Text.Trim();
			purchaseInvoice.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			purchaseInvoice.DeliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			purchaseInvoice.DeliveryStartDate = Conversion.ToDateTime((object)datDeliveryStartDate.Value);
			purchaseInvoice.OrganizationId = UserAccount.OrganizationId;
			purchaseInvoice.CompanyId = UserAccount.CompanyId;
			purchaseInvoice.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			purchaseInvoice.EntryUser = UserAccount.ID;
			purchaseInvoice.EntryDate = DateTime.Now;
			purchaseInvoice.ModifyDate = DateTime.Now;
			purchaseInvoice.ModifyUser = UserAccount.ID;
			purchaseInvoice.DueDate = Conversion.ToDateTime((object)DueDate.Value);
			purchaseInvoice.DueDays = Conversion.ToInt((object)txtDueDays.Text);
			purchaseInvoice.ScreenName = Conversion.ToString((object)base.Name);
			purchaseInvoice.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			purchaseInvoice.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			purchaseInvoice.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			purchaseInvoice.invPurchaseInvoiceDetailList = new List<InvPurchaseInvoiceDetail>();
			if (grd.RowCount > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow r5 in rows)
				{
					InvPurchaseInvoiceDetail Pd = new InvPurchaseInvoiceDetail();
					Pd.Id = Conversion.ToInt(r5.Cells["Id"].Value);
					Pd.PurchaseOrderId = Conversion.ToInt(r5.Cells["PurchaseOrderId"].Value);
					Pd.PurchaseOrderDetailId = Conversion.ToInt(r5.Cells["PurchaseOrderDetailId"].Value);
					if (Conversion.ToSingle(r5.Cells["ItemAmount"].Value) != 0f)
					{
						Pd.ItemAmount = Conversion.ToDouble(r5.Cells["ItemAmount"].Value);
						Pd.ItemCgsRate = 0.0;
						if (Conversion.ToInt(r5.Cells["ItemId"].Value) != 0)
						{
							Pd.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
							if (Conversion.ToSingle(r5.Cells["ItemQty"].Value) != 0f)
							{
								Pd.ItemQty = Conversion.ToDouble(r5.Cells["ItemQty"].Value);
								Pd.GrossWeight = Conversion.ToDouble(r5.Cells["GrossWeight"].Value);
								Pd.AdLsWeight = Conversion.ToDouble(r5.Cells["AdLsWeight"].Value);
								Pd.NetBillWeight = Conversion.ToDouble(r5.Cells["NetBillWeight"].Value);
								Pd.NetStockWeight = Conversion.ToDouble(r5.Cells["NetBillWeight"].Value);
								if (Conversion.ToSingle(r5.Cells["ItemRate"].Value) != 0f)
								{
									Pd.ItemRate = Conversion.ToDouble(r5.Cells["ItemRate"].Value);
									if (Conversion.ToInt(r5.Cells["ItemUomId"].Value) != 0)
									{
										Pd.ItemUOMId = Conversion.ToInt(r5.Cells["ItemUomId"].Value);
										Pd.JobLotId = Conversion.ToInt(r5.Cells["JobLotId"].Value);
										Pd.UomScheduleIdRate = Conversion.ToInt(r5.Cells["OrderItemRateUOMId"].Value);
										if (Pd.UomScheduleIdRate == 0)
										{
											MessageBox.Show("Rate UOM Required");
											return;
										}
										if (Conversion.ToInt(r5.Cells["WarehouseId"].Value) != 0 && Conversion.ToInt(r5.Cells["WarehouseId"].Value) != 0)
										{
											Pd.WarehouseId = Conversion.ToInt(r5.Cells["WarehouseId"].Value);
											Pd.RateCut = Conversion.ToDouble(r5.Cells["RateCut"].Value);
											Pd.RateCutAmount = Conversion.ToDouble(r5.Cells["RateCutAmount"].Value);
											Pd.TaxPercent = Conversion.ToDouble(r5.Cells["Tax%"].Value);
											Pd.TaxAmount = Conversion.ToDouble(r5.Cells["TaxAmount"].Value);
											if (Pd.TaxAmount > 0.0)
											{
												Pd.TaxNameId = Conversion.ToInt(r5.Cells["TaxId"].Value);
												Pd.IsTaxable = "True";
											}
											Pd.GpNo = Conversion.ToInt(r5.Cells["GpNo"].Value);
											Pd.GpDate = Conversion.ToDateTime(r5.Cells["GpDate"].Value);
											Pd.VehicleNo = Conversion.ToString(r5.Cells["VehicleNo"].Value);
											if (Conversion.ToSingle(r5.Cells["BillAmount"].Value) != 0f)
											{
												Pd.BillAmount = Conversion.ToDouble(r5.Cells["BillAmount"].Value);
												DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r5.Cells["ItemId"].Value));
												if (dt.Rows.Count > 0)
												{
													DataRow[] dr = dt.Select("Equivalent='" + Conversion.ToInt(r5.Cells["EquivalentPoRate"].Value) + "'");
													if (dr.Length == 0)
													{
														MessageBox.Show("the RateUom you Enter In Grid is not define Against relevent Item please check");
														return;
													}
													Pd.UomScheduleIdRate = Conversion.ToInt(dr[0].ItemArray[0]);
												}
												Pd.FreightAmount = Conversion.ToDouble(r5.Cells["Freight"].Value);
												Pd.FcyAmount = Conversion.ToDecimal(r5.Cells["FcyAmount"].Value);
												Pd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
												Pd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
												Pd.JournalAmount = Conversion.ToDouble(r5.Cells["Journal"].Value);
												Pd.ExpenseAmount = Conversion.ToDouble(r5.Cells["Expense"].Value);
												Pd.CommissionAmount = Conversion.ToDouble(r5.Cells["Commission"].Value);
												purchaseInvoice.invPurchaseInvoiceDetailList.Add(Pd);
												continue;
											}
											MessageBox.Show("Bill Amount Required");
											return;
										}
										MessageBox.Show("WareHouse field Required");
										return;
									}
									MessageBox.Show("ItemUom Required");
									return;
								}
								MessageBox.Show("Item Rate Required");
								return;
							}
							MessageBox.Show("ItemQty Required");
							return;
						}
						MessageBox.Show("Item Required");
						return;
					}
					MessageBox.Show("Item Amount Required");
					return;
				}
				purchaseInvoice.InvPurchaseInvoiceExpList = new List<InvPurchaseInvoiceExpense>();
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r4 in rows)
				{
					if (Conversion.ToInt((object)r4.Cells["ItemId"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceExpense PE = new InvPurchaseInvoiceExpense();
						PE.InvRevExpItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToInt(r4.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r4.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r4.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r4.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceExpList.Add(PE);
					}
				}
				purchaseInvoice.InvPurchaseInvoicejournalList = new List<InvPurchaseInvoiceJournal>();
				rows = grdGLedger.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (Conversion.ToInt(r3.Cells["AccountId"].Value) != 0 || Conversion.ToInt(r3.Cells["GlAccountId"].Value) != 0 || Conversion.ToInt(r3.Cells["Credit"].Value) != 0 || Conversion.ToInt(r3.Cells["Debit"].Value) != 0)
					{
						InvPurchaseInvoiceJournal PJ = new InvPurchaseInvoiceJournal();
						if (Conversion.ToInt(r3.Cells["AccountId"].Value) == 0 && Conversion.ToInt(r3.Cells["GlAccountId"].Value) == 0)
						{
							throw new Exception("Account Required in Party AddLess Grid And row No: " + (r3.RowIndex + 1));
						}
						PJ.ChartofAccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						PJ.SupplierCustomerId = Conversion.ToInt(r3.Cells["AccountId"].Value);
						PJ.JvRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						PJ.JvPrcnt = Conversion.ToDouble(r3.Cells["Percentage"].Value);
						PJ.JvQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
						PJ.JvRate = Conversion.ToDouble(r3.Cells["Rate"].Value);
						PJ.JvDebit = Conversion.ToDouble(r3.Cells["Debit"].Value);
						PJ.JvCredit = Conversion.ToDouble(r3.Cells["Credit"].Value);
						if (PJ.JvDebit == 0.0 && PJ.JvCredit == 0.0)
						{
							((Control)(object)grdGLedger).Focus();
							throw new Exception("Credit or Debit is Required in Party AddLess Grid And row No: " + (r3.RowIndex + 1));
						}
						if ((PJ.JvDebit > 0.0 || PJ.JvCredit > 0.0) && PJ.JvRemarks == string.Empty)
						{
							((Control)(object)grdGLedger).Focus();
							throw new Exception("Remarks Field is Required in Party AddLess Grid And row No: " + (r3.RowIndex + 1));
						}
						purchaseInvoice.InvPurchaseInvoicejournalList.Add(PJ);
					}
				}
				purchaseInvoice.InvPurchaseInvoiceFreightList = new List<InvPurchaseInvoiceFreight>();
				rows = grdFreight.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["Transporter"].Value.ToString()) != 0)
					{
						InvPurchaseInvoiceFreight PF = new InvPurchaseInvoiceFreight();
						if (SubsidiaryAccountAllownOnVouchers)
						{
							PF.TansporterId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
							PF.SupplierCustomerId = Conversion.ToInt(r.Cells["Transporter"].Value);
						}
						else
						{
							PF.TansporterId = Conversion.ToInt(r.Cells["GlAccountId"].Value);
							PF.SupplierCustomerId = Conversion.ToInt((object)0);
						}
						PF.FreightAmount = Conversion.ToInt(r.Cells["Freight"].Value);
						PF.Debit = Conversion.ToInt(r.Cells["Debit"].Value);
						PF.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						purchaseInvoice.InvPurchaseInvoiceFreightList.Add(PF);
					}
				}
				purchaseInvoice.AttachmentsList = new List<DMSAttachments>();
				purchaseInvoice.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseInvoice>(purchaseInvoice, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				purchaseInvoice.AttachmentsValues = AttachmentsValues;
				purchaseInvoice.CustomAttachmentsValues = CustomAttachmentsValues;
				int sucess = InvPurchaseInvoice.Save(purchaseInvoice);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully [" + purchaseInvoice.DocNo + "] ");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1604, base.Name);
				}
				else
				{
					MessageBox.Show("Record Saved Successfully [" + purchaseInvoice.DocNo + "] ");
				}
				int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
				DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
				Reset();
				if (ChkBok.Checked)
				{
					CommonServices.PurchaseInvoiceDirectSlipEngr_1604A(sucess, SupCustId, 0, Date, Date);
				}
				if (ChkVoucherPreview.Checked)
				{
					VoucherHeadIdGet(sucess);
					CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 1604);
					VoucherHeadId = 0;
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not found");
			}
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

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			InvPurchaseInvoice invPI = InvPurchaseInvoice.GetByID(RecId);
			if (invPI != null)
			{
				cmbbranch.Value = invPI.BranchesId;
				cmbproject.Value = invPI.ProjectsId;
				txtdocno.Text = invPI.DocNo.ToString();
				DocDate.Value = invPI.DocDate;
				DueDate.Value = invPI.DueDate;
				datDeliveryStartDate.Value = Conversion.ToDateTime((object)invPI.DeliveryStartDate);
				tabControl1.SelectedIndex = 0;
				cmbsuppliername.Value = invPI.SupplierCustomerId;
				txtbillno.Text = invPI.ManualBillNo;
				cmbcommagent.Value = invPI.CommissionAgentId;
				((Control)(object)cmbcommtype).Text = Conversion.ToString((object)invPI.CommissionType);
				txtcommrate.Text = invPI.CommRate.ToString();
				((Control)(object)cmbcommuom).Text = Conversion.ToString((object)invPI.UomScheduleIdCmRate);
				txtcommamount.Text = invPI.CommAmount.ToString();
				txtcommremarks.Text = Conversion.ToString((object)invPI.CommissionRemarks);
				txtRemarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
				txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtDeliveryDays.Text = invPI.DeliveryDays.ToString();
				txtDueDays.Text = invPI.DueDays.ToString();
				((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)invPI.DeliveryTerm);
				combpttrm.Value = invPI.PaymentTermsId;
				AttachmentsValues = invPI.AttachmentsValues;
				CustomAttachmentsValues = invPI.CustomAttachmentsValues;
				CmbTaxAccount.Value = invPI.TaxAccountId;
				if (invPI.invPurchaseInvoiceDetailList[0].PurchaseOrderId > 0)
				{
					((Control)(object)cmbsuppliername).Enabled = false;
					IsDetailHasPurchaseOrderId = true;
				}
				else
				{
					IsDetailHasPurchaseOrderId = false;
				}
				VoucherHeadIdGet(RecId);
				dtGrid.Rows.Clear();
				for (int m = 0; m < invPI.invPurchaseInvoiceDetailList.Count; m++)
				{
					dtGrid.Rows.Add(invPI.invPurchaseInvoiceDetailList[m].Id, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderId, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrderDetailId, invPI.invPurchaseInvoiceDetailList[m].PurchaseOrder, invPI.invPurchaseInvoiceDetailList[m].WarehouseId, invPI.invPurchaseInvoiceDetailList[m].JobLotId, invPI.invPurchaseInvoiceDetailList[m].ItemId, invPI.invPurchaseInvoiceDetailList[m].ItemName, invPI.invPurchaseInvoiceDetailList[m].ItemUOMId, invPI.invPurchaseInvoiceDetailList[m].UOMCodeItem, invPI.invPurchaseInvoiceDetailList[m].ItemQty, invPI.invPurchaseInvoiceDetailList[m].ItemQty, invPI.invPurchaseInvoiceDetailList[m].GrossWeight, invPI.invPurchaseInvoiceDetailList[m].AdLsWeight, invPI.invPurchaseInvoiceDetailList[m].NetBillWeight, invPI.invPurchaseInvoiceDetailList[m].ItemRate, invPI.invPurchaseInvoiceDetailList[m].UomScheduleIdRate, invPI.invPurchaseInvoiceDetailList[m].RateUom, invPI.invPurchaseInvoiceDetailList[m].EquivalentPoRate, invPI.invPurchaseInvoiceDetailList[m].ItemAmount, invPI.invPurchaseInvoiceDetailList[m].RateCut, invPI.invPurchaseInvoiceDetailList[m].RateCutAmount, invPI.invPurchaseInvoiceDetailList[m].TaxNameId, invPI.invPurchaseInvoiceDetailList[m].TaxPercent, invPI.invPurchaseInvoiceDetailList[m].TaxAmount, invPI.invPurchaseInvoiceDetailList[m].BillAmount, invPI.invPurchaseInvoiceDetailList[m].FcyAmount, invPI.invPurchaseInvoiceDetailList[m].GpNo, invPI.invPurchaseInvoiceDetailList[m].GpDate, invPI.invPurchaseInvoiceDetailList[m].VehicleNo, invPI.invPurchaseInvoiceDetailList[m].FreightAmount, invPI.invPurchaseInvoiceDetailList[m].JournalAmount, invPI.invPurchaseInvoiceDetailList[m].ExpenseAmount, invPI.invPurchaseInvoiceDetailList[m].CommissionAmount);
				}
				dtInvExp.Rows.Clear();
				for (int l = 0; l < invPI.InvPurchaseInvoiceExpList.Count; l++)
				{
					dtInvExp.Rows.Add(invPI.InvPurchaseInvoiceExpList[l].InvRevExpItemId, invPI.InvPurchaseInvoiceExpList[l].Qty, invPI.InvPurchaseInvoiceExpList[l].Rate, invPI.InvPurchaseInvoiceExpList[l].Amount, invPI.InvPurchaseInvoiceExpList[l].Remarks);
				}
				dtGrdGL.Rows.Clear();
				for (int k = 0; k < invPI.InvPurchaseInvoicejournalList.Count; k++)
				{
					dtGrdGL.Rows.Add(invPI.InvPurchaseInvoicejournalList[k].SupplierCustomerId, invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId, (invPI.InvPurchaseInvoicejournalList[k].SupplierCustomerId > 0) ? invPI.InvPurchaseInvoicejournalList[k].SupplierName : ((invPI.InvPurchaseInvoicejournalList[k].ChartofAccountId > 0) ? invPI.InvPurchaseInvoicejournalList[k].AccountTitle : ""), invPI.InvPurchaseInvoicejournalList[k].JvRemarks, invPI.InvPurchaseInvoicejournalList[k].JvPrcnt, invPI.InvPurchaseInvoicejournalList[k].JvQty, invPI.InvPurchaseInvoicejournalList[k].JvRate, invPI.InvPurchaseInvoicejournalList[k].JvDebit, invPI.InvPurchaseInvoicejournalList[k].JvCredit);
				}
				dtFreight.Rows.Clear();
				for (int j = 0; j < invPI.InvPurchaseInvoiceFreightList.Count; j++)
				{
					dtFreight.Rows.Add(SubsidiaryAccountAllownOnVouchers ? invPI.InvPurchaseInvoiceFreightList[j].SupplierCustomerId : invPI.InvPurchaseInvoiceFreightList[j].TansporterId, invPI.InvPurchaseInvoiceFreightList[j].TansporterId, invPI.InvPurchaseInvoiceFreightList[j].FreightAmount, invPI.InvPurchaseInvoiceFreightList[j].Debit, invPI.InvPurchaseInvoiceFreightList[j].Remarks);
				}
				dtSchedule.Rows.Clear();
				for (int i = 0; i < invPI.PaymentDuesSchedules.Count; i++)
				{
					dtSchedule.Rows.Add(invPI.PaymentDuesSchedules[i].DueDays, invPI.PaymentDuesSchedules[i].DueDate.ToString("dd-MMM-yyyy"), invPI.PaymentDuesSchedules[i].DuePercentage, invPI.PaymentDuesSchedules[i].Amount);
				}
				if (dtSchedule.Rows.Count == 0)
				{
					AddRowInScheduleGrid();
				}
				grdSchedule.DataSource = dtSchedule;
				grdSchedule.RetrieveStructure();
				ScheduleGridSetting();
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.DataSource = dtInvExp;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
				grdGLedger.DataSource = dtGrdGL;
				grdGLedger.RetrieveStructure();
				gridGLSettings();
				if (dtFreight.Rows.Count == 0)
				{
					AddRowInFreightGrid();
				}
				grdFreight.DataSource = dtFreight;
				grdFreight.RetrieveStructure();
				grdFreightSettings();
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				FreightProportion();
				ExpProportion();
				BillAmount();
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				if (invPI.CurrencyId == 0)
				{
					DefaultConfigurations();
				}
				else
				{
					cmbCurrency.Value = invPI.CurrencyId;
					txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
				}
				txtExchangeRate_TextChanged(null, null);
				tabControl1.SelectedIndex = 0;
				cmbsuppliername.Focus();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			if (InvPurchaseInvoice.GetIdbyDocNoPurchaseInvoice(new InvPurchaseInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtdocno.Text.Trim()),
				DocumentTypeId = 1604
			}) == 0)
			{
				throw new Exception("Record not exist against this number");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvfrmPurchaseInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnFrmRefresh_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
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
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					CmbWareHouse.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
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

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnSlipDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateReportItemSlip(RecId);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReportItemSlip(int PrintId)
	{
		try
		{
			CommonServices.PurchaseInvoiceDirectSlipEngr_1604(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPartySlip_Click(object sender, EventArgs e)
	{
		try
		{
			int SupCustId = Conversion.ToInt(cmbsuppliername.Value);
			DateTime Date = Conversion.ToDateTime((object)DocDate.Value);
			if (RecId > 0)
			{
				CommonServices.PurchaseInvoiceDirectSlipEngr_1604A(RecId, SupCustId, 0, Date, Date);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDeliveryDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdLsWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetBillWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGpNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateCut_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDueDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0 && btnSave.Enabled && btnSave.Visible)
			{
				string ItemIds = "";
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
				}
				DataTable dt = ChangedTaxPercentFromGridByDatechange(ItemIds);
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double TaxPercent = 0.0;
					double ItemAmount = 0.0;
					double TaxAmount = 0.0;
					bool flag = false;
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
						{
							flag = true;
							item.Cells["TaxId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
							TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
							item.Cells["Tax%"].Value = TaxPercent;
							ItemAmount = Conversion.ToDouble(item.Cells["ItemAmount"].Value);
							Conversion.ToDouble(item.Cells["BillAmount"].Value);
							if (ItemAmount > 0.0 && TaxPercent > 0.0)
							{
								TaxAmount = ItemAmount * TaxPercent / 100.0;
								item.Cells["TaxAmount"].Value = TaxAmount;
							}
						}
					}
					if (!flag)
					{
						item.Cells["TaxId"].Value = 0;
						item.Cells["Tax%"].Value = 0;
						item.Cells["TaxAmount"].Value = 0;
					}
					item.EndEdit();
				}
			}
			CalculateDueDate();
			CalculateDeliveryDate();
			BillAmount();
			GetallTaxType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ChangedTaxPercentFromGridByDatechange(string ItemIds)
	{
		DataTable dt = new DataTable();
		try
		{
			dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void CalculateDueDate()
	{
		try
		{
			if (txtDueDays.Text.Trim() != string.Empty && txtDueDays.Text != null)
			{
				DueDate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtDueDays.Text.Trim()));
			}
			else
			{
				DueDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDeliveryDate()
	{
		try
		{
			if (datDeliveryStartDate.Value.Date < DocDate.Value.Date)
			{
				datDeliveryStartDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliveryDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDeliveryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void datDeliveryStartDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (datDeliveryStartDate.Value.Date < DocDate.Value.Date)
			{
				datDeliveryStartDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (DueDate.Value.Date < DocDate.Value.Date)
			{
				DueDate.Value = DocDate.Value;
			}
			int days = (int)(DueDate.Value.Date - DocDate.Value.Date).TotalDays;
			txtDueDays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtRate.Text = Conversion.ToDouble((object)txtRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRateCut_Leave(object sender, EventArgs e)
	{
		try
		{
			txtRateCut.Text = Conversion.ToDouble((object)txtRateCut.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtqty_Leave(object sender, EventArgs e)
	{
		try
		{
			txtqty.Text = Conversion.ToDouble((object)txtqty.Text).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combpttrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtDueDays.Enabled = false;
				DueDate.Enabled = false;
				txtDueDays.Text = string.Empty;
				DueDate.Value = DocDate.Value;
			}
			else
			{
				txtDueDays.Enabled = true;
				DueDate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbtaxtype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseInvoiceTradePro_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1604",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
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
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["ItemAmount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
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
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDouble((object)txtExchangeRate.Text) > 0.0)
			{
				double billAmount = Conversion.ToDouble((object)txtBillAmount.Text);
				double ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				txtFcyAmount.Text = (billAmount / ExchangeRate).ToString("#,##0.###");
			}
			else
			{
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			grdHistory.ClearStructure();
			grdHistoryDetail.ClearStructure();
			rddoc.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboFill();
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
			GetAll();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRateCutTotal_Leave(object sender, EventArgs e)
	{
		try
		{
			txtRateCutTotal.Text = Conversion.ToDouble((object)txtRateCutTotal.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtRateCutTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateCutTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
			CalculateTaxAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl?.Name != null && base.ActiveControl.Name == "grdHistory")
			{
				FromDateHistory.Focus();
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
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Expected O, but got Unknown
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Expected O, but got Unknown
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Expected O, but got Unknown
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Expected O, but got Unknown
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Expected O, but got Unknown
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Expected O, but got Unknown
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Expected O, but got Unknown
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_04c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Expected O, but got Unknown
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Expected O, but got Unknown
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Expected O, but got Unknown
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050f: Expected O, but got Unknown
		//IL_050f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Expected O, but got Unknown
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Expected O, but got Unknown
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Expected O, but got Unknown
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_056a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Expected O, but got Unknown
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Expected O, but got Unknown
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_05b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Expected O, but got Unknown
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Expected O, but got Unknown
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05da: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Expected O, but got Unknown
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0604: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Expected O, but got Unknown
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Expected O, but got Unknown
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Expected O, but got Unknown
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Expected O, but got Unknown
		//IL_0873: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Expected O, but got Unknown
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Expected O, but got Unknown
		//IL_08c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Expected O, but got Unknown
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e0: Expected O, but got Unknown
		//IL_090d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Expected O, but got Unknown
		//IL_097b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Expected O, but got Unknown
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f3: Expected O, but got Unknown
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4b: Expected O, but got Unknown
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a82: Expected O, but got Unknown
		//IL_0a83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8d: Expected O, but got Unknown
		//IL_0a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Expected O, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Expected O, but got Unknown
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Expected O, but got Unknown
		//IL_0b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Expected O, but got Unknown
		//IL_0c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c50: Expected O, but got Unknown
		//IL_0c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7c: Expected O, but got Unknown
		//IL_0d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0b: Expected O, but got Unknown
		//IL_139f: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Expected O, but got Unknown
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d7: Expected O, but got Unknown
		//IL_18b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18bf: Expected O, but got Unknown
		//IL_18cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18d6: Expected O, but got Unknown
		//IL_1b89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b93: Expected O, but got Unknown
		//IL_1ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1baa: Expected O, but got Unknown
		//IL_1e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3f: Expected O, but got Unknown
		//IL_1e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e56: Expected O, but got Unknown
		//IL_1e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e6d: Expected O, but got Unknown
		//IL_54b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_54ba: Expected O, but got Unknown
		//IL_a86c: Unknown result type (might be due to invalid IL or missing references)
		//IL_a876: Expected O, but got Unknown
		//IL_a89a: Unknown result type (might be due to invalid IL or missing references)
		//IL_a8a4: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFreight_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		GridEXLayout grdSchedule_DesignTimeLayout = new GridEXLayout();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
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
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label31 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdFreight = new GridEX();
		this.label45 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.label15 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.label46 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRateCutTotal = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtRateCut = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbtaxtype = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.GpDate = new System.Windows.Forms.DateTimePicker();
		this.label24 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.txtAdLsWeight = new System.Windows.Forms.TextBox();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.txtGpNo = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.combrateuom = new UltraCombo();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.combjob = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbPackUOM = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdSchedule = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.lblExchangeRate = new System.Windows.Forms.Label();
		this.datDeliveryStartDate = new System.Windows.Forms.DateTimePicker();
		this.lblFcyAmount = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.cmbCurrency = new UltraCombo();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.lblFcyCode = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DueDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.combpttrm = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbTaxAccount = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.combdeliverytrm = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbbranch = new UltraCombo();
		this.cmbsuppliername = new UltraCombo();
		this.cmbproject = new UltraCombo();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ChkVoucherPreview = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.cmbstockprty = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.cmbcommuom = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbcommtype = new UltraCombo();
		this.cmbcommagent = new UltraCombo();
		this.txtcommremarks = new System.Windows.Forms.RichTextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnFrmRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPartySlip = new System.Windows.Forms.ToolStripButton();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnLoadPo = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproved = new System.Windows.Forms.RadioButton();
		this.rdmodify = new System.Windows.Forms.RadioButton();
		this.rddoc = new System.Windows.Forms.RadioButton();
		this.rdentry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.button1 = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel18.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFreight).BeginInit();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.tabPage3.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel8.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1214, 707);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1214, 707);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 8;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1206, 681);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel18);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 31);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1200, 647);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 506);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1200, 141);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar1);
		this.panel18.Controls.Add(this.label31);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 482);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1200, 24);
		this.panel18.TabIndex = 8;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1153, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(6, 4);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(53, 18);
		this.label31.TabIndex = 24;
		this.label31.Text = "Detail";
		this.panel7.Controls.Add(this.tabControl2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 319);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1200, 163);
		this.panel7.TabIndex = 6;
		this.tabControl2.Controls.Add(this.tabPage1);
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1200, 163);
		this.tabControl2.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.label45);
		this.tabPage1.Location = new System.Drawing.Point(4, 22);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1192, 137);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Transsporter";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdFreight);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 22);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1186, 112);
		this.panel11.TabIndex = 2;
		((ScrollableBase)this.grdFreight).BackColor = System.Drawing.Color.White;
		this.grdFreight.ColumnAutoResize = true;
		grdFreight_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFreight.DesignTimeLayout = grdFreight_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFreight).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFreight).Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
		this.grdFreight.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFreight).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdFreight).Name = "grdFreight";
		((ScrollableBase)this.grdFreight).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFreight).Size = new System.Drawing.Size(1186, 112);
		((System.Windows.Forms.Control)(object)this.grdFreight).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdFreight).Tag = "GrdFreight";
		this.grdFreight.TotalRow = (InheritableBoolean)1;
		this.grdFreight.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFreight).VisualStyle = (VisualStyle)4;
		this.grdFreight.CellUpdated += new ColumnActionEventHandler(grdFreight_CellUpdated);
		this.grdFreight.ColumnButtonClick += new ColumnActionEventHandler(grdFreight_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFreight).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFreight_KeyDown);
		this.label45.BackColor = System.Drawing.Color.Teal;
		this.label45.Dock = System.Windows.Forms.DockStyle.Top;
		this.label45.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(3, 3);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(1186, 19);
		this.label45.TabIndex = 1;
		this.label45.Text = "Amount Credit to selected A/c and Debit to Product";
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.label15);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1192, 137);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Other Item Expense";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 22);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1186, 112);
		this.panel10.TabIndex = 1;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		this.grdInvExp.ColumnAutoResize = true;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1186, 112);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Dock = System.Windows.Forms.DockStyle.Top;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(1186, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Other Expense (Credit To Supplier & Debit To Product)";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add(this.label46);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1192, 137);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Supplier Add Less";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 19);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1192, 118);
		this.panel9.TabIndex = 2;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(1192, 118);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGLedger).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGLedger_KeyDown);
		this.label46.BackColor = System.Drawing.Color.Teal;
		this.label46.Dock = System.Windows.Forms.DockStyle.Top;
		this.label46.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(0, 0);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(1192, 19);
		this.label46.TabIndex = 1;
		this.label46.Text = "Supplier Add/Less (Amount Debit or credit to Selected A/c Against Supplier";
		this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 191);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1200, 128);
		this.panel8.TabIndex = 5;
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.txtRateCutTotal);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtRateCut);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtaxtype);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.txtNetBillWeight);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.GpDate);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.txtRate);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.txtAdLsWeight);
		this.groupBox2.Controls.Add(this.txtTotalAmount);
		this.groupBox2.Controls.Add(this.txtVehicleNo);
		this.groupBox2.Controls.Add(this.txtItemAmount);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtGpNo);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.groupBox2.Controls.Add(this.txtGrossWeight);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtTaxAmount);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtTaxPercent);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combjob);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUOM);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 10.25f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(10, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1159, 121);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Information";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(995, 88);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(23, 26);
		this.btnplus.TabIndex = 20;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRateCutTotal.BackColor = System.Drawing.Color.White;
		this.txtRateCutTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateCutTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateCutTotal.Location = new System.Drawing.Point(151, 90);
		this.txtRateCutTotal.Name = "txtRateCutTotal";
		this.txtRateCutTotal.Size = new System.Drawing.Size(98, 23);
		this.txtRateCutTotal.TabIndex = 11;
		this.txtRateCutTotal.Tag = "RateCutTotal";
		this.txtRateCutTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateCutTotal.TextChanged += new System.EventHandler(txtRateCutTotal_TextChanged);
		this.txtRateCutTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateCutTotal_KeyPress);
		this.txtRateCutTotal.Leave += new System.EventHandler(txtRateCutTotal_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(148, 70);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(82, 15);
		this.label42.TabIndex = 44;
		this.label42.Text = "R.Cut Amount";
		this.txtRateCut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateCut.Location = new System.Drawing.Point(85, 90);
		this.txtRateCut.Name = "txtRateCut";
		this.txtRateCut.Size = new System.Drawing.Size(65, 23);
		this.txtRateCut.TabIndex = 10;
		this.txtRateCut.Tag = "RateCut";
		this.txtRateCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateCut.TextChanged += new System.EventHandler(txtRateCut_TextChanged);
		this.txtRateCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateCut_KeyPress);
		this.txtRateCut.Leave += new System.EventHandler(txtRateCut_Leave);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(85, 70);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(51, 15);
		this.label39.TabIndex = 42;
		this.label39.Text = "Rate Cut";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(351, 70);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(53, 15);
		this.label35.TabIndex = 40;
		this.label35.Text = "Tax Type";
		this.cmbtaxtype.AllowNull = (DefaultableBoolean)1;
		this.cmbtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance137;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Location = new System.Drawing.Point(354, 88);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Name = "cmbtaxtype";
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Leave += new System.EventHandler(cmbtaxtype_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(967, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(87, 15);
		this.label27.TabIndex = 31;
		this.label27.Text = "Net Bill Weight";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(779, 70);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(47, 15);
		this.label16.TabIndex = 38;
		this.label16.Text = "GpDate";
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Enabled = false;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeight.Location = new System.Drawing.Point(967, 41);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.Size = new System.Drawing.Size(92, 23);
		this.txtNetBillWeight.TabIndex = 7;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetBillWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetBillWeight_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(995, 88);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(72, 26);
		this.btnUpdateDetail.TabIndex = 20;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(443, 70);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(38, 15);
		this.label43.TabIndex = 31;
		this.label43.Text = "Tax %";
		this.GpDate.CustomFormat = "dd-MMM-yyyy";
		this.GpDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.GpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GpDate.Location = new System.Drawing.Point(779, 90);
		this.GpDate.Name = "GpDate";
		this.GpDate.Size = new System.Drawing.Size(108, 23);
		this.GpDate.TabIndex = 18;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(888, 20);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 29;
		this.label24.Text = "Ad/Ls Wt";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(706, 70);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(38, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "GpNo";
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(1061, 41);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(81, 23);
		this.txtRate.TabIndex = 8;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtRate.Leave += new System.EventHandler(txtRate_Leave);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1068, 88);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(73, 26);
		this.btnCancelUpdateDetial.TabIndex = 21;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.txtAdLsWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdLsWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdLsWeight.Location = new System.Drawing.Point(888, 41);
		this.txtAdLsWeight.Name = "txtAdLsWeight";
		this.txtAdLsWeight.Size = new System.Drawing.Size(77, 23);
		this.txtAdLsWeight.TabIndex = 6;
		this.txtAdLsWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdLsWeight.TextChanged += new System.EventHandler(txtAdLsWeight_TextChanged);
		this.txtAdLsWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAdLsWeight_KeyPress);
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(590, 90);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.Size = new System.Drawing.Size(114, 23);
		this.txtTotalAmount.TabIndex = 16;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNo.Location = new System.Drawing.Point(888, 90);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(104, 23);
		this.txtVehicleNo.TabIndex = 19;
		this.txtItemAmount.BackColor = System.Drawing.Color.White;
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Enabled = false;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmount.Location = new System.Drawing.Point(250, 90);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(103, 23);
		this.txtItemAmount.TabIndex = 12;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(888, 70);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "Vehicle No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(505, 70);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 15);
		this.label48.TabIndex = 33;
		this.label48.Text = "Tax Amount";
		this.txtGpNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGpNo.Location = new System.Drawing.Point(706, 90);
		this.txtGpNo.Name = "txtGpNo";
		this.txtGpNo.Size = new System.Drawing.Size(71, 23);
		this.txtGpNo.TabIndex = 17;
		this.txtGpNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGpNo_KeyPress);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(784, 20);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(78, 15);
		this.label17.TabIndex = 27;
		this.label17.Text = "Gross Weight";
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(4, 88);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(79, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.combrateuom).Leave += new System.EventHandler(combrateuom_Leave);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(784, 41);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(102, 23);
		this.txtGrossWeight.TabIndex = 5;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(250, 70);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(505, 90);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(84, 23);
		this.txtTaxAmount.TabIndex = 15;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(590, 70);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(80, 15);
		this.label49.TabIndex = 35;
		this.label49.Text = "Total Amount";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(4, 39);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(4, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 0;
		this.label14.Text = "WareHouse";
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(443, 90);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(61, 23);
		this.txtTaxPercent.TabIndex = 14;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.txtTaxPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTaxPercent_KeyPress);
		this.combjob.AllowNull = (DefaultableBoolean)1;
		this.combjob.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjob.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjob.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjob).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combjob).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjob).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combjob).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjob).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjob).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjob).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjob).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combjob.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjob).Location = new System.Drawing.Point(169, 39);
		((System.Windows.Forms.Control)(object)this.combjob).Name = "combjob";
		((System.Windows.Forms.Control)(object)this.combjob).Size = new System.Drawing.Size(182, 26);
		((System.Windows.Forms.Control)(object)this.combjob).TabIndex = 1;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(4, 70);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(707, 20);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(173, 20);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(48, 15);
		this.label34.TabIndex = 23;
		this.label34.Text = "Job/Lot";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(354, 39);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(255, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(1061, 21);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.CmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Location = new System.Drawing.Point(611, 39);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Name = "CmbPackUOM";
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPackUOM).Leave += new System.EventHandler(CmbPackUOM_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(357, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(608, 20);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(707, 41);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(75, 23);
		this.txtqty.TabIndex = 4;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtqty.Leave += new System.EventHandler(txtqty_Leave);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdSchedule);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel13);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstockprty);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.txtcommamount);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtcommrate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommuom);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommagent);
		this.panel3.Controls.Add(this.txtcommremarks);
		this.panel3.Controls.Add(this.label20);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1200, 191);
		this.panel3.TabIndex = 4;
		((ScrollableBase)this.grdSchedule).BackColor = System.Drawing.Color.White;
		this.grdSchedule.ColumnAutoResize = true;
		grdSchedule_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSchedule.DesignTimeLayout = grdSchedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSchedule.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Location = new System.Drawing.Point(1299, 35);
		((System.Windows.Forms.Control)(object)this.grdSchedule).Name = "grdSchedule";
		((ScrollableBase)this.grdSchedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Size = new System.Drawing.Size(54, 135);
		((System.Windows.Forms.Control)(object)this.grdSchedule).TabIndex = 0;
		this.grdSchedule.TotalRow = (InheritableBoolean)1;
		this.grdSchedule.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdSchedule).Visible = false;
		((ScrollableBase)this.grdSchedule).VisualStyle = (VisualStyle)4;
		this.grdSchedule.CellUpdated += new ColumnActionEventHandler(grdSchedule_CellUpdated);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.lblExchangeRate);
		this.groupBox1.Controls.Add(this.datDeliveryStartDate);
		this.groupBox1.Controls.Add(this.lblFcyAmount);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.txtDeliveryDays);
		this.groupBox1.Controls.Add(this.lblFcyCode);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.DueDate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxAccount);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtDueDays);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.txtbillno);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtBillAmount);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsuppliername);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10.75f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(10, 28);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1159, 160);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 136);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(52, 15);
		this.label7.TabIndex = 59;
		this.label7.Text = "Remarks";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(944, 50);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(111, 23);
		this.txtExchangeRate.TabIndex = 16;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(96, 132);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(757, 23);
		this.txtRemarks.TabIndex = 14;
		this.lblExchangeRate.AutoSize = true;
		this.lblExchangeRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblExchangeRate.Location = new System.Drawing.Point(859, 54);
		this.lblExchangeRate.Name = "lblExchangeRate";
		this.lblExchangeRate.Size = new System.Drawing.Size(84, 15);
		this.lblExchangeRate.TabIndex = 58;
		this.lblExchangeRate.Text = "Exchange Rate";
		this.datDeliveryStartDate.CustomFormat = "dd-MMM-yyyy";
		this.datDeliveryStartDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDeliveryStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDeliveryStartDate.Location = new System.Drawing.Point(721, 50);
		this.datDeliveryStartDate.Name = "datDeliveryStartDate";
		this.datDeliveryStartDate.Size = new System.Drawing.Size(132, 23);
		this.datDeliveryStartDate.TabIndex = 11;
		this.datDeliveryStartDate.ValueChanged += new System.EventHandler(datDeliveryStartDate_ValueChanged);
		this.lblFcyAmount.AutoSize = true;
		this.lblFcyAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFcyAmount.Location = new System.Drawing.Point(859, 81);
		this.lblFcyAmount.Name = "lblFcyAmount";
		this.lblFcyAmount.Size = new System.Drawing.Size(72, 15);
		this.lblFcyAmount.TabIndex = 57;
		this.lblFcyAmount.Text = "Fcy Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(5, 27);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch Name";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(944, 77);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(111, 23);
		this.txtFcyAmount.TabIndex = 17;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance182).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(944, 21);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtDeliveryDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryDays.Location = new System.Drawing.Point(663, 50);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(55, 23);
		this.txtDeliveryDays.TabIndex = 10;
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtDeliveryDays_TextChanged);
		this.txtDeliveryDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryDays_KeyPress);
		this.lblFcyCode.AutoSize = true;
		this.lblFcyCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFcyCode.Location = new System.Drawing.Point(859, 27);
		this.lblFcyCode.Name = "lblFcyCode";
		this.lblFcyCode.Size = new System.Drawing.Size(55, 15);
		this.lblFcyCode.TabIndex = 56;
		this.lblFcyCode.Text = "Fcy Code";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(96, 77);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(81, 23);
		this.txtdocno.TabIndex = 2;
		this.DueDate.CustomFormat = "dd-MMM-yyyy";
		this.DueDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DueDate.Location = new System.Drawing.Point(417, 105);
		this.DueDate.Name = "DueDate";
		this.DueDate.Size = new System.Drawing.Size(122, 22);
		this.DueDate.TabIndex = 8;
		this.DueDate.ValueChanged += new System.EventHandler(DueDate_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(552, 54);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(100, 15);
		this.label33.TabIndex = 43;
		this.label33.Text = "Dlvy Days && Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(552, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(73, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Tax Account";
		this.DocDate.CustomFormat = "dd-MMM-yyyy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(180, 77);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(136, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(417, 48);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(5, 54);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project Name";
		this.CmbTaxAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Location = new System.Drawing.Point(663, 76);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Name = "CmbTaxAccount";
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).Size = new System.Drawing.Size(190, 24);
		((System.Windows.Forms.Control)(object)this.CmbTaxAccount).TabIndex = 12;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(324, 54);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 15);
		this.label1.TabIndex = 38;
		this.label1.Text = "Payment Term";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(5, 81);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Date";
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(417, 77);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(122, 23);
		this.txtDueDays.TabIndex = 7;
		this.txtDueDays.TextChanged += new System.EventHandler(txtDueDays_TextChanged);
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(552, 27);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 33;
		this.label30.Text = "Delivery Term";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(417, 23);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(122, 23);
		this.txtbillno.TabIndex = 5;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(324, 81);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(58, 15);
		this.label22.TabIndex = 37;
		this.label22.Text = "Due Days";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(552, 107);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(83, 19);
		this.label3.TabIndex = 17;
		this.label3.Text = "Bill Amount";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(663, 22);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(190, 24);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(324, 109);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(57, 15);
		this.label4.TabIndex = 36;
		this.label4.Text = "Due Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(324, 27);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Manual Bill No";
		this.txtBillAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 11f);
		this.txtBillAmount.Location = new System.Drawing.Point(663, 104);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(190, 25);
		this.txtBillAmount.TabIndex = 13;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(5, 109);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Supplier Name";
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance214).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(96, 21);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 0;
		this.cmbsuppliername.AllowNull = (DefaultableBoolean)1;
		this.cmbsuppliername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsuppliername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsuppliername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsuppliername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsuppliername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Location = new System.Drawing.Point(96, 103);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Name = "cmbsuppliername";
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbsuppliername).TabIndex = 4;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(96, 48);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ChkVoucherPreview);
		this.panel13.Controls.Add(this.label23);
		this.panel13.Controls.Add(this.ChkBok);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1200, 26);
		this.panel13.TabIndex = 37;
		this.ChkVoucherPreview.AutoSize = true;
		this.ChkVoucherPreview.Checked = true;
		this.ChkVoucherPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkVoucherPreview.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkVoucherPreview.ForeColor = System.Drawing.Color.White;
		this.ChkVoucherPreview.Location = new System.Drawing.Point(1052, 3);
		this.ChkVoucherPreview.Margin = new System.Windows.Forms.Padding(2);
		this.ChkVoucherPreview.Name = "ChkVoucherPreview";
		this.ChkVoucherPreview.Size = new System.Drawing.Size(121, 19);
		this.ChkVoucherPreview.TabIndex = 44;
		this.ChkVoucherPreview.Text = "Voucher Preview";
		this.ChkVoucherPreview.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(174, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "PURCHASE INVOICE";
		this.ChkBok.AutoSize = true;
		this.ChkBok.Checked = true;
		this.ChkBok.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBok.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(977, 3);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(72, 19);
		this.ChkBok.TabIndex = 14;
		this.ChkBok.Text = "Preview";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1249, 85);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(43, 26);
		this.txtCommAgentId.TabIndex = 17;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1249, 114);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(43, 26);
		this.txtSupplierGLId.TabIndex = 16;
		this.txtSupplierGLId.Visible = false;
		this.cmbstockprty.AllowNull = (DefaultableBoolean)1;
		this.cmbstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Location = new System.Drawing.Point(1249, 146);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Name = "cmbstockprty";
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Size = new System.Drawing.Size(43, 24);
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbstockprty).Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1225, 173);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1442, 135);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.Size = new System.Drawing.Size(43, 22);
		this.txtcommamount.TabIndex = 15;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1359, 116);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "UOM";
		this.label19.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(1357, 139);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1359, 61);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(71, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Type";
		this.label11.Visible = false;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommrate.Location = new System.Drawing.Point(1442, 83);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(43, 22);
		this.txtcommrate.TabIndex = 13;
		this.txtcommrate.Visible = false;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.cmbcommuom.AllowNull = (DefaultableBoolean)1;
		this.cmbcommuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Location = new System.Drawing.Point(1442, 108);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Name = "cmbcommuom";
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Size = new System.Drawing.Size(43, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommuom).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommuom).Leave += new System.EventHandler(cmbcommuom_Leave);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1359, 87);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Comm Rate";
		this.label21.Visible = false;
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(1442, 56);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(43, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.cmbcommagent.AllowNull = (DefaultableBoolean)1;
		this.cmbcommagent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommagent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommagent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommagent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcommagent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Location = new System.Drawing.Point(1442, 29);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Name = "cmbcommagent";
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Size = new System.Drawing.Size(43, 24);
		((System.Windows.Forms.Control)(object)this.cmbcommagent).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcommagent).Visible = false;
		this.txtcommremarks.Location = new System.Drawing.Point(1442, 161);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(43, 23);
		this.txtcommremarks.TabIndex = 16;
		this.txtcommremarks.Text = "";
		this.txtcommremarks.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1359, 34);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(78, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Agent";
		this.label20.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1357, 165);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label25.Visible = false;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 31);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1691, 185);
		this.panel4.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1200, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.toolStripSeparator1, this.btnNew, this.btnFrmRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.toolStripSeparator2, this.btnPrint,
			this.btnPartySlip, this.btnSlip, this.btnLoadPo
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1200, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(60, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnFrmRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnFrmRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnFrmRefresh.Name = "btnFrmRefresh";
		this.btnFrmRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnFrmRefresh.Text = "&Refresh";
		this.btnFrmRefresh.Click += new System.EventHandler(btnFrmRefresh_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(74, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(116, 24);
		this.btnPrint.Text = "103-&Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPartySlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPartySlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPartySlip.Name = "btnPartySlip";
		this.btnPartySlip.Size = new System.Drawing.Size(138, 24);
		this.btnPartySlip.Text = "1604A-&PartySlip";
		this.btnPartySlip.Click += new System.EventHandler(btnPartySlip_Click);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(124, 24);
		this.btnSlip.Text = "1604-&ItemSlip";
		this.btnSlip.Click += new System.EventHandler(btnSlipDetail_Click);
		this.btnLoadPo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadPo.Name = "btnLoadPo";
		this.btnLoadPo.Size = new System.Drawing.Size(66, 24);
		this.btnLoadPo.Text = "Load &PO";
		this.btnLoadPo.Click += new System.EventHandler(toolStripButton3_Click_1);
		this.tabHistory.Controls.Add(this.panel17);
		this.tabHistory.Controls.Add(this.panel16);
		this.tabHistory.Controls.Add(this.panel12);
		this.tabHistory.Controls.Add(this.panel14);
		this.tabHistory.Controls.Add(this.panel6);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1206, 681);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel15);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 419);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1200, 259);
		this.panel17.TabIndex = 54;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 23);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1200, 236);
		this.panel19.TabIndex = 47;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1200, 236);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 46;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label32);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1200, 23);
		this.panel15.TabIndex = 45;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 3);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(256, 18);
		this.label32.TabIndex = 25;
		this.label32.Text = "Detail of the Above Selected Row";
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 131);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1200, 288);
		this.panel16.TabIndex = 53;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1200, 288);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 44;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		this.panel12.Controls.Add(this.groupBox3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 56);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1200, 75);
		this.panel12.TabIndex = 52;
		this.groupBox3.Controls.Add(this.button1);
		this.groupBox3.Controls.Add(this.rdapproved);
		this.groupBox3.Controls.Add(this.rdmodify);
		this.groupBox3.Controls.Add(this.rddoc);
		this.groupBox3.Controls.Add(this.rdentry);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(974, 69);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproved.AutoSize = true;
		this.rdapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproved.Location = new System.Drawing.Point(869, 44);
		this.rdapproved.Name = "rdapproved";
		this.rdapproved.Size = new System.Drawing.Size(105, 19);
		this.rdapproved.TabIndex = 8;
		this.rdapproved.Text = "Approved Date";
		this.rdapproved.UseVisualStyleBackColor = true;
		this.rdmodify.AutoSize = true;
		this.rdmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodify.Location = new System.Drawing.Point(869, 25);
		this.rdmodify.Name = "rdmodify";
		this.rdmodify.Size = new System.Drawing.Size(91, 19);
		this.rdmodify.TabIndex = 7;
		this.rdmodify.Text = "Modify Date";
		this.rdmodify.UseVisualStyleBackColor = true;
		this.rddoc.AutoSize = true;
		this.rddoc.Checked = true;
		this.rddoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rddoc.Location = new System.Drawing.Point(783, 25);
		this.rddoc.Name = "rddoc";
		this.rddoc.Size = new System.Drawing.Size(75, 19);
		this.rddoc.TabIndex = 5;
		this.rddoc.TabStop = true;
		this.rddoc.Text = "Doc Date";
		this.rddoc.UseVisualStyleBackColor = true;
		this.rdentry.AutoSize = true;
		this.rdentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentry.Location = new System.Drawing.Point(783, 45);
		this.rdentry.Name = "rdentry";
		this.rdentry.Size = new System.Drawing.Size(80, 19);
		this.rdentry.TabIndex = 6;
		this.rdentry.Text = "Entry Date";
		this.rdentry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(869, 196);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(479, 19);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(86, 15);
		this.label59.TabIndex = 142;
		this.label59.Text = "Supplier Name";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(378, 19);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(276, 19);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(143, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 20);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance195).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(378, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(276, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel14.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label9);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 31);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1200, 25);
		this.panel14.TabIndex = 29;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1161, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(254, 19);
		this.label9.TabIndex = 24;
		this.label9.Text = "PURCHASE INVOICE HISTORY";
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1200, 28);
		this.panel6.TabIndex = 28;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1200, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(69, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(78, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(toolStripButton4_Click);
		this.button1.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.button1.Location = new System.Drawing.Point(725, 36);
		this.button1.Margin = new System.Windows.Forms.Padding(2);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(53, 26);
		this.button1.TabIndex = 143;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(btnshow_Click);
		this.button1.Leave += new System.EventHandler(button1_Leave);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1214, 707);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "PurchaseInvoiceTradePro";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchaseInvoiceTradePro_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchaseInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchaseInvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFreight).EndInit();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.tabPage3.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel8.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUOM).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSchedule).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsuppliername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommagent).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
