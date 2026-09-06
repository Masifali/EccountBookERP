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
using Architecture.BLL.Inventory;
using Architecture.BLL.PartyProcessing;
using Architecture.Common;
using Architecture.DAL.PartyProcessing;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
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
namespace Architecture.WinApp.PartyProcessing
{

public class PartyProGrnAndGdn : Form
{
	private int updateDetailIndex;

	private int DefaultDaysToLessFromHistoryFromDate;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int RecId;

	private int GPID;

	public int DocumentTypeId;

	private bool ExpenseGridsInTabOrinFront;

	private bool WagesStatusForGrn;

	private bool WagesActiveOrInActiveGrn;

	private bool WagesStatusForGdn;

	private bool WagesActiveOrInActiveGdn;

	private bool AllowMultipleOrPartialInvoicingForADO;

	private decimal PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance;

	private double GpQty;

	private string GpType = "";

	private Rightsobjects formrights = new Rightsobjects();

	private DataTable dtdetail = new DataTable();

	private DataTable dtEmptyBag = new DataTable();

	private DataTable dtWare = new DataTable();

	private DataTable dtJob = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtcityall = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private List<InvGrnPartyProcessingDetail> lstRemoveRecord = new List<InvGrnPartyProcessingDetail>();

	private LoadavailableTransactionsForIssuancePartyProcessing LoadTransactions;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton printToolStripButton;

	private ToolStripButton btnAttachment;

	private Panel panel4;

	private Panel panel5;

	private Label label24;

	private Label label25;

	private Label label21;

	private Label lblvehno;

	private TextBox txtbltyno;

	private Label label7;

	private TextBox txtvehno;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtcarramount;

	private Label label20;

	private TextBox txtsuppwt;

	private Label lblsupwt;

	private TextBox txtfctwt;

	private Label label11;

	private TextBox txtwtdiff;

	internal UltraCombo CmbStockParty;

	private Label label2;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbCity;

	internal UltraCombo CmbWareHouse;

	internal UltraCombo CmbPackingtype;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbItem;

	private Label label30;

	private Label label27;

	private Label label22;

	private TextBox txtebu;

	private TextBox txtGrossWeight;

	private TextBox txtQty;

	private TextBox txtadlswt;

	private TextBox txtstockwt;

	private Label label29;

	private Label label28;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label41;

	private Label label40;

	private Label label36;

	private Panel panel7;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbCropYear;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private TextBox combgatepass;

	internal GridEX grd;

	private CheckBox ChkBox;

	private TextBox txtEmptyBagsTotal;

	internal GridEX grdGp;

	private ToolStripButton CityDefine;

	private Button Add;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	internal UltraCombo combvehtyp;

	private ToolStripButton btnDelete;

	internal GridEX grdEmptyBags;

	private Panel panel9;

	private Panel PanelEB;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnGrnFormHistory;

	internal UltraCombo CmbRefParty;

	private Label label1;

	private Label label3;

	private TextBox txtCommentsDetail;

	private ToolStripButton btnGdn339;

	private Label lblContainerNo;

	private Label lblSealNo;

	private TextBox txtContainerNo;

	private TextBox txtSealNo;

	private Label label9;

	private Panel paneGP;

	private Label label6;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator1;

	private Label lblFormCaption;

	private TextBox txtremarks;

	private TabControl tabControl2;

	private TabPage tabDetail;

	private Label label4;

	private TabPage tabEB;

	private TabPage tabGP;

	private Panel PanelOtherGridsMain;

	private Panel panel3;

	private RadioButton rdTemplate2;

	private RadioButton rdTemplate1;

	private Panel panel18;

	private Label lblHistory;

	private Panel panel23;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private GroupBox groupBox3;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private Button btnshowHistory;

	private Label label49;

	private RadioButton RadAll;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	internal UltraCombo cmbStockPartyHistory;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label8;

	internal UltraCombo cmbReferencePartyHistory;

	private Panel panel6;

	private Panel panel8;

	private Panel panel20;

	private Label label32;

	private CtrlGrdBar ctrlGrdBar4;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnDefineReferenceParty;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnLoadAvailableData;

	internal UltraCombo CmbTradeToCompany;

	private Label label10;

	private Button BtnLookup;

	public PartyProGrnAndGdn(UserAccount user)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1160, 750);
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			AllowMultipleOrPartialInvoicingForADO = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("AllowMultipleOrPartialInvoicingForADO"));
			PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance = Conversion.ToDecimal((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance"));
			if (PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance == 0m)
			{
				PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance = 10m;
			}
			if (Conversion.ToString(base.Tag) == "GoodsReceivingNotesPartyProcessing")
			{
				DocumentTypeId = 49;
				txtCommentsDetail.Size = new Size(462, 23);
				lblContainerNo.Hide();
				txtContainerNo.Hide();
				lblSealNo.Hide();
				txtSealNo.Hide();
				lblFormCaption.Text = "Goods Receiving Notes";
				base.Name = "GoodsReceivingNotesPartyProcessing";
				lblHistory.Text = "Goods Receiving Notes Party Processing History";
				loadonGdnvisibleitem(value: false);
				combgatepass.ReadOnly = true;
				RadAll.Visible = true;
				RadReffered.Visible = true;
				RadNotReffered.Visible = true;
				WagesStatusForGrn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryForGrnPartyProcessing"));
				WagesRefDocumentsStatus obj = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 49);
				WagesActiveOrInActiveGrn = obj != null && obj.IsActive;
				btnLoadAvailableData.Visible = false;
				btnLoadAvailableData.Enabled = false;
			}
			if (Conversion.ToString(base.Tag) == "GoodsDispatchNotesPartyProcessing")
			{
				btnGdn339.Visible = true;
				printToolStripButton.Visible = false;
				DocumentTypeId = 89;
				txtCommentsDetail.Size = new Size(208, 23);
				lblContainerNo.Show();
				txtContainerNo.Show();
				lblSealNo.Show();
				txtSealNo.Show();
				lblFormCaption.Text = "Goods Dispatch Notes";
				lblHistory.Text = "Goods Dispatch Notes Party Processing History";
				base.Name = "GoodsDispatchNotesPartyProcessing";
				txtsuppwt.Visible = false;
				lblsupwt.Visible = false;
				loadonGdnvisibleitem(value: false);
				RadAll.Visible = false;
				RadReffered.Visible = false;
				RadNotReffered.Visible = false;
				WagesStatusForGdn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WagesCompulsoryForGdnPartyProcessing"));
				WagesRefDocumentsStatus obj2 = clsGlobalVariables.WagesRefDocumentsStatusList.Find((WagesRefDocumentsStatus row) => row.RefDocumentTypeId == 89);
				WagesActiveOrInActiveGdn = obj2 != null && obj2.IsActive;
				btnLoadAvailableData.Visible = true;
				btnLoadAvailableData.Enabled = true;
			}
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			printToolStripButton.Enabled = formrights.DoHavePrintRights;
			btnGdn339.Enabled = formrights.DoHavePrintRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("Id");
			dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("RefDocIdNo", typeof(int));
			dtdetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtdetail.Columns.Add("DoDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("DeliveryOrderId", typeof(int));
			dtdetail.Columns.Add("DeliveryOrderDetailId", typeof(int));
			dtdetail.Columns.Add("DeliveryOrderNo", typeof(int));
			dtdetail.Columns.Add("InvoiceDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("InvoiceId", typeof(int));
			dtdetail.Columns.Add("InvoiceDetailId", typeof(int));
			dtdetail.Columns.Add("InvoiceDocType", typeof(string));
			dtdetail.Columns.Add("InvoiceNo", typeof(int));
			dtdetail.Columns.Add("InvoiceManualBillNo", typeof(string));
			dtdetail.Columns.Add("WareHouseId", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYearId", typeof(int));
			dtdetail.Columns.Add("JobId", typeof(int));
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("PackUomId", typeof(int));
			dtdetail.Columns.Add("PackUom");
			dtdetail.Columns.Add("Equivalent", typeof(double));
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("NetBillWeight", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("BalWeight", typeof(double));
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("Comments");
			dtdetail.Columns.Add("ContainerNo");
			dtdetail.Columns.Add("SealNo");
			dtdetail.Columns.Add("InvoiceQty", typeof(double));
			dtdetail.Columns.Add("InvoiceWeight", typeof(double));
			dtdetail.Columns.Add("InvoiceEbUnit", typeof(double));
			dtdetail.Columns.Add("InvoiceEbTotal", typeof(double));
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Columns.Add("ItemId");
			dtEmptyBag.Columns.Add("Qty", typeof(double));
			dtEmptyBag.Columns.Add("Remarks");
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			GenerateCode();
			StockPartyBind();
			ReferencePartyBind();
			TradeToCompanyFill();
			VehicleTypesFill();
			CropYear();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
			ItemNameFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			Gatepass();
			ChkBox.Checked = true;
			ChangeTemplate(ExpenseGridsInTabOrinFront);
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosFill()
	{
		try
		{
			DataTable dtStock = new DataTable();
			dtStock.Columns.Add("Id", typeof(int));
			dtStock.Columns.Add("name", typeof(string));
			DataTable dtRef = new DataTable();
			dtRef.Columns.Add("Id", typeof(int));
			dtRef.Columns.Add("name", typeof(string));
			DataTable dt = InvGrnPartyProcessing.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, DocumentTypeId.ToString(), (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "StockParty")
					{
						dtStock.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ReferenceParty")
					{
						dtRef.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtStock, cmbStockPartyHistory, "Id", "name", "Stock Party", ZeroIndex: false);
			DDL.BindDDL(dtRef, cmbReferencePartyHistory, "Id", "name", "Reference Party", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TradeToCompanyFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTradeToCompany.Value);
			DataTable dt = CommonServices.GetLookupsByTypeIdDt(17);
			DataTable dtType = new DataTable();
			dtType.Columns.Add("Id", typeof(int));
			dtType.Columns.Add("Type");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LookupName"]);
				}
			}
			DDL.BindDDL(dtType, CmbTradeToCompany, "Id", "Type", "Trade To Company", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbTradeToCompany.Value = Id;
				}
				else
				{
					((Control)(object)CmbTradeToCompany).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int DocNo = Conversion.ToInt((object)InvGrnPartyProcessing.GenerateCode(new InvGrnPartyProcessing
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			}));
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockPartyBind()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerForPartyProcessing();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbStockParty, "Id", "CompanyName", "StockParty", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferencePartyBind()
	{
		try
		{
			DataTable dt = CommonServices.ReferencePartyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbRefParty, "Id", "ReferencePartyName", "ReferencePartyName", false);
				DDL.BindDDLNew(dt, CmbTransport, "Id", "ReferencePartyName", "TransporterName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GRNGridSetting()
	{
		try
		{
			grdGp.RootTable.Columns["Id"].Visible = false;
			grdGp.RootTable.Columns["StockPartyId"].Visible = false;
			grdGp.RootTable.Columns["RefPartyId"].Visible = false;
			grdGp.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdGp.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdGp.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.###";
			grdGp.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.###";
			grdGp.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdGp.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdGp.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			grdGp.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdGp);
			grdGp.RootTable.Columns.Add("Load");
			grdGp.RootTable.Columns["Load"].Caption = "Load";
			grdGp.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGp.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdGp.RootTable.Columns["Load"].ButtonText = "Load";
			grdGp.RootTable.Columns["Load"].Width = 40;
			grdGp.RootTable.Columns["Load"].Position = 0;
			grdGp.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleTypesFill()
	{
		try
		{
			DataTable dtVehicle = VehicleType.GetAll();
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtVehicle, combvehtyp, "Id", "VehicleDescription", "VehicleDescription", false);
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
			int ID = Conversion.ToInt(CmbCropYear.Value);
			dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "Crop Year", true);
				((UltraGridBase)CmbCropYear).Rows[1].Activate();
				if (ID > 0)
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbCropYear.Value = ID;
					}
					else
					{
						((Control)(object)CmbCropYear).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbPackingtype.Value);
			dtPackType = InvPackingType.Getall();
			if (dtPackType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackType, CmbPackingtype, "Id", "PackTypeDesc", "PackingType", true);
				((UltraGridBase)CmbPackingtype).Rows[1].Activate();
				if (ID > 0)
				{
					if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbPackingtype.Value = ID;
					}
					else
					{
						((Control)(object)CmbPackingtype).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackingtype).Text = string.Empty;
				((UltraGridBase)CmbPackingtype).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			string PackUom = ((Control)(object)CmbPackUom).Text;
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(CmbItem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("UomCode", typeof(string));
				dt.Columns.Add("Equivalent", typeof(double));
				for (int i = 0; i < dtsearchby.Rows.Count; i++)
				{
					dt.Rows.Add(dtsearchby.Rows[i]["Id"], dtsearchby.Rows[i]["UOMCode"], dtsearchby.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dt, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == PackUom))
				{
					((Control)(object)CmbPackUom).Text = PackUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFillWithoutOrder()
	{
		try
		{
			int id = Conversion.ToInt(CmbJobLot.Value);
			dtJob = jobLot.AllJobLotForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJob, CmbJobLot, "Id", "JobLotDescription", "JobLotDescription", true);
				if (id > 0)
				{
					if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbJobLot.Value = id;
					}
					else
					{
						CmbJobLot.Value = string.Empty;
					}
				}
			}
			else
			{
				CmbJobLot.Value = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbItem.Value);
			DataTable dt = Item.ReadAllForItemsForPartyProcessing(UserAccount.OrganizationId, UserAccount.CompanyId, "1,2,4");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbItem.Value = id;
					}
					else
					{
						CmbItem.Value = string.Empty;
					}
				}
			}
			else
			{
				CmbItem.Value = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			int id = Conversion.ToInt(CmbWareHouse.Value);
			dtWare = CommonServices.WareHouseGetAllService();
			if (dtWare.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWare, CmbWareHouse, "Id", "WareHouseName", "WareHouse Name", true);
				if (id > 0)
				{
					if (dtWare.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbWareHouse.Value = id;
					}
					else
					{
						CmbWareHouse.Value = string.Empty;
					}
				}
			}
			else
			{
				CmbWareHouse.Value = string.Empty;
				((UltraGridBase)CmbWareHouse).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		try
		{
			dtcityall.Clear();
			dtcityall = CommonServices.CityGetAllService();
			if (dtcityall.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcityall, CmbCity, "Id", "CityName", "City Name", false);
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
			PackUOMFillWithoutOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		if (btnSave.Visible && btnSave.Enabled)
		{
			TotalWeight();
		}
		Total();
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStockParty).ActiveRow == null || Conversion.ToInt(CmbStockParty.Value) <= 0)
		{
			MessageBox.Show("Stock Party Field is Required");
			CmbStockParty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRefParty).ActiveRow == null || Conversion.ToInt(CmbRefParty.Value) <= 0)
		{
			MessageBox.Show("Reference Party Field is Required");
			CmbRefParty.Focus();
			return false;
		}
		if (Conversion.ToInt((object)combgatepass.Text.Trim()) == 0)
		{
			MessageBox.Show("Gatepass Field is Required");
			combgatepass.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtcarramount.Text.Trim()) > 0.0 && (((UltraGridBase)CmbTransport).ActiveRow == null || Conversion.ToInt(CmbTransport.Value) == 0))
		{
			MessageBox.Show("Transporter Field is Required");
			CmbTransport.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransport).ActiveRow != null && Conversion.ToInt(CmbTransport.Value) > 0 && Conversion.ToDouble((object)txtcarramount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtcarramount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) <= 0)
		{
			MessageBox.Show("Warehouse Name Field is Required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) <= 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) <= 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) <= 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingtype).ActiveRow == null || Conversion.ToInt(CmbPackingtype.Value) <= 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingtype.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) <= 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtGrossWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtstockwt.Text.Trim() == "" || Conversion.ToDouble((object)txtstockwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtstockwt.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null || Conversion.ToInt(CmbCity.Value) <= 0)
		{
			MessageBox.Show("City Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private void loadonGdnvisibleitem(bool value)
	{
		try
		{
			combvehtyp.ReadOnly = value;
			txtvehno.ReadOnly = value;
			txtbltyno.ReadOnly = value;
			combgatepass.ReadOnly = value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdTemplate1_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdTemplate1.Checked)
			{
				ExpenseGridsInTabOrinFront = false;
			}
			else
			{
				ExpenseGridsInTabOrinFront = true;
			}
			ChangeTemplate(ExpenseGridsInTabOrinFront);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChangeTemplate(bool Type)
	{
		try
		{
			tabControl2.TabPages.Remove(tabEB);
			tabControl2.TabPages.Remove(tabGP);
			if (Type)
			{
				PanelOtherGridsMain.Visible = false;
				tabControl2.TabPages.Add(tabEB);
				tabControl2.TabPages.Add(tabGP);
				PanelEB.Dock = DockStyle.Fill;
				tabEB.Controls.Add(PanelEB);
				paneGP.Dock = DockStyle.Fill;
				tabGP.Controls.Add(paneGP);
				tabControl2.Dock = DockStyle.Fill;
			}
			else
			{
				PanelOtherGridsMain.Visible = true;
				PanelOtherGridsMain.Controls.Add(PanelEB);
				PanelEB.Dock = DockStyle.Left;
				PanelOtherGridsMain.Controls.Add(paneGP);
				paneGP.Dock = DockStyle.Fill;
				paneGP.BringToFront();
				tabControl2.Dock = DockStyle.Top;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 89 && grd.RowCount > 0)
			{
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId"))
				{
					throw new Exception("You can not add manual Record beacause record against Do exist in Grid");
				}
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocumentTypeId") || GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocIdNo"))
				{
					throw new Exception("You can not add manual Record beacause record against Loader exist in Grid");
				}
			}
			if (FormValidationDetail())
			{
				dtdetail.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Conversion.ToInt(CmbWareHouse.Value), Conversion.ToInt(CmbItem.Value), ((Control)(object)CmbItem).Text, Conversion.ToInt(CmbCropYear.Value), Conversion.ToInt(CmbJobLot.Value), Conversion.ToInt(CmbPackingtype.Value), Conversion.ToInt(CmbPackUom.Value), ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtebu.Text.Trim()), Conversion.ToDouble((object)txtEmptyBagsTotal.Text.Trim()), Conversion.ToDouble((object)txtadlswt.Text.Trim()), Conversion.ToDouble((object)txtstockwt.Text.Trim()), Conversion.ToDouble((object)txtstockwt.Text.Trim()), 0, Conversion.ToInt(CmbCity.Value), txtCommentsDetail.Text.Trim(), txtContainerNo.Text.Trim(), txtSealNo.Text.Trim(), 0, 0, 0, 0);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetial();
				double gridWeight = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					gridWeight += Conversion.ToDouble(item.Cells["GrossWight"].Value);
				}
				double FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
				if (FactoryWeight > 0.0)
				{
					txtGrossWeight.Text = (FactoryWeight - gridWeight).ToString();
				}
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
			bool hasRefDoc = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocumentTypeId");
			bool hasDeliveryOrder = GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId");
			List<string> hiddenCols = new List<string>
			{
				"Id", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "DoDocumentTypeId", "DeliveryOrderId", "DeliveryOrderNo", "InvoiceDocumentTypeId", "InvoiceId", "InvoiceDetailId",
				"InvoiceDocType", "InvoiceNo", "InvoiceManualBillNo", "ItemId", "PackUomId", "DeliveryOrderDetailId", "Equivalent", "BalWeight", "InvoiceEbUnit", "InvoiceEbTotal",
				"InvoiceWeight", "InvoiceQty"
			};
			Dictionary<string, EditType> editableCols = new Dictionary<string, EditType>
			{
				{
					"Qty",
					(EditType)1
				},
				{
					"GrossWight",
					(EditType)1
				},
				{
					"EbUnit",
					(EditType)1
				},
				{
					"EbTotal",
					(EditType)1
				},
				{
					"AddLesswt",
					(EditType)1
				},
				{
					"Comments",
					(EditType)1
				},
				{
					"ContainerNo",
					(EditType)1
				},
				{
					"SealNo",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 2, hiddenCols, editableCols);
			GridDetailComboBind();
			if (DocumentTypeId == 49)
			{
				grd.RootTable.Columns["ContainerNo"].Visible = false;
				grd.RootTable.Columns["SealNo"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["ContainerNo"].Visible = true;
				grd.RootTable.Columns["SealNo"].Visible = true;
				if (hasRefDoc || hasDeliveryOrder)
				{
					panel5.Visible = false;
					((Control)(object)CmbStockParty).Enabled = false;
				}
				if (hasRefDoc || (hasDeliveryOrder && AllowMultipleOrPartialInvoicingForADO))
				{
					string[] array = new string[4] { "WareHouseId", "CropYearId", "JobId", "PackingTypeId" };
					foreach (string colName2 in array)
					{
						grd.RootTable.Columns[colName2].EditType = (EditType)0;
					}
				}
				if (hasDeliveryOrder)
				{
					string[] array = new string[8] { "DeliveryOrderNo", "InvoiceDocType", "InvoiceNo", "InvoiceManualBillNo", "InvoiceQty", "InvoiceWeight", "InvoiceEbUnit", "InvoiceEbTotal" };
					foreach (string colName in array)
					{
						grd.RootTable.Columns[colName].Visible = true;
					}
				}
			}
			if (DocumentTypeId == 49 || !AllowMultipleOrPartialInvoicingForADO)
			{
				GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
				GridEX_Helper.AddButton(grd, "Add", "+", 20, 1);
				grd.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
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
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRecord(r);
				}
				else if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				else if (e.Column.Key == "Add")
				{
					AddDetailRow(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailRow(GridEXRow r)
	{
	}

	private void DeleteDetailRecord(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (btnSave.Visible && btnSave.Enabled)
				{
					r.Delete();
				}
				else
				{
					InvGrnPartyProcessingDetail grnDetail = new InvGrnPartyProcessingDetail();
					grnDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					grnDetail.RefDocNoId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					grnDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					grnDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					grnDetail.DeliveryOrderId = Conversion.ToInt(r.Cells["DeliveryOrderId"].Value);
					grnDetail.DeliveryOrderDetailId = Conversion.ToInt(r.Cells["DeliveryOrderDetailId"].Value);
					grnDetail.DoDocumentTypeId = Conversion.ToInt(r.Cells["DoDocumentTypeId"].Value);
					grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					grnDetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
					grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
					grnDetail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
					grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
					grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
					grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
					grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
					grnDetail.NetBillWeight = grnDetail.StockWeight;
					grnDetail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					grnDetail.CommentsDetail = Conversion.ToString((object)r.Cells["Comments"].Text.Trim());
					grnDetail.ContainerNo = Conversion.ToString((object)r.Cells["ContainerNo"].Text.Trim());
					grnDetail.SealNo = Conversion.ToString((object)r.Cells["SealNo"].Text.Trim());
					grnDetail.ActionTypeId = 3;
					lstRemoveRecord.Add(grnDetail);
					dtdetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (GpType != "AdvanceDO" && grd.GetRows().Count() == 0)
				{
					((Control)(object)CmbStockParty).Enabled = true;
					panel5.Visible = true;
				}
				grd.UpdateData();
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
			if (DocumentTypeId == 89 && grd.RowCount > 0)
			{
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId"))
				{
					throw new Exception("You can not Update like manual Record beacause record against Do exist in Grid");
				}
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocumentTypeId") || GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "RefDocIdNo"))
				{
					throw new Exception("You can not Update like manual Record beacause record against Loader exist in Grid");
				}
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobId"].Value.ToString();
				CmbPackingtype.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				txtQty.Text = item.Cells["Qty"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtEmptyBagsTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtstockwt.Text = item.Cells["StockWeight"].Value.ToString();
				CmbCity.Value = item.Cells["CityId"].Value.ToString();
				txtCommentsDetail.Text = item.Cells["Comments"].Text.ToString();
				txtContainerNo.Text = item.Cells["ContainerNo"].Text.ToString();
				txtSealNo.Text = item.Cells["SealNo"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouse.Focus();
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
				dtdetail.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItem).Text;
				dtdetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				dtdetail.Rows[updateDetailIndex]["JobId"] = Conversion.ToInt(CmbJobLot.Value);
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingtype.Value);
				dtdetail.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
				dtdetail.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["Equivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
				dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQty.Text);
				dtdetail.Rows[updateDetailIndex]["GrossWight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtdetail.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtebu.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = Conversion.ToDouble((object)txtadlswt.Text.Trim());
				dtdetail.Rows[updateDetailIndex]["StockWeight"] = Conversion.ToDouble((object)txtstockwt.Text);
				dtdetail.Rows[updateDetailIndex]["BalWeight"] = Conversion.ToDouble((object)txtstockwt.Text);
				dtdetail.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(CmbCity.Value);
				dtdetail.Rows[updateDetailIndex]["Comments"] = txtCommentsDetail.Text.Trim();
				dtdetail.Rows[updateDetailIndex]["ContainerNo"] = txtContainerNo.Text.Trim();
				dtdetail.Rows[updateDetailIndex]["SealNo"] = txtSealNo.Text.Trim();
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetial();
				double gridWeight = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					gridWeight += Conversion.ToDouble(item.Cells["GrossWight"].Value);
				}
				double GrossBal = Conversion.ToDouble((object)txtfctwt.Text.Trim()) - gridWeight;
				txtGrossWeight.Text = GrossBal.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void GridDetailComboBind()
	{
		try
		{
			GridEX_Helper.ComboBind(grd.RootTable.Columns["WareHouseId"], dtWare, "Id", "WareHouseName", "WareHouse Name", 130);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CropYearId"], dtCrop, "Id", "CropYear", "Crop Year", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["JobId"], dtJob, "Id", "JobLotDescription", "Job Lot", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["PackingTypeId"], dtPackType, "Id", "PackTypeDesc", "Packing Type", 100);
			GridEX_Helper.ComboBind(grd.RootTable.Columns["CityId"], dtcityall, "Id", "CityName", "City Name", 100);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grd_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "EbUnit" || e.Column.Key == "EbTotal") && !CommonServices.IsPositiveNumeric(e))
			{
				throw new Exception("Please Type Only Positive Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (DocumentTypeId == 89 && row != null && (int)row.RowType == 0 && row.Cells.Count != 0)
			{
				string col = e.Column.Key;
				double qty = Conversion.ToDouble(row.Cells["Qty"].Value);
				double uom = Conversion.ToDouble(row.Cells["Equivalent"].Value);
				double grossWeight = Conversion.ToDouble(row.Cells["GrossWight"].Value);
				double ebUnit = Conversion.ToDouble(row.Cells["EbUnit"].Value);
				double ebTotal = Conversion.ToDouble(row.Cells["EbTotal"].Value);
				double addLess = Conversion.ToDouble(row.Cells["AddLesswt"].Value);
				double invoiceWeight = Conversion.ToDouble(row.Cells["InvoiceWeight"].Value);
				double invoiceQty = Conversion.ToDouble(row.Cells["InvoiceQty"].Value);
				switch (col)
				{
				case "Qty":
				{
					double weight = uom * qty;
					row.Cells["GrossWight"].Value = weight;
					double netBillWeight5 = weight - ebTotal + addLess;
					row.Cells["NetBillWeight"].Value = netBillWeight5;
					double invoiceWeightPerQty = ((invoiceQty != 0.0) ? (invoiceWeight / invoiceQty) : 0.0);
					row.Cells["StockWeight"].Value = invoiceWeightPerQty * qty;
					break;
				}
				case "GrossWight":
				{
					double netBillWeight4 = grossWeight - ebTotal + addLess;
					row.Cells["NetBillWeight"].Value = netBillWeight4;
					break;
				}
				case "EbUnit":
				{
					double totalEB = ebUnit * qty;
					row.Cells["EbTotal"].Value = totalEB;
					double netBillWeight3 = grossWeight - totalEB + addLess;
					row.Cells["NetBillWeight"].Value = netBillWeight3;
					break;
				}
				case "EbTotal":
				{
					double ebU = ((qty != 0.0) ? (ebTotal / qty) : 0.0);
					row.Cells["EbUnit"].Value = ebU;
					double netBillWeight2 = grossWeight - ebTotal + addLess;
					row.Cells["NetBillWeight"].Value = netBillWeight2;
					break;
				}
				case "AddLesswt":
				{
					double netBillWeight = grossWeight - ebTotal + addLess;
					row.Cells["NetBillWeight"].Value = netBillWeight;
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

	private void AddRowInvEmptyBagsGrid()
	{
		dtEmptyBag.Rows.Add(0, 0, "");
		grdEmptyBags.DataSource = dtEmptyBag;
	}

	private void grdEmptyBagsSettings()
	{
		try
		{
			DataTable dt = CommonServices.GetItemPartyProcessing("7");
			grdEmptyBags.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdEmptyBags.RootTable.Columns["ItemId"].HasValueList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].LimitToList = true;
			grdEmptyBags.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdEmptyBags.RootTable.Columns["ItemId"].Caption = "Item Name";
			grdEmptyBags.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "ItemName");
			grdEmptyBags.RootTable.Columns["ItemId"].Width = 250;
			grdEmptyBags.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdEmptyBags.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdEmptyBags.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdEmptyBags.RootTable.Columns.Add("Delete");
			grdEmptyBags.RootTable.Columns["Delete"].Key = "Delete";
			grdEmptyBags.RootTable.Columns["Delete"].Caption = "X";
			grdEmptyBags.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Delete"].ButtonText = "X";
			grdEmptyBags.RootTable.Columns["Delete"].Width = 20;
			grdEmptyBags.RootTable.Columns["Delete"].Position = 0;
			grdEmptyBags.RootTable.Columns.Add("Add");
			grdEmptyBags.RootTable.Columns["Add"].Key = "Add";
			grdEmptyBags.RootTable.Columns["Add"].Caption = "+";
			grdEmptyBags.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdEmptyBags.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdEmptyBags.RootTable.Columns["Add"].ButtonText = "+";
			grdEmptyBags.RootTable.Columns["Add"].Width = 20;
			grdEmptyBags.RootTable.Columns["Add"].Position = 1;
			grdEmptyBags.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdEmptyBags.CurrentRow;
				dtEmptyBag.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtEmptyBag.Rows.Count == 0)
				{
					AddRowInvEmptyBagsGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			GatePassPartyProcessing obj = new GatePassPartyProcessing();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.GpSrNo = Conversion.ToInt((object)combgatepass.Text.Trim());
			obj.FinancialYearId = Conversion.ToInt((object)clsGlobalVariables.ActiveYr.Id);
			if (DocumentTypeId == 49)
			{
				obj.DocumentTypeId = 54;
			}
			if (DocumentTypeId == 89)
			{
				obj.DocumentTypeId = 55;
			}
			DataTable dtgpForGRn = GatePassPartyProcessing.ReadByGpNoForInvGrnPartyProcessing(obj);
			if (dtgpForGRn.Rows.Count > 0)
			{
				GpType = dtgpForGRn.Rows[0]["GatepassType"].ToString();
				((Control)(object)combvehtyp).Text = dtgpForGRn.Rows[0]["VehicleType"].ToString();
				txtvehno.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
				txtbltyno.Text = dtgpForGRn.Rows[0]["BiltyNo"].ToString();
				txtremarks.Text = dtgpForGRn.Rows[0]["OtherRemarks"].ToString();
				txtcarramount.Text = dtgpForGRn.Rows[0]["Freight"].ToString();
				txtQty.Text = dtgpForGRn.Rows[0]["ItemQty"].ToString();
				GpQty = Conversion.ToDouble(dtgpForGRn.Rows[0]["ItemQty"]);
				txtsuppwt.Text = dtgpForGRn.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtgpForGRn.Rows[0]["FactoryWeight"].ToString();
				txtGrossWeight.Text = dtgpForGRn.Rows[0]["FactoryWeight"].ToString();
				txtwtdiff.Text = dtgpForGRn.Rows[0]["DifferenceWeight"].ToString();
				CmbCity.Value = dtgpForGRn.Rows[0]["CityId"].ToString();
				((Control)(object)CmbItem).Text = dtgpForGRn.Rows[0]["VarietyName"].ToString();
				CmbStockParty.Focus();
				loadonGdnvisibleitem(value: true);
			}
			if (DocumentTypeId == 89 && Conversion.ToString(dtgpForGRn.Rows[0]["GatepassType"]) == "AdvanceDO")
			{
				GridDetailFillFromAdvanceDeliveryOrderData(Conversion.ToInt(CmbStockParty.Value), GPID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailFillFromAdvanceDeliveryOrderData(int partyId, int gpId)
	{
		try
		{
			dtdetail.Clear();
			DataTable dt = InvDeliveryOrder.DeliveryOrder_GetDataForGdnPP(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id, UserAccount.BranchesId, partyId, gpId);
			if (dt == null || dt.Rows.Count == 0)
			{
				reset();
				MessageBox.Show("Record not found...");
				return;
			}
			DataRow firstRow = dt.Rows[0];
			DataTable dtStockParty = new DataTable();
			dtStockParty.Columns.Add("Id", typeof(int));
			dtStockParty.Columns.Add("CompanyName", typeof(string));
			dtStockParty.Rows.Add(firstRow["SupplierCustomerId"], firstRow["CustomerName"]);
			DDL.BindDDL(dtStockParty, CmbStockParty, "Id", "CompanyName", "Stock Party", ZeroIndex: false);
			CmbStockParty.Value = Conversion.ToInt(firstRow["SupplierCustomerId"]);
			foreach (DataRow row in dt.Rows)
			{
				double refDocQty = Conversion.ToDouble(row["RefDocQty"]);
				double refDocWeight = Conversion.ToDouble(row["RefDocWeight"]);
				double qty = Conversion.ToDouble(row["Qty"]);
				double netWeight = Conversion.ToDouble(row["NetWeight"]);
				double stockWeight = ((refDocQty > 0.0) ? (refDocWeight / refDocQty * qty) : 0.0);
				dtdetail.Rows.Add(0, 0, 0, 0, row["DocumentTypeId"], row["InvDeliveryOrderId"], row["InvDeliveryOrderDetailId"], row["DocNo"], row["RefDocumentTypeId"], row["RefDocIdNo"], row["RefDocSubIdNo"], row["RefDocumentType"], row["RefDocNo"], row["RefManualBillNo"], row["WarehouseId"], row["ItemId"], row["ItemName"], row["CropYearId"], row["JobLotId"], row["PackingTypeId"], row["PackUomId"], row["PackUom"], row["PackEquivalent"], row["Qty"], row["GrossWight"], row["EbUnit"], row["EbTotal"], 0, netWeight, stockWeight, netWeight, 0, "", "", "", row["RefDocQty"], row["RefDocWeight"], row["RefDocEbUnit"], row["RefDocEbTotal"]);
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gatepass()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			GatePassPartyProcessing obj = new GatePassPartyProcessing();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (DocumentTypeId == 49)
			{
				obj.DocumentTypeId = 54;
			}
			if (DocumentTypeId == 89)
			{
				obj.DocumentTypeId = 55;
			}
			DataTable dtpendingGp = GatePassPartyProcessing.GpNoPandingforInvGrnPartyProcessing(obj);
			if (dtpendingGp.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("GatepassType", typeof(string));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("Status", typeof(string));
				dt.Columns.Add("StockPartyId", typeof(int));
				dt.Columns.Add("StockParty", typeof(string));
				dt.Columns.Add("RefPartyId", typeof(int));
				dt.Columns.Add("ReferenceParty", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("SupplierWeight", typeof(double));
				dt.Columns.Add("FactoryWeight", typeof(double));
				for (int i = 0; i < dtpendingGp.Rows.Count; i++)
				{
					dt.Rows.Add(dtpendingGp.Rows[i]["Id"], dtpendingGp.Rows[i]["GatepassType"], dtpendingGp.Rows[i]["GpSrNo"], Conversion.ToDateTime(dtpendingGp.Rows[i]["GpDate"]).ToShortDateString(), dtpendingGp.Rows[i]["Status"], dtpendingGp.Rows[i]["StockPartyId"], dtpendingGp.Rows[i]["StockPartyName"], dtpendingGp.Rows[i]["RefPartyId"], dtpendingGp.Rows[i]["ReferencePartyName"], dtpendingGp.Rows[i]["VehicleNo"], dtpendingGp.Rows[i]["BiltyNo"], dtpendingGp.Rows[i]["VarietyName"], dtpendingGp.Rows[i]["ItemQty"], dtpendingGp.Rows[i]["SupplierWeight"], dtpendingGp.Rows[i]["FactoryWeight"]);
				}
				grdGp.DataSource = dt;
				grdGp.RetrieveStructure();
				GRNGridSetting();
			}
			else
			{
				grdGp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (e.Column.Key == "Load")
				{
					if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
					{
						throw new Exception("Status Not Accepted Please check status");
					}
					if (((Control)(object)CmbStockParty).Enabled && grd.GetRows().Count() == 0 && Conversion.ToInt((object)combgatepass.Text) == 0)
					{
						CmbStockParty.Value = Conversion.ToInt(grdGp.CurrentRow.Cells["StockPartyId"].Value);
						CmbRefParty.Value = Conversion.ToInt(grdGp.CurrentRow.Cells["RefPartyId"].Value);
					}
					GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
					combgatepass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpNo"].Value);
					GatePassRecordFill();
				}
				return;
			}
			throw new Exception("Please Reset the form First...");
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
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Expected O, but got Unknown
		//IL_0b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7c: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvGrnPartyProcessing obj = new InvGrnPartyProcessing();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			double GridTotalQty = GridEX_Helper.GetColumnSum(grd, "Qty");
			if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId") && GridTotalQty != GpQty)
			{
				throw new Exception($"Sum of Detail Qty '{GridTotalQty}' not equal to GpQty '{GpQty}' Please Check");
			}
			obj.DocDate = DocDate.Value;
			obj.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			obj.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
			obj.GpNo = Conversion.ToInt((object)combgatepass.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbRefParty.Value);
			obj.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			obj.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			obj.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			obj.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			obj.BiltyNo = txtbltyno.Text.Trim();
			obj.TransporterId = Conversion.ToInt(CmbTransport.Value);
			obj.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.GatePassId = Conversion.ToInt((object)GPID);
			obj.TradeToCompanyId = Conversion.ToInt(CmbTradeToCompany.Value);
			obj.ScreenName = base.Name;
			obj.DocumentTypeId = DocumentTypeId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.InvGrnPartyProcessingDetailslist = new List<InvGrnPartyProcessingDetail>();
			obj.InvGrnEmptyBagsPartyProcessingslist = new List<InvGrnEmptyBagsPartyProcessing>();
			double GrossWeight = 0.0;
			double RiceQty = 0.0;
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.InvGrnPartyProcessingDetailslist.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvGrnPartyProcessingDetail grnDetail = new InvGrnPartyProcessingDetail();
				if (Conversion.ToInt(r3.Cells["WareHouseId"].Value) == 0)
				{
					MessageBox.Show($"Warehouse Name Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToInt(r3.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show($"Item Name Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToInt(r3.Cells["CropYearId"].Value) == 0 || !DatatableHelper.DoesValueExistInColumn(dtCrop, "Id", r3.Cells["CropYearId"].Value))
				{
					MessageBox.Show($"Crop Year Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					CmbCropYear.Focus();
					return;
				}
				if (Conversion.ToInt(r3.Cells["JobId"].Value) == 0 || !DatatableHelper.DoesValueExistInColumn(dtJob, "Id", r3.Cells["JobId"].Value))
				{
					MessageBox.Show($"Job/Lot Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToInt(r3.Cells["PackingTypeId"].Value) == 0 || !DatatableHelper.DoesValueExistInColumn(dtPackType, "Id", r3.Cells["PackingTypeId"].Value))
				{
					MessageBox.Show($"Packing Type Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToInt(r3.Cells["PackUomId"].Value) == 0)
				{
					MessageBox.Show($"Pack Unit Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToDouble(r3.Cells["Qty"].Value) == 0.0)
				{
					MessageBox.Show($"Qty Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToDouble(r3.Cells["GrossWight"].Value) == 0.0)
				{
					MessageBox.Show($"Gross Weight Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToDouble(r3.Cells["NetBillWeight"].Value) == 0.0)
				{
					MessageBox.Show($"NetBillWeight Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToDouble(r3.Cells["StockWeight"].Value) == 0.0)
				{
					MessageBox.Show($"Stock Weight Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				if (Conversion.ToInt(r3.Cells["CityId"].Value) == 0)
				{
					MessageBox.Show($"City Field is Required in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				grnDetail.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				grnDetail.RefDocNoId = Conversion.ToInt(r3.Cells["RefDocIdNo"].Value);
				grnDetail.RefDocumentTypeId = Conversion.ToInt(r3.Cells["RefDocumentTypeId"].Value);
				grnDetail.RefDocSubIdNo = Conversion.ToInt(r3.Cells["RefDocSubIdNo"].Value);
				grnDetail.DeliveryOrderId = Conversion.ToInt(r3.Cells["DeliveryOrderId"].Value);
				grnDetail.DeliveryOrderDetailId = Conversion.ToInt(r3.Cells["DeliveryOrderDetailId"].Value);
				grnDetail.DoDocumentTypeId = Conversion.ToInt(r3.Cells["DoDocumentTypeId"].Value);
				grnDetail.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				grnDetail.CropYearId = Conversion.ToInt(r3.Cells["CropYearId"].Value);
				grnDetail.JobLotId = Conversion.ToInt(r3.Cells["JobId"].Value);
				grnDetail.PackingTypeId = Conversion.ToInt(r3.Cells["PackingTypeId"].Value);
				grnDetail.ItemQty = Conversion.ToDouble(r3.Cells["Qty"].Value);
				grnDetail.ItemUomId = Conversion.ToInt(r3.Cells["PackUomId"].Value);
				grnDetail.GrossWeight = Conversion.ToDouble(r3.Cells["GrossWight"].Value);
				GrossWeight += grnDetail.GrossWeight;
				RiceQty += grnDetail.ItemQty;
				grnDetail.EBWPerUnit = Conversion.ToDouble(r3.Cells["EbUnit"].Value);
				grnDetail.EBWTotal = Conversion.ToDouble(r3.Cells["EbTotal"].Value);
				grnDetail.AdLsWeight = Conversion.ToDouble(r3.Cells["AddLesswt"].Value);
				grnDetail.NetBillWeight = Conversion.ToDouble(r3.Cells["NetBillWeight"].Value);
				grnDetail.StockWeight = Conversion.ToDouble(r3.Cells["StockWeight"].Value);
				grnDetail.WarehouseId = Conversion.ToInt(r3.Cells["WareHouseId"].Value);
				grnDetail.CityId = Conversion.ToInt(r3.Cells["CityId"].Value);
				grnDetail.CommentsDetail = Conversion.ToString((object)r3.Cells["Comments"].Text.Trim());
				grnDetail.ContainerNo = Conversion.ToString((object)r3.Cells["ContainerNo"].Text.Trim());
				grnDetail.SealNo = Conversion.ToString((object)r3.Cells["SealNo"].Text.Trim());
				grnDetail.ActionTypeId = ((grnDetail.Id <= 0) ? 1 : 2);
				if (grnDetail.StockWeight > grnDetail.GrossWeight && !AllowMultipleOrPartialInvoicingForADO)
				{
					MessageBox.Show($"StockWeight cannot greater than Gross Weight '{grnDetail.GrossWeight}' Please check! in Detail Grid (Row No : {r3.RowIndex + 1} )");
					return;
				}
				obj.InvGrnPartyProcessingDetailslist.Add(grnDetail);
			}
			double Factoryweight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			bool IfDoExist = obj.InvGrnPartyProcessingDetailslist.Any((InvGrnPartyProcessingDetail r) => r.DeliveryOrderId > 0);
			if (obj.DocumentTypeId == 89 && IfDoExist)
			{
				decimal diff = Math.Abs(Conversion.ToDecimal((object)(GrossWeight - Factoryweight)));
				if (diff > PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance)
				{
					throw new Exception($"The difference of gross weight and factory weight:'{diff}' is greater than the tolerance: '{PartyProcessingGdnGrossWeightAndFactoryWeightDifferenceTolerance}' Please Check");
				}
			}
			else if (GrossWeight != Factoryweight)
			{
				throw new Exception($"Sum of GrossWeight '{GrossWeight}' not equal to FactoryWeight '{Factoryweight}' Please Check");
			}
			double EBQty = 0.0;
			rows = grdEmptyBags.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvGrnEmptyBagsPartyProcessing invGrnEmpty = new InvGrnEmptyBagsPartyProcessing();
				invGrnEmpty.DocumentTypeId = DocumentTypeId;
				invGrnEmpty.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				invGrnEmpty.ItemQty = Conversion.ToDouble(r2.Cells["Qty"].Value);
				invGrnEmpty.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				obj.InvGrnEmptyBagsPartyProcessingslist.Add(invGrnEmpty);
				EBQty += invGrnEmpty.ItemQty;
			}
			if (EBQty > RiceQty)
			{
				throw new Exception($"EmptyBags Qty '{EBQty}' cannot be greater than RiceQty '{RiceQty}'");
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrnPartyProcessing>(obj, (DocumentTypeId == 49) ? "GoodsReceivingNotesPartyProcessing" : ((DocumentTypeId == 89) ? "GoodsDispatchNotesPartyProcessing" : ""), AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvGrnPartyProcessing.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, obj.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + obj.DocNo + "]");
			}
			if ((WagesStatusForGrn && WagesActiveOrInActiveGrn && DocumentTypeId == 49) || (WagesStatusForGdn && WagesActiveOrInActiveGdn && DocumentTypeId == 89))
			{
				frmWagesBillPartyProcessing obj2 = new frmWagesBillPartyProcessing(UserAccount);
				obj2.RefDocTypeId = DocumentTypeId;
				obj2.RefDocId = success;
				obj2.GrossWeightTotal = GrossWeight;
				obj2.ShowDialog();
				obj2.BringToFront();
			}
			reset();
			if (ChkBox.Checked)
			{
				if (DocumentTypeId == 49)
				{
					CrystalReportPrint_Helper.GrnSlipPartyProcessing333(success);
				}
				else if (DocumentTypeId == 89)
				{
					CrystalReportPrint_Helper.GdnSlipPartyProcessing339(success);
				}
			}
			grd.DataSource = null;
			btnSave.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Id not found");
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
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			RecId = ID;
			lstRemoveRecord.Clear();
			InvGrnPartyProcessing obj = InvGrnPartyProcessing.GetByID(RecId);
			txtdocno.Text = obj.DocNo.ToString();
			DocDate.Value = obj.DocDate;
			tabControl1.SelectedIndex = 0;
			CmbStockParty.Value = obj.StockPartyId;
			CmbRefParty.Value = obj.SupplierCustomerId;
			CmbTransport.Value = obj.TransporterId;
			txtcarramount.Text = obj.CarriageAmount.ToString();
			txtsuppwt.Text = obj.PartyWeight.ToString();
			txtfctwt.Text = obj.FactoryWeight.ToString();
			combgatepass.Text = obj.GpNo.ToString();
			((Control)(object)combvehtyp).Text = obj.VehicleType;
			txtvehno.Text = obj.VehicleNo;
			txtbltyno.Text = obj.BiltyNo;
			txtremarks.Text = obj.RemarksHeader;
			GPID = Conversion.ToInt((object)obj.GatePassId);
			if (obj.TradeToCompanyId > 0)
			{
				CmbTradeToCompany.Value = obj.TradeToCompanyId;
			}
			dtdetail.Rows.Clear();
			FilldtDetailFromListCommonForReadById(obj, dtdetail);
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId"))
			{
				GpQty = GridEX_Helper.GetColumnSum(grd, "Qty");
				GpType = "AdvanceDO";
				DataTable dtsp = new DataTable();
				dtsp.Columns.Add("Id", typeof(int));
				dtsp.Columns.Add("CompanyName", typeof(string));
				dtsp.Rows.Add(obj.StockPartyId, obj.StockPartyName);
				DDL.BindDDL(dtsp, CmbStockParty, "Id", "CompanyName", "Stock Party", ZeroIndex: false);
				CmbStockParty.Value = obj.StockPartyId;
			}
			dtEmptyBag.Clear();
			for (int i = 0; i < obj.InvGrnEmptyBagsPartyProcessingslist.Count; i++)
			{
				dtEmptyBag.Rows.Add(obj.InvGrnEmptyBagsPartyProcessingslist[i].ItemId, obj.InvGrnEmptyBagsPartyProcessingslist[i].ItemQty, obj.InvGrnEmptyBagsPartyProcessingslist[i].Remarks);
			}
			if (dtEmptyBag.Rows.Count == 0)
			{
				AddRowInvEmptyBagsGrid();
			}
			grdEmptyBags.DataSource = dtEmptyBag;
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, (DocumentTypeId == 49) ? "GoodsReceivingNotesPartyProcessing" : ((DocumentTypeId == 89) ? "GoodsDispatchNotesPartyProcessing" : ""));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					new InvGrnPartyProcessing().DeleteById(RecId, UserAccount.ID);
					MessageBox.Show("Delete Record Successfully");
					reset();
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

	private void FilldtDetailFromListCommonForReadById(InvGrnPartyProcessing invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.InvGrnPartyProcessingDetailslist == null || invGrn.InvGrnPartyProcessingDetailslist.Count == 0)
		{
			return;
		}
		foreach (InvGrnPartyProcessingDetail detail in invGrn.InvGrnPartyProcessingDetailslist)
		{
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.RefDocumentTypeId, detail.RefDocNoId, detail.RefDocSubIdNo, detail.DoDocumentTypeId, detail.DeliveryOrderId, detail.DeliveryOrderDetailId, detail.DeliveryOrderNo, detail.InvoiceDocumentTypeId, detail.InvoiceId,
				detail.InvoiceDetailId, detail.InvoiceDocumentType, detail.InvoiceDocNo, detail.InvoiceManualBillNo, detail.WarehouseId, detail.ItemId, detail.ItemName, detail.CropYearId, detail.JobLotId, detail.PackingTypeId,
				detail.ItemUomId, detail.UOMCode, detail.Equivalent, detail.ItemQty, detail.GrossWeight, detail.EBWPerUnit, detail.EBWTotal, detail.AdLsWeight, detail.NetBillWeight, detail.StockWeight,
				detail.NetBillWeight, detail.CityId, detail.CommentsDetail, detail.ContainerNo, detail.SealNo, detail.InvoiceQty, detail.InvoiceWeight, detail.InvoiceEbUnit, detail.InvoiceEbTotal
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.WareHouseName);
				rowValues.Add(detail.CropYear);
				rowValues.Add(detail.JobLotDescription);
				rowValues.Add(detail.PackTypeDesc);
				rowValues.Add(detail.CityName);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			((Control)(object)CmbTradeToCompany).Text = string.Empty;
			RecId = 0;
			GPID = 0;
			GpQty = 0.0;
			GpType = "";
			panel5.Visible = true;
			((Control)(object)CmbStockParty).Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtwtdiff.Text = string.Empty;
			combgatepass.Text = string.Empty;
			((UltraGridBase)combvehtyp).Rows[0].Activate();
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtContainerNo.Text = string.Empty;
			txtSealNo.Text = string.Empty;
			DocDate.Focus();
			dtdetail.Clear();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtEmptyBag.Rows.Clear();
			AddRowInvEmptyBagsGrid();
			grdEmptyBags.RetrieveStructure();
			grdEmptyBagsSettings();
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			GenerateCode();
			Gatepass();
			if (DocumentTypeId == 89)
			{
				loadonGdnvisibleitem(value: false);
			}
			((Control)(object)CmbStockParty).Enabled = grd.GetRows().Count() <= 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetial()
	{
		try
		{
			txtQty.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtCommentsDetail.Text = string.Empty;
			txtContainerNo.Text = string.Empty;
			txtSealNo.Text = string.Empty;
			CmbWareHouse.Focus();
			((Control)(object)CmbStockParty).Enabled = grd.GetRows().Count() <= 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyProGrnAndGdn_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			StockPartyBind();
			ReferencePartyBind();
			VehicleTypesFill();
			TradeToCompanyFill();
			WarehouseFill();
			ItemNameFill();
			CropYear();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			CityFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void Total()
	{
		try
		{
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = Conversion.ToDouble((object)txtebu.Text);
			double EBTotal = Conversion.ToDouble((object)txtEmptyBagsTotal.Text);
			double AddLess = 0.0;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && ((Control)(object)CmbPackUom).Text != string.Empty)
			{
				Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim().ToString());
			}
			if (txtGrossWeight.Text != string.Empty && txtGrossWeight.Text != null)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim().ToString());
			}
			if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbUnit")
			{
				EBTotal = EBUnit * Qty;
				txtEmptyBagsTotal.Text = EBTotal.ToString("#,##0.###");
			}
			else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "EbTotal")
			{
				EBUnit = EBTotal / Qty;
				txtebu.Text = EBUnit.ToString("#,##0.#####");
			}
			else
			{
				EBUnit = ((Qty > 0.0) ? (EBTotal / Qty) : 0.0);
				txtebu.Text = EBUnit.ToString("#,##0.#####");
			}
			AddLess = ((!(txtadlswt.Text != string.Empty) || txtadlswt.Text == null) ? 0.0 : Conversion.ToDouble((object)txtadlswt.Text.Trim()));
			double Weight = 0.0;
			Weight = GrossWeight - EBTotal + AddLess;
			if (Weight > 0.0)
			{
				txtstockwt.Text = Math.Round(Weight, 2).ToString();
			}
			else
			{
				txtstockwt.Text = "0";
			}
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
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && ((Control)(object)CmbPackUom).Text != string.Empty)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text);
			}
			if (txtGrossWeight.Text != string.Empty)
			{
				GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text);
			}
			txtGrossWeight.Text = (PackUOM * Qty).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (txtsuppwt.Text != string.Empty && txtfctwt.Text != string.Empty)
			{
				double num = double.Parse(txtsuppwt.Text.Trim());
				double FactoryWeight = double.Parse(txtfctwt.Text.Trim());
				txtwtdiff.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		if (btnSave.Visible && btnSave.Enabled)
		{
			TotalWeight();
		}
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtEbWtOther_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtEmptyBagsTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
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
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				reset();
				RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				int Id = Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value);
				switch (Conversion.ToInt(GrdHistory.CurrentRow.Cells["DocumentTypeId"].Value))
				{
				case 49:
					CrystalReportPrint_Helper.GrnSlipPartyProcessing333(Id);
					break;
				case 89:
					CrystalReportPrint_Helper.GdnSlipPartyProcessing339(Id);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			if (DocumentTypeId == 49)
			{
				GrdHistoryDetail.RootTable.Columns["ContainerNo"].Visible = false;
				GrdHistoryDetail.RootTable.Columns["SealNo"].Visible = false;
			}
			else
			{
				GrdHistoryDetail.RootTable.Columns["ContainerNo"].Visible = true;
				GrdHistoryDetail.RootTable.Columns["SealNo"].Visible = true;
			}
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].TotalFormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].TotalFormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
			ctrlGrdBar4_Load(null, null);
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
		else if (tabControl1.SelectedIndex == 0)
		{
			DocDate.Focus();
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.DocumentTypeId = DocumentTypeId;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ModifyDateTo = ToDateHistory.Value;
				}
			}
			grn.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			grn.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			grn.StockPartyId = Conversion.ToInt(cmbStockPartyHistory.Value);
			grn.RefPartyId = Conversion.ToInt(cmbReferencePartyHistory.Value);
			if (RadReffered.Checked)
			{
				grn.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				grn.ActionId = 2;
			}
			DataTable dthis = new DataTable();
			dthis = InvGrnPartyProcessing.FormHistory(grn);
			if (dthis.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("GatePassId", typeof(int));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("StockPartyName", typeof(string));
				dt.Columns.Add("ReferencePartyName", typeof(string));
				dt.Columns.Add("TradeToCompany", typeof(string));
				dt.Columns.Add("TransporterName", typeof(string));
				dt.Columns.Add("Freight", typeof(double));
				dt.Columns.Add("VehicleType", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("PartyWeight", typeof(double));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("RemarksHeader", typeof(string));
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToShortDateString(), dthis.Rows[i]["DocNo"], dthis.Rows[i]["GatePassId"], dthis.Rows[i]["GpNo"], dthis.Rows[i]["StockPartyName"], dthis.Rows[i]["ReferencePartyName"], dthis.Rows[i]["TradeToCompany"], dthis.Rows[i]["TransporterName"], dthis.Rows[i]["CarriageAmount"], dthis.Rows[i]["VehicleType"], dthis.Rows[i]["VehicleNo"], dthis.Rows[i]["BiltyNo"], dthis.Rows[i]["PartyWeight"], dthis.Rows[i]["FactoryWeight"], dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUserName"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUserName"], dthis.Rows[i]["NoOfAttachments"], dthis.Rows[i]["RemarksHeader"]);
				}
				GrdHistory.DataSource = dt;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
				GrdHistoryDetail.ClearStructure();
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
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["GatePassId"].Visible = false;
			GrdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["PartyWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["PartyWeight"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["PartyWeight"].FormatString = "#,##0.###";
			GrdHistory.RootTable.Columns["PartyWeight"].TotalFormatString = "#,##0.###";
			GrdHistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			GrdHistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(GrdHistory);
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Width = 40;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Width = 40;
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
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
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)cmbStockPartyHistory).Text = string.Empty;
			((Control)(object)cmbReferencePartyHistory).Text = string.Empty;
			GrdHistory.ClearStructure();
			GrdHistoryDetail.ClearStructure();
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
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				DetailGridBind(CurrId);
			}
			else
			{
				GrdHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridBind(int CurrId)
	{
		try
		{
			InvGrnPartyProcessing invGrn = InvGrnPartyProcessing.GetByID(CurrId);
			if (invGrn == null)
			{
				GrdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtdetail.Clone();
			dtDetailHistory.Columns.Add("WareHouseName");
			dtDetailHistory.Columns.Add("CropYear");
			dtDetailHistory.Columns.Add("JobLot");
			dtDetailHistory.Columns.Add("PackingType");
			dtDetailHistory.Columns.Add("CityName");
			FilldtDetailFromListCommonForReadById(invGrn, dtDetailHistory, AddExtraColumns: true);
			GrdHistoryDetail.DataSource = dtDetailHistory;
			GrdHistoryDetail.RetrieveStructure();
			grdHistoryDetailSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailSettings()
	{
		try
		{
			List<string> hiddenCols = new List<string>
			{
				"Id", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "DoDocumentTypeId", "DeliveryOrderId", "DeliveryOrderNo", "InvoiceDocumentTypeId", "InvoiceId", "InvoiceDetailId",
				"InvoiceDocType", "InvoiceNo", "InvoiceManualBillNo", "ItemId", "PackUomId", "DeliveryOrderDetailId", "Equivalent", "BalWeight", "WareHouseId", "CropYearId",
				"JobId", "PackingTypeId", "CityId", "InvoiceWeight", "InvoiceQty"
			};
			GridEX_Helper.GridWrappingAndColumnSettings(GrdHistoryDetail, 2, 2, hiddenCols);
			if (DocumentTypeId == 49)
			{
				GrdHistoryDetail.RootTable.Columns["ContainerNo"].Visible = false;
				GrdHistoryDetail.RootTable.Columns["SealNo"].Visible = false;
			}
			else
			{
				GrdHistoryDetail.RootTable.Columns["ContainerNo"].Visible = true;
				GrdHistoryDetail.RootTable.Columns["SealNo"].Visible = true;
				if (GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId"))
				{
					string[] array = new string[6] { "DeliveryOrderNo", "InvoiceDocType", "InvoiceNo", "InvoiceManualBillNo", "InvoiceQty", "InvoiceWeight" };
					foreach (string colName in array)
					{
						GrdHistoryDetail.RootTable.Columns[colName].Visible = true;
					}
				}
			}
			GrdHistoryDetail.RootTable.Columns["WareHouseName"].Position = GrdHistoryDetail.RootTable.Columns["WareHouseId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["CropYear"].Position = GrdHistoryDetail.RootTable.Columns["CropYearId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["JobLot"].Position = GrdHistoryDetail.RootTable.Columns["JobId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["PackingType"].Position = GrdHistoryDetail.RootTable.Columns["PackingTypeId"].Position + 1;
			GrdHistoryDetail.RootTable.Columns["CityName"].Position = GrdHistoryDetail.RootTable.Columns["CityId"].Position + 1;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGp).Name);
			if (fs != null)
			{
				grdGp.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistoryDetail).Name);
			if (fs != null)
			{
				GrdHistoryDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			CrystalReportPrint_Helper.GrnSlipPartyProcessing333(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGdn339_Click(object sender, EventArgs e)
	{
		try
		{
			CrystalReportPrint_Helper.GdnSlipPartyProcessing339(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtebu_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcarramount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void txtEmptyBagsTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.R)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					toolStripButton1_Click(null, null);
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					btnRefreshHistory_Click(null, null);
				}
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
					btnshowHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
			{
				if (DocumentTypeId == 49)
				{
					printToolStripButton_Click(null, null);
				}
				else if (DocumentTypeId == 89)
				{
					btnGdn339_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (rdTemplate1.Checked)
					{
						if (((Control)(object)grd).Focused)
						{
							((Control)(object)grdEmptyBags).Focus();
						}
						else if (((Control)(object)grdEmptyBags).Focused)
						{
							((Control)(object)grdGp).Focus();
						}
						else if (((Control)(object)grdGp).Focused)
						{
							((Control)(object)grd).Focus();
						}
						else
						{
							((Control)(object)grd).Focus();
						}
					}
					else if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdEmptyBags).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdGp).Focus();
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)GrdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (tabControl2.SelectedIndex == 0 && ((Control)(object)grd).Focused)
					{
						CmbWareHouse.Focus();
					}
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Right && tabControl1.SelectedIndex == 0 && rdTemplate2.Checked)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					tabControl2.SelectedIndex = 1;
					((Control)(object)grdEmptyBags).Focus();
				}
				else if (tabControl2.SelectedIndex == 1)
				{
					tabControl2.SelectedIndex = 2;
					((Control)(object)grdGp).Focus();
				}
				else if (tabControl2.SelectedIndex == 2)
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grd).Focus();
				}
				else
				{
					tabControl2.SelectedIndex = 0;
					((Control)(object)grd).Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Alt && e.KeyCode == Keys.C)
			{
				CityDefine_Click(null, null);
			}
			if (e.Alt && e.KeyCode == Keys.R)
			{
				btnDefineReferenceParty_Click(null, null);
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Save in form tab and to show history in history tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Alt+C", "To Show Define City Form");
			dt.Rows.Add("Alt+R", "To Show Define Reference Party Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Warehouse when in form tab and for focus on FromDate when in history tab");
			dt.Rows.Add("Ctrl+Right", "when Template 2 toggle between Form tab grids");
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

	private void grdGp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdGp.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdGp.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdGp.CurrentColumn.Key == "Load")
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					throw new Exception("Please Reset the form First...");
				}
				if (!(Conversion.ToString(grdGp.CurrentRow.Cells["Status"].Value) == "Accepted"))
				{
					throw new Exception("Status Not Accepted Please check status");
				}
				if (((Control)(object)CmbStockParty).Enabled && grd.GetRows().Count() == 0 && Conversion.ToInt((object)combgatepass.Text) == 0)
				{
					CmbStockParty.Value = Conversion.ToInt(grdGp.CurrentRow.Cells["StockPartyId"].Value);
					CmbRefParty.Value = Conversion.ToInt(grdGp.CurrentRow.Cells["RefPartyId"].Value);
				}
				GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["Id"].Value);
				combgatepass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpNo"].Value);
				GatePassRecordFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || GrdHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formrights.DoHaveUpdateRights)
					{
						throw new Exception("You dont have update right");
					}
					reset();
					RecId = Id;
					ReadById(RecId);
				}
				else if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
				else if (GrdHistory.CurrentColumn.Key == "Print")
				{
					if (!formrights.DoHavePrintRights)
					{
						throw new Exception("you dont have print rights");
					}
					if (DocumentTypeId == 49)
					{
						CrystalReportPrint_Helper.GrnSlipPartyProcessing333(Id);
					}
					else if (DocumentTypeId == 89)
					{
						CrystalReportPrint_Helper.GdnSlipPartyProcessing339(Id);
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formrights.DoHaveUpdateRights)
				{
					throw new Exception("You dont have update right");
				}
				reset();
				RecId = Id;
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRecord(r);
				}
				else if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				else if (grd.CurrentColumn.Key == "Add")
				{
					AddDetailRow(r);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRecord(r);
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddDetailRow(r);
			}
			else if (e.KeyCode == Keys.F1 && DocumentTypeId == 89 && Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) > 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "CityId")
			{
				GrdPopUp grdPopUp = new GrdPopUp(dtcityall, "Id", "CityName");
				grdPopUp.StartPosition = FormStartPosition.CenterScreen;
				grdPopUp.ShowDialog();
				grdPopUp.BringToFront();
				int Id = Conversion.ToInt((object)grdPopUp.ReturnId);
				r.Cells["CityId"].Value = Id;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEmptyBags_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEmptyBags.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdEmptyBags.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdEmptyBags.CurrentColumn.Key == "Delete")
				{
					r.Delete();
					if (dtEmptyBag.Rows.Count == 0)
					{
						AddRowInvEmptyBagsGrid();
					}
				}
				else if (grdEmptyBags.CurrentColumn.Key == "Add")
				{
					AddRowInvEmptyBagsGrid();
				}
				grdEmptyBags.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvEmptyBagsGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineReferenceParty_Click(object sender, EventArgs e)
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

	private void BtnLookup_Click(object sender, EventArgs e)
	{
		try
		{
			frmInvLookup frmInvLookup = new frmInvLookup(UserAccount);
			frmInvLookup.cmbProfileName.Value = 17;
			frmInvLookup.ShowDialog();
			TradeToCompanyFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAvailableData_Click(object sender, EventArgs e)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DocumentTypeId == 89 && GridEX_Helper.IsAnyCellValueGreaterThanZero(grd, "DeliveryOrderId"))
			{
				LoadTransactions = new LoadavailableTransactionsForIssuancePartyProcessing(UserAccount);
				LoadTransactions.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
				LoadTransactions.btnLoadOnInvoice.Visible = false;
				LoadTransactions.ShowDialog();
				LoadTransactions.BringToFront();
				return;
			}
			if (grd.RowCount > 0)
			{
				GridEXRow r = grd.GetRow(0);
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value) == 0 && Conversion.ToInt(r.Cells["RefDocIdNo"].Value) == 0 && Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value) == 0)
				{
					throw new Exception("You can not add Record From Loader beacause manual record exist in Grid");
				}
			}
			if (((UltraGridBase)CmbStockParty).ActiveRow == null || Conversion.ToInt(CmbStockParty.Value) == 0)
			{
				CmbStockParty.Focus();
				throw new Exception("Stock Party Account select First");
			}
			LoadTransactions = new LoadavailableTransactionsForIssuancePartyProcessing(UserAccount);
			LoadTransactions.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromPurchaseInvoivce();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			if (grd.RowCount > 0 && Conversion.ToInt(CmbStockParty.Value) != Conversion.ToInt(dtpurchaseinvoice.Rows[0]["StockPartyId"]))
			{
				throw new Exception("Data against another Stock Party Already Exist");
			}
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				bool flag = false;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocIdNo"].Value) && Conversion.ToInt(dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
					{
						flag = true;
					}
				}
				if (!flag && !flag)
				{
					dtdetail.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], 0, 0, 0, 0, 0, 0, 0, "", 0, "", dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["CropYearId"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["InvPackingTypeId"], dtpurchaseinvoice.Rows[i]["ItemUomId"], dtpurchaseinvoice.Rows[i]["PackUom"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["QtyBalance"], dtpurchaseinvoice.Rows[i]["WeightBalance"], 0, 0, 0, dtpurchaseinvoice.Rows[i]["WeightBalance"], dtpurchaseinvoice.Rows[i]["WeightBalance"], 0, "", "", "", 0, 0, 0, 0);
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
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
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Expected O, but got Unknown
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Expected O, but got Unknown
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Expected O, but got Unknown
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_061d: Expected O, but got Unknown
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Expected O, but got Unknown
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0655: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Expected O, but got Unknown
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0788: Expected O, but got Unknown
		//IL_0794: Unknown result type (might be due to invalid IL or missing references)
		//IL_079e: Expected O, but got Unknown
		//IL_07c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Expected O, but got Unknown
		//IL_084f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0859: Expected O, but got Unknown
		//IL_0978: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Expected O, but got Unknown
		//IL_09af: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Expected O, but got Unknown
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Expected O, but got Unknown
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0e: Expected O, but got Unknown
		//IL_116e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Expected O, but got Unknown
		//IL_14bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c7: Expected O, but got Unknown
		//IL_1881: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Expected O, but got Unknown
		//IL_1898: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a2: Expected O, but got Unknown
		//IL_18af: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b9: Expected O, but got Unknown
		//IL_7b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b39: Expected O, but got Unknown
		//IL_7b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7b67: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEmptyBags_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PartyProcessing.PartyProGrnAndGdn));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.PanelOtherGridsMain = new System.Windows.Forms.Panel();
		this.paneGP = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.PanelEB = new System.Windows.Forms.Panel();
		this.grdEmptyBags = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.tabEB = new System.Windows.Forms.TabPage();
		this.tabGP = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.Add = new System.Windows.Forms.Button();
		this.lblContainerNo = new System.Windows.Forms.Label();
		this.lblSealNo = new System.Windows.Forms.Label();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.txtSealNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtCommentsDetail = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbItem = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.CmbCity = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbPackingtype = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtstockwt = new System.Windows.Forms.TextBox();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.rdTemplate2 = new System.Windows.Forms.RadioButton();
		this.rdTemplate1 = new System.Windows.Forms.RadioButton();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.txtwtdiff = new System.Windows.Forms.TextBox();
		this.combgatepass = new System.Windows.Forms.TextBox();
		this.CmbTransport = new UltraCombo();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.combvehtyp = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbRefParty = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.lblvehno = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblsupwt = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbStockParty = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnGdn339 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDefineReferenceParty = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAvailableData = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.lblFormCaption = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbReferencePartyHistory = new UltraCombo();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.label49 = new System.Windows.Forms.Label();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.cmbStockPartyHistory = new UltraCombo();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.lblHistory = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.CmbTradeToCompany = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.BtnLookup = new System.Windows.Forms.Button();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.PanelOtherGridsMain.SuspendLayout();
		this.paneGP.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel9.SuspendLayout();
		this.PanelEB.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).BeginInit();
		this.panel3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel20.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel23.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferencePartyHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockPartyHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTradeToCompany).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1197, 721);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1197, 721);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.PanelOtherGridsMain);
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1189, 692);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.PanelOtherGridsMain.Controls.Add(this.paneGP);
		this.PanelOtherGridsMain.Controls.Add(this.PanelEB);
		this.PanelOtherGridsMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelOtherGridsMain.Location = new System.Drawing.Point(2, 548);
		this.PanelOtherGridsMain.Name = "PanelOtherGridsMain";
		this.PanelOtherGridsMain.Size = new System.Drawing.Size(1185, 142);
		this.PanelOtherGridsMain.TabIndex = 8;
		this.paneGP.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.paneGP.Controls.Add(this.panel9);
		this.paneGP.Dock = System.Windows.Forms.DockStyle.Fill;
		this.paneGP.Location = new System.Drawing.Point(522, 0);
		this.paneGP.Name = "paneGP";
		this.paneGP.Size = new System.Drawing.Size(663, 142);
		this.paneGP.TabIndex = 6;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(663, 117);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdGp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdGp_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar3);
		this.panel9.Controls.Add(this.label6);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(663, 25);
		this.panel9.TabIndex = 11;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(623, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(5, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(130, 16);
		this.label6.TabIndex = 5;
		this.label6.Text = "Gate Pass Info Grid";
		this.PanelEB.Controls.Add((System.Windows.Forms.Control)(object)this.grdEmptyBags);
		this.PanelEB.Controls.Add(this.panel3);
		this.PanelEB.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelEB.Location = new System.Drawing.Point(0, 0);
		this.PanelEB.Name = "PanelEB";
		this.PanelEB.Size = new System.Drawing.Size(522, 142);
		this.PanelEB.TabIndex = 10;
		((ScrollableBase)this.grdEmptyBags).BackColor = System.Drawing.Color.White;
		this.grdEmptyBags.ColumnAutoResize = true;
		grdEmptyBags_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEmptyBags.DesignTimeLayout = grdEmptyBags_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEmptyBags.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Name = "grdEmptyBags";
		((ScrollableBase)this.grdEmptyBags).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).Size = new System.Drawing.Size(522, 117);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).TabIndex = 0;
		this.grdEmptyBags.TotalRow = (InheritableBoolean)1;
		this.grdEmptyBags.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEmptyBags).VisualStyle = (VisualStyle)4;
		this.grdEmptyBags.ColumnButtonClick += new ColumnActionEventHandler(grdEmptyBags_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdEmptyBags).KeyDown += new System.Windows.Forms.KeyEventHandler(grdEmptyBags_KeyDown);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label9);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(522, 25);
		this.panel3.TabIndex = 11;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(104, 16);
		this.label9.TabIndex = 4;
		this.label9.Text = "Empty Bag Grid";
		this.tabControl2.Controls.Add(this.tabDetail);
		this.tabControl2.Controls.Add(this.tabEB);
		this.tabControl2.Controls.Add(this.tabGP);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Top;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(2, 280);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1185, 268);
		this.tabControl2.TabIndex = 7;
		this.tabDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabDetail.Controls.Add(this.ctrlGrdBar1);
		this.tabDetail.Controls.Add(this.label4);
		this.tabDetail.Location = new System.Drawing.Point(4, 25);
		this.tabDetail.Name = "tabDetail";
		this.tabDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabDetail.Size = new System.Drawing.Size(1177, 239);
		this.tabDetail.TabIndex = 0;
		this.tabDetail.Text = "Detail";
		this.tabDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1171, 211);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.UpdatingCell += new UpdatingCellEventHandler(grd_UpdatingCell);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1134, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(1171, 22);
		this.label4.TabIndex = 4;
		this.label4.Text = "Detail Grid";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabEB.Location = new System.Drawing.Point(4, 25);
		this.tabEB.Name = "tabEB";
		this.tabEB.Padding = new System.Windows.Forms.Padding(3);
		this.tabEB.Size = new System.Drawing.Size(1125, 239);
		this.tabEB.TabIndex = 1;
		this.tabEB.Text = "Empty Bag";
		this.tabEB.UseVisualStyleBackColor = true;
		this.tabGP.Location = new System.Drawing.Point(4, 25);
		this.tabGP.Name = "tabGP";
		this.tabGP.Size = new System.Drawing.Size(1125, 239);
		this.tabGP.TabIndex = 2;
		this.tabGP.Text = "Gate Pass Info";
		this.tabGP.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.Add);
		this.panel5.Controls.Add(this.lblContainerNo);
		this.panel5.Controls.Add(this.lblSealNo);
		this.panel5.Controls.Add(this.txtContainerNo);
		this.panel5.Controls.Add(this.txtSealNo);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtCommentsDetail);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingtype);
		this.panel5.Controls.Add(this.label44);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Controls.Add(this.txtEmptyBagsTotal);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.txtstockwt);
		this.panel5.Controls.Add(this.txtadlswt);
		this.panel5.Controls.Add(this.txtQty);
		this.panel5.Controls.Add(this.txtebu);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 175);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1185, 105);
		this.panel5.TabIndex = 1;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(941, 72);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 28);
		this.Add.TabIndex = 16;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.lblContainerNo.AutoSize = true;
		this.lblContainerNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblContainerNo.Location = new System.Drawing.Point(687, 54);
		this.lblContainerNo.Name = "lblContainerNo";
		this.lblContainerNo.Size = new System.Drawing.Size(65, 15);
		this.lblContainerNo.TabIndex = 50;
		this.lblContainerNo.Text = "Container#";
		this.lblSealNo.AutoSize = true;
		this.lblSealNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblSealNo.Location = new System.Drawing.Point(810, 54);
		this.lblSealNo.Name = "lblSealNo";
		this.lblSealNo.Size = new System.Drawing.Size(36, 15);
		this.lblSealNo.TabIndex = 51;
		this.lblSealNo.Text = "Seal#";
		this.txtContainerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNo.Location = new System.Drawing.Point(687, 75);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.Size = new System.Drawing.Size(121, 23);
		this.txtContainerNo.TabIndex = 14;
		this.txtSealNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSealNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSealNo.Location = new System.Drawing.Point(810, 75);
		this.txtSealNo.Name = "txtSealNo";
		this.txtSealNo.Size = new System.Drawing.Size(129, 23);
		this.txtSealNo.TabIndex = 15;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(474, 54);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 47;
		this.label3.Text = "Comments";
		this.txtCommentsDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommentsDetail.Location = new System.Drawing.Point(477, 75);
		this.txtCommentsDetail.Name = "txtCommentsDetail";
		this.txtCommentsDetail.Size = new System.Drawing.Size(208, 23);
		this.txtCommentsDetail.TabIndex = 13;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1008, 73);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(60, 26);
		this.btnCancelUpdateDetial.TabIndex = 18;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(941, 73);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 17;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(192, 23);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 1;
		this.CmbItem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(combitem_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(192, 5);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(414, 23);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(108, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(687, 5);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(77, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing Type";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(889, 5);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(809, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(79, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combpckuom_Leave);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(314, 73);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 12;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(809, 5);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(63, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(6, 23);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(184, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(969, 5);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(78, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.CmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance129;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Location = new System.Drawing.Point(687, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Name = "CmbPackingtype";
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).Size = new System.Drawing.Size(121, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingtype).TabIndex = 4;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(6, 54);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(524, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(75, 54);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(154, 54);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(38, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Ad/Ls";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(315, 54);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(56, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City/Area";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(6, 5);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(67, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(524, 5);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(46, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job Lot";
		this.txtEmptyBagsTotal.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(71, 75);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(78, 23);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.Tag = "EbTotal";
		this.txtEmptyBagsTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtEmptyBagsTotal.TextChanged += new System.EventHandler(txtEmptyBagsTotal_TextChanged);
		this.txtEmptyBagsTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEmptyBagsTotal_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(207, 54);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(79, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(414, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(57, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.txtstockwt.BackColor = System.Drawing.Color.White;
		this.txtstockwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstockwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtstockwt.Location = new System.Drawing.Point(206, 75);
		this.txtstockwt.Name = "txtstockwt";
		this.txtstockwt.ReadOnly = true;
		this.txtstockwt.Size = new System.Drawing.Size(106, 23);
		this.txtstockwt.TabIndex = 11;
		this.txtstockwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtadlswt.Location = new System.Drawing.Point(151, 75);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.Size = new System.Drawing.Size(53, 23);
		this.txtadlswt.TabIndex = 10;
		this.txtadlswt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(889, 25);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(78, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtebu.BackColor = System.Drawing.Color.White;
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtebu.Location = new System.Drawing.Point(6, 75);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(64, 23);
		this.txtebu.TabIndex = 8;
		this.txtebu.Tag = "EbUnit";
		this.txtebu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged_1);
		this.txtebu.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtebu_KeyPress);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(969, 25);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(96, 23);
		this.txtGrossWeight.TabIndex = 7;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGrossWeight.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtGrossWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgwt_KeyPress);
		this.panel4.Controls.Add(this.BtnLookup);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTradeToCompany);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.rdTemplate2);
		this.panel4.Controls.Add(this.rdTemplate1);
		this.panel4.Controls.Add(this.txtsuppwt);
		this.panel4.Controls.Add(this.txtfctwt);
		this.panel4.Controls.Add(this.txtwtdiff);
		this.panel4.Controls.Add(this.combgatepass);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.panel4.Controls.Add(this.txtcarramount);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefParty);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.lblvehno);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.txtbltyno);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtvehno);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.lblsupwt);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockParty);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1185, 120);
		this.panel4.TabIndex = 0;
		this.rdTemplate2.AutoSize = true;
		this.rdTemplate2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate2.Location = new System.Drawing.Point(1069, 37);
		this.rdTemplate2.Name = "rdTemplate2";
		this.rdTemplate2.Size = new System.Drawing.Size(85, 19);
		this.rdTemplate2.TabIndex = 16;
		this.rdTemplate2.Text = "Template II";
		this.rdTemplate2.UseVisualStyleBackColor = true;
		this.rdTemplate1.AutoSize = true;
		this.rdTemplate1.Checked = true;
		this.rdTemplate1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdTemplate1.Location = new System.Drawing.Point(1069, 10);
		this.rdTemplate1.Name = "rdTemplate1";
		this.rdTemplate1.Size = new System.Drawing.Size(81, 19);
		this.rdTemplate1.TabIndex = 15;
		this.rdTemplate1.TabStop = true;
		this.rdTemplate1.Text = "Template I";
		this.rdTemplate1.UseVisualStyleBackColor = true;
		this.rdTemplate1.CheckedChanged += new System.EventHandler(rdTemplate1_CheckedChanged);
		this.txtsuppwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsuppwt.Location = new System.Drawing.Point(919, 8);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(146, 23);
		this.txtsuppwt.TabIndex = 12;
		this.txtsuppwt.TabStop = false;
		this.txtsuppwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsuppwt.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.txtfctwt.BackColor = System.Drawing.SystemColors.Control;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfctwt.Location = new System.Drawing.Point(919, 35);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(146, 23);
		this.txtfctwt.TabIndex = 13;
		this.txtfctwt.TabStop = false;
		this.txtfctwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfctwt.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.txtwtdiff.BackColor = System.Drawing.SystemColors.Control;
		this.txtwtdiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtdiff.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtwtdiff.Location = new System.Drawing.Point(919, 63);
		this.txtwtdiff.Name = "txtwtdiff";
		this.txtwtdiff.ReadOnly = true;
		this.txtwtdiff.Size = new System.Drawing.Size(146, 23);
		this.txtwtdiff.TabIndex = 14;
		this.txtwtdiff.TabStop = false;
		this.txtwtdiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.combgatepass.BackColor = System.Drawing.SystemColors.Control;
		this.combgatepass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.combgatepass.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combgatepass.Location = new System.Drawing.Point(367, 63);
		this.combgatepass.Name = "combgatepass";
		this.combgatepass.ReadOnly = true;
		this.combgatepass.Size = new System.Drawing.Size(245, 23);
		this.combgatepass.TabIndex = 7;
		this.combgatepass.TabStop = false;
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(367, 6);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 5;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcarramount.Location = new System.Drawing.Point(367, 35);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(245, 23);
		this.txtcarramount.TabIndex = 6;
		this.txtcarramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(690, 6);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		this.combvehtyp.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(147, 26);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabStop = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(296, 67);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(616, 67);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(296, 94);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.CmbRefParty.AllowNull = (DefaultableBoolean)1;
		this.CmbRefParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRefParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefParty).Location = new System.Drawing.Point(73, 61);
		((System.Windows.Forms.Control)(object)this.CmbRefParty).Name = "CmbRefParty";
		((System.Windows.Forms.Control)(object)this.CmbRefParty).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefParty).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 67);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(54, 15);
		this.label1.TabIndex = 8;
		this.label1.Text = "Ref Party";
		this.lblvehno.AutoSize = true;
		this.lblvehno.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblvehno.Location = new System.Drawing.Point(616, 39);
		this.lblvehno.Name = "lblvehno";
		this.lblvehno.Size = new System.Drawing.Size(64, 15);
		this.lblvehno.TabIndex = 0;
		this.lblvehno.Text = "Vehicle No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(296, 12);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(616, 12);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(296, 39);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(70, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight Amt";
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbltyno.Location = new System.Drawing.Point(690, 63);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.ReadOnly = true;
		this.txtbltyno.Size = new System.Drawing.Size(147, 23);
		this.txtbltyno.TabIndex = 11;
		this.txtbltyno.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(841, 67);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight Diff";
		this.txtvehno.BackColor = System.Drawing.SystemColors.Control;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehno.Location = new System.Drawing.Point(690, 35);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.ReadOnly = true;
		this.txtvehno.Size = new System.Drawing.Size(147, 23);
		this.txtvehno.TabIndex = 10;
		this.txtvehno.TabStop = false;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(367, 90);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(698, 23);
		this.txtremarks.TabIndex = 8;
		this.lblsupwt.AutoSize = true;
		this.lblsupwt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblsupwt.Location = new System.Drawing.Point(841, 12);
		this.lblsupwt.Name = "lblsupwt";
		this.lblsupwt.Size = new System.Drawing.Size(77, 15);
		this.lblsupwt.TabIndex = 0;
		this.lblsupwt.Text = "Supp Weight";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(841, 39);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 39);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Stock Party";
		this.CmbStockParty.AllowNull = (DefaultableBoolean)1;
		this.CmbStockParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStockParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Location = new System.Drawing.Point(73, 33);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Name = "CmbStockParty";
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Size = new System.Drawing.Size(221, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).TabIndex = 2;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(163, 8);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(131, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(73, 8);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(88, 23);
		this.txtdocno.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.ChkBox);
		this.panel2.Controls.Add(this.lblFormCaption);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1185, 53);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnNew, this.toolStripButton1, this.toolStripSeparator2, this.btnSave, this.btnUpdate, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.printToolStripButton,
			this.btnGdn339, this.toolStripSeparator5, this.CityDefine, this.toolStripSeparator1, this.btnDefineReferenceParty, this.btnLoadAvailableData, this.BtnShortCutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1185, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(92, 24);
		this.printToolStripButton.Text = "333-Print";
		this.printToolStripButton.ToolTipText = "Ctrl_P";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnGdn339.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGdn339.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGdn339.Name = "btnGdn339";
		this.btnGdn339.Size = new System.Drawing.Size(87, 24);
		this.btnGdn339.Text = "339-Gdn";
		this.btnGdn339.Visible = false;
		this.btnGdn339.Click += new System.EventHandler(btnGdn339_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.CityDefine.Image = Architecture.WinApp.Properties.Resources.Export;
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(57, 24);
		this.CityDefine.Text = "City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnDefineReferenceParty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDefineReferenceParty.Image = (System.Drawing.Image)resources.GetObject("btnDefineReferenceParty.Image");
		this.btnDefineReferenceParty.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineReferenceParty.Name = "btnDefineReferenceParty";
		this.btnDefineReferenceParty.Size = new System.Drawing.Size(163, 24);
		this.btnDefineReferenceParty.Text = "Define Reference Party";
		this.btnDefineReferenceParty.Click += new System.EventHandler(btnDefineReferenceParty_Click);
		this.btnLoadAvailableData.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnLoadAvailableData.Image = (System.Drawing.Image)resources.GetObject("btnLoadAvailableData.Image");
		this.btnLoadAvailableData.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAvailableData.Name = "btnLoadAvailableData";
		this.btnLoadAvailableData.Size = new System.Drawing.Size(98, 24);
		this.btnLoadAvailableData.Text = "Load Data";
		this.btnLoadAvailableData.Click += new System.EventHandler(btnLoadAvailableData_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1058, 31);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(122, 20);
		this.ChkBox.TabIndex = 18;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.lblFormCaption.AutoSize = true;
		this.lblFormCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFormCaption.ForeColor = System.Drawing.Color.White;
		this.lblFormCaption.Location = new System.Drawing.Point(5, 30);
		this.lblFormCaption.Name = "lblFormCaption";
		this.lblFormCaption.Size = new System.Drawing.Size(260, 19);
		this.lblFormCaption.TabIndex = 3;
		this.lblFormCaption.Text = "Goods Receiving Notes             ";
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel20);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel23);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1189, 692);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 488);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1185, 202);
		this.panel8.TabIndex = 54;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1185, 202);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label32);
		this.panel20.Controls.Add(this.ctrlGrdBar4);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(2, 464);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1185, 24);
		this.panel20.TabIndex = 53;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 1);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(242, 19);
		this.label32.TabIndex = 24;
		this.label32.Text = "Detail of above selected row";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1145, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.GrdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 125);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1185, 339);
		this.panel6.TabIndex = 52;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1185, 339);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.panel23.Controls.Add(this.rdmodifydate);
		this.panel23.Controls.Add(this.drdocdate);
		this.panel23.Controls.Add(this.rdentrydate);
		this.panel23.Controls.Add(this.groupBox3);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(2, 55);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1185, 70);
		this.panel23.TabIndex = 51;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1027, 45);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 12;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1027, 2);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 10;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1027, 23);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 11;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory);
		this.groupBox3.Controls.Add(this.RadNotReffered);
		this.groupBox3.Controls.Add(this.RadReffered);
		this.groupBox3.Controls.Add(this.btnshowHistory);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.RadAll);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label51);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1016, 66);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(648, 18);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(89, 15);
		this.label8.TabIndex = 144;
		this.label8.Text = "Reference Party";
		this.cmbReferencePartyHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbReferencePartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferencePartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferencePartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferencePartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReferencePartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory).Location = new System.Drawing.Point(648, 36);
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory).Name = "cmbReferencePartyHistory";
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferencePartyHistory).TabIndex = 143;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(909, 43);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 8;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(909, 25);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 7;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(853, 36);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(55, 26);
		this.btnshowHistory.TabIndex = 5;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(434, 19);
		this.label49.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(67, 15);
		this.label49.TabIndex = 142;
		this.label49.Text = "Stock Party";
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(909, 7);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(44, 23);
		this.RadAll.TabIndex = 6;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(350, 19);
		this.label50.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(64, 15);
		this.label50.TabIndex = 141;
		this.label50.Text = "To Doc No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(270, 19);
		this.label51.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(79, 15);
		this.label51.TabIndex = 140;
		this.label51.Text = "From Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(136, 19);
		this.label52.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(48, 15);
		this.label52.TabIndex = 139;
		this.label52.Text = "To Date";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(5, 20);
		this.label53.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(63, 15);
		this.label53.TabIndex = 13;
		this.label53.Text = "From Date";
		this.cmbStockPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbStockPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStockPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory).Location = new System.Drawing.Point(434, 36);
		((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory).Name = "cmbStockPartyHistory";
		((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockPartyHistory).TabIndex = 4;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(350, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(83, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(270, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(79, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(136, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(133, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar2);
		this.panel18.Controls.Add(this.lblHistory);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(2, 30);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1185, 25);
		this.panel18.TabIndex = 46;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1145, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.lblHistory.AutoSize = true;
		this.lblHistory.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHistory.ForeColor = System.Drawing.Color.White;
		this.lblHistory.Location = new System.Drawing.Point(4, 2);
		this.lblHistory.Name = "lblHistory";
		this.lblHistory.Size = new System.Drawing.Size(263, 19);
		this.lblHistory.TabIndex = 24;
		this.lblHistory.Text = "Goods Receiving Notes  History";
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1185, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1185, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Visible = false;
		this.CmbTradeToCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbTradeToCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTradeToCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTradeToCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTradeToCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTradeToCompany).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTradeToCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTradeToCompany).Location = new System.Drawing.Point(73, 88);
		((System.Windows.Forms.Control)(object)this.CmbTradeToCompany).Name = "CmbTradeToCompany";
		((System.Windows.Forms.Control)(object)this.CmbTradeToCompany).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbTradeToCompany).TabIndex = 4;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(6, 88);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(61, 30);
		this.label10.TabIndex = 102;
		this.label10.Text = "Trade to Company";
		this.BtnLookup.BackColor = System.Drawing.Color.White;
		this.BtnLookup.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.BtnLookup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnLookup.Location = new System.Drawing.Point(272, 87);
		this.BtnLookup.Margin = new System.Windows.Forms.Padding(2);
		this.BtnLookup.Name = "BtnLookup";
		this.BtnLookup.Size = new System.Drawing.Size(22, 27);
		this.BtnLookup.TabIndex = 103;
		this.BtnLookup.TabStop = false;
		this.BtnLookup.UseVisualStyleBackColor = false;
		this.BtnLookup.Click += new System.EventHandler(BtnLookup_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1197, 721);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "PartyProGrnAndGdn";
		this.Text = "   ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PartyProGrnAndGdn_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.PanelOtherGridsMain.ResumeLayout(false);
		this.paneGP.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.PanelEB.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdEmptyBags).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReferencePartyHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockPartyHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTradeToCompany).EndInit();
		base.ResumeLayout(false);
	}
}

}
