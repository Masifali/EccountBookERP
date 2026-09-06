using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.PurchaseTrading;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class frmSupplierDispatchPreBill : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable table = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtCommBrokeryDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtLocation = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtStatus = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtLastSavedRecord = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtBranch = new DataTable();

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	public static int RecId;

	private bool Approved;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmSupplierDispatchPreBill";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool HasMultiCurrencyFeature;

	private bool ItemSearchWithNameOrCode;

	private bool BranchFeature;

	private bool BranchImplemented;

	private bool FormInitialized;

	private LoadPurchaseOrder LoadPO;

	private List<SupplierDispatchDetail> lstRemoveRecord = new List<SupplierDispatchDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private DateTimePicker txtDocDateMain;

	private Label label17;

	private TextBox txtDocNoMain;

	private Panel PanelDetail;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtRemarksdetail;

	private Label label47;

	private TextBox txtRateDetail;

	private TextBox txtAmountDetail;

	private TextBox txtNetBillWeightDetail;

	private TextBox txtQtyDetail;

	private Label label40;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel7;

	internal UltraCombo CmbItemNameDetail;

	internal UltraCombo CmbPackUomDetail;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Label label3;

	internal UltraCombo CmbCropYearDetail;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal GridEX DataGridHistoryDetail;

	private ToolStrip toolStrip2;

	internal UltraCombo CmbRateUomDetail;

	private ToolStripButton btnDelete;

	private ToolStripButton btnSaveAs;

	internal UltraCombo CmbPackingTypeDetail;

	private Label label1;

	private GroupBox groupBox1;

	private Panel panel10;

	internal UltraCombo CmbCurrencyMain;

	private Label label15;

	private Label label48;

	private TextBox txtExchangeRateMain;

	private TextBox txtFcyAmountMain;

	private Label label16;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel8;

	private GroupBox groupBox2;

	private Button btnShow;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel9;

	private Panel panel12;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel13;

	private Label label49;

	private Label label55;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel14;

	private Label label5;

	private TextBox txtRemarksMain;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Panel panel6;

	internal UltraCombo cmbBranchName;

	private Label label2;

	private DateTimePicker txtETAdestinationMain;

	private DateTimePicker txtDepartureDateMain;

	private TextBox txtTotalAmountMain;

	private Label label22;

	private Label label11;

	private Label label9;

	private Label label7;

	private Label label6;

	private TextBox txtBiltyNoMain;

	private TextBox txtTransitDaysMain;

	private Label label12;

	private TextBox txtVehicleNoMain;

	internal UltraCombo CmbCityNameMain;

	private Label label28;

	private Label label26;

	private Label label25;

	private Label label24;

	private Label label21;

	private TextBox txtOtherExpenseMain;

	private TextBox txtFreightMain;

	private TextBox txtTotalWeightMain;

	private TextBox txtQtyMain;

	internal UltraCombo CmbRefPartyDetail;

	private Label label18;

	internal UltraCombo CmbSupplierDetail;

	private Label label10;

	internal UltraCombo CmbBranchDetail;

	private Label label8;

	private TextBox txtBranchSrNo;

	private ToolStripButton BtnRefPartyForm;

	private Label label31;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label59;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label50;

	private Label label46;

	private Label label45;

	private Label label43;

	private Label label42;

	private Label label35;

	private Label label34;

	private Label label64;

	private Label label66;

	private Label label65;

	private TextBox txtSupplierWbWtMain;

	private TextBox txtAdvanceFreight;

	private Label label14;

	private Label label19;

	private Label label29;

	internal UltraCombo CmbCommType;

	internal UltraCombo CmbCommUOM;

	internal UltraCombo CmbCommisionAgent;

	private Label label30;

	private Label label67;

	private Label label68;

	private Label label70;

	private TextBox txtcommRate;

	private Label label71;

	private TextBox txtcommamount;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbBrokeryAc;

	private Label label69;

	private Label label72;

	private Label label73;

	private Label label74;

	private TextBox txtBrokeryRate;

	private Label label75;

	private TextBox txtBrokeryAmount;

	private Label label76;

	private Label label13;

	private TextBox txtGrossWeight;

	private Label label77;

	private TextBox txtAddLss;

	private Label label78;

	private TextBox txtEBTotal;

	private Label label79;

	private TextBox txtEBUnit;

	private Label label80;

	internal GridEX grdInvExp;

	private Panel panel11;

	private Label label81;

	private Panel panel15;

	private Panel panel16;

	private Label label82;

	private TextBox txtItemAmountFooter;

	private TextBox txtNetBillAmountFooter;

	private Label label84;

	private Label label83;

	private TextBox txtExpenseAmountFooter;

	private TextBox txtAdvanceFreightFooter;

	private Label label85;

	private Label label20;

	private TextBox txtManualBillNo;

	private Label label32;

	internal UltraCombo CmbDeliveryTerm;

	private Label label27;

	private GroupBox grpUpdateHistoryStatus;

	private Label label86;

	private Button btnUpdateStatusHistory;

	private Label label87;

	private ToolStripButton btnPurchaseOrderLoader;

	public frmSupplierDispatchPreBill(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1300, 720);
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtPack.Columns.Add("Id");
			dtPack.Columns.Add("PackTypeDesc");
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderNo");
			table.Columns.Add("OrderDetailId", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("PackUomEquivalent", typeof(double));
			table.Columns.Add("Qty", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("EBUnit", typeof(double));
			table.Columns.Add("EBTotal", typeof(double));
			table.Columns.Add("AddLss");
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom");
			table.Columns.Add("RateUomEquivalent", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("Remarks");
			table.Columns.Add("ReceiverLocationId", typeof(int));
			table.Columns.Add("Expense", typeof(double));
			table.Columns.Add("Commission", typeof(double));
			table.Columns.Add("BillAmount", typeof(double));
			dtInvExp.Columns.Add("Id", typeof(int));
			dtInvExp.Columns.Add("OrderId", typeof(int));
			dtInvExp.Columns.Add("OrderExpId", typeof(int));
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			dtCommBrokeryDetail.Columns.Add("Id", typeof(int));
			dtCommBrokeryDetail.Columns.Add("TypeId", typeof(int));
			dtCommBrokeryDetail.Columns.Add("CommissionAgentGLId", typeof(int));
			dtCommBrokeryDetail.Columns.Add("CommissionAgentId", typeof(int));
			dtCommBrokeryDetail.Columns.Add("CommType", typeof(string));
			dtCommBrokeryDetail.Columns.Add("Uom", typeof(double));
			dtCommBrokeryDetail.Columns.Add("Rate", typeof(double));
			dtCommBrokeryDetail.Columns.Add("Amount", typeof(double));
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnsave.Visible = true;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			BranchFeature = CommonServices.GetERPFeatureById(9);
			string docNo = DocumentNoFill();
			string branchSrNo = BranchDocumentNoFill();
			HistoryBranchComboDbCall();
			DataTable referencePartyData = CommonServices.ReferencePartyServiceBind();
			DataTable currencyData = CurrencyDbCall();
			OtherItemdtDbCall();
			BranchDetailDbCall();
			return new
			{
				docNo = docNo,
				branchSrNo = branchSrNo,
				ReferencePartyData = referencePartyData,
				CurrencyData = currencyData
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
				GetConfigurationsFromGlobal();
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnSaveAs.Enabled = formright.DoHaveSaveRight;
				btnprint.Enabled = formright.DoHavePrintRights;
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				ChkBox.Checked = formright.DoHavePrintRights;
				grpUpdateHistoryStatus.Visible = formright.DoHaveUpdateRights;
				grpUpdateHistoryStatus.Enabled = false;
				UpdateControlGroupRights(ctrlGrdBar1, formright);
				UpdateControlGroupRights(ctrlGrdBar2, formright);
				HandleBranchFeature(BranchFeature);
				HandleMultiCurrencyFeature(HasMultiCurrencyFeature);
				if (ItemSearchWithNameOrCode)
				{
					rdSearchByCode.Checked = true;
				}
				else
				{
					rdSearchByName.Checked = true;
				}
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.docNo, result.branchSrNo);
				}
				SupplierDtFillFromGlobal();
				BindSupplierName();
				ReferancePartyBind(result.ReferencePartyData);
				CurrencyBind(result.CurrencyData);
				DeliveryTerm();
				CommissionTypeFill();
				CommissionUOMFill();
				ItemDtFillFromGlobal();
				ItemNameBind();
				CropDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				CityDtFillFromGlobalAndBind();
				BranchDetailBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				if (table != null && table.Columns.Count > 0)
				{
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtInvExp != null && dtInvExp.Columns.Count > 0)
				{
					AddRowInvExpGrid();
					grdInvExp.RetrieveStructure();
					grdInvExpSettings();
				}
				HistoryBranchComboFill();
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				txtDocDateMain.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void UpdateControlGroupRights(dynamic ctrlGrdBar, dynamic rights)
	{
		ctrlGrdBar.mGridChooseFielder.Enabled = rights.DoHaveFieldChooserRights;
		ctrlGrdBar.mGridSaveLayouts.Enabled = rights.DoHaveSaveLayoutRights;
		ctrlGrdBar.mGridPrint.Enabled = rights.DoHaveGridPrintRights;
		ctrlGrdBar.mGridExport.Enabled = rights.DoHaveGridExportRights;
		ctrlGrdBar.GroupCollaps.Enabled = rights.DoHaveGridGroupCollapseRights;
		ctrlGrdBar.GroupExpand.Enabled = rights.DoHaveGridGroupExpandRights;
	}

	private void HandleBranchFeature(bool BranchFeature)
	{
		label63.Visible = BranchFeature && !BranchImplemented;
		label8.Visible = BranchFeature && !BranchImplemented;
		((Control)(object)CmbBranchDetail).Visible = BranchFeature && !BranchImplemented;
		txtRemarksdetail.Size = new Size(797, 23);
		if (BranchFeature && !BranchImplemented)
		{
			txtRemarksdetail.Size = new Size(602, 23);
		}
	}

	private void HandleMultiCurrencyFeature(bool hasMultiCurrencyFeature)
	{
		((Control)(object)CmbCurrencyMain).Visible = hasMultiCurrencyFeature;
		txtExchangeRateMain.Visible = hasMultiCurrencyFeature;
		txtFcyAmountMain.Visible = hasMultiCurrencyFeature;
		((Control)(object)CmbCurrencyMain).TabStop = hasMultiCurrencyFeature;
		txtExchangeRateMain.TabStop = hasMultiCurrencyFeature;
		txtFcyAmountMain.TabStop = hasMultiCurrencyFeature;
		label15.Visible = hasMultiCurrencyFeature;
		label16.Visible = hasMultiCurrencyFeature;
		label48.Visible = hasMultiCurrencyFeature;
		label43.Visible = hasMultiCurrencyFeature;
		if (!hasMultiCurrencyFeature)
		{
			GetBaseCurrencyAndRate();
		}
	}

	private void HistoryBranchComboDbCall()
	{
		try
		{
			dtBranch.Clear();
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
				return;
			}
			DataTable dt = SupplierDispatch.GetBranchsAllocatedToUserFromSupplierDispatch(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryBranchComboFill()
	{
		try
		{
			((UltraGridBase)cmbBranchName).DataSource = null;
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

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0)
			{
				CmbCropYearDetail.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				CmbPackingTypeDetail.Value = packingTypeId;
			}
			int cityId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("City Area"));
			if (cityId > 0)
			{
				CmbCityNameMain.Value = cityId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SupplierDispatchBranchWise"));
			ItemSearchWithNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)SupplierDispatch.GenerateCode(new SupplierDispatch
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 251,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private string BranchDocumentNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		string Code = "";
		try
		{
			Code = Conversion.ToString((object)SupplierDispatch.GenerateBranchCode(new SupplierDispatch
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 251,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return Code.ToString();
	}

	private void UpdateDocumentNoUI(string DocNo, string BranchSrNo)
	{
		txtDocNoMain.Text = DocNo;
		txtBranchSrNo.Text = BranchSrNo;
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
			DDL.BindDDL(dt, CmbCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			DDL.BindDDL(dt, CmbBrokeryType, "Id", "CommissionType", "BrokeryType", ZeroIndex: false);
			((UltraGridBase)CmbCommType).Rows[1].Activate();
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, CmbCommUOM, "Id", "UOM", "UOM", ZeroIndex: false);
			DDL.BindDDL(dt, CmbBrokeryRateUom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)CmbCommUOM).Rows[0].Activate();
			((UltraGridBase)CmbBrokeryRateUom).Rows[0].Activate();
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
			List<getGlobalSupplierCustomer> obj = clsGlobalVariables.globalSupplierCustomer;
			dtSupplier.Clear();
			if (obj != null && obj.Count > 0)
			{
				for (int i = 0; i < obj.Count; i++)
				{
					dtSupplier.Rows.Add(Conversion.ToInt((object)obj[i].Id), Conversion.ToString((object)obj[i].CompanyName), Conversion.ToString((object)obj[i].PartyCode), Conversion.ToInt((object)obj[i].GlAccountId), Conversion.ToInt((object)obj[i].CityId), Conversion.ToString((object)obj[i].CityName), Conversion.ToString((object)obj[i].MobilePersonal));
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
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierDetail, dtSupplier, "Id", "CompanyName", "Supplier Name", AllColumns: true, Conversion.ToInt(CmbSupplierDetail.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbSupplierDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbSupplierDetail).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			InfragisticsHelper.BindAndRetainSelection(CmbCommisionAgent, dtSupplier, "Id", "CompanyName", "CommisionAgent", AllColumns: true, Conversion.ToInt(CmbCommisionAgent.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbCommisionAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbCommisionAgent).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryAc, dtSupplier, "Id", "CompanyName", "Broker Account", AllColumns: true, Conversion.ToInt(CmbBrokeryAc.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			((UltraGridBase)CmbBrokeryAc).DisplayLayout.Bands[0].Columns[4].Hidden = true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReferancePartyBind(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbRefPartyDetail.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbRefPartyDetail, dt, "Id", "ReferencePartyName", "ReferenceParty Name", AllColumns: false, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			combitem_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalAllCity> lst = clsGlobalVariables.globalAllCities;
			DatatableHelper.PopulateDataTable(dtcity, lst, (GlobalAllCity x) => x.Id, (GlobalAllCity x) => x.CityName);
			int Id = Conversion.ToInt(CmbCityNameMain.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCityNameMain, dtcity, "Id", "CityName", "City Name", AllColumns: true, Id, insertDefaultRow: true);
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
			int ID = Conversion.ToInt(CmbDeliveryTerm.Value);
			DataTable dtTerm = DeliveryTerm.FormHistory();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDL(dtTerm, CmbDeliveryTerm, "Id", "Description", "Delivery Term", ZeroIndex: true);
				((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbDeliveryTerm).Rows[2].Activate();
				((UltraGridBase)CmbDeliveryTerm).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				CommonServices.SetComboValue(CmbDeliveryTerm, "Id", ID, dtTerm);
			}
			else
			{
				((Control)(object)CmbDeliveryTerm).Text = string.Empty;
				((UltraGridBase)CmbDeliveryTerm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable CurrencyDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CurrencyBind(DataTable dt)
	{
		try
		{
			int ID = Conversion.ToInt(CmbCurrencyMain.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCurrencyMain, dt, "Id", "CurrencyCode", "Currency", AllColumns: true, ID, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtFillFromGlobal()
	{
		try
		{
			dtitem.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => row.ItemTypeOfTypeId != 14 && row.ItemTypeOfTypeId != 17).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				dtitem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode);
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
			int Id = Conversion.ToInt(CmbItemNameDetail.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameDetail, dtitem, "Id", ValueMember, "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: true, 1);
			combitem_Leave(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CropDtFillFromGlobalAndBind()
	{
		try
		{
			List<GlobalCropYear> lst = clsGlobalVariables.globalCropYear;
			DatatableHelper.PopulateDataTable(dtCrop, lst, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			int Id = Conversion.ToInt(CmbCropYearDetail.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYearDetail, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			List<InvPackingType> Finallist = clsGlobalVariables.globalInvPackingType;
			DatatableHelper.PopulateDataTable(dtPack, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingTypeDetail.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingTypeDetail, dtPack, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackUomFromGlobalBind(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUomDetail).Text;
			string Rateuom = ((Control)(object)CmbRateUomDetail).Text;
			dtUOM.Clear();
			dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId);
			InfragisticsHelper.BindAndRetainSelection(CmbPackUomDetail, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			InfragisticsHelper.BindAndRetainSelection(CmbRateUomDetail, dtUOM, "Id", "UOMCode", "Rate Uom", AllColumns: true, Rateuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchDetailDbCall()
	{
		try
		{
			dtLocation.Clear();
			dtLocation = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchDetailBind()
	{
		try
		{
			int ID = Conversion.ToInt(CmbBranchDetail.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbBranchDetail, dtLocation, "BranchId", "BranchName", "BranchName", AllColumns: true, ID, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OtherItemdtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
			dtStatus = CommonServices.StaticColumnsService("SupplierDispatchStatus");
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
				if (Conversion.ToInt(CmbItemNameDetail.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemNameDetail, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemNameDetail.Value = ItemId;
				CmbItemNameDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemNameDetail.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			PackUomFromGlobalBind(Conversion.ToInt(CmbItemNameDetail.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int SupplierId = Conversion.ToInt(CmbSupplierDetail.Value);
			if (((UltraGridBase)CmbSupplierDetail).ActiveRow != null && SupplierId > 0)
			{
				dtLastSavedRecord = SupplierDispatch.LastSavedRecord(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, SupplierId, 0);
				if (dtLastSavedRecord != null && dtLastSavedRecord.Rows.Count > 0)
				{
					if (Conversion.ToInt(dtLastSavedRecord.Rows[0]["CityId"]) > 0 && (RecId == 0 || Conversion.ToInt(CmbCityNameMain.Value) == 0))
					{
						CmbCityNameMain.Value = Conversion.ToInt(dtLastSavedRecord.Rows[0]["CityId"]);
					}
					if (!string.IsNullOrEmpty(Conversion.ToString(dtLastSavedRecord.Rows[0]["DeliveryTerm"])) && (RecId == 0 || string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text))))
					{
						((Control)(object)CmbDeliveryTerm).Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["DeliveryTerm"]);
					}
					if (!string.IsNullOrEmpty(Conversion.ToString(dtLastSavedRecord.Rows[0]["CommType"])) && (RecId == 0 || string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)CmbCommType).Text))))
					{
						((Control)(object)CmbCommType).Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["CommType"]);
					}
					if (Conversion.ToDouble(dtLastSavedRecord.Rows[0]["CommRate"]) > 0.0 && (RecId == 0 || Conversion.ToDouble((object)txtcommRate.Text) == 0.0))
					{
						txtcommRate.Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["CommRate"]);
					}
					if (Conversion.ToDouble(dtLastSavedRecord.Rows[0]["CommUom"]) > 0.0 && (RecId == 0 || Conversion.ToDouble((object)((Control)(object)CmbCommUOM).Text) == 0.0))
					{
						((Control)(object)CmbCommUOM).Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["CommUom"]);
					}
					if (!string.IsNullOrEmpty(Conversion.ToString(dtLastSavedRecord.Rows[0]["BrokeryType"])) && (RecId == 0 || string.IsNullOrEmpty(Conversion.ToString((object)((Control)(object)CmbBrokeryType).Text))))
					{
						((Control)(object)CmbBrokeryType).Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["BrokeryType"]);
					}
					if (Conversion.ToDouble(dtLastSavedRecord.Rows[0]["BrokeryRate"]) > 0.0 && (RecId == 0 || Conversion.ToDouble((object)txtBrokeryRate.Text) == 0.0))
					{
						txtBrokeryRate.Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["BrokeryRate"]);
					}
					if (Conversion.ToDouble(dtLastSavedRecord.Rows[0]["BrokeryUom"]) > 0.0 && (RecId == 0 || Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text) == 0.0))
					{
						((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString(dtLastSavedRecord.Rows[0]["BrokeryUom"]);
					}
				}
			}
			else
			{
				dtLastSavedRecord.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNoMain.Text.Trim() == string.Empty || txtDocNoMain.Text.Trim() == "0" || txtDocNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtDocNoMain.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplierDetail).ActiveRow == null || Conversion.ToInt(CmbSupplierDetail.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			CmbSupplierDetail.Focus();
			return false;
		}
		if (txtVehicleNoMain.Text.Trim() == string.Empty || txtVehicleNoMain.Text.Trim() == "0" || txtVehicleNoMain.Text.Trim() == null)
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtVehicleNoMain.Focus();
			return false;
		}
		if (txtTransitDaysMain.Text.Trim() == string.Empty || Conversion.ToInt((object)txtTransitDaysMain.Text) == 0)
		{
			MessageBox.Show("TransitDays Field is Required");
			txtTransitDaysMain.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCityNameMain).ActiveRow == null || Conversion.ToInt(CmbCityNameMain.Value) == 0)
		{
			MessageBox.Show("CityName Field is Required");
			CmbCityNameMain.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)CmbCurrencyMain).ActiveRow == null || Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				CmbCurrencyMain.Focus();
				return false;
			}
			if (txtExchangeRateMain.Text.Trim() == string.Empty || txtExchangeRateMain.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRateMain.Focus();
				return false;
			}
			if (txtFcyAmountMain.Text.Trim() == string.Empty || txtFcyAmountMain.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmountMain.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)CmbCurrencyMain).ActiveRow == null || Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				CmbCurrencyMain.Focus();
				return false;
			}
			if (txtExchangeRateMain.Text.Trim() == string.Empty || txtExchangeRateMain.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRateMain.Focus();
				return false;
			}
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbSupplierDetail).ActiveRow == null || Conversion.ToInt(CmbSupplierDetail.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			CmbSupplierDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemNameDetail).ActiveRow == null || Conversion.ToInt(CmbItemNameDetail.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItemNameDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYearDetail).ActiveRow == null || Conversion.ToInt(CmbCropYearDetail.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYearDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingTypeDetail).ActiveRow == null || Conversion.ToInt(CmbPackingTypeDetail.Value) == 0)
		{
			MessageBox.Show("Pack Type Field is Required");
			CmbPackingTypeDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUomDetail).ActiveRow == null || Conversion.ToInt(CmbPackUomDetail.Value) == 0)
		{
			MessageBox.Show("Item Pack Unit Field is Required");
			CmbPackUomDetail.Focus();
			return false;
		}
		if (txtQtyDetail.Text.Trim() == string.Empty || txtQtyDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtQtyDetail.Text) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQtyDetail.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || txtGrossWeight.Text.Trim() == "0" || Conversion.ToDouble((object)txtGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetBillWeightDetail.Text.Trim() == string.Empty || txtNetBillWeightDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtNetBillWeightDetail.Text) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetBillWeightDetail.Focus();
			return false;
		}
		if (txtRateDetail.Text.Trim() == string.Empty || txtRateDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtRateDetail.Text) == 0.0)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRateDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUomDetail).ActiveRow == null || Conversion.ToInt(CmbRateUomDetail.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			CmbRateUomDetail.Focus();
			return false;
		}
		if (txtAmountDetail.Text.Trim() == string.Empty || txtAmountDetail.Text.Trim() == "0" || Conversion.ToDouble((object)txtAmountDetail.Text) == 0.0)
		{
			MessageBox.Show("Item amount Field is Required");
			txtAmountDetail.Focus();
			return false;
		}
		if (BranchFeature && (((UltraGridBase)CmbBranchDetail).ActiveRow == null || Conversion.ToInt(CmbBranchDetail.Value) == 0))
		{
			MessageBox.Show("Receiver's Location Field is Required");
			CmbBranchDetail.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "OrderId"))
			{
				throw new Exception("Grid has records from Purchase Order. Manual Entry is not allowed!");
			}
			if (FormValidationDetail())
			{
				int ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				string ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemName"].Value);
				string ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemCode"].Value);
				table.Rows.Add(0, 0, 0, 0, ItemId, ItemCode, ItemName, Conversion.ToInt(CmbCropYearDetail.Value), Conversion.ToInt(CmbPackingTypeDetail.Value), Conversion.ToInt(CmbPackUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbPackUomDetail).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQtyDetail.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtEBUnit.Text.Trim()), Conversion.ToDouble((object)txtEBTotal.Text.Trim()), Conversion.ToDouble((object)txtAddLss.Text.Trim()), Conversion.ToDouble((object)txtNetBillWeightDetail.Text.Trim()), Conversion.ToInt(CmbRateUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbRateUomDetail).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtRateDetail.Text.Trim()), Conversion.ToDouble((object)txtAmountDetail.Text), (Conversion.ToDecimal((object)txtExchangeRateMain.Text) > 0m && Conversion.ToDecimal((object)txtAmountDetail.Text) > 0m) ? (Conversion.ToDecimal((object)txtAmountDetail.Text) / Conversion.ToDecimal((object)txtExchangeRateMain.Text)) : 0m, Conversion.ToString((object)txtRemarksdetail.Text.Trim()), Conversion.ToInt(CmbBranchDetail.Value));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				((Control)(object)CmbItemNameDetail).Text = string.Empty;
				((Control)(object)CmbPackUomDetail).Text = string.Empty;
				((Control)(object)CmbPackingTypeDetail).Text = string.Empty;
				txtQtyDetail.Text = string.Empty;
				txtNetBillWeightDetail.Text = string.Empty;
				((Control)(object)CmbRateUomDetail).Text = string.Empty;
				txtRateDetail.Text = string.Empty;
				txtAmountDetail.Text = string.Empty;
				txtRemarksdetail.Text = string.Empty;
				CmbItemNameDetail.Focus();
				TotalCommissionAmount();
				TotalBrokeryAmount();
				CommissionProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
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
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
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
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
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
					SupplierDispatchDetail vd = new SupplierDispatchDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.SupplierId = Conversion.ToInt(CmbSupplierDetail.Value);
					vd.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.CommissionAgentId = Conversion.ToInt(r.Cells["CommissionAgentId"].Value);
					vd.ReferencePartyId = Conversion.ToInt(CmbRefPartyDetail.Value);
					vd.OrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.PackSizeId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
					vd.WeightKgs = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
					vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.BranchId = Conversion.ToInt(r.Cells["ReceiverLocationId"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
				}
				else
				{
					r.Delete();
					grd.UpdateData();
				}
				TotalCommissionAmount();
				TotalBrokeryAmount();
				CommissionProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
				return;
			}
			throw new Exception("Please Reset the Detail First...");
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
			List<string> hidCol = new List<string> { "Id", "OrderId", "OrderDetailId", "ItemId", "PackUomId", "PackUomEquivalent", "RateUomId", "RateUomEquivalent" };
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"GrossWeight",
					(EditType)1
				},
				{
					"EBUnit",
					(EditType)1
				},
				{
					"AddLss",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hidCol, editCol);
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["ReceiverLocationId"].Visible = BranchFeature && !BranchImplemented;
			grd.RootTable.Columns["Weight"].Caption = "Net Bill Weight";
			ComboInGrid(grd);
			grd.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYearId"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["PackingTypeId"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["ReceiverLocationId"].Width = Constants.InventoryConstants.BranchName;
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			bool OrderExists = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "OrderId");
			grd.RootTable.Columns["OrderNo"].Visible = OrderExists;
			if (!OrderExists)
			{
				GridEX_Helper.AddButton(grd, "Edit", "Edit", 50, 1);
			}
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && !GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "OrderId"))
			{
				updateDetailIndex = r.RowIndex;
				CmbSupplierDetail_Leave(null, null);
				CmbItemNameDetail.Value = r.Cells["ItemId"].Value.ToString();
				CmbRateUomDetail.Value = r.Cells["RateUomId"].Value.ToString();
				CmbPackUomDetail.Value = r.Cells["PackUomId"].Value.ToString();
				combitem_Leave(null, null);
				CmbCropYearDetail.Value = r.Cells["CropYearId"].Value.ToString();
				CmbPackingTypeDetail.Value = r.Cells["PackingTypeId"].Value.ToString();
				CmbPackUomDetail.Value = r.Cells["PackUomId"].Value.ToString();
				txtQtyDetail.Text = Conversion.ToDouble(r.Cells["Qty"].Value).ToString("#,##0.###");
				txtGrossWeight.Text = Conversion.ToDouble(r.Cells["GrossWeight"].Value).ToString("#,##0.###");
				txtEBUnit.Text = Conversion.ToDouble(r.Cells["EBUnit"].Value).ToString("#,##0.###");
				txtEBTotal.Text = Conversion.ToDouble(r.Cells["EBTotal"].Value).ToString("#,##0.###");
				txtAddLss.Text = Conversion.ToDouble(r.Cells["AddLss"].Value).ToString("#,##0.###");
				txtNetBillWeightDetail.Text = Conversion.ToDouble(r.Cells["Weight"].Value).ToString("#,##0.###");
				txtRateDetail.Text = Conversion.ToDouble(r.Cells["Rate"].Value).ToString("#,##0.###");
				CmbRateUomDetail.Value = r.Cells["RateUomId"].Value.ToString();
				txtAmountDetail.Text = Conversion.ToDouble(r.Cells["Amount"].Value).ToString("#,##0.###");
				txtRemarksdetail.Text = r.Cells["Remarks"].Value.ToString();
				CmbBranchDetail.Value = r.Cells["ReceiverLocationId"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItemNameDetail.Focus();
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
			if (FormValidationDetail())
			{
				int ItemId = Conversion.ToInt(CmbItemNameDetail.Value);
				string ItemName = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemName"].Value);
				string ItemCode = Conversion.ToString(((UltraDropDownBase)CmbItemNameDetail).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemId"] = ItemId;
				table.Rows[updateDetailIndex]["ItemCode"] = ItemCode;
				table.Rows[updateDetailIndex]["ItemName"] = ItemName;
				table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYearDetail.Value);
				table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingTypeDetail.Value);
				table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUomDetail.Value);
				table.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUomDetail).Text);
				table.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQtyDetail.Text);
				table.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				table.Rows[updateDetailIndex]["EBUnit"] = Conversion.ToDouble((object)txtEBUnit.Text);
				table.Rows[updateDetailIndex]["EBTotal"] = Conversion.ToDouble((object)txtEBTotal.Text);
				table.Rows[updateDetailIndex]["AddLss"] = Conversion.ToDouble((object)txtAddLss.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtNetBillWeightDetail.Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUomDetail.Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUomDetail).Text);
				table.Rows[updateDetailIndex]["RateUomEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value);
				table.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRateDetail.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmountDetail.Text);
				table.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksdetail.Text);
				table.Rows[updateDetailIndex]["ReceiverLocationId"] = Conversion.ToInt(CmbBranchDetail.Value);
				grd.DataSource = table;
				TotalCommissionAmount();
				TotalBrokeryAmount();
				CommissionProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
				ResetDetail();
				CmbItemNameDetail.Focus();
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
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				case "Qty":
				case "PackUomEquivalent":
				case "EBUnit":
				case "EBTotal":
				case "AddLss":
					UpdateCalculatedFields(row);
					break;
				case "GrossWeight":
				{
					double gross = Conversion.ToDouble(row.Cells["GrossWeight"].Value);
					double ebTotal = Conversion.ToDouble(row.Cells["EBTotal"].Value);
					double addLss = Conversion.ToDouble(row.Cells["AddLss"].Value);
					row.Cells["Weight"].Value = gross - ebTotal + addLss;
					break;
				}
				}
				double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
				double rateUom = Conversion.ToDouble(row.Cells["RateUomEquivalent"].Value);
				double weight = Conversion.ToDouble(row.Cells["Weight"].Value);
				double amount = ((rateUom > 0.0) ? (weight / rateUom * rate) : 0.0);
				row.Cells["Amount"].Value = amount;
				row.Cells["FcyAmount"].Value = ((Conversion.ToDouble((object)txtExchangeRateMain.Text) > 0.0) ? (amount / Conversion.ToDouble((object)txtExchangeRateMain.Text)) : 0.0);
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboInGrid(GridEX grd)
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["ReceiverLocationId"], dtLocation, "BranchId", "BranchName", "Receiver Location", 130);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["PackingTypeId"], dtPack, "Id", "PackTypeDesc", "Packing Type", 100);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateCalculatedFields(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double packUom = Conversion.ToDouble(row.Cells["PackUomEquivalent"].Value);
		double grossWeight = qty * packUom;
		row.Cells["GrossWeight"].Value = grossWeight;
		double ebUnit = Conversion.ToDouble(row.Cells["EBUnit"].Value);
		double ebTotal = Conversion.ToDouble(row.Cells["EBTotal"].Value);
		if (ebTotal == 0.0 && ebUnit > 0.0)
		{
			ebTotal = qty * ebUnit;
		}
		else if (ebUnit == 0.0 && ebTotal > 0.0)
		{
			ebUnit = ebTotal / qty;
		}
		row.Cells["EBUnit"].Value = ebUnit;
		row.Cells["EBTotal"].Value = ebTotal;
		double addLess = Conversion.ToDouble(row.Cells["AddLss"].Value);
		double netWeight = grossWeight - ebTotal + addLess;
		row.Cells["Weight"].Value = netWeight;
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		double rateUom = Conversion.ToDouble(row.Cells["RateUomEquivalent"].Value);
		double amount = ((rateUom > 0.0) ? (netWeight / rateUom * rate) : 0.0);
		row.Cells["Amount"].Value = amount;
		row.Cells["FcyAmount"].Value = ((Conversion.ToDouble((object)txtExchangeRateMain.Text) > 0.0) ? (amount / Conversion.ToDouble((object)txtExchangeRateMain.Text)) : 0.0);
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		try
		{
			grdInvExp.RootTable.Columns["Id"].Visible = false;
			grdInvExp.RootTable.Columns["OrderId"].Visible = false;
			grdInvExp.RootTable.Columns["OrderExpId"].Visible = false;
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Other Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInvExp.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			grdInvExp.RootTable.Columns["ItemId"].Width = Constants.InventoryConstants.ItemName - 70;
			grdInvExp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant - 20;
			grdInvExp.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant - 20;
			grdInvExp.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant - 20;
			grdInvExp.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants - 30;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "X";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "X";
			grdInvExp.RootTable.Columns["Delete"].Width = 20;
			grdInvExp.RootTable.Columns["Delete"].Position = 0;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "+";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "+";
			grdInvExp.RootTable.Columns["Add"].Width = 20;
			grdInvExp.RootTable.Columns["Add"].Position = 1;
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					dtInvExp.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				if (e.Column.Key == "Add")
				{
					AddRowInvExpGrid();
				}
			}
			grdInvExp.UpdateData();
			ExpProportion();
			txtExchangeRate_TextChanged(null, null);
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
			ExpProportion();
			txtExchangeRate_TextChanged(null, null);
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
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					SupplierDispatch.DeleteByID(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Expected O, but got Unknown
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Expected O, but got Unknown
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				SupplierDispatch po = new SupplierDispatch();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					po.Id = RecId;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				TotalCommissionAmount();
				TotalBrokeryAmount();
				CommissionProportion();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
				int CommissionAgentId = Conversion.ToInt(CmbCommisionAgent.Value);
				if (Conversion.ToDecimal((object)txtcommamount.Text.Trim()) > 0m && CommissionAgentId == 0)
				{
					MessageBox.Show("Please Select Commission Agent Account First");
					CmbCommisionAgent.Focus();
					return;
				}
				if (CommissionAgentId > 0 && Conversion.ToDecimal((object)txtcommamount.Text.Trim()) == 0m)
				{
					MessageBox.Show("Commission Amount Required when Commission agent is Selected");
					txtcommRate.Focus();
					return;
				}
				int BrokeryAcId = Conversion.ToInt(CmbBrokeryAc.Value);
				if (Conversion.ToDecimal((object)txtBrokeryAmount.Text.Trim()) > 0m && BrokeryAcId == 0)
				{
					MessageBox.Show("Please Select Broker Account First");
					CmbCommisionAgent.Focus();
					return;
				}
				if (BrokeryAcId > 0 && Conversion.ToDecimal((object)txtBrokeryAmount.Text.Trim()) == 0m)
				{
					MessageBox.Show("Brokery Amount Required when Broker is Selected");
					txtBrokeryRate.Focus();
					return;
				}
				if (BrokeryAcId == Conversion.ToInt(CmbSupplierDetail.Value))
				{
					throw new Exception("Broker And Supplier Can't be Same...");
				}
				po.OrganizationId = UserAccount.OrganizationId;
				po.CompanyId = UserAccount.CompanyId;
				po.BranchesId = UserAccount.BranchesId;
				po.ProjectId = UserAccount.BranchesId;
				po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				po.DocumentTypeId = 251;
				po.EntryUserId = UserAccount.ID;
				po.ModifyUserId = UserAccount.ID;
				po.ApprovedUserId = UserAccount.ID;
				po.ModifyDate = DateTime.Now;
				po.EntryDate = DateTime.Now;
				po.ApprovedDate = DateTime.Now;
				po.DocDate = txtDocDateMain.Value;
				po.DocNo = Conversion.ToInt((object)txtDocNoMain.Text);
				po.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
				po.ManualBillNo = Conversion.ToString((object)txtManualBillNo.Text);
				po.VehicleNo = Conversion.ToString((object)txtVehicleNoMain.Text);
				po.BiltyNo = Conversion.ToString((object)txtBiltyNoMain.Text);
				po.DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
				po.TransitDays = Conversion.ToInt((object)txtTransitDaysMain.Text);
				po.ETAdestination = Conversion.ToDateTime((object)txtETAdestinationMain.Value);
				po.Freight = Conversion.ToDecimal((object)txtFreightMain.Text);
				po.OtherExpense = Conversion.ToDecimal((object)txtOtherExpenseMain.Text);
				po.AdvanceFreight = Conversion.ToDecimal((object)txtAdvanceFreight.Text);
				po.CityId = Conversion.ToInt(CmbCityNameMain.Value);
				po.Driver1CellNo = "";
				po.SupplierWbWeight = Conversion.ToDecimal((object)txtSupplierWbWtMain.Text);
				po.TotalQty = Conversion.ToDecimal((object)txtQtyMain.Text);
				po.TotalWeight = Conversion.ToDecimal((object)txtTotalWeightMain.Text);
				po.TotalAmount = Conversion.ToDecimal((object)txtTotalAmountMain.Text);
				po.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
				po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
				po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmountMain.Text);
				po.RemarksHeader = Conversion.ToString((object)txtRemarksMain.Text);
				po.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
				po.SupplierDispatchDetailList = new List<SupplierDispatchDetail>();
				po.SupplierDispatchCommDetailList = new List<SupplierDispatchCommDetail>();
				po.SupplierDispatchExpenseList = new List<SupplierDispatchExpense>();
				po.AttachmentsList = new List<DMSAttachments>();
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				double TotalGrossWt = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					SupplierDispatchDetail vd = new SupplierDispatchDetail();
					if (RecId == 0)
					{
						vd.ActionTypeId = 1;
					}
					else
					{
						vd.Id = Conversion.ToInt(r3.Cells["Id"].Value);
						if (vd.Id > 0)
						{
							vd.ActionTypeId = 2;
						}
						else
						{
							vd.ActionTypeId = 1;
						}
					}
					if (Conversion.ToInt(r3.Cells["ItemId"].Value) == 0)
					{
						throw new Exception($"Item Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToInt(r3.Cells["CropYearId"].Value) == 0)
					{
						throw new Exception($"CropYear Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToInt(r3.Cells["PackingTypeId"].Value) == 0)
					{
						throw new Exception($"PackingType Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToInt(r3.Cells["PackUomId"].Value) == 0)
					{
						throw new Exception($"PackUom Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r3.Cells["Qty"].Value) == 0.0)
					{
						throw new Exception($"Qty Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r3.Cells["GrossWeight"].Value) == 0.0)
					{
						throw new Exception($"GrossWeight Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r3.Cells["Weight"].Value) == 0.0)
					{
						throw new Exception($"Weight Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToInt(r3.Cells["RateUomId"].Value) == 0)
					{
						throw new Exception($"RateUom Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r3.Cells["Rate"].Value) == 0.0)
					{
						throw new Exception($"Rate Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (Conversion.ToDouble(r3.Cells["Amount"].Value) == 0.0)
					{
						throw new Exception($"Amount Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					if (BranchFeature && !BranchImplemented && Conversion.ToInt(r3.Cells["ReceiverLocationId"].Value) == 0)
					{
						throw new Exception($"ReceiverLocation Field Required in Grid Row no : {r3.RowIndex + 1}");
					}
					vd.SupplierId = Conversion.ToInt(CmbSupplierDetail.Value);
					vd.OrderId = Conversion.ToInt(r3.Cells["OrderId"].Value);
					vd.CommissionAgentId = 0;
					vd.ReferencePartyId = Conversion.ToInt(CmbRefPartyDetail.Value);
					vd.OrderDetailId = Conversion.ToInt(r3.Cells["OrderDetailId"].Value);
					vd.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
					vd.PackSizeId = Conversion.ToInt(r3.Cells["PackUomId"].Value);
					vd.Qty = Conversion.ToDouble(r3.Cells["Qty"].Value);
					vd.GrossWeight = Conversion.ToDouble(r3.Cells["GrossWeight"].Value);
					TotalGrossWt += vd.GrossWeight;
					vd.EBWeight = Conversion.ToDouble(r3.Cells["EBUnit"].Value);
					vd.EBTotalWt = Conversion.ToDouble(r3.Cells["EBTotal"].Value);
					vd.AdLsWeight = Conversion.ToDouble(r3.Cells["AddLss"].Value);
					vd.WeightKgs = Conversion.ToDouble(r3.Cells["Weight"].Value);
					vd.RateUomId = Conversion.ToInt(r3.Cells["RateUomId"].Value);
					vd.Rate = Conversion.ToDouble(r3.Cells["Rate"].Value);
					vd.Amount = Conversion.ToDouble(r3.Cells["Amount"].Value);
					vd.CurrencyId = Conversion.ToInt(CmbCurrencyMain.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRateMain.Text);
					vd.FcyAmount = Conversion.ToDecimal(r3.Cells["FcyAmount"].Value);
					vd.Remarks = Conversion.ToString(r3.Cells["Remarks"].Value);
					vd.BranchId = Conversion.ToInt(r3.Cells["ReceiverLocationId"].Value);
					po.SupplierDispatchDetailList.Add(vd);
				}
				if (po.SupplierDispatchDetailList.Count == 0)
				{
					throw new Exception("At least enter value/quantity in one of the rows");
				}
				double SupplierWb = Conversion.ToDouble((object)txtSupplierWbWtMain.Text.Trim());
				if (TotalGrossWt != SupplierWb)
				{
					throw new Exception($"Total Gross Weight {TotalGrossWt} in Detail Grid is not Equal to SupplierWb {SupplierWb} Weight!");
				}
				if (lstRemoveRecord.Count > 0)
				{
					for (int i = 0; i < lstRemoveRecord.Count; i++)
					{
						po.SupplierDispatchDetailList.Add(lstRemoveRecord[i]);
					}
				}
				dtCommBrokeryDetail.Clear();
				if (Conversion.ToInt(CmbCommisionAgent.Value) != 0 && Conversion.ToDecimal((object)txtcommRate.Text) != 0m)
				{
					dtCommBrokeryDetail.Rows.Add(0, 1, Conversion.ToInt(((UltraDropDownBase)CmbCommisionAgent).SelectedRow.Cells["GlAccountId"].Value), Conversion.ToInt(((UltraDropDownBase)CmbCommisionAgent).SelectedRow.Cells["Id"].Value), Conversion.ToInt(CmbCommType.Value), Conversion.ToDouble((object)((Control)(object)CmbCommUOM).Text), Conversion.ToDouble((object)txtcommRate.Text), Conversion.ToDouble((object)txtcommamount.Text));
				}
				if (Conversion.ToInt(CmbBrokeryAc.Value) != 0 && Conversion.ToDecimal((object)txtBrokeryRate.Text) != 0m)
				{
					dtCommBrokeryDetail.Rows.Add(0, 2, Conversion.ToInt(((UltraDropDownBase)CmbBrokeryAc).SelectedRow.Cells["GlAccountId"].Value), Conversion.ToInt(((UltraDropDownBase)CmbBrokeryAc).SelectedRow.Cells["Id"].Value), Conversion.ToInt(CmbBrokeryType.Value), Conversion.ToDouble(CmbBrokeryRateUom.Value), Conversion.ToDouble((object)txtBrokeryRate.Text), Conversion.ToDouble((object)txtBrokeryAmount.Text));
				}
				foreach (DataRow r2 in dtCommBrokeryDetail.Rows)
				{
					if ((Conversion.ToInt(r2["CommissionAgentId"]) != 0 || Conversion.ToInt(r2["CommissionAgentGLId"]) != 0) && Conversion.ToDecimal(r2["Rate"]) != 0m)
					{
						SupplierDispatchCommDetail PJ = new SupplierDispatchCommDetail();
						PJ.Id = Conversion.ToInt(r2["Id"]);
						PJ.TypeId = Conversion.ToInt(r2["TypeId"]);
						PJ.CommissionAgentId = Conversion.ToInt(r2["CommissionAgentId"]);
						PJ.CommissionAgentGLId = Conversion.ToInt(r2["CommissionAgentGLId"]);
						if (PJ.CommissionAgentId == 0 && PJ.CommissionAgentGLId == 0)
						{
							MessageBox.Show(((PJ.TypeId == 1) ? "CommissionAgent" : "Broker") + "  Is Required");
							return;
						}
						PJ.CommType = Conversion.ToString(r2["CommType"]);
						PJ.RateUom = Conversion.ToDouble(r2["Uom"]);
						PJ.CommRate = Conversion.ToDouble(r2["Rate"]);
						PJ.CommAmount = Conversion.ToDouble(r2["Amount"]);
						po.SupplierDispatchCommDetailList.Add(PJ);
					}
				}
				rows = grdInvExp.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
					{
						SupplierDispatchExpense PE = new SupplierDispatchExpense();
						if (Conversion.ToDouble(r.Cells["Amount"].Value) == 0.0)
						{
							throw new Exception($"Amount Field Required in Expense Grid Row no : {r.RowIndex + 1}");
						}
						PE.Id = Conversion.ToInt(r.Cells["Id"].Value);
						PE.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						PE.OrderExpId = Conversion.ToInt(r.Cells["OrderExpId"].Value);
						PE.OtherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						PE.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
						PE.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PE.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						PE.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						po.SupplierDispatchExpenseList.Add(PE);
					}
				}
				po.AttachmentsList = new List<DMSAttachments>();
				po.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<SupplierDispatch>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				po.AttachmentsValues = AttachmentsValues;
				po.CustomAttachmentsValues = CustomAttachmentsValues;
				int success = (po.Id = SupplierDispatch.Save(po));
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + po.DocNo);
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 251, 0);
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + po.DocNo);
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.SupplierDispatchPreBillSlip251(success);
				}
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
			Approved = false;
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			SupplierDispatch po = SupplierDispatch.GetByID(RecId);
			if (po == null || po.SupplierDispatchDetailList.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtDocDateMain.Value = po.DocDate;
			txtDocNoMain.Text = Conversion.ToString((object)po.DocNo);
			txtBranchSrNo.Text = Conversion.ToString((object)po.BranchSrNo);
			txtManualBillNo.Text = Conversion.ToString((object)po.ManualBillNo);
			txtVehicleNoMain.Text = Conversion.ToString((object)po.VehicleNo);
			txtBiltyNoMain.Text = Conversion.ToString((object)po.BiltyNo);
			txtDepartureDateMain.Value = po.DepartureDate;
			txtTransitDaysMain.Text = Conversion.ToString((object)po.TransitDays);
			txtETAdestinationMain.Value = po.ETAdestination;
			txtFreightMain.Text = po.Freight.ToString("#,##0.##");
			txtOtherExpenseMain.Text = po.OtherExpense.ToString("#,##0.##");
			txtAdvanceFreight.Text = po.AdvanceFreight.ToString("#,##0.##");
			CmbCityNameMain.Value = Conversion.ToInt((object)po.CityId);
			txtSupplierWbWtMain.Text = po.SupplierWbWeight.ToString("#,##0.##");
			CmbDeliveryTerm.Value = Conversion.ToString((object)po.DeliveryTerm);
			txtQtyMain.Text = po.TotalQty.ToString("#,##0.##");
			txtTotalWeightMain.Text = po.TotalWeight.ToString("#,##0.##");
			txtTotalAmountMain.Text = po.TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			CmbCurrencyMain.Value = Conversion.ToInt((object)po.CurrencyId);
			txtExchangeRateMain.Text = po.ExchangeRate.ToString("#,##0.##");
			txtFcyAmountMain.Text = po.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			txtRemarksMain.Text = Conversion.ToString((object)po.RemarksHeader);
			Approved = Conversion.ToBool((object)po.IsApproved);
			if (po.CurrencyId == 0)
			{
				GetBaseCurrencyAndRate();
			}
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			CmbSupplierDetail.Value = po.SupplierDispatchDetailList[0].SupplierId;
			CmbRefPartyDetail.Value = po.SupplierDispatchDetailList[0].ReferencePartyId;
			table.Clear();
			for (int k = 0; k < po.SupplierDispatchDetailList.Count; k++)
			{
				table.Rows.Add(po.SupplierDispatchDetailList[k].Id, po.SupplierDispatchDetailList[k].OrderId, po.SupplierDispatchDetailList[k].OrderNo, po.SupplierDispatchDetailList[k].OrderDetailId, po.SupplierDispatchDetailList[k].ItemId, po.SupplierDispatchDetailList[k].ItemCode, po.SupplierDispatchDetailList[k].ItemName, po.SupplierDispatchDetailList[k].CropYearId, po.SupplierDispatchDetailList[k].PackingTypeId, po.SupplierDispatchDetailList[k].PackSizeId, po.SupplierDispatchDetailList[k].PackUomCode, po.SupplierDispatchDetailList[k].PackUomEquivalent, po.SupplierDispatchDetailList[k].Qty, po.SupplierDispatchDetailList[k].GrossWeight, po.SupplierDispatchDetailList[k].EBWeight, po.SupplierDispatchDetailList[k].EBTotalWt, po.SupplierDispatchDetailList[k].AdLsWeight, po.SupplierDispatchDetailList[k].WeightKgs, po.SupplierDispatchDetailList[k].RateUomId, po.SupplierDispatchDetailList[k].RateUomCode, po.SupplierDispatchDetailList[k].RateUomEquivalent, po.SupplierDispatchDetailList[k].Rate, po.SupplierDispatchDetailList[k].Amount, po.SupplierDispatchDetailList[k].FcyAmount, po.SupplierDispatchDetailList[k].Remarks, po.SupplierDispatchDetailList[k].BranchId);
				if (po.SupplierDispatchDetailList[k].OrderId > 0)
				{
					EnableDisableFeilds(Value: false);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			if (po.SupplierDispatchCommDetailList.Count > 0)
			{
				for (int j = 0; j < po.SupplierDispatchCommDetailList.Count; j++)
				{
					if (po.SupplierDispatchCommDetailList[j].TypeId == 1)
					{
						CmbCommisionAgent.Value = Conversion.ToInt((object)po.SupplierDispatchCommDetailList[j].CommissionAgentId);
						CmbCommType.Value = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommType);
						txtcommRate.Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommRate);
						((Control)(object)CmbCommUOM).Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].RateUom);
						txtcommamount.Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommAmount);
					}
					if (po.SupplierDispatchCommDetailList[j].TypeId == 2)
					{
						CmbBrokeryAc.Value = Conversion.ToInt((object)po.SupplierDispatchCommDetailList[j].CommissionAgentId);
						CmbBrokeryType.Value = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommType);
						txtBrokeryRate.Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommRate);
						((Control)(object)CmbBrokeryRateUom).Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].RateUom);
						txtBrokeryAmount.Text = Conversion.ToString((object)po.SupplierDispatchCommDetailList[j].CommAmount);
					}
				}
			}
			if (string.IsNullOrEmpty(((Control)(object)CmbBrokeryType).Text) && ((UltraGridBase)CmbBrokeryType).DataSource != null)
			{
				((UltraGridBase)CmbBrokeryType).Rows[1].Activate();
			}
			if (string.IsNullOrEmpty(((Control)(object)CmbCommType).Text) && ((UltraGridBase)CmbCommType).DataSource != null)
			{
				((UltraGridBase)CmbCommType).Rows[1].Activate();
			}
			dtInvExp.Rows.Clear();
			for (int i = 0; i < po.SupplierDispatchExpenseList.Count; i++)
			{
				dtInvExp.Rows.Add(po.SupplierDispatchExpenseList[i].Id, po.SupplierDispatchExpenseList[i].OrderId, po.SupplierDispatchExpenseList[i].OrderExpId, po.SupplierDispatchExpenseList[i].OtherItemId, po.SupplierDispatchExpenseList[i].Qty, po.SupplierDispatchExpenseList[i].Rate, po.SupplierDispatchExpenseList[i].Amount, po.SupplierDispatchExpenseList[i].Remarks);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				TA.ActionId = 2;
				AT.lst.Add(TA);
			}
			TotalCommissionAmount();
			TotalBrokeryAmount();
			CommissionProportion();
			ExpProportion();
			txtExchangeRate_TextChanged(null, null);
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnDelete.Visible = true;
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
			List<string> allServices = new List<string>
			{
				"BranchesAllocatedToUser", "AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "LookUpsEngr", "PaymentTerm", "UomSchedule", "SupplierCustomerLists", "VendorsAndCustomersForTransporters", "WarehousesWithBranches", "AllItems",
				"MultiCurrency", "AllCities", "CropYear", "JobLot", "InvPackingType"
			};
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GetConfigurationsFromGlobal();
			SupplierDtFillFromGlobal();
			BindSupplierName();
			ReferancePartyBind(CommonServices.ReferencePartyServiceBind());
			ItemDtFillFromGlobal();
			ItemNameBind();
			CropDtFillFromGlobalAndBind();
			PackingTypeDtFillFromGlobalAndBind();
			CityDtFillFromGlobalAndBind();
			OtherItemdtDbCall();
			CurrencyBind(CurrencyDbCall());
			GetBaseCurrencyAndRate();
			BranchDetailDbCall();
			BranchDetailBind();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			Approved = false;
			RecId = 0;
			dtLastSavedRecord.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			((Control)(object)CmbSupplierDetail).Text = string.Empty;
			((Control)(object)CmbRefPartyDetail).Text = string.Empty;
			txtManualBillNo.Text = string.Empty;
			txtRemarksMain.Text = string.Empty;
			txtVehicleNoMain.Text = string.Empty;
			txtBiltyNoMain.Text = string.Empty;
			txtTransitDaysMain.Text = string.Empty;
			txtFreightMain.Text = string.Empty;
			txtOtherExpenseMain.Text = string.Empty;
			txtAdvanceFreight.Text = string.Empty;
			txtSupplierWbWtMain.Text = string.Empty;
			txtQtyMain.Text = string.Empty;
			txtTotalWeightMain.Text = string.Empty;
			txtFcyAmountMain.Text = string.Empty;
			((Control)(object)CmbCommisionAgent).Text = string.Empty;
			((Control)(object)CmbCommType).Text = string.Empty;
			txtcommRate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)CmbCommUOM).Text = string.Empty;
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			txtItemAmountFooter.Text = string.Empty;
			txtExpenseAmountFooter.Text = string.Empty;
			txtAdvanceFreightFooter.Text = string.Empty;
			txtNetBillAmountFooter.Text = string.Empty;
			txtTotalAmountMain.Text = string.Empty;
			updateDetailIndex = -1;
			((Control)(object)CmbItemNameDetail).Text = string.Empty;
			((Control)(object)CmbPackUomDetail).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEBUnit.Text = string.Empty;
			txtEBTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeightDetail.Text = string.Empty;
			((Control)(object)CmbRateUomDetail).Text = string.Empty;
			txtRateDetail.Text = string.Empty;
			txtAmountDetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			table.Clear();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Clear();
			AddRowInvExpGrid();
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			txtDocNoMain.Text = DocumentNoFill();
			txtBranchSrNo.Text = BranchDocumentNoFill();
			GetConfigurationsFromGlobal();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			txtDocDateMain.Focus();
			EnableDisableFeilds(Value: true);
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
			((Control)(object)CmbItemNameDetail).Text = string.Empty;
			((Control)(object)CmbPackUomDetail).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtEBUnit.Text = string.Empty;
			txtEBTotal.Text = string.Empty;
			txtAddLss.Text = string.Empty;
			txtNetBillWeightDetail.Text = string.Empty;
			((Control)(object)CmbRateUomDetail).Text = string.Empty;
			txtRateDetail.Text = string.Empty;
			txtAmountDetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierDispatch_FormClosing(object sender, FormClosingEventArgs e)
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

	private void EnableDisableFeilds(bool Value)
	{
		try
		{
			PanelDetail.Visible = Value;
			((Control)(object)CmbSupplierDetail).Enabled = Value;
			((Control)(object)CmbDeliveryTerm).Enabled = Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetBaseCurrencyAndRate()
	{
		try
		{
			int CurrId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Base Currency"));
			double Rate = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BaseCurrencyRate"));
			if (Conversion.ToInt(CmbCurrencyMain.Value) == 0)
			{
				CmbCurrencyMain.Value = CurrId;
			}
			if (Conversion.ToDouble((object)txtExchangeRateMain.Text) == 0.0)
			{
				txtExchangeRateMain.Text = Conversion.ToString((object)Rate);
			}
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["QTY"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtQtyMain.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtTotalWeightMain.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtFcyAmountMain.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtQtyMain.Text = "0";
				txtTotalWeightMain.Text = "0";
				txtFcyAmountMain.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRateMain.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal amount = Conversion.ToDecimal(obj.Cells["Amount"].Value) / Conversion.ToDecimal((object)txtExchangeRateMain.Text);
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
			BillAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRateMain.Text = Conversion.ToDecimal((object)txtExchangeRateMain.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)CmbPackUomDetail).ActiveRow != null && Conversion.ToDouble((object)txtQtyDetail.Text) > 0.0)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUomDetail).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtQtyDetail.Text);
				txtGrossWeight.Text = (ItemPack * Qty).ToString("#,##0.####");
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
			double Qty = Conversion.ToDouble((object)txtQtyDetail.Text.Trim().ToString());
			double num = Conversion.ToDouble((object)txtGrossWeight.Text.Trim().ToString());
			double EBUnit = Conversion.ToDouble((object)txtEBUnit.Text.Trim().ToString());
			double EBTotal = 0.0;
			double AddLess = 0.0;
			if (EBUnit * Qty > 0.0)
			{
				EBTotal = EBUnit * Qty;
				txtEBTotal.Text = EBTotal.ToString();
			}
			else
			{
				txtEBTotal.Text = "0";
			}
			AddLess = Conversion.ToDouble((object)txtAddLss.Text.Trim());
			double NetWeight = num - EBTotal + AddLess;
			txtNetBillWeightDetail.Text = Math.Round(NetWeight, 2).ToString();
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
			double TotalAmount = 0.0;
			if (Conversion.ToDouble((object)txtNetBillWeightDetail.Text) > 0.0 && ((UltraGridBase)CmbRateUomDetail).ActiveRow != null && Conversion.ToDouble((object)txtRateDetail.Text) > 0.0)
			{
				txtAmountDetail.Text = string.Empty;
				double num = Conversion.ToDouble((object)txtNetBillWeightDetail.Text);
				RateUOM = Conversion.ToDouble(((UltraDropDownBase)CmbRateUomDetail).SelectedRow.Cells[2].Value);
				Rate = Conversion.ToDouble((object)txtRateDetail.Text);
				TotalAmount = num / RateUOM * Rate;
				txtAmountDetail.Text = Math.Round(TotalAmount, 0, MidpointRounding.AwayFromZero).ToString("#,##0.####");
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
			Total();
			TotalAmount();
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
			Total();
			TotalAmount();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEBUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLss_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
			TotalAmount();
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

	private void combrateuom_TextChanged(object sender, EventArgs e)
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

	private void combrateuom_Leave(object sender, EventArgs e)
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

	private void txtAdvanceFreight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double AdvanceFreight = Conversion.ToDouble((object)txtAdvanceFreight.Text);
			double ItemAmount = GridEX_Helper.GetColumnSum(grd, "Amount");
			double ExpAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			BillAmount = ItemAmount + ExpAmount;
			if (Conversion.ToInt(CmbSupplierDetail.Value) == Conversion.ToInt(CmbCommisionAgent.Value))
			{
				BillAmount += Conversion.ToDouble((object)txtcommamount.Text.Trim());
				Conversion.ToDouble((object)txtcommamount.Text.Trim());
			}
			if (((UltraGridBase)CmbBrokeryAc).ActiveRow != null)
			{
				if (Conversion.ToDouble((object)txtBrokeryAmount.Text) > 0.0)
				{
					BillAmount -= Conversion.ToDouble((object)txtBrokeryAmount.Text.Trim());
				}
				else
				{
					txtBrokeryAmount.Text = "0";
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			BillAmount += AdvanceFreight;
			txtItemAmountFooter.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtAdvanceFreightFooter.Text = AdvanceFreight.ToString(clsGlobalVariables.stringFormatsingle);
			txtExpenseAmountFooter.Text = ExpAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalAmountMain.Text = BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtNetBillAmountFooter.Text = BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalAmount = GridEX_Helper.GetColumnSum(grdInvExp, "Amount");
			double TotalQty = GridEX_Helper.GetColumnSum(grd, "Weight");
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Qty = Conversion.ToDouble((object)obj.Cells["Weight"].Value.ToString());
					obj.Cells["Expense"].Value = (TotalAmount / TotalQty * Qty).ToString();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expense"].Value = 0;
				}
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				r.Cells["BillAmount"].Value = Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Expense"].Value.ToString()) + Conversion.ToDouble((object)r.Cells["Commission"].Value.ToString());
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionProportion()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble((object)txtcommamount.Text);
			double CommPercent = Conversion.ToDouble((object)txtcommRate.Text);
			double NetWeight = GridEX_Helper.GetColumnSum(grd, "Weight");
			if (TotalAmount > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					if (((Control)(object)CmbCommType).Text == "Percent")
					{
						double ItemAmount = Conversion.ToDouble((object)r.Cells["Amount"].Value.ToString());
						r.Cells["Commission"].Value = ItemAmount * CommPercent / 100.0;
						r.EndEdit();
					}
					else
					{
						double Weight = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
						r.Cells["Commission"].Value = (TotalAmount / NetWeight * Weight).ToString();
						r.EndEdit();
					}
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["Commission"].Value = 0;
					obj.EndEdit();
				}
			}
			BillProportion();
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
			if (txtcommRate.Text != string.Empty)
			{
				if (((Control)(object)CmbCommType).Text == "Flat")
				{
					double CommRate3 = Conversion.ToDouble((object)txtcommRate.Text.Trim());
					txtcommamount.Text = CommRate3.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtcommamount.Text = "0";
				}
			}
			if (txtcommRate.Text != string.Empty)
			{
				if (((Control)(object)CmbCommType).Text == "Percent" || ((Control)(object)CmbCommType).Text == "Percentage")
				{
					double CommRate2 = Conversion.ToDouble((object)txtcommRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
						txtcommamount.Text = CommAmt2.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
			if (txtcommRate.Text != string.Empty)
			{
				if (((Control)(object)CmbCommType).Text == "Comm Weight")
				{
					double CommRate = Conversion.ToDouble((object)txtcommRate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)CmbCommUOM).Text.Trim());
						double CommAmt = num / UOM * CommRate;
						txtcommamount.Text = CommAmt.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtcommamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		CommissionProportion();
	}

	private void combCommType_Leave(object sender, EventArgs e)
	{
		BillAmount();
		TotalCommissionAmount();
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void combcommUOM_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		BillAmount();
	}

	private void combcommAgent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbSupplierDetail.Value) == Conversion.ToInt(CmbCommisionAgent.Value))
			{
				TotalCommissionAmount();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalBrokeryAmount()
	{
		try
		{
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Flat")
				{
					double CommRate3 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					txtBrokeryAmount.Text = CommRate3.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Percent")
				{
					double CommRate2 = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
					if (total != null)
					{
						double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate2 / 100.0;
						txtBrokeryAmount.Text = CommAmt2.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			if (txtBrokeryRate.Text != string.Empty)
			{
				if (((Control)(object)CmbBrokeryType).Text == "Comm Weight")
				{
					double CommRate = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
					object Weight = grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2);
					if (Weight != null)
					{
						double num = Conversion.ToDouble((object)Weight.ToString());
						double UOM = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
						double CommAmt = num / UOM * CommRate;
						txtBrokeryAmount.Text = CommAmt.ToString(clsGlobalVariables.stringFormatsingle);
					}
				}
			}
			else
			{
				txtBrokeryAmount.Text = "0";
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryAc_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryType_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBrokeryRate_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBrokeryRateUom_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalBrokeryAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCommisionAgent_Leave(object sender, EventArgs e)
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

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			_ = ((UltraGridBase)cmbBranchName).ActiveRow;
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
				txtDocDateMain.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			DataGridHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryBranchComboDbCall();
			HistoryBranchComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 251;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
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
				reports.BranchesIds = BranchIds;
				DataTable dtGridRegister = SupplierDispatch.FormHistory(reports);
				if (dtGridRegister.Rows.Count > 0)
				{
					DataTable dtcol = new DataTable();
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("DocumentTypeId", typeof(int));
					dtcol.Columns.Add("DocNo", typeof(int));
					dtcol.Columns.Add("BranchSrNo", typeof(int));
					dtcol.Columns.Add("DocDate", typeof(DateTime));
					dtcol.Columns.Add("SupplierId", typeof(int));
					dtcol.Columns.Add("Supplier");
					dtcol.Columns.Add("ReferenceParty");
					dtcol.Columns.Add("ManualBillNo", typeof(string));
					dtcol.Columns.Add("VehicleNo", typeof(string));
					dtcol.Columns.Add("BiltyNo", typeof(string));
					dtcol.Columns.Add("DepartureDate", typeof(DateTime));
					dtcol.Columns.Add("TransitDays", typeof(int));
					dtcol.Columns.Add("ETADestination", typeof(DateTime));
					dtcol.Columns.Add("SupplierWbWeight", typeof(decimal));
					dtcol.Columns.Add("TotalQty", typeof(decimal));
					dtcol.Columns.Add("TotalWeight", typeof(decimal));
					dtcol.Columns.Add("TotalAmount", typeof(decimal));
					dtcol.Columns.Add("FcyAmount", typeof(decimal));
					dtcol.Columns.Add("Freight", typeof(decimal));
					dtcol.Columns.Add("OtherExpense", typeof(decimal));
					dtcol.Columns.Add("CityName", typeof(string));
					dtcol.Columns.Add("RemarksHeader", typeof(string));
					dtcol.Columns.Add("DeliveryTerm", typeof(string));
					dtcol.Columns.Add("Status", typeof(string));
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("EntryUser", typeof(string));
					dtcol.Columns.Add("ModifyDate", typeof(DateTime));
					dtcol.Columns.Add("ModifyUser", typeof(string));
					dtcol.Columns.Add("IsApproved", typeof(string));
					dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
					dtcol.Columns.Add("ApprovedUser", typeof(string));
					dtcol.Columns.Add("NoOfAttachments", typeof(int));
					dtcol.Rows.Clear();
					foreach (DataRow i in dtGridRegister.Rows)
					{
						dtcol.Rows.Add(i["Id"], i["DocumentTypeId"], i["DocNo"], i["BranchSrNo"], i["DocDate"], i["SupplierId"], i["SupplierName"], i["ReferencePartyName"], i["ManualBillNo"], i["VehicleNo"], i["BiltyNo"], i["DepartureDate"], i["TransitDays"], i["ETAdestination"], i["SupplierWbWeight"], i["TotalQty"], i["TotalWeight"], i["TotalAmount"], i["FcyAmount"], i["Freight"], i["OtherExpense"], i["CityName"], i["RemarksHeader"], i["DeliveryTerm"], i["Status"], i["EntryDate"], i["EntryUser"], i["ModifyDate"], i["ModifyUser"], Conversion.ToBool(i["IsApproved"]) ? "Approved" : "Not Approved", i["ApprovedDate"], i["ApprovedUser"], i["NoOfAttachments"]);
					}
					grdhistory.DataSource = dtcol;
					grdhistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdhistory.ClearStructure();
					DataGridHistoryDetail.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridHistoryStatusUpdate()
	{
		try
		{
			if (formright.DoHaveUpdateRights && grpUpdateHistoryStatus.Visible && grpUpdateHistoryStatus.Enabled)
			{
				grdhistory.RootTable.Columns["Status"].ColumnType = (ColumnType)0;
				grdhistory.RootTable.Columns["Status"].HasValueList = true;
				grdhistory.RootTable.Columns["Status"].LimitToList = true;
				grdhistory.RootTable.Columns["Status"].EditType = (EditType)4;
				grdhistory.RootTable.Columns["Status"].Caption = "Status";
				grdhistory.RootTable.Columns["Status"].ValueList.PopulateValueList((IEnumerable)dtStatus.DefaultView, "Id", "status");
				if (!grdhistory.RootTable.Columns.Contains("Select"))
				{
					grdhistory.RootTable.Columns.Add("Select");
					grdhistory.RootTable.Columns["Select"].ActAsSelector = true;
					grdhistory.RootTable.Columns["Select"].UseHeaderSelector = true;
					grdhistory.RootTable.Columns["Select"].Width = 40;
					grdhistory.RootTable.Columns["Select"].Position = 0;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryGridSettings(bool Value = false)
	{
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		try
		{
			grdhistory.AllowEdit = (InheritableBoolean)1;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["SupplierId"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdhistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdhistory.RootTable.Columns["DepartureDate"].FormatString = "dd-MMM-yyyy";
			grdhistory.RootTable.Columns["ETADestination"].FormatString = "dd-MMM-yyyy";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["BranchSrNo"].Visible = BranchFeature && !BranchImplemented;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
				column.EditType = (EditType)0;
				column.FilterEditType = (FilterEditType)1;
			}
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["BranchSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdhistory.RootTable.Columns["ManualBillNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdhistory.RootTable.Columns["Supplier"].Width = Constants.InventoryConstants.SupplierName;
			grdhistory.RootTable.Columns["ReferenceParty"].Width = Constants.InventoryConstants.ReferenceParty;
			grdhistory.RootTable.Columns["DepartureDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdhistory.RootTable.Columns["TransitDays"].Width = Constants.InventoryConstants.DueDays;
			grdhistory.RootTable.Columns["ETADestination"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdhistory.RootTable.Columns["TotalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["SupplierWbWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.FreightConstant;
			grdhistory.RootTable.Columns["OtherExpense"].Width = Constants.InventoryConstants.FreightConstant;
			grdhistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdhistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdhistory.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			GridHistoryStatusUpdate();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = CurrId;
					ReadById(RecId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.SupplierDispatchPreBillSlip251(CurrId);
				}
				if (e.Column.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadbyIdForSaveAsRecord(int CurrId)
	{
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 251);
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
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridDetailBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			SupplierDispatch po = SupplierDispatch.GetByID(Conversion.ToInt(r.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderNo");
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackingType");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("EBUnit", typeof(double));
				dt.Columns.Add("EBTotal", typeof(double));
				dt.Columns.Add("AddLss");
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Rate", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Columns.Add("ReceiverLocation");
				for (int i = 0; i < po.SupplierDispatchDetailList.Count; i++)
				{
					dt.Rows.Add(po.SupplierDispatchDetailList[i].Id, po.SupplierDispatchDetailList[i].OrderId, po.SupplierDispatchDetailList[i].OrderNo, po.SupplierDispatchDetailList[i].ItemId, po.SupplierDispatchDetailList[i].ItemCode, po.SupplierDispatchDetailList[i].ItemName, po.SupplierDispatchDetailList[i].CropYear, po.SupplierDispatchDetailList[i].PackingType, po.SupplierDispatchDetailList[i].PackUomCode, po.SupplierDispatchDetailList[i].Qty, po.SupplierDispatchDetailList[i].GrossWeight, po.SupplierDispatchDetailList[i].EBWeight, po.SupplierDispatchDetailList[i].EBTotalWt, po.SupplierDispatchDetailList[i].AdLsWeight, po.SupplierDispatchDetailList[i].WeightKgs, po.SupplierDispatchDetailList[i].RateUomCode, po.SupplierDispatchDetailList[i].Rate, po.SupplierDispatchDetailList[i].Amount, po.SupplierDispatchDetailList[i].FcyAmount, po.SupplierDispatchDetailList[i].Remarks, po.SupplierDispatchDetailList[i].BranchName);
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
			else
			{
				DataGridHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		try
		{
			DataGridHistoryDetail.RootTable.Columns["Weight"].Caption = "Net Bill Weight";
			DataGridHistoryDetail.RootTable.Columns["Id"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			DataGridHistoryDetail.RootTable.Columns["ReceiverLocation"].Visible = BranchFeature && !BranchImplemented;
			DataGridHistoryDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistoryDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			DataGridHistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			DataGridHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DataGridHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			DataGridHistoryDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			DataGridHistoryDetail.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistoryDetail.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistoryDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			DataGridHistoryDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistoryDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistoryDetail.RootTable.Columns["ReceiverLocation"].Width = Constants.InventoryConstants.BranchName;
			bool OrderExists = GridEX_Helper.IsAnyCellValueGreaterThanZero(DataGridHistoryDetail, "OrderId");
			DataGridHistoryDetail.RootTable.Columns["OrderNo"].Visible = OrderExists;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateStatusHistory_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				GridEXRow[] SelectedRows = grdhistory.GetCheckedRows();
				if (SelectedRows.Count() == 0)
				{
					throw new Exception("Please select check box first");
				}
				if (MessageBox.Show("Are you sure to Update Selected Records?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] array = SelectedRows;
					foreach (GridEXRow item in array)
					{
						ApprovalList detail = new ApprovalList();
						detail.OrganizationId = UserAccount.OrganizationId;
						detail.CompanyId = UserAccount.CompanyId;
						detail.EntryUser = UserAccount.ID;
						detail.Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
						detail.Comments = Conversion.ToString((object)item.Cells["Status"].Text);
						detail.ReqType = "UpdateStatus";
						obj.AllApprovalLists.Add(detail);
					}
					SupplierDispatch.SupplierDispatchApprove(obj);
					MessageBox.Show("Record Approved Successfully");
					gridhistoryfill();
				}
				return;
			}
			throw new Exception("You Don't have right for Update");
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grd.RootTable.Columns).Count;
			int TableColumnsCount = table.Columns.Count + 1;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(table, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(table.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
				grd.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grdhistory.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grdhistory.RootTable.Columns).Count;
			int TableColumnsCount = dtHistory.Columns.Count + 3;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtHistory, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtHistory.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
				grdhistory.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SupplierDispatchPreBillSlip251(RecId);
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

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtTransitDaysMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFreightMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtOtherExpenseMain_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtWeightDetail_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtEBUnit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAddLss_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefPartyForm_Click(object sender, EventArgs e)
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

	private void SupplierDispatch_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtDocDateMain.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					BtnRefPartyForm_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocDateMain.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnSaveAs.Visible && btnSaveAs.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItemNameDetail.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					if (formright.DoHavePrintRights)
					{
						btnprint_Click(null, null);
					}
				}
				else if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && formright.DoHaveUpdateRights && grpUpdateHistoryStatus.Enabled)
			{
				btnUpdateStatusHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnResetHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefreshHistory_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdhistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.H && grpUpdateHistoryStatus.Visible)
			{
				grpUpdateHistoryStatus.Enabled = true;
				if (grdhistory.RowCount > 0)
				{
					GridHistoryStatusUpdate();
				}
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273");
			dt.Rows.Add("Alt+1", "For Print 273");
			dt.Rows.Add("Ctrl+F1", "For Open Reference Party Form");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXColumn currentColumn = grd.CurrentColumn;
				if (((currentColumn != null) ? currentColumn.Key : null) == "Delete")
				{
					DeleteDetailRow(r);
					return;
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			else
			{
				if (e.KeyCode != Keys.F1)
				{
					return;
				}
				GridEXColumn currentColumn2 = grd.CurrentColumn;
				switch ((currentColumn2 != null) ? currentColumn2.Key : null)
				{
				case "CropYearId":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtCrop, "Id", "CropYear", r, "CropYearId");
					break;
				case "PackingTypeId":
					GridEX_Helper.OpenComboSelectionPopUpAndSetGridRow(dtPack, "Id", "PackTypeDesc", r, "PackingTypeId");
					break;
				case "PackUomId":
				case "PackUom":
				case "PackUomEquivalent":
				{
					int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (itemId <= 0)
					{
						MessageBox.Show("Please Select an Item First");
						return;
					}
					DataTable dataTable = CommonServices.dtUomFromGloablUomScheduleByItemId(itemId);
					GrdPopUp obj = new GrdPopUp(dataTable, "Id", "UOMCode")
					{
						StartPosition = FormStartPosition.CenterScreen
					};
					obj.ShowDialog();
					obj.BringToFront();
					int id = Conversion.ToInt((object)obj.ReturnId);
					string name = Conversion.ToString((object)obj.ReturnName);
					r.Cells["PackUomId"].Value = id;
					r.Cells["PackUom"].Value = name;
					DataRow foundRow = dataTable.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == id);
					r.Cells["PackUomEquivalent"].Value = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Equivalent"]) : 0m);
					UpdateCalculatedFields(r);
					break;
				}
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdInvExp.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Add")
				{
					AddRowInvExpGrid();
				}
				if (grdInvExp.CurrentColumn != null && grdInvExp.CurrentColumn.Key == "Delete")
				{
					dtInvExp.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtInvExp.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
				grdInvExp.UpdateData();
				ExpProportion();
				txtExchangeRate_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.SupplierDispatchPreBillSlip251(CuurID);
				}
				else if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "SaveAs")
				{
					ReadbyIdForSaveAsRecord(CuurID);
				}
				else if (grdhistory.CurrentColumn != null && grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 251);
				}
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				ReadbyIdForSaveAsRecord(CuurID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void txtSupplierWbWtMain_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocDateMain_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtDepartureDateMain.Value = txtDocDateMain.Value;
			ETACalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ETACalculations()
	{
		try
		{
			int Days = Conversion.ToInt((object)txtTransitDaysMain.Text);
			DateTime DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
			txtETAdestinationMain.Value = DepartureDate.AddDays(Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTransitDaysMain_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtDepartureDateMain.Value = txtDocDateMain.Value;
			ETACalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtETAdestinationMain_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime DepartureDate = Conversion.ToDateTime((object)txtDepartureDateMain.Value);
			int differenceInDays = Math.Abs((Conversion.ToDateTime((object)txtETAdestinationMain.Value) - DepartureDate).Days);
			txtTransitDaysMain.Text = differenceInDays.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPurchaseOrderLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && !GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "OrderId"))
			{
				MessageBox.Show("You cannot load an order because no valid 'OrderId' exists in the grid.");
				return;
			}
			LoadPO = new LoadPurchaseOrder(UserAccount)
			{
				DocumentTypeId = 41,
				ReturnFulldt = true
			};
			LoadPO.ShowDialog();
			LoadPO.BringToFront();
			LoadInGridDetail(LoadPO.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dtLoader)
	{
		try
		{
			if (dtLoader.Rows.Count == 0)
			{
				return;
			}
			string poIds = string.Join(",", from row in dtLoader.AsEnumerable()
				select row[0].ToString());
			DataRow firstRow = dtLoader.Rows[0];
			int previousSupplierId = Conversion.ToInt(CmbSupplierDetail.Value);
			int rowSupplierId = Conversion.ToInt(firstRow["SupplierCustomerId"]);
			string previousDeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
			string rowDeliveryTerm = Conversion.ToString(firstRow["DeliveryTerm"]);
			if (((UltraGridBase)CmbSupplierDetail).ActiveRow != null && previousSupplierId > 0 && rowSupplierId != previousSupplierId)
			{
				throw new Exception("You can't load an order from a different party at the same time.");
			}
			if (((UltraGridBase)CmbDeliveryTerm).ActiveRow != null && !string.IsNullOrWhiteSpace(previousDeliveryTerm) && rowDeliveryTerm != previousDeliveryTerm)
			{
				throw new Exception("You can't load an order with a different delivery term at the same time.");
			}
			CmbSupplierDetail.Value = firstRow["SupplierCustomerId"];
			((Control)(object)CmbDeliveryTerm).Text = firstRow["DeliveryTerm"].ToString();
			CmbBrokeryAc.Value = firstRow["BrokerAgentId"];
			((Control)(object)CmbBrokeryType).Text = firstRow["BrokeryType"].ToString();
			txtBrokeryRate.Text = firstRow["BrokeryRate"].ToString();
			((Control)(object)CmbBrokeryRateUom).Text = firstRow["BrokeryUom"].ToString();
			txtBrokeryAmount.Text = firstRow["BrokeryAmount"].ToString();
			CmbCommisionAgent.Value = firstRow["BrokerAgentSupCustId"];
			((Control)(object)CmbCommType).Text = firstRow["CommissionType"].ToString();
			txtcommRate.Text = firstRow["CommRate"].ToString();
			((Control)(object)CmbCommUOM).Text = firstRow["UomScheduleIdCmRate"].ToString();
			txtcommamount.Text = firstRow["CommAmount"].ToString();
			txtRemarksMain.Text = firstRow["RemarksHeader"].ToString();
			EnableDisableFeilds(Value: false);
			HashSet<int> existingDetailIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["OrderDetailId"].Value));
			HashSet<int> OrderIds = new HashSet<int>(from r in grd.GetRows()
				select Conversion.ToInt(r.Cells["OrderId"].Value));
			foreach (DataRow row2 in dtLoader.Rows)
			{
				int Id = Conversion.ToInt(row2["Id"]);
				if (OrderIds != null && OrderIds.Count > 0 && !OrderIds.Contains(Id))
				{
					MessageBox.Show("Data against another OrderNo Already Exist in Detail");
					return;
				}
				int detailId = Conversion.ToInt(row2["PODetailId"]);
				if (!existingDetailIds.Contains(detailId))
				{
					double num = Conversion.ToDouble(row2["GrossWeight"]);
					double emptyBagsTotal = Conversion.ToDouble(row2["EmptyBagsTotal"]);
					double netWeight = num - emptyBagsTotal;
					double rateUom = Conversion.ToDouble(row2["Equivalent"]);
					double rate = Conversion.ToDouble(row2["OrderItemRate"]);
					double amount = ((rateUom == 0.0) ? 0.0 : (netWeight / rateUom * rate));
					double exchangeRate = Conversion.ToDouble((object)txtExchangeRateMain.Text);
					double convertedAmount = ((exchangeRate > 0.0) ? (amount / exchangeRate) : 0.0);
					table.Rows.Add(0, row2["Id"], row2["DocNo"], row2["PODetailId"], row2["OrderItemId"], row2["ItemCode"], row2["ItemName"], row2["CropYearId"], 0, row2["PackUomId"], row2["PackUom"], row2["PackEquivalent"], row2["ItemQty"], row2["GrossWeight"], row2["EmptyBags"], row2["EmptyBagsTotal"], 0, netWeight, row2["RateUomId"], row2["UOMCode"], row2["Equivalent"], row2["OrderItemRate"], amount, convertedAmount, "", 0, 0, 0, 0);
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			LoadExpData(poIds);
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			TotalCommissionAmount();
			TotalBrokeryAmount();
			CommissionProportion();
			ExpProportion();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string poIds)
	{
		try
		{
			DataTable expItems = InvPurchaseInvoice.PurchaseOrderSupplierExpenseByOrderIds(poIds);
			if (expItems.Rows.Count == 0)
			{
				return;
			}
			HashSet<int> existingExpIds = new HashSet<int>(from r in grdInvExp.GetRows()
				select Conversion.ToInt(r.Cells["OrderExpId"].Value));
			HashSet<int> existingOrderIds = new HashSet<int>(from r in grdInvExp.GetRows()
				where Conversion.ToInt(r.Cells["OrderId"].Value) > 0
				select Conversion.ToInt(r.Cells["OrderId"].Value));
			foreach (DataRow row in expItems.Rows)
			{
				int orderId = Conversion.ToInt(row["PurchaseOrderId"]);
				int expId = Conversion.ToInt(row["Id"]);
				if (!existingExpIds.Contains(expId))
				{
					if (existingOrderIds.Count > 0 && !existingOrderIds.Contains(orderId))
					{
						throw new Exception("Data against another OrderNo already exists in Expense Grid.");
					}
					dtInvExp.Rows.Add(0, row["PurchaseOrderId"], row["Id"], row["InvRevExpItemId"], Conversion.ToDouble(row["Qty"]), Conversion.ToDouble(row["Rate"]), Conversion.ToDouble(row["Amount"]), Conversion.ToString(row["Remarks"]));
					existingExpIds.Add(expId);
					existingOrderIds.Add(orderId);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Expected O, but got Unknown
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Expected O, but got Unknown
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Expected O, but got Unknown
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Expected O, but got Unknown
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Expected O, but got Unknown
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Expected O, but got Unknown
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Expected O, but got Unknown
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Expected O, but got Unknown
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Expected O, but got Unknown
		//IL_0a1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a26: Expected O, but got Unknown
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Expected O, but got Unknown
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b23: Expected O, but got Unknown
		//IL_0b24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Expected O, but got Unknown
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba7: Expected O, but got Unknown
		//IL_0d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d54: Expected O, but got Unknown
		//IL_0d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d75: Expected O, but got Unknown
		//IL_0def: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Expected O, but got Unknown
		//IL_15b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c2: Expected O, but got Unknown
		//IL_15cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Expected O, but got Unknown
		//IL_759b: Unknown result type (might be due to invalid IL or missing references)
		//IL_75a5: Expected O, but got Unknown
		//IL_75b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_75bc: Expected O, but got Unknown
		//IL_b8e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_b8ee: Expected O, but got Unknown
		//IL_b912: Unknown result type (might be due to invalid IL or missing references)
		//IL_b91c: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
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
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
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
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance98 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.frmSupplierDispatchPreBill));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.txtAdvanceFreightFooter = new System.Windows.Forms.TextBox();
		this.txtNetBillAmountFooter = new System.Windows.Forms.TextBox();
		this.CmbCommisionAgent = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.CmbCommType = new UltraCombo();
		this.CmbBrokeryType = new UltraCombo();
		this.txtcommRate = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.label85 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label84 = new System.Windows.Forms.Label();
		this.CmbCommUOM = new UltraCombo();
		this.label83 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtExpenseAmountFooter = new System.Windows.Forms.TextBox();
		this.txtItemAmountFooter = new System.Windows.Forms.TextBox();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.label74 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.label67 = new System.Windows.Forms.Label();
		this.CmbBrokeryAc = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.PanelDetail = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label77 = new System.Windows.Forms.Label();
		this.txtAddLss = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.txtEBTotal = new System.Windows.Forms.TextBox();
		this.label79 = new System.Windows.Forms.Label();
		this.txtEBUnit = new System.Windows.Forms.TextBox();
		this.label80 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.CmbBranchDetail = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.CmbPackingTypeDetail = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbRateUomDetail = new UltraCombo();
		this.CmbCropYearDetail = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbPackUomDetail = new UltraCombo();
		this.CmbItemNameDetail = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRemarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRateDetail = new System.Windows.Forms.TextBox();
		this.txtAmountDetail = new System.Windows.Forms.TextBox();
		this.txtNetBillWeightDetail = new System.Windows.Forms.TextBox();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label81 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtTotalAmountMain = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtTotalWeightMain = new System.Windows.Forms.TextBox();
		this.CmbRefPartyDetail = new UltraCombo();
		this.CmbSupplierDetail = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtVehicleNoMain = new System.Windows.Forms.TextBox();
		this.CmbDeliveryTerm = new UltraCombo();
		this.CmbCityNameMain = new UltraCombo();
		this.txtAdvanceFreight = new System.Windows.Forms.TextBox();
		this.txtOtherExpenseMain = new System.Windows.Forms.TextBox();
		this.txtETAdestinationMain = new System.Windows.Forms.DateTimePicker();
		this.txtFreightMain = new System.Windows.Forms.TextBox();
		this.txtExchangeRateMain = new System.Windows.Forms.TextBox();
		this.txtSupplierWbWtMain = new System.Windows.Forms.TextBox();
		this.txtQtyMain = new System.Windows.Forms.TextBox();
		this.txtFcyAmountMain = new System.Windows.Forms.TextBox();
		this.txtTransitDaysMain = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCurrencyMain = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.txtDepartureDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtDocDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksMain = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtDocNoMain = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtManualBillNo = new System.Windows.Forms.TextBox();
		this.txtBiltyNoMain = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnRefPartyForm = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label49 = new System.Windows.Forms.Label();
		this.DataGridHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label55 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grpUpdateHistoryStatus = new System.Windows.Forms.GroupBox();
		this.label87 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.btnUpdateStatusHistory = new System.Windows.Forms.Button();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnPurchaseOrderLoader = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommisionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		this.PanelDetail.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefPartyDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyMain).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.grpUpdateHistoryStatus.SuspendLayout();
		this.panel14.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1288, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1288, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel15);
		this.tabPage1.Controls.Add(this.PanelDetail);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1280, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(2, 338);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1276, 182);
		this.tabControl2.TabIndex = 4;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabDetail.Controls.Add(this.panel6);
		this.tabDetail.Controls.Add(this.panel10);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1268, 153);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1262, 122);
		this.panel6.TabIndex = 3;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1262, 122);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
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
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1262, 25);
		this.panel10.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1221, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel15.BackColor = System.Drawing.Color.White;
		this.panel15.Controls.Add(this.txtAdvanceFreightFooter);
		this.panel15.Controls.Add(this.txtNetBillAmountFooter);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommisionAgent);
		this.panel15.Controls.Add(this.label30);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommType);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.panel15.Controls.Add(this.txtcommRate);
		this.panel15.Controls.Add(this.label70);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.panel15.Controls.Add(this.label85);
		this.panel15.Controls.Add(this.label68);
		this.panel15.Controls.Add(this.label84);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommUOM);
		this.panel15.Controls.Add(this.label83);
		this.panel15.Controls.Add(this.label82);
		this.panel15.Controls.Add(this.label72);
		this.panel15.Controls.Add(this.txtcommamount);
		this.panel15.Controls.Add(this.txtExpenseAmountFooter);
		this.panel15.Controls.Add(this.txtItemAmountFooter);
		this.panel15.Controls.Add(this.txtBrokeryAmount);
		this.panel15.Controls.Add(this.label74);
		this.panel15.Controls.Add(this.label71);
		this.panel15.Controls.Add(this.txtBrokeryRate);
		this.panel15.Controls.Add(this.label67);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.panel15.Controls.Add(this.label75);
		this.panel15.Controls.Add(this.label73);
		this.panel15.Controls.Add(this.label69);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel15.Location = new System.Drawing.Point(2, 520);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1276, 160);
		this.panel15.TabIndex = 0;
		this.txtAdvanceFreightFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceFreightFooter.Enabled = false;
		this.txtAdvanceFreightFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdvanceFreightFooter.Location = new System.Drawing.Point(918, 107);
		this.txtAdvanceFreightFooter.Name = "txtAdvanceFreightFooter";
		this.txtAdvanceFreightFooter.Size = new System.Drawing.Size(146, 23);
		this.txtAdvanceFreightFooter.TabIndex = 11;
		this.txtAdvanceFreightFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetBillAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillAmountFooter.Enabled = false;
		this.txtNetBillAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillAmountFooter.Location = new System.Drawing.Point(918, 132);
		this.txtNetBillAmountFooter.Name = "txtNetBillAmountFooter";
		this.txtNetBillAmountFooter.Size = new System.Drawing.Size(146, 23);
		this.txtNetBillAmountFooter.TabIndex = 12;
		this.txtNetBillAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbCommisionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommisionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommisionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommisionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommisionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommisionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).Location = new System.Drawing.Point(88, 27);
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).Name = "CmbCommisionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).TextChanged += new System.EventHandler(combcommAgent_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbCommisionAgent).Leave += new System.EventHandler(CmbCommisionAgent_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(6, 33);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(78, 15);
		this.label30.TabIndex = 227;
		this.label30.Text = "Comm Agent";
		this.CmbCommType.AllowNull = (DefaultableBoolean)1;
		this.CmbCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommType).Location = new System.Drawing.Point(412, 27);
		((System.Windows.Forms.Control)(object)this.CmbCommType).Name = "CmbCommType";
		((System.Windows.Forms.Control)(object)this.CmbCommType).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCommType).TextChanged += new System.EventHandler(combCommType_Leave);
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance203;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance209).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(412, 55);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Leave += new System.EventHandler(CmbBrokeryType_Leave);
		this.txtcommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommRate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommRate.Location = new System.Drawing.Point(541, 29);
		this.txtcommRate.Name = "txtcommRate";
		this.txtcommRate.Size = new System.Drawing.Size(100, 23);
		this.txtcommRate.TabIndex = 2;
		this.txtcommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommRate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(510, 33);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(30, 15);
		this.label70.TabIndex = 230;
		this.label70.Text = "Rate";
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(682, 55);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Leave += new System.EventHandler(CmbBrokeryRateUom_Leave);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label85.Location = new System.Drawing.Point(821, 111);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(93, 15);
		this.label85.TabIndex = 37;
		this.label85.Text = "Advance Freight";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(337, 33);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(71, 15);
		this.label68.TabIndex = 225;
		this.label68.Text = "Comm Type";
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label84.Location = new System.Drawing.Point(821, 136);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(92, 15);
		this.label84.TabIndex = 37;
		this.label84.Text = "Net Bill Amount";
		this.CmbCommUOM.AllowNull = (DefaultableBoolean)1;
		this.CmbCommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).Location = new System.Drawing.Point(682, 27);
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).Name = "CmbCommUOM";
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCommUOM).Leave += new System.EventHandler(combcommUOM_Leave);
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label83.Location = new System.Drawing.Point(821, 86);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(97, 15);
		this.label83.TabIndex = 239;
		this.label83.Text = "Expense Amount";
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label82.Location = new System.Drawing.Point(821, 8);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(79, 15);
		this.label82.TabIndex = 239;
		this.label82.Text = "Item Amount";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(821, 61);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(94, 15);
		this.label72.TabIndex = 239;
		this.label72.Text = "Brokery Amount";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(918, 29);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.Size = new System.Drawing.Size(146, 23);
		this.txtcommamount.TabIndex = 4;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExpenseAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtExpenseAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpenseAmountFooter.Enabled = false;
		this.txtExpenseAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpenseAmountFooter.Location = new System.Drawing.Point(918, 82);
		this.txtExpenseAmountFooter.Name = "txtExpenseAmountFooter";
		this.txtExpenseAmountFooter.Size = new System.Drawing.Size(146, 23);
		this.txtExpenseAmountFooter.TabIndex = 10;
		this.txtExpenseAmountFooter.TabStop = false;
		this.txtExpenseAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountFooter.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountFooter.Enabled = false;
		this.txtItemAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmountFooter.Location = new System.Drawing.Point(918, 4);
		this.txtItemAmountFooter.Name = "txtItemAmountFooter";
		this.txtItemAmountFooter.Size = new System.Drawing.Size(146, 23);
		this.txtItemAmountFooter.TabIndex = 9;
		this.txtItemAmountFooter.TabStop = false;
		this.txtItemAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Enabled = false;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(918, 57);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.Size = new System.Drawing.Size(146, 23);
		this.txtBrokeryAmount.TabIndex = 9;
		this.txtBrokeryAmount.TabStop = false;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(510, 61);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(30, 15);
		this.label74.TabIndex = 240;
		this.label74.Text = "Rate";
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(646, 33);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(35, 15);
		this.label71.TabIndex = 226;
		this.label71.Text = "UOM";
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBrokeryRate.Location = new System.Drawing.Point(541, 57);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(100, 23);
		this.txtBrokeryRate.TabIndex = 7;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_Leave);
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(821, 33);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(90, 15);
		this.label67.TabIndex = 229;
		this.label67.Text = "Comm Amount";
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance3;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(88, 55);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Leave += new System.EventHandler(CmbBrokeryAc_Leave);
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(646, 61);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(35, 15);
		this.label75.TabIndex = 237;
		this.label75.Text = "UOM";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(337, 61);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(75, 15);
		this.label73.TabIndex = 236;
		this.label73.Text = "Brokery Type";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(6, 61);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(82, 15);
		this.label69.TabIndex = 238;
		this.label69.Text = "Brokery Agent";
		this.PanelDetail.BackColor = System.Drawing.Color.White;
		this.PanelDetail.Controls.Add(this.groupBox3);
		this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.PanelDetail.Location = new System.Drawing.Point(2, 224);
		this.PanelDetail.Margin = new System.Windows.Forms.Padding(2);
		this.PanelDetail.Name = "PanelDetail";
		this.PanelDetail.Size = new System.Drawing.Size(1276, 114);
		this.PanelDetail.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label77);
		this.groupBox3.Controls.Add(this.txtAddLss);
		this.groupBox3.Controls.Add(this.label78);
		this.groupBox3.Controls.Add(this.txtEBTotal);
		this.groupBox3.Controls.Add(this.label79);
		this.groupBox3.Controls.Add(this.txtEBUnit);
		this.groupBox3.Controls.Add(this.label80);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label58);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchDetail);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearDetail);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUomDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameDetail);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtRemarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtRateDetail);
		this.groupBox3.Controls.Add(this.txtAmountDetail);
		this.groupBox3.Controls.Add(this.txtNetBillWeightDetail);
		this.groupBox3.Controls.Add(this.txtQtyDetail);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1233, 110);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(766, 36);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(101, 23);
		this.txtGrossWeight.TabIndex = 6;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtGrossWeight_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(766, 17);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(78, 15);
		this.label77.TabIndex = 226;
		this.label77.Text = "Gross Weight";
		this.txtAddLss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLss.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLss.Location = new System.Drawing.Point(1018, 36);
		this.txtAddLss.Name = "txtAddLss";
		this.txtAddLss.Size = new System.Drawing.Size(63, 23);
		this.txtAddLss.TabIndex = 9;
		this.txtAddLss.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLss.TextChanged += new System.EventHandler(txtAddLss_TextChanged);
		this.txtAddLss.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLss_KeyPress);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(1018, 17);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(42, 15);
		this.label78.TabIndex = 225;
		this.label78.Text = "AD/LS";
		this.txtEBTotal.BackColor = System.Drawing.Color.White;
		this.txtEBTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEBTotal.Enabled = false;
		this.txtEBTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBTotal.Location = new System.Drawing.Point(935, 36);
		this.txtEBTotal.Name = "txtEBTotal";
		this.txtEBTotal.Size = new System.Drawing.Size(81, 23);
		this.txtEBTotal.TabIndex = 8;
		this.txtEBTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(935, 17);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(49, 15);
		this.label79.TabIndex = 224;
		this.label79.Text = "EB Total";
		this.txtEBUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEBUnit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEBUnit.Location = new System.Drawing.Point(869, 36);
		this.txtEBUnit.Name = "txtEBUnit";
		this.txtEBUnit.Size = new System.Drawing.Size(64, 23);
		this.txtEBUnit.TabIndex = 7;
		this.txtEBUnit.TextChanged += new System.EventHandler(txtEBUnit_TextChanged);
		this.txtEBUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEBUnit_KeyPress);
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.Location = new System.Drawing.Point(869, 17);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(45, 15);
		this.label80.TabIndex = 223;
		this.label80.Text = "EB Unit";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(1102, 63);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(14, 17);
		this.label63.TabIndex = 218;
		this.label63.Text = "*";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Red;
		this.label62.Location = new System.Drawing.Point(315, 63);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(14, 17);
		this.label62.TabIndex = 218;
		this.label62.Text = "*";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(161, 63);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(14, 17);
		this.label61.TabIndex = 218;
		this.label61.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(73, 63);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 218;
		this.label60.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(1170, 16);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 218;
		this.label59.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(739, 16);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 17);
		this.label58.TabIndex = 218;
		this.label58.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(641, 16);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(14, 17);
		this.label57.TabIndex = 218;
		this.label57.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(541, 16);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(14, 17);
		this.label56.TabIndex = 218;
		this.label56.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(427, 16);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 218;
		this.label50.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(274, 16);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 218;
		this.label46.Text = "*";
		this.CmbBranchDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Location = new System.Drawing.Point(935, 81);
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Name = "CmbBranchDetail";
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchDetail).TabIndex = 15;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(935, 64);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(108, 15);
		this.label8.TabIndex = 55;
		this.label8.Text = "Receiver's Location";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(107, 15);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 52;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(48, 15);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 51;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.CmbPackingTypeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingTypeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingTypeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingTypeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance206).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingTypeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingTypeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Location = new System.Drawing.Point(447, 34);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Name = "CmbPackingTypeDetail";
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingTypeDetail).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(447, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 15);
		this.label1.TabIndex = 34;
		this.label1.Text = "Pack Type";
		this.CmbRateUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Location = new System.Drawing.Point(104, 81);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Name = "CmbRateUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbRateUomDetail).Leave += new System.EventHandler(combrateuom_Leave);
		this.CmbCropYearDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance150;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Location = new System.Drawing.Point(331, 34);
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Name = "CmbCropYearDetail";
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearDetail).TabIndex = 2;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1132, 61);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(63, 21);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbPackUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Location = new System.Drawing.Point(576, 34);
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Name = "CmbPackUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Size = new System.Drawing.Size(95, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUomDetail).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemNameDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance199;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Location = new System.Drawing.Point(3, 34);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Name = "CmbItemNameDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).TabIndex = 1;
		this.CmbItemNameDetail.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemNameDetail).Leave += new System.EventHandler(combitem_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1132, 84);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 21);
		this.btnplus.TabIndex = 16;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRemarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksdetail.Location = new System.Drawing.Point(331, 83);
		this.txtRemarksdetail.Name = "txtRemarksdetail";
		this.txtRemarksdetail.Size = new System.Drawing.Size(602, 23);
		this.txtRemarksdetail.TabIndex = 14;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(331, 64);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtRateDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateDetail.Location = new System.Drawing.Point(3, 83);
		this.txtRateDetail.Name = "txtRateDetail";
		this.txtRateDetail.Size = new System.Drawing.Size(99, 23);
		this.txtRateDetail.TabIndex = 11;
		this.txtRateDetail.Text = "0";
		this.txtRateDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateDetail.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRateDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtAmountDetail.BackColor = System.Drawing.Color.White;
		this.txtAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountDetail.Enabled = false;
		this.txtAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountDetail.Location = new System.Drawing.Point(201, 83);
		this.txtAmountDetail.Name = "txtAmountDetail";
		this.txtAmountDetail.Size = new System.Drawing.Size(128, 23);
		this.txtAmountDetail.TabIndex = 13;
		this.txtAmountDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetBillWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeightDetail.Enabled = false;
		this.txtNetBillWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetBillWeightDetail.Location = new System.Drawing.Point(1083, 36);
		this.txtNetBillWeightDetail.Name = "txtNetBillWeightDetail";
		this.txtNetBillWeightDetail.Size = new System.Drawing.Size(112, 23);
		this.txtNetBillWeightDetail.TabIndex = 10;
		this.txtNetBillWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetBillWeightDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightDetail_KeyPress);
		this.txtQtyDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(674, 36);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(90, 23);
		this.txtQtyDetail.TabIndex = 5;
		this.txtQtyDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyDetail.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQtyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(3, 64);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(30, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Rate";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(201, 64);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Amount";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(331, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Crop Year";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(104, 64);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(61, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate UOM";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(1083, 17);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(87, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Net Bill Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(675, 17);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(576, 17);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(3, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1132, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(63, 21);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 51);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1276, 173);
		this.panel4.TabIndex = 0;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel16.Controls.Add(this.panel11);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(922, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(348, 173);
		this.panel16.TabIndex = 1;
		this.grdInvExp.AutomaticSort = false;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(348, 153);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label81);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(348, 20);
		this.panel11.TabIndex = 36;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label81.ForeColor = System.Drawing.Color.White;
		this.label81.Location = new System.Drawing.Point(3, 3);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(337, 14);
		this.label81.TabIndex = 25;
		this.label81.Text = "Other Expenses Credit to Supplier & Charge to Product";
		this.groupBox1.Controls.Add(this.txtTotalAmountMain);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtTotalWeightMain);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierDetail);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label76);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label66);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label65);
		this.groupBox1.Controls.Add(this.label64);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtVehicleNoMain);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCityNameMain);
		this.groupBox1.Controls.Add(this.txtAdvanceFreight);
		this.groupBox1.Controls.Add(this.txtOtherExpenseMain);
		this.groupBox1.Controls.Add(this.txtETAdestinationMain);
		this.groupBox1.Controls.Add(this.txtFreightMain);
		this.groupBox1.Controls.Add(this.txtExchangeRateMain);
		this.groupBox1.Controls.Add(this.txtSupplierWbWtMain);
		this.groupBox1.Controls.Add(this.txtQtyMain);
		this.groupBox1.Controls.Add(this.txtFcyAmountMain);
		this.groupBox1.Controls.Add(this.txtTransitDaysMain);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencyMain);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtDepartureDateMain);
		this.groupBox1.Controls.Add(this.txtDocDateMain);
		this.groupBox1.Controls.Add(this.txtRemarksMain);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add(this.txtDocNoMain);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtManualBillNo);
		this.groupBox1.Controls.Add(this.txtBiltyNoMain);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(922, 173);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtTotalAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmountMain.Enabled = false;
		this.txtTotalAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmountMain.Location = new System.Drawing.Point(802, 67);
		this.txtTotalAmountMain.Name = "txtTotalAmountMain";
		this.txtTotalAmountMain.Size = new System.Drawing.Size(118, 23);
		this.txtTotalAmountMain.TabIndex = 20;
		this.txtTotalAmountMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(793, 70);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 218;
		this.label42.Text = "*";
		this.txtTotalWeightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWeightMain.Enabled = false;
		this.txtTotalWeightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalWeightMain.Location = new System.Drawing.Point(802, 40);
		this.txtTotalWeightMain.Name = "txtTotalWeightMain";
		this.txtTotalWeightMain.Size = new System.Drawing.Size(118, 23);
		this.txtTotalWeightMain.TabIndex = 19;
		this.txtTotalWeightMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbRefPartyDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbRefPartyDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefPartyDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefPartyDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefPartyDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefPartyDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Location = new System.Drawing.Point(89, 92);
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Name = "CmbRefPartyDetail";
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefPartyDetail).TabIndex = 4;
		this.CmbSupplierDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance151;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Location = new System.Drawing.Point(89, 65);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Name = "CmbSupplierDetail";
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Size = new System.Drawing.Size(188, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDetail).Leave += new System.EventHandler(CmbSupplierDetail_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(2, 98);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(89, 15);
		this.label18.TabIndex = 55;
		this.label18.Text = "Reference Party";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.ForeColor = System.Drawing.Color.Red;
		this.label76.Location = new System.Drawing.Point(59, 43);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(14, 17);
		this.label76.TabIndex = 218;
		this.label76.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(59, 70);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 218;
		this.label45.Text = "*";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(2, 71);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(51, 15);
		this.label10.TabIndex = 55;
		this.label10.Text = "Supplier";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(717, 71);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(80, 15);
		this.label22.TabIndex = 37;
		this.label22.Text = "Total Amount";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(717, 152);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 15);
		this.label48.TabIndex = 43;
		this.label48.Text = "Fcy Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(717, 98);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(55, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Fcy Code";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(717, 125);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 42;
		this.label16.Text = "Echange Rate";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(499, 125);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(89, 15);
		this.label66.TabIndex = 3;
		this.label66.Text = "SupplierWb Wt";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(717, 44);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(75, 15);
		this.label24.TabIndex = 3;
		this.label24.Text = "Total Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(717, 17);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(55, 15);
		this.label21.TabIndex = 3;
		this.label21.Text = "Total Qty";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(573, 124);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 17);
		this.label65.TabIndex = 218;
		this.label65.Text = "*";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Red;
		this.label64.Location = new System.Drawing.Point(789, 43);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(14, 17);
		this.label64.TabIndex = 218;
		this.label64.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(787, 97);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 218;
		this.label43.Text = "*";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(787, 16);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(14, 17);
		this.label35.TabIndex = 218;
		this.label35.Text = "*";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(574, 151);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 17);
		this.label32.TabIndex = 218;
		this.label32.Text = "*";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(573, 97);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 218;
		this.label34.Text = "*";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(350, 97);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 17);
		this.label31.TabIndex = 218;
		this.label31.Text = "*";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(59, 16);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 218;
		this.label19.Text = "*";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(346, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(14, 17);
		this.label29.TabIndex = 218;
		this.label29.Text = "*";
		this.txtVehicleNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNoMain.Location = new System.Drawing.Point(370, 13);
		this.txtVehicleNoMain.Name = "txtVehicleNoMain";
		this.txtVehicleNoMain.Size = new System.Drawing.Size(125, 23);
		this.txtVehicleNoMain.TabIndex = 7;
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(589, 146);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 17;
		this.CmbCityNameMain.AllowNull = (DefaultableBoolean)1;
		this.CmbCityNameMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCityNameMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCityNameMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCityNameMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCityNameMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Location = new System.Drawing.Point(589, 92);
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Name = "CmbCityNameMain";
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbCityNameMain).TabIndex = 15;
		this.txtAdvanceFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdvanceFreight.Location = new System.Drawing.Point(589, 67);
		this.txtAdvanceFreight.Name = "txtAdvanceFreight";
		this.txtAdvanceFreight.Size = new System.Drawing.Size(123, 23);
		this.txtAdvanceFreight.TabIndex = 14;
		this.txtAdvanceFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdvanceFreight.TextChanged += new System.EventHandler(txtAdvanceFreight_TextChanged);
		this.txtAdvanceFreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherExpenseMain_KeyPress);
		this.txtOtherExpenseMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherExpenseMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOtherExpenseMain.Location = new System.Drawing.Point(589, 40);
		this.txtOtherExpenseMain.Name = "txtOtherExpenseMain";
		this.txtOtherExpenseMain.Size = new System.Drawing.Size(123, 23);
		this.txtOtherExpenseMain.TabIndex = 13;
		this.txtOtherExpenseMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherExpenseMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherExpenseMain_KeyPress);
		this.txtETAdestinationMain.CustomFormat = "dd-MMM-yy";
		this.txtETAdestinationMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtETAdestinationMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETAdestinationMain.Location = new System.Drawing.Point(370, 121);
		this.txtETAdestinationMain.Name = "txtETAdestinationMain";
		this.txtETAdestinationMain.Size = new System.Drawing.Size(125, 23);
		this.txtETAdestinationMain.TabIndex = 11;
		this.txtETAdestinationMain.TabStop = false;
		this.txtETAdestinationMain.ValueChanged += new System.EventHandler(txtETAdestinationMain_ValueChanged);
		this.txtFreightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreightMain.Location = new System.Drawing.Point(589, 13);
		this.txtFreightMain.Name = "txtFreightMain";
		this.txtFreightMain.Size = new System.Drawing.Size(123, 23);
		this.txtFreightMain.TabIndex = 12;
		this.txtFreightMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightMain_KeyPress);
		this.txtExchangeRateMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRateMain.Location = new System.Drawing.Point(802, 121);
		this.txtExchangeRateMain.Name = "txtExchangeRateMain";
		this.txtExchangeRateMain.Size = new System.Drawing.Size(118, 23);
		this.txtExchangeRateMain.TabIndex = 22;
		this.txtExchangeRateMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRateMain.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRateMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRateMain.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtSupplierWbWtMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWbWtMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSupplierWbWtMain.Location = new System.Drawing.Point(589, 121);
		this.txtSupplierWbWtMain.Name = "txtSupplierWbWtMain";
		this.txtSupplierWbWtMain.Size = new System.Drawing.Size(123, 23);
		this.txtSupplierWbWtMain.TabIndex = 16;
		this.txtSupplierWbWtMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupplierWbWtMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierWbWtMain_KeyPress);
		this.txtQtyMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyMain.Enabled = false;
		this.txtQtyMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQtyMain.Location = new System.Drawing.Point(802, 13);
		this.txtQtyMain.Name = "txtQtyMain";
		this.txtQtyMain.Size = new System.Drawing.Size(118, 23);
		this.txtQtyMain.TabIndex = 18;
		this.txtQtyMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFcyAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountMain.Enabled = false;
		this.txtFcyAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountMain.Location = new System.Drawing.Point(802, 148);
		this.txtFcyAmountMain.Name = "txtFcyAmountMain";
		this.txtFcyAmountMain.Size = new System.Drawing.Size(118, 23);
		this.txtFcyAmountMain.TabIndex = 23;
		this.txtFcyAmountMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransitDaysMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransitDaysMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTransitDaysMain.Location = new System.Drawing.Point(370, 94);
		this.txtTransitDaysMain.Name = "txtTransitDaysMain";
		this.txtTransitDaysMain.Size = new System.Drawing.Size(125, 23);
		this.txtTransitDaysMain.TabIndex = 10;
		this.txtTransitDaysMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransitDaysMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTransitDaysMain_KeyPress);
		this.txtTransitDaysMain.Leave += new System.EventHandler(txtTransitDaysMain_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(499, 152);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(80, 15);
		this.label27.TabIndex = 34;
		this.label27.Text = "Delivery Term";
		this.CmbCurrencyMain.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencyMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencyMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencyMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance177;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance185).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencyMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCurrencyMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Location = new System.Drawing.Point(802, 92);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Name = "CmbCurrencyMain";
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencyMain).TabIndex = 21;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(499, 98);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(62, 15);
		this.label28.TabIndex = 34;
		this.label28.Text = "City Name";
		this.txtDepartureDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDepartureDateMain.Enabled = false;
		this.txtDepartureDateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDepartureDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDepartureDateMain.Location = new System.Drawing.Point(370, 67);
		this.txtDepartureDateMain.Name = "txtDepartureDateMain";
		this.txtDepartureDateMain.Size = new System.Drawing.Size(125, 23);
		this.txtDepartureDateMain.TabIndex = 9;
		this.txtDepartureDateMain.TabStop = false;
		this.txtDocDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDocDateMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateMain.Location = new System.Drawing.Point(89, 40);
		this.txtDocDateMain.Name = "txtDocDateMain";
		this.txtDocDateMain.Size = new System.Drawing.Size(188, 23);
		this.txtDocDateMain.TabIndex = 2;
		this.txtDocDateMain.TabStop = false;
		this.txtDocDateMain.ValueChanged += new System.EventHandler(txtDocDateMain_ValueChanged);
		this.txtRemarksMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksMain.Location = new System.Drawing.Point(89, 148);
		this.txtRemarksMain.Name = "txtRemarksMain";
		this.txtRemarksMain.Size = new System.Drawing.Size(406, 23);
		this.txtRemarksMain.TabIndex = 6;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(279, 125);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 15);
		this.label11.TabIndex = 3;
		this.label11.Text = "ETA Destination";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(279, 71);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(88, 15);
		this.label9.TabIndex = 3;
		this.label9.Text = "Departure Date";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(195, 13);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(82, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TabStop = false;
		this.txtBranchSrNo.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.txtDocNoMain.BackColor = System.Drawing.Color.White;
		this.txtDocNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoMain.Enabled = false;
		this.txtDocNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNoMain.Location = new System.Drawing.Point(89, 13);
		this.txtDocNoMain.Name = "txtDocNoMain";
		this.txtDocNoMain.Size = new System.Drawing.Size(105, 23);
		this.txtDocNoMain.TabIndex = 0;
		this.txtDocNoMain.TabStop = false;
		this.txtDocNoMain.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(2, 44);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 15);
		this.label13.TabIndex = 3;
		this.label13.Text = "Doc Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(2, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(499, 71);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(93, 15);
		this.label14.TabIndex = 3;
		this.label14.Text = "Advance Freight";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(2, 152);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(499, 44);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(83, 15);
		this.label26.TabIndex = 3;
		this.label26.Text = "Other Expense";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(499, 17);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(44, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Freight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(279, 17);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 3;
		this.label12.Text = "Vehicle No";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(2, 125);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(42, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Bill No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(279, 44);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(49, 15);
		this.label7.TabIndex = 3;
		this.label7.Text = "Bilty No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(279, 98);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(71, 15);
		this.label6.TabIndex = 3;
		this.label6.Text = "Transit Days";
		this.txtManualBillNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualBillNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtManualBillNo.Location = new System.Drawing.Point(89, 121);
		this.txtManualBillNo.Name = "txtManualBillNo";
		this.txtManualBillNo.Size = new System.Drawing.Size(188, 23);
		this.txtManualBillNo.TabIndex = 5;
		this.txtBiltyNoMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNoMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBiltyNoMain.Location = new System.Drawing.Point(370, 40);
		this.txtBiltyNoMain.Name = "txtBiltyNoMain";
		this.txtBiltyNoMain.Size = new System.Drawing.Size(125, 23);
		this.txtBiltyNoMain.TabIndex = 8;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1276, 24);
		this.panel3.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(252, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Supplier Dispatch And Pre Bill";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1152, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1276, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnupdate, this.btnDelete, this.btnattachment, this.btnprint, this.btnPurchaseOrderLoader, this.BtnRefPartyForm,
			this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1276, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "251-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.BtnRefPartyForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefPartyForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnRefPartyForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefPartyForm.Name = "BtnRefPartyForm";
		this.BtnRefPartyForm.Size = new System.Drawing.Size(137, 24);
		this.BtnRefPartyForm.Text = "Reference Party";
		this.BtnRefPartyForm.Click += new System.EventHandler(BtnRefPartyForm_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1280, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel13);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1276, 678);
		this.panel7.TabIndex = 0;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 141);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1276, 360);
		this.panel12.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1276, 360);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label49);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(0, 501);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1276, 19);
		this.panel13.TabIndex = 49;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(2, 3);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(269, 14);
		this.label49.TabIndex = 0;
		this.label49.Text = "Detail Information Of Above Selected Row";
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 520);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1276, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 0;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label55);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 119);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1276, 22);
		this.panel9.TabIndex = 47;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(1, 1);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(132, 18);
		this.label55.TabIndex = 0;
		this.label55.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1227, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.grpUpdateHistoryStatus);
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Controls.Add(this.groupBox2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 27);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1276, 92);
		this.panel8.TabIndex = 0;
		this.grpUpdateHistoryStatus.Controls.Add(this.label87);
		this.grpUpdateHistoryStatus.Controls.Add(this.label86);
		this.grpUpdateHistoryStatus.Controls.Add(this.btnUpdateStatusHistory);
		this.grpUpdateHistoryStatus.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.grpUpdateHistoryStatus.Location = new System.Drawing.Point(887, 25);
		this.grpUpdateHistoryStatus.Name = "grpUpdateHistoryStatus";
		this.grpUpdateHistoryStatus.Size = new System.Drawing.Size(383, 66);
		this.grpUpdateHistoryStatus.TabIndex = 1;
		this.grpUpdateHistoryStatus.TabStop = false;
		this.grpUpdateHistoryStatus.Visible = false;
		this.label87.AutoSize = true;
		this.label87.BackColor = System.Drawing.Color.White;
		this.label87.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label87.Location = new System.Drawing.Point(172, 2);
		this.label87.Name = "label87";
		this.label87.Size = new System.Drawing.Size(115, 14);
		this.label87.TabIndex = 0;
		this.label87.Text = "To Update Status";
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.label86.ForeColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label86.Location = new System.Drawing.Point(3, 11);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(310, 52);
		this.label86.TabIndex = 17;
		this.label86.Text = resources.GetString("label86.Text");
		this.btnUpdateStatusHistory.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateStatusHistory.FlatAppearance.BorderSize = 0;
		this.btnUpdateStatusHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateStatusHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateStatusHistory.ForeColor = System.Drawing.Color.White;
		this.btnUpdateStatusHistory.Location = new System.Drawing.Point(311, 22);
		this.btnUpdateStatusHistory.Name = "btnUpdateStatusHistory";
		this.btnUpdateStatusHistory.Size = new System.Drawing.Size(67, 40);
		this.btnUpdateStatusHistory.TabIndex = 0;
		this.btnUpdateStatusHistory.TabStop = false;
		this.btnUpdateStatusHistory.Text = "Update Status";
		this.btnUpdateStatusHistory.UseVisualStyleBackColor = false;
		this.btnUpdateStatusHistory.Click += new System.EventHandler(btnUpdateStatusHistory_Click);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label5);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1276, 25);
		this.panel14.TabIndex = 50;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(214, 19);
		this.label5.TabIndex = 0;
		this.label5.Text = "Supplier Dispatch History";
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1, 25);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(886, 66);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(6, 34);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(782, 38);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(782, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(701, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(701, 38);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(640, 35);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 13;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(558, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 9;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(479, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 7;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(337, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 5;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(194, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 3;
		this.label54.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(561, 36);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 10;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(481, 36);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 8;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(339, 36);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(140, 23);
		this.ToDateHistory.TabIndex = 6;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(197, 36);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(140, 23);
		this.FromDateHistory.TabIndex = 4;
		this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1276, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(68, 24);
		this.btnResetHistory.Text = "Reset";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnPurchaseOrderLoader.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnPurchaseOrderLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPurchaseOrderLoader.Name = "btnPurchaseOrderLoader";
		this.btnPurchaseOrderLoader.Size = new System.Drawing.Size(98, 24);
		this.btnPurchaseOrderLoader.Text = "LoadOrder";
		this.btnPurchaseOrderLoader.Click += new System.EventHandler(btnPurchaseOrderLoader_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1288, 711);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmSupplierDispatchPreBill";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SupplierDispatch_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(SupplierDispatch_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommisionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		this.PanelDetail.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingTypeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUomDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRefPartyDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCityNameMain).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencyMain).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.grpUpdateHistoryStatus.ResumeLayout(false);
		this.grpUpdateHistoryStatus.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
