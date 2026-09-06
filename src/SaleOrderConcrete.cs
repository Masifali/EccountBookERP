using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.pcc.LookUp;
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

public class SaleOrderConcrete : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtdtail = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable dtJob = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtItemForWages = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool HasMultiCurrencyFeature;

	private bool UpdateMode;

	private int updateDetailIndex;

	private bool Approved;

	private int ItemId;

	private int DetailPoId;

	private int BaseCurrency;

	private double BaseRate;

	public static int Id;

	private bool ItemSearchWithCode;

	private bool PartySearchWithCode;

	private DataTable dtCustomerHistory = new DataTable();

	private List<SaleOrderDetail> lstRemoveRecord = new List<SaleOrderDetail>();

	public UserAccount UserAccount = new UserAccount();

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

	private Label label12;

	private TextBox txtsupprefno;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label6;

	private Label label25;

	private Label label20;

	private Label label19;

	private TextBox txtcommremarks;

	private TextBox txtCommrate;

	private Label label11;

	private TextBox txtcommamount;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private Label label28;

	private TextBox txtdeliverydays;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtamount;

	private TextBox txtqty;

	private Label label40;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label38;

	private Label label37;

	private Panel panel7;

	private Panel panel9;

	private Panel panel8;

	internal UltraCombo combsuppname;

	internal UltraCombo combsalesman;

	internal UltraCombo combitem;

	internal UltraCombo combjob;

	internal UltraCombo combcityarea;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnRefreshHistory;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combpttrm;

	internal UltraCombo combcommtype;

	private ToolStripButton btnDelete;

	private ToolStripButton btnSaveAs;

	private GroupBox groupBox1;

	private Panel panel10;

	private Label label2;

	private Label label5;

	private TextBox txtOrderQty;

	private Label label43;

	private TextBox txtOrderWeight;

	private Label label34;

	internal UltraCombo cmbCurrency;

	private Label label15;

	private Label label48;

	private TextBox txtExchangeRate;

	private TextBox txtFcyAmount;

	private Label label16;

	private CheckBox chkCommissionOnSale;

	internal UltraCombo CmbVisitedbyName;

	private Label label49;

	private GroupBox groupBox2;

	internal UltraCombo CmbAttributeVarient;

	private Label label53;

	private Label label52;

	private Label label51;

	private TextBox txtItemWeight;

	private TextBox txtVarientEquivalent;

	private TabControl tabControl2;

	private TabPage TabDetail;

	internal GridEX grd;

	internal UltraCombo CmbDiscountType;

	private Label label54;

	private Label label55;

	private TextBox txtDiscountRate;

	private TextBox txtDiscountAmount;

	private Label label56;

	private Label label57;

	private TextBox txtTotalAmount;

	private Panel panel14;

	internal GridEX grdContractorWages;

	private Panel panel6;

	private Label label58;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private Button btnDefinePlant;

	private ToolStripButton btnshortcutkeys;

	internal GridEX DataGridHistoryDetail;

	private Panel panel13;

	private Label label1;

	private Button btnUpdateDiscountInGrid;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Panel panel12;

	internal UltraCombo cmbApproveStatusHistory;

	private Label label4;

	internal UltraCombo CmbIsApprovedHistory;

	private Button btnshowHistory;

	private Label label3;

	private DateTimePicker ToDate;

	private Label label7;

	private DateTimePicker fromdate;

	private Label label9;

	internal UltraCombo CmbItemHistory;

	private Label label10;

	internal UltraCombo CmbSupplierHistory;

	private Label label13;

	private Panel panel11;

	internal UltraCombo CmbFrieghtAccount;

	private TextBox txtOrderAmount;

	private Label label63;

	private Label label65;

	private Label label64;

	private Label label62;

	private Label label66;

	private Label label61;

	private Label label59;

	private Label label22;

	private TextBox txtDiscountFooter;

	private TextBox txtItemAmountFooter;

	private TextBox txtItemNetAmountFooter;

	private TextBox txtFrieghtFooter;

	private TextBox txtTotalWagesFooter;

	private Label label27;

	private Label label18;

	private Label label14;

	private TextBox txtRefPartyCellNo;

	private TextBox txtRefPartyAddress;

	private TextBox txtremarks;

	private Label label39;

	private Label label36;

	private Label label32;

	private Label label41;

	private TextBox txtNetWeight;

	internal UltraCombo CmbOtherWagesAccount;

	private Label label46;

	private Label label42;

	private TextBox txtOtherWagesFooter;

	internal UltraCombo CmbRefSalesMan;

	private Label label50;

	private Button BtnDefineCity;

	private TextBox txtAddLessRate;

	private Label label31;

	private TextBox txtItemPrice;

	private Label label29;

	private TextBox txtDistance;

	private Label label60;

	private Label label67;

	private Label label68;

	private TextBox txtAdjustDiscamount;

	private RadioButton RdPartyByCode;

	private RadioButton RdPartyByName;

	private RadioButton RdCustomerHistorySearchByCode;

	private RadioButton RdCustomerHistorySearchByName;

	internal UltraCombo Cmbheight;

	internal UltraCombo Cmbstoreys;

	internal UltraCombo CmbReferenceParty;

	private Label label70;

	private Label label69;

	private TextBox txtArea;

	private Label label21;

	private Button BtnDefineBuildingHeights;

	private Button BtnDefineBuildingStoreys;

	private Button BtnDefineRefParties;

	private Label label71;

	private TextBox txtManualTotalOrderAmount;

	private Panel PanelMessage;

	private ToolStrip ToolStripMessage;

	private ToolStripButton BtnMessage;

	public SaleOrderConcrete(UserAccount user)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1320, 775);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnprint.Enabled = formright.DoHavePrintRights;
			ChkBox.Checked = formright.DoHavePrintRights;
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
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("ItemWeight", typeof(double));
			dtitem.Columns.Add("ItemRate", typeof(double));
			dtitem.Columns.Add("ScheduleId", typeof(int));
			dtItemForWages.Columns.Add("Id", typeof(int));
			dtItemForWages.Columns.Add("ItemName", typeof(string));
			dtItemForWages.Columns.Add("ItemCode", typeof(string));
			dtdtail.Columns.Add("Id", typeof(int));
			dtdtail.Columns.Add("ItemId", typeof(int));
			dtdtail.Columns.Add("ItemCode", typeof(string));
			dtdtail.Columns.Add("Item", typeof(string));
			dtdtail.Columns.Add("ScheduleId", typeof(int));
			dtdtail.Columns.Add("AttributeVarientId", typeof(int));
			dtdtail.Columns.Add("AttributeVarient", typeof(string));
			dtdtail.Columns.Add("VarientUnit", typeof(double));
			dtdtail.Columns.Add("QTY", typeof(double));
			dtdtail.Columns.Add("Weight", typeof(double));
			dtdtail.Columns.Add("NetWeight", typeof(double));
			dtdtail.Columns.Add("ItemPrice", typeof(double));
			dtdtail.Columns.Add("AddLessRate", typeof(double));
			dtdtail.Columns.Add("NetRate", typeof(double));
			dtdtail.Columns.Add("Amount", typeof(double));
			dtdtail.Columns.Add("DiscountId", typeof(int));
			dtdtail.Columns.Add("DiscountType", typeof(string));
			dtdtail.Columns.Add("DiscRate", typeof(double));
			dtdtail.Columns.Add("DiscAmount", typeof(double));
			dtdtail.Columns.Add("TotalAmount", typeof(double));
			dtdtail.Columns.Add("JobLotId", typeof(int));
			dtdtail.Columns.Add("JobLot", typeof(string));
			dtdtail.Columns.Add("CityID", typeof(int));
			dtdtail.Columns.Add("City", typeof(string));
			dtdtail.Columns.Add("Remarks", typeof(string));
			dtdtail.Columns.Add("FcyAmount", typeof(double));
			dtdtail.Columns.Add("CommOnSale", typeof(bool));
			grd.DataSource = dtdtail;
			grd.RetrieveStructure();
			grdSettings();
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("JobLotDescription");
			dtWagesDetail.Columns.Add("Id", typeof(int));
			dtWagesDetail.Columns.Add("ItemId", typeof(int));
			dtWagesDetail.Columns.Add("ItemName");
			dtWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtWagesDetail.Columns.Add("ContractorName");
			dtWagesDetail.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtWagesDetail.Columns.Add("ServiceActivity");
			dtWagesDetail.Columns.Add("Qty", typeof(decimal));
			dtWagesDetail.Columns.Add("Rate", typeof(decimal));
			dtWagesDetail.Columns.Add("Amount", typeof(decimal));
			dtWagesDetail.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			ItemSearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithCode)
			{
				rdbtnItemCode.Checked = true;
			}
			else
			{
				rdbtnItemName.Checked = true;
			}
			PartySearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SupplierCustomerDefaultFilterByPartyCode"));
			if (PartySearchWithCode)
			{
				RdPartyByCode.Checked = true;
			}
			else
			{
				RdPartyByName.Checked = true;
			}
			if (PartySearchWithCode)
			{
				RdCustomerHistorySearchByCode.Checked = true;
			}
			else
			{
				RdCustomerHistorySearchByName.Checked = true;
			}
			DocumentNoFill();
			SupplierNameFill();
			CommAgentNameFill();
			ReferencePartyFill();
			BuildingStoreysFill();
			BuildingHeightFill();
			VisitedByFill();
			PaymentTerms();
			DeliveryTerms();
			CurrencyFill();
			MultiCurrencyFeature();
			ConfigurationDefault();
			CommissionTypeFill();
			TransporterAcFill();
			OtherWagesAcFill();
			ItemDetailFill();
			DiscountTypefill();
			combojoblotfill();
			cmbCityFill();
			dtForGridComboFill();
			RefSalesManFill();
			dtCustomerHistory.Columns.Add("Id", typeof(int));
			dtCustomerHistory.Columns.Add("PartyName");
			dtCustomerHistory.Columns.Add("PartyCode");
			GetDataFromSaleOrdersForComboBind();
			StatusFill();
			ApproveStatusFill();
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnSaveAs.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)combcommtype).Enabled = false;
			txtCommrate.Enabled = false;
			txtcommamount.Enabled = false;
			txtcommremarks.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = SaleOrder.GenerateCode(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1852
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbFrieghtAccount.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,3");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbFrieghtAccount, "Id", "AccountTitle", "Account Title", true);
			if (ItId1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					CmbFrieghtAccount.Value = ItId1;
				}
				else
				{
					((Control)(object)CmbFrieghtAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherWagesAcFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbOtherWagesAccount.Value);
			dtAccountlst.Clear();
			dtAccountlst = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10");
			if (dtAccountlst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtAccountlst, CmbOtherWagesAccount, "Id", "AccountTitle", "OtherWages Account Title", true);
			if (ItId1 > 0)
			{
				if (dtAccountlst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					CmbOtherWagesAccount.Value = ItId1;
				}
				else
				{
					((Control)(object)CmbOtherWagesAccount).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(combsuppname.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtsuppcus = CommonServices.GetVendorsAndCustomers(2);
			}
			else
			{
				dtsuppcus = CommonServices.SupplierCustomerGetforComboServiceBind();
			}
			if (dtsuppcus.Rows.Count > 0)
			{
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtsuppcus, combsuppname, "Id", "CompanyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtsuppcus, combsuppname, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
			}
			if (ItId1 > 0)
			{
				if (dtsuppcus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					combsuppname.Value = ItId1;
				}
				else
				{
					((Control)(object)combsuppname).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommAgentNameFill()
	{
		try
		{
			int ItId = Conversion.ToInt(combsalesman.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dt = CommonServices.GetVendorsAndCustomers();
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, combsalesman, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
					((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					if (ItId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
						{
							combsalesman.Value = ItId;
						}
						else
						{
							((Control)(object)combsalesman).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)combsalesman).Text = string.Empty;
					((UltraGridBase)combsalesman).DataSource = null;
				}
				return;
			}
			DataTable dt2 = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt2.Rows.Count > 0)
			{
				DDL.BindDDL(dt2, combsalesman, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
				if (ItId > 0)
				{
					if (dt2.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						combsalesman.Value = ItId;
					}
					else
					{
						((Control)(object)combsalesman).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combsalesman).Text = string.Empty;
				((UltraGridBase)combsalesman).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VisitedByFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(CmbVisitedbyName.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 2
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbVisitedbyName, "Id", "ReferencePartyName", "Visited By", ZeroIndex: true);
				((UltraGridBase)CmbVisitedbyName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbVisitedbyName.Value = ItId;
					}
					else
					{
						((Control)(object)CmbVisitedbyName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbVisitedbyName).Text = string.Empty;
				((UltraGridBase)CmbVisitedbyName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BuildingHeightFill()
	{
		try
		{
			int ItId = Conversion.ToInt(Cmbheight.Value);
			DataTable dt = LookUps.GetByLookUpTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, Cmbheight, "Id", "LookupName", "Building Height", true);
				((UltraGridBase)Cmbheight).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						Cmbheight.Value = ItId;
					}
					else
					{
						((Control)(object)Cmbheight).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)Cmbheight).Text = string.Empty;
				((UltraGridBase)Cmbheight).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BuildingStoreysFill()
	{
		try
		{
			int ItId = Conversion.ToInt(Cmbstoreys.Value);
			DataTable dt = LookUps.GetByLookUpTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 2);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, Cmbstoreys, "Id", "LookupName", "Building Storey", true);
				((UltraGridBase)Cmbstoreys).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						Cmbstoreys.Value = ItId;
					}
					else
					{
						((Control)(object)Cmbstoreys).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)Cmbstoreys).Text = string.Empty;
				((UltraGridBase)Cmbstoreys).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferencePartyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(CmbReferenceParty.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 1
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbReferenceParty, "Id", "ReferencePartyName", "Reference Party Name", false);
				((UltraGridBase)CmbReferenceParty).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbReferenceParty.Value = ItId;
					}
					else
					{
						((Control)(object)CmbReferenceParty).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbReferenceParty).Text = string.Empty;
				((UltraGridBase)CmbReferenceParty).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefSalesManFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(CmbRefSalesMan.Value);
			DataTable dt = ReferenceParties.ReferencePArtyByReferencePartyTypeIdOrSupplierCustomerId(new ReferenceParties
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferencePartyTypeId = 3,
				SupplierCustomerId = Conversion.ToInt(combsalesman.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRefSalesMan, "Id", "ReferencePartyName", "RefSalesMan", ZeroIndex: true);
				((UltraGridBase)CmbRefSalesMan).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						CmbRefSalesMan.Value = ItId;
					}
					else
					{
						((Control)(object)CmbRefSalesMan).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRefSalesMan).Text = string.Empty;
				((UltraGridBase)CmbRefSalesMan).DataSource = null;
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
			int ItId = Conversion.ToInt(combpttrm.Value);
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)combpttrm).Rows[1].Activate();
				((UltraGridBase)combpttrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						combpttrm.Value = ItId;
					}
					else
					{
						((Control)(object)combpttrm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combpttrm).Text = string.Empty;
				((UltraGridBase)combpttrm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerms()
	{
		try
		{
			int ItId = Conversion.ToInt(combdeliverytrm.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Factory Loading");
			dt.Rows.Add(2, "Delivery");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: true);
			((UltraGridBase)combdeliverytrm).Rows[1].Activate();
			((UltraGridBase)combdeliverytrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					combdeliverytrm.Value = ItId;
				}
				else
				{
					((Control)(object)combdeliverytrm).Text = string.Empty;
				}
			}
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
			int ItId = Conversion.ToInt(combcommtype.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			DDL.BindDDL(dt, combcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: true);
			((UltraGridBase)combcommtype).Rows[1].Activate();
			((UltraGridBase)combcommtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					combcommtype.Value = ItId;
				}
				else
				{
					((Control)(object)combcommtype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsalesman_Leave(object sender, EventArgs e)
	{
		try
		{
			RefSalesManFill();
			TotalCommissionAmount();
			CalculateTotalInformation();
			EnableDiableCommissionFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			int ItId = Conversion.ToInt(combitem.Value);
			dtitem.Clear();
			DataTable dt = ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = DocDate.Value,
				DocumentTypeId = 6
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["WeightKgs"], dt.Rows[i]["PreviousRate"], dt.Rows[i]["ScheduleId"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (ItId > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						combitem.Value = ItId;
					}
					else
					{
						((Control)(object)combitem).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)combitem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId", typeof(int));
				dt.Columns.Add("ItemAttribute");
				dt.Columns.Add("VarientEquivalent", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["ItemAttribute"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dt, CmbAttributeVarient, "ItemAttributeVarientId", "ItemAttribute", "Varient", ZeroIndex: true);
				if (ItId != null && ItId != string.Empty)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("ItemAttribute") == ItId))
					{
						((Control)(object)CmbAttributeVarient).Text = ItId;
					}
					else
					{
						((Control)(object)CmbAttributeVarient).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAttributeVarient).Text = string.Empty;
				txtVarientEquivalent.Text = string.Empty;
				((UltraGridBase)CmbAttributeVarient).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			int ItId = Conversion.ToInt(combjob.Value);
			dtJob.Clear();
			dtJob = CommonServices.JobLotGetAllService();
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, combjob, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: true);
				((UltraGridBase)combjob).Rows[0].Activate();
				((UltraGridBase)combjob).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (ItId > 0)
			{
				if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					combjob.Value = ItId;
				}
				else
				{
					((Control)(object)combjob).Text = string.Empty;
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
		try
		{
			int ItId = Conversion.ToInt(combcityarea.Value);
			DataTable dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCity, combcityarea, "Id", "CityName", "Description", true);
				((UltraGridBase)combcityarea).Rows[0].Activate();
				((UltraGridBase)combcityarea).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ItId > 0)
				{
					if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						combcityarea.Value = ItId;
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

	private void DiscountTypefill()
	{
		try
		{
			int ItId = Conversion.ToInt(CmbDiscountType.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DiscountType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			DDL.BindDDL(dt, CmbDiscountType, "Id", "DiscountType", "DiscountType", ZeroIndex: true);
			((UltraGridBase)CmbDiscountType).Rows[0].Activate();
			((UltraGridBase)CmbDiscountType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbDiscountType.Value = ItId;
				}
				else
				{
					((Control)(object)CmbDiscountType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtForGridComboFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtContr.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
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
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				combcityarea.Value = CityId;
			}
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
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(combitem.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemWeight"].Value);
				double price = 0.0;
				price = Conversion.ToDouble(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemRate"].Value);
				txtItemPrice.Text = price.ToString("#,##0.####");
				txtRate.Text = price.ToString("#,##0.####");
				bindvarientunit(Conversion.ToInt(combitem.Value));
			}
			else
			{
				txtItemPrice.Text = 0.ToString();
				txtItemWeight.Text = 0.ToString();
				txtRate.Text = "0";
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

	private void CmbAttributeVarient_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "combitem")
			{
				combitem.Focus();
			}
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
			if (dtsuppcus.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(combsuppname.Value) > 0)
				{
					Id = Conversion.ToInt(combsuppname.Value);
				}
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtsuppcus, combsuppname, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtsuppcus, combsuppname, "Id", "PartyCode", "Customer Code", ZeroIndex: true);
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

	private void RdCustomerHistorySearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtCustomerHistory.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbSupplierHistory.Value) > 0)
				{
					Id = Conversion.ToInt(CmbSupplierHistory.Value);
				}
				if (RdCustomerHistorySearchByName.Checked)
				{
					DDL.BindDDL(dtCustomerHistory, CmbSupplierHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtCustomerHistory, CmbSupplierHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					CmbSupplierHistory.Value = Id;
				}
				CmbSupplierHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combsuppname).ActiveRow == null || Conversion.ToInt(combsuppname.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			combsuppname.Focus();
			return false;
		}
		if (((UltraGridBase)CmbVisitedbyName).ActiveRow == null || Conversion.ToInt(CmbVisitedbyName.Value) == 0)
		{
			MessageBox.Show("Visitedby Name Field is Required");
			CmbVisitedbyName.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null || Conversion.ToInt(combpttrm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit" && (txtduedays.Text.Trim() == "" || txtduedays.Text.Trim() == null))
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (((Control)(object)CmbReferenceParty).Text.Trim() == "" || ((Control)(object)CmbReferenceParty).Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Name Field is Required");
			CmbReferenceParty.Focus();
			return false;
		}
		if (txtRefPartyAddress.Text.Trim() == "" || txtRefPartyAddress.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Address Field is Required");
			txtRefPartyAddress.Focus();
			return false;
		}
		if (txtRefPartyCellNo.Text.Trim() == "" || txtRefPartyCellNo.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party CellNo Field is Required");
			txtRefPartyCellNo.Focus();
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

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)combitem).ActiveRow == null || Conversion.ToInt(combitem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAttributeVarient).ActiveRow == null || Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Varient Field is Required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || txtqty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == string.Empty || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDiscountType.Value) != 0 && (txtDiscountRate.Text.Trim() == "" || txtDiscountRate.Text.Trim() == "0"))
		{
			MessageBox.Show("Discount Rate Field is Required when Discount Type is Selected");
			txtDiscountRate.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbDiscountType.Value) != 0 && (txtDiscountAmount.Text.Trim() == "" || txtDiscountAmount.Text.Trim() == "0"))
		{
			MessageBox.Show("Discount Amount Field is Required when Discount Type is Selected");
			txtDiscountAmount.Focus();
			return false;
		}
		if (((UltraGridBase)combjob).ActiveRow == null || Conversion.ToInt(combjob.Value) == 0)
		{
			MessageBox.Show("Job Lot Field is Required");
			combjob.Focus();
			return false;
		}
		if (((UltraGridBase)combcityarea).ActiveRow == null || Conversion.ToInt(combcityarea.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			combcityarea.Focus();
			return false;
		}
		return true;
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
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
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, combitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					combitem.Value = ItemId;
				}
				combitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableDiableCommissionFields()
	{
		try
		{
			if (Conversion.ToInt(combsalesman.Value) > 0)
			{
				((Control)(object)combcommtype).Enabled = true;
				txtCommrate.Enabled = true;
				txtcommamount.Enabled = true;
				txtcommremarks.Enabled = true;
				combcommtype.Focus();
			}
			else
			{
				((Control)(object)combcommtype).Enabled = false;
				txtCommrate.Enabled = false;
				txtcommamount.Enabled = false;
				txtcommremarks.Enabled = false;
			}
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
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
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
			decimal TotalQty = default(decimal);
			decimal TotalWeight = default(decimal);
			decimal ItemAmount = default(decimal);
			decimal DiscountAmount = default(decimal);
			decimal ItemTotalAmount = default(decimal);
			decimal TotalFcyAmount = default(decimal);
			decimal FrieghtFooter = default(decimal);
			decimal WagesFooter = default(decimal);
			decimal OrderAmount = default(decimal);
			decimal AdjustDiscAmount = default(decimal);
			if (grd.GetRows().Count() > 0)
			{
				TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["QTY"], (AggregateFunction)2));
				TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				ItemAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
				DiscountAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["DiscAmount"], (AggregateFunction)2));
				ItemTotalAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["TotalAmount"], (AggregateFunction)2));
				TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
			}
			if (grdContractorWages.GetRows().Count() > 0)
			{
				WagesFooter = Conversion.ToDecimal(grdContractorWages.GetTotal(grdContractorWages.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			AdjustDiscAmount = Conversion.ToDecimal((object)txtAdjustDiscamount.Text);
			txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
			txtTotalWagesFooter.Text = Math.Round(WagesFooter, 2).ToString("#,##0.##");
			txtOrderWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
			txtItemAmountFooter.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtDiscountFooter.Text = DiscountAmount.ToString(clsGlobalVariables.stringFormatsingle);
			ItemTotalAmount += AdjustDiscAmount;
			txtItemNetAmountFooter.Text = ItemTotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			FrieghtFooter = Conversion.ToDecimal((object)txtFrieghtFooter.Text.Trim());
			if (Conversion.ToInt(CmbFrieghtAccount.Value) > 0)
			{
				DataTable dt2 = CommonServices.GetSupplierCustomerInfoByGLAccountId(Conversion.ToInt(CmbFrieghtAccount.Value));
				if (dt2.Rows.Count > 0)
				{
					int SupGlId2 = Conversion.ToInt(dt2.Rows[0][0]);
					OrderAmount = ((Conversion.ToInt(CmbFrieghtAccount.Value) != SupGlId2) ? (ItemTotalAmount + FrieghtFooter) : (ItemTotalAmount - FrieghtFooter));
				}
				else
				{
					OrderAmount = ItemTotalAmount + FrieghtFooter;
				}
			}
			else
			{
				OrderAmount = ItemTotalAmount + FrieghtFooter;
			}
			decimal OtherWagesFooter = Conversion.ToDecimal((object)txtOtherWagesFooter.Text.Trim());
			if (Conversion.ToInt(CmbOtherWagesAccount.Value) > 0)
			{
				DataTable dt = CommonServices.GetSupplierCustomerInfoByGLAccountId(Conversion.ToInt(CmbOtherWagesAccount.Value));
				if (dt.Rows.Count > 0)
				{
					int SupGlId = Conversion.ToInt(dt.Rows[0][0]);
					if (Conversion.ToInt(CmbOtherWagesAccount.Value) == SupGlId)
					{
						OrderAmount -= OtherWagesFooter;
					}
					else
					{
						OrderAmount += OtherWagesFooter;
					}
				}
				else
				{
					OrderAmount += OtherWagesFooter;
				}
			}
			else
			{
				OrderAmount += OtherWagesFooter;
			}
			OrderAmount += WagesFooter;
			txtOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtManualTotalOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1852",
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

	private void MultiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label15.Visible = true;
				label16.Visible = true;
				label48.Visible = true;
				((Control)(object)CmbReferenceParty).Size = new Size(193, 26);
				txtRefPartyAddress.Size = new Size(193, 23);
				txtRefPartyCellNo.Size = new Size(193, 23);
				BtnDefineRefParties.Location = new Point(789, 70);
				BtnDefineRefParties.Size = new Size(29, 26);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label15.Visible = false;
				label16.Visible = false;
				label48.Visible = false;
				((Control)(object)CmbReferenceParty).Size = new Size(360, 26);
				txtRefPartyAddress.Size = new Size(390, 23);
				txtRefPartyCellNo.Size = new Size(390, 23);
				BtnDefineRefParties.Location = new Point(986, 70);
				BtnDefineRefParties.Size = new Size(29, 26);
			}
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
					decimal amount = Conversion.ToDecimal(obj.Cells["Amount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
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

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			Approved = false;
			Id = 0;
			ItemId = 0;
			BaseRate = 0.0;
			BaseCurrency = 0;
			DetailPoId = 0;
			dtdtail.Clear();
			grd.DataSource = dtdtail;
			txtArea.Text = string.Empty;
			((Control)(object)combitem).Text = string.Empty;
			((Control)(object)CmbFrieghtAccount).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtqty.Text = string.Empty;
			txtItemPrice.Text = string.Empty;
			txtAddLessRate.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtamount.Text = string.Empty;
			((Control)(object)CmbDiscountType).Text = string.Empty;
			txtDiscountAmount.Text = string.Empty;
			txtDiscountRate.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			((Control)(object)combjob).Text = string.Empty;
			((Control)(object)combcityarea).Text = string.Empty;
			txtremarks.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			((Control)(object)combsuppname).Text = string.Empty;
			((Control)(object)combsalesman).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			((UltraGridBase)combcommtype).Rows[1].Activate();
			txtCommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			((Control)(object)CmbReferenceParty).Text = string.Empty;
			txtRefPartyAddress.Text = string.Empty;
			txtRefPartyCellNo.Text = string.Empty;
			txtOrderQty.Text = string.Empty;
			txtOrderWeight.Text = string.Empty;
			txtDiscountFooter.Text = string.Empty;
			txtItemAmountFooter.Text = string.Empty;
			txtItemNetAmountFooter.Text = string.Empty;
			txtOrderAmount.Text = string.Empty;
			txtFrieghtFooter.Text = string.Empty;
			txtManualTotalOrderAmount.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnSaveAs.Visible = false;
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
			EnableDiableCommissionFields();
			DocDate.Focus();
			DocumentNoFill();
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
			txtDiscountAmount.Text = string.Empty;
			txtDiscountRate.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			combitem.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ReferencePartyFill();
			BuildingStoreysFill();
			BuildingHeightFill();
			VisitedByFill();
			RefSalesManFill();
			SupplierNameFill();
			CommAgentNameFill();
			ItemDetailFill();
			combojoblotfill();
			cmbCityFill();
			TransporterAcFill();
			ConfigurationDefault();
			dtForGridComboFill();
			WagesItemFillFromDetail();
			OtherWagesAcFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtdtail.Rows.Add(0, Conversion.ToInt(combitem.Value), Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ScheduleId"].Value), Conversion.ToInt(CmbAttributeVarient.Value), Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text), Conversion.ToDouble((object)txtVarientEquivalent.Text), Conversion.ToDouble((object)txtqty.Text), Conversion.ToDouble((object)txtItemWeight.Text), Conversion.ToDouble((object)txtNetWeight.Text), Conversion.ToDouble((object)txtItemPrice.Text), Conversion.ToDouble((object)txtAddLessRate.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToDouble((object)txtamount.Text), Conversion.ToInt(CmbDiscountType.Value), Conversion.ToString((object)((Control)(object)CmbDiscountType).Text), Conversion.ToDouble((object)txtDiscountRate.Text), Conversion.ToDouble((object)txtDiscountAmount.Text), Conversion.ToDouble((object)txtTotalAmount.Text), Conversion.ToInt(combjob.Value), Conversion.ToString((object)((Control)(object)combjob).Text.Trim()), Conversion.ToInt(combcityarea.Value), Conversion.ToString((object)((Control)(object)combcityarea).Text.Trim()), Conversion.ToString((object)txtremarksdetail.Text.Trim()), chkCommissionOnSale.Checked);
				grd.DataSource = dtdtail;
				grd.RetrieveStructure();
				grdSettings();
				WagesItemFillFromDetail();
				TotalCommissionAmount();
				CalculateTotalInformation();
				ResetDetail();
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
				ItemId = 0;
				dtdtail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(combitem.Value);
				dtdtail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemCode"].Value);
				dtdtail.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)combitem).SelectedRow.Cells["ItemName"].Value);
				dtdtail.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToInt(((UltraDropDownBase)combitem).SelectedRow.Cells["ScheduleId"].Value);
				dtdtail.Rows[updateDetailIndex]["AttributeVarientId"] = Conversion.ToInt(CmbAttributeVarient.Value);
				dtdtail.Rows[updateDetailIndex]["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
				dtdtail.Rows[updateDetailIndex]["VarientUnit"] = Conversion.ToDouble((object)txtVarientEquivalent.Text);
				dtdtail.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtqty.Text);
				dtdtail.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtItemWeight.Text);
				dtdtail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
				dtdtail.Rows[updateDetailIndex]["ItemPrice"] = Conversion.ToDouble((object)txtItemPrice.Text);
				dtdtail.Rows[updateDetailIndex]["AddLessRate"] = Conversion.ToDouble((object)txtAddLessRate.Text);
				dtdtail.Rows[updateDetailIndex]["NetRate"] = Conversion.ToDouble((object)txtRate.Text);
				dtdtail.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtamount.Text);
				dtdtail.Rows[updateDetailIndex]["DiscountId"] = Conversion.ToDouble(CmbDiscountType.Value);
				dtdtail.Rows[updateDetailIndex]["DiscountType"] = Conversion.ToString((object)((Control)(object)CmbDiscountType).Text);
				dtdtail.Rows[updateDetailIndex]["DiscRate"] = Conversion.ToDouble((object)txtDiscountRate.Text);
				dtdtail.Rows[updateDetailIndex]["DiscAmount"] = Conversion.ToDouble((object)txtDiscountAmount.Text);
				dtdtail.Rows[updateDetailIndex]["TotalAmount"] = Conversion.ToDouble((object)txtTotalAmount.Text);
				dtdtail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(combjob.Value);
				dtdtail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)combjob).Text);
				dtdtail.Rows[updateDetailIndex]["CityID"] = Conversion.ToInt(combcityarea.Value);
				dtdtail.Rows[updateDetailIndex]["City"] = Conversion.ToString((object)((Control)(object)combcityarea).Text);
				dtdtail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
				dtdtail.Rows[updateDetailIndex]["CommOnSale"] = chkCommissionOnSale.Checked;
				grd.DataSource = dtdtail;
				chkCommissionOnSale.Checked = false;
				txtExchangeRate_TextChanged(null, null);
				WagesItemFillFromDetail();
				TotalCommissionAmount();
				CalculateTotalInformation();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				bindvarientunit(ItemId);
				CmbAttributeVarient.Value = item.Cells["AttributeVarientId"].Value.ToString();
				txtVarientEquivalent.Text = item.Cells["VarientUnit"].Value.ToString();
				txtItemWeight.Text = item.Cells["Weight"].Value.ToString();
				txtNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtqty.Text = item.Cells["QTY"].Value.ToString();
				txtItemPrice.Text = item.Cells["ItemPrice"].Value.ToString();
				txtAddLessRate.Text = item.Cells["AddLessRate"].Value.ToString();
				txtRate.Text = item.Cells["NetRate"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				CmbDiscountType.Value = item.Cells["DiscountId"].Value.ToString();
				txtDiscountRate.Text = item.Cells["DiscRate"].Value.ToString();
				txtDiscountAmount.Text = item.Cells["DiscAmount"].Value.ToString();
				txtTotalAmount.Text = item.Cells["TotalAmount"].Value.ToString();
				combjob.Value = item.Cells["JobLotId"].Value.ToString();
				combcityarea.Value = item.Cells["CityID"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				chkCommissionOnSale.Checked = Conversion.ToBool(item.Cells["CommOnSale"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CalculateDetailAmount();
				CalculateDiscountAndTotalAmount();
				combitem.Focus();
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
			WagesItemFillFromDetail();
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["CommOnSale"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["CityID"].Visible = false;
			grd.RootTable.Columns["DiscountId"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["ScheduleId"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["QTY"].EditType = (EditType)0;
			grd.RootTable.Columns["Weight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemPrice"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLessRate"].EditType = (EditType)0;
			grd.RootTable.Columns["NetRate"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscountId"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscountType"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscRate"].EditType = (EditType)0;
			grd.RootTable.Columns["DiscAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["CityID"].EditType = (EditType)0;
			grd.RootTable.Columns["City"].EditType = (EditType)0;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grd.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["CommOnSale"].EditType = (EditType)0;
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "##,#.##";
			grd.RootTable.Columns["Weight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "##,#.##";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].FormatString = "##,#.##";
			grd.RootTable.Columns["QTY"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscRate"].AggregateFunction = (AggregateFunction)3;
			grd.RootTable.Columns["DiscRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscRate"].FormatString = "#,##0.####";
			grd.RootTable.Columns["DiscRate"].TotalFormatString = "#,##0.####";
			grd.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 50;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					vd.VarientEquivalent = Conversion.ToInt(r.Cells["VarientUnit"].Value);
					vd.OrderItemQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
					vd.ItemWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
					vd.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					vd.ItemRateWithOutAddLess = Conversion.ToDecimal(r.Cells["ItemPrice"].Value);
					vd.RateAddLess = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
					vd.OrderItemRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
					vd.OrderItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					vd.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountId"].Value);
					vd.ItemDiscountRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
					vd.ItemDiscountAmount = Conversion.ToDecimal(r.Cells["DiscAmount"].Value);
					vd.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
					vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdtail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
				TotalCommissionAmount();
				txtExchangeRate_TextChanged(null, null);
				WagesItemFillFromDetail();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if ((e.Column.Key == "QTY" || e.Column.Key == "NetRate") && item != null && (int)item.RowType == 0)
			{
				double Qty = Conversion.ToDouble(item.Cells["QTY"].Value);
				double VarientUnit = Conversion.ToDouble(item.Cells["VarientUnit"].Value);
				double ItemRate = Conversion.ToDouble(item.Cells["NetRate"].Value);
				double ItemWeight = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
				if (Qty > 0.0 && VarientUnit > 0.0)
				{
					double NetWeight = Qty * VarientUnit * ItemWeight;
					item.Cells["NetWeight"].Value = NetWeight;
					double ItemAmount = Qty * VarientUnit * ItemRate;
					item.Cells["Amount"].Value = ItemAmount;
				}
				else
				{
					item.Cells["Amount"].Value = 0;
				}
			}
			grd.UpdateData();
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		dtWagesDetail.Rows.Add(0, 0, "", 0, "", 0, "", 0, 0, 0, "");
		grdContractorWages.DataSource = dtWagesDetail;
	}

	private void WagesItemFillFromDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				dtItemForWages.Clear();
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					dtItemForWages.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["Item"].Value), Conversion.ToString(r.Cells["ItemCode"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["ItemId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 40;
			grdContractorWages.RootTable.Columns["Delete"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 40;
			grdContractorWages.RootTable.Columns["Add"].Position = 1;
			grdContractorWages.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdContractorWages.CurrentRow;
				dtWagesDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
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
			CommonServices.SaleOrderSlipConcrete(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3c: Expected O, but got Unknown
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1b: Expected O, but got Unknown
		//IL_0dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Expected O, but got Unknown
		try
		{
			double BillAmount = 0.0;
			if (grd.RowCount == 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			SaleOrder po = new SaleOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToDouble((object)txtFrieghtFooter.Text.Trim()) > 0.0 && ((UltraGridBase)CmbFrieghtAccount).ActiveRow == null)
			{
				CmbFrieghtAccount.Focus();
				throw new Exception("Frieght Account field Required");
			}
			po.DocumentTypeId = 1852;
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchesId = UserAccount.BranchesId;
			po.ProjectsId = UserAccount.BranchesId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.EntryUserId = UserAccount.ID;
			po.EntryDate = DateTime.Now;
			po.ApprovedDate = DateTime.Now;
			po.ModifyUserId = UserAccount.ID;
			po.ModifyDate = DateTime.Now;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DocDate = Conversion.ToDateTime((object)DocDate.Text);
			po.SupplierCustomerId = Conversion.ToInt(combsuppname.Value);
			po.RefrenenceNo = Conversion.ToString((object)txtsupprefno.Text);
			po.VisitedById = Conversion.ToInt(CmbVisitedbyName.Value);
			po.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			po.Distance = Conversion.ToString((object)txtDistance.Text);
			po.BuildingHeightId = Conversion.ToInt(Cmbheight.Value);
			po.BuildingStoreyId = Conversion.ToInt(Cmbstoreys.Value);
			po.BuildingArea = Conversion.ToString((object)txtArea.Text);
			po.ReferencePartyId = Conversion.ToInt(CmbReferenceParty.Value);
			po.ReferencPartyName = Conversion.ToString((object)((Control)(object)CmbReferenceParty).Text);
			po.ReferencPartyAddress = Conversion.ToString((object)txtRefPartyAddress.Text);
			po.ReferencPartyCellNo = Conversion.ToString((object)txtRefPartyCellNo.Text);
			po.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
			po.OrderDueDate = Conversion.ToDateTime((object)duedate.Value);
			po.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			po.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
			po.DeliveryStartDate = Conversion.ToDateTime((object)deliverystartdate.Value);
			po.OrderExpiryDate = Conversion.ToDateTime((object)deliverystartdate.Value).AddDays(Conversion.ToInt((object)txtdeliverydays.Text));
			po.RefSalesManId = Conversion.ToInt(CmbRefSalesMan.Value);
			po.OtherWagesAccountId = Conversion.ToInt(CmbOtherWagesAccount.Value);
			po.IsApproved = false;
			po.OrderStatus = "Open";
			po.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			po.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			po.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			po.OrderQty = Conversion.ToDecimal((object)txtOrderQty.Text);
			po.OrderWeight = Conversion.ToDecimal((object)txtOrderWeight.Text);
			po.TransporterId = Conversion.ToInt(CmbFrieghtAccount.Value);
			po.OrderItemAmount = Conversion.ToDecimal((object)txtItemAmountFooter.Text);
			po.AdjustDiscountAmount = Conversion.ToDecimal((object)txtAdjustDiscamount.Text);
			po.OrderDiscountAmount = Conversion.ToDecimal((object)txtDiscountFooter.Text);
			po.OrderItemNetAmount = Conversion.ToDecimal((object)txtItemNetAmountFooter.Text);
			po.WageNetAmount = Conversion.ToDecimal((object)txtTotalWagesFooter.Text);
			po.OtherWages = Conversion.ToDecimal((object)txtOtherWagesFooter.Text);
			po.FrieghtAmount = Conversion.ToDecimal((object)txtFrieghtFooter.Text);
			po.OrderAmount = Conversion.ToDecimal((object)txtOrderAmount.Text);
			if (po.OrderAmount == 0m)
			{
				throw new Exception("OrderAmount Required");
			}
			if (Conversion.ToInt(combsalesman.Value) > 0)
			{
				po.CommissionAgentId = Conversion.ToInt(combsalesman.Value);
				po.CommissionType = Conversion.ToString((object)((Control)(object)combcommtype).Text);
				po.CommissionRate = Conversion.ToDecimal((object)txtCommrate.Text);
				po.CommissionUom = 0m;
				po.CommissionAmount = Conversion.ToDecimal((object)txtcommamount.Text.Trim());
				po.CommissionRemarks = Conversion.ToString((object)txtcommremarks.Text);
				if (po.CommissionAgentId == po.SupplierCustomerId)
				{
					BillAmount += Conversion.ToDouble((object)po.CommissionAmount);
				}
			}
			po.SaleOrderDetailList = new List<SaleOrderDetail>();
			po.SaleOrderWagesDetailList = new List<SaleOrderWagesDetail>();
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				SaleOrderDetail vd = new SaleOrderDetail();
				if (Conversion.ToDouble(r2.Cells["QTY"].Value) > 0.0)
				{
					vd.Id = ((Id != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					if (vd.Id > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
					vd.OrderItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vd.ScheduleId = Conversion.ToInt(r2.Cells["ScheduleId"].Value);
					vd.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
					vd.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
					vd.OrderItemQty = Conversion.ToDecimal(r2.Cells["QTY"].Value);
					vd.ItemWeight = Conversion.ToDecimal(r2.Cells["Weight"].Value);
					vd.ItemRateWithOutAddLess = Conversion.ToDecimal(r2.Cells["ItemPrice"].Value);
					vd.RateAddLess = Conversion.ToDecimal(r2.Cells["AddLessRate"].Value);
					vd.ItemNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					if (!(Conversion.ToDouble(r2.Cells["NetRate"].Value) > 0.0))
					{
						throw new Exception("Rate Field Required in detail and row no: " + r2.RowIndex);
					}
					vd.OrderItemRate = Conversion.ToDecimal(r2.Cells["NetRate"].Value);
					vd.OrderItemAmount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
					vd.ItemDiscountTypeId = Conversion.ToInt(r2.Cells["DiscountId"].Value);
					vd.ItemDiscountRate = Conversion.ToDecimal(r2.Cells["DiscRate"].Value);
					vd.ItemDiscountAmount = Conversion.ToDecimal(r2.Cells["DiscAmount"].Value);
					vd.ItemNetAmount = Conversion.ToDecimal(r2.Cells["TotalAmount"].Value);
					BillAmount += Conversion.ToDouble((object)vd.ItemNetAmount);
					vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					vd.CityId = Conversion.ToInt(r2.Cells["CityID"].Value);
					vd.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					vd.FcyAmount = Conversion.ToDecimal(r2.Cells["FcyAmount"].Value);
					vd.RemarksDetail = Conversion.ToString(r2.Cells["Remarks"].Value);
					vd.CommOnSale = Conversion.ToBool(r2.Cells["CommOnSale"].Value);
					po.SaleOrderDetailList.Add(vd);
				}
			}
			if (po.SaleOrderDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					po.SaleOrderDetailList.Add(lstRemoveRecord[j]);
				}
			}
			rows = grdContractorWages.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0)
				{
					SaleOrderWagesDetail d = new SaleOrderWagesDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					po.SaleOrderWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				po.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			double AvailableLimit = SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = po.SupplierCustomerId,
				Amount = BillAmount,
				ActionId = 2
			});
			if (AvailableLimit > 0.0)
			{
				if (MessageBox.Show("Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   " + Conversion.ToString((object)AvailableLimit), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}
			else if (AvailableLimit < 0.0 && MessageBox.Show("Debit Limit of Customer already Exceeds Define Debit Limit", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			int success = (po.Id = SaleOrder.Save(po));
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.SaleOrderSlipConcrete(success);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			Id = 0;
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
			if (Id == 0)
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

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			Approved = false;
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			Reset();
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			SaleOrder po = new SaleOrder();
			Id = Conversion.ToInt((object)ID);
			po = SaleOrder.GetByID(Id);
			if (po == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			combsuppname.Value = Conversion.ToInt((object)po.SupplierCustomerId);
			txtsupprefno.Text = Conversion.ToString((object)po.RefrenenceNo);
			CmbVisitedbyName.Value = Conversion.ToInt((object)po.VisitedById);
			txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
			txtDistance.Text = Conversion.ToString((object)po.Distance);
			combpttrm.Value = Conversion.ToInt((object)po.PaymentTermsId);
			CmbFrieghtAccount.Value = Conversion.ToInt((object)po.TransporterId);
			txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
			duedate.Value = po.OrderDueDate;
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)po.DeliveryTerm);
			deliverystartdate.Value = po.DeliveryStartDate;
			txtdeliverydays.Text = Conversion.ToString((object)po.DeliveryDays);
			Cmbheight.Value = Conversion.ToInt((object)po.BuildingHeightId);
			Cmbstoreys.Value = Conversion.ToInt((object)po.BuildingStoreyId);
			txtArea.Text = Conversion.ToString((object)po.BuildingArea);
			((Control)(object)CmbReferenceParty).Text = Conversion.ToString((object)po.ReferencPartyName);
			txtRefPartyAddress.Text = Conversion.ToString((object)po.ReferencPartyAddress);
			txtRefPartyCellNo.Text = Conversion.ToString((object)po.ReferencPartyCellNo);
			CmbOtherWagesAccount.Value = Conversion.ToInt((object)po.OtherWagesAccountId);
			combsalesman.Value = Conversion.ToInt((object)po.CommissionAgentId);
			CmbRefSalesMan.Value = Conversion.ToInt((object)po.RefSalesManId);
			((Control)(object)combcommtype).Text = Conversion.ToString((object)po.CommissionType);
			txtCommrate.Text = Conversion.ToString((object)po.CommissionRate);
			txtcommamount.Text = po.CommissionAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtcommremarks.Text = Conversion.ToString((object)po.CommissionRemarks);
			txtOrderQty.Text = po.OrderQty.ToString("#,##0.##");
			txtOrderWeight.Text = po.OrderWeight.ToString("#,##0.##");
			txtItemAmountFooter.Text = po.OrderItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtDiscountFooter.Text = po.OrderDiscountAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtItemNetAmountFooter.Text = po.OrderItemNetAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalWagesFooter.Text = po.WageNetAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtOtherWagesFooter.Text = po.OtherWages.ToString(clsGlobalVariables.stringFormatsingle);
			txtFrieghtFooter.Text = po.FrieghtAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtOrderAmount.Text = po.OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtManualTotalOrderAmount.Text = po.OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtAdjustDiscamount.Text = po.AdjustDiscountAmount.ToString(clsGlobalVariables.stringFormatsingle);
			Approved = Conversion.ToBool((object)po.IsApproved);
			cmbCurrency.Value = po.CurrencyId;
			if (po.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)po.ExchangeRate);
			}
			else if (po.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			txtFcyAmount.Text = po.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			dtdtail.Clear();
			for (int j = 0; j < po.SaleOrderDetailList.Count; j++)
			{
				dtdtail.Rows.Add(po.SaleOrderDetailList[j].Id, po.SaleOrderDetailList[j].OrderItemId, po.SaleOrderDetailList[j].ItemCode, po.SaleOrderDetailList[j].ItemName, po.SaleOrderDetailList[j].ScheduleId, po.SaleOrderDetailList[j].ItemAttributeVarientId, po.SaleOrderDetailList[j].ItemAttributeVarient, po.SaleOrderDetailList[j].VarientEquivalent, po.SaleOrderDetailList[j].OrderItemQty, po.SaleOrderDetailList[j].ItemWeight, po.SaleOrderDetailList[j].ItemNetWeight, po.SaleOrderDetailList[j].ItemRateWithOutAddLess, po.SaleOrderDetailList[j].RateAddLess, po.SaleOrderDetailList[j].OrderItemRate, po.SaleOrderDetailList[j].OrderItemAmount, po.SaleOrderDetailList[j].ItemDiscountTypeId, po.SaleOrderDetailList[j].ItemDiscountType, po.SaleOrderDetailList[j].ItemDiscountRate, po.SaleOrderDetailList[j].ItemDiscountAmount, po.SaleOrderDetailList[j].ItemNetAmount, po.SaleOrderDetailList[j].JobLotId, po.SaleOrderDetailList[j].JobLotDescription, po.SaleOrderDetailList[j].CityId, po.SaleOrderDetailList[j].CityName, po.SaleOrderDetailList[j].RemarksDetail, po.SaleOrderDetailList[j].FcyAmount, po.SaleOrderDetailList[j].CommOnSale);
			}
			if (po.SaleOrderWagesDetailList.Count > 0)
			{
				dtWagesDetail.Clear();
				for (int i = 0; i < po.SaleOrderWagesDetailList.Count; i++)
				{
					dtWagesDetail.Rows.Add(po.SaleOrderWagesDetailList[i].Id, po.SaleOrderWagesDetailList[i].ItemId, po.SaleOrderWagesDetailList[i].ItemName, po.SaleOrderWagesDetailList[i].ContractorId, po.SaleOrderWagesDetailList[i].ContractorName, po.SaleOrderWagesDetailList[i].ContractorWagesRateScheduleId, po.SaleOrderWagesDetailList[i].ServiceActivity, po.SaleOrderWagesDetailList[i].Qty, po.SaleOrderWagesDetailList[i].Rate, po.SaleOrderWagesDetailList[i].Amount, po.SaleOrderWagesDetailList[i].Remarks);
				}
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			grd.DataSource = dtdtail;
			grd.RetrieveStructure();
			grdSettings();
			grdContractorWages.DataSource = dtWagesDetail;
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Conversion.ToInt((object)Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnsave.Visible = false;
			btnupdate.Visible = true;
			btnupdate.Enabled = true;
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
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				new SaleOrder();
				SaleOrder.DeleteById(UserAccount.EntryUserId, Id);
				MessageBox.Show("Delete Record Seccessfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDetailAmount()
	{
		try
		{
			double qty = 0.0;
			double amount = 0.0;
			double rate = 0.0;
			double ItemWeight = 0.0;
			double NetWeight = 0.0;
			qty = Conversion.ToDouble((object)txtqty.Text);
			rate = Conversion.ToDouble((object)txtRate.Text);
			double num = Conversion.ToDouble((object)txtVarientEquivalent.Text);
			ItemWeight = Conversion.ToDouble((object)txtItemWeight.Text);
			NetWeight = Conversion.ToDouble((object)(num * qty * ItemWeight));
			txtNetWeight.Text = NetWeight.ToString("##,#.##");
			amount = Conversion.ToDouble((object)(num * qty * rate));
			if (amount < 0.0)
			{
				txtamount.Text = "";
				throw new Exception("Amount can not be Less than zero");
			}
			txtamount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
			txtTotalAmount.Text = amount.ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDiscountAndTotalAmount()
	{
		try
		{
			double ItemPercentage = Conversion.ToDouble((object)txtDiscountRate.Text);
			double ItemAmount = Conversion.ToDouble((object)txtamount.Text);
			double ItemDiscount = Conversion.ToDouble((object)txtDiscountAmount.Text);
			if (((Control)(object)CmbDiscountType).Text == "Percent")
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "DiscPercent")
				{
					if (ItemPercentage > 0.0)
					{
						ItemDiscount = ItemAmount * ItemPercentage / 100.0;
						ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
					}
					else
					{
						txtDiscountAmount.Text = "0";
					}
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "DiscAmount")
				{
					ItemPercentage = Math.Round(ItemDiscount * 100.0 / ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForRate, MidpointRounding.AwayFromZero);
					if (ItemPercentage > 99.0)
					{
						ItemPercentage = 99.0;
						txtDiscountRate.Text = ItemPercentage.ToString("#,##0.###");
						ItemDiscount = ItemAmount * ItemPercentage / 100.0;
						ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
						return;
					}
					txtDiscountRate.Text = ItemPercentage.ToString("#,##0.###");
				}
				else
				{
					ItemDiscount = ItemAmount * ItemPercentage / 100.0;
					ItemDiscount = Math.Round(ItemDiscount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					txtDiscountAmount.Text = ItemDiscount.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			else if (((Control)(object)CmbDiscountType).Text == "Flat")
			{
				if (Conversion.ToString(base.ActiveControl.Tag) == "DiscPercent")
				{
					ItemPercentage = Math.Round(Conversion.ToDouble((object)txtDiscountRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					ItemDiscount = ItemPercentage;
					txtDiscountAmount.Text = ItemPercentage.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "DiscAmount")
				{
					txtDiscountAmount.Text = Math.Round(Conversion.ToDouble((object)txtDiscountAmount.Text), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString("#,##0.###");
					ItemDiscount = Conversion.ToDouble((object)txtDiscountAmount.Text);
					ItemPercentage = ItemDiscount;
					txtDiscountRate.Text = ItemDiscount.ToString("#,##0.###");
				}
				else
				{
					ItemPercentage = Math.Round(Conversion.ToDouble((object)txtDiscountRate.Text), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					ItemDiscount = ItemPercentage;
					txtDiscountAmount.Text = ItemPercentage.ToString(clsGlobalVariables.stringFormatsingle);
				}
			}
			if (((Control)(object)CmbDiscountType).Text == "Percent" && Conversion.ToDouble((object)txtDiscountRate.Text) > 100.0)
			{
				MessageBox.Show("Discount Percentage Cannot greater than 100");
				double Percent = Conversion.ToDouble((object)txtDiscountRate.Text);
				double i = Percent - 100.0;
				txtDiscountRate.Text = (Percent - i).ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (ItemAmount < 0.0 || ItemDiscount < 0.0)
			{
				txtamount.Text = "";
				txtDiscountAmount.Text = "";
				throw new Exception("Amounts can not be Less than zero");
			}
			txtTotalAmount.Text = (ItemAmount - ItemDiscount).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherWagesFooter_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalInformation();
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
			CalculateTotalInformation();
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
			EnableDiableCommissionFields();
			TotalCommissionAmount();
			CalculateTotalInformation();
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
			CalculateTotalInformation();
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
			TotalCommissionAmount();
			CalculateTotalInformation();
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
			if (!(txtCommrate.Text != string.Empty))
			{
				return;
			}
			if (((Control)(object)combcommtype).Text == "Flat")
			{
				double CommRate2 = Conversion.ToDouble((object)txtCommrate.Text.Trim());
				txtcommamount.Text = CommRate2.ToString(clsGlobalVariables.stringFormatsingle);
			}
			if (((Control)(object)combcommtype).Text == "Percent")
			{
				double CommRate = Conversion.ToDouble((object)txtCommrate.Text.Trim());
				decimal OrderAmt = Conversion.ToDecimal((object)txtOrderAmount.Text);
				if (OrderAmt != 0m)
				{
					double CommAmt = Conversion.ToDouble((object)OrderAmt.ToString()) * CommRate / 100.0;
					txtcommamount.Text = CommAmt.ToString(clsGlobalVariables.stringFormatsingle);
				}
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
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttributeVarient).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVarientEquivalent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
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
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
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
			NetRateCalculation();
			CalculateDetailAmount();
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDiscountType_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscountRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbDiscountType.Value) == 2 && Conversion.ToDecimal((object)txtDiscountRate.Text) > 99m)
			{
				MessageBox.Show("The Discount Percent Can't Be Greater Than 99", "WARNING!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtDiscountRate.Text = 99.ToString();
			}
			CalculateDiscountAndTotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFrieghtFooter_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemPrice_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetRateCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddLessRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			NetRateCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetRateCalculation()
	{
		try
		{
			if (Conversion.ToDouble((object)txtItemPrice.Text) > 0.0 || Conversion.ToDouble((object)txtRate.Text) > 0.0)
			{
				double ItemPrice = Conversion.ToDouble((object)txtItemPrice.Text);
				double AddLess = Conversion.ToDouble((object)txtAddLessRate.Text);
				double NetRate = Conversion.ToDouble((object)txtRate.Text);
				if (Conversion.ToString(base.ActiveControl.Tag) == "NetRate")
				{
					txtAddLessRate.Text = (NetRate - ItemPrice).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "AddLess")
				{
					txtRate.Text = (ItemPrice + AddLess).ToString();
				}
				else
				{
					txtRate.Text = (ItemPrice + AddLess).ToString("#,##0.####");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDiscountInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateDiscountTypeAndRateInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateDiscountTypeAndRateInGrid()
	{
		try
		{
			if (Conversion.ToInt(CmbDiscountType.Value) == 0)
			{
				MessageBox.Show("DiscountType Required");
				CmbDiscountType.Focus();
			}
			else if (Conversion.ToDouble((object)txtDiscountRate.Text) == 0.0)
			{
				MessageBox.Show("Discount Rate Required");
				txtDiscountRate.Focus();
			}
			else if (grd.RowCount > 0)
			{
				int DiscId = Conversion.ToInt(CmbDiscountType.Value);
				string DiscType = Conversion.ToString((object)((Control)(object)CmbDiscountType).Text);
				double DisRate = Conversion.ToDouble((object)txtDiscountRate.Text);
				if (DiscId == 2 && DisRate >= 100.0)
				{
					MessageBox.Show("Discount Rate Cannot be greater than 99 when Discount Type is Percent");
					return;
				}
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					r.BeginEdit();
					r.Cells["DiscountId"].Value = DiscId;
					r.Cells["DiscountType"].Value = DiscType;
					r.Cells["DiscRate"].Value = DisRate;
					double AmtWithoutDisc = Conversion.ToDouble(r.Cells["Amount"].Value);
					double DisAmt = 0.0;
					if (Conversion.ToInt(r.Cells["DiscountId"].Value) != 0)
					{
						if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 1)
						{
							DisAmt = DisRate;
							r.Cells["DiscAmount"].Value = DisAmt;
						}
						else if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 2)
						{
							DisAmt = AmtWithoutDisc * DisRate / 100.0;
							r.Cells["DiscAmount"].Value = DisAmt;
						}
					}
					else
					{
						r.Cells["DiscRate"].Value = 0;
						r.Cells["DiscAmount"].Value = 0;
					}
					if (AmtWithoutDisc > 0.0 && DisAmt > 0.0)
					{
						r.Cells["TotalAmount"].Value = AmtWithoutDisc - DisAmt;
					}
					else if (AmtWithoutDisc > 0.0)
					{
						r.Cells["TotalAmount"].Value = AmtWithoutDisc;
					}
					else
					{
						r.Cells["TotalAmount"].Value = 0;
					}
					r.EndEdit();
				}
				TotalCommissionAmount();
				CalculateTotalInformation();
			}
			else
			{
				MessageBox.Show("Atlest one Record Required in Grid");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PropotionateDiscountInGrid()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			decimal totalGridQty = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["QTY"], (AggregateFunction)2).ToString());
			decimal FooterDisAmt = Conversion.ToDecimal((object)txtDiscountFooter.Text);
			decimal totalCalculatedDiscount = default(decimal);
			decimal Qty = default(decimal);
			decimal ItemAmount = default(decimal);
			decimal Discount = default(decimal);
			decimal DisRate = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				Qty = Conversion.ToDecimal((object)r2.Cells["QTY"].Value.ToString());
				ItemAmount = Conversion.ToDecimal((object)r2.Cells["Amount"].Value.ToString());
				Discount = FooterDisAmt / totalGridQty * Qty;
				r2.BeginEdit();
				r2.Cells["DiscAmount"].Value = Math.Round(Discount, 2);
				r2.Cells["TotalAmount"].Value = ItemAmount - Discount;
				if (Conversion.ToInt(r2.Cells["DiscountId"].Value) == 1)
				{
					DisRate = Discount;
					r2.Cells["DisRate"].Value = Math.Round(DisRate, 2);
				}
				else if (Conversion.ToInt(r2.Cells["DiscountId"].Value) == 2)
				{
					if (FooterDisAmt > 0m && Discount > 0m)
					{
						DisRate = Discount / ItemAmount * 100m;
						r2.Cells["DiscRate"].Value = Math.Round(DisRate, 2);
					}
					else
					{
						DisRate = default(decimal);
						r2.Cells["DiscRate"].Value = 0;
					}
				}
				else
				{
					DisRate = default(decimal);
					r2.Cells["DiscRate"].Value = 0;
				}
				if (ItemAmount > 0m && Discount > 0m)
				{
					r2.Cells["TotalAmount"].Value = ItemAmount - Discount;
				}
				else if (ItemAmount > 0m)
				{
					r2.Cells["TotalAmount"].Value = ItemAmount;
				}
				else
				{
					r2.Cells["TotalAmount"].Value = 0;
				}
				r2.EndEdit();
				totalCalculatedDiscount += Discount;
			}
			if (!(Math.Abs(totalCalculatedDiscount - FooterDisAmt) > 0.01m))
			{
				return;
			}
			decimal adjustmentFactor = FooterDisAmt / totalCalculatedDiscount;
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				decimal adjustedDiscount = Conversion.ToDecimal((object)r.Cells["DiscAmount"].Value.ToString()) * adjustmentFactor;
				r.BeginEdit();
				r.Cells["DiscAmount"].Value = Math.Round(adjustedDiscount, 2);
				r.Cells["TotalAmount"].Value = ItemAmount - Discount;
				if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 1)
				{
					DisRate = Discount;
					r.Cells["DisRate"].Value = Math.Round(DisRate, 2);
				}
				else if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 2)
				{
					if (ItemAmount > 0m && Discount > 0m)
					{
						DisRate = Discount / ItemAmount * 100m;
						r.Cells["DiscRate"].Value = Math.Round(DisRate, 2);
					}
					else
					{
						DisRate = default(decimal);
						r.Cells["DiscRate"].Value = 0;
					}
				}
				else
				{
					DisRate = default(decimal);
					r.Cells["DiscRate"].Value = 0;
				}
				if (ItemAmount > 0m && Discount > 0m)
				{
					r.Cells["TotalAmount"].Value = ItemAmount - Discount;
				}
				else if (ItemAmount > 0m)
				{
					r.Cells["TotalAmount"].Value = ItemAmount;
				}
				else
				{
					r.Cells["TotalAmount"].Value = 0;
				}
				r.EndEdit();
			}
			TotalCommissionAmount();
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdjustDiscamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtManualTotalOrderAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtManualTotalOrderAmount.Text) > 0.0)
			{
				CalculateWithManualOrderAmountTotal();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtManualTotalOrderAmount_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWithManualOrderAmountTotal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWithManualOrderAmountTotal()
	{
		try
		{
			decimal TotalManualOrderAmount = Conversion.ToDecimal((object)txtManualTotalOrderAmount.Text);
			decimal FreightFooter = Conversion.ToDecimal((object)txtFrieghtFooter.Text);
			decimal WagesFooter = default(decimal);
			decimal OrderAmount = default(decimal);
			decimal AdjustDiscAmount = Conversion.ToDecimal((object)txtAdjustDiscamount.Text);
			decimal OtherWagesFooter = Conversion.ToDecimal((object)txtOtherWagesFooter.Text);
			if (grd.GetRows().Count() > 0)
			{
				decimal num = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
				if (grdContractorWages.GetRows().Count() > 0)
				{
					WagesFooter = Conversion.ToDecimal(grdContractorWages.GetTotal(grdContractorWages.RootTable.Columns["Amount"], (AggregateFunction)2));
				}
				OrderAmount = num - AdjustDiscAmount + OtherWagesFooter + FreightFooter + WagesFooter;
				if (TotalManualOrderAmount == 0m)
				{
					txtDiscountFooter.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtManualTotalOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else if (TotalManualOrderAmount > OrderAmount)
				{
					MessageBox.Show($"Manual Order Amount {TotalManualOrderAmount} Can't be Greater Than Expected Order Amount {OrderAmount}");
					txtDiscountFooter.Text = 0.ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
					txtManualTotalOrderAmount.Text = OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtDiscountFooter.Text = (OrderAmount - TotalManualOrderAmount).ToString(clsGlobalVariables.stringFormatsingle);
					txtOrderAmount.Text = TotalManualOrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				PropotionateDiscountByManualOrderAmountInGrid();
				txtItemNetAmountFooter.Text = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["TotalAmount"], (AggregateFunction)2)).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PropotionateDiscountByManualOrderAmountInGrid()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			decimal FooterDisAmt = Conversion.ToDecimal((object)txtDiscountFooter.Text);
			decimal totalGridAmount = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			decimal totalCalculatedDiscount = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				decimal RowItemAmount2 = Conversion.ToDecimal((object)r2.Cells["Amount"].Value.ToString());
				decimal RowDiscountAmount = ((totalGridAmount > 0m) ? (RowItemAmount2 / totalGridAmount * 100m) : 0m) * FooterDisAmt / 100m;
				decimal DisRate2 = default(decimal);
				r2.BeginEdit();
				r2.Cells["DiscAmount"].Value = Math.Round(RowDiscountAmount, 2);
				r2.Cells["TotalAmount"].Value = RowItemAmount2 - RowDiscountAmount;
				if (Conversion.ToInt(r2.Cells["DiscountId"].Value) == 0)
				{
					r2.Cells["DiscountId"].Value = 2;
					r2.Cells["DiscountType"].Value = "Percent";
				}
				if (Conversion.ToInt(r2.Cells["DiscountId"].Value) == 1)
				{
					DisRate2 = RowDiscountAmount;
					r2.Cells["DisRate"].Value = Math.Round(DisRate2, 2);
				}
				else if (Conversion.ToInt(r2.Cells["DiscountId"].Value) == 2)
				{
					if (FooterDisAmt > 0m && RowDiscountAmount > 0m)
					{
						DisRate2 = ((RowItemAmount2 > 0m) ? (RowDiscountAmount / RowItemAmount2 * 100m) : 0m);
						r2.Cells["DiscRate"].Value = Math.Round(DisRate2, 2);
					}
					else
					{
						DisRate2 = default(decimal);
						r2.Cells["DiscRate"].Value = 0;
					}
				}
				else
				{
					DisRate2 = default(decimal);
					r2.Cells["DiscRate"].Value = 0;
				}
				if (RowItemAmount2 > 0m && RowDiscountAmount > 0m)
				{
					r2.Cells["TotalAmount"].Value = RowItemAmount2 - RowDiscountAmount;
				}
				else if (RowItemAmount2 > 0m)
				{
					r2.Cells["TotalAmount"].Value = RowItemAmount2;
				}
				else
				{
					r2.Cells["TotalAmount"].Value = 0;
				}
				r2.EndEdit();
				totalCalculatedDiscount += RowDiscountAmount;
			}
			if (!(Math.Abs(totalCalculatedDiscount - FooterDisAmt) > 0.01m))
			{
				return;
			}
			decimal adjustmentFactor = FooterDisAmt / totalCalculatedDiscount;
			rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				decimal RowItemAmount = Conversion.ToDecimal((object)r.Cells["Amount"].Value.ToString());
				decimal UpdatedDiscountAmount = Math.Round(Conversion.ToDecimal((object)r.Cells["DiscAmount"].Value.ToString()) * adjustmentFactor, 2);
				r.BeginEdit();
				r.Cells["DiscAmount"].Value = UpdatedDiscountAmount;
				r.Cells["TotalAmount"].Value = RowItemAmount - UpdatedDiscountAmount;
				decimal DisRate = default(decimal);
				if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 1)
				{
					DisRate = UpdatedDiscountAmount;
					r.Cells["DisRate"].Value = Math.Round(DisRate, 2);
				}
				else if (Conversion.ToInt(r.Cells["DiscountId"].Value) == 2)
				{
					if (RowItemAmount > 0m && UpdatedDiscountAmount > 0m)
					{
						DisRate = ((RowItemAmount > 0m) ? (UpdatedDiscountAmount / RowItemAmount * 100m) : 0m);
						r.Cells["DiscRate"].Value = Math.Round(DisRate, 2);
					}
					else
					{
						DisRate = default(decimal);
						r.Cells["DiscRate"].Value = 0;
					}
				}
				else
				{
					DisRate = default(decimal);
					r.Cells["DiscRate"].Value = 0;
				}
				if (RowItemAmount > 0m && UpdatedDiscountAmount > 0m)
				{
					r.Cells["TotalAmount"].Value = RowItemAmount - UpdatedDiscountAmount;
				}
				else if (RowItemAmount > 0m)
				{
					r.Cells["TotalAmount"].Value = RowItemAmount;
				}
				else
				{
					r.Cells["TotalAmount"].Value = 0;
				}
				r.EndEdit();
			}
			TotalCommissionAmount();
			CalculateTotalInformation();
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
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
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
			DueDateGenerate();
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
			fromdate.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void GetDataFromSaleOrdersForComboBind()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			dtCustomerHistory.Rows.Clear();
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtItem = new DataTable
			{
				Columns = { "Id", "ItemName" }
			};
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(val);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
				{
					dtCustomerHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"], dt.Rows[i]["OtherDescription"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtCustomerHistory.Rows.Count > 0)
			{
				if (RdCustomerHistorySearchByName.Checked)
				{
					DDL.BindDDL(dtCustomerHistory, CmbSupplierHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtCustomerHistory, CmbSupplierHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
			}
			else
			{
				((UltraGridBase)CmbSupplierHistory).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, CmbItemHistory, "Id", "ItemName", "Item Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbItemHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "UnApproved");
			dt.Rows.Add(2, "Approve");
			dt.Rows.Add(3, "All");
			DDL.BindDDL(dt, CmbIsApprovedHistory, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbIsApprovedHistory).Rows[0].Activate();
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
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Cancel");
			dt.Rows.Add(3, "Complete");
			DDL.BindDDL(dt, cmbApproveStatusHistory, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbApproveStatusHistory).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			GetDataFromSaleOrdersForComboBind();
			StatusFill();
			ApproveStatusFill();
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

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.BranchesId = UserAccount.BranchesId;
			reports.DocumentTypeId = 1852;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.FromDate = fromdate.Value;
			reports.ToDate = ToDate.Value;
			reports.SupplierCustomerId = Conversion.ToInt(CmbSupplierHistory.Value);
			reports.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			reports.Status = Conversion.ToString((object)((Control)(object)cmbApproveStatusHistory).Text);
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (((Control)(object)CmbIsApprovedHistory).Text == "Approve")
			{
				reports.IsApproved = true;
			}
			if (((Control)(object)CmbIsApprovedHistory).Text == "UnApproved")
			{
				reports.IsApproved = false;
			}
			if (((Control)(object)CmbIsApprovedHistory).Text == "All")
			{
				reports.ApprovedFilter = "All";
			}
			DataTable dthistoryfill = new DataTable();
			dthistoryfill = SaleOrder.FormHistory(reports);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtOrders = new DataTable();
				dtOrders.Columns.Add("Id", typeof(int));
				dtOrders.Columns.Add("DocumentTypeId", typeof(int));
				dtOrders.Columns.Add("DocNo", typeof(int));
				dtOrders.Columns.Add("DocDate", typeof(DateTime));
				dtOrders.Columns.Add("SupplierCustomerId", typeof(int));
				dtOrders.Columns.Add("CustomerName", typeof(string));
				dtOrders.Columns.Add("CustomerCode", typeof(string));
				dtOrders.Columns.Add("RefrenenceNo", typeof(string));
				dtOrders.Columns.Add("OrderStatus", typeof(string));
				dtOrders.Columns.Add("RemarksHeader", typeof(string));
				dtOrders.Columns.Add("VisitedByName", typeof(string));
				dtOrders.Columns.Add("ReferencePartyName", typeof(string));
				dtOrders.Columns.Add("ReferencePartyCellNo", typeof(string));
				dtOrders.Columns.Add("ReferencePartyAddress", typeof(string));
				dtOrders.Columns.Add("CommissionAgent", typeof(string));
				dtOrders.Columns.Add("CommissionRate", typeof(decimal));
				dtOrders.Columns.Add("CommissionAmount", typeof(decimal));
				dtOrders.Columns.Add("PaymentTerm", typeof(string));
				dtOrders.Columns.Add("OrderDueDays", typeof(int));
				dtOrders.Columns.Add("OrderDueDate", typeof(DateTime));
				dtOrders.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dtOrders.Columns.Add("DeliveryTerm", typeof(string));
				dtOrders.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dtOrders.Columns.Add("DeliveryDays", typeof(int));
				dtOrders.Columns.Add("EntryDate", typeof(DateTime));
				dtOrders.Columns.Add("EntryUserName", typeof(string));
				dtOrders.Columns.Add("ModifyDate", typeof(DateTime));
				dtOrders.Columns.Add("ModifyUserName", typeof(string));
				dtOrders.Columns.Add("OrderQty", typeof(decimal));
				dtOrders.Columns.Add("OrderWeight", typeof(decimal));
				dtOrders.Columns.Add("OrderItemAmount", typeof(decimal));
				dtOrders.Columns.Add("OrderDiscountAmount", typeof(decimal));
				dtOrders.Columns.Add("OrderItemNetAmount", typeof(decimal));
				dtOrders.Columns.Add("FrieghtAmount", typeof(decimal));
				dtOrders.Columns.Add("WageNetAmount", typeof(decimal));
				dtOrders.Columns.Add("OtherWagesAmount", typeof(decimal));
				dtOrders.Columns.Add("OrderAmount", typeof(decimal));
				dtOrders.Columns.Add("NoOfAttachments", typeof(int));
				dtOrders.Columns.Add("Distance", typeof(string));
				dtOrders.Rows.Clear();
				foreach (DataRow row in dthistoryfill.Rows)
				{
					dtOrders.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["SupplierCustomerId"], row["SupplierName"], row["PartyCode"], row["RefrenenceNo"], row["OrderStatus"], row["RemarksHeader"], row["VisitedByName"], row["ReferencPartyName"], row["ReferencPartyCellNo"], row["ReferencPartyAddress"], row["CommissionAgent"], row["CommissionRate"], row["CommissionAmount"], row["PaymentTerm"], row["OrderDueDays"], row["OrderDueDate"], row["OrderExpiryDate"], row["DeliveryTerm"], row["DeliveryStartDate"], row["DeliveryDays"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["OrderQty"], row["OrderWeight"], row["OrderItemAmount"], row["OrderDiscountAmount"], row["OrderItemNetAmount"], row["FrieghtAmount"], row["WageNetAmount"], row["OtherWages"], row["OrderAmount"], row["NoOfAttachments"], row["Distance"]);
				}
				grdhistory.DataSource = dtOrders;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				DataGridHistoryDetail.ClearStructure();
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
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdhistory.RootTable.Columns["OrderItemAmount"].Visible = false;
			grdhistory.RootTable.Columns["OrderDiscountAmount"].Visible = false;
			grdhistory.RootTable.Columns["CommissionRate"].Caption = "Comm Rate";
			grdhistory.RootTable.Columns["CommissionAmount"].Caption = "Comm Amount";
			grdhistory.RootTable.Columns["OrderItemNetAmount"].Caption = "Item Net Amount";
			grdhistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderQty"].FormatString = "##,#.##";
			grdhistory.RootTable.Columns["OrderQty"].TotalFormatString = "##,#.##";
			grdhistory.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderWeight"].FormatString = "##,#.##";
			grdhistory.RootTable.Columns["OrderWeight"].TotalFormatString = "##,#.##";
			grdhistory.RootTable.Columns["CommissionRate"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["CommissionRate"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommissionRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["CommissionRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdhistory.RootTable.Columns["CommissionAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["CommissionAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["CommissionAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["CommissionAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderItemAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderDiscountAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderDiscountAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderDiscountAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderDiscountAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["FrieghtAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FrieghtAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FrieghtAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["FrieghtAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["WageNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["WageNetAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["WageNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["WageNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OtherWagesAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OtherWagesAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OtherWagesAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OtherWagesAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns["CustomerName"].Width = 150;
			grdhistory.RootTable.Columns["ReferencePartyName"].Width = 150;
			grdhistory.RootTable.Columns["ReferencePartyAddress"].Width = 200;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Width = 80;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["SaveAs"].Position = 1;
			grdhistory.RootTable.Columns["Print"].Position = 2;
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.SaleOrderSlipConcrete(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				Reset();
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
				btnsave.Visible = false;
				btnupdate.Visible = false;
				btnSaveAs.Visible = true;
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
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), 1852);
			}
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
			GridDetailBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			if (grdhistory.CurrentRow == null || (int)grdhistory.CurrentRow.RowType != 0 || grdhistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			SaleOrder po = new SaleOrder();
			po = SaleOrder.GetByID(Id);
			if (po != null)
			{
				DataTable dtdetailhistory = new DataTable();
				dtdetailhistory.Columns.Add("ItemCode");
				dtdetailhistory.Columns.Add("Item");
				dtdetailhistory.Columns.Add("AttributeVarient");
				dtdetailhistory.Columns.Add("VarientUnit");
				dtdetailhistory.Columns.Add("QTY", typeof(decimal));
				dtdetailhistory.Columns.Add("Weight", typeof(decimal));
				dtdetailhistory.Columns.Add("NetWeight", typeof(decimal));
				dtdetailhistory.Columns.Add("ItemPrice");
				dtdetailhistory.Columns.Add("AddLessRate");
				dtdetailhistory.Columns.Add("NetRate");
				dtdetailhistory.Columns.Add("Amount");
				dtdetailhistory.Columns.Add("Discount");
				dtdetailhistory.Columns.Add("DiscRate");
				dtdetailhistory.Columns.Add("DiscAmount");
				dtdetailhistory.Columns.Add("TotalAmount", typeof(decimal));
				dtdetailhistory.Columns.Add("FcyAmount", typeof(decimal));
				dtdetailhistory.Columns.Add("JobLot");
				dtdetailhistory.Columns.Add("City");
				dtdetailhistory.Columns.Add("Remarks");
				dtdetailhistory.Columns.Add("CommOnSale");
				for (int i = 0; i < po.SaleOrderDetailList.Count; i++)
				{
					dtdetailhistory.Rows.Add(po.SaleOrderDetailList[i].ItemCode, po.SaleOrderDetailList[i].ItemName, po.SaleOrderDetailList[i].ItemAttributeVarient, po.SaleOrderDetailList[i].VarientEquivalent, po.SaleOrderDetailList[i].OrderItemQty, po.SaleOrderDetailList[i].ItemWeight, po.SaleOrderDetailList[i].ItemNetWeight, po.SaleOrderDetailList[i].ItemRateWithOutAddLess, po.SaleOrderDetailList[i].RateAddLess, po.SaleOrderDetailList[i].OrderItemRate, po.SaleOrderDetailList[i].OrderItemAmount, po.SaleOrderDetailList[i].ItemDiscountType, po.SaleOrderDetailList[i].ItemDiscountRate, po.SaleOrderDetailList[i].ItemDiscountAmount, po.SaleOrderDetailList[i].ItemNetAmount, po.SaleOrderDetailList[i].FcyAmount, po.SaleOrderDetailList[i].JobLotDescription, po.SaleOrderDetailList[i].CityName, po.SaleOrderDetailList[i].RemarksDetail, po.SaleOrderDetailList[i].CommOnSale);
				}
				DataGridHistoryDetail.DataSource = dtdetailhistory;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		try
		{
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			DataGridHistoryDetail.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["QTY"].FormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["QTY"].TotalFormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistoryDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataGridHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			DataGridHistoryDetail.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ItemPrice"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["AddLessRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["AddLessRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["NetRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistoryDetail.RootTable.Columns["NetRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["NetRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["NetRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["DiscRate"].AggregateFunction = (AggregateFunction)3;
			DataGridHistoryDetail.RootTable.Columns["DiscRate"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["DiscRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["DiscRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			DataGridHistoryDetail.RootTable.Columns["DiscAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["DiscAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["DiscAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["DiscAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(DataGridHistoryDetail);
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
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefinePlant_Click(object sender, EventArgs e)
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

	private void BtnDefineCity_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineBuildingStoreys_Click(object sender, EventArgs e)
	{
		try
		{
			InvLookup invLookup = new InvLookup(UserAccount);
			invLookup.Show();
			invLookup.cmbProfileName.Value = 2;
			((Control)(object)invLookup.cmbProfileName).Enabled = false;
			invLookup.GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineBuildingHeights_Click(object sender, EventArgs e)
	{
		try
		{
			InvLookup invLookup = new InvLookup(UserAccount);
			invLookup.Show();
			invLookup.cmbProfileName.Value = 1;
			((Control)(object)invLookup.cmbProfileName).Enabled = false;
			invLookup.GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineRefParties_Click(object sender, EventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFrieghtFooter_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherWagesFooter_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLessRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAdjustDiscamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtManualTotalOrderAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (grd.RowCount > 0)
			{
				CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
			}
			else
			{
				e.Handled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					fromdate.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnSaveAs.Visible && btnSaveAs.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formright.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnprint.Visible && btnprint.Enabled)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					combitem.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdContractorWages).Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistoryDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdhistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right && ((Control)(object)grdhistory).Focused)
				{
					((Control)(object)grdContractorWages).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For Save As");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Item Combo");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdContractorWages.CurrentColumn.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtWagesDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtWagesDetail.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
				CalculateTotalInformation();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtWagesDetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
				CalculateTotalInformation();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ItemId" || grdContractorWages.CurrentColumn.Key == "ItemName"))
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(dtItemForWages, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = Name;
				if (Id != PreviousItemId)
				{
					r.Cells["ContractorWagesRateScheduleId"].Value = 0;
					r.Cells["ServiceActivity"].Value = 0;
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
			{
				GrdPopUp obj2 = new GrdPopUp(dtContr, "Id", "CompanyName");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["ContractorId"].Value = Id;
				r.Cells["ContractorName"].Value = Name;
			}
			if (grdContractorWages.CurrentColumn != null && grdContractorWages.CurrentColumn.Key == "ServiceActivity")
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, 0);
				GrdPopUp obj = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				r.Cells["ContractorWagesRateScheduleId"].Value = Id;
				r.Cells["ServiceActivity"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
					decimal Qty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Qty"]) : 0m);
					decimal RatePerQty = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m) / Qty;
					r.Cells["Rate"].Value = RatePerQty;
					decimal CurrRowQty = Conversion.ToInt(r.Cells["Qty"].Value);
					decimal Amount = RatePerQty * CurrRowQty;
					r.Cells["Amount"].Value = Amount;
				}
				else
				{
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			grdContractorWages.UpdateData();
			CalculateTotalInformation();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						SaleOrderDetail vd2 = new SaleOrderDetail();
						vd2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd2.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd2.ScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
						vd2.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
						vd2.VarientEquivalent = Conversion.ToInt(r.Cells["VarientUnit"].Value);
						vd2.ItemWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
						vd2.OrderItemQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
						vd2.ItemRateWithOutAddLess = Conversion.ToDecimal(r.Cells["ItemPrice"].Value);
						vd2.RateAddLess = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
						vd2.OrderItemRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
						vd2.OrderItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
						vd2.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountId"].Value);
						vd2.ItemDiscountRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
						vd2.ItemDiscountAmount = Conversion.ToDecimal(r.Cells["DiscAmount"].Value);
						vd2.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
						vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd2.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
						vd2.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						vd2.ActionTypeId = 3;
						lstRemoveRecord.Add(vd2);
						dtdtail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grd.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					TotalCommissionAmount();
					txtExchangeRate_TextChanged(null, null);
					WagesItemFillFromDetail();
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					vd.VarientEquivalent = Conversion.ToInt(r.Cells["VarientUnit"].Value);
					vd.ItemWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
					vd.OrderItemQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
					vd.ItemRateWithOutAddLess = Conversion.ToDecimal(r.Cells["ItemPrice"].Value);
					vd.RateAddLess = Conversion.ToDecimal(r.Cells["AddLessRate"].Value);
					vd.OrderItemRate = Conversion.ToDecimal(r.Cells["NetRate"].Value);
					vd.OrderItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					vd.ItemDiscountTypeId = Conversion.ToInt(r.Cells["DiscountId"].Value);
					vd.ItemDiscountRate = Conversion.ToDecimal(r.Cells["DiscRate"].Value);
					vd.ItemDiscountAmount = Conversion.ToDecimal(r.Cells["DiscAmount"].Value);
					vd.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
					vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdtail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				TotalCommissionAmount();
				txtExchangeRate_TextChanged(null, null);
				WagesItemFillFromDetail();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)Id));
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.SaleOrderSlipConcrete(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1852);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)Id));
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.SaleOrderSlipConcrete(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
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
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bb: Expected O, but got Unknown
		//IL_07bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Expected O, but got Unknown
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Expected O, but got Unknown
		//IL_091c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Expected O, but got Unknown
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a91: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Expected O, but got Unknown
		//IL_0cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccd: Expected O, but got Unknown
		//IL_0d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Expected O, but got Unknown
		//IL_0d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Expected O, but got Unknown
		//IL_0d47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Expected O, but got Unknown
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5c: Expected O, but got Unknown
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Expected O, but got Unknown
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efe: Expected O, but got Unknown
		//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f14: Expected O, but got Unknown
		//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Expected O, but got Unknown
		//IL_0f4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f56: Expected O, but got Unknown
		//IL_0f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa3: Expected O, but got Unknown
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Expected O, but got Unknown
		//IL_2ecd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed7: Expected O, but got Unknown
		//IL_2ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eee: Expected O, but got Unknown
		//IL_3232: Unknown result type (might be due to invalid IL or missing references)
		//IL_323c: Expected O, but got Unknown
		//IL_3249: Unknown result type (might be due to invalid IL or missing references)
		//IL_3253: Expected O, but got Unknown
		//IL_c0b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_c0c0: Expected O, but got Unknown
		//IL_c0e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_c0ee: Expected O, but got Unknown
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
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
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
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
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
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
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance234 = new Appearance();
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
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.CmbOtherWagesAccount = new UltraCombo();
		this.CmbFrieghtAccount = new UltraCombo();
		this.txtOrderAmount = new System.Windows.Forms.TextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtManualTotalOrderAmount = new System.Windows.Forms.TextBox();
		this.txtAdjustDiscamount = new System.Windows.Forms.TextBox();
		this.txtDiscountFooter = new System.Windows.Forms.TextBox();
		this.txtItemAmountFooter = new System.Windows.Forms.TextBox();
		this.txtOtherWagesFooter = new System.Windows.Forms.TextBox();
		this.txtItemNetAmountFooter = new System.Windows.Forms.TextBox();
		this.txtFrieghtFooter = new System.Windows.Forms.TextBox();
		this.txtTotalWagesFooter = new System.Windows.Forms.TextBox();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdContractorWages = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label58 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.TabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.btnUpdateDiscountInGrid = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtAddLessRate = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.txtItemPrice = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.BtnDefineCity = new System.Windows.Forms.Button();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.btnplus = new System.Windows.Forms.Button();
		this.CmbDiscountType = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.txtDiscountRate = new System.Windows.Forms.TextBox();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.chkCommissionOnSale = new System.Windows.Forms.CheckBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.combcityarea = new UltraCombo();
		this.CmbAttributeVarient = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.combjob = new UltraCombo();
		this.combitem = new UltraCombo();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.combsalesman = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.combcommtype = new UltraCombo();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.txtCommrate = new System.Windows.Forms.TextBox();
		this.CmbRefSalesMan = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.combdeliverytrm = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.RdPartyByCode = new System.Windows.Forms.RadioButton();
		this.RdPartyByName = new System.Windows.Forms.RadioButton();
		this.BtnDefineBuildingHeights = new System.Windows.Forms.Button();
		this.BtnDefineBuildingStoreys = new System.Windows.Forms.Button();
		this.BtnDefineRefParties = new System.Windows.Forms.Button();
		this.btnDefinePlant = new System.Windows.Forms.Button();
		this.label48 = new System.Windows.Forms.Label();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.cmbCurrency = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.Cmbheight = new UltraCombo();
		this.txtArea = new System.Windows.Forms.TextBox();
		this.txtDistance = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.Cmbstoreys = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbReferenceParty = new UltraCombo();
		this.CmbVisitedbyName = new UltraCombo();
		this.combsuppname = new UltraCombo();
		this.label70 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtRefPartyCellNo = new System.Windows.Forms.TextBox();
		this.txtRefPartyAddress = new System.Windows.Forms.TextBox();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistoryDetail = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.RdCustomerHistorySearchByCode = new System.Windows.Forms.RadioButton();
		this.RdCustomerHistorySearchByName = new System.Windows.Forms.RadioButton();
		this.cmbApproveStatusHistory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbIsApprovedHistory = new UltraCombo();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbItemHistory = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbSupplierHistory = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.PanelMessage = new System.Windows.Forms.Panel();
		this.ToolStripMessage = new System.Windows.Forms.ToolStrip();
		this.BtnMessage = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFrieghtAccount).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.TabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefSalesMan).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbheight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbstoreys).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVisitedbyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApprovedHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.PanelMessage.SuspendLayout();
		this.ToolStripMessage.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1334, 787);
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
		this.tabControl1.Size = new System.Drawing.Size(1334, 787);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel11);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1326, 758);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add(this.PanelMessage);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount);
		this.panel11.Controls.Add(this.txtOrderAmount);
		this.panel11.Controls.Add(this.label71);
		this.panel11.Controls.Add(this.label67);
		this.panel11.Controls.Add(this.label63);
		this.panel11.Controls.Add(this.label39);
		this.panel11.Controls.Add(this.label65);
		this.panel11.Controls.Add(this.label46);
		this.panel11.Controls.Add(this.label36);
		this.panel11.Controls.Add(this.label32);
		this.panel11.Controls.Add(this.label68);
		this.panel11.Controls.Add(this.label64);
		this.panel11.Controls.Add(this.label62);
		this.panel11.Controls.Add(this.label42);
		this.panel11.Controls.Add(this.label66);
		this.panel11.Controls.Add(this.label61);
		this.panel11.Controls.Add(this.label59);
		this.panel11.Controls.Add(this.label22);
		this.panel11.Controls.Add(this.txtManualTotalOrderAmount);
		this.panel11.Controls.Add(this.txtAdjustDiscamount);
		this.panel11.Controls.Add(this.txtDiscountFooter);
		this.panel11.Controls.Add(this.txtItemAmountFooter);
		this.panel11.Controls.Add(this.txtOtherWagesFooter);
		this.panel11.Controls.Add(this.txtItemNetAmountFooter);
		this.panel11.Controls.Add(this.txtFrieghtFooter);
		this.panel11.Controls.Add(this.txtTotalWagesFooter);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(747, 592);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(577, 164);
		this.panel11.TabIndex = 0;
		this.CmbOtherWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOtherWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Location = new System.Drawing.Point(135, 75);
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Name = "CmbOtherWagesAccount";
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherWagesAccount).TabIndex = 6;
		this.CmbFrieghtAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbFrieghtAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFrieghtAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFrieghtAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFrieghtAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFrieghtAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount).Location = new System.Drawing.Point(135, 46);
		((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount).Name = "CmbFrieghtAccount";
		((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.CmbFrieghtAccount).TabIndex = 4;
		this.txtOrderAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderAmount.Enabled = false;
		this.txtOrderAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderAmount.Location = new System.Drawing.Point(415, 131);
		this.txtOrderAmount.Name = "txtOrderAmount";
		this.txtOrderAmount.Size = new System.Drawing.Size(143, 23);
		this.txtOrderAmount.TabIndex = 9;
		this.txtOrderAmount.TabStop = false;
		this.txtOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label71.BackColor = System.Drawing.Color.Gainsboro;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(3, 110);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(229, 19);
		this.label71.TabIndex = 0;
		this.label71.Text = "Add Total Order Amount Manually";
		this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(282, 4);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(92, 15);
		this.label67.TabIndex = 0;
		this.label67.Text = "Adjust Discount";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(149, 4);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(55, 15);
		this.label63.TabIndex = 0;
		this.label63.Text = "Discount";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(392, 133);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(21, 18);
		this.label39.TabIndex = 0;
		this.label39.Text = "=";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(392, 24);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(21, 18);
		this.label65.TabIndex = 0;
		this.label65.Text = "=";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(392, 79);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(21, 18);
		this.label46.TabIndex = 0;
		this.label46.Text = "+";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(392, 107);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(21, 18);
		this.label36.TabIndex = 0;
		this.label36.Text = "+";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(392, 50);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(21, 18);
		this.label32.TabIndex = 0;
		this.label32.Text = "+";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(244, 24);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(15, 18);
		this.label68.TabIndex = 0;
		this.label68.Text = "-";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(117, 24);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(15, 18);
		this.label64.TabIndex = 0;
		this.label64.Text = "-";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(7, 4);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(79, 15);
		this.label62.TabIndex = 0;
		this.label62.Text = "Item Amount";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(6, 81);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(131, 15);
		this.label42.TabIndex = 0;
		this.label42.Text = "Other Wages Ac && Amt";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(415, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(101, 15);
		this.label66.TabIndex = 0;
		this.label66.Text = "Net Item Amount";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(6, 52);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(123, 15);
		this.label61.TabIndex = 0;
		this.label61.Text = "Frieght Ac &&  Amount";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(271, 109);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(118, 15);
		this.label59.TabIndex = 0;
		this.label59.Text = "Total Wages Expense";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(271, 135);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(113, 15);
		this.label22.TabIndex = 37;
		this.label22.Text = "Total Order Amount";
		this.txtManualTotalOrderAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualTotalOrderAmount.Font = new System.Drawing.Font("Verdana", 10.5f);
		this.txtManualTotalOrderAmount.Location = new System.Drawing.Point(3, 131);
		this.txtManualTotalOrderAmount.Name = "txtManualTotalOrderAmount";
		this.txtManualTotalOrderAmount.Size = new System.Drawing.Size(244, 25);
		this.txtManualTotalOrderAmount.TabIndex = 1;
		this.txtManualTotalOrderAmount.TabStop = false;
		this.txtManualTotalOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtManualTotalOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtManualTotalOrderAmount.TextChanged += new System.EventHandler(txtManualTotalOrderAmount_TextChanged);
		this.txtManualTotalOrderAmount.Leave += new System.EventHandler(txtManualTotalOrderAmount_Leave);
		this.txtManualTotalOrderAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtManualTotalOrderAmount_KeyPress);
		this.txtAdjustDiscamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdjustDiscamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAdjustDiscamount.Location = new System.Drawing.Point(265, 22);
		this.txtAdjustDiscamount.Name = "txtAdjustDiscamount";
		this.txtAdjustDiscamount.Size = new System.Drawing.Size(124, 23);
		this.txtAdjustDiscamount.TabIndex = 1;
		this.txtAdjustDiscamount.TabStop = false;
		this.txtAdjustDiscamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdjustDiscamount.TextChanged += new System.EventHandler(txtAdjustDiscamount_TextChanged);
		this.txtAdjustDiscamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAdjustDiscamount_KeyPress);
		this.txtDiscountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountFooter.Enabled = false;
		this.txtDiscountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountFooter.Location = new System.Drawing.Point(135, 22);
		this.txtDiscountFooter.Name = "txtDiscountFooter";
		this.txtDiscountFooter.Size = new System.Drawing.Size(107, 23);
		this.txtDiscountFooter.TabIndex = 1;
		this.txtDiscountFooter.TabStop = false;
		this.txtDiscountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmountFooter.Enabled = false;
		this.txtItemAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmountFooter.Location = new System.Drawing.Point(8, 22);
		this.txtItemAmountFooter.Name = "txtItemAmountFooter";
		this.txtItemAmountFooter.Size = new System.Drawing.Size(107, 23);
		this.txtItemAmountFooter.TabIndex = 0;
		this.txtItemAmountFooter.TabStop = false;
		this.txtItemAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWagesFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherWagesFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherWagesFooter.Location = new System.Drawing.Point(415, 77);
		this.txtOtherWagesFooter.Name = "txtOtherWagesFooter";
		this.txtOtherWagesFooter.Size = new System.Drawing.Size(143, 23);
		this.txtOtherWagesFooter.TabIndex = 7;
		this.txtOtherWagesFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWagesFooter.TextChanged += new System.EventHandler(txtOtherWagesFooter_TextChanged);
		this.txtOtherWagesFooter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherWagesFooter_KeyPress);
		this.txtItemNetAmountFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemNetAmountFooter.Enabled = false;
		this.txtItemNetAmountFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemNetAmountFooter.Location = new System.Drawing.Point(415, 22);
		this.txtItemNetAmountFooter.Name = "txtItemNetAmountFooter";
		this.txtItemNetAmountFooter.Size = new System.Drawing.Size(143, 23);
		this.txtItemNetAmountFooter.TabIndex = 3;
		this.txtItemNetAmountFooter.TabStop = false;
		this.txtItemNetAmountFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFrieghtFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFrieghtFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFrieghtFooter.Location = new System.Drawing.Point(415, 48);
		this.txtFrieghtFooter.Name = "txtFrieghtFooter";
		this.txtFrieghtFooter.Size = new System.Drawing.Size(143, 23);
		this.txtFrieghtFooter.TabIndex = 5;
		this.txtFrieghtFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFrieghtFooter.TextChanged += new System.EventHandler(txtFrieghtFooter_TextChanged);
		this.txtFrieghtFooter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFrieghtFooter_KeyPress);
		this.txtTotalWagesFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalWagesFooter.Enabled = false;
		this.txtTotalWagesFooter.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalWagesFooter.Location = new System.Drawing.Point(415, 105);
		this.txtTotalWagesFooter.Name = "txtTotalWagesFooter";
		this.txtTotalWagesFooter.Size = new System.Drawing.Size(143, 23);
		this.txtTotalWagesFooter.TabIndex = 8;
		this.txtTotalWagesFooter.TabStop = false;
		this.txtTotalWagesFooter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.panel14.Controls.Add(this.panel6);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(2, 592);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(745, 164);
		this.panel14.TabIndex = 7;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(0, 20);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(745, 144);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 0;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label58);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(745, 20);
		this.panel6.TabIndex = 16;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(4, 2);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(130, 16);
		this.label58.TabIndex = 126;
		this.label58.Text = "Contractor Wages ";
		this.tabControl2.Controls.Add(this.TabDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(2, 360);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1322, 232);
		this.tabControl2.TabIndex = 0;
		this.TabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.TabDetail.Controls.Add(this.panel10);
		this.TabDetail.Location = new System.Drawing.Point(4, 23);
		this.TabDetail.Name = "TabDetail";
		this.TabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.TabDetail.Size = new System.Drawing.Size(1314, 205);
		this.TabDetail.TabIndex = 0;
		this.TabDetail.Text = "Detail";
		this.TabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1308, 177);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label1);
		this.panel10.Controls.Add(this.btnUpdateDiscountInGrid);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1308, 22);
		this.panel10.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(305, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(660, 14);
		this.label1.TabIndex = 37;
		this.label1.Text = "Click the Button To Update Same Discount Type as in Combo and Discount Rate as In text box In All rows";
		this.btnUpdateDiscountInGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnUpdateDiscountInGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDiscountInGrid.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDiscountInGrid.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDiscountInGrid.Location = new System.Drawing.Point(972, -1);
		this.btnUpdateDiscountInGrid.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDiscountInGrid.Name = "btnUpdateDiscountInGrid";
		this.btnUpdateDiscountInGrid.Size = new System.Drawing.Size(163, 23);
		this.btnUpdateDiscountInGrid.TabIndex = 36;
		this.btnUpdateDiscountInGrid.Text = "Update Same Discount";
		this.btnUpdateDiscountInGrid.UseVisualStyleBackColor = false;
		this.btnUpdateDiscountInGrid.Click += new System.EventHandler(btnUpdateDiscountInGrid_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1266, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 22);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 17);
		this.label2.TabIndex = 0;
		this.label2.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 242);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1322, 118);
		this.panel5.TabIndex = 1;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtAddLessRate);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.txtItemPrice);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.BtnDefineCity);
		this.groupBox3.Controls.Add(this.rdbtnItemCode);
		this.groupBox3.Controls.Add(this.rdbtnItemName);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add(this.txtDiscountRate);
		this.groupBox3.Controls.Add(this.txtDiscountAmount);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.txtTotalAmount);
		this.groupBox3.Controls.Add(this.chkCommissionOnSale);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcityarea);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combjob);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtRate);
		this.groupBox3.Controls.Add(this.txtamount);
		this.groupBox3.Controls.Add(this.txtNetWeight);
		this.groupBox3.Controls.Add(this.txtItemWeight);
		this.groupBox3.Controls.Add(this.txtVarientEquivalent);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1300, 114);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.txtAddLessRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLessRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddLessRate.Location = new System.Drawing.Point(810, 37);
		this.txtAddLessRate.Name = "txtAddLessRate";
		this.txtAddLessRate.Size = new System.Drawing.Size(80, 23);
		this.txtAddLessRate.TabIndex = 7;
		this.txtAddLessRate.Tag = "AddLess";
		this.txtAddLessRate.Text = "0";
		this.txtAddLessRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLessRate.TextChanged += new System.EventHandler(txtAddLessRate_TextChanged);
		this.txtAddLessRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLessRate_KeyPress);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(810, 18);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(54, 15);
		this.label31.TabIndex = 131;
		this.label31.Text = "Add Less";
		this.txtItemPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemPrice.Enabled = false;
		this.txtItemPrice.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemPrice.Location = new System.Drawing.Point(726, 37);
		this.txtItemPrice.Name = "txtItemPrice";
		this.txtItemPrice.Size = new System.Drawing.Size(83, 23);
		this.txtItemPrice.TabIndex = 6;
		this.txtItemPrice.Text = "0";
		this.txtItemPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemPrice.TextChanged += new System.EventHandler(txtItemPrice_TextChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(726, 18);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(61, 15);
		this.label29.TabIndex = 129;
		this.label29.Text = "Item Price";
		this.BtnDefineCity.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineCity.Location = new System.Drawing.Point(615, 84);
		this.BtnDefineCity.Name = "BtnDefineCity";
		this.BtnDefineCity.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineCity.TabIndex = 15;
		this.BtnDefineCity.TabStop = false;
		this.BtnDefineCity.UseVisualStyleBackColor = true;
		this.BtnDefineCity.Click += new System.EventHandler(BtnDefineCity_Click);
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(113, 15);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(55, 15);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1145, 84);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 17;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(1105, 35);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Leave += new System.EventHandler(CmbDiscountType_Leave);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(1204, 18);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(56, 15);
		this.label54.TabIndex = 62;
		this.label54.Text = "Disc Rate";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(5, 66);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(77, 15);
		this.label55.TabIndex = 63;
		this.label55.Text = "Disc Amount";
		this.txtDiscountRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountRate.Location = new System.Drawing.Point(1207, 37);
		this.txtDiscountRate.Name = "txtDiscountRate";
		this.txtDiscountRate.Size = new System.Drawing.Size(74, 23);
		this.txtDiscountRate.TabIndex = 11;
		this.txtDiscountRate.Tag = "DiscPercent";
		this.txtDiscountRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountRate.TextChanged += new System.EventHandler(txtDiscountRate_TextChanged);
		this.txtDiscountRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscountRate_KeyPress);
		this.txtDiscountAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountAmount.Location = new System.Drawing.Point(5, 85);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(107, 23);
		this.txtDiscountAmount.TabIndex = 12;
		this.txtDiscountAmount.TabStop = false;
		this.txtDiscountAmount.Tag = "DiscAmount";
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountAmount.TextChanged += new System.EventHandler(txtDiscountAmount_TextChanged);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(1105, 18);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(83, 15);
		this.label56.TabIndex = 61;
		this.label56.Text = "Discount Type";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(111, 66);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(80, 15);
		this.label57.TabIndex = 60;
		this.label57.Text = "Total Amount";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(113, 85);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(124, 23);
		this.txtTotalAmount.TabIndex = 13;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.chkCommissionOnSale.AutoSize = true;
		this.chkCommissionOnSale.Enabled = false;
		this.chkCommissionOnSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnSale.Location = new System.Drawing.Point(1146, 63);
		this.chkCommissionOnSale.Name = "chkCommissionOnSale";
		this.chkCommissionOnSale.Size = new System.Drawing.Size(106, 19);
		this.chkCommissionOnSale.TabIndex = 20;
		this.chkCommissionOnSale.Text = "Comm On Sale";
		this.chkCommissionOnSale.UseVisualStyleBackColor = true;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1215, 84);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 19;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1145, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(69, 26);
		this.btnUpdateDetail.TabIndex = 18;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.combcityarea.AllowNull = (DefaultableBoolean)1;
		this.combcityarea.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcityarea.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcityarea.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcityarea).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combcityarea).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcityarea).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcityarea).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcityarea.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcityarea).Location = new System.Drawing.Point(461, 84);
		((System.Windows.Forms.Control)(object)this.combcityarea).Name = "combcityarea";
		((System.Windows.Forms.Control)(object)this.combcityarea).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.combcityarea).TabIndex = 15;
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance250;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance251).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance251).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance256).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance257).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance257;
		((AppearanceBase)appearance261).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(240, 35);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(combitempck_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(621, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(68, 15);
		this.label41.TabIndex = 8;
		this.label41.Text = "Net Weight";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(545, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(74, 15);
		this.label53.TabIndex = 8;
		this.label53.Text = "Item Weight";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(384, 18);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(69, 15);
		this.label52.TabIndex = 8;
		this.label52.Text = "Varient Unit";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(240, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(94, 15);
		this.label51.TabIndex = 8;
		this.label51.Text = "Attribute Varient";
		this.combjob.AllowNull = (DefaultableBoolean)1;
		this.combjob.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjob.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjob.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjob).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combjob).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjob).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combjob).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjob).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combjob).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjob).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjob).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjob).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjob).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjob).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjob).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combjob).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjob).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjob).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjob).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjob).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combjob.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjob).Location = new System.Drawing.Point(240, 84);
		((System.Windows.Forms.Control)(object)this.combjob).Name = "combjob";
		((System.Windows.Forms.Control)(object)this.combjob).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.combjob).TabIndex = 14;
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 0;
		this.combitem.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(644, 86);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(495, 23);
		this.txtremarksdetail.TabIndex = 16;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(645, 65);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(891, 37);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(90, 23);
		this.txtRate.TabIndex = 8;
		this.txtRate.Tag = "NetRate";
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtamount.BackColor = System.Drawing.Color.White;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Enabled = false;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(982, 37);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(121, 23);
		this.txtamount.TabIndex = 9;
		this.txtamount.TabStop = false;
		this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(621, 37);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(104, 23);
		this.txtNetWeight.TabIndex = 5;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(545, 37);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(74, 23);
		this.txtItemWeight.TabIndex = 4;
		this.txtItemWeight.TabStop = false;
		this.txtItemWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(378, 37);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(82, 23);
		this.txtVarientEquivalent.TabIndex = 2;
		this.txtVarientEquivalent.TabStop = false;
		this.txtVarientEquivalent.TextChanged += new System.EventHandler(txtVarientEquivalent_TextChanged);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(461, 37);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(82, 23);
		this.txtqty.TabIndex = 3;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(891, 18);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(52, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Net Rate";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(459, 65);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(56, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "City/Area";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(241, 65);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Job/Lot";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(982, 18);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(51, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Amount";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(463, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(3, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 51);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1322, 191);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combsalesman);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.txtcommremarks);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combcommtype);
		this.groupBox2.Controls.Add(this.txtcommamount);
		this.groupBox2.Controls.Add(this.txtCommrate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefSalesMan);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1026, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(281, 186);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Commission";
		this.combsalesman.AllowNull = (DefaultableBoolean)1;
		this.combsalesman.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsalesman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsalesman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsalesman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.combsalesman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance194).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance195).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance195;
		((AppearanceBase)appearance196).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsalesman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsalesman).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combsalesman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsalesman).Location = new System.Drawing.Point(55, 14);
		((System.Windows.Forms.Control)(object)this.combsalesman).Name = "combsalesman";
		((System.Windows.Forms.Control)(object)this.combsalesman).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.combsalesman).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combsalesman).Leave += new System.EventHandler(combsalesman_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(3, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(39, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Agent";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(145, 76);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(30, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Rate";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(3, 105);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Amount";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(3, 133);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Remarks";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(3, 76);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(32, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Type";
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommremarks.Location = new System.Drawing.Point(55, 129);
		this.txtcommremarks.Multiline = true;
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(221, 49);
		this.txtcommremarks.TabIndex = 5;
		this.combcommtype.AllowNull = (DefaultableBoolean)1;
		this.combcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommtype).Location = new System.Drawing.Point(55, 70);
		((System.Windows.Forms.Control)(object)this.combcommtype).Name = "combcommtype";
		((System.Windows.Forms.Control)(object)this.combcommtype).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.combcommtype).TabIndex = 2;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(55, 101);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(221, 23);
		this.txtcommamount.TabIndex = 4;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommrate.Location = new System.Drawing.Point(196, 72);
		this.txtCommrate.Name = "txtCommrate";
		this.txtCommrate.Size = new System.Drawing.Size(80, 23);
		this.txtCommrate.TabIndex = 3;
		this.txtCommrate.TextChanged += new System.EventHandler(txtCommrate_TextChanged);
		this.txtCommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommrate_KeyPress);
		this.CmbRefSalesMan.AllowNull = (DefaultableBoolean)1;
		this.CmbRefSalesMan.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefSalesMan.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefSalesMan.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Appearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefSalesMan).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRefSalesMan.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Location = new System.Drawing.Point(55, 42);
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Name = "CmbRefSalesMan";
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefSalesMan).TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(3, 48);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(53, 15);
		this.label50.TabIndex = 0;
		this.label50.Text = "SaleMan";
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.RdPartyByCode);
		this.groupBox1.Controls.Add(this.RdPartyByName);
		this.groupBox1.Controls.Add(this.BtnDefineBuildingHeights);
		this.groupBox1.Controls.Add(this.BtnDefineBuildingStoreys);
		this.groupBox1.Controls.Add(this.BtnDefineRefParties);
		this.groupBox1.Controls.Add(this.btnDefinePlant);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.txtOrderQty);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.txtOrderWeight);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbheight);
		this.groupBox1.Controls.Add(this.txtArea);
		this.groupBox1.Controls.Add(this.txtDistance);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbstoreys);
		this.groupBox1.Controls.Add(this.label60);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceParty);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVisitedbyName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label69);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.txtRefPartyCellNo);
		this.groupBox1.Controls.Add(this.txtRefPartyAddress);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1020, 186);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(508, 72);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(60, 23);
		this.txtdeliverydays.TabIndex = 12;
		this.txtdeliverydays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(390, 43);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 10;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(97, 127);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combpttrm).Leave += new System.EventHandler(combpttrm_Leave);
		this.deliverystartdate.CustomFormat = "dd-MMM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(390, 72);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(116, 23);
		this.deliverystartdate.TabIndex = 11;
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(452, 16);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(116, 23);
		this.duedate.TabIndex = 9;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(390, 16);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(60, 23);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.RdPartyByCode.AutoSize = true;
		this.RdPartyByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByCode.Location = new System.Drawing.Point(38, 55);
		this.RdPartyByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByCode.Name = "RdPartyByCode";
		this.RdPartyByCode.Size = new System.Drawing.Size(52, 19);
		this.RdPartyByCode.TabIndex = 133;
		this.RdPartyByCode.Text = "Code";
		this.RdPartyByCode.UseVisualStyleBackColor = true;
		this.RdPartyByName.AutoSize = true;
		this.RdPartyByName.Checked = true;
		this.RdPartyByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByName.Location = new System.Drawing.Point(38, 39);
		this.RdPartyByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByName.Name = "RdPartyByName";
		this.RdPartyByName.Size = new System.Drawing.Size(57, 19);
		this.RdPartyByName.TabIndex = 132;
		this.RdPartyByName.TabStop = true;
		this.RdPartyByName.Text = "Name";
		this.RdPartyByName.UseVisualStyleBackColor = true;
		this.RdPartyByName.CheckedChanged += new System.EventHandler(RdPartyByName_CheckedChanged);
		this.BtnDefineBuildingHeights.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineBuildingHeights.Location = new System.Drawing.Point(539, 127);
		this.BtnDefineBuildingHeights.Name = "BtnDefineBuildingHeights";
		this.BtnDefineBuildingHeights.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineBuildingHeights.TabIndex = 5;
		this.BtnDefineBuildingHeights.TabStop = false;
		this.BtnDefineBuildingHeights.UseVisualStyleBackColor = true;
		this.BtnDefineBuildingHeights.Click += new System.EventHandler(BtnDefineBuildingHeights_Click);
		this.BtnDefineBuildingStoreys.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineBuildingStoreys.Location = new System.Drawing.Point(539, 99);
		this.BtnDefineBuildingStoreys.Name = "BtnDefineBuildingStoreys";
		this.BtnDefineBuildingStoreys.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineBuildingStoreys.TabIndex = 5;
		this.BtnDefineBuildingStoreys.TabStop = false;
		this.BtnDefineBuildingStoreys.UseVisualStyleBackColor = true;
		this.BtnDefineBuildingStoreys.Click += new System.EventHandler(BtnDefineBuildingStoreys_Click);
		this.BtnDefineRefParties.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineRefParties.Location = new System.Drawing.Point(789, 70);
		this.BtnDefineRefParties.Name = "BtnDefineRefParties";
		this.BtnDefineRefParties.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineRefParties.TabIndex = 5;
		this.BtnDefineRefParties.TabStop = false;
		this.BtnDefineRefParties.UseVisualStyleBackColor = true;
		this.BtnDefineRefParties.Click += new System.EventHandler(BtnDefineRefParties_Click);
		this.btnDefinePlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefinePlant.Location = new System.Drawing.Point(259, 99);
		this.btnDefinePlant.Name = "btnDefinePlant";
		this.btnDefinePlant.Size = new System.Drawing.Size(29, 26);
		this.btnDefinePlant.TabIndex = 5;
		this.btnDefinePlant.TabStop = false;
		this.btnDefinePlant.UseVisualStyleBackColor = true;
		this.btnDefinePlant.Click += new System.EventHandler(btnDefinePlant_Click);
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(820, 133);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 15);
		this.label48.TabIndex = 43;
		this.label48.Text = "Fcy Amount";
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderQty.Location = new System.Drawing.Point(899, 16);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(116, 23);
		this.txtOrderQty.TabIndex = 20;
		this.txtOrderQty.TabStop = false;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(899, 101);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(116, 23);
		this.txtExchangeRate.TabIndex = 23;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(820, 49);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(79, 15);
		this.label34.TabIndex = 38;
		this.label34.Text = "Order Weight";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(2, 159);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(85, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Order Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(899, 129);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(116, 23);
		this.txtFcyAmount.TabIndex = 24;
		this.txtFcyAmount.TabStop = false;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(820, 105);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 42;
		this.label16.Text = "Echange Rate";
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderWeight.Location = new System.Drawing.Point(899, 45);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.Size = new System.Drawing.Size(116, 23);
		this.txtOrderWeight.TabIndex = 21;
		this.txtOrderWeight.TabStop = false;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(174, 16);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(114, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance237;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance242).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance245).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(899, 70);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(116, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(97, 155);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(918, 23);
		this.txtremarks.TabIndex = 7;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(820, 20);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(59, 15);
		this.label43.TabIndex = 39;
		this.label43.Text = "Order Qty";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(820, 76);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(55, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Fcy Code";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(292, 49);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(292, 76);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(92, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Del S.Date,Days";
		this.Cmbheight.AllowNull = (DefaultableBoolean)1;
		this.Cmbheight.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbheight.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbheight.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Appearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.Cmbheight).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbheight).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance224;
		((SpecialBoxBase)((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.Cmbheight).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbheight).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance229).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbheight.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Location = new System.Drawing.Point(388, 127);
		((System.Windows.Forms.Control)(object)this.Cmbheight).Name = "Cmbheight";
		((System.Windows.Forms.Control)(object)this.Cmbheight).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.Cmbheight).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Leave += new System.EventHandler(combpttrm_Leave);
		this.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtArea.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtArea.Location = new System.Drawing.Point(625, 16);
		this.txtArea.Name = "txtArea";
		this.txtArea.Size = new System.Drawing.Size(193, 23);
		this.txtArea.TabIndex = 15;
		this.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDistance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDistance.Location = new System.Drawing.Point(625, 45);
		this.txtDistance.Name = "txtDistance";
		this.txtDistance.Size = new System.Drawing.Size(193, 23);
		this.txtDistance.TabIndex = 16;
		this.txtDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(571, 12);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(52, 30);
		this.label21.TabIndex = 13;
		this.label21.Text = "Building Area";
		this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.Cmbstoreys.AllowNull = (DefaultableBoolean)1;
		this.Cmbstoreys.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbstoreys.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbstoreys.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbstoreys.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Location = new System.Drawing.Point(388, 99);
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Name = "Cmbstoreys";
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Leave += new System.EventHandler(combpttrm_Leave);
		this.label60.AutoSize = true;
		this.label60.BackColor = System.Drawing.Color.White;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(571, 49);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(53, 15);
		this.label60.TabIndex = 13;
		this.label60.Text = "Distance";
		this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbReferenceParty.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Location = new System.Drawing.Point(625, 70);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Name = "CmbReferenceParty";
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Size = new System.Drawing.Size(163, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Leave += new System.EventHandler(combpttrm_Leave);
		this.CmbVisitedbyName.AllowNull = (DefaultableBoolean)1;
		this.CmbVisitedbyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVisitedbyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVisitedbyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance51;
		((SpecialBoxBase)((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVisitedbyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVisitedbyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Location = new System.Drawing.Point(97, 99);
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Name = "CmbVisitedbyName";
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbVisitedbyName).TabIndex = 4;
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance117).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(97, 43);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 2;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(292, 133);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(90, 15);
		this.label70.TabIndex = 8;
		this.label70.Text = "Building Height";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(97, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(292, 105);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(93, 15);
		this.label69.TabIndex = 8;
		this.label69.Text = "Building Storeys";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(292, 20);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(98, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days && Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(2, 20);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(2, 133);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(84, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Term";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(2, 105);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(59, 15);
		this.label49.TabIndex = 0;
		this.label49.Text = "Visited By";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(2, 49);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(34, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Party";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(571, 133);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(45, 15);
		this.label27.TabIndex = 3;
		this.label27.Text = "Cell No";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(571, 105);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(49, 15);
		this.label18.TabIndex = 3;
		this.label18.Text = "Address";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(571, 76);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(54, 15);
		this.label14.TabIndex = 3;
		this.label14.Text = "Ref Party";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(2, 76);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Reference No";
		this.txtRefPartyCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyCellNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyCellNo.Location = new System.Drawing.Point(625, 129);
		this.txtRefPartyCellNo.Name = "txtRefPartyCellNo";
		this.txtRefPartyCellNo.Size = new System.Drawing.Size(193, 23);
		this.txtRefPartyCellNo.TabIndex = 19;
		this.txtRefPartyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyAddress.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyAddress.Location = new System.Drawing.Point(625, 101);
		this.txtRefPartyAddress.Name = "txtRefPartyAddress";
		this.txtRefPartyAddress.Size = new System.Drawing.Size(193, 23);
		this.txtRefPartyAddress.TabIndex = 18;
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsupprefno.Location = new System.Drawing.Point(97, 72);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(191, 23);
		this.txtsupprefno.TabIndex = 3;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1322, 24);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(113, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "SALE ORDER";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1239, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(81, 18);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1322, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[13]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.btnsave, this.btnDelete, this.btnSaveAs, this.toolStripSeparator3, this.btnattachment, this.toolStripSeparator4,
			this.btnprint, this.toolStripSeparator1, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1322, 27);
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1852-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1326, 758);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1322, 754);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 113);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1322, 641);
		this.panel9.TabIndex = 1;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(0, 409);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1322, 232);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 1;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 377);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1322, 32);
		this.panel13.TabIndex = 48;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1322, 377);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.RdCustomerHistorySearchByCode);
		this.panel8.Controls.Add(this.RdCustomerHistorySearchByName);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory);
		this.panel8.Controls.Add(this.label4);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory);
		this.panel8.Controls.Add(this.btnshowHistory);
		this.panel8.Controls.Add(this.label3);
		this.panel8.Controls.Add(this.ToDate);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Controls.Add(this.fromdate);
		this.panel8.Controls.Add(this.label9);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierHistory);
		this.panel8.Controls.Add(this.label13);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1322, 113);
		this.panel8.TabIndex = 0;
		this.RdCustomerHistorySearchByCode.AutoSize = true;
		this.RdCustomerHistorySearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByCode.Location = new System.Drawing.Point(334, 60);
		this.RdCustomerHistorySearchByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByCode.Name = "RdCustomerHistorySearchByCode";
		this.RdCustomerHistorySearchByCode.Size = new System.Drawing.Size(52, 19);
		this.RdCustomerHistorySearchByCode.TabIndex = 153;
		this.RdCustomerHistorySearchByCode.Text = "Code";
		this.RdCustomerHistorySearchByCode.UseVisualStyleBackColor = true;
		this.RdCustomerHistorySearchByName.AutoSize = true;
		this.RdCustomerHistorySearchByName.Checked = true;
		this.RdCustomerHistorySearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByName.Location = new System.Drawing.Point(276, 60);
		this.RdCustomerHistorySearchByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByName.Name = "RdCustomerHistorySearchByName";
		this.RdCustomerHistorySearchByName.Size = new System.Drawing.Size(57, 19);
		this.RdCustomerHistorySearchByName.TabIndex = 152;
		this.RdCustomerHistorySearchByName.TabStop = true;
		this.RdCustomerHistorySearchByName.Text = "Name";
		this.RdCustomerHistorySearchByName.UseVisualStyleBackColor = true;
		this.RdCustomerHistorySearchByName.CheckedChanged += new System.EventHandler(RdCustomerHistorySearchByName_CheckedChanged);
		this.cmbApproveStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance255).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance264).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance264).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance264).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbApproveStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory).Location = new System.Drawing.Point(837, 80);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory).Name = "cmbApproveStatusHistory";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatusHistory).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(684, 62);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(95, 15);
		this.label4.TabIndex = 151;
		this.label4.Text = "Approved Status";
		this.CmbIsApprovedHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbIsApprovedHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIsApprovedHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIsApprovedHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIsApprovedHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbIsApprovedHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory).Location = new System.Drawing.Point(684, 80);
		((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory).Name = "CmbIsApprovedHistory";
		((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory).Size = new System.Drawing.Size(151, 26);
		((System.Windows.Forms.Control)(object)this.CmbIsApprovedHistory).TabIndex = 4;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(973, 80);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(57, 26);
		this.btnshowHistory.TabIndex = 6;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshow_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(837, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(40, 15);
		this.label3.TabIndex = 148;
		this.label3.Text = "Status";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(124, 82);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(113, 23);
		this.ToDate.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(124, 62);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 15);
		this.label7.TabIndex = 147;
		this.label7.Text = "To Date";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(7, 82);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(115, 23);
		this.fromdate.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 62);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 15);
		this.label9.TabIndex = 146;
		this.label9.Text = "From Date";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(480, 80);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 3;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(480, 62);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 145;
		this.label10.Text = "Item Name";
		this.CmbSupplierHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierHistory).Location = new System.Drawing.Point(239, 80);
		((System.Windows.Forms.Control)(object)this.CmbSupplierHistory).Name = "CmbSupplierHistory";
		((System.Windows.Forms.Control)(object)this.CmbSupplierHistory).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierHistory).TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(239, 62);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(34, 15);
		this.label13.TabIndex = 144;
		this.label13.Text = "Party";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Transparent;
		this.label5.Location = new System.Drawing.Point(6, 33);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(193, 19);
		this.label5.TabIndex = 22;
		this.label5.Text = "SALE ORDER HISTORY";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 27);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1322, 31);
		this.panel12.TabIndex = 23;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1280, 5);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1322, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.PanelMessage.BackColor = System.Drawing.Color.White;
		this.PanelMessage.Controls.Add(this.ToolStripMessage);
		this.PanelMessage.Location = new System.Drawing.Point(223, 109);
		this.PanelMessage.Name = "PanelMessage";
		this.PanelMessage.Size = new System.Drawing.Size(24, 20);
		this.PanelMessage.TabIndex = 128;
		this.ToolStripMessage.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStripMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnMessage });
		this.ToolStripMessage.Location = new System.Drawing.Point(0, 0);
		this.ToolStripMessage.Name = "ToolStripMessage";
		this.ToolStripMessage.Size = new System.Drawing.Size(24, 25);
		this.ToolStripMessage.TabIndex = 0;
		this.ToolStripMessage.Text = "toolStrip3";
		this.BtnMessage.BackColor = System.Drawing.Color.LightGray;
		this.BtnMessage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.BtnMessage.Image = Architecture.WinApp.Properties.Resources.info_i;
		this.BtnMessage.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnMessage.Name = "BtnMessage";
		this.BtnMessage.Size = new System.Drawing.Size(23, 22);
		this.BtnMessage.ToolTipText = "Add Total Order Amount and System Will Generate the Discount Automatically\r\nAccording To Following Formula\r\nDiscount = (ItemAmount +Frieght+OtherWages+Wages) - ManualTotalOrderAmount";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1334, 787);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "SaleOrderConcrete";
		this.Text = "Sale Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleOrder_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFrieghtAccount).EndInit();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.TabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjob).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefSalesMan).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbheight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbstoreys).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVisitedbyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIsApprovedHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.PanelMessage.ResumeLayout(false);
		this.PanelMessage.PerformLayout();
		this.ToolStripMessage.ResumeLayout(false);
		this.ToolStripMessage.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
