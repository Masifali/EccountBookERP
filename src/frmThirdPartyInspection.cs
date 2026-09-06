// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.DAL.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Export;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.ExportReports;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
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
namespace Architecture.WinApp.Export
{

public class frmThirdPartyInspection : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtdetail = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtParamsdetail = new DataTable();

	private DataTable dtSamplingDetail = new DataTable();

	private DataTable dtcol = new DataTable();

	private int RecId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int MostUsedRequestedById;

	private int MostUsedSampleHandedOverId;

	private int updateDetailIndex = -1;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmThirdPartyInspection";

	private bool FormInitialized;

	private bool SubsidiaryAccountAllownOnVouchers;

	private int AnalysisGroupId;

	private int DefaultExportInspectionCountryOfOriginId;

	private string RemoveDetailIds = "";

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnPrint;

	private ToolStripButton btnShortCutKey;

	private Panel panel28;

	private Label label66;

	private Panel panel1;

	private GroupBox groupBox1;

	private DateTimePicker txtRequestLodgDate;

	private Label label11;

	private TextBox txtRequestReferenceNo;

	private Label label27;

	private Label label3;

	private Label label2;

	private Label label1;

	private TextBox txtHeaderMtons;

	private Label label5;

	internal UltraCombo CmbItemMain;

	private Label label4;

	private GroupBox groupBox2;

	private Label label12;

	internal UltraCombo CmbResultStatus;

	private DateTimePicker txtDateOfInspection;

	private Label label6;

	private Label label9;

	private TextBox txtReportCertificateNo;

	private Label label8;

	private Panel panel2;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label14;

	private Panel panel6;

	private TextBox txtSealNoForShipper;

	private Label label20;

	private TextBox txtSealNoForBuyer;

	private Label label19;

	private TextBox txtDetailRemarks;

	private Button btnCancelDetial;

	private TextBox txtMtons;

	private Label label34;

	private Label label16;

	private Label label15;

	private Label label17;

	private Button btnUpdateDetail;

	internal UltraCombo CmbCustomer;

	private Button btnAdd;

	private Label label39;

	internal UltraCombo cmbContractNo;

	internal GridEX grd;

	private TextBox txtSubLot;

	private ToolStrip toolStrip3;

	private Panel panel4;

	private Label label18;

	private Panel panel5;

	private Panel panel7;

	private Label label21;

	internal GridEX DataGridHistory;

	internal GridEX grdDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label22;

	internal UltraCombo CmbInvoiceNoDetail;

	private DateTimePicker txtSampleATADesDate;

	private Label label23;

	private Label label24;

	internal UltraCombo CmbScheduleNo;

	private TextBox txtInspectionRemarks;

	internal UltraCombo CmbRequestedBy;

	internal UltraCombo CmbMedium;

	internal UltraCombo CmbCountryOfInspection;

	internal UltraCombo CmbCountryOfOrigin;

	internal UltraCombo CmbSamplingResponsibity;

	internal UltraCombo CmbInspectionAgency;

	private Label label31;

	private TextBox txtExporterLotNo;

	private Label label30;

	private Label label26;

	private Label label29;

	private Label label28;

	internal UltraCombo CmbResultRemarks;

	private DateTimePicker txtSampleDispatchedDate;

	private Label label33;

	private DateTimePicker txtSampleETADesDate;

	private Label label36;

	private DateTimePicker txtReportDate;

	private Label label40;

	private Label label37;

	private Label label38;

	private TextBox txtTransitDays;

	private Label label35;

	private TextBox txtCourierTrackingNo;

	private Label label32;

	private GroupBox groupBox3;

	private DateTimePicker txtHandOverDate;

	private Label label46;

	private Label label45;

	private TextBox txtLabRemarks;

	private DateTimePicker txtStockSealedDate;

	private Label label43;

	internal UltraCombo CmbSampleHandOver;

	private DateTimePicker txtSampleTakenDate;

	private Label label42;

	private DateTimePicker txtStockReservedDate;

	private Label label41;

	private Label label44;

	private GroupBox groupBox4;

	internal GridEX grdParameters;

	private Panel panel8;

	internal UltraCombo CmbRequiredAnalysisGroup;

	private Label label7;

	internal UltraCombo CmbLotTrackingNo;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShow;

	internal UltraCombo CmbItemHistory;

	private Label label52;

	internal UltraCombo CmbTrackingNoHistory;

	private Label label13;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label54;

	private ToolStripButton btnResetHistory;

	private ToolStripButton BtnRefreshHistory;

	private Label label10;

	private Button BtnInspectionTeam;

	private Button BtnInspectionTeam2;

	private ToolStripButton BtnTrackingReport;

	private Label label57;

	private Label label56;

	private Label label55;

	private Label label53;

	private Label label51;

	private Label label50;

	private Label label49;

	private Label label48;

	private Label label47;

	private Label label25;

	private Label label59;

	private TextBox CmbLotCurrentStage;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private Button btnDefineJobLot;

	internal UltraCombo CmbJobLot;

	private Label label58;

	private ToolStripButton BtnPrintStockReserved;

	private ToolStripButton BtnStockReservedForm;

	private RadioButton RadNotRefferedHistory;

	private RadioButton RadRefferedHistory;

	private RadioButton RadAllHistory;

	private Panel panel9;

	internal GridEX grdLotSampling;

	private Panel panel10;

	private Label label60;

	private RadioButton RadNotReffered;

	private RadioButton RadReffered;

	private RadioButton RadAll;

	private Panel panel11;

	private Label label61;

	internal UltraCombo CmbFarmingNTrade;

	internal GridEX grdContractInfo;

	private Panel panel12;

	private ToolStripButton btnCountryDefinition;

	private Label label62;

	private TextBox txtInstructionRemarks;

	private Label label63;

	private DateTimePicker txtResultDate;

	public frmThirdPartyInspection(UserAccount user)
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
			ScreenName = Conversion.ToString((object)base.Name);
			base.WindowState = FormWindowState.Maximized;
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			dtItem.Columns.Add("InventoryParentCategoriesId");
			dtItem.Columns.Add("ItemCategoryId");
			dtItem.Columns.Add("ItemCategory");
			dtItem.Columns.Add("ItemTypeId");
			dtItem.Columns.Add("ItemType");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("Lot");
			dtdetail.Columns.Add("CustomerId", typeof(int));
			dtdetail.Columns.Add("Customer");
			dtdetail.Columns.Add("ContractId", typeof(int));
			dtdetail.Columns.Add("ContractNo");
			dtdetail.Columns.Add("ContractScheduleId", typeof(int));
			dtdetail.Columns.Add("ContractScheduleNo");
			dtdetail.Columns.Add("InvoiceId", typeof(int));
			dtdetail.Columns.Add("InvoiceNo");
			dtdetail.Columns.Add("M.Tons", typeof(double));
			dtdetail.Columns.Add("SealNoForBuyer");
			dtdetail.Columns.Add("SealNoForShipper");
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("ReferredStatusId", typeof(int));
			dtdetail.Columns.Add("ReferredStatus", typeof(string));
			dtParamsdetail.Columns.Add("Id", typeof(int));
			dtParamsdetail.Columns.Add("ParameterId", typeof(int));
			dtParamsdetail.Columns.Add("Parameter");
			dtParamsdetail.Columns.Add("Remarks");
			dtSamplingDetail.Columns.Add("Id", typeof(int));
			dtSamplingDetail.Columns.Add("ThirdPartyTypeId", typeof(int));
			dtSamplingDetail.Columns.Add("ThirdPartyType", typeof(string));
			dtSamplingDetail.Columns.Add("NoOfSample", typeof(int));
			dtSamplingDetail.Columns.Add("SampleWeight_Kg", typeof(double));
			dtSamplingDetail.Columns.Add("TotalWeight_Kg", typeof(double));
			dtSamplingDetail.Columns.Add("SealNo");
			dtSamplingDetail.Columns.Add("Remarks");
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetial.Visible = false;
			CmbLotTrackingNo.Focus();
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
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultExportInspectionCountryOfOriginId");
			DefaultExportInspectionCountryOfOriginId = Conversion.ToInt((object)((obj2 != null) ? obj2.ConfigKey : null));
			DataTable exportLookData = ExportLookDtDbCall();
			DataTable exportContractCombData = CustomerFromContractDbCall();
			DataTable countryData = CountryDtDbCall();
			DataTable historyComboData = HistoryComboDbCall();
			DataTable analysisGroupData = AnalysisGroupdbCall();
			DataTable farmingNTradeData = FarmingNTradeDbCall();
			DataTable tackingData = TackingDbCall();
			DataTable mostUsedData = InvLabPreProductionExportLotInspectionHeader.InvLabPreProductionExportLotInspectionHeader_MostUsedIds(UserAccount.OrganizationId, UserAccount.CompanyId, clsGlobalVariables.ActiveYr.Id);
			return new
			{
				ExportLookData = exportLookData,
				ExportContractCombData = exportContractCombData,
				CountryData = countryData,
				HistoryComboData = historyComboData,
				MostUsedData = mostUsedData,
				TackingData = tackingData,
				AnalysisGroupData = analysisGroupData,
				FarmingNTradeData = farmingNTradeData
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
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnPrint.Enabled = formrights.DoHavePrintRights;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				_ = RecId;
				ExportComboBindFromLookup(result.ExportLookData);
				CustomerFromContractBind(result.ExportContractCombData);
				OriginAndInspectionCountryBind(result.CountryData);
				AnalysisGroupBind(result.AnalysisGroupData);
				FarmingNTradeBind(result.FarmingNTradeData);
				JobLotDtFillFromGlobal();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				if (dtdetail != null && dtdetail.Columns.Count > 0)
				{
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					grdSettings();
				}
				if (dtParamsdetail != null && dtParamsdetail.Columns.Count > 0)
				{
					grdParameters.DataSource = dtParamsdetail;
					grdParameters.RetrieveStructure();
					grdParamsSettings();
				}
				if (dtSamplingDetail != null && dtSamplingDetail.Columns.Count > 0)
				{
					grdLotSampling.DataSource = dtSamplingDetail;
					grdLotSampling.RetrieveStructure();
					grdSettingLotSampling();
				}
				TackingNoBind(result.TackingData);
				HistoryComboFill(result.HistoryComboData);
				BindMostUsed(result.MostUsedData);
				((UltraGridBase)CmbResultStatus).Rows[1].Activate();
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable TackingDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = true;
			obj.ScreenName = ScreenName;
			obj.ApprovedFilter = "All";
			if (RadReffered.Checked)
			{
				obj.ActionId = 1;
			}
			else if (RadNotReffered.Checked)
			{
				obj.ActionId = 2;
			}
			return InvLabPreProductionExportLotInspectionHeader.FormHistory(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void TackingNoBind(DataTable dt)
	{
		try
		{
			DataTable dtTrack = new DataTable();
			dtTrack.Columns.Add("Id", typeof(int));
			dtTrack.Columns.Add("name", typeof(string));
			dtTrack.Columns.Add("Status", typeof(string));
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtTrack.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["LotRefNo"], dt.Rows[i]["SampleStatus"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbLotTrackingNo, dtTrack, "Id", "name", "TrackingNo", AllColumns: true, Conversion.ToInt(CmbLotTrackingNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ExportLookDtDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ExImLookUps.GetByLookTypeId(new ExImLookUps
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

	private DataTable FarmingNTradeDbCall()
	{
		try
		{
			return ExImLcOrder.GetFarmingNTrade();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FarmingNTradeBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbFarmingNTrade, dt, "Id", "FarmingNTrade", "Farming N Trade", AllColumns: true, Conversion.ToInt(CmbFarmingNTrade.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbFarmingNTrade).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportComboBindFromLookup(DataTable dt)
	{
		try
		{
			DataTable dtRequestedBy = new DataTable();
			dtRequestedBy.Columns.Add("Id", typeof(int));
			dtRequestedBy.Columns.Add("Name", typeof(string));
			DataTable dtMedium = dtRequestedBy.Clone();
			DataTable dtAgency = dtRequestedBy.Clone();
			DataTable dtSamplingResp = dtRequestedBy.Clone();
			DataTable dtResultStatus = dtRequestedBy.Clone();
			DataTable dtResultRemarks = dtRequestedBy.Clone();
			foreach (DataRow row in dt.Rows)
			{
				int typeId = Conversion.ToInt(row["ExImLookUptypesId"]);
				int id = Conversion.ToInt(row["Id"]);
				string name = Conversion.ToString(row["LookUpName"]);
				switch (typeId)
				{
				case 8:
					dtRequestedBy.Rows.Add(id, name);
					break;
				case 11:
					dtMedium.Rows.Add(id, name);
					break;
				case 12:
					dtAgency.Rows.Add(id, name);
					break;
				case 13:
					dtSamplingResp.Rows.Add(id, name);
					break;
				case 14:
					dtResultStatus.Rows.Add(id, name);
					break;
				case 15:
					dtResultRemarks.Rows.Add(id, name);
					break;
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbRequestedBy, dtRequestedBy, "Id", "Name", "Requested By", AllColumns: true, Conversion.ToInt(CmbRequestedBy.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbSampleHandOver, dtRequestedBy, "Id", "Name", "SampleHand Over", AllColumns: true, Conversion.ToInt(CmbSampleHandOver.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbMedium, dtMedium, "Id", "Name", "Medium", AllColumns: true, Conversion.ToInt(CmbMedium.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbInspectionAgency, dtAgency, "Id", "Name", "Inspection Agency", AllColumns: true, Conversion.ToInt(CmbInspectionAgency.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbSamplingResponsibity, dtSamplingResp, "Id", "Name", "Sampling Responsibility", AllColumns: true, Conversion.ToInt(CmbSamplingResponsibity.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbResultStatus, dtResultStatus, "Id", "Name", "Result Status", AllColumns: true, Conversion.ToInt(CmbResultStatus.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(CmbResultRemarks, dtResultRemarks, "Id", "Name", "Result Remarks", AllColumns: true, Conversion.ToInt(CmbResultRemarks.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error binding lookup combos: " + ex.Message);
		}
	}

	private DataTable CountryDtDbCall()
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

	private void OriginAndInspectionCountryBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCountryOfOrigin, dt, "Id", "Description", "Country Of Origin", AllColumns: false, Conversion.ToInt(CmbCountryOfOrigin.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCountryOfInspection, dt, "Id", "Description", "Country Of Inspection", AllColumns: false, Conversion.ToInt(CmbCountryOfInspection.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error binding lookup combos: " + ex.Message);
		}
	}

	private DataTable AnalysisGroupdbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
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

	private void AnalysisGroupBind(DataTable dtLAG)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbRequiredAnalysisGroup, dtLAG, "Id", "AnalysisGroupDescription", "Analysis Group", AllColumns: false, Conversion.ToInt(CmbRequiredAnalysisGroup.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtItem.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (dynamic row in (IEnumerable<object>)allItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17))
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.InventoryParentCategoriesId, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void JobLotDtFillFromGlobal()
	{
		try
		{
			int jobId = Conversion.ToInt(CmbJobLot.Value);
			DataTable dtJobLot = new DataTable();
			dtJobLot.Columns.Add("Id", typeof(int));
			dtJobLot.Columns.Add("Lot", typeof(string));
			List<GetJobLotGlIdsandName> JobLots = clsGlobalVariables.globalJobLot;
			if (JobLots == null || !JobLots.Any())
			{
				return;
			}
			foreach (GetJobLotGlIdsandName row in JobLots)
			{
				dtJobLot.Rows.Add(row.Id, row.JobLotDescription);
			}
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDL(dtJobLot, CmbJobLot, "Id", "Lot", "job Lot", ZeroIndex: false);
				CommonServices.SetComboValue(CmbJobLot, "Id", jobId, dtJobLot);
			}
			else
			{
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error loading JobLot: " + ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind();
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
			int Id = Conversion.ToInt(CmbItemMain.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbItemMain, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbItemMain).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemMain).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemMain).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable CustomerFromContractDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Customer"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CustomerFromContractBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomer, dt, "Id", "name", "Customer Name", AllColumns: false, Conversion.ToInt(CmbCustomer.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindMostUsed(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				MostUsedRequestedById = Conversion.ToInt(dt.Rows[0]["RequestedById"]);
				MostUsedSampleHandedOverId = Conversion.ToInt(dt.Rows[0]["SampleHandedOverId"]);
				DataTable dtRequestedBy = ((UltraGridBase)CmbRequestedBy).DataSource as DataTable;
				CommonServices.SetComboValue(CmbRequestedBy, "Id", MostUsedRequestedById, dtRequestedBy);
			}
			_ = ((UltraGridBase)CmbCountryOfOrigin).DataSource;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ContractNoDbCall(int CustomerId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.PendingDataLoaderForThirdPartyLab(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = CustomerId,
				RecId = RecId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ContractBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbContractNo, dt, "Id", "LcOrderNo", "Contract No", AllColumns: true, Conversion.ToInt(cmbContractNo.Value));
			if (dt != null && dt.Rows.Count > 0)
			{
				cmbContractNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindInvoiceNoByContactID(int Id)
	{
		try
		{
			int ComboId = Conversion.ToInt((object)Conversion.ToInt(CmbInvoiceNoDetail.Value));
			DataTable dt = CommonServices.GetInvoicesDataForShipmentAnalysis(Id);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbInvoiceNoDetail, "InvoiceId", "InvoiceNo", "Invoice No", ZeroIndex: false);
				((UltraGridBase)CmbInvoiceNoDetail).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (ComboId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("InvoiceId") == ComboId))
					{
						CmbInvoiceNoDetail.Value = ComboId;
					}
					else
					{
						((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbInvoiceNoDetail).DataSource = null;
				((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGetContractSchedulesByContactID(int Id, int SupplierId)
	{
		try
		{
			int ComboId = Conversion.ToInt((object)Conversion.ToInt(CmbScheduleNo.Value));
			if (Id > 0 && SupplierId > 0)
			{
				DataTable dt = CommonServices.ContractSchedulesDataForShipmentAnalysis(Id, SupplierId);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbScheduleNo, "ContractScheduleId", "ContractScheduleNo", "ContractSchedule No", ZeroIndex: true);
					((UltraGridBase)CmbScheduleNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (ComboId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ContractScheduleId") == ComboId))
						{
							CmbScheduleNo.Value = ComboId;
						}
						else
						{
							((Control)(object)CmbScheduleNo).Text = string.Empty;
						}
					}
					if (dt.Rows.Count == 2)
					{
						((UltraGridBase)CmbScheduleNo).Rows[1].Activate();
						CmbScheduleNo_Leave(null, null);
					}
				}
				else
				{
					((UltraGridBase)CmbScheduleNo).DataSource = null;
					((Control)(object)CmbScheduleNo).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)CmbScheduleNo).DataSource = null;
				((Control)(object)CmbScheduleNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbCustomer.Value) > 0)
			{
				ContractBind(ContractNoDbCall(Conversion.ToInt(CmbCustomer.Value)));
			}
			BindGetContractSchedulesByContactID(Conversion.ToInt(cmbContractNo.Value), Conversion.ToInt(CmbCustomer.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbContractNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbContractNo.Value) != 0)
			{
				BindInvoiceNoByContactID(Conversion.ToInt(cmbContractNo.Value));
				UltraGridRow selectedRow = ((UltraDropDownBase)cmbContractNo).SelectedRow;
				double Mtons = Conversion.ToDouble((selectedRow != null) ? selectedRow.Cells["BalanceMton"].Value : null);
				txtMtons.Text = Mtons.ToString("#,##.###");
			}
			else
			{
				((UltraGridBase)CmbInvoiceNoDetail).DataSource = null;
				((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
			}
			BindGetContractSchedulesByContactID(Conversion.ToInt(cmbContractNo.Value), Conversion.ToInt(CmbCustomer.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbScheduleNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbScheduleNo).ActiveRow == null || Conversion.ToInt(CmbScheduleNo.Value) <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[2].Value);
			double Mtons = Conversion.ToInt(((UltraDropDownBase)CmbScheduleNo).SelectedRow.Cells[4].Value);
			if (Conversion.ToDouble(txtMtons.Tag) == 0.0 && updateDetailIndex == -1)
			{
				txtMtons.Text = Mtons.ToString("#,##0.###");
			}
			if (((UltraGridBase)CmbInvoiceNoDetail).DataSource is DataTable dt && dt.Rows.Count > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("InvoiceId") == Id))
				{
					CmbInvoiceNoDetail.Value = Id;
				}
				else
				{
					((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNoDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbInvoiceNoDetail).ActiveRow == null || Conversion.ToInt(CmbInvoiceNoDetail.Value) <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(((UltraDropDownBase)CmbInvoiceNoDetail).SelectedRow.Cells[3].Value);
			if (((UltraGridBase)CmbScheduleNo).DataSource is DataTable dt)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("ContractScheduleId") == Id))
				{
					CmbScheduleNo.Value = Id;
				}
				else
				{
					((Control)(object)CmbScheduleNo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbLotTrackingNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbLotTrackingNo.Value);
			if (((UltraGridBase)CmbLotTrackingNo).ActiveRow != null && Id > 0)
			{
				ReadById(Id);
			}
			else
			{
				CmbLotCurrentStage.Text = "Pending For Inspection";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtTransitDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ETADate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString(txtExporterLotNo.Tag) == "" || txtExporterLotNo.Text == "")
			{
				txtExporterLotNo.Text = ((Control)(object)CmbJobLot).Text;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadAll_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			TackingNoBind(TackingDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (string.IsNullOrWhiteSpace(((Control)(object)CmbLotTrackingNo).Text))
		{
			MessageBox.Show("Lot Reference/tracking No is required.");
			CmbLotTrackingNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbRequestedBy.Value) == 0)
		{
			MessageBox.Show("Requested By is required.");
			CmbRequestedBy.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbMedium.Value) == 0)
		{
			MessageBox.Show("Request Medium is required.");
			CmbMedium.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbItemMain.Value) == 0)
		{
			MessageBox.Show("Item selection is required.");
			CmbItemMain.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtHeaderMtons.Text) <= 0.0)
		{
			MessageBox.Show("Quantity in M.Tons must be greater than 0.");
			txtHeaderMtons.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbInspectionAgency.Value) == 0)
		{
			MessageBox.Show("Inspection Agency is required.");
			CmbInspectionAgency.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbSamplingResponsibity.Value) == 0)
		{
			MessageBox.Show("Sampling Responsibility is required.");
			CmbSamplingResponsibity.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbCountryOfOrigin.Value) == 0)
		{
			MessageBox.Show("Country of Origin is required.");
			CmbCountryOfOrigin.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbResultStatus.Value) == 0)
		{
			MessageBox.Show("Result Status is required.");
			CmbResultStatus.Focus();
			return false;
		}
		if (txtSampleDispatchedDate.Checked && txtSampleATADesDate.Checked && Conversion.ToDateTime((object)txtSampleDispatchedDate.Value).Date >= Conversion.ToDateTime((object)txtSampleATADesDate.Value).Date)
		{
			MessageBox.Show("'Sample Dispatched Date' cannot be greater than or Equal 'Sample ATA Destination Date'");
			txtSampleDispatchedDate.Focus();
			return false;
		}
		if (txtSampleDispatchedDate.Checked && txtSampleTakenDate.Checked && Conversion.ToDateTime((object)txtSampleTakenDate.Value).Date > Conversion.ToDateTime((object)txtSampleDispatchedDate.Value).Date)
		{
			MessageBox.Show("'Sample Dispatched Date' cannot be lesser than 'Sample Taken Date'");
			txtSampleDispatchedDate.Focus();
			return false;
		}
		if (txtStockReservedDate.Checked && txtSampleTakenDate.Checked && Conversion.ToDateTime((object)txtStockReservedDate.Value).Date > Conversion.ToDateTime((object)txtSampleTakenDate.Value).Date)
		{
			MessageBox.Show("'Stock Reserved Date' cannot be greater than 'Sample Taken Date'");
			txtStockReservedDate.Focus();
			return false;
		}
		if (txtStockSealedDate.Checked && txtSampleTakenDate.Checked && Conversion.ToDateTime((object)txtStockSealedDate.Value).Date < Conversion.ToDateTime((object)txtSampleTakenDate.Value).Date)
		{
			MessageBox.Show("'Sample Taken Date' cannot be greater than 'Stock Sealed Date'");
			txtSampleTakenDate.Focus();
			return false;
		}
		if (txtStockSealedDate.Checked && txtHandOverDate.Checked && Conversion.ToDateTime((object)txtStockSealedDate.Value).Date > Conversion.ToDateTime((object)txtHandOverDate.Value).Date)
		{
			MessageBox.Show("'Stock Sealed Date' cannot be greater than 'Sample hand over Date'");
			txtStockSealedDate.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		DataTable dtInvoice = (DataTable)((UltraGridBase)CmbInvoiceNoDetail).DataSource;
		if (Conversion.ToInt(CmbScheduleNo.Value) > 0)
		{
			int selectedScheduleId = Conversion.ToInt(CmbScheduleNo.Value);
			if (dtInvoice != null && dtInvoice.Rows.Count > 0 && dtInvoice.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["ContractScheduleId"]) == selectedScheduleId) && Conversion.ToInt(CmbInvoiceNoDetail.Value) == 0)
			{
				MessageBox.Show("Invoice No field is required");
				CmbInvoiceNoDetail.Focus();
				return false;
			}
		}
		if (Conversion.ToDouble((object)txtMtons.Text.Trim()) == 0.0)
		{
			MessageBox.Show("M.Tons field is required");
			txtMtons.Focus();
			return false;
		}
		int num = Conversion.ToInt(cmbContractNo.Value);
		double balanceMtons = 0.0;
		double enteredMtons = 0.0;
		if (num > 0 && ((UltraDropDownBase)cmbContractNo).SelectedRow != null)
		{
			UltraGridCell obj = ((UltraDropDownBase)cmbContractNo).SelectedRow.Cells["BalanceMton"];
			object val = ((obj != null) ? obj.Value : null);
			if (val != null && val != DBNull.Value)
			{
				balanceMtons = Conversion.ToDouble(val);
			}
			if (!double.TryParse(txtMtons.Text, out enteredMtons))
			{
				txtMtons.Focus();
				throw new Exception("Invalid MTon value. Please enter a valid number.");
			}
			if (enteredMtons > balanceMtons)
			{
				txtMtons.Focus();
				throw new Exception($"Entered MTon ({enteredMtons:N3}) cannot exceed the contract balance ({balanceMtons:N3}).\n\n" + "Contract: " + Conversion.ToString((object)((Control)(object)cmbContractNo).Text) + "\nPlease enter a value within the allowed limit.");
			}
			return false;
		}
		return true;
	}

	private void CmbRequiredAnalysisGroup_Leave(object sender, EventArgs e)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbRequiredAnalysisGroup.Value);
			if (ID <= 0)
			{
				return;
			}
			if (AnalysisGroupId > 0 && ID != AnalysisGroupId)
			{
				if (MessageBox.Show("You are Changing Analysis group.Parameters Data Will be Reset.are you Sure to proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					CmbRequiredAnalysisGroup.Value = AnalysisGroupId;
					return;
				}
				dtParamsdetail.Clear();
			}
			AnalysisGroupId = Conversion.ToInt(CmbRequiredAnalysisGroup.Value);
			DataTable dtDetail = InvLabGroupAnalysisStandards.GetParametersFromGroupStandards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = AnalysisGroupId
			});
			if (dtDetail.Rows.Count > 0)
			{
				for (int i = 0; i < dtDetail.Rows.Count; i++)
				{
					bool flag = false;
					if (grdParameters.GetRows().Count() > 0)
					{
						GridEXRow[] rows = grdParameters.GetRows();
						for (int j = 0; j < rows.Length; j++)
						{
							if (Conversion.ToInt(rows[j].Cells["ParameterId"].Value) == Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtParamsdetail.Rows.Add(0, Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]), Conversion.ToString(dtDetail.Rows[i]["AnalysisParameterDescription"]), "");
					}
				}
			}
			grdParameters.DataSource = dtParamsdetail;
			grdParameters.RetrieveStructure();
			grdParamsSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdParamsSettings()
	{
		try
		{
			grdParameters.TotalRow = (InheritableBoolean)2;
			grdParameters.AllowEdit = (InheritableBoolean)1;
			grdParameters.RootTable.Columns["Id"].Visible = false;
			grdParameters.RootTable.Columns["ParameterId"].Visible = false;
			grdParameters.RootTable.Columns["Parameter"].EditType = (EditType)0;
			grdParameters.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grdParameters.RootTable.Columns["Parameter"].Width = 100;
			grdParameters.RootTable.Columns["Remarks"].Width = 140;
			grdParameters.RootTable.Columns.Add("Select");
			grdParameters.RootTable.Columns["Select"].ActAsSelector = true;
			grdParameters.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdParameters.RootTable.Columns["Select"].Position = 0;
			grdParameters.RootTable.Columns["Select"].Width = 30;
			grdParameters.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ValidateDuplicateWeightPerItemInSampling()
	{
		try
		{
			var duplicateGroups = (from row in dtSamplingDetail.AsEnumerable()
				where row.Field<double>("SampleWeight_Kg") > 0.0
				group row by new
				{
					ItemId = row.Field<int>("ThirdPartyTypeId"),
					SampleWeight_Kg = row.Field<double>("SampleWeight_Kg")
				} into g
				where g.Count() > 1
				select g).ToList();
			if (duplicateGroups.Any())
			{
				IEnumerable<string> errorMessages = duplicateGroups.Select(g =>
				{
					string arg = g.First().Field<string>("ThirdPartyType");
					return $"ThirdPartyType '{arg}' has duplicate SampleWeight_Kg: {g.Key.SampleWeight_Kg}";
				});
				throw new Exception("Duplicate SampleWeight_Kg values found:\n" + string.Join("\n", errorMessages));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddPartyTypeInLotSampling(DataTable dtThirdPartyTypes)
	{
		try
		{
			if (dtSamplingDetail.Rows.Count == 0)
			{
				grdLotSampling.ClearStructure();
			}
			HashSet<int> existingIds = (from row in dtSamplingDetail.AsEnumerable()
				select row.Field<int>("ThirdPartyTypeId")).ToHashSet();
			foreach (DataRow r in dtThirdPartyTypes.Rows)
			{
				int partyTypeId = Conversion.ToInt(r["ThirdPartyTypeID"]);
				if (dtSamplingDetail.Rows.Count == 0 || !existingIds.Contains(partyTypeId))
				{
					dtSamplingDetail.Rows.Add(0, partyTypeId, r["PartyTypeName"], 0, 0, 0, "", "");
				}
			}
			grdLotSampling.DataSource = dtSamplingDetail;
			grdLotSampling.RetrieveStructure();
			grdSettingLotSampling();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingLotSampling()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ThirdPartyTypeId", "Remarks" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"NoOfSample",
					(EditType)1
				},
				{
					"SampleWeight_Kg",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				},
				{
					"SealNo",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdLotSampling, 3, 3, hiddenColumns, editableColumns);
			grdLotSampling.RootTable.Columns["ThirdPartyType"].Width = 60;
			grdLotSampling.RootTable.Columns["NoOfSample"].Width = 55;
			grdLotSampling.RootTable.Columns["SampleWeight_Kg"].Width = 55;
			grdLotSampling.RootTable.Columns["TotalWeight_Kg"].Width = 55;
			grdLotSampling.RootTable.Columns["Remarks"].Width = 80;
			grdLotSampling.RootTable.Columns["SealNo"].Width = 70;
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
			grd.RootTable.Columns["CustomerId"].Visible = false;
			grd.RootTable.Columns["ContractScheduleId"].Visible = false;
			grd.RootTable.Columns["ContractId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["ReferredStatusId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grd, 3);
			grd.RootTable.Columns["Lot"].Width = Constants.InventoryConstants.JobLotConstant;
			grd.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["ContractNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grd.RootTable.Columns["ContractScheduleNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grd.RootTable.Columns["M.Tons"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["SealNoForBuyer"].Width = Constants.InventoryConstants.SealNo;
			grd.RootTable.Columns["SealNoForShipper"].Width = Constants.InventoryConstants.SealNo;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				int InvoiceId = 0;
				string InvoiceNo = "";
				if (Conversion.ToInt(CmbInvoiceNoDetail.Value) != 0)
				{
					InvoiceId = Conversion.ToInt(CmbInvoiceNoDetail.Value);
					InvoiceNo = Conversion.ToString((object)((Control)(object)CmbInvoiceNoDetail).Text);
				}
				int ScheduleId = 0;
				string ScheduleNo = "";
				if (Conversion.ToInt(CmbScheduleNo.Value) != 0)
				{
					ScheduleId = Conversion.ToInt(CmbScheduleNo.Value);
					ScheduleNo = Conversion.ToString((object)((Control)(object)CmbScheduleNo).Text);
				}
				dtdetail.Rows.Add(0, txtSubLot.Text, CmbCustomer.Value, ((Control)(object)CmbCustomer).Text, cmbContractNo.Value, ((Control)(object)cmbContractNo).Text, ScheduleId, ScheduleNo, InvoiceId, InvoiceNo, Conversion.ToDouble((object)txtMtons.Text), txtSealNoForBuyer.Text, txtSealNoForShipper.Text, txtDetailRemarks.Text, 0, "Not Referred");
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				txtSubLot.Focus();
				InstertMtonInHeader();
				GetContractsInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetContractsInformation()
	{
		try
		{
			DataTable dt = (DataTable)grd.DataSource;
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			string contractIds = string.Join(",", (from r in dt.AsEnumerable()
				where r["ContractId"] != DBNull.Value
				select r.Field<int>("ContractId")).Distinct());
			DataTable dtInfo = new DataTable();
			dtInfo.Columns.Add("ContractId", typeof(int));
			dtInfo.Columns.Add("ContractNo", typeof(string));
			dtInfo.Columns.Add("Mton", typeof(double));
			dtInfo.Columns.Add("ShippedMton", typeof(double));
			dtInfo.Columns.Add("BalanceMton", typeof(double));
			foreach (DataRow row in ExImLcOrder.ExportContractBalanceByIds(UserAccount.OrganizationId, UserAccount.CompanyId, contractIds).Rows)
			{
				int contractId = ((row["ContractId"] != DBNull.Value) ? Convert.ToInt32(row["ContractId"]) : 0);
				string contractNo = row["ContractNo"]?.ToString();
				double mton = ((row["Mton"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row["Mton"]));
				double shippedMton = ((row["ShippedMton"] == DBNull.Value) ? 0.0 : Convert.ToDouble(row["ShippedMton"]));
				double balanceMton = mton - shippedMton;
				dtInfo.Rows.Add(contractId, contractNo, mton, shippedMton, balanceMton);
			}
			grdContractInfo.DataSource = dtInfo;
			grdContractInfo.RetrieveStructure();
			grdContractInfo.RootTable.Columns["ContractId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdContractInfo, 2, 3);
			grdContractInfo.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant + 50;
			grdContractInfo.RootTable.Columns["Mton"].Width = Constants.ExportConstants.QtyConstant;
			grdContractInfo.RootTable.Columns["ShippedMton"].Width = Constants.ExportConstants.QtyConstant;
			grdContractInfo.RootTable.Columns["BalanceMton"].Width = Constants.ExportConstants.QtyConstant;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				int InvoiceId = 0;
				string InvoiceNo = "";
				if (Conversion.ToInt(CmbInvoiceNoDetail.Value) != 0)
				{
					InvoiceId = Conversion.ToInt(CmbInvoiceNoDetail.Value);
					InvoiceNo = Conversion.ToString((object)((Control)(object)CmbInvoiceNoDetail).Text);
				}
				int ScheduleId = 0;
				string ScheduleNo = "";
				if (Conversion.ToInt(CmbScheduleNo.Value) != 0)
				{
					ScheduleId = Conversion.ToInt(CmbScheduleNo.Value);
					ScheduleNo = Conversion.ToString((object)((Control)(object)CmbScheduleNo).Text);
				}
				dtdetail.Rows[updateDetailIndex]["Lot"] = txtSubLot.Text;
				dtdetail.Rows[updateDetailIndex]["CustomerId"] = CmbCustomer.Value;
				dtdetail.Rows[updateDetailIndex]["Customer"] = ((Control)(object)CmbCustomer).Text;
				dtdetail.Rows[updateDetailIndex]["ContractId"] = cmbContractNo.Value;
				dtdetail.Rows[updateDetailIndex]["ContractNo"] = ((Control)(object)cmbContractNo).Text;
				dtdetail.Rows[updateDetailIndex]["ContractScheduleId"] = ScheduleId;
				dtdetail.Rows[updateDetailIndex]["ContractScheduleNo"] = ScheduleNo;
				dtdetail.Rows[updateDetailIndex]["InvoiceId"] = InvoiceId;
				dtdetail.Rows[updateDetailIndex]["InvoiceNo"] = InvoiceNo;
				dtdetail.Rows[updateDetailIndex]["M.Tons"] = txtMtons.Text;
				dtdetail.Rows[updateDetailIndex]["SealNoForBuyer"] = txtSealNoForBuyer.Text;
				dtdetail.Rows[updateDetailIndex]["SealNoForShipper"] = txtSealNoForShipper.Text;
				dtdetail.Rows[updateDetailIndex]["Remarks"] = txtDetailRemarks.Text;
				grd.DataSource = dtdetail;
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetial.Visible = false;
				ResetDetail();
				txtSubLot.Focus();
				InstertMtonInHeader();
				GetContractsInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetial_Click(object sender, EventArgs e)
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

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grd")
			{
				txtSubLot.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grd")
			{
				txtSubLot.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0 && Conversion.ToInt(item.Cells["ReferredStatusId"].Value) == 1)
				{
					throw new Exception("Record cannot be delete because record has been referred in delivery order");
				}
				if (Conversion.ToInt(item.Cells["Id"].Value) > 0 && Conversion.ToInt(item.Cells["ReferredStatusId"].Value) == 0)
				{
					RemoveDetailIds = RemoveDetailIds + Conversion.ToInt(item.Cells["Id"].Value) + ",";
					DeleteDetailRow(item);
				}
				else
				{
					DeleteDetailRow(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow item)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (item != null && (int)item.RowType == 0)
			{
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail");
				}
				dtdetail.Rows[item.RowIndex].Delete();
				item.Delete();
				grd.UpdateData();
				InstertMtonInHeader();
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
			if (item != null && (int)item.RowType == 0)
			{
				updateDetailIndex = item.RowIndex;
				txtSubLot.Text = item.Cells["Lot"].Text;
				CmbCustomer.Value = item.Cells["CustomerId"].Value;
				CmbCustomer_Leave(null, null);
				cmbContractNo.Value = item.Cells["ContractId"].Value;
				CmbScheduleNo.Value = item.Cells["ContractScheduleId"].Value;
				cmbContractNo_Leave(null, null);
				if (Conversion.ToInt(item.Cells["InvoiceId"].Value) > 0)
				{
					CmbInvoiceNoDetail.Value = item.Cells["InvoiceId"].Value;
				}
				txtMtons.Text = Conversion.ToDouble(item.Cells["M.Tons"].Value).ToString("#,##0.###");
				txtSealNoForBuyer.Text = item.Cells["SealNoForBuyer"].Text;
				txtSealNoForShipper.Text = item.Cells["SealNoForShipper"].Text;
				txtDetailRemarks.Text = item.Cells["Remarks"].Text;
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetial.Visible = true;
				txtSubLot.Focus();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grd, dtdetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtcol);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExporterLotNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtExporterLotNo.Tag = txtExporterLotNo.Text;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMtons_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtMtons.Tag = txtMtons.Text;
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Expected O, but got Unknown
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ValidateDuplicateWeightPerItemInSampling();
			InvLabPreProductionExportLotInspectionHeader obj = new InvLabPreProductionExportLotInspectionHeader();
			obj.Id = RecId;
			obj.LotRefNo = ((Control)(object)CmbLotTrackingNo).Text;
			obj.RequestDate = Conversion.ToDateTime((object)txtRequestLodgDate.Text.Trim());
			obj.RequestRefNo = txtRequestReferenceNo.Text;
			obj.RequestedById = Conversion.ToInt(CmbRequestedBy.Value);
			obj.RequestedBy = ((Control)(object)CmbRequestedBy).Text;
			obj.RequestMediumId = Conversion.ToInt(CmbMedium.Value);
			obj.RequestMedium = ((Control)(object)CmbMedium).Text;
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			obj.ExporterLotRefNo = Conversion.ToString((object)txtExporterLotNo.Text.Trim());
			obj.ItemId = Conversion.ToInt(CmbItemMain.Value);
			obj.ProductSpecification = Conversion.ToString((object)((Control)(object)CmbItemMain).Text.Trim());
			obj.QtyKgs = Conversion.ToDouble((object)txtHeaderMtons.Text.Trim());
			obj.InspectionAgencyId = Conversion.ToInt(CmbInspectionAgency.Value);
			obj.SamplingResponsibilityId = Conversion.ToInt(CmbSamplingResponsibity.Value);
			obj.CountryOfOriginId = Conversion.ToInt(CmbCountryOfOrigin.Value);
			obj.CountryOfInspectionId = Conversion.ToInt(CmbCountryOfInspection.Value);
			obj.PlaceOfInspection = ((obj.CountryOfInspectionId > 0) ? Conversion.ToString((object)((Control)(object)CmbCountryOfInspection).Text) : "");
			obj.CourierTrackingNo = Conversion.ToString((object)txtCourierTrackingNo.Text);
			if (txtSampleDispatchedDate.Checked)
			{
				obj.SampleDispatchedDate = Conversion.ToDateTime((object)txtSampleDispatchedDate.Value);
			}
			obj.TransitDays = Conversion.ToInt((object)txtTransitDays.Text.Trim());
			if (txtSampleDispatchedDate.Checked)
			{
				obj.SampleETADestinationDate = Conversion.ToDateTime((object)txtSampleDispatchedDate.Value).AddDays(obj.TransitDays);
			}
			if (txtSampleATADesDate.Checked)
			{
				obj.SampleATADestinationDate = Conversion.ToDateTime((object)txtSampleATADesDate.Value);
			}
			obj.InspectionRemarks = Conversion.ToString((object)txtInspectionRemarks.Text);
			obj.ReportRefNo = Conversion.ToString((object)txtReportCertificateNo.Text.Trim());
			if (txtReportDate.Checked)
			{
				obj.ReportDate = Conversion.ToDateTime((object)txtReportDate.Value);
			}
			if (txtDateOfInspection.Checked)
			{
				obj.DateOfInspection = Conversion.ToDateTime((object)txtDateOfInspection.Value);
			}
			if (txtResultDate.Checked)
			{
				obj.ResultDate = Conversion.ToDateTime((object)txtResultDate.Value);
			}
			obj.ReportStatus = ((Control)(object)CmbResultStatus).Text;
			obj.ReportResultRemarksId = Conversion.ToInt(CmbResultRemarks.Value);
			obj.RequiredAnalysisId = Conversion.ToInt(CmbRequiredAnalysisGroup.Value);
			if (txtStockReservedDate.Checked)
			{
				obj.StockReservedDate = Conversion.ToDateTime((object)txtStockReservedDate.Value);
			}
			if (txtSampleTakenDate.Checked)
			{
				obj.SampleTakenDate = Conversion.ToDateTime((object)txtSampleTakenDate.Value);
			}
			if (txtStockSealedDate.Checked)
			{
				obj.StockRSealedDate = Conversion.ToDateTime((object)txtStockSealedDate.Value);
			}
			obj.SampleHandedOverId = Conversion.ToInt(CmbSampleHandOver.Value);
			if (txtHandOverDate.Checked)
			{
				obj.SampleHandedOverDate = Conversion.ToDateTime((object)txtHandOverDate.Value);
			}
			obj.LabRemarks = Conversion.ToString((object)txtLabRemarks.Text);
			if (((UltraGridBase)CmbFarmingNTrade).ActiveRow != null && Conversion.ToInt(CmbFarmingNTrade.Value) > 0)
			{
				obj.ExImFarmingNTradeId = Conversion.ToInt(CmbFarmingNTrade.Value);
				obj.ExImFarmingTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTrade).SelectedRow.Cells["ExImFarmingTypeId"].Value);
				obj.ExImTradeTypeId = Conversion.ToInt(((UltraDropDownBase)CmbFarmingNTrade).SelectedRow.Cells["ExImTradeTypeId"].Value);
			}
			obj.LotInstructionsOrRemarks = Conversion.ToString((object)txtInstructionRemarks.Text.Trim());
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.ProjectsId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ScreenName = ScreenName;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedUserId = UserAccount.ID;
			obj.LotInspectionDetails = new List<InvLabPreProductionExportLotInspectionDetail>();
			obj.InvLabPreProductionExportLotInspectionParametersDetailList = new List<InvLabPreProductionExportLotInspectionParametersDetail>();
			obj.ThirdPartyInspectionLotSamplingDetailList = new List<ThirdPartyInspectionLotSamplingDetail>();
			decimal rowMTon = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				InvLabPreProductionExportLotInspectionDetail vd3 = new InvLabPreProductionExportLotInspectionDetail();
				vd3.Id = Conversion.ToInt(r3.Cells["Id"].Value);
				vd3.SubLot = Conversion.ToString((object)r3.Cells["Lot"].Text);
				vd3.SupplierCustomerId = Conversion.ToInt(r3.Cells["CustomerId"].Value);
				vd3.ContractId = Conversion.ToInt(r3.Cells["ContractId"].Value);
				vd3.ContractScheduleId = Conversion.ToInt(r3.Cells["ContractScheduleId"].Value);
				vd3.InvoiceId = Conversion.ToInt(r3.Cells["InvoiceId"].Value);
				vd3.Weight = Conversion.ToDecimal(r3.Cells["M.Tons"].Value);
				rowMTon += vd3.Weight;
				vd3.SealNoBuyer = r3.Cells["SealNoForBuyer"].Text;
				vd3.SealNoShipper = r3.Cells["SealNoForShipper"].Text;
				vd3.SubRemarks = r3.Cells["Remarks"].Text;
				obj.LotInspectionDetails.Add(vd3);
			}
			if (rowMTon > Conversion.ToDecimal((object)obj.QtyKgs))
			{
				MessageBox.Show($"Detail MTon ({rowMTon}) cannot be greater than Header MTon ({obj.QtyKgs}).", "Validation Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			rows = grdParameters.GetCheckedRows();
			foreach (GridEXRow r2 in rows)
			{
				InvLabPreProductionExportLotInspectionParametersDetail vd2 = new InvLabPreProductionExportLotInspectionParametersDetail();
				vd2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				vd2.ParameterId = Conversion.ToInt(r2.Cells["ParameterId"].Value);
				vd2.Remarks = r2.Cells["Remarks"].Text;
				obj.InvLabPreProductionExportLotInspectionParametersDetailList.Add(vd2);
			}
			rows = grdLotSampling.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ThirdPartyTypeId"].Value) > 0 && Conversion.ToInt(r.Cells["NoOfSample"].Value) > 0 && Conversion.ToDouble(r.Cells["SampleWeight_Kg"].Value) > 0.0)
				{
					ThirdPartyInspectionLotSamplingDetail vd = new ThirdPartyInspectionLotSamplingDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ThirdPartyTypeId = Conversion.ToInt(r.Cells["ThirdPartyTypeId"].Value);
					vd.PartyTypeName = Conversion.ToString(r.Cells["ThirdPartyType"].Value);
					vd.NoOfSample = Conversion.ToInt(r.Cells["NoOfSample"].Value);
					vd.SampleWeight_Kg = Conversion.ToDouble(r.Cells["SampleWeight_Kg"].Value);
					vd.TotalWeight_Kg = Conversion.ToDouble(r.Cells["TotalWeight_Kg"].Value);
					vd.Remarks = r.Cells["Remarks"].Text;
					obj.ThirdPartyInspectionLotSamplingDetailList.Add(vd);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvLabPreProductionExportLotInspectionHeader>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			InvLabPreProductionExportLotInspectionHeader.Save(obj, RemoveDetailIds);
			if (RecId == 0)
			{
				MessageBox.Show("Record Saved Successfully");
			}
			else
			{
				MessageBox.Show("Record Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			Reset();
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

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			InvLabPreProductionExportLotInspectionHeader obj = InvLabPreProductionExportLotInspectionHeader.GetByID(ID);
			if (obj == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RadAll.Checked = true;
			((Control)(object)CmbLotTrackingNo).Text = obj.LotRefNo;
			txtRequestLodgDate.Value = Conversion.ToDateTime((object)obj.RequestDate);
			txtRequestReferenceNo.Text = obj.RequestRefNo;
			CmbRequestedBy.Value = obj.RequestedById;
			((Control)(object)CmbRequestedBy).Text = obj.RequestedBy;
			CmbMedium.Value = obj.RequestMediumId;
			((Control)(object)CmbMedium).Text = obj.RequestMedium;
			if (obj.JobLotId > 0)
			{
				CmbJobLot.Value = obj.JobLotId;
			}
			txtExporterLotNo.Text = obj.ExporterLotRefNo;
			CmbItemMain.Value = obj.ItemId;
			txtHeaderMtons.Text = obj.QtyKgs.ToString("#,##0.##");
			CmbInspectionAgency.Value = obj.InspectionAgencyId;
			CmbSamplingResponsibity.Value = obj.SamplingResponsibilityId;
			CmbCountryOfOrigin.Value = obj.CountryOfOriginId;
			CmbCountryOfInspection.Value = obj.CountryOfInspectionId;
			txtCourierTrackingNo.Text = obj.CourierTrackingNo;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.SampleDispatchedDate)))
			{
				txtSampleDispatchedDate.Value = Conversion.ToDateTime((object)obj.SampleDispatchedDate);
				txtSampleDispatchedDate.Checked = true;
			}
			else
			{
				txtSampleDispatchedDate.Checked = false;
			}
			txtTransitDays.Text = obj.TransitDays.ToString();
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.SampleATADestinationDate)))
			{
				txtSampleATADesDate.Value = Conversion.ToDateTime((object)obj.SampleATADestinationDate);
				txtSampleATADesDate.Checked = true;
			}
			else
			{
				txtSampleATADesDate.Checked = false;
			}
			txtInspectionRemarks.Text = obj.InspectionRemarks;
			txtReportCertificateNo.Text = obj.ReportRefNo;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.ReportDate)))
			{
				txtReportDate.Value = Conversion.ToDateTime((object)obj.ReportDate);
				txtReportDate.Checked = true;
			}
			else
			{
				txtReportDate.Checked = false;
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.DateOfInspection)))
			{
				txtDateOfInspection.Value = Conversion.ToDateTime((object)obj.DateOfInspection);
				txtDateOfInspection.Checked = true;
			}
			else
			{
				txtDateOfInspection.Checked = false;
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.ResultDate)))
			{
				txtResultDate.Value = Conversion.ToDateTime((object)obj.ResultDate);
				txtResultDate.Checked = true;
			}
			else
			{
				txtResultDate.Checked = false;
			}
			if (obj.ExImFarmingNTradeId > 0)
			{
				CmbFarmingNTrade.Value = obj.ExImFarmingNTradeId;
			}
			((Control)(object)CmbResultStatus).Text = obj.ReportStatus;
			CmbResultRemarks.Value = obj.ReportResultRemarksId;
			CmbLotCurrentStage.Text = obj.LotCurrentStage;
			CmbRequiredAnalysisGroup.Value = obj.RequiredAnalysisId;
			CmbRequiredAnalysisGroup_Leave(null, null);
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.StockReservedDate)))
			{
				txtStockReservedDate.Value = Conversion.ToDateTime((object)obj.StockReservedDate);
				txtStockReservedDate.Checked = true;
			}
			else
			{
				txtStockReservedDate.Checked = false;
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.SampleTakenDate)))
			{
				txtSampleTakenDate.Value = Conversion.ToDateTime((object)obj.SampleTakenDate);
				txtSampleTakenDate.Checked = true;
			}
			else
			{
				txtSampleTakenDate.Checked = false;
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.StockRSealedDate)))
			{
				txtStockSealedDate.Value = Conversion.ToDateTime((object)obj.StockRSealedDate);
				txtStockSealedDate.Checked = true;
			}
			else
			{
				txtStockSealedDate.Checked = false;
			}
			CmbSampleHandOver.Value = obj.SampleHandedOverId;
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.SampleHandedOverDate)))
			{
				txtHandOverDate.Value = Conversion.ToDateTime((object)obj.SampleHandedOverDate);
				txtHandOverDate.Checked = true;
			}
			else
			{
				txtHandOverDate.Checked = false;
			}
			txtInstructionRemarks.Text = Conversion.ToString((object)obj.LotInstructionsOrRemarks);
			txtLabRemarks.Text = obj.LabRemarks;
			dtdetail.Rows.Clear();
			foreach (InvLabPreProductionExportLotInspectionDetail detail2 in obj.LotInspectionDetails)
			{
				dtdetail.Rows.Add(detail2.Id, detail2.SubLot, detail2.SupplierCustomerId, detail2.SupplierCustomer, detail2.ContractId, detail2.ContractNo, detail2.ContractScheduleId, detail2.ContractScheduleNo, detail2.InvoiceId, detail2.InvoiceNo, detail2.Weight, detail2.SealNoBuyer, detail2.SealNoShipper, detail2.SubRemarks, detail2.ReferredStatusId, (detail2.ReferredStatusId == 1) ? "Referred" : "Not Referred");
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtSamplingDetail.Rows.Clear();
			foreach (ThirdPartyInspectionLotSamplingDetail detail in obj.ThirdPartyInspectionLotSamplingDetailList)
			{
				dtSamplingDetail.Rows.Add(detail.Id, detail.ThirdPartyTypeId, detail.PartyTypeName, detail.NoOfSample, detail.SampleWeight_Kg, detail.TotalWeight_Kg, Conversion.ToString((object)detail.SealNo), detail.Remarks);
			}
			AddPartyTypeInLotSampling(ThirdPartyType.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId, 0, false));
			grdLotSampling.DataSource = dtSamplingDetail;
			grdLotSampling.RetrieveStructure();
			grdSettingLotSampling();
			Dictionary<int, string> paramDict = obj.InvLabPreProductionExportLotInspectionParametersDetailList.ToDictionary((InvLabPreProductionExportLotInspectionParametersDetail p) => p.ParameterId, (InvLabPreProductionExportLotInspectionParametersDetail p) => p.Remarks);
			GridEXRow[] rows = grdParameters.GetRows();
			foreach (GridEXRow row in rows)
			{
				int paramId = Conversion.ToInt(row.Cells["ParameterId"].Value);
				row.BeginEdit();
				if (paramDict.TryGetValue(paramId, out var remarks))
				{
					row.IsChecked = true;
					row.Cells["Remarks"].Value = remarks;
				}
				else
				{
					row.IsChecked = false;
					row.Cells["Remarks"].Value = string.Empty;
				}
			}
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			txtRequestLodgDate.Focus();
			GetContractsInformation();
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
				throw new Exception("RecId not Found...");
			}
			Insert();
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
			txtSubLot.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)cmbContractNo).Text = string.Empty;
			((UltraGridBase)cmbContractNo).DataSource = null;
			((Control)(object)CmbInvoiceNoDetail).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNoDetail).DataSource = null;
			((Control)(object)CmbScheduleNo).Text = string.Empty;
			((UltraGridBase)CmbScheduleNo).DataSource = null;
			txtMtons.Text = string.Empty;
			txtSealNoForBuyer.Text = string.Empty;
			txtSealNoForShipper.Text = string.Empty;
			txtDetailRemarks.Text = string.Empty;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetial.Visible = false;
			txtSubLot.Focus();
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
			RecId = 0;
			AnalysisGroupId = 0;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			RemoveDetailIds = "";
			((Control)(object)CmbLotTrackingNo).Text = "";
			txtRequestReferenceNo.Text = string.Empty;
			txtExporterLotNo.Text = string.Empty;
			((Control)(object)CmbItemMain).Text = string.Empty;
			txtCourierTrackingNo.Text = string.Empty;
			txtTransitDays.Text = string.Empty;
			txtInspectionRemarks.Text = string.Empty;
			txtReportCertificateNo.Text = string.Empty;
			((Control)(object)CmbResultStatus).Text = string.Empty;
			((Control)(object)CmbRequiredAnalysisGroup).Text = "";
			dtParamsdetail.Clear();
			grdParameters.ClearStructure();
			((Control)(object)CmbSampleHandOver).Text = string.Empty;
			txtLabRemarks.Text = string.Empty;
			txtSampleDispatchedDate.Checked = false;
			txtSampleETADesDate.Checked = false;
			txtSampleATADesDate.Checked = false;
			txtReportDate.Checked = false;
			txtDateOfInspection.Checked = false;
			txtResultDate.Checked = false;
			txtStockReservedDate.Checked = false;
			txtSampleTakenDate.Checked = false;
			txtStockSealedDate.Checked = false;
			txtHandOverDate.Checked = false;
			((Control)(object)CmbFarmingNTrade).Text = string.Empty;
			txtInstructionRemarks.Text = string.Empty;
			ResetDetail();
			dtdetail.Rows.Clear();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtSamplingDetail.Rows.Clear();
			grdLotSampling.DataSource = dtSamplingDetail;
			grdLotSampling.RetrieveStructure();
			grdSettingLotSampling();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			if (((UltraGridBase)CmbResultStatus).DataSource != null)
			{
				((UltraGridBase)CmbResultStatus).Rows[2].Activate();
			}
			TackingNoBind(TackingDbCall());
			grdContractInfo.DataSource = null;
			txtRequestLodgDate.Focus();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "JobLot", "AllItems", "AllCities" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			ExportComboBindFromLookup(ExportLookDtDbCall());
			CustomerFromContractBind(CustomerFromContractDbCall());
			OriginAndInspectionCountryBind(CountryDtDbCall());
			AnalysisGroupBind(AnalysisGroupdbCall());
			FarmingNTradeBind(FarmingNTradeDbCall());
			ItemDtsFillFromGlobal();
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmThirdPartyInspection_FormClosed(object sender, FormClosedEventArgs e)
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

	private void ETADate()
	{
		try
		{
			if (txtTransitDays.Text.Trim() != string.Empty && txtTransitDays.Text != null)
			{
				txtSampleETADesDate.Value = txtSampleDispatchedDate.Value.AddDays(Conversion.ToDouble((object)txtTransitDays.Text.Trim()));
			}
			else
			{
				txtSampleETADesDate.Value = txtSampleDispatchedDate.Value;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InstertMtonInHeader()
	{
	}

	private void txtSampleDispatchedDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			ETADate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				CmbLotTrackingNo.Focus();
			}
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
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
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
			obj.Id = Conversion.ToInt(CmbTrackingNoHistory.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			obj.ScreenName = ScreenName;
			if (RadRefferedHistory.Checked)
			{
				obj.ActionId = 1;
			}
			else if (RadNotRefferedHistory.Checked)
			{
				obj.ActionId = 2;
			}
			DataTable dthistory = InvLabPreProductionExportLotInspectionHeader.FormHistory(obj);
			if (dthistory.Rows.Count > 0)
			{
				if (dtcol.Columns.Count == 0)
				{
					dtcol.Columns.Clear();
					dtcol.Columns.Add("Id", typeof(int));
					dtcol.Columns.Add("LotRefNo");
					dtcol.Columns.Add("RequestDate", typeof(DateTime));
					dtcol.Columns.Add("RequestRefNo");
					dtcol.Columns.Add("RequestedByName");
					dtcol.Columns.Add("RequestedBy");
					dtcol.Columns.Add("RequestMedium");
					dtcol.Columns.Add("ExporterLotRefNo");
					dtcol.Columns.Add("ItemName");
					dtcol.Columns.Add("M.Ton", typeof(double));
					dtcol.Columns.Add("InspectionAgency");
					dtcol.Columns.Add("CountryOfOrigin");
					dtcol.Columns.Add("CountryOfInspection");
					dtcol.Columns.Add("PlaceOfInspection");
					dtcol.Columns.Add("CourierTrackingNo");
					dtcol.Columns.Add("SampleDispatchedDate", typeof(DateTime));
					dtcol.Columns.Add("TransitDays", typeof(int));
					dtcol.Columns.Add("SampleETADestinationDate", typeof(DateTime));
					dtcol.Columns.Add("SampleATADestinationDate", typeof(DateTime));
					dtcol.Columns.Add("InspectionRemarks");
					dtcol.Columns.Add("ReportRefNo");
					dtcol.Columns.Add("ReportDate", typeof(DateTime));
					dtcol.Columns.Add("DateOfInspection", typeof(DateTime));
					dtcol.Columns.Add("ReportStatus");
					dtcol.Columns.Add("LotCurrentStage");
					dtcol.Columns.Add("RequiredAnalysisGroup");
					dtcol.Columns.Add("StockReservedDate", typeof(DateTime));
					dtcol.Columns.Add("SampleTakenDate", typeof(DateTime));
					dtcol.Columns.Add("StockRSealedDate", typeof(DateTime));
					dtcol.Columns.Add("SampleHandedOver");
					dtcol.Columns.Add("SampleHandedOverDate", typeof(DateTime));
					dtcol.Columns.Add("LabRemarks");
					dtcol.Columns.Add("SampleStatus");
					dtcol.Columns.Add("EntryUserName");
					dtcol.Columns.Add("EntryDate", typeof(DateTime));
					dtcol.Columns.Add("ModifyUserName");
					dtcol.Columns.Add("ModifyDate", typeof(DateTime));
					dtcol.Columns.Add("ApprovedUser");
					dtcol.Columns.Add("ApprovedDate", typeof(DateTime));
					dtcol.Columns.Add("NoOfAttachments");
				}
				dtcol.Clear();
				foreach (DataRow row in dthistory.Rows)
				{
					dtcol.Rows.Add(row["Id"], row["LotRefNo"], row["RequestDate"], row["RequestRefNo"], row["RequestedByName"], row["RequestedBy"], row["RequestMedium"], row["ExporterLotRefNo"], row["ItemName"], row["QtyKgs"], row["InspectionAgencyName"], row["CountryOfOrigin"], row["CountryOfInspection"], row["PlaceOfInspection"], row["CourierTrackingNo"], row["SampleDispatchedDate"], row["TransitDays"], row["SampleETADestinationDate"], row["SampleATADestinationDate"], row["InspectionRemarks"], row["ReportRefNo"], row["ReportDate"], row["DateOfInspection"], row["ReportStatus"], row["LotCurrentStage"], row["RequiredAnalysisGroup"], row["StockReservedDate"], row["SampleTakenDate"], row["StockRSealedDate"], row["SampleHandedOver"], row["SampleHandedOverDate"], row["LabRemarks"], row["SampleStatus"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["ApprovedUser"], row["ApprovedDate"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 3);
			DataGridHistory.RootTable.Columns["LotRefNo"].Width = Constants.InventoryConstants.JobLotConstant;
			DataGridHistory.RootTable.Columns["RequestDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["RequestRefNo"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["RequestedByName"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["RequestedBy"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["RequestMedium"].Width = 65;
			DataGridHistory.RootTable.Columns["ExporterLotRefNo"].Width = Constants.InventoryConstants.RecordNo;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["M.Ton"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["InspectionAgency"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["CountryOfOrigin"].Width = Constants.InventoryConstants.CityName;
			DataGridHistory.RootTable.Columns["CountryOfInspection"].Width = Constants.InventoryConstants.CityName;
			DataGridHistory.RootTable.Columns["PlaceOfInspection"].Width = Constants.InventoryConstants.CityName;
			DataGridHistory.RootTable.Columns["CourierTrackingNo"].Width = 70;
			DataGridHistory.RootTable.Columns["SampleDispatchedDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["TransitDays"].Width = 60;
			DataGridHistory.RootTable.Columns["SampleETADestinationDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["SampleATADestinationDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["InspectionRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["ReportRefNo"].Width = Constants.InventoryConstants.RecordNo;
			DataGridHistory.RootTable.Columns["ReportDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DateOfInspection"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["ReportStatus"].Width = Constants.InventoryConstants.StatusConstant;
			DataGridHistory.RootTable.Columns["LotCurrentStage"].Width = Constants.InventoryConstants.StatusConstant;
			DataGridHistory.RootTable.Columns["RequiredAnalysisGroup"].Width = 100;
			DataGridHistory.RootTable.Columns["StockReservedDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["SampleTakenDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["StockRSealedDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["SampleHandedOver"].Width = Constants.InventoryConstants.ReferenceParty;
			DataGridHistory.RootTable.Columns["SampleHandedOverDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["LabRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 50;
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHaveUpdateRights && formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.RootTable.Columns["Print"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar2_Load(null, null);
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
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				BindDetailByHistoryHeader(Id);
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindDetailByHistoryHeader(int Id)
	{
		try
		{
			if (Id <= 0)
			{
				return;
			}
			InvLabPreProductionExportLotInspectionHeader obj = InvLabPreProductionExportLotInspectionHeader.GetByID(Id);
			if (obj != null && obj.LotInspectionDetails != null && obj.LotInspectionDetails.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Lot");
				dt.Columns.Add("Customer");
				dt.Columns.Add("ContractNo");
				dt.Columns.Add("ContractScheduleNo");
				dt.Columns.Add("InvoiceNo");
				dt.Columns.Add("M.Tons", typeof(double));
				dt.Columns.Add("SealNoForBuyer");
				dt.Columns.Add("SealNoForShipper");
				dt.Columns.Add("Remarks");
				dt.Clear();
				foreach (InvLabPreProductionExportLotInspectionDetail detail in obj.LotInspectionDetails)
				{
					dt.Rows.Add(detail.SubLot, detail.SupplierCustomer, detail.ContractNo, detail.ContractScheduleNo, detail.InvoiceNo, detail.Weight, detail.SealNoBuyer, detail.SealNoShipper, detail.SubRemarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Expected O, but got Unknown
		try
		{
			CommonServices.GridWrappingAndColumnSettings(grdDetail, 3);
			grdDetail.RootTable.Columns["Lot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdDetail.RootTable.Columns["Customer"].Width = Constants.InventoryConstants.SupplierName;
			grdDetail.RootTable.Columns["ContractNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grdDetail.RootTable.Columns["ContractScheduleNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grdDetail.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.ContractNoConstant;
			grdDetail.RootTable.Columns["M.Tons"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["SealNoForBuyer"].Width = Constants.InventoryConstants.SealNo;
			grdDetail.RootTable.Columns["SealNoForShipper"].Width = Constants.InventoryConstants.SealNo;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEXColumn column = new GridEXColumn();
			column = grdDetail.RootTable.Columns["InvoiceNo"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)14, (object)null);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grdDetail.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReport(Id);
				}
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Id, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), ScreenName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall(string Activity = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return InvLabPreProductionExportLotInspectionHeader.GetDataForDropDownFrom_LabPreExportLotInspection(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = Activity
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboFill(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("name", typeof(string));
			DataTable dtTrack = new DataTable();
			dtTrack.Columns.Add("Id", typeof(int));
			dtTrack.Columns.Add("name", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Items")
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
				else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "TrackingNo")
				{
					dtTrack.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtTrack, CmbTrackingNoHistory, "Id", "name", "TrackingNo", ZeroIndex: false);
			DDL.BindDDL(dtitem, CmbItemHistory, "Id", "name", "Item", ZeroIndex: false);
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			((Control)(object)CmbTrackingNoHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grdDetail.DataSource = null;
			drdocdate.Checked = true;
			FromDateHistory.Focus();
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
			HistoryComboFill(HistoryComboDbCall());
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
			if (RecId > 0)
			{
				GenerateReport(RecId);
				return;
			}
			throw new Exception("No Record Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int id)
	{
		try
		{
			CommonServices.ThirdPartyInspectionSlip514(id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintStockReserved_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			DataTable dt = new DataTable();
			dt = InventoryStockReserved.InventoryStockReserved_SlipAndRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RefDocIdNo = RecId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "514_01_InventoryStockReservedSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnExportLookUP_Click(object sender, EventArgs e)
	{
		try
		{
			ExImLookups exImLookups = new ExImLookups(UserAccount);
			exImLookups.LookupId = 8;
			((Control)(object)exImLookups.CmbLookupsType).Enabled = false;
			exImLookups.StartPosition = FormStartPosition.CenterParent;
			exImLookups.BringToFront();
			exImLookups.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnTrackingReport_Click(object sender, EventArgs e)
	{
		try
		{
			frmThirdPartyInspectionLotTrackingReport frmThirdPartyInspectionLotTrackingReport = new frmThirdPartyInspectionLotTrackingReport(UserAccount);
			frmThirdPartyInspectionLotTrackingReport.BringToFront();
			frmThirdPartyInspectionLotTrackingReport.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnStockReservedForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmStockReservedAgainstThirdPartyInspection obj = new frmStockReservedAgainstThirdPartyInspection(UserAccount);
			obj.BringToFront();
			obj.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineJobLot_Click(object sender, EventArgs e)
	{
		try
		{
			AcfrmDefineLots acfrmDefineLots = new AcfrmDefineLots(UserAccount);
			acfrmDefineLots.StartPosition = FormStartPosition.CenterParent;
			acfrmDefineLots.BringToFront();
			acfrmDefineLots.ShowDialog();
			clsGlobalVariables.globalJobLot = CommonServices.GetJobLotGlIdsandName(UserAccount.OrganizationId, UserAccount.CompanyId);
			JobLotDtFillFromGlobal();
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
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					if (formrights.DoHavePrintRights)
					{
						GenerateReport(Id);
					}
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(Id, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(item);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmThirdPartyInspection_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && btnsave.Visible)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && RecId > 0)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5 && tabControl1.SelectedIndex == 0)
			{
				CmbLotTrackingNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10 && tabControl1.SelectedIndex == 0)
			{
				btnAttachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R && tabControl1.SelectedIndex == 0)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbLotTrackingNo.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up && tabControl1.SelectedIndex == 0)
			{
				txtSubLot.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down && tabControl1.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on LotTrackingNo");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On SubLot in Detail Box");
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

	private void frmThirdPartyInspection_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1050, 800);
	}

	private void btnCityDefinition_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCountry(UserAccount).Show();
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
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a79: Expected O, but got Unknown
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Expected O, but got Unknown
		//IL_0b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b97: Expected O, but got Unknown
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba2: Expected O, but got Unknown
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Expected O, but got Unknown
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb8: Expected O, but got Unknown
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Expected O, but got Unknown
		//IL_0bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bce: Expected O, but got Unknown
		//IL_0d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0d: Expected O, but got Unknown
		//IL_0d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2e: Expected O, but got Unknown
		//IL_0d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da7: Expected O, but got Unknown
		//IL_0db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbd: Expected O, but got Unknown
		//IL_150b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1515: Expected O, but got Unknown
		//IL_ad34: Unknown result type (might be due to invalid IL or missing references)
		//IL_ad3e: Expected O, but got Unknown
		//IL_ad62: Unknown result type (might be due to invalid IL or missing references)
		//IL_ad6c: Expected O, but got Unknown
		GridEXLayout grdContractInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance169 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		GridEXLayout grdLotSampling_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdParameters_DesignTimeLayout = new GridEXLayout();
		Appearance appearance154 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
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
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
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
		Appearance appearance238 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmThirdPartyInspection));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdContractInfo = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.txtDetailRemarks = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label59 = new System.Windows.Forms.Label();
		this.cmbContractNo = new UltraCombo();
		this.txtSubLot = new System.Windows.Forms.TextBox();
		this.CmbScheduleNo = new UltraCombo();
		this.txtSealNoForShipper = new System.Windows.Forms.TextBox();
		this.CmbInvoiceNoDetail = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtSealNoForBuyer = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancelDetial = new System.Windows.Forms.Button();
		this.CmbCustomer = new UltraCombo();
		this.txtMtons = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.txtInstructionRemarks = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdLotSampling = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label60 = new System.Windows.Forms.Label();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.grdParameters = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.CmbRequiredAnalysisGroup = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BtnInspectionTeam2 = new System.Windows.Forms.Button();
		this.txtHandOverDate = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtLabRemarks = new System.Windows.Forms.TextBox();
		this.txtStockSealedDate = new System.Windows.Forms.DateTimePicker();
		this.label43 = new System.Windows.Forms.Label();
		this.CmbSampleHandOver = new UltraCombo();
		this.txtSampleTakenDate = new System.Windows.Forms.DateTimePicker();
		this.label42 = new System.Windows.Forms.Label();
		this.txtStockReservedDate = new System.Windows.Forms.DateTimePicker();
		this.label41 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtInspectionRemarks = new System.Windows.Forms.TextBox();
		this.CmbResultRemarks = new UltraCombo();
		this.txtSampleDispatchedDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.txtSampleETADesDate = new System.Windows.Forms.DateTimePicker();
		this.CmbResultStatus = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.txtReportDate = new System.Windows.Forms.DateTimePicker();
		this.label40 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbLotCurrentStage = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtSampleATADesDate = new System.Windows.Forms.DateTimePicker();
		this.label23 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtTransitDays = new System.Windows.Forms.TextBox();
		this.txtDateOfInspection = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtCourierTrackingNo = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.txtReportCertificateNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label61 = new System.Windows.Forms.Label();
		this.CmbFarmingNTrade = new UltraCombo();
		this.panel11 = new System.Windows.Forms.Panel();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.label58 = new System.Windows.Forms.Label();
		this.btnDefineJobLot = new System.Windows.Forms.Button();
		this.CmbCountryOfInspection = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label57 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.BtnInspectionTeam = new System.Windows.Forms.Button();
		this.CmbSamplingResponsibity = new UltraCombo();
		this.txtHeaderMtons = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbLotTrackingNo = new UltraCombo();
		this.CmbRequestedBy = new UltraCombo();
		this.CmbCountryOfOrigin = new UltraCombo();
		this.CmbInspectionAgency = new UltraCombo();
		this.CmbMedium = new UltraCombo();
		this.CmbItemMain = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtRequestReferenceNo = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtRequestLodgDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtExporterLotNo = new System.Windows.Forms.TextBox();
		this.panel28 = new System.Windows.Forms.Panel();
		this.label66 = new System.Windows.Forms.Label();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintStockReserved = new System.Windows.Forms.ToolStripButton();
		this.BtnStockReservedForm = new System.Windows.Forms.ToolStripButton();
		this.BtnTrackingReport = new System.Windows.Forms.ToolStripButton();
		this.btnCountryDefinition = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.RadNotRefferedHistory = new System.Windows.Forms.RadioButton();
		this.RadRefferedHistory = new System.Windows.Forms.RadioButton();
		this.RadAllHistory = new System.Windows.Forms.RadioButton();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbItemHistory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.CmbTrackingNoHistory = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label18 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txtResultDate = new System.Windows.Forms.DateTimePicker();
		this.label63 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractInfo).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		this.panel3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdLotSampling).BeginInit();
		this.panel10.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParameters).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequiredAnalysisGroup).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleHandOver).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbResultRemarks).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbResultStatus).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryOfInspection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSamplingResponsibity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryOfOrigin).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionAgency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMedium).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemMain).BeginInit();
		this.panel28.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTrackingNoHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1362, 754);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.panel28);
		this.tabPage1.Controls.Add(this.toolstrip);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1354, 725);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractInfo);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 476);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1348, 246);
		this.panel2.TabIndex = 563;
		this.grdContractInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdContractInfo).BackColor = System.Drawing.Color.White;
		this.grdContractInfo.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdContractInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractInfo.DesignTimeLayout = grdContractInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractInfo).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdContractInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractInfo).Location = new System.Drawing.Point(962, 28);
		((System.Windows.Forms.Control)(object)this.grdContractInfo).Name = "grdContractInfo";
		((ScrollableBase)this.grdContractInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractInfo).Size = new System.Drawing.Size(386, 218);
		((System.Windows.Forms.Control)(object)this.grdContractInfo).TabIndex = 517;
		this.grdContractInfo.TotalRow = (InheritableBoolean)1;
		this.grdContractInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractInfo).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(0, 28);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(962, 218);
		this.panel6.TabIndex = 0;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 95);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(962, 123);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel12.Controls.Add(this.txtDetailRemarks);
		this.panel12.Controls.Add(this.btnUpdateDetail);
		this.panel12.Controls.Add(this.label59);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContractNo);
		this.panel12.Controls.Add(this.txtSubLot);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleNo);
		this.panel12.Controls.Add(this.txtSealNoForShipper);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail);
		this.panel12.Controls.Add(this.label20);
		this.panel12.Controls.Add(this.label39);
		this.panel12.Controls.Add(this.txtSealNoForBuyer);
		this.panel12.Controls.Add(this.label24);
		this.panel12.Controls.Add(this.label19);
		this.panel12.Controls.Add(this.label22);
		this.panel12.Controls.Add(this.btnAdd);
		this.panel12.Controls.Add(this.btnCancelDetial);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel12.Controls.Add(this.txtMtons);
		this.panel12.Controls.Add(this.label17);
		this.panel12.Controls.Add(this.label34);
		this.panel12.Controls.Add(this.label15);
		this.panel12.Controls.Add(this.label16);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(962, 95);
		this.panel12.TabIndex = 517;
		this.txtDetailRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarks.Location = new System.Drawing.Point(362, 69);
		this.txtDetailRemarks.Name = "txtDetailRemarks";
		this.txtDetailRemarks.Size = new System.Drawing.Size(459, 23);
		this.txtDetailRemarks.TabIndex = 8;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(823, 66);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(66, 26);
		this.btnUpdateDetail.TabIndex = 10;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(941, 6);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(16, 14);
		this.label59.TabIndex = 516;
		this.label59.Text = "*";
		this.cmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Location = new System.Drawing.Point(362, 24);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Name = "cmbContractNo";
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Leave += new System.EventHandler(cmbContractNo_Leave);
		this.txtSubLot.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubLot.Location = new System.Drawing.Point(6, 26);
		this.txtSubLot.Name = "txtSubLot";
		this.txtSubLot.Size = new System.Drawing.Size(144, 23);
		this.txtSubLot.TabIndex = 0;
		this.CmbScheduleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance197;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance208).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance208).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance208).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Location = new System.Drawing.Point(541, 24);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Name = "CmbScheduleNo";
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNo).Leave += new System.EventHandler(CmbScheduleNo_Leave);
		this.txtSealNoForShipper.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSealNoForShipper.Location = new System.Drawing.Point(152, 69);
		this.txtSealNoForShipper.Name = "txtSealNoForShipper";
		this.txtSealNoForShipper.Size = new System.Drawing.Size(208, 23);
		this.txtSealNoForShipper.TabIndex = 7;
		this.CmbInvoiceNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Location = new System.Drawing.Point(700, 24);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Name = "CmbInvoiceNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoDetail).Leave += new System.EventHandler(CmbInvoiceNoDetail_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(152, 52);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(95, 15);
		this.label20.TabIndex = 493;
		this.label20.Text = "Shipper Seal No.\r\n";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(363, 6);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(85, 15);
		this.label39.TabIndex = 476;
		this.label39.Text = "Buyer P.O / Ref\r\n";
		this.txtSealNoForBuyer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSealNoForBuyer.Location = new System.Drawing.Point(6, 69);
		this.txtSealNoForBuyer.Name = "txtSealNoForBuyer";
		this.txtSealNoForBuyer.Size = new System.Drawing.Size(144, 23);
		this.txtSealNoForBuyer.TabIndex = 6;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(542, 6);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(126, 15);
		this.label24.TabIndex = 476;
		this.label24.Text = "Contract Schedule No.\r\n";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(6, 52);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(107, 15);
		this.label19.TabIndex = 491;
		this.label19.Text = "Inspection Seal No\r\n";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(701, 6);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(68, 15);
		this.label22.TabIndex = 476;
		this.label22.Text = "Invoice No.\t\r\n";
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderSize = 0;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(823, 66);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(23, 26);
		this.btnAdd.TabIndex = 9;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnCancelDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetial.Location = new System.Drawing.Point(890, 66);
		this.btnCancelDetial.Name = "btnCancelDetial";
		this.btnCancelDetial.Size = new System.Drawing.Size(62, 26);
		this.btnCancelDetial.TabIndex = 11;
		this.btnCancelDetial.Text = "Cancel";
		this.btnCancelDetial.UseVisualStyleBackColor = false;
		this.btnCancelDetial.Visible = false;
		this.btnCancelDetial.Click += new System.EventHandler(btnCancelDetial_Click);
		this.btnCancelDetial.Leave += new System.EventHandler(btnCancelDetial_Leave);
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance200).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(152, 24);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.txtMtons.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMtons.Location = new System.Drawing.Point(856, 26);
		this.txtMtons.Name = "txtMtons";
		this.txtMtons.Size = new System.Drawing.Size(101, 23);
		this.txtMtons.TabIndex = 5;
		this.txtMtons.Text = "0";
		this.txtMtons.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMtons.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtMtons_KeyPress);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(8, 6);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 470;
		this.label17.Text = "Sub Lot";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(362, 52);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(52, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Remarks\r\n";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(155, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(99, 15);
		this.label15.TabIndex = 476;
		this.label15.Text = "Customer / Buyer";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(860, 6);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(46, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "M.Tons\t\r\n";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1348, 28);
		this.panel3.TabIndex = 562;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1306, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 562;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(516, 19);
		this.label14.TabIndex = 561;
		this.label14.Text = "Generate Sub-Lots and Map to Shipment Schedule and Invoice";
		this.panel1.Controls.Add(this.txtInstructionRemarks);
		this.panel1.Controls.Add(this.label62);
		this.panel1.Controls.Add(this.panel9);
		this.panel1.Controls.Add(this.groupBox4);
		this.panel1.Controls.Add(this.groupBox3);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 58);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1348, 418);
		this.panel1.TabIndex = 0;
		this.txtInstructionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInstructionRemarks.Location = new System.Drawing.Point(499, 392);
		this.txtInstructionRemarks.Name = "txtInstructionRemarks";
		this.txtInstructionRemarks.Size = new System.Drawing.Size(491, 23);
		this.txtInstructionRemarks.TabIndex = 4;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(361, 395);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(126, 15);
		this.label62.TabIndex = 510;
		this.label62.Text = "Instructions / Remarks";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdLotSampling);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Enabled = false;
		this.panel9.Location = new System.Drawing.Point(994, 8);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(351, 398);
		this.panel9.TabIndex = 5;
		this.grdLotSampling.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdLotSampling).BackColor = System.Drawing.Color.White;
		this.grdLotSampling.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdLotSampling_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdLotSampling.DesignTimeLayout = grdLotSampling_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdLotSampling).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdLotSampling.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdLotSampling).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdLotSampling.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdLotSampling).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdLotSampling).Name = "grdLotSampling";
		((ScrollableBase)this.grdLotSampling).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdLotSampling).Size = new System.Drawing.Size(351, 375);
		((System.Windows.Forms.Control)(object)this.grdLotSampling).TabIndex = 563;
		this.grdLotSampling.TotalRow = (InheritableBoolean)1;
		this.grdLotSampling.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdLotSampling).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label60);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(351, 23);
		this.panel10.TabIndex = 562;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label60.ForeColor = System.Drawing.Color.White;
		this.label60.Location = new System.Drawing.Point(3, 2);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(98, 17);
		this.label60.TabIndex = 561;
		this.label60.Text = "Lot Sampling";
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.grdParameters);
		this.groupBox4.Controls.Add(this.panel8);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.groupBox4.Location = new System.Drawing.Point(693, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(299, 156);
		this.groupBox4.TabIndex = 2;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Required Analysis";
		this.grdParameters.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdParameters).BackColor = System.Drawing.Color.White;
		this.grdParameters.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdParameters_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdParameters.DesignTimeLayout = grdParameters_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdParameters).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParameters.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParameters).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParameters.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdParameters).Location = new System.Drawing.Point(3, 51);
		((System.Windows.Forms.Control)(object)this.grdParameters).Name = "grdParameters";
		((ScrollableBase)this.grdParameters).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParameters).Size = new System.Drawing.Size(293, 102);
		((System.Windows.Forms.Control)(object)this.grdParameters).TabIndex = 0;
		this.grdParameters.TotalRow = (InheritableBoolean)1;
		this.grdParameters.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParameters).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup);
		this.panel8.Controls.Add(this.label7);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 18);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(293, 33);
		this.panel8.TabIndex = 498;
		this.CmbRequiredAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbRequiredAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequiredAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequiredAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance225).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequiredAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequiredAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).Location = new System.Drawing.Point(94, 3);
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).Name = "CmbRequiredAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbRequiredAnalysisGroup).Leave += new System.EventHandler(CmbRequiredAnalysisGroup_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 497;
		this.label7.Text = "Analysis Group";
		this.groupBox3.Controls.Add(this.BtnInspectionTeam2);
		this.groupBox3.Controls.Add(this.txtHandOverDate);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.txtLabRemarks);
		this.groupBox3.Controls.Add(this.txtStockSealedDate);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleHandOver);
		this.groupBox3.Controls.Add(this.txtSampleTakenDate);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.txtStockReservedDate);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Enabled = false;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(694, 161);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(299, 230);
		this.groupBox3.TabIndex = 3;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Lab Information";
		this.BtnInspectionTeam2.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnInspectionTeam2.Location = new System.Drawing.Point(270, 95);
		this.BtnInspectionTeam2.Name = "BtnInspectionTeam2";
		this.BtnInspectionTeam2.Size = new System.Drawing.Size(26, 26);
		this.BtnInspectionTeam2.TabIndex = 4;
		this.BtnInspectionTeam2.TabStop = false;
		this.BtnInspectionTeam2.UseVisualStyleBackColor = true;
		this.BtnInspectionTeam2.Click += new System.EventHandler(btnExportLookUP_Click);
		this.txtHandOverDate.Checked = false;
		this.txtHandOverDate.CustomFormat = "dd-MMM-yyyy";
		this.txtHandOverDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHandOverDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtHandOverDate.Location = new System.Drawing.Point(125, 124);
		this.txtHandOverDate.Name = "txtHandOverDate";
		this.txtHandOverDate.ShowCheckBox = true;
		this.txtHandOverDate.Size = new System.Drawing.Size(171, 23);
		this.txtHandOverDate.TabIndex = 5;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(6, 155);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(74, 15);
		this.label46.TabIndex = 509;
		this.label46.Text = "Lab Remarks\r\n";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(6, 130);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(105, 15);
		this.label45.TabIndex = 509;
		this.label45.Text = "Handed Over Date\r\n";
		this.txtLabRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLabRemarks.Location = new System.Drawing.Point(125, 148);
		this.txtLabRemarks.Multiline = true;
		this.txtLabRemarks.Name = "txtLabRemarks";
		this.txtLabRemarks.Size = new System.Drawing.Size(171, 68);
		this.txtLabRemarks.TabIndex = 6;
		this.txtStockSealedDate.Checked = false;
		this.txtStockSealedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtStockSealedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockSealedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStockSealedDate.Location = new System.Drawing.Point(125, 69);
		this.txtStockSealedDate.Name = "txtStockSealedDate";
		this.txtStockSealedDate.ShowCheckBox = true;
		this.txtStockSealedDate.Size = new System.Drawing.Size(171, 23);
		this.txtStockSealedDate.TabIndex = 2;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(6, 75);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(75, 15);
		this.label43.TabIndex = 509;
		this.label43.Text = "Stock Sealed\r\n";
		this.CmbSampleHandOver.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleHandOver.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleHandOver.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleHandOver.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleHandOver).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleHandOver).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleHandOver.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleHandOver).Location = new System.Drawing.Point(125, 95);
		((System.Windows.Forms.Control)(object)this.CmbSampleHandOver).Name = "CmbSampleHandOver";
		((System.Windows.Forms.Control)(object)this.CmbSampleHandOver).Size = new System.Drawing.Size(143, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleHandOver).TabIndex = 3;
		this.txtSampleTakenDate.Checked = false;
		this.txtSampleTakenDate.CustomFormat = "dd-MMM-yyyy";
		this.txtSampleTakenDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSampleTakenDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSampleTakenDate.Location = new System.Drawing.Point(125, 40);
		this.txtSampleTakenDate.Name = "txtSampleTakenDate";
		this.txtSampleTakenDate.ShowCheckBox = true;
		this.txtSampleTakenDate.Size = new System.Drawing.Size(171, 23);
		this.txtSampleTakenDate.TabIndex = 1;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(6, 46);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(81, 15);
		this.label42.TabIndex = 509;
		this.label42.Text = "Sample Taken\r\n";
		this.txtStockReservedDate.Checked = false;
		this.txtStockReservedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtStockReservedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtStockReservedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStockReservedDate.Location = new System.Drawing.Point(125, 14);
		this.txtStockReservedDate.Name = "txtStockReservedDate";
		this.txtStockReservedDate.ShowCheckBox = true;
		this.txtStockReservedDate.Size = new System.Drawing.Size(171, 23);
		this.txtStockReservedDate.TabIndex = 0;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(6, 18);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(87, 15);
		this.label41.TabIndex = 509;
		this.label41.Text = "Stock Reserved\r\n";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(6, 99);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(120, 15);
		this.label44.TabIndex = 495;
		this.label44.Text = "Sample Handed Over\r\n";
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtInspectionRemarks);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbResultRemarks);
		this.groupBox2.Controls.Add(this.txtSampleDispatchedDate);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtSampleETADesDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbResultStatus);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.txtReportDate);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.CmbLotCurrentStage);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.txtSampleATADesDate);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtTransitDays);
		this.groupBox2.Controls.Add(this.txtResultDate);
		this.groupBox2.Controls.Add(this.txtDateOfInspection);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.txtCourierTrackingNo);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.txtReportCertificateNo);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(359, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(331, 389);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Inspection Company";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(6, 148);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 510;
		this.label12.Text = "Remarks";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtInspectionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionRemarks.Location = new System.Drawing.Point(140, 144);
		this.txtInspectionRemarks.Name = "txtInspectionRemarks";
		this.txtInspectionRemarks.Size = new System.Drawing.Size(185, 23);
		this.txtInspectionRemarks.TabIndex = 5;
		this.CmbResultRemarks.AllowNull = (DefaultableBoolean)1;
		this.CmbResultRemarks.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbResultRemarks.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbResultRemarks.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbResultRemarks).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbResultRemarks).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbResultRemarks).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbResultRemarks).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbResultRemarks.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbResultRemarks).Location = new System.Drawing.Point(140, 298);
		((System.Windows.Forms.Control)(object)this.CmbResultRemarks).Name = "CmbResultRemarks";
		((System.Windows.Forms.Control)(object)this.CmbResultRemarks).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbResultRemarks).TabIndex = 11;
		this.txtSampleDispatchedDate.Checked = false;
		this.txtSampleDispatchedDate.CustomFormat = "dd-MMM-yyyy";
		this.txtSampleDispatchedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSampleDispatchedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSampleDispatchedDate.Location = new System.Drawing.Point(140, 44);
		this.txtSampleDispatchedDate.Name = "txtSampleDispatchedDate";
		this.txtSampleDispatchedDate.ShowCheckBox = true;
		this.txtSampleDispatchedDate.Size = new System.Drawing.Size(185, 23);
		this.txtSampleDispatchedDate.TabIndex = 1;
		this.txtSampleDispatchedDate.ValueChanged += new System.EventHandler(txtSampleDispatchedDate_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(6, 48);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(110, 15);
		this.label33.TabIndex = 509;
		this.label33.Text = "Sample Dispatched\r\n";
		this.txtSampleETADesDate.Checked = false;
		this.txtSampleETADesDate.CustomFormat = "dd-MMM-yyyy";
		this.txtSampleETADesDate.Enabled = false;
		this.txtSampleETADesDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSampleETADesDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSampleETADesDate.Location = new System.Drawing.Point(140, 94);
		this.txtSampleETADesDate.Name = "txtSampleETADesDate";
		this.txtSampleETADesDate.ShowCheckBox = true;
		this.txtSampleETADesDate.Size = new System.Drawing.Size(185, 23);
		this.txtSampleETADesDate.TabIndex = 3;
		this.CmbResultStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbResultStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbResultStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbResultStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbResultStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance231;
		((SpecialBoxBase)((UltraGridBase)this.CmbResultStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance232).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance232).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbResultStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbResultStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbResultStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbResultStatus).Location = new System.Drawing.Point(140, 270);
		((System.Windows.Forms.Control)(object)this.CmbResultStatus).Name = "CmbResultStatus";
		((System.Windows.Forms.Control)(object)this.CmbResultStatus).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbResultStatus).TabIndex = 10;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(6, 98);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(134, 15);
		this.label36.TabIndex = 509;
		this.label36.Text = "Sample ETA Destination\r\n";
		this.txtReportDate.Checked = false;
		this.txtReportDate.CustomFormat = "dd-MMM-yyyy";
		this.txtReportDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReportDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtReportDate.Location = new System.Drawing.Point(140, 195);
		this.txtReportDate.Name = "txtReportDate";
		this.txtReportDate.ShowCheckBox = true;
		this.txtReportDate.Size = new System.Drawing.Size(185, 23);
		this.txtReportDate.TabIndex = 7;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 304);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(87, 15);
		this.label40.TabIndex = 497;
		this.label40.Text = "Result Remarks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 199);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(70, 15);
		this.label37.TabIndex = 509;
		this.label37.Text = "Report Date\r\n";
		this.CmbLotCurrentStage.Enabled = false;
		this.CmbLotCurrentStage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLotCurrentStage.Location = new System.Drawing.Point(140, 325);
		this.CmbLotCurrentStage.Multiline = true;
		this.CmbLotCurrentStage.Name = "CmbLotCurrentStage";
		this.CmbLotCurrentStage.Size = new System.Drawing.Size(185, 51);
		this.CmbLotCurrentStage.TabIndex = 12;
		this.CmbLotCurrentStage.Text = "Pending For Inspection\r\n";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 329);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(99, 15);
		this.label9.TabIndex = 507;
		this.label9.Text = "Lot Current Stage\r\n";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(6, 276);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(75, 15);
		this.label38.TabIndex = 497;
		this.label38.Text = "Result Status";
		this.txtSampleATADesDate.Checked = false;
		this.txtSampleATADesDate.CustomFormat = "dd-MMM-yyyy";
		this.txtSampleATADesDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSampleATADesDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSampleATADesDate.Location = new System.Drawing.Point(140, 119);
		this.txtSampleATADesDate.Name = "txtSampleATADesDate";
		this.txtSampleATADesDate.ShowCheckBox = true;
		this.txtSampleATADesDate.Size = new System.Drawing.Size(185, 23);
		this.txtSampleATADesDate.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(6, 123);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(135, 15);
		this.label23.TabIndex = 509;
		this.label23.Text = "Sample ATA Destination\r\n";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 224);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(105, 15);
		this.label6.TabIndex = 509;
		this.label6.Text = "Date of Inspection";
		this.txtTransitDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransitDays.Location = new System.Drawing.Point(140, 69);
		this.txtTransitDays.Name = "txtTransitDays";
		this.txtTransitDays.Size = new System.Drawing.Size(185, 23);
		this.txtTransitDays.TabIndex = 2;
		this.txtTransitDays.Text = "3";
		this.txtTransitDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTransitDays.TextChanged += new System.EventHandler(txtTransitDays_TextChanged);
		this.txtDateOfInspection.Checked = false;
		this.txtDateOfInspection.CustomFormat = "dd-MMM-yyyy";
		this.txtDateOfInspection.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateOfInspection.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateOfInspection.Location = new System.Drawing.Point(140, 220);
		this.txtDateOfInspection.Name = "txtDateOfInspection";
		this.txtDateOfInspection.ShowCheckBox = true;
		this.txtDateOfInspection.Size = new System.Drawing.Size(185, 23);
		this.txtDateOfInspection.TabIndex = 8;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(6, 73);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(71, 15);
		this.label35.TabIndex = 504;
		this.label35.Text = "Transit Days\r\n";
		this.txtCourierTrackingNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCourierTrackingNo.Location = new System.Drawing.Point(140, 19);
		this.txtCourierTrackingNo.Name = "txtCourierTrackingNo";
		this.txtCourierTrackingNo.Size = new System.Drawing.Size(185, 23);
		this.txtCourierTrackingNo.TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(6, 23);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(115, 15);
		this.label32.TabIndex = 502;
		this.label32.Text = "Courier Tracking No.\r\n";
		this.txtReportCertificateNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReportCertificateNo.Location = new System.Drawing.Point(140, 169);
		this.txtReportCertificateNo.Name = "txtReportCertificateNo";
		this.txtReportCertificateNo.Size = new System.Drawing.Size(185, 23);
		this.txtReportCertificateNo.TabIndex = 6;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 173);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(128, 15);
		this.label8.TabIndex = 502;
		this.label8.Text = "Report / Certificate No.\r\n";
		this.groupBox1.Controls.Add(this.label61);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade);
		this.groupBox1.Controls.Add(this.panel11);
		this.groupBox1.Controls.Add(this.label58);
		this.groupBox1.Controls.Add(this.btnDefineJobLot);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label57);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.rdSearchByCode);
		this.groupBox1.Controls.Add(this.rdSearchByName);
		this.groupBox1.Controls.Add(this.label56);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.BtnInspectionTeam);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity);
		this.groupBox1.Controls.Add(this.txtHeaderMtons);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequestedBy);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInspectionAgency);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMedium);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemMain);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtRequestReferenceNo);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtRequestLodgDate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtExporterLotNo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(5, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(352, 415);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Requesting Company (Export Department)";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(5, 365);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(95, 15);
		this.label61.TabIndex = 526;
		this.label61.Text = "Farming N Trade";
		this.CmbFarmingNTrade.AllowNull = (DefaultableBoolean)1;
		this.CmbFarmingNTrade.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFarmingNTrade.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFarmingNTrade.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFarmingNTrade).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFarmingNTrade.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Location = new System.Drawing.Point(151, 359);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Name = "CmbFarmingNTrade";
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbFarmingNTrade).TabIndex = 13;
		this.panel11.Controls.Add(this.RadAll);
		this.panel11.Controls.Add(this.RadNotReffered);
		this.panel11.Controls.Add(this.RadReffered);
		this.panel11.Location = new System.Drawing.Point(151, 16);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(198, 24);
		this.panel11.TabIndex = 524;
		this.RadAll.AutoSize = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(4, 2);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(39, 19);
		this.RadAll.TabIndex = 521;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadAll.CheckedChanged += new System.EventHandler(RadAll_CheckedChanged);
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Checked = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(45, 2);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(69, 19);
		this.RadNotReffered.TabIndex = 523;
		this.RadNotReffered.TabStop = true;
		this.RadNotReffered.Text = "Pending";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.RadNotReffered.CheckedChanged += new System.EventHandler(RadAll_CheckedChanged);
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(119, 2);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(76, 19);
		this.RadReffered.TabIndex = 522;
		this.RadReffered.Text = "Complete";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(5, 205);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(109, 15);
		this.label58.TabIndex = 520;
		this.label58.Text = "Exporter Lot / Ref #\r\n";
		this.btnDefineJobLot.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnDefineJobLot.Location = new System.Drawing.Point(318, 173);
		this.btnDefineJobLot.Name = "btnDefineJobLot";
		this.btnDefineJobLot.Size = new System.Drawing.Size(29, 26);
		this.btnDefineJobLot.TabIndex = 519;
		this.btnDefineJobLot.TabStop = false;
		this.btnDefineJobLot.UseVisualStyleBackColor = true;
		this.btnDefineJobLot.Click += new System.EventHandler(btnDefineJobLot_Click);
		this.CmbCountryOfInspection.AllowNull = (DefaultableBoolean)1;
		this.CmbCountryOfInspection.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCountryOfInspection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCountryOfInspection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCountryOfInspection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCountryOfInspection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection).Location = new System.Drawing.Point(151, 305);
		((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection).Name = "CmbCountryOfInspection";
		((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbCountryOfInspection).TabIndex = 11;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(5, 311);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(92, 15);
		this.label31.TabIndex = 497;
		this.label31.Text = "Agency Country";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(151, 173);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(134, 311);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(16, 14);
		this.label57.TabIndex = 515;
		this.label57.Text = "*";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(134, 258);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(16, 14);
		this.label51.TabIndex = 511;
		this.label51.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(134, 231);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(16, 14);
		this.label50.TabIndex = 510;
		this.label50.Text = "*";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(84, 241);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 517;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(84, 224);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 516;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(134, 338);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(16, 14);
		this.label56.TabIndex = 514;
		this.label56.Text = "*";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.Red;
		this.label55.Location = new System.Drawing.Point(134, 392);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(16, 14);
		this.label55.TabIndex = 513;
		this.label55.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(134, 284);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(16, 14);
		this.label53.TabIndex = 512;
		this.label53.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(134, 152);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(16, 14);
		this.label49.TabIndex = 509;
		this.label49.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(134, 125);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(16, 14);
		this.label48.TabIndex = 508;
		this.label48.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(134, 73);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(16, 14);
		this.label47.TabIndex = 507;
		this.label47.Text = "*";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(134, 47);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(16, 14);
		this.label25.TabIndex = 506;
		this.label25.Text = "*";
		this.BtnInspectionTeam.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnInspectionTeam.Location = new System.Drawing.Point(318, 119);
		this.BtnInspectionTeam.Name = "BtnInspectionTeam";
		this.BtnInspectionTeam.Size = new System.Drawing.Size(29, 26);
		this.BtnInspectionTeam.TabIndex = 4;
		this.BtnInspectionTeam.TabStop = false;
		this.BtnInspectionTeam.UseVisualStyleBackColor = true;
		this.BtnInspectionTeam.Click += new System.EventHandler(btnExportLookUP_Click);
		this.CmbSamplingResponsibity.AllowNull = (DefaultableBoolean)1;
		this.CmbSamplingResponsibity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSamplingResponsibity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSamplingResponsibity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSamplingResponsibity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSamplingResponsibity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity).Location = new System.Drawing.Point(151, 386);
		((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity).Name = "CmbSamplingResponsibity";
		((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbSamplingResponsibity).TabIndex = 14;
		this.txtHeaderMtons.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHeaderMtons.Location = new System.Drawing.Point(151, 254);
		this.txtHeaderMtons.Name = "txtHeaderMtons";
		this.txtHeaderMtons.Size = new System.Drawing.Size(196, 23);
		this.txtHeaderMtons.TabIndex = 9;
		this.txtHeaderMtons.Text = "0";
		this.txtHeaderMtons.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 258);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 505;
		this.label5.Text = " M.Tons \r\n";
		this.CmbLotTrackingNo.AllowNull = (DefaultableBoolean)1;
		this.CmbLotTrackingNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotTrackingNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotTrackingNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotTrackingNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Location = new System.Drawing.Point(151, 41);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Name = "CmbLotTrackingNo";
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbLotTrackingNo).Leave += new System.EventHandler(CmbLotTrackingNo_Leave);
		this.CmbRequestedBy.AllowNull = (DefaultableBoolean)1;
		this.CmbRequestedBy.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequestedBy.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequestedBy.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance187).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance188).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequestedBy.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Location = new System.Drawing.Point(151, 119);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Name = "CmbRequestedBy";
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).TabIndex = 3;
		this.CmbCountryOfOrigin.AllowNull = (DefaultableBoolean)1;
		this.CmbCountryOfOrigin.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCountryOfOrigin.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCountryOfOrigin.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCountryOfOrigin).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCountryOfOrigin.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin).Location = new System.Drawing.Point(151, 332);
		((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin).Name = "CmbCountryOfOrigin";
		((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbCountryOfOrigin).TabIndex = 12;
		this.CmbInspectionAgency.AllowNull = (DefaultableBoolean)1;
		this.CmbInspectionAgency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInspectionAgency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInspectionAgency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInspectionAgency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInspectionAgency).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInspectionAgency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInspectionAgency).Location = new System.Drawing.Point(151, 278);
		((System.Windows.Forms.Control)(object)this.CmbInspectionAgency).Name = "CmbInspectionAgency";
		((System.Windows.Forms.Control)(object)this.CmbInspectionAgency).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbInspectionAgency).TabIndex = 10;
		this.CmbMedium.AllowNull = (DefaultableBoolean)1;
		this.CmbMedium.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMedium.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMedium.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbMedium).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMedium).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMedium).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMedium).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbMedium).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMedium).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbMedium).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMedium).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMedium).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbMedium).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMedium).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMedium).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMedium).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMedium.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMedium).Location = new System.Drawing.Point(151, 146);
		((System.Windows.Forms.Control)(object)this.CmbMedium).Name = "CmbMedium";
		((System.Windows.Forms.Control)(object)this.CmbMedium).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbMedium).TabIndex = 5;
		this.CmbItemMain.AllowNull = (DefaultableBoolean)1;
		this.CmbItemMain.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemMain.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemMain.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Appearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemMain).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemMain).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance209).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemMain).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemMain).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemMain.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemMain).Location = new System.Drawing.Point(151, 226);
		((System.Windows.Forms.Control)(object)this.CmbItemMain).Name = "CmbItemMain";
		((System.Windows.Forms.Control)(object)this.CmbItemMain).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemMain).TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 232);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 15);
		this.label4.TabIndex = 501;
		this.label4.Text = "Item / Product\r\n";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(5, 338);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(85, 15);
		this.label30.TabIndex = 497;
		this.label30.Text = "Product Origin";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 179);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(46, 15);
		this.label26.TabIndex = 499;
		this.label26.Text = "Job Lot";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(5, 392);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(134, 15);
		this.label29.TabIndex = 497;
		this.label29.Text = "Sampling Responsibility\r\n";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 284);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(106, 15);
		this.label28.TabIndex = 497;
		this.label28.Text = "Inspection Agency";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 47);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(132, 15);
		this.label3.TabIndex = 499;
		this.label3.Text = "Lot Code / Tracking No.\r\n";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 152);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(97, 15);
		this.label2.TabIndex = 497;
		this.label2.Text = "Request Medium";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 125);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 495;
		this.label1.Text = "Requested By";
		this.txtRequestReferenceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequestReferenceNo.Location = new System.Drawing.Point(151, 94);
		this.txtRequestReferenceNo.Name = "txtRequestReferenceNo";
		this.txtRequestReferenceNo.Size = new System.Drawing.Size(196, 23);
		this.txtRequestReferenceNo.TabIndex = 2;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 98);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(123, 15);
		this.label27.TabIndex = 493;
		this.label27.Text = "Request Reference No\r\n";
		this.txtRequestLodgDate.CustomFormat = "dd-MMM-yyyy";
		this.txtRequestLodgDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequestLodgDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtRequestLodgDate.Location = new System.Drawing.Point(151, 69);
		this.txtRequestLodgDate.Name = "txtRequestLodgDate";
		this.txtRequestLodgDate.Size = new System.Drawing.Size(195, 23);
		this.txtRequestLodgDate.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 73);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(113, 15);
		this.label11.TabIndex = 464;
		this.label11.Text = "Request Lodge Date\r\n";
		this.txtExporterLotNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExporterLotNo.Location = new System.Drawing.Point(151, 201);
		this.txtExporterLotNo.Name = "txtExporterLotNo";
		this.txtExporterLotNo.Size = new System.Drawing.Size(195, 23);
		this.txtExporterLotNo.TabIndex = 7;
		this.txtExporterLotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExporterLotNo_KeyPress);
		this.panel28.BackColor = System.Drawing.Color.Teal;
		this.panel28.Controls.Add(this.label66);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(3, 30);
		this.panel28.Margin = new System.Windows.Forms.Padding(2);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1348, 28);
		this.panel28.TabIndex = 561;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.White;
		this.label66.Location = new System.Drawing.Point(3, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(362, 19);
		this.label66.TabIndex = 561;
		this.label66.Text = "Pre Shipment 3rd Party Inspection Request\r\n";
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnAttachment, this.btnPrint, this.BtnPrintStockReserved, this.BtnStockReservedForm, this.BtnTrackingReport, this.btnCountryDefinition,
			this.btnShortCutKey
		});
		this.toolstrip.Location = new System.Drawing.Point(3, 3);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(1348, 27);
		this.toolstrip.TabIndex = 6;
		this.toolstrip.Text = "toolStrip1";
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
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "514-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.BtnPrintStockReserved.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintStockReserved.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintStockReserved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintStockReserved.Name = "BtnPrintStockReserved";
		this.BtnPrintStockReserved.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintStockReserved.Text = "514_01-Print";
		this.BtnPrintStockReserved.Click += new System.EventHandler(BtnPrintStockReserved_Click);
		this.BtnStockReservedForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnStockReservedForm.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnStockReservedForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnStockReservedForm.Name = "BtnStockReservedForm";
		this.BtnStockReservedForm.Size = new System.Drawing.Size(171, 24);
		this.BtnStockReservedForm.Text = "Stock Reserved Form";
		this.BtnStockReservedForm.Click += new System.EventHandler(BtnStockReservedForm_Click);
		this.BtnTrackingReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnTrackingReport.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnTrackingReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnTrackingReport.Name = "BtnTrackingReport";
		this.BtnTrackingReport.Size = new System.Drawing.Size(134, 24);
		this.BtnTrackingReport.Text = "Tracking Report";
		this.BtnTrackingReport.Click += new System.EventHandler(BtnTrackingReport_Click);
		this.btnCountryDefinition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnCountryDefinition.Image = (System.Drawing.Image)resources.GetObject("btnCountryDefinition.Image");
		this.btnCountryDefinition.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCountryDefinition.Name = "btnCountryDefinition";
		this.btnCountryDefinition.Size = new System.Drawing.Size(129, 24);
		this.btnCountryDefinition.Text = "Country Definition";
		this.btnCountryDefinition.Click += new System.EventHandler(btnCityDefinition_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.toolStrip3);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1354, 725);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 131);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1348, 346);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label21);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(3, 477);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1348, 22);
		this.panel7.TabIndex = 564;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(3, 2);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(209, 17);
		this.label21.TabIndex = 561;
		this.label21.Text = "Detail of Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 499);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1348, 223);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel5.Controls.Add(this.RadNotRefferedHistory);
		this.panel5.Controls.Add(this.RadRefferedHistory);
		this.panel5.Controls.Add(this.RadAllHistory);
		this.panel5.Controls.Add(this.groupBox5);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 58);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1348, 73);
		this.panel5.TabIndex = 0;
		this.RadNotRefferedHistory.AutoSize = true;
		this.RadNotRefferedHistory.Checked = true;
		this.RadNotRefferedHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotRefferedHistory.Location = new System.Drawing.Point(811, 29);
		this.RadNotRefferedHistory.Name = "RadNotRefferedHistory";
		this.RadNotRefferedHistory.Size = new System.Drawing.Size(78, 23);
		this.RadNotRefferedHistory.TabIndex = 11;
		this.RadNotRefferedHistory.TabStop = true;
		this.RadNotRefferedHistory.Text = "Pending";
		this.RadNotRefferedHistory.UseVisualStyleBackColor = true;
		this.RadRefferedHistory.AutoSize = true;
		this.RadRefferedHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadRefferedHistory.Location = new System.Drawing.Point(811, 47);
		this.RadRefferedHistory.Name = "RadRefferedHistory";
		this.RadRefferedHistory.Size = new System.Drawing.Size(87, 23);
		this.RadRefferedHistory.TabIndex = 10;
		this.RadRefferedHistory.Text = "Complete";
		this.RadRefferedHistory.UseVisualStyleBackColor = true;
		this.RadAllHistory.AutoSize = true;
		this.RadAllHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAllHistory.Location = new System.Drawing.Point(811, 10);
		this.RadAllHistory.Name = "RadAllHistory";
		this.RadAllHistory.Size = new System.Drawing.Size(44, 23);
		this.RadAllHistory.TabIndex = 9;
		this.RadAllHistory.Text = "All";
		this.RadAllHistory.UseVisualStyleBackColor = true;
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.btnShow);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox5.Controls.Add(this.label52);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add(this.label54);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(4, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(803, 72);
		this.groupBox5.TabIndex = 3;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(695, 49);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(695, 31);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(597, 31);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(95, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Request Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(597, 49);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(528, 43);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 24);
		this.btnShow.TabIndex = 4;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(269, 42);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 3;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(199, 48);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(32, 15);
		this.label52.TabIndex = 14;
		this.label52.Text = "Item";
		this.CmbTrackingNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbTrackingNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTrackingNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTrackingNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance157;
		((SpecialBoxBase)((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTrackingNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTrackingNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory).Location = new System.Drawing.Point(269, 14);
		((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory).Name = "CmbTrackingNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.CmbTrackingNoHistory).TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(199, 20);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(71, 15);
		this.label13.TabIndex = 12;
		this.label13.Text = "Tracking No";
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(68, 44);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(68, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(125, 22);
		this.FromDateHistory.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(6, 48);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(48, 15);
		this.label10.TabIndex = 5;
		this.label10.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 5;
		this.label54.Text = "From Date";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1348, 28);
		this.panel4.TabIndex = 562;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1307, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 563;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(425, 19);
		this.label18.TabIndex = 561;
		this.label18.Text = "Pre Shipment 3rd Party Inspection Request History";
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.BtnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(3, 3);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip3.Size = new System.Drawing.Size(1348, 27);
		this.toolStrip3.TabIndex = 8;
		this.toolStrip3.Text = "toolStrip3";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.txtResultDate.Checked = false;
		this.txtResultDate.CustomFormat = "dd-MMM-yyyy";
		this.txtResultDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtResultDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtResultDate.Location = new System.Drawing.Point(140, 245);
		this.txtResultDate.Name = "txtResultDate";
		this.txtResultDate.ShowCheckBox = true;
		this.txtResultDate.Size = new System.Drawing.Size(185, 23);
		this.txtResultDate.TabIndex = 9;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(6, 249);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(67, 15);
		this.label63.TabIndex = 509;
		this.label63.Text = "Result Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1362, 754);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmThirdPartyInspection";
		this.Text = "EccountBook ERP";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmThirdPartyInspection_FormClosed);
		base.Load += new System.EventHandler(frmThirdPartyInspection_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmThirdPartyInspection_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractInfo).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdLotSampling).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.groupBox4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdParameters).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequiredAnalysisGroup).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleHandOver).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbResultRemarks).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbResultStatus).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingNTrade).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryOfInspection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSamplingResponsibity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotTrackingNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCountryOfOrigin).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInspectionAgency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMedium).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemMain).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTrackingNoHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
