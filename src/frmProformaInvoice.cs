using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Import
{

public class frmProformaInvoice : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private long RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private int updateDetailIndex = -1;

	private int updateDetailIndexOtherItem = -1;

	private int updateDetailIndexPT = -1;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int AppActionId;

	private long RowVersionLong;

	private DataTable dtdetail = new DataTable();

	private DataTable dtPerformaNo = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtFiList = new DataTable();

	private bool BranchFeature;

	private List<ProformaDetail> LstRemoveRecordDetail = new List<ProformaDetail>();

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private GroupBox HeaderGropBox;

	internal UltraCombo CmbExporterName;

	private Label label21;

	private Label label1;

	internal GridEX grdDetail;

	private Panel panel10;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel14;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbPaymentTerm;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo CmbLCContractor;

	internal UltraCombo cmbCarierType;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbEFormNo;

	internal UltraCombo cmbdeliveryterm;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	private Label label56;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	private Label label16;

	private TextBox txtfcyAmountHeader;

	private Label label25;

	private Label label27;

	internal UltraCombo CmbFcyCode;

	private Label label49;

	private TextBox txtGrossWeightHeader;

	private Label label54;

	private TextBox txtNetWeightHeader;

	private Label label55;

	private TextBox txtNoOfContainerHeader;

	private Label label57;

	private Label label53;

	private Label label50;

	private Label label60;

	internal GridEX gridDetailHistory;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel22;

	private Panel panel23;

	private Panel panel17;

	private Panel panel11;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TextBox txtitemDescription;

	private Label label34;

	private TextBox txtCostMTon;

	private Label label29;

	private TextBox txtNoOfBags;

	private Label label30;

	internal UltraCombo CmbItemName;

	private Label label32;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	internal UltraCombo CmbPackUom;

	private Label label36;

	private TextBox txtAmount;

	private Label label37;

	private Label label38;

	internal UltraCombo CmbRateUom;

	private Label label85;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKey;

	private Panel panel15;

	private Label label69;

	private Panel panel3;

	internal GridEX DataGridHistory;

	private Label label92;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label20;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label93;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label94;

	internal UltraCombo CmbJobLot;

	private Label label109;

	internal UltraCombo CmbImporterName;

	private DateTimePicker txtPerformaDate;

	internal UltraCombo CmbPerformaNo;

	private Label label98;

	private Label label101;

	private Label label96;

	internal UltraCombo CmbIncomeTerm;

	private Label label107;

	private Button BtnDefinePerformaNo;

	private TextBox txtWeightDetail;

	private Label label2;

	private TextBox txtRemarksDetail;

	private Label label19;

	private Label txtDocNo;

	private TextBox DocNo;

	private TextBox txtQtyMTon;

	private Label label35;

	internal UltraCombo CmbBranchName;

	private Label labelBranchName;

	private Label label6;

	private Label label51;

	internal UltraCombo CmbPaymentTermsNew;

	private Label label11;

	private Label label5;

	private DateTimePicker datValidityUpto;

	private Label label3;

	private TextBox txtTermsAndConditions;

	private Label label10;

	private Label label9;

	private Label label8;

	internal UltraCombo CmbGoodsOrigin;

	private Label label7;

	private Label label12;

	public frmProformaInvoice(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		base.Load += ImProformaInvoice_Load;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtPerformaNo.Columns.Add("Id", typeof(long));
			dtPerformaNo.Columns.Add("ProformaNo", typeof(string));
			dtPerformaNo.Columns.Add("ProformaDate", typeof(DateTime));
			dtdetail.Columns.Add("Id", typeof(long));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemDetail");
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId", typeof(int));
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("PackEquivalent", typeof(double));
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("RateUOMId", typeof(int));
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("RateEquivalent", typeof(double));
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("RowVersionLong", typeof(long));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnupdate.Visible = false;
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
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			BranchFeature = CommonServices.GetERPFeatureById(11);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			string documentNoData = generateCode();
			DataTable suppCustForExportData = BindImporterAndExporter();
			DataTable proformaNoData = BindProformaNo();
			DataTable importRelatedData = BindImportRelatedCombo();
			DataTable currencyData = BindMultiCurrency();
			DataTable itemData = CommonServices.ItemGetForComboServiceBind();
			DataTable jobLotData = CommonServices.JobLotGetAllService();
			DataTable countryData = BindGoodsOrigin();
			return new
			{
				formrights = formrights,
				documentNoData = documentNoData,
				SuppCustForExportData = suppCustForExportData,
				proformaNoData = proformaNoData,
				ImportRelatedData = importRelatedData,
				CurrencyData = currencyData,
				ItemData = itemData,
				JobLotData = jobLotData,
				CountryData = countryData
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
				AutoScroll = true;
				base.AutoScrollMargin = new Size(20, 20);
				base.AutoScrollMinSize = new Size(1200, 700);
				formrights = result.formrights;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				btnPrint.Enabled = formrights.DoHavePrintRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
				ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
				ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
				((Control)(object)CmbBranchName).Visible = true;
				labelBranchName.Visible = true;
				if (RecId == 0L)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BranchFill();
				ProformaNoFill(result.proformaNoData);
				ImporterAndExporterFill(result.SuppCustForExportData);
				ImportRelatedComboFill(result.ImportRelatedData);
				MultiCurrencyFill(result.CurrencyData);
				ItemFill(result.ItemData);
				JobLotFill(result.JobLotData);
				GoodsOriginFill(result.CountryData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void ImProformaInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbImporterName).Enabled = true;
			((Control)(object)CmbExporterName).Enabled = true;
			((Control)(object)CmbItemName).Enabled = true;
			((Control)(object)cmbFCurrencr).Enabled = true;
			((Control)(object)CmbFcyCode).Enabled = true;
			HistoryCombosFill();
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
			List<getGlobalBranchesAllocateToUser> lst = clsGlobalVariables.globalBranchesAllocateToUser;
			int id = Conversion.ToInt(CmbBranchName.Value);
			if (lst.Count > 0)
			{
				DataTable dtBranch = new DataTable();
				dtBranch.Columns.Add("BranchId", typeof(int));
				dtBranch.Columns.Add("BranchName", typeof(string));
				for (int i = 0; i < lst.Count; i++)
				{
					dtBranch.Rows.Add(lst[i].BranchId, lst[i].BranchName);
				}
				DDL.BindDDL(dtBranch, CmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				if (id > 0)
				{
					if (dtBranch.AsEnumerable().Any((DataRow row) => row.Field<int>("BranchId") == id))
					{
						CmbBranchName.Value = id;
					}
					else
					{
						((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
					}
				}
				else
				{
					((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
				}
			}
			else
			{
				((Control)(object)CmbBranchName).Text = string.Empty;
				((UltraGridBase)CmbBranchName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string generateCode()
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
			return Conversion.ToString((object)ProformaMaster.GenerateCode(new ProformaMaster
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				documentTypeId = 900,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		DocNo.Text = documentData;
	}

	private DataTable BindProformaNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			return ProformaMaster.GetProformaNoFromMasterDocumentSerial(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RecId = RecId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ProformaNoFill(DataTable dt)
	{
		try
		{
			long id = Conversion.ToInt(CmbPerformaNo.Value);
			if (dt.Rows.Count > 0)
			{
				dtPerformaNo.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPerformaNo.Rows.Add(dt.Rows[i]["proformaMasterId"], dt.Rows[i]["proformaNo"], dt.Rows[i]["proformaDate"]);
				}
			}
			if (dtPerformaNo != null && dtPerformaNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtPerformaNo, CmbPerformaNo, "Id", "ProformaNo", "Proforma No", ZeroIndex: false);
				if (id > 0)
				{
					if (dtPerformaNo.AsEnumerable().Any((DataRow row) => row.Field<long>("Id") == id))
					{
						CmbPerformaNo.Value = id;
					}
					else
					{
						((Control)(object)CmbPerformaNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPerformaNo).DataSource = null;
				((Control)(object)CmbPerformaNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindImporterAndExporter()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
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

	private void ImporterAndExporterFill(DataTable dtSupplier)
	{
		try
		{
			int id = Conversion.ToInt(CmbImporterName.Value);
			Conversion.ToInt(CmbExporterName.Value);
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, CmbImporterName, "Id", "CompanyName", "Importer Name", false);
				if (id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbImporterName.Value = id;
					}
					else
					{
						((Control)(object)CmbImporterName).Text = string.Empty;
					}
				}
				DDL.BindDDLNew(dtSupplier, CmbExporterName, "Id", "CompanyName", "Importer Name", false);
				if (id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbExporterName.Value = id;
					}
					else
					{
						((Control)(object)CmbExporterName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbExporterName).DataSource = null;
				((UltraGridBase)CmbImporterName).DataSource = null;
				((Control)(object)CmbExporterName).Text = string.Empty;
				((Control)(object)CmbImporterName).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindImportRelatedCombo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return invoiceMaster.GetDataForDropDownFrom_ImportRelated(new ReportsParameters
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

	private void ImportRelatedComboFill(DataTable dt)
	{
		try
		{
			int id1 = Conversion.ToInt(CmbIncomeTerm.Value);
			int id2 = Conversion.ToInt(cmbPaymentTerm.Value);
			int id3 = Conversion.ToInt(CmbPaymentTermsNew.Value);
			int id4 = Conversion.ToInt(cmbLoadingPort.Value);
			int id5 = Conversion.ToInt(cmbDestinationPort.Value);
			DataTable dtInco = new DataTable();
			dtInco.Columns.Add("Id", typeof(int));
			dtInco.Columns.Add("Name");
			DataTable dtPayterm = new DataTable();
			dtPayterm.Columns.Add("Id", typeof(int));
			dtPayterm.Columns.Add("Name");
			DataTable dtSeaAirport = new DataTable();
			dtSeaAirport.Columns.Add("Id", typeof(int));
			dtSeaAirport.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "IncoTerm")
					{
						dtInco.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PaymentTermTitle")
					{
						dtPayterm.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SeaAirport")
					{
						dtSeaAirport.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			if (dtInco.Rows.Count > 0)
			{
				DDL.BindDDL(dtInco, CmbIncomeTerm, "Id", "Name", "Inco Term", ZeroIndex: true);
				((UltraGridBase)CmbIncomeTerm).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (id1 > 0)
				{
					if (dtInco.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id1))
					{
						CmbIncomeTerm.Value = id1;
					}
					else
					{
						((Control)(object)CmbIncomeTerm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbIncomeTerm).Text = string.Empty;
				((UltraGridBase)CmbIncomeTerm).DataSource = null;
			}
			if (dtPayterm.Rows.Count > 0)
			{
				DDL.BindDDL(dtPayterm, cmbPaymentTerm, "Id", "Name", "Payment Term", ZeroIndex: true);
				((UltraGridBase)cmbPaymentTerm).DisplayLayout.Bands[0].Columns[1].Width = 200;
				if (id2 > 0)
				{
					if (dtPayterm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id2))
					{
						cmbPaymentTerm.Value = id2;
					}
					else
					{
						((Control)(object)cmbPaymentTerm).Text = string.Empty;
					}
				}
				DDL.BindDDL(dtPayterm, CmbPaymentTermsNew, "Id", "Name", "Payment Term", ZeroIndex: true);
				((UltraGridBase)CmbPaymentTermsNew).DisplayLayout.Bands[0].Columns[1].Width = 200;
				if (id3 > 0)
				{
					if (dtPayterm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id3))
					{
						CmbPaymentTermsNew.Value = id3;
					}
					else
					{
						((Control)(object)CmbPaymentTermsNew).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbPaymentTerm).Text = string.Empty;
				((UltraGridBase)cmbPaymentTerm).DataSource = null;
				((Control)(object)CmbPaymentTermsNew).Text = string.Empty;
				((UltraGridBase)CmbPaymentTermsNew).DataSource = null;
			}
			if (dtSeaAirport.Rows.Count > 0)
			{
				DDL.BindDDL(dtSeaAirport, cmbLoadingPort, "Id", "Name", "loading port", ZeroIndex: true);
				if (id4 > 0)
				{
					if (dtSeaAirport.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id4))
					{
						cmbLoadingPort.Value = id4;
					}
					else
					{
						((Control)(object)cmbLoadingPort).Text = string.Empty;
					}
				}
				DDL.BindDDL(dtSeaAirport, cmbDestinationPort, "Id", "Name", "Destination port", ZeroIndex: true);
				if (id5 > 0)
				{
					if (dtSeaAirport.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id5))
					{
						cmbDestinationPort.Value = id5;
					}
					else
					{
						((Control)(object)cmbDestinationPort).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLoadingPort).Text = string.Empty;
				((UltraGridBase)cmbLoadingPort).DataSource = null;
				((Control)(object)cmbDestinationPort).Text = string.Empty;
				((UltraGridBase)cmbDestinationPort).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable BindGoodsOrigin()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return country.GetAll(new Country
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

	private void GoodsOriginFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbGoodsOrigin.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbGoodsOrigin, "Id", "Description", "Goods Origin", true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbGoodsOrigin.Value = id;
					}
					else
					{
						((Control)(object)CmbGoodsOrigin).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbGoodsOrigin).Text = string.Empty;
				((UltraGridBase)CmbGoodsOrigin).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable BindMultiCurrency()
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

	private void MultiCurrencyFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbFcyCode.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbFcyCode, "Id", "CurrencyName", "CurrencyName", true);
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbFcyCode.Value = id;
					}
					else
					{
						((Control)(object)CmbFcyCode).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbFcyCode).Text = string.Empty;
				((UltraGridBase)CmbFcyCode).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbItemName.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 400;
				if (id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						CmbItemName.Value = id;
					}
					else
					{
						((Control)(object)CmbItemName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotFill(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbJobLot.Value);
			DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "Job Lot", true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					CmbJobLot.Value = id;
				}
				else
				{
					((Control)(object)CmbJobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			string packUom = ((Control)(object)CmbPackUom).Text;
			string RateUom = ((Control)(object)CmbRateUom).Text;
			DataTable dtUOM = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItemName.Value));
			DDL.BindDDL(dtUOM, CmbPackUom, "Id", "UOMCode", "Pack Uom", ZeroIndex: true);
			DDL.BindDDL(dtUOM, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: true);
			if (packUom != null && packUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == packUom))
				{
					((Control)(object)CmbPackUom).Text = packUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			if (RateUom != null && RateUom != string.Empty)
			{
				if (dtUOM.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == RateUom))
				{
					((Control)(object)CmbRateUom).Text = RateUom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		AT = new Attachment();
		Attachment.dtAttachments.Rows.Clear();
		LstRemoveRecordDetail.Clear();
		AttachmentsValues = "";
		CustomAttachmentsValues = "";
		RowVersionLong = 0L;
		btnsave.Visible = true;
		btnupdate.Visible = false;
		UpdateMode = false;
		RecId = 0L;
		((Control)(object)CmbImporterName).Text = string.Empty;
		((Control)(object)CmbExporterName).Text = string.Empty;
		((Control)(object)CmbPerformaNo).Text = string.Empty;
		txtPerformaDate.Value = DateTime.Now;
		((Control)(object)CmbGoodsOrigin).Text = string.Empty;
		((Control)(object)cmbLoadingPort).Text = string.Empty;
		((Control)(object)cmbDestinationPort).Text = string.Empty;
		((Control)(object)CmbIncomeTerm).Text = string.Empty;
		txtNetWeightHeader.Text = string.Empty;
		txtGrossWeightHeader.Text = string.Empty;
		txtNoOfContainerHeader.Text = string.Empty;
		CmbPaymentTermsNew.Value = 0;
		CmbFcyCode.Value = 0;
		txtfcyAmountHeader.Text = string.Empty;
		UpdateDocumentNoUI(generateCode());
		ProformaNoFill(BindProformaNo());
		((Control)(object)CmbPerformaNo).Enabled = true;
		dtdetail.Rows.Clear();
		grdDetail.ClearStructure();
		grdDetail.DataSource = dtdetail;
		grdDetail.RetrieveStructure();
		grdSettings();
		DetailFormReset();
		if (BranchFeature)
		{
			CmbBranchName.Focus();
		}
		else
		{
			datValidityUpto.Focus();
		}
	}

	private void DetailFormReset()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbItemName).Text = string.Empty;
			txtQtyMTon.Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtNoOfBags.Text = string.Empty;
			txtCostMTon.Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtitemDescription.Text = string.Empty;
			btnAddinGrid.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			tabControl2.SelectedIndex = 0;
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
			List<string> allServices = new List<string> { "BranchesAllocatedToUser" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BranchFill();
			ProformaNoFill(BindProformaNo());
			ImporterAndExporterFill(BindImporterAndExporter());
			ImportRelatedComboFill(BindImportRelatedCombo());
			MultiCurrencyFill(BindMultiCurrency());
			ItemFill(CommonServices.ItemGetForComboServiceBind());
			JobLotFill(CommonServices.JobLotGetAllService());
			GoodsOriginFill(BindGoodsOrigin());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmImportInvoice_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnCancel_Leave(object sender, EventArgs e)
	{
		try
		{
			_ = base.ActiveControl.Name == "tabControl1";
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, Conversion.ToInt(CmbItemName.Value), Conversion.ToString((object)((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Text), Conversion.ToString((object)((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Text), Conversion.ToString((object)txtitemDescription.Text.Trim()), Conversion.ToString(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text.Trim()), Conversion.ToDouble((object)txtQtyMTon.Text.Trim()), Conversion.ToString(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text.Trim()), Conversion.ToString(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtNoOfBags.Text.Trim()), Conversion.ToDouble((object)txtWeightDetail.Text.Trim()), Conversion.ToDouble((object)txtCostMTon.Text.Trim()), Conversion.ToString(CmbRateUom.Value), Conversion.ToString((object)((Control)(object)CmbRateUom).Text.Trim()), Conversion.ToString((object)((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Text), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToString((object)txtRemarksDetail.Text.Trim()), 0);
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				getTotalnetWeightFromGrid();
				DetailFormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbItemName.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbItemName).Text;
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value;
				dtdetail.Rows[updateDetailIndex]["ItemDetail"] = txtitemDescription.Text;
				dtdetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtdetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				dtdetail.Rows[updateDetailIndex]["Qty/M.Ton"] = txtQtyMTon.Text;
				dtdetail.Rows[updateDetailIndex]["PackSizeId"] = CmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["PackSize"] = ((Control)(object)CmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["PackEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
				dtdetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtWeightDetail.Text;
				dtdetail.Rows[updateDetailIndex]["Cost/M.Ton"] = txtCostMTon.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUom).Text;
				dtdetail.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value);
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtAmount.Text;
				grdDetail.DataSource = dtdetail;
				getTotalnetWeightFromGrid();
				DetailFormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Expected O, but got Unknown
		try
		{
			grdDetail.AllowEdit = (InheritableBoolean)1;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetail.RootTable.Columns["PackEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetail.RootTable.Columns["RateEquivalent"].Visible = false;
			grdDetail.RootTable.Columns["RowVersionLong"].Visible = false;
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["ItemDetail"].Width = Constants.InventoryConstants.OtherDescription;
			grdDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["Qty/M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["PackEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["Cost/M.Ton"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["RateUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["RateEquivalent"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom();
				txtitemDescription.Text = item.Cells["ItemDetail"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value;
				CmbPackUom.Value = item.Cells["PackSizeId"].Value;
				txtQtyMTon.Text = item.Cells["Qty/M.Ton"].Value.ToString();
				txtNoOfBags.Text = item.Cells["NoOfBags"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUOMId"].Value;
				txtCostMTon.Text = item.Cells["Cost/M.Ton"].Value.ToString();
				txtAmount.Text = item.Cells["Amount"].Value.ToString();
				btnAddinGrid.Visible = false;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailGridRow(r);
				}
				else if (e.Column.Key == "Add")
				{
					AddDetailGridRow(r);
				}
				grdDetail.UpdateData();
				getTotalnetWeightFromGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailGridRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (grdDetail.GetRows().Count() <= 1)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						ProformaDetail vd = new ProformaDetail();
						vd.proformaDetailId = Conversion.ToInt64(r.Cells["Id"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemDescription = Conversion.ToString((object)r.Cells["ItemDetail"].Text);
						vd.lotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.packSizeIdinner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.packSizeIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
						vd.qtyInner = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						vd.qtyOuter = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						vd.netWeightInner = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) / 1000.0;
						vd.netWeightOuter = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) / 1000.0;
						vd.ItemRate = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
						vd.uomIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
						vd.isApproved = false;
						vd.createdOn = DateTime.Now;
						vd.createdUserId = UserAccount.ID;
						vd.lastModifiedOn = DateTime.Now;
						vd.lastModifiedUserId = UserAccount.ID;
						vd.approvedOn = DateTime.Now;
						vd.appActionId = 3;
						LstRemoveRecordDetail.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					r.Delete();
				}
				return;
			}
			throw new Exception("Reset the Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddDetailGridRow(GridEXRow r)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				DataRow dr = dtdetail.Rows[r.RowIndex];
				dtdetail.Rows.Add(dr.ItemArray);
				dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
				grdDetail.DataSource = dtdetail;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				_ = r.Cells.Count;
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getTotalnetWeightFromGrid()
	{
		try
		{
			double NetWeightkg = 0.0;
			object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			double Amount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			NetWeightkg = Conversion.ToDouble(total) * 1000.0;
			txtNetWeightHeader.Text = Conversion.ToDouble((object)NetWeightkg).ToString("#,##0.#####");
			txtfcyAmountHeader.Text = Amount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (BranchFeature && (((UltraGridBase)CmbBranchName).ActiveRow == null || Conversion.ToInt(CmbBranchName.Value) == 0))
		{
			MessageBox.Show("BranchName Is required");
			CmbBranchName.Focus();
			return false;
		}
		if (DocNo.Text == "" || Conversion.ToInt((object)DocNo.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			DocNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbImporterName.Value) == 0)
		{
			MessageBox.Show("Importer Is Required");
			CmbImporterName.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbExporterName.Value) == 0)
		{
			MessageBox.Show("Exporter Is Required");
			CmbExporterName.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPerformaNo.Value) == 0)
		{
			MessageBox.Show("Proforma No Is Required");
			CmbPerformaNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbGoodsOrigin.Value) == 0)
		{
			MessageBox.Show("Goods Origin Is Required");
			CmbGoodsOrigin.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Loading Port Is Required");
			cmbLoadingPort.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Destination Port Is Required");
			cmbDestinationPort.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbIncomeTerm.Value) == 0)
		{
			MessageBox.Show("Inco Term Is Required");
			CmbIncomeTerm.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNetWeightHeader.Text) > 0.0))
		{
			MessageBox.Show("Net Weight  Is Required");
			txtNetWeightHeader.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtGrossWeightHeader.Text) >= Conversion.ToDouble((object)txtNetWeightHeader.Text)))
		{
			MessageBox.Show("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
			txtGrossWeightHeader.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNoOfContainerHeader.Text) == 0.0)
		{
			MessageBox.Show("Fcl Qty Field Is Required");
			txtNoOfContainerHeader.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPaymentTermsNew.Value) == 0)
		{
			MessageBox.Show("Payment Term Is Required");
			CmbPaymentTermsNew.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbFcyCode.Value) == 0)
		{
			MessageBox.Show("Currency Code Is Required");
			CmbFcyCode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfcyAmountHeader.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount  Is Required");
			txtfcyAmountHeader.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("Item field required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack uom field required");
			CmbPackUom.Focus();
			return false;
		}
		if (txtQtyMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtQtyMTon.Text) > 0.0))
		{
			MessageBox.Show("Qty/M.Ton field required");
			txtQtyMTon.Focus();
			return false;
		}
		if (txtNoOfBags.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNoOfBags.Text.Trim()) > 0.0))
		{
			MessageBox.Show("No of Bags field required");
			txtNoOfBags.Focus();
			return false;
		}
		if (txtCostMTon.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtCostMTon.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Cost/M.Ton field required");
			txtCostMTon.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("Rate UOM field required");
			CmbRateUom.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtAmount.Text.Trim()) > 0.0))
		{
			MessageBox.Show("Amount field required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 0;
				throw new Exception("Detail Grid Record Not Found. Please Check! ");
			}
			ProformaMaster PIH = new ProformaMaster();
			if (RecId == 0L)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				PIH.proformaMasterId = RecId;
			}
			double MTonTotal = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2));
			txtNetWeightHeader.Text = Conversion.ToDouble((object)(MTonTotal * 1000.0)).ToString("#,##0.###");
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.BranchesId = UserAccount.BranchesId;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.ProjectsId = UserAccount.BranchesId;
			PIH.documentTypeId = 900;
			PIH.appActionId = AppActionId;
			PIH.isApproved = false;
			PIH.createdUserId = UserAccount.ID;
			PIH.lastModifiedUserId = UserAccount.ID;
			PIH.createdOn = DateTime.Now;
			PIH.lastModifiedOn = DateTime.Now;
			PIH.dateOfIssue = DateTime.Now;
			PIH.lastShipmentDate = DateTime.Now;
			PIH.approvedOn = DateTime.Now;
			PIH.docNo = Conversion.ToInt((object)DocNo.Text);
			PIH.validityUpto = datValidityUpto.Value;
			PIH.importerId = Conversion.ToInt(CmbImporterName.Value);
			PIH.exporterId = Conversion.ToInt(CmbExporterName.Value);
			PIH.proformaNo = ((Control)(object)CmbPerformaNo).Text;
			PIH.proformaMasterId = Conversion.ToInt64(CmbPerformaNo.Value);
			PIH.docDate = Conversion.ToDateTime((object)txtPerformaDate.Value);
			PIH.GoodsOriginId = Conversion.ToInt(CmbGoodsOrigin.Value);
			PIH.loadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			PIH.destinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			PIH.incoTermId = Conversion.ToInt(CmbIncomeTerm.Value);
			PIH.netWeightTotal = Conversion.ToDecimal((object)txtNetWeightHeader.Text);
			PIH.grossWeightTotal = Conversion.ToDecimal((object)txtGrossWeightHeader.Text.Trim());
			PIH.fclTotal = Conversion.ToInt((object)txtNoOfContainerHeader.Text.Trim());
			PIH.paymentTermId = Conversion.ToInt(CmbPaymentTermsNew.Value);
			PIH.currencyId = Conversion.ToInt(CmbFcyCode.Value);
			PIH.fcyAmountTotal = Conversion.ToDecimal((object)txtfcyAmountHeader.Text);
			PIH.TermsConditions = txtTermsAndConditions.Text;
			PIH.RowVersionLong = RowVersionLong;
			PIH.ProformaDetailList = new List<ProformaDetail>();
			PIH.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (LstRemoveRecordDetail.Count > 0)
			{
				for (int j = 0; j < LstRemoveRecordDetail.Count; j++)
				{
					PIH.ProformaDetailList.Add(LstRemoveRecordDetail[j]);
				}
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				ProformaDetail vd = new ProformaDetail();
				vd.proformaDetailId = ((RecId == 0L) ? 0 : Conversion.ToInt64(r.Cells["Id"].Value));
				vd.appActionId = ((vd.proformaDetailId <= 0) ? 1 : 2);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemDescription = Conversion.ToString((object)r.Cells["ItemDetail"].Text);
				vd.lotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.packSizeIdinner = Conversion.ToInt(r.Cells["PackSizeId"].Value);
				vd.packSizeIdOuter = Conversion.ToInt(r.Cells["PackSizeId"].Value);
				vd.qtyInner = Conversion.ToInt(r.Cells["NoOfBags"].Value);
				vd.qtyOuter = Conversion.ToInt(r.Cells["NoOfBags"].Value);
				vd.netWeightInner = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd.netWeightOuter = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd.NetWeightKgs = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value) * 1000.0;
				vd.ItemRate = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
				vd.uomIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
				vd.FcAmount = Conversion.ToDouble(r.Cells["Amount"].Value);
				vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
				vd.RowVersionLong = Conversion.ToInt64(r.Cells["RowVersionLong"].Value);
				vd.isApproved = false;
				vd.createdOn = DateTime.Now;
				vd.createdUserId = UserAccount.ID;
				vd.lastModifiedOn = DateTime.Now;
				vd.lastModifiedUserId = UserAccount.ID;
				vd.approvedOn = DateTime.Now;
				PIH.ProformaDetailList.Add(vd);
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.UploadedFileCustomName = Attachment.dtAttachments.Rows[i]["CustomFileName"].ToString();
				attach.ScreenName = base.Name;
				PIH.AttachmentsList.Add(attach);
			}
			PIH.AttachmentsValues = AttachmentsValues;
			PIH.CustomAttachmentsValues = CustomAttachmentsValues;
			if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
			{
				PIH.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
				{
					DMSAttachments delete = new DMSAttachments();
					delete.Attachment = Path.GetFileName(AT.RemovedAttachmentListInUpdateCase[k].Path);
					delete.ScreenName = base.Name;
					PIH.DeleteAttachmentsList.Add(delete);
				}
			}
			long success = ProformaMaster.Save(PIH);
			if (RecId == 0L)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (RecId > 0)
			{
				MessageBox.Show("Update SuccessFully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && Attachment.dtAttachments.Rows.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Conversion.ToInt((object)RecId), base.Name, 900, 0);
				}
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				InvoiceSlip(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0L;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0L)
			{
				throw new Exception("Rec Id not found...");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(long Id)
	{
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			RecId = Id;
			ProformaMaster PIH = ProformaMaster.GetByID(RecId);
			if (PIH == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			if (PIH.ProformaDetailList != null && PIH.ProformaDetailList.Count > 0)
			{
				CmbBranchName.Value = PIH.ProformaDetailList[0].locationBranchId;
			}
			ProformaNoFill(BindProformaNo());
			DocNo.Text = PIH.docNo.ToString();
			datValidityUpto.Value = PIH.validityUpto;
			CmbImporterName.Value = PIH.importerId;
			CmbExporterName.Value = PIH.exporterId;
			CmbPerformaNo.Value = PIH.proformaMasterId;
			txtPerformaDate.Value = PIH.docDate;
			((Control)(object)CmbPerformaNo).Enabled = false;
			txtPerformaDate.Enabled = false;
			CmbGoodsOrigin.Value = PIH.GoodsOriginId;
			cmbLoadingPort.Value = PIH.loadingPortId;
			cmbDestinationPort.Value = PIH.destinationPortId;
			CmbIncomeTerm.Value = PIH.incoTermId;
			txtGrossWeightHeader.Text = PIH.grossWeightTotal.ToString("#,##0.####");
			txtNetWeightHeader.Text = PIH.netWeightTotal.ToString("#,##0.####");
			txtNoOfContainerHeader.Text = PIH.fclTotal.ToString();
			CmbPaymentTermsNew.Value = PIH.paymentTermId;
			CmbFcyCode.Value = PIH.currencyId;
			txtfcyAmountHeader.Text = PIH.fcyAmountTotal.ToString("#,##0.####");
			AttachmentsValues = PIH.AttachmentsValues;
			CustomAttachmentsValues = PIH.CustomAttachmentsValues;
			txtTermsAndConditions.Text = PIH.TermsConditions;
			RowVersionLong = PIH.RowVersionLong;
			AppActionId = PIH.appActionId;
			dtdetail.Rows.Clear();
			for (int i = 0; i < PIH.ProformaDetailList.Count; i++)
			{
				dtdetail.Rows.Add(PIH.ProformaDetailList[i].proformaDetailId, PIH.ProformaDetailList[i].ItemId, PIH.ProformaDetailList[i].ItemCode, PIH.ProformaDetailList[i].ItemName, PIH.ProformaDetailList[i].ItemDescription, PIH.ProformaDetailList[i].lotJobId, PIH.ProformaDetailList[i].JobLotDescription, PIH.ProformaDetailList[i].netWeightOuter / 1000.0, PIH.ProformaDetailList[i].packSizeIdOuter, PIH.ProformaDetailList[i].OuterUOM, PIH.ProformaDetailList[i].OuterEquivalent, PIH.ProformaDetailList[i].qtyOuter, PIH.ProformaDetailList[i].netWeightOuter, PIH.ProformaDetailList[i].ItemRate, PIH.ProformaDetailList[i].uomIdRate, PIH.ProformaDetailList[i].RateUOM, PIH.ProformaDetailList[i].RateEquivalent, PIH.ProformaDetailList[i].FcAmount, PIH.ProformaDetailList[i].RemarksDetail, PIH.ProformaDetailList[i].RowVersionLong);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Conversion.ToInt((object)RecId), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				TA.ActionId = 2;
				TA.CustomFileName = Attachment.UploadedFileCustomName;
				AT.lst.Add(TA);
			}
			UpdateMode = true;
			datValidityUpto.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt64(r.Cells["Id"].Value));
			}
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
		else if (BranchFeature)
		{
			CmbBranchName.Focus();
		}
		else
		{
			CmbExporterName.Focus();
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
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			gridDetailHistory.DataSource = null;
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
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		try
		{
			DataTable dtImporter = new DataTable();
			dtImporter.Columns.Add("Id", typeof(int));
			dtImporter.Columns.Add("name", typeof(string));
			DataTable dt = ProformaMaster.GetDataForDropDownFrom_ProformaMaster(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ImporterName")
					{
						dtImporter.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtImporter, CmbCustomerHistory, "Id", "name", "Importer Name", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters Obj = new ReportsParameters();
			Obj.OrganizationId = UserAccount.OrganizationId;
			Obj.CompanyId = UserAccount.CompanyId;
			Obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Obj.DocumentTypeId = 900;
			Obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			Obj.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					Obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					Obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			Obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			Obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			Obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtgrd = ProformaMaster.SearchHistory(Obj);
			if (dtgrd.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(long));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("ValidityUpto", typeof(DateTime));
				dt.Columns.Add("Importer", typeof(string));
				dt.Columns.Add("Exporter", typeof(string));
				dt.Columns.Add("ProformaNo", typeof(string));
				dt.Columns.Add("ProformaDate", typeof(DateTime));
				dt.Columns.Add("GoodsOrigin", typeof(string));
				dt.Columns.Add("LoadingPort", typeof(string));
				dt.Columns.Add("DestinationPort", typeof(string));
				dt.Columns.Add("IncoTerm", typeof(string));
				dt.Columns.Add("GrossWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("FclQty", typeof(int));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("Currency", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("TermsAndConditions", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Rows.Clear();
				foreach (DataRow row in dtgrd.Rows)
				{
					dt.Rows.Add(row["proformaMasterId"], row["docNo"], row["validityUpto"], row["ImporterName"], row["ExporterName"], row["proformaNo"], row["docDate"], row["GoodsOrigin"], row["LoadingPortName"], row["DestinationPortName"], row["incoTermName"], row["grossWeightTotal"], row["netWeightTotal"], row["fclTotal"], row["PaymentTermCode"], row["CurrencyCode"], row["fcyAmountTotal"], row["TermsConditions"], row["CreatedOn"], row["CreatedUserName"], row["LastModifiedOn"], row["LastModifiedUserName"], row["IsApproved"], row["ApprovedOn"], row["ApprovedUserName"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
				gridDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			DataGridHistory.RootTable.Columns["ValidityUpto"].FormatString = "dd-MM-yy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["docNo"].Width = Constants.ExportConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["ValidityUpto"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["Importer"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["Exporter"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["ProformaNo"].Width = Constants.ExportConstants.ProformaNo;
			DataGridHistory.RootTable.Columns["ProformaDate"].Width = Constants.ExportConstants.DateConstant;
			DataGridHistory.RootTable.Columns["GoodsOrigin"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["IncoTerm"].Width = Constants.ExportConstants.TermConstant;
			DataGridHistory.RootTable.Columns["GrossWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["FclQty"].Width = Constants.ExportConstants.FclConstant;
			DataGridHistory.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.StringConstant;
			DataGridHistory.RootTable.Columns["Currency"].Width = Constants.ExportConstants.FcyConstant;
			DataGridHistory.RootTable.Columns["Amount"].Width = Constants.ExportConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["TermsAndConditions"].Width = Constants.ExportConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["IsApproved"].Width = Constants.ExportConstants.IsApproved;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.ExportConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			if (formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Slip");
				DataGridHistory.RootTable.Columns["Slip"].Caption = "Slip";
				DataGridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Slip"].ButtonText = "900-Slip";
				DataGridHistory.RootTable.Columns["Slip"].Width = 60;
				DataGridHistory.RootTable.Columns["Slip"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 35;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHavePrintRights && formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.RootTable.Columns["Slip"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 902);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				long Id = Conversion.ToInt64(r.Cells["Id"].Value);
				if (e.Column.Key == "Slip")
				{
					InvoiceSlip(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				long Id = Conversion.ToInt64(r.Cells["Id"].Value);
				GetDetailByHeaderId(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId(long rr)
	{
		try
		{
			ProformaMaster PIH = ProformaMaster.GetByID(rr);
			if (PIH.ProformaDetailList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(long));
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("ItemDetail");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("Qty/M.Ton", typeof(double));
				dt.Columns.Add("PackSize");
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("Cost/M.Ton", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks");
				dt.Rows.Clear();
				for (int i = 0; i < PIH.ProformaDetailList.Count; i++)
				{
					dt.Rows.Add(PIH.ProformaDetailList[i].proformaDetailId, PIH.ProformaDetailList[i].ItemCode, PIH.ProformaDetailList[i].ItemName, PIH.ProformaDetailList[i].ItemDescription, PIH.ProformaDetailList[i].JobLotDescription, PIH.ProformaDetailList[i].netWeightOuter / 1000.0, PIH.ProformaDetailList[i].OuterUOM, PIH.ProformaDetailList[i].qtyOuter, PIH.ProformaDetailList[i].netWeightOuter, PIH.ProformaDetailList[i].ItemRate, PIH.ProformaDetailList[i].RateUOM, PIH.ProformaDetailList[i].FcAmount, PIH.ProformaDetailList[i].RemarksDetail);
				}
				gridDetailHistory.DataSource = dt;
				gridDetailHistory.RetrieveStructure();
				grdHistorySettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySettings()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			gridDetailHistory.RootTable.Columns["Id"].Visible = false;
			gridDetailHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)gridDetailHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			gridDetailHistory.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			gridDetailHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			gridDetailHistory.RootTable.Columns["ItemDetail"].Width = Constants.InventoryConstants.OtherDescription;
			gridDetailHistory.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			gridDetailHistory.RootTable.Columns["Qty/M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["PackSize"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			gridDetailHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			gridDetailHistory.RootTable.Columns["Cost/M.Ton"].Width = Constants.InventoryConstants.ExRateConstant;
			gridDetailHistory.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			gridDetailHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			gridDetailHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmount_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtaddlesscommnets_TextChanged(object sender, EventArgs e)
	{
	}

	private void CalculateWeight()
	{
		try
		{
			double MTon = Conversion.ToDouble((object)txtQtyMTon.Text);
			double NoOfBags = Conversion.ToDouble((object)txtNoOfBags.Text);
			double OuterUom = 0.0;
			string Bool_Mton = txtQtyMTon.AccessibleName;
			string Bool_NoOfBags = txtNoOfBags.AccessibleName;
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				if (Conversion.ToString(base.ActiveControl.Tag) == "Mton")
				{
					double noofbags = ((MTon > 0.0) ? (MTon * 1000.0 / OuterUom) : 0.0);
					txtNoOfBags.Text = Math.Round(noofbags).ToString();
					txtQtyMTon.AccessibleName = "True";
					txtNoOfBags.AccessibleName = "False";
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "NoOfBags")
				{
					MTon = ((NoOfBags > 0.0) ? (NoOfBags * OuterUom / 1000.0) : 0.0);
					txtQtyMTon.Text = MTon.ToString("#,##0.###");
					txtNoOfBags.AccessibleName = "True";
					txtQtyMTon.AccessibleName = "False";
				}
				else if (Bool_Mton == "True")
				{
					double noofbags2 = ((MTon > 0.0) ? (MTon * 1000.0 / OuterUom) : 0.0);
					txtNoOfBags.Text = Math.Round(noofbags2).ToString();
				}
				else if (Bool_NoOfBags == "True")
				{
					MTon = ((NoOfBags > 0.0) ? (NoOfBags * OuterUom / 1000.0) : 0.0);
					txtQtyMTon.Text = MTon.ToString("#,##0.###");
				}
			}
			else
			{
				txtNoOfBags.Text = 0.ToString();
				txtQtyMTon.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculatAmount()
	{
		try
		{
			double ItemRate = 0.0;
			double ItemRateUom = 0.0;
			double Qty = 0.0;
			txtAmount.Text = 0.ToString();
			if (((UltraGridBase)CmbRateUom).ActiveRow != null)
			{
				ItemRate = Conversion.ToDouble((object)txtCostMTon.Text);
				ItemRateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				Qty = Conversion.ToDouble((object)txtQtyMTon.Text);
				if (ItemRate > 0.0 && ItemRateUom > 0.0 && Qty > 0.0)
				{
					double weightkgs = Qty * 1000.0;
					double amount = ItemRate / ItemRateUom * weightkgs;
					txtAmount.Text = amount.ToString("0,0");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtQtyMTon.Text) > 0.0)
			{
				double Weight = Conversion.ToDouble((object)txtQtyMTon.Text) * 1000.0;
				txtWeightDetail.Text = Weight.ToString();
			}
			else
			{
				txtWeightDetail.Text = "0";
			}
			CalculateWeight();
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_TextChanged(object sender, EventArgs e)
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

	private void txtCostMTon_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculatAmount();
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
			CalculatAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBags_TextChanged(object sender, EventArgs e)
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

	private void InvoiceSlip(long PrintId)
	{
		try
		{
			CommonServices.ProformaInvoiceSlip900(PrintId);
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
			InvoiceSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtCostMTon_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnDefinePerformaNo_Click(object sender, EventArgs e)
	{
		try
		{
			new frmProformaDocumentSerial(UserAccount).ShowDialog();
			ProformaNoFill(BindProformaNo());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImProformaInvoice_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					if (BranchFeature)
					{
						CmbBranchName.Focus();
					}
					else
					{
						CmbExporterName.Focus();
					}
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					if (BranchFeature)
					{
						CmbBranchName.Focus();
					}
					else
					{
						CmbExporterName.Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Down && tabControl2.SelectedIndex == 0)
				{
					((Control)(object)grdDetail).Focus();
					tabControl2.SelectedIndex = 0;
				}
				if (e.Control && e.KeyCode == Keys.Up && tabControl2.SelectedIndex == 0)
				{
					CmbItemName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					tabControl2.SelectedIndex = 0;
					CmbItemName.Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowHistory_Click(null, null);
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
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				FromDateHistory.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)DataGridHistory).Focused)
				{
					((Control)(object)gridDetailHistory).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+L", "For Load All Records");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowRight", "For Moving In Detail Tabs");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Selected Tab Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On First Entry of Detail Selected Tab");
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn != null && grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailGridRow(item);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailGridRow(item);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			long Id = Conversion.ToInt64(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Slip" && formrights.DoHavePrintRights)
				{
					InvoiceSlip(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Attachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 902);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPerformaNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt64(CmbPerformaNo.Value) > 0)
			{
				txtPerformaDate.Value = Conversion.ToDateTime(((UltraDropDownBase)CmbPerformaNo).SelectedRow.Cells[2].Value);
			}
			else
			{
				txtPerformaDate.Value = DateTime.Now;
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
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Expected O, but got Unknown
		//IL_0a91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9b: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Expected O, but got Unknown
		//IL_0c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Expected O, but got Unknown
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Expected O, but got Unknown
		//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed1: Expected O, but got Unknown
		//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee7: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efd: Expected O, but got Unknown
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f08: Expected O, but got Unknown
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1e: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Expected O, but got Unknown
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Expected O, but got Unknown
		//IL_0f40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f4a: Expected O, but got Unknown
		//IL_17aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b4: Expected O, but got Unknown
		//IL_17c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cb: Expected O, but got Unknown
		//IL_8647: Unknown result type (might be due to invalid IL or missing references)
		//IL_8651: Expected O, but got Unknown
		//IL_8675: Unknown result type (might be due to invalid IL or missing references)
		//IL_867f: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout gridDetailHistory_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.frmProformaInvoice));
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label85 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.txtQtyMTon = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.label94 = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.txtitemDescription = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtCostMTon = new System.Windows.Forms.TextBox();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label29 = new System.Windows.Forms.Label();
		this.btnCancel = new System.Windows.Forms.Button();
		this.txtWeightDetail = new System.Windows.Forms.TextBox();
		this.CmbItemName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtNoOfContainerHeader = new System.Windows.Forms.TextBox();
		this.txtfcyAmountHeader = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbPaymentTermsNew = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbFcyCode = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.CmbGoodsOrigin = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.txtTermsAndConditions = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.datValidityUpto = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.DocNo = new System.Windows.Forms.TextBox();
		this.txtDocNo = new System.Windows.Forms.Label();
		this.BtnDefinePerformaNo = new System.Windows.Forms.Button();
		this.label109 = new System.Windows.Forms.Label();
		this.CmbImporterName = new UltraCombo();
		this.txtPerformaDate = new System.Windows.Forms.DateTimePicker();
		this.CmbPerformaNo = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label98 = new System.Windows.Forms.Label();
		this.label101 = new System.Windows.Forms.Label();
		this.CmbExporterName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label96 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbIncomeTerm = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.cmbDestinationPort = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label107 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtNetWeightHeader = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtGrossWeightHeader = new System.Windows.Forms.TextBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.CmbBranchName = new UltraCombo();
		this.labelBranchName = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label20 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.gridDetailHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbPaymentTerm = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbImporterBank = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel17.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel23.SuspendLayout();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel11.SuspendLayout();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermsNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGoodsOrigin).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPerformaNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIncomeTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1096, 711);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1096, 711);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1096, 711);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1088, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 57);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1082, 622);
		this.panel8.TabIndex = 3;
		this.panel17.AutoScroll = true;
		this.panel17.Controls.Add(this.tabControl2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 205);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1082, 417);
		this.panel17.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1082, 417);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage3.Controls.Add(this.panel23);
		this.tabPage3.Controls.Add(this.panel22);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1074, 388);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 125);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1068, 260);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar2);
		this.panel23.Controls.Add(this.label85);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(3, 101);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1068, 24);
		this.panel23.TabIndex = 1;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1029, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label85.ForeColor = System.Drawing.Color.White;
		this.label85.Location = new System.Drawing.Point(3, 2);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(49, 20);
		this.label85.TabIndex = 27;
		this.label85.Text = "Detail";
		this.panel22.Controls.Add(this.txtQtyMTon);
		this.panel22.Controls.Add(this.label35);
		this.panel22.Controls.Add(this.btnAddinGrid);
		this.panel22.Controls.Add(this.label32);
		this.panel22.Controls.Add(this.label94);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel22.Controls.Add(this.label38);
		this.panel22.Controls.Add(this.txtRemarksDetail);
		this.panel22.Controls.Add(this.label37);
		this.panel22.Controls.Add(this.label19);
		this.panel22.Controls.Add(this.txtAmount);
		this.panel22.Controls.Add(this.txtitemDescription);
		this.panel22.Controls.Add(this.label36);
		this.panel22.Controls.Add(this.label34);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel22.Controls.Add(this.txtCostMTon);
		this.panel22.Controls.Add(this.btnGridUpdate);
		this.panel22.Controls.Add(this.label29);
		this.panel22.Controls.Add(this.btnCancel);
		this.panel22.Controls.Add(this.txtWeightDetail);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel22.Controls.Add(this.label2);
		this.panel22.Controls.Add(this.label30);
		this.panel22.Controls.Add(this.txtNoOfBags);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1068, 98);
		this.panel22.TabIndex = 5;
		this.txtQtyMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyMTon.Location = new System.Drawing.Point(623, 24);
		this.txtQtyMTon.Name = "txtQtyMTon";
		this.txtQtyMTon.Size = new System.Drawing.Size(98, 22);
		this.txtQtyMTon.TabIndex = 3;
		this.txtQtyMTon.Tag = "Mton";
		this.txtQtyMTon.Text = "0";
		this.txtQtyMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyMTon.TextChanged += new System.EventHandler(txtQtyMTon_TextChanged);
		this.txtQtyMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyMTon_KeyPress);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(621, 6);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 498;
		this.label35.Text = "Qty/M.Ton";
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatAppearance.BorderSize = 0;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(900, 66);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(31, 26);
		this.btnAddinGrid.TabIndex = 11;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(4, 5);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(67, 15);
		this.label32.TabIndex = 470;
		this.label32.Text = "Item Name";
		this.label94.AutoSize = true;
		this.label94.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label94.Location = new System.Drawing.Point(417, 5);
		this.label94.Name = "label94";
		this.label94.Size = new System.Drawing.Size(24, 15);
		this.label94.TabIndex = 493;
		this.label94.Text = "Lot";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance268;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance279).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance290).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance290).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance290).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance290).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(101, 67);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(100, 25);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(417, 23);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(204, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 2;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(101, 51);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(61, 15);
		this.label38.TabIndex = 479;
		this.label38.Text = "Rate UOM";
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(313, 68);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(580, 22);
		this.txtRemarksDetail.TabIndex = 10;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(203, 51);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(51, 15);
		this.label37.TabIndex = 485;
		this.label37.Text = "Amount";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(313, 51);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(52, 15);
		this.label19.TabIndex = 491;
		this.label19.Text = "Remarks";
		this.txtAmount.BackColor = System.Drawing.SystemColors.ControlLightLight;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(203, 68);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(109, 22);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.TabStop = false;
		this.txtAmount.Text = "0";
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtitemDescription.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtitemDescription.Location = new System.Drawing.Point(203, 24);
		this.txtitemDescription.Name = "txtitemDescription";
		this.txtitemDescription.Size = new System.Drawing.Size(212, 22);
		this.txtitemDescription.TabIndex = 1;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(723, 6);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(57, 15);
		this.label36.TabIndex = 474;
		this.label36.Text = "Pack Size";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(203, 5);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(66, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Item Detail";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(723, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(104, 25);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(combitempck_TextChanged);
		this.txtCostMTon.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCostMTon.Location = new System.Drawing.Point(4, 68);
		this.txtCostMTon.Name = "txtCostMTon";
		this.txtCostMTon.Size = new System.Drawing.Size(95, 22);
		this.txtCostMTon.TabIndex = 7;
		this.txtCostMTon.Text = "0";
		this.txtCostMTon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCostMTon.TextChanged += new System.EventHandler(txtCostMTon_TextChanged);
		this.txtCostMTon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCostMTon_KeyPress);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnGridUpdate.FlatAppearance.BorderSize = 0;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(896, 66);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(65, 26);
		this.btnGridUpdate.TabIndex = 12;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(4, 51);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 487;
		this.label29.Text = "Rate/M.Ton";
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderSize = 0;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(964, 66);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(65, 26);
		this.btnCancel.TabIndex = 13;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnCancel_Leave);
		this.txtWeightDetail.BackColor = System.Drawing.Color.White;
		this.txtWeightDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightDetail.Location = new System.Drawing.Point(915, 24);
		this.txtWeightDetail.Name = "txtWeightDetail";
		this.txtWeightDetail.ReadOnly = true;
		this.txtWeightDetail.Size = new System.Drawing.Size(114, 22);
		this.txtWeightDetail.TabIndex = 6;
		this.txtWeightDetail.TabStop = false;
		this.txtWeightDetail.Text = "0";
		this.txtWeightDetail.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance240).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(4, 23);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(197, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(combitem_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(915, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 485;
		this.label2.Text = "Weight(kg)";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(829, 6);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 485;
		this.label30.Text = "Qty/NoOfBags";
		this.txtNoOfBags.BackColor = System.Drawing.Color.White;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(829, 24);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(85, 22);
		this.txtNoOfBags.TabIndex = 5;
		this.txtNoOfBags.Tag = "NoOfBags";
		this.txtNoOfBags.Text = "0";
		this.txtNoOfBags.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.panel11.Controls.Add(this.HeaderGropBox);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1082, 205);
		this.panel11.TabIndex = 0;
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add(this.label11);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainerHeader);
		this.HeaderGropBox.Controls.Add(this.txtfcyAmountHeader);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFcyCode);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add(this.label51);
		this.HeaderGropBox.Controls.Add(this.txtTermsAndConditions);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Controls.Add(this.datValidityUpto);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.DocNo);
		this.HeaderGropBox.Controls.Add(this.txtDocNo);
		this.HeaderGropBox.Controls.Add(this.BtnDefinePerformaNo);
		this.HeaderGropBox.Controls.Add(this.label109);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterName);
		this.HeaderGropBox.Controls.Add(this.txtPerformaDate);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPerformaNo);
		this.HeaderGropBox.Controls.Add(this.label6);
		this.HeaderGropBox.Controls.Add(this.label98);
		this.HeaderGropBox.Controls.Add(this.label101);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbExporterName);
		this.HeaderGropBox.Controls.Add(this.label4);
		this.HeaderGropBox.Controls.Add(this.label96);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbIncomeTerm);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.label107);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Controls.Add(this.txtNetWeightHeader);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeightHeader);
		this.HeaderGropBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.HeaderGropBox.Location = new System.Drawing.Point(4, 2);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1037, 199);
		this.HeaderGropBox.TabIndex = 0;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.label12.BackColor = System.Drawing.Color.Silver;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(700, 16);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(332, 24);
		this.label12.TabIndex = 608;
		this.label12.Text = "Terms && Conditions";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(412, 124);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 527;
		this.label11.Text = "Payment Term";
		this.txtNoOfContainerHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainerHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainerHeader.Location = new System.Drawing.Point(531, 94);
		this.txtNoOfContainerHeader.Name = "txtNoOfContainerHeader";
		this.txtNoOfContainerHeader.Size = new System.Drawing.Size(163, 22);
		this.txtNoOfContainerHeader.TabIndex = 13;
		this.txtNoOfContainerHeader.Text = "0";
		this.txtNoOfContainerHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmountHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfcyAmountHeader.Location = new System.Drawing.Point(532, 173);
		this.txtfcyAmountHeader.Name = "txtfcyAmountHeader";
		this.txtfcyAmountHeader.ReadOnly = true;
		this.txtfcyAmountHeader.Size = new System.Drawing.Size(163, 22);
		this.txtfcyAmountHeader.TabIndex = 16;
		this.txtfcyAmountHeader.Text = "0";
		this.txtfcyAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmountHeader.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(412, 151);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(84, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Currency Code";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(514, 97);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.CmbPaymentTermsNew.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermsNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermsNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermsNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance248;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance252;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance253).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance255).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance256).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance258;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermsNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermsNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Location = new System.Drawing.Point(532, 119);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Name = "CmbPaymentTermsNew";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermsNew).TabIndex = 14;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(111, 176);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(14, 17);
		this.label10.TabIndex = 607;
		this.label10.Text = "*";
		this.CmbFcyCode.AllowNull = (DefaultableBoolean)1;
		this.CmbFcyCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFcyCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFcyCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance260).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance260).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance261;
		((SpecialBoxBase)((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance262).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance262).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance266).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance267).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance267).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance267).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance267).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance267).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance267;
		((AppearanceBase)appearance269).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFcyCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFcyCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Location = new System.Drawing.Point(532, 146);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Name = "CmbFcyCode";
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbFcyCode).TabIndex = 15;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(111, 150);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 606;
		this.label9.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(412, 177);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(51, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Amount";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Red;
		this.label8.Location = new System.Drawing.Point(111, 123);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(14, 17);
		this.label8.TabIndex = 605;
		this.label8.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(515, 150);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.CmbGoodsOrigin.AllowNull = (DefaultableBoolean)1;
		this.CmbGoodsOrigin.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGoodsOrigin.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGoodsOrigin.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Appearance = (AppearanceBase)(object)appearance272;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance273).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance273).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance273).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance273;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance274;
		((SpecialBoxBase)((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance275).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance275).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance277).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance280).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance280;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance281).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance281).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance281).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance281;
		((AppearanceBase)appearance282).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance283).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGoodsOrigin).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGoodsOrigin.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin).Location = new System.Drawing.Point(125, 119);
		((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin).Name = "CmbGoodsOrigin";
		((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbGoodsOrigin).TabIndex = 7;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(515, 176);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 124);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 604;
		this.label7.Text = "Goods Origin";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(515, 123);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 17);
		this.label51.TabIndex = 592;
		this.label51.Text = "*";
		this.label51.Visible = false;
		this.txtTermsAndConditions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTermsAndConditions.Location = new System.Drawing.Point(700, 41);
		this.txtTermsAndConditions.Multiline = true;
		this.txtTermsAndConditions.Name = "txtTermsAndConditions";
		this.txtTermsAndConditions.Size = new System.Drawing.Size(332, 157);
		this.txtTermsAndConditions.TabIndex = 17;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(412, 98);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(44, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "Fcl Qty";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(224, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(75, 15);
		this.label5.TabIndex = 601;
		this.label5.Text = "Validity Upto";
		this.datValidityUpto.CustomFormat = "dd-MMM-yy";
		this.datValidityUpto.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datValidityUpto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datValidityUpto.Location = new System.Drawing.Point(304, 16);
		this.datValidityUpto.Name = "datValidityUpto";
		this.datValidityUpto.Size = new System.Drawing.Size(106, 22);
		this.datValidityUpto.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(111, 97);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 599;
		this.label3.Text = "*";
		this.DocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.DocNo.Enabled = false;
		this.DocNo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocNo.Location = new System.Drawing.Point(125, 16);
		this.DocNo.Name = "DocNo";
		this.DocNo.Size = new System.Drawing.Size(97, 22);
		this.DocNo.TabIndex = 0;
		this.txtDocNo.AutoSize = true;
		this.txtDocNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.txtDocNo.Location = new System.Drawing.Point(3, 20);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(48, 15);
		this.txtDocNo.TabIndex = 561;
		this.txtDocNo.Text = "Doc No";
		this.BtnDefinePerformaNo.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefinePerformaNo.Location = new System.Drawing.Point(381, 93);
		this.BtnDefinePerformaNo.Name = "BtnDefinePerformaNo";
		this.BtnDefinePerformaNo.Size = new System.Drawing.Size(29, 25);
		this.BtnDefinePerformaNo.TabIndex = 6;
		this.BtnDefinePerformaNo.TabStop = false;
		this.BtnDefinePerformaNo.UseVisualStyleBackColor = true;
		this.BtnDefinePerformaNo.Click += new System.EventHandler(BtnDefinePerformaNo_Click);
		this.label109.AutoSize = true;
		this.label109.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label109.ForeColor = System.Drawing.Color.Red;
		this.label109.Location = new System.Drawing.Point(514, 19);
		this.label109.Name = "label109";
		this.label109.Size = new System.Drawing.Size(14, 17);
		this.label109.TabIndex = 592;
		this.label109.Text = "*";
		this.label109.Visible = false;
		this.CmbImporterName.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance285;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance286).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance286).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance286).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance286).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance286;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance287;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance288).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance288).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance288;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance289;
		((AppearanceBase)appearance291).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance291).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance293).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance293;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance294).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance294).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance294).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance294;
		((AppearanceBase)appearance295).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance296).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbImporterName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Location = new System.Drawing.Point(125, 41);
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Name = "CmbImporterName";
		((System.Windows.Forms.Control)(object)this.CmbImporterName).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbImporterName).TabIndex = 2;
		this.txtPerformaDate.CustomFormat = "dd-MMM-yy";
		this.txtPerformaDate.Enabled = false;
		this.txtPerformaDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPerformaDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPerformaDate.Location = new System.Drawing.Point(273, 94);
		this.txtPerformaDate.Name = "txtPerformaDate";
		this.txtPerformaDate.Size = new System.Drawing.Size(106, 22);
		this.txtPerformaDate.TabIndex = 5;
		this.CmbPerformaNo.AllowNull = (DefaultableBoolean)1;
		this.CmbPerformaNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPerformaNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPerformaNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance299).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance299).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance299).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance300;
		((SpecialBoxBase)((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPerformaNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPerformaNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Location = new System.Drawing.Point(125, 93);
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Name = "CmbPerformaNo";
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.CmbPerformaNo).TabIndex = 4;
		this.CmbPerformaNo.ValueChanged += new System.EventHandler(CmbPerformaNo_ValueChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(111, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(14, 17);
		this.label6.TabIndex = 115;
		this.label6.Text = "*";
		this.label98.AutoSize = true;
		this.label98.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label98.ForeColor = System.Drawing.Color.Red;
		this.label98.Location = new System.Drawing.Point(111, 45);
		this.label98.Name = "label98";
		this.label98.Size = new System.Drawing.Size(14, 17);
		this.label98.TabIndex = 115;
		this.label98.Text = "*";
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label101.Location = new System.Drawing.Point(3, 98);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(103, 15);
		this.label101.TabIndex = 594;
		this.label101.Text = "Performa No,Date";
		this.CmbExporterName.AllowNull = (DefaultableBoolean)1;
		this.CmbExporterName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbExporterName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbExporterName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbExporterName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbExporterName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Location = new System.Drawing.Point(125, 67);
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Name = "CmbExporterName";
		((System.Windows.Forms.Control)(object)this.CmbExporterName).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.CmbExporterName).TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(111, 71);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 115;
		this.label4.Text = "*";
		this.label96.AutoSize = true;
		this.label96.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label96.Location = new System.Drawing.Point(3, 72);
		this.label96.Name = "label96";
		this.label96.Size = new System.Drawing.Size(97, 15);
		this.label96.TabIndex = 78;
		this.label96.Text = "Exporter/Shipper";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(3, 46);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(114, 15);
		this.label21.TabIndex = 78;
		this.label21.Text = "Importer/Consignee";
		this.CmbIncomeTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbIncomeTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbIncomeTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbIncomeTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbIncomeTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbIncomeTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Location = new System.Drawing.Point(531, 15);
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Name = "CmbIncomeTerm";
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).Size = new System.Drawing.Size(163, 25);
		((System.Windows.Forms.Control)(object)this.CmbIncomeTerm).TabIndex = 10;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(3, 177);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(93, 15);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(125, 146);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 8;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(125, 172);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(285, 25);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 9;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(412, 46);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(101, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net Weight (Kg's)";
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label107.Location = new System.Drawing.Point(412, 20);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(78, 15);
		this.label107.TabIndex = 527;
		this.label107.Text = "Income Term";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label56.Location = new System.Drawing.Point(3, 151);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.txtNetWeightHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightHeader.Location = new System.Drawing.Point(531, 42);
		this.txtNetWeightHeader.Name = "txtNetWeightHeader";
		this.txtNetWeightHeader.ReadOnly = true;
		this.txtNetWeightHeader.Size = new System.Drawing.Size(163, 22);
		this.txtNetWeightHeader.TabIndex = 11;
		this.txtNetWeightHeader.Text = "0";
		this.txtNetWeightHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(514, 71);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(514, 45);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(412, 72);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(111, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross Weight (Kg's)";
		this.txtGrossWeightHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeightHeader.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeightHeader.Location = new System.Drawing.Point(531, 68);
		this.txtGrossWeightHeader.Name = "txtGrossWeightHeader";
		this.txtGrossWeightHeader.Size = new System.Drawing.Size(163, 22);
		this.txtGrossWeightHeader.TabIndex = 12;
		this.txtGrossWeightHeader.Text = "0";
		this.txtGrossWeightHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.panel10.Controls.Add(this.labelBranchName);
		this.panel10.Controls.Add(this.label23);
		this.panel10.Controls.Add(this.ChkPrintPreview);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1082, 27);
		this.panel10.TabIndex = 1;
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranchName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(218, 1);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 500;
		this.labelBranchName.AutoSize = true;
		this.labelBranchName.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelBranchName.ForeColor = System.Drawing.Color.White;
		this.labelBranchName.Location = new System.Drawing.Point(168, 7);
		this.labelBranchName.Name = "labelBranchName";
		this.labelBranchName.Size = new System.Drawing.Size(49, 15);
		this.labelBranchName.TabIndex = 501;
		this.labelBranchName.Text = "Branch ";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(149, 19);
		this.label23.TabIndex = 25;
		this.label23.Text = "Proforma Invoice";
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(994, 4);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkPrintPreview.TabIndex = 32;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1082, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnNew, this.btnRefresh, this.btnsave, this.btnupdate, this.btnAttachment, this.btnPrint, this.btnShortCutKey });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1082, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "Print-900";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel14);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.gridDetailHistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1088, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 156);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1082, 252);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label92);
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 128);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1082, 28);
		this.panel15.TabIndex = 47;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(2, 3);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 1;
		this.label92.Text = "Filtered Records";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1034, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1082, 73);
		this.panel3.TabIndex = 46;
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add(this.label90);
		this.groupBox2.Controls.Add(this.label91);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(932, 71);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(373, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(89, 15);
		this.label20.TabIndex = 87;
		this.label20.Text = "Importer Name";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 46);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 46);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(462, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(266, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label69);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1082, 52);
		this.panel16.TabIndex = 0;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(1, 29);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(212, 19);
		this.label69.TabIndex = 1;
		this.label69.Text = "Proforma Invoice History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnResetHistory, this.btnRefreshHistory, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1082, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label93);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(3, 408);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1082, 22);
		this.panel14.TabIndex = 2;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(3, 3);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(213, 14);
		this.label93.TabIndex = 1;
		this.label93.Text = "Detail of the Selected above Row";
		this.gridDetailHistory.AllowEdit = (InheritableBoolean)2;
		gridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.gridDetailHistory.DesignTimeLayout = gridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.gridDetailHistory.DynamicFiltering = true;
		this.gridDetailHistory.FilterMode = (FilterMode)2;
		this.gridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.gridDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Location = new System.Drawing.Point(3, 430);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Name = "gridDetailHistory";
		((ScrollableBase)this.gridDetailHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.gridDetailHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.gridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).Size = new System.Drawing.Size(1082, 249);
		((System.Windows.Forms.Control)(object)this.gridDetailHistory).TabIndex = 1;
		this.gridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.gridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.gridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1096, 711);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(346, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(280, 27);
		this.miniToolStrip.TabIndex = 0;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(280, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip1";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 24);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1096, 711);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmProformaInvoice";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmImportInvoice_FormClosing);
		base.Load += new System.EventHandler(ImProformaInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel11.ResumeLayout(false);
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermsNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFcyCode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGoodsOrigin).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPerformaNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbExporterName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbIncomeTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.gridDetailHistory).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
