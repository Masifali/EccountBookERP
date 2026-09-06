// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.WeightBridge;
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
namespace Architecture.WinApp.Sale
{

public class OutwardGatePass : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private bool UpdateMode;

	private bool GpUpdateWithoutApproval;

	public UserAccount UserAccount = new UserAccount();

	private int Id;

	private int Qty;

	public int SOId;

	private int GpId;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool Approve;

	private DataTable table = new DataTable();

	private DataTable tablegrd = new DataTable();

	private DataTable lstSupplier = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtOrderType = new DataTable();

	private DataTable dtPtOrder = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

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

	private Label label15;

	private Panel panel4;

	private DateTimePicker txtouttime;

	private DateTimePicker txtintime;

	private DateTimePicker txtgpdate;

	private TextBox txtNetPaid;

	private TextBox txtbiltyno;

	private TextBox txtfreight;

	private TextBox txtvehicleno;

	private Label label13;

	private Label label4;

	private Label label14;

	private Label label10;

	private Label label12;

	private Label label11;

	private Label label20;

	private Label label8;

	private Label label2;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtqty;

	private Label label22;

	private Label label23;

	private Label label25;

	private Label label24;

	private Label label26;

	private TextBox txtFactoryWeight;

	private TextBox txtWeighBridgeSlipNo;

	private Label label28;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo CmbVariety;

	internal UltraCombo cmbsupp;

	private TextBox txtContainerNo2;

	private TextBox txtContainerNo1;

	private Label containerno2;

	private Label ContainerNo1;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal UltraCombo cmbvehicletype;

	internal UltraCombo cmbgptype;

	internal UltraCombo cmbcity;

	private ToolStrip toolStrip2;

	internal UltraCombo CmbStatus;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbGPBasedOn;

	private Label label18;

	private Label label19;

	private TextBox txtContrainerWeight;

	private Label label27;

	private ToolStripButton btnPrint295;

	private Label lblApprovedBy;

	private TextBox txtApprovedBy;

	private Label label29;

	private TextBox txtDifferenceWeight;

	internal UltraCombo CmbWeighBridgeStatus;

	private Label label35;

	private Label label36;

	private Label label32;

	private Label label31;

	private ToolStripButton btndefvehicle;

	private Label label6;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private TabPage SoInformation;

	private TextBox txtCompareWeight;

	private Label label39;

	private TextBox txtPackUnit;

	private Label label38;

	private TextBox txtgpno;

	private TextBox txtgptypeno;

	private Label label1;

	private Label label3;

	private TextBox txtremarks;

	private TextBox txtDiffWeightRemarks;

	private GroupBox groupBox1;

	internal GridEX grd;

	private Panel panel6;

	internal GridEX grdDoInformation;

	private Panel panel10;

	internal GridEX GrdSaleOrderInformation;

	private Panel panel11;

	private Panel panel7;

	private Label label16;

	private Label label9;

	private Label label17;

	internal GridEX grdhistory;

	private Panel panel5;

	private Label label21;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker txtToDateHistory;

	private DateTimePicker txtFromDateHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnShortCut;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Panel panel8;

	private Label label30;

	private GroupBox groupBox4;

	private TextBox txtToDocDoInfo;

	private TextBox txtFromDocDoInfo;

	internal UltraCombo CmbCustomerDoInfo;

	private DateTimePicker ToDateDoInfo;

	private DateTimePicker FromDateDoInfo;

	private Button BtnResetDoInfo;

	private Button BtnShowDoInfo;

	private Label label7;

	private Label label33;

	private Label label34;

	private Label label37;

	private Label label40;

	private Panel panel12;

	private Label label41;

	private GroupBox groupBox3;

	private TextBox txtToDocSoInfo;

	private TextBox txtFromDocSoInfo;

	internal UltraCombo cmbCustomerSoInfo;

	private DateTimePicker ToDateSoInfo;

	private DateTimePicker FromDateSoInfo;

	private Button BtnResetSoInfo;

	private Button BtnShowSoInfo;

	private Label label5;

	private Label label42;

	private Label label43;

	private Label label44;

	private Label label45;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private CheckBox chkDriverInfoForm;

	private ToolStripButton BtnDriverForm;

	public TextBox CmbOrderno;

	private LinkLabel labelGdnForm;

	private LinkLabel labelWeightBridgeForm;

	private RadioButton RadNoneFormToOpenOnInsert;

	private RadioButton RadGrnFormToOpenOnInsert;

	private RadioButton RadWBFormToOpenOnInsert;

	private Panel panelPaymentTermOrder;

	internal GridEX grdPaymentTermOrder;

	private Label labelSealNo1;

	private Label labelSealNo2;

	private TextBox txtSealNo1;

	private TextBox txtSealNo2;

	private CheckBox chkPrintPreview290;

	private DateTimePicker txtBiltyDate;

	private Label label46;

	public OutwardGatePass(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void OutwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 625);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formright);
			FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formright);
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			if (!Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SO Compulsory in GPO")))
			{
				SupplierFill();
			}
			Status();
			WeighBridgeStatus();
			GpBasedOn();
			gpnofill();
			gatepasstype();
			vehicleTypefill();
			CityFill();
			grdfrmfill();
			btnupdate.Visible = false;
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			txtDiffWeightRemarks.ReadOnly = true;
			WarehouseFill();
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			CmbGPBasedOn.Focus();
			ChkBox.Checked = true;
			cmbgptype_Leave_1(null, null);
			CustomerHistoryFill();
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				cmbcity.Value = CityId;
			}
			List<ConfigrationsAllocation> lis = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "GatePassOutwardUpdateWithoutApproval").ToList();
			if (lis.Count > 0)
			{
				GpUpdateWithoutApproval = Conversion.ToBool((object)lis[0].ConfigKey.ToString());
			}
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			CustomerDoInFoFill();
			FromDateDoInfo.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateDoInfo.Value = DateTime.Now;
			CustomerSoInFoFill();
			FromDateSoInfo.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateSoInfo.Value = DateTime.Now;
			dtPtOrder.Columns.Add("OrderId", typeof(int));
			dtPtOrder.Columns.Add("OrderNo", typeof(int));
			dtPtOrder.Columns.Add("PaymentTerm", typeof(string));
			CustomerHistoryFill();
			txtFromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			txtToDateHistory.Value = DateTime.Now;
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int Code = GatePassOutward.GenerategpCode(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 91,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			txtgpno.Text = Code.ToString();
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
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeighBridgeStatus()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Auto");
			dt.Rows.Add(2, "Mannual");
			DDL.BindDDL(dt, CmbWeighBridgeStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbWeighBridgeStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpBasedOn()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbGPBasedOn.Value);
			GatePassOutward obj = new GatePassOutward();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtOrderType = GatePassOutward.GetOrderTypeForOutwardGP(obj);
			if (dtOrderType.Rows.Count > 0)
			{
				DDL.BindDDL(dtOrderType, CmbGPBasedOn, "Id", "OrderType", "OrderType", ZeroIndex: true);
				((UltraGridBase)CmbGPBasedOn).Rows[1].Activate();
				if (ID > 0)
				{
					if (dtOrderType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbGPBasedOn.Value = ID;
					}
					else
					{
						((Control)(object)CmbGPBasedOn).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbGPBasedOn).Text = string.Empty;
				((UltraGridBase)CmbGPBasedOn).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gatepasstype()
	{
		try
		{
			int ID = Conversion.ToInt(cmbgptype.Value);
			DataTable dt = CommonServices.GatePassTypeFill("Outward");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbgptype, "Id", "GpTypeDescription", "Gate Pass Type", ZeroIndex: true);
				((UltraGridBase)cmbgptype).Rows[2].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbgptype.Value = ID;
					}
					else
					{
						((Control)(object)cmbgptype).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbgptype).Text = string.Empty;
				((UltraGridBase)cmbgptype).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgptype_Leave_1(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		try
		{
			CmbOrderno.Text = string.Empty;
			int GpTypeCode = GatePassOutward.GenerateGPTypeCode(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 91,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				GatepassType = ((Control)(object)cmbgptype).Text.ToString(),
				BranchesId = UserAccount.BranchesId
			});
			if (GpTypeCode > 0)
			{
				txtgptypeno.Text = GpTypeCode.ToString();
			}
			else
			{
				MessageBox.Show("Please GatePass Type Select");
				cmbgptype.Focus();
			}
			bool Value = ((Control)(object)cmbgptype).Text == "Export" || ((Control)(object)cmbgptype).Text == "Export Return" || ((Control)(object)cmbgptype).Text == "Import";
			txtContainerNo1.Visible = Value;
			txtContainerNo2.Visible = Value;
			ContainerNo1.Visible = Value;
			containerno2.Visible = Value;
			txtSealNo1.Visible = Value;
			txtSealNo2.Visible = Value;
			labelSealNo1.Visible = Value;
			labelSealNo2.Visible = Value;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbvehicletype.Value);
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Description", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbvehicletype.Value = ID;
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
			MessageBox.Show(ex.Message);
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
			int ID = Conversion.ToInt(cmbcity.Value);
			DataTable dt = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbcity, "Id", "CityName", "City Name", true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbcity.Value = ID;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
	{
		try
		{
			int ID = Conversion.ToInt(cmbsupp.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				lstSupplier = CommonServices.GetVendorsAndCustomers(1);
				if (lstSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(lstSupplier, cmbsupp, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					if (ID > 0)
					{
						if (lstSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
						{
							cmbsupp.Value = ID;
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
				return;
			}
			lstSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (lstSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(lstSupplier, cmbsupp, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				if (ID > 0)
				{
					if (lstSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						cmbsupp.Value = ID;
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
			MessageBox.Show(ex.Message);
		}
	}

	public void CmbOrderno_Leave(object sender, EventArgs e)
	{
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f3: Expected O, but got Unknown
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		try
		{
			txtContainerNo1.Enabled = true;
			txtContainerNo2.Enabled = true;
			txtSealNo1.Enabled = true;
			txtSealNo2.Enabled = true;
			if (((Control)(object)cmbgptype).Text == string.Empty || ((Control)(object)cmbgptype).Text == null || Conversion.ToInt(cmbgptype.Value) == 0)
			{
				cmbgptype.Focus();
				MessageBox.Show("GatePass Type Field Required");
				return;
			}
			if (((Control)(object)CmbGPBasedOn).Text == string.Empty || ((Control)(object)CmbGPBasedOn).Text == null || Conversion.ToInt(CmbGPBasedOn.Value) == 0)
			{
				CmbGPBasedOn.Focus();
				MessageBox.Show("Order Type Field Required");
				return;
			}
			if (((Control)(object)CmbGPBasedOn).Text != "General" && ((Control)(object)CmbGPBasedOn).Text != "Purchase Return" && Conversion.ToInt((object)CmbOrderno.Text.Trim().ToString()) <= 0)
			{
				MessageBox.Show("No Field Required");
				return;
			}
			((Control)(object)CmbVariety).Text = string.Empty;
			((UltraGridBase)CmbVariety).DataSource = null;
			int DocumentTypeId = Conversion.ToInt(CmbGPBasedOn.Value);
			switch (DocumentTypeId)
			{
			case 81:
			case 90:
			case 1605:
			{
				DataTable lst = SupplierCustomer.GetSupplierBySaleOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					OrderId = Conversion.ToInt((object)CmbOrderno.Text.Trim()),
					DocumentTypeId = DocumentTypeId,
					DocDate = txtgpdate.Value,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (lst.Rows.Count > 0)
				{
					SOId = Conversion.ToInt((object)lst.Rows[0]["SaleOrderId"].ToString());
					DataTable dt = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
					{
						Id = SOId
					});
					if (dt.Rows.Count > 0)
					{
						DataTable itemdt = new DataTable();
						itemdt.Columns.Add("ItemId");
						itemdt.Columns.Add("ItemName");
						itemdt.Columns.Add("Equivalent");
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							itemdt.Rows.Add(dt.Rows[i]["OrderItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["Equivalent"]);
						}
						DDL.BindDDL(itemdt, CmbVariety, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)CmbVariety).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbVariety).Rows[0].Activate();
					}
					if (lst.Rows.Count > 0)
					{
						DDL.BindDDLNew(lst, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
					}
					if ((DocumentTypeId == 90 || DocumentTypeId == 81) && SOId > 0)
					{
						DataTable dtQty = GatePassOutward.GetSaleOrderBalQtyAndWeightByOrderAndItemId(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							OrderId = SOId
						});
						if (dtQty.Rows.Count > 0 && dtQty != null)
						{
							txtqty.Text = Conversion.ToString(dtQty.Rows[0]["BalQty"]);
						}
					}
				}
				else
				{
					SOId = 0;
					((Control)(object)cmbsupp).Text = string.Empty;
					((UltraGridBase)cmbsupp).DataSource = null;
					CmbOrderno.Text = string.Empty;
				}
				break;
			}
			case 84:
			case 1606:
			{
				int Code = InvDeliveryOrder.DeliveryOrderNoCheckinGatePass(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = DocumentTypeId,
					DocNo = Conversion.ToInt((object)CmbOrderno.Text.Trim().ToString()),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					BranchesId = UserAccount.BranchesId
				});
				if (Code > 0)
				{
					SOId = Code;
					DataTable Weight = InvDeliveryOrder.GetTotalWeightFromDeliveryOrder(new InvDeliveryOrder
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = DocumentTypeId,
						Id = SOId
					});
					if (Weight.Rows.Count > 0)
					{
						for (int j = 0; j < Weight.Rows.Count; j++)
						{
							if (Conversion.ToString(Weight.Rows[j]["Container"]) != string.Empty)
							{
								if (j == 0)
								{
									txtContainerNo1.Text = Conversion.ToString(Weight.Rows[j]["Container"]);
								}
								if (Conversion.ToString(Weight.Rows[j]["Container"]) != Conversion.ToString((object)txtContainerNo1.Text))
								{
									txtContainerNo1.Enabled = false;
									txtContainerNo2.Enabled = false;
									txtContainerNo2.Text = Conversion.ToString(Weight.Rows[j]["Container"]);
									break;
								}
							}
						}
						DDL.BindDDLNew(Weight, CmbVariety, "ItemId", "ItemName", "Item Name", false);
						CmbVariety.Value = Conversion.ToInt(Weight.Rows[0]["ItemId"]);
						CmbWarehouse.Value = Weight.Rows[0]["WarehouseId"].ToString();
						txtContrainerWeight.Text = Weight.Rows[0]["TotalWeight"].ToString();
						txtqty.Text = Weight.Rows[0]["DoTotalQty"].ToString();
						string obj = Weight.Rows[0]["DeliveryOrderType"].ToString();
						txtvehicleno.Text = Weight.Rows[0]["VehicleNo"].ToString();
						((Control)(object)cmbvehicletype).Text = Weight.Rows[0]["VehicleType"].ToString();
						if (obj == "Export" && ((Control)(object)cmbgptype).Text != "Export")
						{
							MessageBox.Show("Please Select GatePass Type Export");
							CmbOrderno.Text = string.Empty;
							break;
						}
						DDL.BindDDLNew(Weight, cmbsupp, "Id", "CompanyName", "CustomerName", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
						DifferenceWeight();
						txtContrainerWeight.Enabled = false;
					}
					else
					{
						SOId = 0;
						Weight.Rows.Clear();
						txtContrainerWeight.Text = string.Empty;
						txtFactoryWeight.Text = string.Empty;
						((Control)(object)cmbsupp).Text = string.Empty;
						((UltraGridBase)cmbsupp).DataSource = null;
						txtContrainerWeight.Enabled = true;
					}
				}
				else
				{
					SOId = 0;
					CmbOrderno.Text = string.Empty;
					((Control)(object)cmbsupp).Text = string.Empty;
					((UltraGridBase)cmbsupp).DataSource = null;
				}
				grdPaymentTermFill(SOId, DocumentTypeId);
				break;
			}
			case 92:
			{
				BindAllSupplierCustomer();
				DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
				DDL.BindDDLNew(dtItem, CmbVariety, "Id", "ItemName", "Item Name", false);
				break;
			}
			case 59:
				SupplierItemBindForPurchaseReturn();
				break;
			}
		}
		catch (Exception ex)
		{
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BindAllSupplierCustomer()
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
				return;
			}
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdPaymentTermFill(int Id, int DocumentTypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		try
		{
			DataTable dt = InvDeliveryOrder.InvDeliveryOrder_GetSaleOrderWithPaymentTerm(new InvDeliveryOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				Id = Id
			});
			if (dt.Rows.Count > 0)
			{
				dtPtOrder.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtPtOrder.Rows.Add(dt.Rows[i]["OrderId"], dt.Rows[i]["OrderNo"], dt.Rows[i]["PaymentTerm"]);
				}
				panelPaymentTermOrder.Visible = true;
				grdPaymentTermOrder.DataSource = dtPtOrder;
				grdPaymentTermOrder.RetrieveStructure();
				grdPaymentTermOrder.RootTable.Columns["OrderId"].Visible = false;
				grdPaymentTermOrder.RootTable.Columns["OrderNo"].Width = 60;
				grdPaymentTermOrder.RootTable.Columns["PaymentTerm"].Width = 100;
				grdPaymentTermOrder.TotalRow = (InheritableBoolean)2;
				GridEXColumn column = new GridEXColumn();
				column = grdPaymentTermOrder.RootTable.Columns["PaymentTerm"];
				GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)0, (object)"Cash");
				fc.TargetColumn = column;
				fc.FormatStyle.FontBold = (TriState)2;
				fc.FormatStyle.ForeColor = Color.Red;
				grdPaymentTermOrder.RootTable.FormatConditions.Add(fc);
				column = grdPaymentTermOrder.RootTable.Columns["PaymentTerm"];
				fc = new GridEXFormatCondition(column, (ConditionOperator)0, (object)"Credit");
				fc.TargetColumn = column;
				fc.FormatStyle.FontBold = (TriState)2;
				fc.FormatStyle.ForeColor = Color.Green;
				grdPaymentTermOrder.RootTable.FormatConditions.Add(fc);
			}
			else
			{
				panelPaymentTermOrder.Visible = false;
				grdPaymentTermOrder.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DifferenceWeight()
	{
		try
		{
			double FactoryWeight = 0.0;
			if (txtContrainerWeight.Text != string.Empty && txtFactoryWeight.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtContrainerWeight.Text.Trim());
				FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
				txtDifferenceWeight.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(CmbWarehouse.Value);
			DataTable lst = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbWarehouse, "Id", "WareHouseName", "Godown Name", ZeroIndex: true);
				if (ID > 0)
				{
					if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbWarehouse.Value = ID;
					}
					else
					{
						((Control)(object)CmbWarehouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWarehouse).Text = string.Empty;
				((UltraGridBase)CmbWarehouse).DataSource = null;
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
			int ID = Conversion.ToInt(CmbVariety.Value);
			bool hasId = false;
			foreach (DataRow row in dtOrderType.Rows)
			{
				int id = Conversion.ToInt(row["Id"]);
				if (id == 1605 || id == 1606)
				{
					hasId = true;
					break;
				}
			}
			if (hasId)
			{
				dtItem = CommonServices.ItemGetForComboServiceBindForEngr();
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, CmbVariety, "Id", "ItemName", "Item Name", ZeroIndex: false);
					((UltraGridBase)CmbVariety).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
			else
			{
				dtItem = CommonServices.ItemGetForComboServiceBind();
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, CmbVariety, "Id", "ItemName", "Item Name", ZeroIndex: false);
					((UltraGridBase)CmbVariety).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
			if (ID > 0)
			{
				if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
				{
					CmbVariety.Value = ID;
				}
				else
				{
					((Control)(object)CmbVariety).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbGPBasedOn_Leave(object sender, EventArgs e)
	{
		try
		{
			CmbOrderno.Enabled = true;
			if (((Control)(object)CmbGPBasedOn).Text == "DeliverOrder")
			{
				txtPackUnit.TabStop = false;
				txtCompareWeight.TabStop = false;
			}
			else if (((Control)(object)CmbGPBasedOn).Text == "General")
			{
				txtPackUnit.TabStop = false;
				txtCompareWeight.TabStop = false;
				CmbOrderno.Enabled = false;
				CmbOrderno.Text = txtgpno.Text;
				CmbOrderno_Leave(null, null);
			}
			else if (((Control)(object)CmbGPBasedOn).Text == "Purchase Return")
			{
				txtPackUnit.TabStop = false;
				txtCompareWeight.TabStop = false;
				CmbOrderno.Enabled = false;
				CmbOrderno.Text = txtgpno.Text;
				CmbOrderno_Leave(null, null);
			}
			else
			{
				txtPackUnit.TabStop = true;
				txtCompareWeight.TabStop = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		txtvehicleno.Text = txtvehicleno.Text.ToUpper();
	}

	private void CmbVariety_Leave(object sender, EventArgs e)
	{
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbgptype).Text.Trim() == "" || ((Control)(object)cmbgptype).Text.Trim() == null || ((Control)(object)cmbgptype).Text.Trim() == "0" || Conversion.ToInt(cmbgptype.Value) == 0)
		{
			MessageBox.Show("Please select GatePass Type");
			cmbgptype.Focus();
			return false;
		}
		if (txtgpno.Text.Trim() == "0" || txtgpno.Text.Trim() == string.Empty || txtgpno.Text.Trim() == null)
		{
			MessageBox.Show("Please select Gate Pass No");
			txtgpno.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == "" || txtvehicleno.Text == null)
		{
			MessageBox.Show("Please Select Vehicle No");
			txtvehicleno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbvehicletype).ActiveRow == null || Conversion.ToInt(cmbvehicletype.Value) == 0)
		{
			MessageBox.Show("Please Select Vehicle Type");
			cmbvehicletype.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStatus).ActiveRow == null || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Please Select Status");
			CmbStatus.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWeighBridgeStatus).ActiveRow == null || Conversion.ToInt(cmbgptype.Value) == 0)
		{
			MessageBox.Show("Please Select WeighBridgeStatus");
			CmbWeighBridgeStatus.Focus();
			return false;
		}
		if (((Control)(object)CmbGPBasedOn).Text == "SaleOrder" || ((Control)(object)CmbGPBasedOn).Text == "SaleOrderStoreAndPm")
		{
			if (CmbOrderno.Text.Trim() == string.Empty || CmbOrderno.Text.Trim() == "0" || CmbOrderno.Text.Trim() == null)
			{
				MessageBox.Show("Please Enter Order#");
				CmbOrderno.Focus();
				return false;
			}
			if (((UltraGridBase)cmbsupp).ActiveRow == null || Conversion.ToInt(cmbsupp.Value) == 0)
			{
				MessageBox.Show("Please Select Customer Name");
				cmbsupp.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtPackUnit.Text) <= 0.0)
			{
				MessageBox.Show("PackUnit Field Required");
				txtPackUnit.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtCompareWeight.Text) <= 0.0)
			{
				MessageBox.Show("Weight Field Required");
				txtCompareWeight.Focus();
				return false;
			}
		}
		else if (((Control)(object)CmbGPBasedOn).Text == "DeliverOrder")
		{
			if (CmbOrderno.Text.Trim() == string.Empty || CmbOrderno.Text.Trim() == "0" || CmbOrderno.Text.Trim() == null)
			{
				MessageBox.Show("Please Enter Delivery Order #");
				CmbOrderno.Focus();
				return false;
			}
			if (txtContrainerWeight.Text.Trim() == string.Empty || txtContrainerWeight.Text.Trim() == "0" || txtContrainerWeight.Text.Trim() == null)
			{
				MessageBox.Show("Container Weight field required");
				txtContrainerWeight.Focus();
				return false;
			}
		}
		return true;
	}

	private void SupplierItemBindForPurchaseReturn()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtCombo = InvPurchaseInvoice.AllComboBindAgainstPurchaseInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "56,57,64,702"
			});
			if (dtCombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id");
			dtItemName.Columns.Add("Name");
			dtItemName.Clear();
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("Name");
			dtSupp.Clear();
			new HashSet<int> { 1, 2, 4 };
			for (int i = 0; i < dtCombo.Rows.Count; i++)
			{
				string activity = Conversion.ToString(dtCombo.Rows[i]["Activity"]);
				Convert.ToInt32(dtCombo.Rows[i]["Id"]);
				if (!(activity == "ItemName"))
				{
					if (activity == "Supplier")
					{
						dtSupp.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
					}
				}
				else
				{
					dtItemName.Rows.Add(dtCombo.Rows[i]["Id"], dtCombo.Rows[i]["ReferenceName"]);
				}
			}
			DDL.BindDDL(dtItemName, CmbVariety, "Id", "Name", "Item Name", ZeroIndex: true);
			DDL.BindDDL(dtSupp, cmbsupp, "Id", "Name", "Customer Name", ZeroIndex: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			GatePassOutward gatePass = new GatePassOutward();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			gatePass.GpDate = txtgpdate.Value;
			gatePass.GpSrNo = Conversion.ToInt((object)txtgpno.Text);
			gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
			gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
			gatePass.RefDocumentTypeId = Conversion.ToInt(CmbGPBasedOn.Value);
			gatePass.OtherSupCust = ((Control)(object)CmbGPBasedOn).Text.Trim().ToString();
			gatePass.SupplierContractCode = CmbOrderno.Text.Trim();
			gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupp.Value);
			gatePass.ItemId = Conversion.ToInt(CmbVariety.Value);
			gatePass.VarietyName = ((Control)(object)CmbVariety).Text.Trim();
			gatePass.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			gatePass.NoOfPackages = Conversion.ToInt((object)txtqty.Text.Trim());
			if (((Control)(object)CmbGPBasedOn).Text == "SaleOrder" || ((Control)(object)CmbGPBasedOn).Text == "SaleOrderStoreAndPm")
			{
				gatePass.PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
				gatePass.WeightCommapredToSoWt = Conversion.ToDouble((object)txtCompareWeight.Text.Trim());
			}
			else
			{
				DifferenceWeight();
			}
			gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			gatePass.VehicleNo = txtvehicleno.Text.Trim();
			gatePass.BiltyNo = txtbiltyno.Text.Trim();
			gatePass.BiltyDate = Conversion.ToDateTime((object)txtBiltyDate.Value);
			if (txtfreight.Text.Trim() != string.Empty)
			{
				gatePass.Freight = float.Parse(txtfreight.Text.ToString());
			}
			else
			{
				gatePass.Freight = 0.0;
			}
			if (txtNetPaid.Text.Trim() != string.Empty && txtNetPaid.Text.Trim() != null)
			{
				gatePass.NetPaid = float.Parse(txtNetPaid.Text.Trim());
			}
			else
			{
				gatePass.NetPaid = 0.0;
			}
			if (((Control)(object)CmbStatus).Text != "Open")
			{
				throw new Exception("Status Not Change");
			}
			gatePass.Status = ((Control)(object)CmbStatus).Text.Trim().ToString();
			gatePass.CityId = Conversion.ToInt(cmbcity.Value);
			gatePass.InDateTimeStamp = txtintime.Value;
			gatePass.OtherRemarks = txtremarks.Text.Trim();
			gatePass.OutDateTimeStamp = txtouttime.Value;
			if (txtContrainerWeight.Text != string.Empty && txtContrainerWeight.Text != null)
			{
				gatePass.SupplierWeight = Convert.ToDouble(txtContrainerWeight.Text.Trim());
			}
			else
			{
				gatePass.SupplierWeight = Convert.ToDouble(0);
			}
			if (txtFactoryWeight.Text.Trim() != string.Empty && txtFactoryWeight.Text.Trim() != null)
			{
				gatePass.FactoryWeight = float.Parse(txtFactoryWeight.Text.Trim());
			}
			else
			{
				gatePass.FactoryWeight = 0.0;
			}
			if (txtDifferenceWeight.Text != string.Empty && txtDifferenceWeight.Text != null)
			{
				gatePass.DifferenceWeight = Convert.ToDouble(txtDifferenceWeight.Text.Trim());
			}
			else
			{
				gatePass.DifferenceWeight = Convert.ToDouble(0);
			}
			gatePass.WeighBridgeId = Conversion.ToInt((object)txtWeighBridgeSlipNo.Text.Trim());
			if (gatePass.OtherSupCust != "General" && gatePass.OtherSupCust != "Purchase Return")
			{
				if (SOId == 0)
				{
					CmbOrderno.Text = string.Empty;
					throw new Exception("Order No Not Found");
				}
				gatePass.SaleOrderId = SOId;
			}
			gatePass.DocAttachment = ((Control)(object)CmbWeighBridgeStatus).Text.Trim();
			gatePass.Container = txtContainerNo1.Text.Trim();
			gatePass.Container1 = txtContainerNo2.Text.Trim();
			gatePass.SealNo = txtSealNo1.Text.Trim();
			gatePass.SealNo1 = txtSealNo2.Text.Trim();
			gatePass.DocumentTypeId = 91;
			gatePass.EntryUser = UserAccount.ID;
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.InDateTimeStamp = DateTime.Now;
			gatePass.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			gatePass.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GatePassOutward>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = GatePassOutward.Save(gatePass);
			if (success > 0)
			{
				MessageBox.Show("Record Save Successfully" + gatePass.GpSrNo);
			}
			Reset();
			OpenLinkedFormOnInsert(success, "");
			if (ChkBox.Checked)
			{
				CommonServices.GatePassOutwardSlipandRegister(success);
			}
			if (chkPrintPreview290.Checked)
			{
				Print_290(success);
			}
			if (chkDriverInfoForm.Checked)
			{
				if (Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Select((tblUserRights r) => r.ScreenName == "frmDriverBioForOutWard")))
				{
					frmDriverBio obj = new frmDriverBio(UserAccount)
					{
						RefDocumentTypeId = 91
					};
					obj.cmbGatePass.Value = success;
					obj.cmbGatePass_TextChanged(null, null);
					obj.Show();
				}
				else
				{
					if (!Conversion.ToBool((object)clsGlobalVariables.ScreenViewReights.Select((tblUserRights r) => r.ScreenName == "frmDriverBio")))
					{
						MessageBox.Show("You Don't Have View-right Of This Form..");
						return;
					}
					frmDriverBio obj2 = new frmDriverBio(UserAccount)
					{
						RefDocumentTypeId = 0
					};
					obj2.cmbGatePass.Value = success;
					obj2.cmbGatePass_TextChanged(null, null);
					obj2.Show();
				}
			}
			if (RadGrnFormToOpenOnInsert.Checked)
			{
				RadNoneFormToOpenOnInsert.Checked = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Expected O, but got Unknown
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Expected O, but got Unknown
		try
		{
			Id = ID;
			GatePassOutward gatePass = GatePassOutward.GetByID(Id);
			RadGrnFormToOpenOnInsert.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtbiltyno.Text = gatePass.BiltyNo;
			txtBiltyDate.Value = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.BiltyDate)) ? gatePass.GpDate : Conversion.ToDateTime((object)gatePass.BiltyDate));
			((Control)(object)cmbcity).Text = Conversion.ToString((object)gatePass.CityId);
			txtfreight.Text = gatePass.Freight.ToString();
			((Control)(object)cmbgptype).Text = gatePass.GatepassType.ToString();
			txtgpdate.Value = gatePass.GpDate;
			txtgpno.Text = gatePass.GpSrNo.ToString();
			txtgptypeno.Text = gatePass.GpTypeSrNo.ToString();
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.InDateTimeStamp)))
			{
				txtintime.Value = Conversion.ToDateTime((object)gatePass.InDateTimeStamp);
			}
			if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.OutDateTimeStamp)))
			{
				txtouttime.Value = Conversion.ToDateTime((object)gatePass.OutDateTimeStamp);
			}
			txtqty.Text = gatePass.NoOfPackages.ToString();
			Qty = gatePass.NoOfPackages;
			txtremarks.Text = gatePass.OtherRemarks;
			((Control)(object)CmbGPBasedOn).Text = gatePass.OtherSupCust;
			txtouttime.Value = DateTime.Now;
			CmbOrderno.Text = gatePass.SupplierContractCode;
			txtvehicleno.Text = gatePass.VehicleNo;
			((Control)(object)cmbvehicletype).Text = gatePass.VehicleType;
			int DocumentTypeId = Conversion.ToInt(CmbGPBasedOn.Value);
			if (DocumentTypeId == 81 || DocumentTypeId == 90)
			{
				DataTable lst = SupplierCustomer.GetSupplierBySaleOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					OrderId = Conversion.ToInt((object)CmbOrderno.Text),
					DocumentTypeId = DocumentTypeId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (lst.Rows.Count > 0)
				{
					SOId = Conversion.ToInt((object)lst.Rows[0]["SaleOrderId"].ToString());
					DataTable dtItemDetail = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
					{
						Id = SOId
					});
					if (dtItemDetail.Rows.Count > 0)
					{
						DataTable itemdt = new DataTable();
						itemdt.Columns.Add("ItemId");
						itemdt.Columns.Add("ItemName");
						itemdt.Columns.Add("Equivalent");
						for (int j = 0; j < dtItemDetail.Rows.Count; j++)
						{
							itemdt.Rows.Add(dtItemDetail.Rows[j]["OrderItemId"], dtItemDetail.Rows[j]["ItemName"], dtItemDetail.Rows[j]["Equivalent"]);
						}
						DDL.BindDDL(itemdt, CmbVariety, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)CmbVariety).DisplayLayout.Bands[0].Columns[2].Hidden = true;
						((UltraGridBase)CmbVariety).Rows[0].Activate();
					}
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("CompanyName");
					for (int i = 0; i < lst.Rows.Count; i++)
					{
						dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["CompanyName"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, cmbsupp, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
					}
				}
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 84)
			{
				SOId = gatePass.SaleOrderId;
				DataTable Weight = InvDeliveryOrder.GetTotalWeightFromDeliveryOrder(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 84,
					Id = SOId
				});
				if (Weight.Rows.Count > 0)
				{
					DDL.BindDDLNew(Weight, cmbsupp, "Id", "CompanyName", "CustomerName", false);
					((UltraGridBase)cmbsupp).Rows[0].Activate();
					DDL.BindDDLNew(Weight, CmbVariety, "ItemId", "ItemName", "ItemName", false);
				}
				grdPaymentTermFill(SOId, 84);
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 1605)
			{
				DataTable lst2 = SupplierCustomer.GetSupplierBySaleOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					OrderId = Conversion.ToInt((object)CmbOrderno.Text),
					DocumentTypeId = 1605,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (lst2.Rows.Count > 0)
				{
					SOId = Conversion.ToInt((object)lst2.Rows[0]["SaleOrderId"].ToString());
					DataTable dt2 = new DataTable();
					dt2.Columns.Add("Id");
					dt2.Columns.Add("CompanyName");
					for (int k = 0; k < lst2.Rows.Count; k++)
					{
						dt2.Rows.Add(lst2.Rows[k]["Id"], lst2.Rows[k]["CompanyName"]);
					}
					if (dt2.Rows.Count > 0)
					{
						DDL.BindDDL(dt2, cmbsupp, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
					}
				}
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 1606)
			{
				SOId = gatePass.SaleOrderId;
				DataTable Weight2 = InvDeliveryOrder.GetTotalWeightFromDeliveryOrder(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 1606,
					Id = SOId
				});
				if (Weight2.Rows.Count > 0)
				{
					DDL.BindDDLNew(Weight2, cmbsupp, "Id", "CompanyName", "CustomerName", false);
					((UltraGridBase)cmbsupp).Rows[0].Activate();
					DDL.BindDDLNew(Weight2, CmbVariety, "ItemId", "ItemName", "ItemName", false);
				}
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 92)
			{
				txtPackUnit.TabStop = false;
				txtCompareWeight.TabStop = false;
				CmbOrderno.Enabled = false;
				CmbOrderno.Text = txtgpno.Text;
				DataTable dtCustomer = CommonServices.SupplierCustomerGetAllServiceBind();
				DDL.BindDDLNew(dtCustomer, cmbsupp, "Id", "CompanyName", "Customer Name", false);
				DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
				DDL.BindDDLNew(dtItem, CmbVariety, "Id", "ItemName", "Item Name", false);
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 59)
			{
				txtPackUnit.TabStop = false;
				txtCompareWeight.TabStop = false;
				CmbOrderno.Enabled = false;
				CmbOrderno.Text = txtgpno.Text;
				SupplierItemBindForPurchaseReturn();
			}
			cmbsupp.Value = Conversion.ToInt((object)gatePass.SupplierCustomerId);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)gatePass.Status);
			txtNetPaid.Text = gatePass.NetPaid.ToString();
			((Control)(object)CmbVariety).Text = gatePass.VarietyName;
			txtContrainerWeight.Text = gatePass.SupplierWeight.ToString();
			txtWeighBridgeSlipNo.Text = gatePass.WeighBridgeId.ToString();
			txtFactoryWeight.Text = gatePass.FactoryWeight.ToString();
			((Control)(object)CmbWarehouse).Text = gatePass.WarehouseId.ToString();
			txtContainerNo1.Text = gatePass.Container1;
			txtContainerNo2.Text = gatePass.Container;
			txtSealNo1.Text = gatePass.SealNo;
			txtSealNo2.Text = gatePass.SealNo1;
			txtApprovedBy.Text = gatePass.ApprovedUserName;
			((Control)(object)CmbWeighBridgeStatus).Text = Conversion.ToString((object)gatePass.DocAttachment);
			txtDiffWeightRemarks.Text = Conversion.ToString((object)gatePass.WeightDiffRemarks);
			txtPackUnit.Text = Conversion.ToString((object)gatePass.PackUnit);
			txtCompareWeight.Text = Conversion.ToString((object)gatePass.WeightCommapredToSoWt);
			Approve = gatePass.IsApproved;
			txtDiffWeightRemarks.ReadOnly = false;
			DifferenceWeight();
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = false;
				CmbStatus.Value = 2;
			}
			else
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = true;
			}
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnupdate.Visible = true;
			btnsave.Visible = false;
			UpdateMode = true;
			GetTicketNoandWeighBridgeWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			if (!formvalidation() || MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GatePassOutward gatePass = new GatePassOutward();
			gatePass.Id = Id;
			gatePass.BiltyNo = txtbiltyno.Text.Trim();
			gatePass.BiltyDate = Conversion.ToDateTime((object)txtBiltyDate.Value);
			gatePass.CityId = Conversion.ToInt(cmbcity.Value);
			gatePass.DocumentTypeId = 91;
			if (txtfreight.Text.Trim() != string.Empty && txtfreight.Text.Trim() != null)
			{
				gatePass.Freight = float.Parse(txtfreight.Text.ToString());
			}
			else
			{
				gatePass.Freight = 0.0;
			}
			gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
			gatePass.GpDate = txtgpdate.Value;
			gatePass.GpSrNo = Conversion.ToInt((object)txtgpno.Text);
			gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
			gatePass.InDateTimeStamp = txtintime.Value;
			gatePass.NoOfPackages = Conversion.ToInt((object)txtqty.Text.Trim());
			gatePass.OtherRemarks = txtremarks.Text.Trim();
			gatePass.OutDateTimeStamp = DateTime.Now;
			gatePass.SupplierContractCode = CmbOrderno.Text.Trim();
			gatePass.VehicleNo = txtvehicleno.Text.Trim();
			gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupp.Value);
			gatePass.Status = ((Control)(object)CmbStatus).Text.ToString();
			if (txtNetPaid.Text.Trim() != string.Empty && txtNetPaid.Text.Trim() != null)
			{
				gatePass.NetPaid = float.Parse(txtNetPaid.Text.Trim());
			}
			else
			{
				gatePass.NetPaid = 0.0;
			}
			if (txtFactoryWeight.Text.Trim() != string.Empty && txtFactoryWeight.Text.Trim() != null && txtFactoryWeight.Text.Trim() != "0")
			{
				gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			}
			else if (((Control)(object)CmbStatus).Text == "Accepted" && ((Control)(object)CmbGPBasedOn).Text != "SaleOrderStoreAndPm")
			{
				throw new Exception("Factory Weight field required");
			}
			gatePass.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			gatePass.WeighBridgeId = Conversion.ToInt((object)txtWeighBridgeSlipNo.Text.Trim());
			gatePass.ItemId = Conversion.ToInt(CmbVariety.Value);
			gatePass.VarietyName = ((Control)(object)CmbVariety).Text.Trim();
			gatePass.RefDocumentTypeId = Conversion.ToInt(CmbGPBasedOn.Value);
			gatePass.OtherSupCust = ((Control)(object)CmbGPBasedOn).Text.Trim().ToString();
			if (((Control)(object)CmbGPBasedOn).Text == "SaleOrder" || ((Control)(object)CmbGPBasedOn).Text == "SaleOrderStoreAndPm")
			{
				gatePass.PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
				gatePass.WeightCommapredToSoWt = Conversion.ToDouble((object)txtCompareWeight.Text.Trim());
			}
			else
			{
				DifferenceWeight();
			}
			if (txtContrainerWeight.Text != string.Empty && txtContrainerWeight.Text != null)
			{
				gatePass.SupplierWeight = Convert.ToDouble(txtContrainerWeight.Text.Trim());
			}
			else
			{
				gatePass.SupplierWeight = Convert.ToDouble(0);
			}
			if (txtDifferenceWeight.Text != string.Empty && txtDifferenceWeight.Text != null)
			{
				gatePass.DifferenceWeight = Convert.ToDouble(txtDifferenceWeight.Text.Trim());
			}
			else
			{
				gatePass.DifferenceWeight = Convert.ToDouble(0);
			}
			if (gatePass.OtherSupCust != "General" && gatePass.OtherSupCust != "Purchase Return")
			{
				if (SOId == 0)
				{
					CmbOrderno.Text = string.Empty;
					throw new Exception("Order No Not Found");
				}
				gatePass.SaleOrderId = SOId;
			}
			gatePass.Container = txtContainerNo1.Text.Trim();
			gatePass.Container1 = txtContainerNo2.Text.Trim();
			gatePass.SealNo = txtSealNo1.Text.Trim();
			gatePass.SealNo1 = txtSealNo2.Text.Trim();
			gatePass.ModifyUser = UserAccount.ID;
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.DocAttachment = ((Control)(object)CmbWeighBridgeStatus).Text.Trim();
			gatePass.IsApproved = Approve;
			gatePass.WeightDiffRemarks = Conversion.ToString((object)txtDiffWeightRemarks.Text.Trim());
			gatePass.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			gatePass.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GatePassOutward>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = GatePassOutward.Save(gatePass);
			if (success > 0)
			{
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 91, 0);
				}
				MessageBox.Show("Record Update Successfully" + gatePass.GpSrNo);
			}
			Reset();
			OpenLinkedFormOnInsert(success, gatePass.Status);
			if (ChkBox.Checked)
			{
				CommonServices.GatePassOutwardSlipandRegister(success);
			}
			if (chkPrintPreview290.Checked)
			{
				Print_290(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtgpno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				GpId = Id;
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
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
			Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			GpId = Id;
			ReadById(Id);
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassOutward.GetHistoryonlystatusOpen(new GatePassOutward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 91,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtOpenGatePass = new DataTable();
				dtOpenGatePass.Columns.Add("Id", typeof(int));
				dtOpenGatePass.Columns.Add("GpDate", typeof(DateTime));
				dtOpenGatePass.Columns.Add("GpNo", typeof(int));
				dtOpenGatePass.Columns.Add("GatepassType");
				dtOpenGatePass.Columns.Add("OrderType");
				dtOpenGatePass.Columns.Add("OrderNo");
				dtOpenGatePass.Columns.Add("Customer");
				dtOpenGatePass.Columns.Add("ItemName");
				dtOpenGatePass.Columns.Add("Warehouse");
				dtOpenGatePass.Columns.Add("Qty", typeof(double));
				dtOpenGatePass.Columns.Add("VehicleType");
				dtOpenGatePass.Columns.Add("VehicleNo");
				dtOpenGatePass.Columns.Add("BiltyNo");
				dtOpenGatePass.Columns.Add("BiltyDate", typeof(DateTime));
				dtOpenGatePass.Columns.Add("Status");
				dtOpenGatePass.Columns.Add("CityName");
				dtOpenGatePass.Columns.Add("FactoryWeight", typeof(double));
				dtOpenGatePass.Columns.Add("ApprovedBy");
				dtOpenGatePass.Columns.Add("Remaks");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtOpenGatePass.Rows.Add(dt.Rows[i][Id], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GpSrNo"], dt.Rows[i]["GatepassType"], dt.Rows[i]["OrderType"], dt.Rows[i]["OrderNo"], dt.Rows[i]["CustomerName"], dt.Rows[i]["VarietyName"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["NoOfBags"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["BiltyDate"], dt.Rows[i]["Status"], dt.Rows[i]["CityName"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["ApprovedBy"], dt.Rows[i]["OtherRemarks"]);
				}
				grd.DataSource = dtOpenGatePass;
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
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3);
			grd.RootTable.Columns["BiltyDate"].FormatString = "dd-MM-yyyy";
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 40;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void DeliveryOrderGridFill()
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
			obj.DocumentTypeIds = "84,1606";
			if (FromDateDoInfo.Checked)
			{
				obj.FromDate = FromDateDoInfo.Value;
			}
			if (ToDateDoInfo.Checked)
			{
				obj.ToDate = ToDateDoInfo.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocDoInfo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocDoInfo.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerDoInfo.Value);
			DataTable dtGrid = InvDeliveryOrder.DeliveryOrderDetailForGatePassOutward(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dtDeliveryOrder = new DataTable();
				dtDeliveryOrder.Columns.Add("Id");
				dtDeliveryOrder.Columns.Add("DocumentTypeId");
				dtDeliveryOrder.Columns.Add("DocDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("DoNo");
				dtDeliveryOrder.Columns.Add("DeliveryOrderType");
				dtDeliveryOrder.Columns.Add("InvoiceNo");
				dtDeliveryOrder.Columns.Add("SaleOrderNo");
				dtDeliveryOrder.Columns.Add("CustomerName");
				dtDeliveryOrder.Columns.Add("ItemName");
				dtDeliveryOrder.Columns.Add("DoQty", typeof(double));
				dtDeliveryOrder.Columns.Add("PackUOM");
				dtDeliveryOrder.Columns.Add("DoWeight", typeof(double));
				dtDeliveryOrder.Columns.Add("EntryDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("EntryUser");
				dtDeliveryOrder.Columns.Add("ModifyDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("ModifyUser");
				dtDeliveryOrder.Columns.Add("Remarks");
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtDeliveryOrder.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["DocumentTypeId"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["DoNo"], dtGrid.Rows[i]["DeliveryOrderType"], dtGrid.Rows[i]["InvoiceNo"], dtGrid.Rows[i]["SaleOrderNo"], dtGrid.Rows[i]["CustomerName"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["DoQty"], dtGrid.Rows[i]["PackUOM"], dtGrid.Rows[i]["DoWeight"], Conversion.ToDateTime(dtGrid.Rows[i]["EntryDate"]).ToShortDateString(), dtGrid.Rows[i]["UserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ModifyUser"], dtGrid.Rows[i]["LoadingRemarks"]);
				}
				grdDoInformation.DataSource = dtDeliveryOrder;
				grdDoInformation.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdDoInformation.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdDoInformation.ColumnAutoResize = false;
			grdDoInformation.RootTable.Columns["Id"].Visible = false;
			grdDoInformation.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdDoInformation.RootTable.Columns["DoNo"].ColumnType = (ColumnType)5;
			grdDoInformation.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdDoInformation.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdDoInformation.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			grdDoInformation.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			grdDoInformation.RootTable.Columns["DoQty"].FormatString = "#,##0.###";
			grdDoInformation.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.###";
			grdDoInformation.RootTable.Columns["DoWeight"].AggregateFunction = (AggregateFunction)2;
			grdDoInformation.RootTable.Columns["DoWeight"].TextAlignment = (TextAlignment)3;
			grdDoInformation.RootTable.Columns["DoWeight"].FormatString = "#,##0.###";
			grdDoInformation.RootTable.Columns["DoWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdDoInformation);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDoInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.InvDeliveryOrderSlip(Conversion.ToInt(grdDoInformation.CurrentRow.Cells["Id"].Value), Conversion.ToInt(grdDoInformation.CurrentRow.Cells["DocumentTypeId"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowDoInfo_Click(object sender, EventArgs e)
	{
		try
		{
			DeliveryOrderGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetDoInfo_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDoInfo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerDoInFoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstDeliveryOrder(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCustomerDoInfo, "Id", "CompanyName", "Customer Name", false);
				((UltraGridBase)CmbCustomerDoInfo).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDoInfo()
	{
		try
		{
			txtFromDocDoInfo.Text = string.Empty;
			txtToDocDoInfo.Text = string.Empty;
			((Control)(object)CmbCustomerDoInfo).Text = string.Empty;
			FromDateDoInfo.Value = DateTime.Now.AddDays(-3.0);
			FromDateDoInfo.Focus();
			grdDoInformation.ClearStructure();
			CustomerDoInFoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrderGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = "81,1605,90";
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.BranchesId = UserAccount.BranchesId;
			if (FromDateSoInfo.Checked)
			{
				obj.FromDate = FromDateSoInfo.Value;
			}
			if (ToDateSoInfo.Checked)
			{
				obj.ToDate = ToDateSoInfo.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocSoInfo.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocSoInfo.Text);
			obj.SupplierCustomerId = Conversion.ToInt(cmbCustomerSoInfo.Value);
			DataTable dtGrid = InvDeliveryOrder.DeliveryOrderDetailForGatePassOutward(obj);
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dtSaleOrder = new DataTable();
				dtSaleOrder.Columns.Add("Id", typeof(int));
				dtSaleOrder.Columns.Add("DocumentTypeId", typeof(int));
				dtSaleOrder.Columns.Add("DocumentType");
				dtSaleOrder.Columns.Add("DocDate", typeof(DateTime));
				dtSaleOrder.Columns.Add("OrderNo");
				dtSaleOrder.Columns.Add("BranchSrNo", typeof(int));
				dtSaleOrder.Columns.Add("BranchName");
				dtSaleOrder.Columns.Add("CustomerName");
				dtSaleOrder.Columns.Add("ItemName");
				dtSaleOrder.Columns.Add("ItemQty", typeof(double));
				dtSaleOrder.Columns.Add("PackUOM");
				dtSaleOrder.Columns.Add("NetWeight", typeof(double));
				dtSaleOrder.Columns.Add("DispatchQty", typeof(double));
				dtSaleOrder.Columns.Add("DispatchWeight", typeof(double));
				dtSaleOrder.Columns.Add("BalQty", typeof(double));
				dtSaleOrder.Columns.Add("BalWeight", typeof(double));
				dtSaleOrder.Columns.Add("Status");
				dtSaleOrder.Columns.Add("EntryDate", typeof(DateTime));
				dtSaleOrder.Columns.Add("EntryUser");
				dtSaleOrder.Columns.Add("ModifyDate", typeof(DateTime));
				dtSaleOrder.Columns.Add("ModifyUser");
				dtSaleOrder.Columns.Add("Remarks");
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtSaleOrder.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["DocumentTypeId"], dtGrid.Rows[i]["DocumentTypeDescription"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["OrderNo"], dtGrid.Rows[i]["BranchSrNo"], dtGrid.Rows[i]["BranchName"], dtGrid.Rows[i]["CustomerName"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["ItemQty"], dtGrid.Rows[i]["PackUOM"], dtGrid.Rows[i]["NetWeight"], dtGrid.Rows[i]["DispatchQty"], dtGrid.Rows[i]["DispatchWeight"], dtGrid.Rows[i]["BalQty"], dtGrid.Rows[i]["BalWeight"], dtGrid.Rows[i]["OrderStatus"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["UserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ModifyUser"], dtGrid.Rows[i]["OrderRemarks"]);
				}
				GrdSaleOrderInformation.DataSource = dtSaleOrder;
				GrdSaleOrderInformation.RetrieveStructure();
				GridSettingForSaleOrder();
			}
			else
			{
				GrdSaleOrderInformation.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForSaleOrder()
	{
		try
		{
			GrdSaleOrderInformation.RootTable.Columns["Id"].Visible = false;
			GrdSaleOrderInformation.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(GrdSaleOrderInformation, 2, 3);
			GrdSaleOrderInformation.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdSaleOrderInformation.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdSaleOrderInformation.RootTable.Columns["ItemQty"].Caption = "Order Qty";
			GrdSaleOrderInformation.RootTable.Columns["NetWeight"].Caption = "Order Weight";
			GrdSaleOrderInformation.RootTable.Columns["CustomerName"].Width = 300;
			GrdSaleOrderInformation.RootTable.Columns["ItemName"].Width = 300;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShowSoInfo_Click(object sender, EventArgs e)
	{
		try
		{
			SaleOrderGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnResetSoInfo_Click(object sender, EventArgs e)
	{
		try
		{
			ResetSoInfo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerSoInFoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstSaleOrder(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCustomerSoInfo, "Id", "CompanyName", "Customer Name", false);
				((UltraGridBase)cmbCustomerSoInfo).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetSoInfo()
	{
		try
		{
			txtFromDocSoInfo.Text = string.Empty;
			txtToDocSoInfo.Text = string.Empty;
			((Control)(object)cmbCustomerSoInfo).Text = string.Empty;
			FromDateSoInfo.Value = DateTime.Now.AddDays(-3.0);
			FromDateSoInfo.Focus();
			GrdSaleOrderInformation.ClearStructure();
			CustomerSoInFoFill();
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
			SOId = 0;
			Id = 0;
			GpId = 0;
			Approve = false;
			txtPackUnit.Text = "0";
			txtCompareWeight.Text = "0";
			UpdateMode = false;
			txtDiffWeightRemarks.Text = string.Empty;
			txtApprovedBy.Text = string.Empty;
			txtgptypeno.Text = string.Empty;
			CmbOrderno.Text = string.Empty;
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			((Control)(object)cmbvehicletype).Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtbiltyno.Text = string.Empty;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			txtqty.Text = string.Empty;
			txtContrainerWeight.Text = "0";
			txtfreight.Text = string.Empty;
			txtContainerNo1.Text = string.Empty;
			txtSealNo1.Text = string.Empty;
			txtSealNo2.Text = string.Empty;
			txtFactoryWeight.Text = "0";
			txtDifferenceWeight.Text = "0";
			txtContainerNo2.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtWeighBridgeSlipNo.Text = string.Empty;
			gpnofill();
			grdfrmfill();
			txtNetPaid.Text = string.Empty;
			btnsave.Visible = true;
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbgptype_Leave_1(null, null);
			((Control)(object)CmbGPBasedOn).Enabled = true;
			((Control)(object)cmbgptype).Enabled = true;
			CmbOrderno.Enabled = true;
			((Control)(object)CmbWeighBridgeStatus).Enabled = true;
			((Control)(object)CmbVariety).Text = string.Empty;
			((UltraGridBase)CmbVariety).DataSource = null;
			((UltraGridBase)CmbWeighBridgeStatus).Rows[0].Activate();
			CmbOrderno.Enabled = true;
			RadGrnFormToOpenOnInsert.Visible = false;
			txtgpdate.Focus();
			grdPaymentTermOrder.ClearStructure();
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
			if (!Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("SO Compulsory in GPO")))
			{
				SupplierFill();
			}
			if (((Control)(object)CmbGPBasedOn).Text == "General")
			{
				List<string> allServices = new List<string> { "SupplierCustomerLists", "VendorsAndCustomersForTransporters" };
				DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
				BindAllSupplierCustomer();
			}
			ItemNameFill();
			gatepasstype();
			vehicleTypefill();
			CityFill();
			WarehouseFill();
			grdfrmfill();
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

	private void OutwardGatePass_FormClosing(object sender, FormClosingEventArgs e)
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

	private void CustomerHistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstOutwarGP(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomerHistory, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)CmbCustomerHistory).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			txtFromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			txtFromDateHistory.Focus();
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
				txtFromDateHistory.Value = DateTime.Now.AddDays(-3.0);
				txtFromDateHistory.Focus();
			}
			else
			{
				txtgpdate.Focus();
			}
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		CustomerHistoryFill();
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Reset();
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			GpId = Id;
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), base.Name);
				}
			}
			if (!(e.Column.Key == "DeliveryOrderNo"))
			{
				return;
			}
			DeliveryOrder obj = new DeliveryOrder(UserAccount);
			if (clsGlobalVariables.ScreenViewReights.Count > 0)
			{
				if (bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "DeliveryOrder").ToList()[0].Value.ToString()))
				{
					obj.Show();
					obj.ReadById(Conversion.ToInt(grdhistory.CurrentRow.Cells[2].Value));
				}
				else
				{
					MessageBox.Show("Please Check Screen Rights");
				}
			}
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
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 91;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.GpDateF = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.GpDateT = txtToDateHistory.Value;
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
			obj.GpSrNoF = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.GpSrNoT = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable getList = GatePassOutward.GatepassHistory(obj);
			if (getList.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("GatepassType");
				dt.Columns.Add("OrderType");
				dt.Columns.Add("GpSrNo", typeof(int));
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("DeliveryOrderNo");
				dt.Columns.Add("OrderNo");
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("VarietyName");
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("VehicleType");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
				dt.Columns.Add("BiltyDate", typeof(DateTime));
				dt.Columns.Add("InTime", typeof(DateTime));
				dt.Columns.Add("OutTime", typeof(DateTime));
				dt.Columns.Add("CityName");
				dt.Columns.Add("Freight", typeof(double));
				dt.Columns.Add("NetPaid", typeof(double));
				dt.Columns.Add("Status");
				dt.Columns.Add("SupplierWeight", typeof(double));
				dt.Columns.Add("FactoryWeight", typeof(double));
				dt.Columns.Add("DifferenceWeight", typeof(double));
				dt.Columns.Add("WeighBridgeStatus");
				dt.Columns.Add("Container1");
				dt.Columns.Add("Container2");
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("OtherRemarks");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < getList.Rows.Count; i++)
				{
					dt.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["GatepassType"], getList.Rows[i]["OrderType"], getList.Rows[i]["GpSrNo"], Conversion.ToDateTime(getList.Rows[i]["GpDate"]).ToString("dd-MMM-yyyy"), getList.Rows[i]["DeliveryOrderNo"], getList.Rows[i]["OrderNo"], getList.Rows[i]["CompanyName"], getList.Rows[i]["VarietyName"], getList.Rows[i]["ItemQty"], getList.Rows[i]["VehicleType"], getList.Rows[i]["VehicleNo"], getList.Rows[i]["BiltyNo"], getList.Rows[i]["BiltyDate"], Conversion.ToDateTime(getList.Rows[i]["InTime"]).ToString("dd-MMM-yyyy HH:mm:ss"), Conversion.ToDateTime(getList.Rows[i]["OutTime"]).ToString("dd-MMM-yyyy HH:mm:ss"), getList.Rows[i]["Description"], getList.Rows[i]["Freight"], getList.Rows[i]["NetPaid"], getList.Rows[i]["Status"], getList.Rows[i]["SupplierWeight"], getList.Rows[i]["FactoryWeight"], getList.Rows[i]["DifferenceWeight"], getList.Rows[i]["WeighBridgeStatus"], getList.Rows[i]["Container"], getList.Rows[i]["Container1"], getList.Rows[i]["UserName"], getList.Rows[i]["EntryDate"], getList.Rows[i]["ModifyUserName"], getList.Rows[i]["ModifyDate"], getList.Rows[i]["IsApproved"], getList.Rows[i]["OtherRemarks"], getList.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dt;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistorysetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["IsApproved"].Visible = false;
			grdhistory.RootTable.Columns["GpSrNo"].Caption = "Gp No";
			grdhistory.RootTable.Columns["VarietyName"].Caption = "Item Name";
			grdhistory.RootTable.Columns["BiltyDate"].FormatString = "dd-MM-yyyy";
			grdhistory.RootTable.Columns["InTime"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["OutTime"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["GatepassType"].Width = Constants.InventoryConstants.GpType;
			grdhistory.RootTable.Columns["OrderType"].Width = Constants.InventoryConstants.GpType + 20;
			grdhistory.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdhistory.RootTable.Columns["DeliveryOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdhistory.RootTable.Columns["VarietyName"].Width = Constants.InventoryConstants.ItemName;
			grdhistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VesselNoConstant;
			grdhistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdhistory.RootTable.Columns["BiltyDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["OutTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["CityName"].Width = Constants.InventoryConstants.CityName;
			grdhistory.RootTable.Columns["Freight"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["NetPaid"].Width = Constants.InventoryConstants.AmountConstant;
			grdhistory.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["DifferenceWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["WeighBridgeStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["Container1"].Width = Constants.InventoryConstants.ContainerConstant;
			grdhistory.RootTable.Columns["Container2"].Width = Constants.InventoryConstants.ContainerConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			grdhistory.RootTable.Columns["OtherRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["DifferenceWeight"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["DifferenceWeight"].TotalFormatString = "#,##0.###";
			grdhistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["NetPaid"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["NetPaid"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print290");
			grdhistory.RootTable.Columns["Print290"].Caption = "Print290";
			grdhistory.RootTable.Columns["Print290"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print290"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print290"].ButtonText = "Print290";
			grdhistory.RootTable.Columns["Print290"].Width = 60;
			grdhistory.RootTable.Columns["Print290"].Position = 1;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 2;
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				GpId = Id;
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Print290")
			{
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString())
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "290-InvRptOutwardGatePassSlip.rpt");
			}
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
			Print_290(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_290(int printId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected O, but got Unknown
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (printId == 0)
			{
				throw new Exception("PrintId not found...");
			}
			DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = printId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "290-InvRptOutwardGatePassSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint291_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.GatePassOutwardSlipandRegister(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutwardGatePass_KeyDown(object sender, KeyEventArgs e)
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
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnPrint.Visible && btnPrint.Enabled)
				{
					btnPrint_Click(null, null);
				}
				if (e.Alt && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbsupp.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						FromDateDoInfo.Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						FromDateSoInfo.Focus();
						tabControl2.SelectedIndex = 2;
					}
					else if (tabControl2.SelectedIndex == 2)
					{
						((Control)(object)grd).Focus();
						tabControl2.SelectedIndex = 0;
					}
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtFromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer");
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

	private void btnShortCut_Click(object sender, EventArgs e)
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

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int gId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					((UltraGridBase)CmbStatus).Rows[1].Activate();
					Id = gId;
					GpId = gId;
					ReadById(Id);
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.GatePassOutwardSlipandRegister(gId);
				}
				if (grdhistory.CurrentColumn.Key == "Print290")
				{
					if (gId <= 0)
					{
						throw new Exception("Record Not Found For Display");
					}
					DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = gId
					});
					if (dt.Rows.Count == 0 || dt == null)
					{
						throw new Exception("Record Not Found For Display");
					}
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.ShowReportWithDataTable(dt, "290-InvRptOutwardGatePassSlip.rpt");
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(gId, 91);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				GpId = Id;
				ReadById(Id);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
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
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
					((UltraGridBase)CmbStatus).Rows[1].Activate();
					GpId = Id;
					ReadById(Id);
				}
				if (grd.CurrentColumn.Key == "Print")
				{
					CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				GpId = Id;
				ReadById(Id);
				((UltraGridBase)CmbStatus).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDoInformation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDoInformation.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDoInformation.CurrentColumn != null && e.Control && e.KeyData == Keys.Space && grdDoInformation.CurrentColumn.Key == "DoNo")
			{
				CommonServices.InvDeliveryOrderSlip(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTicketNoandWeighBridgeWeight()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 91,
				ReferenceDocNoId = GpId
			});
			if (lstWb.Rows.Count > 0)
			{
				string TicketNo = "";
				double NetWeight = 0.0;
				for (int i = 0; i < lstWb.Rows.Count; i++)
				{
					TicketNo = TicketNo + "," + lstWb.Rows[i]["TicketNo"].ToString();
					NetWeight += Convert.ToDouble(lstWb.Rows[i]["NetWbWeight"]);
				}
				txtWeighBridgeSlipNo.Text = TicketNo;
				txtFactoryWeight.Text = NetWeight.ToString();
				if (txtFactoryWeight.Text != string.Empty && txtFactoryWeight.Text != "0")
				{
					((Control)(object)CmbWeighBridgeStatus).Enabled = false;
				}
				DifferenceWeight();
			}
			else
			{
				txtWeighBridgeSlipNo.Text = "0";
				txtFactoryWeight.Text = "0";
			}
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = false;
				CmbStatus.Value = 2;
			}
			else
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightCalculation()
	{
		try
		{
			if (Conversion.ToInt(CmbGPBasedOn.Value) == 81 || Conversion.ToInt(CmbGPBasedOn.Value) == 90 || Conversion.ToInt(CmbGPBasedOn.Value) == 1605)
			{
				double Qty = Conversion.ToDouble((object)txtqty.Text.Trim());
				double PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
				if (Qty > 0.0 && PackUnit > 0.0)
				{
					txtCompareWeight.Text = (Qty * PackUnit).ToString();
				}
				else
				{
					txtCompareWeight.Text = "0";
				}
			}
			else
			{
				txtPackUnit.Text = "0";
				txtCompareWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void txtContrainerWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtfreight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtNetPaid_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CmbOrderno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtgpno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl2.SelectedIndex == 0)
			{
				((Control)(object)grd).Focus();
			}
			else if (tabControl2.SelectedIndex == 1)
			{
				FromDateDoInfo.Focus();
			}
			else if (tabControl2.SelectedIndex == 2)
			{
				FromDateSoInfo.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocDoInfo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocDoInfo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocSoInfo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocSoInfo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnDriverForm_Click(object sender, EventArgs e)
	{
		try
		{
			frmDriverBio obj = new frmDriverBio(UserAccount);
			int refDocTypeId = 0;
			if (UserAccount.RoleName == "Admin")
			{
				refDocTypeId = ((CommonServices.GetScreenIdByName("frmDriverBioForOutWard") > 0) ? 91 : 0);
			}
			else
			{
				bool num = clsGlobalVariables.ScreenViewReights.Any((tblUserRights r) => r.ScreenName == "frmDriverBioForOutWard");
				bool hasDriverBioRight = clsGlobalVariables.ScreenViewReights.Any((tblUserRights r) => r.ScreenName == "frmDriverBio");
				if (num)
				{
					refDocTypeId = 91;
				}
				else
				{
					if (!hasDriverBioRight)
					{
						MessageBox.Show("You don't have view rights for this form.");
						return;
					}
					refDocTypeId = 0;
				}
			}
			obj.RefDocumentTypeId = refDocTypeId;
			obj.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void labelWeightBridgeForm_Click(object sender, EventArgs e)
	{
		FormHelper.CanOpenForm("frmWeightbridge", delegate
		{
			frmWeightbridge frmWeightbridge = new frmWeightbridge(UserAccount);
			frmWeightbridge.Show();
			frmWeightbridge.BringToFront();
		}, UserAccount);
	}

	private void labelGdnForm_Click(object sender, EventArgs e)
	{
		if (Conversion.ToString((object)((Control)(object)cmbgptype).Text) == "Export")
		{
			FormHelper.CanOpenForm("EximForwarding", delegate
			{
				EximForwarding eximForwarding = new EximForwarding(UserAccount);
				eximForwarding.Show();
				eximForwarding.BringToFront();
			}, UserAccount);
		}
		else
		{
			FormHelper.CanOpenForm("InvFrmGDN", delegate
			{
				InvFrmGDN invFrmGDN = new InvFrmGDN(UserAccount);
				invFrmGDN.Show();
				invFrmGDN.BringToFront();
			}, UserAccount);
		}
	}

	private void OpenLinkedFormOnInsert(int GatePassId, string Status)
	{
		try
		{
			if (RadWBFormToOpenOnInsert.Checked)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					WBForm(GatePassId);
				}
			}
			else if (RadGrnFormToOpenOnInsert.Checked && Status == "Accepted")
			{
				GdnForm(GatePassId);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
				frmWeightbridge.cmbReferenceType.Value = 91;
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

	private void GdnForm(int GPId)
	{
		try
		{
			if (Conversion.ToString((object)((Control)(object)cmbgptype).Text) == "Export")
			{
				FormHelper.CanOpenForm("EximForwarding", delegate
				{
					EximForwarding eximForwarding = new EximForwarding(UserAccount);
					eximForwarding.Show();
					eximForwarding.cmbGpNo.Value = GPId;
					eximForwarding.cmbGpNo_Leave(null, null);
					eximForwarding.BringToFront();
					bool formClosed2 = false;
					eximForwarding.FormClosed += delegate
					{
						formClosed2 = true;
					};
					while (!formClosed2)
					{
						Application.DoEvents();
					}
				}, UserAccount);
				return;
			}
			FormHelper.CanOpenForm("InvFrmGDN", delegate
			{
				InvFrmGDN invFrmGDN = new InvFrmGDN(UserAccount);
				invFrmGDN.Show();
				invFrmGDN.LoadGpRow(GPId);
				invFrmGDN.BringToFront();
				bool formClosed = false;
				invFrmGDN.FormClosed += delegate
				{
					formClosed = true;
				};
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
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_049a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Expected O, but got Unknown
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b9: Expected O, but got Unknown
		//IL_08c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Expected O, but got Unknown
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Expected O, but got Unknown
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Expected O, but got Unknown
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Expected O, but got Unknown
		//IL_0907: Unknown result type (might be due to invalid IL or missing references)
		//IL_0911: Expected O, but got Unknown
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_091c: Expected O, but got Unknown
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Expected O, but got Unknown
		//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7c: Expected O, but got Unknown
		//IL_0b0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b16: Expected O, but got Unknown
		//IL_12b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bc: Expected O, but got Unknown
		//IL_1669: Unknown result type (might be due to invalid IL or missing references)
		//IL_1673: Expected O, but got Unknown
		//IL_9595: Unknown result type (might be due to invalid IL or missing references)
		//IL_959f: Expected O, but got Unknown
		//IL_95ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_95b6: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDoInformation_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout GrdSaleOrderInformation_DesignTimeLayout = new GridEXLayout();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		GridEXLayout grdPaymentTermOrder_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
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
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.OutwardGatePass));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdDoInformation = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtToDocDoInfo = new System.Windows.Forms.TextBox();
		this.txtFromDocDoInfo = new System.Windows.Forms.TextBox();
		this.CmbCustomerDoInfo = new UltraCombo();
		this.ToDateDoInfo = new System.Windows.Forms.DateTimePicker();
		this.FromDateDoInfo = new System.Windows.Forms.DateTimePicker();
		this.BtnResetDoInfo = new System.Windows.Forms.Button();
		this.BtnShowDoInfo = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.SoInformation = new System.Windows.Forms.TabPage();
		this.GrdSaleOrderInformation = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label41 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtToDocSoInfo = new System.Windows.Forms.TextBox();
		this.txtFromDocSoInfo = new System.Windows.Forms.TextBox();
		this.cmbCustomerSoInfo = new UltraCombo();
		this.ToDateSoInfo = new System.Windows.Forms.DateTimePicker();
		this.FromDateSoInfo = new System.Windows.Forms.DateTimePicker();
		this.BtnResetSoInfo = new System.Windows.Forms.Button();
		this.BtnShowSoInfo = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panelPaymentTermOrder = new System.Windows.Forms.Panel();
		this.grdPaymentTermOrder = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtDiffWeightRemarks = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtgptypeno = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.txtCompareWeight = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.labelSealNo1 = new System.Windows.Forms.Label();
		this.ContainerNo1 = new System.Windows.Forms.Label();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.labelSealNo2 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.containerno2 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbWeighBridgeStatus = new UltraCombo();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbOrderno = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtfreight = new System.Windows.Forms.TextBox();
		this.txtDifferenceWeight = new System.Windows.Forms.TextBox();
		this.txtWeighBridgeSlipNo = new System.Windows.Forms.TextBox();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.lblApprovedBy = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtContrainerWeight = new System.Windows.Forms.TextBox();
		this.txtNetPaid = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtSealNo1 = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtContainerNo1 = new System.Windows.Forms.TextBox();
		this.txtSealNo2 = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtContainerNo2 = new System.Windows.Forms.TextBox();
		this.CmbGPBasedOn = new UltraCombo();
		this.txtintime = new System.Windows.Forms.DateTimePicker();
		this.CmbStatus = new UltraCombo();
		this.txtouttime = new System.Windows.Forms.DateTimePicker();
		this.cmbcity = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbgptype = new UltraCombo();
		this.CmbWarehouse = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.CmbVariety = new UltraCombo();
		this.cmbsupp = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.chkPrintPreview290 = new System.Windows.Forms.CheckBox();
		this.labelGdnForm = new System.Windows.Forms.LinkLabel();
		this.labelWeightBridgeForm = new System.Windows.Forms.LinkLabel();
		this.RadNoneFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadGrnFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.RadWBFormToOpenOnInsert = new System.Windows.Forms.RadioButton();
		this.label15 = new System.Windows.Forms.Label();
		this.chkDriverInfoForm = new System.Windows.Forms.CheckBox();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint295 = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnDriverForm = new System.Windows.Forms.ToolStripButton();
		this.btndefvehicle = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txtBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoInformation).BeginInit();
		this.panel8.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDoInfo).BeginInit();
		this.SoInformation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrderInformation).BeginInit();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSoInfo).BeginInit();
		this.panel4.SuspendLayout();
		this.panelPaymentTermOrder.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermOrder).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGPBasedOn).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(1184, 661);
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
		this.tabPage1.Size = new System.Drawing.Size(1176, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.tabControl2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 339);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1172, 291);
		this.panel9.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Controls.Add(this.SoInformation);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1172, 291);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabControl2.KeyDown += new System.Windows.Forms.KeyEventHandler(grdDoInformation_KeyDown);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1164, 262);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Open Gatepass";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1158, 230);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 3;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label9);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1158, 26);
		this.panel6.TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(583, 16);
		this.label9.TabIndex = 60;
		this.label9.Text = "This Grid Show the  Vehicles Present In The Factory,Pending For Acceptance Or Rejection";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1120, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 59;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoInformation);
		this.tabPage4.Controls.Add(this.panel8);
		this.tabPage4.Controls.Add(this.panel10);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1164, 262);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Delivery Order Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdDoInformation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDoInformation).BackColor = System.Drawing.Color.White;
		grdDoInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoInformation.DesignTimeLayout = grdDoInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDoInformation.DynamicFiltering = true;
		this.grdDoInformation.FilterMode = (FilterMode)2;
		this.grdDoInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Location = new System.Drawing.Point(3, 81);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Name = "grdDoInformation";
		((ScrollableBase)this.grdDoInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Size = new System.Drawing.Size(1158, 178);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).TabIndex = 0;
		this.grdDoInformation.TotalRow = (InheritableBoolean)1;
		this.grdDoInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoInformation).VisualStyle = (VisualStyle)4;
		this.grdDoInformation.LinkClicked += new ColumnActionEventHandler(grdDoInformation_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label30);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 55);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1158, 26);
		this.panel8.TabIndex = 3;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 6);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(440, 16);
		this.label30.TabIndex = 61;
		this.label30.Text = "This Grid Show The Un-Completed And Approved Delivery Orders....";
		this.panel10.BackColor = System.Drawing.Color.White;
		this.panel10.Controls.Add(this.groupBox4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1158, 52);
		this.panel10.TabIndex = 2;
		this.groupBox4.Controls.Add(this.txtToDocDoInfo);
		this.groupBox4.Controls.Add(this.txtFromDocDoInfo);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo);
		this.groupBox4.Controls.Add(this.ToDateDoInfo);
		this.groupBox4.Controls.Add(this.FromDateDoInfo);
		this.groupBox4.Controls.Add(this.BtnResetDoInfo);
		this.groupBox4.Controls.Add(this.BtnShowDoInfo);
		this.groupBox4.Controls.Add(this.label7);
		this.groupBox4.Controls.Add(this.label33);
		this.groupBox4.Controls.Add(this.label34);
		this.groupBox4.Controls.Add(this.label37);
		this.groupBox4.Controls.Add(this.label40);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 4);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(960, 46);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.txtToDocDoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocDoInfo.Location = new System.Drawing.Point(539, 15);
		this.txtToDocDoInfo.Name = "txtToDocDoInfo";
		this.txtToDocDoInfo.ShortcutsEnabled = false;
		this.txtToDocDoInfo.Size = new System.Drawing.Size(56, 23);
		this.txtToDocDoInfo.TabIndex = 3;
		this.txtToDocDoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocDoInfo_KeyPress);
		this.txtFromDocDoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocDoInfo.Location = new System.Drawing.Point(439, 15);
		this.txtFromDocDoInfo.Name = "txtFromDocDoInfo";
		this.txtFromDocDoInfo.ShortcutsEnabled = false;
		this.txtFromDocDoInfo.Size = new System.Drawing.Size(56, 23);
		this.txtFromDocDoInfo.TabIndex = 2;
		this.txtFromDocDoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocDoInfo_KeyPress);
		this.CmbCustomerDoInfo.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerDoInfo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerDoInfo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerDoInfo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerDoInfo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerDoInfo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo).Location = new System.Drawing.Point(654, 13);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo).Name = "CmbCustomerDoInfo";
		((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDoInfo).TabIndex = 4;
		this.ToDateDoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateDoInfo.CustomFormat = "dd-MMM-yy";
		this.ToDateDoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateDoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateDoInfo.Location = new System.Drawing.Point(250, 15);
		this.ToDateDoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateDoInfo.Name = "ToDateDoInfo";
		this.ToDateDoInfo.ShowCheckBox = true;
		this.ToDateDoInfo.Size = new System.Drawing.Size(131, 23);
		this.ToDateDoInfo.TabIndex = 1;
		this.FromDateDoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateDoInfo.CustomFormat = "dd-MMM-yy";
		this.FromDateDoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateDoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateDoInfo.Location = new System.Drawing.Point(64, 15);
		this.FromDateDoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateDoInfo.Name = "FromDateDoInfo";
		this.FromDateDoInfo.ShowCheckBox = true;
		this.FromDateDoInfo.Size = new System.Drawing.Size(137, 23);
		this.FromDateDoInfo.TabIndex = 0;
		this.BtnResetDoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnResetDoInfo.FlatAppearance.BorderSize = 0;
		this.BtnResetDoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetDoInfo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnResetDoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnResetDoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnResetDoInfo.Location = new System.Drawing.Point(896, 13);
		this.BtnResetDoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetDoInfo.Name = "BtnResetDoInfo";
		this.BtnResetDoInfo.Size = new System.Drawing.Size(58, 26);
		this.BtnResetDoInfo.TabIndex = 6;
		this.BtnResetDoInfo.Text = "Reset";
		this.BtnResetDoInfo.UseVisualStyleBackColor = false;
		this.BtnResetDoInfo.Click += new System.EventHandler(BtnResetDoInfo_Click);
		this.BtnShowDoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowDoInfo.FlatAppearance.BorderSize = 0;
		this.BtnShowDoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowDoInfo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowDoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnShowDoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowDoInfo.Location = new System.Drawing.Point(835, 13);
		this.BtnShowDoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowDoInfo.Name = "BtnShowDoInfo";
		this.BtnShowDoInfo.Size = new System.Drawing.Size(58, 26);
		this.BtnShowDoInfo.TabIndex = 5;
		this.BtnShowDoInfo.Text = "Show";
		this.BtnShowDoInfo.UseVisualStyleBackColor = false;
		this.BtnShowDoInfo.Click += new System.EventHandler(BtnShowDoInfo_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(596, 19);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 142;
		this.label7.Text = "Customer";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(497, 19);
		this.label33.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(45, 15);
		this.label33.TabIndex = 141;
		this.label33.Text = "To Doc";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(382, 19);
		this.label34.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(60, 15);
		this.label34.TabIndex = 140;
		this.label34.Text = "From Doc";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(204, 19);
		this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(48, 15);
		this.label37.TabIndex = 139;
		this.label37.Text = "To Date";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(2, 19);
		this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 13;
		this.label40.Text = "From Date";
		this.SoInformation.AccessibleName = "";
		this.SoInformation.Controls.Add((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation);
		this.SoInformation.Controls.Add(this.panel12);
		this.SoInformation.Controls.Add(this.panel11);
		this.SoInformation.Location = new System.Drawing.Point(4, 25);
		this.SoInformation.Name = "SoInformation";
		this.SoInformation.Padding = new System.Windows.Forms.Padding(3);
		this.SoInformation.Size = new System.Drawing.Size(1164, 262);
		this.SoInformation.TabIndex = 2;
		this.SoInformation.Text = "Sale Order Detail";
		this.SoInformation.UseVisualStyleBackColor = true;
		this.GrdSaleOrderInformation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdSaleOrderInformation).BackColor = System.Drawing.Color.White;
		GrdSaleOrderInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdSaleOrderInformation.DesignTimeLayout = GrdSaleOrderInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdSaleOrderInformation.DynamicFiltering = true;
		this.GrdSaleOrderInformation.FilterMode = (FilterMode)2;
		this.GrdSaleOrderInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Location = new System.Drawing.Point(3, 78);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Name = "GrdSaleOrderInformation";
		((ScrollableBase)this.GrdSaleOrderInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Size = new System.Drawing.Size(1158, 181);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).TabIndex = 0;
		this.GrdSaleOrderInformation.TotalRow = (InheritableBoolean)1;
		this.GrdSaleOrderInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdSaleOrderInformation).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label41);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 52);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1158, 26);
		this.panel12.TabIndex = 5;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(3, 6);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(838, 16);
		this.label41.TabIndex = 61;
		this.label41.Text = "This Grid Show The Un-Completed And Approved Sale Orders.... Complete.Cancel And Un-Approved Orders Will Not Appear here. ";
		this.panel11.BackColor = System.Drawing.Color.White;
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1158, 49);
		this.panel11.TabIndex = 3;
		this.groupBox3.Controls.Add(this.txtToDocSoInfo);
		this.groupBox3.Controls.Add(this.txtFromDocSoInfo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo);
		this.groupBox3.Controls.Add(this.ToDateSoInfo);
		this.groupBox3.Controls.Add(this.FromDateSoInfo);
		this.groupBox3.Controls.Add(this.BtnResetSoInfo);
		this.groupBox3.Controls.Add(this.BtnShowSoInfo);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(960, 46);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.txtToDocSoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocSoInfo.Location = new System.Drawing.Point(539, 15);
		this.txtToDocSoInfo.Name = "txtToDocSoInfo";
		this.txtToDocSoInfo.ShortcutsEnabled = false;
		this.txtToDocSoInfo.Size = new System.Drawing.Size(56, 23);
		this.txtToDocSoInfo.TabIndex = 3;
		this.txtToDocSoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocSoInfo_KeyPress);
		this.txtFromDocSoInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocSoInfo.Location = new System.Drawing.Point(439, 15);
		this.txtFromDocSoInfo.Name = "txtFromDocSoInfo";
		this.txtFromDocSoInfo.ShortcutsEnabled = false;
		this.txtFromDocSoInfo.Size = new System.Drawing.Size(56, 23);
		this.txtFromDocSoInfo.TabIndex = 2;
		this.txtFromDocSoInfo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocSoInfo_KeyPress);
		this.cmbCustomerSoInfo.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomerSoInfo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomerSoInfo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomerSoInfo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomerSoInfo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomerSoInfo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo).Location = new System.Drawing.Point(654, 13);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo).Name = "cmbCustomerSoInfo";
		((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomerSoInfo).TabIndex = 4;
		this.ToDateSoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateSoInfo.CustomFormat = "dd-MMM-yy";
		this.ToDateSoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateSoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateSoInfo.Location = new System.Drawing.Point(250, 15);
		this.ToDateSoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateSoInfo.Name = "ToDateSoInfo";
		this.ToDateSoInfo.ShowCheckBox = true;
		this.ToDateSoInfo.Size = new System.Drawing.Size(131, 23);
		this.ToDateSoInfo.TabIndex = 1;
		this.FromDateSoInfo.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateSoInfo.CustomFormat = "dd-MMM-yy";
		this.FromDateSoInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateSoInfo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateSoInfo.Location = new System.Drawing.Point(64, 15);
		this.FromDateSoInfo.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateSoInfo.Name = "FromDateSoInfo";
		this.FromDateSoInfo.ShowCheckBox = true;
		this.FromDateSoInfo.Size = new System.Drawing.Size(137, 23);
		this.FromDateSoInfo.TabIndex = 0;
		this.BtnResetSoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnResetSoInfo.FlatAppearance.BorderSize = 0;
		this.BtnResetSoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnResetSoInfo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnResetSoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnResetSoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnResetSoInfo.Location = new System.Drawing.Point(896, 13);
		this.BtnResetSoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnResetSoInfo.Name = "BtnResetSoInfo";
		this.BtnResetSoInfo.Size = new System.Drawing.Size(58, 26);
		this.BtnResetSoInfo.TabIndex = 6;
		this.BtnResetSoInfo.Text = "Reset";
		this.BtnResetSoInfo.UseVisualStyleBackColor = false;
		this.BtnResetSoInfo.Click += new System.EventHandler(BtnResetSoInfo_Click);
		this.BtnShowSoInfo.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.BtnShowSoInfo.FlatAppearance.BorderSize = 0;
		this.BtnShowSoInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowSoInfo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowSoInfo.ForeColor = System.Drawing.Color.White;
		this.BtnShowSoInfo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowSoInfo.Location = new System.Drawing.Point(835, 13);
		this.BtnShowSoInfo.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowSoInfo.Name = "BtnShowSoInfo";
		this.BtnShowSoInfo.Size = new System.Drawing.Size(58, 26);
		this.BtnShowSoInfo.TabIndex = 5;
		this.BtnShowSoInfo.Text = "Show";
		this.BtnShowSoInfo.UseVisualStyleBackColor = false;
		this.BtnShowSoInfo.Click += new System.EventHandler(BtnShowSoInfo_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(596, 19);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Customer";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(497, 19);
		this.label42.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(45, 15);
		this.label42.TabIndex = 141;
		this.label42.Text = "To Doc";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(382, 19);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(60, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(204, 19);
		this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(48, 15);
		this.label44.TabIndex = 139;
		this.label44.Text = "To Date";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(2, 19);
		this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(63, 15);
		this.label45.TabIndex = 13;
		this.label45.Text = "From Date";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panelPaymentTermOrder);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1172, 282);
		this.panel4.TabIndex = 0;
		this.panelPaymentTermOrder.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder);
		this.panelPaymentTermOrder.Location = new System.Drawing.Point(971, 3);
		this.panelPaymentTermOrder.Name = "panelPaymentTermOrder";
		this.panelPaymentTermOrder.Size = new System.Drawing.Size(192, 274);
		this.panelPaymentTermOrder.TabIndex = 50;
		this.grdPaymentTermOrder.AllowEdit = (InheritableBoolean)2;
		this.grdPaymentTermOrder.AutomaticSort = false;
		((ScrollableBase)this.grdPaymentTermOrder).BackColor = System.Drawing.Color.White;
		this.grdPaymentTermOrder.ColumnAutoResize = true;
		grdPaymentTermOrder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTermOrder.DesignTimeLayout = grdPaymentTermOrder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTermOrder.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).Name = "grdPaymentTermOrder";
		((ScrollableBase)this.grdPaymentTermOrder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).Size = new System.Drawing.Size(192, 274);
		((System.Windows.Forms.Control)(object)this.grdPaymentTermOrder).TabIndex = 1;
		this.grdPaymentTermOrder.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTermOrder.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTermOrder).VisualStyle = (VisualStyle)4;
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtBiltyDate);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtDiffWeightRemarks);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtgpno);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtgptypeno);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtCompareWeight);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label39);
		this.groupBox1.Controls.Add(this.labelSealNo1);
		this.groupBox1.Controls.Add(this.ContainerNo1);
		this.groupBox1.Controls.Add(this.txtPackUnit);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.labelSealNo2);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.containerno2);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus);
		this.groupBox1.Controls.Add(this.txtvehicleno);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.CmbOrderno);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtfreight);
		this.groupBox1.Controls.Add(this.txtDifferenceWeight);
		this.groupBox1.Controls.Add(this.txtWeighBridgeSlipNo);
		this.groupBox1.Controls.Add(this.txtApprovedBy);
		this.groupBox1.Controls.Add(this.txtbiltyno);
		this.groupBox1.Controls.Add(this.lblApprovedBy);
		this.groupBox1.Controls.Add(this.txtqty);
		this.groupBox1.Controls.Add(this.txtContrainerWeight);
		this.groupBox1.Controls.Add(this.txtNetPaid);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtFactoryWeight);
		this.groupBox1.Controls.Add(this.txtSealNo1);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtContainerNo1);
		this.groupBox1.Controls.Add(this.txtSealNo2);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtContainerNo2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGPBasedOn);
		this.groupBox1.Controls.Add(this.txtintime);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add(this.txtouttime);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgptype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariety);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupp);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(966, 274);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(552, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(44, 15);
		this.label17.TabIndex = 106;
		this.label17.Text = "Freight";
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(111, 19);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(146, 23);
		this.txtgpdate.TabIndex = 0;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(111, 186);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(632, 53);
		this.txtremarks.TabIndex = 9;
		this.txtDiffWeightRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiffWeightRemarks.Location = new System.Drawing.Point(111, 240);
		this.txtDiffWeightRemarks.Name = "txtDiffWeightRemarks";
		this.txtDiffWeightRemarks.Size = new System.Drawing.Size(632, 23);
		this.txtDiffWeightRemarks.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 108);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 47;
		this.label2.Text = "Customer Name";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(356, 94);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 48;
		this.label8.Text = "Vehicle Type";
		this.label36.AutoSize = true;
		this.label36.BackColor = System.Drawing.Color.White;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(4, 244);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(94, 15);
		this.label36.TabIndex = 87;
		this.label36.Text = "Wt Diff Remarks";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(356, 119);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Vehicle No";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(552, 165);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(62, 15);
		this.label20.TabIndex = 50;
		this.label20.Text = "City Name";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(4, 192);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(93, 15);
		this.label14.TabIndex = 53;
		this.label14.Text = "Remarks Header";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(552, 50);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 51;
		this.label11.Text = "Net Paid";
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Enabled = false;
		this.txtgpno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpno.Location = new System.Drawing.Point(260, 19);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(90, 23);
		this.txtgpno.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(552, 137);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(61, 15);
		this.label23.TabIndex = 50;
		this.label23.Text = "G.P Status";
		this.txtgptypeno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgptypeno.Enabled = false;
		this.txtgptypeno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgptypeno.Location = new System.Drawing.Point(260, 46);
		this.txtgptypeno.Name = "txtgptypeno";
		this.txtgptypeno.ReadOnly = true;
		this.txtgptypeno.Size = new System.Drawing.Size(90, 23);
		this.txtgptypeno.TabIndex = 3;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.White;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(746, 23);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(65, 15);
		this.label28.TabIndex = 51;
		this.label28.Text = "Do Weight";
		this.txtCompareWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCompareWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCompareWeight.Location = new System.Drawing.Point(429, 63);
		this.txtCompareWeight.Name = "txtCompareWeight";
		this.txtCompareWeight.ReadOnly = true;
		this.txtCompareWeight.Size = new System.Drawing.Size(117, 23);
		this.txtCompareWeight.TabIndex = 13;
		this.txtCompareWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(4, 165);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(69, 15);
		this.label24.TabIndex = 50;
		this.label24.Text = "WareHouse";
		this.label39.AutoSize = true;
		this.label39.BackColor = System.Drawing.Color.White;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(356, 67);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 98;
		this.label39.Text = "Weight";
		this.labelSealNo1.AutoSize = true;
		this.labelSealNo1.BackColor = System.Drawing.Color.White;
		this.labelSealNo1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSealNo1.Location = new System.Drawing.Point(746, 224);
		this.labelSealNo1.Name = "labelSealNo1";
		this.labelSealNo1.Size = new System.Drawing.Size(56, 15);
		this.labelSealNo1.TabIndex = 51;
		this.labelSealNo1.Text = "Seal No 1";
		this.labelSealNo1.Visible = false;
		this.ContainerNo1.AutoSize = true;
		this.ContainerNo1.BackColor = System.Drawing.Color.White;
		this.ContainerNo1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ContainerNo1.Location = new System.Drawing.Point(746, 176);
		this.ContainerNo1.Name = "ContainerNo1";
		this.ContainerNo1.Size = new System.Drawing.Size(85, 15);
		this.ContainerNo1.TabIndex = 51;
		this.ContainerNo1.Text = "Container No 1";
		this.ContainerNo1.Visible = false;
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackUnit.Location = new System.Drawing.Point(429, 38);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(117, 23);
		this.txtPackUnit.TabIndex = 12;
		this.txtPackUnit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackUnit.TextChanged += new System.EventHandler(textBox1_TextChanged);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 60;
		this.label1.Text = "Gp Date && No";
		this.labelSealNo2.AutoSize = true;
		this.labelSealNo2.BackColor = System.Drawing.Color.White;
		this.labelSealNo2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelSealNo2.Location = new System.Drawing.Point(746, 248);
		this.labelSealNo2.Name = "labelSealNo2";
		this.labelSealNo2.Size = new System.Drawing.Size(58, 15);
		this.labelSealNo2.TabIndex = 51;
		this.labelSealNo2.Text = "Seal No 2";
		this.labelSealNo2.Visible = false;
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(356, 42);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(61, 15);
		this.label38.TabIndex = 96;
		this.label38.Text = "Pack Uom";
		this.containerno2.AutoSize = true;
		this.containerno2.BackColor = System.Drawing.Color.White;
		this.containerno2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.containerno2.Location = new System.Drawing.Point(746, 200);
		this.containerno2.Name = "containerno2";
		this.containerno2.Size = new System.Drawing.Size(87, 15);
		this.containerno2.TabIndex = 51;
		this.containerno2.Text = "Container No 2";
		this.containerno2.Visible = false;
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(4, 136);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(67, 15);
		this.label25.TabIndex = 50;
		this.label25.Text = "Item Name";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(552, 79);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(48, 15);
		this.label12.TabIndex = 12;
		this.label12.Text = "In Time";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(96, 47);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(15, 20);
		this.label6.TabIndex = 2;
		this.label6.Text = "*";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(356, 143);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(49, 15);
		this.label10.TabIndex = 56;
		this.label10.Text = "Bilty No";
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(96, 20);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(15, 20);
		this.label32.TabIndex = 90;
		this.label32.Text = "*";
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(356, 17);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(54, 15);
		this.label22.TabIndex = 10;
		this.label22.Text = "Item Qty";
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(96, 105);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(15, 20);
		this.label31.TabIndex = 6;
		this.label31.Text = "*";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(4, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 57;
		this.label4.Text = "GP Type  && No";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(552, 108);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 15);
		this.label13.TabIndex = 17;
		this.label13.Text = "Out Time";
		this.CmbWeighBridgeStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridgeStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridgeStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridgeStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWeighBridgeStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Location = new System.Drawing.Point(835, 120);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Name = "CmbWeighBridgeStatus";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).TabIndex = 28;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(429, 115);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(117, 23);
		this.txtvehicleno.TabIndex = 15;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(746, 126);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(64, 15);
		this.label35.TabIndex = 85;
		this.label35.Text = "W.B Status";
		this.CmbOrderno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbOrderno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderno.Location = new System.Drawing.Point(260, 75);
		this.CmbOrderno.Name = "CmbOrderno";
		this.CmbOrderno.Size = new System.Drawing.Size(90, 23);
		this.CmbOrderno.TabIndex = 5;
		this.CmbOrderno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CmbOrderno_KeyPress);
		this.CmbOrderno.Leave += new System.EventHandler(CmbOrderno_Leave);
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.White;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(746, 73);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 83;
		this.label29.Text = "Diff Weight";
		this.txtfreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfreight.Location = new System.Drawing.Point(614, 19);
		this.txtfreight.Name = "txtfreight";
		this.txtfreight.Size = new System.Drawing.Size(129, 23);
		this.txtfreight.TabIndex = 18;
		this.txtfreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfreight_KeyPress);
		this.txtDifferenceWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtDifferenceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDifferenceWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDifferenceWeight.Location = new System.Drawing.Point(835, 69);
		this.txtDifferenceWeight.Name = "txtDifferenceWeight";
		this.txtDifferenceWeight.ReadOnly = true;
		this.txtDifferenceWeight.Size = new System.Drawing.Size(125, 23);
		this.txtDifferenceWeight.TabIndex = 26;
		this.txtDifferenceWeight.TabStop = false;
		this.txtDifferenceWeight.Text = "0";
		this.txtDifferenceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeighBridgeSlipNo.BackColor = System.Drawing.SystemColors.Menu;
		this.txtWeighBridgeSlipNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighBridgeSlipNo.Enabled = false;
		this.txtWeighBridgeSlipNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeighBridgeSlipNo.Location = new System.Drawing.Point(835, 94);
		this.txtWeighBridgeSlipNo.Name = "txtWeighBridgeSlipNo";
		this.txtWeighBridgeSlipNo.Size = new System.Drawing.Size(125, 23);
		this.txtWeighBridgeSlipNo.TabIndex = 27;
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtApprovedBy.Location = new System.Drawing.Point(835, 148);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.ReadOnly = true;
		this.txtApprovedBy.Size = new System.Drawing.Size(125, 23);
		this.txtApprovedBy.TabIndex = 29;
		this.txtApprovedBy.TabStop = false;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbiltyno.Location = new System.Drawing.Point(429, 139);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(117, 23);
		this.txtbiltyno.TabIndex = 16;
		this.lblApprovedBy.AutoSize = true;
		this.lblApprovedBy.BackColor = System.Drawing.Color.White;
		this.lblApprovedBy.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedBy.Location = new System.Drawing.Point(746, 152);
		this.lblApprovedBy.Name = "lblApprovedBy";
		this.lblApprovedBy.Size = new System.Drawing.Size(75, 15);
		this.lblApprovedBy.TabIndex = 80;
		this.lblApprovedBy.Text = "Approved By";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(429, 13);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(117, 23);
		this.txtqty.TabIndex = 11;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtContrainerWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtContrainerWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContrainerWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContrainerWeight.Location = new System.Drawing.Point(835, 19);
		this.txtContrainerWeight.Name = "txtContrainerWeight";
		this.txtContrainerWeight.ReadOnly = true;
		this.txtContrainerWeight.Size = new System.Drawing.Size(125, 23);
		this.txtContrainerWeight.TabIndex = 24;
		this.txtContrainerWeight.TabStop = false;
		this.txtContrainerWeight.Text = "0";
		this.txtContrainerWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtContrainerWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtContrainerWeight_KeyPress);
		this.txtNetPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetPaid.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetPaid.Location = new System.Drawing.Point(614, 46);
		this.txtNetPaid.Name = "txtNetPaid";
		this.txtNetPaid.Size = new System.Drawing.Size(129, 23);
		this.txtNetPaid.TabIndex = 19;
		this.txtNetPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetPaid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetPaid_KeyPress);
		this.label27.AutoSize = true;
		this.label27.BackColor = System.Drawing.Color.White;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(746, 48);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(88, 15);
		this.label27.TabIndex = 78;
		this.label27.Text = "Factory Weight";
		this.txtFactoryWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFactoryWeight.Location = new System.Drawing.Point(835, 44);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(125, 23);
		this.txtFactoryWeight.TabIndex = 25;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.Text = "0";
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSealNo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSealNo1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSealNo1.Location = new System.Drawing.Point(835, 220);
		this.txtSealNo1.Name = "txtSealNo1";
		this.txtSealNo1.Size = new System.Drawing.Size(125, 23);
		this.txtSealNo1.TabIndex = 32;
		this.txtSealNo1.Visible = false;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(96, 76);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(15, 20);
		this.label19.TabIndex = 77;
		this.label19.Text = "*";
		this.txtContainerNo1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNo1.Location = new System.Drawing.Point(835, 172);
		this.txtContainerNo1.Name = "txtContainerNo1";
		this.txtContainerNo1.Size = new System.Drawing.Size(125, 23);
		this.txtContainerNo1.TabIndex = 30;
		this.txtContainerNo1.Visible = false;
		this.txtSealNo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSealNo2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSealNo2.Location = new System.Drawing.Point(835, 244);
		this.txtSealNo2.Name = "txtSealNo2";
		this.txtSealNo2.Size = new System.Drawing.Size(125, 23);
		this.txtSealNo2.TabIndex = 33;
		this.txtSealNo2.Visible = false;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(4, 79);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(96, 15);
		this.label18.TabIndex = 76;
		this.label18.Text = "Order Type && No";
		this.txtContainerNo2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerNo2.Location = new System.Drawing.Point(835, 196);
		this.txtContainerNo2.Name = "txtContainerNo2";
		this.txtContainerNo2.Size = new System.Drawing.Size(125, 23);
		this.txtContainerNo2.TabIndex = 31;
		this.txtContainerNo2.Visible = false;
		this.CmbGPBasedOn.AllowNull = (DefaultableBoolean)1;
		this.CmbGPBasedOn.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGPBasedOn.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGPBasedOn.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGPBasedOn).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGPBasedOn.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGPBasedOn).Location = new System.Drawing.Point(111, 73);
		((System.Windows.Forms.Control)(object)this.CmbGPBasedOn).Name = "CmbGPBasedOn";
		((System.Windows.Forms.Control)(object)this.CmbGPBasedOn).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.CmbGPBasedOn).TabIndex = 4;
		this.CmbGPBasedOn.ValueChanged += new System.EventHandler(CmbGPBasedOn_Leave);
		this.txtintime.CustomFormat = "HH:mm";
		this.txtintime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtintime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtintime.Location = new System.Drawing.Point(614, 75);
		this.txtintime.Name = "txtintime";
		this.txtintime.Size = new System.Drawing.Size(129, 23);
		this.txtintime.TabIndex = 20;
		this.txtintime.TabStop = false;
		this.txtintime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(614, 131);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(129, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 22;
		this.txtouttime.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtouttime.CustomFormat = "HH:mm";
		this.txtouttime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtouttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtouttime.Location = new System.Drawing.Point(614, 104);
		this.txtouttime.Name = "txtouttime";
		this.txtouttime.Size = new System.Drawing.Size(129, 23);
		this.txtouttime.TabIndex = 21;
		this.txtouttime.TabStop = false;
		this.txtouttime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(614, 159);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(129, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 23;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.White;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(746, 98);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(70, 15);
		this.label26.TabIndex = 64;
		this.label26.Text = "W.B Slip No";
		this.cmbgptype.AllowNull = (DefaultableBoolean)1;
		this.cmbgptype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgptype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgptype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbgptype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbgptype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Location = new System.Drawing.Point(111, 44);
		((System.Windows.Forms.Control)(object)this.cmbgptype).Name = "cmbgptype";
		((System.Windows.Forms.Control)(object)this.cmbgptype).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.cmbgptype).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbgptype).TextChanged += new System.EventHandler(cmbgptype_Leave_1);
		((System.Windows.Forms.Control)(object)this.cmbgptype).Leave += new System.EventHandler(cmbgptype_Leave_1);
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(111, 159);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 8;
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(429, 88);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 14;
		this.CmbVariety.AllowNull = (DefaultableBoolean)1;
		this.CmbVariety.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariety.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariety.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbVariety).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVariety.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Location = new System.Drawing.Point(111, 130);
		((System.Windows.Forms.Control)(object)this.CmbVariety).Name = "CmbVariety";
		((System.Windows.Forms.Control)(object)this.CmbVariety).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbVariety).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Leave += new System.EventHandler(CmbVariety_Leave);
		this.cmbsupp.AllowNull = (DefaultableBoolean)1;
		this.cmbsupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbsupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupp).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupp).Location = new System.Drawing.Point(111, 102);
		((System.Windows.Forms.Control)(object)this.cmbsupp).Name = "cmbsupp";
		((System.Windows.Forms.Control)(object)this.cmbsupp).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupp).TabIndex = 6;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.chkPrintPreview290);
		this.panel3.Controls.Add(this.labelGdnForm);
		this.panel3.Controls.Add(this.labelWeightBridgeForm);
		this.panel3.Controls.Add(this.RadNoneFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadGrnFormToOpenOnInsert);
		this.panel3.Controls.Add(this.RadWBFormToOpenOnInsert);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.chkDriverInfoForm);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1172, 26);
		this.panel3.TabIndex = 1;
		this.chkPrintPreview290.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintPreview290.AutoSize = true;
		this.chkPrintPreview290.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintPreview290.ForeColor = System.Drawing.Color.White;
		this.chkPrintPreview290.Location = new System.Drawing.Point(865, 3);
		this.chkPrintPreview290.Name = "chkPrintPreview290";
		this.chkPrintPreview290.Size = new System.Drawing.Size(153, 20);
		this.chkPrintPreview290.TabIndex = 39;
		this.chkPrintPreview290.Text = "Print Preview 290";
		this.chkPrintPreview290.UseVisualStyleBackColor = true;
		this.labelGdnForm.AutoSize = true;
		this.labelGdnForm.BackColor = System.Drawing.Color.Teal;
		this.labelGdnForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelGdnForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelGdnForm.Location = new System.Drawing.Point(488, 6);
		this.labelGdnForm.Name = "labelGdnForm";
		this.labelGdnForm.Size = new System.Drawing.Size(163, 14);
		this.labelGdnForm.TabIndex = 34;
		this.labelGdnForm.TabStop = true;
		this.labelGdnForm.Text = "Gdn / Forwarding Form";
		this.labelGdnForm.Click += new System.EventHandler(labelGdnForm_Click);
		this.labelWeightBridgeForm.AutoSize = true;
		this.labelWeightBridgeForm.BackColor = System.Drawing.Color.Teal;
		this.labelWeightBridgeForm.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.labelWeightBridgeForm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelWeightBridgeForm.Location = new System.Drawing.Point(325, 6);
		this.labelWeightBridgeForm.Name = "labelWeightBridgeForm";
		this.labelWeightBridgeForm.Size = new System.Drawing.Size(139, 14);
		this.labelWeightBridgeForm.TabIndex = 35;
		this.labelWeightBridgeForm.TabStop = true;
		this.labelWeightBridgeForm.Text = "Weight Bridge Form";
		this.labelWeightBridgeForm.Click += new System.EventHandler(labelWeightBridgeForm_Click);
		this.RadNoneFormToOpenOnInsert.AutoSize = true;
		this.RadNoneFormToOpenOnInsert.Checked = true;
		this.RadNoneFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNoneFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadNoneFormToOpenOnInsert.Location = new System.Drawing.Point(238, 4);
		this.RadNoneFormToOpenOnInsert.Name = "RadNoneFormToOpenOnInsert";
		this.RadNoneFormToOpenOnInsert.Size = new System.Drawing.Size(59, 18);
		this.RadNoneFormToOpenOnInsert.TabIndex = 36;
		this.RadNoneFormToOpenOnInsert.TabStop = true;
		this.RadNoneFormToOpenOnInsert.Text = "None";
		this.RadNoneFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGrnFormToOpenOnInsert.AutoSize = true;
		this.RadGrnFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadGrnFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadGrnFormToOpenOnInsert.Location = new System.Drawing.Point(468, 7);
		this.RadGrnFormToOpenOnInsert.Name = "RadGrnFormToOpenOnInsert";
		this.RadGrnFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadGrnFormToOpenOnInsert.TabIndex = 37;
		this.RadGrnFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.RadGrnFormToOpenOnInsert.Visible = false;
		this.RadWBFormToOpenOnInsert.AutoSize = true;
		this.RadWBFormToOpenOnInsert.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadWBFormToOpenOnInsert.ForeColor = System.Drawing.Color.White;
		this.RadWBFormToOpenOnInsert.Location = new System.Drawing.Point(307, 7);
		this.RadWBFormToOpenOnInsert.Name = "RadWBFormToOpenOnInsert";
		this.RadWBFormToOpenOnInsert.Size = new System.Drawing.Size(14, 13);
		this.RadWBFormToOpenOnInsert.TabIndex = 38;
		this.RadWBFormToOpenOnInsert.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(165, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Outward Gate Pass";
		this.chkDriverInfoForm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkDriverInfoForm.AutoSize = true;
		this.chkDriverInfoForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkDriverInfoForm.ForeColor = System.Drawing.Color.White;
		this.chkDriverInfoForm.Location = new System.Drawing.Point(680, 3);
		this.chkDriverInfoForm.Name = "chkDriverInfoForm";
		this.chkDriverInfoForm.Size = new System.Drawing.Size(181, 20);
		this.chkDriverInfoForm.TabIndex = 31;
		this.chkDriverInfoForm.Text = "Open Driver Bio Form";
		this.chkDriverInfoForm.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1020, 3);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(153, 20);
		this.ChkBox.TabIndex = 31;
		this.ChkBox.Text = "Print Preview 295";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1172, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator6, this.btnupdate, this.btnsave, this.btnDelete, this.toolStripSeparator5, this.btnPrint295, this.btnAttachment, this.toolStripSeparator4,
			this.btnPrint, this.toolStripSeparator3, this.BtnDriverForm, this.btndefvehicle, this.toolStripSeparator2, this.btnShortCut, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1172, 27);
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
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.ToolTipText = "Ctrl_R";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.ToolTipText = "Ctrl+Shift+Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrint295.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint295.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint295.Name = "btnPrint295";
		this.btnPrint295.Size = new System.Drawing.Size(92, 24);
		this.btnPrint295.Text = "295-Print";
		this.btnPrint295.ToolTipText = "Alt+2";
		this.btnPrint295.Click += new System.EventHandler(btnPrint291_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Ctrl+F12";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "290-Print";
		this.btnPrint.ToolTipText = "Ctrl_P / Alt+1";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnDriverForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDriverForm.Name = "BtnDriverForm";
		this.BtnDriverForm.Size = new System.Drawing.Size(73, 24);
		this.BtnDriverForm.Text = "Driver Bio";
		this.BtnDriverForm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnDriverForm.ToolTipText = "Ctrl+F1";
		this.BtnDriverForm.Click += new System.EventHandler(BtnDriverForm_Click);
		this.btndefvehicle.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndefvehicle.Name = "btndefvehicle";
		this.btndefvehicle.Size = new System.Drawing.Size(103, 24);
		this.btndefvehicle.Text = "Define Vehicle";
		this.btndefvehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btndefvehicle.ToolTipText = "Ctrl+F1";
		this.btndefvehicle.Click += new System.EventHandler(btndefvehicle_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnShortCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(126, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.ToolTipText = "Ctrl+Alt";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.groupBox2);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1176, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 153);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1172, 477);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label21);
		this.panel5.Controls.Add(this.ctrlGrdBar2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 130);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1172, 23);
		this.panel5.TabIndex = 77;
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label21.Location = new System.Drawing.Point(5, 3);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(112, 17);
		this.label21.TabIndex = 74;
		this.label21.Text = "Filtered Record";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1127, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar2.TabIndex = 73;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.txtToDateHistory);
		this.groupBox2.Controls.Add(this.txtFromDateHistory);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 57);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1172, 73);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(872, 46);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(706, 46);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(786, 46);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(638, 42);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(387, 20);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Customer";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(222, 48);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(222, 20);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(6, 48);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(6, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(448, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(303, 44);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(303, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(80, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(72, 44);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(146, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.txtFromDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateHistory.Location = new System.Drawing.Point(72, 16);
		this.txtFromDateHistory.Name = "txtFromDateHistory";
		this.txtFromDateHistory.ShowCheckBox = true;
		this.txtFromDateHistory.Size = new System.Drawing.Size(146, 23);
		this.txtFromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1172, 55);
		this.panel7.TabIndex = 74;
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label16.Location = new System.Drawing.Point(3, 31);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(228, 19);
		this.label16.TabIndex = 74;
		this.label16.Text = "Outward Gate Pass History";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1172, 27);
		this.toolStrip2.TabIndex = 59;
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
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.txtBiltyDate.CustomFormat = "dd-MMM-yy";
		this.txtBiltyDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtBiltyDate.Location = new System.Drawing.Point(429, 163);
		this.txtBiltyDate.Name = "txtBiltyDate";
		this.txtBiltyDate.Size = new System.Drawing.Size(117, 22);
		this.txtBiltyDate.TabIndex = 17;
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.White;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(356, 167);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(58, 15);
		this.label46.TabIndex = 60;
		this.label46.Text = "Bilty Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "OutwardGatePass";
		this.Text = "Outward GatePass";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OutwardGatePass_FormClosing);
		base.Load += new System.EventHandler(OutwardGatePass_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(OutwardGatePass_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoInformation).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDoInfo).EndInit();
		this.SoInformation.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrderInformation).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomerSoInfo).EndInit();
		this.panel4.ResumeLayout(false);
		this.panelPaymentTermOrder.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTermOrder).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGPBasedOn).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
