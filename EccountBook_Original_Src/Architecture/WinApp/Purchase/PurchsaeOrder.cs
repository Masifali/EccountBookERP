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
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SMSSystem;
using Architecture.WinApp.SupplierPortal.Forms;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class PurchsaeOrder : Form
{
	public class Criteria
	{
		public int ItemId { get; set; }

		public int PackSizeId { get; set; }

		public int RateUomId { get; set; }

		public int CropYearId { get; set; }

		public double Rate { get; set; }

		public override bool Equals(object obj)
		{
			if (obj is Criteria other)
			{
				if (ItemId == other.ItemId && PackSizeId == other.PackSizeId && RateUomId == other.RateUomId && CropYearId == other.CropYearId)
				{
					return Rate == other.Rate;
				}
				return false;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return ((((17 * 23 + ItemId.GetHashCode()) * 23 + PackSizeId.GetHashCode()) * 23 + RateUomId.GetHashCode()) * 23 + CropYearId.GetHashCode()) * 23 + Rate.GetHashCode();
		}
	}

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string OrderDetailRemoveIds = "";

	private bool UpdateMode;

	private bool IsApproved;

	private int updateDetailIndex;

	private int ItemId;

	private int DetailPoId;

	public int RecId;

	private int OrderDefaultDeliveryDays;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double JuteBagsCut;

	private double PPBagsCut;

	private double PreviousQty;

	private DataTable table = new DataTable();

	private DataTable dtlab = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtEmpType = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtEmptyBagsItem = new DataTable();

	private DataTable dtChargeToProduct = new DataTable();

	private DataTable dtSupplierLoadingDetail = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private bool BranchImplemented;

	private bool HasMultiCurrencyFeature;

	private bool ItemSearchWithNameOrCode;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool WarningMessageOnPurchaseOrderForExpenseGridIsOn;

	private bool StopShowingPurchaseOrderEmptyBagsWeightValidations;

	private static bool CalculateByQty = true;

	private static bool CalculateByWeight = false;

	private static bool PCalculateByPercent = false;

	private static bool PCalculateByAmount = true;

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadPendingSupplierDispatchForPO LoadTransactions;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private GroupBox groupBox1;

	private TextBox txtcatsr;

	private Label label33;

	private Label label12;

	private TextBox txtsupprefno;

	private Label label7;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private GroupBox GroupCommission;

	private Label label6;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtCommrate;

	private Label label11;

	private TextBox txtcommamount;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private Label label32;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private Label label29;

	private Label label28;

	private TextBox txtdeliverydays;

	private CheckBox ChkCommission;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtamount;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label46;

	private Label label40;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel8;

	internal UltraCombo combsuppname;

	internal UltraCombo combsalesman;

	internal UltraCombo combitem;

	internal UltraCombo combjob;

	internal UltraCombo combitempck;

	internal UltraCombo combcityarea;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Label label3;

	internal UltraCombo CmbCropyr;

	private Panel panel11;

	private Label label31;

	private ToolStripButton BtnRefresh;

	private CheckBox ChkBoxPrint;

	private TextBox txtMoisture;

	private Label label34;

	private ToolStripButton btnSMS;

	internal UltraCombo combrateuom;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combordercat;

	internal UltraCombo combpttrm;

	internal GridEX grdDetail;

	internal GridEX grdhistory;

	private ToolStrip toolStrip2;

	internal UltraCombo CmbStatus;

	internal UltraCombo combruom;

	internal UltraCombo combcommtype;

	private ToolStripButton btnDelete;

	private ToolStripButton btnUpdate;

	private TabControl tabControl2;

	private TabPage PurchaseOrderDetail;

	private TabPage EmptyBags;

	private TabPage ExpensesChargeToProduct;

	internal GridEX grdEmptyBags;

	internal GridEX grdExpensesChargeToProduct;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryAccount;

	private Label label1;

	private Label label25;

	private Label label15;

	private Label label10;

	private TextBox txtBrokeryRate;

	private Label label4;

	private TextBox txtBrokeryAmount;

	private Label label51;

	private Label label48;

	private Label label27;

	private Label label53;

	private Label label52;

	private TabPage LabDeductionStandard;

	internal GridEX grdlab;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnDefineCity;

	private RadioButton rdFreightCredit;

	private RadioButton rdFreightCash;

	internal UltraCombo CmbLabSampleNo;

	private Label label2;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private Panel panel10;

	private Panel panel14;

	private Label label42;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label55;

	private Label label54;

	private Label label50;

	internal UltraCombo cmbCurrency;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private TextBox txtOrderAmount;

	private TextBox txtOrderWeight;

	private TextBox txtOrderQty;

	private Label label59;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel7;

	private Panel panel9;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripSeparator toolStripSeparator2;

	private Label label60;

	private TextBox txtPpBagCut;

	private Label label61;

	private TextBox txtJuteBagCut;

	private Panel panel1;

	private GroupBox groupBox2;

	private Button btnshow;

	private Label label16;

	private Label label22;

	private Label label43;

	private Label label63;

	internal UltraCombo cmbSupplierNameHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel6;

	private Panel panel12;

	private Panel panel13;

	private Panel panel15;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label9;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator6;

	private TextBox txtremarks;

	private TabPage SupplierExpense;

	internal GridEX grdInvExp;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label13;

	private ToolStripButton BtnPrintII;

	private ToolStripButton BtnPrintWithLabSample;

	private CheckBox chkPrintIII;

	private CheckBox chkPrintII;

	private RadioButton RdPartyByCode;

	private RadioButton RdPartyByName;

	private Label labelLocationType;

	internal UltraCombo cmbLocationType;

	private Panel panel5;

	private RadioButton RadCategory;

	private RadioButton RadType;

	private Label label18;

	internal UltraCombo CmbCategory;

	private ToolStripButton BtnLoadOrder;

	internal GridEX grdSupplierLoadingDetail;

	private ToolStripButton BtnPrintNew;

	private CheckBox ChkBoxPrintA;

	private TextBox txtDocNoShow;

	private Label label5;

	internal UltraCombo CmbBookingPerson;

	private Label label49;

	private Button btnDefineBookingPerson;

	private Label label14;

	internal UltraCombo CmbBookingPersonHistory;

	private TabPage tabPaymentDetail;

	internal GridEX grdPaymentDetail;

	private RadioButton radFactoryStandard;

	private RadioButton radFactorySample;

	private Panel panel16;

	public PurchsaeOrder(UserAccount user)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		tabControl2.DrawItem += tabControl2_DrawItem;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 625);
			tabControl2.DrawMode = TabDrawMode.OwnerDrawFixed;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnprint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			OrderDefaultDeliveryDays = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OrderDefaultDeliveryDays"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseOrderBranchWise"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "StopShowingPurchaseOrderEmptyBagsWeightValidations");
			StopShowingPurchaseOrderEmptyBagsWeightValidations = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			OtherItemsBind();
			LocationTypeFill();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("rowUpdatedByLoadingMore", typeof(bool));
			table.Columns.Add("flagForSupplierDispatch", typeof(bool));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item", typeof(string));
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear", typeof(string));
			table.Columns.Add("ItemPackUOMID", typeof(int));
			table.Columns.Add("UOM", typeof(string));
			table.Columns.Add("PackEquivalent", typeof(double));
			table.Columns.Add("ItemQTY", typeof(decimal));
			table.Columns.Add("Weight", typeof(decimal));
			table.Columns.Add("RateUOMID", typeof(int));
			table.Columns.Add("RateUOM", typeof(string));
			table.Columns.Add("RateEquivalent", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("Job/LotID", typeof(int));
			table.Columns.Add("Job/Lot", typeof(string));
			table.Columns.Add("CityId", typeof(int));
			table.Columns.Add("CityName", typeof(string));
			table.Columns.Add("Moisture", typeof(string));
			table.Columns.Add("LabSampleId", typeof(int));
			table.Columns.Add("LabSample", typeof(string));
			table.Columns.Add("Remarks", typeof(string));
			table.Columns.Add("FcyAmount", typeof(decimal));
			table.Columns.Add("LabAnalysisStandardScheduleId", typeof(int));
			grd.DataSource = table;
			dtPackingType.Columns.Add("Id");
			dtPackingType.Columns.Add("PackTypeDesc");
			EmptyBagsTypeFill();
			PackingTypeFillForEmptyBagsGrid();
			EmptyBagsItemFill();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("InventoryParentCategoriesId");
			dtitem.Columns.Add("ItemCategoryId");
			dtitem.Columns.Add("ItemCategory");
			dtitem.Columns.Add("ItemTypeId");
			dtitem.Columns.Add("ItemType");
			dtitem.Columns.Add("ProductionStageId");
			dtitem.Columns.Add("ProductionStage");
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("SupplierCustomerId");
			dtAccounts.Columns.Add("AccountTitle");
			AccountTitleFill();
			dtEmptyBags.Columns.Add("EmptyBagsType");
			dtEmptyBags.Columns.Add("ItemId");
			dtEmptyBags.Columns.Add("ItemRate");
			dtEmptyBags.Columns.Add("PackingType");
			dtEmptyBags.Columns.Add("WeightCut");
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			gridEmptyBagsSettings();
			dtChargeToProduct.Columns.Add("AccountId");
			dtChargeToProduct.Columns.Add("GlAccountId");
			dtChargeToProduct.Columns.Add("Percentage");
			dtChargeToProduct.Columns.Add("ItemQty", typeof(double));
			dtChargeToProduct.Columns.Add("ItemRate", typeof(double));
			dtChargeToProduct.Columns.Add("Amount", typeof(double));
			dtChargeToProduct.Columns.Add("Remarks");
			AddRowChargeToProductGrid();
			grdExpensesChargeToProduct.RetrieveStructure();
			grdChargeToProductSetting();
			dtlab.Columns.Add("HeaderId");
			dtlab.Columns.Add("DetailId");
			dtlab.Columns.Add("ItemId");
			dtlab.Columns.Add("PerameterId");
			dtlab.Columns.Add("ItemName");
			dtlab.Columns.Add("AnalysisPerameter");
			dtlab.Columns.Add("RangeFrom", typeof(double));
			dtlab.Columns.Add("RangeTo", typeof(double));
			dtlab.Columns.Add("DeductionOn");
			dtlab.Columns.Add("Weight");
			dtlab.Columns.Add("StandardValue", typeof(double));
			dtlab.Columns.Add("DeductionValue", typeof(double));
			dtInvExp.Columns.Add("ItemId");
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtSupplierLoadingDetail.Columns.Add("Id", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("LoadingId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("LoadingDetailId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("PreBillNo", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("ItemId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("ItemName", typeof(string));
			dtSupplierLoadingDetail.Columns.Add("CropYearId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("CropYear", typeof(string));
			dtSupplierLoadingDetail.Columns.Add("ItemUOMId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("PackUom", typeof(string));
			dtSupplierLoadingDetail.Columns.Add("PackEquivalent", typeof(double));
			dtSupplierLoadingDetail.Columns.Add("PackingTypeId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("PackingType", typeof(string));
			dtSupplierLoadingDetail.Columns.Add("ItemQty", typeof(double));
			dtSupplierLoadingDetail.Columns.Add("NetWeight", typeof(double));
			dtSupplierLoadingDetail.Columns.Add("RateUOMId", typeof(int));
			dtSupplierLoadingDetail.Columns.Add("RateUom", typeof(string));
			dtSupplierLoadingDetail.Columns.Add("RateEquivalent", typeof(double));
			dtSupplierLoadingDetail.Columns.Add("ItemRate", typeof(double));
			dtSupplierLoadingDetail.Columns.Add("Amount", typeof(double));
			grdSupplierLoadingDetail.DataSource = dtSupplierLoadingDetail;
			grdSupplierLoadingDetail.RetrieveStructure();
			grdSettingForSupplierLoading();
			AddRowsInExpenseGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtPaymentTerm.Columns.Add("PaymentTerm", typeof(int));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("%OfTotal", typeof(decimal));
			dtPaymentTerm.Columns.Add("Amount", typeof(decimal));
			dtPaymentTerm.Columns.Add("Remarks", typeof(string));
			AddRowInPaymentGrid();
			grdPaymentDetail.DataSource = dtPaymentTerm;
			grdPaymentDetail.RetrieveStructure();
			gridPaymentTermSetting();
			ChkBoxPrint.Checked = true;
			OrderStatus();
			OrderCatagoryfill();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			BookingPersonFill();
			CommissionUOMFill();
			CommissionTypeFill();
			DeliveryTerm();
			PaymentTerms();
			UOMFill();
			ItemCategoryOrTypeBind(Conversion.ToInt(combordercat.Value));
			ItemdtFillFromAll(Conversion.ToInt(combordercat.Value), Conversion.ToInt(CmbCategory.Value));
			ItemNameBind();
			combojoblotfill();
			DocumentNoFill();
			BranchSrNoFill();
			cmbCityFill();
			CropYear();
			CurrencyFill();
			FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
			DocDate.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtcommamount.Enabled = false;
			tabControl2.SelectedIndex = 0;
			txtdeliverydays.Text = Conversion.ToString((object)OrderDefaultDeliveryDays);
			combordercat_Leave(null, null);
			defaultConfiquration();
			multiCurrencyFeature();
			DataTable dtTerm = (DataTable)((UltraGridBase)combpttrm).DataSource;
			if (dtTerm != null && dtTerm.Rows.Count > 0)
			{
				((UltraGridBase)combpttrm).Rows[2].Activate();
			}
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryBranchComboFill();
			HistorySupplierComboFill(NotValidateBranch: true);
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbStatus).Enabled = btnUpdate.Visible;
			((Control)(object)grdSupplierLoadingDetail).Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_DrawItem(object sender, DrawItemEventArgs e)
	{
		TabPage tabPage = tabControl2.TabPages[e.Index];
		Rectangle tabRect = tabControl2.GetTabRect(e.Index);
		Color color = ((e.Index == tabControl2.SelectedIndex) ? Color.White : Color.White);
		Color textColor = ((e.Index == tabControl2.SelectedIndex) ? Color.Black : Color.Red);
		using (SolidBrush b = new SolidBrush(color))
		{
			e.Graphics.FillRectangle(b, tabRect);
		}
		TextRenderer.DrawText(e.Graphics, tabPage.Text, tabControl2.Font, tabRect, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
	}

	private void LocationTypeFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetLocationType();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbLocationType, "Id", "Location", "Location Type", ZeroIndex: true);
				((UltraGridBase)cmbLocationType).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OtherItemsBind()
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

	private DataTable FactorySampleOrStandardDbCall()
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			if (dt != null && dt.Columns.Count == 0)
			{
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Description", typeof(string));
			}
			if (radFactorySample.Checked)
			{
				DataTable sampleNoBySupplierCustomerIdAndITemId = InvLabSampleAnalysisHeader.GetSampleNoBySupplierCustomerIdAndITemId(new InvLabSampleAnalysisHeader
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = RecId,
					ItemId = Conversion.ToInt(combitem.Value),
					SupplierCustomerId = Conversion.ToInt(combsuppname.Value),
					CommissionAgentId = Conversion.ToInt(combsalesman.Value)
				});
				dt.Clear();
				foreach (DataRow row in sampleNoBySupplierCustomerIdAndITemId.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocNo"]);
				}
			}
			else
			{
				DataTable dataTable = LabAnalysisStandardSchedule.ComboFill(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocDate = DocDate.Value
				});
				dt.Clear();
				foreach (DataRow row2 in dataTable.Rows)
				{
					string name = Convert.ToString(row2["LabAnalysisStandardScheduleName"]);
					string date = Convert.ToDateTime(row2["EffectiveDateFrom"]).ToShortDateString();
					dt.Rows.Add(row2["LabAnalysisGroupId"], name + " - " + date);
				}
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FactorySampleOrStandardbind(DataTable dt)
	{
		try
		{
			string Caption = (radFactorySample.Checked ? "Saple No" : "Standard");
			InfragisticsHelper.BindAndRetainSelection(CmbLabSampleNo, dt, "Id", "Description", Caption, AllColumns: true, Conversion.ToInt(CmbLabSampleNo.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderCatagoryfill()
	{
		try
		{
			int ID = Conversion.ToInt(combordercat.Value);
			DataTable dt = InvOrderCategory.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combordercat, "Id", "OrderCategoryName", "Category", true);
				((UltraGridBase)combordercat).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)combordercat).Rows[2].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combordercat.Value = ID;
					}
					else
					{
						((Control)(object)combordercat).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combordercat).Text = string.Empty;
				((UltraGridBase)combordercat).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			List<getGlobalSupplierCustomer> obj = (from r in clsGlobalVariables.globalSupplierCustomer.AsEnumerable()
				where r.CustomerGroupId != 7
				select r).ToList();
			dtSupplier.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				List<getGlobalSupplierCustomer> lst = (from r in obj.AsEnumerable()
					where r.PartyTypeId == 1 && r.CustomerGroupId != 7
					select r).ToList();
				if (lst != null && lst.Count > 0)
				{
					for (int i = 0; i < lst.Count; i++)
					{
						dtSupplier.Rows.Add(Conversion.ToInt((object)lst[i].Id), Conversion.ToString((object)lst[i].CompanyName), Conversion.ToInt((object)lst[i].GlAccountId), Conversion.ToString((object)lst[i].PartyCode), Conversion.ToInt((object)lst[i].CityId), Conversion.ToString((object)lst[i].CityName), Conversion.ToString((object)lst[i].MobilePersonal));
					}
				}
			}
			else if (obj != null && obj.Count > 0)
			{
				for (int j = 0; j < obj.Count; j++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[j].Id), Conversion.ToString((object)obj[j].CompanyName), Conversion.ToInt((object)obj[j].GlAccountId), Conversion.ToString((object)obj[j].PartyCode), Conversion.ToInt((object)obj[j].CityId), Conversion.ToString((object)obj[j].CityName), Conversion.ToString((object)obj[j].MobilePersonal));
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			int ID = Conversion.ToInt(combsuppname.Value);
			int ID2 = Conversion.ToInt(combsalesman.Value);
			int ID3 = Conversion.ToInt(CmbBrokeryAccount.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(combsuppname, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, ID, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)combsuppname).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)combsuppname).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				InfragisticsHelper.BindAndRetainSelection(combsalesman, dtSupplier, "Id", "CompanyName", "Commission Agent", AllColumns: true, ID2, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				InfragisticsHelper.BindAndRetainSelection(CmbBrokeryAccount, dtSupplier, "Id", "CompanyName", "Broker Ac", AllColumns: true, ID3, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
			else
			{
				((Control)(object)combsuppname).Text = string.Empty;
				((UltraGridBase)combsuppname).DataSource = null;
				((Control)(object)combsalesman).Text = string.Empty;
				((UltraGridBase)combsalesman).DataSource = null;
				((Control)(object)CmbBrokeryAccount).Text = string.Empty;
				((UltraGridBase)CmbBrokeryAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			InfragisticsHelper.BindAndRetainSelection(combpttrm, dtTerm, "Id", "Description", "Payment Term", AllColumns: false, Conversion.ToInt(combpttrm.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BookingPersonFill()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			Conversion.ToInt(CmbBookingPerson.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 5
			});
			InfragisticsHelper.BindAndRetainSelection(CmbBookingPerson, dt, "Id", "ReferencePartyName", "Booking Person", AllColumns: true, Conversion.ToInt(CmbBookingPerson.Value), insertDefaultRow: true, null, null, ActivateRow: false);
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
			int ID = Conversion.ToInt(combdeliverytrm.Value);
			DataTable dtTerm = DeliveryTerm.FormHistory();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDL(dtTerm, combdeliverytrm, "Id", "Description", "Delivery Term", ZeroIndex: true);
				((UltraGridBase)combdeliverytrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)combdeliverytrm).Rows[2].Activate();
				((UltraGridBase)combdeliverytrm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				CommonServices.SetComboValue(combdeliverytrm, "Id", ID, dtTerm);
			}
			else
			{
				((Control)(object)combdeliverytrm).Text = string.Empty;
				((UltraGridBase)combdeliverytrm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatus()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDLNew(dt, CmbStatus, "Id", "Status", "Status", true);
			((UltraGridBase)CmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbStatus).Rows[1].Activate();
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
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDLNew(dt, combcommtype, "Id", "CommissionType", "Comm Type", true);
			((UltraGridBase)combcommtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDLNew(dt, CmbBrokeryType, "Id", "CommissionType", "Brokery Type", true);
			((UltraGridBase)CmbBrokeryType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)combcommtype).Rows[1].Activate();
			((UltraGridBase)CmbBrokeryType).Rows[1].Activate();
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
			DataTable dt = CommonServices.StaticColumnsService("GetCommissionUom");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combruom, "Id", "type", "UOM", true);
				((UltraGridBase)combruom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dt, CmbBrokeryRateUom, "Id", "type", "UOM", true);
				((UltraGridBase)CmbBrokeryRateUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)combruom).Rows[1].Activate();
				((UltraGridBase)CmbBrokeryRateUom).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			int DocNo = CommonServices.PurchaseOrderGenerateCode(41);
			if (DocNo > 0)
			{
				txtdocno.Text = DocNo.ToString();
				txtDocNoShow.Text = "PO-" + DocNo;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchSrNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = PurchaseOrder.GeneratePurchaseOrderBranchCodeByDocId(new PurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 41,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemCategoryOrTypeBind(int ParentCategoryId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => (ParentCategoryId == 0 || row.InventoryParentCategoriesId == ParentCategoryId) && row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			if (RadCategory.Checked)
			{
				foreach (var category in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemCategory) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemCategory)
					select new
					{
						Id = Conversion.ToInt((object)row.ItemCategoryId),
						Description = row.ItemCategory
					}).Distinct().ToList())
				{
					dt.Rows.Add(category.Id, category.Description);
				}
			}
			else if (RadType.Checked)
			{
				foreach (var type in (from row in dtItemAll
					where !string.IsNullOrEmpty(row.ItemType) && !dt.Rows.Cast<DataRow>().Any((DataRow r) => r["Description"].ToString() == row.ItemType)
					select new
					{
						Id = Conversion.ToInt((object)row.ItemTypeId),
						Description = row.ItemType
					}).Distinct().ToList())
				{
					dt.Rows.Add(type.Id, type.Description);
				}
			}
			int ID = Conversion.ToInt(CmbCategory.Value);
			if (dt.Rows.Count > 0)
			{
				string DisplayName = (RadCategory.Checked ? "Item Category" : "ItemType");
				InfragisticsHelper.BindAndRetainSelection(CmbCategory, dt, "Id", "Description", DisplayName, AllColumns: true, ID, insertDefaultRow: false, null, null, ActivateRow: false);
				((UltraGridBase)CmbCategory).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFillFromAll(int ParentCategoryId, int CategoryOrTypeId = 0)
	{
		try
		{
			dtitem.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => (ParentCategoryId == 0 || row.InventoryParentCategoriesId == ParentCategoryId) && row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				int ItemTypeId = Conversion.ToInt((object)row2.ItemTypeId);
				int ItemCategoryId = Conversion.ToInt((object)row2.ItemCategoryId);
				if (CategoryOrTypeId == 0 || (RadCategory.Checked && CategoryOrTypeId == ItemCategoryId) || (RadType.Checked && CategoryOrTypeId == ItemTypeId))
				{
					dtitem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode, row2.InventoryParentCategoriesId, row2.ItemCategoryId, row2.ItemCategory, row2.ItemTypeId, row2.ItemType, row2.ItemProductionStageId, row2.productionStageName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int ID = Conversion.ToInt(combitem.Value);
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[6].Hidden = true;
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[8].Hidden = true;
				CommonServices.SetComboValue(combitem, "Id", ID, dtitem);
			}
			else
			{
				((Control)(object)combitem).Text = string.Empty;
				((UltraGridBase)combitem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			listUOM = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int detailId = 0)
	{
		try
		{
			if (Conversion.ToInt(combitem.Value) > 0)
			{
				if (listUOM.Rows.Count <= 0)
				{
					return;
				}
				string Packuom = ((Control)(object)combitempck).Text;
				string Rateuom = ((Control)(object)combrateuom).Text;
				((Control)(object)combitempck).Text = string.Empty;
				((Control)(object)combrateuom).Text = string.Empty;
				DataTable dtUOM = new DataTable();
				dtUOM.Columns.Add("Id");
				dtUOM.Columns.Add("UOM");
				dtUOM.Columns.Add("UOMCode", typeof(double));
				dtUOM.Columns.Add("BaseRateUom", typeof(bool));
				dtUOM.Rows.Clear();
				for (int i = 0; i < listUOM.Rows.Count; i++)
				{
					if (Conversion.ToInt(listUOM.Rows[i]["ItemId"]) == Conversion.ToInt(combitem.Value))
					{
						dtUOM.Rows.Add(listUOM.Rows[i]["Id"], listUOM.Rows[i]["UOMCode"], listUOM.Rows[i]["Equivalent"], listUOM.Rows[i]["BaseRateUom"]);
					}
				}
				if (dtUOM != null && dtUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtUOM, combitempck, "Id", "UOM", "UOM", ZeroIndex: true);
					DDL.BindDDL(dtUOM, combrateuom, "Id", "UOM", "RateUOM", ZeroIndex: true);
					((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)combitempck).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)combrateuom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)combrateuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOM") == Packuom))
					{
						((Control)(object)combitempck).Text = Packuom;
					}
					else
					{
						((Control)(object)combitempck).Text = string.Empty;
					}
					if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOM") == Rateuom))
					{
						((Control)(object)combrateuom).Text = Rateuom;
					}
					else
					{
						((Control)(object)combrateuom).Text = string.Empty;
					}
					if (detailId != 0)
					{
						return;
					}
					if (Conversion.ToInt(combitem.Value) != 0 && (Rateuom != "40" || Rateuom.ToUpper() != "40KG" || Conversion.ToInt(combrateuom.Value) == 0))
					{
						int BaseRateUomId = CommonServices.GetBaseRateUomId(dtUOM);
						if (BaseRateUomId > 0)
						{
							combrateuom.Value = BaseRateUomId;
						}
						else
						{
							DataRow foundRow = dtUOM.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<double>("UOMCode") == 40.0);
							int RateUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["Id"]) : 0);
							if (RateUomId > 0)
							{
								combrateuom.Value = RateUomId;
							}
						}
					}
					if (((Control)(object)combrateuom).Text.ToUpper() != "40KG" && ((Control)(object)combrateuom).Text != "40")
					{
						((Control)(object)combrateuom).Text = string.Empty;
					}
				}
				else
				{
					((Control)(object)combitempck).Text = string.Empty;
					((UltraGridBase)combitempck).DataSource = null;
					((Control)(object)combrateuom).Text = string.Empty;
					((UltraGridBase)combrateuom).DataSource = null;
				}
			}
			else
			{
				((Control)(object)combitempck).Text = string.Empty;
				((UltraGridBase)combitempck).DataSource = null;
				((Control)(object)combrateuom).Text = string.Empty;
				((UltraGridBase)combrateuom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(combjob.Value);
			DataTable dt = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combjob, "Id", "JobLotDescription", "JobLot", true);
				((UltraGridBase)combjob).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combjob.Value = ID;
					}
					else
					{
						((Control)(object)combjob).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combjob).Text = string.Empty;
				((UltraGridBase)combjob).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomers(1);
				if (dt.Rows.Count > 0)
				{
					dtAccounts.Clear();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtAccounts.Rows.Add(Conversion.ToInt(dt.Rows[i]["GlAccountId"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["CompanyName"]));
					}
				}
				return;
			}
			dtAccounts.Rows.Clear();
			DataTable dt2 = CommonServices.CoaAllocationAccountTitleByAccountTypeIds(null, "2,11,12,,13,14,15,20,21,22");
			if (dt2.Rows.Count > 0)
			{
				dtAccounts.Clear();
				for (int j = 0; j < dt2.Rows.Count; j++)
				{
					dtAccounts.Rows.Add(Conversion.ToInt(dt2.Rows[j]["Id"]), 0, Conversion.ToString(dt2.Rows[j]["AccountTitle"]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCityFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(combcityarea.Value);
			DataTable dt = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combcityarea, "Id", "CityName", "City Name", true);
				((UltraGridBase)combcityarea).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combcityarea.Value = ID;
					}
					else
					{
						((Control)(object)combcityarea).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combcityarea).Text = string.Empty;
				((UltraGridBase)combcityarea).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			int ID = Conversion.ToInt(CmbCropyr.Value);
			DataTable dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop != null && dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", true);
				if (ID > 0)
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCropyr.Value = ID;
					}
					else
					{
						((Control)(object)CmbCropyr).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropyr).Text = string.Empty;
				((UltraGridBase)CmbCropyr).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void defaultConfiquration()
	{
		try
		{
			int JobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (JobId > 0)
			{
				combjob.Value = JobId;
			}
			int CropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (CropYearId > 0)
			{
				CmbCropyr.Value = CropYearId;
			}
			int CityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("City Area"));
			if (CityId > 0)
			{
				combcityarea.Value = CityId;
			}
			JuteBagsCut = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForJuteBags"));
			if (JuteBagsCut != 0.0)
			{
				txtJuteBagCut.Text = JuteBagsCut.ToString("#,##0.###");
			}
			PPBagsCut = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForPPBags"));
			if (PPBagsCut != 0.0)
			{
				txtPpBagCut.Text = PPBagsCut.ToString("#,##0.###");
			}
			WarningMessageOnPurchaseOrderForExpenseGridIsOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WarningMessageOnPurchaseOrderForExpenseGridIsOn"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsalesman_Leave(object sender, EventArgs e)
	{
		((Control)(object)combcommtype).Enabled = true;
		txtCommrate.Enabled = true;
		((Control)(object)combruom).Enabled = true;
		txtcommamount.Enabled = true;
		combcommtype.Focus();
	}

	private void combordercat_Leave(object sender, EventArgs e)
	{
		try
		{
			int val = Conversion.ToInt(combordercat.Value);
			if (((UltraGridBase)combordercat).ActiveRow != null)
			{
				int code = CommonServices.GenerateOrderCategoryCodebyId(val);
				if (code > 0)
				{
					txtcatsr.Text = code.ToString();
				}
				int CategoryId = val switch
				{
					6 => 4, 
					1 => 3, 
					5 => 2, 
					4 => 1, 
					_ => 0, 
				};
				ItemCategoryOrTypeBind(CategoryId);
				ItemdtFillFromAll(CategoryId, Conversion.ToInt(CmbCategory.Value));
				ItemNameBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemdtFillFromAll(Conversion.ToInt(combordercat.Value) switch
			{
				6 => 4, 
				1 => 3, 
				5 => 2, 
				4 => 1, 
				_ => 0, 
			}, Conversion.ToInt(CmbCategory.Value));
			ItemNameBind();
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl2")
			{
				if (rdFreightCash.Checked)
				{
					rdFreightCash.Focus();
				}
				else
				{
					rdFreightCredit.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				combjob.Value = JobId;
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				CmbCropyr.Value = CropYearId;
			}
			FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsuppname_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)combsuppname).ActiveRow != null && Conversion.ToInt(((UltraDropDownBase)combsuppname).SelectedRow.Cells["CityId"].Value) > 0)
			{
				combcityarea.Value = Conversion.ToInt(((UltraDropDownBase)combsuppname).SelectedRow.Cells["CityId"].Value);
			}
			FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_Leave(object sender, EventArgs e)
	{
		try
		{
			txtduedays.Enabled = true;
			duedate.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1 || Conversion.ToInt(combpttrm.Value) == 3)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
				duedate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
	}

	private void RadCategory_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			int val = Conversion.ToInt(combordercat.Value);
			if (((UltraGridBase)combordercat).ActiveRow != null)
			{
				int code = CommonServices.GenerateOrderCategoryCodebyId(val);
				if (code > 0)
				{
					txtcatsr.Text = code.ToString();
				}
				int CategoryId = val switch
				{
					6 => 4, 
					1 => 3, 
					5 => 2, 
					4 => 1, 
					_ => 0, 
				};
				ItemCategoryOrTypeBind(CategoryId);
				ItemdtFillFromAll(CategoryId, Conversion.ToInt(CmbCategory.Value));
				ItemNameBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(combitem.Value) > 0)
				{
					ItemId = Conversion.ToInt(combitem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				combitem.Value = ItemId;
				combitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistorySupplierComboFill();
				return;
			}
			((Control)(object)cmbSupplierNameHistory).Text = string.Empty;
			((UltraGridBase)cmbSupplierNameHistory).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdPartyByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(combsuppname.Value) > 0)
				{
					Id = Conversion.ToInt(combsuppname.Value);
				}
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtSupplier, combsuppname, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtSupplier, combsuppname, "Id", "PartyCode", "Supplier Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					combsuppname.Value = Id;
				}
				combsuppname.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void radFactorySample_CheckedChanged(object sender, EventArgs e)
	{
		FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbLocationType).DataSource != null && (((UltraGridBase)cmbLocationType).ActiveRow == null || Conversion.ToInt(cmbLocationType.Value) == 0))
		{
			MessageBox.Show("Location Type Field is Required");
			cmbLocationType.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("Order No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combordercat).ActiveRow == null || Conversion.ToInt(combordercat.Value) == 0)
		{
			MessageBox.Show("Category Field is Required");
			combordercat.Focus();
			return false;
		}
		if (((UltraGridBase)combsuppname).ActiveRow == null || Conversion.ToInt(combsuppname.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			combsuppname.Focus();
			return false;
		}
		if (Conversion.ToInt(combordercat.Value) == 8 && (txtsupprefno.Text.Trim() == string.Empty || txtsupprefno.Text.Trim() == "0"))
		{
			MessageBox.Show("Supplier RefNo Field is Required");
			txtsupprefno.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null || Conversion.ToInt(combpttrm.Value) == 0)
		{
			MessageBox.Show("Payment Terms Field is Required");
			combpttrm.Focus();
			return false;
		}
		if ((Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit" || Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Company_Policy") && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due days Required when Payment Term is Credit");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStatus).ActiveRow == null || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Status Field is Required");
			CmbStatus.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		return true;
	}

	private bool FormDetailValidation()
	{
		if (((UltraGridBase)combitem).ActiveRow == null || Conversion.ToInt(combitem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropyr).ActiveRow == null || Conversion.ToInt(CmbCropyr.Value) == 0)
		{
			MessageBox.Show("CropYear Field is Required");
			CmbCropyr.Focus();
			return false;
		}
		if (((UltraGridBase)combjob).ActiveRow == null || Conversion.ToInt(combjob.Value) == 0)
		{
			MessageBox.Show("JobLot Field is Required");
			combjob.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null || Conversion.ToInt(combitempck.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			combitempck.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)combrateuom).ActiveRow == null || Conversion.ToInt(combrateuom.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			combrateuom.Focus();
			return false;
		}
		if (txtamount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount Field is Required");
			txtamount.Focus();
			return false;
		}
		return true;
	}

	private void PartyDisableEnableOnLabAnalysis()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				int SampleId = 0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt(r.Cells["LabSampleId"].Value) > 0)
					{
						SampleId = Conversion.ToInt(r.Cells["LabSampleId"].Value);
						break;
					}
				}
				if (SampleId > 0)
				{
					((Control)(object)combsuppname).Enabled = false;
				}
				else
				{
					((Control)(object)combsuppname).Enabled = true;
				}
				((Control)(object)combordercat).Enabled = false;
			}
			else
			{
				((Control)(object)combordercat).Enabled = true;
				((Control)(object)combsuppname).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void LabGridSetting()
	{
		try
		{
			grdlab.RootTable.Columns["HeaderId"].Visible = false;
			grdlab.RootTable.Columns["DetailId"].Visible = false;
			grdlab.RootTable.Columns["ItemId"].Visible = false;
			grdlab.RootTable.Columns["PerameterId"].Visible = false;
			grdlab.RootTable.Columns["StandardValue"].FormatString = "#,##0.##";
			grdlab.RootTable.Columns["DeductionValue"].FormatString = "#,##0.##";
			grdlab.RootTable.Columns["Weight"].FormatString = "0,0";
			grdlab.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdlab.RootTable.Columns["AnalysisPerameter"].EditType = (EditType)0;
			grdlab.RootTable.Columns["RangeFrom"].EditType = (EditType)0;
			grdlab.RootTable.Columns["RangeTo"].EditType = (EditType)0;
			grdlab.RootTable.Columns["DeductionOn"].EditType = (EditType)0;
			grdlab.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdlab.RootTable.Columns["StandardValue"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetLabDetailByItemId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable lab = InvLabAnalysisStandardDeductionPolicyHeader.ReadByItemId(new InvLabAnalysisStandardDeductionPolicyHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PolicyApplyOn = "PurchaseOrder",
				ItemId = Conversion.ToInt(combitem.Value)
			});
			if (lab.Rows.Count <= 0)
			{
				return;
			}
			for (int j = dtlab.Rows.Count - 1; j >= 0; j--)
			{
				DataRow dr = dtlab.Rows[j];
				if (Conversion.ToInt(dr["ItemId"]) == Conversion.ToInt(combitem.Value))
				{
					dr.Delete();
				}
			}
			for (int i = 0; i < lab.Rows.Count; i++)
			{
				dtlab.Rows.Add(lab.Rows[i]["headerId"], lab.Rows[i]["detailId"], lab.Rows[i]["ItemId"], lab.Rows[i]["AnalysisParameterId"], lab.Rows[i]["ItemName"], lab.Rows[i]["AnalysisPerameter"], lab.Rows[i]["RangeFrom"], lab.Rows[i]["RangeTo"], lab.Rows[i]["DeductFrom"], lab.Rows[i]["WeightKg"], lab.Rows[i]["DedValue"], lab.Rows[i]["DedValue"]);
			}
			grdlab.DataSource = dtlab;
			grdlab.RetrieveStructure();
			LabGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsItemFill()
	{
		try
		{
			dtEmptyBagsItem = CommonServices.GetPackingMaterialItemsAllocateToFlow();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFillForEmptyBagsGrid()
	{
		try
		{
			DataTable dtpkt = CommonServices.InvPackingTypeGetAllService();
			if (dtpkt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtpkt.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtpkt.Rows[i]["Id"]) == 1 || Conversion.ToInt(dtpkt.Rows[i]["Id"]) == 2)
				{
					dtPackingType.Rows.Add(dtpkt.Rows[i]["Id"], dtpkt.Rows[i]["PackTypeDesc"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EmptyBagsTypeFill()
	{
		try
		{
			dtEmpType = CommonServices.StaticColumnsService("PurchaseOrderEmptyBagsType");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvEmptyBagsGrid()
	{
		try
		{
			double WeightCutForJuteBags = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForJuteBags"));
			double WeightCutForPPBags = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeightCutForPPBags"));
			dtEmptyBags.Rows.Add(1, 0, 0, 1, WeightCutForJuteBags);
			dtEmptyBags.Rows.Add(1, 0, 0, 2, WeightCutForPPBags);
			grdEmptyBags.DataSource = dtEmptyBags;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowChargeToProductGrid()
	{
		dtChargeToProduct.Rows.Add(0, 0, 0, 0, 0, 0, "");
		grdExpensesChargeToProduct.DataSource = dtChargeToProduct;
	}

	private void AddRowsInExpenseGrid()
	{
		try
		{
			for (int i = 0; i < dtOtherItem.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(dtOtherItem.Rows[i]["Id"], 0, 0, 0, "");
			}
			grdInvExp.DataSource = dtInvExp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Other Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0)
				{
					if (Conversion.ToDouble(item.Cells["Qty"].Value) > 0.0 && Conversion.ToDouble(item.Cells["Rate"].Value) > 0.0)
					{
						double num = Conversion.ToDouble(item.Cells["Qty"].Value);
						double Rate = Conversion.ToDouble(item.Cells["Rate"].Value);
						double Amount = Math.Round(num * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						item.Cells["Amount"].Value = Amount;
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
			}
			else if (e.Column.Key == "Amount" && item != null && (int)item.RowType == 0 && item.Cells["Amount"].Value.ToString() != string.Empty)
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
				double Amount2 = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Amount"].Value = Amount2;
			}
			grdInvExp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdInvExp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPaymentGrid()
	{
		try
		{
			dtPaymentTerm.Rows.Add(0, 0, DateTime.Now, 0, 0, "");
			grdPaymentDetail.DataSource = dtPaymentTerm;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridPaymentTermRefresh()
	{
		try
		{
			DataTable dt = (DataTable)((UltraGridBase)combpttrm).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].ColumnType = (ColumnType)0;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].HasValueList = true;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].LimitToList = true;
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].Caption = "Payment Term";
				grdPaymentDetail.RootTable.Columns["PaymentTerm"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "Description");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridPaymentTermSetting()
	{
		try
		{
			GridPaymentTermRefresh();
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"PaymentTerm",
					(EditType)4
				},
				{
					"DueDays",
					(EditType)1
				},
				{
					"%OfTotal",
					(EditType)1
				},
				{
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				},
				{
					"DueDate",
					(EditType)3
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentDetail, 2, 3, null, editableColumns);
			grdPaymentDetail.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant + 50;
			grdPaymentDetail.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdPaymentDetail.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdPaymentDetail.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdPaymentDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			GridEX_Helper.AddButton(grdPaymentDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdPaymentDetail, "Add", "+", 20, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtPaymentTerm.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddRowInPaymentGrid();
				}
			}
			else if (e.Column.Key == "Add")
			{
				AddRowInPaymentGrid();
			}
			grdPaymentDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPaymentDetail.UpdateData();
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (grd.GetRows().Count() > 0)
				{
					decimal TotalAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Amount"));
					if (e.Column.Key == "%OfTotal")
					{
						decimal prcnt = Conversion.ToDecimal(item.Cells["%ofTotal"].Value);
						if (prcnt > 100m)
						{
							MessageBox.Show("%of Total Can't Greater than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							item.Cells["%ofTotal"].Value = 100;
							prcnt = 100m;
						}
						decimal amount2 = TotalAmount * prcnt / 100m;
						item.Cells["Amount"].Value = Math.Round(amount2, 4);
						PCalculateByPercent = true;
						PCalculateByAmount = false;
					}
					if (e.Column.Key == "Amount")
					{
						decimal amount = Conversion.ToDecimal(item.Cells["Amount"].Value);
						if (TotalAmount < amount)
						{
							MessageBox.Show($"Amount Cant be Greater than Order Amount:{TotalAmount}", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							amount = default(decimal);
							item.Cells["Amount"].Value = amount;
						}
						decimal prcnt2 = amount * 100m / TotalAmount;
						item.Cells["%ofTotal"].Value = Math.Round(prcnt2, 8);
						PCalculateByAmount = true;
						PCalculateByPercent = false;
					}
					if (e.Column.Key == "DueDays")
					{
						int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
						item.Cells["DueDate"].Value = DocDate.Value.AddDays(days2);
					}
					if (e.Column.Key == "DueDate")
					{
						DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
						if (date.Date < DocDate.Value.Date)
						{
							item.Cells["DueDate"].Value = DocDate.Value;
							throw new Exception("Due Date Can't less Than DocDate");
						}
						int days = (date.Date - DocDate.Value.Date).Days;
						item.Cells["DueDays"].Value = days;
					}
				}
				grdPaymentDetail.UpdateData();
				return;
			}
			throw new Exception("No Detail Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdPaymentDetail.CurrentColumn != null)
			{
				if (grdPaymentDetail.CurrentColumn.Key == "Delete")
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
					grdPaymentDetail.UpdateData();
				}
				else if (grdPaymentDetail.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentGrid();
					grdPaymentDetail.UpdateData();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtPaymentTerm.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddRowInPaymentGrid();
				}
				grdPaymentDetail.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentGrid();
				grdPaymentDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentAmountReCalculate()
	{
		try
		{
			decimal TotalAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Amount"));
			if (TotalAmount > 0m && grdPaymentDetail.GetRows().Count() > 0 && PCalculateByPercent)
			{
				GridEXRow[] rows = grdPaymentDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Amount = Conversion.ToDecimal(obj.Cells["%OfTotal"].Value) * TotalAmount / 100m;
					Amount = Math.Round(Amount, 4);
					obj.Cells["Amount"].Value = Amount;
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormDetailValidation())
			{
				return;
			}
			if (grdSupplierLoadingDetail.GetRows().Count() > 0)
			{
				MessageBox.Show("You Can Not Add Manual Record. Because record against supplier dispatch already exist");
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int ItemId = Conversion.ToInt(rows[i].Cells["ItemId"].Value);
				if (Conversion.ToInt(combitem.Value) == ItemId)
				{
					MessageBox.Show("Duplicate Item Not Add in Grid");
					return;
				}
			}
			if (((UltraGridBase)combrateuom).ActiveRow != null && Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value) != 40.0 && MessageBox.Show("Are you sure to add record because your RateUom not 40Kg?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				combrateuom.Focus();
				return;
			}
			int SampleId = 0;
			int LabAnalysisStandardScheduleId = 0;
			if (radFactorySample.Checked)
			{
				SampleId = Conversion.ToInt(CmbLabSampleNo.Value);
			}
			else
			{
				LabAnalysisStandardScheduleId = Conversion.ToInt(CmbLabSampleNo.Value);
			}
			table.Rows.Add(0, false, false, ((UltraDropDownBase)combitem).SelectedRow.Cells["ItemCode"].Value, combitem.Value, ((UltraDropDownBase)combitem).SelectedRow.Cells["ItemName"].Value, CmbCropyr.Value, ((Control)(object)CmbCropyr).Text.Trim(), Conversion.ToInt(combitempck.Value), ((Control)(object)combitempck).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value), txtqty.Text.Trim(), txtweight.Text.Trim(), Conversion.ToInt(combrateuom.Value), ((Control)(object)combrateuom).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtRate.Text.Trim()), Convert.ToDouble(txtamount.Text), Conversion.ToInt(combjob.Value), ((Control)(object)combjob).Text.Trim(), combcityarea.Value, ((Control)(object)combcityarea).Text.Trim(), txtMoisture.Text.Trim(), SampleId, (Conversion.ToInt(CmbLabSampleNo.Value) == 0) ? "" : ((Control)(object)CmbLabSampleNo).Text.Trim(), txtremarksdetail.Text.Trim(), LabAnalysisStandardScheduleId);
			GetLabDetailByItemId();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			ResetDetail();
			TotalCommissionAmount();
			TotalBrokeryAmountCalculate();
			CalculateTotalInformation();
			txtExchangeRate_TextChanged(null, null);
			PartyDisableEnableOnLabAnalysis();
			PaymentAmountReCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int CountIds = (from row in table.AsEnumerable()
				where row.Field<int>("Id") > 0
				select row).Count();
			GridEXRow r = grd.CurrentRow;
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (r == null || (int)r.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
			}
			else
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (id > 0)
				{
					if (CountIds > 1)
					{
						OrderDetailRemoveIds = OrderDetailRemoveIds + "," + id;
						r.Delete();
					}
					else
					{
						MessageBox.Show("All rows Can not be Deleted in Update Mode");
					}
				}
				else
				{
					r.Delete();
				}
			}
			PartyDisableEnableOnLabAnalysis();
			PaymentAmountReCalculate();
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
			GridEXRow item = grd.CurrentRow;
			if (item != null && !Conversion.ToBool(item.Cells["flagForSupplierDispatch"].Value))
			{
				updateDetailIndex = item.RowIndex;
				DetailPoId = Conversion.ToInt(item.Cells["Id"].Value);
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(DetailPoId);
				List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
				if (lstJob.Count > 0)
				{
					int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
					combjob.Value = JobId;
				}
				List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
				if (lstCrop.Count > 0)
				{
					int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
					CmbCropyr.Value = CropYearId;
				}
				int SampleId = Conversion.ToInt((object)item.Cells["LabSampleId"].Value.ToString());
				int LabAnalysisStandardScheduleId = Conversion.ToInt((object)item.Cells["LabAnalysisStandardScheduleId"].Value.ToString());
				if (SampleId > 0)
				{
					radFactorySample.Checked = SampleId > 0;
				}
				else
				{
					radFactoryStandard.Checked = true;
				}
				FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				combitempck.Value = item.Cells["ItemPackUOMID"].Value.ToString();
				txtqty.Text = item.Cells["ItemQTY"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				combrateuom.Value = item.Cells["RateUOMID"].Value.ToString();
				txtRate.Text = item.Cells["ItemRate"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				combjob.Value = item.Cells["Job/LotID"].Value.ToString();
				combcityarea.Value = item.Cells["CityId"].Value.ToString();
				((Control)(object)CmbCropyr).Text = item.Cells["CropYear"].Text.ToString();
				txtMoisture.Text = item.Cells["Moisture"].Value.ToString();
				CmbLabSampleNo.Value = ((SampleId > 0) ? SampleId : LabAnalysisStandardScheduleId);
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				tabControl2.SelectedIndex = 0;
				combitem.Focus();
			}
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
			if (!FormDetailValidation())
			{
				return;
			}
			if (Conversion.ToInt((object)this.ItemId) != Conversion.ToInt(combitem.Value))
			{
				GridEXRow[] rows = grd.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					int ItemId = Conversion.ToInt(rows[i].Cells["ItemId"].Value);
					if (Conversion.ToInt(combitem.Value) == ItemId)
					{
						MessageBox.Show("Duplicate Item Not Add in Grid");
						return;
					}
				}
			}
			if (((UltraGridBase)combrateuom).ActiveRow == null || Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value) == 40.0 || MessageBox.Show("Are you sure to add record because your RateUom not 40Kg?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				int SampleId = 0;
				int LabAnalysisStandardScheduleId = 0;
				if (radFactorySample.Checked)
				{
					SampleId = Conversion.ToInt(CmbLabSampleNo.Value);
				}
				else
				{
					LabAnalysisStandardScheduleId = Conversion.ToInt(CmbLabSampleNo.Value);
				}
				this.ItemId = 0;
				table.Rows[updateDetailIndex]["Id"] = DetailPoId;
				table.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)combitem).SelectedRow.Cells[2].Value;
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(combitem.Value);
				table.Rows[updateDetailIndex]["Item"] = ((UltraDropDownBase)combitem).SelectedRow.Cells[1].Value;
				table.Rows[updateDetailIndex]["ItemPackUOMID"] = Conversion.ToInt(combitempck.Value);
				table.Rows[updateDetailIndex]["UOM"] = ((Control)(object)combitempck).Text;
				table.Rows[updateDetailIndex]["ItemQTY"] = txtqty.Text;
				table.Rows[updateDetailIndex]["Weight"] = txtweight.Text;
				table.Rows[updateDetailIndex]["RateUOMID"] = Conversion.ToInt(combrateuom.Value);
				table.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)combrateuom).Text;
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtamount.Text);
				table.Rows[updateDetailIndex]["Job/LotID"] = Conversion.ToInt(combjob.Value);
				table.Rows[updateDetailIndex]["Job/Lot"] = ((Control)(object)combjob).Text;
				table.Rows[updateDetailIndex]["CityId"] = combcityarea.Value;
				table.Rows[updateDetailIndex]["CityName"] = ((Control)(object)combcityarea).Text;
				table.Rows[updateDetailIndex]["CropYearId"] = CmbCropyr.Value;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
				table.Rows[updateDetailIndex]["Moisture"] = txtMoisture.Text;
				table.Rows[updateDetailIndex]["LabSampleId"] = SampleId;
				table.Rows[updateDetailIndex]["LabSample"] = ((Conversion.ToInt(CmbLabSampleNo.Value) == 0) ? "" : ((Control)(object)CmbLabSampleNo).Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
				table.Rows[updateDetailIndex]["LabAnalysisStandardScheduleId"] = LabAnalysisStandardScheduleId;
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
				GetLabDetailByItemId();
				TotalCommissionAmount();
				TotalBrokeryAmountCalculate();
				CalculateTotalInformation();
				PartyDisableEnableOnLabAnalysis();
				PaymentAmountReCalculate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExpensesChargeToProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdExpensesChargeToProduct.CurrentRow;
				dtChargeToProduct.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtChargeToProduct.Rows.Count == 0)
				{
					AddRowChargeToProductGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowChargeToProductGrid();
			}
			grdExpensesChargeToProduct.UpdateData();
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
			grdExpensesChargeToProduct.UpdateData();
			GridEXRow r = grdExpensesChargeToProduct.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				string selectedAcId = Conversion.ToString((object)r.Cells["AccountId"].Text);
				if (selectedAcId != "0" && selectedAcId != string.Empty)
				{
					DataRow[] matchingRows = dtAccounts.Select("AccountTitle='" + selectedAcId + "'");
					if (matchingRows.Length != 0)
					{
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = Conversion.ToDouble(matchingRows[0].ItemArray[0]);
						r.EndEdit();
					}
				}
				if (!SubsidiaryAccountAllownOnVouchers)
				{
					if (Conversion.ToInt(((UltraDropDownBase)combsuppname).SelectedRow.Cells[2].Value) == Conversion.ToInt(r.Cells["GlAccountId"].Value))
					{
						MessageBox.Show("Selected Account Can not be Same As Supplier Account");
						r.BeginEdit();
						r.Cells["GlAccountId"].Value = 0;
						r.Cells["AccountId"].Value = 0;
						r.EndEdit();
						return;
					}
				}
				else if (Conversion.ToInt(combsuppname.Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
				{
					MessageBox.Show("Selected Account Can not be Same As Supplier Account");
					r.BeginEdit();
					r.Cells["AccountId"].Value = 0;
					r.Cells["GlAccountId"].Value = 0;
					r.EndEdit();
					return;
				}
			}
			grdExpensesChargeToProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExpensesChargeToProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdExpensesChargeToProduct.UpdateData();
			GridEXRow item = grdExpensesChargeToProduct.CurrentRow;
			if (e.Column.Key == "AccountId")
			{
				SupCustIdUpdateforFreightGrid();
			}
			if ((e.Column.Key == "ItemQty" || e.Column.Key == "ItemRate") && item != null && (int)item.RowType == 0 && item.Cells["ItemQty"].Value.ToString() != string.Empty && item.Cells["ItemRate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				item.Cells["Amount"].Value = Qty * Rate;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Amount"].Value = Math.Round(TotlaPercentage);
				}
				item.Cells["ItemQty"].Value = 0;
				item.Cells["ItemRate"].Value = 0;
			}
			grdExpensesChargeToProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			combitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			combitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		TotalCommissionAmount();
	}

	private void grdSettings()
	{
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["LabAnalysisStandardScheduleId"].Visible = false;
			grd.RootTable.Columns["flagForSupplierDispatch"].Visible = false;
			grd.RootTable.Columns["rowUpdatedByLoadingMore"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ItemPackUOMID"].Visible = false;
			grd.RootTable.Columns["PackEquivalent"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["Job/LotID"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["RateUOMID"].Visible = false;
			grd.RootTable.Columns["LabSampleId"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = false;
			grd.RootTable.Columns["Moisture"].Caption = "Moisture %";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName + 60;
			grd.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemQTY"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Job/Lot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["LabSample"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["Moisture"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQTY"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQTY"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			if (!Conversion.ToBool(grd.GetRow(0).Cells["flagForSupplierDispatch"].Value))
			{
				grd.RootTable.Columns.Add("Delete");
				grd.RootTable.Columns["Delete"].Key = "Delete";
				grd.RootTable.Columns["Delete"].Caption = "X";
				grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grd.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
				grd.RootTable.Columns["Delete"].Width = 35;
				grd.RootTable.Columns["Delete"].Position = 0;
				grd.FrozenColumns = 1;
				((Control)(object)grdSupplierLoadingDetail).Visible = false;
			}
			else
			{
				((Control)(object)grdSupplierLoadingDetail).Visible = true;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdEmptyBagsRefresh()
	{
		try
		{
			grdEmptyBags.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["ItemId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdEmptyBags.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtEmptyBagsItem.DefaultView, "ItemId", "ItemName");
			grdEmptyBags.RootTable.Columns["ItemId"].Width = 250;
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].Caption = "Type";
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].ValueList.PopulateValueList((IEnumerable)dtEmpType.DefaultView, "Id", "Type");
			grdEmptyBags.RootTable.Columns["EmptyBagsType"].Width = 250;
			grdEmptyBags.RootTable.Columns["PackingType"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["PackingType"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["PackingType"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["PackingType"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["PackingType"].Caption = "PackingType";
			grdEmptyBags.RootTable.Columns["PackingType"].ValueList.PopulateValueList((IEnumerable)dtPackingType.DefaultView, "Id", "PackTypeDesc");
			grdEmptyBags.RootTable.Columns["PackingType"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridEmptyBagsSettings()
	{
		try
		{
			GrdEmptyBagsRefresh();
			grdEmptyBags.RootTable.Columns["PackingType"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdChargeToProductRefresh()
	{
		try
		{
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].HasValueList = true;
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].LimitToList = true;
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].Caption = "AccountTitle";
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
			if (SubsidiaryAccountAllownOnVouchers)
			{
				grdExpensesChargeToProduct.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "SupplierCustomerId", "AccountTitle");
			}
			else
			{
				grdExpensesChargeToProduct.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
			}
			grdExpensesChargeToProduct.RootTable.Columns["AccountId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdChargeToProductSetting()
	{
		try
		{
			grdExpensesChargeToProduct.RootTable.Columns["GlAccountId"].Visible = false;
			grdExpensesChargeToProduct.RootTable.Columns["GlAccountId"].EditType = (EditType)0;
			grdChargeToProductRefresh();
			grdExpensesChargeToProduct.RootTable.Columns.Add("Delete");
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].Caption = "X";
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdExpensesChargeToProduct.RootTable.Columns["Delete"].Width = 20;
			grdExpensesChargeToProduct.RootTable.Columns.Add("Add");
			grdExpensesChargeToProduct.RootTable.Columns["Add"].Key = "Add";
			grdExpensesChargeToProduct.RootTable.Columns["Add"].Caption = "+";
			grdExpensesChargeToProduct.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdExpensesChargeToProduct.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdExpensesChargeToProduct.RootTable.Columns["Add"].ButtonText = "+";
			grdExpensesChargeToProduct.RootTable.Columns["Add"].Width = 20;
			grdExpensesChargeToProduct.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdExpensesChargeToProduct.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdExpensesChargeToProduct.RootTable.Columns["Amount"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_06a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Expected O, but got Unknown
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3c: Expected O, but got Unknown
		//IL_0c19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c20: Expected O, but got Unknown
		//IL_0eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb7: Expected O, but got Unknown
		//IL_1094: Unknown result type (might be due to invalid IL or missing references)
		//IL_109b: Expected O, but got Unknown
		//IL_1260: Unknown result type (might be due to invalid IL or missing references)
		//IL_1267: Expected O, but got Unknown
		//IL_166d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1674: Expected O, but got Unknown
		//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b2: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (Conversion.ToInt(combordercat.Value) == 8)
				{
					string RefNo = PurchaseOrder.GetSupplierRefNo(new PurchaseOrder
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 41,
						OrderCatagoryId = Conversion.ToInt(combordercat.Value)
					});
					if (RefNo != string.Empty && RefNo.ToUpper() == Conversion.ToString((object)txtsupprefno.Text.Trim()).ToUpper())
					{
						txtsupprefno.Focus();
						throw new Exception("SupplierRefNo already exist please check!");
					}
				}
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			if (grd.GetRows().Count() == 0)
			{
				tabControl2.SelectedIndex = 0;
				throw new Exception("Detail Information Required");
			}
			if (grdEmptyBags.GetRows().Count() == 0)
			{
				tabControl2.SelectedIndex = 1;
				throw new Exception("Empty Bags Information Required");
			}
			int AgentId = Conversion.ToInt(combsalesman.Value);
			double CommAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double CommRate = Conversion.ToDouble((object)txtCommrate.Text);
			if ((CommAmount > 0.0 || CommRate > 0.0) && AgentId == 0)
			{
				throw new Exception("Please Select Commission Agent Required when Commission Amount or Rate is Present...");
			}
			if (Conversion.ToInt(combsalesman.Value) > 0 && (CommAmount == 0.0 || CommRate == 0.0))
			{
				if (CommAmount == 0.0)
				{
					throw new Exception("Commission Amount Required when Commission Agent is Selected...");
				}
				if (CommRate == 0.0)
				{
					throw new Exception("Commission Rate Required when Commission Agent is Selected...");
				}
			}
			PurchaseOrder po = new PurchaseOrder();
			if (RecId > 0)
			{
				po.Id = RecId;
				po.IsAproved = IsApproved;
			}
			else
			{
				po.IsAproved = false;
			}
			po.DocumentTypeId = 41;
			po.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			po.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			po.OrderCatagoryId = Conversion.ToInt(combordercat.Value);
			po.CatagorySrNo = Conversion.ToInt((object)txtcatsr.Text);
			po.OrderSupCustId = Conversion.ToInt(combsuppname.Value);
			po.SupplierRefNo = Conversion.ToString((object)txtsupprefno.Text);
			po.BookingPersonId = Conversion.ToInt(CmbBookingPerson.Value);
			po.RemarksHeader = Conversion.ToString((object)txtremarks.Text.Trim());
			po.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
			po.OrderDueDate = duedate.Value;
			po.OrderExpiryDate = DateTime.Now;
			po.DeliveryTermId = Conversion.ToInt(combdeliverytrm.Value);
			po.DeliveryTerm = Conversion.ToString(((UltraDropDownBase)combdeliverytrm).SelectedRow.Cells[2].Value);
			po.DeliveryStartDate = deliverystartdate.Value;
			po.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
			po.OrderQty = Conversion.ToDecimal((object)txtOrderQty.Text);
			po.OrderWeight = Conversion.ToDecimal((object)txtOrderWeight.Text);
			po.OrderAmount = Conversion.ToDecimal((object)txtOrderAmount.Text);
			po.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			if (((UltraGridBase)combsalesman).ActiveRow != null && Conversion.ToInt(combsalesman.Value) > 0)
			{
				po.BrokerAgentSupCustId = Conversion.ToInt(combsalesman.Value);
				po.CommissionType = Conversion.ToString((object)((Control)(object)combcommtype).Text);
				po.CommRate = Conversion.ToDouble((object)txtCommrate.Text);
				po.UomScheduleIdCmRate = Conversion.ToInt((object)((Control)(object)combruom).Text);
				po.CommAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			if (((UltraGridBase)CmbBrokeryAccount).ActiveRow != null && Conversion.ToInt(CmbBrokeryAccount.Value) > 0)
			{
				po.BrokerAgentId = Conversion.ToInt(CmbBrokeryAccount.Value);
				po.BrokeryType = Conversion.ToString((object)((Control)(object)CmbBrokeryType).Text);
				po.BrokeryRate = Conversion.ToDouble((object)txtBrokeryRate.Text);
				po.BrokeryUom = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text);
				po.BrokeryAmount = Conversion.ToDouble((object)txtBrokeryAmount.Text.Trim());
			}
			if (rdFreightCash.Checked)
			{
				po.CashFreight = true;
			}
			else
			{
				po.CreditFreight = true;
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.EntryDate = DateTime.Now;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.ModifyDate = DateTime.Now;
			po.OrderStatus = ((Control)(object)CmbStatus).Text.Trim();
			po.LocationTypeId = Conversion.ToInt(cmbLocationType.Value);
			po.OrderDetailRemoveIds = OrderDetailRemoveIds;
			po.purchaseOrderDetailList = new List<PurchaseOrderDetail>();
			po.PurchaseorderEmptyBagsList = new List<PurchaseorderEmptyBags>();
			po.PurchaseOrderExpensesChargeToProductsList = new List<PurchaseOrderExpensesChargeToProduct>();
			po.PurchaseOrderLabDeductionList = new List<PurchaseOrderLabDeduction>();
			double OrderQty = 0.0;
			decimal DetailSumAmount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				PurchaseOrderDetail vd = new PurchaseOrderDetail();
				if (RecId > 0)
				{
					vd.Id = Conversion.ToInt(r5.Cells["Id"].Value);
				}
				else
				{
					vd.Id = 0;
				}
				vd.OrderItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
				Conversion.ToString(r5.Cells["Item"].Value);
				vd.OrderItemUOMId = Conversion.ToInt(r5.Cells["ItemPackUOMID"].Value);
				vd.OrderItemQty = Conversion.ToSingle(r5.Cells["ItemQTY"].Value);
				OrderQty += vd.OrderItemQty;
				vd.NetWeight = Conversion.ToSingle(r5.Cells["Weight"].Value);
				vd.OrderItemRate = Conversion.ToSingle(r5.Cells["ItemRate"].Value);
				vd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				vd.FcyAmount = Conversion.ToDecimal(r5.Cells["FcyAmount"].Value);
				_ = vd.OrderItemRate;
				if (Conversion.ToInt(r5.Cells["RateUOMID"].Value) > 0)
				{
					vd.OrderItemRateUOMId = Conversion.ToInt(r5.Cells["RateUOMID"].Value);
					vd.Amount = Conversion.ToDouble(r5.Cells["Amount"].Value);
					DetailSumAmount += Conversion.ToDecimal((object)vd.Amount);
					vd.JobLotId = Conversion.ToInt(r5.Cells["Job/LotID"].Value);
					vd.CityId = Conversion.ToInt(r5.Cells["CityId"].Value);
					vd.CityArea = Conversion.ToString((object)r5.Cells["CityName"].Text);
					vd.CropYearId = Conversion.ToInt(r5.Cells["CropYearId"].Value);
					vd.Crop = Conversion.ToString((object)r5.Cells["CropYear"].Text);
					vd.InvLabSampleAnalysisHeaderId = Conversion.ToInt(r5.Cells["LabSampleId"].Value);
					vd.LabSampleNo = Conversion.ToString(r5.Cells["LabSample"].Value);
					vd.LabAnalysisStandardScheduleId = Conversion.ToInt(r5.Cells["LabAnalysisStandardScheduleId"].Value);
					vd.OrderRemarks = Conversion.ToString(r5.Cells["Remarks"].Value);
					vd.Moisture = Conversion.ToString(r5.Cells["Moisture"].Value);
					po.purchaseOrderDetailList.Add(vd);
					continue;
				}
				MessageBox.Show("Rate UOM Not Found");
				return;
			}
			po.SupplierDispatchDetailList = new List<PurchaseOrderSupplierDispatchDetail>();
			rows = grdSupplierLoadingDetail.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				PurchaseOrderSupplierDispatchDetail d = new PurchaseOrderSupplierDispatchDetail();
				d.Id = ((RecId != 0) ? Conversion.ToInt(item2.Cells["Id"].Value) : 0);
				d.SupplierDispatchId = Conversion.ToInt(item2.Cells["LoadingId"].Value);
				d.LoadingDetailId = Conversion.ToInt(item2.Cells["LoadingDetailId"].Value);
				d.ItemId = Conversion.ToInt(item2.Cells["ItemId"].Value);
				d.CropYearId = Conversion.ToInt(item2.Cells["CropYearId"].Value);
				d.PackingTypeId = Conversion.ToInt(item2.Cells["PackingTypeId"].Value);
				d.ItemUOMId = Conversion.ToInt(item2.Cells["ItemUOMId"].Value);
				d.ItemQty = Conversion.ToDouble(item2.Cells["ItemQty"].Value);
				d.NetWeight = Conversion.ToDouble(item2.Cells["NetWeight"].Value);
				d.ItemRate = Conversion.ToDouble(item2.Cells["ItemRate"].Value);
				d.RateUOMId = Conversion.ToInt(item2.Cells["RateUOMId"].Value);
				d.Amount = Conversion.ToDouble(item2.Cells["Amount"].Value);
				po.SupplierDispatchDetailList.Add(d);
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				PurchaseorderEmptyBags emptyBags = new PurchaseorderEmptyBags();
				emptyBags.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				emptyBags.Type = Conversion.ToInt(r4.Cells["EmptyBagsType"].Value);
				emptyBags.Rate = Conversion.ToDouble(r4.Cells["ItemRate"].Value);
				emptyBags.PackingTypeId = Conversion.ToInt(r4.Cells["PackingType"].Value);
				emptyBags.WeightCut = Conversion.ToDouble(r4.Cells["WeightCut"].Value);
				_ = emptyBags.Type;
				if (emptyBags.ItemId > 0 && Conversion.ToInt((object)emptyBags.Type) != 5)
				{
					Conversion.ToInt((object)emptyBags.Type);
					_ = 4;
				}
				if (!StopShowingPurchaseOrderEmptyBagsWeightValidations)
				{
					if (emptyBags.Type == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdEmptyBags).Focus();
						throw new Exception("EmptyBagsType filed required In Empty bags Grid...");
					}
					if (emptyBags.PackingTypeId == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdEmptyBags).Focus();
						throw new Exception("PackingType filed required In Empty bags Grid...");
					}
					if (emptyBags.Type != 2 && emptyBags.WeightCut <= 0.0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdEmptyBags).Focus();
						throw new Exception("WeightCut filed required In Empty bags Grid...");
					}
					if (emptyBags.WeightCut > 0.0)
					{
						int validationTypeId = 0;
						if (emptyBags.PackingTypeId == 1 || emptyBags.PackingTypeId == 2 || emptyBags.PackingTypeId == 5)
						{
							validationTypeId = emptyBags.PackingTypeId;
						}
						else
						{
							validationTypeId = 2;
						}
						InvPackingType packType = clsGlobalVariables.globalInvPackingType.Find((InvPackingType row) => row.Id == validationTypeId);
						if (packType != null && (emptyBags.WeightCut < packType.MinEbWeight || emptyBags.WeightCut > packType.MaxEbWeight))
						{
							throw new Exception($"Weight Cut Should be in Range of: {packType.MinEbWeight} to {packType.MaxEbWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
						}
					}
				}
				po.PurchaseorderEmptyBagsList.Add(emptyBags);
			}
			rows = grdExpensesChargeToProduct.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				PurchaseOrderExpensesChargeToProduct expensesChargeToProduct = new PurchaseOrderExpensesChargeToProduct();
				if (Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
				{
					expensesChargeToProduct.AccountId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					if (expensesChargeToProduct.AccountId == 0)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdExpensesChargeToProduct).Focus();
						throw new Exception("AccountTitle Field Required");
					}
					if (SubsidiaryAccountAllownOnVouchers)
					{
						expensesChargeToProduct.AccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						expensesChargeToProduct.SupplierCustomerId = Conversion.ToInt(r3.Cells["AccountId"].Value);
					}
					else
					{
						expensesChargeToProduct.AccountId = Conversion.ToInt(r3.Cells["GlAccountId"].Value);
						expensesChargeToProduct.SupplierCustomerId = Conversion.ToInt((object)0);
					}
					expensesChargeToProduct.Percentage = Conversion.ToDouble(r3.Cells["Percentage"].Value);
					expensesChargeToProduct.Qty = Conversion.ToDouble(r3.Cells["ItemQty"].Value);
					expensesChargeToProduct.Rate = Conversion.ToDouble(r3.Cells["ItemRate"].Value);
					expensesChargeToProduct.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					expensesChargeToProduct.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					po.PurchaseOrderExpensesChargeToProductsList.Add(expensesChargeToProduct);
				}
			}
			rows = grdlab.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				PurchaseOrderLabDeduction lab = new PurchaseOrderLabDeduction();
				lab.InvLabAnalysisStandardDeductionPolicyHeaderId = Conversion.ToInt(r2.Cells["HeaderId"].Value);
				lab.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				lab.AnalysisParameterId = Conversion.ToInt(r2.Cells["PerameterId"].Value);
				lab.RangeFrom = Conversion.ToDecimal(r2.Cells["RangeFrom"].Value);
				lab.RangeTo = Conversion.ToDecimal(r2.Cells["RangeTo"].Value);
				lab.DeductFrom = Conversion.ToString(r2.Cells["DeductionOn"].Value);
				lab.StandardValue = Conversion.ToDecimal(r2.Cells["StandardValue"].Value);
				lab.DeductionValue = Conversion.ToDecimal(r2.Cells["DeductionValue"].Value);
				lab.WeightKgs = Conversion.ToDecimal(r2.Cells["Weight"].Value);
				po.PurchaseOrderLabDeductionList.Add(lab);
			}
			po.PurchaseOrderSupplierExpList = new List<PurchaseOrderSupplierExpense>();
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
				{
					PurchaseOrderSupplierExpense PE = new PurchaseOrderSupplierExpense();
					PE.InvRevExpItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PE.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text.Trim());
					if (PE.Remarks == "0" || PE.Remarks == string.Empty || PE.Remarks == null)
					{
						PE.Remarks = "Expense : " + Conversion.ToString((object)r.Cells["ItemId"].Text.Trim()) + "  Qty" + PE.Qty + "  @" + PE.Rate;
					}
					po.PurchaseOrderSupplierExpList.Add(PE);
				}
			}
			if (WarningMessageOnPurchaseOrderForExpenseGridIsOn && po.PurchaseOrderSupplierExpList.Count == 0)
			{
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update? There is No Expense Added", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
				}
				else if (MessageBox.Show("Are you sure to Save? There is No Expense Added", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			po.PurchaseOrderPaymentTermsDetailList = new List<PurchaseOrderPaymentTermsDetail>();
			decimal num = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentDetail, "Amount"));
			decimal PaymentDetailAmount = default(decimal);
			decimal PrcntOfTotal = default(decimal);
			if (num > 0m)
			{
				if (grdPaymentDetail.RowCount == 1)
				{
					if (PCalculateByAmount)
					{
						PCalculateByAmount = false;
						PCalculateByPercent = true;
					}
					PaymentAmountReCalculate();
				}
				rows = grdPaymentDetail.GetDataRows();
				foreach (GridEXRow item in rows)
				{
					PurchaseOrderPaymentTermsDetail pd = new PurchaseOrderPaymentTermsDetail();
					pd.PaymentTermId = Conversion.ToInt(item.Cells["PaymentTerm"].Value);
					pd.PaymentTerm = Conversion.ToString((object)item.Cells["PaymentTerm"].Text);
					pd.DueDays = Conversion.ToInt(item.Cells["DueDays"].Value);
					pd.PrcntOfTotal = Conversion.ToDecimal(item.Cells["%OfTotal"].Value);
					pd.Amount = Conversion.ToDecimal(item.Cells["Amount"].Value);
					pd.DueDate = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					PrcntOfTotal += pd.PrcntOfTotal;
					PaymentDetailAmount += pd.Amount;
					if (pd.PaymentTermId <= 0)
					{
						MessageBox.Show("Payment Term Required in row#" + (item.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 5;
						((Control)(object)grdPaymentDetail).Focus();
						return;
					}
					if (pd.PaymentTermId == 2 && pd.DueDays <= 0)
					{
						MessageBox.Show("Due Days Required In case Of Credit row in row#" + (item.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 5;
						((Control)(object)grdPaymentDetail).Focus();
						return;
					}
					po.PurchaseOrderPaymentTermsDetailList.Add(pd);
				}
			}
			else
			{
				PurchaseOrderPaymentTermsDetail psD = new PurchaseOrderPaymentTermsDetail();
				psD.PaymentTermId = Conversion.ToInt(combpttrm.Value);
				psD.PaymentTerm = Conversion.ToString((object)((Control)(object)combpttrm).Text);
				psD.DueDays = Conversion.ToInt((object)txtduedays.Text);
				psD.DueDate = po.DocDate.AddDays(psD.DueDays);
				psD.PrcntOfTotal = 100m;
				psD.Amount = DetailSumAmount;
				po.PurchaseOrderPaymentTermsDetailList.Add(psD);
				PaymentDetailAmount = DetailSumAmount;
				PrcntOfTotal = 100m;
			}
			if (Math.Abs(PaymentDetailAmount - DetailSumAmount) > 0.3m)
			{
				MessageBox.Show("Payment Detail Amount:" + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Amount:" + DetailSumAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 5;
				((Control)(object)grdPaymentDetail).Focus();
				return;
			}
			PrcntOfTotal = Math.Round(PrcntOfTotal, 4);
			if (Math.Abs(100m - PrcntOfTotal) > 0.01m)
			{
				MessageBox.Show("Payment Detail Total% not near to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 5;
				((Control)(object)grdPaymentDetail).Focus();
				return;
			}
			po.AttachmentsList = new List<DMSAttachments>();
			po.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<PurchaseOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = PurchaseOrder.Save(po);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....[" + po.DocNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 41, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....[" + po.DocNo + "]");
			}
			Reset();
			if (ChkBoxPrint.Checked)
			{
				CommonServices.PurchaseOrderSlipReport203(success);
			}
			if (ChkBoxPrintA.Checked)
			{
				CommonServices.SupplierDispatchSubReport_203A(success);
			}
			if (chkPrintII.Checked)
			{
				CommonServices.PurchaseOrderSlipReport203_01(success);
			}
			if (chkPrintIII.Checked)
			{
				CommonServices.PurchaseOrderSlipReport203_02(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			if (Conversion.ToString((object)((Control)(object)CmbStatus).Text) == "Complete" || Conversion.ToString((object)((Control)(object)CmbStatus).Text) == "Cancel")
			{
				MessageBox.Show("Order Status Should not Be Complete Or Cancel In Save Mode");
				CmbStatus.Focus();
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click_1(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			PurchaseOrder po = new PurchaseOrder();
			RecId = Conversion.ToInt((object)ID);
			po = PurchaseOrder.GetByID(RecId);
			OrderDetailRemoveIds = "";
			btnSaveAs.Visible = true;
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			txtDocNoShow.Text = " PO-" + Conversion.ToString((object)po.DocNo);
			txtBranchSrNo.Text = Conversion.ToString((object)po.BranchSrNo);
			combordercat.Value = Conversion.ToInt((object)po.OrderCatagoryId);
			txtcatsr.Text = po.CatagorySrNo.ToString();
			combsuppname.Value = Conversion.ToInt((object)po.OrderSupCustId);
			if (po.BookingPersonId > 0)
			{
				CmbBookingPerson.Value = po.BookingPersonId;
			}
			txtsupprefno.Text = Conversion.ToString((object)po.SupplierRefNo);
			txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
			combpttrm.Value = Conversion.ToInt((object)po.PaymentTermsId);
			txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
			duedate.Value = po.OrderDueDate;
			txtqty.Text = po.OrderQty.ToString("#,##0.###");
			txtweight.Text = po.OrderWeight.ToString("#,##0.###");
			txtamount.Text = po.OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			cmbCurrency.Value = Conversion.ToInt((object)po.CurrencyId);
			txtExchangeRate.Text = po.ExchangeRate.ToString(clsGlobalVariables.DecimalRateFormate);
			txtFcyAmount.Text = po.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			if (po.CurrencyId == 0)
			{
				multiCurrencyFeature();
			}
			combdeliverytrm.Value = Conversion.ToInt((object)po.DeliveryTermId);
			deliverystartdate.Value = po.DeliveryStartDate;
			txtdeliverydays.Text = Conversion.ToString((object)po.DeliveryDays);
			combsalesman.Value = Conversion.ToInt((object)po.BrokerAgentSupCustId);
			((Control)(object)combcommtype).Text = Conversion.ToString((object)po.CommissionType);
			((Control)(object)combruom).Text = Conversion.ToString((object)po.UomScheduleIdCmRate);
			txtCommrate.Text = po.CommRate.ToString(clsGlobalVariables.DecimalRateFormate);
			txtcommamount.Text = po.CommAmount.ToString(clsGlobalVariables.stringFormatsingle);
			CmbBrokeryAccount.Value = Conversion.ToInt((object)po.BrokerAgentId);
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)po.BrokeryType);
			txtBrokeryRate.Text = po.BrokeryRate.ToString(clsGlobalVariables.DecimalRateFormate);
			((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString((object)po.BrokeryUom);
			txtBrokeryAmount.Text = po.BrokeryAmount.ToString(clsGlobalVariables.stringFormatsingle);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)po.OrderStatus);
			IsApproved = Conversion.ToBool((object)po.IsAproved);
			rdFreightCash.Checked = Conversion.ToBool((object)po.CashFreight);
			rdFreightCredit.Checked = Conversion.ToBool((object)po.CreditFreight);
			if (!Conversion.ToBool((object)po.CreditFreight) && !Conversion.ToBool((object)po.CashFreight))
			{
				if (po.DeliveryTermId == 1 || po.DeliveryTermId == 3 || po.DeliveryTermId == 4)
				{
					rdFreightCash.Checked = true;
				}
				else
				{
					rdFreightCredit.Checked = true;
				}
			}
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			cmbLocationType.Value = Conversion.ToInt((object)po.LocationTypeId);
			table.Rows.Clear();
			for (int i2 = 0; i2 < po.purchaseOrderDetailList.Count; i2++)
			{
				table.Rows.Add(po.purchaseOrderDetailList[i2].Id, false, (po.SupplierDispatchDetailList != null && po.SupplierDispatchDetailList.Count > 0) ? true : false, po.purchaseOrderDetailList[i2].ItemCode, po.purchaseOrderDetailList[i2].OrderItemId, po.purchaseOrderDetailList[i2].ItemName, po.purchaseOrderDetailList[i2].CropYearId, po.purchaseOrderDetailList[i2].Crop, po.purchaseOrderDetailList[i2].OrderItemUOMId, po.purchaseOrderDetailList[i2].UOMCode, po.purchaseOrderDetailList[i2].UOMDescription, po.purchaseOrderDetailList[i2].OrderItemQty, po.purchaseOrderDetailList[i2].NetWeight, po.purchaseOrderDetailList[i2].OrderItemRateUOMId, po.purchaseOrderDetailList[i2].RateUom, po.purchaseOrderDetailList[i2].EquivalentRate, po.purchaseOrderDetailList[i2].OrderItemRate, po.purchaseOrderDetailList[i2].Amount, po.purchaseOrderDetailList[i2].JobLotId, po.purchaseOrderDetailList[i2].JobLotDescription, po.purchaseOrderDetailList[i2].CityId, po.purchaseOrderDetailList[i2].CityArea, po.purchaseOrderDetailList[i2].Moisture, po.purchaseOrderDetailList[i2].InvLabSampleAnalysisHeaderId, po.purchaseOrderDetailList[i2].LabSampleNo, po.purchaseOrderDetailList[i2].OrderRemarks, po.purchaseOrderDetailList[i2].FcyAmount, po.purchaseOrderDetailList[i2].LabAnalysisStandardScheduleId);
				if (po.SupplierDispatchDetailList != null && po.SupplierDispatchDetailList.Count > 0 && po.SupplierDispatchDetailList[i2].SupplierDispatchId > 0)
				{
					((Control)(object)combsuppname).Enabled = false;
					((Control)(object)combordercat).Enabled = false;
					((Control)(object)combdeliverytrm).Enabled = false;
					((Control)(object)grdSupplierLoadingDetail).Visible = true;
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtSupplierLoadingDetail.Rows.Clear();
			for (int n = 0; n < po.SupplierDispatchDetailList.Count; n++)
			{
				dtSupplierLoadingDetail.Rows.Add(po.SupplierDispatchDetailList[n].Id, po.SupplierDispatchDetailList[n].SupplierDispatchId, po.SupplierDispatchDetailList[n].LoadingDetailId, po.SupplierDispatchDetailList[n].SupplierDispatchNo, po.SupplierDispatchDetailList[n].ItemId, po.SupplierDispatchDetailList[n].ItemName, po.SupplierDispatchDetailList[n].CropYearId, po.SupplierDispatchDetailList[n].CropYear, po.SupplierDispatchDetailList[n].ItemUOMId, po.SupplierDispatchDetailList[n].ItemUomCode, po.SupplierDispatchDetailList[n].ItemUomEquivalent, po.SupplierDispatchDetailList[n].PackingTypeId, po.SupplierDispatchDetailList[n].PackingType, po.SupplierDispatchDetailList[n].ItemQty, po.SupplierDispatchDetailList[n].NetWeight, po.SupplierDispatchDetailList[n].RateUOMId, po.SupplierDispatchDetailList[n].RateUomCode, po.SupplierDispatchDetailList[n].RateUomEquivalent, po.SupplierDispatchDetailList[n].ItemRate, po.SupplierDispatchDetailList[n].Amount);
			}
			grdSupplierLoadingDetail.DataSource = dtSupplierLoadingDetail;
			grdSupplierLoadingDetail.RetrieveStructure();
			grdSettingForSupplierLoading();
			dtEmptyBags.Rows.Clear();
			for (int m = 0; m < po.PurchaseorderEmptyBagsList.Count; m++)
			{
				dtEmptyBags.Rows.Add(po.PurchaseorderEmptyBagsList[m].Type, po.PurchaseorderEmptyBagsList[m].ItemId, po.PurchaseorderEmptyBagsList[m].Rate, po.PurchaseorderEmptyBagsList[m].PackingTypeId, po.PurchaseorderEmptyBagsList[m].WeightCut);
			}
			grdEmptyBags.DataSource = dtEmptyBags;
			grdEmptyBags.RetrieveStructure();
			gridEmptyBagsSettings();
			dtChargeToProduct.Rows.Clear();
			for (int l = 0; l < po.PurchaseOrderExpensesChargeToProductsList.Count; l++)
			{
				dtChargeToProduct.Rows.Add(SubsidiaryAccountAllownOnVouchers ? po.PurchaseOrderExpensesChargeToProductsList[l].SupplierCustomerId : po.PurchaseOrderExpensesChargeToProductsList[l].AccountId, po.PurchaseOrderExpensesChargeToProductsList[l].AccountId, po.PurchaseOrderExpensesChargeToProductsList[l].Percentage, po.PurchaseOrderExpensesChargeToProductsList[l].Qty, po.PurchaseOrderExpensesChargeToProductsList[l].Rate, po.PurchaseOrderExpensesChargeToProductsList[l].Amount, po.PurchaseOrderExpensesChargeToProductsList[l].Remarks);
			}
			grdExpensesChargeToProduct.DataSource = dtChargeToProduct;
			grdExpensesChargeToProduct.RetrieveStructure();
			grdChargeToProductSetting();
			dtlab.Rows.Clear();
			for (int k = 0; k < po.PurchaseOrderLabDeductionList.Count; k++)
			{
				dtlab.Rows.Add(0, 0, po.PurchaseOrderLabDeductionList[k].ItemId, po.PurchaseOrderLabDeductionList[k].AnalysisParameterId, po.PurchaseOrderLabDeductionList[k].ItemName, po.PurchaseOrderLabDeductionList[k].AnalysisItem, po.PurchaseOrderLabDeductionList[k].RangeFrom, po.PurchaseOrderLabDeductionList[k].RangeTo, po.PurchaseOrderLabDeductionList[k].DeductFrom, po.PurchaseOrderLabDeductionList[k].WeightKgs, po.PurchaseOrderLabDeductionList[k].StandardValue, po.PurchaseOrderLabDeductionList[k].DeductionValue);
			}
			grdlab.DataSource = dtlab;
			grdlab.RetrieveStructure();
			LabGridSetting();
			dtInvExp.Rows.Clear();
			AddRowsInExpenseGrid();
			for (int j = 0; j < po.PurchaseOrderSupplierExpList.Count; j++)
			{
				DataRow[] rowToUpdate = dtInvExp.Select($"ItemId = '{po.PurchaseOrderSupplierExpList[j].InvRevExpItemId}'");
				if (rowToUpdate != null)
				{
					rowToUpdate[0].SetField("ItemId", po.PurchaseOrderSupplierExpList[j].InvRevExpItemId);
					rowToUpdate[0].SetField("Qty", po.PurchaseOrderSupplierExpList[j].Qty);
					rowToUpdate[0].SetField("Rate", po.PurchaseOrderSupplierExpList[j].Rate);
					rowToUpdate[0].SetField("Amount", po.PurchaseOrderSupplierExpList[j].Amount);
					rowToUpdate[0].SetField("Remarks", po.PurchaseOrderSupplierExpList[j].Remarks);
				}
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			if (dtChargeToProduct.Rows.Count == 0)
			{
				AddRowChargeToProductGrid();
			}
			dtPaymentTerm.Rows.Clear();
			for (int i = 0; i < po.PurchaseOrderPaymentTermsDetailList.Count; i++)
			{
				dtPaymentTerm.Rows.Add(po.PurchaseOrderPaymentTermsDetailList[i].PaymentTermId, po.PurchaseOrderPaymentTermsDetailList[i].DueDays, po.PurchaseOrderPaymentTermsDetailList[i].DueDate, po.PurchaseOrderPaymentTermsDetailList[i].PrcntOfTotal, po.PurchaseOrderPaymentTermsDetailList[i].Amount, po.PurchaseOrderPaymentTermsDetailList[i].PaymentRemarks);
			}
			if (dtPaymentTerm.Rows.Count == 0)
			{
				AddRowInPaymentGrid();
			}
			grdPaymentDetail.DataSource = dtPaymentTerm;
			grdPaymentDetail.RetrieveStructure();
			gridPaymentTermSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			txtExchangeRate_TextChanged(null, null);
			PartyDisableEnableOnLabAnalysis();
			((Control)(object)CmbStatus).Enabled = btnUpdate.Visible;
			combordercat_Leave(null, null);
			TotalCommissionAmount();
			TotalBrokeryAmountCalculate();
			PaymentAmountReCalculate();
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

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			PreviousQty = 0.0;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			OrderDetailRemoveIds = "";
			ItemId = 0;
			RecId = 0;
			DetailPoId = 0;
			IsApproved = false;
			((Control)(object)combsuppname).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			((Control)(object)combdeliverytrm).Enabled = true;
			txtdeliverydays.Text = Conversion.ToString((object)OrderDefaultDeliveryDays);
			deliverystartdate.Value = DateTime.Now;
			((Control)(object)combsalesman).Text = string.Empty;
			((Control)(object)combcommtype).Text = string.Empty;
			((Control)(object)combruom).Text = string.Empty;
			txtCommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)CmbBrokeryAccount).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			combitem.Value = 0;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtamount.Text = string.Empty;
			txtMoisture.Text = string.Empty;
			((Control)(object)combjob).Text = string.Empty;
			((Control)(object)combcityarea).Text = string.Empty;
			((Control)(object)CmbLabSampleNo).Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)combcommtype).Enabled = false;
			txtCommrate.Enabled = false;
			((Control)(object)combruom).Enabled = false;
			txtcommamount.Enabled = false;
			txtOrderQty.Text = string.Empty;
			txtOrderAmount.Text = string.Empty;
			txtOrderWeight.Text = string.Empty;
			txtremarks.Text = string.Empty;
			DocumentNoFill();
			BranchSrNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			dtlab.Rows.Clear();
			grdlab.DataSource = dtlab;
			defaultConfiquration();
			dtEmptyBags.Rows.Clear();
			dtChargeToProduct.Rows.Clear();
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			if (dtChargeToProduct.Rows.Count == 0)
			{
				AddRowChargeToProductGrid();
			}
			dtInvExp.Rows.Clear();
			AddRowsInExpenseGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtSupplierLoadingDetail.Rows.Clear();
			grdSupplierLoadingDetail.DataSource = dtSupplierLoadingDetail;
			grdSupplierLoadingDetail.RetrieveStructure();
			grdSettingForSupplierLoading();
			dtPaymentTerm.Rows.Clear();
			AddRowInPaymentGrid();
			grdPaymentDetail.RetrieveStructure();
			gridPaymentTermSetting();
			((Control)(object)grdSupplierLoadingDetail).Visible = false;
			tabControl2.SelectedIndex = 0;
			combordercat_Leave(null, null);
			combordercat.Focus();
			((Control)(object)CmbLabSampleNo).Text = string.Empty;
			((UltraGridBase)CmbLabSampleNo).DataSource = null;
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			((Control)(object)CmbStatus).Enabled = btnUpdate.Visible;
			PartyDisableEnableOnLabAnalysis();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			OrderDefaultDeliveryDays = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("OrderDefaultDeliveryDays"));
			BranchImplemented = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PurchaseOrderBranchWise"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "StopShowingPurchaseOrderEmptyBagsWeightValidations");
			StopShowingPurchaseOrderEmptyBagsWeightValidations = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			LocationTypeFill();
			List<string> allServices = new List<string> { "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			SupplierDtFillFromGlobal();
			BindSupplierName();
			CommissionUOMFill();
			BookingPersonFill();
			PaymentTerms();
			ItemCategoryOrTypeBind(Conversion.ToInt(combordercat.Value));
			ItemdtFillFromAll(Conversion.ToInt(combordercat.Value), Conversion.ToInt(CmbCategory.Value));
			ItemNameBind();
			combojoblotfill();
			cmbCityFill();
			CropYear();
			UOMFill();
			bindRateUomAndItemPackUom();
			GrdEmptyBagsRefresh();
			GridPaymentTermRefresh();
			if (dtEmptyBags.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			if (dtChargeToProduct.Rows.Count == 0)
			{
				AddRowChargeToProductGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ResetDetail()
	{
		try
		{
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtMoisture.Text = string.Empty;
			((Control)(object)combrateuom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtamount.Text = string.Empty;
			((Control)(object)CmbLabSampleNo).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			combitem.Value = 0;
			combitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkBoxPrint_Leave(object sender, EventArgs e)
	{
		combitem.Focus();
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
					decimal amount = Conversion.ToDecimal(obj.Cells["Amount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.BeginEdit();
					obj.Cells["FcyAmount"].Value = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
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
				DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void multiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label56.Visible = true;
				label57.Visible = true;
				label58.Visible = true;
				return;
			}
			((Control)(object)cmbCurrency).Visible = false;
			txtExchangeRate.Visible = false;
			txtFcyAmount.Visible = false;
			label56.Visible = false;
			label57.Visible = false;
			label58.Visible = false;
			DataTable dtCurren = CommonServices.GetConfigurationsByDefinitionIds("1");
			DataTable dtRate = CommonServices.GetConfigurationsByDefinitionIds("160");
			if (dtCurren.Rows.Count > 0)
			{
				cmbCurrency.Value = Conversion.ToInt(dtCurren.Rows[0]["ConfigKey"]);
			}
			if (dtRate.Rows.Count > 0)
			{
				txtExchangeRate.Text = Conversion.ToString(dtRate.Rows[0]["ConfigKey"]);
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				decimal TotalAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtOrderWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtOrderAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtOrderQty.Text = "0";
				txtOrderWeight.Text = "0";
				txtOrderAmount.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			int rate = 0;
			int totalamount = 0;
			if (txtweight.Text != string.Empty && ((Control)(object)combrateuom).Text != string.Empty && txtRate.Text != string.Empty && txtqty.Text != string.Empty)
			{
				Conversion.ToInt((object)txtweight.Text);
				Conversion.ToInt((object)((Control)(object)combrateuom).Text);
				rate = Conversion.ToInt((object)txtRate.Text);
				Conversion.ToInt((object)txtqty.Text);
				totalamount = Conversion.ToInt((object)(Convert.ToInt32(int.Parse(txtweight.ToString()) / int.Parse(((object)combrateuom).ToString())) * rate));
				txtamount.Text = totalamount.ToString();
				return;
			}
			throw new Exception("Value Not Valid");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtqty.Text);
			double Weight = Conversion.ToDouble((object)txtweight.Text);
			double Uom = ((((UltraGridBase)combitempck).ActiveRow != null && Conversion.ToInt(combitempck.Value) > 0) ? Conversion.ToDouble(((UltraDropDownBase)combitempck).SelectedRow.Cells[2].Value) : 0.0);
			string activeTag = Convert.ToString(base.ActiveControl?.AccessibleName);
			double calcValue = 0.0;
			if (Uom <= 0.0)
			{
				txtqty.Text = "0";
				txtweight.Text = "0";
			}
			else if (activeTag == Convert.ToString(txtqty.AccessibleName))
			{
				calcValue = ((Qty > 0.0) ? (Qty * Uom) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtweight.Text = calcValue.ToString("#,##0.###");
				CalculateByQty = true;
				CalculateByWeight = false;
			}
			else if (activeTag == Convert.ToString(txtweight.AccessibleName))
			{
				calcValue = ((Weight > 0.0) ? (Weight / Uom) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtqty.Text = calcValue.ToString("#,##0.###");
				CalculateByQty = false;
				CalculateByWeight = true;
			}
			else if (CalculateByQty)
			{
				calcValue = ((Qty > 0.0) ? (Qty * Uom) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtweight.Text = calcValue.ToString("#,##0.###");
			}
			else if (CalculateByWeight)
			{
				calcValue = ((Weight > 0.0) ? (Weight / Uom) : 0.0);
				calcValue = Math.Round(calcValue, 3);
				txtqty.Text = calcValue.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
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
			CalculateWeight();
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
			double weight = Conversion.ToDouble((object)txtweight.Text);
			double Rate = Conversion.ToDouble((object)txtRate.Text);
			double RateUOM = ((((UltraGridBase)combrateuom).ActiveRow != null && Conversion.ToInt(combrateuom.Value) > 0) ? Conversion.ToDouble(((UltraDropDownBase)combrateuom).SelectedRow.Cells[2].Value) : 0.0);
			double TotalAmount = 0.0;
			TotalAmount = ((RateUOM > 0.0) ? (weight / RateUOM * Rate) : 0.0);
			txtamount.Text = Math.Round(TotalAmount).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			txtcommamount.Text = "0";
			if (((UltraGridBase)combsalesman).ActiveRow != null)
			{
				decimal CommRate = Conversion.ToDecimal((object)txtCommrate.Text.Trim());
				decimal total = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Amount"));
				decimal Weight = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Weight"));
				decimal CommAmt = default(decimal);
				if (((Control)(object)combcommtype).Text == "Flat")
				{
					CommAmt = CommRate;
				}
				else if (((Control)(object)combcommtype).Text == "Percent")
				{
					CommAmt = total * CommRate / 100m;
				}
				else if (((Control)(object)combcommtype).Text == "Comm Weight")
				{
					decimal UOM = Conversion.ToDecimal((object)((Control)(object)combruom).Text.Trim());
					CommAmt = Weight / UOM * CommRate;
				}
				txtcommamount.Text = Math.Round(CommAmt, 4).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalBrokeryAmountCalculate()
	{
		try
		{
			txtBrokeryAmount.Text = "0";
			if (((UltraGridBase)CmbBrokeryAccount).ActiveRow != null)
			{
				decimal total = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Amount"));
				decimal Weight = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grd, "Weight"));
				decimal CommRate = Conversion.ToDecimal((object)txtBrokeryRate.Text.Trim());
				decimal CommAmt = default(decimal);
				if (((Control)(object)CmbBrokeryType).Text == "Flat")
				{
					CommAmt = CommRate;
				}
				else if (((Control)(object)CmbBrokeryType).Text == "Percent")
				{
					CommAmt = total * CommRate / 100m;
					CommAmt = Math.Round(CommAmt, 4);
				}
				else if (((Control)(object)CmbBrokeryType).Text == "Comm Weight")
				{
					decimal UOM = Conversion.ToDecimal((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
					CommAmt = Weight / UOM * CommRate;
					CommAmt = Math.Round(CommAmt, 4);
				}
				txtBrokeryAmount.Text = CommAmt.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtamount_TextChanged(object sender, EventArgs e)
	{
	}

	private void combrateuom_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommtype_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			((UltraGridBase)combruom).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combruom_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommtype_ValueChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		DueDaysCalculate();
		FactorySampleOrStandardbind(FactorySampleOrStandardDbCall());
		ResetStandardFromGrid();
	}

	private void ResetStandardFromGrid()
	{
		try
		{
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int num = Conversion.ToInt(r.Cells["LabAnalysisStandardScheduleId"].Value);
				r.BeginEdit();
				if (num > 0)
				{
					r.Cells["LabAnalysisStandardScheduleId"].Value = 0;
					r.Cells["LabSample"].Value = "";
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryAccount_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBrokeryRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmountCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combruom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryBranchComboFill()
	{
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = PurchaseOrder.GetBranchesAllocatedToUserFromPurchaseOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 41);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistorySupplierComboFill(bool NotValidateBranch = false)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			DataTable dtBookingPerson = dtSupplier.Clone();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "41";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			else if (!NotValidateBranch)
			{
				cmbBranchName.Focus();
				throw new Exception("Select branch first");
			}
			DataTable dt = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(obj);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "BookingPerson")
					{
						dtBookingPerson.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbSupplierNameHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtBookingPerson, CmbBookingPersonHistory, "Id", "Supplier", "Booking Person", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 41;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNameHistory.Value);
			obj.BookingPersonId = Conversion.ToInt(CmbBookingPersonHistory.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				DataTable dt = PurchaseOrder.Getall(obj);
				if (dt.Rows.Count > 0)
				{
					DataTable dtHistory = new DataTable();
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DocNo", typeof(int));
					dtHistory.Columns.Add("BranchSrNo", typeof(int));
					dtHistory.Columns.Add("BranchName", typeof(string));
					dtHistory.Columns.Add("DocDate", typeof(DateTime));
					dtHistory.Columns.Add("ItemQty", typeof(decimal));
					dtHistory.Columns.Add("SupplierName", typeof(string));
					dtHistory.Columns.Add("BookingPerson", typeof(string));
					dtHistory.Columns.Add("PaymentTerm", typeof(string));
					dtHistory.Columns.Add("DueDays", typeof(int));
					dtHistory.Columns.Add("DueDate", typeof(DateTime));
					dtHistory.Columns.Add("DeliveryTerm", typeof(string));
					dtHistory.Columns.Add("DeliveryDays", typeof(int));
					dtHistory.Columns.Add("DeliveryStartDate", typeof(DateTime));
					dtHistory.Columns.Add("ExpiryDate", typeof(DateTime));
					dtHistory.Columns.Add("OrderStatus", typeof(string));
					dtHistory.Columns.Add("CommAgent", typeof(string));
					dtHistory.Columns.Add("CommType", typeof(string));
					dtHistory.Columns.Add("CommRate", typeof(double));
					dtHistory.Columns.Add("CommAmount", typeof(double));
					dtHistory.Columns.Add("CommRemarks", typeof(string));
					dtHistory.Columns.Add("BrokeryAc", typeof(string));
					dtHistory.Columns.Add("BrokeryType", typeof(string));
					dtHistory.Columns.Add("BrokeryRate", typeof(double));
					dtHistory.Columns.Add("BrokeryAmount", typeof(double));
					dtHistory.Columns.Add("ApprovedStatus", typeof(string));
					dtHistory.Columns.Add("PreBillNos", typeof(string));
					dtHistory.Columns.Add("PreBillVehicleNos", typeof(string));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
					dtHistory.Columns.Add("Remarks", typeof(string));
					dtHistory.Columns.Add("LocationType", typeof(string));
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtHistory.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToInt(dt.Rows[i]["DocNo"]), Conversion.ToInt(dt.Rows[i]["BranchSrNo"]), Conversion.ToString(dt.Rows[i]["BranchName"]), Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToDecimal(dt.Rows[i]["OrderQty"]), Conversion.ToString(dt.Rows[i]["SupplierName"]), Conversion.ToString(dt.Rows[i]["BookingPerson"]), Conversion.ToString(dt.Rows[i]["TermsDescription"]), Conversion.ToInt(dt.Rows[i]["DueDays"]), Conversion.ToDateTime(dt.Rows[i]["DueDate"]).ToShortDateString(), Conversion.ToString(dt.Rows[i]["DeliveryTermDB"]), Conversion.ToInt(dt.Rows[i]["DeliveryDays"]), Conversion.ToDateTime(dt.Rows[i]["DeliveryStartDate"]).ToShortDateString(), Conversion.ToDateTime(dt.Rows[i]["OrderExpiryDate"]).ToShortDateString(), Conversion.ToString(dt.Rows[i]["OrderStatus"]), Conversion.ToString(dt.Rows[i]["CommissionAgentName"]), Conversion.ToString(dt.Rows[i]["CommissionType"]), Conversion.ToDouble(dt.Rows[i]["CommRate"]), Conversion.ToDouble(dt.Rows[i]["CommAmount"]), Conversion.ToString(dt.Rows[i]["CommissionRemarks"]), Conversion.ToString(dt.Rows[i]["BrokerAgentName"]), Conversion.ToString(dt.Rows[i]["BrokeryType"]), Conversion.ToDouble(dt.Rows[i]["BrokeryRate"]), Conversion.ToDouble(dt.Rows[i]["BrokeryAmount"]), Conversion.ToBool(dt.Rows[i]["IsAproved"]) ? "Approved" : "Not Approved", Conversion.ToString(dt.Rows[i]["PreBillNos"]), Conversion.ToString(dt.Rows[i]["PreBillVehicleNos"]), Conversion.ToString(dt.Rows[i]["UserName"]), dt.Rows[i]["EntryDate"], Conversion.ToString(dt.Rows[i]["ModifyUserName"]), dt.Rows[i]["ModifyDate"], dt.Rows[i]["ApprovedUserName"], dt.Rows[i]["PostDate"], Conversion.ToInt(dt.Rows[i]["NoOfAttachments"]), Conversion.ToString(dt.Rows[i]["RemarksHeader"]), Conversion.ToString(dt.Rows[i]["LocationType"]));
					}
					grdhistory.DataSource = dtHistory;
					grdhistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdhistory.DataSource = null;
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			if (BranchImplemented)
			{
				grdhistory.RootTable.Columns["BranchSrNo"].Visible = false;
				grdhistory.RootTable.Columns["BranchName"].Visible = false;
			}
			grdhistory.RootTable.Columns["Id"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["BranchName"].Width = Constants.InventoryConstants.BranchName;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName + 35;
			grdhistory.RootTable.Columns["BookingPerson"].Width = Constants.InventoryConstants.ReferenceParty;
			grdhistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdhistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdhistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdhistory.RootTable.Columns["DeliveryDays"].Width = Constants.InventoryConstants.Days + 20;
			grdhistory.RootTable.Columns["DeliveryStartDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdhistory.RootTable.Columns["ExpiryDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["OrderStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["CommAgent"].Width = Constants.InventoryConstants.CommissionAgent;
			grdhistory.RootTable.Columns["CommType"].Width = Constants.InventoryConstants.DiscountType;
			grdhistory.RootTable.Columns["CommRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdhistory.RootTable.Columns["CommAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["CommRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["BrokeryAc"].Width = Constants.InventoryConstants.CommissionAgent;
			grdhistory.RootTable.Columns["BrokeryType"].Width = Constants.InventoryConstants.DiscountType;
			grdhistory.RootTable.Columns["BrokeryRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdhistory.RootTable.Columns["BrokeryAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["ApprovedStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["LocationType"].Width = Constants.InventoryConstants.StringConstant;
			grdhistory.RootTable.Columns["PreBillNos"].Width = Constants.InventoryConstants.StringConstant;
			grdhistory.RootTable.Columns["PreBillVehicleNos"].Width = Constants.InventoryConstants.StringConstant;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			grdhistory.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			grdhistory.RootTable.Columns["BranchSrNo"].Width = 80;
			grdhistory.RootTable.Columns["BranchName"].Width = 150;
			grdhistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["BrokeryRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["BrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			if (formright.DoHaveUpdateRights)
			{
				grdhistory.RootTable.Columns.Add("Edit");
				grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["Edit"].Width = 40;
				grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdhistory.RootTable.Columns["Edit"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
			}
			if (formright.DoHaveSaveRight)
			{
				grdhistory.RootTable.Columns.Add("SaveAs");
				grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
				grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["SaveAs"].Width = 60;
				grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
				grdhistory.RootTable.Columns["SaveAs"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
			}
			if (formright.DoHavePrintRights)
			{
				grdhistory.RootTable.Columns.Add("Print");
				grdhistory.RootTable.Columns["Print"].Caption = "Print";
				grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
				grdhistory.RootTable.Columns["Print"].Width = 40;
				grdhistory.RootTable.Columns["Print"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
				grdhistory.RootTable.Columns.Add("Print-A");
				grdhistory.RootTable.Columns["Print-A"].Caption = "Print-A";
				grdhistory.RootTable.Columns["Print-A"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["Print-A"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["Print-A"].ButtonText = "Print-A";
				grdhistory.RootTable.Columns["Print-A"].Width = 50;
				grdhistory.RootTable.Columns["Print-A"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
				grdhistory.RootTable.Columns.Add("PrintII");
				grdhistory.RootTable.Columns["PrintII"].Caption = "PrintII";
				grdhistory.RootTable.Columns["PrintII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["PrintII"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["PrintII"].ButtonText = "PrintII";
				grdhistory.RootTable.Columns["PrintII"].Width = 50;
				grdhistory.RootTable.Columns["PrintII"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
				grdhistory.RootTable.Columns.Add("PrintIII");
				grdhistory.RootTable.Columns["PrintIII"].Caption = "PrintIII";
				grdhistory.RootTable.Columns["PrintIII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdhistory.RootTable.Columns["PrintIII"].ButtonStyle = (ButtonStyle)5;
				grdhistory.RootTable.Columns["PrintIII"].ButtonText = "PrintIII";
				grdhistory.RootTable.Columns["PrintIII"].Width = 55;
				grdhistory.RootTable.Columns["PrintIII"].Position = grdhistory.RootTable.Columns["DocNo"].Position - 1;
			}
			GridEX_Helper.AddButton(grdhistory, "AddAttachment", "Add Attachment", 105);
			grdhistory.FrozenColumns = 2;
			((Control)(object)grdhistory).Refresh();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getUpdateForHistory()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		try
		{
			if (grdhistory.CurrentRow.Cells.Count <= 0 || (int)grdhistory.CurrentRow.RowType != 0)
			{
				return;
			}
			PurchaseOrder po = new PurchaseOrder();
			po = PurchaseOrder.GetByID(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
			if (po != null)
			{
				DataTable table = new DataTable();
				table.Columns.Add("ItemCode", typeof(string));
				table.Columns.Add("ItemName", typeof(string));
				table.Columns.Add("CropYear", typeof(string));
				table.Columns.Add("UOM", typeof(string));
				table.Columns.Add("ItemQTY", typeof(double));
				table.Columns.Add("Weight", typeof(double));
				table.Columns.Add("ItemRate", typeof(double));
				table.Columns.Add("RateUOM", typeof(string));
				table.Columns.Add("Amount", typeof(double));
				table.Columns.Add("Job/Lot", typeof(string));
				table.Columns.Add("FcyAmount", typeof(decimal));
				table.Columns.Add("CityName", typeof(string));
				table.Columns.Add("Moisture%", typeof(string));
				table.Columns.Add("Remarks", typeof(string));
				table.Clear();
				for (int i = 0; i < po.purchaseOrderDetailList.Count; i++)
				{
					table.Rows.Add(po.purchaseOrderDetailList[i].ItemCode, po.purchaseOrderDetailList[i].ItemName, po.purchaseOrderDetailList[i].Crop, po.purchaseOrderDetailList[i].UOMCode, po.purchaseOrderDetailList[i].OrderItemQty, po.purchaseOrderDetailList[i].NetWeight, po.purchaseOrderDetailList[i].OrderItemRate, po.purchaseOrderDetailList[i].RateUom, Math.Round(po.purchaseOrderDetailList[i].Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero), po.purchaseOrderDetailList[i].JobLotDescription, po.purchaseOrderDetailList[i].FcyAmount, po.purchaseOrderDetailList[i].CityArea, po.purchaseOrderDetailList[i].Moisture, po.purchaseOrderDetailList[i].OrderRemarks);
				}
				grdDetail.DataSource = table;
				grdDetail.RetrieveStructure();
				grdDetailSettings();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["FcyAmount"].Visible = false;
			grdDetail.RootTable.Columns["ItemQTY"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			getUpdateForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = id;
				ReadById(RecId);
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnUpdate.Visible = true;
			}
			else if (e.Column.Key == "SaveAs")
			{
				Reset();
				RecId = id;
				ReadById(RecId);
				btnsave.Visible = false;
				btnUpdate.Visible = false;
				btnSaveAs.Visible = true;
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				((Control)(object)CmbStatus).Enabled = btnUpdate.Visible;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.PurchaseOrderSlipReport203(id);
			}
			else if (e.Column.Key == "Print-A")
			{
				CommonServices.SupplierDispatchSubReport_203A(id);
			}
			else if (e.Column.Key == "PrintII")
			{
				CommonServices.PurchaseOrderSlipReport203_01(id);
			}
			else if (e.Column.Key == "PrintIII")
			{
				CommonServices.PurchaseOrderSlipReport203_02(id);
			}
			else if (e.Column.Key == "AddAttachment")
			{
				AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, id, 41, base.Name);
				attachmentAddingFromHistory.BringToFront();
				attachmentAddingFromHistory.ShowDialog();
			}
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
				DocDate.Focus();
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 41);
			}
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = id;
				ReadById(RecId);
			}
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
			grdhistory.ClearStructure();
			grdDetail.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryBranchComboFill();
			HistorySupplierComboFill();
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
			gridhistoryfill();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintII_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203_01(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintWithLabSample_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.PurchaseOrderSlipReport203_02(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintNew_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SupplierDispatchSubReport_203A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSMS_Click(object sender, EventArgs e)
	{
		try
		{
			CreateSMSTemplate createSMSTemplate = new CreateSMSTemplate(UserAccount);
			createSMSTemplate.ScreenName = base.Name;
			createSMSTemplate.Show();
			createSMSTemplate.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineCity_Click(object sender, EventArgs e)
	{
		try
		{
			DefineCity defineCity = new DefineCity(UserAccount);
			defineCity.ShowDialog();
			defineCity.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDaysCalculate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDaysCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCommrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtBrokeryRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void txtMoisture_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			int id = Conversion.ToInt(r.Cells["Id"].Value);
			if (r == null || (int)r.RowType != 0 || !e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			int CountIds = (from row in table.AsEnumerable()
				where row.Field<int>("Id") > 0
				select row).Count();
			if (grd.CurrentColumn == null || !(grd.CurrentColumn.Key == "Delete"))
			{
				return;
			}
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
			}
			else if (id > 0)
			{
				if (CountIds > 1)
				{
					OrderDetailRemoveIds = OrderDetailRemoveIds + "," + id;
					r.Delete();
				}
				else
				{
					MessageBox.Show("All rows Can not be Deleted in Update Mode");
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
			}
			PartyDisableEnableOnLabAnalysis();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int Id = int.Parse(r.Cells["Id"].Value.ToString());
			if (grdhistory.CurrentColumn.Key == "Edit")
			{
				Reset();
				ReadById(Id);
			}
			if (grdhistory.CurrentColumn.Key == "SaveAs")
			{
				Reset();
				ReadById(Id);
				btnsave.Visible = false;
				btnUpdate.Visible = false;
				btnSaveAs.Visible = true;
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				((Control)(object)CmbStatus).Enabled = btnUpdate.Visible;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
			}
			if (grdhistory.CurrentColumn.Key == "Print")
			{
				CommonServices.PurchaseOrderSlipReport203(Id);
			}
			if (grdhistory.CurrentColumn.Key == "Print-A")
			{
				CommonServices.SupplierDispatchSubReport_203A(Id);
			}
			if (grdhistory.CurrentColumn.Key == "PrintII")
			{
				CommonServices.PurchaseOrderSlipReport203_01(Id);
			}
			if (grdhistory.CurrentColumn.Key == "PrintIII")
			{
				CommonServices.PurchaseOrderSlipReport203_02(Id);
			}
			if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 41);
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "To change Grid Tab Control,");
			dt.Rows.Add("", "When On Tab 'Purchase Order Detail',it will move you on tab 'Empty Bags'");
			dt.Rows.Add("", "When On Tab 'Empty Bags',it will move you on tab 'Account Credit and charge to Product'");
			dt.Rows.Add("", "When On Tab 'Account Credit and charge to Product',it will move you on tab 'Lab deduction Standard'");
			dt.Rows.Add("", "and When On Tab 'Lab deduction Standard',it will move you on tab 'Purchase Order Detail', ");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
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
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnshow_Click(null, null);
				}
			}
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 0)
			{
				btnUpdate_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				if (tabControl1.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnprint.Visible && btnprint.Enabled && e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0)
			{
				btnprint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.PurchaseOrderSlipReport203(RecId);
			}
			if (e.Control && e.KeyCode == Keys.Delete && tabControl2.SelectedIndex == 2)
			{
				GridEXRow item = grdExpensesChargeToProduct.CurrentRow;
				dtChargeToProduct.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtChargeToProduct.Rows.Count == 0)
				{
					AddRowChargeToProductGrid();
				}
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.D && tabControl2.SelectedIndex == 2)
			{
				AddRowChargeToProductGrid();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdhistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					combitem.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Left)
			{
				tabControl2.SelectedIndex = 0;
				combitem.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdEmptyBags).Focus();
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
					((Control)(object)grdInvExp).Focus();
				}
				else if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 3;
					((Control)(object)grdExpensesChargeToProduct).Focus();
				}
				else if (tabControl2.SelectedIndex == 3)
				{
					tabControl2.SelectedIndex = 4;
					((Control)(object)grdlab).Focus();
				}
				else if (tabControl2.SelectedIndex == 4)
				{
					tabControl2.SelectedIndex = 0;
					combitem.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExpensesChargeToProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExpensesChargeToProduct.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdExpensesChargeToProduct.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtChargeToProduct.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtChargeToProduct.Rows.Count == 0)
					{
						AddRowChargeToProductGrid();
					}
				}
				else if (grdExpensesChargeToProduct.CurrentColumn.Key == "Add")
				{
					AddRowChargeToProductGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtChargeToProduct.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtChargeToProduct.Rows.Count == 0)
				{
					AddRowChargeToProductGrid();
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowChargeToProductGrid();
			}
			if (e.KeyCode == Keys.F1 && grdExpensesChargeToProduct.CurrentColumn != null && grdExpensesChargeToProduct.CurrentColumn.Key == "AccountId")
			{
				if (SubsidiaryAccountAllownOnVouchers)
				{
					GrdPopUp obj = new GrdPopUp(dtAccounts, "SupplierCustomerId", "AccountTitle");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					r.Cells["AccountId"].Value = obj.ReturnId;
				}
				else
				{
					GrdPopUp obj2 = new GrdPopUp(dtAccounts, "Id", "AccountTitle");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					r.Cells["AccountId"].Value = obj2.ReturnId;
				}
				SupCustIdUpdateforFreightGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void rdFreightCash_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grd")
			{
				CmbCategory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadOrder_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)combordercat).ActiveRow == null || Conversion.ToInt(combordercat.Value) == 0)
			{
				MessageBox.Show("Please Select Category First...");
				return;
			}
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && Conversion.ToInt(item.Cells["flagForSupplierDispatch"].Value) == 0)
				{
					MessageBox.Show("You cannot load Supplier Dispatch because a record exists without Supplier Dispatch in the grid");
					return;
				}
			}
			int ParentId = 0;
			if (Conversion.ToInt(combordercat.Value) == 4)
			{
				ParentId = 1;
			}
			if (Conversion.ToInt(combordercat.Value) == 5)
			{
				ParentId = 2;
			}
			if (Conversion.ToInt(combordercat.Value) == 6)
			{
				ParentId = 4;
			}
			LoadTransactions = new LoadPendingSupplierDispatchForPO(UserAccount);
			LoadTransactions.cmbParentCategory.Value = ParentId;
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadInGridDetail();
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
			DataTable dtRecords = LoadTransactions.dtLoader;
			if (dtRecords.Rows.Count <= 0)
			{
				return;
			}
			int PreviousSupplierId = Conversion.ToInt(combsuppname.Value);
			string PreviousDeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			int SupplierId = Conversion.ToInt(dtRecords.Rows[0]["SupplierId"]);
			string DeliveryTerm = Conversion.ToString(dtRecords.Rows[0]["DeliveryTerm"]);
			if (grd.RowCount > 0 && (PreviousSupplierId > 0 || string.IsNullOrEmpty(PreviousDeliveryTerm)))
			{
				if (PreviousSupplierId != SupplierId)
				{
					throw new Exception("Data Against Another Supplier Exists...");
				}
				if (PreviousDeliveryTerm != DeliveryTerm)
				{
					throw new Exception("Data Against Another DeliveryTerm Exists...");
				}
			}
			combsuppname.Value = Conversion.ToInt(dtRecords.Rows[0]["SupplierId"]);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString(dtRecords.Rows[0]["DeliveryTerm"]);
			txtdeliverydays.Text = Conversion.ToString((object)dtRecords.Rows[0]["TransitDays"].ToString());
			combsalesman.Value = Conversion.ToInt((object)dtRecords.Rows[0]["CommissionAgentId"].ToString());
			((Control)(object)combcommtype).Text = Conversion.ToString((object)dtRecords.Rows[0]["CommType"].ToString());
			txtCommrate.Text = Conversion.ToString((object)dtRecords.Rows[0]["CommRate"].ToString());
			((Control)(object)combruom).Text = Conversion.ToString((object)dtRecords.Rows[0]["ComUom"].ToString());
			txtcommamount.Text = Conversion.ToString((object)dtRecords.Rows[0]["CommAmount"].ToString());
			CmbBrokeryAccount.Value = Conversion.ToInt((object)dtRecords.Rows[0]["BrokerId"].ToString());
			((Control)(object)CmbBrokeryType).Text = Conversion.ToString((object)dtRecords.Rows[0]["BrokeryType"].ToString());
			txtBrokeryRate.Text = Conversion.ToString((object)dtRecords.Rows[0]["BrokeryRate"].ToString());
			((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString((object)dtRecords.Rows[0]["BrokeryUom"].ToString());
			txtBrokeryAmount.Text = Conversion.ToString((object)dtRecords.Rows[0]["BrokeryAmount"].ToString());
			((Control)(object)combsuppname).Enabled = false;
			((Control)(object)combdeliverytrm).Enabled = false;
			((Control)(object)combordercat).Enabled = false;
			bool flag = false;
			bool flagForLoading = false;
			bool recordsAlreadyPresent = false;
			HashSet<Criteria> existingIds = new HashSet<Criteria>();
			if (grd.GetRows().Count() > 0)
			{
				recordsAlreadyPresent = true;
			}
			if (recordsAlreadyPresent)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					Criteria detailGridCriteria = new Criteria();
					detailGridCriteria.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
					detailGridCriteria.PackSizeId = Conversion.ToInt(item.Cells["ItemPackUOMID"].Value);
					detailGridCriteria.RateUomId = Conversion.ToInt(item.Cells["RateUOMID"].Value);
					detailGridCriteria.CropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
					detailGridCriteria.Rate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					existingIds.Add(detailGridCriteria);
				}
			}
			foreach (DataRow r in dtRecords.Rows)
			{
				if (grdSupplierLoadingDetail.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdSupplierLoadingDetail.GetRows();
					for (int i = 0; i < rows.Length; i++)
					{
						if (Conversion.ToInt(rows[i].Cells["LoadingDetailId"].Value) == Conversion.ToInt(r["DetailId"]))
						{
							flagForLoading = true;
							break;
						}
					}
				}
				if (!flagForLoading)
				{
					dtSupplierLoadingDetail.Rows.Add(0, r["Id"], r["DetailId"], r["DocNo"], r["ItemId"], r["ItemName"], r["CropYearId"], r["CropYear"], r["PackSizeId"], r["PackUomCode"], Conversion.ToDouble(r["PackUomEquivalent"]), r["PackingTypeId"], r["PackingType"], r["Qty"], Conversion.ToDouble(r["Qty"]) * Conversion.ToDouble(r["PackUomEquivalent"]), r["RateUomId"], r["RateUomCode"], Conversion.ToDouble(r["RateUomEquivalent"]), r["Rate"], (Conversion.ToDouble(r["Qty"]) * Conversion.ToDouble(r["PackUomEquivalent"]) > 0.0 && Conversion.ToDouble(r["RateUomEquivalent"]) > 0.0) ? (Conversion.ToDouble(r["Qty"]) * Conversion.ToDouble(r["PackUomEquivalent"]) / Conversion.ToDouble(r["RateUomEquivalent"]) * Conversion.ToDouble(r["Rate"])) : 0.0);
				}
				Criteria c = new Criteria();
				c.ItemId = Conversion.ToInt(r["ItemId"]);
				c.PackSizeId = Conversion.ToInt(r["PackSizeId"]);
				c.RateUomId = Conversion.ToInt(r["RateUomId"]);
				c.CropYearId = Conversion.ToInt(r["CropYearId"]);
				c.Rate = Conversion.ToDouble(r["Rate"]);
				string filter = "ItemId = '" + Conversion.ToString((object)c.ItemId) + "' AND CropYearId = '" + Conversion.ToString((object)c.CropYearId) + "'AND RateUomId = '" + Conversion.ToString((object)c.RateUomId) + "' AND PackSizeId = '" + Conversion.ToString((object)c.PackSizeId) + "'AND Rate = '" + Conversion.ToString((object)c.Rate) + "'";
				double qty = Conversion.ToDouble(dtRecords.Compute("Sum(Qty)", filter));
				if (existingIds.Contains(c))
				{
					flag = true;
				}
				if (!flag && !flagForLoading)
				{
					table.Rows.Add(0, false, true, r["ItemCode"], r["ItemId"], r["ItemName"], r["CropYearId"], r["CropYear"], r["PackSizeId"], r["PackUomCode"], Conversion.ToDouble(r["PackUomEquivalent"]), qty, qty * Conversion.ToDouble(r["PackUomEquivalent"]), r["RateUomId"], r["RateUomCode"], Conversion.ToDouble(r["RateUomEquivalent"]), r["Rate"], (qty * Conversion.ToDouble(r["PackUomEquivalent"]) > 0.0 && Conversion.ToDouble(r["RateUomEquivalent"]) > 0.0) ? (qty * Conversion.ToDouble(r["PackUomEquivalent"]) / Conversion.ToDouble(r["RateUomEquivalent"]) * Conversion.ToDouble(r["Rate"])) : 0.0, combjob.Value, ((Control)(object)combjob).Text, r["CityId"], r["CityName"], 0, 0, "", r["Remarks"], 0);
				}
				if (recordsAlreadyPresent && flag)
				{
					filter = "ItemId = '" + Conversion.ToString((object)c.ItemId) + "' AND CropYearId = '" + Conversion.ToString((object)c.CropYearId) + "'AND RateUOMID = '" + Conversion.ToString((object)c.RateUomId) + "' AND ItemPackUOMID = '" + Conversion.ToString((object)c.PackSizeId) + "'AND Rate = '" + Conversion.ToString((object)c.Rate) + "'";
					DataRow[] array = table.Select(filter);
					foreach (DataRow row2 in array)
					{
						if (!Conversion.ToBool(row2["rowUpdatedByLoadingMore"]))
						{
							double NewQty = Conversion.ToDouble(row2["ItemQTY"]) + qty;
							double NewWeight = NewQty * Conversion.ToDouble(row2["PackEquivalent"]);
							double NewAmount = NewQty * Conversion.ToDouble(row2["PackEquivalent"]) / Conversion.ToDouble(row2["RateEquivalent"]) * Conversion.ToDouble(row2["ItemRate"]);
							row2.SetField("rowUpdatedByLoadingMore", value: true);
							row2.SetField("ItemQTY", NewQty);
							row2.SetField("Weight", NewWeight);
							row2.SetField("Amount", NewAmount);
						}
					}
				}
				existingIds.Add(c);
				flag = false;
				flagForLoading = false;
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			grdSupplierLoadingDetail.DataSource = dtSupplierLoadingDetail;
			grdSupplierLoadingDetail.RetrieveStructure();
			grdSettingForSupplierLoading();
			if (grdSupplierLoadingDetail.GetRows().Count() > 0)
			{
				((Control)(object)grdSupplierLoadingDetail).Visible = true;
			}
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingForSupplierLoading()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSupplierLoadingDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdSupplierLoadingDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdSupplierLoadingDetail.RootTable.Columns["Id"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["ItemId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["LoadingId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["LoadingDetailId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdSupplierLoadingDetail.RootTable.Columns["PreBillNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdSupplierLoadingDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 60;
			grdSupplierLoadingDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdSupplierLoadingDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdSupplierLoadingDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdSupplierLoadingDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSupplierLoadingDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdSupplierLoadingDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdSupplierLoadingDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["LoadingId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["LoadingDetailId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["PreBillNo"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["PackEquivalent"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grdSupplierLoadingDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdSupplierLoadingDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdSupplierLoadingDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdSupplierLoadingDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdSupplierLoadingDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdSupplierLoadingDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdSupplierLoadingDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdSupplierLoadingDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdSupplierLoadingDetail.RootTable.Columns.Add("Delete");
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].Caption = "X";
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].Image = Resources.icons8_delete_bin_16;
			grdSupplierLoadingDetail.RootTable.Columns["Delete"].Width = 35;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSupplierLoadingDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdSupplierLoadingDetail.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					int LoadingId = Conversion.ToInt(item.Cells["LoadingId"].Value);
					foreach (DataRow row2 in (from row in dtSupplierLoadingDetail.AsEnumerable()
						where row.Field<int>("LoadingId") == LoadingId
						select row).ToList())
					{
						GridEXRow[] rows = grd.GetRows();
						foreach (GridEXRow detailRow in rows)
						{
							int DetailItemId = Conversion.ToInt(detailRow.Cells["ItemId"].Value);
							int DetailPackSizeId = Conversion.ToInt(detailRow.Cells["ItemPackUOMID"].Value);
							int DetailRateUomId = Conversion.ToInt(detailRow.Cells["RateUOMID"].Value);
							int DetailCropYearId = Conversion.ToInt(detailRow.Cells["CropYearId"].Value);
							double DetailRate = Conversion.ToDouble(detailRow.Cells["ItemRate"].Value);
							int SdItemId = Conversion.ToInt(row2["ItemId"]);
							int SdPackSizeId = Conversion.ToInt(row2["ItemUOMId"]);
							int SdRateUomId = Conversion.ToInt(row2["RateUOMId"]);
							int SdCropYearId = Conversion.ToInt(row2["CropYearId"]);
							double SdRate = Conversion.ToDouble(row2["ItemRate"]);
							if (DetailItemId == SdItemId && DetailPackSizeId == SdPackSizeId && DetailRateUomId == SdRateUomId && DetailCropYearId == SdCropYearId && DetailRate == SdRate)
							{
								double SDqty = Conversion.ToDouble(row2["ItemQty"]);
								double detailQty = Conversion.ToDouble(detailRow.Cells["ItemQTY"].Value);
								double PackEquivalent = Conversion.ToDouble(detailRow.Cells["PackEquivalent"].Value);
								double RateEquivalent = Conversion.ToDouble(detailRow.Cells["RateEquivalent"].Value);
								double Rate = Conversion.ToDouble(detailRow.Cells["ItemRate"].Value);
								detailRow.BeginEdit();
								double NewQty = detailQty - SDqty;
								detailRow.Cells["ItemQTY"].Value = NewQty;
								detailRow.Cells["Weight"].Value = NewQty * PackEquivalent;
								detailRow.Cells["Amount"].Value = NewQty * PackEquivalent / RateEquivalent * Rate;
								detailRow.EndEdit();
							}
						}
						row2.Delete();
					}
					foreach (DataRow item2 in (from r in table.AsEnumerable()
						where r.Field<double>("ItemQTY") == 0.0
						select r).ToList())
					{
						item2.Delete();
					}
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
			}
			if (grdSupplierLoadingDetail.RowCount == 0)
			{
				((Control)(object)combsuppname).Enabled = true;
			}
			((Control)(object)combordercat).Enabled = true;
			((Control)(object)combdeliverytrm).Enabled = true;
			grdSupplierLoadingDetail.UpdateData();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSupplierLoadingDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "ItemQty")
			{
				GridEXRow item = grdSupplierLoadingDetail.CurrentRow;
				Conversion.ToDouble(item.Cells["ItemQty"].Value);
				grdSupplierLoadingDetail.UpdateData();
				if (item != null && (int)item.RowType == 0)
				{
					double SDqty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
					if (SDqty > 0.0)
					{
						int SdItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
						int SdPackSizeId = Conversion.ToInt(item.Cells["ItemUOMId"].Value);
						int SdRateUomId = Conversion.ToInt(item.Cells["RateUOMId"].Value);
						int SdCropYearId = Conversion.ToInt(item.Cells["CropYearId"].Value);
						double SdRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
						double SdPackEquivalent = Conversion.ToDouble(item.Cells["PackEquivalent"].Value);
						double SdRateEquivalent = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
						string filter = "";
						if (btnsave.Visible && btnsave.Enabled)
						{
							filter = "ItemId = '" + Conversion.ToString((object)SdItemId) + "' AND CropYearId = '" + Conversion.ToString((object)SdCropYearId) + "'AND RateUOMID = '" + Conversion.ToString((object)SdRateUomId) + "' AND ItemPackUOMID = '" + Conversion.ToString((object)SdPackSizeId) + "'AND ItemRate = '" + Conversion.ToString((object)SdRate) + "'";
						}
						else
						{
							string sdCropYear = Conversion.ToString(item.Cells["CropYear"].Value);
							filter = "ItemId = '" + Conversion.ToString((object)SdItemId) + "' AND CropYear = '" + Conversion.ToString((object)sdCropYear) + "'AND RateUOMID = '" + Conversion.ToString((object)SdRateUomId) + "' AND ItemPackUOMID = '" + Conversion.ToString((object)SdPackSizeId) + "'AND ItemRate = '" + Conversion.ToString((object)SdRate) + "'";
						}
						DataRow[] array = table.Select(filter);
						foreach (DataRow row2 in array)
						{
							double NewQty = Conversion.ToDouble(row2["ItemQTY"]) - (PreviousQty - SDqty);
							double NewWeight = NewQty * Conversion.ToDouble(row2["PackEquivalent"]);
							double NewAmount = NewQty * Conversion.ToDouble(row2["PackEquivalent"]) / Conversion.ToDouble(row2["RateEquivalent"]) * Conversion.ToDouble(row2["ItemRate"]);
							row2.SetField("ItemQTY", NewQty);
							row2.SetField("Weight", NewWeight);
							row2.SetField("Amount", NewAmount);
						}
						item.BeginEdit();
						item.Cells["ItemQty"].Value = SDqty;
						item.Cells["NetWeight"].Value = SDqty * SdPackEquivalent;
						item.Cells["Amount"].Value = SDqty * SdPackEquivalent / SdRateEquivalent * SdRate;
						item.EndEdit();
					}
				}
			}
			grdSupplierLoadingDetail.UpdateData();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSupplierLoadingDetail_EditingCell(object sender, EditingCellEventArgs e)
	{
		PreviousQty = Conversion.ToInt(e.Value);
	}

	private void btnDefineBookingPerson_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineReferenceParties(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBookingPerson_InitializeLayout(object sender, InitializeLayoutEventArgs e)
	{
	}

	private void label33_Click(object sender, EventArgs e)
	{
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
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Expected O, but got Unknown
		//IL_0a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Expected O, but got Unknown
		//IL_0be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf0: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cab: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Expected O, but got Unknown
		//IL_0e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8f: Expected O, but got Unknown
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9a: Expected O, but got Unknown
		//IL_0e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea5: Expected O, but got Unknown
		//IL_0ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb0: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Expected O, but got Unknown
		//IL_0ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec6: Expected O, but got Unknown
		//IL_0f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f76: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cb: Expected O, but got Unknown
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Expected O, but got Unknown
		//IL_10f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Expected O, but got Unknown
		//IL_1171: Unknown result type (might be due to invalid IL or missing references)
		//IL_117b: Expected O, but got Unknown
		//IL_1242: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Expected O, but got Unknown
		//IL_161f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1629: Expected O, but got Unknown
		//IL_1636: Unknown result type (might be due to invalid IL or missing references)
		//IL_1640: Expected O, but got Unknown
		//IL_505e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5068: Expected O, but got Unknown
		//IL_51df: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e9: Expected O, but got Unknown
		//IL_51f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5200: Expected O, but got Unknown
		//IL_520d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5217: Expected O, but got Unknown
		//IL_5372: Unknown result type (might be due to invalid IL or missing references)
		//IL_537c: Expected O, but got Unknown
		//IL_5389: Unknown result type (might be due to invalid IL or missing references)
		//IL_5393: Expected O, but got Unknown
		//IL_5664: Unknown result type (might be due to invalid IL or missing references)
		//IL_566e: Expected O, but got Unknown
		//IL_567b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5685: Expected O, but got Unknown
		//IL_6207: Unknown result type (might be due to invalid IL or missing references)
		//IL_6211: Expected O, but got Unknown
		//IL_d28e: Unknown result type (might be due to invalid IL or missing references)
		//IL_d298: Expected O, but got Unknown
		//IL_d2bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_d2c6: Expected O, but got Unknown
		//IL_f5f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_f5fb: Expected O, but got Unknown
		//IL_f608: Unknown result type (might be due to invalid IL or missing references)
		//IL_f612: Expected O, but got Unknown
		//IL_f61f: Unknown result type (might be due to invalid IL or missing references)
		//IL_f629: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExpensesChargeToProduct_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdlab_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
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
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.PurchsaeOrder));
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		GridEXLayout grdSupplierLoadingDetail_DesignTimeLayout = new GridEXLayout();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.PurchaseOrderDetail = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.radFactorySample = new System.Windows.Forms.RadioButton();
		this.radFactoryStandard = new System.Windows.Forms.RadioButton();
		this.CmbCategory = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.panel5 = new System.Windows.Forms.Panel();
		this.RadCategory = new System.Windows.Forms.RadioButton();
		this.RadType = new System.Windows.Forms.RadioButton();
		this.CmbLabSampleNo = new UltraCombo();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.combrateuom = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label37 = new System.Windows.Forms.Label();
		this.combcityarea = new UltraCombo();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.CmbCropyr = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.combjob = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.label35 = new System.Windows.Forms.Label();
		this.combitem = new UltraCombo();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.combitempck = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.EmptyBags = new System.Windows.Forms.TabPage();
		this.grdEmptyBags = new GridEX();
		this.SupplierExpense = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.ExpensesChargeToProduct = new System.Windows.Forms.TabPage();
		this.grdExpensesChargeToProduct = new GridEX();
		this.LabDeductionStandard = new System.Windows.Forms.TabPage();
		this.grdlab = new GridEX();
		this.tabPaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnDefineBookingPerson = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbBookingPerson = new UltraCombo();
		this.txtDocNoShow = new System.Windows.Forms.TextBox();
		this.combordercat = new UltraCombo();
		this.combsuppname = new UltraCombo();
		this.RdPartyByCode = new System.Windows.Forms.RadioButton();
		this.RdPartyByName = new System.Windows.Forms.RadioButton();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtOrderAmount = new System.Windows.Forms.TextBox();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.GroupCommission = new System.Windows.Forms.GroupBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.txtCommrate = new System.Windows.Forms.TextBox();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtPpBagCut = new System.Windows.Forms.TextBox();
		this.label61 = new System.Windows.Forms.Label();
		this.txtJuteBagCut = new System.Windows.Forms.TextBox();
		this.rdFreightCredit = new System.Windows.Forms.RadioButton();
		this.rdFreightCash = new System.Windows.Forms.RadioButton();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.CmbBrokeryType = new UltraCombo();
		this.combruom = new UltraCombo();
		this.combcommtype = new UltraCombo();
		this.CmbBrokeryAccount = new UltraCombo();
		this.combsalesman = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.ChkCommission = new System.Windows.Forms.CheckBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.ChkBoxPrint = new System.Windows.Forms.CheckBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkBoxPrintA = new System.Windows.Forms.CheckBox();
		this.labelLocationType = new System.Windows.Forms.Label();
		this.cmbLocationType = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrintIII = new System.Windows.Forms.CheckBox();
		this.chkPrintII = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintNew = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintII = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintWithLabSample = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDefineCity = new System.Windows.Forms.ToolStripButton();
		this.btnSMS = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadOrder = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.grdDetail = new GridEX();
		this.label59 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbBookingPersonHistory = new UltraCombo();
		this.cmbBranchName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.cmbSupplierNameHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label42 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdSupplierLoadingDetail = new GridEX();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.PurchaseOrderDetail.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLabSampleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		this.EmptyBags.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.SupplierExpense.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.ExpensesChargeToProduct.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExpensesChargeToProduct).BeginInit();
		this.LabDeductionStandard.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlab).BeginInit();
		this.tabPaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combordercat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		this.GroupCommission.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combruom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPersonHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).BeginInit();
		this.panel14.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSupplierLoadingDetail).BeginInit();
		this.tabControl1.SuspendLayout();
		base.SuspendLayout();
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 482);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1419, 122);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 457);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1419, 25);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1380, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabControl2.Controls.Add(this.PurchaseOrderDetail);
		this.tabControl2.Controls.Add(this.EmptyBags);
		this.tabControl2.Controls.Add(this.SupplierExpense);
		this.tabControl2.Controls.Add(this.ExpensesChargeToProduct);
		this.tabControl2.Controls.Add(this.LabDeductionStandard);
		this.tabControl2.Controls.Add(this.tabPaymentDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 294);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1419, 163);
		this.tabControl2.TabIndex = 1;
		this.PurchaseOrderDetail.Controls.Add(this.panel16);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.PurchaseOrderDetail.Controls.Add(this.btnplus);
		this.PurchaseOrderDetail.Controls.Add(this.panel5);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabSampleNo);
		this.PurchaseOrderDetail.Controls.Add(this.txtMoisture);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combrateuom);
		this.PurchaseOrderDetail.Controls.Add(this.label34);
		this.PurchaseOrderDetail.Controls.Add(this.label18);
		this.PurchaseOrderDetail.Controls.Add(this.btnCancelUpdateDetial);
		this.PurchaseOrderDetail.Controls.Add(this.btnUpdateDetail);
		this.PurchaseOrderDetail.Controls.Add(this.label37);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combcityarea);
		this.PurchaseOrderDetail.Controls.Add(this.txtremarksdetail);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.PurchaseOrderDetail.Controls.Add(this.label47);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combjob);
		this.PurchaseOrderDetail.Controls.Add(this.label46);
		this.PurchaseOrderDetail.Controls.Add(this.rdSearchByCode);
		this.PurchaseOrderDetail.Controls.Add(this.label35);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.PurchaseOrderDetail.Controls.Add(this.rdSearchByName);
		this.PurchaseOrderDetail.Controls.Add(this.txtamount);
		this.PurchaseOrderDetail.Controls.Add(this.label45);
		this.PurchaseOrderDetail.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.PurchaseOrderDetail.Controls.Add(this.label36);
		this.PurchaseOrderDetail.Controls.Add(this.txtRate);
		this.PurchaseOrderDetail.Controls.Add(this.label44);
		this.PurchaseOrderDetail.Controls.Add(this.label3);
		this.PurchaseOrderDetail.Controls.Add(this.txtweight);
		this.PurchaseOrderDetail.Controls.Add(this.label40);
		this.PurchaseOrderDetail.Controls.Add(this.label41);
		this.PurchaseOrderDetail.Controls.Add(this.label38);
		this.PurchaseOrderDetail.Controls.Add(this.txtqty);
		this.PurchaseOrderDetail.Controls.Add(this.label39);
		this.PurchaseOrderDetail.Location = new System.Drawing.Point(4, 25);
		this.PurchaseOrderDetail.Name = "PurchaseOrderDetail";
		this.PurchaseOrderDetail.Padding = new System.Windows.Forms.Padding(3);
		this.PurchaseOrderDetail.Size = new System.Drawing.Size(1411, 134);
		this.PurchaseOrderDetail.TabIndex = 0;
		this.PurchaseOrderDetail.Text = "Purchase Order Detail";
		this.PurchaseOrderDetail.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add(this.radFactorySample);
		this.panel16.Controls.Add(this.radFactoryStandard);
		this.panel16.Location = new System.Drawing.Point(503, 65);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(149, 23);
		this.panel16.TabIndex = 42;
		this.radFactorySample.AutoSize = true;
		this.radFactorySample.Checked = true;
		this.radFactorySample.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radFactorySample.Location = new System.Drawing.Point(5, 4);
		this.radFactorySample.Name = "radFactorySample";
		this.radFactorySample.Size = new System.Drawing.Size(63, 17);
		this.radFactorySample.TabIndex = 40;
		this.radFactorySample.TabStop = true;
		this.radFactorySample.Text = "Sample";
		this.radFactorySample.UseVisualStyleBackColor = true;
		this.radFactorySample.CheckedChanged += new System.EventHandler(radFactorySample_CheckedChanged);
		this.radFactoryStandard.AutoSize = true;
		this.radFactoryStandard.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radFactoryStandard.Location = new System.Drawing.Point(71, 4);
		this.radFactoryStandard.Name = "radFactoryStandard";
		this.radFactoryStandard.Size = new System.Drawing.Size(72, 17);
		this.radFactoryStandard.TabIndex = 40;
		this.radFactoryStandard.Text = "Standard";
		this.radFactoryStandard.UseVisualStyleBackColor = true;
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance279).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(8, 39);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1029, 86);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(27, 26);
		this.btnplus.TabIndex = 15;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.panel5.Controls.Add(this.RadCategory);
		this.panel5.Controls.Add(this.RadType);
		this.panel5.Location = new System.Drawing.Point(39, 15);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(136, 21);
		this.panel5.TabIndex = 39;
		this.RadCategory.AutoSize = true;
		this.RadCategory.Checked = true;
		this.RadCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadCategory.Location = new System.Drawing.Point(4, 1);
		this.RadCategory.Name = "RadCategory";
		this.RadCategory.Size = new System.Drawing.Size(72, 19);
		this.RadCategory.TabIndex = 35;
		this.RadCategory.TabStop = true;
		this.RadCategory.Text = "Category";
		this.RadCategory.UseVisualStyleBackColor = true;
		this.RadCategory.CheckedChanged += new System.EventHandler(RadCategory_CheckedChanged);
		this.RadType.AutoSize = true;
		this.RadType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadType.Location = new System.Drawing.Point(80, 1);
		this.RadType.Name = "RadType";
		this.RadType.Size = new System.Drawing.Size(50, 19);
		this.RadType.TabIndex = 36;
		this.RadType.Text = "Type";
		this.RadType.UseVisualStyleBackColor = true;
		this.CmbLabSampleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLabSampleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabSampleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabSampleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabSampleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabSampleNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbLabSampleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabSampleNo).Location = new System.Drawing.Point(456, 86);
		((System.Windows.Forms.Control)(object)this.CmbLabSampleNo).Name = "CmbLabSampleNo";
		((System.Windows.Forms.Control)(object)this.CmbLabSampleNo).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabSampleNo).TabIndex = 13;
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtMoisture.Location = new System.Drawing.Point(387, 88);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(67, 23);
		this.txtMoisture.TabIndex = 12;
		this.txtMoisture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMoisture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMoisture_KeyPress);
		this.combrateuom.AllowNull = (DefaultableBoolean)1;
		this.combrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrateuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrateuom).Location = new System.Drawing.Point(919, 38);
		((System.Windows.Forms.Control)(object)this.combrateuom).Name = "combrateuom";
		((System.Windows.Forms.Control)(object)this.combrateuom).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.combrateuom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combrateuom).TextChanged += new System.EventHandler(combrateuom_SelectedIndexChanged);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(385, 67);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(67, 15);
		this.label34.TabIndex = 34;
		this.label34.Text = "Moisture %";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(8, 20);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(32, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Item";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1095, 86);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(61, 26);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1029, 86);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(64, 26);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(180, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(35, 15);
		this.label37.TabIndex = 0;
		this.label37.Text = "Item ";
		this.combcityarea.AllowNull = (DefaultableBoolean)1;
		this.combcityarea.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcityarea.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcityarea.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcityarea).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.combcityarea).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance240).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcityarea).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcityarea).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcityarea.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcityarea).Location = new System.Drawing.Point(180, 86);
		((System.Windows.Forms.Control)(object)this.combcityarea).Name = "combcityarea";
		((System.Windows.Forms.Control)(object)this.combcityarea).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.combcityarea).TabIndex = 11;
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(655, 88);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(372, 23);
		this.txtremarksdetail.TabIndex = 14;
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance248;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance253).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(387, 39);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(138, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 3;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(655, 67);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.combjob.AllowNull = (DefaultableBoolean)1;
		this.combjob.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjob.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjob.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjob).DisplayLayout.Appearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.combjob).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjob).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance261;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance262).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance262).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.combjob).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjob).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance266).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance269).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjob).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjob).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjob).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combjob.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjob).Location = new System.Drawing.Point(8, 85);
		((System.Windows.Forms.Control)(object)this.combjob).Name = "combjob";
		((System.Windows.Forms.Control)(object)this.combjob).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.combjob).TabIndex = 10;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(461, 69);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(45, 13);
		this.label46.TabIndex = 25;
		this.label46.Text = "Factory";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(284, 18);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 36;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(180, 67);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(62, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "City Name";
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance274;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance280).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(180, 39);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(205, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 2;
		this.combitem.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave_1);
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(221, 18);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 35;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Enabled = false;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(1046, 39);
		this.txtamount.Name = "txtamount";
		this.txtamount.Size = new System.Drawing.Size(107, 23);
		this.txtamount.TabIndex = 9;
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtamount.TextChanged += new System.EventHandler(txtamount_TextChanged);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(8, 67);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Job/Lot";
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance287;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance293).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(527, 39);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combitempck).Leave += new System.EventHandler(combitempck_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(527, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(35, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "UOM";
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(835, 42);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(82, 23);
		this.txtRate.TabIndex = 7;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtRate.Leave += new System.EventHandler(txtRate_Leave);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(1049, 17);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(387, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Crop Year";
		this.txtweight.AccessibleName = "ItemWeight";
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(738, 41);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(96, 23);
		this.txtweight.TabIndex = 6;
		this.txtweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(838, 20);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(921, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(655, 19);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.txtqty.AccessibleName = "ItemQty";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(655, 41);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(82, 23);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(742, 19);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.EmptyBags.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.EmptyBags.Location = new System.Drawing.Point(4, 25);
		this.EmptyBags.Name = "EmptyBags";
		this.EmptyBags.Padding = new System.Windows.Forms.Padding(3);
		this.EmptyBags.Size = new System.Drawing.Size(1411, 134);
		this.EmptyBags.TabIndex = 1;
		this.EmptyBags.Text = "Packing Material(Empty Bags)";
		this.EmptyBags.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(1405, 128);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 2;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		this.SupplierExpense.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.SupplierExpense.Location = new System.Drawing.Point(4, 25);
		this.SupplierExpense.Name = "SupplierExpense";
		this.SupplierExpense.Size = new System.Drawing.Size(1411, 134);
		this.SupplierExpense.TabIndex = 4;
		this.SupplierExpense.Text = "Supplier Expense (Credit To Supplier & Debit To Product)";
		this.SupplierExpense.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1411, 134);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Tag = "GrdExpense";
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.ExpensesChargeToProduct.Controls.Add((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct);
		this.ExpensesChargeToProduct.Location = new System.Drawing.Point(4, 25);
		this.ExpensesChargeToProduct.Name = "ExpensesChargeToProduct";
		this.ExpensesChargeToProduct.Size = new System.Drawing.Size(1411, 134);
		this.ExpensesChargeToProduct.TabIndex = 2;
		this.ExpensesChargeToProduct.Text = "Account Credit & Charge to Product";
		this.ExpensesChargeToProduct.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdExpensesChargeToProduct).BackColor = System.Drawing.Color.White;
		grdExpensesChargeToProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExpensesChargeToProduct.DesignTimeLayout = grdExpensesChargeToProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExpensesChargeToProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).Name = "grdExpensesChargeToProduct";
		((ScrollableBase)this.grdExpensesChargeToProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).Size = new System.Drawing.Size(1411, 134);
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).TabIndex = 2;
		this.grdExpensesChargeToProduct.TotalRow = (InheritableBoolean)1;
		this.grdExpensesChargeToProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExpensesChargeToProduct).VisualStyle = (VisualStyle)4;
		this.grdExpensesChargeToProduct.CellUpdated += new ColumnActionEventHandler(grdExpensesChargeToProduct_CellUpdated);
		this.grdExpensesChargeToProduct.ColumnButtonClick += new ColumnActionEventHandler(grdExpensesChargeToProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdExpensesChargeToProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdExpensesChargeToProduct_KeyDown);
		this.LabDeductionStandard.Controls.Add((System.Windows.Forms.Control)(object)this.grdlab);
		this.LabDeductionStandard.Location = new System.Drawing.Point(4, 25);
		this.LabDeductionStandard.Name = "LabDeductionStandard";
		this.LabDeductionStandard.Padding = new System.Windows.Forms.Padding(3);
		this.LabDeductionStandard.Size = new System.Drawing.Size(1411, 134);
		this.LabDeductionStandard.TabIndex = 3;
		this.LabDeductionStandard.Text = "Lab Deduction Standard";
		this.LabDeductionStandard.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdlab).BackColor = System.Drawing.Color.White;
		grdlab_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdlab.DesignTimeLayout = grdlab_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdlab).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdlab).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlab.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdlab).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdlab).Name = "grdlab";
		((ScrollableBase)this.grdlab).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlab).Size = new System.Drawing.Size(1405, 128);
		((System.Windows.Forms.Control)(object)this.grdlab).TabIndex = 3;
		this.grdlab.TotalRow = (InheritableBoolean)1;
		this.grdlab.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlab).VisualStyle = (VisualStyle)4;
		this.tabPaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentDetail);
		this.tabPaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentDetail.Name = "tabPaymentDetail";
		this.tabPaymentDetail.Size = new System.Drawing.Size(1411, 134);
		this.tabPaymentDetail.TabIndex = 5;
		this.tabPaymentDetail.Text = "Payment Detail";
		this.tabPaymentDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentDetail).BackColor = System.Drawing.Color.White;
		grdPaymentDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentDetail.DesignTimeLayout = grdPaymentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Name = "grdPaymentDetail";
		((ScrollableBase)this.grdPaymentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).Size = new System.Drawing.Size(1411, 134);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).TabIndex = 4;
		this.grdPaymentDetail.TotalRow = (InheritableBoolean)1;
		this.grdPaymentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentDetail).VisualStyle = (VisualStyle)4;
		this.grdPaymentDetail.CellUpdated += new ColumnActionEventHandler(grdPaymentDetail_CellUpdated);
		this.grdPaymentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentDetail_KeyDown);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.GroupCommission);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1419, 238);
		this.panel4.TabIndex = 0;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.btnDefineBookingPerson);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBookingPerson);
		this.groupBox1.Controls.Add(this.txtDocNoShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combordercat);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.groupBox1.Controls.Add(this.RdPartyByCode);
		this.groupBox1.Controls.Add(this.RdPartyByName);
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add(this.txtcatsr);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.txtOrderAmount);
		this.groupBox1.Controls.Add(this.txtOrderWeight);
		this.groupBox1.Controls.Add(this.txtOrderQty);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label58);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.label54);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(756, 229);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.btnDefineBookingPerson.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineBookingPerson.Location = new System.Drawing.Point(282, 151);
		this.btnDefineBookingPerson.Name = "btnDefineBookingPerson";
		this.btnDefineBookingPerson.Size = new System.Drawing.Size(23, 26);
		this.btnDefineBookingPerson.TabIndex = 8;
		this.btnDefineBookingPerson.TabStop = false;
		this.btnDefineBookingPerson.UseVisualStyleBackColor = true;
		this.btnDefineBookingPerson.Click += new System.EventHandler(btnDefineBookingPerson_Click);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(96, 47);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 588;
		this.label49.Text = "*";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(4, 183);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 587;
		this.label5.Text = "Remarks";
		this.CmbBookingPerson.AllowNull = (DefaultableBoolean)1;
		this.CmbBookingPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBookingPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBookingPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance300;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBookingPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBookingPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Location = new System.Drawing.Point(110, 152);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Name = "CmbBookingPerson";
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbBookingPerson).TabIndex = 7;
		this.CmbBookingPerson.InitializeLayout += new InitializeLayoutEventHandler(CmbBookingPerson_InitializeLayout);
		this.txtDocNoShow.BackColor = System.Drawing.Color.White;
		this.txtDocNoShow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoShow.Enabled = false;
		this.txtDocNoShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoShow.Location = new System.Drawing.Point(110, 18);
		this.txtDocNoShow.Name = "txtDocNoShow";
		this.txtDocNoShow.ReadOnly = true;
		this.txtDocNoShow.Size = new System.Drawing.Size(102, 23);
		this.txtDocNoShow.TabIndex = 0;
		this.txtDocNoShow.TabStop = false;
		this.combordercat.AllowNull = (DefaultableBoolean)1;
		this.combordercat.AutoCompleteMode = (AutoCompleteMode)4;
		this.combordercat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combordercat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combordercat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combordercat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combordercat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combordercat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combordercat).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combordercat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combordercat).Location = new System.Drawing.Point(110, 69);
		((System.Windows.Forms.Control)(object)this.combordercat).Name = "combordercat";
		((System.Windows.Forms.Control)(object)this.combordercat).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.combordercat).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combordercat).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combordercat).Leave += new System.EventHandler(combordercat_Leave);
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(110, 98);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.combsuppname).Leave += new System.EventHandler(combsuppname_Leave);
		this.RdPartyByCode.AutoSize = true;
		this.RdPartyByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByCode.Location = new System.Drawing.Point(53, 110);
		this.RdPartyByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByCode.Name = "RdPartyByCode";
		this.RdPartyByCode.Size = new System.Drawing.Size(52, 19);
		this.RdPartyByCode.TabIndex = 585;
		this.RdPartyByCode.Text = "Code";
		this.RdPartyByCode.UseVisualStyleBackColor = true;
		this.RdPartyByName.AutoSize = true;
		this.RdPartyByName.Checked = true;
		this.RdPartyByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByName.Location = new System.Drawing.Point(53, 94);
		this.RdPartyByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByName.Name = "RdPartyByName";
		this.RdPartyByName.Size = new System.Drawing.Size(57, 19);
		this.RdPartyByName.TabIndex = 584;
		this.RdPartyByName.TabStop = true;
		this.RdPartyByName.Text = "Name";
		this.RdPartyByName.UseVisualStyleBackColor = true;
		this.RdPartyByName.CheckedChanged += new System.EventHandler(RdPartyByName_CheckedChanged);
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(213, 18);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(92, 23);
		this.txtBranchSrNo.TabIndex = 582;
		this.txtBranchSrNo.TabStop = false;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(110, 180);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(451, 40);
		this.txtremarks.TabIndex = 8;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(402, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(14, 17);
		this.label2.TabIndex = 578;
		this.label2.Text = "*";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(622, 21);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 579;
		this.label51.Text = "*";
		this.label51.Visible = false;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(91, 74);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 576;
		this.label48.Text = "*";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(647, 16);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(107, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 15;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(565, 22);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(40, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Status";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(402, 103);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 581;
		this.label53.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(95, 21);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(14, 17);
		this.label27.TabIndex = 574;
		this.label27.Text = "*";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(402, 21);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 580;
		this.label52.Text = "*";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(647, 125);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 19;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(420, 16);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.combpttrm).Leave += new System.EventHandler(combpttrm_Leave);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(420, 98);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 12;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(309, 22);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Terms";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(420, 154);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(141, 23);
		this.txtdeliverydays.TabIndex = 14;
		this.txtdeliverydays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Enabled = false;
		this.txtcatsr.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcatsr.Location = new System.Drawing.Point(242, 71);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.Size = new System.Drawing.Size(63, 23);
		this.txtcatsr.TabIndex = 4;
		this.txtcatsr.TabStop = false;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(647, 179);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(107, 23);
		this.txtFcyAmount.TabIndex = 21;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmount.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtFcyAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(647, 154);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(107, 23);
		this.txtExchangeRate.TabIndex = 20;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtOrderAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderAmount.Enabled = false;
		this.txtOrderAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderAmount.Location = new System.Drawing.Point(647, 100);
		this.txtOrderAmount.Name = "txtOrderAmount";
		this.txtOrderAmount.Size = new System.Drawing.Size(107, 23);
		this.txtOrderAmount.TabIndex = 18;
		this.txtOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderAmount.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtOrderAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderWeight.Location = new System.Drawing.Point(647, 71);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.Size = new System.Drawing.Size(107, 23);
		this.txtOrderWeight.TabIndex = 17;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderWeight.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtOrderWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderQty.Location = new System.Drawing.Point(647, 44);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(107, 23);
		this.txtOrderQty.TabIndex = 16;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderQty.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtOrderQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(420, 44);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(141, 23);
		this.txtduedays.TabIndex = 10;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(4, 158);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(90, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Booking Person";
		this.label33.Click += new System.EventHandler(label33_Click);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(309, 131);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(106, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Start Date";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(4, 103);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(51, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Supplier";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(309, 48);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days";
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupprefno.Location = new System.Drawing.Point(110, 127);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(195, 23);
		this.txtsupprefno.TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(309, 75);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 3;
		this.label29.Text = "Due Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 75);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(85, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Catagory && No";
		this.duedate.CustomFormat = "dd-MMM-yyyy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(420, 71);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(141, 23);
		this.duedate.TabIndex = 11;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(4, 131);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(90, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Supplier Ref No";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(309, 104);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(565, 183);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(72, 15);
		this.label58.TabIndex = 3;
		this.label58.Text = "Fcy Amount";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(565, 158);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(84, 15);
		this.label57.TabIndex = 3;
		this.label57.Text = "Exchange Rate";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(565, 131);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(55, 15);
		this.label56.TabIndex = 3;
		this.label56.Text = "Fcy Code";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(565, 104);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(84, 15);
		this.label55.TabIndex = 3;
		this.label55.Text = "Order Amount";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(565, 75);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(79, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "Order Weight";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(565, 48);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(54, 15);
		this.label50.TabIndex = 3;
		this.label50.Text = "Item Qty";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(309, 158);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(79, 15);
		this.label32.TabIndex = 3;
		this.label32.Text = "Delivery Days";
		this.deliverystartdate.CustomFormat = "dd-MMM-yyyy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(420, 127);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(141, 23);
		this.deliverystartdate.TabIndex = 13;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(110, 44);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(195, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(4, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(80, 15);
		this.label9.TabIndex = 3;
		this.label9.Text = "Contract Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(91, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc && Branch #";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(111, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(85, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.GroupCommission.BackColor = System.Drawing.Color.White;
		this.GroupCommission.Controls.Add(this.label60);
		this.GroupCommission.Controls.Add(this.txtBrokeryRate);
		this.GroupCommission.Controls.Add(this.txtCommrate);
		this.GroupCommission.Controls.Add(this.txtBrokeryAmount);
		this.GroupCommission.Controls.Add(this.txtcommamount);
		this.GroupCommission.Controls.Add(this.txtPpBagCut);
		this.GroupCommission.Controls.Add(this.label61);
		this.GroupCommission.Controls.Add(this.txtJuteBagCut);
		this.GroupCommission.Controls.Add(this.rdFreightCredit);
		this.GroupCommission.Controls.Add(this.rdFreightCash);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.combruom);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.combcommtype);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount);
		this.GroupCommission.Controls.Add((System.Windows.Forms.Control)(object)this.combsalesman);
		this.GroupCommission.Controls.Add(this.label1);
		this.GroupCommission.Controls.Add(this.ChkCommission);
		this.GroupCommission.Controls.Add(this.label25);
		this.GroupCommission.Controls.Add(this.label6);
		this.GroupCommission.Controls.Add(this.label15);
		this.GroupCommission.Controls.Add(this.label21);
		this.GroupCommission.Controls.Add(this.label10);
		this.GroupCommission.Controls.Add(this.label20);
		this.GroupCommission.Controls.Add(this.label19);
		this.GroupCommission.Controls.Add(this.label4);
		this.GroupCommission.Controls.Add(this.label11);
		this.GroupCommission.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupCommission.Location = new System.Drawing.Point(762, 4);
		this.GroupCommission.Name = "GroupCommission";
		this.GroupCommission.Size = new System.Drawing.Size(401, 229);
		this.GroupCommission.TabIndex = 1;
		this.GroupCommission.TabStop = false;
		this.GroupCommission.Text = "   Commission && Brokery";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(313, 75);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(70, 15);
		this.label60.TabIndex = 582;
		this.label60.Text = "JuteBag Cut";
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryRate.Location = new System.Drawing.Point(216, 158);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(77, 23);
		this.txtBrokeryRate.TabIndex = 7;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_TextChanged);
		this.txtBrokeryRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBrokeryRate_KeyPress);
		this.txtCommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommrate.Location = new System.Drawing.Point(216, 52);
		this.txtCommrate.Name = "txtCommrate";
		this.txtCommrate.Size = new System.Drawing.Size(77, 23);
		this.txtCommrate.TabIndex = 2;
		this.txtCommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommrate.TextChanged += new System.EventHandler(txtCommrate_TextChanged);
		this.txtCommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommrate_KeyPress);
		this.txtBrokeryAmount.BackColor = System.Drawing.Color.White;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(216, 185);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(77, 23);
		this.txtBrokeryAmount.TabIndex = 9;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(216, 82);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(77, 23);
		this.txtcommamount.TabIndex = 4;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPpBagCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPpBagCut.Enabled = false;
		this.txtPpBagCut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPpBagCut.Location = new System.Drawing.Point(311, 145);
		this.txtPpBagCut.Name = "txtPpBagCut";
		this.txtPpBagCut.Size = new System.Drawing.Size(74, 23);
		this.txtPpBagCut.TabIndex = 13;
		this.txtPpBagCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(317, 126);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(62, 15);
		this.label61.TabIndex = 584;
		this.label61.Text = "PPBag Cut";
		this.txtJuteBagCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtJuteBagCut.Enabled = false;
		this.txtJuteBagCut.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtJuteBagCut.Location = new System.Drawing.Point(311, 96);
		this.txtJuteBagCut.Name = "txtJuteBagCut";
		this.txtJuteBagCut.Size = new System.Drawing.Size(74, 23);
		this.txtJuteBagCut.TabIndex = 12;
		this.txtJuteBagCut.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.rdFreightCredit.AutoSize = true;
		this.rdFreightCredit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.rdFreightCredit.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdFreightCredit.Location = new System.Drawing.Point(298, 55);
		this.rdFreightCredit.Name = "rdFreightCredit";
		this.rdFreightCredit.Size = new System.Drawing.Size(94, 17);
		this.rdFreightCredit.TabIndex = 11;
		this.rdFreightCredit.TabStop = true;
		this.rdFreightCredit.Text = "Credit Freight";
		this.rdFreightCredit.UseVisualStyleBackColor = true;
		this.rdFreightCredit.Leave += new System.EventHandler(rdFreightCash_Leave);
		this.rdFreightCash.AutoSize = true;
		this.rdFreightCash.Checked = true;
		this.rdFreightCash.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.rdFreightCash.Location = new System.Drawing.Point(298, 26);
		this.rdFreightCash.Name = "rdFreightCash";
		this.rdFreightCash.Size = new System.Drawing.Size(88, 17);
		this.rdFreightCash.TabIndex = 10;
		this.rdFreightCash.TabStop = true;
		this.rdFreightCash.Text = "Cash Freight";
		this.rdFreightCash.UseVisualStyleBackColor = true;
		this.rdFreightCash.Leave += new System.EventHandler(rdFreightCash_Leave);
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(79, 183);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_TextChanged);
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(79, 156);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 6;
		this.CmbBrokeryType.ValueChanged += new System.EventHandler(CmbBrokeryType_ValueChanged);
		this.combruom.AllowNull = (DefaultableBoolean)1;
		this.combruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.combruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combruom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.combruom).Location = new System.Drawing.Point(79, 80);
		((System.Windows.Forms.Control)(object)this.combruom).Name = "combruom";
		((System.Windows.Forms.Control)(object)this.combruom).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.combruom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combruom).TextChanged += new System.EventHandler(combruom_TextChanged);
		this.combcommtype.AllowNull = (DefaultableBoolean)1;
		this.combcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommtype).Location = new System.Drawing.Point(79, 50);
		((System.Windows.Forms.Control)(object)this.combcommtype).Name = "combcommtype";
		((System.Windows.Forms.Control)(object)this.combcommtype).Size = new System.Drawing.Size(82, 26);
		((System.Windows.Forms.Control)(object)this.combcommtype).TabIndex = 1;
		this.combcommtype.ValueChanged += new System.EventHandler(combcommtype_ValueChanged);
		this.CmbBrokeryAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).Location = new System.Drawing.Point(79, 128);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).Name = "CmbBrokeryAccount";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAccount).Leave += new System.EventHandler(CmbBrokeryAccount_Leave);
		this.combsalesman.AllowNull = (DefaultableBoolean)1;
		this.combsalesman.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsalesman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsalesman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsalesman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combsalesman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsalesman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsalesman).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combsalesman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsalesman).Location = new System.Drawing.Point(79, 21);
		((System.Windows.Forms.Control)(object)this.combsalesman).Name = "combsalesman";
		((System.Windows.Forms.Control)(object)this.combsalesman).Size = new System.Drawing.Size(214, 26);
		((System.Windows.Forms.Control)(object)this.combsalesman).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combsalesman).Leave += new System.EventHandler(combsalesman_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 134);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Broker Ac";
		this.ChkCommission.AutoSize = true;
		this.ChkCommission.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkCommission.Location = new System.Drawing.Point(-31, 2);
		this.ChkCommission.Name = "ChkCommission";
		this.ChkCommission.Size = new System.Drawing.Size(15, 14);
		this.ChkCommission.TabIndex = 0;
		this.ChkCommission.UseVisualStyleBackColor = true;
		this.ChkCommission.Visible = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(3, 189);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(61, 15);
		this.label25.TabIndex = 8;
		this.label25.Text = "Rate UOM";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(3, 27);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Comm Agent";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(3, 162);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(75, 15);
		this.label15.TabIndex = 3;
		this.label15.Text = "Brokery Type";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(3, 86);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(61, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate UOM";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(165, 189);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Amount";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(3, 56);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Type";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(165, 86);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Amount";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(165, 162);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(30, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Rate";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(165, 56);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(30, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Rate";
		this.ChkBoxPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrint.AutoSize = true;
		this.ChkBoxPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrint.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrint.Location = new System.Drawing.Point(887, 6);
		this.ChkBoxPrint.Name = "ChkBoxPrint";
		this.ChkBoxPrint.Size = new System.Drawing.Size(112, 18);
		this.ChkBoxPrint.TabIndex = 0;
		this.ChkBoxPrint.Text = "Preview 203";
		this.ChkBoxPrint.UseVisualStyleBackColor = true;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkBoxPrintA);
		this.panel3.Controls.Add(this.labelLocationType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLocationType);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrintIII);
		this.panel3.Controls.Add(this.chkPrintII);
		this.panel3.Controls.Add(this.ChkBoxPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1419, 29);
		this.panel3.TabIndex = 1;
		this.ChkBoxPrintA.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintA.AutoSize = true;
		this.ChkBoxPrintA.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintA.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintA.Location = new System.Drawing.Point(996, 6);
		this.ChkBoxPrintA.Name = "ChkBoxPrintA";
		this.ChkBoxPrintA.Size = new System.Drawing.Size(121, 18);
		this.ChkBoxPrintA.TabIndex = 509;
		this.ChkBoxPrintA.Text = "Preview 203A";
		this.ChkBoxPrintA.UseVisualStyleBackColor = true;
		this.labelLocationType.AutoSize = true;
		this.labelLocationType.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelLocationType.ForeColor = System.Drawing.Color.White;
		this.labelLocationType.Location = new System.Drawing.Point(178, 8);
		this.labelLocationType.Name = "labelLocationType";
		this.labelLocationType.Size = new System.Drawing.Size(83, 15);
		this.labelLocationType.TabIndex = 508;
		this.labelLocationType.Text = "Location Type";
		this.cmbLocationType.AllowNull = (DefaultableBoolean)1;
		this.cmbLocationType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLocationType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLocationType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLocationType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLocationType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Location = new System.Drawing.Point(267, 2);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Name = "cmbLocationType";
		((System.Windows.Forms.Control)(object)this.cmbLocationType).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbLocationType).TabIndex = 507;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(162, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "PURCHASE ORDER";
		this.chkPrintIII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintIII.AutoSize = true;
		this.chkPrintIII.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintIII.ForeColor = System.Drawing.Color.White;
		this.chkPrintIII.Location = new System.Drawing.Point(1258, 6);
		this.chkPrintIII.Name = "chkPrintIII";
		this.chkPrintIII.Size = new System.Drawing.Size(139, 18);
		this.chkPrintIII.TabIndex = 2;
		this.chkPrintIII.Text = "Preview 203_02";
		this.chkPrintIII.UseVisualStyleBackColor = true;
		this.chkPrintII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintII.AutoSize = true;
		this.chkPrintII.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintII.ForeColor = System.Drawing.Color.White;
		this.chkPrintII.Location = new System.Drawing.Point(1114, 6);
		this.chkPrintII.Name = "chkPrintII";
		this.chkPrintII.Size = new System.Drawing.Size(139, 18);
		this.chkPrintII.TabIndex = 1;
		this.chkPrintII.Text = "Preview 203_01";
		this.chkPrintII.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1419, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[21]
		{
			this.btnnew, this.BtnRefresh, this.toolStripSeparator1, this.btnSaveAs, this.btnsave, this.btnUpdate, this.btnDelete, this.toolStripSeparator3, this.btnprint, this.BtnPrintNew,
			this.BtnPrintII, this.BtnPrintWithLabSample, this.toolStripSeparator, this.btnattachment, this.toolStripSeparator4, this.btnDefineCity, this.btnSMS, this.BtnLoadOrder, this.toolStripSeparator6, this.BtnShortCutkeys,
			this.toolStripSeparator2
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1419, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = (System.Drawing.Image)resources.GetObject("btnSaveAs.Image");
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click_1);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "&203-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnPrintNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintNew.Image = (System.Drawing.Image)resources.GetObject("BtnPrintNew.Image");
		this.BtnPrintNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintNew.Name = "BtnPrintNew";
		this.BtnPrintNew.Size = new System.Drawing.Size(102, 24);
		this.BtnPrintNew.Text = "&203A_Print";
		this.BtnPrintNew.Click += new System.EventHandler(BtnPrintNew_Click);
		this.BtnPrintII.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintII.Image = (System.Drawing.Image)resources.GetObject("BtnPrintII.Image");
		this.BtnPrintII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintII.Name = "BtnPrintII";
		this.BtnPrintII.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintII.Text = "&203_01-Print";
		this.BtnPrintII.Click += new System.EventHandler(BtnPrintII_Click);
		this.BtnPrintWithLabSample.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintWithLabSample.Image = (System.Drawing.Image)resources.GetObject("BtnPrintWithLabSample.Image");
		this.BtnPrintWithLabSample.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintWithLabSample.Name = "BtnPrintWithLabSample";
		this.BtnPrintWithLabSample.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintWithLabSample.Text = "&203_02-Print";
		this.BtnPrintWithLabSample.Click += new System.EventHandler(BtnPrintWithLabSample_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnDefineCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineCity.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineCity.Name = "btnDefineCity";
		this.btnDefineCity.Size = new System.Drawing.Size(83, 24);
		this.btnDefineCity.Text = "Define City";
		this.btnDefineCity.Click += new System.EventHandler(btnDefineCity_Click);
		this.btnSMS.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSMS.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.btnSMS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSMS.Name = "btnSMS";
		this.btnSMS.Size = new System.Drawing.Size(60, 24);
		this.btnSMS.Text = "&SMS";
		this.btnSMS.Visible = false;
		this.btnSMS.Click += new System.EventHandler(btnSMS_Click);
		this.BtnLoadOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadOrder.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadOrder.Name = "BtnLoadOrder";
		this.BtnLoadOrder.Size = new System.Drawing.Size(103, 24);
		this.BtnLoadOrder.Text = "Load Order";
		this.BtnLoadOrder.Click += new System.EventHandler(BtnLoadOrder_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1419, 302);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(3, 2);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(211, 17);
		this.label59.TabIndex = 0;
		this.label59.Text = "Detail Of Above Selected Row";
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.panel6);
		this.panel11.Controls.Add(this.panel1);
		this.panel11.Controls.Add(this.panel14);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1419, 400);
		this.panel11.TabIndex = 45;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 110);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1419, 290);
		this.panel6.TabIndex = 77;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1419, 290);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1419, 83);
		this.panel1.TabIndex = 0;
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnshow);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory);
		this.groupBox2.Controls.Add(this.txtToDocNoHistory);
		this.groupBox2.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1086, 77);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(539, 48);
		this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(90, 15);
		this.label14.TabIndex = 146;
		this.label14.Text = "Booking Person";
		this.CmbBookingPersonHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBookingPersonHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBookingPersonHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBookingPersonHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBookingPersonHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBookingPersonHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory).Location = new System.Drawing.Point(629, 42);
		((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory).Name = "CmbBookingPersonHistory";
		((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.CmbBookingPersonHistory).TabIndex = 6;
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(101, 15);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 21);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 143;
		this.label13.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(978, 46);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 11;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(978, 19);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(897, 19);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 8;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(897, 46);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(835, 42);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(539, 21);
		this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(86, 15);
		this.label16.TabIndex = 142;
		this.label16.Text = "Supplier Name";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(362, 48);
		this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(64, 15);
		this.label22.TabIndex = 141;
		this.label22.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(362, 21);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc No";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(8, 48);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(91, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From && To Date";
		this.cmbSupplierNameHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Location = new System.Drawing.Point(629, 15);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Name = "cmbSupplierNameHistory";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).Size = new System.Drawing.Size(203, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNameHistory).TabIndex = 5;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(444, 44);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(444, 17);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(91, 23);
		this.txtFromDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(230, 44);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(127, 22);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(101, 44);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(127, 22);
		this.FromDateHistory.TabIndex = 1;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Controls.Add(this.label42);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1419, 27);
		this.panel14.TabIndex = 75;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1379, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 9;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.White;
		this.label42.Location = new System.Drawing.Point(8, 3);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(197, 19);
		this.label42.TabIndex = 0;
		this.label42.Text = "Purchase Order History";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1419, 27);
		this.panel8.TabIndex = 0;
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1419, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = (System.Drawing.Image)resources.GetObject("btnNewHistory.Image");
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = (System.Drawing.Image)resources.GetObject("btnRefreshHistory.Image");
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1423, 754);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1419, 750);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1419, 723);
		this.panel9.TabIndex = 1;
		this.panel12.Controls.Add(this.panel15);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 400);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1419, 323);
		this.panel12.TabIndex = 46;
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(0, 21);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1419, 302);
		this.panel15.TabIndex = 1;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label59);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1419, 21);
		this.panel13.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1423, 754);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdSupplierLoadingDetail).BackColor = System.Drawing.Color.White;
		grdSupplierLoadingDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSupplierLoadingDetail.DesignTimeLayout = grdSupplierLoadingDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSupplierLoadingDetail.GroupByBoxVisible = false;
		this.grdSupplierLoadingDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).Location = new System.Drawing.Point(2, 604);
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).Name = "grdSupplierLoadingDetail";
		((ScrollableBase)this.grdSupplierLoadingDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).Size = new System.Drawing.Size(1419, 148);
		((System.Windows.Forms.Control)(object)this.grdSupplierLoadingDetail).TabIndex = 3;
		this.grdSupplierLoadingDetail.TotalRow = (InheritableBoolean)1;
		this.grdSupplierLoadingDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSupplierLoadingDetail).VisualStyle = (VisualStyle)4;
		this.grdSupplierLoadingDetail.EditingCell += new EditingCellEventHandler(grdSupplierLoadingDetail_EditingCell);
		this.grdSupplierLoadingDetail.CellUpdated += new ColumnActionEventHandler(grdSupplierLoadingDetail_CellUpdated);
		this.grdSupplierLoadingDetail.ColumnButtonClick += new ColumnActionEventHandler(grdSupplierLoadingDetail_ColumnButtonClick);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1431, 783);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.TabStop = false;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1431, 783);
		base.Controls.Add(this.tabControl1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PurchsaeOrder";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PurchsaeOrder_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.PurchaseOrderDetail.ResumeLayout(false);
		this.PurchaseOrderDetail.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbLabSampleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		this.EmptyBags.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.SupplierExpense.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.ExpensesChargeToProduct.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExpensesChargeToProduct).EndInit();
		this.LabDeductionStandard.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdlab).EndInit();
		this.tabPaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combordercat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		this.GroupCommission.ResumeLayout(false);
		this.GroupCommission.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combruom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLocationType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBookingPersonHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNameHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSupplierLoadingDetail).EndInit();
		this.tabControl1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
