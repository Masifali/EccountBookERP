using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.Main;
using Architecture.BLL.Steel.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.WeightBridge;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lab;
using Architecture.WinApp.PackingMaterial_Store;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Sale;
using Architecture.WinApp.WeightBridge;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Purchase;

public class InwardGatePass : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public int POId;

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool UpdateMode;

	private bool LockBreakUp;

	private bool AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;

	public UserAccount UserAccount = new UserAccount();

	public int RecId;

	public int POInfoDocumentTypeId;

	private int ActionIdForSpecialApproval;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool PurchaseOrderBranchWise;

	private bool WbNotCompulsoryForPmPurchaseOrder;

	private bool StockReserveAmanatMall;

	private double PoToleranceWeightPercentage;

	private DataTable table = new DataTable();

	private DataTable tablegrd = new DataTable();

	private DataTable dtcust = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtPurchaseBreakup = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtStockReceiving = new DataTable();

	private DataTable dtCustomerAgainstInvoice = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private byte[] GpQrCode;

	private int[] AryColWidth;

	private bool _internalChange;

	private int DriverBioId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private Panel panel4;

	private DateTimePicker txtouttime;

	private DateTimePicker txtintime;

	private DateTimePicker txtgpdate;

	private TextBox txtNetPaid;

	private TextBox txtbiltyno;

	private TextBox txtfreight;

	private TextBox txtvehicleno;

	private TextBox txtgptypeno;

	private TextBox txtgpno;

	private Label label11;

	private Panel panel5;

	private Panel panel8;

	internal GridEX grdhistory;

	private Panel panel7;

	private Panel panel9;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtqty;

	private TextBox txtWeighBridgeSlipNo;

	private TextBox CmbOrderno;

	internal UltraCombo CmbVariety;

	internal UltraCombo cmbsupp;

	private ToolStripButton btnRefresh;

	private TextBox txtBalQty;

	private TextBox txtRecQty;

	private TextBox txtOrderQty;

	private CheckBox ChkBox;

	private ToolStripButton btndefinecity;

	private ToolStripButton toolStripButton4;

	internal UltraCombo cmbcity;

	internal UltraCombo cmbvehicletype;

	internal UltraCombo cmbgptype;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbStatus;

	internal UltraCombo cmbWeighBridge;

	private TextBox txtLabReportNo;

	private TextBox txtAnaylstName;

	private TextBox txtLabRemarks;

	private TextBox txtReportStatus;

	internal UltraCombo CmbOrderType;

	internal UltraCombo cmbContainerNo1;

	private Label ContainerNo1;

	internal UltraCombo cmbContainerNo2;

	private Label containerno2;

	private ToolStripButton btndefvehicle;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdPurchaseOrder;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtWeightCompared;

	private TextBox txtPackUnit;

	private Label label42;

	private CheckBox ChkIsApproved;

	private PictureBox pictureBox1;

	private TextBox txtWeightDiffRemarks;

	private Label label43;

	private Label label44;

	private Label label45;

	private Label label46;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	private Label label58;

	private Label label59;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private Label label64;

	private DateTimePicker txtBiltyDate;

	private GroupBox groupBox1;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label74;

	private Label label75;

	private Label label76;

	private Label lblDiffRemarks;

	private Label label66;

	private Label label67;

	private Label label68;

	private Label label69;

	private Label label73;

	private Panel panel10;

	private Panel panel12;

	private Panel panel11;

	private Panel panel13;

	private Panel panel14;

	private Label label5;

	private Label label6;

	private TabPage tabPage5;

	internal GridEX grdStockReceivingNotes;

	private CtrlGrdBar ctrlGrdBar4;

	private GroupBox groupBox2;

	private Label label9;

	private Label label8;

	private Label label15;

	private Label label16;

	private Label label12;

	private Label label10;

	private Label label17;

	private Panel panel6;

	private Panel PanelBreakup;

	internal GridEX grdPurchaseBrakup;

	private Panel panel22;

	private Label label38;

	private ToolStripButton btnSlip257;

	private Panel panel17;

	private GroupBox groupBox3;

	private Button btnShowHistory;

	private Label label18;

	private Label label19;

	private Label label20;

	private Label label21;

	private Label label22;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker txtToDateHistory;

	private DateTimePicker txtFromDateHistory;

	private Panel panel18;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private CtrlGrdBar ctrlGrdBar5;

	private GroupBox groupBox4;

	private RadioButton RadAapprovedDatePoInfo;

	private RadioButton RadEntryDatePoInfo;

	private RadioButton RadModifyDatePoInfo;

	private RadioButton RadDocDatePoInfo;

	internal UltraCombo cmbSupplierNamePoInfo;

	private TextBox txtToDocNoHistoryPoInfo;

	private TextBox txtFromDocNoHistoryPoInfo;

	private DateTimePicker ToDateHistoryPoInfo;

	private DateTimePicker FromDateHistoryPoInfo;

	private Button BtnShowPoInfo;

	private Label label23;

	private Label label24;

	private Label label25;

	private Label label26;

	private Label label27;

	private Button BtnResetPoInfo;

	private Panel panel16;

	private Label label7;

	private Panel panel15;

	private GroupBox groupBox5;

	private RadioButton RadApproveDateStockInfo;

	private RadioButton RadEntryDateStockInfo;

	private RadioButton RadModifyDateStockInfo;

	private RadioButton RadDocDateStockInfo;

	private TextBox ToDocStockInfo;

	private TextBox FromDocStockInfo;

	private DateTimePicker ToDateStockInfo;

	private DateTimePicker FromDateStockInfo;

	private Button BtnResetStockInfo;

	private Button BtnShowStockInfo;

	private Label label29;

	private Label label30;

	private Label label31;

	private Label label32;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo CmbDocumentTypePoInfo;

	private Label label28;

	private TextBox txtExpiryDaysPoInfo;

	private Label label34;

	private TextBox txtAccessWeight;

	private Label label33;

	private CheckBox chkDriverInfoForm;

	private ToolStripButton BtnDriverForm;

	private RadioButton RadNoneFormToOpenOnInsert;

	private RadioButton RadPurchaseLabFormToOpenOnInsert;

	private RadioButton RadGrnFormToOpenOnInsert;

	private RadioButton RadWBFormToOpenOnInsert;

	private LinkLabel labelWeightBridgeForm;

	private LinkLabel labelLabAnalysisForm;

	private LinkLabel labelGrnForm;

	internal UltraCombo CmbPackingType;

	private Label label35;

	private Label label36;

	private Label label37;

	private Label label40;

	private TextBox txtTotalPayablesFreight;

	private TextBox txtAdvanceByParty;

	private TextBox txtAdvanceByFactory;

	private LinkLabel FreightVoucherFormToOpen;

	private RadioButton RadFreightVoucherFormToOpen;

	private RadioButton rdFreightCredit;

	private RadioButton rdFreightCash;

	private Label label14;

	internal UltraCombo CmbSupplierDispatchedPreBillNo;

	private Label label13;

	private TextBox txtDifferenceWeight;

	private TextBox txtSupplierWeight;

	private TextBox txtSecondWtDifference;

	private TextBox txtSupplierSecondWeight;

	private Label label80;

	private Label label82;

	private TextBox txtFirstWtDifference;

	private Label label83;

	private TextBox txtSupplierFirstWeight;

	private Label label84;

	private Label label85;

	private Label label86;

	private TextBox txtFactoryWeight;

	private TextBox txtsecondWeight;

	private TextBox txtFirstWeight;

	private TextBox txtremarks;

	private GroupBox groupBox6;

	private PictureBox btnResetDriverInfo;

	private MaskedTextBox txtFatherCNIC;

	private Label label65;

	private MaskedTextBox txtWhatsAppNo;

	private Label label70;

	private MaskedTextBox txtAlternateCellNo;

	private Label label71;

	private Label label72;

	private TextBox txtFatherName;

	private MaskedTextBox txtDriverCellNo;

	private Label label77;

	private TextBox txtDriverName;

	private Label label78;

	private MaskedTextBox txtCNIC;

	private Label label79;

	public InwardGatePass(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "WbNotCompulsoryForPmPurchaseOrder");
			WbNotCompulsoryForPmPurchaseOrder = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			StockReserveAmanatMall = CommonServices.GetERPFeatureById(21);
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 750);
			ChkIsApproved.Checked = true;
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			txtSupplierFirstWeight.Enabled = true;
			txtSupplierSecondWeight.Enabled = true;
			ChkIsApproved.Visible = formright.DoHaveApproveRights;
			dtViewAttachments.Columns.Clear();
			dtViewAttachments.Columns.Add("AttchmentPath");
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			tabControl2.TabPages.Remove(tabPage4);
			tabControl2.TabPages.Remove(tabPage5);
			PanelBreakup.Visible = false;
			dtPurchaseBreakup.Columns.Add("Id", typeof(int));
			dtPurchaseBreakup.Columns.Add("Qty", typeof(double));
			dtPurchaseBreakup.Columns.Add("UOM", typeof(double));
			dtPurchaseBreakup.Columns.Add("GrossWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("EbWeight", typeof(double));
			dtPurchaseBreakup.Columns.Add("EBTotal", typeof(double));
			dtPurchaseBreakup.Columns.Add("NetWeight", typeof(double));
			AddRowInPurchaseBreakUpGrid();
			grdPurchaseBrakup.RetrieveStructure();
			grdPurchaseBreakUpSettings();
			ItemNameFill();
			PackingTypeDtFillFromGlobalAndBind();
			gatepasstype();
			SupplierCustomerFill();
			gpnofill();
			vehicleTypefill();
			CityFill();
			grdfrmfill();
			Status();
			OrderTypeFill();
			cmbWeighBridgeFill();
			cmbgptype_Leave_1(null, null);
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			ChkBox.Checked = true;
			txtPackUnit.Enabled = true;
			txtFactoryWeight.Enabled = false;
			txtgpdate.Focus();
			List<ConfigrationsAllocation> list2 = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt").ToList();
			if (list2.Count > 0)
			{
				AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt = Conversion.ToBool((object)list2[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				cmbcity.Value = CityId;
			}
			txtFromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			txtToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PreBillNoFill(int PartyId, int OrderId)
	{
		try
		{
			DataTable dt = SupplierDispatch.SupplierDispatch_GetNoForGpandGrn(UserAccount.OrganizationId, UserAccount.CompanyId, PartyId, 0, RecId, 0, OrderId);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplierDispatchedPreBillNo, dt, "SupplierDispatchId", "SupplierDispatchNo", "Pre Bill No", AllColumns: true, Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value));
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)CmbSupplierDispatchedPreBillNo).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				CmbSupplierDispatchedPreBillNo_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gpnofill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int Code = GatePassInward.GenerategpCode(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 51
			});
			if (Code > 0)
			{
				txtgpno.Text = Conversion.ToString((object)Code);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void gatepasstype()
	{
		try
		{
			int Id = Conversion.ToInt(cmbgptype.Value);
			DataTable dt = CommonServices.GatePassTypeFill("Inward");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbgptype, "Id", "GpTypeDescription", "GatePassType", ZeroIndex: true);
				((UltraGridBase)cmbgptype).Rows[2].Activate();
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbgptype.Value = Id;
					}
					else
					{
						((Control)(object)cmbgptype).Text = string.Empty;
					}
				}
				cmbgptype_Leave(null, null);
			}
			else
			{
				((Control)(object)cmbgptype).Text = string.Empty;
				((UltraGridBase)cmbgptype).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbgptype_Leave_1(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			int Code = GatePassInward.GenerateGPTypeCode(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				GatepassType = Conversion.ToString((object)((Control)(object)cmbgptype).Text),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtgptypeno.Text = Code.ToString();
				if (((Control)(object)cmbgptype).Text == "Export" || ((Control)(object)cmbgptype).Text == "Export Return" || ((Control)(object)cmbgptype).Text == "Import")
				{
					((Control)(object)cmbContainerNo1).Visible = true;
					((Control)(object)cmbContainerNo2).Visible = true;
					ContainerNo1.Visible = true;
					containerno2.Visible = true;
				}
				else
				{
					((Control)(object)cmbContainerNo1).Visible = false;
					((Control)(object)cmbContainerNo2).Visible = false;
					ContainerNo1.Visible = false;
					containerno2.Visible = false;
				}
			}
			else
			{
				MessageBox.Show("Please GatePass Type Select");
				cmbgptype.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Status()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Accepted");
			dt.Rows.Add(3, "Rejected");
			if (StockReserveAmanatMall)
			{
				dt.Rows.Add(4, "Accept As Reserved");
			}
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OrderTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbOrderType.Value);
			DataTable dt = GatePassInward.GetOrderType(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 41)
					{
						tabControl2.TabPages.Add(tabPage4);
						POInfoDocumentTypeId = 41;
						OrderInformationComboFill(POInfoDocumentTypeId);
						FromDateHistoryPoInfo.Value = DateTime.Now.AddDays(-3.0);
						ToDateHistoryPoInfo.Value = DateTime.Now;
					}
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 1500)
					{
						tabControl2.TabPages.Add(tabPage4);
						POInfoDocumentTypeId = 1500;
						OrderInformationComboFill(POInfoDocumentTypeId);
						FromDateHistoryPoInfo.Value = DateTime.Now.AddDays(-3.0);
						ToDateHistoryPoInfo.Value = DateTime.Now;
					}
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 175)
					{
						tabControl2.TabPages.Add(tabPage5);
						FromDateStockInfo.Value = DateTime.Now.AddDays(-3.0);
						ToDateStockInfo.Value = DateTime.Now;
					}
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 98 || Conversion.ToInt(dt.Rows[i]["Id"]) == 52)
					{
						BindSaleInvoiceSupplierCustomer();
					}
					if (Conversion.ToInt(dt.Rows[i]["Id"]) == 105)
					{
						PanelBreakup.Visible = true;
					}
				}
				DDL.BindDDL(dt, CmbOrderType, "Id", "OrderType", "Purchase Type", ZeroIndex: true);
				((UltraGridBase)CmbOrderType).Rows[1].Activate();
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbOrderType.Value = Id;
					}
					else
					{
						((Control)(object)CmbOrderType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbOrderType).Text = string.Empty;
				((UltraGridBase)CmbOrderType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbvehicletype.Value);
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Name", ZeroIndex: true);
				((UltraGridBase)cmbvehicletype).Rows[1].Activate();
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbvehicletype.Value = Id;
					}
					else
					{
						((Control)(object)cmbvehicletype).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbvehicletype).Text = string.Empty;
				((UltraGridBase)cmbvehicletype).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbcity.Value);
			DataTable dtcity = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcity, cmbcity, "Id", "CityName", "City Name", true);
				((UltraGridBase)cmbcity).Rows[1].Activate();
				if (Id > 0)
				{
					if (dtcity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbcity.Value = Id;
					}
					else
					{
						((Control)(object)cmbcity).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbcity).Text = string.Empty;
				((UltraGridBase)cmbcity).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SupplierCustomerFill()
	{
		try
		{
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(1);
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSupplierCustomer()
	{
		try
		{
			int Id = Conversion.ToInt(cmbsupp.Value);
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CompanyName");
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(cmbsupp.Value))
				{
					dt.Rows.Add(dtSupplier.Rows[i]["Id"], dtSupplier.Rows[i]["CompanyName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbsupp, "Id", "CompanyName", "SupplierName", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbsupp.Value = Id;
					}
					else
					{
						((Control)(object)cmbsupp).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbsupp).Text = string.Empty;
				((UltraGridBase)cmbsupp).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbOrderno_Leave(object sender, EventArgs e)
	{
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5a: Expected O, but got Unknown
		try
		{
			if (!(((Control)(object)cmbgptype).Text != "Export Return"))
			{
				return;
			}
			if (Conversion.ToInt(CmbOrderType.Value) == 41)
			{
				if (((Control)(object)cmbgptype).Text != "Export" && ((Control)(object)cmbgptype).Text != "Import")
				{
					ReportsParameters parameters = new ReportsParameters();
					parameters.OrganizationId = UserAccount.OrganizationId;
					parameters.CompanyId = UserAccount.CompanyId;
					parameters.BranchesId = UserAccount.BranchesId;
					parameters.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
					parameters.DocumentTypeId = 41;
					parameters.DocDate = txtgpdate.Value;
					parameters.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					parameters.Id = RecId;
					dtcust = SupplierCustomer.GetSupplierByPurchaseOrderNo(parameters);
					if (dtcust.Rows.Count > 0)
					{
						POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
						((Control)(object)cmbgptype).Text = Conversion.ToString(dtcust.Rows[0]["OrderCategoryName"]);
						DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DDL.BindDDLNew(dtcust, CmbVariety, "ItemId", "ItemName", "ItemName", false);
						((UltraGridBase)CmbVariety).Rows[0].Activate();
						((Control)(object)cmbcity).Text = Conversion.ToString(dtcust.Rows[0]["CityArea"]);
					}
					else
					{
						POId = 0;
						((Control)(object)cmbsupp).Text = string.Empty;
						((UltraGridBase)cmbsupp).DataSource = null;
						CmbOrderno.Text = string.Empty;
						((Control)(object)CmbVariety).Text = string.Empty;
						((UltraGridBase)CmbVariety).DataSource = null;
					}
					((Control)(object)CmbSupplierDispatchedPreBillNo).Enabled = true;
					PreBillNoFill(Conversion.ToInt(cmbsupp.Value), POId);
				}
			}
			else if (Conversion.ToInt(CmbOrderType.Value) == 700)
			{
				if (((Control)(object)cmbgptype).Text != "Export" && ((Control)(object)cmbgptype).Text != "Import")
				{
					ReportsParameters parameters2 = new ReportsParameters();
					parameters2.OrganizationId = UserAccount.OrganizationId;
					parameters2.CompanyId = UserAccount.CompanyId;
					parameters2.BranchesId = UserAccount.BranchesId;
					parameters2.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
					parameters2.DocumentTypeId = 700;
					parameters2.DocDate = txtgpdate.Value;
					parameters2.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					parameters2.Id = RecId;
					dtcust = SupplierCustomer.GetSupplierByPurchaseOrderNo(parameters2);
					if (dtcust.Rows.Count > 0)
					{
						POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
						DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DDL.BindDDLNew(dtcust, CmbVariety, "ItemId", "ItemName", "ItemName", false);
						((UltraGridBase)CmbVariety).Rows[0].Activate();
						((Control)(object)cmbcity).Text = Conversion.ToString(dtcust.Rows[0]["CityArea"]);
					}
					else
					{
						POId = 0;
						((Control)(object)cmbsupp).Text = string.Empty;
						((UltraGridBase)cmbsupp).DataSource = null;
						CmbOrderno.Text = string.Empty;
						((Control)(object)CmbVariety).Text = string.Empty;
						((UltraGridBase)CmbVariety).DataSource = null;
					}
				}
			}
			else if (Conversion.ToInt(CmbOrderType.Value) == 104)
			{
				DataTable dt = InvSupplyOrder.GetOutstandingSupplyOrderForGatePassInward(new GatePassInward
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)CmbOrderno.Text.Trim()),
					GpDate = txtgpdate.Value,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dt.Rows.Count > 0)
				{
					POId = Conversion.ToInt(dt.Rows[0]["Id"]);
					grd.DataSource = dt;
					grd.RetrieveStructure();
					GrdsettingForSupplyOrder();
					txtvehicleno.Text = Conversion.ToString(dt.Rows[0]["VehicleNos"]);
					txtbiltyno.Text = Conversion.ToString(dt.Rows[0]["BiltyNos"]);
					txtqty.Text = Conversion.ToString(grd.GetTotal(grd.RootTable.Columns["Qty"], (AggregateFunction)2));
					txtSupplierWeight.Text = Conversion.ToString(grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2));
					txtSupplierWeight.Enabled = false;
				}
				else
				{
					POId = 0;
					txtvehicleno.Text = string.Empty;
					txtbiltyno.Text = string.Empty;
				}
			}
			else if (Conversion.ToInt(CmbOrderType.Value) == 232)
			{
				if (((Control)(object)cmbgptype).Text == "Import")
				{
					DataTable dtcustImport = SupplierCustomer.GetSupplierByImportPurchaseContract(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						OrderId = Conversion.ToInt((object)CmbOrderno.Text),
						DocumentTypeId = 232,
						FinancialYearId = clsGlobalVariables.ActiveYr.Id
					});
					if (dtcustImport.Rows.Count > 0)
					{
						POId = int.Parse(Conversion.ToString(dtcustImport.Rows[0]["PurchaseOrderId"]));
						DDL.BindDDLNew(dtcustImport, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						GetContainerNo(POId);
					}
					else
					{
						POId = 0;
						((Control)(object)cmbsupp).Text = string.Empty;
						((UltraGridBase)cmbsupp).DataSource = null;
						CmbOrderno.Text = string.Empty;
						((Control)(object)CmbVariety).Text = string.Empty;
						((UltraGridBase)CmbVariety).DataSource = null;
					}
				}
				else
				{
					MessageBox.Show("Please select Gate pass Type Import First");
				}
			}
			else if (Conversion.ToInt(CmbOrderType.Value) == 175)
			{
				if (dtStockReceiving.Rows.Count <= 0 || Conversion.ToInt((object)CmbOrderno.Text.Trim()) <= 0)
				{
					POId = 0;
					CmbOrderno.Text = "";
					txtvehicleno.Text = string.Empty;
					((Control)(object)cmbvehicletype).Text = string.Empty;
					txtbiltyno.Text = string.Empty;
					txtqty.Text = string.Empty;
					txtSupplierWeight.Text = string.Empty;
					throw new Exception("Stock Receiving Note Record not found");
				}
				EnumerableRowCollection<DataRow> rows = from r in dtStockReceiving.AsEnumerable()
					where r.Field<int>("GpSrNo") == Conversion.ToInt((object)CmbOrderno.Text.Trim())
					select r;
				DataTable dt2 = (rows.Any() ? rows.CopyToDataTable() : dtStockReceiving.Clone());
				if (dt2 == null || dt2.Rows.Count <= 0)
				{
					CmbOrderno.Text = "";
					throw new Exception("Stock Receiving Note Record not found");
				}
				POId = Conversion.ToInt(dt2.Rows[0]["Id"]);
				txtvehicleno.Text = Conversion.ToString(dt2.Rows[0]["VehicleNo"]);
				DataTable dtSupplier = new DataTable();
				dtSupplier.Columns.Add("Id");
				dtSupplier.Columns.Add("SupplierName");
				dtSupplier.Rows.Add(dt2.Rows[0]["SupplierCustomerId"], dt2.Rows[0]["CustomerName"]);
				DDL.BindDDL(dtSupplier, cmbsupp, "Id", "SupplierName", "Supplier Name", ZeroIndex: false);
				((UltraGridBase)cmbsupp).Rows[0].Activate();
				((Control)(object)cmbvehicletype).Text = Conversion.ToString(dt2.Rows[0]["VehicleType"]);
				txtbiltyno.Text = Conversion.ToString(dt2.Rows[0]["BiltyNo"]);
				double qty = Conversion.ToDouble(dt2.Rows[0]["ItemQty"]);
				txtqty.Text = qty.ToString();
				double SupplierWeight = Conversion.ToDouble(dt2.Rows[0]["DoWeight"]);
				txtSupplierWeight.Text = SupplierWeight.ToString();
				double packuom = SupplierWeight / qty;
				txtPackUnit.Text = packuom.ToString();
				((Control)(object)CmbVariety).Text = Conversion.ToString(dt2.Rows[0]["ItemName"]);
			}
			else if (Conversion.ToInt(CmbOrderType.Value) == 1500 && ((Control)(object)cmbgptype).Text != "Export" && ((Control)(object)cmbgptype).Text != "Import")
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
				obj.DocumentTypeId = 1500;
				obj.DocDate = txtgpdate.Value;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				dtcust = PurchaseOrder.GetPurchaseOrderForGatePassInwardByOrderId(obj);
				if (dtcust.Rows.Count > 0)
				{
					POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
					((Control)(object)cmbgptype).Text = Conversion.ToString(dtcust.Rows[0]["OrderCategoryName"]);
					DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
					((UltraGridBase)cmbsupp).Rows[0].Activate();
					DDL.BindDDLNew(dtcust, CmbVariety, "ItemId", "ItemName", "ItemName", false);
					((UltraGridBase)CmbVariety).Rows[0].Activate();
					cmbcity.Value = Conversion.ToInt(dtcust.Rows[0]["CityId"]);
				}
				else
				{
					POId = 0;
					((Control)(object)cmbsupp).Text = string.Empty;
					((UltraGridBase)cmbsupp).DataSource = null;
					CmbOrderno.Text = string.Empty;
					((Control)(object)CmbVariety).Text = string.Empty;
					((UltraGridBase)CmbVariety).DataSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			POId = 0;
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			CmbOrderno.Text = string.Empty;
			((Control)(object)CmbVariety).Text = string.Empty;
			((UltraGridBase)CmbVariety).DataSource = null;
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetContainerNo(int Ids)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		try
		{
			if (RecId <= 0)
			{
				DataTable dtcontainer = ExImLcOrderPurchaseOrder.GetContainerNosByOrderId(new ExImLcOrderPurchaseOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Ids
				});
				if (dtcontainer.Rows.Count > 0)
				{
					DDL.BindDDL(dtcontainer, cmbContainerNo1, "ContainerNo", "ContainerNo", "ContainerNo", ZeroIndex: false);
					DDL.BindDDL(dtcontainer, cmbContainerNo2, "ContainerNo", "ContainerNo", "ContainerNo", ZeroIndex: false);
					((UltraGridBase)cmbContainerNo1).Rows[0].Activate();
					((UltraGridBase)cmbContainerNo2).Rows[1].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			dtItem = CommonServices.ReadAllItemsIncludedPM();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbWeighBridgeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Auto");
			dt.Rows.Add(2, "Mannual");
			DDL.BindDDL(dt, cmbWeighBridge, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbWeighBridge).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindLocalSupplierCustomer(int CurrCustomerId)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbsupp, "Id", "CompanyName", "SupplierName", false);
				if (CurrCustomerId > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CurrCustomerId))
					{
						cmbsupp.Value = CurrCustomerId;
					}
					else
					{
						((Control)(object)cmbsupp).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbsupp).Text = string.Empty;
				((UltraGridBase)cmbsupp).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindAllSupplierCustomer(int CurrCustomerId)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CompanyName", typeof(string));
			List<getGlobalAllSupplierCustomer> list = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => !r.IsSubSupCust).ToList();
			if (list == null || list.Count == 0)
			{
				return;
			}
			foreach (getGlobalAllSupplierCustomer item in list)
			{
				object[] rowData = new object[2]
				{
					Conversion.ToInt((object)item.Id),
					Conversion.ToString((object)item.CompanyName)
				};
				dt.Rows.Add(rowData);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbsupp, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				if (CurrCustomerId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CurrCustomerId))
					{
						cmbsupp.Value = CurrCustomerId;
					}
					else
					{
						((Control)(object)cmbsupp).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbsupp).Text = string.Empty;
				((UltraGridBase)cmbsupp).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindCustomerAgainstSaleInvoice(int CurrCustomerId)
	{
		try
		{
			if (dtCustomerAgainstInvoice.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCustomerAgainstInvoice, cmbsupp, "Id", "CompanyName", "SupplierName", false);
				if (CurrCustomerId > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CurrCustomerId))
					{
						cmbsupp.Value = CurrCustomerId;
					}
					else
					{
						((Control)(object)cmbsupp).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbsupp).Text = string.Empty;
				((UltraGridBase)cmbsupp).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindSaleInvoiceSupplierCustomer()
	{
		try
		{
			dtCustomerAgainstInvoice = InvSaleInvoice.GetPartiesFromSaleInvoiceWithGlAccount(UserAccount.OrganizationId, UserAccount.CompanyId, "95,99,186");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		try
		{
			DataTable dt = GatePassInward.GetDataForDropDownFromGPI(UserAccount.OrganizationId, UserAccount.CompanyId, "Supplier", Conversion.ToString((object)UserAccount.BranchesId));
			DDL.BindDDLNew(dt, CmbCustomerHistory, "Id", "ReferenceName", "Supplier", false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void QRCodeGenerate()
	{
	}

	private void LabDataGetByGpId(int GpId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassInward.GetDataByGpId(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				Id = GpId
			});
			if (dt.Rows.Count > 0)
			{
				txtAnaylstName.Text = Conversion.ToString(dt.Rows[0]["AnalystName"]);
				txtLabReportNo.Text = Conversion.ToString(dt.Rows[0]["DocNo"]);
				txtReportStatus.Text = Conversion.ToString(dt.Rows[0]["LabStatus"]);
				txtLabRemarks.Text = Conversion.ToString(dt.Rows[0]["RemarksHeader"]);
			}
			else
			{
				txtAnaylstName.Text = string.Empty;
				txtLabReportNo.Text = string.Empty;
				txtReportStatus.Text = string.Empty;
				txtLabRemarks.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbOrderType_Leave(object sender, EventArgs e)
	{
		try
		{
			txtSupplierWeight.Enabled = true;
			((Control)(object)CmbSupplierDispatchedPreBillNo).Enabled = false;
			int CurrCustomerId = Conversion.ToInt(cmbsupp.Value);
			CmbOrderno.Text = string.Empty;
			CmbOrderno.Enabled = true;
			POId = 0;
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			if (Conversion.ToInt(CmbOrderType.Value) == 105 || Conversion.ToInt(CmbOrderType.Value) == 106 || Conversion.ToInt(CmbOrderType.Value) == 98 || Conversion.ToInt(CmbOrderType.Value) == 52)
			{
				POId = Conversion.ToInt((object)txtgpno.Text.Trim());
				CmbOrderno.Text = txtgpno.Text.Trim();
				CmbOrderno.Enabled = false;
				if (Conversion.ToInt(CmbOrderType.Value) != 98)
				{
					BindLocalSupplierCustomer(CurrCustomerId);
				}
				else
				{
					BindCustomerAgainstSaleInvoice(CurrCustomerId);
				}
				cmbgptype_Leave(null, null);
			}
			if (Conversion.ToInt(CmbOrderType.Value) == 241 || Conversion.ToInt(CmbOrderType.Value) == 204)
			{
				POId = Conversion.ToInt((object)txtgpno.Text.Trim());
				CmbOrderno.Text = txtgpno.Text.Trim();
				CmbOrderno.Enabled = false;
				BindAllSupplierCustomer(CurrCustomerId);
				cmbgptype_Leave(null, null);
			}
			if (Conversion.ToInt(CmbOrderType.Value) == 175 || Conversion.ToInt(CmbOrderType.Value) == 700)
			{
				POId = Conversion.ToInt((object)txtgpno.Text.Trim());
				BindLocalSupplierCustomer(CurrCustomerId);
			}
			if (Conversion.ToInt(CmbOrderType.Value) != 105)
			{
				txtAdvanceByFactory.Enabled = true;
				txtAdvanceByParty.Enabled = true;
			}
			else
			{
				txtAdvanceByFactory.Text = "0";
				txtAdvanceByParty.Text = "0";
				txtAdvanceByFactory.Enabled = false;
				txtAdvanceByParty.Enabled = false;
			}
			if (Conversion.ToInt(CmbOrderType.Value) == 41)
			{
				txtSupplierWeight.Tag = "";
				((Control)(object)CmbSupplierDispatchedPreBillNo).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgptype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbgptype).ActiveRow == null || Conversion.ToInt(CmbOrderType.Value) == 700 || Conversion.ToInt(CmbOrderType.Value) == 41)
			{
				return;
			}
			int Id = Conversion.ToInt(CmbVariety.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("ItemName");
			if (dtItem.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtItem.Rows.Count; i++)
			{
				dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbVariety, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbVariety).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbVariety.Value = Id;
					}
					else
					{
						((Control)(object)CmbVariety).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbVariety).Text = string.Empty;
				((UltraGridBase)CmbVariety).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeDtFillFromGlobalAndBind()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalInvPackingType, (InvPackingType x) => x.Id, (InvPackingType x) => x.PackTypeDesc);
			int Id = Conversion.ToInt(CmbPackingType.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbPackingType, dt, "Id", "Description", "Packing Type", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbgptype).ActiveRow == null || Conversion.ToInt(cmbgptype.Value) == 0)
		{
			MessageBox.Show("GatePassType Field Required");
			cmbgptype.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == string.Empty)
		{
			MessageBox.Show("VehicleNo Field Required");
			txtvehicleno.Focus();
			return false;
		}
		string value = txtvehicleno.Text.Trim().ToUpper();
		if (!Regex.IsMatch(value, "^[A-Z]{1,6}-\\d{1,6}$"))
		{
			MessageBox.Show("Vehicle no is not valid. Please check!");
			txtvehicleno.Focus();
			return false;
		}
		string[] array = value.Split('-');
		string letters = array[0];
		string digits = array[1];
		if ((letters.Length <= 2 || digits.Length <= 2) && (letters.Length <= 2 || digits.Length <= 2) && MessageBox.Show("The vehicle number is unusually short (letters or digits ≤ 2).\nAre you sure you want to proceed?", "Confirm Vehicle Number", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
		{
			txtvehicleno.Focus();
			return false;
		}
		if (((Control)(object)cmbgptype).Text == "Import" || ((Control)(object)CmbOrderType).Text == "Import Order")
		{
			if (((Control)(object)cmbContainerNo1).Text == "" && ((Control)(object)cmbContainerNo2).Text == "")
			{
				MessageBox.Show("Please Select At least One Container No In Case of Import");
				cmbContainerNo1.Focus();
				return false;
			}
			if (((Control)(object)cmbContainerNo1).Text == "" || ((Control)(object)cmbContainerNo1).Text == string.Empty)
			{
				MessageBox.Show("Please Select Container No 1");
				cmbContainerNo1.Focus();
				return false;
			}
			if (((Control)(object)cmbContainerNo1).Text != string.Empty && ((Control)(object)cmbContainerNo1).Text == ((Control)(object)cmbContainerNo2).Text)
			{
				MessageBox.Show("Cannot Save With Same Container No Please Check");
				cmbContainerNo2.Focus();
				return false;
			}
		}
		if (((UltraGridBase)cmbcity).ActiveRow == null || Conversion.ToInt(cmbcity.Value) == 0)
		{
			MessageBox.Show("CityName Field Required");
			cmbcity.Focus();
			return false;
		}
		if (((UltraGridBase)cmbvehicletype).ActiveRow == null || Conversion.ToInt(cmbvehicletype.Value) == 0)
		{
			MessageBox.Show("VehicleType Field Required");
			cmbvehicletype.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtqty.Text.Trim()) == 0 || Conversion.ToInt((object)txtqty.Text.Trim()) < 0)
		{
			MessageBox.Show("ItemQty Field Required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackUnit.Text.Trim()) == 0.0 || Conversion.ToDouble((object)txtPackUnit.Text.Trim()) < 0.0)
		{
			MessageBox.Show("PackUom Field Required");
			txtPackUnit.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtPackUnit.Text.Trim()) > 110)
		{
			MessageBox.Show("Pack Uom Field can't greater 110");
			txtPackUnit.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtWeightCompared.Text.Trim()) == 0.0 || Conversion.ToDouble((object)txtWeightCompared.Text.Trim()) < 0.0)
		{
			MessageBox.Show("Weight Field Required");
			txtWeightCompared.Focus();
			return false;
		}
		if (txtgpno.Text.Trim() == string.Empty || txtgpno.Text.Trim() == "0" || txtgpno.Text.Trim() == null)
		{
			MessageBox.Show("GatePassNo Field Required");
			txtgpno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStatus).ActiveRow == null || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Status Field Required");
			CmbStatus.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWeighBridge).ActiveRow == null || Conversion.ToInt(cmbWeighBridge.Value) == 0)
		{
			MessageBox.Show("WeighBridge Status Field Required");
			return false;
		}
		if (CmbOrderno.Text.Trim() == string.Empty || CmbOrderno.Text.Trim() == "0")
		{
			MessageBox.Show("OrderNo Field Required");
			CmbOrderno.Focus();
			return false;
		}
		double BiltyFreight = Conversion.ToDouble((object)txtfreight.Text.Trim());
		double num = Conversion.ToDouble((object)txtAdvanceByParty.Text.Trim());
		double advanceByFactory = Conversion.ToDouble((object)txtAdvanceByFactory.Text.Trim());
		double totalAdvance = num + advanceByFactory;
		if ((BiltyFreight > 0.0 || totalAdvance > 0.0) && totalAdvance > BiltyFreight)
		{
			MessageBox.Show("Advance freight cannot be greater than bilty freight...");
			txtfreight.Focus();
			return false;
		}
		if ((Conversion.ToInt(CmbOrderType.Value) == 41 || Conversion.ToInt(CmbOrderType.Value) == 105 || Conversion.ToInt(CmbOrderType.Value) == 106) && ((Control)(object)CmbStatus).Text.Trim() == "Accepted" && Conversion.ToDouble((object)txtFactoryWeight.Text) == 0.0)
		{
			MessageBox.Show("Factory weight is required");
			return false;
		}
		if (Conversion.ToInt(CmbOrderType.Value) == 41 && (Conversion.ToDouble((object)txtSupplierFirstWeight.Text) > 0.0 || Conversion.ToDouble((object)txtSupplierSecondWeight.Text) > 0.0))
		{
			if (Conversion.ToDouble((object)txtSupplierFirstWeight.Text) == 0.0)
			{
				MessageBox.Show("Supplier First Weight Field Required");
				txtSupplierFirstWeight.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtSupplierSecondWeight.Text) == 0.0)
			{
				MessageBox.Show("Supplier Second Weight Field Required");
				txtSupplierFirstWeight.Focus();
				return false;
			}
		}
		if (Conversion.ToInt(CmbOrderType.Value) != 104 && Conversion.ToInt(CmbOrderType.Value) != 175 && (((UltraGridBase)cmbsupp).ActiveRow == null || Conversion.ToInt(cmbsupp.Value) == 0))
		{
			MessageBox.Show("Supplier Name Field Required");
			cmbsupp.Focus();
			return false;
		}
		return true;
	}

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 1)
			{
				FromDateHistoryPoInfo.Focus();
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				FromDateStockInfo.Focus();
			}
			else
			{
				((Control)(object)grd).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBreakUpSettings()
	{
		try
		{
			grdPurchaseBrakup.RootTable.Columns["Id"].Visible = false;
			grdPurchaseBrakup.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseBrakup.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["GrossWeight"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["EbWeight"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseBrakup.RootTable.Columns["EbWeight"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["EbWeight"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["EbWeight"].TotalFormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["EBTotal"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseBrakup.RootTable.Columns["EBTotal"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["EBTotal"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["EBTotal"].TotalFormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseBrakup.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdPurchaseBrakup.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["NetWeight"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["NetWeight"].TotalFormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["UOM"].TextAlignment = (TextAlignment)3;
			grdPurchaseBrakup.RootTable.Columns["UOM"].FormatString = "#,##.##";
			grdPurchaseBrakup.RootTable.Columns["UOM"].TotalFormatString = "#,##.##";
			if (LockBreakUp)
			{
				grdPurchaseBrakup.AllowEdit = (InheritableBoolean)2;
			}
			else
			{
				grdPurchaseBrakup.AllowEdit = (InheritableBoolean)1;
				grdPurchaseBrakup.RootTable.Columns["Id"].EditType = (EditType)0;
				grdPurchaseBrakup.RootTable.Columns["EBTotal"].EditType = (EditType)0;
				grdPurchaseBrakup.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
				grdPurchaseBrakup.RootTable.Columns["NetWeight"].EditType = (EditType)0;
				grdPurchaseBrakup.RootTable.Columns.Add("Add");
				grdPurchaseBrakup.RootTable.Columns["Add"].Key = "Add";
				grdPurchaseBrakup.RootTable.Columns["Add"].Caption = "+";
				grdPurchaseBrakup.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPurchaseBrakup.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
				grdPurchaseBrakup.RootTable.Columns["Add"].ButtonText = "+";
				grdPurchaseBrakup.RootTable.Columns["Add"].Width = 20;
				grdPurchaseBrakup.RootTable.Columns["Add"].Position = 0;
				grdPurchaseBrakup.RootTable.Columns.Add("Delete");
				grdPurchaseBrakup.RootTable.Columns["Delete"].Key = "Delete";
				grdPurchaseBrakup.RootTable.Columns["Delete"].Caption = "X";
				grdPurchaseBrakup.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPurchaseBrakup.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdPurchaseBrakup.RootTable.Columns["Delete"].ButtonText = "X";
				grdPurchaseBrakup.RootTable.Columns["Delete"].Width = 20;
				grdPurchaseBrakup.RootTable.Columns["Delete"].Position = 1;
				grdPurchaseBrakup.FrozenColumns = 2;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPurchaseBreakUpGrid()
	{
		try
		{
			dtPurchaseBreakup.Rows.Add(0, 0, 0, 0, 0, 0, 0);
			grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPurchaseBrakup_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdPurchaseBrakup.UpdateData();
			GridEXRow item = grdPurchaseBrakup.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "Qty" || e.Column.Key == "UOM")
				{
					if (item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["UOM"].Value.ToString() != string.Empty)
					{
						double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
						double UOM = Conversion.ToDouble((object)item.Cells["UOM"].Value.ToString());
						double GrossWeight = Qty * UOM;
						item.Cells["GrossWeight"].Value = GrossWeight;
						if (item.Cells["EbWeight"].Value.ToString() != string.Empty)
						{
							double EbWeight = Conversion.ToDouble((object)item.Cells["EbWeight"].Value.ToString());
							double EBTotal = Qty * EbWeight;
							item.Cells["EBTotal"].Value = EBTotal;
							double NetWeight = GrossWeight + EBTotal;
							item.Cells["NetWeight"].Value = NetWeight;
						}
						else
						{
							double NetWeight2 = GrossWeight;
							item.Cells["NetWeight"].Value = NetWeight2;
						}
					}
					else
					{
						item.Cells["GrossWeight"].Value = 0;
					}
				}
				else if (e.Column.Key == "EbWeight")
				{
					double Qty2 = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double UOM2 = Conversion.ToDouble((object)item.Cells["UOM"].Value.ToString());
					double GrossWeight2 = Qty2 * UOM2;
					if (item.Cells["EbWeight"].Value.ToString() != string.Empty)
					{
						double EbWeight2 = Conversion.ToDouble((object)item.Cells["EbWeight"].Value.ToString());
						double EBTotal2 = Qty2 * EbWeight2;
						item.Cells["EBTotal"].Value = EBTotal2;
						double NetWeight3 = GrossWeight2 + EBTotal2;
						item.Cells["NetWeight"].Value = NetWeight3;
					}
					else
					{
						double NetWeight4 = GrossWeight2;
						item.Cells["NetWeight"].Value = NetWeight4;
					}
				}
			}
			grdPurchaseBrakup.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdPurchaseBrakup);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrakup_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPurchaseBrakup.CurrentRow;
				dtPurchaseBreakup.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPurchaseBreakup.Rows.Count == 0)
				{
					AddRowInPurchaseBreakUpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPurchaseBreakUpGrid();
			}
			grdPurchaseBrakup.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmfill()
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
			DataTable dt = GatePassInward.ReadByGPDate(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("GpNo", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GatepassType", typeof(string));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("OrderNo", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(double));
				dtcol.Columns.Add("PackUnit", typeof(double));
				dtcol.Columns.Add("WeightComparedToPoWt", typeof(double));
				dtcol.Columns.Add("SupplierFirstWeight", typeof(double));
				dtcol.Columns.Add("SupplierSecondWeight", typeof(double));
				dtcol.Columns.Add("SupplierWeight", typeof(double));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("DifferenceWeight", typeof(double));
				dtcol.Columns.Add("AccessWeight", typeof(double));
				dtcol.Columns.Add("NetPaid", typeof(double));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("WareHouseName", typeof(string));
				dtcol.Columns.Add("CityName", typeof(string));
				dtcol.Columns.Add("InTime", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Columns.Add("OtherRemarks", typeof(string));
				dtcol.Columns.Add("ApprovalStatus", typeof(string));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["GpSrNo"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GatepassType"], dt.Rows[i]["OrderType"], dt.Rows[i]["SupplierName"], dt.Rows[i]["OrderNo"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["ItemQty"], dt.Rows[i]["PackUnit"], dt.Rows[i]["WeightComparedToPoWt"], dt.Rows[i]["SupplierFirstWeight"], dt.Rows[i]["SupplierSecondWeight"], dt.Rows[i]["SupplierWeight"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["DifferenceWeight"], dt.Rows[i]["AccessWeight"], dt.Rows[i]["NetPaid"], dt.Rows[i]["VarietyName"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["CityName"], Conversion.ToDateTime(dt.Rows[i]["InTime"]), dt.Rows[i]["NoOfAttachments"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["ApprovalStatus"], dt.Rows[i]["Status"]);
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				grdfrmsetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmsetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["WareHouseName"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2);
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["GatepassType"].Caption = "G.Type";
			grd.RootTable.Columns["DifferenceWeight"].Caption = "Difference";
			grd.RootTable.Columns["AccessWeight"].Caption = "Excess Weight";
			grd.RootTable.Columns["InTime"].FormatString = "hh:mm tt";
			grd.RootTable.Columns["SupplierFirstWeight"].Caption = "Supplier Load Weight";
			grd.RootTable.Columns["SupplierSecondWeight"].Caption = "Supplier Tare Weight";
			grd.RootTable.Columns["PackUnit"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["GatepassType"].Width = Constants.InventoryConstants.GpType;
			grd.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.GpType;
			grd.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grd.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.GpType;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["SupplierFirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["SupplierSecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["DifferenceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["AccessWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateTimeConstant;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns["OtherRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["ApprovalStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].Width = 40;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 40;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXColumn AccessWeight = grd.RootTable.Columns["AccessWeight"];
			GridEXColumn ApprovalStatus = grd.RootTable.Columns["ApprovalStatus"];
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

	private void GrdsettingForSupplyOrder()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbWtTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].FormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].FormatString = "#,#";
			grd.RootTable.Columns["EbWtTotal"].FormatString = "#,#";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,#";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,#";
			grd.RootTable.Columns["GrossWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["EbWtTotal"].TotalFormatString = "#,#";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,#";
			CommonServices.GridAutoAdjustmentNew(grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				RecId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
				LabDataGetByGpId(RecId);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
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
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			RecId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			LabDataGetByGpId(RecId);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 51);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockReceivingNotes_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Load")
			{
				GridEXRow item = grdStockReceivingNotes.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CmbOrderType.Value = 175;
					CmbOrderno.Text = Conversion.ToString(item.Cells["GpSrNo"].Value);
					CmbOrderno_Leave(null, null);
					cmbsupp.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void StockReceivingNotesInformationForGatePassInward()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtStockReceiving.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (RadDocDateStockInfo.Checked)
			{
				if (FromDateStockInfo.Checked)
				{
					obj.FromDate = FromDateStockInfo.Value;
				}
				if (ToDateStockInfo.Checked)
				{
					obj.ToDate = ToDateStockInfo.Value;
				}
			}
			else if (RadEntryDateStockInfo.Checked)
			{
				if (FromDateStockInfo.Checked)
				{
					obj.EntryDateFrom = FromDateStockInfo.Value;
				}
				if (ToDateStockInfo.Checked)
				{
					obj.EntryDateTo = ToDateStockInfo.Value;
				}
			}
			else if (RadModifyDateStockInfo.Checked)
			{
				if (FromDateStockInfo.Checked)
				{
					obj.ModifyDateFrom = FromDateStockInfo.Value;
				}
				if (ToDateStockInfo.Checked)
				{
					obj.ModifyDateTo = ToDateStockInfo.Value;
				}
			}
			else if (RadApproveDateStockInfo.Checked)
			{
				if (FromDateStockInfo.Checked)
				{
					obj.ApprovedDateFrom = FromDateStockInfo.Value;
				}
				if (ToDateStockInfo.Checked)
				{
					obj.ApprovedDateTo = ToDateStockInfo.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocStockInfo.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocStockInfo.Text);
			dtStockReceiving = GatePassInward.GetPendingOutwardForReceivingNotes(obj);
			if (dtStockReceiving.Rows.Count > 0)
			{
				DataTable dtTarget = new DataTable();
				dtTarget.Columns.Add("Id", typeof(int));
				dtTarget.Columns.Add("GpDate", typeof(DateTime));
				dtTarget.Columns.Add("GpSrNo", typeof(int));
				dtTarget.Columns.Add("ItemName", typeof(string));
				dtTarget.Columns.Add("VehicleType", typeof(string));
				dtTarget.Columns.Add("VehicleNo", typeof(string));
				dtTarget.Columns.Add("BiltyNo", typeof(string));
				dtTarget.Columns.Add("ItemQty", typeof(decimal));
				dtTarget.Columns.Add("DoWeight", typeof(decimal));
				dtTarget.Columns.Add("FactoryWeight", typeof(decimal));
				dtTarget.Columns.Add("Status", typeof(string));
				dtTarget.Columns.Add("EntryDate", typeof(DateTime));
				dtTarget.Columns.Add("EntryUser", typeof(string));
				dtTarget.Columns.Add("FromBranch", typeof(string));
				dtTarget.Columns.Add("ToBranch", typeof(string));
				dtTarget.Columns.Add("SupplierCustomerId", typeof(int));
				dtTarget.Columns.Add("CustomerName", typeof(string));
				foreach (DataRow row in dtStockReceiving.Rows)
				{
					dtTarget.Rows.Add(row["Id"], row["GpDate"], row["GpSrNo"], row["ItemName"], row["VehicleType"], row["VehicleNo"], row["BiltyNo"], row["ItemQty"], row["DoWeight"], row["FactoryWeight"], row["Status"], row["EntryDate"], row["EntryUser"], row["FromBranch"], row["ToBranch"], row["SupplierCustomerId"], row["CustomerName"]);
				}
				grdStockReceivingNotes.DataSource = dtTarget;
				grdStockReceivingNotes.RetrieveStructure();
				GridSettingStockReceivingNotes();
			}
			else
			{
				grdStockReceivingNotes.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingStockReceivingNotes()
	{
		try
		{
			grdStockReceivingNotes.RootTable.Columns["Id"].Visible = false;
			grdStockReceivingNotes.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdStockReceivingNotes.RootTable.Columns["ItemName"].Width = 250;
			grdStockReceivingNotes.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdStockReceivingNotes.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdStockReceivingNotes.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdStockReceivingNotes.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			grdStockReceivingNotes.RootTable.Columns["DoWeight"].AggregateFunction = (AggregateFunction)2;
			grdStockReceivingNotes.RootTable.Columns["DoWeight"].TextAlignment = (TextAlignment)3;
			grdStockReceivingNotes.RootTable.Columns["DoWeight"].FormatString = "#,#";
			grdStockReceivingNotes.RootTable.Columns["DoWeight"].TotalFormatString = "#,#";
			grdStockReceivingNotes.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdStockReceivingNotes.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdStockReceivingNotes.RootTable.Columns["FactoryWeight"].FormatString = "#,#";
			grdStockReceivingNotes.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,#";
			CommonServices.GridAutoAdjustmentNew(grdStockReceivingNotes);
			grdStockReceivingNotes.RootTable.Columns.Add("Load");
			grdStockReceivingNotes.RootTable.Columns["Load"].Key = "Load";
			grdStockReceivingNotes.RootTable.Columns["Load"].Caption = "Load";
			grdStockReceivingNotes.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdStockReceivingNotes.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdStockReceivingNotes.RootTable.Columns["Load"].ButtonText = "Load";
			grdStockReceivingNotes.RootTable.Columns["Load"].Width = 50;
			grdStockReceivingNotes.RootTable.Columns["Load"].Position = 0;
			grdStockReceivingNotes.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnShowStockInfo_Click(object sender, EventArgs e)
	{
		try
		{
			StockReceivingNotesInformationForGatePassInward();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetStockInfo_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateStockInfo.Value = DateTime.Now;
			ToDateStockInfo.Value = DateTime.Now;
			FromDocStockInfo.Text = string.Empty;
			ToDocStockInfo.Text = string.Empty;
			grdStockReceivingNotes.ClearStructure();
			RadDocDateStockInfo.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderInformationForGatePassInward()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeIds = "41,700";
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (RadDocDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.FromDate = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ToDate = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadEntryDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.EntryDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.EntryDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadModifyDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.ModifyDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ModifyDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadAapprovedDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ApprovedDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistoryPoInfo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistoryPoInfo.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNamePoInfo.Value);
			obj.DocumentTypeId = Conversion.ToInt(CmbDocumentTypePoInfo.Value);
			obj.ExpiryDays = Conversion.ToInt((object)txtExpiryDaysPoInfo.Text);
			DataTable dt = PurchaseOrder.GetPurchaseOrderInformationForGatepassInward(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("BranchName", typeof(string));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocumentType", typeof(string));
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("OrderQty", typeof(double));
				dtcol.Columns.Add("OrderWeight", typeof(double));
				dtcol.Columns.Add("GpReceivedQty", typeof(double));
				dtcol.Columns.Add("GpReceivedWeight", typeof(double));
				dtcol.Columns.Add("BalQtyByGp", typeof(double));
				dtcol.Columns.Add("BalWeightByGp", typeof(double));
				dtcol.Columns.Add("GrnReceivedQty", typeof(double));
				dtcol.Columns.Add("GrnReceivedWeight", typeof(double));
				dtcol.Columns.Add("BalQtyByGrn", typeof(double));
				dtcol.Columns.Add("BalWeightByGrn", typeof(double));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("ApprovedStatus", typeof(string));
				dtcol.Columns.Add("OrderStatus", typeof(string));
				dtcol.Columns.Add("ExpiryDate", typeof(DateTime));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["BranchName"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocumentTypeDescription"], dt.Rows[i]["OrderNo"], dt.Rows[i]["SupplierName"], dt.Rows[i]["ItemName"], dt.Rows[i]["PackUom"], dt.Rows[i]["OrderQty"], dt.Rows[i]["OrderWeight"], dt.Rows[i]["ReceivedQty"], dt.Rows[i]["ReceivedWeight"], dt.Rows[i]["BalQty"], dt.Rows[i]["BalWeight"], dt.Rows[i]["GrnReceivedQty"], dt.Rows[i]["GrnReceivedWeight"], dt.Rows[i]["BalQtyByGrn"], dt.Rows[i]["BalWeightByGrn"], dt.Rows[i]["RemarksHeader"], dt.Rows[i]["ApprovedStatus"], dt.Rows[i]["OrderStatus"], dt.Rows[i]["OrderExpiryDate"]);
				}
				grdPurchaseOrder.DataSource = dtcol;
				grdPurchaseOrder.RetrieveStructure();
				GridSettingPurchaseOrder();
			}
			else
			{
				grdPurchaseOrder.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void PurchaseOrderInformationForGatePassInwardForSteel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (RadDocDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.FromDate = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ToDate = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadEntryDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.EntryDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.EntryDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadModifyDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.ModifyDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ModifyDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			else if (RadAapprovedDatePoInfo.Checked)
			{
				if (FromDateHistoryPoInfo.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistoryPoInfo.Value;
				}
				if (ToDateHistoryPoInfo.Checked)
				{
					obj.ApprovedDateTo = ToDateHistoryPoInfo.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistoryPoInfo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistoryPoInfo.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierNamePoInfo.Value);
			obj.DocumentTypeId = Conversion.ToInt(CmbDocumentTypePoInfo.Value);
			obj.ExpiryDays = Conversion.ToInt((object)txtExpiryDaysPoInfo.Text);
			DataTable dt = PurchaseOrder.PurchaseOrderInformationForGPInward_Steel(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocumentType", typeof(string));
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("OrderQty", typeof(double));
				dtcol.Columns.Add("OrderWeight", typeof(double));
				dtcol.Columns.Add("GpReceivedQty", typeof(double));
				dtcol.Columns.Add("GpReceivedWeight", typeof(double));
				dtcol.Columns.Add("BalQtyByGp", typeof(double));
				dtcol.Columns.Add("BalWeightByGp", typeof(double));
				dtcol.Columns.Add("GrnReceivedQty", typeof(double));
				dtcol.Columns.Add("GrnReceivedWeight", typeof(double));
				dtcol.Columns.Add("BalQtyByGrn", typeof(double));
				dtcol.Columns.Add("BalWeightByGrn", typeof(double));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("ApprovedStatus", typeof(string));
				dtcol.Columns.Add("OrderStatus", typeof(string));
				dtcol.Columns.Add("ExpiryDate", typeof(DateTime));
				dtcol.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocumentTypeDescription"], dt.Rows[i]["OrderNo"], dt.Rows[i]["SupplierName"], dt.Rows[i]["ItemName"], dt.Rows[i]["PackUom"], dt.Rows[i]["OrderQty"], dt.Rows[i]["OrderWeight"], dt.Rows[i]["ReceivedQty"], dt.Rows[i]["ReceivedWeight"], dt.Rows[i]["BalQty"], dt.Rows[i]["BalWeight"], dt.Rows[i]["GrnReceivedQty"], dt.Rows[i]["GrnReceivedWeight"], dt.Rows[i]["BalQtyByGrn"], dt.Rows[i]["BalWeightByGrn"], dt.Rows[i]["RemarksHeader"], dt.Rows[i]["ApprovedStatus"], dt.Rows[i]["OrderStatus"], dt.Rows[i]["OrderExpiryDate"]);
				}
				grdPurchaseOrder.DataSource = dtcol;
				grdPurchaseOrder.RetrieveStructure();
				GridSettingPurchaseOrder();
			}
			else
			{
				grdPurchaseOrder.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPurchaseOrder()
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			grdPurchaseOrder.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdPurchaseOrder.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdPurchaseOrder.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
				if (column.Key.Contains("Qty"))
				{
					column.Width = 80;
				}
				if (column.Key.Contains("Weight"))
				{
					column.Width = 105;
				}
				if (column.Key.Contains("Status"))
				{
					column.Width = 70;
				}
			}
			grdPurchaseOrder.RootTable.Columns["DocumentType"].Width = 120;
			grdPurchaseOrder.RootTable.Columns["OrderNo"].Width = 70;
			grdPurchaseOrder.RootTable.Columns["PackUom"].Width = 60;
			grdPurchaseOrder.RootTable.Columns["RemarksHeader"].Width = 130;
			grdPurchaseOrder.RootTable.Columns["SupplierName"].Width = 220;
			grdPurchaseOrder.RootTable.Columns["ItemName"].Width = 180;
			grdPurchaseOrder.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			ctrlGrdBar3_Load(null, null);
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
			column.FormatString = "#,##0";
			column.TotalFormatString = "#,##0";
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			if (POInfoDocumentTypeId == 41)
			{
				PurchaseOrderInformationForGatePassInward();
			}
			else if (POInfoDocumentTypeId == 1500)
			{
				PurchaseOrderInformationForGatePassInwardForSteel();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderInformationComboFill(int DocumentTypeId)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Expected O, but got Unknown
		try
		{
			DataTable Dttype = new DataTable();
			Dttype.Columns.Add("Id", typeof(int));
			Dttype.Columns.Add("name", typeof(string));
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			switch (DocumentTypeId)
			{
			case 41:
			{
				DataTable dt = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId
				});
				if (dt.Rows.Count <= 0)
				{
					break;
				}
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DocumentType")
					{
						Dttype.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
				break;
			}
			case 1500:
			{
				DataTable dt2 = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(UserAccount.OrganizationId, UserAccount.OrganizationId, (string)null);
				if (dt2.Rows.Count <= 0)
				{
					break;
				}
				for (int j = 0; j < dt2.Rows.Count; j++)
				{
					if (Conversion.ToString(dt2.Rows[j]["Activity"]) == "DocumentType")
					{
						Dttype.Rows.Add(dt2.Rows[j]["Id"], dt2.Rows[j]["ReferenceName"]);
					}
					if (Conversion.ToString(dt2.Rows[j]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt2.Rows[j]["Id"], dt2.Rows[j]["ReferenceName"]);
					}
				}
				break;
			}
			}
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(dtSupplier, cmbSupplierNamePoInfo, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
			}
			if (Dttype.Rows.Count > 0)
			{
				DDL.BindDDL(Dttype, CmbDocumentTypePoInfo, "Id", "name", "DocumentType", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetOrderInfo_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistoryPoInfo.Value = DateTime.Now;
			ToDateHistoryPoInfo.Value = DateTime.Now;
			txtFromDocNoHistoryPoInfo.Text = string.Empty;
			txtToDocNoHistoryPoInfo.Text = string.Empty;
			((Control)(object)cmbSupplierNamePoInfo).Text = string.Empty;
			grdPurchaseOrder.ClearStructure();
			drdocdate.Checked = true;
			OrderInformationComboFill(POInfoDocumentTypeId);
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
			if (Conversion.ToInt(CmbOrderType.Value) == 175)
			{
				StockReceivingNotesInformationForGatePassInward();
			}
			RecId = 0;
			ActionIdForSpecialApproval = 0;
			lblDiffRemarks.Visible = false;
			txtWeightDiffRemarks.Visible = false;
			txtSupplierWeight.Enabled = true;
			txtSupplierWeight.Tag = "";
			txtfreight.Enabled = true;
			txtNetPaid.Enabled = true;
			txtAdvanceByFactory.Enabled = true;
			txtAdvanceByParty.Enabled = true;
			txtAdvanceByFactory.Text = string.Empty;
			txtAdvanceByParty.Text = string.Empty;
			txtTotalPayablesFreight.Text = string.Empty;
			rdFreightCash.Enabled = true;
			rdFreightCredit.Enabled = true;
			tabControl2.SelectedIndex = 0;
			POId = 0;
			UpdateMode = false;
			((Control)(object)CmbOrderType).Enabled = true;
			txtPackUnit.Enabled = true;
			((UltraGridBase)CmbOrderType).Rows[1].Activate();
			Conversion.ToInt(CmbOrderType.Value);
			_ = 41;
			cmbgptype_Leave_1(null, null);
			((Control)(object)cmbContainerNo1).Text = string.Empty;
			((Control)(object)cmbContainerNo2).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtBalQty.Text = string.Empty;
			CmbOrderno.Text = "0";
			((Control)(object)cmbsupp).Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtbiltyno.Text = string.Empty;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			((UltraGridBase)cmbWeighBridge).Rows[0].Activate();
			((UltraGridBase)CmbVariety).DataSource = null;
			((Control)(object)CmbVariety).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			((Control)(object)cmbsupp).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtfreight.Text = string.Empty;
			txtSupplierWeight.Text = string.Empty;
			txtSupplierFirstWeight.Text = string.Empty;
			txtSupplierSecondWeight.Text = string.Empty;
			txtFirstWeight.Text = string.Empty;
			txtsecondWeight.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
			txtDifferenceWeight.Text = string.Empty;
			txtWeighBridgeSlipNo.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			txtLabReportNo.Text = string.Empty;
			txtReportStatus.Text = string.Empty;
			txtLabRemarks.Text = string.Empty;
			txtPackUnit.Text = string.Empty;
			table.Rows.Clear();
			grd.DataSource = table;
			gpnofill();
			grdfrmfill();
			txtNetPaid.Text = string.Empty;
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			txtvehicleno.Enabled = true;
			LockBreakUp = false;
			dtPurchaseBreakup.Rows.Clear();
			AddRowInPurchaseBreakUpGrid();
			grdPurchaseBrakup.RetrieveStructure();
			grdPurchaseBreakUpSettings();
			txtgpdate.Focus();
			CmbOrderno.Enabled = true;
			((Control)(object)cmbWeighBridge).Enabled = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			txtAccessWeight.Text = string.Empty;
			RadGrnFormToOpenOnInsert.Visible = false;
			((Control)(object)CmbSupplierDispatchedPreBillNo).Text = string.Empty;
			((UltraGridBase)CmbSupplierDispatchedPreBillNo).DataSource = null;
			DriverBioId = 0;
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtWhatsAppNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtFatherCNIC.Text = string.Empty;
			txtDriverCellNo.Text = "092-3-";
			txtWhatsAppNo.Text = "092-3-";
			txtAlternateCellNo.Text = "092-3-";
			DriverFieldsDisableOrEnable(flag: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "DriverBioInfo", "InvPackingType" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			ItemNameFill();
			PackingTypeDtFillFromGlobalAndBind();
			gatepasstype();
			vehicleTypefill();
			CityFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InwardGatePass_FormClosing(object sender, FormClosingEventArgs e)
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

	public void ReadById(int ID)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			UpdateMode = true;
			RadGrnFormToOpenOnInsert.Visible = true;
			GatePassInward gatePass = new GatePassInward();
			RecId = Conversion.ToInt((object)ID);
			gatePass = GatePassInward.GetByID(RecId);
			if (gatePass != null)
			{
				tabControl1.SelectedIndex = 0;
				((Control)(object)cmbContainerNo1).Text = Conversion.ToString((object)gatePass.Container);
				((Control)(object)cmbContainerNo2).Text = Conversion.ToString((object)gatePass.Container1);
				txtbiltyno.Text = gatePass.BiltyNo;
				cmbcity.Value = gatePass.CityId;
				txtfreight.Text = gatePass.Freight.ToString();
				((Control)(object)cmbgptype).Text = gatePass.GatepassType.ToString();
				cmbgptype_Leave(null, null);
				((Control)(object)cmbWeighBridge).Text = Conversion.ToString((object)gatePass.DocAttachment);
				txtgpdate.Value = gatePass.GpDate;
				txtgpno.Text = Conversion.ToString((object)gatePass.GpSrNo);
				txtgptypeno.Text = gatePass.GpTypeSrNo.ToString();
				CmbPackingType.Value = gatePass.PackingTypeId;
				txtqty.Text = gatePass.NoOfPackages.ToString();
				txtPackUnit.Text = Conversion.ToDouble((object)Math.Round(gatePass.PackUnit, 2)).ToString();
				txtPackUnit.Enabled = false;
				txtWeightCompared.Text = Conversion.ToString((object)gatePass.WeightComparedToPoWt);
				txtAccessWeight.Text = Conversion.ToString((object)gatePass.AccessWeight);
				ChkIsApproved.Checked = gatePass.IsApproved;
				ActionIdForSpecialApproval = gatePass.ActionIdForSpecialApproval;
				txtremarks.Text = gatePass.OtherRemarks;
				if (gatePass.FreightOn == 2)
				{
					rdFreightCredit.Checked = true;
				}
				else
				{
					rdFreightCash.Checked = true;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.InDateTimeStamp)))
				{
					txtintime.Value = Conversion.ToDateTime((object)gatePass.InDateTimeStamp);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.OutDateTimeStamp)))
				{
					txtouttime.Value = Conversion.ToDateTime((object)gatePass.OutDateTimeStamp);
				}
				txtBiltyDate.Value = Conversion.ToDateTime((object)gatePass.BiltyDate);
				GpQrCode = Conversion.ToByteArray((object)gatePass.GpQRCode);
				if (GpQrCode == null)
				{
					QRCodeGenerate();
				}
				txtWeighBridgeSlipNo.Text = gatePass.WeighBridgeId.ToString();
				txtSupplierWeight.Text = gatePass.SupplierWeight.ToString();
				txtSupplierFirstWeight.Text = gatePass.SupplierFirstWeight.ToString();
				txtSupplierSecondWeight.Text = gatePass.SupplierSecondWeight.ToString();
				txtFactoryWeight.Text = gatePass.FactoryWeight.ToString();
				CmbOrderno.Text = gatePass.SupplierContractCode;
				txtvehicleno.Text = gatePass.VehicleNo;
				txtvehicleno_Leave(null, null);
				((Control)(object)cmbvehicletype).Text = gatePass.VehicleType;
				GetWeighBridgeWeightAndTicketNos(RecId);
				if (gatePass.RefDocumentTypeId == 41 && gatePass.GatepassType != "Import")
				{
					((UltraGridBase)CmbOrderType).Rows[0].Activate();
					ReportsParameters parameters = new ReportsParameters();
					parameters.OrganizationId = UserAccount.OrganizationId;
					parameters.CompanyId = UserAccount.CompanyId;
					parameters.BranchesId = UserAccount.BranchesId;
					parameters.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
					parameters.DocumentTypeId = 41;
					parameters.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					parameters.Id = RecId;
					dtcust = SupplierCustomer.GetSupplierByPurchaseOrderNo(parameters);
					if (dtcust.Rows.Count > 0)
					{
						POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
						DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DDL.BindDDLNew(dtcust, CmbVariety, "ItemId", "ItemName", "ItemName", false);
						((UltraGridBase)CmbVariety).Rows[0].Activate();
					}
				}
				else if (gatePass.RefDocumentTypeId == 700 && gatePass.GatepassType != "Import")
				{
					((UltraGridBase)CmbOrderType).Rows[0].Activate();
					ReportsParameters parameters2 = new ReportsParameters();
					parameters2.OrganizationId = UserAccount.OrganizationId;
					parameters2.CompanyId = UserAccount.CompanyId;
					parameters2.BranchesId = UserAccount.BranchesId;
					parameters2.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
					parameters2.DocumentTypeId = 700;
					parameters2.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					parameters2.Id = RecId;
					dtcust = SupplierCustomer.GetSupplierByPurchaseOrderNo(parameters2);
					if (dtcust.Rows.Count > 0)
					{
						POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
						DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DDL.BindDDLNew(dtcust, CmbVariety, "ItemId", "ItemName", "ItemName", false);
						((UltraGridBase)CmbVariety).Rows[0].Activate();
					}
				}
				else if (gatePass.RefDocumentTypeId == 1500 && gatePass.GatepassType != "Import")
				{
					ReportsParameters parameters3 = new ReportsParameters();
					parameters3.OrganizationId = UserAccount.OrganizationId;
					parameters3.CompanyId = UserAccount.CompanyId;
					parameters3.DocumentTypeId = 1500;
					parameters3.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					parameters3.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
					dtcust = PurchaseOrder.GetPurchaseOrderForGatePassInwardByOrderId(parameters3);
					if (dtcust.Rows.Count > 0)
					{
						POId = Conversion.ToInt(dtcust.Rows[0]["PurchaseOrderId"]);
						DDL.BindDDLNew(dtcust, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DDL.BindDDLNew(dtcust, CmbVariety, "Id", "ItemName", "ItemName", false);
						((UltraGridBase)CmbVariety).Rows[0].Activate();
					}
				}
				else if (gatePass.RefDocumentTypeId == 232 && gatePass.GatepassType == "Import")
				{
					cmbsupp.Value = Conversion.ToInt((object)gatePass.SupplierCustomerId);
					BindSupplierCustomer();
				}
				else if (gatePass.RefDocumentTypeId != 104)
				{
					if (gatePass.RefDocumentTypeId == 105 || gatePass.RefDocumentTypeId == 106 || gatePass.RefDocumentTypeId == 98 || gatePass.RefDocumentTypeId == 52)
					{
						BindLocalSupplierCustomer(Conversion.ToInt(cmbsupp.Value));
						((Control)(object)CmbOrderType).Text = gatePass.OtherSupCust;
						CmbOrderno.Enabled = false;
					}
					else if (gatePass.RefDocumentTypeId == 241 || gatePass.RefDocumentTypeId == 204)
					{
						BindAllSupplierCustomer(Conversion.ToInt(cmbsupp.Value));
						((Control)(object)CmbOrderType).Text = gatePass.OtherSupCust;
						CmbOrderno.Enabled = false;
					}
					else if (gatePass.RefDocumentTypeId == 175)
					{
						POId = gatePass.PurchaseOrderId;
						BindLocalSupplierCustomer(Conversion.ToInt(cmbsupp.Value));
						((Control)(object)CmbOrderType).Text = gatePass.OtherSupCust;
						cmbgptype_Leave(null, null);
						cmbsupp.Focus();
					}
				}
				((Control)(object)CmbOrderType).Text = gatePass.OtherSupCust;
				cmbsupp.Value = Conversion.ToInt((object)gatePass.SupplierCustomerId);
				((Control)(object)CmbStatus).Text = gatePass.Status.ToString();
				if (((Control)(object)CmbStatus).Text == "Rejected")
				{
					((UltraGridBase)CmbStatus).Rows[2].Activate();
				}
				else if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0 && gatePass.Status.ToString() == "Open")
				{
					((UltraGridBase)CmbStatus).Rows[1].Activate();
				}
				txtAdvanceByParty.Text = ((gatePass.AdvanceByParty > 0.0) ? Convert.ToString(gatePass.AdvanceByParty) : "0");
				txtAdvanceByFactory.Text = ((gatePass.AdvanceByFactory > 0.0) ? Convert.ToString(gatePass.AdvanceByFactory) : "0");
				txtNetPaid.Text = gatePass.NetPaid.ToString();
				((Control)(object)CmbVariety).Text = gatePass.VarietyName;
				if (txtFactoryWeight.Text != string.Empty && txtFactoryWeight.Text != "0")
				{
					((Control)(object)cmbWeighBridge).Enabled = false;
				}
				else
				{
					((Control)(object)cmbWeighBridge).Enabled = true;
				}
				txtDifferenceWeight.Text = gatePass.DifferenceWeight.ToString();
				Total();
				if (Conversion.ToInt((object)gatePass.freightVoucherId) > 0)
				{
					if (Conversion.ToDouble((object)gatePass.SupplierWeight) == 0.0)
					{
						txtSupplierWeight.Text = txtFactoryWeight.Text;
					}
					txtSupplierFirstWeight.Enabled = false;
					txtSupplierSecondWeight.Enabled = false;
					txtfreight.Enabled = false;
					txtNetPaid.Enabled = false;
					rdFreightCash.Enabled = false;
					rdFreightCredit.Enabled = false;
				}
				else
				{
					txtSupplierFirstWeight.Enabled = true;
					txtSupplierSecondWeight.Enabled = true;
					txtSupplierWeight.Enabled = true;
					txtfreight.Enabled = true;
					txtNetPaid.Enabled = true;
					rdFreightCash.Enabled = true;
					rdFreightCredit.Enabled = true;
				}
				DriverBioId = gatePass.driverBioDataId;
				if (DriverBioId > 0)
				{
					txtCNIC.Text = gatePass.DriverCNICNO;
					txtDriverCellNo.Text = gatePass.DriverMobileNo;
					txtWhatsAppNo.Text = gatePass.whatsappNo;
					txtAlternateCellNo.Text = gatePass.AlternateCellNo;
					txtDriverName.Text = gatePass.DriverName;
					txtFatherName.Text = gatePass.FatherName;
					txtFatherCNIC.Text = gatePass.fatherCnicNo;
					DriverFieldsDisableOrEnable(flag: false);
				}
				dtPurchaseBreakup.Clear();
				LockBreakUp = false;
				if (gatePass.GatePassInwardPurchaseBreakUpList.Count > 0)
				{
					LockBreakUp = true;
					for (int i = 0; i < gatePass.GatePassInwardPurchaseBreakUpList.Count; i++)
					{
						dtPurchaseBreakup.Rows.Add(gatePass.GatePassInwardPurchaseBreakUpList[i].Id, gatePass.GatePassInwardPurchaseBreakUpList[i].Qty, gatePass.GatePassInwardPurchaseBreakUpList[i].UOM, gatePass.GatePassInwardPurchaseBreakUpList[i].GrossWeight, gatePass.GatePassInwardPurchaseBreakUpList[i].EbWeight, gatePass.GatePassInwardPurchaseBreakUpList[i].EBTotal, gatePass.GatePassInwardPurchaseBreakUpList[i].NetWeight);
					}
				}
				if (dtPurchaseBreakup.Rows.Count == 0)
				{
					AddRowInPurchaseBreakUpGrid();
				}
				grdPurchaseBrakup.DataSource = dtPurchaseBreakup;
				grdPurchaseBrakup.RetrieveStructure();
				grdPurchaseBreakUpSettings();
				if (gatePass.RefDocumentTypeId != 105)
				{
					txtAdvanceByFactory.Enabled = true;
					txtAdvanceByParty.Enabled = true;
				}
				else
				{
					txtAdvanceByFactory.Text = "0";
					txtAdvanceByParty.Text = "0";
					txtAdvanceByFactory.Enabled = false;
					txtAdvanceByParty.Enabled = false;
				}
				if (gatePass.RefferedInWbOrLab)
				{
					((Control)(object)CmbOrderType).Enabled = false;
					CmbOrderno.Enabled = false;
				}
				PreBillNoFill(Conversion.ToInt(cmbsupp.Value), POId);
				CmbSupplierDispatchedPreBillNo.Value = gatePass.SupplierDispatchId;
				CmbSupplierDispatchedPreBillNo_Leave(null, null);
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				cmbsupp.Focus();
			}
			else
			{
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Expected O, but got Unknown
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Expected O, but got Unknown
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if ((Conversion.ToInt(CmbOrderType.Value) == 204 || Conversion.ToInt(CmbOrderType.Value) == 241) && Conversion.ToInt(CmbVariety.Value) == 0)
			{
				MessageBox.Show("Please select Variety First");
				return;
			}
			if (((Control)(object)cmbgptype).Text != "Export Return")
			{
				if (Conversion.ToInt(CmbOrderType.Value) == 41)
				{
					if (((Control)(object)cmbgptype).Text != "Export" && ((Control)(object)cmbgptype).Text != "Import")
					{
						ReportsParameters parameters = new ReportsParameters();
						parameters.OrganizationId = UserAccount.OrganizationId;
						parameters.CompanyId = UserAccount.CompanyId;
						parameters.BranchesId = UserAccount.BranchesId;
						parameters.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
						parameters.DocumentTypeId = 41;
						parameters.DocDate = txtgpdate.Value;
						parameters.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						parameters.Id = RecId;
						dtcust = SupplierCustomer.GetSupplierByPurchaseOrderNo(parameters);
					}
				}
				else if (Conversion.ToInt(CmbOrderType.Value) == 1500)
				{
					if (((Control)(object)cmbgptype).Text != "Export" && ((Control)(object)cmbgptype).Text != "Import")
					{
						ReportsParameters parameters2 = new ReportsParameters();
						parameters2.OrganizationId = UserAccount.OrganizationId;
						parameters2.CompanyId = UserAccount.CompanyId;
						parameters2.OrderId = Conversion.ToInt((object)CmbOrderno.Text);
						parameters2.DocumentTypeId = 1500;
						parameters2.DocDate = txtgpdate.Value;
						parameters2.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						dtcust = PurchaseOrder.GetPurchaseOrderForGatePassInwardByOrderId(parameters2);
					}
				}
				else if (Conversion.ToInt(CmbOrderType.Value) == 104)
				{
					InvSupplyOrder.GetOutstandingSupplyOrderForGatePassInward(new GatePassInward
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = Conversion.ToInt((object)CmbOrderno.Text.Trim()),
						GpDate = txtgpdate.Value,
						FinancialYearId = clsGlobalVariables.ActiveYr.Id
					});
				}
				else if (Conversion.ToInt(CmbOrderType.Value) == 232)
				{
					if (((Control)(object)cmbgptype).Text == "Import")
					{
						SupplierCustomer.GetSupplierByImportPurchaseContract(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							OrderId = Conversion.ToInt((object)CmbOrderno.Text),
							DocumentTypeId = 232,
							FinancialYearId = clsGlobalVariables.ActiveYr.Id
						});
					}
					else
					{
						MessageBox.Show("Please select Gate pass Type Import First");
					}
				}
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GatePassInward gatePass = new GatePassInward();
			gatePass.BiltyNo = txtbiltyno.Text.Trim();
			gatePass.CityId = Conversion.ToInt(cmbcity.Value);
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.DocumentTypeId = 51;
			if (txtfreight.Text.Trim() != string.Empty)
			{
				gatePass.Freight = float.Parse(txtfreight.Text.ToString());
			}
			else
			{
				gatePass.Freight = 0.0;
			}
			gatePass.AdvanceByParty = Conversion.ToDouble((object)txtAdvanceByParty.Text.Trim());
			gatePass.AdvanceByFactory = Conversion.ToDouble((object)txtAdvanceByFactory.Text.Trim());
			if (Conversion.ToInt(CmbOrderType.Value) == 105 && (gatePass.AdvanceByFactory > 0.0 || gatePass.AdvanceByParty > 0.0))
			{
				txtAdvanceByParty.Text = "0";
				txtAdvanceByFactory.Text = "0";
				throw new Exception("In Case Of Market Purchase Advance by Factory or Advance by Party can't be greater than 0");
			}
			gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
			gatePass.GpDate = txtgpdate.Value;
			gatePass.BiltyDate = txtBiltyDate.Value;
			gatePass.Container = ((Control)(object)cmbContainerNo1).Text.ToString();
			gatePass.Container1 = ((Control)(object)cmbContainerNo2).Text.ToString();
			gatePass.GpSrNo = Conversion.ToInt((object)txtgpno.Text.Trim());
			gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
			gatePass.InDateTimeStamp = DateTime.Now;
			gatePass.OtherSupCust = ((Control)(object)CmbOrderType).Text;
			gatePass.NoOfPackages = Conversion.ToInt((object)txtqty.Text.Trim());
			gatePass.PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
			gatePass.WeightComparedToPoWt = Conversion.ToDouble((object)txtWeightCompared.Text.Trim());
			gatePass.OtherRemarks = txtremarks.Text.Trim();
			gatePass.SupplierContractCode = CmbOrderno.Text.Trim();
			gatePass.VehicleNo = txtvehicleno.Text.Trim();
			gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupp.Value);
			gatePass.SupplierDispatchId = Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value);
			gatePass.PackingTypeId = Conversion.ToInt(CmbPackingType.Value);
			gatePass.Status = ((Control)(object)CmbStatus).Text.Trim().ToString();
			gatePass.GpQRCode = Conversion.ToByteArray((object)GpQrCode);
			if (txtNetPaid.Text.Trim() != string.Empty && txtNetPaid.Text.Trim() != null)
			{
				gatePass.NetPaid = float.Parse(txtNetPaid.Text.Trim());
			}
			else
			{
				gatePass.NetPaid = 0.0;
			}
			gatePass.SupplierFirstWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
			gatePass.SupplierSecondWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
			if (txtSupplierWeight.Text.Trim() != string.Empty && txtSupplierWeight.Text.Trim() != null)
			{
				gatePass.SupplierWeight = float.Parse(txtSupplierWeight.Text.Trim());
			}
			else
			{
				gatePass.SupplierWeight = 0.0;
			}
			gatePass.FactoryWeight = 0.0;
			if (txtDifferenceWeight.Text.Trim() != string.Empty && txtDifferenceWeight.Text.Trim() != null)
			{
				gatePass.DifferenceWeight = float.Parse(txtDifferenceWeight.Text.Trim());
			}
			else
			{
				gatePass.DifferenceWeight = 0.0;
			}
			gatePass.FreightOn = 1;
			gatePass.DocAttachment = Conversion.ToString((object)((Control)(object)cmbWeighBridge).Text);
			gatePass.WeighBridgeId = Conversion.ToInt((object)txtWeighBridgeSlipNo.Text.Trim());
			gatePass.VarietyName = ((Control)(object)CmbVariety).Text.Trim();
			gatePass.ItemId = Conversion.ToInt(CmbVariety.Value);
			gatePass.RefDocumentTypeId = Conversion.ToInt(CmbOrderType.Value);
			gatePass.PurchaseOrderId = POId;
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.EntryUser = UserAccount.ID;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (gatePass.Status == "Rejected" && MessageBox.Show("Are you sure to Reject GatePass?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			if (formright.DoHaveApproveRights)
			{
				gatePass.IsApproved = ChkIsApproved.Checked && formright.DoHaveApproveRights;
			}
			gatePass.GatePassInwardPurchaseBreakUpList = new List<GatePassInwardPurchaseBreakUp>();
			if (grdPurchaseBrakup.RowCount > 0)
			{
				GridEXRow[] rows = grdPurchaseBrakup.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					GatePassInwardPurchaseBreakUp rr = new GatePassInwardPurchaseBreakUp();
					rr.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					rr.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					rr.UOM = Conversion.ToDecimal(r2.Cells["UOM"].Value);
					rr.GrossWeight = Conversion.ToDecimal(r2.Cells["GrossWeight"].Value);
					rr.EbWeight = Conversion.ToDecimal(r2.Cells["EbWeight"].Value);
					rr.EBTotal = Conversion.ToDecimal(r2.Cells["EBTotal"].Value);
					rr.NetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					gatePass.GatePassInwardPurchaseBreakUpList.Add(rr);
				}
			}
			if (gatePass.RefDocumentTypeId == 41)
			{
				DataTable dtAccessWeight = GatePassInward.GetAccessWeightByWeightComparedToPoWtAndGpId(gatePass, 0.0);
				if (dtAccessWeight.Rows.Count > 0)
				{
					double AccessWeight = Conversion.ToDouble(dtAccessWeight.Rows[0]["AccessWeight"]);
					if (AccessWeight > 0.0)
					{
						double BalWeightOfPo = Conversion.ToDouble(dtAccessWeight.Rows[0]["BalWeightOfPo"]);
						if (MessageBox.Show($"The GatePass weight exceeds the PO weight. The remaining PO weight is {BalWeightOfPo} and the access weight is {AccessWeight}.\nAre you sure you want to Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return;
						}
						gatePass.AccessWeight = AccessWeight;
						gatePass.IsApproved = false;
						gatePass.PostState = false;
						gatePass.ActionIdForSpecialApproval = 1;
					}
					else
					{
						gatePass.PostDate = DateTime.Now;
						gatePass.PostState = formright.DoHaveApproveRights;
						gatePass.PostUser = UserAccount.ID;
					}
				}
			}
			else
			{
				gatePass.PostDate = DateTime.Now;
				gatePass.PostState = formright.DoHaveApproveRights;
				gatePass.PostUser = UserAccount.ID;
				gatePass.ActionIdForSpecialApproval = 0;
				gatePass.AccessWeight = 0.0;
			}
			gatePass.driverBioDataId = DriverBioId;
			gatePass.DriverCNICNO = txtCNIC.Text;
			gatePass.DriverMobileNo = txtDriverCellNo.Text;
			gatePass.AlternateCellNo = txtAlternateCellNo.Text;
			gatePass.whatsappNo = txtWhatsAppNo.Text;
			gatePass.DriverName = txtDriverName.Text;
			gatePass.FatherName = txtFatherName.Text;
			gatePass.fatherCnicNo = txtFatherCNIC.Text;
			gatePass.AttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GatePassInward>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = GatePassInward.Save(gatePass);
			if (success >= 1)
			{
				MessageBox.Show("Record Save Successfully[" + gatePass.GpSrNo + "]");
			}
			Reset();
			OpenLinkformOnInsert(success, "");
			if (ChkBox.Checked)
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(success);
			}
			btnupdate.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			if (chkDriverInfoForm.Checked)
			{
				if (!Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Select((tblUserRights r) => r.ScreenName == "frmDriverBioForInWard")))
				{
					MessageBox.Show("You Don't Have View-right Of This Driver-Bio For Inward..");
					return;
				}
				frmDriverBio obj = new frmDriverBio(UserAccount)
				{
					RefDocumentTypeId = 51
				};
				obj.cmbGatePass.Value = success;
				obj.cmbGatePass_TextChanged(null, null);
				obj.Show();
			}
			if (RadGrnFormToOpenOnInsert.Checked)
			{
				RadNoneFormToOpenOnInsert.Checked = true;
			}
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because Id not Found");
			}
			if ((Conversion.ToInt(CmbOrderType.Value) == 204 || Conversion.ToInt(CmbOrderType.Value) == 241) && Conversion.ToInt(CmbVariety.Value) == 0)
			{
				MessageBox.Show("Please select Variety First");
			}
			else
			{
				if (!formvalidation())
				{
					return;
				}
				double supplierWeight = Conversion.ToDouble((object)txtSupplierWeight.Text);
				double factoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
				if (factoryWeight > supplierWeight)
				{
					lblDiffRemarks.Visible = true;
					txtWeightDiffRemarks.Visible = true;
					double diff = factoryWeight - supplierWeight;
					double maxAllowedDiff = Math.Round(0.03 * factoryWeight, 0);
					double minAllowedDiff = Math.Round(0.01 * factoryWeight, 0);
					string remarks = txtWeightDiffRemarks.Text.Trim();
					if (diff >= maxAllowedDiff)
					{
						if (string.IsNullOrEmpty(remarks))
						{
							MessageBox.Show($"Difference '{diff}' between Factory Weight '{factoryWeight}' and Supplier Weight '{supplierWeight}'\n" + $"is **greater than or equal to** maximum allowed 3% of factory weight ({maxAllowedDiff}).\n\n" + "Please add Weight Difference Remarks if you want to proceed.", "Weight Difference Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
					}
					else if (diff >= minAllowedDiff && MessageBox.Show($"Difference '{diff}' between Factory Weight '{factoryWeight}' and Supplier Weight '{supplierWeight}'\n" + $"is **greater than or equal to** the minimum allowed 1% ({minAllowedDiff:F2})\n" + $"but **less than** the maximum allowed 3% ({maxAllowedDiff:F2}).\n\n" + "Do you want to proceed?", "Weight Difference Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
				}
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GatePassInward gatePass = new GatePassInward();
				gatePass.Id = RecId;
				gatePass.BiltyNo = txtbiltyno.Text.Trim();
				gatePass.CityId = Conversion.ToInt(cmbcity.Value);
				gatePass.DocumentTypeId = 51;
				gatePass.Freight = Conversion.ToDouble((object)txtfreight.Text.Trim());
				gatePass.AdvanceByParty = Conversion.ToDouble((object)txtAdvanceByParty.Text.Trim());
				gatePass.AdvanceByFactory = Conversion.ToDouble((object)txtAdvanceByFactory.Text.Trim());
				if (Conversion.ToInt(CmbOrderType.Value) == 105 && (gatePass.AdvanceByFactory > 0.0 || gatePass.AdvanceByParty > 0.0))
				{
					txtAdvanceByParty.Text = "0";
					txtAdvanceByFactory.Text = "0";
					throw new Exception("In Case Of Market Purchase Advance by Factory or Advance by Party can't be greater than 0");
				}
				gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
				gatePass.Container = Conversion.ToString((object)((Control)(object)cmbContainerNo1).Text);
				gatePass.Container1 = Conversion.ToString((object)((Control)(object)cmbContainerNo2).Text);
				gatePass.GpDate = txtgpdate.Value;
				gatePass.BiltyDate = txtBiltyDate.Value;
				gatePass.GpSrNo = Conversion.ToInt((object)txtgpno.Text.Trim());
				gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
				gatePass.NoOfPackages = Conversion.ToInt((object)txtqty.Text.Trim());
				gatePass.PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
				gatePass.WeightComparedToPoWt = Conversion.ToDouble((object)txtWeightCompared.Text.Trim());
				gatePass.OtherRemarks = txtremarks.Text.Trim();
				gatePass.OtherSupCust = ((Control)(object)CmbOrderType).Text;
				gatePass.OutDateTimeStamp = DateTime.Now;
				gatePass.SupplierContractCode = CmbOrderno.Text.Trim();
				gatePass.VehicleNo = txtvehicleno.Text.Trim();
				gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
				gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupp.Value);
				gatePass.SupplierDispatchId = Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value);
				gatePass.Status = ((Control)(object)CmbStatus).Text.ToString();
				gatePass.GpQRCode = Conversion.ToByteArray((object)GpQrCode);
				gatePass.NetPaid = Conversion.ToDouble((object)txtNetPaid.Text.Trim());
				gatePass.SupplierFirstWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
				gatePass.SupplierSecondWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
				gatePass.SupplierWeight = Conversion.ToDouble((object)txtSupplierWeight.Text.Trim());
				gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
				gatePass.DifferenceWeight = Conversion.ToDouble((object)txtDifferenceWeight.Text.Trim());
				gatePass.DocAttachment = Conversion.ToString((object)((Control)(object)cmbWeighBridge).Text);
				gatePass.WeighBridgeId = Conversion.ToInt((object)txtWeighBridgeSlipNo.Text.Trim());
				gatePass.ItemId = Conversion.ToInt(CmbVariety.Value);
				gatePass.VarietyName = ((Control)(object)CmbVariety).Text.Trim();
				gatePass.RefDocumentTypeId = Conversion.ToInt(CmbOrderType.Value);
				gatePass.PackingTypeId = Conversion.ToInt(CmbPackingType.Value);
				gatePass.FreightOn = 1;
				if (gatePass.RefDocumentTypeId == 105 || gatePass.RefDocumentTypeId == 106 || gatePass.RefDocumentTypeId == 204 || gatePass.RefDocumentTypeId == 241)
				{
					POId = gatePass.GpSrNo;
				}
				gatePass.PurchaseOrderId = POId;
				gatePass.OrganizationId = UserAccount.OrganizationId;
				gatePass.CompanyId = UserAccount.CompanyId;
				gatePass.BranchesId = UserAccount.BranchesId;
				gatePass.ModifyUser = UserAccount.ID;
				gatePass.EntryUser = UserAccount.ID;
				gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				gatePass.WeightDiffComments = Conversion.ToString((object)txtWeightDiffRemarks.Text.Trim());
				if (gatePass.Status == "Rejected" && MessageBox.Show("Are you sure to Reject GatePass?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				gatePass.GatePassInwardPurchaseBreakUpList = new List<GatePassInwardPurchaseBreakUp>();
				if (grdPurchaseBrakup.RowCount > 0)
				{
					GridEXRow[] rows = grdPurchaseBrakup.GetRows();
					foreach (GridEXRow r in rows)
					{
						GatePassInwardPurchaseBreakUp rr = new GatePassInwardPurchaseBreakUp();
						rr.Id = Conversion.ToInt(r.Cells["Id"].Value);
						rr.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						rr.UOM = Conversion.ToDecimal(r.Cells["UOM"].Value);
						rr.GrossWeight = Conversion.ToDecimal(r.Cells["GrossWeight"].Value);
						rr.EbWeight = Conversion.ToDecimal(r.Cells["EbWeight"].Value);
						rr.EBTotal = Conversion.ToDecimal(r.Cells["EBTotal"].Value);
						rr.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						gatePass.GatePassInwardPurchaseBreakUpList.Add(rr);
					}
				}
				if (gatePass.RefDocumentTypeId == 41 && ActionIdForSpecialApproval != 2)
				{
					double EmptyBagsTotalWeight = Conversion.ToDouble((object)txtqty.Text.Trim()) * 2.0;
					double FactoryWeightWithoutEmptyBagsWeight = Conversion.ToDouble((object)gatePass.FactoryWeight) - EmptyBagsTotalWeight;
					DataTable dtAccessWeight = GatePassInward.GetAccessWeightByWeightComparedToPoWtAndGpId(gatePass, FactoryWeightWithoutEmptyBagsWeight);
					if (dtAccessWeight.Rows.Count > 0 && Math.Abs(Conversion.ToDouble(dtAccessWeight.Rows[0]["AccessWeight"])) > 0.0)
					{
						double AccessWeight = Conversion.ToDouble(dtAccessWeight.Rows[0]["AccessWeight"]);
						double BalQtyOfPo = Conversion.ToDouble(dtAccessWeight.Rows[0]["BalWeightOfPo"]);
						if (MessageBox.Show($"The GatePass weight exceeds the PO weight. The remaining PO weight is {BalQtyOfPo} and the access weight is {AccessWeight}.\nAre you sure you want to Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
						{
							return;
						}
						gatePass.AccessWeight = AccessWeight;
						gatePass.IsApproved = false;
						gatePass.PostState = false;
						gatePass.ActionIdForSpecialApproval = 1;
					}
					else if (ActionIdForSpecialApproval == 1)
					{
						gatePass.IsApproved = ChkIsApproved.Checked && formright.DoHaveApproveRights;
						gatePass.PostDate = DateTime.Now;
						gatePass.PostState = formright.DoHaveApproveRights;
						gatePass.PostUser = UserAccount.ID;
						gatePass.ActionIdForSpecialApproval = 0;
						gatePass.AccessWeight = 0.0;
					}
				}
				gatePass.driverBioDataId = DriverBioId;
				gatePass.DriverCNICNO = txtCNIC.Text;
				gatePass.DriverMobileNo = txtDriverCellNo.Text;
				gatePass.AlternateCellNo = txtAlternateCellNo.Text;
				gatePass.whatsappNo = txtWhatsAppNo.Text;
				gatePass.DriverName = txtDriverName.Text;
				gatePass.FatherName = txtFatherName.Text;
				gatePass.fatherCnicNo = txtFatherCNIC.Text;
				gatePass.AttachmentsList = new List<DMSAttachments>();
				gatePass.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<GatePassInward>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = GatePassInward.Save(gatePass);
				if (success > 0)
				{
					MessageBox.Show("Record Update Successfully [" + gatePass.GpSrNo + "]");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 51, base.Name);
				}
				string status = Conversion.ToString((object)((Control)(object)CmbStatus).Text);
				Reset();
				btnupdate.Visible = false;
				btnsave.Visible = true;
				UpdateMode = false;
				OpenLinkformOnInsert(success, status);
				if (ChkBox.Checked)
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(success);
				}
				if (RadGrnFormToOpenOnInsert.Checked)
				{
					RadNoneFormToOpenOnInsert.Checked = true;
				}
				clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			}
		}
		catch (Exception ex)
		{
			lblDiffRemarks.Visible = true;
			txtWeightDiffRemarks.Visible = true;
			txtWeightDiffRemarks.Focus();
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void RadPurchaseLabFormToOpenOnInsert_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("InvLabPurchaseAnalysis", delegate
		{
			InvLabPurchaseAnalysis invLabPurchaseAnalysis = new InvLabPurchaseAnalysis(UserAccount);
			invLabPurchaseAnalysis.Show();
			invLabPurchaseAnalysis.BringToFront();
		}, UserAccount);
	}

	private void RadWBFormToOpenOnInsert_Click(object sender, EventArgs e)
	{
		try
		{
			FormHelper.CanOpenForm("frmWeightbridge", delegate
			{
				frmWeightbridge frmWeightbridge = new frmWeightbridge(UserAccount);
				frmWeightbridge.Show();
				frmWeightbridge.BringToFront();
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadGrnFormToOpenOnInsert_Click(object sender, EventArgs e)
	{
		if (Conversion.ToString((object)((Control)(object)CmbOrderType).Text) == "Purchase_Order_PM")
		{
			FormHelper.CanOpenForm("GrnPackingMaterial", delegate
			{
				GrnPackingMaterial grnPackingMaterial = new GrnPackingMaterial(UserAccount);
				grnPackingMaterial.Show();
				grnPackingMaterial.BringToFront();
			}, UserAccount);
		}
		else
		{
			FormHelper.CanOpenForm("InvFrmGRN", delegate
			{
				InvFrmGRN invFrmGRN = new InvFrmGRN(UserAccount);
				invFrmGRN.Show();
				invFrmGRN.BringToFront();
			}, UserAccount);
		}
	}

	private void FreightVoucherFormToOpen_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("FreightVoucher", delegate
		{
			FreightVoucher freightVoucher = new FreightVoucher(UserAccount);
			freightVoucher.Show();
			freightVoucher.BringToFront();
		}, UserAccount);
	}

	private void PurchaseLabForm(int GPId)
	{
		try
		{
			FormHelper.CanOpenForm("InvLabPurchaseAnalysis", delegate
			{
				InvLabPurchaseAnalysis invLabPurchaseAnalysis = new InvLabPurchaseAnalysis(UserAccount);
				invLabPurchaseAnalysis.Show();
				if (GPId > 0)
				{
					invLabPurchaseAnalysis.cmbGatePassNo.Value = GPId;
				}
				invLabPurchaseAnalysis.BringToFront();
				bool formClosed = false;
				invLabPurchaseAnalysis.FormClosed += delegate
				{
					formClosed = true;
				};
				invLabPurchaseAnalysis.Show();
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WBForm(int GPId)
	{
		try
		{
			FormHelper.CanOpenForm("frmWeightbridge", delegate
			{
				frmWeightbridge frmWeightbridge = new frmWeightbridge(UserAccount);
				frmWeightbridge.Show();
				frmWeightbridge.cmbReferenceType.Value = 51;
				frmWeightbridge.cmbReferenceType_Leave(null, null);
				if (GPId > 0)
				{
					frmWeightbridge.cmbRefDocNo.Focus();
					frmWeightbridge.cmbRefDocNo.Value = GPId;
				}
				frmWeightbridge.BringToFront();
				bool formClosed = false;
				frmWeightbridge.FormClosed += delegate
				{
					formClosed = true;
				};
				frmWeightbridge.Show();
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrnForm(int GPId)
	{
		try
		{
			if (Conversion.ToString((object)((Control)(object)CmbOrderType).Text) == "Purchase_Order_PM")
			{
				FormHelper.CanOpenForm("GrnPackingMaterial", delegate
				{
					GrnPackingMaterial grnPackingMaterial = new GrnPackingMaterial(UserAccount);
					grnPackingMaterial.Show();
					grnPackingMaterial.LoadGpData(GPId);
					grnPackingMaterial.BringToFront();
					bool formClosed2 = false;
					grnPackingMaterial.FormClosed += delegate
					{
						formClosed2 = true;
					};
					grnPackingMaterial.Show();
					while (!formClosed2)
					{
						Application.DoEvents();
					}
				}, UserAccount);
				return;
			}
			FormHelper.CanOpenForm("InvFrmGRN", delegate
			{
				InvFrmGRN frm = new InvFrmGRN(UserAccount);
				frm.Show();
				frm.FormInitializedAction += delegate
				{
					frm.LoadGPByRow(GPId);
				};
				frm.BringToFront();
				bool formClosed = false;
				frm.FormClosed += delegate
				{
					formClosed = true;
				};
				frm.Show();
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightVoucherForm(int GPId)
	{
		try
		{
			FormHelper.CanOpenForm("FreightVoucher", delegate
			{
				FreightVoucher frm = new FreightVoucher(UserAccount);
				frm.Show();
				frm.FormInitializedAction += delegate
				{
					GridEXRow val = frm.grdPendingRecord.GetDataRows()?.FirstOrDefault((GridEXRow row) => GPId == Conversion.ToInt(row.Cells["Id"].Value));
					if (val != null)
					{
						frm.LoadRecord(val);
					}
				};
				frm.BringToFront();
				bool formClosed = false;
				frm.FormClosed += delegate
				{
					formClosed = true;
				};
				frm.Show();
				while (!formClosed)
				{
					Application.DoEvents();
				}
			}, UserAccount);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenLinkformOnInsert(int GatePassId, string Status)
	{
		try
		{
			if (RadPurchaseLabFormToOpenOnInsert.Checked)
			{
				PurchaseLabForm(GatePassId);
			}
			else if (RadWBFormToOpenOnInsert.Checked)
			{
				WBForm(GatePassId);
			}
			else if (RadFreightVoucherFormToOpen.Checked)
			{
				FreightVoucherForm(GatePassId);
			}
			else if (RadGrnFormToOpenOnInsert.Checked && Status == "Accepted")
			{
				GrnForm(GatePassId);
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
			CalculateDifferenceWeights();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void txtSupplierWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtSupplierWeight.Tag != null && txtSupplierWeight.Tag != string.Empty)
			{
				txtSupplierSecondWeight.Text = "0";
				txtSupplierFirstWeight.Text = "0";
				txtSupplierWeight.Tag = "";
			}
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFactoryWeight_TextChanged(object sender, EventArgs e)
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

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		WeightComparedPoWtCalculation();
	}

	private void WeightComparedPoWtCalculation()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			double PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
			if (Qty > 0.0 && PackUnit > 0.0)
			{
				txtWeightCompared.Text = (Qty * PackUnit).ToString();
			}
			else
			{
				txtWeightCompared.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtPackUnit_TextChanged(object sender, EventArgs e)
	{
		WeightComparedPoWtCalculation();
	}

	private void GetWeighBridgeWeightAndTicketNos(int recordId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 51,
				ReferenceDocNoId = recordId
			});
			if (lstWb.Rows.Count > 0)
			{
				string TicketNo = "";
				double FirstWeight = 0.0;
				double SecondWeight = 0.0;
				double NetWeight = 0.0;
				for (int i = 0; i < lstWb.Rows.Count; i++)
				{
					TicketNo = TicketNo + "," + lstWb.Rows[i]["TicketNo"].ToString();
					NetWeight += Convert.ToDouble(lstWb.Rows[i]["NetWbWeight"]);
					FirstWeight += Convert.ToDouble(lstWb.Rows[i]["FirstWeight"]);
					SecondWeight += Convert.ToDouble(lstWb.Rows[i]["SecondWeight"]);
				}
				txtWeighBridgeSlipNo.Text = TicketNo;
				txtFirstWeight.Text = FirstWeight.ToString();
				txtsecondWeight.Text = SecondWeight.ToString();
				txtFactoryWeight.Text = NetWeight.ToString();
				if (txtFactoryWeight.Text != string.Empty && txtFactoryWeight.Text != "0")
				{
					((Control)(object)cmbWeighBridge).Enabled = false;
				}
			}
			else
			{
				txtWeighBridgeSlipNo.Text = "0";
				txtFactoryWeight.Text = "0";
				txtFirstWeight.Text = 0.ToString();
				txtsecondWeight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtfreight_Leave(object sender, EventArgs e)
	{
		try
		{
			FreightCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		if (!_internalChange)
		{
			TextBox obj = (TextBox)sender;
			int pos = obj.SelectionStart;
			_internalChange = true;
			obj.Text = obj.Text.ToUpper();
			_internalChange = false;
			obj.SelectionStart = pos;
		}
	}

	private void FreightCalculations()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtfreight.Text.Trim());
			double advancebyparty = Conversion.ToDouble((object)txtAdvanceByParty.Text.Trim());
			double advancebyfactory = Conversion.ToDouble((object)txtAdvanceByFactory.Text.Trim());
			double totalpayablesfreight = num - advancebyparty - advancebyfactory;
			txtTotalPayablesFreight.Text = totalpayablesfreight.ToString();
			if (Conversion.ToDouble(txtNetPaid.Tag) == 0.0)
			{
				txtNetPaid.Text = totalpayablesfreight.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdStockReceivingNotes).Name);
			if (fs != null)
			{
				grdStockReceivingNotes.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPurchaseOrder).Name);
			if (fs != null)
			{
				grdPurchaseOrder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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

	private void GenerateReport()
	{
		try
		{
			CommonServices.GatePassInwardSlipAndRegisterReport(RecId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
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

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.GpId = RecId;
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
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void btnSlip257_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InwardGatePassWithWbAndLabSlip(RecId);
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

	private void gridhistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 51;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.FromDate = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.EntryDateFrom = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.ModifyDateFrom = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dt = GatePassInward.GatepassHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GatepassType", typeof(string));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("OrderNo", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("CityName", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyDate", typeof(DateTime));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("VarietyName", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(double));
				dtcol.Columns.Add("PackUnit", typeof(double));
				dtcol.Columns.Add("WeightComparedToPoWt", typeof(double));
				dtcol.Columns.Add("SupplierFirstWeight", typeof(double));
				dtcol.Columns.Add("SupplierSecondWeight", typeof(double));
				dtcol.Columns.Add("SupplierWeight", typeof(double));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("DifferenceWeight", typeof(double));
				dtcol.Columns.Add("Freight", typeof(double));
				dtcol.Columns.Add("AdvanceByParty", typeof(double));
				dtcol.Columns.Add("AdvanceByFactory", typeof(double));
				dtcol.Columns.Add("TotalPayableFreight", typeof(double));
				dtcol.Columns.Add("NetPaid", typeof(double));
				dtcol.Columns.Add("InTime", typeof(DateTime));
				dtcol.Columns.Add("OutTime", typeof(DateTime));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("PurchaseOrderId", typeof(int));
				dtcol.Columns.Add("ApprovalStatus", typeof(string));
				dtcol.Columns.Add("AccessWeight", typeof(double));
				dtcol.Columns.Add("PackingType", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["RecordNo"], dt.Rows[i]["Id"], dt.Rows[i]["GpSrNo"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GatepassType"], dt.Rows[i]["OrderType"], dt.Rows[i]["OrderNo"], dt.Rows[i]["CompanyName"], dt.Rows[i]["Description"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyDate"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["VarietyName"], dt.Rows[i]["ItemQty"], dt.Rows[i]["PackUnit"], dt.Rows[i]["WeightComparedToPoWt"], dt.Rows[i]["SupplierFirstWeight"], dt.Rows[i]["SupplierSecondWeight"], dt.Rows[i]["SupplierWeight"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["DifferenceWeight"], dt.Rows[i]["Freight"], dt.Rows[i]["AdvanceByParty"], dt.Rows[i]["AdvanceByFactory"], dt.Rows[i]["TotalPayableFreight"], dt.Rows[i]["NetPaid"], dt.Rows[i]["InTime"], dt.Rows[i]["OutTime"], dt.Rows[i]["Status"], dt.Rows[i]["UserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["PurchaseOrderId"], dt.Rows[i]["ApprovalStatus"], dt.Rows[i]["AccessWeight"], dt.Rows[i]["PackingType"]);
				}
				grdhistory.DataSource = dtcol;
				grdhistory.RetrieveStructure();
				grdhistorysetting();
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdhistorysetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["PurchaseOrderId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["GpSrNo"].Caption = "Gp No";
			grdhistory.RootTable.Columns["GatepassType"].Caption = "Gp Type";
			grdhistory.RootTable.Columns["DifferenceWeight"].Caption = "Diff Weight";
			grdhistory.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grdhistory.RootTable.Columns["AccessWeight"].Caption = "Excess Weight";
			grdhistory.RootTable.Columns["SupplierFirstWeight"].Caption = "Supplier Load Weight";
			grdhistory.RootTable.Columns["SupplierSecondWeight"].Caption = "Supplier Tare Weight";
			grdhistory.RootTable.Columns["SupplierWeight"].Caption = "Supplier Net Weight";
			grdhistory.RootTable.Columns["InTime"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["OutTime"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["PackUnit"].Width = 60;
			grdhistory.RootTable.Columns["GpSrNo"].Width = 60;
			grdhistory.RootTable.Columns["GpDate"].Width = 80;
			grdhistory.RootTable.Columns["GatepassType"].Width = 75;
			grdhistory.RootTable.Columns["OrderType"].Width = 100;
			grdhistory.RootTable.Columns["OrderNo"].Width = 70;
			grdhistory.RootTable.Columns["SupplierName"].Width = 130;
			grdhistory.RootTable.Columns["CityName"].Width = 100;
			grdhistory.RootTable.Columns["VehicleType"].Width = 100;
			grdhistory.RootTable.Columns["VehicleNo"].Width = 80;
			grdhistory.RootTable.Columns["BiltyDate"].Width = 85;
			grdhistory.RootTable.Columns["BiltyNo"].Width = 70;
			grdhistory.RootTable.Columns["VarietyName"].Width = 130;
			grdhistory.RootTable.Columns["ItemQty"].Width = 75;
			grdhistory.RootTable.Columns["SupplierWeight"].Width = 100;
			grdhistory.RootTable.Columns["FactoryWeight"].Width = 100;
			grdhistory.RootTable.Columns["DifferenceWeight"].Width = 90;
			grdhistory.RootTable.Columns["Freight"].Width = 85;
			grdhistory.RootTable.Columns["AdvanceByParty"].Width = 85;
			grdhistory.RootTable.Columns["AdvanceByFactory"].Width = 85;
			grdhistory.RootTable.Columns["TotalPayableFreight"].Width = 85;
			grdhistory.RootTable.Columns["NetPaid"].Width = 85;
			grdhistory.RootTable.Columns["InTime"].Width = 140;
			grdhistory.RootTable.Columns["OutTime"].Width = 140;
			grdhistory.RootTable.Columns["Status"].Width = 75;
			grdhistory.RootTable.Columns["EntryUser"].Width = 120;
			grdhistory.RootTable.Columns["EntryDate"].Width = 150;
			grdhistory.RootTable.Columns["ModifyUser"].Width = 120;
			grdhistory.RootTable.Columns["ModifyDate"].Width = 150;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = 100;
			grdhistory.RootTable.Columns["Remarks"].Width = 150;
			grdhistory.RootTable.Columns["PackingType"].Width = 120;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 0;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
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
				txtFromDateHistory.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (DMSAttachments itemList in dMSList)
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

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistory();
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
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				LabDataGetByGpId(RecId);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
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
			txtFromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			txtToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			txtFromDateHistory.Focus();
			grdhistory.ClearStructure();
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
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells[1].Value);
			LabDataGetByGpId(RecId);
			ReadById(RecId);
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
			gridhistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		try
		{
			if (grdhistory.RowCount <= 0)
			{
				return;
			}
			GridEXColumn AccessWeight = grdhistory.RootTable.Columns["AccessWeight"];
			GridEXColumn ApprovalStatus = grdhistory.RootTable.Columns["ApprovalStatus"];
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

	private void txtSupplierWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtSupplierWeight.Tag = txtSupplierWeight.Text;
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtPackUnit_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtfreight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtNetPaid_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			txtNetPaid.Tag = Conversion.ToDouble((object)txtNetPaid.Text).ToString();
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

	private void CmbOrderno_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void FromDocStockInfo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void ToDocStockInfo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtFromDocNoHistoryPoInfo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtToDocNoHistoryPoInfo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void txtExpiryDaysPoInfo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void toolStripButton3_Click(object sender, EventArgs e)
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

	private void btndefvehicle_Click(object sender, EventArgs e)
	{
		try
		{
			new VehicleType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InwardGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
					txtgpdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					txtFromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtgpdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Left)
				{
					if (tabControl2.SelectedIndex == 2)
					{
						FromDateHistoryPoInfo.Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						FromDateStockInfo.Focus();
						tabControl2.SelectedIndex = 0;
					}
					else if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
						tabControl2.SelectedIndex = 2;
					}
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						FromDateHistoryPoInfo.Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						FromDateStockInfo.Focus();
						tabControl2.SelectedIndex = 2;
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grd).Focus();
						tabControl2.SelectedIndex = 0;
					}
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Alt && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
					txtgpdate.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 80;
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtFromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
				}
				if (e.Alt && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdhistory_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseBrakup_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdPurchaseBrakup.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPurchaseBrakup.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPurchaseBrakup.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					GridEXRow item = grdPurchaseBrakup.CurrentRow;
					dtPurchaseBreakup.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPurchaseBreakup.Rows.Count == 0)
					{
						AddRowInPurchaseBreakUpGrid();
					}
				}
				if (grdPurchaseBrakup.CurrentColumn.Key == "Add")
				{
					AddRowInPurchaseBreakUpGrid();
				}
				grdPurchaseBrakup.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					GridEXRow item2 = grdPurchaseBrakup.CurrentRow;
					dtPurchaseBreakup.Rows[item2.RowIndex].Delete();
					if (item2 != null && (int)item2.RowType == 0)
					{
						item2.Delete();
					}
					if (dtPurchaseBreakup.Rows.Count == 0)
					{
						AddRowInPurchaseBreakUpGrid();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPurchaseBreakUpGrid();
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
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Edit")
				{
					((UltraGridBase)CmbStatus).Rows[1].Activate();
					RecId = Conversion.ToInt(r.Cells["Id"].Value);
					LabDataGetByGpId(RecId);
					ReadById(RecId);
				}
				if (grd.CurrentColumn.Key == "Print")
				{
					CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
				}
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
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null || !e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					List<DMSAttachments> dMSList = DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
					dtViewAttachments.Clear();
					if (dMSList.Count > 0)
					{
						foreach (DMSAttachments itemList in dMSList)
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
			}
			if (grdhistory.CurrentColumn.Key == "Edit")
			{
				Reset();
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				RecId = Conversion.ToInt(r.Cells["Id"].Value);
				LabDataGetByGpId(RecId);
				ReadById(RecId);
			}
			if (grdhistory.CurrentColumn.Key == "Print")
			{
				CommonServices.GatePassInwardSlipAndRegisterReport(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockReceivingNotes_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdStockReceivingNotes.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdStockReceivingNotes.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space && grdStockReceivingNotes.CurrentColumn.Key == "Load")
			{
				CmbOrderType.Value = 175;
				CmbOrderno.Text = Conversion.ToString(r.Cells["GpSrNo"].Value);
				CmbOrderno_Leave(null, null);
				cmbsupp.Focus();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Alt+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on gp Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+ArrowLeft", "For Change Focus from one Grid To another Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
	{
		MakeShortCutKeys();
	}

	private void txtAccessWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ChkIsApproved.Enabled = !(Conversion.ToDouble((object)txtAccessWeight.Text) > 0.0) || !AcceptAccessWtVehiclesandHoldForSpecialApprovalOn1stWt;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDriverForm_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Select((tblUserRights r) => r.ScreenName == "frmDriverBioForInWard")))
			{
				frmDriverBio frmDriverBio = new frmDriverBio(UserAccount);
				frmDriverBio.RefDocumentTypeId = 51;
				frmDriverBio.Show();
			}
			else
			{
				MessageBox.Show("You Don't Have View-right Of Of This Driver-Bio For Inward..");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAdvanceByParty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtAdvanceByFactory_TextChanged(object sender, EventArgs e)
	{
		try
		{
			FreightCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbSupplierDispatchedPreBillNo_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSupplierDispatchedPreBillNo.Value);
			if (((UltraGridBase)CmbSupplierDispatchedPreBillNo).ActiveRow != null && Id > 0)
			{
				CellsCollection cells = ((UltraDropDownBase)CmbSupplierDispatchedPreBillNo).SelectedRow.Cells;
				string vehicleNo = Conversion.ToString(cells["VehicleNo"].Value);
				if (!string.IsNullOrWhiteSpace(vehicleNo))
				{
					txtvehicleno.Text = vehicleNo;
				}
				string biltyNo = Conversion.ToString(cells["BiltyNo"].Value);
				if (!string.IsNullOrWhiteSpace(biltyNo))
				{
					txtbiltyno.Text = biltyNo;
				}
				int cityId = Conversion.ToInt(cells["CityId"].Value);
				if (cityId > 0)
				{
					cmbcity.Value = cityId;
				}
				double freight = Conversion.ToDouble(cells["Freight"].Value);
				if (freight > 0.0)
				{
					txtfreight.Text = freight.ToString("#,##");
				}
				double advanceFreight = Conversion.ToDouble(cells["AdvanceFreight"].Value);
				if (freight > 0.0 && advanceFreight > 0.0)
				{
					txtAdvanceByParty.Text = advanceFreight.ToString("#,##");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupp_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(cmbsupp.Value);
			if (Id > 0 || POId > 0)
			{
				PreBillNoFill(Id, POId);
				return;
			}
			((Control)(object)CmbSupplierDispatchedPreBillNo).Text = string.Empty;
			((UltraGridBase)CmbSupplierDispatchedPreBillNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierFirstWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CalculateSupplierNetWeight()
	{
		try
		{
			double firstWeight = Conversion.ToDouble((object)txtSupplierFirstWeight.Text);
			double secondWeight = Conversion.ToDouble((object)txtSupplierSecondWeight.Text);
			if (firstWeight > 0.0 || secondWeight > 0.0)
			{
				double Net = Math.Abs(firstWeight - secondWeight);
				txtSupplierWeight.Text = Net.ToString("#,##0.###");
			}
			CalculateDifferenceWeights();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierSecondWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupplierFirstWeight_TextChanged(object sender, EventArgs e)
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

	private void txtSupplierSecondWeight_TextChanged(object sender, EventArgs e)
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

	private void txtFirstWeight_TextChanged(object sender, EventArgs e)
	{
		CalculateDifferenceWeights();
	}

	private void txtsecondWeight_TextChanged(object sender, EventArgs e)
	{
		CalculateDifferenceWeights();
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
			double num3 = Conversion.ToDouble((object)txtFactoryWeight.Text);
			double SupplierNetWeight = Conversion.ToDouble((object)txtSupplierWeight.Text);
			double NetWtDiff = num3 - SupplierNetWeight;
			txtDifferenceWeight.Text = Math.Abs(NetWtDiff).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvehicleno_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (_internalChange || char.IsControl(e.KeyChar))
		{
			return;
		}
		if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '-')
		{
			e.Handled = true;
			return;
		}
		TextBox tb = (TextBox)sender;
		string text = tb.Text;
		int selectionStart = tb.SelectionStart;
		if (e.KeyChar == '-' && text.Contains("-"))
		{
			e.Handled = true;
			return;
		}
		string futureText = text.Substring(0, selectionStart) + e.KeyChar + text.Substring(selectionStart);
		int dashIndex = futureText.IndexOf('-');
		string lettersPart = ((dashIndex >= 0) ? futureText.Substring(0, dashIndex) : futureText);
		string digitsPart = ((dashIndex >= 0 && dashIndex < futureText.Length - 1) ? futureText.Substring(dashIndex + 1) : "");
		if (char.IsLetter(e.KeyChar) && lettersPart.Length > 6)
		{
			e.Handled = true;
		}
		else if (char.IsDigit(e.KeyChar) && dashIndex >= 0 && digitsPart.Length > 6)
		{
			e.Handled = true;
		}
		else if (char.IsDigit(e.KeyChar) && !text.Contains("-"))
		{
			int letterCount = text.TakeWhile(char.IsLetter).Count();
			if (letterCount >= 1 && letterCount <= 6)
			{
				_internalChange = true;
				tb.Text = text.Insert(letterCount, "-");
				tb.SelectionStart = selectionStart + 1;
				_internalChange = false;
			}
		}
	}

	private void txtvehicleno_Leave(object sender, EventArgs e)
	{
		if (_internalChange || string.IsNullOrWhiteSpace(txtvehicleno.Text))
		{
			return;
		}
		string text = txtvehicleno.Text.Trim().ToUpper();
		if (!text.Contains("-"))
		{
			int letterCount = text.TakeWhile(char.IsLetter).Count();
			if (letterCount >= 1 && letterCount <= 6)
			{
				text = text.Insert(letterCount, "-");
			}
		}
		_internalChange = true;
		txtvehicleno.Text = text;
		_internalChange = false;
	}

	private void txtvehicleno_KeyDown(object sender, KeyEventArgs e)
	{
		if (!_internalChange && e.KeyCode == Keys.Back)
		{
			TextBox tb = (TextBox)sender;
			int pos = tb.SelectionStart;
			if (pos > 0 && pos <= tb.Text.Length && tb.Text[pos - 1] == '-')
			{
				e.Handled = true;
				e.SuppressKeyPress = true;
				_internalChange = true;
				string newText = tb.Text.Remove(pos - 1, 1);
				tb.Text = newText;
				tb.SelectionStart = pos - 1;
				tb.SelectionLength = 0;
				_internalChange = false;
			}
		}
	}

	private void txtvehicleno_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.Control && e.KeyCode == Keys.V)
		{
			string text = txtvehicleno.Text.Trim().ToUpper();
			text = Regex.Replace(text, "[^A-Z0-9]", "");
			int letterCount = 0;
			string text2 = text;
			for (int i = 0; i < text2.Length && char.IsLetter(text2[i]); i++)
			{
				letterCount++;
			}
			if (letterCount >= 1 && letterCount <= 6)
			{
				text = text.Insert(letterCount, "-");
			}
			txtvehicleno.Text = text;
			txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		}
	}

	private void DriverFieldsDisableOrEnable(bool flag)
	{
		try
		{
			txtCNIC.Enabled = flag;
			txtDriverCellNo.Enabled = flag;
			txtWhatsAppNo.Enabled = flag;
			txtAlternateCellNo.Enabled = flag;
			txtDriverName.Enabled = flag;
			txtFatherName.Enabled = flag;
			txtFatherCNIC.Enabled = flag;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetDriverFields()
	{
		try
		{
			DriverBioId = 0;
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtWhatsAppNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtFatherCNIC.Text = string.Empty;
			txtDriverCellNo.Text = "092-3-";
			txtWhatsAppNo.Text = "092-3-";
			txtAlternateCellNo.Text = "092-3-";
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtCNIC_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.CnicNo == txtCNIC.Text.Trim());
			if (obj != null)
			{
				DriverBioId = obj.Id;
				txtDriverCellNo.Text = obj.DriverCellNo;
				txtWhatsAppNo.Text = obj.WhatsappNo;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtDriverName.Text = obj.DriverName;
				txtFatherName.Text = obj.FatherName;
				txtFatherCNIC.Text = obj.FatherCnicNo;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDriverCellNo_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.DriverCellNo == txtDriverCellNo.Text.Trim());
			if (obj != null)
			{
				DriverBioId = obj.Id;
				txtCNIC.Text = obj.CnicNo;
				txtWhatsAppNo.Text = obj.WhatsappNo;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtDriverName.Text = obj.DriverName;
				txtFatherName.Text = obj.FatherName;
				txtFatherCNIC.Text = obj.FatherCnicNo;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetDriverInfo_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			ResetDriverFields();
			DriverFieldsDisableOrEnable(flag: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCNIC_Click(object sender, EventArgs e)
	{
		txtCNIC.Focus();
	}

	private void txtDriverCellNo_Click(object sender, EventArgs e)
	{
		txtDriverCellNo.Focus();
	}

	private void txtWhatsAppNo_Click(object sender, EventArgs e)
	{
		txtWhatsAppNo.Focus();
	}

	private void txtAlternateCellNo_Click(object sender, EventArgs e)
	{
		txtAlternateCellNo.Focus();
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
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Expected O, but got Unknown
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ce: Expected O, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Expected O, but got Unknown
		//IL_09ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f7: Expected O, but got Unknown
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a44: Expected O, but got Unknown
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4f: Expected O, but got Unknown
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a5a: Expected O, but got Unknown
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Expected O, but got Unknown
		//IL_0b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Expected O, but got Unknown
		//IL_0bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bba: Expected O, but got Unknown
		//IL_0bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be6: Expected O, but got Unknown
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Expected O, but got Unknown
		//IL_0c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c12: Expected O, but got Unknown
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Expected O, but got Unknown
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Expected O, but got Unknown
		//IL_18aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b4: Expected O, but got Unknown
		//IL_18c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cb: Expected O, but got Unknown
		//IL_18d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e2: Expected O, but got Unknown
		//IL_3615: Unknown result type (might be due to invalid IL or missing references)
		//IL_361f: Expected O, but got Unknown
		//IL_4591: Unknown result type (might be due to invalid IL or missing references)
		//IL_459b: Expected O, but got Unknown
		//IL_45a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_45b2: Expected O, but got Unknown
		//IL_d3a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_d3af: Expected O, but got Unknown
		//IL_d3bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_d3c6: Expected O, but got Unknown
		//IL_d3d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_d3dd: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPurchaseOrder_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		GridEXLayout grdStockReceivingNotes_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPurchaseBrakup_DesignTimeLayout = new GridEXLayout();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
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
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Purchase.InwardGatePass));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdPurchaseOrder = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.CmbDocumentTypePoInfo = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.RadAapprovedDatePoInfo = new System.Windows.Forms.RadioButton();
		this.txtExpiryDaysPoInfo = new System.Windows.Forms.TextBox();
		this.RadEntryDatePoInfo = new System.Windows.Forms.RadioButton();
		this.label34 = new System.Windows.Forms.Label();
		this.RadModifyDatePoInfo = new System.Windows.Forms.RadioButton();
		this.RadDocDatePoInfo = new System.Windows.Forms.RadioButton();
		this.cmbSupplierNamePoInfo = new UltraCombo();
		this.txtToDocNoHistoryPoInfo = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistoryPoInfo = new System.Windows.Forms.TextBox();
		this.ToDateHistoryPoInfo = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistoryPoInfo = new System.Windows.Forms.DateTimePicker();
		this.BtnResetPoInfo = new System.Windows.Forms.Button();
		this.BtnShowPoInfo = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdStockReceivingNotes = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.RadApproveDateStockInfo = new System.Windows.Forms.RadioButton();
		this.RadEntryDateStockInfo = new System.Windows.Forms.RadioButton();
		this.RadModifyDateStockInfo = new System.Windows.Forms.RadioButton();
		this.RadDocDateStockInfo = new System.Windows.Forms.RadioButton();
		this.ToDocStockInfo = new System.Windows.Forms.TextBox();
		this.FromDocStockInfo = new System.Windows.Forms.TextBox();
		this.ToDateStockInfo = new System.Windows.Forms.DateTimePicker();
		this.FromDateStockInfo = new System.Windows.Forms.DateTimePicker();
		this.BtnResetStockInfo = new System.Windows.Forms.Button();
		this.BtnShowStockInfo = new System.Windows.Forms.Button();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.PanelBreakup = new System.Windows.Forms.Panel();
		this.grdPurchaseBrakup = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label67 = new System.Windows.Forms.Label();
		this.txtDifferenceWeight = new System.Windows.Forms.TextBox();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.txtSecondWtDifference = new System.Windows.Forms.TextBox();
		this.txtSupplierSecondWeight = new System.Windows.Forms.TextBox();
		this.label80 = new System.Windows.Forms.Label();
		this.label82 = new System.Windows.Forms.Label();
		this.txtFirstWtDifference = new System.Windows.Forms.TextBox();
		this.label83 = new System.Windows.Forms.Label();
		this.txtSupplierFirstWeight = new System.Windows.Forms.TextBox();
		this.label84 = new System.Windows.Forms.Label();
		this.label85 = new System.Windows.Forms.Label();
		this.label86 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtsecondWeight = new System.Windows.Forms.TextBox();
		this.txtFirstWeight = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.CmbSupplierDispatchedPreBillNo = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtAccessWeight = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.txtTotalPayablesFreight = new System.Windows.Forms.TextBox();
		this.label69 = new System.Windows.Forms.Label();
		this.txtintime = new System.Windows.Forms.DateTimePicker();
		this.txtAdvanceByParty = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.txtAdvanceByFactory = new System.Windows.Forms.TextBox();
		this.txtouttime = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.lblDiffRemarks = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtWeightDiffRemarks = new System.Windows.Forms.TextBox();
		this.label68 = new System.Windows.Forms.Label();
		this.cmbContainerNo1 = new UltraCombo();
		this.ContainerNo1 = new System.Windows.Forms.Label();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.cmbContainerNo2 = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.containerno2 = new System.Windows.Forms.Label();
		this.txtgptypeno = new System.Windows.Forms.TextBox();
		this.label76 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.CmbOrderno = new System.Windows.Forms.TextBox();
		this.cmbsupp = new UltraCombo();
		this.cmbgptype = new UltraCombo();
		this.CmbOrderType = new UltraCombo();
		this.label64 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.cmbWeighBridge = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.txtWeighBridgeSlipNo = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.txtBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.txtNetPaid = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.txtfreight = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.CmbVariety = new UltraCombo();
		this.txtWeightCompared = new System.Windows.Forms.TextBox();
		this.CmbStatus = new UltraCombo();
		this.label63 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.cmbcity = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.CmbPackingType = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.txtLabReportNo = new System.Windows.Forms.TextBox();
		this.txtReportStatus = new System.Windows.Forms.TextBox();
		this.txtLabRemarks = new System.Windows.Forms.TextBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.txtBalQty = new System.Windows.Forms.TextBox();
		this.txtRecQty = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.FreightVoucherFormToOpen = new System.Windows.Forms.LinkLabel();
		this.RadFreightVoucherFormToOpen = new System.Windows.Forms.RadioButton();
		this.chkDriverInfoForm = new System.Windows.Forms.CheckBox();
		this.labelGrnForm = new System.Windows.Forms.LinkLabel();
		this.labelWeightBridgeForm = new System.Windows.Forms.LinkLabel();
		this.labelLabAnalysisForm = new System.Windows.Forms.LinkLabel();
		this.label15 = new System.Windows.Forms.Label();
		this.rdFreightCredit = new System.Windows.Forms.RadioButton();
		this.rdFreightCash = new System.Windows.Forms.RadioButton();
		this.RadNoneFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadGrnFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadWBFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadPurchaseLabFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.ChkIsApproved = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btndefvehicle = new System.Windows.Forms.ToolStripButton();
		this.btndefinecity = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnDriverForm = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.CmbCustomerHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.groupBox6 = new System.Windows.Forms.GroupBox();
		this.btnResetDriverInfo = new System.Windows.Forms.PictureBox();
		this.txtFatherCNIC = new System.Windows.Forms.MaskedTextBox();
		this.label65 = new System.Windows.Forms.Label();
		this.txtWhatsAppNo = new System.Windows.Forms.MaskedTextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.txtAlternateCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.txtDriverCellNo = new System.Windows.Forms.MaskedTextBox();
		this.label77 = new System.Windows.Forms.Label();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label78 = new System.Windows.Forms.Label();
		this.txtCNIC = new System.Windows.Forms.MaskedTextBox();
		this.label79 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOrder).BeginInit();
		this.panel14.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypePoInfo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNamePoInfo).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockReceivingNotes).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.PanelBreakup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrakup).BeginInit();
		this.panel22.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDispatchedPreBillNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel17.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.groupBox6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1619, 737);
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
		this.tabControl1.Size = new System.Drawing.Size(1619, 737);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1611, 708);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.tabControl2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 422);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1607, 284);
		this.panel9.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1607, 284);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel10);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1599, 255);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "GatePass History Open Status";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1593, 249);
		this.panel10.TabIndex = 1;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 23);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1593, 226);
		this.panel12.TabIndex = 1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1593, 226);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label5);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1593, 23);
		this.panel11.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(583, 16);
		this.label5.TabIndex = 1;
		this.label5.Text = "This Grid Show the  Vehicles Present In The Factory,Pending For Acceptance Or Rejection";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1555, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseOrder);
		this.tabPage4.Controls.Add(this.panel14);
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1599, 255);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Purchase Order Information";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdPurchaseOrder.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPurchaseOrder).BackColor = System.Drawing.Color.White;
		grdPurchaseOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseOrder.DesignTimeLayout = grdPurchaseOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPurchaseOrder.DynamicFiltering = true;
		this.grdPurchaseOrder.FilterMode = (FilterMode)2;
		this.grdPurchaseOrder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseOrder.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).Location = new System.Drawing.Point(3, 107);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).Name = "grdPurchaseOrder";
		((ScrollableBase)this.grdPurchaseOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).Size = new System.Drawing.Size(1593, 145);
		((System.Windows.Forms.Control)(object)this.grdPurchaseOrder).TabIndex = 1;
		this.grdPurchaseOrder.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseOrder).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Controls.Add(this.label6);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 83);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1593, 24);
		this.panel14.TabIndex = 3;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1552, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdPurchaseOrder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 59;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(871, 16);
		this.label6.TabIndex = 60;
		this.label6.Text = "This Grid Show The Un-Completed And Approved Purchase Orders.... Complete.Cancel And Un-Approved Orders Will Not Appear here. ";
		this.panel13.Controls.Add(this.groupBox4);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1593, 80);
		this.panel13.TabIndex = 0;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Controls.Add(this.RadAapprovedDatePoInfo);
		this.groupBox4.Controls.Add(this.txtExpiryDaysPoInfo);
		this.groupBox4.Controls.Add(this.RadEntryDatePoInfo);
		this.groupBox4.Controls.Add(this.label34);
		this.groupBox4.Controls.Add(this.RadModifyDatePoInfo);
		this.groupBox4.Controls.Add(this.RadDocDatePoInfo);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo);
		this.groupBox4.Controls.Add(this.txtToDocNoHistoryPoInfo);
		this.groupBox4.Controls.Add(this.txtFromDocNoHistoryPoInfo);
		this.groupBox4.Controls.Add(this.ToDateHistoryPoInfo);
		this.groupBox4.Controls.Add(this.FromDateHistoryPoInfo);
		this.groupBox4.Controls.Add(this.BtnResetPoInfo);
		this.groupBox4.Controls.Add(this.BtnShowPoInfo);
		this.groupBox4.Controls.Add(this.label23);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1025, 77);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.CmbDocumentTypePoInfo.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentTypePoInfo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentTypePoInfo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentTypePoInfo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentTypePoInfo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentTypePoInfo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo).Location = new System.Drawing.Point(424, 45);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo).Name = "CmbDocumentTypePoInfo";
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypePoInfo).TabIndex = 5;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(334, 51);
		this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(92, 15);
		this.label28.TabIndex = 111;
		this.label28.Text = "Document Type";
		this.RadAapprovedDatePoInfo.AutoSize = true;
		this.RadAapprovedDatePoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAapprovedDatePoInfo.Location = new System.Drawing.Point(915, 43);
		this.RadAapprovedDatePoInfo.Name = "RadAapprovedDatePoInfo";
		this.RadAapprovedDatePoInfo.Size = new System.Drawing.Size(105, 19);
		this.RadAapprovedDatePoInfo.TabIndex = 12;
		this.RadAapprovedDatePoInfo.Text = "Approved Date";
		this.RadAapprovedDatePoInfo.UseVisualStyleBackColor = true;
		this.txtExpiryDaysPoInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpiryDaysPoInfo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExpiryDaysPoInfo.Location = new System.Drawing.Point(637, 47);
		this.txtExpiryDaysPoInfo.Name = "txtExpiryDaysPoInfo";
		this.txtExpiryDaysPoInfo.Size = new System.Drawing.Size(122, 23);
		this.txtExpiryDaysPoInfo.TabIndex = 6;
		this.txtExpiryDaysPoInfo.Text = "7";
		this.txtExpiryDaysPoInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExpiryDaysPoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExpiryDaysPoInfo_KeyPress);
		this.RadEntryDatePoInfo.AutoSize = true;
		this.RadEntryDatePoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadEntryDatePoInfo.Location = new System.Drawing.Point(829, 43);
		this.RadEntryDatePoInfo.Name = "RadEntryDatePoInfo";
		this.RadEntryDatePoInfo.Size = new System.Drawing.Size(80, 19);
		this.RadEntryDatePoInfo.TabIndex = 10;
		this.RadEntryDatePoInfo.Text = "Entry Date";
		this.RadEntryDatePoInfo.UseVisualStyleBackColor = true;
		this.label34.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(637, 14);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(122, 30);
		this.label34.TabIndex = 114;
		this.label34.Text = "Hide Orders Expired Over Days Ago";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.RadModifyDatePoInfo.AutoSize = true;
		this.RadModifyDatePoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadModifyDatePoInfo.Location = new System.Drawing.Point(915, 22);
		this.RadModifyDatePoInfo.Name = "RadModifyDatePoInfo";
		this.RadModifyDatePoInfo.Size = new System.Drawing.Size(91, 19);
		this.RadModifyDatePoInfo.TabIndex = 11;
		this.RadModifyDatePoInfo.Text = "Modify Date";
		this.RadModifyDatePoInfo.UseVisualStyleBackColor = true;
		this.RadDocDatePoInfo.AutoSize = true;
		this.RadDocDatePoInfo.Checked = true;
		this.RadDocDatePoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadDocDatePoInfo.Location = new System.Drawing.Point(829, 22);
		this.RadDocDatePoInfo.Name = "RadDocDatePoInfo";
		this.RadDocDatePoInfo.Size = new System.Drawing.Size(75, 19);
		this.RadDocDatePoInfo.TabIndex = 9;
		this.RadDocDatePoInfo.TabStop = true;
		this.RadDocDatePoInfo.Text = "Doc Date";
		this.RadDocDatePoInfo.UseVisualStyleBackColor = true;
		this.cmbSupplierNamePoInfo.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierNamePoInfo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierNamePoInfo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierNamePoInfo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierNamePoInfo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierNamePoInfo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo).Location = new System.Drawing.Point(424, 16);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo).Name = "cmbSupplierNamePoInfo";
		((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierNamePoInfo).TabIndex = 4;
		this.txtToDocNoHistoryPoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistoryPoInfo.Location = new System.Drawing.Point(266, 47);
		this.txtToDocNoHistoryPoInfo.Name = "txtToDocNoHistoryPoInfo";
		this.txtToDocNoHistoryPoInfo.ShortcutsEnabled = false;
		this.txtToDocNoHistoryPoInfo.Size = new System.Drawing.Size(65, 23);
		this.txtToDocNoHistoryPoInfo.TabIndex = 3;
		this.txtToDocNoHistoryPoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistoryPoInfo_KeyPress);
		this.txtFromDocNoHistoryPoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistoryPoInfo.Location = new System.Drawing.Point(266, 18);
		this.txtFromDocNoHistoryPoInfo.Name = "txtFromDocNoHistoryPoInfo";
		this.txtFromDocNoHistoryPoInfo.ShortcutsEnabled = false;
		this.txtFromDocNoHistoryPoInfo.Size = new System.Drawing.Size(65, 23);
		this.txtFromDocNoHistoryPoInfo.TabIndex = 2;
		this.txtFromDocNoHistoryPoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistoryPoInfo_KeyPress);
		this.ToDateHistoryPoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryPoInfo.CustomFormat = "dd-MMM-yy";
		this.ToDateHistoryPoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistoryPoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistoryPoInfo.Location = new System.Drawing.Point(64, 47);
		this.ToDateHistoryPoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistoryPoInfo.Name = "ToDateHistoryPoInfo";
		this.ToDateHistoryPoInfo.ShowCheckBox = true;
		this.ToDateHistoryPoInfo.Size = new System.Drawing.Size(137, 23);
		this.ToDateHistoryPoInfo.TabIndex = 1;
		this.FromDateHistoryPoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryPoInfo.CustomFormat = "dd-MMM-yy";
		this.FromDateHistoryPoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistoryPoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistoryPoInfo.Location = new System.Drawing.Point(64, 18);
		this.FromDateHistoryPoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistoryPoInfo.Name = "FromDateHistoryPoInfo";
		this.FromDateHistoryPoInfo.ShowCheckBox = true;
		this.FromDateHistoryPoInfo.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistoryPoInfo.TabIndex = 0;
		this.BtnResetPoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnResetPoInfo.FlatAppearance.BorderSize = 0;
		this.BtnResetPoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetPoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetPoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnResetPoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnResetPoInfo.Location = new System.Drawing.Point(763, 16);
		this.BtnResetPoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetPoInfo.Name = "BtnResetPoInfo";
		this.BtnResetPoInfo.Size = new System.Drawing.Size(62, 26);
		this.BtnResetPoInfo.TabIndex = 7;
		this.BtnResetPoInfo.Text = "Reset";
		this.BtnResetPoInfo.UseVisualStyleBackColor = false;
		this.BtnResetPoInfo.Click += new System.EventHandler(BtnResetOrderInfo_Click);
		this.BtnShowPoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowPoInfo.FlatAppearance.BorderSize = 0;
		this.BtnShowPoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowPoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowPoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnShowPoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowPoInfo.Location = new System.Drawing.Point(763, 45);
		this.BtnShowPoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowPoInfo.Name = "BtnShowPoInfo";
		this.BtnShowPoInfo.Size = new System.Drawing.Size(62, 26);
		this.BtnShowPoInfo.TabIndex = 8;
		this.BtnShowPoInfo.Text = "Show";
		this.BtnShowPoInfo.UseVisualStyleBackColor = false;
		this.BtnShowPoInfo.Click += new System.EventHandler(btnshow_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(334, 22);
		this.label23.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(86, 15);
		this.label23.TabIndex = 142;
		this.label23.Text = "Supplier Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(204, 51);
		this.label24.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(45, 15);
		this.label24.TabIndex = 141;
		this.label24.Text = "To Doc";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(204, 22);
		this.label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(60, 15);
		this.label25.TabIndex = 140;
		this.label25.Text = "From Doc";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(4, 51);
		this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(48, 15);
		this.label26.TabIndex = 139;
		this.label26.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(4, 22);
		this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 13;
		this.label27.Text = "From Date";
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes);
		this.tabPage5.Controls.Add(this.panel16);
		this.tabPage5.Controls.Add(this.panel15);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1599, 255);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Stock Receiving Notes Information";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdStockReceivingNotes.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdStockReceivingNotes).BackColor = System.Drawing.Color.White;
		grdStockReceivingNotes_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStockReceivingNotes.DesignTimeLayout = grdStockReceivingNotes_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdStockReceivingNotes.DynamicFiltering = true;
		this.grdStockReceivingNotes.FilterMode = (FilterMode)2;
		this.grdStockReceivingNotes.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStockReceivingNotes.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).Location = new System.Drawing.Point(0, 75);
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).Name = "grdStockReceivingNotes";
		((ScrollableBase)this.grdStockReceivingNotes).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).Size = new System.Drawing.Size(1599, 180);
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).TabIndex = 0;
		this.grdStockReceivingNotes.TotalRow = (InheritableBoolean)1;
		this.grdStockReceivingNotes.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockReceivingNotes).VisualStyle = (VisualStyle)4;
		this.grdStockReceivingNotes.ColumnButtonClick += new ColumnActionEventHandler(grdStockReceivingNotes_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdStockReceivingNotes).KeyDown += new System.Windows.Forms.KeyEventHandler(grdStockReceivingNotes_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar4);
		this.panel16.Controls.Add(this.label7);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 48);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1599, 27);
		this.panel16.TabIndex = 7;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1556, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdStockReceivingNotes;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(37, 23);
		this.ctrlGrdBar4.TabIndex = 59;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(315, 16);
		this.label7.TabIndex = 60;
		this.label7.Text = "This Grid Show Pending Stock Transfer Vehicles.";
		this.panel15.Controls.Add(this.groupBox5);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1599, 48);
		this.panel15.TabIndex = 6;
		this.groupBox5.Controls.Add(this.RadApproveDateStockInfo);
		this.groupBox5.Controls.Add(this.RadEntryDateStockInfo);
		this.groupBox5.Controls.Add(this.RadModifyDateStockInfo);
		this.groupBox5.Controls.Add(this.RadDocDateStockInfo);
		this.groupBox5.Controls.Add(this.ToDocStockInfo);
		this.groupBox5.Controls.Add(this.FromDocStockInfo);
		this.groupBox5.Controls.Add(this.ToDateStockInfo);
		this.groupBox5.Controls.Add(this.FromDateStockInfo);
		this.groupBox5.Controls.Add(this.BtnResetStockInfo);
		this.groupBox5.Controls.Add(this.BtnShowStockInfo);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add(this.label30);
		this.groupBox5.Controls.Add(this.label31);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(3, 1);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1216, 46);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.RadApproveDateStockInfo.AutoSize = true;
		this.RadApproveDateStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadApproveDateStockInfo.Location = new System.Drawing.Point(1025, 17);
		this.RadApproveDateStockInfo.Name = "RadApproveDateStockInfo";
		this.RadApproveDateStockInfo.Size = new System.Drawing.Size(105, 19);
		this.RadApproveDateStockInfo.TabIndex = 9;
		this.RadApproveDateStockInfo.Text = "Approved Date";
		this.RadApproveDateStockInfo.UseVisualStyleBackColor = true;
		this.RadEntryDateStockInfo.AutoSize = true;
		this.RadEntryDateStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadEntryDateStockInfo.Location = new System.Drawing.Point(842, 17);
		this.RadEntryDateStockInfo.Name = "RadEntryDateStockInfo";
		this.RadEntryDateStockInfo.Size = new System.Drawing.Size(80, 19);
		this.RadEntryDateStockInfo.TabIndex = 7;
		this.RadEntryDateStockInfo.Text = "Entry Date";
		this.RadEntryDateStockInfo.UseVisualStyleBackColor = true;
		this.RadModifyDateStockInfo.AutoSize = true;
		this.RadModifyDateStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadModifyDateStockInfo.Location = new System.Drawing.Point(928, 17);
		this.RadModifyDateStockInfo.Name = "RadModifyDateStockInfo";
		this.RadModifyDateStockInfo.Size = new System.Drawing.Size(91, 19);
		this.RadModifyDateStockInfo.TabIndex = 8;
		this.RadModifyDateStockInfo.Text = "Modify Date";
		this.RadModifyDateStockInfo.UseVisualStyleBackColor = true;
		this.RadDocDateStockInfo.AutoSize = true;
		this.RadDocDateStockInfo.Checked = true;
		this.RadDocDateStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadDocDateStockInfo.Location = new System.Drawing.Point(761, 17);
		this.RadDocDateStockInfo.Name = "RadDocDateStockInfo";
		this.RadDocDateStockInfo.Size = new System.Drawing.Size(75, 19);
		this.RadDocDateStockInfo.TabIndex = 6;
		this.RadDocDateStockInfo.TabStop = true;
		this.RadDocDateStockInfo.Text = "Doc Date";
		this.RadDocDateStockInfo.UseVisualStyleBackColor = true;
		this.ToDocStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocStockInfo.Location = new System.Drawing.Point(558, 15);
		this.ToDocStockInfo.Name = "ToDocStockInfo";
		this.ToDocStockInfo.ShortcutsEnabled = false;
		this.ToDocStockInfo.Size = new System.Drawing.Size(65, 23);
		this.ToDocStockInfo.TabIndex = 3;
		this.ToDocStockInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocStockInfo_KeyPress);
		this.FromDocStockInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocStockInfo.Location = new System.Drawing.Point(445, 15);
		this.FromDocStockInfo.Name = "FromDocStockInfo";
		this.FromDocStockInfo.ShortcutsEnabled = false;
		this.FromDocStockInfo.Size = new System.Drawing.Size(65, 23);
		this.FromDocStockInfo.TabIndex = 2;
		this.FromDocStockInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocStockInfo_KeyPress);
		this.ToDateStockInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateStockInfo.CustomFormat = "dd-MMM-yy";
		this.ToDateStockInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateStockInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateStockInfo.Location = new System.Drawing.Point(250, 15);
		this.ToDateStockInfo.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateStockInfo.Name = "ToDateStockInfo";
		this.ToDateStockInfo.ShowCheckBox = true;
		this.ToDateStockInfo.Size = new System.Drawing.Size(131, 23);
		this.ToDateStockInfo.TabIndex = 1;
		this.FromDateStockInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateStockInfo.CustomFormat = "dd-MMM-yy";
		this.FromDateStockInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateStockInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateStockInfo.Location = new System.Drawing.Point(64, 15);
		this.FromDateStockInfo.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateStockInfo.Name = "FromDateStockInfo";
		this.FromDateStockInfo.ShowCheckBox = true;
		this.FromDateStockInfo.Size = new System.Drawing.Size(137, 23);
		this.FromDateStockInfo.TabIndex = 0;
		this.BtnResetStockInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnResetStockInfo.FlatAppearance.BorderSize = 0;
		this.BtnResetStockInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetStockInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnResetStockInfo.ForeColor = System.Drawing.Color.White;
		this.BtnResetStockInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnResetStockInfo.Location = new System.Drawing.Point(694, 13);
		this.BtnResetStockInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetStockInfo.Name = "BtnResetStockInfo";
		this.BtnResetStockInfo.Size = new System.Drawing.Size(62, 26);
		this.BtnResetStockInfo.TabIndex = 5;
		this.BtnResetStockInfo.Text = "Reset";
		this.BtnResetStockInfo.UseVisualStyleBackColor = false;
		this.BtnResetStockInfo.Click += new System.EventHandler(BtnResetStockInfo_Click);
		this.BtnShowStockInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowStockInfo.FlatAppearance.BorderSize = 0;
		this.BtnShowStockInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowStockInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowStockInfo.ForeColor = System.Drawing.Color.White;
		this.BtnShowStockInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowStockInfo.Location = new System.Drawing.Point(628, 13);
		this.BtnShowStockInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowStockInfo.Name = "BtnShowStockInfo";
		this.BtnShowStockInfo.Size = new System.Drawing.Size(62, 26);
		this.BtnShowStockInfo.TabIndex = 4;
		this.BtnShowStockInfo.Text = "Show";
		this.BtnShowStockInfo.UseVisualStyleBackColor = false;
		this.BtnShowStockInfo.Click += new System.EventHandler(BtnShowStockInfo_Click);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(513, 19);
		this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(45, 15);
		this.label29.TabIndex = 141;
		this.label29.Text = "To Doc";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(385, 19);
		this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(60, 15);
		this.label30.TabIndex = 140;
		this.label30.Text = "From Doc";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(204, 19);
		this.label31.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(48, 15);
		this.label31.TabIndex = 139;
		this.label31.Text = "To Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(2, 19);
		this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(63, 15);
		this.label32.TabIndex = 13;
		this.label32.Text = "From Date";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.PanelBreakup);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.pictureBox1);
		this.panel4.Controls.Add(this.txtBalQty);
		this.panel4.Controls.Add(this.txtRecQty);
		this.panel4.Controls.Add(this.txtOrderQty);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 58);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1607, 364);
		this.panel4.TabIndex = 0;
		this.PanelBreakup.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup);
		this.PanelBreakup.Controls.Add(this.panel22);
		this.PanelBreakup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelBreakup.Location = new System.Drawing.Point(1052, 0);
		this.PanelBreakup.Name = "PanelBreakup";
		this.PanelBreakup.Size = new System.Drawing.Size(555, 364);
		this.PanelBreakup.TabIndex = 92;
		((ScrollableBase)this.grdPurchaseBrakup).BackColor = System.Drawing.Color.White;
		grdPurchaseBrakup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseBrakup.DesignTimeLayout = grdPurchaseBrakup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseBrakup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Name = "grdPurchaseBrakup";
		((ScrollableBase)this.grdPurchaseBrakup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).Size = new System.Drawing.Size(555, 338);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).TabIndex = 0;
		this.grdPurchaseBrakup.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseBrakup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseBrakup).VisualStyle = (VisualStyle)4;
		this.grdPurchaseBrakup.CellUpdated += new ColumnActionEventHandler(grdPurchaseBrakup_CellUpdated);
		this.grdPurchaseBrakup.ColumnButtonClick += new ColumnActionEventHandler(grdPurchaseBrakup_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPurchaseBrakup).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchaseBrakup_KeyDown);
		this.panel22.BackColor = System.Drawing.Color.White;
		this.panel22.Controls.Add(this.label38);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(555, 26);
		this.panel22.TabIndex = 3;
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.label38.ForeColor = System.Drawing.Color.Teal;
		this.label38.Location = new System.Drawing.Point(3, 6);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(129, 14);
		this.label38.TabIndex = 25;
		this.label38.Text = "Purchase BreakUp";
		this.panel6.Controls.Add(this.groupBox6);
		this.panel6.Controls.Add(this.groupBox2);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1052, 364);
		this.panel6.TabIndex = 91;
		this.groupBox2.Controls.Add(this.label67);
		this.groupBox2.Controls.Add(this.txtDifferenceWeight);
		this.groupBox2.Controls.Add(this.txtSupplierWeight);
		this.groupBox2.Controls.Add(this.txtSecondWtDifference);
		this.groupBox2.Controls.Add(this.txtSupplierSecondWeight);
		this.groupBox2.Controls.Add(this.label80);
		this.groupBox2.Controls.Add(this.label82);
		this.groupBox2.Controls.Add(this.txtFirstWtDifference);
		this.groupBox2.Controls.Add(this.label83);
		this.groupBox2.Controls.Add(this.txtSupplierFirstWeight);
		this.groupBox2.Controls.Add(this.label84);
		this.groupBox2.Controls.Add(this.label85);
		this.groupBox2.Controls.Add(this.label86);
		this.groupBox2.Controls.Add(this.txtFactoryWeight);
		this.groupBox2.Controls.Add(this.txtsecondWeight);
		this.groupBox2.Controls.Add(this.txtFirstWeight);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtPackUnit);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.txtAccessWeight);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.txtTotalPayablesFreight);
		this.groupBox2.Controls.Add(this.label69);
		this.groupBox2.Controls.Add(this.txtintime);
		this.groupBox2.Controls.Add(this.txtAdvanceByParty);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.txtAdvanceByFactory);
		this.groupBox2.Controls.Add(this.txtouttime);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.lblDiffRemarks);
		this.groupBox2.Controls.Add(this.txtgpdate);
		this.groupBox2.Controls.Add(this.txtWeightDiffRemarks);
		this.groupBox2.Controls.Add(this.label68);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContainerNo1);
		this.groupBox2.Controls.Add(this.ContainerNo1);
		this.groupBox2.Controls.Add(this.txtgpno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContainerNo2);
		this.groupBox2.Controls.Add(this.label74);
		this.groupBox2.Controls.Add(this.containerno2);
		this.groupBox2.Controls.Add(this.txtgptypeno);
		this.groupBox2.Controls.Add(this.label76);
		this.groupBox2.Controls.Add(this.label75);
		this.groupBox2.Controls.Add(this.CmbOrderno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupp);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgptype);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderType);
		this.groupBox2.Controls.Add(this.label64);
		this.groupBox2.Controls.Add(this.label61);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label66);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label58);
		this.groupBox2.Controls.Add(this.label73);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWeighBridge);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.txtWeighBridgeSlipNo);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.label59);
		this.groupBox2.Controls.Add(this.txtBiltyDate);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.txtNetPaid);
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtfreight);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariety);
		this.groupBox2.Controls.Add(this.txtWeightCompared);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtbiltyno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.txtvehicleno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1040, 263);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.label67.AutoSize = true;
		this.label67.BackColor = System.Drawing.Color.White;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(567, 106);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(70, 15);
		this.label67.TabIndex = 132;
		this.label67.Text = "W.B Slip No";
		this.txtDifferenceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDifferenceWeight.Enabled = false;
		this.txtDifferenceWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtDifferenceWeight.Location = new System.Drawing.Point(872, 76);
		this.txtDifferenceWeight.Name = "txtDifferenceWeight";
		this.txtDifferenceWeight.Size = new System.Drawing.Size(162, 24);
		this.txtDifferenceWeight.TabIndex = 31;
		this.txtDifferenceWeight.Tag = "double";
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierWeight.Location = new System.Drawing.Point(761, 76);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierWeight.TabIndex = 30;
		this.txtSupplierWeight.Tag = "double";
		this.txtSupplierWeight.TextChanged += new System.EventHandler(txtSupplierWeight_TextChanged);
		this.txtSupplierWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierWeight_KeyPress);
		this.txtSecondWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSecondWtDifference.Enabled = false;
		this.txtSecondWtDifference.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSecondWtDifference.Location = new System.Drawing.Point(872, 50);
		this.txtSecondWtDifference.Name = "txtSecondWtDifference";
		this.txtSecondWtDifference.Size = new System.Drawing.Size(162, 24);
		this.txtSecondWtDifference.TabIndex = 28;
		this.txtSecondWtDifference.Tag = "double";
		this.txtSupplierSecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierSecondWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierSecondWeight.Location = new System.Drawing.Point(761, 50);
		this.txtSupplierSecondWeight.Name = "txtSupplierSecondWeight";
		this.txtSupplierSecondWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierSecondWeight.TabIndex = 27;
		this.txtSupplierSecondWeight.Tag = "double";
		this.txtSupplierSecondWeight.TextChanged += new System.EventHandler(txtSupplierSecondWeight_TextChanged);
		this.txtSupplierSecondWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierSecondWeight_KeyPress);
		this.label80.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label80.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label80.Location = new System.Drawing.Point(872, 5);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(162, 18);
		this.label80.TabIndex = 254;
		this.label80.Text = "Difference";
		this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label82.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label82.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label82.Location = new System.Drawing.Point(761, 5);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(110, 18);
		this.label82.TabIndex = 255;
		this.label82.Text = "Supplier";
		this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtFirstWtDifference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWtDifference.Enabled = false;
		this.txtFirstWtDifference.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtFirstWtDifference.Location = new System.Drawing.Point(872, 24);
		this.txtFirstWtDifference.Name = "txtFirstWtDifference";
		this.txtFirstWtDifference.Size = new System.Drawing.Size(162, 24);
		this.txtFirstWtDifference.TabIndex = 25;
		this.txtFirstWtDifference.Tag = "double";
		this.label83.BackColor = System.Drawing.Color.WhiteSmoke;
		this.label83.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label83.Location = new System.Drawing.Point(650, 5);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(110, 18);
		this.label83.TabIndex = 256;
		this.label83.Text = "Factory";
		this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSupplierFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierFirstWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtSupplierFirstWeight.Location = new System.Drawing.Point(761, 24);
		this.txtSupplierFirstWeight.Name = "txtSupplierFirstWeight";
		this.txtSupplierFirstWeight.Size = new System.Drawing.Size(110, 24);
		this.txtSupplierFirstWeight.TabIndex = 24;
		this.txtSupplierFirstWeight.Tag = "double";
		this.txtSupplierFirstWeight.TextChanged += new System.EventHandler(txtSupplierFirstWeight_TextChanged);
		this.txtSupplierFirstWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSupplierFirstWeight_KeyPress);
		this.label84.AutoSize = true;
		this.label84.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label84.Location = new System.Drawing.Point(567, 54);
		this.label84.Name = "label84";
		this.label84.Size = new System.Drawing.Size(81, 17);
		this.label84.TabIndex = 253;
		this.label84.Text = "Tare Weight";
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label85.Location = new System.Drawing.Point(567, 28);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(85, 17);
		this.label85.TabIndex = 252;
		this.label85.Text = "Load Weight";
		this.label86.AutoSize = true;
		this.label86.Font = new System.Drawing.Font("Segoe UI Semibold", 9.25f, System.Drawing.FontStyle.Bold);
		this.label86.Location = new System.Drawing.Point(567, 81);
		this.label86.Name = "label86";
		this.label86.Size = new System.Drawing.Size(78, 17);
		this.label86.TabIndex = 251;
		this.label86.Text = "Net Weight";
		this.txtFactoryWeight.BackColor = System.Drawing.Color.White;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtFactoryWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtFactoryWeight.Location = new System.Drawing.Point(650, 76);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(110, 24);
		this.txtFactoryWeight.TabIndex = 29;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.Tag = "double";
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtFactoryWeight_TextChanged);
		this.txtsecondWeight.BackColor = System.Drawing.Color.White;
		this.txtsecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsecondWeight.Enabled = false;
		this.txtsecondWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtsecondWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtsecondWeight.Location = new System.Drawing.Point(650, 50);
		this.txtsecondWeight.Name = "txtsecondWeight";
		this.txtsecondWeight.ReadOnly = true;
		this.txtsecondWeight.Size = new System.Drawing.Size(110, 24);
		this.txtsecondWeight.TabIndex = 26;
		this.txtsecondWeight.TabStop = false;
		this.txtsecondWeight.Tag = "double";
		this.txtsecondWeight.TextChanged += new System.EventHandler(txtsecondWeight_TextChanged);
		this.txtFirstWeight.BackColor = System.Drawing.Color.White;
		this.txtFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWeight.Enabled = false;
		this.txtFirstWeight.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold);
		this.txtFirstWeight.ForeColor = System.Drawing.Color.Blue;
		this.txtFirstWeight.Location = new System.Drawing.Point(650, 24);
		this.txtFirstWeight.Name = "txtFirstWeight";
		this.txtFirstWeight.ReadOnly = true;
		this.txtFirstWeight.Size = new System.Drawing.Size(110, 24);
		this.txtFirstWeight.TabIndex = 23;
		this.txtFirstWeight.Tag = "double";
		this.txtFirstWeight.TextChanged += new System.EventHandler(txtFirstWeight_TextChanged);
		this.label44.AutoSize = true;
		this.label44.BackColor = System.Drawing.Color.White;
		this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Red;
		this.label44.Location = new System.Drawing.Point(409, 113);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(13, 16);
		this.label44.TabIndex = 124;
		this.label44.Text = "*";
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackUnit.Location = new System.Drawing.Point(242, 176);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(98, 23);
		this.txtPackUnit.TabIndex = 10;
		this.txtPackUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackUnit.TextChanged += new System.EventHandler(txtPackUnit_TextChanged);
		this.txtPackUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUnit_KeyPress);
		this.CmbSupplierDispatchedPreBillNo.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierDispatchedPreBillNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierDispatchedPreBillNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierDispatchedPreBillNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierDispatchedPreBillNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierDispatchedPreBillNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Location = new System.Drawing.Point(650, 207);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Name = "CmbSupplierDispatchedPreBillNo";
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).TabIndex = 36;
		((System.Windows.Forms.Control)(object)this.CmbSupplierDispatchedPreBillNo).Leave += new System.EventHandler(CmbSupplierDispatchedPreBillNo_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(567, 213);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(83, 15);
		this.label13.TabIndex = 241;
		this.label13.Text = "Transit Vehicle";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Black;
		this.label14.Location = new System.Drawing.Point(761, 130);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(57, 15);
		this.label14.TabIndex = 239;
		this.label14.Text = "Out Time";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(343, 181);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(80, 30);
		this.label36.TabIndex = 235;
		this.label36.Text = "Total Payable \r\nFreight";
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(343, 152);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(72, 30);
		this.label37.TabIndex = 236;
		this.label37.Text = "Advance By \r\nFactory";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(761, 155);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(77, 15);
		this.label35.TabIndex = 231;
		this.label35.Text = "Packing Type";
		this.txtAccessWeight.Enabled = false;
		this.txtAccessWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccessWeight.Location = new System.Drawing.Point(650, 183);
		this.txtAccessWeight.Name = "txtAccessWeight";
		this.txtAccessWeight.Size = new System.Drawing.Size(110, 23);
		this.txtAccessWeight.TabIndex = 35;
		this.txtAccessWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAccessWeight.TextChanged += new System.EventHandler(txtAccessWeight_TextChanged);
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(343, 138);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(99, 15);
		this.label40.TabIndex = 237;
		this.label40.Text = "Advance By Party";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(567, 187);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(80, 15);
		this.label33.TabIndex = 229;
		this.label33.Text = "P.O Excess Wt";
		this.txtTotalPayablesFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalPayablesFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalPayablesFreight.Location = new System.Drawing.Point(441, 185);
		this.txtTotalPayablesFreight.Name = "txtTotalPayablesFreight";
		this.txtTotalPayablesFreight.ReadOnly = true;
		this.txtTotalPayablesFreight.Size = new System.Drawing.Size(125, 23);
		this.txtTotalPayablesFreight.TabIndex = 20;
		this.txtTotalPayablesFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label69.AutoSize = true;
		this.label69.BackColor = System.Drawing.Color.White;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Black;
		this.label69.Location = new System.Drawing.Point(761, 106);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(48, 15);
		this.label69.TabIndex = 126;
		this.label69.Text = "In Time";
		this.txtintime.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtintime.CustomFormat = "dd-MMM-yyyy hh:mm tt ";
		this.txtintime.Enabled = false;
		this.txtintime.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtintime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtintime.Location = new System.Drawing.Point(851, 102);
		this.txtintime.Name = "txtintime";
		this.txtintime.Size = new System.Drawing.Size(183, 22);
		this.txtintime.TabIndex = 37;
		this.txtintime.TabStop = false;
		this.txtintime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.txtAdvanceByParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceByParty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdvanceByParty.Location = new System.Drawing.Point(441, 134);
		this.txtAdvanceByParty.Name = "txtAdvanceByParty";
		this.txtAdvanceByParty.Size = new System.Drawing.Size(125, 23);
		this.txtAdvanceByParty.TabIndex = 18;
		this.txtAdvanceByParty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdvanceByParty.TextChanged += new System.EventHandler(txtAdvanceByParty_TextChanged);
		this.txtAdvanceByParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfreight_KeyPress);
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(409, 66);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(13, 16);
		this.label16.TabIndex = 228;
		this.label16.Text = "*";
		this.txtAdvanceByFactory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAdvanceByFactory.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAdvanceByFactory.Location = new System.Drawing.Point(441, 158);
		this.txtAdvanceByFactory.Name = "txtAdvanceByFactory";
		this.txtAdvanceByFactory.Size = new System.Drawing.Size(125, 23);
		this.txtAdvanceByFactory.TabIndex = 19;
		this.txtAdvanceByFactory.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAdvanceByFactory.TextChanged += new System.EventHandler(txtAdvanceByFactory_TextChanged);
		this.txtAdvanceByFactory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfreight_KeyPress);
		this.txtouttime.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtouttime.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtouttime.CustomFormat = "dd-MMM-yyyy hh:mm tt ";
		this.txtouttime.Enabled = false;
		this.txtouttime.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtouttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtouttime.Location = new System.Drawing.Point(851, 126);
		this.txtouttime.Name = "txtouttime";
		this.txtouttime.Size = new System.Drawing.Size(183, 22);
		this.txtouttime.TabIndex = 38;
		this.txtouttime.TabStop = false;
		this.txtouttime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(409, 212);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(13, 16);
		this.label12.TabIndex = 226;
		this.label12.Text = "*";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(230, 182);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(13, 16);
		this.label10.TabIndex = 225;
		this.label10.Text = "*";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(343, 43);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(64, 15);
		this.label9.TabIndex = 224;
		this.label9.Text = "Vehicle No";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(343, 91);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 223;
		this.label8.Text = "Bilty Date";
		this.lblDiffRemarks.AutoSize = true;
		this.lblDiffRemarks.BackColor = System.Drawing.Color.White;
		this.lblDiffRemarks.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblDiffRemarks.Location = new System.Drawing.Point(1, 238);
		this.lblDiffRemarks.Name = "lblDiffRemarks";
		this.lblDiffRemarks.Size = new System.Drawing.Size(94, 15);
		this.lblDiffRemarks.TabIndex = 134;
		this.lblDiffRemarks.Text = "Wt Diff Remarks";
		this.lblDiffRemarks.Visible = false;
		this.txtgpdate.CustomFormat = "dd-MMM-yyyy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(109, 13);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(130, 22);
		this.txtgpdate.TabIndex = 0;
		this.txtWeightDiffRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightDiffRemarks.Location = new System.Drawing.Point(109, 234);
		this.txtWeightDiffRemarks.Name = "txtWeightDiffRemarks";
		this.txtWeightDiffRemarks.Size = new System.Drawing.Size(231, 23);
		this.txtWeightDiffRemarks.TabIndex = 12;
		this.txtWeightDiffRemarks.Visible = false;
		this.label68.AutoSize = true;
		this.label68.BackColor = System.Drawing.Color.White;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label68.Location = new System.Drawing.Point(343, 238);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(93, 15);
		this.label68.TabIndex = 131;
		this.label68.Text = "Remarks Header";
		this.cmbContainerNo1.AllowNull = (DefaultableBoolean)1;
		this.cmbContainerNo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContainerNo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContainerNo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContainerNo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContainerNo1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbContainerNo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContainerNo1).Location = new System.Drawing.Point(851, 177);
		((System.Windows.Forms.Control)(object)this.cmbContainerNo1).Name = "cmbContainerNo1";
		((System.Windows.Forms.Control)(object)this.cmbContainerNo1).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbContainerNo1).TabIndex = 40;
		this.ContainerNo1.AutoSize = true;
		this.ContainerNo1.BackColor = System.Drawing.Color.White;
		this.ContainerNo1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ContainerNo1.Location = new System.Drawing.Point(761, 183);
		this.ContainerNo1.Name = "ContainerNo1";
		this.ContainerNo1.Size = new System.Drawing.Size(87, 15);
		this.ContainerNo1.TabIndex = 52;
		this.ContainerNo1.Text = "Container No-1";
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Enabled = false;
		this.txtgpno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpno.Location = new System.Drawing.Point(240, 13);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(100, 23);
		this.txtgpno.TabIndex = 1;
		this.txtgpno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbContainerNo2.AllowNull = (DefaultableBoolean)1;
		this.cmbContainerNo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContainerNo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContainerNo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContainerNo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContainerNo2).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbContainerNo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContainerNo2).Location = new System.Drawing.Point(851, 205);
		((System.Windows.Forms.Control)(object)this.cmbContainerNo2).Name = "cmbContainerNo2";
		((System.Windows.Forms.Control)(object)this.cmbContainerNo2).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.cmbContainerNo2).TabIndex = 41;
		this.label74.AutoSize = true;
		this.label74.BackColor = System.Drawing.Color.White;
		this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Red;
		this.label74.Location = new System.Drawing.Point(632, 132);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(13, 16);
		this.label74.TabIndex = 142;
		this.label74.Text = "*";
		this.containerno2.AutoSize = true;
		this.containerno2.BackColor = System.Drawing.Color.White;
		this.containerno2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.containerno2.Location = new System.Drawing.Point(761, 211);
		this.containerno2.Name = "containerno2";
		this.containerno2.Size = new System.Drawing.Size(89, 15);
		this.containerno2.TabIndex = 54;
		this.containerno2.Text = "Container No-2";
		this.txtgptypeno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgptypeno.Enabled = false;
		this.txtgptypeno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgptypeno.Location = new System.Drawing.Point(240, 40);
		this.txtgptypeno.Name = "txtgptypeno";
		this.txtgptypeno.ReadOnly = true;
		this.txtgptypeno.Size = new System.Drawing.Size(100, 23);
		this.txtgptypeno.TabIndex = 3;
		this.txtgptypeno.TabStop = false;
		this.txtgptypeno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label76.AutoSize = true;
		this.label76.BackColor = System.Drawing.Color.White;
		this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.ForeColor = System.Drawing.Color.Red;
		this.label76.Location = new System.Drawing.Point(632, 160);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(13, 16);
		this.label76.TabIndex = 140;
		this.label76.Text = "*";
		this.label75.AutoSize = true;
		this.label75.BackColor = System.Drawing.Color.White;
		this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Red;
		this.label75.Location = new System.Drawing.Point(635, 109);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(13, 16);
		this.label75.TabIndex = 141;
		this.label75.Text = "*";
		this.CmbOrderno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbOrderno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderno.Location = new System.Drawing.Point(240, 67);
		this.CmbOrderno.Name = "CmbOrderno";
		this.CmbOrderno.Size = new System.Drawing.Size(100, 23);
		this.CmbOrderno.TabIndex = 5;
		this.CmbOrderno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CmbOrderno_KeyPress);
		this.CmbOrderno.Leave += new System.EventHandler(CmbOrderno_Leave);
		this.cmbsupp.AllowNull = (DefaultableBoolean)1;
		this.cmbsupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbsupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupp).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupp).Location = new System.Drawing.Point(109, 92);
		((System.Windows.Forms.Control)(object)this.cmbsupp).Name = "cmbsupp";
		((System.Windows.Forms.Control)(object)this.cmbsupp).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupp).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbsupp).Leave += new System.EventHandler(cmbsupp_Leave);
		this.cmbgptype.AllowNull = (DefaultableBoolean)1;
		this.cmbgptype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgptype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgptype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbgptype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbgptype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Location = new System.Drawing.Point(109, 38);
		((System.Windows.Forms.Control)(object)this.cmbgptype).Name = "cmbgptype";
		((System.Windows.Forms.Control)(object)this.cmbgptype).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbgptype).TabIndex = 2;
		this.cmbgptype.ValueChanged += new System.EventHandler(cmbgptype_Leave_1);
		((System.Windows.Forms.Control)(object)this.cmbgptype).Leave += new System.EventHandler(cmbgptype_Leave);
		this.CmbOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Location = new System.Drawing.Point(109, 65);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Name = "CmbOrderType";
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Leave += new System.EventHandler(CmbOrderType_Leave);
		this.label64.AutoSize = true;
		this.label64.BackColor = System.Drawing.Color.White;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.Location = new System.Drawing.Point(3, 98);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(86, 15);
		this.label64.TabIndex = 106;
		this.label64.Text = "Supplier Name";
		this.label61.AutoSize = true;
		this.label61.BackColor = System.Drawing.Color.White;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(3, 17);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(84, 15);
		this.label61.TabIndex = 114;
		this.label61.Text = "G.P Date && No";
		this.label56.AutoSize = true;
		this.label56.BackColor = System.Drawing.Color.White;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(3, 44);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(87, 15);
		this.label56.TabIndex = 113;
		this.label56.Text = "G.P Type && No ";
		this.label55.AutoSize = true;
		this.label55.BackColor = System.Drawing.Color.White;
		this.label55.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.Red;
		this.label55.Location = new System.Drawing.Point(95, 70);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(13, 16);
		this.label55.TabIndex = 115;
		this.label55.Text = "*";
		this.label45.AutoSize = true;
		this.label45.BackColor = System.Drawing.Color.White;
		this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(95, 209);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(13, 16);
		this.label45.TabIndex = 11;
		this.label45.Text = "*";
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.White;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(95, 146);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(13, 16);
		this.label43.TabIndex = 125;
		this.label43.Text = "*";
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.White;
		this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(95, 179);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(13, 16);
		this.label51.TabIndex = 9;
		this.label51.Text = "*";
		this.label53.BackColor = System.Drawing.Color.White;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(3, 63);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(86, 30);
		this.label53.TabIndex = 117;
		this.label53.Text = "Purchase Type && No";
		this.label66.AutoSize = true;
		this.label66.BackColor = System.Drawing.Color.White;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(567, 133);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(64, 15);
		this.label66.TabIndex = 133;
		this.label66.Text = "W.B Status";
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.White;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(3, 180);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(54, 15);
		this.label57.TabIndex = 112;
		this.label57.Text = "Item Qty";
		this.label52.AutoSize = true;
		this.label52.BackColor = System.Drawing.Color.White;
		this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(95, 43);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(13, 16);
		this.label52.TabIndex = 118;
		this.label52.Text = "*";
		this.label58.AutoSize = true;
		this.label58.BackColor = System.Drawing.Color.White;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(3, 210);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(46, 15);
		this.label58.TabIndex = 111;
		this.label58.Text = "Weight";
		this.label73.AutoSize = true;
		this.label73.BackColor = System.Drawing.Color.White;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label73.Location = new System.Drawing.Point(567, 161);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(61, 15);
		this.label73.TabIndex = 127;
		this.label73.Text = "G.P Status";
		this.label49.AutoSize = true;
		this.label49.BackColor = System.Drawing.Color.White;
		this.label49.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(95, 16);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(13, 16);
		this.label49.TabIndex = 119;
		this.label49.Text = "*";
		this.label50.AutoSize = true;
		this.label50.BackColor = System.Drawing.Color.White;
		this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(409, 42);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(13, 16);
		this.label50.TabIndex = 105;
		this.label50.Text = "*";
		this.label60.AutoSize = true;
		this.label60.BackColor = System.Drawing.Color.White;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(3, 152);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(67, 15);
		this.label60.TabIndex = 109;
		this.label60.Text = "Item Name";
		this.cmbWeighBridge.AllowNull = (DefaultableBoolean)1;
		this.cmbWeighBridge.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWeighBridge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWeighBridge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWeighBridge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Location = new System.Drawing.Point(650, 127);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Name = "cmbWeighBridge";
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).TabIndex = 33;
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.White;
		this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.Red;
		this.label54.Location = new System.Drawing.Point(409, 90);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(13, 16);
		this.label54.TabIndex = 116;
		this.label54.Text = "*";
		this.txtWeighBridgeSlipNo.BackColor = System.Drawing.SystemColors.Control;
		this.txtWeighBridgeSlipNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighBridgeSlipNo.Enabled = false;
		this.txtWeighBridgeSlipNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeighBridgeSlipNo.Location = new System.Drawing.Point(650, 102);
		this.txtWeighBridgeSlipNo.Name = "txtWeighBridgeSlipNo";
		this.txtWeighBridgeSlipNo.Size = new System.Drawing.Size(110, 23);
		this.txtWeighBridgeSlipNo.TabIndex = 32;
		this.txtWeighBridgeSlipNo.TabStop = false;
		this.txtWeighBridgeSlipNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label47.AutoSize = true;
		this.label47.BackColor = System.Drawing.Color.White;
		this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(95, 97);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(13, 16);
		this.label47.TabIndex = 121;
		this.label47.Text = "*";
		this.label59.AutoSize = true;
		this.label59.BackColor = System.Drawing.Color.White;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(343, 114);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(70, 15);
		this.label59.TabIndex = 110;
		this.label59.Text = "Bilty Freight";
		this.txtBiltyDate.CustomFormat = "dd-MMM-yyyy";
		this.txtBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBiltyDate.Location = new System.Drawing.Point(441, 87);
		this.txtBiltyDate.Name = "txtBiltyDate";
		this.txtBiltyDate.Size = new System.Drawing.Size(125, 22);
		this.txtBiltyDate.TabIndex = 16;
		this.label48.AutoSize = true;
		this.label48.BackColor = System.Drawing.Color.White;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(343, 67);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(49, 15);
		this.label48.TabIndex = 120;
		this.label48.Text = "Bilty No";
		this.txtNetPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetPaid.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetPaid.Location = new System.Drawing.Point(441, 210);
		this.txtNetPaid.Name = "txtNetPaid";
		this.txtNetPaid.Size = new System.Drawing.Size(125, 23);
		this.txtNetPaid.TabIndex = 21;
		this.txtNetPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetPaid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetPaid_KeyPress);
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.White;
		this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(95, 124);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(13, 16);
		this.label46.TabIndex = 122;
		this.label46.Text = "*";
		this.label62.AutoSize = true;
		this.label62.BackColor = System.Drawing.Color.White;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(3, 125);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(62, 15);
		this.label62.TabIndex = 108;
		this.label62.Text = "City Name";
		this.txtfreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfreight.Location = new System.Drawing.Point(441, 110);
		this.txtfreight.Name = "txtfreight";
		this.txtfreight.Size = new System.Drawing.Size(125, 23);
		this.txtfreight.TabIndex = 17;
		this.txtfreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfreight_KeyPress);
		this.txtfreight.Leave += new System.EventHandler(txtfreight_Leave);
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(343, 213);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 51;
		this.label11.Text = "Net Paid";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(109, 176);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(90, 23);
		this.txtqty.TabIndex = 9;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.CmbVariety.AllowNull = (DefaultableBoolean)1;
		this.CmbVariety.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariety.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariety.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbVariety).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVariety.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Location = new System.Drawing.Point(109, 146);
		((System.Windows.Forms.Control)(object)this.CmbVariety).Name = "CmbVariety";
		((System.Windows.Forms.Control)(object)this.CmbVariety).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.CmbVariety).TabIndex = 8;
		this.txtWeightCompared.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCompared.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightCompared.Location = new System.Drawing.Point(109, 206);
		this.txtWeightCompared.Name = "txtWeightCompared";
		this.txtWeightCompared.ReadOnly = true;
		this.txtWeightCompared.Size = new System.Drawing.Size(231, 23);
		this.txtWeightCompared.TabIndex = 11;
		this.txtWeightCompared.TabStop = false;
		this.txtWeightCompared.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(650, 155);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 34;
		this.label63.AutoSize = true;
		this.label63.BackColor = System.Drawing.Color.White;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(343, 17);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(73, 15);
		this.label63.TabIndex = 107;
		this.label63.Text = "Vehicle Type";
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(202, 172);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(33, 30);
		this.label42.TabIndex = 54;
		this.label42.Text = "Pack\r\nUom";
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbiltyno.Location = new System.Drawing.Point(441, 63);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(125, 23);
		this.txtbiltyno.TabIndex = 15;
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(109, 119);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(231, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 7;
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(441, 11);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 13;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(442, 234);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(318, 23);
		this.txtremarks.TabIndex = 22;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(441, 39);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(125, 23);
		this.txtvehicleno.TabIndex = 14;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.txtvehicleno.KeyDown += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyDown);
		this.txtvehicleno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtvehicleno_KeyPress);
		this.txtvehicleno.KeyUp += new System.Windows.Forms.KeyEventHandler(txtvehicleno_KeyUp);
		this.txtvehicleno.Leave += new System.EventHandler(txtvehicleno_Leave);
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(851, 149);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 39;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtAnaylstName);
		this.groupBox1.Controls.Add(this.txtLabReportNo);
		this.groupBox1.Controls.Add(this.txtReportStatus);
		this.groupBox1.Controls.Add(this.txtLabRemarks);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 264);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1041, 42);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Lab Info";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 76;
		this.label1.Text = "Analyst Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(311, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(61, 15);
		this.label2.TabIndex = 78;
		this.label2.Text = "Report No";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(487, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(78, 15);
		this.label3.TabIndex = 80;
		this.label3.Text = "Report Status";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(680, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 15);
		this.label4.TabIndex = 82;
		this.label4.Text = "Remarks";
		this.txtAnaylstName.BackColor = System.Drawing.SystemColors.Control;
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Enabled = false;
		this.txtAnaylstName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAnaylstName.Location = new System.Drawing.Point(87, 15);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.ReadOnly = true;
		this.txtAnaylstName.Size = new System.Drawing.Size(220, 23);
		this.txtAnaylstName.TabIndex = 0;
		this.txtAnaylstName.TabStop = false;
		this.txtLabReportNo.BackColor = System.Drawing.SystemColors.Control;
		this.txtLabReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabReportNo.Enabled = false;
		this.txtLabReportNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLabReportNo.Location = new System.Drawing.Point(372, 15);
		this.txtLabReportNo.Name = "txtLabReportNo";
		this.txtLabReportNo.ReadOnly = true;
		this.txtLabReportNo.Size = new System.Drawing.Size(113, 23);
		this.txtLabReportNo.TabIndex = 1;
		this.txtLabReportNo.TabStop = false;
		this.txtReportStatus.BackColor = System.Drawing.SystemColors.Control;
		this.txtReportStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReportStatus.Enabled = false;
		this.txtReportStatus.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReportStatus.Location = new System.Drawing.Point(566, 15);
		this.txtReportStatus.Name = "txtReportStatus";
		this.txtReportStatus.ReadOnly = true;
		this.txtReportStatus.Size = new System.Drawing.Size(111, 23);
		this.txtReportStatus.TabIndex = 2;
		this.txtReportStatus.TabStop = false;
		this.txtLabRemarks.BackColor = System.Drawing.SystemColors.Control;
		this.txtLabRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLabRemarks.Enabled = false;
		this.txtLabRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLabRemarks.Location = new System.Drawing.Point(733, 15);
		this.txtLabRemarks.Name = "txtLabRemarks";
		this.txtLabRemarks.ReadOnly = true;
		this.txtLabRemarks.Size = new System.Drawing.Size(302, 23);
		this.txtLabRemarks.TabIndex = 3;
		this.txtLabRemarks.TabStop = false;
		this.pictureBox1.Location = new System.Drawing.Point(1270, 11);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(120, 102);
		this.pictureBox1.TabIndex = 90;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Visible = false;
		this.txtBalQty.Location = new System.Drawing.Point(1270, 118);
		this.txtBalQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtBalQty.Name = "txtBalQty";
		this.txtBalQty.ReadOnly = true;
		this.txtBalQty.Size = new System.Drawing.Size(44, 21);
		this.txtBalQty.TabIndex = 71;
		this.txtBalQty.TabStop = false;
		this.txtBalQty.Visible = false;
		this.txtRecQty.Location = new System.Drawing.Point(1270, 171);
		this.txtRecQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtRecQty.Name = "txtRecQty";
		this.txtRecQty.Size = new System.Drawing.Size(44, 21);
		this.txtRecQty.TabIndex = 71;
		this.txtRecQty.Visible = false;
		this.txtOrderQty.Location = new System.Drawing.Point(1270, 148);
		this.txtOrderQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(44, 21);
		this.txtOrderQty.TabIndex = 69;
		this.txtOrderQty.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.FreightVoucherFormToOpen);
		this.panel3.Controls.Add(this.RadFreightVoucherFormToOpen);
		this.panel3.Controls.Add(this.chkDriverInfoForm);
		this.panel3.Controls.Add(this.labelGrnForm);
		this.panel3.Controls.Add(this.labelWeightBridgeForm);
		this.panel3.Controls.Add(this.labelLabAnalysisForm);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.rdFreightCredit);
		this.panel3.Controls.Add(this.rdFreightCash);
		this.panel3.Controls.Add(this.RadNoneFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadGrnFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadWBFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadPurchaseLabFormToOpenOnInsert);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Controls.Add(this.ChkIsApproved);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1607, 27);
		this.panel3.TabIndex = 1;
		this.FreightVoucherFormToOpen.AutoSize = true;
		this.FreightVoucherFormToOpen.BackColor = System.Drawing.Color.Teal;
		this.FreightVoucherFormToOpen.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.FreightVoucherFormToOpen.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.FreightVoucherFormToOpen.Location = new System.Drawing.Point(618, 6);
		this.FreightVoucherFormToOpen.Name = "FreightVoucherFormToOpen";
		this.FreightVoucherFormToOpen.Size = new System.Drawing.Size(112, 14);
		this.FreightVoucherFormToOpen.TabIndex = 48;
		this.FreightVoucherFormToOpen.TabStop = true;
		this.FreightVoucherFormToOpen.Text = "Freight Voucher";
		this.FreightVoucherFormToOpen.Click += new System.EventHandler(FreightVoucherFormToOpen_Click);
		this.RadFreightVoucherFormToOpen.AutoSize = true;
		this.RadFreightVoucherFormToOpen.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadFreightVoucherFormToOpen.ForeColor = System.Drawing.Color.White;
		this.RadFreightVoucherFormToOpen.Location = new System.Drawing.Point(602, 7);
		this.RadFreightVoucherFormToOpen.Name = "RadFreightVoucherFormToOpen";
		this.RadFreightVoucherFormToOpen.Size = new System.Drawing.Size(14, 13);
		this.RadFreightVoucherFormToOpen.TabIndex = 49;
		this.RadFreightVoucherFormToOpen.UseVisualStyleBackColor = true;
		this.chkDriverInfoForm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkDriverInfoForm.AutoSize = true;
		this.chkDriverInfoForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkDriverInfoForm.ForeColor = System.Drawing.Color.White;
		this.chkDriverInfoForm.Location = new System.Drawing.Point(1343, 3);
		this.chkDriverInfoForm.Name = "chkDriverInfoForm";
		this.chkDriverInfoForm.Size = new System.Drawing.Size(181, 20);
		this.chkDriverInfoForm.TabIndex = 47;
		this.chkDriverInfoForm.Text = "Open Driver Bio Form";
		this.chkDriverInfoForm.UseVisualStyleBackColor = true;
		this.chkDriverInfoForm.Visible = false;
		this.labelGrnForm.AutoSize = true;
		this.labelGrnForm.BackColor = System.Drawing.Color.Teal;
		this.labelGrnForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelGrnForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelGrnForm.Location = new System.Drawing.Point(538, 6);
		this.labelGrnForm.Name = "labelGrnForm";
		this.labelGrnForm.Size = new System.Drawing.Size(69, 14);
		this.labelGrnForm.TabIndex = 0;
		this.labelGrnForm.TabStop = true;
		this.labelGrnForm.Text = "Grn Form";
		this.labelGrnForm.Click += new System.EventHandler(RadGrnFormToOpenOnInsert_Click);
		this.labelWeightBridgeForm.AutoSize = true;
		this.labelWeightBridgeForm.BackColor = System.Drawing.Color.Teal;
		this.labelWeightBridgeForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelWeightBridgeForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelWeightBridgeForm.Location = new System.Drawing.Point(385, 6);
		this.labelWeightBridgeForm.Name = "labelWeightBridgeForm";
		this.labelWeightBridgeForm.Size = new System.Drawing.Size(139, 14);
		this.labelWeightBridgeForm.TabIndex = 0;
		this.labelWeightBridgeForm.TabStop = true;
		this.labelWeightBridgeForm.Text = "Weight Bridge Form";
		this.labelWeightBridgeForm.Click += new System.EventHandler(RadWBFormToOpenOnInsert_Click);
		this.labelLabAnalysisForm.AutoSize = true;
		this.labelLabAnalysisForm.BackColor = System.Drawing.Color.Teal;
		this.labelLabAnalysisForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelLabAnalysisForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelLabAnalysisForm.Location = new System.Drawing.Point(244, 6);
		this.labelLabAnalysisForm.Name = "labelLabAnalysisForm";
		this.labelLabAnalysisForm.Size = new System.Drawing.Size(128, 14);
		this.labelLabAnalysisForm.TabIndex = 0;
		this.labelLabAnalysisForm.TabStop = true;
		this.labelLabAnalysisForm.Text = "Lab Analysis Form";
		this.labelLabAnalysisForm.Click += new System.EventHandler(RadPurchaseLabFormToOpenOnInsert_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(153, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Inward Gate Pass";
		this.rdFreightCredit.AutoSize = true;
		this.rdFreightCredit.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.rdFreightCredit.Location = new System.Drawing.Point(878, 4);
		this.rdFreightCredit.Name = "rdFreightCredit";
		this.rdFreightCredit.Size = new System.Drawing.Size(96, 19);
		this.rdFreightCredit.TabIndex = 35;
		this.rdFreightCredit.TabStop = true;
		this.rdFreightCredit.Text = "Credit Freight";
		this.rdFreightCredit.UseVisualStyleBackColor = true;
		this.rdFreightCredit.Visible = false;
		this.rdFreightCash.AutoSize = true;
		this.rdFreightCash.Checked = true;
		this.rdFreightCash.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.rdFreightCash.Location = new System.Drawing.Point(788, 4);
		this.rdFreightCash.Name = "rdFreightCash";
		this.rdFreightCash.Size = new System.Drawing.Size(90, 19);
		this.rdFreightCash.TabIndex = 36;
		this.rdFreightCash.TabStop = true;
		this.rdFreightCash.Text = "Cash Freight";
		this.rdFreightCash.UseVisualStyleBackColor = true;
		this.rdFreightCash.Visible = false;
		this.RadNoneFormToOpenOnInsert.AutoSize = true;
		this.RadNoneFormToOpenOnInsert.Checked = true;
		this.RadNoneFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoneFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadNoneFormToOpenOnInsert.Location = new System.Drawing.Point(166, 4);
		this.RadNoneFormToOpenOnInsert.Name = "RadNoneFormToOpenOnInsert";
		this.RadNoneFormToOpenOnInsert.Size = new System.Drawing.Size(59, 18);
		this.RadNoneFormToOpenOnInsert.TabIndex = 32;
		this.RadNoneFormToOpenOnInsert.TabStop = true;
		this.RadNoneFormToOpenOnInsert.Text = "None";
		this.RadNoneFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGrnFormToOpenOnInsert.AutoSize = true;
		this.RadGrnFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGrnFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadGrnFormToOpenOnInsert.Location = new System.Drawing.Point(524, 7);
		this.RadGrnFormToOpenOnInsert.Name = "RadGrnFormToOpenOnInsert";
		this.RadGrnFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadGrnFormToOpenOnInsert.TabIndex = 33;
		this.RadGrnFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGrnFormToOpenOnInsert.Visible = false;
		this.RadWBFormToOpenOnInsert.AutoSize = true;
		this.RadWBFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadWBFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadWBFormToOpenOnInsert.Location = new System.Drawing.Point(372, 7);
		this.RadWBFormToOpenOnInsert.Name = "RadWBFormToOpenOnInsert";
		this.RadWBFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadWBFormToOpenOnInsert.TabIndex = 33;
		this.RadWBFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadPurchaseLabFormToOpenOnInsert.AutoSize = true;
		this.RadPurchaseLabFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadPurchaseLabFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadPurchaseLabFormToOpenOnInsert.Location = new System.Drawing.Point(229, 7);
		this.RadPurchaseLabFormToOpenOnInsert.Name = "RadPurchaseLabFormToOpenOnInsert";
		this.RadPurchaseLabFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadPurchaseLabFormToOpenOnInsert.TabIndex = 33;
		this.RadPurchaseLabFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1529, 2);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(75, 21);
		this.ChkBox.TabIndex = 35;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.ChkIsApproved.AutoSize = true;
		this.ChkIsApproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkIsApproved.Location = new System.Drawing.Point(979, 4);
		this.ChkIsApproved.Margin = new System.Windows.Forms.Padding(2);
		this.ChkIsApproved.Name = "ChkIsApproved";
		this.ChkIsApproved.Size = new System.Drawing.Size(83, 19);
		this.ChkIsApproved.TabIndex = 34;
		this.ChkIsApproved.Text = "Is Approve";
		this.ChkIsApproved.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1607, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[19]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.btnsave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btndefvehicle,
			this.btndefinecity, this.toolStripSeparator5, this.btnPrint, this.btnSlip257, this.toolStripButton4, this.toolStripSeparator6, this.BtnDriverForm, this.btnShortcutKeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1607, 27);
		this.toolStrip1.TabIndex = 58;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.ToolTipText = "Ctrl_N";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.ToolTipText = "Alt_R";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.ToolTipText = "Ctrl_U";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.ToolTipText = "Ctrl_S";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btndefvehicle.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btndefvehicle.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btndefvehicle.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndefvehicle.Name = "btndefvehicle";
		this.btndefvehicle.Size = new System.Drawing.Size(123, 24);
		this.btndefvehicle.Text = "Define Vehicle";
		this.btndefvehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btndefvehicle.Click += new System.EventHandler(btndefvehicle_Click);
		this.btndefinecity.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btndefinecity.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.btndefinecity.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndefinecity.Name = "btndefinecity";
		this.btndefinecity.Size = new System.Drawing.Size(103, 24);
		this.btndefinecity.Text = "Define City";
		this.btndefinecity.Click += new System.EventHandler(toolStripButton3_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "251-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnSlip257.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSlip257.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip257.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip257.Name = "btnSlip257";
		this.btnSlip257.Size = new System.Drawing.Size(102, 24);
		this.btnSlip257.Text = "257-GPSlip";
		this.btnSlip257.Click += new System.EventHandler(btnSlip257_Click);
		this.toolStripButton4.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(128, 24);
		this.toolStripButton4.Text = "653-LabReport";
		this.toolStripButton4.Click += new System.EventHandler(toolStripButton4_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.BtnDriverForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDriverForm.Name = "BtnDriverForm";
		this.BtnDriverForm.Size = new System.Drawing.Size(73, 24);
		this.BtnDriverForm.Text = "Driver Bio";
		this.BtnDriverForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnDriverForm.ToolTipText = "Ctrl+F1";
		this.BtnDriverForm.Click += new System.EventHandler(BtnDriverForm_Click);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1611, 708);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1607, 704);
		this.panel5.TabIndex = 0;
		this.panel8.Controls.Add(this.panel18);
		this.panel8.Controls.Add(this.panel17);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 53);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1607, 651);
		this.panel8.TabIndex = 1;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 71);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1607, 580);
		this.panel18.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1607, 580);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.FormattingRow += new RowLoadEventHandler(grdhistory_FormattingRow);
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel17.Controls.Add(this.groupBox3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1607, 71);
		this.panel17.TabIndex = 0;
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.txtToDateHistory);
		this.groupBox3.Controls.Add(this.txtFromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1092, 65);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(987, 36);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(820, 36);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(901, 36);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(748, 32);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(486, 15);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(51, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Supplier";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(380, 15);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 86;
		this.label19.Text = "To Doc No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(140, 15);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(48, 15);
		this.label21.TabIndex = 84;
		this.label21.Text = "To Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(380, 34);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(104, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(9, 15);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(63, 15);
		this.label22.TabIndex = 83;
		this.label22.Text = "From Date";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(275, 15);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(79, 15);
		this.label20.TabIndex = 85;
		this.label20.Text = "From Doc No";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(275, 34);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(104, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
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
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(486, 32);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(143, 34);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.txtFromDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateHistory.Location = new System.Drawing.Point(9, 34);
		this.txtFromDateHistory.Name = "txtFromDateHistory";
		this.txtFromDateHistory.ShowCheckBox = true;
		this.txtFromDateHistory.Size = new System.Drawing.Size(132, 23);
		this.txtFromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar5);
		this.panel7.Controls.Add(this.label17);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1607, 53);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1559, 29);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(9, 7, 9, 7);
		this.ctrlGrdBar5.MyGrid = this.grdhistory;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(44, 23);
		this.ctrlGrdBar5.TabIndex = 75;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label17.Location = new System.Drawing.Point(6, 30);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(216, 19);
		this.label17.TabIndex = 74;
		this.label17.Text = "Inward Gate Pass History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1901, 34);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(57, 30);
		this.ctrlGrdBar2.TabIndex = 73;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1607, 27);
		this.toolStrip2.TabIndex = 73;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(70, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnLoadAll.BackColor = System.Drawing.Color.White;
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.groupBox6.Controls.Add(this.btnResetDriverInfo);
		this.groupBox6.Controls.Add(this.txtFatherCNIC);
		this.groupBox6.Controls.Add(this.label65);
		this.groupBox6.Controls.Add(this.txtWhatsAppNo);
		this.groupBox6.Controls.Add(this.label70);
		this.groupBox6.Controls.Add(this.txtAlternateCellNo);
		this.groupBox6.Controls.Add(this.label71);
		this.groupBox6.Controls.Add(this.label72);
		this.groupBox6.Controls.Add(this.txtFatherName);
		this.groupBox6.Controls.Add(this.txtDriverCellNo);
		this.groupBox6.Controls.Add(this.label77);
		this.groupBox6.Controls.Add(this.txtDriverName);
		this.groupBox6.Controls.Add(this.label78);
		this.groupBox6.Controls.Add(this.txtCNIC);
		this.groupBox6.Controls.Add(this.label79);
		this.groupBox6.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox6.Location = new System.Drawing.Point(4, 305);
		this.groupBox6.Name = "groupBox6";
		this.groupBox6.Size = new System.Drawing.Size(1040, 56);
		this.groupBox6.TabIndex = 2;
		this.groupBox6.TabStop = false;
		this.groupBox6.Text = "Driver Information";
		this.btnResetDriverInfo.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnResetDriverInfo.Location = new System.Drawing.Point(114, 9);
		this.btnResetDriverInfo.Name = "btnResetDriverInfo";
		this.btnResetDriverInfo.Size = new System.Drawing.Size(20, 20);
		this.btnResetDriverInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.btnResetDriverInfo.TabIndex = 85;
		this.btnResetDriverInfo.TabStop = false;
		this.btnResetDriverInfo.Click += new System.EventHandler(btnResetDriverInfo_Click);
		this.txtFatherCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherCNIC.Location = new System.Drawing.Point(891, 29);
		this.txtFatherCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtFatherCNIC.Mask = "00000-0000000-0";
		this.txtFatherCNIC.Name = "txtFatherCNIC";
		this.txtFatherCNIC.Size = new System.Drawing.Size(139, 23);
		this.txtFatherCNIC.TabIndex = 6;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(891, 12);
		this.label65.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(70, 15);
		this.label65.TabIndex = 81;
		this.label65.Text = "Father CNIC";
		this.txtWhatsAppNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWhatsAppNo.Location = new System.Drawing.Point(266, 29);
		this.txtWhatsAppNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtWhatsAppNo.Mask = "000-000-0000000";
		this.txtWhatsAppNo.Name = "txtWhatsAppNo";
		this.txtWhatsAppNo.Size = new System.Drawing.Size(133, 23);
		this.txtWhatsAppNo.TabIndex = 2;
		this.txtWhatsAppNo.Text = "0923";
		this.txtWhatsAppNo.Click += new System.EventHandler(txtWhatsAppNo_Click);
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(267, 12);
		this.label70.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(73, 15);
		this.label70.TabIndex = 75;
		this.label70.Text = "WhatsApp #";
		this.txtAlternateCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAlternateCellNo.Location = new System.Drawing.Point(400, 29);
		this.txtAlternateCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtAlternateCellNo.Mask = "000-000-0000000";
		this.txtAlternateCellNo.Name = "txtAlternateCellNo";
		this.txtAlternateCellNo.Size = new System.Drawing.Size(132, 23);
		this.txtAlternateCellNo.TabIndex = 3;
		this.txtAlternateCellNo.Text = "0923";
		this.txtAlternateCellNo.Click += new System.EventHandler(txtAlternateCellNo_Click);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(400, 12);
		this.label71.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(84, 15);
		this.label71.TabIndex = 79;
		this.label71.Text = "Alternate Cell#";
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(734, 12);
		this.label72.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(75, 15);
		this.label72.TabIndex = 78;
		this.label72.Text = "Father Name";
		this.txtFatherName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(734, 29);
		this.txtFatherName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(156, 23);
		this.txtFatherName.TabIndex = 5;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(135, 29);
		this.txtDriverCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtDriverCellNo.Mask = "000-000-0000000";
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(130, 23);
		this.txtDriverCellNo.TabIndex = 1;
		this.txtDriverCellNo.Text = "0923";
		this.txtDriverCellNo.Click += new System.EventHandler(txtDriverCellNo_Click);
		this.txtDriverCellNo.Leave += new System.EventHandler(txtDriverCellNo_Leave);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(136, 12);
		this.label77.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(71, 15);
		this.label77.TabIndex = 75;
		this.label77.Text = "Driver Cell #";
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(533, 29);
		this.txtDriverName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(200, 23);
		this.txtDriverName.TabIndex = 4;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.Location = new System.Drawing.Point(533, 12);
		this.label78.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(74, 15);
		this.label78.TabIndex = 73;
		this.label78.Text = "Driver Name";
		this.txtCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNIC.Location = new System.Drawing.Point(5, 29);
		this.txtCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtCNIC.Mask = "00000-0000000-0";
		this.txtCNIC.Name = "txtCNIC";
		this.txtCNIC.Size = new System.Drawing.Size(129, 23);
		this.txtCNIC.TabIndex = 0;
		this.txtCNIC.Click += new System.EventHandler(txtCNIC_Click);
		this.txtCNIC.Leave += new System.EventHandler(txtCNIC_Leave);
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.Location = new System.Drawing.Point(5, 12);
		this.label79.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(41, 15);
		this.label79.TabIndex = 71;
		this.label79.Text = "CNIC#";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1619, 737);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InwardGatePass";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InwardGatePass_FormClosing);
		base.Load += new System.EventHandler(InwardGatePass_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InwardGatePass_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseOrder).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypePoInfo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierNamePoInfo).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockReceivingNotes).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.PanelBreakup.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseBrakup).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierDispatchedPreBillNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.groupBox6.ResumeLayout(false);
		this.groupBox6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).EndInit();
		base.ResumeLayout(false);
	}
}
