using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.CommissionAgent;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.CommissionAgent;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.FromWise_Helper_Methods;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Cmagt
{

public class frmSupplierOfferCmagt : Form
{
	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode = true;

	private int RecId;

	private int updateDetailIndex = -1;

	private int DocumentTypeId = 1051;

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmSupplierOfferCmagt";

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private DataTable dtExpGrid = new DataTable();

	private DataTable dtEmptyBags = new DataTable();

	private DataTable dtEmptyBagsPM = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtSaleOrderMap = new DataTable();

	private DataTable dtHistoryForLayout = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtViewsData = new DataTable();

	private DataTable dtHistoryCombo = new DataTable();

	private DataTable dtBaseDateType = new DataTable();

	private DataTable dtEbGridPackingTypes = new DataTable();

	private DataTable dtItemForEmptyBag = new DataTable();

	private DataTable dtShipToAddress = new DataTable();

	private Attachment AT = new Attachment();

	private GridExHoverTooltipHelper _tooltipHelper;

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<purchaseOrderDetail> lstRemoveRecordDetail = new List<purchaseOrderDetail>();

	private List<SupplierOfferBuyerInquiryMappingDetail> lstRemoveRecordMappingDetail = new List<SupplierOfferBuyerInquiryMappingDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabForm;

	private TabPage tabHistory;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton BtnRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripSeparator toolStripSeparator2;

	private Panel panel3;

	private Label label23;

	private CheckBox chkPrint;

	private Panel panel1;

	private DateTimePicker datDocDate;

	private Label label17;

	private TextBox txtDocNo;

	private Label label3;

	internal UltraCombo CmbSupplierName;

	private Label label7;

	internal UltraCombo CmbCommissionAgent;

	private Label label4;

	internal UltraCombo CmbPaymentTerm;

	private Label label5;

	private TextBox txtDueDays;

	private Label label6;

	private TextBox txtDeliveryDays;

	private Label label8;

	internal UltraCombo CmbDeliveryTerm;

	private Label label12;

	private TextBox txtRemarks;

	private DateTimePicker datExpiryDate;

	private Label label11;

	private DateTimePicker datDeliveryStartDate;

	private Label label10;

	private Panel panel2;

	private Label label15;

	internal UltraCombo CmbItemName;

	private Label label14;

	internal UltraCombo cmbParentItem;

	private Label label19;

	internal UltraCombo CmbPackingType;

	private Label label18;

	private TextBox txtWeight;

	private Label label16;

	private TextBox txtQty;

	private Label label21;

	internal UltraCombo CmbPackUom;

	private Label label20;

	internal UltraCombo CmbCropYear;

	private TextBox txtRate;

	private Label label26;

	internal UltraCombo CmbCompanyName;

	private ToolStripButton btnAttachments;

	private ToolStripButton btnPrint;

	private Label label28;

	internal UltraCombo CmbRateUom;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton BtnRefreshHistory;

	private Panel panel4;

	private Label label30;

	private Panel panel13;

	private GroupBox groupBox1;

	internal UltraCombo CmbBuyerNameHistory;

	private Label label29;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	internal UltraCombo CmbCommissionAgentHistory;

	private Label label31;

	private DateTimePicker ToDateHistory;

	private Label label33;

	private DateTimePicker FromDateHistory;

	private DateTimePicker datValidityToHistory;

	private Label label32;

	private DateTimePicker datValidityFromHistory;

	internal UltraCombo CmbItemNameHistory;

	private Label label37;

	internal UltraCombo CmbParentItemHistory;

	private Label label36;

	private Panel panel15;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label43;

	internal GridEX grdHistory;

	internal UltraCombo CmbBrokeryType;

	internal UltraCombo cmbcommtype;

	internal UltraCombo CmbBrokeryRateUom;

	internal UltraCombo CmbCommUom;

	internal UltraCombo CmbBrokeryAc;

	internal UltraCombo CmbCommissionAc;

	private Label label1;

	private Label label2;

	private Label label9;

	private Label label13;

	private Label label41;

	private Label label42;

	private Label label44;

	private Label label46;

	private TextBox txtBrokeryRate;

	private TextBox txtCommRate;

	private Label label47;

	private TextBox txtBrokeryAmount;

	private Label label48;

	private TextBox txtCommAmount;

	private Label label49;

	private GroupBox groupBox4;

	private GroupBox groupBox3;

	private GroupBox groupBox2;

	private GroupBox groupBox6;

	internal GridEX grdPaymentTerm;

	private Label label45;

	private TextBox txtAmount;

	private Label label50;

	private TextBox txtTotalAmount;

	private TextBox txtTaxPercnt;

	private Label label22;

	private Label label24;

	private TextBox txtTaxAmount;

	private Label label25;

	private Label label40;

	internal UltraCombo CmbTaxName;

	private TextBox txtRemarksDetail;

	private Label label51;

	private Button BtnAdd;

	private Button BtnCancelDetail;

	private Button BtnUpdateDetail;

	private CheckBox chkOtherExpenseAllowed;

	private CheckBox chkWithHoldingTaxApplied;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private TabPage tabExpenses;

	private TabPage tabEmptyBags;

	internal GridEX grdDetail;

	private Panel panel14;

	private CtrlGrdBar ctrlGrdBar3;

	internal GridEX grdEmptyBags;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdInvExp;

	private Panel panel9;

	private Label label52;

	internal GridEX grdHistoryDetail;

	private RadioButton rdBagFocAndWeightCutNotApply;

	private Label label27;

	private RadioButton rdBagPurchaseAndWeightCutApply;

	private RadioButton rdCompanyOwnedAndWeightCutApply;

	private TabPage tabPaymentSchedule;

	private Panel panel8;

	private Label label38;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label53;

	private TextBox txtPaymentScheduleRemarks;

	private Label label54;

	internal UltraCombo CmbBranch;

	private TabControl tabControl3;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel10;

	private Label label57;

	private Panel panel12;

	private Label label59;

	internal GridEX grdExpenseHistory;

	private Panel panel11;

	private Label label58;

	internal GridEX grdPaymentTermHistory;

	internal GridEX grdEmptyBagHistory;

	private ToolStripButton btnLoadSo;

	private ToolStripSeparator toolStripSeparator4;

	internal GridEX grdInquiryMapping;

	private Panel panel16;

	private Label label60;

	private CtrlGrdBar ctrlGrdBar6;

	private Panel panel17;

	private GroupBox groupBox5;

	internal GridEX grdEmptyBagsPm;

	private Panel panel18;

	private Panel panel19;

	private Panel panel6;

	private Panel panel20;

	private Panel panel21;

	private Label label61;

	private Label label62;

	private TabPage tabPage5;

	internal GridEX grdSaleOrderMappingHistory;

	private Panel panel22;

	private Label label63;

	private Panel panel23;

	private Label label64;

	private Panel panel24;

	private Panel panel25;

	private Label label65;

	private Label label66;

	internal GridEX grdEmptyBagPmHistory;

	private ToolStripButton btnSaveAs;

	private Panel panel26;

	private RadioButton RadBusinessName;

	private RadioButton RadNickName;

	private RadioButton radPartyCode;

	private Button BtnAddShiptoAddress;

	private Label label35;

	internal UltraCombo CmbDeliveryToParty;

	private TextBox txtShipToAddress;

	private Label label34;

	internal UltraCombo CmbShipToAddress;

	private RadioButton RadBagReturnAndWeightCutApply;

	internal UltraCombo CmbDeliverToAddressHistory;

	internal UltraCombo CmbDeliveryToPartyHistory;

	private Label label39;

	private Label label55;

	public frmSupplierOfferCmagt(UserAccount user)
	{
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		_tooltipHelper = new GridExHoverTooltipHelper(grdHistory);
		_tooltipHelper = new GridExHoverTooltipHelper(grdHistoryDetail);
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmSupplierOfferCmagt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1120, 850);
			txtDeliveryDays.Text = "1";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("CompanyName", typeof(string));
			dtSupplier.Columns.Add("PartyCode", typeof(string));
			dtSupplier.Columns.Add("GlAccountId", typeof(int));
			dtSupplier.Columns.Add("CityName", typeof(string));
			dtSupplier.Columns.Add("MobileNo", typeof(string));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("InventoryParentCategoriesId");
			dtItem.Columns.Add("ItemCategoryId");
			dtItem.Columns.Add("ItemCategory");
			dtCrop.Columns.Add("Id", typeof(int));
			dtCrop.Columns.Add("CropYear");
			dtPackType.Columns.Add("Id", typeof(int));
			dtPackType.Columns.Add("PackTypeDesc");
			PurchaseOrderCmagt_Helper.InitializeDetailtable(dtDetail);
			PurchaseOrderCmagt_Helper.InitializeExpenseDetailtable(dtExpGrid);
			PurchaseOrderCmagt_Helper.InitializeEmptyBagDetailtable(dtEmptyBags);
			PurchaseOrderCmagt_Helper.InitializeEmptyBagPMDetailtable(dtEmptyBagsPM);
			PurchaseOrderCmagt_Helper.InitializePaymentTermDetailtable(dtPaymentTerm);
			PurchaseOrderCmagt_Helper.InitializeInquiryMapDetailtable(dtSaleOrderMap);
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
			DataTable viewCombosData = AllComboServices_Views();
			DataTable historyComboData = HistoryComboDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable companyData = CompanyDropdownData();
			AllShipToAddress();
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow();
			return new
			{
				ViewCombosData = viewCombosData,
				HistoryComboData = historyComboData,
				DocumentNoData = documentNoData,
				CompanyData = companyData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else if (!base.IsDisposed)
			{
				var result = t.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnDelete.Enabled = formright.DoHaveCanDelete;
				btnPrint.Enabled = formright.DoHavePrintRights;
				GetConfigurationsFromGlobal();
				if (RecId == 0)
				{
					txtDocNo.Text = result.DocumentNoData;
				}
				dtViewsData = result.ViewCombosData;
				CompanyBind(result.CompanyData);
				CmbCompanyName.Value = UserAccount.CompanyId;
				BranchesFill();
				CmbBranch.Value = UserAccount.BranchesId;
				OtherItemsBind();
				PaymentTermsFill();
				DeliveryTermFill();
				SupplierDtFillFromGlobal();
				SupplierBind();
				BindShipToAddressAgainstBuyer(Conversion.ToInt(CmbDeliveryToParty.Value));
				ParentCategoryBindFromGlobal();
				ItemdtFillFromGlobal();
				ItemNameBind();
				CropDtFillFromGlobalAndBind();
				PackingTypeDtFillFromGlobalAndBind();
				GetCommissionAgentConfigurationsFromGlobalandBind();
				BindViewCombos();
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				if (((DataTable)((UltraGridBase)CmbPaymentTerm).DataSource).Rows.Count > 0)
				{
					CmbPaymentTerm.Value = 2;
				}
				FormInitialized = true;
				datDocDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable AllComboServices_Views()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return purchaseOrderMaster.AllComboServices_FromViews(new ReportsParameters
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

	private void BindViewCombos()
	{
		try
		{
			dtBaseDateType.Rows.Clear();
			dtEbGridPackingTypes.Rows.Clear();
			DataTable dtCommType = new DataTable();
			dtCommType.Columns.Add("Id", typeof(int));
			dtCommType.Columns.Add("Name", typeof(string));
			DataTable dtCommUom = dtCommType.Clone();
			dtBaseDateType = dtCommType.Clone();
			dtEbGridPackingTypes = dtCommType.Clone();
			for (int i = 0; i < dtViewsData.Rows.Count; i++)
			{
				switch (Conversion.ToString(dtViewsData.Rows[i]["Activity"]))
				{
				case "CommissionType":
					dtCommType.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "CommissionRateUom":
					dtCommUom.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "PaymentBaseDate":
					dtBaseDateType.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				case "AllocatedPackingType":
					dtEbGridPackingTypes.Rows.Add(dtViewsData.Rows[i]["Id"], dtViewsData.Rows[i]["ReferenceName"]);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbcommtype, dtCommType, "Id", "Name", "Commission Type", AllColumns: true, Conversion.ToInt(cmbcommtype.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryType, dtCommType, "Id", "Name", "Brokery Type", AllColumns: true, Conversion.ToInt(CmbBrokeryType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCommUom, dtCommUom, "Id", "Name", "Commission Uom", AllColumns: true, Conversion.ToInt(CmbCommUom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbBrokeryRateUom, dtCommUom, "Id", "Name", "Brokery Uom", AllColumns: true, Conversion.ToInt(CmbBrokeryRateUom.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			DataTable dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			InfragisticsHelper.BindAndRetainSelection(CmbBranch, dtBranch, "BranchId", "BranchName", "Branch Name", AllColumns: true, Conversion.ToInt(CmbBranch.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryCombo.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			dtHistoryCombo = purchaseOrderMaster.GetDataForDropDown(obj);
			return dtHistoryCombo;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return purchaseOrderMaster.GenerateCode(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable CompanyDropdownData()
	{
		try
		{
			return CommonServices.CompanyServiceBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CompanyBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCompanyName, dt, "Id", "CompName", "Company Name", AllColumns: false, Conversion.ToInt(CmbCompanyName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer;
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			bool useNickName = RadNickName.Checked;
			Dictionary<int, getGlobalAllSupplierCustomer> customerLookup = customers.ToDictionary((getGlobalAllSupplierCustomer x) => x.Id);
			dtSupplier.BeginLoadData();
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				string name = (useNickName ? (item.NickName ?? string.Empty) : (item.CompanyName ?? string.Empty));
				if (item.IsSubSupCust && item.ParentsSupCustId > 0 && customerLookup.TryGetValue(item.ParentsSupCustId, out var parent))
				{
					name = (useNickName ? (parent.NickName ?? string.Empty) : (parent.CompanyName ?? string.Empty)) + " / " + name;
				}
				dtSupplier.Rows.Add(item.Id, name, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal);
			}
			dtSupplier.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierBind()
	{
		CommissionAndBuyerBind(CmbCommissionAgent, dtSupplier, "Commission Agent");
		CommissionAndBuyerBind(CmbSupplierName, dtSupplier, "Supplier Name");
		CommissionAndBuyerBind(CmbDeliveryToParty, dtSupplier, "Deliver To Party");
		CommissionAndBuyerBind(CmbCommissionAc, dtSupplier, "Commission Account");
		CommissionAndBuyerBind(CmbBrokeryAc, dtSupplier, "Brokery Account");
	}

	private void CommissionAndBuyerBind(UltraCombo ultraCombo, DataTable dataTable, string Caption)
	{
		try
		{
			string ValueMember = (radPartyCode.Checked ? "PartyCode" : "CompanyName");
			string DisplayMember = (radPartyCode.Checked ? "Party Code" : Caption);
			InfragisticsHelper.BindAndRetainSelection(ultraCombo, dataTable, "Id", ValueMember, DisplayMember, AllColumns: true, Conversion.ToInt(ultraCombo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dataTable.Rows.Count > 0)
			{
				((UltraGridBase)ultraCombo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadBusinessName_CheckedChanged(object sender, EventArgs e)
	{
		SupplierDtFillFromGlobal();
		SupplierBind();
	}

	private void PaymentTermsFill()
	{
		try
		{
			DataTable dtTerm = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalPaymentTerm, (InvDueTerms x) => x.Id, (InvDueTerms x) => x.TermsDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbPaymentTerm, dtTerm, "Id", "Description", "Payment Term", AllColumns: true, Conversion.ToInt(CmbPaymentTerm.Value));
			if (dtTerm != null && dtTerm.Rows.Count > 0)
			{
				CmbPaymentTerm_ValueChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbPaymentTerm_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtDueDays.Enabled = true;
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 1 || Conversion.ToInt(CmbPaymentTerm.Value) == 3)
			{
				txtDueDays.Text = "0";
				txtDueDays.Enabled = false;
			}
			else if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) == 0)
			{
				txtDueDays.Text = "2";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCommissionAgentConfigurationsFromGlobalandBind()
	{
		try
		{
			int commissionAgentId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultCommissionAgentIdForCommissionAgentPortal"));
			if (commissionAgentId > 0)
			{
				CmbCommissionAgent.Value = commissionAgentId;
			}
			int subCommissionAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultSubCommissionAccountIdForCommissionAgentPortal"));
			if (subCommissionAccountId > 0)
			{
				CmbCommissionAc.Value = subCommissionAccountId;
			}
			int subBrokerageAccountId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultSubBrokerageAccountIdForCommissionAgentPortal"));
			if (subBrokerageAccountId > 0)
			{
				CmbBrokeryAc.Value = subBrokerageAccountId;
			}
			int paymentTermId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultPaymentTermIdForCommissionAgentPortal"));
			if (paymentTermId > 0)
			{
				CmbPaymentTerm.Value = paymentTermId;
			}
			int deliveryTermId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDeliveryTermIdForCommissionAgentPortal"));
			if (deliveryTermId > 0)
			{
				CmbDeliveryTerm.Value = deliveryTermId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultCropYearIdForCommissionAgentPortal"));
			if (cropYearId > 0)
			{
				CmbCropYear.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultPackingTypeIdForCommissionAgentPortal"));
			if (packingTypeId > 0)
			{
				CmbPackingType.Value = packingTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDeliveryToParty_Leave(object sender, EventArgs e)
	{
		try
		{
			int buyerId = Conversion.ToInt(CmbDeliveryToParty.Value);
			BindShipToAddressAgainstBuyer(buyerId);
			if (buyerId == 0)
			{
				((Control)(object)CmbShipToAddress).Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbShipToAddress_Leave(object sender, EventArgs e)
	{
		try
		{
			int DeliveryToPartyId = 0;
			int num = Conversion.ToInt(CmbShipToAddress.Value);
			UltraGridRow Row = ((UltraDropDownBase)CmbShipToAddress).SelectedRow;
			if (num > 0 && Row != null)
			{
				DeliveryToPartyId = Conversion.ToInt(Row.Cells["PartyId"].Value);
			}
			if (DeliveryToPartyId > 0)
			{
				CmbDeliveryToParty.Value = DeliveryToPartyId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllShipToAddress()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtShipToAddress = SupplierCustomerShipToAddress.GetAll_Combo(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindShipToAddressAgainstBuyer(int BuyerId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Address", typeof(string));
			dt.Columns.Add("PartyId", typeof(int));
			dt.Columns.Add("PartyName", typeof(string));
			EnumerableRowCollection<DataRow> rows = from r in dtShipToAddress.AsEnumerable()
				where BuyerId == 0 || Conversion.ToInt(r["SupplierCustomerId"]) == BuyerId
				select r;
			DataTable dtResult = (rows.Any() ? rows.CopyToDataTable() : dtShipToAddress.Clone());
			if (dtResult.Rows.Count > 0)
			{
				for (int i = 0; i < dtResult.Rows.Count; i++)
				{
					dt.Rows.Add(dtResult.Rows[i]["Id"], dtResult.Rows[i]["AddressLine1"], dtResult.Rows[i]["SupplierCustomerId"], dtResult.Rows[i]["CompanyName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbShipToAddress, dt, "Id", "Address", "Ship To Address", AllColumns: true, Conversion.ToInt(CmbShipToAddress.Value), insertDefaultRow: false, null, null, dt.Rows.Count == 1);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbShipToAddress).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtTerm = DeliveryTerm.FormHistory();
			InfragisticsHelper.BindAndRetainSelection(CmbDeliveryTerm, dtTerm, "Id", "Description", "Delivery Term", AllColumns: false, Conversion.ToInt(CmbDeliveryTerm.Value));
			if (dtTerm != null && dtTerm.Rows.Count > 0)
			{
				txtDeliveryDays_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryBindFromGlobal()
	{
		try
		{
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (!allItems.Any())
			{
				return;
			}
			DataTable dtParentCategory = new DataTable();
			dtParentCategory.Columns.Add("Id", typeof(int));
			dtParentCategory.Columns.Add("Description", typeof(string));
			HashSet<string> parentSet = new HashSet<string>();
			foreach (getGlobalAllItems item in allItems)
			{
				if (!string.IsNullOrEmpty(item.InvParentCateDescription) && parentSet.Add(item.InvParentCateDescription))
				{
					dtParentCategory.Rows.Add(item.InventoryParentCategoriesId, item.InvParentCateDescription);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(cmbParentItem, dtParentCategory, "Id", "Description", "Parent Item", AllColumns: true, Conversion.ToInt(cmbParentItem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading categories: " + ex.Message);
		}
	}

	private void cmbParentItem_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemdtFillFromGlobal(Conversion.ToInt(cmbParentItem.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFillFromGlobal(int ParentCategoryId = 0)
	{
		try
		{
			dtItem.Rows.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems row) => ParentCategoryId == 0 || row.InventoryParentCategoriesId == ParentCategoryId).ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row2 in dtItemAll)
			{
				dtItem.Rows.Add(row2.Id, row2.ItemName, row2.ItemCode, row2.InventoryParentCategoriesId, row2.ItemCategoryId, row2.ItemCategory);
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
			int Id = Conversion.ToInt(CmbItemName.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
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
			int Id = Conversion.ToInt(CmbCropYear.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, dtCrop, "Id", "CropYear", "Crop Year", AllColumns: true, Id, insertDefaultRow: true);
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
			DatatableHelper.PopulateDataTable(dtPackType, Finallist, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dtPackType, "Id", "PackTypeDesc", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = 0;
			if (ItemId > 0 && clsGlobalVariables.getGlobalAllItems != null)
			{
				getGlobalAllItems item = clsGlobalVariables.getGlobalAllItems.FirstOrDefault((getGlobalAllItems x) => x.Id == ItemId);
				if (item != null)
				{
					cmbParentItem.Value = item.InventoryParentCategoriesId;
				}
			}
			if (((UltraGridBase)CmbItemName).ActiveRow != null)
			{
				ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				TaxTypeBind(TaxTypeDbCall(ItemId, datDocDate.Value));
			}
			else
			{
				((Control)(object)CmbTaxName).Text = string.Empty;
				((UltraGridBase)CmbTaxName).DataSource = null;
				txtTaxPercnt.Clear();
				txtTaxAmount.Clear();
			}
			CommonBindings.ItemUomFromGlobalBind(ItemId, CmbPackUom, CmbRateUom);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable TaxTypeDbCall(int ItemId, DateTime DocDate)
	{
		try
		{
			DataTable dtTax = new DataTable();
			dtTax.Columns.Add("Id");
			dtTax.Columns.Add("TaxType");
			dtTax.Columns.Add("TaxPrcnt");
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(ItemId, DocDate);
			if (dt.Rows.Count > 0)
			{
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
			}
			return dt;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TaxTypeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbTaxName, dt, "TaxNameId", "TaxName", "Tax Name", AllColumns: true, Conversion.ToInt(CmbTaxName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (updateDetailIndex > -1 && grdDetail.CurrentRow != null && Conversion.ToInt(grdDetail.CurrentRow.Cells["TaxNameId"].Value) > 0)
			{
				CmbTaxName.Value = Conversion.ToInt(grdDetail.CurrentRow.Cells["TaxNameId"].Value);
			}
			CmbTaxName_Leave(null, null);
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
			decimal num = Conversion.ToDecimal((object)txtQty.Text);
			decimal PackEquivalent = ((Conversion.ToInt(CmbPackUom.Value) > 0 && ((UltraGridBase)CmbPackUom).ActiveRow != null) ? Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value) : 0m);
			decimal weight = num * PackEquivalent;
			txtWeight.Text = weight.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			decimal weight = Conversion.ToDecimal((object)txtWeight.Text);
			decimal rateEquivalent = ((Conversion.ToInt(CmbRateUom.Value) > 0 && ((UltraGridBase)CmbRateUom).ActiveRow != null) ? Conversion.ToDecimal(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value) : 0m);
			decimal rate = Conversion.ToDecimal((object)txtRate.Text);
			decimal amount = ((weight > 0m && rateEquivalent > 0m) ? (weight / rateEquivalent * rate) : 0m);
			txtAmount.Text = amount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbPackUom_Leave(object sender, EventArgs e)
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

	private void txtQty_TextChanged(object sender, EventArgs e)
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

	private void txtWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
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
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbTaxName_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTaxAmount()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtAmount.Text);
			double TaxPercent = 0.0;
			double TaxAmt = 0.0;
			double TotalAmount = 0.0;
			if (((UltraGridBase)CmbTaxName).ActiveRow != null)
			{
				TaxPercent = ((Conversion.ToDouble((object)txtTaxPercnt.Text) > 0.0) ? Conversion.ToDouble((object)txtTaxPercnt.Text) : ((((UltraDropDownBase)CmbTaxName).SelectedRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbTaxName).SelectedRow.Cells[2].Value) : 0.0));
			}
			TaxAmt = num * TaxPercent / 100.0;
			TotalAmount = num + TaxAmt;
			txtTaxPercnt.Text = TaxPercent.ToString("#,##0.###");
			txtTaxAmount.Text = Math.Round(TaxAmt, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
			txtTotalAmount.Text = Math.Round(TotalAmount, 3, MidpointRounding.AwayFromZero).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbcommtype_TextChanged(object sender, EventArgs e)
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

	private void txtCommRate_TextChanged(object sender, EventArgs e)
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

	private void CmbCommUom_TextChanged(object sender, EventArgs e)
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

	private void CmbBrokeryType_TextChanged(object sender, EventArgs e)
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

	private void TotalCommissionAmount()
	{
		try
		{
			if (((UltraGridBase)CmbCommissionAc).ActiveRow != null)
			{
				if (Conversion.ToDouble((object)txtCommRate.Text.Trim()) > 0.0)
				{
					double CommRate = Conversion.ToDouble((object)txtCommRate.Text.Trim());
					if (((Control)(object)cmbcommtype).Text == "Flat")
					{
						txtCommAmount.Text = CommRate.ToString("#,##0.###");
					}
					else if (((Control)(object)cmbcommtype).Text == "Percent")
					{
						if (CommRate > 100.0)
						{
							CommRate = 100.0;
							txtCommRate.Text = "100";
						}
						object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2);
						if (total != null)
						{
							double CommAmt = Conversion.ToDouble((object)total.ToString()) * CommRate / 100.0;
							txtCommAmount.Text = Math.Round(CommAmt).ToString("#,##0.###");
						}
					}
					else if (((Control)(object)cmbcommtype).Text == "Comm Weight")
					{
						object Weight = grdDetail.GetTotal(grdDetail.RootTable.Columns["Weight"], (AggregateFunction)2);
						if (Weight != null)
						{
							double num = Conversion.ToDouble((object)Weight.ToString());
							double UOM = Conversion.ToDouble((object)((Control)(object)CmbCommUom).Text.Trim());
							double CommAmt2 = num / UOM * CommRate;
							txtCommAmount.Text = Math.Round(CommAmt2).ToString("#,##0.###");
						}
					}
				}
				else
				{
					txtCommAmount.Text = "0";
				}
			}
			else
			{
				txtCommAmount.Text = "0";
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
			if (((UltraGridBase)CmbBrokeryAc).ActiveRow == null)
			{
				return;
			}
			double CommRate = Conversion.ToDouble((object)txtBrokeryRate.Text.Trim());
			if (!(CommRate > 0.0))
			{
				return;
			}
			if (((Control)(object)CmbBrokeryType).Text == "Flat")
			{
				txtBrokeryAmount.Text = CommRate.ToString("#,##0.###");
			}
			else if (((Control)(object)CmbBrokeryType).Text == "Percent")
			{
				if (CommRate > 100.0)
				{
					CommRate = 100.0;
					txtBrokeryRate.Text = "100";
				}
				object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt = double.Parse(total.ToString()) * CommRate / 100.0;
					txtBrokeryAmount.Text = Math.Round(CommAmt).ToString("#,##0.###");
				}
			}
			else if (((Control)(object)CmbBrokeryType).Text == "Comm Weight")
			{
				double Weight = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Weight"], (AggregateFunction)2));
				if (Weight > 0.0)
				{
					double num = Conversion.ToDouble((object)Weight.ToString());
					double UOM = Conversion.ToDouble((object)((Control)(object)CmbBrokeryRateUom).Text.Trim());
					double CommAmt2 = num / UOM * CommRate;
					txtBrokeryAmount.Text = Math.Round(CommAmt2).ToString("#,##0.###");
				}
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
			decimal TotalAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdDetail, "TotalAmount"));
			if (TotalAmount > 0m && grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal Amount = Conversion.ToDecimal(obj.Cells["%OfTotal"].Value) * TotalAmount / 100m;
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

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdDetailSetting();
			}
			DataTable dataTable2 = dtExpGrid;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				if (dtExpGrid.Rows.Count == 0)
				{
					AddRowsInExpenseGrid();
				}
				grdInvExp.DataSource = dtExpGrid;
				grdInvExp.RetrieveStructure();
				grdInvExpSettings();
			}
			DataTable dataTable3 = dtEmptyBags;
			if (dataTable3 != null && dataTable3.Columns.Count > 0)
			{
				if (dtEmptyBags.Rows.Count == 0)
				{
					AddRowsInvEmptyBagsGrid();
				}
				grdEmptyBags.RetrieveStructure();
				gridEmptyBagsSettings();
			}
			DataTable dataTable4 = dtEmptyBagsPM;
			if (dataTable4 != null && dataTable4.Columns.Count > 0)
			{
				if (dtEmptyBagsPM.Rows.Count == 0)
				{
					AddRowInEmptyBagsPmGrid();
				}
				grdEmptyBagsPm.RetrieveStructure();
				gridEmptyBagsPmSettings();
			}
			DataTable dataTable5 = dtPaymentTerm;
			if (dataTable5 != null && dataTable5.Columns.Count > 0)
			{
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddRowInPaymentGrid();
				}
				grdPaymentTerm.RetrieveStructure();
				gridPaymentTermSetting();
			}
			if (dtSaleOrderMap.Columns.Count > 0)
			{
				grdInquiryMapping.DataSource = dtSaleOrderMap;
				grdInquiryMapping.RetrieveStructure();
				grdSaleOrderMappingSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSetting()
	{
		try
		{
			PurchaseOrderCmagt_Helper.DetailGridCommonSetting(grdDetail);
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdDetail, "Edit", "Edit", 40, 1);
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
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				grdDetail.UpdateData();
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
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail first..");
				}
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					purchaseOrderDetail d = new purchaseOrderDetail();
					d.purchaseOrderDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d, r);
					d.actionTypeId = 3;
					lstRemoveRecordDetail.Add(d);
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			EnableDisableColumns();
			TotalCommissionAmount();
			TotalBrokeryAmountCalculate();
			PaymentAmountReCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDisableColumns()
	{
		bool hasTax = grdDetail.GetDataRows().Any((GridEXRow r) => Conversion.ToInt(r.Cells["TaxAmount"].Value) > 0);
		datDocDate.Enabled = !hasTax;
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow row = grdDetail.CurrentRow;
			if (row != null)
			{
				updateDetailIndex = row.RowIndex;
				cmbParentItem.Value = Conversion.ToInt(row.Cells["ParentItemId"].Value);
				cmbParentItem_Leave(null, null);
				CmbItemName.Value = Conversion.ToInt(row.Cells["ItemId"].Value);
				CmbItemName_Leave(null, null);
				CmbCropYear.Value = Conversion.ToInt(row.Cells["CropYearId"].Value);
				CmbPackingType.Value = Conversion.ToInt(row.Cells["PackingTypeId"].Value);
				CmbPackUom.Value = Conversion.ToInt(row.Cells["PackUomId"].Value);
				txtQty.Text = Conversion.ToDecimal(row.Cells["Qty"].Value).ToString("#,##0.###");
				txtWeight.Text = Conversion.ToDecimal(row.Cells["Weight"].Value).ToString("#,##0.###");
				txtRate.Text = Conversion.ToDecimal(row.Cells["Rate"].Value).ToString("#,##0.###");
				CmbRateUom.Value = Conversion.ToInt(row.Cells["RateUomId"].Value);
				txtAmount.Text = Conversion.ToDecimal(row.Cells["Amount"].Value).ToString("#,##0.###");
				CmbTaxName.Value = row.Cells["TaxNameId"].Value;
				txtTaxPercnt.Text = Conversion.ToString(row.Cells["Tax%"].Value);
				txtTaxAmount.Text = Conversion.ToDecimal(row.Cells["TaxAmount"].Value).ToString("#,##0.###");
				txtTotalAmount.Text = Conversion.ToDecimal(row.Cells["TotalAmount"].Value).ToString("#,##0.###");
				txtRemarksDetail.Text = Conversion.ToString(row.Cells["Remarks"].Value);
				BtnAdd.Visible = false;
				BtnUpdateDetail.Visible = true;
				BtnCancelDetail.Visible = true;
				cmbParentItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string NameCode = "";
			decimal Name = default(decimal);
			if (grdDetail.CurrentColumn.Key == "TaxNameId" || grdDetail.CurrentColumn.Key == "TaxName")
			{
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (ItemId == 0)
				{
					throw new Exception("Service Item Not Found In Current Row");
				}
				GrdPopUp obj = new GrdPopUp(TaxTypeDbCall(ItemId, datDocDate.Value), "TaxNameId", "TaxName", "TaxPercent");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				if (Id > 0)
				{
					NameCode = Conversion.ToString((object)obj.ReturnName);
					Name = Conversion.ToDecimal((object)obj.ReturnCode);
					r.Cells["TaxNameId"].Value = Id;
					r.Cells["TaxName"].Value = NameCode;
					r.Cells["Tax%"].Value = Name;
				}
				else if (Conversion.ToInt(r.Cells["TaxNameId"].Value) > 0 && MessageBox.Show("No tax selected. Do you want to clear the current Tax fields?", "Clear Tax", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					r.Cells["TaxNameId"].Value = 0;
					r.Cells["TaxName"].Value = "";
					r.Cells["Tax%"].Value = 0;
				}
				Detail_TaxUpdateInRow(r);
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Detail_TaxUpdateInRow(GridEXRow row)
	{
		decimal num = Conversion.ToDecimal(row.Cells["Amount"].Value);
		decimal taxPercent = default(decimal);
		decimal taxAmount = default(decimal);
		decimal totalAmount = num;
		if (Conversion.ToDouble(row.Cells["Tax%"].Value) > 0.0)
		{
			taxPercent = Conversion.ToDecimal(row.Cells["Tax%"].Value);
		}
		else if (Conversion.ToInt(row.Cells["TaxNameId"].Value) > 0)
		{
			taxPercent = Conversion.ToDecimal(row.Cells["Tax%"].Value);
		}
		taxAmount = num * taxPercent / 100m;
		totalAmount = num + taxAmount;
		row.Cells["Tax%"].Value = taxPercent;
		row.Cells["TaxAmount"].Value = Math.Round(taxAmount, 3);
		row.Cells["TotalAmount"].Value = Math.Round(totalAmount, 3);
	}

	private void BtnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				int ItemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				if (dtDetail.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["ItemId"]) == ItemId))
				{
					MessageBox.Show(((Control)(object)CmbItemName).Text + " already in detail. So can't add this item");
					CmbItemName.Focus();
					return;
				}
			}
			DataRow dr = dtDetail.NewRow();
			dr["Id"] = 0;
			FillDetailRow(dr);
			dtDetail.Rows.Add(dr);
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			grdDetailSetting();
			ResetDetail();
			EnableDisableColumns();
			TotalCommissionAmount();
			TotalBrokeryAmountCalculate();
			PaymentAmountReCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailRow(DataRow dr)
	{
		dr["ParentItemId"] = Conversion.ToInt(cmbParentItem.Value);
		dr["ParentItem"] = Conversion.ToString((object)((Control)(object)cmbParentItem).Text);
		int itemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
		dr["ItemId"] = itemId;
		string value;
		if (itemId <= 0)
		{
			value = string.Empty;
		}
		else
		{
			UltraGridRow selectedRow = ((UltraDropDownBase)CmbItemName).SelectedRow;
			value = Conversion.ToString((selectedRow != null) ? selectedRow.Cells["ItemName"].Value : null);
		}
		dr["ItemName"] = value;
		dr["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
		dr["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
		dr["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
		dr["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
		dr["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
		dr["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
		dr["Qty"] = Conversion.ToDecimal((object)txtQty.Text);
		dr["Weight"] = Conversion.ToDecimal((object)txtWeight.Text);
		dr["Rate"] = Conversion.ToDecimal((object)txtRate.Text);
		dr["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
		dr["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
		decimal Amount = Conversion.ToDecimal((object)txtAmount.Text);
		dr["Amount"] = Amount;
		int taxId = Conversion.ToInt(CmbTaxName.Value);
		decimal taxPercent = Conversion.ToDecimal((object)txtTaxPercnt.Text);
		decimal taxAmount = Conversion.ToDecimal((object)txtTaxAmount.Text);
		decimal totalTcyAmount = Amount + taxAmount;
		dr["TaxNameId"] = taxId;
		dr["TaxName"] = ((taxId > 0) ? Conversion.ToString((object)((Control)(object)CmbTaxName).Text) : string.Empty);
		dr["Tax%"] = taxPercent;
		dr["TaxAmount"] = taxAmount;
		dr["TotalAmount"] = totalTcyAmount;
		dr["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
	}

	private void BtnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail() || updateDetailIndex < 0 || updateDetailIndex >= dtDetail.Rows.Count)
			{
				return;
			}
			if (grdDetail.GetRows().Count() > 0)
			{
				int itemId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["Id"].Value);
				if (dtDetail.AsEnumerable().Where((DataRow r, int index) => index != updateDetailIndex).Any((DataRow r) => Conversion.ToInt(r["ItemId"]) == itemId))
				{
					MessageBox.Show(((Control)(object)CmbItemName).Text + " already in detail. So can't add this item");
					CmbItemName.Focus();
					return;
				}
			}
			DataRow dr = dtDetail.Rows[updateDetailIndex];
			FillDetailRow(dr);
			grdDetail.DataSource = dtDetail;
			((Control)(object)grdDetail).Refresh();
			ResetDetail();
			EnableDisableColumns();
			TotalCommissionAmount();
			TotalBrokeryAmountCalculate();
			PaymentAmountReCalculate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowsInExpenseGrid()
	{
		try
		{
			if (dtOtherItem.Rows.Count == 0)
			{
				OtherItemsBind();
			}
			for (int i = 0; i < dtOtherItem.Rows.Count; i++)
			{
				dtExpGrid.Rows.Add(0, 0, 0, 0, dtOtherItem.Rows[i]["Id"], 0, 0, 0, "");
			}
			grdInvExp.DataSource = dtExpGrid;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpenseGridCombBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grdInvExp.RootTable.Columns["ItemId"], dtOtherItem, "Id", "OtherItemName", "Other Item Name", 200);
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExpSettings()
	{
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SoId", "SoExpenseId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"Rate",
					(EditType)1
				},
				{
					"Amount",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdInvExp, 2, 3, hiddenColumns, editableColumns);
			ExpenseGridCombBind();
			grdInvExp.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdInvExp.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdInvExp.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInvExp.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInvExp.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 250;
			GridEXRow rowForCondition = grdInvExp.GetRow(0);
			if (rowForCondition != null && (int)rowForCondition.RowType == 0)
			{
				if (Conversion.ToInt(rowForCondition.Cells["SoId"].Value) > 0)
				{
					grdInvExp.RootTable.Columns["SaleOrderNo"].Visible = true;
				}
				else
				{
					grdInvExp.RootTable.Columns["SaleOrderNo"].Visible = false;
				}
			}
			ctrlGrdBar3_Load(null, null);
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
			if (item != null)
			{
				_ = item.RowType;
			}
			grdInvExp.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdInvExp.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				switch (e.Column.Key)
				{
				default:
					return;
				case "Qty":
				case "Rate":
					UpdateAmount(row);
					break;
				case "Amount":
					UpdateRate(row);
					break;
				}
				grdInvExp.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateAmount(GridEXRow row)
	{
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		double rate = Conversion.ToDouble(row.Cells["Rate"].Value);
		row.Cells["Amount"].Value = qty * rate;
	}

	private void UpdateRate(GridEXRow row)
	{
		double amount = Conversion.ToDouble(row.Cells["Amount"].Value);
		double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
		row.Cells["Rate"].Value = ((qty == 0.0) ? 0.0 : (amount / qty));
	}

	private void AddRowsInvEmptyBagsGrid()
	{
		try
		{
			for (int i = 0; i < dtEbGridPackingTypes.Rows.Count; i++)
			{
				dtEmptyBags.Rows.Add(0, 0, 0, dtEbGridPackingTypes.Rows[i]["Id"], 0);
			}
			grdEmptyBags.DataSource = dtEmptyBags;
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
			grdEmptyBags.RootTable.Columns["PackingType"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["PackingType"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["PackingType"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["PackingType"].Caption = "Packing Type";
			grdEmptyBags.RootTable.Columns["PackingType"].ValueList.PopulateValueList((IEnumerable)dtEbGridPackingTypes.DefaultView, "Id", "Name");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridEmptyBagsSettings()
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SoId", "SoEbId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"WeightCut",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBags, 2, 3, hiddenColumns, editableColumns);
			GrdEmptyBagsRefresh();
			grdEmptyBags.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdEmptyBags.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdEmptyBags.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType + 50;
			grdEmptyBags.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WeightConstant;
			GridEXRow rowForCondition = grdEmptyBags.GetRow(0);
			if (rowForCondition != null && (int)rowForCondition.RowType == 0)
			{
				if (Conversion.ToInt(rowForCondition.Cells["SoId"].Value) > 0)
				{
					grdEmptyBags.RootTable.Columns["SaleOrderNo"].Visible = true;
				}
				else
				{
					grdEmptyBags.RootTable.Columns["SaleOrderNo"].Visible = false;
				}
			}
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInEmptyBagsPmGrid()
	{
		try
		{
			dtEmptyBagsPM.Rows.Add(0, 0, 0);
			grdEmptyBagsPm.DataSource = dtEmptyBagsPM;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdEmptyBagsPmRefresh()
	{
		try
		{
			grdEmptyBagsPm.RootTable.Columns["PackingType"].ColumnType = (ColumnType)0;
			grdEmptyBagsPm.RootTable.Columns["PackingType"].HasValueList = true;
			grdEmptyBagsPm.RootTable.Columns["PackingType"].LimitToList = true;
			grdEmptyBagsPm.RootTable.Columns["PackingType"].Caption = "Packing Type";
			grdEmptyBagsPm.RootTable.Columns["PackingType"].ValueList.PopulateValueList((IEnumerable)dtEbGridPackingTypes.DefaultView, "Id", "Name");
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].ColumnType = (ColumnType)0;
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].HasValueList = true;
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].LimitToList = true;
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].Caption = "Empty Bag Item";
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].ValueList.PopulateValueList((IEnumerable)dtItemForEmptyBag.DefaultView, "ItemId", "ItemName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridEmptyBagsPmSettings()
	{
		try
		{
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"PackingType",
					(EditType)4
				},
				{
					"PurchaseRate",
					(EditType)1
				},
				{
					"EmptyBagItem",
					(EditType)4
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBagsPm, 2, 3, null, editableColumns);
			GrdEmptyBagsPmRefresh();
			grdEmptyBagsPm.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType + 50;
			grdEmptyBagsPm.RootTable.Columns["PurchaseRate"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdEmptyBagsPm.RootTable.Columns["EmptyBagItem"].Width = Constants.InventoryConstants.ItemName + 20;
			GridEX_Helper.AddButton(grdEmptyBagsPm, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdEmptyBagsPm, "Add", "+", 20, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBagsPm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdEmptyBagsPm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtEmptyBagsPM.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtEmptyBagsPM.Rows.Count == 0)
				{
					AddRowInEmptyBagsPmGrid();
				}
			}
			else if (e.Column.Key == "Add")
			{
				AddRowInEmptyBagsPmGrid();
			}
			grdEmptyBagsPm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBagsPm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBagsPm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdEmptyBagsPm.CurrentColumn.Key == "Delete")
				{
					dtEmptyBagsPM.Rows[r.RowIndex].Delete();
					r.Delete();
					if (dtEmptyBagsPM.Rows.Count == 0)
					{
						AddRowInEmptyBagsPmGrid();
					}
				}
				else if (grdEmptyBagsPm.CurrentColumn.Key == "Add")
				{
					AddRowInEmptyBagsPmGrid();
				}
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtEmptyBagsPM.Rows[r.RowIndex].Delete();
				r.Delete();
				if (dtEmptyBagsPM.Rows.Count == 0)
				{
					AddRowInEmptyBagsPmGrid();
				}
			}
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
			dtPaymentTerm.Rows.Add(0, 0, 0, 0, 0);
			grdPaymentTerm.DataSource = dtPaymentTerm;
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
			grdPaymentTerm.RootTable.Columns["BaseDateType"].ColumnType = (ColumnType)0;
			grdPaymentTerm.RootTable.Columns["BaseDateType"].HasValueList = true;
			grdPaymentTerm.RootTable.Columns["BaseDateType"].LimitToList = true;
			grdPaymentTerm.RootTable.Columns["BaseDateType"].Caption = "Base Date Type";
			grdPaymentTerm.RootTable.Columns["BaseDateType"].ValueList.PopulateValueList((IEnumerable)dtBaseDateType.DefaultView, "Id", "Name");
			DataTable dt = (DataTable)((UltraGridBase)CmbPaymentTerm).DataSource;
			if (dt != null && dt.Rows.Count > 0)
			{
				grdPaymentTerm.RootTable.Columns["PaymentTerm"].ColumnType = (ColumnType)0;
				grdPaymentTerm.RootTable.Columns["PaymentTerm"].HasValueList = true;
				grdPaymentTerm.RootTable.Columns["PaymentTerm"].LimitToList = true;
				grdPaymentTerm.RootTable.Columns["PaymentTerm"].Caption = "Payment Term";
				grdPaymentTerm.RootTable.Columns["PaymentTerm"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "Description");
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
					"BaseDateType",
					(EditType)4
				},
				{
					"DueDate",
					(EditType)3
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentTerm, 2, 3, null, editableColumns);
			grdPaymentTerm.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant + 50;
			grdPaymentTerm.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.DueDays;
			grdPaymentTerm.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.ExRateConstant + 20;
			grdPaymentTerm.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			grdPaymentTerm.RootTable.Columns["BaseDateType"].Width = Constants.InventoryConstants.DateConstant + 40;
			grdPaymentTerm.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			GridEX_Helper.AddButton(grdPaymentTerm, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdPaymentTerm, "Add", "+", 20, 1);
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
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
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (grdDetail.GetRows().Count() > 0)
				{
					decimal TotalAmount = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdDetail, "TotalAmount"));
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
					}
					if (e.Column.Key == "DueDays")
					{
						int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
						item.Cells["DueDate"].Value = datDocDate.Value.AddDays(days2);
					}
					if (e.Column.Key == "DueDate")
					{
						DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
						if (date.Date < datDocDate.Value.Date)
						{
							item.Cells["DueDate"].Value = datDocDate.Value;
							throw new Exception("Due Date Can't less Than DocDate");
						}
						int days = (date.Date - datDocDate.Value.Date).Days;
						item.Cells["DueDays"].Value = days;
					}
				}
				grdPaymentTerm.UpdateData();
				if (grdPaymentTerm.GetRows().Count() > 0 && Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentTerm, "Amount")) > 0m && grdPaymentTerm.DataSource is DataTable dt)
				{
					txtPaymentScheduleRemarks.Text = string.Join(", ", from x in dt.AsEnumerable()
						select string.Format("[Term:{0},", x["PaymentTerm"]) + string.Format("DueDays:{0},%OfTotal:{1},", x["DueDays"], x["%OfTotal"]) + string.Format("DueAmount:{0},BaseDateType:{1}]", x["Amount"], x["BaseDateType"]));
				}
				grdPaymentTerm.UpdateData();
				return;
			}
			throw new Exception("No Detail Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentTerm.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space && grdPaymentTerm.CurrentColumn != null)
			{
				if (grdPaymentTerm.CurrentColumn.Key == "Delete")
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					item.Delete();
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
					grdPaymentTerm.UpdateData();
				}
				else if (grdPaymentTerm.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentGrid();
					grdPaymentTerm.UpdateData();
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
				grdPaymentTerm.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentGrid();
				grdPaymentTerm.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOrderMappingSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InquiryId", "InquiryDetailId", "BuyerId", "ItemId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"CurrentAllocationWeight",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdInquiryMapping, 2, 3, hiddenColumns, editableColumns);
			grdInquiryMapping.RootTable.Columns["InquiryNo"].Width = Constants.InventoryConstants.DocNoConstant + 15;
			grdInquiryMapping.RootTable.Columns["BuyerName"].Width = Constants.InventoryConstants.SupplierName;
			grdInquiryMapping.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInquiryMapping.RootTable.Columns["ItemWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInquiryMapping.RootTable.Columns["BookedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInquiryMapping.RootTable.Columns["BalanceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInquiryMapping.RootTable.Columns["CurrentAllocationWeight"].Width = Constants.InventoryConstants.WeightConstant + 20;
			GridEX_Helper.AddButton(grdInquiryMapping, "Delete", "X", 20, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOrderMapping_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInquiryMapping.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteSoMappingRow(r);
			}
			grdInquiryMapping.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteSoMappingRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) == 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtSaleOrderMap.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			SupplierOfferBuyerInquiryMappingDetail d = new SupplierOfferBuyerInquiryMappingDetail();
			d.SupplierOfferBuyerInquiryMappingDetailId = Conversion.ToInt(r.Cells["Id"].Value);
			d.actionTypeId = 3;
			d.inquiryBookingMasterId = Conversion.ToInt(r.Cells["InquiryId"].Value);
			d.inquiryBookingDetailId = Conversion.ToInt(r.Cells["InquiryDetailId"].Value);
			d.buyerId = Conversion.ToInt(r.Cells["BuyerId"].Value);
			d.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			d.itemNetWeight = Conversion.ToDecimal(r.Cells["CurrentAllocationWeight"].Value);
			lstRemoveRecordMappingDetail.Add(d);
			dtSaleOrderMap.Rows[r.RowIndex].Delete();
			r.Delete();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSaleOrderMapping_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInquiryMapping.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "CurrentAllocationWeight")
			{
				decimal BalanceWeight = Conversion.ToDecimal(r.Cells["BalanceWeight"].Value);
				decimal CurrentAllocationWeight = Conversion.ToDecimal(r.Cells["CurrentAllocationWeight"].Value);
				if (CurrentAllocationWeight > BalanceWeight)
				{
					MessageBox.Show(string.Format("Current Allocation Weight:{0} can't be greater than Balance weight:{1}", CurrentAllocationWeight.ToString("#,##0.###"), BalanceWeight));
					r.Cells["CurrentAllocationWeight"].Value = BalanceWeight;
				}
			}
			grdInquiryMapping.UpdateData();
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

	private void Reset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecordDetail.Clear();
			lstRemoveRecordMappingDetail.Clear();
			btnsave.Visible = true;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			txtDeliveryDays.Text = string.Empty;
			((Control)(object)CmbPaymentTerm).Text = string.Empty;
			txtDueDays.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)CmbCommissionAc).Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtCommRate.Text = string.Empty;
			((Control)(object)CmbCommUom).Text = string.Empty;
			txtCommAmount.Text = string.Empty;
			((Control)(object)CmbBrokeryAc).Text = string.Empty;
			((Control)(object)CmbBrokeryType).Text = string.Empty;
			txtBrokeryRate.Text = string.Empty;
			((Control)(object)CmbBrokeryRateUom).Text = string.Empty;
			txtBrokeryAmount.Text = string.Empty;
			((Control)(object)cmbParentItem).Enabled = true;
			txtPaymentScheduleRemarks.Text = string.Empty;
			ResetDetail();
			dtDetail.Rows.Clear();
			dtExpGrid.Rows.Clear();
			dtEmptyBags.Rows.Clear();
			dtEmptyBagsPM.Rows.Clear();
			dtPaymentTerm.Rows.Clear();
			dtSaleOrderMap.Rows.Clear();
			BindGrids();
			txtDocNo.Text = DocumentNoDbCall();
			datDocDate.Focus();
			GetCommissionAgentConfigurationsFromGlobalandBind();
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
			((Control)(object)CmbItemName).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			((Control)(object)CmbRateUom).Text = string.Empty;
			((UltraGridBase)CmbRateUom).DataSource = null;
			((Control)(object)CmbTaxName).Text = string.Empty;
			((UltraGridBase)CmbTaxName).DataSource = null;
			txtTaxPercnt.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			updateDetailIndex = -1;
			BtnAdd.Visible = true;
			BtnCancelDetail.Visible = false;
			BtnUpdateDetail.Visible = false;
			cmbParentItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "SupplierCustomerLists", "AllItems", "CropYear", "InvPackingType", "UomSchedule", "PaymentTerm" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CompanyBind(CompanyDropdownData());
			BranchesFill();
			PaymentTermsFill();
			DeliveryTermFill();
			SupplierDtFillFromGlobal();
			SupplierBind();
			PackingTypeDtFillFromGlobalAndBind();
			AllShipToAddress();
			BindShipToAddressAgainstBuyer(Conversion.ToInt(CmbDeliveryToParty.Value));
			dtViewsData = AllComboServices_Views();
			BindViewCombos();
			CropDtFillFromGlobalAndBind();
			ParentCategoryBindFromGlobal();
			((Control)(object)cmbParentItem).Enabled = true;
			ItemdtFillFromGlobal(Conversion.ToInt(cmbParentItem.Value));
			ItemNameBind();
			dtItemForEmptyBag = CommonServices.GetPackingMaterialItemsAllocateToFlow();
			ExpenseGridCombBind();
			GrdEmptyBagsRefresh();
			GrdEmptyBagsPmRefresh();
			GridPaymentTermRefresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSupplierOfferCmagt_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			_tooltipHelper.Dispose();
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)CmbCommissionAgent).ActiveRow == null || Conversion.ToInt(CmbCommissionAgent.Value) == 0)
		{
			MessageBox.Show("Please Select Commission Agent / Broker");
			CmbCommissionAgent.Focus();
			return false;
		}
		if (((UltraGridBase)CmbSupplierName).ActiveRow == null || Conversion.ToInt(CmbSupplierName.Value) == 0)
		{
			MessageBox.Show("Please Select Supplier Name");
			CmbSupplierName.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentTerm, "Amount")) == 0m)
		{
			if (((UltraGridBase)CmbPaymentTerm).ActiveRow == null || Conversion.ToInt(CmbPaymentTerm.Value) == 0)
			{
				MessageBox.Show("Please Select Paymrnt Term");
				CmbPaymentTerm.Focus();
				return false;
			}
			if (Conversion.ToInt(CmbPaymentTerm.Value) == 2 && Conversion.ToInt((object)txtDueDays.Text) == 0)
			{
				MessageBox.Show("Due Days field is required");
				txtDueDays.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null || Conversion.ToInt(CmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("Please Select Delivery Term");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
		{
			MessageBox.Show("Delivery Days field is required");
			txtDeliveryDays.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
		{
			((Control)(object)cmbParentItem, "Parent Item", FormHelper.ValidType.INT),
			((Control)(object)CmbItemName, "Item Name", FormHelper.ValidType.INT),
			((Control)(object)CmbCropYear, "Crop Year", FormHelper.ValidType.INT),
			((Control)(object)CmbPackingType, "Packing Type", FormHelper.ValidType.INT),
			((Control)(object)CmbPackUom, "Pack Uom", FormHelper.ValidType.INT),
			(txtQty, "Qty", FormHelper.ValidType.Double),
			(txtWeight, "Weight", FormHelper.ValidType.Double),
			(txtRate, "NetWeight", FormHelper.ValidType.Double),
			((Control)(object)CmbRateUom, "Rate Uom", FormHelper.ValidType.INT),
			(txtAmount, "NetWeight", FormHelper.ValidType.Double),
			(txtTotalAmount, "Tax + Amount", FormHelper.ValidType.Double)
		}))
		{
			return false;
		}
		return true;
	}

	private void FillDetailListCommonForInsertAndDelete(purchaseOrderDetail vd, GridEXRow r)
	{
		vd.inventoryParentCategoryId = Conversion.ToInt(r.Cells["ParentItemId"].Value);
		vd.itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.cropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
		vd.cropYear = Conversion.ToString(r.Cells["CropYear"].Value);
		vd.packingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
		vd.PackingType = Conversion.ToString(r.Cells["PackingType"].Value);
		vd.packUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
		vd.itemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
		vd.itemWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
		vd.itemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
		vd.rateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
		vd.RateUomCode = Conversion.ToString(r.Cells["RateUom"].Value);
		vd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
		vd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
		vd.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
		vd.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
		vd.TotalAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
		vd.remarks = Conversion.ToString(r.Cells["Remarks"].Value);
	}

	private void Insert()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Expected O, but got Unknown
		//IL_0933: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_0b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Expected O, but got Unknown
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Expected O, but got Unknown
		//IL_118c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1193: Expected O, but got Unknown
		//IL_0e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5d: Expected O, but got Unknown
		//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f66: Expected O, but got Unknown
		//IL_13ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Detail record not found. Please check!");
			}
			purchaseOrderMaster obj = new purchaseOrderMaster();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.purchaseOrderMasterId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.documentTypeId = DocumentTypeId;
			obj.docNo = Conversion.ToInt((object)txtDocNo.Text);
			obj.docDate = Conversion.ToDateTime((object)datDocDate.Value);
			obj.ValidityDate = Conversion.ToDateTime((object)datExpiryDate.Value);
			obj.statusId = 1;
			obj.commissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
			obj.supplierId = Conversion.ToInt(CmbSupplierName.Value);
			obj.DeliveryToPartyId = Conversion.ToInt(CmbDeliveryToParty.Value);
			obj.shipToAddressId = Conversion.ToInt(CmbShipToAddress.Value);
			obj.ShipToAddress = Conversion.ToString((object)txtShipToAddress.Text);
			obj.deliveryTermId = Conversion.ToInt(CmbDeliveryTerm.Value);
			obj.deliveryDays = Conversion.ToInt((object)txtDeliveryDays.Text);
			obj.deliveryStartDate = Conversion.ToDateTime((object)datDeliveryStartDate.Value);
			obj.remarksHeader = txtRemarks.Text;
			obj.companyId = Conversion.ToInt(CmbCompanyName.Value);
			obj.organizationId = UserAccount.OrganizationId;
			obj.branchId = Conversion.ToInt(CmbBranch.Value);
			obj.financialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.entryDate = DateTime.Now;
			obj.entryUserId = UserAccount.ID;
			obj.modifyDate = DateTime.Now;
			obj.modifyUserId = UserAccount.ID;
			obj.approvedDate = DateTime.Now;
			obj.isApproved = false;
			obj.isWhtApplied = chkWithHoldingTaxApplied.Checked;
			obj.isSupplierOtherChargesAllowed = chkOtherExpenseAllowed.Checked;
			obj.EBWeightDeductionTermId = (rdCompanyOwnedAndWeightCutApply.Checked ? 1 : (rdBagPurchaseAndWeightCutApply.Checked ? 2 : (rdBagFocAndWeightCutNotApply.Checked ? 3 : (RadBagReturnAndWeightCutApply.Checked ? 4 : 0))));
			obj.purchaseOrderDetailList = new List<purchaseOrderDetail>();
			if (lstRemoveRecordDetail.Count > 0 && RecId > 0)
			{
				for (int k = 0; k < lstRemoveRecordDetail.Count; k++)
				{
					obj.purchaseOrderDetailList.Add(lstRemoveRecordDetail[k]);
				}
			}
			decimal DetailSumAmount = default(decimal);
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				purchaseOrderDetail vd = new purchaseOrderDetail();
				vd.purchaseOrderDetailId = ((RecId != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
				vd.actionTypeId = ((vd.purchaseOrderDetailId <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(vd, r4);
				FormHelper.ValidateField(vd.inventoryParentCategoryId, "Parent Category", r4.RowIndex);
				FormHelper.ValidateField(vd.itemId, "Item Name", r4.RowIndex);
				FormHelper.ValidateField(vd.cropYearId, "Crop Year", r4.RowIndex);
				FormHelper.ValidateField(vd.packingTypeId, "Packing Type", r4.RowIndex);
				FormHelper.ValidateField(vd.packUomId, "Pack Uom", r4.RowIndex);
				FormHelper.ValidateField(vd.itemQty, "Qty", r4.RowIndex);
				FormHelper.ValidateField(vd.itemWeight, "Weight", r4.RowIndex);
				FormHelper.ValidateField(vd.itemRate, "Rate", r4.RowIndex);
				FormHelper.ValidateField(vd.rateUomId, "Rate Uom", r4.RowIndex);
				FormHelper.ValidateField(vd.ItemAmount, "Amount", r4.RowIndex);
				if (vd.TaxNameId > 0 || vd.TaxPercent > 0m || vd.TaxAmount > 0m)
				{
					FormHelper.ValidateField(vd.TaxNameId, "Tax Name", r4.RowIndex);
					FormHelper.ValidateField(vd.TaxPercent, "Tax Percent", r4.RowIndex);
					FormHelper.ValidateField(vd.TaxAmount, "Tax Amount", r4.RowIndex);
				}
				FormHelper.ValidateField(vd.TotalAmount, "Total Amount", r4.RowIndex);
				if (string.IsNullOrWhiteSpace(obj.remarksHeader))
				{
					obj.remarksHeader = vd.remarks;
				}
				DetailSumAmount += vd.TotalAmount;
				obj.purchaseOrderDetailList.Add(vd);
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r5 in rows)
			{
				if (Conversion.ToInt((object)r5.Cells["ItemId"].Value.ToString()) != 0 && Conversion.ToDouble(r5.Cells["Amount"].Value) > 0.0)
				{
					purchaseOrderSupplierExpenseDetail PE = new purchaseOrderSupplierExpenseDetail();
					PE.saleOrderMasterId = Conversion.ToInt(r5.Cells["SoId"].Value);
					PE.saleOrderBuyerOtherExpenseDetailId = Conversion.ToInt(r5.Cells["SoExpenseId"].Value);
					PE.ItemId = Conversion.ToInt(r5.Cells["ItemId"].Value);
					PE.OtherItemName = Conversion.ToString((object)r5.Cells["ItemId"].Text);
					PE.Qty = Conversion.ToDecimal(r5.Cells["Qty"].Value);
					PE.rate = Conversion.ToDouble(r5.Cells["Rate"].Value);
					PE.amount = Conversion.ToDouble(r5.Cells["Amount"].Value);
					PE.remarks = Conversion.ToString((object)r5.Cells["Remarks"].Text.Trim());
					if (PE.remarks == "0" || PE.remarks == string.Empty || PE.remarks == null)
					{
						PE.remarks = "Expense : " + Conversion.ToString((object)r5.Cells["ItemId"].Text.Trim()) + "  Qty" + PE.Qty + "  @" + PE.rate;
					}
					obj.purchaseOrderSupplierExpenseDetailList.Add(PE);
				}
			}
			if (obj.purchaseOrderSupplierExpenseDetailList != null)
			{
				obj.purchaseOrderSupplierExpenseDetailDescription = string.Join(", ", obj.purchaseOrderSupplierExpenseDetailList.Select((purchaseOrderSupplierExpenseDetail x) => $"[Item:{x.ItemId}:{x.OtherItemName}, Qty:{x.Qty},Rate:{x.rate},Amount:{x.amount}]"));
			}
			else
			{
				obj.purchaseOrderSupplierExpenseDetailDescription = string.Empty;
			}
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				purchaseOrderEmptyBagDetail emptyBags2 = new purchaseOrderEmptyBagDetail();
				emptyBags2.PackingTypeId = Conversion.ToInt(r3.Cells["PackingType"].Value);
				emptyBags2.PackingType = Conversion.ToString((object)r3.Cells["PackingType"].Text);
				emptyBags2.weightCutKg = Conversion.ToDecimal(r3.Cells["WeightCut"].Value);
				emptyBags2.entryTypeId = 1;
				if (!rdBagFocAndWeightCutNotApply.Checked)
				{
					if (emptyBags2.weightCutKg <= 0m)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdEmptyBags).Focus();
						throw new Exception("WeightCut filed required In Empty bags Grid...");
					}
					int validationTypeId = 0;
					if (emptyBags2.PackingTypeId == 1 || emptyBags2.PackingTypeId == 2 || emptyBags2.PackingTypeId == 5)
					{
						validationTypeId = emptyBags2.PackingTypeId;
					}
					else
					{
						validationTypeId = 2;
					}
					InvPackingType packType = clsGlobalVariables.globalInvPackingType.Find((InvPackingType row) => row.Id == validationTypeId);
					if (packType != null && (Conversion.ToDouble((object)emptyBags2.weightCutKg) < packType.MinEbWeight || Conversion.ToDouble((object)emptyBags2.weightCutKg) > packType.MaxEbWeight))
					{
						throw new Exception($"Weight Cut Should be in Range of: {packType.MinEbWeight} to {packType.MaxEbWeight}\n" + "For Packing Type:" + packType.PackTypeDesc);
					}
				}
				obj.PurchaseOrderEmptyBagDetailList.Add(emptyBags2);
			}
			rows = grdEmptyBagsPm.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt(r2.Cells["EmptyBagItem"].Value) > 0 && Conversion.ToInt(r2.Cells["PackingType"].Value) > 0)
				{
					purchaseOrderEmptyBagDetail emptyBags = new purchaseOrderEmptyBagDetail();
					emptyBags.PackingTypeId = Conversion.ToInt(r2.Cells["PackingType"].Value);
					emptyBags.PackingType = Conversion.ToString((object)r2.Cells["PackingType"].Text);
					emptyBags.Rate = Conversion.ToDecimal(r2.Cells["PurchaseRate"].Value);
					emptyBags.emptyBagPackingMaterialItemId = Conversion.ToInt(r2.Cells["EmptyBagItem"].Value);
					emptyBags.EmptyBagItem = Conversion.ToString((object)r2.Cells["EmptyBagItem"].Text);
					emptyBags.entryTypeId = 2;
					if (emptyBags.Rate <= 0m)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdEmptyBagsPm).Focus();
						throw new Exception("Rate filed required In Empty bags Pm Grid...");
					}
					obj.PurchaseOrderEmptyBagDetailList.Add(emptyBags);
				}
			}
			obj.purchaseOrderEmptyBagDetailDescription = ((obj.PurchaseOrderEmptyBagDetailList != null) ? string.Join(", ", from x in obj.PurchaseOrderEmptyBagDetailList
				where x.entryTypeId == 1
				select $"[PackingType:{x.PackingTypeId}:{x.PackingType}, WeightCut:{x.weightCutKg}]") : string.Empty);
			obj.purchaseOrderEmptyBagDetailDescriptionII = ((obj.PurchaseOrderEmptyBagDetailList != null) ? string.Join(", ", from x in obj.PurchaseOrderEmptyBagDetailList
				where x.entryTypeId == 2
				select $"[PackingType:{x.PackingTypeId}:{x.PackingType},Rate:{x.Rate}],EmptyBagItem:{x.EmptyBagItem}") : string.Empty);
			obj.PurchaseOrderCommissionDetailList = new List<purchaseOrderCommissionDetail>();
			if (Conversion.ToDecimal((object)txtCommAmount.Text) > 0m && Conversion.ToInt(CmbCommissionAc.Value) > 0)
			{
				purchaseOrderCommissionDetail comm2 = new purchaseOrderCommissionDetail();
				comm2.agentTypeId = 1;
				comm2.commissionAgentId = Conversion.ToInt(CmbCommissionAc.Value);
				comm2.commissionTypeId = Conversion.ToInt(cmbcommtype.Value);
				comm2.commissionRate = Conversion.ToDecimal((object)txtCommRate.Text);
				comm2.rateUomId = Conversion.ToInt(CmbCommUom.Value);
				comm2.commissionAmount = Conversion.ToDecimal((object)txtCommAmount.Text);
				obj.PurchaseOrderCommissionDetailList.Add(comm2);
			}
			if (Conversion.ToDecimal((object)txtBrokeryAmount.Text) > 0m && Conversion.ToInt(CmbBrokeryAc.Value) > 0)
			{
				purchaseOrderCommissionDetail comm = new purchaseOrderCommissionDetail();
				comm.agentTypeId = 2;
				comm.commissionAgentId = Conversion.ToInt(CmbBrokeryAc.Value);
				comm.commissionTypeId = Conversion.ToInt(CmbBrokeryType.Value);
				comm.commissionRate = Conversion.ToDecimal((object)txtBrokeryRate.Text);
				comm.rateUomId = Conversion.ToInt(CmbBrokeryRateUom.Value);
				comm.commissionAmount = Conversion.ToDecimal((object)txtBrokeryAmount.Text);
				obj.PurchaseOrderCommissionDetailList.Add(comm);
			}
			obj.purchaseOrderPaymentDetailList = new List<purchaseOrderPaymentDetail>();
			obj.SupplierOfferBuyerInquiryMappingDetailList = new List<SupplierOfferBuyerInquiryMappingDetail>();
			decimal num = Conversion.ToDecimal((object)GridEX_Helper.GetColumnSum(grdPaymentTerm, "TotalAmount"));
			decimal PaymentDetailAmount = default(decimal);
			decimal PrcntOfTotal = default(decimal);
			if (num > 0m)
			{
				rows = grdPaymentTerm.GetDataRows();
				foreach (GridEXRow item2 in rows)
				{
					purchaseOrderPaymentDetail pd = new purchaseOrderPaymentDetail();
					pd.PaymentTermId = Conversion.ToInt(item2.Cells["PaymentTerm"].Value);
					pd.PaymentTerm = Conversion.ToString((object)item2.Cells["PaymentTerm"].Text);
					pd.DueDays = Conversion.ToInt(item2.Cells["DueDays"].Value);
					pd.pctOfTotal = Conversion.ToDecimal(item2.Cells["%OfTotal"].Value);
					pd.dueAmount = Conversion.ToDecimal(item2.Cells["Amount"].Value);
					pd.BaseDueDateTypeId = Conversion.ToInt(item2.Cells["BaseDateType"].Value);
					pd.DueDate = ((pd.BaseDueDateTypeId == 1) ? new DateTime?(obj.docDate.AddDays(pd.DueDays)) : ((pd.BaseDueDateTypeId == 4) ? new DateTime?(Conversion.ToDateTime(item2.Cells["DueDate"].Value)) : null));
					PrcntOfTotal += pd.pctOfTotal;
					PaymentDetailAmount += pd.dueAmount;
					if (pd.PaymentTermId <= 0)
					{
						MessageBox.Show("Payment Term Required in row#" + (item2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPaymentTerm).Focus();
						return;
					}
					if (pd.PaymentTermId == 2 && pd.DueDays <= 0)
					{
						MessageBox.Show("Due Days Required In case Of Credit row in row#" + (item2.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPaymentTerm).Focus();
						return;
					}
					obj.purchaseOrderPaymentDetailList.Add(pd);
				}
			}
			else
			{
				purchaseOrderPaymentDetail psD = new purchaseOrderPaymentDetail();
				psD.PaymentTermId = Conversion.ToInt(CmbPaymentTerm.Value);
				psD.PaymentTerm = Conversion.ToString((object)((Control)(object)CmbPaymentTerm).Text);
				psD.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				psD.BaseDueDateTypeId = 1;
				psD.DueDate = obj.docDate.AddDays(psD.DueDays);
				psD.DueDate = obj.docDate.AddDays(psD.DueDays);
				psD.pctOfTotal = 100m;
				psD.dueAmount = DetailSumAmount;
				PaymentDetailAmount = psD.dueAmount;
				PrcntOfTotal = 100m;
				obj.purchaseOrderPaymentDetailList.Add(psD);
			}
			obj.paymentScheduleDescription = string.Join(", ", obj.purchaseOrderPaymentDetailList.Select((purchaseOrderPaymentDetail x) => $"[Term:{x.PaymentTermId}:{x.PaymentTerm},DueDays:{x.DueDays},%OfTotal:{x.pctOfTotal},DueAmount:{x.dueAmount},BaseDateType:{x.BaseDueDateTypeId}]"));
			if (Math.Abs(PaymentDetailAmount - DetailSumAmount) > 0.3m)
			{
				MessageBox.Show("Payment Detail Amount:" + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Amount:" + DetailSumAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 3;
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			PrcntOfTotal = Math.Round(PrcntOfTotal, 4);
			if (Math.Abs(100m - PrcntOfTotal) > 0.01m)
			{
				MessageBox.Show("Payment Detail Total% not near to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 3;
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			Dictionary<int, string> validItems = obj.purchaseOrderDetailList.ToDictionary((purchaseOrderDetail x) => x.itemId, (purchaseOrderDetail x) => x.ItemName);
			List<string> invalidItems = new List<string>();
			rows = grdInquiryMapping.GetDataRows();
			foreach (GridEXRow item in rows)
			{
				SupplierOfferBuyerInquiryMappingDetail d = new SupplierOfferBuyerInquiryMappingDetail();
				d.SupplierOfferBuyerInquiryMappingDetailId = Conversion.ToInt(item.Cells["Id"].Value);
				d.actionTypeId = ((d.SupplierOfferBuyerInquiryMappingDetailId <= 0) ? 1 : 2);
				d.inquiryBookingMasterId = Conversion.ToInt(item.Cells["InquiryId"].Value);
				if (d.inquiryBookingMasterId == 0)
				{
					throw new Exception("Inquiry Id not found. please check");
				}
				d.inquiryBookingDetailId = Conversion.ToInt(item.Cells["InquiryDetailId"].Value);
				if (d.inquiryBookingDetailId == 0)
				{
					throw new Exception("Inquiry detail Id not found. please check");
				}
				d.supplierId = Conversion.ToInt(CmbSupplierName.Value);
				d.buyerId = Conversion.ToInt(item.Cells["BuyerId"].Value);
				if (d.buyerId == 0)
				{
					throw new Exception("buyer not found. please check");
				}
				d.itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				if (d.itemId == 0)
				{
					throw new Exception("Item not found. please check");
				}
				d.itemNetWeight = Conversion.ToDecimal(item.Cells["CurrentAllocationWeight"].Value);
				if (d.itemNetWeight <= 0m)
				{
					throw new Exception("Allocation weight should be greater than '0'. please check");
				}
				if (!validItems.ContainsKey(d.itemId))
				{
					GridEXCell obj2 = item.Cells["ItemName"];
					string itemName = ((obj2 != null) ? obj2.Text : null) ?? $"ItemId {d.itemId}";
					invalidItems.Add(itemName);
				}
				else
				{
					obj.SupplierOfferBuyerInquiryMappingDetailList.Add(d);
				}
			}
			var invalidRow = obj.SupplierOfferBuyerInquiryMappingDetailList?.Select((SupplierOfferBuyerInquiryMappingDetail x, int i) => new { x, i }).FirstOrDefault(r => r.x.buyerId == obj.supplierId);
			if (invalidRow != null)
			{
				throw new Exception($"Row {invalidRow.i + 1}: Supplier and buyer cannot be the same in mapping grid.");
			}
			if (invalidItems.Any())
			{
				string itemList = string.Join(", ", invalidItems.Distinct());
				throw new Exception("Following item(s) are not present in Purchase Order Detail: " + itemList + ". Please delete those rows which contain these items from mapping.");
			}
			if (obj.purchaseOrderDetailList.Count > 0)
			{
				List<SupplierOfferBuyerInquiryMappingDetail> supplierOfferBuyerInquiryMappingDetailList = obj.SupplierOfferBuyerInquiryMappingDetailList;
				if (supplierOfferBuyerInquiryMappingDetailList != null && supplierOfferBuyerInquiryMappingDetailList.Count > 0)
				{
					foreach (purchaseOrderDetail detail in obj.purchaseOrderDetailList)
					{
						decimal sumOfWeight = obj.SupplierOfferBuyerInquiryMappingDetailList.Where((SupplierOfferBuyerInquiryMappingDetail x) => x.itemId == detail.itemId).Sum((SupplierOfferBuyerInquiryMappingDetail x) => x.itemNetWeight);
						if (sumOfWeight > detail.itemWeight)
						{
							MessageBox.Show($"Mapped weight ({sumOfWeight}) cannot be greater than Offer weight ({detail.itemWeight}) for Item {detail.ItemName}");
							return;
						}
					}
				}
			}
			if (lstRemoveRecordMappingDetail.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecordMappingDetail.Count; j++)
				{
					obj.SupplierOfferBuyerInquiryMappingDetailList.Add(lstRemoveRecordMappingDetail[j]);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<purchaseOrderMaster>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = purchaseOrderMaster.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			Reset();
			if (chkPrint.Checked)
			{
				OpenSlip(success);
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int recordId)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			Reset();
			RecId = recordId;
			purchaseOrderMaster obj = new purchaseOrderMaster();
			obj = purchaseOrderMaster.ReadById(recordId);
			if (obj == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtDocNo.Text = Conversion.ToString((object)obj.docNo);
			datDocDate.Value = obj.docDate;
			CmbCompanyName.Value = Conversion.ToInt((object)obj.companyId);
			CmbBranch.Value = Conversion.ToInt((object)obj.branchId);
			CmbCommissionAgent.Value = Conversion.ToInt((object)obj.commissionAgentId);
			CmbSupplierName.Value = Conversion.ToInt((object)obj.supplierId);
			CmbDeliveryToParty.Value = Conversion.ToInt((object)obj.DeliveryToPartyId);
			CmbDeliveryToParty_Leave(null, null);
			if (obj.shipToAddressId > 0)
			{
				CmbShipToAddress.Value = Conversion.ToInt((object)obj.shipToAddressId);
			}
			CmbDeliveryTerm.Value = obj.deliveryTermId;
			txtDeliveryDays.Text = Conversion.ToString((object)obj.deliveryDays);
			datDeliveryStartDate.Value = Conversion.ToDateTime((object)obj.deliveryStartDate);
			datExpiryDate.Value = Conversion.ToDateTime((object)obj.ValidityDate);
			txtRemarks.Text = Conversion.ToString((object)obj.remarksHeader);
			if (obj.PurchaseOrderCommissionDetailList.Count > 0)
			{
				purchaseOrderCommissionDetail CommissionObj = obj.PurchaseOrderCommissionDetailList.FirstOrDefault((purchaseOrderCommissionDetail r) => r.agentTypeId == 1);
				if (CommissionObj != null)
				{
					CmbCommissionAc.Value = CommissionObj.commissionAgentId;
					cmbcommtype.Value = CommissionObj.commissionTypeId;
					txtCommRate.Text = Conversion.ToString((object)CommissionObj.commissionRate);
					if (CommissionObj.rateUomId > 0)
					{
						CmbCommUom.Value = CommissionObj.rateUomId;
					}
					txtCommAmount.Text = Conversion.ToString((object)CommissionObj.commissionAmount);
				}
				purchaseOrderCommissionDetail BrokeryObj = obj.PurchaseOrderCommissionDetailList.FirstOrDefault((purchaseOrderCommissionDetail r) => r.agentTypeId == 2);
				if (BrokeryObj != null)
				{
					CmbBrokeryAc.Value = BrokeryObj.commissionAgentId;
					CmbBrokeryType.Value = BrokeryObj.commissionTypeId;
					txtBrokeryRate.Text = Conversion.ToString((object)BrokeryObj.commissionRate);
					if (BrokeryObj.rateUomId > 0)
					{
						CmbBrokeryRateUom.Value = BrokeryObj.rateUomId;
					}
					txtBrokeryAmount.Text = Conversion.ToString((object)BrokeryObj.commissionAmount);
				}
			}
			txtPaymentScheduleRemarks.Text = Conversion.ToString((object)obj.paymentScheduleDescription);
			chkOtherExpenseAllowed.Checked = obj.isSupplierOtherChargesAllowed;
			chkWithHoldingTaxApplied.Checked = obj.isWhtApplied;
			if (obj.EBWeightDeductionTermId == 1)
			{
				rdCompanyOwnedAndWeightCutApply.Checked = true;
			}
			else if (obj.EBWeightDeductionTermId == 2)
			{
				rdBagPurchaseAndWeightCutApply.Checked = true;
			}
			else if (obj.EBWeightDeductionTermId == 3)
			{
				rdBagFocAndWeightCutNotApply.Checked = true;
			}
			else if (obj.EBWeightDeductionTermId == 4)
			{
				RadBagReturnAndWeightCutApply.Checked = true;
			}
			AttachmentsValues = obj.attachmentsValues;
			CustomAttachmentsValues = obj.customAttachmentsValues;
			if (obj.purchaseOrderDetailList.Count > 0)
			{
				dtDetail.Rows.Clear();
				PurchaseOrderCmagt_Helper.FillDetailFromListCommonForReadById(obj, dtDetail);
			}
			dtEmptyBags.Rows.Clear();
			dtEmptyBagsPM.Rows.Clear();
			Dictionary<int, List<purchaseOrderEmptyBagDetail>> grouped = obj.PurchaseOrderEmptyBagDetailList?.GroupBy((purchaseOrderEmptyBagDetail r) => r.entryTypeId).ToDictionary((IGrouping<int, purchaseOrderEmptyBagDetail> g) => g.Key, (IGrouping<int, purchaseOrderEmptyBagDetail> g) => g.ToList());
			if (grouped != null)
			{
				if (grouped.TryGetValue(1, out var ebList))
				{
					foreach (purchaseOrderEmptyBagDetail item2 in ebList)
					{
						dtEmptyBags.Rows.Add(item2.saleOrderMasterId, item2.saleOrderEmptyBagDetailId, item2.SaleOrderNo, item2.PackingTypeId, item2.weightCutKg);
					}
				}
				if (grouped.TryGetValue(2, out var ebPmList))
				{
					foreach (purchaseOrderEmptyBagDetail item in ebPmList)
					{
						dtEmptyBagsPM.Rows.Add(item.PackingTypeId, item.Rate, item.emptyBagPackingMaterialItemId);
					}
				}
			}
			dtExpGrid.Rows.Clear();
			AddRowsInExpenseGrid();
			for (int k = 0; k < obj.purchaseOrderSupplierExpenseDetailList.Count; k++)
			{
				DataRow[] rowToUpdate = dtExpGrid.Select($"ItemId = '{obj.purchaseOrderSupplierExpenseDetailList[k].ItemId}'");
				if (rowToUpdate != null)
				{
					rowToUpdate[0].SetField("Id", obj.purchaseOrderSupplierExpenseDetailList[k].purchaseOrderSupplierExpenseDetailId);
					rowToUpdate[0].SetField("SoId", obj.purchaseOrderSupplierExpenseDetailList[k].saleOrderMasterId);
					rowToUpdate[0].SetField("SoExpenseId", obj.purchaseOrderSupplierExpenseDetailList[k].saleOrderBuyerOtherExpenseDetailId);
					rowToUpdate[0].SetField("SaleOrderNo", Conversion.ToInt((object)obj.purchaseOrderSupplierExpenseDetailList[k].SaleOrderNo));
					rowToUpdate[0].SetField("ItemId", obj.purchaseOrderSupplierExpenseDetailList[k].ItemId);
					rowToUpdate[0].SetField("Qty", obj.purchaseOrderSupplierExpenseDetailList[k].Qty);
					rowToUpdate[0].SetField("Rate", obj.purchaseOrderSupplierExpenseDetailList[k].rate);
					rowToUpdate[0].SetField("Amount", obj.purchaseOrderSupplierExpenseDetailList[k].amount);
					rowToUpdate[0].SetField("Remarks", obj.purchaseOrderSupplierExpenseDetailList[k].remarks);
				}
			}
			dtPaymentTerm.Rows.Clear();
			for (int j = 0; j < obj.purchaseOrderPaymentDetailList.Count; j++)
			{
				dtPaymentTerm.Rows.Add(obj.purchaseOrderPaymentDetailList[j].PaymentTermId, obj.purchaseOrderPaymentDetailList[j].DueDays, obj.purchaseOrderPaymentDetailList[j].pctOfTotal, obj.purchaseOrderPaymentDetailList[j].dueAmount, obj.purchaseOrderPaymentDetailList[j].BaseDueDateTypeId, obj.purchaseOrderPaymentDetailList[j].DueDate);
			}
			dtSaleOrderMap.Rows.Clear();
			for (int i = 0; i < obj.SupplierOfferBuyerInquiryMappingDetailList?.Count; i++)
			{
				dtSaleOrderMap.Rows.Add(obj.SupplierOfferBuyerInquiryMappingDetailList[i].SupplierOfferBuyerInquiryMappingDetailId, obj.SupplierOfferBuyerInquiryMappingDetailList[i].inquiryBookingMasterId, obj.SupplierOfferBuyerInquiryMappingDetailList[i].inquiryBookingDetailId, obj.SupplierOfferBuyerInquiryMappingDetailList[i].InquiryBookingNo, obj.SupplierOfferBuyerInquiryMappingDetailList[i].buyerId, obj.SupplierOfferBuyerInquiryMappingDetailList[i].BuyerName, obj.SupplierOfferBuyerInquiryMappingDetailList[i].itemId, obj.SupplierOfferBuyerInquiryMappingDetailList[i].ItemName, obj.SupplierOfferBuyerInquiryMappingDetailList[i].InquiryBookingQty, obj.SupplierOfferBuyerInquiryMappingDetailList[i].UsedinquiryBookingWeightInPoOffer, obj.SupplierOfferBuyerInquiryMappingDetailList[i].inquiryBookingBalWeight, obj.SupplierOfferBuyerInquiryMappingDetailList[i].itemNetWeight);
			}
			BindGrids();
			if (obj.purchaseOrderPaymentDetailList.Count == 1)
			{
				UltraCombo cmbPaymentTerm = CmbPaymentTerm;
				List<purchaseOrderPaymentDetail> purchaseOrderPaymentDetailList = obj.purchaseOrderPaymentDetailList;
				cmbPaymentTerm.Value = ((purchaseOrderPaymentDetailList != null) ? new int?(purchaseOrderPaymentDetailList.FirstOrDefault().PaymentTermId) : null);
				TextBox textBox = txtDueDays;
				List<purchaseOrderPaymentDetail> purchaseOrderPaymentDetailList2 = obj.purchaseOrderPaymentDetailList;
				textBox.Text = Conversion.ToString((object)((purchaseOrderPaymentDetailList2 != null) ? new int?(purchaseOrderPaymentDetailList2.FirstOrDefault().DueDays) : null));
			}
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			datDocDate.Focus();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
					purchaseOrderMaster.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
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

	private void btnAttachments_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			OpenSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenSlip(int PrintId)
	{
		try
		{
			if (PrintId > 0)
			{
				CrystalReportPrint_Helper.SupplierOfferSlip_1051(PrintId);
				return;
			}
			throw new Exception("No Data found to display");
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
			if (datDeliveryStartDate.Value.Date < datDocDate.Value.Date)
			{
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
				datDeliveryStartDate.Value = datDocDate.Value;
			}
			else
			{
				CalculateExpiryDate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateExpiryDate()
	{
		try
		{
			if (Conversion.ToInt((object)txtDeliveryDays.Text) == 0)
			{
				datExpiryDate.Value = datDeliveryStartDate.Value;
			}
			else
			{
				datExpiryDate.Value = datDeliveryStartDate.Value.AddDays(Conversion.ToDouble((object)txtDeliveryDays.Text.Trim()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDeliveryDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateExpiryDate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbParentItemHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			BindDropdownsAgainstParentCategory();
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
			((Control)(object)CmbItemNameHistory).Text = string.Empty;
			((Control)(object)CmbCommissionAgentHistory).Text = string.Empty;
			((Control)(object)CmbBuyerNameHistory).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboDbCall();
			if (((Control)(object)CmbParentItemHistory).Text != null && ((Control)(object)CmbParentItemHistory).Text != string.Empty)
			{
				BindDropdownsAgainstParentCategory();
			}
			else
			{
				HistoryComboBind(dtHistoryCombo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			if (dtcombo == null || dtcombo.Rows.Count == 0)
			{
				return;
			}
			DataTable obj = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"Name"
				}
			};
			DataTable dtCA = obj.Clone();
			DataTable dtBuyer = obj.Clone();
			DataTable dtParentCategory = obj.Clone();
			DataTable dtItem = obj.Clone();
			DataTable dtMostUsedPC = obj.Clone();
			DataTable dtDeliveryParty = obj.Clone();
			DataTable dtDeliverAddress = obj.Clone();
			Dictionary<string, List<DataRow>> dtLookup = (from r in dtcombo.AsEnumerable()
				group r by r["Activity"]?.ToString() ?? "").ToDictionary((IGrouping<string, DataRow> g) => g.Key, (IGrouping<string, DataRow> g) => g.ToList());
			int mostUsedId = 0;
			if (dtLookup.TryGetValue("GetMostUsedParentCategoryId", out var most))
			{
				DataRow r3 = most.FirstOrDefault();
				if (r3 != null)
				{
					mostUsedId = Conversion.ToInt(r3["Id"]);
					dtMostUsedPC.Rows.Add(r3["Id"], r3["ReferenceName"]);
				}
			}
			dtCA.BeginLoadData();
			dtBuyer.BeginLoadData();
			dtItem.BeginLoadData();
			dtParentCategory.BeginLoadData();
			dtDeliveryParty.BeginLoadData();
			dtDeliverAddress.BeginLoadData();
			if (dtLookup.TryGetValue("ParentCategories", out var parents))
			{
				foreach (DataRow r2 in parents)
				{
					dtParentCategory.LoadDataRow(new object[2]
					{
						r2["Id"],
						r2["ReferenceName"]
					}, fAcceptChanges: true);
				}
			}
			Load("CommissionAgent", dtCA);
			Load("SupplierName", dtBuyer);
			Load("Item", dtItem);
			Load("DeliveryToParty", dtDeliveryParty);
			Load("DeliverToAddress", dtDeliverAddress);
			dtCA.EndLoadData();
			dtBuyer.EndLoadData();
			dtItem.EndLoadData();
			dtParentCategory.EndLoadData();
			dtDeliveryParty.EndLoadData();
			dtDeliverAddress.EndLoadData();
			Bind(CmbCommissionAgentHistory, dtCA, "Commission Agent");
			Bind(CmbBuyerNameHistory, dtBuyer, "SupplierName");
			Bind(CmbItemNameHistory, dtItem, "Buyer");
			Bind(CmbDeliveryToPartyHistory, dtDeliveryParty, "Deliver To Party");
			Bind(CmbDeliverToAddressHistory, dtDeliverAddress, "Deliver To Address");
			DDL.BindDDL(dtParentCategory, CmbParentItemHistory, "Id", "name", "Parent Categories", ZeroIndex: false);
			((UltraGridBase)CmbParentItemHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridBand band = ((UltraGridBase)CmbParentItemHistory).DisplayLayout.Bands[0];
			if (!((KeyedSubObjectsCollectionBase)band.Columns).Exists("Selected"))
			{
				UltraGridColumn obj2 = band.Columns.Add("Selected");
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
			}
			CmbParentItemHistory.LimitToList = false;
			CmbParentItemHistory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			if (dtMostUsedPC.Rows.Count > 0)
			{
				((Control)(object)CmbParentItemHistory).Text = Conversion.ToString(dtMostUsedPC.Rows[0]["Name"]);
			}
			void Load(string key, DataTable target)
			{
				if (!dtLookup.TryGetValue(key, out var list))
				{
					return;
				}
				foreach (DataRow r4 in list)
				{
					if (Conversion.ToInt(r4["ParentCategoryId"]) == mostUsedId)
					{
						target.LoadDataRow(new object[2]
						{
							r4["Id"],
							r4["ReferenceName"]
						}, fAcceptChanges: true);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		static void Bind(UltraCombo combo, DataTable dt, string caption)
		{
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Name", caption, AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
	}

	private void BindDropdownsAgainstParentCategory()
	{
		try
		{
			string ParentIds = "";
			if (!string.IsNullOrWhiteSpace(((Control)(object)CmbParentItemHistory).Text))
			{
				DataTable dt2 = (DataTable)((UltraGridBase)CmbParentItemHistory).DataSource;
				string[] array = ((Control)(object)CmbParentItemHistory).Text.Trim().Split(',');
				foreach (string name in array)
				{
					DataRow[] dr = dt2.Select("name='" + Conversion.ToString((object)name) + "'");
					if (dr.Length != 0)
					{
						ParentIds = ParentIds + Conversion.ToString(dr[0]["Id"]) + ",";
					}
				}
			}
			if (ParentIds.EndsWith(","))
			{
				ParentIds = ParentIds.TrimEnd(',');
			}
			DataTable dtcombo = dtHistoryCombo;
			DataTable dtBase = new DataTable();
			dtBase.Columns.Add("Id", typeof(int));
			dtBase.Columns.Add("Name");
			DataTable dtCA = dtBase.Clone();
			DataTable dtBuyer = dtBase.Clone();
			DataTable dtItem = dtBase.Clone();
			DataTable dtDeliveryParty = dtBase.Clone();
			DataTable dtDeliverAddress = dtBase.Clone();
			HashSet<int> CaIds = new HashSet<int>();
			HashSet<int> BuyerIds = new HashSet<int>();
			HashSet<int> DeliveryPartyIds = new HashSet<int>();
			HashSet<int> DeliverAddressIds = new HashSet<int>();
			Dictionary<string, Action<DataRow>> map = new Dictionary<string, Action<DataRow>>(StringComparer.OrdinalIgnoreCase)
			{
				["CommissionAgent"] = delegate(DataRow r)
				{
					int num4 = Conversion.ToInt(r["Id"]);
					if (CaIds.Add(num4))
					{
						dtCA.Rows.Add(num4, r["ReferenceName"]);
					}
				},
				["SupplierName"] = delegate(DataRow r)
				{
					int num3 = Conversion.ToInt(r["Id"]);
					if (BuyerIds.Add(num3))
					{
						dtBuyer.Rows.Add(num3, r["ReferenceName"]);
					}
				},
				["Item"] = delegate(DataRow r)
				{
					dtItem.Rows.Add(r["Id"], r["ReferenceName"]);
				},
				["DeliveryToParty"] = delegate(DataRow r)
				{
					int num2 = Conversion.ToInt(r["Id"]);
					if (DeliveryPartyIds.Add(num2))
					{
						dtDeliveryParty.Rows.Add(num2, r["ReferenceName"]);
					}
				},
				["DeliverToAddress"] = delegate(DataRow r)
				{
					int num = Conversion.ToInt(r["Id"]);
					if (DeliverAddressIds.Add(num))
					{
						dtDeliverAddress.Rows.Add(num, r["ReferenceName"]);
					}
				}
			};
			bool hasFilter = !string.IsNullOrEmpty(ParentIds);
			HashSet<string> parentSet = (hasFilter ? new HashSet<string>(ParentIds.Split(',')) : null);
			foreach (DataRow row in dtcombo.Rows)
			{
				if (!hasFilter || parentSet.Contains(row["ParentCategoryId"].ToString()))
				{
					string activity = row["Activity"].ToString();
					if (map.TryGetValue(activity, out var action))
					{
						action(row);
					}
				}
			}
			Bind(CmbCommissionAgentHistory, dtCA, "Commission Agent");
			Bind(CmbBuyerNameHistory, dtBuyer, "SupplierName");
			Bind(CmbItemNameHistory, dtItem, "Buyer");
			Bind(CmbDeliveryToPartyHistory, dtDeliveryParty, "Deliver To Party");
			Bind(CmbDeliverToAddressHistory, dtDeliverAddress, "Deliver To Address");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
		static void Bind(UltraCombo combo, DataTable dt, string caption)
		{
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Name", caption, AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
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
			if (datValidityFromHistory.Checked)
			{
				obj.ValidityDateFrom = datValidityFromHistory.Value;
			}
			if (datValidityToHistory.Checked)
			{
				obj.ValidityDateTo = datValidityToHistory.Value;
			}
			string ParentIds = "";
			if (((Control)(object)CmbParentItemHistory).Text != null && ((Control)(object)CmbParentItemHistory).Text != string.Empty)
			{
				DataTable dt2 = (DataTable)((UltraGridBase)CmbParentItemHistory).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbParentItemHistory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr2 = dt2.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr2.Length != 0)
					{
						ParentIds = ParentIds + Conversion.ToString(dr2[0].ItemArray[0]) + ",";
					}
				}
			}
			obj.ParentCategoryIds = ParentIds;
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.CommissionAgentId = Conversion.ToInt(CmbCommissionAgentHistory.Value);
			obj.SupplierId = Conversion.ToInt(CmbBuyerNameHistory.Value);
			obj.DeliverToPartyId = Conversion.ToInt(CmbDeliveryToPartyHistory.Value);
			obj.ShipToAddress = Conversion.ToString((object)((Control)(object)CmbDeliverToAddressHistory).Text);
			DataTable dtHistory = purchaseOrderMaster.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CommissionAgent", typeof(string));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("DeliverToParty", typeof(string));
				dt.Columns.Add("DeliverToAddress", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dt.Columns.Add("DeliveryDays", typeof(int));
				dt.Columns.Add("ValidityDate", typeof(DateTime));
				dt.Columns.Add("WhtApplied", typeof(bool));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dtHistoryForLayout = dt.Clone();
				foreach (DataRow row in dtHistory.Rows)
				{
					DataRow dr = dt.NewRow();
					dr["Id"] = Conversion.ToInt(row["purchaseOrderMasterId"]);
					dr["DocNo"] = Conversion.ToInt(row["docNo"]);
					dr["DocDate"] = Conversion.ToDateTime(row["docDate"]).ToShortDateString();
					dr["CommissionAgent"] = Conversion.ToString(row["CommissionAgentName"]);
					dr["SupplierName"] = Conversion.ToString(row["SupplierName"]);
					dr["DeliverToParty"] = Conversion.ToString(row["DeliveryToPartyName"]);
					dr["DeliverToAddress"] = Conversion.ToString(row["ShipToAddress"]);
					dr["DeliveryTerm"] = Conversion.ToString(row["DeliveryTerm"]);
					dr["DeliveryStartDate"] = ((row["deliveryStartDate"] == DBNull.Value) ? DBNull.Value : ((object)Conversion.ToDateTime(row["deliveryStartDate"])));
					dr["DeliveryDays"] = ((row["deliveryDays"] != DBNull.Value) ? Conversion.ToInt(row["deliveryDays"]) : 0);
					dr["ValidityDate"] = ((row["ValidityDate"] == DBNull.Value) ? DBNull.Value : ((object)Conversion.ToDateTime(row["ValidityDate"])));
					dr["WhtApplied"] = row["isWhtApplied"] != DBNull.Value && Conversion.ToBool(row["isWhtApplied"]);
					dr["Status"] = Conversion.ToString(row["Status"]);
					dr["EntryUser"] = Conversion.ToString(row["EntryUserName"]);
					dr["EntryDate"] = Conversion.ToDateTime(row["EntryDate"]);
					dr["ModifyUser"] = Conversion.ToString(row["ModifyUserName"]);
					dr["ModifyDate"] = row["ModifyDate"];
					dr["NoOfAttachments"] = Conversion.ToInt(row["NoOfAttachments"]);
					dt.Rows.Add(dr);
				}
				dtHistoryForLayout = dt.Copy();
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				GridSettingHistory();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistoryDetail.ClearStructure();
				grdEmptyBagHistory.ClearStructure();
				grdEmptyBagPmHistory.ClearStructure();
				grdExpenseHistory.ClearStructure();
				grdPaymentTermHistory.ClearStructure();
				grdSaleOrderMappingHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingHistory()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			List<string> NotTotalColumns = new List<string> { "BuyerTargetPrice", "TargetPurchasePrice" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["DeliveryStartDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["ValidityDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["ValidityDate"].Caption = "Expiry Date";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["DeliverToParty"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["DeliverToAddress"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdHistory.RootTable.Columns["DeliveryTerm"].Width = Constants.InventoryConstants.TermConstant;
			grdHistory.RootTable.Columns["DeliveryStartDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["DeliveryDays"].Width = 60;
			grdHistory.RootTable.Columns["ValidityDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["WhtApplied"].Width = 60;
			grdHistory.RootTable.Columns["Status"].Width = 70;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 0);
			GridEX_Helper.AddButton(grdHistory, "SaveAs", "SaveAs", 60, 2);
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 40, 3);
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnDelete.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			datDocDate.Value = DateTime.Now;
			dtSaleOrderMap.Rows.Clear();
			if (dtSaleOrderMap.Columns.Count > 0)
			{
				grdInquiryMapping.DataSource = dtSaleOrderMap;
				grdInquiryMapping.RetrieveStructure();
				grdSaleOrderMappingSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				else if (e.Column.Key == "Print")
				{
					OpenSlip(Id);
				}
				if (e.Column.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(Id);
				}
			}
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
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
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
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			purchaseOrderMaster Obj = purchaseOrderMaster.ReadById(Id);
			if (Obj != null && Obj.purchaseOrderDetailList.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				grdEmptyBagHistory.ClearStructure();
				grdEmptyBagPmHistory.ClearStructure();
				grdExpenseHistory.ClearStructure();
				grdPaymentTermHistory.ClearStructure();
				grdSaleOrderMappingHistory.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtDetail.Clone();
			PurchaseOrderCmagt_Helper.FillDetailFromListCommonForReadById(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			HistoryDetailGridSetting();
			DataTable dtExpense = new DataTable();
			dtExpense.Columns.Add("OtherItemName", typeof(string));
			dtExpense.Columns.Add("Qty", typeof(decimal));
			dtExpense.Columns.Add("Rate", typeof(decimal));
			dtExpense.Columns.Add("Amount", typeof(decimal));
			dtExpense.Columns.Add("Remarks", typeof(string));
			for (int l = 0; l < Obj.purchaseOrderSupplierExpenseDetailList.Count; l++)
			{
				dtExpense.Rows.Add(Obj.purchaseOrderSupplierExpenseDetailList[l].OtherItemName, Obj.purchaseOrderSupplierExpenseDetailList[l].Qty, Obj.purchaseOrderSupplierExpenseDetailList[l].rate, Obj.purchaseOrderSupplierExpenseDetailList[l].amount, Obj.purchaseOrderSupplierExpenseDetailList[l].remarks);
			}
			grdExpenseHistory.DataSource = dtExpense;
			grdExpenseHistory.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(grdExpenseHistory, 2, 3);
			grdExpenseHistory.RootTable.Columns["OtherItemName"].Width = Constants.InventoryConstants.ItemName;
			grdExpenseHistory.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdExpenseHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdExpenseHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdExpenseHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 200;
			DataTable dtEb = new DataTable();
			dtEb.Columns.Add("PackingType", typeof(string));
			dtEb.Columns.Add("WeightCut", typeof(decimal));
			DataTable dtEbPm = new DataTable();
			dtEbPm.Columns.Add("PackingType", typeof(string));
			dtEbPm.Columns.Add("PurchaseRate", typeof(decimal));
			dtEbPm.Columns.Add("EmptyBagItem", typeof(string));
			for (int k = 0; k < Obj.PurchaseOrderEmptyBagDetailList.Count; k++)
			{
				if (Obj.PurchaseOrderEmptyBagDetailList[k].entryTypeId == 1)
				{
					dtEb.Rows.Add(Obj.PurchaseOrderEmptyBagDetailList[k].PackingType, Obj.PurchaseOrderEmptyBagDetailList[k].weightCutKg);
				}
				else if (Obj.PurchaseOrderEmptyBagDetailList[k].entryTypeId == 2)
				{
					dtEbPm.Rows.Add(Obj.PurchaseOrderEmptyBagDetailList[k].PackingType, Obj.PurchaseOrderEmptyBagDetailList[k].Rate, Obj.PurchaseOrderEmptyBagDetailList[k].EmptyBagItem);
				}
			}
			grdEmptyBagHistory.DataSource = dtEb;
			grdEmptyBagHistory.RetrieveStructure();
			List<string> excludeColumnsFromSum = new List<string> { "WeightCut" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBagHistory, 2, 3, null, null, null, ColumnSetting: true, BracketsForamt: false, excludeColumnsFromSum);
			grdEmptyBagHistory.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.ItemName;
			grdEmptyBagHistory.RootTable.Columns["WeightCut"].Width = Constants.InventoryConstants.WeightConstant;
			grdEmptyBagPmHistory.DataSource = dtEbPm;
			grdEmptyBagPmHistory.RetrieveStructure();
			List<string> excludeColumnsFromSumPm = new List<string> { "PurchaseRate" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdEmptyBagPmHistory, 2, 3, null, null, null, ColumnSetting: true, BracketsForamt: false, excludeColumnsFromSumPm);
			grdEmptyBagPmHistory.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.ItemName;
			grdEmptyBagPmHistory.RootTable.Columns["PurchaseRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdEmptyBagPmHistory.RootTable.Columns["EmptyBagItem"].Width = Constants.InventoryConstants.ItemName;
			DataTable dtTerm = new DataTable();
			dtTerm.Columns.Add("PaymentTerm", typeof(string));
			dtTerm.Columns.Add("DueDays", typeof(int));
			dtTerm.Columns.Add("%OfTotal", typeof(decimal));
			dtTerm.Columns.Add("Amount", typeof(decimal));
			dtTerm.Columns.Add("BaseDateType", typeof(string));
			dtTerm.Columns.Add("DueDate", typeof(DateTime));
			for (int j = 0; j < Obj.purchaseOrderPaymentDetailList.Count; j++)
			{
				dtTerm.Rows.Add(Obj.purchaseOrderPaymentDetailList[j].PaymentTerm, Obj.purchaseOrderPaymentDetailList[j].DueDays, Obj.purchaseOrderPaymentDetailList[j].pctOfTotal, Obj.purchaseOrderPaymentDetailList[j].dueAmount, Obj.purchaseOrderPaymentDetailList[j].BaseDateType, Obj.purchaseOrderPaymentDetailList[j].DueDate);
			}
			grdPaymentTermHistory.DataSource = dtTerm;
			grdPaymentTermHistory.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(grdPaymentTermHistory, 2, 3);
			grdPaymentTermHistory.RootTable.Columns["PaymentTerm"].Width = Constants.InventoryConstants.TermConstant + 10;
			grdPaymentTermHistory.RootTable.Columns["DueDays"].Width = Constants.InventoryConstants.QtyConstant;
			grdPaymentTermHistory.RootTable.Columns["%OfTotal"].Width = Constants.InventoryConstants.ExRateConstant;
			grdPaymentTermHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdPaymentTermHistory.RootTable.Columns["BaseDateType"].Width = Constants.InventoryConstants.StringConstant;
			grdPaymentTermHistory.RootTable.Columns["DueDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			DataTable dtMapping = new DataTable();
			dtMapping.Columns.Add("InquiryNo", typeof(int));
			dtMapping.Columns.Add("Buyer", typeof(string));
			dtMapping.Columns.Add("ItemName", typeof(string));
			dtMapping.Columns.Add("ItemTotalWeight", typeof(decimal));
			dtMapping.Columns.Add("BookedWeight", typeof(decimal));
			dtMapping.Columns.Add("BalanceWeight", typeof(decimal));
			dtMapping.Columns.Add("CurrentAllocatedWeight", typeof(decimal));
			for (int i = 0; i < Obj.SupplierOfferBuyerInquiryMappingDetailList?.Count; i++)
			{
				dtMapping.Rows.Add(Obj.SupplierOfferBuyerInquiryMappingDetailList[i].InquiryBookingNo, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].BuyerName, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].ItemName, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].InquiryBookingWeight, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].UsedinquiryBookingWeightInPoOffer, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].inquiryBookingBalWeight, Obj.SupplierOfferBuyerInquiryMappingDetailList[i].itemNetWeight);
			}
			grdSaleOrderMappingHistory.DataSource = dtMapping;
			grdSaleOrderMappingHistory.RetrieveStructure();
			List<string> excludeColumnsFromSumMap = new List<string> { "ItemTotalWeight", "BookedWeight", "BalWeight" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSaleOrderMappingHistory, 2, 3, null, null, null, ColumnSetting: true, BracketsForamt: false, excludeColumnsFromSumMap);
			grdSaleOrderMappingHistory.RootTable.Columns["InquiryNo"].Width = Constants.InventoryConstants.DocNoConstant + 10;
			grdSaleOrderMappingHistory.RootTable.Columns["Buyer"].Width = Constants.InventoryConstants.SupplierName + 15;
			grdSaleOrderMappingHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdSaleOrderMappingHistory.RootTable.Columns["ItemTotalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMappingHistory.RootTable.Columns["BookedWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMappingHistory.RootTable.Columns["BalanceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdSaleOrderMappingHistory.RootTable.Columns["CurrentAllocatedWeight"].Width = Constants.InventoryConstants.WeightConstant + 20;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSetting()
	{
		try
		{
			PurchaseOrderCmagt_Helper.DetailGridCommonSetting(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (!formright.DoHavePrintRights)
					{
						throw new Exception("you don't have print rights...");
					}
					OpenSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
				if (grdHistory.CurrentColumn.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryForLayout);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtDetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInvExp, dtExpGrid);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdEmptyBags, dtEmptyBags);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar5_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdPaymentTerm, dtPaymentTerm);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdInquiryMapping, dtSaleOrderMap);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSupplierOfferCmagt_KeyDown(object sender, KeyEventArgs e)
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
					datDocDate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (btnSaveAs.Visible && btnSaveAs.Enabled)
					{
						btnSaveAs_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					datDocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdDetail).Focused)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdInvExp).Focus();
					}
					else if (((Control)(object)grdInvExp).Focused)
					{
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdEmptyBags).Focus();
					}
					else if (((Control)(object)grdEmptyBags).Focused)
					{
						tabControl2.SelectedIndex = 3;
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (((Control)(object)grdPaymentTerm).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdDetail).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbParentItem.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Inquiry Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Parent Item");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Inquiry Date");
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

	private void btnLoadSo_Click(object sender, EventArgs e)
	{
		try
		{
			int DelPartryId = Conversion.ToInt(CmbDeliveryToParty.Value);
			string ShipToAddress = Conversion.ToString((object)((Control)(object)CmbShipToAddress).Text);
			if (string.IsNullOrEmpty(ShipToAddress))
			{
				ShipToAddress = Conversion.ToString((object)txtShipToAddress.Text);
			}
			frmBuyerInquiryLoaderForOffer LoadSo = new frmBuyerInquiryLoaderForOffer(UserAccount);
			if (grdDetail.GetRows().Count() > 0)
			{
				if (LoadSo.dtDistinctItems.Columns.Count == 0)
				{
					LoadSo.dtDistinctItems.Columns.Add("ItemId", typeof(int));
					LoadSo.dtDistinctItems.Columns.Add("ItemName", typeof(string));
				}
				LoadSo.dtDistinctItems = (from r in dtDetail.AsEnumerable()
					group r by new
					{
						ItemId = r.Field<int>("ItemId"),
						ItemName = r.Field<string>("ItemName")
					}).Select(g =>
				{
					DataRow dataRow = LoadSo.dtDistinctItems.NewRow();
					dataRow["ItemId"] = g.Key.ItemId;
					dataRow["ItemName"] = g.Key.ItemName;
					return dataRow;
				}).CopyToDataTable();
			}
			if (DelPartryId > 0)
			{
				LoadSo.CmbDeliveryToParty.Value = DelPartryId;
			}
			if (!string.IsNullOrEmpty(ShipToAddress))
			{
				((Control)(object)LoadSo.CmbShipToAddress).Text = ShipToAddress;
			}
			LoadSo.ShowDialog();
			LoadSo.BringToFront();
			LoadInGridDetail(LoadSo.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grdInquiryMapping.GetRows()
					select Conversion.ToInt(r.Cells["InquiryDetailId"].Value));
				foreach (DataRow dr in dt.Rows)
				{
					int SoDetailId = Conversion.ToInt(dr["DetailId"]);
					if (existingDetailIds.Add(SoDetailId))
					{
						DataRow newRow = dtSaleOrderMap.NewRow();
						newRow["InquiryId"] = dr["inquiryBookingMasterId"];
						newRow["InquiryDetailId"] = SoDetailId;
						newRow["InquiryNo"] = dr["inquiryBookingNo"];
						newRow["BuyerId"] = dr["buyerId"];
						newRow["BuyerName"] = dr["buyerName"];
						newRow["ItemId"] = dr["itemId"];
						newRow["ItemName"] = dr["ItemName"];
						newRow["ItemWeight"] = dr["itemWeight"];
						newRow["BookedWeight"] = dr["WeightUsedInOffer"];
						newRow["BalanceWeight"] = dr["BalanceWeight"];
						newRow["CurrentAllocationWeight"] = dr["BalanceWeight"];
						dtSaleOrderMap.Rows.Add(newRow);
					}
				}
			}
			grdInquiryMapping.DataSource = dtSaleOrderMap;
			grdInquiryMapping.RetrieveStructure();
			grdSaleOrderMappingSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddShiptoAddress_Click(object sender, EventArgs e)
	{
		try
		{
			using SupfrmShipToAddress obj = new SupfrmShipToAddress(UserAccount);
			obj.ShowDialog();
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
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
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
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Expected O, but got Unknown
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_0ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ced: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2f: Expected O, but got Unknown
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7c: Expected O, but got Unknown
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e37: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Expected O, but got Unknown
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1026: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Expected O, but got Unknown
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109f: Expected O, but got Unknown
		//IL_1145: Unknown result type (might be due to invalid IL or missing references)
		//IL_114f: Expected O, but got Unknown
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1170: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_119c: Expected O, but got Unknown
		//IL_11c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d3: Expected O, but got Unknown
		//IL_1200: Unknown result type (might be due to invalid IL or missing references)
		//IL_120a: Expected O, but got Unknown
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_1241: Expected O, but got Unknown
		//IL_1263: Unknown result type (might be due to invalid IL or missing references)
		//IL_126d: Expected O, but got Unknown
		//IL_12f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fc: Expected O, but got Unknown
		//IL_1308: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Expected O, but got Unknown
		//IL_134a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1354: Expected O, but got Unknown
		//IL_1360: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Expected O, but got Unknown
		//IL_13c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Expected O, but got Unknown
		//IL_13ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d8: Expected O, but got Unknown
		//IL_1bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbe: Expected O, but got Unknown
		//IL_1e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9c: Expected O, but got Unknown
		//IL_1ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb3: Expected O, but got Unknown
		//IL_224f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2259: Expected O, but got Unknown
		//IL_2c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c76: Expected O, but got Unknown
		//IL_2c83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8d: Expected O, but got Unknown
		//IL_2ff9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3003: Expected O, but got Unknown
		//IL_3010: Unknown result type (might be due to invalid IL or missing references)
		//IL_301a: Expected O, but got Unknown
		//IL_c743: Unknown result type (might be due to invalid IL or missing references)
		//IL_c74d: Expected O, but got Unknown
		//IL_c771: Unknown result type (might be due to invalid IL or missing references)
		//IL_c77b: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBagsPm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInquiryMapping_DesignTimeLayout = new GridEXLayout();
		Appearance appearance155 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
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
		Appearance appearance185 = new Appearance();
		Appearance appearance179 = new Appearance();
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
		Appearance appearance269 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
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
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
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
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance141 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Cmagt.frmSupplierOfferCmagt));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExpenseHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBagPmHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBagHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPaymentTermHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleOrderMappingHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
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
		Appearance appearance259 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance279 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabExpenses = new System.Windows.Forms.TabPage();
		this.grdInvExp = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabEmptyBags = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdEmptyBagsPm = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label61 = new System.Windows.Forms.Label();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel19 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.RadBagReturnAndWeightCutApply = new System.Windows.Forms.RadioButton();
		this.label62 = new System.Windows.Forms.Label();
		this.rdBagFocAndWeightCutNotApply = new System.Windows.Forms.RadioButton();
		this.rdBagPurchaseAndWeightCutApply = new System.Windows.Forms.RadioButton();
		this.rdCompanyOwnedAndWeightCutApply = new System.Windows.Forms.RadioButton();
		this.tabPaymentSchedule = new System.Windows.Forms.TabPage();
		this.grdPaymentTerm = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdInquiryMapping = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label60 = new System.Windows.Forms.Label();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.CmbPackingType = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.BtnAdd = new System.Windows.Forms.Button();
		this.BtnCancelDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDetail = new System.Windows.Forms.Button();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtTaxPercnt = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.cmbParentItem = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbTaxName = new UltraCombo();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.CmbCropYear = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.chkOtherExpenseAllowed = new System.Windows.Forms.CheckBox();
		this.txtPaymentScheduleRemarks = new System.Windows.Forms.TextBox();
		this.chkWithHoldingTaxApplied = new System.Windows.Forms.CheckBox();
		this.label53 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbPaymentTerm = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.CmbBrokeryRateUom = new UltraCombo();
		this.CmbBrokeryAc = new UltraCombo();
		this.txtBrokeryAmount = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.CmbBrokeryType = new UltraCombo();
		this.txtBrokeryRate = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtCommAmount = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtCommRate = new System.Windows.Forms.TextBox();
		this.cmbcommtype = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbCommUom = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbCommissionAc = new UltraCombo();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.BtnAddShiptoAddress = new System.Windows.Forms.Button();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbDeliveryToParty = new UltraCombo();
		this.txtShipToAddress = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbShipToAddress = new UltraCombo();
		this.panel26 = new System.Windows.Forms.Panel();
		this.RadBusinessName = new System.Windows.Forms.RadioButton();
		this.RadNickName = new System.Windows.Forms.RadioButton();
		this.radPartyCode = new System.Windows.Forms.RadioButton();
		this.datExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.datDocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtDeliveryDays = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.datDeliveryStartDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbCommissionAgent = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.CmbCompanyName = new UltraCombo();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnLoadSo = new System.Windows.Forms.ToolStripButton();
		this.btnAttachments = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdExpenseHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel24 = new System.Windows.Forms.Panel();
		this.grdEmptyBagPmHistory = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdEmptyBagHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdPaymentTermHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdSaleOrderMappingHistory = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label63 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label43 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datValidityToHistory = new System.Windows.Forms.DateTimePicker();
		this.datValidityFromHistory = new System.Windows.Forms.DateTimePicker();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbItemNameHistory = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbParentItemHistory = new UltraCombo();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label36 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbBuyerNameHistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbCommissionAgentHistory = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.CmbDeliverToAddressHistory = new UltraCombo();
		this.CmbDeliveryToPartyHistory = new UltraCombo();
		this.label39 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel14.SuspendLayout();
		this.tabExpenses.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel5.SuspendLayout();
		this.tabEmptyBags.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagsPm).BeginInit();
		this.panel21.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel19.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabPaymentSchedule.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel8.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInquiryMapping).BeginInit();
		this.panel16.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).BeginInit();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAc).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).BeginInit();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel9.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExpenseHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel24.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagPmHistory).BeginInit();
		this.panel25.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderMappingHistory).BeginInit();
		this.panel22.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1094, 811);
		this.tabControl1.TabIndex = 0;
		this.tabForm.Controls.Add(this.tabControl2);
		this.tabForm.Controls.Add(this.panel17);
		this.tabForm.Controls.Add(this.panel2);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Controls.Add(this.toolStrip1);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1086, 782);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.tabForm.UseVisualStyleBackColor = true;
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabExpenses);
		this.tabControl2.Controls.Add(this.tabEmptyBags);
		this.tabControl2.Controls.Add(this.tabPaymentSchedule);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(3, 426);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1080, 158);
		this.tabControl2.TabIndex = 5;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabDetail.Controls.Add(this.panel14);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1072, 129);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1066, 102);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 132;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1066, 21);
		this.panel14.TabIndex = 133;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1026, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar2.TabIndex = 127;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabExpenses.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.tabExpenses.Controls.Add(this.panel5);
		this.tabExpenses.Location = new System.Drawing.Point(4, 25);
		this.tabExpenses.Name = "tabExpenses";
		this.tabExpenses.Padding = new System.Windows.Forms.Padding(3);
		this.tabExpenses.Size = new System.Drawing.Size(1072, 129);
		this.tabExpenses.TabIndex = 1;
		this.tabExpenses.Text = "Supplier Other Charges";
		this.tabExpenses.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(1066, 102);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 135;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Tag = "GrdExpense";
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1066, 21);
		this.panel5.TabIndex = 134;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1026, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdInvExp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar3.TabIndex = 127;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabEmptyBags.Controls.Add(this.panel6);
		this.tabEmptyBags.Controls.Add(this.panel7);
		this.tabEmptyBags.Location = new System.Drawing.Point(4, 25);
		this.tabEmptyBags.Name = "tabEmptyBags";
		this.tabEmptyBags.Size = new System.Drawing.Size(1072, 129);
		this.tabEmptyBags.TabIndex = 2;
		this.tabEmptyBags.Text = "Empty Bags Weight Deduction Policy";
		this.tabEmptyBags.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel20);
		this.panel6.Controls.Add(this.panel18);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 20);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1072, 109);
		this.panel6.TabIndex = 136;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm);
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(520, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(552, 109);
		this.panel20.TabIndex = 137;
		((ScrollableBase)this.grdEmptyBagsPm).BackColor = System.Drawing.Color.White;
		grdEmptyBagsPm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBagsPm.DesignTimeLayout = grdEmptyBagsPm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBagsPm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Location = new System.Drawing.Point(0, 18);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Name = "grdEmptyBagsPm";
		((ScrollableBase)this.grdEmptyBagsPm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Size = new System.Drawing.Size(552, 91);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).TabIndex = 135;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).Tag = "GrdEmptyBags";
		this.grdEmptyBagsPm.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBagsPm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBagsPm).VisualStyle = (VisualStyle)4;
		this.grdEmptyBagsPm.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBagsPm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagsPm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdEmptyBagsPm_KeyDown);
		this.panel21.BackColor = System.Drawing.Color.White;
		this.panel21.Controls.Add(this.label61);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(552, 18);
		this.panel21.TabIndex = 59;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Black;
		this.label61.Location = new System.Drawing.Point(3, 2);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(153, 15);
		this.label61.TabIndex = 238;
		this.label61.Text = "Empty Bags Purchase Rate";
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(520, 109);
		this.panel18.TabIndex = 136;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 18);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(520, 91);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Tag = "GrdEmptyBags";
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.panel19.BackColor = System.Drawing.Color.White;
		this.panel19.Controls.Add(this.label27);
		this.panel19.Controls.Add(this.ctrlGrdBar4);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(520, 18);
		this.panel19.TabIndex = 58;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(3, 1);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(176, 15);
		this.label27.TabIndex = 238;
		this.label27.Text = "Empty Bags Weight Deduction";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(474, -1);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdEmptyBags;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 18);
		this.ctrlGrdBar4.TabIndex = 127;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.RadBagReturnAndWeightCutApply);
		this.panel7.Controls.Add(this.label62);
		this.panel7.Controls.Add(this.rdBagFocAndWeightCutNotApply);
		this.panel7.Controls.Add(this.rdBagPurchaseAndWeightCutApply);
		this.panel7.Controls.Add(this.rdCompanyOwnedAndWeightCutApply);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1072, 20);
		this.panel7.TabIndex = 134;
		this.RadBagReturnAndWeightCutApply.AutoSize = true;
		this.RadBagReturnAndWeightCutApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBagReturnAndWeightCutApply.ForeColor = System.Drawing.Color.White;
		this.RadBagReturnAndWeightCutApply.Location = new System.Drawing.Point(863, 2);
		this.RadBagReturnAndWeightCutApply.Margin = new System.Windows.Forms.Padding(0);
		this.RadBagReturnAndWeightCutApply.Name = "RadBagReturnAndWeightCutApply";
		this.RadBagReturnAndWeightCutApply.Size = new System.Drawing.Size(205, 19);
		this.RadBagReturnAndWeightCutApply.TabIndex = 3;
		this.RadBagReturnAndWeightCutApply.Text = "Bag Return And Weight Cut Apply";
		this.RadBagReturnAndWeightCutApply.UseVisualStyleBackColor = true;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Yellow;
		this.label62.Location = new System.Drawing.Point(5, 3);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(174, 15);
		this.label62.TabIndex = 239;
		this.label62.Text = "Term-------------------------->";
		this.rdBagFocAndWeightCutNotApply.AutoSize = true;
		this.rdBagFocAndWeightCutNotApply.Checked = true;
		this.rdBagFocAndWeightCutNotApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBagFocAndWeightCutNotApply.ForeColor = System.Drawing.Color.White;
		this.rdBagFocAndWeightCutNotApply.Location = new System.Drawing.Point(187, 2);
		this.rdBagFocAndWeightCutNotApply.Margin = new System.Windows.Forms.Padding(0);
		this.rdBagFocAndWeightCutNotApply.Name = "rdBagFocAndWeightCutNotApply";
		this.rdBagFocAndWeightCutNotApply.Size = new System.Drawing.Size(215, 19);
		this.rdBagFocAndWeightCutNotApply.TabIndex = 0;
		this.rdBagFocAndWeightCutNotApply.TabStop = true;
		this.rdBagFocAndWeightCutNotApply.Text = "Bag FOC And Weight Cut Not Apply";
		this.rdBagFocAndWeightCutNotApply.UseVisualStyleBackColor = true;
		this.rdBagPurchaseAndWeightCutApply.AutoSize = true;
		this.rdBagPurchaseAndWeightCutApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdBagPurchaseAndWeightCutApply.ForeColor = System.Drawing.Color.White;
		this.rdBagPurchaseAndWeightCutApply.Location = new System.Drawing.Point(645, 2);
		this.rdBagPurchaseAndWeightCutApply.Margin = new System.Windows.Forms.Padding(0);
		this.rdBagPurchaseAndWeightCutApply.Name = "rdBagPurchaseAndWeightCutApply";
		this.rdBagPurchaseAndWeightCutApply.Size = new System.Drawing.Size(218, 19);
		this.rdBagPurchaseAndWeightCutApply.TabIndex = 2;
		this.rdBagPurchaseAndWeightCutApply.Text = "Bag Purchase And Weight Cut Apply";
		this.rdBagPurchaseAndWeightCutApply.UseVisualStyleBackColor = true;
		this.rdCompanyOwnedAndWeightCutApply.AutoSize = true;
		this.rdCompanyOwnedAndWeightCutApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdCompanyOwnedAndWeightCutApply.ForeColor = System.Drawing.Color.White;
		this.rdCompanyOwnedAndWeightCutApply.Location = new System.Drawing.Point(405, 2);
		this.rdCompanyOwnedAndWeightCutApply.Margin = new System.Windows.Forms.Padding(0);
		this.rdCompanyOwnedAndWeightCutApply.Name = "rdCompanyOwnedAndWeightCutApply";
		this.rdCompanyOwnedAndWeightCutApply.Size = new System.Drawing.Size(239, 19);
		this.rdCompanyOwnedAndWeightCutApply.TabIndex = 1;
		this.rdCompanyOwnedAndWeightCutApply.Text = "Company Owned And Weight Cut Apply";
		this.rdCompanyOwnedAndWeightCutApply.UseVisualStyleBackColor = true;
		this.tabPaymentSchedule.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.tabPaymentSchedule.Controls.Add(this.panel8);
		this.tabPaymentSchedule.Location = new System.Drawing.Point(4, 25);
		this.tabPaymentSchedule.Name = "tabPaymentSchedule";
		this.tabPaymentSchedule.Size = new System.Drawing.Size(1072, 129);
		this.tabPaymentSchedule.TabIndex = 3;
		this.tabPaymentSchedule.Text = "Payment Schedule";
		this.tabPaymentSchedule.UseVisualStyleBackColor = true;
		this.grdPaymentTerm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentTerm.FilterMode = (FilterMode)2;
		this.grdPaymentTerm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(0, 21);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1072, 108);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 52;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.CellUpdated += new ColumnActionEventHandler(grdPaymentTerm_CellUpdated);
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label38);
		this.panel8.Controls.Add(this.ctrlGrdBar5);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1072, 21);
		this.panel8.TabIndex = 135;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.ForeColor = System.Drawing.Color.Yellow;
		this.label38.Location = new System.Drawing.Point(2, 3);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(219, 15);
		this.label38.TabIndex = 239;
		this.label38.Text = "Add Single or Multi Payment Schedule";
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1026, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar5.MyGrid = this.grdInvExp;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar5.TabIndex = 127;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdInquiryMapping);
		this.panel17.Controls.Add(this.panel16);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel17.Location = new System.Drawing.Point(3, 584);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1080, 195);
		this.panel17.TabIndex = 6;
		this.grdInquiryMapping.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdInquiryMapping).BackColor = System.Drawing.Color.White;
		this.grdInquiryMapping.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdInquiryMapping_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInquiryMapping.DesignTimeLayout = grdInquiryMapping_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInquiryMapping.FilterMode = (FilterMode)2;
		this.grdInquiryMapping.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInquiryMapping.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).Name = "grdInquiryMapping";
		((ScrollableBase)this.grdInquiryMapping).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).Size = new System.Drawing.Size(1080, 172);
		((System.Windows.Forms.Control)(object)this.grdInquiryMapping).TabIndex = 136;
		this.grdInquiryMapping.TotalRow = (InheritableBoolean)1;
		this.grdInquiryMapping.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInquiryMapping).VisualStyle = (VisualStyle)4;
		this.grdInquiryMapping.CellUpdated += new ColumnActionEventHandler(grdSaleOrderMapping_CellUpdated);
		this.grdInquiryMapping.ColumnButtonClick += new ColumnActionEventHandler(grdSaleOrderMapping_ColumnButtonClick);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label60);
		this.panel16.Controls.Add(this.ctrlGrdBar6);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1080, 23);
		this.panel16.TabIndex = 137;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Yellow;
		this.label60.Location = new System.Drawing.Point(4, 4);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(97, 15);
		this.label60.TabIndex = 239;
		this.label60.Text = "Inquiry Mapping";
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1040, 1);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar6.MyGrid = this.grdInquiryMapping;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(38, 20);
		this.ctrlGrdBar6.TabIndex = 127;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.panel2.Controls.Add(this.groupBox6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 316);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1080, 110);
		this.panel2.TabIndex = 4;
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox6.Controls.Add(this.label16);
		this.groupBox6.Controls.Add(this.label21);
		this.groupBox6.Controls.Add(this.txtRemarksDetail);
		this.groupBox6.Controls.Add(this.label51);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox6.Controls.Add(this.txtQty);
		this.groupBox6.Controls.Add(this.label19);
		this.groupBox6.Controls.Add(this.BtnAdd);
		this.groupBox6.Controls.Add(this.BtnCancelDetail);
		this.groupBox6.Controls.Add(this.BtnUpdateDetail);
		this.groupBox6.Controls.Add(this.txtTotalAmount);
		this.groupBox6.Controls.Add(this.label14);
		this.groupBox6.Controls.Add(this.txtTaxPercnt);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox6.Controls.Add(this.label22);
		this.groupBox6.Controls.Add(this.label45);
		this.groupBox6.Controls.Add(this.label24);
		this.groupBox6.Controls.Add(this.label28);
		this.groupBox6.Controls.Add(this.txtTaxAmount);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbParentItem);
		this.groupBox6.Controls.Add(this.label25);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox6.Controls.Add(this.label40);
		this.groupBox6.Controls.Add(this.label15);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxName);
		this.groupBox6.Controls.Add(this.txtRate);
		this.groupBox6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox6.Controls.Add(this.label20);
		this.groupBox6.Controls.Add(this.txtAmount);
		this.groupBox6.Controls.Add(this.txtWeight);
		this.groupBox6.Controls.Add(this.label50);
		this.groupBox6.Controls.Add(this.label18);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold);
		this.groupBox6.Location = new System.Drawing.Point(2, 3);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1071, 106);
		this.groupBox6.TabIndex = 50;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Detail";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(511, 31);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(723, 13);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(26, 15);
		this.label16.TabIndex = 31;
		this.label16.Text = "Qty";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(637, 13);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(61, 15);
		this.label21.TabIndex = 40;
		this.label21.Text = "Pack Uom";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(449, 78);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(502, 23);
		this.txtRemarksDetail.TabIndex = 19;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(449, 60);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(52, 15);
		this.label51.TabIndex = 652;
		this.label51.Text = "Remarks";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(637, 31);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.txtQty.BackColor = System.Drawing.Color.White;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(723, 33);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(70, 23);
		this.txtQty.TabIndex = 10;
		this.txtQty.Tag = "double";
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(511, 13);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(77, 15);
		this.label19.TabIndex = 36;
		this.label19.Text = "Packing Type";
		this.BtnAdd.BackColor = System.Drawing.Color.Teal;
		this.BtnAdd.FlatAppearance.BorderSize = 0;
		this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAdd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAdd.ForeColor = System.Drawing.Color.White;
		this.BtnAdd.Location = new System.Drawing.Point(954, 78);
		this.BtnAdd.Name = "BtnAdd";
		this.BtnAdd.Size = new System.Drawing.Size(25, 23);
		this.BtnAdd.TabIndex = 20;
		this.BtnAdd.Text = "+";
		this.BtnAdd.UseVisualStyleBackColor = false;
		this.BtnAdd.Click += new System.EventHandler(BtnAdd_Click);
		this.BtnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDetail.FlatAppearance.BorderSize = 0;
		this.BtnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDetail.Location = new System.Drawing.Point(1016, 78);
		this.BtnCancelDetail.Name = "BtnCancelDetail";
		this.BtnCancelDetail.Size = new System.Drawing.Size(50, 22);
		this.BtnCancelDetail.TabIndex = 22;
		this.BtnCancelDetail.Text = "Clear";
		this.BtnCancelDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDetail.Visible = false;
		this.BtnCancelDetail.Click += new System.EventHandler(BtnCancelDetail_Click);
		this.BtnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.BtnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDetail.Location = new System.Drawing.Point(953, 78);
		this.BtnUpdateDetail.Name = "BtnUpdateDetail";
		this.BtnUpdateDetail.Size = new System.Drawing.Size(62, 23);
		this.BtnUpdateDetail.TabIndex = 21;
		this.BtnUpdateDetail.Text = "Update";
		this.BtnUpdateDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDetail.Visible = false;
		this.BtnUpdateDetail.Click += new System.EventHandler(BtnUpdateDetail_Click);
		this.txtTotalAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(354, 78);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(94, 23);
		this.txtTotalAmount.TabIndex = 18;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(9, 13);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 30;
		this.label14.Text = "Parent Item";
		this.txtTaxPercnt.Enabled = false;
		this.txtTaxPercnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercnt.Location = new System.Drawing.Point(202, 78);
		this.txtTaxPercnt.Name = "txtTaxPercnt";
		this.txtTaxPercnt.ShortcutsEnabled = false;
		this.txtTaxPercnt.Size = new System.Drawing.Size(60, 23);
		this.txtTaxPercnt.TabIndex = 16;
		this.txtTaxPercnt.Tag = "double";
		this.txtTaxPercnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance226).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance226).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance227;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(968, 31);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(202, 60);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(35, 15);
		this.label22.TabIndex = 654;
		this.label22.Text = "Tax%";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(968, 13);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(59, 15);
		this.label45.TabIndex = 49;
		this.label45.Text = "Rate Uom";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(354, 60);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(83, 15);
		this.label24.TabIndex = 656;
		this.label24.Text = "Tax + Amount";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(885, 13);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(30, 15);
		this.label28.TabIndex = 49;
		this.label28.Text = "Rate";
		this.txtTaxAmount.BackColor = System.Drawing.SystemColors.Menu;
		this.txtTaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(263, 78);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(90, 23);
		this.txtTaxAmount.TabIndex = 17;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbParentItem.AllowNull = (DefaultableBoolean)1;
		this.cmbParentItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbParentItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbParentItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbParentItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbParentItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbParentItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbParentItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbParentItem).Location = new System.Drawing.Point(9, 31);
		((System.Windows.Forms.Control)(object)this.cmbParentItem).Name = "cmbParentItem";
		((System.Windows.Forms.Control)(object)this.cmbParentItem).Size = new System.Drawing.Size(169, 26);
		((System.Windows.Forms.Control)(object)this.cmbParentItem).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbParentItem).Leave += new System.EventHandler(cmbParentItem_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(263, 60);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(72, 15);
		this.label25.TabIndex = 655;
		this.label25.Text = "Tax Amount";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(179, 31);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(213, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(101, 60);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(60, 15);
		this.label40.TabIndex = 653;
		this.label40.Text = "Tax Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(179, 13);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(67, 15);
		this.label15.TabIndex = 32;
		this.label15.Text = "Item Name";
		this.CmbTaxName.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance246).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance252).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Location = new System.Drawing.Point(101, 76);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Name = "CmbTaxName";
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxName).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbTaxName).Leave += new System.EventHandler(CmbTaxName_Leave);
		this.txtRate.BackColor = System.Drawing.Color.White;
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(885, 33);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(82, 23);
		this.txtRate.TabIndex = 12;
		this.txtRate.Tag = "double";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(393, 31);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 7;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(393, 13);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(57, 15);
		this.label20.TabIndex = 38;
		this.label20.Text = "Crop Year";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(10, 78);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(90, 23);
		this.txtAmount.TabIndex = 14;
		this.txtAmount.Tag = "double";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAmount.TextChanged += new System.EventHandler(txtAmount_TextChanged);
		this.txtWeight.BackColor = System.Drawing.Color.White;
		this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeight.Location = new System.Drawing.Point(794, 33);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.Size = new System.Drawing.Size(90, 23);
		this.txtWeight.TabIndex = 11;
		this.txtWeight.Tag = "double";
		this.txtWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeight.TextChanged += new System.EventHandler(txtWeight_TextChanged);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(10, 60);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(51, 15);
		this.label50.TabIndex = 35;
		this.label50.Text = "Amount";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(794, 13);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 15);
		this.label18.TabIndex = 35;
		this.label18.Text = "Weight";
		this.panel1.Controls.Add(this.groupBox5);
		this.panel1.Controls.Add(this.groupBox4);
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.txtRemarks);
		this.panel1.Controls.Add(this.label12);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 61);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1080, 255);
		this.panel1.TabIndex = 0;
		this.groupBox5.Controls.Add(this.txtDueDays);
		this.groupBox5.Controls.Add(this.chkOtherExpenseAllowed);
		this.groupBox5.Controls.Add(this.txtPaymentScheduleRemarks);
		this.groupBox5.Controls.Add(this.chkWithHoldingTaxApplied);
		this.groupBox5.Controls.Add(this.label53);
		this.groupBox5.Controls.Add(this.label5);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTerm);
		this.groupBox5.Controls.Add(this.label4);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(690, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(383, 200);
		this.groupBox5.TabIndex = 3;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Add Single Payment Term";
		this.txtDueDays.BackColor = System.Drawing.Color.White;
		this.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(320, 15);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(60, 23);
		this.txtDueDays.TabIndex = 1;
		this.txtDueDays.Tag = "int";
		this.chkOtherExpenseAllowed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkOtherExpenseAllowed.AutoSize = true;
		this.chkOtherExpenseAllowed.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkOtherExpenseAllowed.ForeColor = System.Drawing.Color.Teal;
		this.chkOtherExpenseAllowed.Location = new System.Drawing.Point(199, 175);
		this.chkOtherExpenseAllowed.Name = "chkOtherExpenseAllowed";
		this.chkOtherExpenseAllowed.Size = new System.Drawing.Size(176, 17);
		this.chkOtherExpenseAllowed.TabIndex = 4;
		this.chkOtherExpenseAllowed.Text = "Other Expense Allowed";
		this.chkOtherExpenseAllowed.UseVisualStyleBackColor = true;
		this.txtPaymentScheduleRemarks.BackColor = System.Drawing.Color.White;
		this.txtPaymentScheduleRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPaymentScheduleRemarks.Enabled = false;
		this.txtPaymentScheduleRemarks.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtPaymentScheduleRemarks.Location = new System.Drawing.Point(8, 64);
		this.txtPaymentScheduleRemarks.Multiline = true;
		this.txtPaymentScheduleRemarks.Name = "txtPaymentScheduleRemarks";
		this.txtPaymentScheduleRemarks.Size = new System.Drawing.Size(370, 106);
		this.txtPaymentScheduleRemarks.TabIndex = 2;
		this.chkWithHoldingTaxApplied.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkWithHoldingTaxApplied.AutoSize = true;
		this.chkWithHoldingTaxApplied.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.chkWithHoldingTaxApplied.ForeColor = System.Drawing.Color.Teal;
		this.chkWithHoldingTaxApplied.Location = new System.Drawing.Point(9, 176);
		this.chkWithHoldingTaxApplied.Name = "chkWithHoldingTaxApplied";
		this.chkWithHoldingTaxApplied.Size = new System.Drawing.Size(189, 17);
		this.chkWithHoldingTaxApplied.TabIndex = 3;
		this.chkWithHoldingTaxApplied.Text = "With Holding Tax Applied";
		this.chkWithHoldingTaxApplied.UseVisualStyleBackColor = true;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(6, 43);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(170, 15);
		this.label53.TabIndex = 30;
		this.label53.Text = "Payment Schedule Description";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(263, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 15);
		this.label5.TabIndex = 19;
		this.label5.Text = "Due Days";
		this.CmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Location = new System.Drawing.Point(92, 13);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Name = "CmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).TabIndex = 0;
		this.CmbPaymentTerm.ValueChanged += new System.EventHandler(CmbPaymentTerm_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTerm).Leave += new System.EventHandler(CmbPaymentTerm_ValueChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 16;
		this.label4.Text = "Payment Term";
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryAc);
		this.groupBox4.Controls.Add(this.txtBrokeryAmount);
		this.groupBox4.Controls.Add(this.label47);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrokeryType);
		this.groupBox4.Controls.Add(this.txtBrokeryRate);
		this.groupBox4.Controls.Add(this.label44);
		this.groupBox4.Controls.Add(this.label13);
		this.groupBox4.Controls.Add(this.label1);
		this.groupBox4.Controls.Add(this.label2);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(390, 101);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(297, 99);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Sub_Brokery Agent";
		this.CmbBrokeryRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance271).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance271).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance271;
		((AppearanceBase)appearance272).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance272;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance277).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance228).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Location = new System.Drawing.Point(62, 69);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Name = "CmbBrokeryRateUom";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryRateUom).TextChanged += new System.EventHandler(CmbBrokeryRateUom_TextChanged);
		this.CmbBrokeryAc.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance323).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance323).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance323).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance323;
		((AppearanceBase)appearance324).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance324;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Location = new System.Drawing.Point(62, 13);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Name = "CmbBrokeryAc";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryAc).TabIndex = 0;
		this.txtBrokeryAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtBrokeryAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryAmount.Location = new System.Drawing.Point(202, 71);
		this.txtBrokeryAmount.Name = "txtBrokeryAmount";
		this.txtBrokeryAmount.ReadOnly = true;
		this.txtBrokeryAmount.Size = new System.Drawing.Size(90, 23);
		this.txtBrokeryAmount.TabIndex = 4;
		this.txtBrokeryAmount.TabStop = false;
		this.txtBrokeryAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(4, 75);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(59, 15);
		this.label47.TabIndex = 29;
		this.label47.Text = "Rate Uom";
		this.CmbBrokeryType.AllowNull = (DefaultableBoolean)1;
		this.CmbBrokeryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrokeryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrokeryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance311;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance315).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance315;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance316).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance316;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance317).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance317;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance318).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance318).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance318).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance318).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance318).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance318;
		((AppearanceBase)appearance319).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance320).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance321).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrokeryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBrokeryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Location = new System.Drawing.Point(62, 41);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Name = "CmbBrokeryType";
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbBrokeryType).TextChanged += new System.EventHandler(CmbBrokeryType_TextChanged);
		this.txtBrokeryRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBrokeryRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBrokeryRate.Location = new System.Drawing.Point(202, 43);
		this.txtBrokeryRate.Name = "txtBrokeryRate";
		this.txtBrokeryRate.Size = new System.Drawing.Size(90, 23);
		this.txtBrokeryRate.TabIndex = 2;
		this.txtBrokeryRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBrokeryRate.TextChanged += new System.EventHandler(txtBrokeryRate_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(151, 47);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(30, 15);
		this.label44.TabIndex = 38;
		this.label44.Text = "Rate";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 47);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(32, 15);
		this.label13.TabIndex = 28;
		this.label13.Text = "Type";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 32;
		this.label1.Text = "Account";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(151, 75);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 36;
		this.label2.Text = "Amount";
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.txtCommAmount);
		this.groupBox3.Controls.Add(this.label48);
		this.groupBox3.Controls.Add(this.txtCommRate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommUom);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAc);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(390, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(297, 99);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Sub_Commission Agent";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 33;
		this.label9.Text = "Account";
		this.txtCommAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtCommAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommAmount.Location = new System.Drawing.Point(204, 71);
		this.txtCommAmount.Name = "txtCommAmount";
		this.txtCommAmount.Size = new System.Drawing.Size(90, 23);
		this.txtCommAmount.TabIndex = 4;
		this.txtCommAmount.TabStop = false;
		this.txtCommAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(5, 75);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(59, 15);
		this.label48.TabIndex = 30;
		this.label48.Text = "Rate Uom";
		this.txtCommRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommRate.Location = new System.Drawing.Point(204, 43);
		this.txtCommRate.Name = "txtCommRate";
		this.txtCommRate.Size = new System.Drawing.Size(90, 23);
		this.txtCommRate.TabIndex = 2;
		this.txtCommRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommRate.TextChanged += new System.EventHandler(txtCommRate_TextChanged);
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance230;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance296).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(64, 41);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TextChanged += new System.EventHandler(cmbcommtype_TextChanged);
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(153, 47);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(30, 15);
		this.label46.TabIndex = 35;
		this.label46.Text = "Rate";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(5, 47);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(32, 15);
		this.label42.TabIndex = 31;
		this.label42.Text = "Type";
		this.CmbCommUom.AllowNull = (DefaultableBoolean)1;
		this.CmbCommUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance302;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance304).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance304).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance305).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance305;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance306).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance307;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Location = new System.Drawing.Point(64, 69);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Name = "CmbCommUom";
		((System.Windows.Forms.Control)(object)this.CmbCommUom).Size = new System.Drawing.Size(85, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommUom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCommUom).TextChanged += new System.EventHandler(CmbCommUom_TextChanged);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(153, 75);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(51, 15);
		this.label41.TabIndex = 37;
		this.label41.Text = "Amount";
		this.CmbCommissionAc.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCommissionAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAc).Location = new System.Drawing.Point(64, 13);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAc).Name = "CmbCommissionAc";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAc).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAc).TabIndex = 0;
		this.groupBox2.Controls.Add(this.BtnAddShiptoAddress);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty);
		this.groupBox2.Controls.Add(this.txtShipToAddress);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbShipToAddress);
		this.groupBox2.Controls.Add(this.panel26);
		this.groupBox2.Controls.Add(this.datExpiryDate);
		this.groupBox2.Controls.Add(this.datDocDate);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtDeliveryDays);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.datDeliveryStartDate);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.groupBox2.Controls.Add(this.txtDocNo);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(387, 255);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.BtnAddShiptoAddress.BackColor = System.Drawing.Color.White;
		this.BtnAddShiptoAddress.FlatAppearance.BorderSize = 0;
		this.BtnAddShiptoAddress.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddShiptoAddress.ForeColor = System.Drawing.Color.White;
		this.BtnAddShiptoAddress.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnAddShiptoAddress.Location = new System.Drawing.Point(359, 146);
		this.BtnAddShiptoAddress.Name = "BtnAddShiptoAddress";
		this.BtnAddShiptoAddress.Size = new System.Drawing.Size(25, 26);
		this.BtnAddShiptoAddress.TabIndex = 6;
		this.BtnAddShiptoAddress.Text = "+";
		this.BtnAddShiptoAddress.UseVisualStyleBackColor = false;
		this.BtnAddShiptoAddress.Click += new System.EventHandler(BtnAddShiptoAddress_Click);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(4, 157);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 30);
		this.label35.TabIndex = 131;
		this.label35.Text = "Delivery / Ship \r\nTo Address";
		this.CmbDeliveryToParty.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance240).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance100).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Location = new System.Drawing.Point(92, 117);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Name = "CmbDeliveryToParty";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Size = new System.Drawing.Size(292, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToParty).Leave += new System.EventHandler(CmbDeliveryToParty_Leave);
		this.txtShipToAddress.BackColor = System.Drawing.Color.White;
		this.txtShipToAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShipToAddress.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShipToAddress.Location = new System.Drawing.Point(92, 175);
		this.txtShipToAddress.Name = "txtShipToAddress";
		this.txtShipToAddress.Size = new System.Drawing.Size(292, 23);
		this.txtShipToAddress.TabIndex = 7;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(4, 123);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(82, 30);
		this.label34.TabIndex = 130;
		this.label34.Text = "Deliver / Ship \r\nTo Party";
		this.CmbShipToAddress.AllowNull = (DefaultableBoolean)1;
		this.CmbShipToAddress.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbShipToAddress.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbShipToAddress.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance165).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbShipToAddress).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbShipToAddress.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Location = new System.Drawing.Point(92, 146);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Name = "CmbShipToAddress";
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbShipToAddress).Leave += new System.EventHandler(CmbShipToAddress_Leave);
		this.panel26.Controls.Add(this.RadBusinessName);
		this.panel26.Controls.Add(this.RadNickName);
		this.panel26.Controls.Add(this.radPartyCode);
		this.panel26.Location = new System.Drawing.Point(92, 37);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(290, 22);
		this.panel26.TabIndex = 125;
		this.RadBusinessName.AutoSize = true;
		this.RadBusinessName.Checked = true;
		this.RadBusinessName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBusinessName.Location = new System.Drawing.Point(9, 3);
		this.RadBusinessName.Name = "RadBusinessName";
		this.RadBusinessName.Size = new System.Drawing.Size(123, 17);
		this.RadBusinessName.TabIndex = 0;
		this.RadBusinessName.TabStop = true;
		this.RadBusinessName.Text = "Business Name";
		this.RadBusinessName.UseVisualStyleBackColor = true;
		this.RadBusinessName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.RadNickName.AutoSize = true;
		this.RadNickName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNickName.Location = new System.Drawing.Point(138, 3);
		this.RadNickName.Name = "RadNickName";
		this.RadNickName.Size = new System.Drawing.Size(94, 17);
		this.RadNickName.TabIndex = 65;
		this.RadNickName.Text = "Nick Name";
		this.RadNickName.UseVisualStyleBackColor = true;
		this.RadNickName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.radPartyCode.AutoSize = true;
		this.radPartyCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPartyCode.Location = new System.Drawing.Point(233, 4);
		this.radPartyCode.Name = "radPartyCode";
		this.radPartyCode.Size = new System.Drawing.Size(57, 17);
		this.radPartyCode.TabIndex = 65;
		this.radPartyCode.Text = "Code";
		this.radPartyCode.UseVisualStyleBackColor = true;
		this.radPartyCode.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.datExpiryDate.CustomFormat = "dd-MMM-yy";
		this.datExpiryDate.Enabled = false;
		this.datExpiryDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datExpiryDate.Location = new System.Drawing.Point(259, 229);
		this.datExpiryDate.Name = "datExpiryDate";
		this.datExpiryDate.Size = new System.Drawing.Size(125, 22);
		this.datExpiryDate.TabIndex = 11;
		this.datExpiryDate.TabStop = false;
		this.datDocDate.CustomFormat = "dd-MMM-yy";
		this.datDocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDocDate.Location = new System.Drawing.Point(259, 13);
		this.datDocDate.Name = "datDocDate";
		this.datDocDate.Size = new System.Drawing.Size(125, 22);
		this.datDocDate.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 7;
		this.label17.Text = "Doc No,Date";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(197, 17);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(57, 15);
		this.label49.TabIndex = 7;
		this.label49.Text = "Doc Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 59);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 30);
		this.label7.TabIndex = 12;
		this.label7.Text = "Commission\r\nAgent / Broker";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 95);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(86, 15);
		this.label3.TabIndex = 14;
		this.label3.Text = "Supplier Name";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(92, 201);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 8;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 207);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 20;
		this.label8.Text = "Delivery Term";
		this.txtDeliveryDays.BackColor = System.Drawing.Color.White;
		this.txtDeliveryDays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryDays.Location = new System.Drawing.Point(92, 229);
		this.txtDeliveryDays.Name = "txtDeliveryDays";
		this.txtDeliveryDays.Size = new System.Drawing.Size(100, 23);
		this.txtDeliveryDays.TabIndex = 9;
		this.txtDeliveryDays.Tag = "int";
		this.txtDeliveryDays.TextChanged += new System.EventHandler(txtDeliveryDays_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(4, 233);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 23;
		this.label6.Text = "Delivery Days";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(194, 199);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(60, 30);
		this.label10.TabIndex = 25;
		this.label10.Text = "Delivery \r\nStart Date";
		this.datDeliveryStartDate.CustomFormat = "dd-MMM-yy";
		this.datDeliveryStartDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.datDeliveryStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDeliveryStartDate.Location = new System.Drawing.Point(259, 203);
		this.datDeliveryStartDate.Name = "datDeliveryStartDate";
		this.datDeliveryStartDate.Size = new System.Drawing.Size(125, 22);
		this.datDeliveryStartDate.TabIndex = 10;
		this.datDeliveryStartDate.ValueChanged += new System.EventHandler(datDeliveryStartDate_ValueChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(194, 233);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 15);
		this.label11.TabIndex = 27;
		this.label11.Text = "Expiry Date";
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance247;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance136).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(92, 61);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(292, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 2;
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(92, 89);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(292, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 3;
		this.txtDocNo.BackColor = System.Drawing.Color.White;
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(92, 13);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.Size = new System.Drawing.Size(100, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.TabStop = false;
		this.txtDocNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRemarks.BackColor = System.Drawing.Color.White;
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(452, 203);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(616, 49);
		this.txtRemarks.TabIndex = 2;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(388, 204);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Remarks";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label54);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompanyName);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1080, 31);
		this.panel3.TabIndex = 2;
		this.label54.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(722, 8);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(46, 15);
		this.label54.TabIndex = 32;
		this.label54.Text = "Branch";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(775, 2);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 33;
		this.label26.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(436, 8);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(58, 15);
		this.label26.TabIndex = 30;
		this.label26.Text = "Company";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(122, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Supplier Offer";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(997, 6);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(81, 18);
		this.chkPrint.TabIndex = 2;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.CmbCompanyName.AllowNull = (DefaultableBoolean)1;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.CmbCompanyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompanyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompanyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompanyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCompanyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Location = new System.Drawing.Point(498, 2);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Name = "CmbCompanyName";
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompanyName).TabIndex = 31;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnnew, this.BtnRefresh, this.toolStripSeparator1, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.btnLoadSo, this.btnAttachments, this.btnPrint,
			this.toolStripSeparator2, this.toolStripSeparator4, this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1080, 27);
		this.toolStrip1.TabIndex = 1;
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
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = (System.Drawing.Image)resources.GetObject("btnDelete.Image");
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnLoadSo.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadSo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadSo.Name = "btnLoadSo";
		this.btnLoadSo.Size = new System.Drawing.Size(112, 24);
		this.btnLoadSo.Text = "Load Inquiry";
		this.btnLoadSo.Click += new System.EventHandler(btnLoadSo_Click);
		this.btnAttachments.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachments.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachments.Name = "btnAttachments";
		this.btnAttachments.Size = new System.Drawing.Size(116, 24);
		this.btnAttachments.Text = "Attachments";
		this.btnAttachments.Click += new System.EventHandler(btnAttachments_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1051-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.tabControl3);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1086, 782);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 182);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1080, 345);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPage1);
		this.tabControl3.Controls.Add(this.tabPage2);
		this.tabControl3.Controls.Add(this.tabPage3);
		this.tabControl3.Controls.Add(this.tabPage4);
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(3, 527);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(1080, 252);
		this.tabControl3.TabIndex = 58;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1072, 223);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1066, 193);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 0;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label52);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1066, 24);
		this.panel9.TabIndex = 57;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(4, 5);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(225, 16);
		this.label52.TabIndex = 129;
		this.label52.Text = "Detail of the Above Selected Row";
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdExpenseHistory);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1072, 223);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Supplier Other Charges";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdExpenseHistory.AllowEdit = (InheritableBoolean)2;
		grdExpenseHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExpenseHistory.DesignTimeLayout = grdExpenseHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExpenseHistory.DynamicFiltering = true;
		this.grdExpenseHistory.FilterMode = (FilterMode)2;
		this.grdExpenseHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExpenseHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Name = "grdExpenseHistory";
		((ScrollableBase)this.grdExpenseHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).Size = new System.Drawing.Size(1066, 193);
		((System.Windows.Forms.Control)(object)this.grdExpenseHistory).TabIndex = 59;
		this.grdExpenseHistory.TotalRow = (InheritableBoolean)1;
		this.grdExpenseHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExpenseHistory).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label57);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1066, 24);
		this.panel10.TabIndex = 58;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(4, 5);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(0, 16);
		this.label57.TabIndex = 129;
		this.tabPage3.Controls.Add(this.panel24);
		this.tabPage3.Controls.Add(this.panel23);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1072, 223);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Empty Bags Weight Deduction Policy";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel24.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory);
		this.panel24.Controls.Add(this.panel25);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel24.Location = new System.Drawing.Point(471, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(601, 223);
		this.panel24.TabIndex = 63;
		this.grdEmptyBagPmHistory.AllowEdit = (InheritableBoolean)2;
		grdEmptyBagPmHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBagPmHistory.DesignTimeLayout = grdEmptyBagPmHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmptyBagPmHistory.DynamicFiltering = true;
		this.grdEmptyBagPmHistory.FilterMode = (FilterMode)2;
		this.grdEmptyBagPmHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBagPmHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).Name = "grdEmptyBagPmHistory";
		((ScrollableBase)this.grdEmptyBagPmHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).Size = new System.Drawing.Size(601, 199);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagPmHistory).TabIndex = 62;
		this.grdEmptyBagPmHistory.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBagPmHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBagPmHistory).VisualStyle = (VisualStyle)4;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.label65);
		this.panel25.Controls.Add(this.label66);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(601, 24);
		this.panel25.TabIndex = 61;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Yellow;
		this.label65.Location = new System.Drawing.Point(5, 4);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(153, 15);
		this.label65.TabIndex = 239;
		this.label65.Text = "Empty Bags Purchase Rate";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(4, 5);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(0, 16);
		this.label66.TabIndex = 129;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory);
		this.panel23.Controls.Add(this.panel11);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(471, 223);
		this.panel23.TabIndex = 62;
		this.grdEmptyBagHistory.AllowEdit = (InheritableBoolean)2;
		grdEmptyBagHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBagHistory.DesignTimeLayout = grdEmptyBagHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdEmptyBagHistory.DynamicFiltering = true;
		this.grdEmptyBagHistory.FilterMode = (FilterMode)2;
		this.grdEmptyBagHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBagHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Name = "grdEmptyBagHistory";
		((ScrollableBase)this.grdEmptyBagHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).Size = new System.Drawing.Size(471, 199);
		((System.Windows.Forms.Control)(object)this.grdEmptyBagHistory).TabIndex = 61;
		this.grdEmptyBagHistory.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBagHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBagHistory).VisualStyle = (VisualStyle)4;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label64);
		this.panel11.Controls.Add(this.label58);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(471, 24);
		this.panel11.TabIndex = 60;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Yellow;
		this.label64.Location = new System.Drawing.Point(5, 4);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(176, 15);
		this.label64.TabIndex = 239;
		this.label64.Text = "Empty Bags Weight Deduction";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 5);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(0, 16);
		this.label58.TabIndex = 129;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory);
		this.tabPage4.Controls.Add(this.panel12);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1072, 223);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Payment Schedule";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdPaymentTermHistory.AllowEdit = (InheritableBoolean)2;
		grdPaymentTermHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermHistory.DesignTimeLayout = grdPaymentTermHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentTermHistory.DynamicFiltering = true;
		this.grdPaymentTermHistory.FilterMode = (FilterMode)2;
		this.grdPaymentTermHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).Name = "grdPaymentTermHistory";
		((ScrollableBase)this.grdPaymentTermHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).Size = new System.Drawing.Size(1072, 199);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermHistory).TabIndex = 60;
		this.grdPaymentTermHistory.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermHistory).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label59);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1072, 24);
		this.panel12.TabIndex = 59;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(4, 5);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(0, 16);
		this.label59.TabIndex = 129;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory);
		this.tabPage5.Controls.Add(this.panel22);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1072, 223);
		this.tabPage5.TabIndex = 4;
		this.tabPage5.Text = "Inquiry Mapping";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdSaleOrderMappingHistory.AllowEdit = (InheritableBoolean)2;
		grdSaleOrderMappingHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSaleOrderMappingHistory.DesignTimeLayout = grdSaleOrderMappingHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSaleOrderMappingHistory.DynamicFiltering = true;
		this.grdSaleOrderMappingHistory.FilterMode = (FilterMode)2;
		this.grdSaleOrderMappingHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleOrderMappingHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).Name = "grdSaleOrderMappingHistory";
		((ScrollableBase)this.grdSaleOrderMappingHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).Size = new System.Drawing.Size(1072, 199);
		((System.Windows.Forms.Control)(object)this.grdSaleOrderMappingHistory).TabIndex = 61;
		this.grdSaleOrderMappingHistory.TotalRow = (InheritableBoolean)1;
		this.grdSaleOrderMappingHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleOrderMappingHistory).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.label63);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1072, 24);
		this.panel22.TabIndex = 60;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.White;
		this.label63.Location = new System.Drawing.Point(4, 5);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(0, 16);
		this.label63.TabIndex = 129;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label43);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 159);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1080, 23);
		this.panel15.TabIndex = 54;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1039, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(7, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(112, 16);
		this.label43.TabIndex = 127;
		this.label43.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 59);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1080, 100);
		this.panel13.TabIndex = 6;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory);
		this.groupBox1.Controls.Add(this.datValidityToHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.datValidityFromHistory);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentItemHistory);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1070, 95);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.datValidityToHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityToHistory.Checked = false;
		this.datValidityToHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityToHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityToHistory.Location = new System.Drawing.Point(228, 40);
		this.datValidityToHistory.Name = "datValidityToHistory";
		this.datValidityToHistory.ShowCheckBox = true;
		this.datValidityToHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityToHistory.TabIndex = 3;
		this.datValidityFromHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityFromHistory.Checked = false;
		this.datValidityFromHistory.CustomFormat = "dd-MMM-yy";
		this.datValidityFromHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.datValidityFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityFromHistory.Location = new System.Drawing.Point(97, 40);
		this.datValidityFromHistory.Name = "datValidityFromHistory";
		this.datValidityFromHistory.ShowCheckBox = true;
		this.datValidityFromHistory.Size = new System.Drawing.Size(130, 22);
		this.datValidityFromHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(228, 13);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(130, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(97, 13);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 22);
		this.FromDateHistory.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(925, 67);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(50, 25);
		this.btnshow.TabIndex = 10;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance289;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance290;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance291;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance293).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance295).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance297).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(434, 11);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 5;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(361, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 189;
		this.label37.Text = "Item Name";
		this.CmbParentItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance270).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance270).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance281).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance281;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance292).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance292).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance292).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance260).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Location = new System.Drawing.Point(97, 66);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Name = "CmbParentItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Leave += new System.EventHandler(CmbParentItemHistory_Leave);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(978, 42);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 13;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(978, 25);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 12;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(3, 72);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(69, 15);
		this.label36.TabIndex = 187;
		this.label36.Text = "Parent Item";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(977, 8);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 11;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(3, 44);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(93, 15);
		this.label32.TabIndex = 185;
		this.label32.Text = "Validity From,To";
		this.CmbBuyerNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance283).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance283).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance283;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance284).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance286).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance286;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance287).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance287;
		((AppearanceBase)appearance288).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Location = new System.Drawing.Point(434, 66);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Name = "CmbBuyerNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).TabIndex = 7;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(361, 72);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(51, 15);
		this.label29.TabIndex = 182;
		this.label29.Text = "Supplier";
		this.CmbCommissionAgentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Location = new System.Drawing.Point(434, 38);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Name = "CmbCommissionAgentHistory";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgentHistory).TabIndex = 6;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(361, 36);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(73, 30);
		this.label31.TabIndex = 180;
		this.label31.Text = "Commission\r\nAgent";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(3, 17);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(91, 15);
		this.label33.TabIndex = 171;
		this.label33.Text = "Date From && To";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label30);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1080, 29);
		this.panel4.TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(185, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "Supplier Offer History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator3, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1080, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.CmbDeliverToAddressHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliverToAddressHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliverToAddressHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliverToAddressHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance130).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliverToAddressHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliverToAddressHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Location = new System.Drawing.Point(738, 38);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Name = "CmbDeliverToAddressHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliverToAddressHistory).TabIndex = 9;
		this.CmbDeliveryToPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryToPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryToPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryToPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryToPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryToPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Location = new System.Drawing.Point(738, 11);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Name = "CmbDeliveryToPartyHistory";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryToPartyHistory).TabIndex = 8;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(636, 44);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(105, 15);
		this.label39.TabIndex = 531;
		this.label39.Text = "Deliver To Address\r\n";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(636, 17);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(90, 15);
		this.label55.TabIndex = 530;
		this.label55.Text = "Deliver To Party";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1094, 811);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmSupplierOfferCmagt";
		this.Text = "frmSupplierOfferCmagt";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSupplierOfferCmagt_FormClosing);
		base.Load += new System.EventHandler(frmSupplierOfferCmagt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSupplierOfferCmagt_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel14.ResumeLayout(false);
		this.tabExpenses.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel5.ResumeLayout(false);
		this.tabEmptyBags.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagsPm).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabPaymentSchedule.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInquiryMapping).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbParentItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTerm).EndInit();
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBrokeryType).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAc).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbShipToAddress).EndInit();
		this.panel26.ResumeLayout(false);
		this.panel26.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCompanyName).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExpenseHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel24.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagPmHistory).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBagHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSaleOrderMappingHistory).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgentHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliverToAddressHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryToPartyHistory).EndInit();
		base.ResumeLayout(false);
	}
}

}
