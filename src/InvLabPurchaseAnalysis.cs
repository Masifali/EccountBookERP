// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AForge.Video;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using CameraControl;
using EDSDKLib;
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
namespace Architecture.WinApp.Lab
{

public class InvLabPurchaseAnalysis : Form, IObserver
{
	private CameraController _controller;

	private ActionSource _actionSource = new ActionSource();

	private List<ActionListener> _actionListenerList = new List<ActionListener>();

	private IObserver formAs;

	private Form _remoteCapture;

	private IntPtr[] _volumes = new IntPtr[2];

	private int _volume_count;

	private string DriverPicPath = string.Empty;

	private string DriverPicPath2 = string.Empty;

	private string UniqFileName = string.Empty;

	private string UniqFileName2 = string.Empty;

	private int RefDocumentTypeId;

	private bool RateFromScheduleOnLabForGatePurchase;

	private PendingGPForPurchaseLab LoadGp;

	public UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	public static Image imgpreview = null;

	public static string saveDirectory = "";

	public static string UniqPathAnalysis = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private DataTable dtgpsr = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtgetIdbyDocNo = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private DataTable dtPurchaseAnalysisDetailOriginalData = new DataTable();

	private List<InvLabAnalysisPurchaseDetail> detaillst = new List<InvLabAnalysisPurchaseDetail>();

	private List<InvLabSampleAnalysisDetail> sampledetailist = new List<InvLabSampleAnalysisDetail>();

	private int AnalysisGroupId;

	private IContainer components;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnprint;

	private Button btnimg1;

	private PictureBox LiveViewPicBox;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnNew;

	private ToolStrip btn;

	private ToolStripButton btnattachment;

	private TextBox txtdocno;

	private Label cmpcity;

	private Label label6;

	private Label label5;

	private Label cmbsupplier;

	private Label label10;

	private Label txtsampledate;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private Label txt;

	private Label label8;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel PanalParamGrdsForm;

	private Panel PanelFeildForms;

	private Label label7;

	private DateTimePicker txtdocdate;

	private Label label1;

	private TextBox txtOrderNo;

	private Label label3;

	private Label label2;

	private Button btnimage2;

	private PictureBox cookingpic;

	private Label label4;

	private TextBox txtvehicleno;

	private TextBox txtbiltyno;

	private Label label9;

	private TextBox txtSampleId;

	private Button btnprv;

	private Button btnimgprevcookingpic;

	private TextBox txtWeightCut;

	private Label labeldeduciton;

	private TextBox txtAddRate;

	private TextBox txtAnaylstName;

	private Label label12;

	internal UltraCombo cmbitem;

	internal UltraCombo cmbanalysisgroup;

	internal UltraCombo cmbCropYear;

	private CheckBox IsPrint;

	internal UltraCombo cmbGatePassNo;

	private TextBox txtPurchaseOrderId;

	internal UltraCombo CmbLabStatus;

	private ComboBox comboBox1;

	private Button btnResetDriv;

	private Button TakePhotoButton;

	private Button LiveViewButton;

	private Button btnResetCooking;

	private Button btnCapturecooking;

	private Button btnStartCooking;

	private Button button3;

	private Panel panel3;

	internal GridEX grdSubItems;

	internal UltraCombo CmbSampleAnaylsis;

	private Panel panel5;

	private Panel panel10;

	internal GridEX grdSampleSubItem;

	internal GridEX grdSample;

	private Panel panel9;

	private TextBox txtNetRate;

	private TextBox txtLessRate;

	private TextBox txtItemRate;

	internal UltraCombo CmbRateUom;

	private TextBox txtScheduleId;

	private Label label18;

	private TextBox txtNoofBagsInspection;

	private Panel panel13;

	private Panel panel15;

	private Panel panel19;

	internal GridEX grdHistrorySamAnalysis;

	private Panel panel18;

	private Label label11;

	private Panel panel14;

	private Panel panel17;

	internal GridEX grdHistroryPurAnalysis;

	private Panel panel16;

	private Label label14;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbPackingType;

	private Label label16;

	private Label label17;

	private TextBox PartyLotRefNo;

	internal UltraCombo cmbPartyName;

	private Label label19;

	private Panel panel22;

	private Label label21;

	private TextBox txtremarks;

	internal UltraCombo CmbWarehouse;

	private Label label20;

	private TextBox txtMoisture;

	private Label label22;

	private ToolStripButton btnSlip257;

	private ToolStripButton BtnLoader;

	private Panel panel21;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshHistory;

	private Panel panel20;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label53;

	private Label label56;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label27;

	internal UltraCombo CmbGpNoHistory;

	private Label label26;

	internal UltraCombo CmbVehicleNoHistory;

	private Label label25;

	private Label label24;

	private Label label23;

	internal UltraCombo cmbApprovedStatusHistory;

	internal UltraCombo CmbAnalystStatusHistory;

	internal UltraCombo CmbItemHistory;

	private ToolStripButton btnNewHistory;

	private Panel panel23;

	private Label label28;

	internal UltraCombo CmbWtCuton;

	private Label label29;

	private TextBox txtQtyForWtCut;

	private Label label30;

	private CheckBox chkPrintII;

	private CheckBox ChkRateCutWillApply;

	private CheckBox ChkWtCutWillApply;

	private TextBox txtPurchaseType;

	private TextBox txtCityArea;

	private Label label33;

	private Label label32;

	private Label label31;

	private DateTimePicker datBiltyDate;

	private TextBox txtApprovedBy;

	private Label label34;

	internal UltraCombo CmbJobLot;

	private Label label36;

	private TextBox txtApprovalStatus;

	private Label label35;

	private TextBox txtWeightCutUom;

	private Label label37;

	private Panel panel6;

	private Label label13;

	private Panel panel2;

	private Label PaymentVoucherLable;

	private TextBox txtRateCut;

	private GroupBox groupBoxGP;

	private Label label38;

	private Label label39;

	private Label label40;

	private Label label41;

	private Label label42;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbPestResult;

	private Label label43;

	internal UltraCombo CmbPestStatus;

	private Label label44;

	private Label label45;

	private Label label48;

	internal UltraCombo CmbPestStatusHistory;

	private Label label47;

	internal UltraCombo CmbPestResultHistory;

	private Label label46;

	private Label label49;

	private TextBox txtGpQty;

	public InvLabPurchaseAnalysis(UserAccount pUserAccount)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	public InvLabPurchaseAnalysis(ref CameraController controller, UserAccount pUserAccount)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		formAs = this;
		InitializeComponent();
		UserAccount = pUserAccount;
		_controller = controller;
		if (_controller == null)
		{
			return;
		}
		EDSDK.EdsGetChildCount(_controller.GetModel().Camera, out _volume_count);
		if (_volume_count > 0)
		{
			EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 0, out _volumes[0]);
			EDSDK.EdsGetVolumeInfo(_volumes[0], out var outVolumeInfo);
			_ = outVolumeInfo.StorageType;
			if (_volume_count > 1)
			{
				EDSDK.EdsGetChildAtIndex(_controller.GetModel().Camera, 1, out _volumes[1]);
				EDSDK.EdsGetVolumeInfo(_volumes[1], out outVolumeInfo);
				_ = outVolumeInfo.StorageType;
			}
		}
		_actionListenerList.Add(_controller);
		_actionListenerList.ForEach(delegate(ActionListener actionListener)
		{
			_actionSource.AddActionListener(ref actionListener);
		});
	}

	private void InvLabPurchaseAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 750);
			PanalParamGrdsForm.AutoScroll = true;
			PanalParamGrdsForm.AutoScrollMargin = new Size(20, 20);
			PanalParamGrdsForm.AutoScrollMinSize = new Size(900, 750);
			SomeFieldsDisablesAndEnables(Status: true);
			GetConfiguration();
			dtViewAttachments.Columns.Add("AttchmentPath");
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			cmbGatePassNo.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtGpQty.Enabled = false;
			GpNoFill();
			CropBindFromGlobal();
			PackingTypeBindFromGlobal();
			JobLotBindFromGlobal();
			GenerateCode();
			LabstatusFill();
			WarehouseFill();
			WeightCutOnFill();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			PestResult();
			PestStatus();
			if (((DataTable)((UltraGridBase)CmbPestResult).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbPestResult).Rows[0].Activate();
			}
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
			txtWeightCutUom.Enabled = Conversion.ToInt(CmbWtCuton.Value) == 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("name");
			DataTable dtItem = dtSupp.Clone();
			DataTable dtAnalyst = dtSupp.Clone();
			DataTable dtApprovedStatus = dtSupp.Clone();
			DataTable dtGpNo = dtSupp.Clone();
			DataTable dtVehicle = dtSupp.Clone();
			DataTable dtPestResult = dtSupp.Clone();
			DataTable dtPestStatus = dtSupp.Clone();
			DataTable dt = InvLabAnalysisPurchaseHeader.GetDataForDropDownFromLabPurchaseAnaylsis(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "",
				DocumentTypeIds = ""
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "AnaylstStatus")
					{
						dtAnalyst.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ApprovedStatus")
					{
						dtApprovedStatus.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "GpNo")
					{
						dtGpNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "VehicleNo")
					{
						dtVehicle.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PestResult")
					{
						dtPestResult.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "PestStatus")
					{
						dtPestStatus.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupp, CmbCustomerHistory, "Id", "name", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtAnalyst, CmbAnalystStatusHistory, "Id", "name", "Analyst Status", ZeroIndex: false);
			DDL.BindDDL(dtApprovedStatus, cmbApprovedStatusHistory, "Id", "name", "Approved Status", ZeroIndex: false);
			DDL.BindDDL(dtGpNo, CmbGpNoHistory, "Id", "name", "Gp No", ZeroIndex: false);
			((UltraGridBase)CmbGpNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtVehicle, CmbVehicleNoHistory, "Id", "name", "Vahicle No", ZeroIndex: false);
			((UltraGridBase)CmbVehicleNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtPestResult, CmbPestResultHistory, "Id", "name", "Pest Result", ZeroIndex: false);
			((UltraGridBase)CmbPestResultHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtPestStatus, CmbPestStatusHistory, "Id", "name", "Pest Status", ZeroIndex: false);
			((UltraGridBase)CmbPestStatusHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfiguration()
	{
		try
		{
			RateFromScheduleOnLabForGatePurchase = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("RateFromScheduleOnLabForGatePurchase"));
			saveDirectory = Conversion.ToString((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Attachment Folder Path"));
			txtItemRate.Enabled = !RateFromScheduleOnLabForGatePurchase;
			txtItemRate.ReadOnly = RateFromScheduleOnLabForGatePurchase;
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
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0)
			{
				CmbJobLot.Value = jobId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0 && ((UltraGridBase)cmbCropYear).DataSource != null)
			{
				cmbCropYear.Value = cropYearId;
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0 && ((UltraGridBase)CmbPackingType).DataSource != null)
			{
				CmbPackingType.Value = packingTypeId;
			}
			int wareHouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Warehouse"));
			if (wareHouseId != 0 && ((UltraGridBase)CmbWarehouse).DataSource != null)
			{
				CmbWarehouse.Value = wareHouseId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void WarehouseFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbWarehouse, "Id", "WareHouseName", "Warehouse Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GpNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dtGpNo = GatePassInward.GetGpSrNoForPurchaseAnalysis(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("GpSrNo");
			dt.Columns.Add("VehicleNo");
			dt.Columns.Add("OrderType");
			dt.Columns.Add("SupplierName");
			dt.Columns.Add("LabType");
			if (dtGpNo.Rows.Count > 0)
			{
				for (int i = 0; i < dtGpNo.Rows.Count; i++)
				{
					dt.Rows.Add(dtGpNo.Rows[i]["Id"], dtGpNo.Rows[i]["GpSrNo"], dtGpNo.Rows[i]["VehicleNo"], dtGpNo.Rows[i]["OrderType"], dtGpNo.Rows[i]["SupplierName"], dtGpNo.Rows[i]["LabType"]);
				}
			}
			DDL.BindDDL(dt, cmbGatePassNo, "Id", "GpSrNo", "GpSrNo", ZeroIndex: false);
			((UltraGridBase)cmbGatePassNo).DisplayLayout.Bands[0].Columns[1].Width = 120;
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvLabAnalysisPurchaseHeader.GenerateCode(new InvLabAnalysisPurchaseHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 303,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			if (Conversion.ToInt((object)Code) > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgatepassno_Leave(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			SomeFieldsDisablesAndEnables(Status: true);
			dtgpsr.Rows.Clear();
			RefDocumentTypeId = 0;
			((Control)(object)cmbitem).Text = string.Empty;
			((UltraGridBase)cmbitem).DataSource = null;
			InvLabAnalysisPurchaseHeader obj = new InvLabAnalysisPurchaseHeader();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.GatePassInwardId = Conversion.ToInt(cmbGatePassNo.Value);
			dtgpsr = InvLabAnalysisPurchaseHeader.GetGpDetailByGatepassNo(obj);
			if (dtgpsr.Rows.Count > 0)
			{
				txtOrderNo.Text = dtgpsr.Rows[0]["OrderNo"].ToString();
				txtbiltyno.Text = dtgpsr.Rows[0]["BiltyNo"].ToString();
				datBiltyDate.Value = Conversion.ToDateTime(dtgpsr.Rows[0]["BiltyDate"]);
				txtvehicleno.Text = dtgpsr.Rows[0]["VehicleNo"].ToString();
				txtCityArea.Text = Conversion.ToString(dtgpsr.Rows[0]["CityName"]);
				RefDocumentTypeId = Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]);
				txtPurchaseType.Text = Conversion.ToString(dtgpsr.Rows[0]["OrderType"]);
				txtGpQty.Text = Conversion.ToString(dtgpsr.Rows[0]["GpQty"]);
				label21.Text = "Lab Purchase Analysis (" + Conversion.ToString(dtgpsr.Rows[0]["OrderType"]) + ")";
				if (RefDocumentTypeId != 41)
				{
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id");
					dt2.Columns.Add("PartyName");
					dt2.Rows.Add(dtgpsr.Rows[0]["SupplierCustomerId"], dtgpsr.Rows[0]["PartyName"]);
					DDL.BindDDL(dt2, cmbPartyName, "Id", "PartyName", "Party Name", ZeroIndex: false);
					((UltraGridBase)cmbPartyName).Rows[0].Activate();
				}
				if (RefDocumentTypeId == 41)
				{
					txtPurchaseOrderId.Text = Conversion.ToString(dtgpsr.Rows[0]["PurchaseOrderId"]);
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("ItemName");
					dt.Columns.Add("PoDetailId");
					dt.Columns.Add("SupplierCustomerId");
					dt.Columns.Add("PartyName");
					dt.Columns.Add("ParentcategoryId");
					dt.Columns.Add("Parentcategory");
					for (int i = 0; i < dtgpsr.Rows.Count; i++)
					{
						dt.Rows.Add(dtgpsr.Rows[i]["OrderItemId"], dtgpsr.Rows[i]["ItemName"], dtgpsr.Rows[i]["PoDetailId"], dtgpsr.Rows[i]["SupplierCustomerId"], dtgpsr.Rows[i]["PartyName"], dtgpsr.Rows[i]["ParentcategoryId"], dtgpsr.Rows[i]["Parentcategory"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "ItemName", ZeroIndex: false);
						((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
						((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
						((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[5].Hidden = true;
						((UltraGridBase)cmbitem).Rows[0].Activate();
						DDL.BindDDL(dt, cmbPartyName, "SupplierCustomerId", "PartyName", "PartyName", ZeroIndex: false);
						((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[0].Hidden = true;
						((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[1].Hidden = true;
						((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
						((UltraGridBase)cmbPartyName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
						((UltraGridBase)cmbPartyName).Rows[0].Activate();
					}
					int itemId = Conversion.ToInt(cmbitem.Value);
					int OrderId = Conversion.ToInt((object)txtPurchaseOrderId.Text);
					double Moisture = CommonServices.GetMoistureByItemIdAndPurchaseOrderId(itemId, OrderId);
					txtMoisture.Text = Conversion.ToString((object)Moisture);
				}
				else if (RefDocumentTypeId == 105 || RefDocumentTypeId == 111)
				{
					ItemsBind();
				}
				else if (RefDocumentTypeId == 106 || RefDocumentTypeId == 110 || RefDocumentTypeId == 98 || RefDocumentTypeId == 52 || RefDocumentTypeId == 700 || RefDocumentTypeId == 175)
				{
					_ = RefDocumentTypeId;
					_ = 110;
					if (RateFromScheduleOnLabForGatePurchase)
					{
						ItemsBindFromPricingSchedule();
					}
					else
					{
						ItemsBind();
					}
				}
				if (((UltraGridBase)cmbitem).ActiveRow != null && Conversion.ToInt(cmbitem.Value) > 0)
				{
					AnalysisGroup(Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["ParentcategoryId"].Value));
				}
				txtItemRate.Enabled = RefDocumentTypeId == 106 && !RateFromScheduleOnLabForGatePurchase;
				txtAddRate.Enabled = RefDocumentTypeId == 106;
				txtLessRate.Enabled = RefDocumentTypeId == 106;
				((Control)(object)CmbRateUom).Enabled = RefDocumentTypeId == 106;
				txtRateCut.Enabled = RefDocumentTypeId != 106;
			}
			else
			{
				txtOrderNo.Text = string.Empty;
				txtbiltyno.Text = string.Empty;
				txtvehicleno.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisGroup(int ParentcategoryId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dtLAG = InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ParentCategoryId = ParentcategoryId
			});
			if (dtLAG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("AnalysisGroupDescription", typeof(string));
				dt.Columns.Add("GroupTypeId", typeof(int));
				dt.Columns.Add("GroupType", typeof(string));
				for (int i = 0; i < dtLAG.Rows.Count; i++)
				{
					dt.Rows.Add(dtLAG.Rows[i]["Id"], dtLAG.Rows[i]["AnalysisGroupDescription"], dtLAG.Rows[i]["GroupType"], dtLAG.Rows[i]["InvParentCateDescription"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbanalysisgroup, "Id", "AnalysisGroupDescription", "Analysis Group", ZeroIndex: false);
					((UltraGridBase)cmbanalysisgroup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
			else
			{
				((Control)(object)cmbanalysisgroup).Text = string.Empty;
				((UltraGridBase)cmbanalysisgroup).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemsBind()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetAllServiceBind("1,2,4");
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PoDetailId");
				dt.Columns.Add("ParentcategoryId");
				dt.Columns.Add("Parentcategory");
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"], 0, dtItem.Rows[i]["ParentcategoryId"], dtItem.Rows[i]["Parentcategory"]);
				}
				DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "ItemName", ZeroIndex: false);
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalCropYear, (GlobalCropYear x) => x.Id, (GlobalCropYear x) => x.CropYear);
			InfragisticsHelper.BindAndRetainSelection(cmbCropYear, dt, "Id", "Description", "Crop Year", AllColumns: true, Conversion.ToInt(cmbCropYear.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PackingTypeBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalInvPackingType, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dt, "Id", "Description", "Packing Type", AllColumns: true, Conversion.ToInt(cmbCropYear.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobLotBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalJobLot, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dt, "Id", "Description", "Job Lot", AllColumns: true, Conversion.ToInt(CmbJobLot.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemsBindFromPricingSchedule()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItem = ItemPricingScheduleForRice.GetItemsFromPricingSchedule(new ItemPricingScheduleForRice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PoDetailId");
				dt.Columns.Add("ParentcategoryId");
				dt.Columns.Add("Parentcategory");
				for (int i = 0; i < dtItem.Rows.Count; i++)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"], 0, dtItem.Rows[i]["ParentcategoryId"], dtItem.Rows[i]["Parentcategory"]);
				}
				DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "ItemName", ZeroIndex: false);
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabstatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Accepted");
			dt.Rows.Add(2, "Rejected");
			DDL.BindDDL(dt, CmbLabStatus, "Id", "Status", "Analysis Status", ZeroIndex: false);
			((UltraGridBase)CmbLabStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PestResult()
	{
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.GetPestResult();
			InfragisticsHelper.BindAndRetainSelection(CmbPestResult, dt, "Id", "PestResult", "Pest Result", AllColumns: false, Conversion.ToInt(CmbPestResult.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PestStatus()
	{
		try
		{
			DataTable dt = InvLabAnalysisPurchaseHeader.GetPestStatus();
			InfragisticsHelper.BindAndRetainSelection(CmbPestStatus, dt, "Id", "PestStatus", "Pest Status", AllColumns: false, Conversion.ToInt(CmbPestStatus.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightCutOnFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("LabWeightCutOn");
			DDL.BindDDL(dt, CmbWtCuton, "Id", "type", "CutOn", ZeroIndex: false);
			((UltraGridBase)CmbWtCuton).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbanalysisgroup_Leave(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		try
		{
			if (RecId > 0 && Conversion.ToInt(cmbanalysisgroup.Value) == AnalysisGroupId)
			{
				return;
			}
			detaillst.Clear();
			AnalysisGroupId = 0;
			DataTable dtDetail = InvLabGroupAnalysisStandards.GetParametersFromGroupStandards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbanalysisgroup.Value)
			});
			if (dtDetail.Rows.Count <= 0)
			{
				return;
			}
			List<InvLabAnalysisPurchaseSubParamsDetail> lstsub = new List<InvLabAnalysisPurchaseSubParamsDetail>();
			for (int j = 0; j < dtDetail.Rows.Count; j++)
			{
				if (Conversion.ToInt(dtDetail.Rows[j]["SubParamsId"]) > 0)
				{
					InvLabAnalysisPurchaseSubParamsDetail objsub = new InvLabAnalysisPurchaseSubParamsDetail();
					objsub.InvParentParameterId = Conversion.ToInt(dtDetail.Rows[j]["ParentParameterId"]);
					objsub.SubParameterId = Conversion.ToInt(dtDetail.Rows[j]["SubParamsId"]);
					objsub.SubParameterName = Conversion.ToString(dtDetail.Rows[j]["SubParameterName"]);
					lstsub.Add(objsub);
				}
			}
			int ParamsId = 0;
			for (int i = 0; i < dtDetail.Rows.Count; i++)
			{
				InvLabAnalysisPurchaseDetail lstobj = new InvLabAnalysisPurchaseDetail();
				lstobj.InvLabAnalysisPurchaseSubParamsDetailslist = new List<InvLabAnalysisPurchaseSubParamsDetail>();
				if (ParamsId == Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]))
				{
					continue;
				}
				bool flag = true;
				for (int k = 0; k < detaillst.Count; k++)
				{
					if (Conversion.ToInt((object)detaillst[k].InvLabAnalysisItemsId) == Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]))
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					lstobj.InvLabAnalysisItemsId = Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]);
					lstobj.InvLabGroupAnalysisStandardsId = Conversion.ToInt(dtDetail.Rows[i]["Id"]);
					lstobj.AnalysisGroupDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisGroupDescription"]);
					lstobj.AnalysisParameterDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisParameterDescription"]);
					lstobj.MinValue = Conversion.ToDouble(dtDetail.Rows[i]["MinValue"]);
					lstobj.MaxValue = Conversion.ToDouble(dtDetail.Rows[i]["MaxValue"]);
					lstobj.ParamsMinValue = Conversion.ToDouble(dtDetail.Rows[i]["ParamsMinValue"]);
					lstobj.ParamsMaxValue = Conversion.ToDouble(dtDetail.Rows[i]["ParamsMaxValue"]);
					lstobj.InAnalysisResult = 0.0;
					lstobj.InvLabAnalysisPurchaseSubParamsDetailslist = lstsub.FindAll((InvLabAnalysisPurchaseSubParamsDetail x) => lstobj.InvLabAnalysisItemsId == x.InvParentParameterId);
					detaillst.Add(lstobj);
					ParamsId = Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]);
				}
			}
			grd.DataSource = detaillst;
			grd.RetrieveStructure();
			grdsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)cmbitem).ActiveRow != null && Conversion.ToInt(cmbitem.Value) > 0)
			{
				if (dtgpsr.Rows.Count > 0)
				{
					if (Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]) == 41)
					{
						DataTable dt = new DataTable();
						dt.Columns.Add("Id");
						dt.Columns.Add("SampleNo");
						for (int i = 0; i < dtgpsr.Rows?.Count; i++)
						{
							if (Conversion.ToInt(dtgpsr.Rows[i]["PoDetailId"]) == Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["PoDetailId"].Value))
							{
								((Control)(object)cmbCropYear).Text = Conversion.ToString(dtgpsr.Rows[i]["Crop"]);
								dt.Rows.Add(dtgpsr.Rows[i]["InvLabSampleAnalysisHeaderId"], dtgpsr.Rows[i]["SampleNo"]);
								break;
							}
						}
						if (dt.Rows.Count > 0)
						{
							DDL.BindDDL(dt, CmbSampleAnaylsis, "Id", "SampleNo", "SampleNo", ZeroIndex: false);
						}
						else
						{
							((Control)(object)CmbSampleAnaylsis).Text = string.Empty;
							((UltraGridBase)CmbSampleAnaylsis).DataSource = null;
						}
						int itemId = Conversion.ToInt(cmbitem.Value);
						int OrderId = Conversion.ToInt((object)txtPurchaseOrderId.Text);
						double Moisture = CommonServices.GetMoistureByItemIdAndPurchaseOrderId(itemId, OrderId);
						txtMoisture.Text = Conversion.ToString((object)Moisture);
					}
					else if (Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]) == 106 || Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]) == 110)
					{
						if (RateFromScheduleOnLabForGatePurchase)
						{
							DataTable dtRateUom = ItemPricingScheduleForRice.GetRateUomFromItemPricingSchedule(new ItemPricingScheduleForRice
							{
								OrganizationId = UserAccount.OrganizationId,
								CompanyId = UserAccount.CompanyId,
								ItemId = Conversion.ToInt(cmbitem.Value)
							});
							if (dtRateUom.Rows.Count > 0)
							{
								DDL.BindDDL(dtRateUom, CmbRateUom, "RateUomId", "RateUom", "RateUom", ZeroIndex: false);
								((UltraGridBase)CmbRateUom).Rows[0].Activate();
								CmbRateUom_Leave(null, null);
							}
							else
							{
								txtItemRate.Text = "0";
								txtAddRate.Text = "0";
								txtLessRate.Text = "0";
								txtNetRate.Text = "0";
								((Control)(object)CmbRateUom).Text = string.Empty;
								((UltraGridBase)CmbRateUom).DataSource = null;
							}
						}
						else
						{
							DataTable dtRateUom2 = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbitem.Value));
							if (dtRateUom2.Rows.Count > 0)
							{
								DDL.BindDDL(dtRateUom2, CmbRateUom, "Id", "UOMCode", "RateUom", ZeroIndex: false);
								((UltraGridBase)CmbRateUom).Rows[0].Activate();
								CmbRateUom_Leave(null, null);
							}
							else
							{
								txtItemRate.Text = "0";
								txtAddRate.Text = "0";
								txtLessRate.Text = "0";
								txtNetRate.Text = "0";
								((Control)(object)CmbRateUom).Text = string.Empty;
								((UltraGridBase)CmbRateUom).DataSource = null;
							}
						}
					}
				}
				AnalysisGroup(Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["ParentcategoryId"].Value));
			}
			else
			{
				txtMoisture.Text = "0";
				txtItemRate.Text = "0";
				txtAddRate.Text = "0";
				txtLessRate.Text = "0";
				txtNetRate.Text = "0";
				((Control)(object)CmbRateUom).Text = string.Empty;
				((Control)(object)cmbanalysisgroup).Text = string.Empty;
				((UltraGridBase)CmbRateUom).DataSource = null;
				txtScheduleId.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSampleAnaylsis_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			InvLabSampleAnalysisHeader obj = new InvLabSampleAnalysisHeader();
			obj = InvLabSampleAnalysisHeader.GetByID(Conversion.ToInt(CmbSampleAnaylsis.Value));
			if (obj == null)
			{
				return;
			}
			List<InvLabSampleAnalysiSubParamsDetail> lstDetailSub = new List<InvLabSampleAnalysiSubParamsDetail>();
			for (int i = 0; i < obj.LabSampleDetail.Count; i++)
			{
				for (int j = 0; j < obj.LabSampleDetail[i].InvLabSampleAnalysiSubParamsDetailsList.Count; j++)
				{
					if (obj.LabSampleDetail[i].InvLabSampleAnalysiSubParamsDetailsList[j].ResultValue > 0.0)
					{
						InvLabSampleAnalysiSubParamsDetail objdt = new InvLabSampleAnalysiSubParamsDetail();
						objdt = obj.LabSampleDetail[i].InvLabSampleAnalysiSubParamsDetailsList[j];
						lstDetailSub.Add(objdt);
					}
				}
			}
			sampledetailist = obj.LabSampleDetail;
			grdSample.DataSource = sampledetailist;
			grdSample.RetrieveStructure();
			GrdSamplesetting();
			grdSampleSubItem.DataSource = lstDetailSub;
			grdSampleSubItem.RetrieveStructure();
			GrdSampleSubsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_Leave(object sender, EventArgs e)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && ((UltraGridBase)cmbitem).ActiveRow != null && RateFromScheduleOnLabForGatePurchase)
			{
				if (dtgpsr.Rows.Count > 0 && (Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]) == 106 || Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]) == 110))
				{
					DataTable dt = ItemPricingScheduleForRice.GetItemRateByItemIdAndUomId(new ItemPricingScheduleForRice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = Conversion.ToInt(cmbitem.Value),
						RateUomId = Conversion.ToInt(CmbRateUom.Value),
						EffectiveDate = Conversion.ToDateTime((object)txtdocdate.Value)
					});
					if (dt.Rows.Count > 0)
					{
						txtItemRate.Text = dt.Rows[0]["ItemRate"].ToString();
						txtNetRate.Text = dt.Rows[0]["ItemRate"].ToString();
						txtScheduleId.Text = dt.Rows[0]["Id"].ToString();
						NetRateCalculation();
					}
					else
					{
						txtItemRate.Text = "0";
						txtScheduleId.Text = "0";
					}
				}
			}
			else
			{
				txtScheduleId.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbGatePassNo).ActiveRow == null || Conversion.ToInt(cmbGatePassNo.Value) == 0)
		{
			MessageBox.Show("Please GpNo GatePass No");
			cmbGatePassNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbanalysisgroup).ActiveRow == null || Conversion.ToInt(cmbanalysisgroup.Value) == 0)
		{
			MessageBox.Show("Please Select Analysis Group No");
			cmbanalysisgroup.Focus();
			return false;
		}
		if (((UltraGridBase)cmbitem).ActiveRow == null || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			cmbitem.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoofBagsInspection.Text) == 0)
		{
			MessageBox.Show("NoofBags Field Required");
			txtNoofBagsInspection.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (RefDocumentTypeId == 106 && RateFromScheduleOnLabForGatePurchase)
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUom.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtItemRate.Text) == 0.0)
			{
				MessageBox.Show("ItemRate Field Required");
				txtItemRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtNetRate.Text) == 0.0)
			{
				MessageBox.Show("NetRate Field Required");
				txtNetRate.Focus();
				return false;
			}
		}
		if (Conversion.ToDouble((object)txtItemRate.Text) > 0.0 && Conversion.ToDouble((object)txtNetRate.Text) == 0.0)
		{
			MessageBox.Show("NetRate Field Required");
			txtNetRate.Focus();
			return false;
		}
		if (txtAnaylstName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("AnaylstName field required");
			txtAnaylstName.Focus();
			return false;
		}
		int weightcut = Conversion.ToInt((object)txtWeightCut.Text);
		if (weightcut != 0 && weightcut > 10)
		{
			MessageBox.Show("WeightCut / Touch Cannot Be Greater Than 10");
			txtWeightCut.Focus();
			return false;
		}
		int weightcutUom = Conversion.ToInt((object)txtWeightCutUom.Text);
		if (weightcutUom > 0 && (weightcutUom < 40 || weightcutUom > 105))
		{
			MessageBox.Show("WeightCutUom Must Be Between 40 and 105");
			txtWeightCutUom.Focus();
			return false;
		}
		decimal gpQty = Conversion.ToDecimal((object)txtGpQty.Text);
		decimal qtyForWeightCut = Conversion.ToDecimal((object)txtQtyForWtCut.Text);
		if (qtyForWeightCut > 0m && qtyForWeightCut > gpQty)
		{
			MessageBox.Show("Qty For Weight Cut / Touch Cannot Be Greater Than GatePass Qty");
			txtQtyForWtCut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null)
		{
			MessageBox.Show("Warehouse field required");
			CmbWarehouse.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtWeightCut.Text) > 0 && (((UltraGridBase)CmbWtCuton).ActiveRow == null || Conversion.ToInt(CmbWtCuton.Value) == 0))
		{
			MessageBox.Show("Weight Cut On field required");
			CmbWtCuton.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLabStatus).ActiveRow == null)
		{
			MessageBox.Show("Status field required");
			CmbLabStatus.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPestResult).ActiveRow == null)
		{
			MessageBox.Show("Pest Result field required");
			CmbPestResult.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPestStatus).ActiveRow == null)
		{
			MessageBox.Show("Pest Status field required");
			CmbPestStatus.Focus();
			return false;
		}
		return true;
	}

	private void SomeFieldsDisablesAndEnables(bool Status)
	{
	}

	private void GrdSamplesetting()
	{
		try
		{
			grdSample.AllowEdit = (InheritableBoolean)2;
			grdSample.AllowDelete = (InheritableBoolean)2;
			grdSample.RootTable.Columns["Id"].Visible = false;
			grdSample.RootTable.Columns["InvLabAnalysisItemsId"].Visible = false;
			grdSample.RootTable.Columns["InvLabSampleAnalysisHeaderId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdSample, 3, 0, null, null, null, ColumnSetting: false, BracketsForamt: false, null, ExcludeAllColumnsFromTotals: true);
			grdSample.RootTable.Columns["AnalysisGroupDescription"].Width = 110;
			grdSample.RootTable.Columns["AnalysisParameterDescription"].Width = 150;
			grdSample.RootTable.Columns["MinValue"].Width = 55;
			grdSample.RootTable.Columns["MaxValue"].Width = 55;
			grdSample.RootTable.Columns["ResultValue"].Width = 55;
			grdSample.RootTable.Columns["RemarksDetail"].Width = 200;
			grdSample.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdSample.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
			grdSample.RootTable.Columns["ResultValue"].FormatString = "#,##0.###";
			grdSample.RootTable.Columns["ResultValue"].TotalFormatString = "#,##0.###";
			grdSample.RootTable.Columns["MinValue"].AggregateFunction = (AggregateFunction)2;
			grdSample.RootTable.Columns["MinValue"].TextAlignment = (TextAlignment)3;
			grdSample.RootTable.Columns["MinValue"].FormatString = "#,##0.###";
			grdSample.RootTable.Columns["MinValue"].TotalFormatString = "#,##0.###";
			grdSample.RootTable.Columns["MaxValue"].AggregateFunction = (AggregateFunction)2;
			grdSample.RootTable.Columns["MaxValue"].TextAlignment = (TextAlignment)3;
			grdSample.RootTable.Columns["MaxValue"].FormatString = "#,##0.###";
			grdSample.RootTable.Columns["MaxValue"].TotalFormatString = "#,##0.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "InvLabGroupAnalysisStandardsId", "InvLabAnalysisPurchaseHeaderId", "InvLabAnalysisItemsId", "ParamsMinValue", "ParamsMaxValue" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"IsEditableAfterApproval",
					(EditType)8
				},
				{
					"InAnalysisResult",
					(EditType)1
				},
				{
					"RemarksDetail",
					(EditType)1
				}
			};
			List<string> NotTotalColumns = new List<string> { "ParamsMinValue", "ParamsMaxValue" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grd.RootTable.Columns["AnalysisParameterDescription"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["AnalysisGroupDescription"].Caption = "Analysis Group";
			grd.RootTable.Columns["AnalysisParameterDescription"].Caption = "Analysis Parameter";
			grd.RootTable.Columns["InAnalysisResult"].Caption = "Analysis Result";
			grd.RootTable.Columns["IsEditableAfterApproval"].Caption = "Editable After Approval";
			grd.RootTable.Groups.Add("AnalysisGroupDescription");
			grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grd.RootTable.Columns["AnalysisParameterDescription"].Width = Constants.LabConstants.AnalysisParameter + 20;
			grd.RootTable.Columns["MinValue"].Width = Constants.LabConstants.MinValue;
			grd.RootTable.Columns["MaxValue"].Width = Constants.LabConstants.MaxValue;
			grd.RootTable.Columns["InAnalysisResult"].Width = Constants.LabConstants.ResultValue;
			grd.RootTable.Columns["IsEditableAfterApproval"].Width = 70;
			grd.RootTable.Columns["RemarksDetail"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSubsetting()
	{
		try
		{
			grdSubItems.AllowDelete = (InheritableBoolean)2;
			grdSubItems.ColumnAutoResize = true;
			grdSubItems.RootTable.Columns["Id"].Visible = false;
			grdSubItems.RootTable.Columns["InvParentParameterId"].Visible = false;
			grdSubItems.RootTable.Columns["SubParameterId"].Visible = false;
			grdSubItems.RootTable.Columns["InvLabAnalysisPurchaseDetailId"].Visible = false;
			grdSubItems.RootTable.Columns["InvLabAnalysisPurchaseHeaderId"].Visible = false;
			grdSubItems.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdSubItems.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdSubItems.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdSubItems.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSampleSubsetting()
	{
		try
		{
			grdSampleSubItem.AllowDelete = (InheritableBoolean)2;
			grdSampleSubItem.AllowEdit = (InheritableBoolean)2;
			grdSampleSubItem.ColumnAutoResize = true;
			grdSampleSubItem.RootTable.Columns["Id"].Visible = false;
			grdSampleSubItem.RootTable.Columns["InvParentParameterId"].Visible = false;
			grdSampleSubItem.RootTable.Columns["SubParameterId"].Visible = false;
			grdSampleSubItem.RootTable.Columns["InvLabSampleAnalysisDetailId"].Visible = false;
			grdSampleSubItem.RootTable.Columns["InvLabSampleAnalysisHeaderId"].Visible = false;
			grdSampleSubItem.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdSampleSubItem.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdSampleSubItem.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdSampleSubItem.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
			grdSampleSubItem.RootTable.Groups.Add("ParentParameterName");
			grdSampleSubItem.RootTable.Columns["ParentParameterName"].Visible = false;
			grdSampleSubItem.RootTable.GroupTotals = (GroupTotals)2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
	}

	private void grdSample_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdSample.CurrentRow;
			grdSampleSubItem.DataSource = sampledetailist[item.RowIndex].InvLabSampleAnalysiSubParamsDetailsList;
			grdSampleSubItem.RetrieveStructure();
			GrdSampleSubsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			grdSubItems.DataSource = detaillst[item.RowIndex].InvLabAnalysisPurchaseSubParamsDetailslist;
			grdSubItems.RetrieveStructure();
			GrdSubsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXColumn maxCol = grd.RootTable.Columns["MaxValue"];
			GridEXColumn minCol = grd.RootTable.Columns["MinValue"];
			GridEXColumn resultCol = grd.RootTable.Columns["InAnalysisResult"];
			GridEXColumn ParameterId = grd.RootTable.Columns["InvLabAnalysisItemsId"];
			if (maxCol == null || minCol == null || resultCol == null)
			{
				return;
			}
			GridEXCell maxCell = e.Row.Cells[((JanusCollectionElement)maxCol).Index];
			_ = e.Row.Cells[((JanusCollectionElement)minCol).Index];
			GridEXCell ResultCell = e.Row.Cells[((JanusCollectionElement)resultCol).Index];
			GridEXCell ParamCell = e.Row.Cells[((JanusCollectionElement)ParameterId).Index];
			double SampleAnalysisParamValue = 0.0;
			if (grdSample.RowCount > 0)
			{
				double PurchaseAnalysisParamValue = Conversion.ToDouble(ParamCell.Value);
				GridEXRow[] rows = grdSample.GetRows();
				int num = 0;
				if (num < rows.Length)
				{
					GridEXRow obj = rows[num];
					double value = Conversion.ToDouble(obj.Cells["ResultValue"].Value);
					if ((double)Conversion.ToInt(obj.Cells["InvLabAnalysisItemsId"].Value) == PurchaseAnalysisParamValue && value > 0.0)
					{
						SampleAnalysisParamValue = value;
					}
				}
			}
			if (Conversion.ToDouble(ResultCell.Value) > 0.0 && Conversion.ToDouble(ResultCell.Value) > Conversion.ToDouble(maxCell.Value))
			{
				ResultCell.FormatStyle = new GridEXFormatStyle();
				ResultCell.FormatStyle.ForeColor = Color.Red;
				ResultCell.FormatStyle.FontBold = (TriState)2;
				if (SampleAnalysisParamValue > 0.0 && Conversion.ToDouble(ResultCell.Value) > 0.0 && Conversion.ToDouble(ResultCell.Value) > SampleAnalysisParamValue)
				{
					GridEXFormatStyle style = new GridEXFormatStyle();
					style.ForeColor = Color.Red;
					e.Row.RowStyle = style;
				}
			}
			else if (SampleAnalysisParamValue > 0.0 && Conversion.ToDouble(ResultCell.Value) > 0.0 && Conversion.ToDouble(ResultCell.Value) > SampleAnalysisParamValue)
			{
				GridEXFormatStyle style2 = new GridEXFormatStyle();
				style2.ForeColor = Color.Red;
				e.Row.RowStyle = style2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NotEditTableFields()
	{
		try
		{
			grdHistroryPurAnalysis.RootTable.Columns["Id"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisGroup"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["MinValue"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["MaxValue"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["IsEditableAfterApproval"].EditType = (EditType)0;
			grdHistroryPurAnalysis.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisGroup"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["MinValue"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["MaxValue"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdHistroryPurAnalysis.RootTable.Columns["IsEditableAfterApproval"].FilterEditType = (FilterEditType)1;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			InvLabAnalysisPurchaseHeader labheader = new InvLabAnalysisPurchaseHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				labheader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double Result = 0.0;
			for (int i = 0; i < detaillst.Count; i++)
			{
				double TotalValueParent = detaillst[i].InAnalysisResult;
				double ParamsMinValue = detaillst[i].ParamsMinValue;
				double ParamsMaxValue = detaillst[i].ParamsMaxValue;
				if (TotalValueParent > 0.0)
				{
					if (TotalValueParent > ParamsMaxValue)
					{
						throw new Exception("Analysis Result is not Greater than Maximum Value " + detaillst[i].AnalysisParameterDescription + ": " + Conversion.ToString((object)detaillst[i].ParamsMaxValue));
					}
					if (TotalValueParent < ParamsMinValue)
					{
						throw new Exception("Analysis Result is not Less than Minimum Value " + detaillst[i].AnalysisParameterDescription + ": " + Conversion.ToString((object)detaillst[i].ParamsMinValue));
					}
				}
				Result += TotalValueParent;
				double TotalValueSub = 0.0;
				for (int j = 0; j < detaillst[i].InvLabAnalysisPurchaseSubParamsDetailslist?.Count; j++)
				{
					TotalValueSub += detaillst[i].InvLabAnalysisPurchaseSubParamsDetailslist[j].ResultValue;
				}
				if (TotalValueSub > 0.0 && TotalValueParent != TotalValueSub && detaillst[i].InvLabAnalysisPurchaseSubParamsDetailslist.Count > 0)
				{
					throw new Exception("Total Value of all sub parameters for " + detaillst[i].AnalysisParameterDescription + " must be equal to value of " + detaillst[i].AnalysisParameterDescription);
				}
			}
			if (Result == 0.0)
			{
				throw new Exception("ResultValue cannot be equal to zero");
			}
			labheader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			labheader.DocumentTypeId = 303;
			labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			labheader.SupplierCustomerId = Conversion.ToInt(cmbPartyName.Value);
			labheader.ApprovedDate = DateTime.Now;
			labheader.GatePassInwardId = Conversion.ToInt(cmbGatePassNo.Value);
			labheader.PurchaseOrderId = Conversion.ToInt((object)txtPurchaseOrderId.Text);
			labheader.SupCustCode = Conversion.ToString((object)txtOrderNo.Text);
			labheader.InvLabSampleLogRegisterId = Conversion.ToInt(CmbSampleAnaylsis.Value);
			if (((Control)(object)CmbLabStatus).Text == "Accepted")
			{
				labheader.IsAccepted = true;
			}
			else
			{
				labheader.IsAccepted = false;
			}
			labheader.ItemId = Conversion.ToInt(cmbitem.Value);
			labheader.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			labheader.PoDetailId = Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["PoDetailId"].Value);
			labheader.Crop = Conversion.ToString((object)((Control)(object)cmbCropYear).Text);
			labheader.InvLabAnalysisGroup = Conversion.ToInt(cmbanalysisgroup.Value);
			labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			labheader.Pic1Pathe = Conversion.ToString((object)UniqFileName);
			labheader.CookingPic = Conversion.ToString((object)UniqFileName2);
			labheader.Premium = Conversion.ToDouble((object)txtAddRate.Text);
			labheader.DeductionRate = ((RefDocumentTypeId == 106) ? Conversion.ToDouble((object)txtLessRate.Text) : Conversion.ToDouble((object)txtRateCut.Text));
			labheader.NoofBagsInspection = Conversion.ToDouble((object)txtNoofBagsInspection.Text);
			labheader.ItemRate = Conversion.ToDouble((object)txtItemRate.Text);
			labheader.PricingScheduleId = Conversion.ToInt((object)txtScheduleId.Text);
			labheader.RateUomId = Conversion.ToInt(CmbRateUom.Value);
			labheader.PackingTypeId = Conversion.ToInt(CmbPackingType.Value);
			labheader.PartyLotRefNo = Conversion.ToString((object)PartyLotRefNo.Text);
			labheader.DeductionWeight = Conversion.ToDouble((object)txtWeightCut.Text);
			labheader.QtyForWtCut = Conversion.ToDouble((object)txtQtyForWtCut.Text);
			labheader.WeightCutOnId = Conversion.ToInt(CmbWtCuton.Value);
			labheader.WeightCutUom = Conversion.ToDouble((object)txtWeightCutUom.Text);
			labheader.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			labheader.PestResultId = Conversion.ToInt(CmbPestResult.Value);
			labheader.PestStatusId = Conversion.ToInt(CmbPestStatus.Value);
			labheader.AnalystName = txtAnaylstName.Text;
			labheader.CompanyId = UserAccount.CompanyId;
			labheader.OrganizationId = UserAccount.OrganizationId;
			labheader.BranchesId = UserAccount.BranchesId;
			labheader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			labheader.EntryDate = DateTime.Now;
			labheader.EntryUser = UserAccount.ID;
			labheader.ModifyDate = DateTime.Now;
			labheader.ModifyUser = UserAccount.ID;
			labheader.ApprovedDate = DateTime.Now;
			labheader.IsApproved = false;
			labheader.LabSampleDetail = detaillst;
			if (labheader.DeductionWeight == 0.0)
			{
				labheader.IsWeightCutCompulsory = ChkWtCutWillApply.Checked;
			}
			if (labheader.DeductionRate == 0.0)
			{
				labheader.IsRateCutCompulsory = ChkRateCutWillApply.Checked;
			}
			labheader.dmsAttachment = new List<DMSAttachments>();
			labheader.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvLabAnalysisPurchaseHeader>(labheader, base.Name, AT, "dmsAttachment", "DeleteAttachmentsList");
			if (ofd != null && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				LiveViewPicBox.Image = null;
				LiveViewPicBox.Invalidate();
			}
			if (ofdcookingpic != null && ofdcookingpic != string.Empty)
			{
				File.Copy(ofdcookingpic, fileSavePathcookingpic, overwrite: true);
				ofdcookingpic = string.Empty;
				fileSavePathcookingpic = string.Empty;
				cookingpic.Image = null;
				cookingpic.Invalidate();
			}
			int success = InvLabAnalysisPurchaseHeader.Save(labheader);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 303, base.Name);
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			int GPId = Conversion.ToInt(cmbGatePassNo.Value);
			refresh();
			if (IsPrint.Checked)
			{
				LabAnalysisReport653(success);
			}
			if (chkPrintII.Checked)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(GPId);
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

	private void ReadById(int ID)
	{
		try
		{
			refresh();
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)cmbGatePassNo).Enabled = false;
			new DataTable();
			RecId = ID;
			InvLabAnalysisPurchaseHeader obj = InvLabAnalysisPurchaseHeader.ReadById(ID);
			if (obj == null)
			{
				return;
			}
			detaillst.Clear();
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = Conversion.ToString((object)obj.DocNo);
			txtdocdate.Value = Conversion.ToDateTime((object)obj.DocDate);
			txtbiltyno.Text = Conversion.ToString((object)obj.BiltyNo);
			datBiltyDate.Value = Conversion.ToDateTime((object)obj.BiltyDate);
			txtCityArea.Text = Conversion.ToString((object)obj.CityName);
			txtPurchaseType.Text = Conversion.ToString((object)obj.PurchaseType);
			txtApprovedBy.Text = Conversion.ToString((object)obj.UserNameAusr);
			txtApprovalStatus.Text = Conversion.ToString((object)obj.ApprovalStatus);
			txtvehicleno.Text = Conversion.ToString((object)obj.VehicleNo);
			txtPurchaseOrderId.Text = Conversion.ToString((object)obj.PurchaseOrderId);
			txtOrderNo.Text = Conversion.ToString((object)obj.SupCustCode);
			txtNoofBagsInspection.Text = Conversion.ToString((object)obj.NoofBagsInspection);
			RefDocumentTypeId = Conversion.ToInt((object)obj.RefDocumentTypeId);
			CmbWtCuton.Value = Conversion.ToInt((object)obj.WeightCutOnId);
			txtWeightCutUom.Text = Conversion.ToString((object)obj.WeightCutUom);
			ChkRateCutWillApply.Checked = obj.IsRateCutCompulsory;
			ChkWtCutWillApply.Checked = obj.IsWeightCutCompulsory;
			ChkRateCutWillApply.Enabled = false;
			ChkWtCutWillApply.Enabled = false;
			DataTable dtgp = new DataTable();
			dtgp.Columns.Add("Id");
			dtgp.Columns.Add("GpSrNo");
			dtgp.Rows.Add(Conversion.ToInt((object)obj.GatePassInwardId), Conversion.ToString((object)obj.GpSrNo));
			DDL.BindDDL(dtgp, cmbGatePassNo, "Id", "GpSrNo", "GpSrNo", ZeroIndex: false);
			((UltraGridBase)cmbGatePassNo).Rows[0].Activate();
			txtgatepassno_Leave(null, null);
			PartyLotRefNo.Text = obj.PartyLotRefNo;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("PoDetailId");
			dt.Columns.Add("SupplierCustomerId");
			dt.Columns.Add("PartyName");
			dt.Columns.Add("ParentcategoryId");
			dt.Columns.Add("Parentcategory");
			dt.Rows.Add(obj.ItemId, obj.ItemName, obj.PoDetailId, obj.SupplierCustomerId, obj.PartyName, obj.ParentcategoryId, obj.Parentcategory);
			_ = dt.Rows.Count;
			_ = 0;
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbPartyName).Text = string.Empty;
			cmbitem.Value = obj.ItemId;
			cmbPartyName.Value = obj.SupplierCustomerId;
			if (obj.RateUomId > 0)
			{
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("Id");
				dtUom.Columns.Add("RateUom");
				dtUom.Rows.Add(Conversion.ToInt((object)obj.RateUomId), Conversion.ToString((object)obj.RateUom));
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "RateUom", "RateUom", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).Rows[0].Activate();
			}
			if (RefDocumentTypeId == 106)
			{
				cmbitem_Leave(null, null);
				if (obj.RateUomId > 0)
				{
					CmbRateUom.Value = obj.RateUomId;
				}
			}
			else
			{
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
			DataTable dtsam = new DataTable();
			dtsam.Columns.Add("Id");
			dtsam.Columns.Add("SampleNo");
			dtsam.Rows.Add(obj.InvLabSampleLogRegisterId, obj.LabSampleNo);
			if (dtsam.Rows.Count > 0)
			{
				DDL.BindDDL(dtsam, CmbSampleAnaylsis, "Id", "SampleNo", "ItemName", ZeroIndex: false);
				((UltraGridBase)CmbSampleAnaylsis).Rows[0].Activate();
				CmbSampleAnaylsis_Leave(null, null);
			}
			if (obj.IsAccepted)
			{
				((Control)(object)CmbLabStatus).Text = "Accepted";
			}
			else
			{
				((Control)(object)CmbLabStatus).Text = "Rejected";
			}
			if (((UltraGridBase)cmbitem).ActiveRow != null && Conversion.ToInt(cmbitem.Value) > 0)
			{
				AnalysisGroup(Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["ParentcategoryId"].Value));
			}
			((Control)(object)cmbCropYear).Text = Conversion.ToString((object)obj.Crop);
			CmbWarehouse.Value = obj.WarehouseId;
			CmbPackingType.Value = Conversion.ToInt((object)obj.PackingTypeId);
			if (obj.JobLotId > 0)
			{
				CmbJobLot.Value = Conversion.ToInt((object)obj.JobLotId);
			}
			if (obj.PestResultId > 0)
			{
				CmbPestResult.Value = Conversion.ToInt((object)obj.PestResultId);
			}
			if (obj.PestStatusId > 0)
			{
				CmbPestStatus.Value = Conversion.ToInt((object)obj.PestStatusId);
			}
			cmbanalysisgroup.Value = Conversion.ToInt((object)obj.InvLabAnalysisGroup);
			AnalysisGroupId = obj.InvLabAnalysisGroup;
			txtremarks.Text = Conversion.ToString((object)obj.RemarksHeader);
			if (RefDocumentTypeId == 106)
			{
				txtAddRate.Text = Conversion.ToString((object)obj.Premium);
				txtLessRate.Text = Conversion.ToString((object)obj.DeductionRate);
				txtItemRate.Text = Conversion.ToString((object)obj.ItemRate);
			}
			txtRateCut.Text = Conversion.ToString((object)obj.DeductionRate);
			txtWeightCut.Text = Conversion.ToString((object)obj.DeductionWeight);
			txtQtyForWtCut.Text = Conversion.ToString((object)obj.QtyForWtCut);
			txtAnaylstName.Text = Conversion.ToString((object)obj.AnalystName);
			txtScheduleId.Text = Conversion.ToString((object)obj.PricingScheduleId);
			NetRateCalculation();
			detaillst = obj.LabSampleDetail;
			grd.DataSource = detaillst;
			grd.RetrieveStructure();
			grdsetting();
			string fileSavePath = Conversion.ToString((object)obj.Pic1Pathe);
			string fileSavePathcookingpic = Conversion.ToString((object)obj.CookingPic);
			if (fileSavePath != "")
			{
				UniqFileName = fileSavePath;
				string DrvPicPath2 = saveDirectory + "\\" + fileSavePath;
				if (File.Exists(DrvPicPath2))
				{
					Image image2 = Image.FromFile(DrvPicPath2);
					LiveViewPicBox.Image = image2;
				}
				else
				{
					fileSavePath = "";
					LiveViewPicBox.Image = null;
					LiveViewPicBox.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				LiveViewPicBox.Image = null;
				LiveViewPicBox.Invalidate();
			}
			if (fileSavePathcookingpic != "")
			{
				UniqFileName2 = fileSavePathcookingpic;
				string DrvPicPath = saveDirectory + "\\" + fileSavePathcookingpic;
				if (File.Exists(DrvPicPath))
				{
					Image image = Image.FromFile(DrvPicPath);
					cookingpic.Image = image;
				}
				else
				{
					fileSavePathcookingpic = "";
					cookingpic.Image = null;
					cookingpic.Invalidate();
				}
			}
			else
			{
				fileSavePathcookingpic = "";
				cookingpic.Image = null;
				cookingpic.Invalidate();
			}
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			SomeFieldsDisablesAndEnables(Status: true);
			AnalysisGroupId = 0;
			RecId = 0;
			label21.Text = "Lab Purchase Analysis";
			GenerateCode();
			GpNoFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)cmbGatePassNo).Text = string.Empty;
			PartyLotRefNo.Text = string.Empty;
			txtbiltyno.Text = "";
			datBiltyDate.Value = DateTime.Now;
			txtCityArea.Text = string.Empty;
			txtPurchaseType.Text = string.Empty;
			txtApprovedBy.Text = string.Empty;
			txtApprovalStatus.Text = string.Empty;
			txtGpQty.Text = string.Empty;
			txtvehicleno.Text = "";
			txtOrderNo.Text = "";
			((Control)(object)CmbSampleAnaylsis).Text = "";
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmbanalysisgroup).Text = "";
			txtWeightCut.Text = "";
			txtAddRate.Text = "";
			txtLessRate.Text = "";
			txtRateCut.Text = "";
			txtItemRate.Text = "";
			txtNetRate.Text = "";
			txtQtyForWtCut.Text = "";
			txtPurchaseOrderId.Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtNoofBagsInspection.Text = string.Empty;
			cookingpic.Image = null;
			cookingpic.Invalidate();
			LiveViewPicBox.Image = null;
			LiveViewPicBox.Invalidate();
			cmbGatePassNo.Focus();
			((Control)(object)cmbGatePassNo).Enabled = true;
			UniqFileName = string.Empty;
			UniqFileName2 = string.Empty;
			DriverPicPath = string.Empty;
			DriverPicPath2 = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtScheduleId.Text = string.Empty;
			((UltraGridBase)CmbRateUom).DataSource = null;
			detaillst.Clear();
			dtgpsr.Rows.Clear();
			grd.ClearStructure();
			grdSample.ClearStructure();
			grdSubItems.ClearStructure();
			grdSampleSubItem.ClearStructure();
			ChkRateCutWillApply.Enabled = true;
			ChkWtCutWillApply.Enabled = true;
			GetConfiguration();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			if (((DataTable)((UltraGridBase)CmbPestResult).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)CmbPestResult).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaveImage()
	{
		UniqFileName2 = $"{Guid.NewGuid()}.Bmp";
		DriverPicPath += UniqFileName2;
		if (File.Exists(DriverPicPath))
		{
			LiveViewPicBox.Image = Image.FromFile(DriverPicPath);
		}
		LiveViewPicBox.Image.Save(DriverPicPath, ImageFormat.Bmp);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			FromDateHistory.Focus();
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			ToDateHistory.Text = string.Empty;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			((Control)(object)CmbAnalystStatusHistory).Text = string.Empty;
			((Control)(object)cmbApprovedStatusHistory).Text = string.Empty;
			((Control)(object)CmbGpNoHistory).Text = string.Empty;
			((Control)(object)CmbVehicleNoHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdHistrorySamAnalysis.ClearStructure();
			grdHistroryPurAnalysis.ClearStructure();
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

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			historygridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "WeightCutBefore", "RateCutBefore", "AnalysisPicValue", "CookingPicValue" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"RateCut",
					(EditType)1
				},
				{
					"WeightCut",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3, 3, hiddenColumns, editableColumns);
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["AnalysisPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["CookingPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["AnalysisPic"].Width = 60;
			grdhistory.RootTable.Columns["CookingPic"].Width = 60;
			grdhistory.RootTable.Columns["LabType"].Width = 80;
			grdhistory.RootTable.Columns["DocNo"].Width = 60;
			grdhistory.RootTable.Columns["AnalysisItem"].Width = 250;
			grdhistory.RootTable.Columns["GatePass#"].Width = 80;
			grdhistory.RootTable.Columns["SupplierCode"].Width = 90;
			grdhistory.RootTable.Columns["PartyName"].Width = 220;
			grdhistory.RootTable.Columns["BiltyNo"].Width = 70;
			grdhistory.RootTable.Columns["VehicleNo"].Width = 100;
			grdhistory.RootTable.Columns["CropYear"].Width = 80;
			grdhistory.RootTable.Columns["Remarks"].Width = 300;
			grdhistory.RootTable.Columns["AnalystName"].Width = 200;
			grdhistory.RootTable.Columns["PestResult"].Width = Constants.InventoryConstants.StringConstant;
			grdhistory.RootTable.Columns["PestStatus"].Width = Constants.InventoryConstants.StringConstant;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 50, 1);
			GridEX_Helper.AddButton(grdhistory, "Update", "Update", 60, 2);
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void historygridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters cat = new ReportsParameters();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.BranchesId = UserAccount.BranchesId;
			cat.DocumentTypeId = 303;
			cat.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			cat.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!cat.CanViewAllRecord)
			{
				cat.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				cat.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				cat.ToDate = ToDateHistory.Value;
			}
			cat.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			cat.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			cat.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			cat.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			cat.GpId = Conversion.ToInt(CmbGpNoHistory.Value);
			cat.ApprovedFilter = ((Control)(object)cmbApprovedStatusHistory).Text;
			cat.AnalystStatus = ((Control)(object)CmbAnalystStatusHistory).Text;
			cat.VehicleNo = ((Control)(object)CmbVehicleNoHistory).Text;
			DataTable dtgetAll = InvLabAnalysisPurchaseHeader.GetAll(cat);
			if (dtgetAll.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found");
				grdhistory.ClearStructure();
				grdHistroryPurAnalysis.ClearStructure();
				grdHistrorySamAnalysis.ClearStructure();
				return;
			}
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("LabType", typeof(string));
			table.Columns.Add("DocNo", typeof(int));
			table.Columns.Add("DocDate", typeof(DateTime));
			table.Columns.Add("GatePass#", typeof(int));
			table.Columns.Add("SupplierCode");
			table.Columns.Add("PartyName");
			table.Columns.Add("BiltyNo");
			table.Columns.Add("VehicleNo");
			table.Columns.Add("CropYear");
			table.Columns.Add("WarehouseName");
			table.Columns.Add("AnalysisItem");
			table.Columns.Add("WeightCutOn");
			table.Columns.Add("WeightCutCompulsory", typeof(bool));
			table.Columns.Add("WeightCut");
			table.Columns.Add("RateCutCompulsory", typeof(bool));
			table.Columns.Add("RateCut");
			table.Columns.Add("AnalystName");
			table.Columns.Add("AnalystStatus");
			table.Columns.Add("ApprovedStatus");
			table.Columns.Add("PestResult");
			table.Columns.Add("PestStatus");
			table.Columns.Add("EntryUser");
			table.Columns.Add("EntryDate", typeof(DateTime));
			table.Columns.Add("ApprovedUser");
			table.Columns.Add("ApprovedDate", typeof(DateTime));
			table.Columns.Add("AnalysisPic", typeof(int));
			table.Columns.Add("CookingPic", typeof(int));
			table.Columns.Add("Attachments", typeof(int));
			table.Columns.Add("Remarks");
			table.Columns.Add("WeightCutBefore");
			table.Columns.Add("RateCutBefore");
			table.Columns.Add("AnalysisPicValue");
			table.Columns.Add("CookingPicValue");
			table.Clear();
			for (int i = 0; i < dtgetAll.Rows.Count; i++)
			{
				table.Rows.Add(dtgetAll.Rows[i]["Id"], dtgetAll.Rows[i]["LabType"], dtgetAll.Rows[i]["DocNo"], Conversion.ToDateTime(dtgetAll.Rows[i]["DocDate"]).ToShortDateString(), dtgetAll.Rows[i]["GpSrNo"], dtgetAll.Rows[i]["SupCustCode"], dtgetAll.Rows[i]["PartyName"], dtgetAll.Rows[i]["BiltyNo"], dtgetAll.Rows[i]["VehicleNo"], dtgetAll.Rows[i]["Crop"], dtgetAll.Rows[i]["WareHouseName"], dtgetAll.Rows[i]["ItemName"], dtgetAll.Rows[i]["WeightCutOn"], dtgetAll.Rows[i]["IsWeightCutCompulsory"], dtgetAll.Rows[i]["DeductionWeight"], dtgetAll.Rows[i]["IsRateCutCompulsory"], dtgetAll.Rows[i]["DeductionRate"], dtgetAll.Rows[i]["AnalystName"], dtgetAll.Rows[i]["AnalystStatus"], dtgetAll.Rows[i]["ApprovedStatus"], dtgetAll.Rows[i]["PestResult"], dtgetAll.Rows[i]["PestStatus"], dtgetAll.Rows[i]["UserName"], dtgetAll.Rows[i]["EntryDate"], dtgetAll.Rows[i]["ApprovedUser"], dtgetAll.Rows[i]["ApprovedDate"], (!string.IsNullOrEmpty(Conversion.ToString(dtgetAll.Rows[i]["Pic1Pathe"]))) ? 1 : 0, (!string.IsNullOrEmpty(Conversion.ToString(dtgetAll.Rows[i]["CookingPic"]))) ? 1 : 0, dtgetAll.Rows[i]["NoOfAttachments"], dtgetAll.Rows[i]["RemarksHeader"], dtgetAll.Rows[i]["DeductionWeight"], dtgetAll.Rows[i]["DeductionRate"], dtgetAll.Rows[i]["Pic1Pathe"], dtgetAll.Rows[i]["CookingPic"]);
			}
			grdhistory.DataSource = table;
			grdhistory.RetrieveStructure();
			gridsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdhistory_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count == 0)
			{
				return;
			}
			string columnKey = e.Column.Key;
			if (columnKey != "WeightCut" && columnKey != "RateCut")
			{
				return;
			}
			Conversion.ToInt(row.Cells["Id"].Value);
			if (columnKey == "WeightCut")
			{
				bool num = Conversion.ToBool(row.Cells["WeightCutCompulsory"].Value);
				double weightCut = Conversion.ToDouble(row.Cells["WeightCut"].Value);
				double weightCutBefore = Conversion.ToDouble(row.Cells["WeightCutBefore"].Value);
				if (!num)
				{
					MessageBox.Show("WeightCut can't be changed when WeightCut is not Compulsory .");
					row.Cells["WeightCut"].Value = weightCutBefore;
					return;
				}
				if (weightCut < 0.0)
				{
					MessageBox.Show("WeightCut cannot be less than zero.");
					row.Cells["WeightCut"].Value = 0;
					return;
				}
				row.Cells["WeightCutBefore"].Value = weightCut;
			}
			if (columnKey == "RateCut")
			{
				bool num2 = Conversion.ToBool(row.Cells["RateCutCompulsory"].Value);
				double rateCut = Conversion.ToDouble(row.Cells["RateCut"].Value);
				double rateCutBefore = Conversion.ToDouble(row.Cells["RateCutBefore"].Value);
				if (!num2)
				{
					MessageBox.Show("Rate Cut can't be changed when RateCut is not Compulsory .");
					row.Cells["RateCut"].Value = rateCutBefore;
					return;
				}
				if (rateCut < 0.0)
				{
					MessageBox.Show("Rate Cut cannot be less than zero.");
					row.Cells["RateCut"].Value = 0;
					return;
				}
				row.Cells["RateCutBefore"].Value = rateCut;
			}
			grdhistory.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[0].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(id, base.Name);
				}
				if (e.Column.Key == "AnalysisPic")
				{
					string AnalysisPic = Conversion.ToString(item.Cells["AnalysisPicValue"].Value);
					FormHelper.ShowLabImagesPreview(Path.Combine(clsGlobalVariables.AttachmentFolderPath, AnalysisPic));
				}
				if (e.Column.Key == "CookingPic")
				{
					string CookingPic = Conversion.ToString(item.Cells["CookingPicValue"].Value);
					FormHelper.ShowLabImagesPreview(Path.Combine(clsGlobalVariables.AttachmentFolderPath, CookingPic));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count == 0)
			{
				return;
			}
			string key = e.Column.Key;
			int Id = Conversion.ToInt(row.Cells["Id"].Value);
			if (key == "Edit")
			{
				ReadById(Id);
			}
			if (key == "Print")
			{
				LabAnalysisReport653(Id);
			}
			if (!(key == "Update"))
			{
				return;
			}
			double weightCut = Conversion.ToDouble(row.Cells["WeightCut"].Value);
			double rateCut = Conversion.ToDouble(row.Cells["RateCut"].Value);
			bool num = Conversion.ToBool(row.Cells["WeightCutCompulsory"].Value);
			double weightCutBefore = Conversion.ToDouble(row.Cells["WeightCutBefore"].Value);
			if (!num)
			{
				MessageBox.Show("Weight Cut can't be changed when it is not compulsory.");
				row.Cells["WeightCut"].Value = weightCutBefore;
				return;
			}
			bool num2 = Conversion.ToBool(row.Cells["RateCutCompulsory"].Value);
			double rateCutBefore = Conversion.ToDouble(row.Cells["RateCutBefore"].Value);
			if (!num2)
			{
				MessageBox.Show("Rate Cut can't be changed when it is not compulsory.");
				row.Cells["RateCut"].Value = rateCutBefore;
			}
			else
			{
				InvLabAnalysisPurchaseHeader.LabAnalysisPurchase_CutsUpdatationById(Id, UserAccount.ID, rateCut, weightCut);
				MessageBox.Show("Data updated successfully.");
				historygridfill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int ID = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			int DocNo = Conversion.ToInt(grdhistory.CurrentRow.Cells["DocNo"].Value);
			label14.Text = "Purchase Analysis";
			Label label = label14;
			label.Text = label.Text + " (" + DocNo + ")";
			if (grdHistroryPurAnalysis.GetDataRows().Count() > 0)
			{
				DataTable dt = (DataTable)grdHistroryPurAnalysis.DataSource;
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<bool>("IsEditableAfterApproval")))
				{
					EnumerableRowCollection<DataRow> rows = from row in dt.AsEnumerable()
						where row.Field<bool>("IsEditableAfterApproval")
						select row;
					if (!rows.Any())
					{
						dt.Clone();
					}
					else
					{
						rows.CopyToDataTable();
					}
					ReportsParameters update = new ReportsParameters();
					update.ReportsParameterslist = new List<ReportsParameters>();
					List<DataRow> SaveList = dt.AsEnumerable().Where(delegate(DataRow originalRow)
					{
						if (!originalRow.Field<bool>("IsEditableAfterApproval"))
						{
							return false;
						}
						DataRow dataRow = dtPurchaseAnalysisDetailOriginalData.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == originalRow.Field<int>("Id"));
						return dataRow != null && originalRow.Field<double?>("ResultValue") != dataRow.Field<double?>("ResultValue");
					}).ToList();
					for (int k = 0; k < SaveList?.Count; k++)
					{
						ReportsParameters d = new ReportsParameters();
						d.Id = Conversion.ToInt(SaveList[k]["Id"]);
						d.ResultValue = Conversion.ToDouble(SaveList[k]["ResultValue"]);
						d.Remarks = Conversion.ToString(SaveList[k]["Remarks"]);
						update.ReportsParameterslist.Add(d);
					}
					if (update.ReportsParameterslist != null && update.ReportsParameterslist.Count > 0)
					{
						InvLabAnalysisPurchaseHeader.UpdatePurchaseAnalysisParameterResultValue(update);
						MessageBox.Show("Data Updated Successfully");
					}
				}
			}
			InvLabAnalysisPurchaseHeader obj = InvLabAnalysisPurchaseHeader.ReadById(ID);
			DataTable dtPurHistory = new DataTable();
			dtPurHistory.Columns.Add("Id", typeof(int));
			dtPurHistory.Columns.Add("AnalysisGroup");
			dtPurHistory.Columns.Add("AnalysisParameter");
			dtPurHistory.Columns.Add("MinValue");
			dtPurHistory.Columns.Add("MaxValue");
			dtPurHistory.Columns.Add("ResultValue", typeof(double));
			dtPurHistory.Columns.Add("Remarks");
			dtPurHistory.Columns.Add("ParamsMinValue", typeof(double));
			dtPurHistory.Columns.Add("ParamsMaxValue", typeof(double));
			dtPurHistory.Columns.Add("IsEditableAfterApproval", typeof(bool));
			dtPurHistory.Rows.Clear();
			if (obj.LabSampleDetail != null && obj.LabSampleDetail.Count > 0)
			{
				for (int j = 0; j < obj.LabSampleDetail.Count; j++)
				{
					dtPurHistory.Rows.Add(obj.LabSampleDetail[j].Id, obj.LabSampleDetail[j].AnalysisGroupDescription, obj.LabSampleDetail[j].AnalysisParameterDescription, obj.LabSampleDetail[j].MinValue, obj.LabSampleDetail[j].MaxValue, obj.LabSampleDetail[j].InAnalysisResult, obj.LabSampleDetail[j].RemarksDetail, obj.LabSampleDetail[j].ParamsMinValue, obj.LabSampleDetail[j].ParamsMaxValue, obj.LabSampleDetail[j].IsEditableAfterApproval);
				}
			}
			grdHistroryPurAnalysis.DataSource = dtPurHistory;
			grdHistroryPurAnalysis.RetrieveStructure();
			GrdHistoryDetailPurSetting();
			dtPurchaseAnalysisDetailOriginalData = dtPurHistory.Copy();
			InvLabSampleAnalysisHeader objSam = new InvLabSampleAnalysisHeader();
			objSam = InvLabSampleAnalysisHeader.GetByID(Conversion.ToInt((object)obj.InvLabSampleLogRegisterId));
			DataTable dtSamHistory = new DataTable();
			dtSamHistory.Columns.Add("Id");
			dtSamHistory.Columns.Add("AnalysisGroup");
			dtSamHistory.Columns.Add("AnalysisParameter");
			dtSamHistory.Columns.Add("MinValue");
			dtSamHistory.Columns.Add("MaxValue");
			dtSamHistory.Columns.Add("ResultValue", typeof(double));
			dtSamHistory.Rows.Clear();
			grdHistrorySamAnalysis.ClearStructure();
			if (objSam != null)
			{
				for (int i = 0; i < objSam.LabSampleDetail.Count; i++)
				{
					dtSamHistory.Rows.Add(objSam.LabSampleDetail[i].Id, objSam.LabSampleDetail[i].AnalysisGroupDescription, objSam.LabSampleDetail[i].AnalysisParameterDescription, objSam.LabSampleDetail[i].MinValue, objSam.LabSampleDetail[i].MaxValue, objSam.LabSampleDetail[i].ResultValue);
				}
				grdHistrorySamAnalysis.DataSource = dtSamHistory;
				grdHistrorySamAnalysis.RetrieveStructure();
				GrdHistorySampSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryDetailPurSetting()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistroryPurAnalysis.RootTable.Columns["Id"].Visible = false;
			grdHistroryPurAnalysis.RootTable.Columns["IsEditableAfterApproval"].Visible = false;
			grdHistroryPurAnalysis.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistroryPurAnalysis.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistroryPurAnalysis.RootTable.Groups.Add("AnalysisGroup");
			grdHistroryPurAnalysis.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"].Width = Constants.LabConstants.AnalysisParameter;
			grdHistroryPurAnalysis.RootTable.Columns["MinValue"].Width = Constants.LabConstants.MinValue;
			grdHistroryPurAnalysis.RootTable.Columns["MaxValue"].Width = Constants.LabConstants.MaxValue;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["ParamsMinValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["ParamsMaxValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["Remarks"].Width = 185;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistroryPurAnalysis_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		try
		{
			if (grdHistroryPurAnalysis.RowCount <= 0)
			{
				return;
			}
			GridEXColumn Editable = grdHistroryPurAnalysis.RootTable.Columns["IsEditableAfterApproval"];
			GridEXColumn Parameter = grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"];
			if (Editable != null && Parameter != null)
			{
				GridEXCell obj = e.Row.Cells[((JanusCollectionElement)Editable).Index];
				GridEXCell ParamCell = e.Row.Cells[((JanusCollectionElement)Parameter).Index];
				if (Conversion.ToBool(obj.Value))
				{
					ParamCell.FormatStyle = new GridEXFormatStyle();
					ParamCell.FormatStyle.ForeColor = Color.DarkGreen;
					ParamCell.FormatStyle.FontBold = (TriState)2;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistroryPurAnalysis_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistroryPurAnalysis.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (!Conversion.ToBool(r.Cells["IsEditableAfterApproval"].Value))
				{
					grdHistroryPurAnalysis.AllowEdit = (InheritableBoolean)2;
					return;
				}
				grdHistroryPurAnalysis.AllowEdit = (InheritableBoolean)1;
				NotEditTableFields();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistorySampSetting()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistrorySamAnalysis.RootTable.Columns["Id"].Visible = false;
			grdHistrorySamAnalysis.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistrorySamAnalysis.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistrorySamAnalysis.RootTable.Groups.Add("AnalysisGroup");
			grdHistrorySamAnalysis.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdHistrorySamAnalysis.RootTable.Columns["AnalysisParameter"].Width = Constants.LabConstants.AnalysisParameter;
			grdHistrorySamAnalysis.RootTable.Columns["MinValue"].Width = Constants.LabConstants.MinValue;
			grdHistrorySamAnalysis.RootTable.Columns["MaxValue"].Width = Constants.LabConstants.MaxValue;
			grdHistrorySamAnalysis.RootTable.Columns["ResultValue"].Width = Constants.LabConstants.ResultValue;
			grdHistrorySamAnalysis.RootTable.Columns["Remarks"].Width = 185;
			grdHistrorySamAnalysis.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdHistrorySamAnalysis.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdHistrorySamAnalysis.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdHistrorySamAnalysis.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TakePhotoButton_Click(object sender, EventArgs e)
	{
		try
		{
			UniqFileName = string.Empty;
			if (saveDirectory != string.Empty)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					UniqFileName = $"{Guid.NewGuid()}.Jpg";
					ofd = openFileDialog.FileName;
					fileSavePath = Path.Combine(saveDirectory, UniqFileName);
					LiveViewPicBox.Image = new Bitmap(openFileDialog.FileName);
				}
				return;
			}
			throw new Exception("Attachment FilePath Not Configure Please Check!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCapturecooking_Click(object sender, EventArgs e)
	{
		try
		{
			UniqFileName2 = string.Empty;
			if (saveDirectory != string.Empty)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					UniqFileName2 = $"{Guid.NewGuid()}.Jpg";
					ofdcookingpic = openFileDialog.FileName;
					fileSavePathcookingpic = Path.Combine(saveDirectory, UniqFileName2);
					cookingpic.Image = new Bitmap(openFileDialog.FileName);
				}
				return;
			}
			throw new Exception("Attachment FilePath Not Configure Please Check!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprv_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = LiveViewPicBox.Image;
		labImagesPreview.Show();
	}

	private void btnimgprevcookingpic_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = cookingpic.Image;
		labImagesPreview.Show();
	}

	private void btnDriverImage_Click(object sender, EventArgs e)
	{
	}

	private void videosource_NewFrame(object sender, NewFrameEventArgs eventArgs)
	{
		LiveViewPicBox.Image = (Bitmap)eventArgs.Frame.Clone();
		cookingpic.Image = (Bitmap)eventArgs.Frame.Clone();
	}

	private void btnPreview_Click(object sender, EventArgs e)
	{
	}

	private void btnResetDriv_Click(object sender, EventArgs e)
	{
		try
		{
			LiveViewPicBox.Image = null;
			LiveViewPicBox.Invalidate();
			UniqFileName = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStartCooking_Click(object sender, EventArgs e)
	{
		try
		{
			UniqPathAnalysis = "";
			if (_controller == null)
			{
				throw new Exception("Camera not connected,Checked the camera and reload the page");
			}
			_remoteCapture = new RemoteCapture(ref _controller, ref _actionSource);
			_remoteCapture.ShowDialog(this);
			_remoteCapture.Dispose();
			if (saveDirectory != string.Empty)
			{
				UniqFileName2 = UniqPathAnalysis;
				string DrvPicPath = saveDirectory + UniqFileName2;
				if (File.Exists(DrvPicPath))
				{
					Image image = Image.FromFile(DrvPicPath);
					cookingpic.Image = image;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetCooking_Click(object sender, EventArgs e)
	{
		try
		{
			cookingpic.Image = null;
			cookingpic.Invalidate();
			UniqFileName2 = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LiveViewButton_Click(object sender, EventArgs e)
	{
		try
		{
			UniqPathAnalysis = "";
			if (_controller == null)
			{
				throw new Exception("Camera not connected,Checked the camera and reload the page");
			}
			_remoteCapture = new RemoteCapture(ref _controller, ref _actionSource);
			_remoteCapture.ShowDialog(this);
			_remoteCapture.Dispose();
			if (saveDirectory != string.Empty)
			{
				UniqFileName = UniqPathAnalysis;
				string DrvPicPath = saveDirectory + UniqFileName;
				if (File.Exists(DrvPicPath))
				{
					Image image = Image.FromFile(DrvPicPath);
					LiveViewPicBox.Image = image;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Update(Observable from, CameraEvent e)
	{
		switch (e.GetEventType())
		{
		case CameraEvent.Type.SHUT_DOWN:
			MessageBox.Show("Camera is disconnected");
			if (_remoteCapture != null && !_remoteCapture.IsDisposed)
			{
				_remoteCapture.Close();
				_remoteCapture.Dispose();
			}
			Close();
			break;
		case CameraEvent.Type.PROPERTY_CHANGED:
			if (_remoteCapture != null)
			{
				int num = (int)e.GetArg();
				if (num == 1294)
				{
					(_remoteCapture as RemoteCapture).controlFocusButton(2 != _controller.GetModel().EvfAFMode);
				}
				if (num == 16778274)
				{
					(_remoteCapture as RemoteCapture).updateFixedMovie(_controller.GetModel().FixedMovie);
				}
				if (num == 16778273)
				{
					(_remoteCapture as RemoteCapture).updateMirrorLockUpState(_controller.GetModel().MirrorLockUpState);
				}
			}
			break;
		case CameraEvent.Type.ANGLEINFO:
			if (_remoteCapture != null)
			{
				IntPtr cameraPosPtr = e.GetArg();
				if (cameraPosPtr != IntPtr.Zero)
				{
					EDSDK.EdsCameraPos obj = (EDSDK.EdsCameraPos)Marshal.PtrToStructure(cameraPosPtr, typeof(EDSDK.EdsCameraPos));
					double pos = obj.position;
					double roll = (double)obj.rolling * 0.01;
					double pitc = (double)obj.pitching * 0.01;
					(_remoteCapture as RemoteCapture).updateAngleInfoLabel(pos.ToString(), roll.ToString(), pitc.ToString());
				}
				else
				{
					EDSDK.EdsSendCommand(_controller.GetModel().Camera, 265u, 1);
					(_remoteCapture as RemoteCapture).updateAngleInfoLabel("-", "-", "-");
				}
			}
			break;
		case CameraEvent.Type.MOUSE_CURSOR:
			if (_remoteCapture == null)
			{
				break;
			}
			if (e.GetArg() != IntPtr.Zero)
			{
				(_remoteCapture as RemoteCapture).changeMouseCursor(onoff: true);
				break;
			}
			(_remoteCapture as RemoteCapture).changeMouseCursor(onoff: false);
			if (_controller.ClickType == 1)
			{
				_actionSource.FireEvent(ActionEvent.Command.CLICK_MOUSE_WB, IntPtr.Zero);
			}
			if (_controller.ClickType == 2)
			{
				_actionSource.FireEvent(ActionEvent.Command.CLICK_MOUSE_AF, IntPtr.Zero);
			}
			break;
		}
	}

	private void InvLabPurchaseAnalysis_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (_controller != null)
		{
			_actionSource.FireEvent(ActionEvent.Command.CLOSING, IntPtr.Zero);
			_actionListenerList.ForEach(delegate(ActionListener actionListener)
			{
				_actionSource.RemoveActionListener(ref actionListener);
			});
		}
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button3_Click(object sender, EventArgs e)
	{
		if (_controller != null)
		{
			IObserver frmSetting = new CameraSetting(ref _controller, ref _volumes, ref _volume_count);
			EDSDK.EdsSendStatusCommand(_controller.GetModel().Camera, 0u, 1);
			_controller.GetModel().Add(ref frmSetting);
			((CameraSetting)frmSetting).ShowSettingDialog();
			_controller.GetModel().Remove(ref frmSetting);
			EDSDK.EdsSendStatusCommand(_controller.GetModel().Camera, 1u, 1);
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
			LabAnalysisReport653(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabAnalysisReport653(int RecId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = RecId;
			DataTable dt = InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport(obj.Id), "LabPurchaseAnalysisSubParameterReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "653-RptInvLabPurchaseAnalysisSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip257_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(cmbGatePassNo.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			LoadGp = new PendingGPForPurchaseLab(UserAccount);
			LoadGp.ShowDialog();
			LoadGp.BringToFront();
			if (LoadGp.GpId > 0)
			{
				cmbGatePassNo.Value = LoadGp.GpId;
				txtgatepassno_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddRate_TextChanged(object sender, EventArgs e)
	{
		NetRateCalculation();
	}

	private void NetRateCalculation()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemRate.Text.Trim());
			double AddRate = Conversion.ToDouble((object)txtAddRate.Text.Trim());
			double LessRate = Conversion.ToDouble((object)txtLessRate.Text.Trim());
			txtNetRate.Text = (num + AddRate - LessRate).ToString("#,#");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtLessRate_TextChanged(object sender, EventArgs e)
	{
		NetRateCalculation();
	}

	private void CmbWtCuton_Leave(object sender, EventArgs e)
	{
		txtWeightCutUom.Enabled = Conversion.ToInt(CmbWtCuton.Value) == 2;
		txtWeightCutUom.Text = ((Conversion.ToInt(CmbWtCuton.Value) == 2) ? txtWeightCutUom.Text : "0");
		if (Conversion.ToInt(CmbWtCuton.Value) == 2 && base.ActiveControl != null && base.ActiveControl.Name == "txtWeightCutUom")
		{
			txtWeightCutUom.Focus();
		}
	}

	private void txtAddRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLessRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoofBagsInspection_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyForWtCut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtWeightCutUom_KeyPress(object sender, KeyPressEventArgs e)
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
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabPurchaseAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
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
				btnShowHistory_Click(null, null);
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
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
				txtdocdate.Focus();
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
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (btnprint.Visible && btnprint.Enabled && e.Control && e.KeyCode == Keys.P)
		{
			btnprint_Click(null, null);
		}
	}

	private void grdHistroryPurAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "UomSchedule", "WarehousesWithBranches", "AllItems", "AllCities", "CropYear", "JobLot", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GpNoFill();
			CropBindFromGlobal();
			PackingTypeBindFromGlobal();
			JobLotBindFromGlobal();
			GenerateCode();
			LabstatusFill();
			WarehouseFill();
			WeightCutOnFill();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPestResult_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPestResult.Value) > 0)
			{
				if (Conversion.ToInt(CmbPestResult.Value) == 1)
				{
					CmbPestStatus.Value = 2;
				}
				else
				{
					CmbPestStatus.Value = 1;
				}
			}
			else
			{
				((Control)(object)CmbPestStatus).Text = string.Empty;
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
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0507: Expected O, but got Unknown
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
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
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
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
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0930: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Expected O, but got Unknown
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b4: Expected O, but got Unknown
		//IL_09c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Expected O, but got Unknown
		//IL_09ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Expected O, but got Unknown
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af3: Expected O, but got Unknown
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Expected O, but got Unknown
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c27: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Expected O, but got Unknown
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d87: Expected O, but got Unknown
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9d: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Expected O, but got Unknown
		//IL_0dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Expected O, but got Unknown
		//IL_1b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b85: Expected O, but got Unknown
		//IL_1b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9c: Expected O, but got Unknown
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb3: Expected O, but got Unknown
		//IL_21ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b8: Expected O, but got Unknown
		//IL_251b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2525: Expected O, but got Unknown
		//IL_2532: Unknown result type (might be due to invalid IL or missing references)
		//IL_253c: Expected O, but got Unknown
		//IL_2560: Unknown result type (might be due to invalid IL or missing references)
		//IL_256a: Expected O, but got Unknown
		//IL_5bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bc1: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabPurchaseAnalysis));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistrorySamAnalysis_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistroryPurAnalysis_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
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
		GridEXLayout grdSampleSubItem_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSample_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSubItems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
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
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
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
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmpcity = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbsupplier = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtsampledate = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdHistrorySamAnalysis = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdHistroryPurAnalysis = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label48 = new System.Windows.Forms.Label();
		this.CmbPestStatusHistory = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.CmbPestResultHistory = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbGpNoHistory = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbVehicleNoHistory = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbApprovedStatusHistory = new UltraCombo();
		this.CmbAnalystStatusHistory = new UltraCombo();
		this.CmbItemHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txt = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.PanalParamGrdsForm = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdSampleSubItem = new GridEX();
		this.grdSample = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdSubItems = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.PanelFeildForms = new System.Windows.Forms.Panel();
		this.CmbPestStatus = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.CmbPestResult = new UltraCombo();
		this.label43 = new System.Windows.Forms.Label();
		this.txtRateCut = new System.Windows.Forms.TextBox();
		this.groupBoxGP = new System.Windows.Forms.GroupBox();
		this.txtLessRate = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.txtNetRate = new System.Windows.Forms.TextBox();
		this.txtAddRate = new System.Windows.Forms.TextBox();
		this.PartyLotRefNo = new System.Windows.Forms.TextBox();
		this.CmbPackingType = new UltraCombo();
		this.CmbSampleAnaylsis = new UltraCombo();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.datBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtWeightCutUom = new System.Windows.Forms.TextBox();
		this.cmbitem = new UltraCombo();
		this.txtQtyForWtCut = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbWarehouse = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.cmbPartyName = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtScheduleId = new System.Windows.Forms.TextBox();
		this.txtApprovalStatus = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.button3 = new System.Windows.Forms.Button();
		this.btnResetCooking = new System.Windows.Forms.Button();
		this.btnCapturecooking = new System.Windows.Forms.Button();
		this.btnStartCooking = new System.Windows.Forms.Button();
		this.btnResetDriv = new System.Windows.Forms.Button();
		this.TakePhotoButton = new System.Windows.Forms.Button();
		this.LiveViewButton = new System.Windows.Forms.Button();
		this.CmbLabStatus = new UltraCombo();
		this.txtPurchaseOrderId = new System.Windows.Forms.TextBox();
		this.cmbGatePassNo = new UltraCombo();
		this.ChkRateCutWillApply = new System.Windows.Forms.CheckBox();
		this.ChkWtCutWillApply = new System.Windows.Forms.CheckBox();
		this.cmbCropYear = new UltraCombo();
		this.CmbWtCuton = new UltraCombo();
		this.cmbanalysisgroup = new UltraCombo();
		this.txtWeightCut = new System.Windows.Forms.TextBox();
		this.labeldeduciton = new System.Windows.Forms.Label();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.btnimgprevcookingpic = new System.Windows.Forms.Button();
		this.btnprv = new System.Windows.Forms.Button();
		this.txtSampleId = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtNoofBagsInspection = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.txtPurchaseType = new System.Windows.Forms.TextBox();
		this.txtCityArea = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.btnimage2 = new System.Windows.Forms.Button();
		this.cookingpic = new System.Windows.Forms.PictureBox();
		this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
		this.txtOrderNo = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.label21 = new System.Windows.Forms.Label();
		this.IsPrint = new System.Windows.Forms.CheckBox();
		this.chkPrintII = new System.Windows.Forms.CheckBox();
		this.txtGpQty = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistrorySamAnalysis).BeginInit();
		this.panel18.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).BeginInit();
		this.panel16.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel20.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPestStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPestResultHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.PanalParamGrdsForm.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSampleSubItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdSample).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSubItems).BeginInit();
		this.panel2.SuspendLayout();
		this.PanelFeildForms.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPestStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPestResult).BeginInit();
		this.groupBoxGP.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleAnaylsis).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePassNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWtCuton).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).BeginInit();
		this.panel22.SuspendLayout();
		base.SuspendLayout();
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnimg1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.Location = new System.Drawing.Point(1313, 359);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(77, 27);
		this.btnimg1.TabIndex = 16;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = true;
		this.btnimg1.Visible = false;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnSlip257, this.btnprint, this.btnattachment, this.BtnLoader, this.toolStripSeparator });
		this.btn.Location = new System.Drawing.Point(0, 0);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1320, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
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
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnSlip257.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSlip257.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip257.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip257.Name = "btnSlip257";
		this.btnSlip257.Size = new System.Drawing.Size(102, 24);
		this.btnSlip257.Text = "257-GPSlip";
		this.btnSlip257.Click += new System.EventHandler(btnSlip257_Click);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "653-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(55, 24);
		this.BtnLoader.Text = "&Loader";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(107, 4);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(100, 23);
		this.txtdocno.TabIndex = 0;
		this.cmpcity.AutoSize = true;
		this.cmpcity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmpcity.Location = new System.Drawing.Point(7, 113);
		this.cmpcity.Name = "cmpcity";
		this.cmpcity.Size = new System.Drawing.Size(56, 15);
		this.cmpcity.TabIndex = 0;
		this.cmpcity.Text = "Order No";
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(7, 299);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(105, 30);
		this.label6.TabIndex = 13;
		this.label6.Text = "Status Accepted / Rejected";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(209, 113);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "Sample #";
		this.cmbsupplier.AutoSize = true;
		this.cmbsupplier.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplier.Location = new System.Drawing.Point(7, 35);
		this.cmbsupplier.Name = "cmbsupplier";
		this.cmbsupplier.Size = new System.Drawing.Size(67, 15);
		this.cmbsupplier.TabIndex = 0;
		this.cmbsupplier.Text = "Gate Pass #";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 225);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Analysis Group";
		this.txtsampledate.AutoSize = true;
		this.txtsampledate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsampledate.Location = new System.Drawing.Point(209, 8);
		this.txtsampledate.Name = "txtsampledate";
		this.txtsampledate.Size = new System.Drawing.Size(57, 15);
		this.txtsampledate.TabIndex = 0;
		this.txtsampledate.Text = "Doc Date";
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(500, 463);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1326, 685);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 448);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1320, 234);
		this.panel13.TabIndex = 2;
		this.panel15.Controls.Add(this.panel19);
		this.panel15.Controls.Add(this.panel18);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel15.Location = new System.Drawing.Point(563, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(548, 234);
		this.panel15.TabIndex = 1;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 32);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(548, 202);
		this.panel19.TabIndex = 1;
		this.grdHistrorySamAnalysis.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistrorySamAnalysis).BackColor = System.Drawing.Color.White;
		grdHistrorySamAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistrorySamAnalysis.DesignTimeLayout = grdHistrorySamAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistrorySamAnalysis.FilterMode = (FilterMode)2;
		this.grdHistrorySamAnalysis.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistrorySamAnalysis.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).Name = "grdHistrorySamAnalysis";
		((ScrollableBase)this.grdHistrorySamAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).Size = new System.Drawing.Size(548, 202);
		((System.Windows.Forms.Control)(object)this.grdHistrorySamAnalysis).TabIndex = 4;
		this.grdHistrorySamAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdHistrorySamAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistrorySamAnalysis).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label11);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(548, 32);
		this.panel18.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(6, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(148, 24);
		this.label11.TabIndex = 4;
		this.label11.Text = "Sample Analysis";
		this.panel14.Controls.Add(this.panel17);
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(563, 234);
		this.panel14.TabIndex = 0;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 32);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(563, 202);
		this.panel17.TabIndex = 1;
		this.grdHistroryPurAnalysis.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistroryPurAnalysis).BackColor = System.Drawing.Color.White;
		grdHistroryPurAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistroryPurAnalysis.DesignTimeLayout = grdHistroryPurAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistroryPurAnalysis.FilterMode = (FilterMode)2;
		this.grdHistroryPurAnalysis.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistroryPurAnalysis.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Name = "grdHistroryPurAnalysis";
		((ScrollableBase)this.grdHistroryPurAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Size = new System.Drawing.Size(563, 202);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).TabIndex = 3;
		this.grdHistroryPurAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdHistroryPurAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistroryPurAnalysis).VisualStyle = (VisualStyle)4;
		this.grdHistroryPurAnalysis.FormattingRow += new RowLoadEventHandler(grdHistroryPurAnalysis_FormattingRow);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Click += new System.EventHandler(grdHistroryPurAnalysis_Click);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistroryPurAnalysis_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label14);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(563, 32);
		this.panel16.TabIndex = 0;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(5, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(164, 24);
		this.label14.TabIndex = 3;
		this.label14.Text = "Purchase Analysis";
		this.panel4.Controls.Add(this.panel23);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.panel21);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1320, 445);
		this.panel4.TabIndex = 1;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 185);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1320, 260);
		this.panel23.TabIndex = 31;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1320, 260);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.CellUpdated += new ColumnActionEventHandler(grdhistory_CellUpdated);
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel20.Controls.Add(this.groupBox2);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 55);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1320, 130);
		this.panel20.TabIndex = 30;
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPestResultHistory);
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNoHistory);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(862, 124);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(554, 99);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(65, 15);
		this.label48.TabIndex = 103;
		this.label48.Text = "Pest Status";
		this.CmbPestStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPestStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPestStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPestStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPestStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPestStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory).Location = new System.Drawing.Point(624, 93);
		((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory).Name = "CmbPestStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbPestStatusHistory).TabIndex = 11;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(554, 72);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(64, 15);
		this.label47.TabIndex = 101;
		this.label47.Text = "Pest Result";
		this.CmbPestResultHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPestResultHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPestResultHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPestResultHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPestResultHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPestResultHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPestResultHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPestResultHistory).Location = new System.Drawing.Point(624, 66);
		((System.Windows.Forms.Control)(object)this.CmbPestResultHistory).Name = "CmbPestResultHistory";
		((System.Windows.Forms.Control)(object)this.CmbPestResultHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbPestResultHistory).TabIndex = 10;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(7, 99);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(64, 15);
		this.label46.TabIndex = 99;
		this.label46.Text = "To Doc No";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(7, 45);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 98;
		this.label45.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(554, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(41, 15);
		this.label27.TabIndex = 97;
		this.label27.Text = "GP No";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(328, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(229, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(86, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Supplier Name";
		this.CmbGpNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance218;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance227).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Location = new System.Drawing.Point(624, 12);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Name = "CmbGpNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(554, 45);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(64, 15);
		this.label26.TabIndex = 95;
		this.label26.Text = "Vehicle No";
		this.CmbVehicleNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Location = new System.Drawing.Point(624, 39);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Name = "CmbVehicleNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).TabIndex = 9;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(229, 99);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(95, 15);
		this.label25.TabIndex = 93;
		this.label25.Text = "Approved Status";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(229, 72);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(82, 15);
		this.label24.TabIndex = 92;
		this.label24.Text = "Analyst Status";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(229, 45);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 91;
		this.label23.Text = "Item Name";
		this.cmbApprovedStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbApprovedStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApprovedStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApprovedStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApprovedStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Location = new System.Drawing.Point(328, 93);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Name = "cmbApprovedStatusHistory";
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).TabIndex = 7;
		this.CmbAnalystStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalystStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalystStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalystStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalystStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Location = new System.Drawing.Point(328, 66);
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Name = "CmbAnalystStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).TabIndex = 6;
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(328, 39);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 5;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(794, 93);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 12;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(7, 72);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(79, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "From Doc No";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(7, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(90, 95);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(90, 68);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(90, 41);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(90, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label15);
		this.panel21.Controls.Add(this.ctrlGrdBar2);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 27);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1320, 28);
		this.panel21.TabIndex = 29;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(67, 19);
		this.label15.TabIndex = 4;
		this.label15.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1280, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1320, 27);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.txt.AutoSize = true;
		this.txt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txt.Location = new System.Drawing.Point(209, 61);
		this.txt.Name = "txt";
		this.txt.Size = new System.Drawing.Size(49, 15);
		this.txt.TabIndex = 0;
		this.txt.Text = "Bilty No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 168);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Item";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1334, 711);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.ctrlGrdBar1);
		this.tabForm.Controls.Add(this.PanalParamGrdsForm);
		this.tabForm.Controls.Add(this.PanelFeildForms);
		this.tabForm.Controls.Add(this.panel22);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1326, 685);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1375, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 35;
		this.PanalParamGrdsForm.AutoSize = true;
		this.PanalParamGrdsForm.Controls.Add(this.panel10);
		this.PanalParamGrdsForm.Controls.Add(this.panel9);
		this.PanalParamGrdsForm.Controls.Add(this.panel5);
		this.PanalParamGrdsForm.Controls.Add(this.panel3);
		this.PanalParamGrdsForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanalParamGrdsForm.Location = new System.Drawing.Point(806, 53);
		this.PanalParamGrdsForm.Name = "PanalParamGrdsForm";
		this.PanalParamGrdsForm.Size = new System.Drawing.Size(517, 629);
		this.PanalParamGrdsForm.TabIndex = 38;
		this.panel10.AutoScroll = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdSampleSubItem);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdSample);
		this.panel10.Controls.Add(this.panel6);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(500, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(450, 629);
		this.panel10.TabIndex = 5;
		this.grdSampleSubItem.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSampleSubItem).BackColor = System.Drawing.Color.White;
		grdSampleSubItem_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSampleSubItem.DesignTimeLayout = grdSampleSubItem_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSampleSubItem.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSampleSubItem.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).Location = new System.Drawing.Point(0, 488);
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).Name = "grdSampleSubItem";
		((ScrollableBase)this.grdSampleSubItem).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).Size = new System.Drawing.Size(450, 141);
		((System.Windows.Forms.Control)(object)this.grdSampleSubItem).TabIndex = 0;
		this.grdSampleSubItem.TotalRow = (InheritableBoolean)1;
		this.grdSampleSubItem.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSampleSubItem).VisualStyle = (VisualStyle)4;
		this.grdSample.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSample).BackColor = System.Drawing.Color.White;
		grdSample_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSample.DesignTimeLayout = grdSample_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSample).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdSample).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSample.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSample).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdSample).Name = "grdSample";
		((ScrollableBase)this.grdSample).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSample).Size = new System.Drawing.Size(450, 463);
		((System.Windows.Forms.Control)(object)this.grdSample).TabIndex = 0;
		this.grdSample.TotalRow = (InheritableBoolean)1;
		this.grdSample.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSample).VisualStyle = (VisualStyle)4;
		this.grdSample.LinkClicked += new ColumnActionEventHandler(grdSample_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label13);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(450, 25);
		this.panel6.TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.Teal;
		this.label13.Location = new System.Drawing.Point(3, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(285, 18);
		this.label13.TabIndex = 2;
		this.label13.Text = "Sample Analysis Parameters' Results";
		this.panel9.AutoSize = true;
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(500, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(0, 629);
		this.panel9.TabIndex = 4;
		this.panel5.AutoSize = true;
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(500, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(0, 629);
		this.panel5.TabIndex = 3;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdSubItems);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(500, 629);
		this.panel3.TabIndex = 2;
		this.grdSubItems.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSubItems).BackColor = System.Drawing.Color.White;
		grdSubItems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSubItems.DesignTimeLayout = grdSubItems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSubItems.FilterMode = (FilterMode)2;
		this.grdSubItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSubItems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Location = new System.Drawing.Point(0, 488);
		((System.Windows.Forms.Control)(object)this.grdSubItems).Name = "grdSubItems";
		((ScrollableBase)this.grdSubItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Size = new System.Drawing.Size(500, 141);
		((System.Windows.Forms.Control)(object)this.grdSubItems).TabIndex = 0;
		this.grdSubItems.TotalRow = (InheritableBoolean)1;
		this.grdSubItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSubItems).VisualStyle = (VisualStyle)4;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.PaymentVoucherLable);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(500, 25);
		this.panel2.TabIndex = 2;
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.Teal;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(5, 2);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(299, 18);
		this.PaymentVoucherLable.TabIndex = 1;
		this.PaymentVoucherLable.Text = "Purchase Analysis Parameters' Results";
		this.PanelFeildForms.Controls.Add(this.label49);
		this.PanelFeildForms.Controls.Add(this.txtGpQty);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPestStatus);
		this.PanelFeildForms.Controls.Add(this.label44);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPestResult);
		this.PanelFeildForms.Controls.Add(this.label43);
		this.PanelFeildForms.Controls.Add(this.txtRateCut);
		this.PanelFeildForms.Controls.Add(this.groupBoxGP);
		this.PanelFeildForms.Controls.Add(this.PartyLotRefNo);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis);
		this.PanelFeildForms.Controls.Add(this.txtbiltyno);
		this.PanelFeildForms.Controls.Add(this.datBiltyDate);
		this.PanelFeildForms.Controls.Add(this.txtdocdate);
		this.PanelFeildForms.Controls.Add(this.txtWeightCutUom);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.PanelFeildForms.Controls.Add(this.txtQtyForWtCut);
		this.PanelFeildForms.Controls.Add(this.label30);
		this.PanelFeildForms.Controls.Add(this.label37);
		this.PanelFeildForms.Controls.Add(this.label28);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.PanelFeildForms.Controls.Add(this.label36);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.PanelFeildForms.Controls.Add(this.label20);
		this.PanelFeildForms.Controls.Add(this.txtremarks);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyName);
		this.PanelFeildForms.Controls.Add(this.label19);
		this.PanelFeildForms.Controls.Add(this.label17);
		this.PanelFeildForms.Controls.Add(this.label16);
		this.PanelFeildForms.Controls.Add(this.txtScheduleId);
		this.PanelFeildForms.Controls.Add(this.txtApprovalStatus);
		this.PanelFeildForms.Controls.Add(this.label35);
		this.PanelFeildForms.Controls.Add(this.txtApprovedBy);
		this.PanelFeildForms.Controls.Add(this.label34);
		this.PanelFeildForms.Controls.Add(this.button3);
		this.PanelFeildForms.Controls.Add(this.btnResetCooking);
		this.PanelFeildForms.Controls.Add(this.btnCapturecooking);
		this.PanelFeildForms.Controls.Add(this.btnStartCooking);
		this.PanelFeildForms.Controls.Add(this.btnResetDriv);
		this.PanelFeildForms.Controls.Add(this.TakePhotoButton);
		this.PanelFeildForms.Controls.Add(this.LiveViewButton);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabStatus);
		this.PanelFeildForms.Controls.Add(this.txtPurchaseOrderId);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGatePassNo);
		this.PanelFeildForms.Controls.Add(this.ChkRateCutWillApply);
		this.PanelFeildForms.Controls.Add(this.ChkWtCutWillApply);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWtCuton);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbanalysisgroup);
		this.PanelFeildForms.Controls.Add(this.txtWeightCut);
		this.PanelFeildForms.Controls.Add(this.labeldeduciton);
		this.PanelFeildForms.Controls.Add(this.txtMoisture);
		this.PanelFeildForms.Controls.Add(this.label22);
		this.PanelFeildForms.Controls.Add(this.txtAnaylstName);
		this.PanelFeildForms.Controls.Add(this.label12);
		this.PanelFeildForms.Controls.Add(this.btnimgprevcookingpic);
		this.PanelFeildForms.Controls.Add(this.btnprv);
		this.PanelFeildForms.Controls.Add(this.txtSampleId);
		this.PanelFeildForms.Controls.Add(this.label9);
		this.PanelFeildForms.Controls.Add(this.label18);
		this.PanelFeildForms.Controls.Add(this.label4);
		this.PanelFeildForms.Controls.Add(this.txtNoofBagsInspection);
		this.PanelFeildForms.Controls.Add(this.txtvehicleno);
		this.PanelFeildForms.Controls.Add(this.txtPurchaseType);
		this.PanelFeildForms.Controls.Add(this.txtCityArea);
		this.PanelFeildForms.Controls.Add(this.label3);
		this.PanelFeildForms.Controls.Add(this.label2);
		this.PanelFeildForms.Controls.Add(this.btnimage2);
		this.PanelFeildForms.Controls.Add(this.btnimg1);
		this.PanelFeildForms.Controls.Add(this.cookingpic);
		this.PanelFeildForms.Controls.Add(this.LiveViewPicBox);
		this.PanelFeildForms.Controls.Add(this.txtOrderNo);
		this.PanelFeildForms.Controls.Add(this.txtdocno);
		this.PanelFeildForms.Controls.Add(this.cmpcity);
		this.PanelFeildForms.Controls.Add(this.label33);
		this.PanelFeildForms.Controls.Add(this.label6);
		this.PanelFeildForms.Controls.Add(this.label32);
		this.PanelFeildForms.Controls.Add(this.label5);
		this.PanelFeildForms.Controls.Add(this.txt);
		this.PanelFeildForms.Controls.Add(this.cmbsupplier);
		this.PanelFeildForms.Controls.Add(this.label10);
		this.PanelFeildForms.Controls.Add(this.label29);
		this.PanelFeildForms.Controls.Add(this.label31);
		this.PanelFeildForms.Controls.Add(this.txtsampledate);
		this.PanelFeildForms.Controls.Add(this.label8);
		this.PanelFeildForms.Controls.Add(this.label7);
		this.PanelFeildForms.Controls.Add(this.label1);
		this.PanelFeildForms.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelFeildForms.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.PanelFeildForms.Location = new System.Drawing.Point(3, 53);
		this.PanelFeildForms.Margin = new System.Windows.Forms.Padding(2);
		this.PanelFeildForms.Name = "PanelFeildForms";
		this.PanelFeildForms.Size = new System.Drawing.Size(803, 629);
		this.PanelFeildForms.TabIndex = 0;
		this.CmbPestStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbPestStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPestStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPestStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance251;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance252).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance252).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance252).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance253).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance255).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance256;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance257).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance258).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance258).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance258).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance258).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance260).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance260;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPestStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPestStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).Location = new System.Drawing.Point(511, 329);
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).Name = "CmbPestStatus";
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).Size = new System.Drawing.Size(291, 26);
		((System.Windows.Forms.Control)(object)this.CmbPestStatus).TabIndex = 37;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(400, 335);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(65, 15);
		this.label44.TabIndex = 69;
		this.label44.Text = "Pest Status";
		this.CmbPestResult.AllowNull = (DefaultableBoolean)1;
		this.CmbPestResult.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPestResult.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPestResult.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestResult).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbPestResult).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPestResult).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPestResult).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPestResult.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPestResult).Location = new System.Drawing.Point(511, 302);
		((System.Windows.Forms.Control)(object)this.CmbPestResult).Name = "CmbPestResult";
		((System.Windows.Forms.Control)(object)this.CmbPestResult).Size = new System.Drawing.Size(291, 26);
		((System.Windows.Forms.Control)(object)this.CmbPestResult).TabIndex = 36;
		this.CmbPestResult.ValueChanged += new System.EventHandler(CmbPestResult_ValueChanged);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(401, 308);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(64, 15);
		this.label43.TabIndex = 67;
		this.label43.Text = "Pest Result";
		this.txtRateCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateCut.Location = new System.Drawing.Point(511, 82);
		this.txtRateCut.Name = "txtRateCut";
		this.txtRateCut.Size = new System.Drawing.Size(80, 23);
		this.txtRateCut.TabIndex = 30;
		this.txtRateCut.TextChanged += new System.EventHandler(txtLessRate_TextChanged);
		this.txtRateCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLessRate_KeyPress);
		this.groupBoxGP.Controls.Add(this.txtLessRate);
		this.groupBoxGP.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBoxGP.Controls.Add(this.label38);
		this.groupBoxGP.Controls.Add(this.label39);
		this.groupBoxGP.Controls.Add(this.label40);
		this.groupBoxGP.Controls.Add(this.label41);
		this.groupBoxGP.Controls.Add(this.label42);
		this.groupBoxGP.Controls.Add(this.txtItemRate);
		this.groupBoxGP.Controls.Add(this.txtNetRate);
		this.groupBoxGP.Controls.Add(this.txtAddRate);
		this.groupBoxGP.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBoxGP.Location = new System.Drawing.Point(403, 135);
		this.groupBoxGP.Name = "groupBoxGP";
		this.groupBoxGP.Size = new System.Drawing.Size(398, 73);
		this.groupBoxGP.TabIndex = 65;
		this.groupBoxGP.TabStop = false;
		this.groupBoxGP.Text = "Only for Gate Purchase";
		this.txtLessRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLessRate.Location = new System.Drawing.Point(205, 18);
		this.txtLessRate.Name = "txtLessRate";
		this.txtLessRate.Size = new System.Drawing.Size(60, 23);
		this.txtLessRate.TabIndex = 30;
		this.txtLessRate.TextChanged += new System.EventHandler(txtLessRate_TextChanged);
		this.txtLessRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLessRate_KeyPress);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(205, 43);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(189, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 28;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(CmbRateUom_Leave);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(3, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(60, 15);
		this.label38.TabIndex = 89;
		this.label38.Text = "Net / Rate";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(147, 21);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(50, 15);
		this.label39.TabIndex = 92;
		this.label39.Text = "Less Cut";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(147, 47);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(59, 15);
		this.label40.TabIndex = 67;
		this.label40.Text = "Rate Uom";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(3, 21);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(58, 15);
		this.label41.TabIndex = 84;
		this.label41.Text = "Item Rate";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(269, 21);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(56, 15);
		this.label42.TabIndex = 85;
		this.label42.Text = "Premium";
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(64, 14);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.ReadOnly = true;
		this.txtItemRate.Size = new System.Drawing.Size(80, 23);
		this.txtItemRate.TabIndex = 29;
		this.txtItemRate.TextChanged += new System.EventHandler(txtLessRate_TextChanged);
		this.txtItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateCut_KeyPress);
		this.txtNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetRate.Location = new System.Drawing.Point(64, 43);
		this.txtNetRate.Name = "txtNetRate";
		this.txtNetRate.ReadOnly = true;
		this.txtNetRate.Size = new System.Drawing.Size(80, 23);
		this.txtNetRate.TabIndex = 32;
		this.txtNetRate.TabStop = false;
		this.txtAddRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddRate.Location = new System.Drawing.Point(329, 18);
		this.txtAddRate.Name = "txtAddRate";
		this.txtAddRate.Size = new System.Drawing.Size(65, 23);
		this.txtAddRate.TabIndex = 31;
		this.txtAddRate.TextChanged += new System.EventHandler(txtAddRate_TextChanged);
		this.txtAddRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddRate_KeyPress);
		this.PartyLotRefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PartyLotRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PartyLotRefNo.Location = new System.Drawing.Point(267, 31);
		this.PartyLotRefNo.Name = "PartyLotRefNo";
		this.PartyLotRefNo.Size = new System.Drawing.Size(130, 23);
		this.PartyLotRefNo.TabIndex = 3;
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(267, 190);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 13;
		this.CmbSampleAnaylsis.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleAnaylsis.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleAnaylsis.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleAnaylsis.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleAnaylsis).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleAnaylsis.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).Location = new System.Drawing.Point(267, 107);
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).Name = "CmbSampleAnaylsis";
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbSampleAnaylsis).Leave += new System.EventHandler(CmbSampleAnaylsis_Leave);
		this.txtbiltyno.BackColor = System.Drawing.SystemColors.Control;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Enabled = false;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyno.Location = new System.Drawing.Point(267, 57);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(130, 23);
		this.txtbiltyno.TabIndex = 5;
		this.datBiltyDate.CustomFormat = "dd-MM-yy";
		this.datBiltyDate.Enabled = false;
		this.datBiltyDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datBiltyDate.Location = new System.Drawing.Point(267, 82);
		this.datBiltyDate.Name = "datBiltyDate";
		this.datBiltyDate.Size = new System.Drawing.Size(130, 23);
		this.datBiltyDate.TabIndex = 7;
		this.txtdocdate.CustomFormat = "dd-MM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(267, 4);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(130, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtWeightCutUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCutUom.Location = new System.Drawing.Point(511, 57);
		this.txtWeightCutUom.Name = "txtWeightCutUom";
		this.txtWeightCutUom.ShortcutsEnabled = false;
		this.txtWeightCutUom.Size = new System.Drawing.Size(80, 23);
		this.txtWeightCutUom.TabIndex = 23;
		this.txtWeightCutUom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightCutUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance264;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(107, 162);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.txtQtyForWtCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyForWtCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyForWtCut.Location = new System.Drawing.Point(684, 61);
		this.txtQtyForWtCut.Name = "txtQtyForWtCut";
		this.txtQtyForWtCut.Size = new System.Drawing.Size(117, 23);
		this.txtQtyForWtCut.TabIndex = 24;
		this.txtQtyForWtCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyForWtCut_KeyPress);
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(595, 60);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(90, 30);
		this.label30.TabIndex = 64;
		this.label30.Text = "Qty For Weight Cut / Touch";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(400, 61);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(113, 15);
		this.label37.TabIndex = 62;
		this.label37.Text = "Weight Cut Uom Kg";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(400, 86);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(51, 15);
		this.label28.TabIndex = 62;
		this.label28.Text = "Rate Cut";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(511, 247);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 34;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(400, 253);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(89, 15);
		this.label36.TabIndex = 60;
		this.label36.Text = "JobLot (Assign)";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(511, 219);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 33;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(400, 225);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(115, 15);
		this.label20.TabIndex = 60;
		this.label20.Text = "WareHouse (Assign)";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(511, 357);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(290, 39);
		this.txtremarks.TabIndex = 38;
		this.cmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance238;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance239).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance239).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance244).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance247).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Location = new System.Drawing.Point(107, 134);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Name = "cmbPartyName";
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).TabIndex = 10;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(7, 140);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(69, 15);
		this.label19.TabIndex = 57;
		this.label19.Text = "Party Name";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(209, 35);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 56;
		this.label17.Text = "Party Lot#";
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(209, 188);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(52, 30);
		this.label16.TabIndex = 54;
		this.label16.Text = "Packing Type";
		this.txtScheduleId.Location = new System.Drawing.Point(1496, 361);
		this.txtScheduleId.Name = "txtScheduleId";
		this.txtScheduleId.Size = new System.Drawing.Size(26, 25);
		this.txtScheduleId.TabIndex = 53;
		this.txtScheduleId.Visible = false;
		this.txtApprovalStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalStatus.Enabled = false;
		this.txtApprovalStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalStatus.ForeColor = System.Drawing.Color.Red;
		this.txtApprovalStatus.Location = new System.Drawing.Point(107, 355);
		this.txtApprovalStatus.Name = "txtApprovalStatus";
		this.txtApprovalStatus.Size = new System.Drawing.Size(290, 23);
		this.txtApprovalStatus.TabIndex = 19;
		this.txtApprovalStatus.TabStop = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(7, 359);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(91, 15);
		this.label35.TabIndex = 52;
		this.label35.Text = "Approval Status";
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovedBy.Location = new System.Drawing.Point(107, 331);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.ReadOnly = true;
		this.txtApprovedBy.Size = new System.Drawing.Size(290, 23);
		this.txtApprovedBy.TabIndex = 18;
		this.txtApprovedBy.TabStop = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(7, 335);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(75, 15);
		this.label34.TabIndex = 52;
		this.label34.Text = "Approved By";
		this.button3.BackColor = System.Drawing.Color.Teal;
		this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button3.ForeColor = System.Drawing.Color.White;
		this.button3.Location = new System.Drawing.Point(416, 579);
		this.button3.Name = "button3";
		this.button3.Size = new System.Drawing.Size(80, 43);
		this.button3.TabIndex = 45;
		this.button3.Text = "Camera Setting";
		this.button3.UseVisualStyleBackColor = false;
		this.button3.Visible = false;
		this.button3.Click += new System.EventHandler(button3_Click);
		this.btnResetCooking.BackColor = System.Drawing.Color.Teal;
		this.btnResetCooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetCooking.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetCooking.ForeColor = System.Drawing.Color.White;
		this.btnResetCooking.Location = new System.Drawing.Point(702, 593);
		this.btnResetCooking.Name = "btnResetCooking";
		this.btnResetCooking.Size = new System.Drawing.Size(99, 29);
		this.btnResetCooking.TabIndex = 44;
		this.btnResetCooking.Text = "Reset";
		this.btnResetCooking.UseVisualStyleBackColor = false;
		this.btnResetCooking.Click += new System.EventHandler(btnResetCooking_Click);
		this.btnCapturecooking.BackColor = System.Drawing.Color.Teal;
		this.btnCapturecooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCapturecooking.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCapturecooking.ForeColor = System.Drawing.Color.White;
		this.btnCapturecooking.Location = new System.Drawing.Point(607, 593);
		this.btnCapturecooking.Name = "btnCapturecooking";
		this.btnCapturecooking.Size = new System.Drawing.Size(94, 29);
		this.btnCapturecooking.TabIndex = 43;
		this.btnCapturecooking.Text = "Browse";
		this.btnCapturecooking.UseVisualStyleBackColor = false;
		this.btnCapturecooking.Click += new System.EventHandler(btnCapturecooking_Click);
		this.btnStartCooking.BackColor = System.Drawing.Color.Teal;
		this.btnStartCooking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnStartCooking.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnStartCooking.ForeColor = System.Drawing.Color.White;
		this.btnStartCooking.Location = new System.Drawing.Point(511, 593);
		this.btnStartCooking.Name = "btnStartCooking";
		this.btnStartCooking.Size = new System.Drawing.Size(94, 29);
		this.btnStartCooking.TabIndex = 42;
		this.btnStartCooking.Text = "Camera";
		this.btnStartCooking.UseVisualStyleBackColor = false;
		this.btnStartCooking.Click += new System.EventHandler(btnStartCooking_Click);
		this.btnResetDriv.BackColor = System.Drawing.Color.Teal;
		this.btnResetDriv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetDriv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetDriv.ForeColor = System.Drawing.Color.White;
		this.btnResetDriv.Location = new System.Drawing.Point(298, 593);
		this.btnResetDriv.Name = "btnResetDriv";
		this.btnResetDriv.Size = new System.Drawing.Size(99, 29);
		this.btnResetDriv.TabIndex = 41;
		this.btnResetDriv.Text = "Reset";
		this.btnResetDriv.UseVisualStyleBackColor = false;
		this.btnResetDriv.Click += new System.EventHandler(btnResetDriv_Click);
		this.TakePhotoButton.BackColor = System.Drawing.Color.Teal;
		this.TakePhotoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.TakePhotoButton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.TakePhotoButton.ForeColor = System.Drawing.Color.White;
		this.TakePhotoButton.Location = new System.Drawing.Point(107, 593);
		this.TakePhotoButton.Name = "TakePhotoButton";
		this.TakePhotoButton.Size = new System.Drawing.Size(94, 29);
		this.TakePhotoButton.TabIndex = 39;
		this.TakePhotoButton.Text = "Browse";
		this.TakePhotoButton.UseVisualStyleBackColor = false;
		this.TakePhotoButton.Click += new System.EventHandler(TakePhotoButton_Click);
		this.LiveViewButton.BackColor = System.Drawing.Color.Teal;
		this.LiveViewButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.LiveViewButton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LiveViewButton.ForeColor = System.Drawing.Color.White;
		this.LiveViewButton.Location = new System.Drawing.Point(202, 593);
		this.LiveViewButton.Name = "LiveViewButton";
		this.LiveViewButton.Size = new System.Drawing.Size(94, 29);
		this.LiveViewButton.TabIndex = 40;
		this.LiveViewButton.Text = "Camera";
		this.LiveViewButton.UseVisualStyleBackColor = false;
		this.LiveViewButton.Click += new System.EventHandler(LiveViewButton_Click);
		this.CmbLabStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbLabStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLabStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Location = new System.Drawing.Point(107, 302);
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Name = "CmbLabStatus";
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).TabIndex = 17;
		this.txtPurchaseOrderId.Location = new System.Drawing.Point(1468, 332);
		this.txtPurchaseOrderId.Margin = new System.Windows.Forms.Padding(2);
		this.txtPurchaseOrderId.Name = "txtPurchaseOrderId";
		this.txtPurchaseOrderId.Size = new System.Drawing.Size(26, 25);
		this.txtPurchaseOrderId.TabIndex = 42;
		this.txtPurchaseOrderId.Visible = false;
		this.cmbGatePassNo.AllowNull = (DefaultableBoolean)1;
		this.cmbGatePassNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGatePassNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGatePassNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance243).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance254).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance254).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance254).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance254;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGatePassNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Location = new System.Drawing.Point(107, 29);
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Name = "cmbGatePassNo";
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Leave += new System.EventHandler(txtgatepassno_Leave);
		this.ChkRateCutWillApply.AutoSize = true;
		this.ChkRateCutWillApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkRateCutWillApply.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.ChkRateCutWillApply.Location = new System.Drawing.Point(664, 108);
		this.ChkRateCutWillApply.Name = "ChkRateCutWillApply";
		this.ChkRateCutWillApply.Size = new System.Drawing.Size(133, 19);
		this.ChkRateCutWillApply.TabIndex = 26;
		this.ChkRateCutWillApply.Text = "Apply Rate Cut Later";
		this.ChkRateCutWillApply.UseVisualStyleBackColor = true;
		this.ChkWtCutWillApply.AutoSize = true;
		this.ChkWtCutWillApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkWtCutWillApply.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.ChkWtCutWillApply.Location = new System.Drawing.Point(511, 108);
		this.ChkWtCutWillApply.Name = "ChkWtCutWillApply";
		this.ChkWtCutWillApply.Size = new System.Drawing.Size(149, 19);
		this.ChkWtCutWillApply.TabIndex = 25;
		this.ChkWtCutWillApply.Text = "Apply Weight Cut Later";
		this.ChkWtCutWillApply.UseVisualStyleBackColor = true;
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(107, 190);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 12;
		this.CmbWtCuton.AllowNull = (DefaultableBoolean)1;
		this.CmbWtCuton.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWtCuton.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWtCuton.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance219).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWtCuton.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Location = new System.Drawing.Point(684, 29);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Name = "CmbWtCuton";
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).TabIndex = 22;
		this.CmbWtCuton.ValueChanged += new System.EventHandler(CmbWtCuton_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Leave += new System.EventHandler(CmbWtCuton_Leave);
		this.cmbanalysisgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbanalysisgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbanalysisgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbanalysisgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance225;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance233).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbanalysisgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Location = new System.Drawing.Point(107, 219);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Name = "cmbanalysisgroup";
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Leave += new System.EventHandler(cmbanalysisgroup_Leave);
		this.txtWeightCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCut.Location = new System.Drawing.Point(511, 31);
		this.txtWeightCut.Name = "txtWeightCut";
		this.txtWeightCut.Size = new System.Drawing.Size(80, 23);
		this.txtWeightCut.TabIndex = 21;
		this.labeldeduciton.AutoSize = true;
		this.labeldeduciton.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labeldeduciton.Location = new System.Drawing.Point(400, 35);
		this.labeldeduciton.Name = "labeldeduciton";
		this.labeldeduciton.Size = new System.Drawing.Size(111, 15);
		this.labeldeduciton.TabIndex = 40;
		this.labeldeduciton.Text = "Weight Cut / Touch";
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Enabled = false;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(511, 274);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(290, 27);
		this.txtMoisture.TabIndex = 35;
		this.txtMoisture.Text = "0";
		this.txtMoisture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(400, 277);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 21);
		this.label22.TabIndex = 38;
		this.label22.Text = "Moisture";
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnaylstName.Location = new System.Drawing.Point(107, 249);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.Size = new System.Drawing.Size(290, 23);
		this.txtAnaylstName.TabIndex = 15;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 253);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 38;
		this.label12.Text = "AnaylstName";
		this.btnimgprevcookingpic.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimgprevcookingpic.Location = new System.Drawing.Point(1390, 332);
		this.btnimgprevcookingpic.Name = "btnimgprevcookingpic";
		this.btnimgprevcookingpic.Size = new System.Drawing.Size(76, 27);
		this.btnimgprevcookingpic.TabIndex = 19;
		this.btnimgprevcookingpic.Text = "Preview";
		this.btnimgprevcookingpic.UseVisualStyleBackColor = true;
		this.btnimgprevcookingpic.Visible = false;
		this.btnimgprevcookingpic.Click += new System.EventHandler(btnimgprevcookingpic_Click);
		this.btnprv.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprv.Location = new System.Drawing.Point(1313, 330);
		this.btnprv.Name = "btnprv";
		this.btnprv.Size = new System.Drawing.Size(70, 27);
		this.btnprv.TabIndex = 17;
		this.btnprv.Text = "Preview";
		this.btnprv.UseVisualStyleBackColor = true;
		this.btnprv.Visible = false;
		this.btnprv.Click += new System.EventHandler(btnprv_Click);
		this.txtSampleId.Location = new System.Drawing.Point(1468, 361);
		this.txtSampleId.Margin = new System.Windows.Forms.Padding(2);
		this.txtSampleId.Name = "txtSampleId";
		this.txtSampleId.Size = new System.Drawing.Size(26, 25);
		this.txtSampleId.TabIndex = 35;
		this.txtSampleId.Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(7, 196);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 33;
		this.label9.Text = "Crop Year";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 86);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 32;
		this.label18.Text = "Analysis Qty";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 61);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "Vehicle No";
		this.txtNoofBagsInspection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoofBagsInspection.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoofBagsInspection.Location = new System.Drawing.Point(107, 82);
		this.txtNoofBagsInspection.Name = "txtNoofBagsInspection";
		this.txtNoofBagsInspection.Size = new System.Drawing.Size(100, 23);
		this.txtNoofBagsInspection.TabIndex = 6;
		this.txtNoofBagsInspection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoofBagsInspection_KeyPress);
		this.txtvehicleno.BackColor = System.Drawing.SystemColors.Control;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Enabled = false;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(107, 57);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(100, 23);
		this.txtvehicleno.TabIndex = 4;
		this.txtPurchaseType.BackColor = System.Drawing.SystemColors.Control;
		this.txtPurchaseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseType.Enabled = false;
		this.txtPurchaseType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPurchaseType.Location = new System.Drawing.Point(511, 4);
		this.txtPurchaseType.Name = "txtPurchaseType";
		this.txtPurchaseType.Size = new System.Drawing.Size(174, 23);
		this.txtPurchaseType.TabIndex = 20;
		this.txtCityArea.BackColor = System.Drawing.SystemColors.Control;
		this.txtCityArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCityArea.Enabled = false;
		this.txtCityArea.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCityArea.Location = new System.Drawing.Point(107, 274);
		this.txtCityArea.Name = "txtCityArea";
		this.txtCityArea.Size = new System.Drawing.Size(290, 23);
		this.txtCityArea.TabIndex = 16;
		this.label3.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label3.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(511, 401);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(290, 15);
		this.label3.TabIndex = 29;
		this.label3.Text = "Cooking Pic";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label2.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(107, 403);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(290, 15);
		this.label2.TabIndex = 28;
		this.label2.Text = "Analysis Pic";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnimage2.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimage2.Location = new System.Drawing.Point(1390, 359);
		this.btnimage2.Name = "btnimage2";
		this.btnimage2.Size = new System.Drawing.Size(75, 27);
		this.btnimage2.TabIndex = 18;
		this.btnimage2.Text = "Browse";
		this.btnimage2.UseVisualStyleBackColor = true;
		this.btnimage2.Visible = false;
		this.cookingpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cookingpic.Location = new System.Drawing.Point(511, 421);
		this.cookingpic.Name = "cookingpic";
		this.cookingpic.Size = new System.Drawing.Size(290, 170);
		this.cookingpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.cookingpic.TabIndex = 27;
		this.cookingpic.TabStop = false;
		this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.LiveViewPicBox.Location = new System.Drawing.Point(107, 421);
		this.LiveViewPicBox.Name = "LiveViewPicBox";
		this.LiveViewPicBox.Size = new System.Drawing.Size(290, 170);
		this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.LiveViewPicBox.TabIndex = 27;
		this.LiveViewPicBox.TabStop = false;
		this.txtOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNo.Enabled = false;
		this.txtOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNo.Location = new System.Drawing.Point(107, 109);
		this.txtOrderNo.Name = "txtOrderNo";
		this.txtOrderNo.Size = new System.Drawing.Size(100, 23);
		this.txtOrderNo.TabIndex = 8;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(400, 8);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(83, 15);
		this.label33.TabIndex = 0;
		this.label33.Text = "Purchase Type";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(7, 278);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(54, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "City Area";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(595, 35);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(86, 15);
		this.label29.TabIndex = 0;
		this.label29.Text = "Weight Cut On";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(209, 86);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(58, 15);
		this.label31.TabIndex = 0;
		this.label31.Text = "Bilty Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(400, 357);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 17);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Document No";
		this.panel22.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel22.Controls.Add(this.comboBox1);
		this.panel22.Controls.Add(this.label21);
		this.panel22.Controls.Add(this.IsPrint);
		this.panel22.Controls.Add(this.chkPrintII);
		this.panel22.Controls.Add(this.btn);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1320, 50);
		this.panel22.TabIndex = 58;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(693, 3);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(181, 21);
		this.comboBox1.TabIndex = 85;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(5, 30);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(155, 19);
		this.label21.TabIndex = 1;
		this.label21.Text = "Purchase Analysis";
		this.IsPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.IsPrint.AutoSize = true;
		this.IsPrint.Checked = true;
		this.IsPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.IsPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsPrint.ForeColor = System.Drawing.Color.White;
		this.IsPrint.Location = new System.Drawing.Point(1243, 30);
		this.IsPrint.Name = "IsPrint";
		this.IsPrint.Size = new System.Drawing.Size(75, 18);
		this.IsPrint.TabIndex = 37;
		this.IsPrint.Text = "Peview";
		this.IsPrint.UseVisualStyleBackColor = true;
		this.chkPrintII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintII.AutoSize = true;
		this.chkPrintII.Checked = true;
		this.chkPrintII.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintII.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintII.ForeColor = System.Drawing.Color.White;
		this.chkPrintII.Location = new System.Drawing.Point(1130, 30);
		this.chkPrintII.Name = "chkPrintII";
		this.chkPrintII.Size = new System.Drawing.Size(106, 18);
		this.chkPrintII.TabIndex = 38;
		this.chkPrintII.Text = "257 Peview";
		this.chkPrintII.UseVisualStyleBackColor = true;
		this.txtGpQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGpQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGpQty.Location = new System.Drawing.Point(736, 4);
		this.txtGpQty.Name = "txtGpQty";
		this.txtGpQty.ShortcutsEnabled = false;
		this.txtGpQty.Size = new System.Drawing.Size(63, 23);
		this.txtGpQty.TabIndex = 70;
		this.txtGpQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(687, 8);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(44, 15);
		this.label49.TabIndex = 71;
		this.label49.Text = "Gp Qty";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1334, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvLabPurchaseAnalysis";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabPurchaseAnalysis_FormClosing);
		base.Load += new System.EventHandler(InvLabPurchaseAnalysis_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabPurchaseAnalysis_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistrorySamAnalysis).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPestStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPestResultHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.PanalParamGrdsForm.ResumeLayout(false);
		this.PanalParamGrdsForm.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSampleSubItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdSample).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSubItems).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.PanelFeildForms.ResumeLayout(false);
		this.PanelFeildForms.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPestStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPestResult).EndInit();
		this.groupBoxGP.ResumeLayout(false);
		this.groupBoxGP.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleAnaylsis).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePassNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWtCuton).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).EndInit();
		((System.ComponentModel.ISupportInitialize)this.LiveViewPicBox).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
