using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Main;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.PartyProcessing;
using Architecture.Model.WeightBridge;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lab;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.WeightBridge;

public class frmWeightbridge : Form
{
	private string CameraPixBox1Path = string.Empty;

	private string PixBox1Path = "";

	private string IpCam01 = string.Empty;

	private short Port01;

	private string UserNameCam01 = string.Empty;

	private string PasswordCam01 = string.Empty;

	private string IpCam02 = string.Empty;

	private short Port02;

	private string UserNameCam02 = string.Empty;

	private string PasswordCam02 = string.Empty;

	private string IpCam03 = string.Empty;

	private short Port03;

	private string UserNameCam03 = string.Empty;

	private string PasswordCam03 = string.Empty;

	private HVUtility HvCam01;

	private HVUtility HvCam02;

	private HVUtility HvCam03;

	private HVUtility HvCam04;

	private HVUtility HvCam05;

	private HVUtility HvCam06;

	private string PORTNAMEWB = "";

	private int BaudRateWb;

	private int ReadTimeOutWb;

	private int WriteTimeOutWb;

	public static Image Logo;

	private int RECID;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string fileSavePath2 = "";

	private string ofd1 = string.Empty;

	private string ofd = string.Empty;

	private string fileSavePath3 = "";

	private string fileSavePath4 = "";

	private string ofd2 = string.Empty;

	private string ofd3 = string.Empty;

	private string fileSavePath5 = "";

	private string fileSavePath6 = "";

	private string ofd4 = string.Empty;

	private string ofd5 = string.Empty;

	private bool grosschk;

	private bool tarechk;

	private int Id;

	private bool Approved;

	private int RefDocumentTypeId;

	private bool lstIsSavedVehicle;

	private bool IsLabCompulsoryOnWBForMarketPur;

	private bool IsLabCompulsoryOnWBForGatePur;

	private bool IsLabCompulsoryOnWBForRegularPur;

	private bool LabCompulsoryBeforFirstWeight;

	private bool LabCompulsoryAfterFirstWeight;

	private double weighbridgeTolerance;

	private string UniqFileName = string.Empty;

	private string UniqFileName2 = string.Empty;

	private string UniqFileName3 = string.Empty;

	private string UniqFileName4 = string.Empty;

	private string UniqFileName5 = string.Empty;

	private string UniqFileName6 = string.Empty;

	private string keySequence = "";

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtWbList = new DataTable();

	private bool UpdateMode;

	private bool WeighBridgeForOutwardPartyAndItemWise;

	private bool MultiWeightBridge;

	private bool FormInitialized;

	private int DefaultDaysToLessFromHistoryFromDate;

	private double SupplierFirstWeight;

	private double SupplierSecondWeight;

	private DataTable dtitem = new DataTable();

	private DataTable dtitemLookups = new DataTable();

	private DataTable dtParty = new DataTable();

	private DataTable dtPartiesAndItemsFromDeliveryOrder = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private TextBox txtsecondWeight;

	private TextBox txtFirstWeight;

	private Label label13;

	private TextBox txtRemarks;

	private Label label12;

	private TextBox txtWbCharges;

	private Label label11;

	private Label label10;

	private TextBox txtItemqty;

	private Label label9;

	private Label label8;

	private TextBox txtVehicleNo;

	private Label label7;

	private Label label5;

	private TextBox txtBilityNo;

	private Label label4;

	private DateTimePicker txtSecondDate;

	private DateTimePicker txtFirstDate;

	private Label label2;

	private TextBox txtTickectNo;

	private TextBox txtWeightReading;

	private Panel panel6;

	private Label label15;

	private Panel panel4;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Button btnTareForntPic;

	private Button btnGrossFontPIc;

	private Button btnVehicleFront1stTime;

	private Button btnVehicle2ndTime;

	private Button btnTareReading2ndtime;

	private Button btnGrossReading1stTime;

	private Button btnVehicle2ndTimepreview;

	private Button btnVehicleFront1stTimePreview;

	private Button btnTareForntPicpreview;

	private Button btnGrossFontPIcPreview;

	private Button previewImageTareReading;

	private Button previewImageGrossReading;

	internal GridEX grdPendingForSecond;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label17;

	private TextBox txtNetWeight;

	private TabPage tabPage2;

	private Label label14;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label18;

	private Label label19;

	private Panel panel5;

	private Panel panel8;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton4;

	private Label label20;

	private Label label22;

	private Label label21;

	private SerialPort serialPort1;

	private TextBox txtrefdocNumberofMoveOrder;

	private Label label23;

	private TextBox txtContainerNo;

	private CheckBox ChkBox;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnLoadAll;

	private TextBox txtWorkingReportNo;

	private Label label24;

	private TextBox txtWeightDiffComments;

	private CheckBox ChkBoxPrintImages;

	private ToolStripButton btnWeighBridgeLookUp;

	private ToolStripButton btnRefresh;

	private Button btnCapture;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdPendingFirst;

	internal UltraCombo CmbWeighBridgeType;

	private Label label31;

	private CheckBox ChkMultiItem;

	private ToolStripButton BtnWeightDefine;

	internal UltraCombo cmbPackingType;

	private Label label25;

	private ToolStripButton BtnShortCutkeys;

	private GroupBox GroupInwardLabInfo;

	private Label labInwardfirstWtCompleteCount;

	private Label labInwardfirstWtTotalCount;

	private Label label27;

	private Label labInwardfirstWtPendingCount;

	private Label label29;

	private Label label28;

	private CheckBox chkPrintWithLab;

	private ToolStripButton btnSlip257;

	private ToolStripButton BtnPurchaseLab;

	internal UltraCombo CmbInvoiceNo;

	private Panel panel11;

	private Panel panel10;

	private Label label32;

	private Panel panel7;

	private GroupBox groupBox3;

	private Panel panel9;

	internal UltraCombo CmbWbTypeHistory;

	private Label label26;

	internal UltraCombo CmbGatePassTypeHistory;

	private Label lblLanguage;

	private Button btnshow;

	private TextBox GpNoTo;

	private Label label30;

	private TextBox GPNoFrom;

	private Label label33;

	private DateTimePicker ToDate;

	private Label label34;

	private Label label35;

	private TextBox TxtVehicleNumber;

	private Label label36;

	private TextBox txtTicketNoTo;

	private Label label37;

	private Label label38;

	private TextBox txtTicketNoFrom;

	private DateTimePicker FromDate;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Label labelSerialPortData;

	internal UltraCombo CmbPartyName;

	internal UltraCombo CmbItem;

	private CheckBox chkEnableWeights;

	public UltraCombo cmbRefDocNo;

	public UltraCombo cmbReferenceType;

	private TextBox txtWeightAfterPakingWeight;

	private Label label40;

	private TextBox txtEbUnit;

	private Label label6;

	private Label label16;

	private TextBox txtEbTotalWeight;

	private TextBox txtSupplierNetWeight;

	private TextBox txtSupplierSecondWeight;

	private TextBox txtSupplierFirstWeight;

	private Label label44;

	private Label label45;

	private Label label1;

	private GroupBox groupBox2;

	private PictureBox PicSecondReading2ndtime;

	private PictureBox PicSecondUpper;

	private PictureBox PicSecondVehicle2ndTime;

	private GroupBox groupBox1;

	private PictureBox PicFirstReading1stTime;

	private PictureBox picFirstUpper;

	private PictureBox PicVehicleFront1stTime;

	private GroupBox FilterBoxPendingFor2nd;

	private Button BtnResetPendingFor2nd;

	private Button BtnShowPendingFor2nd;

	private Label label3;

	private Label label41;

	private Label label42;

	private Label label43;

	private TextBox txtToGpNoPendingFor2nd;

	private TextBox txtFromGpNoPendingFor2nd;

	private DateTimePicker ToGpDatePendingFor2nd;

	private DateTimePicker FromGpDatePendingFor2nd;

	private Panel panel12;

	private GroupBox FilterBoxGpFor1st;

	private Button BtnResetPendingFor1st;

	private Button btnShowPendingFor1st;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox txtToGpNoPendingFor1st;

	private TextBox txtFromGpNoPendingFor1st;

	private DateTimePicker ToDatePendingFor1st;

	private DateTimePicker FromDatePendingFor1st;

	private Panel panel13;

	internal UltraCombo cmbVehicleNo;

	internal UltraCombo cmbVehicleType;

	private TextBox txtNetWtDifference;

	private TextBox txtSecondWtDifference;

	private Label label39;

	private TextBox txtFirstWtDifference;

	public frmWeightbridge(UserAccount user)
	{
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmWeightbridge_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1470, 935);
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
			if (dtParty.Columns.Count == 0)
			{
				dtParty.Columns.Add("Id", typeof(int));
				dtParty.Columns.Add("PartyName");
			}
			if (dtitem.Columns.Count == 0)
			{
				dtitem.Columns.Add("Id", typeof(int));
				dtitem.Columns.Add("ItemName");
				dtitem.Columns.Add("ItemCode");
				dtitem.Columns.Add("PartyId", typeof(int));
			}
			if (dtitemLookups.Columns.Count == 0)
			{
				dtitemLookups.Columns.Add("Id", typeof(int));
				dtitemLookups.Columns.Add("ItemName");
				dtitemLookups.Columns.Add("PartyId", typeof(int));
			}
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
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			WeighBridgeListdtFillDbCall();
			string ticketNoDbCall = WeightBridge_Helper.GetTicketNoDbCall(UserAccount);
			DataTable gatePassReferenceTypeData = CommonServices.GatePassReferenceTypes();
			WeightBridge_Helper.WbPartyandItemsDtFillDbCall(UserAccount, dtParty, dtitemLookups);
			DataTable wbTypeData = CommonServices.GatePassTypeFill("WeighBridge");
			DataTable all = VehicleType.GetAll();
			return new
			{
				GatePassReferenceTypeData = gatePassReferenceTypeData,
				documentNoData = ticketNoDbCall,
				WbTypeData = wbTypeData,
				VehicleTypeData = all
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
				Print.Enabled = formrights.DoHavePrintRights;
				btnUpdate.Enabled = formrights.DoHaveUpdateRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
				GetConfigurationsFromGlobal();
				if (Id == 0)
				{
					txtTickectNo.Text = result.documentNoData;
				}
				GatePassReferenceTypeBind(result.GatePassReferenceTypeData);
				VehicleTypesBind(result.VehicleTypeData);
				WbType(result.WbTypeData);
				ItemDtFillFromGlobal();
				PackingTypeBindFromGlobal();
				PartyBind();
				ItemBind();
				GetConfigurationsFromGlobalAndBindValuesInColumns();
				FromDate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDate.Value = DateTime.Now;
				FormInitialized = true;
				LoadCamera();
				cmbReferenceType.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void LoadCamera()
	{
		if (!string.IsNullOrWhiteSpace(CameraPixBox1Path))
		{
			HvCam01 = new HVUtility(this, PicFirstReading1stTime);
			HvCam02 = new HVUtility(this, picFirstUpper);
			HvCam03 = new HVUtility(this, PicVehicleFront1stTime);
			WeightBridge_Helper.CameraInitilization(HvCam01, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 01");
			WeightBridge_Helper.CameraInitilization(HvCam02, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 02");
			WeightBridge_Helper.CameraInitilization(HvCam03, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 03");
		}
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			CameraPixBox1Path = Conversion.ToString((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CameraPictureBox"));
			IsLabCompulsoryOnWBForMarketPur = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryForWeighBridgeAgainstMarketPurchase"));
			IsLabCompulsoryOnWBForGatePur = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryForWeighBridgeAgainstGatePurchase"));
			IsLabCompulsoryOnWBForRegularPur = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("LabCompulsoryForWeighBridge"));
			LabCompulsoryBeforFirstWeight = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Lab Compulsory Befor First Weight"));
			GroupInwardLabInfo.Visible = IsLabCompulsoryOnWBForRegularPur || IsLabCompulsoryOnWBForGatePur || IsLabCompulsoryOnWBForMarketPur;
			lstIsSavedVehicle = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Get Saved Vehicle Weight Against MoveOrder"));
			MultiWeightBridge = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("More Than 1 Weight By 1GpNoO Against"));
			WeighBridgeForOutwardPartyAndItemWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeighBridgeForOutwardPartyAndItemWise"));
			weighbridgeTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BillWeightAndStockWeightDifferenceTolerance"));
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				cmbPackingType.Value = packingTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			if (!string.IsNullOrEmpty(CameraPixBox1Path))
			{
				ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == "IpCameraAddress01");
				IpCam01 = ((obj == null) ? null : obj.ConfigKey?.ToString());
				Port01 = Convert.ToInt16(GlobalVariables_Helper.GetConfigValueFromGlobal("PortNo01"));
				UserNameCam01 = GlobalVariables_Helper.GetConfigValueFromGlobal("UserNameCamer01");
				PasswordCam01 = GlobalVariables_Helper.GetConfigValueFromGlobal("PasswordCamer01");
				IpCam02 = GlobalVariables_Helper.GetConfigValueFromGlobal("IpCameraAddress02");
				Port02 = Convert.ToInt16(Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PortNo02")));
				UserNameCam02 = GlobalVariables_Helper.GetConfigValueFromGlobal("UserNameCamer02");
				PasswordCam02 = GlobalVariables_Helper.GetConfigValueFromGlobal("PasswordCamer02");
				IpCam03 = GlobalVariables_Helper.GetConfigValueFromGlobal("IpCameraAddress03");
				Port03 = Convert.ToInt16(Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PortNo03")));
				UserNameCam03 = GlobalVariables_Helper.GetConfigValueFromGlobal("UserNameCamer03");
				PasswordCam03 = GlobalVariables_Helper.GetConfigValueFromGlobal("PasswordCamer03");
			}
			int packingTypeId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Paking Type"));
			if (packingTypeId != 0)
			{
				cmbPackingType.Value = packingTypeId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void GatePassReferenceTypeBind(DataTable dtGatePassRefence)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbReferenceType, dtGatePassRefence, "Id", "Name", "Reference Name", AllColumns: true, Conversion.ToInt(cmbReferenceType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbGatePassTypeHistory, dtGatePassRefence, "Id", "Name", "Reference Name", AllColumns: true, Conversion.ToInt(CmbGatePassTypeHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpNoBind(DataTable dt, bool HideColumnFlag)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbRefDocNo, dt, "Id", "GpNo", "GpNo", AllColumns: true, Conversion.ToInt(cmbRefDocNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt.Rows.Count > 0 && HideColumnFlag)
			{
				((UltraGridBase)cmbRefDocNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbRefDocNo).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleTypesBind(DataTable dtVehicle)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbVehicleType, dtVehicle, "Id", "VehicleDescription", "VehicleDescription", AllColumns: true, Conversion.ToInt(cmbVehicleType.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtFillFromGlobal()
	{
		try
		{
			dtitem.Clear();
			List<getGlobalAllItems> dtItemAll = clsGlobalVariables.getGlobalAllItems.ToList();
			if (dtItemAll == null || dtItemAll.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllItems row in dtItemAll)
			{
				dtitem.Rows.Add(row.Id, row.ItemName, row.ItemCode, 0);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyBind(bool limittolist = false)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbPartyName, dtParty, "Id", "PartyName", "PartyName", AllColumns: true, Conversion.ToInt(CmbPartyName.Value), insertDefaultRow: true);
			if (dtParty.Rows.Count > 0)
			{
				CmbPartyName.LimitToList = limittolist;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemLookupsBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbItem, dtitemLookups, "Id", "ItemName", "ItemName", AllColumns: true, Conversion.ToInt(CmbItem.Value), insertDefaultRow: true);
			if (dtitemLookups.Rows.Count > 0)
			{
				CmbItem.LimitToList = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbItem, dtitem, "Id", "ItemName", "ItemName", AllColumns: true, Conversion.ToInt(CmbItem.Value), insertDefaultRow: true);
			if (dtitem.Rows.Count > 0)
			{
				CmbItem.LimitToList = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyNameBindforOutward()
	{
		try
		{
			Conversion.ToInt(CmbPartyName.Value);
			if (dtPartiesAndItemsFromDeliveryOrder != null && dtPartiesAndItemsFromDeliveryOrder.Rows.Count > 0)
			{
				dtParty.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow r in dtPartiesAndItemsFromDeliveryOrder.Rows)
				{
					int id = Conversion.ToInt(r["SupplierCustomerId"]);
					if (!existingIds.Contains(id))
					{
						dtParty.Rows.Add(r["SupplierCustomerId"], r["CustomerName"]);
						existingIds.Add(id);
					}
				}
			}
			PartyBind(limittolist: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassOutward(bool value)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbRefDocNo).Enabled = true;
			if (!(((Control)(object)cmbReferenceType).Text == "GatePass OutWard"))
			{
				return;
			}
			DataTable dtout = GatePassInward.GetRefDocTypeIdbyRefDocNoId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				RefDocumentTypeId = 91,
				DocumentTypeId = 102,
				CheckConfig = value,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId,
				RecId = RECID
			});
			if (dtout.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GP");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("GatepassType");
				for (int i = 0; i < dtout.Rows.Count; i++)
				{
					dt.Rows.Add(dtout.Rows[i]["Id"], dtout.Rows[i]["GpSrNo"], dtout.Rows[i]["VehicleNo"], dtout.Rows[i]["GatepassType"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbRefDocNo, "Id", "GP", "GatePass  ", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)cmbRefDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassInwardForWeighBridge_Engr()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbRefDocNo).Enabled = true;
			DataTable dt = InvLabAnalysisPurchaseHeader.GetGatePassIdandNoforWeighBridge_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, cmbRefDocNo, "Id", "GpSrNo", "GpNo  ", ZeroIndex: false);
				return;
			}
			((Control)(object)cmbRefDocNo).Text = string.Empty;
			((UltraGridBase)cmbRefDocNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocNoGatePassOutward()
	{
		try
		{
			List<ConfigrationsAllocation> lstOut = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "More Than 1 Weight By 1GpNoO Against").ToList();
			if (lstOut.Count > 0)
			{
				bool value = bool.Parse(lstOut[0].ConfigKey.ToString());
				GetGatePassOutward(value);
			}
			else
			{
				GetGatePassOutward(value: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocNoGatePassGeneral()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbRefDocNo).Enabled = true;
			if (!(((Control)(object)cmbReferenceType).Text == "General GatePass Inward"))
			{
				return;
			}
			DataTable dtout = GatePassInward.GetForWeighBridgeGatePassGeneral(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 52,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtout.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpSrNo");
				for (int i = 0; i < dtout.Rows.Count; i++)
				{
					dt.Rows.Add(dtout.Rows[i]["Id"], dtout.Rows[i]["GpSrNo"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbRefDocNo, "Id", "GpSrNo", "GatePassNo", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)cmbRefDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocNoGatePassGeneralOutward()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbRefDocNo).Enabled = true;
			if (!(((Control)(object)cmbReferenceType).Text == "General GatePass Outward"))
			{
				return;
			}
			DataTable dtout = GatePassInward.GetForWeighBridgeGatePassGeneral(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 92,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtout.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpSrNo");
				for (int i = 0; i < dtout.Rows.Count; i++)
				{
					dt.Rows.Add(dtout.Rows[i]["Id"], dtout.Rows[i]["GpSrNo"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbRefDocNo, "Id", "GpSrNo", "GatePassNo", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)cmbRefDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassPartyProcessing()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbRefDocNo).Enabled = true;
			DataTable dtout = GatePassPartyProcessing.GetGatePassPartyProcessingForWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(cmbReferenceType.Value),
				WeighBridgeStatus = "Auto"
			});
			if (dtout.Rows.Count > 0)
			{
				DDL.BindDDL(dtout, cmbRefDocNo, "Id", "GpSrNo", "GatePassNo", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbRefDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeighBridgeListdtFillDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			WeighbridgeScalesList obj = new WeighbridgeScalesList();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.MacAddress = "";
			dtWbList = WeighbridgeScalesList.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeBindFromGlobal()
	{
		try
		{
			int Id = Conversion.ToInt(cmbPackingType.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalInvPackingType, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			InfragisticsHelper.BindAndRetainSelection(cmbPackingType, dt, "Id", "Description", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WbType(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbWeighBridgeType, dt, "Id", "GpTypeDescription", "WeighBridge Type", AllColumns: true, Conversion.ToInt(CmbWeighBridgeType.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbWbTypeHistory, dt, "Id", "GpTypeDescription", "WeighBridge Type", AllColumns: true, Conversion.ToInt(CmbWbTypeHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemsAgainstPartyForOutward(int PartyId)
	{
		try
		{
			int ID = Conversion.ToInt(CmbItem.Value);
			if (dtPartiesAndItemsFromDeliveryOrder != null && dtPartiesAndItemsFromDeliveryOrder.Rows.Count > 0)
			{
				dtitem.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow r in dtPartiesAndItemsFromDeliveryOrder.Rows)
				{
					int id = Conversion.ToInt(r["ItemId"]);
					int SupplierCustomerId = Conversion.ToInt(r["SupplierCustomerId"]);
					if (!existingIds.Contains(id) && SupplierCustomerId == PartyId)
					{
						dtitem.Rows.Add(r["ItemId"], r["ItemName"], r["ItemCode"], SupplierCustomerId);
						existingIds.Add(id);
					}
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "ItemName", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItem.Value = ID;
					}
					else
					{
						((Control)(object)CmbItem).Text = string.Empty;
					}
				}
				CmbItem.LimitToList = true;
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemsFromDeliveryOrder(int DeliveryOrderId = 0, int GpId = 0)
	{
		try
		{
			int ID = Conversion.ToInt(CmbItem.Value);
			DataTable dt = InvDeliveryOrder.GetItemsByDeliveryOrderId(UserAccount.OrganizationId, UserAccount.CompanyId, DeliveryOrderId, GpId);
			if (dt.Rows.Count > 0)
			{
				dtitem.Clear();
				foreach (DataRow r in dt.Rows)
				{
					dtitem.Rows.Add(r["Id"], r["ItemName"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "ItemName", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItem.Value = ID;
					}
					else
					{
						((Control)(object)CmbItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemsFromSaleOrder(int SaleOrderId)
	{
		try
		{
			int ID = Conversion.ToInt(CmbItem.Value);
			DataTable dt = SaleOrder.GetItemsBySaleOrderId(UserAccount.OrganizationId, UserAccount.CompanyId, SaleOrderId);
			if (dt.Rows.Count > 0)
			{
				dtitem.Clear();
				foreach (DataRow r in dt.Rows)
				{
					dtitem.Rows.Add(r["Id"], r["ItemName"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "ItemName", ZeroIndex: true);
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbItem.Value = ID;
					}
					else
					{
						((Control)(object)CmbItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable LabItemsBindByGpId(int ActionId = 0)
	{
		DataTable dtLabInfo = new DataTable();
		try
		{
			dtLabInfo = GatePassInward.getItemsFromLabOrPurchaseOrderOrAllByGpId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbRefDocNo.Value), ActionId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dtLabInfo;
	}

	private void GetSavedVehicles()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbVehicleNo).Enabled = true;
			((Control)(object)cmbVehicleNo).Visible = true;
			txtVehicleNo.Enabled = false;
			txtVehicleNo.Visible = false;
			DataTable dVehiclet = GatePassPartyProcessing.GetVehicleWeight(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dVehiclet.Rows.Count > 0)
			{
				DDL.BindDDL(dVehiclet, cmbVehicleNo, "Id", "VehicleNo", "Vehicle No", ZeroIndex: false);
				((UltraGridBase)cmbVehicleNo).DisplayLayout.Bands[0].Columns[0].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void formRefresh()
	{
		try
		{
			RECID = 0;
			SupplierFirstWeight = 0.0;
			SupplierSecondWeight = 0.0;
			Approved = false;
			txtFirstWeight.Enabled = false;
			txtFirstWeight.ReadOnly = true;
			txtsecondWeight.Enabled = false;
			txtsecondWeight.ReadOnly = true;
			chkEnableWeights.Visible = false;
			UniqFileName = string.Empty;
			UniqFileName2 = string.Empty;
			UniqFileName3 = string.Empty;
			UniqFileName4 = string.Empty;
			UniqFileName5 = string.Empty;
			UniqFileName6 = string.Empty;
			txtWeightReading.Text = "0";
			txtsecondWeight.Text = string.Empty;
			txtsecondWeight.Enabled = false;
			txtTickectNo.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			txtWorkingReportNo.Text = string.Empty;
			txtWeightDiffComments.Text = string.Empty;
			((Control)(object)CmbWeighBridgeType).Text = string.Empty;
			txtWorkingReportNo.Enabled = false;
			txtFirstWeight.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtItemqty.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtWbCharges.Text = string.Empty;
			((Control)(object)CmbPartyName).Text = string.Empty;
			txtContainerNo.Text = string.Empty;
			PicFirstReading1stTime.Image = null;
			PicFirstReading1stTime.Invalidate();
			PicFirstReading1stTime.Tag = "";
			picFirstUpper.Image = null;
			picFirstUpper.Invalidate();
			picFirstUpper.Tag = "";
			PicVehicleFront1stTime.Image = null;
			PicVehicleFront1stTime.Invalidate();
			PicVehicleFront1stTime.Tag = "";
			PicSecondReading2ndtime.Image = null;
			PicSecondReading2ndtime.Invalidate();
			PicSecondReading2ndtime.Tag = "";
			PicSecondUpper.Image = null;
			PicSecondUpper.Invalidate();
			PicSecondUpper.Tag = "";
			PicSecondVehicle2ndTime.Image = null;
			PicSecondVehicle2ndTime.Invalidate();
			PicSecondVehicle2ndTime.Tag = "";
			txtNetWeight.Text = string.Empty;
			txtrefdocNumberofMoveOrder.Text = string.Empty;
			txtrefdocNumberofMoveOrder.Visible = false;
			txtEbUnit.Text = string.Empty;
			txtEbTotalWeight.Text = string.Empty;
			txtWeightAfterPakingWeight.Text = string.Empty;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNo).DataSource = null;
			((Control)(object)cmbRefDocNo).Text = string.Empty;
			((UltraGridBase)cmbRefDocNo).DataSource = null;
			((Control)(object)cmbRefDocNo).Visible = true;
			EnableTrueFileds();
			((Control)(object)cmbVehicleNo).Enabled = false;
			((Control)(object)cmbVehicleNo).Visible = false;
			txtVehicleNo.Enabled = true;
			txtVehicleNo.Visible = true;
			ChkMultiItem.Visible = false;
			ChkMultiItem.Checked = false;
			txtSecondDate.Value = DateTime.Now;
			txtFirstDate.Value = DateTime.Now;
			txtsecondWeight.Enabled = false;
			txtSecondDate.Enabled = false;
			txtFirstDate.Enabled = false;
			txtBilityNo.Enabled = true;
			txtVehicleNo.Enabled = true;
			fileSavePath = string.Empty;
			fileSavePath2 = string.Empty;
			fileSavePath3 = string.Empty;
			fileSavePath4 = string.Empty;
			fileSavePath5 = string.Empty;
			fileSavePath6 = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbReferenceType.Focus();
			((Control)(object)cmbReferenceType).Text = string.Empty;
			txtSupplierFirstWeight.Text = string.Empty;
			txtSupplierSecondWeight.Text = string.Empty;
			txtSupplierNetWeight.Text = string.Empty;
			UpdateMode = false;
			((Control)(object)cmbReferenceType).Enabled = true;
			WeightBridge_Helper.WbPartyandItemsDtFillDbCall(UserAccount, dtParty, dtitemLookups);
			ItemBind();
			PartyBind();
			GroupInwardLabInfo.Visible = IsLabCompulsoryOnWBForRegularPur || IsLabCompulsoryOnWBForGatePur || IsLabCompulsoryOnWBForMarketPur;
			txtTickectNo.Text = WeightBridge_Helper.GetTicketNoDbCall(UserAccount);
			if (!string.IsNullOrWhiteSpace(CameraPixBox1Path))
			{
				WeightBridge_Helper.CameraDisconnect(HvCam01);
				WeightBridge_Helper.CameraDisconnect(HvCam02);
				WeightBridge_Helper.CameraDisconnect(HvCam03);
				LoadCamera();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DisableFileds()
	{
		try
		{
			((Control)(object)cmbVehicleType).Enabled = false;
			txtVehicleNo.Enabled = false;
			txtBilityNo.Enabled = false;
			((Control)(object)CmbPartyName).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnableTrueFileds()
	{
		try
		{
			((Control)(object)cmbVehicleType).Enabled = true;
			txtVehicleNo.Enabled = true;
			txtBilityNo.Enabled = true;
			((Control)(object)CmbItem).Enabled = true;
			txtItemqty.Enabled = true;
			((Control)(object)CmbPartyName).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		try
		{
			int referenceTypeValue = Conversion.ToInt(cmbReferenceType.Value);
			if (((UltraGridBase)cmbReferenceType).ActiveRow == null)
			{
				MessageBox.Show("ReferenceType Field is Required");
				cmbReferenceType.Focus();
				return false;
			}
			if (referenceTypeValue == 51)
			{
				double num = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
				double tareWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
				double SupplierNet = Conversion.ToDouble((object)txtSupplierNetWeight.Text);
				if (num == 0.0)
				{
					MessageBox.Show("Supplier First Weight Field is Required");
					txtSupplierFirstWeight.Focus();
					return false;
				}
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					if (tareWeight == 0.0)
					{
						MessageBox.Show("Supplier Second Weight Field is Required");
						txtSupplierSecondWeight.Focus();
						return false;
					}
					if (SupplierNet == 0.0)
					{
						MessageBox.Show("Supplier Net Weight Field is Required");
						txtSupplierNetWeight.Focus();
						return false;
					}
				}
			}
			if ((((Control)(object)cmbReferenceType).Text == "Move Order" || ((Control)(object)cmbReferenceType).Text == "General") && (txtrefdocNumberofMoveOrder.Text.Trim() == string.Empty || txtrefdocNumberofMoveOrder.Text.Trim() == "0"))
			{
				MessageBox.Show("GatePass No Field is Required");
				cmbRefDocNo.Focus();
				return false;
			}
			if (((Control)(object)cmbReferenceType).Text != "Move Order" && ((Control)(object)cmbReferenceType).Text != "General" && ((UltraGridBase)cmbRefDocNo).ActiveRow == null)
			{
				MessageBox.Show("GatePass No Field is Required");
				cmbRefDocNo.Focus();
				return false;
			}
			if (((UltraGridBase)CmbWeighBridgeType).ActiveRow == null)
			{
				MessageBox.Show("WeighBridge Type Field is Required");
				CmbWeighBridgeType.Focus();
				return false;
			}
			if (Conversion.ToInt(cmbReferenceType.Value) == 91 && ((UltraGridBase)cmbRefDocNo).ActiveRow != null && Conversion.ToInt(cmbRefDocNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) == "Export" && (((UltraGridBase)CmbInvoiceNo).ActiveRow == null || Conversion.ToInt(CmbInvoiceNo.Value) == 0))
			{
				MessageBox.Show("Invoice number Field is Required");
				CmbInvoiceNo.Focus();
				return false;
			}
			if ((Conversion.ToInt(cmbReferenceType.Value) == 74 || Conversion.ToInt(cmbReferenceType.Value) == 75) && (((UltraGridBase)cmbPackingType).ActiveRow == null || Conversion.ToInt(cmbPackingType.Value) == 0))
			{
				MessageBox.Show("PackingType Field is Required");
				cmbPackingType.Focus();
				return false;
			}
			if (btnUpdate.Visible && btnUpdate.Enabled)
			{
				if (txtsecondWeight.Text.Trim() == string.Empty || txtsecondWeight.Text.Trim() == null || Conversion.ToDouble((object)txtsecondWeight.Text.Trim()) == 0.0)
				{
					MessageBox.Show("Second Weight  Field is Required");
					txtsecondWeight.Focus();
					return false;
				}
				if (((Control)(object)cmbReferenceType).Text == "Move Order" || ((Control)(object)cmbReferenceType).Text == "General GatePass Inward" || ((Control)(object)cmbReferenceType).Text == "General GatePass Outward")
				{
					if (txtWorkingReportNo.Text.Trim() == string.Empty || txtWorkingReportNo.Text.Trim() == "0")
					{
						txtWorkingReportNo.Enabled = true;
						MessageBox.Show("WorkingReportNo Field is Required");
						txtWorkingReportNo.Focus();
						return false;
					}
				}
				else if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard" && (txtWorkingReportNo.Text.Trim() == string.Empty || txtWorkingReportNo.Text.Trim() == "0"))
				{
					txtWorkingReportNo.Enabled = true;
					if (GatePassOutward.GetDeliveryOrderType(new GatePassOutward
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = Conversion.ToInt(cmbRefDocNo.Value)
					}) == "StockTransfer")
					{
						MessageBox.Show("WorkingReportNo Field is Required");
						txtWorkingReportNo.Focus();
						return false;
					}
				}
			}
			if (btnsave.Visible && btnsave.Enabled && (txtFirstWeight.Text.Trim() == string.Empty || txtFirstWeight.Text.Trim() == null || Conversion.ToDouble((object)txtFirstWeight.Text.Trim()) == 0.0))
			{
				MessageBox.Show("First Weight  Field is Required");
				txtFirstWeight.Focus();
				return false;
			}
			if (Conversion.ToInt(cmbReferenceType.Value) == 74 && lstIsSavedVehicle)
			{
				if (((Control)(object)cmbVehicleNo).Text.Trim() == "")
				{
					MessageBox.Show("Vehicle Number Field is Required");
					txtVehicleNo.Focus();
					return false;
				}
			}
			else if (txtVehicleNo.Text.Trim() == "")
			{
				MessageBox.Show("Vehicle Number Field is Required");
				txtVehicleNo.Focus();
				return false;
			}
			if (txtTickectNo.Text.Trim() == string.Empty || txtTickectNo.Text.Trim() == "0")
			{
				MessageBox.Show("Ticket No Field is Required");
				txtTickectNo.Focus();
				return false;
			}
			if (WeighBridgeForOutwardPartyAndItemWise && ((Control)(object)cmbReferenceType).Text == "GatePass OutWard" && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) != "Export")
			{
				if (((UltraGridBase)CmbPartyName).ActiveRow == null || Conversion.ToInt(CmbPartyName.Value) == 0)
				{
					MessageBox.Show("PartyName Field is Required");
					CmbPartyName.Focus();
					return false;
				}
				if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
				{
					MessageBox.Show("ItemName Field is Required");
					CmbItem.Focus();
					return false;
				}
				if (Conversion.ToInt(CmbPartyName.Value) != Conversion.ToInt(((UltraDropDownBase)CmbItem).SelectedRow.Cells["PartyId"].Value))
				{
					((Control)(object)CmbPartyName).Text = "";
					MessageBox.Show("PartyName Field is Required");
					CmbPartyName.Focus();
					return false;
				}
			}
			if (txtRemarks.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Remarks Field is Required");
				txtRemarks.Focus();
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShowPendingFor1st_Click(object sender, EventArgs e)
	{
		try
		{
			BindPendingFor1st();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetPendingFor1st_Click(object sender, EventArgs e)
	{
		try
		{
			FromDatePendingFor1st.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDatePendingFor1st.Value = DateTime.Now;
			txtFromGpNoPendingFor1st.Text = string.Empty;
			txtToGpNoPendingFor1st.Text = string.Empty;
			grdPendingFirst.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindPendingFor1st()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			if (FromDatePendingFor1st.Checked)
			{
				obj.GpDateF = FromDatePendingFor1st.Value;
			}
			if (ToDatePendingFor1st.Checked)
			{
				obj.GpDateT = ToDatePendingFor1st.Value;
			}
			obj.GpSrNoF = Conversion.ToInt((object)txtFromGpNoPendingFor1st.Text);
			obj.GpSrNoT = Conversion.ToInt((object)txtToGpNoPendingFor1st.Text);
			DataTable dt = WbTransactions.GatepassPendingForWeighBridge(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("DocumentTypeId", typeof(int));
				dtTarget.Columns.Add("LabStatus", typeof(string));
				dtTarget.Columns.Add("LabApprovalStatus", typeof(string));
				dtTarget.Columns.Add("DocumentType", typeof(string));
				dtTarget.Columns.Add("GpDate", typeof(DateTime));
				dtTarget.Columns.Add("GpSrNo", typeof(int));
				dtTarget.Columns.Add("PartyName", typeof(string));
				dtTarget.Columns.Add("ItemName", typeof(string));
				dtTarget.Columns.Add("ItemQty", typeof(double));
				dtTarget.Columns.Add("VehicleType", typeof(string));
				dtTarget.Columns.Add("VehicleNo", typeof(string));
				dtTarget.Columns.Add("BiltyNo", typeof(string));
				dtTarget.Columns.Add("Remarks", typeof(string));
				dtTarget.Columns.Add("GpStatus", typeof(string));
				dtTarget.Columns.Add("EntryUser", typeof(string));
				dtTarget.Columns.Add("GpApprovalStatus", typeof(string));
				dtTarget.Columns.Add("AccessWeight", typeof(double));
				foreach (DataRow row in dt.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["DocumentTypeId"], row["LabStatus"], row["LabApprovalStatus"], row["DocumentType"], row["GpDate"], row["GpSrNo"], row["PartyName"], row["ItemName"], row["ItemQty"], row["VehicleType"], row["VehicleNo"], row["BiltyNo"], row["Remarks"], row["GpStatus"], row["UserName"], row["GpApprovalStatus"], row["AccessWeight"]);
				}
				grdPendingFirst.DataSource = dtTarget;
				grdPendingFirst.RetrieveStructure();
				grdPendingFirst.RootTable.Columns["Id"].Visible = false;
				grdPendingFirst.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdPendingFirst.RootTable.Columns["EntryUser"].Visible = false;
				grdPendingFirst.RootTable.Columns["GpSrNo"].Caption = "Gp No";
				grdPendingFirst.RootTable.Columns["DocumentType"].Width = 150;
				grdPendingFirst.RootTable.Columns["GpDate"].Width = 80;
				grdPendingFirst.RootTable.Columns["GpSrNo"].Width = 60;
				grdPendingFirst.RootTable.Columns["PartyName"].Width = 180;
				grdPendingFirst.RootTable.Columns["ItemName"].Width = 180;
				grdPendingFirst.RootTable.Columns["ItemQty"].Width = 70;
				grdPendingFirst.RootTable.Columns["VehicleType"].Width = 85;
				grdPendingFirst.RootTable.Columns["VehicleNo"].Width = 80;
				grdPendingFirst.RootTable.Columns["BiltyNo"].Width = 80;
				grdPendingFirst.RootTable.Columns["Remarks"].Width = 150;
				grdPendingFirst.RootTable.Columns["GpStatus"].Width = 80;
				grdPendingFirst.RootTable.Columns["EntryUser"].Width = 110;
				grdPendingFirst.RootTable.Columns["LabStatus"].Width = 80;
				grdPendingFirst.RootTable.Columns["LabApprovalStatus"].Width = 130;
				grdPendingFirst.RootTable.Columns["AccessWeight"].Width = 80;
				grdPendingFirst.RootTable.Columns["GpApprovalStatus"].Width = 100;
				grdPendingFirst.RootTable.HeaderLines = 3;
				foreach (GridEXColumn item in (JanusCollectionBase)grdPendingFirst.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdPendingFirst.RootTable.Columns["AccessWeight"].AggregateFunction = (AggregateFunction)2;
				grdPendingFirst.RootTable.Columns["AccessWeight"].TextAlignment = (TextAlignment)3;
				grdPendingFirst.RootTable.Columns["AccessWeight"].FormatString = "#,##0.###";
				grdPendingFirst.RootTable.Columns["AccessWeight"].TotalFormatString = "#,##0.###";
				grdPendingFirst.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
				grdPendingFirst.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
				grdPendingFirst.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
				grdPendingFirst.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
				grdPendingFirst.AllowDelete = (InheritableBoolean)2;
				grdPendingFirst.AllowEdit = (InheritableBoolean)2;
				grdPendingFirst.ColumnAutoResize = false;
				InwardLabStatusCountCalculate();
			}
			else
			{
				grdPendingFirst.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingGpForWeighBridge_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		try
		{
			if (grdPendingFirst.RowCount <= 0)
			{
				return;
			}
			GridEXColumn LabStatus = grdPendingFirst.RootTable.Columns["LabStatus"];
			GridEXColumn LabApprovalStatus = grdPendingFirst.RootTable.Columns["LabApprovalStatus"];
			GridEXColumn AccessWeight = grdPendingFirst.RootTable.Columns["AccessWeight"];
			GridEXColumn ApprovalStatus = grdPendingFirst.RootTable.Columns["GpApprovalStatus"];
			if (LabStatus != null)
			{
				GridEXCell LabStatusCell = e.Row.Cells[((JanusCollectionElement)LabStatus).Index];
				if (Conversion.ToString(LabStatusCell.Value) == "Pending")
				{
					LabStatusCell.FormatStyle = new GridEXFormatStyle();
					LabStatusCell.FormatStyle.ForeColor = Color.Red;
				}
				else if (Conversion.ToString(LabStatusCell.Value) == "Accepted")
				{
					LabStatusCell.FormatStyle = new GridEXFormatStyle();
					LabStatusCell.FormatStyle.ForeColor = Color.Green;
				}
			}
			if (LabApprovalStatus != null)
			{
				GridEXCell ApprovalStatusCell2 = e.Row.Cells[((JanusCollectionElement)LabApprovalStatus).Index];
				if (Conversion.ToString(ApprovalStatusCell2.Value) == "Not Approved")
				{
					ApprovalStatusCell2.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell2.FormatStyle.ForeColor = Color.Red;
				}
				else if (Conversion.ToString(ApprovalStatusCell2.Value) == "Approved")
				{
					ApprovalStatusCell2.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell2.FormatStyle.ForeColor = Color.Green;
				}
			}
			if (AccessWeight != null)
			{
				GridEXCell AccessWeightCell = e.Row.Cells[((JanusCollectionElement)AccessWeight).Index];
				if (Conversion.ToDouble(AccessWeightCell.Value) == 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Green;
				}
				else if (Conversion.ToDouble(AccessWeightCell.Value) > 0.0)
				{
					AccessWeightCell.FormatStyle = new GridEXFormatStyle();
					AccessWeightCell.FormatStyle.ForeColor = Color.Red;
				}
			}
			if (ApprovalStatus != null)
			{
				GridEXCell ApprovalStatusCell = e.Row.Cells[((JanusCollectionElement)ApprovalStatus).Index];
				if (Conversion.ToString(ApprovalStatusCell.Value) == "Not Approved")
				{
					ApprovalStatusCell.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell.FormatStyle.ForeColor = Color.Red;
				}
				else if (Conversion.ToString(ApprovalStatusCell.Value) == "Approved")
				{
					ApprovalStatusCell.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell.FormatStyle.ForeColor = Color.Green;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InwardLabStatusCountCalculate()
	{
		try
		{
			if (grdPendingFirst.RowCount > 0)
			{
				int TotalCount = 0;
				int CompleteCount = 0;
				int PendingCount = 0;
				PendingCount = (from GridEXRow row in grdPendingFirst.GetRows()
					where Conversion.ToString(row.Cells["LabStatus"].Value) == "Pending" && Conversion.ToInt(row.Cells["DocumentTypeId"].Value) == 51
					select row).Count();
				CompleteCount = (from GridEXRow row in grdPendingFirst.GetRows()
					where Conversion.ToString(row.Cells["LabStatus"].Value) == "Accepted" && Conversion.ToInt(row.Cells["DocumentTypeId"].Value) == 51
					select row).Count();
				TotalCount = (from GridEXRow row in grdPendingFirst.GetRows()
					where Conversion.ToInt(row.Cells["DocumentTypeId"].Value) == 51
					select row).Count();
				labInwardfirstWtTotalCount.Text = Conversion.ToString((object)TotalCount);
				labInwardfirstWtCompleteCount.Text = Conversion.ToString((object)CompleteCount);
				labInwardfirstWtPendingCount.Text = Conversion.ToString((object)PendingCount);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowPendingFor2nd_Click(object sender, EventArgs e)
	{
		try
		{
			bindGridPendingForSecond();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetPendingFor2nd_Click(object sender, EventArgs e)
	{
		try
		{
			FromGpDatePendingFor2nd.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToGpDatePendingFor2nd.Value = DateTime.Now;
			txtToGpNoPendingFor2nd.Text = string.Empty;
			txtFromGpNoPendingFor2nd.Text = string.Empty;
			grdPendingForSecond.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindGridPendingForSecond()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FirstWeighBridgeId = Conversion.ToInt(dtWbList.Rows[0]["Id"]);
			if (FromGpDatePendingFor2nd.Checked)
			{
				obj.GpDateF = FromGpDatePendingFor2nd.Value;
			}
			if (ToGpDatePendingFor2nd.Checked)
			{
				obj.GpDateT = ToGpDatePendingFor2nd.Value;
			}
			obj.GpSrNoF = Conversion.ToInt((object)txtFromGpNoPendingFor2nd.Text);
			obj.GpSrNoT = Conversion.ToInt((object)txtToGpNoPendingFor2nd.Text);
			DataTable dt = WbTransactions.PendingWeighBridgeForSecondWeight(obj);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocTypeId", typeof(int));
				dtcol.Columns.Add("ReferenceDocTypeId", typeof(int));
				dtcol.Columns.Add("LabStatus", typeof(string));
				dtcol.Columns.Add("LabApprovalStatus", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocumentType", typeof(string));
				dtcol.Columns.Add("TicketNo", typeof(int));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("PartyName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(double));
				dtcol.Columns.Add("WbCharges", typeof(double));
				dtcol.Columns.Add("WbRemarks", typeof(string));
				dtcol.Columns.Add("FirstWeight", typeof(double));
				dtcol.Columns.Add("FirstWtWbStatus", typeof(string));
				dtcol.Columns.Add("SecondWeight", typeof(double));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocTypeId"], dt.Rows[i]["ReferenceDocTypeId"], dt.Rows[i]["LabStatus"], dt.Rows[i]["LabApprovalStatus"], dt.Rows[i]["DocDate"], dt.Rows[i]["DocumentTypeDescription"], dt.Rows[i]["TicketNo"], dt.Rows[i]["GpSrNo"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["PartyName"], dt.Rows[i]["ItemDescription"], dt.Rows[i]["ItemQty"], dt.Rows[i]["WbCharges"], dt.Rows[i]["WbRemarks"], dt.Rows[i]["FirstWeight"], dt.Rows[i]["FirstWtWbStatus"], dt.Rows[i]["SecondWeight"], dt.Rows[i]["UserName"]);
				}
				grdPendingForSecond.DataSource = dtcol;
				grdPendingForSecond.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdPendingForSecond.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfill_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		try
		{
			if (grdPendingForSecond.RowCount <= 0)
			{
				return;
			}
			GridEXColumn LabStatus = grdPendingForSecond.RootTable.Columns["LabStatus"];
			GridEXColumn LabApprovalStatus = grdPendingForSecond.RootTable.Columns["LabApprovalStatus"];
			if (LabStatus != null)
			{
				GridEXCell LabStatusCell = e.Row.Cells[((JanusCollectionElement)LabStatus).Index];
				if (Conversion.ToString(LabStatusCell.Value) == "Pending")
				{
					LabStatusCell.FormatStyle = new GridEXFormatStyle();
					LabStatusCell.FormatStyle.ForeColor = Color.Red;
				}
				else if (Conversion.ToString(LabStatusCell.Value) == "Accepted")
				{
					LabStatusCell.FormatStyle = new GridEXFormatStyle();
					LabStatusCell.FormatStyle.ForeColor = Color.Green;
				}
			}
			if (LabApprovalStatus != null)
			{
				GridEXCell ApprovalStatusCell = e.Row.Cells[((JanusCollectionElement)LabApprovalStatus).Index];
				if (Conversion.ToString(ApprovalStatusCell.Value) == "Not Approved")
				{
					ApprovalStatusCell.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell.FormatStyle.ForeColor = Color.Red;
				}
				else if (Conversion.ToString(ApprovalStatusCell.Value) == "Approved")
				{
					ApprovalStatusCell.FormatStyle = new GridEXFormatStyle();
					ApprovalStatusCell.FormatStyle.ForeColor = Color.Green;
				}
			}
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
			grdPendingForSecond.RootTable.Columns["DocDate"].Width = 80;
			grdPendingForSecond.RootTable.Columns["DocumentType"].Width = 150;
			grdPendingForSecond.RootTable.Columns["TicketNo"].Width = 65;
			grdPendingForSecond.RootTable.Columns["GpSrNo"].Width = 70;
			grdPendingForSecond.RootTable.Columns["VehicleNo"].Width = 80;
			grdPendingForSecond.RootTable.Columns["BiltyNo"].Width = 70;
			grdPendingForSecond.RootTable.Columns["PartyName"].Width = 150;
			grdPendingForSecond.RootTable.Columns["ItemName"].Width = 140;
			grdPendingForSecond.RootTable.Columns["ItemQty"].Width = 70;
			grdPendingForSecond.RootTable.Columns["WbCharges"].Width = 85;
			grdPendingForSecond.RootTable.Columns["WbRemarks"].Width = 150;
			grdPendingForSecond.RootTable.Columns["FirstWeight"].Width = 90;
			grdPendingForSecond.RootTable.Columns["FirstWtWbStatus"].Width = 125;
			grdPendingForSecond.RootTable.Columns["SecondWeight"].Width = 100;
			grdPendingForSecond.RootTable.Columns["EntryUser"].Width = 150;
			grdPendingForSecond.RootTable.Columns["LabStatus"].Width = 90;
			grdPendingForSecond.RootTable.Columns["LabApprovalStatus"].Width = 130;
			grdPendingForSecond.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdPendingForSecond.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdPendingForSecond.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdPendingForSecond.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdPendingForSecond.RootTable.Columns["WbCharges"].TextAlignment = (TextAlignment)3;
			grdPendingForSecond.RootTable.Columns["WbCharges"].FormatString = "#,##0.###";
			grdPendingForSecond.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			grdPendingForSecond.RootTable.Columns["FirstWeight"].FormatString = "#,##0.###";
			grdPendingForSecond.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			grdPendingForSecond.RootTable.Columns["SecondWeight"].FormatString = "#,##0.###";
			grdPendingForSecond.RootTable.Columns["Id"].Visible = false;
			grdPendingForSecond.RootTable.Columns["DocTypeId"].Visible = false;
			grdPendingForSecond.RootTable.Columns["ReferenceDocTypeId"].Visible = false;
			grdPendingForSecond.RootTable.Columns.Add("Edit");
			grdPendingForSecond.RootTable.Columns["Edit"].Caption = "Edit";
			grdPendingForSecond.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingForSecond.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdPendingForSecond.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdPendingForSecond.RootTable.Columns["Edit"].Width = 50;
			grdPendingForSecond.RootTable.Columns["Edit"].Position = 0;
			grdPendingForSecond.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rbdgrossweight_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (!UpdateMode)
			{
				rdgross();
				return;
			}
			if (tarechk)
			{
				btnTareReading2ndtime.Enabled = false;
				btnTareForntPic.Enabled = false;
				btnVehicle2ndTime.Enabled = false;
				previewImageTareReading.Enabled = false;
				btnTareForntPicpreview.Enabled = false;
				btnVehicle2ndTimepreview.Enabled = false;
				txtsecondWeight.Enabled = false;
				txtFirstWeight.Enabled = true;
				txtFirstDate.Enabled = true;
				txtSecondDate.Enabled = false;
			}
			else
			{
				btnTareReading2ndtime.Enabled = true;
				btnTareForntPic.Enabled = true;
				btnVehicle2ndTime.Enabled = true;
				previewImageTareReading.Enabled = true;
				btnTareForntPicpreview.Enabled = true;
				btnVehicle2ndTimepreview.Enabled = true;
			}
			if (grosschk)
			{
				btnGrossReading1stTime.Enabled = false;
				btnGrossFontPIc.Enabled = false;
				btnVehicleFront1stTime.Enabled = false;
				previewImageGrossReading.Enabled = false;
				btnGrossFontPIcPreview.Enabled = false;
				btnVehicleFront1stTimePreview.Enabled = false;
			}
			else
			{
				btnGrossReading1stTime.Enabled = true;
				btnGrossFontPIc.Enabled = true;
				btnVehicleFront1stTime.Enabled = true;
				previewImageGrossReading.Enabled = true;
				btnGrossFontPIcPreview.Enabled = true;
				btnVehicleFront1stTimePreview.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void rdgross()
	{
		try
		{
			txtsecondWeight.Text = string.Empty;
			txtFirstWeight.Enabled = true;
			txtsecondWeight.Enabled = false;
			txtSecondDate.Enabled = false;
			txtFirstDate.Enabled = true;
			btnTareReading2ndtime.Enabled = false;
			btnTareForntPic.Enabled = false;
			btnVehicle2ndTime.Enabled = false;
			previewImageTareReading.Enabled = false;
			btnTareForntPicpreview.Enabled = false;
			btnVehicle2ndTimepreview.Enabled = false;
			btnGrossReading1stTime.Enabled = true;
			btnGrossFontPIc.Enabled = true;
			btnVehicleFront1stTime.Enabled = true;
			previewImageGrossReading.Enabled = true;
			btnGrossFontPIcPreview.Enabled = true;
			btnVehicleFront1stTimePreview.Enabled = true;
			ofd5 = string.Empty;
			fileSavePath6 = string.Empty;
			PicSecondVehicle2ndTime.Image = null;
			PicSecondVehicle2ndTime.Invalidate();
			ofd1 = string.Empty;
			fileSavePath2 = string.Empty;
			PicSecondReading2ndtime.Image = null;
			PicSecondReading2ndtime.Invalidate();
			ofd3 = string.Empty;
			fileSavePath4 = string.Empty;
			PicSecondUpper.Image = null;
			PicSecondUpper.Invalidate();
			PicSecondVehicle2ndTime.Image = null;
			PicSecondVehicle2ndTime.Invalidate();
			fileSavePath6 = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void rdtare()
	{
		try
		{
			txtFirstWeight.Text = string.Empty;
			txtFirstWeight.Enabled = false;
			txtFirstDate.Enabled = false;
			txtSecondDate.Enabled = true;
			btnTareReading2ndtime.Enabled = true;
			btnTareForntPic.Enabled = true;
			btnVehicle2ndTime.Enabled = true;
			previewImageTareReading.Enabled = true;
			btnTareForntPicpreview.Enabled = true;
			btnVehicle2ndTimepreview.Enabled = true;
			btnGrossReading1stTime.Enabled = false;
			btnGrossFontPIc.Enabled = false;
			btnVehicleFront1stTime.Enabled = false;
			previewImageGrossReading.Enabled = false;
			btnGrossFontPIcPreview.Enabled = false;
			btnVehicleFront1stTimePreview.Enabled = false;
			ofd = string.Empty;
			fileSavePath = string.Empty;
			PicFirstReading1stTime.Image = null;
			PicFirstReading1stTime.Invalidate();
			ofd2 = string.Empty;
			fileSavePath3 = string.Empty;
			picFirstUpper.Image = null;
			picFirstUpper.Invalidate();
			ofd4 = string.Empty;
			fileSavePath5 = string.Empty;
			PicVehicleFront1stTime.Image = null;
			PicVehicleFront1stTime.Invalidate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rbdtareweight_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (!UpdateMode)
			{
				rdtare();
				return;
			}
			if (tarechk)
			{
				btnTareReading2ndtime.Enabled = false;
				btnTareForntPic.Enabled = false;
				btnVehicle2ndTime.Enabled = false;
				previewImageTareReading.Enabled = false;
				btnTareForntPicpreview.Enabled = false;
				btnVehicle2ndTimepreview.Enabled = false;
			}
			else
			{
				btnTareReading2ndtime.Enabled = true;
				btnTareForntPic.Enabled = true;
				btnVehicle2ndTime.Enabled = true;
				previewImageTareReading.Enabled = true;
				btnTareForntPicpreview.Enabled = true;
				btnVehicle2ndTimepreview.Enabled = true;
			}
			txtSecondDate.Enabled = true;
			txtFirstWeight.Enabled = false;
			txtFirstDate.Enabled = false;
			btnGrossReading1stTime.Enabled = false;
			btnGrossFontPIc.Enabled = false;
			btnVehicleFront1stTime.Enabled = false;
			previewImageGrossReading.Enabled = false;
			btnGrossFontPIcPreview.Enabled = false;
			btnVehicleFront1stTimePreview.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			RECID = Convert.ToInt32(DataGridHistory.CurrentRow.Cells[1].Value);
			ReadById(RECID);
			if (!string.IsNullOrWhiteSpace(CameraPixBox1Path) && !Approved)
			{
				HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
				HvCam05 = new HVUtility(this, PicSecondUpper);
				HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
				WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
				WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
				WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0983: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Expected O, but got Unknown
		//IL_09a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			txtsecondWeight.Enabled = false;
			if (!FormValidation())
			{
				return;
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				double loadWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
				double tareWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
				if ((SupplierFirstWeight > 0.0 && SupplierFirstWeight != loadWeight && MessageBox.Show("Supplier First Weight in Gate Pass does not match the Weighbridge First Weight.\n" + $"Gate Pass Weight: {SupplierFirstWeight}\n" + $"Weighbridge Weight: {loadWeight}\n\n" + "Do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) || (SupplierSecondWeight > 0.0 && tareWeight > 0.0 && SupplierSecondWeight != tareWeight && MessageBox.Show("Supplier Second Weight in Gate Pass does not match the Weighbridge Second Weight.\n" + $"Gate Pass Weight: {SupplierSecondWeight}\n" + $"Weighbridge Weight: {tareWeight}\n\n" + "Do you want to continue?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
				{
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			WbTransactions wb = new WbTransactions();
			wb.TicketNo = Conversion.ToInt((object)txtTickectNo.Text);
			wb.BiltyNo = Conversion.ToString((object)txtBilityNo.Text);
			wb.EntryNo = Conversion.ToInt((object)0);
			wb.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			wb.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			wb.PartyName = Conversion.ToString((object)((Control)(object)CmbPartyName).Text);
			wb.WbCharges = Conversion.ToDouble((object)txtWbCharges.Text);
			if (Conversion.ToInt(cmbReferenceType.Value) == 74 && lstIsSavedVehicle)
			{
				wb.VehicleNo = Conversion.ToString((object)((Control)(object)cmbVehicleNo).Text);
			}
			else
			{
				wb.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
			}
			wb.VehicleType = Conversion.ToString((object)((Control)(object)cmbVehicleType).Text);
			wb.WeighBridgeType = Conversion.ToString((object)((Control)(object)CmbWeighBridgeType).Text);
			wb.PackingType = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
			wb.ItemQty = Conversion.ToDouble((object)txtItemqty.Text);
			wb.ItemId = Conversion.ToInt(CmbItem.Value);
			wb.ItemDescription = Conversion.ToString((object)((Control)(object)CmbItem).Text);
			wb.WbRemarks = Conversion.ToString((object)txtRemarks.Text);
			wb.FirstWeight = Conversion.ToDouble((object)txtFirstWeight.Text);
			wb.SecondWeight = Conversion.ToDouble((object)txtsecondWeight.Text);
			wb.FirstDateTime = Conversion.ToDateTime((object)txtFirstDate.Value);
			wb.SecondDateTime = Conversion.ToDateTime((object)txtSecondDate.Value);
			wb.PackingUnitWeight = Conversion.ToDouble((object)txtEbUnit.Text);
			wb.PackingTotalWeight = Conversion.ToDouble((object)txtEbTotalWeight.Text);
			wb.WeightAfterPackingWeight = Conversion.ToDouble((object)txtWeightAfterPakingWeight.Text);
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				wb.ReferenceDocTypeId = 51;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard")
			{
				wb.ReferenceDocTypeId = 91;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.ContainerNo = txtContainerNo.Text.Trim();
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "Move Order")
			{
				wb.ReferenceDocTypeId = 74;
				wb.ReferenceDocNoId = Conversion.ToInt((object)txtrefdocNumberofMoveOrder.Text);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "General")
			{
				wb.ReferenceDocTypeId = 75;
				wb.ReferenceDocNoId = Conversion.ToInt((object)txtrefdocNumberofMoveOrder.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Inward")
			{
				wb.ReferenceDocTypeId = 52;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Outward")
			{
				wb.ReferenceDocTypeId = 92;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (Conversion.ToInt(cmbReferenceType.Value) == 54 || Conversion.ToInt(cmbReferenceType.Value) == 55)
			{
				wb.ReferenceDocTypeId = Conversion.ToInt(cmbReferenceType.Value);
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
			}
			wb.DocTypeId = 102;
			wb.DocDate = DateTime.Now;
			wb.EntryDate = DateTime.Now;
			wb.ModifyDate = DateTime.Now;
			wb.EntryUser = UserAccount.ID;
			wb.ModifyUser = UserAccount.ID;
			wb.BranchId = UserAccount.BranchesId;
			wb.OrganizationId = UserAccount.OrganizationId;
			wb.CompanyId = UserAccount.CompanyId;
			wb.FirstWeighBridgeId = Conversion.ToInt(dtWbList.Rows[0]["Id"]);
			wb.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			wb.MultiItem = false;
			if ((txtFirstWeight.Text != "0" || txtFirstWeight.Text != null) && !string.IsNullOrWhiteSpace(CameraPixBox1Path))
			{
				wb.FirstWtPicReading = Conversion.ToString((object)UniqFileName);
				wb.FirstWtUpperPic = Conversion.ToString((object)UniqFileName2);
				wb.FirstWtFrontPic = Conversion.ToString((object)UniqFileName3);
			}
			wb.LoadWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
			wb.TearWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
			wb.SupplierWbNetWeight = Conversion.ToDouble((object)txtSupplierNetWeight.Text.Trim());
			int success = WbTransactions.Save(wb);
			if (success <= 0)
			{
				return;
			}
			MessageBox.Show("Record Save Successfully" + wb.TicketNo);
			if (ofd != null && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				PicFirstReading1stTime.Image = null;
				PicFirstReading1stTime.Invalidate();
			}
			if (ofd1 != null && ofd1 != string.Empty)
			{
				File.Copy(ofd1, fileSavePath2, overwrite: true);
				ofd1 = string.Empty;
				fileSavePath2 = string.Empty;
				PicSecondReading2ndtime.Image = null;
				PicSecondReading2ndtime.Invalidate();
			}
			if (ofd2 != null && ofd2 != string.Empty)
			{
				File.Copy(ofd2, fileSavePath3, overwrite: true);
				ofd2 = string.Empty;
				fileSavePath3 = string.Empty;
				picFirstUpper.Image = null;
				picFirstUpper.Invalidate();
			}
			if (ofd3 != null && ofd3 != string.Empty)
			{
				File.Copy(ofd3, fileSavePath4, overwrite: true);
				ofd3 = string.Empty;
				fileSavePath4 = string.Empty;
				PicSecondUpper.Image = null;
				PicSecondUpper.Invalidate();
			}
			if (ofd4 != null && ofd4 != string.Empty)
			{
				File.Copy(ofd4, fileSavePath5, overwrite: true);
				ofd4 = string.Empty;
				fileSavePath5 = string.Empty;
				PicVehicleFront1stTime.Image = null;
				PicVehicleFront1stTime.Invalidate();
			}
			if (ofd5 != null && ofd5 != string.Empty)
			{
				File.Copy(ofd5, fileSavePath6, overwrite: true);
				ofd5 = string.Empty;
				fileSavePath6 = string.Empty;
				PicSecondVehicle2ndTime.Image = null;
				PicSecondVehicle2ndTime.Invalidate();
			}
			int GPID = Conversion.ToInt(cmbRefDocNo.Value);
			formRefresh();
			if (chkPrintWithLab.Checked)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(GPID);
			}
			if (ChkBox.Checked)
			{
				DataTable dt2 = new DataTable();
				dt2 = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = success
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt2, "280-InvRptWeighBridgeSlip.rpt");
				}
			}
			if (ChkBoxPrintImages.Checked)
			{
				DataTable dt = new DataTable();
				dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 102,
					Id = success
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.ShowReportWithDataTable(dt, "281-InvRptWeighBridgeSlipWithPics.rpt");
			}
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
			ChkMultiItem.Visible = true;
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)cmbReferenceType).Enabled = false;
			DataTable dtgetbyId = new DataTable();
			RECID = Conversion.ToInt((object)ID);
			dtgetbyId = WbTransactions.GetByID(RECID);
			if (dtgetbyId.Rows.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtTickectNo.Text = dtgetbyId.Rows[0]["TicketNo"].ToString();
			txtFirstWeight.Text = dtgetbyId.Rows[0]["FirstWeight"].ToString();
			txtFirstDate.Value = Conversion.ToDateTime(dtgetbyId.Rows[0]["FirstDateTime"]);
			txtsecondWeight.Text = Conversion.ToString(dtgetbyId.Rows[0]["SecondWeight"]);
			txtNetWeight.Text = dtgetbyId.Rows[0]["NetWbWeight"].ToString();
			txtWorkingReportNo.Text = Conversion.ToString(dtgetbyId.Rows[0]["WorkingReportNo"]);
			RefDocumentTypeId = Conversion.ToInt(dtgetbyId.Rows[0]["RefDocumentTypeId"]);
			if (txtNetWeight.Text == string.Empty || txtNetWeight.Text == null)
			{
				CalculateNetWeight();
			}
			if (txtFirstWeight.Text != string.Empty && Conversion.ToInt((object)txtsecondWeight.Text.ToString()) > 0)
			{
				txtsecondWeight.Enabled = false;
				txtFirstWeight.Enabled = false;
				txtBilityNo.Enabled = false;
				txtVehicleNo.Enabled = false;
				((Control)(object)cmbRefDocNo).Enabled = false;
			}
			txtBilityNo.Text = dtgetbyId.Rows[0]["BiltyNo"].ToString();
			((Control)(object)CmbPartyName).Text = dtgetbyId.Rows[0]["PartyName"].ToString();
			txtVehicleNo.Text = dtgetbyId.Rows[0]["VehicleNo"].ToString();
			((Control)(object)cmbVehicleType).Text = Conversion.ToString(dtgetbyId.Rows[0]["VehicleType"]);
			((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtgetbyId.Rows[0]["WeighBridgeType"]);
			((Control)(object)cmbPackingType).Text = Conversion.ToString(dtgetbyId.Rows[0]["PackingType"]);
			txtEbUnit.Text = Conversion.ToString(dtgetbyId.Rows[0]["PackingUnitWeight"]);
			txtEbTotalWeight.Text = Conversion.ToString(dtgetbyId.Rows[0]["PackingTotalWeight"]);
			txtWeightAfterPakingWeight.Text = Conversion.ToString(dtgetbyId.Rows[0]["WeightAfterPackingWeight"]);
			txtWbCharges.Text = dtgetbyId.Rows[0]["WbCharges"].ToString();
			txtItemqty.Text = dtgetbyId.Rows[0]["ItemQty"].ToString();
			txtRemarks.Text = dtgetbyId.Rows[0]["WbRemarks"].ToString();
			((Control)(object)cmbRefDocNo).Visible = true;
			txtrefdocNumberofMoveOrder.Visible = false;
			int reftypeid = Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocTypeId"]);
			if (reftypeid == 51 || reftypeid == 1601)
			{
				DataTable dt5 = new DataTable();
				dt5.Columns.Add("Id");
				dt5.Columns.Add("GpNo");
				dt5.Columns.Add("VehicleNo");
				dt5.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]));
				DDL.BindDDL(dt5, cmbRefDocNo, "Id", "GpNo", "GatePassNo", ZeroIndex: false);
				((Control)(object)cmbReferenceType).Text = "GatePass Inward";
			}
			((Control)(object)cmbRefDocNo).Enabled = false;
			if (reftypeid == 91)
			{
				((Control)(object)cmbReferenceType).Text = "GatePass OutWard";
				DataTable dt4 = new DataTable();
				dt4.Columns.Add("Id");
				dt4.Columns.Add("GpNo");
				dt4.Columns.Add("VehicleNo");
				dt4.Columns.Add("GatepassType");
				dt4.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]), Conversion.ToString(dtgetbyId.Rows[0]["GatepassType"]));
				DDL.BindDDL(dt4, cmbRefDocNo, "Id", "GpNo", "GatePassNo", ZeroIndex: false);
				txtContainerNo.Text = dtgetbyId.Rows[0]["ContainerNo"].ToString();
				txtWeightDiffComments.Text = Conversion.ToString(dtgetbyId.Rows[0]["WeightDiffComments"]);
				((UltraGridBase)cmbRefDocNo).Rows[0].Activate();
				DisableFileds();
				string OrderType = Conversion.ToString(dtgetbyId.Rows[0]["OtherSupCust"]);
				int OrderId = Conversion.ToInt(dtgetbyId.Rows[0]["OrderId"]);
				switch (OrderType)
				{
				case "DeliverOrder":
					if (((UltraGridBase)cmbRefDocNo).ActiveRow != null && Conversion.ToInt(cmbRefDocNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) == "Export")
					{
						getInvoiceNoFromDeliveryOrderByGpId(Conversion.ToInt(cmbRefDocNo.Value));
					}
					if (WeighBridgeForOutwardPartyAndItemWise && ((UltraGridBase)cmbRefDocNo).ActiveRow != null && Conversion.ToInt(cmbRefDocNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) != "Export")
					{
						dtPartiesAndItemsFromDeliveryOrder = InvDeliveryOrder.GetPartiesAndItemsFromDeliveryOrderByGpId(Conversion.ToInt(cmbRefDocNo.Value), RECID);
						PartyNameBindforOutward();
						((UltraGridBase)CmbItem).DataSource = null;
						((Control)(object)CmbItem).Text = "";
						CmbPartyName_Leave(null, null);
					}
					else
					{
						ItemsFromDeliveryOrder(OrderId);
					}
					break;
				case "SaleOrder":
				case "SaleOrderStoreAndPm":
					ItemsFromSaleOrder(OrderId);
					break;
				default:
					ItemBind();
					PartyBind();
					break;
				}
				((Control)(object)CmbPartyName).Text = dtgetbyId.Rows[0]["PartyName"].ToString();
				((Control)(object)CmbPartyName).Enabled = WeighBridgeForOutwardPartyAndItemWise;
				if (WeighBridgeForOutwardPartyAndItemWise && OrderType == "DeliverOrder" && Conversion.ToInt(CmbPartyName.Value) > 0)
				{
					ItemsAgainstPartyForOutward(Conversion.ToInt(CmbPartyName.Value));
				}
			}
			if (reftypeid == 52)
			{
				((Control)(object)cmbReferenceType).Text = "General GatePass Inward";
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("Id");
				dt3.Columns.Add("GpNo");
				dt3.Columns.Add("VehicleNo");
				dt3.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]));
				DDL.BindDDL(dt3, cmbRefDocNo, "Id", "GpNo", "GatePassNo", ZeroIndex: false);
				txtContainerNo.Text = dtgetbyId.Rows[0]["ContainerNo"].ToString();
			}
			if (reftypeid == 92)
			{
				((Control)(object)cmbReferenceType).Text = "General GatePass Outward";
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("GpNo");
				dt2.Columns.Add("VehicleNo");
				dt2.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]));
				DDL.BindDDL(dt2, cmbRefDocNo, "Id", "GpNo", "GatePassNo", ZeroIndex: false);
				txtContainerNo.Text = dtgetbyId.Rows[0]["ContainerNo"].ToString();
			}
			if (reftypeid == 74)
			{
				((Control)(object)cmbRefDocNo).Visible = false;
				txtrefdocNumberofMoveOrder.Visible = true;
				((Control)(object)cmbReferenceType).Text = "Move Order";
				txtrefdocNumberofMoveOrder.Text = dtgetbyId.Rows[0]["ReferenceDocNoId"].ToString();
			}
			if (reftypeid == 75)
			{
				ItemLookupsBind();
				PartyBind();
				((Control)(object)cmbRefDocNo).Visible = false;
				txtrefdocNumberofMoveOrder.Visible = true;
				((Control)(object)cmbReferenceType).Text = "General";
				txtrefdocNumberofMoveOrder.Text = dtgetbyId.Rows[0]["ReferenceDocNoId"].ToString();
			}
			if (reftypeid == 54 || reftypeid == 55)
			{
				cmbReferenceType.Value = dtgetbyId.Rows[0]["ReferenceDocTypeId"].ToString();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpNo");
				dt.Columns.Add("VehicleNo");
				dt.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]));
				DDL.BindDDL(dt, cmbRefDocNo, "Id", "GpNo", "GatePassNo", ZeroIndex: false);
			}
			if (Conversion.ToInt((object)txtsecondWeight.Text) > 0)
			{
				Approved = true;
			}
			else
			{
				Approved = Conversion.ToBool(dtgetbyId.Rows[0]["IsApproved"]);
			}
			cmbRefDocNo.Value = dtgetbyId.Rows[0]["ReferenceDocNoId"].ToString();
			if (reftypeid == 51 && (RefDocumentTypeId == 105 || RefDocumentTypeId == 106 || RefDocumentTypeId == 110 || RefDocumentTypeId == 41))
			{
				((Control)(object)CmbItem).Text = string.Empty;
				DataTable dtLabInfo = LabItemsBindByGpId();
				if (dtLabInfo.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtLabInfo, CmbItem, "ItemId", "ItemName", "ItemName", false);
					((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					((UltraGridBase)CmbItem).Rows[0].Activate();
				}
				else if (Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]) > 0)
				{
					CmbItem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]);
				}
				else
				{
					((Control)(object)CmbItem).Text = dtgetbyId.Rows[0]["ItemDescription"].ToString();
				}
			}
			else if (Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]) > 0)
			{
				CmbItem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]);
			}
			else
			{
				((Control)(object)CmbItem).Text = dtgetbyId.Rows[0]["ItemDescription"].ToString();
			}
			txtSupplierFirstWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["LoadWeight"]).ToString();
			txtSupplierSecondWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["TearWeight"]).ToString();
			txtSupplierNetWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["SupplierWbNetWeight"]).ToString();
			UpdateMode = true;
			txtsecondWeight.Focus();
			txtFirstWeight.Enabled = false;
			txtFirstDate.Enabled = false;
			txtSecondDate.Enabled = false;
			txtsecondWeight.Enabled = false;
			if (string.IsNullOrEmpty(CameraPixBox1Path))
			{
				return;
			}
			WeightBridge_Helper.CameraDisconnect(HvCam01);
			WeightBridge_Helper.CameraDisconnect(HvCam02);
			WeightBridge_Helper.CameraDisconnect(HvCam03);
			if (Conversion.ToDouble((object)txtFirstWeight.Text) > 0.0 && Conversion.ToDouble((object)txtsecondWeight.Text) > 0.0)
			{
				WeightBridge_Helper.CameraDisconnect(HvCam04);
				WeightBridge_Helper.CameraDisconnect(HvCam05);
				WeightBridge_Helper.CameraDisconnect(HvCam06);
			}
			string first1 = dtgetbyId.Rows[0]["FirstWtPicReading"].ToString();
			string first2 = dtgetbyId.Rows[0]["FirstWtUpperPic"].ToString();
			string first3 = dtgetbyId.Rows[0]["FirstWtFrontPic"].ToString();
			string Second1 = dtgetbyId.Rows[0]["SecondPicReading"].ToString();
			string Second2 = dtgetbyId.Rows[0]["SecondWtUpperPic"].ToString();
			string Second3 = dtgetbyId.Rows[0]["SecondWtFrontPic"].ToString();
			UniqFileName = first1;
			UniqFileName2 = first2;
			UniqFileName3 = first3;
			UniqFileName4 = Second1;
			UniqFileName5 = Second2;
			UniqFileName6 = Second3;
			fileSavePath = CameraPixBox1Path + "\\" + first1.ToString();
			fileSavePath3 = CameraPixBox1Path + "\\" + first2.ToString();
			fileSavePath5 = CameraPixBox1Path + "\\" + first3.ToString();
			fileSavePath2 = CameraPixBox1Path + "\\" + Second1.ToString();
			fileSavePath4 = CameraPixBox1Path + "\\" + Second2.ToString();
			fileSavePath6 = CameraPixBox1Path + "\\" + Second3.ToString();
			if (fileSavePath != "")
			{
				if (File.Exists(fileSavePath))
				{
					PicFirstReading1stTime.Image = Image.FromFile(fileSavePath);
					fileSavePath = dtgetbyId.Rows[0]["FirstWtPicReading"].ToString();
				}
				else
				{
					fileSavePath = "";
					PicFirstReading1stTime.Image = null;
					PicFirstReading1stTime.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				PicFirstReading1stTime.Image = null;
				PicFirstReading1stTime.Invalidate();
			}
			if (fileSavePath2 != "")
			{
				if (File.Exists(fileSavePath2))
				{
					PicSecondReading2ndtime.Image = Image.FromFile(fileSavePath2);
					fileSavePath2 = dtgetbyId.Rows[0]["SecondPicReading"].ToString();
				}
				else
				{
					fileSavePath2 = "";
					PicSecondReading2ndtime.Image = null;
					PicSecondReading2ndtime.Invalidate();
				}
			}
			else
			{
				fileSavePath2 = "";
				PicSecondReading2ndtime.Image = null;
				PicSecondReading2ndtime.Invalidate();
			}
			if (fileSavePath3 != "")
			{
				if (File.Exists(fileSavePath3))
				{
					picFirstUpper.Image = Image.FromFile(fileSavePath3);
					fileSavePath3 = dtgetbyId.Rows[0]["FirstWtUpperPic"].ToString();
				}
				else
				{
					fileSavePath3 = "";
					picFirstUpper.Image = null;
					picFirstUpper.Invalidate();
				}
			}
			else
			{
				fileSavePath3 = "";
				picFirstUpper.Image = null;
				picFirstUpper.Invalidate();
			}
			if (fileSavePath4 != "")
			{
				if (File.Exists(fileSavePath4))
				{
					PicSecondUpper.Image = Image.FromFile(fileSavePath4);
					fileSavePath4 = dtgetbyId.Rows[0]["SecondWtUpperPic"].ToString();
				}
				else
				{
					fileSavePath4 = "";
					PicSecondUpper.Image = null;
					PicSecondUpper.Invalidate();
				}
			}
			else
			{
				fileSavePath4 = "";
				PicSecondUpper.Image = null;
				PicSecondUpper.Invalidate();
			}
			if (fileSavePath5 != "")
			{
				if (File.Exists(fileSavePath5))
				{
					PicVehicleFront1stTime.Image = Image.FromFile(fileSavePath5);
					fileSavePath5 = dtgetbyId.Rows[0]["FirstWtFrontPic"].ToString();
				}
				else
				{
					fileSavePath5 = "";
					PicVehicleFront1stTime.Image = null;
					PicVehicleFront1stTime.Invalidate();
				}
			}
			else
			{
				fileSavePath5 = "";
				PicVehicleFront1stTime.Image = null;
				PicVehicleFront1stTime.Invalidate();
			}
			if (fileSavePath6 != "")
			{
				if (File.Exists(fileSavePath6))
				{
					PicSecondVehicle2ndTime.Image = Image.FromFile(fileSavePath6);
					fileSavePath6 = dtgetbyId.Rows[0]["SecondWtFrontPic"].ToString();
				}
				else
				{
					fileSavePath6 = "";
					PicSecondVehicle2ndTime.Image = null;
					PicSecondVehicle2ndTime.Invalidate();
				}
			}
			else
			{
				fileSavePath6 = "";
				PicSecondVehicle2ndTime.Image = null;
				PicSecondVehicle2ndTime.Invalidate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Expected O, but got Unknown
		//IL_0a81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Approved)
			{
				throw new Exception("Record cannot be Updated because Record has Approved");
			}
			if (!FormValidation())
			{
				return;
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				double wbnetweight = Conversion.ToDouble((object)txtNetWeight.Text);
				double wbsupplierweight = Conversion.ToDouble((object)txtSupplierNetWeight.Text);
				if (Math.Abs(wbnetweight - wbsupplierweight) > weighbridgeTolerance && string.IsNullOrWhiteSpace(txtWeightDiffComments.Text.Trim()))
				{
					MessageBox.Show("Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\n" + $"Supplier Net Weight: {wbsupplierweight}\n" + $"Weighbridge Net Weight: {wbnetweight}\n\n" + $"Weighbridge Tolerance is : {weighbridgeTolerance}\n\n" + "Difference Weight Remarks are required", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			txtsecondWeight.Focus();
			WbTransactions wb = new WbTransactions();
			wb.Id = RECID;
			wb.TicketNo = Conversion.ToInt((object)txtTickectNo.Text.Trim());
			wb.BiltyNo = Conversion.ToString((object)txtBilityNo.Text.Trim());
			wb.EntryNo = Conversion.ToInt((object)0);
			wb.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			wb.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			wb.PartyName = Conversion.ToString((object)((Control)(object)CmbPartyName).Text.Trim());
			wb.WbCharges = Conversion.ToDouble((object)txtWbCharges.Text.Trim());
			wb.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			wb.VehicleType = Conversion.ToString((object)((Control)(object)cmbVehicleType).Text);
			wb.WeighBridgeType = Conversion.ToString((object)((Control)(object)CmbWeighBridgeType).Text);
			wb.PackingType = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
			wb.ItemQty = Conversion.ToDouble((object)txtItemqty.Text.Trim());
			wb.ItemId = Conversion.ToInt(CmbItem.Value);
			wb.ItemDescription = Conversion.ToString((object)((Control)(object)CmbItem).Text.Trim());
			wb.WbRemarks = Conversion.ToString((object)txtRemarks.Text.Trim());
			wb.SecondWeight = Conversion.ToDouble((object)txtsecondWeight.Text.Trim());
			wb.NetWbWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			wb.SecondDateTime = Conversion.ToDateTime((object)txtSecondDate.Value);
			wb.FirstDateTime = Conversion.ToDateTime((object)txtFirstDate.Value);
			wb.FirstWeight = Conversion.ToDouble((object)txtFirstWeight.Text.Trim());
			wb.IsApproved = true;
			wb.ApprovedDateTime = Conversion.ToDateTime((object)DateTime.Now);
			wb.ApprovedUserId = UserAccount.ID;
			wb.WeightDiffComments = txtWeightDiffComments.Text.Trim();
			wb.PackingUnitWeight = Conversion.ToDouble((object)txtEbUnit.Text);
			wb.PackingTotalWeight = Conversion.ToDouble((object)txtEbTotalWeight.Text);
			wb.WeightAfterPackingWeight = Conversion.ToDouble((object)txtWeightAfterPakingWeight.Text);
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				wb.ReferenceDocTypeId = 51;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard")
			{
				wb.ReferenceDocTypeId = 91;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.ContainerNo = txtContainerNo.Text.Trim();
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "Move Order")
			{
				wb.ReferenceDocTypeId = 74;
				wb.ReferenceDocNoId = Conversion.ToInt((object)RECID);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "General")
			{
				wb.ReferenceDocTypeId = 75;
				wb.ReferenceDocNoId = Conversion.ToInt((object)RECID);
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Inward")
			{
				wb.ReferenceDocTypeId = 52;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Outward")
			{
				wb.ReferenceDocTypeId = 92;
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			}
			if (Conversion.ToInt(cmbReferenceType.Value) == 54 || Conversion.ToInt(cmbReferenceType.Value) == 55)
			{
				wb.ReferenceDocTypeId = Conversion.ToInt(cmbReferenceType.Value);
				wb.ReferenceDocNoId = Conversion.ToInt(cmbRefDocNo.Value);
			}
			wb.LoadWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text.Trim());
			wb.TearWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text.Trim());
			wb.SupplierWbNetWeight = Conversion.ToDouble((object)txtSupplierNetWeight.Text.Trim());
			wb.DocTypeId = 102;
			wb.DocDate = Conversion.ToDateTime((object)DateTime.Now);
			wb.EntryDate = DateTime.Now;
			wb.ModifyDate = DateTime.Now;
			wb.EntryUser = UserAccount.ID;
			wb.ModifyUser = UserAccount.ID;
			wb.BranchId = UserAccount.BranchesId;
			wb.OrganizationId = UserAccount.OrganizationId;
			wb.CompanyId = UserAccount.CompanyId;
			wb.SecondWeighBridgeId = Conversion.ToInt(dtWbList.Rows[0]["Id"]);
			wb.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (ChkMultiItem.Checked)
			{
				wb.MultiItem = true;
			}
			else
			{
				wb.MultiItem = false;
			}
			if (!string.IsNullOrEmpty(CameraPixBox1Path))
			{
				wb.SecondPicReading = Conversion.ToString((object)UniqFileName4);
				wb.SecondWtUpperPic = Conversion.ToString((object)UniqFileName5);
				wb.SecondWtFrontPic = Conversion.ToString((object)UniqFileName6);
			}
			int success = WbTransactions.Save(wb);
			if (success > 0)
			{
				MessageBox.Show("Record Update Successfully" + wb.TicketNo);
			}
			if (ofd != null && ofd != string.Empty)
			{
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				PicFirstReading1stTime.Image = null;
				PicFirstReading1stTime.Invalidate();
			}
			if (ofd1 != null && ofd1 != string.Empty)
			{
				File.Copy(ofd1, fileSavePath2, overwrite: true);
				ofd1 = string.Empty;
				fileSavePath2 = string.Empty;
				PicSecondReading2ndtime.Image = null;
				PicSecondReading2ndtime.Invalidate();
			}
			if (ofd2 != null && ofd2 != string.Empty)
			{
				File.Copy(ofd2, fileSavePath3, overwrite: true);
				ofd2 = string.Empty;
				fileSavePath3 = string.Empty;
				picFirstUpper.Image = null;
				picFirstUpper.Invalidate();
			}
			if (ofd3 != null && ofd3 != string.Empty)
			{
				File.Copy(ofd3, fileSavePath4, overwrite: true);
				ofd3 = string.Empty;
				fileSavePath4 = string.Empty;
				PicSecondUpper.Image = null;
				PicSecondUpper.Invalidate();
			}
			if (ofd4 != null && ofd4 != string.Empty)
			{
				File.Copy(ofd4, fileSavePath5, overwrite: true);
				ofd4 = string.Empty;
				fileSavePath5 = string.Empty;
				PicVehicleFront1stTime.Image = null;
				PicVehicleFront1stTime.Invalidate();
			}
			if (ofd5 != null && ofd5 != string.Empty)
			{
				File.Copy(ofd5, fileSavePath6, overwrite: true);
				ofd5 = string.Empty;
				fileSavePath6 = string.Empty;
				PicSecondVehicle2ndTime.Image = null;
				PicSecondVehicle2ndTime.Invalidate();
			}
			int GPID = Conversion.ToInt(cmbRefDocNo.Value);
			formRefresh();
			UpdateMode = false;
			((Control)(object)cmbReferenceType).Enabled = true;
			((Control)(object)cmbRefDocNo).Enabled = true;
			txtFirstWeight.Enabled = true;
			cmbReferenceType.Focus();
			if (chkPrintWithLab.Checked)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(GPID);
			}
			if (ChkBox.Checked)
			{
				DataTable dt2 = new DataTable();
				dt2 = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = success
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt2, "280-InvRptWeighBridgeSlip.rpt");
				}
			}
			if (ChkBoxPrintImages.Checked)
			{
				DataTable dt = new DataTable();
				dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 102,
					Id = success
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.ShowReportWithDataTable(dt, "281-InvRptWeighBridgeSlipWithPics.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			formRefresh();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGrossReading1stTime_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd);
					string exten = Path.GetExtension(ofd);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath = Path.Combine(saveDirectory, fileName);
					PicFirstReading1stTime.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTareReading2ndtime_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd1 = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd1);
					string exten = Path.GetExtension(ofd1);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath2 = Path.Combine(saveDirectory, fileName);
					PicSecondReading2ndtime.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGrossFontPIc_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd2 = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd2);
					string exten = Path.GetExtension(ofd2);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath3 = Path.Combine(saveDirectory, fileName);
					picFirstUpper.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTareForntPic_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd3 = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd3);
					string exten = Path.GetExtension(ofd3);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath4 = Path.Combine(saveDirectory, fileName);
					PicSecondUpper.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVehicleFront1stTime_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd4 = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd4);
					string exten = Path.GetExtension(ofd4);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath5 = Path.Combine(saveDirectory, fileName);
					PicVehicleFront1stTime.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVehicle2ndTime_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (Directory.Exists(saveDirectory))
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					ofd5 = openFileDialog.FileName;
					string fileName = Path.GetFileNameWithoutExtension(ofd5);
					string exten = Path.GetExtension(ofd5);
					string date = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
					fileName = fileName + date + exten;
					fileSavePath6 = Path.Combine(saveDirectory, fileName);
					PicSecondVehicle2ndTime.Image = new Bitmap(openFileDialog.FileName);
				}
			}
			else
			{
				MessageBox.Show("Please Check Your Folder Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void previewImageGrossReading_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = PicFirstReading1stTime.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void previewImageTareReading_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = PicSecondReading2ndtime.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGrossFontPIcPreview_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = picFirstUpper.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnTareForntPicpreview_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = PicSecondUpper.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVehicleFront1stTimePreview_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = PicVehicleFront1stTime.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVehicle2ndTimepreview_Click(object sender, EventArgs e)
	{
		try
		{
			ImagePreview imagePreview = new ImagePreview();
			Logo = PicSecondVehicle2ndTime.Image;
			imagePreview.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters wb = new ReportsParameters();
			wb.OrganizationId = UserAccount.OrganizationId;
			wb.CompanyId = UserAccount.CompanyId;
			wb.BranchesId = UserAccount.BranchesId;
			wb.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			wb.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!wb.CanViewAllRecord)
			{
				wb.EntryUser = UserAccount.ID;
			}
			wb.ActionId = Conversion.ToInt(dtWbList.Rows[0]["Id"]);
			wb.FromDocNo = Conversion.ToInt((object)txtTicketNoFrom.Text.Trim());
			wb.ToDocNo = Conversion.ToInt((object)txtTicketNoTo.Text.Trim());
			if (drdocdate.Checked)
			{
				if (FromDate.Checked)
				{
					wb.FromDate = FromDate.Value;
				}
				if (ToDate.Checked)
				{
					wb.ToDate = ToDate.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDate.Checked)
				{
					wb.EntryDateFrom = FromDate.Value;
				}
				if (ToDate.Checked)
				{
					wb.EntryDateTo = ToDate.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDate.Checked)
				{
					wb.ModifyDateFrom = FromDate.Value;
				}
				if (ToDate.Checked)
				{
					wb.ModifyDateTo = ToDate.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDate.Checked)
				{
					wb.ApprovedDateFrom = FromDate.Value;
				}
				if (ToDate.Checked)
				{
					wb.ApprovedDateTo = ToDate.Value;
				}
			}
			wb.GpSrNoF = Conversion.ToInt((object)GPNoFrom.Text.Trim());
			wb.GpSrNoT = Conversion.ToInt((object)GpNoTo.Text.Trim());
			wb.VehicleNo = Conversion.ToString((object)TxtVehicleNumber.Text.Trim());
			wb.RefDocumentTypeId = Conversion.ToInt(CmbGatePassTypeHistory.Value);
			wb.Activity = ((Conversion.ToInt(CmbWbTypeHistory.Value) > 0) ? Conversion.ToString((object)((Control)(object)CmbWbTypeHistory).Text) : "");
			DataTable dtget = WbTransactions.FormHistory(wb);
			if (dtget.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocumentType", typeof(string));
				dtcol.Columns.Add("WeighBridgeType", typeof(string));
				dtcol.Columns.Add("ReferenceDocNoId", typeof(int));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("TicketNo", typeof(int));
				dtcol.Columns.Add("PartyName", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("ItemDescription", typeof(string));
				dtcol.Columns.Add("ReferenceDocTypeId", typeof(int));
				dtcol.Columns.Add("ItemQty", typeof(double));
				dtcol.Columns.Add("WbCharges", typeof(double));
				dtcol.Columns.Add("WorkingReportNo", typeof(string));
				dtcol.Columns.Add("FirstWeight", typeof(double));
				dtcol.Columns.Add("FirstWtWbStatus", typeof(string));
				dtcol.Columns.Add("SecondWeight", typeof(double));
				dtcol.Columns.Add("SecondWtWbStatus", typeof(string));
				dtcol.Columns.Add("NetWbWeight", typeof(double));
				dtcol.Columns.Add("EbUnit", typeof(double));
				dtcol.Columns.Add("EbTotal", typeof(double));
				dtcol.Columns.Add("WeightAfterPackingWeight", typeof(double));
				dtcol.Columns.Add("WbRemarks", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtget.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtget.Rows[i]["RecordNo"], dtget.Rows[i]["Id"], Conversion.ToDateTime(dtget.Rows[i]["DocDate"]).ToShortDateString(), dtget.Rows[i]["DocumentTypeDescription"], dtget.Rows[i]["WeighBridgeType"], dtget.Rows[i]["ReferenceDocNoId"], dtget.Rows[i]["GpSrNo"], dtget.Rows[i]["TicketNo"], dtget.Rows[i]["PartyName"], dtget.Rows[i]["VehicleNo"], dtget.Rows[i]["BiltyNo"], dtget.Rows[i]["ItemDescription"], dtget.Rows[i]["ReferenceDocTypeId"], dtget.Rows[i]["ItemQty"], dtget.Rows[i]["WbCharges"], dtget.Rows[i]["WorkingReportNo"], dtget.Rows[i]["FirstWeight"], dtget.Rows[i]["FirstWtWbStatus"], dtget.Rows[i]["SecondWeight"], dtget.Rows[i]["SecondWtWbStatus"], dtget.Rows[i]["NetWbWeight"], dtget.Rows[i]["PackingUnitWeight"], dtget.Rows[i]["PackingTotalWeight"], dtget.Rows[i]["WeightAfterPackingWeight"], dtget.Rows[i]["WbRemarks"], Conversion.ToDateTime(dtget.Rows[i]["EntryDate"]).ToShortDateString(), dtget.Rows[i]["UserName"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "ReferenceDocTypeId", "ReferenceDocNoId" };
			List<string> ExcludeFromSum = new List<string> { "EbUnit" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 0, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, ExcludeFromSum);
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 50, 1);
			GridEX_Helper.AddButton(DataGridHistory, "PrintWithImage", "PrintImage", 80, 2);
			GridEX_Helper.AddButton(DataGridHistory, "PrintWithLab", "PrintWithLab", 110, 3);
			DataGridHistory.FrozenColumns = 4;
			ctrlGrdBar3_Load(null, null);
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
			BindHistoryGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			txtTicketNoFrom.Text = "0";
			txtTicketNoTo.Text = string.Empty;
			TxtVehicleNumber.Text = string.Empty;
			GPNoFrom.Text = string.Empty;
			GpNoTo.Text = string.Empty;
			FromDate.Value = DateTime.Now.AddDays(-3.0);
			ToDate.Value = DateTime.Now;
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
			WbType(CommonServices.GatePassReferenceTypes());
			GatePassReferenceTypeBind(CommonServices.GatePassReferenceTypes());
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingForSecond).Name);
			if (fs != null)
			{
				grdPendingForSecond.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtGroosWeight_Leave(object sender, EventArgs e)
	{
		CalculateNetWeight();
	}

	public void CalculateNetWeight()
	{
		try
		{
			if (txtFirstWeight.Text != string.Empty && txtsecondWeight.Text != string.Empty)
			{
				double a = double.Parse(txtFirstWeight.Text);
				double b = double.Parse(txtsecondWeight.Text);
				if (a > b)
				{
					double c2 = a - b;
					if (c2 >= 0.0)
					{
						txtNetWeight.Text = c2.ToString();
					}
					else
					{
						MessageBox.Show("Please Enter The Correct Weight");
					}
				}
				if (b > a)
				{
					double c = b - a;
					if (c >= 0.0)
					{
						txtNetWeight.Text = c.ToString();
					}
					else
					{
						MessageBox.Show("Please Enter The Correct Weight");
					}
				}
				if (a == b)
				{
					txtNetWeight.Text = "0";
				}
			}
			CalculateDifferenceWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeight_Leave(object sender, EventArgs e)
	{
		CalculateNetWeight();
		PackingWeightCalculateAndNetWeight();
	}

	private void CalculateDifferenceWeights()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtFirstWeight.Text);
			double SupplierFirstWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
			double FirstWtDiff = num - SupplierFirstWeight;
			txtFirstWtDifference.Text = Math.Abs(FirstWtDiff).ToString();
			double num2 = Conversion.ToDouble((object)txtsecondWeight.Text);
			double SupplierSecondWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
			double SecondWtDiff = num2 - SupplierSecondWeight;
			txtSecondWtDifference.Text = Math.Abs(SecondWtDiff).ToString();
			double num3 = Conversion.ToDouble((object)txtNetWeight.Text);
			double SupplierNetWeight = Conversion.ToDouble((object)txtSupplierNetWeight.Text);
			double NetWtDiff = num3 - SupplierNetWeight;
			txtNetWtDifference.Text = Math.Abs(NetWtDiff).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassBending()
	{
		try
		{
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNo).DataSource = null;
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (((Control)(object)cmbReferenceType).Text.Trim() == "GatePass Inward")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					((Control)(object)cmbRefDocNo).Visible = true;
					((Control)(object)cmbRefDocNo).Enabled = true;
					txtContainerNo.Enabled = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					GpNoBind(WeightBridge_Helper.GetGpNoFromLab(UserAccount), HideColumnFlag: true);
				}
				else if (Conversion.ToInt(cmbReferenceType.Value) == 1601 && ((Control)(object)cmbReferenceType).Text.Trim() == "GatePass Inward")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					((Control)(object)cmbRefDocNo).Visible = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					GetGatePassInwardForWeighBridge_Engr();
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "GatePass OutWard")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					((Control)(object)cmbRefDocNo).Visible = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					txtContainerNo.Enabled = true;
					RefDocNoGatePassOutward();
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "Move Order" || Conversion.ToInt(cmbReferenceType.Value) == 218)
				{
					txtItemqty.Text = string.Empty;
					txtItemqty.Enabled = true;
					txtWorkingReportNo.Enabled = true;
					((Control)(object)cmbRefDocNo).Visible = false;
					((Control)(object)cmbRefDocNo).Enabled = false;
					txtrefdocNumberofMoveOrder.Visible = true;
					txtrefdocNumberofMoveOrder.Text = txtTickectNo.Text;
					txtrefdocNumberofMoveOrder.Enabled = false;
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "General")
				{
					txtItemqty.Text = string.Empty;
					txtItemqty.Enabled = true;
					txtWorkingReportNo.Enabled = true;
					((Control)(object)cmbRefDocNo).Visible = false;
					((Control)(object)cmbRefDocNo).Enabled = false;
					txtrefdocNumberofMoveOrder.Visible = true;
					txtrefdocNumberofMoveOrder.Text = txtTickectNo.Text;
					txtrefdocNumberofMoveOrder.Enabled = false;
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "General GatePass Inward")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					txtWorkingReportNo.Enabled = true;
					((Control)(object)cmbRefDocNo).Visible = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					txtContainerNo.Enabled = true;
					RefDocNoGatePassGeneral();
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "General GatePass Outward")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					((Control)(object)cmbRefDocNo).Visible = true;
					txtWorkingReportNo.Enabled = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					txtContainerNo.Enabled = true;
					RefDocNoGatePassGeneralOutward();
				}
				else if (((Control)(object)cmbReferenceType).Text.Trim() == "GatePassInwardPartyProcessing" || ((Control)(object)cmbReferenceType).Text.Trim() == "GatePassOutwardPartyProcessing")
				{
					((Control)(object)cmbRefDocNo).Text = string.Empty;
					txtBilityNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtItemqty.Text = string.Empty;
					((Control)(object)cmbVehicleType).Text = string.Empty;
					((Control)(object)cmbRefDocNo).Visible = true;
					txtWorkingReportNo.Enabled = true;
					txtrefdocNumberofMoveOrder.Visible = false;
					txtContainerNo.Enabled = true;
					GatePassPartyProcessing();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void cmbReferenceType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbReferenceType).Text.Trim() == "Move Order" || Conversion.ToInt(cmbReferenceType.Value) == 218)
			{
				((UltraGridBase)cmbPackingType).Rows[0].Activate();
				txtWorkingReportNo.Enabled = true;
			}
			GatePassBending();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRefDocNo_Leave(object sender, EventArgs e)
	{
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Expected O, but got Unknown
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d48: Expected O, but got Unknown
		try
		{
			SupplierFirstWeight = 0.0;
			SupplierSecondWeight = 0.0;
			if (!btnsave.Visible || !btnsave.Enabled)
			{
				return;
			}
			ItemBind();
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				if (Conversion.ToInt(cmbReferenceType.Value) == 1601)
				{
					DataTable dtoutwardsrNo5 = CommonServices.GetByGpNoInward(1601, Conversion.ToInt(cmbRefDocNo.Value));
					if (dtoutwardsrNo5.Rows.Count > 0)
					{
						txtBilityNo.Text = dtoutwardsrNo5.Rows[0]["BiltyNo"].ToString();
						txtVehicleNo.Text = dtoutwardsrNo5.Rows[0]["VehicleNo"].ToString();
						((Control)(object)cmbVehicleType).Text = dtoutwardsrNo5.Rows[0]["VehicleType"].ToString();
						txtItemqty.Text = dtoutwardsrNo5.Rows[0]["ItemQty"].ToString();
						((Control)(object)CmbItem).Text = dtoutwardsrNo5.Rows[0]["VarietyName"].ToString();
						((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo5.Rows[0]["GatepassType"]);
						txtRemarks.Text = Conversion.ToString(dtoutwardsrNo5.Rows[0]["OtherRemarks"]);
						DisableFileds();
					}
				}
				else
				{
					DataTable dtsrNo = CommonServices.GetByGpNoInward(51, Conversion.ToInt(cmbRefDocNo.Value));
					if (dtsrNo.Rows.Count > 0)
					{
						RefDocumentTypeId = Conversion.ToInt(dtsrNo.Rows[0]["RefDocumentTypeId"]);
						txtBilityNo.Text = dtsrNo.Rows[0]["BiltyNo"].ToString();
						txtVehicleNo.Text = dtsrNo.Rows[0]["VehicleNo"].ToString();
						((Control)(object)cmbVehicleType).Text = dtsrNo.Rows[0]["VehicleType"].ToString();
						((Control)(object)CmbPartyName).Text = dtsrNo.Rows[0]["CompanyName"].ToString();
						txtItemqty.Text = dtsrNo.Rows[0]["NoOfPackages"].ToString();
						((Control)(object)CmbItem).Text = dtsrNo.Rows[0]["VarietyName"].ToString();
						txtRemarks.Text = Conversion.ToString(dtsrNo.Rows[0]["OtherRemarks"]);
						((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtsrNo.Rows[0]["GatepassType"]);
						cmbPackingType.Value = Conversion.ToInt(dtsrNo.Rows[0]["PackingTypeId"]);
						SupplierFirstWeight = ((RefDocumentTypeId == 41 && Conversion.ToInt(dtsrNo.Rows[0]["PurchaseOrderId"]) > 0) ? Conversion.ToDouble(dtsrNo.Rows[0]["SupplierFirstWeight"]) : 0.0);
						SupplierSecondWeight = ((RefDocumentTypeId == 41 && Conversion.ToInt(dtsrNo.Rows[0]["PurchaseOrderId"]) > 0) ? Conversion.ToDouble(dtsrNo.Rows[0]["SupplierSecondWeight"]) : 0.0);
						int gpentryUserId = Conversion.ToInt(dtsrNo.Rows[0]["EntryUser"]);
						if (UserAccount.ID == gpentryUserId)
						{
							txtSupplierFirstWeight.Text = Conversion.ToString(dtsrNo.Rows[0]["SupplierFirstWeight"]);
							txtSupplierSecondWeight.Text = Conversion.ToString(dtsrNo.Rows[0]["SupplierSecondWeight"]);
							txtSupplierNetWeight.Text = Conversion.ToString(dtsrNo.Rows[0]["SupplierWeight"]);
						}
						else
						{
							txtSupplierFirstWeight.Text = "0";
							txtSupplierSecondWeight.Text = "0";
							txtSupplierNetWeight.Text = "0";
						}
						DisableFileds();
						if ((LabCompulsoryBeforFirstWeight && (RefDocumentTypeId == 105 || RefDocumentTypeId == 106 || RefDocumentTypeId == 110)) || RefDocumentTypeId == 41)
						{
							DataTable dtLabInfo = LabItemsBindByGpId(1);
							if (dtLabInfo.Rows.Count > 0)
							{
								((Control)(object)CmbItem).Text = string.Empty;
								DDL.BindDDLNew(dtLabInfo, CmbItem, "ItemId", "ItemName", "ItemName", false);
								((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
								((UltraGridBase)CmbItem).Rows[0].Activate();
							}
						}
					}
				}
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard")
			{
				DataTable dtoutwardsrNo4 = CommonServices.GetByGpNo(91, Conversion.ToInt(cmbRefDocNo.Value));
				if (dtoutwardsrNo4.Rows.Count > 0)
				{
					DisableFileds();
					txtBilityNo.Text = dtoutwardsrNo4.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo4.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo4.Rows[0]["VehicleType"].ToString();
					txtItemqty.Text = dtoutwardsrNo4.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["GatepassType"]);
					txtRemarks.Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["OtherRemarks"]);
					string OrderType = Conversion.ToString(dtoutwardsrNo4.Rows[0]["OtherSupCust"]);
					int OrderId = Conversion.ToInt(dtoutwardsrNo4.Rows[0]["SaleOrderId"]);
					switch (OrderType)
					{
					case "DeliverOrder":
						if (((UltraGridBase)cmbRefDocNo).ActiveRow != null && Conversion.ToInt(cmbRefDocNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) == "Export")
						{
							getInvoiceNoFromDeliveryOrderByGpId(Conversion.ToInt(cmbRefDocNo.Value));
						}
						if (WeighBridgeForOutwardPartyAndItemWise && ((UltraGridBase)cmbRefDocNo).ActiveRow != null && Conversion.ToInt(cmbRefDocNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)cmbRefDocNo).SelectedRow.Cells["GatepassType"].Value) != "Export")
						{
							dtPartiesAndItemsFromDeliveryOrder = InvDeliveryOrder.GetPartiesAndItemsFromDeliveryOrderByGpId(Conversion.ToInt(cmbRefDocNo.Value), RECID);
							PartyNameBindforOutward();
							((UltraGridBase)CmbItem).DataSource = null;
							((Control)(object)CmbItem).Text = "";
							CmbPartyName_Leave(null, null);
						}
						else
						{
							ItemsFromDeliveryOrder(OrderId);
							((Control)(object)CmbItem).Text = dtoutwardsrNo4.Rows[0]["VarietyName"].ToString();
						}
						break;
					case "SaleOrder":
					case "SaleOrderStoreAndPm":
						ItemsFromSaleOrder(OrderId);
						break;
					default:
						ItemBind();
						PartyBind();
						((Control)(object)CmbItem).Text = dtoutwardsrNo4.Rows[0]["VarietyName"].ToString();
						break;
					}
					((Control)(object)CmbPartyName).Text = dtoutwardsrNo4.Rows[0]["CompanyName"].ToString();
					((Control)(object)CmbPartyName).Enabled = WeighBridgeForOutwardPartyAndItemWise;
					if (WeighBridgeForOutwardPartyAndItemWise && OrderType == "DeliverOrder" && Conversion.ToInt(CmbPartyName.Value) > 0)
					{
						ItemsAgainstPartyForOutward(Conversion.ToInt(CmbPartyName.Value));
					}
				}
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Inward")
			{
				DataTable dtoutwardsrNo3 = GatePassGeneral.GatePassGeneralReadByGpNo(new GatePassGeneral
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 52,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					GpSrNo = Conversion.ToInt((object)((Control)(object)cmbRefDocNo).Text.Trim())
				});
				if (dtoutwardsrNo3.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo3.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo3.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo3.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo3.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo3.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo3.Rows[0]["VarietyName"].ToString();
					((Control)(object)CmbPartyName).Text = dtoutwardsrNo3.Rows[0]["PartyName"].ToString();
					DisableFileds();
				}
			}
			if (((Control)(object)cmbReferenceType).Text == "General GatePass Outward")
			{
				DataTable dtoutwardsrNo2 = GatePassGeneral.GatePassGeneralReadByGpNo(new GatePassGeneral
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					DocumentTypeId = 92,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					GpSrNo = Conversion.ToInt((object)((Control)(object)cmbRefDocNo).Text.Trim())
				});
				if (dtoutwardsrNo2.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo2.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo2.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo2.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo2.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo2.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo2.Rows[0]["VarietyName"].ToString();
					((Control)(object)CmbPartyName).Text = dtoutwardsrNo2.Rows[0]["PartyName"].ToString();
					DisableFileds();
				}
			}
			if (Conversion.ToInt(cmbReferenceType.Value) == 54 || Conversion.ToInt(cmbReferenceType.Value) == 55)
			{
				DataTable dtoutwardsrNo = GatePassPartyProcessing.ReadByGpNoForWeighBridge(new GatePassPartyProcessing
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = Conversion.ToInt(cmbReferenceType.Value),
					BranchesId = clsGlobalVariables.UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					GpSrNo = Conversion.ToInt((object)((Control)(object)cmbRefDocNo).Text.Trim())
				});
				if (dtoutwardsrNo.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo.Rows[0]["ItemQty"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo.Rows[0]["VarietyName"].ToString();
					((Control)(object)CmbPartyName).Text = dtoutwardsrNo.Rows[0]["CompanyName"].ToString();
					((Control)(object)CmbWeighBridgeType).Text = dtoutwardsrNo.Rows[0]["GatepassType"].ToString();
					DisableFileds();
				}
			}
		}
		catch (Exception ex)
		{
			((Control)(object)CmbPartyName).Text = "";
			((Control)(object)CmbItem).Text = "";
			txtRemarks.Text = "";
			txtBilityNo.Text = "";
			txtVehicleNo.Text = "";
			((Control)(object)cmbVehicleType).Text = "";
			txtItemqty.Text = "";
			((Control)(object)CmbWeighBridgeType).Text = "";
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadWeightFromWeightBridge()
	{
		try
		{
			decimal getdata = default(decimal);
			serialPort1 = new SerialPort();
			serialPort1.PortName = Conversion.ToString(dtWbList.Rows[0]["PortName"]);
			serialPort1.BaudRate = Conversion.ToInt(dtWbList.Rows[0]["BaudRate"]);
			serialPort1.ReadTimeout = Conversion.ToInt(dtWbList.Rows[0]["ReadTimeOutWb"]);
			serialPort1.WriteTimeout = Conversion.ToInt(dtWbList.Rows[0]["WriteTimeOutWb"]);
			serialPort1.DataBits = Conversion.ToInt(dtWbList.Rows[0]["DataBits"]);
			serialPort1.StopBits = StopBits.One;
			if (Conversion.ToString(dtWbList.Rows[0]["Parity"]) == "None")
			{
				serialPort1.Parity = Parity.None;
			}
			else if (Conversion.ToString(dtWbList.Rows[0]["Parity"]) == "Even")
			{
				serialPort1.Parity = Parity.Even;
			}
			else if (Conversion.ToString(dtWbList.Rows[0]["Parity"]) == "Odd")
			{
				serialPort1.Parity = Parity.Odd;
			}
			serialPort1.Open();
			Thread.Sleep(500);
			string MachineData = serialPort1.ReadExisting().ToString();
			labelSerialPortData.Text = MachineData.ToString();
			if (dtWbList.Rows.Count > 0)
			{
				if (Conversion.ToString(dtWbList.Rows[0]["ModelName"]) == "Standard")
				{
					int kgIndex = MachineData.IndexOf("KG");
					if (kgIndex > 6)
					{
						getdata = Conversion.ToDecimal((object)MachineData.Substring(kgIndex - 6, 6));
					}
				}
				else if (Conversion.ToString(dtWbList.Rows[0]["ModelName"]) == "XK3190")
				{
					int getindex = MachineData.IndexOf("+");
					if (getindex > 0)
					{
						getdata = Conversion.ToDecimal((object)MachineData.Substring(getindex + 1, 6));
					}
				}
				else if (Conversion.ToString(dtWbList.Rows[0]["ModelName"]) == "NGA10075")
				{
					int getindex2 = MachineData.IndexOf("+");
					if (getindex2 > 0)
					{
						getdata = Conversion.ToDecimal((object)MachineData.Substring(getindex2 + 1, 7));
					}
				}
				else if (Conversion.ToString(dtWbList.Rows[0]["ModelName"]) == "SC17D")
				{
					int getindex3 = MachineData.IndexOf("+");
					if (getindex3 >= 0)
					{
						getdata = Conversion.ToDecimal((object)MachineData.Substring(getindex3 + 1, 8));
					}
				}
				else if (UserAccount.CompName == "SAKINA INDUSTRY")
				{
					int getindex4 = MachineData.IndexOf("kg");
					if (getindex4 > 0)
					{
						int index = getindex4 - 7;
						getdata = Conversion.ToDecimal((object)MachineData.Substring(index, 6));
					}
				}
				else if (Conversion.ToString(dtWbList.Rows[0]["ModelName"]) == "MatrixII")
				{
					Match mx = new Regex("^\\D*?((-?(\\d+(\\.\\d+)?))|(-?\\.\\d+)).*").Match(MachineData);
					getdata = (mx.Success ? Convert.ToDecimal(mx.Groups[1].Value) : 0m);
				}
				string b = getdata.ToString();
				txtWeightReading.Text = getdata + "  Kg";
				if (UpdateMode && !Approved)
				{
					txtsecondWeight.Text = b;
					if (Conversion.ToDouble((object)txtsecondWeight.Text) > 0.0)
					{
						string PixBox1Path2 = string.Empty;
						string PixBox2Path2 = string.Empty;
						string PixBox3Path2 = string.Empty;
						if (!string.IsNullOrEmpty(CameraPixBox1Path))
						{
							UniqFileName4 = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							UniqFileName5 = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							UniqFileName6 = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							if (PicSecondReading2ndtime.Tag != null && Conversion.ToString(PicSecondReading2ndtime.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(PicSecondReading2ndtime.Tag));
							}
							if (PicSecondUpper.Tag != null && Conversion.ToString(PicSecondUpper.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(PicSecondUpper.Tag));
							}
							if (PicSecondVehicle2ndTime.Tag != null && Conversion.ToString(PicSecondVehicle2ndTime.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(PicSecondVehicle2ndTime.Tag));
							}
							PixBox1Path2 = CameraPixBox1Path + "\\" + UniqFileName4;
							HvCam04.CreateJPG(PixBox1Path2);
							PicSecondReading2ndtime.Tag = UniqFileName4;
							PixBox2Path2 = CameraPixBox1Path + "\\" + UniqFileName5;
							HvCam05.CreateJPG(PixBox2Path2);
							PicSecondUpper.Tag = UniqFileName5;
							PixBox3Path2 = CameraPixBox1Path + "\\" + UniqFileName6;
							HvCam06.CreateJPG(PixBox3Path2);
							PicSecondVehicle2ndTime.Tag = UniqFileName6;
						}
					}
				}
				if (!UpdateMode)
				{
					txtFirstWeight.Text = b;
					if (Conversion.ToDouble((object)txtFirstWeight.Text) > 0.0)
					{
						string PixBox1Path = string.Empty;
						string PixBox2Path = string.Empty;
						string PixBox3Path = string.Empty;
						if (!string.IsNullOrEmpty(CameraPixBox1Path))
						{
							UniqFileName = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							UniqFileName2 = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							UniqFileName3 = string.Format("{0}_{1}.jpg", "WBTicket_" + txtTickectNo.Text, Guid.NewGuid());
							if (PicFirstReading1stTime.Tag != null && Conversion.ToString(PicFirstReading1stTime.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(PicFirstReading1stTime.Tag));
							}
							if (picFirstUpper.Tag != null && Conversion.ToString(picFirstUpper.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(picFirstUpper.Tag));
							}
							if (PicVehicleFront1stTime.Tag != null && Conversion.ToString(PicVehicleFront1stTime.Tag) != "")
							{
								DeleteCameraPics(Conversion.ToString(PicVehicleFront1stTime.Tag));
							}
							PixBox1Path = CameraPixBox1Path + "\\" + UniqFileName;
							HvCam01.CreateJPG(PixBox1Path);
							PicFirstReading1stTime.Tag = UniqFileName;
							PixBox2Path = CameraPixBox1Path + "\\" + UniqFileName2;
							HvCam02.CreateJPG(PixBox2Path);
							picFirstUpper.Tag = UniqFileName2;
							PixBox3Path = CameraPixBox1Path + "\\" + UniqFileName3;
							HvCam03.CreateJPG(PixBox3Path);
							PicVehicleFront1stTime.Tag = UniqFileName3;
						}
					}
				}
				serialPort1.Close();
				return;
			}
			throw new Exception("WeighBridge list not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			serialPort1.Close();
		}
	}

	private void DeleteCameraPics(string pictureName)
	{
		if (pictureName == null || !(pictureName != ""))
		{
			return;
		}
		string path = Path.Combine(CameraPixBox1Path, pictureName);
		if (!File.Exists(path))
		{
			return;
		}
		using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
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
				File.Delete(path);
				fileDeleted = true;
			}
			catch (IOException)
			{
				attempts++;
				Thread.Sleep(500);
			}
		}
	}

	private void GenerateReport()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = RECID
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 102,
				Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString())
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "281-InvRptWeighBridgeSlipWithPics.rpt");
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
			FromDate.Focus();
		}
	}

	private void grdfill_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RECID = Conversion.ToInt(grdPendingForSecond.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
				txtsecondWeight.Focus();
				if (!string.IsNullOrWhiteSpace(CameraPixBox1Path))
				{
					HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
					HvCam05 = new HVUtility(this, PicSecondUpper);
					HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
					WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
					WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
					WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
				}
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
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				RECID = Convert.ToInt32(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
				if (!string.IsNullOrWhiteSpace(CameraPixBox1Path) && !Approved)
				{
					HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
					HvCam05 = new HVUtility(this, PicSecondUpper);
					HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
					WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
					WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
					WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
				}
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt2 = new DataTable();
				dt2 = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[1].Value)
				});
				if (dt2 == null || dt2.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt2, "280-InvRptWeighBridgeSlip.rpt");
				}
			}
			if (e.Column.Key == "PrintWithImage")
			{
				DataTable dt = new DataTable();
				dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 102,
					Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString())
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val2 = new Reporting();
					val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val2.ShowReportWithDataTable(dt, "281-InvRptWeighBridgeSlipWithPics.rpt");
				}
			}
			if (e.Column.Key == "PrintWithLab")
			{
				if (Conversion.ToInt(r.Cells["ReferenceDocTypeId"].Value) != 51 || Conversion.ToInt(r.Cells["ReferenceDocNoId"].Value) <= 0)
				{
					throw new Exception("This Print Is Only For 'Inward GatePass'");
				}
				CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(r.Cells["ReferenceDocNoId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbReferenceType_SelectionChangeCommitted(object sender, EventArgs e)
	{
		cmbReferenceType_Leave(null, null);
	}

	private void txtWorkingReportNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnWeighBridgeLookUp_Click(object sender, EventArgs e)
	{
		new WeighBridgeGeneralLookups(UserAccount).Show();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			VehicleTypesBind(VehicleType.GetAll());
			clsGlobalVariables.globalInvPackingType = GlobalServicesMethods.getGlobalAllPackingType(UserAccount.OrganizationId, UserAccount.CompanyId);
			WeighBridgeForOutwardPartyAndItemWise = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("WeighBridgeForOutwardPartyAndItemWise"));
			if (WeighBridgeForOutwardPartyAndItemWise && ((Control)(object)cmbReferenceType).Text == "GatePass OutWard")
			{
				PartyNameBindforOutward();
				int Id = Conversion.ToInt(CmbPartyName.Value);
				if (Id > 0)
				{
					ItemsAgainstPartyForOutward(Id);
				}
			}
			else
			{
				List<string> allServices = new List<string> { "AllItems" };
				DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
				ItemDtFillFromGlobal();
				WeightBridge_Helper.WbPartyandItemsDtFillDbCall(UserAccount, dtParty, dtitemLookups);
				PartyBind();
				ItemBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWbCharges_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
	}

	private void btnCapture_Click(object sender, EventArgs e)
	{
	}

	public void CaptureBmp(string FileName)
	{
		try
		{
			if (!(FileName != "") || FileName == null || PicFirstReading1stTime.Image == null)
			{
				return;
			}
			Bitmap bmp = new Bitmap(PicFirstReading1stTime.Image.Width, PicFirstReading1stTime.Image.Height, PixelFormat.Format24bppRgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImage(PicFirstReading1stTime.Image, 0, 0, bmp.Width, bmp.Height);
			}
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
			IntPtr scan = bmpData.Scan0;
			int bytes = bmpData.Stride * bmpData.Height;
			byte[] rgbValues = new byte[bytes];
			Marshal.Copy(scan, rgbValues, 0, bytes);
			Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
			Bitmap bit = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);
			BitmapData bmpData2 = bit.LockBits(rect2, ImageLockMode.ReadWrite, bit.PixelFormat);
			IntPtr ptr2 = bmpData2.Scan0;
			int bytes2 = bmpData2.Stride * bmpData2.Height;
			byte[] rgbValues2 = new byte[bytes2];
			Marshal.Copy(ptr2, rgbValues2, 0, bytes2);
			double colorTemp = 0.0;
			for (int j = 0; j < bmpData.Height; j++)
			{
				for (int k = 0; k < bmpData.Width * 3; k += 3)
				{
					colorTemp = (double)(int)rgbValues[j * bmpData.Stride + k + 2] * 0.299 + (double)(int)rgbValues[j * bmpData.Stride + k + 1] * 0.578 + (double)(int)rgbValues[j * bmpData.Stride + k] * 0.114;
					rgbValues2[j * bmpData2.Stride + k / 3] = (byte)colorTemp;
				}
			}
			Marshal.Copy(rgbValues2, 0, ptr2, bytes2);
			bmp.UnlockBits(bmpData);
			ColorPalette tempPalette;
			using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
			{
				tempPalette = tempBmp.Palette;
			}
			for (int i = 0; i < 256; i++)
			{
				tempPalette.Entries[i] = Color.FromArgb(i, i, i);
			}
			bit.Palette = tempPalette;
			bit.UnlockBits(bmpData2);
			bit.Save(FileName, PicFirstReading1stTime.Image.RawFormat);
			bit.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfill_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RECID = Conversion.ToInt(grdPendingForSecond.CurrentRow.Cells["Id"].Value);
			ReadById(RECID);
			txtsecondWeight.Focus();
			if (!string.IsNullOrWhiteSpace(CameraPixBox1Path) && !Approved)
			{
				HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
				HvCam05 = new HVUtility(this, PicSecondUpper);
				HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
				WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
				WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
				WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbVehicleNo_TextChanged(object sender, EventArgs e)
	{
	}

	private void BtnWeightDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new VehicleWeightLookUp(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWeightbridge_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(CameraPixBox1Path))
			{
				WeightBridge_Helper.CameraDisconnect(HvCam01);
				WeightBridge_Helper.CameraDisconnect(HvCam02);
				WeightBridge_Helper.CameraDisconnect(HvCam03);
				WeightBridge_Helper.CameraDisconnect(HvCam04);
				WeightBridge_Helper.CameraDisconnect(HvCam05);
				WeightBridge_Helper.CameraDisconnect(HvCam06);
			}
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWeightbridge_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
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
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Shift)
			{
				if (e.KeyCode == Keys.Z || e.KeyCode == Keys.A || e.KeyCode == Keys.I || e.KeyCode == Keys.N)
				{
					keySequence += e.KeyCode;
					if (keySequence.Length > 10)
					{
						keySequence = keySequence.Substring(keySequence.Length - 10);
					}
					if (keySequence.EndsWith("ZAIN"))
					{
						chkEnableWeights.Visible = true;
						keySequence = "";
					}
				}
			}
			else
			{
				keySequence = "";
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.P)
				{
					DataTable dt = new DataTable();
					dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[1].Value)
					});
					if (dt == null || dt.Rows.Count == 0)
					{
						MessageBox.Show("Not Record Found For Display");
						return;
					}
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 0)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S)
				{
					btnsave_Click(null, null);
				}
				if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					formRefresh();
				}
				_ = e.KeyCode;
				_ = 121;
				if (e.KeyCode == Keys.F5)
				{
					ReadWeightFromWeightBridge();
				}
				if (e.Control && e.KeyCode == Keys.P && btnUpdate.Visible && btnUpdate.Enabled)
				{
					GenerateReport();
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					btnWeighBridgeLookUp_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F2)
				{
					BtnWeightDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbReferenceType.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPendingForSecond).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdPendingForSecond).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPendingFirst).Focus();
					}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F1", "For Weight Bridge Lookup ");
			dt.Rows.Add("Ctrl+F1", "For Lookup Vehicle Weight");
			dt.Rows.Add("Ctrl+F5", "For Getting Weight From Indicator");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on GatePass Type");
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

	private void grdfill_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r2 = grdPendingForSecond.CurrentRow;
				if (r2 != null && (int)r2.RowType == 0 && grdPendingForSecond.CurrentColumn != null && grdPendingForSecond.CurrentColumn.Key == "Edit")
				{
					RECID = Conversion.ToInt(grdPendingForSecond.CurrentRow.Cells["Id"].Value);
					ReadById(RECID);
					txtsecondWeight.Focus();
					if (!string.IsNullOrWhiteSpace(CameraPixBox1Path))
					{
						HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
						HvCam05 = new HVUtility(this, PicSecondUpper);
						HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
						WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
						WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
						WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
					}
				}
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				return;
			}
			GridEXRow r = grdPendingForSecond.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPendingForSecond.CurrentColumn != null)
			{
				RECID = Conversion.ToInt(grdPendingForSecond.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
				txtsecondWeight.Focus();
				if (!string.IsNullOrWhiteSpace(CameraPixBox1Path))
				{
					HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
					HvCam05 = new HVUtility(this, PicSecondUpper);
					HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
					WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
					WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
					WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r2 = DataGridHistory.CurrentRow;
				if (r2 != null && (int)r2.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					if (DataGridHistory.CurrentColumn.Key == "Edit")
					{
						RECID = Convert.ToInt32(DataGridHistory.CurrentRow.Cells[1].Value);
						ReadById(RECID);
						if (!string.IsNullOrWhiteSpace(CameraPixBox1Path) && !Approved)
						{
							HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
							HvCam05 = new HVUtility(this, PicSecondUpper);
							HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
							WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
							WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
							WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
						}
					}
					if (DataGridHistory.CurrentColumn.Key == "Print")
					{
						DataTable dt2 = new DataTable();
						dt2 = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells[1].Value)
						});
						if (dt2 == null || dt2.Rows.Count == 0)
						{
							MessageBox.Show("Not Record Found For Display");
						}
						else
						{
							Reporting val = new Reporting();
							val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
							val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
							val.ShowReportWithDataTable(dt2, "280-InvRptWeighBridgeSlip.rpt");
						}
					}
					if (DataGridHistory.CurrentColumn.Key == "PrintWithImage")
					{
						DataTable dt = new DataTable();
						dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							DocumentTypeId = 102,
							Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[1].Value.ToString())
						});
						if (dt == null || dt.Rows.Count == 0)
						{
							MessageBox.Show("Not Record Found For Display");
						}
						else
						{
							Reporting val2 = new Reporting();
							val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
							val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
							val2.ShowReportWithDataTable(dt, "281-InvRptWeighBridgeSlipWithPics.rpt");
						}
					}
				}
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				RECID = Convert.ToInt32(DataGridHistory.CurrentRow.Cells[1].Value);
				ReadById(RECID);
				if (!string.IsNullOrWhiteSpace(CameraPixBox1Path) && !Approved)
				{
					HvCam04 = new HVUtility(this, PicSecondReading2ndtime);
					HvCam05 = new HVUtility(this, PicSecondUpper);
					HvCam06 = new HVUtility(this, PicSecondVehicle2ndTime);
					WeightBridge_Helper.CameraInitilization(HvCam04, IpCam01, Port01, UserNameCam01, PasswordCam01, "Camera 04");
					WeightBridge_Helper.CameraInitilization(HvCam05, IpCam02, Port02, UserNameCam02, PasswordCam02, "Camera 05");
					WeightBridge_Helper.CameraInitilization(HvCam06, IpCam03, Port03, UserNameCam03, PasswordCam03, "Camera 06");
				}
			}
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
			if (Conversion.ToInt(cmbReferenceType.Value) == 51 && ((Control)(object)cmbReferenceType).Text.Trim() == "GatePass Inward")
			{
				if (Conversion.ToInt(cmbRefDocNo.Value) > 0)
				{
					CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(cmbRefDocNo.Value));
					return;
				}
				throw new Exception("Please Select the GatePass No First...");
			}
			throw new Exception("Please Select the GatePass Type To Inward...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPurchaseLab_Click(object sender, EventArgs e)
	{
		try
		{
			new InvLabPurchaseAnalysis(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getInvoiceNoFromDeliveryOrderByGpId(int GpId)
	{
		try
		{
			((Control)(object)CmbInvoiceNo).Enabled = true;
			int ID = Conversion.ToInt(CmbInvoiceNo.Value);
			DataTable dt = InvDeliveryOrder.getInvoiceNoFromDeliveryOrderByGpId(GpId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbInvoiceNo, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: false);
				((UltraGridBase)CmbInvoiceNo).Rows[0].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbInvoiceNo.Value = ID;
					}
					else
					{
						((Control)(object)CmbInvoiceNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbInvoiceNo).Text = string.Empty;
				((UltraGridBase)CmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTicketNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTicketNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GPNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GpNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CmbPartyName_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbPartyName.Value);
			if (WeighBridgeForOutwardPartyAndItemWise && ((Control)(object)cmbReferenceType).Text == "GatePass OutWard" && Id > 0)
			{
				ItemsAgainstPartyForOutward(Id);
			}
			else
			{
				ItemBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkEnableWeights_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			txtFirstWeight.Enabled = chkEnableWeights.Checked;
			txtFirstWeight.ReadOnly = !chkEnableWeights.Checked;
			txtsecondWeight.Enabled = chkEnableWeights.Checked;
			txtsecondWeight.ReadOnly = !chkEnableWeights.Checked;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PackingWeightCalculateAndNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingWeightCalculateAndNetWeight()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemqty.Text);
			double ebunit = Conversion.ToDouble((object)txtEbUnit.Text);
			double netweight = Conversion.ToDouble((object)txtNetWeight.Text);
			double TotalPackingWeight = Math.Round(num * ebunit, 0, MidpointRounding.AwayFromZero);
			txtEbTotalWeight.Text = Conversion.ToString((object)TotalPackingWeight);
			double ebtotalweight = Conversion.ToDouble((object)txtEbTotalWeight.Text);
			double weightafterpackingweight = netweight - ebtotalweight;
			txtWeightAfterPakingWeight.Text = Conversion.ToString((object)weightafterpackingweight);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbTotalWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PackingWeightCalculateAndNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			PackingWeightCalculateAndNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateSupplierNetWeight()
	{
		try
		{
			if (txtSupplierFirstWeight.Text != string.Empty && txtSupplierSecondWeight.Text != string.Empty)
			{
				double num = double.Parse(txtSupplierFirstWeight.Text);
				double b = double.Parse(txtSupplierSecondWeight.Text);
				double c = Math.Abs(num - b);
				if (c >= 0.0)
				{
					txtSupplierNetWeight.Text = c.ToString();
				}
			}
			else
			{
				txtSupplierNetWeight.Text = "0";
			}
			CalculateDifferenceWeights();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtLoadWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTareWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLoadWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSupplierNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateSupplierNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowFullScreen(string CameraIp, short Port, string UserName, string Password)
	{
		try
		{
			if (!string.IsNullOrEmpty(CameraIp) && Port != 0 && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
			{
				new frmLiveStreaming(CameraIp, Port, UserName, Password).ShowDialog();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PicSecondVehicle2ndTime_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (PicSecondVehicle2ndTime != null)
			{
				ShowFullScreen(IpCam03, Port03, UserNameCam03, PasswordCam03);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PicSecondUpper_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (PicSecondUpper != null)
			{
				ShowFullScreen(IpCam02, Port02, UserNameCam02, PasswordCam02);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PicSecondReading2ndtime_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (PicSecondReading2ndtime != null)
			{
				ShowFullScreen(IpCam01, Port01, UserNameCam01, PasswordCam01);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PicVehicleFront1stTime_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (PicVehicleFront1stTime != null)
			{
				ShowFullScreen(IpCam03, Port03, UserNameCam03, PasswordCam03);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void picFirstUpper_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (picFirstUpper != null)
			{
				ShowFullScreen(IpCam02, Port02, UserNameCam02, PasswordCam02);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PicFirstReading1stTime_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (PicFirstReading1stTime != null)
			{
				ShowFullScreen(IpCam01, Port01, UserNameCam01, PasswordCam01);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDifferenceWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDifferenceWeights();
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
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Expected O, but got Unknown
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Expected O, but got Unknown
		//IL_05da: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_068a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Expected O, but got Unknown
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Expected O, but got Unknown
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Expected O, but got Unknown
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Expected O, but got Unknown
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Expected O, but got Unknown
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Expected O, but got Unknown
		//IL_0a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Expected O, but got Unknown
		//IL_0a7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a88: Expected O, but got Unknown
		//IL_0a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9e: Expected O, but got Unknown
		//IL_129e: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Expected O, but got Unknown
		//IL_12b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bf: Expected O, but got Unknown
		//IL_1d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9a: Expected O, but got Unknown
		//IL_8d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d2f: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdPendingForSecond_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingFirst_DesignTimeLayout = new GridEXLayout();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
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
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WeightBridge.frmWeightbridge));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdPendingForSecond = new GridEX();
		this.FilterBoxPendingFor2nd = new System.Windows.Forms.GroupBox();
		this.BtnResetPendingFor2nd = new System.Windows.Forms.Button();
		this.BtnShowPendingFor2nd = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtToGpNoPendingFor2nd = new System.Windows.Forms.TextBox();
		this.txtFromGpNoPendingFor2nd = new System.Windows.Forms.TextBox();
		this.ToGpDatePendingFor2nd = new System.Windows.Forms.DateTimePicker();
		this.FromGpDatePendingFor2nd = new System.Windows.Forms.DateTimePicker();
		this.panel12 = new System.Windows.Forms.Panel();
		this.labelSerialPortData = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdPendingFirst = new GridEX();
		this.FilterBoxGpFor1st = new System.Windows.Forms.GroupBox();
		this.BtnResetPendingFor1st = new System.Windows.Forms.Button();
		this.btnShowPendingFor1st = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.txtToGpNoPendingFor1st = new System.Windows.Forms.TextBox();
		this.txtFromGpNoPendingFor1st = new System.Windows.Forms.TextBox();
		this.ToDatePendingFor1st = new System.Windows.Forms.DateTimePicker();
		this.FromDatePendingFor1st = new System.Windows.Forms.DateTimePicker();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.cmbVehicleNo = new UltraCombo();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtWeightDiffComments = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.PicSecondReading2ndtime = new System.Windows.Forms.PictureBox();
		this.PicSecondUpper = new System.Windows.Forms.PictureBox();
		this.PicSecondVehicle2ndTime = new System.Windows.Forms.PictureBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.PicFirstReading1stTime = new System.Windows.Forms.PictureBox();
		this.picFirstUpper = new System.Windows.Forms.PictureBox();
		this.PicVehicleFront1stTime = new System.Windows.Forms.PictureBox();
		this.txtrefdocNumberofMoveOrder = new System.Windows.Forms.TextBox();
		this.cmbRefDocNo = new UltraCombo();
		this.txtWeightReading = new System.Windows.Forms.TextBox();
		this.txtNetWtDifference = new System.Windows.Forms.TextBox();
		this.txtSupplierNetWeight = new System.Windows.Forms.TextBox();
		this.txtSecondWtDifference = new System.Windows.Forms.TextBox();
		this.txtSupplierSecondWeight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtFirstWtDifference = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSupplierFirstWeight = new System.Windows.Forms.TextBox();
		this.txtWeightAfterPakingWeight = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtEbUnit = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.CmbPartyName = new UltraCombo();
		this.cmbVehicleType = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.CmbInvoiceNo = new UltraCombo();
		this.GroupInwardLabInfo = new System.Windows.Forms.GroupBox();
		this.labInwardfirstWtCompleteCount = new System.Windows.Forms.Label();
		this.labInwardfirstWtTotalCount = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.labInwardfirstWtPendingCount = new System.Windows.Forms.Label();
		this.ChkMultiItem = new System.Windows.Forms.CheckBox();
		this.cmbPackingType = new UltraCombo();
		this.CmbWeighBridgeType = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.btnCapture = new System.Windows.Forms.Button();
		this.cmbReferenceType = new UltraCombo();
		this.btnTareReading2ndtime = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.btnVehicle2ndTimepreview = new System.Windows.Forms.Button();
		this.btnVehicle2ndTime = new System.Windows.Forms.Button();
		this.txtWorkingReportNo = new System.Windows.Forms.TextBox();
		this.btnTareForntPicpreview = new System.Windows.Forms.Button();
		this.btnTareForntPic = new System.Windows.Forms.Button();
		this.btnVehicleFront1stTimePreview = new System.Windows.Forms.Button();
		this.btnVehicleFront1stTime = new System.Windows.Forms.Button();
		this.btnGrossFontPIc = new System.Windows.Forms.Button();
		this.previewImageTareReading = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.btnGrossFontPIcPreview = new System.Windows.Forms.Button();
		this.label44 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.btnGrossReading1stTime = new System.Windows.Forms.Button();
		this.previewImageGrossReading = new System.Windows.Forms.Button();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtsecondWeight = new System.Windows.Forms.TextBox();
		this.txtFirstWeight = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtWbCharges = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtEbTotalWeight = new System.Windows.Forms.TextBox();
		this.txtItemqty = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtTickectNo = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.chkEnableWeights = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtFirstDate = new System.Windows.Forms.DateTimePicker();
		this.txtSecondDate = new System.Windows.Forms.DateTimePicker();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.ChkBoxPrintImages = new System.Windows.Forms.CheckBox();
		this.chkPrintWithLab = new System.Windows.Forms.CheckBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnWeighBridgeLookUp = new System.Windows.Forms.ToolStripButton();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.BtnPurchaseLab = new System.Windows.Forms.ToolStripButton();
		this.BtnWeightDefine = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.CmbWbTypeHistory = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbGatePassTypeHistory = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.GpNoTo = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.GPNoFrom = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.TxtVehicleNumber = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtTicketNoTo = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtTicketNoFrom = new System.Windows.Forms.TextBox();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.label14 = new System.Windows.Forms.Label();
		this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingForSecond).BeginInit();
		this.FilterBoxPendingFor2nd.SuspendLayout();
		this.panel12.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingFirst).BeginInit();
		this.FilterBoxGpFor1st.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleNo).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicSecondReading2ndtime).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicSecondUpper).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicSecondVehicle2ndTime).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.PicFirstReading1stTime).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.picFirstUpper).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicVehicleFront1stTime).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefDocNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		this.GroupInwardLabInfo.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceType).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1284, 711);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1284, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1276, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.tabControl2);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1270, 676);
		this.panel2.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 441);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1270, 235);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingForSecond);
		this.tabPage3.Controls.Add(this.FilterBoxPendingFor2nd);
		this.tabPage3.Controls.Add(this.panel12);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1262, 206);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Pending Tickets For Second Weight";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdPendingForSecond.AllowEdit = (InheritableBoolean)2;
		grdPendingForSecond_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingForSecond.DesignTimeLayout = grdPendingForSecond_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingForSecond.DynamicFiltering = true;
		this.grdPendingForSecond.FilterMode = (FilterMode)2;
		this.grdPendingForSecond.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingForSecond.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).Location = new System.Drawing.Point(3, 65);
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).Name = "grdPendingForSecond";
		((ScrollableBase)this.grdPendingForSecond).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).Size = new System.Drawing.Size(1256, 138);
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).TabIndex = 0;
		this.grdPendingForSecond.TotalRow = (InheritableBoolean)1;
		this.grdPendingForSecond.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingForSecond).VisualStyle = (VisualStyle)4;
		this.grdPendingForSecond.FormattingRow += new RowLoadEventHandler(grdfill_FormattingRow);
		this.grdPendingForSecond.ColumnButtonClick += new ColumnActionEventHandler(grdfill_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).DoubleClick += new System.EventHandler(grdfill_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPendingForSecond).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfill_KeyDown);
		this.FilterBoxPendingFor2nd.Controls.Add(this.BtnResetPendingFor2nd);
		this.FilterBoxPendingFor2nd.Controls.Add(this.BtnShowPendingFor2nd);
		this.FilterBoxPendingFor2nd.Controls.Add(this.label3);
		this.FilterBoxPendingFor2nd.Controls.Add(this.label41);
		this.FilterBoxPendingFor2nd.Controls.Add(this.label42);
		this.FilterBoxPendingFor2nd.Controls.Add(this.label43);
		this.FilterBoxPendingFor2nd.Controls.Add(this.txtToGpNoPendingFor2nd);
		this.FilterBoxPendingFor2nd.Controls.Add(this.txtFromGpNoPendingFor2nd);
		this.FilterBoxPendingFor2nd.Controls.Add(this.ToGpDatePendingFor2nd);
		this.FilterBoxPendingFor2nd.Controls.Add(this.FromGpDatePendingFor2nd);
		this.FilterBoxPendingFor2nd.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterBoxPendingFor2nd.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.FilterBoxPendingFor2nd.Location = new System.Drawing.Point(3, 24);
		this.FilterBoxPendingFor2nd.Name = "FilterBoxPendingFor2nd";
		this.FilterBoxPendingFor2nd.Size = new System.Drawing.Size(1256, 41);
		this.FilterBoxPendingFor2nd.TabIndex = 0;
		this.FilterBoxPendingFor2nd.TabStop = false;
		this.FilterBoxPendingFor2nd.Text = "Filters";
		this.BtnResetPendingFor2nd.BackColor = System.Drawing.Color.Teal;
		this.BtnResetPendingFor2nd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetPendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetPendingFor2nd.ForeColor = System.Drawing.Color.White;
		this.BtnResetPendingFor2nd.Location = new System.Drawing.Point(824, 11);
		this.BtnResetPendingFor2nd.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetPendingFor2nd.Name = "BtnResetPendingFor2nd";
		this.BtnResetPendingFor2nd.Size = new System.Drawing.Size(61, 24);
		this.BtnResetPendingFor2nd.TabIndex = 5;
		this.BtnResetPendingFor2nd.Text = "Reset";
		this.BtnResetPendingFor2nd.UseVisualStyleBackColor = false;
		this.BtnResetPendingFor2nd.Click += new System.EventHandler(BtnResetPendingFor2nd_Click);
		this.BtnShowPendingFor2nd.BackColor = System.Drawing.Color.Teal;
		this.BtnShowPendingFor2nd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowPendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowPendingFor2nd.ForeColor = System.Drawing.Color.White;
		this.BtnShowPendingFor2nd.Location = new System.Drawing.Point(761, 11);
		this.BtnShowPendingFor2nd.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowPendingFor2nd.Name = "BtnShowPendingFor2nd";
		this.BtnShowPendingFor2nd.Size = new System.Drawing.Size(62, 24);
		this.BtnShowPendingFor2nd.TabIndex = 4;
		this.BtnShowPendingFor2nd.Text = "Show";
		this.BtnShowPendingFor2nd.UseVisualStyleBackColor = false;
		this.BtnShowPendingFor2nd.Click += new System.EventHandler(BtnShowPendingFor2nd_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(599, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 86;
		this.label3.Text = "To Gp No";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(421, 16);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(72, 15);
		this.label41.TabIndex = 85;
		this.label41.Text = "From Gp No";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(221, 16);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 84;
		this.label42.Text = "To Gp Date";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(3, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(81, 15);
		this.label43.TabIndex = 83;
		this.label43.Text = "From Gp Date";
		this.txtToGpNoPendingFor2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToGpNoPendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToGpNoPendingFor2nd.Location = new System.Drawing.Point(657, 12);
		this.txtToGpNoPendingFor2nd.Name = "txtToGpNoPendingFor2nd";
		this.txtToGpNoPendingFor2nd.ShortcutsEnabled = false;
		this.txtToGpNoPendingFor2nd.Size = new System.Drawing.Size(102, 22);
		this.txtToGpNoPendingFor2nd.TabIndex = 3;
		this.txtFromGpNoPendingFor2nd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromGpNoPendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromGpNoPendingFor2nd.Location = new System.Drawing.Point(493, 12);
		this.txtFromGpNoPendingFor2nd.Name = "txtFromGpNoPendingFor2nd";
		this.txtFromGpNoPendingFor2nd.ShortcutsEnabled = false;
		this.txtFromGpNoPendingFor2nd.Size = new System.Drawing.Size(102, 22);
		this.txtFromGpNoPendingFor2nd.TabIndex = 2;
		this.ToGpDatePendingFor2nd.CustomFormat = "dd-MMM-yy";
		this.ToGpDatePendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToGpDatePendingFor2nd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToGpDatePendingFor2nd.Location = new System.Drawing.Point(287, 12);
		this.ToGpDatePendingFor2nd.Name = "ToGpDatePendingFor2nd";
		this.ToGpDatePendingFor2nd.ShowCheckBox = true;
		this.ToGpDatePendingFor2nd.Size = new System.Drawing.Size(133, 22);
		this.ToGpDatePendingFor2nd.TabIndex = 1;
		this.FromGpDatePendingFor2nd.CustomFormat = "dd-MMM-yy";
		this.FromGpDatePendingFor2nd.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromGpDatePendingFor2nd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromGpDatePendingFor2nd.Location = new System.Drawing.Point(85, 12);
		this.FromGpDatePendingFor2nd.Name = "FromGpDatePendingFor2nd";
		this.FromGpDatePendingFor2nd.ShowCheckBox = true;
		this.FromGpDatePendingFor2nd.Size = new System.Drawing.Size(133, 22);
		this.FromGpDatePendingFor2nd.TabIndex = 0;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.labelSerialPortData);
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1256, 21);
		this.panel12.TabIndex = 5;
		this.labelSerialPortData.AutoSize = true;
		this.labelSerialPortData.Font = new System.Drawing.Font("Verdana", 9f);
		this.labelSerialPortData.ForeColor = System.Drawing.Color.White;
		this.labelSerialPortData.Location = new System.Drawing.Point(3, 3);
		this.labelSerialPortData.Name = "labelSerialPortData";
		this.labelSerialPortData.Size = new System.Drawing.Size(106, 14);
		this.labelSerialPortData.TabIndex = 21;
		this.labelSerialPortData.Text = "Serial Port Data";
		this.labelSerialPortData.Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1212, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdPendingForSecond;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingFirst);
		this.tabPage4.Controls.Add(this.FilterBoxGpFor1st);
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1262, 206);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Pending Gate Pass For First Weight";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdPendingFirst.AllowEdit = (InheritableBoolean)2;
		grdPendingFirst_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingFirst.DesignTimeLayout = grdPendingFirst_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingFirst.DynamicFiltering = true;
		this.grdPendingFirst.FilterMode = (FilterMode)2;
		this.grdPendingFirst.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingFirst.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).Location = new System.Drawing.Point(3, 59);
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).Name = "grdPendingFirst";
		((ScrollableBase)this.grdPendingFirst).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).Size = new System.Drawing.Size(1256, 144);
		((System.Windows.Forms.Control)(object)this.grdPendingFirst).TabIndex = 2;
		this.grdPendingFirst.TotalRow = (InheritableBoolean)1;
		this.grdPendingFirst.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingFirst).VisualStyle = (VisualStyle)4;
		this.grdPendingFirst.FormattingRow += new RowLoadEventHandler(grdPendingGpForWeighBridge_FormattingRow);
		this.FilterBoxGpFor1st.Controls.Add(this.BtnResetPendingFor1st);
		this.FilterBoxGpFor1st.Controls.Add(this.btnShowPendingFor1st);
		this.FilterBoxGpFor1st.Controls.Add(this.label51);
		this.FilterBoxGpFor1st.Controls.Add(this.label52);
		this.FilterBoxGpFor1st.Controls.Add(this.label53);
		this.FilterBoxGpFor1st.Controls.Add(this.label54);
		this.FilterBoxGpFor1st.Controls.Add(this.txtToGpNoPendingFor1st);
		this.FilterBoxGpFor1st.Controls.Add(this.txtFromGpNoPendingFor1st);
		this.FilterBoxGpFor1st.Controls.Add(this.ToDatePendingFor1st);
		this.FilterBoxGpFor1st.Controls.Add(this.FromDatePendingFor1st);
		this.FilterBoxGpFor1st.Dock = System.Windows.Forms.DockStyle.Top;
		this.FilterBoxGpFor1st.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.FilterBoxGpFor1st.Location = new System.Drawing.Point(3, 22);
		this.FilterBoxGpFor1st.Name = "FilterBoxGpFor1st";
		this.FilterBoxGpFor1st.Size = new System.Drawing.Size(1256, 37);
		this.FilterBoxGpFor1st.TabIndex = 6;
		this.FilterBoxGpFor1st.TabStop = false;
		this.FilterBoxGpFor1st.Text = "Filters";
		this.BtnResetPendingFor1st.BackColor = System.Drawing.Color.Teal;
		this.BtnResetPendingFor1st.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetPendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetPendingFor1st.ForeColor = System.Drawing.Color.White;
		this.BtnResetPendingFor1st.Location = new System.Drawing.Point(867, 11);
		this.BtnResetPendingFor1st.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetPendingFor1st.Name = "BtnResetPendingFor1st";
		this.BtnResetPendingFor1st.Size = new System.Drawing.Size(61, 23);
		this.BtnResetPendingFor1st.TabIndex = 5;
		this.BtnResetPendingFor1st.Text = "Reset";
		this.BtnResetPendingFor1st.UseVisualStyleBackColor = false;
		this.BtnResetPendingFor1st.Click += new System.EventHandler(BtnResetPendingFor1st_Click);
		this.btnShowPendingFor1st.BackColor = System.Drawing.Color.Teal;
		this.btnShowPendingFor1st.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowPendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowPendingFor1st.ForeColor = System.Drawing.Color.White;
		this.btnShowPendingFor1st.Location = new System.Drawing.Point(801, 11);
		this.btnShowPendingFor1st.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowPendingFor1st.Name = "btnShowPendingFor1st";
		this.btnShowPendingFor1st.Size = new System.Drawing.Size(65, 23);
		this.btnShowPendingFor1st.TabIndex = 5;
		this.btnShowPendingFor1st.Text = "Show";
		this.btnShowPendingFor1st.UseVisualStyleBackColor = false;
		this.btnShowPendingFor1st.Click += new System.EventHandler(btnShowPendingFor1st_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(637, 15);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(57, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Gp No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(458, 15);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(72, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Gp No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(238, 15);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(83, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Ticket Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 15);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(98, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Ticket Date";
		this.txtToGpNoPendingFor1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToGpNoPendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToGpNoPendingFor1st.Location = new System.Drawing.Point(697, 11);
		this.txtToGpNoPendingFor1st.Name = "txtToGpNoPendingFor1st";
		this.txtToGpNoPendingFor1st.ShortcutsEnabled = false;
		this.txtToGpNoPendingFor1st.Size = new System.Drawing.Size(102, 22);
		this.txtToGpNoPendingFor1st.TabIndex = 3;
		this.txtFromGpNoPendingFor1st.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromGpNoPendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromGpNoPendingFor1st.Location = new System.Drawing.Point(531, 11);
		this.txtFromGpNoPendingFor1st.Name = "txtFromGpNoPendingFor1st";
		this.txtFromGpNoPendingFor1st.ShortcutsEnabled = false;
		this.txtFromGpNoPendingFor1st.Size = new System.Drawing.Size(102, 22);
		this.txtFromGpNoPendingFor1st.TabIndex = 2;
		this.ToDatePendingFor1st.CustomFormat = "dd-MMM-yy";
		this.ToDatePendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDatePendingFor1st.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDatePendingFor1st.Location = new System.Drawing.Point(324, 11);
		this.ToDatePendingFor1st.Name = "ToDatePendingFor1st";
		this.ToDatePendingFor1st.ShowCheckBox = true;
		this.ToDatePendingFor1st.Size = new System.Drawing.Size(133, 22);
		this.ToDatePendingFor1st.TabIndex = 1;
		this.FromDatePendingFor1st.CustomFormat = "dd-MMM-yy";
		this.FromDatePendingFor1st.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDatePendingFor1st.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDatePendingFor1st.Location = new System.Drawing.Point(102, 11);
		this.FromDatePendingFor1st.Name = "FromDatePendingFor1st";
		this.FromDatePendingFor1st.ShowCheckBox = true;
		this.FromDatePendingFor1st.Size = new System.Drawing.Size(133, 22);
		this.FromDatePendingFor1st.TabIndex = 0;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Margin = new System.Windows.Forms.Padding(2);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1256, 19);
		this.panel13.TabIndex = 5;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleNo);
		this.panel3.Controls.Add(this.txtRemarks);
		this.panel3.Controls.Add(this.txtWeightDiffComments);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.txtrefdocNumberofMoveOrder);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRefDocNo);
		this.panel3.Controls.Add(this.txtWeightReading);
		this.panel3.Controls.Add(this.txtNetWtDifference);
		this.panel3.Controls.Add(this.txtSupplierNetWeight);
		this.panel3.Controls.Add(this.txtSecondWtDifference);
		this.panel3.Controls.Add(this.txtSupplierSecondWeight);
		this.panel3.Controls.Add(this.label39);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.txtFirstWtDifference);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtSupplierFirstWeight);
		this.panel3.Controls.Add(this.txtWeightAfterPakingWeight);
		this.panel3.Controls.Add(this.label40);
		this.panel3.Controls.Add(this.txtEbUnit);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.txtVehicleNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel3.Controls.Add(this.GroupInwardLabInfo);
		this.panel3.Controls.Add(this.ChkMultiItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType);
		this.panel3.Controls.Add(this.label31);
		this.panel3.Controls.Add(this.btnCapture);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceType);
		this.panel3.Controls.Add(this.btnTareReading2ndtime);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.btnVehicle2ndTimepreview);
		this.panel3.Controls.Add(this.btnVehicle2ndTime);
		this.panel3.Controls.Add(this.txtWorkingReportNo);
		this.panel3.Controls.Add(this.btnTareForntPicpreview);
		this.panel3.Controls.Add(this.btnTareForntPic);
		this.panel3.Controls.Add(this.btnVehicleFront1stTimePreview);
		this.panel3.Controls.Add(this.btnVehicleFront1stTime);
		this.panel3.Controls.Add(this.btnGrossFontPIc);
		this.panel3.Controls.Add(this.previewImageTareReading);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.btnGrossFontPIcPreview);
		this.panel3.Controls.Add(this.label44);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.btnGrossReading1stTime);
		this.panel3.Controls.Add(this.previewImageGrossReading);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.txtNetWeight);
		this.panel3.Controls.Add(this.txtsecondWeight);
		this.panel3.Controls.Add(this.txtFirstWeight);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.txtWbCharges);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtContainerNo);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtEbTotalWeight);
		this.panel3.Controls.Add(this.txtItemqty);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtBilityNo);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtTickectNo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 57);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1270, 384);
		this.panel3.TabIndex = 0;
		this.cmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Location = new System.Drawing.Point(92, 114);
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Name = "cmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Size = new System.Drawing.Size(98, 25);
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).TextChanged += new System.EventHandler(cmbVehicleNo_TextChanged);
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(92, 274);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(601, 23);
		this.txtRemarks.TabIndex = 12;
		this.txtWeightDiffComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightDiffComments.Location = new System.Drawing.Point(92, 299);
		this.txtWeightDiffComments.Name = "txtWeightDiffComments";
		this.txtWeightDiffComments.Size = new System.Drawing.Size(510, 23);
		this.txtWeightDiffComments.TabIndex = 13;
		this.groupBox2.Controls.Add(this.PicSecondReading2ndtime);
		this.groupBox2.Controls.Add(this.PicSecondUpper);
		this.groupBox2.Controls.Add(this.PicSecondVehicle2ndTime);
		this.groupBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(696, 192);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(576, 190);
		this.groupBox2.TabIndex = 30;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Second Weight Images";
		this.PicSecondReading2ndtime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicSecondReading2ndtime.Location = new System.Drawing.Point(2, 19);
		this.PicSecondReading2ndtime.Name = "PicSecondReading2ndtime";
		this.PicSecondReading2ndtime.Size = new System.Drawing.Size(190, 165);
		this.PicSecondReading2ndtime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicSecondReading2ndtime.TabIndex = 37;
		this.PicSecondReading2ndtime.TabStop = false;
		this.PicSecondReading2ndtime.DoubleClick += new System.EventHandler(PicSecondReading2ndtime_DoubleClick);
		this.PicSecondUpper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicSecondUpper.Location = new System.Drawing.Point(193, 19);
		this.PicSecondUpper.Name = "PicSecondUpper";
		this.PicSecondUpper.Size = new System.Drawing.Size(190, 165);
		this.PicSecondUpper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicSecondUpper.TabIndex = 38;
		this.PicSecondUpper.TabStop = false;
		this.PicSecondUpper.DoubleClick += new System.EventHandler(PicSecondUpper_DoubleClick);
		this.PicSecondVehicle2ndTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicSecondVehicle2ndTime.Location = new System.Drawing.Point(384, 19);
		this.PicSecondVehicle2ndTime.Name = "PicSecondVehicle2ndTime";
		this.PicSecondVehicle2ndTime.Size = new System.Drawing.Size(190, 165);
		this.PicSecondVehicle2ndTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicSecondVehicle2ndTime.TabIndex = 39;
		this.PicSecondVehicle2ndTime.TabStop = false;
		this.PicSecondVehicle2ndTime.DoubleClick += new System.EventHandler(PicSecondVehicle2ndTime_DoubleClick);
		this.groupBox1.Controls.Add(this.PicFirstReading1stTime);
		this.groupBox1.Controls.Add(this.picFirstUpper);
		this.groupBox1.Controls.Add(this.PicVehicleFront1stTime);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(696, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(576, 188);
		this.groupBox1.TabIndex = 29;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "First Weight Images";
		this.PicFirstReading1stTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicFirstReading1stTime.Location = new System.Drawing.Point(2, 18);
		this.PicFirstReading1stTime.Name = "PicFirstReading1stTime";
		this.PicFirstReading1stTime.Size = new System.Drawing.Size(190, 165);
		this.PicFirstReading1stTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicFirstReading1stTime.TabIndex = 29;
		this.PicFirstReading1stTime.TabStop = false;
		this.PicFirstReading1stTime.DoubleClick += new System.EventHandler(PicFirstReading1stTime_DoubleClick);
		this.picFirstUpper.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picFirstUpper.Location = new System.Drawing.Point(193, 18);
		this.picFirstUpper.Name = "picFirstUpper";
		this.picFirstUpper.Size = new System.Drawing.Size(190, 165);
		this.picFirstUpper.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFirstUpper.TabIndex = 31;
		this.picFirstUpper.TabStop = false;
		this.picFirstUpper.DoubleClick += new System.EventHandler(picFirstUpper_DoubleClick);
		this.PicVehicleFront1stTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicVehicleFront1stTime.Location = new System.Drawing.Point(384, 18);
		this.PicVehicleFront1stTime.Name = "PicVehicleFront1stTime";
		this.PicVehicleFront1stTime.Size = new System.Drawing.Size(190, 165);
		this.PicVehicleFront1stTime.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicVehicleFront1stTime.TabIndex = 33;
		this.PicVehicleFront1stTime.TabStop = false;
		this.PicVehicleFront1stTime.DoubleClick += new System.EventHandler(PicVehicleFront1stTime_DoubleClick);
		this.txtrefdocNumberofMoveOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrefdocNumberofMoveOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefdocNumberofMoveOrder.Location = new System.Drawing.Point(91, 32);
		this.txtrefdocNumberofMoveOrder.Name = "txtrefdocNumberofMoveOrder";
		this.txtrefdocNumberofMoveOrder.Size = new System.Drawing.Size(99, 23);
		this.txtrefdocNumberofMoveOrder.TabIndex = 1;
		this.txtrefdocNumberofMoveOrder.Visible = false;
		this.txtrefdocNumberofMoveOrder.Leave += new System.EventHandler(cmbRefDocNo_Leave);
		this.cmbRefDocNo.AllowNull = (DefaultableBoolean)1;
		this.cmbRefDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRefDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRefDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.cmbRefDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRefDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRefDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).Location = new System.Drawing.Point(91, 30);
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).Name = "cmbRefDocNo";
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).Size = new System.Drawing.Size(99, 26);
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbRefDocNo).Leave += new System.EventHandler(cmbRefDocNo_Leave);
		this.txtWeightReading.BackColor = System.Drawing.Color.Beige;
		this.txtWeightReading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightReading.ForeColor = System.Drawing.Color.Blue;
		this.txtWeightReading.Location = new System.Drawing.Point(390, 3);
		this.txtWeightReading.Name = "txtWeightReading";
		this.txtWeightReading.ReadOnly = true;
		this.txtWeightReading.Size = new System.Drawing.Size(303, 38);
		this.txtWeightReading.TabIndex = 14;
		this.txtWeightReading.TabStop = false;
		this.txtNetWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWtDifference.Enabled = false;
		this.txtNetWtDifference.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtNetWtDifference.Location = new System.Drawing.Point(603, 134);
		this.txtNetWtDifference.Name = "txtNetWtDifference";
		this.txtNetWtDifference.Size = new System.Drawing.Size(90, 32);
		this.txtNetWtDifference.TabIndex = 20;
		this.txtNetWtDifference.Tag = "double";
		this.txtSupplierNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierNetWeight.Enabled = false;
		this.txtSupplierNetWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierNetWeight.Location = new System.Drawing.Point(496, 134);
		this.txtSupplierNetWeight.Name = "txtSupplierNetWeight";
		this.txtSupplierNetWeight.Size = new System.Drawing.Size(105, 32);
		this.txtSupplierNetWeight.TabIndex = 20;
		this.txtSupplierNetWeight.Tag = "double";
		this.txtSupplierNetWeight.TextChanged += new System.EventHandler(txtSupplierNetWeight_TextChanged);
		this.txtSecondWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondWtDifference.Enabled = false;
		this.txtSecondWtDifference.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtSecondWtDifference.Location = new System.Drawing.Point(603, 100);
		this.txtSecondWtDifference.Name = "txtSecondWtDifference";
		this.txtSecondWtDifference.Size = new System.Drawing.Size(90, 32);
		this.txtSecondWtDifference.TabIndex = 19;
		this.txtSecondWtDifference.Tag = "double";
		this.txtSecondWtDifference.TextChanged += new System.EventHandler(txtTareWeight_TextChanged);
		this.txtSecondWtDifference.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTareWeight_KeyPress);
		this.txtSupplierSecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierSecondWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierSecondWeight.Location = new System.Drawing.Point(496, 100);
		this.txtSupplierSecondWeight.Name = "txtSupplierSecondWeight";
		this.txtSupplierSecondWeight.Size = new System.Drawing.Size(105, 32);
		this.txtSupplierSecondWeight.TabIndex = 19;
		this.txtSupplierSecondWeight.Tag = "double";
		this.txtSupplierSecondWeight.TextChanged += new System.EventHandler(txtTareWeight_TextChanged);
		this.txtSupplierSecondWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTareWeight_KeyPress);
		this.label39.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(603, 43);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(90, 20);
		this.label39.TabIndex = 135;
		this.label39.Text = "Difference";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label45.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(496, 43);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(105, 20);
		this.label45.TabIndex = 135;
		this.label45.Text = "Supplier";
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFirstWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWtDifference.Enabled = false;
		this.txtFirstWtDifference.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtFirstWtDifference.Location = new System.Drawing.Point(603, 66);
		this.txtFirstWtDifference.Name = "txtFirstWtDifference";
		this.txtFirstWtDifference.Size = new System.Drawing.Size(90, 32);
		this.txtFirstWtDifference.TabIndex = 18;
		this.txtFirstWtDifference.Tag = "double";
		this.txtFirstWtDifference.TextChanged += new System.EventHandler(txtLoadWeight_TextChanged);
		this.txtFirstWtDifference.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLoadWeight_KeyPress);
		this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(390, 43);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(105, 20);
		this.label1.TabIndex = 135;
		this.label1.Text = "Factory";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSupplierFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierFirstWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierFirstWeight.Location = new System.Drawing.Point(496, 66);
		this.txtSupplierFirstWeight.Name = "txtSupplierFirstWeight";
		this.txtSupplierFirstWeight.Size = new System.Drawing.Size(105, 32);
		this.txtSupplierFirstWeight.TabIndex = 18;
		this.txtSupplierFirstWeight.Tag = "double";
		this.txtSupplierFirstWeight.TextChanged += new System.EventHandler(txtLoadWeight_TextChanged);
		this.txtSupplierFirstWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLoadWeight_KeyPress);
		this.txtWeightAfterPakingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightAfterPakingWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightAfterPakingWeight.Location = new System.Drawing.Point(573, 249);
		this.txtWeightAfterPakingWeight.Name = "txtWeightAfterPakingWeight";
		this.txtWeightAfterPakingWeight.ReadOnly = true;
		this.txtWeightAfterPakingWeight.Size = new System.Drawing.Size(120, 23);
		this.txtWeightAfterPakingWeight.TabIndex = 26;
		this.txtWeightAfterPakingWeight.Tag = "double";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(508, 253);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(68, 15);
		this.label40.TabIndex = 132;
		this.label40.Text = "Net Weight";
		this.txtEbUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbUnit.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbUnit.Location = new System.Drawing.Point(573, 224);
		this.txtEbUnit.Name = "txtEbUnit";
		this.txtEbUnit.Size = new System.Drawing.Size(120, 23);
		this.txtEbUnit.TabIndex = 24;
		this.txtEbUnit.Tag = "double";
		this.txtEbUnit.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(509, 228);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(45, 15);
		this.label6.TabIndex = 132;
		this.label6.Text = "Eb Unit";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(92, 115);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(98, 23);
		this.txtVehicleNo.TabIndex = 5;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(92, 168);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Leave += new System.EventHandler(CmbPartyName_Leave);
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(92, 85);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 4;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(390, 168);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(303, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 21;
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbInvoiceNo.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(92, 140);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 7;
		this.GroupInwardLabInfo.Controls.Add(this.labInwardfirstWtCompleteCount);
		this.GroupInwardLabInfo.Controls.Add(this.labInwardfirstWtTotalCount);
		this.GroupInwardLabInfo.Controls.Add(this.label29);
		this.GroupInwardLabInfo.Controls.Add(this.label28);
		this.GroupInwardLabInfo.Controls.Add(this.label27);
		this.GroupInwardLabInfo.Controls.Add(this.labInwardfirstWtPendingCount);
		this.GroupInwardLabInfo.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.GroupInwardLabInfo.Location = new System.Drawing.Point(91, 322);
		this.GroupInwardLabInfo.Name = "GroupInwardLabInfo";
		this.GroupInwardLabInfo.Size = new System.Drawing.Size(602, 60);
		this.GroupInwardLabInfo.TabIndex = 28;
		this.GroupInwardLabInfo.TabStop = false;
		this.GroupInwardLabInfo.Visible = false;
		this.labInwardfirstWtCompleteCount.AutoSize = true;
		this.labInwardfirstWtCompleteCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labInwardfirstWtCompleteCount.Location = new System.Drawing.Point(250, 39);
		this.labInwardfirstWtCompleteCount.Name = "labInwardfirstWtCompleteCount";
		this.labInwardfirstWtCompleteCount.Size = new System.Drawing.Size(50, 16);
		this.labInwardfirstWtCompleteCount.TabIndex = 22;
		this.labInwardfirstWtCompleteCount.Text = "Count";
		this.labInwardfirstWtCompleteCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.labInwardfirstWtTotalCount.AutoSize = true;
		this.labInwardfirstWtTotalCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labInwardfirstWtTotalCount.Location = new System.Drawing.Point(37, 39);
		this.labInwardfirstWtTotalCount.Name = "labInwardfirstWtTotalCount";
		this.labInwardfirstWtTotalCount.Size = new System.Drawing.Size(50, 16);
		this.labInwardfirstWtTotalCount.TabIndex = 22;
		this.labInwardfirstWtTotalCount.Text = "Count";
		this.labInwardfirstWtTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.label29.BackColor = System.Drawing.Color.Red;
		this.label29.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label29.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(431, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(168, 20);
		this.label29.TabIndex = 22;
		this.label29.Text = "Pending For Acceptance";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label28.BackColor = System.Drawing.Color.Green;
		this.label28.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label28.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(122, 16);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(306, 20);
		this.label28.TabIndex = 22;
		this.label28.Text = "Accepted Lab Done && Pending For First Weight";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label27.BackColor = System.Drawing.Color.FromArgb(0, 192, 192);
		this.label27.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label27.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.Black;
		this.label27.Location = new System.Drawing.Point(5, 16);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(114, 20);
		this.label27.TabIndex = 22;
		this.label27.Text = "Total Lab Done";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.labInwardfirstWtPendingCount.AutoSize = true;
		this.labInwardfirstWtPendingCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labInwardfirstWtPendingCount.Location = new System.Drawing.Point(490, 39);
		this.labInwardfirstWtPendingCount.Name = "labInwardfirstWtPendingCount";
		this.labInwardfirstWtPendingCount.Size = new System.Drawing.Size(50, 16);
		this.labInwardfirstWtPendingCount.TabIndex = 22;
		this.labInwardfirstWtPendingCount.Text = "Count";
		this.labInwardfirstWtPendingCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.ChkMultiItem.AutoSize = true;
		this.ChkMultiItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkMultiItem.Location = new System.Drawing.Point(606, 303);
		this.ChkMultiItem.Margin = new System.Windows.Forms.Padding(2);
		this.ChkMultiItem.Name = "ChkMultiItem";
		this.ChkMultiItem.Size = new System.Drawing.Size(87, 19);
		this.ChkMultiItem.TabIndex = 27;
		this.ChkMultiItem.Text = "Multi Items";
		this.ChkMultiItem.UseVisualStyleBackColor = true;
		this.ChkMultiItem.Visible = false;
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(390, 196);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(303, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TextChanged += new System.EventHandler(cmbVehicleNo_TextChanged);
		this.CmbWeighBridgeType.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridgeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridgeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridgeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbWeighBridgeType.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWeighBridgeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Location = new System.Drawing.Point(92, 58);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Name = "CmbWeighBridgeType";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).TabIndex = 3;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(8, 64);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(54, 15);
		this.label31.TabIndex = 130;
		this.label31.Text = "Wb Type";
		this.btnCapture.Location = new System.Drawing.Point(1459, 450);
		this.btnCapture.Name = "btnCapture";
		this.btnCapture.Size = new System.Drawing.Size(75, 30);
		this.btnCapture.TabIndex = 54;
		this.btnCapture.Text = "Capture";
		this.btnCapture.UseVisualStyleBackColor = true;
		this.btnCapture.Visible = false;
		this.btnCapture.Click += new System.EventHandler(btnCapture_Click);
		this.cmbReferenceType.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReferenceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Location = new System.Drawing.Point(91, 3);
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Name = "cmbReferenceType";
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Leave += new System.EventHandler(cmbReferenceType_Leave);
		this.btnTareReading2ndtime.Location = new System.Drawing.Point(1406, 486);
		this.btnTareReading2ndtime.Name = "btnTareReading2ndtime";
		this.btnTareReading2ndtime.Size = new System.Drawing.Size(61, 23);
		this.btnTareReading2ndtime.TabIndex = 0;
		this.btnTareReading2ndtime.TabStop = false;
		this.btnTareReading2ndtime.Text = "Browse";
		this.btnTareReading2ndtime.UseVisualStyleBackColor = true;
		this.btnTareReading2ndtime.Visible = false;
		this.btnTareReading2ndtime.Click += new System.EventHandler(btnTareReading2ndtime_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(8, 295);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 30);
		this.label24.TabIndex = 52;
		this.label24.Text = "Wt Diff \r\nRemarks";
		this.btnVehicle2ndTimepreview.Location = new System.Drawing.Point(1473, 544);
		this.btnVehicle2ndTimepreview.Name = "btnVehicle2ndTimepreview";
		this.btnVehicle2ndTimepreview.Size = new System.Drawing.Size(61, 23);
		this.btnVehicle2ndTimepreview.TabIndex = 5;
		this.btnVehicle2ndTimepreview.TabStop = false;
		this.btnVehicle2ndTimepreview.Text = "Preview";
		this.btnVehicle2ndTimepreview.UseVisualStyleBackColor = true;
		this.btnVehicle2ndTimepreview.Visible = false;
		this.btnVehicle2ndTimepreview.Click += new System.EventHandler(btnVehicle2ndTimepreview_Click);
		this.btnVehicle2ndTime.Location = new System.Drawing.Point(1406, 544);
		this.btnVehicle2ndTime.Name = "btnVehicle2ndTime";
		this.btnVehicle2ndTime.Size = new System.Drawing.Size(61, 23);
		this.btnVehicle2ndTime.TabIndex = 4;
		this.btnVehicle2ndTime.TabStop = false;
		this.btnVehicle2ndTime.Text = "Browse";
		this.btnVehicle2ndTime.UseVisualStyleBackColor = true;
		this.btnVehicle2ndTime.Visible = false;
		this.btnVehicle2ndTime.Click += new System.EventHandler(btnVehicle2ndTime_Click);
		this.txtWorkingReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkingReportNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkingReportNo.Location = new System.Drawing.Point(92, 249);
		this.txtWorkingReportNo.Name = "txtWorkingReportNo";
		this.txtWorkingReportNo.Size = new System.Drawing.Size(200, 23);
		this.txtWorkingReportNo.TabIndex = 11;
		this.txtWorkingReportNo.Tag = "int";
		this.txtWorkingReportNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWorkingReportNo_KeyPress);
		this.btnTareForntPicpreview.Location = new System.Drawing.Point(1473, 515);
		this.btnTareForntPicpreview.Name = "btnTareForntPicpreview";
		this.btnTareForntPicpreview.Size = new System.Drawing.Size(61, 23);
		this.btnTareForntPicpreview.TabIndex = 3;
		this.btnTareForntPicpreview.TabStop = false;
		this.btnTareForntPicpreview.Text = "Preview";
		this.btnTareForntPicpreview.UseVisualStyleBackColor = true;
		this.btnTareForntPicpreview.Visible = false;
		this.btnTareForntPicpreview.Click += new System.EventHandler(btnTareForntPicpreview_Click);
		this.btnTareForntPic.Location = new System.Drawing.Point(1406, 515);
		this.btnTareForntPic.Name = "btnTareForntPic";
		this.btnTareForntPic.Size = new System.Drawing.Size(61, 23);
		this.btnTareForntPic.TabIndex = 2;
		this.btnTareForntPic.TabStop = false;
		this.btnTareForntPic.Text = "Browse";
		this.btnTareForntPic.UseVisualStyleBackColor = true;
		this.btnTareForntPic.Visible = false;
		this.btnTareForntPic.Click += new System.EventHandler(btnTareForntPic_Click);
		this.btnVehicleFront1stTimePreview.Location = new System.Drawing.Point(1329, 544);
		this.btnVehicleFront1stTimePreview.Name = "btnVehicleFront1stTimePreview";
		this.btnVehicleFront1stTimePreview.Size = new System.Drawing.Size(71, 23);
		this.btnVehicleFront1stTimePreview.TabIndex = 5;
		this.btnVehicleFront1stTimePreview.TabStop = false;
		this.btnVehicleFront1stTimePreview.Text = "Preview";
		this.btnVehicleFront1stTimePreview.UseVisualStyleBackColor = true;
		this.btnVehicleFront1stTimePreview.Visible = false;
		this.btnVehicleFront1stTimePreview.Click += new System.EventHandler(btnVehicleFront1stTimePreview_Click);
		this.btnVehicleFront1stTime.Location = new System.Drawing.Point(1262, 544);
		this.btnVehicleFront1stTime.Name = "btnVehicleFront1stTime";
		this.btnVehicleFront1stTime.Size = new System.Drawing.Size(61, 23);
		this.btnVehicleFront1stTime.TabIndex = 4;
		this.btnVehicleFront1stTime.TabStop = false;
		this.btnVehicleFront1stTime.Text = "Browse";
		this.btnVehicleFront1stTime.UseVisualStyleBackColor = true;
		this.btnVehicleFront1stTime.Visible = false;
		this.btnVehicleFront1stTime.Click += new System.EventHandler(btnVehicleFront1stTime_Click);
		this.btnGrossFontPIc.Location = new System.Drawing.Point(1262, 515);
		this.btnGrossFontPIc.Name = "btnGrossFontPIc";
		this.btnGrossFontPIc.Size = new System.Drawing.Size(61, 23);
		this.btnGrossFontPIc.TabIndex = 2;
		this.btnGrossFontPIc.TabStop = false;
		this.btnGrossFontPIc.Text = "Browse";
		this.btnGrossFontPIc.UseVisualStyleBackColor = true;
		this.btnGrossFontPIc.Visible = false;
		this.btnGrossFontPIc.Click += new System.EventHandler(btnGrossFontPIc_Click);
		this.previewImageTareReading.Location = new System.Drawing.Point(1473, 486);
		this.previewImageTareReading.Name = "previewImageTareReading";
		this.previewImageTareReading.Size = new System.Drawing.Size(61, 23);
		this.previewImageTareReading.TabIndex = 1;
		this.previewImageTareReading.TabStop = false;
		this.previewImageTareReading.Text = "Preview";
		this.previewImageTareReading.UseVisualStyleBackColor = true;
		this.previewImageTareReading.Visible = false;
		this.previewImageTareReading.Click += new System.EventHandler(previewImageTareReading_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(298, 97);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(63, 40);
		this.label22.TabIndex = 49;
		this.label22.Text = "Second \r\nWeight";
		this.btnGrossFontPIcPreview.Location = new System.Drawing.Point(1329, 515);
		this.btnGrossFontPIcPreview.Name = "btnGrossFontPIcPreview";
		this.btnGrossFontPIcPreview.Size = new System.Drawing.Size(71, 22);
		this.btnGrossFontPIcPreview.TabIndex = 3;
		this.btnGrossFontPIcPreview.TabStop = false;
		this.btnGrossFontPIcPreview.Text = "Preview";
		this.btnGrossFontPIcPreview.UseVisualStyleBackColor = true;
		this.btnGrossFontPIcPreview.Visible = false;
		this.btnGrossFontPIcPreview.Click += new System.EventHandler(btnGrossFontPIcPreview_Click);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(294, 5);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(99, 40);
		this.label44.TabIndex = 48;
		this.label44.Text = "Factory Scale\r\nReading";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(298, 73);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(91, 20);
		this.label21.TabIndex = 48;
		this.label21.Text = "First Weight";
		this.btnGrossReading1stTime.Location = new System.Drawing.Point(1262, 486);
		this.btnGrossReading1stTime.Name = "btnGrossReading1stTime";
		this.btnGrossReading1stTime.Size = new System.Drawing.Size(61, 23);
		this.btnGrossReading1stTime.TabIndex = 0;
		this.btnGrossReading1stTime.TabStop = false;
		this.btnGrossReading1stTime.Text = "Browse";
		this.btnGrossReading1stTime.UseVisualStyleBackColor = true;
		this.btnGrossReading1stTime.Visible = false;
		this.btnGrossReading1stTime.Click += new System.EventHandler(btnGrossReading1stTime_Click);
		this.previewImageGrossReading.Location = new System.Drawing.Point(1329, 486);
		this.previewImageGrossReading.Name = "previewImageGrossReading";
		this.previewImageGrossReading.Size = new System.Drawing.Size(71, 23);
		this.previewImageGrossReading.TabIndex = 1;
		this.previewImageGrossReading.TabStop = false;
		this.previewImageGrossReading.Text = "Preview";
		this.previewImageGrossReading.UseVisualStyleBackColor = true;
		this.previewImageGrossReading.Visible = false;
		this.previewImageGrossReading.Click += new System.EventHandler(previewImageGrossReading_Click);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(7, 28);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(58, 30);
		this.label19.TabIndex = 47;
		this.label19.Text = "Gate Pass\r\nTicket No";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(7, 8);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(85, 15);
		this.label18.TabIndex = 45;
		this.label18.Text = "Gate Pass Type";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(298, 140);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(87, 20);
		this.label17.TabIndex = 43;
		this.label17.Text = "Net Weight";
		this.txtNetWeight.BackColor = System.Drawing.Color.White;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtNetWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtNetWeight.Location = new System.Drawing.Point(390, 134);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(105, 32);
		this.txtNetWeight.TabIndex = 17;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.Tag = "double";
		this.txtNetWeight.TextChanged += new System.EventHandler(txtNetWeight_TextChanged);
		this.txtsecondWeight.BackColor = System.Drawing.Color.White;
		this.txtsecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsecondWeight.Enabled = false;
		this.txtsecondWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtsecondWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtsecondWeight.Location = new System.Drawing.Point(390, 100);
		this.txtsecondWeight.Name = "txtsecondWeight";
		this.txtsecondWeight.ReadOnly = true;
		this.txtsecondWeight.Size = new System.Drawing.Size(105, 32);
		this.txtsecondWeight.TabIndex = 16;
		this.txtsecondWeight.TabStop = false;
		this.txtsecondWeight.Tag = "double";
		this.txtsecondWeight.TextChanged += new System.EventHandler(txtTareWeight_Leave);
		this.txtFirstWeight.BackColor = System.Drawing.Color.White;
		this.txtFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWeight.Enabled = false;
		this.txtFirstWeight.Font = new System.Drawing.Font("Verdana", 15.2f, System.Drawing.FontStyle.Bold);
		this.txtFirstWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtFirstWeight.Location = new System.Drawing.Point(390, 66);
		this.txtFirstWeight.Name = "txtFirstWeight";
		this.txtFirstWeight.ReadOnly = true;
		this.txtFirstWeight.Size = new System.Drawing.Size(105, 32);
		this.txtFirstWeight.TabIndex = 15;
		this.txtFirstWeight.Tag = "double";
		this.txtFirstWeight.TextChanged += new System.EventHandler(txtGroosWeight_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(8, 278);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 28;
		this.label13.Text = "Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(6, 202);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 26;
		this.label12.Text = "Wb Charges";
		this.txtWbCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbCharges.Location = new System.Drawing.Point(92, 198);
		this.txtWbCharges.Name = "txtWbCharges";
		this.txtWbCharges.Size = new System.Drawing.Size(200, 23);
		this.txtWbCharges.TabIndex = 9;
		this.txtWbCharges.Tag = "double";
		this.txtWbCharges.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWbCharges_KeyPress);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(6, 228);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(77, 15);
		this.label23.TabIndex = 24;
		this.label23.Text = "Container No";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(295, 174);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(61, 15);
		this.label11.TabIndex = 24;
		this.label11.Text = "Item Desc";
		this.txtContainerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainerNo.Location = new System.Drawing.Point(92, 224);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.Size = new System.Drawing.Size(200, 23);
		this.txtContainerNo.TabIndex = 10;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(295, 253);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(91, 15);
		this.label16.TabIndex = 22;
		this.label16.Text = "Eb Weight Total";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(295, 228);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(54, 15);
		this.label10.TabIndex = 22;
		this.label10.Text = "Item Qty";
		this.txtEbTotalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbTotalWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbTotalWeight.Location = new System.Drawing.Point(390, 249);
		this.txtEbTotalWeight.Name = "txtEbTotalWeight";
		this.txtEbTotalWeight.ReadOnly = true;
		this.txtEbTotalWeight.Size = new System.Drawing.Size(115, 23);
		this.txtEbTotalWeight.TabIndex = 25;
		this.txtEbTotalWeight.Tag = "double";
		this.txtEbTotalWeight.TextChanged += new System.EventHandler(txtEbTotalWeight_TextChanged);
		this.txtItemqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemqty.Location = new System.Drawing.Point(390, 224);
		this.txtItemqty.Name = "txtItemqty";
		this.txtItemqty.Size = new System.Drawing.Size(115, 23);
		this.txtItemqty.TabIndex = 23;
		this.txtItemqty.Tag = "double";
		this.txtItemqty.TextChanged += new System.EventHandler(txtItemqty_TextChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(296, 202);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(77, 15);
		this.label25.TabIndex = 18;
		this.label25.Text = "Packing Type";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(8, 91);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 20;
		this.label9.Text = "Vehicle Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(8, 119);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(86, 15);
		this.label8.TabIndex = 18;
		this.label8.Text = "Vehicle && Bility";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(8, 174);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 16;
		this.label7.Text = "Party Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(8, 146);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(65, 15);
		this.label5.TabIndex = 12;
		this.label5.Text = "Invoice No";
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBilityNo.Location = new System.Drawing.Point(193, 115);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(100, 23);
		this.txtBilityNo.TabIndex = 6;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(7, 245);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 30);
		this.label4.TabIndex = 10;
		this.label4.Text = "Working \r\nReport No";
		this.txtTickectNo.BackColor = System.Drawing.Color.White;
		this.txtTickectNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTickectNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTickectNo.Location = new System.Drawing.Point(191, 32);
		this.txtTickectNo.Name = "txtTickectNo";
		this.txtTickectNo.ReadOnly = true;
		this.txtTickectNo.Size = new System.Drawing.Size(100, 23);
		this.txtTickectNo.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.chkEnableWeights);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.txtFirstDate);
		this.panel6.Controls.Add(this.txtSecondDate);
		this.panel6.Controls.Add(this.ChkBox);
		this.panel6.Controls.Add(this.ChkBoxPrintImages);
		this.panel6.Controls.Add(this.chkPrintWithLab);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1270, 30);
		this.panel6.TabIndex = 2;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(123, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = " Weigh Bridge";
		this.chkEnableWeights.AutoSize = true;
		this.chkEnableWeights.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkEnableWeights.ForeColor = System.Drawing.Color.White;
		this.chkEnableWeights.Location = new System.Drawing.Point(131, 6);
		this.chkEnableWeights.Margin = new System.Windows.Forms.Padding(2);
		this.chkEnableWeights.Name = "chkEnableWeights";
		this.chkEnableWeights.Size = new System.Drawing.Size(128, 18);
		this.chkEnableWeights.TabIndex = 25;
		this.chkEnableWeights.Text = "Enable Weights";
		this.chkEnableWeights.UseVisualStyleBackColor = true;
		this.chkEnableWeights.Visible = false;
		this.chkEnableWeights.CheckedChanged += new System.EventHandler(chkEnableWeights_CheckedChanged);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(294, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(92, 15);
		this.label2.TabIndex = 5;
		this.label2.Text = "1st && 2nd Time";
		this.txtFirstDate.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtFirstDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstDate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.txtFirstDate.Location = new System.Drawing.Point(390, 4);
		this.txtFirstDate.Name = "txtFirstDate";
		this.txtFirstDate.Size = new System.Drawing.Size(150, 23);
		this.txtFirstDate.TabIndex = 15;
		this.txtSecondDate.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtSecondDate.Enabled = false;
		this.txtSecondDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondDate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.txtSecondDate.Location = new System.Drawing.Point(543, 4);
		this.txtSecondDate.Name = "txtSecondDate";
		this.txtSecondDate.Size = new System.Drawing.Size(150, 23);
		this.txtSecondDate.TabIndex = 16;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1157, 7);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(112, 18);
		this.ChkBox.TabIndex = 27;
		this.ChkBox.Text = "Preview 280";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.ChkBoxPrintImages.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintImages.AutoSize = true;
		this.ChkBoxPrintImages.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBoxPrintImages.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintImages.Location = new System.Drawing.Point(958, 7);
		this.ChkBoxPrintImages.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBoxPrintImages.Name = "ChkBoxPrintImages";
		this.ChkBoxPrintImages.Size = new System.Drawing.Size(193, 18);
		this.ChkBoxPrintImages.TabIndex = 29;
		this.ChkBoxPrintImages.Text = "Preview 280 With Image";
		this.ChkBoxPrintImages.UseVisualStyleBackColor = true;
		this.chkPrintWithLab.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintWithLab.AutoSize = true;
		this.chkPrintWithLab.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.chkPrintWithLab.ForeColor = System.Drawing.Color.White;
		this.chkPrintWithLab.Location = new System.Drawing.Point(811, 7);
		this.chkPrintWithLab.Margin = new System.Windows.Forms.Padding(2);
		this.chkPrintWithLab.Name = "chkPrintWithLab";
		this.chkPrintWithLab.Size = new System.Drawing.Size(140, 18);
		this.chkPrintWithLab.TabIndex = 28;
		this.chkPrintWithLab.Text = "Preview 257 Lab";
		this.chkPrintWithLab.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1270, 27);
		this.panel4.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnsave, this.btnUpdate, this.btnRefresh, this.Print, this.btnWeighBridgeLookUp, this.btnSlip257, this.BtnPurchaseLab, this.BtnWeightDefine, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1270, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "280-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnWeighBridgeLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWeighBridgeLookUp.Name = "btnWeighBridgeLookUp";
		this.btnWeighBridgeLookUp.Size = new System.Drawing.Size(146, 24);
		this.btnWeighBridgeLookUp.Text = "Weigh Bridge Lookup";
		this.btnWeighBridgeLookUp.Click += new System.EventHandler(btnWeighBridgeLookUp_Click);
		this.btnSlip257.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSlip257.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip257.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip257.Name = "btnSlip257";
		this.btnSlip257.Size = new System.Drawing.Size(102, 24);
		this.btnSlip257.Text = "257-GPSlip";
		this.btnSlip257.Click += new System.EventHandler(btnSlip257_Click);
		this.BtnPurchaseLab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPurchaseLab.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPurchaseLab.Name = "BtnPurchaseLab";
		this.BtnPurchaseLab.Size = new System.Drawing.Size(157, 24);
		this.BtnPurchaseLab.Text = "Purchase Lab Analysis";
		this.BtnPurchaseLab.Click += new System.EventHandler(BtnPurchaseLab_Click);
		this.BtnWeightDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnWeightDefine.Name = "BtnWeightDefine";
		this.BtnWeightDefine.Size = new System.Drawing.Size(153, 24);
		this.BtnWeightDefine.Text = "Vehicle Weight Define";
		this.BtnWeightDefine.Click += new System.EventHandler(BtnWeightDefine_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1276, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel7);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 33);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1270, 646);
		this.panel8.TabIndex = 2;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 132);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1270, 514);
		this.panel11.TabIndex = 5;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1270, 514);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label32);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 99);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1270, 33);
		this.panel10.TabIndex = 4;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.Teal;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label32.Location = new System.Drawing.Point(5, 7);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(142, 19);
		this.label32.TabIndex = 1;
		this.label32.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1223, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel7.Controls.Add(this.groupBox3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 28);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1270, 71);
		this.panel7.TabIndex = 0;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory);
		this.groupBox3.Controls.Add(this.lblLanguage);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.GpNoTo);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.GPNoFrom);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.ToDate);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.TxtVehicleNumber);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.txtTicketNoTo);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.txtTicketNoFrom);
		this.groupBox3.Controls.Add(this.FromDate);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1185, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1066, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 13;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1066, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 12;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(985, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 10;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(985, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 11;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.CmbWbTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWbTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWbTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWbTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance46).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWbTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Location = new System.Drawing.Point(842, 13);
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Name = "CmbWbTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(789, 19);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(56, 15);
		this.label26.TabIndex = 217;
		this.label26.Text = "W.B Type";
		this.CmbGatePassTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGatePassTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Location = new System.Drawing.Point(636, 41);
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Name = "CmbGatePassTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).TabIndex = 7;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblLanguage.Location = new System.Drawing.Point(552, 47);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(82, 15);
		this.lblLanguage.TabIndex = 216;
		this.lblLanguage.Text = "GatePass Type";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(913, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(66, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.GpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.GpNoTo.Location = new System.Drawing.Point(461, 43);
		this.GpNoTo.Name = "GpNoTo";
		this.GpNoTo.Size = new System.Drawing.Size(87, 23);
		this.GpNoTo.TabIndex = 5;
		this.GpNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpNoTo_KeyPress);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(386, 47);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 215;
		this.label30.Text = "Gp No To";
		this.GPNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GPNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.GPNoFrom.Location = new System.Drawing.Point(461, 15);
		this.GPNoFrom.Name = "GPNoFrom";
		this.GPNoFrom.Size = new System.Drawing.Size(87, 23);
		this.GPNoFrom.TabIndex = 4;
		this.GPNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GPNoFrom_KeyPress);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(386, 19);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(72, 15);
		this.label33.TabIndex = 214;
		this.label33.Text = "Gp No From";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(69, 43);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(130, 23);
		this.ToDate.TabIndex = 1;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(6, 19);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(63, 15);
		this.label34.TabIndex = 213;
		this.label34.Text = "From Date";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(6, 47);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(48, 15);
		this.label35.TabIndex = 212;
		this.label35.Text = "To Date";
		this.TxtVehicleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TxtVehicleNumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtVehicleNumber.Location = new System.Drawing.Point(636, 15);
		this.TxtVehicleNumber.Name = "TxtVehicleNumber";
		this.TxtVehicleNumber.Size = new System.Drawing.Size(150, 23);
		this.TxtVehicleNumber.TabIndex = 6;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(552, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 211;
		this.label36.Text = "Vehicle No.";
		this.txtTicketNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNoTo.Location = new System.Drawing.Point(296, 43);
		this.txtTicketNoTo.Name = "txtTicketNoTo";
		this.txtTicketNoTo.Size = new System.Drawing.Size(87, 23);
		this.txtTicketNoTo.TabIndex = 3;
		this.txtTicketNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTicketNoTo_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(205, 47);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(74, 15);
		this.label37.TabIndex = 210;
		this.label37.Text = "Ticket No To";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(205, 19);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(89, 15);
		this.label38.TabIndex = 209;
		this.label38.Text = "Ticket No From";
		this.txtTicketNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNoFrom.Location = new System.Drawing.Point(296, 15);
		this.txtTicketNoFrom.Name = "txtTicketNoFrom";
		this.txtTicketNoFrom.Size = new System.Drawing.Size(87, 23);
		this.txtTicketNoFrom.TabIndex = 2;
		this.txtTicketNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTicketNoFrom_KeyPress);
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(69, 15);
		this.FromDate.Name = "FromDate";
		this.FromDate.ShowCheckBox = true;
		this.FromDate.Size = new System.Drawing.Size(130, 23);
		this.FromDate.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label20);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1270, 28);
		this.panel9.TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(4, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(186, 19);
		this.label20.TabIndex = 1;
		this.label20.Text = " Weigh Bridge History";
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1270, 30);
		this.panel5.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewHistory, this.BtnRefreshHistory, this.toolStripButton4, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1270, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "&New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Visible = false;
		this.toolStripButton4.Click += new System.EventHandler(toolStripButton4_Click);
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(82, 24);
		this.btnLoadAll.Text = "Load All";
		this.btnLoadAll.Visible = false;
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.FromArgb(32, 168, 216);
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label14.Location = new System.Drawing.Point(16, 8);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(196, 24);
		this.label14.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1169, 7);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar2.TabIndex = 21;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1284, 711);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmWeightbridge";
		this.Text = "WeighBridge";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWeightbridge_FormClosing);
		base.Load += new System.EventHandler(frmWeightbridge_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmWeightbridge_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingForSecond).EndInit();
		this.FilterBoxPendingFor2nd.ResumeLayout(false);
		this.FilterBoxPendingFor2nd.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingFirst).EndInit();
		this.FilterBoxGpFor1st.ResumeLayout(false);
		this.FilterBoxGpFor1st.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleNo).EndInit();
		this.groupBox2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PicSecondReading2ndtime).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicSecondUpper).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicSecondVehicle2ndTime).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.PicFirstReading1stTime).EndInit();
		((System.ComponentModel.ISupportInitialize)this.picFirstUpper).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicVehicleFront1stTime).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRefDocNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		this.GroupInwardLabInfo.ResumeLayout(false);
		this.GroupInwardLabInfo.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceType).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
