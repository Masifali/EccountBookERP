using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.BLL.PartyProcessing;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.PartyProcessing;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lab;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.WeightBridge;

public class WeightbridgeMannual : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private int RECID;

	private bool Approved;

	private bool UpdateMode;

	private int RefDocumentTypeId;

	private bool lstIsSavedVehicle;

	private bool WeighBridgeForOutwardPartyAndItemWise;

	private bool MultiWeightBridge;

	private bool LabCompulsoryBeforFirstWeight;

	private double weighbridgeTolerance;

	private DataTable dtitem = new DataTable();

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

	private Label label10;

	private TextBox txtItemqty;

	private Label label9;

	private Label label8;

	private TextBox txtVehicleNo;

	private Label label5;

	private TextBox txtBilityNo;

	private DateTimePicker txtSecondDate;

	private Label label3;

	private DateTimePicker txtFirstDate;

	private Label label2;

	private Label label1;

	private TextBox txtTickectNo;

	private Panel panel6;

	private Label label15;

	private Panel panel4;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	internal GridEX grdfill;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label17;

	private TextBox txtNetWeight;

	private TabPage tabPage2;

	private Label label14;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label18;

	private Label label19;

	internal UltraCombo CmbGatePassNo;

	private Panel panel7;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip2;

	private Label label20;

	private Label label22;

	private Label label21;

	private SerialPort serialPort1;

	private CheckBox ChkBox;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo cmbVehicleType;

	internal UltraCombo cmbReferenceType;

	private Label label24;

	private TextBox txtWeightDiffComments;

	private TextBox txtWorkingReportNo;

	private Label label4;

	private Label label30;

	private Label label29;

	private Label label28;

	private Label label27;

	private Label label26;

	private Label label25;

	private Label label7;

	private Label label23;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdPendingGpForWeighBridge;

	private Label label11;

	internal UltraCombo CmbItem;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbWeighBridgeType;

	private Label label31;

	private CheckBox ChkMultiItem;

	private ToolStripButton BtnWeightDefine;

	internal UltraCombo cmbVehicleNo;

	private Label label32;

	internal UltraCombo cmbPackingType;

	private ToolStripButton BtnShortCutkeys;

	private Panel panel9;

	private Label label33;

	private Panel panel10;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnSlip257;

	private ToolStripButton BtnPurchaseLab;

	private CheckBox chkPrintWithLab;

	private Label label34;

	private ToolStripButton btnWeighBridgeLookUp;

	internal UltraCombo CmbInvoiceNo;

	private Label label35;

	private Panel panel11;

	private Label label36;

	private Panel panel12;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbWbTypeHistory;

	private Label label37;

	internal UltraCombo CmbGatePassTypeHistory;

	private Label lblLanguage;

	private Button btnshow;

	private TextBox GpNoTo;

	private TextBox GPNoFrom;

	private Label label39;

	private DateTimePicker ToDate;

	private Label label40;

	private TextBox TxtVehicleNumber;

	private Label label42;

	private TextBox txtTicketNoTo;

	private Label label44;

	private TextBox txtTicketNoFrom;

	private DateTimePicker FromDate;

	private Label label38;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	internal UltraCombo CmbPartyName;

	private Label label6;

	private Label label16;

	private Label label41;

	private Label label43;

	private Label label45;

	private Label label46;

	private TextBox txtSupplierWbNetWeight;

	private TextBox txtTearWeight;

	private TextBox txtLoadWeight;

	public WeightbridgeMannual(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmWeightbridge_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(20, 20);
			tabPage1.AutoScrollMinSize = new Size(930, 700);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "Get Saved Vehicle Weight Against MoveOrder");
			lstIsSavedVehicle = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "More Than 1 Weight By 1GpNoO Against");
			MultiWeightBridge = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "WeighBridgeForOutwardPartyAndItemWise");
			WeighBridgeForOutwardPartyAndItemWise = Conversion.ToBool((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation x) => x.ConfigDescription == "Lab Compulsory Befor First Weight");
			LabCompulsoryBeforFirstWeight = Conversion.ToBool((object)((obj4 != null) ? obj4.ConfigKey : null));
			weighbridgeTolerance = Conversion.ToDouble((object)GlobalVariables_Helper.GetConfigValueFromGlobal("BillWeightAndStockWeightDifferenceTolerance"));
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			Print.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			if (dtParty.Columns.Count == 0)
			{
				dtParty.Columns.Add("Id", typeof(int));
				dtParty.Columns.Add("PartyName");
			}
			if (dtitem.Columns.Count == 0)
			{
				dtitem.Columns.Add("Id", typeof(int));
				dtitem.Columns.Add("ItemName");
				dtitem.Columns.Add("PartyId", typeof(int));
			}
			cmbReferenceType.Focus();
			VehicleTypesFill();
			getAllitems();
			GatePassTypeFill();
			WbType();
			btnUpdate.Visible = false;
			GenerateTicketNo();
			GatepassPendingForWeighBridge();
			bindGridPending();
			PackingTypeForWBFill();
			PartyNameBind();
			ChkBox.Checked = true;
			txtsecondWeight.Enabled = true;
			((Control)(object)cmbVehicleNo).Enabled = false;
			((Control)(object)cmbVehicleNo).Visible = false;
			txtVehicleNo.Enabled = true;
			txtVehicleNo.Visible = true;
			ChkMultiItem.Visible = false;
			ChkMultiItem.Checked = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GenerateTicketNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			int code = WbTransactions.GenerateCode(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchId = UserAccount.BranchesId
			});
			if (code > 0)
			{
				txtTickectNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyNameBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbPartyName.Value);
			DataTable dt = WbPartyandItemsDefine.GetAll(new WbPartyandItemsDefine
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				dtParty.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["WbType"]) == "PartyName")
					{
						dtParty.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["PartyName"]);
					}
				}
			}
			if (dtParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtParty, CmbPartyName, "Id", "PartyName", "PartyName", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtParty.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPartyName.Value = Id;
					}
					else
					{
						((Control)(object)CmbPartyName).Text = string.Empty;
					}
				}
				CmbPartyName.LimitToList = false;
			}
			else
			{
				((UltraGridBase)CmbPartyName).DataSource = null;
				((Control)(object)CmbPartyName).Text = string.Empty;
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
			int Id = Conversion.ToInt(CmbPartyName.Value);
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
			if (dtParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtParty, CmbPartyName, "Id", "PartyName", "PartyName", ZeroIndex: false);
				if (Id > 0)
				{
					if (dtParty.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPartyName.Value = Id;
					}
					else
					{
						((Control)(object)CmbPartyName).Text = string.Empty;
					}
				}
				CmbPartyName.LimitToList = true;
			}
			else
			{
				((UltraGridBase)CmbPartyName).DataSource = null;
				((Control)(object)CmbPartyName).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeForWBFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbPackingType.Value);
			DataTable dt = CommonServices.StaticColumnsService("PackingTypeForWB");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPackingType, "Id", "type", "Packing Type", ZeroIndex: false);
				if (Id > 0)
				{
					cmbPackingType.Value = Id;
				}
			}
			else
			{
				((UltraGridBase)cmbPackingType).DataSource = null;
				((Control)(object)cmbPackingType).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassOutward()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbGatePassNo).Enabled = true;
			DataTable dtout = GatePassInward.GetGatePassOutwardForMannualWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 91,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				CheckConfig = MultiWeightBridge,
				RecId = RECID
			});
			if (dtout.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpSrNo");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("GatepassType");
				for (int i = 0; i < dtout.Rows.Count; i++)
				{
					dt.Rows.Add(dtout.Rows[i]["Id"], dtout.Rows[i]["GpSrNo"], dtout.Rows[i]["VehicleNo"], dtout.Rows[i]["GatepassType"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)CmbGatePassNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassOutwardSteel()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbGatePassNo).Enabled = true;
			DataTable dtout = GatePassInward.GetGatePassOutwardForMannualWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1507,
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
					DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)CmbGatePassNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassInwardForWeighBridge()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbGatePassNo).Enabled = true;
			DataTable dt = InvLabAnalysisPurchaseHeader.GetGatePassIdandNoforWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				WeighBridgeStatus = "Mannual",
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbGatePassNo).Text = string.Empty;
			((UltraGridBase)CmbGatePassNo).DataSource = null;
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
			((Control)(object)CmbGatePassNo).Enabled = true;
			DataTable dt = InvLabAnalysisPurchaseHeader.GetGatePassIdandNoforWeighBridge_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbGatePassNo).Text = string.Empty;
			((UltraGridBase)CmbGatePassNo).DataSource = null;
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
			GetGatePassOutward();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocNoGatePassGeneral()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbGatePassNo).Enabled = true;
			if (Conversion.ToInt(cmbReferenceType.Value) != 52 || !(((Control)(object)cmbReferenceType).Text == "General GatePass Inward"))
			{
				return;
			}
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 52,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtout = new DataTable();
			dtout = GatePassInward.GetForWeighBridgeGatePassGeneral(val);
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
					DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)CmbGatePassNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefDocNoGatePassGeneralOutward()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbGatePassNo).Enabled = true;
			if (Conversion.ToInt(cmbReferenceType.Value) != 92 || !(((Control)(object)cmbReferenceType).Text == "General GatePass Outward"))
			{
				return;
			}
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 92,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtout = new DataTable();
			dtout = GatePassInward.GetForWeighBridgeGatePassGeneral(val);
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
					DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				}
			}
			else
			{
				((UltraGridBase)CmbGatePassNo).DataSource = null;
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
			((Control)(object)CmbGatePassNo).Enabled = true;
			DataTable dtout = GatePassPartyProcessing.GetGatePassPartyProcessingForWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = Conversion.ToInt(cmbReferenceType.Value),
				WeighBridgeStatus = "Manual"
			});
			if (dtout.Rows.Count > 0)
			{
				DDL.BindDDL(dtout, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbGatePassNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			else
			{
				((UltraGridBase)cmbVehicleNo).DataSource = null;
			}
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
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbVehicleType, "Id", "VehicleDescription", "VehicleDescription", false);
				((UltraGridBase)cmbVehicleType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.GatePassReferenceTypes();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbReferenceType, "Id", "Name", "GatePassType", ZeroIndex: false);
				DDL.BindDDL(dt, CmbGatePassTypeHistory, "Id", "Name", "GatePassType", ZeroIndex: false);
				((UltraGridBase)cmbReferenceType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getAllitems()
	{
		try
		{
			int ID = Conversion.ToInt(CmbItem.Value);
			DataTable dt = CommonServices.ReadAllItemsWithPackingAndStore();
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
			throw ex;
		}
	}

	private DataTable LabItemsBindByGpId(int ActionId = 0)
	{
		DataTable dtLabInfo = new DataTable();
		try
		{
			dtLabInfo = GatePassInward.getItemsFromLabOrPurchaseOrderOrAllByGpId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbGatePassNo.Value), ActionId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dtLabInfo;
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
						dtitem.Rows.Add(r["ItemId"], r["ItemName"], SupplierCustomerId);
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

	private void ItemsFromDeliveryOrder(int DeliveryOrderId)
	{
		try
		{
			int ID = Conversion.ToInt(CmbItem.Value);
			DataTable dt = InvDeliveryOrder.GetItemsByDeliveryOrderId(UserAccount.OrganizationId, UserAccount.CompanyId, DeliveryOrderId, 0);
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

	private void WbType()
	{
		try
		{
			DataTable dt = CommonServices.GatePassTypeFill("WeighBridge");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbWeighBridgeType, "Id", "GpTypeDescription", "WeighBridge Type", ZeroIndex: false);
				DDL.BindDDL(dt, CmbWbTypeHistory, "Id", "GpTypeDescription", "WeighBridge Type", ZeroIndex: false);
			}
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
			if (((Control)(object)cmbReferenceType).Text.Trim() == "GatePass OutWard")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				RefDocNoGatePassOutward();
			}
			else if (((Control)(object)cmbReferenceType).Text.Trim() == "GatePass OutWard Steel")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				GetGatePassOutwardSteel();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 51 && ((Control)(object)cmbReferenceType).Text.Trim() == "GatePass Inward")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				GetGatePassInwardForWeighBridge();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 1601 && ((Control)(object)cmbReferenceType).Text.Trim() == "GatePass Inward")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				GetGatePassInwardForWeighBridge_Engr();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 52 && ((Control)(object)cmbReferenceType).Text == "General GatePass Inward")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				RefDocNoGatePassGeneral();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 92 && ((Control)(object)cmbReferenceType).Text == "General GatePass Outward")
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				RefDocNoGatePassGeneralOutward();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 74 || Conversion.ToInt(cmbReferenceType.Value) == 75 || Conversion.ToInt(cmbReferenceType.Value) == 218)
			{
				FieldsEnables();
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpSrNo");
				dt.Rows.Add(txtTickectNo.Text, txtTickectNo.Text);
				DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				((UltraGridBase)CmbGatePassNo).Rows[0].Activate();
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 54 || Conversion.ToInt(cmbReferenceType.Value) == 55)
			{
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				GatePassPartyProcessing();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbReferenceType_Leave(object sender, EventArgs e)
	{
		try
		{
			GatePassBending();
			if (lstIsSavedVehicle)
			{
				if (Conversion.ToInt(cmbReferenceType.Value) == 74 || Conversion.ToInt(cmbReferenceType.Value) == 218)
				{
					GetSavedVehicles();
					return;
				}
				((Control)(object)cmbVehicleNo).Enabled = false;
				((Control)(object)cmbVehicleNo).Visible = false;
				txtVehicleNo.Enabled = true;
				txtVehicleNo.Visible = true;
			}
			else
			{
				txtsecondWeight.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbVehicleNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Get Saved Vehicle Weight Against MoveOrder").ToList().Count > 0 && ((Control)(object)cmbVehicleNo).Enabled && ((UltraGridBase)cmbVehicleNo).ActiveRow != null)
			{
				txtsecondWeight.Text = Conversion.ToString(((UltraDropDownBase)cmbVehicleNo).SelectedRow.Cells[3].Value);
				txtsecondWeight.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRefDocNo_Leave(object sender, EventArgs e)
	{
		try
		{
			BindingAgainstGatePass();
		}
		catch (Exception ex)
		{
			ClearFormFields();
			MessageBox.Show(ex.Message);
		}
	}

	private void BindingAgainstGatePass()
	{
		//IL_0a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Expected O, but got Unknown
		//IL_0c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Expected O, but got Unknown
		try
		{
			label6.Visible = false;
			label16.Visible = false;
			label41.Visible = false;
			((Control)(object)CmbItem).Enabled = true;
			((Control)(object)CmbPartyName).Enabled = true;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNo).DataSource = null;
			dtPartiesAndItemsFromDeliveryOrder.Clear();
			if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard")
			{
				FieldsDisables();
				DataTable dtoutwardsrNo = CommonServices.GetByGpNo(91, Conversion.ToInt(CmbGatePassNo.Value));
				if (dtoutwardsrNo == null || dtoutwardsrNo.Rows.Count <= 0)
				{
					return;
				}
				txtBilityNo.Text = dtoutwardsrNo.Rows[0]["BiltyNo"].ToString();
				txtVehicleNo.Text = dtoutwardsrNo.Rows[0]["VehicleNo"].ToString();
				((Control)(object)cmbVehicleType).Text = dtoutwardsrNo.Rows[0]["VehicleType"].ToString();
				txtItemqty.Text = dtoutwardsrNo.Rows[0]["NoOfPackages"].ToString();
				((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo.Rows[0]["GatepassType"]);
				string OrderType = Conversion.ToString(dtoutwardsrNo.Rows[0]["OtherSupCust"]);
				int OrderId = Conversion.ToInt(dtoutwardsrNo.Rows[0]["SaleOrderId"]);
				switch (OrderType)
				{
				case "DeliverOrder":
					if (((UltraGridBase)CmbGatePassNo).ActiveRow != null && Conversion.ToInt(CmbGatePassNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)CmbGatePassNo).SelectedRow.Cells["GatepassType"].Value) == "Export")
					{
						getInvoiceNoFromDeliveryOrderByGpId(Conversion.ToInt(CmbGatePassNo.Value));
					}
					if (WeighBridgeForOutwardPartyAndItemWise && ((UltraGridBase)CmbGatePassNo).ActiveRow != null && Conversion.ToInt(CmbGatePassNo.Value) > 0 && Conversion.ToString(((UltraDropDownBase)CmbGatePassNo).SelectedRow.Cells["GatepassType"].Value) != "Export")
					{
						dtPartiesAndItemsFromDeliveryOrder = InvDeliveryOrder.GetPartiesAndItemsFromDeliveryOrderByGpId(Conversion.ToInt(CmbGatePassNo.Value), RECID);
						PartyNameBindforOutward();
						((UltraGridBase)CmbItem).DataSource = null;
						((Control)(object)CmbItem).Text = "";
						CmbPartyName_Leave(null, null);
					}
					else
					{
						ItemsFromDeliveryOrder(OrderId);
						((Control)(object)CmbItem).Text = dtoutwardsrNo.Rows[0]["VarietyName"].ToString();
					}
					break;
				case "SaleOrder":
				case "SaleOrderStoreAndPm":
					ItemsFromSaleOrder(OrderId);
					break;
				default:
					PartyNameBind();
					getAllitems();
					break;
				}
				((Control)(object)CmbItem).Text = ((OrderType != "DeliverOrder") ? dtoutwardsrNo.Rows[0]["VarietyName"].ToString() : "");
				((Control)(object)CmbPartyName).Text = dtoutwardsrNo.Rows[0]["CompanyName"].ToString();
				((Control)(object)CmbItem).Enabled = true;
				((Control)(object)CmbPartyName).Enabled = WeighBridgeForOutwardPartyAndItemWise;
				if (WeighBridgeForOutwardPartyAndItemWise && OrderType == "DeliverOrder" && Conversion.ToInt(CmbPartyName.Value) > 0)
				{
					ItemsAgainstPartyForOutward(Conversion.ToInt(CmbPartyName.Value));
				}
			}
			else if (((Control)(object)cmbReferenceType).Text == "GatePass OutWard Steel")
			{
				FieldsDisables();
				DataTable dtoutwardsrNo2 = CommonServices.GetByGpNo(1507, Conversion.ToInt(CmbGatePassNo.Value));
				if (dtoutwardsrNo2.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo2.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo2.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo2.Rows[0]["VehicleType"].ToString();
					txtItemqty.Text = dtoutwardsrNo2.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo2.Rows[0]["VarietyName"].ToString();
					((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo2.Rows[0]["GatepassType"]);
					PartyNameBind();
					getAllitems();
					((Control)(object)CmbItem).Enabled = false;
				}
			}
			else if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				label6.Visible = true;
				label16.Visible = true;
				label41.Visible = true;
				PartyNameBind();
				getAllitems();
				if (Conversion.ToInt(cmbReferenceType.Value) == 1601)
				{
					FieldsDisables();
					DataTable dtoutwardsrNo3 = CommonServices.GetByGpNoInward(1601, Conversion.ToInt(CmbGatePassNo.Value));
					if (dtoutwardsrNo3.Rows.Count > 0)
					{
						txtBilityNo.Text = dtoutwardsrNo3.Rows[0]["BiltyNo"].ToString();
						txtVehicleNo.Text = dtoutwardsrNo3.Rows[0]["VehicleNo"].ToString();
						((Control)(object)cmbVehicleType).Text = dtoutwardsrNo3.Rows[0]["VehicleType"].ToString();
						txtItemqty.Text = dtoutwardsrNo3.Rows[0]["ItemQty"].ToString();
						CmbItem.Value = Conversion.ToInt(dtoutwardsrNo3.Rows[0]["ItemId"]);
						RefDocumentTypeId = Conversion.ToInt(dtoutwardsrNo3.Rows[0]["RefDocumentTypeId"]);
						((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo3.Rows[0]["GatepassType"]);
					}
					return;
				}
				FieldsDisables();
				DataTable dtoutwardsrNo4 = CommonServices.GetByGpNoInward(51, Conversion.ToInt(CmbGatePassNo.Value));
				if (dtoutwardsrNo4.Rows.Count <= 0)
				{
					return;
				}
				txtBilityNo.Text = dtoutwardsrNo4.Rows[0]["BiltyNo"].ToString();
				txtVehicleNo.Text = dtoutwardsrNo4.Rows[0]["VehicleNo"].ToString();
				((Control)(object)cmbVehicleType).Text = dtoutwardsrNo4.Rows[0]["VehicleType"].ToString();
				txtItemqty.Text = dtoutwardsrNo4.Rows[0]["NoOfPackages"].ToString();
				if (Conversion.ToInt(dtoutwardsrNo4.Rows[0]["ItemId"]) == 0)
				{
					((Control)(object)CmbItem).Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["VarietyName"]);
				}
				else
				{
					CmbItem.Value = Conversion.ToInt(dtoutwardsrNo4.Rows[0]["ItemId"]);
				}
				((Control)(object)CmbPartyName).Text = dtoutwardsrNo4.Rows[0]["CompanyName"].ToString();
				RefDocumentTypeId = Conversion.ToInt(dtoutwardsrNo4.Rows[0]["RefDocumentTypeId"]);
				((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["GatepassType"]);
				cmbPackingType.Value = Conversion.ToInt(dtoutwardsrNo4.Rows[0]["PackingTypeId"]);
				int gpentryUserId = Conversion.ToInt(dtoutwardsrNo4.Rows[0]["EntryUser"]);
				if (UserAccount.ID == gpentryUserId)
				{
					txtLoadWeight.Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["SupplierFirstWeight"]);
					txtTearWeight.Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["SupplierSecondWeight"]);
					txtSupplierWbNetWeight.Text = Conversion.ToString(dtoutwardsrNo4.Rows[0]["SupplierWeight"]);
				}
				else
				{
					txtLoadWeight.Text = "0";
					txtTearWeight.Text = "0";
					txtSupplierWbNetWeight.Text = "0";
				}
				((Control)(object)CmbPartyName).Enabled = false;
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
			else if (Conversion.ToInt(cmbReferenceType.Value) == 52)
			{
				getAllitems();
				FieldsDisables();
				DataTable dtoutwardsrNo5 = GatePassGeneral.GatePassGeneralReadByGpNo(new GatePassGeneral
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = 52,
					GpSrNo = Conversion.ToInt((object)((Control)(object)CmbGatePassNo).Text.Trim()),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dtoutwardsrNo5.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo5.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo5.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo5.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo5.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo5.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo5.Rows[0]["VarietyName"].ToString();
				}
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 92)
			{
				getAllitems();
				FieldsDisables();
				DataTable dtoutwardsrNo6 = GatePassGeneral.GatePassGeneralReadByGpNo(new GatePassGeneral
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 92,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					GpSrNo = Conversion.ToInt((object)((Control)(object)CmbGatePassNo).Text.Trim()),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dtoutwardsrNo6.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo6.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo6.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo6.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo6.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo6.Rows[0]["NoOfPackages"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo6.Rows[0]["VarietyName"].ToString();
				}
			}
			else if (Conversion.ToInt(cmbReferenceType.Value) == 54 || Conversion.ToInt(cmbReferenceType.Value) == 55)
			{
				getAllitems();
				FieldsDisables();
				DataTable dtoutwardsrNo7 = GatePassPartyProcessing.ReadByGpNoForWeighBridge(new GatePassPartyProcessing
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = Conversion.ToInt(cmbReferenceType.Value),
					Id = Conversion.ToInt(CmbGatePassNo.Value)
				});
				if (dtoutwardsrNo7.Rows.Count > 0)
				{
					txtBilityNo.Text = dtoutwardsrNo7.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtoutwardsrNo7.Rows[0]["VehicleNo"].ToString();
					((Control)(object)cmbVehicleType).Text = dtoutwardsrNo7.Rows[0]["VehicleType"].ToString();
					txtRemarks.Text = dtoutwardsrNo7.Rows[0]["OtherRemarks"].ToString();
					txtItemqty.Text = dtoutwardsrNo7.Rows[0]["ItemQty"].ToString();
					((Control)(object)CmbItem).Text = dtoutwardsrNo7.Rows[0]["VarietyName"].ToString();
					((Control)(object)CmbPartyName).Text = dtoutwardsrNo7.Rows[0]["CompanyName"].ToString();
					((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtoutwardsrNo7.Rows[0]["GatepassType"]);
				}
			}
			else if (((Control)(object)cmbReferenceType).Text == string.Empty)
			{
				throw new Exception("Please Select Gate pass Type");
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void ClearFormFields()
	{
		txtBilityNo.Text = "";
		txtVehicleNo.Text = "";
		((Control)(object)cmbVehicleType).Text = "";
		txtItemqty.Text = "";
		((Control)(object)CmbItem).Text = "";
		RefDocumentTypeId = 0;
		((Control)(object)CmbWeighBridgeType).Text = "";
	}

	private void cmbReferenceType_SelectionChangeCommitted(object sender, EventArgs e)
	{
		try
		{
			cmbReferenceType_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
		txtVehicleNo.SelectionStart = txtVehicleNo.Text.Length;
		txtVehicleNo.Text = txtVehicleNo.Text.ToUpper();
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
				getAllitems();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		try
		{
			int referenceTypeValue = Conversion.ToInt(cmbReferenceType.Value);
			string gatePassType = string.Empty;
			if (((UltraDropDownBase)CmbGatePassNo).SelectedRow != null && ((KeyedSubObjectsCollectionBase)((UltraDropDownBase)CmbGatePassNo).SelectedRow.Cells).Exists("GatepassType"))
			{
				object cellValue = ((UltraDropDownBase)CmbGatePassNo).SelectedRow.Cells["GatepassType"].Value;
				gatePassType = ((cellValue != null) ? Conversion.ToString(cellValue) : string.Empty);
			}
			if (((UltraGridBase)cmbReferenceType).ActiveRow == null)
			{
				MessageBox.Show("GatePassType No Field is Required");
				cmbReferenceType.Focus();
				return false;
			}
			if (((UltraGridBase)CmbGatePassNo).ActiveRow == null)
			{
				MessageBox.Show("GatePass No Field is Required");
				CmbGatePassNo.Focus();
				return false;
			}
			if (((UltraGridBase)CmbWeighBridgeType).ActiveRow == null)
			{
				MessageBox.Show("WeighBridge Type Field is Required");
				CmbWeighBridgeType.Focus();
				return false;
			}
			if (referenceTypeValue == 91 && ((UltraGridBase)CmbGatePassNo).ActiveRow != null && Conversion.ToInt(CmbGatePassNo.Value) > 0 && gatePassType == "Export" && (((UltraGridBase)CmbInvoiceNo).ActiveRow == null || Conversion.ToInt(CmbInvoiceNo.Value) == 0))
			{
				MessageBox.Show("Invoice number Field is Required");
				CmbInvoiceNo.Focus();
				return false;
			}
			if ((referenceTypeValue == 74 || referenceTypeValue == 75) && (((UltraGridBase)cmbPackingType).ActiveRow == null || Conversion.ToInt(cmbPackingType.Value) == 0))
			{
				MessageBox.Show("PackingType Field is Required");
				cmbPackingType.Focus();
				return false;
			}
			if (referenceTypeValue == 51 && (txtLoadWeight.Text == "" || Conversion.ToDouble((object)txtLoadWeight.Text) == 0.0))
			{
				MessageBox.Show("Load Weight Field is Required");
				txtLoadWeight.Focus();
				return false;
			}
			string firstWeight = txtFirstWeight.Text.Trim();
			if (string.IsNullOrEmpty(firstWeight) || Conversion.ToDouble((object)firstWeight) == 0.0)
			{
				MessageBox.Show("First Weight Field is Required");
				txtFirstWeight.Focus();
				return false;
			}
			if (btnUpdate.Visible && btnUpdate.Enabled)
			{
				string secondWeight = txtsecondWeight.Text.Trim();
				if (string.IsNullOrEmpty(secondWeight) || Conversion.ToDouble((object)secondWeight) == 0.0)
				{
					MessageBox.Show("Second Weight Field is Required");
					txtsecondWeight.Focus();
					return false;
				}
				if (referenceTypeValue == 51)
				{
					string tareWeight = txtTearWeight.Text.Trim();
					if (string.IsNullOrEmpty(tareWeight) || Conversion.ToDouble((object)tareWeight) == 0.0)
					{
						MessageBox.Show("Tare Weight Field is Required");
						txtTearWeight.Focus();
						return false;
					}
					string SupplierNet = txtSupplierWbNetWeight.Text.Trim();
					if (string.IsNullOrEmpty(SupplierNet) || Conversion.ToDouble((object)SupplierNet) == 0.0)
					{
						MessageBox.Show("Supplier Net Weight Field is Required");
						txtSupplierWbNetWeight.Focus();
						return false;
					}
				}
				switch (referenceTypeValue)
				{
				case 52:
				case 74:
				case 92:
					if (string.IsNullOrEmpty(txtWorkingReportNo.Text.Trim()) || txtWorkingReportNo.Text.Trim() == "0")
					{
						txtWorkingReportNo.Enabled = true;
						MessageBox.Show("WorkingReportNo Field is Required");
						txtWorkingReportNo.Focus();
						return false;
					}
					break;
				case 91:
					if (string.IsNullOrEmpty(txtWorkingReportNo.Text.Trim()) || txtWorkingReportNo.Text.Trim() == "0")
					{
						txtWorkingReportNo.Enabled = true;
						if (GatePassOutward.GetDeliveryOrderType(new GatePassOutward
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							Id = Conversion.ToInt(CmbGatePassNo.Value)
						}) == "StockTransfer")
						{
							MessageBox.Show("WorkingReportNo Field is Required");
							txtWorkingReportNo.Focus();
							return false;
						}
					}
					break;
				}
			}
			if (string.IsNullOrEmpty(txtTickectNo.Text.Trim()) || txtTickectNo.Text.Trim() == "0")
			{
				MessageBox.Show("Ticket No Field is Required");
				txtTickectNo.Focus();
				return false;
			}
			if (string.IsNullOrWhiteSpace(txtVehicleNo.Text))
			{
				MessageBox.Show("Vehicle Number Field is Required");
				txtVehicleNo.Focus();
				return false;
			}
			if (WeighBridgeForOutwardPartyAndItemWise && ((Control)(object)cmbReferenceType).Text == "GatePass OutWard" && (((UltraGridBase)CmbPartyName).ActiveRow == null || Conversion.ToInt(CmbPartyName.Value) == 0) && gatePassType != "Export" && gatePassType != "General")
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
			if (WeighBridgeForOutwardPartyAndItemWise && ((Control)(object)cmbReferenceType).Text == "GatePass OutWard" && Conversion.ToInt(CmbPartyName.Value) != Conversion.ToInt(((UltraDropDownBase)CmbItem).SelectedRow.Cells["PartyId"].Value) && gatePassType != "Export" && gatePassType != "General")
			{
				((Control)(object)CmbPartyName).Text = "";
				MessageBox.Show("PartyName Field is Required");
				CmbPartyName.Focus();
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			WbTransactions wb = new WbTransactions();
			wb.TicketNo = Conversion.ToInt((object)txtTickectNo.Text);
			wb.BiltyNo = Conversion.ToString((object)txtBilityNo.Text);
			wb.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			wb.WbCharges = Conversion.ToSingle((object)txtWbCharges.Text);
			if (Conversion.ToInt(cmbReferenceType.Value) == 74 && lstIsSavedVehicle)
			{
				wb.VehicleNo = Conversion.ToString((object)((Control)(object)cmbVehicleNo).Text);
			}
			else
			{
				wb.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text);
			}
			wb.VehicleType = Conversion.ToString(cmbVehicleType.Value);
			wb.WeighBridgeType = Conversion.ToString((object)((Control)(object)CmbWeighBridgeType).Text);
			wb.PackingType = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
			wb.ItemQty = Conversion.ToInt((object)txtItemqty.Text);
			wb.ItemId = Conversion.ToInt(CmbItem.Value);
			wb.ItemDescription = Conversion.ToString((object)((Control)(object)CmbItem).Text);
			wb.PartyName = Conversion.ToString((object)((Control)(object)CmbPartyName).Text);
			wb.WbRemarks = Conversion.ToString((object)txtRemarks.Text);
			wb.FirstWeight = Conversion.ToInt((object)txtFirstWeight.Text);
			wb.LoadWeight = Conversion.ToDouble((object)txtLoadWeight.Text);
			wb.FirstDateTime = Conversion.ToDateTime((object)txtFirstDate.Value);
			wb.SecondDateTime = Conversion.ToDateTime((object)txtSecondDate.Value);
			wb.ReferenceDocTypeId = Conversion.ToInt(cmbReferenceType.Value);
			wb.ReferenceDocNoId = Conversion.ToInt(CmbGatePassNo.Value);
			wb.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text.Trim());
			wb.DocTypeId = 113;
			wb.DocDate = DateTime.Now;
			wb.EntryDate = DateTime.Now;
			wb.ModifyDate = DateTime.Now;
			wb.EntryUser = UserAccount.ID;
			wb.ModifyUser = UserAccount.ID;
			wb.OrganizationId = UserAccount.OrganizationId;
			wb.CompanyId = UserAccount.CompanyId;
			wb.BranchId = UserAccount.BranchesId;
			wb.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (ChkMultiItem.Checked)
			{
				wb.MultiItem = true;
			}
			else
			{
				wb.MultiItem = false;
			}
			int success = WbTransactions.Save(wb);
			if (success > 0)
			{
				MessageBox.Show("Record Save Successfully" + wb.TicketNo);
				int GPID = Conversion.ToInt(CmbGatePassNo.Value);
				formRefresh();
				if (ChkBox.Checked)
				{
					CommonServices.WbTransactionSlip280(success);
				}
				if (chkPrintWithLab.Checked)
				{
					CommonServices.InwardGatePassWithWbAndLabSlip(GPID);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update because record has approve");
			}
			if (!FormValidation())
			{
				return;
			}
			if (((Control)(object)cmbReferenceType).Text == "GatePass Inward")
			{
				double wbnetweight = Conversion.ToDouble((object)txtNetWeight.Text);
				double wbsupplierweight = Conversion.ToDouble((object)txtSupplierWbNetWeight.Text);
				if (Math.Abs(wbnetweight - wbsupplierweight) > weighbridgeTolerance && string.IsNullOrWhiteSpace(txtWeightDiffComments.Text.Trim()))
				{
					MessageBox.Show("Supplier Net Weight in Weigh Bridge does not match the Weighbridge Net Weight.\n" + $"Supplier Net Weight: {wbsupplierweight}\n" + $"Weighbridge Net Weight: {wbnetweight}\n\n" + "Difference Weight Remarks are required", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
			}
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				txtsecondWeight.Focus();
				WbTransactions wb = new WbTransactions();
				wb.Id = RECID;
				wb.TicketNo = Conversion.ToInt((object)txtTickectNo.Text.Trim());
				wb.BiltyNo = Conversion.ToString((object)txtBilityNo.Text.Trim());
				wb.PartyName = Conversion.ToString((object)((Control)(object)CmbPartyName).Text.Trim());
				wb.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
				wb.WbCharges = Conversion.ToInt((object)txtWbCharges.Text.Trim());
				wb.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
				wb.VehicleType = Conversion.ToString(cmbVehicleType.Value);
				wb.WeighBridgeType = Conversion.ToString((object)((Control)(object)CmbWeighBridgeType).Text);
				wb.ItemQty = Conversion.ToInt((object)txtItemqty.Text.Trim());
				wb.ItemId = Conversion.ToInt(CmbItem.Value);
				wb.ItemDescription = Conversion.ToString((object)((Control)(object)CmbItem).Text.Trim());
				wb.PackingType = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
				wb.WbRemarks = Conversion.ToString((object)txtRemarks.Text.Trim());
				wb.SecondWeight = Conversion.ToInt((object)txtsecondWeight.Text.Trim());
				wb.NetWbWeight = Conversion.ToInt((object)txtNetWeight.Text.Trim());
				wb.SecondDateTime = Conversion.ToDateTime((object)txtSecondDate.Value);
				wb.FirstDateTime = Conversion.ToDateTime((object)txtFirstDate.Value);
				wb.FirstWeight = Conversion.ToInt((object)txtFirstWeight.Text.Trim());
				wb.LoadWeight = Conversion.ToDouble((object)txtLoadWeight.Text.Trim());
				wb.TearWeight = Conversion.ToDouble((object)txtTearWeight.Text.Trim());
				wb.SupplierWbNetWeight = Conversion.ToDouble((object)txtSupplierWbNetWeight.Text.Trim());
				wb.IsApproved = true;
				wb.ApprovedDateTime = Conversion.ToDateTime((object)DateTime.Now);
				wb.ApprovedUserId = UserAccount.ID;
				wb.ReferenceDocTypeId = Conversion.ToInt(cmbReferenceType.Value);
				wb.ReferenceDocNoId = Conversion.ToInt(CmbGatePassNo.Value);
				wb.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
				wb.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text.Trim());
				wb.WeightDiffComments = txtWeightDiffComments.Text.Trim();
				wb.DocTypeId = 113;
				wb.DocDate = Conversion.ToDateTime((object)DateTime.Now);
				wb.EntryDate = DateTime.Now;
				wb.ModifyDate = DateTime.Now;
				wb.EntryUser = UserAccount.ID;
				wb.ModifyUser = UserAccount.ID;
				wb.OrganizationId = UserAccount.OrganizationId;
				wb.CompanyId = UserAccount.CompanyId;
				wb.BranchId = UserAccount.BranchesId;
				wb.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				if (ChkMultiItem.Checked)
				{
					wb.MultiItem = true;
				}
				else
				{
					wb.MultiItem = false;
				}
				int success = WbTransactions.Save(wb);
				if (success > 0)
				{
					MessageBox.Show("Record Update Successfully" + wb.TicketNo);
				}
				int GPId = Conversion.ToInt(CmbGatePassNo.Value);
				formRefresh();
				if (ChkBox.Checked)
				{
					CommonServices.WbTransactionSlip280(success);
				}
				if (chkPrintWithLab.Checked)
				{
					CommonServices.InwardGatePassWithWbAndLabSlip(GPId);
				}
			}
		}
		catch (Exception ex)
		{
			txtWeightDiffComments.Focus();
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			ChkMultiItem.Visible = true;
			DataTable dtgetbyId = new DataTable();
			RECID = Conversion.ToInt((object)ID);
			dtgetbyId = WbTransactions.GetByID(RECID);
			if (dtgetbyId.Rows.Count <= 0)
			{
				return;
			}
			txtTickectNo.Enabled = true;
			tabControl1.SelectedIndex = 0;
			txtTickectNo.Text = dtgetbyId.Rows[0]["TicketNo"].ToString();
			RefDocumentTypeId = Conversion.ToInt(dtgetbyId.Rows[0]["RefDocumentTypeId"]);
			int reftypeid = Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocTypeId"]);
			((Control)(object)CmbGatePassNo).Enabled = false;
			if (reftypeid == 91)
			{
				cmbReferenceType.Value = reftypeid;
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("Id");
				dt3.Columns.Add("GpSrNo");
				dt3.Columns.Add("VehicleNo");
				dt3.Columns.Add("GatepassType");
				dt3.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToString(dtgetbyId.Rows[0]["VehicleNo"]), Conversion.ToString(dtgetbyId.Rows[0]["GatepassType"]));
				DDL.BindDDL(dt3, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				txtWeightDiffComments.Text = Conversion.ToString(dtgetbyId.Rows[0]["WeightDiffComments"]);
				((UltraGridBase)CmbGatePassNo).Rows[0].Activate();
				if (Conversion.ToInt(CmbGatePassNo.Value) > 0 && Conversion.ToString(dtgetbyId.Rows[0]["GatepassType"]) == "Export")
				{
					getInvoiceNoFromDeliveryOrderByGpId(Conversion.ToInt(CmbGatePassNo.Value));
				}
				if (WeighBridgeForOutwardPartyAndItemWise && Conversion.ToInt(CmbGatePassNo.Value) > 0 && Conversion.ToString(dtgetbyId.Rows[0]["GatepassType"]) != "Export")
				{
					dtPartiesAndItemsFromDeliveryOrder = InvDeliveryOrder.GetPartiesAndItemsFromDeliveryOrderByGpId(Conversion.ToInt(CmbGatePassNo.Value), RECID);
					PartyNameBindforOutward();
					ChkMultiItem.Visible = false;
					CmbPartyName_Leave(null, null);
				}
			}
			if (reftypeid == 51 || reftypeid == 52 || reftypeid == 92 || reftypeid == 54 || reftypeid == 55 || reftypeid == 1507 || reftypeid == 1601)
			{
				cmbReferenceType.Value = reftypeid;
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("GpSrNo");
				dt2.Columns.Add("VehicleNo");
				dt2.Rows.Add(Conversion.ToInt(dtgetbyId.Rows[0]["ReferenceDocNoId"]), Conversion.ToInt(dtgetbyId.Rows[0]["GpSrNo"]), Conversion.ToInt(dtgetbyId.Rows[0]["VehicleNo"]));
				DDL.BindDDL(dt2, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				((UltraGridBase)CmbGatePassNo).Rows[0].Activate();
			}
			if (reftypeid == 74 || reftypeid == 75)
			{
				cmbReferenceType.Value = reftypeid;
				((Control)(object)CmbGatePassNo).Text = string.Empty;
				((Control)(object)CmbGatePassNo).Visible = true;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("GpSrNo");
				dt.Rows.Add(txtTickectNo.Text, txtTickectNo.Text);
				DDL.BindDDL(dt, CmbGatePassNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				((UltraGridBase)CmbGatePassNo).Rows[0].Activate();
			}
			cmbReferenceType.Value = reftypeid;
			txtFirstWeight.Text = dtgetbyId.Rows[0]["FirstWeight"].ToString();
			txtFirstDate.Value = Conversion.ToDateTime(dtgetbyId.Rows[0]["FirstDateTime"]);
			txtsecondWeight.Text = dtgetbyId.Rows[0]["SecondWeight"].ToString();
			txtNetWeight.Text = dtgetbyId.Rows[0]["NetWbWeight"].ToString();
			txtWorkingReportNo.Text = Conversion.ToString(dtgetbyId.Rows[0]["WorkingReportNo"]);
			if (txtNetWeight.Text == string.Empty || txtNetWeight.Text == null)
			{
				CalculateNetWeight();
			}
			if (txtFirstWeight.Text != string.Empty && Conversion.ToInt((object)txtsecondWeight.Text.ToString()) > 0)
			{
				txtBilityNo.Enabled = false;
				txtVehicleNo.Enabled = false;
				((Control)(object)CmbGatePassNo).Enabled = false;
			}
			else
			{
				txtsecondWeight.Enabled = true;
			}
			txtBilityNo.Text = dtgetbyId.Rows[0]["BiltyNo"].ToString();
			txtVehicleNo.Text = dtgetbyId.Rows[0]["VehicleNo"].ToString();
			((Control)(object)CmbPartyName).Text = dtgetbyId.Rows[0]["PartyName"].ToString();
			cmbVehicleType.Value = Conversion.ToInt(dtgetbyId.Rows[0]["VehicleType"]);
			((Control)(object)CmbWeighBridgeType).Text = Conversion.ToString(dtgetbyId.Rows[0]["WeighBridgeType"]);
			((Control)(object)cmbPackingType).Text = Conversion.ToString(dtgetbyId.Rows[0]["PackingType"]);
			txtWbCharges.Text = dtgetbyId.Rows[0]["WbCharges"].ToString();
			txtLoadWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["LoadWeight"]).ToString();
			txtTearWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["TearWeight"]).ToString();
			txtSupplierWbNetWeight.Text = Conversion.ToDouble(dtgetbyId.Rows[0]["SupplierWbNetWeight"]).ToString();
			txtItemqty.Text = dtgetbyId.Rows[0]["ItemQty"].ToString();
			txtRemarks.Text = dtgetbyId.Rows[0]["WbRemarks"].ToString();
			Approved = Conversion.ToBool(dtgetbyId.Rows[0]["IsApproved"]);
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
				else
				{
					CmbItem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]);
				}
			}
			else
			{
				CmbItem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]);
			}
			txtsecondWeight.Focus();
			btnsave.Visible = false;
			btnUpdate.Visible = true;
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
			formRefresh();
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
			Approved = false;
			txtsecondWeight.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			((Control)(object)CmbWeighBridgeType).Text = string.Empty;
			txtFirstWeight.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			txtItemqty.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtWbCharges.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbInvoiceNo).DataSource = null;
			((Control)(object)CmbGatePassNo).Visible = true;
			GenerateTicketNo();
			bindGridPending();
			GetGatePassOutward();
			GenerateTicketNo();
			GatepassPendingForWeighBridge();
			PartyNameBind();
			getAllitems();
			((Control)(object)CmbGatePassNo).Text = string.Empty;
			txtSecondDate.Value = DateTime.Now;
			txtFirstDate.Value = DateTime.Now;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			cmbReferenceType.Focus();
			label16.Visible = false;
			label6.Visible = false;
			label41.Visible = false;
			txtLoadWeight.Text = string.Empty;
			txtTearWeight.Text = string.Empty;
			txtSupplierWbNetWeight.Text = string.Empty;
			txtsecondWeight.Enabled = true;
			((Control)(object)cmbVehicleNo).Enabled = false;
			((Control)(object)cmbVehicleNo).Visible = false;
			txtVehicleNo.Enabled = true;
			txtVehicleNo.Visible = true;
			ChkMultiItem.Visible = false;
			ChkMultiItem.Checked = false;
			((Control)(object)CmbPartyName).Enabled = true;
			ChkMultiItem.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void FieldsDisables()
	{
		try
		{
			txtBilityNo.Enabled = false;
			txtVehicleNo.Enabled = false;
			((Control)(object)cmbVehicleType).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FieldsEnables()
	{
		try
		{
			txtBilityNo.Enabled = true;
			txtVehicleNo.Enabled = true;
			((Control)(object)cmbVehicleType).Enabled = true;
			txtItemqty.Enabled = true;
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
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "Get Saved Vehicle Weight Against MoveOrder");
			lstIsSavedVehicle = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "WeighBridgeForOutwardPartyAndItemWise");
			WeighBridgeForOutwardPartyAndItemWise = Conversion.ToBool((object)((obj2 != null) ? obj2.ConfigKey : null));
			PackingTypeForWBFill();
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
				PartyNameBind();
				getAllitems();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightbridgeMannual_FormClosing(object sender, FormClosingEventArgs e)
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

	private void bindGridPending()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dt = WbTransactions.PendingWeighBridgeForSecondWeight(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
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
				grdfill.DataSource = dtcol;
				grdfill.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdfill.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridsetting()
	{
		try
		{
			grdfill.RootTable.Columns["DocDate"].Width = 80;
			grdfill.RootTable.Columns["DocumentType"].Width = 150;
			grdfill.RootTable.Columns["TicketNo"].Width = 65;
			grdfill.RootTable.Columns["GpSrNo"].Width = 70;
			grdfill.RootTable.Columns["VehicleNo"].Width = 80;
			grdfill.RootTable.Columns["BiltyNo"].Width = 70;
			grdfill.RootTable.Columns["PartyName"].Width = 150;
			grdfill.RootTable.Columns["ItemName"].Width = 140;
			grdfill.RootTable.Columns["ItemQty"].Width = 70;
			grdfill.RootTable.Columns["WbCharges"].Width = 85;
			grdfill.RootTable.Columns["WbRemarks"].Width = 150;
			grdfill.RootTable.Columns["FirstWeight"].Width = 90;
			grdfill.RootTable.Columns["FirstWtWbStatus"].Width = 125;
			grdfill.RootTable.Columns["SecondWeight"].Width = 100;
			grdfill.RootTable.Columns["EntryUser"].Width = 150;
			grdfill.RootTable.Columns["LabStatus"].Width = 90;
			grdfill.RootTable.Columns["LabApprovalStatus"].Width = 130;
			grdfill.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdfill.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdfill.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdfill.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdfill.RootTable.Columns["WbCharges"].TextAlignment = (TextAlignment)3;
			grdfill.RootTable.Columns["WbCharges"].FormatString = "#,##0.###";
			grdfill.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			grdfill.RootTable.Columns["FirstWeight"].FormatString = "#,##0.###";
			grdfill.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			grdfill.RootTable.Columns["SecondWeight"].FormatString = "#,##0.###";
			grdfill.RootTable.Columns["Id"].Visible = false;
			grdfill.RootTable.Columns["DocTypeId"].Visible = false;
			grdfill.RootTable.Columns["ReferenceDocTypeId"].Visible = false;
			grdfill.RootTable.Columns.Add("Edit");
			grdfill.RootTable.Columns["Edit"].Caption = "Edit";
			grdfill.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfill.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdfill.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdfill.RootTable.Columns["Edit"].Width = 50;
			grdfill.RootTable.Columns["Edit"].Position = 0;
			grdfill.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
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
			if (grdfill.RowCount <= 0)
			{
				return;
			}
			GridEXColumn LabStatus = grdfill.RootTable.Columns["LabStatus"];
			GridEXColumn LabApprovalStatus = grdfill.RootTable.Columns["LabApprovalStatus"];
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

	private void grdfill_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RECID = Convert.ToInt32(grdfill.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
			}
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
			RECID = Convert.ToInt32(grdfill.CurrentRow.Cells[0].Value);
			ReadById(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatepassPendingForWeighBridge()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = WbTransactions.GatepassPendingForWeighBridge(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
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
				grdPendingGpForWeighBridge.DataSource = dtTarget;
				grdPendingGpForWeighBridge.RetrieveStructure();
				grdPendingGpForWeighBridge.RootTable.Columns["Id"].Visible = false;
				grdPendingGpForWeighBridge.RootTable.Columns["DocumentTypeId"].Visible = false;
				grdPendingGpForWeighBridge.RootTable.Columns["EntryUser"].Visible = false;
				grdPendingGpForWeighBridge.RootTable.Columns["GpSrNo"].Caption = "Gp No";
				grdPendingGpForWeighBridge.RootTable.Columns["DocumentType"].Width = 150;
				grdPendingGpForWeighBridge.RootTable.Columns["GpDate"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["GpSrNo"].Width = 60;
				grdPendingGpForWeighBridge.RootTable.Columns["PartyName"].Width = 180;
				grdPendingGpForWeighBridge.RootTable.Columns["ItemName"].Width = 180;
				grdPendingGpForWeighBridge.RootTable.Columns["ItemQty"].Width = 70;
				grdPendingGpForWeighBridge.RootTable.Columns["VehicleType"].Width = 85;
				grdPendingGpForWeighBridge.RootTable.Columns["VehicleNo"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["BiltyNo"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["Remarks"].Width = 150;
				grdPendingGpForWeighBridge.RootTable.Columns["GpStatus"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["EntryUser"].Width = 100;
				grdPendingGpForWeighBridge.RootTable.Columns["LabStatus"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["LabApprovalStatus"].Width = 100;
				grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"].Width = 80;
				grdPendingGpForWeighBridge.RootTable.Columns["GpApprovalStatus"].Width = 100;
				grdPendingGpForWeighBridge.RootTable.HeaderLines = 3;
				foreach (GridEXColumn item in (JanusCollectionBase)grdPendingGpForWeighBridge.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"].AggregateFunction = (AggregateFunction)2;
				grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"].TextAlignment = (TextAlignment)3;
				grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"].FormatString = "#,##0.###";
				grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"].TotalFormatString = "#,##0.###";
				grdPendingGpForWeighBridge.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
				grdPendingGpForWeighBridge.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
				grdPendingGpForWeighBridge.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
				grdPendingGpForWeighBridge.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
				grdPendingGpForWeighBridge.AllowDelete = (InheritableBoolean)2;
				grdPendingGpForWeighBridge.AllowEdit = (InheritableBoolean)2;
				grdPendingGpForWeighBridge.ColumnAutoResize = false;
			}
			else
			{
				grdPendingGpForWeighBridge.DataSource = null;
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
			if (grdPendingGpForWeighBridge.RowCount <= 0)
			{
				return;
			}
			GridEXColumn LabStatus = grdPendingGpForWeighBridge.RootTable.Columns["LabStatus"];
			GridEXColumn LabApprovalStatus = grdPendingGpForWeighBridge.RootTable.Columns["LabApprovalStatus"];
			GridEXColumn AccessWeight = grdPendingGpForWeighBridge.RootTable.Columns["AccessWeight"];
			GridEXColumn ApprovalStatus = grdPendingGpForWeighBridge.RootTable.Columns["GpApprovalStatus"];
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

	private void bindGrid()
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
				dtcol.Columns.Add("WbRemarks", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtget.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtget.Rows[i]["RecordNo"], dtget.Rows[i]["Id"], Conversion.ToDateTime(dtget.Rows[i]["DocDate"]).ToShortDateString(), dtget.Rows[i]["DocumentTypeDescription"], dtget.Rows[i]["WeighBridgeType"], dtget.Rows[i]["ReferenceDocNoId"], dtget.Rows[i]["GpSrNo"], dtget.Rows[i]["TicketNo"], dtget.Rows[i]["PartyName"], dtget.Rows[i]["VehicleNo"], dtget.Rows[i]["BiltyNo"], dtget.Rows[i]["ItemDescription"], dtget.Rows[i]["ReferenceDocTypeId"], dtget.Rows[i]["ItemQty"], dtget.Rows[i]["WbCharges"], dtget.Rows[i]["WorkingReportNo"], dtget.Rows[i]["FirstWeight"], dtget.Rows[i]["FirstWtWbStatus"], dtget.Rows[i]["SecondWeight"], dtget.Rows[i]["SecondWtWbStatus"], dtget.Rows[i]["NetWbWeight"], dtget.Rows[i]["WbRemarks"], Conversion.ToDateTime(dtget.Rows[i]["EntryDate"]).ToShortDateString(), dtget.Rows[i]["UserName"]);
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ReferenceDocTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ReferenceDocNoId"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 2);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			DataGridHistory.RootTable.Columns.Add("PrintWithLab");
			DataGridHistory.RootTable.Columns["PrintWithLab"].Caption = "PrintWithLab";
			DataGridHistory.RootTable.Columns["PrintWithLab"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["PrintWithLab"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["PrintWithLab"].ButtonText = "PrintWithLab";
			DataGridHistory.RootTable.Columns["PrintWithLab"].Width = 110;
			DataGridHistory.RootTable.Columns["PrintWithLab"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar3_Load(null, null);
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
				FromDate.Focus();
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
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				formRefresh();
				RECID = Convert.ToInt32(r.Cells["Id"].Value);
				ReadById(RECID);
			}
			if (e.Column.Key == "Print")
			{
				PrintFromGrid();
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			bindGrid();
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
			WbType();
			GatePassTypeFill();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfill).Name);
			if (fs != null)
			{
				grdfill.LoadLayoutFile((Stream)fs);
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
		try
		{
			CalculateNetWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateNetWeight()
	{
		try
		{
			if (!(txtFirstWeight.Text != string.Empty) || !(txtsecondWeight.Text != string.Empty))
			{
				return;
			}
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
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTareWeight_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateNetWeight();
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

	private void GenerateReport()
	{
		try
		{
			CommonServices.WbTransactionSlip280(RECID);
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

	private void PrintFromGrid()
	{
		try
		{
			CommonServices.WbTransactionSlip280(Convert.ToInt32(DataGridHistory.CurrentRow.Cells["Id"].Value));
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
				if (Conversion.ToInt(CmbGatePassNo.Value) > 0)
				{
					CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(CmbGatePassNo.Value));
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

	private void txtFirstWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtsecondWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtWbCharges_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtWorkingReportNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void btnWeighBridgeLookUp_Click(object sender, EventArgs e)
	{
		try
		{
			new WeighBridgeGeneralLookups(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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
			dt.Rows.Add("Ctrl+F1", "For Lookup");
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

	private void frmWeightbridge_KeyDown(object sender, KeyEventArgs e)
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
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.P)
				{
					PrintFromGrid();
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
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbReferenceType.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && btnUpdate.Visible && btnUpdate.Enabled)
				{
					GenerateReport();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbReferenceType.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdfill).Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 112;
				}
				if (e.Control && e.KeyCode == Keys.F1)
				{
					BtnWeightDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdfill).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPendingGpForWeighBridge).Focus();
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfill_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r2 = grdfill.CurrentRow;
				if (r2 != null && (int)r2.RowType == 0 && grdfill.CurrentColumn != null && grdfill.CurrentColumn.Key == "Edit")
				{
					RECID = Conversion.ToInt(grdfill.CurrentRow.Cells["Id"].Value);
					ReadById(RECID);
					txtsecondWeight.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				GridEXRow r = grdfill.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdfill.CurrentColumn != null)
				{
					RECID = Conversion.ToInt(grdfill.CurrentRow.Cells["Id"].Value);
					ReadById(RECID);
					txtsecondWeight.Focus();
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
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
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
						txtsecondWeight.Focus();
					}
					if (DataGridHistory.CurrentColumn.Key == "Print")
					{
						PrintFromGrid();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					formRefresh();
					RECID = Convert.ToInt32(DataGridHistory.CurrentRow.Cells[1].Value);
					ReadById(RECID);
					txtsecondWeight.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void txtTearWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	public void CalculateSupplierNetWeight()
	{
		try
		{
			if (txtLoadWeight.Text != string.Empty && txtTearWeight.Text != string.Empty)
			{
				double num = double.Parse(txtLoadWeight.Text);
				double b = double.Parse(txtTearWeight.Text);
				double c = Math.Abs(num - b);
				if (c >= 0.0)
				{
					txtSupplierWbNetWeight.Text = c.ToString();
				}
			}
			else
			{
				txtSupplierWbNetWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void txtTearWeight_TextChanged(object sender, EventArgs e)
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
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Expected O, but got Unknown
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_04f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Expected O, but got Unknown
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Expected O, but got Unknown
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_0883: Expected O, but got Unknown
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Expected O, but got Unknown
		//IL_0fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbc: Expected O, but got Unknown
		//IL_0fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd3: Expected O, but got Unknown
		//IL_1364: Unknown result type (might be due to invalid IL or missing references)
		//IL_136e: Expected O, but got Unknown
		//IL_685d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6867: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdfill_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingGpForWeighBridge_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.WeightBridge.WeightbridgeMannual));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
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
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdfill = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label33 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdPendingGpForWeighBridge = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtSupplierWbNetWeight = new System.Windows.Forms.TextBox();
		this.txtTearWeight = new System.Windows.Forms.TextBox();
		this.txtLoadWeight = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.cmbVehicleNo = new UltraCombo();
		this.ChkMultiItem = new System.Windows.Forms.CheckBox();
		this.CmbPartyName = new UltraCombo();
		this.CmbWeighBridgeType = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtWorkingReportNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtWeightDiffComments = new System.Windows.Forms.TextBox();
		this.cmbVehicleType = new UltraCombo();
		this.cmbReferenceType = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbGatePassNo = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.txtsecondWeight = new System.Windows.Forms.TextBox();
		this.txtFirstWeight = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtWbCharges = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtItemqty = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtTickectNo = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.chkPrintWithLab = new System.Windows.Forms.CheckBox();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.txtFirstDate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtSecondDate = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.BtnPurchaseLab = new System.Windows.Forms.ToolStripButton();
		this.btnWeighBridgeLookUp = new System.Windows.Forms.ToolStripButton();
		this.BtnWeightDefine = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label36 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.CmbWbTypeHistory = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbGatePassTypeHistory = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.GpNoTo = new System.Windows.Forms.TextBox();
		this.GPNoFrom = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.TxtVehicleNumber = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtTicketNoTo = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtTicketNoFrom = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label20 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label14 = new System.Windows.Forms.Label();
		this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfill).BeginInit();
		this.panel9.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingGpForWeighBridge).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassNo).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassTypeHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1075, 661);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1075, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1067, 632);
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
		this.panel2.Size = new System.Drawing.Size(1061, 626);
		this.panel2.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 273);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1061, 353);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdfill);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1053, 324);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Pending Tickets For Second Weight";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdfill.AllowEdit = (InheritableBoolean)2;
		grdfill_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdfill.DesignTimeLayout = grdfill_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfill).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfill.DynamicFiltering = true;
		this.grdfill.FilterMode = (FilterMode)2;
		this.grdfill.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfill).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfill.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdfill).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdfill).Name = "grdfill";
		((ScrollableBase)this.grdfill).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfill).Size = new System.Drawing.Size(1047, 297);
		((System.Windows.Forms.Control)(object)this.grdfill).TabIndex = 0;
		this.grdfill.TotalRow = (InheritableBoolean)1;
		this.grdfill.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfill).VisualStyle = (VisualStyle)4;
		this.grdfill.FormattingRow += new RowLoadEventHandler(grdfill_FormattingRow);
		this.grdfill.ColumnButtonClick += new ColumnActionEventHandler(grdfill_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfill).DoubleClick += new System.EventHandler(grdfill_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdfill).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfill_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.label33);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1047, 21);
		this.panel9.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(998, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfill;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 19);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.Teal;
		this.label33.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label33.Location = new System.Drawing.Point(5, 2);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(223, 14);
		this.label33.TabIndex = 0;
		this.label33.Text = "Pending Tickets For Second Weight";
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge);
		this.tabPage4.Controls.Add(this.panel10);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1053, 324);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Pending Gatepass For First Weight";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdPendingGpForWeighBridge.AllowEdit = (InheritableBoolean)2;
		grdPendingGpForWeighBridge_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingGpForWeighBridge.DesignTimeLayout = grdPendingGpForWeighBridge_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingGpForWeighBridge.DynamicFiltering = true;
		this.grdPendingGpForWeighBridge.FilterMode = (FilterMode)2;
		this.grdPendingGpForWeighBridge.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingGpForWeighBridge.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).Name = "grdPendingGpForWeighBridge";
		((ScrollableBase)this.grdPendingGpForWeighBridge).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).Size = new System.Drawing.Size(1047, 297);
		((System.Windows.Forms.Control)(object)this.grdPendingGpForWeighBridge).TabIndex = 1;
		this.grdPendingGpForWeighBridge.TotalRow = (InheritableBoolean)1;
		this.grdPendingGpForWeighBridge.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingGpForWeighBridge).VisualStyle = (VisualStyle)4;
		this.grdPendingGpForWeighBridge.FormattingRow += new RowLoadEventHandler(grdPendingGpForWeighBridge_FormattingRow);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1047, 21);
		this.panel10.TabIndex = 3;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label41);
		this.panel3.Controls.Add(this.label43);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.label46);
		this.panel3.Controls.Add(this.txtSupplierWbNetWeight);
		this.panel3.Controls.Add(this.txtTearWeight);
		this.panel3.Controls.Add(this.txtLoadWeight);
		this.panel3.Controls.Add(this.label34);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleNo);
		this.panel3.Controls.Add(this.ChkMultiItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType);
		this.panel3.Controls.Add(this.label31);
		this.panel3.Controls.Add(this.label32);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel3.Controls.Add(this.label30);
		this.panel3.Controls.Add(this.label29);
		this.panel3.Controls.Add(this.label28);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.txtWorkingReportNo);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.txtWeightDiffComments);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceType);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassNo);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.txtNetWeight);
		this.panel3.Controls.Add(this.txtsecondWeight);
		this.panel3.Controls.Add(this.txtFirstWeight);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.txtRemarks);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.txtWbCharges);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtItemqty);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.txtVehicleNo);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.txtBilityNo);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtTickectNo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 53);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1061, 220);
		this.panel3.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(650, 164);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(12, 24);
		this.label6.TabIndex = 134;
		this.label6.Text = "*";
		this.label6.UseCompatibleTextRendering = true;
		this.label6.Visible = false;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.Red;
		this.label16.Location = new System.Drawing.Point(650, 139);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(12, 24);
		this.label16.TabIndex = 132;
		this.label16.Text = "*";
		this.label16.UseCompatibleTextRendering = true;
		this.label16.Visible = false;
		this.label41.AutoSize = true;
		this.label41.BackColor = System.Drawing.Color.White;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(650, 189);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(12, 24);
		this.label41.TabIndex = 136;
		this.label41.Text = "*";
		this.label41.UseCompatibleTextRendering = true;
		this.label41.Visible = false;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(559, 169);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(71, 15);
		this.label43.TabIndex = 139;
		this.label43.Text = "Tare Weight";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(559, 144);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(75, 15);
		this.label45.TabIndex = 138;
		this.label45.Text = "Load Weight";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(559, 187);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(76, 30);
		this.label46.TabIndex = 137;
		this.label46.Text = "Supplier Net \r\nWeight";
		this.txtSupplierWbNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWbNetWeight.Enabled = false;
		this.txtSupplierWbNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierWbNetWeight.Location = new System.Drawing.Point(666, 190);
		this.txtSupplierWbNetWeight.Name = "txtSupplierWbNetWeight";
		this.txtSupplierWbNetWeight.ReadOnly = true;
		this.txtSupplierWbNetWeight.Size = new System.Drawing.Size(135, 23);
		this.txtSupplierWbNetWeight.TabIndex = 20;
		this.txtSupplierWbNetWeight.TabStop = false;
		this.txtTearWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTearWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTearWeight.Location = new System.Drawing.Point(666, 165);
		this.txtTearWeight.Name = "txtTearWeight";
		this.txtTearWeight.Size = new System.Drawing.Size(135, 23);
		this.txtTearWeight.TabIndex = 19;
		this.txtTearWeight.TextChanged += new System.EventHandler(txtTearWeight_TextChanged);
		this.txtTearWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTearWeight_KeyPress);
		this.txtLoadWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLoadWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadWeight.Location = new System.Drawing.Point(666, 140);
		this.txtLoadWeight.Name = "txtLoadWeight";
		this.txtLoadWeight.Size = new System.Drawing.Size(135, 23);
		this.txtLoadWeight.TabIndex = 18;
		this.txtLoadWeight.TextChanged += new System.EventHandler(txtLoadWeight_TextChanged);
		this.txtLoadWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLoadWeight_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(7, 91);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(80, 17);
		this.label34.TabIndex = 130;
		this.label34.Text = "Party Name";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbInvoiceNo.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(391, 138);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TextChanged += new System.EventHandler(cmbVehicleNo_TextChanged);
		this.cmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbVehicleNo.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Location = new System.Drawing.Point(391, 60);
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Name = "cmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbVehicleNo).TextChanged += new System.EventHandler(cmbVehicleNo_TextChanged);
		this.ChkMultiItem.AutoSize = true;
		this.ChkMultiItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkMultiItem.Location = new System.Drawing.Point(806, 192);
		this.ChkMultiItem.Margin = new System.Windows.Forms.Padding(2);
		this.ChkMultiItem.Name = "ChkMultiItem";
		this.ChkMultiItem.Size = new System.Drawing.Size(87, 19);
		this.ChkMultiItem.TabIndex = 21;
		this.ChkMultiItem.Text = "Multi Items";
		this.ChkMultiItem.UseVisualStyleBackColor = true;
		this.ChkMultiItem.Visible = false;
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
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(110, 86);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Leave += new System.EventHandler(CmbPartyName_Leave);
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
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Location = new System.Drawing.Point(666, 6);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Name = "CmbWeighBridgeType";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).TabIndex = 14;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(559, 12);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(54, 15);
		this.label31.TabIndex = 128;
		this.label31.Text = "Wb Type";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(297, 12);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(77, 15);
		this.label32.TabIndex = 126;
		this.label32.Text = "Packing Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(7, 144);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 15);
		this.label11.TabIndex = 126;
		this.label11.Text = "Item Name";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(391, 6);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 8;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance119;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance125).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(110, 138);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.BackColor = System.Drawing.Color.White;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(375, 63);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(10, 21);
		this.label30.TabIndex = 11;
		this.label30.Text = "*";
		this.label30.UseCompatibleTextRendering = true;
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.White;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(375, 34);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(12, 24);
		this.label29.TabIndex = 9;
		this.label29.Text = "*";
		this.label29.UseCompatibleTextRendering = true;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.White;
		this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(650, 87);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(12, 24);
		this.label28.TabIndex = 17;
		this.label28.Text = "*";
		this.label28.UseCompatibleTextRendering = true;
		this.label27.AutoSize = true;
		this.label27.BackColor = System.Drawing.Color.White;
		this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(650, 61);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(12, 24);
		this.label27.TabIndex = 16;
		this.label27.Text = "*";
		this.label27.UseCompatibleTextRendering = true;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.White;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(650, 112);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(12, 24);
		this.label26.TabIndex = 18;
		this.label26.Text = "*";
		this.label26.UseCompatibleTextRendering = true;
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.ForeColor = System.Drawing.Color.Red;
		this.label25.Location = new System.Drawing.Point(93, 61);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(12, 24);
		this.label25.TabIndex = 2;
		this.label25.Text = "*";
		this.label25.UseCompatibleTextRendering = true;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(93, 34);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(12, 24);
		this.label7.TabIndex = 1;
		this.label7.Text = "*";
		this.label7.UseCompatibleTextRendering = true;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.Red;
		this.label23.Location = new System.Drawing.Point(93, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(12, 24);
		this.label23.TabIndex = 0;
		this.label23.Text = "*";
		this.label23.UseCompatibleTextRendering = true;
		this.txtWorkingReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkingReportNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkingReportNo.Location = new System.Drawing.Point(666, 35);
		this.txtWorkingReportNo.Name = "txtWorkingReportNo";
		this.txtWorkingReportNo.Size = new System.Drawing.Size(135, 23);
		this.txtWorkingReportNo.TabIndex = 15;
		this.txtWorkingReportNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWorkingReportNo_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(559, 39);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(104, 15);
		this.label4.TabIndex = 56;
		this.label4.Text = "WorkingReportNo";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(7, 169);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(94, 15);
		this.label24.TabIndex = 54;
		this.label24.Text = "Wt Diff Remarks";
		this.txtWeightDiffComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightDiffComments.Location = new System.Drawing.Point(110, 165);
		this.txtWeightDiffComments.Name = "txtWeightDiffComments";
		this.txtWeightDiffComments.Size = new System.Drawing.Size(443, 23);
		this.txtWeightDiffComments.TabIndex = 6;
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance112).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbVehicleType.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(391, 33);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 9;
		this.cmbReferenceType.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbReferenceType.DropDownStyle = (UltraComboStyle)1;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReferenceType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Location = new System.Drawing.Point(110, 6);
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Name = "cmbReferenceType";
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).TextChanged += new System.EventHandler(cmbReferenceType_Leave);
		((System.Windows.Forms.Control)(object)this.cmbReferenceType).Leave += new System.EventHandler(cmbReferenceType_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(559, 92);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(89, 15);
		this.label22.TabIndex = 49;
		this.label22.Text = "Second Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(559, 66);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(71, 15);
		this.label21.TabIndex = 48;
		this.label21.Text = "First Weight";
		this.CmbGatePassNo.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePassNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Location = new System.Drawing.Point(110, 33);
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Name = "CmbGatePassNo";
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).TextChanged += new System.EventHandler(cmbRefDocNo_Leave);
		((System.Windows.Forms.Control)(object)this.CmbGatePassNo).Leave += new System.EventHandler(cmbRefDocNo_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(7, 39);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 15);
		this.label19.TabIndex = 47;
		this.label19.Text = "GatePass No";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 12);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(85, 15);
		this.label18.TabIndex = 45;
		this.label18.Text = "Gate Pass Type";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(559, 117);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(68, 15);
		this.label17.TabIndex = 43;
		this.label17.Text = "Net Weight";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(666, 113);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(135, 23);
		this.txtNetWeight.TabIndex = 17;
		this.txtNetWeight.TabStop = false;
		this.txtsecondWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsecondWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsecondWeight.Location = new System.Drawing.Point(666, 88);
		this.txtsecondWeight.Name = "txtsecondWeight";
		this.txtsecondWeight.Size = new System.Drawing.Size(135, 23);
		this.txtsecondWeight.TabIndex = 16;
		this.txtsecondWeight.TextChanged += new System.EventHandler(txtTareWeight_Leave);
		this.txtsecondWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsecondWeight_KeyPress);
		this.txtFirstWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFirstWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstWeight.Location = new System.Drawing.Point(666, 62);
		this.txtFirstWeight.Name = "txtFirstWeight";
		this.txtFirstWeight.Size = new System.Drawing.Size(135, 23);
		this.txtFirstWeight.TabIndex = 15;
		this.txtFirstWeight.TextChanged += new System.EventHandler(txtGroosWeight_Leave);
		this.txtFirstWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFirstWeight_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 194);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(52, 15);
		this.label13.TabIndex = 28;
		this.label13.Text = "Remarks";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(110, 190);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(443, 23);
		this.txtRemarks.TabIndex = 7;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(297, 117);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(71, 15);
		this.label12.TabIndex = 26;
		this.label12.Text = "Wb Charges";
		this.txtWbCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWbCharges.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWbCharges.Location = new System.Drawing.Point(391, 113);
		this.txtWbCharges.Name = "txtWbCharges";
		this.txtWbCharges.Size = new System.Drawing.Size(162, 23);
		this.txtWbCharges.TabIndex = 12;
		this.txtWbCharges.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWbCharges_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 117);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(54, 15);
		this.label10.TabIndex = 22;
		this.label10.Text = "Item Qty";
		this.txtItemqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemqty.Location = new System.Drawing.Point(110, 113);
		this.txtItemqty.Name = "txtItemqty";
		this.txtItemqty.Size = new System.Drawing.Size(179, 23);
		this.txtItemqty.TabIndex = 4;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(297, 144);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 18;
		this.label35.Text = "Invoice No";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(297, 39);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 15);
		this.label9.TabIndex = 20;
		this.label9.Text = "Vehicle Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(297, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 15);
		this.label8.TabIndex = 18;
		this.label8.Text = "Vehicle No";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(391, 62);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(162, 23);
		this.txtVehicleNo.TabIndex = 9;
		this.txtVehicleNo.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(297, 92);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 12;
		this.label5.Text = "Bility No";
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBilityNo.Location = new System.Drawing.Point(391, 88);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(162, 23);
		this.txtBilityNo.TabIndex = 11;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 66);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 15);
		this.label1.TabIndex = 2;
		this.label1.Text = "Ticket No.";
		this.txtTickectNo.BackColor = System.Drawing.Color.White;
		this.txtTickectNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTickectNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTickectNo.Location = new System.Drawing.Point(110, 62);
		this.txtTickectNo.Name = "txtTickectNo";
		this.txtTickectNo.ReadOnly = true;
		this.txtTickectNo.Size = new System.Drawing.Size(179, 23);
		this.txtTickectNo.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.chkPrintWithLab);
		this.panel6.Controls.Add(this.ChkBox);
		this.panel6.Controls.Add(this.txtFirstDate);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Controls.Add(this.txtSecondDate);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1061, 26);
		this.panel6.TabIndex = 2;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(187, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = " Weigh Bridge Manual";
		this.chkPrintWithLab.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintWithLab.AutoSize = true;
		this.chkPrintWithLab.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintWithLab.ForeColor = System.Drawing.Color.White;
		this.chkPrintWithLab.Location = new System.Drawing.Point(765, 4);
		this.chkPrintWithLab.Margin = new System.Windows.Forms.Padding(2);
		this.chkPrintWithLab.Name = "chkPrintWithLab";
		this.chkPrintWithLab.Size = new System.Drawing.Size(169, 20);
		this.chkPrintWithLab.TabIndex = 0;
		this.chkPrintWithLab.Text = "Is Preview With Lab";
		this.chkPrintWithLab.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(956, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(102, 20);
		this.ChkBox.TabIndex = 1;
		this.ChkBox.Text = "Is Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.txtFirstDate.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtFirstDate.Enabled = false;
		this.txtFirstDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstDate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.txtFirstDate.Location = new System.Drawing.Point(266, 2);
		this.txtFirstDate.Name = "txtFirstDate";
		this.txtFirstDate.Size = new System.Drawing.Size(128, 23);
		this.txtFirstDate.TabIndex = 19;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(204, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(62, 15);
		this.label2.TabIndex = 5;
		this.label2.Text = "First Time";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(397, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(79, 15);
		this.label3.TabIndex = 21;
		this.label3.Text = "Second Time";
		this.txtSecondDate.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtSecondDate.Enabled = false;
		this.txtSecondDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSecondDate.Format = System.Windows.Forms.DateTimePickerFormat.Time;
		this.txtSecondDate.Location = new System.Drawing.Point(478, 2);
		this.txtSecondDate.Name = "txtSecondDate";
		this.txtSecondDate.Size = new System.Drawing.Size(124, 23);
		this.txtSecondDate.TabIndex = 20;
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1061, 27);
		this.panel4.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator5, this.btnsave, this.btnUpdate, this.toolStripSeparator4, this.Print, this.toolStripSeparator3, this.btnSlip257, this.BtnPurchaseLab,
			this.btnWeighBridgeLookUp, this.BtnWeightDefine, this.toolStripSeparator2, this.BtnShortCutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1061, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
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
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "280-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
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
		this.btnWeighBridgeLookUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnWeighBridgeLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWeighBridgeLookUp.Name = "btnWeighBridgeLookUp";
		this.btnWeighBridgeLookUp.Size = new System.Drawing.Size(146, 24);
		this.btnWeighBridgeLookUp.Text = "Weigh Bridge Lookup";
		this.btnWeighBridgeLookUp.Click += new System.EventHandler(btnWeighBridgeLookUp_Click);
		this.BtnWeightDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnWeightDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnWeightDefine.Name = "BtnWeightDefine";
		this.BtnWeightDefine.Size = new System.Drawing.Size(153, 24);
		this.BtnWeightDefine.Text = "Vehicle Weight Define";
		this.BtnWeightDefine.Click += new System.EventHandler(BtnWeightDefine_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1067, 632);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 157);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1061, 472);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 45;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar3);
		this.panel11.Controls.Add(this.label36);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 132);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1061, 25);
		this.panel11.TabIndex = 47;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1012, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar3.TabIndex = 21;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.Teal;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label36.Location = new System.Drawing.Point(4, 3);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(142, 19);
		this.label36.TabIndex = 1;
		this.label36.Text = "Filtered Records";
		this.panel12.Controls.Add(this.groupBox3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 61);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1061, 71);
		this.panel12.TabIndex = 46;
		this.groupBox3.Controls.Add(this.ToDate);
		this.groupBox3.Controls.Add(this.FromDate);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory);
		this.groupBox3.Controls.Add(this.lblLanguage);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.GpNoTo);
		this.groupBox3.Controls.Add(this.GPNoFrom);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.TxtVehicleNumber);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.txtTicketNoTo);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.txtTicketNoFrom);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(953, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(66, 43);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(123, 22);
		this.ToDate.TabIndex = 1;
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(66, 16);
		this.FromDate.Name = "FromDate";
		this.FromDate.ShowCheckBox = true;
		this.FromDate.Size = new System.Drawing.Size(123, 22);
		this.FromDate.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(943, 14);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 13;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdapproveddate.Visible = false;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(863, 47);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 12;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(863, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 10;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(863, 29);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 11;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.CmbWbTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbWbTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWbTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWbTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWbTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
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
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Location = new System.Drawing.Point(722, 14);
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Name = "CmbWbTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).Size = new System.Drawing.Size(137, 26);
		((System.Windows.Forms.Control)(object)this.CmbWbTypeHistory).TabIndex = 8;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(669, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(56, 15);
		this.label37.TabIndex = 217;
		this.label37.Text = "W.B Type";
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
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGatePassTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Location = new System.Drawing.Point(516, 41);
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Name = "CmbGatePassTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassTypeHistory).TabIndex = 7;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblLanguage.Location = new System.Drawing.Point(432, 47);
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
		this.btnshow.Location = new System.Drawing.Point(793, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(66, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.GpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.GpNoTo.Location = new System.Drawing.Point(364, 43);
		this.GpNoTo.Name = "GpNoTo";
		this.GpNoTo.Size = new System.Drawing.Size(65, 23);
		this.GpNoTo.TabIndex = 5;
		this.GPNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GPNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.GPNoFrom.Location = new System.Drawing.Point(297, 43);
		this.GPNoFrom.Name = "GPNoFrom";
		this.GPNoFrom.Size = new System.Drawing.Size(65, 23);
		this.GPNoFrom.TabIndex = 4;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(192, 47);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(88, 15);
		this.label39.TabIndex = 214;
		this.label39.Text = "Gp No From,To";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(4, 47);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(48, 15);
		this.label38.TabIndex = 213;
		this.label38.Text = "Date To";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(4, 20);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 213;
		this.label40.Text = "Date From";
		this.TxtVehicleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TxtVehicleNumber.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtVehicleNumber.Location = new System.Drawing.Point(516, 16);
		this.TxtVehicleNumber.Name = "TxtVehicleNumber";
		this.TxtVehicleNumber.Size = new System.Drawing.Size(150, 23);
		this.TxtVehicleNumber.TabIndex = 6;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(432, 20);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(67, 15);
		this.label42.TabIndex = 211;
		this.label42.Text = "Vehicle No.";
		this.txtTicketNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNoTo.Location = new System.Drawing.Point(364, 16);
		this.txtTicketNoTo.Name = "txtTicketNoTo";
		this.txtTicketNoTo.Size = new System.Drawing.Size(65, 23);
		this.txtTicketNoTo.TabIndex = 3;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(192, 20);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(105, 15);
		this.label44.TabIndex = 209;
		this.label44.Text = "Ticket No From,To";
		this.txtTicketNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTicketNoFrom.Location = new System.Drawing.Point(297, 16);
		this.txtTicketNoFrom.Name = "txtTicketNoFrom";
		this.txtTicketNoFrom.Size = new System.Drawing.Size(65, 23);
		this.txtTicketNoFrom.TabIndex = 2;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label20);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 33);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1061, 28);
		this.panel7.TabIndex = 1;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.Teal;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label20.Location = new System.Drawing.Point(2, 4);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(181, 19);
		this.label20.TabIndex = 1;
		this.label20.Text = "Weigh Bridge History";
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1061, 30);
		this.panel5.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1061, 27);
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
		base.ClientSize = new System.Drawing.Size(1075, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "WeightbridgeMannual";
		this.Text = "WeighBridge Manual";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(WeightbridgeMannual_FormClosing);
		base.Load += new System.EventHandler(frmWeightbridge_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmWeightbridge_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfill).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingGpForWeighBridge).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassNo).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassTypeHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
